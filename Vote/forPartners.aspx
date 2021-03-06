<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
 CodeBehind="forPartners.aspx.cs" Inherits="Vote.ForPartnersPage" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
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
    .intro-copy a.ivn
    {
      min-height: 68px;
      background: url(/images/ivn-main.png) no-repeat;
      padding-top: 5px;
      padding-left: 187px;
    }
    .intro-copy a.endpartisanship
    {
      min-height: 56px;
      background: url(/images/end-partisanship.png) no-repeat;
      padding-top: 5px;
      padding-left: 311px;
    }
    .intro-copy a.nolabels
    {
      min-height: 73px;
      background: url(/images/NoLabels.gif) no-repeat;
      padding-top: 10px;
      padding-left: 103px;
    }
    .intro-copy a.recovering
    {
      min-height: 91px;
      background: url(/images/RecoveringPolitician.jpg) no-repeat;
      padding-top: 20px;
      padding-left: 301px;
    }
    .intro-copy a.bol
    {
      min-height: 30px;
      background: url(/images/businessonlinelogosmall.png) no-repeat;
      padding-top: 0;
      padding-left: 180px;
    }
    .intro-copy a.infiniden
    {
      min-height: 20px;
      background: url(/images/infiniden.gif) no-repeat;
      padding-top: 0;
      padding-left: 99px;
    }
    .intro-copy a.google
    {
      min-height: 29px;
      background: url(/images/googlelogosmall.gif) no-repeat;
      padding-top: 0;
      padding-left: 100px;
    }
    .intro-copy a.aristotle
    {
      min-height: 35px;
      background: url(/images/Aristotle.png) no-repeat;
      padding-top: 0;
      padding-left: 220px;
    }
    .intro-copy a.predelection
    {
      min-height: 80px;
      background: url(/images/predelection.png) no-repeat;
      padding-top: 0;
      padding-left: 301px;
    }
    .intro-copy a.usvotefoundation
    {
      min-height: 91px;
      background: url(/images/usvotefoundation.jpg) no-repeat;
      padding-top: 0;
      padding-left: 119px;
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
      .intro-copy a.partner.ivn .filler {
        height: 68px;
      }
      .intro-copy a.partner.endpartisanship .filler {
        height: 56px;
      }
      .intro-copy a.partner.nolabels .filler {
        height: 73px;
      }
      .intro-copy a.partner.recovering .filler {
        height: 82px;
      }
      .intro-copy a.partner.bol .filler {
        height: 30px;
      }
      .intro-copy a.partner.infiniden .filler {
        height: 20px;
      }
      .intro-copy a.partner.google .filler {
        height: 29px;
      }
      .intro-copy a.partner.aristotle .filler {
        height: 35px;
      }
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>For Partners</h1>
  <div class="intro-copy">
    <p>
      We welcome the opportunity to partner with any organizations that can help fulfill
      our goal of providing voters with as much useful information as possible. Some possibilities
      are:</p>
    <p>
      <b>Non-Partisan Politically Engaged Organizations</b><br />
      We welcome the opportunity to cooperate with and support 
      non-partisan organizations that provide valuable electoral and political information. 
      We help to increase public awareness of these organizations by providing links to their websites. 
      We can also provide them with tools to communicate their special information, 
      like voting records and political contributions, to the public.
    </p>

    <p>Currently we are partnering with the following non-partisan organizations.</p>

    <p>
    <a class="partner predelection" href="http://www.predelection.com/" title="PredElection" target="_blank">
    <span class="filler"></span>  
    <span class="copy">PredElection is America's only free game where players can win prizes 
    predicting elections. With turnout hitting 70-year lows and people turning away from traditional 
    media, PredElection uses the proven principles of gamification to build a mobile solution for 
    an electorate that needs mobilizing.</span></a></p>

    <p>
    <a class="partner usvotefoundation" href="http://www.usvotefoundation.org/" title="U.S. Vote Foundation" target="_blank">
    <span class="filler"></span>  
    <span class="copy">U.S. Vote Foundation provides absentee ballot request and voter registration services for all U.S. voters in all states at home and abroad.</span></a></p>

    <p>
    <a class="partner ivn" href="http://www.endpartisanship.com/" title="End Partisanship" target="_blank">
    <span class="filler"></span>  
    <span class="copy">The Independent Voter Network (IVN) is an online news platform for 
    independent-minded voters, public officials, civic leaders, and 
    journalists. It provides political analysis and commentary in an 
    effort to elevate the level of our public discourse.</span></a></p>

    <p>
    <a class="partner endpartisanship" href="http://ivn.us/" title="Independent Voter Network" target="_blank">
    <span class="filler"></span>  
    <span class="copy">End Partisanship is a coalition of non-partisan organizations advocating for an end to 
    party control of elections. They believe that representatives should represent people,
    not parties and are challenging partisan primaries in the courtroom.</span></a></p>

    <p>
    <a class="partner nolabels" href="http://nolabels.org/" target="_blank">
    <span class="filler"></span>  
    <span class="copy">No Labels supports reforms, leaders and legislation
      that will help fix America’s broken government and break the stranglehold that the
      extremes currently have on our political process.</span></a></p>

    <p>
    <a class="partner recovering" href="http://therecoveringpolitician.com" target="_blank">
    <span class="filler"></span>  
    <span class="copy">The Recovering Politician is a post-partisan forum 
    featuring officials who’ve left the area and lived to write about it.</span></a></p>

    <p>
      <b>Foundations</b><br />
      We have applied to a number of foundations whose initiatives seem to fit with our
      objectives. Although to date we have not received any such funding assistance, we
      would be grateful for the opportunity to work with foundations whose mission it
      is to strengthen our representative democracy.</p>
    <p>
      <b>Media and News Outlets</b><br />
      We have given newspapers and other media permission to use our pictures and candidate
      information in their coverage of elections.</p>
    <p>
      <b>Political Parties</b><br />
      We have provided a number of state political parties with the ability to provide
      information and pictures of their candidates.</p>
    <p>
      <b>Politically Engaged Organizations</b><br />
      We can provide politically engaged organizations with pre-checked ballot choices
      that highlight endorsed candidates. These customized ballot pages can then be integrated
      into that organization’s website.
    </p>

    <p>
      <b>Support from For-Profit Businesses</b><br />
      For-profit businesses can support us by donating goods and services. These contributions
      are considered “in-kind” donations and are tax deductible. We would be grateful
      for other in-kind donations including data entry, hosting, technical consulting,
      and assistance with social networking and search engine issues. </p>
            
      <p>These are the businesses
      that have been a great help and support to us to date:
      </p>
          
    <p>
    <a class="partner bol" href="http://businessol.com"  title="Business OnLine" target="_blank">
    <span class="filler"></span>  
    <span class="copy">Business OnLine has provided Vote-USA with substantial expertise involving many
      technical aspects of our platform.</span></a></p>

    <p>
    <a class="partner infiniden" href="http://www.infiniden.com" title="infiniden Web Presence Design" target="_blank">
    <span class="filler"></span>  
    <span class="copy">Infiniden has provided Vote-USA with web design.</span></a></p>

    <p>
    <a class="partner google" href="http://google.com" title="Google.com" target="_blank">
    <span class="filler"></span>  
    <span class="copy">Vote USA is a recipient of a Google AdWord Grant award.</span></a></p>
    
    <!--<p>
    <a class="partner aristotle" href="http://www.aristotle.com" title="Aristotle, Inc - aristotle.com" target="_blank">
    <span class="filler"></span>  
    <span class="copy">Aristotle has provided Vote-USA with legislative district data needed in the construction
      of our customized voter ballots.</span></a></p>-->

    <user:AddSampleBallotButtons ID="AddSampleBallotButtons" runat="server" />

    <p class="no-print">
      Please contact us with any ideas regarding how we could help your organization,
      or how your organization might help us:</p>
    <user:EmailForm ID="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
