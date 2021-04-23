<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="CountyJurisdictionsReport.aspx.cs" 
Inherits="Vote.Admin.CountyJurisdictionsReportPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="mainContent" style="margin: 20px 0">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
  </div>
</asp:Content>
