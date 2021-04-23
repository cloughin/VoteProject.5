<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Offices.aspx.cs" Inherits="Vote.Admin.OfficesPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet"/>
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: bold;
      color: #373737;
      font-size: 11px;
      height: 13px;
      padding-left: 5px;
      padding-right: 0;
      padding-top: 0;
      padding-bottom: 0;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <h1 id="H1" runat="server"></h1>
  
  <asp:UpdatePanel ID="UpdatePanelMsg" UpdateMode="Always" runat="server">
    <ContentTemplate>
      <table id="MsgTable" class="tableAdmin">
        <tr>
          <td align="left" class="T">
            <asp:Label ID="Msg" CssClass="label-msg" EnableViewState="False" runat="server"></asp:Label>
          </td>
        </tr>
      </table>
    </ContentTemplate>
  </asp:UpdatePanel>


  <asp:UpdatePanel ID="UpdatePanel" UpdateMode="Conditional" runat="server">
    <ContentTemplate>

      <table id="TopTable" class="tableAdmin">
         <tr>
          <td align="center" class="H">
            Add and Edit Elected Offices
          </td>
        </tr>
        <tr>
          <td align="left" class="T">
            Because of the large number of elected offices, offices are grouped in the various
            categories show below. Select a radio button below to obtain a report of all the
            elected offices in that office category.&nbsp;The offices are not for any particular
            election. To obtain the office contests in a particular election navigate to the
            Elections Page and select an election. Use the links on the report presented to
            perform these functions:<br />
            <strong>Editing Offices:</strong> Click an <span style="text-decoration: underline">
              Office Title</span> to edit an office, like changing the office title, position
            on ballot, etc.<br />
            <strong>Identify Current Office Incumbent:</strong> Click an <span style="text-decoration: underline">
              Office Title </span>
            <br />
            <strong>Add Office(s):</strong> Offices can only be added in office categories where
            all the offices have not yet been identified. For these categories use the <span
              style="text-decoration: underline">Add Office Category</span> link in the report
            headers to add an office. There are no links when all the elected offices in a category
            have been defined.<strong> </strong>Be careful to select the correct category because
            this determines where the office is placed on ballots.<br />
            <strong>Number in Parentheses::</strong> The number in the parentheses for each
            office category are the number of elected offices currently identified in that category.
            These numbers should match the number of offices presented on the report when a
            radio button is selected.
          </td>
        </tr>
      </table>

      <table id="OfficeCategoriesTable" class="tableAdmin">
        <tr>
          <td class="H">
            Report of Elected Offices in an Office Category<tr>
              <td class="T">
                Select a radio blutton below to obtain a report of all the elected offices
              in that category. Office categories that have no offices are not shown.<tr>
          <td align="center" class="RadioButtons">
            <asp:RadioButtonList ID="RadioButtonListOfficeClass" CssClass="input-list-report" runat="server" AutoPostBack="True"
              CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="RadioButtonListOffice_SelectedIndexChanged"
              RepeatColumns="1">
            </asp:RadioButtonList>
          </td>
        </tr>
      </table>

      <asp:UpdatePanel ID="MasterUpdatePanel" runat="server">
        <ContentTemplate>
          
          <table id="MasterOnlyTable" class="tableAdmin" visible="False" runat="server">
            <tr>
              <td class="H">
                MASTER ONLY
              </td>
            </tr>
            <tr>
              <td class="HSmall">
                Reports of All Offices
              </td>
            </tr>
            <tr>
              <td class="T">
                These reports make it easy to compare the offices on election roster with the offices
                in the database.
              </td>
            </tr>
            <tr>
              <td class="TBold">
                <asp:HyperLink ID="HyperLinkOffices" runat="server" CssClass="HyperLink" Target="view">In State, Counties and Local Districts</asp:HyperLink>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:HyperLink ID="HyperLinkStateOffices" runat="server" CssClass="HyperLink" Target="view">In State Only</asp:HyperLink>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:HyperLink ID="HyperLinkCountyOffices" runat="server" CssClass="HyperLink"
                  Target="view">In All Counties Only</asp:HyperLink>
              </td>
            </tr>
            <tr>
              <td class="TBold">
                <asp:HyperLink ID="HyperLinkLocalOffices" runat="server" CssClass="HyperLink" Target="view">In All Local Districts Only</asp:HyperLink>
              </td>
            </tr>
            <tr>
              <td class="HSmall">
                ALL Offices Identified in Office Classes (to prohibit office additions in various
                classes)
              </td>
            </tr>
            <tr>
              <td class="T">
                Check or Uncheck the various offices classes to indicate whether ALL offices in 
                an office class has been identified or not.
                <br />
                <strong>Prohibit the Additon of Offices: </strong>Check an office category to 
                indicate that all the offices in that category have all been identified and NO 
                NEW OFFICES CAN BE ADDED in that category.
                <br />
                <strong>Add Offices:</strong> Uncheck an office category to indicate that NOT 
                all the offices in that category have been identified and NEW OFFICES CAN BE 
                ADDED. Office titles are links to enable the adding of offices in that category. 
                Then click the office title link.</td>
            </tr>
            <tr>
              <td class="RadioButtons">
                <asp:CheckBoxList ID="CheckBoxListOfficeClassAllIdentified" CssClass="input-list-ajax" runat="server" AutoPostBack="True"
                  OnSelectedIndexChanged="CheckBoxListOfficeClassAllIdentified_SelectedIndexChanged">
                </asp:CheckBoxList>
              </td>
            </tr>
          </table>

        </ContentTemplate>
      </asp:UpdatePanel>

      <table id="ReportTable" class="tableAdmin" runat="server">
        <tr>
          <td class="H">
            <asp:Label ID="LabelOfficesReportTitle" runat="server"></asp:Label>
          </td>
        </tr>
        <tr>
          <td class="T">
            A category of offices is not shown when all the offices in a category have been
            identified but there are no offices. Many county and local office categories are
            marked as being all identified because have no offices in the category.
          </td>
        </tr>
      </table>

      <div class="HLargeColor">
        <asp:PlaceHolder ID="ReportPlaceHolder" EnableViewState="False" runat="server"></asp:PlaceHolder>
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>

</asp:Content>
