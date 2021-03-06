<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonationRequest.ascx.cs" Inherits="Vote.Controls.DonationRequest" %>

<div id="donationRequestDialog">
  <div class="header">
    <div id="DonationRequestDialogHeadingText" class="headingText" runat="server">We Need Your Help</div>
  </div>
  <div class="inner">
    <div id="DonationRequestDialogMessageText" class="messageText" runat="server"></div>
    <div class="inner2">
      <div class="yes"><a id="DonateLink" target="donate" runat="server">OK, I'll Donate</a></div>
      <div class="no"><a>Sorry, I'll Pass</a></div>
      <div class="already"><a>I Already Donated</a></div>
    </div>
  </div>
</div>