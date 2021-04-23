<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Admin.Master" 
 AutoEventWireup="true" 
 CodeBehind="Nags2.aspx.cs" Inherits="Vote.Master.Nags2Page" %>

<%@ Register Src="/Controls/FeedbackContainer.ascx" TagName="FeedbackContainer" TagPrefix="user" %>
<%@ Register Src="/Controls/SubHeadingWithHelp.ascx" TagName="SubHeadingWithHelp" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
<div id="outer">
  <h1 id="H1" runat="server"></h1>

  <asp:UpdatePanel ID="NagMaintenanceUpdatePanel" runat="server">
    <ContentTemplate>
      <div id="NagContainer" class="maint-container nag-container shadow rounded-border">
        <asp:HiddenField ID="NagEditMode" runat="server" />
        <user:SubHeadingWithHelp ID="NagSubHeadingWithHelp" runat="server"
         Title="Donation Nag Maintenance" CssClass="tiptip"
         Tooltip="Show/hide information about Donation Nag Maintenance">
          <ContentTemplate>
            <p>Nags are presented to a user in the order listed below. The current nag is maintained in a browser cookie, and is incremented each time a nag is presented. If the last nag has been presented, the sequence loops back to the beginning.</p>
            <div class="icon edit-icon"></div><div class="icon-text"><p>Edit a message</p></div>
            <div class="icon delete-icon"></div><div class="icon-text"><p>Delete a message</p></div>
            <div class="icon add-icon"></div><div class="icon-text"><p>Add a new message</p></div>
            <div class="icon update-icon"></div><div class="icon-text"><p>Save the changes you have made</p></div>
            <div class="icon cancel-icon"></div><div class="icon-text"><p>Cancel the current operation</p></div>
            <div style="clear:both"></div>
          </ContentTemplate>
        </user:SubHeadingWithHelp>
        <div id="NagTableContainer" runat="server"></div>
        <user:FeedbackContainer ID="NagFeedback" runat="server" />
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>

  <asp:UpdatePanel ID="NagControlUpdatePanel" runat="server">
    <ContentTemplate>
      <div id="NagControlContainer" class="maint-container nag-control-container shadow rounded-border">
        <user:SubHeadingWithHelp ID="NagControlSubHeadingWithHelp" runat="server"
         Title="Donation Nag Dialog On/Off Control" CssClass="tiptip" 
         Tooltip="Show/hide information about the Donation Nag Dialog">
          <ContentTemplate>
            <p>The <strong>Donation Nag Dialog</strong> can be enabled or disabled. Due to the 
            database cache and memory cache propagation and refresh latencies, it will take 
            at least two and up to ten munutes for any change to take effect.</p>
            <p>Because all donation nagging is handled through Ajax, no page invalidation
            is necessary when this setting is changed.</p>
            <p>Normally select On for production server on Off when developing.</p>
          </ContentTemplate>
        </user:SubHeadingWithHelp>
        <asp:RadioButtonList ID="NagControl" runat="server"
          CssClass="on-off-buttons nag-control-on-off-buttons"
          RepeatDirection="Horizontal" AutoPostBack="True" 
          onselectedindexchanged="NagControl_SelectedIndexChanged">
            <asp:ListItem Value="T">On</asp:ListItem>
            <asp:ListItem Value="F">Off</asp:ListItem>
        </asp:RadioButtonList>
        <user:FeedbackContainer ID="NagControlFeedback" runat="server" />
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>

  <asp:UpdatePanel ID="SampleBallotUpdatePanel" runat="server">
    <ContentTemplate>
      <div id="SampleBallotControlContainer" class="maint-container sample-ballot-control-container shadow rounded-border">
        <user:SubHeadingWithHelp ID="SampleBallotControlSubHeading" runat="server"
         Title="&apos;Get Future Ballot Choices Automatically&apos; Dialog On/Off Control" CssClass="tiptip"
         Tooltip="Show/hide information about the &apos;Get Future Ballot Choices Automatically&apos; Dialog">
          <ContentTemplate>
            <p>The <strong>&apos;Get Future Ballot Choices Automatically&apos; Dialog</strong>
             can be enabled or disabled. Due to the database cache and memory cache 
             propagation and refresh latencies and page invalidation, it will take 
             at least two and up to ten munutes for any change to take effect.</p>
             <p>This only applies to the Ballot pages. Ballot page invalidation happens
             automatically</p>
            <p>Normally select On for production server on Off when developing.</p>
          </ContentTemplate>
        </user:SubHeadingWithHelp>
        <asp:RadioButtonList ID="SampleBallotControl" runat="server"
          CssClass="on-off-buttons sample-ballot-control-on-off-buttons"
          RepeatDirection="Horizontal" AutoPostBack="True" 
          onselectedindexchanged="SampleBallotControl_SelectedIndexChanged">
            <asp:ListItem Value="T">On</asp:ListItem>
            <asp:ListItem Value="F">Off</asp:ListItem>
        </asp:RadioButtonList>
        <user:FeedbackContainer ID="SampleBallotControlFeedback" runat="server" />
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>

</div>
</asp:Content>
