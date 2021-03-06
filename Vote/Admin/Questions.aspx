<%@ Page ValidateRequest="false" Language="c#" Codebehind="Questions.aspx.cs" AutoEventWireup="true"
  MasterPageFile="~/Master/Admin.Master"
 Inherits="Vote.Admin.QuestionsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
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
    <table class="tableAdmin" id="TableUpdate" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="H" colspan="2">
          Edit a Question</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Button ID="ButtonUpdate" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Update" OnClick="ButtonUpdate_Click1" Width="113px"></asp:Button></td>
        <td align="left" class="T">
          Click a <b>Topic Question</b> whose wording
          or order you want to change in the report below. Then enter any changes in the textboxes
          provided. Finally click <b>Update</b> to implement your changes.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableAdd" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="center" colspan="2" class="H">
          Add a Question</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Button ID="ButtonClear" runat="server" CssClass="Buttons" OnClick="ButtonClear_Click"
            Text="Clear" Width="109px" /><br />
          <asp:Button ID="ButtonAdd" runat="server" CssClass="Buttons" Font-Bold="True" Text="Add"
            OnClick="ButtonAdd_Click1" Width="108px"></asp:Button></td>
        <td align="left" class="T">
          Adding a Topic Question is done in several steps.
          First click the <b>Clear</b> Button if you need to clear the Order and Question
          textboxes. Then make certain the new issue is not already in the list of question
          show in the list below. Then enter the Topic Question. If the Order is omitted the
          question will be added to the bottom of the list. Finally, click <b>Add</b>.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableUpdate2" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="T">
          &nbsp; Order:
          <user:TextBoxWithNormalizedLineBreaks ID="QuestionOrder" runat="server" Width="93px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          Question*:<user:TextBoxWithNormalizedLineBreaks ID="Question" runat="server" Width="425px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="center" class="T">
          <table id="Table2" cellspacing="0" cellpadding="0" border="0">
            <tr>
              <td align="right" class="H" colspan="3">
                Change Question Status</td>
            </tr>
            <tr>
              <td align="right" class="T">
                Omit In Next Election:<br />
                <asp:RadioButtonList ID="RadioButtonListOmit" runat="server" CssClass="RadioButtons"
                  RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListOmit_SelectedIndexChanged">
                  <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                  <asp:ListItem Value="Yes">Yes</asp:ListItem>
                </asp:RadioButtonList></td>
              <td align="right" class="T">
                Tag for Deletion:<br />
                <asp:RadioButtonList ID="RadiobuttonlistDelete" runat="server" CssClass="RadioButtons"
                  RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadiobuttonlistDelete_SelectedIndexChanged">
                  <asp:ListItem Value="No" Selected="True">No</asp:ListItem>
                  <asp:ListItem Value="Yes">Yes</asp:ListItem>
                </asp:RadioButtonList></td>
              <td class="T">
                Select the Topic Question whose status
                you want to change from the list below. Then set the status by clicking one of the
                radio buttons.</td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="H" colspan="2">
          Renumber Questions</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Button ID="ButtonRenumber" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Renumber Questions" OnClick="ButtonRenumber_Click1" Width="179px"></asp:Button></td>
        <td align="left" class="T">
          &nbsp;Click this button to renumber all Questions by increments of 10 and retain
          the order.</td>
      </tr>
      <tr>
        <td colspan="2" class="H">
          Delete Tagged Questions</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Button ID="ButtonDeleteTaggedQuestions" runat="server" CssClass="Buttons" OnClick="ButtonDeleteTaggedQuestions_Click1"
            Text="Delete Tagged Questions" Width="185px" /></td>
        <td align="left" class="T">
          Click this button to permanently delete all Topic Questions with a <b>D</b> in the
          first column. If there are answers, as shown in parenthesis, then you will need
          to move or delete the answers using the button below.</td>
      </tr>
      <tr>
        <td align="center" colspan="2" class="H">
          Move Question Answers</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Button ID="ButtonMoveQuestion" runat="server" CssClass="Buttons" Font-Bold="True"
            Text="Move Questions Answers" OnClick="ButtonMoveQuestion_Click1" Width="187px">
          </asp:Button></td>
        <td align="left" class="T">
          This button will provide a report of all the Issues and Questions associated with
          each Issue. On this form you will be able to: (1) move questions from one issue
          to another, (2) move question answers from one question to another, and (3) delete
          questions that have no associated answers. NOTE: It will take several seconds for
          this form to be presented.</td>
      </tr>
      <tr>
        <td colspan="2" class="H">
          Report of Issues and Questions</td>
      </tr>
      <tr>
        <td align="left">
          <asp:HyperLink ID="HyperLinkReport" runat="server" CssClass="HyperLink" Target="view"
            >Issues & Question Report</asp:HyperLink></td>
        <td align="left" class="T">
          Click this button to obtain a report of all Issues, Questions and Answers.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td class="T">
          O = Topic Question data will be omitted on all pages and can be reinstated anytime;
          D = Topic Question is tagged for deletion and will be physically deleted when the
          button above is clicked.</td>
      </tr>
    </table>
    <!-- Table -->
    <asp:Label ID="LabelQuestionsTable" runat="server"></asp:Label>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
