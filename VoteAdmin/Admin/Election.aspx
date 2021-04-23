<%@ Page Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="Election.aspx.cs" Inherits="Vote.Admin.ElectionPage" %>
<%@ Import Namespace="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <%-- following is because this is a port from a public page --%>
  <link type="text/css" rel="stylesheet" href="//<%=UrlManager.SiteHostNameAndPort %>/css/vote/public.css"/>
  <style type="text/css">

    body.admin-page .content h1
    {
      margin-bottom: 0;
    }

    .content .election-title
    {
      font-size: 28px;
      margin-top: 24px;
      font-weight: bold;
    }

    .content .election-subtitle
    {
      font-weight: normal;
      font-size: 1rem;
    }

    body.admin-page h3.multi-county-message
    {
      max-width: 800px;
      line-height: 120%;
      margin-bottom: 0;
    }

    #MainAdminMenu *
    {
      box-sizing: content-box;
    }

    .content .intro
    {
      margin: 0;
      padding: 0 5px;
    }

    .content .election-report
    {
      margin-top: 10px;
      padding: 0 3px;
      font-family: Verdana,Arial,sans-serif;
      font-size: 1.1em;    
    }

    .election-report .accordion-header
    {
      border: 1px solid #aaaaaa;
      border-top-left-radius: 4px;
      border-top-right-radius: 4px;
      border-bottom-left-radius: 0;
      border-bottom-right-radius: 0;
      padding-left: 8px;
    }

    .election-report .accordion-content
    {
      padding: .5em;
      color: #000000;    
      border: 1px solid #aaaaaa;
      border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
      border-top: 0;
      overflow: auto;
      margin-top: 0;
    }

    .election-report .office-title .vote-for
    {
      font-size: 90%;
    }

    .election-report .office-title .county-message
    {
      font-size: 80%;
    }

    /*.ui-corner-all
    {
      border-bottom-left-radius: 0;
      border-bottom-right-radius: 0;
    }*/

    /*.content .higher-level-links
    {
      padding-bottom: 3px;
    }*/

    /*.content .higher-level-links a
    {
      display: block;
      margin-top: 10px;
      font-weight: bold;
      font-size: .9rem;
    }*/

    .content .additional-info
    {
      margin-top: 5px;
      font-size: .9rem;
      line-height: 120%;
    }

    /*.content .election-report.accordion-content
    {
      margin-top: 0;
    }*/

    /*.content .intro
    {
      padding: 0 5px;
    }*/

    .master-links
    {
     margin-bottom: 2px;
    }

    .master-links > *
    {
      padding: 0 20px;
      font-size: 14px;
      font-weight: normal;
    }

    body.admin-page.for-iframe form
    {
      width: 880px;
      margin-top: -20px;
    }

    body.admin-page.for-iframe #AdminPage
    {
      display: none;
    }  

    .boldred
    {
      color: red !important;
      font-weight: bold !important;
    }

  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div class="content">
  <div class="intro">
    <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
    <h3 id="MuliCountyMessage" class="multi-county-message hidden" runat="server"></h3>
    <h2 id="ElectionSubTitle" class="election-subtitle" runat="server"></h2>
    
    <%--<div id="HigherLevelLinks" class="higher-level-links" runat="server"></div>--%>
    <%--<div id="AdditionalInfo" class="additional-info" runat="server"></div>--%>
  </div>
        
  <asp:PlaceHolder ID="ElectionPlaceHolder" runat="server"  EnableViewState="False"></asp:PlaceHolder>
  </div>
</asp:Content>
