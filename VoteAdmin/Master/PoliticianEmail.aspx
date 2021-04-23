<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="PoliticianEmail.aspx.cs" Inherits="Vote.PoliticianEmail.PoliticianEmail" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%--
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
          &nbsp;Politician Email</td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableEmail" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="T">
          <asp:Button ID="ButtonRecord" runat="server" OnClick="ButtonRecord_Click" Text="Record"
            Width="193px" CssClass="Buttons" />[[USERNAME]] | [[PASSWORD]] | [[VOTEXXANCHOR]] | [[MGREMAIL]]
          | [[INTROANCHOR]] | [[STATE]] | [[POLITICIANENTRY]] substitutions allowed.</td>
      </tr>
      <tr>
        <td class="T">
          Existing Politician:<br />
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmailText1" runat="server" CssClass="TextBoxInputMultiLine" Height="235px" TextMode="MultiLine" Width="688px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T">
          Politician Just Added:<br />
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmailText2" runat="server" CssClass="TextBoxInputMultiLine" Height="235px" TextMode="MultiLine" Width="689px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>