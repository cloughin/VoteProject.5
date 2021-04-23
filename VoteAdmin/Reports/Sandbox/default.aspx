<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Vote.Sandbox._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Test</title>
  <asp:PlaceHolder ID="NonPublicCss" Visible = "false" runat="server">
    <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
    <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
  </asp:PlaceHolder>
  <asp:PlaceHolder ID="ElectionNonPublicCss" Visible = "false" runat="server">
    <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
    <link href="/css/Election.css" type="text/css" rel="stylesheet" />
  </asp:PlaceHolder>
  <asp:PlaceHolder ID="PublicCss" Visible = "false" runat="server">
    <link href="/css/Reset.css" type="text/css" rel="stylesheet" />
    <link href="/css/MainCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/SecondaryCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/All.css" type="text/css" rel="stylesheet" />
    <link href="/css/Officials.css" type="text/css" rel="stylesheet" />
    <link href="/css/SocialMediaButtons.css" type="text/css" rel="stylesheet" />
  </asp:PlaceHolder>
  <asp:PlaceHolder ID="ElectionPublicCss" Visible = "false" runat="server">
    <link href="/css/Reset.css" type="text/css" rel="stylesheet" />
    <link href="/css/MainCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/SecondaryCommon.css" type="text/css" rel="stylesheet" />
    <link href="/css/All.css" type="text/css" rel="stylesheet" />
    <link href="/css/Election.css" type="text/css" rel="stylesheet" />
    <link href="/css/SocialMediaButtons.css" type="text/css" rel="stylesheet" />
  </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:PlaceHolder ID="NonPublicPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <div id="mainContent">
      <asp:PlaceHolder ID="PublicPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
