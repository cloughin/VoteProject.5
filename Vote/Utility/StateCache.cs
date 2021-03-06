using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote
{
  public static class StateCache
  {
    // This class caches state descriptive information. It also handles pseudo-state
    // codes (ie Federal codes U1, U2, U3 and U4).

    #region Private

    static StateCache()
    {
      // Create the dictionaries
      StateCodeDictionary =
        new Dictionary<string, StateInfo>(StringComparer.OrdinalIgnoreCase);
      LdsStateCodeDictionary =
        new Dictionary<string, StateInfo>(StringComparer.OrdinalIgnoreCase);
      StateAbbreviationDictionary =
        new Dictionary<string, StateInfo>(StringComparer.OrdinalIgnoreCase);

      // Load it up
      var table = States.GetAllCachedData();
      foreach (var row in table)
      {
        // Create a StateInfo object
        var stateInfo = new StateInfo
          {
            StateCode = row.StateCode,
            Name = row.State,
            ShortName = row.ShortName,
            BallotStateName = row.BallotStateName,
            Email = row.Email,
            ContactEmail = row.ContactEmail,
            ElectionsAuthority = row.ElectionsAuthority,
            IsIncumbentShownOnBallots = row.IsIncumbentShownOnBallots,
            EncloseNickname = row.EncloseNickname,
            LdsStateCode = row.LdsStateCode,
            IsState = row.IsState,
            Uri = row.Url.ToUri(),
            ShowUnopposed = row.ShowUnopposed,
            ShowWriteIn = row.ShowWriteIn
          };

        // Add it to the StateCodeDictionary
        StateCodeDictionary.Add(stateInfo.StateCode, stateInfo);

        // Add numeric LdsStateCodes to the LdsStateCodeDictionary
        if (stateInfo.LdsStateCode.IsDigits())
          LdsStateCodeDictionary.Add(stateInfo.LdsStateCode, stateInfo);

        // Load the abbreviations dictionary
        StateAbbreviationDictionary.Add(stateInfo.StateCode, stateInfo);
        StateAbbreviationDictionary.Add(stateInfo.Name, stateInfo);
        var abbreviations = row.Abbreviations.Split(',');
        foreach (
          var trimmed in abbreviations.Select(abbreviation => abbreviation.Trim())
            .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed)))
          StateAbbreviationDictionary.Add(trimmed, stateInfo);
      }

      // Load the sorted list from the StateCodeDictionary
      StateList =
        StateCodeDictionary.Values.OrderBy(info => info.Name.ToLowerInvariant())
          .ToList();
    }

    private class StateInfo
    {
      public string StateCode;
      public string Name;
      public string ShortName;
      public string BallotStateName;
      public string Email;
      public string ContactEmail;
      public string ElectionsAuthority;
      public bool IsIncumbentShownOnBallots;
      public string EncloseNickname;
      public string LdsStateCode;
      public bool IsState;
      public Uri Uri;
      public bool ShowUnopposed;
      public bool ShowWriteIn;

      public bool IsUSCode => StateCode == "US";

      public bool IsFederalCode => !IsState && StateCode != "PP";

      public bool IsPresidentialPrimary => StateCode == "US" || StateCode == "PP";
    }

    // This list is sorted by Name (case insensitive)
    private static readonly List<StateInfo> StateList;

    // This dictionary is indexed by StateCode (case insensitive)
    private static readonly Dictionary<string, StateInfo> StateCodeDictionary;

    // This dictionary is indexed by LdsStateCode (numeric)
    private static readonly Dictionary<string, StateInfo> LdsStateCodeDictionary;

    // This dictionary is indexed by all recognized names for the state
    // (case insensitive).
    // Includes the state code, name, and all abbreviations in the db.
    // Key could contain up to 3 words (District of Columbia).
    private static readonly Dictionary<string, StateInfo> StateAbbreviationDictionary;

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

    public static IEnumerable<string> AllFederalCodes
    {
      get
      {
        return StateList.Where(info => info.IsFederalCode && !info.IsUSCode)
          .Select(info => info.StateCode)
          .OrderBy(code => code);
      }
    }

    public static IEnumerable<string> All51StateCodes
    {
      get
      {
        return StateList.Where(info => info.IsState)
          .OrderBy(info => info.Name)
          .Select(info => info.StateCode);
      }
    }

    public static string GetBallotStateName(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.BallotStateName;
      return null;
    }

    public static string GetContactEmail(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.ContactEmail;
      return null;
    }

    public static string GetElectionsAuthority(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.ElectionsAuthority;
      return null;
    }

    public static string GetEmail(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.Email;
      return null;
    }

    public static string GetEncloseNicknameCode(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.EncloseNickname;
      return null;
    }

    public static bool GetIsIncumbentShownOnBallots(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.IsIncumbentShownOnBallots;
      return false;
    }

    public static string GetShortName(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.ShortName;
      return null;
    }

    public static bool GetShowUnopposed(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.ShowUnopposed;
      return false;
    }

    public static bool GetShowWriteIn(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.ShowWriteIn;
      return false;
    }

    public static string GetStateCode(string stateName)
    {
      StateInfo info;
      return StateAbbreviationDictionary.TryGetValue(stateName, out info) 
        ? info.StateCode 
: null;
    }

    public static string GetStateName(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.Name;
      return null;
    }

    public static Uri GetUri(string stateCode)
    {
      StateInfo info;
      if (stateCode != null && StateCodeDictionary.TryGetValue(stateCode, out info))
        return info.Uri;
      return null;
    }

    public static bool IsPresidentialPrimary(string code)
    {
      StateInfo info;
      if (code != null && StateCodeDictionary.TryGetValue(code, out info))
        return info.IsPresidentialPrimary;
      return false;
    }

    public static bool IsUS(string code)
    {
      StateInfo info;
      if (code != null && StateCodeDictionary.TryGetValue(code, out info))
        return info.IsUSCode;
      return false;
    }

    public static bool IsUSGovernors(string code)
    {
      return code == "U4";
    }

    public static bool IsUSHouse(string code)
    {
      return code == "U3";
    }

    public static bool IsUSPresident(string code)
    {
      return code == "U1";
    }

    public static bool IsUSSenate(string code)
    {
      return code == "U2";
    }

    public static bool IsValidFederalCode(string code, bool includeUS = true)
    {
      StateInfo info;
      if (code != null && StateCodeDictionary.TryGetValue(code, out info))
        return info.IsFederalCode && (includeUS || !info.IsUSCode);
      return false;
    }

    public static bool IsValidStateCode(string code)
    {
      StateInfo info;
      if (code != null && StateCodeDictionary.TryGetValue(code, out info))
        return info.IsState;
      return false;
    }

    public static bool IsValidStateOrFederalCode(string code, bool includeUS = true)
    {
      return code != null && StateCodeDictionary.ContainsKey(code) && 
        (includeUS || code.IsNeIgnoreCase("US"));
    }

    public static string StateCodeFromLdsStateCode(string ldsStateCode)
    {
      StateInfo info;
      return LdsStateCodeDictionary.TryGetValue(ldsStateCode, out info)
        ? info.StateCode
        : null;
    }

    public static void Populate(DropDownList dropDownList, string selectedValue = null)
    {
      Populate(dropDownList, null, null, selectedValue);
    }

    public static void Populate(DropDownList dropDownList, bool clear)
    {
      Populate(dropDownList, null, null, null, clear);
    }

    public static void Populate(DropDownList dropDownList, string firstEntry,
      string firstEntryValue, string selectedValue = null, bool clear = true)
    {
      if (clear) dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null)
          firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue,
          firstEntryValue == selectedValue);
      }

      // Add all real states (plus DC)
      foreach (var si in StateList.Where(si => si.IsState))
        dropDownList.AddItem(si.Name, si.StateCode, si.StateCode == selectedValue);
    }

    public static void Populate(HtmlSelect dropDownList, string selectedValue = null)
    {
      Populate(dropDownList, null, null, selectedValue);
    }

    public static void Populate(HtmlSelect dropDownList, bool clear)
    {
      Populate(dropDownList, null, null, null, clear);
    }

    public static void Populate(HtmlSelect dropDownList, string firstEntry,
      string firstEntryValue, string selectedValue = null, bool clear = true)
    {
      if (clear) dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null)
          firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue, 
          firstEntryValue == selectedValue);
      }

      // Add all real states (plus DC)
      foreach (var si in StateList)
        if (si.IsState)
          dropDownList.AddItem(si.Name, si.StateCode, si.StateCode == selectedValue);
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