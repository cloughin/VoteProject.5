<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="AnswersEdit.aspx.cs" Inherits="Vote.AnswersEdit.AnswersEdit" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
<head runat="server">
  <title>Answers Edit</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table -->
    <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0" width=700>
      <tr>
        <td align="left" class="HLarge">
          &nbsp;Edit Answer</td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          <asp:Label ID="LabelPolitician" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="HSmall">
          Question:
          <asp:Label ID="LabelQuestion" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td>
          <asp:Button ID="ButtonRecordChanges" runat="server" Text="Record Changes" OnClick="ButtonRecordChanges_Click" CssClass="Buttons" Width="154px" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="TableAnswers" cellspacing="0" cellpadding="0" width=700>
      <tr>
        <td class="T">
          Date: Enter as MM/DD/YYYY &nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextBoxDateStamp" runat="server" CssClass="TextBoxInput" Width="187px"></user:TextBoxWithNormalizedLineBreaks>
          <br />
          <strong>
          Source: </strong>Enter without http:// if web page. Max Char Length is 85. Current:
          <asp:Label ID="LabelSourceLength" runat="server"></asp:Label>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxSource" runat="server" Width="598px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks><br />
          <strong>
          Answer:</strong> Max Character Length is 1,000. Current Length is: &nbsp;<asp:Label ID="LabelAnswerLength"
            runat="server"></asp:Label><br />
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAnswer" runat="server" Height="200px" Width="700px" CssClass="TextBoxInputMultiLine" TextMode="MultiLine"></user:TextBoxWithNormalizedLineBreaks>
        </td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>