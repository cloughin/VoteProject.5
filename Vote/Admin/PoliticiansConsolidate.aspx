<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="PoliticiansConsolidate.aspx.cs" Inherits="Vote.Combine2Politicians.PoliticiansConsolidatePage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
    <%--
      <h1 id="H1" runat="server"></h1>
    <!-- Table -->
    <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="HLarge">
          Consolidate Data and Pictures of Two Politicians Into One</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableName" cellspacing="0" cellpadding="0" border="0"
      runat="server">
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Find Politicians</td>
        <td align="right">
        </td>
      </tr>
      <tr>
        <td align="left" class="T">
          Enter any part or parts of the politicianís name in the textboxes provided. When
          you click the button below, all rows matching these parts will be presented in the bottom of this page.
        </td>
        <td align="right">
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="TBold" valign="top">
          State:</td>
        <td valign="top" class="TBold">
          First:</td>
        <td valign="top" class="TBold">
          Middle:</td>
        <td valign="top" class="TBold">
          Nickname:</td>
        <td valign="top" class="TBold">
          Last (required):</td>
        <td valign="top" class="TBold">
          Suffix:</td>
      </tr>
      <tr>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_State" runat="server" CssClass="TextBoxInput" Width="35px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFirst" runat="server" Width="125px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMiddle" runat="server" Width="109px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxNickName" runat="server" Width="106px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLast" runat="server" Width="140px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxSuffix" runat="server" Width="56px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" colspan="6" valign="top">
          <asp:Button ID="Button_Show_For_Name_Parts" runat="server" OnClick="Button_Show_For_Name_Parts_Click"
            Text="Show All Politicians' Data with these Name Parts" CssClass="Buttons" Width="300px" /></td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <table id="TableConsolidate" runat="server" border="0" cellpadding="0" cellspacing="0"
      class="tableAdmin">
      <tr>
        <td class="H" colspan="2">
          Consolidate One Politician Into Another</td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          If two politicians in the report are actually the same person, their data and pictures
          should be consolidated into one politician. To do this:<br />
          Copy the politician's Id
          to be deleted (usually the older data) in the top textbox.<br />
          Copy the politician's Id
          to be retained (usually the newer data) in the bottom textbox. 
          <br />
          Click the Show Both Politicians&#39; Data Button to view information about the 
          politicians in a report at the bottom of this form.<br />
          Click the Consolidate
          Button. 
          <br />
          If there is any doubt, you can find the politicians, as
          directed above.</td>
      </tr>
      <tr>
        <td class="T" width="450">
          <strong>Consolidate and Delete&nbsp;</strong> politician (usually older) whose Id is:</td>
        <td class="T" colspan="1" style="width: 9px">
          <user:TextBoxWithNormalizedLineBreaks ID="Textbox_PoliticianKey_From" runat="server" CssClass="TextBoxInput"
            Width="200px" OnTextChanged="Textbox_PoliticianKey_From_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T">
          <strong>Into this </strong> politician (usually newer) whose Id is:</td>
        <td class="T" colspan="1">
          <user:TextBoxWithNormalizedLineBreaks ID="Textbox_PoliticianKey_To" runat="server" CssClass="TextBoxInput"
            Width="200px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T">
          This button needs to be clicked first to view the politicians before they are consolidated.</td>
        <td class="T" colspan="1">
          <asp:Button ID="Button_Show_Consolidate_One" runat="server" CssClass="Buttons" OnClick="Button_Show_Consolidate_One_Click"
            Text="Show Both Politicians' Data" Width="300px" /></td>
      </tr>
      <tr>
        <td class="T" colspan="2">
          <asp:Button ID="Button_Consolidate_One_Into_Another" runat="server" 
            CssClass="Buttons" OnClick="Button_Consolidate_One_Into_Another_Click"
            Text="Consolidate Top Politician's Data into Bottom Politician's Data" 
            Width="400px" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="0"
      runat="server">
      <tr>
        <td align="left" class="H" >
          Create a NEW Politician (new PoliticianKey) using an Existing Politician or 
          Consolidate Two Politicians into One</td>
        <td align="right">
        </td>
      </tr>
      <tr>
        <td align="left" class="HSmall" >
          Step 1:</td>
      </tr>
      <tr>
        <td align="left" class="T" >
          <strong>Create a New Politician From an Existing Politician:</strong> Enter the 
          existing PoliticianKey in the first textbox and leave the second textbox empty.
          MS SQL treats accented characters like Š and ŗ as different characters. MySQL treats
          both like a. Politicians whose key contains these special characters should be consolidated
          into a new politicain using a key without these special characters. 
          <br />
          <strong>Consolidate Two Politicians into One:</strong> Enter the PoliticianKeys 
          in the two textboxes.</td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          First PoliticianKey:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Id_First" runat="server" CssClass="TextBoxInput" 
            Width="300px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="TBold">
          Second PoliticianKey:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Id_Second" runat="server" CssClass="TextBoxInput" 
            Width="300px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          Step 2:</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click this button to view the politicians before they are consolidated.
          <asp:Button ID="Button_Show_Consolidate_Two" runat="server" OnClick="Button_Show_Consolidate_Two_Click"
            Text="Show Politicians' Data" CssClass="Buttons" Width="300px" />
          <br />
