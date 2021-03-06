<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master"
 AutoEventWireup="true" CodeBehind="ElectionReport.aspx.cs" 
 Inherits="Vote.Admin.ElectionReportPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <link href="/css/Election.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="mainContent" style="margin: 20px 0">
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
  </div>
</asp:Content>
