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
  public partial class ElectionsOffices
  {
    public static Dictionary<string, string> GetCountyAndLocalElections(
      string stateElectionKey, bool allowEmptyOffices = false, int commandTimeout = -1)
    {
      using (var cn = VoteDb.GetOpenConnection())
      {
        // the first select just gets counties with elections
        var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
        var cmdText =
          "SELECT eo.CountyCode,eo.ElectionKey FROM ElectionsOffices eo" +
          (allowEmptyOffices
            ? Empty
            : " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey") +
          " WHERE eo.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND" +
          " eo.CountyCode<>''" +
          " GROUP BY eo.CountyCode";
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKeyState", stateElectionKey);
        VoteDb.AddCommandParameter(cmd, "AltElectionKey", altElectionKey);
        var table = new DataTable();
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        var list1 =
          table.Rows.OfType<DataRow>()
            .Select(
              row => new {CountyCode = row.CountyCode(), ElectionKey = row.ElectionKey()}).ToList();

        // the second select gets locals with elections
        var cmdText2 =
          "SELECT eo.CountyCode,eo.ElectionKey FROM ElectionsOffices eo" +
          (allowEmptyOffices
            ? Empty
            : " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey") +
          " WHERE eo.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND" +
          " eo.LocalKey<>''" +
          " GROUP BY eo.ElectionKey";
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

    public static DataTable GetElectionOffices(string electionKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT o.OfficeKey,o.DistrictCode," + 
        //"o.DistrictCodeAlpha," + 
        "o.OfficeLevel," +
        "o.OfficeLine1,o.OfficeLine2,o.OfficeOrderWithinLevel" +
        " FROM ElectionsOffices eo" +
        " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
        " WHERE eo.ElectionKey=@ElectionKey ORDER BY o.OfficeLevel," +
        "o.DistrictCode,o.OfficeOrderWithinLevel," +
        //"o.DistrictCodeAlpha," +
        "o.OfficeLine1,o.OfficeLine2";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static string GetLatestViewableElectionKeyStateByOfficeKey(
      string officeKey, [CanBeNull] string defaultValue = null)
    {
      var cmdText = "SELECT ElectionsOffices.ElectionKeyState" +
        " FROM Elections,ElectionsOffices" +
        " WHERE ElectionsOffices.OfficeKey=@OfficeKey" +
        "  AND Elections.IsViewable='1'" +
        " ORDER BY ElectionsOffices.ElectionKeyState DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static Dictionary<string, string> GetLocalElections(string stateElectionKey, 
      string countyCode, IEnumerable<string> localKeys = null, bool allowEmptyOffices = false, 
      int commandTimeout = -1)
    {
      if (localKeys == null)
        localKeys =
          LocalDistricts.GetLocalKeysForCounty(Elections.GetStateCodeFromKey(stateElectionKey),
            countyCode);
      var localKeysClause = localKeys.SqlIn("eo.LocalKey");
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      var cmdText =
        "SELECT eo.LocalKey,eo.ElectionKey FROM ElectionsOffices eo" +
        (allowEmptyOffices
          ? Empty
          : " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey") +
        $" WHERE eo.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND {localKeysClause}" +
        " GROUP BY eo.LocalKey";
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

    public static IDictionary<string, int> GetOfficeContestsByYear()
    {
      const string cmdText = "SELECT SUBSTR(eo.ElectionKey,3,4) AS Year FROM ElectionsOffices eo" +
        " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey = eo.ElectionKey AND ep.OfficeKey = eo.OfficeKey" +
        " GROUP BY ep.ElectionKey,eo.OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .GroupBy(r => r.Year())
          .ToDictionary(g => g.Key, g => g.Count());
      }
    }

    public static List<IGrouping<string, DataRow>> GetOfficesWithCandidatesToEliminate(
      string electionKey, int commandTimeout = -1)
    {
      const string cmdText = "SELECT eo.OfficeKey,oo.PoliticianKey,o.OfficeLine1," +
        "o.OfficeLine2,o.Incumbents,o.ElectionPositions,p.FName as FirstName," +
        "p.MName as MiddleName,p.Nickname,p.LName as LastName,p.Suffix" +
        " FROM ElectionsOffices eo" +
        // Only include offices that do not re-elect all incumbents at the same time
        " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey AND" +
        " o.Incumbents>o.ElectionPositions" +
        // Generate a row for each current incumbent
        " INNER JOIN OfficesOfficials oo ON oo.OfficeKey=eo.OfficeKey" +
        // Get the incumbent details
        " INNER JOIN Politicians p ON p.PoliticianKey=oo.PoliticianKey" +
        // Determine if the incumbent is in the selected election
        " LEFT OUTER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey" +
        " AND ep.OfficeKey= eo.OfficeKey AND ep.PoliticianKey=oo.PoliticianKey" +
        // Include incumbent if not in current election or if there are any winners marked
        // in the election
        " WHERE eo.ElectionKey=@ElectionKey AND (ep.IsWinner IS NULL" +
        " OR 0 <> (SELECT COUNT(*) FROM ElectionsPoliticians" +
        " WHERE ElectionKey=@ElectionKey AND OfficeKey=eo.OfficeKey" +
        " AND PoliticianKey<>oo.PoliticianKey AND IsWinner=1))";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      return table.AsEnumerable()
        .GroupBy(r => r.OfficeKey())
        //.Where(g => g.First().Incumbents() - g.Count() < g.First().ElectionPositions())
        .Where(g => g.Count() > g.First().Incumbents() - g.First().ElectionPositions())
        .ToList();
    }

    public static DateTime GetOldAnswerCutoffDate(string electionKey, string officeKey)
    {
      return VotePage.DefaultDbDate; // to disable date pruning per RAK 2020/9/30
    }

    //public static DateTime GetOldAnswerCutoffDate(string electionKey, string officeKey)
    //{
    //  var likeElectionKey = $"{electionKey.Substring(0, 2)}________{electionKey.Substring(10)}";
    //  //const string cmdText = "SELECT e.ElectionDate FROM ElectionsOffices eo" +
    //  //  " INNER JOIN Elections e ON e.ElectionKey=eo.ElectionKey" +
    //  //  " WHERE eo.OfficeKey=@OfficeKey AND e.ElectionKey LIKE @LikeElectionKey" +
    //  //  " ORDER BY e.ElectionDate DESC LIMIT 2";
    //  const string cmdText = "SELECT e.ElectionDate FROM ElectionsPoliticians ep" +
    //    " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
    //    " WHERE ep.OfficeKey=@OfficeKey AND e.ElectionKey LIKE @LikeElectionKey" +
    //    " GROUP BY e.ElectionDate ORDER BY e.ElectionDate DESC LIMIT 2";

    //  var cmd = VoteDb.GetCommand(cmdText);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //    VoteDb.AddCommandParameter(cmd, "LikeElectionKey", likeElectionKey);
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    if (table.Rows.Count < 2)
    //      return DateTime.UtcNow.AddDays(-1461).Date; // subtract 4 years including leap day
    //    return table.Rows[1].ElectionDate().Date;
    //  }
    //}

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText = "SELECT COUNT(*) FROM ElectionsOffices WHERE OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }
  }
}