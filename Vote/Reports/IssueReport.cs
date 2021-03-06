//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal abstract class IssueReport : Report
//  {
//    #region Protected

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable VirtualMemberNeverOverriden.Global
//    // ReSharper disable UnusedMember.Global

//    #endregion ReSharper disable

//    protected const int ImageSize200Profile = 200;

//    protected bool IsRunningMateOffice;
//    protected HtmlTable HtmlTable;
//    //protected IssueReportDataManager DataManager;

//    protected abstract class IssueReportDataManager : ReportDataManager<DataRow>
//    {
//      private sealed class RunningMate : FilterBy
//      {
//        private readonly string _PoliticianKey;

//        public RunningMate(string politicianKey)
//        {
//          _PoliticianKey = politicianKey;
//        }

//        public override bool Filter(DataRow row)
//        {
//          return row.IsRunningMate() && row.PoliticianKey()
//            .IsEqIgnoreCase(_PoliticianKey);
//        }
//      }

//      private sealed class NotRunningMate : FilterBy
//      {
//        public override bool Filter(DataRow row)
//        {
//          return !row.IsRunningMate();
//        }
//      }

//      public abstract void GetData();

//      public IList<IGrouping<string, DataRow>> GetPoliticians()
//      {
//        return GetDataSubset(new NotRunningMate())
//          .GroupBy(row => row.PoliticianKey())
//          .ToList();
//      }

//      internal IGrouping<string, DataRow> GetRunningMate(string runningMateKey)
//      {
//        var runningMate = GetDataSubset(new RunningMate(runningMateKey));
//        return runningMate.Count == 0
//          ? null
//          : runningMate.GroupBy(row => row.PoliticianKey())
//            .First();
//      }
//    }

//    protected Control GenerateReport(string issue, out int answerCount)
//    {
//      // For the bio report, the "groups" will all contain
//      // exactly one DataRow. For the issue report, the groups will
//      // always have at least one DataRow. If there were no answers,
//      // that one row's answer data will be null.

//      DataManager.GetData();
//      var politicians = DataManager.GetPoliticians();
//      answerCount = 0;
//      if (politicians.Count == 0) return new PlaceHolder();
//      IsRunningMateOffice = politicians[0].First()
//        .IsRunningMateOffice();

//      HtmlTable =
//        new HtmlTable {CellSpacing = 0, CellPadding = 0}.AddCssClasses("tablePage");

//      var maxPoliticiansPerRow = IsRunningMateOffice ? 1 : 3;
//      var politicianCount = politicians.Count;
//      var endCandidateRow = politicianCount - 1;
//      var thisStartCandidateRow = 0;
//      var thisEndCandidateRow = Math.Min(endCandidateRow,
//        (thisStartCandidateRow - 1) + maxPoliticiansPerRow);

//      do
//      {
//        answerCount += OneRow(issue, politicians, thisStartCandidateRow,
//          thisEndCandidateRow);
//        thisStartCandidateRow = thisEndCandidateRow + 1;
//        thisEndCandidateRow = Math.Min(endCandidateRow,
//          (thisStartCandidateRow - 1) + maxPoliticiansPerRow);
//      } while (thisStartCandidateRow <= endCandidateRow);

//      return HtmlTable;
//    }

//    protected int OneRow(string issue,
//      IEnumerable<IGrouping<string, DataRow>> politicians, int startCandidateRow,
//      int endCandidateRow)
//    {
//      var politiciansInRow = politicians.Skip(startCandidateRow)
//        .Take(endCandidateRow - startCandidateRow + 1)
//        .ToList();

//      if (IsRunningMateOffice) // append running mate
//      {
//        var runningMateKey = politiciansInRow[0].First()
//          .RunningMateKey();
//        if (string.IsNullOrWhiteSpace(runningMateKey)) runningMateKey = "NoRunningMate";
//        var runningMate = DataManager.GetRunningMate(runningMateKey);
//        politiciansInRow.Add(runningMate);
//      }

//      var tr = new HtmlTableRow().AddTo(HtmlTable, "trIssuePoliticiansHeading");
//      new HtmlTableCell {InnerHtml = issue}.AddTo(tr, "tdIssueHeading");

//      foreach (var politicianGroup in politiciansInRow)
//      {
//        var td = new HtmlTableCell().AddTo(tr, "tdIssuePoliticiansHeading");
//        if (politicianGroup == null)
//          new HtmlSpan {InnerHtml = "No Running Mate Selected"}
//            .AddTo(td, "no-running-mate");
//        else
//        {
//          var politician = politicianGroup.First();
//          var politicianName = Politicians.FormatName(politician);
//          var toolTip = "Biographical Information about " + politicianName;
//          CreatePoliticianIntroAnchor(politician, politicianName, toolTip)
//            .AddTo(td);
//          if (politician.IsIncumbent()) new LiteralControl("* ").AddTo(td);
//          if (!string.IsNullOrWhiteSpace(politician.PartyCode()))
//          {
//            new LiteralControl(" - ").AddTo(td);
//            CreatePartyAnchor(politician)
//              .AddTo(td);
//          }
//          new HtmlBreak(2).AddTo(td);
//          CreateCenteredPoliticianImageAnchor(
//            UrlManager.GetIntroPageUri(politician.PoliticianKey())
//              .ToString(), politician.PoliticianKey(), ImageSize200Profile,
//            politicianName + "'s Introduction Page")
//            .AddTo(td);
//          FormatPoliticianWebsiteTable(politician)
//            .AddTo(td);
//          new HtmlBreak(2).AddTo(td);
//          SocialMedia.GetAnchors(politician)
//            .AddTo(td);
//        }
//      }

//      return ReportDetails(politiciansInRow);
//    }

//    protected abstract int ReportDetails(
//      List<IGrouping<string, DataRow>> politiciansInRow);

//    #region ReSharper restore

//    // ReSharper restore UnusedMember.Global
//    // ReSharper restore VirtualMemberNeverOverriden.Global
//    // ReSharper restore MemberCanBePrivate.Global

//    #endregion ReSharper restore

//    #endregion Protected

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    //public string GetPoliticianNames()
//    //{
//    //  return DataManager.GetPoliticians()
//    //    .Select(g => Politicians.FormatName(g.First()))
//    //    .ToList()
//    //    .JoinText();
//    //}

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