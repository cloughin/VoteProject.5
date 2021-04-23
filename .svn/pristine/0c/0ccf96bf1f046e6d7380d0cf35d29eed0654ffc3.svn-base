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

namespace Vote.Admin
{
  public partial class DesignDefaultPageNoState : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
    //    , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection");

    //  //----------------- MainContent After Selection
    //  db.RadioButtonResetIsTextCustom(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
    //    , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection");

    //  db.RadioButtonResetIsTextCustom(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
    //    , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection");

    //  //-----------------Instructions After Selection
    //  db.RadioButtonResetIsTextCustom(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
    //    , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelTitleTagDefaultPageAllStatesDomain
    //    , "TitleTagDefaultPageAllStatesDomain");

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelMetaDescriptionTagDefaultPageAllStatesDomain
    //    , "MetaDescriptionTagDefaultPageAllStatesDomain");

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelMetaKeywordsTagDefaultPageAllStatesDomain
    //    , "MetaKeywordsTagDefaultPageAllStatesDomain");

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
    //    , "MainContentDefaultPageAllStatesDomainBeforeSelection"
    //    , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
    //    );

    //  //----------------- MainContent After Selection
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
    //    , "MainContentDefaultPageAllStatesDomainAfterSelection"
    //    , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
    //    );


    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsDefaultPageAllStatesDomainBeforeSelection
    //    , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
    //    , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsDefaultPageAllStatesDomainAfterSelection
    //    , "InstructionsDefaultPageAllStatesDomainAfterSelection"
    //    , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
    //    );
    //  #endregion

    //  #region Custom Content in Texboxes
    //  TextBoxTitleTagDefaultPageAllStatesDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "TitleTagDefaultPageAllStatesDomain");
    //  TextBoxMetaDescriptionTagDefaultPageAllStatesDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MetaDescriptionTagDefaultPageAllStatesDomain");
    //  TextBoxMetaKeywordsTagDefaultPageAllStatesDomain.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MetaKeywordsTagDefaultPageAllStatesDomain");

    //  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MainContentDefaultPageAllStatesDomainBeforeSelection");

    //  //----------------- MainContent After Selection
    //  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "MainContentDefaultPageAllStatesDomainAfterSelection");

    //  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "InstructionsDefaultPageAllStatesDomainBeforeSelection");
    //  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection.Text =
    //    db.DomainDesigns_Str(db.Domain_DesignCode_This()
    //    , "InstructionsDefaultPageAllStatesDomainAfterSelection");
    //  #endregion Custom Content in Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Default.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Default.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Default.css");
    //  #endregion Style Sheets
    //}

    //// ---------- Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
    //      , "IsIncludedMainContentDefaultPageAllStatesDomain"
    //      , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
    //      , "MainContentDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////----------------- MainContent After Selection 
    //protected void RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
    //      , "IsIncludedMainContentDefaultPageAllStatesDomain"
    //      , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
    //      , "MainContentDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}


    //protected void RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      , "IsIncludedInstructionsDefaultPageAllStatesDomain"
    //      , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
    //      , "IsIncludedInstructionsDefaultPageAllStatesDomain"
    //      , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
    //      , "InstructionsDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Get Default Design Buttons ---------------------
    //protected void ButtonGetDefaultTitleTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxTitleTagDefaultPageAllStatesDomain
    //      , "TitleTagDefaultPageAllStatesDomain");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultMetaDescriptionTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxMetaDescriptionTagDefaultPageAllStatesDomain
    //      , "MetaDescriptionTagDefaultPageAllStatesDomain");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultMetaKeywordsTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxMetaKeywordsTagDefaultPageAllStatesDomain
    //      , "MetaKeywordsTagDefaultPageAllStatesDomain");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultMainContentDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
    //      , "MainContentDefaultPageAllStatesDomainBeforeSelection");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// --- Main contnet After 
    //protected void ButtonGetDefaultMainContentDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
    //      , "MainContentDefaultPageAllStatesDomainAfterSelection");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}


    //protected void ButtonGetDefaultInstructionsDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      , "InstructionsDefaultPageAllStatesDomainBeforeSelection");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
    //      , "InstructionsDefaultPageAllStatesDomainAfterSelection");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Clear Custom Design Buttons ---------------------
    //protected void ButtonClearCustomTitleTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxTitleTagDefaultPageAllStatesDomain
    //      ,  LabelTitleTagDefaultPageAllStatesDomain
    //      , "TitleTagDefaultPageAllStatesDomain"
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

    //protected void ButtonClearCustomMetaDescriptionTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxMetaDescriptionTagDefaultPageAllStatesDomain
    //      ,  LabelMetaDescriptionTagDefaultPageAllStatesDomain
    //      , "MetaDescriptionTagDefaultPageAllStatesDomain"
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

    //protected void ButtonClearCustomMetaKeywordsTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxMetaKeywordsTagDefaultPageAllStatesDomain
    //      ,  LabelMetaKeywordsTagDefaultPageAllStatesDomain
    //      , "MetaKeywordsTagDefaultPageAllStatesDomain"
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

    //protected void ButtonClearCustomMainContentDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
    //      , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
    //      , "MainContentDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
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

    ////-------Main Content After 
    //protected void ButtonClearCustomMainContentDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
    //      , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
    //      , "MainContentDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
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


    //protected void ButtonClearCustomInstructionsDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
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

    //protected void ButtonClearCustomInstructionsDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
    //      ,  LabelInstructionsDefaultPageAllStatesDomainAfterSelection
    //      , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
    //      , "InstructionsDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
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
    ////-------------- Submit Custom Design Buttons ---------------------
    //protected void ButtonSubmitCustomTitleTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxTitleTagDefaultPageAllStatesDomain
    //      ,  LabelTitleTagDefaultPageAllStatesDomain
    //      , "IsIncludedTitleTagDefaultPageAllStatesDomain"
    //      , "TitleTagDefaultPageAllStatesDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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

    //protected void ButtonSubmitCustomMetaDescriptionTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxMetaDescriptionTagDefaultPageAllStatesDomain
    //      ,  LabelMetaDescriptionTagDefaultPageAllStatesDomain
    //      , "IsIncludedMetaDescriptionTagDefaultPageAllStatesDomain"
    //      , "MetaDescriptionTagDefaultPageAllStatesDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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

    //protected void ButtonSubmitCustomMetaKeywordsTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxMetaKeywordsTagDefaultPageAllStatesDomain
    //      ,  LabelMetaKeywordsTagDefaultPageAllStatesDomain
    //      , "IsIncludedMetaKeywordsTagDefaultPageAllStatesDomain"
    //      , "MetaKeywordsTagDefaultPageAllStatesDomain"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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

    //protected void ButtonSubmitCustomMainContentDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
    //      , "IsIncludedMainContentDefaultPageAllStatesDomain"
    //      , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
    //      , "MainContentDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      //"US"
    //      db.Domain_DesignCode_This());
    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////---- Main Content AFter 
    //protected void ButtonSubmitCustomMainContentDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
    //      ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
    //      , "IsIncludedMainContentDefaultPageAllStatesDomain"
    //      , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
    //      , "MainContentDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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


    //protected void ButtonSubmitCustomInstructionsDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
    //      , "IsIncludedInstructionsDefaultPageAllStatesDomain"
    //      , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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

    //protected void ButtonSubmitCustomInstructionsDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
    //      ,  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
    //      ,  LabelInstructionsDefaultPageAllStatesDomainAfterSelection
    //      , "IsIncludedInstructionsDefaultPageAllStatesDomain"
    //      , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
    //      , "InstructionsDefaultPageAllStatesDomainAfterSelection"
    //      //, "Default"
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css")//css\Designs\VOTE-USA\Default.css
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css")//css\Designs\VOTE-USA\Default.css
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4ButtonUploadStyleSheet();
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Default.css")//css\Designs\VOTE-USA\Default.css
    //      );

    //    db.States_Update_HomePageUpdated_Design(
    //      db.Domain_DesignCode_This()
    //      );

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

    //      //HyperLinkHomePage.NavigateUrl = db.Url_Default();
    //      HyperLinkSamplePage.NavigateUrl = db.Url_SamplePage(
    //        //db.User_Security()
    //        //db.User_Security()
    //      db.Domain_DesignCode_This()
    //      //, db.Url_Default(db.StateCode_SamplePage()));
    //      , UrlManager.SiteUri).ToString();

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
