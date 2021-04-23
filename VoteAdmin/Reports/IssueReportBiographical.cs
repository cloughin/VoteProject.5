//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal sealed class IssueReportBiographical : IssueReport
//  {
//    #region Private

//    private sealed class IssueReportBiographicalDataManager :
//      IssueReportDataManager
//    {
//      private readonly string _ElectionKey;
//      private readonly string _OfficeKey;

//      public IssueReportBiographicalDataManager(string electionKey,
//        string officeKey)
//      {
//        _ElectionKey = electionKey;
//        _OfficeKey = officeKey;
//      }

//      public override void GetData()
//      {
//        DataTable = Issues.GetIssuePageBioData(_ElectionKey, _OfficeKey);
//      }
//    }

//    //private void OneBioAnswer(string heading,
//    //  IEnumerable<IGrouping<string, DataRow>> politiciansInRow, string columnName)
//    //{
//    //  var tr = new HtmlTableRow().AddTo(HtmlTable, "trIssueQuestion");
//    //  new HtmlTableCell { InnerHtml = heading }.AddTo(tr, "tdIssueIssueQuestion");

//    //  foreach (var politicianGroup in politiciansInRow)
//    //  {
//    //    var td = new HtmlTableCell().AddTo(tr, "tdIssueAnswer");

//    //    var answer = string.Empty;
//    //    if (politicianGroup != null)
//    //      answer = (politicianGroup.First()[columnName] as string).SafeString();
//    //    answer = TruncateAnswer(answer);
//    //    if (string.IsNullOrWhiteSpace(answer))
//    //      new HtmlNbsp().AddTo(td);
//    //    else
//    //    {
//    //      Debug.Assert(politicianGroup != null, "politicianGroup != null");
//    //      if (IsRunningMateOffice)
//    //        PrependName(politicianGroup.First()
//    //          .LastName(), td);
//    //      new LiteralControl(answer).AddTo(td);
//    //    }
//    //  }
//    //}

//    private void OneBioAnswer(string heading,
//      IEnumerable<IGrouping<string, DataRow>> politiciansInRow, string columnName)
//    {
//      var tr = new HtmlTableRow().AddTo(HtmlTable, "trIssueQuestion");
//      new HtmlTableCell {InnerHtml = heading}.AddTo(tr, "tdIssueIssueQuestion");

//      foreach (var politicianGroup in politiciansInRow)
//      {
//        var td = new HtmlTableCell().AddTo(tr, "tdIssueAnswer");

//        var answer = string.Empty;
//        if (politicianGroup != null)
//          answer = (politicianGroup.First()[columnName] as string).SafeString();
//        answer = TruncateAnswer(answer);
//        if (string.IsNullOrWhiteSpace(answer))
//          new HtmlNbsp().AddTo(td);
//        else
//        {
//          var div = new HtmlDiv().AddTo(td);
//          Debug.Assert(politicianGroup != null, "politicianGroup != null");
//          if (IsRunningMateOffice)
//            //PrependName(politicianGroup.First()
//            //  .LastName(), div);
//            answer = PrependName(politicianGroup.First()
//              .LastName(), answer);
//          new LiteralControl(answer.ReplaceNewLinesWithParagraphs()).AddTo(div);
//        }
//      }
//    }

//    #endregion Private

//    #region Protected

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable VirtualMemberNeverOverriden.Global
//    // ReSharper disable UnusedMember.Global

//    #endregion ReSharper disable

//    protected override int ReportDetails(
//      List<IGrouping<string, DataRow>> politiciansInRow)
//    {
//      var tr = new HtmlTableRow().AddTo(HtmlTable, "trIssueQuestion");
//      new HtmlTableCell {InnerHtml = "Age:"}.AddTo(tr, "tdIssueIssueQuestion");

//      foreach (var politicianGroup in politiciansInRow)
//      {
//        var age = String.Empty;
//        if (politicianGroup != null)
//          age = politicianGroup.First()
//            .Age();
//        if (string.IsNullOrWhiteSpace(age))
//          age = "&nbsp";
//        new HtmlTableCell {InnerHtml = age}.AddTo(tr, "tdIssueAnswer");
//      }

//      OneBioAnswer("General: (goals, objectives, views, philosophies)",
//        politiciansInRow, "GeneralStatement");

//      OneBioAnswer(
//        "Personal: (gender, age, marital status, spouse, children, residence)",
//        politiciansInRow, "Personal");

//      OneBioAnswer("Education: (schools, colleges, major, degrees)",
//        politiciansInRow, "Education");

//      OneBioAnswer("Profession: (profession,work experience outside politics)",
//        politiciansInRow, "Profession");

//      OneBioAnswer(
//        "Military: (branch, service, active duty, rank, honors, discharge)",
//        politiciansInRow, "Military");

//      OneBioAnswer("Civic: (organizations, charities)", politiciansInRow, "Civic");

//      OneBioAnswer("Political: (offices held)", politiciansInRow, "Political");

//      OneBioAnswer("Religion: (religious affiliations and beliefs)",
//        politiciansInRow, "Religion");

//      OneBioAnswer("Accomplishments: (awards, achievements)", politiciansInRow,
//        "Accomplishments");

//      return 0;
//    }

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

//    public Control GenerateReport(string electionKey, string officeKey)
//    {
//      int notUsed;
//      DataManager = new IssueReportBiographicalDataManager(electionKey, officeKey);
//      return GenerateReport("Biographical", out notUsed);
//    }

//    public static Control GetReport(string electionKey, string officeKey)
//    {
//      var reportObject = new IssueReportBiographical();
//      return reportObject.GenerateReport(electionKey, officeKey);
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