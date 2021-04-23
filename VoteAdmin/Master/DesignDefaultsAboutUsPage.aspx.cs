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
  public partial class DesignDefaultsAboutUsPage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextContentAboutUs, "IsTextContentAboutUsPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      LabelContentAboutUs
    //     , "ContentAboutUsPage");

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("ContentAboutUsPage") != string.Empty)
    //    TextBoxContentAboutUs.Text = db.MasterDesign_Str("ContentAboutUsPage");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\AboutUs.css");
    //}
    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextContentAboutUs_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextContentAboutUs
    //      ,  TextBoxContentAboutUs
    //      ,  LabelContentAboutUs
    //      , string.Empty //IsInclude = true
    //      , "IsTextContentAboutUsPage"
    //      , "ContentAboutUsPage"
    //      //, "AboutUs"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextContentAboutUs);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ----------------- Submit Default Design Button -----------------
    //protected void ButtonSubmitDefaultContentAboutUs_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextContentAboutUs
    //      ,  TextBoxContentAboutUs
    //      ,  LabelContentAboutUs
    //      , string.Empty //IsInclude = true
    //      , "IsTextContentAboutUsPage"
    //      , "ContentAboutUsPage"
    //      //, "AboutUs"
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


    ////-------------- Style Sheet Buttons ---------------------
    //protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\AboutUs.css");
    //    //Don't need to remove Cache pages when a style sheet changes
    //    //db.Cache_Remove_For_Page("AboutUs");

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
    //        UrlManager.GetAboutUsPageUri().ToString();
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
