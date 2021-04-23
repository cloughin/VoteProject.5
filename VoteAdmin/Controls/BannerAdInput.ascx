<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BannerAdInput.ascx.cs" Inherits="VoteAdmin.Controls.BannerAdInput" %>

<div id="banner-ad-input">

<div class="input-element adtype">
  <div class="databox kalypto-radio-element" >
    <p class="fieldlabel">Ad Type <span class="reqd">◄</span></p>
    <div class="radio-container kalypto-radio-container" runat="server">
      <input id="AdTypeYouTube" class="kalypto youtube-type" type="radio" name="SetupAdAdType" value="Y"/>
      <label for="AdTypeYouTube">YouTube Video (video thumbnail used as graphic)</label>
      &nbsp;&nbsp;
      <input id="AdTypeImage" class="kalypto image-type" type="radio" name="SetupAdAdType" value="I"/>
      <label for="AdTypeImage">Display Image (custom graphic image)</label>
    </div></div>
</div>

<div class="input-element video hidden">
  <p class="fieldlabel">YouTube Video<span class="reqd">◄</span></p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="textdata youtube-url shadow-2"/>
    <div class="view-button view-video tiptip" title="View video"></div>
  </div>
  <p class="fieldnote">The YouTube thumbnail will be used as the ad graphic unless an optional image is uploaded.</p>
</div>
              
<div class="input-element imagefile">
  <p class="fieldlabel only-image hidden">Ad Image <span class="reqd">◄</span></p>
  <p class="fieldlabel only-youtube hidden">Image to Use Instead of Video Thumbnail (optional)</p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="image-file-name shadow-2" disabled="disabled" />
    <label class="button-1 button-smaller" for="AdImageFile">Browse...</label>
    <input type="file" id="AdImageFile" style="display:none"/>
  </div>
  <p class="fieldnote only-image">The image ad should be 810 pixels wide or wider and about half as high, like 400 to make it banner-shaped (much wider than tall).</p>
  <p class="fieldnote only-youtube">The image ad should be 400 pixels wide or wider and about half as high to make it banner shaped (wider than tall).</p>
</div>

<div class="input-element removeimage only-youtube clearfix">
  <div class="databox kalypto-checkbox">
    <div class="kalypto-container">
      <input class="kalypto remove-image"  type="checkbox" />
    </div>
    <div class="kalypto-checkbox-label">Check to remove the optional image</div>
  </div>
</div>
              
<div class="input-element targeturl hidden">
  <p class="fieldlabel">Target Page URL <span class="reqd">◄</span></p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="textdata target-url shadow-2" />
    <div class="view-button view-target tiptip" title="View target page"></div>
  </div>
  <p class="fieldnote">Url of the page to be presented when the ad is clicked, like a video url or web address.</p>
</div>
              
<div class="input-element description1">
  <p class="fieldlabel only-youtube">Ad Description Line 1</p>
  <p class="fieldlabel only-image">Ad Description Line 1 (optional)</p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="textdata description1 shadow-2" />
  </div>
  <p class="fieldnote">like: Sept 18, 2020 Arizona US Senate Debate OR Elect Guy Phillips for City Council.</p>
</div>
              
<div class="input-element description2">
  <p class="fieldlabel">Ad Description Line 2 (optional)</p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="textdata description2 shadow-2" />
  </div>
  <p class="fieldnote">like: Iowa PBS OR Daily Herald OR UMUR-TV.</p>
</div>
              
<div class="input-element descriptionurl">
  <p class="fieldlabel">Ad Description URL (optional)</p>
  <div class="databox textbox">
    <input type="text" spellcheck="false" class="textdata description-url shadow-2" />
    <div class="view-button view-url tiptip" title="View description URL"></div>
  </div>
  <p class="fieldnote">The media channel or website.</p>
</div>

<div class="input-element paidad clearfix">
  <div class="databox kalypto-checkbox">
    <div class="kalypto-container">
      <input class="kalypto ad-is-paid"  type="checkbox" />
    </div>
    <div class="kalypto-checkbox-label">Check to include an additional line to indicate the ad is a "Paid Advertisement"</div>
  </div>
</div>

<div class="input-element adenabled clearfix">
  <div class="databox kalypto-checkbox">
    <div class="kalypto-container">
      <input class="kalypto ad-enabled"  type="checkbox" />
    </div>
    <div class="kalypto-checkbox-label">Enable Ad</div>
  </div>
</div>
  
  <div>
    <input type ="button" value="Update" class="button-1 update-button"/>
  </div>
  
  <div id="ads" class="sample-ad"></div>

</div>
