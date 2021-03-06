<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forElectionAuthorities.aspx.cs" Inherits="Vote.ForElectionAuthoritiesPage" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/AddSampleBallotButtons.ascx" TagName="AddSampleBallotButtons" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>For Election Authorities</h1>

  <div class="intro-copy">

    <h2>Election Authority Tools</h2>

    <p>Most of the 51 state election authorities have been very cooperative in 
    response to our requests for election rosters. Since 2004, Vote-USA has 
    used these rosters to cover all general elections, most off-year elections, 
    and many primary elections.</p>

    <p>All states, counties, and many local districts have their own rules 
    and regulations regarding elections. Some states are responsible 
    for federal, state, county and local elected offices, while other 
    states delegate this responsibility to the counties and local districts. 
    Each authority has its own ballot access requirements, set of elected 
    offices, election rosters, and election calendars. Although we have not yet
    begun to work with county and local district election authorities, we 
    intend to do so in the near future, finances permitting.</p>

    <p>Vote-USA has the tools to allow any state election authority to enter 
    and edit its own election rosters for the Vote-USA platform. 
    Doing so makes our customized ballot choices available at the earliest 
    possible time, guarantees their accuracy, and reduces our data entry load. 
    We also have the tools to add our customized ballot choices and other 
    reporting capabilities to any state election website. We welcome the 
    opportunity to work with any state that desires to utilize these 
    capabilities.</p>

    <user:AddSampleBallotButtons ID="AddSampleBallotButtons" runat="server" />

    <h2 class="no-print">Email Us</h2>

    <p class="no-print">If you would like to work with us regarding any of these possibilities, 
    please use the form below to contact us:</p>

    <user:EmailForm id="EmailForm" runat="server" />

  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
