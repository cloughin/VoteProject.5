using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Reports
{
  internal class CountyAndLocalLinks : Report
  {
    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected const int MaxCellsInRow = 4;

    protected static HtmlAnchor CreateAdminDefaultAnchor(string stateCode,
      string countyCode, string localCode, string anchorText, string title = "",
      string target = "")
    {
      return new HtmlAnchor
      {
        HRef = SecureAdminPage.GetDefaultPageUrl(stateCode, countyCode, localCode),
        InnerHtml = anchorText,
        Title = title,
        Target = target
      };
    }

    protected static HtmlAnchor CreateAdminDistrictsAnchor(string stateCode,
      string countyCode, string localCode, string anchorText, string title = "",
      string target = "")
    {
      return new HtmlAnchor
      {
        HRef =
          SecureAdminPage.GetDistrictsPageUrl(stateCode, countyCode, localCode),
        InnerHtml = anchorText,
        Title = title,
        Target = target
      };
    }

    //protected static HtmlAnchor CreateAdminElectionAnchor(string electionKey,
    //  string anchorText, string title = "", string target = "")
    //{
    //  return new HtmlAnchor
    //    {
    //      HRef = SecureAdminPage.GetElectionReportPageUrl(electionKey),
    //      InnerHtml = anchorText,
    //      Title = title,
    //      Target = target
    //    };
    //}

    //protected static HtmlAnchor CreateAdminElectionOfficesAnchor(
    //  string electionKey, string anchorText, string title = "", string target = "")
    //{
    //  return new HtmlAnchor
    //    {
    //      HRef = SecureAdminPage.GetElectionOfficesPageUrl(electionKey),
    //      InnerHtml = anchorText,
    //      Title = title,
    //      Target = target
    //    };
    //}

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }

// ReSharper disable ClassNeverInstantiated.Global
  internal sealed class CountyLinks : CountyAndLocalLinks
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

    public static Control GetReport(string stateCode,
      Func<string, string, string, Control> getAnchor, bool sortByCode = false)
    {
      const int maxCellsInRow = 5; // "override" default of 4

      var counties = CountyCache.GetCountiesByState(stateCode)
        .AsEnumerable();
      if (sortByCode) counties = counties.OrderBy(code => code);
      var reorderedCounties = counties.ReorderVertically(maxCellsInRow);

      var htmlTable =
        new HtmlTable {CellSpacing = 0, Border = 0}.AddCssClasses("tableAdmin");
      HtmlTableRow tr = null;
      var cellsInCurrentRow = int.MaxValue; // force initial new row

      foreach (var countyCode in reorderedCounties)
      {
        if (cellsInCurrentRow >= maxCellsInRow)
        {
          tr = new HtmlTableRow().AddTo(htmlTable, "trLinks");
          cellsInCurrentRow = 0;
        }
        cellsInCurrentRow++;

        var td = new HtmlTableCell().AddTo(tr, "tdLinks");
        getAnchor(stateCode, countyCode,
            CountyCache.GetCountyName(stateCode, countyCode))
          .AddTo(td);
      }

      return htmlTable;
    }

    public static Control GetDefaultCountyLinks(string stateCode,
      bool sortByCode = false)
    {
      return GetReport(stateCode,
        (s, c, name) => CreateAdminDefaultAnchor(s, c, null, name), sortByCode);
    }

    //public static Control GetElectionCountyLinks(string electionKey, bool sortByCode)
    //{
    //  return GetElectionCountyLinks(electionKey, null, sortByCode);
    //}

    //public static Control GetElectionCountyLinks(string electionKey,
    //  string stateCode = null, bool sortByCode = false)
    //{
    //  if (string.IsNullOrWhiteSpace(stateCode))
    //    stateCode = Elections.GetStateCodeFromKey(electionKey);

    //  return GetReport(stateCode, 
    //    (s, c, name) =>
    //      CreateAdminElectionAnchor(
    //        Elections.GetCountyElectionKeyFromKey(electionKey, s, c), name),
    //    sortByCode);
    //}

    //public static Control GetElectionOfficesCountyLinks(string electionKey,
    //  bool sortByCode)
    //{
    //  return GetElectionOfficesCountyLinks(electionKey, null, sortByCode);
    //}

    //public static Control GetElectionOfficesCountyLinks(string electionKey,
    //  string stateCode = null, bool sortByCode = false)
    //{
    //  if (string.IsNullOrWhiteSpace(stateCode))
    //    stateCode = Elections.GetStateCodeFromKey(electionKey);

    //  return GetReport(stateCode,
    //    (s, c, name) =>
    //      CreateAdminElectionOfficesAnchor(
    //        Elections.GetCountyElectionKeyFromKey(electionKey, s, c), name),
    //    sortByCode);
    //}

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
  internal sealed class LocalLinks : CountyAndLocalLinks
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

    public static Control GetReport(string stateCode, string countyCode,
      Func<string, string, string, string, Control> getAnchor,
      bool sortByCode = false)
    {
      var localsDictionary = LocalDistricts.GetNamesDictionary(stateCode,
        countyCode);
      var reorderedLocals = localsDictionary.OrderBy(
          kvp => sortByCode ? kvp.Key : kvp.Value)
        .ReorderVertically(MaxCellsInRow);

      var htmlTable =
        new HtmlTable {CellSpacing = 0, Border = 0}.AddCssClasses("tableAdmin");
      HtmlTableRow tr = null;
      var cellsInCurrentRow = int.MaxValue; // force initial new row

      foreach (var local in reorderedLocals)
      {
        if (cellsInCurrentRow >= MaxCellsInRow)
        {
          tr = new HtmlTableRow().AddTo(htmlTable, "trLinks");
          cellsInCurrentRow = 0;
        }
        cellsInCurrentRow++;

        var td = new HtmlTableCell().AddTo(tr, "tdLinks");
        getAnchor(stateCode, countyCode, local.Key, local.Value)
          .AddTo(td);
      }

      return htmlTable;
    }

    public static Control GetDefaultLocalLinks(string stateCode, string countyCode,
      bool sortByCode = false, bool showCode = false)
    {
      return GetReport(stateCode, countyCode,
        (s, c, l, name) =>
            CreateAdminDefaultAnchor(s, c, l, name + (showCode ? " (" + l + ")" : string.Empty)),
        sortByCode);
    }

    public static Control GetDistrictsLocalLinks(string stateCode,
      string countyCode, bool sortByCode = false)
    {
      return GetReport(stateCode, countyCode,
        (s, c, l, name) =>
            CreateAdminDistrictsAnchor(s, c, l, name + " (" + l + ")"), sortByCode);
    }

    //public static Control GetElectionLocalLinks(string electionKey, bool sortByCode)
    //{
    //  return GetElectionLocalLinks(electionKey, null, null, sortByCode);
    //}

    //public static Control GetElectionLocalLinks(string electionKey,
    //  string stateCode = null, string countyCode = null, bool sortByCode = false)
    //{
    //  if (string.IsNullOrWhiteSpace(stateCode))
    //    stateCode = Elections.GetStateCodeFromKey(electionKey);
    //  if (string.IsNullOrWhiteSpace(countyCode))
    //    countyCode = Elections.GetCountyCodeFromKey(electionKey);

    //  return GetReport(stateCode, countyCode,
    //    (s, c, l, name) =>
    //      CreateAdminElectionAnchor(
    //        Elections.GetLocalElectionKeyFromKey(electionKey, s, c, l), name),
    //    sortByCode);
    //}

    //public static Control GetElectionOfficesLocalLinks(string electionKey,
    //  bool sortByCode)
    //{
    //  return GetElectionOfficesLocalLinks(electionKey, null, null, sortByCode);
    //}

    //public static Control GetElectionOfficesLocalLinks(string electionKey,
    //  string stateCode = null, string countyCode = null, bool sortByCode = false)
    //{
    //  if (string.IsNullOrWhiteSpace(stateCode))
    //    stateCode = Elections.GetStateCodeFromKey(electionKey);
    //  if (string.IsNullOrWhiteSpace(countyCode))
    //    countyCode = Elections.GetCountyCodeFromKey(electionKey);

    //  return GetReport(stateCode, countyCode,
    //    (s, c, l, name) =>
    //      CreateAdminElectionOfficesAnchor(
    //        Elections.GetLocalElectionKeyFromKey(electionKey, s, c, l), name),
    //    sortByCode);
    //}

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