using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal sealed class ElectionReport : TableBasedReport
  {
    #region Private

    private readonly ElectionReportDataManager _DataManager =
      new ElectionReportDataManager();

    private bool _IsMostRecentElection;

    private const int MaxCellsPerRow = 3;
    private const int MaxCellsPerRowRunningMate = 2;

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

    private sealed class ElectionReportDataManager : ReportDataManager<DataRow>
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

      public DataRow GetRunningMate(string officeKey, string politicianKey)
      {
        return DataTable.Rows.OfType<DataRow>()
          .FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
              .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }
    }

    private abstract class OneCategory
    {
      protected ElectionReport ElectionReport;
      protected string ElectionKey;
      protected string StateCode;
      protected string CountyCode;
      protected string LocalCode;
      protected bool IsForAllStatesReport;

      //protected bool IsRunningMateOffice;

      public void Generate(ElectionReport electionReport, string electionKey,
        bool isForAllStatesReport = false)
      {
        ElectionReport = electionReport;
        ElectionKey = electionKey;
        StateCode = Elections.GetStateCodeFromKey(electionKey);
        CountyCode = Elections.GetCountyCodeFromKey(electionKey);
        LocalCode = Elections.GetLocalCodeFromKey(electionKey);
        IsForAllStatesReport = isForAllStatesReport;

        // per Mantis #264, comment eliminates empty offices on admin report
        var offices = GetOffices()
          .Where(g => /*electionReport.ReportUser != ReportUser.Public ||*/ g.First()
            .ElectionsPoliticianKey() != null)
          .ToList();

        if (offices.Count <= 0) return;
        if (electionReport.ReportUser == ReportUser.Public &&
          Elections.IsPrimaryElection(electionKey))
          offices = FilterUncontestedOffices(offices)
            .ToList();

        ElectionReport.StartNewHtmlTable();
        ElectionReport.CreateCategoryHeading(GetCategoryTitle());

        foreach (var office in offices)
        {
          var politicians = office.ToList();
          var officeInfo = politicians[0];

          ElectionReport.StartNewHtmlTable();

          ElectionReport.CreateOfficeTitle(politicians, GetOfficeTitle(officeInfo));
          ElectionReport.StartNewHtmlTable();
          ElectionReport.ReportOffice(officeInfo.IsRunningMateOffice(), politicians);
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
          ? "U.S. Presidential Candidates"
          : StateCache.GetStateName(StateCode);
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
            title = "U.S. President";
            //IsRunningMateOffice = false;
            break;
          case Elections.ElectionTypeUSPresidentialPrimary:
            title = "U.S. Presidential Candidates";
            //IsRunningMateOffice = false;
            break;
          default:
            title = "U.S. President & Vice President";
            //IsRunningMateOffice = true;
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
          return (row.StateCode()
            .IsEqIgnoreCase(_StateCode)) &&
            row.OfficeClass() == OfficeClass.USSenate && !row.IsRunningMate();
        }
      }

      protected override string GetCategoryTitle()
      {
        return StateCache.GetStateName(StateCode);
      }

      protected override IEnumerable<IGrouping<string, DataRow>> GetOffices()
      {
        return
          ElectionReport._DataManager.GetGroupedSubset(new USSenateFilter(StateCode));
      }

      protected override string GetOfficeTitle(DataRow officeInfo)
      {
        return "U.S. Senate";
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
          return (row.StateCode()
            .IsEqIgnoreCase(_StateCode)) &&
            row.OfficeClass() == OfficeClass.USHouse && !row.IsRunningMate();
        }
      }

      private sealed class USHouseSort : ReportDataManager<DataRow>.OrderBy
      {
        public override int Compare(DataRow row1, DataRow row2)
        {
          // new sort per Mantis #417 (like OfficeControl sort)
          return MixedNumericComparer.Instance.Compare(Offices.FormatOfficeName(row1), Offices.FormatOfficeName(row2));
        }
      }

      protected override string GetCategoryTitle()
      {
        //Federal State-by-State reports have a header of State Name
        //State reports have a header of the Office Title
        var title = StateCache.GetStateName(StateCode);
        if (!IsForAllStatesReport) title += " U.S. House of Representatives";
        return title;
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
        if (IsForAllStatesReport) title = StateCache.GetStateName(StateCode) + " " + title;
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
              row.AlternateOfficeClass() == OfficeClass.DCShadowRepresentative;

          return row.StateCode()
            .IsEqIgnoreCase(_StateCode) &&
            (row.OfficeClass() == OfficeClass.StateWide && row.AlternateOfficeClass() != OfficeClass.USGovernors && row.AlternateOfficeClass() != OfficeClass.USLtGovernor
            || row.OfficeClass() == OfficeClass.StateJudicial || row.OfficeClass() == OfficeClass.StateParty)
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
          if (_StateCode == "DC") return row.AlternateOfficeClass() == OfficeClass.DCBoardOfEducation;

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

    private void CreateCandidateCell(DataRow politician)
    {
      if (politician != null)
        ReportPolitician(politician,
          politician.IsWinner() /*&& !politician.IsRunningMate()*/,
          politician.IsIncumbent());
      else // create empty cell
      {
        new HtmlTableCell {InnerHtml = "&nbsp;"}.AddTo(CurrentPoliticianRow,
          "tdReportImage");
        new HtmlTableCell {InnerHtml = "&nbsp;"}.AddTo(CurrentPoliticianRow,
          "tdReportDetail");
      }
    }

    private Control CreateLowerLevelElectionAnchors(string electionKey)
    {
      switch (Elections.GetElectoralClass(electionKey))
      {
        case ElectoralClass.State:
          return CountyElectionLinks.GetReport(ReportUser, electionKey);

        case ElectoralClass.County:
          return LocalElectionLinks.GetReport(ReportUser, electionKey);
      }
      return null;
    }

    private void CreateOfficeTitle(IList<DataRow> politicians, string officeTitle)
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

      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading");
      var td = new HtmlTableCell {Align = "center"}.AddTo(tr,
        "tdReportGroupHeading");

      switch (ReportUser)
      {
        case ReportUser.Public:
        {
          //DC ANCs are not anchors because there are too many candidates to compare
          //and we don't know who is running against who
          if (officeInfo.StateCode() == "DC" &&
            officeClass == OfficeClass.StateHouse) new Literal {Text = officeHeading}.AddTo(td);
          else if (politicians.Count > 1)
          {
            CreateIssuePageAnchor(electionKey, officeKey, "ALLBio", officeHeading,
              "Compare candidates' bios and views and positions on the issues")
              .AddTo(td);
            new HtmlBreak().AddTo(td);
            CreateCompareTheCandidatesAnchorTable(electionKey, officeKey)
              .AddTo(td);
          }
          else
            CreatePoliticianIntroAnchor(politicians[0], officeHeading)
              .AddTo(td);
          break;
        }
        default:
        {
          CreateAdminOfficeAnchor(officeKey, officeHeading)
            .AddTo(td);
          new HtmlBreak().AddTo(td);
          CreateAdminOfficeCandidateListAnchor(electionKey, officeKey, "Office Contest",
            Offices.FormatOfficeName(officeInfo) + " edit office contest")
            .AddTo(td);
          if (_IsMostRecentElection)
          {
            new Literal {Text = " "}.AddTo(td);
            CreateAdminOfficeWinnerAnchor(electionKey, officeKey,
              " ... identify winner(s)")
              .AddTo(td);
          }
          break;
        }
      }
    }

    private static HtmlAnchor CreateReferendumPageAnchor(string electionKey,
      string referendumKey, string anchorText)
    {
      var a = new HtmlAnchor
      {
        HRef = UrlManager.GetReferendumPageUri(electionKey, referendumKey)
          .ToString(),
        Title = "Referendum Description, Details and Full Text",
        InnerHtml = anchorText
      };
      return a;
    }

    private Control GenerateReport(ReportUser reportUser, string electionKey)
    {
      ReportUser = reportUser;

      if (ReportUser != ReportUser.Public)
      {
        var recentElections =
          Elections.GetLatestElectionsByStateCode(
            Elections.GetStateCodeFromKey(electionKey));
        _IsMostRecentElection = recentElections.Contains(electionKey,
          StringComparer.OrdinalIgnoreCase);
      }

      _DataManager.GetData(electionKey);

      var electoralClass = Elections.GetElectoralClass(electionKey);
      switch (electoralClass)
      {
        case ElectoralClass.USPresident:
          if (Elections.GetElectionTypeFromKey(electionKey) ==
            Elections.ElectionTypeUSPresidentialPrimary) (new USPresidentCategory()).Generate(this, electionKey);
          else
            foreach (var stateCode in StateCache.All51StateCodes)
              (new USPresidentCategory()).Generate(this,
                Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode));
          break;

        case ElectoralClass.USSenate:
          foreach (var stateCode in StateCache.All51StateCodes)
            (new USSenateCategory()).Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode));
          break;

        case ElectoralClass.USHouse:
          foreach (var stateCode in StateCache.All51StateCodes)
            (new USHouseCategory()).Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode),
              true);
          break;

        case ElectoralClass.USGovernors:
          foreach (var stateCode in StateCache.All51StateCodes)
            (new USGovernorsCategory()).Generate(this,
              Elections.InsertStateCodeIntoElectionKey(electionKey, stateCode),
              true);
          break;

        case ElectoralClass.State:
          (new USPresidentCategory()).Generate(this, electionKey);
          (new USSenateCategory()).Generate(this, electionKey);
          (new USHouseCategory()).Generate(this, electionKey);
          (new USGovernorsCategory()).Generate(this, electionKey);
          (new StatewideCategory()).Generate(this, electionKey);
          (new StateSenateCategory()).Generate(this, electionKey);
          (new StateHouseCategory()).Generate(this, electionKey);
          break;

        case ElectoralClass.County:
          (new CountyCategory()).Generate(this, electionKey);
          break;

        case ElectoralClass.Local:
          (new LocalCategory()).Generate(this, electionKey);
          break;
      }

      ReportReferendums(electionKey);

      switch (electoralClass)
      {
        case ElectoralClass.State:
        case ElectoralClass.County:
          var control = CreateLowerLevelElectionAnchors(electionKey);
          if (control != null) control.AddTo(ReportContainer);
          break;
      }

      return ReportContainer;
    }

    private void ReportReferendums(string electionKey)
    {
      var referendums = Referendums.GetElectionReportSummaryData(electionKey);
      if (referendums.Count == 0) return;

      var htmlTable = CreateInitializedHtmlTable()
        .AddTo(ReportContainer);
      var tr = new HtmlTableRow().AddTo(htmlTable, "trReportHeading");
      new HtmlTableCell
      {
        Align = "center",
        InnerHtml = "Referendums and Ballot Measures"
      }.AddTo(tr, "tdReportHeading");

      foreach (var referendum in referendums) ReportOneReferendum(htmlTable, referendum);
    }

    private void ReportOffice(bool isRunningMateOffice, IList<DataRow> politicians)
    {
      var officeInfo = politicians[0];

      var maxCellsPerRow = isRunningMateOffice
        ? MaxCellsPerRowRunningMate
        : MaxCellsPerRow;

      var cellsInCurrentRow = int.MaxValue; // to force new row

      foreach (
        var row in politicians.Where(p => p.ElectionsPoliticianKey() != null))
      {
        if (cellsInCurrentRow >= maxCellsPerRow)
        {
          StartNewHtmlRow();
          cellsInCurrentRow = 0;
        }

        CreateCandidateCell(row);

        cellsInCurrentRow++;

        if (!isRunningMateOffice) continue;

        if (cellsInCurrentRow >= maxCellsPerRow)
        {
          StartNewHtmlRow();
          cellsInCurrentRow = 0;
        }

        var runningMate = _DataManager.GetRunningMate(officeInfo.OfficeKey(),
          row.RunningMateKey());

        CreateCandidateCell(runningMate);

        cellsInCurrentRow++;
      }
    }

    private void ReportOneReferendum(HtmlTable htmlTable, ReferendumsRow row)
    {
      var tr = new HtmlTableRow().AddTo(htmlTable, "trReportGroupHeading");
      var td = new HtmlTableCell().AddTo(tr, "tdReportGroupHeading");

      var formattedTitle = row.ReferendumTitle.ReplaceNewLinesWithBreakTags();
      var formattedDescription =
        row.ReferendumDescription.ReplaceNewLinesWithBreakTags();

      if (ReportUser == ReportUser.Public)
        CreateReferendumPageAnchor(row.ElectionKey, row.ReferendumKey,
          formattedTitle)
          .AddTo(td);
      else
        CreateAdminReferendumAnchor(row.ElectionKey, row.ReferendumKey,
          formattedTitle)
          .AddTo(td);

      if (row.IsResultRecorded && row.IsPassed)
      {
        new HtmlBreak().AddTo(td);
        // center is deprecated
        var center = new HtmlGenericControl("center").AddTo(td);
        var strong = new HtmlStrong().AddTo(center);
        var span =
          new HtmlSpan {InnerHtml = "(passed)"}.AddTo(strong);
        span.Style.Add(HtmlTextWriterStyle.Color, "red");
      }

      var referendumDetailDesc = row.ReferendumDescription != string.Empty
        ? formattedDescription
        : formattedTitle;

      tr = new HtmlTableRow().AddTo(htmlTable, "trReportDetail");
      new HtmlTableCell {InnerHtml = referendumDetailDesc}.AddTo(tr,
        "tdReportDetail");
    }

    private void StartNewHtmlRow()
    {
      CurrentPoliticianRow = new HtmlTableRow().AddTo(CurrentHtmlTable,
        "trReportGroupHeading");
    }

    #endregion Private

    public static Control GetReport(ReportUser reportUser, string electionKey)
    {
      var reportObject = new ElectionReport();
      return reportObject.GenerateReport(reportUser, electionKey);
    }
  }
}