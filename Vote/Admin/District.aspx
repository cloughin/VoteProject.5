<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="District.aspx.cs" Inherits="Vote.Admin.DistrictPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

        <h1 id="H1" runat="server"></h1>
        <!-- Table -->
        <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
          <tr>
            <td align="left" class="HLarge">
              <asp:Label ID="LabelPageTitle" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td class="T">
              <asp:Label ID="Msg" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td class="H">
              Change District Name</td>
          </tr>
          <tr>
            <td class="T">
              Make any name change in the textbox provided then click the Submit Button.</td>
          </tr>
          <tr>
            <td class="HSmall">
              District Name:
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDistrict" runat="server" CssClass="TextBoxInput" Width="300px"></user:TextBoxWithNormalizedLineBreaks>
              <asp:Button ID="ButtonUpdateJudicialDistrict" runat="server" Text="Submit"
                          OnClick="ButtonUpdateJudicialDistrict_Click" CssClass="Buttons" Width="150px" /></td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster1" cellspacing="0" cellpadding="0" border="0" runat="server">
          <tr>
            <td class="H">
              District Maintenance</td>
          </tr>
          <tr>
            <td class="T">
              Use the controls below to add, edit or define the counties that comprise each district.
              To define or edit the elected offices in a district navigate
              to the Judicial Districts
              Page.</td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster4" cellspacing="0" cellpadding="0" border="0" runat="server">
          <tr>
            <td class="H" colspan="1">
              Navigate to Add or Edit Existing Districts
            </td>
          </tr>
          <tr>
            <td class="T" valign="top">
              <asp:HyperLink ID="HyperLinkHellpNivigate" runat="server" NavigateUrl="/Admin/HelpNavigate.htm"
                             Target="Help">Navigate to Add or Edit Existing Districts Help Notes</asp:HyperLink></td>
          </tr>
          <tr>
            <td class="T" valign="top">
              The District Code consists of 3 digits (or no digits) and/or 1 to 4 alphabetic characters
              (or no alpha characters). Examples of valid District Codes are: 023, 023A, DAL,
              023DALS. Examples of invalid codes would be: 23, 2DAL (if digits
              are present there
              needs to be 3.</td>
          </tr>
          <tr>
            <td class="T" valign="top">
              &nbsp;<strong>3 Digit District Code:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBoxDistrictCode" runat="server" CssClass="TextBoxInput" Width="35px"></user:TextBoxWithNormalizedLineBreaks>&nbsp; <strong>
                                                                                                                                                                                                                              1 to 4 Character Alpaha District Code:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBoxAlphaCode" runat="server"
                                                                                                                                                                                                                                                                                                                   CssClass="TextBoxInput" Width="52px"></user:TextBoxWithNormalizedLineBreaks>
              <asp:Button ID="ButtonGetDistrict" runat="server" OnClick="ButtonGetDistrict_Click"
                          Text="Get this District" Width="150px" CssClass="Buttons" />&nbsp;&nbsp;&nbsp;
            </td>
          </tr>
          <tr>
            <td class="TLarge">
              <asp:Button ID="ButtonGetPrevious" runat="server" Text="Get Previous District" OnClick="ButtonGetPrevious_Click"
                          Width="150px" CssClass="Buttons" />&nbsp;
              <asp:Button ID="ButtonGetNext" runat="server" Text="Get Next District" Width="150px"
                          OnClick="ButtonGetNext_Click" CssClass="Buttons" /></td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster5" cellspacing="0" cellpadding="0"
               border="0" runat="server">
          <tr>
            <td class="H" colspan="2">
              Add New District(s)</td>
          </tr>
          <tr>
            <td class="TSmall" colspan="2">
            </td>
          </tr>
          <tr>
            <td class="T">
              <strong>District Name: </strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBox1" runat="server" CssClass="TextBoxInput" Width="287px"></user:TextBoxWithNormalizedLineBreaks>
            </td>
            <td class="T" width="100%">
              <asp:HyperLink ID="HyperLinkAddMultiCountyDistricts" runat="server" CssClass="HSmall"
                             NavigateUrl="/Admin/HelpAddMultiCountyDistricts.htm" Target="Help">Add Multi-County Help Notes</asp:HyperLink>
              <asp:HyperLink ID="HyperLinkAddJudicialDistricts" runat="server" CssClass="HSmall"
                             NavigateUrl="/Admin/HelpAddJudicialDistricts.htm" Target="Help">Add Judicial Districts Help Notes</asp:HyperLink></td>
          </tr>
          <tr>
            <td class="T">
              <strong>
                Optional Office Title: </strong>
              <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeTitle" runat="server" CssClass="TextBoxInput" Width="320px"></user:TextBoxWithNormalizedLineBreaks></td>
            <td class="T" width="100%">
              Use only when adding a district.</td>
          </tr>
          <tr>
            <td class="T">
              <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click" Text="Add District"
                          Width="200px" CssClass="Buttons" /></td>
            <td class="T" width="100%">
              Click this button to add this district for the counties checked. An office will
              also be added if an Office Title is provided in the textbox above.</td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster6" cellspacing="0" cellpadding="0" border="0" runat="server">
          <tr>
            <td class="H" colspan="2">
              Edit or Delete District</td>
          </tr>
          <tr>
            <td class="HSmall">
              <asp:Button ID="ButtonRecord" runat="server" Text="Record this District's Changes"
                          OnClick="ButtonRecord_Click" Width="200px" CssClass="Buttons" /></td>
            <td class="HSmall" width="100%">
              <asp:HyperLink ID="HyperLinkEditDelete" runat="server" NavigateUrl="/Admin/HelpEditDistricts.htm"
                             Target="Help">Edit or Delete Help Notes</asp:HyperLink></td>
          </tr>
          <tr>
            <td class="HSmall">
              <asp:Button ID="ButtonDelete" runat="server" OnClick="ButtonDelete_Click" Text="Delete this District"
                          Width="200px" CssClass="Buttons" /></td>
            <td class="HSmall" width="100%">
            </td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster7" cellspacing="0" cellpadding="0" border="0" runat="server">
          <tr>
            <td class="H" colspan="2">
              Counties in District</td>
          </tr>
          <tr>
            <td class="HSmall">
              <asp:Button ID="ButtonClearCheckboxes" runat="server" OnClick="ButtonClearCheckboxes_Click"
                          Text="Clear All County Checkboxes" Width="200px" CssClass="Buttons" /></td>
            <td class="HSmall" width="100%">
            </td>
          </tr>
        </table>
        <!-- Table -->
        <table class="tableAdmin" id="TableMaster8" cellspacing="0" cellpadding="0"
               border="0" runat="server">
          <tr>
            <td>
              <asp:CheckBoxList ID="CheckBoxListCounties" runat="server" CssClass="CheckBoxes"
                                RepeatDirection="Horizontal" RepeatColumns="6">
              </asp:CheckBoxList></td>
          </tr>
        </table>
    <!-- Table -->

    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
