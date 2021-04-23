<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Donate.aspx.cs" Inherits="VoteNew.Donate" %>

<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title runat="server" id="TitleTag" >Donate - VoteUSA.org</title>
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
          <img src="/images/donatetitle.png" alt="Donate" />
        </div>
        <div class="intro">
        <p><em>Please help us strengthen our democracy with a better-informed electorate.</em></p>

        <p>Vote-USA.org currently derives 100% of its financial support from voters like you. If you find
        the information we provide to be valuable to you and other voters, please consider making
        a tax-deductible donation of any size to help us continue to provide these resources.</p>

        <p><strong>Donations do not support any particular candidate.</strong></p>

        <p class="donatetoday"><a href="https://co.clickandpledge.com/Default.aspx?WID=14250" target="donate">Donate now</a></p>
        </div>

        <div class="donatebox"><a href="https://co.clickandpledge.com/Default.aspx?WID=14250" target="donate">Donate now</a></div>

        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->

    </form>
</body>
</html>
