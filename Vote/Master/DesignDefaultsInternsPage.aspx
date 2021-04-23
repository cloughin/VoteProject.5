<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignDefaultsInternsPage.aspx.cs" Inherits="Vote.Master.DesignDefaultsInternsPage" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc8" %>
<%@ Register Src="Instructions/DesignInstructionsStyleSheets.ascx" TagName="DesignInstructionsStyleSheets"
  TagPrefix="uc9" %>

<%@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements" TagPrefix="uc7" %>

<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml" TagPrefix="uc6" %>

<%@ Register Src="Instructions/DesignDefaultsStyleSheet.ascx" TagName="DesignDefaultsStyleSheet"
  TagPrefix="uc5" %>

<%@ Register Src="Instructions/DesignInstructionsControlsDefault.ascx" TagName="DesignInstructionsControlsDefault"
  TagPrefix="uc4" %>

<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>

<%@ Register Src="../Instructions/DesignInstructionsSubstitutionsNoState.ascx" TagName="DesignInstructionsSubstitutionsNoState"
  TagPrefix="user" %>

<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
<head id="Head1" runat="server">
  <title>Interns.aspx Design</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link href="/css/Interns.css" type="text/css" rel="stylesheet" />
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
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          DEFAULT Design for <span style="color: red">Interns.aspx</span></td>
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
      <user:DesignInstructionsSubstitutionsNoState ID="DesignInstructionsSubstitutionsNoState1"
        runat="server" />
      
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
    <!-- Table -->
      <uc7:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" valign="top">
          Interns Page Title</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trInternsPageTitle"&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;td class="tdInternsPageTitle"&gt;Interns&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0">
      <tr class="trInternsPageTitle">
        <td class="tdInternsPageTitle">
          Interns</td>
      </tr>
    </table>
    <!-- Table --><table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="H" valign="top">
          Interns Page Content</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;td class="tdInternsPageContent"&gt;At Vote-USA.org, we feel that American representative
          democracy...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cellpadding="0">
      <tr class="trInternsPageContent">
        <td class="tdInternsPageContent">
          <asp:Label ID="LabelContentInterns" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight" >
          <asp:RadioButtonList ID="RadioButtonListIsTextContentInterns" runat="server" AutoPostBack="True"
            CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListIsTextContentInterns_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" class="T" width="100%" >
          <uc6:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContentInterns" runat="server" TextMode="MultiLine"
            Rows="1" Height="400px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;<asp:Button ID="ButtonSubmitDefaultContentInterns" runat="server" Text="Update Default Design" 
          OnClick="ButtonSubmitDefaultContentInterns_Click"
            CssClass="Buttons" Width="167px" />&nbsp;
          </td>
      </tr>
     </table>    
    <!-- Table -->
      <uc5:DesignDefaultsStyleSheet ID="DesignDefaultsStyleSheet1" runat="server" />
     <!-- Table -->
     <table id="Table9" cellpadding="0" cellspacing="0" class="tableAdmin">
        <tr>
          <td align="left" class="H">
            Default Style Sheet(/css/Interns.css)</td>
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