<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ElectionCreate.aspx.cs"
  Inherits="Vote.Admin.ElectionCreate" %>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Create an Election</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: bold;
      color: #373737;
      font-size: 11px;
      height: 13px;
      padding-left: 5px;
      padding-right: 0px;
      padding-top: 0px;
      padding-bottom: 0px;
    }
  </style>
</head>
<body>
  <form id="form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <user:Banner ID="Banner" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
    <tr>
      <td valign="middle" align="left" class="HLarge">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td valign="middle" align="left" class="T">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="center" class="H">
        Create an Election
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableCreateGeneralStateElections" border="0" cellpadding="0" cellspacing="0"
    class="tableAdmin" runat="server">
    <tr>
      <td class="HSmall">
        To Create a General Even Year State Election in Each of the 51 States and the State-by-State
        Election Reports for US President, US Senate, US House and Governors (Election Type
        G)
      </td>
    </tr>
    <tr>
      <td class="T">
        State 2 year general elections are created for all 51 states as a single operation
        by the master administrator. The state-by-state reports (U1...U4 StateCodes), that
        are treated as special elections for reporting purposes are also created in this
        single operation.
      </td>
    </tr>
    <tr>
      <td class="T">
        1) Select whether the election should include the US President contest.
        <br />
        2) Enter the November date of general election. Then select the Enter key.<br />
        3) Finally click the &#39;Create Election&#39; button to create an election in all
        51 states.
        <br />
        A row for each state in the Elections Table will be inserted. And all ElectionsOffices
        rows for US President, US Senate, US House and Governors will also be inserted,
        but no statewide or state senate or state house offices. Then 4 Elections (rows
        in the Elections Table) will be inserted for the state-by-state reports, but no
        ElectionsOffices are ever created for these state-by-state reports, because these
        are <strong>not real </strong>seperate elections. These state-by-state elections
        are constructed by extracting specific election contests from each state.
        <br />
        Note: This operation may take about 15 minutes to run.
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        U.S. President Contest in Elections
      </td>
    </tr>
    <tr>
      <td class="Checkboxes">
        <asp:RadioButtonList ID="RadioButtonListIncludePresident" runat="server">
          <asp:ListItem Value="Y">INCLUDE US President Contest in Election</asp:ListItem>
          <asp:ListItem Value="N">EXCLUDE US President Contest in Election</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableCreateElection" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        To Create a New Election
      </td>
    </tr>
    <tr>
      <td class="T">
        Even year general elections held on the first Tuesday in November every 2 years
        are created for each state in one operation by the master administrator. Use this
        form to create all the OTHER TYPES of elections.
        <br />
        Most of the controls below should be loaded with the appropriate selections and
        data.
      </td>
    </tr>
    <tr>
      <td class="T">
        1) Check the election date, type of election, political party and election title.<br />
        2) If any of thse are incorrect, change them. With any changes a new election title
        will be constructed.<br />
        3) For state primaries and presidential prinaries select the party, whether there
        is a US President contest, and an ElectionKey if the same candidates are to be inserted.<br />
        4) Add any optional additional information and/or ballot instructions in the textboxs
        provided.<br />
        5) Finally click the 'Create Election' button to create the election.
        <br />
        <strong>Note::</strong> For party primaries where seperate party ballots are given
        to voters a seperate election needs to be created, even if the primaries are on
        the same day. To do this, return to the Elections.aspx form and click the &#39;Create
        Election&#39; link.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionDate" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        Election Date of New Election (Required and needs to be in the future)
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionDate" runat="server" CssClass="TextBoxInput" Width="172px"
          AutoPostBack="True" OnTextChanged="TextboxElectionDate_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;Enter any date format, like: 11/6/12,&nbsp; 11/06/2012,&nbsp; 2/16/2012,&nbsp;
        November 6, 2012
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionType" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        Type of Election
      </td>
    </tr>
    <tr>
      <td>
        <asp:RadioButtonList ID="RadioButtonListElectionType" runat="server" CssClass="RadioButtons"
          Height="84px" OnSelectedIndexChanged="RadioButtonListElectionType_SelectedIndexChanged1"
          RepeatLayout="Flow" Width="898px" AutoPostBack="True">
          <asp:ListItem Value="O">Off Year Election held in a year between national elections on the first Tuesday in November (Election Type O)</asp:ListItem>
          <asp:ListItem Value="S">Special Election for vacated offices, or county and local elections. Not a primary or November bi-annual election  (Election Type S)</asp:ListItem>
          <asp:ListItem Value="P">State Primary, All Parties, Single Pary or Non-prtisan, w/o US President (Election Type P)</asp:ListItem>
          <asp:ListItem Value="B">State Presidential Primary or Caucus -US President candidates only (Election Type B)</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <asp:RadioButtonList ID="RadioButtonListElectionTypeNational" runat="server" CssClass="RadioButtons"
          Height="42px" OnSelectedIndexChanged="RadioButtonListElectionTypeNational_SelectedIndexChanged1"
          RepeatLayout="Flow" Width="898px" AutoPostBack="True">
          <asp:ListItem Value="US">Presidential Primary Remaining Candidates (Election Type A, StateCode US)</asp:ListItem>
          <asp:ListItem Value="PP">Canonical Presidential Primary Template with Same Candidates (Election Type A, StateCode PP)</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableParty" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        <strong>Political Party</strong>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonListParty" runat="server" CssClass="RadioButtons"
          RepeatDirection="Horizontal" Width="695px" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListParty_SelectedIndexChanged">
          <asp:ListItem Value="A" Selected="True">All Parties</asp:ListItem>
          <asp:ListItem Value="D">Democratic</asp:ListItem>
          <asp:ListItem Value="R">Republican</asp:ListItem>
          <asp:ListItem Value="G">Green</asp:ListItem>
          <asp:ListItem Value="L">Libertarian</asp:ListItem>
