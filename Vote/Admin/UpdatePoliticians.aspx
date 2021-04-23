<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdatePoliticians.aspx.cs" Inherits="Vote.Admin.UpdatePoliticiansPage" %>

<%@ Register Src="/Controls/ManagePoliticiansPanel.ascx" TagName="ManagePoliticianPanel" TagPrefix="user" %>
<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <div id="outer">
    <h1 id="H1" EnableViewState="false" runat="server"></h1>
    <div class="sub-heading headings">
      <h2 id="H2" EnableViewState="false" class="jurisdiction-message" runat="server"></h2>
    </div>
    
    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li class="tab htab" EnableViewState="false" runat="server"><a href="#tab-addpolitician" onclick="this.blur()" id="TabAddOffice" EnableViewState="false" runat="server">Search, Add,<br />Consolidate</a></li>
        </ul>

        <div id="tab-addpolitician" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelAddCandidates" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              <div id="ContainerAddCandidates" runat="server">
                <input id="AddCandidatesReloading" class="reloading" type="hidden" runat="server" />
                <div class="inner-panel rounded-border">         
                  <div class="tab-panel-heading horz-center">
                    <div class="center-inner">
                      <h4 class="center-element">Add Politician</h4>
                    </div>
                  </div>
                  <hr/>
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
                        Text="Update" CssClass="update-button button-1 tiptip" 
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

</asp:Content>
