<%@ Page Title="" Language="C#" MasterPageFile="~/Public2.Master" AutoEventWireup="true" 
CodeBehind="default2.aspx.cs" Inherits="Vote.Default2Page" %>

<%@ Register Src="/Controls/SiteVerification.ascx" TagName="SiteVerification" TagPrefix="user" %>
<%@ Register Src="/Controls/GoogleAddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
  <user:SiteVerification ID="SiteVerification" runat="server"/>
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <script type="text/javascript" src="/js/iframeResizer.contentWindow.js"></script>
  <style>
    .default-page .content
    {
      margin: 50px 100px 0;
    }

    .default-page .intro p
    {
      font-size: 20px;
    }

    .default-page .address-entry-outer
    {
      margin-top: 30px;
    }

    .default-page .big-orange-button
    {
      margin-right: 0;
    }

    .visit-research
    {
      margin-top: 50px;
      font-size: 13px;
    }

    body.iframed,
    .iframed #page-container
    {
      min-height: 310px;
    }

    .iframed header,
    .iframed footer, 
    .iframed .or-view,
    .iframed .elected-officials,
    .iframed .visit-research
    {
      display: none;
    }

    .iframed .outer-page
    {
      padding-bottom: 30px !important;
    }

    @media only screen and (max-width: 979px)
    {
      .default-page .content
      {
        margin: calc(6.06vw + 0.7px) calc(12.12vw - 18.8px) 0;
      }

      .default-page .big-orange-button
      {
        font-size: calc(0.455vw + 10.54px);
      }
    }

    @media only screen and (max-width: 399px)
    {
      .default-page .intro .head
      {
        font-size: 22pt;
      }

      .default-page .intro p
      {
        font-size: 16px;
      }
    }
  </style>
  <script>
    $(function() {
      if (PUBLIC.isIframed())
        $("body").addClass("iframed");
      $(".iframed .active-button-block ul").on("click", ".big-orange-button", function(event) {
        var $this = $(this);
        var href = $this.attr("href");
        var pos = href.indexOf('?');
        parent.postMessage("[ballot]" + href.substring(pos), "*");
        event.preventDefault();
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div class="intro">
    <h1>Vote informed&hellip;</h1>
    <p>Enter your postal address to view the office contests, candidates and ballot measures that will appear on your ballot.
      Candidate comparisons include pictures, bios, website and social media links, objectives, positions and views on issues.</p>
  </div><!-- no space
  --><div class="address-entry-outer">
    <div class="address-entry no-print">
      <user:AddressEntry ID="AddressEntry" runat="server"/>
    </div>
    <p class="visit-research">Visit <a href="/forresearch.aspx">Historical Voter Information</a> for officials and past elections from all states.</p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>