&nbsp; </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td class="TBold" colspan="6" valign="top">
          First Politician &amp; OfficeKey:
          <asp:Label ID="Label_OfficeKey_First" runat="server" CssClass="TBoldColor"></asp:Label></td>
      </tr>
      <tr>
        <td class="TBold" valign="top">
          State:</td>
        <td valign="top" class="TBold">
          First:</td>
        <td valign="top" class="TBold">
          Middle:</td>
        <td valign="top" class="TBold">
          Nickname:</td>
        <td valign="top" class="TBold">
          Last (required):</td>
        <td valign="top" class="TBold">
          Suffix:</td>
      </tr>
      <tr>
        <td valign="top" class="TBoldColor">
          &nbsp;<asp:Label ID="Label_State_First" runat="server"></asp:Label></td>
        <td valign="top" class="TBoldColor">
          <asp:Label ID="Label_FName_First" runat="server"></asp:Label></td>
        <td valign="top" class="TBoldColor">
          <asp:Label ID="Label_Middle_First" runat="server"></asp:Label></td>
        <td valign="top" class="TBoldColor">
          <asp:Label ID="Label_NickName_First" runat="server"></asp:Label></td>
        <td valign="top" class="TBoldColor">
          <asp:Label ID="Label_LName_First" runat="server"></asp:Label></td>
        <td valign="top" class="TBoldColor">
          <asp:Label ID="Label_Suffix_First" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="TBold" colspan="6" valign="top">
          Second Politician &amp; OfficeKey:
          <asp:Label ID="Label_OfficeKey_Second" runat="server" CssClass="TBoldColor"></asp:Label></td>
      </tr>
      <tr>
        <td class="TBold" valign="top" style="height: 13px">
          State:</td>
        <td valign="top" class="TBold" style="height: 13px">
          First:</td>
        <td valign="top" class="TBold" style="height: 13px">
          Middle:</td>
        <td valign="top" class="TBold" style="height: 13px">
          Nickname:</td>
        <td valign="top" class="TBold" style="height: 13px">
          Last (required):</td>
        <td valign="top" class="TBold" style="height: 13px">
          Suffix:</td>
      </tr>
      <tr>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_State_Second" runat="server"></asp:Label></td>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_FName_Second" runat="server"></asp:Label></td>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_Middle_Second" runat="server"></asp:Label></td>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_NickName_Second" runat="server"></asp:Label></td>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_LName_Second" runat="server"></asp:Label></td>
        <td valign="top" style="height: 16px" class="TBoldColor">
          <asp:Label ID="Label_Suffix_Second" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="HSmall" colspan="6" valign="top">
          Step 3:</td>
      </tr>
      <tr>
        <td class="T" colspan="6" valign="top">
          Copy and paste
          the
          OfficeKey in the textbox below. Then enter the name parts of the new politician. 
          The StateCode is almost never changed and should only be changed it the 
          politician is identified with the wrong state.</td>
      </tr>
      <tr>
        <td class="TBold" colspan="6" valign="top">
          New Politician OfficeKey:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_OfficeKey_New" runat="server" CssClass="TextBoxInput" Width="400px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="TBold" valign="top">
          StateCode:</td>
        <td valign="top" class="TBold">
          First:</td>
        <td valign="top" class="TBold">
          Middle:</td>
        <td valign="top" class="TBold">
          Nickname:</td>
        <td valign="top" class="TBold">
          Last (required):</td>
        <td valign="top" class="TBold">
          Suffix:</td>
      </tr>
      <tr>
        <td valign="top" class="TBoldColor">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_StateNew" runat="server" CssClass="TextBoxInput" 
            Width="35px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_FName_New" runat="server" CssClass="TextBoxInput" Width="125px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Middle_New" runat="server" CssClass="TextBoxInput" Width="109px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_NickName_Last" runat="server" CssClass="TextBoxInput" Width="106px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_LName_New" runat="server" CssClass="TextBoxInput" Width="140px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Suffix_New" runat="server" CssClass="TextBoxInput" Width="56px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="HSmall">
          Step 4:</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click this button to create the new politician
          <asp:Button ID="Button_Consolidate_Two_Into_New" runat="server" OnClick="Button_Consolidate_Two_Into_New_Click"
            Text="Create a New Politician" CssClass="Buttons" Width="300px" />
          <br />
&nbsp; </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticiansHeading" cellspacing="0" cellpadding="0"
      border="0">
      <tr>
        <td align="center" class="H">
          Politicians</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click on any of the names in this report to view the politicianís Intro Page.
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TablePoliticians" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="center">
          <asp:Label ID="HTMLTablePoliticians" runat="server"></asp:Label>
        </td>
      </tr>
    </table>
    --%>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
