<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailForm.ascx.cs" Inherits="Vote.Controls.EmailForm" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"  TagPrefix="BotDetect" %>

<div id="emailForm">
  <p class="emailFormLabel">Subject:</p>
  <select id="EmailFormSubject" class="emailFormSubject" runat="server" />
  <p class="emailFormLabel">Your email address:</p>
  <user:TextBoxWithNormalizedLineBreaks ID="EmailFormFromEmailAddress" class="emailFormFromEmailAddress" runat="server"  />
  <p class="emailFormLabel">Message:</p>
  <user:TextBoxWithNormalizedLineBreaks ID="EmailFormMessage" CssClass="emailFormMessage" runat="server" TextMode="MultiLine" Rows="10" />
  <div class="emailFormCaptchaControls">
    <p>To help us control spam, please retype the characters from this picture:</p>
    <p><BotDetect:Captcha ID="EmailFormCaptcha" runat="server" /></p>
    <p><user:TextBoxWithNormalizedLineBreaks ID="EmailFormCaptchaCodeTextBox" runat="server" /><br />
    <asp:Label ID="EmailFormErrorLabel" class="emailFormErrorLabel" runat="server"/>
    <asp:Label ID="EmailFormGoodLabel" class="emailFormGoodLabel" runat="server"/></p>
    <p><asp:Button ID="EmailFormSubmitButton" cssClass="submitButton" runat="server" Text="Send Email" /></p>
  </div>
</div>
