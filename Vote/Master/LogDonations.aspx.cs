using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB.Vote;
using Microsoft.VisualBasic.FileIO;

namespace Vote.Master
{
  public partial class LogDonationsPage : SecurePage, ISuperUser
  {
    //private void ParseEmails()
    //{
    //  var now = DateTime.UtcNow;
    //  var messages = new List<string>();
    //  var reversals = new List<DonorInfo>();
    //  var good = 0;
    //  var bad = 0;
    //  var reversed = 0;
    //  for (var i = 0; i < Request.Files.Count; i++)
    //  {
    //    var file = Request.Files[i];
    //    if (file.ContentLength == 0) continue;
    //    try
    //    {
    //      var info = EmailUtility.ExtractDonorInfoFromMsg(file.InputStream);

    //      if (info.IsReversal)
    //      // save reversals until end in case reversal precedes donation in file list
    //      {
    //        reversals.Add(info);
    //        continue;
    //      }

    //      if (Donations.EmailDateExists(info.Email, info.Date))
    //        throw new VoteException(
    //          "A donation for this email, date and time already exists");
    //      Donations.Insert(info.Email, info.Date, info.FirstName, info.LastName,
    //        info.FullName, info.Address, info.City, info.StateCode, info.Zip5,
    //        info.Zip4, info.Phone, info.Amount, false);

    //      // Get the encoding
    //      var input = info.Address + " " + info.City + " " + info.StateCode + " " +
    //        info.Zip5;
    //      if (!string.IsNullOrWhiteSpace(info.Zip4)) input += "-" + info.Zip4;
    //      var result = AddressFinder.Find(input, null, false);
    //      if ((result.Congress != null) && (result.Congress.Length == 3))
    //        result.Congress = result.Congress.Substring(1);
    //      var table = Addresses.GetDataByEmail(info.Email);
    //      if (table.Count == 0) // Insert
    //        Addresses.Insert(info.FirstName, info.LastName, info.Address,
    //          info.City, info.StateCode, info.Zip5, info.Zip4, info.Email,
    //          info.Phone, now, "DONR", false, true, false, DefaultDbDate, string.Empty,
    //          result.Congress.SafeString(), result.StateSenate.SafeString(),
    //          result.StateHouse.SafeString(), result.County.SafeString(),
    //          result.Success ? now : DefaultDbDate, 0, DefaultDbDate);
    //      else if (result.Success) // update all matching Addresses
    //      {
    //        foreach (var row in table)
    //        {
    //          row.FirstName = info.FirstName;
    //          row.LastName = info.LastName;
    //          row.Address = info.Address;
    //          row.City = info.City;
    //          row.StateCode = info.StateCode;
    //          row.Zip5 = info.Zip5;
    //          row.Zip4 = info.Zip4;
    //          row.Phone = info.Phone;
    //          row.CongressionalDistrict = result.Congress;
    //          row.StateSenateDistrict = result.StateSenate;
    //          row.StateHouseDistrict = result.StateHouse;
    //          row.County = result.County;
    //          row.DistrictLookupDate = now;
    //        }
    //        Addresses.UpdateTable(table);
    //      }

    //      messages.Add(
    //        $"<p>{info.Amount:C} donation added for {info.FullName}&lt;{info.Email}&gt; on {info.Date:G}</p>");
    //      good++;
    //    }
    //    catch (Exception ex)
    //    {
    //      messages.Add(
    //        $"<p class=\"error\">Error processing file: {file.FileName}, {ex.Message}</p>");
    //      bad++;
    //    }
    //  }

    //  // process reversals
    //  foreach (var reversal in reversals)
    //    try
    //    {
    //      if (Donations.DeleteByEmailDate(reversal.Email, reversal.Date) == 0)
    //        throw new VoteException("Could not find donation to reverse");
    //      messages.Add($"<p>Donation from {reversal.Email} on {reversal.Date:G} was reversed</p>");
    //      reversed++;
    //    }
    //    catch (Exception ex)
    //    {
    //      messages.Add(
    //        $"<p class=\"error\">Could not reverse donation from {reversal.Email} on {reversal.Date:G}: {ex.Message}</p>");
    //      bad++;
    //    }

