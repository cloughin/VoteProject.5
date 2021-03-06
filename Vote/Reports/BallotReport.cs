//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal sealed class BallotReport : Report
//  {
//    #region Private

//    private readonly BallotReportDataManager _DataManager =
//      new BallotReportDataManager();

//    private const int MaxContestsInRow = 3;
//    private const int ImageSizeReports = 100;

//    private HtmlTable _MainHtmlTable;
//    private HtmlTableRow _MainTableRow;
//    private int _ContestsInRow = int.MaxValue; // force new row on first contest
//    private int _TotalContests;

//    private string _ElectionKey;
//    private string _StateElectionKey;
//    private string _StateCode;
//    private string _Congress;
//    private string _StateSenate;
//    private string _StateHouse;
//    private string _CountyCode;
//    private bool _UseExplorer;

//    private sealed class StateFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return string.IsNullOrWhiteSpace(row.CountyCode()) && !row.IsRunningMate();
//      }
//    }

//    private sealed class CountyFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return !string.IsNullOrWhiteSpace(row.CountyCode()) &&
//          string.IsNullOrWhiteSpace(row.LocalCode()) && !row.IsRunningMate();
//      }
//    }

//    private sealed class LocalFilter : ReportDataManager<DataRow>.FilterBy
//    {
//      public override bool Filter(DataRow row)
//      {
//        return !string.IsNullOrWhiteSpace(row.LocalCode()) && !row.IsRunningMate();
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
//        return row.LocalCode() == _LocalCode && !row.IsRunningMate();
//      }
//    }

//    private sealed class BallotOrderBy : ReportDataManager<DataRow>.OrderBy
//    {
//      public override int Compare(DataRow row1, DataRow row2)
//      {
//        var result = row1.OfficeOrder()
//          .CompareTo(row2.OfficeOrder());
//        if (result != 0) return result;
//        result = row1.OfficeOrderWithinLevel()
//          .CompareTo(row2.OfficeOrderWithinLevel());
//        if (result != 0) return result;
//        result = string.Compare(row1.OfficeKey(), row2.OfficeKey(),
//          StringComparison.OrdinalIgnoreCase);
//        if (result != 0) return result;
//        result = row1.OrderOnBallot()
//          .CompareTo(row2.OrderOnBallot());
//        if (result != 0) return result;
//        result = string.Compare(row1.LastName(), row2.LastName(),
//          StringComparison.OrdinalIgnoreCase);
//        if (result != 0) return result;
//        return string.Compare(row1.FirstName(), row2.FirstName(),
//          StringComparison.OrdinalIgnoreCase);
//      }
//    }

//    private sealed class BallotReportDataManager : ReportDataManager<DataRow>
//    {
//      private string ElectionKey { get; set; }

//      public void GetData(string electionKey, string congress, string stateSenate,
//        string stateHouse, string countyCode)
//      {
//        ElectionKey = electionKey;
//        DataTable = ElectionsPoliticians.GetSampleBallotData(electionKey, congress,
//          stateSenate, stateHouse, countyCode);
//      }

//      public List<IGrouping<string, DataRow>> GetOfficeGroups(
//        FilterBy filterBy = null, OrderBy orderBy = null)
//      {
//        var offices = GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
//          .GroupBy(row => row.OfficeKey());
//        if (Elections.IsPrimaryElection(ElectionKey)) offices = FilterUncontestedOffices(offices);
//        return offices.ToList();
//      }

//      public List<IGrouping<string, DataRow>> GetLocalGroups(
//        FilterBy filterBy = null, OrderBy orderBy = null)
//      {
//        return GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
//          .GroupBy(row => row.LocalCode())
//          .ToList();
//      }

//      public DataRow GetRunningMate(string officeKey, string politicianKey)
//      {
//        return DataTable.Rows.OfType<DataRow>()
//          .FirstOrDefault(row => row.OfficeKey()
//            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
//              .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
//      }
//    }

//    private Control GenerateReport(string electionKey, string congress,
//      string stateSenate, string stateHouse, string countyCode, bool useExplorer,
//      out int totalContests)
//    {
//      _ElectionKey = electionKey;
//      _StateElectionKey = Elections.GetStateElectionKeyFromKey(_ElectionKey);
//      _StateCode = Elections.GetStateCodeFromKey(_ElectionKey);
//      _Congress = congress;
//      _StateSenate = stateSenate;
//      _StateHouse = stateHouse;
//      _CountyCode = countyCode;
//      _UseExplorer = useExplorer;

//      _DataManager.GetData(_ElectionKey, congress, stateSenate, stateHouse,
//        _CountyCode);

//      _MainHtmlTable =
//        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");

//      if (Elections.IsStateElection(_ElectionKey))
//        ReportStateContests();

//      if (Elections.IsStateElection(_ElectionKey) ||
//        Elections.IsCountyElection(_ElectionKey))
//        ReportCountyContests();

//      if (Elections.IsLocalElection(_ElectionKey))
//        ReportLocalContestsForOneLocal();
//      else
//        ReportLocalContestsForAllLocals();

