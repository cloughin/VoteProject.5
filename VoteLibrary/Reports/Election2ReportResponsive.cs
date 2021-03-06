using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using JetBrains.Annotations;

namespace Vote.Reports
{
  public sealed class Election2ReportResponsive : ResponsiveReport
  {
    #region Private

    private readonly ElectionReportDataManager _DataManager =
      new ElectionReportDataManager();

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
        [CanBeNull] FilterBy filterBy = null, [CanBeNull] OrderBy orderBy = null)
      {
        return GetDataSubset(filterBy, orderBy ?? new ElectionSort())
          .GroupBy(row => row.OfficeKey())
          .ToList();
      }
    }

    private abstract class OneCategory
    {
      protected Election2ReportResponsive ElectionReport;
      protected string ElectionKey;
      protected string StateCode;
      protected string CountyCode;
      protected string LocalKey;
      protected bool IsForAllStatesReport;

      public void Generate(Election2ReportResponsive electionReport, string electionKey,
        bool isForAllStatesReport = false)
      {
        ElectionReport = electionReport;
        ElectionKey = electionKey;
        StateCode = Elections.GetStateCodeFromKey(electionKey);
        CountyCode = Elections.GetCountyCodeFromKey(electionKey);
        LocalKey = Elections.GetLocalKeyFromKey(electionKey);
        var counties = LocalIdsCodes.FindCountiesWithNames(StateCode, LocalKey);
        var isMultiCountyLocal = counties.Length > 1;
        IsForAllStatesReport = isForAllStatesReport;

        var offices = GetOffices()
          .Where(g => g.First().ElectionsPoliticianKey() != null)
          .ToList();

        if (offices.Count <= 0) return;
        if (Elections.IsPrimaryElection(electionKey))
          offices = FilterUncontestedOffices(offices)
            .ToList();

        Control container;
        // only create a category wrapper if more than one office, or isMultiCountyLocal
        if (isMultiCountyLocal || offices.Count > 1 || isForAllStatesReport)
        {
          // ReSharper disable once PossibleNullReferenceException
          var title = new PlaceHolder();
          // ReSharper disable once PossibleNullReferenceException
          (new HtmlSpan().AddTo(title) as HtmlGenericControl).InnerText = GetCategoryTitle();
          if (isMultiCountyLocal)
          {
            var allCounties =
              LocalIdsCodes.FormatAllCountyNames(StateCode, LocalKey, true);
            // ReSharper disable once PossibleNullReferenceException
            (new HtmlP().AddTo(title) as HtmlGenericControl)
              .InnerText = $"Parts of this local district are in {allCounties}";
          }
          // ReSharper disable once PossibleNullReferenceException
          new HtmlDiv().AddTo(ElectionReport.ReportContainer, "category-title accordion-header")
            .Controls.Add(title);
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
          CreateOfficeTitle(container, officeContent, politicians,
            GetOfficeTitle(officeInfo));
          officeContent.AddTo(container, "office-content accordion-content");
          //var isRunningMateOffice = officeInfo.IsRunningMateOffice() &&
          //  !Elections.IsPrimaryElection(electionKey);
          var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(electionKey,
              officeInfo.OfficeKey());
          ReportOffice(officeContent, isRunningMateOffice, politicians,
            ElectionReport._DataManager);
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
            row.OfficeClass() == OfficeClass.USPresident && !row.IsRunningMate();
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
            row.OfficeClass() == OfficeClass.USSenate && !row.IsRunningMate();
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
            row.OfficeClass() == OfficeClass.USHouse && !row.IsRunningMate();
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
          if (_IsForAllStatesReport && _StateCode == "DC") return false;
          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (row.AlternateOfficeClass() == OfficeClass.USGovernors ||
              row.AlternateOfficeClass() == OfficeClass.USLtGovernor ||
              row.AlternateOfficeClass() == OfficeClass.DCMayor) &&
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
            return row.AlternateOfficeClass() == OfficeClass.DCCouncil ||
              row.AlternateOfficeClass() == OfficeClass.DCShadowSenator ||
              row.AlternateOfficeClass() == OfficeClass.DCShadowRepresentative ||
              row.AlternateOfficeClass() == OfficeClass.DCBoardOfEducation ||
              row.OfficeClass() == OfficeClass.StateSenate && row.AlternateOfficeClass() == OfficeClass.Undefined ||
              row.OfficeClass() == OfficeClass.StateWide && row.AlternateOfficeClass() == OfficeClass.Undefined;

          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            (Offices.StateOfficesWithoutLegislature.Contains(row.OfficeClass()) &&
              row.AlternateOfficeClass() != OfficeClass.USGovernors &&
              row.AlternateOfficeClass() != OfficeClass.USLtGovernor
              || row.OfficeClass() == OfficeClass.StateJudicial ||
              row.OfficeClass() == OfficeClass.StateParty)
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
          //if (_StateCode == "DC")
          //  return row.AlternateOfficeClass() == OfficeClass.DCBoardOfEducation ||
          //    row.OfficeClass() == OfficeClass.StateSenate && row.AlternateOfficeClass() == OfficeClass.Undefined;

          if (_StateCode == "DC") return false;

          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            row.OfficeClass() == OfficeClass.StateSenate && !row.IsRunningMate();
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
        //if (StateCode == "DC")
        //  switch (officeInfo.OfficeKey())
        //  {
        //    case "DCPresidentOfTheBoardOfEducation":
        //      return "President of the Board";

        //    case "DCStateBoardOfEducationStudentRepresentative":
        //      return "Student Representative";

        //    default:
        //      return "Board Member Ward " + officeInfo.DistrictCode()
        //        .TrimStart('0');
        //  }

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
            row.OfficeClass() == OfficeClass.StateHouse && !row.IsRunningMate();
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
          StateCache.GetStateName(StateCode) + " - Countywide Offices";
      }
    }

    private sealed class LocalCategory : OneCategory
    {
      protected override string GetCategoryTitle()
      {
        var pageCache = VotePage.GetPageCache();
        return pageCache.LocalDistricts.GetLocalDistrict(StateCode, LocalKey) + ", " +
          CountyCache.GetCountyDescription(StateCode, CountyCode, LocalKey) + ", " +
          StateCache.GetStateName(StateCode) + " - Local Offices";
      }
    }

    #endregion Private classes

    private void CreateLowerLevelElectionAnchors(string electionKey)
    {
      switch (Elections.GetElectoralClass(electionKey))
      {
        case ElectoralClass.State:
          CountyElectionLinksResponsive.GetReport(ReportContainer, electionKey);
          break;

        case ElectoralClass.County:
          LocalElectionLinksResponsive.GetReport(ReportContainer, electionKey);
          break;
      }
    }

    //private int CreateLowerLevelElectionReports(string electionKey)
    //{
    //  switch (Elections.GetElectoralClass(electionKey))
    //  {
    //    case ElectoralClass.State:
    //      return GetCountyElections(ReportContainer, electionKey);

    //    case ElectoralClass.County:
    //      return GetLocalElections(ReportContainer, electionKey, _MultiCountySection);

    //    default:
    //      return 0;
    //  }
    //}

    private static void CreateOfficeTitle(Control categoryContent, Control officeContent,
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
          break;
      }

      var titleDiv = new HtmlDiv().AddTo(categoryContent, "office-title accordion-header");
      new HtmlSpan {InnerText = officeHeading}.AddTo(titleDiv);
      //DC ANCs are not anchors because there are too many candidates to compare
      //and we don't know who is running against who
      if (officeInfo.StateCode() != "DC" || officeClass != OfficeClass.StateHouse)
        CreateCompareOrIntroAnchor(officeContent, politicians, electionKey, officeKey);
    }

    private Control GenerateReport(string electionKey)
    {
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

    public static Control GetReport(string electionKey)
    {
      var reportObject = new Election2ReportResponsive();
      return reportObject.GenerateReport(electionKey);
    }
  }
}