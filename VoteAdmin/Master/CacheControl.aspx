<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
 CodeBehind="CacheControl.aspx.cs" Inherits="Vote.Master.CacheControlPage" %>

<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/SubHeadingWithHelp.ascx" TagName="SubHeadingWithHelp" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
    /* page caching container */
    #outer
    {
      margin-bottom: 10px;
    }
    #outer .page-container table
    {
      border-collapse: collapse;
      margin-left: 217px;
    }
    #outer .page-container th,
    #outer .page-container td
    {
      padding: 5px;
      border: 1px solid #ccc;
      font-size: 12px;
    }
    #outer .page-container td.col1
    {
      vertical-align: middle;
    }
    #outer .page-container td.col1 > span
    {
      padding-left: 15px;
    }
    #outer .page-container td.col2
    {
      vertical-align: middle;
      width: 250px;
    }
    #outer .page-container input[type=submit]
    {
      width: 270px;
    }
    #outer .page-container .last-removed,
    #outer .page-container .current-cached
    {
      font-weight: bold;
    }
    #outer .cache-expiration-container td
    {
      padding: 0 10px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
<div id="outer">
  <h1 id="H1" runat="server"></h1>

  <asp:UpdatePanel ID="PageCachingUpdatePanel" runat="server">
    <ContentTemplate>
      <div id="PageCachingContainer" class="maint-container page-container shadow rounded-border">
        <user:SubHeadingWithHelp ID="PageCachingSubHeadingWithHelp" runat="server"
         Title="Page Caching Control" CssClass="tiptip"
         Tooltip="Show/hide information about Page Caching Control">
          <ContentTemplate>
            <p>Clicking this button schedules all cached pages to be cleared. 
            Scheduled cache operations on the common server are processed by a background
            process that runs every 5 minutes. Once the common server has been updated, the
            corresponding operations are scheduled on each load balanced web server. Each
            has its own background process to handle the invalidation, also running every
            5 minutes. Thus it can take up to 10 minutes for changes to completely propagate
            through the site.</p>
            <p>Because of possible synchronization issues. It is possible that some load balanced
            servers may be serving the old page at the same time others are serving the new page.
            This situation is rare and should never last more than 5 minutes.</p>
          </ContentTemplate>
        </user:SubHeadingWithHelp>
        <table>
          <tr>
            <td class="col1">
              <asp:Button ID="ButtonRemoveAllPages" runat="server" CssClass="button-1"
               Text="Remove All Cached Pages"
               OnClick="ButtonRemoveAllPages_Click" /><br />
              <span>Last removed: <asp:Label CssClass="last-removed" ID="LabelAllPagesLastRemoved" runat="server"></asp:Label></span>
            </td>
            <td class="col2">
              Clears <strong>all</strong> pages from the cache table<br />
              Current cached pages: <asp:Label CssClass="current-cached" ID="LabelAllPagesCurrent" runat="server"></asp:Label>
            </td>
          </tr>
        </table>
        <user:FeedbackContainer ID="PageCachingFeedback" runat="server" />
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>

  <asp:UpdatePanel ID="CacheExpirationUpdatePanel" runat="server">
    <ContentTemplate>
      <div id="CacheExpirationContainer" class="maint-container cache-expiration-container shadow rounded-border">
        <user:SubHeadingWithHelp ID="SubHeadingWithHelp1" runat="server"
         Title="Page Caching Expiration"  CssClass="tiptip"
         Tooltip="Show/hide information about setting page caching expiration">
          <ContentTemplate>
            <p>This setting controls how long a cached page is served before it is regenerated. The load balanced 
            web servers cache this value in memory, refreshing the value every 5 
            minutes. Thus it can take up to 5 minutes for a change to take effect.</p>
            <p>Normally set to one hour on the production server and 0 (no caching) on the test server.</p>
          </ContentTemplate>
        </user:SubHeadingWithHelp>
        <asp:RadioButtonList ID="RadioCacheExpiration" runat="server"
          CssClass="cache-expiration-buttons"
          RepeatDirection="Horizontal" AutoPostBack="True" 
          onselectedindexchanged="RadioCacheExpiration_SelectedIndexChanged">
            <asp:ListItem Value="0">0 (no caching)</asp:ListItem>
            <asp:ListItem Value="5">5 minutes</asp:ListItem>
            <asp:ListItem Value="15">15 minutes</asp:ListItem>
            <asp:ListItem Value="60">1 hour</asp:ListItem>
            <asp:ListItem Value="360">6 hours</asp:ListItem>
            <asp:ListItem Value="720">12 hours</asp:ListItem>
            <asp:ListItem Value="1440">1 day</asp:ListItem>
            <asp:ListItem Value="4320">3 days</asp:ListItem>
        </asp:RadioButtonList>
        <user:FeedbackContainer ID="CacheExpirationFeedback" runat="server" />
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>
</div>
</asp:Content>
