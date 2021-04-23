<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="DesignDefaultsArchivesPage.aspx.cs"
  Inherits="Vote.Master.DesignDefaultsArchivesPage" %>

<html><head><title></title></head><body></body></html>


<%--
<%@ Import Namespace="Vote" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc8" %>
<%@ Register Src="Instructions/DesignInstructionsStyleSheets.ascx" TagName="DesignInstructionsStyleSheets"
  TagPrefix="uc9" %>
<%@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements"
  TagPrefix="uc7" %>
<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml"
  TagPrefix="uc6" %>
<%@ Register Src="Instructions/DesignDefaultsStyleSheet.ascx" TagName="DesignDefaultsStyleSheet"
  TagPrefix="uc5" %>
<%@ Register Src="Instructions/DesignInstructionsControlsDefault.ascx" TagName="DesignInstructionsControlsDefault"
  TagPrefix="user" %>
<%@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="uc4" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Officials.aspx Design</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link href="/css/Archives.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body>
  <form id="form1" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table1 -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          DEFAULT Design for <span style="color: red">Archives.aspx</span></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <uc8:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
    <!-- Table -->
    <uc9:DesignInstructionsStyleSheets ID="DesignInstructionsStyleSheets1" runat="server" />
    <!-- Table -->
    <user:DesignInstructionsControlsDefault ID="DesignInstructionsControlsDefault1" runat="server" />
    <!-- Table -->
    <uc4:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
    <!-- Table -->
    <table id="Table26" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2" valign="middle">
          Sample of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view">Sample Page</asp:HyperLink>&nbsp;
        </td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample report page.</td>
      </tr>
    </table>
    <uc7:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table27" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Page Title</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trArchivesTitle"&gt;&lt;td class="tdArchivesTitle"&gt;Federal and
          Virginia&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trArchivesTitle">
        <td class="tdArchivesTitle">
          Archives of Federal and Virginia Elections</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table30" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2" valign="top">
          Report Instructions</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trArchivesInstruction"&gt;&lt;td class="tdArchivesInstruction"&gt;This
          is a...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trArchivesInstruction">
        <td class="tdArchivesInstruction">
          These are some of the elections in our archives. Click on an <strong><u>Election Description</u></strong>
          Link to obtain a report of the information we have on that election.</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for Archives Page&nbsp;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table3" cellspacing="0" cellpadding="0" border="0">
      <tr class="trArchivesInstruction">
        <td class="tdArchivesInstruction">
          <asp:Label ID="LabelInstructionsArchivesPage" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsArchivesPage" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonListIsTextInstructionsArchivesPage_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsArchivesPage" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsArchivesPage" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="169px" OnClick="ButtonSubmitDefaultInstructionsArchivesPage_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <!-- Table -->
    <table id="Table22" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Report of Archives</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;table&gt;&lt;tr class="trReportGroupHeading"&gt;&lt;td class="tdReportGroupHeading"&gt;Elections
          for U.S. President&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetailHeading"&gt;&lt;td class="tdReportDetailHeading"&gt;Date&lt;/td&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;td class="tdReportDetailHeading"&gt;Election Description&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetail"&gt;&lt;td class="tdReportDetail" &gt;11/04/2008&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdReportDetail" &gt;Nov 2008 US President Elections&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellspacing="0" cellpadding="0" border="0" class="tableAdmin">
      <tr class="trReportGroupHeading">
        <td class="tdReportGroupHeading" align="center" colspan="2">
          Elections for U.S. President</td>
      </tr>
      <tr class="trReportDetailHeading">
        <td class="tdReportDetailHeading">
          Date</td>
        <td class="tdReportDetailHeading">
          Election Description</td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          11/04/2008</td>
        <td class="tdReportDetail">
          <a href="<% =UrlManager.GetElectionPageUri("VA20101102GA").ToString() %>">Nov 2008 US
            President Elections</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          11/04/2008</td>
        <td class="tdReportDetail">
          <a href="<% =UrlManager.GetElectionPageUri("VA20101102GA").ToString() %>">2008 McCain
            vs Obama</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          11/02/2004</td>
        <td class="tdReportDetail">
          <a href="<% =UrlManager.GetElectionPageUri("VA20101102GA").ToString() %>">Nov 2004 US
            President Elections</a></td>
      </tr>
    </table>
    <!-- Table -->
    <uc5:DesignDefaultsStyleSheet ID="DesignDefaultsStyleSheet1" runat="server" />
    <!-- Table -->
    <table id="Table5" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H">
          Default Style Sheet(/css/Officials.css)</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="ButtonUpdateStyleSheet" runat="server" CssClass="Buttons" OnClick="ButtonUpdateStyleSheet_Click"
            Text="Update Style Sheet" Width="150px" /></td>
      </tr>
      <tr>
        <td align="center" class="BorderLight">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStyleSheet" runat="server" CssClass="TextBoxInputMultiLine" Height="3000px"
            TextMode="MultiLine" Width="450px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>