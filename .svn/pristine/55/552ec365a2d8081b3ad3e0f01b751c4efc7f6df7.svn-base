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
  public partial class DesignDefaultsIssuePage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsIssueIssueListAnswers, "IsTextInstructionsIssuePageIssueListAnswers");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsIssueIssueListNoAnswers, "IsTextInstructionsIssuePageIssueListNoAnswers");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsIssueIssueListReport, "IsTextInstructionsIssuePageIssueListReport");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //     LabelInstructionsIssueIssueListAnswers
    //    , "InstructionsIssuePageIssueListAnswers"
    //    , "IsTextInstructionsIssuePageIssueListAnswers"
    //    );

    //  db.Label_Default_Set(
    //     LabelInstructionsIssueIssueListNoAnswers
    //    , "InstructionsIssuePageIssueListNoAnswers"
    //    , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //    );

    //  db.Label_Default_Set(
    //     LabelInstructionsIssueIssueListReport
    //    , "InstructionsIssuePageIssueListReport"
    //    , "IsTextInstructionsIssuePageIssueListReport"
    //    );

    //  db.Label_Default_Set(
    //     LabelInstructionsIssue
    //    , "InstructionsIssuePage"
    //    , "IsTextInstructionsIssuePage"
    //    );

      
    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("InstructionsIssuePageIssueListAnswers") != string.Empty)
    //    TextBoxInstructionsIssueIssueListAnswers.Text = db.MasterDesign_Str("InstructionsIssuePageIssueListAnswers");

    //  if (db.MasterDesign_Str("InstructionsIssuePageIssueListNoAnswers") != string.Empty)
    //    TextBoxInstructionsIssueIssueListNoAnswers.Text = db.MasterDesign_Str("InstructionsIssuePageIssueListNoAnswers");

    //  if (db.MasterDesign_Str("InstructionsIssuePageIssueListReport") != string.Empty)
    //    TextBoxInstructionsIssueIssueListReport.Text = db.MasterDesign_Str("InstructionsIssuePageIssueListReport");

    //  if (db.MasterDesign_Str("InstructionsIssuePage") != string.Empty)
    //    TextBoxInstructionsIssue.Text = db.MasterDesign_Str("InstructionsIssuePage");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Issue.css");
    //}
    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsIssueIssueListAnswers_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListAnswers
    //      ,  TextBoxInstructionsIssueIssueListAnswers
    //      ,  LabelInstructionsIssueIssueListAnswers
    //      , string.Empty //IsInclude = true
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
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListNoAnswers
    //      , TextBoxInstructionsIssueIssueListNoAnswers
    //      , LabelInstructionsIssueIssueListNoAnswers
    //      , string.Empty //IsInclude = true
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

    //protected void RadioButtonListIsTextInstructionsIssueIssueListReport_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListReport
    //      , TextBoxInstructionsIssueIssueListReport
    //      , LabelInstructionsIssueIssueListReport
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIssuePageIssueListReport"
    //      , "InstructionsIssuePageIssueListReport"
    //      //, "Issue"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIssueIssueListReport);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsIssue_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssue
    //      , TextBoxInstructionsIssue
    //      , LabelInstructionsIssue
    //      , string.Empty //IsInclude = true
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

    //// ----------------- Update Default Design Button -----------------

    //protected void ButtonSubmitDefaultInstructionsIssueIssueListAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListAnswers
    //      , TextBoxInstructionsIssueIssueListAnswers
    //      , LabelInstructionsIssueIssueListAnswers
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIssuePageIssueListAnswers"
    //      , "InstructionsIssuePageIssueListAnswers"
    //      //, "Issue"
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

    //protected void ButtonSubmitDefaultInstructionsIssueIssueListNoAnswers_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListNoAnswers
    //      , TextBoxInstructionsIssueIssueListNoAnswers
    //      , LabelInstructionsIssueIssueListNoAnswers
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIssuePageIssueListNoAnswers"
    //      , "InstructionsIssuePageIssueListNoAnswers"
    //      //, "Issue"
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

    //protected void ButtonSubmitDefaultInstructionsIssueIssueListReport_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssueIssueListReport
    //      ,  TextBoxInstructionsIssueIssueListReport
    //      ,  LabelInstructionsIssueIssueListReport
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIssuePageIssueListReport"
    //      , "InstructionsIssuePageIssueListReport"
    //      //, "Issue"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsIssueIssueListReport);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitDefaultInstructionsIssue_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsIssue
    //      ,  TextBoxInstructionsIssue
    //      ,  LabelInstructionsIssue
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsIssuePage"
    //      , "InstructionsIssuePage"
    //      //, "Issue"
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
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Issue.css");

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

    //      HyperLinkSamplePage.NavigateUrl = db.Url_Issue(
    //        "20081104GPA000000ALL"
    //        , "USPresident"
    //        ,"BUSEnvironment"
    //        );
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
