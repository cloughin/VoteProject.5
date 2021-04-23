<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" AutoEventWireup="true" 
CodeBehind="UpdateParties.aspx.cs" Inherits="Vote.Admin.UpdatePartiesPage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" --%>
<%--@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" --%>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
    <div id="outer">
      <h1 id="H1" EnableViewState="false" runat="server"></h1>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />
    
    <div id="UpdateControls" class="update-controls" runat="server">
  
      <div id="main-tabs" class="tab-control htab-control jqueryui-tabs start-hidden shadow">
 
        <ul class="main-tabs tabs htabs unselectable">
          <li id="Li1" class="tab htab" EnableViewState="false" runat="server"><a href="#tab-temptab" onclick="this.blur()" id="TabTempTab" EnableViewState="false" runat="server">Temp<br />Tab</a></li>
        </ul>

        <div id="tab-temptab" class="main-tab content-panel tab-panel htab-panel">
          <asp:UpdatePanel ID="UpdatePanelTempTab" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
              
            </ContentTemplate>
          </asp:updatePanel>
        </div>
      </div>
    </div>

    </div>

</asp:Content>
