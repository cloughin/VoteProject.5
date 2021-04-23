<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainNavigation.ascx.cs" Inherits="Vote.Controls.MainNavigation" %>

<div id="mainNavigation">
  <ul class="cssmenu2">
    <li class="home"><a href="/" id="HomeLink" runat="server">Home</a></li>
    <li class="forVoters"><a id="ForVotersLink" runat="server" href="/forVoters.aspx" 
    title="Get your sample ballot or current elected officials">for Voters</a></li>
    <li class="forCandidates"><a href="/forCandidates.aspx" id="ForCandidatesLink" runat="server"
    title="How candidates can provide and update their information">for Candidates</a></li>
    <li class="forVolunteers"><a href="/forVolunteers.aspx" id="ForVolunteersLink" runat="server"
    title="How you can participate in the Vote USA project">for Volunteers</a></li>
    <li class="donate"><a id="DonateLink" class="donateLink"
    title="Help support this project with a donation" target="donate" runat="server">Donate</a></li>
  </ul>
</div>

<div id="donateInfoDialog">
  <div class="header">
    <div>Donate to Vote-USA.org</div>
  </div>
  <div class="inner">
    <div class="blurb">The donation you are about to make will go to Vote-USA.org, not to any particular candidate.</div>
    <div class="inner2">
      <div class="buttons"><a class="proceed">Continue with Donation</a><a class="cancel">Cancel</a></div>
    </div>
  </div>
</div>