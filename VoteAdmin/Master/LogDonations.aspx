<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="LogDonations.aspx.cs" 
Inherits="Vote.Master.LogDonationsPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <div class="rounded-border boxed-group csv-upload-container">
      <div class="boxed-group-label">Select a Click &amp; Pledge .csv file to upload</div>
      <input name="CevUpload" id="CsvUpload" type="file" />
    </div>

    <div class="rounded-border boxed-group summary-container hidden" runat="Server" id="SummaryContainer">
      <div class="boxed-group-label">Processing summary</div>
      <div class="summary">
        <asp:PlaceHolder ID="SummaryPlaceHolder" runat="server"></asp:PlaceHolder>       
      </div>
    </div>

    <div class="submit-container">
      <input type="submit" class="button-1 submit-form button-smaller" value="Submit" /> 
    </div>
    
    <div class="rounded-border boxed-group reversal-container">
      <div class="boxed-group-label">Record reversal</div>
      <p>Enter donor's email address below then click <em>Find Donation</em></p>
      <input name="Reversal" id="Reversal" class="reversal" type="text" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false"/>
      <input type="button" class="button-2 find-reversal-button button-smallest" value="Find Donation" /> 
      <div id="reversals"></div>
    </div>

  </div>
</asp:Content>
