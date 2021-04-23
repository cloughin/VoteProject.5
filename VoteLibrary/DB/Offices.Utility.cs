using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Vote;
using static System.String;

namespace DB.Vote
{
  public static class OfficeExtensionMethods
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static string Description(this OfficeClass officeClass, 
      string stateCode = null)
    {
      return Offices.GetOfficeClassDescription(officeClass, stateCode);
    }

    public static ElectoralClass ElectoralClass(this OfficeClass officeClass)
    {
      return Offices.GetElectoralClass(officeClass);
    }

    public static bool IsCounty(this OfficeClass officeClass)
    {
      return Offices.GetElectoralClass(officeClass) == Vote.ElectoralClass.County;
    }

    public static bool IsFederal(this OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.USPresident:
        case OfficeClass.USSenate:
        case OfficeClass.USHouse:
          return true;

        default:
          return false;
      }
    }

    public static bool IsLocal(this OfficeClass officeClass)
    {
      return Offices.GetElectoralClass(officeClass) == Vote.ElectoralClass.Local;
    }

    public static bool IsState(this OfficeClass officeClass)
    {
      return Offices.GetElectoralClass(officeClass) == Vote.ElectoralClass.State;
    }

    public static bool IsStateOrFederal(this OfficeClass officeClass)
    {
      return Offices.GetElectoralClass(officeClass) == Vote.ElectoralClass.State ||
        officeClass.IsFederal();
    }

    public static bool IsValid(this OfficeClass officeClass)
    {
      return Enum.IsDefined(typeof (OfficeClass), officeClass) &&
        officeClass != OfficeClass.All && officeClass != OfficeClass.Undefined;
    }

    public static OfficeClass ParseOfficeClass(this string str)
    {
      return Enum.TryParse(str, true, out OfficeClass officeClass)
        ? officeClass
        : OfficeClass.Undefined;
    }

    public static string ShortDescription(this OfficeClass officeClass,
      string stateCode = null)
    {
      return Offices.GetOfficeClassShortDescription(officeClass, stateCode);
    }

    public static string StateCodeProxy(this OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.USPresident:
          return "U1";

        case OfficeClass.USSenate:
          return "U2";

        case OfficeClass.USHouse:
          return "U3";

        case OfficeClass.USGovernors:
          return "U4";

        default:
          return Empty;
      }
    }

    public static OfficeClass ToOfficeClass(this object value)
    {
      try
      {
        return (OfficeClass) Convert.ToInt32(value);
      }
      catch (Exception)
      {
        return OfficeClass.Undefined;
      }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public class OfficialsReportOptions
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

    public ElectoralClass ElectoralClass = ElectoralClass.Unknown;
    public string StateCode = Empty;
    public string CountyCode = Empty;
    public string LocalKey = Empty;

    public string BuildWhereClause(bool forRunningMateSelect)
    {
      string where;

      switch (ElectoralClass)
      {
        case ElectoralClass.USPresident:
          where = $"o.OfficeLevel={OfficeClass.USPresident.ToInt()}";
          break;

        case ElectoralClass.USSenate:
          where = $"o.OfficeLevel={OfficeClass.USSenate.ToInt()}";
          break;

        case ElectoralClass.USHouse:
          where = $"o.OfficeLevel={OfficeClass.USHouse.ToInt()}";
          break;

        case ElectoralClass.USGovernors:
          where =
            $"o.AlternateOfficeLevel IN ({OfficeClass.USGovernors.ToInt()},{OfficeClass.USLtGovernor.ToInt()})";
          break;

        case ElectoralClass.State:
          where =
            $"(o.OfficeLevel={OfficeClass.USPresident.ToInt()} OR o.StateCode='{StateCode}'" +
            $" AND o.OfficeLevel IN ({OfficeClass.USSenate.ToInt()},{OfficeClass.USHouse.ToInt()}," +
            $"{OfficeClass.StateWide.ToInt()},{OfficeClass.StateSenate.ToInt()}," +
            $"{OfficeClass.StateHouse.ToInt()},{OfficeClass.StateJudicial.ToInt()}," + 
            $"{OfficeClass.StateParty.ToInt()}))";
          break;

        case ElectoralClass.County:
          where = $"o.StateCode='{StateCode}' AND o.CountyCode='{CountyCode}'";
          break;

        case ElectoralClass.Local:
          where = $"o.StateCode='{StateCode}' AND o.LocalKey='{LocalKey}'";
          break;

        default:
          throw new ArgumentException("unsupported ElectoralClass in options");
      }

      return
        $" {(forRunningMateSelect ? "oo.RunningMateKey <> '' AND" : Empty)} o.IsInactive=0 AND o.IsOnlyForPrimaries=0 AND {where}";
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

  #region Public

  #region ReSharper disable

  // ReSharper disable MemberCanBePrivate.Global
  // ReSharper disable MemberCanBeProtected.Global
  // ReSharper disable UnusedMember.Global
  // ReSharper disable UnusedMethodReturnValue.Global
  // ReSharper disable UnusedAutoPropertyAccessor.Global
  // ReSharper disable UnassignedField.Global

  #endregion ReSharper disable

  public enum ElectoralClass
  {
    All,
    USPresident,
    USSenate,
    USHouse,
    USGovernors,
    State,
    County,
    Local,
    Unknown
  }

  [Flags]
  public enum GetOfficeClassesOptions
  {
    None = 0x00,
    IncludeAll = 0x01,
    IncludeUSPresident = 0x02,
    IncludeUSSenate = 0x04,
    IncludeUSHouse = 0x08,
    IncludeState = 0x10,
    IncludeCounty = 0x20,
    IncludeLocal = 0x40,
    IncludeFederal = 0x0E,
    IncludeCongress = 0x0C,
    IncludeByUSCongressionalDistrict = 0x100,
    IncludeByStateSenateDistrict = 0x200,
    IncludeByStateHouseDistrict = 0x400,
    IncludeByDistrict = 0x700
  }

  public enum OfficeClass
  {
    // Federal & State Offices - Group 1 
    All = 0,
    USPresident = 1,
    USSenate = 2,
    USHouse = 3,
    USGovernors = 25, // alternate only with 4
    USLtGovernor = 26, // alternate only with 4
    StateWide = 4,
    StateSenate = 5,
    StateHouse = 6,
    //StateDistrictMultiCounties = 7, // not used

    // County Offices - Group 2 
    CountyExecutive = 8,
    CountyLegislative = 9,
    CountySchoolBoard = 10,
    CountyCommission = 11,

    // Local and Town Offices 
    LocalExecutive = 12,
    LocalLegislative = 13,
    LocalSchoolBoard = 14,
    LocalCommission = 15,

    // Judicial Offices 
    StateJudicial = 16,
    //StateDistrictMultiCountiesJudicial = 17, // not used
    CountyJudicial = 18,
    LocalJudicial = 19,

    // Political Party Offices - Group 5 
    StateParty = 20,
    //StateDistrictMultiCountiesParty = 21, // not used
    CountyParty = 22,
    LocalParty = 23,

    // Local and Town Offices in Multiple Partial Counties
    ////StateDistrictMultiPartialCounties = 24, // not used

    // DC Specials
    DCMayor = 27, // alternate only with 4
    DCCouncil = 28, // alternate only with 4,5
    DCBoardOfEducation = 29, // alternate only with 4,5
    DCShadowSenator = 30, // alternate only with 4
    DCShadowRepresentative = 31, // alternate only with 4

    // Offices for Legislative Districts
    USCongressionalDistrictOffice = 40,
    StateSenateDistrictOffice = 41,
    StateHouseDistrictOffice = 42,

    Undefined = 99 // alternate only
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

  public partial class Offices
  {
    #region Private

    private static readonly Dictionary<OfficeClass, ElectoralClass>
      ElectoralClassDictionary = new Dictionary<OfficeClass, ElectoralClass>
        {
          {OfficeClass.All, ElectoralClass.All},
          {OfficeClass.USPresident, ElectoralClass.USPresident},
          {OfficeClass.USSenate, ElectoralClass.USSenate},
          {OfficeClass.USHouse, ElectoralClass.USHouse},
          {OfficeClass.USGovernors, ElectoralClass.USGovernors},
          {OfficeClass.StateWide, ElectoralClass.State},
          {OfficeClass.StateSenate, ElectoralClass.State},
          {OfficeClass.StateHouse, ElectoralClass.State},
          //{OfficeClass.StateDistrictMultiCounties, ElectoralClass.State},
          //{OfficeClass.StateDistrictMultiPartialCounties, ElectoralClass.State},
          {OfficeClass.StateJudicial, ElectoralClass.State},
          //{OfficeClass.StateDistrictMultiCountiesJudicial, ElectoralClass.State},
          {OfficeClass.StateParty, ElectoralClass.State},
          //{OfficeClass.StateDistrictMultiCountiesParty, ElectoralClass.State},
          {OfficeClass.CountyExecutive, ElectoralClass.County},
          {OfficeClass.CountyLegislative, ElectoralClass.County},
          {OfficeClass.CountySchoolBoard, ElectoralClass.County},
          {OfficeClass.CountyCommission, ElectoralClass.County},
          {OfficeClass.CountyJudicial, ElectoralClass.County},
          {OfficeClass.CountyParty, ElectoralClass.County},
          {OfficeClass.LocalExecutive, ElectoralClass.Local},
          {OfficeClass.LocalLegislative, ElectoralClass.Local},
          {OfficeClass.LocalSchoolBoard, ElectoralClass.Local},
          {OfficeClass.LocalCommission, ElectoralClass.Local},
          {OfficeClass.LocalJudicial, ElectoralClass.Local},
          {OfficeClass.LocalParty, ElectoralClass.Local}
        };

    private static readonly OfficeClass[] StateOffices =
    {
      OfficeClass.StateWide, OfficeClass.StateSenate, OfficeClass.StateHouse,
      //OfficeClass.StateDistrictMultiCounties,
      OfficeClass.StateJudicial,
      //OfficeClass.StateDistrictMultiCountiesJudicial,
      OfficeClass.StateParty//,
      //OfficeClass.StateDistrictMultiCountiesParty
      //OfficeClass.StateDistrictMultiPartialCounties
    };

    public static readonly OfficeClass[] StateOfficesWithoutLegislature =
    {
      OfficeClass.StateWide, 
      //OfficeClass.StateDistrictMultiCounties,
      OfficeClass.StateJudicial,
      //OfficeClass.StateDistrictMultiCountiesJudicial,
      OfficeClass.StateParty//,
      //OfficeClass.StateDistrictMultiCountiesParty
    };

    private static readonly OfficeClass[] CountyOffices =
    {
      OfficeClass.CountyExecutive, OfficeClass.CountyLegislative,
      OfficeClass.CountySchoolBoard, OfficeClass.CountyCommission,
      OfficeClass.CountyJudicial, OfficeClass.CountyParty
    };

    private static readonly OfficeClass[] LocalOffices =
    {
      OfficeClass.LocalExecutive, OfficeClass.LocalLegislative,
      OfficeClass.LocalSchoolBoard, OfficeClass.LocalCommission,
      OfficeClass.LocalJudicial, OfficeClass.LocalParty
    };

    #endregion Private

    #region Public
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public const bool EnableNewClasses = false;

    public const string USPresident = "USPresident";

    public static string ActualizeKey(string officeKey, string countyCode, string localKey)
    {
      if (IsVirtualKey(officeKey))
        if (IsCountyOffice(officeKey))
          officeKey = GetStateCodeFromKey(officeKey) + countyCode + officeKey.Substring(5);
        else // local
          officeKey = GetStateCodeFromKey(officeKey) + localKey + officeKey.Substring(7);
      return officeKey;
    }

    public static string ActualizeOffice(string officeKey, string countyCode,
      string localKey)
    {
      if (IsVirtualKey(officeKey))
      {
        var table = GetDataByOfficeKey(officeKey);
        if (table.Count != 0)
        {
          var row = table[0];
          officeKey = ActualizeKey(officeKey, countyCode, localKey);
          var stateCode = GetStateCodeFromKey(officeKey);
          if (!IsNullOrEmpty(localKey))
            countyCode = Empty;
          if (!OfficeKeyExists(officeKey))
            Insert(officeKey, stateCode, countyCode, GetLocalKeyFromKey(officeKey),
              row.DistrictCode, /*row.DistrictCodeAlpha,*/ row.OfficeLine1, row.OfficeLine2,
              row.OfficeLevel, row.AlternateOfficeLevel, row.OfficeOrderWithinLevel,
              row.IsRunningMateOffice, row.IsPrimaryRunningMateOffice, row.IsOnlyForPrimaries, 
              row.Incumbents, row.VoteInstructions, row.WriteInInstructions, row.WriteInWording,
              row.WriteInLines, false, DateTime.UtcNow, false, row.IsSpecialOffice, false,
              row.ElectionPositions, row.PrimaryPositions, row.PrimaryRunoffPositions,
              row.GeneralRunoffPositions, row.PrimaryAdRate, row.GeneralAdRate, false, row.Undeletable);
        }
      }
      else if (!OfficeKeyExists(officeKey))
      {
        // could be an unactualized actual key
        var virtualKey = Empty;
        if (officeKey.Substring(2, 5).IsDigits())
          virtualKey = officeKey.Substring(0, 2) + "#####" + officeKey.Substring(7);
        else if (officeKey.Substring(2, 3).IsDigits())
          virtualKey = officeKey.Substring(0, 2) + "###" + officeKey.Substring(5);
        if (virtualKey != Empty && OfficeKeyExists(virtualKey))
          officeKey = ActualizeOffice(virtualKey, countyCode, localKey);
      }
      return officeKey;
    }

    public static bool CanAddOfficesToOfficeClass(OfficeClass officeClass,
      string stateCode)
    {
      switch (officeClass)
      {
        case OfficeClass.All:
        case OfficeClass.USPresident:
        case OfficeClass.USSenate:
        case OfficeClass.USHouse:
          return false;
        case OfficeClass.StateSenate:
          return stateCode == "DC";
        case OfficeClass.StateHouse:
          return stateCode == "DC";
        default:
          return true;
      }
    }

    public static bool CanAddOfficesToOfficeClass(string officeKey)
    {
      return CanAddOfficesToOfficeClass(GetOfficeClass(officeKey),
        GetStateCodeFromKey(officeKey));
    }

    public static string CreateOfficeKey(OfficeClass officeClass, string stateCode, 
      string countyCode, string localKey, string districtCode, /*string districtCodeAlpha,*/ 
      string officeLine1, string officeLine2)
    {
      var officeName = (officeLine1 + " " + officeLine2)
        .SimpleRecase()
        .StripNonAlphaNumeric();

      // remove any occurrence of state name
      officeName = Regex.Replace(officeName,
        StateCache.GetStateName(stateCode).StripNonAlphaNumeric(),
        Empty,
        RegexOptions.IgnoreCase);

      var districtCodeWithoutLeadingZeroes = districtCode.TrimStart('0');

      var officeKey = Empty;
      officeName = officeName.StripNonAlphaNumeric();
      switch (officeClass)
      {
        case OfficeClass.USPresident:
          officeKey = stateCode.ToUpper() + "President";
          break;

        case OfficeClass.USSenate:
          officeKey = stateCode.ToUpper() + "USSenate" +
            officeLine2
              .SimpleRecase()
              .StripNonAlphaNumeric();
          break;

        case OfficeClass.USHouse:
          officeKey = stateCode.ToUpper() + "USHouse" + districtCodeWithoutLeadingZeroes;
          break;

        case OfficeClass.StateWide:
        case OfficeClass.StateJudicial:
        case OfficeClass.StateParty:
          officeKey = stateCode.ToUpper() + officeName;
          break;

        case OfficeClass.StateSenate:
          officeKey = stateCode.ToUpper() + "StateSenate" + districtCodeWithoutLeadingZeroes
            /*+ districtCodeAlpha.Trim()*/;
          break;

        case OfficeClass.StateHouse:
          officeKey = stateCode.ToUpper() + "StateHouse" + districtCodeWithoutLeadingZeroes
            /*+ districtCodeAlpha.Trim()*/;
          break;

        //case OfficeClass.StateDistrictMultiCounties:
        ////case OfficeClass.StateDistrictMultiCountiesJudicial:
        ////case OfficeClass.StateDistrictMultiCountiesParty:
        //  officeKey = stateCode.ToUpper() + districtCode + districtCodeAlpha.Trim()
        //    + officeName;
        //  break;

        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          officeKey = stateCode.ToUpper() + countyCode + officeName;
          break;

        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalParty:
        case OfficeClass.LocalJudicial:
          officeKey = stateCode.ToUpper() + localKey + officeName;
          break;
      }

      if (officeKey.Length > 150)
        officeKey = officeKey.Substring(0, 150);

      return officeKey;
    }

    // Direct replacement for db.Name_Office
    public static string FormatOfficeName(string officeKey, string separator = " ")
    {
      return FormatOfficeName(VotePage.GetPageCache(), officeKey, separator);
    }

    public static string FormatOfficeName(PageCache cache, string officeKey,
      string separator = " ")
    {
      return FormatOfficeName(cache.Offices.GetOfficeLine1(officeKey),
        cache.Offices.GetOfficeLine2(officeKey), officeKey, separator);
    }

    public static string FormatOfficeName(DataRow row, string separator = " ")
    {
      var officeKey = GetOfficeKey(row);
      return FormatOfficeName(row.OfficeLine1(), row.OfficeLine2(), officeKey,
        separator);
    }

    public static string FormatOfficeName(OfficesRow row, string separator = " ")
    {
      return FormatOfficeName(row.OfficeLine1, row.OfficeLine2, row.OfficeKey,
        separator);
    }

    public static string FormatOfficeName(string officeLine1, string officeLine2,
      string officeKey, string separator = " ")
    {
      var officeName = Empty;
      if (!IsNullOrEmpty(officeLine1))
        officeName += officeLine1;

      if (officeKey != USPresident && !IsNullOrEmpty(officeLine2))
      {
        if (!IsNullOrEmpty(officeName))
          officeName += separator;
        officeName += officeLine2;
      }

      return officeName;
    }

    public static string GetCountyCodeFromKey(string officeKey)
    {
      if (!IsNullOrEmpty(GetLocalKeyFromKey(officeKey)))
        return Empty;
      var countyCodeCandidate = officeKey.SafeSubstring(2, 3);
      return countyCodeCandidate.Length == 3 &&
        (countyCodeCandidate.IsDigits() || countyCodeCandidate == "###")
          ? countyCodeCandidate
          : Empty;
    }

    public static IEnumerable<OfficeClass> GetCountyOfficeClasses(
      GetOfficeClassesOptions options = GetOfficeClassesOptions.None)
    {
      if ((options & GetOfficeClassesOptions.IncludeAll) != 0)
        yield return OfficeClass.All;

      foreach (var officeClass in CountyOffices)
        yield return officeClass;
    }

    public static string GetValidDistrictCode(string officeKey)
    {
      var districtCode = VotePage.GetPageCache()
        .Offices.GetDistrictCode(officeKey);
      return districtCode == "000" ? Empty : districtCode;
    }

    public static ElectoralClass GetElectoralClass(OfficeClass officeClass)
    {
      return ElectoralClassDictionary.TryGetValue(officeClass, out var electoralClass)
        ? electoralClass
        : ElectoralClass.Unknown;
    }

    public static ElectoralClass GetElectoralClass(string stateCode, string countyCode,
      string localKey)
    {
      if (IsNullOrEmpty(stateCode)) return ElectoralClass.Unknown;
      if (!IsNullOrEmpty(localKey)) return ElectoralClass.Local;
      if (!IsNullOrEmpty(countyCode)) return ElectoralClass.County;

      switch (stateCode)
      {
        case "PP":
        case "US":
        case "U1":
          return ElectoralClass.USPresident;
        case "U2":
          return ElectoralClass.USSenate;
        case "U3":
          return ElectoralClass.USHouse;
        case "U4":
          return ElectoralClass.USGovernors;
        default:
          return StateCache.IsValidStateCode(stateCode)
            ? ElectoralClass.State
            : ElectoralClass.All;
      }
    }

    public static string GetElectoralClassDescription(string stateCode,
      string countyCode = "", string localKey = "")
    {
      switch (GetElectoralClass(stateCode, countyCode, localKey))
      {
        case ElectoralClass.USPresident:
          return "U.S. President";

        case ElectoralClass.USSenate:
          return "U.S. Senate";

        case ElectoralClass.USHouse:
          return "U.S. House of Representatives";

        case ElectoralClass.USGovernors:
          return "State Governors";

        case ElectoralClass.State:
          return StateCache.IsValidStateCode(stateCode)
            ? StateCache.GetStateName(stateCode)
            : Empty;

        case ElectoralClass.County:
          if (countyCode != "000")
            if (CountyCache.CountyExists(stateCode, countyCode))
              if (stateCode != "DC")
                return CountyCache.GetCountyName(stateCode, countyCode) + ", " +
                  StateCache.GetStateName(stateCode);
              else
                return StateCache.GetStateName(stateCode) + " Local";
            else
              return Empty;
          return GetElectoralClassDescription(stateCode) + " Counties";

        case ElectoralClass.Local:
          if (countyCode == "000")
            return GetElectoralClassDescription(stateCode, countyCode) + " Local Districts";
          var localName =
            VotePage.GetPageCache()
              .LocalDistricts.GetLocalDistrict(stateCode, localKey);
          if (!IsNullOrWhiteSpace(localName))
          {
            var countyDescription = CountyCache.GetCountyDescription(stateCode, countyCode,
              localKey);
            return localName + ", " + countyDescription + ", " +
              StateCache.GetShortName(stateCode);
          }
          return Empty;

        default:
          return Empty;
      }
    }

    public static string GetElectoralClassDescriptionFromOfficeKey(string officeKey)
    {
      return GetElectoralClassDescription(GetStateCodeFromKey(officeKey),
        GetCountyCodeFromKey(officeKey), GetLocalKeyFromKey(officeKey));
    }

    public static string GetElectoralClassShortDescription(string stateCode,
      string countyCode = "", string localKey = "")
    {
      switch (GetElectoralClass(stateCode, countyCode, localKey))
      {
        case ElectoralClass.USPresident:
          return "US President";

        case ElectoralClass.USSenate:
          return "US Senate";

        case ElectoralClass.USHouse:
          return "US House";

        case ElectoralClass.USGovernors:
          return "Governors";

        case ElectoralClass.State:
          return StateCache.IsValidStateCode(stateCode)
            ? StateCache.GetShortName(stateCode)
            : Empty;

        case ElectoralClass.County:
          if (countyCode == "000")
            return GetElectoralClassShortDescription(stateCode) + " Counties";
          if (CountyCache.CountyExists(stateCode, countyCode))
            if (stateCode != "DC")
              return CountyCache.GetCountyName(stateCode, countyCode) + ", " +
                StateCache.GetShortName(stateCode);
            else
              return StateCache.GetShortName(stateCode) + " Local";
          return Empty;

        case ElectoralClass.Local:
          if (countyCode == "000")
            return GetElectoralClassShortDescription(stateCode, countyCode) +
              " Locals";
          var localName =
            VotePage.GetPageCache()
              .LocalDistricts.GetLocalDistrict(stateCode, localKey);
          if (!IsNullOrWhiteSpace(localName))
            return localName + ", " +
              CountyCache.GetCountyName(stateCode, countyCode) + ", " +
              StateCache.GetShortName(stateCode);
          return Empty;

        default:
          return Empty;
      }
    }

    public static string GetLocalKeyFromKey(string officeKey)
    {
      var localKeyCandidate = officeKey.SafeSubstring(2, 5);
      return localKeyCandidate.Length == 5 &&
        (localKeyCandidate.IsDigits() || localKeyCandidate == "#####")
        ? localKeyCandidate
        : Empty;
    }

    public static string GetLocalizedOfficeClassDescription(
      OfficeClass officeClass, string stateCode, string countyCode,
      string localKey)
    {
      var desc = GetElectoralClassDescription(stateCode, countyCode, localKey);
      if (!StateCache.IsValidFederalCode(stateCode))
        desc += " " + GetOfficeClassDescription(officeClass, stateCode);
      return desc;
    }

    // Direct replacement for db.Name_Office_State
    // ===========================================
    // Sample output:
    // Offices.GetLocalizedOfficeName("USPresident") [Level 1:USPresident] = U.S. President
    // Offices.GetLocalizedOfficeName("CAUSSenate") [Level 2:USSenate] = United States Senate
    // Offices.GetLocalizedOfficeName("CAUSHouse33") [Level 3:USHouse] = United States RepresentativeDistrict 33
    // Offices.GetLocalizedOfficeName("CAGovernor") [Level 4:StateWide] = Governor
    // Offices.GetLocalizedOfficeName("DCMayor") [Level 4:StateWide] = Mayor
    // Offices.GetLocalizedOfficeName("CAStateSenate34") [Level 5:StateSenate] = State SenatorDistrict 34
    // Offices.GetLocalizedOfficeName("DCBoardOfEducation3") [Level 5:StateSenate] = Ward 3Member Of The State Board Of Education
    // Offices.GetLocalizedOfficeName("CAStateHouse61") [Level 6:StateHouse] = State Assembly MemberDistrict 61
    // Offices.GetLocalizedOfficeName("DCStateHouse4") [Level 6:StateHouse] = Advisory Neighborhood Commissioner 1D
    // Offices.GetLocalizedOfficeName("VA059Treasurer") [Level 8:CountyExecutive] = Fairfax CountyTreasurer
    // Offices.GetLocalizedOfficeName("VA059BoardOfSupervisors") [Level 9:CountyLegislative] = Fairfax CountyChairman Board of Supervisors
    // Offices.GetLocalizedOfficeName("VA059SchoolBoard") [Level 10:CountySchoolBoard] = Fairfax CountyMember School Board At Large
    // Offices.GetLocalizedOfficeName("VA05931Mayor") [Level 12:LocalExecutive] = Clifton TownMayor
    // Offices.GetLocalizedOfficeName("VA05916BoardOfSupervisors") [Level 13:LocalLegislative] = Mason DistrictMember Board of Supervisors
    // Offices.GetLocalizedOfficeName("VA05916SchoolBoard") [Level 14:LocalSchoolBoard] = Mason DistrictMember School Board
    // Offices.GetLocalizedOfficeName("VA05918SoilWaterDirector") [Level 15:LocalCommission] = Northern Virginia DistrictSoil And Water Conservation Director

    public static string GetLocalizedOfficeName(string officeKey,
      string separator = " ")
    {
      return GetLocalizedOfficeName(VotePage.GetPageCache(), officeKey, separator);
    }

    public static string GetLocalizedOfficeName(PageCache cache, string officeKey,
      string separator = " ")
    {
      return GetLocalizedOfficeName(officeKey, cache.Offices.GetOfficeLine1(officeKey),
        cache.Offices.GetOfficeLine2(officeKey), separator);
    }

    public static string GetLocalizedOfficeName(OfficesRow row,
      string separator = " ")
    {
      return GetLocalizedOfficeName(row.OfficeKey, row.OfficeLine1, row.OfficeLine2,
        separator);
    }

    public static string GetLocalizedOfficeName(DataRow row, string separator = " ")
    {
      return GetLocalizedOfficeName(GetOfficeKey(row), row.OfficeLine1(),
        row.OfficeLine2(), separator);
    }

    public static string GetLocalizedOfficeName(string officeKey, string officeLine1, 
      string officeLine2, string separator = " ")
    {
      var stateCode = GetStateCodeFromKey(officeKey);
      var countyCode = GetCountyCodeFromKey(officeKey);
      var localKey = GetLocalKeyFromKey(officeKey);
      var officeName = Empty;

      if (IsCountyOffice(officeKey))
        officeName += CountyCache.GetCountyName(stateCode, countyCode);
      else if (IsLocalOffice(officeKey))
        officeName +=  
          VotePage.GetPageCache().LocalDistricts.GetLocalDistrict(stateCode, localKey);

      if (!IsNullOrEmpty(officeName))
        officeName += separator;

      officeName += FormatOfficeName(officeLine1, officeLine2, officeKey, separator);

      return officeName;
    }

    // Direct replacement for db.Name_Office_State_Electoral
    public static string GetLocalizedOfficeNameWithElectoralClass(string officeKey)
    {
      return GetLocalizedOfficeNameWithElectoralClass(VotePage.GetPageCache(),
        officeKey);
    }

    public static string GetLocalizedOfficeNameWithElectoralClass(PageCache cache,
      string officeKey)
    {
      return GetLocalizedOfficeNameWithElectoralClass(officeKey,cache.Offices.GetOfficeLine1(officeKey),
        cache.Offices.GetOfficeLine2(officeKey));
    }

    public static string GetLocalizedOfficeNameWithElectoralClass(OfficesRow row)
    {
      return GetLocalizedOfficeNameWithElectoralClass(row.OfficeKey,
        row.OfficeLine1, row.OfficeLine2);
    }

    public static string GetLocalizedOfficeNameWithElectoralClass(DataRow row)
    {
      return GetLocalizedOfficeNameWithElectoralClass(GetOfficeKey(row),
        row.OfficeLine1(), row.OfficeLine2());
    }

    public static string GetLocalizedOfficeNameWithElectoralClass(string officeKey,
      string officeLine1, string officeLine2)
    {
      var isPresidentialCandidate = String.Equals(officeKey, USPresident,
        StringComparison.OrdinalIgnoreCase);
      var electoralClassDescription = isPresidentialCandidate
        ? Empty
        : GetElectoralClassDescriptionFromOfficeKey(officeKey);
      if (IsNullOrEmpty(electoralClassDescription))
        return GetLocalizedOfficeName(officeKey, officeLine1, officeLine2);
      return FormatOfficeName(officeLine1, officeLine2, officeKey) + ", " + electoralClassDescription;
    }

    public static IEnumerable<OfficeClass> GetLocalOfficeClasses(
      GetOfficeClassesOptions options = GetOfficeClassesOptions.None)
    {
      if ((options & GetOfficeClassesOptions.IncludeAll) != 0)
        yield return OfficeClass.All;

      foreach (var officeClass in LocalOffices)
        yield return officeClass;
    }

    public static (string stateCode, string countyCode, string localKey, string level) 
      GetIssuesCoding(string officeKey)
    {
      switch (GetOfficeClass(officeKey))
      {
        case OfficeClass.USPresident:
        case OfficeClass.USSenate:
        case OfficeClass.USHouse:
          return ("US", Empty, Empty, Issues.IssueLevelNational);

        case OfficeClass.USGovernors:
        case OfficeClass.StateWide:
        case OfficeClass.StateSenate:
        case OfficeClass.StateHouse:
        case OfficeClass.StateJudicial:
        case OfficeClass.StateParty:
        case OfficeClass.USCongressionalDistrictOffice:
        case OfficeClass.StateSenateDistrictOffice:
        case OfficeClass.StateHouseDistrictOffice:
          return (GetStateCodeFromKey(officeKey), Empty, Empty, Issues.IssueLevelState);

        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          return (GetStateCodeFromKey(officeKey), GetCountyCodeFromKey(officeKey), Empty, Issues.IssueLevelCounty);

        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalJudicial:
        case OfficeClass.LocalParty:
          return (GetStateCodeFromKey(officeKey), Empty, GetLocalKeyFromKey(officeKey), Issues.IssueLevelLocal);

        default:
          return (Empty, Empty, Empty, "A");
      }
    }

    public static OfficeClass GetOfficeClass(PageCache cache, string officeKey)
    {
      return cache.Offices.GetOfficeClass(officeKey);
    }

    public static OfficeClass GetOfficeClass(string officeKey)
    {
      return GetOfficeClass(VotePage.GetPageCache(), officeKey);
    }

    public static string GetOfficeClassDescription(OfficeClass officeClass,
      string stateCode = null)
    {
      switch (officeClass)
      {
        case OfficeClass.Undefined:
          return "Undefined";

        case OfficeClass.All:
          return "All";

        case OfficeClass.USPresident:
          return "U.S. President";

        case OfficeClass.USSenate:
          return "U.S. Senate";

        case OfficeClass.USHouse:
          return "U.S. House of Representatives";

        case OfficeClass.USGovernors:
          return "State Governors";

        case OfficeClass.StateWide:
          return stateCode == "DC"
            ? "Citywide Non-Judicial"
            : "Statewide Non-Judicial [like Governor, Treasurer...]";

        case OfficeClass.StateSenate:
          return stateCode == "DC" ? "Wards" : "State Senate";

        case OfficeClass.StateHouse:
          return stateCode == "DC" ? "ANCs" : "State House";

        //case OfficeClass.StateDistrictMultiCounties:
        //  return stateCode == "DC"
        //    ? "Multi-ANC Non-Judicial"
        //    : "Multi-County Non-Judicial";

        case OfficeClass.CountyExecutive:
          return
            "County Executive [like Executive , Mayor, Sheriff, Treasurer, Clerk...]";

        case OfficeClass.CountyLegislative:
          return "County Legislative [like Councils, Board of Supervisors...]";

        case OfficeClass.CountySchoolBoard:
          return "County School & College Boards";

        case OfficeClass.CountyCommission:
          return
            "County Special Commission and Committees [like Parks, Soil, Water...]";

        case OfficeClass.LocalExecutive:
          return
            "Local Executive [like Executive , Mayor, Sheriff, Treasurer, Clerk...]";

        case OfficeClass.LocalLegislative:
          return "Local Legislative [like Councils, Board of Supervisors...]";

        case OfficeClass.LocalSchoolBoard:
          return "Local School & College Boards";

        case OfficeClass.LocalCommission:
          return
            "Local Special Commission and Committees [like Parks, Soil, Water...]";

        case OfficeClass.StateJudicial:
          return stateCode == "DC" ? "Citywide Judicial" : "Statewide Judicial";

        //case OfficeClass.StateDistrictMultiCountiesJudicial:
        //  return "JUDICIAL District";

        case OfficeClass.CountyJudicial:
          return "County Judicial";

        case OfficeClass.LocalJudicial:
          return "Local Judicial";

        case OfficeClass.StateParty:
          return stateCode == "DC"
            ? "Political Party"
            : "Statewide Political Party";

        //case OfficeClass.StateDistrictMultiCountiesParty:
        //  return stateCode == "DC"
        //    ? "Local Political PARTY"
        //    : "Multi-County Political PARTY";

        case OfficeClass.CountyParty:
          return "County Political Party";

        case OfficeClass.LocalParty:
          return "Local Political Party";

        case OfficeClass.USCongressionalDistrictOffice:
          return "US Congressional District Office";

        case OfficeClass.StateSenateDistrictOffice:
          return "State Senate District Office";

        case OfficeClass.StateHouseDistrictOffice:
          return "State House District Office";

        default:
          return "Undefined";
      }
    }

    public static string GetOfficeClassShortDescription(OfficeClass officeClass,
      string stateCode = null)
    {
      switch (officeClass)
      {
        case OfficeClass.Undefined:
          return "Undefined";

        case OfficeClass.All:
          return "All";

        case OfficeClass.USPresident:
          return "US President";

        case OfficeClass.USSenate:
          return "US Senate";

        case OfficeClass.USHouse:
          return "US House";

        case OfficeClass.USGovernors:
          return "Governor";

        case OfficeClass.StateWide:
          return stateCode == "DC"
            ? "Citywide"
            : "Statewide";

        case OfficeClass.StateSenate:
          return stateCode == "DC" ? "Wards" : "State Senate";

        case OfficeClass.StateHouse:
          return stateCode == "DC" ? "ANCs" : "State House";

        //case OfficeClass.StateDistrictMultiCounties:
        //  return stateCode == "DC"
        //    ? "Multi-ANC"
        //    : "Multi-County";

        case OfficeClass.CountyExecutive:
          return
            "County Executive";

        case OfficeClass.CountyLegislative:
          return "County Legislative";

        case OfficeClass.CountySchoolBoard:
          return "County School Board";

        case OfficeClass.CountyCommission:
          return
            "County Special Commissions/Committees";

        case OfficeClass.LocalExecutive:
          return
            "Local Executive";

        case OfficeClass.LocalLegislative:
          return "Local Legislative";

        case OfficeClass.LocalSchoolBoard:
          return "Local School Board";

        case OfficeClass.LocalCommission:
          return
            "Local Special Commisions/Committees";

        case OfficeClass.StateJudicial:
          return stateCode == "DC" ? "Citywide Judicial" : "Statewide Judicial";

        //case OfficeClass.StateDistrictMultiCountiesJudicial:
        //  return "Judicial District";

        case OfficeClass.CountyJudicial:
          return "County Judicial";

        case OfficeClass.LocalJudicial:
          return "Local Judicial";

        case OfficeClass.StateParty:
          return stateCode == "DC"
            ? "Citywide Party"
            : "Statewide Party";

        //case OfficeClass.StateDistrictMultiCountiesParty:
        //  return stateCode == "DC"
        //    ? "Local Party"
        //    : "Multi-County Party";

        case OfficeClass.CountyParty:
          return "County Party";

        case OfficeClass.LocalParty:
          return "Local Party";

        case OfficeClass.USCongressionalDistrictOffice:
          return "US Congress District";

        case OfficeClass.StateSenateDistrictOffice:
          return "State Senate District";

        case OfficeClass.StateHouseDistrictOffice:
          return "State House District";

        default:
          return "Undefined";
      }
    }

    public static string GetOfficeClassShortDescriptionExtended(OfficeClass officeClass,
      OfficeClass alternateOfficeClass, string stateCode)
    {
      switch (alternateOfficeClass)
      {
        case OfficeClass.USGovernors:
          return "Governor";

        case OfficeClass.USLtGovernor:
          return "Lt. Governor";

        case OfficeClass.DCMayor:
          return "DC Mayor";

        case OfficeClass.DCCouncil:
          return "DC Council";

        case OfficeClass.DCBoardOfEducation:
          return "DC Board of Education";

        case OfficeClass.DCShadowSenator:
          return "DC Shadow Senator";

        case OfficeClass.DCShadowRepresentative:
          return "DC Shadow Representative";

        default:
          return GetOfficeClassShortDescription(officeClass, stateCode);
      }
    }

    public static string GetOfficeClassShortDescriptionExtended(DataRow row)
    {
      return GetOfficeClassShortDescriptionExtended(row.OfficeClass(),
        row.AlternateOfficeClass(), row.StateCode());
    }

    public static string GetOfficeClassDescription(string officeKey)
    {
      return GetOfficeClassDescription(GetOfficeClass(officeKey),
        GetStateCodeFromKey(officeKey));
    }

    public static string GetOfficeClassShortDescription(string officeKey)
    {
      return GetOfficeClassShortDescription(GetOfficeClass(officeKey),
        GetStateCodeFromKey(officeKey));
    }

    public static IEnumerable<OfficeClass> GetOfficeClasses(
      GetOfficeClassesOptions options)
    {
      if ((options & GetOfficeClassesOptions.IncludeAll) != 0)
        yield return OfficeClass.All;

      if ((options & GetOfficeClassesOptions.IncludeUSPresident) != 0)
        yield return OfficeClass.USPresident;

      if ((options & GetOfficeClassesOptions.IncludeUSSenate) != 0)
        yield return OfficeClass.USSenate;

      if ((options & GetOfficeClassesOptions.IncludeUSHouse) != 0)
        yield return OfficeClass.USHouse;

      if ((options & GetOfficeClassesOptions.IncludeState) != 0)
        foreach (var officeClass in StateOffices)
          yield return officeClass;

      // ReSharper disable once ConditionIsAlwaysTrueOrFalse
      if (EnableNewClasses)
      {
        if ((options & GetOfficeClassesOptions.IncludeByUSCongressionalDistrict) != 0)
          yield return OfficeClass.USCongressionalDistrictOffice;

        if ((options & GetOfficeClassesOptions.IncludeByStateSenateDistrict) != 0)
          yield return OfficeClass.StateSenateDistrictOffice;

        if ((options & GetOfficeClassesOptions.IncludeByStateHouseDistrict) != 0)
          yield return OfficeClass.StateHouseDistrictOffice;
      }

      if ((options & GetOfficeClassesOptions.IncludeCounty) != 0)
        foreach (var officeClass in CountyOffices)
          yield return officeClass;

      if ((options & GetOfficeClassesOptions.IncludeLocal) != 0)
        foreach (var officeClass in LocalOffices)
          yield return officeClass;
    }

    private static string GetOfficeKey(DataRow row)
    {
      var officeKey = row.Table.Columns.Contains("LiveOfficeKey")
        ? row.LiveOfficeKey()
        : row.OfficeKey();
      return officeKey;
    }

    public static string GetShortLocalizedOfficeClassDescription(
      OfficeClass officeClass, string stateCode, string countyCode,
      string localKey)
    {
      var desc = GetElectoralClassShortDescription(stateCode, countyCode, localKey);
      if (!StateCache.IsValidFederalCode(stateCode))
        desc += " " + GetOfficeClassDescription(officeClass, stateCode);
      return desc;
    }

    public static string GetStateCodeFromKey(string officeKey)
    {
      if (officeKey == null || officeKey.Length < 2) return Empty;
      return officeKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    public static IEnumerable<OfficeClass> GetStateOfficeClasses(
      GetOfficeClassesOptions options = GetOfficeClassesOptions.None)
    {
      if ((options & GetOfficeClassesOptions.IncludeAll) != 0)
        yield return OfficeClass.All;

      if ((options & GetOfficeClassesOptions.IncludeUSPresident) != 0)
        yield return OfficeClass.USPresident;

      if ((options & GetOfficeClassesOptions.IncludeUSSenate) != 0)
        yield return OfficeClass.USSenate;

      if ((options & GetOfficeClassesOptions.IncludeUSHouse) != 0)
        yield return OfficeClass.USHouse;

      foreach (var officeClass in StateOffices)
        yield return officeClass;
    }

    public static OfficeClass GetValidatedOfficeClass(int intOfficeClass)
    {
      var officeClass = intOfficeClass.ToOfficeClass();
      if (!Enum.IsDefined(typeof (OfficeClass), officeClass))
        officeClass = OfficeClass.Undefined;
      return officeClass;
    }

    public static OfficeClass GetValidatedOfficeClass(string officeClassAsString)
    {
      return int.TryParse(officeClassAsString, out var officeClassAsInt)
        ? GetValidatedOfficeClass(officeClassAsInt)
        : OfficeClass.Undefined;
    }

    public static string GetValidatedStateCodeFromKey(string officeKey)
    {
      if (officeKey == null || officeKey.Length < 2) return Empty;
      var stateCode = officeKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : Empty;
    }

    public static string GetVoteForNoMoreThanWording(int positions)
    {
      string positionsDesc;
      switch (positions)
      {
        case 1:
          positionsDesc = "one";
          break;

        case 2:
          positionsDesc = "two";
          break;

        case 3:
          positionsDesc = "three";
          break;

        case 4:
          positionsDesc = "four";
          break;

        case 5:
          positionsDesc = "five";
          break;

        case 6:
          positionsDesc = "six";
          break;

        case 7:
          positionsDesc = "seven";
          break;

        case 8:
          positionsDesc = "eight";
          break;

        case 9:
          positionsDesc = "nine";
          break;

        case 10:
          positionsDesc = "ten";
          break;

        default:
          positionsDesc = positions.ToString(CultureInfo.InvariantCulture);
          break;
      }
      return $"(Vote for no more than {positionsDesc})";
    }


    public static bool IsCountyOffice(string officeKey)
    {
      return !IsNullOrWhiteSpace(GetCountyCodeFromKey(officeKey));
    }

    public static bool IsElectoralClassStateCountyOrLocal(string stateCode, string countyCode,
        string localKey)
    {
      var electoralClass = GetElectoralClass(stateCode, countyCode, localKey);
      return electoralClass == ElectoralClass.State || electoralClass == ElectoralClass.County ||
        electoralClass == ElectoralClass.Local;
    }

    public static bool IsFederalOffice(string officeKey)
    {
      if (IsUSPresident(officeKey)) return true;
      var officeClass = GetOfficeClass(officeKey);
      return officeClass == OfficeClass.USSenate ||
        officeClass == OfficeClass.USHouse;
    }

    public static bool IsInElection(string officeKey, string electionKey)
    {
      if (IsNullOrWhiteSpace(electionKey) || IsNullOrWhiteSpace(officeKey))
        return false;
      return ElectionsOffices.ElectionKeyOfficeKeyExists(electionKey, officeKey);
    }

    public static bool IsLocalOffice(string officeKey)
    {
      return GetLocalKeyFromKey(officeKey) != Empty;
    }

    public static bool IsPrimaryRunningMateOffice(string officeKey)
    {
      return VotePage.GetPageCache()
        .Offices.GetIsPrimaryRunningMateOffice(officeKey);
    }

    public static bool IsRunningMateOffice(string officeKey)
    {
      return VotePage.GetPageCache()
        .Offices.GetIsRunningMateOffice(officeKey);
    }

    public static bool IsRunningMateOfficeInElection(string electionKey, string officeKey)
    {
      return Elections.IsPrimaryElection(electionKey)
        ? IsPrimaryRunningMateOffice(officeKey)
        : IsRunningMateOffice(officeKey);
    }

    public static bool IsStateHouse(string officeKey)
    {
      return GetOfficeClass(officeKey) == OfficeClass.StateHouse;
    }

    public static bool IsStateOrFederalOffice(string officeKey)
    {
      return GetCountyCodeFromKey(officeKey) == Empty;
    }

    public static bool IsStateSenate(string officeKey)
    {
      return GetOfficeClass(officeKey) == OfficeClass.StateSenate;
    }

    public static bool IsUSHouse(string officeKey)
    {
      return GetOfficeClass(officeKey) == OfficeClass.USHouse;
    }

    public static bool IsUSPresident(string officeKey)
    {
      return officeKey.IsEqIgnoreCase(USPresident);
    }

    public static bool IsUSSenate(string officeKey)
    {
      return GetOfficeClass(officeKey) == OfficeClass.USSenate;
    }

    public static bool IsValid(string officeKey)
    {
      return MemCache.IsValidOffice(officeKey);
    }

    public static bool IsValidStateHouseDistrict(string district, string stateCode)
    {
      return district != "000" && (!IsNullOrWhiteSpace(district) || stateCode == "DC" || stateCode == "NE");
    }

    public static bool IsVirtualKey(string officeKey)
    {
      if (GetLocalKeyFromKey(officeKey) == "#####")
        return true;
      return GetCountyCodeFromKey(officeKey) == "###";
    }

    public static bool MatchesOfficeCode(string officeKey1, string officeKey2)
    {
      int officeCodeIndex;
      if (IsStateOrFederalOffice(officeKey1))
        officeCodeIndex = 2;
      else if (IsCountyOffice(officeKey1))
        officeCodeIndex = 5;
      else 
        officeCodeIndex = 7;
      return officeKey1.Substring(officeCodeIndex).IsEqIgnoreCase(officeKey2.Substring(officeCodeIndex));
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}