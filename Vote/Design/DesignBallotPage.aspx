<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignBallotPage.aspx.cs" Inherits="Vote.Admin.DesignBallotPage" %>
<%--@ Import Namespace="Vote" --%>

<html><head><title></title></head><body></body></html>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="Vote" --%>
<%--@ Register Src="Instructions/DesignInstructionsWarning.ascx" TagName="DesignInstructionsWarning"
  TagPrefix="uc7" --%>
<%--@ Register Src="Instructions/DesignInstructionsStyleSheets.ascx" TagName="DesignInstructionsStyleSheets"
  TagPrefix="uc8" --%>
<%--@ Register Src="Instructions/DesignInstructionsControls.ascx" TagName="DesignInstructionsControls"
  TagPrefix="uc9" --%>

<%--@ Register Src="../Instructions/DesignPageElements.ascx" TagName="DesignPageElements" TagPrefix="uc6" --%>

<%--@ Register Src="../Instructions/DesignTextOrHtml.ascx" TagName="DesignTextOrHtml" TagPrefix="uc5" --%>

<%--@ Register Src="Instructions/CustomStyleSheet.ascx" TagName="CustomStyleSheet"
  TagPrefix="user" --%>

<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>

<%--@ Register Src="../Instructions/DesignInstructionsSubstitutions.ascx" TagName="DesignInstructionsSubstitutions"
  TagPrefix="uc4" --%>
<%--@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" --%>

