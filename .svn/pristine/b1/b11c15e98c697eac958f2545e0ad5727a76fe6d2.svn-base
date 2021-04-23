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
  public partial class DesignVotersPage : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextContentVoters, "IsTextContentVotersPage");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //     db.Domain_DesignCode_This()
    //     ,  LabelContentVoters
    //    , "ContentVotersPage"
    //    , "IsTextContentVotersPage"
    //    );

    //  #endregion

    //  #region Custom Content in Texboxes
    //  TextBoxContentVoters.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "ContentVotersPage");
    //  #endregion Custom Content in Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, db.Path_Part_Css_Default("Voters.css"));
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Voters.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Voters.css");
    //  #endregion Style Sheets
    //}

    //// ---------- Radion Buttons for Text or HTML -----------
    //protected void RadioButtonListIsTextContentVoters_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextContentVoters
    //      ,  TextBoxContentVoters
    //      ,  LabelContentVoters
    //      , string.Empty
    //      , "IsTextContentVotersPage"
    //      , "ContentVotersPage"
    //      //, "Voters"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextContentVoters);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Get Default Design Buttons ---------------------
    //protected void ButtonGetDefaultContentVoters_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxContentVoters
    //      , "ContentVotersPage");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Clear Custom Design Buttons ---------------------
    //protected void ButtonClearCustomContentVoters_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextContentVoters
    //      ,  TextBoxContentVoters
    //      ,  LabelContentVoters
    //      , "IsTextContentVotersPage"
    //      , "ContentVotersPage"
    //      //, "Voters"
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
    //protected void ButtonSubmitCustomContentVoters_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextContentVoters
    //      ,  TextBoxContentVoters
    //      ,  LabelContentVoters
    //      , string.Empty
    //      , "IsTextContentVotersPage"
    //      , "ContentVotersPage"
    //      //, "Voters"
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Voters.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Voters.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "Voters.css"));

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

    //      HyperLinkSamplePage.NavigateUrl = 
    //        db.Url_SamplePage(db.Domain_DesignCode_This(), 
    //          UrlManager.GetForVotersPageUri())
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
