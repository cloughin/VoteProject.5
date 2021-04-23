using System;

namespace Vote
{
  public partial class Error500Page : PublicPage
  {
    protected Error500Page()
    {
      No404OnUrlNormalizeError = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Master.MenuPage = "/";
      Title = "Server Error (500)";
      Response.StatusCode = 500;
    }
  }
}