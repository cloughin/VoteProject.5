<%@ Page Title="" Language="C#" MasterPageFile="~/Public2.Master" AutoEventWireup="true" 
CodeBehind="Ballot2.aspx.cs" Inherits="Vote.Ballot2Page" %>

<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>
<%@ Register Src="/Controls/SampleBallotEmailDialogResponsive.ascx" TagName="SampleBallotEmailDialog" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
  <meta property="og:url" id="OgUrl" ClientIDMode="Static" runat="server" />
  <meta property="og:title" id="OgTitle" ClientIDMode="Static" runat="server" />
  <meta property="og:description" id="OgDescription" ClientIDMode="Static" runat="server" />
  <meta property="og:image" id="OgImage" ClientIDMode="Static" runat="server" />
  <meta property="og:image:width" id="OgImageWidth" ClientIDMode="Static" runat="server" />
  <meta property="og:image:height" id="OgImageHeight" ClientIDMode="Static" runat="server" />
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <script type="text/javascript" src="/js/iframeResizer.contentWindow.js"></script>
  <style type="text/css">
    .selection-panel div.share-choices {
      display: inline-block;
      vertical-align: top;
    }
    .selection-panel div.share-choices.fbshare2 a {
      width: 63px;
      height: 24px;
      background-image: url(/images/fbshare.png);
      display: block;
    }
    .selection-panel div.share-choices.fbshare2 a:hover {
      opacity: 0.7;
    }
    .selection-panel .instructions {
      font-size: 10pt;
      margin-bottom: 3px;
      color: #666;
      font-style: italic;
    }
    .selection-panel .instructions.head {
      font-weight: bold;
      font-size: 11pt;
      margin: -5px 0 1px 0;
      color: #d16b05;
      font-style: normal;
    }
    .content .title-and-location {
      margin-bottom: 15px;
    }   

    .offer-to-print a {
      cursor: pointer;
    }

    .ballot2-page .content h1
    {
      margin: -8px 0 16px
    }
   
    .ballot-report .county-contests,
    .ballot-report .local-contests,
    .ballot-report .one-local-contests,
    .ballot-report .local-districts-header,
    .ballot-referendum-report .referendums-list,
    .ballot-referendum-report .districts-list {
      margin-top: 20px;
    }
    .ballot-referendum-report .districts-list .referendums-list:first-child {
      margin-top: 0;
    }
    .ballot-referendum-report .districts-list .referendums-list {
      margin-top: 10px;
    }

    .ballot-report .office-cell {
      display: inline-block;
      vertical-align: top;
      width: 33.3333333%;
      padding: 8px 4px;
      /*font-family: sans-serif;*/
    }

    .ballot-report .office-cell-inner {
      border: 1px solid #333333;
      border-bottom: none;
    }

    .ballot-report .office-heading {
      border-bottom: 1px solid #333333;
      text-align: center;
      background-color: #f0f0f0;
      padding: 5px;
    }

    .ballot-report .office-heading-name {
      font-weight: bold;
      padding-bottom: 3px;
    }

    .ballot-report .office-heading-name span {
      display: block;
    }

    .ballot-report .office-heading-vote-for 
    {
      font-size: .75rem;
      padding-bottom: 3px;
    }
    
    .ballot-report .candidate-cell {
      display: block;
      width: 100%;
      padding: 0;
    }
    
    .ballot-report .candidate-cell.write-in-cell {
      padding-top: 4px;
    }
    
    .ballot-report .candidate-cell-inner {
      border-top: none;
      border-left: none;
      border-right: none;
    }
    
    .ballot-report .candidate-name span.running-mate {
      display: block;
      margin-left: 17px;
    }   
    
    .ballot-report .candidate-image img {
      padding: 2px;
    }
    
    .ballot-report .candidate-image img.running-mate-image {
      padding-top: 0;
    }
    
    .ballot-report .write-in-wording {
      padding: 2px 0 4px 0;
      display: inline-block;
    }
    
    .ballot-report .write-in-text {
      border: none;
      border-top: 1px solid #dddddd;
      padding: 5px 4px;
    }
  
    .ballot-referendum-report .referendum-content
    {
      padding-left: 1em;
    }
  
    .ballot-referendum-report .yes-no
    {
      vertical-align: top;
      display: block;
      float: left;
      width: 70px;
      margin-top: 3px;
    }
  
    .ballot-referendum-report .yes-no > span
    {
      display: block;
      margin-bottom: 10px;
    }
  
    .ballot-referendum-report a.kalypto-checkbox
    {
      display: inline-block;
    }
  
    .ballot-report input[type=checkbox].candidate-checkbox
    {
      float: left;
      margin-top: 6px;
    }
 
    .ballot-referendum-report .yes-no .label
    {
      font-size: .95rem;
      display: inline-block;
      padding-left: 4px;
      vertical-align: top;
    }
 
    .ballot-referendum-report .referendum-right-content
    {
      vertical-align: top;
    }
    
    .instructions-accordion.ui-accordion .ui-accordion-header {
      font-size: 17px;
      font-weight: bold;
    }
    
    .instructions-accordion.ui-accordion .ui-accordion-content {
      font-size: 15px;
    }
    
    .instructions-accordion.ui-accordion .ui-accordion-content em {
      font-weight: bold;
    }

    body .selection-panel {
      padding-top: 10px;
      display: block;
      background: #dddddd;
      border-bottom: 1px solid #666666;
      width: 100%;
      height: 46px;
      height: 70px;
      position: fixed;
      z-index: 1;
      top: 0;
      left: 0;
      text-align: center;
      background: #ffffff; /* Old browsers */
      background: -moz-linear-gradient(top,  #ffffff 0, #f6f6f6 47%, #ededed 100%); /* FF3.6-15 */
      background: -webkit-linear-gradient(top,  #ffffff 0,#f6f6f6 47%,#ededed 100%); /* Chrome10-25,Safari5.1-6 */
      background: linear-gradient(to bottom,  #ffffff 0,#f6f6f6 47%,#ededed 100%); /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */
      filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ededed',GradientType=0 ); /* IE6-9 */
    }
    
    body.has-choices-panel .selection-panel {
      height: 140px;
    }
    
    .selection-panel .button-panel {
      height: 53px;
    }
    
    .selection-panel .choices-panel {
      display: none;
    }
    
    body.has-choices-panel .selection-panel .choices-panel {
      display: block;
    }

    .selection-panel select {
      width: 150px;
      padding: 5px;
    }

    .selection-panel select.friend {
      color: red;
    }

    .selection-panel select option {
      color: black;
    }

    .selection-panel input {
      margin: 0 8px;
    }

    .selection-panel input[type=button] {
      cursor: pointer;
    }

    .selection-panel .buttons {
      display: inline-block;
    }
    
    header {
      margin-top: 70px;
    }

    body.has-choices-panel #page-container header {
      margin-top: 140px;
                                                  }
    
    #share-ballot-dialog {
      display: none;
    }
    
    #share-ballot-dialog {
      text-align: left;
    }
    
    #share-ballot-dialog p.intro {
      font-size: 10pt;
      line-height: 120%;
    }
    
    #share-ballot-dialog label,
    #share-ballot-dialog input {
      display: block;
    }
    
    #share-ballot-dialog label {
      margin: 15px 0 5px 0;
      font-size: 12pt;
      /*font-family: arial;*/
      font-weight: bold;    
    }
    
    #share-ballot-dialog input[type=button] 
    {
      margin-top:  15px;
    }
    
    .share-ballot-dialog .ui-dialog-title {
      font-size: 10pt;
      width: auto;
    }

    .incumbent-note
    {
      font-size: 10pt;
      margin-left: 30px;
      display: block;
      text-indent: -10px;

    }

    body.iframed,
    .iframed #page-container
    {
      min-height: 0;
    }

    .iframed header,
    .iframed footer,
    .iframed .selection-panel,
    .iframed .sign-up-button
    {
      display: none;
    }

    .iframed .outer-page
    {
      padding-bottom: 30px !important;
    }

    @media only screen and (max-width : 799px) 
    {
      .ballot-report .office-cell {
        width: 50%;
                                  }
    }
    
    @media only screen and (max-width : 479px) 
    {
      .ballot-report .office-cell {
        width: 100% !important;
      }
    }
     
    @media only screen and (max-width : 759px) 
    {
      /* small & medium*/
      .content .intro .location-info {
        float: none;
        margin-bottom: 10px;
      }    

      body.has-choices-panel .selection-panel {
        height: 160px;
                                              }

      body.has-choices-panel #page-container header {
        margin-top: 160px;
                                                    }

      .selection-panel .buttons {
        display: block;
        margin-top: 5px;
      }
    }
   
    @media print 
    {
    
      header {
        margin-top: 0;
             }

      .incumbent-note
      {
        margin-top: 10px;
      }
    }
    
  </style>
  <script type="text/javascript" src="/js/jq/kalypto.mod.js"></script>
