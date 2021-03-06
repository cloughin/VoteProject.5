using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class IntroIssuesReport : ResponsiveIssuesReport
  {
    #region Private

    private readonly DataManager _DataManager = new DataManager();

    #region Private classes

    private sealed class DataManager : ReportDataManager<DataRow>
    {
      public void GetData(string politicianKey, string officeKey, string questionKey = null)
      {
        DataTable = Answers.GetAnswers(politicianKey, officeKey, questionKey);
      }

      public void GetVideoData(string politicianKey, string officeKey)
      {
        DataTable = Answers.GetVideoAnswers(politicianKey, officeKey);
      }
    }

    #endregion Private classes

    private Control GenerateOneAnswerContent(DataRow politician, string questionKey)
    {
      var container = new PlaceHolder();
      _DataManager.GetData(politician.PoliticianKey(), politician.LiveOfficeKey(), questionKey);
      CreateOneAnswerContent(container, new List<DataRow> {politician}, _DataManager.GetDataSubset(),
        false, questionKey, true);
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

      var issueListLink = new HtmlDiv().AddTo(ReportContainer, "issue-list-link");
      new HtmlAnchor
      {
        HRef =
          UrlManager.GetIssueListPageUri(Offices.GetStateCodeFromKey(politician.LiveOfficeKey()))
            .ToString(),
        InnerText = "View a list of all questions available to the candidate"
      }.AddTo(issueListLink);
      new LiteralControl(". Questions are included below only if the candidate responded.").AddTo(
        issueListLink);

      ReportIssues(new List<DataRow> {politician}, _DataManager.GetDataSubset(), false, 1, true);

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