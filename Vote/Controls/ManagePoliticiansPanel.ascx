﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagePoliticiansPanel.ascx.cs" 
Inherits="Vote.Controls.ManagePoliticiansPanel" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>
<%@ Register TagPrefix="user" TagName="FindPolitician" Src="/Controls/FindPolitician.ascx" %>

<input id="ManagePoliticiansPanelMode" class="manage-politicians-panel-mode" type="hidden" runat="server" value="ManageCandidates" />

<div id="CandidateControl" runat="server" class="input-element candidatelist">
  <div class="databox">
    <h6 id="IncumbentCountMessage" runat="server" class="incumbent-count-message"></h6>
    <h6 id="ManagePoliticiansMessage" class="add-candidates-message" runat="server"></h6>
    <input id="IncumbentCount" runat="server" class="incumbent-count" type="hidden" />
    <input id="ControlAddCandidatesCandidateListValue" type="hidden" runat="server"/>
    <ul id="ControlAddCandidatesCandidateList" runat="server" class="candidates-list value-in-hidden-field">
    </ul>
    <div class="tab-ast" id="AsteriskAddCandidatesCandidateList" EnableViewState="false" runat="server"></div>
  </div>
</div>

<div class="add-politician-panel">
  <div class="inner">

    <div id="add-candidate-tabs" class="jqueryui-tabs shadow">
      
      <ul class="htabs unselectable">
        <li class="tab htab" EnableViewState="false"><a href="#tab-add-candidate-search" onclick="this.blur()" id="TabAddCandidateSearch" EnableViewState="false">Search for Existing Politician</a></li>
        <li class="tab htab" EnableViewState="false"><a href="#tab-add-candidate-add" onclick="this.blur()" id="TabAddCandidateAdd" EnableViewState="false">Add New Politician</a></li>
      </ul>

      <div id="tab-add-candidate-search">
        <div class="search-politician-name">
          <user:FindPolitician ID="FindPolitician" runat="server" CssClass="find-politician" />
          <div class="footer">
            <div>
            <input id="AddExistingCandidateButton" runat="server" type="button" 
            value="Add Politician as Candidate" 
            class="add-search-candidate-button enable-add-candidate-search button-1 button-smallest disabled" />
            <input id="ConsolidatePoliticiansButton" runat="server" type="button" 
            value="Consolidate Politicians" 
            class="consolidate-politicians-button button-1 button-smallest disabled" />
            </div>
            <div class="msg enable-add-candidate-search disabled">
              Or click green <em>+</em> on the left to add politician as a running mate.
            </div>
          </div>
        </div>
      </div>
                       
      <div id="tab-add-candidate-add">
                          
        <div class="intro">Please help us avoid duplicate politician 
        entries. Before you add a new politician, use the 
        <em>Search for Existing Politician</em> tab to be sure 
        there is not already an entry for this politician.</div>

        <asp:UpdatePanel ID="UpdatePanelAddNewCandidate" 
          UpdateMode="Conditional" runat="server">
          <ContentTemplate>
            <div id="ContainerAddNewCandidate" runat="server" class="update-all">
            <input id="AddCandidateValidateDuplicates" class="add-candidate-validate-duplicates" runat="server" type="hidden" />
            <input id="AddCandidateMainIdIfRunningMate" class="add-candidate-main-id-if-running-mate" runat="server" type="hidden" />
            <input id="AddCandidateFormattedName" class="add-candidate-formatted-name" runat="server" type="hidden" />
            <input id="AddCandidateStateName" class="add-candidate-state-name" runat="server" type="hidden" />
            <input id="AddCandidateNewId" class="add-candidate-new-id" runat="server" type="hidden" />
            <div id="AddCandidateDuplicatesHtml" class="add-candidate-duplicates-html hidden" runat="server" />

            <div class="input-element fname">
              <p class="fieldlabel">First Name <span class="reqd">◄</span></p>
              <div class="databox textbox">
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                  ID="ControlAddNewCandidateFName"  autocomplete="off"
                  CssClass="data required trim-special mc mc-group-addnewcandidate-fname mc-data shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              </div>
            </div>

            <div class="input-element mname">
              <p class="fieldlabel">Middle or Initial</p>
              <div class="databox textbox">
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false"  autocomplete="off"
                  ID="ControlAddNewCandidateMName" CssClass="data shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              </div>
            </div>

            <div class="input-element nickname">
              <p class="fieldlabel">Nickname</p>
              <div class="databox textbox">
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false"   autocomplete="off"
                  ID="ControlAddNewCandidateNickname" CssClass="trim-special data shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              </div>
            </div>
                          
            <div class="clear-both spacer"></div>

            <div class="input-element lname">
              <p class="fieldlabel">Last Name <span class="reqd">◄</span></p>
              <div class="databox textbox">
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                  ID="ControlAddNewCandidateLName"   autocomplete="off"
                  CssClass="data required trim-special mc mc-group-addnewcandidate-lname mc-data shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              </div>
            </div>

            <div class="input-element suffix">
              <p class="fieldlabel">Suffix</p>
              <div class="databox textbox">
                <user:TextBoxWithNormalizedLineBreaks spellcheck="false"   autocomplete="off"
                  ID="ControlAddNewCandidateSuffix" CssClass="data shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              </div>
            </div>
                          
            <div class="input-element statecode">
              <p class="fieldlabel">State <span class="reqd">◄</span></p>
              <div class="databox dropdown">
                <div class="shadow-2">
                  <asp:DropDownList id="ControlAddNewCandidateStateCode" 
                  CssClass="data required mc mc-group-addnewcandidate-statecode mc-data" runat="server"></asp:DropDownList>
                </div>
              </div>              
            </div>
                                                    
            <div class="clear-both spacer"></div>

            <div class="add-candidates-open-edit-panel">
              <asp:CheckBox ID="AddNewCandidateOpenEditPanelCheckBox" Checked="true"
              runat="server" />Open edit panel after adding new politician
            </div>

            <user:FeedbackContainer ID="FeedbackAddNewCandidate" 
            EnableViewState="false" CssClass="mc mc-group-addnewcandidate mc-feedback" runat="server" />
                               
            <asp:Button ID="ButtonAddNewCandidate" runat="server" 
            CssClass="hidden button-add-new-candidate" Text="Hidden Submit Button"
            onClick="ButtonAddNewCandidate_OnClick" />
            </div>

          </ContentTemplate>
        </asp:UpdatePanel>

        <div class="footer">
          <div>
          <input id="AddNewCandidateButton" runat="server" type="button" 
          value="Add Politician as Candidate" 
          class="add-new-candidate-button enable-add-candidate-new button-1 button-smallest disabled" />
          </div>
          <div class="msg enable-add-candidate-new disabled">
            Or click green <em>+</em> on the left to add politician as a running mate.
          </div>
        </div>

      </div>

    </div>

  </div>
