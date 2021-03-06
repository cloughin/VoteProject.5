<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
Codebehind="MaintOffices.aspx.cs"  MasterPageFile="~/Master/Admin.Master"
Inherits="Vote.Master.MaintOfficesPage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>

<%--
<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

      <h1 id="H1" runat="server"></h1>
    <table class="tableAdmin" id="TableTop" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableMain" cellspacing="0" cellpadding="0" >
      <tr>
        <td align="left" class="HSmall" colspan="2">
          View 1st Office for OfficeKey:</td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 236px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_OfficeKey1" runat="server" CssClass="InputTextBox" Width="300px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td align="left" class="T" width="100%">
          <asp:Button ID="Button_View_Office1" runat="server" CssClass="Buttons" OnClick="Button_View_Office1_Click"
            Text="View Office Data" Width="150px" /></td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          <asp:Label ID="Label_Office1" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Delete Office Above</td>
      </tr>
      <tr>
        <td style="width: 236px">
          <asp:Button ID="ButtonDelete" runat="server" CssClass="Buttons" Font-Bold="True"
            OnClick="ButtonDelete_Click1" Text="Delete Office" Width="200px" /></td>
        <td align="left" class="T">
          Enter the OfficeKey of the office to delete in the 1st Office Key Textbox above.
          Then click
          this button completely delete this office.<br />
          <strong>CAUTION!!</strong> Deleting an office will delete the office and all&nbsp;
          information about the office including all elections the office was in and the candidates
          for these offices. Probably the only reason to delete an office is if you just created
          an invalid office (i.e the Office Title was incorrect or the office was identified
          in the wrong electoral jurisdiction.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          <strong>View 2nd Office to Compare Office for OfficeKey:</strong></td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 236px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_OfficeKey2" runat="server" CssClass="InputTextBox" Width="300px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          </td>
        <td align="left" class="T" >
          <asp:Button ID="Button_View_Office2" runat="server" CssClass="Buttons" OnClick="Button_View_Office2_Click"
            Text="View Office Data" /></td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          <asp:Label ID="Label_Office2" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Consolidate the 1st Office into the 2nd Office, Deleting the 1st Office</td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 236px">
          <asp:Button ID="Button_Consolidate" runat="server" CssClass="Buttons" OnClick="Button_Consolidate_Click"
            Text="Consolidate 1st Office into 2nd Office" Width="300px" /></td>
        <td align="left" class="T">
          <strong>CAUTION!! </strong>The office title may change.</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2">
          This operation is useful when there more than one OfficeKey representing the 
          same office, for example there was DCUSHouse1 officelevel 3 and 
          DCUnitedStatesSenator officelevel 4.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall" colspan="2">
          Change OfficeKey</td>
      </tr>
      <tr>
        <td align="left" class="TBold" style="width: 236px">
          From:</td>
        <td align="left" class="TBold">
          To:</td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 236px; height: 13px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_ChangeOfficeKey_From" runat="server" CssClass="InputTextBox"
            Width="300px"></user:TextBoxWithNormalizedLineBreaks></td>
        <td align="left" class="T" style="height: 13px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_ChangeOfficeKey_To" runat="server" CssClass="InputTextBox"
            Width="300px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 236px; height: 13px">
          <asp:Button ID="Button_Change_OfficeKey" runat="server" CssClass="Buttons" OnClick="Button_Change_OfficeKey_Click"
            Text="Change OfficeKey" Width="300px" /></td>
        <td align="left" class="T" style="height: 13px">
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" >
      <tr>
        <td class="H">
          Change Office
        </td>
      </tr>
    <tr><td class="TBold">
      OfficeKey of Office to Change:
      <user:TextBoxWithNormalizedLineBreaks ID="TextBox_OfficeKey_Change" runat="server" CssClass="InputTextBox"
        Width="300px"></user:TextBoxWithNormalizedLineBreaks></td></tr>
      <tr>
        <td class="HSmall">
          Change Office Class</td>
      </tr>
      <tr>
        <td class="TSmall">
          US_President = 1, US_Senate = 2, US_House = 3
          <br />
          State_Wide = 4, State_Senate = 5, State_House = 6, State_Multi_County_District =
          7
          <br />
          County_Executive = 8, County_Legislative = 9, County_SchoolBoard = 10 County_Commission
          = 11
          <br />
          Local_Executive = 12, Local_Legislative = 13, Local_SchoolBoard = 14, Local_Commission
          = 15
          <br />
          State_Judicial = 16, State_Multi_County_Judicial_District = 17, County_Judicial
          = 18, Local_Judicial = 19
          <br />
          State_Party = 20, State_Multi_County_Party_District = 21, County_Party = 22, Local_Party
          = 23
        </td>
      </tr>
      <tr>
        <td class="TBold">
          Change Class To:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBox_Office_Class" runat="server" AutoPostBack="True" CssClass="InputTextBox"
            OnTextChanged="TextBox_Office_TextChanged" Width="35px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>


    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>