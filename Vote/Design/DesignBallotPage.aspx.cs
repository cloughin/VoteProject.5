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
  public partial class DesignBallotPage : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsUpcomingElectionBallot, "IsTextInstructionsUpcomingElectionBallotPage");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsPreviousElectionBallotPage, "IsTextInstructionsPreviousElectionBallotPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsUpcomingElectionBallot
    //    , "InstructionsUpcomingElectionBallotPage"
    //    , "IsTextInstructionsUpcomingElectionBallotPage"
    //    );

    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelInstructionsPreviousElectionBallotPage
    //    , "InstructionsPreviousElectionBallotPage"
    //    , "IsTextInstructionsPreviousElectionBallotPage"
    //    );

    //  #endregion


    //  #region Custom Content in Texboxes
    //  TextBoxInstructionsUpcomingElectionBallot.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsUpcomingElectionBallotPage");
    //  TextBoxInstructionsPreviousElectionBallotPage.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsPreviousElectionBallotPage");
    //  #endregion Custom Content in Texboxes
    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Ballot.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Ballot.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Ballot.css");
    //  #endregion Style Sheets
    //}

    //// ---------- Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsUpcomingElectionBallot_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsUpcomingElectionBallot
    //      ,  TextBoxInstructionsUpcomingElectionBallot
    //      ,  LabelInstructionsUpcomingElectionBallot
    //      , string.Empty
    //      , "IsTextInstructionsUpcomingElectionBallotPage"
    //      , "InstructionsUpcomingElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Upcoming(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
    //      );

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsUpcomingElectionBallot);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextInstructionsPreviousElectionBallotPage_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsPreviousElectionBallotPage
    //      ,  TextBoxInstructionsPreviousElectionBallotPage
    //      ,  LabelInstructionsPreviousElectionBallotPage
    //      , "IsIncludedInstructionsPreviousElectionBallotPage"
    //      , "IsTextInstructionsPreviousElectionBallotPage"
    //      , "InstructionsPreviousElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Previous(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
    //      );

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsPreviousElectionBallotPage);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Get Default Design Buttons ---------------------

    //protected void ButtonGetDefaultInstructionsUpcomingElectionBallot_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsUpcomingElectionBallot
    //      , "InstructionsUpcomingElectionBallotPage");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultInstructionsPreviousElectionBallotPage_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxInstructionsPreviousElectionBallotPage
    //      , "InstructionsPreviousElectionBallotPage");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Clear Custom Design Buttons ---------------------

    //protected void ButtonClearCustomInstructionsUpcomingElectionBallot_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsUpcomingElectionBallot
    //      ,  TextBoxInstructionsUpcomingElectionBallot
    //      ,  LabelInstructionsUpcomingElectionBallot
    //      , "IsTextInstructionsUpcomingElectionBallotPage"
    //      , "InstructionsUpcomingElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Upcoming(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomInstructionsPreviousElectionBallotPage_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsPreviousElectionBallotPage
    //      ,  TextBoxInstructionsPreviousElectionBallotPage
    //      ,  LabelInstructionsPreviousElectionBallotPage
    //      , "IsTextInstructionsPreviousElectionBallotPage"
    //      , "InstructionsPreviousElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Previous(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
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

    //protected void ButtonSubmitCustomInstructionsUpcomingElectionBallot_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsUpcomingElectionBallot
    //      ,  TextBoxInstructionsUpcomingElectionBallot
    //      ,  LabelInstructionsUpcomingElectionBallot
    //      , string.Empty
    //      , "IsTextInstructionsUpcomingElectionBallotPage"
    //      , "InstructionsUpcomingElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Upcoming(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitCustomInstructionsPreviousElectionBallotPage_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextInstructionsPreviousElectionBallotPage
    //      ,  TextBoxInstructionsPreviousElectionBallotPage
    //      ,  LabelInstructionsPreviousElectionBallotPage
    //      , "IsIncludedInstructionsPreviousElectionBallotPage"
    //      , "IsTextInstructionsPreviousElectionBallotPage"
    //      , "InstructionsPreviousElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Previous(
    //      db.Domain_DesignCode_This()
    //      //, db.DomanDataCode4DomainDesignCode(db.Domain_DesignCode_This())
    //      , db.Domain_DataCode_This()
    //      );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Ballot.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Ballot.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"Ballot.css"));

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

    //      HyperLinkSamplePage.NavigateUrl = 
    //        db.Url_SamplePage(db.Domain_DesignCode_This(),
    //          UrlManager.GetBallotPageUri(db.StateCode_SamplePage(), 
    //            "20081104GCA000000ALL", "053", "039", "076", "073"))
    //        .ToString();

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
