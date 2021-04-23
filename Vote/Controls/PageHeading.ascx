<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageHeading.ascx.cs" Inherits="Vote.Controls.PageHeading" %>
<div class="header" id="MainHeading" runat="server">
  <div class="title" style="float:left"><asp:Literal ID="MainHeadingLiteral" runat="server">Main Heading</asp:Literal></div>
  <div ID="DonateButton" class="heading-donate" runat="server"><a id="DonateLink" title="Help support Vote-USA.org with a donation" target="donate" runat="server">Donate</a></div>
  <div style="clear:both"></div>
  <div class="subtitle" id="SubHeading" runat="server"><asp:Literal ID="SubHeadingLiteral" runat="server">Sub Heading</asp:Literal></div>
</div>
