using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class DonationRequestResponsive : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Page.IncludeJs("~/js/jq/jquery.scrollintoview.js");
      Page.IncludeJs("~/js/vote.js");
      Page.IncludeJs("~/js/DonationRequestResponsive.js");
      DonateLink.HRef = VotePage.DonateUrl;
    }
  }
}