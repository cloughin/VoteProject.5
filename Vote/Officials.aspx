<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Officials.aspx.cs" Inherits="Vote.OfficialsNew" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/DonationRequestNew.ascx" TagName="DonationRequestNew" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
     .content .instructions {
       margin-top: 5px;
       font-size: .9rem;
       line-height: 120%;
     }

     #mainContent
     {
       margin-top: 20px;
     }

  </style>
  <script type="text/javascript">
    $(function () {
      $(".officials-report").accordion({
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="InnerContent" runat="server">
    <h1 id="H1" runat="server">Directory of Current State and Federal Representatives</h1>

    <div id="mainContent">
      <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
  <user:DonationRequestNew ID="DonationRequestNew" runat="server" />
</asp:Content>
