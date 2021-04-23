<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" CodeBehind="DatabaseStats.aspx.cs" Inherits="VoteAdmin.DatabaseStats" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>
    <table class="stats-table">
      <tbody>
        <tr class="odd">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Candidates</td>
          <td class="value" id="CandidatesCount" runat="server"></td>
        </tr>
        <tr class="even">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Candidate Pictures</td>
          <td class="value" id="CandidatePicturesCount" runat="server"></td>
        </tr>
        <tr class="odd">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Offices</td>
          <td class="value" id="OfficesCount" runat="server"></td>
        </tr>
        <tr class="even" id="OfficeCandidatesRow" runat="server">
          <td class="label expandable-label office-candidates-expandable"><span class="expandable">+</span>&nbsp;Office Candidates</td>
          <td class="value" id="OfficeCandidatesCount" runat="server"></td>
        </tr>
        <tr class="odd" id="OfficeContestsRow" runat="server">
          <td class="label expandable-label office-contests-expandable"><span class="expandable">+</span>&nbsp;Office Contests</td>
          <td class="value" id="OfficeContestCount" runat="server"></td>
        </tr>
        <tr class="even" id="ElectionsRow" runat="server">
          <td class="label expandable-label elections-expandable"><span class="expandable">+</span>&nbsp;Elections</td>
          <td class="value" id="ElectionsCount" runat="server"></td>
        </tr>
        <tr class="odd">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Social Media Links</td>
          <td class="value" id="SocialMediaLinksCount" runat="server"></td>
        </tr>
        <tr class="even">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Biographical Info Entries</td>
          <td class="value" id="BioInfoCount" runat="server"></td>
        </tr>
        <tr class="odd">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Reasons &amp; Objectives Entries</td>
          <td class="value" id="PersonalCount" runat="server"></td>
        </tr>
        <tr class="even">
          <td class="label"><span class="not-expandable">+</span>&nbsp;Issue Responses</td>
          <td class="value" id="IssueResponsesCount" runat="server"></td>
        </tr>
      </tbody>
    </table>
  </div>
</asp:Content>
