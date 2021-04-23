<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="DesignDefaultsElectionPage.aspx.cs"
  Inherits="Vote.Master.DesignDefaultsElectionPage" %>

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
  <title>Election.aspx Design</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link href="/css/Election.css" type="text/css" rel="stylesheet" />
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
          DEFAULT Design for <span style="color: red">Election.aspx</span></td>
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
        <td align="center" class="H" colspan="3" valign="middle">
          Samples of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view">Sample Page 1</asp:HyperLink>&nbsp;
        </td>
        <td class="T" colspan="1" valign="middle">
          <asp:HyperLink ID="HyperLinkSamplePage2" runat="server" CssClass="HyperLink"
            Target="view">Sample Page 2</asp:HyperLink></td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample report page.</td>
      </tr>
    </table>
    <!-- Table -->
    <uc7:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table27" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Page Title</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionName"&gt;&lt;td class="tdElectionName"&gt;Virginia&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionTitle"&gt;&lt;td class="tdElectionTitle"&gt;Directory
          of ...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionElection"&gt;&lt;td class="tdElectionElection"&gt;February
          12, 2008...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0"
      border="0">
      <tr class="trElectionName">
        <td class="tdElectionName">
          <span id="LabelTitle">Virginia</span></td>
      </tr>
      <tr class="trElectionTitle">
        <td class="tdElectionTitle">
          <span id="LabelDirectory">Directory of Offices, Candidates and Ballot Measures</span></td>
      </tr>
      <tr class="trElectionElection">
        <td class="tdElectionElection">
          <span id="LabelElection">February 12, 2008 Virginia Democratic Presidential Primary</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table28" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Election Information</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionElectionInfo"&gt;&lt;td class="tdElectionElectionInfo"&gt;The
          Democratic...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0"
      cellpadding="0" border="0">
      <tr class="trElectionElectionInfo">
        <td class="tdElectionElectionInfo">
          <span id="ElectionElectionInfo">The Democratic Presidential candidates still in
            the race at this time include Barack Obama and Hillary Clinton. John Edwards, Joe
            Biden, and Dennis Kucinich, have withdrawn from the race, but will still appear
            on ballots. Mike Gravel, also on ballots, has not withdrawn but can not win the
            nomination.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table29" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Election Results</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionResultsHead"&gt;&lt;td class="tdElectionResultsHead"&gt;Results&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionResultsImage"&gt;&lt;td class="tdElectionResultsImage"&gt;&lt;img
          class="ResultsImage" /&gt;&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectionResultsSource"&gt;&lt;td class="tdElectionResultsSource"&gt;Source:&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0"
      border="0">
      <tr class="trElectionResultsHead">
        <td class="tdElectionResultsHead">
          Results</td>
      </tr>
      <tr class="trElectionResultsImage">
        <td class="tdElectionResultsImage">
          <img id="ResultsImage" class="ResultsImage" src="/images/Elections/20080212BVA000000VAD.jpg"
            border="0" /></td>
      </tr>
      <tr class="trElectionResultsSource">
        <td class="tdElectionResultsSource">
          Source:<span id="ElectionResultsSource">politics.nytimes.com/election-guide/2008/results/states/VA.html</span>
          <span id="ElectionResultsDate">02/12/2008</span></td>
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
          &lt;tr class="trElectionInstruction"&gt;&lt;td class="tdElectionInstruction"&gt;This
          is a directory...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0"
      border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <span id="InstructionsPage">This is a directory of the Virginia, Directory of Offices,
            Candidates and Ballot Measures, February 12, 2008 Virginia Democratic Presidential
            Primary. Click an <strong><u>Office Title</u></strong> to obtain the positions and
            views that the candidates provided. Not all candidates have responded. Click a <strong>
              <u>Candidate Name</u></strong> to obtain candidate information and a picture,
            if these were provided. If there are ballot measures in this election, click a <strong>
              <u>Referendum Title</u></strong> link(s) to obtain more information about the
            ballot measure. Time and resources permitting, we will include county and local
            office contests sometime before the election. So check back with us later.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for US Presidential Primary Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table5" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSPresPrimary" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSPresPrimary"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSPresPrimary_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table15" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSPresPrimary" runat="server"
            TextMode="MultiLine" Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageUSPresPrimary"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageUSPresPrimary_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for US Presidential and Vice President Election Report
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table31" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSPres" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table16" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSPres"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSPres_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml2" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table17" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSPres" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageUSPres" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageUSPres_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          US Senate Election Report </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table3" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSSenate" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSSenate"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSSenate_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml3" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSSenate" runat="server"
            TextMode="MultiLine" Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageUSSenate" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageUSSenate_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table8" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          US House Election Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table7" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSHouse" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table20" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSHouse"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSHouse_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml4" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table21" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSHouse" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageUSHouse" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageUSHouse_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table10" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          State Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table9" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageState" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table22" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageState"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageState_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml5" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table23" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageState" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageState" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageState_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table12" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          County Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table11" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageCounty" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table24" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageCounty"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageCounty_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml6" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table25" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageCounty" runat="server" CssClass="TextBoxInputMultiLine"
            Height="100px" Rows="1" TextMode="MultiLine" Width="930px">
          </user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageCounty" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" OnClick="ButtonSubmitDefaultInstructionsElectionPageCounty_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table32" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for Local District Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table33" class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageLocal" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table34" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageLocal"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageLocal_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml7" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table35" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageLocal" runat="server" CssClass="TextBoxInputMultiLine" Height="100px"
            Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElectionPageLocal" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="160px" 
            OnClick="ButtonSubmitDefaultInstructionsElectionPageLocal_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <!-- Table -->
    <!-- Table -->
    <table id="Table13" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Report of Candidates on Ballots</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;table id="Report"&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportGroupHeading"&gt;&lt;td class="tdReportGroupHeading"&gt;U.S.
          President...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportIncumbentKeyHeading"&gt;&lt;td class="tdReportIncumbentKeyHeading"&gt;*
          = Incumbent&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetailHeading"&gt;&lt;td class="tdReportDetailHeading" align="center"&gt;Order&lt;/td&gt;&lt;/tr&gt;...</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetail"&gt;&lt;td class="tdReportDetail" &gt;Barack Obama&lt;/td&gt;&lt;/tr&gt;...</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
  <table cellspacing="0" cellpadding="0" border="0" class="tableReport">
    <tr class="trReportGroupHeading">
      <td class="tdReportGroupHeading" align="center" colspan="6">
        <a href="/Issue.aspx?State=VA&Issue=ALLPersonal&Office=USPresident&Election=20080212BVA000000VAD"
          title=" Side-By-Side Comparisons of the Candidates on the Issues for U.S. President Vice President Democratic Party on U.S. Ballots"
          target="_self">U.S. President Vice President Democratic Party<br>
          Side-By-Side Comparisons of the Candidates on the Issues</a></td>
    </tr>
    <tr class="trReportIncumbentKeyHeading">
      <td class="tdReportIncumbentKeyHeading" align="left" colspan="6">
        * = Incumbent</td>
    </tr>
    <tr class="trReportDetailHeading">
      <td class="tdReportDetailHeading" align="center">
        Order</td>
      <td class="tdReportDetailHeading" align="center">
        Name</td>
      <td class="tdReportDetailHeading" align="center">
        Party<br>
        Code</td>
      <td class="tdReportDetailHeading" align="center">
        Phone</td>
      <td class="tdReportDetailHeading" align="center">
        Street Address<br>
        <nobr>
          City, State Zip</nobr></td>
      <td class="tdReportDetailHeading" align="center">
        Email Address<br>
        Web Address</td>
    </tr>
    <tr class="trReportDetail">
      <td class="tdReportDetail">
        &nbsp</td>
      <td class="tdReportDetail">
        <nobr>
          <a href="<% =UrlManager.GetIntroPageUri("ILObamaBarack") %>" title="Barack Obama" target="_self"><span
            class="class">Barack Obama</span></a></nobr></td>
      <td class="tdReportDetail" align="center">
        <a href="http://www.democrats.org/" title="Democratic Party Website" target="_self">
          D</a></td>
      <td class="tdReportDetail" align="center">
        <nobr>
          312.886.3506</nobr></td>
      <td class="tdReportDetail">
        P.O. Box 8102<br>
        <nobr>
          Chicago, IL 60680</nobr></td>
      <td class="tdReportDetail">
        <a href="mailto:info@barackobama.com">info@barackobama.com</a><br>
        <a href="http://my.barackobama.com" title="Barack Obama Website" target="_self">my.barackobama.com</a></td>
    </tr>
    <tr class="trReportRunningMate">
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        N/A<br>
        (Running Mate)</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
    </tr>
    <tr class="trReportDetail">
      <td class="tdReportDetail">
        &nbsp</td>
      <td class="tdReportDetail">
        <nobr>
          <a href="<% =UrlManager.GetIntroPageUri("OHKucinichDennisJ") %>" title="Dennis J. Kucinich" target="_self">
            <span class="class">Dennis J. Kucinich</span></a></nobr></td>
      <td class="tdReportDetail" align="center">
        <a href="http://www.democrats.org/" title="Democratic Party Website" target="_self">
          D</a></td>
      <td class="tdReportDetail" align="center">
        <nobr>
          216.228.8850</nobr></td>
      <td class="tdReportDetail">
        4262 Kennebec Road<br>
        <nobr>
          Dixmont, ME 04932</nobr></td>
      <td class="tdReportDetail">
        <a href="mailto:info@dennis4president.com">info@dennis4president.com</a><br>
        <a href="http://www.dennis4president.com" title="Dennis J. Kucinich Website" target="_self">
          www.dennis4president.com</a></td>
    </tr>
    <tr class="trReportRunningMate">
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        N/A<br>
        (Running Mate)</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
    </tr>
    <tr class="trReportDetail">
      <td class="tdReportDetail">
        &nbsp</td>
      <td class="tdReportDetail">
        <nobr>
          <a href="<% =UrlManager.GetIntroPageUri("NYClintonHillaryRodham") %>" title="Hillary Clinton" target="_self">
            <span class="class">Hillary Clinton</span></a></nobr></td>
      <td class="tdReportDetail" align="center">
        <a href="http://www.democrats.org/" title="Democratic Party Website" target="_self">
          D</a></td>
      <td class="tdReportDetail" align="center">
        <nobr>
          703-469-2008</nobr></td>
      <td class="tdReportDetail">
        4420 North Fairfax Drive<br>
        <nobr>
          Arlington, VA 22203</nobr></td>
      <td class="tdReportDetail">
        <a href="http://www.hillaryclinton.com/help/contact">www.hillaryclinton.com/help/contact</a><br>
        <a href="http://www.hillaryclinton.com" title="Hillary Clinton Website" target="_self">
          www.hillaryclinton.com</a></td>
    </tr>
    <tr class="trReportRunningMate">
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        N/A<br>
        (Running Mate)</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
      <td class="tdReportDetail" align="left" colspan="1">
        &nbsp</td>
    </tr>
  </table>
    <!-- Table -->
    <uc5:DesignDefaultsStyleSheet ID="DesignDefaultsStyleSheet1" runat="server" />
    <table id="Table14" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H">
          Default Style Sheet(/css/Election.css)</td>
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