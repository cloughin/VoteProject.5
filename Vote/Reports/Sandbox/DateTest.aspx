<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DateTest.aspx.cs" Inherits="Vote.Sandbox.DateTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script data-main="/js/require-main.js" src="/js/require.js" type="text/javascript"></script> 
    <script type="text/javascript">
      require(["vote/sandbox/dateTest"], function () {});
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      The date: 
      <asp:PlaceHolder ID="LocalDatePlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
