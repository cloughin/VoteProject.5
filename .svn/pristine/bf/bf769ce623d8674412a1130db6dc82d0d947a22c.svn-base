<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
CodeBehind="OfficeContest.aspx.cs"
  Inherits="Vote.Admin.OfficeContests" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Office Contest</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  </head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
    <tr>
      <td align="left" class="HSmall">
        <asp:Label ID="PageTitle" runat="server"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <asp:Label ID="Msg" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" cellspacing="0" cols="3" cellpadding="0" border="0">
    <tr>
      <td align="center" class="H">
        Office Contest Candidates</td>
    </tr>
    <tr>
      <td class="T">
        The report below are all the candidates currently identified for this office contest
        in this election.
        <br />
        <strong>Adding Candidates:</strong> Enter a candidate&#39;s last 
        name in the textbox provided, and select Enter. You will be provided with a form 
        to complete the addition. <br />
        <strong>Delete a Candidate:</strong> Click a name in the Candidtes report (not 
        the picture). A from will be provided which will enable you to delete the 
        candidate in this office contest.<br />
        <strong>Edit a Candidate: </strong>Click a name in the Candidates report. A from 
        will be provided which will enable you to change the candidate&#39;s ballot order; identify 
        a running mate (like
        President / Vice President and Governor / Lieutenant Governor); and a link on 
        this form will allow you to edit information about the candidate.<br />
        <strong>Edit Information on a Candidate&#39;s Intro Page:</strong> Click on a 
        picture in the Candidates Report to be persented with a form to add, change and delete information 
        found on the candidate&#39;s Introduction Page.<br />
        <strong>Edit This Office:</strong> Click on the link in the &#39;Edit Office&#39; 
        Section.<br />
        <strong>Edit a Different Office Contest:</strong> You need to navigate to to the <strong>Election
        </strong>Page (link in the navbar) to make changes to a different office contest.
      </td>
    </tr>
    <tr>
      <td class="T">
        There is/are<asp:Label ID="LabelIncumbents" runat="server" 
          CssClass="TBoldColor"></asp:Label>
        position(s) for this office.</td>
    </tr>
    <tr>
      <td align="center">
        <asp:Label ID="HTMLTableCandidatesForThisOffice" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
    <!-- Table -->
  <table class="tableAdmin" id="Table_Same_Last_Name_Report" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="center" class="H">
        <!-- Select Candidate to Add to Office Contest -->
        Politicians in Database with Same Last Name</td>
    </tr>
    <tr id="tr_SelectCandidate" runat=server>
      <td align="left" class="HSmall">
        <asp:Label ID="LabelCandidateToAdd" runat="server"></asp:Label>
      </td>
    </tr>
    <tr id="tr_SelectCandidateInstruction" runat=server>
      <td align="left" class="T">
        These are all the politicians with the same last name in the database.
        If the politician is listed, click that person's 
        name link (not the picture)
        to add that person as a candidate in this office contest. When you click the 
        name you will be presented with a form where you will be 
        able to
        provide additional information about the candidate. 
      </td>
    </tr>
    <tr>
      <td align="center">
        <asp:Label ID="HTMLTableSameLastName" runat="server"></asp:Label>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Add_A_Candidate" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H">
        Add a Candidate</td>
    </tr>
    <tr>
      <td align="left" class="T">
        To add a candidate you must insure that the 
        person is not already in 
        the database. This is important because if you add a candidate who is already in the database, 
        the data and pictures will have to be re-entered. Only enter only the person&#39;s 
        last name in the text box provided and then select Enter. Casing is unimportant 
        but include spaces and special characters like dashes and hyphens. If you have 
        any doubts, you may check variations of the last name, like using a name part 
        (i.e. Drew for Van Drew); or adding or omitting spaces and hyphens. For 
        multi-part last name like LaRiva or McKinney try adding a space between the 
        parts like La Riva or Mc Kinney.
      </td>
    </tr>
    <tr>
      <td align="left" valign="middle" class="T">
        <strong>Last Name:</strong>&nbsp;&nbsp;
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLastName" runat="server" Width="240px" 
          CssClass="TextBoxInput" ontextchanged="TextBoxLastName_TextChanged" 
          AutoPostBack="True"></user:TextBoxWithNormalizedLineBreaks><br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Complete_Politician_Add" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="6">
        Complete Addition of Candidate Not in Database
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="6">
        Candidate Name on Ballots:
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="6">
        Check the report above to see if the candidates is already in the database.
        If the politician is not in the database complete the new addition by entering the politician's name exactly as it
        should appear on ballots. You may enter names entirely in upper case and with or
        without quotes and/or parenthesis. When you click the Complete Politician
        Addition Button the politician will either be added to the database or you will 
        get a message confirming the addition of the politician. This will happen when 
        other politicians have the same last name. When the name is accepted you will then be presented with
        a form where you can provide additional information about
        the politician.
      </td>
    </tr>
    <tr id="tr_President_State" runat="server">
      <td  colspan="2">
        <asp:DropDownList ID="DropDownListState" runat="server" Width="320px" CssClass="T">
          <asp:ListItem Value="Select State">Select State</asp:ListItem>
          <asp:ListItem Value="AL">Alabama</asp:ListItem>
          <asp:ListItem Value="AK">Alaska</asp:ListItem>
          <asp:ListItem Value="AZ">Arizona</asp:ListItem>
          <asp:ListItem Value="AR">Arkansas</asp:ListItem>
          <asp:ListItem Value="CA">California</asp:ListItem>
          <asp:ListItem Value="CO">Colorado</asp:ListItem>
          <asp:ListItem Value="CT">Connecticut</asp:ListItem>
          <asp:ListItem Value="DE">Delaware</asp:ListItem>
          <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
          <asp:ListItem Value="FL">Florida</asp:ListItem>
          <asp:ListItem Value="GA">Georgia</asp:ListItem>
          <asp:ListItem Value="HI">Hawaii</asp:ListItem>
          <asp:ListItem Value="ID">Idaho</asp:ListItem>
          <asp:ListItem Value="IL">Illinois</asp:ListItem>
          <asp:ListItem Value="IN">Indiana</asp:ListItem>
          <asp:ListItem Value="IA">Iowa</asp:ListItem>
          <asp:ListItem Value="KS">Kansas</asp:ListItem>
          <asp:ListItem Value="KY">Kentucky</asp:ListItem>
          <asp:ListItem Value="LA">Louisiana</asp:ListItem>
          <asp:ListItem Value="ME">Maine</asp:ListItem>
          <asp:ListItem Value="MD">Maryland</asp:ListItem>
          <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
          <asp:ListItem Value="MI">Michigan</asp:ListItem>
          <asp:ListItem Value="MN">Minnesota</asp:ListItem>
          <asp:ListItem Value="MS">Mississippi</asp:ListItem>
          <asp:ListItem Value="MO">Missouri</asp:ListItem>
          <asp:ListItem Value="MT">Montana</asp:ListItem>
          <asp:ListItem Value="NE">Nebraska</asp:ListItem>
          <asp:ListItem Value="NV">Nevada</asp:ListItem>
          <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
          <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
          <asp:ListItem Value="NM">New Mexico</asp:ListItem>
          <asp:ListItem Value="NY">New York</asp:ListItem>
          <asp:ListItem Value="NC">North Carolina</asp:ListItem>
          <asp:ListItem Value="ND">North Dakota</asp:ListItem>
          <asp:ListItem Value="OH">Ohio</asp:ListItem>
          <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
          <asp:ListItem Value="OR">Oregon</asp:ListItem>
          <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
          <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
          <asp:ListItem Value="SC">South Carolina</asp:ListItem>
          <asp:ListItem Value="SD">South Dakota</asp:ListItem>
          <asp:ListItem Value="TN">Tennessee</asp:ListItem>
          <asp:ListItem Value="TX">Texas</asp:ListItem>
          <asp:ListItem Value="UT">Utah</asp:ListItem>
          <asp:ListItem Value="VT">Vermont</asp:ListItem>
          <asp:ListItem Value="VA">Virginia</asp:ListItem>
          <asp:ListItem Value="WA">Washington</asp:ListItem>
          <asp:ListItem Value="WV">West Virginia</asp:ListItem>
          <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
          <asp:ListItem Value="WY">Wyoming </asp:ListItem>
        </asp:DropDownList>
      </td>
      <td class="T" colspan="4">
        <strong>US Presidential Candidates:</strong> For presidential candidates you 
        need to select the State where the candidate currently resides from the drop 
        down list.&nbsp;
      </td>
    </tr>
    <tr>
      <td class="HSmall" valign="top" width="150">
        First Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top" width="150">
        Middle Name<br />
        or Initial
      </td>
      <td class="HSmall" valign="top" width="150">
        Nickname
      </td>
      <td class="HSmall" valign="top" width="170">
        Last Name<br />
        (required)
      </td>
      <td class="HSmall" valign="top" width="75">
        Suffix:
      </td>
      <td id="tdANCTextAdd" class="HSmall" valign="top" runat="server" width="130">
        DC<br />
        ANC
      </td>
    </tr>
    <tr>
      <td valign="top" class="T">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxFirstAdd" runat="server" CssClass="TextBoxInput" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxMiddleAdd" runat="server" CssClass="TextBoxInput" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxNickNameAdd" runat="server" CssClass="TextBoxInput" Width="120px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxLastAdd" runat="server" CssClass="TextBoxInput" 
          Width="160px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td valign="top">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxSuffixAdd" runat="server" CssClass="TextBoxInput" Width="45px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td id="tdANCTextBoxAdd" valign="top" style="width: 100px" runat="server">
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxAddOnAdd" runat="server" CssClass="TextBoxInput" Enabled="False"
          Width="100px"></user:TextBoxWithNormalizedLineBreaks>
      </td>
    </tr>
    <tr>
      <td class="TColor" colspan="2" valign="top">
        <asp:Button ID="Button_Complete_Politician_Add" runat="server" CssClass="Buttons"
          OnClick="Button_Complete_Politician_Add_Click1" Text="Complete Candidate Addition"
          Width="220px" />
      </td>
      <td class="TBoldColor" colspan="4">
        Be sure to click this button to 
        complete the candidate addition.
      </td>
    </tr>
  </table>
  <!-- Table -->
