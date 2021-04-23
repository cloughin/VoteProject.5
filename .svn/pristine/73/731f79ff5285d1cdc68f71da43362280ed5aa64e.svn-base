using System;
using System.Web.UI;
using Vote.Reports;

namespace Vote.Sandbox
{
  public partial class tpi : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      int answerCount;
      var start0 = DateTime.UtcNow;
      //LinksPlaceHolder.Controls.Add(new TextTag(PoliticianIssueListLinks.Issue_Links(
      //  PageCache.GetTemporary(), db.QueryString("Id"))));
      ReportPlaceHolder.Controls.Add(
        PoliticianIssueReportIssues.GetReport(VotePage.QueryId,
          VotePage.QueryIssue, out answerCount));
      var elapsed0 = DateTime.UtcNow - start0;
      var start = DateTime.UtcNow;
      //LinksPlaceHolder2.Controls.Add(PoliticianIssueListLinks.GetReport(db.QueryString("Id")));
      var elapsed = DateTime.UtcNow - start;
    }
  }
}