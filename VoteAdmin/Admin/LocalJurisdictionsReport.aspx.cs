using System;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class LocalJurisdictionsReportPage : SecureAdminPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      H1.InnerHtml = Page.Title =
        $"Local Jurisdictions Report for {StateCache.GetStateName(StateCode)}";

      //if (!IsMasterUser) HandleSecurityException();

      PlaceHolder.Controls.Add(LocalJurisdictionsReport.GetReport(StateCode));
    }
  }
}