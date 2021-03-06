<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="Logs.aspx.cs" Inherits="Vote.Logs.LogsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>


<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    <!-- Table -->
    <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          Login User Name:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLoginUser" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          From:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFrom" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          To:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxTo" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          <asp:Button ID="ButtonRunReport" runat="server" Text="Run Report" OnClick="ButtonRunReport_Click"
            CssClass="Buttons" /></td>
      </tr>
      <tr>
        <td>
          <asp:CheckBoxList ID="CheckBoxListLogs" runat="server" CssClass="CheckBoxes" RepeatDirection="Horizontal"
            RepeatColumns="5">
            <asp:ListItem>Logins</asp:ListItem>
            <asp:ListItem Value="PoliticianAnswers">Politician Answers</asp:ListItem>
            <asp:ListItem Value="PoliticianAdds">Politician Adds</asp:ListItem>
            <asp:ListItem Value="PoliticianChanges">Politician Changes</asp:ListItem>
            <asp:ListItem Value="ElectionPoliticianAdds">Election Politician Adds</asp:ListItem>
            <asp:ListItem Value="ElectionPoliticianChanges">Election Politician Changes</asp:ListItem>
            <asp:ListItem Value="ElectionOfficeChanges">Election Office Changes</asp:ListItem>
            <asp:ListItem Value="OfficeChanges">Office Changes</asp:ListItem>
            <asp:ListItem Value="OfficeOfficialsAdds">Office Officials Adds</asp:ListItem>
            <asp:ListItem Value="OfficeOfficialsChanges">Office Officials Changes</asp:ListItem>
            <asp:ListItem Value="Picture">Picture Uploads</asp:ListItem>
            <asp:ListItem Value="AdminData">Admin Adds & Updates</asp:ListItem>
          </asp:CheckBoxList>
          <asp:Button ID="ButtonCheckAll" runat="server" OnClick="ButtonCheckAll_Click"
            Text="Check All" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableUserName" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          Users</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelUserName" runat="server" CssClass="ReportRow"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableLogins" cellspacing="0" cellpadding="0" runat="server">
      <tr>
        <td class="H">
          Logins</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelLogins" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticianAnswers" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Politician Answers</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelPoliticianAnswers" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticianAdds" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Politician Adds</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelPoliticianAdds" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticianChanges" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Politician Changes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelPoliticianChanges" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableElectionPoliticianAddsDeletes" cellspacing="0"
      cellpadding="0" runat="server">
      <tr>
        <td class="H">
          Election Politicians Adds and Deletes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelElectionPoliticianAddsDeletes" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableElectionPoliticianChanges" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Election Politician Changes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelElectionPoliticianChanges" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableElectionOfficeChanges" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Election Office Changes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelElectionOfficeChanges" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableOfficeChanges" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Office Changes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelOfficeChanges" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableOfficeOfficialsAdds" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Office Officials Adds</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelOfficeOfficialsAdds" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableOfficeOfficialsChanges" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Office Officials Changes</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelOfficeOfficialsChanges" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePictureUploads" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Picture Uploads</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelPictureUploads" runat="server"></asp:Label></td>
      </tr>
    </table>
    <table class="tableAdmin" id="TableAdminDataUpdates" cellspacing="0" cellpadding="0"
      runat="server">
      <tr>
        <td class="H">
          Admin Data Updates</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelAdminDataUpdates" runat="server"></asp:Label></td>
      </tr>
    </table>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
