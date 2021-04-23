<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainBanner.ascx.cs" Inherits="Vote.Controls.MainBanner" %>

<div id="mainBanner">
  <div class="logo" runat="server" id="MainBannerLogo">
    <a id="MainBannerHomeLink" href="/" title="" runat="server"><img runat="server" id="MainBannerImageTag" src="/images/homelogo.png" alt="Vote USA - Connecting voters and candidates"/></a>
  </div>
  <div class="get-sample-ballot-link">
    <div><a class="get gradient-bg-1-hovering rounded-border shadow" onclick="getSampleBallot2()"><span class="line1">Get Your</span><br />Sample Ballot</a></div>
  </div>
  <div id="MainBannerStateBanner" class="stateBanner" runat="server">
    <div class="stateName"><img src="" id="MainBannerStateName" alt="" runat="server" /></div>
    <div class="stateSite">
      <a id="MainBannerStateSiteLink" class="stateSiteLink" href="#" title="" target="stateSite" runat="server">Click to go to the Official Website →</a><br />
      <span class="notAssociated">We are not associated with any election authority</span>
    </div>
    <div class="anotherState">
      <a class="anotherStateButton" onclick="openAddressEntryDialog(this);">Select another state</a>
    </div>
  </div>
</div>
