<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Elected.aspx.cs" Inherits="Vote.ElectedPage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/DonationRequestNew.ascx" TagName="DonationRequestNew" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .content .locals-accordion {
      margin-top: 15px;
    }
    .content .locals-accordion .accordion-header {
      padding-left: 28px;
    }

    .location-info
    {
      margin: 20px 0;
    }

  </style>
  <script type="text/javascript">
    $(function () {
      if ($(".accordion-toggle").hasClass("no-js")) {
        // Wrap the locals
        $(".elected-report-locals").wrapAll('<div class="locals-accordion"></div>');
        $(".locals-accordion,.elected-report-locals-content").accordion({
          active: false,
          collapsible: true,
          heightStyle: "content",
          activate: PUBLIC.accordionActivate
        });
      } else {
        $(".elected-report,.elected-report-locals-content").accordion({
          collapsible: true,
          heightStyle: "content",
          activate: PUBLIC.accordionActivate
        });
      }
      PUBLIC.initBannerAd("E", $("body").data("state"));
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="InnerContent" runat="server">
    
    <div id="LocationInfo1" class="location-info location-info1" runat="server"></div>
    <div class="intro clearfix">
      <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Your Currently Elected Officials" />
      <div id="LocationInfo2" class="location-info location-info2" runat="server"></div>
    </div>
    <div class="accordion-toggle no-js"> <!-- has-js no-js -->
      <asp:PlaceHolder ID="ReportPlaceHolder" runat="server" EnableViewState="False"></asp:PlaceHolder>
    </div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
  <user:DonationRequestNew ID="DonationRequestNew" runat="server" />
</asp:Content>
