<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="IssueList.aspx.cs" Inherits="Vote.IssueListPage" %>

<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .issue-list-report {
      margin-top: 20px;
    }
    .issue-list-report span.question
    {
      display: block;
      padding: 3px 20px;
      font-size: .95rem;
    }

    /*#issue-list-accordions .section-content,
    #issue-list-accordions .group-content
    {
      padding: 0;
    }

    #issue-list-accordions .group-header,
    #issue-list-accordions .issue-header
    {
      margin-top: 0;
      border-radius: 0;
    }

    #issue-list-accordions .group-header,
    #issue-list-accordions .group-content,
    #issue-list-accordions .issue-header,
    #issue-list-accordions .issue-content
    {
      border-bottom: none;
      border-left: none;
      border-right: none;
      border-color: #ccc;
    }

    #issue-list-accordions .group-header .sub-heading
    {
      font-size: 80%;
    }

    #issue-list-accordions .issue-header
    {
      background: #f8f8f8;
    }

    #issue-list-accordions .issue-content
    {
      border-top: 1px solid #ccc;
    }

    #issue-list-accordions .ui-icon
    {
      background: none;
      display: block;
      height: 0;
      width: 0;
      border: 5px solid transparent;
      border-left-color: #888;
      margin-top: -5px;
      left: 12px;
      transition: all .5s;
    }

    #issue-list-accordions .ui-state-hover .ui-icon
    {
      border-left-color: #222;
      transition: all .5s;
    }

    #issue-list-accordions .ui-state-active .ui-icon
    {
      transform: rotate(90deg);
      left: 10px;
      margin-top: -3px;
      transition: all .5s;
    }*/
    
  </style>
  <script type="text/javascript">
    $(function () {
      $(".accordion-container").each(function() {
        var $this = $(this);
        $this.accordion({
        collapsible: true,
        heightStyle: "content",
        active: $this.children().length <= 2 ? 0 : true,
        activate: PUBLIC.accordionActivate
        });
      });

    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Issues and Issue Questions Available for Candidate Responses</h1>
  
  <div class="intro">
    <p id="Instructions" class="instructions" runat="server"></p>
  </div>

  <asp:PlaceHolder ID="ReportPlaceHolder" runat="server" EnableViewState="False"></asp:PlaceHolder>

</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