<table class="tableAdmin" id="Table_Add_More_Candidates" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="2">
        Add More Candidates
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
        <asp:Button ID="Button_AddCandidates" runat="server" CssClass="Buttons" 
          onclick="Button_AddCandidates_Click" Text="Add More Candidates" Width="200px" />
        <br />
      </td>
      <td align="left" valign="top" class="T">
        <br />
        Click this button to add more candidates to this election contest.
      </td>
    </tr>
  </table>
<!-- Table -->
  <table class="tableAdmin" id="Table_Remove_On_Ballot" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="2">
        Remove the Candidate
      </td>
    </tr>
    <tr>
      <td align="left" class="HSmall" colspan="2">
        <asp:Label ID="Label_Remove_Name" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
    </tr>
    <tr>
      <td align="left" valign="top">
        <asp:Button ID="Button_Remove_From_Election" runat="server" CssClass="Buttons" OnClick="Button_Remove_From_Election_Click"
          Text="Remove from Election" Width="200px" />
      </td>
      <td align="left" valign="top" class="T">
        Click this button to remove this candidate for this office contest. The 
        candidate will not be removed from the database, just as a candidte for this 
        office contest..</td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Ballot_Order" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" valign="top" colspan=2>
        Candidate Ballot Order</td>
    </tr>
    <tr>
      <td class="T" valign="top">
        <br />
        <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOrder" runat="server" Width="47px" CssClass="TextBoxInput"
          AutoPostBack="True" OnTextChanged="TextBoxOrder_TextChanged"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td class="T" valign="top">
        <br />
        Enter a whole number as the candidate&#39;s ballot order relative to the other 
        candidates. Then select Enter. The order will automatically be resequenced in 
        increments of 10. The order is only used as the position and is not shown on ballots. 
        When adding candidates,&nbsp; the candidate order will automatically be assigned 
        placing the candidate last. <br />
        <br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Candidate_Info" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H">
        Candidate's Party and Contact Information
      </td>
    </tr>
    <tr>
      <td class="T">
        <br />
        Click this link to be presented with a form to identify the candidate's political party and provide contact information.</td>
    </tr>
    <tr>
      <td class="T">
        <asp:HyperLink ID="HyperLinkCandidateInfo" runat="server" CssClass="HyperLink"
          Target="_politician">Candidate's Party and Contact Information</asp:HyperLink>
        <br />
