using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using JetBrains.Annotations;
using Vote;
using static System.String;

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
      [CanBeNull] string stateCode = null)
    {
      if (stateCode == "PP")
        return "Template of Major Presidential Party Primary Candidates";
      if (stateCode == "US" && electionType == "G")
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
      string stateCode, [CanBeNull] string selectedValue = null)
    {
      dropDownList.Items.Clear();

      foreach (var kvp in GetElectionTypes(stateCode))
        dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
    }

    public static void PopulateElectionTypes(HtmlSelect dropDownList,
      string stateCode, [CanBeNull] string selectedValue = null)
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

      var stateName = Empty;
      if (!IsNullOrWhiteSpace(stateCode))
        stateName = " " + StateCache.GetStateName(stateCode);

      var partyName = Empty;
      if (partyCode != Parties.NationalPartyAll)
      {
        partyName = Parties.GetNationalPartyDescription(partyCode, "?");
        if (partyName == "?")
        {
          stateName = Empty;
          partyName = Parties.GetPartyName(stateCode + partyCode, "Unknown Major Party");
        }
      }
      if (!IsNullOrWhiteSpace(partyName))
        partyName = " " + partyName;

      var typeDescription = GetElectionTypeDescription(electionType,
        GetStateCodeFromKey(electionKey));

      return electionDate + stateName + partyName + " " + typeDescription;
    }

    public static string FormatElectionKey(DateTime electionDate,
      string electionTypeCode, string nationalPartyCode, string stateCode)
    {
      if (electionDate.Year < MinimumElectionYear ||
        electionDate.Year > MaximumElectionYear)
        throw new VoteException("FormatElectionKey: invalid electionDate");
      if (electionTypeCode.SafeString().Length != 1)
        throw new VoteException("FormatElectionKey: invalid electionTypeCode");
      if (nationalPartyCode.SafeString().Length != 1)
        throw new VoteException("FormatElectionKey: invalid nationalPartyCode");
      var isState = StateCache.IsValidStateCode(stateCode);
      if (!isState && !StateCache.IsValidFederalCode(stateCode) &&
        !stateCode.IsEqIgnoreCase("PP"))
        throw new VoteException("FormatElectionKey: invalid stateCode");
      return
      (stateCode + electionDate.ToString("yyyyMMdd") + electionTypeCode +
        nationalPartyCode).ToUpperInvariant();
    }

    public static string GetCountyCodeFromKey(string electionKey)
    {
      if (!IsNullOrEmpty(GetLocalKeyFromKey(electionKey)))
        return Empty;
      if (electionKey.Length < ElectionKeyLengthCounty)
        return Empty;
      return electionKey.Substring(ElectionKeyLengthStateOrFederal, 3)
        .ToUpperInvariant();
    }

    public static string[] GetCountyElectionKeysFromKey(string electionKey)
    {
      // there may be multiple counties for a local
      var stateCode = GetStateCodeFromKey(electionKey);
      if (electionKey.Length < ElectionKeyLengthCounty)
        return new string[0];
      if (electionKey.Length != ElectionKeyLengthLocal)
        return new[] { electionKey.Substring(0, ElectionKeyLengthCounty) };

      // return all possible keys
      var stateElectionKey = GetStateElectionKeyFromKey(electionKey);
      return
        LocalIdsCodes.FindCounties(stateCode, GetLocalKeyFromKey(electionKey))
          .Select(c => stateElectionKey + c)
          .ToArray();
    }

    public static string GetCountyElectionKeyFromKey(string electionKey,
      string stateCode, [CanBeNull] string countyCode)
    {
      var stateKey = GetStateElectionKeyFromKey(electionKey, stateCode);
      if (IsNullOrWhiteSpace(stateKey) ||
        IsNullOrWhiteSpace(countyCode) || countyCode.Length != 3)
        return Empty;
      return stateKey + countyCode;
    }

    public static string GetDefaultElectionKeyFromKey([NotNull] string electionKey)
    {
      return IsNullOrWhiteSpace(electionKey) || electionKey.Length < 10
        ? Empty
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
          return Empty;
      }
    }

    public static string GetElectionTypeFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < 12)
        return Empty;
      return electionKey.Substring(10, 1)
        .ToUpperInvariant();
    }

    public static ElectoralClass GetElectoralClass(string electionKey)
    {
      var stateCode = GetStateCodeFromKey(electionKey);
      var countyCode = GetCountyCodeFromKey(electionKey);
      var localKey = 
        GetLocalKeyFromKey(electionKey);
      return Offices.GetElectoralClass(stateCode, countyCode, localKey);
    }

    public static string GetElectoralClassDescription(string electionKey)
    {
      var stateCode = GetStateCodeFromKey(electionKey);
      var countyCode = GetCountyCodeFromKey(electionKey);
      var localKey = GetLocalKeyFromKey(electionKey);
      return Offices.GetElectoralClassDescription(stateCode, countyCode, localKey);
    }

    public static string GetFederalElectionKeyFromKey(string electionKey, 
      [NotNull] string federalCode)
    {
      if (electionKey == null || electionKey.Length < ElectionKeyLengthStateOrFederal)
        throw new ArgumentException("Missing or invalid electionKey");
      if (federalCode.Length != 2)
        return Empty;
      return federalCode +
        electionKey.Substring(2, ElectionKeyLengthStateOrFederal - 2);
    }

    public static string GetLocalKeyFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < ElectionKeyLengthLocal)
        return Empty;
      return electionKey.Substring(ElectionKeyLengthStateOrFederal, 5)
        .ToUpperInvariant();
    }

    public static string GetLocalElectionKeyFromKey(string electionKey)
    {
      return electionKey.Length != ElectionKeyLengthLocal
        ? Empty
        : electionKey;
    }

    public static string GetLocalElectionKeyFromKey(string electionKey, [NotNull] string localKey)
    {
      var stateKey = GetStateElectionKeyFromKey(electionKey);
      if (IsNullOrWhiteSpace(stateKey) ||
        IsNullOrWhiteSpace(localKey) || localKey.Length != 5)
        return Empty;
      return stateKey + localKey;
    }

    public static string GetNationalPartyCodeFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < 12)
        return Empty;
      return electionKey.Substring(11, 1)
        .ToUpperInvariant();
    }

    public static int? GetOfficePositions(string electionType, int electionPositions, 
      int primaryPositions, int generalRunoffPositions, int primaryRunoffPositions)
    {
      int? positions;
      switch (electionType)
      {
        case ElectionTypeStatePrimaryRunoff:
          positions = primaryRunoffPositions;
          break;
        case ElectionTypeStateGeneralRunoff:
          positions = generalRunoffPositions;
          break;
        case var testType when IsPrimaryElectionType(testType):
          positions = primaryPositions;
          break;
        default:
          positions = electionPositions;
          break;
      }

      return positions;
    }

    public static int? GetOfficePositions(string electionType, DataRow positionsData)
    {
      return GetOfficePositions(electionType, positionsData.ElectionPositions(),
        positionsData.PrimaryPositions(), positionsData.GeneralRunoffPositions(),
        positionsData.PrimaryRunoffPositions());
    }

    public static int? GetOfficePositions(DataRow row)
    {
      return GetOfficePositions(row.ElectionType(), row);
    }

    public static string GetStateCodeFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < ElectionKeyLengthStateOrFederal)
        return Empty;
      return electionKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    public static string GetStateElectionKeyFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < ElectionKeyLengthStateOrFederal)
        return Empty;
      return electionKey.Substring(0, ElectionKeyLengthStateOrFederal)
        .ToUpperInvariant();
    }

    public static string GetStateElectionKeyFromKey([NotNull] string electionKey,
      [NotNull] string stateCode)
    {
      if (electionKey.Length < ElectionKeyLengthStateOrFederal ||
        stateCode.Length != 2)
        return Empty;
      return
        (stateCode + electionKey.Substring(2, ElectionKeyLengthStateOrFederal - 2))
          .ToUpperInvariant();
    }

    public static string GetValidatedFederalCodeFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < 2) return Empty;
      var stateCode = electionKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidFederalCode(stateCode) ? stateCode : Empty;
    }

    public static string GetValidatedStateCodeFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < 2) return Empty;
      var stateCode = electionKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : Empty;
    }

    public static string GetYyyyMmDdFromKey([NotNull] string electionKey)
    {
      if (electionKey.Length < 10) return Empty;
      return electionKey.Substring(2, 8);
    }

    public static bool HasContestsOrBallotMeasures(string electionKey, string congress,
      string stateSenate, string stateHouse, string countyCode, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict)
    {
      // General Elections and Presidential Primaries are assumed to have contests so
      // skip query
      if (new[]
      {
        ElectionTypeGeneralElection,
        ElectionTypeUSPresidentialPrimary,
        ElectionTypeStatePresidentialPrimary
      }.Contains(GetElectionTypeFromKey(electionKey)))
        return true;

      // check for contests
      if (ElectionsPoliticians.GetSampleBallotData(electionKey, congress,
        stateSenate, stateHouse, countyCode, district, place, elementary, secondary, unified,
        cityCouncil, countySupervisors, schoolDistrictDistrict, true).Rows[0].Count() > 0)
        return true;

      // one last check for ballot measures only if necessary
      return Referendums.GetBallotReportData(electionKey, countyCode, district, place,
        elementary, secondary, unified, cityCouncil, countySupervisors,
        schoolDistrictDistrict).Rows.Count > 0;
    }

    public static string InsertStateCodeIntoElectionKey(string electionKey,
      string stateCode)
    {
      if (electionKey.Length >= 12 && stateCode.Length == 2)
        return stateCode + electionKey.Substring(2, 10);
      return Empty;
    }

    public static bool IsCountyElection(string electionKey)
    {
      return electionKey.Length == ElectionKeyLengthCounty &&
        GetCountyCodeFromKey(electionKey) != "000";
    }

    public static bool IsGeneralElection(string electionKey)
    {
      return GetElectionTypeFromKey(electionKey) == ElectionTypeGeneralElection;
    }

    public static bool IsLocalElection(string electionKey)
    {
      return electionKey.Length == ElectionKeyLengthLocal &&
        GetLocalKeyFromKey(electionKey) != "00000";
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
      return electionKey.Length == ElectionKeyLengthStateOrFederal &&
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

    public static string ReplaceDateInKey([NotNull] string electionKey, DateTime newDate)
    {
      if (newDate.Year < MinimumElectionYear || newDate.Year > MaximumElectionYear)
        throw new VoteException("ReplaceDateInKey: invalid newDate");
      if (electionKey.Length < ElectionKeyLengthStateOrFederal)
        return Empty;
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
  }
}