using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal static class IssuesListLinks
  {
    #region Private

    private static HtmlAnchor CreateIssueAnchor(string electionKey,
      string officeKey, string issueKey, string anchorText, string title,
      string target = "")
    {
      var a = new HtmlAnchor
        {
          HRef =
            UrlManager.GetCompareCandidatesPageUri(Offices.GetStateCodeFromKey(officeKey),
              electionKey, officeKey)
              .ToString(),
          Title = title,
          Target = target,
          InnerHtml = anchorText
        };

      return a;
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control GetReport(string electionKey, string officeKey)
    {
      var dataTable = Issues.GetIssuePageIssues(electionKey, officeKey);
      var placeHolder = new PlaceHolder();
      foreach (DataRow row in dataTable.Rows)
      {
        var issueKey = row.IssueKey();
        var issue = row.Issue();
        string title;
        if (issueKey.EndsWith("IssuesList", StringComparison.Ordinal))
          title =
            "Complete list of issues and issue questions available for candidates' responses.";
        else if (issueKey == "ALLBio")
          title = "Biographical comparison of candidates";
        else
          title =
            string.Format(
              "Comparisons of positions and views of the candidates on {0}", issue);
        if (placeHolder.Controls.Count > 0)
          new Literal {Text = " | "}.AddTo(placeHolder);
        CreateIssueAnchor(electionKey, officeKey, issueKey, issue, title)
          .AddTo(placeHolder);
      }

      return placeHolder;
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