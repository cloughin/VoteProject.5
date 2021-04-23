<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
AutoEventWireup="true" CodeBehind="SetupBallotPageBannerAd.aspx.cs" 
Inherits="VoteAdmin.Admin.SetupBallotPageBannerAdPage" %>

<%@ Register TagPrefix="user" TagName="FeedbackContainer" Src="~/Controls/FeedbackContainer.ascx" %>
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register TagPrefix="user" TagName="NoJurisdiction" Src="~/Controls/NoJurisdiction.ascx" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <style>
    body.admin-page form,
    .ad-image
    {
      width: 980px !important;
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
    div.adurl input[type=text],
    body .feedback-container .feedback
    {
      width: 800px;
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
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div id="outer">
    <h1 id="H1" runat="server"></h1>

    <user:NoJurisdiction ID="NoJurisdiction" runat="server" Visible="false" />

    <div id="UpdateControls" class="update-controls" runat="server">
    <asp:UpdatePanel ID="UpdatePanelSetupAd" UpdateMode="Conditional" runat="server">
      <ContentTemplate>
        <div id="ContainerSetupAd" runat="server">
          <h6>State: <em id="State" EnableViewState="false" runat="server"></em></h6>
          <h6>Ad Rate: <em id="AdRate" EnableViewState="false" runat="server"></em></h6>
              
          <div class="input-element imagefile">
            <p class="fieldlabel">Ad Image <span class="reqd">◄</span></p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSetupAdAdImageName" CssClass="image-file-name shadow-2" disabled="disabled" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <div class="tab-ast" id="AsteriskSetupAdAdImageChanged" EnableViewState="false" runat="server"></div>
              <label class="button-1 button-smaller" for="AdImageFile">Browse...</label>
              <input type="file" id="AdImageFile" style="display:none"/>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSetupAdAdImageChanged" CssClass="image-file-changed hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSetupAdAdImageUpdated" CssClass="image-file-updated hidden" runat="server"></user:TextBoxWithNormalizedLineBreaks>
            </div>
            <p class="fieldnote">The image ad should be 800 to 980 pixels wide (or wider) and banner-shaped (much wider than tall). 980 wide is ideal.</p>
          </div>
 
          <div class="input-element adurl">
            <p class="fieldlabel">Ad URL</p>
            <div class="databox textbox">
              <user:TextBoxWithNormalizedLineBreaks spellcheck="false" ID="ControlSetupAdAdUrl" CssClass="shadow-2" runat="server"></user:TextBoxWithNormalizedLineBreaks>
              <div class="tab-ast" id="AsteriskSetupAdAdUrl" EnableViewState="false" runat="server"></div>
            </div>
          </div>

          <div style="clear:both"></div>

          <div class="input-element adenabled">
            <div class="databox kalypto-checkbox">
              <div class="kalypto-container">
                <input id="ControlSetupAdAdEnabled" class="kalypto" 
                       runat="server" type="checkbox" />
              </div>
              <div class="kalypto-checkbox-label">Enable Ad on ballot page</div>
              <div class="tab-ast" id="AsteriskSetupAdAdEnabled" EnableViewState="false" runat="server"></div>
            </div>
          </div>

          <div style="clear:both"></div>

          <div class="delete-button">
            <asp:Button ID="ButtonDeleteAd" EnableViewState="false" 
                        runat="server" 
                        Text="Delete This Ad" CssClass="delete-button button-3 button-smaller tiptip" 
                        Title="Delete this ballot page ad" 
                        OnClick="ButtonDeleteAd_OnClick" /> 
          </div>   
              
          <div id="SampleAd" class="sample-ad" runat="server">
            <hr/>
            <div class="banner-ad-outer">
              <div class="image-wrapper" id="ImageWrapper" runat="server">
                <a id="ImageLink" runat="server" target="ext">
                  <img class="ad-image" id="AdImage" runat="server" />
                </a>
              </div>
              <div class="clear-both"></div>
            </div>
<%--            <hr/>
            <a href="/" class="ballot-page-link" target="ext" id="BallotPageLink" runat="server">View on Ballot page</a>--%>
          </div>

          <hr />

          <div class="feedback-floater-for-ie7">
            <user:FeedbackContainer ID="FeedbackSetupAd" 
                                    EnableViewState="false" runat="server" />
          </div>
          <div class="update-button">
            <asp:Button ID="ButtonSetupAd" EnableViewState="false" 
                        runat="server" 
                        Text="Update" CssClass="update-button button-1 tiptip" 
                        Title="Update the ballot page ad setup"
                        OnClick="ButtonSetupAd_OnClick" /> 
          </div>   
          <div class="clear-both"></div>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
    </div>
  </div>
</asp:Content>
