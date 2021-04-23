using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public sealed class IntroIssuesReport : ResponsiveIssuesReport
  {
    #region Private

    private readonly DataManager _DataManager = new DataManager();

    #region Private classes

    private sealed class DataManager : ReportDataManager<DataRow>
    {
      public void GetData(string politicianKey, string officeKey, string questionKey = null)
      {
        int? questionId = null;
        if (int.TryParse(questionKey, out var id))
          questionId = id;
        DataTable = VotePage.EnableIssueGroups
          ? Answers.GetAnswersNew3(politicianKey, officeKey, questionId)
          : Answers.GetAnswersNew(politicianKey, officeKey, questionId);
      }

      public void GetVideoData(string politicianKey, string officeKey)
      {
        DataTable = VotePage.EnableIssueGroups
          ? Answers.GetVideoAnswersNew3(politicianKey, officeKey)
          : Answers.GetVideoAnswersNew(politicianKey, officeKey);
      }
    }

    #endregion Private classes

    private Control GenerateOneAnswerContent(DataRow politician, string questionKey)
    {
      var container = new PlaceHolder();
      _DataManager.GetData(politician.PoliticianKey(), politician.LiveOfficeKey(), questionKey);
      CreateOneAnswerContent(container, new List<DataRow> {politician}, _DataManager.GetDataSubset(),
        false, questionKey, true, DateTime.MinValue);
      return container;
    }

    private Control GenerateVideoContent(DataRow politician)
    {
      var container = new PlaceHolder();
      _DataManager.GetVideoData(politician.PoliticianKey(), politician.LiveOfficeKey());
      ReportCandidateVideos(container, politician, _DataManager.GetDataSubset(),
        Politicians.FormatName(politician), false);
      return container;
    }

    private Control GenerateReport(DataRow politician)
    {
      _DataManager.GetData(politician.PoliticianKey(), politician.LiveOfficeKey());

      CreateIssueListLink(politician.LiveOfficeKey());

      if (VotePage.EnableIssueGroups)
        ReportIssues3(new List<DataRow> { politician }, _DataManager.GetDataSubset(), false, 1,
          true);
      else
        ReportIssues(new List<DataRow> { politician }, _DataManager.GetDataSubset(), DateTime.MinValue, false, 1,
          true);

      return ReportContainer.AddCssClasses("intro-issues-report");
    }

    #endregion

    public static Control GetOneAnswerContent(string politicianKey, string questionKey)
    {
      var politician = Politicians.GetPoliticianIntroReportData(politicianKey);
      var reportObject = new IntroIssuesReport();
      return reportObject.GenerateOneAnswerContent(politician, questionKey);
    }

    public static Control GetVideoContent(string politicianKey)
    {
      var politician = Politicians.GetPoliticianIntroReportData(politicianKey);
      var reportObject = new IntroIssuesReport();
      return reportObject.GenerateVideoContent(politician);
    }

    public static Control GetReport(DataRow politician)
    {
      var reportObject = new IntroIssuesReport();
      return reportObject.GenerateReport(politician);
    }
  }
}