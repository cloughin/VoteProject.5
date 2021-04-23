<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdateIssues.aspx.cs" Inherits="Vote.Admin.UpdateIssuesPage" %>

<%@ Register Src="/Controls/SelectJurisdictions.ascx" TagName="SelectJurisdictions" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
<%--  <style>
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
    .main-tab .col2,
    .main-tab .col3
    {
      margin-left: 20px;
    }
    .main-tab .col1,
    .main-tab .col3
    {
      max-width: 250px;
    }
    .main-tab .col2 .fieldlabel,
    .main-tab .col3 .fieldlabel
    {
      margin-top: 14px;
      color: #bf9241;
      font-weight: bold;

    }
    .main-tab input[type=text]
    {
      width: 300px;
    }
    .main-tab .drag-box
    {
      width: 250px;
      min-height: 100px;
      max-height: 500px;
      overflow-y: auto;
      border: 1px solid #aaa;
      margin-top: 5px;
      padding: 3px;
    }
    .main-tab .drag-box.disabled
    {
      background-color: #ddd;
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
    .main-tab .bottom-button
    {
      margin-top: 5px;
    }
    .main-tab .add-button
    {
      margin-top: 15px;
    }
    #main-tabs .div-button
    {
      cursor: pointer;
    }
    #main-tabs .save-button
    {
      margin-right: 16px;
    }
    .main-tab input[type="radio"],
    .main-tab input[type="checkbox"]
    {
      display: none;
    }
    .main-tab .included-container:not(.included) .included-only
    {
      display: none;
    }
    .main-tab .included-container.included .excluded-only
    {
      display: none;
    }
    .main-tab:not(.filter-all) .filter-all-only
    {
      display: none;
    }
    .main-tab.filter-all .not-filter-all
    {
      display: none;
    }
    .main-tab .filter-message
    {
      margin-top: 4px;
      font-size: 8pt;
      font-weight: bold;
    }
    .main-tab .jurisdictions-area
    {
      margin-left: 20px;
    }
    .main-tab .jurisdiction-selection
    {
      margin-left: 20px;
    }
    .main-tab .jurisdiction-selection-radio
    {
      width: 150px;
    }
    .main-tab .jurisdiction-selection-radio,
    .main-tab .jurisdiction-selection-button
    {
      display: inline-block;
    }
    #tab-questions .topics-filter
    {
      width: 258px;
    }
    .jurisdictions-dialog .inner
    {
      padding: 10px;
    }
    .jurisdictions-dialog .jurisdictions-ok-button
    {
      margin-top: 10px;
    }
          </style>--%>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
    <div id="outer">
      <h1 id="H1" EnableViewState="false" runat="server"></h1>
    
    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
<%--          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-issuegroups" onclick="this.blur()" id="TabIssueGroups" EnableViewState="false" runat="server">Issue Groups</a></li>--%>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-issues" onclick="this.blur()" id="TabIssues" EnableViewState="false" runat="server">Issues</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-questions" onclick="this.blur()" id="TabQuestions" EnableViewState="false" runat="server">Topics</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-consolidatetopics" onclick="this.blur()" id="TabConsolidateTopics" EnableViewState="false" runat="server">Topic Consolidation</a></li>
        </ul>

        <div id="tab-issuegroups" class="main-tab content-panel tab-panel htab-panel">
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Issue Group Maintenance</h4>
                <div class="mode-label-line"><span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span></div>
                <hr/>
                <div class="change-button-line"><span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span></div>
              </div>
            </div>
            <hr/>
            <div class="data-area clearfix">
              <div class="col col1">
                <p class="fieldlabel">All Issue Groups</p>
                <p class="info">(number of issues in parens)</p>
                <div class="issue-groups drag-box"></div>
                <p class="info change-only">Click to Select. <%--Drag to Reorder.--%></p>
<%--                <div class="div-button bottom-button delete-issue-group-button button-3 button-smallest change-only">Delete Selected Issue Group</div>--%>
              </div>
              <div class="col col2">
                <p class="fieldlabel">Issue Group Name</p>
                <input type="text" class="issue-group-name"/>
                <p class="fieldlabel">Issue Group Subheading</p>
                <input type="text" class="issue-group-subheading"/>
                <section class="styled-control-container small fieldlabel">
                  <div class="the-checkbox">
                    <input type="checkbox" name="issue-group-disabled" id="issue-group-disabled"/>
                    <label for="issue-group-disabled"><span class="checkbox">Disabled</span></label>
                  </div>
                </section>
                <div class="div-button add-button add-issue-group-button button-1 button-smaller add-only">Add New Issue Group</div>
              </div>
              <div class="col col3 change-only">
                <div class="included-container included">
                  <p class="fieldlabel">Issues</p>
                  <section class="styled-control-container small included-radios">
                    <div class="one-radio">
                      <input type="radio" name="show-issues" id="show-included-issues" value="I" checked="checked"/>
                      <label for="show-included-issues">
                        <span class="radio">Show included issues</span>
                      </label>
                    </div>
                    <div class="one-radio">
                      <input type="radio" name="show-issues" id="show-excluded-issues" value="X"/>
                      <label for="show-excluded-issues">
                        <span class="radio">Show excluded issues</span>
                      </label>
                    </div>
                  </section>
                  <p class="info">(number of topics in parens)</p>
                  <div class="issue-groups-issues drag-box"></div>
                  <div class="included-only">
                    <p class="info">Drag to Reorder.</p>
<%--                    <p class="info">All issues must be assigned to a group. To remove an issue from a group, assign it to another group.</p>--%>
                  </div>
                  <div class="excluded-only">
<%--                    <p class="info">Click to Select.</p>
                    <div class="div-button bottom-button add-issue-to-group-button button-1 button-smallest change-only">Assign Selected Issue to Group</div>--%>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div id="tab-issues" class="main-tab content-panel tab-panel htab-panel change-mode">
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Issue Maintenance</h4>
                <div class="mode-label-line"><span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span></div>
                <hr/>
                <div class="change-button-line"><span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span></div>
              </div>
            </div>
            <hr/>
            <div class="data-area clearfix">
              <div class="col col1">
                <div class="input-area">
                  <p class="fieldlabel">Issue Name</p>
                  <input type="text" class="issue-name"/>
                  <p class="fieldlabel add-only">Issue Group</p>
                  <select class="issue-group add-only"></select>
                  <section class="styled-control-container small fieldlabel">
                    <div class="the-checkbox">
                      <input type="checkbox" name="issue-disabled" id="issue-disabled"/>
                      <label for="issue-disabled"><span class="checkbox">Disabled</span></label>
                    </div>
                  </section>
                  <div class="div-button add-button add-issue-button button-1 button-smaller add-only">Add New Issue</div></div>
                <p class="fieldlabel">Issues</p>
                <p class="info issues-info no-of-answers-msg">(number of topics in parens)</p>
                <p class="filter-message">Filter by Issue Group <span data-order="alpha" class="order-by">ordered alphabetically</span></p>
                <select class="issues-filter">
                  <option value="">All Groups</option>
                </select>
                <div class="issues drag-box"></div>
                <p class="info change-only">Click to Select. 
                  <span class="filter-all-only">Drag to Reorder.</span>
                  <span class="not-filter-all">Select <em>All Groups</em> filter to Reorder.</span>
                  <div class="div-button bottom-button reorder-issue-button button-1 button-smallest filter-all-only">Reorder by Answer Count</div>
                  <div class="div-button bottom-button delete-issue-button button-3 button-smallest change-only">Delete Selected Issue</div>
                </p>
              </div>
              <div class="col col3 change-only">
                <div class="included-container included">
                  <p class="fieldlabel">Topics</p>
                  <p class="info topics-info no-of-answers-msg included-only">(* = topic assigned to multiple issues)</p>
                  <section class="styled-control-container small included-radios">
                    <div class="one-radio">
                      <input type="radio" name="show-questions" id="show-included-questions" value="I" checked="checked"/>
                      <label for="show-included-questions">
                        <span class="radio">Show included topics</span>
                      </label>
                    </div>
                    <div class="one-radio">
                      <input type="radio" name="show-questions" id="show-excluded-questions" value="X"/>
                      <label for="show-excluded-questions">
                        <span class="radio">Show excluded topics</span>
                      </label>
                    </div>
                  </section>
                  <div class="issue-questions drag-box"></div>
                  <div class="included-only">
                    <p class="info">Drag to Reorder.</p>
                    <p class="info">All topics must be assigned to an issue. To remove a single-issue topic from an issue, assign it to another issue.</p>
                    <div class="div-button bottom-button reorder-topics-button button-1 button-smallest">Reorder by Answer Count</div>
                    <div class="div-button bottom-button remove-question-from-issue-button button-3 button-smallest change-only">Remove Selected Topic from Issue</div>
                  </div>
                  <div class="excluded-only">
                    <p class="info">Click to Select.</p>
                    <div class="div-button bottom-button add-question-to-issue-button button-1 button-smallest change-only">Assign Selected Topic to Issue</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div id="tab-questions" class="main-tab content-panel tab-panel htab-panel change-mode">
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Topic Maintenance</h4>
                <div class="mode-label-line"><span class="mode-label"></span><span class="div-button mode-button button-2 button-smallest"></span></div>
                <hr/>
                <div class="change-button-line"><span class="div-button save-button button-1 button-smaller enable-if-changed"></span><span class="div-button cancel-button button-3 button-smaller enable-if-changed">Cancel Changes</span></div>
              </div>
            </div>
            <hr/>
            <div class="data-area clearfix">
              <div class="col col1">
                <p class="fieldlabel">Topics</p>
                <p class="info no-of-answers-msg">(number of answers in parens)</p>
                <p class="filter-message">Filter by Issue</p>
                <select class="topics-filter">
                </select>
                <div class="topics drag-box"></div>
                <p class="info change-only">Click to Select.
                  <div class="div-button bottom-button delete-topic-button button-3 button-smallest change-only">Delete Selected Topic</div>
                </p>
              </div>
              <div class="col col2">
                <p class="fieldlabel">Topic Name</p>
                <input type="text" class="topic-name"/>
                <p class="fieldlabel add-only">Issue</p>
                <select class="issue add-only"></select>
                <section class="styled-control-container small fieldlabel">
                  <div class="the-checkbox">
                    <input type="checkbox" name="topic-disabled" id="topic-disabled"/>
                    <label for="topic-disabled"><span class="checkbox">Disabled</span></label>
                  </div>
                </section>
                <p class="fieldlabel">Jurisdictions</p>
                <div class="jurisdictions-area">
                  <section class="styled-control-container small fieldlabel">
                    <div class="the-checkbox">
                      <input type="checkbox" class="jbutton for-all" name="national-candidates" id="national-candidates"/>
                      <label for="national-candidates"><span class="checkbox">National candidates</span></label>
                    </div>
                  </section>
                  <section class="styled-control-container small fieldlabel">
                    <div class="the-checkbox">
                      <input type="checkbox" class="jbutton for-all" name="state-candidates" id="state-candidates"/>
                      <label for="state-candidates"><span class="checkbox">State candidates</span></label>
                    </div>
                  </section>
                  <div class="jurisdiction-selection">
                    <section class="styled-control-container small state-radios">
                      <div class="one-radio">
                        <input type="radio" class="jbutton for-all" name="state-radios" id="state-radios-all" value="A" checked="checked"/>
                        <label for="state-radios-all">
                          <span class="radio">All states</span>
                        </label>
                      </div>
                      <div class="one-radio jurisdiction-selection-radio">
                        <input type="radio" class="jbutton" name="state-radios" id="state-radios-selected" value="S"/>
                        <label for="state-radios-selected">
                          <span class="radio">Selected states</span>
                        </label>
                      </div>
                      <div class="div-button jurisdiction-selection-button state-selection-button button-2 button-smallest">Modify Selected States</div>
                    </section>
                  </div>
                  <section class="styled-control-container small fieldlabel">
                    <div class="the-checkbox">
                      <input type="checkbox" class="jbutton for-all" name="county-candidates" id="county-candidates"/>
                      <label for="county-candidates"><span class="checkbox">County candidates</span></label>
                    </div>
                  </section>
                  <div class="jurisdiction-selection">
                    <section class="styled-control-container small county-radios">
                      <div class="one-radio">
                        <input type="radio" class="jbutton for-all" name="county-radios" id="county-radios-all" value="A" checked="checked"/>
                        <label for="county-radios-all">
                          <span class="radio">All counties</span>
                        </label>
                      </div>
                      <div class="one-radio jurisdiction-selection-radio">
                        <input type="radio" class="jbutton" name="county-radios" id="county-radios-selected" value="S"/>
                        <label for="county-radios-selected">
                          <span class="radio">Selected counties</span>
                        </label>
                      </div>
                      <div class="div-button jurisdiction-selection-button county-selection-button button-2 button-smallest">Modify Selected Counties</div>
                    </section>
                  </div>
                  <section class="styled-control-container small fieldlabel">
                    <div class="the-checkbox">
                      <input type="checkbox" class="jbutton for-all" name="local-candidates" id="local-candidates"/>
                      <label for="local-candidates"><span class="checkbox">Local candidates</span></label>
                    </div>
                  </section>
                  <div class="jurisdiction-selection">
                  <section class="styled-control-container small local-radios">
                    <div class="one-radio">
                      <input type="radio" class="jbutton for-all" name="local-radios" id="local-radios-all" value="A" checked="checked"/>
                      <label for="local-radios-all">
                        <span class="radio">All locals</span>
                      </label>
                    </div>
                    <div class="one-radio jurisdiction-selection-radio">
                      <input type="radio" class="jbutton" name="local-radios" id="local-radios-selected" value="S"/>
                      <label for="local-radios-selected">
                        <span class="radio">Selected locals</span>
                      </label>
                    </div>
                    <div class="div-button jurisdiction-selection-button local-selection-button button-2 button-smallest">Modify Selected Locals</div>
                  </section>
                  </div>
                </div>
                <div class="div-button add-button add-topic-button button-1 button-smaller add-only">Add New Topic</div>
              </div>
              <div class="col col3 change-only"></div>
            </div>
          </div>
        </div>

      <div id="tab-consolidatetopics" class="main-tab content-panel tab-panel htab-panel change-mode">
        <div class="inner-panel rounded-border">
          <div class="tab-panel-heading horz-center">
            <div class="center-inner">
              <h4 class="center-element">Topic Consolidation</h4>
            </div>
          </div>
          <hr/>
          <div class="data-area clearfix">
            <div class="col col1">                
              <p class="fieldlabel">Topic to Consolidate Into</p>
              <p class="info">(number of answers in parens)</p>
              <div class="topics-to drag-box"></div>
            </div>
            <div class="col col2">
              <p class="fieldlabel">Topic to be Consolidated</p>
              <p class="info">(number of answers in parens)</p>
              <div class="topics-from drag-box"></div>
              <div><input type="button" class="consolidate-topics-button button-1" value="Consolidate Topics"/></div>
            </div>
          </div>
        </div>
      </div>

      </div>
    </div>

    </div>

  <div id="jurisdictions-dialog" class="hidden">
    <div class="inner">
      <user:SelectJurisdictions ID="SelectJurisdictions" runat="server"/>
      <button class="button-1 button-smaller jurisdictions-ok-button">OK</button>
    </div>
  </div>

  <div id="answer-dates-dialog" class="hidden">
    <div class="inner">
      <section class="styled-control-container small answer-dates-radios">
        <div class="one-radio">
          <input type="radio" name="answer-dates" id="show-all-answers" value="A" checked="checked"/>
          <label for="show-all-answers">
            <span class="radio">All Answers</span>
          </label>
        </div>
        <div class="one-radio">
          <input type="radio" name="answer-dates" id="show-between-dates" value="B"/>
          <label for="show-between-dates">
            <span class="radio">Between Dates</span>
          </label>
          <div class="answer-date-item answer-date-from-item">
            <div>From (mm/dd/yyyy)</div>
            <input type="text" class="date-picker answer-date-from"/>
          </div>
          <div class="answer-date-item answer-date-to-item">
            <div>To (mm/dd/yyyy)</div>
            <input type="text" class="date-picker answer-date-to"/>
          </div>
        </div>
      </section>
      <button class="button-1 button-smaller answer-dates-set-button">Set Dates</button>
    </div>
  </div>

</asp:Content>
