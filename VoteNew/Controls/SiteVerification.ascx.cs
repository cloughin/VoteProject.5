using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;

namespace VoteNew.Controls
{
  public partial class SiteVerification : System.Web.UI.UserControl
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