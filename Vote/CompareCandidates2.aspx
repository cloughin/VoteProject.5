<%@ Page Title="" Language="C#" MasterPageFile="~/Public2.Master" AutoEventWireup="true" 
CodeBehind="CompareCandidates2.aspx.cs" Inherits="Vote.CompareCandidates2Page" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>
<%--<%@ Register Src="/Controls/Disqus.ascx" TagName="Disqus" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <script type="text/javascript" src="/js/iframeResizer.contentWindow.js"></script>
  <!--[if IE]>
    <style type="text/css">
      .responsive-report .answer-source::before
      {
        content: "";
      }

      .responsive-report .answer-source a::before
      {
        content: "Source: ";
        color: #222222;
      }

      .responsive-report .answer-source a
      {
        display: inline-block;
      }
    </style>
  <![endif]-->
  <style type="text/css">
    .ads-outer
    {
      background-color: #fff;
    }
    .ad-outer
    {
      border-bottom: 1px solid #ccc;
    }
    .ad-outer .video-wrapper-outer {
      width: 33.3333333%;
      display: inline-block;
      float: right;
      overflow-y: hidden;
                         }
    .ad-outer .video-container {
      display: block;
      width: 100%;
      max-width: 420px;
                     }
    .ad-outer .video-player {
      display: block;
      width: 100%;
      padding-bottom: 56.25%;
      overflow: hidden;
      position: relative;
      width: 100%;
      height: 100%;
      cursor: pointer;
      display: block;
                  }
    .ad-outer .video-thumb {
      bottom: 0;
      display: block;
      left: 0;
      margin: auto;
      max-width: 100%;
      width: 100%;
      position: absolute;
      right: 0;
      top: 0;
      height: auto;
                 }
    .ad-outer div.video-play-button {
      background: url(/images/yt-play-button2.png) no-repeat;
      height: 7.66vw;
      width: 7.66vw;
      left: 50%;
      top: 50%;
      margin-left: -3.83vw;
      margin-top: -3.83vw;
      position: absolute;
      opacity: 0.85;
      background-size: contain;
    }
    .ad-outer .ad-copy
    {
      display: inline-block;
      float: left;
      /*font-family: arial;*/
      margin: 5.32vw 0 0 2.13vw;
      max-width: 45%;
    }
    .ad-outer .ad-name
    {
      font-weight: bold;
      font-size: 3.25vw;
      margin: 0;
    }
    .ad-outer .paid-ad
    {
      font-size: 1.7vw;
      margin: 5px 0 0 0;
    }
    .ad-outer .ad-profile
    {
      display: inline-block;
      float: right;
      height: 18.745vw;
      margin-right: 2px;
      max-width: 20%;
    }

    .ad-inner
    {
      max-width: 980px;
      text-align: left;
      margin: 0 auto;
    }

    body.iframed,
    .iframed #page-container
    {
      min-height: 0;
    }

    .iframed header,
    .iframed footer,
    .iframed .back-to-ballot,
    .iframed .issue-list-link
    {
      display: none;
    }

    .iframed .outer-page
    {
      padding-bottom: 30px !important;
    }

    @media only screen and (min-width : 980px)
    {
      .ad-outer .ad-copy
      {
        margin: 50px 0 0 20px;
      }
      .ad-outer .ad-name
      {
        font-size: 24pt;
      }
      .ad-outer .paid-ad
      {
        font-size: 12pt;
      }
      .ad-outer .ad-profile
      {
        height: 184px;
      }
      .ad-outer div.video-play-button {
        height: 72px;
        width: 72px;
        margin-left: -36px;
        margin-top: -36px;
      }
    }

    @media only screen and (max-width : 480px)
    {
      .ads-outer
      {
        padding-top: 7px;
      }
      .ad-outer .video-wrapper-outer
      {
        width: 50%
      }
      .ad-outer .ad-profile
      {
        height: 28.08vw;
        max-width: none;
      }
      .ad-outer .ad-outer div.video-play-button {
        height: 11.50vw;
        width: 11.50vw;
        margin-left: -5.75vw;
        margin-top: -5.75vw;
                                      }
      .ad-outer .ad-copy
      {
        margin: 0 0 0 10px;
        float: none;
        display: block;
        max-width: none;
      }
      .ad-outer .ad-copy p
      {
        display: inline-block;
        margin-top: 6px;
      }
      .ad-outer .ad-name
      {
        font-size: 12pt;
        margin: 0 12px 0 0;
      }
      .ad-outer .paid-ad
      {
        font-size: 6pt;
      }
    }

    @media only screen and (max-width : 320px)
    {
      .ad-outer .ad-profile
      {
        height: 90px;
      }
    }
  </style>
  <script type="text/javascript" src="/js/jq/kalypto.js"></script>
  <script type="text/javascript">
    $(function () {
      $(".accordion-container").accordion({
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate,
        active: false
      });
      PUBLIC.initBallotCheckBoxes();
      PUBLIC.loadMyBallotChoices();
      PUBLIC.initBallotEvents();
      PUBLIC.setupBackToBallot();
      var $body = $("body");
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetAds",
        data: "{'electionKey':'" + $body.data("adelection") + "','officeKey':'" + $body.data("adoffice") + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
          if (result.d) {
            //setTimeout(function() { $(".donate-banner").hide().after(result.d); }, 500);
            setTimeout(function() { $(".header-inner").before(result.d); });
          }
        }
      });
    });
  </script>
  <script>
    $(function() {
      if (PUBLIC.isIframed())
        $("body").addClass("iframed");
      $(".iframed .candidate-more-info a").on("click", function(event) {
        var $this = $(this);
        var href = $this.attr("href");
        var pos = href.indexOf('?');
        parent.postMessage("[intro]" + href.substring(pos), "*");
        event.preventDefault();
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Compare the Candidates<br/><small>Biographical Profiles and Positions on the Issues</small>" />
  
  <div class="intro">
    <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
    <h2 id="OfficeTitle" class="office-title" runat="server"></h2>
  </div>
        
  <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"  EnableViewState="False"></asp:PlaceHolder>
  
<%--  <user:Disqus ID="Disqus" runat="server" />--%>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
