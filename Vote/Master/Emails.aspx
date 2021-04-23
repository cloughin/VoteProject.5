<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Emails.aspx.cs"
  Inherits="Vote.Master.EmailsPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="uc" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="uc" %>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Emails</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  <style type="text/css">
    .nowrap
    {
      white-space: nowrap;
    }
    #LegislativeTextBox
    {
      width: 680px;
    }
    #NewBatchNameTextBox
    {
      width: 800px;
    }
    #BatchNameTextBox,
    #BatchDescriptionTextBox,
    #BatchFromAddressTextBox,
    #BatchSubjectTextBox,
    #BatchTemplateTextBox
    {
      width: 750px;
    }
    table.substitutions
    {
      width: 750px;
    }
    table.mainTable td
    {
      vertical-align: top;
    }
  </style>
  <script language="javascript" type="text/javascript">
    function clearFeedback() {
      var feedback = document.getElementById('Feedback');
      if (feedback)
        feedback.innerHTML = '';
    }
  </script>
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <uc:LoginBar ID="LoginBar1" runat="server" />
  <uc:Banner ID="Banner" runat="server" />

  <!-- Table -->
  <table class="tableAdmin" id="TitleTable" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HLarge">
        <asp:Label ID="PageTitle" runat="server">Manage Bulk Email Batches</asp:Label>
      </td>
    </tr>
    <tr>
      <td class="T">
        <asp:Label ID="Feedback" runat="server"></asp:Label>
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H" colspan="2">
        Select Email Batch
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        &nbsp;<br />Batch Name:
        <asp:DropDownList ID="BatchNameDropDownList" runat="server" 
          AutoPostBack="True" 
          onselectedindexchanged="BatchNameDropDownList_SelectedIndexChanged" />
      </td>
     </tr>
     <tr>
      <td align="left" class="T" colspan="2">
        &nbsp;
        <div id="NewBatch" runat="server">
          <user:TextBoxWithNormalizedLineBreaks ID="NewBatchNameTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
          <asp:Button ID="CreateBatchButton" runat="server" Text="Create Batch" 
          onclick="CreateBatchButton_Click" onclientclick="clearFeedback()" /><br />
          Enter a unique name for the batch<br />&nbsp;
        </div>
      </td>
     </tr>
  </table>

  <!-- Table -->
  <div id="ExistingBatch" runat="server">
  <table class="tableAdmin mainTable" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H" colspan="2">
        Batch Statistics
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;<br />Time created:
      </td>
      <td align="left" class="T">
        &nbsp;<br /><asp:Label ID="BatchCreationTimeLabel" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Emails in batch:
      </td>
      <td align="left" class="T">
        <asp:Label ID="EmailsInBatchLabel" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Successfully sent:
      </td>
      <td align="left" class="T">
        <asp:Label ID="SuccessfullySentLabel" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Rejected:
      </td>
      <td align="left" class="T">
        <asp:Label ID="RejectedLabel" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        To be sent:
      </td>
      <td align="left" class="T">
        <asp:Label ID="ToBeSentLabel" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
      <td align="left" class="T">
        &nbsp;<br />
        <asp:Button ID="RefreshButton" runat="server" Text="Refresh" 
          onclick="RefreshButton_Click" onclientclick="clearFeedback()" /><br />&nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="H" colspan="2">
        Edit Batch Details
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;<br />
        New name:
      </td>
      <td align="left" class="T">
        &nbsp;<br />
        <user:TextBoxWithNormalizedLineBreaks ID="BatchNameTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Description:
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks TextMode="MultiLine" ID="BatchDescriptionTextBox" Rows="3" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        From Address:<br />
        (Name &lt;xxx@yyy.com&gt;)
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="BatchFromAddressTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Subject Template:
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="BatchSubjectTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        Body Template:
      </td>
      <td align="left" class="T">
        <user:TextBoxWithNormalizedLineBreaks TextMode="MultiLine" ID="BatchTemplateTextBox" Rows="10" runat="server"></user:TextBoxWithNormalizedLineBreaks>
        <table class="tableAdmin substitutions" cellspacing="0" cellpadding="0" border="0">
          <tr>
            <td align="left" class="T" colspan="3">
            &nbsp;<br />
              <b>Template substitution parameters (enclosed in [[ ]]):</b>
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Address</i><br />
              The recipient's street address
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>HtmlAddress</i><br />
              The recipient's complete address<br />
              (street address, city, state, zip),<br />
              multi-line formatted with &lt;br /&gt;'s.
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>StateSenate</i><br />
              3 character state senate district code
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>City</i><br />
              The recipient's city
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Id</i><br />
              The unique row id, used for validating<br />opt-in and opt-out requests.
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>ToEmail</i><br />
              The recipient's email address
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Congress</i><br />
              2 character congressional district code
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>LastName</i><br />
              The recipient's last name.
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Zip</i><br />
              The recipient's 5 or 9 digit zip code
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>County</i><br />
              3 character county code
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>StateCode</i><br />
              The recipient's 2 character state code
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Zip4</i><br />
              The last 4 digits of the 9 digit zip code
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>FirstName</i><br />
              The recipient's first name
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>StateHouse</i><br />
              3 character state house district code
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>Zip5</i><br />
              The first 5 digits of the 9 digit zip code
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;<br />
              <i>FullName</i><br />
              &lt;FirstName&gt; + ' ' + &lt;LastName&gt;<br />
              if empty, &lt;StateCode&gt; + ' ' + Voter
            </td>
            <td align="left" class="T">
              &nbsp;<br />
              <i>StateName</i><br />
              Full name of the recipient's state
            </td>
            <td align="left" class="T">
              &nbsp;
            </td>
          </tr>
       </table>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
      <td align="left" class="T">
        &nbsp;<br />
        <asp:CheckBox ID="BatchClosedCheckBox" runat="server" />Close batch
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        &nbsp;
      </td>
      <td align="left" class="T">
        &nbsp;<br />
        <asp:Button ID="UpdateBatchButton" runat="server" Text="Update Batch" 
          onclick="UpdateBatchButton_Click" onclientclick="clearFeedback()" /><br />&nbsp;
      </td>
    </tr>
  </table>

  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="H" colspan="2">
        Add Email Addresses to Batch
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        &nbsp;<br />
        <b>States to include:</b><br />&nbsp;
      </td>
    </tr>
    <tr>
      <td colspan="2">
        <asp:PlaceHolder ID="StatesPlaceHolder" runat="server"></asp:PlaceHolder>
      </td>
    </tr>
    <tr>
      <td align="center" class="T" colspan="2">
        &nbsp;<br />
        <asp:Button ID="SelectAllStatesButton" runat="server" Text="Select All States" 
          onclick="SelectAllStatesButton_Click" onclientclick="clearFeedback()" />
        <asp:Button ID="ClearAllStatesButton" runat="server" Text="Clear All States" 
          onclick="ClearAllStatesButton_Click" onclientclick="clearFeedback()" />
          <br />&nbsp;
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        &nbsp;<br />
        <b>Filtering:</b>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
      &nbsp;<br />
        <div class="nowrap">
        <asp:CheckBox ID="LegislativeFilterCheckBox" runat="server" />Filter by
        <asp:DropDownList ID="LegislativeFilterDropDownList" runat="server">
          <asp:ListItem Value="CongressionalDistrict">Congressional District</asp:ListItem>
          <asp:ListItem Value="StateSenateDistrict">State Senate District</asp:ListItem>
          <asp:ListItem Value="StateHouseDistrict">State House District</asp:ListItem>
          <asp:ListItem Value="County">County</asp:ListItem>
        </asp:DropDownList>:
        </div>
      </td>
      <td align="left" class="T">
        Enter district codes separated by commas:
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="LegislativeTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="WithNamesCheckBox" runat="server" />Only rows with names
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="WithoutNamesCheckBox" runat="server" />Only rows without names
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="WithAddressesCheckBox" runat="server" />Only rows with addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="WithoutAddressesCheckBox" runat="server" />Only rows without addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="AppendedEmailsCheckBox" runat="server" />Only rows with appended email addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="EnteredEmailsCheckBox" runat="server" />Only rows with entered email addresses
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="KnownDistrictsCheckBox" runat="server" />Only rows with known districts
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        <asp:CheckBox ID="UnknownDistrictsCheckBox" runat="server" />Only rows with unknown districts
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="2">
        Date Range: from 
        <user:TextBoxWithNormalizedLineBreaks ID="FromDateTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks>
        to
        <user:TextBoxWithNormalizedLineBreaks ID="ToDateTextBox" runat="server"></user:TextBoxWithNormalizedLineBreaks> (mm/dd/yyyy or empty)
        <br /><br />
      </td>
    </tr>
    <tr>
      <td colspan="2" align="center">
        <asp:Button ID="AddEmailsButton" runat="server" Text="Add Emails" 
          onclientclick="clearFeedback()" onclick="AddEmailsButton_Click" />
      <br />&nbsp;
      </td>
    </tr>
  </table>
  </div>

  </form>
</body>
</html>
--%>
