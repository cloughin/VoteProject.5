<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" CodeBehind="SetNoCache.aspx.cs" Inherits="Vote.Master.SetNoCachePage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    p.instructions
    {
      font-size: 12pt;
      margin: 4px 0;
    }
    .set-button
    {
      margin-top: 20px;
    }
    #outer table
    {
      width: 100%;
      font-size: 10pt;
      margin-top: 20px;    
      font-size: 10pt;
    }
    #outer th,
    #outer td
    {
      padding: 3px;
      border: 1px solid #666;
    }
    #outer th
    {
      color: #fff;
      background-color: #666;
    }
    #outer iframe 
    {
      width: 743px;
      height: 15px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    <p class="instructions">Click <em>Set</em> to set a cookie for each of VoteUSA's 52 domains that will suppress public page caching.</p>
    <p class="instructions">Note that this will only be effective for the browser on which it is run.</p>
    <p class="instructions">Also note that if you clear cookies on your browser, this will have to be re-executed.</p>
    <asp:UpdatePanel ID="NoCacheUpdatePanel" runat="server">
      <ContentTemplate>
        <asp:Button ID="Set" CssClass="set-button button-1" runat="server" Text="Set" />
        <table>
          <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
        </table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
