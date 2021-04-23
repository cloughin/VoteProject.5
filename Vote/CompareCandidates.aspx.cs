using System;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class CompareCandidates : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}.{QueryState}.{QueryElection}.{QueryOffice}";
    }

    protected override string GetCacheType()
    {
      return "CompareCandidates";
    }

    #endregion Caching support

    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;
    private readonly string _OfficeKey = QueryOffice;
    private string _OfficeDescription;
    private string _ElectionAndOfficeDescription;

    private const string TitleTag =
      "{1} | {0} | Biographical Profiles and Positions on the Issues";

    private const string MetaDescriptionTag =
      "{0}: Compare the candidates biographical profiles" +
      " and positions on the issues, with pictures and social media links.";

    private void PopulateMetaTags()
    {
      Title = Format(TitleTag, _ElectionAndOfficeDescription, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, _ElectionAndOfficeDescription);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      _OfficeDescription = Offices.GetLocalizedOfficeName(_OfficeKey);

      if (Elections.GetElectionTypeFromKey(_ElectionKey) ==
        Elections.ElectionTypeUSPresidentialPrimary ||
        Elections.GetStateCodeFromKey(_ElectionKey) == "US" &&
        Elections.GetElectionTypeFromKey(_ElectionKey) == "G")
      {
        _ElectionAndOfficeDescription = _ElectionDescription;
        ElectionTitle.InnerText = _ElectionDescription;
        OfficeTitle.Visible = false;
      }
      else
      {
        _ElectionAndOfficeDescription = $"{_ElectionDescription}, {_OfficeDescription}";
        // Link removed per Mantis 840
        //new HtmlAnchor
        //{
        //  HRef = UrlManager.GetElectionPageUri(_ElectionKey).ToString(),
        //  InnerText = _ElectionDescription
        //}.AddTo(ElectionTitle);
        ElectionTitle.InnerText = _ElectionDescription;
        OfficeTitle.InnerText =
          $"Candidates for {Offices.GetLocalizedOfficeNameWithElectoralClass(PageCache, _OfficeKey)}";
      }

      PopulateMetaTags();
      if (Master.FindControl("Body") is HtmlGenericControl body)
      {
        body.Attributes.Add("data-election",
          Elections.GetStateElectionKeyFromKey(QueryElection).ToUpperInvariant());
        body.Attributes.Add("data-adelection", _ElectionKey);
        body.Attributes.Add("data-adoffice", _OfficeKey);
      }

      CompareCandidatesReportResponsive.GetReport(_ElectionKey, _OfficeKey)
        .AddTo(ReportPlaceHolder);
    }
  }
}