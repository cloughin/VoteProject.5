<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="IssuesReport.aspx.cs" Inherits="Vote.Admin.IssuesReportPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
      <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="HTMLIssuesTable" cellspacing="0" border="0" runat="server">
    </table>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
