using System;
using System.Web;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class IssuesReportPage : SecureAdminPage, IAllowEmptyStateCode
  {
    protected override void OnPreInit(EventArgs e)
    {
      var query = HttpContext.Current.Request.QueryString.ToString();
      var path = "/admin/UpdateIssues.aspx";
      if (!IsNullOrWhiteSpace(query)) path += "?" + query;
      Response.Redirect(path);
      Response.End();
      //base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = H1.InnerText = "Issues Report";
        var issueLevel = GetQueryString("IssueLevel");
        var group = GetQueryString("Group");

        if (IsNullOrWhiteSpace(issueLevel) || IsNullOrWhiteSpace(group))
          HandleFatalError("IssueLevel and/or Group is missing");

        IssuesReport.GetReport(issueLevel, group)
         .AddTo(ReportPlaceHolder);
      }
    }
  }
}