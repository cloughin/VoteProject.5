using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class VoteUsaAddressResponsive : UserControl
  {
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool NoBreak { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
      Vertical.Visible = !NoBreak;
      Horizontal.Visible = NoBreak;
    }
  }
}