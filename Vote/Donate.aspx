<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="Donate.aspx.cs" Inherits="Vote.DonatePage" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    header .header-inner .donate
    {
      display: none;
    }
    .donate-wrapper
    {
      text-align: center;
      margin-top: 30px;
    }
    p.donate-disclaimer
    {
      font-size: 70%;
      margin: 0 10%;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Donate</h1>

  <div class="intro-copy">

    <p>We are a small non-profit, non-partisan organization with the same costs as top websites: servers, staff and overhead. We are primarily sustained by your contributions. Help us to continue our work. Please take a minute and make a generous contribution.</p>

    <p><strong>Contributions do not support any particular candidate and are 100% tax-deductible for most individuals.</strong></p>

    <div class="donate-wrapper">
       <a id="DonateLink" runat="server" class="donatetoday no-print" href="?" target="donate"><img alt="" src="/images/PayPal-Donate-Button-2.png" title="Donate to Vote-USA.org today"/></a>
    </div>
    <p class="donate-disclaimer">No information about your contribution is shared with any third parties. Your email is used to send you a receipt of your donation. Your credit card information is never saved by Vote USA.</p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
