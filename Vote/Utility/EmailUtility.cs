using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using DB.Vote;
using DocumentServices.Modules.Readers.MsgReader.Outlook;

namespace Vote
{
  public static class EmailUtility
  {
    //[Flags]
    //public enum EmailTypes
    //{
    //  None = 0x0000,
    //  StatePrimary = 0x0001,
    //  StateAlternate = 0x0002,
    //  CountyPrimary = 0x0004,
    //  CountyAlternate = 0x0008,
    //  LocalPrimary = 0x0010,
    //  LocalAlternate = 0x0020,
    //  PoliticianPrimary = 0x0040,
    //  PoliticianCampaign = 0x0080,
    //  PoliticianVote = 0x0100,
    //  PoliticianState = 0x0200,
    //  PoliticianLds = 0x0400,
    //  Visitor = 0x0800
    //}

    private static readonly Dictionary<string, object> DomainWhiteList =
      new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
      {
        {"vote-usa.org", null}
      };

    // regex from regexlib.com
    private static readonly Regex EmailRegex =
      new Regex(
        @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");

    private static string ExtractDomain(string email)
    {
      return Regex.Match(email, "(?<=@).*").Value;
    }

    private static List<string> ExtractAllEmailAddresses(string text, bool filter = true)
    {
      var list = new List<string>();
      var match = EmailRegex.Match(text);
      while (match.Success)
      {
        list.Add(match.Value);
        match = match.NextMatch();
      }
      return list.Distinct(StringComparer.OrdinalIgnoreCase)
        .Where(e => !filter || !DomainWhiteList.ContainsKey(ExtractDomain(e)))
        .ToList();
    }

    private static readonly Regex DonorDateTimeRegex =
      new Regex(
        @"^\p{Lu}\p{Ll}+, (?<date>\p{Lu}\p{Ll}+ \d{1,2}, 20\d{2}) \[(?<time>\d{1,2}:\d{2}:\d{2} [A,P]M)\]$");

    private static readonly Regex DonorAmountRegex =
      new Regex(@"^(?:Total|Total Charge|Your card will be charged:)\s+\$\s*(?<amount>\d+\.\d{2})");

    private static readonly Regex DonorCityRegex =
      new Regex(
        @"^(?<city>[^\,]+),+\s+(?<state>\p{Lu}.+?)(?:\s+(?<zip5>\d{5})(?:\D?(?<zip4>\d{4}))?)?$");

    public static DonorInfo ExtractDonorInfoFromMsg(Stream stream)
    {
      var info = new DonorInfo();
      var message = new Storage.Message(stream);
      if (message.BodyText == null) throw new VoteException("BodyText missing from message");
      var lines = message.BodyText.NormalizeNewLines()
        .Split('\n')
        .Select(l => l.Trim())
        .ToList();

      info.IsReversal = lines.Any(l => l.Contains("reverse their charge"));

      var dateTimeLine = lines.FirstOrDefault(l => DonorDateTimeRegex.IsMatch(l));
      if (dateTimeLine == null) throw new VoteException("Could not find date and time");
      var match = DonorDateTimeRegex.Match(dateTimeLine);
      var parsedDate = match.Groups["date"].Value;
      var parsedTime = match.Groups["time"].Value;
      if (!DateTime.TryParse(parsedDate + " " + parsedTime, out info.Date))
        throw new VoteException("Could not parse date and time");

      if (!info.IsReversal)
      {
        var amountLine = lines.FirstOrDefault(l => DonorAmountRegex.IsMatch(l));
        if (amountLine == null) throw new VoteException("Could not find amount");
        match = DonorAmountRegex.Match(amountLine);
        var parsedAmount = match.Groups["amount"].Value;
        if (!decimal.TryParse(parsedAmount, out info.Amount))
          throw new VoteException("Could not parse amount");
      }

      var billingInfoIndex = lines.FindIndex(l => l == "Billing Information");
      if (billingInfoIndex < 0) throw new VoteException("Could not find Billing Information");
      // Old format : there is a "+++" line after Billing Information that starts the
      // "real" billing info
      var plusesIndex = lines.FindIndex(billingInfoIndex + 1, l => l == "+++");
      int billingInfoTerminatorIndex;
      if (plusesIndex >= 0) // old format
      {
        billingInfoIndex = plusesIndex;
        // Billing Information is delimited by a line of all "="
        billingInfoTerminatorIndex = lines.FindIndex(billingInfoIndex + 1,
          l => l.All(c => c == '='));
      }
      else // new format
      {
        // Billing Information is delimited by a blank line
        billingInfoTerminatorIndex = lines.FindIndex(billingInfoIndex + 1,
          string.IsNullOrWhiteSpace);
      }
      if (billingInfoTerminatorIndex < 0)
        throw new VoteException("Could not find Billing Information delimiter");
      var billingInfo = lines.Skip(billingInfoIndex + 1)
        .Take(billingInfoTerminatorIndex - billingInfoIndex - 1)
        .Where(l => !string.IsNullOrWhiteSpace(l))
        .ToList();

      // assume last line is email
      var email = billingInfo.Count > 0
        ? billingInfo[billingInfo.Count - 1].Replace("Email: ", string.Empty)
        : null;
      if (!Validation.IsValidEmailAddress(email))
        throw new VoteException("Could not find Email");
      info.Email = email;
      billingInfo.RemoveAt(billingInfo.Count - 1);

      if (!info.IsReversal)
      {
        // assume first line is name
        if (billingInfo.Count == 0) throw new VoteException("Could not find Name");
        info.FullName = billingInfo[0];
        billingInfo.RemoveAt(0);
        var parsedName = info.FullName.ParseName();
        if (string.IsNullOrWhiteSpace(parsedName.First) ||
          string.IsNullOrWhiteSpace(parsedName.Last))
          throw new VoteException("Could not parse Name");
        info.FirstName = parsedName.First;
        info.LastName = parsedName.Last;

        // find the city, state zip line
        var cityIndex = billingInfo.FindLastIndex(l => DonorCityRegex.IsMatch(l));
        if (cityIndex < 0) throw new VoteException("Could not find City, State Zip");
        match = DonorCityRegex.Match(billingInfo[cityIndex]);
        info.City = match.Groups["city"].Value;
        info.StateCode = match.Groups["state"].Value;
        if (!StateCache.IsValidStateCode(info.StateCode))
        {
          // might be state name
          info.StateCode = StateCache.GetStateCode(info.StateCode);
          if (!StateCache.IsValidStateCode(info.StateCode))
            throw new VoteException("Invaid state or state code");
        }
        info.Zip5 = match.Groups["zip5"].Value;
        info.Zip4 = match.Groups["zip4"].Value;

        // everything before city, state zip is address
        info.Address = string.Join(", ", billingInfo.Take(cityIndex));

        // last remaining line is assumed to be phone if it contains any digits
        info.Phone = billingInfo.Skip(cityIndex + 1)
          .LastOrDefault() ?? string.Empty;
        info.Phone = info.Phone.Replace("Phone: ", string.Empty);
        if (!info.Phone.Any(char.IsDigit)) info.Phone = string.Empty;
      }

      return info;
    }

    //public static string ExtractUndeliverableEmailAddressFromMsg(string msgFilePath)
    //{
    //  using (var stream = File.Open(msgFilePath, FileMode.Open, FileAccess.Read)) 
    //    return ExtractUndeliverableEmailAddressFromMsg(stream);
    //}

    public static string ExtractUndeliverableEmailAddressFromMsg(Stream stream)
    {
      var message = new Storage.Message(stream);
      if (message.BodyText == null) throw new VoteException("BodyText missing from message");
      var emails = ExtractAllEmailAddresses(message.BodyText);
      if (emails.Count == 0)
        throw new VoteException("Message did not contain any qualifying email addresses");
      if (emails.Count > 1)
        throw new VoteException("Message contains multiple email addresses");
      return emails[0];
    }

    public static SmtpClient GetConfiguredSmtpClient()
    {
      var host = "localhost";
      var port = 25;
      string username = null;
      string password = null;
      var settings = ConfigurationManager.AppSettings["SmtpClientSettings"];
      if (!string.IsNullOrWhiteSpace(settings))
      {
        var setting = settings.Split(':');
        host = setting[0];
        if (setting.Length >= 2)
          int.TryParse(setting[1], out port);
        if (setting.Length == 3)
        {
          var userpw = setting[2].Split('@');
          if (userpw.Length == 2)
          {
            username = userpw[0];
            password = userpw[1];
          }
        }
      }
      var client = new SmtpClient(host, port);
      if (!string.IsNullOrWhiteSpace(username))
      {
        client.Credentials = new NetworkCredential(username, password);
        client.EnableSsl = true;
      }
      return client;
    }

    public static void UpdateSubscription(string email, string op)
    { // get all matching email entries from Addresses
      var addressesTable = Addresses.GetDataByEmail(email);
      if (addressesTable.Count > 0)
        switch (op)
        {
          case "unsubscribe":
            foreach (var row in addressesTable)
            {
              row.SendSampleBallots = false;
              row.OptOut = true;
            }
            break;

          case "ballots":
            foreach (var row in addressesTable)
            {
              row.SendSampleBallots = true;
              row.OptOut = false;
            }
            break;
        }
      Addresses.UpdateTable(addressesTable);
    }
  }

  public class DonorInfo
  {
    public DateTime Date;
    public string FirstName;
    public string LastName;
    public string FullName;
    public string Address;
    public string City;
    public string StateCode;
    public string Zip5;
    public string Zip4;
    public string Email;
    public string Phone;
    public decimal Amount;
    public bool IsReversal;
  }
}