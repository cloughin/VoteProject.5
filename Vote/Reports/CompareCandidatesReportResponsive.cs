using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class CompareCandidatesReportResponsive : ResponsiveIssuesReport
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
          string.Compare(row.PoliticianKey(), _PoliticianKey, StringComparison.OrdinalIgnoreCase) ==
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
        DataTable = ElectionsPoliticians.GetCompareCandidateIssues(electionKey, officeKey,
          questionKey);
      }

      public void GetCandidateVideoData(string electionKey, string officeKey, string politicianKey)
      {
        DataTable = ElectionsPoliticians.GetCompareCandidateVideos(electionKey, officeKey,
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
        var isRunningMateOffice = candidates[0].IsRunningMateOffice() &&
          !Elections.IsPrimaryElection(electionKey);

        var candidateKeys = GetCandidateKeys(isRunningMateOffice, candidates);

        _IssuesDataManager.GetData(electionKey, officeKey, questionKey);
        CreateOneAnswerContent(container, candidateKeys, _IssuesDataManager.GetDataSubset(),
          isRunningMateOffice, questionKey, false);
      }

      return container;
    }

    private Control GenerateOneCandidateVideoContent(string electionKey, string officeKey,
      string politicianKey)
    {
      var container = new PlaceHolder();
      _IssuesDataManager.GetCandidateVideoData(electionKey, officeKey, politicianKey);
      var issuesData = _IssuesDataManager.GetDataSubset();

      var isRunningMateOffice = Offices.GetIsRunningMateOffice(officeKey, false) &&
        !Elections.IsPrimaryElection(electionKey);

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

      if (!string.IsNullOrWhiteSpace(runningMateKey))
      {
        var runningMateVideos = issuesData
          .Where(r => r.PoliticianKey().IsNeIgnoreCase(politicianKey)).ToList();
        qas = GetQuestionAndAnswerList(runningMateVideos, runningMate, true, true);
        if (qas.Any())
          ReportCandidateVideos(container, runningMate, qas, runningMateName, true);
      }

      return container;
    }

    private Control GenerateReport(string electionKey, string officeKey)
    {
      _CandidatesDataManager.GetData(electionKey, officeKey);
      var candidates = _CandidatesDataManager.GetDataSubset(new RunningMateFilter());

      if (candidates.Count > 0)
      {
        var isRunningMateOffice = candidates[0].IsRunningMateOffice() &&
          !Elections.IsPrimaryElection(electionKey);
        var candidatesContainer = new HtmlDiv()
          .AddTo(ReportContainer, "candidates-container office-cell");
        candidatesContainer.Attributes.Add("data-key", officeKey.ToUpperInvariant());
        var positions = GetElectionPositions(electionKey, candidates[0]);
        if (positions > 1)
          candidatesContainer.Attributes.Add("data-positions",
            positions.ToString(CultureInfo.InvariantCulture));
        ReportOffice(candidatesContainer, isRunningMateOffice, candidates, _CandidatesDataManager,
          true, true);

        var issueListLink = new HtmlDiv().AddTo(ReportContainer, "issue-list-link");
        new HtmlAnchor
        {
          HRef = UrlManager.GetIssueListPageUri(Offices.GetStateCodeFromKey(officeKey)).ToString(),
          InnerText = "View a list of all questions available to the candidates"
        }.AddTo(issueListLink);
        new LiteralControl(
          ". Questions are included below only if at least one candidate responded.").AddTo(
          issueListLink);

        var candidateKeys = GetCandidateKeys(isRunningMateOffice, candidates);

        _IssuesDataManager.GetData(electionKey, officeKey);
        ReportIssues(candidateKeys, _IssuesDataManager.GetDataSubset(), isRunningMateOffice,
          candidates.Count);
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

    public static Control GetReport(string electionKey, string officeKey)
    {
      var reportObject = new CompareCandidatesReportResponsive();
      return reportObject.GenerateReport(electionKey, officeKey);
    }
  }
}