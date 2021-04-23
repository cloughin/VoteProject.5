using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  /// <summary>
  /// Summary description for IssueQuestionsTable.
  /// </summary>
  public partial class QuestionsPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    public static string Answers4Question(string questionKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Answers.QuestionKey ";
      sql += ",Answers.PoliticianKey ";
      sql += " FROM Answers ";
      sql += " WHERE Answers.QuestionKey = " + SqlLit(questionKey);
      return sql;
    }

    public static string Question4Issue2Delete(string issueKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " QuestionKey ";
      sql += " ,Question ";
      //sql += " ,xIsQuestionTagForDeletion ";
      sql += " FROM Questions ";
      sql += " WHERE IssueKey = " + SqlLit(issueKey);
      //sql += " AND xIsQuestionTagForDeletion = 1";
      sql += " ORDER BY QuestionOrder";
      return sql;
    }

    public static string Questions4IssueAnyStatus(string issueKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Questions.QuestionKey ";
      sql += " ,Questions.Question ";
      sql += " ,Questions.QuestionOrder ";
      sql += " ,Questions.IsQuestionOmit ";
      //sql += " ,Questions.xIsQuestionTagForDeletion ";
      sql += " FROM Questions ";
      sql += " WHERE Questions.IssueKey = " + SqlLit(issueKey);
      sql += " ORDER BY Questions.QuestionOrder";
      return sql;
    }

    public static string Ok(string msg) =>
      $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) =>
      $"<span class=\"Msg\">{msg}</span>";

    public static void Log_Error_Admin(Exception ex, string message = null)
    {
      var logMessage = string.Empty;
      var stackTrace = string.Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!string.IsNullOrWhiteSpace(message))
      {
        if (!string.IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    public static bool Is_Valid_Integer(string number2Check)
    {
      int value;
      return int.TryParse(number2Check, out value);
    }

    public static bool Is_TextBox_Html(TextBox textBox) => 
      (textBox.Text.IndexOf("<", StringComparison.Ordinal) >= 0)
      || (textBox.Text.IndexOf("/>", StringComparison.Ordinal) >= 0);

    public static bool Is_Str_Script(string strToCheck) => 
      strToCheck.Trim().ToUpper().IndexOf("<SCRIPT", StringComparison.Ordinal) >= 0;

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    public static Random RandomObject;

    public static char GetRandomDigit()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      return Convert.ToChar(RandomObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string MakeUnique6Digits()
    {
      var password = string.Empty;
      //Make a DDDDDD password
      for (var n = 0; n < 6; n++)
        password += GetRandomDigit();
      return password;
    }

    public static string Url_Admin_IssuesReport(string issueLevel, string groupCode) =>
      $"/Admin/IssuesReport.aspx?IssueLevel={issueLevel}&Group={groupCode}";

    private static string Url_Admin_Questions() => "/Admin/Questions.aspx";

    public static string Url_Admin_Questions(string issueGroup, string issueKey,
      string questionKey) =>
      $"{Url_Admin_Questions()}?Group={issueGroup}&Issue={issueKey}&Question={questionKey}";

    public static int Rows(string sql)
    {
      var table = G.Table(sql);
      return table?.Rows.Count ?? 0;
    }

    public static DataRow Row(string sql)
    {
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
        return table.Rows[0];
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a unique row for this Id."));
    }

    public static string Single_Key_Str(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());

      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        // modified to pass through nulls (Politician.Address etc)
        //return (string)table.Rows[0][Column].ToString().Trim();
        var result = table.Rows[0][column] as string;
        result = result?.Trim();
        return result;
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static string Issues_Str(string issueKey, string column) => 
      (issueKey != string.Empty) && !Is_IssueKey_IssuesList(issueKey)
        ? Single_Key_Str("Issues", column, "IssueKey", issueKey)
        : string.Empty;

    public static bool Is_IssueKey_IssuesList(string issueKey) => 
      (issueKey.Length == 13) && (issueKey.Substring(3, 10).ToUpper() == "ISSUESLIST");

    public static string Issues_Issue(string issueKey) => 
      !string.IsNullOrEmpty(issueKey)
      ? Issues_Str(issueKey, "Issue")
      : string.Empty;

    public static void Throw_Exception_TextBox_Html(TextBox textBox)
    {
      if (Is_TextBox_Html(textBox))
        throw new ApplicationException(
          "Text in a textbox appears to be HTML because it contains an opening or closing HTML tag (< or />). Please remove and try again.");
    }

    public static void Throw_Exception_TextBox_Script(TextBox textBox)
    {
      if (Is_Str_Script(textBox.Text))
        throw new ApplicationException("Text in the textbox is illegal.");
    }

    //public static void Throw_Exception_TextBox_Html_Or_Script(TextBox textBox)
    //{
    //  Throw_Exception_TextBox_Html(textBox);
    //  Throw_Exception_TextBox_Script(textBox);
    //}

    private static string PageTitle4Questions(string stateCode, string issueKey, string issueLevel)
    {
      var pageTitle = Issues_Issue(issueKey) + " QUESTIONS";
      switch (issueLevel)
      {
        case "A":
          pageTitle += "<br>for ALL Offices";
          break;
        case "B":
          pageTitle += "<br>for ALL NATIONAL Offices";
          break;
        case "C":
          pageTitle += "<br>for " + StateCache.GetStateName(stateCode) + " Offices";
          break;
      }

      return pageTitle;
    }

    private static string Url_Admin_QuestionAnswers(string issueLevel, string stateCode) =>
      $"/Admin/QuestionAnswers.aspx?IssueLevel={issueLevel}&State={stateCode}";

    private static string Anchor_AdminQuestions(string issueGroup, string issueKey,
      string questionKey, string anchorText)
    {
      var anchor = string.Empty;
      anchor += "<a href=";
      anchor += "\"";
      anchor += Url_Admin_Questions(issueGroup, issueKey, questionKey);
      anchor += "\"";
      anchor += ">";
      anchor += "<nobr>" + anchorText + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    #endregion from db

    private void CheckOrder()
    {
      if (QuestionOrder.Text.Trim() == string.Empty) //create an order as last in list
      {
        var questionsCount = Rows(Questions4IssueAnyStatus(ViewState["IssueKey"].ToString()));
        var rowNumber = (questionsCount + 1) * 10;
        QuestionOrder.Text = rowNumber.ToString(CultureInfo.InvariantCulture);
      }
      if (!Is_Valid_Integer(QuestionOrder.Text.Trim()))
        throw new ApplicationException("The Order needs to be a whole number.");
    }

    private void CheckQuestion()
    {
      if (Question.Text == string.Empty)
        throw new ApplicationException("Question field was empty.");
    }

    private void CheckQuestionNotExist()
    {
      var sql = "SELECT Question FROM Questions"
        + " WHERE Question = " + SqlLit(Question.Text.Trim())
        + " AND SUBSTRING(QuestionKey,1,1) = " + SqlLit(ViewState["IssueLevel"].ToString());
      var questionRows = Rows(sql);
      if (questionRows > 0)
        throw new ApplicationException("This is a duplicate question.");
    }

    private static void Check4Answers(string questionKey)
    {
      var answers = Rows(Answers4Question(questionKey));
      if (answers > 0)
        throw new ApplicationException(
          //"[" + G.Questions_Str(questionKey, "Question") + "]"
          "[" + Questions.GetQuestionByQuestionKey(questionKey, string.Empty) + "]"
          + " was not deleted."
          + " There are " + answers + " Answers for this question."
          + " You need to move the answers to a different question."
          + " Click the [Move Questions Answers] Button to do this.");
    }

    private void LoadControls()
    {
      //DataRow QuestionRow = db.Row(sql.Question(ViewState["QuestionKey"].ToString()));
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Questions.QuestionKey ";
      sql += " ,Questions.IssueKey ";
      sql += " ,Questions.QuestionOrder ";
      sql += " ,Questions.Question ";
      sql += " ,Questions.IsQuestionOmit ";
      sql += " FROM Questions ";
      sql += " WHERE QuestionKey = " + SqlLit(ViewState["QuestionKey"].ToString());
      var questionRow = Row(sql);

      Question.Text = questionRow["Question"].ToString();
      QuestionOrder.Text = questionRow["QuestionOrder"].ToString();

      if ((bool) questionRow["IsQuestionOmit"])
        RadioButtonListOmit.SelectedValue = "Yes";
      else
        RadioButtonListOmit.SelectedValue = "No";

      //if ((bool)QuestionRow["xIsQuestionTagForDeletion"])
      //  RadiobuttonlistDelete.SelectedValue = "Yes";
      //else
      //  RadiobuttonlistDelete.SelectedValue = "No";
    }

    private void ClearControls()
    {
      Question.Text = string.Empty;
      QuestionOrder.Text = string.Empty;
      RadioButtonListOmit.SelectedValue = "No";
      RadiobuttonlistDelete.SelectedValue = "No";
    }

    private string EditRadioButtonListOmit() => 
      RadioButtonListOmit.SelectedValue == "Yes"
      ? "1"
      : "0";

    private void ShowQuestions()
    {
      var questionsHtmlTable = string.Empty;
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

      var questionsTable = G.Table(Questions4IssueAnyStatus(ViewState["IssueKey"].ToString()));

      if (questionsTable.Rows.Count > 0)
      {
        foreach (DataRow questionRow in questionsTable.Rows)
        {
          questionsHtmlTable += "<tr>";

          var omitAndDelete = string.Empty;
          if ((bool) questionRow["IsQuestionOmit"])
            omitAndDelete += " O";
          //if ((bool)QuestionRow["xIsQuestionTagForDeletion"] == true)
          //  OmitAndDelete += " D";

          questionsHtmlTable += "<td class=tdReportDetail>";
          questionsHtmlTable += omitAndDelete;
          questionsHtmlTable += "</td>";

          questionsHtmlTable += "<td class=tdReportDetail>";
          questionsHtmlTable += questionRow["QuestionOrder"].ToString();
          questionsHtmlTable += "</td>";

          var sqlText = "Answers ";
          sqlText += " WHERE Answers.QuestionKey = " + SqlLit(questionRow["QuestionKey"].ToString());
          //int Answers = db.Rows_Sql(sql.Answers4Question(QuestionRow["QuestionKey"].ToString()));
          var answers = G.Rows_Count_From(sqlText);
          questionsHtmlTable += "<td class=tdReportDetail>";
          //QuestionsHTMLTable += db.Anchor("/Admin/Questions.aspx"
          //  + "?Question=" + QuestionRow["QuestionKey"].ToString()
          //  + "&Issue=" + ViewState["IssueKey"].ToString(),
          //  QuestionRow["Question"].ToString()
          //  + " (" + Answers.ToString() + ")"
          //  );
          questionsHtmlTable += Anchor_AdminQuestions(
            ViewState["IssuesGroup"].ToString()
            , ViewState["IssueKey"].ToString()
            , questionRow["QuestionKey"].ToString()
            , questionRow["Question"] + " (" + answers + ")"
          );

          questionsHtmlTable += "</td>";

          questionsHtmlTable += "</tr>";
          ViewState["HighestOrder"] = questionRow["QuestionOrder"];
        }
      }
      else
      {
        questionsHtmlTable += "<tr>";
        questionsHtmlTable += "<td  class=tdReportDetail colspan=3>";
        questionsHtmlTable += "No Questions";
        questionsHtmlTable += "</td>";
        questionsHtmlTable += "</tr>";

        ViewState["HighestOrder"] = 0;
      }
      questionsHtmlTable += "</table>";
      LabelQuestionsTable.Text = questionsHtmlTable;
    }

    protected void RadioButtonListOmit_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (Question.Text == string.Empty)
          throw new ApplicationException(
            "You need to select a question before you can change its status.");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());

        var updateSql =
          $"UPDATE Questions SET  IsQuestionOmit ={EditRadioButtonListOmit()}" +
          $" WHERE QuestionKey = {SqlLit(ViewState["QuestionKey"].ToString())}";
        G.ExecuteSql(updateSql);
        ShowQuestions();

        Msg.Text = Ok(
          $"Question ({Questions.GetQuestionByQuestionKey(ViewState["QuestionKey"].ToString(), string.Empty)})" +
          " will be omitted on all pages.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void RadiobuttonlistDelete_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (Question.Text == string.Empty)
          throw new ApplicationException(
            "You need to select a question before you can change its status.");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());

        //string UpdateSQL = "UPDATE Questions SET "
        //  + " xIsQuestionTagForDeletion =" + EditIsRadiobuttonlistDelete()
        //  + " WHERE QuestionKey = " + db.SQLLit(ViewState["QuestionKey"].ToString());
        //db.ExecuteSQL(UpdateSQL);
        ShowQuestions();

        Msg.Text = Ok("Question ("
          + Questions.GetQuestionByQuestionKey(ViewState["QuestionKey"].ToString(), string.Empty)
          + ") is tagged for deletion.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonUpdate_Click1(object sender, EventArgs e)
    {
      try
      {
        #region Check TextBoxes for Illegal Stuff

        Throw_Exception_TextBox_Script(QuestionOrder);
        Throw_Exception_TextBox_Html(QuestionOrder);

        Throw_Exception_TextBox_Script(Question);
        Throw_Exception_TextBox_Html(Question);

        #endregion Check TextBoxes for Illegal Stuff

        #region Edit Checks for Add and Update

        CheckOrder();
        CheckQuestion();

        #endregion

        #region Remove Issue.aspx cached pages for the Issue

        #region commented out

        //switch (ViewState["IssueLevel"].ToString())
        //{
        //  case db.AllOffices:
        //  //Reasons & Objectives remove all pages with an IssueKey = ALLPersonal
        //    db.Cache_RemoveIssuePages4IssueLevel(ViewState["IssueLevel"].ToString());
        //    break;
        //  case db.NationalOffices:
        //    break;
        //  case db.StateOffices:
        //    break;
        //  case db.CountyOffices://not implemented
        //    break;
        //}

        #endregion

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());

        #endregion

        #region Update QUESTION

        var updateSql =
          $"UPDATE Questions SET  Question = {SqlLit(Question.Text.Trim())}," +
          $"QuestionOrder = {SqlLit(QuestionOrder.Text.Trim())}" +
          $" WHERE QuestionKey = {SqlLit(ViewState["QuestionKey"].ToString())}";
        G.ExecuteSql(updateSql);

        #endregion

        ClearControls();

        ButtonUpdate.Visible = false;
        ButtonClear.Visible = true;
        ButtonAdd.Visible = true;

        ShowQuestions();

        Msg.Text = Ok("Question ("
          + Questions.GetQuestionByQuestionKey(ViewState["QuestionKey"].ToString(), string.Empty)
          + ") was UPDATED.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
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
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonAdd_Click1(object sender, EventArgs e)
    {
      try
      {
        #region Check TextBoxes for Illegal Stuff

        Throw_Exception_TextBox_Script(QuestionOrder);
        Throw_Exception_TextBox_Html(QuestionOrder);

        Throw_Exception_TextBox_Script(Question);
        Throw_Exception_TextBox_Html(Question);

        #endregion Check TextBoxes for Illegal Stuff

        #region Edit Checks for Add and Update

        CheckOrder();
        CheckQuestion();
        CheckQuestionNotExist();

        #endregion

        ViewState["QuestionKey"] = ViewState["IssueKey"] + MakeUnique6Digits();

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());

        #region Add the Question

        var insertSql =
          "INSERT INTO Questions (QuestionKey,IssueKey,Question,QuestionOrder)" +
          $" VALUES({SqlLit(ViewState["QuestionKey"].ToString())}," +
          $"{SqlLit(ViewState["IssueKey"].ToString())}," +
          $"{SqlLit(Question.Text.ReplaceNewLinesWithEmptyString())}," +
          $"{QuestionOrder.Text})";
        G.ExecuteSql(insertSql);

        #endregion

        ClearControls();

        ShowQuestions();

        Msg.Text = Ok("Question ("
          + Questions.GetQuestionByQuestionKey(ViewState["QuestionKey"].ToString(), string.Empty)
          + ") was ADDED.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonRenumber_Click1(object sender, EventArgs e)
    {
      try
      {
        var questionsTable = G.Table(Questions4IssueAnyStatus(ViewState["IssueKey"].ToString()));
        if (questionsTable.Rows.Count > 0)
        {
          var count = 10;
          foreach (DataRow questionRow in questionsTable.Rows)
          {
            var updateSql =
              $"UPDATE Questions SET  QuestionOrder = {count}" +
              $" WHERE QuestionKey= {SqlLit(questionRow["QuestionKey"].ToString())}";
            G.ExecuteSql(updateSql);
            count += 10;
          }
        }

        ShowQuestions();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonMoveQuestion_Click1(object sender, EventArgs e) => 
      Response.Redirect(Url_Admin_QuestionAnswers(ViewState["IssueLevel"].ToString(), 
        ViewState["IssuesGroup"].ToString()));

    protected void ButtonDeleteTaggedQuestions_Click1(object sender, EventArgs e)
    {
      try
      {
        var questions2DeleteTable = G.Table(Question4Issue2Delete(
          ViewState["IssueKey"].ToString()));
        foreach (DataRow question2DeleteRow in questions2DeleteTable.Rows)
        {
          Check4Answers(question2DeleteRow["QuestionKey"].ToString());
          var sqlText = "DELETE FROM Questions WHERE QuestionKey = "
            + SqlLit(question2DeleteRow["QuestionKey"].ToString());
          G.ExecuteSql(sqlText);
        }
        ShowQuestions();
        Msg.Text =
          Ok("All questions tagged for deletion have been physically deleted in the database.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        #region Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  SecurePage.HandleSecurityException();

        #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        Title = H1.InnerText = "Questions";

        #region ViewState Values

        ViewState["IssueLevel"] = string.Empty;
          // A = All Offices, B = National Issues, C = State Issues
        ViewState["IssuesGroup"] = string.Empty;
          //Level A = LL - indicating all politicians; Level B = US; Level C and higher = StateCode
        ViewState["IssueKey"] = string.Empty;
        ViewState["QuestionKey"] = string.Empty;

        #region ViewState["IssuesGroup"] ViewState["IssueKey"] ViewState["IssueLevel"] ViewState["QuestionKey"] QueryString

        if (!string.IsNullOrEmpty(GetQueryString("Group")))
          ViewState["IssuesGroup"] = GetQueryString("Group");

        if (!string.IsNullOrEmpty(QueryIssue))
        {
          ViewState["IssueKey"] = QueryIssue;
          ViewState["IssueLevel"] = Issues_Str(ViewState["IssueKey"].ToString(), "IssueLevel");
        }

        if (!string.IsNullOrEmpty(QueryQuestion))
          ViewState["QuestionKey"] = QueryQuestion;

        #endregion

        #endregion ViewState Values

        #region Additional Security Checks

        if (
          (ViewState["IssuesGroup"].ToString() == string.Empty)
          || (ViewState["IssueKey"].ToString() == string.Empty)
        )
          HandleFatalError("The IssueGroup and/or IssueKey is missing");

        #endregion Security Checks

        try
        {
          #region Page Title

          //PageTitle.Text = db.Issues_Str(ViewState["IssueKey"].ToString(), "Issue") + " QUESTIONS";
          //if (ViewState["IssueLevel"].ToString() == "A")
          //  PageTitle.Text += "<br>for ALL Offices";
          //else if (ViewState["IssueLevel"].ToString() == "B")
          //  PageTitle.Text += "<br>for ALL NATIONAL Offices";
          //else if (ViewState["IssueLevel"].ToString() == "c")
          //  PageTitle.Text += "<br>for " + db.States_Str(ViewState["IssuesGroup"].ToString(), "State") + " Offices";

          #endregion

          PageTitle.Text = PageTitle4Questions(ViewState["IssuesGroup"].ToString()
            , ViewState["IssueKey"].ToString()
            , ViewState["IssueLevel"].ToString());

          HyperLinkReport.NavigateUrl = Url_Admin_IssuesReport(
            ViewState["IssueLevel"].ToString()
            , ViewState["IssuesGroup"].ToString());


          if (ViewState["QuestionKey"] as string == string.Empty)
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
            Msg.Text = Message("You can now either: (1) change the Order or Question"
              + " and click Update"
              + " or (2) chage the Status by clicking a radio button"
              +
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
          Log_Error_Admin(ex);

          #endregion
        }
      }
    }

    #region Dead code

    //private string EditIsRadiobuttonlistDelete()
    //{
    //  if (RadiobuttonlistDelete.SelectedValue == "Yes")
    //    return "1";
    //  else
    //    return "0";
    //}

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
    //      Msg.Text = db.Fail(Session["ErrNavBarAdmin"].ToString());
    //  }
    //  catch /*(Exception ex)*/
    //  {
    //  }
    //}

    #endregion Dead code
  }
}