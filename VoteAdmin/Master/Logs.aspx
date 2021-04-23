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
    <table class="tableAdmin" id="TableHeading">
      <tr>
        <td class="HLarge">
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
    <table class="tableAdmin" id="TableUserName">
      <tr>
        <td class="H">
          Users</td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelUserName" runat="server" CssClass="ReportRow"></asp:Label></td>
      </tr>
    </table>

      <asp:PlaceHolder ID="ReportsPlaceHolder" runat="server"></asp:PlaceHolder>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
