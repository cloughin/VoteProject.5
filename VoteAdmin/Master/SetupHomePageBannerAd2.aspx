<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="SetupHomePageBannerAd2.aspx.cs" 
Inherits="VoteAdmin.Master.SetupHomePageBannerAd2" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<%@ Register TagPrefix="user" TagName="BannerAdInput" Src="~/Controls/BannerAdInput.ascx" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
  </div>
  <user:BannerAdInput ID="BannerAdInput" EnableViewState="false" runat="server" />
</asp:Content>
