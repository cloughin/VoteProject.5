<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="DesignDefaultsElectedOfficialsPage.aspx.cs"
  Inherits="Vote.Master.DesignDefaultsElectedOfficialsCountiesPage" %>

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
  <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
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
          DEFAULT Design for <span style="color: red">Officials.aspx</span></td>
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
            Target="view" >Sample Page</asp:HyperLink>&nbsp;
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
          &lt;tr class="trOfficialsName"&gt;&lt;td class="tdOfficialsName"&gt;Federal
          and Virginia&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trOfficialsTitle"&gt;&lt;td class="tdOfficialsTitle"&gt;Directory
          of ...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0"
      border="0">
      <tr class="trOfficialsName">
        <td class="tdOfficialsName">
          <span id="LabelTitle">Federal and Virginia</span></td>
      </tr>
      <tr class="trOfficialsTitle">
        <td class="tdOfficialsTitle">
          <span id="LabelDirectory">Directory of Currently Elected Office Holders</span></td>
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
          &lt;tr class="trOfficialsInstruction"&gt;&lt;td class="tdOfficialsInstruction"&gt;This
          is a...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0"
      border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <span id="InstructionsPage">This is a Federal and Virginia, Directory of Currently
            Elected Office Holders provided by <a href="http://vote-usa.org/">Vote-USA</a>.
            Use the name links to obtain contact information, biographical information, pictures,
            and the views and positions on various issues of these Virginia Elected Officials.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          US Presidential and Vice President Report
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table3" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSPres" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSPres"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSPres_SelectedIndexChanged">
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
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSPres" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageUSPres"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="169px" OnClick="ButtonSubmitDefaultInstructionsOfficialsPageUSPres_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          US Senate Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table7" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSSenate" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table14" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSSenate"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSSenate_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" colspan="1" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml2" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table15" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSSenate" runat="server"
            TextMode="MultiLine" Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageUSSenate"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="167px" OnClick="ButtonSubmitDefaultInstructionsOfficialsPageUSSenate_Click" />&nbsp;
        </td>
      </tr>
    </table>

    <!-- Table -->
    <table class="tableAdmin" id="Table8" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          US House Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table9" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSHouse" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table16" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSHouse"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSHouse_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml3" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table17" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSHouse" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageUSHouse"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="170px" 
            OnClick="ButtonSubmitDefaultInstructionsOfficialsPageUSHouse_Click" />&nbsp;
        </td>
      </tr>
    </table>


    <!-- Table -->
    <table class="tableAdmin" id="Table29" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          State Governors Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table31" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageGovernors" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table32" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageGovernors"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageGovernors_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml7" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table33" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageGovernors" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageGovernors"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="170px" 
            OnClick="ButtonSubmitDefaultInstructionsOfficialsPageGovernors_Click" />&nbsp;
        </td>
      </tr>
    </table>

    <!-- Table -->

    <table class="tableAdmin" id="Table10" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          State Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table11" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageState" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageState"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageState_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml4" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageState" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageState" runat="server"
            Text="Update Default Design" CssClass="Buttons" Width="165px" OnClick="ButtonSubmitDefaultInstructionsOfficialsPageState_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table12" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          County Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="table13" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageCounty" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table20" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageCounty"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageCounty_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml5" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table21" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageCounty" runat="server" TextMode="MultiLine"
            Rows="1" Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageCounty"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="168px" OnClick="ButtonSubmitDefaultInstructionsOfficialsPageCounty_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table --><table class="tableAdmin" id="Table23" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Local District Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table24" cellspacing="0" cellpadding="0" border="0">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageLocal" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table25" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageLocal"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" RepeatDirection="Horizontal"
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageLocal_SelectedIndexChanged">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml6" runat="server"/>
        </td>
      </tr>
    </table>
    <!-- Table --><table class="tableAdmin" id="Table28" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageLocal" runat="server" CssClass="TextBoxInputMultiLine" Height="100px"
            Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsOfficialsPageLocal"
            runat="server" Text="Update Default Design" CssClass="Buttons" Width="168px" 
            OnClick="ButtonSubmitDefaultInstructionsOfficialsPageLocal_Click" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <!-- Table -->
    <table id="Table22" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Report of Elected Officials</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;table id="Report"&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportGroupHeading"&gt;&lt;td class="tdReportGroupHeading"&gt;U.S.
          President Vice President&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetailHeading"&gt;&lt;td class="tdReportDetailHeading" align="center"&gt;Order&lt;/td&gt;&lt;/tr&gt;...</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trReportDetail"&gt;&lt;td class="tdReportDetail" &gt;George W. Bush&lt;/td&gt;&lt;/tr&gt;...</td>
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
          U.S. President Vice President</td>
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
            <a href="<% =UrlManager.GetIntroPageUri("TXBushGeorgeW") %>" title="George W. Bush" target="_self"><span
              class="class">George W. Bush</span></a></nobr></td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.rnc.org/" title="Republican Party Website" target="_self">R</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            703-647-2700</nobr></td>
        <td class="tdReportDetail">
          43 Prairie Chapel Ranch<br>
          <nobr>
            Crawford TX</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br>
          <a href="http://www.georgewbush.com" title="George W. Bush Website" target="_self">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportRunningMate">
        <td class="tdReportDetail">
          &nbsp</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("WYCheneyRichardB") %>" title="Richard B. Dick Cheney" target="_self">
              <span class="class">Richard B. "Dick" Cheney</span></a></nobr><br>
          (Running Mate)</td>
        <td class="tdReportDetail" align="center">
          &nbsp</td>
        <td class="tdReportDetail" align="center">
          <nobr>
            N/A</nobr></td>
        <td class="tdReportDetail">
          4205 West Greens Pl.<br>
          <nobr>
            Wilson WY 83014</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br>
          <a href="http://www.georgewbush.com" title="Richard B. Dick Cheney Website" target="_self">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportGroupHeading">
        <td class="tdReportGroupHeading" align="center" colspan="6">
          U.S. Senate Virginia</td>
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
            <a href="<% =UrlManager.GetIntroPageUri("VAWarnerJohnW") %>" title="John W. Warner" target="_self"><span
              class="class">John W. Warner</span></a></nobr></td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.rpv.org/" title="Republican Party of Virginia Website" target="_self">
            R</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            804.739.0247</nobr></td>
        <td class="tdReportDetail">
          5309 Commonwealth Centre Parkway<br>
          <nobr>
            Midlothian, VA 23112</nobr></td>
        <td class="tdReportDetail">
          <a href="http://warner.senate.gov/public/index.cfm?">warner.senate.gov/public/index.cfm?</a><br>
          <a href="http://warner.senate.gov/" title="John W. Warner Website" target="_self">
            warner.senate.gov/</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VAWebbJamesH") %>" title="James H. (Jim) Webb" target="_self"><span
              class="class">James H. (Jim) Webb</span></a></nobr></td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.vademocrats.org/" title="Democratic Party of Virginia Website"
            target="_self">D</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            804.771.2221</nobr></td>
        <td class="tdReportDetail">
          PO Box 17427<br>
          <nobr>
            Arlington, VA 22216</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:infor@webbforsenate.com">infor@webbforsenate.com</a><br>
          <a href="http://webbforsenate.com" title="James H. (Jim) Webb Website" target="_self">
            webbforsenate.com</a></td>
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