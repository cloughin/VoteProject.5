using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class DonationRequest : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js");
      Page.IncludeJs(
        "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js");
      Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Page.IncludeJs("~/js/jq/jquery.scrollintoview.js");
      Page.IncludeJs("~/js/vote.js");
      Page.IncludeJs("~/js/DonationRequest.js");
      Page.IncludeJs("~/js/json2.js");
      Page.IncludeCss("~/js/jq/jquery-ui.css");
      Page.IncludeCss("~/css/DonationRequest.css");
      DonateLink.HRef = VotePage.DonateUrl;
    }
  }
}