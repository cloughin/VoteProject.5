using System;
using static System.String;

namespace Vote
{
  public partial class DefaultPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return UrlManager.GetStateCodeFromHostName();
    }

    protected override string GetCacheType()
    {
      return "Home";
    }

    #endregion Caching support

    private const string TitleTag = "{1} | Candidate Comparisons for {0} Elections, Interactive Ballot Choices, Voter Information, etc.";
    private const string MetaDescriptionTag = "Compare the candidates for {0} elections, get interactive ballot choicess, and learn about your elected representatives.";

    protected override void OnPreInit(EventArgs e)
    {
      if (UrlManager.CurrentDomainDataCode != "US")
      {
        Response.Redirect(UrlManager.GetSiteUri().ToString());
        Response.End();
      }

      base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var stateName = PublicMasterPage.StateName;
        Title = Format(TitleTag, stateName, PublicMasterPage.SiteName);
        MetaDescription = Format(MetaDescriptionTag, stateName);
        Master?.ShowDisclaimer();
      }
    }
  }
}