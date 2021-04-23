<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true"
 CodeBehind="Authority.aspx.cs" Inherits="Vote.Admin.AuthorityPage" %>

<html><head><title></title></head><body></body></html>

<%-- @ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" 

 <asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: normal;
      color: #373737;
      font-size: 11px;
      height: 24px;
      padding-left: 5px;
      padding-right: 0;
      padding-top: 0;
      padding-bottom: 0;
    }
  </style>
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

    <table id="StateElectionTable1" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="T" valign="middle">
          Enter 
          the web address, contact names and email address in the textboxes. After each 
          entry hit Enter or click anywhere outside the textbox to record the information.</td>
      </tr>
      <tr>
        <td class="HSmall">
          Web Address
          Url (w/wo http://)</td>
      </tr>
      <tr>
        <td class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxURL" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            OnTextChanged="TextBoxURL_TextChanged" Width="680px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="TBold">
          <br />
          <asp:HyperLink ID="HyperLinkURL" runat="server" Target="view" CssClass="HyperLink">[HyperLinkURL]</asp:HyperLink><br />
          <span style="color: #26466d; background-color: #f5f5f5">&nbsp; </span>
        </td>
      </tr>
      <tr>
        <td class="HSmall">
          <hr />
          Main Contact:</td>
      </tr>
      <tr>
        <td class="style1">
          Name:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxContact" runat="server" 
            CssClass="TextBoxInput" Width="245px" AutoPostBack="True" 
            ontextchanged="TextBoxContact_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
          Title:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContactTitle" runat="server" CssClass="TextBoxInput" 
            Width="320px" AutoPostBack="True" 
            ontextchanged="TextBoxContactTitle_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;Phone:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPhone" runat="server" CssClass="TextBoxInput" 
            Width="210px" AutoPostBack="True" ontextchanged="TextBoxPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="HSmall">
          Email Address Main Contact:</td>
      </tr>
      <tr style="font-size: 8pt">
        <td class="T">
          &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxContactEmail" runat="server" CssClass="TextBoxInput" 
            Width="330px" AutoPostBack="True" 
            ontextchanged="TextBoxContactEmail_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;&nbsp;
          <asp:HyperLink ID="HyperLinkContactEmail" runat="server">[HyperLinkContactEmail]</asp:HyperLink></td>
      </tr>
      <tr>
        <td class="HSmall">
          <hr />
          Alternate Contact:</td>
      </tr>
      <tr>
        <td class="T">
          Name:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContact" 
            runat="server" CssClass="TextBoxInput" Width="245px" AutoPostBack="True" 
            ontextchanged="TextBoxAltContact_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          Title:<user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContactTitle" runat="server" CssClass="TextBoxInput"
            Width="320px" AutoPostBack="True" 
            ontextchanged="TextBoxAltContactTitle_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;Phone:<user:TextBoxWithNormalizedLineBreaks 
            ID="TextBoxAltPhone" runat="server" CssClass="TextBoxInput" 
            Width="210px" ontextchanged="TextBoxAltPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="HSmall">
          Email Address Alternate Contact:</td>
      </tr>
      <tr style="font-size: 8pt">
        <td class="T">
          <strong>&nbsp;</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltEmail" runat="server" CssClass="TextBoxInput" 
            Width="330px" ontextchanged="TextBoxAltEmail_TextChanged"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          <asp:HyperLink ID="HyperLinkAltEMail" runat="server">[HyperLinkAltEMail]</asp:HyperLink></td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          <hr />
          <asp:Button ID="ButtonSwitchContacts" runat="server" CssClass="Buttons" Height="26px"
            OnClick="ButtonSwitchContacts_Click" Text="Switch Main and Alt Contacts" Width="250px" /></td>
      </tr>
      <tr>
        <td align="left" class="T" valign="top">
          Click the button above to switch the information in the textboxes for the main and
          alternate contacts. This button will not record any information, only switch the
          contacts.</td>
      </tr>
      <tr>
        <td class="HSmall" valign="top">
          <hr />
          Election Authority 
          Postal Address:
        </td>
      </tr>
      <tr>
        <td class="TSmallColor" valign="top">
          The textboxes below have been disabled because this information is not used.</td>
      </tr>
      <tr>
        <td class="T" valign="top">
          Address Line 1:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAdressLine1" runat="server" CssClass="TextBoxInput" 
            Width="522px" AutoPostBack="True" 
            ontextchanged="TextBoxAdressLine1_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" valign="top">
          Address Line 2:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddressLine2" runat="server" CssClass="TextBoxInput" 
            Width="522px" AutoPostBack="True" 
            ontextchanged="TextBoxAddressLine2_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" valign="top">
          City, State Zip:&nbsp;
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCityStateZip" runat="server" CssClass="TextBoxInput" 
            Width="523px" AutoPostBack="True" 
            ontextchanged="TextBoxCityStateZip_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="HSmall" valign="middle">
          Notes:</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxNotes" runat="server" CssClass="TextBoxInputMultiLine" Height="200px"
            TextMode="MultiLine" Width="920px" AutoPostBack="True" 
            ontextchanged="TextBoxNotes_TextChanged"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      </table>

    <table id="VoterInformationTable2" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="HSmall">
          Email Address to Contact Election Authority for Voters (w/wo mailto:)</td>
      </tr>
      <tr>
        <td class="T" style="height: 13px">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmail" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
            OnTextChanged="TextBoxEmail_TextChanged" Width="677px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T">
          <br />
          <asp:HyperLink ID="HyperLinkEmail" runat="server" Target="view" 
            CssClass="HyperLink" Enabled="False">[HyperLinkEmail]</asp:HyperLink><br />
          &nbsp;
        </td>
      </tr>
      </table>
    
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>