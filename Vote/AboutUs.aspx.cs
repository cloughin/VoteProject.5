using System;
using static System.String;

namespace Vote
{
  public partial class AboutUsPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return UrlManager.GetStateCodeFromHostName();
    }

    protected override string GetCacheType()
    {
      return "AboutUs";
    }

    #endregion Caching support

    private const string TitleTag = "{0} | About Us";

    private const string MetaDescriptionTag =
      "About {0}.org and its current operations, mission, funding and history.";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = Format(TitleTag, PublicMasterPage.SiteName);
        MetaDescription = Format(MetaDescriptionTag, PublicMasterPage.SiteName);
      }
    }
  }
}