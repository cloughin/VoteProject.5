<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackContainer.ascx.cs" 
Inherits="Vote.Controls.FeedbackContainerControl" %>
<div id="FeedbackContainerOuter" class="feedback-container" runat="server">
  <a id="FeedbackHider" runat="server" class="hider tiptip" title="Hide this message">hide</a>
  <div id="Feedback" runat="server" class="feedback rounded-border"></div>
  <div style="clear:both"></div>
</div>
