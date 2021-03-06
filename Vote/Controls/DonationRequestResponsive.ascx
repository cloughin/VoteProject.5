<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonationRequestResponsive.ascx.cs" 
Inherits="Vote.Controls.DonationRequestResponsive" %>

<div id="donationRequestDialog">
  <div class="header">
    <div id="DonationRequestDialogHeadingText" class="heading-text" runat="server">We Need Your Help</div>
  </div>
  <div class="inner">
    <div id="DonationRequestDialogMessageText" class="message-text" runat="server"></div>
    <div class="inner2">
      <div class="yes"><a id="DonateLink" target="donate" rel="nofollow" runat="server">OK, I'll Donate</a></div>
      <div class="no"><a>Sorry, I'll Pass</a></div>
      <div class="already"><a>I Already Donated</a></div>
    </div>
  </div>
</div>