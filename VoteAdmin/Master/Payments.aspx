<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="Payments.aspx.cs" Inherits="Vote.Master.PaymentsPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
<div id="outer">
  <h1 id="H1" runat="server"></h1>

  <asp:UpdatePanel ID="PaymentsUpdatePanel" runat="server">
    <ContentTemplate>
    </ContentTemplate>
  </asp:UpdatePanel>
</div>
</asp:Content>
