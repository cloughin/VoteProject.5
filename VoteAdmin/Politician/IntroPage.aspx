<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
CodeBehind="IntroPage.aspx.cs" Inherits="Vote.Politician.IntroPage" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register Src="../NavBarAdmin.ascx" TagName="NavBar" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Intro</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
  <meta content="False" name="vs_showGrid" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  <script language="javascript">
    function postPoliticianIntroPage(where) {
      document.postForm.action = "/Intro.aspx";
      document.postForm.Id.value = where;
      document.postForm.target = "view";
      document.postForm.submit();
    }
  </script>
  <script language="javascript">
    function postGooglePage(page, issue, office) {
      document.postGoogleForm.action = "http://www.google.com/addurl/";
      document.postGoogleForm.target = "_SearchEngines";
      document.postGoogleForm.submit();
    }
    function ButtonUploadPicture_onclick() {

    }

  </script>
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: normal;
      color: #373737;
      font-size: 11px;
      height: 26px;
      padding-left: 5px;
      padding-right: 0px;
      padding-top: 0px;
      padding-bottom: 0px;
    }
  </style>
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <user:Banner ID="Banner" runat="server" />
  <uc2:NavBar ID="NavBar" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
    <tr>
      <td valign=top>
        <!-- Table -->
        <table id="TableInstructions" cellspacing="0" cellpadding="0">
          <tr>
            <td align="left" class="tdIntroPoliticianName">
              <asp:Label ID="PoliticianName" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="tdIntroPoliticianStatus">
              <asp:Label ID="PoliticianOffice" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="tdIntroPoliticianStatus">
              <asp:Label ID="PoliticianElection" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td valign="top" align="left">
              <asp:Label ID="Msg" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              <br />
              This form is used to provide the information on your introduction page. Click the
              link below to see this page that we provide voters about your candidacy.
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              <asp:HyperLink ID="HyperLinkViewIntro" runat="server" CssClass="HyperLink" Target="view">View Your Introduction Page that We Provide to Voters</asp:HyperLink>
              <br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              In addition to your introduction page, we provide voters with pages communicating
              your views and positions on numerous issues. These pages are automaticlly created
              when you respond to any issue or issue question. If you click the link below, you
              will be presented with a form where you will be able to add, change or delete your
              views and positions on various issues of your choosing. You may want to do this
              after you complete this form.
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              <asp:HyperLink ID="HyperLinkViews" runat="server" CssClass="HyperLink">Enter Your Views and Positions on Issues</asp:HyperLink>
              <br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              Use the link below to submit your introduction page to the Google search engine.
            </td>
          </tr>
          <tr>
            <td align="left" class="T" valign="top">
              <asp:HyperLink ID="HyperLinkSearchEngineIntroPage" runat="server" CssClass="HyperLink">Submit Your Introduction Page to the Google Search Engine</asp:HyperLink>
              <br />
              &nbsp;
            </td>
          </tr>
        </table>
      </td>
      <td align="right" valign="top">
        <asp:Image ID="CandidateImage" runat="server" CssClass="CandidateImage"></asp:Image>
      </td>
    </tr>
  </table>
  <!-- TableYourPicture -->
  <table class="tableAdmin" id="TableYourPicture" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" align="center">
        Your Picture
        on Pages</td>
    </tr>
    <tr>
      <td align="left" class="TBold">
        <table>
          <tr>
            <td valign="top" class="T">
              To upload a picture for your Introduction Page click <b>Browse</b> to select a image
              picture file on your PC to upload. The image type (extension) can be <strong>.bmp,
                .gif, .jpg, .png, or .tif.</strong><br />
              After you have selected your picture (Open Button) on the dialog presented, click
              <b>Upload Your Picture</b> .
              <br />
              <strong>Note:</strong> The picture you upload may be resized to better fit your
              introduction page. The picture presented on your introduction page is shown at 
              the top of thes form. Click the link entitled &quot;View Your Introduction Page Voters
              See&quot; at the top of this form to view the actual complete public page.
              <br />
            </td>
          </tr>
        </table>
        &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <input id="PictureFile" type="file" name="PictureFile" runat="server" size="100"
          class="TextBoxInput"><br />
        <input class="Buttons" id="ButtonUploadPicture" type="submit" value="Upload Your Picture"
          name="ButtonUploadPicture" runat="server" onserverclick="ButtonUploadPicture_ServerClick1"
          style="width: 200px" onclick="return ButtonUploadPicture_onclick()">
      </td>
    </tr>
    <tr>
      <td align="left">
      </td>
    </tr>
  </table>



  <!-- Table -->
  <table class="tableAdmin" id="TableDOB" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" align="left">
        Your Introduction Page Information</td>
    </tr>
    <tr>
      <td class="T" align="left">
        The information provided to voters on ballots, reports and your introduction page
        is shown in the textboxes on this form. Make any desired additions, changes, or
        deletions in the textboxes provided.
      </td>
    </tr>
    <tr>
      <td class="TSmallColor" align="left">
        The email address may be entered with or without the mailto: Select <strong>Enter</strong>
        after each data item is entered in a textbox to have the data recorded.
      </td>
    </tr>
    <tr>
      <td class="HSmall" align="left">
        Your Date of Birth
      </td>
    </tr>
    <tr>
      <td class="TSmall" align="left">
        Date of birth can be entered in any standard format, like 2/17/1953 or Feb 17, 1953
        or February 17, 1953
      </td>
    </tr>
    <tr>
      <td class="TBold" align="left">
        <strong>Date of Birth:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDateOfBirth" runat="server" Width="257px" CssClass="TextBoxInput"
            AutoPostBack="True" OnTextChanged="TextBoxDateOfBirth_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          Age:
          <asp:Label ID="Age" runat="server"></asp:Label>
        </strong>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableWebAddresses" cellspacing="0" cellpadding="0">
    <tr>
      <td class="HSmall" colspan="2" align="left">
        Your Campaign Website Internet Addresses and Other Websites About Yourself
      </td>
    </tr>
    <tr>
      <td align="left" class="TSmall" colspan="2">
        You can obtain any Internet addresses by right clicking on the link or image, selecting
        &quot;Copy Shortcut&quot; in an IE browser or &quot;Copy Link Location&quot; in
        Firefox browser, and then pasting the shortcut in the textbox provided.
      </td>
    </tr>
    <tr>
      <td class="TSmallColor" colspan="2" align="left">
        All Internet addresses may be entered with or without the http://. Select <strong>
          Enter</strong> after each address is entered in the textbox to have the address
        recorded.
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" style="width: 142px">
        <nobr>
          <asp:HyperLink ID="HyperLink_Website" runat="server" Target="view">Candidate Website:</asp:HyperLink></nobr>
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxWebSite" runat="server" Width="770px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxWebSite_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <asp:PlaceHolder ID="SocialMediaPlaceHolder" runat="server"></asp:PlaceHolder>
  </table>
  <!-- YouTube Video -->
  <!--
  <table class="tableAdmin" id="TableYouTube" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HSmall" colspan="2">
        Your YouTube Video for Your Introduction Page
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        If you have a video currently on YouTube enter the YouTube address in textbox below
        and hit Enter.
      </td>
    </tr>
    <tr id="trYouTubeInstruction" runat="server">
      <td align="left" class="T" colspan="2">
        Or click the link below to be presented with a form where you will be able to upload
        a video to YouTube and have it used on your introduction page.
      </td>
    </tr>
    <tr id="trYouTubeHyperlink" runat="server">
      <td align="left" class="T" colspan="2">
        <asp:HyperLink ID="HyperLinkYouTubeUpload" runat="server" CssClass="HyperLink">Upload a Video on My Computer to YouTube</asp:HyperLink>
      </td>
    </tr>
  </table>
  -->
  <!-- Your Contact Information -->
  <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HSmall" colspan="2">
        Your Contact Information
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <nobr>
          <strong>Candidate Email:</strong></nobr>
      </td>
      <td align="left" class="T" colspan="">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmail" runat="server" Width="400px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxEmail_TextChanged"></user:TextBoxWithNormalizedLineBreaks><strong>Phone:<user:TextBoxWithNormalizedLineBreaks
            ID="TextBoxPhone" runat="server" Width="250px" CssClass="TextBoxInput" AutoPostBack="True"
            OnTextChanged="TextBoxPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks></strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" style="width: 142px">
        <strong>Street Address:</strong>
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddress" runat="server" Width="400px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" style="width: 142px">
        <strong>City, State Zip: </strong>
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCityStateZip" runat="server" Width="400px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCityStateZip_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>

  <!-- TableS Moved-->
  <table class="tableAdmin" id="TableName" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="TSmallColor">
        In this section, select <strong>Enter</strong> after each data item is entered in
        a textbox to have the data recorded.
      </td>
    </tr>
  </table>

  <!-- Table Move -->
  <table class="tableAdmin" id="TableName2" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" colspan=5 class="HSmall">
        Your Name on Ballots and Reports
        <asp:Label ID="LabelCandidateName" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" colspan=5 class="T">
        Your name which is shown on ballots was provided by the election authority and is
        shown above and can only be changed by our election manager. Quotes or parens are
        not shown in the textboxes for any parts of the name. These will be automatically
        added according to the way ballot names are presented.
      </td>
    </tr>
    <tr>
      <td class="HSmall" valign="top">
        First Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top">
        Middle Name<br />
        or Initial
      </td>
      <td class="HSmall" valign="top">
        Nickname without<br />
        quotes or parens
      </td>
      <td class="HSmall" valign="top">
        Last Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top">
        Suffix:
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFirst" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxFirst_TextChanged" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMiddle" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxMiddle_TextChanged" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxNickName" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxNickName_TextChanged" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLast" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxLast_TextChanged" Width="140px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxSuffix" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxSuffix_TextChanged" Width="45px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TablePartyCode" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td class="HSmall" colspan="2" valign="top" style="height: 20px">
        Your Political Party and Code on Ballots
        <asp:Label ID="Party" runat="server" CssClass="TBoldColor"></asp:Label>&nbsp;
        <asp:Label ID="PartyCode" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        Use this drop down list to identify or change your political party.
      </td>
      <td align="right" valign="top" class="T">
        <asp:DropDownList ID="DropDownListParty" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListParty_SelectedIndexChanged">
        </asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
      </td>
      <td align="right" class="T">
        <asp:HyperLink ID="HyperlinkPartyKey" runat="server" Target="view" CssClass="TBoldColor">Political Party Website</asp:HyperLink>
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" id="TableGeneral" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H">
        Your Biographical Information</td>
    </tr>
    <tr>
      <td class="T">
        There are nine sections below with texboxes where you can provide biographical information.
        In each section, the information we have on file is shown in the textbox. Click
        anywhere outside the textbox to record any addition, change or deletion you make.
        Selecting the Enter WILL NOT perform your update.
      </td>
    </tr>
    <tr>
      <td class="TBoldColor">
        In this section you need to click anywhere outside a textbox to have the contents
        of the textbox recorded. Using the Enter key WILL NOT record your response because
        the Enter key only starts a new paragraph.
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        General: (political statement of goals, objectives, views, philosophies)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 2,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxGeneral" runat="server" TextMode="MultiLine" Height="160px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" 
          OnTextChanged="TextBoxGeneral_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall" align="left">
        Personal: (Gender, age, marital status, spouse's name and age, children's name and
        ages, home town, current residence, family background)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPersonal" runat="server" TextMode="MultiLine" Rows="3" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxPersonal_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Education: (times and places of schools, colleges, major, degrees, activities, sports)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEducation" runat="server" TextMode="MultiLine" Rows="2" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxEducation_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Occupation / Profession: (occupation&nbsp; or profession and work experience outside
        politics)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxProfession" runat="server" TextMode="MultiLine" Rows="2"
          Height="80px" Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True"
          OnTextChanged="TextBoxProfession_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    </tr>
    <tr>
      <td class="HSmall">
        Military: (branch, years of service, active duty experience, highest rank, medals,
        honors, discharge date and type)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMilitary" runat="server" TextMode="MultiLine" Rows="2" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxMilitary_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall" style="height: 13px">
        Civic: (past and present organizations, charities involvement)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCivic" runat="server" TextMode="MultiLine" Rows="2" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxCivic_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Political: (dates and titles of previously held political offices, sponsored and
        co-sponsored legislation)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPolitical" runat="server" TextMode="MultiLine" Rows="2" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxPolitical_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Religion: (current and past religious affiliations, beliefs)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxReligion" runat="server" TextMode="MultiLine" Rows="2" Height="80px"
          Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextBoxReligion_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Accomplishments: (significant accomplishments, awards, achievements)
      </td>
    </tr>
    <tr>
      <td class="T">
        Your entry should not exceed 1,000 characters, which is about what can be entered
        in the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAccomplishments" runat="server" TextMode="MultiLine" Rows="2"
          Height="80px" Width="930px" CssClass="TextBoxInputMultiLine" AutoPostBack="True"
          OnTextChanged="TextBoxAccomplishments_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  </form>
  <form name="postForm" method="get">
  <input id="Id" type="hidden" name="Id">
  </form>
  <form name="postGoogleForm" method="get">
  <input type="hidden">
  </form>
</body>
</html>
--%>