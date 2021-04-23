<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="Vote.Organization.Default" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Domain Organizations - Name, URL and Page Parts</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body>
  <form id="form1" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="LabelDesignCode" runat="server" CssClass="HLargeColor"></asp:Label>Organization
          Administration</td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="StateElectionTable1" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2">
          Organization Information</td>
      </tr>
      <tr>
        <td align="left" class="T" valign="middle" colspan="2">
          Use these text boxes to provide information about this organization. Then click
          the button at the bottom to submit the information.</td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          Organization:</td>
        <td class="T" valign="top">
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table2" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="T" align="right" valign="top">
          Organization Name:*<br />
          required</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOrganization" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          <asp:HyperLink ID="HyperLinkOrganizationURL" runat="server" CssClass="T">Organization Web Address:*</asp:HyperLink><br />
          required (like: Abc.com)&nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOrganizationURL" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          <asp:HyperLink ID="HyperLinkOrganizationEmail" runat="server" CssClass="T">Organization EMail:</asp:HyperLink><br />
          (like: info@Abc.com)</td>
        <td class="T" align="left" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOrganizationEmail" runat="server" CssClass="TextBoxInput"
            Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" valign="top" align="right">
          Address Line 1:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAdressLine1" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" valign="top" align="right">
          Address Line 2:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddressLine2" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" valign="top" align="right">
          City, State Zip:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCityStateZip" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="HSmall" valign="top">
          Main Contact:</td>
        <td class="HSmall" valign="top">
        </td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Name:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContact" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Title:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContactTitle" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          <asp:HyperLink ID="HyperLinkEmailContact" runat="server" CssClass="T">Email:</asp:HyperLink><br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContactEmail" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Phone:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxContactPhone" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="HSmall" valign="top">
          Alternate Contact:
        </td>
        <td class="HSmall" valign="top">
        </td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Name:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContact" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Title:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContactTitle" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          <asp:HyperLink ID="HyperLinkEmailAltContact" runat="server" CssClass="T">Email:</asp:HyperLink><br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContactEMail" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" align="right" valign="top">
          Phone:<br />
          &nbsp; &nbsp;</td>
        <td class="T" valign="top">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAltContactPhone" runat="server" CssClass="TextBoxInput" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table3" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="T" valign="middle">
        </td>
        <td class="T" valign="middle">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxNote" runat="server" CssClass="TextBoxInput" Height="82px"
            TextMode="MultiLine" Width="500px"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td class="T" colspan="3">
          <asp:Button ID="ButtonSubmit" runat="server" CssClass="Buttons" OnClick="ButtonSubmit_Click"
            Text="Submit" Width="200px" />
          Click to record or update the above information.</td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>