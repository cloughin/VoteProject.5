<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="ReviewYouTubeChannels.aspx.cs" Inherits="Vote.Master.ReviewYouTubeChannelsPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    .youtube-table tr.even {
      background-color: #eeeeee;
    }
    
    .youtube-table {
      margin-top: 10px;
      width: 100%;
    }
    
    .youtube-table td>div {
      padding: 2px 4px 2px 0;
    }
    
    .youtube-table td.name>div,
    .youtube-table td.video>div,
    .youtube-table td.channel-url>div {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    
    .youtube-table td.name>div {
      width: 120px;
    }
    
    .youtube-table td.video>div {
      width: 85px;
    }
    
    .youtube-table td.channel-url>div {
      width: 100px;
    }
    
    .youtube-table td.channel-title>div {
      width: 120px;
    }
    
    .youtube-table td>div.error {
      color: #cc0000;
    }
    
    .youtube-table td.url-to-use>div {
      width: 120px;
    }
    
    .youtube-table td>div.info {
      color: #00cc00;
    }

    div.instructions ul
    {
      color: #bf9241;
      list-style: disc;
      margin: 0 0 0 30px;
    }

    div.instructions li
    {
      margin-bottom: 5px;
    }

    div.instructions li span
    {
      color: #334f80;
      font-size: 11px;
    }

    div.instructions em
    {
      color: #bf9241;
      font-style: normal;
      font-weight: bold;
    }
    
    input.all-videos-button {
      float: right;
    }

  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    <div class="instructions">
      <ul>
        <li><span>The table lists all politicians with an unverified specific YouTube video as their social media link for YouTube.</span></li>
        <li><span>Once the page loads, it begin to query YouTube for the video's channel information.</span></li>
        <li><span>Based on the channel information, you can choose to use the video or to substitute the channel.</span></li>
        <li><span>If you use the video, no changes are made but the video is marked so it will not appear in this table again.</span></li>
        <li><span>If you use the channel, the video is converted to an issues answer under the topic <em>Reasons &amp; Objectives | Why I'm Running for Office</em> (unless it is already there) and the social media link is converted to the channel url.</span></li>
        <li><span>Click the <em>Use Video for All Remaining Entries</em> button to automatically select all remaining <em>Video</em> buttons. This action only applies to entries that have already been queried.</span></li>
        <li><span>If you don't click either button, the video will continue to appear in this table.</span></li>
        <li><span>Hover the mouse over the <em>Video Id</em> or the <em>Channel Id</em> entry to see the complete url. Click on either to view the video or the channel.</span></li>
        <li><span>You can use the <em>Pause Queries</em> button to temporarily suspend the YouTube queries for channel information.</span></li>
        <li><span>You don't need to wait for the queries to complete before beginning to use the buttons.</span></li>
      </ul>
    </div>
    <div>
      <input type="button" value="Pause Queries" class="pause-button button-2 button-smaller"/>
      <input type="button" value="Use Video for All Remaining Entries" class="all-videos-button button-1 button-smaller"/>
    </div>
    <table class="youtube-table">
      <thead>
        <tr>
          <th>Politician</th>
          <th>Video Id</th>
          <th>Channel Id</th>
          <th>Channel Title</th>
          <th>Channel Description</th>
          <th>Url to Use</th>
        </tr>
      </thead>
      <tbody>
        <asp:PlaceHolder ID="BodyPlaceHolder" runat="server"></asp:PlaceHolder>
      </tbody>
    </table>
  </div>
</asp:Content>
