<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdateVolunteers.aspx.cs" Inherits="Vote.Master.UpdateVolunteersPage" %>
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    
    
    <div id="UpdateControls" class="update-controls" runat="server">
      
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
        
        <ul class="main-tabs tabs htabs unselectable">
         <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addvolunteer" onclick="this.blur()" id="TabAddVolunteer" EnableViewState="false" runat="server">Add<br />Volunteer</a></li>
         <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-report" onclick="this.blur()" id="TabReport" EnableViewState="false" runat="server">Volunteers<br />Report</a></li>
        </ul>

        <div id="tab-addvolunteer" class="main-tab content-panel tab-panel htab-panel">
           

          <asp:UpdatePanel ID="UpdatePanelAddVolunteer" 
            UpdateMode="Conditional" runat="server">
            <ContentTemplate>
            <div id="ContainerAddVolunteer" class="update-all" runat="server">
              <div class="inner-panel rounded-border">
                <div class="tab-panel-heading horz-center">
                  <div class="center-inner">
                    <div class="undo-button tiptip" title="Clear this entry">
                      <div id="UndoAddVolunteer" runat="server"></div>
                    </div>
                    <h4 class="center-element">Add a Volunteer</h4>
                  </div>
                </div>
                <hr/>

                <div class="content-area">
                  <div class="data-area">

                    <div class="input-element email">
                      <p class="fieldlabel">Email Address <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlAddVolunteerEmail" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                        <div class="tab-ast" id="AsteriskAddVolunteerEmail" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="clear-both spacer"></div>

                    <div class="input-element firstname name">
                      <p class="fieldlabel">First Name <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlAddVolunteerFirstName" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                        <div class="tab-ast" id="AsteriskAddVolunteerFirstName" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="input-element lastname name">
                      <p class="fieldlabel">Last Name <span class="reqd">◄</span></p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlAddVolunteerLastName" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                        <div class="tab-ast" id="AsteriskAddVolunteerLastName" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="input-element phone">
                      <p class="fieldlabel">Phone</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlAddVolunteerPhone" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                        <div class="tab-ast" id="AsteriskAddVolunteerPhone" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="clear-both spacer"></div>

                    <div class="input-element password">
                      <p class="fieldlabel">Password</p>
                      <p class="fieldlabel sub">If omitted a random password will be generated.</p>
                      <div class="databox textbox">
                        <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlAddVolunteerPassword" EnableViewState="false" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                        <div class="tab-ast" id="AsteriskAddVolunteerPassword" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="clear-both spacer"></div>

                    <div class="input-element state">
                      <p class="fieldlabel">State <span class="reqd">◄</span></p>
                      <div class="databox dropdown">
                        <div class="shadow-2">
                          <asp:DropDownList ID="ControlAddVolunteerState" runat="server"/>
                        </div>
                        <div class="tab-ast" id="AsteriskAddVolunteerState" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="input-element party">
                      <p class="fieldlabel">Party Preference <span class="reqd">◄</span></p>
                      <div class="databox dropdown">
                        <div class="shadow-2">
                          <asp:DropDownList ID="ControlAddVolunteerParty" runat="server"/>
                        </div>
                        <div class="tab-ast" id="AsteriskAddVolunteerParty" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="clear-both spacer"></div>
                
                    <div class="input-element notes">
                      <p class="fieldlabel">Notes</p>
                      <div class="databox textarea">
                        <user:TextBoxWithNormalizedLineBreaks ID="ControlAddVolunteerNotes" 
                        EnableViewState="false" TextMode="MultiLine" 
                        CssClass="shadow" runat="server" spellcheck="true" />
                        <div class="tab-ast" id="AsteriskAddVolunteerNotes" EnableViewState="false" runat="server"></div>
                      </div>
                    </div>

                    <div class="clear-both spacer"></div>

                  </div>
                </div>

                <hr />

                <div class="content-footer">
                  <div class="feedback-floater-for-ie7">
                    <user:FeedbackContainer ID="FeedbackAddVolunteer" EnableViewState="false" 
                      runat="server" />
                   </div>
                  <div class="footer-item footer-ajax-loader">
                    <asp:Image ID="AjaxLoaderAddVolunteer" EnableViewState="false" 
                      ImageUrl="~/images/ajax-loader32.gif" 
                      CssClass="ajax-loader" runat="server" />
                  </div>
                  <div class="update-button">
                    <asp:Button ID="ButtonAddVolunteer" EnableViewState="true" runat="server" 
                      Text="Add Volunteer" CssClass="update-button button-1 tiptip" 
                      Title="Add the new volunteer"
                      OnClick="ButtonAddVolunteer_OnClick" /> 
                  </div>   
                  <div class="clear-both"></div>
                </div>   
              </div>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>
          
        </div>
        
        <div id="tab-report" class="main-tab content-panel tab-panel htab-panel">
          
          <div class="inner-panel rounded-border">
            <div class="content-area">
              <div class="data-area">
 
                <div class="input-element state">
                  <p class="fieldlabel">State</p>
                  <select id="ReportState" runat="server" class="select-state">
                  </select>
                </div>
 
                <div class="input-element party">
                  <p class="fieldlabel">Party</p>
                  <select id="ReportParty" runat="server" class="select-party">
                  </select>
                </div>

                <div class="update-button">
		              <input type="button" value="Get Report" class="button-1 get-report-button">                  
                </div>  
                
              </div>

              <div class="clear-both"></div>
             
              <div class="volunteer-scroller hidden">
                
              </div>
            </div>
          </div>

        </div>
        
      </div>
        
    </div>
    

  </div>
  
  <div id="volunteer-notes-dialog" class="hidden">
    <div class="content-area">
      <div class="info">
        <div class="info-name item"><span class="label">Name: </span><span class="value"></span></div>
        <div class="info-email item"><span class="label">Email: </span><a class="value"></a></div>
        <div class="clear-both"></div>
        <div class="info-state item"><span class="label">State: </span><span class="value"></span></div>
        <div class="info-party item"><span class="label">Party: </span><span class="value"></span></div>
        <div class="clear-both"></div>
      </div>
      <div class="notes"></div>
      <div class="new-note-label label">New note:</div>
      <div class="new-note"><textarea rows="5" cols="40"></textarea></div>
      <div class="add-button"><input type="button" class="button-1 button-smallest" value="Add new note" /></div>
    </div>
  </div>
  
  <div id="edit-volunteer-dialog" class="hidden">
    <div class="content-panel tab-panel htab-panel">
      <asp:UpdatePanel ID="UpdatePanelEditVolunteer" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
          <div id="ContainerEditVolunteer" runat="server">
            
            <input id="EditVolunteerReloading" class="reloading" type="hidden" runat="server" />
            <input id="VolunteerToEdit" runat="server" class="volunteer-to-edit" type="hidden" />
            <input id="RefreshReport" runat="server" class="refresh-report" type="hidden"/>
            <div class="inner-panel rounded-border">
          
              <div class="tab-panel-heading horz-center">
                <div class="center-inner">
                  <div class="undo-button tiptip" title="Revert all fields on this panel to the latest saved version">
                    <div id="UndoEditVolunteer" runat="server"></div>
                  </div>
                  <h3 id = "EditVolunteerTitle" runat="server" class="center-element">Edit Volunteer Information</h3>
                </div>
              </div>
              <hr/>
              
              <div class="content-area">

                <div class="input-element email">
                  <p class="fieldlabel"><a class="send-email-from-edit" title="Send email to this address">Email Address</a> <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditVolunteerEmail" CssClass="edit-email shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditVolunteerEmail" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="input-element password">
                  <p class="fieldlabel">Password <span class="reqd">◄</span></p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditVolunteerPassword" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditVolunteerPassword" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>

                <div class="input-element name firstname">
                  <p class="fieldlabel">First Name</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditVolunteerFirstName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditVolunteerFirstName" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="input-element name lastname">
                  <p class="fieldlabel">Last Name</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditVolunteerLastName" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditVolunteerLastName" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="input-element phone">
                  <p class="fieldlabel">Phone</p>
                  <div class="databox textbox">
                    <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlEditVolunteerPhone" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
                    <div class="tab-ast" id="AsteriskEditVolunteerPhone" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>

                <div class="input-element statecode">
                  <p class="fieldlabel">State</p>
                  <div class="databox textbox">
                    <asp:DropDownList id="ControlEditVolunteerStateCode" runat="server" CssClass="shadow-2 select-statecode"></asp:DropDownList>
                    <div class="tab-ast" id="AsteriskEditVolunteerStateCode" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="input-element partykey">
                  <p class="fieldlabel">Party</p>
                  <div class="databox textbox">
                    <asp:DropDownList id="ControlEditVolunteerPartyCode" runat="server" CssClass="shadow-2 select-partykey"></asp:DropDownList>
                    <div class="tab-ast" id="AsteriskEditVolunteerPartyCode" EnableViewState="false" runat="server"></div>
                  </div>
                </div>

                <div class="clear-both spacer"></div>

              </div>
          
              <hr />
              <div class="content-footer">
                <div class="feedback-floater-for-ie7">
                  <user:FeedbackContainer ID="FeedbackEditVolunteer" 
                  EnableViewState="false" runat="server" />
                  </div>
                <div class="footer-item footer-ajax-loader">
                  <asp:Image ID="AjaxLoaderEditVolunteer" 
                  EnableViewState="false" ImageUrl="~/images/ajax-loader32.gif" 
                    CssClass="ajax-loader" runat="server" />
                </div>
                <div class="update-button">
                  <asp:Button ID="ButtonEditVolunteer" EnableViewState="false" 
                    runat="server" 
                    Text="Update" CssClass="update-button button-1 tiptip" 
                    Title="Update the volunteer info"
                    OnClick="ButtonEditVolunteer_OnClick" /> 
                </div>   
                <div class="clear-both"></div>
              </div>   

            </div>

          </div>
        </ContentTemplate>
      </asp:UpdatePanel>
    </div>
  </div>

</asp:Content>
