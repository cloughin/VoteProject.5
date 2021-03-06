<%@ Page Language="C#" AutoEventWireup="true" 
 CodeBehind="Ballot.aspx.cs" Inherits="Vote.Admin.BallotPage" %>

<html><head><title></title></head><body></body></html>

<%--
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>

    <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="middle" align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td valign="middle" align="left" class="T">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>

    <table id="TableBallotDesign" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="HSmall">
          Incumbent Indication</td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          <asp:RadioButtonList ID="RadioButtonListIncumbent"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListIncumbent_SelectedIndexChanged"
            RepeatDirection="Horizontal" Width="673px">
            <asp:ListItem Value="True">Show incumbent with * following name</asp:ListItem>
            <asp:ListItem Value="False">Don't show any indication of the incumbent</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          Ballot names can be presented with or without an incumbent indication.</td>
      </tr>

      <tr style="font-size: 11px; background-color: #ffffff">
        <td align="left" class="HSmall">
          Enclosure of Nicknames</td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td align="left" class="T">
          <asp:RadioButtonList ID="RadioButtonListEncloseNickname"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListEncloseNickname_SelectedIndexChanged"
            RepeatDirection="Horizontal" Width="673px">
            <asp:ListItem Value="D">Double Quotes</asp:ListItem>
            <asp:ListItem Value="S">Single Quotes</asp:ListItem>
            <asp:ListItem Value="P">Parens ()</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td align="left" class="T">
          Nicknames on ballots can be enclosed within double quotes, singel quotes or parenthesis.</td>
      </tr>

      <tr>
        <td class="HSmall">
          Unopposed Candidates</td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          <asp:RadioButtonList ID="RadioButtonListUnopposed"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListUnopposed_SelectedIndexChanged"
            RepeatDirection="Horizontal" Width="673px">
            <asp:ListItem Value="True">Show unopposed candidate contests</asp:ListItem>
            <asp:ListItem Value="False">Don't show unopposed candidate contests</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          Contests with unopposed candidates can be shown or hidden on ballots.</td>
      </tr>

      <tr>
        <td class="HSmall">
          Write-In Candidates</td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          <asp:RadioButtonList ID="RadioButtonListWriteIn"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListWriteIn_SelectedIndexChanged"
            RepeatDirection="Horizontal" Width="673px">
            <asp:ListItem Value="True">Show a write-in line for each contest</asp:ListItem>
            <asp:ListItem Value="False">Don't show a write-in line for each contest</asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="T">
          Contests can be shown with or without a write-in line.</td>
      </tr>

      <tr style="font-size: 11px; background-color: #ffffff">
        <td class="HSmall">
          State
          Name on Ballots</td>
      </tr>
      <tr>
        <td class="T">
          The State name that will appear on ballots is shown below.
          Use the textbox to change.
          For example Virginia could be presented as Commonwealth
          of Virginia. Clicking anywhere
          outside the textbox will record the change.</td>
      </tr>
      <tr>
        <td class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxBallotName" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            OnTextChanged="TextBoxBallotName_TextChanged" Width="588px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>

      <tr>
        <td class="T">
        </td>
      </tr>
    </table>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>