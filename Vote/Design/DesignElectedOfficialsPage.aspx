<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignElectedOfficialsPage.aspx.cs" Inherits="Vote.Admin.DesignElectedOfficialsCountiesPage" %>

<html><head><title></title></head><body></body></html>
<%-- 
<%@ Import Namespace="Vote" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc7" %>
<%@ Register Src="Instructions/DesignInstructionsStyleSheets.ascx" TagName="DesignInstructionsStyleSheets"
  TagPrefix="uc8" %>
<%@ Register Src="Instructions/DesignInstructionsControls.ascx" TagName="DesignInstructionsControls"
  TagPrefix="uc9" %>

<%@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements" TagPrefix="uc6" %>

<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml" TagPrefix="uc5" %>

<%@ Register Src="Instructions/CustomStyleSheet.ascx" TagName="CustomStyleSheet"
  TagPrefix="user" %>

<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>

<%@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="uc4" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Design for Officials.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
  <link id="This" type="text/css" rel="stylesheet" runat="server" />
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
          Design for <span style="color: red">Officials.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" ForeColor="Red"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Elected Officials Page (Officials.aspx).</td>
      </tr>
    </table>
    <!-- Table -->
    <uc7:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
    <!-- Table -->
    <uc8:DesignInstructionsStyleSheets id="DesignInstructionsStyleSheets1" runat="server" />
    <!-- Table -->
    <uc9:DesignInstructionsControls ID="DesignInstructionsControls1" runat="server" />
    <!-- Table -->
    <uc4:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
    
    
    <!-- Table -->
    <table id="Table24" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2" valign="middle">
          Sample of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view" >Sample Page</asp:HyperLink>
        </td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample ballot page.</td>
      </tr>
    </table>
    &nbsp;
    <!-- Table -->
    <uc6:DesignPageElements ID="DesignPageElements1" runat="server" />
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <span id="InstructionsPage">This is a Federal and Virginia, Directory of Currently
            Elected Office Holders provided by <a href="http://vote-usa.org/">Vote-USA</a>.
            Use the name links to obtain contact information, biographical information, pictures,
            and the views and positions on various issues of these Virginia Elected Officials.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table25" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          US Presidential and Vice President Report
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table3" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSPres" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSPres" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSPres_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSPres" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageUSPres" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageUSPres_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageUSPres" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageUSPres_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageUSPres" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageUSPres_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table4" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          US Senate Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table7" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSSenate" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table23" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSSenate" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSSenate_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml2" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSSenate" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageUSSenate" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageUSSenate_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageUSSenate" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageUSSenate_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageUSSenate" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustom1_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table8" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          US House Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table9" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageUSHouse" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table14" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageUSHouse" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageUSHouse_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml3" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table15" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageUSHouse" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageUSHouse" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageUSHouse_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageUSHouse" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageUSHouse_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageUSHouse" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageUSHouse_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table32" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          State Governors Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table33" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageGovernors" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table34" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageGovernors" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageGovernors_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml7" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table35" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageGovernors" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageGovernors" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageGovernors_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageGovernors" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageGovernors_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageGovernors" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageGovernors_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table10" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          State Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table11" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageState" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table16" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageState" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageState_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml4" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table17" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageState" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageState" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageState_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageState" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageState_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageState" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageState_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table12" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          County Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table13" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageCounty" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageCounty" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageCounty_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml5" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table21" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageCounty" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine" ></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageCounty" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageCounty_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageCounty" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageCounty_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageCounty" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageCounty_Click" /></td>
      </tr>
    </table>
    <!-- Table --><table id="Table26" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Local District Report Instructions</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table28" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trOfficialsInstruction">
        <td class="tdOfficialsInstruction">
          <asp:Label ID="LabelInstructionsOfficialsPageLocal" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table29" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsOfficialsPageLocal" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsOfficialsPageLocal_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml6" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table31" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsOfficialsPageLocal" runat="server" CssClass="TextBoxInputMultiLine" Height="100px"
            Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsOfficialsPageLocal" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsOfficialsPageLocal_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsOfficialsPageLocal" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsOfficialsPageLocal_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsOfficialsPageLocal" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsOfficialsPageLocal_Click" /></td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0" class="tableReport">
      <tr class="trReportGroupHeading">
        <td align="center" class="tdReportGroupHeading" colspan="6">
          U.S. President Vice President</td>
      </tr>
      <tr class="trReportDetailHeading">
        <td align="center" class="tdReportDetailHeading">
          Order</td>
        <td align="center" class="tdReportDetailHeading">
          Name</td>
        <td align="center" class="tdReportDetailHeading">
          Party<br />
          Code</td>
        <td align="center" class="tdReportDetailHeading">
          Phone</td>
        <td align="center" class="tdReportDetailHeading">
          Street Address<br />
          <nobr>
            City, State Zip</nobr></td>
        <td align="center" class="tdReportDetailHeading">
          Email Address<br />
          Web Address</td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("TXBushGeorgeW") %>" target="_self" title="George W. Bush"><span
              class="class">George W. Bush</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.rnc.org/" target="_self" title="Republican Party Website">R</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            703-647-2700</nobr></td>
        <td class="tdReportDetail">
          43 Prairie Chapel Ranch<br />
          <nobr>
            Crawford TX</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br />
          <a href="http://www.georgewbush.com" target="_self" title="George W. Bush Website">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportRunningMate">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("WYCheneyRichardB") %>" target="_self" title="Richard B. Dick Cheney">
              <span class="class">Richard B. "Dick" Cheney</span></a></nobr><br />
          (Running Mate)</td>
        <td align="center" class="tdReportDetail">
          &nbsp;</td>
        <td align="center" class="tdReportDetail">
          <nobr>
            N/A</nobr></td>
        <td class="tdReportDetail">
          4205 West Greens Pl.<br />
          <nobr>
            Wilson WY 83014</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br />
          <a href="http://www.georgewbush.com" target="_self" title="Richard B. Dick Cheney Website">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportGroupHeading">
        <td align="center" class="tdReportGroupHeading" colspan="6">
          U.S. Senate Virginia</td>
      </tr>
      <tr class="trReportDetailHeading">
        <td align="center" class="tdReportDetailHeading">
          Order</td>
        <td align="center" class="tdReportDetailHeading">
          Name</td>
        <td align="center" class="tdReportDetailHeading">
          Party<br />
          Code</td>
        <td align="center" class="tdReportDetailHeading">
          Phone</td>
        <td align="center" class="tdReportDetailHeading">
          Street Address<br />
          <nobr>
            City, State Zip</nobr></td>
        <td align="center" class="tdReportDetailHeading">
          Email Address<br />
          Web Address</td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VAWarnerJohnW") %>" target="_self" title="John W. Warner"><span
              class="class">John W. Warner</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.rpv.org/" target="_self" title="Republican Party of Virginia Website">
            R</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            804.739.0247</nobr></td>
        <td class="tdReportDetail">
          5309 Commonwealth Centre Parkway<br />
          <nobr>
            Midlothian, VA 23112</nobr></td>
        <td class="tdReportDetail">
          <a href="http://warner.senate.gov/public/index.cfm?">warner.senate.gov/public/index.cfm?</a><br />
          <a href="http://warner.senate.gov/" target="_self" title="John W. Warner Website">
            warner.senate.gov/</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VAWebbJamesH") %>" target="_self" title="James H. (Jim) Webb"><span
              class="class">James H. (Jim) Webb</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.vademocrats.org/" target="_self" title="Democratic Party of Virginia Website">
            D</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            804.771.2221</nobr></td>
        <td class="tdReportDetail">
          PO Box 17427<br />
          <nobr>
            Arlington, VA 22216</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:infor@webbforsenate.com">infor@webbforsenate.com</a><br />
          <a href="http://webbforsenate.com" target="_self" title="James H. (Jim) Webb Website">
            webbforsenate.com</a></td>
      </tr>
    </table>
    <!-- Table -->
    <user:CustomStyleSheet ID="CustomStyleSheet1" runat="server" />
    <!--table -->
    <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0">
      <tr>
        <td align=left>
          <input id="FileStyleSheet" type="file" name="PictureFile" runat="server" size="65" class="TextBoxInput" >
          <input class="Buttons" id="ButtonUploadStyleSheet" type="submit" value="Upload"
            name="ButtonUpload" runat="server" onserverclick="ButtonUploadStyleSheet_ServerClick1" style="width: 150px"></td>
      </tr>
      </table>
      
    <!--table -->
    <table class="tableAdmin" id="Table20" cellspacing="0" cellpadding="0">
      <tr>
        <td colspan="1">
          <asp:Button ID="ButtonDeleteCustomStyleSheet1" runat="server" CssClass="Buttons" OnClick="ButtonDeleteCustomStyleSheet1_Click"
            Text="Delete" Width="150px" />&nbsp;</td>
        <td colspan="1" class="T" width="100%">
          Click this button to delete the current custom style sheet for the page elements
          above.</td>
      </tr>
    </table>
    <!--table -->
    <table class="tableAdmin" id="Table22" cellspacing="0" cellpadding="0">
      <tr>
        <td>
        </td>
        <td class="HSmall" align="center">
          <asp:Button ID="ButtonUpdateCustomStyleSheet" runat="server" CssClass="Buttons" Text="Update"
            Width="150px" OnClick="ButtonUpdateCustomStyleSheet_Click" /></td>
      </tr>
      <tr>
        <td class="H">
          Default Style Sheet</td>
        <td class="H">
          Custom Style Sheet</td>
      </tr>
      <tr>
        <td>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDefaultStyleSheet" runat="server" TextMode="MultiLine" 
            ReadOnly="True" Width="350px" Height="3000px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
        </td>
        <td>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCustomStyleSheet" runat="server"  Height="3000px"
            TextMode="MultiLine" Width="350px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>