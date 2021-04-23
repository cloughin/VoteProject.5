<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignInternsPage.aspx.cs" Inherits="Vote.Admin.DesignInternsPage" %>

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
  <title>Design for Interns.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Interns.css" type="text/css" rel="stylesheet" />
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
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          Design for <span style="color: red">Interns.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" ForeColor="Red"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Interns Page (Interns.aspx).</td>
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
        <td align="left" class="H" valign="top">
          Interns Page Title</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;td class="tdInternsPageTitle"&gt;Interns&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trInternsPageTitle">
        <td class="tdInternsPageTitle">
          Interns</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table3" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" valign="top">
          Interns Page Content</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;tr class="trInternsPageContent"&gt;</td>
      </tr>
      <tr>
        <td class="TSmallBorder">
          &lt;td class="tdInternsPageContent"&gt;At Vote-USA.org, we feel that American representative
          democracy...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trInternsPageContent">
        <td class="tdInternsPageContent">
          <asp:Label ID="LabelContentInterns" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextContentInterns" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextContentInterns_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
     <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContentInterns" runat="server" TextMode="MultiLine" Rows="1"
            Height="400px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultContentInterns" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultContentInterns_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomContentInterns" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomContentInterns_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomContentInterns" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="161px" 
            OnClick="ButtonSubmitCustomContentInterns_Click" /></td>
      </tr>
    </table>
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