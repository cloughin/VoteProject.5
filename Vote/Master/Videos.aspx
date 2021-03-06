<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="Videos.aspx.cs" Inherits="Vote.Master.VideosPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
    
    *
    { box-sizing: border-box;}
  
    div.instructions
    {
      width: 100%;
      margin: 0 20px 20px 20px;
    }

    div.instructions ul
    {
      color: #bf9241;
      list-style: disc;
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
    .col {
      float: left;
      margin: 0 10px;
    }
    .drop-hover {
      background: yellow;
    }
    .videos-list {
      border: 1px solid #999;
      padding: 3px;
      width: 277px;
      font-size: 110%;
      min-height: 16px;
    }
    .videos-list p {
      padding: 2px 2px 2px 10px;
      text-indent: -10px;    
     }
    .videos-list p:hover {
      background-color: #00ffff;
    }
    .videos-list p.selected {
      background-color: #0000ff;
      color: #fff;
    }
    .remove-button {
      margin-top: 4px;
    }
    .add-button {
      margin-top: 4px;
      float: right;
    }
    .data {
      margin: 0 10px 20px 10px;
      padding-top: 20px;
      clear: both;
    }
    .data h6 {
      margin-top: 10px !important;
    }
    .data input[type=text] {
      width: 100%;
    }
    .data textarea {
      width: 100%;
      height: 57px;
    }
    .update-button {
      margin: 10px 0 20px 0;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    <div class="instructions">
      <ul>
        <li><span>Click the <em>Add New Video</em> button to add a new instructional video.</span></li>
        <li><span>Select a video from <em>All Instructional Videos</em> to edit its details. Click the <em>Delete</em> button to completely delete it.</span></li>
        <li><span>Add a video to <em>Admin Videos</em> or <em>Volunteers Videos</em> by dragging from <em>All Instructional Videos</em>.</span></li>
        <li><span>Reorder videos in <em>Admin Videos</em> or <em>Volunteers Videos</em> by dragging up or down.</span></li>
        <li><span>To remove a video from <em>Admin Videos</em> or <em>Volunteers Videos</em>, select it and click the <em>Remove</em> button.</span></li>
        <li><span>Click <em>Update</em> to save all changes.</span></li>
      </ul>
    </div>
    <div>
      <div class="col all-videos-col">
        <h4>All Instructional Videos</h4>
        <div id="AllVideos" class="all-videos videos-list" runat="server"></div>
        <input type="button" value="Delete" class="remove-button button-3 button-smallest disabled"/>
        <input type="button" value="Add New Video" class="add-button button-2 button-smallest"/>
      </div>
      <div class="col admin-col">
        <h4>Admin Videos</h4>
        <div id="AdminVideos" class="admin-videos video-drop-target videos-list" runat="server"></div>
        <input type="button" value="Remove" class="remove-button button-3 button-smallest disabled"/>
      </div>
      <div class="col volunteers-col">
        <h4>Volunteers Videos</h4>
        <div id="VolunteersVideos" class="volunteer-videos video-drop-target videos-list" runat="server"></div>
        <input type="button" value="Remove" class="remove-button button-3 button-smallest disabled"/>
      </div>
    </div>
    <div class="data">
      <h6>Title</h6>
      <input type="text" disabled="disabled" class="data-item data-title" data-type="title"/>
      <h6>Url</h6>
      <input type="text" disabled="disabled" spellcheck="false" class="data-item data-url" data-type="url"/>
      <h6>Description</h6>
      <textarea disabled="disabled" class="data-item data-description" data-type="description"></textarea>
      <h6>Embed Code</h6>
      <textarea disabled="disabled" spellcheck="false" class="data-item data-embedcode" data-type="embedcode"></textarea>
      <input type="button" value="Update" class="update-button button-1" disabled="disabled"/>
    </div>
  </div>
</asp:Content>
