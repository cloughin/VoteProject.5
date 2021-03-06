<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="YouTubeUpload.aspx.cs" Inherits="Vote.Politician.YouTubeUpload" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
    <%--
      <h1 id="H1" runat="server"></h1>
  <!-- Table -->
  <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="tdIntroPoliticianName">
        <asp:Label ID="PoliticianName" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="HLarge">
        &nbsp;<asp:Label ID="QuestionDescription" runat="server" ForeColor="Red"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableYouTubeUpload" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="H">
        Video to Upload to YouTube
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Follow the instructions below to upload a video to YouTube and have it presented 
        as the answer to the question or purpose in the title above.</td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        YouTube Title (required)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Enter title for the video of 100 characters or less. For example:
        Bloomberg GOP debate from New Hampshire.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="YouTubeTitleTextBox" runat="server" Width="924px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        YouTube Description (optional)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Enter a description for the video.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks id="YouTubeDescriptionTextBox" runat="server" 
              TextMode="MultiLine" Height="250px" Width="930px" 
              CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        YouTube Date
      </td>
    </tr>
    <tr>
      <td align="left" class="T" style="height: 13px">
        Enter the date of the YouTube video if today is not an appropriate date. Any date
        format is acceptable, i.e. 10/20/2010.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="YouTubeDateTextBox" runat="server" Width="324px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="HSmall">
        Your Video</td>
    </tr>
  </table>
  <table id="Table3" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server">
    <tr>
      <td class="T">
        Browse to select a video file on your PC to upload. After you have selected your 
        video file (Open Button) on the dialog presented, click Upload File.
      </td>
    </tr>
    <tr>
      <td class="T">
        <b>File to upload:</b><br />
        <input id="UploadFile" runat="server" name="UploadFile" size="95" type="file" /><br />
        <br />
        <input id="UploadFileButton" runat="server" class="Buttons" name="UploadFileButton"
          style="width: 222px" type="submit" value="Upload File" onserverclick="ButtonUploadFile_ServerClick" />
      </td>
    </tr>
  </table>
  --%>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
