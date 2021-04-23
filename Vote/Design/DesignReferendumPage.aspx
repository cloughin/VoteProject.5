<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignReferendumPage.aspx.cs" Inherits="Vote.Admin.DesignReferendumPage" %>

<html><head><title></title></head><body></body></html>

<%-- 
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
  <title>Design for Referendum.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Referendum.css" type="text/css" rel="stylesheet" />
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
          Design for <span style="color: red">Referendum.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" ForeColor="Red"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Referendum Page (Referendum.aspx).</td>
      </tr>
    </table>
    &nbsp;
    <!-- Table -->
    <uc7:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
    <!-- Table -->
    <uc8:DesignInstructionsStyleSheets id="DesignInstructionsStyleSheets1" runat="server" />
    <!-- Table -->
    <uc9:DesignInstructionsControls ID="DesignInstructionsControls1" runat="server" />
    <!-- Table -->
    <uc4:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
    
    <!-- Table -->
    <table id="Table15" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2" valign="middle">
          Sample of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view">Sample Page</asp:HyperLink>
        </td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample ballot page.</td>
      </tr>
    </table>
    &nbsp;
    <!-- Table -->
    <uc6:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table6" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Page Title</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumName"&gt;&lt;td class="tdReferendumName"&gt;Prop 2: Standards...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumTitle"&gt;&lt;td class="tdReferendumTitle"&gt;California
          Ballot Measure...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumElection"&gt;&lt;td class="tdReferendumElection"&gt;November
          4, 2008...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trReferendumName">
        <td class="tdReferendumName">
          <span id="LabelReferendum">Prop 2: Standards For Confining Farm Animals</span></td>
      </tr>
      <tr class="trReferendumTitle">
        <td class="tdReferendumTitle">
          <span id="LabelTitle">California Ballot Measure / Referendum</span></td>
      </tr>
      <tr class="trReferendumElection">
        <td class="tdReferendumElection">
          <span id="LabelElection">November 4, 2008 California General Election</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Description</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumDescription"&gt;&lt;td
          class="tdReferendumDescription"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;span class="ReferendumDescriptionHead"&gt;Description:&lt;/span&gt;Requires
          that calves ...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr id="DescriptionRow" class="trReferendumDescription">
        <td class="tdReferendumDescription">
          <span class="ReferendumDescriptionHead">Description: </span><span id="ReferendumDesc">
            Requires that calves raised for veal, egg-laying hens and pregnant pigs be confined
            only in ways that allow these animals to lie down, stand up, fully extend their
            limbs and turn around freely.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table3" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Detail</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumDetail"&gt;&lt;td class="tdReferendumDetail"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;span class="ReferendumDetailHead"&gt;Detail:&lt;/span&gt;Requires that calves
          ...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table4" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr id="DetailRow" class="trReferendumDetail">
        <td class="tdReferendumDetail">
          <span class="ReferendumDetailHead"><span id="LabelReferendumDetail"><a href="http://www.voterguide.sos.ca.gov/title-sum/prop2-title-sum.htm">
            Detail:</a></span> </span><span id="ReferendumDetail">Requires that calves raised
              for veal, egg-laying hens and pregnant pigs be confined only in ways that allow
              these animals to lie down, stand up, fully extend their limbs and turn around freely.
              Exceptions made for transportation, rodeos, fairs, 4-H programs, lawful slaughter,
              research and veterinary purposes. Provides misdemeanor penalties, including a fine
              not to exceed $1,000 and/or imprisonment in jail for up to 180 days.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table7" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Full Text</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trReferendumFullText"&gt;&lt;td
          class="tdReferendumFullText"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;span class="ReferendumDetailFullText"&gt;Full Text:&lt;/span&gt;Beginning ...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="table5" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr id="FullTextRow" class="trReferendumFullText">
        <td class="tdReferendumFullText">
          <span class="ReferendumDetailFullText"><span id="LabelReferendumFullText"><a href="http://www.voterguide.sos.ca.gov/text-proposed-laws/text-of-proposed-laws.pdf#prop2">
            Full Text:</a></span> </span><span id="ReferendumFullText">Beginning January 1, 2015,
              this measure prohibits with certain exceptions the confinement on a farm of pregnant
              pigs, calves raised for veal, and egg-laying hens in a manner that does not allow
              them to turn around freely, lie down, stand up, and fully extend their limbs. Under
              the measure, any person who violates this law would be guilty of a misdemeanor,
              punishable by a fine of up to $1,000 and/or imprisonment in county jail for up to
              six months.</span></td>
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