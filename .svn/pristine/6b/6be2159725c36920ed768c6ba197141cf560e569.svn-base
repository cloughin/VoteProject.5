<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="Default.aspx.cs" Inherits="VoteNew.Default" %>

<%@ Register Src="/Controls/SiteVerification.ascx" TagName="SiteVerification" TagPrefix="user" %>
<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/MissionSidebar.ascx" TagName="MissionSidebar" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>
<%@ Register Src="/Controls/AddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title runat="server" id="TitleTag" >State Elections, Ballots, Candidate Biographical and Issue Comparisons - VoteUSA.org</title>
    <meta runat="server" id="MetaDescriptionTag" content="Custom Sample Ballots" name="description" />
    <meta runat="server" id="MetaKeywordsTag" content="Vote, Custom Ballot, Ballots" name="keywords" />
    <user:SiteVerification runat="server" />
</head>
<body>
    <form id="form1" runat="server" enableviewstate="True">
    <div id="outer">
    <div id="container">
      <user:SocialMediaButtons runat="server" />
      <user:MainBanner runat="server" />
      <user:MainNavigation runat="server" />
      <div id="mainContent">
        <div id="informedBlock">
        <div id="peopleVoting"><img src="/images/peoplevoting.jpg" alt="people voting" /></div>
        <div id="becomingInformed">
          <div id="becomingInformedTitle"><img src="/images/becominginformed.png" alt="Becoming an Informed Voter Just Got Easier" /></div>
          <div id="becomingInformedText"><div>Get your customized sample ballot and evaluate 
            your candidates and ballot measures.  Pictures, bios, and, most importantly, 
            the candidates’ positions on the issues are all presented side-by-side for 
            easy comparison.  And, all information is candidate authored or obtained 
            from the candidates’ websites.  Vote-USA has no political agenda.  
            We can also provide your elected representatives and information on 
            elections going back to 2004.  So …</div></div>
          <div id="getStarted">
            <a class="getStartedButton" title="Let's Get Started" onclick="$('#addressEntryDialog').dialog('open');">Let's Get Started</a>        
          </div>
        </div>
        </div>
        <div id="missionBlock">
          <div id="ourMission"><div>Our Mission</div></div>
          <div id="missionStatement">
            <div>Our democracy is so precious; we feel there must 
            be a better way to make it work.  Rather than electing our representatives 
            based on money, deceptive advertising, and political posters, we feel that 
            the Internet and social media can provide a truly vibrant and thriving 
            representative democratic environment.  Ancient Greece was the crucible 
            of Western civilization and democracy.  In an arena, citizens would gather 
            to debate and decide public policy.  Clearly, that model would not work 
            for America; however, the model we present here could work.  Finally, 
            consider the positive impact that the Internet and social media played 
            on the recent events in Tunisia, Egypt, and Algeria.</div>
          </div>
          <user:MissionSidebar runat="server" />
        </div>
        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->

    <user:AddressEntry ID="AddressEntry" runat="server" />

    </form>

</body>
</html>
