<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
  CodeBehind="Election.aspx.cs"
  Inherits="Vote.Admin.ElectionPage" %>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Election</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Election.css" type="text/css" rel="stylesheet" />
  <link id="This" type="text/css" rel="stylesheet" runat="server" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <user:Banner ID="Banner" runat="server" />
  <uc2:Navbar ID="Navbar" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="TableButtonsTop" cellspacing="0" cellpadding="0">
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
  </table>
  <!--table-->
  <table id="TableHeading" cellpadding="0" cellspacing="0" class="tableAdmin">
    <!-- commented out Offices Categories
      commented out -->
    <tr>
      <td align="right" class="TSmallColor">
        <asp:HyperLink ID="HyperLink_Interns" runat="server" CssClass="TSmallColor" NavigateUrl="~/Admin/Help/Volunteers/Volunteers.htm"
          Target="_help">Help</asp:HyperLink>&nbsp; &nbsp;
        <asp:HyperLink ID="HyperLink_Help" runat="server" CssClass="TSmallColor" NavigateUrl="~/Admin/Help/Election/Election.htm"
          Target="_help">Help</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        The links in the report at the bottom of this form and the links in the next two
        sections provide the necessary functions to define and maintain this election. Use
        the links in the &quot;COUNTY Office Contests and Ballot Measures&quot; and &quot;LOCAL
        District and TOWN Office Contests and Ballot Measures&quot; sections to navigate
        to counties and local districts where you will be able to define and edit any county
        and/or local office contests and ballot measures respectively.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="TableElectionMaintenance" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="H">
        Office Contests
      </td>
    </tr>
    <tr>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkElectionOffices" runat="server" CssClass="HyperLink"
          Target="_self">[HyperLinkElectionOffices]</asp:HyperLink>
        :This link will provide a form which will allow you to add or delete office contests
        in this election.<br />
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Add, Delete and Edit Candiates in the Office Contests:</strong> Click on
        an <span style="text-decoration: underline">Office Title</span> link in the report
        at the bottom of this form to be presented with a form to edit an office contest,
        including adding and deleting candidates; and/or changing office information and
        placement on ballots.
      </td>
    </tr>
    <tr>
      <td id="TrPoliticianData" align="left" class="T" runat="server">
        <strong>Add, Change and Delete Information about Candidates:</strong> Click on a
        picture link in the report at the bottom of this form to be presented with a form
        to add, change or delete biographical information, picture or issue responses on
        the politician's Introduction and Issue Pages.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>Edit Candidates: </strong>Click on a <span style="text-decoration: underline">
          Candidate Name</span> link in the report at the bottom of this form to edit information
        about that candidate on ballots and in reports, like name, ballot order, party,
        whether incumbnet, address, and contact information. You will also be able to remove
        the candidate in the office contest.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="H">
        Ballot Measures
      </td>
    </tr>
    <tr>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkReferendums" runat="server" CssClass="HyperLink" Target="_self">[HyperLinkReferendums]</asp:HyperLink>:
        This link will provide a form to add or delete referendums and ballot measures in
        this election.<br />
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Edit Ballot Measures:</strong> Click on a <span style="text-decoration: underline">
          Referendum Title</span> link in the report at the bottom of this form to edit
        information about a referendum.
      </td>
    </tr>
    <tr>
      <td class="T">
        &nbsp;
      </td>
    </tr>
  </table>
  <!--table-->
  <table id="TableWinners" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="H" colspan="2">
        Election Results (Winners - Candidates &amp; Ballot Measures)
      </td>
    </tr>
    <tr>
      <td class="T" valign="top" colspan="2">
        When the electin is over, follow the steps below to record the election contest 
        winners and the ballot measures that passed or failed.</td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <asp:Button ID="ButtonSingleCandidateWinners" runat="server" CssClass="Buttons" OnClick="ButtonSingleCandidateWinners_Click"
          Text="Identify Uncontested Winners" Width="300px" />
      </td>
      <td class="T">
        <strong>Step 1</strong>: Click this button to identify candidate winners where the 
        office was uncontested (the same number of candidates running for
        an office contest as there are office positions). This simply saves having to do
        these contests manually.
        At the conclusion the election report will be automatically updated.</td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <br />
        <strong>
        Step 2</strong>: Use the updated election report below (created at the conclusion 
        of identifying uncontested winners) to identify the remaining candidate and ballot measure winners 
        that need winners identified. Clicking an office contest and/or ballot measure
        link on the report will provide a form where the candidte(s) and ballot measure 
        winner can be easily
        identified.<br />
        <strong>Note:</strong>
        The winner of the US President contest needs to be identified in every State election
        since every State may have differnet candidates.<br />
      </td>
    </tr>
    <tr id="tr_BallotMeasures" runat=server>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkBallotMeasures" runat="server" 
          CssClass="TLargeBold" Width="300px">Identify Ballot Measures Passed or Failed</asp:HyperLink>
        <br />
        &nbsp;
      </td>
      <td class="T">
        <strong>Step 2b:</strong> Click this link to be presented with a form which will 
        allow you to record whether each ballot measure passed or failed.</td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <strong>
        <br />
        Step 3</strong>: Check the checkbox below when all winners and ballot measures
        have been identified. When you check this checkbox and some contest winners and/or
        ballot measures have not been identified, links will be provided to complete the
        task.<br />
        <strong>Note:</strong> You can also just check the checkbox below (or uncheck and
        check) to obtain a list of office contest links and/or ballot measures that do not
        yet have a winner or ballot measure identified.
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <asp:CheckBox ID="CheckBoxElectionWinners" runat="server" AutoPostBack="True" CssClass="TLargeBoldColor"
          OnCheckedChanged="CheckBoxElection_Winners_CheckedChanged" />
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <asp:Label ID="LabelWinersNotIdentified" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        <strong>
        <br />
        Step 4:</strong> Click the &#39;Update Report(s)&#39; button below to make
        this election results information available to the public.<br />
        <strong>
        <br />
        Step 5:</strong> Navigate to the State Elected Officials Form and click
        the &#39;Update Elected Officials Report&#39; button to make the newly elected officials
        available to the public.<br />
