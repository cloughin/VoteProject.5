<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
  Inherits="Vote.Politician.DefaultPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html><head><title></title></head><body></body></html>

<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register Src="SupportUs.ascx" TagName="SupportUs" TagPrefix="user" --%>
<%--@ Register Src="../NavBarAdmin.ascx" TagName="NavBar" TagPrefix="uc2" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Candidate Home Page</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
  <meta content="C#" name="CODE_LANGUAGE" />
  <meta content="JavaScript" name="vs_defaultClientScript" />
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
  <script language="javascript">
    function postPoliticianIntroPage(where) {
      document.postIntroForm.action = "/Intro.aspx";
      document.postIntroForm.Id.value = where;
      document.postIntroForm.target = "view";
      document.postIntroForm.submit();
    }
  </script>
  <script language="javascript">
    function postPoliticianIssuePage(Id) {
      document.postPoliticianIssueForm.action = "/PoliticianIssue.aspx";
      document.postPoliticianIssueForm.Id.value = Id;
      document.postPoliticianIssueForm.target = "view";
      document.postPoliticianIssueForm.submit();
    }
  </script>
  <script language="javascript">
    function postIssuePage(Office, Issue) {
      document.postIssueForm.action = "/Issue.aspx";
      document.postIssueForm.Office.value = Office;
      document.postIssueForm.Issue.value = Issue;
      document.postIssueForm.target = "view";
      document.postIssueForm.submit();
    }
  </script>
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <user:LoginBar ID="LoginBar1" runat="server" />
  <user:Banner ID="Banner" runat="server" />
  <uc2:NavBar ID="NavBar" runat="server" />
  <!-- Table -->
  <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0">
    <tr>
      <td valign="top">
        <!-- Table -->
        <table id="Table1" cellspacing="0" cellpadding="0">
          <tr>
            <td align="left" class="tdIntroPoliticianName">
              <asp:Label ID="PoliticianName" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="tdIntroPoliticianStatus">
              <asp:Label ID="PoliticianOffice" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="tdIntroPoliticianStatus">
              <asp:Label ID="PoliticianElection" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left">
              <asp:Label ID="Msg" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              We encourage you to provide information about yourself and your positions and views
              on various issues on our website, free of charge. You have an introduction page
              and numerous pages where you can provide your views and positions on various issues.
              Use the links in the first section below to add, change and delete the content of
              your introduction page. Use the links in the bottom section to add, change or delete
              the content on your issue response pages. Your positions and views will be presented
              in side-by-side comparisons with the other candidates running for the same office.
              <br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
            </td>
          </tr>
          <tr>
            <td align="left" class="H">
              Introduce Yourself
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below to be presented with a form where you can provide a picture
              and your biographical information for your introduction page.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkEnterIntro" runat="server" CssClass="HyperLink">Add, Change or Delete Your Introduction Page Content and Picture</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below to view the introduction page voters will see.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkViewIntro" runat="server" CssClass="HyperLink" Target="view">View Your Introduction Page that We Provide to Voters</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below if you want to promote your introcudtion page on Google's Search
              Engine.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkSearchEngineIntroPage" runat="server" CssClass="HyperLink">Submit Your Introduction Page to the Google Search Engine</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              &nbsp;
              <br />
            </td>
          </tr>
          <tr>
            <td align="left" class="H">
              Provide Your Views and Positions on Issues
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below to be presented with forms where you can provide your positions
              and views on various issues.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkEnterViews" runat="server" CssClass="HyperLink">Add, Change or Delete Your Views and Positions on Issues</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below to view the issue comparison pages voters will see.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperlinkViewPositions" runat="server" CssClass="HyperLink" Target="view">View the Pages of Your Positions and Views that We Provide to Voters</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              Click the link below if you want to promote your issue comparison pages on Google's
              Search Engine.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkSearchEngineComparePage" runat="server" CssClass="HyperLink">Submit Your Pages of Positions and Views to the Google Search Engine</asp:HyperLink><br />
              &nbsp;
            </td>
          </tr>
        </table>
        <!-- Table -->
      </td>
      <td align="right" valign="top">
        <asp:Image ID="CandidateImage" runat="server" CssClass="CandidateImage"></asp:Image>
      </td>
    </tr>
  </table>
  </form>
</body>
</html>
--%>