<asp:ListItem Value="C">American Constitution</asp:ListItem>
          <asp:ListItem Value="X">Non-Partisan</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionTypeP" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        State Primary (Election Type P)
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Create Same Office Contests in this Party Primary:</strong> After the 
        first pary primary is created, subsequent primaries on the same day for 
        different parties can be created with the same office contests as that primary. 
        If a previous primary is shown below whose offices can be copied, check the checkbox to create the same office contests (without candidates)
        in that primary when this primary is created.
      </td>
    </tr>
    <tr>
      <td class="TColor">
        Uncheck if NO office contests are to be created!<asp:CheckBox ID="CheckBox_CreateOfficeContests"
          runat="server" CssClass="CheckBoxes" Text="Create with Same Office Contests as:  "
          Checked="True" AutoPostBack="True" 
          oncheckedchanged="CheckBox_CreateOfficeContests_CheckedChanged" />
      </td>
    </tr>
    <tr>
      <td class="TBold">
        <asp:Label ID="Label_SameDatePrimary" runat="server"></asp:Label>
        &nbsp; -&nbsp; Office Contests:
        <asp:Label ID="Label_OfficeContests" runat="server"></asp:Label>
      &nbsp; -&nbsp;
        ElectionKey:
        <asp:Label ID="Label_ElectionKey_P" runat="server"></asp:Label>
        &nbsp;</td>
    </tr>
    <tr>
      <td class="T">
        <hr color="Gray" size="1" />
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>US Presidential Primary Contest and Candidates:</strong> When 
        the state primary is created a the presidential primary contest can be 
        included. And if a presidential contest is included, the candidates from a 
        presidential primary template (StateCode = PP) can also be 
        added. Select at radio button to determine whether the state primary will 
        contain the presidential primary contest and with or without the template candidates 
        shown below.
      </td>
    </tr>
    <tr>
      <td class="TBold">
        <asp:Label ID="Label_Candidates_President" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonListUSPresident" runat="server" CssClass="RadioButtons"
          AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListUSPresident_SelectedIndexChanged">
          <asp:ListItem Value="ExcludePresident">EXCLUDE US President Office</asp:ListItem>
          <asp:ListItem Value="IncludePresidentNoCandidates">INCLUDE US President Office WITHOUT any Candidates </asp:ListItem>
          <asp:ListItem Value="IncludePresidentTemplateCandidates">INCLUDE US President Office WITH Presidential Primary Template Candidates shown above</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionTypeB" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        State Presidential Primary (Election Type B)
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Canonical Presidential Primary Template:</strong> When a state presidential
        party primary has the same candidates as another state&#39;s presidential party
        primary Google may treat this as duplicate page content for the Election.aspx and
        Issue.aspx pages. When this is the case, a canonical presidential primary template
        needs to be created first. It will have a canonical url point to domain: vote-usa.org
        instead of the state donmain, vote-xx.org. The existance or not of this canonical
        presidential template is shown below.<br />
      </td>
    </tr>
    <tr>
      <td class="TBold" valign="top">
        <asp:Label ID="Label_ElectionKeyCanonical" runat="server" CssClass="TBold"></asp:Label>
        &nbsp;--<asp:Label ID="Label_Election_Canonical" runat="server" CssClass="TBold"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="style1" valign="top">
        <asp:Label ID="Label_Candidates_Canonical" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <hr color="Gray" size="1" />
      </td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <strong>Add Presidential Candidates &amp; Election Information:</strong> The candidates
        and election information of the existing presidential primary template can be automatically
        added to complete the defintion of this election when it is created. Or the template
        can be used to add just the candidates as a starting list of candidates with more
        candidates to be added later. Or no candidates or information copied. Select a radio
        button below to indicate how the existing presidential candidates should be treated.
      </td>
    </tr>
    <tr>
      <td class="TBold" valign="top">
        <asp:RadioButtonList ID="RadioButtonList_PresidentialCandidates" runat="server" 
          CssClass="RadioButtons" 
          onselectedindexchanged="RadioButtonList_PresidentialCandidates_SelectedIndexChanged">
          <asp:ListItem Value="None">Add NO candidates and NO election information. Treat this election like any other new election.</asp:ListItem>
          <asp:ListItem Value="Candidates">Add the candidates with the intent of adding more later but copy NO election information.</asp:ListItem>
          <asp:ListItem Value="Canonical">Add the candidates as the complete list of candidates and make as a Canonical Primary and copy the template election information.</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionTitle" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        Election Title (Required)
      </td>
    </tr>
    <tr>
      <td class="T">
        This is generated automatically for consistency but can be modified. The title is
        used on Buttons &amp; Reports. A maximum of 90 characters is allowed., which is about
        what fits in the textbox provided.<br />
      </td>
    </tr>
    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionTitle" runat="server" CssClass="TextBoxInput" 
          Width="630px" AutoPostBack="True" 
          ontextchanged="TextboxElectionTitle_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;&nbsp;
        ElectionKey:&nbsp;
        <asp:Label ID="Label_ElectionKey" runat="server"></asp:Label>
        <br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableAdditionalInfo" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="HSmall">
        Additional Information on Ballots and Reports (Optional)
      </td>
    </tr>
    <tr>
      <td class="T">
        This is special information about the election and is optional. Any information
        you provide here will appear on the top of all ballots and reports for this election.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxAdditionalInfo" runat="server" CssClass="TextBoxInputMultiLine"
          Height="40px" TextMode="MultiLine" Width="920px" AutoPostBack="True" 
          ontextchanged="TextboxAdditionalInfo_TextChanged"></user:TextBoxWithNormalizedLineBreaks><br />
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Ballot Instructions (Optional)
      </td>
    </tr>
    <tr>
      <td class="T">
        This is any information which should appear on top of ballots for this election.
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxBallotInstructions" runat="server" CssClass="TextBoxInputMultiLine"
          Height="40px" TextMode="MultiLine" Width="920px" AutoPostBack="True" 
          ontextchanged="TextboxBallotInstructions_TextChanged"></user:TextBoxWithNormalizedLineBreaks><br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableCreateButton" border="0" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td align="left" class="T" valign="top">
        <asp:Button ID="ButtonNewElection" runat="server" CssClass="Buttons" OnClick="ButtonNewElection_Click1"
          Text="Create Election" Width="250px" />Click to create the election defined
        above.
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>