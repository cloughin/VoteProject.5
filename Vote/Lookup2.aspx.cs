using System;

namespace Vote
{
  public partial class Lookup2Page : PublicPage
  {
    protected Lookup2Page()
    {
      No404OnUrlNormalizeError = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = "Lookup Test";
      //Master.DisableForm();
      //(Master.FindControl("form1") as HtmlControl)?.Attributes.Add("onsubmit", "return false");
    }
  }
}