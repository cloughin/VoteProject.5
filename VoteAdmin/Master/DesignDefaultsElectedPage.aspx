<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="DesignDefaultsElectedPage.aspx.cs"
  Inherits="Vote.Master.DesignDefaultsElectedPage" %>

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
  TagPrefix="uc4" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Elected.aspx Design</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link href="/css/Elected.css" type="text/css" rel="stylesheet" />
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
          DEFAULT Design for <span style="color: red">Elected.aspx</span></td>
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
    <uc4:DesignInstructionsControlsDefault ID="DesignInstructionsControlsDefault1" runat="server" />
    <!-- Table -->
    <user:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
    <!-- Table -->
    <table id="Table15" cellpadding="0" cellspacing="0" class="tableAdmin">
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
    <!-- Table -->
    <uc7:DesignPageElements ID="DesignPageElements1" runat="server" />
    <table id="Table3" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Page Title</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectedTitle"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdElectedTitle"&gt;Currently Elected Representatives&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdElectedSubTitle"&gt;for Address...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" border="0">
      <tr class="trElectedTitle">
        <td class="tdElectedTitle" align="left" colspan="4">
          Currently Elected Representatives</td>
        <td class="tdElectedSubTitle" align="left" colspan="1">
          for Address in:<br>
          US House District 8 Virginia<br>
          Virginia Senate District 32<br>
          Virginia House District 36<br>
          Fairfax County</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Elected Officials Instructions</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectedInstruction"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdElectedInstruction"&gt;These are the elected officials...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    
    <!-- Table -->
    <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0">
      <tr class="trElectedInstruction">
        <td class="tdElectedInstruction">
          <asp:Label ID="LabelInstructionsElected" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsElected" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListIsTextInstructionsElected_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%">
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsElected" runat="server" TextMode="MultiLine"
            Rows="1" Height="200px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultInstructionsElected" runat="server" Text="Update Default Design"
            OnClick="ButtonSubmitDefaultInstructionsElected_Click" CssClass="Buttons" Width="172px" />&nbsp;
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table10" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Elected Officials for Address</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReportGroupHeading"&gt;&lt;td
          class="tdReportGroupHeading"&gt;U.S. President...&lt;/td&gt;</td>
      </tr>
      <tr class="trBallotOfficeContestsRowOfThree">
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReportDetailHeading"&gt;&lt;td Class="tdReportDetailHeading"&gt;Name&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td Class="tdReportDetailHeading"&gt;Party&lt;/td&gt;...&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReportDetail"&gt;&lt;td Class="tdReportDetail"&gt;&lt;a
          ... &gt;George...&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td Class="tdReportDetail"&gt;&lt;a ... &gt;R&lt;/td&gt;...&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trElectedOfficeMsg"&gt;&lt;td Class="tdElectedOfficeMsg"&gt;County
          &amp; Local Legislative Offices&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0">
      <tr class="trReportGroupHeading">
        <td class="tdReportGroupHeading" align="center" colspan="5">
          U.S. President<br>
          Vice President</td>
      </tr>
      <tr class="trReportDetailHeading">
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
        <td class="tdReportDetail" align="left">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("TXBushGeorgeW") %>" title="George W. Bush, U.S. President Vice President"
              target="_self"><span class="CandidateLink">George W. Bush </span></a>
          </nobr>
        </td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.rnc.org/" title="Republican Party Website" target="_self">R</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            703-647-2700</nobr></td>
        <td class="tdReportDetail" align="left">
          43 Prairie Chapel Ranch<br>
          <nobr>
            Crawford TX</nobr></td>
        <td class="tdReportDetail" align="left">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br>
          <a href="http://www.georgewbush.com" title="George W. Bushs Website" target="_self">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail" align="left">
          <a href="<% =UrlManager.GetIntroPageUri("WYCheneyRichardB") %>" target="_self"><span class="CandidateLink">
            Richard B. "Dick" Cheney</span></a></td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.rnc.org/" title="Republican Party Website" target="_self">R</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            N/A</nobr></td>
        <td class="tdReportDetail" align="left">
          43 Prairie Chapel Ranch<br>
          <nobr>
            Wilson WY 83014</nobr></td>
        <td class="tdReportDetail" align="left">
          <a href="mailto:BushCheney04@GeorgeWBush.com">BushCheney04@GeorgeWBush.com</a><br>
          <a href="http://www.georgewbush.com" title="Richard B. Dick Cheneys Website" target="_self">
            www.georgewbush.com</a></td>
      </tr>
      <tr class="trReportGroupHeading">
        <td class="tdReportGroupHeading" align="center" colspan="5">
          U.S. Senate</td>
      </tr>
      <tr class="trReportDetailHeading">
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
        <td class="tdReportDetail" align="left">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VAWarnerJohnW") %>" title="John W. Warner, U.S. Senate Virginia"
              target="_self"><span class="CandidateLink">John W. Warner </span></a>
          </nobr>
        </td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.rpv.org/" title="Republican Party of Virginia Website" target="_self">
            R</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            804.739.0247</nobr></td>
        <td class="tdReportDetail" align="left">
          5309 Commonwealth Centre Parkway<br>
          <nobr>
            Midlothian, VA 23112</nobr></td>
        <td class="tdReportDetail" align="left">
          <a href="http://warner.senate.gov/public/index.cfm?">warner.senate.gov/public/index.cfm?</a><br>
          <a href="http://warner.senate.gov/" title="John W. Warners Website" target="_self">
            warner.senate.gov/</a></td>
      </tr>
      <tr class="trReportDetail">
        <td class="tdReportDetail" align="left">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VAWebbJamesH") %>" title="James H. (Jim) Webb, U.S. Senate Virginia"
              target="_self"><span class="CandidateLink">James H. (Jim) Webb </span></a>
          </nobr>
        </td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.vademocrats.org/" title="Democratic Party of Virginia Website"
            target="_self">D</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            804.771.2221</nobr></td>
        <td class="tdReportDetail" align="left">
          PO Box 17427<br>
          <nobr>
            Arlington, VA 22216</nobr></td>
        <td class="tdReportDetail" align="left">
          <a href="mailto:infor@webbforsenate.com">infor@webbforsenate.com</a><br>
          <a href="http://webbforsenate.com" title="James H. (Jim) Webbs Website" target="_self">
            webbforsenate.com</a></td>
      </tr>
      <tr class="trElectedOfficeSpacer">
        <td class="tdElectedOfficeSpacer" colspan="3">
          &nbsp;</td>
      </tr>
      <tr class="trElectedOfficeMsg">
        <td class="tdElectedOfficeMsg" colspan="5">
          County & Local Executive Offices</td>
      </tr>
      <tr class="trReportGroupHeading">
        <td class="tdReportGroupHeading" align="center" colspan="5">
          Sheriff<br>
          Fairfax County</td>
      </tr>
      <tr class="trReportDetailHeading">
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
        <td class="tdReportDetail" align="left">
          <nobr>
            <a href="<% =UrlManager.GetIntroPageUri("VABarryStanG") %>" title="Stan G. Barry, Sheriff Fairfax County Virginia"
              target="_self"><span class="CandidateLink">Stan G. Barry </span></a>
          </nobr>
        </td>
        <td class="tdReportDetail" align="center">
          <a href="http://www.vademocrats.org/" title="Democratic Party of Virginia Website"
            target="_self">D</a></td>
        <td class="tdReportDetail" align="center">
          <nobr>
            N/A</nobr></td>
        <td class="tdReportDetail" align="left">
          14301 Flomation Ct<br>
          <nobr>
            Centreville, VA 20121</nobr></td>
        <td class="tdReportDetail" align="left">
          N/A<br>
          <a href="http://www.fairfaxcounty.gov/sheriff/" title="Stan G. Barrys Website" target="_self">
            www.fairfaxcounty.gov/sheriff/</a></td>
      </tr>
    </table>
    <!-- Table -->
    &nbsp;
    <!-- Table -->
    <uc5:DesignDefaultsStyleSheet ID="DesignDefaultsStyleSheet1" runat="server" />
    <!-- Table -->
    <table id="Table9" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H">
          Default Style Sheet(/css/Elected.css)</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="ButtonUpdateStyleSheet" runat="server" CssClass="Buttons" OnClick="ButtonUpdateStyleSheet_Click"
            Text="Update Style Sheet" Width="150px" /></td>
      </tr>
      <tr>
        <td align="center" class="BorderLight">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStyleSheet" runat="server" CssClass="TextBoxInputMultiLine" Height="2000px"
            TextMode="MultiLine" Width="350px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>