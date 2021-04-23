<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
CodeBehind="OfficesAndCandidatesSelection.aspx.cs"
Inherits="Vote.Admin.OfficesAndCandidatesSelection" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    #body h1
    {
      margin-bottom: 0;
    }

    .subhead
    {
      margin-bottom: 20px;
    }

    .selection label
    {
      font-size: 12pt;
      font-weight: bold;
    }

    .selection .dates input
    {
      margin-right: 15px;
      width: 100px;
    }

    .selection .radio-group,
    .generate-button
    {
      margin-top: 15px;
    }

    .error
    {
      color: #ff0;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div vlass="outer">
    <h1 id="H1" runat="server"></h1>
    <p class="subhead">The CSV contains counts of the selected items, summarized by month and year.</p>
    <div class="selection">
      <div class="dates">
        <label for="FromDate">Date From: </label>
        <input type="text" id="FromDate" class="from-date date-picker"/>
        <label for="ToDate">Date To: </label>
        <input type="text" id="ToDate" class="to-date date-picker"/>
      </div>
      <div class="radio-group">
        <label>Report Type</label>
        <div class="radio"><input type="radio" name="ReportType" class="report-type" value="O" checked="checked"/> Offices</div>
        <div class="radio"><input type="radio" name="ReportType" class="report-type" value="C"/> Candidates</div>
        <div class="radio"><input type="radio" name="ReportType" class="report-type" value="OC"/> Offices &amp; Candidates</div>
        <div class="radio"><input type="radio" name="ReportType" class="report-type" value="B"/> Ballot Measures</div>
      </div>
      <div class="radio-group">
        <label>Degree of Detail</label>
        <div class="radio"><input type="radio" name="Detail" class="detail" value="X" checked="checked"/> Exclude Elections</div>
        <div class="radio"><input type="radio" name="Detail" class="detail" value="E"/> Include Elections</div>
      </div>
    </div>
    <div class="generate-button">
      <input type="button" class="button-1 generate-report" value="Generate Report"/>
    </div>
  </div>
</asp:Content>