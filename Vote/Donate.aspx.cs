using System;
using static System.String;

namespace Vote
{
  public partial class DonatePage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return UrlManager.GetStateCodeFromHostName();
    }

    protected override string GetCacheType()
    {
      return "Donate";
    }

    #endregion Caching support

    private const string TitleTag = "{0} | Make a Tax Deductible Donation to the {0}.org Project";
    private const string MetaDescriptionTag = "Your tax-deductible donation to the {0}.org project is our primary support and does not support any particular candidate.";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = Format(TitleTag, PublicMasterPage.SiteName);
        MetaDescription = Format(MetaDescriptionTag, PublicMasterPage.SiteName);
        DonateLink.HRef = DonateUrl;
      }
    }
  }
}