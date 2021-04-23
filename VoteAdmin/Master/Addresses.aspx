<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Addresses.aspx.cs" Inherits="Vote.Master.AddressesPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    #LegislativeTextBox
    {
      width: 680px;
    }
    th
    {
      padding: 0 0 0 5px;
      font-family: Verdada, Arial, Helvetica, Sans-Serif;
      font-weight: bold;
      color: #373737;
      font-size: 11px;
    }
  </style>
  <script language="javascript">
    function clearFeedback() {
      var feedback = document.getElementById('Feedback');
      if (feedback)
        feedback.innerHTML = '';
    }
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
  <!-- Table -->
  <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HLarge">
        <asp:Label ID="PageTitle" runat="server">Select and Download Addresses to CSV</asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Feedback" runat="server"></asp:Label>
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        Select States to Include
      </td>
    </tr>
  </table>

  <!-- States -->
  <asp:PlaceHolder ID="StatesPlaceHolder" runat="server"></asp:PlaceHolder>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="center" class="T">
        <asp:Button ID="SelectAllStatesButton" runat="server" Text="Select All States" 
          onclick="SelectAllStatesButton_Click" onclientclick="clearFeedback()" />
        <asp:Button ID="ClearAllStatesButton" runat="server" Text="Clear All States" 
          onclick="ClearAllStatesButton_Click" onclientclick="clearFeedback()" />
          <br /><br />
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H" colspan="2">
        Filtering
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
      &nbsp;<br />
        <asp:CheckBox ID="LegislativeFilterCheckBox" runat="server" />Filter by
        <asp:DropDownList ID="LegislativeFilterDropDownList" runat="server">
          <asp:ListItem Value="CD">Congressional District</asp:ListItem>
          <asp:ListItem Value="SD">State Senate District</asp:ListItem>
          <asp:ListItem Value="HD">State House District</asp:ListItem>
          <asp:ListItem Value="CNTY">County</asp:ListItem>
        </asp:DropDownList>:
      </td>
      <td align="left" class="T">
        Enter district codes separated by commas:
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="LegislativeTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="NameFilterCheckBox" runat="server" />Only rows with names
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="AddressFilterCheckBox" runat="server" />Only rows with street addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="EmailFilterCheckBox" runat="server" />Only rows with email addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="NoEmailFilterCheckBox" runat="server" />Only rows without email addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="PhoneFilterCheckBox" runat="server" />Only rows with phone numbers
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Date Range: from 
        <user:TextBoxWithNormalizedLineBreaks ID="FromDateTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
        to
        <user:TextBoxWithNormalizedLineBreaks ID="ToDateTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks> (mm/dd/yyyy or empty)
        <br /><br />
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        Output File Format
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="OutputHeadingCheckBox" runat="server" />Include output headings
      </td>
    </tr>
  </table>

  <!-- Output Columns -->
  <asp:PlaceHolder ID="OutputColumnPlaceHolder" runat="server"></asp:PlaceHolder>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="T">
        <br />
      </td>
    </tr>
    <tr>
      <td align="left" class="H">
        Initiate Download
      </td>
    </tr>
    <tr>
      <td align="center" class="T" colspan="2">
      <br />
        <asp:Button ID="DownloadButton" runat="server" Text="Download Now" 
          onclick="DownloadButton_Click" onclientclick="clearFeedback()" /><br /><br />
      </td>
    </tr>
  </table>

  <!-- Table -->
  <%-- 
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        Update Addresses From Log
      </td>
    </tr>
    <tr>
      <td align="center" class="T" colspan="2">
      <br />
        <asp:Button ID="UpdateAddressesFromLogButton" runat="server" 
          Text="Update Addresses From Log" 
          onclick="UpdateAddressesFromLogButton_Click" onclientclick="clearFeedback()" />
      </td>
    </tr>
  </table>
   --%>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
