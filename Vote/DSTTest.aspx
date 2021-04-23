<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DSTTest.aspx.cs" Inherits="Vote.DSTTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Science Toolkit Test</title>
  <style>
    .result
    {
      width: 500px;
      height: 200px
    }
    .address
    {
      width: 500px;
    }
  </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
          <h1>Data Science Toolkit Test</h1>
          <p>Last Result</p>
          <div><asp:TextBox ID="TextBoxResult" runat="server" CssClass="result" TextMode="MultiLine"></asp:TextBox></div>
          <p>
          <asp:TextBox ID="TextBoxAddress" CssClass="address" runat="server" placeholder="Enter an Address"></asp:TextBox>
          <input id="Submit1" type="submit" value="Submit" />
          </p>
        </div>
    </form>
</body>
</html>
