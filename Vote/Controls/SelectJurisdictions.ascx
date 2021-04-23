<%@ Control Language="C#" AutoEventWireup="true" 
  CodeBehind="SelectJurisdictions.ascx.cs" 
  Inherits="Vote.Controls.SelectJurisdictions" %>
  
  <div id="SelectJurisdictionsControl" runat="server" 
    class="heirarchical-control select-jurisdictions-control">
    
    <div class="col rounded-border boxed-group">
                  
    <div class="main-label box-label">Jurisdictions</div>
                 
    <div id="StatesPicker" runat="server" class="rounded-border category jurisdiction states">
      <p class="main-label">States:</p>
      <p id="StatesPickerAll" runat="server" class="sub-label hidden"><input id="StatesPickerAllCheckbox" class="all" runat="server" type="checkbox"/><span>All States</span></p>
      <p id="StatesPickerSpecific" runat="server" class="sub-label specific specific-state">Specific State</p>
      <div id="StatesPickerList" runat="server" class="checkbox-list states-list hidden">
      </div>
    </div>
                  
    <div id="CountiesPicker" runat="server" class="rounded-border category jurisdiction counties">
      <p class="main-label">Counties:</p>
      <p id="CountiesPickerAll" runat="server" class="sub-label"><input id="CountiesPickerAllCheckbox" class="all" runat="server" type="checkbox" /><span>All Counties</span></p>
      <p id="CountiesPickerSpecific" runat="server" class="sub-label specific specific-county hidden">Specific County</p>
      <p id="CountiesPickerListButtonContainer" runat="server" class="get-list hidden">
        <input id="CountiesPickerListButton" runat="server" type="button" value="Get Counties List"
          class="get-list-button button-2 button-smallest"/>
      </p>
      <div id="CountiesPickerList" runat="server" class="checkbox-list counties-list hidden">
      </div>
      <div class="info">To enable county selection, select a single state.</div>
    </div>
                  
    <div id="LocalsPicker" runat="server" class="rounded-border category jurisdiction locals">
      <p class="main-label">Local Districts:</p>
      <p id="LocalsPickerAll" runat="server" class="sub-label"><input id="LocalsPickerAllCheckbox" class="all" runat="server" type="checkbox" /><span>All Local Districts</span></p>
      <p id="LocalsPickerSpecific" runat="server" class="sub-label specific specific-local hidden">Specific Local District</p>
      <p id="LocalsPickerListButtonContainer" runat="server" class="get-list">
        <input id="LocalsPickerListButton" runat="server" type="button" value="Get Local Districts List"
          class="get-list-button button-2 button-smallest"/>
      </p>
      <div id="LocalsPickerList" runat="server" class="checkbox-list locals-list hidden">
      </div>
      <div class="info">To enable local district selection, select a single county.</div>
    </div>
   
    <div class="clear-both"></div>
    
    </div>
      
    <div class="col rounded-border party-box boxed-group hidden">
                   
    <div class="main-label box-label">Parties</div>
                  
    <div id="PartiesPicker" runat="server" class="rounded-border category parties hidden">
      <p class="main-label">Parties:</p>
      <p id="PartiesPickerAll" runat="server" class="sub-label"><input id="PartiesPickerAllCheckbox" class="all" type="checkbox"/><span>All Parties</span></p>
      <p id="PartiesPickerMajor" runat="server" class="sub-label"><input id="PartiesPickerMajorCheckbox" class="major" type="checkbox" checked="checked"/><span>Major Parties Only</span></p>
      <p id="PartiesPickerListButtonContainer" class="get-list">
        <input id="PartiesPickerListButton" type="button" value="Get Parties List" disabled="disabled"
          class="get-list-button button-2 button-smallest"/>
      </p>
      <div id="PartiesPickerList" class="checkbox-list hidden">
      </div>
      <div class="info">To enable party selection, select a single state.</div>
    </div>
    
    </div>
   
    <div class="clear-both"></div>

  </div>

