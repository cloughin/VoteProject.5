using System;
using static System.String;

namespace Vote
{
  public partial class PrivacyPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return UrlManager.GetStateCodeFromHostName();
    }

    protected override string GetCacheType()
    {
      return "Privacy";
    }

    #endregion Caching support

    private const string TitleTag = "{0} | Privacy Policy";
    private const string MetaDescriptionTag = "{0}.org's privacy policy regarding visitor information.";

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