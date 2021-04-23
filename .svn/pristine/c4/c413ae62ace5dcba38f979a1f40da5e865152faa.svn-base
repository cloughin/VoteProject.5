using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Politician
{
  public partial class Answer : VotePage
  {
    #region Dead code

    protected override void OnPreInit(EventArgs e)
    {
      //if (string.IsNullOrWhiteSpace(Request.QueryString["noredirect"]))
      {
        string query = Request.QueryString.ToString();
        string url = "/politician/updateissues.aspx";
        if (!string.IsNullOrWhiteSpace(query))
          url += "?" + query;
        Response.Redirect(url);
      }
      base.OnPreInit(e);
    }

    //protected void Check_Source_And_Date()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxSource);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxDate);

    //  if (TextboxSource.Text.Trim() == string.Empty)
    //    throw new ApplicationException("The Data Source textbox is empty.");

    //  if (TextboxSource.Text.Trim().Length > 85)
    //    throw new ApplicationException("Your Source was "
    //      + TextboxSource.Text.Trim().Length.ToString()
    //      + " characters. Your response can not exceed 85 characters.");

    //  if (
    //    (TextBoxDate.Text.Trim() != string.Empty)
    //    && (!db.Is_Valid_Date(TextBoxDate.Text.Trim()))
    //    )
    //    throw new ApplicationException("The Date textbox is not recognized as valid.");
    //}
    //protected void Set_Session_Source_And_Date()
    //{
    //  Session["Source"] = db.Str_Remove_BRs(
    //    db.Str_Remove_Http(TextboxSource.Text.Trim()));
    //  if (TextBoxDate.Text.Trim() == string.Empty)
    //    //Session["Date"] = DateTime.Today.ToString("MM/dd/yyyy");
    //    Session["Date"] = Db.DbToday;
    //  else
    //    //Session["Date"] = Convert.ToDateTime(
    //    //  TextBoxDate.Text.Trim()).ToString("MM/dd/yyyy");
    //    Session["Date"] = Convert.ToDateTime(
    //      TextBoxDate.Text.Trim()).ToString("yyyy-MM-dd");
    //}

    ////protected void Log_Politician_Answer()
    ////{
    ////  //string test = DateTime.Now.ToString();
    ////  string InsertLogSQL = "INSERT INTO LogPoliticianAnswers (";
    ////  InsertLogSQL += "DateStamp";
    ////  InsertLogSQL += ",Source";
    ////  InsertLogSQL += ",PoliticianKey";
    ////  InsertLogSQL += ",UserSecurity";
    ////  InsertLogSQL += ",UserName";
    ////  InsertLogSQL += ",IssueKey";
    ////  InsertLogSQL += ",QuestionKey";
    ////  InsertLogSQL += ",Question";
    ////  InsertLogSQL += ",AnswerFrom";
    ////  InsertLogSQL += ",AnswerTo";
    ////  InsertLogSQL += ")";
    ////  InsertLogSQL += " VALUES(";
    ////  InsertLogSQL += db.SQLLit(Db.DbNow);

    ////  if (db.User() == db.User_.Politician)
    ////    InsertLogSQL += "," + db.SQLLit(db.Politician(
    ////         ViewState["PoliticianKey"].ToString(), db.Politician_Column.LName));
    ////  else
    ////    InsertLogSQL += "," + db.SQLLit(db.Str_Remove_BRs(db.Str_Remove_Http(
    ////          TextboxSource.Text.Trim())));

    ////  InsertLogSQL += "," + db.SQLLit(ViewState["PoliticianKey"].ToString());
    ////  InsertLogSQL += "," + db.SQLLit(db.User_Security());
    ////  InsertLogSQL += "," + db.SQLLit(db.User_Name());
    ////  InsertLogSQL += "," + db.SQLLit(ViewState["IssueKey"].ToString());
    ////  InsertLogSQL += "," + db.SQLLit(ViewState["QuestionKey"].ToString());
    ////  InsertLogSQL += "," + db.SQLLit(db.Questions_Str(
    ////            ViewState["QuestionKey"].ToString()
    ////            , "Question")
    ////            );
    ////  InsertLogSQL += "," + db.SQLLit(db.Answer_Issue_Question(
    ////          ViewState["PoliticianKey"].ToString().Trim()
    ////          , ViewState["QuestionKey"].ToString()
    ////          , false, false, false));//exclude last naeme,source, datestamp
    ////  InsertLogSQL += "," + db.SQLLit(TextBoxNewResponse.Text.Trim()); //new answer
    ////  InsertLogSQL += ")";
    ////  db.ExecuteSQL(InsertLogSQL);
    ////}

    //protected void Log_Politician_Answer()
    //{
    //  string source;
    //  if (db.User() == db.User_.Politician)
    //    source = db.Politician(ViewState["PoliticianKey"].ToString(),
    //      db.Politician_Column.LName);
    //  else
    //    source = db.Str_Remove_BRs(db.Str_Remove_Http(TextboxSource.Text.Trim()));
    //  string politicianKey = ViewState["PoliticianKey"].ToString().Trim();
    //  string questionKey = ViewState["QuestionKey"].ToString();
    //  string issueKey = ViewState["IssueKey"].ToString();

    //  DB.VoteLog.LogPoliticianAnswers.Insert(
    //    source,
    //    DateTime.Now,
    //    politicianKey,
    //    db.User_Security(),
    //    db.User_Name(),
    //    issueKey,
    //    questionKey,
    //    db.Questions_Str(questionKey, "Question"),
    //    db.Answer_Issue_Question(politicianKey, questionKey, false, false, false),
    //    TextBoxNewResponse.Text.Trim(),
    //    null,
    //    null);
    //}

    //protected void Log_Politician_YouTube()
    //{
    //  string politicianKey = ViewState["PoliticianKey"].ToString().Trim();
    //  string questionKey = ViewState["QuestionKey"].ToString();
    //  string issueKey = ViewState["IssueKey"].ToString();

    //  DB.VoteLog.LogPoliticianAnswers.Insert(
    //    string.Empty,
    //    DateTime.Now,
    //    politicianKey,
    //    db.User_Security(),
    //    db.User_Name(),
    //    issueKey,
    //    questionKey,
    //    db.Questions_Str(questionKey, "Question"),
    //    string.Empty,
    //    string.Empty,
    //    Answers.GetYouTubeUrlByPoliticianKeyQuestionKey(politicianKey, questionKey),
    //    YouTubeUrlTextBox.Text.Trim());
    //}

    //protected void ButtonDelete_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region
    //    //do nothing if there currently is no answer in Answers Table
    //    //otherwise log to indicate answer was changed to 'No Response'
    //    //and delete the answer
    //    if (db.Answer_Issue_Question(
    //      ViewState["PoliticianKey"].ToString()
    //      , ViewState["QuestionKey"].ToString()
    //      , false
    //      , false
    //      , false)
    //      != string.Empty)//exclude last name, source and datestamp
    //    {
    //      #region replaced Log PoliticianAnswers
    //      ////need to do first to get AnswerFrom

    //      //string InsertLogSQL = "INSERT INTO LogPoliticianAnswers ("
    //      //  + "DateStamp"
    //      //  + ",PoliticianKey"
    //      //  + ",UserSecurity"
    //      //  + ",UserName"
    //      //  + ",IssueKey"
    //      //  + ",QuestionKey"
    //      //  + ",Question"
    //      //  + ",AnswerFrom"
    //      //  + ",AnswerTo"
    //      //  + ")"
    //      //  + " VALUES("
    //      //  + db.SQLLit(Db.DbNow)
    //      //  + "," + db.SQLLit(ViewState["PoliticianKey"].ToString())
    //      //  + "," + db.SQLLit(db.User_Security())
    //      //  + "," + db.SQLLit(db.User_Name())
    //      //  + "," + db.SQLLit(ViewState["IssueKey"].ToString())
    //      //  + "," + db.SQLLit(ViewState["QuestionKey"].ToString())
    //      //  + "," + db.SQLLit(db.Questions_Str(
    //      //      ViewState["QuestionKey"].ToString()
    //      //      , "Question")
    //      //      )
    //      //  + "," + db.SQLLit(db.Answer_Issue_Question(ViewState["PoliticianKey"].ToString().Trim(), ViewState["QuestionKey"].ToString(), false, false, false))//Old answer from - exclude source and datestamp
    //      //  + ",'Deleted Response on File'"//new answer
    //      //  + ")";
    //      //db.ExecuteSQL(InsertLogSQL);
    //      #endregion LogPoliticianAnswers

    //      //need to do first to get AnswerFrom
    //      Log_Politician_Answer();

    //      #region Delete Answer in Answer Table
    //      string DeleteSQL = "DELETE FROM Answers";
    //      DeleteSQL += " WHERE PoliticianKey = " + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //      DeleteSQL += " AND QuestionKey = " + db.SQLLit(ViewState["QuestionKey"].ToString());
    //      db.ExecuteSQL(DeleteSQL);

    //      //db.Politicians_Last_Updated_Inrement_Count(db.ModifyPoliticianKeyIfUSPresident(ViewState["PoliticianKey"].ToString()));
    //      db.Politician_Update_Increment_Count_Data_Updated(ViewState["PoliticianKey"].ToString());

    //      #endregion Delete Answer in Answer Table

    //    }
    //    #endregion
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //  Response.Redirect(db.Url_Politician_IssueQuestions(
    //    ViewState["IssueKey"].ToString()
    //    //, ViewState["PoliticianKey"].ToString()
    //    )
    //    );
    //}

    //protected void ButtonClear_Click1(object sender, EventArgs e)
    //{
    //  TextBoxNewResponse.Text = string.Empty;
    //  Msg.Text = db.Ok("The textbox for your answer has been cleared"
    //  + " and nothing has been recorded.");
    //}

    //protected void ButtonReload_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxNewResponse.Text = db.Answer_Issue_Question(
    //      ViewState["PoliticianKey"].ToString()
    //      , ViewState["QuestionKey"].ToString()
    //      );

    //    if (
    //      (db.User() == db.User_.Master)
    //      || (db.User() == db.User_.Admin_Intern)
    //      )
    //    {
    //      #region source
    //      if (db.Answer_Source(
    //        ViewState["PoliticianKey"].ToString()
    //        , ViewState["QuestionKey"].ToString()
    //        ) != string.Empty)
    //      {
    //        TextboxSource.Text = db.Answer_Source(
    //          ViewState["PoliticianKey"].ToString()
    //          , ViewState["QuestionKey"].ToString()
    //          );
    //      }
    //      else
    //      {
    //        #region Last answer source
    //        if (!string.IsNullOrEmpty(Session["Source"].ToString()))
    //          TextboxSource.Text = Session["Source"].ToString();
    //        #endregion Last answer source
    //      }
    //      #endregion source

    //      #region Date
    //      //1/1/1900
    //      if (db.Answer_Date(
    //        ViewState["PoliticianKey"].ToString()
    //        , ViewState["QuestionKey"].ToString()
    //        ) != DateTime.MinValue)
    //      {
    //        TextBoxDate.Text = db.Answer_Date(
    //          ViewState["PoliticianKey"].ToString()
    //          , ViewState["QuestionKey"].ToString()
    //          ).ToString("MM/dd/yyyy");
    //      }
    //      else
    //      {
    //        #region Last answer date
    //        if (!string.IsNullOrEmpty(Session["Date"].ToString()))
    //          TextBoxDate.Text = Convert.ToDateTime(
    //            Session["Date"]).ToString("MM/dd/yyyy");
    //        #endregion Last answer date
    //      }
    //      #endregion Date
    //    }

    //    Msg.Text = db.Ok("The textbox contains your response on file,"
    //    + " but nothing has been recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //}

    //protected void ButtonReturn_Click(object sender, EventArgs e)
    //{
    //  //Response.Redirect("/Politician/IssueQuestions.aspx");
    //  Response.Redirect(db.Url_Politician_IssueQuestions(
    //    ViewState["IssueKey"].ToString())
    //    );
    //}

    //protected void ButtonSubmit_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string questionKey = ViewState["QuestionKey"].ToString();
    //    string issueKey = ViewState["IssueKey"].ToString();
    //    #region All users checks
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNewResponse);
    //    if (TextBoxNewResponse.Text.Trim() == string.Empty)
    //      throw new ApplicationException("The Response field is empty. Click the DELETE Button"
    //        + " in the lower right corner if you want to delete this response.");
    //    if (TextBoxNewResponse.Text.Trim().Length > db.Max_Answer_Length_2000)
    //      throw new ApplicationException("Your Response was "
    //        + TextBoxNewResponse.Text.Trim().Length.ToString()
    //        + " characters. Your response can not exceed "
    //        + db.Max_Answer_Length_2000.ToString() + " characters.");
    //    #endregion All users checks

    //    if (
    //      (db.User() == db.User_.Master)
    //      || (db.User() == db.User_.Admin_Intern)
    //      )
    //    {
    //      Check_Source_And_Date();

    //      Set_Session_Source_And_Date();
    //    }

    //    #region replaced Log From/To Answers BEFORE Updating New and Old answers
    //    //string InsertLogSQL = "INSERT INTO LogPoliticianAnswers (";
    //    //InsertLogSQL += "DateStamp";
    //    //InsertLogSQL += ",Source";
    //    //InsertLogSQL += ",PoliticianKey";
    //    //InsertLogSQL += ",UserSecurity";
    //    //InsertLogSQL += ",UserName";
    //    //InsertLogSQL += ",IssueKey";
    //    //InsertLogSQL += ",QuestionKey";
    //    //InsertLogSQL += ",Question";
    //    //InsertLogSQL += ",AnswerFrom";
    //    //InsertLogSQL += ",AnswerTo";
    //    //InsertLogSQL += ")";
    //    //InsertLogSQL += " VALUES(";
    //    //InsertLogSQL += db.SQLLit(Db.DbNow);

    //    //if (db.User() == db.User_.Politician)
    //    //  InsertLogSQL += "," + db.SQLLit(db.Politician(
    //    //       ViewState["PoliticianKey"].ToString(),db.Politician_Column.LName));
    //    //else
    //    //  InsertLogSQL += "," + db.SQLLit(db.Str_Remove_BRs(db.Str_Remove_Http(
    //    //        TextboxSource.Text.Trim())));

    //    //InsertLogSQL += "," + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //    //InsertLogSQL += "," + db.SQLLit(db.User_Name());
    //    //InsertLogSQL += "," + db.SQLLit(ViewState["IssueKey"].ToString());
    //    //InsertLogSQL += "," + db.SQLLit(ViewState["QuestionKey"].ToString());
    //    //InsertLogSQL += "," + db.SQLLit(db.Questions_Str(
    //    //          ViewState["QuestionKey"].ToString()
    //    //          , "Question")
    //    //          );
    //    //InsertLogSQL += "," + db.SQLLit(db.Answer_Issue_Question(
    //    //        ViewState["PoliticianKey"].ToString().Trim()
    //    //        , ViewState["QuestionKey"].ToString()
    //    //        , false, false, false));//exclude last naeme,source, datestamp
    //    //InsertLogSQL += "," + db.SQLLit(TextBoxNewResponse.Text.Trim()); //new answer
    //    //InsertLogSQL += ")";
    //    //db.ExecuteSQL(InsertLogSQL);
    //    #endregion

    //    //Log From/To Answers BEFORE Updating New and Old answers
    //    Log_Politician_Answer();

    //    #region Add or Update Answers Table
    //    if (!db.Is_Valid_Answer(politicianKey, questionKey))
    //    {
    //      string InsertAnswerSQL = "INSERT INTO Answers (";
    //      InsertAnswerSQL += "PoliticianKey";
    //      InsertAnswerSQL += ",QuestionKey";
    //      InsertAnswerSQL += ",StateCode";
    //      InsertAnswerSQL += ",IssueKey";
    //      InsertAnswerSQL += ",Answer";
    //      InsertAnswerSQL += ",Source";
    //      InsertAnswerSQL += ",DateStamp";
    //      InsertAnswerSQL += ",UserName";
    //      InsertAnswerSQL += ")";
    //      InsertAnswerSQL += " VALUES(";
    //      InsertAnswerSQL += db.SQLLit(politicianKey);
    //      InsertAnswerSQL += "," + db.SQLLit(questionKey);
    //      InsertAnswerSQL += "," + db.SQLLit(Politicians.GetStateCode(politicianKey, string.Empty));
    //      InsertAnswerSQL += "," + db.SQLLit(issueKey);
    //      InsertAnswerSQL += "," + db.SQLLit(TextBoxNewResponse.Text.Trim());

    //      if (db.User() == db.User_.Politician)
    //        InsertAnswerSQL += "," + db.SQLLit(db.Politician(
    //             politicianKey, db.Politician_Column.LName));
    //      else
    //        InsertAnswerSQL += "," + db.SQLLit(db.Str_Remove_BRs(db.Str_Remove_Http(
    //              TextboxSource.Text.Trim())));

    //      //InsertAnswerSQL += "," + db.SQLLit(Convert.ToString(DateTime.Now));
    //      InsertAnswerSQL += "," + db.SQLLit(Db.DbNow);
    //      InsertAnswerSQL += "," + db.SQLLit(db.User_Name());
    //      InsertAnswerSQL += ")";
    //      db.ExecuteSQL(InsertAnswerSQL);
    //    }
    //    else//Previous response - UPDATE Answer
    //    {
    //      string UpdateSQL = "UPDATE Answers SET";
    //      UpdateSQL += " Answer = " + db.SQLLit(TextBoxNewResponse.Text.Trim());

    //      //UpdateSQL += " ,Source = " + db.SQLLit(Source);
    //      if (db.User() == db.User_.Politician)
    //        UpdateSQL += " ,Source = " + db.SQLLit(db.Politician(
    //             politicianKey, db.Politician_Column.LName));
    //      else
    //        UpdateSQL += " ,Source = " + db.SQLLit(db.Str_Remove_BRs(db.Str_Remove_Http(
    //              TextboxSource.Text.Trim())));

    //      //UpdateSQL += " ,DateStamp = " + db.SQLLit(Convert.ToString(DateTime.Now));
    //      UpdateSQL += " ,DateStamp = " + db.SQLLit(Db.DbNow);
    //      UpdateSQL += " ,UserName = " + db.SQLLit(db.User_Name());
    //      UpdateSQL += " WHERE PoliticianKey = " + db.SQLLit(politicianKey);
    //      UpdateSQL += " AND QuestionKey = " + db.SQLLit(questionKey);
    //      db.ExecuteSQL(UpdateSQL);
    //    }
    //    #endregion

    //    db.Politician_Update_Increment_Count_Data_Updated(politicianKey);

    //    #region Remove Cached Pages
    //    //if this is the FIRST answer for this election, office, issue 
    //    //then all pages for the election / office needs to be removed
    //    //because the navbar needs to change to add another issue.
    //    //Otherwise remove only the election, office, issue page.

    //    //string OfficeKey = Politicians.GetOfficeKey(ViewState["PoliticianKey"].ToString(), string.Empty);
    //    string officeKey = PageCache.GetTemporary().Politicians.GetOfficeKey(politicianKey);
    //    //********* BUG FIX db.ElectionKey_State was returning a StateCode not an ElectionKey **********
    //    //string ElectionKey = db.ElectionKey_State(
    //    //  db.Politicians_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //    , "StateCode")
    //    //    );
    //    string electionKey = db.Politician_Current_ElectionKey(politicianKey);

    //    #region sql
    //    string SQL = string.Empty;
    //    SQL += " SELECT ";
    //    SQL += " ElectionsOffices.OfficeKey";
    //    SQL += ",ElectionsPoliticians.PoliticianKey";
    //    SQL += ",Questions.Question";
    //    SQL += ",Answers.Answer";
    //    SQL += " FROM ElectionsOffices,ElectionsPoliticians,Issues,Questions,Answers ";
    //    SQL += " WHERE ElectionsPoliticians.ElectionKey = "
    //      + db.SQLLit(electionKey);
    //    SQL += " AND ElectionsOffices.ElectionKey = "
    //      + db.SQLLit(electionKey);
    //    SQL += " AND ElectionsOffices.OfficeKey = "
    //      + db.SQLLit(officeKey);
    //    SQL += " AND ElectionsOffices.OfficeKey = ElectionsPoliticians.OfficeKey";
    //    SQL += " AND Issues.IssueKey = "
    //      + db.SQLLit(issueKey);
    //    SQL += " AND Questions.IssueKey = Issues.IssueKey";
    //    SQL += " AND Answers.QuestionKey = Questions.QuestionKey";
    //    SQL += " AND Answers.PoliticianKey = ElectionsPoliticians.PoliticianKey";
    //    SQL += " AND Questions.IsQuestionOmit = 0";
    //    SQL += " AND Issues.IsIssueOmit = 0";
    //    #endregion sql

    //    int Answers = db.Rows(SQL);

    //    #region Remove Issue.aspx cached pages
    //    if (officeKey == "USPresident")
    //    {
    //      //USPresident in 51 elections
    //      if (Answers == 1)
    //        db.Cache_Remove_Issue_Pages_Office(officeKey);
    //      else
    //        db.Cache_Remove_Issue_Pages_Politician(
    //          officeKey
    //          , issueKey
    //          );
    //    }
    //    else
    //    {
    //      if (Answers == 1)
    //        db.Cache_Remove_Issue_Pages_Office_Contest(
    //          electionKey
    //          , officeKey
    //          );
    //      else
    //        db.Cache_Remove_Issue_Pages_Politician_Election_Contest(
    //          electionKey
    //          , officeKey
    //          , issueKey
    //          );
    //    }
    //    #endregion Remove Issue.aspx cached pages

    //    #region Remove PoliticianIssue.aspx cached Pages
    //    if (Answers == 1)
    //      db.Cache_Remove_PoliticianIssue_Politician(politicianKey);
    //    else
    //      db.Cache_Remove_PoliticianIssue_Politician_Issue(politicianKey, issueKey);
    //    #endregion Remove PoliticianIssue.aspx cached Pages

    //    #endregion Remove Cached Pages

    //    Response.Redirect(db.Url_Politician_IssueQuestions(
    //      issueKey)
    //      );
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //}

    //protected void Button_Change_Source_Date_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_Source_And_Date();

    //    Set_Session_Source_And_Date();

    //    #region Update Source and Date
    //    string UpdateSQL = "UPDATE Answers SET";
    //    UpdateSQL += " Source = " + db.SQLLit(db.Str_Remove_BRs(db.Str_Remove_Http(
    //          TextboxSource.Text.Trim())));

    //    UpdateSQL += " ,DateStamp = " + db.SQLLit(Db.DbNow);
    //    UpdateSQL += " WHERE PoliticianKey = " + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //    UpdateSQL += " AND QuestionKey = " + db.SQLLit(ViewState["QuestionKey"].ToString());
    //    db.ExecuteSQL(UpdateSQL);
    //    #endregion Update Source and Date
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //}

    //private void ReportException(Exception ex)
    //{
    //  Msg.Visible = true;
    //  Msg.Text = db.Fail(ex.Message);

    //  db.Log_Error_Admin(ex);
    //}

    //protected void TextBoxDate_TextChanged(object sender, EventArgs e)
    //{

    //}

    //protected void TextBoxNewResponse_TextChanged(object sender, EventArgs e)
    //{

    //}

    //protected void SubmitYouTubeUrlButton_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Uri youTubeUri = GetYouTubeUri();
    //    DateTime youTubeDate = GetYouTubeDate();
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string questionKey = ViewState["QuestionKey"].ToString();
    //    YouTubeUrlTextBox.Text = youTubeUri.ToString();
    //    YouTubeDateTextBox.Text = youTubeDate.ToString("d");
    //    Log_Politician_YouTube();
    //    if (Answers.PoliticianKeyQuestionKeyExists(politicianKey, questionKey))
    //    {
    //      Answers.UpdateYouTubeUrlByPoliticianKeyQuestionKey(youTubeUri.ToString(), 
    //        politicianKey, questionKey);
    //      Answers.UpdateYouTubeDateByPoliticianKeyQuestionKey(youTubeDate, 
    //        politicianKey, questionKey);
    //    }
    //    else
    //    {
    //      string stateCode = Politicians.GetStateCode(politicianKey, string.Empty);
    //      string issueKey = ViewState["IssueKey"].ToString();

    //      Answers.Insert(
    //        politicianKey,
    //        questionKey,
    //        stateCode,
    //        issueKey,
    //        string.Empty,
    //        string.Empty,
    //        VoteDb.DateTimeMin,
    //        db.User_Name(),
    //        youTubeUri.ToString(),
    //        youTubeDate);
    //    }
    //    Msg.Text = db.Ok("The YouTube url has been updated");
    //    YouTubeUrlTextBox.Text = youTubeUri.ToString();
    //    YouTubeDateTextBox.Text = youTubeDate.ToString("d");
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //}

    //protected void RemoveYouTubeUrlButton_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string questionKey = ViewState["QuestionKey"].ToString();
    //    YouTubeUrlTextBox.Text = string.Empty;
    //    YouTubeDateTextBox.Text = string.Empty;
    //    Log_Politician_YouTube();
    //    if (Answers.PoliticianKeyQuestionKeyExists(politicianKey, questionKey))
    //    {
    //      Answers.UpdateYouTubeUrlByPoliticianKeyQuestionKey(null, politicianKey, 
    //        questionKey);
    //      Answers.UpdateYouTubeDateByPoliticianKeyQuestionKey(VoteDb.DateTimeMin, 
    //        politicianKey, questionKey);
    //    }
    //    Msg.Text = db.Ok("The YouTube url has been removed");
    //  }
    //  catch (Exception ex)
    //  {
    //    ReportException(ex);
    //  }
    //}

    //private Uri GetYouTubeUri()
    //{
    //  string url = YouTubeUrlTextBox.Text.Trim();
    //  if (string.IsNullOrWhiteSpace(url))
    //    throw new VoteException("The YouTube url is missing");
    //  Uri uri = url.ToUri();
    //  if (uri == null)
    //    throw new VoteException("The YouTube url is not a valid url");
    //  if (uri.Host != "www.youtube.com" && uri.Host != "youtube.com")
    //    throw new VoteException("The YouTube url does not reference youtube.com");
    //  return uri;
    //}

    //private DateTime GetYouTubeDate()
    //{
    //  string dateText = YouTubeDateTextBox.Text.Trim();
    //  if (string.IsNullOrWhiteSpace(dateText))
    //    return DateTime.Now.Date;
    //  try
    //  {
    //    return Convert.ToDateTime(dateText).Date;
    //  }
    //  catch 
    //  {
    //    throw new VoteException("The YouTube date is invalid");
    //  }
    //}

    //protected void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region ViewState["PoliticianKey"] ViewState["QuestionKey"]
    //    ViewState["PoliticianKey"] = db.PoliticianKey_ViewState();
    //    string politicianKey = ViewState["PoliticianKey"].ToString();

    //    ViewState["IssueKey"] = string.Empty;
    //    if (!string.IsNullOrEmpty(db.QueryString("Issue")))
    //      ViewState["IssueKey"] = db.QueryString("Issue").Trim();

    //    ViewState["QuestionKey"] = string.Empty;
    //    if (!string.IsNullOrEmpty(db.QueryString("Question")))
    //      ViewState["QuestionKey"] = db.QueryString("Question").Trim();
    //    #endregion ViewState["PoliticianKey"] ViewState["QuestionKey"]

    //    #region Redirect if Bad Entry
    //    if (
    //      (ViewState["PoliticianKey"].ToString() == string.Empty)
    //      || (ViewState["IssueKey"].ToString() == string.Empty)
    //      || (ViewState["QuestionKey"].ToString() == string.Empty)
    //      )
    //      db.HandleFatalError("The PoliticianKey and/or IssueKey and/or QuestionKey is missing");
    //    #endregion

    //    try
    //    {
    //      ViewState["QuestionKey"] = string.Empty;
    //      #region ViewState["QuestionKey"]
    //      if (!string.IsNullOrEmpty(db.QueryString("Question")))
    //        ViewState["QuestionKey"] = db.QueryString("Question").Trim();
    //      string questionKey = ViewState["QuestionKey"].ToString();
    //      #endregion

    //      //SiteCss.Attributes["HREF"] = db.PathStyleSheetCustomGet();// Set Custom Style Sheet if it exists at: /css/DomainDesignCode.css

    //      //LabelTitle.Text = db.Name_Office_Election_Politician(ViewState["PoliticianKey"].ToString());
    //      PoliticianName.Text = db.GetPoliticianName(ViewState["PoliticianKey"].ToString());
    //      PoliticianOffice.Text = db.Politician_Current_Office_And_Status(ViewState["PoliticianKey"].ToString());
    //      PoliticianElection.Text = db.Politician_Current_Election(ViewState["PoliticianKey"].ToString());

    //      db.CheckQuestionKey(questionKey);

    //      // If the IssueKey is not set, get it from the QuestionKey
    //      if (Session["IssueKey"] == null || 
    //        string.IsNullOrWhiteSpace(Session["IssueKey"].ToString()))
    //        Session["IssueKey"] =
    //          Questions.GetIssueKeyByQuestionKey(questionKey);
 
    //      db.CheckIssueKey(ViewState["IssueKey"].ToString());

    //      //DataRow PoliticianRow = db.PoliticiansRow(ViewState["PoliticianKey"].ToString().Trim());
    //      //if (!IsPostBack) //first time
    //      //{
    //      #region Data Source
    //      //for Pres Primary scrappers this is temporarily hidden
    //      TableDataSource.Visible = false;
    //      if (
    //        (db.User() == db.User_.Master)
    //        || (db.User() == db.User_.Admin_Intern)
    //        )
    //      {
    //        TableDataSource.Visible = true;
    //        if (!string.IsNullOrEmpty(Session["Source"].ToString()))
    //          //TextboxSource.Text = db.Html_From_Db_For_Page(Session["Source"].ToString());
    //          TextboxSource.Text = Session["Source"].ToString();
    //        if (!string.IsNullOrEmpty(Session["Date"].ToString()))
    //          //TextBoxDate.Text = db.Html_From_Db_For_Page(Session["Date"].ToString());
    //          TextBoxDate.Text = Convert.ToDateTime(Session["Date"]).ToString("MM/dd/yyyy");
    //      }
    //      else
    //      {
    //        TableDataSource.Visible = false;
    //      }
    //      #endregion

    //      ViewState["NewAnswer"] = string.Empty;
    //      string SQL = string.Empty;

    //      #region Question
    //      //DataRow QuestionRow = db.Row(sql.Question(ViewState["QuestionKey"].ToString()));
    //      //Question.Text = db.Html_From_Db_For_Page(QuestionRow["Question"].ToString().Trim());
    //      Question.Text = db.Html_From_Db_For_Page(
    //        db.Questions_Question(ViewState["QuestionKey"].ToString()));
    //      //int AnswerLines = Convert.ToUInt16(QuestionRow["AnswerLines"]);
    //      #endregion

    //      #region YouTube ur
    //      string youTubeUrl = Answers.GetYouTubeUrlByPoliticianKeyQuestionKey(
    //        politicianKey, questionKey);
    //      if (youTubeUrl == null) youTubeUrl = string.Empty;
    //      YouTubeUrlTextBox.Text = youTubeUrl;
    //      DateTime? youTubeDate = Answers.GetYouTubeDateByPoliticianKeyQuestionKey(
    //        politicianKey, questionKey);
    //      if (youTubeDate == null || youTubeDate.Value == VoteDb.DateTimeMin)
    //        YouTubeDateTextBox.Text = string.Empty;
    //      else
    //        YouTubeDateTextBox.Text = youTubeDate.Value.ToString("d");
    //      #endregion

    //      #region YouTube Upload
    //      if (SecurePage.IsMasterUser)
    //      {
    //        TableYouTubeUpload.Visible = true;
    //        HyperLinkYouTubeUpload.NavigateUrl = string.Format(
    //          "/Politician/YouTubeUpload.aspx?Id={0}&QuestionKey={1}",
    //          politicianKey, questionKey);

    //        trYouTubeCaption.Visible = true;
    //        trYouTubeInstruction.Visible = true;
    //        trYouTubeTextBox.Visible = true;
    //      }
    //      else
    //      {
    //        TableYouTubeUpload.Visible = false;

    //        trYouTubeCaption.Visible = false;
    //        trYouTubeInstruction.Visible = false;
    //        trYouTubeTextBox.Visible = false;
    //      }
    //      #endregion YouTube Upload

    //      #region On File: & Response:
    //      //string Answer = db.Answer_Issue_Question(
    //      //  ViewState["PoliticianKey"].ToString()
    //      //  , ViewState["QuestionKey"].ToString(), false, false, false
    //      //  );//exclude source and datestamp
    //      //if (Answer != string.Empty)
    //      //  OnFile.Text = db.Html_From_Db_For_Page(Answer);
    //      //else
    //      //  OnFile.Text = db.No_Response;

    //      //TextBoxNewResponse.Text = string.Empty;
    //      TextBoxNewResponse.Text = db.Answer_Issue_Question(
    //        ViewState["PoliticianKey"].ToString()
    //        , ViewState["QuestionKey"].ToString(), false, false, false
    //        );//exclude name, source and datestamp
    //      #endregion

    //      Msg.Text = db.Msg("Use this form to provide your position and views regarding the above.");
    //      //}
    //    }
    //    catch (Exception ex)
    //    {
    //      //Msg.Visible = true;
    //      //Msg.Text = db.Fail(ex.Message);
    //      //db.Log_Error_User_Then_Redirect_301(ex, db.Url_Politician_Default(ViewState["PoliticianKey"].ToString()));
    //      //db.Log_Error_User_Unhandled_Temp_Redirect_302(ex, "Page_Load", db.Url_Politician_Default(ViewState["PoliticianKey"].ToString()));
    //      string redirectUrl = UrlManager.SiteUri.ToString();
    //      db.Log_Temp_Redirect_302(ex, redirectUrl);

    //      //HttpContext.Current.Response.Status = "302 Moved Temporarily";
    //      //HttpContext.Current.Response.AddHeader("Location", redirectUrl);
    //      //HttpContext.Current.Response.End();
    //      Response.Redirect(redirectUrl);
    //    }
    //  }
    //}

    #endregion Dead code


  }
}
