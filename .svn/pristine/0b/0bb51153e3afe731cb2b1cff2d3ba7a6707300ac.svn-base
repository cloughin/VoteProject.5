<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignDefaultsDefaultPage4SingleStateDomain.aspx.cs"
  Inherits="Vote.Master.DesignDefaultsDefaultPageState" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>--%>
<%--<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml"
  TagPrefix="uc9" %>
--%><%--<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc6" %>
<%@ Register Src="Instructions/DesignDefaultsStyleSheet.ascx" TagName="DesignDefaultsStyleSheet"
  TagPrefix="uc5" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Default.aspx Design (State Identified)</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link href="/css/Default.css" type="text/css" rel="stylesheet" />
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
        DEFAULT Design for <span style="color: red">Default.aspx</span> for Domains Presenting
        a &nbsp;<span style="color: #ff0000">Single State's</span> Data
      </td>
    </tr>
    <tr>
      <td align="left">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <uc6:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
  <!-- Table -->
  <user:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Page Element 
        Default Templates &amp; Buttons</td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <strong>Default Template:</strong>
        Above each textbox for each page element is the default template for the 
        election status selected.&nbsp; <br />
      </td>
    </tr>
  </table>
  <table class="tableAdmin" id="Table13" cellspacing="0" cellpadding="0">
    <tr>
      <td class="T" valign="top">
        <strong>Get Default Template Button:</strong> Loads the default element template for the
        election statuses selected in the default element textbox for editing.<br />
        <strong>Save Default Template Button:</strong> Saves the default element 
        template for the election statuses selected.<br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table15" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H">
        Page Element
        Templates for Various Election Statuses</td>
    </tr>
    <tr>
      <td class="T">
        Select one of the radio buttons below to view the various page elements and templates for that
        election status.&nbsp; 
        <asp:RadioButtonList ID="RadioButtonList_Main_Content_Single_State" runat="server"
          AutoPostBack="True" CssClass="RadioButtons" 
          OnSelectedIndexChanged="RadioButtonList_Main_Content_Single_State_SelectedIndexChanged">
          <asp:ListItem Value="Default">Default When There are No Upcoming Viewable Elections</asp:ListItem>
          <asp:ListItem Value="General">When a GENERAL Election is Upcoming and Viewable (type G)</asp:ListItem>
          <asp:ListItem Value="OffYear">When an OFF YEAR Election is Upcoming and Viewable (type O)</asp:ListItem>
          <asp:ListItem Value="Special">When a SPECIAL Election is Upcoming and Viewable (type S)</asp:ListItem>
          <asp:ListItem Value="Primary">When a Statewide Party PRIMARY Election is Upcoming and Viewable (type P)</asp:ListItem>
          <asp:ListItem Value="StatePresidentialPrimary">When a Statewide Presidential Party PRIMARY Election is Upcoming and Viewable (type B)</asp:ListItem>
          <asp:ListItem Value="NationalPresidentialPrimary">When a National Presidential Party PRIMARY Election is Upcoming and Viewable (type A)</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table7" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Default
        Meta Title Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="HSmall">
        Default
        Title Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Title_Template_Default" runat="server" 
          CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxTitleTagDefaultPageSingleStateDomain" runat="server" TextMode="MultiLine"
          Rows="1" Height="50px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left">
        &nbsp;<asp:Button 
          ID="Button_Get_Default_Template_Title" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Default_Template_Title_Click" 
          Text="Get Default Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Default_Template_Title" runat="server"
          Text="Save Default Template" OnClick="Button_Save_Default_Template_Title_Click"
          CssClass="Buttons" Width="150px" />&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Default
        Meta Description Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="HSmall">
        Default
        Description Tag
        Template for the Election Status Selected</td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Description_Template_Default" runat="server" 
          CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMetaDescriptionTagDefaultPageSingleStateDomain" runat="server"
          TextMode="MultiLine" Rows="1" Height="50px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left">
        &nbsp;<asp:Button ID="Button_Get_Default_Template_Description" 
          runat="server" CssClass="Buttons"
          OnClick="Button_Get_Default_Template_Description_Click" 
          Text="Get Default Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Default_Template_Description"
          runat="server" Text="Save Default Template" OnClick="Button_Save_Default_Template_Description_Click"
          CssClass="Buttons" Width="150px" />&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Default
        Meta Keywords Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="HSmall">
        Default
        Keywords Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Keywords_Template_Default" runat="server" 
          CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMetaKeywordsTagDefaultPageSingleStateDomain" runat="server"
          TextMode="MultiLine" Rows="1" Height="50px" Width="930px" 
          CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left">
        &nbsp;<asp:Button ID="Button_Get_Default_Template_Keywords" 
          runat="server" CssClass="Buttons"
          OnClick="Button_Get_Default_Template_Keywords_Click" 
          Text="Get Default Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Default_Template_Keywords"
          runat="server" Text="Save Default Template" 
          OnClick="Button_Save_Default_Template_Keywords_Click"
          CssClass="Buttons" Width="150px" />&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table id="Table6" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td align="left" class="H" style="height: 14px">
        Default Election Information</td>
    </tr>
    </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table12" cellspacing="0" cellpadding="0">
    <tr>
      <td class="HSmall">
        Default
        Election Information
        Template for the Election Status Selected 
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0">
    <tr>
      <td class="T">
        <asp:Label ID="Label_Election_Template_Default" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" style="width: 700px">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMainContentDefaultPageSingleStateDomain" runat="server" TextMode="MultiLine"
          Rows="1" Height="200px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" style="width: 700px">
        &nbsp;<asp:Button 
          ID="Button_Get_Default_Template_Election" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Default_Template_Election_Click" 
          Text="Get Default Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Default_Template_Election"
          runat="server" Text="Save Default Template" 
          OnClick="Button_Save_Default_Template_Election_Click"
          CssClass="Buttons" Width="150px" />&nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->


  <!-- Table -->
  <uc5:DesignDefaultsStyleSheet ID="DesignDefaultsStyleSheet1" runat="server" />
  <table id="Table9" cellpadding="0" cellspacing="0" class="tableAdmin">
    <tr>
      <td align="left" class="H">
        Default Style Sheet(/css/Default.css)
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:Button ID="ButtonUpdateStyleSheet" runat="server" CssClass="Buttons" OnClick="ButtonUpdateStyleSheet_Click"
          Text="Update Style Sheet" Width="150px" />
      </td>
    </tr>
    <tr>
      <td align="center" class="BorderLight">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxStyleSheet" runat="server" CssClass="TextBoxInputMultiLine"
          Height="3000px" TextMode="MultiLine" Width="350px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>