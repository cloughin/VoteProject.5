using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class GoogleAddressEntry : UserControl
  {
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public void Submit()
    {
      // stub for compile-compatibility with AddressEntryInline
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Page.IncludeJs("~/js/jq/jquery.cookie.js");
      //Page.IncludeJs("~/js/vote/controls/GoogleAddressEntry.js");
      Page.IncludeJs("https://maps.googleapis.com/maps/api/js?key=AIzaSyAJGb2AGKOS0mf-VWmBQRRMH-n02RWhNKQ&libraries=places");
    }
  }
}