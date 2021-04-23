using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class ReferendumsRow
  {
  }

  public partial class Referendums
  {
    public static DataTable GetBallotReportData(string electionKey, string countyCode, 
      string district, string place, string elementary, string secondary, string unified, 
      string cityCouncil, string countySupervisors, string schoolDistrictDistrict, int commandTimeout = -1)
    {
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

      var districtsClause = LocalIdsCodes.GetLocals(stateCode, countyCode, district, place,
        elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict)
        .SqlIn("r.LocalKey");

      var cmdText = "SELECT r.ReferendumKey,r.ReferendumTitle,r.ReferendumDesc," +
        " r.OrderOnBallot,r.CountyCode,r.LocalKey,r.ElectionKey,l.LocalDistrict" +
        " FROM Referendums r" +
        " LEFT JOIN LocalDistricts l ON l.StateCode=r.StateCode" +
        "  AND l.LocalKey=r.LocalKey" +
        " WHERE r.ElectionKey LIKE @ElectionKey" +
        $"  AND (r.CountyCode='' AND r.LocalKey='' OR r.CountyCode=@CountyCode OR {districtsClause})" +
        "  AND r.IsReferendumTagForDeletion=0 ORDER BY r.OrderOnBallot";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static Dictionary<string, string> GetCountyAndLocalElections(
      string stateElectionKey, int commandTimeout = -1)
    {
      using (var cn = VoteDb.GetOpenConnection())
      {
        var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
        const string cmdText =
          "SELECT r.CountyCode,r.ElectionKey FROM Referendums r" +
          " WHERE r.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND" +
          " r.CountyCode<>''" +
          " GROUP BY r.CountyCode";
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKeyState", stateElectionKey);
        VoteDb.AddCommandParameter(cmd, "AltElectionKey", altElectionKey);
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        var list1 =
          table.Rows.OfType<DataRow>()
            .Select(
              row => new { CountyCode = row.CountyCode(), ElectionKey = row.ElectionKey() }).ToList();

        // the second select gets locals with elections
        const string cmdText2 = "SELECT r.CountyCode,r.ElectionKey FROM Referendums r" +
          " WHERE r.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND" +
          " r.LocalKey<>''" +
          " GROUP BY r.ElectionKey";
        var cmd2 = VoteDb.GetCommand(cmdText2, commandTimeout);
        VoteDb.AddCommandParameter(cmd2, "ElectionKeyState", stateElectionKey);
        VoteDb.AddCommandParameter(cmd2, "AltElectionKey", altElectionKey);
        var table2 = new DataTable();
        cmd2.Connection = cn;
        DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
        adapter2.Fill(table2);

        // generate county election keys from the local keys and create list
        var list2 =
          table2.Rows.OfType<DataRow>()
            .SelectMany(row => Elections.GetCountyElectionKeysFromKey(row.ElectionKey()))
            .Select(e => new
            {
              CountyCode = Elections.GetCountyCodeFromKey(e),
              ElectionKey = e
            }).ToList();

        // concatenate, eliminate dups and return as dictionary
        return list1.Concat(list2)
          .GroupBy(i => i.CountyCode)
          .ToDictionary(g => g.Key, g => g.First().ElectionKey);
      }
    }

    public static DataTable GetElectionCsvReferendumData(string electionKey)
    {
      const string cmdText =
        "SELECT r.ElectionKey,r.CountyCode,r.LocalKey,r.ReferendumTitle,r.ReferendumDesc," +
        "r.ReferendumDetail,r.ReferendumDetailUrl,r.ReferendumFullText,r.ReferendumFullTextUrl," +
        "r.IsPassed,e.ElectionDesc,e.ElectionDate,c.County,l.LocalDistrict" +
        " FROM Referendums r" + 
        " INNER JOIN Elections e ON e.ElectionKey=r.ElectionKey" +
        " LEFT OUTER JOIN Counties c ON c.StateCode = r.StateCode AND c.CountyCode=r.CountyCode" +
        " LEFT OUTER JOIN LocalDistricts l ON l.StateCode = r.StateCode AND l.LocalKey=r.LocalKey" +
        " WHERE r.ElectionKeyState=@ElectionKey" + " ORDER BY r.OrderOnBallot";

      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);

        return table;
      }
    }

    public static Dictionary<string, string> GetLocalElections(string stateElectionKey, 
      string countyCode, IEnumerable<string> localKeys = null, int commandTimeout = -1)
    {
      if (localKeys == null)
        localKeys =
          LocalDistricts.GetLocalKeysForCounty(Elections.GetStateCodeFromKey(stateElectionKey),
            countyCode);
      var localKeysClause = localKeys.SqlIn("r.LocalKey");
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      var cmdText = "SELECT r.LocalKey,r.ElectionKey AS ElectionKey FROM Referendums r" +
        $" WHERE r.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND {localKeysClause}" +
        " GROUP BY r.LocalKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKeyState", stateElectionKey);
      VoteDb.AddCommandParameter(cmd, "AltElectionKey", altElectionKey);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row.LocalKey(), row => row.ElectionKey());
      }
    }


    public static ReferendumsTable GetElectionReportSummaryData(
      string electionKey, int commandTimeout = -1)
    {
      var cmdText = SelectSummaryCommandText + " WHERE ElectionKey=@ElectionKey" +
        "  AND IsReferendumTagForDeletion = 0" + " ORDER BY OrderOnBallot";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      return FillTable(cmd, ReferendumsTable.ColumnSet.Summary);
    }

    public static int GetNextOrderOnBallot(string electionKey)
    {
      var cmdText =
        "SELECT OrderOnBallot FROM Referendums WHERE ElectionKey=@ElectionKey" +
        " ORDER BY OrderOnBallot DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return result == null ? 10 : Convert.ToInt32(result) + 10;
    }

    public static string GetUniqueKey(string electionKey, string referendumTitle)
    {
      var referendumKey = electionKey + referendumTitle.StripAccents(true, true);
      if (referendumKey.Length > 64)
        referendumKey = referendumKey.Substring(0, 64);

      // Get all existing keys that match
      const string cmdText =
        "SELECT ReferendumKey FROM Referendums WHERE ReferendumKey LIKE @ReferendumKey";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ReferendumKey", referendumKey + "%");
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      // If there's no exact match, no adjustment is necessary
      // Otherwise, append integers until it's unique
      var sequence = 1;
      var uniqueKey = referendumKey;
      while (table.Rows.Cast<DataRow>()
        .FirstOrDefault(row => row.ReferendumKey()
          .IsEqIgnoreCase(uniqueKey)) != null)
        uniqueKey = referendumKey + sequence++;

      return uniqueKey;
    }
  }
}