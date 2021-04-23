<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forVotersNoAddress.aspx.cs" Inherits="VoteNew.forVotersNoAddress" %>

<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>
<%@ Register Src="/Controls/AddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title runat="server" id="TitleTag" >For Voters - VoteUSA.org</title>
</head>
<body>
    <form id="form1" runat="server" enableviewstate="True">
    <div id="outer">
    <div id="container">
      <user:SocialMediaButtons runat="server" />
      <user:MainBanner runat="server" />
      <user:MainNavigation runat="server" />
      <div id="mainContent">
        <div class="header">
        <em>for</em> Voters
        </div>

        <div class="intro">
        <p class="lead">Sample Ballot for Upcoming Election</p>
        <p>Click <a href="#">here</a> to select your state or enter your address so that we can customize a sample ballot for you.</p>
        <p>Please note that our sample ballot is not an exact replica of the ballot at the polls. Rather, we present an interactive
        listing that you can use to easily learn more about the candidates and issues.</p>
        </div>

        <div class="intro">
        <p class="lead">Current Office Holders</p>
        </div>

        <div class="linkBoxes">

        <div class="linkBox">
          <div class="linkHeader">
          Current National Office Holders
          </div>
          <div class="links">
          <p><a href="#">President and Vice President</a></p>
          <p><a href="#">U.S. Senate</a></p>
          <p><a href="#">U.S. House of Representatives</a></p>
          </div>
        </div>

        <div class="linkBox">
          <div class="linkHeader">
          Current State Office Holders
          </div>
          <div class="links">
          <p>Click <a href="#">here</a> to select your state or enter your address so that we can provide a customized
          list of current state office holders for you.</p>
          </div>
        </div>
        </div>

        <div class="intro">
        <p class="lead">Previous Election Results</p>
        </div>

        <div class="linkBoxes">
        <div class="linkBox">
          <div class="linkHeader">
          National Election Results
          </div>
          <div class="links">
          <p><a href="#">President and Vice President<br />
          <span class="date">Nov. 4, 2008</span></a></p>
          <p><a href="#">U.S. Senate<br />
          <span class="date">Nov. 2, 2010</span></a></p>
          <p><a href="#">U.S. House of Representatives<br />
          <span class="date">Nov. 2, 2010</span></a></p>
          </div>
        </div>

        <div class="linkBox">
          <div class="linkHeader">
          State Election Results
          </div>
          <div class="links">
          <p>Click <a href="#">here</a> to select your state or enter your address so that we can provide a customized
          list of state election results for you.</p>
          </div>
          </div>
        </div>

        <div class="intro">
        <p><strong>About Issues</strong></p>
        <p>Every candidate on this website was invited to provide his or her picture and 
        personal information and to express his or her opinions and views on issues of 
        concern. We have tried to be as inclusive and non-partisan as possible regarding 
        the issues to which candidates can respond. There are about 50 different issues and 
        about 20 topics for each issue, providing approximately 1000 different opportunities 
        for each candidate to express his or her views. Candidates select which issues 
        they want to respond to.</p>
        </div>

        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->

    <user:AddressEntry  runat="server" />

    </form>
</body>
</html>
