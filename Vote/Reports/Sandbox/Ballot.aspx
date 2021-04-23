<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ballot.aspx.cs" Inherits="Vote.Sandbox.Ballot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
    <link href="/css/Reset.css" type="text/css" rel="stylesheet" />
    <link href="/css/MainCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/SecondaryCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/All.css" type="text/css" rel="stylesheet" />
    <link href="/css/Ballot.css" type="text/css" rel="stylesheet" />
    <link href="/css/BallotNew.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <div id="outer"><div id="container"><div id="mainContent"><div id="InnerContent">
        <table class="tablePage" id="BallotOfficesTable" cellspacing="0" cellpadding="0"
          runat="server" enableviewstate="False">
        </table>
        <asp:PlaceHolder ID="ReportPlaceHolder" enableviewstate="False" runat="server"></asp:PlaceHolder>
        <table class="tablePage" id="BallotReferendumsTable" cellspacing="0" cellpadding="0" runat="server" EnableViewState="False">
        </table>
        <asp:PlaceHolder ID="ReferendumReportPlaceHolder" enableviewstate="False" runat="server"></asp:PlaceHolder>
      </div></div></div></div>
    </div>
    </form>
</body>
</html>
