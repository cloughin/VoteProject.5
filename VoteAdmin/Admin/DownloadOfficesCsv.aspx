<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="DownloadOfficesCsv.aspx.cs" Inherits="Vote.Admin.DownloadOfficesCsv" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    .selection-group
    {
      display: inline-block;
      margin-right: 20px;
    }
    .level-selection-group
    {
      width: 100px;
    }
    .incumbent-selection-group
    {
      width: 200px;
    }
    .create-csv-button
    {
      margin-top: 20px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1 id="H1" runat="server"></h1>
  
  <div class="rounded-border boxed-group selection-group level-selection-group">
    <div class="boxed-group-label">Report Level</div>
    <div>
      <input id="level-c" value="C" type="radio" name="level" checked="checked"/><label for="level-c">County</label>
      <br />
      <input id="level-l" value="L" type="radio" name="level"/><label for="level-l">Local</label>
    </div>
  </div>
  
  <div class="rounded-border boxed-group selection-group incumbent-selection-group">
    <div class="boxed-group-label">Incumbents</div>
    <div>
      <input id="include-m" type="checkbox" checked="checked"/><label for="level-c">Include offices missing incumbents</label>
      <br />
      <input id="include-w" type="checkbox" checked="checked"/><label for="level-l">Include offices with incumbents</label>
    </div>
  </div>
  
  <div></div>

  <input class="create-csv-button button-1" type="button" value="Create CSV" />

</asp:Content>
