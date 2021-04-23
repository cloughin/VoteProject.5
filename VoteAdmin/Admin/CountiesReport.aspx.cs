using System;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class CountiesReportPage : SecureAdminPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      H1.InnerHtml = Page.Title =
        $"County Contacts Report for {StateCache.GetStateName(StateCode)}";

      //if (!IsMasterUser) HandleSecurityException();

      PlaceHolder.Controls.Add(CountiesReport.GetReport(StateCode));
    }
  }
}