    //  messages.Add($"<p>{good} donations added. {reversed} donations reversed. {bad} errors.</p>");
    //  SummaryPlaceHolder.Controls.Add(new LiteralControl(string.Join(string.Empty, messages)));
    //  SummaryContainer.RemoveCssClass("hidden");
    //}

    private static int GetColumnIndex(string[] columnNames, string columnName, ref int maxColumnInx)
    {
      var inx = Array.IndexOf(columnNames, columnName);
      if (inx < 0)
        throw new VoteException($"Column name {columnName} not found in csv");
      if (inx > maxColumnInx) maxColumnInx = inx;
      return inx;
    }

    private static void NonFatalError(ICollection<string> messages, long lineNumber, 
      string message)
    {
      messages.Add(
        $"<p class=\"error\">Non-fatal error on row {lineNumber}: {message}</p>");
    }

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
        using (var parser = new TextFieldParser(Request.Files[0].InputStream, Encoding.UTF8))
        {
          parser.TextFieldType = FieldType.Delimited;
          parser.HasFieldsEnclosedInQuotes = true;
          parser.SetDelimiters(",");
          // look for the field names -- they aren't the first row so find a row whose
          // first column is "FirstName"

          string[] columnNames = null;
          while (!parser.EndOfData)
          {
            var row = parser.ReadFields();
            // ReSharper disable once PossibleNullReferenceException
            if (row.Length > 0 && row[0] == "FirstName")
            {
              columnNames = row;
              break;
            }
          }

          if (columnNames == null)
            throw new VoteException("Could not find column names row (\"FirstName\" in first column");
          var maxColumnInx = -1;
          var firstNameInx = GetColumnIndex(columnNames, "FirstName", ref maxColumnInx);
          var lastNameInx = GetColumnIndex(columnNames, "LastName", ref maxColumnInx);
          var address1Inx = GetColumnIndex(columnNames, "Address1", ref maxColumnInx);
          var address2Inx = GetColumnIndex(columnNames, "Address2", ref maxColumnInx);
          var cityInx = GetColumnIndex(columnNames, "City", ref maxColumnInx);
          var stateProvinceInx = GetColumnIndex(columnNames, "StateProvince", ref maxColumnInx);
          var zipCodeInx = GetColumnIndex(columnNames, "ZipCode", ref maxColumnInx);
          var emailInx = GetColumnIndex(columnNames, "Email", ref maxColumnInx);
          var phoneInx = GetColumnIndex(columnNames, "Phone", ref maxColumnInx);
          var transactionDateInx = GetColumnIndex(columnNames, "TransactionDate", ref maxColumnInx);
          var totalChangedInx = GetColumnIndex(columnNames, "TotalCharged", ref maxColumnInx);

          while (!parser.EndOfData)
          {
            var row = parser.ReadFields();
            try
            {
              // ReSharper disable once PossibleNullReferenceException
              if (row.Length <= maxColumnInx)
                throw new VoteException($"Contains only {row.Length} columns, {maxColumnInx + 1} required.");

              var email = row[emailInx];
              if (string.IsNullOrWhiteSpace(email))
                throw new VoteException("Email is missing");
              if (!Validation.IsValidEmailAddress(email))
                throw new VoteException($"Invalid Email: {email}");

              var firstName = row[firstNameInx];
              var lastName = row[lastNameInx];
              var fullName = firstName + " " + lastName;
              if (string.IsNullOrWhiteSpace(fullName))
                throw new VoteException("FirstName or LastName is missing");

              var address = row[address1Inx];
              var address2 = row[address2Inx];
              if (!string.IsNullOrWhiteSpace(address) && !string.IsNullOrWhiteSpace(address2))
                address += ", " + address2;

              var city = row[cityInx];
              if (string.IsNullOrWhiteSpace(city))
                throw new VoteException("City is missing");

              var stateCode = row[stateProvinceInx];
              if (string.IsNullOrWhiteSpace(stateCode))
                throw new VoteException("StateProvince is missing");
              if (!StateCache.IsValidStateCode(stateCode))
              {
                // might be state name
                stateCode = StateCache.GetStateCode(stateCode);
                if (!StateCache.IsValidStateCode(stateCode))
                  throw new VoteException("Invaid state or state code");
              }

              var zipCode = row[zipCodeInx];
              if (string.IsNullOrWhiteSpace(zipCode))
                NonFatalError(messages, parser.LineNumber, $"ZipCode is missing ({email})");
              var zipMatch = Regex.Match(zipCode, @"(?<zip5>\d{5})(?:\D?(?<zip4>\d{4}))?");
              var zip5 = string.Empty;
              var zip4 = string.Empty;
              if (!zipMatch.Success)
                NonFatalError(messages, parser.LineNumber, $"Could not parse ZipCode: {zipCode} ({email})");
              else
              {
                zip5 = zipMatch.Groups["zip5"].Value;
                zip4 = zipMatch.Groups["zip4"].Value;
              }

              var phone = row[phoneInx].NormalizePhoneNumber();

              DateTime transactionDate;
              if (!DateTime.TryParse(row[transactionDateInx], out transactionDate))
                throw new VoteException($"Could not parse TransactionDate: {row[transactionDateInx]}");

              decimal totalChanged;
              if (!decimal.TryParse(row[totalChangedInx], out totalChanged))
                throw new VoteException($"Could not parse TotalCharged: {row[totalChangedInx]}");

              if (Donations.EmailDateExists(email, transactionDate))
                throw new VoteException(
                  "A donation for this email, date and time already exists" +
                  $" ({email}, {transactionDate:G}, {totalChanged:C})");
              Donations.Insert(email, transactionDate, firstName, lastName, fullName,
                address, city, stateCode, zip5, zip4, phone, totalChanged, false);

              // Get the encoding
              var input = address + " " + city + " " + stateCode + " " + zip5;
              if (!string.IsNullOrWhiteSpace(zip4)) input += "-" + zip4;
              var result = AddressFinder.Find(input, null, false);
              if ((result.Congress != null) && (result.Congress.Length == 3))
                result.Congress = result.Congress.Substring(1);
              var table = Addresses.GetDataByEmail(email);
              if (table.Count == 0) // Insert
                Addresses.Insert(firstName, lastName, address,
                  city, stateCode, zip5, zip4, email,
                  phone, now, "DONR", false, false, false, DefaultDbDate, string.Empty,
                  result.Congress.SafeString(), result.StateSenate.SafeString(),
                  result.StateHouse.SafeString(), result.County.SafeString(),
                  result.Success ? now : DefaultDbDate, 0, DefaultDbDate, true);
              else if (result.Success) // update all matching Addresses
              {
                foreach (var r in table)
                {
                  if (!string.IsNullOrWhiteSpace(firstName))
                    r.FirstName = firstName;
                  if (!string.IsNullOrWhiteSpace(lastName))
                    r.LastName = lastName;
                  if (!string.IsNullOrWhiteSpace(phone))
                    r.Phone = phone;
                  r.IsDonor = true;
                  if (!string.IsNullOrWhiteSpace(result.Congress))
                  {
                    r.Address = address;
                    r.City = city;
                    r.StateCode = stateCode;
                    r.Zip5 = zip5;
                    r.Zip4 = zip4;
                    r.CongressionalDistrict = result.Congress;
                    r.StateSenateDistrict = result.StateSenate;
                    r.StateHouseDistrict = result.StateHouse;
                    r.County = result.County;
                    r.DistrictLookupDate = now;
                  }
                }
                Addresses.UpdateTable(table);
              }

              messages.Add(
                $"<p>{totalChanged:C} donation added for {fullName}&lt;{email}&gt; on {transactionDate:G}</p>");
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
        messages.Add(
          $"<p class=\"error\">Error: {ex.Message}</p>");
        bad++;
      }
      messages.Add($"<p>{good} donations added. {bad} errors.</p>");
      SummaryPlaceHolder.Controls.Add(new LiteralControl(string.Join(string.Empty, messages)));
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