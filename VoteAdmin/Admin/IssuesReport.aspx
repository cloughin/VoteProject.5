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

      <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"></asp:PlaceHolder>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
