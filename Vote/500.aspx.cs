using System;
using static System.String;

namespace Vote
{
  public partial class Error500Page : PublicPage
  {
    private const string TitleTag = "{0} | Server Error (500)";

    protected Error500Page()
    {
      No404OnUrlNormalizeError = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      //Master.MenuPage = "/";
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      Response.StatusCode = 500;
    }
  }
}