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
  public partial class ImagesPage : VotePage
  {
    //protected string Path_Part_Destination_Single_Image(string Width_Str)
    //{
    //  return @"images\Politicians" + Width_Str + @"\"
    //    + TextBox_Destination_Image_Name.Text.Trim();
    //}

    //protected string Url_Image_Single()
    //{
    //  string Url = string.Empty;
    //  switch (RadioButtonList_Destination_Path.SelectedValue.ToString())
    //  {
    //    case "Candidates":
    //      Url = @"/images/Candidates/";
    //      break;
    //    case "CandidatesSmall":
    //      Url = @"/images/CandidatesSmall/";
    //      break;
    //    case "SampleBallots":
    //      Url = @"/images/SampleBallots/";
    //      break;
    //    case "SampleFileTypesSizes":
    //      Url = @"/images/SampleFileTypesSizes/";
    //      break;
    //    case "Test":
    //      Url = @"/images/Test/";
    //      break;
    //  }
    //  Url += TextBox_Destination_Image_Name.Text.Trim();
    //  Url += ".png";

    //  return Url;
    //}

    //protected void RadioButtonList_Upload_Type_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  //try
    //  //{
    //  //  TextBox_Destination_Image_Name.Enabled = true;

    //  //  if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "Single")
    //  //  {
    //  //    RadioButtonList_Destination_Path.Enabled = true;

    //  //    Msg.Text = db.Msg("Select Destination Folder and enter Image Name."
    //  //      + " Then use the Browse button to select the image file to upload.");
    //  //  }
    //  //  else
    //  //  {
    //  //    RadioButtonList_Destination_Path.Enabled = false;

    //  //    Msg.Text = db.Msg("Select Destination then enter PoliticianKey or NoPhoto in either textbox."
    //  //      + " Then follow the directions above the textbox.");
    //  //  }
    //  //}
    //  //catch (Exception ex)
    //  //{
    //  //  Msg.Text = db.Fail(ex.Message);
    //  //  db.Log_Error_Admin(ex);
    //  //}
    //}

    //protected void ButtonUploadPicture_ServerClick1(object sender, EventArgs e)
    //{
    //  //try
    //  //{
    //  //  #region inits
    //  //  //byte[] Image_Byte_Array = null;
    //  ////int Max_Width = 0;
    //  ////int Max_Height = 0;
    //  //#endregion inits

    //  //  string Url = db.Url_Master_Images();
    //  //  Url += "&Images=" + RadioButtonList_Upload_Type.SelectedValue.ToString();
    //  //  Url += "&Id=" + TextBox_Destination_Image_Name.Text.Trim();

    //  //  string Path_Full_Destination_Image_File = string.Empty;

    //  //  #region checks
    //  //  if (TextBox_Destination_Image_Name.Text.Trim() == string.Empty)
    //  //    throw new ApplicationException("Destination File Name is missing.");
    //  //  #endregion checks

    //  //  if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "Single")
    //  //  {
    //  //    #region Single

    //  //    #region checks
    //  //    if (RadioButtonList_Destination_Path.SelectedIndex == -1)
    //  //      throw new ApplicationException("A Destination Folder needs to be selected.");
    //  //    #endregion checks

    //  //    #region Upload and Save as png regardlsess of image file type

    //  //    #region Destination Path
    //  //    string Path_Part_Destination_Without_Ext = string.Empty;
    //  //    switch (RadioButtonList_Destination_Path.SelectedValue.ToString())
    //  //    {
    //  //      case "Candidates":
    //  //        Path_Part_Destination_Without_Ext = @"images\Candidates\";
    //  //        break;
    //  //      case "CandidatesSmall":
    //  //        Path_Part_Destination_Without_Ext = @"images\CandidatesSmall\";
    //  //        break;
    //  //      case "SampleBallots":
    //  //        Path_Part_Destination_Without_Ext = @"images\SampleBallots\";
    //  //        break;
    //  //      case "SampleFileTypesSizes":
    //  //        Path_Part_Destination_Without_Ext = @"images\SampleFileTypesSizes\";
    //  //        break;
    //  //      case "Test":
    //  //        Path_Part_Destination_Without_Ext = @"images\Test\";
    //  //        break;
    //  //    }
    //  //    Path_Part_Destination_Without_Ext += TextBox_Destination_Image_Name.Text.Trim();
    //  //    #endregion Destination Path

    //  //    Path_Full_Destination_Image_File = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , Path_Part_Destination_Without_Ext);
    //  //    #endregion Upload and Save a png regardlsess of image file type

    //  //    Image_Single.ImageUrl = Path_Full_Destination_Image_File;

    //  //    #region Redirect to force reloading of image on the page
    //  //    Url += "&Destination=" + RadioButtonList_Destination_Path.SelectedValue.ToString();
    //  //    Url += "&Msg= The image was uploaded to the destination path."
    //  //        + " You may have to refresth to view the new uploaded image.";
    //  //    #endregion Redirect to force reloading of image on the page
    //  //    #endregion Single
    //  //  }
    //  //  else if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "Profile")
    //  //  {
    //  //    #region Profile
    //  //    Image_500.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians500\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_500_Profile
    //  //          , 700
    //  //          );
    //  //    Image_400.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians400\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_400_Profile
    //  //          , 550
    //  //          );
    //  //    Image_300.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians300\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_300_Profile
    //  //          , 400
    //  //          );
    //  //    Image_200.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians200\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_200_Profile
    //  //          , 275
    //  //          );

    //  //    Url += "&Msg=The image was uploaded and resized for the PROFILE Pages."
    //  //        + " You may have to refresth to view the new uploaded and resized images.";
    //  //    #endregion Profile
    //  //  }
    //  //  else if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "Headshot")
    //  //  {
    //  //    #region Headshot
    //  //    Image_100.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians100\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_100_Headshot
    //  //          , 100
    //  //          );
    //  //    Image_75.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians75\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_75_Headshot
    //  //          , 75
    //  //          );
    //  //    Image_50.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians50\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_50_Headshot
    //  //          , 50
    //  //          );
    //  //    Image_35.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians35\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_35_Headshot
    //  //          , 35
    //  //          );
    //  //    Image_25.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians25\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_25_Headshot
    //  //          , 25
    //  //          );
    //  //    Image_20.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians20\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_20_Headshot
    //  //          , 20
    //  //          );
    //  //    Image_15.ImageUrl = db.Image_File_From_Http_Posted_File_As_Png(
    //  //           Request.Files["ImageFile"]
    //  //          , @"images\Politicians15\" + TextBox_Destination_Image_Name.Text.Trim()
    //  //          , db.Image_Size_15_Headshot
    //  //          , 15
    //  //          );

    //  //    Url += "&Msg=The image was uploaded and resized for the HEADSHOT Pages."
    //  //        + " You may have to refresth to view the new uploaded and resized images.";
    //  //    #endregion Headshot
    //  //  }
    //  //  else if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "Photo")
    //  //  {
    //  //    #region Photo images in Database
    //  //    db.PoliticiansImages_Update_All(
    //  //       TextBox_Destination_Image_Name.Text.Trim()
    //  //      , Request.Files["ImageFile"]
    //  //      );

    //  //    Url += "&Msg= The image was uploaded and the updated for ALL images in PoliticiansImages Table.";
    //  //    #endregion Photo images in Database
    //  //  }
    //  //  else if (RadioButtonList_Upload_Type.SelectedValue.ToString() == "PoliticianPhoto")
    //  //  {
    //  //    #region Politician Photo all images in Database
    //  //    #endregion Politician Photo all images in Database
    //  //  }

    //  //  Response.Redirect(db.Fix_Url_Parms(Url));

    //  //}
    //  //catch (Exception ex)
    //  //{
    //  //  Msg.Text = db.Fail(ex.Message);
    //  //  db.Log_Error_Admin(ex);
    //  //}
    //}

    ////protected void Load_Database_Images()
    ////{
    ////  TableProfile.Visible = true;
    ////  TableHeadshot.Visible = true;

    ////  Image_Original.ImageUrl = db.Url_Image_Politician_Original(
    ////    TextBox_Image_Name.Text.Trim()
    ////  );

    ////  Image_15.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_15_Headshot
    ////  );

    ////  Image_20.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_20_Headshot
    ////  );

    ////  Image_25.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_25_Headshot
    ////  );

    ////  Image_35.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_35_Headshot
    ////  );

    ////  Image_50.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_50_Headshot
    ////  );

    ////  Image_75.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_75_Headshot
    ////  );

    ////  Image_100.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_100_Headshot
    ////  );

    ////  Image_200.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_200_Profile
    ////  );

    ////  Image_300.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_300_Profile
    ////  );

    ////  Image_400.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_400_Profile
    ////  );

    ////  Image_500.ImageUrl = db.Url_Image_Politician(
    ////    TextBox_Image_Name.Text.Trim()
    ////  , db.Image_Size_500_Profile
    ////  );
    ////}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    if (!SecurePage.IsSuperUser)
    //      SecurePage.HandleSecurityException();

    //    //try
    //    //{
    //    //  #region Images to display
    //    //  if (!string.IsNullOrEmpty(db.QueryString("Id")))
    //    //    TextBox_Destination_Image_Name.Text = db.QueryString("Id");

    //    //  if (!string.IsNullOrEmpty(db.QueryString("Images")))
    //    //    RadioButtonList_Upload_Type.SelectedValue = db.QueryString("Images");

    //    //  if (!string.IsNullOrEmpty(db.QueryString("Destination")))
    //    //    RadioButtonList_Destination_Path.SelectedValue = db.QueryString("Destination");


    //    //  if (!string.IsNullOrEmpty(db.QueryString("Msg")))
    //    //  {
    //    //    //TextBox_Destination_Image_Name.Enabled = true;
    //    //    Msg.Text = db.Msg(db.QueryString("Msg"));
    //    //  }
    //    //  else
    //    //  {
    //    //    //TextBox_Destination_Image_Name.Enabled = false;
    //    //    Msg.Text = db.Msg("Select Image Destination(s).");
    //    //  }

    //    //  RadioButtonList_Destination_Path.Enabled = false;

    //    //  TableSingle.Visible = false;
    //    //  if (db.QueryString("Images") == "Single")
    //    //  {
    //    //    TableSingle.Visible = true;
    //    //    RadioButtonList_Destination_Path.Enabled = true;

    //    //    Image_Single.ImageUrl = Url_Image_Single();

    //    //    Msg.Text = db.Msg("This is the Non-Resized SINGLE image for PoliticianKey= "
    //    //      + db.QueryString("Id")
    //    //      + ": " + db.Politician_Name(db.QueryString("Id")));
    //    //  }

    //    //  TableProfile.Visible = false;
    //    //  TableHeadshot.Visible = false;
    //    //  if (db.QueryString("Images") == "Profile")
    //    //  {
    //    //    TableProfile.Visible = true;

    //    //    Image_500.ImageUrl = @"/images/Politicians500/" + db.QueryString("Id") + ".png";
    //    //    Image_400.ImageUrl = @"/images/Politicians400/" + db.QueryString("Id") + ".png";
    //    //    Image_300.ImageUrl = @"/images/Politicians300/" + db.QueryString("Id") + ".png";
    //    //    Image_200.ImageUrl = @"/images/Politicians200/" + db.QueryString("Id") + ".png";

    //    //    Msg.Text = db.Msg("These are the PROFILE images for PoliticianKey= "
    //    //      + db.QueryString("Id")
    //    //      + ": " + db.Politician_Name(db.QueryString("Id")));
    //    //  }

    //    //  else if (db.QueryString("Images") == "Headshot")
    //    //  {
    //    //    TableHeadshot.Visible = true;

    //    //    Image_100.ImageUrl = @"/images/Politicians100/" + db.QueryString("Id") + ".png";
    //    //    Image_75.ImageUrl = @"/images/Politicians75/" + db.QueryString("Id") + ".png";
    //    //    Image_50.ImageUrl = @"/images/Politicians50/" + db.QueryString("Id") + ".png";
    //    //    Image_35.ImageUrl = @"/images/Politicians35/" + db.QueryString("Id") + ".png";
    //    //    Image_25.ImageUrl = @"/images/Politicians25/" + db.QueryString("Id") + ".png";
    //    //    Image_20.ImageUrl = @"/images/Politicians20/" + db.QueryString("Id") + ".png";
    //    //    Image_15.ImageUrl = @"/images/Politicians15/" + db.QueryString("Id") + ".png";

    //    //    Msg.Text = db.Msg("These are the HEADSHOT images for PoliticianKey= "
    //    //      + db.QueryString("Id")
    //    //      + ": " + db.Politician_Name(db.QueryString("Id")));
    //    //  }
    //    //  else if (db.QueryString("Images") == "Photo")
    //    //  {
    //    //    Load_Database_Images();
    //    //  }
    //    //  #endregion Images to display
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //  Msg.Text = db.Fail(ex.Message);
    //    //  db.Log_Error_Admin(ex);
    //    //}
    //  }
    //}

    //protected void TextBox_Image_Name_TextChanged(object sender, EventArgs e)
    //{
    //  //try
    //  //{
    //  //  #region Checks
    //  //  if (string.IsNullOrEmpty(TextBox_Image_Name.Text.Trim()))
    //  //    throw new ApplicationException("The PoliticianKey is empty.");
    //  //  if (
    //  //    (TextBox_Image_Name.Text.Trim().ToLower() != "nophoto")
    //  //    && (!db.Is_Valid_Politician(TextBox_Image_Name.Text.Trim()))
    //  //    )
    //  //    throw new ApplicationException("The PoliticianKey (or NoPhoto) is not valid.");

    //  //  Load_Database_Images();

    //  //  Msg.Text = db.Ok("These are the images for the PoliticianKey.");
    //  //  #endregion Checks

    //  //}
    //  //catch (Exception ex)
    //  //{
    //  //  Msg.Text = db.Fail(ex.Message);
    //  //  db.Log_Error_Admin(ex);
    //  //}

    //}


  }
}
