using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class ElectionReportResponsive : ResponsiveReport
  {
    #region Private

    private readonly ElectionReportDataManager _DataManager =
      new ElectionReportDataManager();

    private bool _OpenAll;
    private bool _IsMostRecentElection;

    #region Private classes

    private sealed class ElectionSort : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.AlternateOfficeLevel()
          .CompareTo(row2.AlternateOfficeLevel());
        if (result != 0) return result;
        result = row1.OfficeLevel()
          .CompareTo(row2.OfficeLevel());
        if (result != 0) return result;
        result = string.Compare(row1.DistrictCode(), row2.DistrictCode(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeLine1(), row2.OfficeLine1(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return row1.OrderOnBallot()
          .CompareTo(row2.OrderOnBallot());
      }
    }

    private sealed class OfficeOrderSort : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeLine1(), row2.OfficeLine1(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return row1.OrderOnBallot()
          .CompareTo(row2.OrderOnBallot());
      }
    }

    private sealed class ElectionReportDataManager : RunningMateDataManager
    {
      public void GetData(string electionKey)
      {
        DataTable = Elections.GetElectionReportData(electionKey);
      }

      public IEnumerable<IGrouping<string, DataRow>> GetGroupedSubset(
        FilterBy filterBy = null, OrderBy orderBy = null)
      {
        return GetDataSubset(filterBy, orderBy ?? new ElectionSort())
          .GroupBy(row => row.OfficeKey())
          .ToList();
      }
    }

    private abstract class OneCategory
    {
      protected ElectionReportResponsive ElectionReport;
      protected string ElectionKey;
      protected string StateCode;
      protected string CountyCode;
      protected string LocalCode;
      protected bool IsForAllStatesReport;

      public void Generate(ElectionReportResponsive electionReport, string electionKey,
        bool isForAllStatesReport = false)
      {
        ElectionReport = electionReport;
        ElectionKey = electionKey;
        StateCode = Elections.GetStateCodeFromKey(electionKey);
        CountyCode = Elections.GetCountyCodeFromKey(electionKey);
        LocalCode = Elections.GetLocalCodeFromKey(electionKey);
        IsForAllStatesReport = isForAllStatesReport;

        var offices = GetOffices()
          .Where(g => g.First().ElectionsPoliticianKey() != null)
          .ToList();

        if (offices.Count <= 0) return;
        if ((electionReport.ReportUser == ReportUser.Public) &&
          Elections.IsPrimaryElection(electionKey))
          offices = FilterUncontestedOffices(offices)
            .ToList();

        Control container;
        // only create a category wrapper if more than one office
        if ((offices.Count > 1) || isForAllStatesReport)
        {
          // ReSharper disable once PossibleNullReferenceException
          (new HtmlDiv().AddTo(ElectionReport.ReportContainer, "category-title accordion-header")
            as HtmlGenericControl).InnerHtml = GetCategoryTitle();
          container = new HtmlDiv().AddTo(ElectionReport.ReportContainer,
            "category-content accordion-content");
        }
        else
          container = ElectionReport.ReportContainer;

        foreach (var office in offices)
        {
          var politicians = office.ToList();
          var officeInfo = politicians[0];

          var officeContent = new HtmlDiv();
          ElectionReport.CreateOfficeTitle(container, officeContent, politicians,
            GetOfficeTitle(officeInfo));
          officeContent.AddTo(container, "office-content accordion-content");
          var isRunningMateOffice = officeInfo.IsRunningMateOffice() &&
            !Elections.IsPrimaryElection(electionKey);
          ReportOffice(officeContent, isRunningMateOffice, politicians,
            ElectionReport._DataManager, false, false,
            ElectionReport.ReportUser == ReportUser.Master);
        }
      }

      protected abstract string GetCategoryTitle();

      protected virtual IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return ElectionReport._DataManager.GetGroupedSubset(null,
          new OfficeOrderSort());
      }

      protected virtual string GetOfficeTitle(DataRow officeInfo)
      {
        return Offices.FormatOfficeName(officeInfo);
      }
    }

    private sealed class USPresidentCategory : OneCategory
    {
      private class USPresidentFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;
        private readonly bool _IsPresidentialComparison;

        public USPresidentFilter(string stateCode, string electionKey)
        {
          _StateCode = stateCode;
          _IsPresidentialComparison =
            Elections.GetElectionTypeFromKey(electionKey) ==
            Elections.ElectionTypeUSPresidentialPrimary;
        }

        public override bool Filter(DataRow row)
        {
          return (_IsPresidentialComparison || row.StateCode()
              .IsEqIgnoreCase(_StateCode)) &&
            (row.OfficeClass() == OfficeClass.USPresident) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return Elections.GetElectionTypeFromKey(ElectionKey) ==
          Elections.ElectionTypeUSPresidentialPrimary
            ? "US Presidential Candidates"
            : "US President & Vice President (" + StateCache.GetStateName(StateCode) + ")";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(
            new USPresidentFilter(StateCode, ElectionKey));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        string title;
        switch (Elections.GetElectionTypeFromKey(ElectionKey))
        {
          case Elections.ElectionTypeStatePresidentialPrimary:
            title = "US President (" + StateCache.GetStateName(StateCode) + " Primary)";
            break;
          case Elections.ElectionTypeUSPresidentialPrimary:
            title = "US Presidential Candidates";
            break;
          default:
            title = "US President & Vice President (" + StateCache.GetStateName(StateCode) + ")";
            break;
        }
        return title;
      }
    }

    private sealed class USSenateCategory : OneCategory
    {
      private class USSenateFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;

        public USSenateFilter(string stateCode)
        {
          _StateCode = stateCode;
        }

        public override bool Filter(DataRow row)
        {
          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (row.OfficeClass() == OfficeClass.USSenate) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        if (IsForAllStatesReport) return StateCache.GetStateName(StateCode);
        return "US Senate";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(new USSenateFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        if (IsForAllStatesReport) return "US Senate (" + StateCache.GetStateName(StateCode) + ")";
        return "US Senate";
      }
    }

    private sealed class USHouseCategory : OneCategory
    {
      private class USHouseFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;

        public USHouseFilter(string stateCode)
        {
          _StateCode = stateCode;
        }

        public override bool Filter(DataRow row)
        {
          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (row.OfficeClass() == OfficeClass.USHouse) && !row.IsRunningMate();
        }
      }

      private sealed class USHouseSort : ReportDataManager<DataRow>.OrderBy
      {
        public override int Compare(DataRow row1, DataRow row2)
        {
          // new sort per Mantis #417 (like OfficeControl sort)
          return MixedNumericComparer.Instance.Compare(Offices.FormatOfficeName(row1),
            Offices.FormatOfficeName(row2));
        }
      }

      protected override string GetCategoryTitle()
      {
        if (IsForAllStatesReport) return StateCache.GetStateName(StateCode);
        return "US House of Representatives";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(new USHouseFilter(StateCode),
            new USHouseSort());
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        var title = Offices.GetLocalizedOfficeName(officeInfo);
        if (IsForAllStatesReport) title += " (" + StateCache.GetStateName(StateCode) + ")";
        return title;
      }
    }

    private sealed class USGovernorsCategory : OneCategory
    {
      private class USGovernorsFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;
        private readonly bool _IsForAllStatesReport;

        public USGovernorsFilter(string stateCode, bool isForAllStatesReport)
        {
          _StateCode = stateCode;
          _IsForAllStatesReport = isForAllStatesReport;
        }

        public override bool Filter(DataRow row)
        {
          if (_IsForAllStatesReport && (_StateCode == "DC")) return false;
          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            ((row.AlternateOfficeClass() == OfficeClass.USGovernors) ||
              (row.AlternateOfficeClass() == OfficeClass.USLtGovernor) ||
              (row.AlternateOfficeClass() == OfficeClass.DCMayor)) &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return IsForAllStatesReport
          ? StateCache.GetStateName(StateCode)
          : StateCode == "DC"
            ? "Mayor District of Columbia"
            : "Governor and Lieutenant Governor";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(
            new USGovernorsFilter(StateCode, IsForAllStatesReport));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return Offices.GetLocalizedOfficeName(officeInfo);
      }
    }

    private sealed class StatewideCategory : OneCategory
    {
      private class StatewideFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;

        public StatewideFilter(string stateCode)
        {
          _StateCode = stateCode;
        }

        public override bool Filter(DataRow row)
        {
          if (_StateCode == "DC")
            return (row.AlternateOfficeClass() == OfficeClass.DCCouncil) ||
              (row.AlternateOfficeClass() == OfficeClass.DCShadowSenator) ||
              (row.AlternateOfficeClass() == OfficeClass.DCShadowRepresentative);

          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (((row.OfficeClass() == OfficeClass.StateWide) &&
                (row.AlternateOfficeClass() != OfficeClass.USGovernors) &&
                (row.AlternateOfficeClass() != OfficeClass.USLtGovernor))
              || (row.OfficeClass() == OfficeClass.StateJudicial) ||
              (row.OfficeClass() == OfficeClass.StateParty))
            && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCode == "DC"
          ? "District of Columbia"
          : StateCache.GetStateName(StateCode) + " Statewide Offices";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(
            new StatewideFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return Offices.GetLocalizedOfficeName(officeInfo);
      }
    }

    private sealed class StateSenateCategory : OneCategory
    {
      private class StateSenateFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;

        public StateSenateFilter(string stateCode)
        {
          _StateCode = stateCode;
        }

        public override bool Filter(DataRow row)
        {
          if (_StateCode == "DC")
            return row.AlternateOfficeClass() == OfficeClass.DCBoardOfEducation;

          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (row.OfficeClass() == OfficeClass.StateSenate) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCode == "DC"
          ? "District of Columbia State Board of Education"
          : StateCache.GetStateName(StateCode) + " Senate";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(
            new StateSenateFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        if (StateCode == "DC")
          switch (officeInfo.OfficeKey())
          {
            case "DCPresidentOfTheBoardOfEducation":
              return "President of the Board";

            case "DCStateBoardOfEducationStudentRepresentative":
              return "Student Representative";

            default:
              return "Board Member Ward " + officeInfo.DistrictCode()
                .TrimStart('0');
          }

        return Offices.FormatOfficeName(officeInfo);
      }
    }

    private sealed class StateHouseCategory : OneCategory
    {
      private class StateHouseFilter : ReportDataManager<DataRow>.FilterBy
      {
        private readonly string _StateCode;

        public StateHouseFilter(string stateCode)
        {
          _StateCode = stateCode;
        }

        public override bool Filter(DataRow row)
        {
          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (row.OfficeClass() == OfficeClass.StateHouse) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCode == "DC"
          ? "District of Columbia Advisory Neighborhood Commissioners"
          : StateCache.GetStateName(StateCode) + " House";
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(
            new StateHouseFilter(StateCode));
      }
    }

    private sealed class CountyCategory : OneCategory
    {
      protected override string GetCategoryTitle()
      {
        return CountyCache.GetCountyName(StateCode, CountyCode) + ", " +
          StateCache.GetStateName(StateCode) + " Countywide Offices";
      }
    }

    private sealed class LocalCategory : OneCategory
    {
      protected override string GetCategoryTitle()
      {
        return VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(StateCode, CountyCode, LocalCode) + ", " +
          CountyCache.GetCountyName(StateCode, CountyCode) + ", " +
          StateCache.GetStateName(StateCode) + " Offices";
      }
    }

    #endregion Private classes

    private void CreateLowerLevelElectionAnchors(string electionKey)
    {
      switch (Elections.GetElectoralClass(electionKey))
      {
        case ElectoralClass.State:
          CountyElectionLinksResponsive.GetReport(ReportContainer, electionKey, _OpenAll);
          break;

        case ElectoralClass.County:
          LocalElectionLinksResponsive.GetReport(ReportContainer, electionKey, _OpenAll);
          break;
      }
    }

    private void GetLocalElections(Control container, string countyElectionKey)
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
      var content = new HtmlDiv().AddTo(container, "category-content accordion-content");

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

        new HtmlDiv {InnerText = localName}.AddTo(content, "accordion-header");
        new ElectionReportResponsive().GenerateReport(ReportUser, localElectionKey)
          .AddTo(content, "accordion-content");
      }
    }

    private void GetCountyElections(Control container, string stateElectionKey)
    {
      var stateCode = Elections.GetStateCodeFromKey(stateElectionKey);

      // We get a dictionary of counties with elections that match the stateElectionKey
      // Key: countyCode; Value: county electionKey
      // Counties without an election will not be in the dictionary
      // We include local elections too, to account for situations where there is no county
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
      var content = new HtmlDiv().AddTo(container, "category-content accordion-content");

      // For reporting we start with all counties for the state (it will be in
      // order by county name) and select only those in the election dictionary.

      var counties = CountyCache.GetCountiesByState(stateCode)
        .Where(countyElectionDictionary.ContainsKey)
        .ToList();

      // each county report is in its own accordion
      foreach (var countyCode in counties)
      {
        var countyName = CountyCache.GetCountyName(stateCode, countyCode);
        var countyElectionKey = countyElectionDictionary[countyCode];

        new HtmlDiv {InnerText = countyName}.AddTo(content, "accordion-header");
        new ElectionReportResponsive().GenerateReport(ReportUser, countyElectionKey)
          .AddTo(content, "accordion-content");
      }
    }

    private void CreateLowerLevelElectionReports(string electionKey)
    {
      switch (Elections.GetElectoralClass(electionKey))
      {
        case ElectoralClass.State:
          GetCountyElections(ReportContainer, electionKey);
          break;

        case ElectoralClass.County:
          GetLocalElections(ReportContainer, electionKey);
          break;
      }
    }

    private void CreateOfficeTitle(Control categoryContent, Control officeContent,
      IList<DataRow> politicians, string officeTitle)
    {
      var officeHeading = officeTitle;
      var officeInfo = politicians[0];
      var electionKey = officeInfo.ElectionKey();
      var officeKey = officeInfo.OfficeKey();
      var officeClass = officeInfo.OfficeClass();

      switch (officeInfo.ElectionType())
      {
        case Elections.ElectionTypeUSPresidentialPrimary:
        case Elections.ElectionTypeStatePresidentialPrimary:
        case Elections.ElectionTypeStatePrimary:
          officeHeading += " " +
            Parties.GetNationalPartyDescription(officeInfo.NationalPartyCode());
          //Parties.GetNationalOrStatePartyDescription(officeInfo.PartyKey(), officeInfo.PartyName());
          break;
      }

      var titleDiv = new HtmlDiv().AddTo(categoryContent, "office-title accordion-header");
      new HtmlSpan {InnerText = officeHeading}.AddTo(titleDiv);

      if (ReportUser == ReportUser.Master)
        CreateMasterOfficeLinks(officeContent, officeInfo);
      else
      {
        //DC ANCs are not anchors because there are too many candidates to compare
        //and we don't know who is running against who
        if ((officeInfo.StateCode() != "DC") || (officeClass != OfficeClass.StateHouse))
          CreateCompareOrIntroAnchor(officeContent, politicians, electionKey, officeKey);
      }
    }

    private void CreateMasterOfficeLinks(Control officeContent, DataRow officeInfo)
    {
      var div = new HtmlDiv().AddTo(officeContent, "master-links");
      CreateAdminOfficeAnchor(officeInfo.OfficeKey(), "Edit Office").AddTo(div);
      CreateAdminOfficeCandidateListAnchor(officeInfo.ElectionKey(), officeInfo.OfficeKey(),
        "Edit Candidate List").AddTo(div);
      if (_IsMostRecentElection)
        CreateAdminOfficeWinnerAnchor(officeInfo.ElectionKey(), officeInfo.OfficeKey(),
          "Identify Winners").AddTo(div);
    }

    private Control GenerateReport(ReportUser reportUser, string electionKey, bool openAll = false)
    {
      ReportUser = reportUser;

      if (ReportUser == ReportUser.Master)
      {
        var recentElections =
          Elections.GetLatestElectionsByStateCode(
            Elections.GetStateCodeFromKey(electionKey));
        _IsMostRecentElection = recentElections.Contains(electionKey,
          StringComparer.OrdinalIgnoreCase);
      }

      _OpenAll = openAll;
      _DataManager.GetData(electionKey);

      var electoralClass = Elections.GetElectoralClass(electionKey);
      switch (electoralClass)
      {
        case ElectoralClass.USPresident:
          if (Elections.GetElectionTypeFromKey(electionKey) ==
            Elections.ElectionTypeUSPresidentialPrimary)
            new USPresidentCategory().Generate(this, electionKey);
          else
            foreach (var stateCode in StateCache.All51StateCodes)
              new USPresidentCategory().Generate(this,
                Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode), true);
          break;

        case ElectoralClass.USSenate:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USSenateCategory().Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode), true);
          break;

        case ElectoralClass.USHouse:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USHouseCategory().Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode),
              true);
          break;

        case ElectoralClass.USGovernors:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USGovernorsCategory().Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode),
              true);
          break;

        case ElectoralClass.State:
          new USPresidentCategory().Generate(this, electionKey);
          new USSenateCategory().Generate(this, electionKey);
          new USHouseCategory().Generate(this, electionKey);
          new USGovernorsCategory().Generate(this, electionKey);
          new StatewideCategory().Generate(this, electionKey);
          new StateSenateCategory().Generate(this, electionKey);
          new StateHouseCategory().Generate(this, electionKey);
          break;

        case ElectoralClass.County:
          new CountyCategory().Generate(this, electionKey);
          break;

        case ElectoralClass.Local:
          new LocalCategory().Generate(this, electionKey);
          break;
      }

      ReportReferendums(electionKey);

      switch (electoralClass)
      {
        case ElectoralClass.State:
        case ElectoralClass.County:
          if (ReportUser == ReportUser.Master)
            CreateLowerLevelElectionReports(electionKey);
          else
            CreateLowerLevelElectionAnchors(electionKey);
          break;
      }

      return ReportContainer.AddCssClasses("election-report");
    }

    private void ReportReferendums(string electionKey)
    {
      var referendums = Referendums.GetElectionReportSummaryData(electionKey);
      if (referendums.Count == 0) return;

      // ReSharper disable once PossibleNullReferenceException
      (new HtmlDiv().AddTo(ReportContainer, "category-title accordion-header")
        as HtmlGenericControl).InnerHtml = "Referendums and Ballot Measures";
      var container = new HtmlDiv().AddTo(ReportContainer,
        "category-content referendums-content accordion-content");

      foreach (var referendum in referendums) ReportOneReferendum(container, referendum);
    }

    #endregion Private

    public static Control GetReport(ReportUser reportUser, string electionKey, bool openAll = false)
    {
      var reportObject = new ElectionReportResponsive();
      return reportObject.GenerateReport(reportUser, electionKey, openAll);
    }
  }
}