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
  public partial class NotesView : VotePage
  {
  //  private void LoadNotes()
  //  {
  //    string SQL = string.Empty;
  //    SQL += "SELECT ";
  //    SQL += " LogNotes.DateStamp";
  //    SQL += ",LogNotes.UserName";
  //    SQL += ",LogNotes.UserStateCode";
  //    SQL += ",LogNotes.Note";
  //    SQL += " FROM LogNotes ";
  //    SQL += " WHERE UserStateCode = " + db.SQLLit(Session["UserStateCode"].ToString());
  //    SQL += " ORDER BY DateStamp DESC";

  //    DataTable NotesTable = db.Table(SQL);
  //    LabelNotes.Text = string.Empty;
  //    foreach (DataRow NoteRow in NotesTable.Rows)
  //    {
  //      LabelNotes.Text += "<hr>" + NoteRow["DateStamp"].ToString();
  //      LabelNotes.Text += " | " + NoteRow["UserName"].ToString();
  //      LabelNotes.Text += "<br>" + db.Str_CrLf_To_Br(NoteRow["Note"].ToString());
  //    }
  //  }

  //public partial class NotesView : VotePage
  //{
  //  //private void LoadNotes()
  //  //{
  //  //  var notesTable = DB.VoteLog.LogNotes.GetDataByUserStateCode(Session["UserStateCode"].ToString());
  //  //  LabelNotes.Text = string.Empty;
  //  //  foreach (var noteRow in notesTable)
  //  //  {
  //  //    LabelNotes.Text += "<hr>" + noteRow.DateStamp.ToString();
  //  //    LabelNotes.Text += " | " + noteRow.UserName;
  //  //    LabelNotes.Text += "<br>" + noteRow.Note.ReplaceNewLinesWithBreakTags();
  //  //  }
  //  //}

  //  //private void Page_Load(object sender, System.EventArgs e)
  //  //{
  //  //  if (!IsPostBack)//first time
  //  //  {
  //  //    if (1SecurePage.IsMasterUser)
  //  //      SecurePage.HandleSecurityException();

  //  //    try
  //  //    {
  //  //      PageTitle.Text = db.PageTitleDesign(StateCache.GetStateName(Session["UserStateCode"].ToString()) + " Notes"
  //  //        , db.Domain_DesignCode_This()
  //  //        //, db.User_Security()
  //  //        //,db.User_Security()
  //  //        );

  //  //      LoadNotes();
  //  //    }
  //  //    catch (Exception ex)
  //  //    {
  //  //      Msg.Text = db.Fail(ex.Message);
  //  //      db.Log_Error_Admin(ex);
  //  //    }
  //  //  }
  //  //}
  }
}
