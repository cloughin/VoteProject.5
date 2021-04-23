<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAddressEntry.ascx.cs" 
Inherits="Vote.Controls.GoogleAddressEntry" %>

<div id="addressEntryInline" class="address-entry-inline">
  <!--<p class="instructions">Enter your address and select the correct entry when it appears.</p>-->
  <div class="input-block">
    <input id="Address" type="text" class="address-search no-zoom" placeholder="Enter your street address" autocomplete="off" runat="server" />
<%--    <img src="/images/clear-icon-20.png" class="clear-icon"/>--%>
  </div>
  <p id="ErrorMessage" class="error-message" runat="server"></p>
  <div class="button-block default-button-block hidden">
    <a class="use-address">How we use your address</a>
    <ul>
      <li><div class="big-orange-button bob-with-arrow need-address disabled">Evaluate Your Ballot Choices</div></li>
    </ul>
    <p class="or-view">Or view</p>
    <a class="big-orange-button bob-with-arrow need-address elected-officials disabled">Your Currently Elected Officials</a>
  </div>
  <div class="button-block active-button-block hidden">
    <a class="enter-new-address">Enter different address</a>
    <p class="not-available"></p>
    <ul>
    </ul>
    <p class="or-view">Or view</p>
    <a class="big-orange-button bob-with-arrow elected-officials">Your Currently Elected Officials</a>
  </div>
  <div class="button-block enrollment-button-block hidden">
    <a class="enter-new-address">Enter different address</a>
    <ul>
      <li><a class="big-orange-button use-this-address">Use This Address</a></li>
    </ul>
  </div>
</div>
