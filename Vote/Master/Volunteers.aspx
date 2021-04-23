<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master" 
 CodeBehind="Volunteers.aspx.cs" 
 Inherits="Vote.Master.VolunteersPage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>

<%--
<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet"/>
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
  <meta content="C#" name="CODE_LANGUAGE"/>
  <meta content="JavaScript" name="vs_defaultClientScript"/>
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
  <style type="text/css">
    .partyColumn
    {
      width: 25%;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
  <!-- Table -->
  <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HLarge">
        <asp:Label ID="PageTitle" runat="server">Volunteers</asp:Label>
      </td>
      <td class="T">
        <asp:Label ID="ErrorMessage" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H" colspan="2">
        Volunteer Email Addresses and Contacts
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2" style="height: 13px">
        <strong>Add a New Volunteer Email Address:</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2" style="height: 13px">
        Enter the address in the textbox and click the Add button. <strong>DO NOT</strong>
        enter other information. When the address is added you will be given opportunity
        to add all other information.
      </td>
    </tr>
    <tr>
      <td align="left" class="TBold" colspan="2" valign="top">
        New Email Address:
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddEmailAddress" runat="server" CssClass="TextBoxInput" Width="350px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
        <asp:Button ID="ButtonAddVolunteer" runat="server" CssClass="Buttons" OnClick="ButtonAddVolunteer_Click"
          Text="Add" Width="100px" />
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2">
        <strong>Edit Volunteer<asp:Label ID="LabelEditEmailAddress" runat="server" Text=""></asp:Label>
          :</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Select an email address from the report. Then make any desired changes or additions
        to any textbox. Press the Enter key or click outside the textbox to have your change
        recorded. For state and party, first select a state to retrieve a list of parties
        for the state, then select one of the parties.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>Email Address:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBoxEmailAddress" runat="server"
          Width="236px" CssClass="TextBoxInput" AutoPostBack="True" OnTextChanged="TextBoxEmailAddress_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td align="left" class="T">
        <strong>Phone:</strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPhone" runat="server" Width="200px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxPhone_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>First Name for Salutation: </strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFirstName" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxFirstName_TextChanged" Width="141px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
      <td align="left" class="T">
        <strong>Middle and Last Names:</strong><user:TextBoxWithNormalizedLineBreaks ID="TextBoxLastName" runat="server"
          AutoPostBack="True" CssClass="TextBoxInput" OnTextChanged="TextBoxLastName_TextChanged"
          Width="209px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <strong>Password: </strong>
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxPassword" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnTextChanged="TextBoxPassword_TextChanged" Width="300px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <strong>State: </strong>
        <asp:DropDownList ID="DropDownState" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnSelectedIndexChanged="DropDownState_SelectedIndexChanged">
        </asp:DropDownList>
      </td>
      <td align="left" class="T">
        <strong>Party:</strong>
        <asp:DropDownList ID="DropDownParty" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
          OnSelectedIndexChanged="DropDownParty_SelectedIndexChanged">
        </asp:DropDownList>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="HSmall">
        <strong>Delete Volunteer<asp:Label ID="LabelDeleteEmailAddress" runat="server" Text=""></asp:Label>:</strong>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" style="height: 13px">
        Select an email address from the report. Then click this Delete button.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:Button ID="ButtonDeleteVolunteer" runat="server" CssClass="Buttons" OnClick="ButtonDeleteVolunteer_Click"
          Text="Delete Email" Width="100px" />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        Emails</td>
    </tr>
    <tr>
      <td align="left" class="T">
        To send an email to a volunteer you need to first select an email address in the 
        report below or add a new volunteer above. All the controls above must have a 
        value except the Phone.</td>
    </tr>
    <tr>
      <td align="left" class="HSmall">
        Send Email
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Select the email to send.
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:RadioButtonList ID="RadioButtonListEmail" runat="server" AutoPostBack="True"
          CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListEmail_SelectedIndexChanged">
          <asp:ListItem Value="Custom">Custom text file for email</asp:ListItem>
          <asp:ListItem Value="ThankYou">Thank you for volunteering</asp:ListItem>
          <asp:ListItem Value="YourLogin">Your login credentials</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        From:
      </td>
    </tr>
    <tr>
      <td class="T">
        Default is ron.kahlow@vote-usa.org
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_From" runat="server" CssClass="RadioButtons"
          RepeatDirection="Horizontal">
          <asp:ListItem Value="RonKahlow" Selected="True">From: ron.kahlow@vote-usa.org</asp:ListItem>
          <asp:ListItem Value="Mgr">From: mgr@vote-usa.org</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        To:
      </td>
    </tr>
    <tr>
      <td class="T">
        Emails will be sent to the addresses as indicated by the type of email selected.
        Test emails will always sent to ron.kahlow@businessol.com
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Cc:
      </td>
    </tr>
    <tr>
      <td class="T">
        Default is ron.kahlow@vote-usa.org
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:RadioButtonList ID="RadioButtonList_Cc" runat="server" CssClass="RadioButtons"
          RepeatDirection="Horizontal">
          <asp:ListItem Value="RonKahlow" Selected="True">Cc: ron.kahlow@vote-usa.org</asp:ListItem>
          <asp:ListItem Value="None">No copies will be sent</asp:ListItem>
        </asp:RadioButtonList>
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Custom Substitutions in Email Subject and Body</td>
    </tr>
    <tr>
      <td class="T">
        <strong>Strings:</strong> [[Date]]&nbsp; [[UserName]]&nbsp; [[Password]]&nbsp; 
        [[FName]]&nbsp; [[LName]]&nbsp; [[Phone]]&nbsp; [[StateCode]]&nbsp; [[State]]&nbsp; 
        [[PartyCode]]&nbsp;&nbsp; [[PartyKey]]&nbsp;&nbsp; [[Party]]<br />
        <strong>Email Anchors:</strong> @@ron.kahlow@vote-usa.org@@&nbsp; 
        @@info@vote-usa.org@@&nbsp; @@mgr@vote-usa.org@@&nbsp; 
        @@electionmgr@vote-usa.org@@<br />
        <strong>Web Anchors: </strong>##Vote-USA.org##&nbsp; ##video-scrape-bios##&nbsp; 
        ##video-capture-pic##&nbsp; ##video-scrape-views##</td>
    </tr>
    <tr>
      <td class="HSmall">
        Subject:
      </td>
    </tr>
    <tr>
      <td class="T">
        Subject <strong>BEFORE</strong>:
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="SubjectBefore" runat="server" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
        Subject <strong>AFTER</strong>:&nbsp;
      </td>
    </tr>
    <tr>
      <td class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="SubjectAfter" runat="server" Width="930px" Enabled="False" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <!-- Table1 -->
  <table class="tableAdmin" id="Table1" cellspacing="1" cellpadding="1" width="100%"
    border="1">
    <tr>
      <td align="center" class="HSmall">
        Body:
       </td>
    </tr>
    <tr>
      <td align="center" class="T">
        Body <strong>BEFORE</strong> subsitutions are made:<br>
        (1st Candidate in in Election is used for test)
      </td>
    </tr>
    <tr>
      <td align="center">
        <user:TextBoxWithNormalizedLineBreaks ID="EmailBodyBefore" runat="server" TextMode="MultiLine" Rows="40" Width="920px"
          Height="200px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="T">
       For a non-custom selection, use this button to save any changes made to the Subject and Body without sending
        any emails.
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        <asp:Button ID="Button_Save_Changes" runat="server" CssClass="Buttons" OnClick="Button_Save_Changes_Click"
          Text="Save Template Changes" Width="300px" />
      </td>
    </tr>
    <tr>
      <td class="T">
        Use this button to check custom text files and/or any changes and substitutions you made on the BEFORE Subject
        and Body before you send the emails. 
      </td>
    </tr>
    <tr>
      <td align="center">
        <asp:Button ID="Button_View_Substitutions" runat="server" CssClass="Buttons" OnClick="Button_View_Substitutions_Click"
          Text="Make Substitutions &amp; Check" Width="300px" />
      </td>
    </tr>
    <tr>
      <td align="center" class="T">
        Body  <strong>AFTER</strong> subsitutions are made:<br />
        (1st Candidate in in Election is used for test)
      </td>
    </tr>
    <tr>
      <td align="center">
        <user:TextBoxWithNormalizedLineBreaks ID="EmailBodyAfter" runat="server" Rows="40" TextMode="MultiLine" Width="920px"
          Height="200px" CssClass="TextBoxInputMultiLine" ReadOnly="True"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
  </table>
  <!-- Table2 -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td class="H">
        Send Emails
      </td>
    </tr>
    <tr>
      <td class="HSmall">
        Send Test Emails
      </td>
    </tr>
    <tr>
      <td class="T" colspan="2">
       Use these buttons to send a single test email to 
        ron.kahlow@vote-usa.org to confirm SMTP is working correctly.</td>
    </tr>
    <tr>
      <td align="center" class="T" colspan="2">
        <asp:Button ID="Button_Test_Email" runat="server" CssClass="Buttons" OnClick="Button_Test_Email_Click1"
          Text="Send the First Email as a Test" Width="200px" />
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Use this button to actually send the email 
        to the volunteer. After the email is sent the template
        is updated so the same wording will be used for subsequent emails of the same type.
      </td>
    </tr>
    <tr>
      <td align="center" colspan="2">
        <asp:Button ID="ButtonSendEmail" runat="server" CssClass="Buttons" Text="SEND EMAIL"
          OnClick="ButtonSendEmail_Click1" Width="200px"></asp:Button>
      </td>
    </tr>
  </table>


  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H">
        All Volunteers Report
       </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:PlaceHolder ID="ReportPlaceHolder" runat="server"></asp:PlaceHolder>
      </td>
    </tr>
  </table>
  <!-- Table -->
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
--%>
