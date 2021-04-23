<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="PublicInterestOrganizations.aspx.cs" Inherits="Vote.PublicInterestOrganizations" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/AddSampleBallotButtons.ascx" TagName="AddSampleBallotButtons" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .intro-copy a.partner, 
    .intro-copy a:hover.partner
    {
      display: block;
      text-decoration: none;
      color: #444;
    }
    .intro-copy a.tnca
    {
      min-height: 100px;
      background: url(/images/tnca.png)  no-repeat;
      padding-top: 15px;
      padding-left: 170px;
    }
        
    @media only screen and (max-width : 759px) 
    {
      /* small & medium*/
      .intro-copy a.partner {
        padding-left: 0;
      }
      .intro-copy a.partner span {
        display: block;
      }
      .intro-copy a.partner.tnca .filler {
        height: 100px;
                                              }
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Public Interest Organizations</h1>
  <div class="intro-copy">
    <p>
      We support and work with the following organizations whose mission focuses on the public interest. 
    </p>

    <p>
    <a class="partner tnca" href="http://www.tnca.org/" title="Tennessee Citizen Action" target="_blank">
    <span class="filler"></span>  
    <span class="copy">Tennessee Citizen Action works in the public interest as Tennessee’s 
      premier consumer rights organization. Their mission is to work to improve the overall 
      health, well-being, and quality of life for all people who live and work in Tennessee.
    </span></a></p>

    <user:AddSampleBallotButtons ID="AddSampleBallotButtons" runat="server" />

    <p class="no-print">
      Please contact us with any ideas regarding how we could help your organization
      or how your organization might help us:</p>
    <user:EmailForm ID="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
