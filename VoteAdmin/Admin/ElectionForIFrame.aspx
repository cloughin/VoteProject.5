<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
  CodeBehind="ElectionForIFrame.aspx.cs" Inherits="Vote.Admin.ElectionForIFramePage" %>
<%@ Import Namespace="Vote" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <%-- following is because this is a port from a public page --%>
  <link type="text/css" rel="stylesheet" href="http://<%=UrlManager.SiteHostNameAndPort %>/css/vote/public.css"/>
  <style type="text/css">
    .content .intro
    {
      margin: 0;
    }
    #MainAdminMenu *
    {
      box-sizing:content-box;
    }
    .ui-corner-all
    {
      border-bottom-left-radius: 0;
      border-bottom-right-radius: 0;
    }
    .content .election-title {
      font-size: 28px;
      margin-top: 24px;
      font-weight: bold;
                             }
    .content .election-subtitle {
      margin-top: -12px;
      font-weight: normal;
      font-size: 1rem;
                                }
    .content .higher-level-links {
      padding-bottom: 3px;
                                 }
    .content .higher-level-links a {
      display: block;
      margin-top: 10px;
      font-weight: bold;
      font-size: .9rem;
                                   }
    .content .additional-info {
      margin-top: 5px;
      font-size: .9rem;
      line-height: 120%;
                              }
    .content .election-report {
      margin-top: 10px;
                              }
    .content .election-report.accordion-content {
      margin-top: 0;
                                                }
    .content .intro {
      padding: 0 5px;
                    }
    .master-links a {
      padding: 0 20px;
      font-size: 14px;
      font-weight: normal;
                    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div class="content">
    <div class="intro">
      <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
      <h2 id="ElectionSubTitle" class="election-subtitle" runat="server"></h2>
    
      <%--<div id="HigherLevelLinks" class="higher-level-links" runat="server"></div>--%>

      <div id="AdditionalInfo" class="additional-info" runat="server"></div>
    </div>
        
    <asp:PlaceHolder ID="ElectionPlaceHolder" runat="server"  EnableViewState="False"></asp:PlaceHolder>
  </div>
</asp:Content>

