<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SiteMap.aspx.cs" Inherits="Vote.Master.SitemapPage" %>

<%//@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%//@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%//@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>
<%//@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<%--
<!--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>SiteMap Page</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <div>
      < user:LoginBar ID="LoginBar1" runat="server" />
      < user:Banner ID="Banner" runat="server" />
      < uc2:Navbar ID="Navbar" runat="server" />
      <table class="tableAdmin" id="Top" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="left" class="HLarge">
            Sitemaps</td>
        </tr>
        <tr>
          <td align="left">
            <asp:Label ID="Msg" runat="server"></asp:Label></td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table7" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="H">
            Build a Sitemap for All 52 Domains</td>
        </tr>
        <tr>
          <td class="TSmallColor">
            To implemet any code changes on the scheduled sitemap runs on Vote-1.votecolo the
            production dll need to be compiled and copied to the production server. Follow instruction
            found at: D:\$_Vote\_SOPs\400 Misc Instructions\Sitemaps Updating of Scheduled Runs
            on Production Servers.htm</td>
        </tr>
        <tr>
          <td class="T">
            Click this button to create a sitemap for each of the 52 domains using the settings
            shown below.
          </td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="Button_Create_Sitemaps" runat="server" OnClick="Button_Create_Sitemaps_Click"
              Text="Build All 52 Sitemaps" Width="200px" CssClass="Buttons" /></td>
        </tr>
        <tr>
          <td class="T">
          </td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="H">
            Build a Sitemap for a Single Domain</td>
        </tr>
        <tr>
          <td class="T">
            Click this button to create a single sitemap for the domain selected below.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:Button ID="Button_Create_Sitemap" runat="server" CssClass="Buttons" OnClick="Button_Create_Sitemap_Click"
              Text="Build Single Sitemap" Width="200px" /></td>
        </tr>
        <tr>
          <td  class="H">
            Domain Settings or a Single Sitemap</td>
        </tr>
        <tr>
          <td class="T" >
            Select a doamin from dropdown list to obtain and/or change the domain's settings.</td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="TBold">
            Select Domain</td>
          <td class="TBold" align="center">
            Code</td>
          <td class="TBold" align="center">
            Domain</td>
          <td class="TBold" align="center">
            Sitemap</td>
          <td class="TBold" align="center">
            Last Created</td>
        </tr>
        <tr>
          <td class="T" >
            <asp:DropDownList ID="DropDownList_Domain" runat="server" AutoPostBack="True" CssClass="TBold"
              OnSelectedIndexChanged="DropDownListState_SelectedIndexChanged" Width="130px">
              <asp:ListItem Value="US" Selected="True">Vote-USA.org</asp:ListItem>
              <asp:ListItem Value="AL">Vote-AL.org</asp:ListItem>
              <asp:ListItem Value="AK">Vote-AK.org</asp:ListItem>
              <asp:ListItem Value="AZ">Vote-AZ.org</asp:ListItem>
              <asp:ListItem Value="AR">Vote-AR.org</asp:ListItem>
              <asp:ListItem Value="CA">Vote-CA.org</asp:ListItem>
              <asp:ListItem Value="CO">Vote-CO.org</asp:ListItem>
              <asp:ListItem Value="CT">Vote-CT.org</asp:ListItem>
              <asp:ListItem Value="DE">Vote-DE.org</asp:ListItem>
              <asp:ListItem Value="DC">Vote-DC.org</asp:ListItem>
              <asp:ListItem Value="FL">Vote-FL.org</asp:ListItem>
              <asp:ListItem Value="GA">Vote-GA.org</asp:ListItem>
              <asp:ListItem Value="HI">Vote-HI.org</asp:ListItem>
              <asp:ListItem Value="ID">Vote-ID.org</asp:ListItem>
              <asp:ListItem Value="IL">Vote-IL.org</asp:ListItem>
              <asp:ListItem Value="IN">Vote-IN.org</asp:ListItem>
              <asp:ListItem Value="IA">Vote-IA.org</asp:ListItem>
              <asp:ListItem Value="KS">Vote-KS.org</asp:ListItem>
              <asp:ListItem Value="KY">Vote-KY.org</asp:ListItem>
              <asp:ListItem Value="LA">Vote-LA.org</asp:ListItem>
              <asp:ListItem Value="ME">Vote-ME.org</asp:ListItem>
              <asp:ListItem Value="MD">Vote-MD.org</asp:ListItem>
              <asp:ListItem Value="MA">Vote-MA.org</asp:ListItem>
              <asp:ListItem Value="MI">Vote-MI.org</asp:ListItem>
              <asp:ListItem Value="MN">Vote-MN.org</asp:ListItem>
              <asp:ListItem Value="MS">Vote-MS.org</asp:ListItem>
              <asp:ListItem Value="MO">Vote-MO.org</asp:ListItem>
              <asp:ListItem Value="MT">Vote-MT.org</asp:ListItem>
              <asp:ListItem Value="NE">Vote-NE.org</asp:ListItem>
              <asp:ListItem Value="NV">Vote-NV.org</asp:ListItem>
              <asp:ListItem Value="NH">Vote-NH.org</asp:ListItem>
              <asp:ListItem Value="NJ">Vote-NJ.org</asp:ListItem>
              <asp:ListItem Value="NM">Vote-NM.org</asp:ListItem>
              <asp:ListItem Value="NY">Vote-NY.org</asp:ListItem>
              <asp:ListItem Value="NC">Vote-NC.org</asp:ListItem>
              <asp:ListItem Value="ND">Vote-ND.org</asp:ListItem>
              <asp:ListItem Value="OH">Vote-OH.org</asp:ListItem>
              <asp:ListItem Value="OK">Vote-OK.org</asp:ListItem>
              <asp:ListItem Value="OR">Vote-OR.org</asp:ListItem>
              <asp:ListItem Value="PA">Vote-PA.org</asp:ListItem>
              <asp:ListItem Value="RI">Vote-RI.org</asp:ListItem>
              <asp:ListItem Value="SC">Vote-SC.org</asp:ListItem>
              <asp:ListItem Value="SD">Vote-SD.org</asp:ListItem>
              <asp:ListItem Value="TN">Vote-TN.org</asp:ListItem>
              <asp:ListItem Value="TX">Vote-TX.org</asp:ListItem>
              <asp:ListItem Value="UT">Vote-UT.org</asp:ListItem>
              <asp:ListItem Value="VT">Vote-VT.org</asp:ListItem>
              <asp:ListItem Value="VA">Vote-VA.org</asp:ListItem>
              <asp:ListItem Value="WA">Vote-WA.org</asp:ListItem>
              <asp:ListItem Value="WV">Vote-WV.org</asp:ListItem>
              <asp:ListItem Value="WI">Vote-WI.org</asp:ListItem>
              <asp:ListItem Value="WY">Vote-WY.org</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;
          </td>
          <td class="T" align="center">
            <asp:Label ID="Label_Domain_Code" runat="server" CssClass="TBoldColor" Width="35px">Code</asp:Label></td>
          <td class="T"  align="center">
            <asp:Label ID="Label_Domain" runat="server" CssClass="TBoldColor" Width="100px">Domain</asp:Label></td>
          <td class="T" align="center">
            <asp:Label ID="Label_Sitemap" runat="server" CssClass="TBoldColor">Sitemap</asp:Label></td>
          <td class="T" width="100%" align="center">
            <asp:Label ID="Label_Last_Created" runat="server" CssClass="TBoldColor">Created</asp:Label></td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="HSmall">
            Priority 
            Values</td>
        </tr>
        <tr>
          <td class="T">
            Describes the priority of a URL relative to all the other URLs on the site. This
            priority can range from 1.0 (extremely important) to 0.1 (not important at all).
            To avoid having to enter the decimal point acceptable values are between 1 - 10.<br />
            Normally set to '<strong>0</strong>' to automatically compute the priority as follows:<br />
            <strong>Default.aspx: </strong>if upcoming election<strong>: 10; </strong>otherwise<strong>
              4<br />
              Issue.aspx Pages&amp;Issue=ALLBio: 10 </strong>(most important page comparing
            candidate bios) <strong>=BXXxx: 8</strong> (all other issue comparisons)<br />
            <strong> Intro.aspx, PoliticianIssue.aspx: </strong>
            depends when the page content was last changed as follows.<br />
            <strong>10:</strong> less than 1 day; <strong>9:</strong> 2- 3 days; <strong>8:</strong>
            4 - 7 days; <strong>7:</strong> 8 - 30 days; <strong>6:</strong> 31 60 days; <strong>
              5</strong>: 61 - 120 days; <strong>4</strong>: 12- 365 days: <strong>3</strong>:
            366 - 730 days (1-2 years): <strong>2</strong>: 731 - 1460 days (2 -4 years): <strong>
              1</strong>: over 1460 days.<br />
            <strong>Upcoming Election.aspx: 9; Previous Election.aspx: 4</strong><br />
            <strong>Officials.aspx: 8</strong>: less than 120 days; <strong>3</strong> otherwise<br />
            <strong>Ballots.aspx: 3</strong>: if less than 120 days; <strong>1</strong> otherwise</td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table6" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="HSmall" colspan="2">
            Factors to Modify Priority Computation Above <strong>NOT USED ANYMORE</strong>:</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            Previously used to compute priority
            as (Page Answers / Factor) usng the value in the Factor Textbox below. A Factor
            of 1 will set priority to the number of answers. but the Factor will never exceed
            9. Could be reinstated later.</td>
        </tr>
        <tr>
          <td class="T">
            For upcoming election's <strong>Issue Pages:</strong>
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Factor" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              Font-Bold="True" OnTextChanged="TextBox_Factor_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td class="T">
            <asp:Button ID="Button_Set_Or_Compute_Priority" runat="server" CssClass="Buttons"
              Text="All" OnClick="Button_Set_Or_Compute_Priority_Click" Height="21px" /></td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table8" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="HSmall" style="height: 21px">
            Frequency Radio Buttons</td>
        </tr>
        <tr>
          <td class="T">
            Normally set to '<strong>a</strong>' to automatically compute the frequency, depending
            on the when the content of the page was last changed as follows.<br />
            <strong>Hourly:</strong> less than 1 day; <strong>Daily:</strong> 2- 3 days; <strong>
              Weekly:</strong> 4 - 15 days; <strong>Montly:</strong> 16 - 60 days; <strong>Yearly:</strong>
            over 60 days<br />
            Any other selection overrides this automatic computation.</td>
        </tr>
        <tr>
          <td class="HSmall">
            The [All] Buttons</td>
        </tr>
        <tr>
          <td class="T">
            The All Buttons below will set all the domians to the value currently showing for
            the particular domain currently selected. For example, if all the Incluce Checkboxes
            are unchecked, for any domain, and then the 7 All Buttons in the table below are
            clicked, no sitemaps will be created.</td>
        </tr>
        <tr>
          <td class="HSmall">
            Vote-USA.org Domain Sitemap is Created Special</td>
        </tr>
        <tr>
          <td class="T">
            Election.aspx - only US President, US Senate and US House elections are created<br />
            Officials.aspx - only 3 urls created for US President, US Senate and US House<br />
            Intro.aspx - only 1 for US President<br />
            PoliticianIssue.aspx - 1 for each issue for the US President, no US Senate or US
            House officials created.<br />
            Issue.aspx - Normally 0 unless there is a National Presidential Primary (NationalPartyCode
            = C)<br />
            Ballot.aspx - 0 created</td>
        </tr>
        <tr>
          <td class="TSmallColor">
            Ballots have been removed from the sitemaps because they can produce duplicate 
            content. For example, in the special DC election all 8 wards would produce the 
            same ballot because all 8 wards only have the At-Large Council Seat contest.</td>
        </tr>
        <tr>
          <td class="H">
            Page Urls and Parameters in the Xml Sitemaps</td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td align="center" class="tdReportDetailHeading">
            Page</td>
          <td class="tdReportDetailHeading" align="center">
            Priority<br />
            0=automatic<br />
            (1 - 10)=manual</td>
          <td class="tdReportDetailHeading" align="center">
            Frequency<br />
            a=automatic h=hourly d=daily<br />
            w=weekly m=mothly y=yearly 
            <br />
            n=never (i.e. archive pages)</td>
          <td align="center" class="tdReportDetailHeading" >
            &nbsp;All&nbsp;</td>
          <td class="tdReportDetailHeading" align="right">
            Urls</td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Default" runat="server" CssClass="TBold" Text="Default.aspx"></asp:Label><br />
            Home Page</td>
          <td class="tdReportDetail" align="center">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Default" runat="server" CssClass="TextBoxInput"
              Width="20px" AutoPostBack="True" OnTextChanged="TextBox_Priority_Default_TextChanged">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td class="tdReportDetail" align="center">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Default" runat="server" CssClass="TSmall"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Default_SelectedIndexChanged"
              RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td class="tdReportDetail" align="center" >
            <asp:Button ID="Button_All_Default" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_All_Default_Click" Height="21px" /></td>
          <td class="tdReportDetail" align="right">
            <asp:Label ID="Label_Urls_Default" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Election" runat="server" CssClass="TBold" Text="Election.aspx"></asp:Label><br />
            Election Directories</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Election" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              OnTextChanged="TextBox_Priority_Election_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Election" runat="server" CssClass="TBold"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Election_SelectedIndexChanged"
              RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail" >
            <asp:Button ID="Button_All_Election" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_All_Election_Click" Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_Election" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Officials" runat="server" CssClass="TBold" Text="Officials.aspx"></asp:Label><br />
            Elected Officials Directories</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Officials" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              OnTextChanged="TextBox_Priority_Officials_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Officials" runat="server" CssClass="TBold"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Officials_SelectedIndexChanged"
              RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail">
            <asp:Button ID="Button_All_Officials" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_All_Officials_Click" Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_Officials" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Intro" runat="server" CssClass="TBold" Text="Intro.aspx"></asp:Label><br />
            Politician Introduction</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Intro" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              OnTextChanged="TextBox_Priority_Intro_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Intro" runat="server" CssClass="TBold"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Intro_SelectedIndexChanged" RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail" >
            <asp:Button ID="Button_All_Intro" runat="server" CssClass="Buttons" Text="All" OnClick="Button_All_Intro_Click"
              Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_Intro" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_PoliticianIssue" runat="server" CssClass="TBold" Text="PoliticianIssue.aspx"></asp:Label><br />
            Politician on Issue</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_PoliticianIssue" runat="server" AutoPostBack="True"
              CssClass="TextBoxInput" OnTextChanged="TextBox_Priority_PoliticianIssue_TextChanged"
              Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_PoliticianIssue" runat="server"
              CssClass="TBold" Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_PoliticianIssue_SelectedIndexChanged"
              RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail" >
            <asp:Button ID="Button_All_PoliticianIssue" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_All_PoliticianIssue_Click" Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_PoliticianIssue" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Issue" runat="server" CssClass="TBold" Text="Issue.aspx"></asp:Label><br />
            Issue for Election Contest</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Issue" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              OnTextChanged="TextBox_Priority_Issue_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Issue" runat="server" CssClass="TBold"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Issue_SelectedIndexChanged" RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail" >
            <asp:Button ID="Button_All_Issue" runat="server" CssClass="Buttons" Text="All" OnClick="Button_All_Issue_Click"
              Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_Issue" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
            <asp:Label ID="Label_Page_Ballot" runat="server" CssClass="TBold" Text="Ballot.aspx"></asp:Label><br />
            Ballots</td>
          <td align="center" class="tdReportDetail">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Priority_Ballot" runat="server" AutoPostBack="True" CssClass="TextBoxInput"
              OnTextChanged="TextBox_Priority_Ballot_TextChanged" Width="20px">< /user:TextBoxWithNormalizedLineBreaks></td>
          <td align="center" class="tdReportDetail">
            <asp:RadioButtonList ID="RadioButtonList_Frequency_Ballot" runat="server" CssClass="TBold"
              Font-Bold="True" RepeatDirection="Horizontal" Width="100px" AutoPostBack="True"
              OnSelectedIndexChanged="RadioButtonList_Frequency_Ballot_SelectedIndexChanged"
              RepeatColumns="7">
              <asp:ListItem Selected="True" Value="automatic">a</asp:ListItem>
              <asp:ListItem Value="hourly">h</asp:ListItem>
              <asp:ListItem Value="daily">d</asp:ListItem>
              <asp:ListItem Value="weekly">w</asp:ListItem>
              <asp:ListItem Value="monthly">m</asp:ListItem>
              <asp:ListItem Value="yearly">y</asp:ListItem>
              <asp:ListItem Value="never">n</asp:ListItem>
            </asp:RadioButtonList></td>
          <td align="center" class="tdReportDetail" >
            <asp:Button ID="Button_All_Ballot" runat="server" CssClass="Buttons" Text="All" OnClick="Button_All_Ballot_Click"
              Height="21px" /></td>
          <td align="right" class="tdReportDetail">
            <asp:Label ID="Label_Urls_Ballot" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
          </td>
          <td align="right" class="TBold" colspan="2">
            Total Urls This Domain:
          </td>
          <td align="right" class="tdReportDetail" colspan="2">
            <asp:Label ID="Label_Urls_Total" runat="server" CssClass="TBoldColor" Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
          </td>
          <td align="right" class="TBold" colspan="2">
            Total Run Time This Domain:
          </td>
          <td align="left" class="tdReportDetail" colspan="2">
            <asp:Label ID="Label_Run_Time" runat="server" CssClass="TBoldColor" Text="Time"></asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
          </td>
          <td align="right" class="TBold" colspan="2">
            Total Urls All Domians:</td>
          <td align="right" class="tdReportDetail" colspan="2">
            <asp:Label ID="Label_Urls_Total_All_Domains" runat="server" CssClass="TBoldColor"
              Font-Bold="True">Urls</asp:Label></td>
        </tr>
        <tr>
          <td align="center" class="tdReportDetail">
          </td>
          <td align="right" class="TBold" colspan="2">
            Total Run Time All Domains:</td>
          <td align="left" class="tdReportDetail" colspan="2">
            <asp:Label ID="Label_Run_Time_All_Domains" runat="server" CssClass="TBoldColor" Text="Time"></asp:Label></td>
        </tr>
      </table>
      <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0" border="0">
        <tr>
          <td class="H" colspan="2">
            Parameters Independent of Upcoming Viewable Electionsable Elections</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2" style="height: 20px">
            Politician Intro Page Urls: (Intro.aspx)</td>
        </tr>
        <tr>
          <td class="T" colspan="2" style="height: 13px">
            <strong>Urls Generated: </strong>When there is a viewable upcoming election, Intro.aspx
            urls are generated only for the candidates in that election or elections. Otherwise,
            urls are created only for the currently elected officials.</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            <strong>Requirement to Generate a Url:</strong> All requirements of the checkboxes
            checked, must be met to generate an Intro.aspx &nbsp;url. If none are checked ALL
            Intro.aspx Pages are included, even if there is no information or picture. This
            selection applies whether uls are being generated for upcoming election(s) or elected
            officials.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:CheckBox ID="CheckBox_Must_Have_Picture" runat="server" AutoPostBack="True"
              CssClass="TBold" OnCheckedChanged="CheckBox_Must_Have_Picture_CheckedChanged" Text="Picture must be available" /><br />
            <asp:CheckBox ID="CheckBox_Must_Have_Statement" runat="server" AutoPostBack="True"
              CssClass="TBold" OnCheckedChanged="CheckBox_Must_Have_Statement_CheckedChanged"
              Text="General Statement must be available" /></td>
          <td class="T">
            <asp:Button ID="Button_Must_Have_Picture" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_Must_Have_Picture_Click" Height="21px" /></td>
        </tr>
        <tr>
          <td class="H" colspan="2">
          </td>
        </tr>
        <tr>
          <td class="H" colspan="2">
            Parameters When there IS an Upcoming Viewable Election</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2" style="height: 20px">
            Election Directories Included (Election.aspx Pages)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2">
            ONLY include the the single upcoming viewable election directory.</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Elected Officials Directory Urls: (ElectedOfficials.aspx Pages)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2">
            NONE INCLUDED</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Politician Views on a Issue Page Urls: (PoliticianIssue.aspx)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2" style="height: 13px">
            NONE INCLUDED</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Comparison of Candidate Views for an Office on an Issue (Issue.aspx)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2" style="height: 13px">
            ONLY include the politicians in the single upcoming election.</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            Requirement(s): to generate an Issue.aspx Page URL. All requirements must be met.
            If none are checked ALL Issue.aspx Pages are included, even if there is only one
            candidate and no answers.</td>
        </tr>
        <tr>
          <td class="TBold">
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Minium_Candidates_Per_Page" runat="server" AutoPostBack="True"
              CssClass="TextBoxInput" Width="20px" OnTextChanged="TextBox_Minium_Candidates_Per_Page_TextChanged">< /user:TextBoxWithNormalizedLineBreaks>
            Minimum Number of Candidates Running for the Office<br />
            < user:TextBoxWithNormalizedLineBreaks ID="TextBox_Minium_Answers_Per_Page" runat="server" CssClass="TextBoxInput"
              Width="20px" OnTextChanged="TextBox_Minium_Answers_Per_Page_TextChanged" AutoPostBack="True">< /user:TextBoxWithNormalizedLineBreaks>
            Minimun Number of Answers from all Candidates Running for the Office</td>
          <td class="TBold">
            <asp:Button ID="Button_Minimum_Candidates" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_Minimum_Candidates_Click" Height="21px" /></td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            Whether to set or compute the priority for Issue.aspx pages. If compute priority
            is selected then it will compute the priority as (Page Answers / Factor) usng the
            value in the Factor Textbox. 1 will set priority to the number of answers, but will
            never exceed 9.</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Ballot Page Urls: (Ballot.aspx)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2">
            All ballots for the single upcoming election.</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            Ballot Pages are created using the Ballots Table consisting of 14,055 different
            ballots, per election that can be created depending on the State, US House District,
            State Senate District, State House District and County. A State could have between
            250 and 500 possible Ballot Urls generated. And Ballot Pages are ONLY generated
            if office contests are in every unique ballot.</td>
        </tr>
        <tr>
          <td class="H" colspan="2">
            Parameters When there is NO Upcoming Viewable Election
          </td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Election Directories Included (Election.aspx Pages)</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            The first radio button will include only the the last election. If the last election
            is a primary all primaries on that day are included. The second button will include
            the last general election and all subsequent elections.</td>
        </tr>
        <tr>
          <td class="T" style="height: 12px">
            <asp:RadioButtonList ID="RadioButtonList_Election_Directories" runat="server" AutoPostBack="True"
              CssClass="TBold" Font-Bold="True" OnSelectedIndexChanged="RadioButtonList_Election_Directories_SelectedIndexChanged">
              <asp:ListItem Selected="True" Value="Last">Last Previous Election(s)</asp:ListItem>
              <asp:ListItem Value="LastG">All LAST Previous Election(s) Since General Election</asp:ListItem>
              <asp:ListItem Value="All">ALL Previous Elections</asp:ListItem>
            </asp:RadioButtonList></td>
          <td class="T" style="height: 12px">
            <asp:Button ID="Button_Election_Directories" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_Election_Directories_Click" Height="21px" /></td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Elected Officials Directories (ElectedOfficials.aspx Pages)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2" style="height: 13px">
            ALWAYS INCLUDED</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Politician Views on a Issue Pages (PoliticianIssue.aspx)</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
            The first radio button will include only the the last election. If the last election
            is a primary all primaries on that day are included. The second button will include
            the last general election and all subsequent elections.</td>
        </tr>
        <tr>
          <td class="T">
            <asp:RadioButtonList ID="RadioButtonList_Politician_Elections" runat="server" AutoPostBack="True"
              CssClass="TBold" Font-Bold="True" OnSelectedIndexChanged="RadioButtonList_Politician_Elections_SelectedIndexChanged">
              <asp:ListItem Selected="True" Value="Last">Last Previous Election(s)</asp:ListItem>
              <asp:ListItem Value="LastG">All LAST Previous Election(s) Since General Election</asp:ListItem>
              <asp:ListItem Value="All">ALL Previous Elections</asp:ListItem>
            </asp:RadioButtonList></td>
          <td class="T">
            <asp:Button ID="Button_Politician_Elections" runat="server" CssClass="Buttons" Text="All"
              OnClick="Button_Politician_Elections_Click" Height="21px" /></td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Comparison of Candidate Views for an Office on an Issue (Issue.aspx)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2">
            NONE INCLUDED</td>
        </tr>
        <tr>
          <td class="HSmall" colspan="2">
            Ballot Pages (Ballot.aspx)</td>
        </tr>
        <tr>
          <td class="TBold" colspan="2" style="height: 13px">
            NONE INCLUDED</td>
        </tr>
      </table>
    </div>
  </form>
</body>
</html>
-->
 --%>