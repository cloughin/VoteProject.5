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
  public partial class DesignIssuePage : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsIssue, "IsTextInstructionsIssuePage");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsIssueIssueListAnswers, "IsTextInstructionsIssuePageIssueListAnswers");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsIssueIssueListNoAnswers, "IsTextInstructionsIssuePageIssueListNoAnswers");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsIssue
    //    , "InstructionsIssuePage"
    //    , "IsTextInstructionsIssuePage"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsIssueIssueListAnswers
    //    , "InstructionsIssuePageIssueListAnswers"
    //    , "IsTextInstructionsIssuePageIssueListAnswers"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsIssueIssueListNoAnswers
    //    , "InstructionsIssuePageIssueListNoAnswers"
    //    , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //    );

    //  #endregion

    //  #region Custom Content in Texboxes
    //  TextBoxInstructionsIssue.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsIssuePage");
    //  TextBoxInstructionsIssueIssueListAnswers.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsIssuePageIssueListAnswers");
    //  TextBoxInstructionsIssueIssueListNoAnswers.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsIssuePageIssueListNoAnswers");
    //  #endregion Custom Content in Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Issue.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Issue.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Issue.css");
    //  #endregion Style Sheets
    //}

    //// ---------- Radion Buttons for Text or HTML -----------
    //protected void RadioButtonListIsTextInstructionsIssue_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssue
    //      ,  TextBoxInstructionsIssue
    //      ,  LabelInstructionsIssue
    //      , string.Empty
    //      , "IsTextInstructionsIssuePage"
    //      , "InstructionsIssuePage"
    //      //, "Issue"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIssue);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsIssueIssueListAnswers_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListAnswers
    //      ,  TextBoxInstructionsIssueIssueListAnswers
    //      ,  LabelInstructionsIssueIssueListAnswers
    //      , string.Empty
    //      , "IsTextInstructionsIssuePageIssueListAnswers"
    //      , "InstructionsIssuePageIssueListAnswers"
    //      //, "Issue"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIssueIssueListAnswers);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsIssueIssueListNoAnswers_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListNoAnswers
    //      ,  TextBoxInstructionsIssueIssueListNoAnswers
    //      ,  LabelInstructionsIssueIssueListNoAnswers
    //      , string.Empty
    //      , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //      , "InstructionsIssuePageIssueListNoAnswers"
    //      //, "Issue"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIssueIssueListNoAnswers);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Get Default Design Buttons ---------------------

    //protected void ButtonGetDefaultInstructionsIssue_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsIssue
    //      , "InstructionsIssuePage");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsIssueIssueListAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsIssueIssueListAnswers
    //      , "InstructionsIssuePageIssueListAnswers");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsIssueIssueListNoAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsIssueIssueListNoAnswers
    //      , "InstructionsIssuePageIssueListNoAnswers");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}
    ////-------------- Clear Custom Design Buttons ---------------------
    //protected void ButtonClearCustomInstructionsIssue_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssue
    //      ,  TextBoxInstructionsIssue
    //      ,  LabelInstructionsIssue
    //      , "IsTextInstructionsIssuePage"
    //      , "InstructionsIssuePage"
    //      //, "Issue"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsIssueIssueListAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListAnswers
    //      ,  TextBoxInstructionsIssueIssueListAnswers
    //      ,  LabelInstructionsIssueIssueListAnswers
    //      , "IsTextInstructionsIssuePageIssueListAnswers"
    //      , "InstructionsIssuePageIssueListAnswers"
    //      //, "Issue"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsIssueIssueListNoAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListNoAnswers
    //      ,  TextBoxInstructionsIssueIssueListNoAnswers
    //      ,  LabelInstructionsIssueIssueListNoAnswers
    //      , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //      , "InstructionsIssuePageIssueListNoAnswers"
    //      //, "Issue"
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
    //protected void ButtonSubmitCustomInstructionsIssue_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssue
    //      ,  TextBoxInstructionsIssue
    //      ,  LabelInstructionsIssue
    //      , string.Empty
    //      , "IsTextInstructionsIssuePage"
    //      , "InstructionsIssuePage"
    //      //, "Issue"
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

    //protected void ButtonSubmitCustomInstructionsIssueIssueListAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListAnswers
    //      ,  TextBoxInstructionsIssueIssueListAnswers
    //      ,  LabelInstructionsIssueIssueListAnswers
    //      , string.Empty
    //      , "IsTextInstructionsIssuePageIssueListAnswers"
    //      , "InstructionsIssuePageIssueListAnswers"
    //      //, "Issue"
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

    //protected void ButtonSubmitCustomInstructionsIssueIssueListNoAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsIssueIssueListNoAnswers
    //      ,  TextBoxInstructionsIssueIssueListNoAnswers
    //      ,  LabelInstructionsIssueIssueListNoAnswers
    //      , string.Empty
    //      , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //      , "InstructionsIssuePageIssueListNoAnswers"
    //      //, "Issue"
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Issue.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Issue.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Issue.css"));

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

    //    try
    //    {
    //      LabelDesignCode.Text = db.Domain_DesignCode_This();

    //      //HyperLinkSamplePage.NavigateUrl = db.Url_Issue("PA", "BUSEnvironment", "USPresident", "20081104GPA000000ALL");
    //      HyperLinkSamplePage.NavigateUrl = db.Url_SamplePage(
    //        //db.User_Security()
    //        //db.User_Security()
    //      db.Domain_DesignCode_This()
    //      , db.Url_Issue(
    //          "20081104G" + db.StateCode_SamplePage() + "000000ALL"
    //          , "USPresident"
    //          , "BUSEnvironment"
    //          ,db.StateCode_SamplePage()
    //          )
    //          );

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
