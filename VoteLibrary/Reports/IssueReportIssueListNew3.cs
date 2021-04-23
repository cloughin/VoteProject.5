using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class IssueReportIssueListNew3 : ResponsiveReport
  {
    #region Private

    private sealed class IssueReportIssuesDataManager : ReportDataManager<DataRow>
    {
      public void GetDataByJurisdiction(string stateCode, string countyCode, string localKey)
      {
        DataTable = Questions.GetQuestionListByJurisdictionNew3(stateCode, countyCode, localKey);
      }
    }

    private readonly IssueReportIssuesDataManager _DataManager =
      new IssueReportIssuesDataManager();

    private Control GenerateReportByJurisdiction(string stateCode, string countyCode, string localKey)
    {
      ReportContainer.ID = "new-accordions";
      ReportContainer.ClientIDMode = ClientIDMode.Static;
      stateCode = stateCode.ToUpperInvariant();
      _DataManager.GetDataByJurisdiction(stateCode, countyCode, localKey);

      var issueLevelGroups = _DataManager.GetDataSubset()
        .GroupBy(row => new
        {
          IssueGroupId = row.IssueGroupId(),
          IssueId = row.IssueId()
        })
        .ToList()
        .GroupBy(g => g.Key.IssueGroupId)
        .ToList()
        .GroupBy(g => g.First().First()
          .IssueLevel())
        .OrderBy(g => g.Key)
        .ToList();

      foreach (var issueLevelGroup in issueLevelGroups)
        switch (issueLevelGroup.Key)
        {
          case Issues.IssueLevelAll:
            OneReportSection(issueLevelGroup,
              "Questions Available to All Candidates");
            break;

          case Issues.IssueLevelNational:
            OneReportSection(issueLevelGroup,
              "Questions Available to U.S. President, U.S. House and U.S. Senate Candidates");
            break;

          case Issues.IssueLevelState:
            OneReportSection(issueLevelGroup,
              $"Questions Available to {StateCache.GetStateName(stateCode)}" +
              " Statewide, House and Senate Candidates");
            break;

          case Issues.IssueLevelCounty:
            OneReportSection(issueLevelGroup,
              $"Questions Available to {Counties.GetName(stateCode, countyCode)}" +
              $" ({stateCode}) Candidates");
            break;

          case Issues.IssueLevelLocal:
            OneReportSection(issueLevelGroup,
              $"Questions Available to {LocalDistricts.GetName(stateCode, localKey)}" +
              $" ({stateCode}) Candidates");
            break;
        }

      return ReportContainer.AddCssClasses("issue-list-report accordion-container");
    }

    private void OneReportSection(
      IEnumerable<IGrouping<int, IGrouping<dynamic, DataRow>>> groups, string heading)
    {
      var issueGroups = groups.ToList();
      new HtmlDiv {InnerHtml = heading}.AddTo(ReportContainer, "accordion-header accordion-header-1 top");
      var sectionContent = new HtmlDiv().AddTo(ReportContainer, "accordion-content accordion-content-1 top accordion-container");

      foreach (var issueGroup in issueGroups)
      {
        var groupContent = sectionContent;
        // only include issue group header if there's more than one
        if (issueGroups.Count > 1)
        {
          var groupInfo = issueGroup.First().First();
          var groupHeader = new HtmlDiv().AddTo(sectionContent, "accordion-header accordion-header-2");
          new HtmlDiv { InnerHtml = groupInfo.Heading() }.AddTo(groupHeader);
          if (!IsNullOrWhiteSpace(groupInfo.SubHeading()))
            new HtmlDiv { InnerHtml = groupInfo.SubHeading().ReplaceBreakTagsWithSpaces() }.AddTo(groupHeader, "sub-heading");
          groupContent = new HtmlDiv().AddTo(sectionContent, "accordion-container accordion-content accordion-content-2");
        }

        foreach (var issue in issueGroup)
        {
          var issueInfo = issue.First();

          var issueContent = groupContent;

          if (issue.Key.IssueId != Issues.IssueId.Biographical.ToInt() &&
            issue.Key.IssueId != Issues.IssueId.Reasons.ToInt())
          {
            new HtmlDiv {InnerHtml = issueInfo.Issue()}.AddTo(groupContent,
              "accordion-header accordion-header-3 indent");
            issueContent =
              new HtmlDiv().AddTo(groupContent, "accordion-content accordion-content-3 bottom");
          }
          else
          {
            issueContent.RemoveCssClass("accordion-container");
            issueContent.AddCssClasses("bottom indent");
          }

          foreach (var question in issue)
            new HtmlSpan {InnerHtml = question.Question()}.AddTo(issueContent, "question");
        }
      }
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetReportByJurisdiction(string stateCode, string countyCode, string localKey)
    {
      var reportObject = new IssueReportIssueListNew3();
      return reportObject.GenerateReportByJurisdiction(stateCode, countyCode, localKey);
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}