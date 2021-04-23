using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public class OfficialsReport : TableBasedReport
  {
    #region Private

    private ElectoralClass _ElectoralClass;

    private const int MaxCellsPerRow = 3;

    private HtmlTableRow _CurrentTitleRow;

    private Control CreateLowerLevelOfficialsAnchors(string stateCode/*,
      string countyCode*/)
    {
      //switch (_ElectoralClass)
      //{
      //  case ElectoralClass.State:
          return CountyOfficialsLinks.GetReport(ReportUser, stateCode);

        // no longer called for county -- report generated inline
        //case ElectoralClass.County:
        //  return LocalOfficialsLinks.GetReport(ReportUser, stateCode, countyCode);
      //}
      //return null;
    }

    private Control CreateOfficeHeading(string officeTitle, string officeKey)
    {
      Control officeHeading = new PlaceHolder();

      if (!IsNullOrEmpty(officeTitle))

        if (ReportUser == ReportUser.Admin || ReportUser == ReportUser.Master)
          CreateAdminOfficeAnchor(officeKey, officeTitle).AddTo(officeHeading);
        else
          new Literal {Text = officeTitle}.AddTo(officeHeading);

      return officeHeading;
    }

    private Control GenerateReport(ReportUser reportUser, string stateOrFederalCode,
      string countyCode = "", string localKey = "")
    {
      ReportUser = reportUser;
      _ElectoralClass = Offices.GetElectoralClass(stateOrFederalCode, countyCode,
        localKey);

      var options = new OfficialsReportOptions
      {
        ElectoralClass =
          Offices.GetElectoralClass(stateOrFederalCode, countyCode, localKey),
        StateCode = stateOrFederalCode,
        CountyCode = countyCode,
        LocalKey = localKey
      };

      DataManager.GetData(options);

      switch (options.ElectoralClass)
      {
        case ElectoralClass.USPresident:
          new USPresidentCategory().Generate(this);
          break;

        case ElectoralClass.USSenate:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USSenateCategory().Generate(this, stateCode);
          break;

        case ElectoralClass.USHouse:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USHouseCategory().Generate(this, stateCode);
          break;

        case ElectoralClass.USGovernors:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USGovernorsCategory().Generate(this, stateCode,
              isForAllStatesReport: true);
          break;

        case ElectoralClass.State:
          new USPresidentCategory().Generate(this);
          new USSenateCategory().Generate(this, stateOrFederalCode);
          new USHouseCategory().Generate(this, stateOrFederalCode);
          new USGovernorsCategory().Generate(this, stateOrFederalCode);
          new StatewideCategory().Generate(this, stateOrFederalCode);
          new StateSenateCategory().Generate(this, stateOrFederalCode);
          new StateHouseCategory().Generate(this, stateOrFederalCode);
          break;

        case ElectoralClass.County:
          new CountyCategory().Generate(this, stateOrFederalCode, countyCode);
          break;

        case ElectoralClass.Local:
          new LocalCategory().Generate(this, stateOrFederalCode, countyCode,
            localKey);
          break;
      }

      switch (_ElectoralClass)
      {
        case ElectoralClass.State:
        //case ElectoralClass.County:
          var control = CreateLowerLevelOfficialsAnchors(stateOrFederalCode/*,
            countyCode*/);
          control?.AddTo(ReportContainer);
          break;
      }

      if (_ElectoralClass == ElectoralClass.County)
      {
        // we now include all local reports in-line
        // Get a dictionary of all locals with offices defined
        // Key: localKey; Value: localDistrictName
        var localNamesWithOfficesDictionary =
          Offices.GetLocalNamesWithOffices(stateOrFederalCode, countyCode);

        // For reporting, we sort the dictionary by name
        var sortedListOflocalNamesWithOffices = localNamesWithOfficesDictionary
          .OrderBy(kvp => kvp.Value)
          .Select(kvp => kvp.Key)
          .ToList();

        var reportObject = new OfficialsReport();
        foreach (var iteratedLocalKey in sortedListOflocalNamesWithOffices)
        {
          reportObject.GenerateReport(reportUser, stateOrFederalCode, countyCode,
            iteratedLocalKey).AddTo(ReportContainer);
        }
      }

      return ReportContainer;
    }

    private void ReportOfficeHeadingAndIncumbent(string officeTitle,
      DataRow politician)
    {
      var td = new HtmlTableCell {Align = "center", ColSpan = 2}.AddTo(_CurrentTitleRow,
        "tdReportGroupHeading");
      CreateOfficeHeading(officeTitle, politician.OfficeKey()).AddTo(td);

      //There are cases where politicianKeys have changed or consolidated
      var politicianKeyToTest = politician.IsRunningMate()
        ? politician.RunningMateKey()
        : politician.OfficialsPoliticianKey();

      if (politician.PoliticianKey()
        .IsEqIgnoreCase(politicianKeyToTest))
        ReportPolitician(politician, false, false);
      else ReportMissingPolitician();
    }

    private int ReportOneOffice(string officeTitle, string officeTitleRunningMate,
      IList<DataRow> politicians, int cellsInCurrentRow)
    {
      var officeInfo = politicians[0];
      politicians = politicians.Where(row => row.PoliticianKey() != null)
        .ToList();

      var vacancies = officeInfo.Incumbents() - politicians.Count;
      if (vacancies < 0)
        vacancies = 0;

      if (cellsInCurrentRow >= MaxCellsPerRow)
      {
        StartNewHtmlRows();
        cellsInCurrentRow = 0;
      }

      foreach (var politician in politicians)
      {
        if (cellsInCurrentRow >= MaxCellsPerRow)
        {
          StartNewHtmlRows();
          cellsInCurrentRow = 0;
        }

        //When Lieutenant Governors run as a seperate office
        //not as a running mate use the predefined office title
        var officeTitleToUse = officeTitle;
        if (officeInfo.AlternateOfficeClass() == OfficeClass.USLtGovernor)
          officeTitleToUse = officeTitleRunningMate;

        ReportOfficeHeadingAndIncumbent(officeTitleToUse, politician);
        cellsInCurrentRow++;

        if (!officeInfo.IsRunningMateOffice()) continue;

        if (cellsInCurrentRow >= MaxCellsPerRow)
        {
          StartNewHtmlRows();
          cellsInCurrentRow = 0;
        }

        var runningMate = DataManager.GetRunningMate(officeInfo.OfficeKey(),
          officeInfo.RunningMateKey());
        if (runningMate == null)
          ReportVacancy(officeTitleRunningMate, officeInfo.OfficeKey());
        else
          ReportOfficeHeadingAndIncumbent(officeTitleRunningMate, runningMate);

        cellsInCurrentRow++;
      }

      for (var i = 0; i <= vacancies - 1; i++)
      {
        if (cellsInCurrentRow >= MaxCellsPerRow)
        {
          StartNewHtmlRows();
          cellsInCurrentRow = 0;
        }

        ReportVacancy(officeTitle, officeInfo.OfficeKey());
        cellsInCurrentRow++;
      }

      return cellsInCurrentRow;
    }

    private void ReportVacancy(string officeTitle, string officeKey)
    {
      var td =
        new HtmlTableCell {Align = "center", ColSpan = 2}.AddTo(_CurrentTitleRow,
          "tdReportGroupHeading");
      CreateOfficeHeading(officeTitle, officeKey)
        .AddTo(td);
      ReportMissingPolitician();
    }

    private void StartNewHtmlRows()
    {
      _CurrentTitleRow = new HtmlTableRow().AddTo(CurrentHtmlTable,
        "trReportGroupHeading");
      CurrentPoliticianRow = new HtmlTableRow().AddTo(CurrentHtmlTable,
        "trReportGroupHeading");
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected sealed class OfficialsReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(OfficialsReportOptions options)
      {
        DataTable = Offices.GetOfficialsReportData(options);
      }

      public IList<IGrouping<string, DataRow>> GetGroupedSubset(
        FilterBy filterBy = null)
      {
        return GetDataSubset(filterBy, new OfficialsSort())
          .GroupBy(row => row.OfficeKey())
          .ToList();
      }

      public DataRow GetRunningMate(string officeKey, string politicianKey)
      {
        return DataList.FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
            .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }

      //public void SetData(DataTable dataTable)
      //{
      //  DataTable = dataTable;
      //}
    }

    protected abstract class OneCategory
    {
      protected OfficialsReport OfficialsReport;
      protected bool IsForAllStatesReport;
      protected string LocalName;
      protected string StateCode;
      protected string CountyCode;
      private string _LocalKey;

      public void Generate(OfficialsReport officialsReport, string stateCode = "",
        string countyCode = "", string localKey = "",
        bool isForAllStatesReport = false)
      {
        OfficialsReport = officialsReport;
        StateCode = stateCode;
        CountyCode = countyCode;
        _LocalKey = localKey;
        IsForAllStatesReport = isForAllStatesReport;

        if (!IsNullOrWhiteSpace(_LocalKey))
          LocalName = VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(StateCode, _LocalKey);

        var offices = GetOffices();
        if (offices.Count <= 0) return;

        OfficialsReport.StartNewHtmlTable();
        OfficialsReport.CreateCategoryHeading(GetCategoryTitle());

        OfficialsReport.StartNewHtmlTable();
        var cellsInCurrentRow = int.MaxValue; // to force new row

        foreach (var office in offices)
        {
          var politicians = office.ToList();
          var officeInfo = politicians[0];
          cellsInCurrentRow =
            OfficialsReport.ReportOneOffice(GetOfficeTitle(officeInfo),
              GetRunningMateTitle(), politicians, cellsInCurrentRow);
        }
      }

      protected abstract string GetCategoryTitle();

      protected virtual IList<IGrouping<string, DataRow>> GetOffices()
      {
        return OfficialsReport.DataManager.GetGroupedSubset();
      }

      protected abstract string GetOfficeTitle(DataRow officeInfo);

      protected virtual string GetRunningMateTitle()
      {
        return null;
      }
    }

    protected sealed class USPresidentCategory : OneCategory
    {
      private class USPresidentFilter : ReportDataManager<DataRow>.FilterBy
      {
        public override bool Filter(DataRow row)
        {
          return row.OfficeClass() == OfficeClass.USPresident &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return "U.S. President & Vice President";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return OfficialsReport.DataManager.GetGroupedSubset(new USPresidentFilter());
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return "U.S. President";
      }

      protected override string GetRunningMateTitle()
      {
        return "U.S. Vice President";
      }
    }

    protected sealed class USSenateCategory : OneCategory
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
        return "U.S. Senate";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(new USSenateFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return StateCache.GetStateName(StateCode) + " Senator";
      }
    }

    protected sealed class USHouseCategory : OneCategory
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
              .IsEqIgnoreCase(_StateCode) && row.OfficeClass() == OfficeClass.USHouse &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return "U.S. House of Representatives";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(new USHouseFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return StateCache.GetStateName(StateCode) + " Congressional District" + " " +
          officeInfo.DistrictCode()
            .TrimStart('0');
      }
    }

    protected sealed class USGovernorsCategory : OneCategory
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

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(
            new USGovernorsFilter(StateCode, IsForAllStatesReport));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return StateCode == "DC"
          ? "Mayor"
          : StateCache.GetStateName(StateCode) + " Governor";
      }

      protected override string GetRunningMateTitle()
      {
        return StateCache.GetStateName(StateCode) + " Lieutenant Governor";
      }
    }

    protected sealed class StatewideCategory : OneCategory
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
            return row.AlternateOfficeClass() == OfficeClass.DCCouncil;

          return row.StateCode()
              .IsEqIgnoreCase(_StateCode) &&
            Offices.StateOfficesWithoutLegislature.Contains(row.OfficeClass()) &&
            row.AlternateOfficeClass() != OfficeClass.USGovernors &&
            row.AlternateOfficeClass() != OfficeClass.USLtGovernor &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCode == "DC"
          ? "Council of the District of Columbia"
          : "Statewide Officials";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(
            new StatewideFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        if (StateCode == "DC")
          switch (officeInfo.OfficeKey())
          {
            case "DCChairmanOfTheCouncil":
              return "Chairman of the Council";

            case "DCAtLargeMemberOfTheCouncil":
              return "Member D.C. Council At-Large";

            default:
              return "Member D.C. Council Ward " + officeInfo.DistrictCode()
                .TrimStart('0');
          }

        return StateCache.GetStateName(StateCode) + " " +
          Offices.FormatOfficeName(officeInfo);
      }
    }

    protected sealed class StateSenateCategory : OneCategory
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
            row.OfficeClass() == OfficeClass.StateSenate && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCode == "DC"
          ? "District of Columbia State Board of Education"
          : StateCache.GetStateName(StateCode) + " Senate";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(
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

    protected sealed class StateHouseCategory : OneCategory
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

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return
          OfficialsReport.DataManager.GetGroupedSubset(
            new StateHouseFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return Offices.FormatOfficeName(officeInfo);
      }
    }

    protected sealed class CountyCategory : OneCategory
    {
      private class CountyFilter : ReportDataManager<DataRow>.FilterBy
      {
        public override bool Filter(DataRow row)
        {
          return !IsNullOrWhiteSpace(row.CountyCode()) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return CountyCache.GetCountyName(StateCode, CountyCode) + ", " +
          StateCache.GetStateName(StateCode) + " Elected Officials";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return OfficialsReport.DataManager.GetGroupedSubset(new CountyFilter());
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return CountyCache.GetCountyName(StateCode, CountyCode) + " " +
          Offices.FormatOfficeName(officeInfo);
      }
    }

    protected class LocalCategory : OneCategory
    {
      protected class LocalFilter : ReportDataManager<DataRow>.FilterBy
      {
        public override bool Filter(DataRow row)
        {
          return !IsNullOrWhiteSpace(row.LocalKey()) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return LocalName + ", " + CountyCache.GetCountyName(StateCode, CountyCode) +
          ", " + StateCache.GetStateName(StateCode) + " Elected Officials";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return OfficialsReport.DataManager.GetGroupedSubset(new LocalFilter());
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return LocalName + " " + Offices.FormatOfficeName(officeInfo);
      }
    }

    protected readonly OfficialsReportDataManager DataManager =
      new OfficialsReportDataManager();

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetReport(ReportUser reportUser,
      string stateOrFederalCode, string countyCode = "", string localKey = "")
    {
      var reportObject = new OfficialsReport();
      return reportObject.GenerateReport(reportUser, stateOrFederalCode, countyCode,
        localKey);
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