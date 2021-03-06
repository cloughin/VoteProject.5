<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="Vote.Error500Page" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Our Servers Had a Problem&hellip;</h1>

  <div class="intro-copy">
    <p>We are sorry but our servers experienced a problem in processing your request.
    We have captured information about the problem and will work to correct it.</p>
    <p class="no-print">For now please make a selection from the links in the footer, or go to our 
    <a href="/">Home page</a>.</p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
