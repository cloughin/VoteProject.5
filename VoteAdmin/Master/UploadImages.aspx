<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="UploadImages.aspx.cs" 
Inherits="Vote.Master.UploadImagesPage" %>

<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    .preview-image
    {
      max-width: 940px;
    }
    #outer
    {
      margin-bottom: 20px;
    }
    .delete-button
    {
      margin-top:5px;
    }
    .input-element
    {
      float: left;
      margin-top: 12px;
    }

    p.fieldlabel
    {
      font-size: 12px;
    }

    p.fieldnote
    {
      margin-top: 4px;
    }

    div.imagefile input[type=text],
    div.comments textarea,
    body .feedback-container .feedback
    {
      width: 800px;
    }

    div.externalname input[type=text]
    {
      width: 400px;
    }

    div.imagefile label
    {
      /*position: absolute;
    right: -90px;*/
      margin-left: 5px;
    }

    .tab-ast
    {
      Background-image: url(/images/opsprite.png);
      background-position: -164px -4px;
      height: 12px;
      /*position: absolute;*/
      width: 12px;
      margin-left: 3px;
    }
    .radio-modes label
    {
      font-size: 16px;
      font-weight: bold;
    }
    .radio-modes input[type=radio]
    {
      margin: 10px 0;
    }
    .preview-image .head
    {
      font-size: 13px;
      font-weight: bold;
      margin-bottom: 5px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <asp:UpdatePanel ID="UpdatePanelUploadImage" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <asp:RadioButtonList Id="RadioModes" AutoPostBack="True" CssClass="radio-modes" 
          OnSelectedIndexChanged="RadioModes_OnSelectedIndexChanged" runat="server">
          <asp:ListItem Selected="True" Value="new">&nbsp;Upload New Image</asp:ListItem>
          <asp:ListItem Value="update">&nbsp;Update Existing Image</asp:ListItem>
        </asp:RadioButtonList>

        <asp:DropDownList ID="SelectExternalName" cssClass="dropdown-external-name" AutoPostBack="True"
         OnSelectedIndexChanged="SelectExternalName_SelectedIndexChanged" runat="server"></asp:DropDownList>
        <div id="ContainerUploadImage" runat="server">
 
          <div class="input-element externalname">
            <p class="fieldlabel">External Name (without file extension)</p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageExternalName" CssClass="shadow-2 external-name" runat="server"></user:TextBoxWithNormalizedLineBreaks>
<%--              <div class="tab-ast" id="AsteriskUploadImageExternalName" EnableViewState="false" runat="server"></div>--%>
            </div>
          </div>
 
          <div class="input-element imagetype">
            <p class="fieldlabel">File Type</p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageImageType" CssClass="shadow-2" disabled="disabled" runat="server"></user:TextBoxWithNormalizedLineBreaks>
<%--              <div class="tab-ast" id="AsteriskUploadImageImageType" EnableViewState="false" runat="server"></div>--%>
            </div>
          </div>
             
          <div class="input-element imagefile">
            <p class="fieldlabel">Uploaded File Name<span class="reqd">◄</span></p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageFileName" CssClass="image-file-name shadow-2" disabled="disabled" runat="server"></user:TextBoxWithNormalizedLineBreaks>
<%--              <div class="tab-ast" id="AsteriskUploadImageFileName" EnableViewState="false" runat="server"></div>--%>
              <label class="button-1 button-smaller" for="ImageFile">Browse...</label>
              <input type="file" id="ImageFile" style="display:none"/>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageImageChanged" CssClass="image-file-changed hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageImageUpdated" CssClass="image-file-updated hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageImageId" CssClass="image-id hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageDeleteImage" CssClass="delete-image hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
            </div>
          </div>
 
          <div class="input-element comments">
            <p class="fieldlabel">Comments</p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlUploadImageComments" TextMode="MultiLine" CssClass="shadow-2 data-comments" runat="server"></user:TextBoxWithNormalizedLineBreaks>
<%--              <div class="tab-ast" id="AsteriskUploadImageComments" EnableViewState="false" runat="server"></div>--%>
            </div>
          </div>

          <div style="clear:both"></div>

          <div class="delete-button">
            <input id="ButtonDeleteImage" type="button" class="delete-button button-3 button-smaller"
                   value="Delete This Image" title="Delete this image" runat="server" />
<%--            <asp:Button ID="ButtonDeleteImage" EnableViewState="false" 
                        runat="server" 
                        Text="Delete This Image" CssClass="delete-button button-3 button-smaller tiptip" 
                        Title="Delete this image" 
                        OnClick="ButtonDeleteImage_OnClick" /> --%>
          </div>   
              
          <div id="PreviewImage" class="preview-image" runat="server">
            <hr/>
            <div class="head">Preview (actual size <span id="PreviewSize" runat="server">123x456</span>)</div>
            <div class="image-wrapper" id="ImageWrapper" runat="server">
              <img class="preview-image" id="SampleImage" runat="server" />
            </div>
            <div class="clear-both"></div>
          </div>

          <hr />

          <div class="feedback-floater-for-ie7">
            <user:FeedbackContainer ID="FeedbackUploadImage" 
                                    EnableViewState="false" runat="server" />
          </div>
          <div class="update-button">
            <asp:Button ID="ButtonUploadImage" EnableViewState="false" 
                        runat="server" 
                        Text="Upload Image" CssClass="update-button button-1 tiptip no-disable" 
                        Title="Update the image"
                        OnClick="ButtonUploadImage_OnClick" /> 
          </div>   
          <div class="clear-both"></div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
