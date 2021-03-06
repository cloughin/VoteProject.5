using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal class OfficialsReportResponsive : ResponsiveReport
  {
    #region Private

    private ElectoralClass _ElectoralClass;

    private void CreateLowerLevelOfficialsAnchors(string stateCode,
      string countyCode)
    {
      switch (_ElectoralClass)
      {
        case ElectoralClass.State:
          CountyOfficialsLinksResponsive.GetReport(ReportContainer, stateCode);
          break;

        case ElectoralClass.County:
          LocalOfficialsLinksResponsive.GetReport(ReportContainer, stateCode, countyCode);
          break;
      }
    }

    private Control GenerateReport(string stateOrFederalCode,
      string countyCode = "", string localCode = "")
    {
      _ElectoralClass = Offices.GetElectoralClass(stateOrFederalCode, countyCode,
        localCode);

      var options = new OfficialsReportOptions
      {
        ElectoralClass =
          Offices.GetElectoralClass(stateOrFederalCode, countyCode, localCode),
        StateCode = stateOrFederalCode,
        CountyCode = countyCode,
        LocalCode = localCode
      };

      DataManager.GetData(options);

      switch (options.ElectoralClass)
      {
        case ElectoralClass.USPresident:
          new USPresidentCategory().Generate(this, true);
          break;

        case ElectoralClass.USSenate:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USSenateCategory().Generate(this, true, stateCode);
          break;

        case ElectoralClass.USHouse:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USHouseCategory().Generate(this, true, stateCode);
          break;

        case ElectoralClass.USGovernors:
          foreach (var stateCode in StateCache.All51StateCodes)
            new USGovernorsCategory().Generate(this, true, stateCode);
          break;

        case ElectoralClass.State:
          new USPresidentCategory().Generate(this);
          new USSenateCategory().Generate(this, false, stateOrFederalCode);
          new USHouseCategory().Generate(this, false, stateOrFederalCode);
          new USGovernorsCategory().Generate(this, false, stateOrFederalCode);
          new StatewideCategory().Generate(this, false, stateOrFederalCode);
          new StateSenateCategory().Generate(this, false, stateOrFederalCode);
          new StateHouseCategory().Generate(this, false, stateOrFederalCode);
          break;

        case ElectoralClass.County:
          new CountyCategory().Generate(this, false, stateOrFederalCode, countyCode);
          break;

        case ElectoralClass.Local:
          new LocalCategory().Generate(this, false, stateOrFederalCode, countyCode,
            localCode);
          break;
      }

      switch (_ElectoralClass)
      {
        case ElectoralClass.State:
        case ElectoralClass.County:
          CreateLowerLevelOfficialsAnchors(stateOrFederalCode, countyCode);
          break;
      }

      return ReportContainer.AddCssClasses("officials-report");
    }

    private static void ReportMissingPolitician(Control container, string officeTitle)
    {
      var cell = new HtmlDiv().AddTo(container, "candidate-cell");
      var inner = new HtmlDiv().AddTo(cell, "candidate-cell-inner");
      new HtmlDiv {InnerText = officeTitle}.AddTo(inner, "cell-heading");
      new HtmlDiv {InnerText = "Vacant or Not Identified"}.AddTo(inner, "vacant-office");
    }

    private void ReportOfficeHeadingAndIncumbent(Control container, string officeTitle,
      DataRow politician)
    {
      //There are cases where politicianKeys have changed or consolidated
      var politicianKeyToTest = politician.IsRunningMate()
        ? politician.RunningMateKey()
        : politician.OfficialsPoliticianKey();

      if (politician.PoliticianKey()
        .IsEqIgnoreCase(politicianKeyToTest))
        ReportPolitician(container, politician, false, false, officeTitle);
      else ReportMissingPolitician(container, officeTitle);
    }

    private int ReportOneOffice(Control container, string officeTitle, string officeTitleRunningMate,
      IList<DataRow> politicians)
    {
      var entries = 0;
      var officeInfo = politicians[0];
      politicians = politicians.Where(row => row.PoliticianKey() != null)
        .ToList();

      var vacancies = officeInfo.Incumbents() - politicians.Count;
      if (vacancies < 0)
        vacancies = 0;

      foreach (var politician in politicians)
      {
        //When Lieutenant Governors run as a seperate office
        //not as a running mate use the predefined office title
        var officeTitleToUse = officeTitle;
        if (officeInfo.AlternateOfficeClass() == OfficeClass.USLtGovernor)
          officeTitleToUse = officeTitleRunningMate;

        ReportOfficeHeadingAndIncumbent(container, officeTitleToUse, politician);
        entries++;

        if (!officeInfo.IsRunningMateOffice()) continue;

        var runningMate = DataManager.GetRunningMate(officeInfo.OfficeKey(),
          officeInfo.RunningMateKey());
        if (runningMate == null)
          ReportMissingPolitician(container, officeTitleRunningMate);
        else
          ReportOfficeHeadingAndIncumbent(container, officeTitleRunningMate, runningMate);
        entries++;
      }

      for (var i = 0; i <= vacancies - 1; i++)
      {
        ReportMissingPolitician(container, officeTitle);
        entries++;
      }

      return entries;
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
        return DataTable.Rows.OfType<DataRow>()
          .FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
            .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }

      public void SetData(DataTable dataTable)
      {
        DataTable = dataTable;
      }
    }

    protected abstract class OneCategory
    {
      protected OfficialsReportResponsive OfficialsReport;
      protected bool IsForAllStatesReport;
      protected string LocalName;
      protected string StateCode;
      protected string CountyCode;
      private string _LocalCode;

      public void Generate(OfficialsReportResponsive officialsReport,
        bool isForAllStatesReport = false,
        string stateCode = "", string countyCode = "", string localCode = "",
        Control reportContainer = null)
      {
        if (reportContainer == null) reportContainer = officialsReport.ReportContainer;
        OfficialsReport = officialsReport;
        StateCode = stateCode;
        CountyCode = countyCode;
        _LocalCode = localCode;
        IsForAllStatesReport = isForAllStatesReport;

        if (!string.IsNullOrWhiteSpace(_LocalCode))
          LocalName =
            VotePage.GetPageCache()
              .LocalDistricts.GetLocalDistrict(StateCode, CountyCode, _LocalCode);

        var offices = GetOffices();
        if (offices.Count <= 0) return;

        // ReSharper disable once PossibleNullReferenceException
        (new HtmlDiv().AddTo(reportContainer, "category-title accordion-header")
          as HtmlGenericControl).InnerHtml = GetCategoryTitle();
        var container = new HtmlDiv().AddTo(reportContainer,
          "category-content accordion-content");

        var entries = 0;
        foreach (var office in offices)
        {
          var politicians = office.ToList();
          var officeInfo = politicians[0];
          entries += OfficialsReport.ReportOneOffice(container, GetOfficeTitle(officeInfo),
            GetRunningMateTitle(), politicians);
        }
        container.AddCssClasses("candidates-" + entries);
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
          return (row.OfficeClass() == OfficeClass.USPresident) &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return "US President & Vice President";
      }

      protected override IList<IGrouping<string, DataRow>> GetOffices()
      {
        return OfficialsReport.DataManager.GetGroupedSubset(new USPresidentFilter());
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return "US President";
      }

      protected override string GetRunningMateTitle()
      {
        return "US Vice President";
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
            (row.OfficeClass() == OfficeClass.USSenate) && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        if (IsForAllStatesReport) return StateCache.GetStateName(StateCode);
        return "US Senate";
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
              .IsEqIgnoreCase(_StateCode) && (row.OfficeClass() == OfficeClass.USHouse) &&
            !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        if (IsForAllStatesReport) return StateCache.GetStateName(StateCode);
        return "US House of Representatives";
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
            (row.OfficeClass() == OfficeClass.StateWide) &&
            (row.AlternateOfficeClass() != OfficeClass.USGovernors) &&
            (row.AlternateOfficeClass() != OfficeClass.USLtGovernor) &&
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
            (row.OfficeClass() == OfficeClass.StateSenate) && !row.IsRunningMate();
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
            (row.OfficeClass() == OfficeClass.StateHouse) && !row.IsRunningMate();
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
          return (row.CountyCode() != string.Empty) &&
            (row.LocalCode() == string.Empty) && !row.IsRunningMate();
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
          return (row.CountyCode() != string.Empty) &&
            (row.LocalCode() != string.Empty) && !row.IsRunningMate();
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

    public static Control GetReport(string stateOrFederalCode, string countyCode = "",
      string localCode = "")
    {
      var reportObject = new OfficialsReportResponsive();
      return reportObject.GenerateReport(stateOrFederalCode, countyCode,
        localCode);
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