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

namespace Vote.PoliticianEmail
{
  public partial class PoliticianEmail : SecurePage
  {
    //protected void LoadTextBoxes()
    //{
    //  TextBoxEmailText1.Text = db.Master_Str("Report1");
    //  TextBoxEmailText2.Text = db.Master_Str("Report2");
    //}
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  Page.Title = "Politician Email";
    //  if (!SecurePage.IsMasterUser)
    //    SecurePage.HandleSecurityException();

    //  if (!IsPostBack)
    //  {
    //    try
    //    {
    //      LoadTextBoxes();
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

    //protected void ButtonRecord_Click(object sender, EventArgs e)
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmailText1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmailText2);

    //  db.Master_Update_Str("Report1", TextBoxEmailText1.Text);
    //  db.Master_Update_Str("Report2", TextBoxEmailText2.Text);
    //  Msg.Text = db.Ok("Data was recorded!!!");
    //}
  }
}
