<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="BulkEmail.aspx.cs" 
Inherits="Vote.Admin.BulkEmailPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/SelectJurisdictions.ascx" TagName="SelectJurisdictions" TagPrefix="user" %>
<%@ Register Src="/Controls/FindPolitician.ascx" TagName="FindPolitician" TagPrefix="user" %>
<%@ Register Src="/Controls/ElectionsOfficesCandidates.ascx" TagName="ElectionsOfficesCandidates" TagPrefix="user" %>
<%@ Register Src="/Controls/ElectionControl.ascx" TagName="ElectionControl" TagPrefix="user" %>
<%@ Register Src="/Controls/EmailTemplateDialogs.ascx" TagName="EmailTemplateDialogs" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
      <h4 id="CredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
    </div>

    <div class="clear-both"></div>

    <div id="UpdateControls" class="update-controls" runat="server">
        
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="edit-template tab htab" EnableViewState="false" runat="server"><a href="#tab-edittemplate" onclick="this.blur()" id="TabEditTemplate" EnableViewState="false" runat="server">Edit<br />Template</a><div class="tab-ast tiptip" title="There are unsaved changes to this template"></div></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-availablesubstitutions" onclick="this.blur()" id="TabAvailableSubstitutions" EnableViewState="false" runat="server">Available<br />Substitutions</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-selectrecipients" onclick="this.blur()" id="TabSelectRecipients" EnableViewState="false" runat="server">Select<br />Recipients</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-selectrecipientsinstructions" onclick="this.blur()" id="TabRecipientSelectionInstructions" EnableViewState="false" runat="server">Select Recipients<br />Instructions</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-viewrecipients" onclick="this.blur()" id="TabViewRecipients" EnableViewState="false" runat="server">View<br />Recipients</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-previewsample" onclick="this.blur()" id="TabPreviewSample" EnableViewState="false" runat="server">Preview<br />Sample</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emailoptions" onclick="this.blur()" id="TabEmailOptions" EnableViewState="false" runat="server">Email<br />Options</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-sendemails" onclick="this.blur()" id="TabSendEmails" EnableViewState="false" runat="server">Send<br />Emails</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging" onclick="this.blur()" id="TabEmailLogging" EnableViewState="false" runat="server">Email<br />Logging</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabMasterItem"><a href="#tab-masteronly" onclick="this.blur()" id="TabMaster" EnableViewState="false" runat="server">Master<br />Functions</a></li>
        </ul>
 
          <div id="tab-edittemplate" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
              <div class="code-edit-heading subject-edit-heading"><p class="mainlabel">Subject</p><div class="undo-redo"><div class="undo tiptip" title="Undo"></div><div class="redo tiptip" title="Redo"></div></div></div>
              <div id="subject-editor" class="code-edit subject shadow"></div>
              <div class="code-edit-heading body-edit-heading"><p class="mainlabel">Body</p><div class="undo-redo"><div class="undo tiptip" title="Undo"></div><div class="redo tiptip" title="Redo"></div></div></div>
              <div class="clear-both"></div>
              <div id="body-editor-tabs" class="jqueryui-tabs shadow">
                        
                <ul class="htabs unselectable">
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-body-html" onclick="this.blur()" id="TabBodyHtml" EnableViewState="false" runat="server">Body Html</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-body-rendered" onclick="this.blur()" id="TabBodyRendered" EnableViewState="false" runat="server">Rendered Without Substitutions</a></li>
                </ul>

                <div id="tab-body-html">
                  <div id="body-editor" class="code-edit body shadow"></div>
                </div>

                <div id="tab-body-rendered">
                  <div class="inner rounded-border"><iframe></iframe></div>
                </div>

              </div>

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
          </div>

          <div id="tab-availablesubstitutions" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <div class="inner">
                <div class="instructions">
                  <ul>
                    <li><span>Double click a <em>substitution name</em> to add it to your template at the cursor position.</span></li>
                  </ul>
                </div>
                <asp:PlaceHolder ID="AvailableSubstitutionsPlaceHolder" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="SubstitutionOptionsPlaceHolder" runat="server"></asp:PlaceHolder>
                <h3 class="conditionals">Conditionals</h3>
                <div class="conditionals col1">
                  <p class="conditionals"><span class="escape">{{</span><span class="keyword">if</span> <select class="if-condition"><option value="= ">= &lt;comparand&gt;</option><option value="!= ">!= &lt;comparand&gt;</option><option value="&gt; ">&gt; &lt;comparand&gt;</option><option value="&gt;= ">&gt;= &lt;comparand&gt;</option><option value="&lt; ">&lt; &lt;comparand&gt;</option><option value="&lt;= ">&lt;= &lt;comparand&gt;</option><option value="match ">match &lt;comparand&gt;</option><option value="notmatch ">notmatch &lt;comparand&gt;</option><option value="empty">empty</option><option value="notempty">notempty</option></select><span class="escape">}}</span> &lt;test value&gt; <span class="escape">{{</span><span class="keyword">then</span><span class="escape">}}</span> &lt;true value&gt;</p>
                  <p class="conditionals"><span class="escape">{{</span><span class="keyword">elseif</span> <select class="elseif-condition"><option value="= ">= &lt;comparand&gt;</option><option value="!= ">!= &lt;comparand&gt;</option><option value="&gt; ">&gt; &lt;comparand&gt;</option><option value="&gt;= ">&gt;= &lt;comparand&gt;</option><option value="&lt; ">&lt; &lt;comparand&gt;</option><option value="&lt;= ">&lt;= &lt;comparand&gt;</option><option value="match ">match &lt;comparand&gt;</option><option value="notmatch ">notmatch &lt;comparand&gt;</option><option value="empty">empty</option><option value="notempty">notempty</option></select><span class="escape">}}</span> &lt;test value&gt; <span class="escape">{{</span><span class="keyword">then</span><span class="escape">}}</span>...</p>
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
                <h3 class="conditionals">Fail Directive</h3>
                <div class="fail-directive">
                  <p class="conditionals fail-directive"><span class="escape">{{</span><span class="keyword">fail</span>  &lt;message&gt;<span class="escape">}}</span></p>
                  <p class="conditionals">Use this in a conditional after <span class="escape">{{</span><span class="keyword">then</span><span class="escape">}}</span> 
                  or <span class="escape">{{</span><span class="keyword">else</span><span class="escape">}}</span> 
                  to prevent sending an email if a critical piece of information is 
                  missing (for example, an election).</p>
                </div>
              </div>
            </div>
          </div>

          <div id="tab-selectrecipients" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
              <div class="upper">
                
                <div class="col col1 rounded-border boxed-group">
                  <div class="boxed-group-label">Contacts</div>
                  <div class="recipient-buttons rounded-border control-box">
                    <p class="mainlabel">Contact Type</p>
                    <div class="all-contacts">
                      <asp:RadioButton ID="RecipientsAllContact" runat="server" 
                        Text="All Admins" Value="All"
                        Checked="True" CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="state-contacts">
                      <asp:RadioButton ID="RecipientsStateContact" runat="server" 
                        Text="State Admins" Value="State"
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="county-contacts">
                      <asp:RadioButton ID="RecipientsCountyContact" runat="server" 
                        Text="County Admins" Value="County" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="local-contacts">
                      <asp:RadioButton ID="RecipientsLocalContact" runat="server" 
                        Text="Local Admins" Value="Local" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="all-candidates">
                      <asp:RadioButton ID="RecipientsAllCandidate" runat="server" 
                        Text="All Candidates" Value="AllCandidates"
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="state-candidates">
                      <asp:RadioButton ID="RecipientsStateCandidate" runat="server" 
                        Text="State Candidates" Value="StateCandidates" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="county-candidates">
                      <asp:RadioButton ID="RecipientsCountyCandidate" runat="server" 
                        Text="County Candidates" Value="CountyCandidates" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="local-candidates">
                      <asp:RadioButton ID="RecipientsLocalCandidate" runat="server" 
                        Text="Local Candidates" Value="LocalCandidates" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="party-officials">
                      <asp:RadioButton ID="RecipientsPartyOfficial" runat="server" 
                        Text="Party Officials" Value="PartyOfficials" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="volunteers">
                      <asp:RadioButton ID="RecipientsVolunteer" runat="server" 
                        Text="Volunteers" Value="Volunteers" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="website-visitors">
                      <asp:RadioButton ID="RecipientsWebsiteVisitor" runat="server" 
                        Text="Website Visitors" Value="WebsiteVisitors" 
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                    <div class="donors">
                      <asp:RadioButton ID="RecipientsDonors" runat="server" 
                        Text="Donors" Value="Donors"
                        CssClass="is-option-click"
                        GroupName="RecipientsContactType"/>
                    </div>
                  </div>
                  <div class="contacts-buttons rounded-border control-box">
                    <p class="mainlabel">Contacts to Use</p>
                    <div>
                      <asp:RadioButton ID="RecipientsMainContacts" runat="server" 
                        Text="Main contact only" Value="Main" 
                        CssClass="use-main-contacts is-option-click"
                        Checked="True"
                        GroupName="RecipientsContacts"/>
                    </div>
                    <div>
                      <asp:RadioButton ID="RecipientsBothContact" runat="server" 
                        Text="Both contacts" Value="Both" 
                        CssClass="use-both-contacts is-option-click"
                        GroupName="RecipientsContacts"/>
                    </div>
                  </div>
                  <div class="emails-checkboxes rounded-border control-box hidden">
                  <p class="mainlabel">Emails to Use</p>
                    <div>
                      <input type="checkbox" id="RecipientMainEmail" 
                      class="is-option-click"
                      checked="checked" value="EmailAddr"/>
                      <label for="RecipientMainEmail">Main email</label>
                    </div>
                    <div>
                      <input type="checkbox" id="RecipientCampaignEmail" 
                      class="is-option-click"
                      checked="checked" value="CampaignEmail"/>
                      <label for="RecipientMainEmail">Campaign email</label>
                    </div>
                    <div>
                      <input type="checkbox" id="RecipientStateEmail"  
                      class="is-option-click"
                      checked="checked" value="StateEmailAddr"/>
                      <label for="RecipientMainEmail">State email</label>
                   </div>
                    <div>
                      <input type="checkbox" id="RecipientVoteUsaEmail"  
                      class="is-option-click"
                      checked="checked" value="EmailAddrVoteUSA"/>
                      <label for="RecipientMainEmail">Vote-USA email</label>
                    </div>
                </div>
                </div>
                
                <div class="col col2">
                  
                <user:SelectJurisdictions ID="RecipientsSelectJurisdictions" runat="server" />
                   
                <div class="col rounded-border boxed-group district-options">
                  <div class="boxed-group-label">Districts</div>
                  
                    <div class="district-filtering col control-box rounded-border">
                      <div class="no-district-filtering">
                        <asp:RadioButton ID="VisitorNoDistrictFiltering" runat="server" 
                          Text="No district filtering" Value="NoFiltering"
                          Checked="True"
                          GroupName="VisitorsDistrictFiltering"/>
                      </div>
                      <div class="congressional-district-filtering">
                        <asp:RadioButton ID="VisitorsCongressionalDistrictFiltering" runat="server" 
                          Text="By Congressional District" Value="CongressionalDistrict"
                          GroupName="VisitorsDistrictFiltering"/>
                      </div>
                      <div class="state-senate-filtering">
                        <asp:RadioButton ID="VisitorsStateSenateFiltering" runat="server" 
                          Text="By State Senate District" Value="StateSenateDistrict"
                          GroupName="VisitorsDistrictFiltering"/>
                      </div>
                      <div class="state-house-filtering">
                        <asp:RadioButton ID="VisitorsStateHouseFiltering" runat="server" 
                          Text="By State House District" Value="StateHouseDistrict"
                          GroupName="VisitorsDistrictFiltering"/>
                      </div>
                      <div class="visitor-district-filtering-msg single-election-msg" id="VisitorDistrictFilteringMsg" runat="server">
                        Select a single state to enable district filtering.
                      </div>
                    <p id="DistrictsAll" class="sublabel"><input 
                      id="DistrictsAllCheckbox" class="all" disabled="True" 
                       checked="True" runat="server" type="checkbox"/><label for="DistrictsAllCheckbox">All Districts</label>
                    </p>
                    <div class="checkbox-list hidden">
                    </div>
                    </div>

                    <div class="clear-both"></div>
                
                  </div>
                  <div class="clear-both"></div>
              <div class="lower">
                
                <div class="col rounded-border boxed-group elections-options">
                  <div class="boxed-group-label">Elections</div>
                  
                    <div class="election-filtering col control-box rounded-border">
                      <p class="mainlabel">Election Filtering</p>
                      <div class="no-election-filtering">
                        <asp:RadioButton ID="RecipientsNoElectionFiltering" runat="server" 
                          Text="No election filtering" Value="NoFiltering"
                          CssClass = "is-option-click" Checked="True"
                          GroupName="RecipientsElectionFiltering"/>
                      </div>
                      <div class="has-election-type-filtering">
                        <asp:RadioButton ID="RecipientsHasElectionTypeFiltering" runat="server" 
                          Text="Has election of selected type(s)" Value="HasType"
                          CssClass = "is-option-click"
                          GroupName="RecipientsElectionFiltering"/>
                      </div>
                      <div class="hasnt-election-type-filtering">
                        <asp:RadioButton ID="RecipientsHasntElectionTypeFiltering" runat="server" 
                          Text="Does not have election of selected type(s)" 
                          Value="HasntType" CssClass = "is-option-click"
                          GroupName="RecipientsElectionFiltering"/>
                      </div>
                      <div class="single-election-filtering">
                        <asp:RadioButton ID="RecipientsSingleElectionFiltering" runat="server" 
                          Text="Single selected election" Value="Single"
                          CssClass = "is-option-click"
                          GroupName="RecipientsElectionFiltering"/>
                      </div>
                      <div class="single-election-filtering-msg single-election-msg" id="SingleElectionFilteringMsg" runat="server">
                        Select a single state to enable single election selection.
                      </div>
                    </div>
                  
                    <div class="col control-box rounded-border election-filter-type hidden">
                      <p class="mainlabel">Election Filter Types</p>
                      <div class="general-election-filtering">
                        <input type="checkbox" id="RecipientsGeneralElectionFiltering"
                          class = "is-option-click" value="G"/>
                        <label for="RecipientsGeneralElectionFiltering">General (Nov. even years)</label>
                      </div>
                      <div class="offyear-election-filtering">
                        <input type="checkbox" id="RecipientsOffYearElectionFiltering"
                          class = "is-option-click" value="O"/>
                        <label for="RecipientsOffYearElectionFiltering">Off-year (Nov. odd years)</label>
                      </div>
                      <div class="special-election-filtering">
                        <input type="checkbox" id="RecipientsSpecialElectionFiltering"
                          class = "is-option-click" value="S"/>
                        <label for="RecipientsSpecialElectionFiltering">Special election</label>
                      </div>
                      <div class="state-primary-election-filtering">
                        <input type="checkbox" id="RecipientsStatePrimaryElectionFiltering"
                          class = "is-option-click" value="P"/>
                        <label for="RecipientsStatePrimaryElectionFiltering">State primary</label>
                      </div>
                      <div class="presidential-primary-election-filtering">
                        <input type="checkbox" id="RecipientsPresidentialPrimaryElectionFiltering"
                          class = "is-option-click" value="B"/>
                        <label for="RecipientsPresidentialPrimaryElectionFiltering">Presidential primary</label>
                      </div>
                    </div>
                  
                    <div class="col control-box filter-options-box rounded-border election-filter-options hidden">
                      <p class="mainlabel">Election Filter Options</p>
                      <div class="future-election-type-filter">
                        <asp:RadioButton ID="RecipientsFutureElectionFiltering" runat="server" 
                          Text="Closest future election of selected type(s)" Value="Future"
                          CssClass="is-option-click" Checked="True"
                          GroupName="RecipientsElectionFilterOption"/>
                      </div>
                      <div class="past-election-type-filtering">
                        <asp:RadioButton ID="RecipientsPastElectionTypeFiltering" runat="server" 
                          Text="Most recent past election of selected type(s)" Value="Past"
                          CssClass="is-option-click"
                          GroupName="RecipientsElectionFilterOption"/>
                      </div>
                      <div class="viewable-election-type-filtering">
                        <asp:Checkbox ID="RecipientsViewableElectionTypeFiltering" runat="server"
                          CssClass="is-option-click" 
                          Text="Only select publicly viewable elections" Value="Viewable"/>
                      </div>
                    </div>
                   
                    <div class="col control-box select-election-box rounded-border hidden">
                      <p class="mainlabel">Select Single Election</p>
                      <div class="selected-election-desc">Selected election: <span class="name disabled">none</span></div>
                      <div class="include-all-primaries">
                        <asp:Checkbox ID="RecipientsIncludeAllPrimaries" runat="server" 
                          Text="Include all primaries on this date" Value="AllPrimaries"/>
                      </div>
                      
                      <div class="election-control-placeholder"></div>
               
                      <div class="election-control-outer">
                        <div class="election-control-container rounded-border shadow">
                          <a class="select-election-toggler tiptip" title="Show or hide the menu of elections">
                            <div class="toggler"></div>
                            <div class="election-control-heading rounded">Select Election</div>
                          </a>
                          <div class="election-control-slider">
                            <user:ElectionControl ID="ElectionControl" runat="server" />
                          </div>
                        </div>
                      </div>
                    </div>
                    
                    <div class="clear-both"></div>
                
                  </div>

                <div class="col rounded-border boxed-group visitor-type-options visitor-options">
                  <div class="boxed-group-label">Visitor Type</div>
                  <div class="control-box rounded-border visitor-types">
                    <div class="">
                      <input type="checkbox" id="RecipientsVisitorsSampleBallots"
                        class="is-option-click" value="SampleBallots"/>
                      <label for="RecipientsVisitorsSampleBallots">Requested Sample Ballots</label>
                    </div>
                    <div class="">
                      <input type="checkbox" id="RecipientsVisitorsSharedChoices"
                        class="is-option-click" value="SharedChoices"/>
                      <label for="RecipientsVisitorsSharedChoices">Shared Ballot Choices</label>
                    </div>
                    <div class="">
                      <input type="checkbox" id="RecipientsVisitorsDonated"
                        class="is-option-click" value="Donated"/>
                      <label for="RecipientsVisitorsDonated">Donated</label>
                    </div>
                  </div>
                </div>
                
                <div class="col rounded-border boxed-group visitor-donor-options visitor-options">
                  <div class="boxed-group-label">Website Visitor Options</div>
                  <div class="control-box rounded-border visitor-options-filter">
 		                <div class="col col1">
                      <div class="visitors-with-names exclusive-pair">
                        <input type="checkbox" id="RecipientsVisitorsWithNames"
                        class="is-option-click" value="WithNames"/>
                        <label for="RecipientsVisitorsWithNames">Only With Names</label>
                      </div>
                      <div class="visitors-without-names">
                        <input type="checkbox" id="RecipientsVisitorsWithoutNames"
                        class="is-option-click" value="WithoutNames"/>
                        <label for="RecipientsVisitorsWithoutNames">Only Without Names</label>
                      </div>
                      <div class="visitors-with-known-districts exclusive-pair">
                        <input type="checkbox" id="RecipientsVisitorsWithDistrictCoding"
                          class="is-option-click" value="WithDistrictCoding"/>
                        <label for="RecipientsVisitorsWithDistrictCoding">Only With District Coding</label>
                      </div>
                      <div class="visitors-with-unknown-districts">
                        <input type="checkbox" id="RecipientsVisitorsWithoutDistrictCoding"
                            class="is-option-click" value="WithoutDistrictCoding"/>
                        <label for="RecipientsVisitorsWithoutDistrictCoding">Only Without District Coding</label>
                      </div>
	                  </div>
                    
                    <div class="col col3">
                      <div class="date-range">
                        <p class="sublabel">Start Date</p>
                        <div><input class="start-date is-option-change" type="text"/></div>
                        <p class="sublabel">End Date</p>
                        <div><input class="end-date is-option-change" type="text"/></div>
                      </div>
                    </div>
                      
                    <div class="clear-both"></div>
                  </div>
                
                </div>
                
                <div class="col rounded-border boxed-group visitor-donor-options donor-options">
                  <div class="boxed-group-label">Donor Options</div>
                  
                    <!--<p class="msg">Use the Donors Contacts option to access the Donation information.</p>-->
                  
                    <div class="control-box rounded-border donor-options-filter">
                    
                      <div class="col col1">
                        <div class="date-range">
                          <p class="sublabel">Start Date</p>
                          <div><input class="start-date is-option-change" type="text"/></div>
                          <p class="sublabel">End Date</p>
                          <div><input class="end-date is-option-change" type="text"/></div>
                        </div>
                      </div>
                      
                      <div class="clear-both"></div>
                    </div>
                
                  </div>

              </div>
                </div>

                <div class="clear-both"></div>

              </div>
                    
              <div class="save-options">
                <input type="button" value="Save Options With Template" disabled="disabled" 
                  class="save-options-button button-2 button-smallest"/>
              </div>
                    
              <div class="get-recipients">
                <input type="button" value="Create Recipient List" 
                  class="get-recipients-button button-1 button-smallest"/>
              </div>
            
            </div>

          </div>

          <div id="tab-selectrecipientsinstructions" class="main-tab content-panel tab-panel htab-panel instructions-page">
            <div class="inner-panel rounded-border">
              <div class="inner">
              <p class="mainlabel"><em>How to Use the</em> Select Recipients <em>Tab</em></p>
              <p class="intro">Use the <em>Select Recipients</em> tab to create a list of recipients (a batch) for 
              a mailing. Once the recipients are selected, you can view them individually on the 
              <em>View Recipients</em> tab. There you can fine-tune the list by excluding selected 
              recipients.</p>
              <p class="intro">Each batch consists of a single <i>contact type</i> (see below). The contact type 
              determines which substitutions are available to be used in the email template. After you 
              select a batch, The <em>Available Substitutions</em> tab is updated to show which substitutions are 
              available, and the syntax highlighting on the Edit Template tab will show any unavailable 
              substitutions in red.</p>
              <p class="intro">Here are some tips that may be helpful to create a batch:</p>
              <div class="instructions">
                <ul>
                  <li>
                    <p><span>First, select a <em>Contact Type</em></span></p>
                    <p><span>There are five main types of contacts: <i>Election Admins</i>, <i>Candidates</i>, 
                    <i>Party Officials</i>, <i>Website Visitors</i> and <i>Donors</i> (not yet implemented). 
                    Election Admins and Candidates 
                    are broken down further into State, County and Local levels. The <em>Contact Type</em> 
                    selection controls which other selections are available for the batch.</span></p>
                  </li>
                  <li>
                    <p><span>Next, select <em>Jurisdictions</em></span></p>
                    <p><span>The jurisdictions selections let you filter the recipients by geographic 
                    location. The <em>States</em> checkbox list is available for all selection types. 
                    You can select any number of states, but some other selections are only 
                    available if a single state is selected (county selection, for example). 
                    For all of the checkbox lists, use the <i>All</i> checkbox at the top to quickly 
                    check or uncheck all entries.</span></p>
                    <p><span>A <em>Counties</em> checkbox list is available for County and Local Admins, 
                    County and Local Candidates, and Website Visitors. A <em>Local Districts</em> checkbox 
                    is available for Local Admins and Candidates.</span></p>
                  </li>
                  <li>
                    <p><span><em>Contacts to Use</em></span></p>
                    <p><span>For Election Admins, we maintain both a main and an alternate contact 
                    (although they may not both have data). You can select whether to include only 
                    the main contact, or to use both contacts, if available.</span></p>
                  </li>
                  <li>
                    <p><span><em>Election Filtering</em></span></p>
                    <p><span>Election Filtering allows the recipients to be restricted based on elections.</span></p>
                    <p><span><i>Has election of selected type(s)</i> is available for Election Admins and Candidates. 
                    For Admins, this limits recipients to those in jurisdictions with the selected 
                    election type. Candidates are limited to those who are running (or have run) in the 
                    selected type.</span></p>
                    <ul>
                      <li>
                        <p><span>Choose one or more of the five <i>Election Filter Types</i>.</span></p>
                      </li>
                      <li>
                        <p><span>Choose <i>Closest future election</i> or <i>Most recent past election</i> to 
                        look forward or back.</span></p>
                      </li>
                      <li>
                        <p><span>The selection can be limited to only publicly viewable elections.</span></p>
                      </li>
                    </ul>
                    <p><span><i>Does not have election of selected type(s)</i> is available for Admins only and 
                    operates similarly.</span></p>
                    <p><span>Single election selection is available for Candidates and Party Officials, 
                    but only if there is a single jurisdiction (state, county or local district) selected. 
                    You can used the <em>Select Election</em> control to choose a single election. If you 
                    choose a primary on a date with other primaries, you can choose to include all 
                    primaries on that date.</span></p>
                  </li>
                  <li>
                    <p><span><em>Emails to Use</em></span></p>
                    <p><span>For Candidates, we keep up to five different email addresses. The 
                    selections in this box let you choose which ones to include in the recipient 
                    list. Note that this could result in a candidate receiving more than one copy 
                    of the email if they have different email addresses. (Duplicates of the same 
                    email address are filtered out by recipient selection).</span></p>
                  </li>
                  <li>
                    <p><span><em>Parties</em></span></p>
                    <p><span>For Party Officials you can filter by specific parties (if a single 
                    state is selected).</span></p>
                  </li>
                  <li>
                    <p><span><em>Website Visitor Options</em></span></p>
                    <p><span>Website Visitors have a special set of selections available. There are 
                    four pairs of mutually exclusive choices: <i>With/Without Names</i>, <i>With/Without Addresses</i>, 
                    <i>With Appended/Entered Email</i> and <i>With/Without Known Districts</i>. 
                    For each pair you can select neither or either, but not both. You can also 
                    restrict recipients to a range of dates in which they registered.</span></p>
                  </li>
                </ul>
              </div>
              </div>
            </div>
          </div>

          <div id="tab-viewrecipients" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
              <div class="current-list-results list-results">
                <p>No recipients have been selected yet. Use the <em>Select Recipients</em> tab to create a list of recipients.</p>
              </div>
                     
              <div class="save-options">
                <input type="button" value="Save Options With Template" disabled="disabled"
                  class="save-options-button button-2 button-smallest"/>
              </div>
            </div>
          </div>

          <div id="tab-previewsample" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
              <div class="cols col1">
                <div class="button selected-recipient">
                  <asp:RadioButton ID="PreviewSampleSelectedRecipient" runat="server" 
                    Text="Use selected recipient from <em>View Recipients</em> tab"
                    Value="Selected"
                    CssClass="preview-source preview-selected-recipient"
                    GroupName="PreviewSampleSource"/>
                </div>
                <div class="button sample-keys">
                  <asp:RadioButton ID="PreviewSampleKeys" runat="server" 
                    Text="Use these values:" 
                    Value="Keys"
                    CssClass="preview-source preview-sample-keys-recipient"
                    GroupName="PreviewSampleSource" 
                    Checked="True"/>
                </div>
                <div class="options rounded-border">
                  <p class="label">State:</p>
                  <div class="databox shadow-2">
                    <asp:DropDownList ID="PreviewSampleStateDropDownList" CssClass="state-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <p class="label">County:</p>
                  <div class="databox shadow-2 tiptip" title="Select a state to populate this list">
                    <asp:DropDownList ID="PreviewSampleCountyDropDownList" CssClass="county-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <p class="label">Local District:</p>
                  <div class="databox shadow-2 tiptip" title="Select a county to populate this list">
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
                  <p class="label">Election:</p>
                  <div class="databox shadow-2 tiptip" title="Select a state, county and/or local district to populate this list">
                    <asp:DropDownList ID="PreviewSampleElectionDropDownList" CssClass="election-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <p class="label">Office:</p>
                  <div class="databox shadow-2 tiptip" title="Select an election to populate this list">
                    <asp:DropDownList ID="PreviewSampleOfficeDropDownList" CssClass="office-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <p class="label">Candidate:</p>
                  <div class="databox shadow-2 tiptip" title="Select an election and an office to populate this list">
                    <asp:DropDownList ID="PreviewSampleCandidateDropDownList" CssClass="candidate-dropdown shadow-2" 
                    runat="server"/>
                    <input type="hidden" class="politician-key-hidden"/>
                  </div>
                  <p class="label">Party:</p>
                  <div class="databox shadow-2 tiptip" title="Select a state to populate this list">
                    <asp:DropDownList ID="PreviewSamplePartyDropDownList" CssClass="party-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <p class="label">Party Email:</p>
                  <div class="databox shadow-2 tiptip" title="Select a party to populate this list">
                    <asp:DropDownList ID="PreviewSamplePartyEmailDropDownList" CssClass="party-email-dropdown shadow-2" 
                    runat="server"/>
                  </div>
                  <div class="overlay rounded-border"></div>
                </div>
              </div>
              <div class="cols col2">
                    
                <p class="mainlabel"><em>Subject:</em></p>
                <p class="sample-subject">&lt;none&gt;</p>
                  
                <div id="preview-sample-tabs" class="jqueryui-tabs shadow">
                        
                  <ul class="htabs unselectable">
                    <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-preview-html" onclick="this.blur()" id="TabPreviewHtml" EnableViewState="false" runat="server">Body Html</a></li>
                    <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-preview-rendered" onclick="this.blur()" id="TabPreviewRendered" EnableViewState="false" runat="server">Rendered</a></li>
                  </ul>

                  <div id="tab-preview-html">
                    <div class="inner">
                      <div class="error rounded-border"></div>
                      <div id="preview-html" class="rounded-border"></div>
                    </div>
                        
                  </div>

                  <div id="tab-preview-rendered">
                    <div class="inner rounded-border"><iframe></iframe></div>
                  </div>

                </div>

              </div>
              <div class="generate">
                  <input type="button" value="Generate Preview" 
                    class="generate-preview-button button-2 button-smallest"/>
              </div>
            </div>
          </div>

          <div id="tab-emailoptions" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
              <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
              <div class="col email-list radio">
                <p class="mainlabel">From:</p>
                <div id="OptionsFrom" runat="server" class="scrolling-list email-list bulk-email-options-from">
                </div>
                <div>
                  <input type="button" value="Add" 
                    class="email-add-button is-option button-2 button-smallest"/>
                  <input type="text"/>
                </div>
              </div>

              <div class="col email-list">
                <p class="mainlabel">CC:</p>
                <div id="OptionsCC" runat="server" class="scrolling-list email-list bulk-email-options-cc">
                </div>
                <div>
                  <input type="button" value="Add" 
                    class="email-add-button is-option button-2 button-smallest"/>
                  <input type="text"/>
                </div>
              </div>

              <div class="col email-list">
                <p class="mainlabel">BCC:</p>
                <div id="OptionsBCC" runat="server" class="scrolling-list email-list bulk-email-options-bcc">
                </div>
                <div>
                  <input type="button" value="Add" 
                    class="email-add-button is-option button-2 button-smallest"/>
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
                     
              <div class="save-options">
                <input type="button" value="Save Options With Template" disabled="disabled"
                  class="save-options-button button-2 button-smallest"/>
              </div>
           
            </div>

          </div>

          <div id="tab-sendemails" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel rounded-border">
             <p class="mainlabel template-name">Template Name: <span class="edittemplate-name">The template name</span></p>
             <div class="test-email email-panel batch-change-description rounded-border">
                <div class="col button">
                  <input type="button" value="Send Test Email" 
                    class="send-test-email-button button-1"/>
                </div>
                <div class="col radios">
                  <div>
                    <asp:RadioButton ID="SendTestPreview" runat="server" 
                      Text="Send single test email using the Preview Sample"
                      Value="Single"
                      Checked="True"
                      GroupName="SendTest" />
                  </div>
                  <div>
                    <asp:RadioButton ID="SendTestAll" runat="server" 
                      Text="Send all emails as a test"
                      Value="All"
                      GroupName="SendTest" />
                  </div>
                </div>

                <div class="col email-list">
                  <p class="mainlabel">Send Test To:</p>
                  <div id="TestAddress" runat="server" class="scrolling-list email-list bulk-email-options-test-to">
                  </div>
                  <div>
                    <input type="button" value="Add" 
                      class="email-add-button button-2 button-smallest"/>
                    <input type="text"/>
                  </div>
                </div>

                <div class="clear-both"></div>
              </div>

              <div class="all-email email-panel batch-change-description rounded-border">
               <div class="all-email-desc">
                <p class="mainlabel">Description of Email Batch for Logging</p>
                <textarea rows="4" cols="80"></textarea>
               </div>
               <input type="button" value="Send All Emails to Actual Recipients" 
                  class="send-all-emails-button button-1"/>
              </div>
            </div>
          </div>
 
          <div id="tab-emaillogging" class="main-tab content-panel tab-panel htab-panel">
            <div class="inner-panel">
                   
              <div id="email-logging-tabs" class="jqueryui-tabs shadow">
                         
                <ul class="htabs unselectable">
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-basicfiltering" onclick="this.blur()" id="TabEmailLoggingBasicFiltering" EnableViewState="false" runat="server">Basic<br />Filtering</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-morefiltering" onclick="this.blur()" id="TabEmailLoggingMoreFiltering" EnableViewState="false" runat="server">Election, Office and<br />Candidate Filtering</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-politicianfiltering" onclick="this.blur()" id="TabEmailLoggingPoliticianFiltering" EnableViewState="false" runat="server">Politician<br />Filtering</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-batchfiltering" onclick="this.blur()" id="TabEmailLoggingBatchFiltering" EnableViewState="false" runat="server">Batch<br />Filtering</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-logfilteringinstructions" onclick="this.blur()" id="TabLogFilteringInstructions" EnableViewState="false" runat="server">Log Filtering<br />Instructions</a></li>
                  <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-emaillogging-getlogentries" onclick="this.blur()" id="TabEmailLoggingGetLogEntries" EnableViewState="false" runat="server">Get Log<br />Entries</a></li>
                </ul>
 
                <div id="tab-emaillogging-basicfiltering">
                  
                  <div class="col col1">
                    <div class="rounded-border boxed-group level-box">
                      <div class="boxed-group-label">Level</div>
                        <div class="log-level-buttons rounded-border control-box">
                          <div class="state-log-level">
                            <asp:RadioButton ID="LogLevelState" runat="server" 
                              Text="State" Value="states"
                              Checked="True"
                              GroupName="LogLevelType"/>
                          </div>
                          <div class="county-log-level">
                            <asp:RadioButton ID="LogLevelCounty" runat="server" 
                              Text="County" Value="counties"
                              GroupName="LogLevelType"/>
                          </div>
                          <div class="local-log-level">
                            <asp:RadioButton ID="LogLevelLocal" runat="server" 
                              Text="Local" Value="locals"
                              GroupName="LogLevelType"/>
                          </div>
                      </div>
                    </div>
                    <div class="rounded-border boxed-group contact-types-box">
                      <div class="boxed-group-label">Contact Types</div>
                      <div class="contact-types rounded-border control-box">
                        <div>
                          <input type="checkbox" id="ContactTypeA" checked="checked" value="A"/>
                          <label for="ContactTypeA">Election Admins</label>
                        </div>
                        <div>
                          <input type="checkbox" id="ContactTypeC" checked="checked" value="C"/>
                          <label for="ContactTypeC">Candidates</label>
                        </div>
                        <div>
                          <input type="checkbox" id="ContactTypeP" checked="checked" value="P"/>
                          <label for="ContactTypeP">Party Officials</label>
                        </div>
                        <div>
                          <input type="checkbox" id="ContactTypeZ" checked="checked" value="Z"/>
                          <label for="ContactTypeZ">Volunteers</label>
                        </div>
                        <div>
                          <input type="checkbox" id="ContactTypeV" checked="checked" value="V"/>
                          <label for="ContactTypeV">Visitors</label>
                        </div>
                        <div>
                          <input type="checkbox" id="ContactTypeD" checked="checked" value="D"/>
                          <label for="ContactTypeD">Donors</label>
                        </div>
                      </div>
                    </div>
                    <div class="rounded-border boxed-group date-box">
                      <div class="boxed-group-label">Date Range</div>
                      <div class="date-range rounded-border control-box">
                        <p class="mainlabel">Start Date</p>
                        <div><input class="start-date" type="text"/></div>
                        <p class="mainlabel">End Date</p>
                        <div><input class="end-date" type="text"/></div>
                      </div>
                    </div>
                    <div class="rounded-border boxed-group success-box">
                      <div class="boxed-group-label">Success</div>
                        <div class="log-success-buttons rounded-border control-box">
                          <div class="success-any">
                            <asp:RadioButton ID="LogSuccessAll" runat="server" 
                              Text="All" Value="all"
                              Checked="True"
                              GroupName="LogSuccess"/>
                          </div>
                          <div class="success-sent">
                            <asp:RadioButton ID="LogSuccessSent" runat="server" 
                              Text="Sent" Value="sent"
                              GroupName="LogSuccess"/>
                          </div>
                          <div class="success-failed">
                            <asp:RadioButton ID="LogSuccessFailed" runat="server" 
                              Text="Failed" Value="failed"
                              GroupName="LogSuccess"/>
                          </div>
                      </div>
                    </div>
                    <div class="rounded-border boxed-group flagged-box">
                      <div class="boxed-group-label">Flagged</div>
                        <div class="log-flagged-buttons rounded-border control-box">
                          <div class="flagged-any">
                            <asp:RadioButton ID="LogFlaggedAll" runat="server" 
                              Text="All" Value="all"
                              Checked="True"
                              GroupName="LogFlagged"/>
                          </div>
                          <div class="flagged-yes">
                            <asp:RadioButton ID="LogFlaggedYes" runat="server" 
                              Text="Yes" Value="yes"
                              GroupName="LogFlagged"/>
                          </div>
                          <div class="flagged-no">
                            <asp:RadioButton ID="LogFlaggedNo" runat="server" 
                              Text="No" Value="no"
                              GroupName="LogFlagged"/>
                          </div>
                      </div>
                    </div>
                  </div>

                  <div class="col col2">

                    <user:SelectJurisdictions ID="LogSelectJurisdictions" runat="server" />

                    <div class="rounded-border boxed-group addresses-box">
                      <div class="boxed-group-label">Addresses and Users</div>
                      <div class="addresses-boxes rounded-border control-box">
                        <p class="sublabel">Separate multiple entries with commas.</p>
                        <p class="mainlabel">From Address</p>
                        <div><input class="from-addresses" type="text"/></div>
                        <p class="mainlabel">To Address</p>
                        <div><input class="to-addresses" type="text"/></div>
                        <p class="mainlabel">Vote USA User Name</p>
                        <div><input class="user-names" type="text"/></div>
                      </div>
                    </div>
                    
                    <div class="get-emails-div">
                      <input type="button" value="Get Logged Emails"
                        class="get-emails button-2 button-smallest"/>
                       <span class="get-basic-email-filtering tiptip" title="Apply options from the Election, Office and Candidate Filtering Tab when getting the logged emails">
                        <input type="checkbox" id="GetBasicEmailFiltering"/><label class="std-checkbox" for="GetPoliticiansEmailFiltering">Election, Office and Candidate Filtering</label>
                      </span>  
                    </div>
                  
                  </div>

                </div>
   
                <div id="tab-emaillogging-morefiltering">
                  <div class="mainlabel heading">Select a single jurisdiction on the <em>Basic Filtering</em> tab to enable this control.</div>
                   <div class="col instructions">
                     <ul>
                       <li><span>You can filter log entries by either:</span>
                         <ul>
                           <li><span>One or more elections within a single state, county, or local district.</span></li>
                           <li><span>One or more offices within a single election.</span></li>
                           <li><span>One or more candidates within a single office.</span></li>
                         </ul>
                       </li>
                      </ul>
                    </div>
                   <div class="col col2">
                      <input type="button" value="Get Logged Emails" disabled="disabled"
                        class="get-emails button-2 button-smallest"/>
                       <div class="get-eoc-email-filtering tiptip" title="Apply Success, Flagged, To Address, From Address, User Name, Date Range and Contact Type filtering from the Basic Filtering Tab when getting the logged emails">
                        <input type="checkbox" id="GetEocEmailFiltering"/><label class="std-checkbox" for="GetEocEmailFiltering">Basic Filtering</label>
                      </div>  
                  </div>
                  <user:ElectionsOfficesCandidates ID="ElectionsOfficesCandidates" runat="server" />
                </div>
   
                <div id="tab-emaillogging-politicianfiltering">
                  <div class="col rounded-border boxed-group search">
                    <div class="boxed-group-label">Search for Politicans</div>
                    <div class="instructions">
                       <ul>
                         <li><span><em>Double click</em> or click the blue arrow to add a politician to the list on the right.</span></li>
                         <li><span>Only politicians in states that are selected on the <em>Basic Filtering</em> tab are included in this list.</span></li>
                       </ul>
                    </div>
                    <user:FindPolitician ID="FindPolitician" runat="server" />
                  </div>
                  
                  <div class="col blue-arrow-column">
                    <div class="blue-arrow disabled"></div>
                  </div>

                  <div class="col rounded-border boxed-group selections">
                    <div class="boxed-group-label">Politicians to Include</div>
                    <div class="instructions">
                       <ul>
                         <li><span>Email targeted to these politicians will be shown on the <em>Log Results</em> tab.</span></li>
                       </ul>
                    </div>
                    <div class="selected-politician-list"></div>
                    
                    <div class="col remove">
                      <input type="button" value="Remove Selected" disabled="disabled"
                        class="remove-selected-politicians button-3 button-smallest"/>
                    </div>
                    <div class="col get-logged-emails">
                      <input type="button" value="Get Logged Emails" disabled="disabled"
                        class="get-emails button-2 button-smallest"/>
                       <div class="get-politicians-email-filtering tiptip" title="Apply Success, Flagged, To Address, From Address, User Name and Date Range filtering from the Basic Filtering Tab when getting the logged emails">
                        <input type="checkbox" id="GetPoliticiansEmailFiltering"/><label class="std-checkbox" for="GetPoliticiansEmailFiltering">Basic Filtering</label>
                      </div>  
                   </div>     
                 </div>

                </div>
   
                <div id="tab-emaillogging-batchfiltering">
                  <div class="upper">
                    <div class="col search-string-instructions instructions">
                      <ul>
                        <li><span><em>Date</em>, <em>Success</em>, 
                         <em>FromAddress</em> and <em>User Name</em> filtering from 
                         the <em>Basic Filtering</em> tab applies to this tab.</span></li>
                        <li><span>Enter optional search strings, one per line, to limit the 
                          batch results shown.</span></li>
                        <li><span>Use the <i>Join strings by</i> selection to control whether <i>all</i> or <i>any</i> of the search string must
                          appear in the in the <em>Description</em> to be shown.</span></li>
                        <li><span>Check batches you want to view then click <i>Open Batches</i> to display
                        the emails in the <em>Get Log Entries</em> tsb.</span></li>
                        <li><span>Right click for <em>context menu</em>.</span></li>
                      </ul>
                    </div>
                    <div class="col search-string-col">
                      <select class="add-template-name"><option value="first">&lt;Add template name to search strings&gt;</option></select>
                      <p class="sublabel"><em>Description Search Strings</em></p>
                      <textarea cols="40" rows="3" class="search-strings"></textarea>
                      <p class="radios">Join strings by 
                        <asp:RadioButton ID="BatchJoinStringsByOr" runat="server" 
                          Text="OR" Value="or"
                          Checked="True"
                          GroupName="BatchJoinStringsBy"/>
                        <asp:RadioButton ID="BatchJoinStringsByAnd" runat="server" 
                          Text="AND" Value="and"
                          GroupName="BatchJoinStringsBy"/>
                      </p>
                    </div>
                    <div class="col get-buttons">
                      <div class="get-batches-div">
                        <input type="button" value="Get Batches"
                           class="get-batches button-1 button-smallest"/>
                      </div>
                      <div class="open-batches-div">
                        <input type="button" value="Open Batches" disabled="disabled"
                           class="open-batches button-2 button-smallest"/>
                      </div>   
                      <div class="open-batches-filtering tiptip" title="Apply Success, Flagged and To Address filtering from the Basic Filtering Tab when opening the batches">
                        <input type="checkbox" id="OpenBatchesFiltering"/><label class="std-checkbox" for="OpenBatchesFiltering">Basic Filtering</label>
                      </div>  
                    </div>            
                  </div>
                  <div class="clear-both"></div>
                  <div class="batch-list-results list-results">
                    <p>No batches have been retrieved yet. Click the <em>Get Batches</em> button to create a list of batches.</p>
                  </div>

              </div>
 
                <div id="tab-emaillogging-logfilteringinstructions" class="instructions-page">
                  <div class="inner-panel rounded-border">
                    <div class="inner">
                    <p class="mainlabel"><em>How to Use the</em> Filtering Panels <em>Tabs to Find Logged Emails</em></p>
                    <p class="intro">There are four panels of filtering options for
                    finding logged emails. Each of these panels may be used 
                    independently of each other, or they can be combined to
                    make a selection of logged emails based on any combination
                    of criteria.</p>
                    <p class="intro">The <em>Basic Filtering</em> tab allows 
                    searches for emails based on a variety of criteria:</p>
                    <div class="instructions">
                      <ul>
                        <li>
                          <p><span>The State, County or Local <i>Level</i> selection 
                          combined with the <i>Jurisdictions</i> selections 
                          narrow the selected emails to those that were targeted 
                          to recipients in the selected jurisdictions.</span></p>
                        </li>
                        <li>
                          <p><span><i>Contact Types</i> limits selected emails 
                          to those that were sent via batches of that contact type.</span></p>
                        </li>
                        <li>
                          <p><span>The <i>Date Range</i> limits selected emails to 
                          those sent within the selected range of time.</span></p>
                        </li>
                        <li>
                          <p><span><i>Success</i> allows only successfully sent or 
                          only failed emails to be selected.</span></p>
                        </li>
                        <li>
                          <p><span><i>Flagged</i> allows only flagged or unflagged 
                          emails to be selected.</span></p>
                        </li>
                        <li>
                          <p><span>The <i>From Address</i> and <i>To Address</i> 
                          limits selected email to those that match an entered address.</span></p>
                        </li>
                        <li>
                          <p><span>The <i>Vote USA User Name</i> limits selected email to
                          those that were sent by a batch created by the matching user.</span></p>
                        </li>
                      </ul>
                    </div>
                    <p class="intro">Criteria from the <em>Basic Filtering</em> tab 
                    can also be applied to the selections in the other three 
                    filtering tabs.</p>
                    <p class="intro">To enable the <em>Election, Office and 
                    Candidate Filtering</em> tab be sure to select a single state, 
                    county or local district on the <em >Basic Filtering</em> tab. 
                    Once enabled, follow these steps:</p>
                     <div class="instructions">
                      <ul>
                        <li>
                          <p><span>All elections for the single jurisdiction will
                          be listed in the <i>Elections</i> dropdown. Select one or 
                          more elections. Emails that were sent with that election 
                          selected will be displayed.</span></p>
                        </li>
                        <li>
                          <p><span>If you select a single election the <i>Offices</i> 
                          selection will be enabled and you can narrow the filtering 
                          to one or more offices in the selected election.</span></p>
                        </li>
                        <li>
                          <p><span>Similarly, if you select a single office the 
                          <i>Candidates</i> selection will be enabled and you can 
                          narrow the filtering to one or more candidates from 
                          the selected office.</span></p>
                        </li>
                        <li>
                          <p><span>Click <i>Get Logged Emails</i> to display all 
                          emails for the selected elections, offices or candidates. 
                          Use the <i>Basic Filtering</i> checkbox to control 
                          whether <i>Success</i>, <i>Flagged</i>, <i >To Address</i>, 
                          <i>From Address</i>, <i>User Name</i>, <i>Date Range</i> and 
                          <i>Contact Type</i> filtering from the <em >Basic 
                          Filtering</em> tab is applied to the opened emails.</span></p>
                        </li>
                      </ul>
                    </div>
                    <p class="intro">The <em>Politician Filtering</em> tab lets you 
                    find emails that were targeted to a particular candidate or 
                    politician through one of the Candidate selection types on the
                    <em>Basic Filtering</em> tab. Follow these steps to 
                    select politicians:</p>
                     <div class="instructions">
                      <ul>
                        <li>
                          <p><span>Enter a last name, or the beginning of a last name, 
                          at the top of the left panel. Matches and near matches will 
                          display. To select a politician, click on the name then click 
                          the blue arrow (or double-click the politician). The politician 
                          will be moved to the right panel.</span></p>
                        </li>
                        <li>
                          <p><span>Click <em>Get Logged Emails</em> to display the emails for 
                          all politicians listed in the right panel. Use the <i>Basic Filtering</i> 
                          checkbox to control whether <i>Success</i>, <i >Flagged</i>, <i >To Address</i>,
                          <i>From Address</i>, <i>User Name</i> and <i>Date Range </i>filtering from 
                          the <em>Basic Filtering</em> tab is applied to the opened emails.</span></p>
                          <p><span>Double-click a politician in the right panel to 
                          quickly open emails for just that politician.</span></p>
                        </li>
                      </ul>
                    </div>
                    <p class="intro">The <em>Batch Filtering</em> tab works with 
                    entire batches of sent emails. There are two steps to using 
                    this panel:</p>
                    <div class="instructions">
                      <ul>
                        <li>
                          <p><span>Select a list of batches. The <i>Date</i>, <i>Success</i>,
                          <i>From Address</i> and <i>User Name</i> filters from the <em>Basic 
                          Filtering</em> tab are used to determine which batches are 
                          displayed. In addition, you can enter <i>search strings</i> to 
                          look for batches based on the batch descriptions. If a 
                          <i>Success</i> option is chosen (<i>Sent</i> or <i>Failed</i>), 
                          only batches that contain one or more emails of the selected 
                          type are shown.</span></p>
                        </li>
                        <li>
                          <p><span>Use the checkboxes to choose which batches to 
                          display the emails for, then click the <i>Open 
                          Batches</i> Button. Use the Basic Filtering option to 
                          control whether <i>Success</i>, <i>Flagged</i> and 
                          <i>To Address</i> filtering from the <em>Basic 
                          Filtering</em> tab is applied to the opened emails. 
                          </span></p>
                          <p><span>Double-click a batch to quickly open emails for 
                          just that batch.</span></p>
                        </li>
                      </ul>
                    </div>
                    <p class="intro">If you want to combine emails from more than 
                    one panel into a single listing, go to the 
                    <em>Get Log Entries</em> tab and click <i>Get Logged Emails</i>. 
                    The results will include selections from all four panels. 
                    When this button is used, the <em>Basic Filtering</em> options are always 
                    applied to all panels.</p>
                     </div>
                  </div>
                </div>
 
                <div id="tab-emaillogging-getlogentries">
                    <div class="col col1">
                      <div class="instructions">
                        <ul>
                          <li><span>Right click for <em>context menu</em>.</span></li>
                        </ul>
                      </div>
                      <p class="mainlabel msg" />
                    </div>
                  <div class="get-logged-emails tiptip" title="This button will get any qualifying emails from all filtering tabs with all optional filtering applied.">
                    <div class="get">
                      <input type="button" value="Get Logged Emails"
                          class="get-emails button-1 button-smallest"/>
                    </div>
                    <div class="reset">
                      <input type="button" value="Reset All Criteria"
                          class="reset-criteria button-3 button-smallest"/>
                    </div>
                  </div>
                  <div class="rounded-border boxed-group maximum-box">
                    <div class="boxed-group-label">Maximum Results</div>
                    <div class="rounded-border control-box">
                      <div><input class="maximum-results" type="text" value="1000"/></div>
                    </div>
                  </div>
                  <div class="clear-both"></div>
                  <div class="email-list-results list-results">
                    <p>No logged emails have been retrieved yet. Click the <em>Get Logged Emails</em> button to create a list of emails.</p>
                  </div>
               </div>
            
              </div>
             
            </div>
          </div>
  
  <% if (IsMasterUser)
     { %>
         <div id="tab-masteronly" class="main-tab content-panel tab-panel htab-panel">
            <div class="instructions-page">
            <p class="intro">These functions are used to maintain the tables that support bulk email for visitors. All except
            <em>Refresh All Districts</em> only process new data and should run fairly quickly.</p>
            <p class="intro malformed"><em>Remove Malformed Email Addresses</em> clears any email address from the visitor Addresses table that are not in proper email address format. The rest of the data for the visitor is retained. Last run: <span class="last-run">never</span></p>
            <div class="controls"><input type="button" value="Remove Malformed Email Addresses" class="remove-malformed-emails button-2 button-smallest"/></div>
            <p class="intro from-address"><em>Transfer from Address Log</em> transfers postal addresses that were recently entered in the address dialog to the visitor Addresses table. Follow this with <em>Look Up Missing Districts</em> to be sure these addresses have district coding applied. Last run: <span class="last-run">never</span></p>
            <div class="controls"><input type="button" value="Transfer from Address Log" class="transfer-from-address-log button-2 button-smallest"/></div>
            <p class="intro from-sample-ballot"><em>Transfer from Sample Ballot Log</em> transfers email addresses that were recently entered in the sample ballot dialog to the visitor Addresses table. These entries will already have district coding applied. Last run: <span class="last-run">never</span></p>
            <div class="controls"><input type="button" value="Transfer from Sample Ballot Log" class="transfer-from-sample-ballot-log button-2 button-smallest"/></div>
            <p class="intro lookup-missing"><em>Look Up Missing Districts</em> looks up district coding for any postal addresses or 9 digit zip code if the district coding is missing. Last run: <span class="last-run">never</span></p>
            <div class="controls"><input type="button" value="Look Up Missing Districts" class="lookup-missing-districts button-2 button-smallest"/></div>
            <p class="intro refresh-all"><em>Refresh All Districts</em> looks up district coding for all postal addresses or 9 digit zip codes. This is useful after redistricting. This function processes every visitor entry and so can be quite time consuming. Last run: <span class="last-run">never</span></p>
            <div class="controls"><input type="button" value="Refresh All Districts" class="refresh-all-districts button-2 button-smallest"/></div>
            </div>
         </div>
  <% } %>
           
          </div>
      
      </div>

  </div>

  <ul id="batch-filtering-context-menu" class="context-menu">
    <li class="edit rounded-border"><div class="icon"></div><div class="text">Change Description</div><div class="clear-both"></div></li>
    <li class="info rounded-border"><div class="icon"></div><div class="text">More Info</div><div class="clear-both"></div></li>
  </ul>

  <ul id="recipients-context-menu" class="context-menu">
    <li class="info rounded-border"><div class="icon"></div><div class="text">More Info</div><div class="clear-both"></div></li>
  </ul>

  <ul id="results-context-menu" class="context-menu">
    <li class="view rounded-border"><div class="icon"></div><div class="text">View Email</div><div class="clear-both"></div></li>
    <li class="notes rounded-border"><div class="icon"></div><div class="text">Notes</div><div class="clear-both"></div></li>
    <li class="flagged rounded-border"><div class="icon"></div><div class="text">Flagged</div><div class="clear-both"></div></li>
    <li class="info rounded-border"><div class="icon"></div><div class="text">More Info</div><div class="clear-both"></div></li>
    <li class="batch-description rounded-border"><div class="icon"></div><div class="text">Batch Description</div><div class="clear-both"></div></li>
  </ul>
  
  <div id="batch-list-change-description" class="hidden">
    <div class="inner">
      <div class="all-email batch-change-description in-dialog rounded-border">
        <div>
        <p class="mainlabel">Description of Email Batch for Logging</p>
        <textarea rows="4" cols="80"></textarea>
        </div>
        <div class="bottom-box button-box">
          <input type="button" value="Save New Description" 
            class="change-button button-1 button-smallest"/>
          <input type="button" value="Cancel" 
            class="cancel-button button-3 button-smallest"/>
        </div>
      </div>
    </div>
  </div>
  
  <div id="more-recipient-info" class="list-results more-info hidden">
    <table>
      <tbody>
        <tr class="email">
          <td class="col1">Email</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="contact">
          <td class="col1">Contact</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="title">
          <td class="col1">Title</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="jurisdiction">
          <td class="col1">Jurisdiction</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="phone">
          <td class="col1">Phone</td>
          <td class="col2">&nbsp;</td>
        </tr>
      </tbody>
    </table>
  </div>
  
  <div id="more-batch-info" class="list-results more-info hidden">
    <table>
      <tbody>
        <tr class="even creation-time">
          <td class="col1">Creation Time</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd selection-criteria criteria">
          <td class="col1">Selection Criteria</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="even description">
          <td class="col1">Description</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd total-found">
          <td class="col1">Total Found</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="even skipped-by-user">
          <td class="col1">Skipped by User</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd sent">
          <td class="col1">Sent</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="even failed">
          <td class="col1">Failed</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd user-name">
          <td class="col1">User Name</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="even from">
          <td class="col1">From</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd cc">
          <td class="col1">CC</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="even bcc">
          <td class="col1">BCC</td>
          <td class="col2">&nbsp;</td>
        </tr>
      </tbody>
    </table>
  </div>
  
  <div id="more-results-info" class="list-results more-info hidden">
    <table>
      <tbody>
        <tr class="sent-time">
          <td class="col1">Sent Time</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="contact">
          <td class="col1">Contact</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="to-email">
          <td class="col1">To Email</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="from-email">
          <td class="col1">From Email</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="subject">
          <td class="col1">Subject</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="successfully-sent">
          <td class="col1">Successfully Sent?</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="error-message">
          <td class="col1">Error Message</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="contact-type">
          <td class="col1">Contact Type</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="jurisdiction">
          <td class="col1">Jurisdiction</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="user-name">
          <td class="col1">User Name</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="election">
          <td class="col1">Election</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="office">
          <td class="col1">Office</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="politician">
          <td class="col1">Politician</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="forwarded-count">
          <td class="col1">Times Forwarded</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="is-flagged">
          <td class="col1">Is Flagged</td>
          <td class="col2">&nbsp;</td>
        </tr>
      </tbody>
    </table>
  </div>
 
  <div id="view-logged-email" class="hidden">
    <div class="inner">
      <div class="top">
        <div class="forward-button-container">
          <input type="button" value="Forward" 
             class="forward-button button-2 button-smallest"/>
             <p></p>
        </div>
        <table class="info">
          <tbody>
            <tr class="sent"><td class="col1">Sent:</td><td class="col2"></td></tr>
            <tr class="from"><td class="col1">From:</td><td class="col2"></td></tr>
            <tr class="to"><td class="col1">To:</td><td class="col2"></td></tr>
            <tr class="cc"><td class="col1">CC:</td><td class="col2"></td></tr>
            <tr class="bcc"><td class="col1">BCC:</td><td class="col2"></td></tr>
            <tr class="subject"><td class="col1">Subject:</td><td class="col2"></td></tr>
          </tbody>
        </table>
      </div>
      <div class="bottom">
        <iframe></iframe>
      </div>
    </div>
  </div>
 
  <div id="forward-email" class="hidden">
    <div class="inner">
      <div class="top">
        <div class="send-button-container">
          <input type="button" value="Send" 
             class="send-button button-2 button-smallest"/>
             <p></p>
        </div>
        <table class="info">
          <tbody>
            <tr class="sent"><td class="col1">Sent:</td><td class="col2"></td></tr>
            <tr class="from"><td class="col1">From:</td><td class="col2"></td></tr>
            <tr class="to"><td class="col1">To:</td><td class="col2"></td></tr>
            <tr class="subject"><td class="col1">Subject:</td><td class="col2"></td></tr>
            <tr class="cc"><td class="col1">Cc:</td><td class="col2"></td></tr>
            <tr class="bcc"><td class="col1">Bcc:</td><td class="col2"></td></tr>
          </tbody>
        </table>
        <p class="mainlabel">To:</p>
        <input class="to-address" type="text" />
        <p class="mainlabel">Cc:</p>
        <input class="cc-address" type="text" />
        <p class="mainlabel">Bcc:</p>
        <input class="bcc-address" type="text" />
        <p class="mainlabel">Subject:</p>
        <input class="subject" type="text" />
        <p class="mainlabel">Forwarding Message:</p>
      </div>
      <div class="bottom">
        <textarea class="message-text"></textarea>
      </div>
    </div>
  </div>
  
  <div id="logged-email-notes" class="hidden">
    <div class="inner">
      <div class="top">
        <table class="info">
          <tbody>
            <tr class="sent"><td class="col1">Sent:</td><td class="col2"></td></tr>
            <tr class="from"><td class="col1">From:</td><td class="col2"></td></tr>
            <tr class="to"><td class="col1">To:</td><td class="col2"></td></tr>
            <tr class="subject"><td class="col1">Subject:</td><td class="col2"></td></tr>
          </tbody>
        </table>
        <hr/>
        <textarea cols="10" rows="5" class="add-email-note-text"></textarea>
        <div class="add-email-note-button-container">
          <input type="button" value="Add New Note" 
            class="add-email-note-button button-2 button-smallest"/>
        </div>
        <div class="clear-both"></div>
      </div>
      <div class="bottom">
      </div>
    </div>
  </div>

  <div id="batch-description" class="list-results more-info hidden">
    <table>
      <tbody>
        <tr class="even description">
          <td class="col1">Description</td>
          <td class="col2">&nbsp;</td>
        </tr>
        <tr class="odd selection-criteria criteria">
          <td class="col1">Selection Criteria</td>
          <td class="col2">&nbsp;</td>
        </tr>
      </tbody>
    </table>
  </div>

  <div id="email-batch-failures" class="hidden">
    <div class="inner">
      <div class="top">
        <h4>Batch Description</h4>
      </div>
      <div class="bottom list-results more-info">
        <table data-resizable-columns-id="bulk-email-batch-failures">
          <thead>
            <tr>
              <th data-sort="string" data-resizable-column-id="contact">Contact</th>
              <th data-sort="string" data-resizable-column-id="email">Email</th>
              <th data-sort="string-ins" data-resizable-column-id="message">Message</th>
            </tr>
          </thead>
          <tbody>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  
  <div id="email-template-new-dialog" class="hidden">
    <div class="inner">
      <div class="display-box shadow">
        <table><tbody></tbody></table>
      </div>
      <div class="button-box">
        <input type="button" value="Select" 
          class="select-button button-1 button-smallest"/>
        <input type="button" value="Cancel" 
         class="cancel-button button-3 button-smallest"/>
      </div>
    </div>
  </div>
  
  <user:EmailTemplateDialogs ID="EmailTemplateDialogs" runat="server" />

</asp:Content>
