using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  internal sealed class OfficesAdminReportDataManager :
    ReportDataManager<OfficesAdminReportViewRow>
  {
    public void GetData(OfficesAdminReportViewOptions options)
    {
      DataTable = OfficesAdminReportView.GetData(options);
    }
  }

  public sealed class OfficesAdminReport : Report
  {
    #region Private

    private OfficesAdminReportViewOptions _Options;

    #region DataManager

    private readonly OfficesAdminReportDataManager _DataManager =
      new OfficesAdminReportDataManager();

    private class Sort : ReportDataManager<OfficesAdminReportViewRow>.OrderBy
    {
      public override int Compare(OfficesAdminReportViewRow row1,
        OfficesAdminReportViewRow row2)
      {
        return CompareOfficesAdminReportViewRows(row1, row2);
      }
    }

    private class FilterByOffice :
      ReportDataManager<OfficesAdminReportViewRow>.FilterBy
    {
      private readonly OfficeClass _OfficeClass;

      public FilterByOffice(OfficeClass officeClass)
      {
        _OfficeClass = officeClass;
      }

      public override bool Filter(OfficesAdminReportViewRow row)
      {
        return row.OfficeLevel == _OfficeClass.ToInt();
      }
    }

    #endregion DataManager

    private static HtmlTableRow CreateNoOfficesRow()
    {
      var tr = new HtmlTableRow().AddCssClasses("trReportGroupHeadingLeft");

      new HtmlTableCell
      {
        InnerHtml = "No Offices in this office category",
        ColSpan = 2
      }.AddTo(tr, "tdReportGroupHeadingLeft");

      return tr;
    }

    private static HtmlTableRow CreateOfficeColumnLabels()
    {
      var tr = new HtmlTableRow().AddCssClasses("trReportGroupHeading");

      new HtmlTableCell
      {
        InnerHtml = "Office Title on Ballots<BR>(Office ID)",
        Align = "left",
        Width = "40%"
      }.AddTo(tr, "tdReportDetailHeadingLeft");

      new HtmlTableCell
      {
        InnerHtml =
          "Order | Positions | Vote Instructions" +
          "<br>Write In Lines | Write In Instructions | Write In Wording"
      }.AddTo(
        tr, "tdReportDetailHeadingLeft");

      return tr;
    }

    private static HtmlTableRow CreateOfficeHeading(OfficeClass officeClass,
      string stateCode, string countyCode = "", string localKey = "")
    {
      var contestName =
        Offices.GetLocalizedOfficeClassDescription(officeClass, stateCode,
          countyCode, localKey) + " Offices";

      //Control officeHeading;
      var
      //if (!_DataManager.IsOfficeClassClosed(officeClass))
        officeHeading = CreateOfficePageAnchor(stateCode, countyCode, localKey, officeClass,
          "Add " + contestName);
      //else officeHeading = new Literal {Text = contestName};

      var tr = new HtmlTableRow().AddCssClasses("trReportGroupHeadingLeft");
      var td = new HtmlTableCell {ColSpan = 2}.AddTo(tr, "tdReportGroupHeadingLeft");
      officeHeading.AddTo(td);

      return tr;
    }

    private static HtmlAnchor CreateOfficePageAnchor(string stateCode, string countyCode,
      string localKey, OfficeClass officeClass, string text)
    {
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetUpdateOfficesPageUrl(stateCode, countyCode, localKey, officeClass),
        Target = "office",
        InnerHtml = text
      };

      return a;
    }

    private static HtmlAnchor CreateOfficePageAnchor(string officekey, string text)
    {
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetOfficePageEditUrl(officekey),
        Target = "office",
        InnerHtml = text
      };

      return a;
    }

    private Control CreateOfficeTitleAndKeyAnchor(
      OfficesAdminReportViewRow row)
    {
      var officeClass = row.OfficeLevel.ToOfficeClass();

      var container = new PlaceHolder();
      var text = Offices.FormatOfficeName(row.OfficeLine1, row.OfficeLine2,
        row.OfficeKey);

      if (officeClass.IsLocal())
        text += ", " +
          VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(_Options.StateCode, _Options.LocalKey);

      if (officeClass.IsCounty() || officeClass.IsLocal())
        text += ", " + CountyCache.GetCountyName(_Options.StateCode, _Options.CountyCode);

      CreateOfficePageAnchor(row.OfficeKey, text)
        .AddTo(container);

      if (row.IsInactive)
        new Literal {Text = " - INACTIVE"}.AddTo(container);

      new Literal {Text = "<br/>(" + row.OfficeKey + ")"}.AddTo(container);

      return container;
    }

    private static string FormatOfficeData(OfficesAdminReportViewRow row)
    {
      var officeInfo = Empty;

      officeInfo += row.OfficeOrderWithinLevel;
      officeInfo += " | " + row.Incumbents;

      if (row.VoteInstructions != Empty)
        officeInfo += " | " + row.VoteInstructions;
      else
        officeInfo += " | none";

      officeInfo += "<br/>";
      officeInfo += row.WriteInLines;

      if (row.WriteInInstructions != Empty)
        officeInfo += " | " + row.WriteInInstructions;
      else
        officeInfo += " | none";

      if (row.WriteInWording != Empty)
        officeInfo += " | " + row.WriteInWording;
      else
        officeInfo += " | none";

      return officeInfo;
    }

    private void ReportAllOffices(
      ICollection<OfficesAdminReportViewRow> officesTable, HtmlTable htmlTable,
      OfficeClass officeClass, string stateCode, string countyCode = "",
      string localKey = "")
    {
      CreateOfficeHeading(officeClass, stateCode, countyCode, localKey)
        .AddTo(htmlTable);
      CreateOfficeColumnLabels()
        .AddTo(htmlTable);
      ReportEachOffice(htmlTable, officesTable);
    }

    private void ReportEachOffice(HtmlTable htmlTable,
      ICollection<OfficesAdminReportViewRow> table)
    {
      if (table.Count > 0)
        foreach (var row in table)
        {
          var tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
          var td = new HtmlTableCell().AddTo(tr, "tdReportDetail");
          CreateOfficeTitleAndKeyAnchor(row)
            .AddTo(td);
          new HtmlTableCell {InnerHtml = FormatOfficeData(row)}.AddTo(tr,
            "tdReportDetail");
        }
      else
      {
        var tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
        new HtmlTableCell {InnerHtml = "--- No Offices ---"}.AddTo(tr,
          "tdReportDetail");
        new HtmlTableCell {InnerHtml = "--- No Offices ---"}.AddTo(tr,
          "tdReportDetail");
      }
    }

    private void ReportOneOfficeClass(HtmlTable htmlTable, OfficeClass officeClass,
      string stateCode, string countyCode = "", string localKey = "")
    {
      var data = _DataManager.GetDataSubset(new FilterByOffice(officeClass),
        new Sort());

      ReportAllOffices(data, htmlTable, officeClass, stateCode, countyCode,
        localKey);
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static int CompareOfficesAdminReportViewRows(OfficesAdminReportViewRow row1,
      OfficesAdminReportViewRow row2)
    {
      var result =
        row1.OfficeOrderWithinLevel.CompareTo(row2.OfficeOrderWithinLevel);
      if (result != 0) return result;
      result = Compare(row1.DistrictCode, row2.DistrictCode,
        StringComparison.OrdinalIgnoreCase);
      if (result != 0) return result;
      return Compare(row1.OfficeLine1, row2.OfficeLine1,
        StringComparison.OrdinalIgnoreCase);
    }

    public Control GenerateReport(OfficesAdminReportViewOptions options)
    {
      _DataManager.GetData(options);
      _Options = options;

      var htmlTable = new HtmlTable().AddCssClasses("tableAdmin");

      if (options.OfficeClass == OfficeClass.All)
        switch (
          Offices.GetElectoralClass(options.StateCode, options.CountyCode,
            options.LocalKey))
        {
          case ElectoralClass.State:
            foreach (var oc in
              Offices.GetStateOfficeClasses(GetOfficeClassesOptions.IncludeCongress)
            )
              ReportOneOfficeClass(htmlTable, oc, options.StateCode);
            break;

          case ElectoralClass.County:
            foreach (var oc in Offices.GetCountyOfficeClasses())
              ReportOneOfficeClass(htmlTable, oc, options.StateCode,
                options.CountyCode);
            break;

          case ElectoralClass.Local:
            foreach (var oc in Offices.GetLocalOfficeClasses())
              ReportOneOfficeClass(htmlTable, oc, options.StateCode,
                options.CountyCode, options.LocalKey);
            break;
        }
      else
        ReportOneOfficeClass(htmlTable, options.OfficeClass, options.StateCode,
          options.CountyCode, options.LocalKey);

      if (htmlTable.Rows.Count == 0)
        CreateNoOfficesRow()
          .AddTo(htmlTable);

      return htmlTable;
    }

    public static Control GetReport(OfficesAdminReportViewOptions options)
    {
      var reportObject = new OfficesAdminReport();
      return reportObject.GenerateReport(options);
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}