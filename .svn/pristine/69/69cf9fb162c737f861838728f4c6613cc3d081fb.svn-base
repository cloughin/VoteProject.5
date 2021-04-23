<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
CodeBehind="OfficeWinner.aspx.cs" Inherits="Vote.Admin.OfficeWinners" %>

<html><head><title></title></head><body></body></html>

<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<!-- REMOVED BANNER TO MAKE MORE ROOM FOR NOW : MAY PUT BACK LATER-->
<!-- <%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %> --%>
<!-- REMOVED BANNER TO MAKE MORE ROOM FOR NOW : MAY PUT BACK LATER-->
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Politician</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  </head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <!-- REMOVED BANNER TO MAKE MORE ROOM FOR NOW : MAY PUT BACK LATER-->
  <!-- <user:Banner ID="Banner" runat="server" /> -->
  <!-- REMOVED BANNER TO MAKE MORE ROOM -->
  <!-- Table -->
  <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="HSmall">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="right" class="TSmall">
        &nbsp;<asp:Label ID="PoliticianID" runat="server" CssClass="TSmall"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Winners_Heading" cellspacing="0" cols="3" cellpadding="0"
    runat="server" border="0">
    <tr>
      <td align="center" class="H">
        Winner(s) for this Office Contest
      </td>
    </tr>
    <tr>
      <td class="T">
        The report below are all the candidates for this office contest
        in this previous election.
        The candidate(s) who have been identified as winners will have (winner) under 
        the name.<br />
        <strong>Identify a Candidate as a Winner:</strong> Click on a candidate name 
        (not a picture) who <strong>has not</strong> been previously identified as a winner i.e. no 
        (winner) under the name.
        <br />
        <strong>Remove a Candidate as a Winner:</strong> Click on a candidate name (not 
        a picture) who
        <strong>has</strong> 
        been previously identified as a winner i.e. a (winner) under the name.<br />
        <strong>Edit the Office:</strong> Click on the Office Title to change something 
        about the office, like the number of positions up for election.</td>
    </tr>
    <tr>
      <td class="T">
        There is/are 
        <asp:Label ID="LabelElectionPositions" runat="server" CssClass="TBoldColor"></asp:Label>
&nbsp;position(s) up for election and <asp:Label ID="LabelIncumbents" runat="server" 
          CssClass="TBoldColor"></asp:Label>
        &nbsp;elected official
        position(s) for this office.</td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Candidates_Report" cellspacing="0" cols="3" cellpadding="0"
    runat="server" border="0">
    <tr>
      <td align="center">
        <asp:Label ID="HTMLTableWinnersForThisOffice" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Radio_Buttons_Winner_Replaced" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="2">
        Office Holder being Replced by Winner 
      </td>
    </tr>
   <tr>
      <td align="left" valign="top">
        <asp:RadioButtonList ID="RadioButtonListWinnerReplaced" runat="server" 
          AutoPostBack="True" CssClass="CheckBoxes" 
          onselectedindexchanged="RadioButtonListWinnerReplaced_SelectedIndexChanged">
        </asp:RadioButtonList>
      </td>
      <td align="left" valign="top" class="T">
        Select the current office holder to be replaced by this winning candidate.</td>
    </tr>
</table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Report_Incumbents" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" colspan="3">
        Current 
        Elected Official(s) - Incumbent(s) for <asp:Label ID="Label_Office_Name" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T" colspan="3">
        The current office incumbent(s) are shown below. 
        <br />
        <strong>Edit Incumbent:</strong> Click on a <span style="text-decoration: underline">
          Name</span> link to edit information about that incumbent, including removing 
        that person as the current elected official (incumbednt). This is information provided by the state election authority<br />
        <strong>Provide Information About Incumbent:</strong> Click on a politician picture
        or &#39;No Photo&#39; to provide biographical and issue positions for the 
        incumbent. This is information normally provided by the politician.</td>
    </tr>
    <tr>
      <td align="center" colspan="3" valign="top">
        <asp:PlaceHolder ID="IncumbentsReportPlaceHolder" runat="server"></asp:PlaceHolder>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Edit_Office" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="3" valign="top">
        Edit Office
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <br />
        Use the link below if to edit this office including changing the number of 
        office positions or identifying the
        current office incumbent(s).
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <asp:HyperLink ID="HyperLinkOffice" runat="server" CssClass="HyperLink" Target="_office">[HyperLinkOffice]</asp:HyperLink><br />
        &nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Change_Office_Contest_Candidates" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" valign="top">
        Edit Office Contest Candidates</td>
    </tr>
    <tr>
      <td align="left" class="T" valign="top">
       The link below will present you with a form where you will be able to add and remove candidates in this previous election 
        office contest.&nbsp; It will also allow you to change the order of the 
        candidates and edit their data. These are rare cases when the State adds or 
        deletes a candidate after we have been provided with the election roster, or 
        when we made a mistake when inputting the roster. </td>
    </tr>
    <tr>
      <td align="left" class="T" valign="top">
        <asp:HyperLink ID="HyperLinkChangeCandidates" runat="server" 
          CssClass="HyperLink" Target="_officecontest">[HyperLinkChangeCandidates]</asp:HyperLink><br />
        &nbsp;
      </td>
    </tr>
    </table>
  <table>
    <tr>
      <td>
        &nbsp; &nbsp; &nbsp;
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>