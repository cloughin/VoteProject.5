using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class FindPolitician : UserControl
  {
    public string CssClass { get; set; }

    protected void Page_Load(object sender, EventArgs e) => 
      FindPoliticianControl.AddCssClasses(CssClass);
  }
}