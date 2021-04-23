<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="xIssues.aspx.cs" Inherits="Vote.Admin.Issues" %>

<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="uc3" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Issues Add Change and Delete</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body>
  <form id="Form1" method="post" runat="server">
    <uc3:LoginBar ID="LoginBar1" runat="server" />
    <uc1:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table -->
    <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableUpdate2" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="T">
          Order:
          <asp:TextBox
            ID="IssueOrder" runat="server" Width="56px" CssClass="TextBoxInput"></asp:TextBox>&nbsp;
        </td>
        <td class="T">
          &nbsp;Issue Description*:<asp:TextBox ID="IssueDescription" runat="server" Width="367px"
            CssClass="TextBoxInput"></asp:TextBox></td>
        <td class="T">
          &nbsp;Issue ID*:<asp:Label ID="IssueID" runat="server" CssClass="TSmall"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableUpdate" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td colspan="2" class="H">
          Change Name or Order of an Issue</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonUpdate" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Update" OnClick="ButtonUpdate_Click1" Width="100px"></asp:Button></td>
        <td align="left" class="T">
          First click an <b>Issue</b> in
          the right column in the report below of the issue you want to edit. Then change
          the Description and/or Order in the textboxes above. Finally, click <b>Update</b>
          to implement your changes.<br />
          Note: Reasons &amp; Objectives can't be updated becasue all cached Issue.aspx pages
          would have to be removed.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableAdd" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td colspan="2" class="H">
          Add an Issue</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonClear" runat="server" CssClass="Buttons" OnClick="ButtonClear_Click"
            Text="Clear" Width="104px" /><br />
          <asp:Button ID="ButtonAdd" runat="server" CssClass="Buttons" Font-Bold="True" Text="Add"
            OnClick="ButtonAdd_Click" Width="100px"></asp:Button></td>
        <td align="left" class="T">
          Adding an issue is done in several steps. First
          click the Clear button if you need to clear the Order and Description textboxes.
          Then make certain the new issue is not already in the list of issues show in the
          list below. Then enter the description. <b>Make the Description as short as possible</b>
          because it is used to construct a unique Issue ID. You can always edit the description
          later. The Order is optional. If you leave this texbox empty the issue will be added
          to the bottom of the list. Finally, click <b>Add</b>.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableIssue" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H">
          Change Issue Status</td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableRadioButtons" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="middle" align="left" class="T">
          Omit In Next Election:<br />
          <asp:RadioButtonList ID="RadioButtonListOmit" runat="server" CssClass="RadioButtons"
            RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListOmit_SelectedIndexChanged">
            <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
            <asp:ListItem Value="Yes">Yes</asp:ListItem>
          </asp:RadioButtonList></td>
        <td valign="middle" align="left" class="T">
          Tag for Deletion:<br />
          <asp:RadioButtonList ID="RadiobuttonlistDelete" runat="server" CssClass="RadioButtons"
            RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadiobuttonlistDelete_SelectedIndexChanged">
            <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
            <asp:ListItem Value="Yes">Yes</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" valign="middle" class="T">
          Click the <b>Issue ID</b> link whose
          status you want to change from the list below. Then you can change the status by
          clicking one of the radio buttons.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableSubmit" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" colspan="2" class="H">
          Add or Edit Issue Question</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="T">
          Clicking on an <b>Issue Description</b>
          link in the table below will take you to a form where you will be able to add, delete
          and change question topics for that issue.</td>
      </tr>
      <tr>
        <td align="right" class="H" colspan="2">
          Renumber Issues</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonRenumber" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Renumber Issues" OnClick="ButtonRenumber_Click" Width="158px"></asp:Button></td>
        <td align="left" class="T">
          Click this button to renumber all the issues by increments of 10 and retain the
          order.</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="H">
          Delete Tagged Issues</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonDeleteTaggedIssues" runat="server" CssClass="Buttons" OnClick="ButtonDeleteTaggedIssues_Click"
            Text="Delete Tagged Issues" Width="158px" /></td>
        <td align="left" class="T">
          Click this button to permanently delete all issues with a <b>D</b> in the first
          column. Issues will only be deleted if no questions are associated with the Issue.
          Click the [Move or Delete Questions] Button to do this.</td>
      </tr>
      <tr>
        <td colspan="2" class="H">
          Move Questions &amp; Answers</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonMoveQuestion" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Move Questions Answers" OnClick="ButtonMoveQuestion_Click" Width="166px"></asp:Button></td>
        <td align="left" class="T">
          Click this button to permanently delete all issues with a <b>D</b> in the first
          column. Issues will only be deleted if no questions are associated with the Issue.
          Click the [Move or Delete Questions] Button to do this.</td>
      </tr>
      <!--to move-->
      <tr>
        <td align="left" colspan="2" class="H">
          Consolidate Two Issues Into a Single Issue</td>
      </tr>
      <tr>
        <td colspan="2" class="T">
          Click the [Move or Delete Questions] Button to move all the questions into the single
          Issue.
          <br />
          Tag for deletion the Issue with no questions<br />
          Click the [Delete Tagged Issues]
          Button.</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="H">
          Consolidate Two Issues Into a Single Issue</td>
      </tr>
      <tr>
        <td colspan="2" class="T">
          Click the [Move or Delete Questions] Button to move all the questions into the single
          Issue.<br />
          Tag for deletion the Issue with no questions<strong>.<br />
          </strong>Click the [Delete Tagged Issues]
          Button.</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="H">
          Split an Issue into Two Issues</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="T">
          Create a new issue.<br />
          Click
          the [Move or Delete Questions] Button to move questions into the new Issue.</td>
      </tr>
    </table>
    <!-- Table -->
    <asp:Label ID="LabelIssuesTable" runat="server"></asp:Label>
    <!-- Table -->
  </form>
</body>
</html>
