<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="SampleBallotEnrolled.aspx.cs" Inherits="Vote.SampleBallotEnrolledPage" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    h1
    {
      text-align: center;
      font-size: 20pt;
      font-weight: bold;
      margin-top: 20px;
    }

    h2
    {
      margin: 10px 10px 0 10px;
      text-align: center;
    }

    .email-fixed
    {
      margin: 20px 10px 0 10px;
      text-align: center;
    }

    .email-fixed span
    {
      font-weight: bold;
    }

    .table-wrapper
    {
      text-align: center;
      padding: 0 10px;
    }

    .districts-table
    {
      margin: 0 auto;
      text-align: left;
      margin-top: 15px;
      border-collapse: collapse;
    }

    .districts-table td
    {
      border: 1px solid #ddd;
      padding: 4px;
    }

    .districts-table tr td:first-child
    {
      font-weight: bold;
      color: #444;
    }

    .for-voters-link
    {
      text-align: center;
      margin-top: 10px;
    }

    .for-voters-link span
    {
      font-weight: bold;
    }
  </style>
  <script type="text/javascript">
    $(function() {
      $.cookie('sampleBallotEmailEntered', 'true', { expires: 365 });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Congratulations!</h1>
  <h2>You are now enrolled for automatic ballot choices</h2>

  <div id="EmailFixed" class="email-fixed">
    Ballot choices will be emailed to <span id="EmailFixedAddress" class="email-fixed-address" runat="server"></span> as soon as they are available.
  </div>
  
  <div class="for-voters-link"><a href="/">Visit our home page to view ballot choices available now.</a></div>

  <div class="table-wrapper">
    <asp:Table ID="DistrictsTable" CssClass="districts-table" runat="server"></asp:Table>
  </div>

</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
