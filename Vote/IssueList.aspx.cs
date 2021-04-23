using System;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class IssueListPage : CacheablePage
  {
    #region Caching support

    protected override bool SuppressCaching
    {
      get { return true; }
    }

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}." +
        $"{QueryState}.{QueryCounty}.{QueryLocal}";
    }

    protected override string GetCacheType()
    {
      return "IssueList";
    }

    #endregion Caching support

    private const string TitleTag = "{1} | Issues and issue questions available for {0}candidate responses";
    private const string MetaDescriptionTag = "These are the issues and questions" +
        " we make available to {0}candidates to provide voters with their positions" +
        " and views.";

    protected void Page_Load(object sender, EventArgs e)
    {
      var stateName = PublicMasterPage.StateName + " ";
      if (stateName == "US ") stateName = Empty;
      Title = Format(TitleTag, stateName, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, stateName); 

      Instructions.InnerHtml = "These are the issues and questions" +
        " we make available to the candidates to inform voters of their positions" +
        " and views. All responses are provided by the candidate or" +
        " are taken from the candidate&rsquo;s website, public statements or legitimate news articles.";

      if (EnableIssueGroups)
        IssueReportIssueListNew3
          .GetReportByJurisdiction(QueryState, QueryCounty, QueryLocal)
          .AddTo(ReportPlaceHolder);
      else
        IssueReportIssueListNew
          .GetReportByJurisdiction(QueryState, QueryCounty, QueryLocal)
          .AddTo(ReportPlaceHolder);
    }
  }
}