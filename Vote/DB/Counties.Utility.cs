using System.Collections.ObjectModel;
using Vote;

namespace DB.Vote
{
  public partial class Counties
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static ReadOnlyCollection<string> GetCountiesByState(string stateCode) => 
      CountyCache.GetCountiesByState(stateCode);

    public static string GetFullName(string stateCode, string countyCode) => 
      (string.IsNullOrWhiteSpace(countyCode) || (countyCode == "000")
      ? string.Empty
      : GetName(stateCode, countyCode) + ", ") + States.GetName(stateCode);

    public static string GetName(string stateCode, string countyCode) => 
      CountyCache.GetCountyName(stateCode, countyCode);

    public static bool IsValid(string stateCode, string countyCode) => 
      CountyCache.CountyExists(stateCode, countyCode);

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}