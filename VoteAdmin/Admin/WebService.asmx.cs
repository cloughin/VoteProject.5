using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
using VoteAdmin;
using VoteAdmin.Politician;
using VoteLibrary.Utility;
using static System.String;

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

    public sealed class AdRateUpdate
    {
      public string Type;
      public int OfficeLevel;
      public int AlternateOfficeLevel;
      public decimal GeneralAdRate;
      public decimal PrimaryAdRate;
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
      public string TransactionId;
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
      public string LocalKey;
      public string Jurisdiction;
      public string PoliticianKey;
      public string ElectionKey;
      public string ElectionKeyToInclude;
      public string OfficeKey;
      public string PartyKey;
      public string PartyEmail;
      public int VisitorId;
      public int DonorId;
      public int OrgContactId;
      public string SortName;
      public string SourceCode;
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
      public string EmailTypeCode;
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
      public string LocalKey;
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

      // following items for Organizations type
      public string OrgAbbreviation;
      public string OrgType;
      public string OrgSubType;
      public string Ideology;
      public string Address1;
      public string Address2;
      public string City;
      public string StateCode;
      public string Zip;
    }

    public sealed class OfficeAnalysis
    {
      public string[] InElectionsOffices;
      public string[] InElectionsPoliticians;
      public string[] InOfficesOfficials;
      public string[] InElectionsIncumbentsRemoved;
      public string[] InPoliticiansLiveOfficeKey;
    }

    public sealed class PoliticianEmail
    {
      public string TemplateName;
      public string Subject;
      public string Body;
      public string SubjectForEmail;
      public string BodyForEmail;
      public string Email;
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
      public string LocalKey;
      public SimpleListItem[] Counties;
      public SimpleListItem[] Locals;
      public SimpleListItem[] Parties;
      public SimpleListItem[] Districts;
      public string ElectionControlHtml;
    }

    public sealed class SearchTopicsResults
    {
      public int Id;
      public string Html;
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
      public string LocalKey;
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

    private static string FindCandidate(string stateCode, string lastName, string firstName,
      ref List<BallotPediaProposedCandidate> proposed)
    {
      // try to encode with exact match
      var table = Politicians.GetNamesDataByStateCodeLastName(stateCode, lastName);
      var matches = table.Where(row => row.FirstName.IsEqIgnoreCase(firstName)).ToList();
      if (matches.Count == 1)
      {
        proposed.Clear();
        return matches[0].PoliticianKey;
      }

      if (table.Count == 0)
        proposed.AddRange(Politicians.GetCandidateListRows(lastName, stateCode).Select(r =>
        {
          var name = Politicians.GetFormattedName(r.PoliticianKey());
          var status = Politicians.FormatOfficeAndStatus(r);
          if (!IsNullOrWhiteSpace(r.PartyCode())) name += " (" + r.PartyCode() + ")";
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
          if (!IsNullOrWhiteSpace(party)) name += " (" + party + ")";
          return new BallotPediaProposedCandidate
          {
            Name = name,
            VoteUsaId = r.PoliticianKey,
            Status = status
          };
        }));

      proposed = proposed.DistinctBy(p => p.VoteUsaId).ToList();
      return Empty;
    }

    private static LoggedEmailNote[] GetEmailNotesPrivate(int id)
    {
      return LogEmailNotes.GetDataByLogEmailId(id)
        .Select(row => new LoggedEmailNote
        {
          DateStamp = row.DateStamp.AsUtc(),
          Note = row.Note,
          IsSystemNote = row.IsSystemNote
        }).OrderByDescending(row => row.DateStamp).ToArray();
    }

    private static VolunteerNote[] GetVolunteerNotesArray(string email)
    {
      return VolunteersNotes.GetDataByEmail(email).Select(n =>
        new VolunteerNote
        {
          Notes = HttpUtility.HtmlEncode(n.Notes).ReplaceNewLinesWithBreakTags(),
          DateStamp = n.DateStamp.AsUtc(),
          Id = n.Id
        }).OrderByDescending(n => n.Id).ToArray();
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
    public string AddOffice(string stateCode, string countyCode, string localKey,
      string officeClass, string line1, string line2)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      var officeKey = Offices.CreateOfficeKey(parsedOfficeClass, stateCode, countyCode,
        localKey, Empty, /*Empty,*/ line1, line2);

      if (IsNullOrWhiteSpace(line1))
        return "*The 1st Line of Office Title is required";

      if (Offices.OfficeKeyExists(officeKey))
        return $"*The proposed office key ({officeKey}) already exists";

      Offices.Insert(officeKey, stateCode, Offices.GetCountyCodeFromKey(officeKey),
        Offices.GetLocalKeyFromKey(officeKey), Empty, /*Empty,*/ line1, line2,
        parsedOfficeClass.ToInt(), 0, 0, false, false, false, 1, Empty, Empty, "Write in", 1,
        false, DateTime.UtcNow, false, false, false, 1, 1, 0, 0, 0, 0, false, false);

      return officeKey;
    }

    [WebMethod]
    public string AddOfficeTemplate(string stateCode, string officeClass, string line1,
      string line2)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      var localKey = parsedOfficeClass.ElectoralClass() == ElectoralClass.Local
        ? "#####"
        : Empty;
      var officeKey = Offices.CreateOfficeKey(parsedOfficeClass, stateCode, "###", localKey,
        Empty, /*Empty,*/ line1, line2);

      if (IsNullOrWhiteSpace(line1))
        return "*The 1st Line of Office Title is required";

      if (Offices.OfficeKeyExists(officeKey))
        return $"*The proposed office key ({officeKey}) already exists";

      Offices.Insert(officeKey, stateCode, Offices.GetCountyCodeFromKey(officeKey),
        Offices.GetLocalKeyFromKey(officeKey), Empty, /*Empty,*/ line1, line2,
        parsedOfficeClass.ToInt(), 0, 0, false, false, false, 1, Empty, Empty, "Write in", 1,
        false, DateTime.UtcNow, false, false, false, 1, 1, 0, 0, 0, 0, true, false);

      return officeKey;
    }

    [WebMethod]
    public void AddOrganizationNote(int orgId, string note)
    {
      OrganizationNotes.Insert(orgId, DateTime.UtcNow, note);
    }

    [WebMethod(EnableSession = true)]
    public string AddUser(string userName, string password, string level, string stateCode,
      string countyCode, string localKey)
    {
      if (!SecurePage.IsSuperUser) return "*Unauthorized";
      userName = userName.Trim();
      password = password.Trim();
      if (IsNullOrWhiteSpace(userName)) return "*User Name is required";
      if (IsNullOrWhiteSpace(password)) return "*Password is required";
      if (IsNullOrWhiteSpace(level)) return "*Security Level is required";
      if (Security.UserNameExists(userName)) return "*User Name is already in use";
      if (level != "MASTER")
      {
        if (IsNullOrWhiteSpace(stateCode))
          return "*State Code is required";
      }
      else stateCode = Empty;

      if (level != "MASTER" && level != "ADMIN")
      {
        if (IsNullOrWhiteSpace(countyCode))
          return "*County Code is required";
      }
      else countyCode = Empty;

      if (level == "LOCAL")
      {
        if (IsNullOrWhiteSpace(localKey))
          return "*Local Key is required";
      }
      else localKey = Empty;

      var userLocalKey = Empty;
      if (!IsNullOrWhiteSpace(stateCode) && !IsNullOrWhiteSpace(localKey))
        userLocalKey = localKey;
      Security.Insert(level, userName, password, Empty, Empty, stateCode, countyCode,
        userLocalKey, false, false);
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

      return new OfficeAnalysis
      {
        InElectionsOffices =
          ElectionsOffices.GetDataByOfficeKey(officeKey).OrderBy(r => r.ElectionKey)
            .Select(r => $"{Elections.GetElectionDesc(r.ElectionKey)} ({r.ElectionKey})")
            .ToArray(),
        InElectionsPoliticians =
          ElectionsPoliticians.GetDataByOfficeKey(officeKey).GroupBy(r => r.ElectionKey)
            .OrderBy(g => g.Key)
            .Select(g =>
              $"{Elections.GetElectionDesc(g.First().ElectionKey)} ({g.First().ElectionKey}: {g.Count()} candidates)")
            .ToArray(),
        InOfficesOfficials =
          OfficesOfficials.GetDataByOfficeKey(officeKey).Select(r =>
              $"{Politicians.GetFormattedName(r.PoliticianKey)} ({r.PoliticianKey})")
            .OrderBy(d => d).ToArray(),
        InElectionsIncumbentsRemoved =
          ElectionsIncumbentsRemoved.GetDataByOfficeKey(officeKey)
            .GroupBy(r => r.ElectionKey).OrderBy(g => g.Key)
            .Select(g =>
              $"{Elections.GetElectionDesc(g.First().ElectionKey)} ({g.First().ElectionKey}: {g.Count()} incumbents)")
            .ToArray(),
        InPoliticiansLiveOfficeKey = Politicians.GetDataByLiveOfficeKey(officeKey)
          .Select(r =>
            $"{Politicians.GetFormattedName(r.PoliticianKey)} ({r.PoliticianKey})")
          .OrderBy(d => d).ToArray()
      };
    }

    [WebMethod]
    public string ApplyBallotPediaLinks(int csvId)
    {
      var message = "BallotPedia Links applied to VoteUSA website.";
      try
      {
        var content = BallotPediaCsvs.GetContentById(csvId);
        if (IsNullOrWhiteSpace(content))
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
            if (IsNullOrWhiteSpace(url) || IsNullOrWhiteSpace(id))
              missingIdOrLink++;
            else if (Politicians.UpdateBallotPediaWebAddress(url, id) == 0)
              noPolitician++;
            else
              updated++;
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
    public void AspKeepAlive()
    {
      Session["KeepAlive"] = DateTime.UtcNow;
    }

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
      if (!IsNullOrWhiteSpace(password))
        Security.UpdateUserPassword(password, userName);
    }

    [WebMethod]
    public void ClearAdImage(string electionKey, string officeKey, string politicianKey)
    {
      ElectionsPoliticians.UpdateAdImage(null, electionKey, officeKey, politicianKey);
    }

    [WebMethod]
    public void ConsolidateTopics(int toId, int fromId)
    {
      try
      {
        UpdateIssuesPage.ConsolidateTopics(toId, fromId);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/ConsolidateTopics", e);
        throw;
      }
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
      Politicians.UpdateLiveOfficeKeyByLiveOfficeKey(Empty, officeKey);
      Offices.DeleteByOfficeKey(officeKey);
    }

    [WebMethod]
    public void DeleteOrganization(int orgId)
    {
      Organizations.DeleteByOrgId(orgId);
    }

    [WebMethod]
    public void DeleteOrganizationAd(int orgId)
    {
      Organizations.UpdateAdInfo(orgId, Empty, true, Empty, null);
    }

    [WebMethod]
    public void DeleteOrganizationNote(int id)
    {
      OrganizationNotes.DeleteById(id);
    }

    [WebMethod(EnableSession = true)]
    public UpdateQuestionInfo DeleteResponse(string politicianKey, bool isVideo, int questionId, int sequence)
    {
      try
      {
        var userName = VotePage.UserName;
        var table =
          Answers2.GetDataByPoliticianKeyQuestionIdSequence(politicianKey, questionId,
            sequence);
        if (table.Count == 0)
          throw new VoteException("We could not find the response to delete");
        var a = table[0];
        if (isVideo && !IsNullOrWhiteSpace(a.Answer))
        {
          // remove the video portion of the entry
          LogDataChange.LogUpdate(Answers2.Column.YouTubeUrl, a.YouTubeUrl, 
            null, userName, SecurePage.UserSecurityClass, DateTime.UtcNow, 
            politicianKey, questionId, sequence);
          a.YouTubeUrl = null;
          a.YouTubeSource = Empty;
          a.YouTubeSourceUrl = null;
          a.YouTubeDate = VotePage.DefaultDbDate;
          a.UserName = userName;
          Answers2.UpdateTable(table);
          DeletedAnswers.Insert(userName, DateTime.UtcNow, a.PoliticianKey, a.QuestionId,
            a.Sequence, Empty, Empty, VotePage.DefaultDbDate, a.UserName, a.YouTubeUrl,
            a.YouTubeDescription, a.YouTubeRunningTime, a.YouTubeSource, a.YouTubeSourceUrl,
            a.YouTubeDate, a.YouTubeRefreshTime, a.YouTubeAutoDisable, a.FacebookVideoUrl,
            a.FacebookVideoDescription, a.FacebookVideoRunningTime, a.FacebookVideoDate,
            a.FacebookVideoRefreshTime, a.FacebookVideoAutoDisable);
        }
        else if (!isVideo && !IsNullOrWhiteSpace(a.YouTubeUrl))
        {
          // remove the text portion of the entry
          LogDataChange.LogUpdate(Answers2.Column.Answer, a.Answer,
            Empty, userName, SecurePage.UserSecurityClass, DateTime.UtcNow,
            politicianKey, questionId, sequence);
          a.Answer = Empty;
          a.Source = Empty;
          a.DateStamp = VotePage.DefaultDbDate;
          a.UserName = userName;
          Answers2.UpdateTable(table);
          DeletedAnswers.Insert(userName, DateTime.UtcNow, a.PoliticianKey, a.QuestionId,
            a.Sequence, a.Answer, a.Source, a.DateStamp, a.UserName, Empty,
            Empty, default, Empty, Empty,
            VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty, Empty,
            Empty, default, VotePage.DefaultDbDate,
            VotePage.DefaultDbDate, Empty);
        }
        else
        {
          // delete the entire entry
          LogDataChange.LogDelete(Answers2.TableName, userName, SecurePage.UserSecurityClass,
            DateTime.UtcNow, politicianKey, questionId, sequence);
          Answers2.DeleteByPoliticianKeyQuestionIdSequence(politicianKey, questionId,
            sequence);
          DeletedAnswers.Insert(userName, DateTime.UtcNow, a.PoliticianKey, a.QuestionId,
            a.Sequence, a.Answer, a.Source, a.DateStamp, a.UserName, a.YouTubeUrl,
            a.YouTubeDescription, a.YouTubeRunningTime, a.YouTubeSource, a.YouTubeSourceUrl,
            a.YouTubeDate, a.YouTubeRefreshTime, a.YouTubeAutoDisable, a.FacebookVideoUrl,
            a.FacebookVideoDescription, a.FacebookVideoRunningTime, a.FacebookVideoDate,
            a.FacebookVideoRefreshTime, a.FacebookVideoAutoDisable);
        }
        return new UpdateQuestionInfo
        {
          Answers = DoGetAnswers(politicianKey, questionId)
        };
      }
      catch (VoteException ex)
      {
        return new UpdateQuestionInfo
        {
          ErrorMessage = ex.Message
        };
      }
    }

    [WebMethod(EnableSession = true)]
    public void DoPoliticianUpdate(string politicianKey, PoliticianUpdateData data)
    {
      var feedback = new List<UpdateAnswer.UpdateAnswerFeedback>();
      UpdatePage.DoPoliticianUpdate(politicianKey, data, feedback);
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
      return Donations.GetDataByEmail(email).Select(d =>
          new Donation
          {
            Date = d.PayPalTransactionId == null 
              ? d.Date.ToString("G")
              : d.Date.ToShortDateString(),
            Amount = d.Amount.ToString("C"),
            TransactionId = d.PayPalTransactionId
          })
        .ToArray();
    }

    [WebMethod]
    public void ForwardEmail(int id, string to, string cc, string bcc, string subject,
      string message)
    {
      try
      {
        to = to.Trim();
        cc = cc.Trim();
        bcc = bcc.Trim();
        if (!Validation.IsValidEmailAddress(to))
          throw new Exception("The to email address is not valid");
        string[] ccs = null;
        if (!IsNullOrWhiteSpace(cc))
        {
          ccs = cc.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()).ToArray();
          foreach (var i in ccs)
            if (!Validation.IsValidEmailAddress(i))
              throw new Exception("The cc email address is not valid");
        }

        string[] bccs = null;
        if (!IsNullOrWhiteSpace(bcc))
        {
          bccs = bcc.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()).ToArray();
          foreach (var i in bccs)
            if (!Validation.IsValidEmailAddress(i))
              throw new Exception("The bcc email address is not valid");
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
            if (!IsNullOrWhiteSpace(message))
              body = message.ReplaceNewLinesWithParagraphs() + "<hr />" + body;
            EmailTemplates.SendEmail(subject, body, batchRow.FromEmail, new[] {to}, ccs,
              bccs);
            var note =
              $"Forwarded to: {to} {(to.IsEqIgnoreCase(emailRow.ToEmail) ? "(original recipient)" : Empty)}\n" +
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
      return AddressesMaster.GetAllData().Select(row => new AddressesDates
      {
        LastRemoveMalformed = row.LastRemoveMalformed.AsUtc(),
        LastTransferFromAddressLog = row.LastTransferFromAddressLog.AsUtc(),
        LastTransferFromSampleBallotLog = row.LastTransferFromSampleBallotLog.AsUtc(),
        LastLookupMissingDistricts = row.LastLookupMissingDistricts.AsUtc(),
        LastRefreshAllDistricts = row.LastRefreshAllDistricts.AsUtc()
      }).SingleOrDefault();
    }

    // ReSharper disable NotAccessedField.Global
    public sealed class OneAnswer
    {
      public bool IsVideo;
      public int QuestionId;
      public int Sequence;
      public string Answer;
      public string Source;
      public ulong DateStamp;
      public string YouTubeUrl;
      public string YouTubeId;
      public string YouTubeDescription;
      public ulong YouTubeRunningTime;
      public string YouTubeSource;
      public string YouTubeSourceUrl;
      public ulong YouTubeDate;
      public string YouTubeAutoDisable;
    }
    // ReSharper restore NotAccessedField.Global

    private static OneAnswer[] DoGetAnswers(string politicianKey, int questionId)
    {
      var table = Answers2.GetDataByPoliticianKeyQuestionId(politicianKey, questionId);
      var answers = new List<OneAnswer>();
      foreach (var row in table)
      {
        if (!IsNullOrWhiteSpace(row.YouTubeUrl))
        {
          // create a video answer item
          answers.Add(new OneAnswer
          {
            IsVideo = true,
            QuestionId = row.QuestionId,
            Sequence = row.Sequence,
            YouTubeUrl = row.YouTubeUrl,
            YouTubeId = row.YouTubeUrl.GetYouTubeVideoId(),
            YouTubeDescription = row.YouTubeDescription,
            YouTubeRunningTime = Convert.ToUInt64(row.YouTubeRunningTime.TotalMilliseconds),
            YouTubeSource = row.YouTubeSource,
            YouTubeSourceUrl = VotePage.NormalizeUrl(row.YouTubeSourceUrl),
            YouTubeDate = row.YouTubeDate.ToJavascriptMilliseconds(),
            YouTubeAutoDisable = row.YouTubeAutoDisable
          });
        }

        if (!IsNullOrWhiteSpace(row.Answer))
        {
          // create a text answer item
          answers.Add(new OneAnswer
          {
            IsVideo = false,
            QuestionId = row.QuestionId,
            Sequence = row.Sequence,
            Answer = row.Answer,
            Source = row.Source,
            DateStamp = row.DateStamp.ToJavascriptMilliseconds(),
          });
        }
      }

      return answers.OrderByDescending(a => a.IsVideo ? a.YouTubeDate : a.DateStamp)
        .ThenByDescending(a => a.Sequence)
        .ThenByDescending(a => a.IsVideo).ToArray();
    }

    [WebMethod]
    public OneAnswer[] GetAnswers(string politicianKey, int questionId)
    {
      return DoGetAnswers(politicianKey, questionId);
    }

    [WebMethod]
    public string GetAnswersHtml(string politicianKey, string officeKey, int questionId, int duplicate)
    {
      //var data = Answers.GetAnswersNew(politicianKey, officeKey, questionId, true);
      //return UpdatePage.FormatAnswers(data.Rows.OfType<DataRow>(), duplicate);

      // We get rid of duplicates for multi-issue questions
      var data = Answers.GetAnswersNew(politicianKey, officeKey, questionId, true).Rows
        .OfType<DataRow>()
        .GroupBy(r => new {QuestionId = r.QuestionId(), Sequence = r.Sequence()})
        .Select(g => g.First()).ToList();
      return UpdatePage.FormatAnswers(data, duplicate);
    }

    [WebMethod]
    public string GetAnswerQuestionsHtml(string politicianKey, string officeKey, int issueId)
    {
      var questionsHtml = Join(Empty, Answers.GetAnswerQuestionsNew(politicianKey, officeKey, issueId)
        .Rows.OfType<DataRow>().Select(q => $"<div class=\"question-accordion\" data-id=\"{q.QuestionId()}\">{q.Question()}<img class=\"ajax-loader\" src=\"/images/ajax-loader16.gif\"/></div><div class=\"question-accordion-content\"></div>"));
      return $"<div class=\"accordion-container\">{questionsHtml}</div>";
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
        if (!IsNullOrWhiteSpace(content))
        {
          using (var csvReader = new CsvReader(new StringReader(content), true))
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
              if (IsNullOrWhiteSpace(stateCode))
                stateCode = csvReader["State"];
              else if (IsNullOrWhiteSpace(voteUsaId))
              {
                voteUsaId = FindCandidate(stateCode, lastName, firstName, ref proposed);
                var lastName2 = Empty;
                if (IsNullOrWhiteSpace(voteUsaId))
                {
                  // parse BallotPedia's full name - if last name
                  // is different retry
                  var parsedName = csvReader["Name"].ParseName();
                  if (parsedName.Last.IsNeIgnoreCase(lastName))
                  {
                    voteUsaId = FindCandidate(stateCode, parsedName.Last, parsedName.First,
                      ref proposed);
                    lastName2 = parsedName.Last;
                  }
                }

                if (IsNullOrWhiteSpace(voteUsaId))
                {
                  // parse BallotPedia's page title - if last name
                  // is different retry
                  var parsedName = csvReader["Page Title"].Replace('_', ' ').ParseName();
                  if (parsedName.Last.IsNeIgnoreCase(lastName) &&
                    parsedName.Last.IsNeIgnoreCase(lastName2))
                    voteUsaId = FindCandidate(stateCode, parsedName.Last, parsedName.First,
                      ref proposed);
                }

                csvChanged = !IsNullOrWhiteSpace(voteUsaId);
              }

              foreach (var col in headers)
                csvWriter.AddField(col == "VoteUSA Id" ? voteUsaId : csvReader[col]);
              csvWriter.Write(streamWriter);
              if (!IsNullOrWhiteSpace(voteUsaId)) coded++;
              candidates.Add(new BallotPediaCsvCandidate
              {
                Name = hasName ? csvReader["Name"] : firstName + " " + lastName,
                SplitName =
                  firstName + " | " + lastName + " (" + csvReader["Page Title"] + ")",
                SortName = lastName + " " + firstName,
                Office = hasOffice ? csvReader["Office"] : Empty,
                StateCode = stateCode,
                VoteUsaId = voteUsaId,
                IsIncumbent = hasStatus && csvReader["Status"] == "Incumbent",
                Party = hasParty ? csvReader["Party"] : Empty,
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

    public sealed class BannerAdInfo
    {
      // ReSharper disable NotAccessedField.Global
      public bool HasAdImage;
      public string AdImageName = Empty;
      public string AdUrl = Empty;
      public bool AdEnabled;
      public string AdMediaType = Empty;
      public string AdYouTubeUrl = Empty;
      public string AdDescription1 = Empty;
      public string AdDescription2 = Empty;
      public string AdDescriptionUrl = Empty;
      public bool AdIsPaid;
      public string AdHtml;
      // ReSharper restore NotAccessedField.Global
    }

    [WebMethod]
    public BannerAdInfo GetBannerAdInfo(string adType, string stateCode, string electionKey,
      string officeKey)
    {
      var row = BannerAds.GetBannerAdInfo(adType, stateCode, electionKey, officeKey);
      if (row == null) return new BannerAdInfo();
      return new BannerAdInfo
      {
        HasAdImage = row.HasAdImage(),
        AdImageName = row.AdImageName(),
        AdUrl = row.AdUrl(),
        AdEnabled = row.AdEnabled(),
        AdMediaType = row.AdMediaType(),
        AdYouTubeUrl = row.AdYouTubeUrl().SafeString(),
        AdDescription1 = row.AdDescription1().SafeString(),
        AdDescription2 = row.AdDescription2().SafeString(),
        AdDescriptionUrl = row.AdDescriptionUrl().SafeString(),
        AdIsPaid = row.AdIsPaid(),
        AdHtml = Utility.RenderBannerAd2(adType, stateCode, electionKey, officeKey, true)
      };
    }

    [WebMethod(EnableSession = true)]
    public GetCandidateHtmlResult GetCandidateHtml(string electionKey, string politicianKey,
      string officeKey, bool openEditDialog, string mode)
    {
      if (!Enum.TryParse(mode, out ManagePoliticiansPanel.DataMode theMode))
        theMode = ManagePoliticiansPanel.DataMode.ManageCandidates;
      try
      {
        return new GetCandidateHtmlResult
        {
          PoliticianKey = politicianKey,
          OpenEditDialog = openEditDialog,
          CandidateHtml =
            ManagePoliticiansPanel.GetCandidateHtml(electionKey, politicianKey, officeKey,
              theMode)
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
      string officeKey, string[] currentCandidates)
    {
      try
      {
        // Note: the officeKey is used only to get stateCode (1st 2 characters of
        // office key, so a state code can be passed.
        // Enhancement to support multi-state filtering: can pass in a comma-separated
        // string of states. 
        return Politicians.GetCandidateListHtml(partialName, selectedPoliticians, officeKey,
          currentCandidates);
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
        return BulkEmailPage.GetPreviewCandidateItems(electionKey, officeKey).ToArray();
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
      {
        // reduce CongressionalDistrict codes from 3 to 2 characters
        return Offices.GetDistrictItems(stateCode, OfficeClass.USHouse).Select(i =>
        {
          i.Value = i.Value.Substring(1);
          return i;
        }).ToArray();
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
        return CountyCache.GetCountiesByState(stateCode).Select(countyCode =>
          new SimpleListItem
          {
            Text = Counties.GetName(stateCode, countyCode),
            Value = countyCode
          }).ToArray();
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
      return new Report {Html = CountiesReport.GetReport(stateCode).RenderToString()};
    }

    [WebMethod]
    public Report GetCountyJurisdictionsReport(string stateCode)
    {
      return new Report
      {
        Html = CountyJurisdictionsReport.GetReport(stateCode).RenderToString()
      };
    }

    [WebMethod]
    public string GetElectionControlHtml(string stateCode, string countyCode,
      string localKey)
    {
      try
      {
        return ElectionControl.GenerateHtml(false, stateCode, countyCode.SafeString(),
          localKey.SafeString());
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionControlHtml", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetElections(string stateCode, string countyCode,
      string localKey)
    {
      try
      {
        return BulkEmailPage.GetPreviewElectionItems(stateCode, countyCode, localKey)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElections", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetElectionsForState(string stateCode)
    {
      try
      {
        return Elections.GetElectionItemsByStateCode(stateCode)
          .ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionsForState", e);
        throw;
      }
    }

    [WebMethod]
    public string GetElectionSpreadsheets(bool all)
    {
      try
      {
        return ElectionSpreadsheetsPage.GetSpreadsheetListHtml(all);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionSpreadsheets", e);
        throw;
      }
    }

    [WebMethod]
    public string GetElectionSpreadsheetsMapping(int id)
    {
      try
      {
        return ElectionSpreadsheetsPage.GetSpreadsheetMappingHtml(id);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionSpreadsheetsMapping", e);
        throw;
      }
    }

    [WebMethod]
    public SpreadsheetRow GetElectionSpreadsheetsRow(int id, int sequence)
    {
      try
      {
        return ElectionSpreadsheetsPage.GetSpreadsheetRow(id, sequence);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetElectionSpreadsheetsRow", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public EmailBatch GetEmailBatch(int batchId, string contactType, string[] stateCodes,
      string[] countyCodes, string[] localKeysOrCodes, string[] partyKeys,
      bool majorParties, bool useBothContacts, string[] politicianEmailsToUse,
      string electionFiltering, string[] electionFilterTypes, bool useFutureElections,
      bool viewableOnly, string electionKey, bool includeAllElectionsOnDate,
      string adFiltering,
      TempEmailBatches.VisitorBatchOptions visitorOptions,
      TempEmailBatches.DonorBatchOptions donorOptions,
      TempEmailBatches.OrgBatchOptions orgOptions,
      TempEmailBatches.EmailTypeBatchOptions emailTypeOptions,
      TempEmailBatches.LoginDateBatchOptions loginDateOptions)
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
                duplicates = TempEmailBatches.CreateAllContactsBatch(batchId, stateCodes,
                  useBothContacts, emailTypeOptions);
                break;

              case "HasType":
                duplicates = TempEmailBatches.CreateAllContactsBatchWithElectionTypes(
                  batchId, stateCodes, useBothContacts, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions);
                break;

              case "HasntType":
                duplicates = TempEmailBatches.CreateAllContactsBatchWithoutElectionTypes(
                  batchId, stateCodes, useBothContacts, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions);
                break;
            }

            break;

          case "State":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateStatesContactsBatch(batchId, stateCodes,
                  useBothContacts, emailTypeOptions);
                break;

              case "HasType":
                duplicates = TempEmailBatches.CreateStatesContactsBatchWithElectionTypes(
                  batchId, stateCodes, useBothContacts, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions);
                break;

              case "HasntType":
                duplicates = TempEmailBatches.CreateStatesContactsBatchWithoutElectionTypes(
                  batchId, stateCodes, useBothContacts, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions);
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
                  stateCodes, countyCodes, useBothContacts, emailTypeOptions);
                break;

              case "HasType":
                duplicates = TempEmailBatches.CreateCountiesContactsBatchWithElectionTypes(
                  batchId, stateCodes, countyCodes, useBothContacts, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions);
                break;

              case "HasntType":
                duplicates =
                  TempEmailBatches.CreateCountiesContactsBatchWithoutElectionTypes(batchId,
                    stateCodes, countyCodes, useBothContacts, electionFilterTypes,
                    useFutureElections, viewableOnly, emailTypeOptions);
                break;
            }

            break;

          case "Local":
            if (stateCodes.Length < 1) throw new VoteException("Missing StateCode");
            if (countyCodes.Length < 1) throw new VoteException("Missing CountyCode");
            if (localKeysOrCodes.Length < 1) throw new VoteException("Missing LocalKey");
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateLocalsContactsBatch(batchId, stateCodes,
                  countyCodes, localKeysOrCodes, useBothContacts, emailTypeOptions);
                break;

              case "HasType":
                duplicates = TempEmailBatches.CreateLocalsContactsBatchWithElectionTypes(
                  batchId, stateCodes, countyCodes, localKeysOrCodes, useBothContacts,
                  electionFilterTypes, useFutureElections, viewableOnly, emailTypeOptions);
                break;

              case "HasntType":
                TempEmailBatches.CreateLocalsContactsBatchWithoutElectionTypes(batchId,
                  stateCodes, countyCodes, localKeysOrCodes, useBothContacts,
                  electionFilterTypes, useFutureElections, viewableOnly, emailTypeOptions);
                break;
            }

            break;

          case "AllCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates = TempEmailBatches.CreateAllCandidatesBatchWithElectionTypes(
                  batchId, stateCodes, politicianEmailsToUse, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions, loginDateOptions);
                break;

              case "Single":
                duplicates = TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                  politicianEmailsToUse, electionKey, includeAllElectionsOnDate, true,
                  emailTypeOptions, adFiltering, loginDateOptions);
                break;
            }

            break;

          case "StateCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates = TempEmailBatches.CreateStatesCandidatesBatchWithElectionTypes(
                  batchId, stateCodes, politicianEmailsToUse, electionFilterTypes,
                  useFutureElections, viewableOnly, emailTypeOptions, loginDateOptions);
                break;

              case "Single":
                duplicates = TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                  politicianEmailsToUse, electionKey, includeAllElectionsOnDate, false,
                  emailTypeOptions, adFiltering, loginDateOptions);
                break;

              case "NoFiltering":
                duplicates =
                  TempEmailBatches.CreateStateCandidatesBatchWithNoElectionFiltering(
                    batchId, stateCodes, politicianEmailsToUse, emailTypeOptions,
                    loginDateOptions);
                break;
            }

            break;

          case "CountyCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates =
                  TempEmailBatches.CreateCountiesCandidatesBatchWithElectionTypes(batchId,
                    stateCodes, countyCodes, politicianEmailsToUse, electionFilterTypes,
                    useFutureElections, viewableOnly, emailTypeOptions, loginDateOptions);
                break;

              case "Single":
                duplicates = TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                  politicianEmailsToUse, electionKey, includeAllElectionsOnDate, false,
                  emailTypeOptions, adFiltering, loginDateOptions);
                break;
            }

            break;

          case "LocalCandidates":
            switch (electionFiltering)
            {
              case "HasType":
                duplicates = TempEmailBatches.CreateLocalsCandidatesBatchWithElectionTypes(
                  batchId, stateCodes, countyCodes, localKeysOrCodes, politicianEmailsToUse,
                  electionFilterTypes, useFutureElections, viewableOnly, emailTypeOptions,
                  loginDateOptions);
                break;

              case "Single":
                duplicates = TempEmailBatches.CreateCandidatesBatchByElection(batchId,
                  politicianEmailsToUse, electionKey, includeAllElectionsOnDate, false,
                  emailTypeOptions, adFiltering, loginDateOptions);
                break;
            }

            break;

          case "PartyOfficials":
            switch (electionFiltering)
            {
              case "NoFiltering":
                duplicates = TempEmailBatches.CreateStatesPartiesBatchByState(batchId,
                  stateCodes, partyKeys, majorParties, emailTypeOptions, loginDateOptions);
                break;

              case "Single":
                duplicates = TempEmailBatches.CreateStatesPartiesBatchByElection(batchId,
                  electionKey, includeAllElectionsOnDate, emailTypeOptions,
                  loginDateOptions);
                break;
            }

            break;

          case "Volunteers":
            duplicates = TempEmailBatches.CreateVolunteersBatch(batchId, stateCodes,
              partyKeys, majorParties, emailTypeOptions, loginDateOptions);
            break;

          case "WebsiteVisitors":
            duplicates = TempEmailBatches.CreateVisitorsBatch(batchId, stateCodes,
              countyCodes, visitorOptions, emailTypeOptions);
            break;

          case "Donors":
            duplicates = TempEmailBatches.CreateDonorsBatch(batchId, stateCodes,
              countyCodes, visitorOptions, donorOptions, emailTypeOptions);
            break;

          case "Organizations":
            duplicates = TempEmailBatches.CreateOrganizationsBatch(batchId, stateCodes, 
              orgOptions, emailTypeOptions);
            break;
        }

        var table = TempEmail.GetDataByBatchId(batchId);
        var namesDictionary = LocalDistricts.GetFocusedNamesDictionary(table);
        return new EmailBatch
        {
          BatchId = batchId,
          Duplicates = duplicates,
          Items = table
            .Select(row => new EmailItem
            {
              Id = row.Id,
              Email = row.Email,
              Contact = row.Contact,
              Title = row.Title,
              Phone = row.Phone,
              StateCode = row.StateCode,
              CountyCode =
                IsNullOrWhiteSpace(row.LocalKey)
                  ? row.CountyCode
                  : namesDictionary[row.StateCode + "|" + row.LocalKey].Value,
              LocalKey = row.LocalKey,
              Jurisdiction =
                IsNullOrWhiteSpace(row.LocalKey)
                  ? Counties.GetFullName(row.StateCode, row.CountyCode)
                  : $"{namesDictionary[row.StateCode + "|" + row.LocalKey].Text}," +
                  $" {StateCache.GetStateName(row.StateCode)}",
              PoliticianKey = row.PoliticianKey,
              ElectionKey = row.ElectionKey,
              ElectionKeyToInclude = row.ElectionKeyToInclude,
              OfficeKey = row.OfficeKey,
              PartyKey = row.PartyKey,
              PartyEmail = row.PartyEmail,
              VisitorId = row.VisitorId ?? 0,
              DonorId = row.DonorId ?? 0,
              OrgContactId = row.OrgContactId ?? 0,
              SortName = row.SortName,
              SourceCode = row.SourceCode
            }).OrderBy(i => StateCache.GetStateName(i.StateCode))
            .ThenBy(i => i.CountyCode ?? Empty).ThenBy(i => i.LocalKey ?? Empty)
            .ThenBy(i => i.PoliticianKey ?? Empty).ThenBy(i => i.PartyKey ?? Empty)
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
        var beginTime = IsNullOrWhiteSpace(beginDate)
          ? DateTime.MinValue
          : DateTime.Parse(beginDate);
        var endTime = IsNullOrWhiteSpace(endDate)
          ? DateTime.MaxValue
          : DateTime.Parse(endDate) + new TimeSpan(1, 0, 0, 0); // add a day
        var reportSuccess = success != "failed";
        var reportFailure = success != "sent";

        return LogEmailBatches
          .GetDataForSearchCriteria(beginTime, endTime, reportSuccess, reportFailure, froms,
            users, searchStrings, joinOption).Select(row => new LogEmailBatchItem
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
              row.CcEmails.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries),
            BccEmails =
              row.BccEmails.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
          }).ToArray();
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
          .Select(row => new EmailTemplateItem
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
          }).OrderByDescending(i => i.IsOwner)
          .ThenBy(i => i.Owner, StringComparer.InvariantCultureIgnoreCase)
          .ThenBy(i => i.Name, StringComparer.InvariantCultureIgnoreCase).ToArray();
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
          .OrderBy(name => name, StringComparer.InvariantCultureIgnoreCase).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmailTemplateNames", e);
        throw;
      }
    }

    [WebMethod]
    public ElectionsPoliticians.EmbeddedElection[] GetEmbeddedKeyDataForState(string stateCode)
    {
      try
      {
        return ElectionsPoliticians.GetEmbeddedKeyDataForState(stateCode);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetEmbeddedKeyDataForState", e);
        throw;
      }
    }

    [WebMethod]
    public string GetGeneralElectionDescription(string input)
    {
      try
      {
        if (DateTime.TryParse(input.Trim(), out var date))
          return Elections.GetGeneralElectionDescriptionTemplate(date.Date);
      }
      // ReSharper disable once EmptyStatement
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetGeneralElectionDescription", e);
      }

      // no fail
      return Empty;
    }

    [WebMethod]
    public Report GetLocalJurisdictionsReport(string stateCode)
    {
      return new Report
      {
        Html = LocalJurisdictionsReport.GetReport(stateCode).RenderToString()
      };
    }

    [WebMethod]
    public string[] GetLocalNameAutosuggest(string term, string stateCode,
      string countyCode)
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
          .OrderBy(kvp => kvp.Value, new AlphanumericComparer()).Select(kvp =>
            new SimpleListItem {Text = kvp.Value, Value = kvp.Key}).ToArray();
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
      string[] localKeysOrCodes, string beginDate, string endDate, string success,
      string flagged, int maximumResults, string[] froms, string[] tos, string[] users,
      string[] electionKeys, string[] officeKeys, string[] candidateKeys,
      string[] politicianKeys, int[] batchIds)
    {
      try
      {
        var beginTime = IsNullOrWhiteSpace(beginDate)
          ? DateTime.MinValue
          : DateTime.Parse(beginDate);
        var endTime = IsNullOrWhiteSpace(endDate)
          ? DateTime.MaxValue
          : DateTime.Parse(endDate) + new TimeSpan(1, 0, 0, 0); // add a day
        var reportSuccess = success != "failed";
        var reportFailure = success != "sent";
        var reportFlagged = flagged != "no";
        var reportUnflagged = flagged != "yes";

        var table = LogEmailBatches.GetLoggedEmails(contactTypes, jurisdictionLevel,
          stateCodes, countyCodes, localKeysOrCodes, beginTime, endTime, reportSuccess,
          reportFailure, reportFlagged, reportUnflagged, maximumResults, froms, tos, users,
          electionKeys, officeKeys, candidateKeys, politicianKeys, batchIds, 0);

        return table.Rows.Cast<DataRow>().Select(row => new LoggedEmailItem
        {
          Id = row.Id(),
          SentTime = row.SentTime().AsUtc(),
          WasSent = row.WasSent(),
          ContactType = row.ContactType(),
          StateCode = row.StateCode(),
          CountyCode = row.CountyCode(),
          LocalKey = row.LocalKey(),
          ElectionKey = row.ElectionKey(),
          OfficeKey = row.OfficeKey(),
          PoliticianKey = row.PoliticianKey(),
          Contact = row.Contact(),
          FromEmail = row.FromEmail(),
          ToEmail = row.ToEmail(),
          Subject = row.Subject(),
          UserName = row.UserName(),
          Jurisdiction =
            (IsNullOrWhiteSpace(row.LocalDistrict()) ? Empty : row.LocalDistrict() + ", ") +
            (row.CountyCode() == "999"
              ? "Multiple counties, " + StateCache.GetStateName(row.StateCode())
              : Counties.GetFullName(row.StateCode(), row.CountyCode())),
          ElectionDescription = row.ElectionDescription().SafeString(),
          OfficeName = Offices.FormatOfficeName(row),
          PoliticianName = Politicians.FormatName(row),
          PartyCode = row.PartyCode().SafeString(),
          SortName = TempEmailBatches.ConcoctSortName(row.Contact()),
          ForwardedCount = row.ForwardedCount(),
          IsFlagged = row.IsFlagged(),
          ErrorMessage = row.ErrorMessage()
        }).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetLoggedEmails", e);
        throw;
      }
    }

    [WebMethod]
    public MoreRecipientInfo GetMoreRecipientInfo(string electionKey, string officeKey,
      string partyKey, int visitorId, int orgContactId)
    {
      try
      {
        var cache = PageCache.GetTemporary();
        var info = new MoreRecipientInfo();
        if (!IsNullOrWhiteSpace(electionKey))
          info.Election = cache.Elections.GetElectionDesc(electionKey);
        if (!IsNullOrWhiteSpace(officeKey))
          info.Office = Offices.FormatOfficeName(cache, officeKey);
        if (!IsNullOrWhiteSpace(partyKey))
          info.Party = cache.Parties.GetPartyName(partyKey);
        if (visitorId != 0)
        {
          var table = Addresses.GetDataById(visitorId);
          if (table.Count == 1)
          {
            var row = table[0];
            info.DateAdded = row.DateStamp.AsUtc();
            info.Address = row.Address;
            var cityStateZip = Empty;
            if (!IsNullOrWhiteSpace(row.City))
              cityStateZip = row.City + ", " + row.StateCode + " ";
            if (!IsNullOrWhiteSpace(row.Zip5))
            {
              cityStateZip += row.Zip5;
              if (!IsNullOrWhiteSpace(row.Zip4)) cityStateZip += "-" + row.Zip4;
            }

            info.CityStateZip = cityStateZip;
            if (!IsNullOrWhiteSpace(row.CongressionalDistrict) &&
              row.CongressionalDistrict != "00")
            {
              var officeTable = Offices.GetDataByStateCodeOfficeLevelDistrictCode(
                row.StateCode, OfficeClass.USHouse.ToInt(),
                row.CongressionalDistrict.ZeroPad(3));
              if (officeTable.Count == 1)
                info.CongressionalDistrict = Offices.FormatOfficeName(officeTable[0]);
            }

            if (!IsNullOrWhiteSpace(row.StateSenateDistrict) &&
              row.StateSenateDistrict != "000")
            {
              var officeTable = Offices.GetDataByStateCodeOfficeLevelDistrictCode(
                row.StateCode, OfficeClass.StateSenate.ToInt(), row.StateSenateDistrict);
              if (officeTable.Count == 1)
                info.StateSenateDistrict = Offices.FormatOfficeName(officeTable[0]);
            }

            if (!IsNullOrWhiteSpace(row.StateHouseDistrict) &&
              row.StateHouseDistrict != "000")
            {
              var officeTable = Offices.GetDataByStateCodeOfficeLevelDistrictCode(
                row.StateCode, OfficeClass.StateHouse.ToInt(), row.StateHouseDistrict);
              if (officeTable.Count == 1)
                info.StateHouseDistrict = Offices.FormatOfficeName(officeTable[0]);
            }
          }
        }

        if (cache.OrgContacts.Exists(orgContactId))
        {
          info.OrgAbbreviation = cache.OrgContacts.GetOrgAbbreviation(orgContactId);
          info.OrgType = cache.OrgContacts.GetOrgType(orgContactId);
          info.OrgSubType = cache.OrgContacts.GetOrgSubType(orgContactId);
          info.Ideology = cache.OrgContacts.GetIdeology(orgContactId);
          info.Address1 = cache.OrgContacts.GetAddress1(orgContactId);
          info.Address2 = cache.OrgContacts.GetAddress2(orgContactId);
          info.City = cache.OrgContacts.GetCity(orgContactId);
          info.StateCode = cache.OrgContacts.GetStateCode(orgContactId);
          info.Zip = cache.OrgContacts.GetZip(orgContactId);
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
        return BulkEmailPage.GetPreviewOfficeItems(electionKey).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetOffices", e);
        throw;
      }
    }

    [WebMethod]
    public string[] GetOfficesByClass(string stateCode, string countyCode, string localKey,
      string officeClass)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      return Offices.GetOfficesByClass(stateCode, countyCode, localKey, parsedOfficeClass)
        .Select(r => Offices.FormatOfficeName(r)).ToArray();
    }

    [WebMethod]
    public string[] GetOfficeTemplatesByClass(string stateCode, string officeClass)
    {
      var parsedOfficeClass = officeClass.ParseOfficeClass();
      return Offices.GetOfficeTemplatesByClass(stateCode, parsedOfficeClass)
        .Select(r => Offices.FormatOfficeName(r)).ToArray();
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
    public OrganizationReportItem[] GetOrganizationsReportData(int orgTypeId, int subTypeId,
      int ideologyId, string stateCode, int[] tagIds, string sortItem, string sortDir)
    {
      return Organizations.GetOrganizationsReportData(orgTypeId, subTypeId, ideologyId,
        stateCode, tagIds, sortItem, sortDir);
    }

    [WebMethod]
    public SimpleListItem[] GetParties(string stateCode)
    {
      try
      {
        return BulkEmailPage.GetPreviewPartyItems(stateCode).ToArray();
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
        return BulkEmailPage.GetPreviewPartyEmailItems(partyKey).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetPartyEmails", e);
        throw;
      }
    }

    [WebMethod]
    public ListItem[] GetPartyPrimariesWithOffices(string stateCode, string electionDate)
    {
      try
      {
        if (DateTime.TryParse(electionDate.Trim(), out var date))
          return Elections.GetPartyPrimariesWithOffices(stateCode, date.Date).Select(row =>
            new ListItem {Text = row.ElectionDesc, Value = row.ElectionKey}).ToArray();
      }
      // ReSharper disable once EmptyStatement
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetPartyPrimariesWithOffices", e);
      }

      // no fail
      return new ListItem[0];
    }

    [WebMethod(EnableSession = true)]
    public PoliticianEmail[] GetPoliticianEmails(string key)
    {
      if (!SecurePage.IsMasterUser)
        throw new ApplicationException("Unauthorized");
      var email = Politicians.GetPublicEmail(key);
      var officeKey = Politicians.GetLiveOfficeKey(key);
      var electionKey = Politicians.GetLiveElectionKey(key);
      var templates = new[]
      {
        "Candidate Credentials"//, "Amazon Charity After Providing Credentials",
        //"Amazon Charity After Candidate Updated Their Information"
      };
      var substitutions = new Substitutions
      {
        PoliticianKey = key,
        ElectionKey = electionKey,
        OfficeKey = officeKey
      };
      return templates.Select(t =>
      {
        var template =
          EmailTemplates.GetDataByNameOwnerTypeOwner(t, "U", "SpecialTemplates")[0];
        var subject = substitutions.Substitute(template.Subject);
        var body = substitutions.Substitute(template.Body);
        return new PoliticianEmail
        {
          TemplateName = t,
          Subject = subject,
          Body = body,
          SubjectForEmail = Uri.EscapeDataString(subject),
          BodyForEmail =
            Uri.EscapeDataString(body.ReplaceNewLinesWithEmptyString()
              .ReplaceBreakTagsWithNewLines().StripHtml()),
          Email = email
        };
      }).ToArray();
    }

    [WebMethod(EnableSession = true)]
    public string GetPoliticianPassword(string key)
    {
      if (!SecurePage.IsMasterUser)
        throw new ApplicationException("Unauthorized");
      return Politicians.GetPassword(key, "******");
    }

    [WebMethod]
    public RestoreInfo GetRestoreInfo(string stateCode, string countyCode, string localKey,
      string districtFiltering, bool getCounties, bool getLocals, bool getParties,
      bool getElections, string[] getBestCounty)
    {
      if (getBestCounty != null)
      {
        var forEach = getBestCounty.Select(k => LocalIdsCodes.FindCounties(stateCode, k)).ToArray();
        var counties = forEach.SelectMany(f => f).Distinct().ToArray();
        countyCode = counties
          .Select(c => new { CountyCode = c, Count = forEach.Count(i => i.Contains(c)) })
          .OrderByDescending(i => i.Count)
          .First().CountyCode;
      }

      var result = new RestoreInfo
      {
        StateCode = stateCode,
        CountyCode = countyCode,
        LocalKey = localKey
      };
      if (getCounties) result.Counties = GetCounties(stateCode);
      if (getLocals) result.Locals = GetLocals(stateCode, countyCode);
      // skip <none>
      if (getParties)
        result.Parties = GetParties(stateCode).Skip(1).ToArray();
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
        result.ElectionControlHtml =
          GetElectionControlHtml(stateCode, countyCode, localKey);

      return result;
    }

    [WebMethod(EnableSession = true)]
    public GetRunningMateHtmlResult GetRunningMateHtml(string electionKey,
      string politicianKey, string mainCandidateKey, bool openEditDialog, string mode)
    {
      if (!Enum.TryParse(mode, out ManagePoliticiansPanel.DataMode theMode))
        theMode = ManagePoliticiansPanel.DataMode.ManageCandidates;
      try
      {
        return new GetRunningMateHtmlResult
        {
          PoliticianKey = politicianKey,
          MainCandidateKey = mainCandidateKey,
          OpenEditDialog = openEditDialog,
          RunningMateHtml = ManagePoliticiansPanel.GetRunningMateHtml(electionKey,
            politicianKey, mainCandidateKey, theMode)
        };
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetRunningMateHtml", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetSearchDistrictsByState(string stateCode, string searchString,
      bool includeCounties)
    {
      try
      {
        var regex = new Regex(Regex.Escape(HttpUtility.HtmlEncode(searchString.Trim())),
          RegexOptions.IgnoreCase);

        var counties = new List<SimpleListItem>();
        if (includeCounties)
        {
          counties = Counties.GetSearchCountiesInState(stateCode, searchString);
          foreach (var item in counties)
            item.Text = regex.Replace(HttpUtility.HtmlEncode(item.Text).SafeString(),
              m => $"<b>{m}</b>");
        }

        var districts = LocalDistricts.GetSearchDistrictsInState(stateCode, searchString);
        foreach (var item in districts)
          item.Text = regex.Replace(HttpUtility.HtmlEncode(item.Text).SafeString(),
            m => $"<b>{m}</b>");

        // get counties for districts
        if (districts.Count > 0)
        {
          var countyNames =
            LocalIdsCodes.FindCountiesWithNames(stateCode, districts.Select(i => i.Value));
          foreach (var item in districts)
          {
            var name = countyNames[item.Value].Length == 1
              ? countyNames[item.Value][0].Text
              : "multiple counties";
            item.Text = $"{item.Text}, <i>{name}</i>";

          }
        }

        return counties.Concat(districts).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetSearchDistrictsByState", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetStateHouseDistricts(string stateCode)
    {
      try
      {
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateHouse).ToArray();
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
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateSenate).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/GetStateSenateDistricts", e);
        throw;
      }
    }

    //[WebMethod]
    //public SimpleListItem[] GetTopics(int issueId)
    //{
    //  return Questions2.GetTopicsForIssue(issueId);
    //}

    [WebMethod]
    public BallotPediaCsv[] GetUploadedBallotPediaCsvs(bool all)
    {
      try
      {
        var table = all
          ? BallotPediaCsvs.GetAllNoContentData()
          : BallotPediaCsvs.GetNoContentDataByCompleted(false);
        return table.Select(r => new BallotPediaCsv
        {
          Id = r.Id,
          Filename = r.Filename,
          UploadTime = r.UploadTime,
          Candidates = r.Candidates,
          CandidatesCoded = r.CandidatesCoded,
          Completed = r.Completed
        }).ToArray();
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
        LocalKey = r.UserLocalKey(),
        CountyName = r.County().SafeString(),
        LocalName = r.LocalDistrict().SafeString()
      }).ToArray();
    }

    [WebMethod]
    public VolunteerNotes GetVolunteerNotes(string email)
    {
      return VolunteersView.GetDataByEmail(email).Select(v => new VolunteerNotes
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
      return VolunteersView.GetAllDataSorted(stateCode, partyKey).Select(r =>
        new Volunteer
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
        }).ToArray();
    }

    [WebMethod]
    public VideoInfo GetYouTubeVideoChannel(string politicianKey)
    {
      var id = Politicians.GetYouTubeWebAddress(politicianKey).GetYouTubeVideoId();
      var result = YouTubeVideoUtility.GetVideoChannelInfo(id, true);
      return result;
    }

    [WebMethod]
    public IssueGroupsData[] LoadIssueGroups()
    {
      return IssueGroups2.GetIssueGroupsData();
    }

    [WebMethod]
    public IssuesData LoadIssues(DateTime? minDate, DateTime? maxDate)
    {
      try
      {
        return Issues2.GetIssuesData(minDate, maxDate);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/LoadIssues", e);
        throw;
      }
    }

    [WebMethod]
    public OrganizationsData LoadOrganizations()
    {
      return Organizations.GetOrganizationsData();
    }

    [WebMethod]
    public OrganizationAdData LoadOrganizationAdData(int orgId)
    {
      var table = Organizations.GetAdData(orgId);
      if (table.Count == 0) return new OrganizationAdData();
      return new OrganizationAdData
      {
        AdImageName = table[0].AdImageName,
        AdUrl = table[0].AdUrl,
        OrgUrl = Organizations.GetUrl(orgId),
        Sample = IsNullOrWhiteSpace(table[0].AdImageName)
        ? Empty
        : Utility.RenderBannerAd(Empty, Empty, Empty, Empty, true, orgId, true)
      };
    }

    [WebMethod]
    public OrganizationEmailTagsData[] LoadOrganizationEmailTags()
    {
      return OrganizationEmailTags.GetOrganizationEmailTagsData();
    }

    [WebMethod]
    public OrganizationImageData[] LoadOrganizationImageData()
    {
      return Organizations.GetOrganizationImageData();
    }

    [WebMethod]
    public OrganizationNotesData[] LoadOrganizationNotes()
    {
      return OrganizationNotes.GetOrganizationNotesData();
    }

    [WebMethod]
    public OrganizationsSelectReportData LoadOrganizationsSelectReportData()
    {
      return Organizations.GetOrganizationsSelectReportData();
    }

    [WebMethod]
    public OrganizationSubTypesData[] LoadOrganizationSubTypes()
    {
      return OrganizationSubTypes.GetOrganizationSubTypesData();
    }

    [WebMethod]
    public OrganizationTypesData[] LoadOrganizationTypes()
    {
      return OrganizationTypes.GetOrganizationTypesData();
    }

    [WebMethod]
    public QuestionsData LoadTopics(DateTime? minDate, DateTime? maxDate)
    {
      try
      {
        return Questions2.GetTopicsData(minDate, maxDate);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/LoadTopics", e);
        throw;
      }
    }

    [WebMethod]
    public string LookupEmailSourceCode(string sourceCode)
    {
      var result = EmailUtility.LookupUpEmailSourceCode(sourceCode.Trim('*'));
      if (IsNullOrWhiteSpace(result)) result = "not found";
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
          EmailTypeCode = row.EmailTypeCode,
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
        return Empty;
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
      var match = Regex.Match(donation, @"^(?<date>\d\d?/\d\d?/\d{4}(?: \d\d?:\d\d?:\d\d? [AP]M)?) \$\d{1,6}\.\d\d(?: (?<id>.+))?");
      if (match.Success)
      {
        var transactionId = match.Groups["id"]?.Value;
        if (IsNullOrWhiteSpace(transactionId))
        {
          // it's an old click and pledge
          var date = DateTime.Parse(match.Groups["date"].Value);
          Donations.DeleteByEmailDate(email, date);
        }
        else
        {
          // paypal
          Donations.DeleteByPayPalTransactionId(transactionId);
        }
      }
      if (optOut)
        EmailUtility.UpdateSubscription(email, "unsubscribe");
    }

    [WebMethod(EnableSession = true)]
    public string SaveEmailTemplate(string name, string subject, string body,
      string emailType, string followAction)
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
          row.EmailTypeCode = emailType;
          row.Requirements = EmailTemplates.GetTemplateRequirementsString(subject, body);
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
    public void SaveEmailTemplateAs(string name, bool isPublic, string subject, string body,
      string emailType, bool isNew)
    {
      try
      {
        var now = DateTime.UtcNow;
        EmailTemplates.Upsert(name, EmailTemplates.GetOwnerType(), VotePage.UserName,
          isPublic, now, now, subject, body, emailType, isNew);
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
      EmailTemplates.UpdateEmailOptionsByNameOwnerTypeOwner(emailOptions, name, ownerType,
        VotePage.UserName);
    }

    [WebMethod]
    public void SaveIssueGroups(IssueGroupsData[] data)
    {
      IssueGroups2.SaveIssueGroupsData(data);
    }

    [WebMethod]
    public void SaveIssues(IssuesDataIssue[] data)
    {
      Issues2.SaveIssuesData(data);
    }

    [WebMethod]
    public void SaveOrganizations(OneOrgType[] data)
    {
      Organizations.SaveOrganizationsData(data);
    }

    [WebMethod]
    public void SaveOrganizationEmailTags(OrganizationEmailTagsData[] data)
    {
      OrganizationEmailTags.SaveOrganizationEmailTagsData(data);
    }

    [WebMethod]
    public void SaveOrganizationNote(int id, string note)
    {
      OrganizationNotes.UpdateNotesById(
        HttpUtility.HtmlDecode(note.ReplaceBreakTagsWithNewLines()), id);
    }

    [WebMethod]
    public void SaveOrganizationTypes(OrganizationTypesData[] data)
    {
      OrganizationTypes.SaveOrganizationTypesData(data);
    }

    [WebMethod]
    public void SaveOrganizationSubTypes(OrganizationSubTypesData[] data)
    {
      OrganizationSubTypes.SaveOrganizationSubTypesData(data);
    }

    [WebMethod(EnableSession = true)]
    public UpdateQuestionInfo SaveResponse(string politicianKey, bool isVideo, int questionId, string value)
    {
      try
      {
        var userName = VotePage.UserName;
        var sequence = Answers.GetNextSequenceNew(politicianKey, questionId);
        if (isVideo)
          value = value.StripHtml();
        value = value.StripRedundantSpaces();
        if (isVideo)
        {
          var newYouTubeUrl = value;
          if (IsNullOrWhiteSpace(newYouTubeUrl))
            throw new VoteException("The YouTube URL is empty.");
          var youTubeId = newYouTubeUrl.GetYouTubeVideoId();
          if (IsNullOrWhiteSpace(youTubeId))
            throw new VoteException("The response was not updated because the" +
              " YouTube URL is not valid.");
          var youTubeInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
          if (!youTubeInfo.IsValid)
            throw new VoteException("The response was not updated because the" +
              " YouTube video was not found on YouTube.");
          if (!youTubeInfo.IsPublic)
            throw new VoteException("The response was not updated because the" +
              " YouTube video is not public.");
          const string newYouTubeSource = "Uploaded by candidate";
          var newYouTubeSourceUrl = Empty;
          var newYouTubeDate = youTubeInfo.PublishedAt;
          LogDataChange.LogInsert(Answers2.TableName, userName, SecurePage.UserSecurityClass,
            DateTime.UtcNow, politicianKey, questionId, sequence);
          Answers2.Insert(politicianKey, questionId, sequence, Empty, Empty,
            VotePage.DefaultDbDate, userName, newYouTubeUrl, 
            youTubeInfo.ShortDescription, youTubeInfo.Duration,
            newYouTubeSource, newYouTubeSourceUrl,
            newYouTubeDate,
            VotePage.DefaultDbDate, null, null, null,
            default, VotePage.DefaultDbDate,
            VotePage.DefaultDbDate, null);
        }
        else
        {
          var textAnswer = value;
          if (IsNullOrWhiteSpace(textAnswer))
            throw new VoteException("The response text is empty.");
          var textSource = Politicians.GetLastName(politicianKey);
          var textDate = DateTime.UtcNow.Date;
          LogDataChange.LogInsert(Answers2.TableName, userName, SecurePage.UserSecurityClass, 
            DateTime.UtcNow, politicianKey, questionId, sequence);
          Answers2.Insert(politicianKey, questionId, sequence, textAnswer, textSource,
            textDate, userName, null, null, default,
            Empty, null, VotePage.DefaultDbDate, 
            VotePage.DefaultDbDate, null, null, null,
            default, VotePage.DefaultDbDate, 
            VotePage.DefaultDbDate, null);
        }
        return new UpdateQuestionInfo
        {
          Answers = DoGetAnswers(politicianKey, questionId)
        };
      }
      catch (VoteException ex)
      {
        return new UpdateQuestionInfo
        {
          ErrorMessage = ex.Message
        };
      }
    }

    [WebMethod]
    public void SaveTopics(QuestionsData data)
    {
      Questions2.SaveTopicsData(data);
    }

    [WebMethod]
    public VolunteerNote[] SaveVolunteerNote(string email, int id, string note)
    {
      VolunteersNotes.UpdateNotesById(
        HttpUtility.HtmlDecode(note.ReplaceBreakTagsWithNewLines()), id);
      return GetVolunteerNotesArray(email);
    }

    [WebMethod]
    public SearchTopicsResults SearchIssuesAndTopics(string searchString, string politicianKey, int id)
    {
      var searchWords = Regex.Replace(searchString, "[^a-z0-9 ]", "").ToLower().Split(new [] {' '}, 
        StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length > 1).Distinct().ToArray();
      var matchedTopics = new List<string>();
      if (searchWords.Length > 1 || searchWords.Length == 1 && searchWords[0].Length > 2)
      {
        var rows = AnswersView.GetIssueTopicsByPoliticianKey(politicianKey).Rows
          .OfType<DataRow>().ToList();
        var regex = new Regex("(" + Join(")|(", searchWords) + ")",
          RegexOptions.IgnoreCase);
        foreach (var row in rows)
        {
          var uniques = new List<string>();
          var matchedTopic = regex.Replace(row.Question(), m =>
          {
            var val = m.ToString();
            if (!uniques.Contains(val, StringComparer.OrdinalIgnoreCase))
              uniques.Add(val);
            return $"<strong>{val}</strong>";
          });
          if (uniques.Count == searchWords.Length)
            matchedTopics.Add(rows.Count(r => r.QuestionId() == row.QuestionId()) > 1
              ? $"<p data-issueid=\"{row.IssueId()}\" data-questionid=\"{row.QuestionId()}\">{matchedTopic} in {row.Issue()}</p>"
              : $"<p data-issueid=\"{row.IssueId()}\" data-questionid=\"{row.QuestionId()}\">{matchedTopic}</p>");
        }
      }

      return new SearchTopicsResults
      {
        Id = id,
        Html = Join(Empty, matchedTopics)
      };
    }

    [WebMethod(EnableSession = true)]
    public BatchEmailSummary SendEmailBatch(string contactType, string subjectTemplate,
      string bodyTemplate, string emailType, string from, string[] cc, string[] bcc,
      int batchId, int found, int[] skip, string selectionCriteria, string description)
    {
      var summary = new BatchEmailSummary {Description = description};
      var logBatchId = -1;
      try
      {
        // reset the batch stats
        TempEmailBatches.UpdateFailedById(0, batchId);
        TempEmailBatches.UpdateSentById(0, batchId);

        // create a logging batch
        logBatchId = LogEmailBatches.Insert(DateTime.UtcNow, contactType, selectionCriteria,
          description, found, skip.Length, 0, 0, VotePage.UserName, from, Join(",", cc),
          Join(",", bcc));

        TempEmailBatches.UpdateTimeLastSentById(DateTime.UtcNow, batchId);
        foreach (var row in TempEmail.GetDataByBatchId(batchId))
          if (!skip.Contains(row.Id))
          {
            summary.Total++;
            try
            {
              EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, emailType, from,
                new[] {row.Email}, cc, bcc, row.StateCode.SafeString(),
                row.CountyCode.SafeString(), row.LocalKey.SafeString(),
                row.Contact.SafeString(), row.Email.SafeString(), row.Title.SafeString(),
                row.Phone.SafeString(), row.ElectionKey.SafeString(),
                row.OfficeKey.SafeString(), row.PoliticianKey.SafeString(),
                row.PartyKey.SafeString(), row.PartyEmail.SafeString(), row.VisitorId ?? 0,
                row.DonorId ?? 0, row.OrgContactId ?? 0, row.SourceCode, logBatchId);
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
      string from, string[] to, string stateCode, string countyCode, string localKey,
      string contact, string email, string title, string phone, string electionKey,
      string officeKey, string politicianKey, string partyKey, string partyEmail,
      int visitorId, int donorId, int orgContactId, string sourceCode)
    {
      try
      {
        EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, null, from, to, null, null,
          stateCode, countyCode, localKey, contact, email, title, phone, electionKey,
          officeKey, politicianKey, partyKey, partyEmail, visitorId, donorId, orgContactId, 
          sourceCode, 0);
      }
      catch (Exception e)
      {
        if (!(e is VoteSubstitutionException))
          VotePage.LogException("Admin/WebService/SendSingleTestEmail", e);
        return e.Message;
      }

      return null;
    }

    [WebMethod]
    public BatchEmailSummary SendTestEmailBatch(string subjectTemplate, string bodyTemplate,
      string from, string[] to, int batchId, int[] skip)
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
              EmailTemplates.SendEmail(subjectTemplate, bodyTemplate, null, from, to, null,
                null, row.StateCode.SafeString(), row.CountyCode.SafeString(),
                row.LocalKey.SafeString(), row.Contact.SafeString(), row.Email.SafeString(),
                row.Title.SafeString(), row.Phone.SafeString(),
                row.ElectionKey.SafeString(), row.OfficeKey.SafeString(),
                row.PoliticianKey.SafeString(), row.PartyKey.SafeString(),
                row.PartyEmail.SafeString(), row.VisitorId ?? 0, row.DonorId ?? 0, 
                row.OrgContactId ?? 0, row.SourceCode, 0);
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
                ToAddresses = Join(",", to),
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
      string stateCode, string countyCode, string localKey, string politicianKey,
      string electionKey, string officeKey, string partyKey, string partyEmail,
      int visitorId, int donorId, int orgContactId, string sourceCode)
    {
      try
      {
        var substitution = EmailTemplates.GetSubstititionsForEmail(stateCode, countyCode,
          localKey, contact, email, title, phone, electionKey, officeKey, politicianKey,
          partyKey, partyEmail, visitorId, donorId, orgContactId, sourceCode);
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

    [WebMethod]
    public void UpdateAdRates(AdRateUpdate[] rates)
    {
      try
      {
        foreach (var rate in rates)
          switch (rate.Type)
          {
            case "H":
              DB.Vote.Master.UpdateHomeAdRate(rate.GeneralAdRate);
              break;

            case "B":
              DB.Vote.Master.UpdateBallotAdRate(rate.GeneralAdRate);
              break;

            case "C":
              DB.Vote.Master.UpdateContestAdRate(rate.GeneralAdRate);
              break;

            default:
              OfficeClasses.UpdateGeneralAdRateByOfficeLevelAlternateOfficeLevel(
                rate.GeneralAdRate, rate.OfficeLevel, rate.AlternateOfficeLevel);
              OfficeClasses.UpdatePrimaryAdRateByOfficeLevelAlternateOfficeLevel(
                rate.PrimaryAdRate, rate.OfficeLevel, rate.AlternateOfficeLevel);
              break;
          }
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/UpdateAdRates", e);
        throw;
      }
    }

    [WebMethod(EnableSession = true)]
    public UpdateBallotPediaCsvInfo UpdateBallotPediaCsv(int csvId, bool isComplete,
      SimpleListItem[] candidateIds)
    {
      var info = new UpdateBallotPediaCsvInfo();

      var invalidIds = new List<string>();
      var candidateIdDictionary = new Dictionary<int, string>();

      foreach (var item in candidateIds)
      {
        if (int.TryParse(item.Value, out var index) &&
          Politicians.PoliticianKeyExists(item.Text))
          candidateIdDictionary.Add(index, item.Text);
        else invalidIds.Add(item.Text);
      }

      try
      {
        if (candidateIdDictionary.Count > 0)
        {
          var content = BallotPediaCsvs.GetContentById(csvId);
          if (IsNullOrWhiteSpace(content))
            throw new VoteException("Missing CSV");
          content = UpdateCsv(content, candidateIdDictionary, out var coded);
          BallotPediaCsvs.UpdateCandidatesCodedById(coded, csvId);
          BallotPediaCsvs.UpdateContentById(content, csvId);
        }

        BallotPediaCsvs.UpdateCompletedById(isComplete, csvId);
        info.Message = "The CSV was updated.";
        if (invalidIds.Count > 0)
          info.Message += "\n\nThe following invalid ids were not updated:\n" +
            Join(", ", invalidIds);
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
                var id = dictionary.ContainsKey(index) ? dictionary[index] : csvReader[col];
                csvWriter.AddField(id);
                if (!IsNullOrWhiteSpace(id)) coded++;
              }
              else
                csvWriter.AddField(csvReader[col]);

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
            (int) v["volunteers"]);
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
    public void UpdateElectionSpreadsheetsMapping(int id, int sequence, string mapping)
    {
      try
      {
        ElectionSpreadsheetsColumns.UpdateMappingByIdSequence(mapping, id, sequence);
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/UpdateElectionSpreadsheetsMapping", e);
        throw;
      }
    }

    [WebMethod]
    public List<SimpleListItem> UpdateEmailType(string emailTypeCode, string description)
    {
      try
      {
        if (IsNullOrEmpty(emailTypeCode))
        {
          // create new -- concoct key
          var key = Regex.Replace(description.ToUpper(), "[^A-Z0-9]", "", RegexOptions.IgnoreCase);
          if (IsNullOrWhiteSpace(key)) key = "KEY";
          else if (key.Length > EmailTypes.EmailTypeCodeMaxLength)
            key = key.Substring(0, EmailTypes.EmailTypeCodeMaxLength);
          if (EmailTypes.EmailTypeCodeExists(key))
          {
            var inx = 1;
            while (true)
            {
              var suffix = inx.ToString();
              if (key.Length + suffix.Length > EmailTypes.EmailTypeCodeMaxLength)
                key = key.Substring(0, EmailTypes.EmailTypeCodeMaxLength - suffix.Length);
              key += suffix;
              if (EmailTypes.EmailTypeCodeExists(key))
                inx++;
              else break;
            }
          }
          EmailTypes.Insert(key, description);
        }
        else
        {
          EmailTypes.UpdateDescriptionByEmailTypeCode(description, emailTypeCode);
        }
        return BulkEmailPage.GetEmailTypes();
      }
      catch (Exception e)
      {
        VotePage.LogException("Admin/WebService/UpdateEmailType", e);
        throw;
      }
    }

    [WebMethod]
    public void UpdateOrganizationAd()
    {
      var result = new Dictionary<string, string>();
      try
      {
        // we access the form data through the request object to handle uploaded file
        if (!int.TryParse(Context.Request.Form["orgId"], out var orgId))
          throw new ApplicationException("Invalid organization id");
        var url = Context.Request.Form["url"];
        var filename = Context.Request.Form["filename"];
        byte[] blob = null;
        var imageFileChanged = Context.Request.Form["imageFileChanged"] == "true";
        if (imageFileChanged)
        {
          if (Context.Request.Files.Count == 0)
            throw new ApplicationException("Upload file is missing");
          var file = Context.Request.Files[0];
          Debug.Assert(file != null, nameof(file) + " != null");
          if (file.ContentLength == 0)
            throw new ApplicationException("Upload file is missing");

          // make sure the blob is really an image
          using (var memoryStream = new MemoryStream())
          {
            file.InputStream.CopyTo(memoryStream);
            blob = memoryStream.ToArray();
            try
            {
              System.Drawing.Image.FromStream(memoryStream);
            }
            catch (Exception)
            {
              throw new ApplicationException("File is not a valid image file");
            }
          }
        }

        Organizations.UpdateAdInfo(orgId, url, imageFileChanged, filename, blob);
      }
      catch (Exception e)
      {
        result["error"] = e.Message;
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }

    // ReSharper disable NotAccessedField.Global
    public sealed class UpdateQuestionInfo
    {
      public OneAnswer[] Answers;
      public string ErrorMessage;
    }
    // ReSharper restore NotAccessedField.Global

    [WebMethod(EnableSession = true)]
    public UpdateQuestionInfo UpdateResponse(string politicianKey, bool isVideo, int questionId, int sequence, string newValue)
    {
      try
      {
        var userName = VotePage.UserName;
        if (isVideo)
          newValue = newValue.StripHtml();
        newValue = newValue.StripRedundantSpaces();
        var table =
          Answers2.GetDataByPoliticianKeyQuestionIdSequence(politicianKey, questionId,
            sequence);
        if (table.Count == 0)
          throw new VoteException("We could not find the response to update");
        var answerRow = table[0];
        if (isVideo)
        {
          var newYouTubeUrl = newValue;
          if (IsNullOrWhiteSpace(newYouTubeUrl))
            throw new VoteException("The YouTube URL is empty. To completely" +
              " remove a response use the 'Delete Response' button.");
          if (newYouTubeUrl == answerRow.YouTubeUrl)
            throw new VoteException("The response was not updated because the" +
              " YouTube URL is unchanged.");
          var youTubeId = newYouTubeUrl.GetYouTubeVideoId();
          if (IsNullOrWhiteSpace(youTubeId))
            throw new VoteException("The response was not updated because the" +
              " YouTube URL is not valid.");
          var youTubeInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
          if (!youTubeInfo.IsValid)
            throw new VoteException("The response was not updated because the" +
              " YouTube video was not found on YouTube.");
          if (!youTubeInfo.IsPublic)
            throw new VoteException("The response was not updated because the" +
              " YouTube video is not public.");
          const string newYouTubeSource = "Uploaded by candidate";
          var newYouTubeSourceUrl = Empty;
          var newYouTubeDate = youTubeInfo.PublishedAt;
          LogDataChange.LogUpdate(Answers2.Column.YouTubeUrl, answerRow.YouTubeUrl,
            newYouTubeUrl, newYouTubeSource, userName,
            SecurePage.UserSecurityClass, DateTime.UtcNow, politicianKey, questionId,
            sequence);
          answerRow.YouTubeUrl = newYouTubeUrl;
          answerRow.YouTubeSource = newYouTubeSource;
          answerRow.YouTubeSourceUrl = newYouTubeSourceUrl;
          answerRow.YouTubeDate = newYouTubeDate;
          answerRow.UserName = userName;
          Answers2.UpdateTable(table);
        }
        else
        {
          var newTextAnswer = newValue;
          if (IsNullOrWhiteSpace(newTextAnswer))
            throw new VoteException("The response text is empty. To completely" +
              " remove a response use the 'Delete Response' button.");
          if (newTextAnswer == answerRow.Answer)
            throw new VoteException("The response was not updated because the" +
              " response text is unchanged.");
          var newTextSource = Politicians.GetLastName(politicianKey);
          var newTextDate = DateTime.UtcNow.Date;
          LogDataChange.LogUpdate(Answers2.Column.Answer, answerRow.Answer, 
            newTextAnswer, newTextSource, userName, 
            SecurePage.UserSecurityClass, DateTime.UtcNow, politicianKey, questionId,
            sequence);
          answerRow.Answer = newTextAnswer;
          answerRow.Source = newTextSource;
          answerRow.DateStamp = newTextDate;
          answerRow.UserName = userName;
          Answers2.UpdateTable(table);
        }
        return new UpdateQuestionInfo
        {
          Answers = DoGetAnswers(politicianKey, questionId)
        };
      }
      catch (VoteException ex)
      {
        return new UpdateQuestionInfo
        {
          ErrorMessage = ex.Message
        };
      }
    }

    [WebMethod]
    public void UploadAdImage()
    {
      var result = new Dictionary<string, string>();
      try
      {
        // we access the form data through the request object to handle uploaded file
        var electionKey = Context.Request.Form["electionKey"];
        var officeKey = Context.Request.Form["officeKey"];
        var politicianKey = Context.Request.Form["politicianKey"];

        if (Context.Request.Files.Count == 0)
          throw new ApplicationException("Upload file is missing");
        var file = Context.Request.Files[0];
        Debug.Assert(file != null, nameof(file) + " != null");
        if (file.ContentLength == 0)
          throw new ApplicationException("Upload file is missing");

        // make sure the blob is really an image
        byte[] blob;
        using (var memoryStream = new MemoryStream())
        {
          file.InputStream.CopyTo(memoryStream);
          blob = memoryStream.ToArray();
          try
          {
            System.Drawing.Image.FromStream(memoryStream);
          }
          catch (Exception)
          {
            throw new ApplicationException("File is not a valid image file");
          }
        }

        ElectionsPoliticians.UpdateAdImage(blob, electionKey, officeKey, politicianKey);
      }
      catch (Exception e)
      {
        result["error"] = e.Message;
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }

    [WebMethod]
    public void UploadBannerAd()
    {
      var result = new Dictionary<string, string>();
      try
      {
        // key fields
        var adType = Context.Request.Form["adType"];
        var stateCode = Context.Request.Form["stateCode"];
        var electionKey = Context.Request.Form["electionKey"];
        var officeKey = Context.Request.Form["officeKey"];

        // data fields
        var hasAdImage = bool.Parse(Context.Request.Form["hasAdImage"]);
        var adImageName = Context.Request.Form["adImageName"];
        var adUrl = Context.Request.Form["adUrl"].RemoveHttp();
        var adEnabled = bool.Parse(Context.Request.Form["adEnabled"]);
        var adMediaType = Context.Request.Form["adMediaType"];
        var adYouTubeUrl = Context.Request.Form["adYouTubeUrl"].RemoveHttp();
        var adDescription1 = Context.Request.Form["adDescription1"];
        var adDescription2 = Context.Request.Form["adDescription2"];
        var adDescriptionUrl = Context.Request.Form["adDescriptionUrl"].RemoveHttp();
        var adIsPaid = bool.Parse(Context.Request.Form["adIsPaid"]);
        var removeImage = bool.Parse(Context.Request.Form["removeImage"]) && adMediaType == "Y";

        // correct a missing adDescription1 with a present adDescription2
        if (IsNullOrWhiteSpace(adDescription1))
        {
          adDescription1 = adDescription2;
          adDescription2 = Empty;
        }

        // Description is trequired for YouTube
        if (IsNullOrWhiteSpace(adDescription1) && adMediaType == "Y")
        {
          throw new ApplicationException("Ad Description is required for YouTube ads");
        }

        byte[] blob = null;
        if (!removeImage && Context.Request.Files.Count == 1)
        {
          var file = Context.Request.Files[0];
          Debug.Assert(file != null, nameof(file) + " != null");
          if (file.ContentLength > 0)
            using (var memoryStream = new MemoryStream())
            {
              file.InputStream.CopyTo(memoryStream);
              blob = memoryStream.ToArray();
              // make sure it's an image
              System.Drawing.Image image;
              try
              {
                image = System.Drawing.Image.FromStream(memoryStream);
              }
              catch (Exception)
              {
                throw new ApplicationException("File is not a valid image file");
              }

              if (!(image.RawFormat.Equals(ImageFormat.Bmp) ||
                image.RawFormat.Equals(ImageFormat.Gif) ||
                image.RawFormat.Equals(ImageFormat.Jpeg) ||
                image.RawFormat.Equals(ImageFormat.Png) ||
                image.RawFormat.Equals(ImageFormat.Tiff)))
                throw new ApplicationException("File is not a valid image file format");
            }
        }
        switch (adMediaType)
        {
          case "I": // image
            // for image type we need either a new file or an existing file (hasAdImage)
            if (blob == null && !hasAdImage)
              throw new ApplicationException("An image file is required");
            // and we need a target URL
            if (IsNullOrWhiteSpace(adUrl))
              throw new ApplicationException("The Target Page URL is required");
            // no adYouTubeUrl
            adYouTubeUrl = Empty;
            break;

          case "Y": // YouTube
            // for YouTube type  we need a description
            if (IsNullOrWhiteSpace(adDescription1))
              throw new ApplicationException("Ad description is required for a YouTube ad");
            // and we need a vaild YouTube url
            if (IsNullOrWhiteSpace(adYouTubeUrl))
              throw new ApplicationException("A YouTube video is required");
            if (!adYouTubeUrl.IsValidYouTubeVideoUrl())
              throw new ApplicationException("The YouTube video URL is invalid");
            // no adUrl for YouTube type
            adUrl = Empty;
            if (removeImage) adImageName = Empty;
            break;

          default:
            throw new ApplicationException($"Invalid ad media type: '{adMediaType}'");
        }

        // if we got this far we are good to upsert
        if (blob == null && !removeImage) // keep existing image
          BannerAds.Upsert(adType, stateCode, electionKey, officeKey, adImageName, adUrl,
            adEnabled, adMediaType, adYouTubeUrl, adDescription1, adDescription2,
            adDescriptionUrl, adIsPaid);
        else
          BannerAds.Upsert(adType, stateCode, electionKey, officeKey, adImageName, adUrl,
            adEnabled, adMediaType, adYouTubeUrl, adDescription1, adDescription2,
            adDescriptionUrl, adIsPaid, blob);
      }
      catch (Exception e)
      {
        result["error"] = e.Message;
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }

    [WebMethod]
    public void UploadBannerAdImage()
    {
      var result = new Dictionary<string, string>();
      try
      {
        // we access the form data through the request object to handle uploaded file
        var adType = Context.Request.Form["adType"];
        var stateCode = Context.Request.Form["stateCode"];
        var electionKey = Context.Request.Form["electionKey"];
        var officeKey = Context.Request.Form["officeKey"];

        if (Context.Request.Files.Count == 0)
          throw new ApplicationException("Upload file is missing");
        var file = Context.Request.Files[0];
        Debug.Assert(file != null, nameof(file) + " != null");
        if (file.ContentLength == 0)
          throw new ApplicationException("Upload file is missing");
        byte[] blob;

        // make sure the blob is really an image
        using (var memoryStream = new MemoryStream())
        {
          file.InputStream.CopyTo(memoryStream);
          blob = memoryStream.ToArray();
          try
          {
            System.Drawing.Image.FromStream(memoryStream);
          }
          catch (Exception)
          {
            throw new ApplicationException("File is not a valid image file");
          }
        }

        // create or update
        if (BannerAds.AdTypeStateCodeElectionKeyOfficeKeyExists(adType, stateCode, electionKey, officeKey))
          BannerAds.UpdateAdImage(blob, adType, stateCode, electionKey, officeKey);
        else
          BannerAds.Insert(adType, stateCode, electionKey, officeKey, blob, Empty, Empty,
            false, Empty, null, null, null, null, false);
      }
      catch (Exception e)
      {
        result["error"] = e.Message;
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }

    [WebMethod]
    public void UploadImage()
    {
      var result = new Dictionary<string, string>();
      try
      {
        // we access the form data through the request object to handle uploaded file
        var id = int.Parse(Context.Request.Form["id"]);
        var externalName = Context.Request.Form["externalName"];
        var fileName = Context.Request.Form["fileName"];
        var comments = Context.Request.Form["comments"];
        var isNew = id == 0;

        byte[] blob= null;
        string imageType = null;
        if (Context.Request.Files.Count == 1)
        {
          var file = Context.Request.Files[0];
          Debug.Assert(file != null, nameof(file) + " != null");
          if (file.ContentLength == 0)
            throw new ApplicationException("Upload file is missing");
          // make sure the blob is really an image
          using (var memoryStream = new MemoryStream())
          {
            file.InputStream.CopyTo(memoryStream);
            blob = memoryStream.ToArray();
            try
            {
              var image = System.Drawing.Image.FromStream(memoryStream);
              if (image.RawFormat.Equals(ImageFormat.Bmp))
                imageType = ".bmp";
              else if (image.RawFormat.Equals(ImageFormat.Gif))
                imageType = ".gif";
              else if (image.RawFormat.Equals(ImageFormat.Jpeg))
                imageType = ".jpeg";
              else if (image.RawFormat.Equals(ImageFormat.Png))
                imageType = ".png";
              if (imageType == null)
                throw new ApplicationException("File is not a valid image file");
            }
            catch (Exception)
            {
              throw new ApplicationException("File is not a valid image file");
            }
          }
        }
        else if (isNew)
          throw new ApplicationException("Upload file is missing");
        if (IsNullOrWhiteSpace(externalName))
          throw new ApplicationException("An external name is required");
        var originalExternalName = isNew ? Empty : UploadedImages.GetExternalName(id);
        if (Compare(externalName, originalExternalName,
          StringComparison.OrdinalIgnoreCase) != 0)
          // make sure name is unique
          if (UploadedImages.ExternalNameExists(externalName))
            throw new ApplicationException($"The external name \"{externalName}\" already exists");

        if (isNew)
          id = UploadedImages.Insert(externalName, imageType, fileName, blob, comments,
            DateTime.UtcNow);
        else
        {
          UploadedImages.UpdateExternalName(externalName, id);
          UploadedImages.UpdateComments(comments, id);
          if (blob != null)
          {
            UploadedImages.UpdateImage(blob, id);
            UploadedImages.UpdateImageChangeTime(DateTime.UtcNow, id);
            UploadedImages.UpdateFileName(fileName, id);
            UploadedImages.UpdateImageType(imageType, id);
          }
        }

        result["id"] = id.ToString();


        //// create or update
        //if (BannerAds.AdTypeStateCodeElectionKeyOfficeKeyExists(adType, stateCode, electionKey, officeKey))
        //  BannerAds.UpdateAdImage(blob, adType, stateCode, electionKey, officeKey);
        //else
        //  BannerAds.Insert(adType, stateCode, electionKey, officeKey, blob, Empty, Empty,
        //    false);
      }
      catch (Exception e)
      {
        result["error"] = e.Message;
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }

    [WebMethod]
    public string VisitorRefreshAllDistricts()
    {
      try
      {
        var now = DateTime.UtcNow;
        var table = Addresses.GetAllGeocodedData();
        var recoded = 0;
        var bad = 0;
        foreach (var row in table)
        {
          // ReSharper disable PossibleInvalidOperationException
          var tiger = TigerLookup.Lookup(row.Latitude.Value, row.Longitude.Value);
          // ReSharper restore PossibleInvalidOperationException
          row.DistrictLookupDate = now;
          if (!IsNullOrWhiteSpace(tiger.Congress) && !IsNullOrWhiteSpace(tiger.Upper) &&
            !IsNullOrWhiteSpace(tiger.County) && tiger.StateCode.IsEqIgnoreCase(row.StateCode))
          {
            row.StateCode = tiger.StateCode;
            row.CongressionalDistrict = tiger.Congress;
            row.StateSenateDistrict = tiger.Upper;
            row.StateHouseDistrict = tiger.Lower;
            row.County = tiger.County;
            row.District = tiger.District;
            row.Place = tiger.Place;
            row.Elementary = tiger.Elementary;
            row.Secondary = tiger.Secondary;
            row.Unified = tiger.Unified;
            row.CityCouncil = tiger.CityCouncil;
            row.CountySupervisors = tiger.CountySupervisors;
            row.SchoolDistrictDistrict = tiger.SchoolDistrictDistrict;
            recoded++;
            //if (recoded % 100 == 0)
            //  Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding);
          }
          else
            bad++;
        }

        Addresses.UpdateTable(table, AddressesTable.ColumnSet.DistrictCoding, -1,
          ConflictOption.OverwriteChanges);
        AddressesMaster.UpdateLastRefreshAllDistricts(DateTime.UtcNow);
        return $"{recoded} districts refreshed, {bad} address lookups failed.";

      }
      catch (Exception ex)
      {
        SecurePage.LogAdminError(ex);
        throw;
      }
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
          row.Email = Empty;
        }

      Addresses.UpdateTable(table, AddressesTable.ColumnSet.Emails);
      AddressesMaster.UpdateLastRemoveMalformed(DateTime.UtcNow);
      return $"{malformed} malformed email addresses removed.";
    }

    [WebMethod]
    public string VisitorTransferFromSampleBallotLog()
    {
      var totalCount = 0;
      var transferredCount = 0;
      var duplicateCount = 0;
      using (var reader =
        LogSampleBallotRequests.GetDataReaderByNotTransferredToAddresses(0))
      {
        try
        {
          while (reader.Read())
          {
            var email = reader.Email.SafeString().Trim();
            var state = reader.StateCode.SafeString().Trim().ToUpperInvariant();
            var congress = reader.CongressionalDistrict.SafeString().Trim();
            if (congress.Length > 2) congress = congress.Substring(congress.Length - 2);
            var stateSenate = reader.StateSenateDistrict.SafeString().Trim();
            var stateHouse = reader.StateSenateDistrict.SafeString().Trim();
            var county = reader.County.SafeString().Trim();
            var district = reader.District.SafeString().Trim();
            var place = reader.Place.SafeString().Trim();
            var elementary = reader.Elementary.SafeString().Trim();
            var secondary = reader.Secondary.SafeString().Trim();
            var unified = reader.Unified.SafeString().Trim();
            var cityCouncil = reader.CityCouncil.SafeString().Trim();
            var countySupervisors = reader.CountySupervisors.SafeString().Trim();
            var schoolDistrictDistrict = reader.SchoolDistrictDistrict.SafeString().Trim();
            var date = (reader.LastUpdateDate ?? VoteDb.DateTimeMin).Date;
            totalCount++;
            var matchCount = Addresses.EmailExists(email) ? 1 : 0;
            if (matchCount == 0) // it's new
            {
              transferredCount++;
              Addresses.Insert(Empty, Empty, Empty, Empty, state, Empty, Empty, email,
                Empty, date, "SBRL", false,
                // since these are log entries, any email address 
                // indicates an opt-in for sample ballots
                true, false, VoteDb.DateTimeMin, Empty, congress, stateSenate, stateHouse,
                county, district, place, elementary, secondary, unified, cityCouncil,
                countySupervisors, schoolDistrictDistrict, null, null, date, 0,
                VotePage.DefaultDbDate, false);
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
      Politicians.MoveVideoToAnswersNew(politicianKey);
      Politicians.UpdateYouTubeWebAddress("www.youtube.com/channel/" + channelId,
        politicianKey);
    }

    [WebMethod]
    public void YouTubeUseVideo(string politicianKey)
    {
      // just update the flag
      Politicians.UpdateYouTubeVideoVerified(true, politicianKey);
    }
  }
}