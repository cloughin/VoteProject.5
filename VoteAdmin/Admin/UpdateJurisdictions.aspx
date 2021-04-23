<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="UpdateJurisdictions.aspx.cs" 
Inherits="Vote.Admin.UpdateJurisdictionsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/NavigateJurisdiction.ascx" TagName="NavigateJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/SelectJurisdictionButton.ascx" TagName="SelectJurisdictionButton" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
      <h3 id="MultiCountyMessage" class="multi-county-message hidden" runat="server"></h3>
      <h4 id="CredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
    </div>
    <user:SelectJurisdictionButton ID="SelectJurisdictionButton" runat="server"
     Tooltip="Update jurisdictions for a different state, county or local jurisdiction -- subject to your sign-in credentials."/>
    
    <a id="SetupElectedAdButton" runat="server" class="setup-elected-ad-button button-2 button-smaller" target="ad">Setup Elected Page Ad</a>
    <a id="SetupBallotAdButton" runat="server" class="setup-ballot-ad-button button-2 button-smaller" target="ad">Setup Ballot Page Ad</a>

    <div class="clear-both"></div>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server" id="TabGeneralVoterInfo"><a href="#tab-generalvoterinfo" onclick="this.blur()" id="TabGeneralVoterInformation" EnableViewState="false" runat="server">General Voter<br />Information</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabVoterUrls"><a href="#tab-voterurls" onclick="this.blur()" id="TabStateVoterUrls" EnableViewState="false" runat="server">State Voter<br />URLs</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-ballot" onclick="this.blur()" id="TabBallot" EnableViewState="false" runat="server">Ballot<br />Settings</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-electionauthority" onclick="this.blur()" id="TabElectionAuthority" EnableViewState="false" runat="server">Election<br />Authority</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabAddDistrictsItem"><a href="#tab-adddistricts" onclick="this.blur()" id="TabAddDistricts" EnableViewState="false" runat="server">Add Single<br />Districts</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabDeleteDistrictsItem"><a href="#tab-deletedistricts" onclick="this.blur()" id="TabDeleteDistricts" EnableViewState="false" runat="server">Delete<br />Districts</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabIncludeMultiCountyDistrictsItem"><a href="#tab-includemulticountydistricts" onclick="this.blur()" id="TabIncludeMultiCountyDistricts" EnableViewState="false" runat="server">Include Multi-<br />County Districts</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabRemoveMultiCountyDistrictsItem"><a href="#tab-removemulticountydistricts" onclick="this.blur()" id="TabRemoveMultiCountyDistricts" EnableViewState="false" runat="server">Remove Multi-<br />County Districts</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabSetupCountySupervisorsItem"><a href="#tab-setupcountysupervisors" onclick="this.blur()" id="TabSetupCountySupervisors" EnableViewState="false" runat="server">Setup County<br />Supervisors</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabSetupCityCouncilItem"><a href="#tab-setupcitycouncil" onclick="this.blur()" id="TabSetupCityCouncil" EnableViewState="false" runat="server">Setup City<br />Council</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabSetupSchoolDistrictItem"><a href="#tab-setupschooldistrict" onclick="this.blur()" id="TabSetupSchoolDistrict" EnableViewState="false" runat="server">Setup School<br />District</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabTigerSettingsItem"><a href="#tab-tigersettings" onclick="this.blur()" id="TabTigerSettings" EnableViewState="false" runat="server">Tiger<br />Settings</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabViewReportItem"><a href="#tab-viewreports" onclick="this.blur()" id="TabViewReports" EnableViewState="false" runat="server">View<br />Reports</a></li>
        </ul>

  <% if (AdminPageLevel == AdminPageLevel.State)
     { %>

        <div id="tab-generalvoterinfo" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelGeneralVoterInfo" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerGeneralVoterInfo" runat="server">
              <input id="GeneralVoterInfoReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoGeneralVoterInfo" runat="server"></div>
                    </div>
                    <h4 class="center-element">Enter or Change General Voter Information</h4>
                  </div>
                </div>
                <div class="instructions heading-instructions">This information is shown in the <em>Voting Information</em> accordion on ballots. Both the General and Primary entries should explain the voter identification requirements. For Primary Elections an explanation also should be provided regarding the various party ballots and the requirements to vote in each of them.</div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
                
                    <div class="input-element pollhours">
                      <p class="fieldlabel">Normal Polling Hours</p>
                      <p class="fieldlabel sub">Enter any time range format, like <em>7:00 AM-7:00 PM</em>
                      or an explanation like <em>Poll times vary depending on the county and location</em>.</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlGeneralVoterInfoPollHours" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoPollHours" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                
                    <div class="input-element howvotingisdone">
                      <p class="fieldlabel">Voter ID Requirements and Poll Procedures for <em>General</em> Elections</p>
                      <div class="databox textarea is-resizable">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlGeneralVoterInfoHowVotingIsDone" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoHowVotingIsDone" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element howprimariesaredone">
                      <p class="fieldlabel">Voter ID Requirements and Poll Procedures for <em>Primary</em> Elections</p>
                      <div class="databox textarea is-resizable">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlGeneralVoterInfoHowPrimariesAreDone" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow is-resizable" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoHowPrimariesAreDone" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                </div>
 
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackGeneralVoterInfo" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderGeneralVoterInfo" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonGeneralVoterInfo" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the general voter information"
                      OnClick="ButtonGeneralVoterInfo_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

        <div id="tab-voterurls" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelVoterUrls" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerVoterUrls" runat="server">
              <input id="VoterUrlsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoVoterUrls" runat="server"></div>
                    </div>
                    <h4 class="center-element">State Web Addresses (URLs)</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
               
                    <div class="input-element voterregistrationwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsVoterRegistrationWebAddress" runat="server">Voter Registration URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsVoterRegistrationWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the voter registration information, if available (http(s):// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoterRegistrationWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element earlyvotingwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsEarlyVotingWebAddress" runat="server">Early Voting URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsEarlyVotingWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the early voting information, if available (http(s):// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsEarlyVotingWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element votebymailwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsVoteByMailWebAddress" runat="server">Vote by Mail URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsVoteByMailWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the vote by mail information, if available (http(s):// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoteByMailWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element votebyabsenteeballotwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsVoteByAbsenteeBallotWebAddress" runat="server">Vote by Absentee Ballot URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsVoteByAbsenteeBallotWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the vote by absentee ballots information, if available (http(s):// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoteByAbsenteeBallotWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element pollhoursurl webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsPollHoursUrl" runat="server">Polling Hours URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlVoterUrlsPollHoursUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of a page with the polling hours information, if available (http:(s)// optional)" runat="server"/>
                        <div class="tab-ast" id="AsteriskVoterUrlsPollHoursUrl" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element pollplacesurl webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsPollPlacesUrl" runat="server">Polling Places URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlVoterUrlsPollPlacesUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of a page with polling location information, if available (http:(s)// optional)" runat="server"/>
                        <div class="tab-ast" id="AsteriskVoterUrlsPollPlacesUrl" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>
                    <div class="clear-both spacer"></div>

                </div>
 
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackVoterUrls" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderVoterUrls" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonVoterUrls" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the general voter information"
                      OnClick="ButtonVoterUrls_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        
        <div id="tab-ballot" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdateBallot" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerBallot" runat="server">
              <input id="BallotReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoBallot" runat="server"></div>
                    </div>
                    <h4 class="center-element">How Ballots are Displayed</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
                  
                    <div class="input-element enclosenickname">
                      <div class="databox kalypto-radio-element">
                        <div class="tab-ast" id="AsteriskBallotEncloseNickname" EnableViewState="false" runat="server"></div>
                        <p class="fieldlabel kalypto-radio-label">How nicknames are enclosed</p>
                        <div id="ControlBallotEncloseNickname" class="radio-container kalypto-radio-container" runat="server">
                          <input id="BallotEncloseNicknameDoubleQuotes" class="kalypto" type="radio" name="BallotEncloseNickname" value="D" runat="server" ClientIDMode="Static" />
			                    <label for="BallotEncloseNicknameDoubleQuotes">In double quotes: "Nickname"</label>
                          <input id="BallotEncloseNicknameSingleQuotes" class="kalypto" type="radio" name="BallotEncloseNickname" value="S" runat="server" ClientIDMode="Static" />
			                    <label for="BallotEncloseNicknameSingleQuotes">In single quotes: 'Nickname'</label>
                          <input id="BallotEncloseNicknameParens" class="kalypto" type="radio" name="BallotEncloseNickname" value="P" runat="server" ClientIDMode="Static" />
			                    <label for="BallotEncloseNicknameParens">In parens: (Nickname)</label>
                        </div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                
                    <div class="input-element ballotstatename">
                      <p class="fieldlabel">Ballot State Name</p>
                      <p class="fieldlabel sub">The state name that will appear on ballots. For example Virginia could be presented as <em>Commonwealth of Virginia</em>.</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlBallotBallotStateName" 
                        EnableViewState="false" CssClass="shadow-2" runat="server" spellcheck="false" />
                        <div class="tab-ast" id="AsteriskBallotBallotStateName" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                  
                    <div class="check-boxes">
                      <div class="input-element isincumbentshownonballots">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlBallotIsIncumbentShownOnBallots" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Show incumbent (with an asterisk following the name)</div>
                          <div class="tab-ast" id="AsteriskBallotIsIncumbentShownOnBallots" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>
                  
                      <div class="input-element showunopposed">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlBallotShowUnopposed" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Show contests with unopposed candidates</div>
                          <div class="tab-ast" id="AsteriskBallotShowUnopposed" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>
                    </div>
                    
                    <div class="check-boxes">
                      <div class="input-element showwritein">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlBallotShowWriteIn" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Show a write-in line for each non-primary contest</div>
                          <div class="tab-ast" id="AsteriskBallotShowWriteIn" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>
                  
                      <div class="input-element showprimarywritein">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlBallotShowPrimaryWriteIn" class="kalypto" 
                              runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Show a write-in line for each primary contest</div>
                          <div class="tab-ast" id="AsteriskBallotShowPrimaryWriteIn" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      <div class="clear-both spacer"></div>
                    </div>
                 
                    <div class="input-element electionadditionalinfo">
                      <p class="fieldlabel"><em>Default</em> Additional Election Information on <em>All</em> Ballots and Reports</p>
                      <p class="fieldlabel sub">For new elections, this is the default information that will appear on the top of all ballots 
                      and reports. It usually contains a disclaimer of the election 
                      contests not covered. Click one of the <em>Disclaimer</em> buttons below to automatically 
                      load the appropriate text. You can then modify the loaded text, or you can simply 
                      enter custom text.</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlBallotElectionAdditionalInfo" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskBallotElectionAdditionalInfo" EnableViewState="false" runat="server"></div>
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
                      <p class="fieldlabel sub">For new elections, this is the default information that will appear only on ballots, just below the <em>Additional Election Information</em></p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlBallotBallotInstructions" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskBallotBallotInstructions" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                <div class="clear-both spacer"></div>
               
                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackBallot" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderBallot" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonBallot" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the primary voter information"
                      OnClick="ButtonBallot_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

        <div id="tab-viewreports" class="main-tab content-panel tab-panel htab-panel">
          <div class="col select-report">
             <select class="select-report">
               <option value="">&lt;Select a report&gt;</option>
               <option value="ctyc">County Contacts</option>
               <option value="elof">Elected Officials</option>
               <option value="ctyj">County Jurisdictions</option>
               <option value="locj">Local Jurisdictions</option>
             </select>
          </div>  
          <div class="col get-report">
             <input type="button" value="Get Report" 
                class="get-report-button button-1 button-smallest"/>
          </div>  
          <div class="col get-report-in-new-window">
             <input type="checkbox" id="get-report-in-new-window-checkbox"
                class="get-report-in-new-window-checkbox"/>
             <label for="get-report-in-new-window-checkbox">In new window</label>
          </div>  
          <div class="clear-both"></div>
          <div id="Report" class="report hidden"></div>      
        </div>
  
  <% } %>

        <div id="tab-electionauthority" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelElectionAuthority" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerElectionAuthority" runat="server">
              <input id="ElectionAuthorityReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoElectionAuthority" runat="server"></div>
                    </div>
                    <h4 class="center-element">Enter or Change Election Authority Information</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">

                    <div class="input-element jurisdictionname">
                      <p class="fieldlabel">Jurisdiction Name <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlElectionAuthorityJurisdictionName"  
                          CssClass="shadow-2 tiptip" title="Enter the name of the jurisdiction" runat="server"/>
                        <div class="tab-ast" id="AsteriskElectionAuthorityJurisdictionName" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>

                    <div class="input-element url">
                      <p class="fieldlabel"><span id="LabelElectionAuthorityUrl" runat="server">Web Address</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlElectionAuthorityUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of the election authority web site (http(s):// optional)" runat="server"/>
                        <div class="tab-ast" id="AsteriskElectionAuthorityUrl" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>

                    <div class="input-element email">
                      <p class="fieldlabel"><span id="LabelElectionAuthorityEmail" runat="server">Email Address for Voters</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlElectionAuthorityEmail"  
                          CssClass="shadow-2 tiptip" title="Enter the email address that voters should use to contact the election authority" runat="server"/>
                        <div class="tab-ast" id="AsteriskElectionAuthorityEmail" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>
                    <div class="clear-both spacer"></div>
                    
                    <div class="data-group mailing-address">
 
                      <div class="input-element addressline1">
                        <p class="fieldlabel">Mailing Address</p>
                        <p class="fieldlabel sub">Line 1</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAddressLine1"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAddressLine1" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element addressline2">
                        <p class="fieldlabel sub">Line 2</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAddressLine2"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAddressLine2" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element citystatezip">
                        <p class="fieldlabel sub">City State Zip</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityCityStateZip"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityCityStateZip" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>
 
                    </div>
                    
                    <div class="data-group main-contact">

                      <div class="input-element contact">
                        <p class="fieldlabel head"><span id="LabelElectionAuthorityContactEmail" runat="server">Main Contact</span></p>
                        <p class="move-button"><input type="button" value="Move to Notes" class="move-main-to-notes-button move-to-notes-button button-2 button-smallest"/></p>
                        <p class="fieldlabel sub name">Name</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityContact"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityContact" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element contacttitle">
                        <p class="fieldlabel sub">Title</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityContactTitle"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityContactTitle" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element contactphone">
                        <p class="fieldlabel sub">Phone</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityPhone"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityPhone" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element contactemail">
                        <p class="fieldlabel sub">Email</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityContactEmail"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityContactEmail" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                    </div>
                    
                    <div class="data-group swap-button tiptip"
                     title="Swap the Main Contact and the Alternate Contact"></div>
                    
                    <div class="data-group alternate-contact">

                      <div class="input-element altcontact">
                        <p class="fieldlabel head"><span id="LabelElectionAuthorityAltEmail" runat="server">Alt Contact</span></p>
                         <p class="move-button"><input type="button" value="Move to Notes" class="move-alt-to-notes-button move-to-notes-button button-2 button-smallest"/></p>
                       <p class="fieldlabel sub name">Name</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAltContact"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAltContact" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element altcontacttitle">
                        <p class="fieldlabel sub">Title</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAltContactTitle"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAltContactTitle" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element altcontactphone">
                        <p class="fieldlabel sub">Phone</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAltPhone"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAltPhone" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                      <div class="input-element altcontactemail">
                        <p class="fieldlabel sub">Email</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlElectionAuthorityAltEmail"  
                            CssClass="shadow-2" runat="server"/>
                          <div class="tab-ast" id="AsteriskElectionAuthorityAltEmail" EnableViewState="false" runat="server"></div>
                        </div>                
                      </div>
                      <div class="clear-both"></div>

                    </div>

                    <div class="clear-both"></div>
             
                    <div class="input-element notes">
                      <p class="fieldlabel">Notes</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlElectionAuthorityNotes" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskElectionAuthorityNotes" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both"></div>
               
                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackElectionAuthority" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderElectionAuthority" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonElectionAuthority" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the election authority information"
                      OnClick="ButtonElectionAuthority_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

  <% if (AdminPageLevel == AdminPageLevel.County)
     { %>

        <div id="tab-adddistricts" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelAddDistricts" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerAddDistricts" runat="server">
              <input id="AddDistrictsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <h4 class="center-element">Add Single Local Districts for This County</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
                  
                    <div class="col col1">
                      <div class="input-element localdistrict">
                        <p class="fieldlabel">New Local District Name <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlAddDistrictsLocalDistrict"
                            CssClass="shadow-2 tiptip mc-nomonitor" title="Enter the name of the local district" runat="server"/>
                          <p class="fieldlabel sub">Will be re-cased if not already mixed case.</p>
                        </div>                
                      </div>

                      <div class="clear-both spacer"></div>

                      <div class="input-element entirecounty">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlAddDistrictsEntireCounty" class="add-districts-entire-county kalypto mc-nomonitor" 
                            runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Include this district in entire county</div>
                        </div>
                      </div>
                      
                      <div class="tiger-selection">
                      <div class="clear-both spacer"></div>
                        
                      <div class="instructions">Select <em>ONE</em> of the following. Items already assigned are disabled.</div>

                      <div class="input-element tiger tigerdistrict">
                        <p class="fieldlabel">Assign Tiger Place</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlAddDistrictsTigerPlace" 
                              CssClass="mc-nomonitor" runat="server"/>
                          </div>
                        </div>
                      </div>
                        
                      <div class="clear-both spacer" ></div>

                      <div class="input-element tiger tigerdistrict">
                        <p class="fieldlabel">Assign Tiger District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlAddDistrictsTigerDistrict" 
                              CssClass="mc-nomonitor" runat="server"/>
                          </div>
                        </div>
                      </div>
                        
                      <div class="clear-both spacer" ></div>

                      <div class="input-element tiger schooldistrict">
                        <p class="fieldlabel">Assign School District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlAddDistrictsSchoolDistrict" 
                              CssClass="mc-nomonitor" runat="server"/>
                          </div>
                        </div>
                      </div>
                        
                      <div class="clear-both spacer" ></div>

                      <div class="input-element tiger councildistrict">
                        <p class="fieldlabel">Assign City Council District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlAddDistrictsCouncilDistrict" 
                              CssClass="mc-nomonitor" runat="server"/>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>


                      <div class="input-element tiger supervisordistrict">
                        <p class="fieldlabel">Assign County Supervisor District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlAddDistrictsSupervisorsDistrict" 
                              CssClass="mc-nomonitor" runat="server"/>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>

                      </div>

                      <div class="clear-both spacer"></div>
                      
                      <div id="AddDistrictsOverride" runat="server" class="override-box rounded-border boxed-group hidden">
                        <div class="boxed-group-label">Potential Duplicate District or Code</div>
                        <p class="duplicate-message">There were one or more potential duplicate district names detected (highlighted in red in the list at right).</p>
                        <p>If you want to proceed with adding the district anyway, check the box below and click <em>Add</em> again.</p>
                        <div class="input-element override">
                          <div class="databox kalypto-checkbox">
                            <div class="kalypto-container">
                              <input id="ControlAddDistrictsOverride" class="kalypto mc-nomonitor" 
                              runat="server" type="checkbox" />
                            </div>
                            <div class="kalypto-checkbox-label">Check to add conflicted district</div>
                          </div>
                        </div>
                        <div class="clear-both spacer"></div>
                      </div>

                    </div>
                 
                    <div class="col col2">
                      <p class="head">Current Districts in this County</p>
                      <div id="AddDistrictsCurrentDistricts" runat="server" class="current-districts"></div>
                    </div>

                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddDistricts" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddDistricts" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddDistricts" EnableViewState="false" 
                     runat="server" 
                      Text="Add" CssClass="update-button button-1 tiptip no-disable" 
                      Title="Add the new district"
                      OnClick="ButtonAddDistricts_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

        <div id="tab-deletedistricts" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelDeleteDistricts" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerDeleteDistricts" runat="server">
              <input id="DeleteDistrictsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <h4 class="center-element">Delete Local Districts from This County</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">

                <div class="input-element localkey" id="DeleteDistrictsLocalKey" runat="server">
                  <p class="fieldlabel">Local District to Delete <span class="reqd">◄</span></p>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlDeleteDistrictsLocalKey" 
                        CssClass="delete-district-local-key-dropdown mc-nomonitor" runat="server"/>
                    </div>
                  </div>
                </div>

                <div class="input-element override hidden" id="DeleteDistrictOverride" runat="server">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlDeleteDistrictOverride" class="kalypto mc-nomonitor" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Check to delete district along with all related data</div>
                  </div>
                </div>

                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackDeleteDistricts" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderDeleteDistricts" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonDeleteDistricts" EnableViewState="false" 
                     runat="server" 
                      Text="Delete" CssClass="update-button button-1 tiptip no-disable" 
                      Title="Delete the district"
                      OnClick="ButtonDeleteDistricts_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

        <div id="tab-includemulticountydistricts" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelIncludeMultiCountyDistricts" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerIncludeMultiCountyDistricts" runat="server">
              <input id="IncludeMultiCountyDistrictsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <h4 class="center-element">Include Multi-County Districts for This County</h4>
                  </div>
                </div>
                <div class="instructions heading-instructions">This setting is only for districts that are marked 
                  <em>Include this district in entire county</em> in the Tiger Settings, and will control in which 
                  counties the district displays on ballots. For all other districts, the shapefiles 
                  determine the district visibility on ballots, admin panels and reports.</div>
                <hr/>
                <div class="content-area">
                <div class="data-area">

                <div class="input-element localkey" id="IncludeMultiCountyDistrictsLocalKey" runat="server">
                  <p class="fieldlabel">Local District to Include in this County<span class="reqd">◄</span></p>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlIncludeMultiCountyDistrictsLocalKey" 
                        CssClass="include-multi-county-districts-localkey-dropdown mc-nomonitor" runat="server"/>
                    </div>
                  </div>
                </div>

                </div>
                </div>
                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackIncludeMultiCountyDistricts" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderIncludeMultiCountyDistricts" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonIncludeMultiCountyDistricts" EnableViewState="false" 
                     runat="server" 
                      Text="Include" CssClass="update-button button-1 tiptip no-disable" 
                      Title="Include this district"
                      OnClick="ButtonIncludeMultiCountyDistricts_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
 
        <div id="tab-removemulticountydistricts" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelRemoveMultiCountyDistricts" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerRemoveMultiCountyDistricts" runat="server">
              <input id="RemoveMultiCountyDistrictsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <h4 class="center-element">Remove Multi-County Districts for This County</h4>
                  </div>
                </div>
                 <div class="instructions heading-instructions">This setting is only for districts that are marked 
                  <em>Include this district in entire county</em> in the Tiger Settings, and will control in which 
                  counties the district displays on ballots. For all other districts, the shapefiles 
                  determine the district visibility on ballots, Admin panels and reports.</div>
               <hr/>
                <div class="content-area">
                <div class="data-area">

                <div class="input-element localkey" id="RemoveMultiCountyDistrictsLocalKey" runat="server">
                  <p class="fieldlabel">Local District to Remove from this County<span class="reqd">◄</span></p>
                  <div class="databox dropdown">
                    <div class="shadow-2">
                      <asp:DropDownList ID="ControlRemoveMultiCountyDistrictsLocalKey" 
                        CssClass="remove-multi-county-districts-localkey-dropdown mc-nomonitor" runat="server"/>
                    </div>
                  </div>
                </div>

                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackRemoveMultiCountyDistricts" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderRemoveMultiCountyDistricts" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonRemoveMultiCountyDistricts" EnableViewState="false" 
                     runat="server" 
                      Text="Remove" CssClass="update-button button-1 tiptip no-disable" 
                      Title="Remove this district"
                      OnClick="ButtonRemoveMultiCountyDistricts_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

        <div id="tab-setupcountysupervisors" class="main-tab content-panel tab-panel htab-panel setup-tab">
          <asp:UpdatePanel ID="UpdateSetupCountySupervisors" 
                           UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerSetupCountySupervisors" runat="server">
                <input id="SetupCountySupervisorsReloading" class="reloading" type="hidden" runat="server" />
                <div class="inner-panel rounded-border">
                  <div class="tab-panel-heading horz-center">
                    <div class="center-inner">
                      <h4 class="center-element">Setup County Supervisors for This County</h4>
                    </div>
                  </div>
                  <div class="instructions heading-instructions">Use this panel to set up county supervisor districts that will
                    appear in the <em>Assign County Supervisor District</em> dropdowns. The primary purpose of the entries in this
                    panel is to connect county supervisor districts to the entries in the county supervisors shapefile. A county
                    supervisor district needs <em>both</em> an entry on this panel <em>and</em> a local districts entry that connects
                    to the entry in this panel.</div>
                  <hr/>
                  <div class="content-area">
                    <div class="data-area">
                      <input id="ControlSetupCountySupervisorsSupervisorsListValue" type="hidden" runat="server"/>
                      <div id="ControlSetupCountySupervisorsSupervisorsList" runat="server" class="supervisors-list value-in-hidden-field">
                        <table id="county-supervisors-table" class="setup-table">
                          <thead>
                          <tr>
                            <th class="delete">&nbsp;<br/>Delete</th>
                            <th class="exists">Local&nbsp;District<br/>Exists</th>
                            <th class="create">Auto&nbsp;Create<br/>Local&nbsp;District</th>
                            <th class="code">County&nbsp;Supervisor<br/>Shapefile&nbsp;Id</th>
                            <th class="in-shapefile">Is&nbsp;In<br/>Shapefile</th>
                            <th class="name">&nbsp;<br/>District&nbsp; Name</th>
                          </tr>
                          </thead>
                          <tbody id="CountySupervisorsTableBody" runat="server"></tbody>
                        </table>
                      </div>
                    </div>
                    <div class="buttons">
                      <input type="button" value="Add County Supervisor District" class="button-1 button-smaller add-supervisor-button"/>
                      <input type="button" value="Bulk Add County Supervisor Districts" id="BulkAddCountySupervisors" runat="server" class="button-1 button-smaller bulk-add-supervisors-button"/>
                    </div>
                  </div>

                  <hr />
                  <div class="content-footer">
                    <div class="feedback-floater-for-ie7">
                      <user:FeedbackContainer ID="FeedbackSetupCountySupervisors" 
                                              EnableViewState="false" runat="server" />
                    </div>
                    <div class="footer-item footer-ajax-loader">
                      <asp:Image ID="AjaxLoaderSetupCountySupervisors" 
                                 EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                                 CssClass="ajax-loader" runat="server" />
                    </div>
                    <div class="update-button">
                      <asp:Button ID="ButtonSetupCountySupervisors" EnableViewState="false" 
                                  runat="server" 
                                  Text="Update" CssClass="update-button button-1 tiptip" 
                                  Title="Update County Supervisors"
                                  OnClick="ButtonSetupCountySupervisors_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   

                </div>
              </div>

            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
  
  <% } %>
        
  <% if (IsTigerPlace)
      { %>

        <div id="tab-setupcitycouncil" class="main-tab content-panel tab-panel htab-panel setup-tab">
          <asp:UpdatePanel ID="UpdateSetupCityCouncil" 
                           UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerSetupCityCouncil" runat="server">
                <input id="SetupCityCouncilReloading" class="reloading" type="hidden" runat="server" />
                <div class="inner-panel rounded-border">
                  <div class="tab-panel-heading horz-center">
                    <div class="center-inner">
                      <h4 class="center-element">Setup City Council for This City</h4>
                    </div>
                  </div>
                  <div class="instructions heading-instructions">Use this panel to set up city council districts that will
                    appear in the <em>Assign City Council District</em> dropdowns. The primary purpose of the entries in this
                    panel is to connect city council districts to the entries in the city council shapefile. A city
                    council district needs <em>both</em> an entry on this panel <em>and</em> a local districts entry that connects
                    to the entry in this panel.</div>
                  <hr/>
                  <div class="content-area">
                    <div class="input-element prefix">
                      <p class="fieldlabel">City Council Prefix</p>
                      <asp:TextBox id="CityCouncilPrefix" runat="server" disabled="true" class="city-council-prefix" />
                      <input type="hidden" id="PlaceName" runat="server" class="city-council-place-name" />
                      <p class="fieldlabel sub">The <em>Prefix</em> is an arbitrary two character code automatically assigned to each city 
                        in the state that has a city council. It is unique within the state and is used as the first two characters 
                        of the Shapefile Id.</p>
                    </div>
                    <hr/>
                    <div class="data-area">
                      <input id="ControlSetupCityCouncilCouncilListValue" type="hidden" runat="server"/>
                      <div id="ControlSetupCityCouncilCouncilList" runat="server" class="council-list value-in-hidden-field">
                        <table id="city-council-table" class="setup-table">
                          <thead>
                          <tr>
                            <th class="delete">&nbsp;<br/>Delete</th>
                            <th class="exists">Local&nbsp;District<br/>Exists</th>
                            <th class="create">Auto&nbsp;Create<br/>Local&nbsp;District</th>
                            <th class="code">City&nbsp;Council<br/>Shapefile&nbsp;Id</th>
                            <th class="in-shapefile">Is&nbsp;In<br/>Shapefile</th>
                            <th class="district">&nbsp;<br/>District</th>
                            <th class="name">&nbsp;<br/>Full&nbsp;District&nbsp;Name</th>
                          </tr>
                          </thead>
                          <tbody id="CityCouncilTableBody" runat="server"></tbody>
                        </table>
                      </div>
                    </div>
                    <div class="buttons">
                      <input type="button" value="Add City Council District" class="button-1 button-smaller add-council-button"/>
                      <input type="button" value="Bulk Add City Council Districts" id="BulkAddCityCouncil" runat="server" class="button-1 button-smaller bulk-add-council-button"/>
                    </div>
                  </div>

                  <hr />
                  <div class="content-footer">
                    <div class="feedback-floater-for-ie7">
                      <user:FeedbackContainer ID="FeedbackSetupCityCouncil" 
                                              EnableViewState="false" runat="server" />
                    </div>
                    <div class="footer-item footer-ajax-loader">
                      <asp:Image ID="AjaxLoaderSetupCityCouncil" 
                                 EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                                 CssClass="ajax-loader" runat="server" />
                    </div>
                    <div class="update-button">
                      <asp:Button ID="ButtonSetupCityCouncil" EnableViewState="false" 
                                  runat="server" 
                                  Text="Update" CssClass="update-button button-1 tiptip" 
                                  Title="Update City Council"
                                  OnClick="ButtonSetupCityCouncil_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   

                </div>
              </div>

            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

  <% } %>

  <% if (IsTigerSchool)
      { %>

        <div id="tab-setupschooldistrict" class="main-tab content-panel tab-panel htab-panel setup-tab">
          <asp:UpdatePanel ID="UpdateSetupSchoolDistrict" 
                           UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerSetupSchoolDistrict" runat="server">
                <input id="SetupSchoolDistrictReloading" class="reloading" type="hidden" runat="server" />
                <div class="inner-panel rounded-border">
                  <div class="tab-panel-heading horz-center">
                    <div class="center-inner">
                      <h4 class="center-element">Setup Sub-Districts for This School District</h4>
                    </div>
                  </div>
                  <div class="instructions heading-instructions">Use this panel to set up sub-districts that will
                    appear in the <em>Assign School District</em> dropdowns. Sub-districts are divisions of a school 
                    district for the purposes of voting and representation. Both the entire school district and the 
                    sub-districts will appear in the dropdowns. The primary purpose of the entries in this panel is 
                    to connect school sub-districts to the entries in the school sub-districts shapefile. A school 
                    sub-district needs <em>both</em> an entry on this panel <em>and</em> a local districts entry 
                    that connects to the entry in this panel.</div>
                  <hr/>
                  <div class="content-area">
                    <div class="input-element prefix">
                      <p class="fieldlabel">School District Prefix</p>
                      <asp:TextBox id="SchoolDistrictPrefix" runat="server" disabled="true" class="school-district-prefix" />
                      <input type="hidden" id="SchoolName" runat="server" class="school-district-name" />
                      <p class="fieldlabel sub">The <em>Prefix</em> is an arbitrary three character code automatically assigned to each school district
                        in the state that has sub-districts. It is unique within the state and is used as the first three characters 
                        of the Shapefile Id.</p>
                    </div>
                    <hr />
                    <div class="data-area">
                      <input id="ControlSetupSchoolDistrictSubDistrictListValue" type="hidden" runat="server"/>
                      <div id="ControlSetupSchoolDistrictSubDistrictList" runat="server" class="subdistrict-list value-in-hidden-field">
                        <table id="school-district-table" class="setup-table">
                          <thead>
                          <tr>
                            <th class="delete">&nbsp;<br/>Delete</th>
                            <th class="exists">Local&nbsp;District<br/>Exists</th>
                            <th class="create">Auto&nbsp;Create<br/>Local&nbsp;District</th>
                            <th class="code">School&nbsp;District<br/>Shapefile&nbsp;Id</th>
                            <th class="in-shapefile">Is&nbsp;In<br/>Shapefile</th>
                            <th class="name">&nbsp;<br/>Sub-District&nbsp; Name</th>
                          </tr>
                          </thead>
                          <tbody id="SchoolDistrictTableBody" runat="server"></tbody>
                        </table>
                      </div>
                    </div>
                    <div class="buttons">
                      <input type="button" value="Add School Sub-District" class="button-1 button-smaller add-subdistrict-button"/>
                      <input type="button" value="Bulk Add School Sub-Districts" id="BulkAddSchoolDistrict" runat="server" class="button-1 button-smaller bulk-add-subdistricts-button"/>
                    </div>
                  </div>

                  <hr />
                  <div class="content-footer">
                    <div class="feedback-floater-for-ie7">
                      <user:FeedbackContainer ID="FeedbackSetupSchoolDistrict" 
                                              EnableViewState="false" runat="server" />
                    </div>
                    <div class="footer-item footer-ajax-loader">
                      <asp:Image ID="AjaxLoaderSetupSchoolDistrict" 
                                 EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                                 CssClass="ajax-loader" runat="server" />
                    </div>
                    <div class="update-button">
                      <asp:Button ID="ButtonSetupSchoolDistrict" EnableViewState="false" 
                                  runat="server" 
                                  Text="Update" CssClass="update-button button-1 tiptip" 
                                  Title="Update School District"
                                  OnClick="ButtonSetupSchoolDistrict_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   

                </div>
              </div>

            </ContentTemplate>
          </asp:UpdatePanel>
        </div>

  <% } %>
        
  <% if (AdminPageLevel == AdminPageLevel.Local)
     { %>
        
        <div id="tab-tigersettings" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdateTigerSettings" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerTigerSettings" runat="server">
              <input id="TigerSettingsReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoTigerSettings" runat="server"></div>
                    </div>
                    <h4 class="center-element">Tiger Settings</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
                <div class="clear-both spacer"></div>

                      <div class="input-element entirecounty">
                        <div class="databox kalypto-checkbox">
                          <div class="kalypto-container">
                            <input id="ControlTigerSettingsEntireCounty" class="tiger-settings-entire-county kalypto" 
                            runat="server" type="checkbox" />
                          </div>
                          <div class="kalypto-checkbox-label">Include this district in entire county</div>
                          <div class="tab-ast" id="AsteriskTigerSettingsEntireCounty" EnableViewState="false" runat="server"></div>
                        </div>
                      </div>
                      
                      <div class="tiger-selection">
                      <div class="clear-both spacer"></div>
                        
                      <div class="instructions">Select <em>ONE</em> of the following. Items already assigned are disabled.</div>

                      <div class="input-element tiger tigerdistrict">
                        <p class="fieldlabel">Assign Tiger Place</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlTigerSettingsTigerPlace" 
                              runat="server"/>
                            <div class="tab-ast" id="AsteriskTigerSettingsTigerPlace" EnableViewState="false" runat="server"></div>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>

                      <div class="input-element tiger tigerdistrict">
                        <p class="fieldlabel">Assign Tiger District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlTigerSettingsTigerDistrict" 
                              runat="server"/>
                            <div class="tab-ast" id="AsteriskTigerSettingsTigerDistrict" EnableViewState="false" runat="server"></div>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>

                      <div class="input-element tiger schooldistrict">
                        <p class="fieldlabel">Assign School District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlTigerSettingsSchoolDistrict" runat="server"/>
                            <div class="tab-ast" id="AsteriskTigerSettingsSchoolDistrict" EnableViewState="false" runat="server"></div>
                         </div>
                        </div>
                      </div>
                        
                      <div class="clear-both spacer" ></div>

                      <div class="input-element tiger councildistrict">
                        <p class="fieldlabel">Assign City Council District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlTigerSettingsCouncilDistrict"  runat="server"/>
                            <div class="tab-ast" id="AsteriskTigerSettingsCouncilDistrict" EnableViewState="false" runat="server"></div>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>

                      <div class="input-element tiger supervisordistrict">
                        <p class="fieldlabel">Assign County Supervisor District</p>
                        <div class="databox dropdown">
                          <div class="shadow-2">
                            <asp:DropDownList ID="ControlTigerSettingsSupervisorsDistrict" runat="server"/>
                            <div class="tab-ast" id="AsteriskTigerSettingsSupervisorsDistrict" EnableViewState="false" runat="server"></div>
                          </div>
                        </div>
                      </div>

                      <div class="clear-both spacer"></div>
               
                </div>
                </div>

              </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackTigerSettings" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderTigerSettings" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonTigerSettings" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the primary voter information"
                      OnClick="ButtonTigerSettings_OnClick" /> 
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

  <asp:UpdatePanel ID="NavigateJurisdictionUpdatePanel" UpdateMode="Conditional" runat="server">
     <ContentTemplate>
-      <user:NavigateJurisdiction ID="NavigateJurisdiction" runat="server" AdminPageName="UpdateJurisdictions" />
     </ContentTemplate>
   </asp:UpdatePanel>
  
  <div id="bulk-add-supervisors-dialog" class="hidden bulk-add-dialog">
    <div class="content">
      <div>
        <div class="rounded-border boxed-group supervisor-type-group">
          <div class="boxed-group-label">Supervisor Type</div>
          <select class="supervisor-type-dropdown selection">
            <option value="County Supervisor">County Supervisor</option>
            <option value="County Council">County Council</option>
            <option value="County Commission">County Commission</option>
            <option value="County Commissioner">County Commissioner</option>
            <option value="County Legislature">County Legislature</option>
            <option value="Freeholder">Freeholder</option>
            <option value="other">&lt;other&gt;</option>
          </select>
          <input type="text" disabled="disabled" placeholder="&lt;other&gt;" class="supervisor-other-type selection"/>
        </div>
        <div class="rounded-border boxed-group supervisor-name-group">
          <div class="boxed-group-label">District Type</div>
          <select class="supervisor-name-dropdown selection">
            <option value="District #">District #</option>
            <option value="Precinct #">Precinct #</option>
            <option value="Post #">Post #</option>
            <option value="other">&lt;other&gt;</option>
          </select>
          <input type="text" disabled="disabled" placeholder="&lt;other&gt;" class="supervisor-other-name selection"/>
        </div>
        <div class="rounded-border boxed-group supervisors-number-group">
          <div class="boxed-group-label">Number to Create</div>
          <input type="text" class="supervisors-number-to-create selection"/>
          <input type="checkbox" id="supervisors-include-at-large" class="supervisors-include-at-large selection"/>
          <label for="supervisors-include-at-large">Include an at-large district</label>
        </div>
      </div> 
      <div class="sample-container">
        <label>Sample District Name</label>
        <input type="text" class="sample" disabled="disabled"/>
      </div>
      <input type="button" value="Create Districts" runat="server" class="button-1 button-smaller create-supervisors-button"/>
    </div>
  </div>
  
<div id="bulk-add-council-dialog" class="hidden bulk-add-dialog narrow">
  <div class="content">
    <div>
      <div class="rounded-border boxed-group council-name-group">
        <div class="boxed-group-label">District Type</div>
        <select class="council-name-dropdown selection">
          <option value="District #">District #</option>
          <option value="Precinct #">Precinct #</option>
          <option value="Ward #">Ward #</option>
          <option value="Section #">Section #</option>
          <option value="Place #">Place #</option>
          <option value="Position #">Position #</option>
          <option value="other">&lt;other&gt;</option>
        </select>
        <input type="text" disabled="disabled" placeholder="&lt;other&gt;" class="council-other-name selection"/>
      </div>
      <div class="rounded-border boxed-group council-number-group">
        <div class="boxed-group-label">Number to Create</div>
        <input type="text" class="council-number-to-create selection"/>
        <input type="checkbox" id="council-include-at-large" class="council-include-at-large selection"/>
        <label for="council-include-at-large">Include an at-large district</label>
      </div>
    </div> 
    <div class="sample-container">
      <label>Sample District Name</label>
      <input type="text" class="sample" disabled="disabled"/>
    </div>
    <input type="button" value="Create Districts" runat="server" class="button-1 button-smaller create-council-button"/>
  </div>
</div>
  
<div id="bulk-add-subdistrict-dialog" class="hidden bulk-add-dialog narrow">
  <div class="content">
    <div>
      <div class="rounded-border boxed-group subdistrict-name-group">
        <div class="boxed-group-label">District Type</div>
        <select class="subdistrict-name-dropdown selection">
          <option value="#">#</option>
          <option value="District #">District #</option>
          <option value="District Area #">District Area #</option>
          <option value="District Trustee Area #">District Trustee Area #</option>
          <option value="Trustee Area #">Trustee Area #</option>
          <option value="Post #">Post #</option>
          <option value="Place #">Place #</option>
          <option value="other">&lt;other&gt;</option>
        </select>
        <input type="text" disabled="disabled" placeholder="&lt;other&gt;" class="subdistrict-other-name selection"/>
      </div>
      <div class="rounded-border boxed-group subdistrict-number-group">
        <div class="boxed-group-label">Number to Create</div>
        <input type="text" class="subdistrict-number-to-create selection"/>
        <input type="checkbox" id="subdistrict-include-at-large" class="subdistrict-include-at-large selection"/>
        <label for="subdistrict-include-at-large">Include an at-large district</label>
      </div>
    </div> 
    <div class="sample-container">
      <label>Sample District Name</label>
      <input type="text" class="sample" disabled="disabled"/>
    </div>
    <input type="button" value="Create Districts" runat="server" class="button-1 button-smaller create-subdistricts-button"/>
  </div>
</div>

</asp:Content>
