<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forPoliticalParties.aspx.cs" Inherits="Vote.ForPoliticalPartiesPage" %>
<%@ Import Namespace="Vote" %>

<%@ Register Src="/Controls/EmailFormResponsive.ascx" TagName="EmailForm" TagPrefix="user" %>
<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/AddSampleBallotButtons.ascx" TagName="AddSampleBallotButtons" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>For Political Parties</h1>

  <div class="intro-copy">

    <h2>Enter Information About Your Party Candidates</h2>

    <p>Prior to each election, Vote-USA requests the election roster from each 
    state election authority.  For all political parties involved, if we have their 
    email address, we send them login credentials and instructions to upload their 
    candidate’s pictures and to enter biographical information and their views on 
    the issues.</p>

    <p>If you are an officer or legitimate representative of a political party and 
    you have your username and password, you can 
    <a href="<% =UrlManager.ToAdminUrl("/party/")%>" title="Login as a political party representative">login</a> 
    to enter information about your candidates.
    If you do not have your login credentials, we would be happy to email them to you. 
    We will need the following:</p>

    <ul>
      <li>Your complete name, state, and the political party you represent.</li>
      <li>Some proof of your identity.</li>
    </ul>
          
    <p>You can use either of these ways to prove your identity:</p>
          
    <ul>
      <li>Send us an email with the web address of the page on your party’s website 
      where you are listed as an officer.</li>
      <li>Send us an email with an email address associated with your position in 
      the political party.</li>
    </ul>

    <user:AddSampleBallotButtons ID="AddSampleBallotButtons" runat="server" />

    <h2 class="no-print">Email Us</h2>

    <p class="no-print">Use the form below to email us with any questions or requests:</p>

  <user:EmailForm id="EmailForm" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
