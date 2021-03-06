using System;
using System.Web;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class AddressEntryInline : UserControl
  {
    public void Submit()
    {
      var address = Address.Value.Trim();
      if (string.IsNullOrWhiteSpace(address))
      {
        ErrorMessage.InnerText = "Please enter an address or a 9-digit zip";
      }
      else
      {
        var result = AddressFinder.Find(Address.Value, true, null, Remember.Checked);
        if (result.Success)
        {
          if (UrlManager.CurrentDomainDataCode == "US")
          {
            var cookie = new HttpCookie("Address", result.OriginalInput);
            if (Remember.Checked) cookie.Expires = DateTime.UtcNow.AddYears(1);
            Response.Cookies.Add(cookie);
          }
          Response.Redirect(result.RedirectUrl);
        }
        else
        {
          ErrorMessage.InnerText = result.ErrorMessage;
        }
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("~/js/jq/jquery.cookie.js");
      Page.IncludeJs("~/js/vote/controls/AddressEntryInline.js");
    }
  }
}