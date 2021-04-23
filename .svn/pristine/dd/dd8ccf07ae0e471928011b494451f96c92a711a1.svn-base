<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forCandidates.aspx.cs" Inherits="VoteNew.forCandidates" %>

<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/EmailForm.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"  TagPrefix="BotDetect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title runat="server" id="TitleTag" >For Candidates - VoteUSA.org</title>
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
          <img src="/images/forcandidatestitle.png" alt="For Candidates" />
        </div>

        <div class="intro">
          <p>If you are a candidate whose name appears on this website, Vote-USA.org should have sent you an email containing instructions 
          on how to securely enter information about yourself, upload your picture, and express your opinions and views on the issues.</p> 

          <p>If you did not receive an email or no longer have it, we can provide you with your username, password, and instructions for 
          entering your data. However, you will need to provide us with the following before we can do so:</p>

          <ul>
            <li>Your complete name, state, and the office you are running for.</li>
            <li>Some proof of your identity.</li>
          </ul>

          <p>You can use either of these ways to prove your identity:</p>

          <ul>
            <li>Send us an e-mail using an email address associated with your candidacy or that matches the URL of your campaign website.</li>
            <li>Provide us with some other proof of identity like a scanned copy of your driver’s license or some other official document.</li>
          </ul>

          <p>Use the form below to email us with any questions or requests:</p>
          <user:EmailForm id="EmailForm" runat="server" />
          </div>
        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->

    </form>
</body>
</html>
