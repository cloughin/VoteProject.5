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

namespace Vote.Master
{
  public partial class NotesRecord : VotePage
  {
    //private void LoadLastNote()
    //{
    //  string SQL = string.Empty;
    //  SQL += "SELECT ";
    //  SQL += " LogNotes.DateStamp";
    //  SQL += ",LogNotes.UserName";
    //  SQL += ",LogNotes.UserStateCode";
    //  SQL += ",LogNotes.Note";
    //  SQL += " FROM LogNotes ";
    //  SQL += " WHERE UserStateCode = " + db.SQLLit(Session["UserStateCode"].ToString());
    //  SQL += " ORDER BY DateStamp DESC";

    //  DataRow NoteRow = db.Row_First_Optional(SQL);
    //  if (NoteRow != null)
    //  {
    //    LabelLastNote.Text = NoteRow["DateStamp"].ToString();
    //    LabelLastNote.Text += " | " + NoteRow["UserName"].ToString();
    //    LabelLastNote.Text += "<hr>" + db.Str_CrLf_To_Br(NoteRow["Note"].ToString());
    //  }
    //}

    //private void LoadLastNote()
    //{
    //  var noteTable = DB.VoteLog.LogNotes.GetLatestDataByUserStateCode(Session["UserStateCode"].ToString());
    //  if (noteTable.Rows.Count > 0)
    //  {
    //    var noteRow = noteTable[0];
    //    LabelLastNote.Text = noteRow.DateStamp.ToString();
    //    LabelLastNote.Text += " | " + noteRow.UserName;
    //    LabelLastNote.Text += "<hr>" + noteRow.Note.ReplaceNewLinesWithBreakTags();
    //  }
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  try
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();


    //    if (!IsPostBack) //first time presented
    //    {
    //      if (!IsPostBack)//first time
    //      {
    //        PageTitle.Text = db.PageTitleDesign(db.States_Str(Session["UserStateCode"].ToString(), "State") + " Record Notes"
    //          , db.Domain_DesignCode_This()
    //          //, db.User_Security()
    //          //, db.User_Security()
    //          );

    //        LoadLastNote();
    //      }
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    #region Dead code

    //private void ButtonRecordInfo_Click(object sender, System.EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNote);
    //    //Record Note
    //    string InsertSQL = "INSERT INTO LogNotes ("
    //      + "DateStamp"
    //      + ",UserName"
    //      + ",UserStateCode"
    //      + ",Note"
    //      + ")"
    //      + " VALUES("
    //      + db.SQLLit(Db.DbNow)
    //      + "," + db.SQLLit(db.User_Name())
    //      + "," + db.SQLLit(Session["UserStateCode"].ToString())
    //      + "," + db.SQLLit(TextBoxNote.Text.ToString().Trim())
    //      + ")";
    //    db.ExecuteSQL(InsertSQL);

    //    LoadLastNote();

    //    TextBoxNote.Text = string.Empty;
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    #endregion Dead code

  }
}
