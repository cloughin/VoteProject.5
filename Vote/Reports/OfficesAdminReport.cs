using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class OfficesAdminReportDataManager :
    ReportDataManager<OfficesAdminReportViewRow>
  {
    private Dictionary<OfficeClass, object> _AllIdentifiedDictionary;

    public void GetData(OfficesAdminReportViewOptions options)
    {
      DataTable = OfficesAdminReportView.GetData(options);

      // Get the OfficesAllIdentified row(s) and make a dictionary of
      // only the true values
      var table = options.OfficeClass == OfficeClass.All
        ? OfficesAllIdentified.GetDataByStateCode(options.StateCode)
        : OfficesAllIdentified.GetData(options.StateCode, options.OfficeClass.ToInt());
      _AllIdentifiedDictionary = table.Where(row => row.IsOfficesAllIdentified)
        .ToDictionary(row => row.OfficeLevel.ToOfficeClass(), row => null as object);
    }

    public bool IsOfficeClassClosed(OfficeClass officeClass)
    {
      return _AllIdentifiedDictionary.ContainsKey(officeClass);
    }
  }

  internal sealed class OfficesAdminReport : Report
  {
    #region Private

    #region DataManager

    private readonly OfficesAdminReportDataManager _DataManager =
      new OfficesAdminReportDataManager();

    private class Sort : ReportDataManager<OfficesAdminReportViewRow>.OrderBy
    {
      public override int Compare(OfficesAdminReportViewRow row1,
        OfficesAdminReportViewRow row2)
      {
        //var result =
        //  row1.OfficeOrderWithinLevel.CompareTo(row2.OfficeOrderWithinLevel);
        //if (result != 0) return result;
        //result = string.Compare(row1.DistrictCode, row2.DistrictCode,
        //  StringComparison.OrdinalIgnoreCase);
        //if (result != 0) return result;
        //return string.Compare(row1.OfficeLine1, row2.OfficeLine1,
        //  StringComparison.OrdinalIgnoreCase);
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
          "Order | Positions | Vote Instructions | No More than Wording" +
          "<br>Write In Lines | Write In Instructions | Write In Wording"
      }.AddTo(
        tr, "tdReportDetailHeadingLeft");

      return tr;
    }

    private HtmlTableRow CreateOfficeHeading(OfficeClass officeClass,
      string stateCode, string countyCode = "", string localCode = "")
    {
      var contestName =
        Offices.GetLocalizedOfficeClassDescription(officeClass, stateCode,
          countyCode, localCode) + " Offices";

      Control officeHeading;
      if (!_DataManager.IsOfficeClassClosed(officeClass))
        officeHeading = CreateOfficePageAnchor(stateCode, countyCode, localCode, officeClass,
          "Add " + contestName);
      else officeHeading = new Literal {Text = contestName};

      var tr = new HtmlTableRow().AddCssClasses("trReportGroupHeadingLeft");
      var td = new HtmlTableCell {ColSpan = 2}.AddTo(tr, "tdReportGroupHeadingLeft");
      officeHeading.AddTo(td);

      return tr;
    }

    private static HtmlAnchor CreateOfficePageAnchor(string stateCode, string countyCode,
      string localCode, OfficeClass officeClass, string text)
    {
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetOfficePageUrl(stateCode, officeClass, countyCode, localCode),
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

    private static Control CreateOfficeTitleAndKeyAnchor(
      OfficesAdminReportViewRow row)
    {
      var officeClass = row.OfficeLevel.ToOfficeClass();

      var container = new PlaceHolder();
      var text = Offices.FormatOfficeName(row.OfficeLine1, row.OfficeLine2,
        row.OfficeKey);

      if (officeClass.IsLocal())
        text += ", " + row.LocalDistrict;

      if (officeClass.IsCounty() || officeClass.IsLocal())
        text += ", " + CountyCache.GetCountyName(row.StateCode, row.CountyCode);

      CreateOfficePageAnchor(row.OfficeKey, text)
        .AddTo(container);

      if (row.IsInactive)
        new Literal {Text = " - INACTIVE"}.AddTo(container);

      new Literal {Text = "<br/>(" + row.OfficeKey + ")"}.AddTo(container);

      return container;
    }

    private static string FormatOfficeData(OfficesAdminReportViewRow row)
    {
      var officeInfo = string.Empty;

      officeInfo += row.OfficeOrderWithinLevel;
      officeInfo += " | " + row.Incumbents;

      if (row.VoteInstructions != string.Empty)
        officeInfo += " | " + row.VoteInstructions;
      else
        officeInfo += " | none";

      if (row.VoteForWording != string.Empty)
        officeInfo += " | " + row.VoteForWording;
      else
        officeInfo += " | none";

      officeInfo += "<br/>";
      officeInfo += row.WriteInLines;

      if (row.WriteInInstructions != string.Empty)
        officeInfo += " | " + row.WriteInInstructions;
      else
        officeInfo += " | none";

      if (row.WriteInWording != string.Empty)
        officeInfo += " | " + row.WriteInWording;
      else
        officeInfo += " | none";

      return officeInfo;
    }

    private void ReportAllOffices(
      ICollection<OfficesAdminReportViewRow> officesTable, HtmlTable htmlTable,
      OfficeClass officeClass, string stateCode, string countyCode = "",
      string localCode = "")
    {
      CreateOfficeHeading(officeClass, stateCode, countyCode, localCode)
        .AddTo(htmlTable);
      CreateOfficeColumnLabels()
        .AddTo(htmlTable);
      ReportEachOffice(htmlTable, officesTable);
    }

    private static void ReportEachOffice(HtmlTable htmlTable,
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
      string stateCode, string countyCode = "", string localCode = "")
    {
      var data = _DataManager.GetDataSubset(new FilterByOffice(officeClass),
        new Sort());
      var isClosed = _DataManager.IsOfficeClassClosed(officeClass);

      if ((data.Count == 0) && isClosed) return;

      ReportAllOffices(data, htmlTable, officeClass, stateCode, countyCode,
        localCode);
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
      result = string.Compare(row1.DistrictCode, row2.DistrictCode,
        StringComparison.OrdinalIgnoreCase);
      if (result != 0) return result;
      return string.Compare(row1.OfficeLine1, row2.OfficeLine1,
        StringComparison.OrdinalIgnoreCase);
    }

    public Control GenerateReport(OfficesAdminReportViewOptions options)
    {
      _DataManager.GetData(options);

      var htmlTable = new HtmlTable().AddCssClasses("tableAdmin");

      if (options.OfficeClass == OfficeClass.All)
        switch (
          Offices.GetElectoralClass(options.StateCode, options.CountyCode,
            options.LocalCode))
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
                options.CountyCode, options.LocalCode);
            break;
        }
      else
        ReportOneOfficeClass(htmlTable, options.OfficeClass, options.StateCode,
          options.CountyCode, options.LocalCode);

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