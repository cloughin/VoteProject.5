using System;
using System.Web.UI;
using static System.String;

namespace Vote.Controls
{
  public partial class PageHeadingResponsive : UserControl
  {
    public string MainHeadingText;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        if (!IsNullOrEmpty(MainHeadingText))
          MainHeadingLiteral.Text = MainHeadingText;
        SubHeading.Visible = false;
      }
    }
  }
}