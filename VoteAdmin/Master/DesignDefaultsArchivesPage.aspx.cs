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
  public partial class DesignDefaultsArchivesPage : VotePage
  {
   // private void ReLoad_Entire_Form_To_View_Changes()
   // {
   //   #region Text / HTML Radio Buttons
   //   db.RadioButtonResetIsTextDefault( RadioButtonListIsTextInstructionsArchivesPage, "IsTextInstructionsArchivesPage");
   //   #endregion

   //   #region Current Content
   //   db.Label_Default_Set(
   //       LabelInstructionsArchivesPage
   //      , "InstructionsArchivesPage"
   //      , "IsTextInstructionsArchivesPage"
   //      );

   //   #endregion

   //   #region Current Content in Textboxes

   //   if (db.MasterDesign_Str("InstructionsArchivesPage") != string.Empty)
   //     TextBoxInstructionsArchivesPage.Text = db.MasterDesign_Str("InstructionsArchivesPage");

   //   db.StyleSheetGet(TextBoxStyleSheet, @"css\Archives.css");
   //   #endregion
   // }
   // // ------------------ Radion Buttons for Text or HTML -----------

   // protected void RadioButtonListIsTextInstructionsArchivesPage_SelectedIndexChanged(object sender, EventArgs e)
   // {
   //   try
   //   {
   //     db.Submit_Textbox_Default(
   //        RadioButtonListIsTextInstructionsArchivesPage
   //       ,  TextBoxInstructionsArchivesPage
   //       ,  LabelInstructionsArchivesPage
   //       , string.Empty //IsInclude = true
   //       , "IsTextInstructionsArchivesPage"
   //       , "InstructionsArchivesPage"
   //       //, "Archives"
   //       );

   //     ReLoad_Entire_Form_To_View_Changes();

   //     Msg.Text = db.Msg4RadioButtonTextOrHtml(RadioButtonListIsTextInstructionsArchivesPage);
   //   }
   //   catch (Exception ex)
   //   {
   //     Msg.Text = db.Fail(ex.Message);
   //     db.Log_Error_Admin(ex);
   //   }

   // }

   //// -----------------Submit Default------------
   // protected void ButtonSubmitDefaultInstructionsArchivesPage_Click(object sender, EventArgs e)
   // {
   //   try
   //   {
   //     db.Submit_Textbox_Default(
   //        RadioButtonListIsTextInstructionsArchivesPage
   //       ,  TextBoxInstructionsArchivesPage
   //       ,  LabelInstructionsArchivesPage
   //       , string.Empty //IsInclude = true
   //       , "IsTextInstructionsArchivesPage"
   //       , "InstructionsArchivesPage"
   //       //, "Archives"
   //       );
   //     ReLoad_Entire_Form_To_View_Changes();

   //     Msg.Text = db.Msg4ButtonDefaultSubmit();
   //   }
   //   catch (Exception ex)
   //   {
   //     Msg.Text = db.Fail(ex.Message);
   //     db.Log_Error_Admin(ex);
   //   }
   // }

   // //-------------- Style Sheet Button ---------------------

   // protected void ButtonUpdateStyleSheet_Click(object sender, EventArgs e)
   // {
   //   try
   //   {
   //     db.StyleSheetUpdateAndCheckTextBox(TextBoxStyleSheet, @"css\Archives.css");

   //     ReLoad_Entire_Form_To_View_Changes();

   //     Msg.Text = db.Msg4ButtonUpdateStyleSheet();
   //   }
   //   catch (Exception ex)
   //   {
   //     #region
   //     Msg.Text = db.Fail(ex.Message);
   //     db.Log_Error_Admin(ex);
   //     #endregion
   //   }
   // }

   // // ----------------- Page Load -----------------

   // protected void Page_Load(object sender, EventArgs e)
   // {
   //   if (!IsPostBack)
   //   {
   //     if (!SecurePage.IsMasterUser)
   //       SecurePage.HandleSecurityException();

   //     try
   //     {
   //       ReLoad_Entire_Form_To_View_Changes();

   //       HyperLinkSamplePage.NavigateUrl =
   //         UrlManager.GetForResearchPageUri("VA").ToString();
   //     }
   //     catch (Exception ex)
   //     {
   //       #region
   //       Msg.Text = db.Fail(ex.Message);
   //       db.Log_Error_Admin(ex);
   //       #endregion
   //     }
   //   }
   // }

   // #region Dead code

   // //// ------------ Get Default Design Buttons -------------------
   // //protected void ButtonGetDefaultInstructionsArchivesPage_Click(object sender, EventArgs e)
   // //{
   // //  try
   // //  {
   // //    db.ButtonDefaultGet(
   // //       TextBoxInstructionsArchivesPage
   // //      , "InstructionsArchivesPage");

   // //    Msg.Text = db.Msg4ButtonDefaultGet();
   // //  }
   // //  catch (Exception ex)
   // //  {
   // //    Msg.Text = db.Fail(ex.Message);
   // //    db.Log_Error_Admin(ex);
   // //  }
   // //}

   // #endregion Dead code


  }
}
