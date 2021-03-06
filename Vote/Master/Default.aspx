<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Default.aspx.cs" Inherits="Vote.Master.DefaultPage" %>

<%@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" %>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
  <meta content="C#" name="CODE_LANGUAGE"/>
  <meta content="JavaScript" name="vs_defaultClientScript"/>
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
  <style type="text/css">
    .style1
    {
      font-family: Verdana, Arial, Helvetica, sans-serif;
      font-weight: bold;
      color: #26466D;
      font-size: 13px;
      text-align: center;
      height: 24px;
      border: 1px solid gray;
      padding: 3px;
      background-color: #E0E0E0;
    }
    .page-title {
      font-size: 24pt;
      margin-top: 20px;
      display: inline-block;    
    }
    .landing-page-tasks {
      margin-top: 20px;
    }
    .landing-page-tasks > .head {
      font-size: 14pt;
      text-align: center;
      background: #666;
      color: #fff;
      padding: 8px 0;
    }
    .landing-page-tasks .container {
      border: 1px solid #666;
      padding: 5px;
    }
    .landing-page-tasks .container .col {
      float: left;
    }
    .landing-page-task {
      border: 1px solid #666;
      margin: 5px;
      padding: 5px;
      width: 440px;
      vertical-align: top
    }
    .wide .landing-page-task {
      width: 904px;
    }
    .landing-page-task .head {
      font-size: 12pt;
      font-weight: bold;
      margin-bottom: 4px;
    }
    .landing-page-task .main-menu-entry {
      margin: -2px 0 4px 0;
      color: #779ad4;
    }
    .landing-page-task .description {
      font-size: 10pt;
      margin-bottom: 4px;
    }
    .landing-page-task .description li {
      list-style-type: disc;
      margin-left: 16px;
    }
    .tableAdmin {
      width: 100% !important;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <h1 id="H1" EnableViewState="false" runat="server"></h1>

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
      
      <div><asp:Label ID="Msg" runat="server"></asp:Label></div>

      <user:NoJurisdiction ID="NoJurisdiction" runat="server" />
      
      <div class="landing-page-tasks">
        <div class="head">Descriptions of Master Tasks</div>
        <div class="container">
          
          <div class="col" id="Column1" runat="server">
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/politicianfind.aspx">Find Politician Username and Password</a></div>
              <div class="main-menu-entry">Main Menu: Politicians > | Find Username and Password</div>
              <div class="description">
                <ul>
                  <li>Use to respond to a politician request for login credentials.</li>
                  <li>The result is a pre-written email with credentials and instructions that can be 
                    pasted into your email client.</li>
                </ul>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/updateusers.aspx">Master and Admin Users &ndash; Manage Security</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Manage Users</div>
              <div class="description">
                <p>Manage user accounts and passwords for master users and state, county and local admin users.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/cachecontrol.aspx">Page Cache Control</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Page Cache Control</div>
              <div class="description">
                <ul>
                  <li>Clear all cached pages.</li>
                  <li>Set the page expiration interval.</li>
                </ul>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/nags.aspx">Nag Dialogs</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Nag Dialogs</div>
              <div class="description">
                <ul>
                  <li>Manage the number, order and frequency of donation nags.</li>
                  <li>Turn donation nags on or off.</li>
                  <li>Turn &lsquo;Get Future Sample Ballots Automatically&rsquo; nags on or off.</li>
                </ul>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/imagesheadshots.aspx">Review Candidate Profile and Headshot Pictures</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Headshots</div>
              <div class="description">
                <p>Review newly uploaded candidate pictures and crop for profile and headshot pictures.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/admin/bulkemail.aspx">Bulk Email</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Bulk Email</div>
              <div class="description">
                <p>Send emails to state election authorities, candidates, political party contacts, visitors and volunteers using templates and token substitutions.</p>
                <p><strong>Note:</strong> To edit special email templates (such as the one used for Share My Ballot Choices), log in as SpecialTemplates and do the Bulk Email.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/emailbulkdelete.aspx">Bulk Email Delete</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Bulk Email Delete</div>
              <div class="description">
                <ul>
                  <li>Delete all undeliverable email addressess in a single operation.</li>
                  <li>Delete selected email address wherever they appear in the database.</li>
                </ul>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/logdonations.aspx">Log Donations</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Log donations</div>
              <div class="description">
                <p>Add new email donation receipts to the donations log.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/updatevolunteers.aspx">Volunteers</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Volunteers</div>
              <div class="description">
                <p>Add volunteers and set up their login credentials.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/videos.aspx">Instructional Videos</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Instructional Videos Maintenance</div>
              <div class="description">
                <p>Manage the ScreenCast instructional videos.</p>
              </div>
              <div class="main-menu-entry"><a href="/admin/videos.aspx">View Admin Videos</a></div>
              <div class="main-menu-entry"><a href="/party/videos.aspx">View Volunteer/Party Videos</a></div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/ballotpediacsvs.aspx">Create Election CSV</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | BallotPedia CSVs</div>
              <div class="description">
                <p>Create a CSV file of the office contests and candidates for a state election. This capability was originally created to share our election data with BallotPedia.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/reviewyoutubechannels.aspx">Review YouTube Channels</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Review YouTube Channels</div>
              <div class="description">
                <ul>
                  <li>Review all politicians with an unverified specific YouTube video as their social media link for YouTube.</li>
                  <li>The page queries YouTube for the video's channel information.</li>
                  <li>Based on the channel information, you can choose to use the video or to substitute the channel.</li>
                </ul>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/setstaging.aspx">Set Staging</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Set Staging</div>
              <div class="description">
                <p>Turns staging features on or off. This only affects your current web browser.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/reviewyoutubechannels.aspx">VoteSmart Data Import</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | VoteSmart Import</div>
              <div class="description">
                <p>Import VoteSmart candidate information using the VoteSmart api.</p>
              </div>
            </div>
          
          </div>
          
          <div class="col" id="Column2" runat="server">

            <div class="landing-page-task">
              <div class="head"><a href="/admin/issues.aspx?state=LL">Issue Topics Common to All Candidates</a></div>
              <div class="main-menu-entry">Main Menu: Issues > | Questions > | All Candidates</div>
              <div class="description">
                <ul>
                  <li>Add, change or delete an issue topic from among those we make available to all candidates, regardless of the level of office they are seeking.</li>
                  <li>An issue topic is a group of related issue questions.</li>
                  <li>The issue topic page has links to allow changes to the individual questions.</li>
                </ul>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/issues.aspx?state=US">Issue Topics for National Candidates</a></div>
              <div class="main-menu-entry">Main Menu: Issues > | Questions > | National</div>
              <div class="description">
                <ul>
                  <li>Add, change or delete an issue topic from among those we make available only to national candidates (President, US Senator, US House of Representatives).</li>
                  <li>An issue topic is a group of related issue questions.</li>
                  <li>The issue topic page has links to allow changes to the individual questions.</li>
                </ul>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=U1">US President and Vice President &ndash; All States Report</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | US President</div>
              <div class="description">
                <p>Manage the pseudo-elections that allow us to report the results of presidential elections state-by-state.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=U2">US Senate &ndash; All States Report</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | US Senate</div>
              <div class="description">
                <p>Manage the pseudo-elections that allow us to report the results of US Senate elections state-by-state.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=U3">US House of Representatives &ndash; All States Report</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | US House</div>
              <div class="description">
                <p>Manage the pseudo-elections that allow us to report the results of US House of Representatives elections state-by-state.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=U4">Governor and Lieutenant Governor &ndash; All States Report</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | State Governors</div>
              <div class="description">
                <p>Manage the pseudo-elections that allow us to report the results of gubernatorial elections state-by-state.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=US">Remaining Presidential Party Primary Candidates</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | Remaining Presidential Party Primary Candidates</div>
              <div class="description">
                <p>Manage the pseudo-elections that drive the &ldquo;Compare the Presidential 
                Candidates&rdquo; links for each party at the top of the home page during presidential 
                primary campaigns.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateelections.aspx?state=PP">Presidential Candidates Templates</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Elections > | Presidential Candidates Templates</div>
              <div class="description">
                <p>Manage the pseudo-elections that serve as templates to automatically add the presidential candidates when creating a state election.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/officials.aspx?state=U1">Current President and Vice President</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Incumbents > | US President</div>
              <div class="description">
                <p>Manage the information presented on the public Officials page for US President and Vice President.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/officials.aspx?state=U2">Current US Senators</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Incumbents > | US Senate</div>
              <div class="description">
                <p>Manage the information presented on the public Officials page for US Senators.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/officials.aspx?state=U3">Current US Representatives</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Incumbents > | US House</div>
              <div class="description">
                <p>Manage the information presented on the public Officials page for members of the US House of Representatives.</p>
              </div>
            </div>

            <div class="landing-page-task">
              <div class="head"><a href="/admin/officials.aspx?state=U4">Current State Governors and Lieutenant Governors</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Federal &amp; Governors Incumbents > | State Governors</div>
              <div class="description">
                <p>Manage the information presented on the public Officials page for state Governors and Lientenant Governors.</p>
              </div>
            </div>
          
          </div>
          
          <div class="clear-both"></div>
        </div>

      </div>
      
      <div class="landing-page-tasks">
        <div class="head">Legacy Tasks</div>
        <div class="container">
          <div style="color: red;font-size: 12pt;margin: 8px 0;">Use these functions with caution. Many have been obsoleted or superceded by newer functions.</div>

    <table class="tableAdmin" id="Table4" cellspacing="0" cellpadding="0">
      <tr>
        <td align="center" class="H" colspan="2">
          301 &amp; 404 Error Logs
        </td>
      </tr>
      <tr>
        <td class="TBold">
          <asp:RadioButtonList ID="RadioButtonList_Log_301_404_Errors" runat="server" AutoPostBack="True"
            CssClass="RadioButtons" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList_Log_301_404_Errors_SelectedIndexChanged">
            <asp:ListItem Value="T">On</asp:ListItem>
            <asp:ListItem Value="F">Off</asp:ListItem>
          </asp:RadioButtonList>
        </td>
        <td class="TLargeBold" style="width: 100%">
          Logs of 301, 302, 404 and Unhandled Errors and Good Urls:
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonDeleteUserErrorLogs" runat="server" CssClass="Buttons" OnClick="ButtonDeleteUserErrorLogs_Click"
            Text="Delete User Error Logs" Width="200px" />
        </td>
        <td class="T" style="width: 100%">
          Truncates LogErrorsAdmin, Log301Redirect, Log302Redirect, Log404PageNotFound
        </td>
      </tr>
    </table>
    
    <%--
    <table class="tableAdmin" id="Table18" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H" style="height: 23px">
          Office(s) Maintenance
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLinkMaintOffices" runat="server" CssClass="HyperLink" NavigateUrl="~/Master/MaintOffices.aspx"
            Target="_edit">Office(s) Maintenance</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          The link above will present a form where you can do a number of things including:
          view information about one or two offices; consolidate two offices into one; change
          the OfficeKey; and delete offices.
        </td>
      </tr>
    </table>
    --%>

    <table class="tableAdmin" id="Table16" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H" style="height: 23px">
          Politician(s) Maintenance
        </td>
      </tr>
    </table>

    <table class="tableAdmin" id="Table40" cellspacing="0" cellpadding="0">
      <%--
      <tr>
        <td class="HSmall">
          Delete a Politician
         </td>
      </tr>
      <tr>
        <td class="T">
          Enter PoliticianKey and click button to completely delete all information about
          the politician in all tables, including clearing cached pages.
        </td>
      </tr>
      <tr>
        <td class="T">
          PoliticianKey (Login Username):
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxDeletePoliticianKey" runat="server" CssClass="TextBoxInput"
            Width="300px"></user:TextBoxWithNormalizedLineBreaks>&nbsp;<asp:Button ID="ButtonDeletePolitician" runat="server"
              CssClass="Buttons" OnClick="ButtonDeletePolitician_Click" Text="Delete Politician"
              Width="150px" />
        </td>
      </tr>
      --%>
      <tr>
        <td class="HSmall">
          Erase all Website Address of a Specific Url</td>
      </tr>
      <tr>
        <td class="T">
          Many politicians use the home page of the State Legislature as a web address. 
          The following operation will erase all these urls pointing to non-specific 
          addresses.</td>
      </tr>
      <tr>
        <td class="T">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxWebAddress" runat="server" CssClass="TextBoxInput" 
            Width="400px"></user:TextBoxWithNormalizedLineBreaks>
&nbsp;
          <asp:Button ID="ButtonDeleteWebAddress" runat="server" CssClass="Buttons" 
            onclick="ButtonDeleteWebAddress_Click" Text="Erase Website Address" />
        </td>
      </tr>
    </table>

    <table class="tableAdmin" id="Table39" cellspacing="0" cellpadding="0">
      <tr>
        <td align="center" class="H">
          Management Reports</td>
      </tr>
      <%--
      <tr>
        <td align="center" class="HSmall">
          Future Elections
         </td>
      </tr>
      <tr>
        <td class="T">
          The links below provide reports of future elections. They also provide the early voting
          and registration dates.
        </td>
      </tr>
      <tr>
        <td class="TBold">
          <br />
          <asp:HyperLink ID="HyperLinkFutureElectionsAll" runat="server" CssClass="HyperLink"
            NavigateUrl="/Master/MgtReports.aspx?Report=FutureElectionsAll" Target="view">All Future Elections</asp:HyperLink>
          <br />
          <asp:HyperLink ID="HyperLinkFuturePresidentialPrimaries" runat="server" CssClass="HyperLink"
            NavigateUrl="/Master/MgtReports.aspx?Report=FuturePresidentialPrimaries" Target="view">Future Presidential Primaries</asp:HyperLink>
          <br />
          <asp:HyperLink ID="HyperLinkFutureStatePrimaries" runat="server" CssClass="HyperLink"
            NavigateUrl="/Master/MgtReports.aspx?Report=FutureStatePrimaries" Target="view">Future State Primaries</asp:HyperLink>
          <br />
          <asp:HyperLink ID="HyperLinkFutureGeneralElections" runat="server" CssClass="HyperLink"
            NavigateUrl="/Master/MgtReports.aspx?Report=FutureGeneralElections" Target="view">Future General Elections</asp:HyperLink>
          <br />
          <br />
          <asp:HyperLink ID="HyperLinkFutureSpecialElections" runat="server" CssClass="HyperLink"
            NavigateUrl="/Master/MgtReports.aspx?Report=FutureSpecialElections" Target="view">Future Special Elections</asp:HyperLink>
          <br />
        </td>
      </tr>
      --%>
      <tr>
        <td align="center" class="HSmall">
          Reports of Logs of Data Entered:
        </td>
      </tr>
      <tr>
        <td align="left" class="T">
          Activity in all the Logs for a user MASTER Login Name and computes payment
        </td>
      </tr>
      <tr>
        <td align="left" class="T">
          <asp:HyperLink ID="HyperLinkLogsUserLogin" runat="server" NavigateUrl="/Master/Logs.aspx"
            CssClass="HyperLink" Target="view">Transaction for a User (and payment computation)</asp:HyperLink>
        </td>
      </tr>
      <%--
      <tr>
        <td align="left" class="T">
          A report of the answers entered by a login user name, usually a politician
        </td>
      </tr>
      <tr>
        <td align="left" class="T">
          &nbsp;<asp:HyperLink ID="HyperLinkLogAnswers" runat="server" NavigateUrl="/Master/Answers.aspx"
            CssClass="HyperLink" Target="view">Answers Entered By Login Username</asp:HyperLink>
        </td>
      </tr>
      --%>
      <%--
      <tr>
        <td class="HSmall">
          Intro Page Entries and Answer Page Entries
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLink2" runat="server" CssClass="HyperLink" NavigateUrl="/Master/MgtReports.aspx?Report=PoliticianData&State=U1P">US Presidential Primaries</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLink13" runat="server" CssClass="HyperLink" Target="view"
            NavigateUrl="/Master/MgtReports.aspx?Report=PoliticianData&amp;State=U2">US Senate</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLink14" runat="server" CssClass="HyperLink" Target="view"
            NavigateUrl="/Master/MgtReports.aspx?Report=PoliticianData&amp;State=U3">US House</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="Hyperlink3" runat="server" CssClass="HyperLink" Target="view"
            NavigateUrl="/Master/MgtReports.aspx?Report=PoliticianAdds">Politician Adds</asp:HyperLink>
        </td>
      </tr>
      --%>
      <%--
      <tr>
        <td class="H">
          Reports and Actions to Fix Keys and Cleanup Critical Tables
        </td>
      </tr>
      <tr>
        <td class="T">
          Below are a reports and actions to insure the integrity critical keys in Tables
        </td>
      </tr>
      <tr>
        <td class="HSmall">
          Offices Table
        </td>
      </tr>
      <tr>
        <td class="T">
          Checks that the rows exist in the States, Counties and LocalDistricts Tables for
          the StateCode, CountyCode and LocalCode in the Offices Table. It also checks that
          the LocalCode is empty for county level offices and the CountyCode and LocalCode
          is empty for State level offices.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLink4" runat="server" CssClass="HyperLink" NavigateUrl="/Master/MgtReports.aspx?Report=CheckOfficesTableKeys"
            Target="view">Offices Table's - StateCode, CountyCode, LocalCode</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          This button will run a series of queries to set the CountyCode and LocalCode empty
          for State level offices and set the LocalCode empty for county level offices.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonCleanCodes" runat="server" OnClick="ButtonCleanCodes_Click"
            Text="Clean Offices Table's CountyCode and LocalCode" Width="350px" CssClass="Buttons" />
        </td>
      </tr>
      <tr>
        <td class="HSmall">
          Politicians Table
        </td>
      </tr>
      <tr>
        <td class="T">
          Every Politician row in Politicians Table should have a valid OfficeKey to create
          Intro.aspx and PoliticianIssues.aspx pages. This link reports all cases where there
          is no matching Office row in the Offices Table for the OfficeKey for each politician
          row.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:HyperLink ID="HyperLink5" runat="server" CssClass="HyperLink" NavigateUrl="/Master/MgtReports.aspx?Report=CheckPoliticiansTableKeys"
            Target="view">Politicians Table - OfficeKey</asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td class="T">
          For the PoliticianKeys identified having no matching Office Table rows this button
          will 1) delete all the rows in all the tables (Politicians, ElectionsPoliticians,
          OfficesOfficials, Answers)&nbsp; 2) clear cach for the Intro.aspx and PoliticianIssue.aspx
          pages. Upon completion the above report should be run.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonDeleteBadPoliticianRows" runat="server" CssClass="Buttons"
            OnClick="ButtonDeleteBadPoliticianRows_Click" Text="Delete All Bad Politician Rows"
            Width="350px" />
        </td>
      </tr>
    </table>
    --%>

    <table class="tableAdmin" id="Table33" cellspacing="0" cellpadding="0">
      <tr>
        <td class="H">
          Misc
        </td>
      </tr>
      <tr>
        <td class="T">
          &nbsp;Deletes ElectionsOffices and ElectionsPoliticians Rows where there is no corresponding
          rows in the Offices and Politicians Tables. May be run without risk at any time.<br>
          <asp:Button ID="ButtonCleanElections" runat="server" CssClass="Buttons" Text="Clean ElectionsOffices &amp; ElectionsPoliticians"
            OnClick="ButtonCleanElections_Click" Width="300px"></asp:Button>Deletes Offices
          and Politicians Rows where there is no rows in the Offices and Politicians Tables
          corresponding to an OfficeKey or PoliticianKey in Politicians, ElectionsOffices,
          ElectionsPoliticians, OfficesOfficials Tables.&nbsp;
        </td>
      </tr>
    </table>
    
    <%--
    <table class="tableAdmin" id="Table2" cellspacing="0" cellpadding="0" >
      <tr>
        <td align="left" class="HColor" valign="top" colspan="2">
          Visible MASTER Controls on Forms (Set NOT Visible for production)
        </td>
      </tr>
      <tr>
        <td align="left" valign="top" class="HSmall">
          &nbsp;<asp:RadioButtonList ID="RadioButtonListMasterControls" runat="server" CssClass="RadioButtons"
            AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListMasterControls_SelectedIndexChanged"
            Width="199px">
            <asp:ListItem Selected="True" Value="F">MASTER Contols NOT Visible</asp:ListItem>
            <asp:ListItem Value="T">MASTER Controls Visible</asp:ListItem>
          </asp:RadioButtonList>
        </td>
        <td align="left" valign="top" class="T">
          Normally, set <strong>MASTER Controls NOT Visible</strong>.<br />
          When set On the controls entitled MASTER USER ONLY are made visible providing special
          functionality like:<br />
          Passwords, Graphic of Election Results, Copy Politicians for Offices of Previous
          Elections, Email Candidates, Candidate Letters<br />
        </td>
      </tr>
    </table>
    --%>

    <table class="tableAdmin" id="Table36" cellspacing="0" cellpadding="0" >
      <tr>
        <td valign="top" colspan="2" class="H">
          Election Deletions
        </td>
      </tr>
      <tr>
        <td align="left" valign="top" class="HSmall" style="width: 232px">
          <asp:RadioButtonList ID="RadioButtonListPermitElectionDeletions" runat="server" AutoPostBack="True"
            CssClass="RadioButtons" OnSelectedIndexChanged="RadioButtonListPermitElectionDeletions_SelectedIndexChanged"
            Width="218px">
            <asp:ListItem Selected="True" Value="F">DO NOT Permit Election Deletions</asp:ListItem>
            <asp:ListItem Value="T">Permit Election Deletions</asp:ListItem>
          </asp:RadioButtonList>
        </td>
        <td align="left" valign="top" class="T">
          The radio button must be set to permit election deletions for any election to be
          deleted. This is a safety measure to insure that election information is not accidentally
          deleted.
        </td>
      </tr>
      <tr>
        <td align="center" style="width: 232px">
        </td>
        <td align="center">
        </td>
      </tr>
    </table>

    <table class="tableAdmin" id="Table34" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="H" colspan="2">
          One Shot Utilities to fix Database Problems
        </td>
      </tr>
      <tr>
        <td class="t" colspan="2">
          The functions below can be run anytime without harm to the database, pages or reports
        </td>
      </tr>
    </table>

    <table class="tableAdmin" id="Table23" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td align="center" class="H" colspan="2">
          Politicians
        </td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          <asp:Button ID="Button_Delete_Politicians_Report" runat="server" OnClick="Button_Delete_Politicians_Report_Click"
            Text="Report Politicians to be Deleted" Width="200px" CssClass="Buttons" />
        </td>
        <td class="T" style="width: 490px">
          Reports all the politicians that will be deleted when the button below is clicked.
        </td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          &nbsp;<asp:Button ID="ButtonBadPoliticianOfficeKeys" runat="server" OnClick="ButtonBadPoliticianOfficeKeys_Click"
            Text="Delete Politicians" Width="200px" CssClass="Buttons" />
        </td>
        <td class="T" style="width: 490px">
          Deletes Politicians with Bad OfficeKey. Finds all Politicians rows where OfficeKey
          for politician is not in the Offices Table. Then deletes rows for politician in:
          Politicians, Answers,ElectionsPoliticians, and OfficesOfficials. Also removes cached
          pages CacheIntroPages, CachePoliticianIssuePages. If there are rows it reports the
          PoliticianKey to be fixed manually.<br />
          Error Like: SELECT OfficeLevel FROM Offices WHERE OfficeKey = 'MSUsSenateForUnexpiredTerm'
        </td>
      </tr>
      <tr>
        <td class="H" colspan="2">
          Database Maintenance Utilities
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix2" runat="server" CssClass="Buttons" OnClick="ButtonFix2_Click"
            Text="Delete ElectionsOffices" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Deletes all ElectionsOffices rows where there is no matching OfficeKey in Offices
          Table.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="Button_Update_ElectionsOffices" runat="server" CssClass="Buttons"
            OnClick="Button_Update_ElectionsOffices_Click" Text="Update ElectionsOffices" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Updates DistrictCode in ElectionsOffices rows to match the DistrictCode in the Offices
          Table. Only Office District classes 3, 5, 6, 7, 17, 21 are updated.
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix3" runat="server" CssClass="Buttons" OnClick="ButtonFix3_Click"
            Text="Delete ElectionsPoliticians" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Deletes all ElectionsPoliticians rows where there is no matching OfficeKey in Offices
          Table
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix4" runat="server" CssClass="Buttons" OnClick="ButtonFix4_Click"
            Text="Delete ElectionsPoliticians" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Deletes all ElectionsPoliticians rows where there is no matching PoliticianKey in
          Politicians Table
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix5" runat="server" CssClass="Buttons" OnClick="ButtonFix5_Click"
            Text="Delete OfficesOfficials" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Deletes all OfficesOfficials rows where there is no matching OfficeKey in Offices
          Table
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix6" runat="server" CssClass="Buttons" OnClick="ButtonFix6_Click"
            Text="Update ElectionKeys " Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Updates ElectionKey, ElectionKeyCounty, ElectionKeyLocal,ElectionKeyFederal in ElectionsOffices
          &amp; ElectionsPoliticians Tables
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonFix7" runat="server" CssClass="Buttons" OnClick="ButtonFix7_Click"
            Text="Delete Elections" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Deletes all Elections, ElectionsOffices and ElectionsPoliticians
          rows where there are no 1) ElectionsOffices rows (on the ballot) and 2) no Referendum
          rows for the ElectionKey
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonUpdateAnswers" runat="server" CssClass="Buttons" OnClick="ButtonUpdateAnswers_Click"
            Text="Update Answers" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Updates IssueKey and DateStamp in Answers Table. Provides correct IssueKey whenever
          IssueKey is empty. Updates DateStamp containing '1/1/1900' to '1/1/2004'
        </td>
      </tr>
      <%--
      <tr>
        <td class="T">
          <asp:Button ID="Button_Update_Politician_Names" runat="server" CssClass="Buttons"
            Enabled="False" OnClick="Button_Update_Politician_Names_Click" Text="Update Politician Names" />
        </td>
        <td class="T" style="width: 490px">
          Clean up all Politician Names (FName, MName, Nickname, LName, Suffix, AddOn columns)
          in the Polititicians Table, i.e. Re-casing, consistent suffixes.
        </td>
      </tr>
      --%>
      <tr>
        <td class="T">
          <asp:Button ID="Button_Remake_Party_Email_Passwords" runat="server" CssClass="Buttons"
            Enabled="False" OnClick="Button_Remake_Party_Email_Passwords_Click" Text="Remake Party Email Passwords"
            Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Make new passwords for all emails in the PartiesEmails Table. Done so that tighter
          security can be maintained on parties who enter data for politicians.
        </td>
      </tr>
      <%--
      <tr>
        <td class="H" colspan="2">
          Misc
        </td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          <asp:Button ID="ButtonCleanData" runat="server" Text="Trim Empty Colums" CssClass="Buttons"
            OnClick="ButtonCleanData_Click" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Changes strings containing all spaces to empty strings in all Tables
        </td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          &nbsp;</td>
        <td class="T" style="width: 490px">
          &nbsp;</td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          <asp:Button ID="ButtonRestructure" runat="server" CssClass="Buttons" Text="Elections Restructure"
            Enabled="False" OnClick="ButtonRestructure_Click" Width="200px"></asp:Button>
        </td>
        <td class="T" style="width: 490px">
        </td>
      </tr>
      <tr>
        <td class="T" style="width: 212px">
          <asp:Button ID="ButtonOneShot" runat="server" OnClick="ButtonOneShot_Click" Text="One Shot Utilities"
            CssClass="Buttons" Width="200px" />
        </td>
        <td class="T" style="width: 490px">
          Special utilities that are commented out after they are executed.&nbsp;
        </td>
      </tr>
    </table>
    --%>

    <table class="tableAdmin" id="Table19" cellspacing="0" cellpadding="0" border="1">
      <tr>
        <td class="H" colspan="2">
          Misc Database Maintenance
        </td>
      </tr>
      <tr>
        <td class="TBold" colspan="2">
          <user:TextBoxWithNormalizedLineBreaks ID="TextBoxOfficeKey" runat="server" Width="500px"></user:TextBoxWithNormalizedLineBreaks>
          OfficeKey
        </td>
      </tr>
      <tr>
        <td class="T">
          <asp:Button ID="ButtonDeleteOffice" runat="server" OnClick="ButtonDeleteOffice_Click"
            Text="Delete Office" Width="200px" CssClass="Buttons" />
        </td>
        <td class="T" width="100%">
          Delete all rows in all tables for a bad OfficeKey supplied in above textbox including:
          Offices, OfficesOfficials, ElectionsOffices, ElectionsPoliticians Tables<br />
          Examples: OfficeKey should have been county not local.
        </td>
      </tr>
    </table>
        </div>
      </div>
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
