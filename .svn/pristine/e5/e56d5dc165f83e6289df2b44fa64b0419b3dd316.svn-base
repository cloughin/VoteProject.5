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
  public partial class DesignDefaultsDefaultPageState : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  //db.RadioButtonResetIsTextDefault(
    //  //   RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain
    //  //  , "IsTextInstructionsDefaultPageSingleStateDomain");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      Label_Title_Template_Default
    //     , "TitleTagDefaultPageSingleStateDomain");

    //  //db.Label_Default_Set(
    //  //    Label_Description_Template_Default
    //  //   , "MetaDescriptionTagDefaultPageSingleStateDomain");

    //  //db.Label_Default_Set(
    //  //    Label_Keywords_Template_Default
    //  //   , "MetaKeywordsTagDefaultPageSingleStateDomain");

    //  //if (RadioButtonList_Main_Content_Single_State.SelectedIndex != -1)
    //  //{
    //  //  switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //  //  {
    //  //    case "Default":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionNone");
    //  //      break;
    //  //    case "General":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionGeneral");
    //  //      break;
    //  //    case "OffYear":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionOffYear");
    //  //      break;
    //  //    case "Special":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionSpecial");
    //  //      break;
    //  //    case "Primary":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionPrimary");
    //  //      break;
    //  //    case "OnlyFederalStatewide":
    //  //      LableMainContentDefaultPageSingleStateDomain.Text =
    //  //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainContestsFederalStatewide");
    //  //      break;
    //  //  }
    //  //}
    //  //else
    //  //{
    //  //  LableMainContentDefaultPageSingleStateDomain.Text =
    //  //    "<h1>No Main Content has been selected</h1>"
    //  //    + "<br><h2>Select one of the radio buttons below.</h2>";
    //  //}

    //  //db.Label_Default_Set(
    //  //    LabelInstructionsDefaultPageSingleStateDomain
    //  //   , "InstructionsDefaultPageSingleStateDomain"
    //  //   , "IsTextInstructionsDefaultPageSingleStateDomain"
    //  //   );

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("TitleTagDefaultPageSingleStateDomain") != string.Empty)
    //    TextBoxTitleTagDefaultPageSingleStateDomain.Text =
    //      db.MasterDesign_Str("TitleTagDefaultPageSingleStateDomain");

    //  if (db.MasterDesign_Str("MetaDescriptionTagDefaultPageSingleStateDomain") != string.Empty)
    //    TextBoxMetaDescriptionTagDefaultPageSingleStateDomain.Text =
    //      db.MasterDesign_Str("MetaDescriptionTagDefaultPageSingleStateDomain");

    //  if (db.MasterDesign_Str("MetaKeywordsTagDefaultPageSingleStateDomain") != string.Empty)
    //    TextBoxMetaKeywordsTagDefaultPageSingleStateDomain.Text =
    //      db.MasterDesign_Str("MetaKeywordsTagDefaultPageSingleStateDomain");

    //  //if (db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionNone")
    //  //  != string.Empty)
    //  //  TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //  //    db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionNone");
    //  Load_MainContent_TextBox();


    //  //if (db.MasterDesign_Str("InstructionsDefaultPageSingleStateDomain")
    //  //  != string.Empty)
    //  //  TextBoxInstructionsDefaultPageSingleStateDomain.Text =
    //  //    db.MasterDesign_Str("InstructionsDefaultPageSingleStateDomain");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Default.css");
    //}

    //// ------------------ Radion Buttons for Text or HTML -----------

    ////protected void RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.Submit_Textbox_Default(
    ////       RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain
    ////      ,  TextBoxInstructionsDefaultPageSingleStateDomain
    ////      , "IsIncludedInstructionsDefaultPageSingleStateDomain"
    ////      , "IsTextInstructionsDefaultPageSingleStateDomain"
    ////      , "InstructionsDefaultPageSingleStateDomain"
    ////      //, "Default"
    ////      );

    ////    ReLoad_Entire_Form_To_View_Changes();

    ////    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain);
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    //// ----------------- Update Default Design Buttons -----------------
    //protected void Button_Save_Default_Template_Election_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(
    //       TextBoxMainContentDefaultPageSingleStateDomain);

    //    db.Throw_Exception_TextBox_Empty(
    //       TextBoxMainContentDefaultPageSingleStateDomain);

    //    switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //    {
    //      case "Default":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionNone"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //      case "General":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionGeneral"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //      case "OffYear":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionOffYear"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //      case "Special":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionSpecial"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //      case "Primary":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainElectionPrimary"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //      case "OnlyFederalStatewide":
    //        db.MasterDesign_Update_Str(
    //          "MainContentDefaultPageSingleStateDomainContestsFederalStatewide"
    //        , TextBoxMainContentDefaultPageSingleStateDomain.Text.Trim()
    //        );
    //        break;
    //    }

    //    db.States_Update_HomePageUpdated();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////protected void ButtonSubmitDefaultInstructionsDefaultPageSingleStateDomain_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    db.Submit_Textbox_Default(
    ////       RadioButtonListIsTextInstructionsDefaultPageSingleStateDomain
    ////      ,  TextBoxInstructionsDefaultPageSingleStateDomain
    ////      , "IsIncludedInstructionsDefaultPageSingleStateDomain"
    ////      , "IsTextInstructionsDefaultPageSingleStateDomain"
    ////      , "InstructionsDefaultPageSingleStateDomain"
    ////      //, "Default"
    ////      );

    ////    db.States_Update_HomePageUpdated();

    ////    ReLoad_Entire_Form_To_View_Changes();

    ////    Msg.Text = db.Msg4ButtonDefaultSubmit();
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Msg.Text = db.Fail(ex.Message);
    ////    db.Log_Error_Admin(ex);
    ////  }
    ////}

    ////-------------- Style Sheet Button ---------------------

    //protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Default.css");

    //    db.States_Update_HomePageUpdated();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    ////HyperLinkSamplePage.NavigateUrl = db.Url_Default(db.StateCode_SamplePage());
    //    //HyperLinkSamplePage.NavigateUrl =
    //    //  UrlManager.SiteUri.ToString();

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

    //protected void RadioButtonList_Main_Content_Single_State_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Load_MainContent_TextBox();

    //    ReLoad_Entire_Form_To_View_Changes();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
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

    //protected void Button_Get_Default_Template_Title_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Button_Save_Default_Template_Title_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //         TextBoxTitleTagDefaultPageSingleStateDomain
    //      , Label_Title_Template_Default
    //      , "IsIncludedTitleTagDefaultPageSingleStateDomain"
    //      , "TitleTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
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


    //protected void Button_Save_Default_Template_Description_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //         TextBoxMetaDescriptionTagDefaultPageSingleStateDomain
    //      //,  Label_Description_Template_Default
    //      //, "IsIncludedMetaDescriptionTagDefaultPageSingleStateDomain"
    //      , "MetaDescriptionTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
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

    //protected void Button_Save_Default_Template_Keywords_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //         TextBoxMetaKeywordsTagDefaultPageSingleStateDomain
    //      //,  Label_Keywords_Template_Default
    //      //, "IsIncludedMetaKeywordsTagDefaultPageSingleStateDomain"
    //      , "MetaKeywordsTagDefaultPageSingleStateDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated();

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Get_Default_Template_Election_Click(object sender, EventArgs e)
    //{

    //}

    //protected void Load_MainContent_TextBox()
    //{
    //  switch (RadioButtonList_Main_Content_Single_State.SelectedValue)
    //  {
    //    case "Default":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionNone");
    //      break;
    //    case "General":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionGeneral");
    //      break;
    //    case "OffYear":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionOffYear");
    //      break;
    //    case "Special":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionSpecial");
    //      break;
    //    case "Primary":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainElectionPrimary");
    //      break;
    //    case "OnlyFederalStatewide":
    //      TextBoxMainContentDefaultPageSingleStateDomain.Text =
    //        db.MasterDesign_Str("MainContentDefaultPageSingleStateDomainContestsFederalStatewide");
    //      break;
    //  }
    //}



  }
}
