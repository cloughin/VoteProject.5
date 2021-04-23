using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  /// <summary>
  /// Summary description for IssueQuestionsTable.
  /// </summary>
  public partial class QuestionsPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region legacy

    public static string Ok(string msg)
    {
      return $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";
    }

    public static string Fail(string msg)
    {
      return $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";
    }

    public static string Message(string msg)
    {
      return $"<span class=\"Msg\">{msg}</span>";
    }

    public static Random RandomObject;

    public static char GetRandomDigit()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      return Convert.ToChar(RandomObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string MakeUnique6Digits()
    {
      var password = Empty;
      //Make a DDDDDD password
      for (var n = 0; n < 6; n++)
        password += GetRandomDigit();
      return password;
    }

    #endregion legacy

    private string _IssueKey;
    private string _IssueLevel;
    private string _QuestionKey;
    private string _IssuesGroup;

    private void CheckOrder()
    {
      if (QuestionOrder.Text.Trim() == Empty) //create an order as last in list
      {
        var count = Questions.CountByIssueKey(_IssueKey);
        var rowNumber = (count + 1) * 10;
        QuestionOrder.Text = rowNumber.ToString(CultureInfo.InvariantCulture);
      }
      if (!QuestionOrder.Text.Trim().IsValidInteger())
        throw new ApplicationException("The Order needs to be a whole number.");
    }

    private void CheckQuestion()
    {
      if (Question.Text == Empty)
        throw new ApplicationException("Question field was empty.");
    }

    private static void CheckForAnswers(string questionKey)
    {
      var answerCount = Answers.CountByQuestionKey(questionKey);
      if (answerCount > 0)
        throw new ApplicationException(
          $"[{Questions.GetQuestionByQuestionKey(questionKey, Empty)}] was not deleted." +
          $" There are {answerCount} Answers for this question. You need to move the answers" +
          " to a different question. Click the [Move Questions Answers] Button to do this.");
    }

    private void LoadControls()
    {
      var questionTable = Questions.GetDataByQuestionKey(_QuestionKey);
      if (questionTable.Rows.Count != 1)
        throw new ApplicationException($"Did not find a unique row for this QuestionKey: {_QuestionKey}");
      var questionRow = questionTable.Rows[0];
      Question.Text = questionRow.Question();
      QuestionOrder.Text = questionRow.QuestionOrder().ToString();
      RadioButtonListOmit.SelectedValue = questionRow.IsQuestionOmit() ? "Yes" : "No";
    }

    private void ClearControls()
    {
      Question.Text = Empty;
      QuestionOrder.Text = Empty;
      RadioButtonListOmit.SelectedValue = "No";
      RadiobuttonlistDelete.SelectedValue = "No";
    }

    private bool EditRadioButtonListOmit()
    {
      return RadioButtonListOmit.SelectedValue == "Yes";
    }

    private static string PageTitleForQuestions(string stateCode, string issueKey, string issueLevel)
    {
      var pageTitle = Issues.GetIssue(issueKey) + " QUESTIONS";
      switch (issueLevel)
      {
        case "A":
          pageTitle += "<br>for ALL Offices";
          break;
        case "B":
          pageTitle += "<br>for ALL NATIONAL Offices";
          break;
        case "C":
          pageTitle += $"<br>for {StateCache.GetStateName(stateCode)} Offices";
          break;
      }

      return pageTitle;
    }

    private void ShowQuestions()
    {
      var questionsHtmlTable = Empty;
      questionsHtmlTable += "<table cellspacing=0 cellpadding=0>";

      #region Heading

      questionsHtmlTable += "<tr>";

      questionsHtmlTable += "<td class=tdReportGroupHeadingLeft>";
      questionsHtmlTable += "O D";
      questionsHtmlTable += "</td>";

      questionsHtmlTable += "<td class=tdReportGroupHeadingLeft>";
      questionsHtmlTable += "Order";
      questionsHtmlTable += "</td>";

      questionsHtmlTable += "<td class=tdReportGroupHeadingLeft>";
      questionsHtmlTable += "Question (Answers)";
      questionsHtmlTable += "</td>";

      questionsHtmlTable += "</tr>";

      #endregion

      var questionsRows = Questions.GetDataByIssueKey(_IssueKey).Rows.OfType<DataRow>()
        .OrderBy(r => r.QuestionOrder()).ToList();
      if (questionsRows.Count > 0)
      {
        foreach (var questionRow in questionsRows)
        {
          questionsHtmlTable += "<tr>";

          var omitAndDelete = Empty;
          if (questionRow.IsQuestionOmit())
            omitAndDelete += " O";

          questionsHtmlTable += "<td class=tdReportDetail>";
          questionsHtmlTable += omitAndDelete;
          questionsHtmlTable += "</td>";

          questionsHtmlTable += "<td class=tdReportDetail>";
          questionsHtmlTable += questionRow.QuestionOrder().ToString();
          questionsHtmlTable += "</td>";

          var answers = Answers.CountByQuestionKey(questionRow.QuestionKey());
          questionsHtmlTable += "<td class=tdReportDetail>";
          questionsHtmlTable += "<a href=\"/Admin/Questions.aspx?" +
            $"Group={_IssuesGroup}&Issue={_IssueKey}&Question={questionRow.QuestionKey()}\">" +
            $"<nobr>{questionRow.Question()} ({answers})</nobr></a>";

          questionsHtmlTable += "</td>";

          questionsHtmlTable += "</tr>";
        }
      }
      else
      {
        questionsHtmlTable += "<tr>";
        questionsHtmlTable += "<td  class=tdReportDetail colspan=3>";
        questionsHtmlTable += "No Questions";
        questionsHtmlTable += "</td>";
        questionsHtmlTable += "</tr>";
      }
      questionsHtmlTable += "</table>";
      LabelQuestionsTable.Text = questionsHtmlTable;
    }

    protected void RadioButtonListOmit_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (Question.Text == Empty)
          throw new ApplicationException(
            "You need to select a question before you can change its status.");

        Questions.UpdateIsQuestionOmit(EditRadioButtonListOmit(), _QuestionKey);
        ShowQuestions();

        Msg.Text =
          Ok(
            $"Question ({Questions.GetQuestionByQuestionKey(_QuestionKey, Empty)})" +
            " will be omitted on all pages.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void RadiobuttonlistDelete_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (Question.Text == Empty)
          throw new ApplicationException(
            "You need to select a question before you can change its status.");

        ShowQuestions();

        Msg.Text =
          Ok(
            $"Question ({Questions.GetQuestionByQuestionKey(_QuestionKey, Empty)})" +
            " is tagged for deletion.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonUpdate_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(QuestionOrder, Question);

        CheckOrder();
        CheckQuestion();

        Questions.UpdateQuestion(Question.Text.Trim(), _QuestionKey);
        Questions.UpdateQuestionOrder(int.Parse(QuestionOrder.Text.Trim()), _QuestionKey);

        ClearControls();

        ButtonUpdate.Visible = false;
        ButtonClear.Visible = true;
        ButtonAdd.Visible = true;

        ShowQuestions();

        Msg.Text =
          Ok("Question ({Questions.GetQuestionByQuestionKey(_QuestionKey, Empty)}) was UPDATED.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonClear_Click(object sender, EventArgs e)
    {
      try
      {
        ClearControls();
        ButtonUpdate.Visible = false;
        ButtonAdd.Visible = true;
        Msg.Text = Message("Enter a question to Add. The Order is optional.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonAdd_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(QuestionOrder, Question);

        CheckOrder();
        CheckQuestion();
        if (Questions.QuestionExists(Question.Text.Trim(), _IssueLevel))
          throw new ApplicationException("This is a duplicate question.");

        ViewState["QuestionKey"] = _QuestionKey = _IssueKey + MakeUnique6Digits();

        Questions.Insert(_QuestionKey, _IssueKey, int.Parse(QuestionOrder.Text.Trim()), 
          Question.Text.ReplaceNewLinesWithEmptyString(), false);

        ClearControls();

        ShowQuestions();

        Msg.Text =
          Ok($"Question ({Questions.GetQuestionByQuestionKey(_QuestionKey, Empty)}) was ADDED.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonRenumber_Click1(object sender, EventArgs e)
    {
      try
      {
        var table = Questions.GetDataByIssueKey(_IssueKey)
          .OrderBy(r => r.QuestionOrder);
        var count = 10;
        foreach (var questionRow in table)
        {
          Questions.UpdateQuestionOrderByQuestionKey(count, questionRow.QuestionKey);
          count += 10;
        }
        ShowQuestions();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonMoveQuestion_Click1(object sender, EventArgs e)
    {
      Response.Redirect($"/Admin/QuestionAnswers.aspx?IssueLevel={_IssueLevel}&State={_IssuesGroup}");
    }

    protected void ButtonDeleteTaggedQuestions_Click1(object sender, EventArgs e)
    {
      try
      {
        var questionsToDeleteTable = Questions.GetDataByIssueKey(_IssueKey)
          .OrderBy(r => r.QuestionOrder());
        foreach (var questionToDeleteRow in questionsToDeleteTable)
        {
          CheckForAnswers(questionToDeleteRow.QuestionKey);
          Questions.DeleteByQuestionKey(questionToDeleteRow.QuestionKey);
        }
        ShowQuestions();
        Msg.Text =
          Ok("All questions tagged for deletion have been physically deleted in the database.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected override void OnPreInit(EventArgs e)
    {
      var query = HttpContext.Current.Request.QueryString.ToString();
      var path = "/admin/Questions.aspx";
      if (!IsNullOrWhiteSpace(query)) path += "?" + query;
      Response.Redirect(path);
      Response.End();
      //base.OnPreInit(e);
    }

    private void Page_Load(object sender, EventArgs e)
    {
      _IssueKey = QueryIssue;
      _IssueLevel = Issues.GetIssueLevelFromKey(_IssueKey);
      _QuestionKey = QueryQuestion;
      _IssuesGroup = GetQueryString("Group");

      if (IsNullOrWhiteSpace(_IssuesGroup) || IsNullOrWhiteSpace(_IssueKey))
        HandleFatalError("The IssueGroup and/or IssueKey is missing");

      if (IsPostBack)
      {
        var questionKey = ViewState["QuestionKey"] as string;
        if (!IsNullOrEmpty(questionKey))
          _QuestionKey = questionKey;
      }
      else
      {
        Title = H1.InnerText = "Questions";

        try
        {
          PageTitle.Text = PageTitleForQuestions(_IssuesGroup,
            _IssueKey, _IssueLevel);

          HyperLinkReport.NavigateUrl = 
            $"/Admin/IssuesReport.aspx?IssueLevel={_IssueLevel}&Group={_IssuesGroup}";


          if (QueryQuestion == Empty)
          {
            #region From: /Admin/Issues.aspx

            ClearControls();
            ButtonUpdate.Visible = false;

            #endregion
          }
          else
          {
            #region From: Table of Questions at bottom of this page

            LoadControls();
            Msg.Text =
              Message("You can now either: (1) change the Order or Question" + " and click Update" +
                " or (2) chage the Status by clicking a radio button" +
                " or (3) click Clear to clear the order and topic question in preparation to Add a Question.");

            ButtonUpdate.Visible = true;
            ButtonAdd.Visible = false;

            #endregion
          }

          ShowQuestions();
        }
        catch (Exception ex)
        {
          #region

          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);

          #endregion
        }
      }
    }
  }
}