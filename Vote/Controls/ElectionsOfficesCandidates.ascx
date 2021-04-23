<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="ElectionsOfficesCandidates.ascx.cs" 
Inherits="Vote.Controls.ElectionsOfficesCandidates" %>

<div id="ElectionsOfficesCandidatesControl" runat="server" 
  class="heirarchical-control elections-offices-candidates">
     
    <div class="col rounded-border boxed-group">
                     
    <div class="main-label box-label">Elections, Offices and Candidates</div>
                 
    <div id="ElectionsPicker" runat="server" class="rounded-border category elections disabled">
      <p class="main-label">Elections:</p>
      <p id="ElectionsPickerAll" runat="server" class="sub-label"><input 
        id="ElectionsPickerAllCheckbox" class="all" disabled="True" 
         checked="True" runat="server" type="checkbox"/><span>All Elections</span>
      </p>
      <p id="ElectionsPickerListButtonContainer" runat="server" class="get-list">
        <input id="ElectionsPickerListButton" runat="server" type="button" value="Get Elections List"
          class="get-list-button button-2 button-smallest" disabled="True"/>
      </p>
      <div id="ElectionsPickerList" runat="server" class="checkbox-list hidden">
      </div>
      <div class="info">To enable election selection, select a single jurisdiction.</div>
    </div>
                  
    <div id="OfficesPicker" runat="server" class="rounded-border category offices disabled">
      <p class="main-label">Offices:</p>
      <p id="OfficesPickerAll" runat="server" class="sub-label"><input 
        id="OfficesPickerAllCheckbox" class="all" disabled="True" 
         checked="True" runat="server" type="checkbox" /><span>All Offices</span></p>
      <p id="OfficesPickerListButtonContainer" runat="server" class="get-list">
        <input id="OfficesPickerListButton" runat="server" type="button" value="Get Offices List"
          class="get-list-button button-2 button-smallest" disabled="True"/>
      </p>
      <div id="OfficesPickerList" runat="server" class="checkbox-list hidden">
      </div>
      <div class="info">To enable office selection, select a single election.</div>
    </div>
                  
    <div id="CandidatesPicker" runat="server" class="rounded-border category candidates disabled">
      <p class="main-label">Candidates:</p>
      <p id="CandidatesPickerAll" runat="server" class="sub-label"><input 
      id="CandidatesPickerAllCheckbox" class="all" disabled="True" 
         checked="True" runat="server" type="checkbox" /><span>All Candidates</span></p>
      <p id="CandidatesPickerListButtonContainer" runat="server" class="get-list">
        <input id="CandidatesPickerListButton" runat="server" type="button" value="Get Candidates List"
          class="get-list-button button-2 button-smallest" disabled="True"/>
      </p>
      <div id="CandidatesPickerList" runat="server" class="checkbox-list hidden">
      </div>
      <div class="info">To enable candidate selection, select a single office.</div>
    </div>
    
    <div class="clear-both"></div>
    
    </div>

</div>
