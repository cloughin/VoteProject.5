<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectJurisdictionButton.ascx.cs" 
  Inherits="VoteAdmin.Controls.SelectJurisdictionButton" %>

  <div id="Container" runat="server" class="sub-heading change-button select-jurisdiction-button">
    <input id="ChangeJurisdictionButton" runat="server" type="button" 
        value="Change Jurisdiction" class="jurisdiction-change-button button-1 button-smaller tiptip"
        title="Switch to a different state, county or local jurisdiction -- subject to your sign-in credentials."/>
    <asp:HyperLink ID="StateLink" class="jurisdiction-state-link" runat="server" Visible="False">&#x25ba; State</asp:HyperLink>
    <asp:HyperLink ID="CountyLink" class="jurisdiction-county-link" runat="server" Visible="False">&#x25ba; County</asp:HyperLink>
  </div>
