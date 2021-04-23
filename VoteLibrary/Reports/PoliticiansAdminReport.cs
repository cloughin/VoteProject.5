using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class PoliticiansAdminReportOptions : PoliticiansAdminReportViewOptions
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    public bool SortByOffice;
#pragma warning disable 649
    public bool? AsMaster;
#pragma warning restore 649

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  internal sealed class PoliticiansAdminReportDataManager :
    ReportDataManager<PoliticiansAdminReportViewRow>
  {
    public void GetData(PoliticiansAdminReportViewOptions options)
    {
      DataTable = PoliticiansAdminReportView.GetData(options);
    }
  }

  public class PoliticiansAdminReport : Report
  {
    #region Private

    private bool _AsMaster;
    private PoliticiansAdminReportOptions _Options;

    #region DataManager

    private readonly PoliticiansAdminReportDataManager _DataManager =
      new PoliticiansAdminReportDataManager();

    private class SortByName :
      ReportDataManager<PoliticiansAdminReportViewRow>.OrderBy
    {
      public override int Compare(PoliticiansAdminReportViewRow row1,
        PoliticiansAdminReportViewRow row2)
      {
        Debug.Assert(row1 != null, "row1 != null");
        Debug.Assert(row2 != null, "row2 != null");
        var result = string.Compare(row1.LastName, row2.LastName,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.FirstName, row2.FirstName,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.MiddleName, row2.MiddleName,
          StringComparison.OrdinalIgnoreCase);
      }
    }

    private class SortByOffice :
      ReportDataManager<PoliticiansAdminReportViewRow>.OrderBy
    {
      public override int Compare(PoliticiansAdminReportViewRow row1,
        PoliticiansAdminReportViewRow row2)
      {
        Debug.Assert(row1 != null, "row1 != null");
        Debug.Assert(row2 != null, "row2 != null");
        var result = row1.OfficeLevel.CompareTo(row2.OfficeLevel);
        if (result != 0) return result;
        result = string.Compare(row1.DistrictCode, row2.DistrictCode,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.OfficeLine1, row2.OfficeLine1,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.LastName, row2.LastName,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.FirstName, row2.FirstName,
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.MiddleName, row2.MiddleName,
          StringComparison.OrdinalIgnoreCase);
      }
    }

    #endregion DataManager

    private static HtmlAnchor CreateAdminPoliticianAnchor(
      PoliticiansAdminReportViewRow row, string anchorText, string target)
    {
      var a = new HtmlAnchor
      {
        HRef = SecurePoliticianPage.GetUpdateIntroPageUrl(row.PoliticianKey),
        Title = "Edit Links, Picture, Bio & Reasons",
        InnerHtml = anchorText,
        Target = IsNullOrWhiteSpace(target) ? "politician" : target
      };
      return a;
    }

    private static HtmlTableRow CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddCssClasses("trReportDetailHeading");

      new HtmlTableCell
      {
        InnerHtml =
          "Last Name .. <u>Link to Edit Politician</u> (Party Code) .. Politician ID"
      }.AddTo(tr, "tdReportDetailHeading");

      new HtmlTableCell {InnerHtml = "Links to Edit Office"}.AddTo(tr,
        "tdReportDetailHeading");

      return tr;
    }

    private static HtmlTableRow CreateNoPoliticiansRow()
    {
      var tr = new HtmlTableRow().AddCssClasses("trReportDetail");
      new HtmlTableCell {InnerHtml = "--- No Politicians ---"}.AddTo(tr,
        "tdReportDetail");
      new HtmlTableCell {InnerHtml = "--- No Politicians ---"}.AddTo(tr,
        "tdReportDetail");

      return tr;
    }

    private Control CreateOfficeNameAndKey(PoliticiansAdminReportViewRow row)
    {
      var officeClass = row.OfficeLevel.ToOfficeClass();
      var countyName = row.County.SafeString();
      Control control;
      string literalText;

      if (officeClass.IsCounty())
        literalText = FormatOfficeName(row) + ", " + countyName;
      else if (officeClass.IsLocal())
      {
        countyName = CountyCache.GetCountyName(_Options.StateCode, _Options.CountyCode);
        var localName =
          VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(_Options.StateCode, _Options.LocalKey);
        literalText = FormatOfficeName(row) + ", " + localName + ", " + countyName;
      }
      else
        literalText = FormatOfficeName(row);

      if (officeClass != OfficeClass.USPresident)
        control = CreateAdminOfficeAnchor(row.OfficeKey, literalText, Empty);
      else
        control = new Literal {Text = literalText};

      return control;
    }

    private HtmlTableRow CreateOneOutputRow(PoliticiansAdminReportViewRow row)
    {
      var tr = new HtmlTableRow().AddCssClasses("trReportDetail");

      // Left Column - Politician Name
      var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      CreatePoliticianNameAndKeyAnchor(row)
        .AddTo(td);

      // Right Column - Office Title
      td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
      CreateOfficeNameAndKey(row)
        .AddTo(td);

      return tr;
    }

    private PlaceHolder CreatePoliticianNameAndKeyAnchor(
      PoliticiansAdminReportViewRow row)
    {
      var container = new PlaceHolder();

      var literalText = row.LastName + " .. ";
      if (row.OfficeStatus.ToPoliticianStatus() == PoliticianStatus.Incumbent)
        literalText += "*";
      new Literal {Text = literalText}.AddTo(container);

      Control imageTag;
      if (_AsMaster)
        imageTag =
          CreatePoliticianImageAnchor(
            SecurePoliticianPage.GetUpdateIntroPageUrl(row.PoliticianKey),
            row.PoliticianKey, imageWidth: 15,
            title: FormatPoliticianName(row) + " Intro Page Data Entry",
            target: "intro");
      else
        imageTag = CreatePoliticianImageTag(row.PoliticianKey, 15);
      imageTag.AddTo(container);
      new Literal {Text = "&nbsp"}.AddTo(container);

      CreateAdminPoliticianAnchor(row, FormatPoliticianName(row, true, true),
          Empty)
        .AddTo(container);

      literalText = Empty;
      if (row.PartyKey != Empty)
        literalText += " (" + row.PartyCode.SafeString() + ")";
      literalText += " .. " + row.PoliticianKey;
      new Literal {Text = literalText}.AddTo(container);

      return container;
    }

    private static string FormatPoliticianName(PoliticiansAdminReportViewRow row,
      bool breakAfterPosition = false, bool includeAddOn = false)
    {
      const int maxNameLineLength = 30;

      return Politicians.FormatName(row.FirstName, row.MiddleName, row.Nickname,
        row.LastName, row.Suffix, includeAddOn ? row.AddOn : null, row.StateCode,
        breakAfterPosition ? maxNameLineLength : 0);
    }

    private static string FormatOfficeName(PoliticiansAdminReportViewRow row)
    {
      return Offices.FormatOfficeName(row.OfficeLine1, row.OfficeLine2,
        row.OfficeKey);
    }

    private void GenerateReport(HtmlTable htmlTable,
      ICollection<PoliticiansAdminReportViewRow> rows)
    {
      if (rows.Count > 0)
        foreach (var row in rows)
          CreateOneOutputRow(row)
            .AddTo(htmlTable);
      else
        CreateNoPoliticiansRow()
          .AddTo(htmlTable);
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public Control GenerateReport(PoliticiansAdminReportOptions options)
    {
      _AsMaster = options.AsMaster ?? SecurePage.IsMasterUser;
      _Options = options;

      _DataManager.GetData(options);

      var htmlTable = new HtmlTable();
      htmlTable.AddCssClasses("tableAdmin");

      CreateHeadingRow()
        .AddTo(htmlTable);

      ReportDataManager<PoliticiansAdminReportViewRow>.OrderBy sort;
      if (options.SortByOffice) sort = new SortByOffice();
      else sort = new SortByName();

      GenerateReport(htmlTable, _DataManager.GetDataSubset(null, sort));

      return htmlTable;
    }

    public static Control GetReport(PoliticiansAdminReportOptions options)
    {
      var reportObject = new PoliticiansAdminReport();
      return reportObject.GenerateReport(options);
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}