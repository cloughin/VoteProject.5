<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoCache.aspx.cs" Inherits="Vote.NoCachePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Set NoCache</title>
    <style>
        body
        {
          margin: 0;
        }
       div
       {
         color: #666;
         font-size: 10pt;
         font-family: Arial, Helvetica, sans-serif;
       }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>   
    </div>
    </form>
</body>
</html>
