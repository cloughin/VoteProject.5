<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forCandidates.aspx.cs" Inherits="Vote.ForCandidatesPage" %>
<%@ Import Namespace="Vote" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/AddSampleBallotButtons.ascx" TagName="AddSampleBallotButtons" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>For Candidates</h1>
  <div class="intro-copy">
    <h2>Promote Your Candidacy</h2>

    <p>Prior to each election, Vote-USA requests the election roster and candidate email 
    addresses from each state election authority. Once we have a candidate’s email address, 
    we send them login credentials and instructions to upload a picture and to enter their 
    biographical information, views on the issues, and website, social media, and 
    YouTube addresses.</p>

    <p>If you have your candidate username and password, you can 
    <a href="<% =UrlManager.ToAdminUrl("/politician/")%>" title="Login as a candidate">login</a>
    to enter information and to promote your candidacy. If you do not have your login 
    credentials, we would be happy to email them to you. Use the form below to provide us with:</p>

    <ul>
      <li>Your name and home state</li>
      <li>The office you are running for</li>
      <li>Your website address, if you have one</li>
    </ul>

    <user:AddSampleBallotButtons ID="AddSampleBallotButtons" runat="server" />

    <h2 class="no-print">Email Us</h2>

    <p class="no-print">Use the form below to email us with any questions or requests:</p>

    <user:EmailForm id="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
