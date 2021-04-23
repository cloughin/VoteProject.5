using System;

namespace Vote
{
  public partial class Error404Page : PublicPage
  {
    protected Error404Page()
    {
      No404OnUrlNormalizeError = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Master.MenuPage = "/";
      Title = "Page Not Found (404)";
      Response.StatusCode = 404;
    }
  }
}