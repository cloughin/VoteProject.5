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
    public static DataTable GetBallotReportData(string electionKey,
      string countyCode, int commandTimeout = -1)
    {
      electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

      const string cmdText = "SELECT r.ReferendumKey,r.ReferendumTitle,r.ReferendumDesc," +
        " r.OrderOnBallot,r.CountyCode,r.LocalCode,r.ElectionKey,l.LocalDistrict" +
        " FROM Referendums r" +
        " LEFT JOIN LocalDistricts l ON l.StateCode=r.StateCode" +
        "  AND l.CountyCode=r.CountyCode AND l.LocalCode=r.LocalCode" +
        " WHERE r.ElectionKey LIKE @ElectionKey" +
        "  AND (r.CountyCode='' OR r.CountyCode=@CountyCode)" +
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
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      const string cmdText =
        "SELECT r.CountyCode,r.ElectionKeyCounty AS ElectionKey FROM Referendums r" +
        " WHERE r.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND r.CountyCode<>''" +
        " GROUP BY r.CountyCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKeyState", stateElectionKey);
      VoteDb.AddCommandParameter(cmd, "AltElectionKey", altElectionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row["CountyCode"] as string,
            row => row["ElectionKey"] as string);
      }
    }

    public static Dictionary<string, string> GetLocalElections(
      string stateElectionKey, string countyCode, int commandTimeout = -1)
    {
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      const string cmdText = "SELECT r.LocalCode,r.ElectionKey AS ElectionKey FROM Referendums r" +
        " WHERE r.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND r.CountyCode=@CountyCode AND r.LocalCode<>''" +
        " GROUP BY r.LocalCode";
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
          .ToDictionary(row => row["LocalCode"] as string,
            row => row["ElectionKey"] as string);
      }
    }

    //public static DataTable GetExplorerData(string electionKey,
    //  string countyCode, int commandTimeout = -1)
    //{
    //  electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

    //  const string cmdText = "SELECT ElectionKey,ReferendumKey,ReferendumTitle," +
    //    "IsResultRecorded,IsPassed,ReferendumDesc,OrderOnBallot,CountyCode,LocalCode" +
    //    " FROM Referendums r" +
    //    " WHERE ElectionKey LIKE @ElectionKey" +
    //    "  AND (r.CountyCode='' OR r.CountyCode=@CountyCode)" +
    //    "  AND r.IsReferendumTagForDeletion=0 ORDER BY r.OrderOnBallot";

    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
    //    VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}


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
        .FirstOrDefault(row => row.PoliticianKey()
          .IsEqIgnoreCase(uniqueKey)) != null)
        uniqueKey = referendumKey + sequence++;

      return uniqueKey;
    }
  }
}