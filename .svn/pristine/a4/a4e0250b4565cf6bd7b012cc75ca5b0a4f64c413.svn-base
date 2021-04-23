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
  public partial class DesignElectedOfficialsCountiesPage : VotePage
  {
    #region Dead code

    //    private void ReLoad_Entire_Form_To_View_Changes()
//    {
//      #region Text / HTML Radio Buttons
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageUSPres, "IsTextInstructionsElectedOfficialsPageUSPres");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageUSSenate, "IsTextInstructionsElectedOfficialsPageUSSenate");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageUSHouse, "IsTextInstructionsElectedOfficialsPageUSHouse");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageGovernors, "IsTextInstructionsElectedOfficialsPageGovernors");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageState, "IsTextInstructionsElectedOfficialsPageState");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageCounty, "IsTextInstructionsElectedOfficialsPageCounty");
//      db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextInstructionsOfficialsPageLocal, "IsTextInstructionsElectedOfficialsPageLocal");
//      #endregion

//      #region Current Content in Labels
//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageUSPres
//        , "InstructionsElectedOfficialsPageUSPres"
//        , "IsTextInstructionsElectedOfficialsPageUSPres"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageUSSenate
//        , "InstructionsElectedOfficialsPageUSSenate"
//        , "IsTextInstructionsElectedOfficialsPageUSSenate"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageUSHouse
//        , "InstructionsElectedOfficialsPageUSHouse"
//        , "IsTextInstructionsElectedOfficialsPageUSHouse"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageGovernors
//        , "InstructionsElectedOfficialsPageGovernors"
//        , "IsTextInstructionsElectedOfficialsPageGovernors"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageState
//        , "InstructionsElectedOfficialsPageState"
//        , "IsTextInstructionsElectedOfficialsPageState"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageCounty
//        , "InstructionsElectedOfficialsPageCounty"
//        , "IsTextInstructionsElectedOfficialsPageCounty"
//        );

//      db.Label_Custom_Set(
//         db.Domain_DesignCode_This()
//         ,  LabelInstructionsOfficialsPageLocal
//        , "InstructionsElectedOfficialsPageLocal"
//        , "IsTextInstructionsElectedOfficialsPageLocal"
//        );

//#endregion Current Content in Labels

//      #region Custom Content in Texboxes
//      TextBoxInstructionsOfficialsPageUSPres.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageUSPres");
//      TextBoxInstructionsOfficialsPageUSSenate.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageUSSenate");
//      TextBoxInstructionsOfficialsPageUSHouse.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageUSHouse");
//      TextBoxInstructionsOfficialsPageGovernors.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageGovernors");
//      TextBoxInstructionsOfficialsPageState.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageState");
//      TextBoxInstructionsOfficialsPageCounty.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageCounty");
//      TextBoxInstructionsOfficialsPageLocal.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "InstructionsElectedOfficialsPageLocal");
//      #endregion Custom Content in Texboxes

//      #region Style Sheets
//      db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Officials.css"));
//      db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Officials.css"));

//      db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
//      db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Officials.css");
//      #endregion Style Sheets
//    }
//    // ---------- Radion Buttons for Text or HTML -----------

//    protected void RadioButtonListIsTextInstructionsOfficialsPageUSPres_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSPres
//          ,  TextBoxInstructionsOfficialsPageUSPres
//          ,  LabelInstructionsOfficialsPageUSPres
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSPres"
//          , "InstructionsElectedOfficialsPageUSPres"
//          //, "ElectedOfficials"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSPres);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageUSSenate_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSSenate
//          ,  TextBoxInstructionsOfficialsPageUSSenate
//          ,  LabelInstructionsOfficialsPageUSSenate
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSSenate"
//          , "InstructionsElectedOfficialsPageUSSenate"
//          //, "ElectedOfficials"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSSenate);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageUSHouse_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSHouse
//          ,  TextBoxInstructionsOfficialsPageUSHouse
//          ,  LabelInstructionsOfficialsPageUSHouse
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSHouse"
//          , "InstructionsElectedOfficialsPageUSHouse"
//          //, "ElectedOfficials"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageUSHouse);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageGovernors_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageGovernors
//          ,  TextBoxInstructionsOfficialsPageGovernors
//          ,  LabelInstructionsOfficialsPageGovernors
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageGovernors"
//          , "InstructionsElectedOfficialsPageGovernors"
//          //, "ElectedOfficials"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageGovernors);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageState_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageState
//          ,  TextBoxInstructionsOfficialsPageState
//          ,  LabelInstructionsOfficialsPageState
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageState"
//          , "InstructionsElectedOfficialsPageState"
//          //, "ElectedOfficials"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageState);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageCounty_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageCounty
//          ,  TextBoxInstructionsOfficialsPageCounty
//          ,  LabelInstructionsOfficialsPageCounty
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageCounty"
//          , "InstructionsElectedOfficialsPageCounty"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageCounty);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void RadioButtonListIsTextInstructionsOfficialsPageLocal_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageLocal
//          ,  TextBoxInstructionsOfficialsPageLocal
//          ,  LabelInstructionsOfficialsPageLocal
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageLocal"
//          , "InstructionsElectedOfficialsPageLocal"
//          );

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsOfficialsPageLocal);
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
//    #region commented out
//    ////---------- Get Custom Design Button ---------------------------------
//    //protected void ButtonGetCustomInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonCustomGet(
//    //       db.Domain_DesignCode_This()
//    //      ,  RadioButtonListIsTextInstructionsOfficialsPageUSPres
//    //      ,  TextBoxInstructionsOfficialsPageUSPres
//    //      , "IsTextInstructionsElectedOfficialsPageUSPres"
//    //      , "InstructionsElectedOfficialsPageUSPres");

