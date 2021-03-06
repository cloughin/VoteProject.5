<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forVolunteers.aspx.cs" Inherits="Vote.ForVolunteersPage" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .email-form .narrow {
      max-width: 400px;
    }
    .email-form .email-form-from-email-address {
      display: block;
    }
    .email-form .emailformfirstname-wrapper {
      display: inline-block;
      margin-right: 15px;
    }
    .email-form .emailformlastname-wrapper {
      display: inline-block;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>For Volunteers and Interns</h1>
  <div class="intro-copy">
    <p>The Vote-USA project relies heavily on the efforts of volunteers.  
    To get involved, all you need is an internet connection and the desire. 
    Here are some of ways you can contribute.</p>

    <div class="sub">

      <h4>Become an Intern</h4>

      <p>Each year we host undergraduate and graduate interns with a focus in political science, 
      public policy, social sciences, and other related majors. We post internships with universities 
      throughout the country usually near the beginning of each semester. If your university does 
      not have an internship announcement please fill out the contact information below and an 
      administrator will contact you.</p>

      <h4>Become a Volunteer</h4>

      <p>Volunteers are folks who still want to make significant contributions to their political 
      process, but are not currently enrolled in school. Volunteers share some of the same duties 
      as interns, but play a more physical role in their communities i.e. speaking with local 
      party groups, chambers of commerce, etc. to help spread the work we do at Vote-USA.</p>

      <h4>Contacting Candidates</h4>

      <p>During larger election cycles there are thousands of candidates running throughout the 
      country. While we do scrape information from candidate websites and social media, we would 
      like to give the candidates the opportunity to fill out their Vote-USA profile themselves and 
      provide videos, so they can feel they are speaking directly to the voters. So we need 
      volunteers willing to go into the field to meet and interview the candidates, and to record 
      videos of the candidates expressing their views on issues of their choice. These videos 
      will be posted to YouTube and linked to from pages on our site.</p>

      <h4>Upload Candidate Pictures and Obtain Videos, Website and Social Media Links</h4>

      <p>Candidate pictures, videos and website and social media links are the most important 
      data we provide to voters. We present this data in our ballots 
      and reports to help voters compare candidates. We need help in capturing this information 
      and provide the tools to make it easy to do.</p>

      <h4>Obtain Biographical Information and Candidates&rsquo; Views on the Issues</h4>

      <p>This information is also very important but can be easily ferreted out by voters with 
      the links we provide to their website and social media pages. When there is a state election 
      or primary of particular importance, like presidential primaries or state general elections 
      we believe it is worth the effort to dig out this information to make it easily available 
      for voters to make comparisons.</p>

      <h4>Enter State Election Rosters for Upcoming Elections</h4>

      <p>Each state provides us with its election roster prior to each election. These rosters 
      must be loaded into our platform as quickly as possible so that we can provide timely 
      information to the voters. Here again we could use help.</p>

    </div>

    <p class="no-print">If you are interested in helping Vote-USA, please use to form below to 
    provide us with the information we need to communicate with you. If you would like, please 
    use the Message text box to tell us a little bit about yourself and/or how you would like 
    to become involved with our project.</p> 

    <user:EmailForm id="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
