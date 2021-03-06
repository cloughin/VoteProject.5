using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class PageHeadingResponsive : UserControl
  {
    public string MainHeadingText;
    public string SubHeadingText;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (!string.IsNullOrEmpty(MainHeadingText))
          MainHeadingLiteral.Text = MainHeadingText;
        if (!string.IsNullOrEmpty(SubHeadingText))
          SubHeadingLiteral.Text = SubHeadingText;
        else
          SubHeading.Visible = false;
      }
    }
  }
}