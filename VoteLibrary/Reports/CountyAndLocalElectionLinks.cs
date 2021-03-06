using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Reports
{
  //internal class CountyAndLocalElectionLinks : CountyAndLocalLinks
  //{
  //  #region Private

  //  //private static HtmlAnchor CreateAdminElectionAnchor(string electionKey,
  //  //  string anchorText)
  //  //{
  //  //  var a = new HtmlAnchor
  //  //    {
  //  //      HRef = SecureAdminPage.GetElectionReportPageUrl(electionKey),
  //  //      InnerHtml = anchorText
  //  //    };

  //  //  return a;
  //  //}

  //  private static HtmlAnchor CreatePublicElectionAnchor(string electionKey,
  //    string anchorText)
  //  {
  //    var a = new HtmlAnchor
  //    {
  //      HRef = UrlManager.GetElectionPageUri(electionKey)
  //        .ToString(),
  //      InnerHtml = anchorText
  //    };

  //    return a;
  //  }

  //  #endregion Private

  //  #region Protected

  //  protected static HtmlTable CreateHtmlTableWithHeading(ReportUser reportUser,
  //    bool isCountiesReport)
  //  {
  //    var htmlTable =
  //      new HtmlTable {CellSpacing = 0, CellPadding = 0, Border = 0}.AddCssClasses(
  //        reportUser == ReportUser.Public ? "tablePage" : "tableAdmin");

  //    var headerTitle = isCountiesReport
  //      ? "County Elections"
  //      : "Local District Elections";

  //    var tr = new HtmlTableRow().AddTo(htmlTable, "trReportHeading");
  //    new HtmlTableCell
  //    {
  //      Align = "center",
  //      ColSpan = MaxCellsInRow,
  //      InnerHtml = headerTitle
  //    }.AddTo(tr, "tdReportHeading");

  //    tr = new HtmlTableRow().AddTo(htmlTable, "trReportGroupHeading");
  //    new HtmlTableCell
  //    {
  //      ColSpan = MaxCellsInRow,
  //      InnerHtml =
  //        isCountiesReport
  //          ? "Use these links for specific county office contests and ballot measures in this election."
  //          : "Use these links for specific local district and town office contests and ballot measures in this election."
  //    }.AddTo(tr, "tdReportGroupHeading");
  //    return htmlTable;
  //  }

  //  protected static int CreateOneAnchorCell(ReportUser reportUser,
  //    HtmlTable htmlTable, ref HtmlTableRow tr, string name, string electionKey,
  //    int cellsDisplayedInRow)
  //  {
  //    if (MaxCellsInRow == cellsDisplayedInRow)
  //    {
  //      tr = new HtmlTableRow().AddTo(htmlTable, "trLinks");
  //      cellsDisplayedInRow = 0;
  //    }

  //    var td = new HtmlTableCell().AddTo(tr, "tdCountyLocalLinks");
  //    // nobr is deprecated
  //    var nobr = new HtmlGenericControl("nobr").AddTo(td);
  //    //switch (reportUser)
  //    //{
  //    //  case ReportUser.Public:
  //    //    electionAnchor = CreatePublicElectionAnchor(electionKey, name);
  //    //    break;
  //    //  default:
  //    //    electionAnchor = CreateAdminElectionAnchor(electionKey, name);
  //    //    break;
  //    //}
  //    // only public now
  //    var electionAnchor = CreatePublicElectionAnchor(electionKey, name);
  //    electionAnchor.AddTo(nobr);

  //    cellsDisplayedInRow++;
  //    return cellsDisplayedInRow;
  //  }

  //  #endregion Protected
  //}

  //// ReSharper disable ClassNeverInstantiated.Global
  //internal class CountyElectionLinks : CountyAndLocalElectionLinks
  //  // ReSharper restore ClassNeverInstantiated.Global
  //{
  //  #region Public

  //  #region ReSharper disable

  //  // ReSharper disable MemberCanBePrivate.Global
  //  // ReSharper disable MemberCanBeProtected.Global
  //  // ReSharper disable UnusedMember.Global
  //  // ReSharper disable UnusedMethodReturnValue.Global
  //  // ReSharper disable UnusedAutoPropertyAccessor.Global
  //  // ReSharper disable UnassignedField.Global

  //  #endregion ReSharper disable

  //  public static Control GetReport(ReportUser reportUser, string stateElectionKey)
  //  {
  //    var stateCode = Elections.GetStateCodeFromKey(stateElectionKey);

  //    // We get a dictionary of counties with elections that match the stateElectionKey
  //    // Key: countyCode; Value: county electionKey
  //    // Counties without an election will not be in the dictionary
  //    var countyElectionDictionary =
  //      ElectionsOffices.GetCountyAndLocalElections(stateElectionKey);
  //    if (countyElectionDictionary.Count == 0) return null;

  //    // For reporting we start with all counties for the state (it will be in
  //    // order by county name). Then we select only those in the election dictionary
  //    // and reorder for vertical presentation.
  //    var reorderedCounties = CountyCache.GetCountiesByState(stateCode)
  //      .Where(countyElectionDictionary.ContainsKey)
  //      .ToList()
  //      .ReorderVertically(MaxCellsInRow);

  //    var htmlTable = CreateHtmlTableWithHeading(reportUser, true);
  //    HtmlTableRow tr = null;
  //    var cellsDisplayedInRow = MaxCellsInRow; // force new row

  //    foreach (var countyCode in reorderedCounties)
  //    {
  //      var countyName = CountyCache.GetCountyName(stateCode, countyCode);
  //      var countyElectionKey = countyElectionDictionary[countyCode];
  //      cellsDisplayedInRow = CreateOneAnchorCell(reportUser, htmlTable, ref tr,
  //        countyName, countyElectionKey, cellsDisplayedInRow);
  //    }

  //    return htmlTable;
  //  }

  //  #region ReSharper restore

  //  // ReSharper restore UnassignedField.Global
  //  // ReSharper restore UnusedAutoPropertyAccessor.Global
  //  // ReSharper restore UnusedMethodReturnValue.Global
  //  // ReSharper restore UnusedMember.Global
  //  // ReSharper restore MemberCanBeProtected.Global
  //  // ReSharper restore MemberCanBePrivate.Global

  //  #endregion ReSharper restore

  //  #endregion Public
  //}

  //// ReSharper disable ClassNeverInstantiated.Global
  //internal class LocalElectionLinks : CountyAndLocalElectionLinks
  //  // ReSharper restore ClassNeverInstantiated.Global
  //{
  //  #region Public

  //  #region ReSharper disable

  //  // ReSharper disable MemberCanBePrivate.Global
  //  // ReSharper disable MemberCanBeProtected.Global
  //  // ReSharper disable UnusedMember.Global
  //  // ReSharper disable UnusedMethodReturnValue.Global
  //  // ReSharper disable UnusedAutoPropertyAccessor.Global
  //  // ReSharper disable UnassignedField.Global

  //  #endregion ReSharper disable

  //  public static Control GetReport(ReportUser reportUser, string countyElectionKey)
  //  {
  //    var stateCode = Elections.GetStateCodeFromKey(countyElectionKey);
  //    var countyCode = Elections.GetCountyCodeFromKey(countyElectionKey);
  //    var stateElectionKey = Elections.GetStateElectionKeyFromKey(countyElectionKey);

  //    // We get a dictionary of locals with elections that match the stateElectionKey
  //    // Key: localCode; Value: local electionKey
  //    // Locals without an election will not be in the dictionary
  //    var localElectionDictionary =
  //      ElectionsOffices.GetLocalElections(stateElectionKey, countyCode);
  //    if (localElectionDictionary.Count == 0) return null;

  //    // We also get a dictionary of all local names for the county
  //    var localNamesDictionary = LocalDistricts.GetNamesDictionary(stateCode,
  //      countyCode);

  //    // For reporting we filter only locals with elections, sort by name, 
  //    // then reorder for vertical presentation
  //    var reorderedLocals = localNamesDictionary.Where(
  //        kvp => localElectionDictionary.ContainsKey(kvp.Key))
  //      .OrderBy(kvp => kvp.Value)
  //      .ToList()
  //      .ReorderVertically(MaxCellsInRow);

  //    var htmlTable = CreateHtmlTableWithHeading(reportUser, false);
  //    HtmlTableRow tr = null;
  //    var cellsDisplayedInRow = MaxCellsInRow; // force new row

  //    foreach (var kvp in reorderedLocals)
  //    {
  //      var localCode = kvp.Key;
  //      var localName = kvp.Value;
  //      var localElectionKey = localElectionDictionary[localCode];
  //      cellsDisplayedInRow = CreateOneAnchorCell(reportUser, htmlTable, ref tr,
  //        localName, localElectionKey, cellsDisplayedInRow);
  //    }

  //    return htmlTable;
  //  }

  //  #region ReSharper restore

  //  // ReSharper restore UnassignedField.Global
  //  // ReSharper restore UnusedAutoPropertyAccessor.Global
  //  // ReSharper restore UnusedMethodReturnValue.Global
  //  // ReSharper restore UnusedMember.Global
  //  // ReSharper restore MemberCanBeProtected.Global
  //  // ReSharper restore MemberCanBePrivate.Global

  //  #endregion ReSharper restore

  //  #endregion Public
  //}
}