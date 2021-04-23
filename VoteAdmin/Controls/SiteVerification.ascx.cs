using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class SiteVerification : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        GoogleSiteVerificationTag.Attributes["content"] = UrlManager.CurrentGoogleVerificationCode;
        YahooSiteVerificationTag.Attributes["content"] = UrlManager.CurrentYahooVerificationCode;
        BingSiteVerificationTag.Attributes["content"] = UrlManager.CurrentBingVerificationCode;
      }
    }
  }
}