<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddSampleBallotButtons.ascx.cs" Inherits="Vote.Controls.AddSampleBallotButtons" %>

<%@ Register Src="/Controls/SampleBallotButton1.ascx" TagName="SampleBallotButton1" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton2.ascx" TagName="SampleBallotButton2" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton3.ascx" TagName="SampleBallotButton3" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton4.ascx" TagName="SampleBallotButton4" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton5.ascx" TagName="SampleBallotButton5" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton6.ascx" TagName="SampleBallotButton6" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton7.ascx" TagName="SampleBallotButton7" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton8.ascx" TagName="SampleBallotButton8" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotButton9.ascx" TagName="SampleBallotButton9" TagPrefix="user" %>

<h2>Add Our Interactive Ballot Choices Buttons to Your Website</h2>

<p>We provide some <a href="/SampleBallotButtons.aspx" class="blue-link">very simple code</a> 
that you can easily add to your website to add any of these 6 buttons linking 
to this website:</p>

<div class="sample-ballot-button-box">
<div class="button-group button-group-1">
<div class="style-1 style-x">
<div id="sample-button-1" class="sample-button">
  <user:SampleBallotButton1 ID="SampleBallotButton1" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-2 style-x">
  <div id="sample-button-2" class="sample-button">
  <user:SampleBallotButton2 ID="SampleBallotButton2" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-3 style-x">
<div id="sample-button-3" class="sample-button">
  <user:SampleBallotButton3 ID="SampleBallotButton3" runat="server" />
</div>
</div>
</div>
<div class="button-group button-group-2">
<div class="style-1 style-x">
<div id="sample-button-4" class="sample-button">
  <user:SampleBallotButton4 ID="SampleBallotButton4" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-2 style-x">
<div id="sample-button-5" class="sample-button">
  <user:SampleBallotButton5 ID="SampleBallotButton5" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-3 style-x">
<div id="sample-button-6" class="sample-button">
  <user:SampleBallotButton6 ID="SampleBallotButton6" runat="server" />
</div>
</div>
</div>
<div class="button-group button-group-3">
<div class="style-1 style-x">
<div id="sample-button-7" class="sample-button">
  <user:SampleBallotButton7 ID="SampleBallotButton7" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-2 style-x">
<div id="sample-button-8" class="sample-button">
  <user:SampleBallotButton8 ID="SampleBallotButton8" runat="server" />
</div>
</div>
<div style="clear:both"></div>
<div class="style-3 style-x">
<div id="sample-button-9" class="sample-button">
  <user:SampleBallotButton9 ID="SampleBallotButton9" runat="server" />
</div>
</div>
</div>
<div style="clear:both"></div>
</div>
