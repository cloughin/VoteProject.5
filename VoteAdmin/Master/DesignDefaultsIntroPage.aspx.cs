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
  public partial class DesignDefaultsIntroPage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsIntro, "IsTextInstructionsIntroPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      LabelInstructionsIntro
    //     , "InstructionsIntroPage"
    //     , "IsTextInstructionsIntroPage"
    //     );
    //  //additional substitution for politician
    //  LabelInstructionsIntro.Text = db.Subsitutions_For_Politician("ILObamaBarack", LabelInstructionsIntro.Text);

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("InstructionsIntroPage") != string.Empty)
    //    TextBoxInstructionsIntro.Text = db.MasterDesign_Str("InstructionsIntroPage");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Intro.css");
    //}
    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsIntro_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIntro
    //      ,  TextBoxInstructionsIntro
    //      ,  LabelInstructionsIntro
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIntroPage"
    //      , "InstructionsIntroPage"
    //      //, "Intro"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIntro);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ----------------- Update Default Design Button -----------------
    //protected void ButtonSubmitDefaultInstructionsIntro_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIntro
    //      ,  TextBoxInstructionsIntro
    //      ,  LabelInstructionsIntro
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIntroPage"
    //      , "InstructionsIntroPage"
    //      //, "Intro"
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
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Intro.css");

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
    //        UrlManager.GetIntroPageUri("ILObamaBarack").ToString();
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
