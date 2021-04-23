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
  public partial class DesignDefaultsElectionPage : VotePage
  {
//    private void ReLoad_Entire_Form_To_View_Changes()
//    {
//      #region Text / HTML Radio Buttons
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageUSPresPrimary, "IsTextInstructionsElectionPageUSPresPrimary");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageUSPres, "IsTextInstructionsElectionPageUSPres");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageUSSenate, "IsTextInstructionsElectionPageUSSenate");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageUSHouse, "IsTextInstructionsElectionPageUSHouse");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageState, "IsTextInstructionsElectionPageState");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageCounty, "IsTextInstructionsElectionPageCounty");
//      db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsElectionPageLocal, "IsTextInstructionsElectionPageLocal");
//      #endregion

//      #region Current Content in Labels
//      db.Label_Default_Set(
//          LabelInstructionsElectionPageUSPresPrimary
//         , "InstructionsElectionPageUSPresPrimary"
//         , "IsTextInstructionsElectionPageUSPresPrimary"
//         );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageUSPres
//          , "InstructionsElectionPageUSPres"
//          , "IsTextInstructionsElectionPageUSPres"
//        );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageUSSenate
//         , "InstructionsElectionPageUSSenate"
//         , "IsTextInstructionsElectionPageUSSenate"
//         );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageUSHouse
//         , "InstructionsElectionPageUSHouse"
//         , "IsTextInstructionsElectionPageUSHouse"
//         );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageState
//         , "InstructionsElectionPageState"
//         , "IsTextInstructionsElectionPageState"
//         );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageCounty
//         , "InstructionsElectionPageCounty"
//         , "IsTextInstructionsElectionPageCounty"
//         );

//      db.Label_Default_Set(
//          LabelInstructionsElectionPageLocal
//         , "InstructionsElectionPageLocal"
//         , "IsTextInstructionsElectionPageLocal"
//         );

//      #endregion Current Content in Labels

//      #region Current Content in Textboxes

//      if (db.MasterDesign_Str("InstructionsElectionPageUSPresPrimary") != string.Empty)
//        TextBoxInstructionsElectionPageUSPresPrimary.Text = db.MasterDesign_Str("InstructionsElectionPageUSPresPrimary");

//      if (db.MasterDesign_Str("InstructionsElectionPageUSPres") != string.Empty)
//        TextBoxInstructionsElectionPageUSPres.Text = db.MasterDesign_Str("InstructionsElectionPageUSPres");

//      if (db.MasterDesign_Str("InstructionsElectionPageUSSenate") != string.Empty)
//        TextBoxInstructionsElectionPageUSSenate.Text = db.MasterDesign_Str("InstructionsElectionPageUSSenate");

//      if (db.MasterDesign_Str("InstructionsElectionPageUSHouse") != string.Empty)
//        TextBoxInstructionsElectionPageUSHouse.Text = db.MasterDesign_Str("InstructionsElectionPageUSHouse");

//      if (db.MasterDesign_Str("InstructionsElectionPageState") != string.Empty)
//        TextBoxInstructionsElectionPageState.Text = db.MasterDesign_Str("InstructionsElectionPageState");

//      if (db.MasterDesign_Str("InstructionsElectionPageCounty") != string.Empty)
//        TextBoxInstructionsElectionPageCounty.Text = db.MasterDesign_Str("InstructionsElectionPageCounty");

//      if (db.MasterDesign_Str("InstructionsElectionPageLocal") != string.Empty)
//        TextBoxInstructionsElectionPageLocal.Text = db.MasterDesign_Str("InstructionsElectionPageLocal");

//      #endregion Current Content in Textboxes

