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
  public partial class EmailsView : VotePage
  {
    //private void LoadEmails()
    //{
    //  string SQL = string.Empty;
    //  SQL += "SELECT ";
    //  SQL += " LogEmailsSent.UserName";
    //  SQL += ",LogEmailsSent.Sent";
    //  SQL += ",LogEmailsSent.FromAddr";
    //  SQL += ",LogEmailsSent.ToAddr";
    //  SQL += ",LogEmailsSent.Subject";
    //  SQL += ",LogEmailsSent.Body";
    //  SQL += " FROM LogEmailsSent ";
    //  SQL += " WHERE StateCode = " + db.SQLLit(Session["UserStateCode"].ToString());
    //  SQL += " ORDER BY Sent DESC";

    //  DataTable EmailsTable = db.Table(SQL);
    //  LabelNotes.Text = string.Empty;
    //  foreach (DataRow EmailRow in EmailsTable.Rows)
    //  {
    //    LabelNotes.Text += "<hr>" + EmailRow["Sent"].ToString();
    //    LabelNotes.Text += " | " + EmailRow["UserName"].ToString();
    //    LabelNotes.Text += "<br>From: " + EmailRow["FromAddr"]
    //      .ToString().ReplaceNewLinesWithBreakTags();
    //    LabelNotes.Text += "<br>To: " + EmailRow["ToAddr"]
    //      .ToString().ReplaceNewLinesWithBreakTags();
    //    LabelNotes.Text += "<br>Subject: " + EmailRow["Subject"]
    //      .ToString().ReplaceNewLinesWithBreakTags();
    //    LabelNotes.Text += "<br>" + EmailRow["Body"]
    //      .ToString().ReplaceNewLinesWithBreakTags();
    //  }
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack) //first time presented
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      PageTitle.Text = db.PageTitleDesign(StateCache.GetStateName(Session["UserStateCode"].ToString()) 
    //        + " Emails"
    //        , db.Domain_DesignCode_This()
    //        //, db.User_Security()
    //        //, db.User_Security()
    //        );

    //      LoadEmails();
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}
  }
}
