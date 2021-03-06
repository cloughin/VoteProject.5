<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="WordTextFile4Letters.aspx.cs" Inherits="Vote.Master.CandidateInfo4Letters" %>

<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
		<title>Candidate Info Letters</title>
		<LINK href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
      <user:Banner ID="Banner" runat="server" />
    <!-- Table -->
			<TABLE class="tableAdmin" id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD class="HLarge">
						<P align="center">
              Text File for Word Generated Letters</P>
					</TD>
				</TR>
				<TR>
					<TD ><asp:label id="Msg" runat="server" ></asp:label></TD>
				</TR>
        <tr>
          <td class="T" >
            Clicking the button will create a tab delimited text file in the Master Folder to be used
            as input to a Word template to create letters and labels. The data elements
            are the names of the merge fields in the Word templates.</td>
        </tr>
				</Table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableCandidates" cellSpacing="1" cellPadding="1" width="100%" border="0" runat=server>
				<TR>
          <td colspan="1" class="T">
						<asp:button id="ButtonMakeFile" runat="server" CssClass="Buttons" Text="Candidates in This Election" OnClick="ButtonMakeFile_Click1" Width="214px"></asp:button>
          </td>
					<TD class="T" >
            The file created is named <b>CandidateDataXX.txt</b>
            (where XX is the StateCode)
            containing the candidates in this election. The data elements are: Name, Office,
            Candidate Address, CityStateZip, Username, Password, State, Election Title</TD>
				</TR>
				</Table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TablePoliticalParties" cellSpacing="1" cellPadding="1" width="100%" border="0" runat=server>
				<TR align="center">
          <td>
            <asp:Button ID="ButtonPoliticalParties" runat="server" CssClass="Buttons" OnClick="ButtonPoliticalParties_Click"
              Text="All Political Parties" Width="210px" /></td>
					<TD align="left" valign="top" class="T">
            The file created is named <strong>PartyAddresses.txt. </strong>The data elements
            are: Party Name, Party Address, CityStateZip.</TD>
				</TR>
				</Table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableStateAuthorities" cellSpacing="1" cellPadding="1" width="100%" border="0" runat=server>
				<TR align="center">
          <td>
            &nbsp;<asp:Button ID="ButtonStateAuthorities" runat="server" CssClass="Buttons" OnClick="ButtonStateAuthorities_Click"
              Text="Best Single Contact in All State Authorities" Width="270px" /></td>
					<TD align="left" valign="top" class="T">
            The file created is named <strong>StateAuthorities.txt. </strong>The data elements
            are: StateCode, State, Name, ElectionAuthority, AddressLine1, AddressLine2, AddressLine3.</TD>
				</TR>
				</Table>
    <!-- Table -->
			<TABLE class="tableAdmin" id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD class="T">
						<P align="center">
							<asp:label id="LabelCandidateDataTable" runat="server"></asp:label></P>
					</TD>
				</TR>
			</TABLE>
		</form>
		<P></P>
	</body>
</HTML>
-->
--%>