<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="UpdateJurisdictions.aspx.cs" 
Inherits="Vote.Admin.UpdateJurisdictionsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/NavigateJurisdiction.ascx" TagName="NavigateJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>
<%--@ Register Src="/Controls/EmailTemplateOpenDialog.ascx" TagName="EmailTemplateOpenDialog" TagPrefix="user" --%>
<%--@ Register Src="/Controls/EmailTemplateSaveAsDialog.ascx" TagName="EmailTemplateSaveAsDialog" TagPrefix="user" --%>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <script type="text/javascript">
  //<![CDATA[
  <asp:Literal ID="AvailableSubstitutionsLiteral" runat="server"></asp:Literal>
  //]]>
  </script>
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
      <h4 id="CredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
    </div>
    <div class="sub-heading change-button">
      <input id="ChangeJurisdictionButton" runat="server" type="button" 
          value="Change Jurisdiction" class="jurisdiction-change-button button-1 button-smaller tiptip"
          title="Update the offices for a different state, county or local jurisdiction -- subject to your sign-in credentials."/>
    </div>

    <div class="clear-both"></div>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server" id="TabGeneralVoterInfo"><a href="#tab-generalvoterinfo" onclick="this.blur()" id="TabGeneralVoterInformation" EnableViewState="false" runat="server">General Voter<br />Information</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabVoterUrls"><a href="#tab-voterurls" onclick="this.blur()" id="TabStateVoterUrls" EnableViewState="false" runat="server">State Voter<br />URLs</a></li>
          <%--<li class="tab htab" EnableViewState="false" runat="server" id="TabPrimaryVoterInfo"><a href="#tab-primaryvoterinfo" onclick="this.blur()" id="TabPrimaryVoterInformation" EnableViewState="false" runat="server">Voter Information<br />for Primaries</a></li>--%>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-ballot" onclick="this.blur()" id="TabBallot" EnableViewState="false" runat="server">Ballot<br />Settings</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-electionauthority" onclick="this.blur()" id="TabElectionAuthority" EnableViewState="false" runat="server">Election<br />Authority</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabViewReportItem"><a href="#tab-viewreports" onclick="this.blur()" id="TabViewReports" EnableViewState="false" runat="server">View<br />Reports</a></li>
          <%-- <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail" onclick="this.blur()" id="TabBulkEmails" EnableViewState="false" runat="server">Bulk<br />Email</a></li>--%>
          <%-- <li class="tab htab" EnableViewState="false" runat="server" id="TabMasterItem"><a href="#tab-masteronly" onclick="this.blur()" id="TabMaster" EnableViewState="false" runat="server">Master<br />Functions</a></li>--%>
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
                <div class="instructions">This information is shown in the <em>Voting Information</em> accordion on ballots. Both the General and Primary entries should explain the voter identification requirements. For Primary Elections an explanation also should be provided regarding the various party ballots and the requirements to vote in each of them.</div>
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
               
                    <%--
                    <div class="input-element countyboardswebaddress">
                      <p class="fieldlabel">County Boards Web Address</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlGeneralVoterInfoCountyBoardsWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false"
                        title="Used as a link for the &ldquo;We suggest that you consult your local county board&rsquo;s website for sample ballots which better represent what you will encounter on election day.&rdquo; disclaimer" />
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoCountyBoardsWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                    
                    <div class="input-element hasearlyvoting">
                      <div class="databox kalypto-checkbox">
                        <div class="kalypto-container">
                          <input id="ControlGeneralVoterInfoHasEarlyVoting" class="kalypto" 
                            runat="server" type="checkbox" />
                        </div>
                        <div class="kalypto-checkbox-label">Has early voting</div>
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoHasEarlyVoting" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                  
                    <div class="input-element hasvotingbymail">
                      <div class="databox kalypto-checkbox">
                        <div class="kalypto-container">
                          <input id="ControlGeneralVoterInfoHasVotingByMail" class="kalypto" 
                            runat="server" type="checkbox" />
                        </div>
                        <div class="kalypto-checkbox-label">Has voting by mail</div>
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoHasVotingByMail" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                 
                    <div class="input-element votingexclusivelybymail">
                      <div class="databox kalypto-checkbox">
                        <div class="kalypto-container">
                          <input id="ControlGeneralVoterInfoVotingExclusivelyByMail" class="kalypto" 
                            runat="server" type="checkbox" />
                        </div>
                        <div class="kalypto-checkbox-label">Voting is exclusively by mail</div>
                        <div class="tab-ast" id="AsteriskGeneralVoterInfoVotingExclusivelyByMail" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both"></div>
                    --%>
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
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the voter registration information, if available (http:// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoterRegistrationWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element earlyvotingwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsEarlyVotingWebAddress" runat="server">Early Voting URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsEarlyVotingWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the early voting information, if available (http:// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsEarlyVotingWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element votebymailwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsVoteByMailWebAddress" runat="server">Vote by Mail URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsVoteByMailWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the vote by mail information, if available (http:// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoteByMailWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element votebyabsenteeballotwebaddress webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsVoteByAbsenteeBallotWebAddress" runat="server">Vote by Absentee Ballot URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlVoterUrlsVoteByAbsenteeBallotWebAddress" 
                        EnableViewState="false"  
                        CssClass="shadow-2 tiptip" runat="server" spellcheck="false" title="Enter the url of a page with the vote by absentee ballots information, if available (http:// optional)" />
                        <div class="tab-ast" id="AsteriskVoterUrlsVoteByAbsenteeBallotWebAddress" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element pollhoursurl webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsPollHoursUrl" runat="server">Polling Hours URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlVoterUrlsPollHoursUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of a page with the polling hours information, if available (http:// optional)" runat="server"/>
                        <div class="tab-ast" id="AsteriskVoterUrlsPollHoursUrl" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>
                    <div class="clear-both spacer"></div>

                    <div class="input-element pollplacesurl webaddress">
                      <p class="fieldlabel"><span id="LabelVoterUrlsPollPlacesUrl" runat="server">Polling Places URL</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlVoterUrlsPollPlacesUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of a page with polling location information, if available (http:// optional)" runat="server"/>
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
        
        <%--
        <div id="tab-primaryvoterinfo" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelPrimaryVoterInfo" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerPrimaryVoterInfo" runat="server">
              <input id="PrimaryVoterInfoReloading" class="reloading" type="hidden" runat="server" />
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                      <div id="UndoPrimaryVoterInfo" runat="server"></div>
                    </div>
                    <h4 class="center-element">Enter or Change Voter Information for Primaries</h4>
                  </div>
                </div>
                <hr/>
                <div class="content-area">
                <div class="data-area">
                    
                    <div class="input-element howprimariesaredone">
                      <p class="fieldlabel">How Primaries are conducted</p>
                      <p class="fieldlabel sub">Include an explanation of the type of primary: closed, open or nonpartisan.</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlPrimaryVoterInfoHowPrimariesAreDone" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskPrimaryVoterInfoHowPrimariesAreDone" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                  
                    <div class="input-element stateprimarieshaveseparatepartyballots">
                      <div class="databox kalypto-checkbox">
                        <div class="kalypto-container">
                          <input id="ControlPrimaryVoterInfoStatePrimariesHaveSeparatePartyBallots" class="kalypto" 
                            runat="server" type="checkbox" />
                        </div>
                        <div class="kalypto-checkbox-label">State primaries have separate ballots for each party</div>
                        <div class="tab-ast" id="AsteriskPrimaryVoterInfoStatePrimariesHaveSeparatePartyBallots" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                  
                    <div class="input-element presidentialprimarieshaveseparatepartyballots">
                      <div class="databox kalypto-checkbox">
                        <div class="kalypto-container">
                          <input id="ControlPrimaryVoterInfoPresidentialPrimariesHaveSeparatePartyBallots" class="kalypto" 
                            runat="server" type="checkbox" />
                        </div>
                        <div class="kalypto-checkbox-label">Presidential primaries have separate ballots for each party</div>
                        <div class="tab-ast" id="AsteriskPrimaryVoterInfoPresidentialPrimariesHaveSeparatePartyBallots" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>
                    <div class="clear-both spacer"></div>
                
                </div>
                </div>

                <hr />
                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackPrimaryVoterInfo" 
                    EnableViewState="false" runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderPrimaryVoterInfo" 
                    EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonPrimaryVoterInfo" EnableViewState="false" 
                     runat="server" 
                      Text="Update" CssClass="update-button button-1 tiptip" 
                      Title="Update the primary voter information"
                      OnClick="ButtonPrimaryVoterInfo_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   

              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        --%>
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
                          CssClass="shadow-2 tiptip" title="Enter name jurisdiction" runat="server"/>
                        <div class="tab-ast" id="AsteriskElectionAuthorityJurisdictionName" EnableViewState="false" runat="server"></div>
                      </div>                
                    </div>

                    <div class="input-element url">
                      <p class="fieldlabel"><span id="LabelElectionAuthorityUrl" runat="server">Web Address</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="ControlElectionAuthorityUrl"  
                          CssClass="shadow-2 tiptip" title="Enter the url of the election authority web site (http:// optional)" runat="server"/>
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
<%-- 
        <div id="tab-bulkemail" class="main-tab content-panel tab-panel htab-panel">
          <div class="inner-panel">

            <div class="content-area">

            <div class="data-area">
                  
              <div id="bulk-email-tabs" class="jqueryui-tabs shadow">
                        
                <ul class="htabs unselectable">
                  <li class="edit-template tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-edittemplate" onclick="this.blur()" id="TabBulkEmailEditTemplate" EnableViewState="false" runat="server">Edit<br />Template</a><div class="tab-ast tiptip" title="There are unsaved changes to this template"></div></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-availablesubstitutions" onclick="this.blur()" id="TabBulkEmailAvailableSubstitutions" EnableViewState="false" runat="server">Available<br />Substitutions</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-selectrecipients" onclick="this.blur()" id="TabBulkEmailSelectRecipients" EnableViewState="false" runat="server">Select<br />Recipients</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-viewrecipients" onclick="this.blur()" id="TabBulkEmailViewRecipients" EnableViewState="false" runat="server">View<br />Recipients</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-previewsample" onclick="this.blur()" id="TabBulkEmailPreviewSample" EnableViewState="false" runat="server">Preview<br />Sample</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-emailoptions" onclick="this.blur()" id="TabBulkEmailEmailOptions" EnableViewState="false" runat="server">Email<br />Options</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-sendemails" onclick="this.blur()" id="TabBulkEmailSendEmails" EnableViewState="false" runat="server">Send<br />Emails</a></li>
                </ul>

                <div id="tab-bulkemail-edittemplate">
                   <p class="mainlabel template-name">Template Name: <span id="edittemplate-name">The template name</span></p>
                   <div class="code-edit-heading subject-edit-heading"><p class="mainlabel">Subject</p><div class="undo-redo"><div class="undo tiptip" title="Undo"></div><div class="redo tiptip" title="Redo"></div></div></div>
                   <div id="subject-editor" class="code-edit subject shadow"></div>
                   <div class="code-edit-heading body-edit-heading"><p class="mainlabel">Body</p><div class="undo-redo"><div class="undo tiptip" title="Undo"></div><div class="redo tiptip" title="Redo"></div></div></div>
                   <div id="body-editor" class="code-edit body shadow"></div>
                   <div class="buttons">
                      <input type="button" value="New" 
                        class="edit-template-new-button button-2 button-smallest"/>
                      <input type="button" value="Open..." 
                        class="edit-template-open-button button-2 button-smallest"/>
                      <input type="button" value="Save" 
                        class="edit-template-save-button button-2 button-smallest"/>
                      <input type="button" value="Save As..." 
                        class="edit-template-save-as-button button-2 button-smallest"/>
                   </div>
                </div>

                <div id="tab-bulkemail-availablesubstitutions">
                  <div class="instructions">
                    <ul>
                      <li><span>Double click a <em>substitution name</em> to add it to your template at the cursor position.</span></li>
                    </ul>
                  </div>
                  <asp:PlaceHolder ID="AvailableSubstitutionsPlaceHolder" runat="server"></asp:PlaceHolder>
                  <h3 class="conditionals">Conditionals</h3>
                  <div class="conditionals col1">
                    <p class="conditionals"><span class="escape">{{</span><span class="keyword">if</span> <span class="operator">&lt;condition&gt;</span> &lt;comparand&gt;<span class="escape">}}</span> &lt;test value&gt; <span class="escape">{{</span><span class="keyword">then</span><span class="escape">}}</span> &lt;true value&gt;</p>
                    <p class="conditionals"><span class="escape">{{</span><span class="keyword">elseif</span> <span class="operator">&lt;condition&gt;</span> &lt;comparand&gt;<span class="escape">}}</span> &lt;test value&gt; <span class="escape">{{</span><span class="keyword">then</span><span class="escape">}}</span>...</p>
                    <p class="conditionals"><span class="escape">{{</span><span class="keyword">else</span><span class="escape">}}</span> &lt;false value&gt;</p>
                    <p class="conditionals"><span class="escape">{{</span><span class="keyword">endif</span><span class="escape">}}</span></p>
                  </div>
                  <div class="conditionals col2">
                     <input type="button" value="Add conditional to template" 
                        class="insert-conditional-button button-2 button-smallest" />
                  </div>
                  <div class="conditionals col3">
                    <div class="elseif"><input readonly="readonly" class="spinner elseif-spinner" value="0"/><span class="label"><span class="keyword">elseif</span>s to include</span></div>
                    <div class="else input-element"><div><input type="checkbox" class="else-checkbox kalypto"/></div><div class="label">include <span class="keyword">else</span></div></div>
                  </div>
                  <div class="clear-both"></div>
                </div>

                <div id="tab-bulkemail-selectrecipients">
                  
                   <div class="instructions">
                    <ul>
                      <%  switch (UserSecurityClass)
                          {
                            case MasterSecurityClass:
                            { %>
                            <li><span>As a <em>Master User</em> you can send email 
                            to any state, county or local contacts.</span></li>

                            <li><span>If you select <em>State Contacts</em>, the 
                            default is to send an email to each state contact. 
                            Optionally, you can click the <em>Get States List</em> 
                            button and choose which states to send to.</span></li>

                            <li><span>If you select <em>County Contacts</em>, you 
                            can select all states or specfic states as above. 
                            An email will be sent to each county contact in the
                            selected states. If you select a single state, you
                            can click the <em>Get Counties List</em> button and
                            select specific counties within that state to target.</span></li>

                            <li><span>If you select <em>Local Contacts</em>, you 
                            can select states and counties as above. An email will 
                            be sent to each local district contact in the
                            selected states and counties. If you select a single 
                            county, you can click the <em>Get Local Districts 
                            List</em> button and select specific local 
                            districts within that county to target.</span></li>
                         <% break;
                            }
                            case StateAdminSecurityClass:
                            { %>
                            <li><span>As a <em>State Administrator</em> you can 
                            send email to your state contact or to any county or 
                            local contacts within your state.</span></li>

                            <li><span>If you select <em>State Contacts</em>, you can 
                            send a single email to your state contact.</span></li>

                            <li><span>If you select <em>County Contacts</em>, the 
                            default is to send an email to each county contact
                            in your state. Optionally, you can click the <em>Get 
                            Counties List</em> button and choose which counties 
                            to send to.</span></li>

                            <li><span>If you select <em>Local Contacts</em>, you 
                            can select all counties or specfic counties in your 
                            state as above. An email will be sent to each local 
                            contact in the selected counties. If you select a 
                            single county, you can click the <em>Get Local 
                            Districts List</em> button and select specific local 
                            districts within that county to target.</span></li>
                        <% break;
                            }
                            case CountyAdminSecurityClass:
                            { %>
                            <li><span>As a <em>County Administrator</em> you can 
                            send email to your county contact or to any local 
                            contacts within your county.</span></li>
 
                            <li><span>If you select <em>County Contacts</em>, 
                            you can send a single email to your county 
                            contact.</span></li>

                            <li><span>If you select <em>Local Contacts</em>, the 
                            default is to send an email to each local contact
                            in your county. Optionally, you can click the <em>Get 
                            Local Districts List</em> button and choose which 
                            local districts to send to.</span></li>
                       <% break;
                            }
                            case LocalAdminSecurityClass:
                            { %>
                            <li><span>As a <em>Local Administrator</em> you can 
                            only send a single email to your local district 
                            contact.</span></li>
                        <% break;
                            }
                            %>
                      <% } %>
                    </ul>
                  </div>
                 
                  <div class="col col1">
                    <div class="recipient-buttons rounded-border box">
                      <p class="mainlabel">Select Contact Type</p>
                      <div class="state-contacts">
                        <asp:RadioButton ID="BulkEmailRecipientsStateContact" runat="server" 
                          Text="State Contacts" Value="State"
                          Checked="True"
                          GroupName="BulkEmailRecipientsContactType"/>
                      </div>
                      <div class="county-contacts">
                        <asp:RadioButton ID="BulkEmailRecipientsCountyContact" runat="server" 
                          Text="County Contacts" Value="County"
                          GroupName="BulkEmailRecipientsContactType"/>
                      </div>
                      <div class="local-contacts">
                        <asp:RadioButton ID="BulkEmailRecipientsLocalContact" runat="server" 
                          Text="Local Contacts" Value="Local"
                          GroupName="BulkEmailRecipientsContactType"/>
                      </div>
                    </div>
                    <div class="contacts-buttons rounded-border box">
                      <p class="mainlabel">Contacts to Use</p>
                      <div>
                        <asp:RadioButton ID="BulkEmailRecipientsMainContacts" runat="server" 
                          Text="Use main contact" Value="Both" CssClass="use-main-contacts"
                          Checked="True"
                          GroupName="BulkEmailRecipientsContacts"/>
                      </div>
                      <div>
                        <asp:RadioButton ID="BulkEmailRecipientsBothContact" runat="server" 
                          Text="Use both contacts" Value="Main" CssClass="use-both-contacts"
                          GroupName="BulkEmailRecipientsContacts"/>
                      </div>
                    </div>
                  </div>
                  
                  <div id="BulkEmailRecipientStates" runat="server" class="col box jurisdiction states rounded-border">
                    <p class="mainlabel">States:</p>
                    <p id="BulkEmailRecipientsAllStates" runat="server" class="sublabel hidden"><input id="BulkEmailRecipientsAllStatesCheckbox" class="all" runat="server" type="checkbox"/>All States</p>
                    <p id="BulkEmailRecipientsSpecificState" runat="server" class="sublabel specific-state">Specific State</p>
                    <div id="BulkEmailRecipientsStatesList" runat="server" class="checkbox-list hidden">
                    </div>
                  </div>
                  
                  <div id="BulkEmailRecipientCounties" runat="server" class="col box jurisdiction counties rounded-border">
                    <p class="mainlabel">Counties:</p>
                    <p id="BulkEmailRecipientsAllCounties" runat="server" class="sublabel"><input id="BulkEmailRecipientsAllCountiesCheckbox" class="all" runat="server" type="checkbox" />All Counties</p>
                    <p id="BulkEmailRecipientsSpecificCounty" runat="server" class="sublabel specific-county hidden">Specific County</p>
                    <p id="BulkEmailRecipientsCountiesListButtonContainer" runat="server" class="get-list hidden">
                      <input id="BulkEmailRecipientsCountiesListButton" runat="server" type="button" value="Get Counties List"
                        class="get-list-button button-2 button-smallest"/>
                    </p>
                    <div id="BulkEmailRecipientsCountiesList" runat="server" class="checkbox-list hidden">
                    </div>
                  </div>
                  
                  <div id="BulkEmailRecipientLocals" runat="server" class="col box jurisdiction locals rounded-border disabled">
                    <p class="mainlabel">Local Districts:</p>
                    <p id="BulkEmailRecipientsAllLocals" runat="server" class="sublabel"><input id="BulkEmailRecipientsAllLocalsCheckbox" class="all" runat="server" type="checkbox" />All Local Districts</p>
                    <p id="BulkEmailRecipientsSpecificLocal" runat="server" class="sublabel specific-local hidden">Specific Local District</p>
                    <p id="BulkEmailRecipientsLocalsListButtonContainer" runat="server" class="get-list">
                      <input id="BulkEmailRecipientsLocalsListButton" runat="server" type="button" value="Get Local Districts List"
                        class="get-list-button button-2 button-smallest"/>
                    </p>
                    <div id="BulkEmailRecipientsLocalsList" runat="server" class="checkbox-list hidden">
                    </div>
                  </div>
                  
                  <div class="clear-both"></div>

                  <div class="get-recipients">
                    <input type="button" value="Create Recipient List" 
                      class="get-recipients-button button-1 button-smallest"/>
                  </div>

                </div>

                <div id="tab-bulkemail-viewrecipients">
                  <div class="current-list-results">
                    <p>No recipients have been selected yet. Use the <em>Select Recipients</em> tab to create a list of recipients.</p>
                  </div>
                </div>

                <div id="tab-bulkemail-previewsample">
                  <div class="cols col1">
                    <div class="button selected-recipient">
                      <asp:RadioButton ID="BulkEmailPreviewSampleSelectedRecipient" runat="server" 
                        Text="Use selected recipient from View Recipients tab"
                        Value="Selected"
                        CssClass="preview-source preview-selected-recipient"
                        GroupName="BulkEmailPreviewSampleSource"/>
                    </div>
                    <div class="button sample-keys">
                      <asp:RadioButton ID="BulkEmailPreviewSampleKeys" runat="server" 
                        Text="Use these values:" 
                        Value="Keys"
                        CssClass="preview-source preview-sample-keys-recipient"
                        GroupName="BulkEmailPreviewSampleSource" 
                        Checked="True"/>
                    </div>
                    <div class="options rounded-border">
                      <p class="label">State:</p>
                      <div class="shadow-2">
                        <asp:DropDownList ID="PreviewSampleStateDropDownList" CssClass="state-dropdown shadow-2" 
                        runat="server"/>
                      </div>
                      <p class="label">County:</p>
                      <div class="shadow-2">
                        <asp:DropDownList ID="PreviewSampleCountyDropDownList" CssClass="county-dropdown shadow-2" 
                        runat="server"/>
                      </div>
                      <p class="label">Local District:</p>
                      <div class="shadow-2">
                        <asp:DropDownList ID="PreviewSampleLocalDropDownList" CssClass="local-dropdown shadow-2" 
                        runat="server" />
                      </div>
                      <p class="label">Contact Name:</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="PreviewSampleContactTextBox"  
                          CssClass="shadow-2 contact-textbox" runat="server"/>
                      </div>                
                      <p class="label">Contact Email:</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="PreviewSampleContactEmailTextBox"  
                          CssClass="shadow-2 email-textbox" runat="server"/>
                      </div>                
                      <p class="label">Contact Title:</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="PreviewSampleContactTitleTextBox"  
                          CssClass="shadow-2 title-textbox" runat="server"/>
                      </div>                
                      <p class="label">Contact Phone:</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                          ID="PreviewSampleContactPhoneTextBox"  
                          CssClass="shadow-2 phone-textbox" runat="server"/>
                      </div>                
                    </div>
                    <div class="generate">
                       <input type="button" value="Generate Preview" 
                          class="generate-preview-button button-2 button-smallest"/>
                    </div>
                  </div>
                  <div class="cols col2">
                    
                    <p class="mainlabel"><em>Subject:</em></p>
                    <p class="sample-subject">&lt;none&gt;</p>
                  
                    <div id="bulk-email-previewtabs" class="jqueryui-tabs shadow">
                        
                      <ul class="htabs unselectable">
                        <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-preview-html" onclick="this.blur()" id="TabBulkEmailPreviewHtml" EnableViewState="false" runat="server">Body Html</a></li>
                        <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-bulkemail-preview-rendered" onclick="this.blur()" id="TabBulkEmailPreviewRendered" EnableViewState="false" runat="server">Rendered</a></li>
                      </ul>

                      <div id="tab-bulkemail-preview-html">
                        <div class="inner">
                          <div class="error rounded-border"></div>
                          <div id="preview-html" class="rounded-border"></div>
                        </div>
                        
                      </div>

                      <div id="tab-bulkemail-preview-rendered">
                        <div class="inner rounded-border"><iframe></iframe></div>
                      </div>

                    </div>

                  </div>
                </div>

                <div id="tab-bulkemail-emailoptions">

                  <div class="col email-list radio">
                    <p class="mainlabel">From:</p>
                    <div id="BulkEmailOptionsFrom" runat="server" class="scrolling-list email-list bulk-email-options-from">
                    </div>
                    <div>
                      <input type="button" value="Add" 
                        class="email-add-button button-2 button-smallest"/>
                      <input type="text"/>
                    </div>
                  </div>

                  <div class="col email-list">
                    <p class="mainlabel">CC:</p>
                    <div id="BulkEmailOptionsCC" runat="server" class="scrolling-list email-list bulk-email-options-cc">
                    </div>
                    <div>
                      <input type="button" value="Add" 
                        class="email-add-button button-2 button-smallest"/>
                      <input type="text"/>
                    </div>
                  </div>

                  <div class="col email-list">
                    <p class="mainlabel">BCC:</p>
                    <div id="BulkEmailOptionsBCC" runat="server" class="scrolling-list email-list bulk-email-options-bcc">
                    </div>
                    <div>
                      <input type="button" value="Add" 
                        class="email-add-button button-2 button-smallest"/>
                      <input type="text"/>
                    </div>
                  </div>

                  <div class="clear-both"></div>
                  <div class="attachments hidden">
                    <p class="mainlabel">Attachments:</p>
                    <div class="scrolling-list attachment-list">
                      <p>image1.jpg</p>
                      <p>info.docx</p>
                    </div>
                    <div class="fileinputs">
                      <div class="masker masker1"></div>
                      <div class="masker masker2"></div>
                      <input type="file" id="AttachmentFile" name="AttachmentFile" runat="server"
                       class="upload-file"/>
                      <input type="button" value="Browse" onclick="$('#<%=AttachmentFile.ClientID %>').click()"
                      class="button-2 browse-button" />
                    </div>
                  </div>

                </div>

                <div id="tab-bulkemail-sendemails">
                  <div class="test-email rounded-border">
                    <div class="col button">
                      <input type="button" value="Send Test Email" 
                        class="send-test-email-button button-1"/>
                    </div>
                    <div class="col radios">
                      <div>
                        <asp:RadioButton ID="BulkEmailSendTestPreview" runat="server" 
                          Text="Send single test email using the Preview Sample"
                          Value="Single"
                          Checked="True"
                          GroupName="BulkEmailSendTest" />
                      </div>
                      <div>
                        <asp:RadioButton ID="BulkEmailSendTestAll" runat="server" 
                          Text="Send all emails as a test"
                          Value="All"
                          GroupName="BulkEmailSendTest" />
                      </div>
                    </div>

                    <div class="col email-list">
                      <p class="mainlabel">Send Test To:</p>
                      <div id="BulkEmailTestAddress" runat="server" class="scrolling-list email-list bulk-email-options-test-to">
                      </div>
                      <div>
                        <input type="button" value="Add" 
                          class="email-add-button button-2 button-smallest"/>
                        <input type="text"/>
                      </div>
                    </div>

                    <div class="clear-both"></div>
                  </div>
                  <div class="all-email">
                    <input type="button" value="Send All Emails to Actual Recipients" 
                      class="send-all-emails-button button-1"/>
                  </div>
                </div>
              </div>
                  
            </div>
 
            </div>

          </div>
        </div>
        --%>
<%--
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

                <div class="data-area">
                  
                  <div id="master-only-tabs" class="jqueryui-tabs shadow">
                        
                    <ul class="htabs unselectable">
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-masteronly-placeholder" onclick="this.blur()" id="TabMasterOnlyPlaceHolder" EnableViewState="false" runat="server">Place<br />Holder</a></li>
                    </ul>

                    <div id="tab-masteronly-placeholder">
                      
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
  --%>
      </div>

    </div>

  </div>

  <user:NavigateJurisdiction ID="NavigateJurisdiction" runat="server" AdminPageName="UpdateJurisdictions" />

  <%--<user:EmailTemplateOpenDialog ID="XEmailTemplateOpenDialog" runat="server" />--%>

  <%--<user:EmailTemplateSaveAsDialog ID="EmailTemplateDialogs" runat="server" /> --%>

</asp:Content>
