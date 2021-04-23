<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="DesignDefaults.aspx.cs" Inherits="Vote.Master.DesignDefaults" %>

<html><head><title></title></head><body></body></html>

<%-- 
<%@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" %>
<%@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" %>

<%@ Register TagPrefix="uc2" TagName="Navbar" Src="../NavbarAdmin.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
<head id="Head1" runat="server">
  <title>All Pages Default Design</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
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
          DEFAULT Design for: <span style="color: red">All Design Codes</span></td>
      </tr>
      <tr>
        <td align="left">
          <asp:Label ID="Msg" runat="server"></asp:Label></td>
      </tr>
      <tr>
        <td align="left" class="H">
          DEFAULT Page Content &amp; Design</td>
      </tr>
      <tr>
        <td align="left" class="T">
          Use the links in the left column to select the Page whose default design or content
          you want to change. When
          no custom design is identified for a page, these default design values and content will
          be used in the construction of the page(s).</td>
      </tr>
    </table>
    <!-- Table -->
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="H" style="width: 181px" valign="top">
          Pages</td>
        <td class="H" valign="top">
          Page Descriptions</td>
      </tr>
      <tr>
        <td class="T" valign="top" colspan="2">
          When there is no custom design for a domain these default designs will be used</td>
      </tr>
    <tr>
    <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkAllPages" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsAllPages.aspx">All Pages</asp:HyperLink><br />
    </td>
    <td class="T" valign="top">
      
      Page Elements Found on ALL Pages
      
    </td>
    </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkDefaultNoState" runat="server" NavigateUrl="/Master/DesignDefaultsDefaultPage4AllStatesDomain.aspx" CssClass="HyperLink">Home All States</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Home Page for a domain that supports ALL States.
          
          (Default.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkDefaultState" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsDefaultPage4SingleStateDomain.aspx?State=VA"
           >Home SINGLE State</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Home Page for a domain that supports only a single State. (Default.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkBallot" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsBallotPage.aspx"
           >Ballot</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Ballot Page based on an address entered&nbsp;
          (Ballot.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkElected" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsElectedPage.aspx"
           >Elected Representatives</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Elected Officials (Incumbents) Page based on an address entered&nbsp;
          (Elected.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkIntro" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsIntroPage.aspx"
           >Politician's Introduction</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Politician's Introduction Page with picture and bio information&nbsp;
          (Intro.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkPoliticianIssue" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsPoliticianIssuePage.aspx"
           >Politician's Issues</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Politician's Isses Page of politicians answeres to issue questions
          (PoliticianIssue.aspx)</td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkIssue" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsIssuePage.aspx"
           >Issue Comparisons</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Candidates' Issue Comparisons Page with side-by-side comparisons of the candidates'
          responsed to issue questions
          (Issue.aspx)</td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkReferendum" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsReferendumPage.aspx"
           >Referendum</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Referendum Page (Referendum.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkElection" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsElectionPage.aspx"
           >Election Report</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Offices and Candidates Election Report containing all the offices and candidates
          in an election
          (Election.aspx)</td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkOfficials" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsElectedOfficialsPage.aspx"
           >Elected Officials Report</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          Report of all the current Elected Officials i.e. incumbents. (Officials.aspx)</td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkAboutUs" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsAboutUsPage.aspx"
           >About Us</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          About Us Page
          (AboutUs.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkCandidates" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsCandidatesPage.aspx"
           >Candidates</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Information for Candidates Page (Candidates.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkContactUs" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsContactUsPage.aspx"
           >Contact Us</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Contact Us Page
          (ContactUs.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkInterns" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsInternsPage.aspx"
           >Interns</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Interns Page
          (Interns.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkVoters" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsVotersPage.aspx"
           >Voters</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Information for
          Voters Page
          (Voters.aspx)
        </td>
      </tr>
      <tr>
        <td style="width: 181px" valign="top">
          <asp:HyperLink ID="HyperLinkArchives" runat="server" CssClass="HyperLink" NavigateUrl="/Master/DesignDefaultsArchivesPage.aspx"
           >Archives</asp:HyperLink><br />
        </td>
        <td class="T" valign="top">
          
          Archives Page
          (Archives.aspx)
        </td>
      </tr>
    </table>
      <!-- Table -->
      <table id="TableCache_Remove" runat="server" cellpadding="0" cellspacing="0" class="tableAdmin">
        <tr>
          <td align="center" class="H" colspan="2">
          Delete All Custom Design for Particular Pages</td>
        </tr>
        <tr>
          <td class="T" colspan="2">
          These buttons will delete all the custom designs, for the particular pages, allowing the default design
          to take effect. They have been disabled because they could delete
            custom designs. They were originally used to reset election information when an
            election was over.</td>
        </tr>
        <tr>
          <td align="left" class="T" valign="top">
        <asp:Button ID="ButtonAllPages" runat="server" CssClass="Buttons" Enabled="False"
          Text="All Pages" Width="150px" /></td>
          <td align="left" class="T" valign="top">
            Page Elements Found on ALL Pages
          </td>
        </tr>
        <tr>
          <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonHomeAllStates" runat="server" OnClick="ButtonHomeAllStates_Click"
            Text="Home All States" CssClass="Buttons" Enabled="False" Width="150px" /></td>
          <td align="left" class="T" valign="top">
            Home Page for a domain that supports ALL States. (Default.aspx)
          </td>
        </tr>
        <tr>
          <td align="left" class="T" valign="top">
          <asp:Button ID="ButtonHomeSingleState" runat="server" OnClick="ButtonHomeSingleState_Click"
            Text="Home SINGLE State" CssClass="Buttons" Width="150px" Enabled="False" /></td>
          <td align="left" class="T" valign="top">
            Home Page for a domain that supports a single State type 
          domain.
          (Default.aspx)
          </td>
        </tr>
        <tr>
          <td align="center" class="H" colspan="2">
          </td>
        </tr>
      </table>
      <!-- Table -->
      <!-- Table -->
    </form>
</body>
</html>
--%>