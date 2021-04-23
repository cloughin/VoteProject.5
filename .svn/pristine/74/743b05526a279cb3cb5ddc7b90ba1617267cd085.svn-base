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
  public partial class AddressEntry : System.Web.UI.UserControl
  {
    public void SetHeaderImage(string imagePath)
    {
      HeaderImage.Src = imagePath;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js");
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js");
      Page.IncludeJs("~/js/vote.js");
      Page.IncludeJs("~/js/AddressEntry.js");
      Page.IncludeCss("~/jq/jquery-ui.css");
      Page.IncludeCss("~/css/AddressEntry.css");

      string stateCode = DomainData.FromQueryStringOrDomain;
      MiniLogo.Src = DomainDesign.GetImageUri("smbanner.png").ToString();
      MiniLogo.Alt = UrlManager.GetDomainDesignCodeHostName(stateCode);

      //MiniLogo.Alt = "Vote-USA.org";

      StateCache.Populate(AddressEntryStatesDropDownList, "", "");
    }
  }
}