using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public class QuestionsSummaryReport : TableBasedReport 
  {
    #region Private

    private readonly QuestionsSummaryReportDataManager _DataManager =
      new QuestionsSummaryReportDataManager();

    #region Private classes

    private sealed class QuestionsSummaryReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string issueKey)
      {
        DataTable = Questions.GetQuestionsSummaryReportData(issueKey);
      }
    }

    protected void CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetailHeading");
      new HtmlTableCell { InnerText = "Order" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Question" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Omit" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "QuestionKey" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Answers" }.AddTo(tr, "tdReportDetailHeading");
    }

    protected void ReportQuestion(DataRow row)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetail");
      new HtmlTableCell { InnerText = row.QuestionOrder().ToString() }
        .AddTo(tr, "tdReportDetail");
      var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetIssuesPageUrl(Issues.GetStateCodeFromKey(row.IssueKey()), 
         row.IssueKey(), row.QuestionKey()),
        InnerHtml = $"<nobr>{row.Question()}</nobr>"
      }.AddTo(td);
      td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetIssuesPageUrl(Issues.GetStateCodeFromKey(row.IssueKey()), 
          row.IssueKey(), row.QuestionKey(), !row.IsQuestionOmit()),
        InnerHtml = row.IsQuestionOmit() ? "<strong>OMIT</strong>" : "ok"
      }.AddTo(td);
      new HtmlTableCell { InnerText = row.QuestionKey() }
        .AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.Count().ToString() }
        .AddTo(tr, "tdReportDetail");
    }

    #endregion Private classes

    private Control GenerateReport(string issueKey)
    {
      ReportUser = ReportUser.Admin;
      _DataManager.GetData(issueKey);
      var data = _DataManager.GetDataSubset();

      StartNewHtmlTable();
      CreateHeadingRow();
      CurrentHtmlTable.RemoveCssClass("tableAdmin");
      foreach (var issue in data)
        ReportQuestion(issue);

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string issueKey)
    {
      var reportObject = new QuestionsSummaryReport();
      return reportObject.GenerateReport(issueKey);
    }
  }
}
