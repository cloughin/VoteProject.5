using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote
{
  public class LinkInfo
  {
    public string Description;
    public Uri HRef;

    public LinkInfo() { }

    public LinkInfo(ElectionsRow electionsRow)
    {
      Description = electionsRow.ElectionDesc;
    }
  }

  public partial class ForVotersPage : CacheablePage
  {
    #region Caching support

    protected override void OnPreInit(EventArgs e)
    {
      Response.Redirect(UrlManager.GetSiteUri().ToString());
      Response.End();
    }

    protected override string GetCacheKey()
    {
      var cookies = LocationCookies.GetCookies();
      var key = UrlManager.GetStateCodeFromHostName();
      if (cookies.IsValid)
        key =
          $"{UrlManager.GetStateCodeFromHostName()}.{cookies.Congress}.{cookies.StateSenate}." +
          $"{cookies.StateHouse}.{cookies.County}.{cookies.District}.{cookies.Place}." + 
          $"{cookies.Elementary}.{cookies.Secondary}.{cookies.Unified}.{cookies.CityCouncil}." +
          $"{cookies.CountySupervisors}.{cookies.SchoolDistrictDistrict}";

      return key;
    }

    protected override string GetCacheType()
    {
      return "forVoters";
    }

    #endregion Caching support

    private const string TitleTagDefaultPageSingleStateDomain = 
      "[[state]] Sample Ballots, Candidates & Election Voting Information";
    private const string MetaDescriptionTagDefaultPageSingleStateDomain = 
      "[[state]] sample ballots using voter's address with comparisons of candidates' pictures, bios, social links and positions and views on the issues.";
    private const string TitleTagDefaultPageAllStatesDomain = 
      "Custom Sample Ballots, Candidates & Election Voting Information";
    private const string MetaDescriptionTagDefaultPageAllStatesDomain = 
      "Custom sample ballots using voter's address with comparisons of candidates' pictures, bios, social links and positions and views on the issues.";

    private static List<LinkInfo> GetUpcomingLinks(string stateCode)
    {
      var linkList = new List<LinkInfo>();

      if (StateCache.IsValidStateCode(stateCode))
      {
        var table = Elections.GetFutureViewableDisplayDataByStateCode(stateCode);
        linkList.AddRange(
          table.Select(
            row =>
              new LinkInfo(row)
              {
                HRef = UrlManager.GetElectionPageUri(row.ElectionKey)
              }));
      }

      return linkList;
    }

    private static void AddUpcomingLinksToDiv(Control div,
      string stateCode)
    {
      var links = GetUpcomingLinks(stateCode);
      if (links.Count > 0)
        ForResearchPage.AddLinksToLinkBox(div, links);
      else
      {
        var p = new HtmlP().AddTo(div);
        var message =
          $"No {StateCache.GetStateName(stateCode)} upcoming election reports are available. " +
          "Please check back later.";
        new LiteralControl(message).AddTo(p);
      }
    }

    private void CreateUpcomingLinks(string stateCode)
    {
      // Sample Ballot links
      if (DomainData.IsValidStateCode) // Single state
        AddUpcomingLinksToDiv(Upcoming, stateCode);
    }

    private static List<LinkInfo> GetRecentSampleBallotLinks(LocationCookies cookies)
    {
      var linkList = new List<LinkInfo>();

      if (StateCache.IsValidStateCode(cookies.State))
      {
        var table =
          Elections.GetViewableDisplayDataByStateCode(cookies.State);

        foreach (var row in table)
        {
          var linkInfo = new LinkInfo(row)
          {
            HRef =
              UrlManager.GetBallotPageUri(row.ElectionKey, cookies.Congress,
                cookies.StateSenate, cookies.StateHouse, cookies.County, cookies.District,
                cookies.Place, cookies.Elementary, cookies.Secondary, cookies.Unified, 
                cookies.CityCouncil, cookies.CountySupervisors, 
                cookies.SchoolDistrictDistrict, null)
          };
          linkList.Add(linkInfo);
          // go back to most recent general or offyear election
          if (row.ElectionType == "G" || row.ElectionType == "O")
            break;
        }
      }

      return linkList;
    }

    private static HtmlControl CreateSampleBallotButton(LinkInfo link)
    {
      var button = new HtmlP().AddCssClasses("button");
      new HtmlAnchor { HRef = link.HRef.ToString(), InnerText =
        $"Interactive Sample Ballot for the {link.Description}"
      }.AddTo(button, "link-button");
      return button;
    }

    private static void GetLinkBox(Control linkBox, string title,
      IEnumerable<LinkInfo> links)
    {
      new HtmlDiv { InnerText = title }.AddTo(linkBox, "link-header");
      var div = new HtmlDiv().AddTo(linkBox, "links");

      ForResearchPage.AddLinksToLinkBox(div, links);
    }

    private void SetupHeading(LocationCookies cookies, IEnumerable<LinkInfo> links)
    {
      if (cookies.IsValid)
      {
        HeaderArea.AddCssClasses("has-address has-state");
        ElectedOfficialsButton.Visible = true;
        StatewideButton.Visible = true;

        if (links != null)
          foreach (var button in links.Select(CreateSampleBallotButton))
            button.AddTo(SampleBallotPlaceHolder);
        ElectedOfficialsLink.HRef =
          UrlManager.GetElectedPageUri(cookies.State, cookies.Congress, cookies.StateSenate, 
           cookies.StateHouse, cookies.County, cookies.District, cookies.Place, cookies.Elementary, 
           cookies.Secondary, cookies.Unified, cookies.CityCouncil, cookies.CountySupervisors,
           cookies.SchoolDistrictDistrict)
            .ToString();
        StatewideLink.HRef = UrlManager.GetOfficialsPageUri(cookies.State)
          .ToString();
      }
      else if (DomainData.IsValidStateCode)
      {
        HeaderArea.AddCssClasses("no-address has-state");
        ElectedOfficialsButton.Visible = false;
        StatewideButton.Visible = true;
        StatewideLink.HRef =
          UrlManager.GetOfficialsPageUri(DomainData.FromQueryStringOrDomain)
            .ToString();
      }
      else
      {
        HeaderArea.AddCssClasses("no-address no-state");
        ElectedOfficialsButton.Visible = false;
        StatewideButton.Visible = false;
      }
      if (IsPostBack)
        AddressEntry.Submit();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var substitutions = new Substitutions();
      if (DomainData.IsValidStateCode) // Single state
      {
        Title =
          $"{PublicMasterPage.SiteName} | {substitutions.Substitute(TitleTagDefaultPageSingleStateDomain)}";
        MetaDescription = substitutions.Substitute(MetaDescriptionTagDefaultPageSingleStateDomain);
      }
      else // use the All states domain
      {
        Title =
          $"{PublicMasterPage.SiteName} | {substitutions.Substitute(TitleTagDefaultPageAllStatesDomain)}";
        MetaDescription = substitutions.Substitute(MetaDescriptionTagDefaultPageAllStatesDomain);
      }

      // My Sample Ballot Buttons

      var cookies = LocationCookies.GetCookies();
      List<LinkInfo> links = null;
      var stateCode = DomainData.FromQueryStringOrDomain;
      if (cookies.IsValid)
        links = GetRecentSampleBallotLinks(cookies);
      ExplorerLinkBox.Visible = false;

      SetupHeading(cookies, links);

      if (DomainData.IsValidStateCode)
        CreateUpcomingLinks(stateCode);
      else
        UpcomingLinkBox.Visible = false;

      GetLinkBox(PresidentLinks, "US President and Vice-President",
        ForResearchPage.GetPresidentLinks());
      GetLinkBox(SenateLinks, "US Senate",
        ForResearchPage.GetSenateLinks());
      GetLinkBox(HouseLinks, "US House of Representatives",
        ForResearchPage.GetHouseLinks());
      GetLinkBox(GovernorLinks,
        "State Governors and Lieutenant Governors",
        ForResearchPage.GetGovernorLinks());

      if (DomainData.IsValidStateCode)
        ForResearchPage.AddStateLinksToDiv(ElectionResults, stateCode, false, false);
      else
        ElectionResultsLinkBox.Visible = false;
    }
  }
}