<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="PoliticianFind.aspx.cs" Inherits="Vote.Master.FindPolitician" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
     <!-- Table -->
   <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          Find Politician</td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
     <tr>
       <td class="T">
         State Code is optional</td>
     </tr>
      <tr>
        <td class="T">
          State Code:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStateCode" runat="server" CssClass="TextBoxInput" Width="64px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          Last Name:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLastName" runat="server" CssClass="TextBoxInput" 
            Width="180px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          <asp:Button ID="ButtonFindPolitician" runat="server"
            Text="Find Politician" Width="189px" CssClass="Buttons" 
            onclick="ButtonFindPolitician_Click" /></td>
      </tr>
    </table>
     <!-- Table -->
   <table class="tableAdmin" id="TableEmail" cellspacing="0" cellpadding="0" runat=server>
      <tr>
        <td class="T">
          Email Address of Request:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Email" runat="server" AutoPostBack="True" 
            CssClass="TextBoxInput" Width="500px" 
            ontextchanged="TextBox_Email_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableReport" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="T">
          <asp:Label ID="ReportPoliticians1" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="Report1" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="Report2" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelPoliticianPage" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelSendEmail" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td>
        </td>
      </tr>
    </table>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
