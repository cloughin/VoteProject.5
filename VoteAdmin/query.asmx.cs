using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using DB;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [System.ComponentModel.ToolboxItem(false)]
  [System.Web.Script.Services.ScriptService]
  public class QueryWebService : WebService
  {
    private class BuildElection
    {
      private readonly bool _Empty;
      private readonly bool _Dates;
      private readonly bool _Offices;
      private readonly bool _Candidates;
      private readonly bool _Bio;
      private readonly bool _Reasons;
      private readonly bool _Issues;

      public BuildElection(string options)
      {
        foreach (var option in options.Split(','))
          switch(option.Trim().ToLowerInvariant())
          {
            case "empty":
              _Empty = true;
              break;

            case "dates":
              _Dates = true;
              break;

            case "offices":
              _Offices = true;
              break;

            case "candidates":
              _Offices = true;
              _Candidates = true;
              break;

            case "bio":
              _Offices = true;
              _Candidates = true;
              _Bio = true;
              break;

            case "reasons":
              _Offices = true;
              _Candidates = true;
              _Reasons = true;
              break;

            case "issues":
              _Offices = true;
              _Candidates = true;
              _Issues = true;
              break;

            case "all":
              _Offices = true;
              _Candidates = true;
              _Bio = true;
              _Reasons = true;
              _Issues = true;
              break;
          }
      }

      private static string FormatDate(DateTime date)
      {
        return date.IsDefaultDate() ? Empty : date.ToString("d");
      }

      private static string FormatTime(TimeSpan time)
      {
        return time == default ? Empty : time.ToString("c");
      }

      private void AddField(IDictionary record, string name, string value)
      {
        if (_Empty || !IsNullOrWhiteSpace(value))
          record.Add(name, value.SafeString().Trim());
      }

      private void AddField(IDictionary record, string name, bool value)
      {
        if (_Empty || value)
          record.Add(name, value);
      }

      private void FillCandidateRecord(IDictionary record, DataRow row, 
        IList<DataRow> issues)
      {
        AddField(record, "politicianKey", row.PoliticianKey());
        AddField(record, "name", Politicians.FormatName(row));
        AddField(record, "address", row.PublicAddress());
        AddField(record, "cityStateZip", row.PublicCityStateZip());
        AddField(record, "phone", row.PublicPhone());
        AddField(record, "dateOfBirth", FormatDate(row.DateOfBirth()));
        AddField(record, "partyName", row.PartyName());
        AddField(record, "email", row.PublicEmail());
        AddField(record, "webAddress", row.PublicWebAddress());
        if (_Bio)
          AddSpecialIssue(record, row, issues, "ALLBio", "bio");
        if (_Reasons)
          AddSpecialIssue(record, row, issues, "ALLPersonal", "reasons");
        if (_Issues)
          AddIssues(record, row, issues);
        if (!row.IsRunningMate())
        {
          AddField(record, "isIncumbent", row.IsIncumbent());
          AddField(record, "isWinner", row.IsWinner());
        }
      }

      private void AddSpecialIssue(IDictionary record, DataRow row,
        IEnumerable<DataRow> issues, string issueKey, string fieldName)
      {
        var subset = issues
          .Where(a => a.PoliticianKey().IsEqIgnoreCase(row.PoliticianKey()) &&
            a.IssueKey() == issueKey)
          .OrderBy(a => a.QuestionKey())
          .ThenBy(a => a.Sequence())
          .GroupBy(a => a.QuestionKey());
        var questions = new ArrayList();
        record.Add(fieldName, questions);
        foreach (var questionGroup in subset)
        {
          var question = new Hashtable();
          questions.Add(question);
          question.Add("question", questionGroup.First().Question());
          var answers = new ArrayList();
          question.Add("answers", answers);
          foreach (var answerRow in questionGroup)
            AddAnswer(answers, answerRow);
        }
      }

      private void AddAnswer(IList answers, DataRow answerRow)
      {
        var answer = new Hashtable();
        answers.Add(answer);
        AddField(answer, "answer", answerRow.Answer());
        AddField(answer, "youTubeUrl", answerRow.YouTubeUrl());
        AddField(answer, "youTubeDescription", answerRow.YouTubeDescription());
        AddField(answer, "youTubeDate", FormatDate(answerRow.YouTubeDate()));
        AddField(answer, "youTubeRunningTime", FormatTime(answerRow.YouTubeRunningTime()));
      }

      private void AddIssues(IDictionary record, DataRow row,
        IEnumerable<DataRow> issueRows)
      {
        var subset = issueRows
          .Where(a => a.PoliticianKey().IsEqIgnoreCase(row.PoliticianKey()) &&
            a.IssueKey() != "ALLBio" && a.IssueKey() != "ALLPersonal")
          .OrderBy(a => a.IssueLevel())
          .ThenBy(a => a.IssueOrder())
          .ThenBy(a => a.IssueKey())
          .ThenBy(a => a.QuestionOrder())
          .ThenBy(a => a.QuestionKey())
          .ThenBy(a => a.Sequence())
          .GroupBy(a => a.IssueKey());
        var issues = new ArrayList();
        record.Add("issues", issues);
        foreach (var issueGroup in subset)
        {
          var issue = new Hashtable();
          issues.Add(issue);
          issue.Add("issue", issueGroup.First().Issue());
          var questionGroups = issueGroup.GroupBy(r => r.QuestionKey());
          var questions = new ArrayList();
          issue.Add("questions", questions);
          foreach (var questionGroup in questionGroups)
          {
            var question = new Hashtable();
            questions.Add(question);
            question.Add("question", questionGroup.First().Question());
            var answers = new ArrayList();
            question.Add("answers", answers);
            foreach (var answerRow in questionGroup)
              AddAnswer(answers, answerRow);
          }
        }
      }

      private void AddCandidateFields(IList candidatesArray,
        IGrouping<string, DataRow> officeGroup, IList<DataRow> issues)
      {
        foreach (var politicianRow in officeGroup.Where(r => !r.IsRunningMate()))
        {
          var candidateRecord = new Hashtable();
          candidatesArray.Add(candidateRecord);
          FillCandidateRecord(candidateRecord, politicianRow, issues);
          var runningMateRow = officeGroup.FirstOrDefault(r =>
            r.PoliticianKey().IsEqIgnoreCase(politicianRow.RunningMateKey()));
          if (runningMateRow != null)
          {
            var runningMateRecord = new Hashtable();
            candidateRecord.Add("runningMate", runningMateRecord);
            FillCandidateRecord(runningMateRecord, runningMateRow, issues);
          }
        }
      }

      private void AddOfficeFields(IDictionary electionsRecord, string electionKey)
      {
        var reportData =
          Elections.GetElectionReportData(electionKey)
            .Rows.OfType<DataRow>()
            .Where(r => !IsNullOrWhiteSpace(r.PoliticianKey()))
            .ToList();
        var reportGroups =
          reportData
            .OrderBy(r => r.AlternateOfficeLevel())
            .ThenBy(r => r.OfficeLevel())
            .ThenBy(r => r.DistrictCode())
            .ThenBy(r => r.OfficeOrderWithinLevel())
            .ThenBy(r => r.OfficeLine1())
            .ThenBy(r => r.OrderOnBallot())
            .GroupBy(r => r.OfficeKey());
        List<DataRow> issues = null;
        if (_Bio || _Reasons || _Issues)
        {
          var politicians =
            reportData.Select(r => r.PoliticianKey())
              .Union(
                reportData.Where(r => !IsNullOrWhiteSpace(r.RunningMateKey()))
                  .Select(r => r.RunningMateKey()))
              .Distinct();
          issues =
            Answers.GetActiveDataByPoliticianKeys(politicians)
              .Rows.OfType<DataRow>()
              .ToList();
        }
        var officesArray = new ArrayList();
        electionsRecord.Add("offices", officesArray);
        foreach (var officeGroup in reportGroups)
        {
          var officeRow = officeGroup.First();
          var officeRecord = new Hashtable();
          officesArray.Add(officeRecord);
          AddField(officeRecord, "officeKey", officeRow.OfficeKey());
          AddField(officeRecord, "officeLine1", officeRow.OfficeLine1());
          AddField(officeRecord, "officeLine2", officeRow.OfficeLine2());
          AddField(officeRecord, "isRunningMateOffice", officeRow.IsRunningMateOffice());
          if (_Candidates)
          {
            var candidatesArray = new ArrayList();
            officeRecord.Add("candidates", candidatesArray);
            AddCandidateFields(candidatesArray, officeGroup, issues);
          }
        }
      }

      private void AddElectionFields(IDictionary electionsRecord, ElectionsRow electionsRow)
      {
        AddField(electionsRecord, "electionKey", electionsRow.ElectionKey);
        AddField(electionsRecord, "stateCode", electionsRow.StateCode);
        AddField(electionsRecord, "electionDate", FormatDate(electionsRow.ElectionDate));
        AddField(electionsRecord, "electionType", electionsRow.ElectionType);
        AddField(electionsRecord, "nationalPartyCode", electionsRow.NationalPartyCode);
        AddField(electionsRecord, "electionDesc", electionsRow.ElectionDesc);
        AddField(electionsRecord, "electionAdditionalInfo", electionsRow.ElectionAdditionalInfo);
        if (_Dates)
        {
          AddField(electionsRecord, "registrationDeadline", FormatDate(electionsRow.RegistrationDeadline));
          AddField(electionsRecord, "earlyVotingBegin", FormatDate(electionsRow.EarlyVotingBegin));
          AddField(electionsRecord, "earlyVotingEnd", FormatDate(electionsRow.EarlyVotingEnd));
          AddField(electionsRecord, "mailBallotBegin", FormatDate(electionsRow.MailBallotBegin));
          AddField(electionsRecord, "mailBallotEnd", FormatDate(electionsRow.MailBallotEnd));
          AddField(electionsRecord, "mailBallotDeadline", FormatDate(electionsRow.MailBallotDeadline));
          AddField(electionsRecord, "absenteeBallotBegin", FormatDate(electionsRow.AbsenteeBallotBegin));
          AddField(electionsRecord, "absenteeBallotEnd", FormatDate(electionsRow.AbsenteeBallotEnd));
          AddField(electionsRecord, "absenteeBallotDeadline", FormatDate(electionsRow.AbsenteeBallotDeadline));
        }
      }

      public Hashtable Build(string electionKey)
      {
        try
        {
          var electionsRecord = new Hashtable();
          var electionsTable = Elections.GetData(electionKey);
          if (electionsTable.Count != 1 || electionsTable[0].CountyCode != Empty || 
            !electionsTable[0].IsViewable)
            electionsRecord.Add("error", "Invalid electionKey");
          else
          {
            var electionsRow = electionsTable[0];
            AddElectionFields(electionsRecord, electionsRow);
            if (_Offices)
              AddOfficeFields(electionsRecord, electionKey);
          }
          return electionsRecord;
        }
        catch
        {
          var result = new Hashtable {{"error", "an unrecoverable error occurred"}};
          return result;
        }
      }
    }

    [WebMethod]
    public void Election(string key, string data)
    {
      var builder = new BuildElection(data);
      HttpContext.Current.Response.Write(new JavaScriptSerializer().Serialize(builder.Build(key)));
    }
  }
}
