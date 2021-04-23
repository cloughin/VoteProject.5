<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
CodeBehind="Emails.aspx.cs"
  Inherits="Vote.Master.EmailCandidates" %>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Email</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <user:Banner ID="Banner" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="Table3" cellspacing="1" cellpadding="1" width="100%"
    border="0">
    <tr>
      <td class="HLarge">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td>
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="H">
        Instructions
      </td>
    </tr>
    <tr>
      <td class="T" style="height: 13px">
        Some email templates are stored in the database. These templates or custom text 
        file templates can be used. When a database template option is selected, the 
        subject and body are automatically loaded, substitutions are made, checked for 
        problems, and the email addresses are extracted from the database. When a custom 
        text file template is selected the subject and body needs to be manually 
        entered, usually copied from a text file. Then the Make Substitutions &amp; Check 
        Button needs to be clicked to check for problems and extract the email 
        addresses. In both cases, the email addresses are presented at the bottom of 
        this form and the Stats Section will show a count and the last time emails were 
        sent if a database template is selected.
        <br />
        <strong>Normal Steps: </strong>
        <br />
        Select a radio button in the Email Templates &amp; Recipients Section..<br />
        Check the Status 
        Section.
        <br />
        Check the list of email addresses at the bottom of the form.<br />
       Either 
        check or enter a Subject and Body in the textboxes provided.<br />
        Check that the From and Cc options are correct..<br />
        Make any desired changes and click Make Substitutions Button to view changes.<br />
        Click the Send Email as a Test Button.<br />
        Check the test email.<br />
       Click Send Emails Button if test email is ok.<br />
        </td>
    </tr>
    <tr>
      <td class="H">
        Email Templates &amp; Recipients</td>
    </tr>
    <tr>
      <td class="HSmall">
        Custom Substitutions in Email Subject and Body </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Formats: String</strong>: [[string]] <strong>Anchor</strong>: 
        ##anchor## <strong>Email</strong>: @@email address@@&nbsp;
        <br />
        (Note: Returns empty string if item does not exist)<br />
        <strong>ALL Emails:</strong>
        <br />
        [[Date]]&nbsp;&nbsp; ##Vote-USA.org##&nbsp; 
        ##Buttons##&nbsp; ##IVN Home##&nbsp; ##IVN Vote-USA##<br />
        @@ron.kahlow@vote-usa.org@@&nbsp; 
        @@mgr@vote-usa.org@@&nbsp;&nbsp;&nbsp; @@electionmgr@vote-usa.org@@&nbsp;&nbsp; 
        <br />
        <strong>ALL Emails based on StateCode Passed::</strong>
        <br />
       [[Vote-XX.org]] ##Vote-XX.org## [[StateCode]] [[State]] [[Contact]] 
        [[BallotName]] [[ContactEmail]] ##OfficialsReport##
        <br />
        <strong>Latest Upcoming Viewable Election/Primary Emails based on 
        StateCode Passed &amp; Elections Table:</strong>
        <br />
        [[ElectionDesc]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        [[ElectionDate]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        [[ElectionDescGeneral]]&nbsp; [[ElectionDateGeneral]] 
        <br />
        [[ElectionDescOffYear]]&nbsp; [[ElectionDateOffYear]]&nbsp; [[ElectionDescSpecial]]&nbsp;&nbsp; 
        [[ElectionDateSpecial]]&nbsp;
        <br />
        [[ElectionDescPrimary]]&nbsp; [[ElectionDatePrimary]]<br />
        <strong>All Viewable Primaries Emails on Same Day based on StateCode &amp; Elections 
        Table (separated by breaks):</strong><br />
    [[FuturePrimariesDesc]] =&gt; list of primary descriptions separated by &lt;br&gt;&nbsp;&nbsp; 
        <br />
        ##FuturePrimariesRosters## =&gt; list of anchors of Primary Rosters separated by 
        &lt;br&gt;<br />
        <strong>Specific Election Emails based on ElectionKey Passed &amp; 
        Elections Table:</strong><br />
        [[Election]] =&gt; election or single party primary description&nbsp;&nbsp; ##ElectionRoster## =&gt; anchor 
        for an Election Directory 
        <br />
        [[Primaries]] =&gt; all party primary descriptions on the same day seperated by &quot;, 
        and&quot;<br />
        <strong>All Emails for Future Election/Primary using StateCode Passed &amp; 
        ElectionsFuture Table</strong>:
        <br />
        [[FutureElectionDescNext]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [[FutureElectionDateNext]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        [[FutureElectionDescGeneral]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [[FutureElectionDateGeneral]]&nbsp;
        <br />
        [[FutureElectionDescPrimary]]&nbsp; [[FutureElectionDatePrimary]]&nbsp; 
        [[FutureElectionDescOffYear]]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        [[FutureElectionDateOffYear]]&nbsp; 
         <br />
        [[FutureElectionDescSpecial]]&nbsp;&nbsp; [[FutureElectionDateSpecial]]&nbsp;&nbsp; 
        [[FutureElectionDescPresidentialPrimary]]&nbsp; 
        [[FutureElectionDatePresidentialPrimary]]<br />
        <strong>Only Emails to Candidates:</strong>
        <br />
        [[Politician]] [[UserName]] [[Password]] [[FName]] [[LName]] 
        ##Vote-XX.org/Intro##&nbsp; ##Vote-XX.org/Politician##&nbsp; [[Office]] =&gt; office 
        description<br />
        ##Vote-XX.org/Issue##&nbsp;&nbsp;&nbsp; [[Politicians]]<br />
        <strong>Only Emails to Parties (empty string if data not available):</strong><br />
        [[Party]] [[PartyEmail]] [[PartyPassword]] [[PartyContactFirstName]] 
        [[PartyContactLastName]] [[PartyContactTitle]] ##Vote-XX.org/Party##</td>
    </tr>
   </table>
 <!-- Table2 -->
  <table class="tableAdmin" id="Table_Election" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="HSmall">
  ELECTION/PRIMARIES: State Contacts, Candidates, Political Parties... (coming from /Admin/Election.aspx)
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Custom Text Files are available and should be used for State Contacts for this Election 
        or Primary:</strong>
        <br />
        - to Notify state of 
        <strong>primary election</strong> completion and request ballot measures. A 
        custom template is used because primaries are normally more than one election on 
        the same day.<br />
        - 
        to Request candidate email addresses<br />
        <strong>Custom Text Files are available for Political Party Contacts in State for this 
        Election or Primary:</strong><br />
        - 
        to provide Login credentials to promote party candiates 
        <br />
        - to Request candidate email addresses</td>
    </tr>
    <tr>
      <td>
        <asp:RadioButtonList ID="RadioButtonList_Election" runat="server" CssClass="RadioButtons"
          Width="98%" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_Election_SelectedIndexChanged">
          <asp:ListItem Value="ElectionCustomStates">Custom Text File for State Contacts for this Election or Primary</asp:ListItem>
          <asp:ListItem Value="ElectionCustomCandidates">Custom Text File for Candidates in this Election or Primray</asp:ListItem>
          <asp:ListItem Value="ElectionCustomParties">Custom Text File for Political Party Contacts in State for this Election or Primary</asp:ListItem>
          <asp:ListItem Value="ElectionCompletion">Database Template to Notify State Contacts of this Single ELECTION (not Primary) Completion</asp:ListItem>
          <asp:ListItem Value="ElectionCandidatesLogin">Database Template to Provide Candidates with Login for this Election</asp:ListItem>
          <asp:ListItem Value="ElectionPartiesLogin">Database Template to Provide State Party Contacts with Login for this Election</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table2 -->
  <table class="tableAdmin" id="Table_State" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="HSmall">
        SINGLE STATE: State Contacts, Candidates, Political Parties... (coming from 
        /Admin/Default.aspx)</td>
    </tr>
    <tr>
      <td>
        <asp:RadioButtonList ID="RadioButtonList_State" runat="server" CssClass="RadioButtons"
          Width="98%" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_State_SelectedIndexChanged">
          <asp:ListItem Value="StateCustom">Custom Text File for State Contacts</asp:ListItem>
          <asp:ListItem Value="StateCustomCandidates">Custom Text File for Candidates in Next Single Viewable Upcoming Election in State</asp:ListItem>
          <asp:ListItem Value="StateCustomParties">Custom Text File for Political Parties in State</asp:ListItem>
          <asp:ListItem Value="StatePrimaryRosters">Database Template to Request Primary Election Roster from State Contacts (based on in Elections Table)</asp:ListItem>
          <asp:ListItem Value="StateGeneralRosters">Database Template to Request General Election Roster from State Contacts (based on Elections Table)</asp:ListItem>
          <asp:ListItem Value="StateCandidates">Database Template to Candidates in Next Single Viewable Upcoming Election in State</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table2 -->
  <table class="tableAdmin" id="Table_All" cellspacing="0" cellpadding="0" border="0" runat=server>
    <tr>
      <td class="HSmall">
        ALL STATES: State Contacts, Candidates, Political Parties... (coming from /Master/Default.aspx)
      </td>
    </tr>
    <tr>
      <td>
        <asp:RadioButtonList ID="RadioButtonList_All" runat="server" CssClass="RadioButtons"
          Width="98%" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_All_SelectedIndexChanged">
          <asp:ListItem Value="AllCustomStates">Custom Text File for ALL States' Contacts (based on States Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesPrimaryElection">Custom Text File only for States' Contacts with Future PRIMARY Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesGeneralElection">Custom Text File only for States' Contacts with Future GENERAL Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesOffYearElection">Custom Text File only for States' Contacts with Future OFF-YEAR Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesSpecialElection">Custom Text File only for States' Contacts with Future SPECIAL Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesPresidentialPrimaryElection">Custom Text File only for States' Contacts with Future PRESIDENTIAL PRIMARY Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesNoPrimaryElection">Custom Text File only for States' Contacts with NO Future PRIMARY Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesNoGeneralElection">Custom Text File only for States' Contacts with NO Future GENERAL Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesNoOffYearElection">Custom Text File only for States' Contacts with NO Future OFF-YEAR Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesNoSpecialElection">Custom Text File only for States' Contacts with NO Future SPECIAL Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomStatesNoPresidentialPrimaryElection">Custom Text File only for States' Contacts with NO Future PRESIDENTIAL PRIMARY Election (based on ElectionsFuture Table)</asp:ListItem>
          <asp:ListItem Value="AllCustomCandidates">Custom Text File for Candidates in Next Viewable Upcoming Elections in All States</asp:ListItem>
          <asp:ListItem Value="AllCustomParties">Custom Text File for Political Parties in ALL States</asp:ListItem>
          <asp:ListItem Value="AllPrimaryRosters">Database Template to Request Primary Election Roster from ALL States Contacts (based on Elections Table)</asp:ListItem>
          <asp:ListItem Value="AllGeneralRosters">Database Template to Request General Election Roster from ALL States Contacts (based on Elections Table)</asp:ListItem>
          <asp:ListItem Value="AllCandidates">Database Template to Candidates in Next Viewable Upcoming Elections in All States</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table2 -->
  <table class="tableAdmin" id="Table8" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="T" align="center">
        <asp:Button ID="Button_Reload_Template" runat="server" CssClass="Buttons" 
          onclick="Button_Reload_Template_Click" Text="Reload Selected Database Template" 
          Width="300px" />
      </td>
      </tr>
    <tr>
      <td class="HSmall">
        Status
      </td>
      </tr>
      <tr>
        <td class="T">
          Emals that will be sent for selection:
          <asp:Label ID="Label_Emails_Sending" runat="server" CssClass="TBoldColor"></asp:Label>
        </td>
    </tr>
    <tr>
      <td class="T">
        Last Time Emails Sent for selection:
        <asp:Label ID="Label_Emails_Last_Sent_Date" runat="server" CssClass="TBoldColor"></asp:Label>
        <asp:Label ID="Label_Election" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="H">
        From: To: Cc: Subject: Body:
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        From:
      </td>
    </tr>
    <tr>
      <td class="T">
        Default is mgr@vote-usa.org
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_From" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem Value="Mgr">From: mgr@vote-usa.org (emails to candidates)</asp:ListItem>
          <asp:ListItem Value="RonKahlow">From: ron.kahlow@vote-usa.org (emails to States)</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        To:
      </td>
    </tr>
    <tr>
      <td class="T">
        Emails will be sent to the addresses as indicated by the type of email selected.
        Test emails will always sent to ron.kahlow@businessol.com
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Cc:
      </td>
    </tr>
    <tr>
      <td class="T">
        Default is no copies. To obtain a copy of all the emails sent use these radio buttons
        to select the cc email address.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_Cc" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem Value="None">No copies will be sent</asp:ListItem>
          <asp:ListItem Value="ElectionMgr">Cc: electionmgr@vote-usa.org</asp:ListItem>
          <asp:ListItem Value="Mgr">cc: mgr@vote-usa.org</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td align="center" class="HSmall">
        Mail Format:
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Default is Text.
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_MailFormat" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem>Text</asp:ListItem>
          <asp:ListItem>Html</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Sample Ballot
        Attachments:
      </td>
    </tr>
    <tr>
      <td class="T">
        Select the second radio button to add the two 
        &quot;Get Your Sample Ballot&quot; images as attachments to each email;
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_Attachments" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem>None</asp:ListItem>
          <asp:ListItem Value="SampleBallot">Small and Large Get Your Sample Ballot Images</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Other Attachments</td>
    </tr>
    <tr>
      <td class="T">
        Enter any other attachemts separated by commas (ex1.jpg,ex2.jpg): NOT IMPLEMENTED
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Attachments" runat="server" Width="930px" Enabled="False"
          CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="center" class="HSmall">
        Custom Substitutions in Email Subject and Body
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Subject:
      </td>
    </tr>
    <tr>
      <td class="T">
        Subject <strong>BEFORE</strong>:
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="SubjectBefore" runat="server" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        Subject <strong>AFTER</strong>:&nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="SubjectAfter" runat="server" Width="930px" Enabled="False" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <!-- Table1 -->
  <table class="tableAdmin" id="Table1" cellspacing="1" cellpadding="1" width="100%"
    border="1">
    <tr>
      <td align="center" class="HSmall">
        Body:
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        Body <strong>BEFORE</strong> subsitutions are made:<br>
        (1st Candidate in in Election is used for test)
      </td>
    </tr>
    <tr>
      <td align="center">
        <user:TextBoxWithNormalizedLineBreaks ID="EmailBodyBefore" runat="server" TextMode="MultiLine" Rows="40" Width="920px"
          Height="400px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
       For a non-custom selection, use this button to save any changes made to the Subject and Body without sending
        any emails.
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        <asp:Button ID="Button_Save_Changes" runat="server" CssClass="Buttons" OnClick="Button_Save_Changes_Click"
          Text="Save Template Changes" Width="300px" />
      </td>
    </tr>
    <tr>
      <td class="T">
        Use this button to check custom text files and/or any changes and substitutions you made on the BEFORE Subject
        and Body before you send the emails. 
      </td>
    </tr>
    <tr>
      <td align="center">
        <asp:Button ID="Button_View_Substitutions" runat="server" CssClass="Buttons" OnClick="Button_View_Substitutions_Click"
          Text="Make Substitutions &amp; Check" Width="300px" />
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        Body  <strong>AFTER</strong> subsitutions are made:<br />
        (1st Candidate in in Election is used for test)
      </td>
    </tr>
    <tr>
      <td align="center">
        <user:TextBoxWithNormalizedLineBreaks ID="EmailBodyAfter" runat="server" Rows="40" TextMode="MultiLine" Width="920px"
          Height="400px" CssClass="TextBoxInputMultiLine" ReadOnly="True"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <!-- Table2 -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="H">
        Send Emails
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Send Test Emails
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
        Use these buttons to send a single test email to ron.kahlow@vote-usa.org to confirm
        SMTP is working correctly. The first will use the subject and content of the first
        email. The second button will send an email with subject 'test email' and body of
        'test'.
      </td>
    </tr>
    <tr>
      <td align="center" class="T" colspan="2">
        <asp:Button ID="Button_Test_First_Email" runat="server" CssClass="Buttons" OnClick="Button_Test_First_Email_Click1"
          Text="Send the First Email as a Test" Width="300px" />
        <asp:Button ID="Button_Emails_To_Ron" runat="server" 
          OnClick="Button_Emails_To_Ron_Click" Text="Send All Emails to ron.kahlow@vote-usa.org"
          CssClass="Buttons" Width="300px" Enabled="False" />
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Use this button to actually send the emails. After the emails are sent the template
        is updated so the same wording will be used for subsequent emails of the same type.
      </td>
    </tr>
    <tr>
      <td align="center" colspan="2">
        <asp:Button ID="Button_Send_All_Emails" runat="server" CssClass="Buttons" Text="SEND ALL EMAILS"
          OnClick="Button_Send_All_Emails_Click1" Width="400px"></asp:Button>
      </td>
    </tr>
  </table>
  <!-- Table4 -->
  <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="H">
        Email Addresses
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="EmailAddresses" runat="server" CssClass="TSmall"></asp:Label>
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>