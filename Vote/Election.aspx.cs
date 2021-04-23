using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class ElectionPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}.{QueryState}.{QueryElection}." + $"{GetQueryString("openall").IsEqIgnoreCase("Y")}" + $"{GetQueryString("complete").IsEqIgnoreCase("1")}";
    }

    protected override string GetCacheType()
    {
      return "Election";
    }

    #endregion Caching support

    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;
    private string _StateCode;

    private const string TitleTag = "{1} | {0} | Election Contests and Ballot Measures";
    private const string MetaDescriptionTag = "{0}: Explore the election contests and ballot measures" +
      " with candidate pictures bios, position statements and links to social media.";

    private void PopulateMetaTags()
    {
      Title = Format(TitleTag, _ElectionDescription, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, _ElectionDescription);
    }

    private void FillInReport()
    {
      var report = ElectionReportResponsive.GetReport(_ElectionKey, GetQueryString("complete") == "1");
      report.AddTo(ElectionPlaceHolder);
    }

    private void FillInAdditionalInfo()
    {
      var additionalInfo =
        PageCache.Elections.GetElectionAdditionalInfo(_ElectionKey);
      if (IsNullOrWhiteSpace(additionalInfo))
        additionalInfo = ElectionsDefaults.GetElectionAdditionalInfo(
          Elections.GetDefaultElectionKeyFromKey(_ElectionKey));

      if (IsNullOrWhiteSpace(additionalInfo))
        AdditionalInfo.Visible = false;
      else
      {
        AdditionalInfo.InnerHtml = additionalInfo.ReplaceBreakTagsWithSpaces().ReplaceNewLinesWithParagraphs();
        AdditionalInfo.Visible = true;
      }
    }

    private void FillInTitles()
    {
      ElectionTitle.InnerText = _ElectionDescription;
      if (StateCache.IsValidStateCode(_StateCode))
        ElectionSubTitle.InnerText = Elections.GetElectoralClassDescription(_ElectionKey);
      else ElectionSubTitle.Visible = false;
    }

    private void FillInStateLink()
    {
      new HtmlAnchor
      {
        InnerText = $"Go to {StateCache.GetStateName(_StateCode)} State Election",
        HRef = UrlManager.GetElectionPageUri(Elections.GetStateElectionKeyFromKey(_ElectionKey)).ToString()
      }.AddTo(HigherLevelLinks, "state-link");
    }

    private void FillInCountyLinks()
    {
      var countyKeys =
        Elections.GetCountyElectionKeysFromKey(_ElectionKey)
          .Select(
            k =>
              new
              {
                key = k,
                name =
                CountyCache.GetCountyName(_StateCode, Elections.GetCountyCodeFromKey(k))
              })
          .OrderBy(k => k.name, StringComparer.OrdinalIgnoreCase);
      foreach (var key in countyKeys)
        new HtmlAnchor
        {
          InnerText = $"Go to {key.name} Election",
          HRef = UrlManager.GetElectionPageUri(key.key).ToString()
        }.AddTo(HigherLevelLinks, "county-link");
    }

    private void FillInHigherLinks()
    {
      switch (Elections.GetElectoralClass(_ElectionKey))
      {
        case ElectoralClass.County:
          FillInStateLink();
          break;

        case ElectoralClass.Local:
          FillInStateLink();
          FillInCountyLinks();
          break;

        default:
          HigherLevelLinks.Visible = false;
          break;
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (PageCache.Elections.GetElectionDate(
        Elections.GetStateElectionKeyFromKey(_ElectionKey)) >= DateTime.Today)
        PastElectionWarning.Visible = false;
      //else
      //  ; //NoIndex = true;

      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      // This could be a county election with no county offices, just local links,
      // in which case the county Elections row won't exist. In that case,
      // use the state description
      if (IsNullOrWhiteSpace(_ElectionDescription))
        _ElectionDescription = PageCache.Elections.GetElectionDesc(Elections.GetStateElectionKeyFromKey(_ElectionKey));
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);

      PopulateMetaTags();
      FillInTitles();
      FillInHigherLinks();
      FillInAdditionalInfo();
      FillInReport();

      if (GetQueryString("openall").SafeString().ToUpper() == "Y")
        Master.FindControl("Body").AddCssClasses("open-all-accordions");
    }
  }
}