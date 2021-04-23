using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using VoteLibrary.Utility;
using static System.String;

namespace Vote.Reports
{
  public sealed class BallotReferendumReportResponsive : ResponsiveReport
  {
    #region Private

    private readonly BallotReferendumReportDataManager _DataManager =
      new BallotReferendumReportDataManager();

    private string _ElectionKey;
    private string _StateCode;
    private string _CountyCode;

    private sealed class StateFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return IsNullOrWhiteSpace(row.CountyCode()) && IsNullOrWhiteSpace(row.LocalKey());
      }
    }

    private sealed class CountyFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !IsNullOrWhiteSpace(row.CountyCode());
      }
    }

    private sealed class LocalFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !IsNullOrWhiteSpace(row.LocalKey());
      }
    }

    private sealed class OneLocalFilter : ReportDataManager<DataRow>.FilterBy
    {
      private readonly string _LocalKey;

      public OneLocalFilter(string localKey)
      {
        _LocalKey = localKey;
      }

      public override bool Filter(DataRow row)
      {
        return row.LocalKey() == _LocalKey;
      }
    }

    private sealed class RererendumOrderBy : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        return row1.OrderOnBallot()
          .CompareTo(row2.OrderOnBallot());
      }
    }

    private sealed class BallotReferendumReportDataManager :
      ReportDataManager<DataRow>
    {
      public void GetData(string electionKey, string countyCode, string district,
       string place, string elementary, string secondary, string unified, string cityCouncil,
       string countySupervisors, string schoolDistrictDistrict)
      {
        DataTable = Referendums.GetBallotReportData(electionKey, countyCode, district,
         place, elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict);
      }
    }

    private Control GenerateReport(string electionKey, string countyCode, string district, 
      string place, string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict)
    {
      _ElectionKey = electionKey;
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);
      _CountyCode = countyCode;

      _DataManager.GetData(electionKey, countyCode, district, place, elementary, secondary, 
        unified, cityCouncil, countySupervisors, schoolDistrictDistrict);

      if (Elections.IsStateElection(_ElectionKey))
        ReportStateReferendums();

      if (Elections.IsStateElection(_ElectionKey) ||
        Elections.IsCountyElection(_ElectionKey))
        ReportCountyReferendums();

      if (Elections.IsLocalElection(_ElectionKey))
        ReportLocalReferendumsForOneLocal();
      else
        ReportLocalReferendumsForAllLocals();

      return ReportContainer.AddCssClasses("ballot-referendum-report ballot-checks-container");
    }

    private void ReportStateReferendums()
    {
      var referendums = _DataManager.GetDataSubset(new StateFilter(),
        new RererendumOrderBy());
      if (referendums.Count == 0) return;

      var outer = new HtmlDiv().AddTo(ReportContainer, "referendums-list");
      // ReSharper disable once PossibleNullReferenceException
      (new HtmlDiv().AddTo(outer, "accordion-header")
        as HtmlGenericControl).InnerHtml = StateCache.GetStateName(_StateCode) + " Ballot Measures";
      var container = new HtmlDiv().AddTo(outer, "referendums-content accordion-content");

      foreach (var referendum in referendums)
        ReportOneReferendum(container, referendum, true);
    }

    private void ReportCountyReferendums()
    {
      var referendums = _DataManager.GetDataSubset(new CountyFilter(),
        new RererendumOrderBy());
      if (referendums.Count == 0) return;

      var outer = new HtmlDiv().AddTo(ReportContainer, "referendums-list");
      // ReSharper disable once PossibleNullReferenceException
      (new HtmlDiv().AddTo(outer, "accordion-header")
        as HtmlGenericControl).InnerHtml = CountyCache.GetCountyName(_StateCode, _CountyCode) +
        " Ballot Measures";
      var container = new HtmlDiv().AddTo(outer, "referendums-content accordion-content");

      foreach (var referendum in referendums)
        ReportOneReferendum(container, referendum, true);
    }

    private void ReportLocalReferendumsForOneLocal()
    {
      var localKey = Elections.GetLocalKeyFromKey(_ElectionKey);
      var referendums = _DataManager.GetDataSubset(new OneLocalFilter(localKey),
        new RererendumOrderBy());
      if (referendums.Count == 0) return;

      var outer = new HtmlDiv().AddTo(ReportContainer, "referendums-list");
      // ReSharper disable once PossibleNullReferenceException
      (new HtmlDiv().AddTo(outer, "accordion-header")
        as HtmlGenericControl).InnerHtml = referendums[0].LocalDistrict() +
        " Ballot Measures";
      var container = new HtmlDiv().AddTo(outer, "referendums-content accordion-content");

      foreach (var referendum in referendums)
        ReportOneReferendum(container, referendum, true);
    }

    private void ReportLocalReferendumsForAllLocals()
    {
      var comparer = new AlphanumericComparer();
      var locals =
        _DataManager.GetDataSubset(new LocalFilter(), new RererendumOrderBy())
          .GroupBy(r => r.LocalKey())
          .OrderBy(g => g.First().LocalDistrict(), comparer)
          .ToList();

      if (locals.Count == 0) return;

      var districtsOuter = new HtmlDiv().AddTo(ReportContainer, "districts-list print-current-state");
      // ReSharper disable once PossibleNullReferenceException
      (new HtmlDiv().AddTo(districtsOuter, "accordion-header")
        as HtmlGenericControl).InnerHtml = "Local District Ballot Measures";
      var districtsContainer = new HtmlDiv().AddTo(districtsOuter, "local-referendums-content accordion-content");

      foreach (var local in locals)
      {
        var outer = new HtmlDiv().AddTo(districtsContainer, "referendums-list print-current-state no-print-closed");
        // ReSharper disable once PossibleNullReferenceException
        (new HtmlDiv().AddTo(outer, "accordion-header")
          as HtmlGenericControl).InnerHtml = local.First().LocalDistrict();
        var container = new HtmlDiv().AddTo(outer, "referendums-content accordion-content");
        //CreateJurisdictionHeading(local.First()
          //.LocalDistrict());
        foreach (var referendum in local)
          ReportOneReferendum(container, referendum, true);
      }
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetReport(string electionKey, string countyCode, string district,
      string place, string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict)
    {
      var reportObject = new BallotReferendumReportResponsive();
      return reportObject.GenerateReport(electionKey, countyCode, district, place,
        elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict);
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