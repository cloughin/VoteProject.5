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
  public partial class DomainDesigns : VotePage
  {
    #region Dead code

    //private void ReLoad_Entire_Form_To_View_Changes()
    //{
    //  #region Include / Omit Radio Buttons
    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedBannerAllPages
    //    , "IsIncludedBannerAllPages"
    //    );
    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedDonateAllPages
    //    , "IsIncludedDonateAllPages");

    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedFirstFooterAllPages
    //    , "IsIncludedFirstFooterAllPages");
    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedSecondFooterAllPages
    //    , "IsIncludedSecondFooterAllPages");
    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedEmailUsAllPages
    //    , "IsIncludedEmailUsAllPages");
    //  db.RadioButtonCustomIsIncludeSet(
    //    db.Domain_DesignCode_This()
    //    ,  RadioButtonListIsIncludedPoweredByAllPages
    //    , "IsIncludedPoweredByAllPages");
    //  #endregion

    //  #region Text / HTML Radio Buttons
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextFirstFooterAllPages, "IsTextFirstFooterAllPages");
    //  db.RadioButtonResetIsTextCustom(db.Domain_DesignCode_This(),  RadioButtonListIsTextSecondFooterAllPages, "IsTextSecondFooterAllPages");
    //  #endregion

    //  #region Current Content
    //  db.Label_Custom_Set(
    //    db.Domain_DesignCode_This()
    //    ,  LabelEmailUsLineAllPages
    //    , "EmailUsLineAllPages"
    //    );
    //  db.Label_Custom_Set(
    //    db.Domain_DesignCode_This()
    //    ,  LabelEmailUsAddressAllPages
    //    , "EmailUsAddressAllPages"
    //    );
    //  #endregion


    //  #region Custom Content in Texboxes
    //  TextBoxFirstFooterAllPages.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "FirstFooterAllPages");
    //  TextBoxSecondFooterAllPages.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "SecondFooterAllPages");
    //  TextBoxEmailUsLineAllPages.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "EmailUsLineAllPages");
    //  TextBoxEmailUsAddressAllPages.Text = db.DomainDesigns_Str(db.Domain_DesignCode_This(), "EmailUsAddressAllPages");
    //  #endregion Custom Content in Texboxes

    //  #region Style Sheets
    //  db.StyleSheetGet(TextBoxDefaultStyleSheet, @"css\All.css");
    //  db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "All.css"));

    //  db.Path_Css_Custom_Href_Set(ref All, db.Domain_DesignCode_This(), "All.css");
    //  #endregion Style Sheets
    //}

    ////-------- Radio Buttons for Include / Omit --------------------

    //protected void RadioButtonListIsIncludedBannerAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedBannerAllPages
    //      , "IsIncludedBannerAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedBannerAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsIncludedFirstFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedFirstFooterAllPages
    //      , "IsIncludedFirstFooterAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedFirstFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsIncludedSecondFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedSecondFooterAllPages
    //      , "IsIncludedSecondFooterAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedSecondFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIsIncludedDonateAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedDonateAllPages
    //      , "IsIncludedDonateAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedDonateAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsIncludedEmailUsAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedEmailUsAllPages
    //      , "IsIncludedEmailUsAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedEmailUsAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }

    //}

    //protected void RadioButtonListIsIncludedPoweredByAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.RadioButtonCustomIsIncludeUpdate(
    //       db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsIncludedPoweredByAllPages
    //      , "IsIncludedPoweredByAllPages"
    //      //, "AllPages"
    //      ,  Msg
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    //db.Cache_Remove_Domain_Design(db.Domain_DesignCode_This());

    //    Msg.Text = db.Msg4RadioButtonIncludeOmit(RadioButtonListIsIncludedPoweredByAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //// ---------- Radion Buttons for Text or HTML -----------

    //protected void RadioButtonListIsTextFirstFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextFirstFooterAllPages
    //      ,  TextBoxFirstFooterAllPages
    //      , "IsIncludedFirstFooterAllPages"
    //      , "IsTextFirstFooterAllPages"
    //      , "FirstFooterAllPages"
    //      //, "AllPages"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextFirstFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListIsTextSecondFooterAllPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextSecondFooterAllPages
    //      ,  TextBoxSecondFooterAllPages
    //      , "IsIncludedSecondFooterAllPages"
    //      , "IsTextSecondFooterAllPages"
    //      , "SecondFooterAllPages"
    //      //, "AllPages"
    //      );

    //    ReLoad_Entire_Form_To_View_Changes();

    //    Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextSecondFooterAllPages);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Get Default Design Buttons ---------------------

    //protected void ButtonGetDefaultFirstFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxFirstFooterAllPages
    //      , "FirstFooterAllPages");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultSecondFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxSecondFooterAllPages
    //      , "SecondFooterAllPages");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonGetDefaultEmailUs_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonDefaultGet(
    //       TextBoxEmailUsLineAllPages
    //      , "EmailUsLineAllPages");

    //    db.ButtonDefaultGet(
    //       TextBoxEmailUsAddressAllPages
    //      , "EmailUsAddressAllPages");

    //    Msg.Text = db.Msg4ButtonDefaultGet4Custom();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Clear Custom Design Buttons ---------------------

    //protected void ButtonClearCustomFirstFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextFirstFooterAllPages
    //      ,  TextBoxFirstFooterAllPages
    //      //,  LabelFirstFooterAllPages
    //      , "IsTextFirstFooterAllPages"
    //      , "FirstFooterAllPages"
    //      //, "AllPages"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomSecondFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextSecondFooterAllPages
    //      ,  TextBoxSecondFooterAllPages
    //      //,  LabelSecondFooterAllPages
    //      , "IsTextSecondFooterAllPages"
    //      , "SecondFooterAllPages"
    //      //, "AllPages"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonClearCustomEmailUs_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxEmailUsLineAllPages
    //      ,  LabelEmailUsLineAllPages
    //      , "EmailUsLineAllPages"
    //      );

    //    Msg.Text = db.Msg4ButtonCustomClear();

    //    db.ButtonCustomClear(
    //      db.Domain_DesignCode_This()
    //      ,  TextBoxEmailUsAddressAllPages
    //      ,  LabelEmailUsAddressAllPages
    //      , "EmailUsAddressAllPages"
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

    //protected void ButtonSubmitCustomFirstFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextFirstFooterAllPages
    //      ,  TextBoxFirstFooterAllPages
    //      , "IsIncludedFirstFooterAllPages"
    //      , "IsTextFirstFooterAllPages"
    //      , "FirstFooterAllPages"
    //      //, "AllPages"
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

    //protected void ButtonSubmitCustomSecondFooterAllPages_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Submit_Textbox_Custom(
    //      db.Domain_DesignCode_This()
    //      ,  RadioButtonListIsTextSecondFooterAllPages
    //      ,  TextBoxSecondFooterAllPages
    //      //,  LabelSecondFooterAllPages
    //      , "IsIncludedSecondFooterAllPages"
    //      , "IsTextSecondFooterAllPages"
    //      , "SecondFooterAllPages"
    //      //, "AllPages"
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

    //protected void ButtonSubmitCustomEmailUs_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (
    //      (TextBoxEmailUsLineAllPages.Text.Trim() == string.Empty)
    //      && (TextBoxEmailUsAddressAllPages.Text.Trim() == string.Empty)
    //    )
    //      throw new ApplicationException("Both textboxes are empty.");


    //    if (TextBoxEmailUsLineAllPages.Text.Trim() != string.Empty)
    //      db.Submit_Textbox_Custom(
    //        db.Domain_DesignCode_This()
    //        ,  TextBoxEmailUsLineAllPages
    //        ,  LabelEmailUsLineAllPages
    //        , "IsIncludedEmailUsAllPages"
    //        , "EmailUsLineAllPages"
    //        //, "AllPages"
    //        );

    //    if (TextBoxEmailUsAddressAllPages.Text.Trim() != string.Empty)
    //      db.Submit_Textbox_Custom(
    //        db.Domain_DesignCode_This()
    //        ,  TextBoxEmailUsAddressAllPages
    //        ,  LabelEmailUsAddressAllPages
    //        , "IsIncludedEmailUsAllPages"
    //        , "EmailUsAddressAllPages"
    //        //, "AllPages"
    //        );

    //    Msg.Text = db.Msg4ButtonCustomSubmit();

    //    ReLoad_Entire_Form_To_View_Changes();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    ////-------------- Upload Banner and Tagline ---------------------

    //protected void ButtonUploadBanner_ServerClick(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Backup_And_Save_Http_Posted_File(
    //     Request.Files["FileBanner"]
    //     , db.Path_Part_Image_Banner_NoExtn(db.Domain_DesignCode_This())
    //     + db.Http_Posted_File_Image_Extn(Request.Files["FileBanner"]));
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonUploadTagline_ServerClick(object sender, EventArgs e)
    //{
    //  //bool IsUploadOk = true;
    //  try
    //  {
    //    db.Backup_And_Save_Http_Posted_File(
    //     Request.Files["FileTagline"]
    //     , db.Path_Part_Image_TagLine_NoExtn(db.Domain_DesignCode_This())
    //     + db.Http_Posted_File_Image_Extn(Request.Files["FileTagLine"]));
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //  //if (IsUploadOk)
    //  //  Response.Redirect("/Design/DesignAllPages.aspx?UploadedBannerTagline=Ok");//To force reloading of the new banner on the page
    //}

    //protected void ButtonUploadCustom_ServerClick(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    //db.CustomFileUpload(
    //    // Request.Files["FileCustom"]
    //    // , db.Path_Part_Image_sCustom(db.Domain_DesignCode_This()
    //    // , Path.GetFileName(Request.Files["FileCustom"].FileName))
    //    // );
    //    db.FileUpload(
    //     Request.Files["FileCustom"]
    //     , db.Path_Part_Image_Custom(db.Domain_DesignCode_This()
    //     , Path.GetFileName(Request.Files["FileCustom"].FileName))
    //     );

    //    Msg.Text = db.Ok("The file was uploaded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"All.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"All.css"));

    //    db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "All.css"));

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
    //      , db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), @"All.css"));

    //    db.StyleSheetGet(TextBoxCustomStyleSheet, db.Path_Part_Css_Custom(db.Domain_DesignCode_This(), "All.css"));

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
    //      LabelDomainDesignCode.Text = db.Domain_DesignCode_This();

    //      //HyperLinkSamplePage.NavigateUrl = db.Url_Default();
    //      HyperLinkSamplePage.NavigateUrl = db.Url_SamplePage(
    //        //db.User_Security()
    //        //db.User_Security()
    //      db.Domain_DesignCode_This()
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
