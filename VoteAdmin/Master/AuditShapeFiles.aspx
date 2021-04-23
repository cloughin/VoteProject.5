<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" CodeBehind="AuditShapeFiles.aspx.cs" Inherits="VoteAdmin.AuditShapeFilesPage" %>

<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/SubHeadingWithHelp.ascx" TagName="SubHeadingWithHelp" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
    .help ul
    {
      list-style-type: disc;
    }
    .help li
    {
      margin-left: 30px;
    } 
    .audit-button
    {
      margin: 10px 20px;
    }
    .results
    {
      margin: 20px;
      padding: 10px;
      border: 1px solid #cccccc;
    }
    .results p
    {
      font-size: 10pt;
      margin: 4px 0;
    }
    .results p.head
    {
      font-size: 12pt;
      font-weight: bold;
    }
    .results p.error
    {
      margin-left: 20px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
      <ContentTemplate>
        <div id="Container" class="maint-container page-container shadow rounded-border">
          <user:SubHeadingWithHelp ID="SubHeadingWithHelp" runat="server"
                                   Title="What is audited?" CssClass="tiptip"
                                   Tooltip="Show/hide information about what is audited">
            <ContentTemplate>
              <p>There are three Vote-USA proprietary shapefiles: City Council, County Supervisors 
                and School Sub-Districts. Each of these has a corresponding table in the database that 
                indicates which districts are actually in the shapefile.</p>
              <p>This audit compares the shapefile to the database table and reports the following issues:
                <ul>
                  <li>District is in the shapefile, but is not in the database.</li>
                  <li>District is in the shapefile, but is marked "not in shapefile" in the database.</li>
                  <li>District is marked "in shapefile" in the database, but is not in the shapefile.</li>
                  <li>For County Supervisors, verify that the county codes agree.</li>
                </ul>
                <p>In addition there is a separate audit for each of the Tiger SChool District shapefiles, to report:</p>
                <ul>
                  <li>District is in the shapefile, but is not in the database.</li>
                  <li>District is in the database, but is not in the shapefile.</li>
                  <li>There is no matching county entry for the district.</li>
                  <li>There is a county entry with no matching district</li>
                </ul>
              </p>
            </ContentTemplate>
          </user:SubHeadingWithHelp>
          <asp:Button ID="ButtonAuditCityCouncil" runat="server" CssClass="audit-button button-1"
                      Text="Audit City Council ShapeFile"
                      OnClick="ButtonAuditCityCouncil_Click" /><br />

          <asp:Button ID="ButtonAuditCountySupervisors" runat="server" CssClass="audit-button button-1"
                      Text="Audit County Supervisors ShapeFile"
                      OnClick="ButtonAuditCountySupervisors_Click" /><br />

          <asp:Button ID="ButtonAuditSchoolSubDistricts" runat="server" CssClass="audit-button button-1"
                      Text="Audit School Sub-Districts ShapeFile"
                      OnClick="ButtonAuditSchoolSubDistricts_Click" /><br />

          <asp:Button ID="ButtonAuditElementarySchoolDistricts" runat="server" CssClass="audit-button button-1"
                      Text="Audit Elementary School Districts ShapeFile"
                      OnClick="ButtonAuditElementarySchoolDistricts_Click" /><br />

          <asp:Button ID="ButtonAuditSecondarySchoolDistricts" runat="server" CssClass="audit-button button-1"
                      Text="Audit Secondary School Districts ShapeFile"
                      OnClick="ButtonAuditSecondarySchoolDistricts_Click" /><br />

          <asp:Button ID="ButtonAuditUnifiedSchoolDistricts" runat="server" CssClass="audit-button button-1"
                      Text="Audit Unified School Districts ShapeFile"
                      OnClick="ButtonAuditUnifiedSchoolDistricts_Click" /><br />

          <asp:PlaceHolder ID="ResultsPlaceHolder" runat="server"></asp:PlaceHolder>

          <user:FeedbackContainer ID="AuditShapeFileFeedback" runat="server" />
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>

  </div>
</asp:Content>
