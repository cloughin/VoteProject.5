using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  internal class CountyAndLocalOfficialsLinks : CountyAndLocalLinks
  {
    #region Private

    private static HtmlAnchor CreateAdminOfficialsAnchor(string anchorText,
      string stateCode, string countyCode = "")
    {
      var a = new HtmlAnchor
      {
        HRef =
          SecureAdminPage.GetOfficialsPageUrl(stateCode, countyCode),
        InnerHtml = anchorText
      };
      return a;
    }

    private static HtmlAnchor CreatePublicOfficalsAnchor(string anchorText,
      string stateOrFederalCode, string countyCode = "")
    {
      var a = new HtmlAnchor
      {
        HRef =
          UrlManager.GetOfficialsPageUri(stateOrFederalCode, countyCode)
            .ToString(),
        InnerHtml = anchorText
      };
      return a;
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected static HtmlTable CreateHtmlTableWithHeading(ReportUser reportUser,
      string stateCode, string countyCode = "")
    {
      var isCountiesReport = IsNullOrWhiteSpace(countyCode);

      var htmlTable =
        new HtmlTable {CellSpacing = 0, CellPadding = 0, Border = 0}.AddCssClasses(
          reportUser == ReportUser.Public ? "tablePage" : "tableAdmin");

      var headerTitle =
        Offices.GetElectoralClassDescription(stateCode, countyCode, Empty) +
        (isCountiesReport
          ? " Counties' Elected Representatives"
          : " Local Districts");

      var tr = new HtmlTableRow().AddTo(htmlTable, "trReportGroupHeading");
      new HtmlTableCell
      {
        Align = "center",
        ColSpan = MaxCellsInRow,
        InnerHtml = headerTitle
      }.AddTo(tr, "tdReportGroupHeading");

      tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
      new HtmlTableCell
      {
        ColSpan = MaxCellsInRow,
        InnerHtml =
          isCountiesReport
            ? "Use these links to view county representatives."
            : "Use these links to view local representatives."
      }.AddTo(tr,
        "tdReportDetail");

      return htmlTable;
    }

    protected static int CreateOneAnchorCell(ReportUser reportUser,
      HtmlTable htmlTable, ref HtmlTableRow tr, string name,
      int cellsDisplayedInRow, string stateCode, string countyCode)
    {
      if (MaxCellsInRow == cellsDisplayedInRow)
      {
        tr = new HtmlTableRow().AddTo(htmlTable, "trLinks");
        cellsDisplayedInRow = 0;
      }

      HtmlAnchor officialsAnchor = null;
      switch (reportUser)
      {
        case ReportUser.Public:
          officialsAnchor = CreatePublicOfficalsAnchor(name, stateCode, countyCode);
          break;
        case ReportUser.Master:
        case ReportUser.Admin:
          officialsAnchor = CreateAdminOfficialsAnchor(name, stateCode, countyCode);
          break;
      }

      var td = new HtmlTableCell().AddTo(tr, "tdCountyLocalLinks");
      officialsAnchor.AddTo(td);

      cellsDisplayedInRow++;
      return cellsDisplayedInRow;
    }

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }

// ReSharper disable ClassNeverInstantiated.Global
  internal class CountyOfficialsLinks : CountyAndLocalOfficialsLinks
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

    public static Control GetReport(ReportUser reportUser, string stateCode)
    {
      // We get a dictionary of counties with county or local offices
      var hasCountyOrLocalOfficesByCounty =
        Offices.HasCountyOrLocalOfficesByCounty(stateCode);
      if (hasCountyOrLocalOfficesByCounty.Count == 0) return null;

      // For reporting we start with all counties for the state (it will be in
      // order by county name). Then we select only those in the offices dictionary
      // and reorder for vertical presentation.
      var reorderedCounties = CountyCache.GetCountiesByState(stateCode)
        .Where(hasCountyOrLocalOfficesByCounty.ContainsKey)
        .ToList()
        .ReorderVertically(MaxCellsInRow);

      var htmlTable = CreateHtmlTableWithHeading(reportUser, stateCode);
      HtmlTableRow tr = null;
      var cellsDisplayedInRow = MaxCellsInRow; // force new row

      foreach (var iteratedCountyCode in reorderedCounties)
      {
        var countyName = CountyCache.GetCountyName(stateCode, iteratedCountyCode);
        cellsDisplayedInRow = CreateOneAnchorCell(reportUser, htmlTable, ref tr,
          countyName, cellsDisplayedInRow, stateCode, iteratedCountyCode);
      }

      return htmlTable;
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