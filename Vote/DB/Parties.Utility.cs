using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;

namespace DB.Vote
{
  public partial class Parties
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

    public static int GetNationalPartyOrder(string nationalPartyCode)
    {
      switch (nationalPartyCode.SafeString().Trim())
      {
        case "":
        case "A":
        case "X":
          return 0;

        case "D":
          return 100;

        case "G":
          return 400;

        case "L":
          return 300;

        case "R":
          return 200;

        default:
          return 1000;
      }
    }

    public static string GetStateCodeFromKey(string partyKey)
    {
      if ((partyKey == null) || (partyKey.Length < 2)) return string.Empty;
      return partyKey.Substring(0, 2)
        .ToUpperInvariant();
    }

    public const string NationalPartyAll = "A";
    public const string NationalPartyDemocratic = "D";
    public const string NationalPartyGreen = "G";
    public const string NationalPartyLibertarian = "L";
    public const string NationalPartyRepublican = "R";
    public const string NationalPartyNonPartisan = "X";

    private class NationalPartyInfo
    {
      public string NationalPartyCode;
      public string Description;
      public bool IsSpecial;
    }

    private static readonly NationalPartyInfo[] NationalPartyInfos =
    {
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyAll,
        Description = "All Parties",
        IsSpecial = true
      },
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyDemocratic,
        Description = "Democratic Party",
        IsSpecial = false
      },
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyRepublican,
        Description = "Republican Party",
        IsSpecial = false
      },
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyGreen,
        Description = "Green Party",
        IsSpecial = false
      },
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyLibertarian,
        Description = "Libertarian Party",
        IsSpecial = false
      },
      new NationalPartyInfo
      {
        NationalPartyCode = NationalPartyNonPartisan,
        Description = "Non-Partisan",
        IsSpecial = true
      }
    };

    private static NationalPartyInfo GetNationalPartyInfo(string partyCode)
    {
      return
        NationalPartyInfos.FirstOrDefault(
          i => i.NationalPartyCode.IsEqIgnoreCase(partyCode));
    }

    public static string GetNationalPartyDescription(string partyCode,
      string def = "Unknown National Party")
    {
      var info = GetNationalPartyInfo(partyCode);
      return info == null ? def : info.Description;
    }

    public static string GetNationalOrStatePartyDescription(string partyKey, string partyName,
      string def = "Unknown Party")
    {
      var partyCode = GetPartyCodeFromKey(partyKey);
      if (string.IsNullOrWhiteSpace(partyCode)) return def;
      var info = GetNationalPartyInfo(partyCode);
      if (info != null)
        if (!info.IsSpecial)
          return info.Description;
      return string.IsNullOrWhiteSpace(partyName)
        ? def
        : partyName;
    }

    public static string GetPartyCodeFromKey(string partyKey)
    {
      return partyKey.Length < 3
        ? string.Empty
        : partyKey.Substring(2);
    }

    public static string FormatPartyKey(string stateCode, string partyCode)
    {
      return partyCode == "A" ? "ALL" : stateCode + partyCode;
    }

    public static IEnumerable<KeyValuePair<string, string>> GetNationalParties(
      bool includeSpecial = true)
    {
      return NationalPartyInfos.Where(i => includeSpecial || !i.IsSpecial)
        .Select(
          info =>
            new KeyValuePair<string, string>(info.NationalPartyCode,
              info.Description));
    }

    public static IEnumerable<KeyValuePair<string, string>> GetMajorParties(
      string stateCode, bool includeSpecial = true)
    {
      return NationalPartyInfos.Where(i => includeSpecial && i.IsSpecial)
        .Select(
          info =>
            new KeyValuePair<string, string>(info.NationalPartyCode,
              info.Description))
        .Union(GetMajorPartyDataByStateCode(stateCode)
          .Select(row => new KeyValuePair<string, string>(row.PartyCode,
            row.PartyName)));
    }

    //public static bool IsDisplayablePartyCode(string partyCode)
    //{
    //  return partyCode != "X";
    //}

    public static void PopulateNationalParties(DropDownList dropDownList,
      bool includeSpecial = true, string selectedValue = null, bool excludeAll = false,
      string firstItem = "--- Select a party ---")
    {
      dropDownList.Items.Clear();
      if (!string.IsNullOrWhiteSpace(firstItem)) dropDownList.AddItem(firstItem, " ");
      foreach (var kvp in GetNationalParties(includeSpecial))
        if (!excludeAll || (kvp.Key != NationalPartyAll))
          dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
    }

    public static void PopulateNationalParties(HtmlSelect dropDownList,
      bool includeSpecial = true, string selectedValue = null, bool excludeAll = false,
      string firstItem = "--- Select a party ---")
    {
      dropDownList.Items.Clear();
      if (!string.IsNullOrWhiteSpace(firstItem)) dropDownList.AddItem(firstItem, " ");
      foreach (var kvp in GetNationalParties(includeSpecial))
        if (!excludeAll || (kvp.Key != NationalPartyAll))
          dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
    }

    public static void PopulateMajorParties(DropDownList dropDownList,
      string stateCode, bool includeSpecial = true, string selectedValue = null)
    {
      dropDownList.Items.Clear();
      dropDownList.AddItem("--- Select a party ---", " ");
      foreach (var kvp in GetMajorParties(stateCode, includeSpecial))
        dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
      if (stateCode == "US")
        dropDownList.AddItem("All (use for final presidential comparison)", "A",
          "A" == selectedValue);
    }

    public static void PopulateMajorParties(HtmlSelect dropDownList,
      string stateCode, bool includeSpecial = true, string selectedValue = null)
    {
      dropDownList.Items.Clear();

      foreach (var kvp in GetMajorParties(stateCode, includeSpecial))
        dropDownList.AddItem(kvp.Value, kvp.Key, kvp.Key == selectedValue);
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