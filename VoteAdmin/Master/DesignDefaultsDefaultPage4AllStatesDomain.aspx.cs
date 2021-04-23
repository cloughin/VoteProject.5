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
  public partial class Design4DefaultNoStateSelected : VotePage
  {
  //  private void ReLoad_Entire_Form_To_View_Changes()
  //  {
  //    #region Text / HTML Radio Buttons
  //    db.RadioButtonResetIsTextDefault(
  //       RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
  //      , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection");

  //    db.RadioButtonResetIsTextDefault(
  //       RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
  //      , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection");

  //    db.RadioButtonResetIsTextDefault(
  //       RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
  //      , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection");
  //    //----
  //    db.RadioButtonResetIsTextDefault(
  //       RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
  //      , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection");
  //    #endregion

  //    #region Current Content
  //    db.Label_Default_Set(
  //        LabelTitleTagDefaultPageAllStatesDomain
  //       , "TitleTagDefaultPageAllStatesDomain");

  //    db.Label_Default_Set(
  //        LabelMetaDescriptionTagDefaultPageAllStatesDomain
  //       , "MetaDescriptionTagDefaultPageAllStatesDomain");

  //    db.Label_Default_Set(
  //        LabelMetaKeywordsTagDefaultPageAllStatesDomain
  //       , "MetaKeywordsTagDefaultPageAllStatesDomain");

  //    db.Label_Default_Set(
  //        LabelMainContentDefaultPageAllStatesDomainBeforeSelection
  //       , "MainContentDefaultPageAllStatesDomainBeforeSelection"
  //       , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
  //       );

  //    db.Label_Default_Set(
  //        LabelMainContentDefaultPageAllStatesDomainAfterSelection
  //       , "MainContentDefaultPageAllStatesDomainAfterSelection"
  //       , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
  //       );

  //    db.Label_Default_Set(
  //        LabelInstructionsDefaultPageAllStatesDomainBeforeSelection
  //       , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
  //       , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
  //       );


  //    db.Label_Default_Set(
  //        LabelInstructionsDefaultPageAllStatesDomainAfterSelection
  //       , "InstructionsDefaultPageAllStatesDomainAfterSelection"
  //       , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
  //       );

  //    #endregion

  //    #region Current Content in Textboxes

  //    if (db.MasterDesign_Str("TitleTagDefaultPageAllStatesDomain") != string.Empty)
  //      TextBoxTitleTagDefaultPageAllStatesDomain.Text = 
  //        db.MasterDesign_Str("TitleTagDefaultPageAllStatesDomain");

  //    if (db.MasterDesign_Str("MetaDescriptionTagDefaultPageAllStatesDomain") != string.Empty)
  //      TextBoxMetaDescriptionTagDefaultPageAllStatesDomain.Text = 
  //        db.MasterDesign_Str("MetaDescriptionTagDefaultPageAllStatesDomain");

  //    if (db.MasterDesign_Str("MetaKeywordsTagDefaultPageAllStatesDomain") != string.Empty)
  //      TextBoxMetaKeywordsTagDefaultPageAllStatesDomain.Text = 
  //        db.MasterDesign_Str("MetaKeywordsTagDefaultPageAllStatesDomain");

  //    if (db.MasterDesign_Str("MainContentDefaultPageAllStatesDomainBeforeSelection") != string.Empty)
  //      TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection.Text =
  //        db.MasterDesign_Str("MainContentDefaultPageAllStatesDomainBeforeSelection");

  //    if (db.MasterDesign_Str("MainContentDefaultPageAllStatesDomainAfterSelection") != string.Empty)
  //      TextBoxMainContentDefaultPageAllStatesDomainAfterSelection.Text =
  //        db.MasterDesign_Str("MainContentDefaultPageAllStatesDomainAfterSelection");

  //    if (db.MasterDesign_Str("InstructionsDefaultPageAllStatesDomainBeforeSelection") != string.Empty)
  //      TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection.Text =
  //        db.MasterDesign_Str("InstructionsDefaultPageAllStatesDomainBeforeSelection");
  //    //---

  //    if (db.MasterDesign_Str("InstructionsDefaultPageAllStatesDomainAfterSelection") != string.Empty)
  //      TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection.Text =
  //        db.MasterDesign_Str("InstructionsDefaultPageAllStatesDomainAfterSelection");
  //    #endregion Current Content in Textboxes

  //    db.StyleSheetGet(TextBoxStyleSheet, @"css\Default.css");
  //  }

  //  // ------------------ Text or HTML Radion Buttons
  //  protected void RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection_SelectedIndexChanged(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
  //        ,  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
  //        ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
  //        , "IsIncludedMainContentDefaultPageAllStatesDomain"
  //        , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
  //        , "MainContentDefaultPageAllStatesDomainBeforeSelection"
  //        //, "Default"
  //        );

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection);
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection_SelectedIndexChanged(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
  //        ,  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
  //        ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
  //        , "IsIncludedMainContentDefaultPageAllStatesDomain"
  //        , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
  //        , "MainContentDefaultPageAllStatesDomainAfterSelection"
  //        //, "Default"
  //        );

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection);
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection_SelectedIndexChanged(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
  //        ,  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
  //        , "IsIncludedInstructionsDefaultPageAllStatesDomain"
  //        , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
  //        , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
  //        //, "Default"
  //        );

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection);
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  //--------------
  //  protected void RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection_SelectedIndexChanged(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
  //        ,  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
  //        , "IsIncludedInstructionsDefaultPageAllStatesDomain"
  //        , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
  //        , "InstructionsDefaultPageAllStatesDomainAfterSelection"
  //        //, "Default"
  //        );

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection);
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }
  //  // ----------------- Update Default Design Buttons -----------------

  //  protected void ButtonSubmitDefaultTitleTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //           TextBoxTitleTagDefaultPageAllStatesDomain
  //        ,  LabelTitleTagDefaultPageAllStatesDomain
  //        , "IsIncludedTitleTagDefaultPageAllStatesDomain"
  //        , "TitleTagDefaultPageAllStatesDomain"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void ButtonSubmitDefaultMetaDescriptionTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //           TextBoxMetaDescriptionTagDefaultPageAllStatesDomain
  //        ,  LabelMetaDescriptionTagDefaultPageAllStatesDomain
  //        , "IsIncludedMetaDescriptionTagDefaultPageAllStatesDomain"
  //        , "MetaDescriptionTagDefaultPageAllStatesDomain"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void ButtonSubmitDefaultMetaKeywordsTagDefaultPageAllStatesDomain_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //           TextBoxMetaKeywordsTagDefaultPageAllStatesDomain
  //        ,  LabelMetaKeywordsTagDefaultPageAllStatesDomain
  //        , "IsIncludedMetaKeywordsTagDefaultPageAllStatesDomain"
  //        , "MetaKeywordsTagDefaultPageAllStatesDomain"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void ButtonSubmitDefaultMainContentDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextMainContentDefaultPageAllStatesDomainBeforeSelection
  //        ,  TextBoxMainContentDefaultPageAllStatesDomainBeforeSelection
  //        ,  LabelMainContentDefaultPageAllStatesDomainBeforeSelection
  //        , "IsIncludedMainContentDefaultPageAllStatesDomain"
  //        , "IsTextMainContentDefaultPageAllStatesDomainBeforeSelection"
  //        , "MainContentDefaultPageAllStatesDomainBeforeSelection"
  //        //, "Default"
  //        );
  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void ButtonSubmitDefaultMainContentDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextMainContentDefaultPageAllStatesDomainAfterSelection
  //        ,  TextBoxMainContentDefaultPageAllStatesDomainAfterSelection
  //        ,  LabelMainContentDefaultPageAllStatesDomainAfterSelection
  //        , "IsIncludedMainContentDefaultPageAllStatesDomain"
  //        , "IsTextMainContentDefaultPageAllStatesDomainAfterSelection"
  //        , "MainContentDefaultPageAllStatesDomainAfterSelection"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  protected void ButtonSubmitDefaultInstructionsDefaultPageAllStatesDomainBeforeSelection_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainBeforeSelection
  //        ,  TextBoxInstructionsDefaultPageAllStatesDomainBeforeSelection
  //        , "IsIncludedInstructionsDefaultPageAllStatesDomain"
  //        , "IsTextInstructionsDefaultPageAllStatesDomainBeforeSelection"
  //        , "InstructionsDefaultPageAllStatesDomainBeforeSelection"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }

  //  //--------------
  //  protected void ButtonSubmitDefaultInstructionsDefaultPageAllStatesDomainAfterSelection_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.Submit_Textbox_Default(
  //         RadioButtonListIsTextInstructionsDefaultPageAllStatesDomainAfterSelection
  //        ,  TextBoxInstructionsDefaultPageAllStatesDomainAfterSelection
  //        , "IsIncludedInstructionsDefaultPageAllStatesDomain"
  //        , "IsTextInstructionsDefaultPageAllStatesDomainAfterSelection"
  //        , "InstructionsDefaultPageAllStatesDomainAfterSelection"
  //        //, "Default"
  //        );

  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonDefaultSubmit();
  //    }
  //    catch (Exception ex)
  //    {
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //    }
  //  }
  //  //-------------- Style Sheet Button ---------------------

  //  protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
  //  {
  //    try
  //    {
  //      db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Default.css");


  //      db.States_Update_HomePageUpdated("US");

  //      ReLoad_Entire_Form_To_View_Changes();

  //      Msg.Text = db.Msg4ButtonUpdateStyleSheet();
  //    }
  //    catch (Exception ex)
  //    {
  //      #region
  //      Msg.Text = db.Fail(ex.Message);
  //      db.Log_Error_Admin(ex);
  //      #endregion
  //    }
  //  }

  //  // ----------------- Page Load -----------------

  //  protected void Page_Load(object sender, EventArgs e)
  //  {
  //    if (!IsPostBack)
  //    {
  //      if (!SecurePage.IsMasterUser)
  //        SecurePage.HandleSecurityException();

  //      try
  //      {
  //        ReLoad_Entire_Form_To_View_Changes();

  //        HyperLinkHomePage.NavigateUrl = UrlManager.SiteUri.ToString();
  //      }
  //      catch (Exception ex)
  //      {
  //        #region
  //        Msg.Text = db.Fail(ex.Message);
  //        db.Log_Error_Admin(ex);
  //        #endregion
  //      }
  //    }
  //  }

  }
}
