<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="Answer.aspx.cs" Inherits="Vote.Politician.Answer" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register Src="../NavBarAdmin.ascx" TagName="NavBar" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
		<title>Answer to an Issue Question</title>
		<link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
    <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:NavBar ID="NavBar" runat="server" />
    <!-- Table -->
			<table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="tdIntroPoliticianName">
          <asp:Label ID="PoliticianName" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="tdIntroPoliticianStatus">
          <asp:Label ID="PoliticianOffice" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="tdIntroPoliticianStatus">
          <asp:Label ID="PoliticianElection" runat="server"></asp:Label></td>
      </tr>
				<tr>
					<td align="left" class="HLarge">&nbsp;<asp:label id="Question" runat="server" ForeColor="Red"></asp:label></td>
				</tr>
				<tr>
					<td align="left"><asp:label id="Msg" runat="server"></asp:label></td>
				</tr>
			</table>
    <!-- Table -->
    <!-- Table -->
			<table class="tableAdmin" id="TableDataSource" cellspacing="0" cellpadding="0" runat="server">
        <tr>
          <td align="left" class="H">
            Answer Source and Date</td>
        </tr>
				<tr>
					<td align="left" class="HSmall">
            Source </td>
				</tr>
        <tr>
          <td align="left" class="T">
            Copy and paste the url of the web page (with or without http://) where you obtained
            the answer or a description of the source in the textbox below, i.e. a news article.</td>
        </tr>
				<tr>
					<td align="left" class="T">
            <user:TextBoxWithNormalizedLineBreaks id="TextboxSource" runat="server" Width="619px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</tr>
        <tr>
          <td align="left" class="HSmall">
            Date</td>
        </tr>
        <tr>
          <td align="left" class="T" style="height: 13px">
            Enter the date of the answer if today is not an appropriate date. For example
            if you obtained the answer from the candidate's campaign website, and the election
            is over, enter some date a couple days prior to the election. Any date format is
            acceptable, i.e. 10/20/2010.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxDate" runat="server" Width="324px" CssClass="TextBoxInput" OnTextChanged="TextBoxDate_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
        </tr>
        <tr>
          <td align="left" class="HSmall">
            Change Source or Date</td>
        </tr>
        <tr>
          <td align="left" class="T">
            If the answer was provided previously, and the source or date above are incorrect,
            change either or both, and then:<br />
            1) Click the button below; 2) Click the 'Return Without Recording Response Above'.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:Button ID="Button_Change_Source_Date" runat="server" CssClass="Buttons" OnClick="Button_Change_Source_Date_Click"
              Text="Change Source and/or Date" /></td>
        </tr>
			</table>
    <!-- Table -->
			<table class="tableAdmin" id="TableAnswer" cellspacing="0" cellpadding="0">
				<tr>
					<td align="left" class="H">
            Add or Change Response</td>
				</tr>
        <tr>
          <td align="left" class="T">
            <strong>Add or Change Response:</strong> Use the texbox below to add or change your response to the issue question above. Your entry should not exceed 2,000 characters,
            which is about what can be entered in the textbox. Click the SUBMIT Button to have your new or edited response recorded. You can delete your response by clicking the
            DELETE Button in the lower right corner. If we have no response, the reports we
            provide will show as 'No Response' to this question. After you add, change or delete
            your response, you will then be presented with a form where you can continue providing
            answers to other issue questions.</td>
        </tr>
				<tr>
					<td align="left" class="T"><user:TextBoxWithNormalizedLineBreaks id="TextBoxNewResponse" runat="server" 
              TextMode="MultiLine" Height="250px" Width="930px" 
              CssClass="TextBoxInputMultiLine" OnTextChanged="TextBoxNewResponse_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
				</tr>
        <tr>
          <td align="center" class="T">
            <asp:Button ID="ButtonSubmit" runat="server" CssClass="Buttons" Font-Bold="True"
              OnClick="ButtonSubmit_Click1" Text="SUBMIT Response Above and Answer More Questions" Width="320px" /></td>
        </tr>
        <tr>
          <td align="left" class="T">
            Use the two buttons below to either clear or load the current response in the textbox above without returning to answer more questions.
            These operations may be helpful in either starting over
            or making minor changes to your respone.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:button id="ButtonClear" runat="server" CssClass="Buttons" Text="Clear Response in Textbox Above" Font-Bold="True" OnClick="ButtonClear_Click1" Width="320px"></asp:button>&nbsp;
            <asp:button id="ButtonReload" runat="server" CssClass="Buttons" Text="Load Textbox with Current Response" Font-Bold="True" OnClick="ButtonReload_Click1" Width="320px"></asp:button></td>
        </tr>
        <tr>
          <td align="left" class="T">
            The button on the left
            will return without recording any changes, leaving your response
            as is. The button on the right will erase your response to this question
            and show 'No Response' as your answer and then return to answer additional questions.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:Button ID="ButtonReturn" runat="server" CssClass="Buttons" OnClick="ButtonReturn_Click"
              Text="Return without Recording Response Above" Width="320px" />&nbsp;
            <asp:button id="ButtonDelete" runat="server" CssClass="Buttons" Text="DELETE Any Response to this Question then Return" Font-Bold="True" OnClick="ButtonDelete_Click1" Width="320px"></asp:button></td>
        </tr>
			</table>
			<table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" runat="server">
        <tr>
          <td align="left" class="H">
            Videos Currently on YouTube</td>
        </tr>
				<tr>
					<td align="left" class="T">
            Use the textboxes below to provide a YouTube video that is currently on YouTube 
            which answers the question or to change of delete an existing video.</td>
				</tr>
				<tr>
					<td align="left" class="HSmall">
            YouTube Link </td>
				</tr>
        <tr>
          <td align="left" class="T">
            To add or change a video paste the YouTube url of the video (with or without http://) in the textbox below.</td>
        </tr>
				<tr>
					<td align="left" class="T">
            <user:TextBoxWithNormalizedLineBreaks id="YouTubeUrlTextBox" runat="server" Width="619px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</tr>
        <tr id="trYouTubeCaption" runat=server>
          <td align="left" class="HSmall">
            YouTube Caption (optional)</td>
        </tr>
        <tr id="trYouTubeInstruction" runat=server>
          <td align="left" class="T">
            Enter an optional caption for the video of 100 characters or less. For example: 
            Bloomberg GOP debate from New Hampshire. </td>
        </tr>
        <tr id="trYouTubeTextBox" runat=server>
          <td align="left" class="T">
            <user:TextBoxWithNormalizedLineBreaks id="YouTubeCaptionTextBox" runat="server" Width="924px" 
              CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
            </td>
        </tr>
        <tr>
          <td align="left" class="HSmall">
            YouTube Date</td>
        </tr>
        <tr>
          <td align="left" class="T" style="height: 13px">
            Enter the date of the YouTube video if today is not an appropriate date. Any date format is
            acceptable, i.e. 10/20/2010.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="YouTubeDateTextBox" runat="server" Width="324px" CssClass="TextBoxInput" ></user:TextBoxWithNormalizedLineBreaks></td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:Button ID="SubmitYouTubeUrlButton" runat="server" CssClass="Buttons" 
              Text="Submit YouTube Url and Date" Width="200px" 
              onclick="SubmitYouTubeUrlButton_Click" />&nbsp;
            <asp:Button ID="RemoveYouTubeUrlButton" runat="server" CssClass="Buttons" 
              Text="Remove YouTube Url and Date" Width="200px" 
              onclick="RemoveYouTubeUrlButton_Click" /></td>
        </tr>

			</table>
			<table class="tableAdmin" id="TableYouTubeUpload" cellspacing="0" cellpadding="0" runat="server">

        <tr>
          <td align="left" class="H">
            Videos to Upload to YouTube</td>
        </tr>
        <tr>
          <td align="left" class="T">
            The link below will present a form where you will be able to upload a video to 
            YouTube which answers the question.</td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:HyperLink ID="HyperLinkYouTubeUpload" runat="server" CssClass="HyperLink">Upload a Video on My Computer to YouTube</asp:HyperLink>
          </td>
        </tr>
			</table>
		</form>
	</body>
</html>
--%>