<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="patchcomparepage.aspx.cs" Inherits="PatchTest.patchcomparepage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
  <h1 class="dummy-title" style="text-align: center">Patch Compare Page Title</h1>
  <script>
    var intropage = '/patchintropage.aspx';
  </script>
  <script type="text/javascript" src="http://vote-usa.org/js/patchcompare.js"></script>
</asp:Content>
