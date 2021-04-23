using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Vote;
using VoteLibrary.Utility;
using static System.String;

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
        return table.Rows.Cast<DataRow>().Select(r => r.LocalDistrict())
          .Where(d => d.StartsWith(term, StringComparison.OrdinalIgnoreCase)).ToArray();
      }
    }

    public static Dictionary<string, SimpleListItem> GetFocusedNamesDictionary(
      DataTable tableIn)
    {
      var rows = tableIn.Rows.OfType<DataRow>()
        .Where(r => !IsNullOrWhiteSpace(r.LocalKey())).ToList();
      if (rows.Count == 0)
        return new Dictionary<string, SimpleListItem>();

      // group the jurisdictions and eliminate dups
      var grouped = rows
        .Select(row => new {StateCode = row.StateCode(), LocalKey = row.LocalKey()})
        .Distinct().GroupBy(i => i.StateCode);

      // build the where clause
      var statesConditions = new List<string>();
      foreach (var stateGroup in grouped)
      {
        var locals = stateGroup.ToList();
        var localsCondition = "IN ('" + Join("','", locals.Select(l => l.LocalKey)) + "')";
        statesConditions.Add("StateCode='" + stateGroup.Key + "' AND LocalKey " +
          localsCondition);
      }

      var cmdText = "SELECT StateCode,LocalKey,LocalDistrict" +
        " FROM LocalDistricts WHERE " + Join(" OR ", statesConditions);

      var cmd = VoteDb.GetCommand(cmdText, 0);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>().ToDictionary(
          row => row.StateCode() + "|" + row.LocalKey(), row =>
          {
            string countyCode = null;
            var countyName = CountyCache.GetCountyDescription(row.StateCode(),
              ref countyCode, row.LocalKey());
            return new SimpleListItem
            {
              Value = countyCode,
              Text = row.LocalDistrict() + ", " + countyName
            };
          });
      }
    }

    public static List<DataRow> GetJurisdictionsReportData(string stateCode)
    {
      using (var cn = VoteDb.GetOpenConnection())
      {
        // first get the counties
        const string cmdText =
          "SELECT StateCode,CountyCode,County,'' AS LocalKey,'' AS LocalDistrict," +
          "URL as Url,Contact,ContactEmail,AltContact,AltEmail FROM Counties" +
          " WHERE StateCode=@StateCode AND IsCountyTagForDeletion=0";
        var cmd = VoteDb.GetCommand(cmdText, 0);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        var list = table.Rows.OfType<DataRow>().ToList();

        // for each county, get the locals and query
        foreach (var c in table.Rows.OfType<DataRow>())
        {
          var localKeysClause =
            GetLocalKeysForCounty(stateCode, c.CountyCode()).SqlIn("LocalKey");
          var cmdText2 =
            $"SELECT StateCode,'{c.CountyCode()}' AS CountyCode,'{c.County()}' AS County," +
            "LocalKey,LocalDistrict,URL AS Url,Contact,ContactEmail,AltContact," +
            $"AltEmail FROM LocalDistricts WHERE StateCode=@StateCode AND {localKeysClause}" +
            " ORDER BY County,LocalDistrict";
          var cmd2 = VoteDb.GetCommand(cmdText2, 0);
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          cmd2.Connection = cn;
          var table2 = new DataTable();
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          // add to the list
          list.AddRange(table2.Rows.OfType<DataRow>());
        }

        // sort and return
        return list.OrderBy(r => r.County()).ThenBy(r => r.LocalDistrict()).ToList();
      }
    }

    public static Dictionary<string, string> GetNamesDictionary(string stateCode,
      string countyCode)
    {
      return GetLocalsForCounty(stateCode, countyCode).Rows.OfType<DataRow>()
        .ToDictionary(row => row.LocalKey(), row => row.LocalDistrict());
    }

    public static string GetAvailableLocalKey(string stateCode)
    {
      var cmdText = "SELECT MAX(LocalKey) FROM LocalDistricts WHERE StateCode=@StateCode";
      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null)
      {
        cmdText =
          "SELECT MAX(LocalKey) FROM LocalDistricts" +
          " WHERE StateCode=@StateCode AND LocalKey<'91001'";
        cmd = VoteDb.GetCommand(cmdText, 0);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        result = VoteDb.ExecuteScalar(cmd);
        if (result == null) result = "10000";
        else if ((string) result == "91000") return null;
      }
      return (int.Parse((string) result) + 1).ToString();
    }

    public static DataTable GetCountyWideLocalsForStateInCounties(string stateCode)
    {
      var cmdText =
        "SELECT d.StateCode,d.LocalKey,d.LocalDistrict,tpc.CountyCode,c.County," +
        "tpc.TigerType,tpc.TigerCode" +
        " FROM LocalDistricts d" +
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=d.StateCode AND lic.LocalKey=d.LocalKey" +
        $"  AND lic.LocalType='{LocalIdsCodes.LocalTypeVote}'" +
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=d.StateCode" +
        "  AND tpc.TigerCode=lic.LocalId AND tpc.TigerType=lic.LocalType" +
        " INNER JOIN Counties c ON c.StateCode=tpc.StateCode AND c.CountyCode=tpc.CountyCode" +
        " WHERE d.StateCode=@StateCode";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static string[] GetLocalKeysForCounty(string stateCode, string countyCode)
    {
      return GetLocalKeysForCounties(stateCode, new[] {countyCode});
    }

    public static string[] GetLocalKeysForCounties(string stateCode,
      IEnumerable<string> countyCodes)
    {
      var cmdText = Format(
        // The first SELECT handles all but county supervisors
        "SELECT d.LocalKey FROM LocalDistricts d" +
        // LocalIdsCodes maps LocalKeys to the specific type and id
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=d.StateCode AND lic.LocalKey=d.LocalKey" +
        // Get the CityCouncil row for city council type
        " LEFT OUTER JOIN CityCouncil cc ON cc.StateCode=d.StateCode AND" +
        $" lic.LocalType='{LocalIdsCodes.LocalTypeCityCouncil}' AND cc.CityCouncilCode=lic.LocalId" +
        // Get the TigerPlacesCounties row to get the county
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=d.StateCode AND" +
        // For city council, match the cc.TigerCode to a tiger place entry
        // 04-10-2019: Allow cosubs to have city council
        //$"   (tpc.TigerType='{TigerPlacesCounties.TigerTypePlace}' AND tpc.TigerCode=cc.TigerCode" +
        $"   (tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}','{TigerPlacesCounties.TigerTypeCousub}')" +
        " AND tpc.TigerCode=cc.TigerCode" +
        // For all others, match the LocalType to the TigerType and the LocalId to the Tiger Code
        // Special case: LocalTypeTiger ('T') can match TigerTypePlace ('P') or TigerTypeCousub ('C')
        $"  OR tpc.TigerType!='{TigerPlacesCounties.TigerTypeCityCouncil}' AND tpc.TigerCode=lic.LocalId" +
        $"   AND (tpc.TigerType=lic.LocalType OR tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}'," +
        $"    '{TigerPlacesCounties.TigerTypeCousub}') AND lic.LocalType='{LocalIdsCodes.LocalTypeTiger}'))" +
        // Get the Counties info
        //" INNER JOIN Counties c ON c.StateCode=tpc.StateCode AND c.CountyCode=tpc.CountyCode" +
        " WHERE d.StateCode=@StateCode AND tpc.CountyCode IN ('{0}')" +
        // 04-10-2019: eliminate dups created by this mod
        " GROUP BY d.LocalKey" +
        // Second SELECT is just for county supervisors. They are not in the TigerPlacesCounties table --
        // the CountySupervisors row contains the CountyCode
        " UNION ALL SELECT d.LocalKey FROM LocalDistricts d" +
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=d.StateCode AND lic.LocalKey=d.LocalKey" +
        // Join to CountySupervisors if LocalTypeCountySupervisors ('K')
        " INNER JOIN CountySupervisors cs ON cs.StateCode=d.StateCode AND" +
        $"  lic.LocalType='{LocalIdsCodes.LocalTypeCountySupervisors}'" +
        "   AND cs.CountySupervisorsCode=lic.LocalId" +
        //" INNER JOIN Counties c ON c.StateCode=cs.StateCode AND c.CountyCode=cs.CountyCode" +
        " WHERE d.StateCode=@StateCode AND cs.CountyCode IN ('{0}')" +
        // Third SELECT is just for school district districts. They are not in the TigerPlacesCounties 
        // table -- include it if the related school district is in the county
        " UNION ALL SELECT d.LocalKey FROM LocalDistricts d" +
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=@StateCode AND lic.LocalKey=d.LocalKey" +
        // Join to SchoolDistrictDistricts if LocalTypeSchoolDistrictDistrict ('D')
        " INNER JOIN SchoolDistrictDistricts sdd ON sdd.StateCode=@StateCode AND" +
        $"  lic.LocalType='{LocalIdsCodes.LocalTypeSchoolDistrictDistrict}'" +
        "   AND sdd.SchoolDistrictDistrictCode=lic.LocalId" +
        // Join parent school district to TigerPlacesCounties
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=@StateCode AND tpc.CountyCode IN ('{0}')" +
        "  AND tpc.TigerType=sdd.TigerType and tpc.TigerCode=sdd.TigerCode" +
        " WHERE d.StateCode=@StateCode", Join("','", countyCodes));

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r => r.LocalKey()).Distinct().ToArray();
      }
    }

    public static DataTable GetLocalsForCounty(string stateCode, string countyCode)
    {
      // Because locals are no longer directly connected to a county, this query replaces the
      // previous straightforward query to get all the locals in a county. It can also be used
      // to get all locals in a state with multi-county locals returned in multiple rows.
      var cmdText =
        // The first SELECT handles all but county supervisors
        "SELECT d.StateCode,d.LocalKey,d.LocalDistrict" + " FROM LocalDistricts d" +
        // LocalIdsCodes maps LocalKeys to the specific type and id
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=@StateCode AND lic.LocalKey=d.LocalKey" +
        // Get the CityCouncil row for city council type
        " LEFT OUTER JOIN CityCouncil cc ON cc.StateCode=@StateCode AND" +
        $" lic.LocalType='{LocalIdsCodes.LocalTypeCityCouncil}' AND cc.CityCouncilCode=lic.LocalId" +
        // Get the TigerPlacesCounties row to get the county
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=@StateCode AND" +
        // For city council, match the cc.TigerCode to a tiger place entry
        // 04-10-2019: Allow cosubs to have city council
        //$"   (tpc.TigerType='{TigerPlacesCounties.TigerTypePlace}' AND tpc.TigerCode=cc.TigerCode" +
        $"   (tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}','{TigerPlacesCounties.TigerTypeCousub}')" +
        " AND tpc.TigerCode=cc.TigerCode" +
        // For all others, match the LocalType to the TigerType and the LocalId to the Tiger Code
        // Special case: LocalTypeTiger ('T') can match TigerTypePlace ('P') or TigerTypeCousub ('C')
        $"  OR tpc.TigerType!='{TigerPlacesCounties.TigerTypeCityCouncil}' AND tpc.TigerCode=lic.LocalId" +
        $"   AND (tpc.TigerType=lic.LocalType OR tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}'," +
        $"    '{TigerPlacesCounties.TigerTypeCousub}') AND lic.LocalType='{LocalIdsCodes.LocalTypeTiger}'))" +
        // Get the Counties info
        " WHERE d.StateCode=@StateCode AND tpc.CountyCode=@CountyCode" +
        // 04-10-2019: eliminate dups created by this mod
        " GROUP BY d.StateCode,d.LocalKey" +
        // Second SELECT is just for county supervisors. They are not in the TigerPlacesCounties table --
        // the CountySupervisors row contains the CountyCode
        " UNION ALL SELECT d.StateCode,d.LocalKey,d.LocalDistrict" + " FROM LocalDistricts d" +
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=@StateCode AND lic.LocalKey=d.LocalKey" +
        // Join to CountySupervisors if LocalTypeCountySupervisors ('K')
        " INNER JOIN CountySupervisors cs ON cs.StateCode=@StateCode AND" +
        $"  lic.LocalType='{LocalIdsCodes.LocalTypeCountySupervisors}'" +
        "   AND cs.CountySupervisorsCode=lic.LocalId" +
        " WHERE d.StateCode=@StateCode AND cs.CountyCode=@CountyCode" +
        // Third SELECT is just for school district districts. They are not in the TigerPlacesCounties 
        // table -- include it if the related school district is in the county
        " UNION ALL SELECT d.StateCode,d.LocalKey,d.LocalDistrict" + " FROM LocalDistricts d" +
        " INNER JOIN LocalIdsCodes lic ON lic.StateCode=@StateCode AND lic.LocalKey=d.LocalKey" +
        // Join to SchoolDistrictDistricts if LocalTypeSchoolDistrictDistrict ('D')
        " INNER JOIN SchoolDistrictDistricts sdd ON sdd.StateCode=@StateCode AND" +
        $"  lic.LocalType='{LocalIdsCodes.LocalTypeSchoolDistrictDistrict}'" +
        "   AND sdd.SchoolDistrictDistrictCode=lic.LocalId" +
        // Join parent school district to TigerPlacesCounties
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode" +
        "  AND tpc.TigerType=sdd.TigerType and tpc.TigerCode=sdd.TigerCode" +
        " WHERE d.StateCode=@StateCode";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static List<IGrouping<string, DataRow>> GetLocalsForInclude(string stateCode,
      string countyCode)
    {
      // Exclude any that are in the selected county
      return GetCountyWideLocalsForStateInCounties(stateCode).Rows.OfType<DataRow>()
        .GroupBy(r => r.LocalKey()).Where(g => g.All(r => r.CountyCode() != countyCode))
        .OrderBy(g => g.First().LocalDistrict()).ToList();
    }

    public static List<IGrouping<string, DataRow>> GetLocalsForRemove(string stateCode,
      string countyCode)
    {
      // Only include any that are in the selected county and that are in more than 1 county
      return GetCountyWideLocalsForStateInCounties(stateCode).Rows.OfType<DataRow>()
        .GroupBy(r => r.LocalKey())
        .Where(g => g.Count() > 1 && g.Any(r => r.CountyCode() == countyCode))
        .SelectMany(g => g.Where(r => r.CountyCode() != countyCode))
        .GroupBy(r => r.LocalKey()).OrderBy(g => g.First().LocalDistrict()).ToList();
    }

    public static DataTable GetOfficesForCsv(string stateCode)
    {
      const string cmdText =
        "SELECT l.StateCode,l.LocalKey,l.LocalDistrict,o.OfficeKey,o.OfficeLine1,o.OfficeLine2," +
        "p.PoliticianKey,p.FName AS FirstName,p.MName AS MiddleName,p.Nickname,p.LName AS LastName," +
        "p.Suffix," +
        "(SELECT ElectionKey FROM ElectionsPoliticians ep" +
        " WHERE ep.OfficeKey = o.OfficeKey" +
        "  AND NOT SUBSTR(ep.ElectionKey, 11, 1) IN ('A', 'B', 'P', 'Q')" +
        " ORDER BY ep.ElectionKey DESC Limit 1) AS ElectionKey" +
        " FROM LocalDistricts l" +
        " LEFT OUTER JOIN Offices o ON o.StateCode = l.StateCode AND o.LocalKey = l.LocalKey AND" +
        " o.IsVirtual = 0 AND o.IsInactive = 0" +
        " LEFT OUTER JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
        " LEFT OUTER JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
        " WHERE l.IsLocalDistrictTagForDeletion = 0 AND l.StateCode = @StateCode";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
    public static List<SimpleListItem> GetSearchDistrictsInState(string stateCode,
      string searchString)
    {
      searchString = searchString.Trim();
      const string cmdText = "SELECT LocalKey,LocalDistrict FROM LocalDistricts" +
        " WHERE StateCode=@StateCode AND LocalDistrict LIKE @LocalDistrictMatchAny" +
        " AND IsLocalDistrictTagForDeletion=0" + 
        " ORDER BY LocalDistrict LIKE @LocalDistrictMatchStart DESC,LocalDistrict";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "LocalDistrictMatchAny", $"%{searchString}%");
      VoteDb.AddCommandParameter(cmd, "LocalDistrictMatchStart", $"{searchString}%");
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r => new SimpleListItem
        {
          Text = r.LocalDistrict(),
          Value = r.LocalKey()
        }).ToList();
      }
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string countyCode, string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, countyCode, null, null, selectedValue);
    }

    public static bool Populate(DropDownList dropDownList, string stateCode,
      string countyCode, string firstEntry, string firstEntryValue,
      string selectedValue = null, bool includeCode = false)
    {
      dropDownList.Items.Clear();

      // handle the optional first (default) entry
      if (firstEntry != null)
      {
        if (firstEntryValue == null) firstEntryValue = firstEntry;
        dropDownList.AddItem(firstEntry, firstEntryValue, firstEntryValue == selectedValue);
      }

      if (IsNullOrWhiteSpace(stateCode) || IsNullOrWhiteSpace(countyCode)) return false;
      var locals = GetNamesDictionary(stateCode, countyCode);
      if (locals.Count == 0) return false;
      var comparer = new AlphanumericComparer();
      foreach (var local in locals.OrderBy(kvp => kvp.Value, comparer))
        dropDownList.AddItem(local.Value + (includeCode ? $" ({local.Key})" : Empty),
          local.Key, local.Key == selectedValue);
      return true;
    }

    public static bool Populate(HtmlSelect dropDownList, string stateCode,
      string countyCode, string selectedValue = null)
    {
      return Populate(dropDownList, stateCode, countyCode, null, null, selectedValue);
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
        dropDownList.AddItem(firstEntry, firstEntryValue, firstEntryValue == selectedValue);
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