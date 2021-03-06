using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using LumenWorks.Framework.IO.Csv;
using Vote.Controls;
using Vote.Reports;

namespace Vote.Admin
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [ToolboxItem(false)]
  [ScriptService]
  public class WebService : System.Web.Services.WebService
  {
    // ReSharper disable NotAccessedField.Global
    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable UnassignedField.Global

    // return classes

    public sealed class AddressesDates
    {
      public DateTime? LastRemoveMalformed;
      public DateTime? LastTransferFromAddressLog;
      public DateTime? LastTransferFromSampleBallotLog;
      public DateTime? LastLookupMissingDistricts;
      public DateTime? LastRefreshAllDistricts;
    }

    public sealed class BallotPediaCsv
    {
      public int Id;
      public string Filename;
      public DateTime UploadTime;
      public int Candidates;
      public int CandidatesCoded;
      public bool Completed;
    }

    public sealed class BallotPediaProposedCandidate
    {
      public string Name;
      public string VoteUsaId;
      public string Status;
    }

    public sealed class BallotPediaCsvCandidate
    {
      public string Name;
      public string SplitName;
      public string SortName;
      public string StateCode;
      public string Office;
      public string VoteUsaId;
      public bool IsIncumbent;
      public string Party;
      public BallotPediaProposedCandidate[] Proposed;
    }

    public sealed class BatchEmailSummary
    {
      public string Description;
      public int Sent;
      public int Failed;
      public int Total;
      // ReSharper disable once CollectionNeverQueried.Global
      public readonly List<EmailFailure> Failures = new List<EmailFailure>();
    }

    public sealed class Donation
    {
      public string Date;
      public string Amount;
    }

    public sealed class EmailBatch
    {
      public int BatchId;
      public int Duplicates;
      public EmailItem[] Items;
    }

    public sealed class EmailFailure
    {
      public string Contact;
      public string SortContact;
      public string ToAddresses;
      public string Message;
    }

    public sealed class EmailItem
    {
      public int Id;
      public string Email;
      public string Contact;
      public string Title;
      public string Phone;
      public string StateCode;
      public string CountyCode;
      public string LocalCode;
      public string Jurisdiction;
      public string PoliticianKey;
      public string ElectionKey;
      public string ElectionKeyToInclude;
      public string OfficeKey;
      public string PartyKey;
      public string PartyEmail;
      public int VisitorId;
      public int DonorId;
      public string SortName;
    }

    public sealed class EmailSendStatus
    {
      public int Sent;
      public int Failed;
    }

    public sealed class EmailSubstitution
    {
      public string Subject;
      public string Body;
      public string ErrorMessage;
    }

    public sealed class EmailTemplateData
    {
      public int Id;
      public string Name;
      public string Owner;
      public string OwnerType;
      public bool IsPublic;
      public DateTime CreateTime;
      public DateTime ModTime;
      public string Subject;
      public string Body;
      public string SelectRecipientOptions;
      public string EmailOptions;
      public bool IsOwner;
    }

    public sealed class EmailTemplateItem
    {
      public int Id;
      public string Name;
      public string Owner;
      public string OwnerType;
      public bool IsPublic;
      public bool IsOwner;
      public string Requirements;
      public DateTime CreateTime;
      public DateTime ModTime;
      public DateTime LastUsedTime;
    }

    public sealed class GetCandidateHtmlResult
    {
      public string PoliticianKey;
      public string CandidateHtml;
      public bool OpenEditDialog;
    }

    public sealed class GetRunningMateHtmlResult
    {
      public string PoliticianKey;
      public string RunningMateHtml;
      public string MainCandidateKey;
      public bool OpenEditDialog;
    }

    public sealed class LogEmailBatchItem
    {
      public int Id;
      public DateTime CreationTime;
      public string SelectionCriteria;
      public string Description;
      public int Found;
      public int Skipped;
      public int Sent;
      public int Failed;
      public string UserName;
      public string FromEmail;
      public string[] CcEmails;
      public string[] BccEmails;
    }

    public sealed class LoggedEmailDetail
    {
      public int Id;
      public string Body;
      public string[] CcEmails;
      public string[] BccEmails;
      public string BatchDescription;
      public string SelectionCriteria;
    }

    public sealed class LoggedEmailItem
    {
      public int Id;
      public DateTime SentTime;
      public bool WasSent;
      public string ContactType;
      public string StateCode;
      public string CountyCode;
      public string LocalCode;
      public string ElectionKey;
      public string OfficeKey;
      public string PoliticianKey;
      public string Contact;
      public string FromEmail;
      public string ToEmail;
      public string Subject;
      public string UserName;
      public string Jurisdiction;
      public string ElectionDescription;
      public string OfficeName;
      public string PoliticianName;
      public string PartyCode;
      public string SortName;
      public int ForwardedCount;
      public bool IsFlagged;
      public string ErrorMessage;
    }

    public sealed class LoggedEmailNote
    {
      public DateTime DateStamp;
      public string Note;
      public bool IsSystemNote;
    }

    public sealed class MoreRecipientInfo
    {
      public string Election;
      public string Office;
      public string Party;
      // following for WebsiteVisitor type
      public DateTime DateAdded;
      public string Address;
      public string CityStateZip;
      public string CongressionalDistrict;
      public string StateSenateDistrict;
      public string StateHouseDistrict;
    }

    public sealed class OfficeAnalysis
    {
      public string[] InElectionsOffices;
      public string[] InElectionsPoliticians;
      public string[] InOfficesOfficials;
      public string[] InElectionsIncumbentsRemoved;
      public string[] InPoliticiansLiveOfficeKey;
      public string[] InPoliticiansTemporaryOfficeKey;
    }

    public sealed class Report
    {
      public string Title;
      public string Html;
    }

    public sealed class RestoreInfo
    {
      public string StateCode;
      public string CountyCode;
      public string LocalCode;
      public SimpleListItem[] Counties;
      public SimpleListItem[] Locals;
      public SimpleListItem[] Parties;
      public SimpleListItem[] Districts;
      public string ElectionControlHtml;
    }

    public sealed class UpdateBallotPediaCsvInfo
    {
      public string Message;
      public bool Ok;
    }

    public sealed class UserData
    {
      public string UserName;
      public string Password;
      public string UserLevel;
      public string StateCode;
      public string CountyCode;
      public string LocalCode;
      public string CountyName;
      public string LocalName;
    }

    public sealed class Volunteer
    {
      public string Email;
      public string Password;
      public string PartyKey;
      public string FirstName;
      public string LastName;
      public string Phone;
      public string PartyName;
      public DateTime DateStamp;
      public string StateCode;
      public string StateName;
    }

    public sealed class VolunteerNote
    {
      public string Notes;
      public DateTime DateStamp;
      public int Id;
    }

    public sealed class VolunteerNotes
    {
      public string Email;
      public string FirstName;
      public string LastName;
      public string Phone;
      public string PartyName;
      public string StateName;
      public VolunteerNote[] Notes;
    }
    // ReSharper restore UnassignedField.Global
    // ReSharper restore ClassNeverInstantiated.Global
    // ReSharper restore NotAccessedField.Global

    // H e l p e r s

    private static string FindCandidate(string stateCode, string lastName,
      string firstName, ref List<BallotPediaProposedCandidate> proposed)
    {
      // try to encode with exact match
      var table = Politicians.GetNamesDataByStateCodeLastName(stateCode, lastName);
      var matches = table.Where(row => row.FirstName.IsEqIgnoreCase(firstName))
        .ToList();
      if (matches.Count == 1)
      {
        proposed.Clear();
        return matches[0].PoliticianKey;
      }
      if (table.Count == 0)
        proposed.AddRange(Politicians.GetCandidateListRows(lastName, stateCode)
          .Select(r =>
          {
            var name = Politicians.GetFormattedName(r.PoliticianKey());
            var status = Politicians.FormatOfficeAndStatus(r);
            if (!string.IsNullOrWhiteSpace(r.PartyCode())) name += " (" + r.PartyCode() + ")";
            return new BallotPediaProposedCandidate
            {
              Name = name,
              VoteUsaId = r.PoliticianKey(),
              Status = status
            };
          }));
      else
        proposed.AddRange(table.Select(r =>
        {
          var name = Politicians.GetFormattedName(r.PoliticianKey);
          var info = Politicians.GetPoliticianIntroReportData(r.PoliticianKey);
          var status = Politicians.FormatOfficeAndStatus(info);
          var party = Parties.GetPartyCode(info.PartyKey());
          if (!string.IsNullOrWhiteSpace(party)) name += " (" + party + ")";
          return new BallotPediaProposedCandidate
          {
            Name = name,
            VoteUsaId = r.PoliticianKey,
            Status = status
          };
        }));

      proposed = proposed.DistinctBy(p => p.VoteUsaId).ToList();
      return string.Empty;
    }

    private static LoggedEmailNote[] GetEmailNotesPrivate(int id)
    {
      return LogEmailNotes.GetDataByLogEmailId(id)
        .Select(
          row =>
            new LoggedEmailNote
            {
              DateStamp = row.DateStamp.AsUtc(),
              Note = row.Note,
              IsSystemNote = row.IsSystemNote
            })
        .OrderByDescending(row => row.DateStamp)
        .ToArray();
    }

    private static VolunteerNote[] GetVolunteerNotesArray(string email)
    {
      return VolunteersNotes.GetDataByEmail(email)
        .Select(n => new VolunteerNote
        {
          Notes = HttpUtility.HtmlEncode(n.Notes).ReplaceNewLinesWithBreakTags(),
          DateStamp = n.DateStamp.AsUtc(),
          Id = n.Id
        })
        .OrderByDescending(n => n.Id)
        .ToArray();
    }

    // W e b   M e t h o d s

    [WebMethod]
    public LoggedEmailNote[] AddEmailNote(int id, string note)
    {
      try
      {
        LogEmailNotes.Insert(id, DateTime.UtcNow, note, false);
        return GetEmailNotesPrivate(id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/AddEmailNote", e);
        throw;
      }
    }

    [WebMethod]
    public string AddOffice(string stateCode, string countyCode, string localCode, 
      string officeClass, string line1, string line2)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      var officeKey = G.CreateOfficeKey(parsedOfficeClass.ToInt(), stateCode, countyCode, localCode,
        string.Empty, string.Empty, line1, line2);

      if (string.IsNullOrWhiteSpace(line1))
        return "*The 1st Line of Office Title is required";

      if (Offices.OfficeKeyExists(officeKey))
        return $"*The proposed office key ({officeKey}) already exists";

      Offices.Insert(officeKey, stateCode, Offices.GetCountyCodeFromKey(officeKey),
        Offices.GetLocalCodeFromKey(officeKey), string.Empty, string.Empty, line1, line2,
        parsedOfficeClass.ToInt(), 0, 0, false, false, 1, string.Empty, "(Vote for no more than one)", 
        string.Empty, "Write in", 1, false, DateTime.UtcNow, false, false, false, 1, 1, 0, 0, false);

      return officeKey;
    }

    [WebMethod]
    public string AddOfficeTemplate(string stateCode, string officeClass, string line1, string line2)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      var localCode = parsedOfficeClass.ElectoralClass() == ElectoralClass.Local
        ? "##"
        : string.Empty;
      var officeKey = G.CreateOfficeKey(parsedOfficeClass.ToInt(), stateCode, "###", localCode,
        string.Empty, string.Empty, line1, line2);

      if (string.IsNullOrWhiteSpace(line1))
        return "*The 1st Line of Office Title is required";

      if (Offices.OfficeKeyExists(officeKey))
        return $"*The proposed office key ({officeKey}) already exists";

      Offices.Insert(officeKey, stateCode, Offices.GetCountyCodeFromKey(officeKey),
        Offices.GetLocalCodeFromKey(officeKey), string.Empty, string.Empty, line1, line2,
        parsedOfficeClass.ToInt(), 0, 0, false, false, 1, string.Empty, "(Vote for no more than one)", 
        string.Empty, "Write in", 1, false, DateTime.UtcNow, false, false, false, 1, 1, 0, 0, true);

      return officeKey;
    }

    [WebMethod(EnableSession = true)]
    public string AddUser(string userName, string password, string level, string stateCode,
      string countyCode, string localCode)
    {
      if (!SecurePage.IsSuperUser) return "*Unauthorized";
      userName = userName.Trim();
      password = password.Trim();
      if (string.IsNullOrWhiteSpace(userName)) return "*User Name is required";
      if (string.IsNullOrWhiteSpace(password)) return "*Password is required";
      if (string.IsNullOrWhiteSpace(level)) return "*Security Level is required";
      if (Security.UserNameExists(userName)) return "*User Name is already in use";
      if (level != "MASTER")
      {
        if (string.IsNullOrWhiteSpace(stateCode))
          return "*State Code is required";
      }
      else stateCode = string.Empty;
      if (level != "MASTER" && level != "ADMIN")
      {
        if (string.IsNullOrWhiteSpace(countyCode))
          return "*County Code is required";
      }
      else countyCode = string.Empty;
      if (level == "LOCAL")
      {
        if (string.IsNullOrWhiteSpace(localCode))
          return "*LOCAL Code is required";
      }
      else localCode = string.Empty;
      Security.Insert(level, userName, password, string.Empty, string.Empty, stateCode, countyCode,
        localCode, string.Empty, string.Empty, string.Empty, string.Empty, false, false, false);
      return "Ok";
    }

    [WebMethod]
    public VolunteerNote[] AddVolunteerNote(string email, string note)
    {
      VolunteersNotes.Insert(email, DateTime.UtcNow, note);
      return GetVolunteerNotesArray(email);
    }

    [WebMethod]
    public OfficeAnalysis AnalyzeOfficeForDeletion(string officeKey)
    {
      // OfficeKey could be referenced in:
      // ElectionsOffices.OfficeKey (will show Elections)
      // ElectionsPoliticians.OfficeKey (will show Elections and Politician count)
      // OfficesOfficials.OfficeKey (will show Politician)
      // ElectionsIncumbentsRemoved.OfficeKey (will show Politician)
      // Politicians.LiveOfficeKey (will show Politician)
      // Politicians.TemporaryOfficeKey (will show Politician)

      return new OfficeAnalysis
      {
        InElectionsOffices = ElectionsOffices.GetDataByOfficeKey(officeKey)
          .OrderBy(r => r.ElectionKey)
          .Select(r => $"{Elections.GetElectionDesc(r.ElectionKey)} ({r.ElectionKey})")
          .ToArray(),

        InElectionsPoliticians = ElectionsPoliticians.GetDataByOfficeKey(officeKey)
          .GroupBy(r => r.ElectionKey)
          .OrderBy(g => g.Key)
          .Select(
            g =>
                $"{Elections.GetElectionDesc(g.First().ElectionKey)} ({g.First().ElectionKey}: {g.Count()} candidates)")
          .ToArray(),

        InOfficesOfficials = OfficesOfficials.GetDataByOfficeKey(officeKey)
          .Select(r => $"{Politicians.GetFormattedName(r.PoliticianKey)} ({r.PoliticianKey})")
          .OrderBy(d => d)
          .ToArray(),

        InElectionsIncumbentsRemoved = ElectionsIncumbentsRemoved.GetDataByOfficeKey(officeKey)
          .GroupBy(r => r.ElectionKey)
          .OrderBy(g => g.Key)
          .Select(
            g =>
                $"{Elections.GetElectionDesc(g.First().ElectionKey)} ({g.First().ElectionKey}: {g.Count()} incumbents)")
          .ToArray(),

        InPoliticiansLiveOfficeKey = Politicians.GetDataByLiveOfficeKey(officeKey)
          .Select(r => $"{Politicians.GetFormattedName(r.PoliticianKey)} ({r.PoliticianKey})")
          .OrderBy(d => d)
          .ToArray(),

        InPoliticiansTemporaryOfficeKey = Politicians.GetDataByTemporaryOfficeKey(officeKey)
          .Select(r => $"{Politicians.GetFormattedName(r.PoliticianKey)} ({r.PoliticianKey})")
          .OrderBy(d => d)
          .ToArray()
      };
    }

    [WebMethod]
    public string ApplyBallotPediaLinks(int csvId)
    {
      var message = "BallotPedia Links applied to VoteUSA website.";
      try
      {
        var content = BallotPediaCsvs.GetContentById(csvId);
        if (string.IsNullOrWhiteSpace(content))
          throw new VoteException("Missing CSV");

        var rowsRead = 0;
        var updated = 0;
        var missingIdOrLink = 0;
        var noPolitician = 0;
        using (var csvReader = new CsvReader(new StringReader(content), true))
        {
          var headers = csvReader.GetFieldHeaders();
          if (!headers.Contains("URL"))
            throw new VoteException("BallotPedia URL column missing");
          if (!headers.Contains("VoteUSA Id"))
            throw new VoteException("VoteUSA Id column missing");
          while (csvReader.ReadNextRecord())
          {
            rowsRead++;
            var url = Validation.StripWebProtocol(csvReader["URL"]);
            var id = csvReader["VoteUSA Id"];
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(id))
              missingIdOrLink++;
            else if (Politicians.UpdateBallotPediaWebAddress(url, id) == 0)
              noPolitician++;
            else updated++;
          }

          message += "\n\n" + $"Rows read: {rowsRead}\nUpdated: {updated}\n" + 
            $"Missing Id or Link in CSV: {missingIdOrLink}\nMissing politician in DB: {noPolitician}";
        }
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/ApplyBallotPediaLinks", e);
        throw;
      }
      return message;
    }

    [WebMethod(EnableSession = true)]
    public void AspKeepAlive() {}

    [WebMethod]
    public void ChangeLogBatchDescription(int id, string description)
    {
      try
      {
        LogEmailBatches.UpdateDescriptionById(description, id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/ChangeLogBatchDescription", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public void ChangeUserPassword(string userName, string password)
    {
      if (!SecurePage.IsSuperUser) throw new Exception("Unauthorized");
      if (!string.IsNullOrWhiteSpace(password))
        Security.UpdateUserPassword(password, userName);
    }

    [WebMethod]
    public void DeleteBallotPediaCsv(int id)
    {
      try
      {
        BallotPediaCsvs.DeleteById(id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/DeleteBallotPediaCsv", e);
        throw;
      }
    }

    [WebMethod]
    public void DeleteEmailTemplate(int id)
    {
      try
      {
        EmailTemplates.DeleteById(id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/DeleteEmailTemplate", e);
        throw;
      }
    }

    [WebMethod]
    public void DeleteOffice(string officeKey)
    {
      ElectionsOffices.DeleteByOfficeKey(officeKey);
      ElectionsPoliticians.DeleteByOfficeKey(officeKey);
      OfficesOfficials.DeleteByOfficeKey(officeKey);
      ElectionsIncumbentsRemoved.DeleteByOfficeKey(officeKey);
      Politicians.UpdateLiveOfficeKeyByLiveOfficeKey(string.Empty, officeKey);
      Politicians.UpdateTemporaryOfficeKeyByTemporaryOfficeKey(string.Empty, officeKey);
      Offices.DeleteByOfficeKey(officeKey);
    }

    [WebMethod(EnableSession = true)]
    public void DeleteUser(string userName)
    {
      if (!SecurePage.IsSuperUser) throw new Exception("Unauthorized");
      Security.DeleteByUserName(userName);
    }

    [WebMethod(EnableSession = true)]
    public void DeleteVolunteer(string email)
    {
      if (!SecurePage.IsSuperUser) throw new Exception("Unauthorized");
      VolunteersView.DeleteByEmail(email);
      VolunteersNotes.DeleteByEmail(email);
    }

    [WebMethod]
    public VolunteerNote[] DeleteVolunteerNote(string email, int id)
    {
      VolunteersNotes.DeleteById(id);
      return GetVolunteerNotesArray(email);
    }

    [WebMethod]
    public Donation[] FindDonation(string email)
    {
      return Donations.GetDataByEmail(email).Select(d => new Donation
      {
        Date = d.Date.ToString("G"),
        Amount = d.Amount.ToString("C")
      }).ToArray();
    }

    [WebMethod]
    public void ForwardEmail(int id, string to, string cc, string bcc,
      string subject, string message)
    {
      try
      {
        to = to.Trim();
        cc = cc.Trim();
        bcc = bcc.Trim();
        if (!Validation.IsValidEmailAddress(to)) throw new Exception("The to email address is not valid");
        string[] ccs = null;
        if (!string.IsNullOrWhiteSpace(cc))
        {
          ccs = cc.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();
          foreach (var i in ccs) if (!Validation.IsValidEmailAddress(i)) throw new Exception("The cc email address is not valid");
        }
        string[] bccs = null;
        if (!string.IsNullOrWhiteSpace(bcc))
        {
          bccs = bcc.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();
          foreach (var i in bccs) if (!Validation.IsValidEmailAddress(i)) throw new Exception("The bcc email address is not valid");
        }
        var emailTable = LogEmail.GetDataById(id);
        if (emailTable.Count == 1)
        {
          var emailRow = emailTable[0];
          var batchTable = LogEmailBatches.GetDataById(emailRow.LogBatchId);
          if (batchTable.Count == 1)
          {
            var batchRow = batchTable[0];
            var body = emailRow.Body;
            if (!string.IsNullOrWhiteSpace(message)) body = message.ReplaceNewLinesWithParagraphs() + "<hr />" + body;
            EmailTemplates.SendEmail(subject, body, batchRow.FromEmail, new[] {to},
              ccs, bccs);
            var note =
              $"Forwarded to: {to} {(to.IsEqIgnoreCase(emailRow.ToEmail) ? "(original recipient)" : string.Empty)}\n" +
              $"Subject: {subject}\nMessage: {message}";
            LogEmailNotes.Insert(id, DateTime.UtcNow, note, true);
            LogEmail.UpdateForwardedCountById(emailRow.ForwardedCount + 1, id);
          }
        }
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/ForwardEmail", e);
        throw;
      }
    }

    [WebMethod]
    public AddressesDates GetAddressesDates()
    {
      return AddressesMaster.GetAllData()
        .Select(row => new AddressesDates
        {
          LastRemoveMalformed = row.LastRemoveMalformed.AsUtc(),
          LastTransferFromAddressLog = row.LastTransferFromAddressLog.AsUtc(),
          LastTransferFromSampleBallotLog = row.LastTransferFromSampleBallotLog.AsUtc(),
          LastLookupMissingDistricts = row.LastLookupMissingDistricts.AsUtc(),
          LastRefreshAllDistricts = row.LastRefreshAllDistricts.AsUtc()
        }).SingleOrDefault();
    }

    [WebMethod]
    public BallotPediaCsvCandidate[] GetBallotPediaCsv(int id)
    {
      try
      {
        var candidates = new List<BallotPediaCsvCandidate>();
        var content = BallotPediaCsvs.GetContentById(id);
        var csvChanged = false;
        var coded = 0;
        if (!string.IsNullOrWhiteSpace(content))
        {
          using (
            var csvReader = new CsvReader(new StringReader(content), true))
          using (var ms = new MemoryStream())
          {
            var streamWriter = new StreamWriter(ms);
            var csvWriter = new SimpleCsvWriter();

            var headers = csvReader.GetFieldHeaders();
            // write headers
            foreach (var col in headers)
              csvWriter.AddField(col);
            csvWriter.Write(streamWriter);

            var hasName = headers.Contains("Name");
            var hasOffice = headers.Contains("Office");
            var hasStatus = headers.Contains("Status");
            var hasParty = headers.Contains("Party");
            while (csvReader.ReadNextRecord())
            {
              var firstName = csvReader["FirstName"];
              var lastName = csvReader["LastName"];
              var voteUsaId = csvReader["VoteUSA Id"];
              var stateCode = StateCache.GetStateCode(csvReader["State"]);
              var proposed = new List<BallotPediaProposedCandidate>();
              if (string.IsNullOrWhiteSpace(stateCode)) 
                stateCode = csvReader["State"];
              else if (string.IsNullOrWhiteSpace(voteUsaId))
              {
                voteUsaId = FindCandidate(stateCode, lastName, firstName, 
                  ref proposed);
                var lastName2 = string.Empty;
                if (string.IsNullOrWhiteSpace(voteUsaId))
                {
                  // parse BallotPedia's full name - if last name
                  // is different retry
                  var parsedName = csvReader["Name"].ParseName();
                  if (parsedName.Last.IsNeIgnoreCase(lastName))
                  {
                    voteUsaId = FindCandidate(stateCode, parsedName.Last,
                      parsedName.First, ref proposed);
                    lastName2 = parsedName.Last;
                  }
                }
                if (string.IsNullOrWhiteSpace(voteUsaId))
                {
                  // parse BallotPedia's page title - if last name
                  // is different retry
                  var parsedName = csvReader["Page Title"].Replace('_', ' ').
                    ParseName();
                  if (parsedName.Last.IsNeIgnoreCase(lastName) &&
                    parsedName.Last.IsNeIgnoreCase(lastName2))
                    voteUsaId = FindCandidate(stateCode, parsedName.Last,
                      parsedName.First, ref proposed);
                }
                csvChanged = !string.IsNullOrWhiteSpace(voteUsaId);
              }
              foreach (var col in headers) 
                csvWriter.AddField(col == "VoteUSA Id" 
                  ? voteUsaId : csvReader[col]);
              csvWriter.Write(streamWriter);
              if (!string.IsNullOrWhiteSpace(voteUsaId)) coded++;
              candidates.Add(new BallotPediaCsvCandidate
              {
                Name = hasName 
                  ? csvReader["Name"]
                  : firstName + " " + lastName,
                SplitName = firstName + " | " + lastName + " (" + csvReader["Page Title"] + ")",
                SortName = lastName + " " + firstName,
                Office = hasOffice ? csvReader["Office"] : string.Empty,
                StateCode = stateCode,
                VoteUsaId = voteUsaId,
                IsIncumbent = hasStatus && csvReader["Status"] == "Incumbent",
                Party = hasParty ? csvReader["Party"] : string.Empty,
                Proposed = proposed.ToArray()
              });
            }
            if (csvChanged)
            {
              streamWriter.Flush();
              ms.Position = 0;
              BallotPediaCsvs.UpdateContentById(new StreamReader(ms).ReadToEnd(), id);
              BallotPediaCsvs.UpdateCandidatesCodedById(coded, id);
            }
          }
        }
        return candidates.ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetBallotPediaCsv", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public GetCandidateHtmlResult GetCandidateHtml(string electionKey,
      string politicianKey, string officeKey, bool openEditDialog, string mode)
    {
      ManagePoliticiansPanel.DataMode theMode;
      if (!Enum.TryParse(mode, out theMode)) 
        theMode = ManagePoliticiansPanel.DataMode.ManageCandidates;
      try
      {
        return new GetCandidateHtmlResult
        {
          PoliticianKey = politicianKey,
          OpenEditDialog = openEditDialog,
          CandidateHtml =
            ManagePoliticiansPanel.GetCandidateHtml(electionKey, politicianKey,
              officeKey, theMode)
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetCandidateHtml", e);
        throw;
      }
    }

    [WebMethod]
    public string GetCandidateList(string partialName, string[] selectedPoliticians,
      string officeKey, string[] currentCandidates, bool noCacheSelected)
    {
      try
      { // Note: the officeKey is used only to get stateCode (1st 2 characters of
        // office key, so a state code can be passed.
        // Enhancement to support multi-state filtering: can pass in a comma-separated
        // string of states. 
        return Politicians.GetCandidateListHtml(partialName, selectedPoliticians,
          officeKey, currentCandidates, false, noCacheSelected);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetCandidateList", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetCandidates(string electionKey, string officeKey)
    {
      try
      {
        return BulkEmailPage.GetPreviewCandidateItems(electionKey, officeKey)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetCandidates", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetCongressionalDistricts(string stateCode)
    {
      try
      { // reduce CongressionalDistrict codes from 3 to 2 characters
        return Offices.GetDistrictItems(stateCode, OfficeClass.USHouse)
          .Select(i =>
          {
            i.Value = i.Value.Substring(1);
            return i;
          })
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetCongressionalDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetCounties(string stateCode)
    {
      try
      {
        return CountyCache.GetCountiesByState(stateCode)
          .Select(
            countyCode =>
              new SimpleListItem
              {
                Text = Counties.GetName(stateCode, countyCode),
                Value = countyCode
              })
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetCounties", e);
        throw;
      }
    }

    [WebMethod]
    public Report GetCountiesReport(string stateCode)
    {
      return new Report
      {
        Html = CountiesReport.GetReport(stateCode).RenderToString()
      };
    }

    [WebMethod]
    public string GetElectionControlHtml(string stateCode, string countyCode,
      string localCode)
    {
      try
      {
        return ElectionControl.GenerateHtml(false, stateCode,
          countyCode.SafeString(), localCode.SafeString());
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionControlHtml", e);
        throw;
      }
    }

    [WebMethod]
    public Report GetElectionReport(string electionKey)
    {
      return new Report
      {
        Title = Elections.GetElectionDesc(electionKey),
        //Html = ElectionReport.GetReport(ReportUser.Master, electionKey)
        //  .RenderToString()
        Html = ElectionReportResponsive.GetReport(ReportUser.Master, electionKey)
          .RenderToString()
      };
    }

    [WebMethod]
    public SimpleListItem[] GetElections(string stateCode, string countyCode,
      string localCode)
    {
      try
      {
        return
          BulkEmailPage.GetPreviewElectionItems(stateCode, countyCode, localCode)
            .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElections", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public EmailBatch GetEmailBatch(int batchId, string contactType,
      string[] stateCodes, string[] countyCodes, string[] localCodes,
      string[] partyKeys, bool majorParties, bool useBothContacts,
      string[] politicianEmailsToUse, string electionFiltering,
      string[] electionFilterTypes, bool useFutureElections, bool viewableOnly,
      string electionKey, bool includeAllElectionsOnDate,
      TempEmailBatches.VisitorBatchOptions visitorOptions, 
      TempEmailBatches.DonorBatchOptions donorOptions)
    {
      try
      {
        var duplicates = 0;
        switch (contactType)
        {
          case "All":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateAllContactsBatch(batchId,
                  stateCodes, useBothContacts);
                break;

              case "HasType":
                duplicates =
                  TempEmailBatches.CreateAllContactsBatchWithElectionTypes(
                    batchId, stateCodes, useBothContacts, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;

              case "HasntType":
                duplicates =
                  TempEmailBatches.CreateAllContactsBatchWithoutElectionTypes(
                    batchId, stateCodes, useBothContacts, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;
            }
            break;

          case "State":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateStatesContactsBatch(batchId,
                  stateCodes, useBothContacts);
                break;

              case "HasType":
                duplicates =
                  TempEmailBatches.CreateStatesContactsBatchWithElectionTypes(
                    batchId, stateCodes, useBothContacts, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;

              case "HasntType":
                duplicates =
                  TempEmailBatches.CreateStatesContactsBatchWithoutElectionTypes(
                    batchId, stateCodes, useBothContacts, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;
            }
            break;

          case "County":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            if (countyCodes.Length < 1) throw new VoteException("Missing CountyCode");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateCountiesContactsBatch(batchId,
                  stateCodes, countyCodes, useBothContacts);
                break;

              case "HasType":
                duplicates =
                  TempEmailBatches.CreateCountiesContactsBatchWithElectionTypes(
                    batchId, stateCodes, countyCodes, useBothContacts,
                    electionFilterTypes, useFutureElections, viewableOnly);
                break;

              case "HasntType":
                duplicates =
                  TempEmailBatches.CreateCountiesContactsBatchWithoutElectionTypes(
                    batchId, stateCodes, countyCodes, useBothContacts,
                    electionFilterTypes, useFutureElections, viewableOnly);
                break;
            }
            break;

          case "Local":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            if (countyCodes.Length < 1) throw new VoteException("Missing CountyCode");
            if (localCodes.Length < 1) throw new VoteException("Missing LocalCode");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateLocalsContactsBatch(batchId,
                  stateCodes, countyCodes, countyCodes, useBothContacts);
                break;

              case "HasType":
                duplicates =
                  TempEmailBatches.CreateLocalsContactsBatchWithElectionTypes(
                    batchId, stateCodes, countyCodes, localCodes, useBothContacts,
                    electionFilterTypes, useFutureElections, viewableOnly);
                break;

              case "HasntType":
                TempEmailBatches.CreateLocalsContactsBatchWithoutElectionTypes(
                  batchId, stateCodes, countyCodes, localCodes, useBothContacts,
                  electionFilterTypes, useFutureElections, viewableOnly);
                break;
            }
            break;

          case "AllCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates =
                  TempEmailBatches.CreateAllCandidatesBatchWithElectionTypes(
                    batchId, stateCodes, politicianEmailsToUse, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;

              case "Single":
                duplicates =
                  TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                    politicianEmailsToUse, electionKey, includeAllElectionsOnDate, true);
                break;
            }
            break;

          case "StateCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates =
                  TempEmailBatches.CreateStatesCandidatesBatchWithElectionTypes(
                    batchId, stateCodes, politicianEmailsToUse, electionFilterTypes,
                    useFutureElections, viewableOnly);
                break;

              case "Single":
                duplicates =
                  TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                    politicianEmailsToUse, electionKey, includeAllElectionsOnDate);
                break;

              case "NoFiltering":
                duplicates =
                  TempEmailBatches.CreateStateCandidatesBatchWithNoElectionFiltering(
                    batchId, stateCodes, politicianEmailsToUse);
                break;
            }
            break;

          case "CountyCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates =
                  TempEmailBatches.CreateCountiesCandidatesBatchWithElectionTypes(
                    batchId, stateCodes, countyCodes, politicianEmailsToUse,
                    electionFilterTypes, useFutureElections, viewableOnly);
                break;

              case "Single":
                duplicates =
                  TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                    politicianEmailsToUse, electionKey, includeAllElectionsOnDate);
                break;
            }
            break;

          case "LocalCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates =
                  TempEmailBatches.CreateLocalsCandidatesBatchWithElectionTypes(
                    batchId, stateCodes, countyCodes, localCodes,
                    politicianEmailsToUse, electionFilterTypes, useFutureElections,
                    viewableOnly);
                break;

              case "Single":
                duplicates =
                  TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                    politicianEmailsToUse, electionKey, includeAllElectionsOnDate);
                break;
            }
            break;

          case "PartyOfficials":
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates =
                  TempEmailBatches.CreateStatesPartiesBatchByState(batchId,
                    stateCodes, partyKeys, majorParties);
                break;

              case "Single":
                duplicates =
                  TempEmailBatches.CreateStatesPartiesBatchByElection(batchId,
                    electionKey, includeAllElectionsOnDate);
                break;
            }
            break;

          case "Volunteers":
            duplicates =
              TempEmailBatches.CreateVolunteersBatch(batchId,
                stateCodes, partyKeys, majorParties);
            break;

          case "WebsiteVisitors":
            duplicates = TempEmailBatches.CreateVisitorsBatch(batchId, stateCodes,
              countyCodes, visitorOptions);
            break;

          case "Donors":
            //visitorOptions.ForSampleBallots = false;
            duplicates = TempEmailBatches.CreateDonorsBatch(batchId, stateCodes,
              countyCodes, visitorOptions, donorOptions);
            break;
        }

        var table = TempEmail.GetDataByBatchId(batchId);
        var names = LocalDistricts.GetFocusedNamesDictionary(table);
        return new EmailBatch
        {
          BatchId = batchId,
          Duplicates = duplicates,
          Items =
            table.Select(
              row =>
                new EmailItem
                {
                  Id = row.Id,
                  Email = row.Email,
                  Contact = row.Contact,
                  Title = row.Title,
                  Phone = row.Phone,
                  StateCode = row.StateCode,
                  CountyCode = row.CountyCode,
                  LocalCode = row.LocalCode,
                  Jurisdiction =
                    (string.IsNullOrWhiteSpace(row.LocalCode)
                      ? string.Empty
                      : names[
                        row.StateCode + "|" + row.CountyCode + "|" + row.LocalCode] +
                        ", ") + Counties.GetFullName(row.StateCode, row.CountyCode),
                  PoliticianKey = row.PoliticianKey,
                  ElectionKey = row.ElectionKey,
                  ElectionKeyToInclude = row.ElectionKeyToInclude,
                  OfficeKey = row.OfficeKey,
                  PartyKey = row.PartyKey,
                  PartyEmail = row.PartyEmail,
                  VisitorId = row.VisitorId ?? 0,
                  DonorId = row.DonorId ?? 0,
                  SortName = row.SortName
                })
              .OrderBy(i => StateCache.GetStateName(i.StateCode))
              .ThenBy(i => i.CountyCode ?? string.Empty)
              .ThenBy(i => i.LocalCode ?? string.Empty)
              .ThenBy(i => i.PoliticianKey ?? string.Empty)
              .ThenBy(i => i.PartyKey ?? string.Empty)
              .ToArray()
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailBatch", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public int GetEmailBatchId()
    {
      try
      {
        return TempEmailBatches.Insert(DateTime.UtcNow, VotePage.UserName,
          VotePage.DefaultDbDate, 0, 0);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailBatchId", e);
        throw;
      }
    }

    [WebMethod]
    public int GetEmailBatchStatus(int batchId)
    {
      try
      {
        return TempEmail.CountByBatchId(batchId);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailBatchStatus", e);
        throw;
      }
    }

    [WebMethod]
    public LogEmailBatchItem[] GetEmailLogBatches(string beginDate, string endDate,
      string success, string[] froms, string[] users, string[] searchStrings,
      string joinOption)
    {
      try
      {
        var beginTime = string.IsNullOrWhiteSpace(beginDate)
          ? DateTime.MinValue
          : DateTime.Parse(beginDate);
        var endTime = string.IsNullOrWhiteSpace(endDate)
          ? DateTime.MaxValue
          : DateTime.Parse(endDate) + new TimeSpan(1, 0, 0, 0); // add a day
        var reportSuccess = success != "failed";
        var reportFailure = success != "sent";

        return
          LogEmailBatches.GetDataForSearchCriteria(beginTime, endTime,
            reportSuccess, reportFailure, froms, users, searchStrings, joinOption)
            .Select(
              row =>
                new LogEmailBatchItem
                {
                  Id = row.Id,
                  CreationTime = row.CreationTime.AsUtc(),
                  SelectionCriteria = row.SelectionCriteria,
                  Description = row.Description,
                  Found = row.Found,
                  Skipped = row.Skipped,
                  Sent = row.Sent,
                  Failed = row.Failed,
                  UserName = row.UserName,
                  FromEmail = row.FromEmail,
                  CcEmails =
                    row.CcEmails.Split(new[] {','},
                      StringSplitOptions.RemoveEmptyEntries),
                  BccEmails =
                    row.BccEmails.Split(new[] {','},
                      StringSplitOptions.RemoveEmptyEntries)
                })
            .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailLogBatches", e);
        throw;
      }
    }

    [WebMethod]
    public LoggedEmailNote[] GetEmailNotes(int id)
    {
      try
      {
        return GetEmailNotesPrivate(id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailNotes", e);
        throw;
      }
    }

    [WebMethod]
    public EmailSendStatus GetEmailSendStatus(int batchId)
    {
      try
      {
        var table = TempEmailBatches.GetDataById(batchId);
        return new EmailSendStatus {Sent = table[0].Sent, Failed = table[0].Failed};
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailSendStatus", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetEmailStarterDocuments()
    {
      return EmailTemplates.GetStarterDocuments();
    }

    [WebMethod(EnableSession = true)]
    public EmailTemplateItem[] GetEmailTemplateInfo(bool allPublic)
    {
      try
      {
        var ownerType = EmailTemplates.GetOwnerType();
        var owner = VotePage.UserName;
        return EmailTemplates.GetAvailableTemplateData(ownerType, owner, allPublic)
          .Select(
            row =>
              new EmailTemplateItem
              {
                Id = row.Id,
                Name = row.Name,
                Owner = row.Owner,
                OwnerType = ownerType,
                IsOwner = row.Owner.IsEqIgnoreCase(owner),
                IsPublic = row.IsPublic,
                Requirements = row.Requirements,
                CreateTime = row.CreateTime.AsUtc(),
                ModTime = row.ModTime.AsUtc(),
                LastUsedTime = row.LastUsedTime.AsUtc()
              })
          .OrderByDescending(i => i.IsOwner)
          .ThenBy(i => i.Owner, StringComparer.InvariantCultureIgnoreCase)
          .ThenBy(i => i.Name, StringComparer.InvariantCultureIgnoreCase)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailTemplateInfo", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public string[] GetEmailTemplateNames()
    {
      try
      {
        var ownerType = EmailTemplates.GetOwnerType();
        var owner = VotePage.UserName;
        return EmailTemplates.GetAvailableTemplateData(ownerType, owner, true)
          .Select(row => row.Name)
          .OrderBy(name => name, StringComparer.InvariantCultureIgnoreCase)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailTemplateNames", e);
        throw;
      }
    }

    [WebMethod]
    public string GetGeneralElectionDescription(string input)
    {
      try
      {
        DateTime date;
        if (DateTime.TryParse(input.Trim(), out date)) return Elections.GetGeneralElectionDescriptionTemplate(date.Date);
      }
        // ReSharper disable once EmptyStatement
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetGeneralElectionDescription", e);
      }
      // no fail
      return string.Empty;
    }

    [WebMethod]
    public string[] GetLocalNameAutosuggest(string term, string stateCode, string countyCode)
    {
      Debug.WriteLine("Called");
      return LocalDistricts.GetAutosuggest(term, stateCode, countyCode);
    }

    [WebMethod]
    public SimpleListItem[] GetLocals(string stateCode, string countyCode)
    {
      try
      {
        return LocalDistricts.GetNamesDictionary(stateCode, countyCode)
          .OrderBy(kvp => kvp.Value)
          .Select(kvp => new SimpleListItem {Text = kvp.Value, Value = kvp.Key})
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetLocals", e);
        throw;
      }
    }

    [WebMethod]
    public LoggedEmailDetail GetLoggedEmailDetail(int id)
    {
      try
      {
        var row = LogEmailBatches.GetLoggedEmailDetail(id);
        if (row == null) return null;
        return new LoggedEmailDetail
        {
          Id = row.Id(),
          Body = row.Body(),
          CcEmails = row.CcEmails(),
          BccEmails = row.BccEmails(),
          BatchDescription = row.Description(),
          SelectionCriteria = row.SelectionCriteria()
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetLoggedEmailDetail", e);
        throw;
      }
    }

    [WebMethod]
    public LoggedEmailItem[] GetLoggedEmails(string[] contactTypes,
      string jurisdictionLevel, string[] stateCodes, string[] countyCodes,
      string[] localCodes, string beginDate, string endDate, string success,
      string flagged, int maximumResults, string[] froms, string[] tos,
      string[] users, string[] electionKeys, string[] officeKeys,
      string[] candidateKeys, string[] politicianKeys, int[] batchIds)
    {
      try
      {
        var beginTime = string.IsNullOrWhiteSpace(beginDate)
          ? DateTime.MinValue
          : DateTime.Parse(beginDate);
        var endTime = string.IsNullOrWhiteSpace(endDate)
          ? DateTime.MaxValue
          : DateTime.Parse(endDate) + new TimeSpan(1, 0, 0, 0); // add a day
        var reportSuccess = success != "failed";
        var reportFailure = success != "sent";
        var reportFlagged = flagged != "no";
        var reportUnflagged = flagged != "yes";

        var table = LogEmailBatches.GetLoggedEmails(contactTypes, jurisdictionLevel,
          stateCodes, countyCodes, localCodes, beginTime, endTime, reportSuccess,
          reportFailure, reportFlagged, reportUnflagged, maximumResults, froms, tos,
          users, electionKeys, officeKeys, candidateKeys, politicianKeys, batchIds, 0);

        return table.Rows.Cast<DataRow>()
          .Select(
            row => new LoggedEmailItem
            {
              Id = row.Id(),
              SentTime = row.SentTime()
                .AsUtc(),
              WasSent = row.WasSent(),
              ContactType = row.ContactType(),
              StateCode = row.StateCode(),
              CountyCode = row.CountyCode(),
              LocalCode = row.LocalCode(),
              ElectionKey = row.ElectionKey(),
              OfficeKey = row.OfficeKey(),
              PoliticianKey = row.PoliticianKey(),
              Contact = row.Contact(),
              FromEmail = row.FromEmail(),
              ToEmail = row.ToEmail(),
              Subject = row.Subject(),
              UserName = row.UserName(),
              Jurisdiction =
                (string.IsNullOrWhiteSpace(row.LocalDistrict())
                  ? string.Empty
                  : row.LocalDistrict() + ", ") +
                  Counties.GetFullName(row.StateCode(), row.CountyCode()),
              ElectionDescription = row.ElectionDescription()
                .SafeString(),
              OfficeName = Offices.FormatOfficeName(row),
              PoliticianName = Politicians.FormatName(row),
              PartyCode = row.PartyCode()
                .SafeString(),
              SortName = TempEmailBatches.ConcoctSortName(row.Contact()),
              ForwardedCount = row.ForwardedCount(),
              IsFlagged = row.IsFlagged(),
              ErrorMessage = row.ErrorMessage()
            })
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetLoggedEmails", e);
        throw;
      }
    }

    [WebMethod]
    public MoreRecipientInfo GetMoreRecipientInfo(string electionKey,
      string officeKey, string partyKey, int visitorId)
    {
      try
      {
        var cache = PageCache.GetTemporary();
        var info = new MoreRecipientInfo();
        if (!string.IsNullOrWhiteSpace(electionKey)) info.Election = cache.Elections.GetElectionDesc(electionKey);
        if (!string.IsNullOrWhiteSpace(officeKey)) info.Office = Offices.FormatOfficeName(cache, officeKey);
        if (!string.IsNullOrWhiteSpace(partyKey)) info.Party = cache.Parties.GetPartyName(partyKey);
        if (visitorId != 0)
        {
          var table = Addresses.GetDataById(visitorId);
          if (table.Count == 1)
          {
            var row = table[0];
            info.DateAdded = row.DateStamp.AsUtc();
            info.Address = row.Address;
            var cityStateZip = string.Empty;
            if (!string.IsNullOrWhiteSpace(row.City)) cityStateZip = row.City + ", " + row.StateCode + " ";
            if (!string.IsNullOrWhiteSpace(row.Zip5))
            {
              cityStateZip += row.Zip5;
              if (!string.IsNullOrWhiteSpace(row.Zip4)) cityStateZip += "-" + row.Zip4;
            }
            info.CityStateZip = cityStateZip;
            if (!string.IsNullOrWhiteSpace(row.CongressionalDistrict) &&
              row.CongressionalDistrict != "00")
            {
              var officeTable =
                Offices.GetDataByStateCodeOfficeLevelDistrictCode(row.StateCode,
                  OfficeClass.USHouse.ToInt(), row.CongressionalDistrict.ZeroPad(3));
              if (officeTable.Count == 1)
                info.CongressionalDistrict =
                  Offices.FormatOfficeName(officeTable[0]);
            }
            if (!string.IsNullOrWhiteSpace(row.StateSenateDistrict) &&
              row.StateSenateDistrict != "000")
            {
              var officeTable =
                Offices.GetDataByStateCodeOfficeLevelDistrictCode(row.StateCode,
                  OfficeClass.StateSenate.ToInt(), row.StateSenateDistrict);
              if (officeTable.Count == 1) info.StateSenateDistrict = Offices.FormatOfficeName(officeTable[0]);
            }
            if (!string.IsNullOrWhiteSpace(row.StateHouseDistrict) &&
              row.StateHouseDistrict != "000")
            {
              var officeTable =
                Offices.GetDataByStateCodeOfficeLevelDistrictCode(row.StateCode,
                  OfficeClass.StateHouse.ToInt(), row.StateHouseDistrict);
              if (officeTable.Count == 1) info.StateHouseDistrict = Offices.FormatOfficeName(officeTable[0]);
            }
          }
        }
        return info;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetMoreRecipientInfo", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetOffices(string electionKey)
    {
      try
      {
        return BulkEmailPage.GetPreviewOfficeItems(electionKey)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetOffices", e);
        throw;
      }
    }

    [WebMethod]
    public string[] GetOfficesByClass(string stateCode, string countyCode, string localCode,
      string officeClass)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      return Offices.GetOfficesByClass(stateCode, countyCode, localCode, parsedOfficeClass)
        .Select(r => Offices.FormatOfficeName(r))
        .ToArray();
    }

    [WebMethod]
    public SimpleListItem[] GetOfficesInClass(string stateCode, string countyCode, string localCode,
      int officeLevel)
    {
      try
      {
        var options = new OfficesAdminReportViewOptions
        {
          StateCode = stateCode,
          CountyCode = countyCode,
          LocalCode = localCode,
          OfficeClass = officeLevel.ToOfficeClass(),
          Option = string.IsNullOrWhiteSpace(countyCode)
           ? OfficesAdminReportViewOption.ByState
           : string.IsNullOrWhiteSpace(localCode)
             ? OfficesAdminReportViewOption.ByCounty
             : OfficesAdminReportViewOption.ByLocal
        };
        var table = OfficesAdminReportView.GetData(options).ToList();
        table.Sort(OfficesAdminReport.CompareOfficesAdminReportViewRows);
        return table.Select(r => new SimpleListItem
        {
          Text = Offices.FormatOfficeName(r.OfficeLine1, r.OfficeLine2, r.OfficeKey),
          Value = r.OfficeKey
        }).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetOfficesInClass", e);
        throw;
      }
    }

    [WebMethod]
    public string[] GetOfficeTemplatesByClass(string stateCode, string officeClass)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      return Offices.GetOfficeTemplatesByClass(stateCode, parsedOfficeClass)
        .Select(r => Offices.FormatOfficeName(r))
        .ToArray();
    }

    [WebMethod]
    public Report GetOfficialsReport(string stateCode)
    {
      return new Report
      {
        Html = OfficialsReport.GetReport(ReportUser.Master, stateCode).RenderToString()
      };
    }

    [WebMethod]
    public SimpleListItem[] GetParties(string stateCode)
    {
      try
      {
        return BulkEmailPage.GetPreviewPartyItems(stateCode)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetParties", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetPartyEmails(string partyKey)
    {
      try
      {
        return BulkEmailPage.GetPreviewPartyEmailItems(partyKey)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetPartyEmails", e);
        throw;
      }
    }

    [WebMethod]
    public ListItem[] GetPartyPrimariesWithOffices(string stateCode,
      string electionDate)
    {
      try
      {
        DateTime date;
        if (DateTime.TryParse(electionDate.Trim(), out date))
          return Elections.GetPartyPrimariesWithOffices(stateCode, date.Date)
            .Select(
              row =>
                new ListItem {Text = row.ElectionDesc, Value = row.ElectionKey})
            .ToArray();
      }
        // ReSharper disable once EmptyStatement
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetPartyPrimariesWithOffices", e);
      }
      // no fail
      return new ListItem[0];
    }

    [WebMethod]
    public RestoreInfo GetRestoreInfo(string stateCode, string countyCode,
      string localCode, string districtFiltering, bool getCounties, bool getLocals,
      bool getParties, bool getElections)
    {
      var result = new RestoreInfo
      {
        StateCode = stateCode,
        CountyCode = countyCode,
        LocalCode = localCode
      };
      if (getCounties) result.Counties = GetCounties(stateCode);
      if (getLocals) result.Locals = GetLocals(stateCode, countyCode);
      // skip <none>
      if (getParties)
        result.Parties = GetParties(stateCode)
          .Skip(1)
          .ToArray();
      switch (districtFiltering)
      {
        case "CongressionalDistrict":
          result.Districts = GetCongressionalDistricts(stateCode);
          break;

        case "StateSenateDistrict":
          result.Districts = GetStateSenateDistricts(stateCode);
          break;

        case "StateHouseDistrict":
          result.Districts = GetStateHouseDistricts(stateCode);
          break;
      }
      if (getElections)
        result.ElectionControlHtml = GetElectionControlHtml(stateCode, countyCode,
          localCode);
      return result;
    }

    [WebMethod(EnableSession = true)]
    public GetRunningMateHtmlResult GetRunningMateHtml(string electionKey,
      string politicianKey, string mainCandidateKey, bool openEditDialog, string mode)
    {
      ManagePoliticiansPanel.DataMode theMode;
      if (!Enum.TryParse(mode, out theMode))
        theMode = ManagePoliticiansPanel.DataMode.ManageCandidates;
      try
      {
        return new GetRunningMateHtmlResult
        {
          PoliticianKey = politicianKey,
          MainCandidateKey = mainCandidateKey,
          OpenEditDialog = openEditDialog,
          RunningMateHtml =
            ManagePoliticiansPanel.GetRunningMateHtml(electionKey, politicianKey,
              mainCandidateKey, theMode)
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetRunningMateHtml", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetStateHouseDistricts(string stateCode)
    {
      try
      {
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateHouse)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetStateHouseDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetStateSenateDistricts(string stateCode)
    {
      try
      {
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateSenate)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetStateSenateDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public BallotPediaCsv[] GetUploadedBallotPediaCsvs(bool all)
    {
      try
      {
        var table = all
          ? BallotPediaCsvs.GetAllNoContentData()
          : BallotPediaCsvs.GetNoContentDataByCompleted(false);
        return
          table.Select(
            r =>
              new BallotPediaCsv
              {
                Id = r.Id,
                Filename = r.Filename,
                UploadTime = r.UploadTime,
                Candidates = r.Candidates,
                CandidatesCoded = r.CandidatesCoded,
                Completed = r.Completed
              })
            .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetUploadedBallotPediaCsvs", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public UserData[] GetUserData()
    {
      if (!SecurePage.IsSuperUser) return new UserData[0];
      return Security.GetUserDataForUpdate().Select(r => new UserData
      {
        UserName = r.UserName(),
        Password = r.UserPassword(),
        UserLevel = r.UserSecurity(),
        StateCode = r.UserStateCode(),
        CountyCode = r.UserCountyCode(),
        LocalCode = r.UserLocalCode(),
        CountyName = r.County().SafeString(),
        LocalName = r.LocalDistrict().SafeString()
      }).ToArray();
    }

    [WebMethod]
    public VolunteerNotes GetVolunteerNotes(string email)
    {
      return VolunteersView.GetDataByEmail(email)
        .Select(v => new VolunteerNotes
      {
        Email = v.Email,
        FirstName = v.FirstName,
        LastName = v.LastName,
        Phone = v.Phone,
        PartyName = Parties.GetNationalPartyDescription(Parties.GetPartyCode(v.PartyKey)),
        StateName = v.StateName,
        Notes = GetVolunteerNotesArray(email)
      }).FirstOrDefault();
    }

    [WebMethod]
    public Volunteer[] GetVolunteerReport(string stateCode, string partyKey)
    {
      return VolunteersView.GetAllDataSorted(stateCode, partyKey)
        .Select(r => new Volunteer
        {
          Email = r.Email,
          Password = r.Password,
          PartyKey = r.PartyKey,
          FirstName = r.FirstName,
          LastName = r.LastName,
          Phone = r.Phone,
          PartyName = Parties.GetNationalPartyDescription(Parties.GetPartyCode(r.PartyKey)),
          DateStamp = r.DateStamp,
          StateCode = r.StateCode,
          StateName = r.StateName
        })
        .ToArray();
    }

    [WebMethod]
    public YouTubeInfo GetYouTubeVideoChannel(string politicianKey)
    {
      var id = Politicians.GetYouTubeWebAddress(politicianKey).GetYouTubeVideoId();
      var result = YouTubeUtility.GetVideoChannelInfo(id, true);
      return result;
    }

    [WebMethod(EnableSession = true)]
    public EmailTemplateData OpenEmailTemplate(int id)
    {
      try
      {
        var ownerType = EmailTemplates.GetOwnerType();
        var owner = VotePage.UserName;
        var table = EmailTemplates.GetDataById(id);
        if (table.Count != 1) return null;
        var row = table[0];
        return new EmailTemplateData
        {
          Id = row.Id,
          Name = row.Name,
          Owner = row.Owner,
          OwnerType = row.OwnerType,
          IsPublic = row.IsPublic,
          CreateTime = row.CreateTime.AsUtc(),
          ModTime = row.ModTime.AsUtc(),
          Subject = row.Subject,
          Body = row.Body,
          SelectRecipientOptions = row.SelectRecipientOptions,
          EmailOptions = row.EmailOptions,
          IsOwner = row.OwnerType == ownerType && row.Owner.IsEqIgnoreCase(owner)
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/OpenEmailTemplate", e);
        throw;
      }
    }

    [WebMethod]
    public string RenameBallotpediaCsv(int id, string newName)
    {
      try
      {
        if (BallotPediaCsvs.FilenameExists(newName)) 
          return "There is already a CSV with the name " + newName;
        BallotPediaCsvs.UpdateFilenameById(newName, id);
        return string.Empty;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/RenameBallotpediaCsv", e);
        throw;
      }
    }

    [WebMethod]
    public string RenameEmailTemplate(int id, string newName)
    {
      try
      {
        EmailTemplates.UpdateNameById(newName, id);
        return newName;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/RenameEmailTemplate", e);
        throw;
      }
    }

    [WebMethod]
    public void ReverseDonation(string email, string donation, bool optOut)
    {
      var dateString = donation.Substring(0, donation.LastIndexOf(" ", StringComparison.Ordinal));
      var date = DateTime.Parse(dateString);
      Donations.DeleteByEmailDate(email, date);
      if (optOut)
        EmailUtility.UpdateSubscription(email, "unsubscribe");
    }

    [WebMethod(EnableSession = true)]
    public string SaveEmailTemplate(string name, string subject, string body,
      string followAction)
    {
      try
      {
        var table = EmailTemplates.GetDataByNameOwnerTypeOwner(name,
          EmailTemplates.GetOwnerType(), VotePage.UserName);
        if (table.Count == 1)
        {
          var row = table[0];
          row.Subject = subject;
          row.Body = body;
          row.Requirements = EmailTemplates.GetTemplateRequirementsString(subject,
            body);
          row.ModTime = DateTime.UtcNow;
          EmailTemplates.UpdateTable(table);
        }
        return followAction;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SaveEmailTemplate", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public void SaveEmailTemplateAs(string name, bool isPublic, string subject,
      string body, bool isNew)
    {
      try
      {
        var now = DateTime.UtcNow;
        EmailTemplates.Upsert(name, EmailTemplates.GetOwnerType(),
          VotePage.UserName, isPublic, now, now, subject, body, isNew);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SaveEmailTemplateAs", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public void SaveEmailTemplateOptions(string name, string selectRecipientOptions,
      string emailOptions)
    {
      var ownerType = EmailTemplates.GetOwnerType();
      EmailTemplates.UpdateSelectRecipientOptionsByNameOwnerTypeOwner(
        selectRecipientOptions, name, ownerType, VotePage.UserName);
      EmailTemplates.UpdateEmailOptionsByNameOwnerTypeOwner(emailOptions, name,
        ownerType, VotePage.UserName);
    }

    [WebMethod]
    public VolunteerNote[] SaveVolunteerNote(string email, int id, string note)
    {
      VolunteersNotes.UpdateNotesById(HttpUtility.HtmlDecode(note.ReplaceBreakTagsWithNewLines()), id);
      return GetVolunteerNotesArray(email);
    }

    [WebMethod(EnableSession = true)]
    public BatchEmailSummary SendEmailBatch(string contactType,
      string subjectTemplate, string bodyTemplate, string from, string[] cc,
      string[] bcc, int batchId, int found, int[] skip, string selectionCriteria,
      string description)
    {
      var summary = new BatchEmailSummary { Description = description };
      var logBatchId = -1;
      try
      {
        // reset the batch stats
        TempEmailBatches.UpdateFailedById(0, batchId);
        TempEmailBatches.UpdateSentById(0, batchId);

        // create a logging batch
        logBatchId = LogEmailBatches.Insert(DateTime.UtcNow, contactType,
          selectionCriteria, description, found, skip.Length, 0, 0,
          VotePage.UserName, from, string.Join(",", cc), string.Join(",", bcc));

        TempEmailBatches.UpdateTimeLastSentById(DateTime.UtcNow, batchId);
        foreach (var row in TempEmail.GetDataByBatchId(batchId))
          if (!skip.Contains(row.Id))
          {
            summary.Total++;
            try
            {
              EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, from,
                new[]
                {
                  row.Email
                }, cc, bcc, row.StateCode.SafeString(),
                row.CountyCode.SafeString(), row.LocalCode.SafeString(),
                row.Contact.SafeString(), row.Email.SafeString(),
                row.Title.SafeString(), row.Phone.SafeString(),
                row.ElectionKey.SafeString(), row.OfficeKey.SafeString(),
                row.PoliticianKey.SafeString(), row.PartyKey.SafeString(),
                row.PartyEmail.SafeString(), row.VisitorId ?? 0,
                row.DonorId ?? 0, logBatchId);
              summary.Sent++;
              TempEmailBatches.UpdateSentById(summary.Sent, batchId);
            }
            catch (Exception e)
            {
              summary.Failed++;
              TempEmailBatches.UpdateFailedById(summary.Failed, batchId);
              summary.Failures.Add(new EmailFailure
              {
                Contact = row.Contact,
                SortContact = row.SortName,
                ToAddresses = row.Email,
                Message = e.Message
              });
            }
          }

        // mark the batch stats complete
        TempEmailBatches.UpdateFailedById(-1, batchId);
        TempEmailBatches.UpdateSentById(-1, batchId);

        return summary;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SendEmailBatch", e);
        throw;
      }
      finally
      {
        // update the log batch
        if (logBatchId != -1)
        {
          LogEmailBatches.UpdateSentById(summary.Sent, logBatchId);
          LogEmailBatches.UpdateFailedById(summary.Failed, logBatchId);
        }
      }
    }

    [WebMethod]
    public string SendSingleTestEmail(string subjectTemplate, string bodyTemplate,
      string from, string[] to, string stateCode, string countyCode,
      string localCode, string contact, string email, string title, string phone,
      string electionKey, string officeKey, string politicianKey, string partyKey,
      string partyEmail, int visitorId, int donorId)
    {
      try
      {
        EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, from, to, null,
          null, stateCode, countyCode, localCode, contact, email, title, phone,
          electionKey, officeKey, politicianKey, partyKey, partyEmail, visitorId, donorId,
          0);
      }
      catch (VoteSubstitutionException e)
      {
        return e.Message;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SendSingleTestEmail", e);
        throw;
      }

      return null;
    }

    [WebMethod]
    public BatchEmailSummary SendTestEmailBatch(string subjectTemplate,
      string bodyTemplate, string from, string[] to, int batchId, int[] skip)
    {
      try
      {
        var summary = new BatchEmailSummary {Description = "Test Email Batch"};

        TempEmailBatches.UpdateTimeLastSentById(DateTime.UtcNow, batchId);
        foreach (var row in TempEmail.GetDataByBatchId(batchId))
          if (!skip.Contains(row.Id))
          {
            summary.Total++;
            try
            {
              EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, from, to,
                null, null, row.StateCode.SafeString(), row.CountyCode.SafeString(),
                row.LocalCode.SafeString(), row.Contact.SafeString(),
                row.Email.SafeString(), row.Title.SafeString(),
                row.Phone.SafeString(), row.ElectionKey.SafeString(),
                row.OfficeKey.SafeString(), row.PoliticianKey.SafeString(),
                row.PartyKey.SafeString(), row.PartyEmail.SafeString(),
                row.VisitorId ?? 0, row.DonorId ?? 0, 0);
              summary.Sent++;
              TempEmailBatches.UpdateSentById(summary.Sent, batchId);
            }
            catch (Exception ex)
            {
              summary.Failed++;
              TempEmailBatches.UpdateFailedById(summary.Failed, batchId);
              summary.Failures.Add(new EmailFailure
              {
                Contact = row.Contact,
                SortContact = row.SortName,
                ToAddresses = string.Join(",", to),
                Message = ex.Message
              });
            }
          }
        return summary;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SendTestEmailBatch", e);
        throw;
      }
    }

    [WebMethod]
    public void SetEmailTemplatePublicFlag(int id, bool isPublic)
    {
      try
      {
        EmailTemplates.UpdateIsPublicById(isPublic, id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SetEmailTemplatePublicFlag", e);
        throw;
      }
    }

    [WebMethod]
    public void SetLoggedEmailFlag(int id, bool isFlagged)
    {
      try
      {
        LogEmail.UpdateIsFlaggedById(isFlagged, id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SetLoggedEmailFlag", e);
        throw;
      }
    }

    [WebMethod]
    public EmailSubstitution SubtituteEmailTemplate(string subjectTemplate,
      string bodyTemplate, string email, string contact, string title, string phone,
      string stateCode, string countyCode, string localCode, string politicianKey,
      string electionKey, string officeKey, string partyKey, string partyEmail,
      int visitorId, int donorId)
    {
      try
      {
        var substitution = EmailTemplates.GetSubstititionsForEmail(stateCode,
          countyCode, localCode, contact, email, title, phone, electionKey,
          officeKey, politicianKey, partyKey, partyEmail, visitorId, donorId);
        return new EmailSubstitution
        {
          Subject = substitution.Substitute(subjectTemplate),
          Body = substitution.Substitute(bodyTemplate)
        };
      }
      catch (VoteSubstitutionException ex)
      {
        return new EmailSubstitution {ErrorMessage = ex.Message};
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/SubtituteEmailTemplate", e);
        throw;
      }
    }

    [WebMethod]
    public string[] ToTitleCase(string[] input)
    {
      for (var inx = 0; inx < input.Length; inx++)
        input[inx] = input[inx].SafeString().Trim().ToTitleCase();
      return input;
    }

    [WebMethod(EnableSession = true)]
    public UpdateBallotPediaCsvInfo UpdateBallotPediaCsv(int csvId, bool isComplete, SimpleListItem[]
      candidateIds)
    {
      var info = new UpdateBallotPediaCsvInfo();

      var invalidIds = new List<string>();
      var candidateIdDictionary = new Dictionary<int, string>();

      foreach (var item in candidateIds)
      {
        int index;
        if (int.TryParse(item.Value, out index) &&
          Politicians.PoliticianKeyExists(item.Text))
          candidateIdDictionary.Add(index, item.Text);
        else invalidIds.Add(item.Text);
      }

      try
      {
        if (candidateIdDictionary.Count > 0)
        {
          int coded;
          var content = BallotPediaCsvs.GetContentById(csvId);
          if (string.IsNullOrWhiteSpace(content))
            throw new VoteException("Missing CSV");
          content = UpdateCsv(content, candidateIdDictionary, out coded);
          BallotPediaCsvs.UpdateCandidatesCodedById(coded, csvId);
          BallotPediaCsvs.UpdateContentById(content, csvId);
        }
        BallotPediaCsvs.UpdateCompletedById(isComplete, csvId);
        info.Message = "The CSV was updated.";
        if (invalidIds.Count > 0)
          info.Message += "\n\nThe following invalid ids were not updated:\n" +
            string.Join(", ", invalidIds);
        info.Ok = true;
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/UpdateBalltopediaCsv", e);
        throw;
      }
      return info;
    }

    private static string UpdateCsv(string csv, IDictionary<int, string> dictionary,
      out int coded)
    {
      coded = 0;
      using (var csvReader = new CsvReader(new StringReader(csv), true))
      {
        var headers = csvReader.GetFieldHeaders();

        using (var ms = new MemoryStream())
        {
          var streamWriter = new StreamWriter(ms);
          var csvWriter = new SimpleCsvWriter();

          // write headers
          foreach (var col in headers)
            csvWriter.AddField(col);
          csvWriter.Write(streamWriter);

          var index = 0;
          while (csvReader.ReadNextRecord())
          {
            foreach (var col in headers)
              if (col == "VoteUSA Id")
              {
                var id = dictionary.ContainsKey(index)
                  ? dictionary[index] 
                  : csvReader[col];
                csvWriter.AddField(id);
                if (!string.IsNullOrWhiteSpace(id))  coded++;
              }
              else csvWriter.AddField(csvReader[col]);
            csvWriter.Write(streamWriter);
            index++;
          }
          streamWriter.Flush();
          ms.Position = 0;
          csv = new StreamReader(ms).ReadToEnd();
        }
      }
      return csv;
    }

    [WebMethod(EnableSession = true)]
    public void UpdateEmailTemplateLastUsed(string name)
    {
      try
      {
        EmailTemplates.UpdateLastUsedTimeByNameOwnerTypeOwner(DateTime.UtcNow, name,
          EmailTemplates.GetOwnerType(), VotePage.UserName);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/UpdateEmailTemplateLastUsed", e);
        throw;
      }
    }

    // ReSharper disable SuggestBaseTypeForParameter
    private static void ApplyInstructionalVideos(string type, IList list, IList all)
    // ReSharper restore SuggestBaseTypeForParameter
    {
      foreach (Dictionary<string, object> v in all)
        v[type] = 0;
      var order = 0;
      foreach (int i in list)
      {
        order += 10;
        var id = i;
        var v = all.Cast<Dictionary<string, object>>().First(o => (int) o["id"] == id);
        v[type] = order;
      }
    }

    [WebMethod]
    public void UpdateInstructionalVideos(string json)
    {
      // parse json
      var obj = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
      var all = obj["all"] as ArrayList;
      var admin = obj["admin"] as ArrayList;
      var volunteers = obj["volunteers"] as ArrayList;

      // apply admin and volunteers to all
      ApplyInstructionalVideos("admin", admin, all);
      ApplyInstructionalVideos("volunteers", volunteers, all);

      // get the current table
      var table = InstructionalVideos.GetAllData();

      // mark deletions
      // ReSharper disable once AssignNullToNotNullAttribute
      var deletions = table.Where(r => 
        all.Cast<Dictionary<string, object>>().All(v => (int) v["id"] != r.Id));
      foreach (var d in deletions)
        d.Delete();

      // process updates and inserts
      // ReSharper disable once PossibleNullReferenceException
      foreach (Dictionary<string, object> v in all)
      {
        var row = table.FirstOrDefault(r => 
          r.RowState != DataRowState.Deleted && (int) v["id"] == r.Id);
        if (row == null)
        {
          // new row
          table.AddRow((int) v["id"], v["title"] as string, v["description"] as string,
            v["embedcode"] as string, v["url"] as string, (int) v["admin"], 
            (int) v["volunteers"] );
        }
        else
        {
          // update row
          row.Title = v["title"] as string;
          row.Description = v["description"] as string;
          row.EmbedCode = v["embedcode"] as string;
          row.Url = v["url"] as string;
          row.AdminOrder = (int) v["admin"];
          row.VolunteersOrder = (int) v["volunteers"];
        }
      }
      InstructionalVideos.UpdateTable(table);
    }

    [WebMethod]
    public string VisitorLookupMissingDistricts()
    {
      var now = DateTime.UtcNow;
      var table = Addresses.GetAllUncodedDistrictCodingData();
      var recoded = 0;
      var bad = 0;
      foreach (var row in table)
      {
        var input = row.Address + " " + row.City + " " + row.StateCode + " " + row.Zip5;
        if (!string.IsNullOrWhiteSpace(row.Zip4)) input += "-" + row.Zip4;
        var result = AddressFinder.Find(input, null, false);
        row.DistrictLookupDate = now;
        if (result.Success)
        {
          row.CongressionalDistrict = result.Congress.Substring(1);
          row.StateSenateDistrict = result.StateSenate;
          row.StateHouseDistrict = result.StateHouse;
          row.County = result.County;
          recoded++;
          if (recoded % 1000 == 0)
            Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding);
        }
        else bad++;
      }
      Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding);
      AddressesMaster.UpdateLastLookupMissingDistricts(DateTime.UtcNow);
      return $"{recoded} districts updated, {bad} address lookups failed.";
    }

    [WebMethod]
    public string VisitorRefreshAllDistricts()
    {
      var now = DateTime.UtcNow;
      var table = Addresses.GetAllCodeableDistrictCodingData();
      var recoded = 0;
      var bad = 0;
      foreach (var row in table)
      {
        var input = row.Address + " " + row.City + " " + row.StateCode + " " + row.Zip5;
        if (!string.IsNullOrWhiteSpace(row.Zip4)) input += "-" + row.Zip4;
        var result = AddressFinder.Find(input, null);
        row.DistrictLookupDate = now;
        if (result.Success)
        {
          row.CongressionalDistrict = result.Congress.Substring(1);
          row.StateSenateDistrict = result.StateSenate;
          row.StateHouseDistrict = result.StateHouse;
          row.County = result.County;
          recoded++;
          if (recoded % 1000 == 0)
            Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding);
        }
        else bad++;
      }
      Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding);
      AddressesMaster.UpdateLastRefreshAllDistricts(DateTime.UtcNow);
      return $"{recoded} districts refreshed, {bad} address lookups failed.";
    }

    [WebMethod]
    public string VisitorRemoveMalformedEmailAddresses()
    {
      var table = Addresses.GetAllNonEmptyEmailsData();
      var malformed = 0;
      foreach (var row in table)
        if (!Validation.IsValidEmailAddress(row.Email))
        {
          malformed++;
          row.Email = string.Empty;
        }
      Addresses.UpdateTable(table, AddressesTable.ColumnSet.Emails);
      AddressesMaster.UpdateLastRemoveMalformed(DateTime.UtcNow);
      return $"{malformed} malformed email addresses removed.";
    }

    [WebMethod]
    public string VisitorTransferFromAddressLog()
    {
      var totalCount = 0;
      var transferredCount = 0;
      var insufficientContentCount = 0;
      var duplicateCount = 0;
      using (var reader = LogAddressesGoodNew.GetDataReaderByNotTransferredToAddresses(0))
      {
        try
        {
          while (reader.Read())
          {
            var address = reader.ParsedAddress.SafeString().Trim().ToUpperInvariant();
            var city = reader.ParsedCity.SafeString().Trim().ToUpperInvariant();
            var state = reader.ParsedStateCode.SafeString().Trim().ToUpperInvariant();
            var zip5 = reader.ParsedZip5.SafeString().Trim().ToUpperInvariant();
            var zip4 = reader.ParsedZip4.SafeString().Trim().ToUpperInvariant();
            var email = reader.Email.SafeString().Trim();
            totalCount++;
            // We need email, address, city, state, at a minimum
            if (email != string.Empty /*&& address != string.Empty && city != string.Empty &&
              state != string.Empty*/)
            {
              //var matchCount = Addresses.CountByEmailAddressCityStateCodeZip5Zip4(
              //  email, address, city, state, zip5, zip4);
              var matchCount = Addresses.EmailExists(email) ? 1 : 0;
              if (matchCount == 0) // it's new
              {
                transferredCount++;
                Addresses.Insert(string.Empty, string.Empty, address, city, state, zip5, zip4,
                  email, string.Empty, reader.DateStamp.Date, "LOG", false,
                  // since these are log entries, any email address 
                  // indicates an opt-in for sample ballots
                  email != string.Empty, false, VoteDb.DateTimeMin, string.Empty, string.Empty,
                  string.Empty, string.Empty, string.Empty, VoteDb.DateTimeMin, 0, 
                  VotePage.DefaultDbDate, false);
              }
              else duplicateCount++;
            }
            else insufficientContentCount++;
            LogAddressesGoodNew.UpdateTransferredToAddressesById(true, reader.Id);
          }
        }
        catch
        {
          // ignored
        }
      }
      AddressesMaster.UpdateLastTransferFromAddressLog(DateTime.UtcNow);
      return $"{totalCount} log rows read\n" + $"{transferredCount} addresses transferred\n" +
        $"{insufficientContentCount} addresses had insufficient information\n" +
        $"{duplicateCount} addresses were exact duplicates";
    }

    [WebMethod]
    public string VisitorTransferFromSampleBallotLog()
    {
      var totalCount = 0;
      var transferredCount = 0;
      var duplicateCount = 0;
      using (var reader = LogSampleBallotRequests.GetDataReaderByNotTransferredToAddresses(0))
      {
        try
        {
          while (reader.Read())
          {
            var email = reader.Email.SafeString()
              .Trim();
            var state = reader.StateCode.SafeString()
              .Trim()
              .ToUpperInvariant();
            var congress = reader.CongressionalDistrict.SafeString()
              .Trim();
            if (congress.Length > 2) congress = congress.Substring(congress.Length - 2);
            var stateSenate = reader.StateSenateDistrict.SafeString()
              .Trim();
            var stateHouse = reader.StateSenateDistrict.SafeString()
              .Trim();
            var county = reader.County.SafeString()
              .Trim();
            var date = (reader.LastUpdateDate ?? VoteDb.DateTimeMin).Date;
            totalCount++;
            var matchCount = Addresses.EmailExists(email) ? 1 : 0;
            if (matchCount == 0) // it's new
            {
              transferredCount++;
              Addresses.Insert(string.Empty, string.Empty, string.Empty, string.Empty, state,
                string.Empty, string.Empty, email, string.Empty, date,
                "SBRL",
                false,
                // since these are log entries, any email address 
                // indicates an opt-in for sample ballots
                true, false, VoteDb.DateTimeMin, string.Empty,
                congress, stateSenate, stateHouse, county,
                date, 0, VotePage.DefaultDbDate, false);
            }
            else
            {
              Addresses.UpdateSendSampleBallotsByEmail(true, email);
              duplicateCount++;
            }
            LogSampleBallotRequests.UpdateTransferredToAddressesByEmail(true, email);
          }
        }
        catch
        {
          // ignored
        }
      }
      AddressesMaster.UpdateLastTransferFromSampleBallotLog(DateTime.UtcNow);
      return $"{totalCount} log rows read\n" + $"{transferredCount} emails transferred\n" +
        $"{duplicateCount} emails were exact duplicates";
    }

    [WebMethod]
    public void YouTubeUseChannel(string politicianKey, string channelId)
    {
      Politicians.MoveVideoToAnswers(politicianKey);
      Politicians.UpdateYouTubeWebAddress("www.youtube.com/channel/" + channelId, politicianKey);
    }

    [WebMethod]
    public void YouTubeUseVideo(string politicianKey)
    {
      // just update the flag
      Politicians.UpdateYouTubeVideoVerified(true, politicianKey);
    }
  }
}