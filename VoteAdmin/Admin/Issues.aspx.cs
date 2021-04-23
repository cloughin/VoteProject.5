using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class IssuesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region legacy

    public static string Ok(string msg)
      => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg)
      => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg)
      => $"<span class=\"Msg\">{msg}</span>";

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

    public static string IssueGroup_IssueKey(string issueKey)
    {
      return issueKey.Length >= 3
        ? issueKey.Substring(1, 2).ToUpper()
        : Empty;
    }

    public static string Issues_List(string issueKey)
    {
      if (IsNullOrEmpty(issueKey)) return Empty;
      var issueLevel = Issues.GetIssueLevelFromKey(issueKey);
      var stateCode = IssueGroup_IssueKey(issueKey); //LL, US or StateCode
      return issueLevel + stateCode + "IssuesList";
    }

    private static string Issue_Desc(string issueKey)
      => Issue_Desc(GetPageCache(), issueKey);

    private static string Issue_Desc(PageCache cache, string issueKey)
    {
      if (issueKey == Issues_List(issueKey))
        return "List of Issues";
      return issueKey.ToUpper() == "ALLBIO"
        ? "Biographical Information"
        : cache.Issues.GetIssue(issueKey);
    }

    #endregion legacy

    public override IEnumerable<string> NonStateCodesAllowed { get; } = new[]
      {"LL", "US"};

    private string _IssueKey;
    private string _QuestionKey;

    private void MakeAllControlsNotVisible()
    {
      Table_Issue_Edit.Visible = false;
      Table_Issue_Add.Visible = false;

      Table_Question_Edit.Visible = false;
      Table_Question_Add.Visible = false;
      Table_Question.Visible = false;
      Table_Questions_Report.Visible = false;

      Table_Delete_Question.Visible = false;
      Table_Delete_Issue.Visible = false;
    }

    // general methods

    private static string GetIssueLevelInIssueKey(string issueKey)
    {
      return !IsNullOrEmpty(issueKey)
        ? issueKey.Substring(0, 1)
        : Empty;
    }

    private string MakeIssueKey(string countyCode, string localKey,
      string issue)
    {
      // currently countyCode and localKey are always empty
      //IssueKey is also the first part of the QuestionsKey.
      var issueLevel = Empty;
      switch (StateCode)
      {
        case "LL":
          issueLevel += "A";
          break;
        case "US":
          issueLevel += "B";
          break;
        default:
          if (!IsNullOrEmpty(localKey))
            issueLevel += "E";
          else if (!IsNullOrEmpty(countyCode))
            issueLevel += "D";
          else if (!IsNullOrEmpty(StateCode))
            issueLevel += "C";
          else
            throw new ApplicationException("IssueLevel invalid in Issue_Key");
          break;
      }

      var issueKey = issueLevel + StateCode + issue;
      issueKey = issueKey.StripNonAlphaNumeric();

      return issueKey;
    }

    // Checks
    private void CheckForDangerousInput()
    {
      CheckForDangerousInput(TextBox_Issue_Description, TextBox_Issue_Order,
        TextBox_Issue_Description_Add, TextBox_Issue_Order_Add,
        TextBox_Delete_IssueKey, TextBox_Question_Description,
        TextBox_Question_Order, TextBox_Question_Description_Add,
        TextBox_Question_Order_Add, TextBox_Delete_QuestionKey);
    }

    private void CreateIssuesReport()
    {
      IssuesSummaryReport.GetReport(StateCode).AddTo(IssuesReportPlaceHolder);
    }

    private void CreateQuestionsReport()
    {
      QuestionsSummaryReport.GetReport(_IssueKey)
        .AddTo(QuestionsReportPlaceHolder);
    }

    private void RenumberIssues()
    {
      var tableIssues = Issues.GetDataByStateCode(StateCode)
        .OrderBy(r => r.IssueOrder);
      var count = 10;
      foreach (var rowIssue in tableIssues)
      {
        Issues.UpdateIssueOrder(count, rowIssue.IssueKey);
        count += 10;
      }
    }

    private void RenumberQuestions()
    {
      var tableQuestions = Questions.GetDataByIssueKey(_IssueKey)
        .OrderBy(r => r.QuestionOrder);
      var count = 10;
      foreach (var rowQuestion in tableQuestions)
      {
        Questions.UpdateQuestionOrder(count, rowQuestion.QuestionKey);
        count += 10;
      }
    }

    // Buttons
    //Issues
    protected void ButtonAddIssue_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        if (!Table_Issue_Add.Visible)
        {
          // 1st Step in Adding an Issue

          //Only Add Issue Table Visible
          Table_Issue_Add.Visible = true;
          Table_Issue_Edit.Visible = false;

          TextBox_Issue_Description_Add.Text = Empty;
          TextBox_Issue_Order_Add.Text = Empty;

          Msg.Text =
            Message(
              "To complete the add, enter the issue description and optional order." +
              " Then click the 'Add an Issue' Button.");
        }
        else
        {
          // 2nd Step - Complete the Issue Addition
          var descripton = TextBox_Issue_Description_Add.Text.Trim();
          var order = TextBox_Issue_Order_Add.Text.Trim();

          // Checks
          if (descripton == Empty)
            throw new ApplicationException("The Description Textbox is empty.");
          if (!IsNullOrEmpty(order) && !order.IsValidInteger())
            throw new ApplicationException(
              "The Order Textbox needs to be an integer.");
          if (descripton.Length > 40)
          {
            var tooLongBy = descripton.Length - 40;
            throw new ApplicationException(
              "When adding a new issue, the Issue description must be 40 characters" +
              $" or less. You need to shorten by {tooLongBy} characters");
          }


          // Make IssueKey and check its unique
          var issueKey = MakeIssueKey(Empty, Empty, descripton.SimpleRecase());

          if (Issues.IsValidIssue(issueKey))
            throw new ApplicationException("Issue ({descripton}) already exists");
          _IssueKey = issueKey;

          // Issue Order
          var issueOrder = order != Empty
            ? Convert.ToInt16(order)
            : 1;

          // Add the Issue
          Issues.Insert(_IssueKey, issueOrder, descripton, GetIssueLevelInIssueKey(_IssueKey),
            StateCode, CountyCode, Empty, false, false);

          RenumberIssues();

          //CreateIssuesReport();

          //  Edit Issue Table and load
          Table_Issue_Add.Visible = false;
          Table_Issue_Edit.Visible = true;
          TextBox_Issue_Description.Text = Issue_Desc(_IssueKey);
          TextBox_Issue_Order.Text =
            Issues.GetIssueOrder(_IssueKey, 0).ToString(CultureInfo.InvariantCulture);

          Msg.Text = Ok("The issue has been added and is in the report below.");
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonDeleteIssue_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckForDangerousInput();
        var issueKey = TextBox_Delete_IssueKey.Text.Trim();

        if (!Issues.IsValidIssue(issueKey))
          throw new ApplicationException("The IssueKey is not valid.");

        var icount = Questions.CountByIssueKey(issueKey);
        if (icount > 0)
          throw new ApplicationException(
            $"There are {icount} questions. These need to be reassigned before" +
            " this issue can be deleted.");

        var qcount = Answers.CountByQuestionKey(TextBox_Delete_QuestionKey.Text.Trim());
        if (qcount > 0)
          throw new ApplicationException(
            $"There are {qcount} answers. These need to be reassigned or deleted" +
            " before this question can be deleted.");

        Issues.DeleteByIssueKey(issueKey);

        TextBox_Delete_IssueKey.Text = Empty;

        RenumberIssues();

        //CreateIssuesReport();

        // Reset Tables Not Visible
        Table_Question_Edit.Visible = false;
        Table_Question_Add.Visible = false;
        Table_Question.Visible = false;
        Table_Delete_Question.Visible = false;
        Table_Questions_Report.Visible = false;


        Table_Issue_Edit.Visible = false;
        Table_Issue_Add.Visible = false;

        Msg.Text = Ok("The issue has been deleted.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    //Questions

    protected void ButtonAddQuestion_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        if (!Table_Question_Add.Visible)
        {
          // 1st Step in Adding an Question

          //Only Add Question Table Visible
          Table_Question_Add.Visible = true;
          Table_Question_Edit.Visible = false;

          TextBox_Question_Description_Add.Text = Empty;
          TextBox_Question_Order_Add.Text = Empty;

          Msg.Text =
            Message(
              "To complete the add, enter the question and optional order." +
              " Then click the 'Add a Question' Button.");
        }
        else
        {
          // 2nd Step - Complete the Question Addition

          // Checks
          if (TextBox_Question_Description_Add.Text.Trim() == Empty)
            throw new ApplicationException("The Question Textbox is empty.");
          if (!IsNullOrEmpty(TextBox_Question_Order_Add.Text.Trim()) &&
            !TextBox_Question_Order_Add.Text.Trim().IsValidInteger())
            throw new ApplicationException(
              "The Order Textbox needs to be an integer.");
          if (TextBox_Question_Description_Add.Text.Trim().Length > 80)
          {
            var tooLongBy =
              TextBox_Question_Description_Add.Text.Trim().Length - 80;
            throw new ApplicationException(
              "The question must be 80 characters or less. You need to" + 
              $" shorten by {tooLongBy} characters");
          }


          // Make QuestionKey and check its unique
          var questionKey = _IssueKey + MakeUnique6Digits();
          var question = TextBox_Question_Description_Add.Text.Trim();
          var order = TextBox_Question_Order_Add.Text.Trim();

          if (Questions.QuestionKeyExists(questionKey))
            throw new ApplicationException($"Question ({question}) already exists");
          _QuestionKey = questionKey;

          // Question Order
          var questionOrder = order != Empty
              ? Convert.ToInt16(order)
              : 1;

          // Add the Question
          Questions.Insert(_QuestionKey, _IssueKey, questionOrder, question, false);

          RenumberQuestions();

          CreateQuestionsReport();

          // Edit Question Table Visible and load
          Table_Question_Add.Visible = false;
          Table_Question_Edit.Visible = true;
          TextBox_Question_Description.Text = Questions.GetQuestion(_QuestionKey);
          TextBox_Question_Order.Text =
            Questions.GetQuestionOrder(_QuestionKey, 0)
              .ToString(CultureInfo.InvariantCulture);

          Msg.Text =
            Ok("The question has been added and is in the report below.");
          TextBox_Question_Description.Text = Empty;
          TextBox_Question_Order.Text = Empty;
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonDeleteQuestion_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckForDangerousInput();
        var questionKey = TextBox_Delete_QuestionKey.Text.Trim();

        if (!Questions.QuestionKeyExists(questionKey))
          throw new ApplicationException("The QuestionKey is not valid.");

        var count = Answers.CountByQuestionKey(questionKey);
        if (count > 0)
          throw new ApplicationException(
            $"There are {count} answers. These need to be reassigned or" +
            " deleted before this question can be deleted.");

        // Msg needs to come before question is deleted
        Msg.Text =
          Ok($"The question ({Questions.GetQuestion(questionKey)})" + 
          " has been deleted.");

        Questions.DeleteByQuestionKey(questionKey);

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (Questions.CountByIssueKey(_IssueKey) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        TextBox_Delete_QuestionKey.Text = Empty;
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonDeleteAnswers_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckForDangerousInput();
        var questionKey = TextBox_Delete_QuestionKey.Text.Trim();

        if (!Questions.QuestionKeyExists(questionKey))
          throw new ApplicationException("The QuestionKey is not valid.");

        Answers.DeleteByQuestionKey(questionKey);

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (Questions.CountByIssueKey(_IssueKey) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        Msg.Text =
          Ok(
            $"All the ANSWERS to question ({Questions.GetQuestion(questionKey)}) have been deleted.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonDeleteAllQuestions_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        var tableQuestions = Questions.GetDataByIssueKey(_IssueKey);
        foreach (var rowQuestion in tableQuestions)
          Questions.DeleteByQuestionKey(rowQuestion.QuestionKey);

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (Questions.CountByIssueKey(_IssueKey) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        Msg.Text = Ok("The questions with no answers have been deleted.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    // Textboxes
    //Issues
    protected void TextBoxIssueDescription_TextChanged(object sender,
      EventArgs e)
    {
      try
      {
        // checks
        CheckForDangerousInput();
        var description = TextBox_Issue_Description.Text.Trim();

        if (description == Empty)
          throw new ApplicationException(
            "The Issue Description Textbox is empty.");

        if (description.Length > 40)
        {
          var tooLongBy = description.Length - 40;
          throw new ApplicationException(
            $"The Issue description must be 40 characters or less. You need to shorten by {tooLongBy} characters");
        }

        Issues.UpdateIssue(description, _IssueKey);
        Table_Issue_Edit.Visible = false;
        //CreateIssuesReport();
        Msg.Text = Ok("The issue description has been updated");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxIssueOrder_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // Checks
        CheckForDangerousInput();
        var order = TextBox_Issue_Order.Text.Trim();

        if (order == Empty)
          throw new ApplicationException("The Order Textbox can not be empty.");

        if (!order.IsValidInteger())
          throw new ApplicationException("The Order Textbox needs to be an integer.");

        var issueOrder = IsNullOrEmpty(order)
          ? 1
          : Convert.ToInt16(order);
        Issues.UpdateIssueOrder(issueOrder, _IssueKey);

        Table_Issue_Edit.Visible = false;
        RenumberIssues();
        //CreateIssuesReport();
        Msg.Text = Ok("The issue order has been updated");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    //Questions
    protected void TextBoxQuestionDescription_TextChanged(object sender,
      EventArgs e)
    {
      try
      {
        // checks
        CheckForDangerousInput();
        var description = TextBox_Question_Description.Text.Trim();
        if (description == Empty)
          throw new ApplicationException("The Question Textbox is empty.");
        Questions.UpdateQuestion(description, _QuestionKey);
        Table_Question_Edit.Visible = false;
        CreateQuestionsReport();
        Msg.Text = Ok("The question has been updated");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxQuestionOrder_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // Checks
        CheckForDangerousInput();
        var order = TextBox_Question_Order.Text.Trim();

        if (order == Empty)
          throw new ApplicationException("The Order Textbox can not be empty.");

        if (!order.IsValidInteger())
          throw new ApplicationException(
            "The Order Textbox needs to be an integer.");

        Questions.UpdateQuestionOrder(Convert.ToInt16(order), _QuestionKey);

        Table_Question_Edit.Visible = false;
        RenumberQuestions();
        CreateQuestionsReport();
        Msg.Text = Ok("The question order has been updated");
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
      var path = "/admin/UpdateIssues.aspx";
      if (!IsNullOrWhiteSpace(query)) path += "?" + query;
      Response.Redirect(path);
      Response.End();
      //base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _IssueKey = QueryIssue;
      _QuestionKey = QueryQuestion;

      if (!IsPostBack)
      {
        H1.InnerText = Page.Title = "Issues and Questions";

        if (IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/Issues.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        try
        {
          // Page Title
          PageTitle.Text = Empty;

          if (StateCode == "LL")
            PageTitle.Text += "<br>All Candidates";
          else if (StateCode == "US")
            PageTitle.Text += "<br>National Candidates";
          else
            PageTitle.Text +=
              $"<br>{StateCache.GetStateName(StateCode)} Candidates";

          if (!IsNullOrEmpty(_IssueKey))
          {
            PageTitle.Text +=
              $"<br><span style=color:red>{Issue_Desc(_IssueKey)}</span>";
          }

          //On initial entry there is only a StateCode Anchor
          //Anchors on Issues and Questions Reports have 4 possible query string prameters:
          //Issue Link => db.QueryString("Issue")
          //Issue Ok/Omit Link => db.QueryString("Issue") & db.QueryString("Omit")
          //Question Link => db.QueryString("Issue") & db.QueryString("Question")
          //Question Ok/Omit Link => db.QueryString("Issue") & db.QueryString("Question") & db.QueryString("Omit")

          if (IsNullOrEmpty(QueryIssue))
          {
            MakeAllControlsNotVisible();
          }
          else
          {
            // Entry from Link on Issues or Question Reports
            if (IsNullOrEmpty(QueryQuestion) &&
              IsNullOrEmpty(GetQueryString("Omit")))
            {
              // Issue Link to edit issue and/or issue questions
              MakeAllControlsNotVisible();
              Table_Issue_Edit.Visible = true;
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              TextBox_Issue_Description.Text = Issue_Desc(QueryIssue);
              TextBox_Issue_Order.Text =
                Issues.GetIssueOrder(QueryIssue, 0)
                  .ToString(CultureInfo.InvariantCulture);

              CreateQuestionsReport();
            }
            else if (IsNullOrEmpty(QueryQuestion) &&
              !IsNullOrEmpty(GetQueryString("Omit")))
            {
              // Issue Ok/Omit Link to change status to ok or omit

              MakeAllControlsNotVisible();

              Issues.UpdateIsIssueOmit(GetQueryString("Omit").IsEqIgnoreCase("true"),
                QueryIssue);
            }
            else if (!IsNullOrEmpty(QueryQuestion) &&
              IsNullOrEmpty(GetQueryString("Omit")))
            {
              // Question Link to edit question

              MakeAllControlsNotVisible();
              Table_Question_Edit.Visible = true;
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              TextBox_Question_Description.Text = Questions.GetQuestion(_QuestionKey);
              TextBox_Question_Order.Text =
                Questions.GetQuestionOrder(_QuestionKey, 0)
                  .ToString(CultureInfo.InvariantCulture);

              RenumberQuestions();

              CreateQuestionsReport();
            }
            else if (!IsNullOrEmpty(QueryQuestion) &&
              !IsNullOrEmpty(GetQueryString("Omit")))
            {
              // Question Ok/Omit Link to change status to ok or omit

              MakeAllControlsNotVisible();
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              Questions.UpdateIsQuestionOmit(GetQueryString("Omit").IsEqIgnoreCase("true"),
                QueryQuestion);

              RenumberQuestions();

              CreateQuestionsReport();
            }
          }

          RenumberIssues();

          // Only Master controls
          if (IsSuperUser)
          {
            Table_Delete_Issue.Visible = true;

            if (!IsNullOrEmpty(QueryIssue) &&
              IsNullOrEmpty(GetQueryString("Omit")))
              Table_Delete_Question.Visible = true;
          }
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          LogAdminError(ex);
        }
      }
      CreateIssuesReport();
    }
  }
}