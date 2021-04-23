using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class QuestionAnswers : SecureAdminPage, IAllowEmptyStateCode
  {
    #region legacy

    public static string Ok(string msg) => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

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

    public override IEnumerable<string> NonStateCodesAllowed { get; } = new[]
      {"LL", "US"};


    private string _IssueLevel;

    private void ReasignAnswerToQuestionMovingTo(string politicianKey)
    {
      var questionKey = Textbox_QuestionKey_Move_Answers_To.Text.Trim();
      var issueKey = Questions.GetIssueKey(questionKey);
      Answers.UpdateIssueKeyByPoliticianKeyQuestionKey(issueKey, politicianKey, questionKey);
      Answers.UpdateQuestionKeyByPoliticianKeyQuestionKey(questionKey, politicianKey, questionKey);
    }

    private static string PageTitle_QuestionAnswers(string stateCode, string issueLevel)
    {
      var pageTitle = Empty;
      switch (issueLevel)
      {
        case "A":
          pageTitle += "for ALL Candidates";
          break;
        case "B":
          pageTitle += "for NATIONAL Office Candidates";
          break;
        case "C":
          if (StateCache.IsValidStateCode(stateCode))
          {
            pageTitle +=
              $"for {StateCache.GetStateName(stateCode).ToUpper()} Office Candidates";
          }
          else
          {
            throw new ApplicationException(
              $"States Table needs a row for StateCode:{stateCode}");
            //return PageTitle;
          }
          break;
        case "D":
          if (StateCache.IsValidStateCode(stateCode))
          {
            pageTitle +=
              $"for {StateCache.GetStateName(stateCode).ToUpper()} Office Candidates";
          }
          else
          {
            throw new ApplicationException(
              $"States Table needs a row for StateCode:{stateCode}");
          }
          break;
      }

      return pageTitle;
    }

    protected void ButtonMoveQuestion_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(Textbox_QuestionKey_Move_Question_From,
          Textbox_IssueKey_Move_To);

        var questionKeyFrom = Textbox_QuestionKey_Move_Question_From.Text.Trim();
        var issueKeyTo = Textbox_IssueKey_Move_To.Text.Trim();

        if (!Questions.QuestionKeyExists(questionKeyFrom))
          throw new ApplicationException($"The QuestionKey({questionKeyFrom}) does not exist");

        if (!Issues.IssueKeyExists(issueKeyTo))
          throw new ApplicationException($"The IssueKey({issueKeyTo}) does not exist.");

        // Create a new QuestionKey for the new Issue
        var newQuestionKey = issueKeyTo;
        newQuestionKey += MakeUnique6Digits();

        if (Questions.QuestionKeyExists(newQuestionKey))
          throw new ApplicationException($"The new QuestionKey ({newQuestionKey}) already exist");

        //Update Questions with new QuestionKey and IssueKey
        Questions.UpdateIssueKeyByQuestionKey(issueKeyTo, questionKeyFrom);
        Questions.UpdateQuestionKeyByQuestionKey(newQuestionKey, questionKeyFrom);

        //Update all the Answers with the new QuestionKey
        Answers.UpdateIssueKeyByQuestionKey(issueKeyTo, questionKeyFrom);
        Answers.UpdateQuestionKeyByQuestionKey(newQuestionKey, questionKeyFrom);

        Msg.Text =
          Ok($"Question: ({Questions.GetQuestion(newQuestionKey)}) has been moved to Issue" +
            $" ({Issues.GetIssue(issueKeyTo, Empty)})");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonMoveAnswers_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(Textbox_QuestionKey_Move_Answers_From,
          Textbox_QuestionKey_Move_Answers_To);

        var questionKeyFrom = Textbox_QuestionKey_Move_Answers_From.Text.Trim();
        var questionKeyTo = Textbox_QuestionKey_Move_Answers_To.Text.Trim();

        if (!Questions.QuestionKeyExists(questionKeyFrom))
          throw new ApplicationException($"The QuestionKey ({questionKeyFrom}) does not exist");

        if (!Questions.QuestionKeyExists(questionKeyTo))
          throw new ApplicationException($"The QuestionKey ({questionKeyTo}) does not exist");

        var tableAnswersToMove = Answers.GetDataByQuestionKey(questionKeyFrom)
          .OrderBy(r => r.PoliticianKey)
          .ThenByDescending(r => r.DateStamp);

        foreach (var rowAnswerToMove in tableAnswersToMove)
        {
          // Process each Answer being moved
          // possible existing answer to question by politician
          var tableAnswerByPoliticianToQuestion =
            Answers.GetDataByPoliticianKeyQuestionKey(rowAnswerToMove.PoliticianKey,
              questionKeyTo).OrderByDescending(r => r.DateStamp)
              .ToList();

          //Check if Politician already answered the question
          if (tableAnswerByPoliticianToQuestion.Count == 0)
          {
            // No answer to the same question by the same politician
            ReasignAnswerToQuestionMovingTo(rowAnswerToMove.PoliticianKey);
          }
          else
          {
            // Politician answered both questions. So keep the most recent answer, delete the other
            var rowAnswerByPoliticianToQuestion = tableAnswerByPoliticianToQuestion[0];
            var timeAnswerToMove = rowAnswerToMove.DateStamp;
            var timeAnswerCurrentForPolitician = rowAnswerByPoliticianToQuestion.DateStamp;
            if (timeAnswerToMove > timeAnswerCurrentForPolitician)
            {
              // Answer being moved is more current
              Answers.DeleteByPoliticianKeyQuestionKey(rowAnswerToMove.PoliticianKey,
                questionKeyTo);
              ReasignAnswerToQuestionMovingTo(rowAnswerToMove.PoliticianKey);
            }
            else
            {
              // Answer being moved is older
              //delete the answer being moved
              Answers.DeleteByPoliticianKeyQuestionKey(rowAnswerToMove.PoliticianKey,
                questionKeyFrom);
            }
          }
        }

        Msg.Text =
          Ok($"All Answers for Question ID ({questionKeyFrom} -" + 
            $" {Questions.GetQuestion(questionKeyFrom)}) have been moved to Question ID" +
            $" ({questionKeyTo} - {Questions.GetQuestion(questionKeyTo)})");

        // Needs to be done after Msg - delete question of the answers being moved
        Questions.DeleteByQuestionKey(questionKeyFrom);
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonMatchKeys_Click(object sender, EventArgs e)
    {
      try
      {
        var tableAnswers = Answers.GetAllKeysData();
        var countChanges = 0;
        var countAnswersDeleted = 0;
        foreach (var rowAnswer in tableAnswers)
        {
          if (Questions.QuestionKeyExists(rowAnswer.QuestionKey))
          {
            var correctIssueKey = Questions.GetIssueKey(rowAnswer.QuestionKey);
            if (correctIssueKey != rowAnswer.IssueKey)
            {
              countChanges++;
              Answers.UpdateIssueKeyByPoliticianKeyQuestionKey(correctIssueKey,
                rowAnswer.PoliticianKey, rowAnswer.QuestionKey);
            }
          }
          else
          {
            countAnswersDeleted++;
            Answers.DeleteByPoliticianKeyQuestionKey(rowAnswer.PoliticianKey, rowAnswer.QuestionKey);
          }
        }

        Msg.Text =
          Ok($"{countChanges} IssueKeys were changed. {countAnswersDeleted} Answers were deleted.");
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
      var path = "/admin/QuestionAnswers.aspx";
      if (!IsNullOrWhiteSpace(query)) path += "?" + query;
      Response.Redirect(path);
      Response.End();
      //base.OnPreInit(e);
    }

    private void Page_Load(object sender, EventArgs e)
    {
      _IssueLevel = GetQueryString("IssueLevel");
      if (_IssueLevel == Empty || StateCode == Empty)
        HandleFatalError("The IssueLevel and/or StateCode is missing");

      if (!IsPostBack)
      {
        H1.InnerText = Page.Title = "Move Questions and Answers";

        if (IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks(
            "/admin/QuestionAnswers.aspx?issuelevel=C&state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        #region note

        //Level A = StateCode LL - indicating all politicians
        //Level B = StateCode US - National Issues
        //Level C and higher = StateCode - State Issues

        #endregion note

        try
        {
          PageTitle.Text = PageTitle_QuestionAnswers(StateCode, _IssueLevel);

          HyperLinkReport.NavigateUrl = $"/Admin/IssuesReport.aspx?IssueLevel={_IssueLevel}&Group={StateCode}";
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);
        }
      }
    }
  }
}