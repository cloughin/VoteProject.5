//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal sealed class IssueReportIssues : IssueReport
//  {
//    #region Private

//    private QuestionsTable _QuestionsTable;

//    private sealed class IssueReportIssuesDataManager : IssueReportDataManager
//    {
//      private readonly string _ElectionKey;
//      private readonly string _OfficeKey;
//      private readonly string _IssueKey;

//      public IssueReportIssuesDataManager(string electionKey, string officeKey,
//        string issueKey)
//      {
//        _ElectionKey = electionKey;
//        _OfficeKey = officeKey;
//        _IssueKey = issueKey;
//      }

//      public override void GetData()
//      {
//        DataTable = Issues.GetIssuePageIssueData(_ElectionKey, _OfficeKey,
//          _IssueKey);
//      }
//    }

//    #endregion Private

//    #region Protected

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable VirtualMemberNeverOverriden.Global
//    // ReSharper disable UnusedMember.Global

//    #endregion ReSharper disable

//    protected override int ReportDetails(
//      List<IGrouping<string, DataRow>> politiciansInRow)
//    {
//      var answerCount = 0;

//      // build a dictionary of questions with answers
//      var questionDictionary = _QuestionsTable.ToDictionary(q => q.QuestionKey,
//        q =>
//          politiciansInRow.Exists(
//            g => g != null && g.FirstOrDefault(p => p.QuestionKey()
//              .IsEqIgnoreCase(q.QuestionKey)) != null),
//        StringComparer.OrdinalIgnoreCase);

//      foreach (var question in
//        _QuestionsTable.Where(q => questionDictionary[q.QuestionKey]))
//      {
//        answerCount++;

//        var tr = new HtmlTableRow().AddTo(HtmlTable, "trIssueQuestion");
//        new HtmlTableCell {InnerHtml = question.Question}.AddTo(tr,
//          "tdIssueIssueQuestion");

//        foreach (var politicianGroup in politiciansInRow)
//        {
//          var td = new HtmlTableCell().AddTo(tr, "tdIssueAnswer");

//          // get the appropriate answer row
//          var politicianAnswer = politicianGroup == null
//            ? null
//            : politicianGroup.FirstOrDefault(p => p.QuestionKey()
//              .IsEqIgnoreCase(question.QuestionKey));
//          FormatAnswer(politicianAnswer, td, IsRunningMateOffice);
//        }
//      }

//      if (questionDictionary.ContainsValue(false))
//      {
//        var tr = new HtmlTableRow().AddTo(HtmlTable, "trNoResponsesHeading");
//        new HtmlTableCell
//          {
//            InnerHtml =
//              "These are available issue topics for which there were no responses.",
//            ColSpan = 4
//          }.AddTo(tr, "tdNoResponsesHeading");

//        foreach (var question in
//          _QuestionsTable.Where(q => !questionDictionary[q.QuestionKey]))
//        {
//          tr = new HtmlTableRow().AddTo(HtmlTable, "trIssueQuestion");
//          new HtmlTableCell {InnerHtml = question.Question, ColSpan = 4}.AddTo(tr,
//            "tdIssueIssueQuestion tdNoResponsesIssue");
//        }
//      }

//      return answerCount;
//    }

//    #region ReSharper restore

//    // ReSharper restore UnusedMember.Global
//    // ReSharper restore VirtualMemberNeverOverriden.Global
//    // ReSharper restore MemberCanBePrivate.Global

//    #endregion ReSharper restore

//    #endregion Protected

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    public Control GenerateReport(string electionKey, string officeKey,
//      string issueKey, out int answerCount)
//    {
//      DataManager = new IssueReportIssuesDataManager(electionKey, officeKey,
//        issueKey);
//      _QuestionsTable = Questions.GetNonOmittedDataByIssueKey(issueKey);
//      return GenerateReport(Issues.GetIssue(issueKey), out answerCount);
//    }

//    public static Control GetReport(string electionKey, string officeKey,
//      string issueKey, out int answerCount)
//    {
//      var reportObject = new IssueReportIssues();
//      return reportObject.GenerateReport(electionKey, officeKey, issueKey,
//        out answerCount);
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