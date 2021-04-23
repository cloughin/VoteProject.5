using System;
using DB.Vote;
using static System.String;

namespace Vote.Politician
{
  public partial class MainPage : SecurePoliticianPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Master.NoMenu = true;
        Page.Title = PoliticianName;
        var liveOfficeKey = Politicians.GetLiveOfficeKey(PoliticianKey, Empty);
        var isPresidentialCandidate = String.Equals(liveOfficeKey, Offices.USPresident,
          StringComparison.OrdinalIgnoreCase);
        H1.InnerHtml = PoliticianName;
        H2.InnerHtml = OfficeAndStatus;
        if (!isPresidentialCandidate)
          H3.InnerHtml = Elections.GetElectionDesc(liveOfficeKey);
        MainImage.ImageUrl = NoCacheImageProfile200Url;
        UpdateIntroLink.Attributes["href"] = UpdateIntroPageUrl;
        ShowIntroLink.Attributes["href"] = IntroPageUrl;
        UpdateIssuesLink.Attributes["href"] = UpdateInfoPageUrl;
        var liveElectionKey = Politicians.GetLiveElectionKey(PoliticianKey, Empty);
        if (IsNullOrWhiteSpace(liveOfficeKey) || isPresidentialCandidate)
          CompareLink.Visible = false;
        else
          ShowCompareLink.HRef = UrlManager
            .GetCompareCandidatesPageUri(liveElectionKey, liveOfficeKey).ToString();
      }
    }
  }
}