using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class OfficesRow
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

    public OfficeClass OfficeClass
    {
      get { return OfficeLevel.ToOfficeClass(); }
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

  public partial class Offices
  {
    #region Private

    private static string BuildElectedReportWhereClause(string stateCode,
      string countyCode, string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, bool forRunningMateSelect, bool includeLocals)
    {
      string where;

      if (stateCode == "DC")
        where =
          string.Format(
            "(o.OfficeLevel={1} OR o.StateCode='{0}' AND (o.OfficeLevel IN ({2},{3},{4}) OR o.OfficeLevel={5} AND o.DistrictCode={6}))",
            stateCode, OfficeClass.USPresident.ToInt(),
            OfficeClass.USSenate.ToInt(), OfficeClass.USHouse.ToInt(),
            OfficeClass.StateWide.ToInt(), OfficeClass.StateSenate.ToInt(),
            stateSenateDistrict);
      else
        where =
          string.Format(
            "(o.OfficeLevel={1} OR o.StateCode='{0}' AND " + "(o.OfficeLevel={2}" +
            " OR o.OfficeLevel={3} AND o.DistrictCode='{8}'" +
            " OR o.OfficeLevel={4}" +
            " OR o.OfficeLevel={5} AND o.DistrictCode='{9}'" +
            " OR o.OfficeLevel={6} AND o.DistrictCode='{10}'" +
            " OR o.CountyCode='{7}' {11}))", stateCode,
            OfficeClass.USPresident.ToInt(), OfficeClass.USSenate.ToInt(),
            OfficeClass.USHouse.ToInt(), OfficeClass.StateWide.ToInt(),
            OfficeClass.StateSenate.ToInt(), OfficeClass.StateHouse.ToInt(),
            countyCode, congressionalDistrict, stateSenateDistrict,
            stateHouseDistrict, includeLocals ? string.Empty : "AND o.LocalCode=''");

      return
        $" {(forRunningMateSelect ? "oo.RunningMateKey <> '' AND" : string.Empty)}" +
        $" o.IsInactive=0 AND o.IsOnlyForPrimaries=0 AND {where}";
    }

    private const string IncumbentReportCmdTextTemplate =
      "SELECT {0}, 0 AS IsRunningMate FROM OfficesOfficials oo" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
      " INNER JOIN Offices o ON o.OfficeKey=oo.OfficeKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
      " WHERE oo.OfficeKey=@OfficeKey" +
      " UNION SELECT {0}, 1 AS IsRunningMate FROM OfficesOfficials oo" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.RunningMateKey" +
      " INNER JOIN Offices o ON o.OfficeKey=oo.OfficeKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
      " WHERE oo.OfficeKey=@OfficeKey" +
      " ORDER BY IsRunningMate,AddOn,LastName,FirstName";

    private const string IncumbentReportColumnList =
      "oo.OfficeKey,oo.RunningMateKey,p.AddOn AS AddOn,p.FName AS FirstName," +
      "p.LName AS LastName,p.MName AS MiddleName,p.Nickname,p.PoliticianKey," +
      "p.Suffix,o.IsRunningMateOffice,pt.PartyCode";

    private const string OfficialsReportCmdTextTemplate =
      "SELECT {0}, 0 AS IsRunningMate FROM Offices o" +
      " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
      " LEFT JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey WHERE {1}" +
      " UNION SELECT {0}, 1 AS IsRunningMate FROM Offices o" +
      " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.RunningMateKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey WHERE {2}";

    private const string OfficialsReportColumnList =
      "o.AlternateOfficeLevel,o.CountyCode,o.DistrictCode,o.Incumbents," +
      "o.IsRunningMateOffice,o.IsVacant,o.LocalCode,o.OfficeKey,o.OfficeLevel," +
      "o.OfficeLine1,o.OfficeLine2,o.OfficeOrderWithinLevel,o.StateCode," +
      "oo.PoliticianKey AS OfficialsPoliticianKey,oo.RunningMateKey,p.AddOn," +
      "p.Address,p.BallotPediaWebAddress,p.BloggerWebAddress,p.CityStateZip,p.DateOfBirth," +
      "p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
      "p.FName AS FirstName,p.GooglePlusWebAddress," +
      //"p.LDSAddress,p.LDSCityStateZip,p.LDSEmailAddr AS LDSEmail,p.LDSPhone,p.LDSWebAddr AS LDSWebAddress," +
      "p.LinkedInWebAddress,p.LName AS LastName," +
      "p.MName AS MiddleName,p.Nickname,p.PartyKey,p.Phone," +
      "p.PinterestWebAddress,p.PoliticianKey,p.RSSFeedWebAddress," +
      "p.StateAddress,p.StateCityStateZip,p.StateEmailAddr AS StateEmail," +
      "p.StatePhone,p.StateWebAddr AS StateWebAddress,p.Suffix," +
      "p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr AS WebAddress," +
      "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
      "pt.PartyCode,pt.PartyName,pt.PartyUrl";

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static int CountAllCountyOffices(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Offices" +
        " WHERE StateCode=@StateCode AND CountyCode<>'' AND LocalCode=''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountAllLocalOffices(string stateCode, string countyCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Offices" +
        " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND LocalCode<>''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static Dictionary<string, int> CountCountyOfficesByCounty(
      string stateCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT CountyCode,COUNT(*) AS Count FROM Offices" +
        " WHERE StateCode=@StateCode AND CountyCode<>'' AND LocalCode=''" +
        " GROUP BY CountyCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row["CountyCode"] as string,
            row => Convert.ToInt32(row["Count"]));
      }
    }

    public static Dictionary<string, int> CountLocalOfficesByLocal(
      string stateCode, string countyCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT LocalCode,COUNT(*) AS Count FROM Offices" +
        " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND LocalCode<>''" +
        " GROUP BY LocalCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row["LocalCode"] as string,
            row => Convert.ToInt32(row["Count"]));
      }
    }

    public static string GetDistrictItem(string stateCode,
      OfficeClass officeClass, string districtCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT DistrictCode,OfficeLine1,OfficeLine2 FROM Offices" +
        " WHERE StateCode=@StateCode AND OfficeLevel=@OfficeLevel" +
        " AND DistrictCode=@DistrictCode";

      if (districtCode.Length < 3)
        districtCode = districtCode.ZeroPad(3);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
      VoteDb.AddCommandParameter(cmd, "DistrictCode", districtCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        if (table.Rows.Count == 0) return string.Empty;
        var row = table.Rows[0];
        return string.IsNullOrWhiteSpace(row.OfficeLine2())
          ? row.OfficeLine1()
          : row.OfficeLine2();
      }
    }

    public static IEnumerable<SimpleListItem> GetDistrictItems(string stateCode,
      OfficeClass officeClass, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT DistrictCode,OfficeLine1,OfficeLine2 FROM Offices" +
        " WHERE StateCode=@StateCode AND OfficeLevel=@OfficeLevel" +
        " ORDER BY DistrictCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .Select(row => new SimpleListItem
          {
            Value = row.DistrictCode(),
            Text = string.IsNullOrWhiteSpace(row.OfficeLine2())
              ? row.OfficeLine1()
              : row.OfficeLine2()
          });
      }
    }

    public static DataTable GetElectedReportData(string stateCode,
      string countyCode, string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, bool includeLocals, int commandTimeout = -1)
    {
      var cmdText = string.Format(OfficialsReportCmdTextTemplate,
        OfficialsReportColumnList,
        BuildElectedReportWhereClause(stateCode, countyCode, congressionalDistrict,
          stateSenateDistrict, stateHouseDistrict, false, includeLocals),
        BuildElectedReportWhereClause(stateCode, countyCode, congressionalDistrict,
          stateSenateDistrict, stateHouseDistrict, true, includeLocals));

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static IEnumerable<OfficesRow> GetGeneralElectionOffices(int commandTimeout = -1)
    {
      const string cmdText = "SELECT OfficeKey,OfficeLevel,StateCode,DistrictCode FROM Offices" +
        " WHERE CountyCode='' AND LocalCode='' AND OfficeLevel<4 AND IsSpecialOffice=0";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, OfficesTable.ColumnSet.GeneralElection);
    }

    public static DataTable GetIncumbentReportData(string officeKey,
      int commandTimeout = -1)
    {
      var cmdText = string.Format(IncumbentReportCmdTextTemplate,
        IncumbentReportColumnList);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static Dictionary<string, string> GetLocalNamesWithOffices(
      string stateCode, string countyCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT o.LocalCode,l.LocalDistrict FROM Offices o" +
        " INNER JOIN LocalDistricts l ON l.StateCode = o.StateCode" +
        "  AND l.CountyCode = o.CountyCode AND l.localCode = o.LocalCode" +
        " WHERE o.StateCode=@StateCode AND o.CountyCode=@CountyCode AND o.LocalCode<>''" +
        " GROUP BY o.LocalCode" + " ORDER BY l.LocalDistrict";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row["LocalCode"] as string,
            row => row["LocalDistrict"] as string);
      }
    }

    public static OfficesTable GetOfficesByClass(string stateCode, string countyCode,
      string localCode,
      OfficeClass officeClass, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT OfficeKey,StateCode,CountyCode,LocalCode,DistrictCode,OfficeLine1," +
        "OfficeLine2,OfficeLevel,IsRunningMateOffice,Incumbents,IsVacant FROM Offices" +
        " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND LocalCode=@LocalCode AND OfficeLevel=@OfficeLevel AND IsVirtual=0" +
        " ORDER BY OfficeLine1,OfficeLine2";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalCode", localCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
      return FillTable(cmd, OfficesTable.ColumnSet.Cache);
    }

    public static OfficesTable GetOfficeTemplatesByClass(string stateCode, OfficeClass officeClass,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT OfficeKey,StateCode,CountyCode,LocalCode,DistrictCode,OfficeLine1," +
        "OfficeLine2,OfficeLevel,IsRunningMateOffice,Incumbents,IsVacant FROM Offices" +
        " WHERE StateCode=@StateCode AND OfficeLevel=@OfficeLevel AND IsVirtual=1" +
        " ORDER BY OfficeLine1,OfficeLine2";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
      return FillTable(cmd, OfficesTable.ColumnSet.Cache);
    }

    public static DataTable GetOfficialsReportData(OfficialsReportOptions options,
      int commandTimeout = -1)
    {
      var cmdText = string.Format(OfficialsReportCmdTextTemplate,
        OfficialsReportColumnList, options.BuildWhereClause(false),
        options.BuildWhereClause(true));

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}