//using System.Data;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal sealed class BallotReferendumReport : Report
//  {
//    #region Private

//    private readonly BallotReferendumReportDataManager _DataManager =
//      new BallotReferendumReportDataManager();

//    private HtmlTable _MainHtmlTable;
//    private string _ElectionKey;
//    private string _StateCode;
//    private string _CountyCode;

//    private sealed class StateFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return string.IsNullOrWhiteSpace(row.CountyCode());
//      }
//    }

//    private sealed class CountyFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return !string.IsNullOrWhiteSpace(row.CountyCode()) &&
//          string.IsNullOrWhiteSpace(row.LocalCode());
//      }
//    }

//    private sealed class LocalFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return !string.IsNullOrWhiteSpace(row.LocalCode());
//      }
//    }

//    private sealed class OneLocalFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      private readonly string _LocalCode;

//      public OneLocalFilter(string localCode)
//      {
//        _LocalCode = localCode;
//      }

//      public override bool Filter(DataRow row)
//      {
//        return row.LocalCode() == _LocalCode;
//      }
//    }

//    private sealed class RererendumOrderBy : ReportDataManager<DataRow>.OrderBy
//    {
//      public override int Compare(DataRow row1, DataRow row2)
//      {
//        return row1.OrderOnBallot()
//          .CompareTo(row2.OrderOnBallot());
//      }
//    }

//    private sealed class BallotReferendumReportDataManager :
//      ReportDataManager<DataRow>
//    {
//      public void GetData(string electionKey, string countyCode)
//      {
//        DataTable = Referendums.GetBallotReportData(electionKey, countyCode);
//      }
//    }

//    private Control GenerateReport(string electionKey, string countyCode)
//    {
//      _ElectionKey = electionKey;
//      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);
//      _CountyCode = countyCode;

//      _DataManager.GetData(_ElectionKey, _CountyCode);

//      _MainHtmlTable =
//        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");

//      if (Elections.IsStateElection(_ElectionKey))
//        ReportStateReferendums();

//      if (Elections.IsStateElection(_ElectionKey) ||
//        Elections.IsCountyElection(_ElectionKey))
//        ReportCountyReferendums();

//      if (Elections.IsLocalElection(_ElectionKey))
//        ReportLocalReferendumsForOneLocal();
//      else
//        ReportLocalReferendumsForAllLocals();

//      return _MainHtmlTable;
//    }

//    private void ReportStateReferendums()
//    {
//      var referendums = _DataManager.GetDataSubset(new StateFilter(),
//        new RererendumOrderBy());
//      if (referendums.Count == 0) return;

//      var stateName = StateCache.GetStateName(_StateCode) + " Ballot Measures";
//      CreateJurisdictionHeading(stateName);

//      foreach (var referendum in referendums)
//        ReportOneReferendum(referendum);
//    }

//    private void ReportCountyReferendums()
//    {
//      var referendums = _DataManager.GetDataSubset(new CountyFilter(),
//        new RererendumOrderBy());
//      if (referendums.Count == 0) return;

//      var countyName = CountyCache.GetCountyName(_StateCode, _CountyCode) +
//        " Ballot Measures";
//      CreateJurisdictionHeading(countyName);

//      foreach (var referendum in referendums)
//        ReportOneReferendum(referendum);
//    }

//    private void ReportLocalReferendumsForOneLocal()
//    {
//      var localCode = Elections.GetLocalCodeFromKey(_ElectionKey);
//      var referendums = _DataManager.GetDataSubset(new OneLocalFilter(localCode),
//        new RererendumOrderBy());
//      if (referendums.Count == 0) return;

//      var localName = referendums[0].LocalDistrict();
//      CreateJurisdictionHeading(localName);

//      foreach (var referendum in referendums)
//        ReportOneReferendum(referendum);
//    }

//    private void ReportLocalReferendumsForAllLocals()
//    {
//      var locals =
//        _DataManager.GetDataSubset(new LocalFilter(), new RererendumOrderBy())
//          .GroupBy(r => r.LocalCode())
//          .OrderBy(g => g.First()
//            .LocalDistrict())
//          .ToList();

//      foreach (var local in locals)
//      {
//        CreateJurisdictionHeading(local.First()
//          .LocalDistrict());
//        foreach (var referendum in local)
//          ReportOneReferendum(referendum);
//      }
//    }

//    private void CreateJurisdictionHeading(string heading)
//    {
//      var tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trBallotSeparator");
//      new HtmlTableCell {ColSpan = 2, InnerHtml = heading}.AddTo(tr,
//        "tdBallotSeparator");
//    }

//    private void ReportOneReferendum(DataRow referendum)
//    {
//      var tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trBallotReferendumHeading");
//      var td = new HtmlTableCell {ColSpan = 2}.AddTo(tr,
//        "tdBallotReferendumHeading");
//      CreateReferendumAnchor(referendum)
//        .AddTo(td);

//      tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trBallotReferendum");
//      var referendumDesc = string.IsNullOrWhiteSpace(referendum.ReferendumDescription())
//        ? referendum.ReferendumTitle()
//        : referendum.ReferendumDescription();
//      referendumDesc = referendumDesc.ReplaceNewLinesWithBreakTags();
//      new HtmlTableCell {InnerHtml = referendumDesc}.AddTo(tr, "tdBallotReferendum");

//      td = new HtmlTableCell {Align = "right"}.AddTo(tr, "tdBallotReferendumYN");
//      var checkboxTable = new HtmlTable {CellSpacing = 0}.AddTo(td,
//        "tableBallotReferendumYN");

//      tr = new HtmlTableRow().AddTo(checkboxTable, "trBallotReferendumYN");
//      new HtmlTableCell {InnerHtml = "Yes"}.AddTo(tr, "tdBallotReferendumYN");
//      td = new HtmlTableCell {Align = "right"}.AddTo(tr,
//        "tdBallotReferendumCheckBox");
//      new HtmlInputCheckBox().AddTo(td);

//      tr = new HtmlTableRow().AddTo(checkboxTable, "trBallotReferendumYN");
//      new HtmlTableCell {InnerHtml = "No"}.AddTo(tr, "tdBallotReferendumYN");
//      td = new HtmlTableCell {Align = "right"}.AddTo(tr,
//        "tdBallotReferendumCheckBox");
//      new HtmlInputCheckBox().AddTo(td);
//    }

//    private static HtmlAnchor CreateReferendumAnchor(DataRow referendum)
//    {
//      var a = new HtmlAnchor
//        {
//          HRef =
//            UrlManager.GetReferendumPageUri(referendum.ElectionKey(),
//              referendum.ReferendumKey())
//              .ToString(),
//          Title = "Referendum Description, Details and Full Text",
//          InnerHtml = referendum.ReferendumTitle()
//            .ReplaceNewLinesWithBreakTags()
//        };
//      return a;
//    }

//    #endregion Private

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    public static Control GetReport(string electionKey, string countyCode)
//    {
//      var reportObject = new BallotReferendumReport();
//      return reportObject.GenerateReport(electionKey, countyCode);
//    }

//    #region ReSharper restore

//    // ReSharper restore UnassignedField.Global
//    // ReSharper restore UnusedAutoPropertyAccessor.Global
//    // ReSharper restore UnusedMethodReturnValue.Global
//    // ReSharper restore UnusedMember.Global
//    // ReSharper restore MemberCanBeProtected.Global
//    // ReSharper restore MemberCanBePrivate.Global

//    #endregion ReSharper restore

//    #endregion Public
//  }
//}