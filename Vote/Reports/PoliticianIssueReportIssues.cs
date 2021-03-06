using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class PoliticianIssueReportIssues : Report
  {
    #region Private

    private HtmlTable _HtmlTable;
    private QuestionsTable _QuestionsTable;
    private DataTable _Answers;

    #endregion Private

    private Control GenerateReport(string politicianKey, string issueKey,
      out int answerCount, string issueDescription = null)
    {
      answerCount = 0;
      if (issueDescription == null)
        issueDescription = VotePage.GetPageCache()
          .Issues.GetIssue(issueKey);

      _QuestionsTable = Questions.GetNonOmittedDataByIssueKey(issueKey);
      _Answers = Issues.GetPoliticianIssuePageIssueData(politicianKey, issueKey);
      var unansweredQuestions = new List<QuestionsRow>(_QuestionsTable);

      _HtmlTable =
        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");

      var tr = new HtmlTableRow().AddTo(_HtmlTable, "trPoliticianIssueHeading");
      new HtmlTableCell {InnerHtml = issueDescription}.AddTo(tr,
        "tdPoliticianIssueHeadingLeft");

      new HtmlTableCell {InnerHtml = "Positions and Views"}.AddTo(tr,
        "tdPoliticianIssueHeadingRight");
      foreach (var question in _QuestionsTable)
      {
        var answer = _Answers.Rows.OfType<DataRow>()
          .FirstOrDefault(row => row.QuestionKey()
            .IsEqIgnoreCase(question.QuestionKey));
        if (answer == null) continue;

        answerCount++;
        unansweredQuestions.Remove(question);

        ReportAnswer(question, answer);
      }

      if (unansweredQuestions.Count > 0)
      {
        tr = new HtmlTableRow().AddTo(_HtmlTable, "trNoResponsesHeading");
        new HtmlTableCell
          {
            InnerHtml =
              "These are available issue topics for which there were no responses.",
            ColSpan = 2
          }.AddTo(tr, "tdNoResponsesHeading");

        foreach (var question in unansweredQuestions)
          ReportUnansweredQuestion(question);
      }

      return _HtmlTable;
    }

    private void ReportAnswer(QuestionsRow question, DataRow answer)
    {
      var tr = new HtmlTableRow().AddTo(_HtmlTable, "trPoliticianIssueQuestion");
      new HtmlTableCell {InnerHtml = question.Question}.AddTo(tr,
        "tdPoliticianIssueQuestion");

      var td = new HtmlTableCell().AddTo(tr, "tdPoliticianIssueAnswer");
      FormatAnswer(answer, td, false);
    }

    private void ReportUnansweredQuestion(QuestionsRow question)
    {
      var tr = new HtmlTableRow().AddTo(_HtmlTable, "trPoliticianIssueQuestion");
      new HtmlTableCell {InnerHtml = question.Question, ColSpan = 2}.AddTo(tr,
        "tdPoliticianIssueQuestion");
    }

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    //public static Control GetReport(string politicianKey, string issueKey,
    //  string issueDescription = null)
    //{
    //  int notUsed;
    //  var reportObject = new PoliticianIssueReportIssues();
    //  return reportObject.GenerateReport(politicianKey, issueKey, out notUsed,
    //    issueDescription);
    //}

    public static Control GetReport(string politicianKey, string issueKey,
      out int answerCount, string issueDescription = null)
    {
      var reportObject = new PoliticianIssueReportIssues();
      return reportObject.GenerateReport(politicianKey, issueKey, out answerCount,
        issueDescription);
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