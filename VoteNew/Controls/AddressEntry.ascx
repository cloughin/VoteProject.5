<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressEntry.ascx.cs" Inherits="VoteNew.Controls.AddressEntry" %>

<div id="addressEntryDialog">

  <div class="inner">
    <a class="dialogClose" onclick="$('#addressEntryDialog').dialog('close')">Close dialog</a>
    <div class="header">
      <img id="HeaderImage" runat="server" src="/images/changelocation.png" alt="Get Started" />
    </div>

    <div class="panel1">
    <div class="subhead">Customize Your Data »</div>
    <div class="text">Either enter your address, ZIP+4 or select your state 
    via the dropdown menu so that we can customize our data for your specific 
    location.</div>

    <div><img id="MiniLogo" src="" runat="server" alt=""/></div>
    </div>

    <div class="divider"><img class="ajaxLoader" alt="Ajax is processing" src="/images/ajax-loader.gif" /><br />&nbsp;</div>

    <div class="panel2">
    <div class="subhead">Get data by address or zipcode</div>
    <div class="textblock">
      <div class="text">Example 1: 555 Oak St. Aspen, CO<br />
      Example 2: 20171-1818 </div>
      <a class="usps" href="http://www.usps.com/zip4" title="Get ZIP+4 from USPS" target="zipplus4">Get ZIP+4 from USPS</a>
    </div>
    <div class="address"><div class="input"><div class="input2"><input type="text" class="addressToFind" /></div></div><a class="find"><img src="/images/findmylocation.png" alt=""/>Find my location</a></div>
    <div class="ajaxMessage"></div>
    <div class="subhead">Get data by state</div>
    <div class="text">Selecting by state will show your statewide information</div>
    <div class="state"><div class="input"><div class="input2"><select id="AddressEntryStatesDropDownList" class="selectState" runat="server"></select></div></div><a class="find"><img src="/images/findmylocation.png" alt=""/>Find my location</a></div>
    <div class="ajaxMessage2"></div>
    </div>
  </div>

</div> <!-- id="addressEntryDialog" -->
