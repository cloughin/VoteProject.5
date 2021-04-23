<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Videos.aspx.cs" Inherits="Vote.VideosPage" %>
<%@ Register Src="/Controls/PageHeading.ascx" TagName="PageHeading" TagPrefix="user" %>

<%@ Register Src="/Controls/MainBanner.ascx" TagName="MainBanner" TagPrefix="user" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Videos</title>
  <style>
    html, body #outer
    {
      background: #fff;
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
  <div id="outer">
    <div id="container">
      <user:MainBanner runat="server" />
      <div id="mainContent">
        <user:PageHeading ID="PageHeading" runat="server" 
          MainHeadingText="Videos" />
        <div class="intro">
          <p>
            The links on this page provide instructional and promotional videos</p>
          <p>
            <b>Instruction Videos</b><br />
            <br />
            The link below provides a video of how to capture biographical information, the 
            campaign website address, social media links for Candidates&#39; Introduction Page
          </p>
          <p>
            <asp:HyperLink runat="server" NavigateUrl="~/Video.aspx?video=introBio" 
              Target="video">Capturing Biographical Information for Candidates&#39; Introduction Page </asp:HyperLink>
          </p>
          <p>
            <strong>Promotional Videos</strong></p>
          <p>
            The link below provides a video that compares the information available for the 
            April 3, 2012 Maryland Primary of the primary websites that provide candidate 
            information.</p>
          <p>
            <asp:HyperLink ID="HyperLinkCompareMdPrimary" runat="server" 
              NavigateUrl="~/Video.aspx?video=compareMdPrimary" Target="video">Comparison of Websites for the April 3, 2012 Maryland Primary</asp:HyperLink>
          </p>
          <p>
            &nbsp;</p>
          <p>
            &nbsp;</p>
        </div>
      </div>
      <!-- id="mainContent" -->
    </div>
    <!-- id="container" -->
  </div>
  <!-- id="outer" -->
  </form>
</body>
</html>
