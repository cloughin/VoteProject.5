<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="QuestionAnswers.aspx.cs"
  Inherits="Vote.Admin.QuestionAnswers" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <h1 id="H1" runat="server"></h1>
    
  <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

  <div id="UpdateControls" class="update-controls" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label></td>
        <td align="left" class="HLarge">
        </td>
      </tr>
      <tr>
        <td colspan="2" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="H" colspan="2" valign="middle">
          Report of Issues, Questions and Answers</td>
      </tr>
      <tr>
        <td valign="middle">
          <asp:HyperLink ID="HyperLinkReport" runat="server" CssClass="HyperLink" Target="view"
            >Report</asp:HyperLink></td>
        <td align="left" class="T" width="100%">
          Click this button to obtain
          a report of all Issues, Questions and Answers. The report provides the IDs to move
          and delete questions and answers for the functions below.</td>
      </tr>
      <tr>
        <td align="left" colspan="2" class="H">
          Move Question 
          and Answers from One Issue to Another Issue</td>
      </tr>
      <tr>
        <td colspan="2" class="T">
          To move a question and
          all associated answers to a different Issue: (a) Copy the Question ID to move into
          the textbox provided. (b) Copy the Issue ID where the question is to be moved to.
          (c) Click <b>Move</b> button. NOTE: It will take a few seconds to perform this operation.</td>
      </tr>
      <tr>
        <td align="right" class="T">
          <asp:Button ID="ButtonMoveQuestion" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Move Question" OnClick="ButtonMoveQuestion_Click1" Width="150px"></asp:Button></td>
        <td align="left" class="T">
          &nbsp;Move Question and Answers FROM Question ID:<user:TextBoxWithNormalizedLineBreaks ID="Textbox_QuestionKey_Move_Question_From"
            runat="server" CssClass="TextBoxInput" Width="400px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;&nbsp;<br />
          TO Issue with Issue ID:
          <user:TextBoxWithNormalizedLineBreaks ID="Textbox_IssueKey_Move_To" runat="server" CssClass="TextBoxInput" 
            Width="400px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;</td>
      </tr>
      <tr>
        <td colspan="2" class="H">
          Move 
          Answers from One Question to Another Question (Issue can be same or differnt) 
          Then Delete Question</td>
      </tr>
      <tr>
        <td colspan="2" class="T">
          If a politician
          answered both questions the most recent answer is kept and the other deleted. After
          all the answers have been moved the question with the answers being moved, will
          be deleted.</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonMoveAnswers" runat="server" CssClass="Buttons" Text="Move Answers"
            OnClick="ButtonMoveAnswers_Click" Width="150px" /></td>
        <td align="left" class="T">
          &nbsp;Move Answers FROM Question ID:<user:TextBoxWithNormalizedLineBreaks 
            ID="Textbox_QuestionKey_Move_Answers_From" runat="server"
            CssClass="TextBoxInput" Width="400px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;<br />
          TO Question ID:
          <user:TextBoxWithNormalizedLineBreaks ID="Textbox_QuestionKey_Move_Answers_To" runat="server" 
            CssClass="TextBoxInput" Width="400px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td colspan="2" class="H">
          Utility to Insure the IssueKeys are correct for all Answers</td>
      </tr>
      <tr>
        <td align="right">
          <asp:Button ID="ButtonMatchKeys" runat="server" CssClass="Buttons" Text="Run Utility"
            OnClick="ButtonMatchKeys_Click" Width="150px" /></td>
        <td class="T">
          Click this button to update all the IssueKeys to be correct 
          for the QuestionKey for all rows int the Answers Table.
          If there is no Question for the QuestionKey in the Questions Table the answer is 
          deleted. May take 10 minutes or more.</td>
      </tr>
      <!-- Table -->
    </table>
    </ContentTemplate>
  </asp:UpdatePanel>
  </div>
</asp:Content>
