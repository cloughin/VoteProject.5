<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Office.aspx.cs" Inherits="Vote.Admin.OfficePage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Ballot.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: normal;
      color: #373737;
      font-size: 11px;
      width: 543px;
      height: 26px;
      padding-left: 5px;
      padding-right: 0;
      padding-top: 0;
      padding-bottom: 0;
    }
    .style2
    {
      height: 26px;
    }
    body.admin-page h1
    {
      line-height: 110%;
      margin: 8px 0 8px 0;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    
  <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
    <%-- 
    <tr>
      <td align="left" class="HSmall">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
    --%>
    <tr>
      <td class="T">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="right" class="TSmall">
        <asp:Label ID="OfficeID" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!---TableOfficePositions Add Back Later if Needed -->
  <table class="tableAdmin" id="Table_Add_Office_Data" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="3">
        Add Office(s)
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3">
        Follow these 5 steps to add an office:<br />
        <strong>Step 1:</strong> Make certain the office you are about to add is NOT in
        the report at the bottom of this page.
        <br />
        <strong>Step 2:</strong> Then enter 1st and 2nd lines of the office title in the
        two textboxes exactly as they should appear on ballots. <strong>DO NOT</strong>
        include the County or Local District Name as part of the office title.
        <br />
        <strong>Step 3:</strong> After entering each office titile line, click anywhere
        outside the text box to re-case the textbox content. This is to force proper casing
        when copying and pasting office titles that are all in upper case.
        <br />
        <strong>Step 4:</strong> Enter the order on ballots for offices in the same category.<br />
        <strong>Step 5:</strong> Click the Add Office Button.<br />
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="3">
        Office Title on Ballots
      </td>
    </tr>
    <tr>
      <td valign="top">
        <table>
          <tr>
            <td class="T">
              After each line of the office title press the Enter key so that the office title 
              will be re-cased. Any &#39;of&#39; in the title will be be lower cased. Or click the 
              Recase Button.</td>
          </tr>
          <tr>
            <td class="TBold">
              1st Line of Office Title (required):
            </td>
          </tr>
          <tr>
            <td>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Line_Add_1" runat="server" CssClass="TextBoxInput"
                Width="350px" ontextchanged="TextBox_Office_Line_Add_1_TextChanged1"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="TBold">
              2nd Line of Office Title (optional):&nbsp; 
            </td>
          </tr>
          <tr>
            <td>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Line_Add_2" runat="server" CssClass="TextBoxInput"
                Width="350px"></user:TextBoxWithNormalizedLineBreaks>
            &nbsp;
            </td>
          </tr>
          <tr>
            <td class="TBold">
              District Number:
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDistrict" runat="server" CssClass="TextBoxInput" 
                Width="30px"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td>
              <asp:Button ID="Button_Recase1" runat="server" CssClass="Buttons" Height="22px" 
                onclick="Button_Recase1_Click" Text="Recase Both Lines" Width="130px" />
            </td>
          </tr>
          <tr id="trAdditionalLinesAdd" runat="server">
            <td class="T">
              <strong>Additional Title Lines:  </strong>Only for county and local offices these
              line(s) will be automatically added to the office title.
            </td>
          </tr>
          <tr id="trLocalDistrictNameAdd" runat="server">
            <td>
              <asp:Label ID="LabelLocalDistrictNameAdd" runat="server" CssClass="TextBoxInput"></asp:Label>
            </td>
          </tr>
          <tr id="trCountyNameAdd" runat="server">
            <td>
              <asp:Label ID="LabelCountyNameAdd" runat="server" CssClass="TextBoxInput"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="TBold">
              Order on Ballots (optional):
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Order" runat="server" CssClass="TextBoxInput" Width="40px"
                AutoPostBack="True" OnTextChanged="TextBox_Order_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
        </table>
      </td>
      <td class="T" valign="top">
        <strong>Office Title:e:</strong> Enter the office title exactlly as it should appear
        on ballots, <strong>WITHOUT</strong> <strong>the state, county or local district name
          in either line of the office title. </strong>&nbsp;The 1st &amp; 2nd Lines of the 
        Office Title should be the 1st &amp; 2nd lines of the office as it should appear on 
        ballots. Only the 1st line of the title is required. The state, county or local 
        distrit names are automatically added on ballots and reports.
        <br />
        <br />
        <strong>District Number:</strong> Enter the legislative district number <strong>
        ONLY</strong> for US House, State Senate and State House Offices.<br />
        <br />
        <br />
        <strong>Order on Ballots::</strong> Enter a whole number in multiples of 10 then
        press the <strong>Enter</strong> key. If left empty, the default is 10.<br />
        <br />
        <br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="Table_Add_Office" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td class="HSmall" colspan="2" valign="top">
        Add the Office
      </td>
    </tr>
    <tr>
      <td colspan="1" class="T">
        <asp:Button ID="Button_Add_Office" runat="server" CssClass="Buttons" OnClick="Button_Add_Office_Click"
          Text="Add Office" Width="200px" />
      </td>
      <td class="TBoldColor" colspan="1" width="100%">
        Be sure to click this button to implement your office addition.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Add_Office_And_In_Election" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="HSmall" colspan="2">
        Add the Office and Add as Office Contest in this Election
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="Button_Add_Office_And_In_Election" runat="server" Text="Add Office"
          OnClick="Button_Add_Office_And_In_Election_Click1" CssClass="Buttons" Width="200px">
        </asp:Button>
      </td>
      <td class="TBoldColor" width="100%">
        Be sure to click this button to implement your office addition.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Another_Office" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="HSmall" valign="middle">
        <asp:HyperLink ID="HyperLinkAddAnotherOffice" runat="server" CssClass="HyperLink"
          Target="office">Add Another Office</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" style="height: 24px" valign="middle">
        Use the link above to continue adding offices in this office category.
      </td>
    </tr>
  </table>


  <!-- Table -->
  <table class="tableAdmin" id="Table_Incumbent" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td class="H" colspan="2" align="center">
        Current Office Incumbent(s)
       </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        &nbsp;<asp:Label ID="LabelIncumbentMsg" runat="server" CssClass="TLargeColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" colspan="2" valign="top" class="T">
        <%-- 
        <asp:Label ID="IncumbentTable" runat="server"></asp:Label>
        --%>
        <asp:PlaceHolder ID="IncumbentsReportPlaceHolder" runat="server"></asp:PlaceHolder>
      </td>
    </tr>
    <tr>
      <td colspan="2" valign="middle">
        <asp:HyperLink ID="HyperLink_EditIncumbents" runat="server" CssClass="HyperLink" Target="EditIncumbents">Edit Incumbents</asp:HyperLink>
        <br />
        &nbsp;
      </td>
    </tr>
    <%--
    <tr id="Tr4" runat="server">
      <td class="T" colspan="2" valign="middle">
        To remove an incorrect incumbent or identify a correct incumbent, copy and paste the
        politician ID in the textbox provided. Then click anywhere outside the textbox.
      </td>
    </tr>
    <tr id="trRemoveIncumbent" runat="server">
      <td valign="middle" class="T">
        <strong>Remove Incumbent with ID:</strong> Copy and paste a politician ID above
        in this textbox and select Enter.
      </td>
      <td valign="middle">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Remove_Incumbent" runat="server" Width="250px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxRemoveIncumbent_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td valign="middle" align="left" class="style1">
        <strong>Identify Incumbent:</strong> Copy and paste the Politician ID in this textbox
        and select Enter. This is done to identify for either an election winner or the
        current incumbent.
      </td>
      <td valign="middle" class="style2">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxSelectIncumbent" runat="server" Width="250px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxSelectIncumbent_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        You can use this link&nbsp; to find a politician&#39;s ID.
      </td>
      <td valign="middle">
        <asp:HyperLink ID="HyperLink_Find_PoliticianID_Incumbent" runat="server" CssClass="HyperLink"
          Target="_politicians">Find Politician ID</asp:HyperLink>
        <br />
        &nbsp;
      </td>
    </tr>
    --%>
  </table>
<%--   <table class="tableAdmin" id="Table_Running_Mate" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" colspan="3" align="center">
        Elected Running Mate for this Office</td>
    </tr>
    <tr>
      <td class="T" colspan="3">
        <strong>Identify Elected Running Mate: </strong>To identify the elected running mate for this office, 
        like Vice President or Lt. Governor,
        the current office incumbent needs to be identified in the first section of this 
        form. If the existing running mate is incorrect you need to first remove the 
        incorrect running mate using the button provided.&nbsp; To identify the correct 
        running mate enter or paste the running mate&#39;s Id in the textbox and select Enter.</td>
    </tr>
    <tr>
      <td valign="middle" class="TBold">
        Running Mate ID:
      </td>
      <td width="260px">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_RunningMate" runat="server" Width="250px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxRunningMate_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td width="300">
        <asp:Label ID="Label_RunningMate" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td valign="middle" class="TBold">
        <asp:Button ID="Button_Remove_RunningMate" runat="server" CssClass="Buttons" OnClick="Button_Remove_RunningMate_Click"
          Text="Remove Running Mate" />
      </td>
      <td class="T" colspan="2">
        Use this button to simply remove an incorrect running mate, leaving the main 
        office holder as is.</td>
    </tr>
  </table>
--%> <!-- Table -->
  <!-- Table -->
  <table id="Table_Edit_Office" class="tableAdmin" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td colspan="2" class="H">
        Edit
        <asp:Label ID="Label_Edit_Name" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2" style="height: 13px">
        Use this section to add, change or delete what will appear on ballots for this office.&nbsp;
      </td>
    </tr>
    <tr>
      <td valign="top" class="TBold">
        <table class="tableBallotOfficeContest" cellspacing="0">
          <tr>
            <td class="HSmall" colspan="2">
              General Vote Instructions for this Office (optional):
            </td>
          </tr>
          <tr>
            <td class="BallotOfficesVoteFor" colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Vote_Instructions" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxVoteInstructions_TextChanged" TextMode="MultiLine"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="HSmall" colspan="2">
              Office Title on Ballots
            </td>
          </tr>
          <tr>
            <td class="T" colspan="2">
              After each line of the office title press the Enter key so that the office title 
              will be re-cased. Any &#39;of&#39; in the title will be be lower cased. Or click the 
              Recase Button.</td>
          </tr>
          <tr>
            <td class="TBold" colspan="2">
              1st Line of Office Title (required):
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Line_Edit_1" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxOfficeLine1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="TBold" colspan="2">
              2nd Line of Office Title (optional):
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Line_Edit_2" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxOfficeLine2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <asp:Button ID="Button_Recase2" runat="server" CssClass="Buttons" Height="22px" 
                onclick="Button_Recase2_Click" Text="Recase Both Lines" />
            </td>
          </tr>
          <tr id="trAdditionalLines" runat="server">
            <td class="T" colspan="2">
              <strong>Additional Title Lines: </strong>Only for county and local offices these
              line(s) will be automatically added to the office title.
            </td>
          </tr>
          <tr id="trLocalDistrictName" runat="server">
            <td colspan="2">
              <asp:Label ID="LabelLocalDistrictName" runat="server" CssClass="TextBoxInput"></asp:Label>
            </td>
          </tr>
          <tr id="trCountyName" runat="server">
            <td colspan="2">
              <asp:Label ID="LabelCountyName" runat="server" CssClass="TextBoxInput"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="HSmall" colspan="2">
              Vote for Wording, i.e. Vote for no more than one (required):
            </td>
          </tr>
          <tr>
            <td class="BallotOfficesVoteFor" colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Vote_Wording" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxVoteForWording_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              Candidate Name - Party Code
            </td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl75" type="checkbox" />
            </td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1" style="height: 21px">
              Candidate Name - Party Code
            </td>
            <td class="tdBallotCheckBox" align="right" colspan="1" style="height: 21px">
              <input name="_ctl75" type="checkbox" />
            </td>
            <tr>
              <td class="HSmall" colspan="2">
                Write in Instructions (optional):
              </td>
            </tr>
          <tr>
            <td class="BallotOfficesVoteFor" colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_WriteIn_Instructions" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxWriteInInstructions_TextChanged" TextMode="MultiLine"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="HSmall" colspan="2">
              Write in Lines Wording, i.e. Write In (required if Write in Lines not 0):
            </td>
          </tr>
          <tr>
            <td class="TBold" colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox_WriteIn_Wording" runat="server" Width="350px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxWriteInWording_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="HSmall" colspan="2">
              Write In Lines:
            </td>
          </tr>
          <tr>
            <td class="TBold" colspan="2">
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxWriteInLines" runat="server" Width="38px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxWriteInLines_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
              (0 to 5, with 0 to omit write in lines)
            </td>
          </tr>
        </table>
      </td>
      <td valign="top">
        <table>
          <tr>
            <td class="HSmall" width="100%">
              Ballot Content Instructions
            </td>
          </tr>
          <tr>
            <td class="T" width="100%" valign="top">
              If a textbox is empty that content item will be excluded. The content of each textbox
              is recorded after you enter content and click the Enter Key or anywhere else outside
              the textbox.<br />
              <strong>Office Title:</strong> Enter the office title exactlly as it should appear
              on ballots, including casing. Only the 1st line of the title is required. If the
              office is a county or local district office do not incluce county and local district
              names in either line of the office title. They will be automatically added on ballots
              and reports.<br />
            </td>
          </tr>
          <tr class="T">
            <td class="T" width="100%">
              <strong>Ballot Office Contest Order (optional):</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeOrderOnBallot" runat="server" Width="42px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxOfficeOrderOnBallot_TextChanged"></user:TextBoxWithNormalizedLineBreaks><br />
              This number is only used to order the offices in this office category on ballots.
              Ballot order numbers are shown as the first number in the right hand column in the
              report below.
            </td>
          </tr>
          <tr>
            <td class="HSmall">
              Other Office Information
            </td>
          </tr>
          <tr>
            <td class="T">
              The two additional pieces of information below are needed for reports and formatting.
            </td>
          </tr>
          <tr>
            <td class="T">
              <strong>Office Positions:</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficePositions" runat="server" Width="24px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxOfficePositions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            &nbsp;</td>
          </tr>
          <tr>
            <td class="T">
              This is the number of elected officials (incumbents) at any time for this 
              office. Most offices are 1, some like US Senate have 2, and commissions can have 
              more than 2.</td>
          </tr>
          <tr>
            <td class="T">
              <strong>Positions Up for Election:</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxElectionPositions" runat="server" Width="24px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxElectionPositions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="T">
              This is the number of office positions that is/are up for election in elections. 
              Most offices have 1 but commissions can have more than 1.</td>
          </tr>
          <tr>
            <td class="T">
              <strong>Primary Positions Up for Election:</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPrimaryPositions" runat="server" Width="24px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxPrimaryPositions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="T">
              This is the number of positions that are up for election in primaries. 
              Most offices have 1.</td>
          </tr>
          <tr>
            <td class="T">
              <strong>Primary Runoff Positions:</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPrimaryRunoffPositions" runat="server" Width="24px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxPrimaryRunoffPositions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="T">
              This is the number of candidates that advance to a primary runoff election if a runoff is necessary. Enter 0 if runoffs are never
              used in primaries for this office. Enter 2 or more to indicate that a fixed number of candidates advance to the potential primary runnoff. 
              Enter -1 if the number of candidates that can advance is variable.</td>
          </tr>
          <tr>
            <td class="T">
              <strong>General Runoff Positions:</strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxGeneralRunoffPositions" runat="server" Width="24px" CssClass="TextBoxInput"
                AutoPostBack="True" OnTextChanged="TextBoxGeneralRunoffPositions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
            </td>
          </tr>
          <tr>
            <td class="T">
              This is the number of candidates that advance to a general runoff election if a runoff is necessary. Enter 0 if runoffs are never
              used in general elections for this office. Enter 2 or more to indicate that a fixed number of candidates advance to the potential general election runnoff. 
              Enter -1 if the number of candidates that can advance is variable.</td>
          </tr>
          <tr>
            <td class="TBold">
              Is Running Mate Type Office:<asp:RadioButtonList ID="RadioButtonListRunningMateOffice"
                runat="server" CssClass="RadioButtons" RepeatDirection="Horizontal" AutoPostBack="True"
                OnSelectedIndexChanged="RadioButtonListRunningMateOffice_SelectedIndexChanged">
                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                <asp:ListItem Value="No">No</asp:ListItem>
              </asp:RadioButtonList>
            </td>
          </tr>
          <tr>
            <td class="T">
              Select Yes when two candidates run together (i.e on a ticket), like Governor and
              Lt Governor in general elections.
            </td>
          </tr>
          <tr>
            <td class="TBold">
              Is Office Only for Primary Elections:
            </td>
          </tr>
          <tr>
            <td class="TBold">
              <asp:RadioButtonList ID="RadioButtonListIsOnlyForPrimaries" runat="server" CssClass="RadioButtons"
                RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListIsOnlyForPrimaries_SelectedIndexChanged">
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem Selected="True">No</asp:ListItem>
              </asp:RadioButtonList>
            </td>
          </tr>
          <tr>
            <td class="T">
              Select Yes if the offices in not really a seperate office and is only used when
              running mate type offices are seperate contests in primaries.
            </td>
          </tr>
          <tr>
            <td class="HSmall">
              Office Status:
            </td>
          </tr>
          <tr>
            <td class="T">
              <asp:RadioButtonList ID="RadioButtonListActive" runat="server" CssClass="RadioButtons"
                RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListActive_SelectedIndexChanged">
                <asp:ListItem Value="No">Active</asp:ListItem>
                <asp:ListItem Value="Yes">Inactive</asp:ListItem>
              </asp:RadioButtonList>
            </td>
          </tr>
          <tr>
            <td class="T">
              Set the status to Inactive for offices that should not appear on reports and ballots
              because the legislative demographic can not be defined, like State Education Boards
              based on counties or congressional districts.
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <!---TableAdd -->
  <table class="tableAdmin" id="Table_Add_New_Politician_Without_Election" cellspacing="0"
    cellpadding="0" border="0" runat="server">
    <tr>
      <td align="left" class="H" valign="middle">
        Add a Politician for
        <asp:Label ID="Label_Add_Politician" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" valign="middle">
        <asp:HyperLink ID="HyperLink_Add_Politician" runat="server" CssClass="HyperLink"
          Target="politician">Add a Politician as Future Candidate or Incumbent for this Office</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        Use the link above to add a new politician as either a candidate for this office
        in a forthcoming election or identify a politician as the incumbnet for this office.
      </td>
    </tr>
  </table>
  <!-- Table -->

  <!-- Table -->
  <table class="tableAdmin" id="TableMasterOnly" cellspacing="0" cellpadding="0" border="1"
    runat="server">
    <tr>
      <td class="H" align="center" colspan="2">
        Master Users Only
      </td>
    </tr>
    <tr>
      <td class="HSmall" colspan="2">
        Additions and Deletions of County and Local Offices Either One-by-one or in Bulk
      </td>
    </tr>
    <tr>
      <td class="TBold" colspan="2">
        Office Class</td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        Select the class of offices for the addition, deletion and update functions 
        below.</td>
    </tr>
    <tr>
      <td class="T" colspan="2" width="460">
        <asp:RadioButtonList ID="RadioButtonList_Office_Classes" runat="server" 
          AutoPostBack="True" CssClass="RadioButtons" 
          onselectedindexchanged="RadioButtonList_Office_Classes_SelectedIndexChanged" 
          RepeatColumns="2" Width="920px">
          <asp:ListItem Value="8">County EXECUTIVE Offices like [Executive, Mayor, Sheriff, Treasurer, Clerk] </asp:ListItem>
          <asp:ListItem Value="9">County LEGISLATIVE Offices like [Councils, Board of Supervisors] </asp:ListItem>
          <asp:ListItem Value="10">County SCHOOL BOARD Offices </asp:ListItem>
          <asp:ListItem Value="11">County COMMISSION Offices </asp:ListItem>
          <asp:ListItem Value="18">County JUDICIAL Offices </asp:ListItem>
          <asp:ListItem Value="22">County PARTY Offices </asp:ListItem>
          <asp:ListItem Value="12">Local EXECUTIVE Offices [Executive, Mayor, Sheriff, Treasurer, Clerk] </asp:ListItem>
          <asp:ListItem Value="13">Local LEGISLATIVE Offices [Councils, Board of Supervisors]</asp:ListItem>
          <asp:ListItem Value="14">Local SCHOOL BOARD Offices</asp:ListItem>
          <asp:ListItem Value="15">Local COMMISSION Offices</asp:ListItem>
          <asp:ListItem Value="19">Local JUDICIAL Offices</asp:ListItem>
          <asp:ListItem Value="23">Local PARTY Offices</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <strong>Bulk Additions and Updates:</strong> </td>
    </tr>
    <tr>
      <td class="TBold" width="250px">
        Part of Office Titles to Search:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Title_Search" runat="server" CssClass="TextBoxInput"
          Width="300px" AutoPostBack="True" 
          OnTextChanged="TextBox_Office_Title_Search_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="T">
        <strong>Report of Existing Offices in Counties and Local Districts:</strong> The two bulk operations 
        below are dependent on the part of the office title entered in this textbox. That part of the office title will be considered as being the office, 
        since office titles may vary for esentially the same office. So before either 
        bulk operation: 
        <br />
        1) Select the office class above<br />
        2) Enter the part of the office title to search<br />
        3) Click the Run Report button below<br />
        The report will show which counties or local 
        districts have and don&#39;t have offices for the office title search part.</td>
    </tr>
    <tr>
      <td class="TBold">
        <asp:Button ID="Button_Add_Bulk_Offices" runat="server" Text="Bulk Addition of Offices"
          OnClick="Button_Add_Bulk_Offices_Click1" CssClass="Buttons" Width="200px"></asp:Button>
          <br />
        <asp:CheckBox ID="CheckBox_SkipCheck" runat="server" 
          Text="Skip Part of Office Titles to Search" />
      </td>
      <td class="T">
        <strong>Bulk Addition of Offices for ALL Counties or Local Districts: </strong>
        This 
        operation will add an office in each county of local district, for those counties or
        local districts <strong>where the office does not exists</strong>. 
        <br />
        1) Enter one or two lines of the office
        title in the textboxes in the section above as they should appear on ballots and 
        press Enter after each.<br />
        2) Enter the order on ballots and press Enter.<br />
        --- Do either of the following ---<br />
        3) Enter the part of the office title to search and press Enter. A report of all 
        with and without this part will be presentetd. All offices with this search part 
        WILL NOT have an office created when the Bulk Addition is clicked.<br />
        3) Check the checkbox to skip the the part of the office title to search. This 
        is necessary when there is no unique office title part to differentiate two or 
        more offices, i.e. Commissioner Precinct 1 and Commissioner Precinct 2.<br />
        4) Click the Bulk Addition
        of Offices button on the left. 
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="Button_Update_Bulk_Offices" runat="server" CssClass="Buttons" 
          onclick="Button_Update_Bulk_Offices_Click" Text="Bulk Update of Offices" 
          Width="200px" />
      </td>
      <td class="T">
        <strong>Bulk Update of Offfices for ALL Counties or Local Districts:</strong>
        <strong>Bulk Update of Offices:</strong>
        This operation will 
        update office titles and/or ballot order (depending which or both that are 
        provided) <strong>for every offices that exist</strong>, per the office title search part: 
        <br />
        1) Run the Report of Existing Offices in Counties and Local Districts as 
        described above. The offices in the report will be the offices that 
        will updated.
        <br />
        2) Enter the two office title lines and/or ballot order that will be used to 
        update the office title and ballot order in the textboxes above.
        <br />
        3) Click the button on the 
        left.</td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="Button_Run_Report" runat="server" CssClass="Buttons" OnClick="Button_Run_Report_Click"
          Text="Run Report on Search Part" Width="200px" />
      </td>
      <td class="T">
        <strong>Run Report:</strong> This operation will provide a report showing which counties or local 
        districts have and don&#39;t the office title part entered in the textbox above:<br />
        1) If an Office Class has not been selected, select it above<br />
        2) Enter the part of the office title to search<br />
        3) Click this Run Report button<br />
        The report generted will show which counties or local 
        districts have and don&#39;t have offices for the office title search part. The 
        purpose of the report is to insure
        that all the county or local office(s) have been added, deleted or edited as you
        intended.
        <br />
        The first and second lines of the title are separated with &quot;|&quot;. 
        An underscore _ indicates no second line.</td>
    </tr>
    <tr>
      <td class="T">
        <asp:CheckBox ID="CheckBox_Supress_Report" runat="server" CssClass="CheckBoxes" 
          Text="Supress Report" />
      </td>
      <td class="T">
        <strong>Supress Report Construction After Each Add or Delete:</strong> Check this box to supress 
        the reconstruction of the report after each <strong>singlee</strong> add or delete. This can save 
        time, especiall when adding or deleteing local offices.</td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <strong>Single Additions, Deletions and Updates: </strong>
      </td>
    </tr>
    <tr>
      <td class="TBold" width="250px" valign="top">
        CountyCode:<user:TextBoxWithNormalizedLineBreaks ID="TextBox_CountyCode" 
          runat="server" CssClass="TextBoxInput"
          Width="35px"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;LocalCode:<user:TextBoxWithNormalizedLineBreaks ID="TextBox_LocalCode" 
        runat="server" CssClass="TextBoxInput"
          Width="25px"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <asp:Button ID="Button_Add_One_Office" runat="server" Text="Add Single Office" 
        OnClick="Button_Add_One_Office_Click1" CssClass="Buttons"
          Width="200px"></asp:Button>
      </td>
      <td class="T">
        <strong>Single Additions of County or Local District Offices:</strong> If you
        want to add offices one at a time, enter the part of the office title to search
        in the textbox above and run a report. Then enter the two office title lines and
        ballot order in the textboxes in the above section. Then enter the 3 digit CountyCode
        and 2 digit LocalCode (if adding local districts) in textboxes on the left. These
        can be copied from the report. Then click the Add Office Button.
      </td>
    </tr>
    <tr>
      <td class="TBold" colspan="2">
        OfficeKey:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_OfficeKey" runat="server" CssClass="TextBoxInput" Width="700px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="TBold">
        <asp:Button ID="Button_Delete_Office" runat="server" CssClass="Buttons" OnClick="Button_Delete_Office_Click"
          Text="Delete Single Office" Width="200px" />
      </td>
      <td class="T">
        <strong>Delete Specific County or Local District Offices:</strong> If you want to
        delete specific offices one at a time, enter the OfficeKey in textboxes provided, 
        then click the Delete Office Button. Copy the report if many offices need to be 
        deleted.<br />
        <strong>CAUTION:</strong> If this is not a newly created office the rows in the
        ElectionsOffices will also be deleted.
      </td>
    </tr>
    <tr>
      <td class="TBold">
        &nbsp;
      </td>
      <td class="T">
        <strong>Edit Specific County or Local District Offices:</strong> Use the edit links in the report to edit the office.
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <hr />
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <asp:Label ID="Label_Bulk_Offices_Report" runat="server"></asp:Label>
      </td>
    </tr>
  </table>


  <!-- Table Add Above-->
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="Table_Report_Add_Office_Anchors" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="H">
        <asp:Label ID="LabelAddOffices" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Click on an <u>ADD Office Category</u> link below to add one or more offices in
        a different office category, other than the one in shown in the title of this form.
      </td>
    </tr>
    <tr>
      <td align="center">
        <asp:Label ID="HTMLTableAddOfficesInThisCategory" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="center">
        &nbsp;&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="Table_Report_Edit_Other_Offices_At_Level" cellspacing="0"
    cellpadding="0" runat="server">
    <tr>
      <td align="left" class="H">
        <asp:Label ID="LabelOfficesHeader" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        The report below are all the offices in this office category. The right hand column
        shows the ballot order and information shown on ballots.
        <br />
        <strong>Edit an Office:</strong> Click on the <span style="text-decoration: underline">
          Office Title</span> link in the left column to change any of an office's presentation
        shown on ballots.<br />
        <strong>Adding Politicians:</strong> Normally politicians not in the database are
        added when an election is created. However, if you click an <span style="text-decoration: underline">
          Office Title</span>, the form presented will allow you to add a politician and
        associate him or her with that office.<br />
        <strong>Add Offices:</strong> Click on an 'Add' link in the header of the report
        to add offices in that category of offices.
        <br />
        <strong>Delete Offices:</strong> Offices can only b deleted on the Master panel.
      </td>
    </tr>
    <tr>
      <td align="center">
        <asp:Label ID="HTMLTableOfficesInThisCategory" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
