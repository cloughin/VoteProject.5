using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class PageHeading : UserControl
  {
    public string MainHeadingText;
    public string MainHeadingCssClass;
    public string SubHeadingText;
    private readonly bool _ShowDonateButton;

    protected PageHeading()
    {
      _ShowDonateButton = UrlManager.CurrentQuerySiteId == "ivn";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (!string.IsNullOrEmpty(MainHeadingText))
          MainHeadingLiteral.Text = MainHeadingText;
        if (!string.IsNullOrEmpty(MainHeadingCssClass))
          MainHeading.Attributes["class"] = MainHeadingCssClass;
        if (!string.IsNullOrEmpty(SubHeadingText))
          SubHeadingLiteral.Text = SubHeadingText;
        else
          SubHeading.Visible = false;
        DonateButton.Visible = _ShowDonateButton;
        DonateLink.HRef = VotePage.DonateUrl;
      }
    }
  }
}