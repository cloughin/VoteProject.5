<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Images.aspx.cs" 
Inherits="Vote.Master.ImagesPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Images Maintenance</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <user:LoginBar ID="LoginBar1" runat="server" />
      <user:Banner ID="Banner" runat="server" />
      <uc2:Navbar ID="Navbar" runat="server" />
      <!-- Table -->
      <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HLarge">
            Images Maintenance</td>
        </tr>
        <tr>
          <td align="left">
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
        <tr>
          <td align="left" class="TLargeColorBold">
            This form is no longer used.</td>
        </tr>
      </table>
      <!-- Table -->
      <table id="TableYourPicture" cellpadding="0" cellspacing="0" class="tableAdmin">
        <tr>
          <td class="H">
            Upload Image</td>
        </tr>
        <tr style="font-size: 11px; background-color: #ffffff">
          <td align="left" class="T">
            When an image file is uploaded it automatically converted to .png file type, if
            it is not a .png file (without regards to file size advantages of other file types).
            <br />
            <strong>Acceptable file types to upload are:</strong> <strong>.bmp, .gif, .jpg, .png,
              .tif.<br />
              Profile</strong> page images are limited by width (Intro.aspx:500x700, PoliticianIssue.aspx:400x550,
            Issue.aspx:300x400, 200x275)<br />
            <strong>Headshot</strong> pages are limited by height (Ballot.aspx:100x100, Reports.aspx:
            75x75, 50x50, 35x35, 25x25, 20x20, 15x15)</td>
        </tr>
        <tr style="font-size: 11px; background-color: #ffffff">
          <td align="left" class="T">
            <strong>Steps:</strong><br />
            </td>
        </tr>
        <tr style="font-size: 11px; background-color: #ffffff">
          <td align="left" class="H">
            Image Upload</td>
        </tr>
        <tr style="font-size: 11px; background-color: #ffffff">
          <td align="left" class="HSmall">
            Upload to Database or Folder</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            Select a destination folder.</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="TBold">
            <asp:RadioButtonList ID="RadioButtonList_Upload_Type" runat="server" AutoPostBack="True"
              CssClass="TBold" OnSelectedIndexChanged="RadioButtonList_Upload_Type_SelectedIndexChanged" Enabled="False">
              <asp:ListItem Value="Photo">Upload and Resize Politician or NoPhoto Image for All Profile and Headshot Images Store in PoliticiansImages, LogPoliticiansImagesOriginal Database Tables</asp:ListItem>
              <asp:ListItem Value="Single">Upload an Image WITHOUT Resizing and save in \image\folder selected below</asp:ListItem>
              <asp:ListItem Value="Profile" Enabled="False">Disabled: Upload and Resize for all Profile Pages (Intro.aspx PoliticianIssue.aspx)</asp:ListItem>
              <asp:ListItem Value="Headshot" Enabled="False">Disabled: Upload and Resize for all Headshot Pages (Issue.aspx, Ballot.aspx, Report.aspx)</asp:ListItem>
            </asp:RadioButtonList></td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="HSmall">
            Single Image \image\Folder
            Destination</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            <asp:RadioButtonList ID="RadioButtonList_Destination_Path" runat="server"
              CssClass="TBold" RepeatColumns="2" Enabled="False">
              <asp:ListItem Value="Candidates">\images\Candidates</asp:ListItem>
              <asp:ListItem Value="CandidatesSmall">\images\CandidatesSmall</asp:ListItem>
              <asp:ListItem Value="SampleBallots">\images\SampleBallots</asp:ListItem>
              <asp:ListItem Value="SampleFileTypesSizes">\images\SampleFileTypesSizes</asp:ListItem>
              <asp:ListItem Value="Test">\images\Test</asp:ListItem>
            </asp:RadioButtonList></td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="HSmall">
            Image Name (which is PoliticianKey or NoPhoto) without Extension </td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            Enter an image file name without the exenstion.<br />
            Browse to select the file to upload.<br />
            Click the Upload Image Button</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="TBold" style="height: 31px">
            PoliticianKey:
            <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Destination_Image_Name" runat="server"
              Width="200px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="HSmall">
            Source Image on PC</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            <input id="ImageFile" runat="server" class="TextBoxInput" name="ImageFile" size="95"
              type="file" /></td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            <input id="ButtonUploadPicture" runat="server" class="Buttons" name="ButtonUploadPicture"
              onserverclick="ButtonUploadPicture_ServerClick1" style="width: 222px" type="submit"
              value="Upload Image" /></td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="H" style="height: 13px">
            View Politician Images</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="T">
            Enter the PoliticianKey or NoPhoto, then Enter to view the politician images.</td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left" class="TBold">
            PoliticianKey or NoPhoto:
            <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Image_Name" runat="server" AutoPostBack="True" 
            OnTextChanged="TextBox_Image_Name_TextChanged"
              Width="200px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          </td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left">
          </td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left">
          </td>
        </tr>
        <tr style="font-size: 11px">
          <td align="left">
          </td>
        </tr>
      </table>
      <!-- Table -->
      <table id="TableHeadshot" cellpadding="0" cellspacing="0" class="tableAdmin" runat=server>
        <tr>
          <td align="left" class="HSmall" colspan="4">
            Headshot Images</td>
        </tr>
        <tr>
          <td align="left" class="TBold">
            25x25</td>
          <td align="left" class="TBold">
            20x20</td>
          <td align="left" class="TBold">
            15x15</td>
          <td align="left" class="TBold">
          </td>
        </tr>
        <tr>
          <td align="left" valign="top">
            <asp:Image ID="Image_25" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_20" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_15" runat="server" /></td>
          <td align="left" valign="top">
            </td>
        </tr>
        <tr>
          <td align="left" class="TBold">
            100x100</td>
          <td align="left" class="TBold">
            75x75</td>
          <td align="left" class="TBold">
            50x50</td>
          <td align="left" class="TBold">
            35x35</td>
        </tr>
        <tr>
          <td align="left" valign="top"><asp:Image ID="Image_100" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_75" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_50" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_35" runat="server" /></td>
        </tr>
      </table>
      <!-- Table -->
      <table id="TableProfile" cellpadding="0" cellspacing="0" class="tableAdmin" runat=server>
        <tr>
          <td align="left" class="HSmall" colspan="2">
            Profile Images</td>
        </tr>
        <tr>
          <td align="left" class="TBold" valign="top" style="height: 19px">
            300x400</td>
          <td align="left" class="TBold" valign="top" style="height: 19px">
            200x275</td>
        </tr>
        <tr>
          <td align="left" valign="top">
            <asp:Image ID="Image_300" runat="server" /></td>
          <td align="left" valign="top">
            <asp:Image ID="Image_200" runat="server" /></td>
        </tr>
        <tr>
          <td align="left" class="TBold">
            400x550</td>
          <td align="left" class="TBold">
            </td>
        </tr>
        <tr>
          <td align="left" style="height: 30px" valign="top" colspan="2">
            <asp:Image ID="Image_400" runat="server" /></td>
        </tr>
        <tr>
          <td align="left" class="TBold">
            500x700</td>
          <td align="left" class="TBold">
            </td>
        </tr>
        <tr>
          <td align="left" valign="top" colspan="2" style="height: 27px"><asp:Image ID="Image_500" runat="server" /></td>
        </tr>
        <tr>
          <td align="left" colspan="2" valign="top">
            <table id="Table1" cellpadding="0" cellspacing="0" class="tableAdmin" runat=server>
              <tr>
                <td align="left" class="TBold" colspan="2">
                  Original</td>
                <td align="left" class="TBold">
                </td>
              </tr>
            </table>
            <asp:Image ID="Image_Original" runat="server" /></td>
        </tr>
      </table>
      <!-- Table -->
      <!-- Table -->
      <table id="TableSingle" cellpadding="0" cellspacing="0" class="tableAdmin" runat=server>
        <tr>
          <td align="left" class="HSmall">
            Single Image</td>
        </tr>
        <tr>
          <td align="left">
            <asp:Image ID="Image_Single" runat="server" /></td>
        </tr>
      </table>
      <!-- Table -->
      <!-- Table -->
    </div>
    </form>
</body>
</html>
--%>