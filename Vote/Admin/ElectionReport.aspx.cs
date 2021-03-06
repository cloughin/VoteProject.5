using System;
using System.Collections.Generic;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class ElectionReportPage : SecureAdminPage
  {
    internal override IEnumerable<string> NonStateCodesAllowed { get { return new[] { "U1", "U2", "U3", "U4", "US", "PP" }; } }

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.Title = "Election Report - Master Version";

      if (!IsMasterUser) HandleSecurityException();

      var electionKey = Request.QueryString["election"];
      if (string.IsNullOrWhiteSpace(electionKey)) return;

      PlaceHolder.Controls.Add(ElectionReport.GetReport(ReportUser.Master, electionKey));

    }
  }
}