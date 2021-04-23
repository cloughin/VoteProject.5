using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using DB.Vote;
using DocumentServices.Modules.Readers.MsgReader.Outlook;
using static System.String;

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

    private static readonly Regex SourceCodeRegex = new Regex(@"\*[A-Z]{1,2}-[A-Z]{0,2}[0-9]*\*");

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

      // also check for source code
      var match2 = EmailRegex.Match(text);
      while (match2.Success)
      {
        var email = LookupUpEmailSourceCode(match.Value.Trim('*'));
        if (!IsNullOrWhiteSpace(email))
          list.Add(match.Value);
        match = match.NextMatch();
      }

      return list.Distinct(StringComparer.OrdinalIgnoreCase)
        .Where(e => !filter || !DomainWhiteList.ContainsKey(ExtractDomain(e)))
        .ToList();
    }

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
      var settings = WebConfigurationManager.AppSettings["SmtpClientSettings"];
      if (!IsNullOrWhiteSpace(settings))
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
      if (!IsNullOrWhiteSpace(username))
      {
        client.Credentials = new NetworkCredential(username, password);
        client.EnableSsl = true;
      }
      return client;
    }

    public static string LookupUpEmailSourceCode(string sourceCode)
    {
      var sourceType = sourceCode[0];
      var subType = sourceCode[1];
      var hyphenPos = sourceCode.IndexOf('-');
      var stateCode = Empty;
      if (hyphenPos <= 0 || hyphenPos > 2 || sourceCode.Length < hyphenPos + 2 )
        return null;

      switch (sourceType)
      {
        case 'S':
        case 'C':
        case 'L':
          if (sourceCode.Length < 5) return null;
          if (subType != 'P' && subType != 'A') return null;
          stateCode = sourceCode.Substring(hyphenPos + 1, 2);
          hyphenPos = 4;
          if (!StateCache.IsValidStateCode(stateCode)) return null;
          break;

        case 'P':
          if (subType != 'M' && subType != 'C' && subType != 'S' && subType != 'V') return null;
          break;

        case 'A':
        case 'Z':
        case 'O':
          break;

        default:
          return null;
      }

      var idString = sourceCode.Substring(hyphenPos + 1);
      if (!int.TryParse(idString, out var id) && sourceType != 'S')
        return null;

      switch (sourceType)
      {
        case 'S':
          switch (subType)
          {
            case 'P':
              return States.GetContactEmail(stateCode);

            case 'A':
              return States.GetAltEmail(stateCode);
          }
          break;

        case 'C':
          switch (subType)
          {
            case 'P':
              return Counties.GetContactEmail(stateCode, idString);

            case 'A':
              return Counties.GetAltEmail(stateCode, idString);
          }
          break;

        case 'L':
          switch (subType)
          {
            case 'P':
              return LocalDistricts.GetContactEmail(stateCode, idString);

            case 'A':
              return LocalDistricts.GetAltEmail(stateCode, idString);
          }
          break;

        case 'P':
          switch (subType)
          {
            case 'M':
              return Politicians.GetEmailById(id);

            case 'C':
              return Politicians.GetCampaignEmailById(id);

            case 'S':
              return Politicians.GetStateEmailById(id);

            case 'V':
              return Politicians.GetEmailVoteUSAById(id);
          }
          break;

        case 'A':
          return Addresses.GetEmailById(id);

        case 'Z':
          return PartiesEmails.GetPartyEmailById(id);

        case 'O':
          return OrganizationContacts.GetEmailByContactId(id);
      }

      return null;
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

  //public class DonorInfo
  //{
  //  public DateTime Date;
  //  public string FirstName;
  //  public string LastName;
  //  public string FullName;
  //  public string Address;
  //  public string City;
  //  public string StateCode;
  //  public string Zip5;
  //  public string Zip4;
  //  public string Email;
  //  public string Phone;
  //  public decimal Amount;
  //  public bool IsReversal;
  //}
}