<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="FixIssues.aspx.cs" Inherits="Vote.Master.FixIssuesPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    <!-- Table -->
    <table class="tableAdmin" id="TableOuter" cellspacing="0" cellpadding="0">
      <tr>
        <td colspan="1" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td colspan="1">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="T">
          The button below will check that there is a matching IssueKey in the Issues Table
          for each row in the Answers and Questions Tables. IssueKeys that are not in the
          Issues Table are listed in the Bad IssueKeys column. Valid IssueKeys are listed
          in the right column.</td>
      </tr>
      <tr>
        <td class="TBold">
          <asp:Button ID="Button_Bad_Issue_Keys" runat="server" CssClass="Buttons" OnClick="Button_Bad_Issue_Keys_Click"
            Text="Check for Bad IssueKeys" Width="200px" /></td>
      </tr>
    </table>
    <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="TBold">
          Change All IssueKeys <strong></strong>
        </td>
      </tr>
      <tr>
      <td class="TBold">
        <strong>From</strong>:<user:TextBoxWithNormalizedLineBreaks ID="TextBox_IssueKey_From" runat="server" CssClass="InputTextBox"
          Width="300px"></user:TextBoxWithNormalizedLineBreaks>
        <strong>To</strong>:<user:TextBoxWithNormalizedLineBreaks ID="TextBox_IssueKey_To" runat="server" CssClass="InputTextBox"
          Width="300px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;</td>
      </tr>
      <tr>
        <td class="T">
          To fix a bad IssueKey, select the Bad IssueKey and the Good Issue Key which will
          replace the bad IssueKey.<br />
          Then click the button below.</td>
      </tr>
      <tr>
        <td class="TBold">
          <asp:Button ID="Button_Make_IssueKey_Change" runat="server" CssClass="Buttons" Text="Make IssueKey Change"
            Width="200px" OnClick="Button_Make_IssueKey_Change_Click" /></td>
      </tr>
    </table>
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border=1>
    <tr>
    <td class="HSmall"> Bad IssueKeys</td>
    <td class="HSmall">Good IssueKeys</td>
    </tr>
      <tr>
        <td valign="top">
          <asp:ListBox ID="ListBox_Bad_IssueKeys" runat="server" CssClass="T" Rows="50" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ListBox_Bad_IssueKeys_SelectedIndexChanged">
          </asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_Good_IssueKeys" runat="server" CssClass="T" Rows="50" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ListBox_Good_IssueKeys_SelectedIndexChanged">
          </asp:ListBox></td>
      </tr>
    </table>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
