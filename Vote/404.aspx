<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Vote.Error404Page" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Page Not Found</h1>

  <div class="intro-copy">
    <p>We are sorry but we cannot find the page you requested.</p>
    <p class="no-print">Please make a selection from the links in the footer, or go to our 
    <a href="/">Home page</a>.</p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
