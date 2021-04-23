using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class ElectionsOffices
  {
    //public static Dictionary<string, string> GetCountyElections(
    //  string stateElectionKey, int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT CountyCode,ElectionKey FROM ElectionsOffices" +
    //    " WHERE ElectionKeyState=@ElectionKeyState AND CountyCode<>'' AND LocalCode=''" +
    //    " GROUP BY CountyCode";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "ElectionKeyState", stateElectionKey);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table.Rows.OfType<DataRow>()
    //      .ToDictionary(row => row["CountyCode"] as string,
    //        row => row["ElectionKey"] as string);
    //  }
    //}

    public static Dictionary<string, string> GetCountyAndLocalElections(
      string stateElectionKey, bool allowEmptyOffices = false, int commandTimeout = -1)
    {
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      var cmdText =
        "SELECT eo.CountyCode,eo.ElectionKeyCounty AS ElectionKey FROM ElectionsOffices eo" +
        (allowEmptyOffices
          ? string.Empty
          : " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey") +
        " WHERE eo.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND eo.CountyCode<>''" +
        " GROUP BY eo.CountyCode";
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

    public static DataTable GetElectionOffices(string electionKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT o.OfficeKey,o.DistrictCode,o.DistrictCodeAlpha,o.OfficeLevel," +
        "o.OfficeLine1,o.OfficeLine2,o.OfficeOrderWithinLevel" +
        " FROM ElectionsOffices eo" +
        " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
        " WHERE eo.ElectionKey=@ElectionKey ORDER BY o.OfficeLevel," +
        "o.DistrictCode,o.OfficeOrderWithinLevel,o.DistrictCodeAlpha," +
        "o.OfficeLine1";

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
      string officeKey, string defaultValue = null)
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
      if ((result == null) || (result == DBNull.Value)) return defaultValue;
      return result as string;
    }

    public static Dictionary<string, string> GetLocalElections(
      string stateElectionKey, string countyCode, bool allowEmptyOffices = false,
      int commandTimeout = -1)
    {
      var altElectionKey = Elections.GetElectionKeyToInclude(stateElectionKey);
      var cmdText =
        "SELECT eo.LocalCode,eo.ElectionKey FROM ElectionsOffices eo" +
        (allowEmptyOffices
          ? string.Empty
          : " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey") +
        " WHERE eo.ElectionKeyState IN (@ElectionKeyState,@AltElectionKey) AND eo.CountyCode=@CountyCode AND eo.LocalCode<>''" +
        " GROUP BY eo.LocalCode";
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

    public static List<IGrouping<string, DataRow>> GetOfficesWithCandidatesToEliminate(
      string electionKey, int commandTimeout = -1)
    {
      const string cmdText = "SELECT eo.OfficeKey,oo.PoliticianKey,o.OfficeLine1," +
        "o.OfficeLine2,o.Incumbents,o.ElectionPositions,p.FName as FirstName," +
        "p.MName as MiddleName,p.Nickname,p.LName as LastName,p.Suffix" +
        " FROM ElectionsOffices eo" +
        " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey AND" +
        " o.Incumbents>o.ElectionPositions" +
        " INNER JOIN OfficesOfficials oo ON oo.OfficeKey=eo.OfficeKey" +
        " INNER JOIN Politicians p ON p.PoliticianKey=oo.PoliticianKey" +
        " LEFT OUTER JOIN ElectionsPoliticians ep ON ep.ElectionKey=eo.ElectionKey" +
        " AND ep.OfficeKey= eo.OfficeKey AND ep.PoliticianKey=oo.PoliticianKey" +
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

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText = "SELECT COUNT(*) FROM ElectionsOffices WHERE OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    //public static int UpdateOfficeKeyByOfficeKey(String newValue, String officeKey)
    //{
    //  const string cmdText = "UPDATE ElectionsOffices SET OfficeKey=@newValue WHERE OfficeKey=@OfficeKey";
    //  var cmd = VoteDb.GetCommand(cmdText, -1);
    //  VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //  VoteDb.AddCommandParameter(cmd, "newValue", newValue);
    //  return VoteDb.ExecuteNonQuery(cmd);
    //}
  }
}