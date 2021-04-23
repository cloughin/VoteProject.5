using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public class IssuesReport : TableBasedReport 
  {
    #region Private

    private readonly IssuesReportDataManager _DataManager =
      new IssuesReportDataManager();

    #region Private classes

    private sealed class IssuesReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string issueLevel, string group)
      {
        DataTable = Issues.GetIssuesReportData(issueLevel, group);
      }
    }

    protected void CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "HeadingRow");
      new HtmlTableCell
      {
        InnerHtml = "Issue on Navbar" + 
          "<br>[Issue ID] O D"
      }.AddTo(tr, "tdReportGroupHeadingLeft");
      new HtmlTableCell
      {
        InnerHtml = "Question (Answers) ...[Question ID] O D" +
          "<br>(What is your position and views regarding)"
      }.AddTo(tr, "tdReportGroupHeadingLeft");
    }

    protected void ReportIssue(IList<DataRow> rows)
    {
      var issueRow = rows.First();
      var issue = $"<hr>{issueRow.Issue()}<br>[{issueRow.IssueKey()}]";
      if (issueRow.IsIssueOmit()) issue += " O";

      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "PartyRow");
      new HtmlTableCell { InnerHtml = issue }.AddTo(tr, "tdReportDetail");
      new HtmlTableCell { InnerHtml = "<hr>" }.AddTo(tr, "tdReportDetail");

      foreach (var row in rows)
        if (row.QuestionKey() != null)
        {
          // single null row indicates issue with no questions
          var question = $"{row.Question()} ({row.Count()}) ...[{row.QuestionKey()}]";
          if (row.IsQuestionOmit()) question += " O";
          tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "PartyRow");
          new HtmlTableCell { InnerHtml = "&nbsp;" }.AddTo(tr, "tdReportDetail");
          new HtmlTableCell { InnerHtml = question }.AddTo(tr, "tdReportDetail");
        }
    }

    #endregion Private classes

    private Control GenerateReport(string issueLevel, string group)
    {
      _DataManager.GetData(issueLevel, group);
      var data = _DataManager.GetDataSubset()
        .GroupBy(r => r.IssueKey())
        .ToList();

      StartNewHtmlTable();
      CurrentHtmlTable.AddCssClasses("tableAdmin");

      if (data.Count == 0)
      {
        new HtmlTableCell
        {
          InnerHtml = "No Data Found"
        }.AddTo(new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading center"),
          "tdReportGroupHeading");
      }
      else
      {
        CreateHeadingRow();
        foreach (var issueGroup in data)
          ReportIssue(issueGroup.ToList());
      }

      return ReportContainer;
    }

    #endregion Private

    public static Control GetReport(string issueLevel, string group)
    {
      var reportObject = new IssuesReport();
      return reportObject.GenerateReport(issueLevel, group);
    }
  }
}
