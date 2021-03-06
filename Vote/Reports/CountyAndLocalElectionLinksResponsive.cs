using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Reports
{
  internal class CountyAndLocalElectionLinksResponsive : Report
  {
    #region Protected

    protected static HtmlAnchor CreatePublicElectionAnchor(string electionKey,
      string anchorText, bool openAll = false)
    {
      var href = UrlManager.GetElectionPageUri(electionKey).ToString();
      if (openAll) href += "&openall=Y";
      var a = new HtmlAnchor
      {
        HRef = href,
        InnerHtml = anchorText
      };

      return a;
    }

    #endregion Protected
  }

  // ReSharper disable ClassNeverInstantiated.Global
  internal class CountyElectionLinksResponsive : CountyAndLocalElectionLinksResponsive
    // ReSharper restore ClassNeverInstantiated.Global
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static void GetReport(Control container, string stateElectionKey, bool openAll = false)
    {
      var stateCode = Elections.GetStateCodeFromKey(stateElectionKey);

      // We get a dictionary of counties with elections that match the stateElectionKey
      // Key: countyCode; Value: county electionKey
      // Counties without an election will not be in the dictionary
      // Update: we now include local elections too, to account for situations where there is no county
      // election but there are local elections.
      var countyElectionDictionary =
        ElectionsOffices.GetCountyAndLocalElections(stateElectionKey);
      // We can't forget the Ballot Measures...
      var countyReferendumDictionary =
        Referendums.GetCountyAndLocalElections(stateElectionKey);
      // merge them into the first dictionary
      foreach (var kvp in countyReferendumDictionary)
        if (!countyElectionDictionary.ContainsKey(kvp.Key))
          countyElectionDictionary.Add(kvp.Key, kvp.Value);
      if (countyElectionDictionary.Count == 0) return;

      new HtmlDiv {InnerText = "County Elections"}.AddTo(container, "accordion-header");
      var content = new HtmlDiv().AddTo(container, "local-anchors accordion-content");

      // For reporting we start with all counties for the state (it will be in
      // order by county name) and select only those in the election dictionary.

      var counties = CountyCache.GetCountiesByState(stateCode)
        .Where(countyElectionDictionary.ContainsKey)
        .ToList();

      foreach (var countyCode in counties)
      {
        var countyName = CountyCache.GetCountyName(stateCode, countyCode);
        var countyElectionKey = countyElectionDictionary[countyCode];
        CreatePublicElectionAnchor(countyElectionKey, countyName, openAll)
          .AddTo(content, "local-anchor");
      }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  // ReSharper disable ClassNeverInstantiated.Global
  internal class LocalElectionLinksResponsive : CountyAndLocalElectionLinksResponsive
    // ReSharper restore ClassNeverInstantiated.Global
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static void GetReport(Control container, string countyElectionKey, bool openAll = false)
    {
      var stateCode = Elections.GetStateCodeFromKey(countyElectionKey);
      var countyCode = Elections.GetCountyCodeFromKey(countyElectionKey);
      var stateElectionKey = Elections.GetStateElectionKeyFromKey(countyElectionKey);

      // We get a dictionary of locals with elections that match the stateElectionKey
      // Key: localCode; Value: local electionKey
      // Locals without an election will not be in the dictionary
      var localElectionDictionary =
        ElectionsOffices.GetLocalElections(stateElectionKey, countyCode);
      // We can't forget the Ballot Measures...
      var localReferendumDictionary =
        Referendums.GetLocalElections(stateElectionKey, countyCode);
      // merge them into the first dictionary
      foreach (var kvp in localReferendumDictionary)
        if (!localElectionDictionary.ContainsKey(kvp.Key))
          localElectionDictionary.Add(kvp.Key, kvp.Value);
      if (localElectionDictionary.Count == 0) return;

      // We also get a dictionary of all local names for the county
      var localNamesDictionary = LocalDistricts.GetNamesDictionary(stateCode,
        countyCode);

      new HtmlDiv {InnerText = "Local District Elections"}.AddTo(container, "accordion-header");
      var content = new HtmlDiv().AddTo(container, "local-anchors accordion-content");

      // For reporting we filter only locals with elections and sort by name, 
      var locals = localNamesDictionary.Where(
          kvp => localElectionDictionary.ContainsKey(kvp.Key))
        .OrderBy(kvp => kvp.Value)
        .ToList();

      foreach (var kvp in locals)
      {
        var localCode = kvp.Key;
        var localName = kvp.Value;
        var localElectionKey = localElectionDictionary[localCode];
        CreatePublicElectionAnchor(localElectionKey, localName, openAll)
          .AddTo(content, "local-anchor");
      }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}