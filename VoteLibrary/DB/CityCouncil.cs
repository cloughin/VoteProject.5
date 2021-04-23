using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class CityCouncil
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    [NotNull]
    public static DataTable GetCityCouncilDistrictsByCountyCode([NotNull] string stateCode,
      [NotNull] string countyCode, int commandTimeout = -1)
    {
      var cmdText = "SELECT cc.StateCode,tpc.CountyCode,cc.CityCouncilCode,cc.Name," +
        "lic.LocalKey FROM vote.CityCouncil cc" +
        " INNER JOIN TigerPlacesCounties tpc ON tpc.StateCode = cc.StateCode AND" +
        " tpc.TigerCode = cc.TigerCode AND tpc.TigerType IN" +
        $" ('{TigerPlacesCounties.TigerTypePlace}','{TigerPlacesCounties.TigerTypeCousub}')" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType = '{LocalIdsCodes.LocalTypeCityCouncil}'" +
        " AND lic.StateCode = tpc.StateCode" +
        " AND lic.localId = cc.CityCouncilCode" +
        " WHERE cc.StateCode = @StateCode AND tpc.CountyCode = @CountyCode" +
        " GROUP BY cc.CityCouncilCode" +
        " ORDER BY cc.Name";

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

    [NotNull]
    public static DataTable GetCityCouncilDistrictsByTigerCode([NotNull] string stateCode,
      [NotNull] string tigerCode, int commandTimeout = -1)
    {
      var cmdText = "SELECT cc.StateCode,cc.CityCouncilCode,cc.District,cc.Name," +
        "cc.IsInShapefile,lic.LocalKey" +
        " FROM vote.CityCouncil cc" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType = '{LocalIdsCodes.LocalTypeCityCouncil}'" +
        " AND lic.StateCode = cc.StateCode" +
        " AND lic.localId = cc.CityCouncilCode" +
        " WHERE cc.StateCode = @StateCode AND cc.TigerCode = @TigerCode" +
        " ORDER BY cc.CityCouncilCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "TigerCode", tigerCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [CanBeNull]
    public static string GetPrefix([NotNull] string stateCode, [NotNull] string tigerCode)
    {
      const string cmdText = "SELECT SUBSTR(CityCouncilCode,1,2) AS Prefix" +
        " FROM vote.CityCouncil" +
        " WHERE StateCode=@StateCode AND TigerCode=@TigerCode" +
        " GROUP BY SUBSTR(CityCouncilCode, 1, 2)";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "TigerCode", tigerCode);
      return VoteDb.ExecuteScalar(cmd) as string;
    }

    [NotNull]
    public static List<string> GetPrefixes([NotNull] string stateCode)
    {
      const string cmdText = "SELECT SUBSTR(CityCouncilCode,1,2) AS Prefix" +
        " FROM vote.CityCouncil WHERE StateCode=@StateCode" +
        " GROUP BY SUBSTR(CityCouncilCode, 1, 2) ORDER BY Prefix";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r => r.Prefix()).ToList();
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