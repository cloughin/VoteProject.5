//using System;
//using System.Data;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using DB;
//using DB.Vote;

//namespace Vote.Reports
//{
//  internal static class PoliticianIssueListLinks
//    // ReSharper restore UnusedMember.Global
//  {
//    #region Private

//    private static HtmlAnchor CreatePoliticianIssueAnchor(string politicianKey,
//      string issueKey, string anchorText, string title)
//    {
//      var a = new HtmlAnchor
//        {
//          HRef =
//            UrlManager.GetPoliticianIssuePageUri(
//              Politicians.GetStateCodeFromKey(politicianKey), politicianKey, issueKey)
//              .ToString(),
//          Title = title,
//          InnerHtml = anchorText
//        };

//      return a;
//    }

//    private static HtmlAnchor CreatePoliticianIntroAnchor(string politicianKey,
//      string anchorText, string title)
//    {
//      return new HtmlAnchor
//        {
//          HRef = UrlManager.GetIntroPageUri(politicianKey)
//            .ToString(),
//          Title = title,
//          InnerHtml = anchorText
//        };
//    }

//    #endregion Private

//    #region Public

//    #region ReSharper disable

//    // ReSharper disable MemberCanBePrivate.Global
//    // ReSharper disable MemberCanBeProtected.Global
//    // ReSharper disable UnusedMember.Global
//    // ReSharper disable UnusedMethodReturnValue.Global
//    // ReSharper disable UnusedAutoPropertyAccessor.Global
//    // ReSharper disable UnassignedField.Global

//    #endregion ReSharper disable

//    public static Control GetReport(string politicianKey,
//      string politicianName = null)
//    {
//      if (politicianName == null)
//        politicianName = VotePage.GetPageCache()
//          .Politicians.GetPoliticianName(politicianKey);
//      var dataTable = Issues.GetPoliticianIssuePageIssues(politicianKey);
//      var placeHolder = new PlaceHolder();
//      foreach (DataRow row in dataTable.Rows)
//      {
//        var issueKey = row.IssueKey();
//        var issue = row.Issue();
//        if (placeHolder.Controls.Count > 0)
//          new Literal {Text = " | "}.AddTo(placeHolder);
//        string title;
//        if (issueKey.EndsWith("IssuesList", StringComparison.Ordinal))
//        {
//          title =
//            string.Format(
//              "Complete list of issues and topics that {0} could respond to",
//              politicianName);
//          CreatePoliticianIssueAnchor(politicianKey, issueKey, issue, title)
//            .AddTo(placeHolder);
//        }
//        else if (issueKey == "ALLBio")
//        {
//          title =
//            string.Format("Introduction page with {0}'s biographical information",
//              politicianName);
//          CreatePoliticianIntroAnchor(politicianKey, "Biographical", title)
//            .AddTo(placeHolder);
//        }
//        else
//        {
//          title = string.Format("{0}'s positions and views on {1}", politicianName,
//            issue);
//          CreatePoliticianIssueAnchor(politicianKey, issueKey, issue, title)
//            .AddTo(placeHolder);
//        }
//      }

//      return placeHolder;
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