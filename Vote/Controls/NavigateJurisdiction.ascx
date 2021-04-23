<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="NavigateJurisdiction.ascx.cs" 
Inherits="Vote.Controls.NavigateJurisdiction" %>

<div id="navigateJurisdiction" class="hidden">
  <input id="PageName" runat="server" class="admin-page-name" type="hidden" />
  <input id="UserSecurityClass" runat="server" class="security-class" type="hidden" />
  <input id="OriginalStateCode" runat="server" class="state-code" type="hidden" />
  <input id="OriginalCountyCode" runat="server" class="county-code" type="hidden" />
  <input id="OriginalLocalCode" runat="server" class="localCode" type="hidden" />
  <h4 id="DialogCredentialMessage" EnableViewState="false" class="credential-message" runat="server"></h4>
  <div class="outer rounded-border shadow">
    <div class="col col1">
      <div class="cell c1 r1">
        <input id="StateRadioButton" runat="server" class="state-radio"
          type="radio" name="Jurisdiction" />        
      </div>
      <div class="cell c1 r2">
        <input id="CountyRadioButton" runat="server" class="county-radio"
         type="radio" name="Jurisdiction" />        
      </div>
      <div class="cell c1 r3">
        <input id="LocalRadioButton" runat="server" class="local-radio"
         type="radio" name="Jurisdiction" />        
      </div>
    </div>
    <div class="col col2">
      <div class="cell c2 r1">
        State
      </div>
      <div class="cell c2 r2">
        County      
      </div>
      <div class="cell c2 r3">
        Local
      </div>
    </div>
    <div class="col col3">
      <div class="cell c3 r1">
        <div id="StateName" runat="server" class="state-name name">&nbsp;</div>
        <div class="shadow-2">
          <asp:DropDownList ID="StateDropDownList" CssClass="state-dropdown shadow-2" 
          runat="server" />
        </div>
      </div>
      <div class="cell c3 r2">
        <div id="CountyName" runat="server" class="county-name name">&nbsp;</div>
        <div class="shadow-2">
          <asp:DropDownList ID="CountyDropDownList" CssClass="county-dropdown shadow-2" 
          runat="server"/>
        </div>
      </div>
      <div class="cell c3 r3">
        <div id="LocalName" runat="server" class="local-name name">&nbsp;</div>
        <div class="shadow-2">
          <asp:DropDownList ID="LocalDropDownList" CssClass="local-dropdown shadow-2" 
          runat="server" />
        </div>
      </div>
    </div>
    <div class="col col4">
      <div class="cell c4 r1">
        <input id="JurisdictionGoButton" type="button" 
        value="Go" class="jurisdiction-go-button button-1 button-smaller"/>      
      </div>
      <div class="cell c4 r2">
        <input id="JurisdictionCancelButton" type="button" 
        value="Cancel" class="jurisdiction-cancel-button button-3 button-smaller"/>
      </div>
      <div class="cell c4 r3">
        <input type="button" 
        value="Placeholder" class="button-3 button-smaller invisible"/>
      </div>
    </div>
    <div class="clear-both"></div>
  </div>
</div>