//    //    Msg.Text = db.Msg4ButtonCustomtGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetCustomInstructionsOfficialsPageUSSenate_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonCustomGet(
//    //       db.Domain_DesignCode_This()
//    //      ,  RadioButtonListIsTextInstructionsOfficialsPageUSSenate
//    //      ,  TextBoxInstructionsOfficialsPageUSSenate
//    //      , "IsTextInstructionsElectedOfficialsPageUSSenate"
//    //      , "InstructionsElectedOfficialsPageUSSenate");

//    //    Msg.Text = db.Msg4ButtonCustomtGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetCustomInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonCustomGet(
//    //       db.Domain_DesignCode_This()
//    //      ,  RadioButtonListIsTextInstructionsOfficialsPageUSHouse
//    //      ,  TextBoxInstructionsOfficialsPageUSHouse
//    //      , "IsTextInstructionsElectedOfficialsPageUSHouse"
//    //      , "InstructionsElectedOfficialsPageUSHouse");

//    //    Msg.Text = db.Msg4ButtonCustomtGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetCustomInstructionsOfficialsPageState_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonCustomGet(
//    //       db.Domain_DesignCode_This()
//    //      ,  RadioButtonListIsTextInstructionsOfficialsPageState
//    //      ,  TextBoxInstructionsOfficialsPageState
//    //      , "IsTextInstructionsElectedOfficialsPageState"
//    //      , "InstructionsElectedOfficialsPageState");

//    //    Msg.Text = db.Msg4ButtonCustomtGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}

//    //protected void ButtonGetCustomInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
//    //{
//    //  try
//    //  {
//    //    db.ButtonCustomGet(
//    //       db.Domain_DesignCode_This()
//    //      ,  RadioButtonListIsTextInstructionsOfficialsPageCounty
//    //      ,  TextBoxInstructionsOfficialsPageCounty
//    //      , "IsTextInstructionsElectedOfficialsPageCounty"
//    //      , "InstructionsElectedOfficialsPageCounty");

//    //    Msg.Text = db.Msg4ButtonCustomtGet();
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //  }
//    //}
//    #endregion

//    //-------------- Get Default Design Button ---------------------
//    protected void ButtonGetDefaultInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageUSPres
//          , "InstructionsElectedOfficialsPageUSPres");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonGetDefaultInstructionsOfficialsPageUSSenate_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageUSSenate
//          , "InstructionsElectedOfficialsPageUSSenate");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonGetDefaultInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageUSHouse
//          , "InstructionsElectedOfficialsPageUSHouse");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonGetDefaultInstructionsOfficialsPageGovernors_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageGovernors
//          , "InstructionsElectedOfficialsPageGovernors");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonGetDefaultInstructionsOfficialsPageState_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageState
//          , "InstructionsElectedOfficialsPageState");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonGetDefaultInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageCounty
//          , "InstructionsElectedOfficialsPageCounty");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
//    protected void ButtonGetDefaultInstructionsOfficialsPageLocal_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonDefaultGet(
//           TextBoxInstructionsOfficialsPageLocal
//          , "InstructionsElectedOfficialsPageLocal");

//        Msg.Text = db.Msg4ButtonDefaultGet4Custom();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }
//    //-------------- Clear Custom Design Button ---------------------
//    protected void ButtonClearCustomInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSPres
//          ,  TextBoxInstructionsOfficialsPageUSPres
//          ,  LabelInstructionsOfficialsPageUSPres
//          , "IsTextInstructionsElectedOfficialsPageUSPres"
//          , "InstructionsElectedOfficialsPageUSPres"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageUSSenate_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSSenate
//          ,  TextBoxInstructionsOfficialsPageUSSenate
//          ,  LabelInstructionsOfficialsPageUSSenate
//          , "IsTextInstructionsElectedOfficialsPageUSSenate"
//          , "InstructionsElectedOfficialsPageUSSenate"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSHouse
//          ,  TextBoxInstructionsOfficialsPageUSHouse
//          ,  LabelInstructionsOfficialsPageUSHouse
//          , "IsTextInstructionsElectedOfficialsPageUSHouse"
//          , "InstructionsElectedOfficialsPageUSHouse"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageGovernors_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageGovernors
//          ,  TextBoxInstructionsOfficialsPageGovernors
//          ,  LabelInstructionsOfficialsPageGovernors
//          , "IsTextInstructionsElectedOfficialsPageGovernors"
//          , "InstructionsElectedOfficialsPageGovernors"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageState_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageState
//          ,  TextBoxInstructionsOfficialsPageState
//          ,  LabelInstructionsOfficialsPageState
//          , "IsTextInstructionsElectedOfficialsPageState"
//          , "InstructionsElectedOfficialsPageState"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageCounty
//          ,  TextBoxInstructionsOfficialsPageCounty
//          ,  LabelInstructionsOfficialsPageCounty
//          , "IsTextInstructionsElectedOfficialsPageCounty"
//          , "InstructionsElectedOfficialsPageCounty"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonClearCustomInstructionsOfficialsPageLocal_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.ButtonCustomClear(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageLocal
//          ,  TextBoxInstructionsOfficialsPageLocal
//          ,  LabelInstructionsOfficialsPageLocal
//          , "IsTextInstructionsElectedOfficialsPageLocal"
//          , "InstructionsElectedOfficialsPageLocal"
//          );

