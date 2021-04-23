<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Parties.aspx.cs" Inherits="Vote.Admin.PartiesPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
    
  <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

  <div id="UpdateControls" class="update-controls" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
  <!-- Table -->
  <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HLarge">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableAdd" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td align="left" class="H" colspan="2">
        Add a Party
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:Label ID="Msg_Add" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Before you attempt to add a political party make certain it is not already in the
        Parties Report below. Enter a Ballot Code as it should appear on ballots, and the
        party name in the text boxes provided. Then, click the Add Button.
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" colspan="2" style="height: 13px">
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2" style="height: 13px">
        <strong>Order</strong> (order on dropdown list for politician):
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Order_Add" runat="server" CssClass="TextBoxInput" Width="30px"></user:TextBoxWithNormalizedLineBreaks>
        <strong>Ballot Code</strong> (1 to 3 chars party code on ballots):
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxBallotCode_Add" runat="server" CssClass="TextBoxInput" Width="30px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" colspan="2" style="height: 13px">
        Party Name:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPartyName_Add" runat="server" CssClass="TextBoxInput" Width="340px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;<asp:Button
          ID="ButtonAdd" runat="server" CssClass="Buttons" OnClick="ButtonAdd_Click1" Text="Add"
          Width="100px" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableDelete" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="2">
        Delete This Party - MASTER Only
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:Label ID="Msg_Delete_Party" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left">
        <asp:Button ID="ButtonDelete" runat="server" CssClass="Buttons" Font-Bold="True"
          Text="DELETE" OnClick="ButtonDelete_Click1" Width="100px"></asp:Button>
      </td>
      <td align="left" class="T">
        Deleting a party may leave broken links because the needed PartyKey will be missing.&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableParty" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td class="H" colspan="2">
        Political Party
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <asp:Label ID="Msg_Party" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2">
        Party Information
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2" style="height: 13px">
        <strong>Edit Party Information:</strong> Make any changes and select the Enter key
        or click outside the textbox.
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Ballot Code*:</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxBallotCode" runat="server" Width="43px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextboxBallotCode_TextChanged"></user:TextBoxWithNormalizedLineBreaks>1
        to 3 chars party code on ballots
      </td>
      <td class="T">
        <strong>Party Key:</strong>
        <asp:Label ID="PartyKey" runat="server" CssClass="TBoldColor"></asp:Label>
        Automatically created using 2 char State Code (if State) + Ballot Code
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Party Name*:
          <br />
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPartyName" runat="server" Width="316px" CssClass="TextBoxInput"
            AutoPostBack="True" OnTextChanged="TextBoxPartyName_TextChanged"></user:TextBoxWithNormalizedLineBreaks></strong>
      </td>
      <td class="T">
        <strong>Address Line 1:</strong><br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddressLine1" runat="server" Height="23px" Width="313px"
          CssClass="TextBoxInput" AutoPostBack="True" OnTextChanged="TextBoxAddressLine1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Web Address:</strong>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxWebAddress" runat="server" Width="312px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextboxWebAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="T">
        <strong>Address Line 2:</strong><br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddressLine2" runat="server" Width="314px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxAddressLine2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        Links to \Politician\Intro.aspx to enter politician data<br />
        <asp:HyperLink ID="HyperLink_Party_Politician_Links" runat="server" CssClass="HyperLink">Party Politician Links</asp:HyperLink>&nbsp;
      </td>
      <td class="T">
        &nbsp;<strong>City, State Zip:</strong><br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCityStateZip" runat="server" Width="320px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxCityStateZip_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="H" colspan="2">
        Party Email Addresses and Contacts
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:Label ID="Msg_Email" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2" style="height: 13px">
        <strong>Add a New Email Address:</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2" style="height: 13px">
        Enter the address in the textbox and click the Add Button. <strong>DO NOT</strong>
        enter other information. When the adderess is added you will be given oportunity
        to add the contact information.
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" colspan="2" valign="top">
        New Email Address:
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxEmailAddress_Add" runat="server" CssClass="TextBoxInput"
          Width="350px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
        <asp:Button ID="Button_Add_Email" runat="server" CssClass="Buttons" OnClick="Button_Add_Email_Click1"
          Text="Add" Width="100px" />
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2">
        <strong>Edit Email Address and Contact:</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Make any desired changes or additions to any textbox. Then select the Enter key
        or click outside the textbox to have your change or addition recorded.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>Email Address:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextboxEmailAddress" runat="server"
          Width="236px" CssClass="TextBoxInput" AutoPostBack="True" OnTextChanged="TextboxEmailAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td align="left" class="T">
        <strong>Phone:</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxPhone" runat="server" Width="200px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextboxPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>First Name for Salutation: </strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_First_Name" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBox_First_Name_TextChanged" Width="141px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
      <td align="left" class="T">
        <strong>M. Last Names:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBox_Last_Name" runat="server"
          AutoPostBack="True" CssClass="TextBoxInput" OnTextChanged="TextBox_Last_Name_TextChanged"
          Width="209px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>Title:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Title" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            OnTextChanged="TextBox_Title_TextChanged" Width="316px"></user:TextBoxWithNormalizedLineBreaks></strong>
      </td>
      <td align="left" class="T">
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2">
        <strong>Delete Email Address and Contact:</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2" style="height: 13px">
        Select email address from the report. Then click this Delete Button.
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:Button ID="Button_Delete_Email" runat="server" CssClass="Buttons" OnClick="Button_Delete_Email_Click"
          Text="Delete Email" Width="100px" />
      </td>
    </tr>
    <tr>
      <td align="left" class="H" colspan="2">
        <asp:Label ID="PartyEmailsAndContactsHeading" runat="server">Party Emails and Contacts</asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="T">
        <asp:Label ID="Label_Contacts_Emails_Report" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        Parties
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        This is a list of all major and minor parties. The Ballot Code is shown on ballots
        and reports and is used to associate a party with a candidate on ballots. The Pary
        Key is a unique internal code used to edit information about that party.<br />
        <strong>Email Addresses in Database:</strong> Click on a Pary Link in the
        4th column to obtain a list of the email addresses in the database.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="T">
        <asp:Label ID="Label_Political_Parties_Report" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
    </ContentTemplate>
  </asp:UpdatePanel>
  </div>
</asp:Content>
