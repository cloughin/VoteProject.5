<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="PatchTest._default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <h1 class="dummy-title" style="text-align: center">Patch Default Page Title</h1>
  <script> var ballotpage = '/patchballotpage.aspx';</script>
  <script type="text/javascript" src="http://vote-usa.org/js/patchdefault.js"></script>
</asp:Content>