<%-- 
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="Head1" runat="server">
  <title>Design for Ballot.aspx</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/All.css" type="text/css" rel="stylesheet" />
  <link id="All" type="text/css" rel="stylesheet" runat="server" />
  <link href="/css/Ballot.css" type="text/css" rel="stylesheet" />
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
      <!-- Table -->
    <table class="tableAdmin" id="Table1" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" class="HLarge">
          Design for <span style="color: red">Ballot.aspx</span> for Design Code:
          <asp:Label ID="LabelDesignCode" runat="server" CssClass="HLargeColor"></asp:Label></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="T">
          The page elements on this form are only for the Ballot Page (Ballot.aspx).</td>
      </tr>
    </table>
      <!-- Table -->
    <uc7:DesignInstructionsWarning ID="DesignInstructionsWarning1" runat="server" />
      <!-- Table -->
    <uc8:DesignInstructionsStyleSheets id="DesignInstructionsStyleSheets1" runat="server" />
      <!-- Table -->
    <uc9:DesignInstructionsControls ID="DesignInstructionsControls1" runat="server" />
    <!-- Table -->
    &nbsp;
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
            Target="view" >Sample Page</asp:HyperLink>
        </td>
        <td class="T" colspan="1" valign="middle" width="100%">
          &nbsp;Click this button to view the design of a sample ballot page.</td>
      </tr>
    </table>
    <!-- Table -->
    <uc6:DesignPageElements ID="DesignPageElements1" runat="server" />
    <!-- Table -->
    <table id="Table9" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Page Title</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trBallotTitle"&gt;&lt;td class="tdBallotTitle"&gt;Sample
          Ballot...&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdBallotSubTitle"&gt;for Address...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
  <table class="tableAdmin" cellspacing="0" border="0">
    <tr class="trTitle">
      <td class="tdBallotTitle">
        Sample Ballot<br>
        November 4, 2008 California General Election</td>
      <td class="tdBallotSubTitle">
        for Address in:<br>
        US House District 53 California<br>
        California Senate District 39<br>
        California House District 76<br>
        San Diego County</td>
    </tr>
    </table>
    <!-- Table -->
    <table id="Table7" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Upcoming Election Ballot Instructions</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trBallotInstruction"&gt;&lt;td
          class="tdBallotInstruction"&gt;This sample ballot...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table5" cellspacing="0" cellpadding="0">
      <tr class="trBallotInstruction">
        <td class="tdBallotInstruction">
          <asp:Label ID="LabelInstructionsUpcomingElectionBallot" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsUpcomingElectionBallot" runat="server"
            AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsUpcomingElectionBallot_SelectedIndexChanged"
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
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsUpcomingElectionBallot" runat="server" TextMode="MultiLine" Rows="1"
            Height="300px" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsUpcomingElectionBallot" runat="server" CssClass="Buttons"
            Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsUpcomingElectionBallot_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsUpcomingElectionBallot" runat="server" CssClass="Buttons"
            Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsUpcomingElectionBallot_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsUpcomingElectionBallot" runat="server" CssClass="Buttons"
            Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsUpcomingElectionBallot_Click" /></td>
      </tr>
    </table>
    <!-- Table -->
    <table id="Table6" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Previous Election Ballot Instructions</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trBallotInstruction"&gt;&lt;td
          class="tdBallotInstruction"&gt;This sample ballot...&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table14" cellspacing="0" cellpadding="0">
      <tr class="trBallotInstruction">
        <td class="tdBallotInstruction">
          <asp:Label ID="LabelInstructionsPreviousElectionBallotPage" runat="server"></asp:Label></td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table3" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="BorderLight">
          <asp:RadioButtonList ID="RadioButtonListIsTextInstructionsPreviousElectionBallotPage"
            runat="server" AutoPostBack="True" CssClass="RadioButtons" 
            OnSelectedIndexChanged="RadioButtonListIsTextInstructionsPreviousElectionBallotPage_SelectedIndexChanged"
            RepeatDirection="Horizontal">
            <asp:ListItem>Text</asp:ListItem>
            <asp:ListItem>HTML</asp:ListItem>
          </asp:RadioButtonList></td>
        <td align="left" width="100%" class="T">
          <uc5:DesignTextOrHtml ID="DesignTextOrHtml2" runat="server" />
          </td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxInstructionsPreviousElectionBallotPage" runat="server" Height="200px"
            TextMode="MultiLine" Width="930px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
      <tr>
        <td align="left">
          &nbsp;&nbsp; &nbsp;
          <asp:Button ID="ButtonGetDefaultInstructionsPreviousElectionBallotPage" runat="server"
            CssClass="Buttons" Text="Get Default Design" Width="150px" 
            OnClick="ButtonGetDefaultInstructionsPreviousElectionBallotPage_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonClearCustomInstructionsPreviousElectionBallotPage" runat="server"
            CssClass="Buttons" Text="Clear Custom Design" Width="150px" 
            OnClick="ButtonClearCustomInstructionsPreviousElectionBallotPage_Click" />&nbsp; &nbsp;
          <asp:Button ID="ButtonSubmitCustomInstructionsPreviousElectionBallotPage" runat="server"
            CssClass="Buttons" Text="Submit Custom Design" Width="160px" 
            OnClick="ButtonSubmitCustomInstructionsPreviousElectionBallotPage_Click" /></td>
      </tr>
    </table>
    <!--table -->
    <table id="Table10" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Ballot Contests - 3 Per Row</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trBallotOfficeContestsRowOfThree"&gt;&lt;td class="tdBallotOfficeContest"&gt;</td>
      </tr>
      <tr class="trBallotOfficeContestsRowOfThree">
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;table Class="tableBallotOfficeContest"&gt;&lt;tr Class="trBallotOfficeHeading"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr&gt;&lt;td Class="tdBallotOfficeHeading" colspan="2"&gt;&lt;a href=...&lt;/a&gt;&lt;br&gt;&lt;span
          class=OfficesNote&gt;US.Pres..&lt;/a&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;span Class="BallotOfficesVoteFor"&gt;Vote for ONE&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr Class="trBallotCandidate"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td Class="tdBallotCandidate"&gt;&lt;a href=...&gt;&lt;span class="BallotCandidateAnchor"&gt;Barack
          Obama&lt;/span&gt;&lt;/a&gt;...</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td Class="tdBallotCheckBox"&gt;&lt;input type="checkbox" /&gt;...&lt;/tr&gt;...</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;/tr&gt;&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/td&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" cellspacing="0" cols="3">
    <tr class="trBallotOfficeContestsRowOfThree">
      <td class="tdBallotOfficeContest" align="center" colspan="1">
        <table class="tableBallotOfficeContest" cellspacing="0">
          <tr class="trBallotOfficeHeading">
            <td class="tdBallotOfficeHeading" align="center" colspan="2">
              <a href="<% =UrlManager.GetIssuePageUri("CA", "CA20081104GA", "USPresident", "ALLPersonal") %>"
                title="Candidate Issue Comparisons for U.S. President Vice President" target="_self">
                U.S. President<br>
                Vice President</a><br>
              <span class="BallotOfficesVoteFor">Vote for ONE</span></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("ILObamaBarack") %>" target="_self"><span class="BallotCandidateAnchor">
                Barack Obama</span></a> - <a href="http://www.democrats.org/" title="Democratic Party Website"
                  target="_self">D</a><br>
              <a href="<% =UrlManager.GetIntroPageUri("DEBidenJosephRJr") %>" target="_self"><span class="BallotCandidateAnchor">
                Joe Biden</span></a></td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl26" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("AZMccainJohn") %>" target="_self"><span class="BallotCandidateAnchor">
                John McCain</span></a> - <a href="http://www.rnc.org/" title="Republican Party Website"
                  target="_self">R</a><br>
              <a href="<% =UrlManager.GetIntroPageUri("AKPalinSarahH") %>" target="_self"><span class="BallotCandidateAnchor">
                Sarah Palin</span></a></td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl30" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("ILKeyesAlanL") %>" target="_self"><span class="BallotCandidateAnchor">
                Alan Keyes</span></a> - <a href="http://www.rnc.org/" title="Republican Party Website"
                  target="_self">R</a><br>
              <a href="<% =UrlManager.GetIntroPageUri("MIDrakeWileySSr") %>" target="_self"><span class="BallotCandidateAnchor">
                Wiley S. Drake, Sr.</span></a></td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl34" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("GAMckinneyCynthia") %>" target="_self"><span class="BallotCandidateAnchor">
                Cynthia McKinney</span></a> - <a href="http://www.accgreens.org" title="Green Party of Georgia Website"
                  target="_self">G</a><br>
              <a href="<% =UrlManager.GetIntroPageUri("NCClementeRosa") %>" target="_self"><span class="BallotCandidateAnchor">
                Rosa Clemente</span></a></td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl38" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("GABarrBob") %>" target="_self"><span class="BallotCandidateAnchor">
                Bob Barr</span></a> - <a href="http://www.ga.lp.org/" title="Libertarian Party of Georgia Website"
                  target="_self">L</a><br>
              <a href="<% =UrlManager.GetIntroPageUri("NVRootWayneA") %>" target="_self"><span class="BallotCandidateAnchor">
                Wayne Allen Root</span></a></td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl42" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              <a href="<% =UrlManager.GetIntroPageUri("CTNaderRalph") %>" target="_self"><span class="BallotCandidateAnchor">
                Ralph Nader</span></a> - I</td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl46" type="checkbox" /></td>
          </tr>
          <tr class="trBallotCandidate">
            <td class="tdBallotCandidate" align="left" colspan="1">
              No Preference</td>
            <td class="tdBallotCheckBox" align="right" colspan="1">
              <input name="_ctl50" type="checkbox" /></td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
    <!--table -->
    <table id="Table11" cellpadding="0" cellspacing="0" class="tableAdmin">
      <tr>
        <td align="left" class="H" colspan="2" valign="top">
          Referendums</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr class="trBallotSeparator"&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td class="tdBallotSeparator"&gt;California Referendum&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr Class="trBallotReferendumHeading"&gt;&lt;td Class="tdBallotReferendumHeading"&gt;&lt;a
          href="....&gt;Safe...&lt;/a&gt;&lt;/td&gt;&lt;/tr&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr Class="trBallotReferendum"&gt;&lt;td Class="tdBallotReferendum"&gt;Finance
          a high-speed...&lt;/td&gt;</td>
      </tr>
      <tr class="trBallotOfficeContestsRowOfThree">
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td Class="tdBallotReferendumYN"&gt;&lt;table Class="tableBallotReferendumsYN&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;tr Class="trBallotReferendumYN"&gt;&lt;td Class="tdBallotReferendumYN"&gt;Yes&lt;/td&gt;</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;td&nbsp; Class="tdBallotReferendumCheckBox"&gt;&lt;input type="checkbox" /&gt;&lt;/td&gt;...</td>
      </tr>
      <tr>
        <td choff="<" class="TSmallBorder">
          &lt;/tr&gt;&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/td&gt;&lt;/table&gt;</td>
      </tr>
    </table>
    &nbsp;
     <!-- Table -->
  <table class="tableAdmin" cellspacing="0">
    <tr class="trBallotSeparator">
      <td class="tdBallotSeparator" colspan="3">
        California Referendum</td>
    </tr>
    <tr class="trBallotReferendumHeading">
      <td class="tdBallotReferendumHeading" align="center" colspan="3">
        <a href="/Referendum.aspx?State=CA&Election=20081104GCA000000ALL&Referendum=CA20081104000000Prop1A"
          title=" Referendum Description, Details and Full Text" target="view">Safe, Reliable
          High-Speed Passenger Train Bond Act.</a></td>
    </tr>
    <tr class="trBallotReferendum">
      <td class="tdBallotReferendum" align="left" colspan="2">
        Finance a high-speed passenger train system by providing the funds
        necessary therefor through the issuance and sale of bonds of the
        State of California and by providing for the handling and disposition
        of those funds, and declaring the urgency thereof, to take effect
        immediately.</td>
      <td class="tdBallotReferendumYN" align="right" colspan="1">
        <table class="tableBallotReferendumYN" cellspacing="0">
          <tr class="trBallotReferendumYN">
            <td class="tdBallotReferendumYN" align="left" colspan="1">
              Yes</td>
            <td class="tdBallotReferendumCheckBox" align="right" colspan="1">
              <input name="_ctl127" type="checkbox" /></td>
          </tr>
          <tr class="trBallotReferendumYN">
            <td class="tdBallotReferendumYN" align="left" colspan="1">
              No</td>
            <td class="tdBallotReferendumCheckBox" align="right" colspan="1">
              <input name="_ctl131" type="checkbox" /></td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
   <!--table -->
    <!--table -->
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
            ReadOnly="True" Width="350px" Height="3500px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks>
        </td>
        <td>
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxCustomStyleSheet" runat="server"  Height="3500px"
            TextMode="MultiLine" Width="350px" CssClass="TextBoxInputMultiLine"></user:TextBoxWithNormalizedLineBreaks></td>
      </tr>
    </table>
  </form>
</body>
</html>
--%>