<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Elections.aspx.cs" 
Inherits="Vote.Admin.ElectionsPage" %>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Elections</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
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
  <!-- Table -->
  <table id="UpcomingElectionsTable" cellpadding="0" cellspacing="0" class="tableAdmin">
    <!-- language="javascript" onclick="return UpcomingElectionsTable_onclick()"> -->
    <tr>
      <td align="right" class="T" valign="top">
        <asp:HyperLink ID="HyperLink_Interns" runat="server" CssClass="TSmallColor" NavigateUrl="~/Admin/Help/Volunteers/Volunteers.htm"
          Target="_help">Help</asp:HyperLink>&nbsp; &nbsp;
          <asp:HyperLink ID="HyperLink_Help" runat="server" CssClass="TSmallColor" NavigateUrl="~/Admin/Help/Elections/Elections.htm"
            Target="_help">Help</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="top">
        This form allows you to provide general voter election information, create 
        elections and edit upcoming and previous elections.<br />
        Use the upcoming and previous election links 
        provided to administer and manage the information about these elections. This 
        includes adding, changing and deleting office contests and candidates.
      </td>
    </tr>
    <tr>
      <td class="H" valign="top">
        Edit Upcoming Elections
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="UpcomingHTMLElectionsTable" cellspacing="0" cellpadding="0"
    runat="server" enableviewstate="true">
  </table>
  <!-- Table -->
  <table id="TableSpace2" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="T">
        &nbsp;&nbsp;&nbsp;&nbsp;
       </td>
    </tr>
  </table>

  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <!-- Table -->
  <table id="TableFutureElections1" cellpadding="0" cellspacing="0" class="tableAdmin" runat=server>
    <tr>
      <td class="H">
        Identify Future Elections
      </td>
    </tr>
    <tr>
      <td class="T">
        The information provide in this section is intended for the state home page for 
        a particular domain (i.e. Vote-VA.org).
        The future elections defined are for 
        all upcoming elections, whether the elections have been created or not. They are 
        independent of the Upcoming Elections links. Also, when this form in 
        entered any future election in the ElectionsFuture Table that are no longer in 
        the future are deleted automatically. 
        <br />
        <strong>Directions: </strong>For each future election first enter the 
        Election Type, then the Party Code (only if the election type is P for State 
        Primary or B for Presidential Primary) and then the 
        Election Date. Press the <strong>Enter Key</strong> after each entry or change. An Election Description 
        will be automatically created after the Election Date is entered. The election description can then be changed, if 
        necessary. The Early Voting Date and Registration Deadline dates are optional.
        <br />
        <strong>Election Types (case insensitive):</strong> <br />
        O = Off Year Election held in a year between national elections, on odd numbered 
        years, on the first Tuesday in November<br />
        S = Special Election for vacated offices, or county and local elections. Not a 
        primary or November bi-annual election<br />
        P = State Primary Election, Pary, 
        All Parties or Non-prtisan, w/o US President<br />
       B = State Presidential Primary or Caucus -US President candidates only
        <br />
        <strong>Note:</strong> A General Federal Election conducted every 2 years, on 
        even number of years on the first Tuesday in November is NOT DONE WITH THIS 
        FORM.<br />
        <strong>Party Code (only for Election Types P or B):</strong> D = Democratic, R = 
        Republican, G = Green Party, L = Liberatarian, X = Non Partisan, A = All Parties
        <br />
        <strong>Election Dates:</strong> The elections NEED NOT be in date order and any date
        format is ok (i.e. 3/15/2012, 3/15/12, March 15, 2012). When Submit is clicked the elections will be sorted by date 
        and reformatted.</td>
    </tr>
    <tr>
      <td class="TColor">
        Notes:
        <br />
        The   <strong>Submit</strong> Button at the bottom of the textboxes <strong>MUST</strong> be clicked to 
        implement any future election additions and/or changes and put the elections in proper date 
        order.<br />
        To <strong>Create the most recent of these future elections</strong> click the 
        link at the 
        bottom of this form.</td>
    </tr>
    </table >

    <table id="TableFutureElections2" cellpadding="0" cellspacing="0" class="tableAdmin" border="1" runat=server>

    <tr>
      <td class="TBold" align="center">
        Election Type 
        (O,S,P,B)<br />
        Party Code (D,R,G,L,X,A) 
        <br />
        Election Date<br />
        (all textboses required)
        <br />
        Select Enter after each entry</td>
      <td class="TBold" align="center">
       Election Description<br />
        (can be edited)</td>
        <td class="TBold">Registration<br />
          Deadline<br />
          ------------------<br />
          Request Mail
          <br />
          Ballot Begin<br />
          ------------------<br />
          Absentee Ballot<br />
          Apply Begin</td>
      <td class="TBold">
       Early Voting<br />
        Begin
        <br />
        -----------------<br />
        Request Mail
        <br />
        Ballot End<br />
        -----------------<br />
        Absentee Ballot<br />
        Apply End</td>
      <td class="TBold">
        Early Voting
        <br />
        End<br />
        -----------------<br />
        Mailed Ballot<br />
        Received<br />
        -----------------<br />
        Absentee Ballot<br />
        Received</td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType1" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty1" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate1" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc1" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc1_TextChanged" ></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate1" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate10" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate1" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate11" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox3" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate9" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate12" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox4" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType2" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty2" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate2" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc2" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate2" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate13" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox5" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate2" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate2_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate14" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox6" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate15" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate16" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox7" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType3" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty3" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate3" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc3" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate3" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate17" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox8" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate3" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate3_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate18" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox9" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate19" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate20" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox10" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType4" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty4" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate4" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc4" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate4" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate21" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox11" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate4" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate4_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate22" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox12" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate23" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate24" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox13" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType5" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty5" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate5" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc5" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate5" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate25" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox14" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate5" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate5_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate26" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox15" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate27" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate28" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox16" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType6" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty6" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate6" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc6" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate6" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate29" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox17" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate6" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate6_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate30" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox18" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate31" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate32" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox19" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType7" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty7" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate7" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc7" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
        <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate7" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate33" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox20" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate7" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate7_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate34" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox21" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate35" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate36" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox22" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxType8" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxType8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxParty8" runat="server" CssClass="TextBoxInput" 
          Width="15px" AutoPostBack="True" ontextchanged="TextBoxParty8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDate8" runat="server" 
          CssClass="TextBoxInput" Width="140px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDate8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpcomingDesc8" runat="server" 
          CssClass="TextBoxInputSmall" Width="350px" AutoPostBack="True" 
          ontextchanged="TextBoxUpcomingDesc8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
         <td class="TBold">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxRegistrationDate8" runat="server" CssClass="TextBoxInputSmall"
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxRegistrationDate8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
           <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate37" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
           <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox23" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
     <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate8" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" ontextchanged="TextBoxEarlyDate8_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate38" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox24" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate39" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEarlyDate40" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox25" runat="server" CssClass="TextBoxInputSmall" 
          Width="115px" AutoPostBack="True" 
          ontextchanged="TextBoxEarlyDate1_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>


    <tr>
      <td class="TBold">
        <asp:Button ID="ButtonFutureElectionDates" runat="server" CssClass="Buttons"
          OnClick="ButtonFutureElectionDates_Click" Text="Submit" Width="150px" />
      </td>
      <td class="TBoldColor">
        Click to record data above.</td>
        <td>&nbsp;</td>
      <td>
        &nbsp;</td>
      <td>
        &nbsp;</td>
    </tr>


    <tr>
      <td class="T" colspan="5">
        <strong>Deleting a Future Election Shown Above:</strong> Only delete an election 
        if the election is incorrect. Delete each election 
        element and click Submit.</td>
    </tr>


    <tr>
      <td class="TSmallColor" colspan="5">
        Note: DO NOT DELETE A FUTURE ELECTION AFTER IT HAS BEEN CREATED because this 
        information is used on the home page of each state and in emails and needs to be 
        retained. </td>
    </tr>
    </table >

  <!-- Table -->

  <table id="PreviousElectionsTable" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="T">
        &nbsp; &nbsp;
      </td>
    </tr>
    <tr>
      <td class="H">
        Edit Previous Elections
      </td>
    </tr>
    <tr>
      <td class="T">
        Data can be changed for these elections, even though this is normally not done after
        the election.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="PreviousElectionsHTMLTable" cellspacing="0" cellpadding="0"
    runat="server" enableviewstate="true">
  </table>
  <!-- Table -->
  <table id="TableSpace" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td class="T">
        &nbsp;&nbsp;&nbsp;&nbsp;
       </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Here -->
  <!-- Table -->
  <table id="TableElectionSpecs" cellpadding="0" cellspacing="0" class="tableAdmin"
    runat="server">
    <tr>
      <td align="center" class="H">
        Voter Information</td>
    </tr>
    <tr>
      <td align="center" class="HSmall">
        &nbsp;Election Polls
       </td>
    </tr>
    <tr>
      <td class="T">
        <strong>State Normal Polling Hours:</strong> Enter any time range format, like &quot;7:00 AM-7:00 PM&quot; 
        or an explaination like &quot;Poll times varry depending on the county and location&quot;. Then select 
        Enter.</td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPollHours" runat="server" Width="900px" 
          AutoPostBack="True" CssClass="TextBoxInputMultiLine" 
          ontextchanged="TextBoxPollHours_TextChanged" Height="40px" 
          TextMode="MultiLine"></user:TextBoxWithNormalizedLineBreaks>
        &nbsp;&nbsp; </td>
    </tr>
    <tr>
      <td class="T">
        <strong>State URL of Polling Hours:</strong> Enter state url with or without 
        //http: Then select Enter.</td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPollHoursUrl" runat="server" Width="900px" 
          AutoPostBack="True" CssClass="TextBoxInput" 
          ontextchanged="TextBoxPollHoursUrl_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        </td>
    </tr>
    <tr>
      <td class="T">
        <strong>State URL of Polling Places:</strong> Enter state url with or without 
        //http: Then select Enter.</td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPollPlacesUrl" runat="server" Width="900px" 
          AutoPostBack="True" CssClass="TextBoxInput" 
          ontextchanged="TextBoxPollPlacesUrl_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
        </td>
    </tr>
    <tr>
      <td align="center" class="HSmall">
        Normal
        Voting</td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem Value="Polls">Voting Done at Polls</asp:ListItem>
          <asp:ListItem Value="Mail">Voting Done EXCLUSIVELY by Mail</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Other Voting Services</td>
    </tr>
    <tr>
      <td class="T">
        <asp:CheckBox ID="CheckBox3" runat="server" CssClass="CheckBoxes" 
          Text="Has Early Voting" />
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:CheckBox ID="CheckBox4" runat="server" CssClass="CheckBoxes" 
          Text="Has Voting by Mail" />
      &nbsp;
      </td>
    </tr>
  </table>

  <!-- Table -->

  <table id="TableElectionPolls" class="tableAdmin"  cellspacing="0" cellpadding="0" runat=server>
  <tr>
      <td class="HSmall">
        Description of How Voting is Done at the Polls</td>
  </tr>
  <tr>
      <td class="T">
        Description of what to expect at the polls like machines or paper ballots, 
        identification needed...</td>
  </tr>
  <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox26" runat="server" Height="70px" TextMode="MultiLine" 
          Width="900px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
  </tr>
  <tr>
      <td class="HSmall">
        Primary Elections
      </td>
  </tr>
  <tr>
      <td class="T">
        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" Checked="True" CssClass="CheckBoxes" 
          Text="State Primaries Have Seperate Ballots for Political Parties" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:CheckBox ID="CheckBox2" runat="server" Checked="True" 
          CssClass="CheckBoxes" 
          Text="Presidential Primaries Have Seperate Ballots for Political Parties" />
      </td>
  </tr>
  <tr>
      <td class="HSmall">
        Political Parties Having Seperate Ballots</td>
  </tr>
  <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox2" runat="server" 
          Width="900px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
  </tr>
  <tr>
      <td class="HSmall">
        Description of how primary elections are conducted 
        (type of primary)</td>
  </tr>
  <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox1" runat="server" Height="120px" TextMode="MultiLine" 
          Width="900px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
  </tr>
  <tr>
      <td class="T">
        &nbsp;&nbsp; &nbsp;</td>
  </tr>
  </table>

  <!-- Table -->
  <table id="TableCreateNewElectionState" runat="server" cellpadding="0" cellspacing="0"
    class="tableAdmin">
    <tr>
      <td align="center" class="H" valign="top">
        Create an Election</td>
    </tr>
    <tr>
      <td class="T">
        <strong>State Elections:</strong> The link below will present a form to create 
        the described election. The election is automatically set to the next election in the list 
        of future elections above, that has not already been created and is not a 
        General Electlion (type G). General elections are Federal elections held every 2 years on even numbered years 
        and 
        are created for all states in one operation outside this form and consequently 
        can 
        not be created here.<br />
        <strong>County and Local Elections: </strong>To create a county or local 
        election you always need to first create a state election. A seperate county or 
        local election is created AUTOMATICALLY the first time you identify the first 
        county or local office contest.</td>
    </tr>
    <tr>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkCreateNewElection" runat="server" CssClass="HyperLink"
          Target="_edit">[HyperLinkCreateNewElection]</asp:HyperLink>
        <br />
