<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
 CodeBehind="Default.aspx.cs" Inherits="Vote.Admin.Default" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    #body .master-main-content
    {
      margin-bottom: 20px;

    }

    body.admin-page h3.multi-county-message
    {
      margin: -15px 0 0 0;
      max-width: 800px;
      line-height: 120%;
    }

    #body .jurisdiction-links .head
    {
      font-size: 14pt;
      text-align: center;
      background: #666;
      color: #fff;
      padding: 8px 0;
      margin-top: 20px;
    }

    #body .jurisdiction-links
    {
      font-size: 10pt;
    }

    #body .jurisdiction-links .the-links
    {
      border: 1px solid #666;
      padding: 5px;
      -moz-column-count: 5;
      -webkit-column-count: 5;
      column-count: 5;
    }

    #body .local-links .the-links
    {
      -moz-column-count: 4;
      -webkit-column-count: 4;
      column-count: 4;
    }

    #body .jurisdiction-links a
    {
      text-decoration: none;
      display: block;
    }

    #body .the-links a
    {
      break-inside: avoid-column;
      -webkit-column-break-inside: avoid; 
      -moz-column-break-inside:avoid; 
      -o-column-break-inside:avoid; 
      -ms-column-break-inside:avoid; 
      /*column-break-inside: avoid;*/
      margin-left: 10px;
      text-indent: -10px;
    }

    #body .add-delete-districts
    {
      border: 1px solid #666;
      padding: 5px;
      border-top: none;
      font-weight: bold;
      text-align: center;
      background-color: #ddd;
    }

    #body table.master-only
    {
      margin-top: 20px;
      border: 1px solid #666;
      padding: 5px;
    }

    #body table.master-only td
    {
      font-size: 14pt;
      text-align: center;
      background: #666;
      color: #fff;
      padding: 8px 0;
      margin-top: 20px;
      font-weight: normal;
      border: none;
    }

    .local-message
    {
      font-size: 12pt;
      margin-top: 20px;
    }

    
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <h1 id="H1" EnableViewState="false" runat="server"></h1>
  <h3 id="MuliCountyMessage" class="multi-county-message hidden" runat="server"></h3>
  
  <div id="LocalMessage" class="local-message" runat="server" Visible="False">Please make a selection from the menu above.</div>
    
  <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

  <div id="UpdateControls" class="update-controls" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

  <table class="tableAdmin" id="TitleTable">
    <tr>
      <td class="T">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  
  <div class="county-links jurisdiction-links" id="CountyLinkContainer" runat="server">
    <div class="head">Links to County Administration Pages</div>
    <div class="the-links" id="CountyLinkList" runat="server"></div>
  </div>
  
  <div class="local-links jurisdiction-links" id="LocalLinksContainer" runat="server">
    <div class="head">Links to Local Districts, Cities and Towns</div>
    <div class="the-links" id="LocalLinkList" runat="server"></div>
    <asp:HyperLink id="AddOrDeleteDistricts" class="add-delete-districts" runat="server">Add or Delete Local Districts, Cities or Towns</asp:HyperLink>
  </div>
  
  <div id="MasterOnlySection" runat="server">
    
  <table class="tableAdmin master-only" id="TableMasterOnly" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="H">
        Master Users Only
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableBallotDesign" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkBallotDesign" runat="server" CssClass="HyperLink" Target="edit"
          ToolTip="Special characteristics on ballots, like whether to indicate the incumbent, how nicknames are presented...">[HyperLinkBallotDesign]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        Define ballot characteristics,
        such as whether to indicate the incumbent and how nicknames are presented.
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableParties" cellspacing="0" cellpadding="0"
    runat="server">
    <tr>
      <td class="HSmall">
        <asp:HyperLink ID="HyperLinkPoliticalParties" runat="server" 
          CssClass="HyperLink" Target="edit">[HyperLinkPoliticalParties]</asp:HyperLink>
      </td>
    </tr>
    <tr>
      <td class="T">
        Edit political party information,
        including email addresses.
      </td>
    </tr>
  </table>

  <table class="tableAdmin" id="TableNotes" cellspacing="0" cellpadding="0" runat="server">
    <tr>
      <td class="HSmall">
        &nbsp;</td>
    </tr>
    <tr>
      <td class="HSmall">
        Status Notes of Statewide, Judicial and County Elected Offices and Incumbents
      </td>
    </tr>
    <tr>
      <td class="T">
        Use these notes to keep track of the status of the various electoral data and districts.
      </td>
    </tr>
    <tr>
      <td class="T">
        Statewide:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficesStatusStatewide" 
          runat="server" TextMode="MultiLine"
          Height="27px" Width="800px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        Judicial: &nbsp;&nbsp; &nbsp;<user:TextBoxWithNormalizedLineBreaks 
          ID="TextBoxOfficesStatusJudicial" runat="server"
          TextMode="MultiLine" Height="29px" Width="800px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        Counties:&nbsp;
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficesStatusCounties" runat="server" TextMode="MultiLine"
          Width="800px" Height="26px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Button ID="ButtonRecordOfficesStaus" runat="server" OnClick="ButtonRecordOfficesStaus_Click"
          Text="Record" CssClass="Buttons" Width="160px" />
        Click to record or update the above notes.
      </td>
    </tr>
  </table>
  
  </div>
    </ContentTemplate>
  </asp:UpdatePanel>
  </div>
</asp:Content>
