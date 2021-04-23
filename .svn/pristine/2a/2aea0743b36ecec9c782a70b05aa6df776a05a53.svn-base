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
  public partial class DesignDefaultPageState : VotePage
  {
    #region Dead code

    //private void ReLoad_Form_Except_Main_Content()
    //{
    //  #region Text / HTML Radio Buttons
    //  //db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This()
    //  //  ,  RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain
    //  //  , "IsTextInstructionsDefaultPageSingleStateDomain");
    //  #endregion

    //  #region Labels

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  Label_Title_Template_Default
    //    , "TitleTagDefaultPageSingleStateDomain"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  Label_Description_Template_Default
    //    , "MetaDescriptionTagDefaultPageSingleStateDomain"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  Label_Keywords_Template_Default
    //    , "MetaKeywordsTagDefaultPageSingleStateDomain"
    //    );

    //  //db.Label_Custom_Set(
    //  //   db.Domain_DesignCode_This()
    //  //   ,  LabelInstructionsDefaultPageSingleStateDomain
    //  //  , "InstructionsDefaultPageSingleStateDomain"
    //  //  , "IsTextInstructionsDefaultPageSingleStateDomain"
    //  //  );

    //  #endregion Labels

    //  #region Texboxes
    //  TextBoxTitleTagDefaultPageSingleStateDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "TitleTagDefaultPageSingleStateDomain");
    //  TextBoxMetaDescriptionTagDefaultPageSingleStateDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MetaDescriptionTagDefaultPageSingleStateDomain");
    //  TextBoxMetaKeywordsTagDefaultPageSingleStateDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MetaKeywordsTagDefaultPageSingleStateDomain");
    //  //TextBoxInstructionsDefaultPageSingleStateDomain.Text =
    //  //  db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //  //  , "InstructionsDefaultPageSingleStateDomain");
    //  #endregion Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Default.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Default.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Default.css");
    //  #endregion Style Sheets
    //}

    //#region  Radion Buttons for Text or HTML

    ////protected void RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.Submit_Textbox_Custom(
    ////      db.Domain_DesignCode_This()
    ////      ,  RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain
    ////      ,  TextBoxInstructionsDefaultPageSingleStateDomain
    ////      ,  LabelInstructionsDefaultPageSingleStateDomain
    ////      , "IsIncludedInstructionsDefaultPageSingleStateDomain"
    ////      , "IsTextInstructionsDefaultPageSingleStateDomain"
    ////      , "InstructionsDefaultPageSingleStateDomain"
    ////      //, "Default"
    ////      );

    ////    ReLoad_Form_Except_Main_Content();

    ////    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain);
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}
    //#endregion  Radion Buttons for Text or HTML

    //#region  Style Sheet Buttons

    //protected void ButtonUploadStyleSheet_ServerClick1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpload(
    //      Request.Files["FileStyleSheet"]
    //      , TextBoxCustomStyleSheet
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css"));

    //    ReLoad_Form_Except_Main_Content();

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css"));

    //    ReLoad_Form_Except_Main_Content();

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css"));

    //    ReLoad_Form_Except_Main_Content();

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

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
    //#endregion  Style Sheet Buttons

    //#region Main Content Buttons and Radio Button List

    //protected void Load_Main_Content_Textboxes_With_Templates()
    //{
    //  #region Automatic and Custom templates in textboxes
    //  switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //  {
    //    case "Default":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionNone"
    //          );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionNone"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates when there are NO"
    //      + " upcoming, viewable elections and their resulting effect is shown in the Main Content.");
    //      break;
    //    case "General":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //        );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates for a GENERAL"
    //      + " upcoming, viewable election and their resulting effect is shown in the Main Content.");
    //      break;
    //    case "OffYear":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //        );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates for an OFF YEAR"
    //      + " upcoming, viewable election and their resulting effect is shown in the Main Content.");
    //      break;
    //    case "Special":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //        );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates for a SPECIAL"
    //      + " upcoming, viewable election and their resulting effect is shown in the Main Content.");
    //      break;
    //    case "Primary":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //        );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates for a PRIMARY"
    //      + " upcoming, viewable election and their resulting effect is shown in the Main Content.");
    //      break;
    //    case "StatePresidentialPrimary":
    //      Label_Election_Template_Default.Text =
    //        db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //        );
    //      TextBox_MainContent_Custom.Text =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //          );
    //      Msg.Text = db.Ok("The textboxes contain the automatic and custom templates for an"
    //        + " upcoming, viewable election office contests only federal and state.");
    //      break;
    //    case "NationalPresidentialPrimary":
    //      break;
    //  }
    //  #endregion Automatic and Custom templates in textboxes
    //}

    //protected void Main_Content_Templates()
    //{
    //  string Template = string.Empty;
    //  switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //  {
    //    case "Default":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionNone"
    //      );
    //      break;
    //    case "General":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //        );
    //      break;
    //    case "OffYear":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //        );
    //      break;
    //    case "Special":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //        );
    //      break;
    //    case "Primary":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //        );
    //      break;
    //    case "StatePresidentialPrimary":
    //      Template =
    //        db.DomainDesigns_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //        );
    //      break;
    //    case "NationalPresidentialPrimary":
    //      break;
    //    default:
    //      Template =
    //        "<h1>Select a radio button to view the template"
    //      + " that will be used for that election status.</h1>";
    //      break;
    //  }
    //  #region commented out
    //  //if (string.IsNullOrEmpty(Template))
    //  //{
    //  //  switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //  //  {
    //  //    case "Default":
    //  //      Template =
    //  //        "<h1>There is no CUSTOM Template when there is NO upcoming, viewable election.</h1>";
    //  //      break;
    //  //    case "General":
    //  //      Template =
    //  //        "<h1>There is no CUSTOM Template for a GENERAL upcoming viewable election.</h1>";
    //  //      break;
    //  //    case "OffYear":
    //  //      Template =
    //  //        "<h1>There is no CUSTOM Template for an OFF YEAR viewable election.</h1>";
    //  //      break;
    //  //    case "Special":
    //  //      Template =
    //  //        "<h1>There is no CUSTOM Template for a SPECIAL upcoming viewable election.</h1>";
    //  //      break;
    //  //    case "Primary":
    //  //      Template =
    //  //        "<h1>There is no CUSTOM Template for a PRIMARY upcoming viewable election.</h1>";
    //  //      break;
    //  //  }
    //  //}
    //  #endregion commented out

    //  //MainContent.Text = db.Content4Html(db.Subsitutions_All(Template));
    //  Label_Election_Current.Text = db.Subsitutions_All(Template);
    //}

    //protected void RadioButtonList_Main_Content_Single_State_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Load_Main_Content_Textboxes_With_Templates();

    //    Main_Content_Templates();

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void Button_Get_Default_Template_Election_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //    {
    //      case "Default":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionNone"
    //        );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template when there are NO"
    //        + " upcoming, viewable elections.");
    //        break;
    //      case "General":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //            "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //          );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template for a GENERAL"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "OffYear":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //            "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //          );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template for an OFF YEAR"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "Special":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //            "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //          );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template for a SPECIAL"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "Primary":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //            "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //          );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template for a PRIMARY"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "StatePresidentialPrimary":
    //        TextBox_MainContent_Custom.Text =
    //          db.MasterDesign_Str(
    //            "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //          );
    //        Msg.Text = db.Ok("The textbox contains the MASTER template for an"
    //        + " upcoming, viewable election office contests only federal and state.");
    //        break;
    //      case "NationalPresidentialPrimary":
    //        break;
    //    }

    //    #region commented out
    //    ////TextBox_MainContent_Custom.Enabled = true;
    //    //MainContent.Text = TextBox_MainContent_Custom.Text;

    //    //ReLoad_Form_Except_Main_Content();
    //    ////Main_Content_Automatic_Or_Custom_Controls_Set();
    //    //Main_Content_Set_Automatic_Or_Custom();
    //    #endregion commented out
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void Button_Get_Custom_Template_Election_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //    {
    //      case "Default":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainElectionNone"
    //            );
    //        Msg.Text = db.Ok("The textbox contains the CUSTOM template when there are NO"
    //        + " upcoming, viewable elections.");
    //        break;
    //      case "General":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //            );
    //        Msg.Text = db.Ok("The textbox contains any CUSTOM template for a GENERAL"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "OffYear":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //            );
    //        Msg.Text = db.Ok("The textbox contains any CUSTOM template for an OFF YEAR"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "Special":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //            );
    //        Msg.Text = db.Ok("The textbox contains any CUSTOM template for a SPECIAL"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "Primary":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //            );
    //        Msg.Text = db.Ok("The textbox contains any CUSTOM template for a PRIMARY"
    //        + " upcoming, viewable election.");
    //        break;
    //      case "StatePresidentialPrimary":
    //        TextBox_MainContent_Custom.Text =
    //          db.DomainDesigns_Str(
    //              db.Domain_DesignCode_This()
    //              , "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //            );
    //        Msg.Text = db.Ok("The textbox contains any CUSTOM template for an"
    //        + " upcoming, viewable election office contests only federal and state.");
    //        break;
    //      case "NationalPresidentialPrimary":
    //        break;
    //    }

    //    #region comment out
    //    ////TextBox_MainContent_Custom.Enabled = true;
    //    //MainContent.Text = TextBox_MainContent_Custom.Text;

    //    //ReLoad_Form_Except_Main_Content();
    //    ////Main_Content_Automatic_Or_Custom_Controls_Set();
    //    //Main_Content_Set_Automatic_Or_Custom();
    //    #endregion comment out
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void Button_Save_Custom_Template_Election_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //    {
    //      case "Default":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionNone"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template when there are NO"
    //        + " upcoming, viewable election was saved.");
    //        break;
    //      case "General":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a GENERAL"
    //        + " upcoming, viewable election was saved.");
    //        break;
    //      case "OffYear":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for an OFF YEAR"
    //        + " upcoming, viewable election was saved.");
    //        break;
    //      case "Special":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a SPECIAL"
    //        + " upcoming, viewable election was saved.");
    //        break;
    //      case "Primary":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a PRIMARY"
    //        + " upcoming, viewable election was saved.");
    //        break;
    //      case "StatePresidentialPrimary":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //          , TextBox_MainContent_Custom.Text.Trim()
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for an"
    //        + " upcoming, viewable election office contests only federal and state.");
    //        break;
    //      case "NationalPresidentialPrimary":
    //        break;
    //    }

    //    Load_Main_Content_Textboxes_With_Templates();
    //    Main_Content_Templates();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void Button_Clear_Custom_Template_Election_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //    {
    //      case "Default":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionNone"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template when there are NO"
    //        + " upcoming, viewable election was deleted.");
    //        break;
    //      case "General":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a GENERAL"
    //        + " upcoming, viewable election was deleted.");
    //        break;
    //      case "OffYear":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for an OFF YEAR"
    //        + " upcoming, viewable election was deleted.");
    //        break;
    //      case "Special":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a SPECIAL"
    //        + " upcoming, viewable election was deleted.");
    //        break;
    //      case "Primary":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for a PRIMARY"
    //        + " upcoming, viewable election was deleted.");
    //        break;
    //      case "StatePresidentialPrimary":
    //        db.DomainDesigns_Update_Str(
    //          db.Domain_DesignCode_This()
    //          , "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //          , string.Empty
    //        );
    //        Msg.Text = db.Ok("The CUSTOM template for an"
    //        + " upcoming, viewable election office contests only federal and state.");
    //        break;
    //      case "OnlyFederalStatewide":
    //        break;
    //    }

    //    //Load_Main_Content_Custom_Changed();
    //    Load_Main_Content_Textboxes_With_Templates();
    //    Main_Content_Templates();

    // }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion Main Content Buttons and Radio Button List

    //// ----------------- Page Load -----------------
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region Security Checks
    //    if (db.Domain_DesignCode_This() == string.Empty)
    //      db.HandleMissingDomainDesignCode();
    //    #endregion Security Checks

    //    try
    //    {
    //      LabelDesignCode.Text = db.Domain_DesignCode_This();

    //      ReLoad_Form_Except_Main_Content();

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
    //// ----------------- Buttons -----------------

    //protected void Button_Get_Default_Template_Title_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Get_Custom_Template_Title_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Save_Custom_Template_Title_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      , TextBoxTitleTagDefaultPageSingleStateDomain
    //      , Label_Title_Template_Default
    //      //, "IsIncludedTitleTagDefaultPageSingleStateDomain"
    //      , "TitleTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Form_Except_Main_Content();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Clear_Custom_Template_Title_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      , TextBoxTitleTagDefaultPageSingleStateDomain
    //      , Label_Title_Template_Default
    //      , "TitleTagDefaultPageSingleStateDomain"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Get_Default_Template_Description_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Get_Custom_Template_Description_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Save_Custom_Template_Description_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      , TextBoxMetaDescriptionTagDefaultPageSingleStateDomain
    //      , Label_Description_Template_Default
    //      //, "IsIncludedMetaDescriptionTagDefaultPageSingleStateDomain"
    //      , "MetaDescriptionTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Form_Except_Main_Content();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}


    //protected void Button_Clear_Custom_Template_Description_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      , TextBoxMetaDescriptionTagDefaultPageSingleStateDomain
    //      , Label_Description_Template_Default
    //      , "MetaDescriptionTagDefaultPageSingleStateDomain"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Get_Default_Template_Keywords_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Get_Custom_Template_Keywords_Click(object sender, EventArgs e)
    //{

    //}


    //protected void Button_Save_Custom_Template_Keywords_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      , TextBoxMetaKeywordsTagDefaultPageSingleStateDomain
    //      , Label_Keywords_Template_Default
    //      //, "IsIncludedMetaKeywordsTagDefaultPageSingleStateDomain"
    //      , "MetaKeywordsTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Form_Except_Main_Content();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Clear_Custom_Template_Keywords_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      , TextBoxMetaKeywordsTagDefaultPageSingleStateDomain
    //      , Label_Keywords_Template_Default
    //      , "MetaKeywordsTagDefaultPageSingleStateDomain"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //#region Dead code


    ////protected void Main_Content()
    ////{
    ////  Label_Election_Current.Text =
    ////    //db.Subsitutions_All(
    ////      db.Main_Content_Default_Or_Custom(
    ////        db.StateCode_Domain_This()
    ////        , db.Domain_DesignCode_This()
    ////    //)
    ////        );
    ////}

    //#endregion Dead code

    #endregion Dead code



  }
}
