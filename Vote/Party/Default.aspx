<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Default.aspx.cs" Inherits="Vote.Party.Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <style>
    table.tableAdmin strong em {
      color: red;
      font-style: normal;
    }
    table.tableAdmin td.tdReportGroupHeading {
      vertical-align: middle;
    }
    table.tableAdmin td.tdReportDetailSmall {
      line-height: 100%;
    }
    table.tableAdmin td span {
      white-space: nowrap;
    }
    table.tableAdmin td .head {
      color: green;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>

    <asp:UpdatePanel ID="UpdatePanel" runat="server">
      <ContentTemplate>
        
        <table id="TitleTable" cellpadding="0" cellspacing="0" class="tableAdmin">
          <tr>
            <td align="left" class="T" valign="middle">
              <asp:Label ID="Msg" runat="server"></asp:Label></td>
          </tr>
        </table>

        <table class="tableAdmin" Visible="false" id="Table_Election_Candidates" cellspacing="0" cellpadding="0" runat=server>
          <tr>
            <td align="left" class="HSmall" valign="middle" colspan="2">
              &nbsp;Party Candidates for
              <asp:Label ID="Label_Election_Candidates" runat="server"></asp:Label></td>
          </tr>
          <tr>
            <td align="left" class="T" valign="middle">
              <asp:Button ID="Button_Election_Candidates" runat="server" CssClass="Buttons" OnClick="Button_Election_Candidates_Click"
                Text="Party Candidates in this Election" Width="300px" /></td>
            <td align="left" class="T" valign="middle">
              Click this button to obtain a list of all your party's candidates in this 
              election. Links are provided to enter or update the information about each elected 
              representative on our website.</td>
          </tr>
          <tr>
            <td align="left" class="T" valign="middle">
              &nbsp;</td>
          </tr>
        </table>

        <table class="tableAdmin" id="Table_Elected_Representatives" cellspacing="0" cellpadding="0">
          <tr>
            <td align="left" class="HSmall" colspan="2" valign="middle">
              Currently Elected Party Members</td>
          </tr>
          <tr>
            <td align="left" class="T" valign="middle">
              <asp:Button ID="Button_Elected_Representatives" runat="server" CssClass="Buttons"
                OnClick="Button_Elected_Representatives_Click" Text="Party Elected Representatives"
                Width="300px" /></td>
            <td align="left" class="T" valign="middle">
              Click this button to obtain a list of all your party&#39;s elected officials. Links are
              provided to enter or update the information about each elected representative on our website.</td>
          </tr>
          <tr>
            <td align="left" class="T" valign="middle">
              &nbsp;</td>
          </tr>
          </table>
          
          
        <div id="ReportContainer" runat="server" Visible="false">
          <table class="tableAdmin" id="ReportLabelTable" cellspacing="0" cellpadding="0" border="0" runat=server>
            <tr>
              <td align="left" class="H">
                <asp:Label ID="ReportLabel" runat="server"></asp:Label></td>
            </tr>
          </table>

          <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"></asp:PlaceHolder>
        </div>

      </ContentTemplate>
    </asp:UpdatePanel>

  </div>
</asp:Content>
