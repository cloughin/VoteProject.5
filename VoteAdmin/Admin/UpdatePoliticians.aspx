<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdatePoliticians.aspx.cs" Inherits="Vote.Admin.UpdatePoliticiansPage" %>

<%@ Register Src="/Controls/ManagePoliticiansPanel.ascx" TagName="ManagePoliticianPanel" TagPrefix="user" %>
<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
    </div>
 
    <user:NoJurisdiction ID="NoJurisdiction" runat="server" IncludeAll="True" Visible="false" />
   
    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addpolitician" onclick="this.blur()" id="TabAddOffice" EnableViewState="false" runat="server">Search, Add,<br />Consolidate, Delete</a></li>
        </ul>

        <div id="tab-addpolitician" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelAddCandidates" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerAddCandidates" runat="server">
                <input id="AddCandidatesReloading" class="reloading" type="hidden" runat="server" />
                <input id="AddCandidatesKeyToDelete" class="key-to-delete" type="hidden" runat="server" />
                <input id="AddCandidatesSearchToRestore" class="search-to-restore" type="hidden" runat="server" />
                <div class="inner-panel rounded-border">         
                  <div class="tab-panel-heading horz-center">
                    <div class="center-inner">
                      <h4 class="center-element">Search, Add, Consolidate or Delete Politicians</h4>
                    </div>
                  </div>
                  <hr/>
                  <div class="change-state-container">
                    <p class="change-state-head">Change State</p>
                    <select id="ChangeState" runat="server" class="change-state"></select>
                  </div>
                  <input type="button" value="Show Password" disabled="disabled" class="button-1 button-smaller show-password-button"/>
                  <input type="button" value="Create Credential Email" disabled="disabled" class="button-1 button-smaller view-emails-button"/>
                  <a class="button-2 button-smallest edit-politician-button disabled" target="politician">Edit Links, Picture, Bio & Reasons</a>
                  <a class="button-2 button-smallest edit-issues-button disabled" target="politician">Edit Issue Topic Responses</a>
                  <input type="button" value="Delete Politician" disabled="disabled" class="button-3 button-smallest delete-politician-button"/>
                
                  <div id="DeletePoliticianOverrideContainer" runat="server" class="kalypto-checkbox input-element override-container">
                    <div class="kalypto-container">
                      <input id="DeletePoliticianOverride" runat="server" class="kalypto delete-politician-override" type="checkbox" />
                    </div>
                    <div class="kalypto-checkbox-label">Check to delete politician along with all related data</div>
                  </div>
                
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
      </div>
    </div>

  </div>
  
  <div id="view-emails-dialog" class="hidden">
    <p class="head">Select Template to View</p>
    <select class="select-email"></select>
    <p class="head">Subject</p>
    <p class="email-subject"></p>
    <p class="head">Body</p>
    <div class="email-body"></div>
    <input type="button" value="Open in Email Client" class="button-1 button-smaller open-email-button"/>
  </div>

</asp:Content>
