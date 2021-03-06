<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailFormResponsive.ascx.cs" Inherits="Vote.Controls.EmailFormResponsive" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteAdmin" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"  TagPrefix="BotDetect" %>

<div class="email-form no-print">
  <p class="email-form-label">Subject:</p>
  <select id="EmailFormSubject" class="email-form-subject no-zoom" runat="server" />
  <user:TextBoxWithNormalizedLineBreaks ID="EmailFormOtherSubject" placeholder="Please specify the subject for Other" CssClass="email-form-other-subject no-zoom" runat="server"  />
  <p class="email-form-label">Your Email Address:</p>
  <user:TextBoxWithNormalizedLineBreaks ID="EmailFormFromEmailAddress" CssClass="email-form-from-email-address no-zoom" runat="server"  />
  <asp:PlaceHolder ID="EmailFormFieldPlaceHolder" runat="server"></asp:PlaceHolder>
  <p id="EmailFormMessageLabel" class="email-form-label" runat="server">Message:</p>
  <user:TextBoxWithNormalizedLineBreaks ID="EmailFormMessage" CssClass="email-form-message no-zoom" runat="server" TextMode="MultiLine" Rows="10" />
  <div class="emailFormCaptchaControls">
    <p>To help us control spam, please retype the characters from this picture:</p>
    <p><BotDetect:Captcha ID="EmailFormCaptcha" runat="server" /></p>
    <p><user:TextBoxWithNormalizedLineBreaks ID="EmailFormCaptchaCodeTextBox" CssClass="captcha-text no-zoom" runat="server" /><br />
    <asp:Label ID="EmailFormErrorLabel" class="email-form-error-label" runat="server"/>
    <asp:Label ID="EmailFormGoodLabel" class="email-form-good-label" runat="server"/></p>
    <p>
      <asp:button ID="Submit" text="Send Email" onclick="SubmitForm" runat="server" CssClass="submit-button"/>
      <%--<input id="Submit" type="submit" class="submit-button" value="Send Email" />--%>
    </p>
  </div>
</div>
