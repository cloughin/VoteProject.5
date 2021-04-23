<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forVolunteers.aspx.cs" Inherits="VoteNew.forVolunteers" %>

<%@ Register Src="/Controls/SocialMediaButtons.ascx" TagName="SocialMediaButtons" TagPrefix="user" %>
<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<%@ Register Src="/Controls/MainNavigation.ascx" TagName="MainNavigation" TagPrefix="user" %>
<%@ Register Src="/Controls/EmailForm.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/MainFooter.ascx" TagName="MainFooter" TagPrefix="user" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"  TagPrefix="BotDetect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title runat="server" id="TitleTag" >For Volunteers - VoteUSA.org</title>
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
          <img src="/images/forvolunteerstitle.png" alt="For Volunteers" />
        </div>

        <div class="intro">
          <p>Working on our project provides interns with an opportunity to better understand, become involved with, and make a valuable contribution to the American democratic 
          political process. Interns can contribute to the project from their home, office, school or anywhere an Internet connection is available. New interns are unpaid, but 
          our paid staff all started as interns. New interns perform two very valuable functions.</p>

          <p>First, we need interns to help us enter the election rosters for each upcoming election in every state and the District of Columbia. States provide us with their 
          rosters about 2 and 6 weeks prior to an election. At that time we need many people working simultaneously on all of the state rosters to get this data into our platform 
          as quickly as possible. This is critical to provide voters with ballots and side-by-side comparison reports well before each election. We provide interns with training 
          and convenient Internet tools to expedite this task.</p>

          <p>Once the rosters are entered, we use interns to investigate, collect and record information and pictures about the candidates. This primarily involves finding the web 
          address of each candidate’s official campaign website. Then each candidate’s biographical profile, picture and views on the issues are copied into our platform.</p>

          <p>We are currently in the process of expanding our database to include county and local office contests and ballot measures. We need interns to identify all of the 
          elected offices for counties and local districts. Interns also coordinate with the state election authorities to set up all upcoming elections, including special elections 
          and primaries.</p>

          <p>To appreciate how valuable your services will be, use the Internet to find the ballot you will see at the polls in the next election. Then try to obtain the information 
          you need to compare the candidates for each contest, and the ballot measures which voters will be asked to decide. We think you will find that each voter is faced with a 
          daunting task to vote on an informed basis. Interns make an enormous contribution to fill this lack of basic voter information.</p>

          <p>Interns perform other very valuable functions, but we like to start all new interns with these two tasks to get them familiar with what we do and how our platform 
          works. If you are interested in helping us, please tell us a little bit about yourself and why you would like to work on this project and let us know which state you would 
          like to be primarily involved with.</p>

          <p>Use the form below to inquire about opportunities to volunteer:</p>
          <user:EmailForm id="EmailForm" runat="server" />
       </div>
        <user:MainFooter runat="server" />
      </div> <!-- id="mainContent" -->
    </div> <!-- id="container" -->
    </div> <!-- id="outer" -->

    </form>
</body>
</html>
