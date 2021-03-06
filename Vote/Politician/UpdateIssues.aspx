<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
ValidateRequest="false"
AutoEventWireup="true" CodeBehind="UpdateIssues.aspx.cs" 
Inherits="Vote.Politician.UpdateIssuesPage" %>
<%@ Import Namespace="DB.VoteCache" %>
<%@ Import Namespace="Vote" %>

<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/SubHeadingWithHelp.ascx" TagName="SubHeadingWithHelp" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <input type="hidden" id="defer-update-answer-init" value="true"/>
  <div id="outer">
  <div class="main-heading">
  <h1 id="H1" EnableViewState="false" runat="server"></h1>
  <h3 id="H2" EnableViewState="false" runat="server"></h3>
  </div>
  
  <div class="politician-links">
      <div class="actions intro-actions">
      <a class="action gradient-bg-1-hovering shadow rounded-border" id="UpdateIntroLink" runat="server">
      <p><em><span>&#x25ba;</span>Update</em> your intro page</p>
      </a>
      <a class="action gradient-bg-1-hovering shadow rounded-border" id="ShowIntroLink" runat="server" target="Show">
      <p><em><span>&#x25ba;</span>View</em> your intro page</p>
      </a>
      </div>
      <div class="actions issue-actions">
      <a class="action gradient-bg-1 disabled shadow rounded-border" id="UpdateIssuesLink" runat="server">
      <p><em><span>&#x25ba;</span>Enter</em> your positions on the issues</p>
      </a>
      <a class="action gradient-bg-1-hovering shadow shadow rounded-border" id="ShowPoliticianIssueLink" runat="server" target="Show">
      <p><em><span>&#x25ba;</span>View</em> your positions page</p>
      </a>
      </div>
  </div>
  
  <div class="clear-both"></div>
  <p class="slo-load">Please be patient &mdash; this page takes a while to load, particularly in Internet Explorer.</p>
  <p class="slo-load">Note: it is much faster in Chrome or Firefox.</p>
  
  <div class="start-hidden search-box maint-container shadow rounded-border">
    <h6>Search for topic</h6>
    <input type="text"/>
    <div class="results"></div>
  </div>
  
  <div class="start-hidden content-header maint-container shadow rounded-border">
    <user:SubHeadingWithHelp ID="PageCachingSubHeadingWithHelp" EnableViewState="false" runat="server"
      Title="Enter or Update Your Positions and Views" CssClass="tiptip"
      Tooltip="Show/hide help">
      <ContentTemplate>
        <p>Because we provide such a large number of topics you can respond on,
        we have organized them on three levels. To get to a topic:</p>
        <ul>
        <li>Select a <strong>category</strong> from the tabs across the top</li>
        <li>Select an <strong>issue</strong> from the tabs on the left</li>
        <li>Select a <strong>topic</strong> by clicking on an accordion bar in the main panel</li>
        </ul>
        <p><strong><em>A note about caching...</em></strong></p>
        <p>When you make changes to your pages through these tools and then click the 
        <strong><a title="View Issues Page" id="ViewIssuesLink" EnableViewState="false" target= "Show" runat="server">
        View Issues Page</a></strong> menu item, we immediately show you how the 
        changes will affect your pages. However, pages on the Vote-USA.com public site 
        are <em>cached</em>, meaning they are not immediately updated with changes. 
        The changes you make here could take up to <%=CachePages.DisplayExpiration(MemCache.CacheExpiration) %> to be seen on the 
        public site.</p>
     </ContentTemplate>
    </user:SubHeadingWithHelp>
  </div>

  <div runat="server" id="ContainerUpdateAll" class="start-hidden content-footer maint-container shadow rounded-border mc mc-g-containerall">
    <asp:UpdatePanel ID="UpdatePanelUpdateAll" EnableViewState="false" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <div class="footer-item footer-feedback">
          <user:FeedbackContainer ID="FeedbackUpdateAll" EnableViewState="false" 
          Placeholder="Use <span>Update All</span> to apply all pending changes at once. Or click <span>Update</span> at the bottom of any panel to apply just those changes."
          CssClass="mc mc-g-feedbackall" runat="server" />
        </div>
        <div class="footer-item footer-ajax-loader">
          <asp:Image ID="ImageAjaxLoader" EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
          CssClass="ajax-loader mc mc-g-ajaxloader" runat="server" />
        </div>
        <div class="footer-item footer-button">
          <asp:Button ID="ButtonUpdateAll" EnableViewState="false" CssClass="button-2 mc mc-g-buttonall tiptip" 
          Tooltip="Update all unsaved data"
          OnClick="ButtonUpdateAll_OnClick" runat="server" Text="Update All" />
        </div>
        <div class="clear-both"></div>
      </ContentTemplate>
    </asp:UpdatePanel>    
  </div>

  <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">

      <asp:PlaceHolder ID="MainTabsPlaceholder" EnableViewState="false" runat="server"></asp:PlaceHolder>

  </div>

</div>
</asp:Content>
