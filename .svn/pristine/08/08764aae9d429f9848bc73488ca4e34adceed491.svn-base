<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
Codebehind="Politicians.aspx.cs" Inherits="Vote.Admin.PoliticiansPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <h1 id="H1" runat="server"></h1>

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
      
    <table id="TopTable" class="tableAdmin">
      <tr>
        <td align="left" style="width: 704px" class="T">
          <asp:Label ID="Msg" CssClass="label-msg" EnableViewState="False" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td align="center" class="H" style="width: 704px">
          Add and Edit Politicians (Candidates and/or Elected Officials)</td>
      </tr>
      <tr>
        <td align="left" class="T" style="width: 704px">
          The number in parentheses for the first radio button is the total number of politicians
          in the database.
          To make it easier to find, edit or update politicians, politicians are grouped in the various
          categories shown by the other radio buttons. When a button
          is selected a report of the politicians in that category will be presented at the bottom of this form. The politician count in the first radio button will not be
          the summation politician counts in the other radio buttons. That is because you
          need to navigate to a specific state, county or local legislative district to obtain
          reports of the politicians in those legislative demographics.
          <br />
          <br />
          <strong>If you do not know the Office Title,</strong> select <a id="UpdatePoliticiansLink" runat="server"><i>Politicians &gt;</i> | <i>Update Politicians &gt;</i> | <i>&lt;state&gt;</i></a> on the main navigation bar.
          <br />
          <br />
        </td>
      </tr>
      <tr>
        <td class="HSmall">
          Select a Category of Politicians to Report</td>
      </tr>
      <tr>
        <td class="T">
          Select
          a radio button of the politician
          category and sort order to obtain a report of all the politicians in that category.&nbsp; On the report, links are provided to edit and add politicians.
          The politicians presented
          are all candidtes who ever sought an office in a particular category, not for any
          particular election. The category of a politician is the office the
          politician last sought. To obtain the office contests
          and politicians in a particular election, navigate to the Elections Page and select an
          election.
          <br />
          <strong>Number in Parenthesis:</strong> The number in the parentheses for each category
          of offices
          is the number of politicians currently identified in that category. They will 
          be presented on the report when a radio button
          is selected. Construction of the report my take some time if the number is 
          large.</td>
      </tr>
      <tr>
        <td align="center" class="RadioButtons">
          <asp:RadioButtonList ID="RadioButtonListOfficeClass" CssClass="input-list-report" runat="server" AutoPostBack="True"
            CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="RadioButtonListOffice_SelectedIndexChanged"
            RepeatColumns="1">
          </asp:RadioButtonList>  
        </td>
      </tr>
    </table>

    <table id="SortOrderTable" class="tableAdmin" runat="server">
      <tr>
        <td class="HSmall">
          Select
          Sort Order of the Report</td>
      </tr>
      <tr>
        <td class="T">
          Use the radio buttons below to present the politician names sorted either by office
          or by last name.</td>
      </tr>
      <tr>
        <td align="center">
           <asp:RadioButtonList ID="RadioButtonListSort" runat="server" CssClass="RadioButtons input-list-report"
            RepeatDirection="Horizontal" CellPadding="0" CellSpacing="2" AutoPostBack="True"
            OnSelectedIndexChanged="RadioButtonListSort_SelectedIndexChanged" Width="289px">
            <asp:ListItem Value="Office">Sort by Office</asp:ListItem>
            <asp:ListItem Value="Name" Selected="True">Sort by Last Name</asp:ListItem>
          </asp:RadioButtonList>
        </td>
      </tr>
    </table>

    <table id="ConsolidateTable" runat="server" border="0" cellpadding="0"
      cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          MASTER ONLY</td>
      </tr>
      <tr>
        <td class="HSmall">
          <asp:HyperLink ID="HyperLinkConsolidate" runat="server" CssClass="HyperLink" Target="edit"
            NavigateUrl="~/Admin/PoliticiansConsolidate.aspx">Consolidate Politicians</asp:HyperLink></td>
      </tr>
      <tr>
        <td align="left" class="T">
          When you click the above link, you wll be presented with a form which will allow
          you to consolidate one or two politician's
          data and pictures into another politician
          or a new politician.
          This should be done if the same person is listed more than
          once in the report below
          or if Politician Id contains special language characters, like the a in Vázquez
          and Vàzquez.</td>
      </tr>
    </table>    

    <table id="AddOfficeTable" class="tableAdmin" runat="server">
      <tr>
        <td align="left" class="HSmall">
          <asp:HyperLink ID="HyperLinkAddOffice" CssClass="HyperLink" Target="office" 
            runat="server">[HyperLink_Add_Office]</asp:HyperLink></td>
      </tr>
      <tr>
        <td id="AddOfficeTableInstructions" align="left" class="T">
          Use the link above to add a new elected office in this category of offices.</td>
      </tr>
    </table>

    <table id="PoliticiansMaintTable" class="tableAdmin" runat="server">
      <tr>
        <td align="left" class="H">
          Politicians Maintenance</td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          Add Politicians</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click an <span style="text-decoration: underline">
            Office Title</span> link to add politicians as incumbents or future candidate.
          <strong>Note:</strong> If you are in the process of defining a new election you
          SHOULD NOT use these links. A different procedure is used to add politicians as
          a new election is being defined. 
        </td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          Edit Politicians</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click a <span style="text-decoration: underline">Politician Name</span> link, in the left hand column of the report, to be presented with a form where you can change
          the data and images for that politician.</td>
      </tr>
      <tr>
        <td class="HSmall">
          Edit Offices</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Click on an <span style="text-decoration: underline">
            Office Title</span> in the right column of the report to change information about
          that office.</td>
      </tr>
    </table>

    <table id="ReportTable" runat="server" class="tableAdmin">
      <tr>
        <td align="left" class="H">
          Report of
          <asp:Label ID="LabelPoliticiansReportTitle" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
        </td>
      </tr>
    </table>

    <div class="HLargeColor">
      <asp:PlaceHolder ID="ReportPlaceHolder" runat="server" EnableViewState="False"></asp:PlaceHolder>
    </div>

    </ContentTemplate>
  </asp:UpdatePanel>

</asp:Content>