&nbsp;
      </td>
    </tr>
    </table>
  <!-- Table -->
  <table id="TableCreateNewElectionPresident" runat="server" cellpadding="0" cellspacing="0"
    class="tableAdmin">
    <tr>
      <td align="center" class="H" valign="top">
        Master Users Only</td>
    </tr>
    <tr>
      <td class="HSmall">
        Create a National Presidential Primary</td>
    </tr>
    <tr>
      <td class="T">
        Two types of national presidential primaries can be created, both have an 
        ElectionType = A but with different StateCodes. 
        <br />
        1) Presidential Primary of 
        Candidates still Remaining in Race (StateCode = US). It is used as a link on top 
        of each Defarul.aspx page and candidates are deleted as they drop out of the 
        race. Eventually, there will only be one candidate and the link will be removed 
        from the top of the pages.<br />
        2) Template of All 
        Major Presidential Primary Candidates that entered the race (StateCode = PP). 
        It is used for two purposes. First, it is used as a template so that candidates 
        can be automatically added to each state presidential contest, eliminating the 
        need to do this manually.
        Second, it provides a canonical ElectlionKey for Election.aspx and Issue.aspx pages that state presidential primaries point to, avoiding duplicate pages, 
        i.e. duplicate page 
        content which Google penalizes for.</td>
    </tr>
    <tr>
      <td class="T">
        <strong>To Create:</strong> Select the Presidential Primary type. Then select the party. Enter the date of the presidential 
        primary and strike Enter. A link should be created describing the election. 
        Click the link if the election template description is correct.</td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonListPresPrimaryType" runat="server" 
          CssClass="RadioButtons" RepeatDirection="Horizontal">
          <asp:ListItem Value="US">Remaining Presidential Party Primary Candidates</asp:ListItem>
          <asp:ListItem Value="PP">Template of Major Presidential Party Primary Candidates</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonListParty" runat="server" CssClass="RadioButtons"
          RepeatDirection="Horizontal" Width="350px">
          <asp:ListItem Value="D">Democratic</asp:ListItem>
          <asp:ListItem Value="R">Republican</asp:ListItem>
          <asp:ListItem Value="G">Green</asp:ListItem>
          <asp:ListItem Value="L">Libertarian</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="T">
        <strong>Date:</strong>&nbsp; 
        <user:TextBoxWithNormalizedLineBreaks ID="TextboxElectionDate" runat="server" CssClass="TextBoxInput" Width="172px"
          AutoPostBack="True" OnTextChanged="TextboxElectionDate_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      &nbsp;Like: 11/6/12, 2/16/2012, November 6, 2012</td>
    </tr>
    <tr>
      <td class="T">
        <br />
        <asp:HyperLink ID="HyperLinkCreateNewElectionPresident" runat="server" CssClass="HyperLink"
          Target="_edit">[HyperLinkCreateNewElectionPresident]</asp:HyperLink>
        <br />
&nbsp;&nbsp;
        </td>
    </tr>
    </table>

  </form>
</body>
</html>
--%>