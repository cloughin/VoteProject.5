<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="xEmailTemplateOpenDialog.ascx.cs" 
Inherits="Vote.Controls.xEmailTemplateOpenDialog" %>

<div id="email-template-open-dialog2" class="hidden">
  <div class="inner">
    <div class="display-box shadow"></div>
    <div class="bottom-box radio-box rounded-border">
      <div><input type="radio" id="EmailTemplateOpenDialog_ShowOwned" 
      name="EmailTemplateOpenDialog_Show" value="Owned"/>
      <label for="EmailTemplateOpenDialog_ShowOwned" class="label">Show my templates only</label></div>
      <div><input type="radio" id="EmailTemplateOpenDialog_ShowAll" 
      name="EmailTemplateOpenDialog_Show" value="All"/>
      <label for="EmailTemplateOpenDialog_ShowAll" class="label">Show all available templates</label></div>
    </div>
    <div class="bottom-box compatible-box rounded-border">
      <div><input id="EmailTemplateOpenDialog_Compatible" type="checkbox" 
      class="compatible-checkbox"/></div>
      <div class="compatible-label"><label for="EmailTemplateOpenDialog_Compatible" class="label">Only show templates that are compatible with selected recipients</label></div>
    </div>
    <div class="bottom-box button-box">
      <input type="button" value="Open" 
        class="open-button button-1 button-smallest"/>
      <input type="button" value="Cancel" 
       class="cancel-button button-3 button-smallest"/>
     </div>
  </div>
</div>

<div id="email-template-open-dialog2-confirm" class="hidden">
  <div class="inner">
    <div class="message"></div>
    <div class="bottom-box button-box">
      <input type="button" value="Save" 
        class="save-button button-1 button-smallest"/>
      <input type="button" value="Don't Save" 
        class="dont-save-button button-2 button-smallest"/>
      <input type="button" value="Cancel" 
       class="cancel-button button-3 button-smallest"/>
     </div>
  </div>
</div>

<ul id="email-template-open-dialog-context-menu" class="context-menu">
  <li class="public rounded-border"><div class="icon"></div><div class="text">Public</div><div class="clear-both"></div></li>
  <li class="rename rounded-border"><div class="icon"></div><div class="text">Rename</div><div class="clear-both"></div></li>
  <li class="delete rounded-border"><div class="icon"></div><div class="text">Delete</div><div class="clear-both"></div></li>
</ul>
