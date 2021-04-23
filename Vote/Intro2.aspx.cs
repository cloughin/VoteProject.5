using System;
using System.Data;
using System.Web.UI;
using DB;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class Intro2Page : PublicPage
  {
    private readonly string _PoliticianKey = QueryId;
    private DataRow _PoliticianInfo;
    private string _PoliticianName;
    private string _OfficeAndStatus;

    private const string TitleTag = "{1} | {0}";
    private const string MetaDescriptionTag = "{0}: Picture, bio, position statements and social media links | {1}";

    private string GetCandidateInfo(string separator)
    {
      var title = _PoliticianName + separator + _OfficeAndStatus;
      if (_PoliticianInfo.LivePoliticianStatus()
        .IsInFutureViewableElection())
        title += separator + _PoliticianInfo.ElectionDescription();
      return title;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _PoliticianInfo = Politicians.GetPoliticianIntroReportData(_PoliticianKey);

      if (_PoliticianInfo == null)
      {
        InnerContent.Controls.Clear();
        var p = new HtmlP().AddTo(InnerContent, "not-found-error");
        new LiteralControl($"Could not find Id {_PoliticianKey}").AddTo(p);
        return;
      }

      _PoliticianName = Politicians.FormatName(_PoliticianInfo);
      _OfficeAndStatus = Politicians.FormatOfficeAndStatus(_PoliticianInfo);

      Title = Format(TitleTag, GetCandidateInfo(" | "), PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, GetCandidateInfo(", "), PublicMasterPage.SiteName);
      //MetaKeywords = _PoliticianName;

      PageHeading.MainHeadingText = Format(PageHeading.MainHeadingText,
        _PoliticianName);

      PoliticianInfoResponsive.GetReport(_PoliticianInfo).AddTo(InfoPlaceHolder);
      IntroIssuesReport.GetReport(_PoliticianInfo).AddTo(ReportPlaceHolder);
    }
  }
}