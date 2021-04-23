using System;
using System.Web.UI;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class OfficialsNew : CacheablePage
  {
    protected OfficialsNew()
    {
      No404OnUrlNormalizeError = true;
    }

    #region Caching support

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}.{QueryReport}." + 
        $"{QueryState}.{QueryCounty}.{QueryLocal}";
    }

    protected override string GetCacheType()
    {
      return "Officials";
    }

    #endregion Caching support

    private string _StateCode;
    private string _CountyCode;
    private string _LocalKey;
    private ElectoralClass _ElectoralClass;

    private const string TitleTag = "{1} | {0} | Pictures, Bios, Position Statements and Social Media Links";

    private const string MetaDescriptionTag = "{0} with pictures, bios, position statements and social media links";

    private void AnalyzeElectoralClass()
    {
      string titleContentDescription;
      var metaContentLocation = Empty;

      switch (_ElectoralClass)
      {
        case ElectoralClass.USPresident:
          titleContentDescription = "Current US President and Vice President";
          break;

        case ElectoralClass.USSenate:
          titleContentDescription = "Current US Senators";
          break;

        case ElectoralClass.USHouse:
          titleContentDescription = "Current US House Members";
          break;

        case ElectoralClass.USGovernors:
          titleContentDescription = "Current State Governors and Lieutenant Governors";
          break;

        case ElectoralClass.State:
          titleContentDescription =
            $"Current {StateCache.GetStateName(_StateCode)} Elected Representatives";
          break;

        case ElectoralClass.County:
          titleContentDescription = "Current County Representatives";
          metaContentLocation =
            $"{CountyCache.GetCountyName(_StateCode, _CountyCode)}, {StateCache.GetStateName(_StateCode)}";
          break;

        case ElectoralClass.Local:
          titleContentDescription = "Current Local District Representatives";
          metaContentLocation =
            $"{LocalDistricts.GetName(_StateCode, _LocalKey)}," + 
            $" {CountyCache.GetCountyName(_StateCode, _CountyCode)}, {StateCache.GetStateName(_StateCode)}";
          break;

        default:
          titleContentDescription = "Elected Representatives";
          break;
      }

      var metaContentDescription = titleContentDescription;
      if (metaContentLocation != Empty)
        metaContentDescription += $", {metaContentLocation}";

      //PageHeading.MainHeadingText = titleContentDescription;
      H1.InnerText = titleContentDescription;
      Title = Format(TitleTag, metaContentDescription, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, metaContentDescription);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (Report.SignedInReportUser != ReportUser.Public)
      {
        this.IncludeCss("~/css/MainCommon.css");
        this.IncludeCss("~/css/SecondaryCommon.css");
        this.IncludeCss("~/css/All.css");
        this.IncludeCss("~/css/Officials.css");
      }

      if (HasUrlError)
      {
        InnerContent.Controls.Clear();
        var p = new HtmlP().AddTo(InnerContent, "not-found-error");
        new LiteralControl($"We could not show the information you requested: {UrlError}").AddTo(p);
        return;
      }

      _StateCode = QueryReport;
      _CountyCode = Empty;
      _LocalKey = Empty;
      if (IsNullOrWhiteSpace(_StateCode))
        _StateCode = QueryState;
      if (StateCache.IsValidStateCode(_StateCode))
      {
        _CountyCode = QueryCounty;
        _LocalKey = QueryLocal;
      }

      _ElectoralClass = Offices.GetElectoralClass(_StateCode, _CountyCode, _LocalKey);

      AnalyzeElectoralClass();
      var report = OfficialsReportResponsive.GetReport(_StateCode, _CountyCode, _LocalKey);
      report.AddTo(ReportPlaceHolder);
    }
  }
}