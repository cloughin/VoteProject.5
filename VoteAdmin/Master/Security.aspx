<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Security.aspx.cs" Inherits="Vote.Master.SecurityPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    <!-- Table -->
			<TABLE class="tableAdmin" id="TableTop" cellSpacing="0" cellPadding="0">
        <tr>
          <td align="left">
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
			</TABLE>
    <!-- Table -->
    <table class="tableAdmin" id="TableNewUser1" cellspacing="0" cellpadding="0" runat=server>
    <tr>
    <td class="H">
      Administrators</td>
    </tr>
      <tr>
        <td class="T">
          For now an unrestricted Master user can only create a new ADMIN User, edit an ADMIN
          User, or delete an ADMIN User.</td>
      </tr>
      <tr>
        <td class="T" >
          <strong>Add New ADMIN User:</strong>
          To create a new user enter the user's name, password, check whether the user has data entry authority (like interns) and then select the User Type. This will create the new user. You can then update the new user by selecting
          the particular design, organization and jurisdictions the user can administer.&nbsp;
          Finally, click the Different User to add or update another user.<br />
          <strong>Update ADMIN User:</strong> Select a user in the first column. To change
          any of the codes or type, simply select it. To change the User Name or Password
          change in textbox provide and click button. Selecting any of the codes will automatically
          update the user. and peryou can change the
          password and select the codes and permissions for that administrator.<br />
          <strong>
          Delete a ADMIN User:</strong> Select a user. Then click the Delete Button.</td>
      </tr>
      <tr>
        <td class="HSmall">
          User Name and Password</td>
      </tr>
			</TABLE>
    <!-- Table -->
    <table class="tableAdmin" id="TableOutter" cellspacing="0" cellpadding="0" >
    <tr>
    <td class="TBold">
      New User Name:</td>
    <td class="TBold">
          User Passowrd:</td>
    </tr>
      <tr>
        <td class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_User_Name" runat="server" Width="250px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_User_Password" runat="server" Width="250px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td class="T">
          <asp:Button ID="Button_Update_UserName_Pasword" runat="server" OnClick="Button_Update_UserName_Pasword_Click"
            Text="Update Name & Password" Width="170px" CssClass="Buttons" /></td>
      </tr>
      <tr>
        <td class="HSmall" colspan="3">
          User Types</td>
      </tr>
      <tr>
        <td class="T" colspan="3" style="height: 15px">
          A User has an Administrative Type which determines the user's permissions.&nbsp;
          <br />
          A MASTER User is restricted by any State Code, Design Code, and/or Organization
          Code assigned. If none are assigned, the user has unrestricted permissions.<br />
          An ADMIN User has permission to administer the data for a single State only, and
          possibly enter politician information if the user is given data entry authority,
          i.e. Interns.
          <br />
          A COUNTY User has permission to administer the data only for a single county within
          a State.
          <br />
          A LOCAL User can only administer the data for a single local district within a county
          of a State. &nbsp;<br />
          A DESIGN User has permission over the design for a design code.
          <br />
          The other two user types have not yet been implemented.</td>
      </tr>
      <tr>
        <td class="HSmall" colspan="3">
          User can Enter Politician Data like Interns</td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          <asp:CheckBox ID="CheckBox_Has_Data_Entry_Authority" runat="server" CssClass="Checkboxes"
            Text="User has politician data entry authority" />&nbsp;<asp:Button ID="Button_Update_Data_Entry_Authority"
              runat="server" CssClass="Buttons" OnClick="Button_Update_Data_Entry_Authority_Click" Text="Update User' Data Entry Authority"
              Width="200px" /></td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          Check this checkbox if the user is an intern. Click the button to update an existing
          user's data entry authority.</td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          <strong>User Type:</strong><asp:RadioButtonList ID="RadioButtonList_User_Type" runat="server" CssClass="CheckBoxes" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList_User_Type_SelectedIndexChanged1" RepeatDirection="Horizontal">
            <asp:ListItem Value="MASTER">Master</asp:ListItem>
            <asp:ListItem Value="ADMIN">ADMIN</asp:ListItem>
            <asp:ListItem Value="COUNTY" Enabled="False">County</asp:ListItem>
            <asp:ListItem Value="LOCAL" Enabled="False">Local</asp:ListItem>
            <asp:ListItem Value="DESIGN">DESIGN</asp:ListItem>
            <asp:ListItem Value="ORGANIZATION">Organization</asp:ListItem>
            <asp:ListItem Value="ISSUES" Enabled="False">Issues</asp:ListItem>
            <asp:ListItem Value="PARTIES" Enabled="False">Parties</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          Selecting a radio button will create a user
        </td>
      </tr>
      <tr>
        <td colspan="3" class="T">
          <asp:Button ID="Button_Different_User" runat="server" CssClass="Buttons" OnClick="Button_Different_User_Click"
            Text="Different User" Width="90px" />
          <asp:Button ID="Button_Delete" runat="server" CssClass="Buttons" Text="Delete User"
            Width="90px" OnClick="Button_Delete_Click" /></td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          Click the first button to add or update a different user. Use the second if you
          are sure you want to delete the user.</td>
      </tr>
      <tr>
        <td class="HSmall" colspan="3">
          Current Users and Permissions</td>
      </tr>
    </table>
      <!-- Table -->
      <!-- Table -->
    <table class="tableAdmin" id="TableNewUser2" cellspacing="0" cellpadding="0" runat=server>
    <tr>
      <td class="TBold" >
        Users:</td>
      <td class="TBold" >
        User. Type<br />
        -Data Entry<br />
        Authority
        Y/N:</td>
    <td class="TBold">
      Design<br />
      Codes:</td>
    <td class="TBold">
      Organization<br />
      Codes:</td>
      <td class="TBold">
        State<br />
        Codes:</td>
      <td class="TBold">
        County:</td>
      <td class="TBold">
        Local:</td>
    </tr>
      <tr>
        <td class="TBold" >
          <asp:Label ID="Label_UserName" runat="server" CssClass="TBoldColor">User</asp:Label></td>
        <td class="TBold" >
          <asp:Label ID="Label_Type" runat="server" CssClass="TBoldColor">T</asp:Label></td>
        <td class="TBold">
          <asp:Label ID="Label_Design" runat="server" CssClass="TBoldColor">Design</asp:Label></td>
        <td class="TBold">
          <asp:Label ID="Label_Organization" runat="server" CssClass="TBoldColor">Org</asp:Label></td>
        <td class="TBold">
          <asp:Label ID="Label_State" runat="server" CssClass="TBoldColor">S</asp:Label></td>
        <td class="TBold">
          <asp:Label ID="Label_County" runat="server" CssClass="TBoldColor">County</asp:Label></td>
        <td class="TBold">
          <asp:Label ID="Label_Local" runat="server" CssClass="TBoldColor">Local</asp:Label></td>
      </tr>
      <tr>
        <td class="TBold" valign="top" >
          <asp:ListBox ID="ListBox_Users" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="140px" OnSelectedIndexChanged="ListBox_Users_SelectedIndexChanged"></asp:ListBox></td>
        <td class="TBold"  valign="top">
          <asp:ListBox ID="ListBox_Type" runat="server" CssClass="TextBoxInput" Enabled="False"
            Height="850px" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ListBox_Type_SelectedIndexChanged"></asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_Design" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="100px" OnSelectedIndexChanged="ListBox_Design_SelectedIndexChanged"></asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_Organization" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="100px" OnSelectedIndexChanged="ListBox_Organization_SelectedIndexChanged"></asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_State" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="50px" OnSelectedIndexChanged="ListBox_State_SelectedIndexChanged"></asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_County" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="100px"></asp:ListBox></td>
        <td valign="top">
          <asp:ListBox ID="ListBox_Local" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            Height="850px"
            Width="100px"></asp:ListBox></td>
      </tr>
			</TABLE>

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
