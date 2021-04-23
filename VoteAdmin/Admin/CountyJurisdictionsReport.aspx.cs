using System;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class CountyJurisdictionsReportPage : SecureAdminPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      H1.InnerHtml = Page.Title =
        $"County Jurisdictions Report for {StateCache.GetStateName(StateCode)}";
      PlaceHolder.Controls.Add(CountyJurisdictionsReport.GetReport(StateCode));
    }
  }
}