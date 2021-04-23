using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote.Controls
{
  public partial class AdminMenuControl : UserControl
  {
    private Control _Root;

    public bool HasMenu { get; private set; }

    private void AddCountiesMenu(Control ul)
    {
      // State Admins always see a counties menu
      // Masters see it if they are on a page with a state code
      var stateCode = SecurePage.FindStateCode();
      if (!StateCache.IsValidStateCode(stateCode)) return;
      var template = Empty;
      if (SecurePage.IsMasterUser && !IsNullOrWhiteSpace(stateCode))
        template = "/admin/{page}?state={StateCode}&county={CountyCode}";
      else if (SecurePage.IsStateAdminUser) template = "/admin/{page}?county={CountyCode}";
      if (!IsNullOrWhiteSpace(template))
      {
        template = GetMenuPage(template);
        AddMenuItem(ul, " Counties", GetCountiesMenu(template, stateCode));
      }
    }

    private static void AddElectionsMenu(Control ul)
    {
      // Admins always see an elections menu
      // Masters see it if they are on any Admin page
      string stateCode = null;
      if (SecurePage.IsMasterUser && SecurePage.IsAdminPage)
        stateCode = SecurePage.FindStateCode();
      if (SecurePage.IsAdminUser || StateCache.IsValidStateCode(stateCode))
        AddMenuItem(ul, "Elections",
          SecureAdminPage.GetUpdateElectionsPageUrl(stateCode, SecurePage.FindCountyCode(),
            SecurePage.FindLocalKey()));
    }

    private static void AddJurisdictionsMenu(Control ul)
    {
      var stateCode = SecurePage.FindStateCode();
      AddMenuItem(ul, "Jurisdictions",
        SecureAdminPage.GetUpdateJurisdictionsPageUrl(stateCode,
          SecurePage.FindCountyCode(), SecurePage.FindLocalKey()));
    }

    private void AddLocalsMenu(Control ul)
    {
      // County Admins always see a local menu
      // Masters and State Admins see it if they are on a County or Local Admin page
      string stateCode = null;
      string countyCode = null;
      if (SecurePage.IsCountyAdminUser ||
        (SecurePage.IsMasterUser || SecurePage.IsStateAdminUser) &&
        (SecurePage.IsCountyAdminPage || SecurePage.IsLocalAdminPage))
      {
        stateCode = SecurePage.FindStateCode();
        countyCode = SecurePage.FindCountyCode();
      }
      var template = "/admin/{page}?state={StateCode}&county={CountyCode}&local={LocalKey}";
      if (SecurePage.IsStateAdminUser)
        template = "/admin/{page}?county={CountyCode}&local={LocalKey}";
      else if (SecurePage.IsCountyAdminUser) template = "/admin/{page}?local={LocalKey}";
      template = GetMenuPage(template);
      if (stateCode != null && countyCode != null)
        AddMenuItem(ul, "Locals",
          GetLocalsMenu(template, stateCode, countyCode));
    }

    private static void AddMenuItem(Control ul, string name, string href, string target,
      Control subMenu)
    {
      if (href == null && (subMenu == null || subMenu.Controls.Count == 0)) return;
      var li = new HtmlLi();
      ul.Controls.Add(li);
      var a = new HtmlAnchor();
      li.Controls.Add(a);
      if (!IsNullOrWhiteSpace(href)) a.HRef = href;
      if (!IsNullOrWhiteSpace(target)) a.Target = target;
      a.InnerHtml = name;
      if (subMenu != null) AddSubMenu(li, subMenu);
    }

    private static void AddMenuItem(Control ul, string name, string href = null,
      Control subMenu = null)
    {
      AddMenuItem(ul, name, href, null, subMenu);
    }

    private static void AddMenuItem(Control ul, string name, Control subMenu)
    {
      AddMenuItem(ul, name, null, subMenu);
    }

    private static void AddMasterOfficesMenu(Control parent)
    {
      // Masters only see it if they are on an Admin page
      if (!SecurePage.IsMasterUser) return;
      string stateCode = null;
      if (SecurePage.IsAdminPage) stateCode = SecurePage.FindStateCode();
      if (StateCache.IsValidStateCode(stateCode))
      {
        var ul = CreateDropdownMenu();
        var countyCode = SecurePage.FindCountyCode();
        var localKey = SecurePage.FindLocalKey();
        AddMenuItem(ul, "Manage Offices, Office Templates and Incumbents",
          SecureAdminPage.GetUpdateOfficesPageUrl(stateCode, countyCode, localKey));
        AddMenuItem(ul, "Offices Report",
          SecureAdminPage.GetOfficesPageUrl(stateCode, countyCode, localKey));
        AddMenuItem(ul, "Incumbents Report",
          SecureAdminPage.GetOfficialsPageUrl(stateCode, countyCode, localKey));
        AddMenuItem(ul, "Download County or Local Offices CSV",
          SecureAdminPage.GetAdminFolderPageUrl("DownloadOfficesCsv", "state", stateCode));
        AddMenuItem(parent, "Offices", ul);
      }
    }

    private static void AddPoliticianMenu(Control ul)
    {
      if (!SecurePage.IsPoliticianPage ||
        !SecurePoliticianPage.GetPoliticianKeyExists()) return;
      var name = SecurePoliticianPage.GetPoliticianName();
      AddMenuItem(ul, name, GetPoliticianMenu());
    }

    private static void AddPoliticianMenuItems(Control ul)
    {
      AddMenuItem(ul, "Start", SecurePoliticianPage.MainPageUrl);
      AddMenuItem(ul, "Update Intro", SecurePoliticianPage.UpdateIntroPageUrl);
      AddMenuItem(ul, "Update Issue Responses", SecurePoliticianPage.UpdateInfoPageUrl);
      AddMenuItem(ul, "View Intro Page", SecurePoliticianPage.IntroPageUrl, "Show", null);
      //AddMenuItem(ul, "Update Issues", SecurePoliticianPage.UpdateIssuesPageUrl);
      //AddMenuItem(ul, "View Issues Page ", SecurePoliticianPage.PoliticianIssuePageUrl,
      //  "Show", null);
    }

    private void AddStateAdminItem(Control ul, string name, string template,
      bool includeAll = false)
    {
      if (SecurePage.IsMasterUser)
        AddMenuItem(ul, name, GetStatesMenu(template, includeAll));
      else if (SecurePage.IsStateAdminUser)
        AddMenuItem(ul, name, template.Replace("{StateCode}", SecurePage.UserStateCode));
    }

    private Control GetStatesMenu()
    {
      var ul = CreateDropdownMenu();
      if (SecurePage.IsMasterUser)
      {
        const string template = "/admin/{page}?state={StateCode}";
        var pageTemplate = GetMenuPage(template);
        if (template != pageTemplate)
          AddMenuItem(ul, "This Page", GetStatesMenu(GetMenuPage("/admin/{page}?state={StateCode}")));
        AddMenuItem(ul, "State Admin Page", GetStatesMenu("/admin/?state={StateCode}"));
        AddMenuItem(ul, "Elected Officials",
          GetStatesMenu("/admin/officials.aspx?state={StateCode}"));
      }
      else
      {
        AddMenuItem(ul, "State Admin", "/admin/?state=" + SecurePage.UserStateCode);
        AddMenuItem(ul, "Elected Officials",
          "/admin/officials.aspx?state=" + SecurePage.UserStateCode);
      }
      return ul;
    }

    private static void AddSubMenu(Control li, Control control)
    {
      li.Controls.AddAt(1, control);
      // Mark with ►
      var a = li.Controls[0] as HtmlAnchor;
      Debug.Assert(a != null, "a != null");
      //if (!a.InnerHtml.EndsWith("&#9658;", StringComparison.Ordinal))
        a.InnerHtml += "<span class=\"nav-arrow\"> &#9658;</span>";
    }

    private void BuildMainMenu(Control parent)
    {
      var ul = new HtmlUl();
      _Root = ul;
      parent.Controls.Add(ul);
      ul.Attributes.Add("class", "main-admin-menu");

      if (SecurePage.IsMasterUser)
      {
        AddMenuItem(ul, "Master Panel", "/master/");
        AddJurisdictionsMenu(ul);
        AddMenuItem(ul, "States", GetStatesMenu());
        AddCountiesMenu(ul);
        AddLocalsMenu(ul);
        AddElectionsMenu(ul);
        AddMasterOfficesMenu(ul);
        AddMenuItem(ul, "Parties", GetPartiesMenu());
        AddMenuItem(ul, "Politicians", GetPoliticiansMenu());
        AddPoliticianMenu(ul);
        AddMenuItem(ul, "Issues", GetIssuesMenu());
        AddMenuItem(ul, "Master Only", GetSiteAdminMenu());
        AddMenuItem(ul, "Help", GetMasterHelpMenu());
      }
      else if (SecurePage.IsStateAdminUser)
      {
        AddMenuItem(ul, SecurePage.UserStateCode + " State Admin", "/admin/");
        AddCountiesMenu(ul);
        AddLocalsMenu(ul);
        AddJurisdictionsMenu(ul);
        AddElectionsMenu(ul);
        AddMenuItem(ul, "Offices", GetOfficesMenu());
        AddMenuItem(ul, "Issues", "/admin/issues.aspx");
        AddMenuItem(ul, "Parties", "/admin/parties.aspx");
        AddMenuItem(ul, "Politicians", "/admin/politicians.aspx");
        AddMenuItem(ul, "Help", GetAdminHelpMenu());
      }
      else if (SecurePage.IsCountyAdminUser)
      {
        AddMenuItem(ul,
          Counties.GetCounty(SecurePage.UserStateCode, SecurePage.UserCountyCode) +
          " Admin", "/admin/");
        AddLocalsMenu(ul);
        AddJurisdictionsMenu(ul);
        AddElectionsMenu(ul);
        AddMenuItem(ul, "Offices", GetOfficesMenu());
        AddMenuItem(ul, "Help", GetAdminHelpMenu());
      }
      else if (SecurePage.IsLocalAdminUser)
      {
        AddMenuItem(ul,
          LocalDistricts.GetLocalDistrictByStateCodeLocalKey(SecurePage.UserStateCode,
            SecurePage.UserLocalKey) + " Admin", "/admin/");
        AddJurisdictionsMenu(ul);
        AddElectionsMenu(ul);
        AddMenuItem(ul, "Offices", GetOfficesMenu());
        AddMenuItem(ul, "Help", GetAdminHelpMenu());
      }
      else if (SecurePage.IsPoliticianUser) AddPoliticianMenuItems(ul);
      else if (SecurePage.IsPartyUser) AddMenuItem(ul, "Help", GetPartyHelpMenu());

      // remove an empty menu
      if (ul.Controls.Count == 0) parent.Controls.Remove(ul);
    }

    private void BuildMenu()
    {
      if (!IsNullOrWhiteSpace(VotePage.UserName)) BuildMainMenu(PlaceHolder);
      if (PlaceHolder.Controls.Count == 0) PlaceHolder.Controls.Add(new HtmlHr());
      else HasMenu = true;
    }

    private static HtmlGenericControl CreateDropdownMenu()
    {
      var ul = new HtmlUl();
      ul.Attributes.Add("class", "admin-dropdown");
      return ul;
    }

    private static Control GetAdminHelpMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Instructional Videos", "/admin/videos.aspx");
      return ul;
    }

    private static Control GetPartyHelpMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Instructional Videos", "/party/videos.aspx");
      return ul;
    }

    private static Control GetMasterHelpMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Admin Videos", "/admin/videos.aspx");
      AddMenuItem(ul, "Volunteer/Party Videos", "/party/videos.aspx");
      return ul;
    }

    private CountiesMenuControl GetCountiesMenu(string template, string stateCode)
    {
      var countiesMenu = LoadControl("~/Controls/CountiesMenu.ascx") as CountiesMenuControl;
      Debug.Assert(countiesMenu != null, "countiesMenu != null");
      countiesMenu.HRefTemplate = template;
      countiesMenu.StateCode = stateCode;
      return countiesMenu;
    }

    private static Control GetFederalAndGovernorsElectionsMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "US President", "/admin/updateelections.aspx?state=U1");
      AddMenuItem(ul, "US Senate", "/admin/updateelections.aspx?state=U2");
      AddMenuItem(ul, "US House", "/admin/updateelections.aspx?state=U3");
      AddMenuItem(ul, "State Governors", "/admin/updateelections.aspx?state=U4");
      AddMenuItem(ul, "Remaining Presidential Party Primary Candidates",
        "/admin/updateelections.aspx?state=US");
      AddMenuItem(ul, "Presidential Candidates Templates",
        "/admin/updateelections.aspx?state=PP");
      return ul;
    }

    private static Control GetFederalAndGovernorsIncumbentsMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "US President", "/admin/officials.aspx?state=U1");
      AddMenuItem(ul, "US Senate", "/admin/officials.aspx?state=U2");
      AddMenuItem(ul, "US House", "/admin/officials.aspx?state=U3");
      AddMenuItem(ul, "State Governors", "/admin/officials.aspx?state=U4");
      return ul;
    }

    private Control GetIssuesAnswersMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "All Candidates",
        "/admin/questionanswers.aspx?issuelevel=A&state=LL");
      AddMenuItem(ul, "National", "/admin/questionanswers.aspx?issuelevel=B&state=US");
      AddMenuItem(ul, "States",
        GetStatesMenu("/admin/questionanswers.aspx?issuelevel=C&state={StateCode}"));
      return ul;
    }

    private Control GetIssuesMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Issues &amp; Topics", "/admin/updateissues.aspx");
      AddMenuItem(ul, "Responses (coming soon)", "/404/");
      return ul;
    }

    //private Control GetIssuesQuestionsMenu()
    //{
    //  var ul = CreateDropdownMenu();
    //  AddMenuItem(ul, "All Candidates", "/admin/issues.aspx?state=LL");
    //  AddMenuItem(ul, "National", "/admin/issues.aspx?state=US");
    //  AddMenuItem(ul, "States", GetStatesMenu("/admin/issues.aspx?state={StateCode}"));
    //  return ul;
    //}

    private LocalsMenuControl GetLocalsMenu(string template, string stateCode,
      string countyCode)
    {
      var localsMenu = LoadControl("~/Controls/LocalsMenu.ascx") as LocalsMenuControl;
      Debug.Assert(localsMenu != null, "localsMenu != null");
      localsMenu.HRefTemplate = template;
      localsMenu.StateCode = stateCode;
      localsMenu.CountyCode = countyCode;
      return localsMenu;
    }

    private static string GetMenuPage(string template)
    {
      var page = Empty;
      var path = HttpContext.Current.Request.Path.ToLowerInvariant();
      switch (path)
      {
        case "/admin/updateelections.aspx":
        case "/admin/updatejurisdictions.aspx":
        case "/admin/updateoffices.aspx":
          page = path.Substring(7);
          break;
      }
      return template.Replace("{page}", page);
    }

    private static Control GetOfficesMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Manage Offices, Office Templates and Incumbents",
        "/admin/updateoffices.aspx");
      AddMenuItem(ul, "Offices Report", "/admin/offices.aspx");
      AddMenuItem(ul, "Incumbents Report", "/admin/officials.aspx");
      return ul;
    }

    private Control GetPartiesMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Candidates and Incumbents", GetPartiesCandidatesAndIncumbentsMenu());
      AddMenuItem(ul, "Party Info", GetPartiesPartyInfoMenu());
      return ul;
    }

    private Control GetPartiesCandidatesAndIncumbentsMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Democratic Party", GetStatesMenu("/party?party={StateCode}D"));
      AddMenuItem(ul, "Republican Party", GetStatesMenu("/party?party={StateCode}R"));
      AddMenuItem(ul, "Libertarian Party", GetStatesMenu("/party?party={StateCode}L"));
      AddMenuItem(ul, "Green Party", GetStatesMenu("/party?party={StateCode}G"));
      return ul;
    }

    private Control GetPartiesPartyInfoMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "National", "/admin/parties.aspx?state=US");
      AddMenuItem(ul, "States", GetStatesMenu("/admin/parties.aspx?state={StateCode}"));
      return ul;
    }

    private static Control GetPoliticianMenu()
    {
      var ul = CreateDropdownMenu();
      AddPoliticianMenuItems(ul);
      return ul;
    }

    private Control GetPoliticiansMenu()
    {
      var ul = CreateDropdownMenu();
      AddStateAdminItem(ul, "Update Politicians",
        "/admin/updatepoliticians.aspx?state={StateCode}");
      AddStateAdminItem(ul, "Find Username and Password",
        "/admin/updatepoliticians.aspx?state={StateCode}", true);
      AddMenuItem(ul, "Profile and Headshot Images", "/master/imagesheadshots.aspx");
      AddMenuItem(ul, "By Office",
        GetStatesMenu("/admin/politicians.aspx?state={StateCode}"));
      return ul;
    }

    private static Control GetSiteAdminMenu()
    {
      var ul = CreateDropdownMenu();
      AddMenuItem(ul, "Federal & Governors Elections",
        GetFederalAndGovernorsElectionsMenu());
      AddMenuItem(ul, "Federal & Governors Incumbents",
        GetFederalAndGovernorsIncumbentsMenu());
      AddMenuItem(ul, "Set NoCache for Public Pages", "/master/setnocache.aspx");
      if (SecurePage.IsSuperUser)
      {
        AddMenuItem(ul, "Manage Users", "/master/updateusers.aspx");
        AddMenuItem(ul, "Page Cache Control", "/master/cachecontrol.aspx");
        AddMenuItem(ul, "Nag Dialogs", "/master/nags.aspx");
        AddMenuItem(ul, "Headshots", "/master/imagesheadshots.aspx");
        AddMenuItem(ul, "Bulk Email", "/admin/bulkemail.aspx");
        AddMenuItem(ul, "Bulk Email Delete", "/master/emailbulkdelete.aspx");
        AddMenuItem(ul, "Upload Images for Bulk Email", "/master/uploadimages.aspx");
        AddMenuItem(ul, "Log Donations", "/master/logpaypaldonations.aspx");
        AddMenuItem(ul, "Volunteers", "/master/updatevolunteers.aspx");
        AddMenuItem(ul, "Instructional Videos Maintenance", "/master/videos.aspx");
        AddMenuItem(ul, "BallotPedia CSVs", "/master/ballotpediacsvs.aspx");
        AddMenuItem(ul, "Review YouTubeChannels", "/master/reviewyoutubechannels.aspx");
        AddMenuItem(ul, "Set Staging", "/master/setstaging.aspx");
        AddMenuItem(ul, "Data Entry Log", "/master/logs.aspx");
        AddMenuItem(ul, "Database Statistics", "/master/databasestats.aspx");
        AddMenuItem(ul, "ShapeFile Audit", "/master/auditshapefiles.aspx");
        AddMenuItem(ul, "Update Candidate Ad Rates", "/master/adrates.aspx");
        AddMenuItem(ul, "Update Organizations", "/master/updateorganizations.aspx");
        AddMenuItem(ul, "Organizations CSV", "/master/downloadorganizationscsv.aspx");
        AddMenuItem(ul, "Elections, Offices, Candidates & Ballot Measures CSV", "/admin/officesandcandidatesselection.aspx");
        AddMenuItem(ul, "Setup Home Page Banner Ad", "/master/SetupHomePageBannerAd2.aspx");
      }
      return ul;
    }

    private StatesMenuControl GetStatesMenu(string template, bool includeAll = false)
    {
      var statesMenu = LoadControl("~/Controls/StatesMenu.ascx") as StatesMenuControl;
      Debug.Assert(statesMenu != null, "statesMenu != null");
      statesMenu.HRefTemplate = template;
      statesMenu.IncludeAll = includeAll;
      return statesMenu;
    }

    private static void HighlightCurrentPage(Control ul)
    {
      var request = HttpContext.Current.Request;
      var url = Regex.Replace(request.Path, @"/default\.aspx$", "/",
        RegexOptions.IgnoreCase);
      var query = request.QueryString.ToString();
      var skipQuery = SecurePage.IsStateAdminUser && SecurePage.IsStateAdminPage ||
        SecurePage.IsCountyAdminUser && SecurePage.IsCountyAdminPage ||
        SecurePage.IsLocalAdminUser && SecurePage.IsLocalAdminPage ||
        SecurePage.IsPartyUser && SecurePage.IsPartyPage || SecurePage.IsPoliticianUser &&
        SecurePage.IsPoliticianPage;
      if (!skipQuery && !IsNullOrWhiteSpace(query)) url += "?" + query;
      HighlightCurrentPage(ul, url);
    }

    private static void HighlightCurrentPage(Control control, string url)
    {
      var a = control as HtmlAnchor;
      if (!IsNullOrWhiteSpace(a?.HRef) && a.HRef.IsEqIgnoreCase(url))
        a.AddCssClasses("current");
      foreach (Control child in control.Controls) HighlightCurrentPage(child, url);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var scriptManager = ScriptManager.GetCurrent(Page);
      Debug.Assert(scriptManager != null, "scriptManager != null");
      if (!scriptManager.IsInAsyncPostBack) BuildMenu();
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
      if (_Root != null) HighlightCurrentPage(_Root);
    }
  }
}