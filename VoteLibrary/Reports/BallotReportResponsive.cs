using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public sealed class BallotReportResponsive : ResponsiveReport
  {
    #region Private

    private readonly BallotReportDataManager _DataManager =
      new BallotReportDataManager();

    //private int _TotalContests;
    private readonly List<string> _AllOffices = new List<string>();

    private string _ElectionKey;
    private string _StateElectionKey;
    private string _StateCode;
    private string _CountyCode;

    private sealed class StateFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return IsNullOrWhiteSpace(row.CountyCode()) && IsNullOrWhiteSpace(row.LocalKey()) && !row.IsRunningMate();
      }
    }

    private sealed class CountyFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !IsNullOrWhiteSpace(row.CountyCode()) && !row.IsRunningMate();
      }
    }

    private sealed class LocalFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !IsNullOrWhiteSpace(row.LocalKey()) && !row.IsRunningMate();
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
        return row.LocalKey() == _LocalKey && !row.IsRunningMate();
      }
    }

    private class BallotOrderBy : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.OfficeOrder()
          .CompareTo(row2.OfficeOrder());
        if (result != 0) return result;
        result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeKey(), row2.OfficeKey(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = row1.OrderOnBallot()
          .CompareTo(row2.OrderOnBallot());
        if (result != 0) return result;
        result = string.Compare(row1.LastName(), row2.LastName(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.FirstName(), row2.FirstName(),
          StringComparison.OrdinalIgnoreCase);
      }
    }

    private sealed class LocalOrderBy : BallotOrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        //var result = string.Compare(row1.LocalDistrict(), row2.LocalDistrict(),
        //  StringComparison.OrdinalIgnoreCase);
        var result = row1.LocalDistrict().CompareAlphanumeric(row2.LocalDistrict());
        if (result != 0) return result;
        return base.Compare(row1, row2);
      }
    }

    private sealed class BallotReportDataManager : ReportDataManager<DataRow>
    {
      //private string _StateCode;
      private string ElectionKey { get; set; }

      //public void GetData(string electionKey, string congress, string stateSenate,
      //  string stateHouse, string countyCode)
      //{
      //  _StateCode = Elections.GetStateCodeFromKey(electionKey);
      //  ElectionKey = electionKey;
      //  DataTable = ElectionsPoliticians.GetSampleBallotData(electionKey, congress,
      //    stateSenate, stateHouse, countyCode);
      //}

      public void GetData(string electionKey, string congress, string stateSenate,
        string stateHouse, string countyCode, string district, string place, string elementary, 
        string secondary, string unified, string cityCouncil, string countySupervisors, 
        string schoolDistrictDistrict)
      {
        //_StateCode = Elections.GetStateCodeFromKey(electionKey);
        ElectionKey = electionKey;
        DataTable = ElectionsPoliticians.GetSampleBallotData(electionKey, congress, stateSenate, 
          stateHouse, countyCode, district, place, elementary, secondary, unified, cityCouncil, 
          countySupervisors, schoolDistrictDistrict);
      }

      public List<IGrouping<string, DataRow>> GetOfficeGroups(
        FilterBy filterBy = null, OrderBy orderBy = null)
      {
        var offices = GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
          .GroupBy(row => row.OfficeKey());
        if (Elections.IsPrimaryElection(ElectionKey)) offices = FilterUncontestedOffices(offices);
        return offices.ToList();
      }

      public List<IGrouping<string, DataRow>> GetLocalGroups(FilterBy filterBy = null,
        OrderBy orderBy = null)
      {
        return
          GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
            .GroupBy(row => row.LocalKey())
            .ToList();
      }

      public DataRow GetRunningMate(string officeKey, string politicianKey)
      {
        return DataList.FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
              .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }
    }

    private Control GenerateReport(string electionKey, string congress, string stateSenate, 
      string stateHouse, string countyCode, string district, string place, 
      string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict, out List<string> totalContests)
    {
      _ElectionKey = electionKey;
      _StateElectionKey = Elections.GetStateElectionKeyFromKey(_ElectionKey);
      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);
      _CountyCode = countyCode;

      _DataManager.GetData(electionKey, congress, stateSenate, stateHouse, countyCode, district, 
        place, elementary, secondary, unified, cityCouncil, countySupervisors, 
        schoolDistrictDistrict);

      if (Elections.IsStateElection(_ElectionKey))
        ReportStateContests();

      if (Elections.IsStateElection(_ElectionKey) ||
        Elections.IsCountyElection(_ElectionKey))
        ReportCountyContests();

      if (Elections.IsLocalElection(_ElectionKey))
        ReportLocalContestsForOneLocal();
      else
        ReportLocalContestsForAllLocals();

      //totalContests = _TotalContests;
      totalContests = _AllOffices;
      return ReportContainer.AddCssClasses("ballot-report ballot-checks-container");

    }

    private void ReportStateContests()
    {
      var offices = _DataManager.GetOfficeGroups(new StateFilter());
      if (offices.Count == 0) return;
      var officesDiv = new HtmlDiv().AddTo(ReportContainer, "office-cells accordion-content");
      foreach (var office in offices)
      {
        if (ReportOneOffice(_StateElectionKey, office, officesDiv))
          //_TotalContests++;
          _AllOffices.Add(office.First().OfficeKey());
      }
    }

    private void ReportCountyContests()
    {
      var offices = _DataManager.GetOfficeGroups(new CountyFilter(/*_StateCode*/));
      if (offices.Count == 0) return;

      var container = new HtmlDiv().AddTo(ReportContainer, 
        "county-contests no-accordion");

      var countyKey = Elections.GetCountyElectionKeyFromKey(_ElectionKey,
        _StateCode, _CountyCode);
      var countyName = CountyCache.GetCountyName(_StateCode, _CountyCode);
      new HtmlDiv { InnerText = countyName }.AddTo(container, "offices-heading accordion-header");

      var officesDiv = new HtmlDiv().AddTo(container, "office-cells accordion-content");
      foreach (var office in offices)
      {
        if (ReportOneOffice(countyKey, office, officesDiv))
          //_TotalContests++;
          _AllOffices.Add(office.First().OfficeKey());
      }
    }

    private void ReportLocalContestsForOneLocal()
    {
      var localKey = Elections.GetLocalKeyFromKey(_ElectionKey);
      var offices = _DataManager.GetOfficeGroups(new OneLocalFilter(localKey));
      if (offices.Count == 0) return;

      var container = new HtmlDiv().AddTo(ReportContainer, "one-local-contests no-accordion");

      var localInfo = offices.First()
        .First();
      var localDistrict = localInfo.LocalDistrict();
      new HtmlDiv { InnerText = localDistrict }.AddTo(container, "offices-heading accordion-header");

      var officesDiv = new HtmlDiv().AddTo(container, "office-cells accordion-content");
      foreach (var office in offices)
      {
        if (ReportOneOffice(_ElectionKey, office, officesDiv))
          //_TotalContests++;
          _AllOffices.Add(office.First().OfficeKey());
      }
    }

    private void ReportLocalContestsForAllLocals()
    {
      var locals = _DataManager.GetLocalGroups(new LocalFilter(/*_StateCode*/), new LocalOrderBy());
      if (locals.Count == 0) return;

      Control districtsContent;
      if (locals.Count == 1)
        districtsContent = new HtmlDiv().AddTo(ReportContainer, "local-contests no-accordion");
      else
      {
        var districtsContainer = new HtmlDiv().AddTo(ReportContainer,
          "local-districts-header print-current-state");
        new HtmlDiv {InnerText = "Local District Elections"}.AddTo(districtsContainer,
          "accordion-header");
        districtsContent = new HtmlDiv().AddTo(districtsContainer,
          "accordion-content local-districts-content print-current-state no-print-closed");
      }

      foreach (var local in locals)
      {
        var localInfo = local.First();
        var localKey = localInfo.LocalKey();
        var localDistrict = localInfo.LocalDistrict();
        var localElectionKey = Elections.GetLocalElectionKeyFromKey(_ElectionKey,
          localKey);
        new HtmlDiv {InnerText = localDistrict}.AddTo(districtsContent,
          "offices-heading accordion-header");
        var offices = local.GroupBy(row => row.OfficeKey());
        var officesDiv = new HtmlDiv().AddTo(districtsContent, "office-cells accordion-content");
        foreach (var office in offices)
        {
          if (ReportOneOffice(localElectionKey, office, officesDiv))
            //_TotalContests++;
            _AllOffices.Add(office.First().OfficeKey());
        }
      }
    }

    private bool ReportOneOffice(string electionKey, IEnumerable<DataRow> office, Control container)
    {
      var candidates = office.ToList();
      var officeInfo = candidates.First();
      var officeKey = officeInfo.OfficeKey();
      var trueElectionKey = officeInfo.ElectionKey();

      // Mantis 349: Use correct election positions
      var positions = GetElectionPositions(electionKey, officeInfo);

      if (!StateCache.GetShowUnopposed(_StateCode))
      {
        var unopposed = candidates.Count <= positions;
        if (unopposed) return false;
      }

      var content = new HtmlDiv().AddTo(container, "office-cell");
      content.Attributes.Add("data-key", officeKey.ToUpperInvariant());
      var trueStateElectionKey = Elections.GetStateElectionKeyFromKey(trueElectionKey);
      if (trueStateElectionKey.IsNeIgnoreCase(Elections.GetStateElectionKeyFromKey(electionKey)))
        content.Attributes.Add("data-election", trueStateElectionKey.ToUpperInvariant());
      if (positions > 1)
        content.Attributes.Add("data-positions", positions.ToString(CultureInfo.InvariantCulture));
      content = new HtmlDiv().AddTo(content, "office-cell-inner");

      var officeHeadingDiv = new HtmlDiv().AddTo(content, "office-heading");
      var formattedOfficeName = FormatOfficeNameForBallot(officeInfo);
      new HtmlDiv { InnerHtml = formattedOfficeName }.AddTo(officeHeadingDiv, "office-heading-name");

      // Mantis 349: Use correct election positions
      var voteForWording = Offices.GetVoteForNoMoreThanWording(positions);
      new HtmlDiv { InnerText = voteForWording }.AddTo(officeHeadingDiv, "office-heading-vote-for");

      CreateCompareOrIntroAnchor(officeHeadingDiv, candidates, trueElectionKey, officeKey);

      foreach (var candidate in candidates)
        ReportOneCandidate(content, candidate);

      if (StateCache.GetShowWriteIn(_StateCode))
      {
        const string writeInWording = "Write in";
        if (officeInfo.WriteInLines() > 0)
        {
          var writeInCell = new HtmlDiv().AddTo(content, "candidate-cell write-in-cell");
          writeInCell = new HtmlDiv().AddTo(writeInCell, "candidate-cell-inner");
          new HtmlInputCheckBox().AddTo(writeInCell, "kalypto candidate-checkbox");
          new HtmlSpan { InnerText = writeInWording }.AddTo(writeInCell, "write-in-wording clicker");
          new HtmlInputText().AddTo(writeInCell, "write-in-text").Attributes["placeholder"] = "Enter candidate name";
        }
      }

      return true;
    }

    private static string FormatOfficeNameForBallot(DataRow officeInfo)
    {
      var officeKey = officeInfo.OfficeKey();
      var officeName = Offices.FormatOfficeName(officeInfo, "</span><span>");
      if (Offices.IsCountyOffice(officeKey) || Offices.IsLocalOffice(officeKey))
        officeName += "</span><span>" +
          CountyCache.GetCountyName(Offices.GetStateCodeFromKey(officeKey),
            Offices.GetCountyCodeFromKey(officeKey));
      if (Offices.IsLocalOffice(officeKey))
        officeName += "</span><span>" + officeInfo.LocalDistrict();
      return "<span>" + officeName + "</span>";
    }

    private void ReportOneCandidate(Control container, DataRow politician)
    {
      //var isRunningMateOffice = politician.IsRunningMateOffice() &&
      //  !Elections.IsPrimaryElection(_ElectionKey);
      var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(_ElectionKey,
          politician.OfficeKey());

      var content = new HtmlDiv().AddTo(container, "candidate-cell");
      content.Attributes.Add("data-key", politician.PoliticianKey().ToUpperInvariant());
      content = new HtmlDiv().AddTo(content, "candidate-cell-inner");

      var politicianKey = politician.PoliticianKey();

      new HtmlInputCheckBox().AddTo(content, "kalypto candidate-checkbox");

      var nameContainer = new HtmlDiv().AddTo(content, "candidate-name");
      // Mantis 349: Only show major parties
      var showParty = !Elections.IsPrimaryElection(politician.ElectionKey())
        && politician.IsPartyMajor();
      // Mantis 349: Show incumbent always
      //var showIncumbent = StateCache.GetIsIncumbentShownOnBallots(_StateCode) &&
      // politician.IsIncumbent();
      FormatCandidateNameAndParty(nameContainer, politician, politician.IsIncumbent(), showParty,
        false, false, "clicker");

      var imageContainer = new HtmlDiv().AddTo(content, "candidate-image");
      CreatePoliticianImageTag(politicianKey, ImageSize100, false, Empty).AddTo(imageContainer);

      DataRow runningMate = null;
      if (isRunningMateOffice)
      {
        var runningMateKey = politician.RunningMateKey();
        runningMate = _DataManager.GetRunningMate(politician.OfficeKey(),
          runningMateKey);
        if (runningMate != null)
        {
          CreatePoliticianImageTag(runningMateKey, ImageSize100, false, Empty).AddTo(imageContainer, "running-mate-image");
        }
      }

      var infoContainer = new HtmlDiv().AddTo(content, "candidate-info");

      if (isRunningMateOffice && runningMate != null)
        FormatCandidateNameAndParty(nameContainer, runningMate, false, false, true, false, "clicker");

      FormatWebAddress(infoContainer, politician);
      FormatSocialMedia(infoContainer, politician);
      FormatAge(infoContainer, politician);
      FormatMoreInfoLink(infoContainer, politician);
    }

    #endregion Private

    public static Control GetReport(string electionKey, string congress, string stateSenate, 
      string stateHouse, string countyCode, string district, string place, 
      string elementary, string secondary, string unified, string cityCouncil, 
      string countySupervisors, string schoolDistrictDistrict, out List<string> officeContests)
    {
      var reportObject = new BallotReportResponsive();
      return reportObject.GenerateReport(electionKey, congress, stateSenate,
        stateHouse, countyCode, district, place, elementary, secondary, unified, 
        cityCouncil, countySupervisors, schoolDistrictDistrict, out officeContests);
    }
  }
}