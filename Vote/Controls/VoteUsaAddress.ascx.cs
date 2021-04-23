using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class VoteUsaAddress : UserControl
  {
    public bool NoBreak { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (NoBreak)
      {
        var literalControl = Controls[0] as LiteralControl;
        if (literalControl != null)
          literalControl.Text = literalControl.Text.Replace("<br />", ", ");
      }
    }
  }
}