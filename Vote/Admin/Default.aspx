<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
 CodeBehind="Default.aspx.cs" Inherits="Vote.Admin.Default" %>

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
  </table>

  <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
    <tr>
      <td align="right" class="T">
        &nbsp;<asp:HyperLink ID="HyperLink_Interns" runat="server" CssClass="TSmallColor"
          NavigateUrl="~/Admin/Help/Volunteers/Volunteers.htm" Target="_help">Help</asp:HyperLink>&nbsp;
        &nbsp;<asp:HyperLink ID="HyperLink_Help" runat="server" CssClass="TSmallColor" NavigateUrl="~/Admin/Help/Default/Default.htm"
          Target="_help">Help</asp:HyperLink>
      </td>
    </tr>
  </table>
  
  <%--
  <table class="tableAdmin" id="TableStateElection" cellspacing="0" cellpadding="0" runat=server>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElections" runat="server" CssClass="HyperLink" Target="_self"
          ToolTip="Administration of all upcoming and previous state elections."></asp:HyperLink>
      &nbsp;&nbsp;
        </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to 
        create and/or edit upcoming or previous elections, including the election status, office contests
        and candidates. You will also be able to send emails. </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TablePresidentialPrimary" cellspacing="0" cellpadding="0" runat=server>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElectionsUS" runat="server"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to create and/or edit a 
        party presidential primary of candidates remaining in the national race.</td>
    </tr>
   </table>

  <table class="tableAdmin" id="TablePresidentialPrimaryTemplate" cellspacing="0" cellpadding="0" runat=server>
   <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElectionsPP" runat="server"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to create and/or edit a 
        template of a party presidential primary of all major candidates in the national 
        race. The template is used to create duplicate state presidential primaries to 
        eliminate manual creation and provide a canonical landing page eliminating 
        duplicate Election.aspx and Issue.aspx pages.</td>
    </tr>
   </table>

  <table class="tableAdmin" id="TableStateByStateElections" cellspacing="0" cellpadding="0" runat=server>
    <tr>
      <td class="T">
        <asp:HyperLink ID="HyperLinkOfficialsUD" runat="server" CssClass="HyperLink" Target="_self"
          
          ToolTip="Report to easily enter the results on an election and identify the currently elected officials (incumbents)">[HyperLinkOfficialsUD]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to edit federal elected 
        officials.</td>
    </tr>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElectionsUD" runat="server"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to edit state-by-state 
        federal elections. These are 
        not really seperate elections. They are constructed by 
        extracting specific election contests from each state and have a StateCode of 
        U1...U4. </td>
    </tr>

   </table>

  <table class="tableAdmin" id="TableStateElectionMaint" cellspacing="0" cellpadding="0" runat=server>

    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkOfficials" runat="server" CssClass="HyperLink" Target="_self"
          ToolTip="Report to easily enter the results on an election and identify the currently elected officials (incumbents)">[HyperLinkOfficials]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to identify the winning candidates
        (new incumbents) after an election or to identify the current elected officials
        for elected offices.
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElectedOffices" runat="server" CssClass="HyperLink" Target="_offices"
          ToolTip="Administration of all elected offices">[HyperLinkElectedOffices]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to add or edit all elected
        offices, without regards to any particular election.
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkPoliticians" runat="server" CssClass="HyperLink" Target="_politicians"
          ToolTip="Add and edit politicians independent of any election">[HyperLinkPoliticians]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to add or edit politicians.
        A politician is any person who sought or held any elected office.
      </td>
    </tr>
  </table>
  --%>

  <table class="tableAdmin" id="TableCountyLinks" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:Label ID="Label_Counties" runat="server"></asp:Label>
      &nbsp;Administration</td>
    </tr>
    <tr>
      <td class="T">
        Use the links below 
        to add, change or delete: elections, elected offices, elected officials, 
        politicians, candidates for counties, including the local districts and towns 
        within each county</td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelCounties" runat="server"></asp:Label>
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableLocalLinks" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:Label ID="Label_County" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelLocalDistricts" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="LabelLocalDesc" runat="server"></asp:Label>
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableLocalLinksEdit" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkLocalDistricts" runat="server" CssClass="HyperLink" Target="edit">[HyperLinkLocalDistricts]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        Use this link to add a local district or town not listed, or to change the name of a local district
        or town (as it should appear on ballots): 
      </td>
    </tr>
    </table>

  <table class="tableAdmin" id="TableMasterOnly" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="H">
        Master Users Only
      </td>
    </tr>
  </table>
  
  <%--
  <table class="tableAdmin" id="TableBulkCountyOfficeAdds" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkBulkCountyOfficeAdds" runat="server" 
          CssClass="HyperLink"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Label_BulkCountyOfficeAdds" runat="server"></asp:Label>
        <br />&nbsp;
      </td>
    </tr>
  </table>
  --%>

  <table class="tableAdmin" id="TableMultiCountyJudicialDistricts" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkJudicialDistricts" runat="server" 
          CssClass="HyperLink"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Label_JudicialDistricts" runat="server"></asp:Label>
        <br />&nbsp;
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableMultiCountyOtherDistricts" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkMultiCountyDistricts" runat="server" 
          CssClass="HyperLink"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Label_MultiCountyDistricts" runat="server"></asp:Label>
        <br />&nbsp;
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableMultiCountyPartyDistricts" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkPartyDistricts" runat="server" 
          CssClass="HyperLink"></asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Label_PartyDistricts" runat="server"></asp:Label>
        <br />&nbsp;
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableBallotDesign" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkBallotDesign" runat="server" CssClass="HyperLink" Target="edit"
          ToolTip="Special characteristics on ballots, like whether to indicate the incumbent, how nicknames are presented...">[HyperLinkBallotDesign]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to define ballot characteristics,
        like whether to indicate the incumbent and how nicknames are presented.
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableElectionAuthority" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkElectionAuthority" runat="server" CssClass="HyperLink"
          Target="edit" ToolTip="Add and edit information about the election authority">[HyperLinkElectionAuthority]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you will be able to add and edit election authority
        information useful to voters.
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkPoliticalParties" runat="server" 
          CssClass="HyperLink" Target="edit">[HyperLinkPoliticalParties]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        This link will present a form where you well be able to edit political party information,
        including email addresses.
      </td>
    </tr>
  </table>
 
  <%--
  <table class="tableAdmin" id="TableSendEmails" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperlinkSendEmail" runat="server" CssClass="HyperLink" NavigateUrl="/Admin/Emails.aspx"
          Target="edit">Send Emails</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="TBold">
        These are the dates and number of emails that have been sent for the various purposes.
      </td>
    </tr>
    <tr>
      <td class="T">
        Request Primary Election Roster:
        <asp:Label ID="Label_Emails_Date_Roster_Primary" runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Roster_Primary" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        Request General Election Roster:
        <asp:Label ID="Label_Emails_Date_Roster_General" runat="server" CssClass="TColor"></asp:Label>&nbsp;
        <asp:Label ID="Label_Emails_Sent_Roster_General" runat="server" CssClass="TColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Note: </strong>Notification of election roster completion is done using 
        the Election.aspx form.</td>
    </tr>
  </table>
  --%>

  <table class="tableAdmin" id="TableNotes" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        &nbsp;</td>
    </tr>
    <tr>
      <td class="HSmall">
        Status Notes of Statewide, Judicial and County Elected Offices and Incumbents
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Use these notes to keep track of the status of the various electoral data and districts.
      </td>
    </tr>
    <tr>
      <td class="T">
        Statewide:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficesStatusStatewide" 
          runat="server" TextMode="MultiLine"
          Height="27px" Width="800px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        Judicial: &nbsp;&nbsp; &nbsp;<user:TextBoxWithNormalizedLineBreaks 
          ID="TextBoxOfficesStatusJudicial" runat="server"
          TextMode="MultiLine" Height="29px" Width="800px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        Counties:&nbsp;
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficesStatusCounties" runat="server" TextMode="MultiLine"
          Width="800px" Height="26px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="ButtonRecordOfficesStaus" runat="server" OnClick="ButtonRecordOfficesStaus_Click"
          Text="Record" CssClass="Buttons" Width="160px" />
        Click to record or update the above notes.
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableMisc" cellspacing="0" cellpadding="0" border="0"
    runat="server">
    <tr>
      <td colspan="2" class="HSmall">
        Misc
      </td>
    </tr>
    <tr>
      <td>
        <asp:HyperLink ID="HyperLinkNames" runat="server" CssClass="HyperLink" Target="view">Google Elected Names</asp:HyperLink>
      </td>
      <td class="T">
        Click this button to generate a report of elected officials names for Google AdWord
        Campaign.
      </td>
    </tr>
    <tr>
      <td colspan="2">
        <table id="Table8" cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td class="T">
              Use BOE's Banner on Vote-XX.org Domains:
            </td>
            <td>
              <asp:RadioButtonList ID="RadioButtonListUseBOLBanner" runat="server" CssClass="RadioButtons"
                RepeatDirection="Horizontal">
                <asp:ListItem Value="F">No - Don't Use Banner</asp:ListItem>
                <asp:ListItem Value="T">Yes - Use Banner</asp:ListItem>
              </asp:RadioButtonList>
            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td colspan="2" align="left">
        <table id="Table9" cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td valign="middle">
              <asp:HyperLink ID="HyperLinkRecordNote" runat="server" CssClass="HyperLink" Target="_self"
                NavigateUrl="/Master/NotesRecord.aspx" Width="150px">Record a Note</asp:HyperLink>
            </td>
            <td valign="middle">
              <asp:HyperLink ID="HyperLinkViewNotes" runat="server" CssClass="HyperLink" Target="view"
                NavigateUrl="/Master/NotesView.aspx" Width="150px">View Notes</asp:HyperLink>
            </td>
            <td valign="middle">
            </td>
            <td valign="middle">
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
    </ContentTemplate>
  </asp:UpdatePanel>
  </div>
</asp:Content>
