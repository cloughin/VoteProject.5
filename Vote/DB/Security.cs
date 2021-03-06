using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class Security
  {
    public static IEnumerable<DataRow> GetUserDataForUpdate(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT s.UserSecurity,s.UserName,s.UserPassword,s.UserStateCode,s.UserCountyCode," +
        "s.UserLocalCode,c.County,l.LocalDistrict FROM Security s" +
        " LEFT OUTER JOIN Counties c ON c.StateCode=s.UserStateCode AND c.CountyCode=s.UserCountyCode" +
        " LEFT OUTER JOIN LocalDistricts l ON l.StateCode=s.UserStateCode AND" +
        " l.CountyCode=s.UserCountyCode AND l.LocalCode=s.UserLocalCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .OrderBy(r =>
          {
            switch (r.UserSecurity())
            {
              case "MASTER":
                return 1;

              case "ADMIN":
                return 2;

              case "COUNTY":
                return 3;

              case "LOCAL":
                return 4;

              default:
                return 0;
            }
          })
          .ThenBy(r => r.UserName());
      }
    }
  }
}