//      FillCurrentMainRow();

//      totalContests = _TotalContests;
//      return _MainHtmlTable;
//    }

//    private void StartNewMainRow()
//    {
//      if (_ContestsInRow <= 0) return;
//      _MainTableRow = new HtmlTableRow().AddTo(_MainHtmlTable);
//      _ContestsInRow = 0;
//    }

//    private void FillCurrentMainRow()
//    {
//      if (_ContestsInRow <= 0) return;
//      var cellsToFill = MaxContestsInRow - _ContestsInRow;
//      if (cellsToFill > 0)
//        new HtmlTableCell {InnerHtml = "&nbsp"}.AddTo(_MainTableRow,
//          "EmptyOfficeCell");
//      _ContestsInRow = MaxContestsInRow;
//    }

//    private void CreateOfficesHeading(string headingText)
//    {
//      FillCurrentMainRow();
//      var tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trBallotSeparator");
//      new HtmlTableCell {InnerHtml = headingText, ColSpan = MaxContestsInRow}.AddTo
//        (tr, "tdBallotSeparator");
//      StartNewMainRow();
//    }

//    private void ReportStateContests()
//    {
//      var offices = _DataManager.GetOfficeGroups(new StateFilter());
//      foreach (var office in offices)
//      {
//        if (ReportOneOffice(_StateElectionKey, office))
//          _TotalContests++;
//      }
//    }

//    private void ReportCountyContests()
//    {
//      var offices = _DataManager.GetOfficeGroups(new CountyFilter());
//      if (offices.Count == 0) return;

//      var countyKey = Elections.GetCountyElectionKeyFromKey(_ElectionKey,
//        _StateCode, _CountyCode);
//      var countyName = CountyCache.GetCountyName(_StateCode, _CountyCode);
//      CreateOfficesHeading(countyName);

//      foreach (var office in offices)
//      {
//        if (ReportOneOffice(countyKey, office))
//          _TotalContests++;
//      }
//    }

//    private void ReportLocalContestsForOneLocal()
//    {
//      var localCode = Elections.GetLocalCodeFromKey(_ElectionKey);
//      var offices = _DataManager.GetOfficeGroups(new OneLocalFilter(localCode));
//      if (offices.Count == 0) return;

//      var localInfo = offices.First()
//        .First();
//      var localDistrict = localInfo.LocalDistrict();
//      CreateOfficesHeading(localDistrict);
//      foreach (var office in offices)
//      {
//        if (ReportOneOffice(_ElectionKey, office))
//          _TotalContests++;
//      }
//    }

//    private void ReportLocalContestsForAllLocals()
//    {
//      var locals = _DataManager.GetLocalGroups(new LocalFilter());
//      if (locals.Count == 0) return;

//      foreach (var local in locals)
//      {
//        var localInfo = local.First();
//        var localCode = localInfo.LocalCode();
//        var localDistrict = localInfo.LocalDistrict();
//        var localKey = Elections.GetLocalElectionKeyFromKey(_ElectionKey,
//          _StateCode, _CountyCode, localCode);
//        CreateOfficesHeading(localDistrict);
//        var offices = local.GroupBy(row => row.OfficeKey());
//        foreach (var office in offices)
//        {
//          if (ReportOneOffice(localKey, office))
//            _TotalContests++;
//        }
//      }
//    }

//    private bool ReportOneOffice(string electionKey, IEnumerable<DataRow> office)
//    {
//      var candidates = office.ToList();
//      var officeInfo = candidates.First();
//      var officeKey = officeInfo.OfficeKey();

//      if (!StateCache.GetShowUnopposed(_StateCode))
//      {
//        var unopposed = Elections.IsPrimaryElection(electionKey)
//          ? candidates.Count <= officeInfo.PrimaryPositions()
//          : candidates.Count <= officeInfo.ElectionPositions();
//        if (unopposed) return false;
//      }

//      if (_ContestsInRow >= MaxContestsInRow)
//      {
//        FillCurrentMainRow();
//        StartNewMainRow();
//      }

//      var td = new HtmlTableCell {Align = "center"}.AddTo(_MainTableRow,
//        "tdBallotOfficeContest");
//      var htmlTable = new HtmlTable {CellSpacing = 0}.AddTo(td,
//        "tableBallotOfficeContest");
//      var tr = new HtmlTableRow().AddTo(htmlTable, "trBallotOfficeHeading");
//      td = new HtmlTableCell {Align = "center", ColSpan = 3}.AddTo(tr,
//        "tdBallotOfficeHeading");

//      HtmlAnchor a;
//      var formattedOfficeName = FormatOfficeNameForBallot(officeInfo);
//      if (candidates.Count > 1)
//        a = CreateIssuePageAnchor(electionKey, officeKey, "ALLBio",
//          formattedOfficeName,
//          "Compare candidates' bios and views and positions on the issues",
//          string.Empty);
//      else
//        a = CreatePoliticianIntroAnchor(candidates[0], formattedOfficeName);
//      a.AddTo(td);

