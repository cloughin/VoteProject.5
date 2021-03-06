using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;

namespace DB.Vote
{
  public partial class Elections
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable ReturnTypeCanBeEnumerable.Global

    public const int ElectionKeyLengthStateOrFederal = 12;
    public const int ElectionKeyLengthCounty = 15;
    public const int ElectionKeyLengthLocal = 17;

    public const int MinimumElectionYear = 2000;
    public const int MaximumElectionYear = 2030;

    #region Election types

    public const string ElectionTypeUSPresidentialPrimary = "A";
    public const string ElectionTypeStatePresidentialPrimary = "B";
    //public const string ElectionTypeNationalPresidentialPrimary = "C";
    public const string ElectionTypeGeneralElection = "G";
    public const string ElectionTypeOffYear = "O";
    public const string ElectionTypeStatePrimary = "P";
    public const string ElectionTypeStatePrimaryRunoff = "Q";
    public const string ElectionTypeStateGeneralRunoff = "R";
    public const string ElectionTypeSpecial = "S";

    private class ElectionTypeInfo
    {
      public string ElectionTypeCode;
      public string Description;
      public bool IsValidForState;
    }

    private static readonly ElectionTypeInfo[] ElectionTypeInfos =
    {
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeGeneralElection,
        Description = "General Election",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeOffYear,
        Description = "Off-Year Election",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeSpecial,
        Description = "Special Election",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeStatePrimary,
        Description = "State Primary",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeStatePrimaryRunoff,
        Description = "State Primary Runoff",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeStateGeneralRunoff,
        Description = "State General Runoff",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeStatePresidentialPrimary,
        Description = "Presidential Primary",
        IsValidForState = true
      },
      new ElectionTypeInfo
      {
        ElectionTypeCode = ElectionTypeUSPresidentialPrimary,
        Description = "Remaining Presidential Party Primary Candidates",
        IsValidForState = false
      }
    };

    private static ElectionTypeInfo GetElectionTypeInfo(string electionType)
    {
      return
        ElectionTypeInfos.FirstOrDefault(
          i => i.ElectionTypeCode.IsEqIgnoreCase(electionType));
    }

    public static string GetElectionTypeDescription(string electionType,
      string stateCode = null)
    {
      if (stateCode == "PP")
        return "Template of Major Presidential Party Primary Candidates";
      if ((stateCode == "US") || (electionType == "G"))
        return "Presidential Candidates";
      var info = GetElectionTypeInfo(electionType);
      var result = info == null ? "Unknown Election Type" : info.Description;
      return result;
    }

    public static IEnumerable<KeyValuePair<string, string>> GetElectionTypes(
      string stateCode)
    {
      if (StateCache.IsValidStateCode(stateCode))
        foreach (var info in ElectionTypeInfos.Where(info => info.IsValidForState))
          yield return
            new KeyValuePair<string, string>(info.ElectionTypeCode,
              info.Description);
      else if (StateCache.IsValidFederalCode(stateCode, includeUS: false))
        yield return
          new KeyValuePair<string, string>(ElectionTypeGeneralElection,
            GetElectionTypeDescription(ElectionTypeGeneralElection));
      else // US or PP
        yield return
          new KeyValuePair<string, string>(ElectionTypeUSPresidentialPrimary,
            GetElectionTypeDescription(ElectionTypeUSPresidentialPrimary, stateCode));
    }

    public static void PopulateElectionTypes(DropDownList dropDownList,
      string stateCode, string selectedValue = null)
    {
      dropDownList.Items.Clear();

      foreach (var kvp in GetElectionTypes(stateCode))
        dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
    }

    public static void PopulateElectionTypes(HtmlSelect dropDownList,
      string stateCode, string selectedValue = null)
    {
      dropDownList.Items.Clear();

      foreach (var kvp in GetElectionTypes(stateCode))
        dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
    }

    #endregion Election types

    public static string FormatElectionDescription(string electionKey)
    {
      var stateCode = GetValidatedStateCodeFromKey(electionKey);
      var electionDate = GetElectionDateFromKey(electionKey)
        .ToString("MMMM d, yyyy");
      var electionType = GetElectionTypeFromKey(electionKey);
      var partyCode = GetNationalPartyCodeFromKey(electionKey);

      var stateName = string.Empty;
      if (!string.IsNullOrWhiteSpace(stateCode))
        stateName = " " + StateCache.GetStateName(stateCode);

      var partyName = string.Empty;
      if ( /*electionType != ElectionTypeUSPresidentialPrimary && */
        partyCode != Parties.NationalPartyAll)
      {
        partyName = Parties.GetNationalPartyDescription(partyCode, "?");
        if (partyName == "?")
        {
          stateName = string.Empty;
          partyName = Parties.GetPartyName(stateCode + partyCode, "Unknown Major Party");
        }
      }
      if (!string.IsNullOrWhiteSpace(partyName))
        partyName = " " + partyName;

      var typeDescription = GetElectionTypeDescription(electionType,
        GetStateCodeFromKey(electionKey));

      return electionDate + stateName + partyName + " " + typeDescription;
    }

    public static string FormatElectionKey(DateTime electionDate,
      string electionTypeCode, string nationalPartyCode, string stateCode,
      string countyCode = "", string localCode = "")
    {
      countyCode = countyCode.SafeString();
      localCode = localCode.SafeString();
      if ((electionDate.Year < MinimumElectionYear) ||
        (electionDate.Year > MaximumElectionYear))
        throw new VoteException("FormatElectionKey: invalid electionDate");
      if (electionTypeCode.SafeString().Length != 1)
        throw new VoteException("FormatElectionKey: invalid electionTypeCode");
      if (nationalPartyCode.SafeString().Length != 1)
        throw new VoteException("FormatElectionKey: invalid nationalPartyCode");
      var isState = StateCache.IsValidStateCode(stateCode);
      if (!isState && !StateCache.IsValidFederalCode(stateCode) &&
        !stateCode.IsEqIgnoreCase("PP"))
        throw new VoteException("FormatElectionKey: invalid stateCode");
      if (isState)
      {
        if (countyCode.Length == 3)
        {
          if ((localCode.Length != 2) && (localCode.Length != 0))
            throw new VoteException("FormatElectionKey: invalid localCode");
        }
        else if (countyCode.Length != 0)
          throw new VoteException("FormatElectionKey: invalid countyCode");
      }
      else
      {
        if (countyCode.Length != 0)
          throw new VoteException("FormatElectionKey: invalid countyCode");
        if (localCode.Length != 0)
          throw new VoteException("FormatElectionKey: invalid localCode");
      }
      return
      (stateCode + electionDate.ToString("yyyyMMdd") + electionTypeCode +
        nationalPartyCode + countyCode + localCode).ToUpperInvariant();
    }

    public static string GetCountyCodeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < ElectionKeyLengthCounty))
        return string.Empty;
      return electionKey.Substring(ElectionKeyLengthStateOrFederal, 3)
        .ToUpperInvariant();
    }

    public static string GetCountyElectionKeyFromKey(string electionKey)
    {
      return electionKey.Length < ElectionKeyLengthCounty
        ? string.Empty
        : electionKey.Substring(0, ElectionKeyLengthCounty);
    }

    public static string GetCountyElectionKeyFromKey(string electionKey,
      string stateCode, string countyCode)
    {
      var stateKey = GetStateElectionKeyFromKey(electionKey, stateCode);
      if (string.IsNullOrWhiteSpace(stateKey) ||
        string.IsNullOrWhiteSpace(countyCode) || (countyCode.Length != 3))
        return string.Empty;
      return stateKey + countyCode;
    }

    public static string GetDefaultElectionKeyFromKey(string electionKey)
    {
      return string.IsNullOrWhiteSpace(electionKey) || (electionKey.Length < 10)
        ? string.Empty
        : electionKey.Substring(0, 10);
    }

    public static DateTime GetElectionDateFromKey(string electionKey)
    {
      return new DateTime(int.Parse(electionKey.Substring(2, 4)),
        int.Parse(electionKey.Substring(6, 2)),
        int.Parse(electionKey.Substring(8, 2)));
    }

    public static string GetElectionDateStringFromKey(string electionKey)
    {
      return electionKey.Substring(2, 8);
    }

    public static string GetElectionTypeForRunoff(string runoffType)
    {
      switch (runoffType)
      {
        case ElectionTypeStatePrimaryRunoff:
          return ElectionTypeStatePrimary;

        case ElectionTypeStateGeneralRunoff:
          return ElectionTypeGeneralElection;

        default:
          return string.Empty;
      }
    }

    public static string GetElectionTypeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < 12))
        return string.Empty;
      return electionKey.Substring(10, 1)
        .ToUpperInvariant();
    }

    public static ElectoralClass GetElectoralClass(string electionKey)
    {
      var stateCode = GetStateCodeFromKey(electionKey);
      var countyCode = GetCountyCodeFromKey(electionKey);
      var localCode = GetLocalCodeFromKey(electionKey);
      return Offices.GetElectoralClass(stateCode, countyCode, localCode);
    }

    public static string GetElectoralClassDescription(string electionKey)
    {
      var stateCode = GetStateCodeFromKey(electionKey);
      var countyCode = GetCountyCodeFromKey(electionKey);
      var localCode = GetLocalCodeFromKey(electionKey);
      return Offices.GetElectoralClassDescription(stateCode, countyCode, localCode);
    }

    public static string GetFederalElectionKeyFromKey(string electionKey,
      string federalCode)
    {
      if ((electionKey == null) || (electionKey.Length < ElectionKeyLengthStateOrFederal))
        throw new ArgumentException("Missing or invalid electionKey");
      if ((federalCode == null) || (federalCode.Length != 2))
        return string.Empty;
      return federalCode +
        electionKey.Substring(2, ElectionKeyLengthStateOrFederal - 2);
    }

    public static string GetLocalCodeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < ElectionKeyLengthLocal))
        return string.Empty;
      return electionKey.Substring(ElectionKeyLengthCounty, 2)
        .ToUpperInvariant();
    }

    public static string GetLocalElectionKeyFromKey(string electionKey)
    {
      return electionKey.Length != ElectionKeyLengthLocal
        ? string.Empty
        : electionKey;
    }

    public static string GetLocalElectionKeyFromKey(string electionKey,
      string stateCode, string countyCode, string localCode)
    {
      var countyKey = GetCountyElectionKeyFromKey(electionKey, stateCode,
        countyCode);
      if (string.IsNullOrWhiteSpace(countyKey) ||
        string.IsNullOrWhiteSpace(localCode) || (localCode.Length != 2))
        return string.Empty;
      return countyKey + localCode;
    }

    public static string GetNationalPartyCodeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < 12))
        return string.Empty;
      return electionKey.Substring(11, 1)
        .ToUpperInvariant();
    }

    public static IEnumerable<string> GetQueryParametersFromKey(string electionKey)
    {
      var list = new List<string> {{"state", GetStateCodeFromKey(electionKey)}};
      var countyCode = GetCountyCodeFromKey(electionKey);
      if (!string.IsNullOrWhiteSpace(countyCode))
      {
        list.Add("county", countyCode);
        var localCode = GetLocalCodeFromKey(electionKey);
        if (!string.IsNullOrWhiteSpace(localCode))
          list.Add("local", localCode);
      }
      return list;
    }

    public static string GetStateCodeFromKey(string electionKey)
    {
      if ((electionKey == null) ||
        (electionKey.Length < ElectionKeyLengthStateOrFederal))
        return string.Empty;
      return electionKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    public static string GetStateElectionKeyFromKey(string electionKey)
    {
      if ((electionKey == null) ||
        (electionKey.Length < ElectionKeyLengthStateOrFederal))
        return string.Empty;
      return electionKey.Substring(0, ElectionKeyLengthStateOrFederal)
        .ToUpperInvariant();
    }

    public static string GetStateElectionKeyFromKey(string electionKey,
      string stateCode)
    {
      if ((electionKey == null) || (stateCode == null) ||
        (electionKey.Length < ElectionKeyLengthStateOrFederal) ||
        (stateCode.Length != 2))
        return string.Empty;
      return
        (stateCode + electionKey.Substring(2, ElectionKeyLengthStateOrFederal - 2))
          .ToUpperInvariant();
    }

    public static string GetValidatedFederalCodeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < 2)) return string.Empty;
      var stateCode = electionKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidFederalCode(stateCode) ? stateCode : string.Empty;
    }

    public static string GetValidatedStateCodeFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < 2)) return string.Empty;
      var stateCode = electionKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : string.Empty;
    }

    public static string GetYyyyMmDdFromKey(string electionKey)
    {
      if ((electionKey == null) || (electionKey.Length < 10)) return string.Empty;
      return electionKey.Substring(2, 8);
    }

    public static string InsertStateCodeIntoElectionKey(string electionKey,
      string stateCode)
    {
      if ((electionKey.Length >= 12) && (stateCode.Length == 2))
        return stateCode + electionKey.Substring(2, 10);
      return string.Empty;
    }

    public static bool IsCountyElection(string electionKey)
    {
      return (electionKey.Length == ElectionKeyLengthCounty) &&
        (GetCountyCodeFromKey(electionKey) != "000");
    }

    public static bool IsGeneralElection(string electionKey)
    {
      return GetElectionTypeFromKey(electionKey) == ElectionTypeGeneralElection;
    }

    public static bool IsLocalElection(string electionKey)
    {
      return (electionKey.Length == ElectionKeyLengthLocal) &&
        (GetLocalCodeFromKey(electionKey) != "00");
    }

    public static bool IsPrimaryElection(string electionKey)
    {
      return IsPrimaryElectionType(GetElectionTypeFromKey(electionKey));
    }

    public static bool IsPrimaryElectionType(string electionType)
    {
      switch (electionType)
      {
        case ElectionTypeUSPresidentialPrimary:
        case ElectionTypeStatePresidentialPrimary:
        //case ElectionTypeNationalPresidentialPrimary:
        case ElectionTypeStatePrimary:
        case ElectionTypeStatePrimaryRunoff:
          return true;

        default:
          return false;
      }
    }

    public static bool IsRunoffElection(string electionKey)
    {
      return IsRunoffElectionType(GetElectionTypeFromKey(electionKey));
    }

    public static bool IsRunoffElectionType(string electionType)
    {
      switch (electionType)
      {
        case ElectionTypeStatePrimaryRunoff:
        case ElectionTypeStateGeneralRunoff:
          return true;

        default:
          return false;
      }
    }

    public static bool IsStateElection(string electionKey)
    {
      return (electionKey.Length == ElectionKeyLengthStateOrFederal) &&
        StateCache.IsValidStateCode(GetStateCodeFromKey(electionKey));
    }

    public static bool IsStateOrFederalElection(string electionKey)
    {
      return electionKey.Length == ElectionKeyLengthStateOrFederal;
    }

    public static bool IsUSPresidentialPrimary(string electionKey)
    {
      return GetElectionTypeFromKey(electionKey) ==
        ElectionTypeUSPresidentialPrimary;
    }

    public static string ReplaceDateInKey(string electionKey, DateTime newDate)
    {
      if ((newDate.Year < MinimumElectionYear) || (newDate.Year > MaximumElectionYear))
        throw new VoteException("ReplaceDateInKey: invalid newDate");
      if ((electionKey == null) || (electionKey.Length < ElectionKeyLengthStateOrFederal))
        return string.Empty;
      return electionKey.Substring(0, 2) + newDate.ToString("yyyyMMdd") +
        electionKey.Substring(10);
    }

    // ReSharper restore ReturnTypeCanBeEnumerable.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Dead code

    //public static int CountFutureViewableByStateCode(
    //  String stateCode, int commandTimeout = -1)
    //{
    //  const string cmdText = "SELECT COUNT(*) FROM Elections " +
    //    " WHERE StateCode=@StateCode " + "  AND CountyCode='' " +
    //    "  AND IsViewable=1 " + "  AND ElectionDate >= @Today ";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
    //  VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
    //  var result = VoteDb.ExecuteScalar(cmd);
    //  return Convert.ToInt32(result);
    //}

    //public static DateTime? GetLatestPreviousViewableElectionKeyByStateCode(
    //  String stateCode)
    //{
    //  return _GetLatestPreviousViewableElectionDateByStateCode(stateCode, null);
    //}

    //public static DateTime GetLatestPreviousViewableElectionDateByStateCode(
    //  String stateCode, DateTime defaultValue)
    //{
    //  var latestPreviousViewableElectionDateByStateCode =
    //    _GetLatestPreviousViewableElectionDateByStateCode(stateCode, defaultValue);
    //  return latestPreviousViewableElectionDateByStateCode != null ?
    //    latestPreviousViewableElectionDateByStateCode.Value :
    //    defaultValue;
    //}

    //private static DateTime? _GetLatestPreviousViewableElectionDateByStateCode(
    //  String stateCode, DateTime? defaultValue)
    //{
    //  string cmdText;
    //  var isFederal = stateCode == "US";

    //  if (isFederal) // includes all federal codes
    //    cmdText = "SELECT ElectionKey" + " FROM Elections" +
    //      " WHERE StateCode IN ('U1','U2','U3')" + "  AND IsViewable=1" +
    //      "  AND ElectionDate < @ElectionDate" + " ORDER BY ElectionKey DESC";
    //  else
    //    cmdText = "SELECT ElectionKey" + " FROM Elections" +
    //      " WHERE StateCode=@StateCode" + "  AND IsViewable=1" +
    //      "  AND CountyCode=''" + "  AND LocalCode=''" +
    //      "  AND ElectionDate < @ElectionDate" + " ORDER BY ElectionKey DESC";

    //  cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
    //  var cmd = VoteDb.GetCommand(cmdText, -1);
    //  if (!isFederal)
    //    VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
    //  VoteDb.AddCommandParameter(cmd, "ElectionDate", DateTime.Today);
    //  var result = VoteDb.ExecuteScalar(cmd);
    //  if (result == null || result == DBNull.Value) return defaultValue;
    //  return (DateTime) result;
    //}

    #endregion Dead code
  }
}