using System;
using System.Web.UI.HtmlControls;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class ElectedPage : CacheablePage
  {
    protected ElectedPage()
    {
      //NoIndex = true;
    }

    #region Caching support

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}.{QueryState}.{QueryCongress}.{QueryStateSenate}." +
        $"{QueryStateHouse}.{QueryCounty}.{QueryDistrict}.{QueryPlace}.{QueryElementary}." +
        $"{QuerySecondary}.{QueryUnified}.{QueryCityCouncil}.{QueryCountySupervisors}." +
        $"{QuerySchoolDistrictDistrict}";
    }

    protected override string GetCacheType()
    {
      return "Elected";
    }

    #endregion Caching support

    #region Private

    private const string TitleTag = "{1} | Elected Representatives for {0}";

    private void FillInHeading()
    {
      LocationInfo1.InnerHtml = LocationInfo2.InnerHtml = FormatLegislativeDistrictsFromQueryStringForHeading(true);
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = Format(TitleTag, FormatLegislativeDistrictsFromQueryString(", "), PublicMasterPage.SiteName);

      FillInHeading();
      ElectedReportResponsive.GetReport(QueryState, QueryCounty, QueryCongress,
        QueryStateSenate, QueryStateHouse, QueryDistrict, QueryPlace, 
        QueryElementary, QuerySecondary, QueryUnified, QueryCityCouncil, 
        QueryCountySupervisors, QuerySchoolDistrictDistrict, true, true).AddTo(ReportPlaceHolder);

      var body = Master.FindControl("Body") as HtmlGenericControl;
      // ReSharper disable once PossibleNullReferenceException
      body.Attributes.Add("data-state", QueryState);
    }

    #endregion Event handlers and overrides
  }
}