using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class AddressEntry : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js");
      Page.IncludeJs(
        "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js");
      Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Page.IncludeJs("~/js/jq/jquery.scrollintoview.js");
      Page.IncludeJs("~/js/vote.js");
      Page.IncludeJs("~/js/AddressEntry.js");
      Page.IncludeJs("~/js/json2.js");
      Page.IncludeCss("~/js/jq/jquery-ui.css");
      Page.IncludeCss("~/css/AddressEntry.css");

      var stateCode = DomainData.FromQueryStringOrDomain;
      MiniLogo.Src = DomainDesign.GetImageUri("smbanner.png")
        .ToString();
      MiniLogo.Alt = UrlManager.GetDomainDesignCodeHostName(stateCode);

      //MiniLogo.Alt = "Vote-USA.org";

      StateCache.Populate(AddressEntryStatesDropDownList, "", "");
    }
  }
}