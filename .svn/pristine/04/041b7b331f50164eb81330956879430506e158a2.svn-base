<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="ElectionForIFrame.aspx.cs" Inherits="Vote.ElectionForIFramePage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .content .election-subtitle {
      margin-top: 4px;
      font-weight: normal;
      font-size: 1rem;
    }
    .content .higher-level-links {
      padding-bottom: 3px;
    }
    .content .higher-level-links a {
      display: block;
      margin-top: 10px;
      font-weight: bold;
      font-size: .9rem;
    }
    .content .past-election-warning {
      font-size: .8rem;
      color: #999999;
      font-style: italic;
      margin-top: 4px;
    }
    .content .additional-info {
      margin-top: 5px;
      font-size: .9rem;
      line-height: 120%;
    }
    .content .election-report {
      margin-top: 10px;
    }
    .content .election-report.accordion-content {
        margin-top: 0;
    }
    .content .intro {
      padding: 0 5px;
    }
    .master-links a {
      padding: 0 20px;
      font-size: 80%;
      font-weight: normal;
    }
    .donate-banner,
    .social-media-outer,
    .main-banner-outer,
    .main-navigation,
    .content .page-heading,
    .content .intro,
    .main-footer {
      display: none;
    }
  </style>
  <script type="text/javascript">
    $(function () {
      $(".election-report,.category-content").accordion({
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Election Contests and Ballot Measures" />
  
  <div class="intro">
    <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
    <h2 id="ElectionSubTitle" class="election-subtitle" runat="server"></h2>
    
    <div id="HigherLevelLinks" class="higher-level-links" runat="server"></div>
  
    <div id="PastElectionWarning" class="past-election-warning" runat="server">
      Note: Candidate pictures are the most current we have on file. Because 
          this is a past election, they may not show a candidate’s likeness at the 
          time of the election. Likewise, some of the links to websites, emails, 
          and social media may be broken.
    </div>

    <div id="AdditionalInfo" class="additional-info" runat="server"></div>
  </div>
        
  <asp:PlaceHolder ID="ElectionPlaceHolder" runat="server"  EnableViewState="False"></asp:PlaceHolder>

</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
  <user:DonationRequest ID="DonationRequest" runat="server" />
</asp:Content>

