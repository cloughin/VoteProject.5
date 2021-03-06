//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal sealed class IssueReportIssueList : Report
//  {
//    #region Private

//    private readonly HtmlTable _HtmlTable;

//    private sealed class IssueReportIssuesDataManager : ReportDataManager<DataRow>
//    {
//      public void GetDataByOfficeKey(string officeKey, string electionKey)
//      {
//        DataTable = Questions.GetQuestionListByOfficeKey(officeKey, electionKey);
//      }

//      public void GetDataByPoliticianKey(string politicianKey, string electionKey)
//      {
//        DataTable = Questions.GetQuestionListByPoliticianeKey(politicianKey,
//          electionKey);
//      }

//      public void GetDataByStateCode(string stateCode)
//      {
//        DataTable = Questions.GetQuestionListByStateCode(stateCode);
//      }
//    }

//    private readonly IssueReportIssuesDataManager _DataManager =
//      new IssueReportIssuesDataManager();

//    private IssueReportIssueList()
//    {
//      _HtmlTable =
//        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");
//    }

//    //private Control GenerateReport(string officeKey, string politicianKey = null,
//    //  string electionKey = null)
//    //{
//    //  if (!string.IsNullOrEmpty(officeKey))
//    //    _DataManager.GetDataByOfficeKey(officeKey, electionKey);
//    //  else if (!string.IsNullOrWhiteSpace(politicianKey))
//    //    _DataManager.GetDataByPoliticianKey(politicianKey, electionKey);
//    //  else
//    //    throw new VoteException(
//    //      "Either an OfficeKey or a PoliticianKey is required.");

//    //  var issueGroups = _DataManager.GetDataSubset()
//    //    .GroupBy(row => row.IssueKey())
//    //    .ToList();

//    //  OneReportSection(issueGroups, "Questions");

//    //  return _HtmlTable;
//    //}

//    private Control GenerateReportByOfficeKey(string officeKey, string electionKey)
//    {
//      _DataManager.GetDataByOfficeKey(officeKey, electionKey);

//      var issueGroups = _DataManager.GetDataSubset()
//        .GroupBy(row => row.IssueKey())
//        .ToList();

//      OneReportSection(issueGroups, "Questions");

//      return _HtmlTable;
//    }

//    private Control GenerateReportByPoliticianKey(string politicianKey,
//      string electionKey)
//    {
//      _DataManager.GetDataByPoliticianKey(politicianKey, electionKey);

//      var issueGroups = _DataManager.GetDataSubset()
//        .GroupBy(row => row.IssueKey())
//        .ToList();

//      OneReportSection(issueGroups, "Questions");

//      return _HtmlTable;
//    }

//    private Control GenerateReportByStateCode(string stateCode)
//    {
//      _DataManager.GetDataByStateCode(stateCode);

//      var issueLevelGroups = _DataManager.GetDataSubset()
//        .GroupBy(row => row.IssueKey())
//        .ToList()
//        .GroupBy(g => g.First()
//          .SortIssueLevel())
//        .ToList();

//      foreach (var issueLevelGroup in issueLevelGroups)
//        switch (issueLevelGroup.Key)
//        {
//          case "A":
//            OneReportSection(issueLevelGroup,
//              "Questions Available to All Candidates");
//            break;

//          case "B":
//            OneReportSection(issueLevelGroup,
//              "Questions Available Only to U.S. President, U.S. House and U.S. Senate Candidates");
//            break;

//          case "C":
//            OneReportSection(issueLevelGroup,
//              "Questions Available Only to " + StateCache.GetStateName(stateCode) +
//                " Statewide, House and Senate Candidates");
//            break;
//        }

//      //OneReportSection(issueGroups, "Questions");

//      return _HtmlTable;
//    }

//    private void OneReportSection(
//      IEnumerable<IGrouping<string, DataRow>> issueGroups, string heading)
//    {
//      var tr = new HtmlTableRow().AddTo(_HtmlTable, "trIssueHeading");
//      new HtmlTableCell {InnerHtml = "Issues"}.AddTo(tr, "tdIssueHeading");
//      new HtmlTableCell {InnerHtml = heading}.AddTo(tr, "tdIssueHeading");

//      foreach (var issueGroup in issueGroups)
//      {
//        var issueInfo = issueGroup.First();

//        tr = new HtmlTableRow().AddTo(_HtmlTable, "trIssue");
//        new HtmlTableCell {InnerHtml = issueInfo.Issue()}.AddTo(tr, "tdIssue");
//        new HtmlTableCell {InnerHtml = "&nbsp;"}.AddTo(tr, "tdBlank");

//        foreach (var question in issueGroup)
//        {
//          tr = new HtmlTableRow().AddTo(_HtmlTable, "trIssue");
//          new HtmlTableCell {InnerHtml = "&nbsp;"}.AddTo(tr, "tdBlank");
//          new HtmlTableCell {InnerHtml = question.Question()}.AddTo(tr,
//            "tdQuestion");
//        }
//      }
//    }

//    #endregion Private

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    public static Control GetReportByOfficeKey(string officeKey,
//      string electionKey = null)
//    {
//      var reportObject = new IssueReportIssueList();
//      return reportObject.GenerateReportByOfficeKey(officeKey, electionKey);
//    }

//    public static Control GetReportByPoliticianKey(string politicianId,
//      string electionKey = null)
//    {
//      var reportObject = new IssueReportIssueList();
//      return reportObject.GenerateReportByPoliticianKey(politicianId, electionKey);
//    }

//    public static Control GetReportByStateCode(string stateCode)
//    {
//      var reportObject = new IssueReportIssueList();
//      return reportObject.GenerateReportByStateCode(stateCode);
//    }

//    #region ReSharper restore

//    // ReSharper restore UnassignedField.Global
//    // ReSharper restore UnusedAutoPropertyAccessor.Global
//    // ReSharper restore UnusedMethodReturnValue.Global
//    // ReSharper restore UnusedMember.Global
//    // ReSharper restore MemberCanBeProtected.Global
//    // ReSharper restore MemberCanBePrivate.Global

//    #endregion ReSharper restore

//    #endregion Public
//  }
//}