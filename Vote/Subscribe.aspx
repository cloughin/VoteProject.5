<%@ Page  Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Subscribe.aspx.cs" Inherits="Vote.SubscribePage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Subscribe and Unsubscribe</h1>

  <div class="intro-copy">
    <p id="Message" runat="server">
    We could not perform the requested operation due to invalid input.            
    </p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
