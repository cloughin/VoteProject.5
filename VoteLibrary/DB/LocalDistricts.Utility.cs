using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class LocalDistricts
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static string GetFullName(string stateCode, string countyCode,
      string localKey)
    {
      return (IsNullOrWhiteSpace(localKey)
        ? Empty
        : GetName(stateCode, localKey) + ", ") +
        Counties.GetFullName(stateCode, countyCode);
    }

    public static string GetName(string stateCode, string localKey)
    {
      return GetName(VotePage.GetPageCache(), stateCode, localKey);
    }

    public static string GetName(PageCache cache, string stateCode, string localKey)
    {
      return cache.LocalDistricts.GetLocalDistrict(stateCode, localKey);
    }

    public static bool IsValidKey(string stateCode, string localKey)
    {
      if (IsNullOrWhiteSpace(stateCode) ||
        IsNullOrWhiteSpace(localKey)) return false;
      return VotePage.GetPageCache()
        .LocalDistricts.Exists(stateCode, localKey);
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}