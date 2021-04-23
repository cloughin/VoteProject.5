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

namespace Vote.AnswersPage
{
  public partial class AnswersPage : VotePage
  {
    //protected void ButtonGetAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLoginUserName);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFrom);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxTo);

    //    if (TextBoxLoginUserName.Text.Trim() == string.Empty)
    //      throw new ApplicationException("The Login Username is empty.");

    //    HtmlTable HTMLTableAnswers = new HtmlTable();
    //    HtmlTableRow trReportDetail = new HtmlTableRow();
    //    HtmlTableRow HTMLAnswerRow = new HtmlTableRow();
    //    #region Heading
    //    //<tr Class="trReportDetail">
    //    //HtmlTableRow trReportDetail = new HtmlTableRow();
    //    trReportDetail = db.Add_Tr_To_Table_Return_Tr(HTMLTableAnswers, "trReportDetail");
    //    //<td Class="ReportHeading" align="center">
    //    db.Add_Td_To_Tr(trReportDetail, "Date", "ReportHeading", "center");
    //    db.Add_Td_To_Tr(trReportDetail, "Length", "ReportHeading", "center");
    //    db.Add_Td_To_Tr(trReportDetail, "Politician", "ReportHeading", "center");
    //    db.Add_Td_To_Tr(trReportDetail, "Question<br>Source (Maximum 85 Characters)<br>Answer (Maximum 2,000 Characters)", "ReportHeading");
    //    #endregion

    //    string SQL = string.Empty;
    //    SQL = "SELECT";
    //    SQL += " PoliticianKey";
    //    SQL += ",IssueKey";
    //    SQL += ",QuestionKey";
    //    //SQL += ",AnswerLength";
    //    SQL += ",Source";
    //    SQL += ",DateStamp";
    //    SQL += ",UserName";
    //    SQL += ",Answer";
    //    SQL += " FROM Answers";
    //    SQL += " WHERE UserName = " + db.SQLLit(TextBoxLoginUserName.Text);
    //    SQL += " AND DateStamp >= " + db.SQLLit(TextBoxFrom.Text);
    //    SQL += " AND DateStamp <= " + db.SQLLit(TextBoxTo.Text);
    //    SQL += " ORDER BY DateStamp DESC";

    //    //DataTable AnswersTable = db.Table(sql.Answers4LoginDates
    //    //  (
    //    //  TextBoxLoginUserName.Text
    //    //  , TextBoxFrom.Text
    //    //  ,TextBoxTo.Text
    //    //  ));
    //    DataTable AnswersTable = db.Table(SQL);
    //    int Answers = 0;
    //    foreach (DataRow AnswerRow in AnswersTable.Rows)
    //    {
    //      #region Answer
    //      //<td Class="tdReportDetail" align="center">
    //      trReportDetail = db.Add_Tr_To_Table_Return_Tr(
    //        HTMLTableAnswers
    //        , "trReportDetail"
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , Convert.ToDateTime(AnswerRow["DateStamp"]).ToString("MM/dd/yyyy")
    //        , "tdReportDetail"
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , ""
    //        , "tdReportDetail"
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , "<nobr>" + Politicians.GetFormattedName(
    //            AnswerRow["PoliticianKey"].ToString()) + "</nobr>"
    //        , "tdReportDetail"
    //        );
    //      string Anchor = db.Anchor_Master_AnswersEdit(
    //        AnswerRow["PoliticianKey"].ToString()
    //        , AnswerRow["QuestionKey"].ToString()
    //        , db.Questions_Str(
    //            AnswerRow["QuestionKey"].ToString()
    //            , "Question"
    //            )
    //        , "_edit"
    //        );
    //      db.Add_Td_To_Tr(trReportDetail, Anchor, "tdReportDetail");

    //      trReportDetail = db.Add_Tr_To_Table_Return_Tr(
    //        HTMLTableAnswers
    //        , "trReportDetail"
    //        );
    //      db.Add_Td_To_Tr(trReportDetail, "", "tdReportDetail", "center", 1);
    //      string Souce = AnswerRow["Source"].ToString();
    //      int len = Souce.Length;
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , len.ToString()
    //        , "tdReportDetail"
    //        , "left"
    //        , 1
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , ""
    //        , "tdReportDetail"
    //        , "center", 1
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , db.Html_From_Db_For_Page(AnswerRow["Source"].ToString())
    //        , "tdReportDetail"
    //        , "left"
    //        , 1
    //        );

    //      trReportDetail = db.Add_Tr_To_Table_Return_Tr(
    //        HTMLTableAnswers
    //        , "trReportDetail"
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , ""
    //        , "tdReportDetail"
    //        , "center"
    //        , 1)
    //        ;
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        //, Convert.ToInt16(AnswerRow["AnswerLength"]).ToString()
    //        ,""
    //        , "tdReportDetail", "left"
    //        , 1
    //        );
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , ""
    //        , "tdReportDetail"
    //        , "center"
    //        , 1)
    //        ;
    //      db.Add_Td_To_Tr(
    //        trReportDetail
    //        , db.Html_From_Db_For_Page(AnswerRow["Answer"].ToString())
    //        , "tdReportDetail"
    //        , "left"
    //        , 1
    //        );
    //      #endregion
    //      Answers++;
    //    }

    //    #region Totals
    //    trReportDetail = db.Add_Tr_To_Table_Return_Tr(HTMLTableAnswers, "trReportDetail");
    //    db.Add_Td_To_Tr(trReportDetail, "Total Answers", "tdReportDetail");
    //    db.Add_Td_To_Tr(trReportDetail, Answers.ToString(), "tdReportDetail");
    //    #endregion

    //    LabelReport.Text = db.RenderToString(HTMLTableAnswers);

    //    Msg.Text = db.Msg("Click on a question to change the Date, Source or Answer."
    //      + " Click Get Answers whenever you need to obtain and new report.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      LabelCurrentDate.Text = DateTime.Now.ToString();
    //      //if (db.User_Name().ToLower() == "ron")
    //      if (SecurePage.IsSuperUser)
    //      {
    //        TextBoxLoginUserName.Enabled = true;
    //      }
    //      else
    //      {
    //        TextBoxLoginUserName.Enabled = false;
    //        TextBoxLoginUserName.Text = SecurePage.UserName;
    //      }
    //      Msg.Text = db.Msg("Enter a small range of dates (MM/DD/YYYY), like one or two days, of Answers to view. Then click Get Answers.");
    //    }
    //    catch (Exception ex)
    //    {
    //      #region
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //      #endregion
    //    }
    //  }
    //}

  }
}
