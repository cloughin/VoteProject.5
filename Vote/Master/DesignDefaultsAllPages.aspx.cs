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
using System.IO;

namespace Vote.Master
{
  public partial class DesignAllPagesDefaults : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextFirstFooterAllPages, "IsTextFirstFooterAllPages");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextSecondFooterAllPages, "IsTextSecondFooterAllPages");
    //  //db.RadioButtonResetIsTextDefault( RadioButtonListIsTextFirstFooterAllPages, "FirstFooterAllPages");
    //  //db.RadioButtonResetIsTextDefault( RadioButtonListIsTextSecondFooterAllPages, "SecondFooterAllPages");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //     LabelEmailUsLineAllPages
    //    , "EmailUsLineAllPages");

    //  db.Label_Default_Set(
    //     LabelEmailUsAddressAllPages
    //    , "EmailUsAddressAllPages");
    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("FirstFooterAllPages") != string.Empty)
    //    TextBoxFirstFooterAllPages.Text = db.MasterDesign_Str("FirstFooterAllPages");

    //  if (db.MasterDesign_Str("SecondFooterAllPages") != string.Empty)
    //    TextBoxSecondFooterAllPages.Text = db.MasterDesign_Str("SecondFooterAllPages");

    //  if (db.MasterDesign_Str("EmailUsLineAllPages") != string.Empty)
    //    TextBoxEmailUsLineAllPages.Text = db.MasterDesign_Str("EmailUsLineAllPages");

    //  if (db.MasterDesign_Str("EmailUsAddressAllPages") != string.Empty)
    //    TextBoxEmailUsAddressAllPages.Text = db.MasterDesign_Str("EmailUsAddressAllPages");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\All.css");
    //}

    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextFirstFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextFirstFooterAllPages
    //      ,  TextBoxFirstFooterAllPages
    //      , "IsIncludedFirstFooterAllPages"
    //      , "IsTextFirstFooterAllPages"
    //      , "FirstFooterAllPages"
    //      //, "AllPages"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextFirstFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextSecondFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextSecondFooterAllPages
    //      ,  TextBoxSecondFooterAllPages
    //      , "IsIncludedSecondFooterAllPages"
    //      , "IsTextSecondFooterAllPages"
    //      , "SecondFooterAllPages"
    //      //, "AllPages"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextSecondFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ----------------- Update Default Design Buttons -----------------

    //protected void ButtonSubmitDefaultFirstFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextFirstFooterAllPages
    //      ,  TextBoxFirstFooterAllPages
    //      //,  LabelFirstFooterAllPages
    //      , "IsIncludedFirstFooterAllPages"
    //      , "IsTextFirstFooterAllPages"
    //      , "FirstFooterAllPages"
    //      //, "AllPages"
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

    //protected void ButtonSubmitDefaultSubmitSecondFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextSecondFooterAllPages
    //      ,  TextBoxSecondFooterAllPages
    //      , "IsIncludedSecondFooterAllPages"
    //      , "IsTextSecondFooterAllPages"
    //      , "SecondFooterAllPages"
    //      //, "AllPages"
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

    //protected void ButtonSubmitDefaultEmailUs_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       TextBoxEmailUsLineAllPages
    //      ,  LabelEmailUsLineAllPages
    //      , "IsIncludedEmailUsAllPages"
    //      , "EmailUsLineAllPages"
    //      //, "AllPages"
    //      );

    //    db.Submit_Textbox_Default(
    //       TextBoxEmailUsAddressAllPages
    //      ,  LabelEmailUsAddressAllPages
    //      , "IsIncludedEmailUsAllPages"
    //      , "EmailUsAddressAllPages"
    //      //, "AllPages"
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
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\All.css");
    //    //Don't need to remove Cache pages when a style sheet changes
    //    //db.Cache_Remove_All();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonUpdateStyleSheet();
    //    //Response.Redirect("/Master/DesignDefaultsAllPages.aspx?Msg=" + db.Msg4ButtonUpdateStyleSheet());
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

    //#region Dead code

    ////// ------------ Get Default Design Buttons -------------------

    ////protected void ButtonGetDefaultFirstFooterAllPages_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxFirstFooterAllPages
    ////      , "FirstFooterAllPages");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultSecondFooterAllPages_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxSecondFooterAllPages
    ////      , "SecondFooterAllPages");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultEmailUs_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxEmailUsLineAllPages
    ////      , "EmailUsLineAllPages");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();

    ////    db.ButtonDefaultGet(
    ////       TextBoxEmailUsAddressAllPages
    ////      , "EmailUsAddressAllPages");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    //#endregion Dead code


  }
}