&nbsp;
      </td>
    </tr>
  </table>
  <!--table-->
  <table id="TableIncumbents" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="H">
        Currently Elected Officials (Incumbents))
      </td>
    </tr>
    <tr>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkElectionResults" runat="server" Target="view" CssClass="HyperLink">[HyperLinkElectionResults]</asp:HyperLink>
        :This link will provide a report of all the currently elected officials (incumbents).
        Office links on this report provide a form where individual politician IDs are used
        to add and remove individuals as the currently elected officials. This is useful
        when single changes need to be made.<br />
        &nbsp;
      </td>
    </tr>
  </table>
  <!--table-->
  <table id="TableViewingStatus" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td class="H">
        Change Public Viewing Status of Election &amp; Update Election Report
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        <asp:CheckBox ID="CheckBoxElectionViewable" runat="server" AutoPostBack="True" CssClass="TLargeBoldColor"
          OnCheckedChanged="CheckBoxElectionViewable_CheckedChanged" />
        &nbsp;Check when the election is viewable by the public.
      </td>
    </tr>
    <tr>
      <td class="T">
        <span id="Span1">As an election is being assembled it should not be available on ballots
          and reports for the public. Check or uncheck the above checkbox to permit ballots
          and election reports to be viewed or not viewed by the public. Checking the status
          will also update the election report. This viewable status is set at the state level.
          County and local elections can not not set individually.
          <br />
          <strong>CAUTION</strong> Checking this checkbox will cause numerous ballot and report
          pages to be constructed and may take considerable time to complete. So only check
          when you are sure the election information is complete.</span>
      </td>
    </tr>
  </table>
 
  <table id="TableLastUpdated" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td align="left" class="HSmall">
        <asp:HyperLink ID="HyperLink_View_Public_Report" runat="server" CssClass="HyperLink"
          Target="view">View Public Report</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Use this link to view the public report.
      </td>
    </tr>
  </table>

  <table id="TableCountyLinks" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="H">
        <asp:Label ID="Label_State" runat="server"></asp:Label>
        &nbsp;COUNTY Office Contests and Ballot Measures
      </td>
    </tr>
    <tr>
      <td class="T">
        Use the county links below to add, change or delete office contests and ballot measures
        of other counties in this election. You will also be provided with the ability to
        add, change and delete local district and town office contests and ballot measures
        in the county selected.<br />
        Technical Note: The first office contest defined for a county election will create
        a seperate election in the Elections Table
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelCounties" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:HyperLink ID="HyperLink_State_Election" runat="server" CssClass="HyperLink">[HyperLink_State_Election]</asp:HyperLink>
      </td>
    </tr>
  </table>
  <!--table-->
  <table id="TableLocalLinks" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="H">
        <asp:Label ID="Label_County" runat="server"></asp:Label>
        &nbsp;LOCAL District and TOWN Office Contests and Ballot Measures
      </td>
    </tr>
    <tr>
      <td class="T">
        Use the links below to add, change or delete election information for any of these
        local district and town elections in this county.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelLocalDistricts" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!--table-->
  <table class="tableAdmin" id="TableElectionInfo" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="center" class="H">
        Title and Information on Ballots and Reports
      </td>
    </tr>
    <tr>
      <td class="T">
        Enter or edit the election information in the three textboxes below. The Election
        Title is required. Additional information and ballot instructions are optional.
        Content in any textboxe is recorded when you select the Enter key or click anywhere
        outside the textbox.
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Election Title (Required)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        This title is used on Buttons &amp; Reports. A maximum of 90 characters is allowed.<br />
        Examples: November 4, 2008 Texas General Election or February 12, 2008 Virgin Democratic
        Presidential Primary
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionDesc" runat="server" Width="630px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextboxElectionDesc_TextChanged"></user:TextBoxWithNormalizedLineBreaks><br />
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Additional Election Information on Ballots and Reports (Optional)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Information provided here will appear on the top of all ballots and reports for
        this election.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionInfo" runat="server" TextMode="MultiLine" Height="90px"
          Width="920px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextboxElectionInfo_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Special Ballot Instructions (Optional)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Information provided here will appear only on the top of ballots for this election
        and will be placed below any additional election information provided in the above
        textbox.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxBallotInstructions" runat="server" TextMode="MultiLine" Height="40px"
          Width="920px" CssClass="TextBoxInputMultiLine" AutoPostBack="True" OnTextChanged="TextboxBallotInstructions_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Election Order on forVoters Page
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxElectionOrder" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxElectionOrder_TextChanged" Width="17px"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;This order is only important in primary elections when there is more than
        one ballot and election report generated for the election day. Then the major parties
        should come first, or in the case of an open primary the &#39;No Pary Preference&#39;
        Election should come first.
      </td>
    </tr>
  </table>
  <!--table-->
  <!--table-->
  <table id="TableEmails" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="H">
        Master Users Only
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Emails
      </td>
    </tr>
    <tr>
      <td class="T">
        Use this link to send emails to state contacts, candidates, and political parties
        for this election. <span id="Span2">Emails are sent for a state election or primary.
          Implementation of county and local election election emails has not yet been implemented.
          <br />
          &nbsp; </span>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:HyperLink ID="Hyperlink_Send_Emails" runat="server" CssClass="HyperLink" Target="_edit">Send Emails for This ELECTION</asp:HyperLink>
        <br />
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="TBold">
        These are the dates and number of emails that have been sent for various purposes
        for this election.
      </td>
    </tr>
    <tr>
      <td class="TBold">
        Election Roster Requested:
        <asp:Label ID="Label_Emails_Date_Election_Roster" runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Election_Roster" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="TBold">
        State Completion Notification:<asp:Label ID="Label_Emails_Date_Election_Completion"
          runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Election_Completion" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="TBold">
        Candidates providing login to enter data:<asp:Label ID="Label_Emails_Date_Candidates_Login"
          runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Candidates_Login" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="TBold">
        State Parties to Promote Candidates:
        <asp:Label ID="Label_Emails_Date_Parties_Login" runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Parties_Login" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
  </table>
  <!--table-->
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableDelete4Master" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="HSmall" valign="middle">
        Change Date of This Election
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        This utility should be used when a state changes the date for this election. Enter
        the new date and then Enter. A new election title will be constructed and displayed.
        Make any necessary changes to the election title. Then click the Change Election
        Date Button.
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" valign="middle">
        Current Date, Title and Key:
        <asp:Label ID="LabelCurrentDate" runat="server"></asp:Label>
        &nbsp; -&nbsp;
        <asp:Label ID="LabelCurrentTitle" runat="server"></asp:Label>
        &nbsp; -&nbsp;
        <asp:Label ID="LabelCurrentElectionKey" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle" class="T">
        <strong>New Election Date:</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionDate" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextboxElectionDate_TextChanged" Width="193px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;like:
        11/4/12, 11/04/2012, November 4, 2012
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle" class="T">
        <strong>New Election Title:</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionTitle" runat="server" Width="630px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextboxElectionDesc_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;90 characters max
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle" class="T">
        <asp:Button ID="ButtonChangeDate" runat="server" CssClass="Buttons" Text="Change Election Date"
          Width="200px" OnClick="ButtonChangeDate_Click" />
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" valign="middle">
        Delete This Election
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        Check this checkbox to also delete all the ElectionsPoliticians and Referendums
        rows for the election. Leave it unchecked so that the candidates for each office
        and referendums need not be reentered, in the event the an election is deleted and
        then recreated.
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle">
        <asp:CheckBox ID="CheckBoxDeletePoliticiansReferendums" runat="server" CssClass="CheckBoxes"
          Text="Also Delete Politicians and Referendums" Checked="True" />
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle" class="T">
        This button will delete all the Elections and ElectionsOffices rows for this election.
        If the checkbox above is checked it will also delete all the ElectionsPoliticians
        and Referendums rows for the election. It will delete both the State election and
        all the county elections.
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle">
        <asp:Button ID="ButtonDelete" runat="server" CssClass="Buttons" OnClick="ButtonDelete_Click1"
          Text="Delete Election" Width="200px" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableElectionStatus" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        Add Candidates from a Previous Elections
      </td>
    </tr>
    <tr>
      <td class="T">
        Many politicians run for the same office in subsequent elections. This utility will
        copy all the politicians for each office from a previous election into this election.
        It will only copy the offices that are listed. To do this copy and paste the Election
        ID in the text box and click Copy. If you do this you will only&nbsp; have to add
        and delete the changes.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="PreviousElections" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td valign="top">
        <table>
          <tr>
            <td valign="top">
              <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionKey" runat="server" Width="337px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
            </td>
            <td valign="bottom">
              <asp:Button ID="ButtonCopy" runat="server" CssClass="Buttons" OnClick="ButtonCopy_Click"
                Text="Copy" Width="113px" />
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall" align="center">
        Status Notes for Vote-USA
      </td>
    </tr>
    <tr>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxElectionStatus" runat="server" TextMode="MultiLine" Height="64px"
          Width="920px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td>
        <asp:Button ID="ButtonRecordStatus" runat="server" CssClass="Buttons" Text="Record"
          OnClick="ButtonRecordStatus_Click" Width="150px" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableCountyElectionLinks" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="HSmall">
        County Electionss
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp; Use the links to manage county elections.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelCountyElectionLinks" runat="server" EnableViewState="False"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableLocalElectionLinks" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="HSmall">
        Local District Elections
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp; Use the links to manage local district elections.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelLocalElectionLinks" runat="server" EnableViewState="False"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="H">
        NO LONGER USED
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableMiscOperations1" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="center" class="HSmall">
        Graphic of Election Results
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="ElectionResultsSource" runat="server" Width="500px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks><br />
        Source Url like: politics.nytimes.com/election-guide/2008/results/states/CA.html
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="ElectionResultsDate" runat="server" Width="167px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
        Date like: 02/06/2008
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="ButtonRecordElectionGraphic" runat="server" CssClass="Buttons" OnClick="ButtonRecordElectionGraphic_Click"
          Text="Record" Width="200px" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableMiscOperations2" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td align="left" class="HSmall">
        Candidate Letters
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        Use this link to generate a text file of candidate names and addresses to be used
        by Word in the creation of mailing labels and letters.
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle">
        <asp:HyperLink ID="HyperlinkDataFile" runat="server" CssClass="HyperLink" Target="_edit">Text File for Word Letters</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Candidate Names
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="middle">
        Use this link to go to a page to generate a report of candidate names as keyword
        phrases for Google AdWord Campaign.
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle">
        <asp:HyperLink ID="HyperLinkCandidateNames" runat="server" CssClass="HyperLink" Target="_edit">Candidate Names for Google</asp:HyperLink>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableInstructions" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="H">
        Election Management
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp; Use the links in this report to edit and manage this election as described
        at the top of this form.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <asp:PlaceHolder ID="ReportPlaceHolder" EnableViewState="False" runat="server"></asp:PlaceHolder>
  <!-- Table -->
  <br />
  &nbsp;
  <br />
  <br />
  <!-- Table -->
  </form>
</body>
</html>
--%>
