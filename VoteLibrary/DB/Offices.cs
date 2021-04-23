using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class Offices
  {
    #region Private

    private static string BuildElectedReportWhereClause(string stateCode,
      string countyCode, string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, bool forRunningMateSelect, string[] localKeys)
    {
      string where;

      if (stateCode == "DC")
        where =
          $"(o.OfficeLevel={OfficeClass.USPresident.ToInt()} OR o.StateCode='{stateCode}'" +
          $" AND (o.OfficeLevel IN ({OfficeClass.USSenate.ToInt()},{OfficeClass.USHouse.ToInt()}," +
          $"{OfficeClass.StateWide.ToInt()},{OfficeClass.StateJudicial.ToInt()}," +
          $"{OfficeClass.StateParty.ToInt()})" +
          $" OR o.OfficeLevel={OfficeClass.StateSenate.ToInt()}" +
          $" AND o.DistrictCode={stateSenateDistrict}))";
      else
        where =
          $"(o.OfficeLevel={OfficeClass.USPresident.ToInt()} OR o.StateCode='{stateCode}' AND " +
          $"(o.OfficeLevel={OfficeClass.USSenate.ToInt()}" +
          $" OR o.OfficeLevel={OfficeClass.USHouse.ToInt()} AND o.DistrictCode='{congressionalDistrict}'" +
          $" OR o.OfficeLevel IN ({OfficeClass.StateWide.ToInt()},{OfficeClass.StateJudicial.ToInt()}," +
          $"{OfficeClass.StateParty.ToInt()})" +
          $" OR o.OfficeLevel={OfficeClass.StateSenate.ToInt()} AND o.DistrictCode='{stateSenateDistrict}'" +
          $" OR o.OfficeLevel={OfficeClass.StateHouse.ToInt()} AND o.DistrictCode='{stateHouseDistrict}'" +
          $" OR o.CountyCode='{countyCode}'" +
          $" {(localKeys == null ? Empty : "OR " + localKeys.SqlIn("o.LocalKey"))}))";

      return
        $" {(forRunningMateSelect ? "oo.RunningMateKey <> '' AND" : Empty)}" +
        $" o.IsInactive=0 AND o.IsOnlyForPrimaries=0 AND {where}";
    }

    private const string IncumbentReportCmdTextTemplate =
      "SELECT {0}, 0 AS IsRunningMate FROM OfficesOfficials oo" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
      " INNER JOIN Offices o ON o.OfficeKey=oo.OfficeKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
      " WHERE oo.OfficeKey=@OfficeKey" +
      " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM OfficesOfficials oo" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.RunningMateKey" +
      " INNER JOIN Offices o ON o.OfficeKey=oo.OfficeKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
      " WHERE oo.OfficeKey=@OfficeKey" +
      " ORDER BY IsRunningMate,AddOn,LastName,FirstName";

    private const string IncumbentReportColumnList =
      "oo.OfficeKey,oo.RunningMateKey,p.AddOn AS AddOn,p.FName AS FirstName," +
      "p.LName AS LastName,p.MName AS MiddleName,p.Nickname,p.PoliticianKey," +
      "p.Suffix,o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice,pt.PartyCode";

    private const string OfficialsReportCmdTextTemplate =
      "SELECT {0}, 0 AS IsRunningMate FROM Offices o" +
      " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
      " LEFT JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey WHERE {1}" +
      " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM Offices o" +
      " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
      " INNER JOIN Politicians p ON p.PoliticianKey = oo.RunningMateKey" +
      " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey WHERE {2}";

    private const string OfficialsReportColumnList =
      "o.AlternateOfficeLevel,o.CountyCode,o.DistrictCode,o.Incumbents," +
      "o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice,o.IsVacant,o.LocalKey,o.OfficeKey,o.OfficeLevel," +
      "o.OfficeLine1,o.OfficeLine2,o.OfficeOrderWithinLevel,o.StateCode," +
      "oo.PoliticianKey AS OfficialsPoliticianKey,oo.RunningMateKey,p.AddOn," +
      "p.Address,p.BallotPediaWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
      "p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip,p.DateOfBirth," +
      "p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
      "p.FName AS FirstName,p.GooglePlusWebAddress," +
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

    public static int CountAllActualOffices(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Offices" +
        " WHERE IsVirtual=0";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static decimal GetAdRate(string electionKey, string officeKey)
    {
      var adRates = GetAdRates(officeKey);
      return Elections.IsPrimaryElection(electionKey)
        ? adRates.PrimaryAdRate
        : adRates.GeneralAdRate;
    }

    public static (decimal GeneralAdRate, decimal PrimaryAdRate) GetAdRates(string officeKey)
    {
      const string cmdText =
        "SELECT IF(o.GeneralAdRate=0,oc.GeneralAdRate,o.GeneralAdRate) AS GeneralAdRate," +
        "IF(o.PrimaryAdRate=0,oc.PrimaryAdRate,o.PrimaryAdRate) AS PrimaryAdRate" +
        " FROM Offices o" +
        " INNER JOIN OfficeClasses oc ON oc.OfficeLevel = o.OfficeLevel" +
        " AND oc.AlternateOfficeLevel = IF(o.AlternateOfficeLevel=0,99,o.AlternateOfficeLevel)" +
        " WHERE o.OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return (table.Rows[0].GeneralAdRate(), table.Rows[0].PrimaryAdRate());
      }
    }

    public static (decimal GeneralAdRate, decimal PrimaryAdRate) GetDefaultAdRates(string officeKey)
    {
      const string cmdText =
        "SELECT oc.GeneralAdRate,oc.PrimaryAdRate FROM Offices o" +
        " INNER JOIN OfficeClasses oc ON oc.OfficeLevel = o.OfficeLevel" +
        " AND oc.AlternateOfficeLevel = IF(o.AlternateOfficeLevel=0,99,o.AlternateOfficeLevel)" +
        " WHERE o.OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return (table.Rows[0].GeneralAdRate(), table.Rows[0].PrimaryAdRate());
      }
    }

    public static Dictionary<string, bool> HasCountyOrLocalOfficesByCounty(
      string stateCode, int commandTimeout = -1)
    {
      using (var cn = VoteDb.GetOpenConnection())
      {
        // first create a dictionary of all counties in the state
        var dictionary = CountyCache.GetCountiesByState(stateCode)
          .ToDictionary(c => c, c => false);

        // the first query just gets the county offices
        const string cmdText =
          "SELECT CountyCode,COUNT(*) AS Count FROM Offices" +
          " WHERE StateCode=@StateCode AND CountyCode<>''" +
          " GROUP BY CountyCode";
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        // counties tagged for deletion won't be in the table
        foreach (var c in table.Rows.OfType<DataRow>())
          if (dictionary.ContainsKey(c.CountyCode()) && c.Count() > 0)
            dictionary[c.CountyCode()] = true;

        // we only have to query locals for counties that are still false
        foreach (var c in dictionary.Where(kvp => !kvp.Value).Select(kvp => kvp.Key).ToList())
        {
          // we need to do a pre-query to get all the locals in the selected county
          var localKeysClause = LocalDistricts.GetLocalKeysForCounty(stateCode, c)
            .SqlIn("LocalKey");
          var cmdText2 = "SELECT COUNT(*) AS Count FROM Offices" +
            $" WHERE StateCode=@StateCode AND {localKeysClause}";
          var cmd2 = VoteDb.GetCommand(cmdText2, commandTimeout);
          VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
          cmd2.Connection = cn;
          if (Convert.ToInt32(cmd2.ExecuteScalar()) > 0) 
            dictionary[c] = true;
        }

        // only return the trues
        return dictionary.Where(kvp => kvp.Value)
          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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
        if (table.Rows.Count == 0) return Empty;
        var row = table.Rows[0];
        return IsNullOrWhiteSpace(row.OfficeLine2())
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
            Text = IsNullOrWhiteSpace(row.OfficeLine2())
              ? row.OfficeLine1()
              : row.OfficeLine2()
          });
      }
    }

    public static DataTable GetElectedReportData(string stateCode,
      string countyCode, string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict, bool includeLocals, 
      int commandTimeout = -1)
    {
      var localKeys = includeLocals
        ? LocalIdsCodes.GetLocals(stateCode, countyCode, district, place, elementary,
          secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict)
        : null;

      var cmdText = Format(OfficialsReportCmdTextTemplate,
        OfficialsReportColumnList,
        BuildElectedReportWhereClause(stateCode, countyCode, congressionalDistrict,
          stateSenateDistrict, stateHouseDistrict, false, localKeys),
        BuildElectedReportWhereClause(stateCode, countyCode, congressionalDistrict,
          stateSenateDistrict, stateHouseDistrict, true, localKeys));

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
        " WHERE CountyCode=''" +
        " AND LocalKey=''" +
        " AND OfficeLevel<4 AND IsSpecialOffice=0";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, OfficesTable.ColumnSet.GeneralElection);
    }

    public static DataTable GetIncumbentReportData(string officeKey,
      int commandTimeout = -1)
    {
      var cmdText = Format(IncumbentReportCmdTextTemplate,
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
      // we need to do a pre-query to get all the locals in the selected county
      var localKeysClause =
        LocalDistricts.GetLocalKeysForCounty(stateCode, countyCode).SqlIn("o.LocalKey");
      var cmdText = "SELECT o.LocalKey,l.LocalDistrict FROM Offices o" +
        " INNER JOIN LocalDistricts l ON l.StateCode = o.StateCode" +
        "  AND l.localKey = o.LocalKey" +
        $" WHERE o.StateCode=@StateCode AND {localKeysClause}" + " GROUP BY o.LocalKey" +
        " ORDER BY l.LocalDistrict";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(row => row.LocalKey(), row => row.LocalDistrict());
      }
    }

    public static OfficesTable GetOfficesByClass(string stateCode, string countyCode,
      string localKey, OfficeClass officeClass, int commandTimeout = -1)
    {
      if (IsNullOrWhiteSpace(localKey))
      {
        const string cmdText =
          "SELECT OfficeKey,StateCode,CountyCode,LocalKey,DistrictCode,OfficeLine1," +
          "OfficeLine2,OfficeLevel,IsRunningMateOffice,IsPrimaryRunningMateOffice,Incumbents,IsVacant FROM Offices" +
          " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND LocalKey='' AND OfficeLevel=@OfficeLevel AND IsVirtual=0" +
          " ORDER BY OfficeLine1,OfficeLine2";
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
        return FillTable(cmd, OfficesTable.ColumnSet.Cache);
      }
      else
      {
        const string cmdText =
          "SELECT OfficeKey,StateCode,CountyCode,LocalKey,DistrictCode,OfficeLine1," +
          "OfficeLine2,OfficeLevel,IsRunningMateOffice,IsPrimaryRunningMateOffice,Incumbents,IsVacant FROM Offices" +
          " WHERE StateCode=@StateCode AND LocalKey=@LocalKey AND OfficeLevel=@OfficeLevel AND IsVirtual=0" +
          " ORDER BY OfficeLine1,OfficeLine2";
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeClass.ToInt());
        return FillTable(cmd, OfficesTable.ColumnSet.Cache);
      }
    }

    public static OfficesTable GetOfficeTemplatesByClass(string stateCode, OfficeClass officeClass,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT OfficeKey,StateCode,CountyCode,LocalKey,DistrictCode,OfficeLine1," +
        "OfficeLine2,OfficeLevel,IsRunningMateOffice,IsPrimaryRunningMateOffice,Incumbents,IsVacant FROM Offices" +
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
      var cmdText = Format(OfficialsReportCmdTextTemplate,
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