using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class ElectionsIncumbentsRemoved
  {
    public static List<IGrouping<string, DataRow>> GetOfficesWithCandidatesToReinstate(
      string electionKey, int commandTimeout = -1)
    {
      const string cmdText = "SELECT eir.OfficeKey,eir.PoliticianKey,o.OfficeLine1,o.OfficeLine2," +
        "p.FName as FirstName,p.MName as MiddleName,p.Nickname," +
        "p.LName as LastName,p.Suffix" +
        " FROM ElectionsIncumbentsRemoved eir" +
        " INNER JOIN Offices o ON o.OfficeKey=eir.OfficeKey" +
        " INNER JOIN Politicians p ON p.PoliticianKey=eir.PoliticianKey" +
        " WHERE eir.ElectionKey=@ElectionKey";

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
        .ToList();
    }

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM ElectionsIncumbentsRemoved WHERE OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    //public static int UpdateOfficeKeyByOfficeKey(String newValue, String officeKey)
    //{
    //  const string cmdText =
    //    "UPDATE ElectionsIncumbentsRemoved SET OfficeKey=@newValue WHERE OfficeKey=@OfficeKey";
    //  var cmd = VoteDb.GetCommand(cmdText, -1);
    //  VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //  VoteDb.AddCommandParameter(cmd, "newValue", newValue);
    //  return VoteDb.ExecuteNonQuery(cmd);
    //}
  }
}