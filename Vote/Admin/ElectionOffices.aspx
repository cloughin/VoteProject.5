<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
CodeBehind="ElectionOffices.aspx.cs" Inherits="Vote.Admin.ElectionOffices1" %>

<html><head><title></title></head><body></body></html>

<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Election Offices</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
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
    <!--table-->
    <table class="tableAdmin" id="TableButtonsTop" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="middle" align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!--table-->
    <table class="tableAdmin" id="TableOfficeGroups" cellspacing="0" cellpadding="0" runat=server>     
      <tr>
        <td class="H">
          Groups of
          <asp:Label ID="LabelOfficeContestsGroups" runat="server"></asp:Label>
          Office Contests to Include or Exclude</td>
      </tr>
      <tr>
        <td class="T">
          As a convenience, you can include or exclude entire groups offices that are normally
          all present or not present in an election by checking or unchecking the checkboxes
          respectively.
          The actual office contests in this election are
          the offices that are checked in the section below this section.<br />
          <strong>Note:</strong> Offices that are check, but have no candidates, will 
          still appear
          on administrative reports for future addition of candidates.
          But these offices will NOT appear on public reports. Consequently checking an office, even when
          there is no candidates are identified, does no harm.</td>
      </tr>
      <tr>
        <td class="T">
          <asp:CheckBoxList ID="CheckBoxList_Offices_In_Election_Groups" runat="server" AutoPostBack="True"
            CssClass="CheckBoxes" OnSelectedIndexChanged="CheckBoxList_Offices_In_Election_Groups_SelectedIndexChanged"
            Width="686px">
          </asp:CheckBoxList></td>
      </tr>
    </table>
    <!--table-->
    <table class="tableAdmin" id="TableAllCountyAndLocal" cellspacing="0" cellpadding="0" runat=server>
      <tr>
        <td class="H">
          All County and/or Local Contest to Include or Exclude</td>
      </tr>
      <tr>
        <td class="T">
          As a convenience, you can include or exclude all the county and/or local office 
          contests. Checking or unchecking either of these checkboxes will cause all the 
          rows in the Elections and ElectionsOffices to be deleted and 
          re-inserted in these tables. Consequently the checking of a checkbox may take 
          considerable time (2 minutes for counties - 4 minutes for local districts).<br />
          <strong>Number after Checkboxes:</strong> Numbers to right of the checkboxes are 
          - Number of Offices in Counties or Local Districts / Number of these Offices in 
          this Election<br />
          <strong>Note:</strong> At the bottom of this form you can identify specific 
          counties or local districts where specific offices can be included or excluded.</td>
      </tr>
      <tr>
        <td class="TColor">
          <strong>Note:</strong> Until a snapshot / revert capability is implemented this 
          is a very dangerous operation because once all offices are included or excluded 
          there is no way to revert back to the specific offices that are currrently 
          identified.</td>
      </tr>
      <tr>
        <td class="T">
          <asp:CheckBox ID="CheckBox_All_County_Offices" runat="server" 
            AutoPostBack="True" CssClass="CheckBoxes" 
            oncheckedchanged="CheckBox_All_County_Offices_CheckedChanged" 
            Text="All Offices in All Counties" Enabled="False" />
          &nbsp;
          <asp:Label ID="Label_All_County_Offices" runat="server" CssClass="CheckBoxes"></asp:Label>
          <br />
          <asp:CheckBox ID="CheckBox_All_Local_Offices" runat="server" 
            AutoPostBack="True" CssClass="CheckBoxes" 
            oncheckedchanged="CheckBox_All_Local_Offices_CheckedChanged" 
            Text="All Offices in All Local Districts" />
        &nbsp;
          <asp:Label ID="Label_All_Local_Offices" runat="server" CssClass="CheckBoxes"></asp:Label>
&nbsp; Note: Takes a long time to run</td>
      </tr>
      <tr>
        <td class="TColor">
          Even though both capabilities are functional, they have been disabled to prevent 
          a major problem if accidently checked or unchecked. </td>
      </tr>
    </table>
    <!--table-->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          <asp:Label ID="LabelOfficeContests" runat="server"></asp:Label>
          Office Contests in this Election</td>
      </tr>
      <tr>
        <td class="T">
          <strong>Include or Exclude Office Contests:</strong> Use the checkboxes to include or
          exclude specific offices, on an office-by-office
          basis. The checkboxes that are
          checked are the actual office contests in this election.
          <br />
          <strong>Identify Candidates in each Office Contest:</strong> Use the office name
          link of each office contest that is checked, to present a form where you can identify
          the candidates for the office contest.</td>
      </tr>
      <tr>
        <td class="TSmall">
          Note: Two offices may run as a ticket in the general elections but run 
          seperately in primaries, like governor and lieutenant governor in Alaska, 
          Hawaii, Illinois, Louisiana, Massachusetts, New York, and Pennsylvania.</td>
      </tr>
      <tr>
        <td class="T">
          <asp:CheckBoxList ID="CheckBoxList_Offices_In_Election" runat="server" AutoPostBack="True"
            CssClass="CheckBoxes" OnSelectedIndexChanged="CheckBoxList_Offices_In_Election_SelectedIndexChanged"
            Width="686px">
          </asp:CheckBoxList></td>
      </tr>
    </table>
    <!--table-->
    <table id="TableCountyLinks" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          County Office Contests</td>
      </tr>
      <tr>
        <td class="T">
          Use the links below to include or exclude county and local office contests for any
          of these counties in this election.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelCounties" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!--table-->
    <table id="TableLocalLinks" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Local District and Town Office Contests&nbsp;</td>
      </tr>
      <tr>
        <td class="T">
          Use the links below to include or exclude local office contests for any of these
          local districts, towns and cities in this election.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Label ID="LabelLocalDistricts" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!--table-->
    <!--table-->
    <table class="tableAdmin" id="TableAddOffice" cellspacing="0" cellpadding="0" runat=server>
      <tr>
        <td class="H">
          Add
          <asp:Label ID="LabelOfficesAdd" runat="server"></asp:Label>
          Elected Office</td>
      </tr>
      <tr>
        <td class="T">
          This link will present a form where you can add elected offices that are NOT on
          this form
          but need to be added, so that they can be included in this election. </td>
      </tr>
      <tr>
        <td class="T" height="30">
          <asp:Label ID="LabelAddOffices" runat="server" CssClass="HyperLink"></asp:Label></td>
      </tr>
    </table>
    <!--table-->
    </form>
</body>
</html>
--%>