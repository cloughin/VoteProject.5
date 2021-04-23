<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="MgtReports.aspx.cs" Inherits="Vote.Master.MgtReports" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
		<title>Management Reports</title>
		<LINK href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Archives.css" type="text/css" rel="stylesheet" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  </HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableHeading" cellSpacing="0" cellPadding="0">
				<tr>
					<td align="left" class="HLarge">
						<asp:label id="PageTitle" runat="server" EnableViewState="False"></asp:label>
					</td>
				</tr>
				<TR>
					<TD>
						<asp:label id="Msg" runat="server" EnableViewState="False" ></asp:label></TD>
				</TR>
			</TABLE>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableReport" cellSpacing="0" cellPadding="0">
				<TR>
					<td class="T">
						<asp:Label id="LabelReport" runat="server" CssClass="TSmall" EnableViewState="False"></asp:Label></td>
				</TR>
			</TABLE>
    <!-- Table -->
    <asp:Label ID="LabelHTMLTableReport" runat="server" EnableViewState="False" ></asp:Label>
    <!-- Table -->
		</form>
	</body>
</HTML>
