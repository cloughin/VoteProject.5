using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Reports;

namespace Vote.Sandbox
{
  public partial class _default : System.Web.UI.Page
  {
    protected override void OnPreInit(EventArgs e)
    {
      //Response.Redirect("/sandbox/TestPoliticiansAdminReport.aspx");
      base.OnPreInit(e);
    }

    protected void xPage_Load(object sender, EventArgs e)
    {
      var reportUser = ReportUser.Public;
      var start = DateTime.UtcNow;
      //var report = OfficialsReport.GetReport(reportUser, "VA", "059", "14");
      //var report = OfficialsReport.GetReport(reportUser, "VA", "059");
      var report = OfficialsReport.GetReport(reportUser, "DC");
      //var table =
      //  Offices.GetOfficialsReportData(new OfficialsReportOptions
      //    {
      //      ElectoralLevel = ElectoralLevel.State,
      //      StateCode = "VA"
      //    });
      var elapsed = DateTime.UtcNow - start;

      PlaceHolder placeHolder;

      if (reportUser == ReportUser.Public)
      {
        PublicCss.Visible = true;
        placeHolder = PublicPlaceHolder;
      }
      else
      {
        NonPublicCss.Visible = true;
        placeHolder = NonPublicPlaceHolder;
      }

      placeHolder.Controls.Add(report);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      // ?election=IN20121106GA
      // ?election=U220121106GA
      // ?election=PP20121106AD

      var electionKey = Request.QueryString["election"];
      if (string.IsNullOrWhiteSpace(electionKey)) return;

      var user = Request.QueryString["user"].SafeString();

      ReportUser reportUser;
      switch (user.ToLowerInvariant())
      {
        case "master":
          reportUser = ReportUser.Master;
          break;

        case "admin":
          reportUser = ReportUser.Admin;
          break;

        default:
          reportUser = ReportUser.Public;
          break;
      }

      var start = DateTime.UtcNow;
      var report = ElectionReport.GetReport(reportUser, electionKey);
      var elapsed = DateTime.UtcNow - start;

      PlaceHolder placeHolder;

      if (reportUser == ReportUser.Public)
      {
        ElectionPublicCss.Visible = true;
        placeHolder = PublicPlaceHolder;
      }
      else
      {
        ElectionNonPublicCss.Visible = true;
        placeHolder = NonPublicPlaceHolder;
      }

      placeHolder.Controls.Add(report);
    }
  }
}