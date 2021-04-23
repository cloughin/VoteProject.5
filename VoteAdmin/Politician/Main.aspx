<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Vote.Politician.MainPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
    body.admin-page h1
    {
      margin-top: 10px !important;
    }
    #outer .main-image
    {
      border: 1px solid #ccc;
      margin: 40px 0 0 40px;
    }
    #outer .top-left
    {
      float: left;
      width: 698px;
    }
    #outer .top-left p
    {
      font-size: 13px;
      line-height: 150%;
      color: #707070;
      font-family: "trebuchet ms",helvetica,sans-serif;
      margin-bottom: 10px;
    }
    #outer .top-left p.first
    {
      margin-top: 25px;
    }
    #outer .top-left p.want-to-do
    {
      font-weight: bold;
      margin-top: 25px;
      font-size: 21px;
      color: #25a;
    }
    #outer .top-right
    {
      float: left;
      width: 202px;
    }
    #outer .grid
    {
      display: grid;
    }
    #outer .intro-actions
    {
      margin-right: 15px;
      width: 320px;
      grid-column: 1;
    }
    #outer .issue-actions
    {
      width: 350px;
      grid-column: 2;
    }
    #outer .actions
    {
      grid-row: 1;
    }
    #outer .actions.view
    {
      grid-row: 2;
    }
    #outer a.action
    {
      border: 1px solid #bf9241;
      padding: 10px;
      margin-bottom: 15px;
      display: block;
      text-decoration: none;
    }
    #outer a.action:hover
    {
      text-decoration: none;
      color: inherit;
      border-color: #ffaf1e;
      cursor:pointer;
    }
    #outer a.action p
    {
      margin-bottom: 0;
    }
    #outer a.action p em
    {
      font-size: 18px;
      font-style: normal;
      font-weight: bold;
      color: #25a;
    }
    #outer a.action p em span
    {
      font-size: 16px;
      font-weight: normal;
      color: #bf9241;
    }
    #outer  a.action ul
    {
      list-style: disc;
      margin: 5px 0 0 50px;
      color: #bf9241;
    }
    #outer a.action li
    {
      font-family: "trebuchet ms",helvetica,sans-serif;
      font-size: 13px;
      line-height: 18px;
      color: #bf9241;
    }
    #outer a.action li span
    {
      color: #707070;
    }
    #outer hr
    {
      margin-top: 20px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <hr/>

    <div class="top-left">
      <h1 id="H1" runat="server"></h1>
      <h2 id="H2" runat="server"></h2>
      <h2 id="H3" runat="server"></h2>
<%--      <p class="first">Here we give you tools to enter and update the information 
        that we present to voters about you, your candidacy and your accomplishments in office.</p>--%>
      <p class="first">Here you can enter and update the information 
        that we present to voters about you and your candidacy.</p>
<%--      <p>You can provide basic introductory information as well as communicate your 
        detailed views on a wide variety of issues. For every
        election we present voters with a comparison of each candidate's positions and views.</p>--%>
      <p>For every
        election we present voters with a comparison of each candidate's responses.</p>
      <p class="want-to-do">What do you want to do?</p>
      <div class="grid">
        <div class="actions intro-actions">
          <a class="action gradient-bg-1-hovering shadow rounded-border" id="UpdateIntroLink" runat="server">
            <p><em><span>&#x25ba;</span>Enter or Update</em> your introductory info</p>
            <ul>
              <li><span>Contact information</span></li>
              <li><span>Picture</span></li>
              <li><span>Web site and social media links</span></li>
              <li><span>Biographical information</span></li>
            </ul>
          </a>
        </div>
        <div class="actions issue-actions">
          <a class="action gradient-bg-1-hovering shadow rounded-border" id="UpdateIssuesLink" runat="server">
            <p><em><span>&#x25ba;</span>Enter or Update</em> your positions and views on:</p>
            <ul>
              <li><span>Current issues</span></li>
              <li><span>...and much more</span></li>
            </ul>
          </a>
        </div>
        <div class="actions intro-actions view">
          <a class="action gradient-bg-1-hovering shadow rounded-border" id="ShowIntroLink" runat="server" target="Show">
            <p><em><span>&#x25ba;</span>View</em> your introductory page</p>
          </a>
        </div>
        <div class="actions issue-actions view" id="CompareLink" runat="server">
          <a class="action gradient-bg-1-hovering shadow rounded-border" id="ShowCompareLink" runat="server" target="Show">
            <p><em><span>&#x25ba;</span>View</em> page comparing you and your opponents</p>
          </a>
        </div>
      </div>
    </div>

    <div class="top-right">
      <asp:Image ID="MainImage" CssClass="main-image" runat="server" />
    </div>

    <div class="clear-both"></div>
    <hr />

  </div>
</asp:Content>
