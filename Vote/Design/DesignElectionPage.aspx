<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignElectionPage.aspx.cs" Inherits="Vote.Admin.DesignElectionPage" %>

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
  <title>Design for Election.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Election.css" type="text/css" rel="stylesheet" />
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
          Design for <span style="color: red">Election.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" ForeColor="Red"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Election Report Page (Election.aspx).</td>
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
    <table id="Table28"
      cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="3" valign="middle">
          Samples of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view" >Sample Page 1</asp:HyperLink>&nbsp;
        </td>
        <td class="T" colspan="1" valign="middle">
          <asp:HyperLink ID="HyperLinkSamplePage2" runat="server" CssClass="HyperLink"
            Target="view" >Sample Page 2</asp:HyperLink></td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample report page.</td>
      </tr>
    </table>
    <!-- Table -->
    <uc6:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table29" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
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
    <table id="Table30" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
      <tr class="trElectionElectionInfo">
        <td class="tdElectionElectionInfo">
          <span id="ElectionElectionInfo">The Democratic Presidential candidates still in the
            race at this time include Barack Obama and Hillary Clinton. John Edwards, Joe Biden,
            and Dennis Kucinich, have withdrawn from the race, but will still appear on ballots.
            Mike Gravel, also on ballots, has not withdrawn but can not win the nomination.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table31" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
      <tr class="trElectionResultsHead">
        <td class="tdElectionResultsHead">
          Results</td>
      </tr>
      <tr class="trElectionResultsImage">
        <td class="tdElectionResultsImage">
          <img id="ResultsImage" border="0" class="ResultsImage" src="/images/Elections/20080212BVA000000VAD.jpg" /></td>
      </tr>
      <tr class="trElectionResultsSource">
        <td class="tdElectionResultsSource">
          Source:<span id="ElectionResultsSource">politics.nytimes.com/election-guide/2008/results/states/VA.html</span>
          <span id="ElectionResultsDate">02/12/2008</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table32" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <table border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
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
    <table id="Table33" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for US Presidential Primary Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table5" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSPresPrimary" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSPresPrimary" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSPresPrimary_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table14" cellspacing="0" cellpadding="0">
     <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSPresPrimary" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageUSPresPrimary" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageUSPresPrimary_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageUSPresPrimary" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageUSPresPrimary_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageUSPresPrimary" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="168px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageUSPresPrimary_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2"valign="top">
          Instructions for US Presidential and Vice President Election Report
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table3" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSPres" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table15" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSPres" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSPres_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table16" cellspacing="0" cellpadding="0">
     <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSPres" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageUSPres" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageUSPres_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageUSPres" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageUSPres_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageUSPres" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="170px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageUSPres_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table4" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          US Senate Election Report 
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table6" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSSenate" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table17" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSSenate" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSSenate_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSSenate" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageUSSenate" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageUSSenate_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageUSSenate" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageUSSenate_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageUSSenate" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="168px" 
            OnClick="ButtonSubmitCustom1_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table8" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          US House Election Report</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table7" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageUSHouse" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table21" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageUSHouse" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageUSHouse_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml4" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table23" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageUSHouse" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageUSHouse" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageUSHouse_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageUSHouse" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageUSHouse_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageUSHouse" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="164px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageUSHouse_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table10" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for State Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table9" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageState" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table24" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageState" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageState_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table25" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageState" runat="server" TextMode="MultiLine" Rows="1"
            Height="100px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageState" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageState_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageState" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageState_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageState" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="169px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageState_Click" /></td>
      </tr>
    </table>
    
    
    <!-- Table -->
    <table id="Table12" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for
          County Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table11" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageCounty" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table26" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageCounty" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageCounty_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table27" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageCounty" runat="server" CssClass="TextBoxInputMultiLine"
            Height="100px" Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageCounty" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageCounty_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageCounty" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageCounty_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageCounty" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageCounty_Click" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table34" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Instructions for County Election Reports</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table35" border="0" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trElectionInstruction">
        <td class="tdElectionInstruction">
          <asp:Label ID="LabelInstructionsElectionPageLocal" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table36" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElectionPageLocal" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElectionPageLocal_SelectedIndexChanged"
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
    <!-- Table -->
    <table class="tableAdmin" id="Table37" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElectionPageLocal" runat="server" CssClass="TextBoxInputMultiLine" Height="100px"
            Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsElectionPageLocal" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsElectionPageLocal_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsElectionPageLocal" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsElectionPageLocal_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsElectionPageLocal" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsElectionPageLocal_Click" /></td>
      </tr>
    </table>
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
          &lt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table border="0" cellpadding="0" cellspacing="0" class="tableReport">
      <tr class="trReportGroupHeading">
        <td align="center" class="tdReportGroupHeading" colspan="6">
          <a href="<% =UrlManager.GetIssuePageUri("WY", "WY20081104GA", "USPresident", "ALLPersonal") %>"
            target="_self" title=" Side-By-Side Comparisons of the Candidates on the Issues for U.S. President Vice President Democratic Party on U.S. Ballots">
            U.S. President Vice President Democratic Party<br />
            Side-By-Side Comparisons of the Candidates on the Issues</a></td>
      </tr>
      <tr class="trReportIncumbentKeyHeading">
        <td align="left" class="tdReportIncumbentKeyHeading" colspan="6">
          * = Incumbent</td>
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
            <a href="<% =UrlManager.GetIntroPageUri("ILObamaBarack") %>" target="_self" title="Barack Obama"><span
              class="class">Barack Obama</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.democrats.org/" target="_self" title="Democratic Party Website">
            D</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            312.886.3506</nobr></td>
        <td class="tdReportDetail">
          P.O. Box 8102<br />
          <nobr>
            Chicago, IL 60680</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:info@barackobama.com">info@barackobama.com</a><br />
          <a href="http://my.barackobama.com" target="_self" title="Barack Obama Website">my.barackobama.com</a></td>
      </tr>
      <tr class="trReportRunningMate">
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          N/A<br />
          (Running Mate)</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("OHKucinichDennisJ") %>" target="_self" title="Dennis J. Kucinich">
              <span class="class">Dennis J. Kucinich</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.democrats.org/" target="_self" title="Democratic Party Website">
            D</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            216.228.8850</nobr></td>
        <td class="tdReportDetail">
          4262 Kennebec Road<br />
          <nobr>
            Dixmont, ME 04932</nobr></td>
        <td class="tdReportDetail">
          <a href="mailto:info@dennis4president.com">info@dennis4president.com</a><br />
          <a href="http://www.dennis4president.com" target="_self" title="Dennis J. Kucinich Website">
            www.dennis4president.com</a></td>
      </tr>
      <tr class="trReportRunningMate">
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          N/A<br />
          (Running Mate)</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail">
          &nbsp;</td>
        <td class="tdReportDetail">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("NYClintonHillaryRodham") %>" target="_self" title="Hillary Clinton">
              <span class="class">Hillary Clinton</span></a></nobr></td>
        <td align="center" class="tdReportDetail">
          <a href="http://www.democrats.org/" target="_self" title="Democratic Party Website">
            D</a></td>
        <td align="center" class="tdReportDetail">
          <nobr>
            703-469-2008</nobr></td>
        <td class="tdReportDetail">
          4420 North Fairfax Drive<br />
          <nobr>
            Arlington, VA 22203</nobr></td>
        <td class="tdReportDetail">
          <a href="http://www.hillaryclinton.com/help/contact">www.hillaryclinton.com/help/contact</a><br />
          <a href="http://www.hillaryclinton.com" target="_self" title="Hillary Clinton Website">
            www.hillaryclinton.com</a></td>
      </tr>
      <tr class="trReportRunningMate">
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          N/A<br />
          (Running Mate)</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
        <td align="left" class="tdReportDetail" colspan="1">
          &nbsp;</td>
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