<%--  <script type="text/javascript" src="/js/vote/publicutil.js"></script>--%>
  <script>
    $(function() {
      if (PUBLIC.isIframed())
        $("body").addClass("iframed");
      $(".iframed .compare-candidates").on("click", function(event) {
        var $this = $(this);
        var href = $this.attr("href");
        var pos = href.indexOf('?');
        parent.postMessage("[compare]" + href.substring(pos), "*");
        event.preventDefault();
      });
      $(".iframed .candidate-more-info a").on("click", function(event) {
        var $this = $(this);
        var href = $this.attr("href");
        var pos = href.indexOf('?');
        parent.postMessage("[intro]" + href.substring(pos), "*");
        event.preventDefault();
      });
      $(".iframed .referendum-details a").on("click", function(event) {
        var $this = $(this);
        var href = $this.attr("href");
        var pos = href.indexOf('?');
        parent.postMessage("[referendum]" + href.substring(pos), "*");
        event.preventDefault();
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div class="selection-panel no-print">
    <div class="button-panel">
      <p class="head instructions">Share your ballot choices with friends!</p>
      <p class="instructions">Make your choices then select one of these buttons.</p>
      <div class="share-choices"><input type="button" class="button-4 button-smaller share-choices-button" value="Share via email"/></div>
      <!--<div class="share-choices fbshare"></div>-->
      <div class="share-choices fbshare2"><a href="#" class="fbshare2"></a></div>
    </div>
    <div class="choices-panel hidden">
      <hr/>
      <span>Choices to display: </span>
      <select class="friends-dropdown no-zoom"><option value="">My choices</option></select>
      <div class="buttons">
      <input type="button" class="button-1 button-smallest make-my-choices-button" value="Make these my choices"/>
      <input type="button" class="button-3 button-smallest delete-choices-button" value="Delete these choices"/>
      </div>
    </div>
  </div>
  <div id="InnerContent" runat="server">
    <div id="LocationInfo1" class="location-info location-info1" runat="server"></div>
    <div class="intro">
      <user:PageHeading ID="PageHeading" runat="server" MainHeadingText="Your Ballot Choices" />
      <div class="title-and-location clearfix">
        <h1 id="ElectionTitle" class="election-title" runat="server"></h1>
        <div id="LocationInfo2" class="location-info location-info2" runat="server"></div>
        <input type="button" class="sign-up-button button-4 button-smaller no-print" value="Get Future Ballot Choices via Email">
      </div>
      <div class="instructions-accordion" id="InstructionsAccordion" runat="server">
        <div>Voting Information</div>
        <div>
          <asp:PlaceHolder ID="VotingInformation" runat="server"></asp:PlaceHolder>
          <asp:PlaceHolder ID="AdditionalInformation" runat="server"></asp:PlaceHolder>
          <asp:PlaceHolder ID="BallotInstructions" runat="server"></asp:PlaceHolder>
        </div>
      </div>
      <p id="Instructions" class="instructions no-print" runat="server"></p>
      <asp:PlaceHolder id="AdditionalInfo" runat="server"></asp:PlaceHolder>
    </div>

    <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="ReferendumReportPlaceHolder" runat="server"></asp:PlaceHolder>

  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
  <user:SampleBallotEmailDialog ID="SampleBallotEmailDialog" runat="server" />
  <div id="share-ballot-dialog">
    <p class="intro">Enter your name and email address. We'll send a you an email with a link that you can forward to friends and colleagues.</p>
    <label>Your name:</label>
    <input type="text" name="name" class="share-ballot-name no-zoom" required="required" autocomplete="name" />
    <label>Your email address:</label>
    <input type="email" name="email" class="share-ballot-email no-zoom" required="required" autocomplete="email" />
    <input type="button" class="share-ballot-button button-1 button-smaller" value="Send Email"/>
  </div>
  <!--<div id="fbshare-dialog">
    <iframe />
  </div>-->
</asp:Content>
