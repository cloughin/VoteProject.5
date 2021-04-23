<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="SetStaging.aspx.cs" Inherits="Vote.Master.SetStagingPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <asp:UpdatePanel ID="SetStagingUpdatePanel" runat="server">
      <ContentTemplate>
        <asp:RadioButton ID="RadioButtonStagingOff" AutoPostBack="true" runat="server" GroupName="Staging" Text="Staging Off" />
        <br/>
        <asp:RadioButton ID="RadioButtonStagingOn" AutoPostBack="true" runat="server" GroupName="Staging" Text="Staging On" />      
       </ContentTemplate>
    </asp:UpdatePanel>

  </div>
</asp:Content>

