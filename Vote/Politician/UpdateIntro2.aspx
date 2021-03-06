<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
ValidateRequest="false"
AutoEventWireup="true" CodeBehind="UpdateIntro2.aspx.cs" 
Inherits="Vote.Politician.UpdateIntro2Page" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/SubHeadingWithHelp.ascx" TagName="SubHeadingWithHelp" TagPrefix="user" %>

<asp:Content ID="HeadContent" EnableViewState="false" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
  <div class="main-heading">
  <asp:UpdatePanel ID="UpdatePanelHeading" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
      <h1 id="H1" runat="server"></h1>
    </ContentTemplate>
  </asp:UpdatePanel>  
  <h3 id="H2" EnableViewState="false" runat="server"></h3>
  </div>
  
  <div class="politician-links">
      <div class="actions intro-actions">
      <a class="action gradient-bg-1 disabled shadow rounded-border" id="UpdateIntroLink" runat="server">
      <p><em><span>►</span>Update</em> your intro page</p>
      </a>
      <a class="action gradient-bg-1-hovering shadow rounded-border" id="ShowIntroLink" runat="server" target="_Show">
      <p><em><span>►</span>View</em> your intro page</p>
      </a>
      </div>
      <div class="actions issue-actions">
      <a class="action gradient-bg-1-hovering shadow rounded-border" id="UpdateIssuesLink" runat="server">
      <p><em><span>►</span>Enter</em> your positions on the issues</p>
      </a>
      <a class="action gradient-bg-1-hovering shadow shadow rounded-border" id="ShowPoliticianIssueLink" runat="server" target="_Show">
      <p><em><span>►</span>View</em> your positions page</p>
      </a>
      </div>
  </div>

  <div class="content-header maint-container shadow rounded-border clear-both">
    <user:SubHeadingWithHelp ID="PageCachingSubHeadingWithHelp" EnableViewState="false" runat="server"
      Title="Enter or Update the Information on Your Introduction Page" CssClass="tiptip"
      Tooltip="Show/hide help">
      <ContentTemplate>
        <p><strong><em>A note about caching...</em></strong></p>
        <p>When you make changes to your pages through these tools and then click the 
        <strong><a title="View Intro Page" id="ViewIntroLink" EnableViewState="false" target= "_Show" runat="server">
        View Intro Page</a></strong> menu item, we immediately show you how the 
        changes will affect your pages. However, pages on the Vote-USA.com public site 
        are <em>cached</em>, meaning they are not immediately updated with changes. 
        The changes you make here could take up to <span id="CacheExpirationMsg" runat="server"></span>
        to be seen on the public site.</p>
     </ContentTemplate>
    </user:SubHeadingWithHelp>
  </div>

  <div runat="server" id="ContainerUpdateAll" class="content-footer maint-container shadow rounded-border mc mc-g-containerall">
    <asp:UpdatePanel ID="UpdatePanelUpdateAll" EnableViewState="false" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <div class="footer-item footer-feedback">
          <user:FeedbackContainer ID="FeedbackUpdateAll" EnableViewState="false" 
          Placeholder="Use <span>Update All</span> to apply all pending changes at once. Or click <span>Update</span> at the bottom of any panel to apply just those changes."
          CssClass="mc mc-g-feedbackall" runat="server" />
        </div>
        <div class="footer-item footer-ajax-loader">
          <asp:Image ID="ImageAjaxLoader" EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
          CssClass="ajax-loader mc mc-g-ajaxloader" runat="server" />
        </div>
        <div class="footer-item footer-button">
          <asp:Button ID="ButtonUpdateAll" EnableViewState="false" CssClass="button-2 mc mc-g-buttonall tiptip" 
          Tooltip="Update all unsaved data"
          OnClick="ButtonUpdateAll_OnClick" runat="server" Text="Update All" />
        </div>
        <div class="clear-both"></div>
      </ContentTemplate>
    </asp:UpdatePanel>    
  </div>

  <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">

      <ul class="tabs htabs unselectable">
          <li id="Li1" class="mt-general tab htab" EnableViewState="false" runat="server"><a href="#tab-contact" onclick="this.blur()" id="TabMainContact" EnableViewState="false" runat="server">Contact & General Info</a></li>
          <li id="Li2" class="mt-bio2 tab htab" EnableViewState="false" runat="server"><a href="#tab-bio2" onclick="this.blur()" id="TabMainBio2" EnableViewState="false" runat="server">Biographical Info</a></li>
          <li id="Li3" class="mt-reasons tab htab" EnableViewState="false" runat="server"><a href="#tab-reasons" onclick="this.blur()" id="TabMainReasons" EnableViewState="false" runat="server">Reasons & Objectives</a></li>
          <li id="Li4" class="mt-social tab htab" EnableViewState="false" runat="server"><a href="#tab-social" onclick="this.blur()" id="TabMainSocial" EnableViewState="false" runat="server">Web Site & Social Media Links</a></li>
          <li id="Li5" class="mt-upload tab htab" EnableViewState="false" runat="server"><a href="#tab-upload" onclick="this.blur()" id="TabMainUpload" EnableViewState="false" runat="server">Upload Picture</a></li>
          <%--
          --%>
      </ul>
    
      <div id="tab-contact" class="content-panel tab-panel htab-panel">

        <asp:UpdatePanel ID="UpdatePanelContact" 
          UpdateMode="Conditional" runat="server">
          <ContentTemplate>
            <div id="ContainerContact" runat="server">
            <input type="hidden" id="DescriptionContact" EnableViewState="false" class="mc-mt-general" runat="server" />
            <div class="inner-panel rounded-border">
              <div class="tab-panel-heading horz-center"><div class="center-inner">
                <div id="Div1" runat="server" EnableViewState="false" class="undo-button tiptip"
                 title="Revert all Contact and General Information to the latest saved version">
                  <div id="UndoContact" EnableViewState="false" runat="server"></div>
                </div>
                 <h4 class="center-element">Contact and General Information</h4>
              </div></div>

              <hr />
              <h5>Name as Shown on Ballots: <em id="NameOnBallots" EnableViewState="false" runat="server"></em></h5>

              <div class="input-element fname">
                <p class="fieldlabel">First Name <span class="reqd">◄</span></p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactFName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactFName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element mname">
                <p class="fieldlabel">Middle or Initial</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactMName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactMName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element nickname">
                <p class="fieldlabel">Nickname</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactNickname" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactNickname" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element lname">
                <p class="fieldlabel">Last Name <span class="reqd">◄</span></p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactLName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactLName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element suffix">
                <p class="fieldlabel">Suffix</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactSuffix" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactSuffix" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both"></div>

              <p class="ballot-name-info" id="BallotNameInfo" runat="server">
              Your name as shown above was provided by the election authority. This is
              how it will be shown on ballots. It can only be changed by our 
              election manager. Any quotes or parentheses are not shown &mdash; these
              will be automatically added when the ballot names are presented.
              </p>

              <div class="spacer"></div>

              <hr />
              <h5>Contact Information</h5>

              <div class="input-element publicaddress">
                <p class="fieldlabel">Street Address</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactPublicAddress" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactPublicAddress" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element publiccitystatezip">
                <p class="fieldlabel">City, State Zip</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactPublicCityStateZip" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactPublicCityStateZip" EnableViewState="false" runat="server"></div>
               </div>
              </div>

              <div class="clear-both spacer"></div>

              <div class="input-element publicphone">
                <p class="fieldlabel">Phone</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactPublicPhone" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactPublicPhone" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element publicemail">
                <p class="fieldlabel">Email</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactPublicEmail" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactPublicEmail" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both spacer"></div>

              <hr />
              <h5>Date of Birth and Age</h5>

              <div class="input-element dateofbirth">
                <p class="fieldlabel">Date of Birth</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactDateOfBirth" EnableViewState="false" CssClass="shadow-2 date-picker-dob" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskContactDateOfBirth" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element age">
                <p class="fieldlabel">Age</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlContactAge" EnableViewState="false" CssClass="shadow-2" enabled="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                </div>
              </div>

              <div class="clear-both spacer"></div>

              <hr />
              <h5>Political Party:  <em id="PartyName" EnableViewState="false" runat="server"></em></h5>

              <div class="input-element partykey">
                <p class="fieldlabel">Select a Party</p>
                <div class="databox dropdown">
                  <div class="shadow-2">
                    <user:DropDownListWithOptionGroup id="ControlContactPartyKey" runat="server"></user:DropDownListWithOptionGroup>
                  </div>
                  <div class="tab-ast" id="AsteriskContactPartyKey" EnableViewState="false" runat="server"></div>
                </div>              
              </div>

              <div class="clear-both spacer"></div>

              <hr />
              <div class="feedback-floater-for-ie7">
                <user:FeedbackContainer ID="FeedbackContact" EnableViewState="false" runat="server" />
              </div>
              <div class="update-button">
                <asp:Button ID="ButtonContact" EnableViewState="false" runat="server" 
                Text="Update" CssClass="update-button button-1 tiptip" 
                OnClick="ButtonContact_OnClick" /> 
              </div>   
              <div class="clear-both"></div>   
              </div>            
            </div>
          </ContentTemplate>
        </asp:UpdatePanel>
      </div>
      
      <div id="tab-bio2" class="content-panel tab-panel htab-panel">

        <div class="bio2-tabs tab-control vtab-control jqueryui-tabs">

          <asp:PlaceHolder ID="PlaceHolderBio2" EnableViewState="false" runat="server"></asp:PlaceHolder>

        </div>

      </div>

      <div id="tab-reasons" class="content-panel tab-panel htab-panel">

        <div class="reasons-tabs tab-control vtab-control jqueryui-tabs">

          <asp:PlaceHolder ID="PlaceHolderReasons" EnableViewState="false" runat="server"></asp:PlaceHolder>

        </div>

      </div>

      <div id="tab-social" class="content-panel tab-panel htab-panel">
        <asp:UpdatePanel ID="UpdatePanelSocial"  EnableViewState="false"
          UpdateMode="Conditional" runat="server">
          <ContentTemplate>
            <div id="ContainerSocial" runat="server">
            <input type="hidden" id="DescriptionSocial" EnableViewState="false" class="mc-mt-social" runat="server" />
            <div class="inner-panel rounded-border">
              <div class="tab-panel-heading horz-center"><div class="center-inner">
                <div id="Div2" EnableViewState="false" runat="server" class="undo-button tiptip"
                 title="Revert all Web Site & Social Media Links to the latest saved version">
                  <div id="UndoSocial" EnableViewState="false" runat="server"></div>
                </div>
                 <h4 class="center-element">Web Site and Social Media Links</h4>
              </div></div>

              <hr />
              <h5>Your Web Site</h5>

              <div class="medium website rounded-border shadow">
                <div class="icon-box"><a class="icon" target="_Website" id="IconBoxPublicWebAddress" EnableViewState="false" runat="server"></a></div>
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialPublicWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                <div class="tab-ast" id="AsteriskSocialPublicWebAddress" EnableViewState="false" runat="server"></div>
                <div class="clear-both"></div>
              </div>

              <div class="instructions">
                <ul>
                  <li><span>Click an icon to view your page</span></li>
                  <li><span>Click <em>Update</em> at the bottom of the panel to save your changes</span></li>
                  <li><span>The http:// prefix is optional on all addresses</span></li>
                </ul>
              </div>

              <div class="clear-both"></div>

              <hr />
              <h5>Social Media</h5>

              <div class="cols">

                <div class="col col1">

                  <div class="medium facebook rounded-border shadow">
                    <h6 id="HeadingFacebookWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxFacebookWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialFacebookWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialFacebookWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium youtube rounded-border shadow">
                    <h6 id="HeadingYouTubeWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxYouTubeWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialYouTubeWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialYouTubeWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium flickr rounded-border shadow">
                    <h6 id="HeadingFlickrWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxFlickrWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialFlickrWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialFlickrWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium twitter rounded-border shadow">
                    <h6 id="HeadingTwitterWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxTwitterWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialTwitterWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialTwitterWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium rssfeed rounded-border shadow">
                    <h6 id="HeadingRSSFeedWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxRSSFeedWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialRSSFeedWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialRSSFeedWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium wikipedia rounded-border shadow">
                    <h6 id="HeadingWikipediaWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxWikipediaWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialWikipediaWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialWikipediaWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium ballotpedia rounded-border shadow">
                    <h6 id="HeadingBallotPediaWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxBallotPediaWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialBallotPediaWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialBallotPediaWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                </div>

                <div class="col col2">

                  <div class="medium vimeo rounded-border shadow">
                    <h6 id="HeadingVimeoWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxVimeoWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialVimeoWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialVimeoWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium googleplus rounded-border shadow">
                    <h6 id="HeadingGooglePlusWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxGooglePlusWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialGooglePlusWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialGooglePlusWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium linkedin rounded-border shadow">
                    <h6 id="HeadingLinkedInWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxLinkedInWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialLinkedInWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialLinkedInWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium pinterest rounded-border shadow">
                    <h6 id="HeadingPinterestWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxPinterestWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialPinterestWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialPinterestWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium blogger rounded-border shadow">
                    <h6 id="HeadingBloggerWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxBloggerWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialBloggerWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialBloggerWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                  <div class="medium webstagram rounded-border shadow">
                    <h6 id="HeadingWebstagramWebAddress" EnableViewState="false" runat="server"></h6>
                    <div class="icon-box"><a class="icon" target="_Show" id="IconBoxWebstagramWebAddress" EnableViewState="false" runat="server"></a></div>
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSocialWebstagramWebAddress" EnableViewState="false" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskSocialWebstagramWebAddress" EnableViewState="false" runat="server"></div>
                    <div class="clear-both"></div>
                  </div>   

                </div>

              </div>
              <div class="clear-both"></div>

              <hr />
              <div class="feedback-floater-for-ie7">
                <user:FeedbackContainer ID="FeedbackSocial" EnableViewState="false" runat="server" />
              </div>
              <div class="update-button">
                <asp:Button ID="ButtonSocial" EnableViewState="false" runat="server" 
                Text="Update" CssClass="update-button button-1 tiptip" 
                OnClick="ButtonSocial_OnClick" /> 
              </div>   
              <div class="clear-both"></div>               
            </div>            
            </div>
         </ContentTemplate>
        </asp:UpdatePanel>
      </div>

      <div id="tab-upload" class="content-panel tab-panel htab-panel">
        <div class="inner-panel rounded-border">
          <div class="tab-panel-heading horz-center"><div class="center-inner">
             <h4 class="center-element">Upload Your Picture</h4>
          </div></div>
         <hr />
         <div class="cols">
            <input type="hidden" id="UploadUrl" EnableViewState="false" class="upload-url mc mc-g-uploadurl" runat="server" />
            <input type="hidden" id="DescriptionUpload" EnableViewState="false" runat="server" />
            <div class="col col1 shadow rounded-border">
              <div class="nx n1"></div><h6>Click <em>Browse</em> to select a picture</h6>
              <div class="fileinputs">
                <div class="masker masker1"></div>
                <div class="masker masker2"></div>
                <input type="file" id="ControlUpload" name="ControlUpload" runat="server"
                class="upload-file mc-mt-upload" ClientIDMode="static"  />
                <input type="button" value="Browse" onclick="$$('ControlUpload').click()"
                class="button-1 browse-button" />
                <div class="file-name-container">
                  <p id="UploadFilename" class="shadow-2 file-name mc mc-filename mc-group-upload" />
                  <div class="file-name-clear tiptip" title="Clear the selected filename"></div>
                  <div class="clear-both"></div>               
                </div>
              </div>
              <div class="nx n2"></div><h6>Click <em>Upload</em> at the bottom of the panel</h6>
            </div>
            <div class="col col2">
            <div class="image-outer-container">
              <div class="image-container">
                <asp:Image ID="ImagePicture" EnableViewState="false" CssClass="center-element image-picture" runat="server" />
              </div>
              <p class="too-small rounded-border">Your picture is smaller than what we 
              like to show on many of our pages. The gray area to the right shows how 
              much bigger your picture could be. Please consider replacing your current 
              image with a larger picture.</p>
            </div>
            </div>
            <div class="clear-both"></div>     
            <hr />
          </div>
          <div class="feedback-floater-for-ie7">
            <user:FeedbackContainer ID="FeedbackUpload" EnableViewState="false" runat="server" />
          </div>
          <div class="update-button">
            <input type="button" id="ButtonUpload" EnableViewState="false" runat="server" value="Upload"
            class="update-button button-1 tiptip"/>
         </div>   
          <div class="clear-both"></div>               
        </div>            

      </div>

  </div>

</div>
</asp:Content>
