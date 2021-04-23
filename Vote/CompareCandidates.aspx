<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="CompareCandidates.aspx.cs" Inherits="Vote.CompareCandidates" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/DonationRequestNew.ascx" TagName="DonationRequestNew" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/Disqus.ascx" TagName="Disqus" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <!--[if IE]>
    <style type="text/css">
      .responsive-report .answer-source::before
      {
        content: "";
      }

      .responsive-report .answer-source a::before
      {
        content: "Source: ";
        color: #222222;
      }

      .responsive-report .answer-source a
      {
        display: inline-block;
      }
    </style>
  <![endif]-->
  <style type="text/css">
  </style>
  <script type="text/javascript" src="/js/jq/kalypto.js"></script>
  <script type="text/javascript">
    $(function () {
      PUBLIC.initAnswerAccordions();
      PUBLIC.initBallotCheckBoxes();
      PUBLIC.loadMyBallotChoices();
      PUBLIC.initBallotEvents();
      PUBLIC.setupBackToBallot();
      PUBLIC.initAds();
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Compare the Candidates<br/><small>Biographical Profiles and Positions on the Issues</small>" />
  
  <div class="intro">
    <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
    <h2 id="OfficeTitle" class="office-title" runat="server"></h2>
  </div>
        
  <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"  EnableViewState="False"></asp:PlaceHolder>
  
<%--  <user:Disqus ID="Disqus" runat="server" />--%>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
  <user:DonationRequestNew ID="DonationRequestNew" runat="server" />
</asp:Content>
