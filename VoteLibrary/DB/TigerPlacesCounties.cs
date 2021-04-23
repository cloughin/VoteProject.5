using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class TigerPlacesCounties
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    // These are in TigerPlacesCounties only
    public const string TigerTypeCousub = "C";
    public const string TigerTypePlace = "P";

    // These are also in LocalIdsCodes
    public const string TigerTypeElementary = "E";
    public const string TigerTypeSecondary = "S";
    public const string TigerTypeUnified = "U";
    public const string TigerTypeSchoolDistrictDistrict = "D";
    public const string TigerTypeCityCouncil = "Z";
    public const string TigerTypeCountySupervisors = "K";
    public const string TigerTypeVote = "V";

    public static readonly string[] TigerTypeSchoolDistricts =
      { TigerTypeElementary, TigerTypeSecondary, TigerTypeUnified, TigerTypeSchoolDistrictDistrict };

    public static DataTable GetAllDataByStateCode(string stateCode, int commandTimeout = -1)
    {
      const string cmdText = "SELECT pc.StateCode,pc.CountyCode,pc.TigerCode,pc.TigerType," +
        "p.Name,p.LongName,p.FuncStat,p.Lsad" +
        " FROM TigerPlacesCounties pc" +
        " LEFT JOIN TigerPlaces p ON p.StateCode=pc.StateCode AND p.TigerCode=pc.TigerCode" +
        " WHERE pc.StateCode=@StateCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCousubsByCountyCode(string stateCode, string countyCode, int commandTimeout = -1)
    {
      var cmdText = "SELECT tpc.StateCode,tpc.CountyCode,tpc.TigerCode,tpc.TigerType," +
        "tp.Name,tp.LongName,lic.LocalKey" +
        " FROM vote.TigerPlacesCounties tpc" +
        " LEFT OUTER JOIN TigerPlacesCounties tpc2 ON tpc2.StateCode=tpc.StateCode" +
        "   AND tpc2.CountyCode=tpc.CountyCode AND tpc2.TigerCode=tpc.TigerCode" +
        $"   AND tpc2.TigerType='{TigerTypePlace}'" +
        " INNER JOIN TigerPlaces tp ON tp.StateCode=tpc.StateCode AND tp.TigerCode=tpc.TigerCode" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType='{LocalIdsCodes.LocalTypeTiger}'" +
        " AND lic.StateCode=tpc.StateCode" +
        //"  AND lic.localId=tpc.TigerCode AND lic.CountyCode=tpc.CountyCode" +
        "  AND lic.localId=tpc.TigerCode" +
        $" WHERE tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode AND tpc.TigerType='{TigerTypeCousub}'" +
        " AND NOT tp.Lsad IN ('22','57') AND tpc2.StateCode IS NULL" +
        " ORDER BY tp.LongName";

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

    public static DataTable GetPlacesByCountyCode(string stateCode, string countyCode, int commandTimeout = -1)
    {
      var cmdText = "SELECT tpc.StateCode,tpc.CountyCode,tpc.TigerCode,tpc.TigerType," +
        "tp.Name,tp.LongName,lic.localKey" +
        " FROM vote.TigerPlacesCounties tpc" +
        " INNER JOIN TigerPlaces tp ON tp.StateCode=tpc.StateCode AND tp.TigerCode=tpc.TigerCode" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType='{LocalIdsCodes.LocalTypeTiger}'" +
        " AND lic.StateCode=tpc.StateCode" +
        //"  AND lic.localId=tpc.TigerCode AND lic.CountyCode=tpc.CountyCode" +
        "  AND lic.localId=tpc.TigerCode" +
        $" WHERE tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode AND tpc.TigerType='{TigerTypePlace}'" +
        " AND NOT tp.Lsad IN ('22','57')" +
        " ORDER BY tp.LongName";

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

    public static DataTable GetTigerSchoolDistrictsByCountyCode(string stateCode, string countyCode,
      int commandTimeout = -1)
    {
      var cmdText = "SELECT tpc.StateCode,tpc.CountyCode,tpc.TigerCode,tpc.TigerType," +
        "ts.Name,lic.LocalKey" +
        " FROM vote.TigerPlacesCounties tpc" +
        " INNER JOIN TigerSchools ts ON ts.StateCode=tpc.StateCode AND ts.TigerCode=tpc.TigerCode" +
        "  AND ts.TigerType=tpc.TigerType" +
        " LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType=tpc.TigerType" +
        "  AND lic.StateCode=tpc.StateCode" +
        //"  AND lic.localId=tpc.TigerCode AND lic.CountyCode=tpc.CountyCode" +
        "  AND lic.localId=tpc.TigerCode" +
        " WHERE tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode AND tpc.TigerType IN" +
        $" ('{TigerTypeElementary}','{TigerTypeSecondary}','{TigerTypeUnified}')" +
        " UNION ALL SELECT tpc.StateCode,tpc.CountyCode,sdd.SchoolDistrictDistrictCode AS TigerCode," +
        $"'{TigerTypeSchoolDistrictDistrict}' AS TigerType,sdd.Name,lic.LocalKey" +
        " FROM vote.TigerPlacesCounties tpc" +
        " INNER JOIN SchoolDistrictDistricts sdd ON sdd.StateCode=tpc.StateCode" +
        "  AND sdd.TigerCode=tpc.TigerCode AND sdd.TigerType=tpc.TigerType" +
        $" LEFT OUTER JOIN LocalIdsCodes lic ON lic.LocalType='{TigerTypeSchoolDistrictDistrict}'" +
        "  AND lic.StateCode=tpc.StateCode" +
        //"  AND lic.localId=sdd.SchoolDistrictDistrictCode AND lic.CountyCode=tpc.CountyCode" +
        "  AND lic.localId=sdd.SchoolDistrictDistrictCode" +
        " WHERE tpc.StateCode=@StateCode AND tpc.CountyCode=@CountyCode AND tpc.TigerType IN" +
        $" ('{TigerTypeElementary}','{TigerTypeSecondary}','{TigerTypeUnified}')" +
        " ORDER BY Name";

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

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}