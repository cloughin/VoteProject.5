<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="LDSDataUpdate.aspx.cs" Inherits="Vote.LDSDataUpdate.LDSDataUpdate" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
    <title>LDS Data Update</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <user:LoginBar ID="LoginBar1" runat="server" />
      <user:Banner ID="Banner" runat="server" />
      <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table -->
      <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HLarge">
            LDS Data Update</td>
        </tr>
        <tr>
          <td align="left">
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
      </table>
    <!-- Table -->
      <table class="tableAdmin" cellspacing="0" cellpadding="0" border="1">
      <tr>
      <td class="T">
        The following operations should be done in button order. Each operation records
        the version and will not allow you to proceed until the previous operation is completed.
        Each operation may be run and rerun any number of times without harm. And you can
        execute previous operations.</td>
        </tr>
        <tr>
          <td class="T">
            The last LDS Version Started:
            <asp:Label ID="LDSVersion" runat="server" CssClass="TSmallColor"></asp:Label>&nbsp;
            <br />
            If a new version change to this
            LDS Version:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxVersion" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks><br />
          </td>
        </tr>
        <tr>
          <td class="T">
            Last LDS Version 
            Completed:<asp:Label ID="LDSVersionCompleted" runat="server" CssClass="TSmallColor"></asp:Label>Date:
            <asp:Label ID="LDSDateCompleted" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td class="T">
            The last completed Update Date:<asp:Label ID="LastUpdateDate" runat="server" CssClass="TSmallColor"></asp:Label>&nbsp;
            <br />
            Date of this LDS Version's Data (Oldest possible date of data on LDS CD):
            <user:TextBoxWithNormalizedLineBreaks ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
            <br />
            No updates will be made to elected officials (OfficesOfficials) or Politicians where
            this date is older than the date of any update. This protects more recent updates.
            A good date to use is the Date Modified for the LEGIDYY.CSV file. If you want to
            update newer files of the same version increase the Last Update Date by a day or
            two.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonCheckStatesCounties" runat="server" CssClass="Buttons" OnClick="ButtonCheckStatesCounties_Click"
              Text="Check LDS Tables" Width="258px" /><br />
            1) Checks the States and
            Counties Tables for any LDSStateCodes or LDSCounty code that may have changed or is missing in the CENSUS and LDCODE Tables.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateMaster" runat="server" OnClick="ButtonUpdateMaster_Click"
              Text="Update Master Table" Width="263px" CssClass="Buttons" /><br />
            1) Updates the LDS Version number and
            Update Date in the Master Table</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateStates" runat="server" OnClick="ButtonUpdateStates_Click"
              Text="Update States Table" Width="265px" CssClass="Buttons" /><br />
            1) Updates LDSStateCode, LDSStateName,
            LDSVersion rows in the States Table.<br />
            Method: Uses CENSUS Table to update corresponding rows for each State.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateCountiesTable" runat="server" OnClick="ButtonUpdateCountiesTable_Click"
              Text="Update Counties Table" Width="266px" CssClass="Buttons" /><br />
            1) Updates columns LDSStateCode, LDSCountyCode,
            LDSCounty, LDSVersion in the Counties
            Table.
            <br />
            2) Adds any new counties but probably never done because the complete list of
            State Counties are already in the Counties Table.<br />
            Method: Uses CENSUS Table to update corresponding rows in Counties Table</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateOffices" runat="server" OnClick="ButtonUpdateOffices_Click"
              Text="Update Offices Table" Width="263px" CssClass="Buttons" /><br />
            Updates columns
            LDSStateCode,
            LDSTypeCode, LDSDistrictCode, LDSOffice, LDSVersion, LDSUpdateDate in the Offices
            Table<br />
            HH:MM:SS Last Run Time:
            <asp:Label ID="OfficesRunTime" runat="server" CssClass="TSmallColor"></asp:Label>
            Date Completed:
            <asp:Label ID="LabelLDSDateCompletedOffices" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td class="T">
          </td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdatePoliticians" runat="server" OnClick="ButtonUpdatePoliticians_Click"
              Text="Update Politicians Table" Width="259px" CssClass="Buttons" /><br />
            Updates all the LDS... columns in the Politicians Table.<br />
            Method: Process each row in the LEGIDYY Table. &nbsp;Matchs are when the LEG_ID_NUM is same or (not implemented: LName/FName/OfficeKey match). LEGIDYY Rows not found in the Politians Table are
            written to the LEGIDYYNotProcessed Table and reported below. This table needs to be processed by the
            button immediately below. 
            This operation takes considerable time to run.
            Check the time below<br />
            HH:MM:SS Last Run Time:
            <asp:Label ID="PoliticiansRunTime" runat="server" CssClass="TSmallColor"></asp:Label>
            Date Completed:
            <asp:Label ID="LabelLDSDateCompletedPoliticians" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonAddNewPoliticians" runat="server"
              Text="Add New Politicians to Politicians Table" Width="263px" CssClass="Buttons" EnableViewState="False" OnClick="ButtonAddNewPoliticians_Click" /><br />
            This will take
            you to a page where the politicians not processed in the previous operation
            (rows in the LEGIDYYNotProcessed Table) can be
            added manually to the Politicians
            Table. You wll manually select and updte the Politicians Table row belonging to
            the LEG_ID_NUM not processed in the previous step.<br />
            Date Completed:
            <asp:Label ID="LabelLDSNewPoliticiansAdded" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td class="TColor">
            Upon completion rerun the [Update Politicians Table] to insure all politicians have
            a LEG_ID_NUM before performing the next step, i.e. the button below.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateOfficesOfficials" runat="server" OnClick="ButtonUpdateOfficesOfficials_Click"
              Text="Update OfficesOfficials & Politicians Tables" Width="261px" CssClass="Buttons" /><br />
            1)Updates OfficesOfficials Table with the current incumbents if the date is more
            recent. Only Offices Table rows
            for Office Levels 2, 3, 5, 6 and 4 for only Governors are processed. The corresponding
            OfficesOfficials row(s) is updating with the new PoliticianKey and RunningMateKey.<br />
            2) Updates the OfficeKey in Politicians Table reflecting that the politicians are
            incumbents, if the date is more recent.&nbsp;<br />
            HH:MM:SS Last Run Time:<asp:Label ID="OfficesOfficialsRunTime" runat="server" CssClass="TSmallColor"></asp:Label>
            Date Completed:
            <asp:Label ID="LabelLDSDateCompletedOfficesOfficials" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateDCCouncilChairman" runat="server" CssClass="Buttons"
              OnClick="ButtonUpdateDCCouncilChairman_Click" Text="Update DC Council Chairman"
              Width="260px" />
            <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDCCouncilChairman" runat="server" CssClass="TextBoxInput"
              Width="250px"></user:TextBoxWithNormalizedLineBreaks><br />
            The LEGIDYY row for DC Council Chairman is treated as just another member of the
            Council At Large. Enter the PoliticianKey for the DC Council Chairman in the textbox
            provided and click button to correct.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="ButtonUpdateReports" runat="server" OnClick="ButtonUpdateReports_Click"
              Text="Update All the Elected Officials Reports" Width="262px" CssClass="Buttons" /><br />
            This operation takes considerable time to run. Check the time below<br />
            HH:MM:SS Last Run Time:
            <asp:Label ID="ReportsRunTime" runat="server" CssClass="TSmallColor"></asp:Label>
            Date Completed:
            <asp:Label ID="LabelLDSDateCompletedReports" runat="server" CssClass="TSmallColor"></asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="HColor">
            Report</td>
        </tr>
        <tr>
          <td class="TSmall">
            <asp:Label ID="Report" runat="server" CssClass="T" EnableViewState="False"></asp:Label></td>
        </tr>
      </table>
    </div>
    </form>
</body>
</html>
--%>