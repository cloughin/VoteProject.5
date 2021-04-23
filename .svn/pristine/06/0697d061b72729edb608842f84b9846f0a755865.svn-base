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

namespace Vote.Admin
{
  public partial class DesignElectionPage : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageUSPresPrimary, "IsTextInstructionsElectionPageUSPresPrimary");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageUSPres, "IsTextInstructionsElectionPageUSPres");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageUSSenate, "IsTextInstructionsElectionPageUSSenate");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageUSHouse, "IsTextInstructionsElectionPageUSHouse");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageState, "IsTextInstructionsElectionPageState");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageCounty, "IsTextInstructionsElectionPageCounty");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsElectionPageLocal, "IsTextInstructionsElectionPageLocal");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageUSPresPrimary
    //    , "InstructionsElectionPageUSPresPrimary"
    //    , "IsTextInstructionsElectionPageUSPresPrimary"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageUSPres
    //    , "InstructionsElectionPageUSPres"
    //    , "IsTextInstructionsElectionPageUSPres"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageUSSenate
    //    , "InstructionsElectionPageUSSenate"
    //    , "IsTextInstructionsElectionPageUSSenate"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageUSHouse
    //    , "InstructionsElectionPageUSHouse"
    //    , "IsTextInstructionsElectionPageUSHouse"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageState
    //    , "InstructionsElectionPageState"
    //    , "IsTextInstructionsElectionPageState"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageCounty
    //    , "InstructionsElectionPageCounty"
    //    , "IsTextInstructionsElectionPageCounty"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsElectionPageLocal
    //    , "InstructionsElectionPageLocal"
    //    , "IsTextInstructionsElectionPageLocal"
    //    );

    //  #endregion


    //  #region Custom Content in Texboxes
    //  TextBoxInstructionsElectionPageUSPresPrimary.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageUSPresPrimary");
    //  TextBoxInstructionsElectionPageUSPres.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageUSPres");
    //  TextBoxInstructionsElectionPageUSSenate.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageUSSenate");
    //  TextBoxInstructionsElectionPageUSHouse.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageUSHouse");
    //  TextBoxInstructionsElectionPageState.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageState");
    //  TextBoxInstructionsElectionPageCounty.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageCounty");
    //  TextBoxInstructionsElectionPageLocal.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectionPageLocal");
    //  #endregion Custom Content in Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Election.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Election.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Election.css");
    //  #endregion Style Sheets
    //}

    //// ---------- Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsElectionPageUSPresPrimary_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPresPrimary
    //      ,  TextBoxInstructionsElectionPageUSPresPrimary
    //      ,  LabelInstructionsElectionPageUSPresPrimary
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSPresPrimary"
    //      , "InstructionsElectionPageUSPresPrimary"
    //      //, "Election"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSPresPrimary);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageUSPres_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPres
    //      ,  TextBoxInstructionsElectionPageUSPres
    //      ,  LabelInstructionsElectionPageUSPres
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSPres"
    //      , "InstructionsElectionPageUSPres"
    //      //, "Election"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSPres);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageUSSenate_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSSenate
    //      ,  TextBoxInstructionsElectionPageUSSenate
    //      ,  LabelInstructionsElectionPageUSSenate
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSSenate"
    //      , "InstructionsElectionPageUSSenate"
    //      //, "Election"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSSenate);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageUSHouse_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSHouse
    //      ,  TextBoxInstructionsElectionPageUSHouse
    //      ,  LabelInstructionsElectionPageUSHouse
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSHouse"
    //      , "InstructionsElectionPageUSHouse"
    //      //, "Election"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSHouse);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageState_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageState
    //      ,  TextBoxInstructionsElectionPageState
    //      ,  LabelInstructionsElectionPageState
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageState"
    //      , "InstructionsElectionPageState"
    //      //, "Election"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageState);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageCounty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageCounty
    //      ,  TextBoxInstructionsElectionPageCounty
    //      ,  LabelInstructionsElectionPageCounty
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageCounty"
    //      , "InstructionsElectionPageCounty"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageCounty);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsElectionPageLocal_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageLocal
    //      ,  TextBoxInstructionsElectionPageLocal
    //      ,  LabelInstructionsElectionPageLocal
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageLocal"
    //      , "InstructionsElectionPageLocal"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageLocal);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Get Default Design Button ---------------------
    //protected void ButtonGetDefaultInstructionsElectionPageUSPresPrimary_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageUSPresPrimary
    //      , "InstructionsElectionPageUSPresPrimary");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageUSPres_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageUSPres
    //      , "InstructionsElectionPageUSPres");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageUSSenate_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageUSSenate
    //      , "InstructionsElectionPageUSSenate");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageUSHouse_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageUSHouse
    //      , "InstructionsElectionPageUSHouse");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageState_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageState
    //      , "InstructionsElectionPageState");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageCounty_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageCounty
    //      , "InstructionsElectionPageCounty");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsElectionPageLocal_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsElectionPageLocal
    //      , "InstructionsElectionPageLocal");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Clear Custom Design Button ---------------------
    //protected void ButtonClearCustomInstructionsElectionPageUSPresPrimary_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPresPrimary
    //      ,  TextBoxInstructionsElectionPageUSPresPrimary
    //      ,  LabelInstructionsElectionPageUSPresPrimary
    //      , "IsTextInstructionsElectionPageUSPresPrimary"
    //      , "InstructionsElectionPageUSPresPrimary"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageUSPres_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPres
    //      ,  TextBoxInstructionsElectionPageUSPres
    //      ,  LabelInstructionsElectionPageUSPres
    //      , "IsTextInstructionsElectionPageUSPres"
    //      , "InstructionsElectionPageUSPres"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageUSSenate_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSSenate
    //      ,  TextBoxInstructionsElectionPageUSSenate
    //      ,  LabelInstructionsElectionPageUSSenate
    //      , "IsTextInstructionsElectionPageUSSenate"
    //      , "InstructionsElectionPageUSSenate"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageUSHouse_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSHouse
    //      ,  TextBoxInstructionsElectionPageUSHouse
    //      ,  LabelInstructionsElectionPageUSHouse
    //      , "IsTextInstructionsElectionPageUSHouse"
    //      , "InstructionsElectionPageUSHouse"
    //      //, "Election"
    //      );
    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageState_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageState
    //      ,  TextBoxInstructionsElectionPageState
    //      ,  LabelInstructionsElectionPageState
    //      , "IsTextInstructionsElectionPageState"
    //      , "InstructionsElectionPageState"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageCounty_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageCounty
    //      ,  TextBoxInstructionsElectionPageCounty
    //      ,  LabelInstructionsElectionPageCounty
    //      , "IsTextInstructionsElectionPageCounty"
    //      , "InstructionsElectionPageCounty"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsElectionPageLocal_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageLocal
    //      ,  TextBoxInstructionsElectionPageLocal
    //      ,  LabelInstructionsElectionPageLocal
    //      , "IsTextInstructionsElectionPageLocal"
    //      , "InstructionsElectionPageLocal"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Submit Custom Design Button ---------------------
    //protected void ButtonSubmitCustomInstructionsElectionPageUSPresPrimary_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPresPrimary
    //      ,  TextBoxInstructionsElectionPageUSPresPrimary
    //      ,  LabelInstructionsElectionPageUSPresPrimary
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSPresPrimary"
    //      , "InstructionsElectionPageUSPresPrimary"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsElectionPageUSPres_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSPres
    //      ,  TextBoxInstructionsElectionPageUSPres
    //      ,  LabelInstructionsElectionPageUSPres
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSPres"
    //      , "InstructionsElectionPageUSPres"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustom1_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSSenate
    //      ,  TextBoxInstructionsElectionPageUSSenate
    //      ,  LabelInstructionsElectionPageUSSenate
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSSenate"
    //      , "InstructionsElectionPageUSSenate"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsElectionPageUSHouse_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageUSHouse
    //      ,  TextBoxInstructionsElectionPageUSHouse
    //      ,  LabelInstructionsElectionPageUSHouse
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageUSHouse"
    //      , "InstructionsElectionPageUSHouse"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsElectionPageState_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageState
    //      ,  TextBoxInstructionsElectionPageState
    //      ,  LabelInstructionsElectionPageState
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageState"
    //      , "InstructionsElectionPageState"
    //      //, "Election"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsElectionPageCounty_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageCounty
    //      ,  TextBoxInstructionsElectionPageCounty
    //      ,  LabelInstructionsElectionPageCounty
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageCounty"
    //      , "InstructionsElectionPageCounty"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsElectionPageLocal_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsElectionPageLocal
    //      ,  TextBoxInstructionsElectionPageLocal
    //      ,  LabelInstructionsElectionPageLocal
    //      , string.Empty
    //      , "IsTextInstructionsElectionPageLocal"
    //      , "InstructionsElectionPageLocal"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Style Sheet Buttons ---------------------

    //protected void ButtonUploadStyleSheet_ServerClick1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpload(
    //      Request.Files["FileStyleSheet"]
    //      , TextBoxCustomStyleSheet
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Election.css"));

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonUploadStyleSheet();
    //  }

    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonDeleteCustomStyleSheet1_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetDelete(
    //      TextBoxCustomStyleSheet
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Election.css"));

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDeleteStyleSheet();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    db.Log_Error_Admin(ex);
    //    Msg.Text = db.Fail(ex.Message);
    //    #endregion
    //  }
    //}

    //protected void ButtonUpdateCustomStyleSheet_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpdateAndCheckTextBox(
    //      TextBoxCustomStyleSheet
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Election.css"));

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
    //    #region Security Checks
    //    if (db.Domain_DesignCode_This() == string.Empty)
    //      db.HandleMissingDomainDesignCode();
    //    #endregion Security Checks

    //    //Needs to be done on PostBack because design changes on any submission and needs to be reloaded.
    //    try
    //    {
    //      LabelDesignCode.Text = db.Domain_DesignCode_This();

    //      HyperLinkSamplePage.NavigateUrl = db.Url_SamplePage(
    //        db.Domain_DesignCode_This(), 
    //        UrlManager.GetElectionPageUri(
    //        "20081104G" + db.StateCode_SamplePage() + "000000ALL")).ToString();

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

    #endregion Dead code





  }
}
