using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB.Vote;
using Microsoft.VisualBasic.FileIO;
using static System.String;

namespace Vote.Master
{
  public partial class LogPaypalDonations : SecurePage, ISuperUser
  {
    private static int GetColumnIndex(string[] columnNames, string[] columnName,
      ref int maxColumnInx)
    {
      var inx = Array.FindIndex(columnNames, columnName.Contains);
      if (inx < 0)
        throw new VoteException(
          $"Column names [{Join(", ", columnName)}] not found in csv");
      if (inx > maxColumnInx) maxColumnInx = inx;
      return inx;
    }

    private static int GetColumnIndex(string[] columnNames, string columnName,
      ref int maxColumnInx)
    {
      return GetColumnIndex(columnNames, new[] { columnName }, ref maxColumnInx);
    }

    //private static void NonFatalError(ICollection<string> messages, long lineNumber,
    //  string message)
    //{
    //  messages.Add(
    //    $"<p class=\"error\">Non-fatal error on row {lineNumber}: {message}</p>");
    //}

    private void PostCsv()
    {
      var now = DateTime.UtcNow;
      var messages = new List<string>();
      var good = 0;
      var bad = 0;

      try
      {
        if (Request.Files.Count != 1 || Request.Files[0].ContentLength == 0)
          throw new VoteException("Could not find CSV file");
        using (var parser =
          new TextFieldParser(Request.Files[0].InputStream, Encoding.UTF8))
        {
          parser.TextFieldType = FieldType.Delimited;
          parser.HasFieldsEnclosedInQuotes = true;
          parser.SetDelimiters(",");
          // look for the field names -- they might not be in the first row so find a row
          // whose first column is "Date"

          string[] columnNames = null;
          while (!parser.EndOfData)
          {
            var row = parser.ReadFields();
            // ReSharper disable once PossibleNullReferenceException
            if (row.Length > 0 && row[0] == "Date")
            {
              columnNames = row;
              break;
            }
          }

          if (columnNames == null)
            throw new VoteException(
              "Could not find column names row (\"FirstName\" in first column");
          var maxColumnInx = -1;
          var nameInx = GetColumnIndex(columnNames, "Name" , ref maxColumnInx);
          var address1Inx = GetColumnIndex(columnNames, "Address Line 1", ref maxColumnInx);
          var address2Inx = GetColumnIndex(columnNames, "Address Line 2/District/Neighborhood", ref maxColumnInx);
          var cityInx = GetColumnIndex(columnNames, "Town/City", ref maxColumnInx);
          var stateProvinceInx = GetColumnIndex(columnNames,
           "State/Province/Region/County/Territory/Prefecture/Republic", ref maxColumnInx);
          var zipCodeInx = GetColumnIndex(columnNames, "Zip/Postal Code",
           ref maxColumnInx);
          var emailInx = GetColumnIndex(columnNames, "From Email Address", ref maxColumnInx);
          var transactionDateInx = GetColumnIndex(columnNames, "Date", ref maxColumnInx);
          var totalChangedInx = GetColumnIndex(columnNames, "Gross", ref maxColumnInx);
          var transactionIdInx = GetColumnIndex(columnNames, "Transaction ID", ref maxColumnInx);
          var typeInx = GetColumnIndex(columnNames, "Type", ref maxColumnInx);

          while (!parser.EndOfData)
          {
            var row = parser.ReadFields();
            if (row[typeInx] != "Donation Payment") continue;
            try
            {
              // ReSharper disable once PossibleNullReferenceException
              if (row.Length <= maxColumnInx)
                throw new VoteException(
                  $"Contains only {row.Length} columns, {maxColumnInx + 1} required.");

              var email = row[emailInx];
              if (IsNullOrWhiteSpace(email))
                throw new VoteException("Email is missing");
              if (!Validation.IsValidEmailAddress(email))
                throw new VoteException($"Invalid Email: {email}");

              // we try to split the single name into first and last
              var name = row[nameInx];
              var nameParts = name.Split(new []{' '}, 
                StringSplitOptions.RemoveEmptyEntries);
              var firstName = Empty;
              var lastName = Empty;
              switch (nameParts.Length)
              {
                case 0:
                  break;

                case 1:
                  lastName = nameParts[0];
                  break;

                case 2:
                  firstName = nameParts[0];
                  lastName = nameParts[1];
                  break;

                default:
                  if (Validation.IsValidNameSuffix(nameParts.Last()))
                  {
                    firstName = Join(" ", nameParts.Take(nameParts.Length - 2));
                    lastName = Join(" ", nameParts.Skip(nameParts.Length - 2));
                  }
                  else
                  {
                    firstName = Join(" ", nameParts.Take(nameParts.Length - 1));
                    lastName = nameParts.Last();
                  }
                  break;
              }

              //        var firstName = row[firstNameInx];
              //        var lastName = row[lastNameInx];
              //        var fullName = firstName + " " + lastName;
              //        if (IsNullOrWhiteSpace(fullName))
              //          throw new VoteException("FirstName or LastName is missing");

              var address = row[address1Inx];
              var address2 = row[address2Inx];
              if (!IsNullOrWhiteSpace(address) &&
                !IsNullOrWhiteSpace(address2)) address += ", " + address2;

              var city = row[cityInx];

              var stateCode = row[stateProvinceInx];
              if (!StateCache.IsValidStateCode(stateCode))
              {
                // might be state name
                stateCode = StateCache.GetStateCode(stateCode).SafeString();
              }

              var zipCode = row[zipCodeInx];
              var zipMatch = Regex.Match(zipCode, @"(?<zip5>\d{5})(?:\D?(?<zip4>\d{4}))?");
              var zip5 = Empty;
              var zip4 = Empty;
              if (zipMatch.Success)
              {
                zip5 = zipMatch.Groups["zip5"].Value;
                zip4 = zipMatch.Groups["zip4"].Value;
              }

              var transactionId = row[transactionIdInx].Trim();
              if (IsNullOrWhiteSpace(transactionId))
                throw new VoteException(
                  $"Transaction ID is missing: {row[transactionDateInx]}");

              //        var phone = row[phoneInx].NormalizePhoneNumber();

              if (!DateTime.TryParse(row[transactionDateInx], out var transactionDate))
                throw new VoteException(
                  $"Could not parse TransactionDate: {row[transactionDateInx]}");

              if (!decimal.TryParse(row[totalChangedInx], out var totalCharged))
                throw new VoteException(
                  $"Could not parse TotalCharged: {row[totalChangedInx]}");
              if (totalCharged == 0) continue;

              if (Donations.PayPalTransactionIdExists(transactionId))
                throw new VoteException(
                  "A donation for this PayPal Transaction Id already exists" +
                  $" ({transactionId}, {email}, {transactionDate:G}, {totalCharged:C})");

              Donations.Insert(email, transactionDate, firstName, lastName, name,
                address, city, stateCode, zip5, zip4, Empty, totalCharged, false, transactionId);

              // Get the encoding
              var googleResult = IsNullOrWhiteSpace(stateCode)
                ? null
                : GoogleMapsLookup.Lookup(address, city, stateCode, zip5);
              TigerLookupData tigerResult = null;
              if (googleResult?.Status == "OK")
                tigerResult =
                  TigerLookup.Lookup(googleResult.Latitude, googleResult.Longitude);
              var table = Addresses.GetDataByEmail(email);
              if (table.Count == 0) // Insert
                Addresses.Insert(firstName, lastName, address, city, stateCode, zip5, zip4,
                  email, Empty, now, "DONR", false, false, false, DefaultDbDate,
                  Empty, tigerResult?.Congress ?? Empty,
                  tigerResult?.Upper ?? Empty, tigerResult?.Lower ?? Empty,
                  tigerResult?.County ?? Empty, tigerResult?.District ?? Empty,
                  tigerResult?.Place ?? Empty, tigerResult?.Elementary ?? Empty,
                  tigerResult?.Secondary ?? Empty, tigerResult?.Unified ?? Empty,
                  tigerResult?.CityCouncil ?? Empty,
                  tigerResult?.CountySupervisors ?? Empty,
                  tigerResult?.SchoolDistrictDistrict ?? Empty, 
                  googleResult?.Latitude ?? 0, googleResult?.Longitude ?? 0, 
                  googleResult?.Status == "OK" ? now : DefaultDbDate,
                  0, DefaultDbDate, true);
              else if (googleResult?.Status == "OK") // update all matching Addresses
              {
                foreach (var r in table)
                {
                  if (!IsNullOrWhiteSpace(firstName)) r.FirstName = firstName;
                  if (!IsNullOrWhiteSpace(lastName)) r.LastName = lastName;
                  //if (!IsNullOrWhiteSpace(phone)) r.Phone = phone;
                  r.IsDonor = true;
                  if (tigerResult != null)
                  {
                    r.Address = address;
                    r.City = city;
                    r.StateCode = stateCode;
                    r.Zip5 = zip5;
                    r.Zip4 = zip4;
                    r.CongressionalDistrict = tigerResult.Congress;
                    r.StateSenateDistrict = tigerResult.Upper;
                    r.StateHouseDistrict = tigerResult.Lower;
                    r.County = tigerResult.County;
                    r.District = tigerResult.District;
                    r.Place = tigerResult.Place;
                    r.Latitude = googleResult.Latitude;
                    r.Longitude = googleResult.Longitude;
                    r.DistrictLookupDate = now;
                  }
                }
                Addresses.UpdateTable(table);
              }

              messages.Add(
                $"<p>{totalCharged:C} donation added for {name}&lt;{email}&gt; on {transactionDate.Date.ToShortDateString()}</p>");
              good++;
            }
            catch (Exception ex)
            {
              messages.Add(
                $"<p class=\"error\">Error on row {parser.LineNumber}: {ex.Message}</p>");
              bad++;
            }
          }
        }
      }
      catch (Exception ex)
      {
        messages.Add($"<p class=\"error\">Error: {ex.Message}</p>");
        bad++;
      }
      messages.Add($"<p>{good} donations added. {bad} errors.</p>");
      SummaryPlaceHolder.Controls.Add(
        new LiteralControl(Join(Empty, messages)));
      SummaryContainer.RemoveCssClass("hidden");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Log Donations";
        H1.InnerHtml = "Log Donations";
        Master.MasterForm.Attributes.Add("enctype", "multipart/form-data");
      }
      else
      {
        //ParseEmails();
        PostCsv();
      }
    }
  }
}