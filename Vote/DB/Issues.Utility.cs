using System;
using Vote;

namespace DB.Vote
{
  public partial class Issues
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static string GetIssueDescription(string issueKey)
    {
      if (IsIssuesListKey(issueKey))

        return "List of Issues";

      return IsBiographicalKey(issueKey)
        ? "Biographical Information"
        : GetIssue(issueKey);
    }

    public static string GetIssueLevelFromKey(string issueKey)
    {
      return issueKey.Length > 0
        ? issueKey.Substring(0, 1).ToUpper()
        : string.Empty;
    }

    public static string GetStateCodeFromKey(string issueKey)
    {
      if ((issueKey == null) || (issueKey.Length < 3))
        return string.Empty;
      return issueKey.Substring(1, 2)
        .ToUpperInvariant();
    }

    public static string GetValidatedStateCodeFromKey(string issueKey)
    {
      if ((issueKey == null) || (issueKey.Length < 3)) return string.Empty;
      var stateCode = issueKey.Substring(1, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : string.Empty;
    }

    public static bool IsBiographicalKey(string issueKey)
    {
      return issueKey.IsEqIgnoreCase("allbio");
    }

    public static bool IsIssuesListKey(string issueKey)
    {
      return issueKey.EndsWith("issueslist",
        StringComparison.OrdinalIgnoreCase);
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