<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" 
 MasterPageFile="~/Master/Admin.Master"
CodeBehind="Default.aspx.cs" Inherits="Vote.Master.DefaultPage" %>

<%--@ Register TagPrefix="user" Namespace="Vote" Assembly="VoteLibrary" --%>
<%@ Register Src="/Controls/NoJurisdiction.ascx" TagName="NoJurisdiction" TagPrefix="user" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="MasterHeadContent" runat="server">
  <link href="/css/AllAdmin.css" type="text/css" rel="stylesheet" />
  <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
  <meta content="C#" name="CODE_LANGUAGE"/>
  <meta content="JavaScript" name="vs_defaultClientScript"/>
  <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
  <style type="text/css">
     .container
     {
       margin-bottom: 20px;
     }
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
     .alert
     {
       color: red;
       font-size: 10pt;
       font-weight: bold;
     }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  
  <h1 id="H1" EnableViewState="false" runat="server"></h1>

  <asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
      
      <div><asp:Label ID="Msg" runat="server"></asp:Label></div>

      <asp:PlaceHolder ID="AlertPlaceHolder" runat="server"></asp:PlaceHolder>

      <user:NoJurisdiction ID="NoJurisdiction" runat="server" />
      
      <div class="landing-page-tasks">
        <div class="head">Descriptions of Master Tasks</div>
        <div class="container">
          
          <div class="col" id="Column1" runat="server">
          
            <div class="landing-page-task">
              <div class="head"><a href="/Admin/updatepoliticians.aspx?state=us">Find Politician Username and Password</a></div>
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
                  <li>Turn &lsquo;Get Future Ballot Choices Automatically&rsquo; nags on or off.</li>
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
              <div class="head"><a href="/master/logpaypaldonations.aspx">Log Donations</a></div>
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
              <div class="head"><a href="/master/logs.aspx">Data Entry Log</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Data Entry Log</div>
              <div class="description">
                <p>Review data entry log for a specific user and compute payments.</p>
              </div>
            </div>
          
            <div class="landing-page-task">
              <div class="head"><a href="/master/SetupHomePageBannerAd.aspx">Setup Home Page Banner Ad</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | Setup Home Page Banner Ad</div>
              <div class="description">
                <ul>
                  <li>Upload the home page banner ad image and set the link URL.</li>
                  <li>Enable or disable the home page banner ad.</li>
                </ul>
              </div>
            </div>
            
            <%--
            <div class="landing-page-task">
              <div class="head"><a href="/admin/votesmartimport.aspx">VoteSmart Data Import</a></div>
              <div class="main-menu-entry">Main Menu: Master Only > | VoteSmart Import</div>
              <div class="description">
                <p>Import VoteSmart candidate information using the VoteSmart api.</p>
              </div>
            </div>
            --%>
          
          </div>
          
          <div class="col" id="Column2" runat="server">

            <div class="landing-page-task">
              <div class="head"><a href="/admin/updateissues.aspx">Issue &amp; Topics</a></div>
              <div class="main-menu-entry">Main Menu: Issues > | Issues &amp; Topics</div>
              <div class="description">
                <ul>
                  <li>Add, change or delete an issue or a topic.</li>
                  <li>An issue is a group of related topics that a candidate can respond to.</li>
                  <li>Topics can be target to national candidates, state candidates, county candidates or local candidates.</li>
                </ul>
              </div>
            </div>

<%--            <div class="landing-page-task">
              <div class="head"><a href="/admin/issues.aspx?state=US">Issue Topics for National Candidates</a></div>
              <div class="main-menu-entry">Main Menu: Issues > | Questions > | National</div>
              <div class="description">
                <ul>
                  <li>Add, change or delete an issue topic from among those we make available only to national candidates (President, US Senator, US House of Representatives).</li>
                  <li>An issue topic is a group of related issue questions.</li>
                  <li>The issue topic page has links to allow changes to the individual questions.</li>
                </ul>
              </div>
            </div>--%>

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
      
    </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
