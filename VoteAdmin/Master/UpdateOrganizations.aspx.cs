using System;
using Vote;
using static System.String;

namespace VoteAdmin.Master
{
  public partial class UpdateOrganizationsPage : SecurePage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        const string title = "Update Organizations";
        Page.Title = title;
        H1.InnerHtml = title;
        StateCache.Populate(OrganizationStates, "<Select State>", Empty);
        StateCache.Populate(ReportStates, "All States", Empty);
        HomePageLink.HRef = Utility.GetAdjustedSiteUri(null, "ad=0");
      }
    }
  }
}