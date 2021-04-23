<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
CodeBehind="Update.aspx.cs" Inherits="VoteAdmin.Politician.UpdatePage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link rel="stylesheet" href="/css/slicknav.css"/>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <div class="main-heading">
      <h1 id="H1" runat="server"></h1>
      <h3 id="H3" runat="server"></h3>
    </div>

    <div class="change-button-line">
      <span class="div-button save-button button-1 button-smaller enable-if-changed disabled"></span>
      <span class="div-button cancel-button button-3 button-smaller enable-if-changed disabled">Cancel All Changes</span>
    </div>

    <div id="main-tabs" class="main-tabs tab-control htab-control jqueryui-tabs start-hidden shadow">

      <ul class="tabs htabs unselectable">
        <li class="tab htab" EnableViewState="false" runat="server">
          <a href="#tab-general" onclick="this.blur()" EnableViewState="false">General</a>
        </li>
        <li class="tab htab" EnableViewState="false" runat="server">
          <a href="#tab-social" onclick="this.blur()" EnableViewState="false">Social</a>
        </li>
        <li class="tab htab" EnableViewState="false" runat="server">
          <a href="#tab-picture" onclick="this.blur()" EnableViewState="false">Picture</a>
        </li>
        <li class="tab htab" EnableViewState="false" runat="server">
          <a href="#tab-bio" onclick="this.blur()" EnableViewState="false">Bio</a>
        </li>
        <li class="tab htab" EnableViewState="false" runat="server">
          <a href="#tab-issues" onclick="this.blur()" EnableViewState="false">Issues</a>
        </li>
      </ul>

      <div id="tab-general" class="content-panel tab-panel htab-panel">
      </div>

      <div id="tab-social" class="content-panel tab-panel htab-panel">
      </div>

      <div id="tab-picture" class="content-panel tab-panel htab-panel">
      </div>

      <div id="tab-bio" class="content-panel tab-panel htab-panel">
        <div class="accordion-container" id="BioAccordion" runat="server"></div>
      </div>

      <div id="tab-issues" class="content-panel tab-panel htab-panel">
        <div class="search-topics">
          <span>Search for topic</span>
          <div class="search-box">
            <input type="text"/>
            <div class="results"></div>
          </div>
        </div>
        <div class="accordion-container" id="IssuesAccordion" runat="server"></div>
      </div>
    </div>
  </div>
</asp:Content>