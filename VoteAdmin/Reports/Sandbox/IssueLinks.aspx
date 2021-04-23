<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueLinks.aspx.cs" Inherits="Vote.Sandbox.IssueLinks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
    <link href="/css/Reset.css" type="text/css" rel="stylesheet" />
    <link href="/css/MainCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/SecondaryCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/All.css" type="text/css" rel="stylesheet" />
    <link href="/css/Issue.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
      <div id="outer">
      <div id="container">
        <div id="mainContent">
        <div id="InnerContent">
          <table id="IssueLinksTable" class="tablePage" cellspacing="0" cellpadding="0" cols="2">
            <tr class="trIssueLinks">
              <td class="tdIssueLinks" align="left" colspan="1">
                <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
              </td>
            </tr>
          </table>
        </div>
      </div></div></div>
    </form>
</body>
</html>
