<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="EmailBulkDelete.aspx.cs" 
Inherits="Vote.Master.EmailBulkDeletePage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <div class="rounded-border boxed-group file-upload-container">
      <div class="boxed-group-label">Select one or more undeliverable email msg files</div>
      <input name="FileUpload" id="FileUpload" type="file" multiple="multiple" />
    </div>

    <div class="rounded-border boxed-group extra-addresses-container">
      <div class="boxed-group-label">Enter additonal email addresses to delete, one per line</div>
      <user:TextBoxWithNormalizedLineBreaks ID="ExtraAddresses" TextMode="MultiLine" Rows="4" CssClass="extra-addresses" runat="server"></user:TextBoxWithNormalizedLineBreaks>
    </div>

    <div class="rounded-border boxed-group summary-container hidden" runat="Server" id="SummaryContainer">
      <div class="boxed-group-label">Processing summary</div>
      <div class="summary">
        <asp:PlaceHolder ID="SummaryPlaceHolder" runat="server"></asp:PlaceHolder>       
      </div>
    </div>
    
    <div class="check-container">
      <asp:CheckBox ID="DeleteCheckBox" CssClass="delete-checkbox"
      runat="server" Text="Check to actually delete, leave unchecked to just analyze." />
    </div>

    <div class="submit-container">
      <input type="button" class="button-1 submit-form button-smaller" value="Submit" /> 
    </div>
    
    <div class="rounded-border boxed-group lookup-source-id-container">
      <div class="boxed-group-label">Lookup up source id</div>
      <input type="text" class="lookup-source-id-input" />
      <input type="button" class="button-2 lookup-source button-smaller" value="Lookup" /> 
      <div class="source-id-email"></div>
    </div>

  </div>
</asp:Content>
