using System;
using static System.String;

namespace Vote
{
  public partial class Error404Page : PublicPage
  {
    private const string TitleTag = "{0} | Page Not Found (404)";

    protected Error404Page()
    {
      No404OnUrlNormalizeError = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Master.MenuPage = "/";
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      Response.StatusCode = 404;
    }
  }
}