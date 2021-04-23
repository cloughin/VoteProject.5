using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew.Controls
{
  public partial class MainBanner : System.Web.UI.UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeCss("~/css/MainBanner.css");

      if (!IsPostBack)
      {
        MainBannerImageTag.Src = DomainDesign.GetImageUri("lgbanner.png").ToString();
        MainBannerHomeLink.HRef = UrlManager.CurrentSiteUri.ToString();

        if (DomainData.IsValidStateCode)
        {
          string stateCode = DomainData.FromQueryStringOrDomain;
          string stateName = StateCache.GetStateName(stateCode);
          MainBannerStateName.Src = DomainDesign.GetImageUri("name.png").ToString();
          MainBannerStateName.Alt = stateName;
          MainBannerStateSiteLink.InnerText = "Official " + stateName + " Website →";
          MainBannerStateSiteLink.Title = "Official " + stateName + " Website";
          MainBannerStateSiteLink.HRef = StateCache.GetUri(stateCode).ToString();
        }
        else
          MainBannerStateBanner.Visible = false;
      }
    }
  }
}