//      var voteForWording = officeInfo.VoteForWording()
//        .Trim();
//      if (string.IsNullOrWhiteSpace(voteForWording))
//        voteForWording = "(Vote for not more than one)";
//      new HtmlBreak().AddTo(td);
//      new HtmlSpan {InnerHtml = voteForWording}.AddTo(td,
//        "BallotOfficesVoteFor");

//      if (candidates.Count > 1)
//      {
//        new HtmlBreak().AddTo(td);
//        if (_UseExplorer)
//          CreateCompareViaExplorerAnchor(electionKey, officeKey, _Congress,
//            _StateSenate, _StateHouse, _CountyCode)
//            .AddTo(td);
//        else
//          CreateCompareTheCandidatesAnchorTable(electionKey, officeKey)
//            .AddTo(td);
//      }

//      foreach (var candidate in candidates)
//        ReportOneCandidate(htmlTable, candidate);

//      if (StateCache.GetShowWriteIn(_StateCode))
//      {
//        var writeInWording = officeInfo.WriteInWording()
//          .Trim();
//        if (writeInWording == string.Empty) writeInWording = "Write in";
//        for (var i = 0; i < officeInfo.WriteInLines(); i++)
//        {
//          tr = new HtmlTableRow().AddTo(htmlTable, "trBallotCandidate");
//          new HtmlTableCell
//          {
//            Align = "left",
//            ColSpan = 2,
//            InnerHtml = writeInWording
//          }.AddTo(tr, "tdBallotCandidate");
//          td = new HtmlTableCell {Align = "right"}.AddTo(tr, "tdBallotCheckBox");
//          new HtmlInputCheckBox().AddTo(td);
//        }
//      }

//      _ContestsInRow++;
//      return true;
//    }

//    private static string FormatOfficeNameForBallot(DataRow officeInfo)
//    {
//      var officeKey = officeInfo.OfficeKey();
//      var officeName = Offices.FormatOfficeName(officeInfo, "<br />");
//      if (Offices.IsCountyOffice(officeKey) || Offices.IsLocalOffice(officeKey))
//        officeName += "<br />" +
//          CountyCache.GetCountyName(Offices.GetStateCodeFromKey(officeKey),
//            Offices.GetCountyCodeFromKey(officeKey));
//      if (Offices.IsLocalOffice(officeKey))
//        officeName += "<br />" + officeInfo.LocalDistrict();
//      return officeName;
//    }

//    private void ReportOneCandidate(HtmlTable htmlTable, DataRow politician)
//    {
//      var tr = new HtmlTableRow().AddTo(htmlTable, "trBallotCandidate");
//      var td = new HtmlTableCell().AddTo(tr, "tdBallotCandidate");

//      var politicianKey = politician.PoliticianKey();

//      CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri(politicianKey)
//        .ToString(), politicianKey, ImageSizeReports,
//        Politicians.FormatName(politician) + " Biographical Information")
//        .AddTo(td);

//      DataRow runningMate = null;
//      if (politician.IsRunningMateOffice())
//      {
//        var runningMateKey = politician.RunningMateKey();
//        runningMate = _DataManager.GetRunningMate(politician.OfficeKey(),
//          runningMateKey);
//        if (runningMate != null)
//        {
//          new HtmlBreak(2).AddTo(td);
//          CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri(runningMateKey)
//            .ToString(), runningMateKey, ImageSizeReports,
//            Politicians.FormatName(runningMate) + " Biographical Information")
//            .AddTo(td);
//        }
//      }

//      td = new HtmlTableCell().AddTo(tr, "tdBallotCandidate");

//      var showParty = !Elections.IsPrimaryElection(politician.ElectionKey());
//      FormatNameAndPartyTable(politician, showParty)
//        .AddTo(td);

//      if (StateCache.GetIsIncumbentShownOnBallots(_StateCode) &&
//        politician.IsIncumbent())
//        new LiteralControl("* ").AddTo(td);

//      if (runningMate != null)
//      {
//        new HtmlBreak(2).AddTo(td);
//        FormatNameAndPartyTable(runningMate, false)
//          .AddTo(td);
//      }

//      FormatPoliticianWebsiteTable(politician)
//        .AddTo(td);

//      new HtmlBreak(2).AddTo(td);
//      SocialMedia.GetAnchors(politician)
//        .AddTo(td);

//      td = new HtmlTableCell {Align = "right"}.AddTo(tr, "tdBallotCheckBox");
//      new HtmlInputCheckBox().AddTo(td);
//    }

//    #endregion Private

//    public static Control GetReport(string electionKey, string congress,
//      string stateSenate, string stateHouse, string countyCode, bool useExplorer,
//      out int officeContests)
//    {
//      var reportObject = new BallotReport();
//      return reportObject.GenerateReport(electionKey, congress, stateSenate,
//        stateHouse, countyCode, useExplorer, out officeContests);
//    }
//  }
//}