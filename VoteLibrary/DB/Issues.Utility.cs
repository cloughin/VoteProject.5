using System;
using Vote;
using static System.String;

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

    public const string IssueLevelAll = "A";
    public const string IssueLevelNational = "B";
    public const string IssueLevelState = "C";
    public const string IssueLevelCounty = "D";
    public const string IssueLevelLocal = "E";

    public enum IssueId
    {
      Biographical = 1,
      Reasons = 2
    }

    public enum QuestionId
    {
      GeneralPhilosophy = 1,
      PersonalAndFamily = 2,
      ProfessionalExperience = 3,
      CivicInvolvement = 4,
      PoliticalExperience = 5,
      ReligiousAffiliation = 6,
      AccomplishmentsAndAwards = 7,
      EducationalBackground = 8,
      MilitaryService = 9,
      WhyIAmRunning = 11,
      GoalsIfElected = 12,
      AchievementsIfElected = 13,
      AreasToConcentrateOn = 14,
      OnEnteringPublicService = 15,
      OpinionsOfOtherCandidates = 16
    }

    //public static string GetIssueDescription(string issueKey)
    //{
    //  if (IsIssuesListKey(issueKey))

    //    return "List of Issues";

    //  return IsBiographicalKey(issueKey)
    //    ? "Biographical Information"
    //    : GetIssue(issueKey);
    //}

    public static string GetIssueLevelFromKey(string issueKey)
    {
      return issueKey.Length > 0
        ? issueKey.Substring(0, 1).ToUpper()
        : Empty;
    }

    public static string GetStateCodeFromKey(string issueKey)
    {
      if (issueKey == null || issueKey.Length < 3)
        return Empty;
      return issueKey.Substring(1, 2)
        .ToUpperInvariant();
    }

    public static string GetValidatedStateCodeFromKey(string issueKey)
    {
      if (issueKey == null || issueKey.Length < 3) return Empty;
      var stateCode = issueKey.Substring(1, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : Empty;
    }

    //public static bool IsBiographicalKey(string issueKey)
    //{
    //  return issueKey.IsEqIgnoreCase("allbio");
    //}

    public static bool IsIssuesListKey(string issueKey)
    {
      return issueKey.EndsWith("issueslist",
        StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsValidIssue(string issueKey)
    {
      if (IsNullOrEmpty(issueKey)) return false;
      if (issueKey.Length == 13 &&
        issueKey.Substring(3, 10).ToUpper() == "ISSUESLIST")
        return true;
      if (issueKey.Length == 6 &&
        issueKey.Substring(3, 3).ToUpper() == "BIO")
        return true;
      return IssueKeyExists(issueKey);
      //return G.Rows("Issues", "IssueKey", issueKey) == 1;
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