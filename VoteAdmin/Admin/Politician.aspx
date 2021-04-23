<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="Politician.aspx.cs"
 MasterPageFile="~/Master/Admin.Master"Inherits="Vote.Admin.PoliticianPage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>

<%--
<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
      <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HSmall">
            <asp:Label ID="PageTitle" runat="server"></asp:Label>
          </td>
        </tr>
        <tr>
          <td align="left" class="T">
            <asp:Label ID="Msg" runat="server"></asp:Label>
          </td>
        </tr>
        <tr>
          <td align="right" class="TSmall">
             <asp:Label ID="PoliticianID" runat="server" CssClass="TSmall"></asp:Label>
          </td>
        </tr>
      </table>
      <!-- Table Ballot Name -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="H" colspan="6">
        Ballot Name
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="6">
        Candidate Name on Ballots:
        <asp:Label ID="LabelCandidateName" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="6">
        <strong>Candidate Name:</strong> The candidate's name should be exactly how it should
        appear on ballots, including casing. Do NOT include quotes or parens in any parts
        of the name. These will be automatically added on ballots.
      </td>
    </tr>
    <tr>
      <td class="HSmall" valign="top" width="150">
        First Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top" width="150">
        Middle Name<br />
        or Initial
      </td>
      <td class="HSmall" valign="top" width="150">
        Nickname without<br />
        quotes or parens
      </td>
      <td class="HSmall" valign="top" width="160">
        Last Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top" width="65">
        Suffix:
      </td>
      <td id="tdANCText" class="HSmall" valign="top" runat="server" width="45">
        DC<br />
        ANC
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
      <td valign="top" id="tdANCTextBox" runat="server">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddOn" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxAddOn_TextChanged" Width="100px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr id="tr_AddPolitician" runat=server>
      <td valign="top" class="T" colspan=1>
        <asp:Button ID="Button_Add_Politician" runat="server" CssClass="Buttons" 
          Text="Add Politician" onclick="Button_Add_Politician_Click" Width="140px" />
      </td>
      <td valign="top" colspan=5 class="T">
        You should make sure the politician you are about to add is not already in the 
        database. Then the politician&#39;s name exactly as it should appear on ballots. You 
        may enter names entirely in upper case and with or without quotes and/or 
        parenthesis. When you click the Add Politician Button the politician will either 
        be added to the database or you will get an error message if a unique ID could 
        not be created. </td>
    </tr>
  </table>
  <!-- Table Last Office Held or Sought -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="H" colspan="2">
        Last Office Held or Sought
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2">
        <asp:HyperLink ID="HyperLinkOfficeTitleLine1Line2" runat="server" CssClass="TBoldColor"
          Target="_office">[HyperLinkOfficeTitleLine1Line2]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        The link above will provide a form to edit the office and identify the current incumbent(s)
        for the office.
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2">
        Change Office Politician is Holding or Identify Current Incumbent
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        To change the office this politician is holding, seeking or last sought, copy and
        paste the correct Office ID in the text box provided and then click anywhere on
        this form. Use the Find Office ID link to find the Office ID.<br />
        <strong>Note: </strong>To obtain the current incumbent(s) for this office click
        the office title linke above.
      </td>
    </tr>
    <tr>
      <td class="TBold">
        Office ID:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeKey" runat="server" CssClass="TextBoxInput" Width="450px"
          AutoPostBack="True" OnTextChanged="TextBoxOfficeKey_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <asp:HyperLink ID="HyperLinkOfficeIDs" runat="server" CssClass="HyperLink" Target="_offices"
          Width="130px"><nobr>Find Office ID</nobr></asp:HyperLink>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Party_Code" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="H" colspan="2" valign="top">
        Political Party
      </td>
    </tr>
    <tr>
      <td class="HSmall" valign="top">
        <asp:Label ID="Party" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
      <td align="right" class="HSmall" colspan="1" valign="top">
        Code Shown on Ballots:
        <asp:Label ID="PartyCode" runat="server" CssClass="TBoldColor"></asp:Label>&nbsp;
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <strong>Change or Identify Political Party:</strong> Click this drop down list to
        identify or change the political party. A candidate's political party is the political
        party organization in the State of the politician. National political party organizations
        are not listed and can not be selected.&nbsp;
      </td>
      <td align="left" colspan="1" valign="top" class="T">
        <asp:DropDownList ID="DropDownListParty" runat="server" CssClass="HSmall" Width="300px"
          AutoPostBack="True" OnSelectedIndexChanged="DropDownListParty_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <asp:HyperLink ID="HyperlinkPartyKey" runat="server" Target="view" CssClass="TBold">Political Party Website</asp:HyperLink>
      </td>
    </tr>
  </table>
  <!-- Table Politician Incumbent Status-->
  <table class="tableAdmin" id="Table_Incumbent_Status" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="H" valign="top">
        Elected Official - Incumbent Status
      </td>
    </tr>
    <tr>
      <td class="HSmall" valign="top">
        <asp:CheckBox ID="CheckBoxIsIncumbent" runat="server" AutoPostBack="True" CssClass="TBoldColor"
          OnCheckedChanged="CheckBoxIsIncumbent_CheckedChanged" />
        <asp:Label ID="LabelIncumbentOffice" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" valign="top">
        Check or uncheck the checkbox above to change the current incumbent status of the
        politician for
        <asp:Label ID="OfficeTitle4Inumbent" runat="server"></asp:Label>.
      </td>
    </tr>
  </table>
  <!-- Table Addresses -->
  <table class="tableAdmin" id="Table_Address" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="H" valign="top" colspan="2">
        Addresses
      </td>
    </tr>
    <tr>
      <td class="TSmallColor" valign="top" colspan="2">
        The textboxes below contain the data provided by the politician or entered using
        this form previously. The textboxes below may be empty while data appears in the
        report of candidates above when data was obtained from some other source but none
        has yet been entered by the politician on using this form.
      </td>
    </tr>
    <tr>
      <td class="TBold" valign="top">
        Street Address
      </td>
      <td class="T" valign="top">
        <strong>Email Address:</strong> (Mailto: and Email: will be stripped off)
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStreetAddress" runat="server" Width="300px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxStreetAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmailAddress" runat="server" Width="550px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxEmailAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <strong>City, State Zip</strong>
      </td>
      <td class="T" valign="top">
        <strong>Web Address:</strong> (http: https and Url: will be stripped off)
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCityStateZipCode" runat="server" Width="300px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCityStateZipCode_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxWebAddress" runat="server" Width="550px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxWebAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <strong>Phone:</strong>
        <br>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPhone" runat="server" Width="300px" CssClass="TextBoxInput"
          OnTextChanged="TextBoxPhoneNum_TextChanged" AutoPostBack="True"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top" class="T">
        Both the street address and city, state zip need to be provided to be included on
        any reports.
      </td>
    </tr>
  </table>
  <!-- Table Edit Office -->
  <table class="tableAdmin" id="Table_Edit_Office" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td align="left" class="H" colspan="3" valign="top">
        Edit Office
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <br />
        Use the link below if to edit this office including changing the number of office
        positions or identifying the office incumbent(s).
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <asp:HyperLink ID="HyperLinkOffice" runat="server" CssClass="HyperLink" Target="_office">[HyperLinkOffice]</asp:HyperLink><br />
        &nbsp;
      </td>
    </tr>
  </table>
  <!-- Table Politician Intro Data and Headshot (Master Users Only) -->
  <table class="tableAdmin" id="Table_Master_Politician_Intro" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" colspan="2" rowspan="1" valign="top">
        Intro &amp; Headshot (Master Users Only)
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2">
        <asp:Label ID="Label_Intro_Page" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        Use the picture link to edit the politician data or text link to view the politician&#39;s
        introduction page.
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2" rowspan="1" valign="top">
        <asp:HyperLink ID="HyperLink_PoliticiansImages" runat="server" CssClass="HyperLink"
          NavigateUrl="~/Master/ImagesHeadshots.aspx" Target="_edit">[HyperLink_PoliticiansImages]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2" rowspan="1" valign="top">
        Use the link above to view and upload this politician's profile and headshot images.
      </td>
    </tr>
  </table>
  <!-- Table Password & Campaign Addresses (Master Users Only)-->
  <table class="tableAdmin" id="Table_Password_Delete_Reinstate" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="H" colspan="2">
        Password & Campaign Addresses (Master Users Only)
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        The content of each textbox is recorded in the Politicians Table as it is entered
        or changed by clicking anywhere outside a textbox.
      </td>
    </tr>
    <tr>
      <td align="left" colspan="2" class="T">
        <strong>Password:&nbsp;</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCandidatePassword" runat="server" Enabled="False" Width="150px"
          CssClass="TextBoxInput" AutoPostBack="True" OnTextChanged="TextBoxCandidatePassword_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        Password Hint:&nbsp;
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPasswordHint" runat="server" Enabled="False" Width="150px"
          CssClass="TextBoxInput" AutoPostBack="True" OnTextChanged="TextBoxPasswordHint_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2" valign="top">
        Campaign Information
      </td>
    </tr>
    <tr>
      <td class="T" valign="top" colspan="2">
        Email:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCampaignEmail" runat="server" Width="300px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCampaignEmail_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        Phone:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCampaignPhone" runat="server" Width="243px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCampaignPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2" valign="top">
        Contact:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCampaignName" runat="server" Width="290px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCampaignName_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2" valign="top">
        Street:&nbsp;
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCampaignAddr" runat="server" Width="296px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCampaignAddr_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
        City, State Zip:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCampaignCityStateZip" runat="server" Width="257px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCampaignCityStateZip_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>