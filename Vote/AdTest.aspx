<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdTest.aspx.cs" Inherits="Vote.AdTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ad Test Page</title>
  <style type="text/css">
    .clear-both
    {
      clear: both;
    }
    .video-wrapper-outer {
      width: 33.3333333%;
      display: inline-block;
      float: right;
      overflow-y: hidden;
                         }
    .video-container {
      display: block;
      width: 100%;
      max-width: 420px;
                     }
    .video-player {
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
    .video-thumb {
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
    div.video-play-button {
      background: url(/images/yt-play-button2.png) no-repeat;
      height: 72px;
      width: 72px;
      left: 50%;
      top: 50%;
      margin-left: -36px;
      margin-top: -36px;
      position: absolute;
      opacity: 0.9;
                          }
    .ad-copy
    {
      display: inline-block;
      float: left;
      font-family: arial;
      margin-left: 20px;
    }
    .ad-copy p
    {
      margin: 0;
    }
    .ad-name
    {
      font-weight: bold;
      font-size: 16pt;
      margin: 30px 0 5px 0;
    }
    .ad-profile
    {
      display: inline-block;
      float: right;
      height: 100%;
      margin-right: 2px;
    }
    .bg
    {
      background: #aaaaaa;
      padding: 20px;
      width: 940px;
    }
    .w
    {
      background: #ffffff;
      margin-top: 20px;
    }
    .w940
    {
      width: 940px;
    }
    .w940 .ad-profile
    {
      height: 176px;
    }
    .w940 .ad-copy
    {
      margin-top: 50px;
    }
    .w940 .ad-name
    {
      font-size: 24pt;
    }
    .w940 .ad-profile
    {
      height: 176px;
    }
    .w760
    {
      width: 760px;
    }
    .w760 .ad-copy
    {
      margin-top: 40px;
    }
    .w760 .ad-name
    {
      font-size: 20pt;
    }
    .w760 .ad-profile
    {
      height: 143px;
    }
    .w759
    {
      width: 759px;
    }
    .w520
    {
      width: 520px;
    }
    .w520 .ad-copy
    {
      margin-top: 25px;
    }
    .w520 .ad-name
    {
      font-size: 16pt;
    }
    .w520 .paid-ad
    {
      font-size: 10pt;
    }
    .w520 .ad-profile
    {
      height: 98px;
    }
    .w519
    {
      width: 519px;
    }
    .w320
    {
      width: 320px;
    }
    .w520 .ad-copy
    {
      margin-top: 25px;
    }
    .w520 .ad-name
    {
      font-size: 16pt;
    }
    .w520 .paid-ad
    {
      font-size: 10pt;
    }
    .w320 .video-wrapper-outer
    {
      width: 50%
    }
    .w320 .ad-profile
    {
      height: 90px;
    }
    .w320 .ad-copy
    {
      margin: 0 0 0 10px;
    }
    .w320 .ad-copy p
    {
      display: inline-block;
      margin-top: 5px;
    }
    .w320 .ad-name
    {
      font-size: 12pt;
    }
    .w320 .paid-ad
    {
      font-size: 8pt;
    }
    .w520 .video-play-button,
    .w320 .video-play-button
    {
      width: 36px;
      height: 36px;
      background-size: contain;
      margin-left: -18px;
      margin-top: -18px;
    }
  </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="bg">
          <div class="w w940">
            <div class="ad-outer">
              <div class="video-wrapper-outer">
                <div class="video-container">
                  <div class="video-player">
                    <div>
                      <img class="video-thumb" src="http://i.ytimg.com/vi/n_blhQ9xQW0/hqdefault.jpg"/>
                      <div class="video-play-button"></div>
                    </div>
                  </div>
                </div>
              </div>
              <img class="ad-profile" src="/Image.aspx?Id=CAFeinsteinDianne&Col=Profile300"/>
              <div class="ad-copy">
                <p class="ad-name">Dianne Feinstein</p>
                <p class="paid-ad">Paid advertisement</p>
              </div>
              <div class="clear-both"></div>
            </div>
          </div>
          <div class="w w760">
            <div class="ad-outer">
              <div class="video-wrapper-outer">
                <div class="video-container">
                  <div class="video-player">
                    <div>
                      <img class="video-thumb" src="http://i.ytimg.com/vi/n_blhQ9xQW0/hqdefault.jpg"/>
                      <div class="video-play-button"></div>
                    </div>
                  </div>
                </div>
              </div>
              <img class="ad-profile" src="/Image.aspx?Id=CAFeinsteinDianne&Col=Profile300"/>
              <div class="ad-copy">
                <p class="ad-name">Dianne Feinstein</p>
                <p class="paid-ad">Paid advertisement</p>
              </div>
              <div class="clear-both"></div>
          </div>
          <!--<div class="w w759"></div>-->
          </div>
          <div class="w w520">
            <div class="ad-outer">
              <div class="video-wrapper-outer">
                <div class="video-container">
                  <div class="video-player">
                    <div>
                      <img class="video-thumb" src="http://i.ytimg.com/vi/n_blhQ9xQW0/hqdefault.jpg"/>
                      <div class="video-play-button"></div>
                    </div>
                  </div>
                </div>
              </div>
              <img class="ad-profile" src="/Image.aspx?Id=CAFeinsteinDianne&Col=Profile300"/>
              <div class="ad-copy">
                <p class="ad-name">Dianne Feinstein</p>
                <p class="paid-ad">Paid advertisement</p>
              </div>
              <div class="clear-both"></div>
            </div>
          </div>
          <!--<div class="w w519"></div>-->
          <div class="w w320">
            <div class="ad-outer">
              <div class="ad-copy">
                <p class="ad-name">Dianne Feinstein</p>
                <p class="paid-ad">Paid advertisement</p>
              </div>
              <div class="video-wrapper-outer">
                <div class="video-container">
                  <div class="video-player">
                    <div>
                      <img class="video-thumb" src="http://i.ytimg.com/vi/n_blhQ9xQW0/hqdefault.jpg"/>
                      <div class="video-play-button"></div>
                    </div>
                  </div>
                </div>
              </div>
              <img class="ad-profile" src="/Image.aspx?Id=CAFeinsteinDianne&Col=Profile300"/>
              <div class="clear-both"></div>
            </div>
          </div>
        </div>
    </form>
</body>
</html>
