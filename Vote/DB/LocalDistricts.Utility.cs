using Vote;

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
      string localCode)
    {
      return (string.IsNullOrWhiteSpace(localCode)
        ? string.Empty
        : GetName(stateCode, countyCode, localCode) + ", ") +
        Counties.GetFullName(stateCode, countyCode);
    }

    public static string GetFullName(PageCache cache, 
      string stateCode, string countyCode, string localCode)
    {
      return (string.IsNullOrWhiteSpace(localCode)
        ? string.Empty
        : GetName(cache, stateCode, countyCode, localCode) + ", ") +
        Counties.GetFullName(stateCode, countyCode);
    }

    public static string GetName(string stateCode, string countyCode,
      string localCode)
    {
      return VotePage.GetPageCache()
        .LocalDistricts.GetLocalDistrict(stateCode, countyCode, localCode);
    }

    public static string GetName(PageCache cache, string stateCode, 
      string countyCode, string localCode)
    {
      return cache.LocalDistricts.
        GetLocalDistrict(stateCode, countyCode, localCode);
    }

    public static bool IsValid(string stateCode, string countyCode,
      string localCode)
    {
      if (string.IsNullOrWhiteSpace(stateCode) ||
        string.IsNullOrWhiteSpace(countyCode) ||
        string.IsNullOrWhiteSpace(localCode)) return false;
      return VotePage.GetPageCache()
        .LocalDistricts.Exists(stateCode, countyCode, localCode);
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}