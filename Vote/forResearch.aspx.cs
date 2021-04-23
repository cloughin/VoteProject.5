using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class ForResearchPage : CacheablePage
  {
    #region Caching support

    protected override string GetCacheKey()
    {
      return UrlManager.GetStateCodeFromHostName();
    }

    protected override string GetCacheType()
    {
      return "forResearch";
    }

    #endregion Caching support

    private const string TitleTag = "{0} | Historical Voter Information for Elections, Office Contests and Candidates";
    private const string MetaDescriptionTag = "Historical voter information for {0}elections, office contests" +
      " and candidates, including pictures, bios, position statements and social media links back to 2004.";

    private static IEnumerable<LinkInfo> GetStateLinks(string stateCode,
      bool includeFutureElections)
    {
      var linkList = new List<LinkInfo>();

      if (StateCache.IsValidStateCode(stateCode))
      {
        var table =
          Elections.GetViewableDisplayDataByStateCode(stateCode);

        var now = DateTime.UtcNow;
        foreach (var row in table)
          if (includeFutureElections || row.ElectionDate < now)
          {
            var linkInfo = new LinkInfo(row)
            {
              HRef = UrlManager.GetElectionPageUri(row.ElectionKey, false, false, true)
            };
            linkList.Add(linkInfo);
          }
      }

      return linkList;
    }

    private static IEnumerable<LinkInfo> GetLinksByCode(string code,
      string currentDescription, string electionType = "G")
    {
      var linkList = new List<LinkInfo>();

      var linkInfo = new LinkInfo
      {
        Description = currentDescription,
        HRef = UrlManager.GetOfficialsPageUri(code)
      };
      linkList.Add(linkInfo);

      var table = Elections.GetDisplayDataByStateCodeElectionTypeIsViewable(code,
        electionType, true);

      foreach (var row in table)
      {
        linkInfo = new LinkInfo(row)
        {
          HRef = UrlManager.GetElectionPageUri(row.ElectionKey)
        };
        linkList.Add(linkInfo);
      }

      return linkList;
    }

    public static IEnumerable<LinkInfo> GetPresidentLinks()
    {
      return GetLinksByCode("U1", "Current President and Vice President");
    }

    public static IEnumerable<LinkInfo> GetSenateLinks()
    {
      return GetLinksByCode("U2", "Current US Senators");
    }

    public static IEnumerable<LinkInfo> GetHouseLinks()
    {
      return GetLinksByCode("U3", "Current US House of Representatives");
    }

    public static IEnumerable<LinkInfo> GetGovernorLinks()
    {
      return GetLinksByCode("U4",
        "Current State Governors and Lieutenant Governors", null);
    }

    public static void AddLinksToLinkBox(Control div, IEnumerable<LinkInfo> links,
      string target = null)
    {
      foreach (var link in links)
        AddLinkToLinkBox(div, link, target);
    }

    private static void AddLinkToLinkBox(Control div, LinkInfo link, string target)
    {
      var p = new HtmlP().AddTo(div);
      new HtmlAnchor
      {
        HRef = link.HRef.ToString(),
        InnerText = link.Description,
        Title = link.Description,
        Target = target
      }.AddTo(p);
    }

    public static void AddStateLinksToDiv(HtmlGenericControl div, string stateCode,
      bool includeCurrentOfficials, bool includeFutureElections)
    {
      if (includeCurrentOfficials)
      {
        var linkInfo = new LinkInfo
        {
          Description =
            StateCache.GetStateName(stateCode) + " Current Elected Officials",
          HRef = UrlManager.GetOfficialsPageUri(stateCode)
        };
        AddLinksToLinkBox(div, new List<LinkInfo> { linkInfo });
      }

      AddLinksToLinkBox(div, GetStateLinks(stateCode, includeFutureElections));
    }

    private static void GetLinkBox(Control linkBox, string title,
      IEnumerable<LinkInfo> links)
    {
      new HtmlDiv { InnerText = title }.AddTo(linkBox, "link-header");
      var div = new HtmlDiv().AddTo(linkBox, "links");

      AddLinksToLinkBox(div, links);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var stateName = PublicMasterPage.StateName + " ";
        if (stateName == "US ") stateName = Empty;
        Title = Format(TitleTag, PublicMasterPage.SiteName);
        MetaDescription = Format(MetaDescriptionTag, stateName);
      }

      string selectedState = null;
      string firstEntry = null;

      if (DomainData.IsValidStateCode) // Single state
      {
        selectedState = DomainData.FromQueryStringOrDomain;
        AddStateLinksToDiv(StateLinkEntries, selectedState, true, true);
        StateName.InnerText = StateCache.GetStateName(selectedState);
      }
      else
      {
        firstEntry = "<select state>";
        new HtmlP { InnerText = "No state selected" }.AddTo(StateLinkEntries);
        StateName.InnerText = "<no state selected>";
      }

      StateCache.Populate(StateElectionDropDown, firstEntry, "XX", selectedState);

      GetLinkBox(PresidentLinks, "US President and Vice-President",
        GetPresidentLinks());
      GetLinkBox(SenateLinks, "US Senate", GetSenateLinks());
      GetLinkBox(HouseLinks, "US House of Representatives", GetHouseLinks());
      GetLinkBox(GovernorLinks, "State Governors and Lieutenant Governors",
        GetGovernorLinks());
    }
  }
}