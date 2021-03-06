<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="UpdateElections.aspx.cs" 
Inherits="Vote.Admin.UpdateElectionsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/NavigateJurisdiction.ascx" TagName="NavigateJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/ElectionControl.ascx" TagName="ElectionControl" TagPrefix="user" %>
<%@ Register Src="/Controls/OfficeControl.ascx" TagName="OfficeControl" TagPrefix="user" %>
<%@ Register Src="/Controls/ManagePoliticiansPanel.ascx" TagName="ManagePoliticiansPanel" TagPrefix="user" %>
<%@ Register Src="/Controls/SelectJurisdictionButton.ascx" TagName="SelectJurisdictionButton" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <input id="ClientStateCode" class="client-state-code" type="hidden" runat="server" />
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
      <h3 id="MultiCountyMessage" class="multi-county-message hidden" runat="server"></h3>
      <h4 id="CredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
    </div>
    <user:SelectJurisdictionButton ID="SelectJurisdictionButton" runat="server"  AddClasses="True"
     Tooltip="Update elections for a different state, county or local jurisdiction -- subject to your sign-in credentials."/>

    <div class="clear-both"></div>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

    <div id="UpdateControls" class="update-controls" runat="server">
      <div class="election-control-outer">
        <div class="mc mc-container mc-group-electioncontrol election-control-container rounded-border shadow disabled"
         id="SelectElectionControl" runat="server">
         <a class="select-election-toggler tiptip" title="Show or hide the menu of elections"><div class="toggler"></div>
         <div class="election-control-heading rounded">Select Election</div></a>
          <asp:UpdatePanel ID="ElectionControlUpdatePanel" 
              UpdateMode="Conditional" runat="server">
              <ContentTemplate>
                <div id="ContainerElectionControl" runat="server">
                <input id="SelectedElectionKey" runat="server" class="selected-election-key" type="hidden" />
                <div class="election-control-slider">
                  <user:ElectionControl ID="ElectionControl" runat="server" />
                </div>
                </div>
              </ContentTemplate>
            </asp:UpdatePanel>
        </div>
      </div>
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server" id="TabAddElectionItem"><a href="#tab-addelection" onclick="this.blur()" id="TabAddElection" EnableViewState="false" runat="server">Add<br />Election</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabStateDefaultsItem"><a href="#tab-statedefaults" onclick="this.blur()" id="TabStateDefaulys" EnableViewState="false" runat="server">State<br />Defaults</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabChangeInfoItem"><a href="#tab-changeinfo" onclick="this.blur()" id="TabChangeInfo" EnableViewState="false" runat="server">Election<br />Info</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabChangeDeadlinesItem"><a href="#tab-changedeadlines" onclick="this.blur()" id="TabChangeDeadlines" EnableViewState="false" runat="server">Election Order<br />and Deadlines</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabChangeOfficesItem"><a href="#tab-addoffices" onclick="this.blur()" id="TabAddOffices" EnableViewState="false" runat="server">Add/Remove<br />Offices</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabAddCandidatesItem"><a href="#tab-addcandidates" onclick="this.blur()" id="TabAddCandidates" EnableViewState="false" runat="server">Setup Candidates<br />for Offices</a></li>
          <li class="tab htab hidden adjust-incumbents-tab" EnableViewState="false" runat="server" id="TabAdjustIncumbentsItem"><a href="#tab-adjustincumbents" onclick="this.blur()" id="TabAdjustIncumbents" EnableViewState="false" runat="server">Adjust<br />Incumbents</a></li>
          <li class="tab htab hidden primary-winners-tab" EnableViewState="false" runat="server" id="TabIdentifyPrimaryWinnersItem"><a href="#tab-identifywinnersbeta" onclick="this.blur()" id="IdentifyPrimaryWinners" EnableViewState="false" runat="server">Primary<br />Winners</a></li>
          <li class="tab htab hidden general-winners-tab" EnableViewState="false" runat="server" id="TabIdentifyGeneralWinnersItem"><a href="#tab-identifywinners" onclick="this.blur()" id="IdentifyGeneralWinners" EnableViewState="false" runat="server">General<br />Winners</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabAddBallotMeasuresItem"><a href="#tab-addballotmeasures" onclick="this.blur()" id="TabAddBallotMeasures" EnableViewState="false" runat="server">Setup Ballot<br />Measures</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabViewReportItem"><a href="#tab-viewreport" onclick="this.blur()" id="TabViewReport" EnableViewState="false" runat="server">View<br />Report</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabMasterItem"><a href="#tab-masteronly" onclick="this.blur()" id="TabMaster" EnableViewState="false" runat="server">Master<br />Functions</a></li>
        </ul>
        
        <% if (ShowAddElections) { %>

        <div id="tab-addelection" class="main-tab content-panel tab-panel htab-panel">

          <asp:UpdatePanel ID="UpdatePanelAddElection" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerAddElection" class="update-all" runat="server">
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Clear this entry">
                      <div id="UndoAddElection" runat="server"></div>
                    </div>
                    <h4 class="center-element">Add an Election</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="inverted">Use this panel to add the basic information for an election.</h6>
                <h6 class="inverted">After adding the election, use the other tabs to enter additional election information.</h6>
                <hr/>
                <div class="data-area">
                    
                <div class="instructions">
                  <ul>
                  <% if (IsMasterUser)
                     { %>
                  <li><span>Note: General elections (November elections in even-numbered years) are created for all states in one operation under the <em>Master Functions | Create General Election</em> tab.</span></li>
                  <% }
                     else
                     { %>
                  <li><span>Note: General elections (November elections in even-numbered years) are created only by Master users.</span></li>
                  <% } %>
                  </ul>
                </div>
                  
                <div class="col col1">
                <div class="input-element electiondate">
                  <p class="fieldlabel">Election Date <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlAddElectionElectionDate" placeholder="mm/dd/yyyy" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                  </div>                
                </div>

                <div class="clear-both spacer"></div>

                <div class="input-element pastelection">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlAddElectionPastElection" class="kalypto mc-nomonitor" 
                      runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Check to allow an election date in the past</div>
                  </div>
                </div>

                <div class="clear-both"></div>
                </div>
                
                <div class="col col2">

                <div class="input-element electiontype" id="AddElectionElectionType" runat="server">
                  <p class="fieldlabel">Election Type <span class="reqd">◄</span></p>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlAddElectionElectionType" 
                        CssClass="add-election-election-type-dropdown mc-nomonitor" runat="server"/>
                    </div>
                  </div>
                </div>

                <div class="clear-both"></div>
                </div>
 
                <div class="clear-both"></div>

                <div class="primaries rounded-border">
                <h6 class="inverted">For Primaries Only:</h6>
                
                <div class="col">
                <div class="input-element party">
                  <p class="fieldlabel">Party <span class="reqd">◄</span></p>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlAddElectionNationalParty" 
                        CssClass="add-election-national-party-dropdown mc-nomonitor" 
                        runat="server"/>
                    </div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                <div class="help">Use these checkboxes to set up a presidential primary</div>
                <div class="input-element includepresident not-for-runoff">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlAddElectionIncludePresident" 
                      class="add-election-include-president kalypto mc-nomonitor" 
                      runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Include the presidential contest</div>
                  </div>
                </div>

                <div class="clear-both"></div>

                <div class="input-element includepresidentcandidates not-for-runoff">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlAddElectionIncludePresidentCandidates" 
                      class="kalypto mc-nomonitor" runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Include presidential candidates from template</div>
                    <div class="help"><a href="/admin/updateelections.aspx?state=PP">Set up presidential primary candidate template</a></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                </div>
                
                <div class="col not-for-runoff">
                <div class="input-element copyoffices" id="AddElectionCopyOffices" runat="server">
                  <input id="AddElectionCopyOfficesHidden" class="add-election-copy-offices-hidden" runat="server" type="hidden" />
                  <p class="fieldlabel">Copy offices and dates from this election</p>
                  <div class="help">For copying offices from other primaries in the same state on the same date</div>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlAddElectionCopyOffices" 
                        CssClass="add-election-copy-offices-dropdown mc-nomonitor"
                        runat="server"/>
                    </div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>

                <div class="input-element copyofficescandidates">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlAddElectionCopyCandidates" 
                      class="kalypto mc-nomonitor" runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Copy candidates along with offices</div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                </div>

                </div>
                

                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddElection" EnableViewState="false" 
                      runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddElection" EnableViewState="false" 
                      ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddElection" EnableViewState="true" runat="server" 
                      Text="Add Election" CssClass="update-button button-1 tiptip" 
                      Title="Add the new election"
                      OnClick="ButtonAddElection_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   
              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>

        </div>
        
        <% } %>
        
        <% if (ShowStateDefaults) { %>

        <div id="tab-statedefaults" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelStateDefaults" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerStateDefaults" runat="server">
              <input id="StateDefaultsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoStateDefaults" runat="server"></div>
                    </div>
                    <h4 class="center-element">Enter or Change Election Information</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingStateDefaults" runat="server">Please select an election from the list on the left</h6>
                <hr/>
                <div class="data-area">
                
                <div class="input-element electionadditionalinfo">
                  <p class="fieldlabel">Additional Election Information on Ballots and Reports</p>
                  <p class="fieldlabel sub">This information will be shown in the <em>About This Enhanced Ballot Choices</em> accordion on ballots and on election reports. 
                  It usually contains a disclaimer of the election 
                  contests not covered. Click one of the <em>Disclaimer</em> buttons below to automatically 
                  load the appropriate text. You can then modify the loaded text, or you can simply 
                  enter custom text.</p>
                  <div class="databox textarea is-resizable">
                    <user:TextBoxWithNormalizedLineBreaks ID="ControlStateDefaultsElectionAdditionalInfo" 
                    EnableViewState="false" TextMode="MultiLine" 
                    CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                    <div class="tab-ast" id="AsteriskStateDefaultsElectionAdditionalInfo" EnableViewState="false" runat="server"></div>
                    <div id="DefaultStateDefaultsElectionAdditionalInfo" runat="server" class="data-default tiptip"></div>
                  </div>
                </div>
                <div class="clear-both" ></div>
                
                <div class="disclaimer-buttons">
                  <input type="button" value="County and Local Offices Not Included Disclaimer" 
                    class="disclaimer-button non-judicial button-2 button-smallest"/>
                  <input type="button" value="County, Local and JUDICIAL Offices Not Included Disclaimer" 
                    class="disclaimer-button judicial button-2 button-smallest"/>
                  <input type="button" value="Clear" 
                    class="disclaimer-clear-button judicial button-3 button-smallest"/>
                </div>
                <div class="clear-both spacer"></div>
               
                <div class="input-element ballotinstructions">
                  <p class="fieldlabel">Special Ballot Instructions</p>
                  <p class="fieldlabel sub">This information will appear only on ballots, just below 
                  the <em>Additional Election Information</em>. It usually contains specifics about the 
                  ballot, like requirements to vote in a party's primary or presidential candidates who 
                  dropped out of the race but are still shown on the ballot.</p>
                  <div class="databox textarea is-resizable">
                    <user:TextBoxWithNormalizedLineBreaks ID="ControlStateDefaultsBallotInstructions" 
                    EnableViewState="false" TextMode="MultiLine" 
                    CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                    <div class="tab-ast" id="AsteriskStateDefaultsBallotInstructions" EnableViewState="false" runat="server"></div>
                    <div id="DefaultStateDefaultsBallotInstructions" runat="server" class="data-default tiptip"></div>
                  </div>
                </div>
                <div class="clear-both spacer"></div>
                  
                <div class="input-element registrationdeadline">
                  <p class="fieldlabel"><em>Registration</em><br/>Deadline</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsRegistrationDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsRegistrationDeadline" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>

                <div class="input-element earlyvotingbegin">
                  <p class="fieldlabel"><em>Early Voting</em><br/>Begin Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsEarlyVotingBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsEarlyVotingBegin" EnableViewState="false" runat="server"></div>
                   </div>                
                </div>

                <div class="input-element earlyvotingend">
                  <p class="fieldlabel">&nbsp;<br/>End Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsEarlyVotingEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsEarlyVotingEnd" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>
                  
                <div class="input-element mailballotbegin">
                  <p class="fieldlabel"><em>Mail Ballot</em><br/>Beginning Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsMailBallotBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsMailBallotBegin" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>

                <div class="input-element mailballotend">
                  <p class="fieldlabel">&nbsp;<br/>Ending Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsMailBallotEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsMailBallotEnd" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>

                <div class="input-element mailballotdeadline">
                  <p class="fieldlabel">&nbsp;<br/>Must Be Received By</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsMailBallotDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsMailBallotDeadline" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>
                  
                <div class="input-element absenteeballotbegin">
                  <p class="fieldlabel"><em>Absentee Ballot</em><br/>Beginning Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsAbsenteeBallotBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsAbsenteeBallotBegin" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>

                <div class="input-element absenteeballotend">
                  <p class="fieldlabel">&nbsp;<br/>Ending Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsAbsenteeBallotEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsAbsenteeBallotEnd" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>

                <div class="input-element absenteeballotdeadline">
                  <p class="fieldlabel">&nbsp;<br/>Must Be Received By</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlStateDefaultsAbsenteeBallotDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskStateDefaultsAbsenteeBallotDeadline" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>
                
                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackStateDefaults" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderStateDefaults" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonStateDefaults" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election information"
                      OnClick="ButtonStateDefaults_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <% } %>
        
        <% if (ShowChangeInfo) { %>

        <div id="tab-changeinfo" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelChangeInfo" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerChangeInfo" runat="server">
              <input id="ChangeInfoReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoChangeInfo" runat="server"></div>
                    </div>
                    <h4 class="center-element">Enter or Change Election Information</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingChangeInfo" runat="server">Please select an election from the list on the left</h6>
                <hr/>
                <div class="data-area">

                <div class="input-element electiondesc">
                  <p class="fieldlabel">Election Title <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeInfoElectionDesc"  
                      CssClass="shadow-2 tiptip" title="Maximum length is 90 characters" runat="server"/>
                    <div class="tab-ast" id="AsteriskChangeInfoElectionDesc" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>
                <div class="clear-both"></div>
                  
                <div class="input-element changelocals" id="InputElementChangeLocals" runat="server">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlChangeInfoChangeLocals" class="kalypto mc-nomonitor" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Check to apply new description to the county and local elections also</div>
                  </div>
                </div>
                <div class="clear-both spacer"></div>
                
                <div class="input-element electionadditionalinfo">
                  <p class="fieldlabel">Additional Election Information on Ballots and Reports</p>
                  <p class="fieldlabel sub">This information will be shown in the <em>About This Enhanced Ballot Choices</em> accordion on ballots and on election reports. 
                  It usually contains a disclaimer of the election 
                  contests not covered. Click one of the <em>Disclaimer</em> buttons below to automatically 
                  load the appropriate text. You can then modify the loaded text, or you can simply 
                  enter custom text.</p>
                  <div class="databox textarea is-resizable">
                    <user:TextBoxWithNormalizedLineBreaks ID="ControlChangeInfoElectionAdditionalInfo" 
                    EnableViewState="false" TextMode="MultiLine" 
                    CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                    <div class="tab-ast" id="AsteriskChangeInfoElectionAdditionalInfo" EnableViewState="false" runat="server"></div>
                    <div id="DefaultChangeInfoElectionAdditionalInfo" runat="server" class="data-default tiptip"></div>
                  </div>
                </div>
                <div class="clear-both" ></div>
                
                <div class="disclaimer-buttons">
                  <input type="button" value="County and Local Offices Not Included Disclaimer" 
                    class="disclaimer-button non-judicial button-2 button-smallest"/>
                  <input type="button" value="County, Local and JUDICIAL Offices Not Included Disclaimer" 
                    class="disclaimer-button judicial button-2 button-smallest"/>
                  <input type="button" value="Clear" 
                    class="disclaimer-clear-button judicial button-3 button-smallest"/>
                </div>
                <div class="clear-both spacer"></div>
               
                <div class="input-element ballotinstructions">
                  <p class="fieldlabel">Special Ballot Instructions</p>
                  <p class="fieldlabel sub">This information will appear only on ballots, just below the <em>Additional Election Information</em></p>
                  <div class="databox textarea is-resizable">
                    <user:TextBoxWithNormalizedLineBreaks ID="ControlChangeInfoBallotInstructions" 
                    EnableViewState="false" TextMode="MultiLine" 
                    CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                    <div class="tab-ast" id="AsteriskChangeInfoBallotInstructions" EnableViewState="false" runat="server"></div>
                    <div id="DefaultChangeInfoBallotInstructions" runat="server" class="data-default tiptip"></div>
                  </div>
                </div>
                <div class="clear-both spacer"></div>
                  
                <div class="input-element isviewable" id="InputElementIsViewable" runat="server">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlChangeInfoIsViewable" class="kalypto" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Check to make all ballots and reports for this election viewable on the public pages</div>
                    <div class="tab-ast" id="AsteriskChangeInfoIsViewable" EnableViewState="false" runat="server"></div>
                  </div>
                </div>
                <div class="clear-both"></div>
                
                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackChangeInfo" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderChangeInfo" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonChangeInfo" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election information"
                      OnClick="ButtonChangeInfo_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <% } %>
        
        <% if (ShowChangeDeadlines) { %>

        <div id="tab-changedeadlines" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelChangeDeadlines" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerChangeDeadlines" runat="server">
              <input id="ChangeDeadlinesReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoChangeDeadlines" runat="server"></div>
                    </div>
                    <h4 class="center-element">Set Election Order - Enter or Change Deadlines</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingChangeDeadlines" runat="server">Please select an election from the list on the left</h6>
                <hr/>
                <div class="data-area">
                  
                <h6 id="SetOrderMessage" runat="server">The <em>Set Display Order</em> 
                  control is only used when
                  there is more than one election on the same date, typically only 
                  for primaries.</h6>
                   
                <div id="SetOrderControl" runat="server" class="input-element electionorder">
                  <p class="fieldlabel">Set Display Order of Elections</p>
                  <p class="fieldlabel sub">Drag to reorder elections</p>
                  <div class="databox sortablelist">
                    <input id="ControlChangeDeadlinesElectionOrderValue" type="hidden" runat="server"/>
                    <ul id="ControlChangeDeadlinesElectionOrder" runat="server"
                    class="sortablelist shadow-2 tiptip value-in-hidden-field" title="Drag elections to reorder them">
                    </ul>
                    <div class="tab-ast" id="AsteriskChangeDeadlinesElectionOrder" EnableViewState="false" runat="server"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>
                <hr/>
                  
                <div class="input-element registrationdeadline">
                  <p class="fieldlabel"><em>Registration</em><br/>Deadline</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesRegistrationDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesRegistrationDeadline" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesRegistrationDeadline" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element earlyvotingbegin">
                  <p class="fieldlabel"><em>Early Voting</em><br/>Begin Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesEarlyVotingBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesEarlyVotingBegin" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesEarlyVotingBegin" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element earlyvotingend">
                  <p class="fieldlabel">&nbsp;<br/>End Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesEarlyVotingEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesEarlyVotingEnd" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesEarlyVotingEnd" runat="server" class="data-default"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>
                <hr />
                  
                <div class="input-element mailballotbegin">
                  <p class="fieldlabel"><em>Mail Ballot</em><br/>Beginning Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesMailBallotBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesMailBallotBegin" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesMailBallotBegin" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element mailballotend">
                  <p class="fieldlabel">&nbsp;<br/>Ending Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesMailBallotEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesMailBallotEnd" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesMailBallotEnd" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element mailballotdeadline">
                  <p class="fieldlabel">&nbsp;<br/>Must Be Received By</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesMailBallotDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesMailBallotDeadline" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesMailBallotDeadline" runat="server" class="data-default"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>
                <hr />
                  
                <div class="input-element absenteeballotbegin">
                  <p class="fieldlabel"><em>Absentee Ballot</em><br/>Beginning Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesAbsenteeBallotBegin" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesAbsenteeBallotBegin" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesAbsenteeBallotBegin" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element absenteeballotend">
                  <p class="fieldlabel">&nbsp;<br/>Ending Request Date</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesAbsenteeBallotEnd" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesAbsenteeBallotEnd" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesAbsenteeBallotEnd" runat="server" class="data-default"></div>
                  </div>                
                </div>

                <div class="input-element absenteeballotdeadline">
                  <p class="fieldlabel">&nbsp;<br/>Must Be Received By</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                      ID="ControlChangeDeadlinesAbsenteeBallotDeadline" 
                      CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                      <div class="tab-ast" id="AsteriskChangeDeadlinesAbsenteeBallotDeadline" EnableViewState="false" runat="server"></div>
                      <div id="DefaultChangeDeadlinesAbsenteeBallotDeadline" runat="server" class="data-default"></div>
                  </div>                
                </div>
                <div class="clear-both spacer"></div>

                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackChangeDeadlines" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderChangeDeadlines" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonChangeDeadlines" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election deadlines"
                      OnClick="ButtonChangeDeadlines_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   
              </div> 
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <% } %>
        
        <% if (ShowChangeOffices) { %>

        <div id="tab-addoffices" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelAddOffices" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerAddOffices" runat="server">
              <input id="AddOfficesReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoAddOffices" runat="server"></div>
                    </div>
                    <h4 class="center-element">Add or Remove Offices</h4>
                  </div>
                </div>
                <hr/>

                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingAddOffices" runat="server">Please select an election from the list on the left</h6>
                <hr/>
                <div class="data-area">
                  
                  <h6 id="AddOfficesMessage" runat="server">No offices were found that can be added to this election.</h6>
                   
                  <div id="AddOfficesControl" runat="server" class="input-element addofficestree">
                    <p class="fieldlabel">Select Offices to Include in Election</p>
                    <div class="databox">
                      <input id="ControlAddOfficesOfficeListValue" type="hidden" runat="server"/>
                      <div ID="ControlAddOfficesOfficeList" runat="server" class="add-offices-control dynatree-type-1 value-in-hidden-field">
                        <asp:PlaceHolder ID="PlaceHolderAddOfficesTree" runat="server"></asp:PlaceHolder>
                      </div>
                      <div class="tab-ast" id="AsteriskAddOfficesOfficeList" EnableViewState="false" runat="server"></div>
                    </div>
                  </div>
                   
                  <div class="instructions">
                    <ul>
                      <li><span>Check or uncheck an <em>Office Title</em> to add it to or remove it from the election.</span></li>
                      <li><span>Check or uncheck an <em>Office Class</em> (in bold) to add or remove all the offices in the class.</span></li>
                      <li><span>A filled-in checkbox for an office class indicates that some (but not all) of the offices are selected.</span></li>
                      <li><span>A number in [square brackets] after an office title indicates the current number of candidates for that office.</span></li>
                      <li><span>Click <em>Update</em> at the bottom of the panel to save your changes. Adding or removing a large number of offices could take a minute or two.</span></li>
                    </ul>
                  </div>
                 
                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddOffices" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddOffices" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddOffices" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the candidates"
                      OnClick="ButtonAddOffices_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <% } %>
        
        <% if (ShowAddCandidates) { %>
 
        <div id="tab-addcandidates" class="main-tab content-panel tab-panel htab-panel">
          
          <user:OfficeControl ID="OfficeControl" runat="server" />

          <asp:UpdatePanel ID="UpdatePanelAddCandidates" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerAddCandidates" runat="server">
              <input id="AddCandidatesReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoAddCandidates" runat="server"></div>
                    </div>
                    <h4 class="center-element">Add, Change or Delete Candidates</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="head-col head-col-1">  
                <h6 class="panel-heading inverted ellipsis" id="HeadingAddCandidates" runat="server">Please select an election from the list on the left</h6>
                <a class="select-office-toggler" >
                <div></div><h6 id="HeadingAddCandidatesOffice" class="office-heading" runat="server">No office selected</h6></a>
                <div class="clear-both"></div>
                </div>
                <div class="head-col head-col-2">
                  <div class="head-col-2-content hidden">
                    <a href="/" target="ad" class="button-2 button-smallest setup-ad-button">Setup Compare Page Ad</a>
                    <div class="edit-block">
                      <a href="/" target="offices" class="button-2 button-smallest edit-office-button">Edit Office</a>
                      <p class="positions"/>
                    </div>
                  </div>
                </div>
                <hr/>
                <div class="data-area">
                  <user:ManagePoliticiansPanel ID="ManagePoliticiansPanel" runat="server" />
                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddCandidates" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddCandidates" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddCandidates" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election order"
                      OnClick="ButtonAddCandidates_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <% } %>
        
        <% if (ShowAdjustIncumbents) { %>
        
        <div id="tab-adjustincumbents" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelAdjustIncumbents" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
    
            <div id="ContainerAdjustIncumbents" runat="server">
              <input id="AdjustIncumbentsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoAdjustIncumbents" runat="server"></div>
                    </div>
                    <h4 class="center-element">Adjust Incumbents</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                  <h6 class="panel-heading inverted ellipsis" id="HeadingAdjustIncumbents" runat="server">Please select an election from the list on the left</h6>
                  <hr/>

                  <div id="AdjustIncumbentsDataArea" runat="server" class="data-area">
                  
                    <h6 id="AdjustIncumbentsMessage" runat="server">No offices that requires incumbents to be adjusted were found for this election.</h6>
                    
                    <div class="control-area">
                      
                      <div id="AdjustIncumbentsControl" runat="server" class="input-element">
                        <div class="databox">
                          <input id="ControlAdjustIncumbentsListValue" type="hidden" runat="server"/>
                          <div ID="ControlAdjustIncumbentsList" runat="server" class="adjust-incumbents-control value-in-hidden-field">
                            <asp:PlaceHolder ID="PlaceHolderAdjustIncumbentsList" runat="server"></asp:PlaceHolder>
                          </div>
                          <div class="tab-ast" id="AsteriskAdjustIncumbentsList" EnableViewState="false" runat="server"></div>
                        </div>

                      </div>

                    </div>

                    <div style="clear:both"></div>
                    
                    <div>

                    <h6 id="ReinstateIncumbentsMessage" class="reinstate-incumbents-message" runat="server">Check any previously removed incumbents to reinstate them.</h6>
                    
                    <div class="control-area">
                      
                      <div id="ReinstateIncumbentsControl" runat="server" class="input-element">
                        <div class="databox">
                          <input id="ControlAdjustIncumbentsReinstatementListValue" type="hidden" runat="server"/>
                          <div id="ControlAdjustIncumbentsReinstatementList" runat="server" class="reinstate-incumbents-control value-in-hidden-field">
                            <asp:PlaceHolder ID="PlaceHolderReinstateIncumbentsList" runat="server"></asp:PlaceHolder>
                          </div>
                          <div class="tab-ast" id="AsteriskAdjustIncumbentsReinstatementList" EnableViewState="false" runat="server"></div>
                        </div>

                      </div>

                    </div>

                    </div>

                  </div>
 
                  <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAdjustIncumbents" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAdjustIncumbents" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAdjustIncumbents" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the incumbents"
                      OnClick="ButtonAdjustIncumbents_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
         
        <% } %>
        
        <% if (ShowIdentifyPrimaryWinners) { %>
   
        <div id="tab-identifywinnersbeta" class="main-tab winners-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelIdentifyWinnersBeta" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerIdentifyWinnersBeta" runat="server" class="mc-alwaysupdate">
              <input id="IdentifyWinnersBetaReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoIdentifyWinnersBeta" runat="server"></div>
                    </div>
                    <div class="next-button" title="Go to next office with unidentified winner.">
                      <div></div>
                    </div>
                    <h4 class="center-element">Identify Winners</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                  <h6 class="panel-heading inverted ellipsis" id="HeadingIdentifyWinnersBeta" runat="server">Please select an election from the list on the left</h6>
                  <hr/>

                  <div id="IdentifyWinnersBetaDataArea" runat="server" class="data-area">
                  
                    <h6 id="IdentifyWinnersBetaMessage" runat="server">No offices were found for this election.</h6>
                    
                    <div class="control-area">
                      
                      <div id="IdentifyWinnersBetaControl" runat="server" class="input-element winnerstree">
                        <div class="databox">
                          <input id="ControlIdentifyWinnersBetaOfficeTreeValue" type="hidden" runat="server"/>
                          <div ID="ControlIdentifyWinnersBetaOfficeTree" runat="server" class="identify-winners-beta-control value-in-hidden-field">
                            <asp:PlaceHolder ID="PlaceHolderIdentifyWinnersBetaTree" runat="server"></asp:PlaceHolder>
                          </div>
                          <div class="tab-ast" id="AsteriskIdentifyWinnersBetaOfficeTree" EnableViewState="false" runat="server"></div>
                        </div>

                        <div class="clear-both spacer"></div>

                        <div class="clear-both spacer"></div>

                      </div>

                    </div>

                  </div>
 
                  <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackIdentifyWinnersBeta" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderIdentifyWinnersBeta" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonIdentifyWinnersBeta" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the winners"
                      OnClick="ButtonIdentifyWinnersBeta_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
         
        <% } %>
        
        <% if (ShowIdentifyGeneralWinners) { %>
  
        <div id="tab-identifywinners" class="main-tab winners-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelIdentifyWinners" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerIdentifyWinners" runat="server" class="mc-alwaysupdate">
              <input id="IdentifyWinnersReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoIdentifyWinners" runat="server"></div>
                    </div>
                    <div class="next-button" title="Go to next office with unidentified winner.">
                      <div></div>
                    </div>
                    <h4 class="center-element">Identify Winners</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                  <h6 class="panel-heading inverted ellipsis" id="HeadingIdentifyWinners" runat="server">Please select an election from the list on the left</h6>
                  <hr/>

                  <div id="IdentifyWinnersDataArea" runat="server" class="data-area">
                  
                    <h6 id="IdentifyWinnersMessage" runat="server">No offices were found for this election.</h6>
                    
                    <div class="control-area">
                      
                      <div id="IdentifyWinnersControl" runat="server" class="input-element winnerstree">
                        <p class="fieldlabel">Select the Winner for Each Office</p>
                        <div class="databox">
                          <input id="ControlIdentifyWinnersOfficeTreeValue" type="hidden" runat="server"/>
                          <div ID="ControlIdentifyWinnersOfficeTree" runat="server" class="identify-winners-control value-in-hidden-field">
                            <asp:PlaceHolder ID="PlaceHolderIdentifyWinnersTree" runat="server"></asp:PlaceHolder>
                          </div>
                          <div class="tab-ast" id="AsteriskIdentifyWinnersOfficeTree" EnableViewState="false" runat="server"></div>
                        </div>

                        <div class="clear-both spacer"></div>
                  
                        <div class="input-element iswinnersidentified">
                          <div class="databox kalypto-checkbox">
                            <div class="kalypto-container">
                              <input id="ControlIdentifyWinnersIsWinnersIdentified" class="kalypto" 
                                runat="server" type="checkbox" />
                            </div>
                            <div class="kalypto-checkbox-label">Check to mark this election as having all winners identified</div>
                            <div class="tab-ast" id="AsteriskIdentifyWinnersIsWinnersIdentified" EnableViewState="false" runat="server"></div>
                          </div>
                        </div>

                        <div class="clear-both spacer"></div>

                      </div>

                    </div>

                    <div class="instructions" id="IdentifyWinnersInstructions" runat="server">
                      <ul>
                        <li><span>Check or uncheck an <em>Office Title</em> to update 
                        incumbents in addition to recording winners.</span></li>
                        <li><span>Check or uncheck an <em>Office Class</em> (in bold) 
                        to check or uncheck all the offices in the class. The topmost 
                        checkbox will check or uncheck all offices in the election.</span></li>
                        <li><span>Candidates that have already been identified as 
                        <em>winners</em> are marked with a ◄ symbol.</span></li>
                        <li><span>Click <em>Update</em> at the bottom of the panel 
                        to save your changes.</span></li>
                      </ul>
                    </div>

                  </div>
 
                  <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackIdentifyWinners" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderIdentifyWinners" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonIdentifyWinners" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election order"
                      OnClick="ButtonIdentifyWinners_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
         
        <% } %>
        
        <% if (ShowAddBallotMeasures) { %>

        <div id="tab-addballotmeasures" class="main-tab content-panel tab-panel htab-panel">
          
          <asp:UpdatePanel ID="UpdatePanelSelectBallotMeasure" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerSelectBallotMeasure" class="sub-panel" runat="server">
              <input id="SelectBallotMeasureReloading" class="reloading" type="hidden" runat="server" />
              <input id="SelectBallotMeasureNoScrollState" class="ballot-measure-noscroll-state" type="hidden" runat="server" />
              <input id="SelectBallotMeasureScrollPosition" class="ballot-measure-scroll-position" type="hidden" runat="server" />
              <input id="SelectedBallotMeasureKey" runat="server" class="selected-ballot-measure-key" type="hidden" />
              <div class="select-ballot-measure-container-outer">
                <div class="select-ballot-measure-container shadow-2">
                  <div class="heading">
                    <div class="label">
                    <p class="fieldlabel"><em>Select a Ballot Measure or click the </em>New<em> button</em></p>
                    <p class="fieldlabel sub">Click the <em>Ballot Measure Title</em> above to hide or show this list of ballot measures</p>
                    </div>
                    <div class="undo-button tiptip" title="Revert the ballot measure order to the latest saved version">
                      <div id="UndoSelectBallotMeasure" runat="server"></div>
                    </div>
                    <div class="clear-both"></div>
                  </div>
                  <div class="slimscroll-toggler tiptip"
                   title="Show or hide the complete list of ballot measures. This is useful when dragging a ballot measure out of the viewable area."></div>
                  <div class="tab-ast" id="AsteriskSelectBallotMeasureList" EnableViewState="false" runat="server"></div>
                  <div class="select-ballot-measure-outer">
                    <input id="ControlSelectBallotMeasureListValue" type="hidden" runat="server" />
                    <ul ID="ControlSelectBallotMeasureList" runat="server" 
                      class="select-ballot-measure-control sortablelist value-in-hidden-field"
                      EnableViewState="false">
                    </ul>
                  </div>
                  <div class="content-footer">
                    <div class="feedback-floater-for-ie7">
                      <user:FeedbackContainer ID="FeedbackSelectBallotMeasure" 
                      EnableViewState="false" runat="server" />
                      </div>
                    <div class="footer-item footer-ajax-loader">
                      <asp:Image ID="AjaxLoaderSelectBallotMeasure" 
                      EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                        CssClass="ajax-loader" runat="server" />
                    </div>
                    <div class="update-button">
                      <input class="add-ballot-measure button-2 button-smallest"
                      type="button" value="New" />
                      <asp:Button ID="ButtonSelectBallotMeasure" EnableViewState="false" 
                        runat="server" 
                        Text="Update" CssClass="update-button button-1 button-smallest tiptip" 
                        Title="Update the ballot measureorder"
                        OnClick="ButtonSelectBallotMeasure_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   
                </div>
              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>

          <asp:UpdatePanel ID="UpdatePanelAddBallotMeasures" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerAddBallotMeasures" class="main-panel" runat="server">
              <input id="AddBallotMeasuresReloading" class="reloading" type="hidden" runat="server" />
              <input id="AddBallotMeasuresSubTabIndex" class="sub-tab-index" type="hidden" runat="server" />
              <input id="AddBallotMeasuresRecased" class="recased" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoAddBallotMeasures" runat="server"></div>
                    </div>
                    <h4 class="center-element">Add or Change Ballot Measures</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingAddBallotMeasures" runat="server">Please select an election from the list on the left</h6>
                <a class="select-ballot-measure-toggler" >
                <div></div><h6 id="HeadingAddBallotMeasuresBallotMeasure" class="ballot-measures-heading ellipsis" runat="server">No ballot measure selected</h6></a>
                <div class="clear-both"></div>
                <hr/>
                <div class="data-area">
                
                  <input id="AddBallotMeasuresAnimate" runat="server" class="addballotmeasures-animate" 
                  value="false" EnableViewState="false" type="hidden" />

                  <div class="input-element referendumtitle">
                    <p class="fieldlabel">Ballot Measure Title <span class="reqd">◄</span></p>
                    <div class="databox textarea">
                      <user:TextBoxWithNormalizedLineBreaks ID="ControlAddBallotMeasuresReferendumTitle" 
                      EnableViewState="false" TextMode="MultiLine" 
                      CssClass="shadow" runat="server" spellcheck="false" />
                      <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumTitle" 
		                    EnableViewState="false" runat="server"></div>
                    </div>
                  </div>
                  <div class="clear-both spacer"></div>

                  <div class="input-element passedstatus">
                    <p class="fieldlabel">Passed/Defeated Status</p>
                    <div class="databox dropdown">
                      <div class="shadow-2">
                        <asp:DropDownList ID="ControlAddBallotMeasuresPassedStatus" 
			                    class="passed-status" runat="server">
                          <asp:ListItem Value="unknown" Text="Status unknown"></asp:ListItem>
                          <asp:ListItem Value="passed" Text="Measure passed"></asp:ListItem>
                          <asp:ListItem Value="defeated" Text="Measure was defeated"></asp:ListItem>
                        </asp:DropDownList>
                      </div>
                      <div class="tab-ast" id="AsteriskAddBallotMeasuresPassedStatus" EnableViewState="false" runat="server"></div>
                    </div>
                  </div>
                 
                  <div class="input-element is-tag-for-deletion">
                    <div class="databox kalypto-checkbox">
                      <div class="kalypto-container">
                        <input id="ControlAddBallotMeasuresIsReferendumTagForDeletion" class="kalypto" 
                          runat="server" type="checkbox" />
                      </div>
                      <div class="kalypto-checkbox-label">Check to mark this ballot measure for deletion</div>
                      <div class="tab-ast" id="AsteriskAddBallotMeasuresIsReferendumTagForDeletion" EnableViewState="false" runat="server"></div>
                    </div>
                  </div>

                  <div class="clear-both spacer"></div>
                  
                  <div id="ballot-measure-tabs" class="jqueryui-tabs shadow">
                        
                    <ul class="htabs unselectable">
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addballotmeasures-referendumdesc" onclick="this.blur()" id="TabAddBallotMeasuresDesc" EnableViewState="false" runat="server">Description</a></li>
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addballotmeasures-referendumdetail" onclick="this.blur()" id="TabAddBallotMeasuresDetail" EnableViewState="false" runat="server">Detail</a></li>
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addballotmeasures-referendumfulltext" onclick="this.blur()" id="TabAddBallotMeasuresFullText" EnableViewState="false" runat="server">Full Text</a></li>
                    </ul>
                        
                    <div id="tab-addballotmeasures-referendumdesc">
                      
                      <div class="input-element remove-line-breaks">
                        <input type="button" value="Remove line breaks"
                        class="button-1 button-smallest disabled" />             
                      </div>

                      <div class="clear-both"></div>
                
                      <div class="input-element referendumdesc">
                        <div class="text-area-toggler tiptip" title="Show or hide the full text of the Ballot Measure Description"
                        onclick="toggleReferendumTextArea(this)"></div><p class="fieldlabel">Ballot Measure Description</p>
                        <div class="databox textarea">
                          <input id="ExpandableAddBallotMeasuresReferendumDesc" runat="server" type="hidden" />
                          <user:TextBoxWithNormalizedLineBreaks ID="ControlAddBallotMeasuresReferendumDesc" 
                          EnableViewState="false" TextMode="MultiLine" 
                          CssClass="shadow expandable" runat="server" spellcheck="false" />
                          <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumDesc" 
		                        EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both"></div>

                    </div>
                         
                    <div id="tab-addballotmeasures-referendumdetail">
                      
                      <div class="input-element remove-line-breaks">
                        <input type="button" value="Remove line breaks"
                        class="button-1 button-smallest disabled" />             
                      </div>
                
                      <div class="input-element url-label">
                        <p class="fieldlabel">Url:</p>
                      </div>

                      <div class="input-element url detailurl">
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlAddBallotMeasuresReferendumDetailUrl"  
                            CssClass="shadow-2 tiptip" title="http(s):// optional" runat="server"/>
                          <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumDetailUrl" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>

                      <div class="icon-box tiptip" title="View the ballot measure detail page">
                        <a class="icon" href="?" target="Show" id="IconBoxReferendumDetailUrl" EnableViewState="false" runat="server"></a>
                      </div>

                      <div class="clear-both"></div>
                
                      <div class="input-element referendumdetail">
                        <div class="text-area-toggler tiptip" title="Show or hide the full text of the Ballot Measure Detail"
                        onclick="toggleReferendumTextArea(this)"></div>
                        <p class="fieldlabel">Ballot Measure Detail</p>
                        <div class="databox textarea">
                          <input id="ExpandableAddBallotMeasuresReferendumDetail" runat="server" type="hidden" />
                          <user:TextBoxWithNormalizedLineBreaks ID="ControlAddBallotMeasuresReferendumDetail" 
                          EnableViewState="false" TextMode="MultiLine" 
                          CssClass="shadow expandable" runat="server" spellcheck="false" />
                          <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumDetail" 
		                        EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both"></div>

                    </div>
                        
                    <div id="tab-addballotmeasures-referendumfulltext">
                      
                      <div class="input-element remove-line-breaks">
                        <input type="button" value="Remove line breaks"
                        class="button-1 button-smallest disabled" />             
                      </div>
                
                      <div class="input-element url-label">
                        <p class="fieldlabel">Url:</p>
                      </div>

                      <div class="input-element url fulltexturl">
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlAddBallotMeasuresReferendumFullTextUrl"  
                            CssClass="shadow-2 tiptip" title="http(s):// optional" runat="server"/>
                          <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumFullTextUrl" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>

                      <div class="icon-box tiptip" title="View the ballot measure full text page">
                        <a class="icon" href="?" target="Show" id="IconBoxReferendumFullTextUrl" EnableViewState="false" runat="server"></a>
                      </div>

                      <div class="clear-both"></div>
                
                      <div class="input-element referendumfulltext">
                        <div class="text-area-toggler tiptip" title="Show or hide the full text of the Ballot Measure Full Text"></div>
                        <p class="fieldlabel">Ballot Measure Full Text</p>
                        <div class="databox textarea">
                          <input id="ExpandableAddBallotMeasuresReferendumFullText" runat="server" type="hidden" />
                          <user:TextBoxWithNormalizedLineBreaks ID="ControlAddBallotMeasuresReferendumFullText" 
                          EnableViewState="false" TextMode="MultiLine" 
                          CssClass="shadow expandable" runat="server" spellcheck="false" />
                          <div class="tab-ast" id="AsteriskAddBallotMeasuresReferendumFullText" 
		                        EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both"></div>

                    </div>

                  </div>
                 
                </div>
 
                <div class="clear-both"></div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddBallotMeasures" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddBallotMeasures" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddBallotMeasures" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the ballot measure"
                      OnClick="ButtonAddBallotMeasures_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
         
        <% } %>
        
  <% if (IsMasterUser || IsStateAdminUser)
     { %>
        <div id="tab-viewreport" class="main-tab content-panel tab-panel htab-panel">
          <div class="col get-report">
             <input type="button" value="Get Report" 
                class="get-report-button button-1 button-smallest"/>
          </div>  
          <div class="col get-report-in-new-window checkbox">
             <input type="checkbox" id="get-report-in-new-window-checkbox"
                class="get-report-in-new-window-checkbox"/>
             <label for="get-report-in-new-window-checkbox">In new window</label>
          </div> 
          <%-- 
          <div class="col pre-open-accordions checkbox">
             <input type="checkbox" id="pre-open-accordions-checkbox"
                class="pre-open-accordions-checkbox"/>
             <label for="pre-open-accordions-checkbox">Pre-open all accordions</label>
          </div>  
          --%>
          <h6 class="report-head inverted"></h6>
          <div class="clear-both"></div>
          <div id="ElectionReport" class="report hidden"></div>      
          <iframe id="ElectionIFrame" class="hidden"></iframe>      
        </div>
 <%  } %>
 
  <% if (IsMasterUser)
     { %>
        <div id="tab-masteronly" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelMasterOnly" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerMasterOnly" runat="server">
              <input id="MasterOnlyReloading" class="reloading" type="hidden" runat="server" />
              <input id="ContainerMasterOnlySubTabIndex" class="sub-tab-index" type="hidden" runat="server" value="0" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoMasterOnly" runat="server"></div>
                    </div>
                    <h4 class="center-element">Master-Only Functions</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <h6 class="panel-heading inverted ellipsis" id="HeadingMasterOnly" runat="server">Please select an election from the list on the left</h6>
                <hr/>

                <div class="data-area">
                  
                  <div id="master-only-tabs" class="jqueryui-tabs shadow">
                        
                    <ul class="htabs unselectable">
                      <li class="tab htab" id="TabMasterOnlyChangeDate" EnableViewState="false" runat="server"><a href="#tab-masteronly-changedate" onclick="this.blur()" EnableViewState="false" runat="server">Change<br />Election Date</a></li>
                      <li class="tab htab master-includeelection-tab" id="TabMasterOnlyIncludeElection" EnableViewState="false" runat="server"><a href="#tab-masteronly-include-election" onclick="this.blur()" EnableViewState="false" runat="server">Include Another Primary<br />In This Election</a></li>
                      <li class="tab htab" id="TabMasterOnlyAddCandidates" EnableViewState="false" runat="server"><a href="#tab-masteronly-addcandidates" onclick="this.blur()" EnableViewState="false" runat="server">Add Candidates From<br />Previous Elections</a></li>
                      <li class="tab htab master-addprimarywinners-tab" id="TabMasterOnlyAddPrimaryWinners" EnableViewState="false" runat="server"><a href="#tab-masteronly-addprimarywinners" onclick="this.blur()" EnableViewState="false" runat="server">Add Winners From<br />Previous Primaries</a></li>
                      <li class="tab htab master-addrunoffadvancers-tab" id="TabMasterOnlyAddRunoffAdvancers" EnableViewState="false" runat="server"><a href="#tab-masteronly-addrunoffadvancers" onclick="this.blur()" EnableViewState="false" runat="server">Add Runoff Advancers From<br />Previous Election</a></li>
                      <li class="tab htab" id="TabMasterOnlyDeleteElection" EnableViewState="false" runat="server"><a href="#tab-masteronly-deleteelection" onclick="this.blur()" EnableViewState="false" runat="server">Delete<br />Election</a></li>
                      <li class="tab htab" id="TabMasterOnlyCreateGeneral" EnableViewState="false" runat="server"><a href="#tab-masteronly-creategeneral" onclick="this.blur()" EnableViewState="false" runat="server">Create General<br />Election</a></li>
                      <li class="tab htab" id="TabMasterOnlyThirdParty" EnableViewState="false" runat="server"><a href="#tab-masteronly-thirdparty" onclick="this.blur()" EnableViewState="false" runat="server">3rd Party<br />Vote Sites</a></li>
                      <li class="tab htab" id="TabMasterOnlyStatusNotes" EnableViewState="false" runat="server"><a href="#tab-masteronly-statusnotes" onclick="this.blur()" EnableViewState="false" runat="server">Status<br />Notes</a></li>
                    </ul>

                    <% if (AdminPageLevel == AdminPageLevel.State) { %> 
                                          
                    <div id="tab-masteronly-changedate">

                      <input id="MasterOnlyDateWasChanged" class="date-was-changed" type="hidden" runat="server" />
                    
                      <div class="instructions">
                        <ul>
                          <li><span>If it is necessary to change the date of an election, first enter the new <em>Election Date</em> and click <em>Update</em> below.</span></li>
                          <li><span>The date will be changed in the database and a new <em>Election Title</em> will be displayed. If necessary, make changes to the title and click <em>Update</em> again.</span></li>
                        </ul>
                      </div>
                 
                      <div class="input-element electiondate">
                        <p class="fieldlabel">Election Date <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyElectionDate" placeholder="mm/dd/yyyy" 
                            CssClass="shadow-2 tiptip date-picker" title="Enter new election date as mm/dd/yyyy" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyElectionDate" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>

                      <div class="input-element electiondesc">
                        <p class="fieldlabel">Election Title <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyElectionDesc"  
                            CssClass="shadow-2 tiptip" title="Maximum length is 90 characters" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyElectionDesc" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>

                    </div>

                    <div id="tab-masteronly-include-election">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>This function is typically used to include office and candidates from a non-partisan primary into a concurrent party primary. It is only available at the state level for primaries.</span></li>
                        </ul>
                      </div>

                      <div class="input-element electionkeytoinclude">
                        <p class="fieldlabel">Election To Include</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <select ID="ControlMasterOnlyElectionKeyToInclude" runat="server"/>
                          </div>
                          <div class="tab-ast" id="AsteriskMasterOnlyElectionKeyToInclude" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>

                    </div>
                    
                    <% } %>

                    <div id="tab-masteronly-addcandidates">
 
                      <input id="MasterOnlyElectionKeyToCopy" class="election-key-to-copy" type="hidden" runat="server" />
                   
                      <div class="instructions">
                        <ul>
                          <li><span>If the candidates for a new election are similar to a previous election, you can copy all candidates for each office to the new election from the previous election.
                          Click the <em>Select Election to Copy</em> button, then click <em>Update</em>.</span></li>
                          <li><span>To copy different offices from different elections, use the following procedure:</span>
                            <ul>
                              <li><span>Use the <em>Add/Remove Offices</em> tab to add just the offices you want to copy from one election, then use this panel to copy from that election.</span></li>
                              <li><span>Use the <em>Add/Remove Offices</em> tab to add the offices you want to copy from the next election, then use this panel to copy from that election. Offices that already have candidates will not be copied.</span></li>
                              <li><span>Finally, use the <em>Add/Remove Offices</em> tab to add any offices that you don't want to copy.</span></li>
                            </ul>
                          </li>
                          <li><span>After copying, use the <em>Add/Remove Offices</em> and the <em>Setup Candidates for Office</em> tabs to make any necessary adjustments.</span></li>
                        </ul>
                      </div>

                      <div class="input-element electiontocopy">
                        <p class="fieldlabel">Election to Copy <span class="reqd">◄</span>        
                        <input id="select-election-to-copy" type="button" 
                          value="Select Election to Copy" 
                          class="select-election-to-copy-button button-2 button-smallest" /></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyElectionToCopy"  Enabled="false"
                            CssClass="shadow-2 tiptip" title="Click the 'Select Election to Copy' button to change this field" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyElectionToCopy" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>

                    </div>
 
                    <div id="tab-masteronly-addprimarywinners">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>After the winners for all primaries preceding this general election have been identified,
                          you can add all primary winners as candidates for the general election.
                          Enter the date of the primary elections, then click <em>Update</em>.</span></li>
                          <li><span>Any candidate already added to the 
                          general election is not added again, so this can be safely re-run.</span></li>
                        </ul>
                      </div>

                      <div class="input-element primarydatetocopy">
                        <p class="fieldlabel">Primary Election Date <span class="reqd">◄</span></p>
                        <asp:HiddenField ID="HiddenMasterOnlyPrimaryDateToCopy" ClientIDMode="static" runat="server" />
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyPrimaryDateToCopy"  
                            CssClass="shadow-2 tiptip date-picker primary-date-to-copy" title="Enter election date as mm/dd/yyyy" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyPrimaryDateToCopy" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>

                      <div class="input-element runoffdatetocopy">
                        <p class="fieldlabel">Runoff Election Date</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyRunoffDateToCopy"  
                            CssClass="shadow-2 tiptip date-picker runoff-date-to-copy" title="Enter election date as mm/dd/yyyy" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyRunoffDateToCopy" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>
                  
                      <div class="input-element enableprimarywinners">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlMasterOnlyEnableCopyPrimaryWinners" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Check to enable update.</div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>
                      
                    </div>
 
                    <div id="tab-masteronly-addrunoffadvancers">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>After all the runoff candidates for the preceding election have been identified,
                          you can add then to the runoff election.
                          Enter the date of the preceeding election, then click <em>Update</em>.</span></li>
                          <li><span>This operation completely replaces the election roster for this runoff, so this can be safely re-run.</span></li>
                        </ul>
                      </div>

                      <div class="input-element electiondatetocopy">
                        <p class="fieldlabel">Previous Election Date <span class="reqd">◄</span></p>
                        <asp:HiddenField ID="HiddenMasterOnlyElectionDateToCopy" ClientIDMode="static" runat="server" />
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyElectionDateToCopy"  
                            CssClass="shadow-2 tiptip date-picker election-date-to-copy" title="Enter election date as mm/dd/yyyy" runat="server"/>
                          <div class="tab-ast" id="AsteriskMasterOnlyElectionDateToCopy" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both spacer"></div>
                      
                    </div>
                        
                    <div id="tab-masteronly-deleteelection">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>To delete all data for this election, check the box and click <em>Update</em>.</span></li>
                          <% if (AdminPageLevel == AdminPageLevel.State) { %> 
                          <li><span>Deleting a state election will also delete all associated county and local elections.</span></li>
                          <% } %>
                          <li><span>Be careful. This cannot be undone.</span></li>
                        </ul>
                      </div>
                  
                      <div class="input-element deleteelection">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlMasterOnlyDeleteElection" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Check to completely delete this election. This action cannot be undone.</div>
                          <div class="tab-ast" id="AsteriskMasterOnlyDeleteElection" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>

                    </div>

                    <div id="tab-masteronly-creategeneral">
                    
                      <div class="instructions">
                        <ul>
                          <li><span>This tab will create an even-year general election for <em>all 50 states plus DC</em> plus the state-by-state 
                           pseudo-elections for US President, US Senate, US House and Governors.</span></li>
                          <li><span>Standard office contests are automatically added to each election.</span></li>
                          <li><span>This should only be run once every 2 years. The currently-selected jurisdiction doesn't matter.</span></li>
                          <li><span>If an election already exists, it <em>will not</em> be replaced or recreated, so it is safe to re-run this function.</span></li>
                        </ul>
                      </div>
                  
                      <div class="input-element generalelectiondate">
                        <p class="fieldlabel">Election Date <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyGeneralElectionDate" placeholder="mm/dd/yyyy" 
                            CssClass="shadow-2 tiptip date-picker" title="Enter date as mm/dd/yyyy" runat="server"/>
                        </div>                
                      </div>

                      <div class="clear-both"></div>

                      <div class="input-element generalpastelection">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlMasterOnlyGeneralPastElection" class="kalypto mc-nomonitor" 
                            runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Check to allow an election date in the past</div>
                        </div>
                      </div>
 
                      <div class="clear-both"></div>
                 
                      <div class="input-element generalincludepresident">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlMasterOnlyGeneralIncludePresident" class="kalypto mc-nomonitor" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Check to include the US Presidential contest</div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>

                      <div class="input-element generalelectiondesc">
                        <p class="fieldlabel">Election Title <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyGeneralElectionDesc"  
                            CssClass="shadow-2 tiptip" title="Maximum length is 90 characters" runat="server"/>
                        </div>                
                      </div>

                      <div class="clear-both spacer"></div>

                    </div>
                        
                    <div id="tab-masteronly-thirdparty">
                      <input type="button" id="voters-edge-button" class="button-2 button-smaller"
                             value="Download CSV for This Election" />
<%--                      <input type="checkbox" id="voters-edge-include-answers"/>Include topic answers--%>
                      <div class="csv-type-radios">
                        <input id="type-no-answers" value="NA" type="radio" name="csv-type" checked="checked"/><label for="type-no-answers">Candidates for each office contest with links, pictures, bios & reasons ONLY</label>
                        <br />
                        <input id="type-only-answers" value="OA" type="radio" name="csv-type"/><label for="type-only-answers">Candidates for each office contest with issue topic responses ONLY</label>
                        <br />
                        <input id="type-with-answers" value="WA" type="radio" name="csv-type"/><label for="type-with-answers">Candidates for each office contest WITH BOTH links, pictures, bios & reasons AND issue topic responses</label>
                        <br />
                        <input id="type-only-keys" value="OK" type="radio" name="csv-type"/><label for="type-only-keys">Candidates for each office contest ONLY NAMES & KEYS</label>
                        <br />
                        <input id="type-ballot-measures" value="BM" type="radio" name="csv-type"/><label for="type-ballot-measures">Ballot measures</label>
                      </div>
                      <a class="hidden" id="voters-edge-anchor"></a>
                    </div>
                        
                    <div id="tab-masteronly-statusnotes">
                
                    <div class="input-element electionstatus">
                      <p class="fieldlabel">Status Notes</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlMasterOnlyElectionStatus" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskMasterOnlyElectionStatus" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    </div>

                  </div>
                  
                </div>
 
                <div class="clear-both"></div>
                </div>
                
                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackMasterOnly" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderMasterOnly" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonMasterOnly" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election order"
                      OnClick="ButtonMasterOnly_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
  <% } %>
  
      </div>
    </div>
  
  </div>

  <user:NavigateJurisdiction runat="server" />

</asp:Content>
