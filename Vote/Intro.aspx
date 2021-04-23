<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true"
CodeBehind="Intro.aspx.cs" Inherits="Vote.IntroPage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/DonationRequestNew.ascx" TagName="DonationRequestNew" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .responsive-report
    {
      padding: 0;
    }

    .intro-page .content h1
    {
      margin-top: 0;
      font-size: 16px;
      line-height: 120%;
    }

    .intro-report
    {
      margin-bottom: 20px;
      padding: 0;
    }

    .intro-report .candidate-image
    {
      display: block;
      float: right;
    }

    .intro-report .candidate-info
    {
      width: 100%;
      padding: 0;
      display: block;
    }

    .intro-report .candidate-status
    {
      font-weight: bold;
      font-size: 1.3rem;
    }

    .intro-report .candidate-election
    {
      margin-top: 3px;
      font-size: 1rem;
      font-weight: normal;
    }

    .intro-report a.compare-candidates
    {
      margin: 5px 0 0 0;
    }

    .intro-report .candidate-party
    {
      margin: 15px 0;
      display: block;
    }

    .intro-report .social-media-anchors
    {
      margin-top: 15px;
    }

    .intro-report .candidate-address,
    .intro-report .candidate-phone,
    .intro-report .candidate-age
    {
      margin-top: 10px;
      line-height: 1.3;
    }

    @media only screen and (max-width: 519px)
    {
      /* small */
      .intro-report .candidate-image img
      {
        max-width: 100px;
      }
    }

    @media only screen and (min-width: 520px) and (max-width: 759px)
    {
      /* medium */
      .intro-report .candidate-image img
      {
        max-width: 200px;
      }
    }
  </style>
  <script type="text/javascript">
    $(function () {
      $(".accordion-container").accordion({
        active: false,
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate
      });
      PUBLIC.setupBackToBallot();
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="InnerContent" runat="server">
    <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="{0}<br/><small>Biographical Profile and Positions on the Issues</small>" />
        
    <asp:PlaceHolder ID="InfoPlaceHolder" runat="server" EnableViewState="False"></asp:PlaceHolder>
    <asp:PlaceHolder ID="ReportPlaceHolder" runat="server" EnableViewState="False"></asp:PlaceHolder>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
  <%--<user:DonationRequest ID="DonationRequest" runat="server" />--%>
  <user:DonationRequestNew ID="DonationRequestNew" runat="server" />
</asp:Content>