&nbsp;
        <br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Terminate_Addition" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" colspan="2">
        Terminate 
        Candidate Addition
      </td>
    </tr>
    <tr>
      <td width="300">
        <asp:HyperLink ID="HyperLink_Terminate_Addition" runat="server" CssClass="HyperLink"
          Target="_officecontest">Terminate Candidate Addition</asp:HyperLink>
      </td>
      <td class="T">
        <br />
        You are not obligated to complete the politician addition. Use this link to 
        terminate the addition of this candidate and return to a form where you can 
        continue identifying the office candidates.
        <br />
        <br />
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Running_Mate" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td class="H" colspan="2">
        Running Mate</td>
    </tr>
    <tr>
      <td class="HSmall">
        &nbsp;<asp:Label ID="RunningMate" runat="server" CssClass="TBoldColor"></asp:Label>
      </td>
      <td align="right" class="HSmall">
        <strong></strong>
        <asp:Label ID="RunningMateID" runat="server" CssClass="TSmall"></asp:Label>
      </td>
    </tr>
    <tr>
      <td valign="top" colspan="2" class="T">
        <strong>Add or Change Candidate's Running Mate:</strong> To change or delete the
        running mate of this candidate, enter the running mate's ID in this text box provided.
        Then click anywhere on this form. The running mate ID may be obtained from the report
        when you click the Find Politician ID link on the right.
      </td>
    </tr>
    <tr>
      <td valign="top" class="TBold">
        <strong>Running Mate ID:&nbsp; </strong>&nbsp;<user:TextBoxWithNormalizedLineBreaks ID="TextboxRunningMate"
          runat="server" Width="300px" AutoPostBack="True" OnTextChanged="TextboxRunningMate_TextChanged"
          CssClass="TextBoxInput"></user:TextBoxWithNormalizedLineBreaks>
      </td>
      <td align="left" class="TBold" colspan="1" valign="top">
        <asp:HyperLink ID="HyperlinkPoliticiansList" runat="server" CssClass="HyperLink"
          Target="_politicians" Width="130px"><nobr>Find Politician ID</nobr></asp:HyperLink>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="Table_Edit_Office" cellspacing="0" cellpadding="0"
    border="0" runat="server">
    <tr>
      <td align="left" class="H" colspan="3" valign="top">
        Edit Office
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <br />
        Use the link below if to edit this office including changing the number of 
        office positions or identifying the
        office incumbent(s).
      </td>
    </tr>
    <tr>
      <td align="left" class="T" colspan="3" valign="top">
        <asp:HyperLink ID="HyperLinkOffice" runat="server" CssClass="HyperLink" Target="_office">[HyperLinkOffice]</asp:HyperLink><br />
        &nbsp;
      </td>
    </tr>
  </table>
</form>
</body>
</html>
--%>