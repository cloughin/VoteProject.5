<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" Codebehind="Answers.aspx.cs" Inherits="Vote.AnswersPage.AnswersPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
  <title>Answers</title>
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
    <table class="tableAdmin" id="TableHeading" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          <asp:Label ID="PageTitle" runat="server"></asp:Label>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td class="T">
          Login User Name:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLoginUserName" runat="server" Width="174px" CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
          &nbsp;
          From:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFrom" runat="server" CssClass="TextBoxInput" Width="152px"></user:TextBoxWithNormalizedLineBreaks>&nbsp; To:
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxTo" runat="server" CssClass="TextBoxInput" Width="159px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
          <asp:Button ID="ButtonGetAnswers" runat="server" Text="Get Answers" OnClick="ButtonGetAnswers_Click" CssClass="Buttons" Width="200px" /><br />
          Today Is:
          <asp:Label ID="LabelCurrentDate" runat="server"></asp:Label><br />
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Report" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          Answers</td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="LabelReport" runat="server" EnableViewState="False"></asp:Label></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>