</div>

<div id="edit-politician-dialog" class="hidden">
  <div class="content-panel tab-panel htab-panel">
    <asp:UpdatePanel ID="UpdatePanelEditPolitician" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <div id="ContainerEditPolitician" runat="server">
          <input id="EditPoliticianReloading" class="reloading" type="hidden" runat="server" />
          <input id="CandidateToEdit" runat="server" class="candidate-to-edit" type="hidden" />
          <input id="MainCandidateIfRunningMate" runat="server" class="main-candidate-if-running-mate" type="hidden" />
          <div id="CandidateHtml" clientidmode="Static" class="candidate-html" style="display:none" runat="server"></div>
          <div class="inner-panel rounded-border">
          
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                  <div id="UndoEditPolitician" runat="server"></div>
                </div>
                <h4 id = "EditPoliticianTitle" runat="server" class="center-element">Edit Candidate Information</h4>
              </div>
            </div>
            <hr/>
          
            <div class="content-area">
                
              <h5>Name as Shown on Ballots: <em id="NameOnBallots" EnableViewState="false" runat="server"></em></h5>

              <div class="input-element fname">
                <p class="fieldlabel">First Name <span class="reqd">◄</span></p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianFName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianFName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element mname">
                <p class="fieldlabel">Middle or Initial</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianMName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianMName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element nickname">
                <p class="fieldlabel">Nickname</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianNickname" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianNickname" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element lname">
                <p class="fieldlabel">Last Name <span class="reqd">◄</span></p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianLName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianLName" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element suffix">
                <p class="fieldlabel">Suffix</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianSuffix" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianSuffix" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element addon" id="AddOnEditElement" runat="server">
                <p class="fieldlabel">ANC AddOn</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianAddOn" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianAddOn" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both spacer"></div>
 
              <hr />
              <h5 class="dob-item">Date of Birth:</h5>
              <div class="input-element dateofbirth dob-item">
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianDateOfBirth" CssClass="shadow-2 date-picker-dob" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianDateOfBirth" EnableViewState="false" runat="server"></div>
                </div>
              </div>
              
 
              <hr />
              <h5>State-Provided Contact Information</h5>
              <h5 class="sub">Use <a href="/politician/updateIntro.aspx" target="updateintro" id="UpdateIntroLink" runat="server">Update Intro</a> to change candidate-provided information
              (go directly to <a href="/politician/updateIntro.aspx" target="updateintro" id="UpdateIntroLinkProfessional" runat="server">Update Intro | Professional Experience</a>)</h5>

              <div class="input-element publicaddress">
                <p class="fieldlabel">Street Address</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianStateAddress" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianStateAddress" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element publiccitystatezip">
                <p class="fieldlabel">City, State Zip</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianStateCityStateZip" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianStateCityStateZip" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both spacer"></div>

              <div class="input-element publicphone">
                <p class="fieldlabel">Phone</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianStatePhone" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianStatePhone" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element publicemail">
                <p class="fieldlabel">Email</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianStateEmailAddr" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianStateEmailAddr" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="input-element publicwebaddress">
                <p class="fieldlabel">Web Site</p>
                <div class="databox textbox">
                  <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditPoliticianStateWebAddr" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                  <div class="tab-ast" id="AsteriskEditPoliticianStateWebAddr" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both spacer"></div>
 
              <hr />
              <h5>Political Party:  <em id="PartyName" EnableViewState="false" runat="server"></em></h5>

              <div class="input-element partykey">
                <p class="fieldlabel">Select a Party</p>
                <div class="databox dropdown">
                  <div class="shadow-2">
                    <user:DropDownListWithOptionGroup id="ControlEditPoliticianPartyKey" runat="server"></user:DropDownListWithOptionGroup>
                  </div>
                  <div class="tab-ast" id="AsteriskEditPoliticianPartyKey" EnableViewState="false" runat="server"></div>
                </div>              
              </div>

              <div class="input-element incumbent">
                <div class="databox kalypto-checkbox" id="InputEditPoliticianIsIncumbent" runat="server">
                  <div class="kalypto-container">
                    <input id="ControlEditPoliticianIsIncumbent" class="kalypto" 
                    runat="server" type="checkbox" />
                  </div>
                  <div class="kalypto-checkbox-label">Candidate is incumbent for this election</div>
                  <div class="tab-ast" id="AsteriskEditPoliticianIsIncumbent" EnableViewState="false" runat="server"></div>
                </div>
              </div>

              <div class="clear-both spacer"></div>
              
              </div>
          
            <hr />
            <div class="content-footer">
              <div class="feedback-floater-for-ie7">
                <user:FeedbackContainer ID="FeedbackEditPolitician" 
                EnableViewState="false" runat="server" />
                </div>
              <div class="footer-item footer-ajax-loader">
                <asp:Image ID="AjaxLoaderEditPolitician" 
                EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                  CssClass="ajax-loader" runat="server" />
              </div>
              <div class="update-button">
                <asp:Button ID="ButtonEditPolitician" EnableViewState="false" 
                  runat="server" 
                  Text="Update" CssClass="update-button button-1 tiptip" 
                  Title="Update the politician info"
                  OnClick="ButtonEditPolitician_OnClick" /> 
              </div>   
              <div class="clear-both"></div>
            </div>   

          </div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</div>

