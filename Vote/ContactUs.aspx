<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="ContactUs.aspx.cs" Inherits="Vote.ContactUsPage" %>

<%@ Register Src="/Controls/VoteUsaAddressResponsive.ascx" TagName="VoteUsaAddress" TagPrefix="user" %>
<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>--%>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Contact Us</h1>
  <div class="intro-copy">
    <p>You can contact us via standard mail at:</p>
    <user:VoteUsaAddress ID="VoteUsaAddress" runat="server" />
    <p class="no-print">Or send us an email using the form below:</p>
    <user:EmailForm id="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
