//using System.Collections.Generic;
//using System.Data;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal class BallotReport2Classic : BallotReport2
//  {
//    #region Private

//    private const int MaxContestsInRow = 3;
//    private const int ImageSizeReports = 100;

//    private HtmlTable _MainHtmlTable;
//    private HtmlTableRow _MainTableRow;
//    private int _ContestsInRow = int.MaxValue; // force new row on first contest

//    private HtmlTable _OfficeContestTable;

//    private void CreateOfficesHeading(string headingText)
//    {
//      FillCurrentMainRow();
//      var tr = new HtmlTableRow().AddTo(_MainHtmlTable, "trBallotSeparator");
//      new HtmlTableCell { InnerHtml = headingText, ColSpan = MaxContestsInRow }.AddTo
//        (tr, "tdBallotSeparator");
//      StartNewMainRow();
//    }

//    private void FillCurrentMainRow()
//    {
//      if (_ContestsInRow <= 0) return;
//      var cellsToFill = MaxContestsInRow - _ContestsInRow;
//      if (cellsToFill > 0)
//        new HtmlTableCell { InnerHtml = "&nbsp" }.AddTo(_MainTableRow,
//          "EmptyOfficeCell");
//      _ContestsInRow = MaxContestsInRow;
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

//    private void StartNewMainRow()
//    {
//      if (_ContestsInRow <= 0) return;
//      _MainTableRow = new HtmlTableRow().AddTo(_MainHtmlTable);
//      _ContestsInRow = 0;
//    }

//    #endregion Private

//    #region Event handlers and overrides

//    protected override void OnBeginCounty(string electionKey, string countyName, int officeCount)
//    {
//      CreateOfficesHeading(countyName);
//    }

//    protected override void OnBeginLocal(string electionKey, string localCode, string localName, int officeCount)
//    {
//      CreateOfficesHeading(localName);
//    }

//    protected override void OnBeginLocals(int localCount)
//    {
//    }

//    protected override void OnBeginOffice(string electionKey, DataRow officeInfo, IList<string> candidateKeys, IList<string> runningMateKeys)
//    {
//      var officeKey = officeInfo.OfficeKey();

//      if (_ContestsInRow >= MaxContestsInRow)
//      {
//        FillCurrentMainRow();
//        StartNewMainRow();
//      }

//      var td = new HtmlTableCell { Align = "center" }.AddTo(_MainTableRow,
//        "tdBallotOfficeContest");
//      _OfficeContestTable = new HtmlTable { CellSpacing = 0 }.AddTo(td,
//        "tableBallotOfficeContest");
//      var tr = new HtmlTableRow().AddTo(_OfficeContestTable, "trBallotOfficeHeading");
//      td = new HtmlTableCell { Align = "center", ColSpan = 3 }.AddTo(tr,
//        "tdBallotOfficeHeading");

//      HtmlAnchor a;
//      var formattedOfficeName = FormatOfficeNameForBallot(officeInfo);
//      if (candidateKeys.Count > 1)
//        a = CreateIssuePageAnchor(electionKey, officeKey, "ALLBio",
//          formattedOfficeName,
//          "Compare candidates' bios and views and positions on the issues",
//          string.Empty);
//      else
//        a = CreatePoliticianIntroAnchor(officeInfo, formattedOfficeName);
//      a.AddTo(td);

//      var voteForWording = officeInfo.VoteForWording()
//        .Trim();
//      if (string.IsNullOrWhiteSpace(voteForWording))
//        voteForWording = "(Vote for not more than one)";
//      new HtmlBreak().AddTo(td);
//      new HtmlSpan { InnerHtml = voteForWording }.AddTo(td,
//        "BallotOfficesVoteFor");

//      if (candidateKeys.Count > 1)
//      {
//        new HtmlBreak().AddTo(td);
//        CreateCompareTheCandidatesAnchorTable(electionKey, officeKey)
//          .AddTo(td);
//      }
//    }

//    protected override void OnBeginReport()
//    {
//      _MainHtmlTable =
//        new HtmlTable { CellSpacing = 0, CellPadding = 0 }.AddCssClasses("tablePage");
//    }

//    protected override void OnBeginState(string electionKey, string stateName, int officeCount)
//    {
//    }

//    protected override void OnEndCounty(string electionKey)
//    {
//    }

//    protected override void OnEndLocal(string electionKey)
//    {
//    }

//    protected override void OnEndLocals()
//    {
//    }

//    protected override void OnEndOffice(string electionKey, DataRow officeInfo)
//    {
//      var writeInWording = officeInfo.WriteInWording()
//        .Trim();
//      if (writeInWording == string.Empty)
//        writeInWording = "Write in";
//      for (var i = 0; i < officeInfo.WriteInLines(); i++)
//      {
//        var tr = new HtmlTableRow().AddTo(_OfficeContestTable, "trBallotCandidate");
//        new HtmlTableCell { Align = "left", ColSpan = 2, InnerHtml = writeInWording }
//          .AddTo(tr, "tdBallotCandidate");
//        var td = new HtmlTableCell { Align = "right" }.AddTo(tr, "tdBallotCheckBox");
//        new HtmlInputCheckBox().AddTo(td);
//      }

//      _ContestsInRow++;
//    }

//    protected override Control OnEndReport(out int ballotMeasures)
//    {
//      ballotMeasures = 0;
//      FillCurrentMainRow();
//      return _MainHtmlTable;
//    }

//    protected override void OnEndState(string electionKey)
//    {
//    }

//    protected override void OnReportCandidate(DataRow candidate)
//    {
//      var tr = new HtmlTableRow().AddTo(_OfficeContestTable, "trBallotCandidate");
//      var td = new HtmlTableCell().AddTo(tr, "tdBallotCandidate");

//      var politicianKey = candidate.PoliticianKey();

//      CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri(politicianKey)
//        .ToString(), politicianKey, ImageSizeReports,
//        Politicians.FormatName(candidate) + " Biographical Information")
//        .AddTo(td);

//      DataRow runningMate = null;
//      if (candidate.IsRunningMateOffice())
//      {
//        var runningMateKey = candidate.RunningMateKey();
//        runningMate = GetRunningMate(candidate.OfficeKey(), runningMateKey);
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

//      var showParty = !Elections.IsPrimaryElection(candidate.ElectionKey());
//      FormatNameAndPartyTable(candidate, showParty)
//        .AddTo(td);

//      if (StateCache.GetIsIncumbentShownOnBallots(StateCode) &&
//        candidate.IsIncumbent())
//        new LiteralControl("* ").AddTo(td);

//      if (runningMate != null)
//      {
//        new HtmlBreak(2).AddTo(td);
//        FormatNameAndPartyTable(runningMate, false)
//          .AddTo(td);
//      }

//      FormatPoliticianWebsiteTable(candidate)
//        .AddTo(td);

//      new HtmlBreak(2).AddTo(td);
//      SocialMedia.GetAnchors(candidate)
//        .AddTo(td);

//      td = new HtmlTableCell { Align = "right" }.AddTo(tr, "tdBallotCheckBox");
//      new HtmlInputCheckBox().AddTo(td);
//    }

//    #endregion Event handlers and overrides

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    public static Control GetReport(string electionKey, string congress,
//      string stateSenate, string stateHouse, string countyCode,
//      out int officeContests)
//    {
//      var reportObject = new BallotReport2Classic();
//      int ballotMeasures;
//      return reportObject.GenerateReport(electionKey, congress, stateSenate,
//        stateHouse, countyCode, out officeContests, out ballotMeasures);
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