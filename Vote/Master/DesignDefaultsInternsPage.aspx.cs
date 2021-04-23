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
  public partial class DesignDefaultsInternsPage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextContentInterns, "IsTextContentInternsPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      LabelContentInterns
    //     , "ContentInternsPage"
    //     , "IsTextContentInternsPage"
    //     );

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("ContentInternsPage") != string.Empty)
    //    TextBoxContentInterns.Text = db.MasterDesign_Str("ContentInternsPage");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Interns.css");
    //}
    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextContentInterns_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextContentInterns
    //      ,  TextBoxContentInterns
    //      ,  LabelContentInterns
    //      , string.Empty //IsInclude = true
    //      , "IsTextContentInternsPage"
    //      , "ContentInternsPage"
    //      //, "Interns"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextContentInterns);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ----------------- Update Default Design Button -----------------
    //protected void ButtonSubmitDefaultContentInterns_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextContentInterns
    //      ,  TextBoxContentInterns
    //      ,  LabelContentInterns
    //      , string.Empty //IsInclude = true
    //      , "IsTextContentInternsPage"
    //      , "ContentInternsPage"
    //      //, "Interns"
    //      );
    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Style Sheet Button ---------------------

    //protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Interns.css");
    //    //Don't need to remove Cache pages when a style sheet changes
    //    //db.Cache_Remove_For_Page("Interns");

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonUpdateStyleSheet();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //// ----------------- Page Load -----------------

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      ReLoad_Entire_Form_To_View_Changes();

    //      HyperLinkSamplePage.NavigateUrl =
    //        UrlManager.GetForVolunteersPageUri().ToString();
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
