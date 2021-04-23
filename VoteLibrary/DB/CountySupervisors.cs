using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class CountySupervisors
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static DataTable GetCountySupervisorsDistrictsByCountyCode(string stateCode, string countyCode,
      int commandTimeout = -1)
    {
      var cmdText = "SELECT cs.StateCode,cs.CountyCode,cs.CountySupervisorsCode,cs.Name," +
        "cs.IsInShapefile,lic.LocalKey FROM vote.CountySupervisors cs" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType='{LocalIdsCodes.LocalTypeCountySupervisors}' AND lic.StateCode = cs.StateCode" +
        " AND lic.localId = cs.CountySupervisorsCode" +
        " WHERE cs.StateCode = @StateCode AND cs.CountyCode = @CountyCode" +
        " GROUP BY cs.CountySupervisorsCode" +
        " ORDER BY cs.Name";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
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