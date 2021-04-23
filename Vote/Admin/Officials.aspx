<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="Officials.aspx.cs" Inherits="Vote.Admin.Officials" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  <style type="text/css">
    .style1
    {
      text-decoration: underline;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label></td>
      </tr>
      </table>
    <!-- Table -->
    <table id="TableIncumbentStatusRadioButtons" border="0" cellpadding="0" cellspacing="0" class="tableAdmin" runat="server">
      <tr>
        <td valign="middle" align="left" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="HSmall">
          Winners Identified (New Incumbents) in Last General Election and Subsequent 
          Elections </td>
      </tr>
      <tr>
        <td align="left" class="T">
          These are the elections where winners have and have not been identified.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          <asp:Label ID="Label_Report_Elections_Winners_Identified"
            runat="server"></asp:Label>
          <br />
&nbsp; </td>
      </tr>
      </table>
    <table id="TableLastUpdated" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="T">
          Use this link to view the report provided to public.</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:HyperLink ID="HyperLink_View_Public_Report" runat="server" CssClass="HyperLink"
            Target="view">View Public Report</asp:HyperLink>
          <br />
&nbsp; </td>
      </tr>
    </table>

    <table class="tableAdmin" id="TableInstructions" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          Edit Currently Elected Officials (Incumbents)</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Normally newly elected officials are identified using the Election form for the 
          election.<br />
          However, clicking on an <span class="style1">Office</span> link in the report below 
          can also be used to identify the current incumbents for that office. It can also 
          be used to change information about the 
          office. 
          <br />
          Click a <u>Name</u>
          link to change information about that person.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticianData" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td align="left" class="T">
          Click on a <span class="style1">picture</span> link to edit biographical information, picture or issue responses on politician's
          Introduction and Issue Pages.
        </td>
      </tr>
    </table>
      <!-- Table -->
    <asp:PlaceHolder ID="ReportPlaceHolder" EnableViewState="False" runat="server"></asp:PlaceHolder>
    <!-- Table -->
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
