using System;
using DB.Vote;

namespace Vote.Politician
{
  public partial class MainPage : SecurePoliticianPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = PoliticianName;
        H1.InnerHtml = PoliticianName;
        H2.InnerHtml = OfficeAndStatus;
        H3.InnerHtml = Elections.GetElectionDesc(CurrentElectionKey);
        MainImage.ImageUrl = NoCacheImageProfile200Url;
        UpdateIntroLink.Attributes["href"] = UpdateIntroPageUrl;
        ShowIntroLink.Attributes["href"] = IntroPageUrl;
        UpdateIssuesLink.Attributes["href"] = UpdateIssuesPageUrl;
        ShowPoliticianIssueLink.Attributes["href"] = PoliticianIssuePageUrl;
      }
    }
  }
}