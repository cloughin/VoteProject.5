using System;
using Vote;

namespace VoteAdmin.Master
{
  public partial class SetupHomePageBannerAd2 : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.Title = "Setup Home Page Banner Ad";
      H1.InnerHtml = "Setup Home Page Banner Ad";
    }
  }
}