<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tpi.aspx.cs" Inherits="Vote.Sandbox.tpi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
    <link href="/css/Reset.css" type="text/css" rel="stylesheet" />
    <link href="/css/MainCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/SecondaryCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/All.css" type="text/css" rel="stylesheet" />
    <link href="/css/PoliticianIssue.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="outer">
      <div id="container">
        <div id="mainContent">
          <div id="InnerContent">
            <table class="tablePage" cellspacing="0" cellpadding="0">
              <tr class="trPoliticianIssueLinks">
                <td class="tdPoliticianIssueLinks">
                  <asp:PlaceHolder ID="LinksPlaceHolder" enableviewstate="False" runat="server"></asp:PlaceHolder>
                </td>
              </tr>
              <tr class="trPoliticianIssueLinks">
                <td class="tdPoliticianIssueLinks">
                  <asp:PlaceHolder ID="LinksPlaceHolder2" enableviewstate="False" runat="server"></asp:PlaceHolder>
                </td>
              </tr>
            </table>
            <table class="tablePage" id="IssueResponsesTable" cellspacing="0" cellpadding="0"
              runat="server" enableviewstate="False">
            </table>
            <asp:PlaceHolder ID="ReportPlaceHolder" enableviewstate="False" runat="server"></asp:PlaceHolder>
         </div>
        </div>
      </div>
    </div>
    </form>
</body>
</html>
