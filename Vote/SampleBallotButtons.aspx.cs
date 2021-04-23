using System;

namespace Vote
{
  public partial class SampleBallotButtonsPage : PublicPage
  {
    protected SampleBallotButtonsPage()
    {
      NoUrlEdit = true;
      NoIndex = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Title = $"{PublicMasterPage.SiteName} | Get Interactive Ballot Choices Buttons";
    }
  }
}