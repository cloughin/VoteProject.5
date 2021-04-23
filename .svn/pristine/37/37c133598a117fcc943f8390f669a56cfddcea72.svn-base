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
  public partial class DesignDefaultsBallotPage : VotePage
  {
    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsUpcomingElectionBallot, "IsTextInstructionsUpcomingElectionBallotPage");
    //  db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsPreviousElectionBallotPage, "IsTextInstructionsPreviousElectionBallotPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Default_Set(
    //      LabelInstructionsUpcomingElectionBallot
    //     , "InstructionsUpcomingElectionBallotPage"
    //     , "IsTextInstructionsUpcomingElectionBallotPage"
    //     );

    //  db.Label_Default_Set(
    //      LabelInstructionsPreviousElectionBallotPage
    //     , "InstructionsPreviousElectionBallotPage"
    //     , "IsTextInstructionsPreviousElectionBallotPage"
    //     );

    //  #endregion

    //  #region Current Content in Textboxes

    //  if (db.MasterDesign_Str("InstructionsUpcomingElectionBallotPage") != string.Empty)
    //    TextBoxInstructionsUpcomingElectionBallot.Text = db.MasterDesign_Str("InstructionsUpcomingElectionBallotPage");

    //  if (db.MasterDesign_Str("InstructionsPreviousElectionBallotPage") != string.Empty)
    //    TextBoxInstructionsPreviousElectionBallotPage.Text = db.MasterDesign_Str("InstructionsPreviousElectionBallotPage");
    //  #endregion Current Content in Textboxes

    //  db.StyleSheetGet(TextBoxStyleSheet, @"css\Ballot.css");
    //}

    //// ------------------ Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextInstructionsUpcomingElectionBallot_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsUpcomingElectionBallot
    //      ,  TextBoxInstructionsUpcomingElectionBallot
    //      ,  LabelInstructionsUpcomingElectionBallot
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsUpcomingElectionBallotPage"
    //      , "InstructionsUpcomingElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Upcoming("InstructionsUpcomingElectionBallotPage");

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
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsPreviousElectionBallotPage
    //      ,  TextBoxInstructionsPreviousElectionBallotPage
    //      ,  LabelInstructionsPreviousElectionBallotPage
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsPreviousElectionBallotPage"
    //      , "InstructionsPreviousElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Previous("InstructionsPreviousElectionBallotPage");

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsPreviousElectionBallotPage);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ----------------- Update Default Design Buttons -----------------
    //protected void ButtonSubmitDefaultInstructionsUpcomingElectionBallot_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsUpcomingElectionBallot
    //      ,  TextBoxInstructionsUpcomingElectionBallot
    //      ,  LabelInstructionsUpcomingElectionBallot
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsUpcomingElectionBallotPage"
    //      , "InstructionsUpcomingElectionBallotPage"
    //      //, "Ballot"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Upcoming("InstructionsUpcomingElectionBallotPage");

    //    Msg.Text = db.Msg4ButtonDefaultSubmit();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSubmitDefaultInstructionsPreviousElectionBallotPage_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Default(
    //       RadioButtonListIsTextInstructionsPreviousElectionBallotPage
    //      ,  TextBoxInstructionsPreviousElectionBallotPage
    //      ,  LabelInstructionsPreviousElectionBallotPage
    //      , string.Empty //IsInclude = true
    //      , "IsTextInstructionsPreviousElectionBallotPage"
    //      , "InstructionsPreviousElectionBallotPage"
    //      //, "Ballot"
    //      );
    //    ReLoad_Entire_Form_To_View_Changes();

    //    db.Cache_Remove_Ballot_Domain_Design_Election_Previous("InstructionsPreviousElectionBallotPage");

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
    //    db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Ballot.css");

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
    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();
    //    #endregion Security Checks

    //    //Needs to be done on PostBack because design changes on any submission and needs to be reloaded.
    //    try
    //    {
    //      HyperLinkSamplePage.NavigateUrl = 
    //        UrlManager.GetBallotPageUri("CA", "20081104GCA000000ALL", 
    //          "053", "039", "076", "073").ToString();

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


  }
}
