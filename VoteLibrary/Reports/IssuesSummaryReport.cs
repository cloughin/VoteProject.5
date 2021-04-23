using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public class IssuesSummaryReport : TableBasedReport 
  {
    #region Private

    private readonly IssuesSummaryReportDataManager _DataManager =
      new IssuesSummaryReportDataManager();

    #region Private classes

    private sealed class IssuesSummaryReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string stateCode)
      {
        DataTable = Issues.GetIssuesSummaryReportData(stateCode);
      }
    }

    protected void CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetailHeading");
      new HtmlTableCell { InnerText = "Order" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Issue" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Omit" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "IssueKey" }.AddTo(tr, "tdReportDetailHeading");
      new HtmlTableCell { InnerText = "Questions" }.AddTo(tr, "tdReportDetailHeading");
    }

    protected void ReportIssue(DataRow row)
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetail");
      new HtmlTableCell { InnerText = row.IssueOrder().ToString() }
        .AddTo(tr, "tdReportDetail");
      var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetIssuesPageUrl(row.StateCode(), row.IssueKey()),
        InnerHtml = $"<nobr>{row.Issue()}</nobr>"
      }.AddTo(td);
      td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      new HtmlAnchor
      {
        HRef = SecureAdminPage.GetIssuesPageUrl(row.StateCode(), row.IssueKey(), null,
          !row.IsIssueOmit()),
        InnerHtml = row.IsIssueOmit() ? "<strong>OMIT</strong>" : "ok"
      }.AddTo(td);
      new HtmlTableCell { InnerText = row.IssueKey() }
        .AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerText = row.Count().ToString() }
        .AddTo(tr, "tdReportDetail");
    }

    #endregion Private classes

    private Control GenerateReport(string stateCode)
    {
      ReportUser = ReportUser.Admin;
      _DataManager.GetData(stateCode);
      var data = _DataManager.GetDataSubset();

      StartNewHtmlTable();
      CreateHeadingRow();
      CurrentHtmlTable.RemoveCssClass("tableAdmin");
      foreach (var issue in data)
        ReportIssue(issue);

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string stateCode)
    {
      var reportObject = new IssuesSummaryReport();
      return reportObject.GenerateReport(stateCode);
    }
  }
}
