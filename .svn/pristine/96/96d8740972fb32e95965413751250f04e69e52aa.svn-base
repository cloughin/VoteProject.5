<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeControl.ascx.cs" 
Inherits="Vote.Controls.OfficeControl" %>

<asp:UpdatePanel ID="UpdatePanelSelectOffice" class="office-control" UpdateMode="Conditional" runat="server">
  <ContentTemplate>
    <div id="ContainerSelectOffice" class="select-office-container shadow-2" runat="server">
    <input id="SelectedOfficeKey" runat="server" class="selected-office-key" type="hidden" />
    <input id="ShowSelectOfficePanelPrivate" runat="server" class="show-select-office-panel" type="hidden" />
    <input id="SelectOfficeExpandedNode" runat="server" class="select-office-expanded-node" type="hidden" />
    <div class="heading">
      <p class="fieldlabel"><em>Select an Office</em></p>
      <p id="FieldLabelMessage" class="fieldlabel sub" runat="server">Click the <em>Office Title</em> above to hide or show this list.</p>
    </div>
    <div ID="ControlSelectOfficeList" runat="server" class="select-office-control">
      <asp:PlaceHolder ID="PlaceHolderSelectOfficeTree" runat="server"></asp:PlaceHolder>
    </div>
    </div>
  </ContentTemplate>
</asp:UpdatePanel>
