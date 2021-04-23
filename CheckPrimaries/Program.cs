using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using DB;
using DB.Vote;
using MySql.Data.MySqlClient;

namespace CheckPrimaries
{
  static class Program
  {

    public static DataTable GetData()
    {
      const string cmdText = "SELECT ep.ElectionKey,o.OfficeLevel,e.ElectionDesc" +
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKeyState" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
        " WHERE e.ElectionType = 'P'";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    private static void Main()
    {
      var data = GetData().Rows.OfType<DataRow>();
      var primaries = data
        .GroupBy(r => new
        {
          ElectionKey = Elections.GetStateElectionKeyFromKey(r.ElectionKey()),
          OfficeLevel = r.OfficeLevel()
        }).Select(g => new {g.Key, ElectionDescription = g.First().ElectionDescription()})
        .GroupBy(e => e.Key.ElectionKey).Where(g =>
          !g.Any(i => new[] {1, 2, 3, 4, 5, 6}.Contains(i.Key.OfficeLevel))).ToList();
      foreach (var e in primaries)
        Console.WriteLine($"{e.Key} {e.First().ElectionDescription}");
      Console.ReadKey();
    }
  }
}
