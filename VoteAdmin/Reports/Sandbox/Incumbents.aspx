<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Incumbents.aspx.cs" Inherits="Vote.Sandbox.Incumbents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link href="/css/admin-new.css" type="text/css" rel="stylesheet" /><link href="/css/tipTip.css" type="text/css" rel="stylesheet" />
  <!--[if IE 7]>
  <link href="/css/admin-new.ie7.css" type="text/css" rel="stylesheet" />
  <![endif]-->
  <!--[if IE 8]>
  <link href="/css/admin-new.ie8.css" type="text/css" rel="stylesheet" />
  <![endif]-->
  <!--[if gte IE 9]>
  <link href="/css/admin-new.ie9.css" type="text/css" rel="stylesheet" />
  <![endif]-->
  
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Ballot.css" type="text/css" rel="stylesheet" />
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: normal;
      color: #373737;
      font-size: 11px;
      width: 543px;
      height: 26px;
      padding-left: 5px;
      padding-right: 0;
      padding-top: 0;
      padding-bottom: 0;
    }
    .style2
    {
      height: 26px;
    }
    body.admin-page h1
    {
      line-height: 110%;
      margin: 8px 0 8px 0;
    }
  </style>
</head>
<body class="admin-page">
    <form id="form1" runat="server">
    <div class="xmaster-main-content">
      <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" colspan="2" valign="top" class="T">
            <asp:PlaceHolder ID="IncumbentReportPlaceHolder" runat="server"></asp:PlaceHolder>
          </td>
        </tr>
      </table>
    </div>
    </form>
</body>
</html>
