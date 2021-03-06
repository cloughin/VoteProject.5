<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="Issues.aspx.cs"
  Inherits="Vote.Admin.IssuesPage" %>

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
    
  <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

  <div id="UpdateControls" class="update-controls" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
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
    <table id="Table_Question_Edit" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
      runat="server">
      <tr>
        <td align="left" class="H">
          Edit Question</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Questions must be 80 characters or less.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Question_Description" runat="server" CssClass="TextBoxInput"
            Width="600px" AutoPostBack="True" OnTextChanged="TextBoxQuestionDescription_TextChanged"></user:TextBoxWithNormalizedLineBreaks>Order:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Question_Order" runat="server" CssClass="TextBoxInput" Width="35px"
            AutoPostBack="True" OnTextChanged="TextBoxQuestionOrder_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table_Question_Add" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
      runat="server">
      <tr>
        <td align="left" class="H">
          Add Question</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Questions must be 80 characters or less.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Question_Description_Add" runat="server" CssClass="TextBoxInput" OnTextChanged="TextBoxIssueDescription_TextChanged"
            Width="600px"></user:TextBoxWithNormalizedLineBreaks>Order:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Question_Order_Add" runat="server" CssClass="TextBoxInput" OnTextChanged="TextBoxIssueOrder_TextChanged"
            Width="35px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin" id="Table_Question" runat=server>
      <tr>
        <td align="left" class="H" colspan="2">
          Question</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Add a Question</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="Button_Add_Question" runat="server" CssClass="Buttons" Text="Add a Question" Width="150px"
            OnClick="ButtonAddQuestion_Click" />
        </td>
        <td align="left" class="T">
          To add a question:<br />
          1) Make certain the new question is not already in the list of questions in the
          report
            below.
            <br />
          2) Click this button<br />
          3) Enter the question and order in the textboxes provided. The question needs to
          be 100 characters or less. The Order is optional. If left empty the issue will be
          added first.<br />
          4) Click this button again to complete the add.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Changes to Question</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          Click on a question in the second columnn to change the question or question order.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Omit or Reinstate a Question</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          Click on an 'ok' of 'OMIT' link in the third column to omit or reinstate the question
          respectfully. OMIT will omit the Issue or Question on all public pages but will 
          retain the data in the database for historical purposes. It is also a way to 
          remove and issue or questions before it is deleted.</td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin" id="Table_Delete_Question"
    runat=server>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Delete a Question (MASTER only)</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          To delete a question there must be no answers associated with the question. Then 
          copy and pasted the QuestionKey in the textbox provided and click the Dlete this 
          Question Button.<br />
          If there are answers, you to either reassign them to another question or delete 
          them. To delete the answers copy and pasted the QuestionKey in the textbox provided 
          and click the Delete All Answers to this Question Button.</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="Button_Delete_Question" runat="server" CssClass="Buttons" Text="Delete this Question" Width="150px"
            OnClick="ButtonDeleteQuestion_Click" /></td>
        <td align="left" class="T" width="100%">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Delete_QuestionKey" runat="server" CssClass="TextBoxInput"
            Width="250px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          <asp:Button ID="Button_Delete_Answers" runat="server" CssClass="Buttons" 
            onclick="ButtonDeleteAnswers_Click" 
            Text="Delete All Answers to this Question" />
        </td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Utility</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          <asp:Button ID="Button_Delete_All_Questions" runat="server" CssClass="Buttons" OnClick="ButtonDeleteAllQuestions_Click"
            Text="Delete ALL Questions with NO Answers" Width="300px" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin" id="Table_Questions_Report"
    runat=server>
      <tr>
        <td align="left" class="HSmall">
          Questions for Issue</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Label ID="LabelQuestionsReport" runat="server"></asp:Label>
          <!-- Table -->
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table_Issue_Edit" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
      runat="server">
      <tr>
        <td align="left" class="H">
          Edit Issue</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Issue Descriptions must be 40 characters or less.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          Issue:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Issue_Description" runat="server" CssClass="TextBoxInput"
            Width="320px" AutoPostBack="True" OnTextChanged="TextBoxIssueDescription_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          Issue Order:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Issue_Order" runat="server" CssClass="TextBoxInput" Width="35px"
            AutoPostBack="True" OnTextChanged="TextBoxIssueOrder_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <table id="Table_Issue_Add" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
      runat="server">
      <tr>
        <td align="left" class="H">
          Add Issue</td>
      </tr>
      <tr>
        <td align="left" class="T">
          When adding an issue, make the Issue Description 20 characters or less. It is used to construct a short 
          unique Issue ID. Look at the IssueKey column for examples. The Order is optional.
          If left empty the issue will be added first.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          Issue:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Issue_Description_Add" runat="server" CssClass="TextBoxInput"
            OnTextChanged="TextBoxIssueDescription_TextChanged" Width="166px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          Issue Order:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Issue_Order_Add" runat="server" CssClass="TextBoxInput"
            OnTextChanged="TextBoxIssueOrder_TextChanged" Width="35px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2">
          Issues</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Add an Issue</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="Button_Add_Issue" runat="server" CssClass="Buttons" Text="Add an Issue"
            Width="150px" OnClick="ButtonAddIssue_Click" />
        </td>
        <td align="left" class="T">
          To add an issue:<br />
          1) Make certain the new issue is not already in the list of issues in the report below.
          <br />
          2) Click this button<br />
          3) Enter the description and order in the textboxes provided. 
          <br />
          4) Click this button again to complete the add.<br />
          5) Click on the newly added issue in the report if the description needs to be edited
          or expanded. The issue description must be 40 characters or less.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Changes to Issue</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2" style="height: 13px">
          Click on an 'ok' of 'OMIT' link in the third column to omit or reinstate the question
          respectfully. OMIT will omit the Issue or Question on all public pages but will 
          retain the data in the database for historical purposes. It is also a way to 
          remove and issue or questions before it is deleted.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2" style="height: 13px">
          Omit or Reinstate an Issue</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          Click on an 'ok' of 'OMIT' link in the third column to omit or reinstate the issue
          respectfully.</td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin" id="Table_Delete_Issue"
    runat=server>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Delete an Issue (MASTER only)</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          To delete an issue there must be no questions 
          or answers associated with the issue. Then copy and pasted the IssueKey in the textbox provided 
          and click the Delete this Issue Button.<br />
          If there are questions or answers associated with this issue, select the issue, 
          then follow the instructions to delete the question(s) and answers associated 
          with this issue.<br />
          Usually the only time an issue should be deleted is when a new issue is 
          accidentally created when one already exists.</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="Button_Delete_Issue" runat="server" CssClass="Buttons" Text="Delete this Issue" Width="150px"
            OnClick="ButtonDeleteIssue_Click" />
        </td>
        <td align="left" class="T" width="100%">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Delete_IssueKey" runat="server" CssClass="TextBoxInput"
            Width="250px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      </table>
    <!-- Table -->
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="T">
          This is a list of all issues. Click on an issue description to change the description
          or order. The Omit column identifies which issues are currently omitted from pages
          and allows you to change this setting for any issue. Click on an <strong>OMIT</strong>
          link to reinstate the issue. Click on an <strong>ok</strong> link to omit the issue
          on pages.</td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="HSmall">
          Issues</td>
      </tr>
      <tr>
        <td align="left" class="T" style="height: 17px">
          <asp:Label ID="Label_Issues_Report" runat="server"></asp:Label>
          <!-- Table -->
      </tr>
    </table>
    </ContentTemplate>
  </asp:UpdatePanel>
    </div>
</asp:Content>
