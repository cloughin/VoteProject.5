<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Referendum.aspx.cs" Inherits="Vote.ReferendumPage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/DonationRequestNew.ascx" TagName="DonationRequestNew" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .intro .referendum-title p {
      font-size: 1.4rem;
      line-height: 110%;
    }
    .intro .referendum-election {
      font-size: 1.2rem;
    }
    .intro .label {
      margin-top: 25px;
      font-weight: bold;
    }
    .intro .label a.parenthetical {
      font-weight: normal;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Ballot Measures" />
  
  <div class="intro">
    <div id="ReferendumTitle" class="referendum-title" runat="server"></div>
    <div id="ReferendumSubTitle" class="referendum-sub-title" runat="server"></div>
    <div id="ReferendumElection" class="referendum-election" runat="server"></div>
    <div id="ReferendumDescriptionLabel" class="referendum-description-label label" runat="server"></div>
    <div id="ReferendumDescription" class="referendum-description" runat="server"></div>
    <div id="ReferendumDetailLabel" class="referendum-detail-label label" runat="server"></div>
    <div id="ReferendumDetail" class="referendum-detail" runat="server"></div>
    <div id="ReferendumFullTextLabel" class="referendum-full-text-label label" runat="server"></div>
    <div id="ReferendumFullText" class="referendum-full-text" runat="server"></div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
  <user:DonationRequestNew ID="DonationRequestNew" runat="server" />
</asp:Content>
