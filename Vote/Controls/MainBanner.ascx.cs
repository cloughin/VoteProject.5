using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class MainBanner : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeCss("~/css/MainBanner.css");

      if (!IsPostBack)
      {
        MainBannerImageTag.Src = DomainDesign.GetImageUri("lgbanner.png")
          .ToString();
        MainBannerHomeLink.HRef = UrlManager.CurrentSiteUri.ToString();

        if (DomainData.IsValidStateCode)
        {
          var stateCode = DomainData.FromQueryStringOrDomain;
          var stateName = StateCache.GetStateName(stateCode);
          MainBannerStateName.Src = DomainDesign.GetImageUri("name.png")
            .ToString();
          MainBannerStateName.Alt = stateName;
          var title = "Click to go to the Official " + stateName +
            " Election Authority Website";
          //MainBannerStateSiteLink.Title = title;
          MainBannerStateSiteLink.InnerText = title + " →";
          MainBannerStateSiteLink.HRef = StateCache.GetUri(stateCode)
            .ToString();
        }
        else
          MainBannerStateBanner.Visible = false;
      }
    }
  }
}