//      db.StyleSheetGet(TextBoxStyleSheet, @"css\Election.css");
//    }
//    // ------------------ Radion Buttons for Text or HTML -----------
//    protected void RadioButtonListIsTextInstructionsElectionPageUSPresPrimary_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSPresPrimary
//          ,  TextBoxInstructionsElectionPageUSPresPrimary
//          ,  LabelInstructionsElectionPageUSPresPrimary
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSPresPrimary"
//          , "InstructionsElectionPageUSPresPrimary"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSPresPrimary);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageUSPres_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSPres
//          ,  TextBoxInstructionsElectionPageUSPres
//          ,  LabelInstructionsElectionPageUSPres
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSPres"
//          , "InstructionsElectionPageUSPres"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSPres);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageUSSenate_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSSenate
//          ,  TextBoxInstructionsElectionPageUSSenate
//          ,  LabelInstructionsElectionPageUSSenate
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSSenate"
//          , "InstructionsElectionPageUSSenate"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSSenate);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageUSHouse_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSHouse
//          ,  TextBoxInstructionsElectionPageUSHouse
//          ,  LabelInstructionsElectionPageUSHouse
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSHouse"
//          , "InstructionsElectionPageUSHouse"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageUSHouse);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageState_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageState
//          ,  TextBoxInstructionsElectionPageState
//          ,  LabelInstructionsElectionPageState
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageState"
//          , "InstructionsElectionPageState"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageState);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageCounty_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageCounty
//          ,  TextBoxInstructionsElectionPageCounty
//          ,  LabelInstructionsElectionPageCounty
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageCounty"
//          , "InstructionsElectionPageCounty"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageCounty);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsElectionPageLocal_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageLocal
//          ,  TextBoxInstructionsElectionPageLocal
//          ,  LabelInstructionsElectionPageLocal
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageLocal"
//          , "InstructionsElectionPageLocal"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsElectionPageLocal);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }    
//#region commented out
//    //// ------------ Get Default Design Buttons -------------------
//    //protected void ButtonGetDefaultInstructionsElectionPageUSPresPrimary_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageUSPresPrimary
//    //      , "InstructionsElectionPageUSPresPrimary");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetDefaultInstructionsElectionPageUSPres_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageUSPres
//    //      , "InstructionsElectionPageUSPres");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetDefaultInstructionsElectionPageUSSenate_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageUSSenate
//    //      , "InstructionsElectionPageUSSenate");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetDefaultInstructionsElectionPageUSHouse_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageUSHouse
//    //      , "InstructionsElectionPageUSHouse");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetDefaultInstructionsElectionPageState_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageState
//    //      , "InstructionsElectionPageState");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetDefaultInstructionsElectionPageCounty_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonDefaultGet(
//    //       TextBoxInstructionsElectionPageCounty
//    //      , "InstructionsElectionPageCounty");

//    //    Msg.Text = db.Msg4ButtonDefaultGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}
//    // -----------------Update Default------------
//    #endregion commented out

//    protected void ButtonSubmitDefaultInstructionsElectionPageUSPresPrimary_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSPresPrimary
//          ,  TextBoxInstructionsElectionPageUSPresPrimary
//          ,  LabelInstructionsElectionPageUSPresPrimary
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSPresPrimary"
//          , "InstructionsElectionPageUSPresPrimary"
//          //, "Election"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageUSPres_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSPres
//          ,  TextBoxInstructionsElectionPageUSPres
//          ,  LabelInstructionsElectionPageUSPres
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSPres"
//          , "InstructionsElectionPageUSPres"
//          //, "Election"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageUSSenate_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSSenate
//          ,  TextBoxInstructionsElectionPageUSSenate
//          ,  LabelInstructionsElectionPageUSSenate
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSSenate"
//          , "InstructionsElectionPageUSSenate"
//          //, "Election"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageUSHouse_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageUSHouse
//          ,  TextBoxInstructionsElectionPageUSHouse
//          ,  LabelInstructionsElectionPageUSHouse
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageUSHouse"
//          , "InstructionsElectionPageUSHouse"
//          //, "Election"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageState_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageState
//          ,  TextBoxInstructionsElectionPageState
//          ,  LabelInstructionsElectionPageState
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageState"
//          , "InstructionsElectionPageState"
//          //, "Election"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageCounty_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageCounty
//          ,  TextBoxInstructionsElectionPageCounty
//          ,  LabelInstructionsElectionPageCounty
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageCounty"
//          , "InstructionsElectionPageCounty"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitDefaultInstructionsElectionPageLocal_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Default(
//           RadioButtonListIsTextInstructionsElectionPageLocal
//          ,  TextBoxInstructionsElectionPageLocal
//          ,  LabelInstructionsElectionPageLocal
//          , string.Empty //IsInclude = true
//          , "IsTextInstructionsElectionPageLocal"
//          , "InstructionsElectionPageLocal"
//          );
//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDefaultSubmit();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
//    //-------------- Style Sheet Button ---------------------

//    protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Election.css");
//        //Don't need to remove Cache pages when a style sheet changes
//        //db.Cache_Remove_Election_For_All();

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonUpdateStyleSheet();
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }


//    // ----------------- Page Load -----------------

//    protected void Page_Load(object sender, EventArgs e)
//    {
//      if (!IsPostBack)
//      {
//        if (!SecurePage.IsMasterUser)
//          SecurePage.HandleSecurityException();

//        try
//        {
//          ReLoad_Entire_Form_To_View_Changes();

//          HyperLinkSamplePage.NavigateUrl = 
//            UrlManager.GetElectionPageUri("20081104GVA000000ALL").ToString();
//          HyperLinkSamplePage2.NavigateUrl =
//            UrlManager.GetElectionPageUri("20080212BVA000000VAD").ToString();
//        }
//        catch (Exception ex)
//        {
//          #region
//          Msg.Text = db.Fail(ex.Message);
//          db.Log_Error_Admin(ex);
//          #endregion
//        }
//      }
//    }




  }
}
