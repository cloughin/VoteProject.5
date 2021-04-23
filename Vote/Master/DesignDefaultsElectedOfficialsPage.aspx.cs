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
  public partial class DesignDefaultsElectedOfficialsCountiesPage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageUSPres, "IsTextInstructionsElectedOfficialsPageUSPres");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageUSSenate, "IsTextInstructionsElectedOfficialsPageUSSenate");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageUSHouse, "IsTextInstructionsElectedOfficialsPageUSHouse");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageGovernors, "IsTextInstructionsElectedOfficialsPageGovernors");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageState, "IsTextInstructionsElectedOfficialsPageState");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageCounty, "IsTextInstructionsElectedOfficialsPageCounty");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsOfficialsPageLocal, "IsTextInstructionsElectedOfficialsPageLocal");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageUSPres
    //     , "InstructionsElectedOfficialsPageUSPres"
    //     , "IsTextInstructionsElectedOfficialsPageUSPres"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageUSSenate
    //     , "InstructionsElectedOfficialsPageUSSenate"
    //     , "IsTextInstructionsElectedOfficialsPageUSSenate"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageUSHouse
    //     , "InstructionsElectedOfficialsPageUSHouse"
    //     , "IsTextInstructionsElectedOfficialsPageUSHouse"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageGovernors
    //     , "InstructionsElectedOfficialsPageGovernors"
    //     , "IsTextInstructionsElectedOfficialsPageGovernors"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageState
    //      , "InstructionsElectedOfficialsPageState"
    //    , "IsTextInstructionsElectedOfficialsPageState"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageCounty
    //     , "InstructionsElectedOfficialsPageCounty"
    //     , "IsTextInstructionsElectedOfficialsPageCounty"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsOfficialsPageLocal
    //     , "InstructionsElectedOfficialsPageLocal"
    //     , "IsTextInstructionsElectedOfficialsPageLocal"
    //     );

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageUSPres") != string.Empty)
    //    TextBoxInstructionsOfficialsPageUSPres.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageUSPres");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageUSSenate") != string.Empty)
    //    TextBoxInstructionsOfficialsPageUSSenate.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageUSSenate");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageUSHouse") != string.Empty)
    //    TextBoxInstructionsOfficialsPageUSHouse.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageUSHouse");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageGovernors") != string.Empty)
    //    TextBoxInstructionsOfficialsPageGovernors.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageGovernors");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageState") != string.Empty)
    //    TextBoxInstructionsOfficialsPageState.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageState");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageCounty") != string.Empty)
    //    TextBoxInstructionsOfficialsPageCounty.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageCounty");

    //  if (db.MasterDesign_Str("InstructionsElectedOfficialsPageLocal") != string.Empty)
    //    TextBoxInstructionsOfficialsPageLocal.Text = db.MasterDesign_Str("InstructionsElectedOfficialsPageLocal");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Officials.css");
    //}
    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsOfficialsPageUSPres_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSPres
    //      ,  TextBoxInstructionsOfficialsPageUSPres
    //      ,  LabelInstructionsOfficialsPageUSPres
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSPres"
    //      , "InstructionsElectedOfficialsPageUSPres"
    //      //, "ElectedOfficials"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSPres);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIsTextInstructionsOfficialsPageUSSenate_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSSenate
    //      ,  TextBoxInstructionsOfficialsPageUSSenate
    //      ,  LabelInstructionsOfficialsPageUSSenate
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSSenate"
    //      , "InstructionsElectedOfficialsPageUSSenate"
    //      //, "ElectedOfficials"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSSenate);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIsTextInstructionsOfficialsPageUSHouse_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSHouse
    //      ,  TextBoxInstructionsOfficialsPageUSHouse
    //      ,  LabelInstructionsOfficialsPageUSHouse
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSHouse"
    //      , "InstructionsElectedOfficialsPageUSHouse"
    //      //, "ElectedOfficials"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSHouse);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIsTextInstructionsOfficialsPageGovernors_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageGovernors
    //      ,  TextBoxInstructionsOfficialsPageGovernors
    //      ,  LabelInstructionsOfficialsPageGovernors
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageGovernors"
    //      , "InstructionsElectedOfficialsPageGovernors"
    //      //, "ElectedOfficials"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageGovernors);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsOfficialsPageState_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageState
    //      ,  TextBoxInstructionsOfficialsPageState
    //      ,  LabelInstructionsOfficialsPageState
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageState"
    //      , "InstructionsElectedOfficialsPageState"
    //      //, "ElectedOfficials"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageState);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsOfficialsPageCounty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageCounty
    //      ,  TextBoxInstructionsOfficialsPageCounty
    //      ,  LabelInstructionsOfficialsPageCounty
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageCounty"
    //      , "InstructionsElectedOfficialsPageCounty"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageCounty);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    //protected void RadioButtonListIsTextInstructionsOfficialsPageLocal_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageLocal
    //      ,  TextBoxInstructionsOfficialsPageLocal
    //      ,  LabelInstructionsOfficialsPageLocal
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageLocal"
    //      , "InstructionsElectedOfficialsPageLocal"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageLocal);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //// ------------ Get Default Design Buttons -------------------
    //#region commented out - removed
    ////protected void ButtonGetDefaultInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxInstructionsOfficialsPageUSPres
    ////      , "InstructionsElectedOfficialsPageUSPres");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultInstructionsOfficialsPageUSSenate_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxInstructionsOfficialsPageUSSenate
    ////      , "InstructionsElectedOfficialsPageUSSenate");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxInstructionsOfficialsPageUSHouse
    ////      , "InstructionsElectedOfficialsPageUSHouse");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultInstructionsOfficialsPageState_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxInstructionsOfficialsPageState
    ////      , "InstructionsElectedOfficialsPageState");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////protected void ButtonGetDefaultInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.ButtonDefaultGet(
    ////       TextBoxInstructionsOfficialsPageCounty
    ////      , "InstructionsElectedOfficialsPageCounty");

    ////    Msg.Text = db.Msg4ButtonDefaultGet();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}
    //#endregion commented out - removed
    //// -----------------Submit Default------------
    //protected void ButtonSubmitDefaultInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSPres
    //      ,  TextBoxInstructionsOfficialsPageUSPres
    //      ,  LabelInstructionsOfficialsPageUSPres
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSPres"
    //      , "InstructionsElectedOfficialsPageUSPres"
    //      //, "ElectedOfficials"
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

    //protected void ButtonSubmitDefaultInstructionsOfficialsPageUSSenate_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSSenate
    //      ,  TextBoxInstructionsOfficialsPageUSSenate
    //      ,  LabelInstructionsOfficialsPageUSSenate
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSSenate"
    //      , "InstructionsElectedOfficialsPageUSSenate"
    //      //, "ElectedOfficials"
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

    //protected void ButtonSubmitDefaultInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageUSHouse
    //      ,  TextBoxInstructionsOfficialsPageUSHouse
    //      ,  LabelInstructionsOfficialsPageUSHouse
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageUSHouse"
    //      , "InstructionsElectedOfficialsPageUSHouse"
    //      //, "ElectedOfficials"
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

    //protected void ButtonSubmitDefaultInstructionsOfficialsPageGovernors_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageGovernors
    //      ,  TextBoxInstructionsOfficialsPageGovernors
    //      ,  LabelInstructionsOfficialsPageGovernors
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageGovernors"
    //      , "InstructionsElectedOfficialsPageGovernors"
    //      //, "ElectedOfficials"
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

    //protected void ButtonSubmitDefaultInstructionsOfficialsPageState_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageState
    //      ,  TextBoxInstructionsOfficialsPageState
    //      ,  LabelInstructionsOfficialsPageState
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageState"
    //      , "InstructionsElectedOfficialsPageState"
    //      //, "ElectedOfficials"
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

    //protected void ButtonSubmitDefaultInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageCounty
    //      ,  TextBoxInstructionsOfficialsPageCounty
    //      ,  LabelInstructionsOfficialsPageCounty
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageCounty"
    //      , "InstructionsElectedOfficialsPageCounty"
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
    //protected void ButtonSubmitDefaultInstructionsOfficialsPageLocal_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsOfficialsPageLocal
    //      ,  TextBoxInstructionsOfficialsPageLocal
    //      ,  LabelInstructionsOfficialsPageLocal
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsElectedOfficialsPageLocal"
    //      , "InstructionsElectedOfficialsPageLocal"
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
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Officials.css");

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
    //    if (db.User_Security() != "MASTER")
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      ReLoad_Entire_Form_To_View_Changes();

    //      HyperLinkSamplePage.NavigateUrl = 
    //        UrlManager.GetOfficialsPageUri("VA").ToString();
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
