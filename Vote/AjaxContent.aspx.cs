using System;
using Vote.Reports;
using static System.String;

namespace Vote
{
  public partial class AjaxContentPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      switch (GetQueryString("Content").ToUpperInvariant())
      {
        case "COMPARE":
          return $"COMPARE:{QueryElection}.{QueryOffice}.{QueryQuestion}";

        case "COMPAREVIDEOS":
          return $"COMPAREVIDEOS:{QueryElection}.{QueryOffice}.{QueryId}";

        case "INTRO":
          return $"INTRO:{QueryId}.{QueryQuestion}";

        case "INTROVIDEOS":
          return $"INTROVIDEOS:{QueryId}";

        default:
          return Empty;
      }
    }

    protected override string GetCacheType()
    {
      return "AjaxContent";
    }

    #endregion Caching support

    protected AjaxContentPage()
    {
      NoUrlEdit = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      switch (GetQueryString("Content").ToUpperInvariant())
      {
        case "COMPARE":
          CompareCandidatesReportResponsive
            .GetOneAnswerContent(QueryElection, QueryOffice, QueryQuestion)
            .AddTo(PlaceHolder);
          break;

        case "COMPAREVIDEOS":
          CompareCandidatesReportResponsive
            .GetOneCandidateVideoContent(QueryElection, QueryOffice, QueryId)
            .AddTo(PlaceHolder);
          break;

        case "INTRO":
          IntroIssuesReport.GetOneAnswerContent(QueryId, QueryQuestion).AddTo(PlaceHolder);
          break;

        case "INTROVIDEOS":
          IntroIssuesReport.GetVideoContent(QueryId).AddTo(PlaceHolder);
          break;
      }
    }
  }
}