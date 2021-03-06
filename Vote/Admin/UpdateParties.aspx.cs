using System;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class UpdatePartiesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (AdminPageLevel == AdminPageLevel.Unknown)
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/UpdateParties.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
        }
      }
    }
  }
}