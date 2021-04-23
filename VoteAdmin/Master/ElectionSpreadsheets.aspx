<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true"
CodeBehind="ElectionSpreadsheets.aspx.cs" Inherits="VoteAdmin.ElectionSpreadsheetsPage" %>

<%@ Register Src="/Controls/ManagePoliticiansPanel.ascx" TagName="ManagePoliticianPanel" TagPrefix="user" %>
<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <div class="step-images">
      <div class="step-image step-image-1 active"></div>
      <div class="step-divider"></div>
      <div class="step-image step-image-2"></div>
      <div class="step-divider"></div>
      <div class="step-image step-image-3"></div>
      <div class="step-divider"></div>
      <div class="step-image step-image-4"></div>
    </div> 
    
    <div class="step step-1">
      <div class="step-title">Select Spreadsheet</div>
      <div class="select-types">
        <section class="styled-control-container step-1-types">
          <div class="one-radio">
            <input type="radio" name="step-1-type-group" id="step-1-type-existing" value="E" checked="checked"/>
            <label for="step-1-type-existing"><span class="radio">Use Existing Spreadsheet</span></label>
          </div>
          <div class="one-radio">
            <input type="radio" name="step-1-type-group" id="step-1-type-upload" value="N"/>
            <label for="step-1-type-upload"><span class="radio">Upload New Spreadsheet</span></label>
          </div>
        </section>
      </div>
      <div class="step-1-option step-1-existing">
        <section class="step-1-scopes styled-control-container small">
          <div class="one-radio">
            <input type="radio" name="step-1-scope-group" id="step-1-scope-inprocess" value="I" checked="checked"/>
            <label for="step-1-scope-inprocess"><span class="radio">Show In-Process Spreadsheets Only</span></label>
          </div>
          <div class="one-radio">
            <input type="radio" name="step-1-scope-group" id="step-1-scope-all" value="A"/>
            <label for="step-1-scope-all"><span class="radio">Show All Spreadsheets</span></label>
          </div>
        </section>
        <div class="spreadsheet-list">
          <asp:Literal ID="SpreadsheetListLiteral" runat="server"></asp:Literal>
        </div>
      </div>
      <div class="step-1-option step-1-upload hidden">
        <div class="file-input-button">
          <label for="UploadFile" class="button-2 button-smaller">
            Choose spreadsheet to Upload
          </label>
          <span>No spreadsheet selected</span>
          <input id="UploadFile" name="UploadFile" class="file-input" type="file"/>
        </div>
        <div class="selection state-selection">
          <span class="field-label">Select State:</span>
          <select id="SelectState" runat="server" class="select-state pointer styled-select"></select>
        </div>
        <div class="step-1-upload-options step-1-contests">
          <section class="styled-control-container small">
            <div class="one-radio">
              <input type="radio" name="step-1-contests-group" id="step-1-contest-state" value="S" checked="checked"/>
              <label for="step-1-contest-state"><span class="radio">Includes only state-level contests</span></label>
            </div>
            <div class="one-radio">
              <input type="radio" name="step-1-contests-group" id="step-1-contest-all" value="A"/>
              <label for="step-1-contest-all"><span class="radio">Includes county and/or local contests</span></label>
            </div>
            <div class="one-radio">
              <input type="radio" name="step-1-contests-group" id="step-1-contest-local" value="L"/>
              <label for="step-1-contest-local"><span class="radio">Includes contests for only a specific county or local jurisdiction</span></label>
            </div>
          </section>
          <div class="select-jurisdiction">
            <input type="button" class="button-2 button-smallest select-jurisdiction-button" value="Select Jurisdiction" disabled="disabled"/>
            <span class="selected-jurisdiction"></span>
          </div>
        </div>
        <!--<section class="styled-control-container small">
          <div class="the-checkbox">
            <input type="checkbox" name="checkbox-test" id="checkbox-test"/>
            <label for="checkbox-test"><span class="checkbox">Checkbox Test</span></label>
          </div>
        </section>
        <section class="styled-control-container checked-box">
          <div class="the-checkbox">
            <input type="checkbox" name="kalypto-test" id="kalypto-test"/>
            <label for="kalypto-test"><span class="checkbox">Kalypto Test</span></label>
          </div>
        </section>-->
        <div class="selection election-selection">
          <span class="field-label">Select Election:</span>
          <select id="SelectElection" runat="server" class="select-election pointer styled-select"></select>
        </div>
        <div class="step-1-upload-options step-1-primaries hidden">
          <section class="styled-control-container small">
            <div class="one-radio">
              <input type="radio" name="step-1-primary-group" id="step-1-primary-all" value="A" checked="checked"/>
              <label for="step-1-primary-all"><span class="radio">Includes all primaries on this date</span></label>
            </div>
            <div class="one-radio">
              <input type="radio" name="step-1-primary-group" id="step-1-primary-single" value="S"/>
              <label for="step-1-primary-single"><span class="radio">Includes only the selected primary</span></label>
            </div>
          </section>
        </div>
        <div class="action-button upload-button disabled pointer rounded">Upload</div>
      </div>
    </div>
    
    <div class="step step-2 hidden">
      <div class="step-title">Map Columns</div>
      <div class="spreadsheet-description"></div>
      <div class="mapping-list"></div>
    </div>
    
    <div id="main-tabs" class="step step-3 hidden">
      <div class="step-title">Process Rows</div>
      <div class="spreadsheet-description"></div>
      <div class="row-navigation">
        Processing row <input readonly="readonly" class="row-spinner" value="1"/> of <span class="row-count"></span>
      </div>
      <div class="candidate-box rounded-border boxed-group">
        <div class="boxed-group-label">Candidate</div>
        <div class="data-raw candidate-raw">Raw name data: <span class="raw"></span></div>
        <div id="tab-addpolitician" class="politicians-panel">
          <asp:UpdatePanel ID="UpdatePanelAddCandidates" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerAddCandidates" runat="server">
                <input id="AddCandidatesReloading" class="reloading" type="hidden" runat="server" />
                <div class="inner-panel">         
                
                  <div class="content-area clearfix">
                    <user:ManagePoliticianPanel ID="ManagePoliticiansPanel" runat="server" />
                  </div>

                  <hr />
                  <div class="content-footer">
                    <div class="feedback-floater-for-ie7">
                      <user:FeedbackContainer ID="FeedbackAddCandidates" 
                                              EnableViewState="false" runat="server" />
                    </div>
                    <div class="update-button">
                      <asp:Button ID="ButtonAddCandidates" EnableViewState="false" 
                                  runat="server" 
                                  Text="Update" CssClass="update-button button-1 tiptip no-disable" 
                                  Title="Update the politician"
                                  OnClick="ButtonAddCandidates_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   
                </div>
              </div>
            </ContentTemplate>
          </asp:updatePanel>
        </div>
        <div class="clear-both"></div>
      </div>
    </div>
    
    <div class="step step-4 hidden">
      <div class="step-title">Mark Spreadsheet Complete</div>
      <div class="spreadsheet-description"></div>
      <section class="styled-control-container step-4-is-complete-container checked-box">
        <div class="the-checkbox">
          <input type="checkbox" name="is-complete" id="step-4-is-complete"/>
          <label for="step-4-is-complete"><span class="checkbox">Spreadsheet is complete</span></label>
        </div>
      </section>

    </div>
    
    <div class="continue-buttons">
      <div class="continue-button back-button disabled hidden-important pointer rounded">&lt; Back</div>
      <div class="continue-button next-button disabled pointer rounded">Next &gt;</div>
    </div>

  </div>

  <div id="searchDistricts" class="hidden">
    <input type="text" placeholder="Enter search text" class="search-districts-text" />
    <p class="instr">Double click to select county or district</p>
    <div class="search-districts-select"></div>
  </div>
</asp:Content>
