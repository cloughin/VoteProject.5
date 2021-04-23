<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
CodeBehind="UpdateOrganizations.aspx.cs" Inherits="VoteAdmin.Master.UpdateOrganizationsPage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    .main-tab .mode-label-line
    {
      margin-top: 10px;
    }

    .main-tab .mode-label
    {
      font-size: 9pt;
      margin-right: 16px;
      font-weight: bold;
    }

    .main-tab.change-mode .mode-label:before
    {
      content: "In Change Mode"
    }

    .main-tab.change-mode .mode-button:before
    {
      content: "Switch to Add Mode"
    }

    .main-tab:not(.change-mode) .change-only
    {
      display: none;
    }

    .main-tab.change-mode .add-only
    {
      display: none;
    }

    .main-tab.add-mode .mode-label:before
    {
      content: "In Add Mode"
    }

    .main-tab.add-mode .mode-button:before
    {
      content: "Switch to Change Mode"
    }

    .main-tab.has-changes .save-button:before
    {
      content: "Save Changes"
    }

    .main-tab:not(.has-changes) .save-button:before
    {
      content: "No Changes to Save"
    }

    .main-tab .col
    {
      float: left;
    }

    .main-tab .col .fieldlabel
    {
      margin-top: 14px;
      color: #bf9241;
      font-weight: bold;
    }

    .main-tab .col .fieldlabel span
    {
      color: #222;
      font-weight: normal;

    }

    .main-tab .col1
    {
      max-width: 250px;
    }

    .main-tab .col2
    {
      max-width: 590px;
      margin-left: 20px;
    }

    .main-tab .col2 .field
    {
      display: inline-block;
      margin-right: 10px;
      vertical-align: top;
    }

    .main-tab .centered-col
    {
      display: inline-block;
      text-align: left;
    }

    #tab-organizations .organization-name input
    {
      width: 360px;
    }

    #tab-organizations .organization-ideology select
    {
      width: 150px;
    }

    #tab-organizations .organization-subtype select
    {
      width: 200px;
    }

    #tab-organizations .organization-emailtags
    {
      width: 200px;
    }

    #tab-organizations .organization-emailtags span
    {
      display: inline-block;
      vertical-align: top;
    }

    #tab-organizations .organization-emailtags span
    {
      max-width: 200px;
    }

    #tab-organizations .organization-emailtags .checkboxes div
    {
     text-indent: -20px;
      margin-left: 20px;
    }

    #tab-organizations .organization-address input
    {
      width: 270px;
    }

    #tab-organizations .organization-zip input
    {
      width: 100px;
    }

    #tab-organizations .organization-url input
    {
      width: 470px;
    }

    #tab-organizations .field textarea
    {
      width: 570px;
      resize: vertical;
    }

    #tab-organizationnotes .notes
    {
      margin: 10px 0 10px 0;
      border: 1px solid #aaaaaa;
      height: 200px;
      overflow-y: scroll;
      padding: 5px;
      width: 550px;
    }

    #tab-organizationnotes .new-note textarea
    {
      width: 562px;
      height: 75px;
      box-sizing: border-box;
      padding: 5px;
      margin: 0;
    }

    #tab-organizationnotes .add-button
    {
      margin-top: 5px;
    }

    #tab-organizationnotes .note-header .date
    {
      font-weight: bold;
      font-size: 90%;
    }

    #tab-organizationnotes .note-header .link
    {
      margin-left: 10px;
      text-decoration: underline;
      font-size: 90%;
    }

    #tab-organizationnotes .note-body
    {
      margin-bottom: 10px;
      border: 1px solid #ffffff;
      display: inline-block;
      width: 100%;
    }
    #tab-organizationnotes .note-body.editing {
      background-color: #eeeecc;
      border-color: #aaaaaa;
                                               }

    .image-tab .image-file-name
    {
      margin-right: 10px;
      width: 460px;
    }

    .image-tab .action-button
    {
      margin-top: 15px;
      display: block !important;
    }

    #tab-organizationad .ad-url
    {
      width: 460px;
    }

    #tab-organizationad .default-url
    {
      margin-top: 5px;
    }

    #tab-organizationad .sample-ad-content
    {
      /*width: 980px;*/
      text-align: center;
      /*margin-left: -60px;*/
      width: 100%;
    }

    #tab-organizationad .sample-ad-content img
{width: 100%;}

    #tab-organizationad .sample-ad-content hr
    {display: none;}

    #tab-organizationad .organizations
    {height: 200px;}

    #tab-organizationad .fieldnote
    {
      font-size: 8pt;
      width: 480px;
      margin-top: 2px;
    }

    #tab-organizationtypes .organization-type input,
    #tab-organizationsubtypes .organization-subtype input,
    #tab-emailtags .email-tag input
    {
      width: 360px;
    }
    #tab-report .field {
      display: inline-block;
      vertical-align: top;
      margin-right: 10px;
                       }

    #tab-report .report-type select
    {
      width: 125px;
    }

    #tab-report .report-subtype select
    {
      width: 110px;
    }
    #tab-report .report-ideology select
    {
      width: 115px;
    }
    #tab-report .report-state select
    {
      width: 120px;
    }
    #tab-report .report-emailtags
    {
      width: 200px;
    }

    #tab-report .found-count
    {
      margin-top: 5px;
    }

    #tab-report .organization-scroller
    {
      max-height: 420px;
      overflow-y: auto;
      border: 1px solid #ccc;
    }

    #tab-report .organization-scroller table
    {
      width: 100%;
      font-size: 90%;
    }

    #tab-report .organization-scroller thead
    {
      background-color: #000;
      color: #fff;
    }

    #tab-report .organization-scroller th,
    #tab-report .organization-scroller td
    {
      padding: 2px;
    }

    #tab-report .organization-scroller th.sorted:after
    {
      content: "";
      width: 0;
      height: 0;
      border: 4px solid transparent;
      display: inline-block;
      position: relative;
      margin-left: 4px;
      transition: all 0.2s;
    }

    #tab-report .organization-scroller th.sorted.asc:after
    {
      top: 2px;
      border-right-color: #fff;
      border-top-color: #fff;
      transform: rotate(-45deg);
      transition: all 0.2s;
    }

    #tab-report .organization-scroller th.sorted.desc:after
    {
      top: -2px;
      border-right-color: #fff;
      border-top-color: #fff;
      transform: rotate(135deg);
      transition: all 0.2s;
    }

    #tab-report .organization-scroller tr:nth-child(even)
    {
      background-color: #d8d8d8;
    }

    #tab-report .organization-scroller td.link
    {
      text-decoration: underline;
      color: blue;
    }
    .main-tab .error-field
    {
      background-color: #ffff00;
    }



    .main-tab .drag-box
    {
      width: 250px;
      min-height: 100px;
      max-height: 500px;
      overflow-y: auto;
      border: 1px solid #ddd;
      margin-top: 5px;
      padding: 3px;
    }

    .main-tab .drag-box.disabled
    {
      background-color: #eee;
    }

    .main-tab .drag-box.can-select .selected
    {
      color: #fff;
      background-color: #00f;
    }

    .main-tab .drag-box p
    {
      padding-left: 16px;
      text-indent: -16px;
    }

    .main-tab .drag-box p.omitted
    {
      color: #aaa;
    }

    .main-tab .info
    {
      font-size: 8pt;
      margin-top: 3px;
      line-height: 120%;
    }

    .main-tab .add-button
    {
      margin-top: 15px;
    }

    .main-tab .bottom-button
    {
      margin-top: 5px;
    }

    #main-tabs .div-button
    {
      cursor: pointer;
    }

    #main-tabs .save-button
    {
      margin-right: 16px;
    }

    .org-dialog .content
    {
      padding: 20px 10px 10px 10px;
    }

    .org-dialog input[type=text]
    {
      width: 400px;
    }

    .org-dialog .button-box
    {
      margin-top: 20px;
    }

    .org-dialog .delete-button
    {
      float: right;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

<div id="outer">
<h1 id="H1" EnableViewState="false" runat="server"></h1>

<div id="UpdateControls" class="update-controls" runat="server">

<div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">

<ul class="main-tabs tabs htabs unselectable">
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizations" onclick="this.blur()" id="TabOrganizations" EnableViewState="false" runat="server">Organizations</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizationnotes" onclick="this.blur()" id="TabOrganizationNotes" EnableViewState="false" runat="server">Notes</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizationlogo" onclick="this.blur()" id="TabOrganizationLogo" EnableViewState="false" runat="server">Upload Logo</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizationad" onclick="this.blur()" id="TabOrganizationAd" EnableViewState="false" runat="server">Upload Ad</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizationtypes" onclick="this.blur()" id="TabOrganizationTypes" EnableViewState="false" runat="server">Organization Types</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-organizationsubtypes" onclick="this.blur()" id="TabOrganizationSubTypes" EnableViewState="false" runat="server">Organization SubTypes</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-emailtags" onclick="this.blur()" id="TabEmailTags" EnableViewState="false" runat="server">Email Tags</a>
  </li>
  <li class="tab htab" EnableViewState="false" runat="server">
    <a href="#tab-report" onclick="this.blur()" id="TabReport" EnableViewState="false" runat="server">Report</a>
  </li>
</ul>

<div id="tab-organizations" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Organization Maintenance</h4>
        <div class="mode-label-line">
          <span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span>
        </div>
        <hr/>
        <div class="change-button-line">
          <span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span>
        </div>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organizations</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="organizations drag-box"></div>
        <p class="info change-only">Click to Select.</p>
        <div class="div-button delete-organization-button bottom-button button-3 button-smallest change-only">Delete Selected Organization</div>
        <div class="organization-lastupdated change-only">
          <p class="fieldlabel">Date Last Updated</p>
          <input type="text" disabled="disabled"/>
        </div>
      </div>
      <div class="col col2">
        <div class="field organization-name">
          <p class="fieldlabel">Organization Name</p>
          <input type="text"/>
        </div>
        <div class="field organization-abbreviation">
          <p class="fieldlabel">Short Name</p>
          <input type="text"/>
        </div>
        <div class="field organization-subtype">
          <p class="fieldlabel">Organization Sub-Type</p>
          <select></select>
        </div>
        <div class="field organization-ideology">
          <p class="fieldlabel">Ideology</p>
          <select></select>
        </div>
        <div class="field organization-emailtags">
          <p class="fieldlabel">Email Tags</p>
          <div class="checkboxes"></div>
        </div>
        <div class="field organization-address organization-address1">
          <p class="fieldlabel">Address Line 1</p>
          <input type="text"/>
        </div>
        <div class="field organization-address organization-address2">
          <p class="fieldlabel">Address Line 2</p>
          <input type="text"/>
        </div>
        <div class="field organization-city">
          <p class="fieldlabel">City</p>
          <input type="text"/>
        </div>
        <div class="field organization-state">
          <p class="fieldlabel">State</p>
          <select id="OrganizationStates" runat="server">
            <option>&lt;Select State&gt;</option>
            <option>VA</option>
          </select>
        </div>
        <div class="field organization-zip">
          <p class="fieldlabel">Zip</p>
          <input type="text"/>
        </div>
        <div class="field organization-url">
          <p class="fieldlabel">Organization URL</p>
          <input type="text"/>
          <button class="button button-2 button-smallest">Test</button>
        </div>
        <div class="field organization-missionurls">
          <p class="fieldlabel">Mission URLs</p>
          <div class="drag-box"></div>
          <p class="info">Click to edit or test. Drag to Reorder.</p>
          <div class="div-button add-button bottom-button add-missionurl-button button-1 button-smallest">Add New Mission URL</div>
        </div>
        <div class="field organization-contacts">
          <p class="fieldlabel">Contacts</p>
          <div class="drag-box"></div>
          <p class="info">Click to edit. Drag primary contact to top.</p>
          <div class="div-button add-button bottom-button add-contact-button button-1 button-smallest">Add Contact</div>
        </div>
        <div class="field organization-longmission">
          <p class="fieldlabel">Long Mission</p>
          <textarea></textarea>
        </div>
        <div class="field organization-shortmission">
          <p class="fieldlabel">Short Mission</p>
          <textarea></textarea>
        </div>
        <div class="field organization-emailmission">
          <p class="fieldlabel">Email Mission</p>
          <textarea></textarea>
        </div>
        <div class="div-button add-button add-organization-button button-1 button-smaller add-only">Add New Organization</div>
      </div>
    </div>
  </div>
</div>

<div id="tab-organizationnotes" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Organization Notes</h4>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organizations</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="organizations drag-box"></div>
        <p class="info">Click to Select.</p>
      </div>
      <div class="col col2">
        <div class="org-header">
          <p class="fieldlabel">Organization: <span>Organization</span></p>
        </div>
        <div class="notes"></div>
        <p class="fieldlabel">New Note</p>
        <div class="new-note">
          <textarea rows="5" cols="40"></textarea>
        </div>
        <div class="add-button">
          <input type="button" class="button-1 button-smallest" value="Add new note">
        </div>
      </div>
    </div>
  </div>
</div>

<div id="tab-organizationlogo" class="main-tab content-panel tab-panel htab-panel image-tab">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Upload Organization Logo</h4>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organizations</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="organizations drag-box"></div>
        <p class="info">Click to Select.</p>
      </div>
      <div class="col col2">
        <div class="org-header">
          <p class="fieldlabel">Organization: <span>Organization</span></p>
        </div>
      </div>
    </div>
  </div>
</div>

<div id="tab-organizationad" class="main-tab content-panel tab-panel htab-panel image-tab">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Upload Organization Ad</h4>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organizations</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="organizations drag-box"></div>
        <p class="info">Click to Select.</p>
      </div>
      <div class="col col2 input-area">
        <div class="org-header">
          <p class="fieldlabel">Organization: <span>Organization</span></p>
        </div>
        <p class="fieldlabel">Ad Image</p>
        <div>
          <input type="text" class="image-file-name shadow-2" disabled="disabled"/>
          <label class="button-1 button-smaller" for="AdImageFile">Browse...</label>
          <input type="file" id="AdImageFile" style="display:none"/>
          <p class="fieldnote">The image ad should be 800 to 980 pixels wide (or wider) and banner-shaped (much wider than tall). 980 wide is ideal.</p>
          <input type="hidden" class="image-file-changed"/>
          <input type="hidden" class="image-file-updated"/>
        </div>
        <p class="fieldlabel">Ad URL</p>
        <input type="text" class="ad-url shadow-2"/>
        <p class="default-url">Default: <span></span></p>
<%--        <input type="button" class="button-2 button-smallest action-button view-sample" value="View Sample Ad">--%>
        <input type="button" class="button-1 button-smallest action-button update-ad-button" value="Update Ad">
        <input type="button" class="button-3 button-smallest action-button delete-ad-button" value="Delete Ad">
      </div>
    </div>
              
    <div class="sample-ad">
      <hr/>
      <div class="sample-ad-content"></div>
      <hr/>
      <a href="/" class="home-page-link" id="HomePageLink" runat="server" target="ext">View on Home page</a>
    </div>
  </div>
</div>

<div id="tab-organizationtypes" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Organization Types</h4>
        <div class="mode-label-line">
          <span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span>
        </div>
        <hr/>
        <div class="change-button-line">
          <span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span>
        </div>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organization Types</p>
        <p class="info">(number of organizations in parens)</p>
        <div class="organization-types drag-box"></div>
        <p class="info change-only">Click to Select. Drag to Reorder.</p>
        <div class="div-button delete-organization-type-button bottom-button button-3 button-smallest change-only">Delete Selected Organization Type</div>
      </div>
      <div class="col col2">
        <div class="field organization-type">
          <p class="fieldlabel">Organization Type</p>
          <input type="text"/>
        </div>
        <div></div>
        <div class="div-button add-button add-organization-type-button button-1 button-smaller add-only">Add New Organization Type</div>
      </div>
    </div>
  </div>
</div>

<div id="tab-organizationsubtypes" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Organization SubTypes</h4>
        <div class="mode-label-line">
          <span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span>
        </div>
        <hr/>
        <div class="change-button-line">
          <span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span>
        </div>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Organization SubTypes</p>
        <p class="info">(number of organizations in parens)</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="organization-subtypes drag-box"></div>
        <p class="info change-only">Click to Select. Drag to Reorder.</p>
        <div class="div-button delete-organization-subtype-button bottom-button button-3 button-smallest change-only">Delete Selected Organization SubType</div>
      </div>
      <div class="col col2">
        <div class="field organization-subtype">
          <p class="fieldlabel">Organization SubType</p>
          <input type="text"/>
        </div>
        <div></div>
        <div class="div-button add-button add-organization-subtype-button button-1 button-smaller add-only">Add New Organization SubType</div>
      </div>
    </div>
  </div>
</div>

<div id="tab-emailtags" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Email Tags</h4>
        <div class="mode-label-line">
          <span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span>
        </div>
        <hr/>
        <div class="change-button-line">
          <span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span>
        </div>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="col col1">
        <p class="fieldlabel">Email Tags</p>
        <p class="info">(number of organizations in parens)</p>
        <select class="organization-types-filter">
          <option value="">&lt;Select Organization Type&gt;</option>
        </select>
        <div class="email-tags drag-box"></div>
        <p class="info change-only">Click to Select. Drag to Reorder.</p>
        <div class="div-button delete-email-tag-button bottom-button button-3 button-smallest change-only">Delete Selected Email Tag</div>
      </div>
      <div class="col col2">
        <div class="field email-tag">
          <p class="fieldlabel">Email Tag</p>
          <input type="text"/>
        </div>
        <div></div>
        <div class="div-button add-button add-email-tag-button button-1 button-smaller add-only">Add New Email Tag</div>
      </div>
    </div>
  </div>
</div>

<div id="tab-report" class="main-tab content-panel tab-panel htab-panel">
  <div class="inner-panel rounded-border">
    <div class="tab-panel-heading horz-center">
      <div class="center-inner">
        <h4 class="center-element">Report</h4>
      </div>
    </div>
    <hr/>
    <div class="data-area clearfix">
      <div class="field report-type">
        <p class="fieldlabel">Organization Type</p>
        <select class="organization-types-filter">
          <option>&lt;Select Type&gt;</option>
        </select>
      </div>
 
      <div class="field report-subtype">
        <p class="fieldlabel">SubType</p>
        <select>
          <option>All SubTypes</option>
        </select>
      </div>
 
      <div class="field report-ideology">
        <p class="fieldlabel">Ideology</p>
        <select>
          <option>All Ideologies</option>
        </select>
      </div>
 
      <div class="field report-state">
        <p class="fieldlabel">State</p>
        <select id="ReportStates" runat="server">
          <option>All States</option>
        </select>
      </div>

      <div class="field report-emailtags">
        <p class="fieldlabel">Email Tags</p>
        <div class="checkboxes"></div>
      </div>

      <div class="update-button">
        <input type="button" value="Get Report" class="button-1 get-report-button">                  
      </div>

      <div class="clear-both"></div>
      
      <p class="found-count hidden">Found <span></span> organizations</p>
             
      <div class="organization-scroller hidden">
                
      </div>
    </div>
  </div>
</div>

</div>
</div>

</div>

<div id="mission-url-dialog" class="hidden org-dialog">
  <div class="content">
    <div class="field organization-missionurl">
      <input type="text"/> <input type="button" value="Test" runat="server" class="button-2 button-smallest test-button"/>
    </div>
    <input type="hidden" class="mode"/>
    <input type="hidden" class="id"/>
    <div class="button-box">
      <input type="button" value="Ok" runat="server" class="button-1 button-smaller ok-button"/>
      <input type="button" value="Delete" runat="server" class="button-3 button-smaller delete-button"/>
    </div>
  </div>
</div>

<div id="contact-dialog" class="hidden org-dialog">
  <div class="content">
    <div class="field organization-contact">
      <p class="fieldlabel">Contact Name</p>
      <input type="text"/>
    </div>
    <div class="field organization-title">
      <p class="fieldlabel">Title</p>
      <input type="text"/>
    </div>
    <div class="field organization-email">
      <p class="fieldlabel">Email</p>
      <input type="text"/>
    </div>
    <div class="field organization-phone">
      <p class="fieldlabel">Phone</p>
      <input type="text"/>
    </div>
    <input type="hidden" class="mode"/>
    <input type="hidden" class="id"/>
    <div class="button-box">
      <input type="button" value="Ok" runat="server" class="button-1 button-smaller ok-button"/>
      <input type="button" value="Delete" runat="server" class="button-3 button-smaller delete-button"/>
    </div>
  </div>
</div>

</asp:Content>