<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="LDSNewPoliticiansAdd.aspx.cs" Inherits="Vote.LDSNewPoliticiansAdd.LDSNewPoliticiansAdd" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head id="Head1" runat="server">
    <title>LDS Add New Politicians</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <user:LoginBar ID="LoginBar1" runat="server" />
      <user:Banner ID="Banner" runat="server" />
      <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Top -->
      <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HLarge">
            Add New LDS Politicians</td>
        </tr>
        <tr>
          <td align="left" class="HSmall" valign="top">
            Progress (done/to do): &nbsp;
            <asp:Label ID="Done" runat="server" CssClass="HSmallColor" Width="20px"></asp:Label>/
            <asp:Label ID="ToDo" runat="server" CssClass="HSmallColor" Width="20px"></asp:Label></td>
        </tr>
        <tr>
          <td align="left" colspan=2>
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
      </table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
			<tr><td class="T">
            This form process the politicians in the &nbsp;LEGIDYYNotProcessed Table. They are
            the politicians whose LDS Id OR Last Name / First Name / Office does not match any
            in the Politicians Table.<br />
            INSTRUCTIONS:<br />
            -
            If the politician is in the report below click on the <span style="text-decoration: underline">
              <strong>Name Link </strong></span>to update the politician so that he or she will
            have the correct LDS ID and be recorded as being elected to the office shown above.&nbsp;
            <br />
            -
            If the politician shown above is not in the report of politicians with the same
            last name below, click '<strong>ADD NEW Politician</strong>'.<br />
            - If you get this failure message: ''<strong>This name creates a duplicate PoliticianKey</strong>'
            , you need to slightly modify the First Name in text box provided so a unique PoliticianKey
            can be creaged. Then click&nbsp; 'Add
            New Politician' to try again.<br />
            - If you are not sure whether the politician is new or not, Add the politician.<br />
            If you get this Failure message&nbsp; "There is no single oldest OfficesOfficials",
            Copy the message and then click&nbsp; '<strong>Skip Politician</strong>'. You will
            need to fix this problem manually.
            <br />
            <strong>DO NOT Skip</strong> Politician if you get this failure message: 'T<strong>his
              name creates a duplicate PoliticianKey</strong>'. Follow the instructions above.<br />
            You should almost never skip a politician. Never select this button for any other
            reason than that mentioned above</td></tr>
			</table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableUpdate" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
          <td align="left" class="T" colspan="1">
            Party:<asp:Label ID="Party" runat="server" CssClass="HSmallColor"></asp:Label></td>
					<TD align="left" colSpan="2">
            </TD>
					<TD align="left" colSpan="1" class="T">
            Version:<asp:label id="LDSVersion" runat="server" CssClass="Text3"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" class="T">
            First:&nbsp;
            <asp:label id="First" runat="server" CssClass="HSmallColor"></asp:label>
            <br />
            <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFirst" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></TD>
					<TD align="left" class="T">
            Middle:&nbsp;
            <asp:label id="Middle" runat="server" CssClass="HSmallColor"></asp:label><br />
            <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMiddle" runat="server" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></TD>
					<TD align="left" class="T">
            Last:&nbsp;
            <asp:label id="Last" runat="server" CssClass="HSmallColor"></asp:label></TD>
					<TD align="left" class="T">
            Suffix:&nbsp;
            <asp:label id="Suffix" runat="server" CssClass="HSmallColor"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" class="T">
            State:<asp:label id="State" runat="server" CssClass="HSmallColor"></asp:label>&nbsp;</TD>
					<TD align="left" class="T">
            Elected to
            Office:<asp:label id="Office" runat="server" CssClass="TSmallColor"></asp:label></TD>
					<TD align="left" class="T">
            District:<asp:label id="District" runat="server" CssClass="HSmallColor"></asp:label></TD>
					<TD align="left" class="T">
            LDS ID:<asp:label id="LDSId" runat="server" CssClass="Text3"></asp:label></TD>
				</TR>
        <tr>
          <td align="left" colspan="1" class="T">
            District Address:</td>
          <td align="left" colspan="3" class="T">
            Capital Address</td>
        </tr>
				<TR>
					<TD align="left" colSpan="1" class="T">
						<asp:label id="DistrictAddress" runat="server" CssClass="TSmallColor"></asp:label></TD>
					<TD align="left" colSpan="3" class="T">
						<asp:label id="CapitalAddress" runat="server" CssClass="TSmallColor"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="1" class="T">
						<asp:label id="DistrictCityStateZip" runat="server" CssClass="TSmallColor"></asp:label></TD>
					<TD align="left" colSpan="3" class="T">
						<asp:label id="CapitalCityStateZip" runat="server" CssClass="TSmallColor"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2" class="T">
            District Phone:<asp:label id="DistrictPhone" runat="server" CssClass="TSmallColor"></asp:label>&nbsp;</TD>
					<TD align="left" colSpan="2" class="T">
            Capitol Phone:<asp:label id="CapitalPhone" runat="server" CssClass="TSmallColor"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2" class="T">
            Email:<asp:label id="Email" runat="server" CssClass="TSmallColor"></asp:label>&nbsp;</TD>
					<TD align="left" colSpan="2" class="T">
            Web:<asp:label id="Web" runat="server" CssClass="TSmallColor"></asp:label></TD>
				</TR>
			</TABLE>
    <!-- TableSameLastName -->
			<TABLE class="tableAdmin" id="TableSameLastName" cellSpacing="0" cellPadding="0" border="1"
				runat="server">
				<TR>
					<TD align="left" class="T">
            <asp:Button ID="ButtonAddPolitician" runat="server" Text="ADD NEW Politician" Width="185px" OnClick="ButtonAddPolitician_Click" CssClass="Buttons" /></td>
					<TD align="left" width="800" class="T">
            <asp:Button ID="ButtonSkipPolitician" runat="server" OnClick="ButtonSkipPolitician_Click"
              Text="Skip Politician" Width="183px" CssClass="Buttons" /></TD>
				</TR>
			</TABLE>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableLastNameReport" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<TD colSpan="2" class="HSmallColor">
            Politicians with Same Last Name in this State</TD>
				</tr>
				<TR>
					<TD align="center" colSpan="2" class="T"><asp:label id="SameLastNameHTMLTable" runat="server"></asp:label></TD>
				</TR>
			</TABLE>
    
    </div>
    </form>
</body>
</html>
--%>