<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="NotesRecord.aspx.cs" Inherits="Vote.Master.NotesRecord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register TagPrefix="uc2"  TagName="Navbar"Src="../NavbarAdmin.ascx" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
		<title>Record Notes</title>
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
      <uc2:Navbar ID="Navbar" runat="server"/>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableButtonsTop" cellSpacing="0" cellPadding="0">
				<TR>
					<TD vAlign="middle" align="left" class="HLarge"><asp:label id="PageTitle" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left"><asp:label id="Msg" runat="server" ></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left" class="T">
            &nbsp;Use this form to record information, actions and communications with this
            State's Board of Elections.</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="center"><asp:hyperlink id="HyperLinkViewNotes" runat="server" CssClass="HyperLink" NavigateUrl="/Master/NotesView.aspx"
							Target="view" >View All Notes</asp:hyperlink></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left" class="T">
            Last Note: Below is the last note recorded. Click the View All Notes Button above
            if you want to view all notes.&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left"><asp:label id="LabelLastNote" runat="server" CssClass="TSmall"></asp:label></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="center" class="H">
            Record New Note</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left" class="T">
            Use the text box below to record a new note. Then click the Record Button.</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left"><user:TextBoxWithNormalizedLineBreaks id="TextBoxNote" runat="server" TextMode="MultiLine"
							Rows="15" CssClass="TextBoxInputMultiLine" Width="689px"></user:TextBoxWithNormalizedLineBreaks></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="center"><asp:button id="ButtonRecordInfo" runat="server" CssClass="Buttons" Text="Record Note" Width="123px"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
--%>
