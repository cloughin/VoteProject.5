<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forVoters.aspx.cs" Inherits="Vote.ForVotersPage" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%@ Register Src="/Controls/GoogleAddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .content .intro-copy.first {
      margin-bottom: 12px;
    }
    .content .float-box
    {
      overflow: auto;
    }
    .content .link-boxes
    {
      margin: 0 60px;
      font-family: 'Roboto', sans-serif;
    }
    .content .link-boxes.ui-accordion .ui-accordion-content {
      padding: 0;
    }
    .content .link-header {
      background: #c8c8c8;
      padding: 8px 8px 8px 28px;
      margin-top: 15px;
      font-size: 16px;
      color: #444;
      line-height: 120%;
    }
    .content .link-header:first-child {
      margin-top: 0;
    }
    .no-js .content .link-header {
      border: 1px solid #aaaaaa;
      border-top-left-radius: 4px;
      border-top-right-radius: 4px;
      padding-left: 8px;
    }
    .no-js .content .links {
      border: 1px solid #aaaaaa;
      border-top: 0;
      border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
    }
    .content .links p {
      padding: 6px;
      border-bottom: 1px solid #c8c8c8;
      font-size: 16px;
      color: #000000;
      line-height: 120%;
    }
    .content .links p:last-child {
      border-bottom: none;
    }
    /*
    .content .links a {
      color: #333333;
    }
    .content .links a:hover {
      color: #d00000;
    }
    */
    .content .header-area
    {
      width: 100%;
      height: auto;
      padding-bottom: 10px;
      background-color: #365e77;
    }
    .content .header-area p
    {
      color: #fff;
      padding: 10px 13px 10px 13px;
      font-family: 'Roboto', sans-serif;
      font-size: 13px;
      line-height: 17px;
      overflow: auto;
    }
    .content .address-entry {
      margin: 0 10px 0 13px;
      max-width: 600px;
    }
    .content .address-entry p.instructions {
      color: #000;
    }
    /*
    .content .no-address .address-entry {
      margin-left: 15px;
    }
    */
    
    .content p.address-entry-heading {
      padding-left: 0;
    }
    
    .content .address-entry-inline {
      background: #dddddd;
      padding: 15px;
    }
    .content .address-entry-inline input[type=submit] {
      margin: 20px 0 0 0;
    }
    
    .content .no-address .with-address,
    .content .has-address .without-address,
    .content .no-state .with-state,
    .content .has-state .without-state {
      display: none;
    }
    .content p.address-entry-heading.with-address.as-button {
      background: #999999;
      display: inline-block;
      border-radius: 5px;
      padding: 3px 6px;
      cursor: pointer;
      margin: 8px 0;
    }
    
    @media only screen and (max-width : 759px) 
    {
      /* small & medium */
      .content .link-boxes
      {
        margin: 0 2px;
      }
    }
    
    @media only screen and (max-device-width : 999px) 
    {
      /* hide explorer link*/
      .election-explorer
      {
        display: none;
      }
    }
  </style>
  <script type="text/javascript">
    $(function () {
      // remove hidden election explorer so it doesn't foul the accordion
     $(".election-explorer").each(function() {
       if ($(this).css("display") === "none") $(this).remove();
     });
     if ($(".content .header-area").hasClass("has-address")) {
        $(".content p.address-entry-heading.with-address").addClass("as-button").click(function () {
          $(".address-entry-inline").toggle(200);
        });
        $(".address-entry-inline").hide();
      }
      $(".link-boxes").accordion({
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="For Voters" />
  <div id="HeaderArea" class="header-area no-print" runat="server">
    <div class="address-entry">
      <p class="address-entry-heading without-address">Get your Interactive Sample Ballot or your current Elected Officials customized for your location. Enter your address or 9-digit zip.</p>
      <p class="address-entry-heading with-address">Change to a different address</p>
      <user:AddressEntry ID="AddressEntry" runat="server" />
    </div>
    <asp:PlaceHolder ID="SampleBallotPlaceHolder" runat="server"></asp:PlaceHolder>
    <p id="ElectedOfficialsButton" class="button" runat="server">
      <a class="link-button" id="ElectedOfficialsLink" runat="server" href="?">Your Currently Elected Officials</a>
    </p>
    <p class="without-state">Select a state (top right) to view all State and Federal Elected Officials.</p>
    <p id="StatewideButton" class="button" runat="server">
      <a class="link-button" id="StatewideLink" runat="server" href="?">All State and Federal Elected Officials</a>
    </p>
</div>

  <div class="intro-copy first no-print">
    <p>These links provide complete directories of all federal and state elected officials, election contests and candidates.</p>
  </div>

  <div class="float-box no-print">
    <div class="link-boxes">
      <asp:PlaceHolder ID="ExplorerLinkBox" runat="server">
        <div class="link-header election-explorer js-only">
        Election Explorer <span style="color:red">New!</span>
        </div>
        <div class="links election-explorer js-only">
          <p>
            <asp:HyperLink ToolTip="Election Explorer" ID="ExplorerLink" runat="server">Explore any election for any address or state</asp:HyperLink></p>
        </div>
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="UpcomingLinkBox" runat="server">
        <div class="link-header">
          Upcoming Election(s)
        </div>
        <div class="links" id="Upcoming" runat="server">
        </div>
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="ElectionResultsLinkBox" runat="server">
        <div class="link-header" runat="server">
          Past Elections
        </div>
        <div class="links" id="ElectionResults" runat="server">
        </div>
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="PresidentLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="SenateLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="HouseLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="GovernorLinks" runat="server">
      </asp:PlaceHolder>
    </div>
  </div>

  <div class="intro-copy">
    <p>All the information we present is candidate authored or copied from the candidate&rsquo;s website.</p> 

    <p>
    We invite and provided every candidate with the means to securely provide his or her picture, 
    biographical information, and to express his or her opinions and views on issues of their choosing. 
    Vote-USA has tried to be as inclusive and non-partisan as possible 
    regarding the issues to which candidates could respond. 
    There are about 50 different issues and about 20 topics for each issue. 
    This provides each candidate with approximately 1,000 different opportunities 
    to express his or her opinions and views. Candidates select issues for which they want to provide a response.
    </p> 

    <p>For candidates who do not respond to our invitation to provide information about themselves, 
    we copy pictures and information from candidates’ websites into this website. </p>

    <p>Links and pages are only provided where we have candidate information 
    or responses. <span class="no-print">This is a report of the 
    <a href="/IssueList.aspx" id="IssueListLink" runat="server">
    issues and issue questions we make available to all candidates</a>.</span></p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
