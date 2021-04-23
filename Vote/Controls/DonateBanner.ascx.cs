using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class DonateBanner : UserControl
  {
    protected void Page_Load(object sender, EventArgs e) => 
      DonateBannerLink.HRef = VotePage.DonateUrl;
  }
}