<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="IssueQuestions.aspx.cs"
  Inherits="Vote.Politician.IssueQuestions1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html><head><title></title></head><body></body></html>

<%--@ Register Src="../LoginBar.ascx" TagName="LoginBar" TagPrefix="user" --%>
<%--@ Register Src="../Banner.ascx" TagName="Banner" TagPrefix="user" --%>
<%--@ Register Src="../NavBarAdmin.ascx" TagName="NavBar" TagPrefix="uc2" --%>
<%--@ Register Src="SupportUs.ascx" TagName="SupportUs" TagPrefix="user" --%>
<%--
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
  <title>Answers to Issue Questions</title>
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <link href="/css/Intro.css" type="text/css" rel="stylesheet" />
  <meta content="False" name="vs_showGrid">
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
  <meta content="C#" name="CODE_LANGUAGE">
  <meta content="JavaScript" name="vs_defaultClientScript">
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  <script language="javascript">
    function postIssuePage(Issue, Office) {
      document.postIssueForm.action = "/Issue.aspx";
      document.postIssueForm.Issue.value = Issue;
      document.postIssueForm.Office.value = Office;
      document.postIssueForm.target = "view";
      document.postIssueForm.submit();
    }
  </script>
  <script language="javascript">
    function postPoliticianIssuePage(Issue, Id) {
      document.postPoliticianIssueForm.action = "/PoliticianIssue.aspx";
      document.postPoliticianIssueForm.Issue.value = Issue;
      document.postPoliticianIssueForm.Id.value = Id;
      document.postPoliticianIssueForm.target = "view";
      document.postPoliticianIssueForm.submit();
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
        <table id="TableHeading" cellspacing="0" cellpadding="0">
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
            <td align="left" class="HLargeColor">
              Issue:
              <asp:Label ID="Issue" runat="server" ForeColor="Red"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left">
              <asp:Label ID="Msg" runat="server"></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              This form allows you to express your views and positions on numerous topics regarding
              the issue shown above. When you click on any topic link in the left hand column
              of the report below, you will be provided with a form where you can provide a response
              to that topic. To the right of each topic is the response we have for you, and show
              on our website pages. Use the links in the blue navigation bar to respond to topics
              for other issues. All or your responses can be changed or deleted at any future
              time.
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              You may find the following three links helpful:
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <asp:HyperLink ID="HyperLinkViewActual" runat="server" CssClass="HyperLink" Target="view">[HyperLinkViewActual]</asp:HyperLink>&nbsp;
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkSearchEngineComparePage" runat="server" CssClass="HyperLink">Submit Your Pages of Positions and Views to the Google Search Engine</asp:HyperLink>
            </td>
          </tr>
          <tr>
            <td align="left" class="T">
              <br />
              <asp:HyperLink ID="HyperLinkIssuesList" runat="server" CssClass="HyperLink" Target="view">Obtain a Complete List of Issues and Questions</asp:HyperLink>
              <br />
              &nbsp;
            </td>
          </tr>
        </table>
      </td>
      <td align="right" valign="top">
        <asp:Image ID="CandidateImage" runat="server" CssClass="CandidateImage"></asp:Image>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <!-- Table -->
  <table class="tableAdmin" id="TableInstructions" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HColor">
        Respond to Other Issues
      </td>
    </tr>
    <tr>
      <td align="left" class="T">
        <br />
        When you click on an issue in the blue navigation bar below, you will be presented
        with a form for that issue with a set of unique topics for that issue.
        <br />
        &nbsp;
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableLinks" cellspacing="0" cellpadding="0">
    <tr>
      <td>
        <table id="IssueLinksTable" cellspacing="0" cols="2" cellpadding="1" align="left"
          border="0" runat="server">
        </table>
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="TableIssueTitle" cellspacing="0" cellpadding="0">
    <tr>
      <td align="left" class="HSmallColor">
        Click on an issue topic in the left column for which you would like to provide a
        response.
      </td>
    </tr>
  </table>
  <!-- Table -->
  <table class="tableAdmin" id="QuestionsAnswersTable" cellspacing="0" cellpadding="0"
    runat="server">
  </table>
  </form>
</body>
</html>
--%>