<div id="duplicate-last-name-dialog" class="hidden">
  <div class="info">There are other politicians in <span id="duplicate-last-name-dialog-state-name">the same state</span> with the same last name 
  as <strong id="duplicate-last-name-dialog-formatted-name">the politician you are adding</strong>. Please check this list to be sure this is 
  not a duplicate.</div>
  <div class="info">If you are sure this is not a duplicate, click <em>Continue Adding 
  New Politician</em>.</div>
  <div class="info" id="UsePoliticianFromListMessage" runat="server">If you find the politician in the list, select it then click <em>Use Politician from List</em>. </div>
  <div class="search-results-container"></div>
  <div class="footer">
    <div>
      <input id="ContinueAddingNewPoliticianButton" type="button" runat="server"
      value="Continue Adding New Politician" 
      class="continue-adding-new-politician-button button-1 button-smallest" />
    </div>
    <div>
      <input id="UsePoliticianFromListButton" type="button" runat="server"
      value="Use Politician from List" 
      class="use-politician-from-list-button button-1 button-smallest disabled" />
    </div>
    <div class="clear-both"></div>
  </div>
</div>

<div id="consolidate-politicians-dialog" class="hidden tabs-container">
  <div class="content-panel tab-panel htab-panel">
    <asp:UpdatePanel ID="UpdateConsolidate" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <div id="ContainerConsolidate" runat="server" class="mc-alwaysupdate">
          <input id="ConsolidateReloading" class="reloading" type="hidden" runat="server" />
          <input id="ConsolidateReloaded" class="reloaded" type="hidden" runat="server" />
          <input id="ConsolidateKey1" runat="server" class="consolidate-key-1" type="hidden" />
          <input id="ConsolidateKey2" runat="server" class="consolidate-key-2" type="hidden" />
          <input id="ConsolidateSelectedIndex" runat="server" class="consolidate-selected-index" type="hidden" />
          <input id="ConsolidateSelectedData" runat="server" class="consolidate-selected-data" type="hidden" />
          <input id="ConsolidateSelectedResponses" runat="server" class="consolidate-selected-responses" type="hidden" />
          <div class="inner-panel rounded-border">
            
            <div class="instructions">Select the politician to be retained. The other politician will be merged into the one you select, then deleted.</div>
            <div class="sub-instructions">Use the tabs to review or change the default selections.</div>
            <div class="pick-politician clearfix">
              <div id="ConsolidateItem1" runat="server" class="consolidate-item consolidate-item-1 search-results-container"></div>
              <div id="ConsolidateItem2" runat="server" class="consolidate-item consolidate-item-2 search-results-container"></div>
            </div>
            
            <div id="consolidate-tabs" class="jqueryui-tabs shadow">
       
              <ul class="htabs unselectable">
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-personal" onclick="this.blur()" EnableViewState="false">&nbsp;<br/>Personal</a></li>
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-contact" onclick="this.blur()" EnableViewState="false">&nbsp;<br/>Contact</a></li>
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-webemail" onclick="this.blur()" EnableViewState="false">Web/<br/>Email</a></li>
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-social" onclick="this.blur()" EnableViewState="false">Social<br/>Media</a></li>
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-pictures" onclick="this.blur()" EnableViewState="false">&nbsp;<br/>Pictures</a></li>
                <%--<li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-bio" onclick="this.blur()" EnableViewState="false">Biographical<br/>Information</a></li>--%>
                <li class="tab htab" EnableViewState="false"><a href="#tab-consolidate-answers" onclick="this.blur()" EnableViewState="false">Issue<br/>Responses</a></li>
              </ul>

              <div id="tab-consolidate-personal">
                <div id="ConsolidatePersonalTabContent" class="consolidate-personal-tab-content consolidate-tab-content" runat="server"></div>
              </div>

              <div id="tab-consolidate-contact">
                <div id="ConsolidateContactTabContent" class="consolidate-contact-tab-content consolidate-tab-content" runat="server"></div>
              </div>

              <div id="tab-consolidate-webemail">
                <div id="ConsolidateWebEmailTabContent" class="consolidate-webemail-tab-content consolidate-tab-content" runat="server"></div>
              </div>

              <div id="tab-consolidate-social">
                <div id="ConsolidateSocialTabContent" class="consolidate-social-tab-content consolidate-tab-content" runat="server"></div>
              </div>

              <div id="tab-consolidate-pictures">
                <div id="ConsolidatePicturesTabContent" class="consolidate-pictures-tab-content consolidate-tab-content" runat="server"></div>
              </div>
              
              <%--
              <div id="tab-consolidate-bio">
                <div id="ConsolidateBioTabContent" class="consolidate-bio-tab-content consolidate-tab-content" runat="server"></div>
              </div>
              --%>

              <div id="tab-consolidate-answers">
                <div id="ConsolidateAnswersTabContent" class="consolidate-answers-tab-content consolidate-tab-content" runat="server"></div>
              </div>
            
            </div>

            <hr />
            <div class="content-footer">
              <div class="feedback-floater-for-ie7">
                <user:FeedbackContainer ID="FeedbackConsolidate" 
                EnableViewState="false" runat="server" />
                </div>
              <div class="footer-item footer-ajax-loader">
                <asp:Image ID="AjaxLoaderConsolidate" 
                EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                  CssClass="ajax-loader" runat="server" />
              </div>
              <div class="update-button">
                <asp:Button ID="ButtonConsolidate" EnableViewState="false" 
                  runat="server" 
                  Text="Update" CssClass="update-button button-1 tiptip" 
                  Title="Do the Consolidation"
                  OnClick="ButtonConsolidate_OnClick" /> 
              </div>   
              <div class="clear-both"></div>
            </div>   
            
          </div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
  
</div>