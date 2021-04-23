using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  public sealed class IssueReportIssueList : ResponsiveReport
  {
    #region Private

    private sealed class IssueReportIssuesDataManager : ReportDataManager<DataRow>
    {
      //public void GetDataByOfficeKey(string officeKey, string electionKey)
      //{
      //  DataTable = Questions.GetQuestionListByOfficeKey(officeKey, electionKey);
      //}

      //public void GetDataByPoliticianKey(string politicianKey, string electionKey)
      //{
      //  DataTable = Questions.GetQuestionListByPoliticianeKey(politicianKey,
      //    electionKey);
      //}

      public void GetDataByStateCode(string stateCode)
      {
        DataTable = Questions.GetQuestionListByStateCode(stateCode);
      }
    }

    private readonly IssueReportIssuesDataManager _DataManager =
      new IssueReportIssuesDataManager();

    //private Control GenerateReportByOfficeKey(string officeKey, string electionKey)
    //{
    //  _DataManager.GetDataByOfficeKey(officeKey, electionKey);

    //  var issueGroups = _DataManager.GetDataSubset()
    //    .GroupBy(row => row.IssueKey())
    //    .ToList();

    //  OneReportSection(issueGroups, "Questions");

    //  return ReportContainer.AddCssClasses("issue-list-report");
    //}

    //private Control GenerateReportByPoliticianKey(string politicianKey,
    //  string electionKey)
    //{
    //  _DataManager.GetDataByPoliticianKey(politicianKey, electionKey);

    //  var issueGroups = _DataManager.GetDataSubset()
    //    .GroupBy(row => row.IssueKey())
    //    .ToList();

    //  OneReportSection(issueGroups, "Questions");

    //  return ReportContainer.AddCssClasses("issue-list-report");
    //}

    private Control GenerateReportByStateCode(string stateCode)
    {
      _DataManager.GetDataByStateCode(stateCode);

      var issueLevelGroups = _DataManager.GetDataSubset()
        .GroupBy(row => row.IssueKey())
        .ToList()
        .GroupBy(g => g.First()
          .SortIssueLevel())
        .ToList();

      foreach (var issueLevelGroup in issueLevelGroups)
        switch (issueLevelGroup.Key)
        {
          case "A":
            OneReportSection(issueLevelGroup,
              "Questions Available to All Candidates");
            break;

          case "B":
            OneReportSection(issueLevelGroup,
              "Questions Available Only to U.S. President, U.S. House and U.S. Senate Candidates");
            break;

          case "C":
            OneReportSection(issueLevelGroup,
              "Questions Available Only to " + StateCache.GetStateName(stateCode) +
              " Statewide, House and Senate Candidates");
            break;
        }

      return ReportContainer.AddCssClasses("issue-list-report accordion-container");
    }

    private void OneReportSection(
      IEnumerable<IGrouping<string, DataRow>> issueGroups, string heading)
    {
      new HtmlDiv {InnerHtml = heading}.AddTo(ReportContainer, "accordion-header");
      var sectionContent = new HtmlDiv().AddTo(ReportContainer, "accordion-content section-content accordion-container");

      foreach (var issueGroup in issueGroups)
      {
        var issueInfo = issueGroup.First();

        new HtmlDiv {InnerHtml = issueInfo.Issue()}.AddTo(sectionContent, "accordion-header");
        var issueContent = new HtmlDiv().AddTo(sectionContent, "accordion-content");

        foreach (var question in issueGroup)
          new HtmlSpan {InnerHtml = question.Question()}.AddTo(issueContent, "question");
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

    //public static Control GetReportByOfficeKey(string officeKey,
    //  string electionKey = null)
    //{
    //  var reportObject = new IssueReportIssueListResponsive();
    //  return reportObject.GenerateReportByOfficeKey(officeKey, electionKey);
    //}

    //public static Control GetReportByPoliticianKey(string politicianId,
    //  string electionKey = null)
    //{
    //  var reportObject = new IssueReportIssueListResponsive();
    //  return reportObject.GenerateReportByPoliticianKey(politicianId, electionKey);
    //}

    public static Control GetReportByStateCode(string stateCode)
    {
      var reportObject = new IssueReportIssueList();
      return reportObject.GenerateReportByStateCode(stateCode);
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