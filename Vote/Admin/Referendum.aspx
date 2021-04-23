<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
Codebehind="Referendum.aspx.cs" Inherits="Vote.Admin.ReferendumPage" %>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Edit Referendum</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</head>
<body>
  <form id="form1" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td align="left" style="height: 18px" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          Use the buttons and links below to add, edit or delete referendums in this election.
          <br />
          At the bottom of this page are buttons to navigate to addition office contests and
          referendums in this election.</td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableAdd" cellspacing="0" cellpadding="0" border="0"
      runat="server">
      <tr>
        <td class="H" colspan="2" align="center">
          Add a Ballot Measure</td>
        <td>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonAdd" runat="server" CssClass="Buttons" Text="Add Ballot Measure"
            OnClick="ButtonAdd_Click1" Width="170px"></asp:Button></td>
        <td class="T">
          Before adding a referendum make certain it is not already in the list of referendums for this
          election below. Then enter the new referendum information in the textboxes
          below. Finally click the <strong>Add Referendum</strong> Button.</td>
        <td>
        </td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableSelect4Update" cellspacing="0" cellpadding="0"
      border="0" runat="server">
      <tr>
        <td class="H">
          Edit Ballot Measures</td>
      </tr>
      <tr>
        <td class="T">
          Click
          the <u>Referendum</u> link you want to edit or tag for deletion in the Report below. Make any desired changes and then click the <strong>Update Referendum</strong>
          Button to update the referendum. Or select the <strong>Don't Show on Ballots</strong>
          Radio Button to delete the referendum on ballots.</td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableUpdate" cellspacing="0" cellpadding="0" border="0"
      runat="server">
      <tr>
        <td class="T">
          <asp:Button ID="ButtonUpdate" runat="server" CssClass="Buttons" Text="Update Ballot Measure"
            OnClick="ButtonUpdate_Click1" Width="170px"></asp:Button>
        </td>
        <td class="T">
          Use the text boxes to make any desired changes, then click this button to implement
          your changes. You may do this any number of times.
        </td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableDelete" cellspacing="0" cellpadding="0" runat="server">
      <tr>
        <td align="left">
          <asp:RadioButtonList ID="RadiobuttonlistDelete" runat="server" CssClass="RadioButtons" AutoPostBack="True" OnSelectedIndexChanged="RadiobuttonlistDelete_SelectedIndexChanged" Width="170px">
            <asp:ListItem Value="No" Selected="True">Show on Ballots</asp:ListItem>
            <asp:ListItem Value="Yes">Don't Show on Ballots</asp:ListItem>
          </asp:RadioButtonList>&nbsp;</td>
        <td align="left" class="T" valign="top">
          <strong>Show or Don't Show on Ballots: </strong>The top radio button must be selected
          to show this referendum on ballots. When the bottom radio button is selected the
          referendum is tagged for deletion and will not appear on ballots. Later you may reinstate this referendum by selecting the Show on Ballots Radio Button.
        </td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableAddNew" cellspacing="0" cellpadding="0" border="0"
      runat="server">
      <tr>
        <td valign="middle" align="left" class="T">
          <asp:Button ID="ButtonNext" runat="server" CssClass="Buttons" Text="Clear Texboxes"
            OnClick="ButtonNext_Click" Width="170px"></asp:Button>
          </td>
        <td valign="middle" class="T" width="100%">
          Click this button to clear the textboxes if you want to add another referendum.</td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableReferendum" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="center">
          <asp:Label ID="ReferendumHTMLTable" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="center">
          &nbsp;</td>
      </tr>
    </table>
      <!-- Table -->
    <table class="tableAdmin" id="TablePassFail" cellspacing="0" cellpadding="0" border="0" runat=server>
      <tr>
        <td align="center" class="H" colspan="2">
          Ballot Meaure 
          Results (Passed or Failed)</td>
      </tr>
      <tr>
        <td class="T" colspan="2">
          To record the result of a ballot measures:<br />
          Select a ballot measure in the table above to load the the ballot measure in the 
          textboxes below.<br />
          Then select either the Passed or Failed radio button below to record its passage 
          or failure.<br />
          Repeat for all the ballot measures in the table.</td>
      </tr>
      <tr>
        <td class="T">
          <asp:RadioButtonList ID="RadioButtonListPassFail" runat="server" AutoPostBack="True" 
            CssClass="CheckBoxes" Width="150px" 
            onselectedindexchanged="RadioButtonListPassFail_SelectedIndexChanged">
            <asp:ListItem>Passed</asp:ListItem>
            <asp:ListItem>Failed</asp:ListItem>
          </asp:RadioButtonList>
        </td>
        <td class="T" width="100%">
          Select either Passed or Failed to indicate whether the ballot measure passed or 
          failed. Not Recorded only indicates whether the success of the ballot has been 
          recorded. </td>
      </tr>
      </table>
      <!-- Table -->
    <table class="tableAdmin" id="TableTextBoxes" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="center" class="H" colspan="2">
          Ballot Measure 
        </td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Title (required)
          Try to keep within the textbox provided because it is part of the ballot measure 
          key.</td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonReCase" runat="server" CssClass="Buttons" OnClick="ButtonReCase_Click"
            Text="ReCase" Width="100px" /><br />
          <asp:Button ID="Button1" runat="server" CssClass="Buttons" OnClick="Button1_Click"
            Text="Remove CrLf" Width="100px" /></td>
        <td align="left" valign="middle" class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="ReferendumTitle" runat="server" TextMode="MultiLine" 
            Width="800px" Height="20px" CssClass="TextBoxInput" 
            OnTextChanged="ReferendumTitle_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" colspan=2 class="T">
          <strong>Note:</strong>
          The <strong>Remove CrLf </strong>(carriage control, line feed) Buttons are useful when you copy and paste text
          and the text contains carriage returns and line feeds that break the normal flow
          of the text.</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2" style="height: 13px">
          The <strong>ReCase</strong> Buttons are useful when you copy and paste text that
          is all upper case.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Ballot Order</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="OrderOnBallot" runat="server" Width="50px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          </td>
        <td align="left" class="T">
          If you leave the Ballot Order textbox empty when adding ballot measures, the 
          referendum will be added as the last referendum on ballots by assigning a ballot 
          order of 10 greater than the last referendum. Enter a Ballot Order if the 
          referendum should not be added last or use to change the ballot order when 
          updating this referendum.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" valign="top" colspan="2">
          Description on Ballots (required)</td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonDescRemoveCrLf" runat="server" CssClass="Buttons" Height="35px"
            OnClick="ButtonDescRemoveCrLf_Click" Text="Remove CrLf" Width="100px" /><br />
          <asp:Button ID="ButtonRecaseDescription" runat="server" CssClass="Buttons" OnClick="ButtonRecaseDescription_Click"
            Text="ReCase" Width="100px" /></td>
        <td align="left" valign="top" class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="ReferendumDesc" runat="server" TextMode="MultiLine" Height="300px"
            Width="800px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" valign="top" class="HSmall" colspan="2">
          Urls can be w/wo http://</td>
      </tr>
      <tr>
        <td align="left" class="TBold" valign="top">
          Detail Url:</td>
        <td align="left" class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDetailLink" runat="server" Width="800px" 
            CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="TBold" valign="top" style="height: 21px">
          Full Text Url:</td>
        <td align="left" class="T" style="height: 21px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFullTextLink" runat="server" Width="800px" 
            CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="HSmall" valign="top" style="height: 13px" colspan="2">
          Detail on Link Page</td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonDetailRemoveCrLf" runat="server" CssClass="Buttons" Height="30px"
            OnClick="ButtonDetailRemoveCrLf_Click" Text="Remove CrLf" Width="95px" /><br />
          <asp:Button ID="ButtonReCaseFullText" runat="server" CssClass="Buttons" OnClick="ButtonReCaseFullText_Click"
            Text="ReCase" Width="94px" /></td>
        <td align="left" valign="top" class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="ReferendumDetail" runat="server" TextMode="MultiLine" Height="300px"
            Width="800px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Full Text on Link Page</td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonFullRemoveCrLf" runat="server" CssClass="Buttons" Height="31px"
            OnClick="ButtonFullRemoveCrLf_Click" Text="Remove CrLf" Width="94px" /></td>
        <td align="left" class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="ReferendumFullText" runat="server" TextMode="MultiLine" Height="400px"
            Width="800px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>