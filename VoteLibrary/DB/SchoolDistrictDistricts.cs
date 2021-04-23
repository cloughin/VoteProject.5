using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class SchoolDistrictDistricts
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    [NotNull]
    public static DataTable GetSchoolDistrictDistrictsByTigerTypeTigerCode([NotNull] string stateCode,
      [NotNull] string tigerType, [NotNull] string tigerCode, int commandTimeout = -1)
    {
      var cmdText = "SELECT sdd.StateCode,sdd.SchoolDistrictDistrictCode,sdd.Name," +
        "sdd.IsInShapefile,lic.LocalKey" +
        " FROM SchoolDistrictDistricts sdd" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType='{LocalIdsCodes.LocalTypeSchoolDistrictDistrict}'" +
        " AND lic.StateCode = sdd.StateCode" +
        " AND lic.localId = sdd.SchoolDistrictDistrictCode" +
        " WHERE sdd.StateCode = @StateCode AND sdd.TigerType=@TigerType AND sdd.TigerCode = @TigerCode" +
        " ORDER BY sdd.SchoolDistrictDistrictCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "TigerType", tigerType);
        VoteDb.AddCommandParameter(cmd, "TigerCode", tigerCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [CanBeNull]
    public static string GetPrefix([NotNull] string stateCode, [NotNull] string tigerType, 
      [NotNull] string tigerCode)
    {
      const string cmdText = "SELECT SUBSTR(SchoolDistrictDistrictCode,1,3) AS Prefix" +
        " FROM SchoolDistrictDistricts" +
        " WHERE StateCode=@StateCode AND TigerType=@TigerType AND TigerCode=@TigerCode" +
        " GROUP BY SUBSTR(SchoolDistrictDistrictCode, 1, 3)";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "TigerType", tigerType);
      VoteDb.AddCommandParameter(cmd, "TigerCode", tigerCode);
      return VoteDb.ExecuteScalar(cmd) as string;
    }

    [NotNull]
    public static List<string> GetPrefixes([NotNull] string stateCode)
    {
      const string cmdText = "SELECT SUBSTR(SchoolDistrictDistrictCode,1,3) AS Prefix" +
        " FROM SchoolDistrictDistricts WHERE StateCode=@StateCode" +
        " GROUP BY SUBSTR(SchoolDistrictDistrictCode, 1,3) ORDER BY Prefix";

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