<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="VoteNew.Contact" %>

<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/VoteUsaAddress.ascx" TagName="VoteUsaAddress" TagPrefix="user" %>
<%@ Register Src="/Controls/EmailForm.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"  TagPrefix="BotDetect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title runat="server" id="TitleTag" >Contact Us - VoteUSA.org</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="outer">
    <div id="container">
      <user:SocialMediaButtons runat="server" />
      <user:MainBanner runat="server" />
      <user:MainNavigation runat="server" />
      <div id="mainContent">
        <div class="header">
          <img src="/images/contactustitle.png" alt="Contact Us" />
        </div>
        <div class="intro">
          <p>You can contact us via standard mail at:</p>
          <div class="address">
            <p><user:VoteUsaAddress ID="VoteUsaAddress1" runat="server" /></p>
          </div>
          <p>Or send us an email using the form below:</p>
          <user:EmailForm id="EmailForm" runat="server" />
        </div>
        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->
    </form>
</body>
</html>
