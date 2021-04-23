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

namespace Vote.AnswersEdit
{
  public partial class AnswersEdit : VotePage
  {
  //  protected void Page_Load(object sender, EventArgs e)
  //  {
  //    if (!IsPostBack)
  //    {
  //      if (!SecurePage.IsMasterUser)
  //        SecurePage.HandleSecurityException();

  //      try
  //      {
  //        #region PoliticianKey, QuestionKey
  //        ViewState["PoliticianKey"] = string.Empty;
  //        if (!string.IsNullOrEmpty(QueryId))
  //          ViewState["PoliticianKey"] = QueryId;

  //        ViewState["QuestionKey"] = string.Empty;
  //        if (!string.IsNullOrEmpty(QueryQuestion))
  //          ViewState["QuestionKey"] = QueryQuestion;
  //        #endregion

  //        #region Redirect if bad entry
  //        if (
  //          (ViewState["PoliticianKey"].ToString() == string.Empty)
  //          || (ViewState["QuestionKey"].ToString() == string.Empty)
  //          )
  //        {
  //          //if (db.User() == db.User_.Master)
  //          //  Response.Redirect(db.Url_Master_USDefault());
  //          //else
  //          //  Response.Redirect(UrlManager.CanonicalSiteUri.ToString());
  //          HandleFatalError("The PoliticianKey and/or QuestionKey is missing");
  //        }
  //        #endregion

  //        #region Msg if Politician or Question No longer in database
  //        if (Politicians.IsValid(ViewState["PoliticianKey"].ToString()))
  //          Msg.Text = db.Fail("There is no politician with an Id of " + ViewState["PoliticianKey"].ToString());
  //        if (db.Is_Valid_Question(ViewState["QuestionKey"].ToString()))
  //          Msg.Text = db.Fail("There is no question with a QuestionKey of " + ViewState["QuestionKey"].ToString());
  //        #endregion

  //        #region Show Data
  //        LabelPolitician.Text = Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString());

  //        LabelQuestion.Text = db.Questions_Str(
  //          ViewState["QuestionKey"].ToString()
  //          , "Question");

  //        //TextBoxDateStamp.Text = db.Answer_Date(
  //        //    ViewState["PoliticianKey"].ToString()
  //        //    , ViewState["QuestionKey"].ToString());

  //        //TextBoxDateStamp.Text = Convert.ToDateTime(
  //        //  TextBoxDateStamp.Text).ToString("MM/dd/yyyy");

  //        TextBoxDateStamp.Text = Convert.ToDateTime(
  //          db.Answer_Date(
  //            ViewState["PoliticianKey"].ToString()
  //            , ViewState["QuestionKey"].ToString())).ToString("MM/dd/yyyy");

  //        TextBoxSource.Text = db.Html_From_Db_For_Page(
  //          db.Answer_Source(
  //            ViewState["PoliticianKey"].ToString()
  //            , ViewState["QuestionKey"].ToString()));

  //        LabelSourceLength.Text = TextBoxSource.Text.Length.ToString();

  //        TextBoxAnswer.Text = db.Answer_Issue_Question(
  //          ViewState["PoliticianKey"].ToString()
  //          , ViewState["QuestionKey"].ToString());

  //        LabelAnswerLength.Text = TextBoxAnswer.Text.Length.ToString();
  //        #endregion

  //        Msg.Text = db.Msg("Make changes. Then click Record Changes.");
  //      }
  //      catch (Exception ex)
  //      {
  //        #region
  //        Msg.Text = db.Fail(ex.Message);
  //        db.Log_Error_Admin(ex);
  //        #endregion
  //      }
  //    }
  //  }

  //  protected void ButtonRecordChanges_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      #region Check Input Textboxes
  //      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxDateStamp);
  //      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxSource);
  //      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAnswer);

  //      if (TextBoxDateStamp.Text.Trim() == string.Empty)
  //        throw new ApplicationException("The Date is empty.");
  //      if (TextBoxSource.Text.Trim() == string.Empty)
  //        throw new ApplicationException("The Souce is empty.");
  //      if (TextBoxAnswer.Text.Trim() == string.Empty)
  //        throw new ApplicationException("The Answer is empty.");

  //      if (!db.Is_Valid_Date(TextBoxDateStamp.Text))
  //        throw new ApplicationException("The date is not valid.");

  //      if(Convert.ToInt16(TextBoxSource.Text.Trim().Length) > 85)
  //      {
  //        int ReduceBy = Convert.ToInt16(TextBoxSource.Text.Trim().Length) - 85;
  //        throw new ApplicationException("The Source can not exceed 85 characters."
  //          + " Reduce by " + ReduceBy.ToString() + " characters.");
  //      }

  //      if (Convert.ToInt16(TextBoxAnswer.Text.Trim().Length) > db.Max_Answer_Length_2000)
  //      {
  //        int ReduceBy = Convert.ToInt16(TextBoxAnswer.Text.Trim().Length) - db.Max_Answer_Length_2000;
  //        throw new ApplicationException("The Answer can not exceed "+ db.Max_Answer_Length_2000.ToString() + " characters."
  //          + " Reduce by " + ReduceBy.ToString() + " characters.");
  //      }
  //      #endregion

  //      #region Update Answers
  //      db.Throw_Exception_TextBox_Script(TextBoxDateStamp);
  //      db.Throw_Exception_TextBox_Script(TextBoxSource);
  //      db.Throw_Exception_TextBox_Script(TextBoxAnswer);

  //      //string Date = TextBoxDateStamp.Text.Trim();
  //      //string Source = TextBoxSource.Text.Trim();
  //      LabelSourceLength.Text = TextBoxDateStamp.Text.Trim().Length.ToString();
  //      LabelAnswerLength.Text = TextBoxAnswer.Text.Trim().Length.ToString();

  //      string UpdateSQL = "UPDATE Answers SET";
  //      UpdateSQL += " Answer = " + db.SQLLit(TextBoxAnswer.Text.Trim());
  //      //UpdateSQL += " ,AnswerLength = " + TextBoxAnswer.Text.Trim().Length.ToString();
  //      UpdateSQL += " ,Source = " + db.SQLLit(TextBoxSource.Text.Trim());
  //      UpdateSQL += " ,DateStamp = " + db.SQLLit(TextBoxDateStamp.Text.Trim());
  //      UpdateSQL += " ,UserName = " + db.SQLLit(SecurePage.UserName);
  //      UpdateSQL += " WHERE PoliticianKey = " + db.SQLLit(ViewState["PoliticianKey"].ToString());
  //      UpdateSQL += " AND QuestionKey = " + db.SQLLit(ViewState["QuestionKey"].ToString());
  //      db.ExecuteSQL(UpdateSQL);
  //      #endregion

  //      Msg.Text = db.Ok("Your changes were recorded.");
  //    }
  //    catch (Exception ex)
  //    {
  //      #region
  //      Msg.Visible = true;
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //      #endregion
  //    }

  //  }
  }
}
