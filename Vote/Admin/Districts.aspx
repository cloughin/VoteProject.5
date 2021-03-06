<%@ Page  ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Districts.aspx.cs" Inherits="Vote.Admin.DistrictsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    #body {
      margin-bottom: 20px;
    }
    .ui-autocomplete .ui-menu-item {
      text-align: left;
    }
    .ui-autocomplete .ui-menu-item.ui-state-focus {
      border: none;
      background: #0000ff;
      color: #ffffff;
    }
  </style>
  <script type="text/javascript">
    //function pageLoad() {
    //}
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    <!-- Table -->
    <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="TSmall">
          &nbsp;This form is used to add districts or edit district names.
           Districts may be local districts in a county or multi-county districts in a state.</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableLocalDistrictAdd" cellspacing="0" cellpadding="0"
      border="0" runat="server">
      <tr>
        <td align="left" class="H">
          Add a Local District or Town Not in the Database</td>
      </tr>
      <tr>
        <td align="left" class="T" >
          <strong>To Add a District:</strong><br />
          1)
          Make certain the district you are about to add is NOT in the report
          of Districts below.<br />
          2)
          Then enter the district name in the textbox provided exactly as
          it should appear on ballots. To save trouble
          you can click one of the Append buttons below the textbox to add the word 'District', 'Town'
          or 'City' to the name. The contents of the textbox should be the name exactly as it should appear
          on ballots.<br />
          3) You may <strong>optionally</strong> provide a 2 digit code for the district. 
          If provided it has to be a unique code. Existing codes are shown in parens
          to the right of the name in the Districts Report. 
          If a code is not provided the next sequential number will be
          used.<br />
          4)
          Finally, click the 'Add this District' Button.</td>
      </tr>
      <tr>
        <td align="left" class="TBold" >
          Name:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLocalDistrictAdd" runat="server" 
           CssClass="TextBoxInput autocomplete" Width="300px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;&nbsp; Code:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCode" runat="server" CssClass="TextBoxInput" Width="25px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          &nbsp;<asp:Button ID="ButtonLocalDistrictAdd" runat="server" Text="Add this District"
            Width="200px" OnClick="ButtonLocalDistrictAdd_Click" CssClass="Buttons" /></td>
      </tr>
      <tr>
        <td align="left" class="TBold" >
          <asp:Button ID="Button_Format_Name_Add" runat="server" CssClass="Buttons" OnClick="Button_Format_Name_Click"
            Text="Re-Case Name" Width="150px" />
          &nbsp;<asp:Button ID="ButtonAppendDistrict" runat="server" OnClick="ButtonAppendDistrict_Click"
            Text="Append 'District' to Name" CssClass="Buttons" Width="150px" />&nbsp;
          <asp:Button ID="ButtonAppendTown" runat="server" OnClick="ButtonAppendTown_Click"
            Text="Append 'Town' to Name" CssClass="Buttons" Width="150px" />&nbsp;
          <asp:Button ID="ButtonAppendCity" runat="server" OnClick="ButtonAppendCity_Click"
            Text="Append 'City' to Name" CssClass="Buttons" Width="150px" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableLocalDistrictUpdate" cellspacing="0" cellpadding="0"
      border="0" runat="server">
      <tr>
        <td align="left" class="H">
          Edit Name or Delete Local District or Town</td>
      </tr>
      <tr>
        <td align="left" class="T" >
          <strong>To Change a Local District or Town Name:</strong><br />
          1) Change the local district or town name in the textbox below to exactly as the
          name should appear on ballots.<br />
          2)
          Then, click the 'Update this Local District or Town' Button.<br />
          <strong>To Delete a Local District or Town and all its offices:</strong><br />
          1) Click the View Offices Button to see all the offices in the local district or
          town about to deleted.<br />
          2) Change or consolidate any offices for the local district or town so there are
          none.&nbsp;
          <br />
          3) Click the Delete Local District or Town Button.<br />
          <strong>Note:</strong> A Local Districts or Town needs to be deleted when it appears
          in more than one county.</td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Label ID="Label_Offices" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T" >
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLocalDistrictUpdate" runat="server" 
           CssClass="TextBoxInput autocomplete" Width="350px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          <asp:Button ID="Button_View_Offices" runat="server" CssClass="Buttons" OnClick="Button_View_Offices_Click"
            Text="View Offices in Local District or Town" Width="240px" /></td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:Button ID="Button_Format_Name_Update" runat="server" CssClass="Buttons" OnClick="Button_Format_Name_Click"
            Text="Re-Case Name" Width="150px" />&nbsp;
          <asp:Button ID="ButtonLocalDistrictUpdate" runat="server" Text="Update this Local District or Town"
            Width="220px" OnClick="ButtonLocalDistrictUpdate_Click" CssClass="Buttons" />&nbsp;
          <asp:Button ID="Button_Delete_Local" runat="server" CssClass="Buttons" OnClick="Button_Delete_Local_Click"
            Text="Delete Local District or Town" Width="220px" /></td>
      </tr>
      <tr>
        <td align="left" class="T">
        </td>
      </tr>
    </table>
    <!-- Table -->
    <!-- Table -->
    <table class="tableAdmin" id="TableReport" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          Local Districts and Towns</td>
      </tr>
      <tr>
        <td class="HSmall">
          Order of Local Districts and Towns&nbsp;</td>
      </tr>
      <tr>
        <td class="T" >
          Use these radio buttons to select the order in which the local districts and towns are presented in the report below.</td>
      </tr>
      <tr>
        <td align="center" class="T" >
          <asp:RadioButtonList ID="RadioButtonListSort" runat="server" AutoPostBack="True"
            CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListSort_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem Value="Name" Selected="True">Sort by Name</asp:ListItem>
            <asp:ListItem Value="Code">Sort by Code</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td class="HSmall">
          Edit or Delete a Local District or Town Name</td>
      </tr>
      <tr>
        <td class="T" >
          Click on a link below to change that district or town name or to delete it and its
          offices.</td>
      </tr>
      <tr>
        <td style="height: 17px">
          <asp:Label ID="LabelLocalDistricts" runat="server"></asp:Label>
      </tr>
    </table>
    <!-- Table -->

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
