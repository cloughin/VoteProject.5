<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignDefaultPage4SingleStateDomain.aspx.cs"
  Inherits="Vote.Admin.DesignDefaultPageState" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc8" %>
--%>
<%-- 
  <%@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements"
  TagPrefix="uc7" %>
<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml"
  TagPrefix="uc6" %>
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
  <title>Design for Default.aspx (State Selected)</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Default.css" type="text/css" rel="stylesheet" />
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
        Design for <span style="color: red">Default.aspx</span> for Design Code:
        <asp:Label ID="LabelDesignCode" runat="server" CssClass="HLargeColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        This form is used to set the variable page elements for the home page (Default.aspx)
        of single state domains (Vote-XX.org).
      </td>
    </tr>
  </table>
  <!-- Table -->
  <uc8:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
  <!-- Table -->
  <uc4:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
  <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Page Elements and
        Their Templates</td>
    </tr>
    <tr>
      <td class="T" valign="top">
        Above each textbox for each page element the following is provided:<br />
        <strong>Current Element:</strong>
        This is the element currently showing for this domain.
        <br />
        <strong>Default Template:</strong> for the Elemant:
        This is the default template for the election status selected. It can NOT be changed
        here. It can only be changed by the master administrator. If you click the Get Default
        Template Button this default template will be loaded into the Custom Template textbox,
        which may provide a good starting point in the design of a new custom template.
        <br />
        <strong>Custom Template:</strong>
        This is the custom template for the election status selected. If the template is
        empty the default template will be applied, otherwise, this custom template will be applied.
        <br />
        <strong>Element with Substitutions Applied:</strong> This is the element with 
        substitutions applied depending on the election status selected. When there is 
        NO upcoming viewable election and an upcoming viewable status is selected the 
        [[substitution]] will be shown.<br />
      </td>
    </tr>
  </table>
  <table class="tableAdmin" id="Table7" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Buttons
      </td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <strong>Get Default Template:</strong> Loads the default element template for the
        election statuses selected in the custom element textbox. This makes it easy to 
        make minor custom changes starting with the default template.<br />
        <strong>Get Custom Template:</strong> Loads any custom element template for the election
        statuses selected in the custom element textbox. This makes it easy to make minor changes to the custom
        template.
        <br />
        <strong>Save Custom Template:</strong> Saves the custom element template for the 
        election statuses selected, causing the custom element to be used instead of the 
        default.<br />
        <strong>Clear Custom Template:</strong> Clears the custom element template for 
        the election statuses selected, causing the element to revert to the default 
        element. <br />
      </td>
    </tr>
  </table>
  <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0">
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
          AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonList_Main_Content_Single_State_SelectedIndexChanged">
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
  <table>
    <tr>
      <td class="H" valign="top">
        Custom
        Meta Title Tag
      </td>
    </tr>


    <!-- rows -->
    <tr>
      <td valign="top" class="HSmall">
        Current Title Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Title_Current" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Default
        Title Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Title_Template_Default" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Custom
        Title Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td align="left" class="BorderLight">
        <asp:Label ID="Label_Title_Template_Custom" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Title Tag with Substitutions Applied Depending for the Election 
        Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Title_Viewable_Status" runat="server" CssClass="TBoldColor"></asp:Label>
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
        &nbsp;&nbsp; &nbsp;
        <asp:Button ID="Button_Get_Default_Template_Title" runat="server" CssClass="Buttons"
          Text="Get Default Template" Width="150px" OnClick="Button_Get_Default_Template_Title_Click" />&nbsp;
        &nbsp; &nbsp; &nbsp;
        <asp:Button ID="Button_Get_Custom_Template_Title" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Custom_Template_Title_Click" 
          Text="Get Custom Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Custom_Template_Title" runat="server" CssClass="Buttons"
          Text="Save Custom Template" Width="160px" OnClick="Button_Save_Custom_Template_Title_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Clear_Custom_Template_Title" runat="server" CssClass="Buttons"
          Text="Clear Custom Template" Width="150px" OnClick="Button_Clear_Custom_Template_Title_Click" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Custom
        Meta Description Tag
      </td>
    </tr>

    <tr>
      <td valign="top" class="HSmall">
        Current Description Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Description_Current" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Default
        Description Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Description_Template_Default" runat="server" 
          CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Custom
        Description Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td align="left" class="BorderLight">
        <asp:Label ID="Label_Description_Template_Custom" runat="server" 
          CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Description Tag with Substitutions Applied Depending for the Election 
        Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Description_Viewable_Status" runat="server" 
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
        &nbsp;&nbsp; &nbsp;
        <asp:Button ID="Button_Get_Default_Template_Description" runat="server" CssClass="Buttons"
          Text="Get Default Template" Width="150px" OnClick="Button_Get_Default_Template_Description_Click" />&nbsp;
        &nbsp; &nbsp; &nbsp;
        <asp:Button ID="Button_Get_Custom_Template_Description" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Custom_Template_Description_Click" 
          Text="Get Custom Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Custom_Template_Description" runat="server" CssClass="Buttons"
          Text="Save Custom Template" Width="160px" OnClick="Button_Save_Custom_Template_Description_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Clear_Custom_Template_Description" runat="server" CssClass="Buttons"
          Text="Clear Custom Template" Width="150px" OnClick="Button_Clear_Custom_Template_Description_Click" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
    <tr>
      <td class="H" valign="top">
        Custom
        Meta Keywords Tag
      </td>
    </tr>

    <tr>
      <td valign="top" class="HSmall">
        Current Keywords Tag
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Keywords_Current" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Default
        Keywords Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Keywords_Template_Default" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Custom
        Keywords Tag
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td align="left" class="BorderLight">
        <asp:Label ID="Label_Keywords_Template_Custom" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Keywords Tag with Substitutions Applied Depending for the Election 
        Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Keywords_Viewable_Status" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>


    <tr>
      <td align="left">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMetaKeywordsTagDefaultPageSingleStateDomain" runat="server"
          TextMode="MultiLine" Rows="1" Height="50px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left">
        &nbsp;&nbsp; &nbsp;
        <asp:Button ID="Button_Get_Default_Template_Keywords" runat="server" CssClass="Buttons"
          Text="Get Default Template" Width="150px" OnClick="Button_Get_Default_Template_Keywords_Click" />&nbsp;
        &nbsp; &nbsp; &nbsp;
        <asp:Button ID="Button_Get_Custom_Template_Keywords" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Custom_Template_Keywords_Click" 
          Text="Get Custom Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Custom_Template_Keywords" runat="server" CssClass="Buttons"
          Text="Save Custom Template" Width="160px" OnClick="Button_Save_Custom_Template_Keywords_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Clear_Custom_Template_Keywords" runat="server" CssClass="Buttons"
          Text="Clear Custom Template" Width="150px" OnClick="Button_Clear_Custom_Template_Keywords_Click" />
      </td>
    </tr>
  </table>
  <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="H">
        Custom Election 
        Information
      </td>
    </tr>
    <tr>
      <td valign="top" class="HSmall">
        Current Election Information
      </td>
    </tr>
    <tr>
      <td valign="top" class="BorderLight">
        <asp:Label ID="Label_Election_Current" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Default
        Election Information
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Election_Template_Default" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Custom
        Election Information
        Template for the Election Status Selected 
      </td>
    </tr>
    <tr>
      <td align="left" class="BorderLight">
        <asp:Label ID="Label_Election_Template_Custom" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Election Information with Substitutions Applied Depending for the Election 
        Status Selected 
      </td>
    </tr>
    <tr>
      <td class="BorderLight">
        <asp:Label ID="Label_Election_Viewable_Status" runat="server" CssClass="TBoldColor"></asp:Label>
      &nbsp;
      </td>
    </tr>
    <tr>
      <td align="left">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBox_MainContent_Custom" runat="server" CssClass="TextBoxInputMultiLine"
          Height="200px" Rows="1" TextMode="MultiLine" Width="930px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Add, Change or Delete Custom Templates
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        1) Select a election status radio button to add, change or delete one of the custom
        templates.<br />
        2) Then use the textbox above and the buttons below to perform the desired custom
        template operation.<br />
      </td>
    </tr>
    <tr>
      <td align="left">
        <asp:Button ID="Button_Get_Default_Template_Election" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Default_Template_Election_Click" Text="Get Default Template"
          Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Get_Custom_Template_Election" runat="server" CssClass="Buttons"
          OnClick="Button_Get_Custom_Template_Election_Click" Text="Get Custom Template" Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Save_Custom_Template_Election" runat="server" CssClass="Buttons"
          OnClick="Button_Save_Custom_Template_Election_Click" Text="Save Custom Template"
          Width="150px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button_Clear_Custom_Template_Election" runat="server" CssClass="Buttons"
          OnClick="Button_Clear_Custom_Template_Election_Click" Text="Clear Custom Template"
          Width="150px" />
      </td>
    </tr>
  </table>
  <user:CustomStyleSheet ID="CustomStyleSheet1" runat="server" />
  <!--table -->
  <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left">
        <input id="FileStyleSheet" type="file" name="PictureFile" runat="server" size="65"
          class="TextBoxInput">
        <input class="Buttons" id="ButtonUploadStyleSheet" type="submit" value="Upload" name="ButtonUpload"
          runat="server" onserverclick="ButtonUploadStyleSheet_ServerClick1" style="width: 150px">
      </td>
    </tr>
  </table>
  <!--table -->
  <table class="tableAdmin" id="Table20" cellspacing="0" cellpadding="0">
    <tr>
      <td colspan="1">
        <asp:Button ID="ButtonDeleteCustomStyleSheet1" runat="server" CssClass="Buttons"
          OnClick="ButtonDeleteCustomStyleSheet1_Click" Text="Delete" Width="150px" />&nbsp;
      </td>
      <td colspan="1" class="T" width="100%">
        Click this button to delete the current custom style sheet for the page elements
        above. e.
      </td>
    </tr>
  </table>
  <!--table -->
  <table class="tableAdmin" id="Table22" cellspacing="0" cellpadding="0">
    <tr>
      <td>
      </td>
      <td class="HSmall" align="center">
        <asp:Button ID="ButtonUpdateCustomStyleSheet" runat="server" CssClass="Buttons" Text="Update"
          Width="150px" OnClick="ButtonUpdateCustomStyleSheet_Click" />
      </td>
    </tr>
    <tr>
      <td class="H">
        Default Style Sheet
      </td>
      <td class="H">
        Custom Style Sheet
      </td>
    </tr>
    <tr>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDefaultStyleSheet" runat="server" TextMode="MultiLine" ReadOnly="True"
          Width="350px" Height="3000px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCustomStyleSheet" runat="server" Height="3000px" TextMode="MultiLine"
          Width="350px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>