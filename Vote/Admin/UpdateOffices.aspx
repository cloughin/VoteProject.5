<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="UpdateOffices.aspx.cs" 
Inherits="Vote.Admin.UpdateOfficesPage" %>

<%@ Register Src="/Controls/NavigateJurisdiction.ascx" TagName="NavigateJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>
<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/OfficeControl.ascx" TagName="OfficeControl" TagPrefix="user" %>
<%@ Register Src="/Controls/ManagePoliticiansPanel.ascx" TagName="ManagePoliticianPanel" TagPrefix="user" %>
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style type="text/css">
    #main-tabs .icon-move {
      visibility: hidden;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
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
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addoffice" onclick="this.blur()" id="TabAddOffice" EnableViewState="false" runat="server">Add<br />Office</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabAddOfficeTemplateItem"><a href="#tab-addofficetemplate" onclick="this.blur()" id="TabAddOfficeTemplate" EnableViewState="false" runat="server">Add Office<br />Template</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-changeinfo" onclick="this.blur()" id="TabChangeInfo" EnableViewState="false" runat="server">Change/Delete<br />Office</a></li>
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addincumbents" onclick="this.blur()" id="TabIncumbents" EnableViewState="false" runat="server">Office<br />Incumbents</a></li>
          <li class="tab htab" EnableViewState="false" runat="server" id="TabMasterItem"><a href="#tab-masteronly" onclick="this.blur()" id="TabMaster" EnableViewState="false" runat="server">Master<br />Functions</a></li>
        </ul>

        <div id="tab-addoffice" class="main-tab content-panel tab-panel htab-panel">
           
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Add an Office</h4>
              </div>
            </div>
            <hr/>
            <div class="content-area">
              <h6 class="inverted">First add the new office. Then a dialog will allow you to enter the office details.</h6>
              <div class="instructions">
                <ul>
                  <li><span>Select an Office Class.</span></li>
                  <li><span>Existing offices for that class (if any) are displayed. Be sure you do not add a duplicate.</span></li>
                  <li><span>Enter the Office Title (2nd line is optional) and click Add.</span></li>
                  <li><span>After the office is added a dialog will allow office defaults to be changed.</span></li>
                </ul>
              </div>
              <hr/>
              <div class="data-area">
                <div class="clearfix">
                  <div class="office-class-box rounded-border boxed-group">
                    <div class="boxed-group-label">Office Class</div>
                    <select id="AddOfficeSelectOfficeClass" runat="server" class="select-office-class"></select>
                  </div>
                </div>
                <div>
                  <div class="hidden new-office-box rounded-border boxed-group clearfix">
                    <div class="boxed-group-label">New Office</div>

                    <div class="current-office-list-container">
                       <p class="fieldlabel">The following offices currently exist for the selected office class.</p>
                       <p class="fieldlabel"><em>Please be sure you are not adding a duplicate office.</em></p>
                       <div class="current-office-list"></div>
                    </div>
 
                    <div class="input-element office-line office-line-1">
                      <p class="fieldlabel">1st Line of Office Title <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeLine1" spellcheck="false" CssClass="shadow-2" runat="server"/>
                      </div>                
                    </div>
 
                    <div class="input-element office-line office-line-2">
                      <p class="fieldlabel">2nd Line of Office Title</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeLine2" spellcheck="false" CssClass="shadow-2" runat="server"/>
                      </div>                
                    </div>
                    
                    <div class="clear-both"></div>

                    <div class="update-button">
		                  <input type="button" value="Add Office" class="button-1 add-office-button">                  
                    </div>  
                    
                    <div class="recase-button">
		                  <input type="button" value="Recase Office Title" class="button-2 button-smallest recase-office-button">                  
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdateOfficeControl" 
              OnLoad="UpdateOfficeControl_Load" ClientIDMode="Static">
            <ContentTemplate>
            </ContentTemplate>
          </asp:UpdatePanel>         

        </div>

  <% if (AdminPageLevel == AdminPageLevel.State)
     { %>
        <div id="tab-addofficetemplate" class="main-tab content-panel tab-panel htab-panel">
          
          <input type="hidden" id="AddOfficeTemplateHidden" class="hidden-data" runat="server"/>
          
          <div class="inner-panel rounded-border">
            <div class="tab-panel-heading horz-center">
              <div class="center-inner">
                <h4 class="center-element">Add an Office Template</h4>
              </div>
            </div>
            <hr/>
            <div class="content-area">
              <h6 class="inverted">Office templates allow county and local offices to be set up on a state-wide basis.</h6>
              <div class="instructions">
                <ul>
                  <li><span>Select an Office Level and an Office Class.</span></li>
                  <li><span>Existing templates for that class (if any) are displayed. Be sure you do not add a duplicate.</span></li>
                  <li><span>Enter the Office Title (2nd line is optional) and click Add.</span></li>
                  <li><span>After the template is added a dialog will allow office defaults to be changed.</span></li>
                </ul>
              </div>
             <hr/>
              <div class="data-area">
                <div class="clearfix">
                  <div class="office-level-box rounded-border boxed-group clearfix">
                    <div class="boxed-group-label">Office Level</div>
                    <div>
                      <input type="radio" id="AddOfficeTemplate_County" 
                        name="AddOfficeTemplate_Level" value="county"/>
                      <label for="AddOfficeTemplate_County" class="label">For county elections</label>
                    </div>
                    <div>
                      <input type="radio" id="AddOfficeTemplate_Local" 
                        name="AddOfficeTemplate_Level" value="local"/>
                      <label for="AddOfficeTemplate_Local" class="label">For local elections</label>
                    </div>
                  </div>
                  <div class="office-class-box rounded-border boxed-group">
                    <div class="boxed-group-label">Office Class</div>
                    <select class="select-office-class" disabled="disabled"><option value="">Choose an office class</option></select>
                  </div>
                </div>
                <div>
                  <div class="hidden new-template-box rounded-border boxed-group clearfix">
                    <div class="boxed-group-label">New Office Template</div>

                    <div class="current-template-list-container">
                       <p class="fieldlabel">The following templates currently exist for the selected office class.</p>
                       <p class="fieldlabel"><em>Please be sure you are not adding a duplicate template.</em></p>
                       <div class="current-template-list"></div>
                    </div>
 
                    <div class="input-element office-line office-line-1">
                      <p class="fieldlabel">1st Line of Office Title <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" CssClass="shadow-2" runat="server"/>
                      </div>                
                    </div>
 
                    <div class="input-element office-line office-line-2">
                      <p class="fieldlabel">2nd Line of Office Title</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" CssClass="shadow-2" runat="server"/>
                      </div>                
                    </div>
                    
                    <div class="clear-both"></div>
 
                    <div class="update-button">
		                  <input type="button" value="Add Office Template" class="button-1 add-office-template-button">                  
                    </div>  
                   
                    <div class="recase-button">
		                  <input type="button" value="Recase Office Title" class="button-2 button-smallest recase-office-button">                  
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

        </div>
  <% } %>

        <div id="tab-changeinfo" class="main-tab content-panel tab-panel htab-panel">
           
          <div class="office-control-container">
          </div>

          <div id="ContainerChangeInfo" runat="server">
            <div class="inner-panel rounded-border">
              <div class="tab-panel-heading horz-center">
                <div class="center-inner">
                  <h4 class="center-element">Change Office Information</h4>
                </div>
              </div>
              <hr/>
              <div class="content-area">
                <h6 class="inverted">Select an office, then click the Edit Office or Delete Office button.</h6>
                <a class="select-office-toggler" >
                  <div></div><h6 id="HeadingChangeInfoOffice" class="office-heading" runat="server">No office selected</h6>
                </a>
                <div class="clear-both"></div>
                <hr/>
                <div class="data-area">
                  <input type="button" value="Edit Office" class="changeinfo-edit-button button-2 disabled"/>
                  <input type="button" value="Delete Office" class="changeinfo-delete-button button-3 disabled"/>
                </div>
 
                <div class="clear-both"></div>
              </div>

            </div>
          </div>

        </div>

        <div id="tab-addincumbents" class="main-tab content-panel tab-panel htab-panel">
           
          <div class="office-control-container">
            <user:OfficeControl ID="OfficeControl" runat="server" />
          </div>

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
                      <h4 class="center-element">Add, Change or Delete Incumbents</h4>
                    </div>
                  </div>
                  <hr/>
                  <div class="content-area">
                    <a class="select-office-toggler" >
                      <div></div><h6 id="HeadingAddCandidatesOffice" class="office-heading" runat="server">No office selected</h6>
                    </a>
                    <div class="clear-both"></div>
                    <hr/>
                    <div class="data-area">
                    <user:ManagePoliticianPanel ID="ManagePoliticiansPanel" runat="server" />
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
                        Title="Update the incumbent(s)"
                        OnClick="ButtonAddCandidates_OnClick" /> 
                    </div>   
                    <div class="clear-both"></div>
                  </div>   

                </div>
              </div>
            </ContentTemplate>
          </asp:UpdatePanel>
       </div>

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
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-masteronly-lockclass" onclick="this.blur()" id="TabMasterOnlyLockClass" EnableViewState="false" runat="server">Lock/Unlock<br />Office Class</a></li>
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-masteronly-changekey" onclick="this.blur()" id="TabMasterOnlyChangeKey" EnableViewState="false" runat="server">Change<br />Office Key</a></li>
                      <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-masteronly-consolidate" onclick="this.blur()" id="TabMasterOnlyConsolidate" EnableViewState="false" runat="server">Consolidate<br />Offices</a></li>
                    </ul>

                    <div id="tab-masteronly-lockclass">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>Check an office class to prevent inadvertant addition of offices within that class.</span></li>
                        </ul>
                      </div>
                      
                      <div class="input-element classestolock rounded-border">
                        <asp:CheckBoxList ID="ControlMasterOnlyClassesToLock" CssClass="check-box-list kalypto" runat="server">
                          <asp:ListItem>&nbsp;</asp:ListItem>
                        </asp:CheckBoxList>
                      </div>

                    </div>

                    <div id="tab-masteronly-changekey">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>Use this function to change an office key for the current jurisdiction. When you enter the
                           old and new keys, do not include the jurisdictional prefix (state code, county code and/or local code). All
                           references to the office key will be changed throughout the database. Before any changes are made,
                           a check will be done to ensure that the new key does not already exist.</span></li>
                        </ul>
                      </div>

                      <div class="input-element jurisdictionalprefix changekeyprefix">
                        <p class="fieldlabel">Jurisdictional Prefix</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyChangeKeyPrefix"  
                            CssClass="shadow-2 tiptip" runat="server" disabled="disabled" />
                        </div>  
                      </div>         
                      
                      <div class="input-element oldkey">
                        <p class="fieldlabel">Old Office Key<br/><em>(without jurisdictional prefix)</em> <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyOldKey"  
                            CssClass="shadow-2 tiptip" title="Enter the old office key without the jurisdictional code prefix" runat="server"/>
                        </div>                
                      </div>
                      
                      <div class="input-element newkey">
                        <p class="fieldlabel">New Office Key<br/><em>(without jurisdictional prefix)</em> <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyNewKey"  
                            CssClass="shadow-2 tiptip" title="Enter the new office key without the jurisdictional code prefix" runat="server"/>
                        </div>                
                      </div>

                    </div>

                    <div id="tab-masteronly-consolidate">
                   
                      <div class="instructions">
                        <ul>
                          <li><span>Use this function to consolidate two office entries for the current jurisdiction. When you enter the
                           old and new keys, do not include the jurisdictional prefix (state code, county code and/or local code). All
                           references to the second office key will be changed to the first key throughout the database. If duplicate entries
		                       exist in either incumbents or elections, only the entry with the first key will be retained</span></li>
                        </ul>
                      </div>

                      <div class="input-element jurisdictionalprefix consolidateprefix">
                        <p class="fieldlabel">Jurisdictional Prefix</p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyConsolidatePrefix"  
                            CssClass="shadow-2 tiptip" runat="server" disabled="disabled" />
                        </div>  
                      </div>         
                      
                      <div class="input-element key1">
                        <p class="fieldlabel">Office Key 1 (retained)<br/><em>(without jurisdictional prefix)</em> <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyKey1"  
                            CssClass="shadow-2 tiptip" title="Enter the office key to be retained without the jurisdictional code prefix" runat="server"/>
                        </div>                
                      </div>
                      
                      <div class="input-element key2">
                        <p class="fieldlabel">Office Key 2 (eliminated)<br/><em>(without jurisdictional prefix)</em> <span class="reqd">◄</span></p>
                        <div class="databox textbox">
                          <user:TextBoxWithNormalizedLineBreaks spellcheck="false" 
                            ID="ControlMasterOnlyKey2"  
                            CssClass="shadow-2 tiptip" title="Enter the office key to be consolidated into the first key without the jurisdictional code prefix" runat="server"/>
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
  
  <div id="edit-office-dialog" class="hidden">
    <div class="content-panel tab-panel htab-panel">
      <asp:UpdatePanel ID="UpdatePanelEditOffice" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
          <div id="ContainerEditOffice" runat="server">
            
            <input id="EditOfficeReloading" class="reloading" type="hidden" runat="server" />
            <input id="OfficeToEdit" runat="server" class="office-to-edit" type="hidden" />
            <div class="inner-panel rounded-border">
          
              <div class="tab-panel-heading horz-center">
                <div class="center-inner">
                  <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                    <div id="UndoEditOffice" runat="server"></div>
                  </div>
                  <h3 id = "EditOfficeTitle" runat="server" class="center-element">Edit Office Information</h3>
                </div>
              </div>
              <hr/>
          
              <div class="content-area">

                <div class="input-element officeline officeline1">
                  <p class="fieldlabel">1st Line of Office Title <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficeOfficeLine1" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficeOfficeLine1" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="input-element officeline officeline2">
                  <p class="fieldlabel">2nd Line of Office Title</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficeOfficeLine2" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficeOfficeLine2" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
 
                <div class="input-element incumbents">
                  <p class="fieldlabel">Incumbents</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficeIncumbents" CssClass="shadow-2 tiptip" runat="server"
                    title="This is the number of elected officials (incumbents) at any time for this office. Most offices are 1, some like US Senate have 2, and commissions can have more than 2."></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficeIncumbents" EnableViewState="false" runat="server"></div>
                  </div>
                </div>
 
                <div class="input-element electionpositions">
                  <p class="fieldlabel">Election Positions</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficeElectionPositions" CssClass="shadow-2 tiptip" runat="server"
                    title="This is the number of office positions that are up for election in elections. Most offices have 1 but commissions can have more than 1."></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficeElectionPositions" EnableViewState="false" runat="server"></div>
                  </div>
                </div>
 
                <div class="input-element primarypositions">
                  <p class="fieldlabel">Primary Positions</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficePrimaryPositions" CssClass="shadow-2 tiptip" runat="server"
                    title="This is the number of positions that are up for election in primaries. Most offices have 1."></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficePrimaryPositions" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
 
                <div class="input-element primaryrunoffpositions">
                  <p class="fieldlabel">Primary Runoff Positions</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficePrimaryRunoffPositions" CssClass="shadow-2 tiptip" runat="server"
                    title="This is the number of candidates that advance to a primary runoff election if a runoff is necessary. Enter 0 if runoffs are never used in primaries for this office. Enter 2 or more to indicate that a fixed number of candidates advance to the potential primary runnoff. Enter -1 if the number of candidates that can advance is variable."></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficePrimaryRunoffPositions" EnableViewState="false" runat="server"></div>
                  </div>
                </div>
 
                <div class="input-element generalrunoffpositions">
                  <p class="fieldlabel">General Runoff Positions</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditOfficeGeneralRunoffPositions" CssClass="shadow-2 tiptip" runat="server"
                    title="This is the number of candidates that advance to a general runoff election if a runoff is necessary. Enter 0 if runoffs are never used in general elections for this office. Enter 2 or more to indicate that a fixed number of candidates advance to the potential general election runnoff. Enter -1 if the number of candidates that can advance is variable."></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditOfficeGeneralRunoffPositions" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                   
                <div class="input-element showwritein">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlEditOfficeShowWriteIn" class="kalypto" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Show a write-in line</div>
                    <div class="tab-ast" id="AsteriskEditOfficeShowWriteIn" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                 
                <div class="input-element isrunningmateoffice">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlEditOfficeIsRunningMateOffice" class="kalypto" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">The candidates for this office have a running mate in the general election</div>
                    <div class="tab-ast" id="AsteriskEditOfficeIsRunningMateOffice" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                  
                <div class="input-element isonlyforprimaries">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlEditOfficeIsOnlyForPrimaries" class="kalypto" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">The office is only for primaries</div>
                    <div class="tab-ast" id="AsteriskEditOfficeIsOnlyForPrimaries" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>
                  
                <div class="input-element isinactive">
                  <div class="databox kalypto-checkbox">
                    <div class="kalypto-container">
                      <input id="ControlEditOfficeIsInactive" class="kalypto" 
                        runat="server" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Inactive</div>
                    <div class="tab-ast" id="AsteriskEditOfficeIsInactive" EnableViewState="false" runat="server"></div>
                  </div>
                </div>
 
                <div class="clear-both spacer"></div>

              </div>
          
              <hr />
              <div class="content-footer">
                <div class="feedback-floater-for-ie7">
                  <user:FeedbackContainer ID="FeedbackEditOffice" 
                  EnableViewState="false" runat="server" />
                  </div>
                <div class="footer-item footer-ajax-loader">
                  <asp:Image ID="AjaxLoaderEditOffice" 
                  EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                    CssClass="ajax-loader" runat="server" />
                </div>
                <div class="update-button">
                  <asp:Button ID="ButtonEditOffice" EnableViewState="false" 
                    runat="server" 
                    Text="Update" CssClass="update-button button-1 tiptip" 
                    Title="Update the office info"
                    OnClick="ButtonEditOffice_OnClick" /> 
                </div>   
                <div class="clear-both"></div>
              </div>   

            </div>

          </div>
        </ContentTemplate>
      </asp:UpdatePanel>
    </div>
  </div>
  
  <div id="delete-office-dialog" class="hidden">
    
    <p class="reference-heading">The following references to this office were found:</p>
    <div class="reference-scroller"></div>
    <p class="reference-heading">If you delete the office, these references will be removed.</p>
  
    <div class="update-button">
		  <input type="button" value="Delete the Office" class="button-1 delete-office-button">                  
    </div>  
   
  </div>

  <user:NavigateJurisdiction ID="NavigateJurisdiction1" runat="server" AdminPageName="UpdateOffices"
   NonStateNames="US|US President" />

</asp:Content>
