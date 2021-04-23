<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignIntroPage.aspx.cs" Inherits="Vote.Admin.DesignIntroPage" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" %>
<%@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc7" %>
<%@ Register Src="Instructions/DesignInstructionsStyleSheets.ascx" TagName="DesignInstructionsStyleSheets"
  TagPrefix="uc8" %>
<%@ Register Src="Instructions/DesignInstructionsControls.ascx" TagName="DesignInstructionsControls"
  TagPrefix="uc9" %>

<%@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements" TagPrefix="uc6" %>

<%@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml" TagPrefix="uc5" %>

<%@ Register Src="Instructions/CustomStyleSheet.ascx" TagName="CustomStyleSheet"
  TagPrefix="user" %>

<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>

<%@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="uc4" %>
<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Design for Intro.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
  <link id="This" type="text/css" rel="stylesheet" runat="server" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
</head>
<body>
  <form id="form1" runat="server">
    <user:LoginBar ID="LoginBar1" runat="server" />
    <user:Banner ID="Banner" runat="server" />
    <uc2:Navbar ID="Navbar" runat="server" />
    <!-- Table1 -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          Design for <span style="color: red">Intro.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" ForeColor="Red"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Politician Introduction Page (Intro.aspx).</td>
      </tr>
    </table>
    <!-- Table -->
    <uc7:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
    <!-- Table -->
    <uc8:DesignInstructionsStyleSheets id="DesignInstructionsStyleSheets1" runat="server" />
    <!-- Table -->
    <uc9:DesignInstructionsControls ID="DesignInstructionsControls1" runat="server" />
    <!-- Table -->
    <uc4:DesignInstructionsSubstitutions ID="DesignInstructionsSubstitutions1" runat="server" />
    <!-- Table -->
    <table id="Table15" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="center" class="H" colspan="2" valign="middle">
          Sample of the Current Design</td>
      </tr>
      <tr>
        <td class="T" valign="middle">
          &nbsp;<asp:HyperLink ID="HyperLinkSamplePage" runat="server" CssClass="HyperLink"
            Target="view">Sample Page</asp:HyperLink>
        </td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample ballot page.</td>
      </tr>
    </table>
    &nbsp;
    <!-- Table Content Voters -->
    <uc6:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table5" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Biographical</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroTop"&gt;&lt;td class="tdIntroBio"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroPoliticianTitle"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdIntroPoliticianName"&gt;Politician&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;tr class="trIntroPoliticianTitle"&gt;&lt;td class="tdIntroPoliticianStatus"&gt;Currently
          Elected..&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;tr class="trIntroPoliticianTitle"&gt;&lt;td class="tdIntroPoliticianStatus"&gt;Election
          Description&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;tr class="trIntroContact"&gt;&lt;td&nbsp; class="tdIntroContactHeading"&gt;Party:&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;td class="tdIntroContactDetail"&gt;&lt;a...&gt;Party&lt;/a&gt;&lt;/td&gt;&lt;/tr&gt;....</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;tr class="trIntroAssignmentsHeader"&gt;&lt;td class="tdIntroAssignmentsHeader"&gt;Legislative
          Assignments&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;tr class="trIntroAssignmentsDetail"&gt;&lt;td class="tdIntroAssignmentsDetail"&gt;Senate
          Committee on..&lt;br&gt;...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder" >
          &lt;/table&gt;&lt;/td&gt;&lt;td class="tdIntroImage"&gt;&lt;img&nbsp; src=... /&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trIntroTop">
        <td align="center" class="tdIntroBio" valign="top">
          <table align="left" border="0" cellpadding="0" cellspacing="0">
            <tr class="trIntroPoliticianTitle">
              <td class="tdIntroPoliticianName" colspan="2">
                <span id="PoliticianName">Barack Obama</span></td>
            </tr>
            <tr class="trIntroPoliticianTitle">
              <td class="tdIntroPoliticianStatus" colspan="2">
                <span id="PoliticianOffice">Currently Elected United States Senator Illinois</span></td>
            </tr>
            <tr class="trIntroPoliticianTitle">
              <td class="tdIntroPoliticianStatus" colspan="2">
                <span id="PoliticianElection"></span>
              </td>
            </tr>
            <tr class="trIntroContact">
              <td class="tdIntroContactHeading">
                Party:</td>
              <td class="tdIntroContactDetail">
                <a id="HyperLinkParty" class="TSmall" href="http://www.democrats.org/" target="_self"
                  title="Democratic Party Website">Democratic Party</a></td>
            </tr>
            <tr class="trIntroAssignments">
              <td class="tdIntroAssignmentsHeading" colspan="2">
                Legislative Assignements</td>
            </tr>
            <tr class="trIntroAssignmentsDetail">
              <td class="tdIntroAssignmentsDetail" colspan="2">
                <span id="LegislativeAssignments">Senate Committee on Foreign Relations<br />
                  Senate Subcommittee on African Affairs</span></td>
            </tr>
          </table>
        </td>
        <td align="right" class="tdIntroImage" valign="top">
          <img id="CandidateImage" border="0" class="CandidateImage" src="/images/CandidatesSmall/ILObamaBarack.jpg" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table7" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Positions and Views Anchors</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroIssueLinksHeading"&gt;&lt;td
          class="tdIntroIssueLinksHeading"&gt;</td>
      </tr>
      <tr>
        <td choff="<>" class="TSmallBorder">
          Barack Obama's Positions and Views...&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroIssueLinks"&gt;&lt;td class="tdIntroIssueLinks"&gt;|&lt;a href=...&gt;Reasons
          &amp; Objectives&lt;/a&gt;|...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trIntroIssueLinksHeading">
        <td class="tdIntroIssueLinksHeading">
          <span id="MyPositionsHeading">Barack Obama's Positions and Views on These Issues:</span>
        </td>
      </tr>
      <tr class="trIntroIssueLinks">
        <td class="tdIntroIssueLinks">
          <span id="MyPositions">| <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=ALLPersonal"
            target="_self" title="Reasons & Objectives">
            <nobr>
              Reasons &amp; Objectives</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSFiscalPolicy"
                target="_self" title="Fiscal Policy">
                <nobr>
                  Fiscal Policy</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSWallStreet"
                    target="_self" title="Wall Street">
                    <nobr>
                      Wall Street</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSBusiness"
                        target="_self" title="Business">
                        <nobr>
                          Business</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSHomelandSecurity"
                            target="_self" title="Homeland Security & War On Terror">
                            <nobr>
                              Homeland Security &amp; War On Terror</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSIraq"
                                target="_self" title="Iraq, Afghanistan & Pakistan">
                                <nobr>
                                  Iraq, Afghanistan &amp; Pakistan</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSIran"
                                    target="_self" title="Iran">
                                    <nobr>
                                      Iran</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSMiddleEast"
                                        target="_self" title="Middle East">
                                        <nobr>
                                          Middle East</nobr></a> | <a href="/PoliticianIssue.aspx?State=IL&Id=ILObamaBarack&Issue=BUSForeignPolicy"
                                            target="_self" title="Foreign Policy">
                                            <nobr>
                                              Foreign Policy</nobr></a> </span>
        </td>
      </tr>
    </table>
    <!-- Table -->
    <table id="tableAdmin" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Profile Heading</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trProfileHeading"&gt;&lt;td
          class="tdProfileHeading"&gt;Profile for..&lt;/td&gt;&lt;/tr&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trProfileHeading">
        <td class="tdProfileHeading">
          <span id="ProfileHead">Profile for Barack Obama</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table8" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td class="H">
          Profile</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroProfileHeading"&gt;&lt;td class="tdIntroProfileHeading"&gt;General:
          (politi..&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<>" class="TSmallBorder">
          &lt;tr class="trIntroProfileDetail"&gt;&lt;td class="tdIntroProfileDetail"&gt;This
          is a defining moment...&lt;td&gt;&lt;/tr&gt;...&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trIntroProfileHeading">
        <td class="tdIntroProfileHeading">
          General: (political statement of goals, objectives, views, philosophies)</td>
      </tr>
      <tr class="trIntroProfileDetail">
        <td class="tdIntroProfileDetail">
          <span id="General">This is a defining moment. Our nation is at war. Our planet is
            in peril. Our American Dream is slipping away. Because of these challenges, Americans
            are listening to what we say in this election. This is our chance to forge a new
            majority of Democrats, Independents and Republicans committed to our common purpose
            as Americans.&nbsp; </span>
        </td>
      </tr>
      <tr class="trIntroProfileHeading">
        <td class="tdIntroProfileHeading">
          Personal: (Gender, age, marital status, spouse's name and age, children's name and
          ages, home town, current residence)</td>
      </tr>
      <tr class="trIntroProfileDetail">
        <td class="tdIntroProfileDetail">
          <span id="Personal">Barack Obama was born on August 4th, 1961, in Hawaii to Barack
            Obama, Sr. and Ann Dunham. Obama is especially proud of being a husband and father
            of two daughters, Malia, 7 and Sasha, 4. Obama and his wife, Michelle, married in
            1992 and live on Chicago ’s South Side. It has been the rich and varied experiences
            of Barack Obama's life - growing up in different places with people who had differing
            ideas - that have animated his political journey.</span></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table10" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Profile Footer</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trIntroFooter"&gt;&lt;td class="tdIntroFooter"&gt;All
          information about&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
      <tr>
        <td align="left" class="T" colspan="2" valign="top">
          <strong>Additional Substitutions for this page element:</strong></td>
      </tr>
      <tr>
        <td class="TSmall" colspan="2" valign="top">
          -<strong>-Candidate Substitutions--<br />
          </strong>[[POLITICIAN]] = Candidate's Name</td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table11" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr class="trIntroFooter">
        <td class="tdIntroFooter">
          <asp:Label ID="LabelInstructionsIntro" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" class="BorderLight">
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsIntro" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsIntro_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml1" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsIntro" runat="server" TextMode="MultiLine" Rows="1"
            Height="150px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsIntro" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsIntro_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsIntro" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsIntro_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsIntro" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsIntro_Click" /></td>
      </tr>
    </table>
    <user:CustomStyleSheet ID="CustomStyleSheet1" runat="server" />
    <!--table -->
    <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0">
      <tr>
        <td align=left>
          <input id="FileStyleSheet" type="file" name="PictureFile" runat="server" size="65" class="TextBoxInput" >
          <input class="Buttons" id="ButtonUploadStyleSheet" type="submit" value="Upload"
            name="ButtonUpload" runat="server" onserverclick="ButtonUploadStyleSheet_ServerClick1" style="width: 150px"></td>
      </tr>
      </table>
      
    <!--table -->
    <table class="tableAdmin" id="Table20" cellspacing="0" cellpadding="0">
      <tr>
        <td colspan="1">
          <asp:Button ID="ButtonDeleteCustomStyleSheet1" runat="server" CssClass="Buttons" OnClick="ButtonDeleteCustomStyleSheet1_Click"
            Text="Delete" Width="150px" />&nbsp;</td>
        <td colspan="1" class="T" width="100%">
          Click this button to delete the current custom style sheet for the page elements
          above.</td>
      </tr>
    </table>
    <!--table -->
    <table class="tableAdmin" id="Table22" cellspacing="0" cellpadding="0">
      <tr>
        <td>
        </td>
        <td class="HSmall" align="center">
          <asp:Button ID="ButtonUpdateCustomStyleSheet" runat="server" CssClass="Buttons" Text="Update"
            Width="150px" OnClick="ButtonUpdateCustomStyleSheet_Click" /></td>
      </tr>
      <tr>
        <td class="H">
          Default Style Sheet</td>
        <td class="H">
          Custom Style Sheet</td>
      </tr>
      <tr>
        <td>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDefaultStyleSheet" runat="server" TextMode="MultiLine" 
            ReadOnly="True" Width="350px" Height="3000px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
        </td>
        <td>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCustomStyleSheet" runat="server"  Height="3000px"
            TextMode="MultiLine" Width="350px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>

  </form>
</body>
</html>
--%>