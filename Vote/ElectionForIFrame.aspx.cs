using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;

namespace Vote
{
  public partial class ElectionForIFramePage : PublicPage
  {
    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;
    private string _StateCode;
    private bool _OpenAll;
    private ReportUser _ReportUser;

    private const string TitleTag = "{2}{0} | Election Contests and Ballot Measures | {1}";
    private const string MetaDescriptionTag = "{0}: Explore the election contests and ballot measures with candidate pictures bios, position statements and links to social media.";

    private string AppendOpenAll(string url)
    {
      return _OpenAll
        ? url + "&openall=Y"
        : url;
    }

    private void PopulateMetaTags()
    {
      Title = string.Format(TitleTag, _ElectionDescription, PublicMasterPage.SiteName,
        _ReportUser == ReportUser.Master
          ? "Master Election Report - "
          : string.Empty);
      MetaDescription = string.Format(MetaDescriptionTag, _ElectionDescription);
    }

    private void FillInReport()
    {
      var report = ElectionReportResponsive.GetReport(_ElectionKey, _OpenAll);
      report.AddTo(ElectionPlaceHolder);
    }

    private void FillInAdditionalInfo()
    {
      var additionalInfo =
        PageCache.Elections.GetElectionAdditionalInfo(_ElectionKey);
      if (string.IsNullOrWhiteSpace(additionalInfo))
        additionalInfo = ElectionsDefaults.GetElectionAdditionalInfo(
          Elections.GetDefaultElectionKeyFromKey(_ElectionKey));

      if (string.IsNullOrWhiteSpace(additionalInfo))
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
        InnerText = "Go to State Election",
        HRef = AppendOpenAll(UrlManager.GetElectionPageUri(Elections.GetStateElectionKeyFromKey(_ElectionKey)).ToString())
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
          HRef = AppendOpenAll(UrlManager.GetElectionPageUri(key.key).ToString())
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
      Master.MenuPage = "forVoters.aspx";
      _OpenAll = GetQueryString("openall").IsEqIgnoreCase("Y");

      var @public = GetQueryString("public").IsEqIgnoreCase("Y");
      var master = GetQueryString("public").IsEqIgnoreCase("N");
      _ReportUser = @public
        ? ReportUser.Public
        : (master
          ? ReportUser.Master
          : (Report.SignedInReportUser == ReportUser.Master
            ? ReportUser.Master
            : ReportUser.Public
          ));

      if (PageCache.Elections.GetElectionDate(Elections.GetStateElectionKeyFromKey(_ElectionKey)) < DateTime.Today)
        NoIndex = true;
      else
        PastElectionWarning.Visible = false;

      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      // This could be a county election with no county offices, just local links,
      // in which case the county Elections row won't exist. In that case,
      // use the state description
      if (string.IsNullOrWhiteSpace(_ElectionDescription))
        _ElectionDescription = PageCache.Elections.GetElectionDesc(Elections.GetStateElectionKeyFromKey(_ElectionKey));
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);

      PopulateMetaTags();
      FillInTitles();
      FillInHigherLinks();
      FillInAdditionalInfo();
      FillInReport();

      if (_OpenAll)
        Master.FindControl("Body").AddCssClasses("open-all-accordions");
    }
  }
}