//        Msg.Text = db.Msg4ButtonCustomClear();
//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
//    //-------------- Submit Custom Design Button ---------------------
//    protected void ButtonSubmitCustomInstructionsOfficialsPageUSPres_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSPres
//          ,  TextBoxInstructionsOfficialsPageUSPres
//          ,  LabelInstructionsOfficialsPageUSPres
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSPres"
//          , "InstructionsElectedOfficialsPageUSPres"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitCustom1_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSSenate
//          ,  TextBoxInstructionsOfficialsPageUSSenate
//          ,  LabelInstructionsOfficialsPageUSSenate
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSSenate"
//          , "InstructionsElectedOfficialsPageUSSenate"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitCustomInstructionsOfficialsPageUSHouse_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageUSHouse
//          ,  TextBoxInstructionsOfficialsPageUSHouse
//          ,  LabelInstructionsOfficialsPageUSHouse
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageUSHouse"
//          , "InstructionsElectedOfficialsPageUSHouse"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitCustomInstructionsOfficialsPageGovernors_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageGovernors
//          ,  TextBoxInstructionsOfficialsPageGovernors
//          ,  LabelInstructionsOfficialsPageGovernors
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageGovernors"
//          , "InstructionsElectedOfficialsPageGovernors"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
    
//    protected void ButtonSubmitCustomInstructionsOfficialsPageState_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageState
//          ,  TextBoxInstructionsOfficialsPageState
//          ,  LabelInstructionsOfficialsPageState
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageState"
//          , "InstructionsElectedOfficialsPageState"
//          //, "ElectedOfficials"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }

//    protected void ButtonSubmitCustomInstructionsOfficialsPageCounty_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageCounty
//          ,  TextBoxInstructionsOfficialsPageCounty
//          ,  LabelInstructionsOfficialsPageCounty
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageCounty"
//          , "InstructionsElectedOfficialsPageCounty"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }
//    }
//    protected void ButtonSubmitCustomInstructionsOfficialsPageLocal_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.Submit_Textbox_Custom(
//          db.Domain_DesignCode_This()
//          ,  RadioButtonListIsTextInstructionsOfficialsPageLocal
//          ,  TextBoxInstructionsOfficialsPageLocal
//          ,  LabelInstructionsOfficialsPageLocal
//          , string.Empty
//          , "IsTextInstructionsElectedOfficialsPageLocal"
//          , "InstructionsElectedOfficialsPageLocal"
//          );

//        Msg.Text = db.Msg4ButtonCustomSubmit();

//        ReLoad_Entire_Form_To_View_Changes();

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//      }

//    }
//    //-------------- Style Sheet Buttons ---------------------

//    protected void ButtonUploadStyleSheet_ServerClick1(object sender, EventArgs e)
//    {
//      try
//      {
//        db.StyleSheetUpload(
//          Request.Files["FileStyleSheet"]
//          , TextBoxCustomStyleSheet
//          , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Officials.css"));

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonUploadStyleSheet();
//      }

//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }

//    protected void ButtonDeleteCustomStyleSheet1_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.StyleSheetDelete(
//          TextBoxCustomStyleSheet
//          , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Officials.css"));

//        ReLoad_Entire_Form_To_View_Changes();

//        Msg.Text = db.Msg4ButtonDeleteStyleSheet();
//      }
//      catch (Exception ex)
//      {
//        #region
//        db.Log_Error_Admin(ex);
//        Msg.Text = db.Fail(ex.Message);
//        #endregion
//      }
//    }

//    protected void ButtonUpdateCustomStyleSheet_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        db.StyleSheetUpdateAndCheckTextBox(
//          TextBoxCustomStyleSheet
//          , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Officials.css"));

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
//        #region Security Checks
//        if (db.Domain_DesignCode_This() == string.Empty)
//          db.HandleMissingDomainDesignCode();
//        #endregion Security Checks

//        try
//        {
//          LabelDesignCode.Text = db.Domain_DesignCode_This();

//          HyperLinkSamplePage.NavigateUrl = 
//            db.Url_SamplePage(db.Domain_DesignCode_This(), 
//              UrlManager.GetOfficialsPageUri(db.StateCode_SamplePage()))
//            .ToString();

//          ReLoad_Entire_Form_To_View_Changes();
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

    #endregion Dead code








  }
}
