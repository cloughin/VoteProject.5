<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Vote.SignInPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <meta NAME="robot" CONTENT="noindex,nofollow">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <hr class="menu-proxy" />

  <div id="outer" class="outer shadow rounded-border">
    <div class="welcome col">
      <h2>Welcome to Vote-USA.</h2>
      <p runat="server" id="InfoMessage1" class="p1">If you would like to use our administrative tools, please sign in.</p>
      <p runat="server" id="InfoMessage2" class="p2">Or continue to our public <a href="/">home page</a>.</p>
    </div>
    <div class="submit col">
      <asp:UpdatePanel ID="SignInUpdatePanel" runat="server">
        <ContentTemplate>
          <div id="SignInContainer" class="maint-container signin-container rounded-border">
            <div class="col col1">
              <div class="field-label no-ph">Username:</div>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUserName" CssClass="input username" runat="server" placeholder="Username"></user:TextBoxWithNormalizedLineBreaks>
              <div class="field-label no-ph">Password:</div>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPassword" CssClass="input password" placeholder="Password" TextMode="Password" runat="server"></user:TextBoxWithNormalizedLineBreaks>
            </div>
            <div class="col col2 no-ph">
              <asp:Button ID="ButtonSignin" runat="server" CssClass="button-2 signin" 
               Text="Sign In"
               OnClick="ButtonSignin_Click" />
            </div>
            <div style="clear:both"></div>
            <user:FeedbackContainer ID="SignInFeedback" runat="server" />
          </div>
        </ContentTemplate>
      </asp:UpdatePanel>  
    </div>
    <div style="clear:both"></div>
  </div>

</asp:Content>
