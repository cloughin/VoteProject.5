using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using DB.Vote;
using Vote.Admin;
using Vote.Reports;

namespace Vote
{
  public partial class ElectionExplorerPage : CacheablePage
  {
    public ElectionExplorerPage()
    {
      NoUrlEdit = true;
    }

    #region Caching support

    protected override string GetCacheKey()
    {
      var iframe = GetQueryString("iframe")
        .ToLowerInvariant() == "true";
      var key = UrlManager.GetStateCodeFromHostName() + "." + QueryState + "." +
        QueryElection + "." + QueryCongress + "." + QueryStateSenate + "." +
        QueryStateHouse + "." + QueryCounty + "." + iframe;
      var siteId = UrlManager.CurrentQuerySiteId;
      if (!string.IsNullOrEmpty(siteId))
        key += "." + siteId;

      return key;
    }

    protected override string GetCacheType()
    {
      return "ElectionExplorer";
    }

    #endregion Caching support

    private void FillInDropdowns(string electionKey, string countyCode, 
      string congress, string stateSenate, string stateHouse)
    {
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      StateCache.Populate(StateList, "<select a state>", string.Empty, stateCode);
      CountyCache.Populate(CountyList, stateCode, "<select a county>", string.Empty,
        countyCode);
      // reduce CongressionalDistrict codes from 3 to 2 characters
      if (congress.Length == 3) congress = congress.Substring(1);
      Utility.PopulateFromList(CongressList,
        new List<SimpleListItem> { new SimpleListItem(string.Empty, "<select a congressional district>") }
        .Union(Offices.GetDistrictItems(stateCode, OfficeClass.USHouse)
        .Select(i =>
        {
          i.Value = i.Value.Substring(1);
          return i;
        })), congress);
      Utility.PopulateFromList(StateSenateList,
        new List<SimpleListItem> { new SimpleListItem(string.Empty, "<select a state senate district>") }
        .Union(Offices.GetDistrictItems(stateCode, OfficeClass.StateSenate)), stateSenate);
      Utility.PopulateFromList(StateHouseList,
        new List<SimpleListItem> { new SimpleListItem(string.Empty, "<select a state house district>") }
        .Union(Offices.GetDistrictItems(stateCode, OfficeClass.StateHouse)), stateHouse);
      Utility.PopulateFromList(ElectionList,
        BulkEmailPage.GetPreviewElectionItems(stateCode, string.Empty, string.Empty,
        "<select an election>"),
        electionKey);
    }

    // This should eventually be automatically called
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      // we do this here to guarantee it comes before any Page_OnLoad's
      Page.IncludeCss("/css/vote/allReports.css", "if IE 7", "ie7", "if IE 8",
        "ie8", "if gte IE 9", "ie9");
      var cssFile =
        Path.ChangeExtension(Path.Combine("/css/vote", Request.Path.Substring(1)),
          "css");
      Page.IncludeCss(cssFile, "if IE", "ie", "if IE 7", "ie7", "if IE 8", "ie8",
        "if gte IE 9", "ie9");
    }

    // This should eventually be automatically called
    private void RegisterClientScript()
    {
      var scriptFile =
        Path.ChangeExtension(Path.Combine("/js/vote", Request.Path.Substring(1)),
          "js");
      if (File.Exists(Server.MapPath(scriptFile)))
        scriptFile = Path.ChangeExtension(
          Path.Combine("vote", Request.Path.Substring(1)), null)
          .Replace('\\', '/');
      else scriptFile = "vote/init";
      var cs = Page.ClientScript;
      var type = GetType();
      const string scriptName = "app";
      if (cs.IsStartupScriptRegistered(type, scriptName)) return;
      cs.RegisterStartupScript(type, scriptName,
        string.Format("require(['{0}'], function(){{}});", scriptFile), true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      RegisterClientScript();

      if (Request.QueryString["iframe"] == "true")
      {
        form.AddCssClasses("iframe");
        this.IncludeJs("/js/jq/iframeResizer.contentWindow.min.js");
      }

      var totalContests = 0;
      var ballotMeasures = 0;
      var electionKey = QueryElection;
      var congress = QueryCongress;
      var stateSenate = QueryStateSenate;
      var stateHouse = QueryStateHouse;
      var countyCode = QueryCounty;
      var electionDesc = Elections.GetElectionDesc(electionKey);
      Control report = null;

      if (!string.IsNullOrWhiteSpace(electionKey) &&
        !string.IsNullOrWhiteSpace(congress) &&
        !string.IsNullOrWhiteSpace(stateSenate) &&
        !string.IsNullOrWhiteSpace(stateHouse) && !string.IsNullOrWhiteSpace(countyCode))
      {
        report = BallotReport2Tabbed.GetReport(electionKey, congress, stateSenate,
          stateHouse, countyCode, out totalContests, out ballotMeasures);
      }

      if (totalContests != 0 || ballotMeasures != 0)
      {
        H1.InnerText = electionDesc;
        report.AddTo(ReportPlaceHolder);
        FillInDropdowns(electionKey, countyCode, congress, stateSenate, stateHouse);
      }
      else
      {
        StateCache.Populate(StateList, "<find address or select state>", string.Empty);
        var defaultList = new List<SimpleListItem>
        {
          new SimpleListItem
          {
            Text = "<find address or select state>",
            Value = string.Empty
          }
        };
        Utility.PopulateFromList(CountyList, defaultList);
        Utility.PopulateFromList(CongressList, defaultList);
        Utility.PopulateFromList(StateSenateList, defaultList);
        Utility.PopulateFromList(StateHouseList, defaultList);
        Utility.PopulateFromList(ElectionList, defaultList);
      }
    }
  }
}