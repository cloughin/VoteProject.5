using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Reports;

namespace Vote
{
  public partial class Test : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      //ReportPlaceHolder.Controls.Add(IssueReportBiographical.GetReport(db.QueryString("Election"), db.QueryString("Office")));
      //var start0 = DateTime.UtcNow;
      //ReportPlaceHolder.Controls.Add(IssueReportIssueList.GetReportByOfficeKey(db.QueryString("Office"), db.QueryString("Election")));
      //ReportPlaceHolder.Controls.Add(IssueReportIssueList.GetReportByPoliticianKey("VAMillerJacksonH", "VA20061107GA"));
      //ReportPlaceHolder.Controls.Add(IssueReportIssueList.GetReportByPoliticianKey("ILObamaBarack"));
      //ReportPlaceHolder.Controls.Add(IssueReportIssueList.GetReportByStateCode("VA"));
      //ReportPlaceHolder.Controls.Add(PoliticianInfo.GetReport("VAWolfFrankR"));
      //var politicianInfo = Politicians.GetPoliticianIssueReportData("VAWolfFrankR");
      //ReportPlaceHolder.Controls.Add(PoliticianInfo.GetReport(politicianInfo));
      //var elapsed0 = DateTime.UtcNow - start0;
      //var start = DateTime.UtcNow;
      //ReportPlaceHolder.Controls.Add(IssueReportIssues.GetReport(db.QueryString("Election"),
      //  db.QueryString("Office"), db.QueryString("Issue"), out answerCount));
      //var elapsed = DateTime.UtcNow - start;
    }
  }
}