using System;
using System.Collections.Generic;
using System.Data;
using Vote;

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
      OfficeClass officeClass;
      return Enum.TryParse(str, true, out officeClass)
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
          return string.Empty;
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
    public string StateCode = string.Empty;
    public string CountyCode = string.Empty;
    public string LocalCode = string.Empty;

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
            string.Format(
              "(o.OfficeLevel={1} OR o.StateCode='{0}' AND o.OfficeLevel IN ({2},{3},{4},{5},{6}))",
              StateCode, OfficeClass.USPresident.ToInt(),
              OfficeClass.USSenate.ToInt(), OfficeClass.USHouse.ToInt(),
              OfficeClass.StateWide.ToInt(), OfficeClass.StateSenate.ToInt(),
              OfficeClass.StateHouse.ToInt());
          break;

        case ElectoralClass.County:
          where =
            $"o.StateCode='{StateCode}' AND o.CountyCode='{CountyCode}' AND o.LocalCode=''";
          break;

        case ElectoralClass.Local:
          where =
            $"o.StateCode='{StateCode}' AND o.CountyCode='{CountyCode}' AND o.LocalCode='{LocalCode}'";
          break;

        default:
          throw new ArgumentException("unsupported ElectoralClass in options");
      }

      return
        $" {(forRunningMateSelect ? "oo.RunningMateKey <> '' AND" : string.Empty)} o.IsInactive=0 AND o.IsOnlyForPrimaries=0 AND {where}";
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
    IncludeCongress = 0x0C
  }

  public enum OfficeClass
  {
    // Federal & State Offices - Group 1 
    All = 0,
    USPresident = 1,
    USSenate = 2,
    USHouse = 3,
    USGovernors = 25,
    USLtGovernor = 26,
    StateWide = 4,
    StateSenate = 5,
    StateHouse = 6,
    StateDistrictMultiCounties = 7,

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
    StateDistrictMultiCountiesJudicial = 17,
    CountyJudicial = 18,
    LocalJudicial = 19,

    // Political Party Offices - Group 5 
    StateParty = 20,
    StateDistrictMultiCountiesParty = 21,
    CountyParty = 22,
    LocalParty = 23,

    // Local and Town Offices in Multiple Partial Counties
    StateDistrictMultiPartialCounties = 24,

    // DC Specials
    DCMayor = 27,
    DCCouncil = 28,
    DCBoardOfEducation = 29,
    DCShadowSenator = 30,
    DCShadowRepresentative = 31,

    Undefined = 99
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
          {OfficeClass.StateDistrictMultiCounties, ElectoralClass.State},
          {OfficeClass.StateDistrictMultiPartialCounties, ElectoralClass.State},
          {OfficeClass.StateJudicial, ElectoralClass.State},
          {OfficeClass.StateDistrictMultiCountiesJudicial, ElectoralClass.State},
          {OfficeClass.StateParty, ElectoralClass.State},
          {OfficeClass.StateDistrictMultiCountiesParty, ElectoralClass.State},
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
      OfficeClass.StateDistrictMultiCounties, OfficeClass.StateJudicial,
      OfficeClass.StateDistrictMultiCountiesJudicial, OfficeClass.StateParty,
      OfficeClass.StateDistrictMultiCountiesParty
      //OfficeClass.StateDistrictMultiPartialCounties
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

    public const string USPresident = "USPresident";

    public static string ActualizeKey(string officeKey, string countyCode, string localCode)
    {
      if (IsVirtualKey(officeKey))
        if (IsCountyOffice(officeKey))
          officeKey = GetStateCodeFromKey(officeKey) + countyCode + officeKey.Substring(5);
        else // local
          officeKey = GetStateCodeFromKey(officeKey) + countyCode + localCode + officeKey.Substring(7);
      return officeKey;
    }

    public static string ActualizeOffice(string officeKey, string countyCode, string localCode)
    {
      if (IsVirtualKey(officeKey))
      {
        var table = GetDataByOfficeKey(officeKey);
        if (table.Count != 0)
        {
          var row = table[0];
          officeKey = ActualizeKey(officeKey, countyCode, localCode);
          if (!OfficeKeyExists(officeKey))
              Insert(officeKey, GetStateCodeFromKey(officeKey), countyCode,
                localCode, row.DistrictCode, row.DistrictCodeAlpha, row.OfficeLine1,
                row.OfficeLine2, row.OfficeLevel, row.AlternateOfficeLevel, row.OfficeOrderWithinLevel,
                row.IsRunningMateOffice, row.IsOnlyForPrimaries, row.Incumbents, row.VoteInstructions,
                row.VoteForWording, row.WriteInInstructions, row.WriteInWording, row.WriteInLines,
                false, DateTime.UtcNow, false, row.IsSpecialOffice, false, row.ElectionPositions,
                row.PrimaryPositions, row.PrimaryRunoffPositions, row.GeneralRunoffPositions, false);
        }
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
      var officeName = string.Empty;
      if (!string.IsNullOrEmpty(officeLine1))
        officeName += officeLine1;

      if (officeKey != USPresident && !string.IsNullOrEmpty(officeLine2))
      {
        if (!string.IsNullOrEmpty(officeName))
          officeName += separator;
        officeName += officeLine2;
      }

      return officeName;
    }

    public static string GetCountyCodeFromKey(string officeKey)
    {
      var countyCodeCandidate = officeKey.SafeSubstring(2, 3);
      return countyCodeCandidate.Length == 3 && 
        (countyCodeCandidate.IsDigits() || countyCodeCandidate == "###")
        ? countyCodeCandidate
        : string.Empty;
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
      return districtCode == "000" ? string.Empty : districtCode;
    }

    public static ElectoralClass GetElectoralClass(OfficeClass officeClass)
    {
      ElectoralClass electoralClass;
      return ElectoralClassDictionary.TryGetValue(officeClass, out electoralClass)
        ? electoralClass
        : ElectoralClass.Unknown;
    }

    public static ElectoralClass GetElectoralClass(string stateCode,
      string countyCode, string localCode)
    {
      if (string.IsNullOrEmpty(stateCode)) return ElectoralClass.Unknown;

      if (string.IsNullOrEmpty(countyCode))
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

      return string.IsNullOrEmpty(localCode)
        ? ElectoralClass.County
        : ElectoralClass.Local;
    }

    // Direct replacement for db.Name_Electoral(stateCode, countyCode, localCode)
    public static string GetElectoralClassDescription(string stateCode,
      string countyCode = "", string localCode = "")
    {
      switch (GetElectoralClass(stateCode, countyCode, localCode))
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
            : string.Empty;

        case ElectoralClass.County:
          if (countyCode != "000")
            if (CountyCache.CountyExists(stateCode, countyCode))
              if (stateCode != "DC")
                return CountyCache.GetCountyName(stateCode, countyCode) + ", " +
                  StateCache.GetStateName(stateCode);
              else
                return StateCache.GetStateName(stateCode) + " Local";
            else
              return string.Empty;
          return GetElectoralClassDescription(stateCode) + " Counties";

        case ElectoralClass.Local:
          if (countyCode == "000")
            return GetElectoralClassDescription(stateCode, countyCode) +
              " Local Districts";
          var localName = VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(stateCode, countyCode, localCode);
          if (!string.IsNullOrWhiteSpace(localName))
            return localName + ", " +
              CountyCache.GetCountyName(stateCode, countyCode) + ", " +
              StateCache.GetShortName(stateCode);
          return string.Empty;

        default:
          return string.Empty;
      }
    }

    public static string GetElectoralClassDescriptionFromOfficeKey(string officeKey)
    {
      return GetElectoralClassDescription(GetStateCodeFromKey(officeKey),
        GetCountyCodeFromKey(officeKey), GetLocalCodeFromKey(officeKey));
    }

    public static string GetElectoralClassShortDescription(string stateCode,
      string countyCode = "", string localCode = "")
    {
      switch (GetElectoralClass(stateCode, countyCode, localCode))
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
            : string.Empty;

        case ElectoralClass.County:
          if (countyCode == "000")
            return GetElectoralClassShortDescription(stateCode) + " Counties";
          if (CountyCache.CountyExists(stateCode, countyCode))
            if (stateCode != "DC")
              return CountyCache.GetCountyName(stateCode, countyCode) + ", " +
                StateCache.GetShortName(stateCode);
            else
              return StateCache.GetShortName(stateCode) + " Local";
          return string.Empty;

        case ElectoralClass.Local:
          if (countyCode == "000")
            return GetElectoralClassShortDescription(stateCode, countyCode) +
              " Locals";
          var localName = VotePage.GetPageCache()
            .LocalDistricts.GetLocalDistrict(stateCode, countyCode, localCode);
          if (!string.IsNullOrWhiteSpace(localName))
            return localName + ", " +
              CountyCache.GetCountyName(stateCode, countyCode) + ", " +
              StateCache.GetShortName(stateCode);
          return string.Empty;

        default:
          return string.Empty;
      }
    }

    public static string GetLocalCodeFromKey(string officeKey)
    {
      var localCodeCandidate = officeKey.SafeSubstring(5, 2);
      return localCodeCandidate.Length == 2 && 
        (localCodeCandidate.IsDigits() || localCodeCandidate == "##")
        ? localCodeCandidate
        : string.Empty;
    }

    public static string GetLocalizedOfficeClassDescription(
      OfficeClass officeClass, string stateCode, string countyCode,
      string localCode)
    {
      var desc = GetElectoralClassDescription(stateCode, countyCode, localCode);
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
      return GetLocalizedOfficeName(officeKey,
        cache.Offices.GetOfficeLine1(officeKey),
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

    public static string GetLocalizedOfficeName(string officeKey,
      string officeLine1, string officeLine2, string separator = " ")
    {
      var stateCode = GetStateCodeFromKey(officeKey);
      var countyCode = GetCountyCodeFromKey(officeKey);
      var localCode = GetLocalCodeFromKey(officeKey);
      var officeName = string.Empty;

      if (IsCountyOffice(officeKey))
        officeName += CountyCache.GetCountyName(stateCode, countyCode);
      else if (IsLocalOffice(officeKey))
        officeName += VotePage.GetPageCache()
          .LocalDistricts.GetLocalDistrict(stateCode, countyCode, localCode);

      if (!string.IsNullOrEmpty(officeName))
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
      return GetLocalizedOfficeNameWithElectoralClass(officeKey,
        cache.Offices.GetOfficeLine1(officeKey),
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
      var electoralClassDescription =
        GetElectoralClassDescriptionFromOfficeKey(officeKey);
      if (string.IsNullOrEmpty(electoralClassDescription))
        return GetLocalizedOfficeName(officeKey, officeLine1, officeLine2);
      return FormatOfficeName(officeLine1, officeLine2, officeKey) +
       ", " + electoralClassDescription;
    }

    public static IEnumerable<OfficeClass> GetLocalOfficeClasses(
      GetOfficeClassesOptions options = GetOfficeClassesOptions.None)
    {
      if ((options & GetOfficeClassesOptions.IncludeAll) != 0)
        yield return OfficeClass.All;

      foreach (var officeClass in LocalOffices)
        yield return officeClass;
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

        case OfficeClass.StateDistrictMultiCounties:
          return stateCode == "DC"
            ? "Multi-ANC Non-Judicial"
            : "Multi-County Non-Judicial";

        case OfficeClass.CountyExecutive:
          return
            "County EXECUTIVE [like Executive , Mayor, Sheriff, Treasurer, Clerk...]";

        case OfficeClass.CountyLegislative:
          return "County LEGISLATIVE [like Councils, Board of Supervisors...]";

        case OfficeClass.CountySchoolBoard:
          return "County SCHOOL & COLLEGE BOARDS";

        case OfficeClass.CountyCommission:
          return
            "County Special COMMISSION and Committees [like Parks, Soil, Water...]";

        case OfficeClass.LocalExecutive:
          return
            "Local EXECUTIVE [like Executive , Mayor, Sheriff, Treasurer, Clerk...]";

        case OfficeClass.LocalLegislative:
          return "Local LEGISLATIVE [like Councils, Board of Supervisors...]";

        case OfficeClass.LocalSchoolBoard:
          return "Local SCHOOL & COLLEGE BOARDS";

        case OfficeClass.LocalCommission:
          return
            "Local Special COMMISSION and Committees [like Parks, Soil, Water...]";

        case OfficeClass.StateJudicial:
          return stateCode == "DC" ? "Citywide JUDICIAL" : "Statewide JUDICIAL";

        case OfficeClass.StateDistrictMultiCountiesJudicial:
          return "JUDICIAL District";

        case OfficeClass.CountyJudicial:
          return "County JUDICIAL";

        case OfficeClass.LocalJudicial:
          return "Local JUDICIAL";

        case OfficeClass.StateParty:
          return stateCode == "DC"
            ? "Political PARTY"
            : "Statewide Political PARTY";

        case OfficeClass.StateDistrictMultiCountiesParty:
          return stateCode == "DC"
            ? "Local Political PARTY"
            : "Multi-County Political PARTY";

        case OfficeClass.CountyParty:
          return "County Political PARTY";

        case OfficeClass.LocalParty:
          return "Local Political PARTY";

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
          return "U.S. President";

        case OfficeClass.USSenate:
          return "U.S. Senate";

        case OfficeClass.USHouse:
          return "U.S. House";

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

        case OfficeClass.StateDistrictMultiCounties:
          return stateCode == "DC"
            ? "Multi-ANC"
            : "Multi-County";

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

        case OfficeClass.StateDistrictMultiCountiesJudicial:
          return "Judicial District";

        case OfficeClass.CountyJudicial:
          return "County Judicial";

        case OfficeClass.LocalJudicial:
          return "Local Judicial";

        case OfficeClass.StateParty:
          return stateCode == "DC"
            ? "Citywide Party"
            : "Statewide Party";

        case OfficeClass.StateDistrictMultiCountiesParty:
          return stateCode == "DC"
            ? "Local Party"
            : "Multi-County Party";

        case OfficeClass.CountyParty:
          return "County Party";

        case OfficeClass.LocalParty:
          return "Local Party";

        default:
          return "Undefined";
      }
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
      string localCode)
    {
      var desc = GetElectoralClassShortDescription(stateCode, countyCode, localCode);
      if (!StateCache.IsValidFederalCode(stateCode))
        desc += " " + GetOfficeClassDescription(officeClass, stateCode);
      return desc;
    }

    public static string GetStateCodeFromKey(string officeKey)
    {
      if (officeKey == null || officeKey.Length < 2) return string.Empty;
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
      int officeClassAsInt;
      return int.TryParse(officeClassAsString, out officeClassAsInt)
        ? GetValidatedOfficeClass(officeClassAsInt)
        : OfficeClass.Undefined;
    }

    public static string GetValidatedStateCodeFromKey(string officeKey)
    {
      if (officeKey == null || officeKey.Length < 2) return string.Empty;
      var stateCode = officeKey.Substring(0, 2)
        .ToUpperInvariant();
      return StateCache.IsValidStateCode(stateCode) ? stateCode : string.Empty;
    }

    public static bool IsCountyOffice(string officeKey)
    {
      return GetCountyCodeFromKey(officeKey) != string.Empty &&
        GetLocalCodeFromKey(officeKey) == string.Empty;
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
      if (string.IsNullOrWhiteSpace(electionKey) || string.IsNullOrWhiteSpace(officeKey))
        return false;
      return ElectionsOffices.ElectionKeyOfficeKeyExists(electionKey, officeKey);
    }

    public static bool IsLocalOffice(string officeKey)
    {
      return GetLocalCodeFromKey(officeKey) != string.Empty;
    }

    public static bool IsRunningMateOffice(string officeKey)
    {
      return VotePage.GetPageCache()
        .Offices.GetIsRunningMateOffice(officeKey);
    }

    public static bool IsStateHouse(string officeKey)
    {
      return GetOfficeClass(officeKey) == OfficeClass.StateHouse;
    }

    public static bool IsStateOrFederalOffice(string officeKey)
    {
      return GetCountyCodeFromKey(officeKey) == string.Empty;
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

    public static bool IsVirtualKey(string officeKey)
    {
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