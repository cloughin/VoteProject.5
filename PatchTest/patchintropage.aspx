<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="patchintropage.aspx.cs" Inherits="PatchTest.patchintropage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <h1 class="dummy-title" style="text-align: center">Patch Intro Page Title</h1>
  <script>
    var comparepage = '/patchcomparepage.aspx';
  </script>
  <script type="text/javascript" src="http://vote-usa.org/js/patchintro.js"></script>
</asp:Content>
