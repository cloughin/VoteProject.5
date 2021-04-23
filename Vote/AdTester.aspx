<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="AdTester.aspx.cs" Inherits="Vote.AdTesterPage" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
  <style type="text/css">
    .donate-banner{display: none;}
    .ad-outer
    {
      border-bottom: 1px solid #888888;
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
      background: url(/images/yt-play-button.png) no-repeat;
      height: 7.66vw;
      width: 7.66vw;
      left: 50%;
      top: 50%;
      margin-left: -3.83vw;
      margin-top: -3.83vw;
      position: absolute;
      opacity: 0.7;
      background-size: contain;
    }
    .ad-outer .ad-copy
    {
      display: inline-block;
      float: left;
      font-family: arial;
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
      margin: 0;
    }
    .ad-outer .ad-profile
    {
      display: inline-block;
      float: right;
      height: 18.745vw;
      margin-right: 2px;
      max-width: 20%;
    }

    @media only screen and (min-width : 940px)
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
        height: 176px;
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
        margin-top: 5px;
      }
      .ad-outer .ad-name
      {
        font-size: 12pt;
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
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="BannerContent" ContentPlaceHolderID="MasterBannerContent" runat="server">
  <div class="ads-outer">
    <%=Vote.Utility.RenderOneAd("ILOBAMABARACK", "Barack Obama", "www.youtube.com/user/BarackObamadotcom", "ISdn7Dckq1I") %>
    <div class="ad-outer">
      <div class="ad-copy">
        <p class="ad-name">Barack Obama</p>
        <p class="paid-ad">Paid advertisement</p>
      </div>
      <div class="video-wrapper-outer">
        <div class="video-container">
          <div class="video-player">
            <div>
              <img class="video-thumb" src="http://i.ytimg.com/vi/ISdn7Dckq1I/hqdefault.jpg"/>
              <div class="video-play-button"></div>
            </div>
          </div>
        </div>
      </div>
      <img class="ad-profile" src="/Image.aspx?Id=ILOBAMABARACK&Col=Headshot100"/>
      <div style="clear:both"></div>
    </div>
    <div class="ad-outer">
      <div class="ad-copy">
        <p class="ad-name">George W. Bush</p>
        <p class="paid-ad">Paid advertisement</p>
      </div>
      <div class="video-wrapper-outer">
        <div class="video-container">
          <div class="video-player">
            <div>
              <img class="video-thumb" src="http://i.ytimg.com/vi/WejYdT3Lof8/hqdefault.jpg"/>
              <div class="video-play-button"></div>
            </div>
          </div>
        </div>
      </div>
      <img class="ad-profile" src="/Image.aspx?Id=TXBushGeorgeW&Col=Headshot100"/>
      <div style="clear:both"></div>
    </div>
  </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>The Page Content</h1>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
