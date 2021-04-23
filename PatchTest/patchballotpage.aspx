<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="patchballotpage.aspx.cs" Inherits="PatchTest.patchballotpage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <h1 class="dummy-title" style="text-align: center">Patch Ballot Choices Page Title</h1>
  <script>
    var comparepage = '/patchcomparepage.aspx';
    var intropage = '/patchintropage.aspx';
    var referendumpage = '/patchreferendumpage.aspx';
  </script>
  <script type="text/javascript" src="http://vote-usa.org/js/patchballot.js"></script>
</asp:Content>
