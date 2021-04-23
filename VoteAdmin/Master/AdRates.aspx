<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master"
AutoEventWireup="true" CodeBehind="AdRates.aspx.cs"
Inherits="VoteAdmin.Master.AdRatesPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    <div id="AdRatesContainer" clientidmode="Static" runat="server"></div>
    <input type="button" class="button-1 update-button" disabled="disabled" value="Update"/>
  </div>
</asp:Content>
