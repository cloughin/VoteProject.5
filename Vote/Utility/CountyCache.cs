using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote
{
  public static class CountyCache
  {
    #region Private

    private static readonly Dictionary<string, string> CountyNameDictionary;

    private static readonly Dictionary<string, ReadOnlyCollection<string>>
      CountiesByStateDictionary;

    static CountyCache()
    {
      var table = Counties.GetAllCacheData();

      CountyNameDictionary =
        table.ToDictionary(row => MakeKey(row.StateCode, row.CountyCode),
          row => row.County, StringComparer.OrdinalIgnoreCase);

      CountiesByStateDictionary = table.GroupBy(row => row.StateCode)
        .ToDictionary(g => g.Key,
          g => g.OrderBy(row => row.County, StringComparer.OrdinalIgnoreCase)
            .Select(row => row.CountyCode)
            .ToList()
            .AsReadOnly());
    }

    private static string MakeKey(string stateCode, string countyCode)
    {
      return stateCode + "|" + countyCode.ZeroPad(3);
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static bool CountyExists(string stateCode, string countyCode)
    {
      if ((stateCode == null) || (countyCode == null)) return false;
      return CountyNameDictionary.ContainsKey(MakeKey(stateCode, countyCode));
    }

    public static string GetCountyName(string stateCode, string countyCode)
    {
      string name;
      if ((stateCode != null) && (countyCode != null) &&
        CountyNameDictionary.TryGetValue(MakeKey(stateCode, countyCode), out name))
        return name;
      return null;
    }

    public static ReadOnlyCollection<string> GetCountiesByState(string stateCode)
    {
      ReadOnlyCollection<string> result;
      CountiesByStateDictionary.TryGetValue(stateCode, out result);
      return result ?? new List<string>().AsReadOnly();
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, null, null, selectedValue);
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string firstEntry, string firstEntryValue, string selectedValue = null)
    {
      dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null)
          firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue,
          firstEntryValue == selectedValue);
      }

      if (string.IsNullOrWhiteSpace(stateCode)) return false;
      var counties = GetCountiesByState(stateCode);
      if (counties.Count == 0) return false;
      foreach (var countyCode in counties)
        dropDownList.AddItem(GetCountyName(stateCode, countyCode), countyCode,
          countyCode == selectedValue);
      return true;
    }

    public static bool Populate(HtmlSelect dropDownList, string stateCode,
      string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, null, null, selectedValue);
    }

    public static bool Populate(HtmlSelect dropDownList, string stateCode, string firstEntry,
      string firstEntryValue, string selectedValue = null)
    {
      dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null)
          firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue,
          firstEntryValue == selectedValue);
      }

      if (string.IsNullOrWhiteSpace(stateCode)) return false;
      var counties = GetCountiesByState(stateCode);
      if (counties.Count == 0) return false;
      foreach (var countyCode in counties)
        dropDownList.AddItem(GetCountyName(stateCode, countyCode), countyCode,
          countyCode == selectedValue);
      return true;
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}