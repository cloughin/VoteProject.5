<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubHeadingWithHelp.ascx.cs" 
Inherits="Vote.Controls.SubHeadingWithHelpControl" %>

<div id="SubHeadingOuter" runat="server">
  <h2 id="SubHeading" runat="server">SubHeading</h2>
  <a onclick="$('#<%=SubHeadingOuter.ClientID%> div.help').toggle(400);" 
    title="Show/hide help" class="help" runat="server" id="HelpButton">help</a>
  <div style="clear:both"></div>
  <div class="help rounded-border">
    <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
  </div>
</div>
