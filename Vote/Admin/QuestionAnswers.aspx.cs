using System;
using System.Data;
using System.Web.UI.WebControls;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class QuestionAnswers : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    public static string Issue(string issueKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " IssueKey ";
      sql += " ,Issue ";
      sql += " ,IssueOrder ";
      sql += " ,IsIssueOmit ";
      //sql += " ,IsIssueTagForDeletion ";
      sql += " FROM Issues ";
      sql += " WHERE IssueKey = " + SqlLit(issueKey);
      return sql;
    }

    public static string Ok(string msg) =>
      $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

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
      "Database Failure for SQL Statement::" + sql + " :: Error Msg:: " + err;

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

    public static DataRow Row_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count == 1
        ? table.Rows[0]
        : null;
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
      (issueKey != string.Empty)
      && !Is_IssueKey_IssuesList(issueKey)
        ? Single_Key_Str("Issues", column, "IssueKey", issueKey)
        : string.Empty;

    public static bool Is_IssueKey_IssuesList(string issueKey) => 
      (issueKey.Length == 13) && (issueKey.Substring(3, 10).ToUpper() == "ISSUESLIST");

    public static string Issues_Issue(string issueKey) => 
      !string.IsNullOrEmpty(issueKey)
      ? Issues_Str(issueKey, "Issue")
      : string.Empty;

    public static bool Is_Valid_Question(string questionKey)
    {
      //if (db.Row_Optional(sql.QuestionKey(QuestionKey)) != null)
      var sql =
        $" SELECT Questions.QuestionKey FROM Questions WHERE Questions.QuestionKey ={SqlLit(questionKey)}";
      return Row_Optional(sql) != null;
    }

    public static string Questions_Str(string questionKey, string column)
    {
      if (questionKey != string.Empty)
        return Single_Key_Str("Questions", column, "QuestionKey", questionKey);
      return string.Empty;
    }

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

    private static DataRow Row_Question_Optional(string questionKey)
    {
      var sql =
        "SELECT Questions.QuestionKey,Questions.IssueKey,Questions.QuestionOrder," +
        "Questions.Question,Questions.IsQuestionOmit FROM Questions" +
        $" WHERE QuestionKey={SqlLit(questionKey)}";
      return Row_Optional(sql);
    }

    #endregion from db

    private void Reasign_Answer_To_Question_Moving_To(string politicianKey)
    {
      var issueKeyForMoveToQuestionKey = Questions_Str(
        Textbox_QuestionKey_Move_Answers_To.Text.Trim(),
        "IssueKey");

      var sqlupdate = string.Empty;
      sqlupdate += "UPDATE Answers";
      sqlupdate += " SET QuestionKey = " + SqlLit(Textbox_QuestionKey_Move_Answers_To.Text.Trim());
      sqlupdate += " ,IssueKey = " + SqlLit(issueKeyForMoveToQuestionKey);
      sqlupdate += " WHERE QuestionKey = " +
        SqlLit(Textbox_QuestionKey_Move_Answers_From.Text.Trim());
      sqlupdate += " AND PoliticianKey = " + SqlLit(politicianKey);
      G.ExecuteSql(sqlupdate);
    }

    private static void Delete_Answer(string politicianKey, string questionKey)
    {
      var sqldelete = string.Empty;
      sqldelete += "DELETE FROM Answers";
      sqldelete += " WHERE PoliticianKey = " + SqlLit(politicianKey);
      sqldelete += " AND QuestionKey = " + SqlLit(questionKey);
      G.ExecuteSql(sqldelete);
    }

    private static string PageTitle_QuestionAnswers(string stateCode, string issueLevel)
    {
      var pageTitle = "Move Questions and Answers";
      pageTitle += "<br>";
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
            pageTitle += "for "
              + StateCache.GetStateName(stateCode).ToUpper()
              + " Office Candidates";
          }
          else
          {
            throw new ApplicationException("States Table needs a row for StateCode:" + stateCode);
            //return PageTitle;
          }
          break;
        case "D":
          if (StateCache.IsValidStateCode(stateCode))
          {
            pageTitle += "for "
              + StateCache.GetStateName(stateCode).ToUpper()
              + " Office Candidates";
          }
          else
          {
            throw new ApplicationException("States Table needs a row for StateCode:" + stateCode);
          }
          break;
      }

      return pageTitle;
    }

    protected void ButtonMoveQuestion_Click1(object sender, EventArgs e)
    {
      try
      {
        #region Checks

        Throw_Exception_TextBox_Script(Textbox_QuestionKey_Move_Question_From);
        Throw_Exception_TextBox_Html(Textbox_QuestionKey_Move_Question_From);

        Throw_Exception_TextBox_Script(Textbox_IssueKey_Move_To);
        Throw_Exception_TextBox_Html(Textbox_IssueKey_Move_To);

        //if (db.Row_Optional(sql.Question(Textbox_QuestionKey_Move_Question_From.Text.Trim())) == null)
        if (Row_Question_Optional(Textbox_QuestionKey_Move_Question_From.Text.Trim()) == null)
          throw new ApplicationException("The QuestionKey(" +
            Textbox_QuestionKey_Move_Question_From.Text.Trim() + ") does not exist");

        if (Row_Optional(Issue(Textbox_IssueKey_Move_To.Text.Trim())) == null)
          throw new ApplicationException("The IssueKey(" + Textbox_IssueKey_Move_To.Text.Trim() +
            ") does not exist.");

        #endregion

        #region Remove all Cached pages for IssueKey - Needs to be done first before old question is deleted

        #region Remove Issue.aspx cached pages

        //db.Cache_Remove_Issue_Pages(Textbox_IssueKey_Move_To.Text.Trim());
        //db.Cache_Remove_Issue_Pages(db.Questions_IssueKey(Textbox_QuestionKey_Move_Question_From.Text.Trim()));

        #endregion

        #region Remove PoliticianIssue.aspx cached pages

        //db.Cache_Remove_PoliticianIssue_Issue(Textbox_IssueKey_Move_To.Text.Trim());
        //db.Cache_Remove_PoliticianIssue_Issue(db.Questions_IssueKey(Textbox_QuestionKey_Move_Question_From.Text.Trim()));

        #endregion

        #endregion

        //Create a new QuestionKey for the new Issue
        var newQuestionKey = Textbox_IssueKey_Move_To.Text.Trim();
        newQuestionKey += MakeUnique6Digits();

        //if (db.Row_Optional(sql.Question(NewQuestionKey)) != null)
        if (Row_Question_Optional(newQuestionKey) != null)
          throw new ApplicationException("The new QuestionKey (" + newQuestionKey +
            ") already exist");

        //Update Questions with new QuestionKey and IssueKey
        var sqlText = "UPDATE Questions";
        sqlText += " SET QuestionKey = " + SqlLit(newQuestionKey);
        sqlText += " ,IssueKey = " + SqlLit(Textbox_IssueKey_Move_To.Text.Trim());
        sqlText += " WHERE QuestionKey = " +
          SqlLit(Textbox_QuestionKey_Move_Question_From.Text.Trim());
        G.ExecuteSql(sqlText);

        //Update all the Answers with the new QuestionKey
        sqlText = "UPDATE Answers";
        sqlText += " SET QuestionKey = " + SqlLit(newQuestionKey);
        sqlText += " ,IssueKey = " + SqlLit(Textbox_IssueKey_Move_To.Text.Trim());
        sqlText += " WHERE QuestionKey = " +
          SqlLit(Textbox_QuestionKey_Move_Question_From.Text.Trim());
        G.ExecuteSql(sqlText);

        LogPoliticianAnswers.UpdateIssueKeyByQuestionKey(
          Textbox_IssueKey_Move_To.Text.Trim(),
          Textbox_QuestionKey_Move_Question_From.Text.Trim()
        );
        LogPoliticianAnswers.UpdateQuestionKeyByQuestionKey(
          newQuestionKey, Textbox_QuestionKey_Move_Question_From.Text.Trim()
        );

        Msg.Text = Ok("Question: ("
          + Questions_Str(
            newQuestionKey
            , "Question"
          )
          + ") has been moved to Issue ("
          + Issues_Issue(Textbox_IssueKey_Move_To.Text.Trim()) + ")");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonMoveAnswers_Click(object sender, EventArgs e)
    {
      try
      {
        #region Checks

        Throw_Exception_TextBox_Script(Textbox_QuestionKey_Move_Answers_From);
        Throw_Exception_TextBox_Html(Textbox_QuestionKey_Move_Answers_From);

        Throw_Exception_TextBox_Script(Textbox_QuestionKey_Move_Answers_To);
        Throw_Exception_TextBox_Html(Textbox_QuestionKey_Move_Answers_To);

        //if (db.Row_Optional(sql.Question(Textbox_QuestionKey_Move_Answers_From.Text.Trim())) == null)
        if (Row_Question_Optional(Textbox_QuestionKey_Move_Answers_From.Text.Trim()) == null)
          throw new ApplicationException("The QuestionKey (" +
            Textbox_QuestionKey_Move_Answers_From.Text.Trim() + ") does not exist");

        //if (db.Row_Optional(sql.Question(Textbox_QuestionKey_Move_Answers_To.Text.Trim())) == null)
        if (Row_Question_Optional(Textbox_QuestionKey_Move_Answers_To.Text.Trim()) == null)
          throw new ApplicationException("The QuestionKey (" +
            Textbox_QuestionKey_Move_Answers_To.Text.Trim() + ") does not exist");

        #endregion

        #region Remove Issue.aspx and PoliticianIssue.aspx cached pages - Needs to be done first before any Question Rows are deleted

        //db.Cache_Remove_Issue_Pages(db.Questions_IssueKey(Textbox_QuestionKey_Move_Answers_From.Text.Trim()));
        //db.Cache_Remove_Issue_Pages(db.Questions_IssueKey(Textbox_QuestionKey_Move_Answers_To.Text.Trim()));

        //db.Cache_Remove_PoliticianIssue_Issue(db.Questions_IssueKey(Textbox_QuestionKey_Move_Answers_From.Text.Trim()));
        //db.Cache_Remove_PoliticianIssue_Issue(db.Questions_IssueKey(Textbox_QuestionKey_Move_Answers_To.Text.Trim()));

        #endregion

        #region inits

        #endregion inits

        #region Table of Answers to move

        var sql = string.Empty;
        sql += " SELECT ";
        sql += " Answers.PoliticianKey ";
        sql += ",Answers.QuestionKey ";
        sql += ",Answers.StateCode ";
        sql += ",Answers.IssueKey ";
        sql += ",Answers.Answer ";
        sql += ",Answers.Source ";
        sql += ",Answers.DateStamp ";
        sql += ",Answers.UserName ";
        sql += ",Answers.YouTubeUrl ";
        sql += ",Answers.YouTubeDate ";
        sql += " FROM Answers ";
        sql += " WHERE Answers.QuestionKey = "
          + SqlLit(Textbox_QuestionKey_Move_Answers_From.Text.Trim());
        sql += " ORDER BY PoliticianKey";
        var tableAnswersToMove = G.Table(sql);

        #endregion Table of Answers to move

        foreach (DataRow rowAnswerToMove in tableAnswersToMove.Rows)
        {
          #region Process each Answer being moved

          #region possible existing answer to question by politician

          sql = string.Empty;
          sql += " SELECT ";
          sql += " Answers.PoliticianKey ";
          sql += ",Answers.QuestionKey ";
          sql += ",Answers.StateCode ";
          sql += ",Answers.IssueKey ";
          sql += ",Answers.Answer ";
          sql += ",Answers.Source ";
          sql += ",Answers.DateStamp ";
          sql += ",Answers.UserName ";
          sql += ",Answers.YouTubeUrl ";
          sql += ",Answers.YouTubeDate ";
          sql += " FROM Answers";
          sql += " WHERE PoliticianKey = "
            + SqlLit(rowAnswerToMove["PoliticianKey"].ToString());
          sql += " AND QuestionKey = "
            + SqlLit(Textbox_QuestionKey_Move_Answers_To.Text.Trim());

          var rowAnswerByPoliticianToQuestion = Row_Optional(sql);

          #endregion existing answer to question by politician

          //Check if Politician already answered the question
          if (rowAnswerByPoliticianToQuestion == null)
          {
            #region No answer to the same question by the same politician

            Reasign_Answer_To_Question_Moving_To(
              rowAnswerToMove["PoliticianKey"].ToString()
            );

            #endregion No answer to the same question by the same politician
          }
          else
          {
            #region Politician answered both questions. So keep the most recent answer, delete the other

            var timeAnswerToMove = (DateTime) rowAnswerToMove["DateStamp"];
            var timeAnswerCurrentForPolitician =
              (DateTime) rowAnswerByPoliticianToQuestion["DateStamp"];

            if (timeAnswerToMove > timeAnswerCurrentForPolitician)
            {
              #region Answer being moved is more current

              //Delete current answer for politician
              //then reasign answer to new question for politician
              Delete_Answer(
                rowAnswerToMove["PoliticianKey"].ToString(),
                Textbox_QuestionKey_Move_Answers_To.Text.Trim()
              );

              Reasign_Answer_To_Question_Moving_To(
                rowAnswerToMove["PoliticianKey"].ToString()
              );

              #endregion Answer being moved is more current
            }
            else
            {
              #region Answer being moved is older

              //delete the answer being moved
              Delete_Answer(
                rowAnswerToMove["PoliticianKey"].ToString(),
                Textbox_QuestionKey_Move_Answers_From.Text.Trim()
              );

              #endregion Answer being moved is older
            }

            #endregion Politician answered both questions. So keep the most recent answer, delete the other
          }

          #endregion Process each Answer being moved
        }

        #region Msg

        Msg.Text = Ok("All Answers for Question ID ("
          + Textbox_QuestionKey_Move_Answers_From.Text.Trim()
          + " - "
          //+ QuestionBeingDeleted
          + Questions_Str(Textbox_QuestionKey_Move_Answers_From.Text.Trim(), "Question")
          + ") have been moved to Question ID ("
          + Textbox_QuestionKey_Move_Answers_To.Text.Trim()
          + " - "
          + Questions_Str(Textbox_QuestionKey_Move_Answers_To.Text.Trim(), "Question")
          + ")"
        );

        #endregion Msg

        #region Needs to be done after Msg - delete question of the answers being moved

        var sqldelete = string.Empty;
        sqldelete += "DELETE FROM Questions";
        sqldelete += " WHERE QuestionKey = " + SqlLit(
          Textbox_QuestionKey_Move_Answers_From.Text.Trim()
        );
        G.ExecuteSql(sqldelete);

        #endregion delete question of the answers being moved
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonMatchKeys_Click(object sender, EventArgs e)
    {
      try
      {
        #region Table of Answers to move

        var sql = string.Empty;
        sql += " SELECT ";
        sql += " Answers.PoliticianKey ";
        sql += ",Answers.QuestionKey ";
        sql += ",Answers.IssueKey ";
        sql += " FROM Answers ";
        var tableAnswers = G.Table(sql);

        var countChanges = 0;
        var countAnswersDeleted = 0;
        foreach (DataRow rowAnswer in tableAnswers.Rows)
        {
          if (Is_Valid_Question(rowAnswer["QuestionKey"].ToString()))
          {
            #region QuestionKey exists in Questions Table

            var correctIssueKey = Questions_Str(
              rowAnswer["QuestionKey"].ToString(),
              "IssueKey");
            if (correctIssueKey != rowAnswer["IssueKey"].ToString())
            {
              countChanges++;
              var sqlupdate = string.Empty;
              sqlupdate += "UPDATE Answers";
              sqlupdate += " SET IssueKey = " + SqlLit(correctIssueKey);
              sqlupdate += " WHERE PoliticianKey = " + SqlLit(rowAnswer["PoliticianKey"].ToString());
              sqlupdate += " AND QuestionKey = " + SqlLit(rowAnswer["QuestionKey"].ToString());
              G.ExecuteSql(sqlupdate);
            }

            #endregion QuestionKey exists in Questions Table
          }
          else
          {
            #region No Question row for QuestionKey

            countAnswersDeleted++;
            Delete_Answer(
              rowAnswer["PoliticianKey"].ToString(),
              rowAnswer["QuestionKey"].ToString()
            );

            #endregion region No Question row for QuestionKey
          }
        }

        #endregion Table of Answers to move

        Msg.Text = Ok(countChanges + " IssueKeys were changed. "
          + countAnswersDeleted + " Answers were deleted.");
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
        Page.Title = "Move Questions and Answers";

        if (string.IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks(
            "/admin/QuestionAnswers.aspx?issuelevel=C&state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        #region Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  HandleSecurityException();

        #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        #region ViewState Values

        #region note

        //Level A = StateCode LL - indicating all politicians
        //Level B = StateCode US - National Issues
        //Level C and higher = StateCode - State Issues

        #endregion note

        ViewState["IssueLevel"] = string.Empty;
        ViewState["StateCode"] = string.Empty;

        #region QueryStrings: ViewState["IssueLevel"]  ViewState["StateCode"]

        if (!string.IsNullOrEmpty(GetQueryString("IssueLevel")))
          ViewState["IssueLevel"] = GetQueryString("IssueLevel");
        if (!string.IsNullOrEmpty(QueryState))
          ViewState["StateCode"] = QueryState;
        //if(ViewState["IssueLevel"].ToString() == "C")
        //  //At State level Group and StateCode are the same
        //  Session["UserStateCode"] = db.QueryString("State");

        #endregion

        #endregion ViewState Values

        #region Additional Security Checks

        if ((ViewState["IssueLevel"].ToString() == string.Empty)
          || (ViewState["StateCode"].ToString() == string.Empty))
          HandleFatalError("The IssueLevel and/or StateCode is missing");

        #endregion Security Checks

        try
        {
          #region PageTitle commented out

          //PageTitle.Text = "Move Questions and Answers";
          //switch (ViewState["IssueLevel"].ToString())
          //{
          //  case "A":
          //    PageTitle.Text += "<br>for ALL Candidates";
          //    break;
          //  case "B":
          //    PageTitle.Text += "<br>for NATIONAL Office Candidates";
          //    break;
          //  case "C":
          //    PageTitle.Text += "<br>for "
          //      + db.States_Str(ViewState["StateCode"].ToString(), "State").ToUpper()
          //      + " Office Candidates";
          //    break;
          //  case "D":
          //    PageTitle.Text += "<br>for "
          //      + db.States_Str(ViewState["StateCode"].ToString(), "State").ToUpper()
          //      + " Office Candidates";
          //    break;
          //}

          #endregion

          PageTitle.Text = PageTitle_QuestionAnswers(ViewState["StateCode"].ToString(),
            ViewState["IssueLevel"].ToString());

          //HyperLinkReport.NavigateUrl = "/Admin/IssuesReport.aspx"
          //+ "?IssueLevel=" + ViewState["IssueLevel"].ToString()
          //+ "&StateCode=" + ViewState["StateCode"].ToString();
          HyperLinkReport.NavigateUrl = Url_Admin_IssuesReport(
            ViewState["IssueLevel"].ToString()
            , ViewState["StateCode"].ToString());

          //ShowReport(ViewState["IssueLevel"].ToString(), ViewState["StateCode"].ToString());
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
      }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      try
      {
        if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
          Msg.Text = Fail(Session["ErrNavBarAdmin"].ToString());
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch /*(Exception ex)*/
      {
      }
    }
  }
}