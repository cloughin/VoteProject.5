<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Privacy.aspx.cs" Inherits="Vote.PrivacyPage" %>

<%--<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>--%>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Privacy Policy</h1>
  <div class="intro-copy">
    <p>Vote-USA uses your address only to provide you 
    with your customized electoral information, including your ballot choices, 
    elected representatives, and various election reports. We do not sell, 
    trade, or otherwise transfer any of this information to any other person, 
    organization or third party.</p>

    <p>We do not sell or trade any of the information we 
    have about candidates and politicians to any other person, organization 
    or third party.</p>

    <p>We will honor a request by any candidate to delete any personal 
    information, including picture, biographical information, and views 
    on the issues. To maintain the accuracy of Vote-USA election reports, 
    we will not delete any candidate’s name or offices the candidate sought. 
    We consider this to be public information.</p>
    
    <p><em>YouTube and Google Privacy Policies</em><br/>
      <a href="https://www.youtube.com/t/terms" target="youtube">YouTube Terms and Services</a><br/>
      <a href="https://policies.google.com/privacy" target="youtube">Google Privacy Policy</a>
    </p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
  <%--<user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
