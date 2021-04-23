<%@ Control Language="C#" AutoEventWireup="true" 
CodeBehind="EmailTemplateDialogs.ascx.cs" 
Inherits="Vote.Controls.EmailTemplateDialogs" %>

<div id="email-template-open-dialog" class="hidden">
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

<div id="email-template-open-dialog-confirm" class="hidden">
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

<div id="email-template-save-as-dialog" class="hidden">
  <div class="inner">
    <div class="display-box shadow"></div>
    <div class="template-name-container">
      <span class="label">Template Name:</span>
      <input type="text" class="template-name"/>
    </div>
    <div class="bottom-box radio-box rounded-border">
      <div><input type="radio" id="EmailTemplateSaveAsDialog_Public" 
      name="EmailTemplateSaveAsDialog_Scope" value="Public"/>
      <label for="EmailTemplateSaveAsDialog_Public" class="label">Public</label></div>
      <div><input type="radio" id="EmailTemplateSaveAsDialog_Private" 
      name="EmailTemplateSaveAsDialog_Scope" value="Private"/>
      <label for="EmailTemplateSaveAsDialog_Private" class="label">Private</label></div>
    </div>
    <div class="bottom-box button-box">
      <input type="button" value="Save" 
        class="save-button button-1 button-smallest"/>
      <input type="button" value="Cancel" 
       class="cancel-button button-3 button-smallest"/>
     </div>
  </div>
</div>

<div id="email-template-rename-dialog" class="email-template-rename-dialog hidden">
  <div class="inner">
    <div>
      <p class="label">Original name:</p>
      <p class="original"></p>
      <p class="label">Rename to:</p>
      <p class="new-name"><input type="text"/></p>
    </div>
    <div class="bottom-box button-box">
      <input type="button" value="Rename" 
        class="rename-button button-1 button-smallest"/>
      <input type="button" value="Cancel" 
        class="cancel-button button-3 button-smallest"/>
     </div>
  </div>
</div>

<ul id="email-template-dialog-context-menu" class="context-menu">
  <li class="public rounded-border"><div class="icon"></div><div class="text">Public</div><div class="clear-both"></div></li>
  <li class="rename rounded-border"><div class="icon"></div><div class="text">Rename</div><div class="clear-both"></div></li>
  <li class="delete rounded-border"><div class="icon"></div><div class="text">Delete</div><div class="clear-both"></div></li>
</ul>

