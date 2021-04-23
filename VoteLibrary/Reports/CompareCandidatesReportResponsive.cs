using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class CompareCandidatesReportResponsive : ResponsiveIssuesReport
  {
    #region Private

    private class CandidateFilter : ReportDataManager<DataRow>.FilterBy
    {
      private readonly string _PoliticianKey;

      public CandidateFilter(string politicianKey)
      {
        _PoliticianKey = politicianKey;
      }

      public override bool Filter(DataRow row)
      {
        return
          Compare(row.PoliticianKey(), _PoliticianKey, StringComparison.OrdinalIgnoreCase) ==
          0;
      }
    }

    private class RunningMateFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !row.IsRunningMate();
      }
    }

    private readonly CandidatesDataManager _CandidatesDataManager =
      new CandidatesDataManager();

    private readonly IssuesDataManager _IssuesDataManager =
      new IssuesDataManager();

    #region Private classes

    private sealed class CandidatesDataManager : RunningMateDataManager
    {
      public void GetData(string electionKey, string officeKey)
      {
        DataTable = Elections.GetCompareCandidatesReportData(electionKey, officeKey);
      }
    }

    private sealed class IssuesDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string electionKey, string officeKey, string questionKey = null)
      {
        int? questionId = null;
        if (int.TryParse(questionKey, out var id))
          questionId = id;
        DataTable = VotePage.EnableIssueGroups
          ? ElectionsPoliticians.GetCompareCandidateIssuesNew3(electionKey, officeKey,
            questionId)
          : ElectionsPoliticians.GetCompareCandidateIssuesNew(electionKey, officeKey,
            questionId);
      }

      public void GetCandidateVideoData(string electionKey, string officeKey, string politicianKey)
      {
        DataTable = VotePage.EnableIssueGroups
          ? ElectionsPoliticians.GetCompareCandidateVideosNew3(electionKey, officeKey,
            politicianKey)
          : ElectionsPoliticians.GetCompareCandidateVideosNew(electionKey, officeKey,
            politicianKey);
      }
    }

    #endregion Private classes

    private Control GenerateOneAnswerContent(string electionKey, string officeKey,
      string questionKey)
    {
      _CandidatesDataManager.GetData(electionKey, officeKey);
      var candidates = _CandidatesDataManager.GetDataSubset(new RunningMateFilter());
      var container = new PlaceHolder();

      if (candidates.Count > 0)
      {
        //var isRunningMateOffice = candidates[0].IsRunningMateOffice() &&
        //  !Elections.IsPrimaryElection(electionKey);
        var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(electionKey, candidates[0].OfficeKey());

        var candidateKeys = GetCandidateKeys(isRunningMateOffice, candidates);

        _IssuesDataManager.GetData(electionKey, officeKey, questionKey);
        var oldAnswerCutoff =
          ElectionsOffices.GetOldAnswerCutoffDate(electionKey, officeKey);
        CreateOneAnswerContent(container, candidateKeys, _IssuesDataManager.GetDataSubset(),
          isRunningMateOffice, questionKey, false, oldAnswerCutoff);
      }

      return container;
    }

    private Control GenerateOneCandidateVideoContent(string electionKey, string officeKey,
      string politicianKey)
    {
      var container = new PlaceHolder();
      _IssuesDataManager.GetCandidateVideoData(electionKey, officeKey, politicianKey);
      // for older elections show all responese
      var electionDate = Elections.GetElectionDateFromKey(electionKey);
      var oldAnswerCutoff = electionDate > DateTime.UtcNow.AddMonths(-6)
        ? ElectionsOffices.GetOldAnswerCutoffDate(electionKey, officeKey)
        : DateTime.MinValue;
      var issuesData = _IssuesDataManager.GetDataSubset()
        .Where(r => r.YouTubeDate() >= oldAnswerCutoff);

      //var isRunningMateOffice = Offices.GetIsRunningMateOffice(officeKey, false) &&
      //  !Elections.IsPrimaryElection(electionKey);
      var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(electionKey, officeKey);

      var politician = Politicians.GetPoliticianIntroReportData(politicianKey);
      DataRow runningMate = null;

      var name = Politicians.FormatName(politician);
      string runningMateKey = null;
      string runningMateName = null;
      if (isRunningMateOffice)
      {
        //var cache = PageCache.GetTemporary();
        //name = cache.Politicians.GetPoliticianName(politicianKey);
        runningMateKey = ElectionsPoliticians
          .GetRunningMateKeyByElectionKeyOfficeKeyPoliticianKey(
            electionKey, officeKey, politicianKey);
        runningMate = Politicians.GetPoliticianIntroReportData(runningMateKey);
        //runningMateName = cache.Politicians.GetPoliticianName(runningMateKey);
        runningMateName = Politicians.FormatName(runningMate);
      }

      var videos = issuesData
        .Where(r => r.PoliticianKey().IsEqIgnoreCase(politicianKey)).ToList();
      var qas = GetQuestionAndAnswerList(videos, politician, true, true);
      if (qas.Any())
        ReportCandidateVideos(container, politician, qas, name, isRunningMateOffice);

      if (!IsNullOrWhiteSpace(runningMateKey))
      {
        var runningMateVideos = issuesData
          .Where(r => r.PoliticianKey().IsNeIgnoreCase(politicianKey)).ToList();
        qas = GetQuestionAndAnswerList(runningMateVideos, runningMate, true, true);
        if (qas.Any())
          ReportCandidateVideos(container, runningMate, qas, runningMateName, true);
      }

      return container;
    }

    private Control GenerateReport(string electionKey, string officeKey, bool forBallotPage = false)
    {
      _CandidatesDataManager.GetData(electionKey, officeKey);
      var candidates = _CandidatesDataManager.GetDataSubset(new RunningMateFilter());

      if (candidates.Count > 0)
      {
        var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(electionKey, candidates[0].OfficeKey());

        if (!forBallotPage)
        {
          var candidatesContainer = new HtmlDiv()
            .AddTo(ReportContainer, "candidates-container office-cell");
          candidatesContainer.Attributes.Add("data-key", officeKey.ToUpperInvariant());
          var positions = GetElectionPositions(electionKey, candidates[0]);
          if (positions > 1)
            candidatesContainer.Attributes.Add("data-positions",
              positions.ToString(CultureInfo.InvariantCulture));
          ReportOffice(candidatesContainer, isRunningMateOffice, candidates, _CandidatesDataManager,
            true, true);
        }

        CreateIssueListLink(officeKey);

        var candidateKeys = GetCandidateKeys(isRunningMateOffice, candidates);

        _IssuesDataManager.GetData(electionKey, officeKey);
        if (VotePage.EnableIssueGroups)
          ReportIssues3(candidateKeys, _IssuesDataManager.GetDataSubset(),
            isRunningMateOffice, candidates.Count);
        else
        {
          // for older elections show all responese
          var electionDate = Elections.GetElectionDateFromKey(electionKey);
          var oldAnswerCutoff = electionDate > DateTime.UtcNow.AddMonths(-6) 
            ? ElectionsOffices.GetOldAnswerCutoffDate(electionKey, officeKey)
            : DateTime.MinValue;
          ReportIssues(candidateKeys, _IssuesDataManager.GetDataSubset(), oldAnswerCutoff,
            isRunningMateOffice, candidates.Count);
        }
      }

      return ReportContainer.AddCssClasses("compare-candidates-report ballot-checks-container");
    }

    private List<DataRow> GetCandidateKeys(bool isRunningMateOffice, IEnumerable<DataRow> candidates)
    {
      // create a list of politicians to reproduce order and create empty cells when 
      // reporting issues
      var candidateKeys = (isRunningMateOffice
          ? candidates.SelectMany(c => new[]
            {
              c.PoliticianKey(),
              c.RunningMateKey()
            })
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList()
          : candidates.Select(c => c.PoliticianKey()).Distinct(StringComparer.OrdinalIgnoreCase))
        .Select(k => _CandidatesDataManager.GetDataSubset(new CandidateFilter(k)).FirstOrDefault())
        .ToList();
      return candidateKeys;
    }

    #endregion Private

    public static Control GetOneAnswerContent(string electionKey, string officeKey,
      string questionKey)
    {
      var reportObject = new CompareCandidatesReportResponsive();
      return reportObject.GenerateOneAnswerContent(electionKey, officeKey, questionKey);
    }

    public static Control GetOneCandidateVideoContent(string electionKey, string officeKey,
      string politicianKey)
    {
      var reportObject = new CompareCandidatesReportResponsive();
      return reportObject.GenerateOneCandidateVideoContent(electionKey, officeKey, politicianKey);
    }

    public static Control GetReport(string electionKey, string officeKey, bool forBallotPage = false)
    {
      var reportObject = new CompareCandidatesReportResponsive();
      return reportObject.GenerateReport(electionKey, officeKey, forBallotPage);
    }
  }
}