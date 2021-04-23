using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class LocalIdsCodes
  {
    #region Private

    private static string BuildWhereClauseForOtherTypes(IEnumerable<DataRow> otherTypes)
    {
      var items = new List<string>();
      foreach (var ot in otherTypes)
      {
        if (ot.LocalType() == LocalTypeTiger)
          items.Add($"tpc.TigerCode='{ot.LocalId()}' AND tpc.TigerType IN" +
            $" ('{TigerPlacesCounties.TigerTypePlace}', '{TigerPlacesCounties.TigerTypeCousub}')");
        else
          items.Add($"tpc.TigerCode='{ot.LocalId()}' AND tpc.TigerType='{ot.LocalType()}'");
      }
      return Join(" OR ", items);
    }

    private static string UseAnd(string s)
    {
      var inx = s.LastIndexOf(", ", StringComparison.OrdinalIgnoreCase);
      if (inx >= 0) s = $"{s.Substring(0, inx)} and {s.Substring(inx + 2)}";
      return s;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    // This is in LocalIdsCodes only
    public const string LocalTypeTiger = "T";

    // These are also in TigerPlacesCounties
    public const string LocalTypeVote = TigerPlacesCounties.TigerTypeVote;
    public const string LocalTypeElementary = TigerPlacesCounties.TigerTypeElementary;
    public const string LocalTypeSecondary = TigerPlacesCounties.TigerTypeSecondary;
    public const string LocalTypeUnified = TigerPlacesCounties.TigerTypeUnified;
    public const string LocalTypeSchoolDistrictDistrict = TigerPlacesCounties.TigerTypeSchoolDistrictDistrict;
    public const string LocalTypeCityCouncil = TigerPlacesCounties.TigerTypeCityCouncil;
    public const string LocalTypeCountySupervisors = TigerPlacesCounties.TigerTypeCountySupervisors;

    public static string[] FindCounties(string stateCode, string localKey)
    {
      var counties = FindCountiesWithNames(stateCode, new[] { localKey });
      if (counties.Count == 0) return new string[0];
      return counties.First().Value.Select(i => i.Value).ToArray();
    }

    public static SimpleListItem[] FindCountiesWithNames(string stateCode, string localKey)
    {
      var counties = FindCountiesWithNames(stateCode, new[] { localKey });
      return counties.Count == 0 ? new SimpleListItem[0] : counties.First().Value;
    }

    public static Dictionary<string, SimpleListItem[]> FindCountiesWithNames(string stateCode, 
      IEnumerable<string> localKeys)
    {
      using (var cn = VoteDb.GetOpenConnection())
      {
        // first get the LocalIdsCodes entries
        var cmdText1 = "SELECT LocalKey,LocalType,LocalId FROM LocalIdsCodes" +
          $" WHERE StateCode=@StateCode AND {localKeys.SqlIn("LocalKey")}";
        var table1 = new DataTable();
        var cmd1 = VoteDb.GetCommand(cmdText1);
        cmd1.Connection = cn;
        VoteDb.AddCommandParameter(cmd1, "StateCode", stateCode);
        DbDataAdapter adapter1 = new MySqlDataAdapter(cmd1 as MySqlCommand);
        adapter1.Fill(table1);
        var rows1 = table1.Rows.OfType<DataRow>().ToArray();

        // separate by LocalType
        var cityCouncils = rows1.Where(r => r.LocalType() == LocalTypeCityCouncil)
          .ToArray();
        var countySupervisors = rows1
          .Where(r => r.LocalType() == LocalTypeCountySupervisors).ToArray();
        var schoolDistrictDistricts = rows1
          .Where(r => r.LocalType() == LocalTypeSchoolDistrictDistrict).ToArray();
        var otherTypes = rows1.Where(r => 
          r.LocalType() != LocalTypeCityCouncil &&
          r.LocalType() != LocalTypeCountySupervisors &&
          r.LocalType() != LocalTypeSchoolDistrictDistrict).ToArray();

        var result = new Dictionary<string, SimpleListItem[]>();

        if (cityCouncils.Length > 0)
        {
          // look up TigerCode from CityCouncil
          var cmdText2 =
            "SELECT cc.CityCouncilCode,c.CountyCode,c.County FROM CityCouncil cc" +
            " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=@StateCode AND" +
            $" tpc.TigerType='{TigerPlacesCounties.TigerTypePlace}' AND tpc.TigerCode=cc.TigerCode" +
            " INNER JOIN Counties c ON c.StateCode=@StateCode AND c.CountyCode=tpc.CountyCode" +
            " WHERE cc.StateCode=@StateCode AND" + 
            $" {cityCouncils.Select(cc => cc.LocalId()).SqlIn("cc.CityCouncilCode")}";
          var table2 = new DataTable();
          var cmd2 = VoteDb.GetCommand(cmdText2);
          cmd2.Connection = cn;
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          var rows2 = table2.Rows.OfType<DataRow>().ToList();
          foreach (var cc in cityCouncils)
          {
            result.Add(cc.LocalKey(),
              rows2.Where(r => r.CityCouncilCode() == cc.LocalId())
                .Select(r => new SimpleListItem {Value = r.CountyCode(), Text = r.County()})
                .OrderBy(i => i.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray());
          }
        }

        if (countySupervisors.Length > 0)
        {
          // look up county from CountySupervisors
          var cmdText2 =
            "SELECT cs.CountySupervisorsCode,c.CountyCode,c.County FROM CountySupervisors cs" +
            " INNER JOIN Counties c ON c.StateCode=@StateCode AND c.CountyCode=cs.CountyCode" +
            " WHERE cs.StateCode=@StateCode AND" + 
            $" {countySupervisors.Select(cs => cs.LocalId()).SqlIn("cs.CountySupervisorsCode")}";
          var table2 = new DataTable();
          var cmd2 = VoteDb.GetCommand(cmdText2);
          cmd2.Connection = cn;
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          var rows2 = table2.Rows.OfType<DataRow>().ToList();
          foreach (var cs in countySupervisors)
          {
            result.Add(cs.LocalKey(),
              rows2.Where(r => r.CountySupervisorsCode() == cs.LocalId())
                .Select(r => new SimpleListItem { Value = r.CountyCode(), Text = r.County() })
                .OrderBy(i => i.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray());
          }
        }

        if (schoolDistrictDistricts.Length > 0)
        {
          // for type SchoolDistrictsDistricts, first get the parent type and code
          // from SchoolDistrictDistricts then get the county from TigerPlacesCounties
          var cmdText2 = "SELECT sdd.SchoolDistrictDistrictCode,c.CountyCode,c.County" +
            " FROM SchoolDistrictDistricts sdd" +
            " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode=@StateCode AND" +
            "  tpc.TigerType=sdd.TigerType AND tpc.TigerCode=sdd.TigerCode" +
            " INNER JOIN Counties c ON c.StateCode=@StateCode AND c.CountyCode=tpc.CountyCode" +
            " WHERE sdd.StateCode=@StateCode AND" + 
            $" {schoolDistrictDistricts.Select(sdd => sdd.LocalId()).SqlIn("sdd.SchoolDistrictDistrictCode")}";
          var table2 = new DataTable();
          var cmd2 = VoteDb.GetCommand(cmdText2);
          cmd2.Connection = cn;
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          var rows2 = table2.Rows.OfType<DataRow>().ToList();
          foreach (var sdd in schoolDistrictDistricts)
          {
            result.Add(sdd.LocalKey(),
              rows2.Where(r => r.SchoolDistrictDistrictCode() == sdd.LocalId())
                .Select(r => new SimpleListItem { Value = r.CountyCode(), Text = r.County() })
                .OrderBy(i => i.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray());
          }
        }

        if (otherTypes.Length > 0)
        {
          // get counties from TigerPlacesCounties
          var cmdText2 = "SELECT tpc.TigerType,tpc.TigerCode,c.CountyCode,c.County" +
            " FROM TigerPlacesCounties tpc" +
            " INNER JOIN Counties c ON c.StateCode=@StateCode AND c.CountyCode=tpc.CountyCode" +
            $" WHERE tpc.StateCode=@StateCode AND ({BuildWhereClauseForOtherTypes(otherTypes)})";
          var table2 = new DataTable();
          var cmd2 = VoteDb.GetCommand(cmdText2);
          cmd2.Connection = cn;
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          var rows2 = table2.Rows.OfType<DataRow>().ToList();
          foreach (var ot in otherTypes)
          {
            result.Add(ot.LocalKey(),
              rows2.Where(r => r.TigerCode() == ot.LocalId() && (r.TigerType() == ot.LocalType() ||
                (r.TigerType() == TigerPlacesCounties.TigerTypePlace || r.TigerType() == TigerPlacesCounties.TigerTypeCousub)
                && ot.LocalType() == LocalTypeTiger))
                .Select(r => new SimpleListItem { Value = r.CountyCode(), Text = r.County() })
                // this is because if there is a 'P' AND a 'C', there will be two entries
                .GroupBy(i => i.Value)
                .Select(g => g.First())
                .OrderBy(i => i.Text, StringComparer.OrdinalIgnoreCase)
                .ToArray());
          }
        }

        return result;
      }
    }

    public static string FormatAllCountyNames(string stateCode, string localKey,
      bool useAnd = false)
    {
      return FormatOtherCountyNamesDictionary(stateCode, null, new[] { localKey }, useAnd)
        .First().Value;
    }

    public static string FormatCountyNames(IEnumerable<SimpleListItem> item,
      bool useAnd = false, [CanBeNull] string countyCodeToExclude = null)
    {
      var names = item.Where(c => c.Value != countyCodeToExclude)
        .OrderBy(c => c.Text, StringComparer.OrdinalIgnoreCase).ToArray();
      var countyNames = names.Length == 0
        ? Empty
        : Join(", ", names.Select(c => c.Text));
      if (useAnd) countyNames = UseAnd(countyNames);
      return countyNames;
    }

    public static Dictionary<string, string> FormatCountyNamesDictionary(
      [NotNull] IDictionary<string, SimpleListItem[]> counties, bool useAnd = false,
      [CanBeNull] string countyCodeToExclude = null)
    {
      var dictionary = new Dictionary<string, string>();
      foreach (var kvp in counties)
        dictionary.Add(kvp.Key, FormatCountyNames(kvp.Value, useAnd, countyCodeToExclude));
      return dictionary;
    }

    public static string FormatMultiCountyNames(string stateCode, string localKey,
      bool useAnd = false)
    {
      var counties = FindCountiesWithNames(stateCode, new[] { localKey });
      if (counties.Count != 1 || counties.First().Value.Length < 2) return Empty;
      return FormatCountyNamesDictionary(counties, useAnd).First().Value;
    }

    public static string FormatOtherCountyNames(string stateCode, string countyCode,
      string localKey, bool useAnd = false)
    {
      return FormatOtherCountyNames(stateCode, countyCode, localKey, out _, useAnd);
    }

    public static string FormatOtherCountyNames(string stateCode, string countyCode,
      string localKey, out int countyCount, bool useAnd = false)
    {
      var counties = FindCountiesWithNames(stateCode, new[] { localKey });
      countyCount = counties.First().Value.Length;
      return FormatCountyNamesDictionary(counties, useAnd, countyCode).First().Value;
    }

    public static Dictionary<string, string> FormatOtherCountyNames(string stateCode, string countyCode,
      IEnumerable<string> localKeys, bool useAnd = false)
    {
      var counties = FindCountiesWithNames(stateCode, localKeys);
      return FormatCountyNamesDictionary(counties, useAnd, countyCode);
    }

    public static Dictionary<string, string> FormatOtherCountyNamesDictionary(string stateCode, 
      string countyCode, IEnumerable<string> localKeys, bool useAnd = false)
    {
      var counties = FindCountiesWithNames(stateCode, localKeys);
      return FormatCountyNamesDictionary(counties, useAnd, countyCode);
    }

    public static DataTable GetLocalData(string stateCode, string localKey,
      int commandTimeout = -1)
    {
      var cmdText = "SELECT lic.StateCode,lic.LocalType," +
        "lic.LocalId,tpc.TigerType FROM LocalIdsCodes lic" +
        " LEFT OUTER JOIN TigerPlacesCounties tpc ON tpc.StateCode=lic.StateCode AND" +
        " tpc.TigerCode=lic.LocalId AND" +
        " (tpc.TigerType=lic.LocalType OR" +
        $"   tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}','{TigerPlacesCounties.TigerTypeCousub}')" +
        $" AND lic.LocalType='{LocalTypeTiger}')" +
        " WHERE lic.StateCode=@StateCode AND lic.LocalKey=@LocalKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static string[] GetLocals(string stateCode, string countyCode, string district,
      string place, string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict)
    {
      // gets an array of localKeys based on all of the shapefile lookups
      // for ballots and elected officials
      var cmdText = "SELECT Distinct lic.LocalKey FROM LocalIdsCodes lic" +
        // for LocalTypeCityCouncil (Z), get CityCouncil row
        " LEFT OUTER JOIN CityCouncil cc ON" +
        $"  cc.StateCode=@StateCode AND lic.LocalType='{LocalTypeCityCouncil}'" +
        "    AND cc.CityCouncilCode=lic.LocalId" +
        // for LocalTypeCountySupervisors (K), get CountySupervisors row
        " LEFT OUTER JOIN CountySupervisors cs ON" +
        $"  cs.StateCode=@StateCode AND lic.LocalType='{LocalTypeCountySupervisors}'" +
        "    AND cs.CountySupervisorsCode=lic.LocalId" +
        // for LocalTypeSchoolDistrictDistrict (D), get SchoolDistrictDistricts row
        " LEFT OUTER JOIN SchoolDistrictDistricts sdd ON" +
        $"  sdd.StateCode=@StateCode AND lic.LocalType='{LocalTypeSchoolDistrictDistrict}'" +
        "    AND sdd.SchoolDistrictDistrictCode=lic.LocalId" +
        // use TigerPlacesCounties for county-based validation
        " LEFT OUTER JOIN TigerPlacesCounties tpc ON" +
        "   tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode AND" +
        // for LocalTypeTiger (T) [= TigerTypeCousub (C) or TigerTypePlace (P)], match on district or place
        $"  (tpc.TigerType IN('{TigerPlacesCounties.TigerTypeCousub}'," +
        $"    '{TigerPlacesCounties.TigerTypePlace}') AND lic.LocalType='{LocalTypeTiger}'" +
        "     AND tpc.TigerCode=lic.LocalId AND lic.LocalId IN(@District, @Place)" +
        // for LocalTypeUnified (U), match on unified
        $"   OR tpc.TigerType=lic.LocalType AND lic.LocalType='{LocalTypeUnified}'" +
        "     AND tpc.TigerCode=lic.LocalId  AND lic.LocalId=@Unified" +
        // for LocalTypeElementary (E), match on elementary
        $"   OR tpc.TigerType=lic.LocalType AND lic.LocalType='{LocalTypeElementary}'" +
        "     AND tpc.TigerCode=lic.LocalId  AND lic.LocalId=@Elementary" +
        // for LocalTypeSecondary (S), match on secondary
        $"   OR tpc.TigerType=lic.LocalType AND lic.LocalType='{LocalTypeSecondary}'" +
        "     AND tpc.TigerCode=lic.LocalId  AND lic.LocalId=@Secondary" +
        // for LocalTypeVote (V) [county-wide], just match the county
        $"   OR tpc.TigerType=lic.LocalType AND lic.LocalType='{LocalTypeVote}'" +
        "     AND tpc.TigerCode=lic.LocalId" +
        // for LocalTypeCityCouncil (Z), match to a TigerTypePlace (P) entry with a matching TigerCode,
        // but only if not in shapefile (in-shapefile will match cityCouncil below)
        $"   OR tpc.TigerType='{TigerPlacesCounties.TigerTypePlace}'" +
        $"    AND lic.LocalType='{LocalTypeCityCouncil}' AND tpc.TigerCode=cc.TigerCode" +
        "     AND cc.IsInShapefile=0" +
        // for LocalTypeSchoolDistrictDistrict (D), match to the parent school district entry,
        // but only if not in shapefile (in-shapefile will match schoolDistrictDistrict, below)
        $"   OR tpc.TigerType IN('{TigerPlacesCounties.TigerTypeUnified}'," +
        $"    '{TigerPlacesCounties.TigerTypeSecondary}', '{TigerPlacesCounties.TigerTypeElementary}')" +
        $"    AND lic.LocalType='{LocalTypeSchoolDistrictDistrict}' AND tpc.TigerCode=sdd.TigerCode" +
        "     AND tpc.TigerType=sdd.TigerType AND sdd.IsInShapefile=0)" +
        // only search selected state
        " WHERE lic.StateCode=@StateCode AND (" +
        // include if TigerPlacesCounties matched
        "   NOT tpc.StateCode IS NULL OR" +
        // include if CityCouncil matched and the CityCouncilCode is selected from the shapefile
        "   NOT cc.StateCode IS NULL AND lic.LocalId=@CityCouncil OR" +
        // include if CountySupervisors matched and the CityCouncilCode is selected from the shapefile
        //  or if it's not in the shapefile and the county matches
        "   NOT cs.StateCode IS NULL AND (lic.LocalId=@CountySupervisors" +
        "    OR cs.IsInShapefile=0 AND cs.CountyCode=@CountyCode) OR" +
        // include if SchoolDistrictDistricts matched and the SchoolDistrictDistrictCode is selected from the shapefile
        "   NOT sdd.StateCode IS NULL AND lic.LocalId=@SchoolDistrictDistrict)";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "District", district);
      VoteDb.AddCommandParameter(cmd, "Place", place);
      VoteDb.AddCommandParameter(cmd, "Elementary", elementary);
      VoteDb.AddCommandParameter(cmd, "Secondary", secondary);
      VoteDb.AddCommandParameter(cmd, "Unified", unified);
      VoteDb.AddCommandParameter(cmd, "CityCouncil", cityCouncil);
      VoteDb.AddCommandParameter(cmd, "CountySupervisors", countySupervisors);
      VoteDb.AddCommandParameter(cmd, "SchoolDistrictDistrict", schoolDistrictDistrict);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(r => r.LocalKey())
          .ToArray();
      }
    }

    public static string GetNextVoteIdForState(string stateCode)
    {
      const string cmdText = "SELECT MAX(LocalId) FROM LocalIdsCodes" +
        " WHERE StateCode=@StateCode AND LocalType='V'";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var max = VoteDb.ExecuteScalar(cmd) as string;
      switch (max)
      {
        case null:
          return "10001"; // the first

        case "99999":
          throw new Exception($"Out of Vote id numbers for state {stateCode}");

        default:
          // ReSharper disable once AssignNullToNotNullAttribute
          return (int.Parse(max) + 1).ToString();
      }
    }

    public static DataTable GetOtherCountyReferences(string stateCode, string countyCode,
      string localKey, int commandTimeout = -1)
    {
      var cmdText = "SELECT c.StateCode,c.CountyCode,c.County" +
        " FROM LocalIdsCodes lic" +
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode = lic.StateCode" +
        "  AND tpc.TigerCode = lic.LocalId" + 
        " INNER JOIN Counties c ON c.StateCode = tpc.StateCode AND c.CountyCode = tpc.CountyCode" +
        "  AND (tpc.TigerType = lic.LocalType OR" +
        $" tpc.TigerType IN ('{TigerPlacesCounties.TigerTypePlace}', '{TigerPlacesCounties.TigerTypeCousub}')" +
        $" AND lic.LocalType = '{LocalTypeTiger}')" +
        " WHERE lic.StateCode = @StateCode AND lic.LocalKey = @LocalKey" +
        " AND tpc.CountyCode != @CountyCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}