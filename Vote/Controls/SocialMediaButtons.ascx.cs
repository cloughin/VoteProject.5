using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class SocialMediaButtons : UserControl
  {
    private void SetSampleBallotVisibility()
    {
      SampleBallot.Visible = false;
      SpecialLinkLogo.Visible = false;
      SpecialLink.Visible = false;
      var cookies = LocationCookies.GetCookies();
      if (cookies.IsValid)
        TopOptionsOfficialsLink.HRef =
          UrlManager.GetElectedPageUri(
              cookies.State, cookies.Congress, cookies.StateSenate, cookies.StateHouse,
              cookies.County)
            .ToString();
      else
        //TopOptionsOfficials.Visible = false;
        TopOptions.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var votePage = Page as VotePage;
      if (votePage?.NoSocialMedia == true)
        AddThisButtons.Visible = false;
      else
        AddThisButtons.Visible = !VotePage.IsDebugging;
      Page.IncludeCss("~/css/SocialMediaButtons.css");
      SetSampleBallotVisibility();
    }
  }
}