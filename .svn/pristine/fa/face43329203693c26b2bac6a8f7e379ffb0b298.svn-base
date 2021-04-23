<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressEntry.ascx.cs" Inherits="Vote.Controls.AddressEntry" %>

<div id="addressEntryDialog">

  <div class="inner">
    <a class="dialogClose" onclick="$('#addressEntryDialog').dialog('close')">Close dialog</a>
    <div class="header">
      <img id="HeaderImage" runat="server" src="/images/yourlocation.png" alt="Get Started" />
    </div>

    <div class="panel1">
    <!--<div class="subhead">Customize Your Data »</div>-->
    <div class="subhead">Your Address, 9 Digit Zip Code, or State »</div>
    <div class="text">To prepare your customized sample ballot or a report of 
    your elected officials please enter an address or a 9 digit zip code in 
    the first text box. For statewide reports only, simply select a 
    state from the dropdown menu of states.</div>

    <div class="miniLogo"><img id="MiniLogo" src="" runat="server" alt=""/></div>
    </div>

    <div class="divider"><img class="ajaxLoader" alt="Ajax is processing" src="/images/ajax-loader16.gif" /><br />&nbsp;</div>

    <div class="panel2">
    <div class="subhead">Address or 9 digit zip code</div>
    <div class="textblock">
      <div class="text">Example 1: 555 Oak St. Aspen, CO<br />
      Example 2: 20171-1818 </div>
      <a class="usps" href="http://www.usps.com/zip4" title="Get 9-digit zip from USPS" target="zipplus4">Get 9 digit zip from USPS</a>
    </div>
    <div class="address"><div class="input"><div class="input2"><input type="text" class="addressToFind" /></div></div><a class="find"><img src="/images/findmylocation.png" alt=""/>Find my location</a></div>
    <div class="ajaxMessage"></div>
    <div class="subhead">Or select state</div>
    <div class="state"><div class="input"><div class="input2"><select id="AddressEntryStatesDropDownList" class="selectState" runat="server"></select></div></div><a class="find"><img src="/images/findmylocation.png" alt=""/>Find my location</a></div>
    <div class="ajaxMessage2"></div>
    </div>
  </div>

</div> <!-- id="addressEntryDialog" -->

<div id="addressEntryEmailDialog">
  <div class="header">
    <div>Get Future Sample Ballots Automatically</div>
  </div>
  <div class="inner">
    <div class="blurb">If you would like us to send future sample ballots as soon as they are available, enter your email address below:</div>
    <div class="inner2">
      <div class="entry"><div class="input"><div class="input2"><input type="text" class="email" /></div></div><a class="emailEnter"><img src="/images/findmylocation.png" alt=""/>Enter email address</a></div>
      <div class="nothanks"><a class="emailNoThanks">No thanks</a></div>
    </div>
  </div>
</div>
