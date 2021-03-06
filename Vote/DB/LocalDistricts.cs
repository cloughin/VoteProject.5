using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class LocalDistrictsRow
  {
  }

  public partial class LocalDistricts
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static string[] GetAutosuggest(string term, string stateCode, string countyCode)
    {
      const string cmdText = "SELECT LocalDistrict" +
        " FROM LocalDistricts WHERE StateCode=@StateCode AND CountyCode=@CountyCode" +
        " ORDER BY LocalDistrict";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(r => r.LocalDistrict())
          .Where(d => d.StartsWith(term, StringComparison.OrdinalIgnoreCase))
          .ToArray();
      }
    }

    public static Dictionary<string, string> GetFocusedNamesDictionary(
      DataTable tableIn)
    {
      if (tableIn.Rows.Count == 0)
        return new Dictionary<string, string>();

      // group the jurisdictions and eliminate dups
      var grouped = tableIn.Rows.Cast<DataRow>()
        .Select(
          row =>
            new
            {
              StateCode = row.StateCode(),
              CountyCode = row.CountyCode(),
              LocalCode = row.LocalCode()
            })
        .Distinct()
        .GroupBy(i => new {i.StateCode, i.CountyCode})
        .GroupBy(g => g.Key.StateCode);

      // build the where clause
      var statesConditions = new List<string>();
      foreach (var stateGroup in grouped)
      {
        var countiesConditions = new List<string>();
        foreach (var countyGroup in stateGroup)
        {
          var locals = countyGroup.ToList();
          var localsCondition = locals.Count == 1
            ? "='" + locals[0].LocalCode + "'"
            : "IN ('" + string.Join("','", locals.Select(l => l.LocalCode)) + "')";
          countiesConditions.Add("CountyCode='" + countyGroup.Key.CountyCode +
            "' AND LocalCode " + localsCondition);
        }
        statesConditions.Add("StateCode='" + stateGroup.Key + "' AND (" +
          string.Join(" OR ", countiesConditions) + ")");
      }

      var cmdText = "SELECT StateCode,CountyCode,LocalCode,LocalDistrict" +
        " FROM LocalDistricts WHERE " + string.Join(" OR ", statesConditions);

      var cmd = VoteDb.GetCommand(cmdText, 0);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .ToDictionary(
            row => row.StateCode() + "|" + row.CountyCode() + "|" + row.LocalCode(),
            row => row.LocalDistrict());
      }
    }

    public static Dictionary<string, string> GetNamesDictionary(string stateCode,
      string countyCode)
    {
      return GetNamesDataByStateCodeCountyCode(stateCode, countyCode)
        .ToDictionary(row => row.LocalCode, row => row.LocalDistrict);
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string countyCode, string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, countyCode, null, null,
        selectedValue);
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string countyCode, string firstEntry, string firstEntryValue,
      string selectedValue = null)
    {
      dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null) firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue,
          firstEntryValue == selectedValue);
      }

      if (string.IsNullOrWhiteSpace(stateCode) ||
        string.IsNullOrWhiteSpace(countyCode)) return false;
      var locals = GetNamesDictionary(stateCode, countyCode);
      if (locals.Count == 0) return false;
      foreach (var local in locals.OrderBy(kvp => kvp.Value))
        dropDownList.AddItem(local.Value, local.Key, local.Key == selectedValue);
      return true;
    }

    public static bool Populate(HtmlSelect dropDownList, string stateCode,
      string countyCode, string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, countyCode, null, null,
        selectedValue);
    }

    public static bool Populate(HtmlSelect dropDownList, string stateCode,
      string countyCode, string firstEntry, string firstEntryValue,
      string selectedValue = null)
    {
      dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null) firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue,
          firstEntryValue == selectedValue);
      }

      var locals = GetNamesDictionary(stateCode, countyCode);
      if (locals.Count == 0) return false;
      foreach (var local in locals.OrderBy(kvp => kvp.Value))
        dropDownList.AddItem(local.Value, local.Key, local.Key == selectedValue);
      return true;
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}