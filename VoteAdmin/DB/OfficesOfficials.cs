using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class OfficesOfficials
  {
    public static string GetIncumbentOfficeKeyByPoliticianKey(
      string politicianKey, int commandTimeout = -1)
    {
      var cmdText = "SELECT OfficesOfficials.OfficeKey" +
        " FROM OfficesOfficials,Offices" +
        " WHERE OfficesOfficials.PoliticianKey=@PoliticianKey" +
        "  AND OfficesOfficials.OfficeKey = Offices.OfficeKey" +
        " ORDER BY OfficesOfficials.DataLastUpdated DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      var table = FillTable(cmd, OfficesOfficialsTable.ColumnSet.OfficeKey);
      return table.Count == 0 ? null : table[0].OfficeKey;
    }

    public static string GetIncumbentOfficeKeyByRunningMateKey(
      string runningMateKey, int commandTimeout = -1)
    {
      var cmdText = "SELECT OfficesOfficials.OfficeKey" +
        " FROM OfficesOfficials,Offices" +
        " WHERE OfficesOfficials.RunningMateKey=@RunningMateKey" +
        "  AND OfficesOfficials.OfficeKey = Offices.OfficeKey" +
        " ORDER BY OfficesOfficials.DataLastUpdated DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "RunningMateKey", runningMateKey);
      var table = FillTable(cmd, OfficesOfficialsTable.ColumnSet.OfficeKey);
      return table.Count == 0 ? null : table[0].OfficeKey;
    }

    public static IEnumerable<string> GetIncumbentsByState(string stateCode, string countyCode = "",
      string localCode = "", int commandTimeout = -1)
    {
      const string cmdText = "SELECT PoliticianKey,RunningMateKey FROM OfficesOfficials" +
        " WHERE StateCode IN (@StateCode,'') AND CountyCode=@CountyCode AND LocalCode=@LocalCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalCode", localCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }
      return table.Rows.Cast<DataRow>().Select(row => row.PoliticianKey())
        .Union(table.Rows.Cast<DataRow>().Select(row => row.RunningMateKey()))
        .Where(k => stateCode.IsEqIgnoreCase(Politicians.GetStateCodeFromKey(k)));
    }

    public static DataTable GetIncumbentsForOffice(string officeKey, int commandTimeout = -1)
    {
      const string columnList =
        "oo.RunningMateKey,p.AddOn,p.Address," +
        "p.CityStateZip,p.EmailAddr AS Email,p.FName AS FirstName," +
        //"p.LDSAddress,p.LDSCityStateZip,p.LDSEmailAddr AS LDSEmail,p.LDSPhone,p.LDSWebAddr AS LDSWebAddress," + 
        "p.MName as MiddleName,p.LName AS LastName," +
        "p.Nickname,p.Phone,p.PoliticianKey,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.WebAddr AS WebAddress," +
        "o.IsRunningMateOffice,o.Incumbents,pt.PartyCode,pt.PartyName,pt.PartyUrl";

      var cmdText =
        string.Format(
          "SELECT {0}, 0 AS IsRunningMate FROM OfficesOfficials oo" +
          " INNER JOIN Politicians p ON p.PoliticianKey=oo.PoliticianKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=@OfficeKey" +
          " WHERE oo.OfficeKey=@OfficeKey" +
          " UNION SELECT {0}, 1 AS IsRunningMate FROM OfficesOfficials oo" +
          " INNER JOIN Politicians p ON p.PoliticianKey=oo.RunningMateKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=@OfficeKey" +
          " WHERE oo.OfficeKey=@OfficeKey" +
          " ORDER BY LastName,FirstName,MiddleName,Nickname,Suffix",
          columnList);

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

    public static DataTable GetPartyIncumbentsByState(string stateCode, string partyKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(a.PoliticianKey) AS AnswerCount," +
        "o.OfficeKey,oo.PoliticianKey,o.StateCode,o.CountyCode,o.LocalCode," +
        "o.OfficeLine1,o.OfficeLine2," +
        "p.DateOfBirth,p.Phone,p.StatePhone,p.Address,p.StateAddress,p.AddOn," +
        "p.FName AS FirstName,p.MName AS MiddleName,p.Nickname,p.LName AS LastName,p.Suffix," +
        "p.EmailAddr AS Email,p.StateEmailAddr AS StateEmail,p.WebAddr AS WebAddress," +
        "p.StateWebAddr AS StateWebAddress,p.BallotPediaWebAddress,p.BloggerWebAddress," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.GooglePlusWebAddress,p.LinkedInWebAddress," +
        "p.PinterestWebAddress,p.RSSFeedWebAddress,p.TwitterWebAddress,p.VimeoWebAddress," +
        "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "p.CityStateZip,p.StateCityStateZip," +
        "NOT a01.QuestionKey IS NULL AS BioGen," +
        "NOT a02.QuestionKey IS NULL AS BioPer," +
        "NOT a03.QuestionKey IS NULL AS BioPro," +
        "NOT a04.QuestionKey IS NULL AS BioCiv," +
        "NOT a05.QuestionKey IS NULL AS BioPol," +
        "NOT a06.QuestionKey IS NULL AS BioRel," +
        "NOT a07.QuestionKey IS NULL AS BioAcc," +
        "NOT a08.QuestionKey IS NULL AS BioEdu," +
        "NOT a09.QuestionKey IS NULL AS BioMil," +
        "NOT a10.QuestionKey IS NULL AS PerWhy," +
        "NOT a11.QuestionKey IS NULL AS PerGls," +
        "NOT a12.QuestionKey IS NULL AS PerAch," +
        "NOT a13.QuestionKey IS NULL AS PerCon," +
        "NOT a14.QuestionKey IS NULL AS PerPub," +
        "NOT a14.QuestionKey IS NULL AS PerOpi" +
        " FROM OfficesOfficials oo" +
        " INNER JOIN Offices o ON o.OfficeKey=oo.OfficeKey AND o.IsOfficeTagForDeletion=0" +
        " INNER JOIN Politicians p ON p.PoliticianKey=oo.PoliticianKey AND p.PartyKey=@PartyKey" +
        " LEFT OUTER JOIN Answers a01 ON a01.PoliticianKey=oo.PoliticianKey AND a01.QuestionKey='ALLBio111111'" +
        " LEFT OUTER JOIN Answers a02 ON a02.PoliticianKey=oo.PoliticianKey AND a02.QuestionKey='ALLBio222222'" +
        " LEFT OUTER JOIN Answers a03 ON a03.PoliticianKey=oo.PoliticianKey AND a03.QuestionKey='ALLBio333333'" +
        " LEFT OUTER JOIN Answers a04 ON a04.PoliticianKey=oo.PoliticianKey AND a04.QuestionKey='ALLBio444444'" +
        " LEFT OUTER JOIN Answers a05 ON a05.PoliticianKey=oo.PoliticianKey AND a05.QuestionKey='ALLBio555555'" +
        " LEFT OUTER JOIN Answers a06 ON a06.PoliticianKey=oo.PoliticianKey AND a06.QuestionKey='ALLBio666666'" +
        " LEFT OUTER JOIN Answers a07 ON a07.PoliticianKey=oo.PoliticianKey AND a07.QuestionKey='ALLBio777777'" +
        " LEFT OUTER JOIN Answers a08 ON a08.PoliticianKey=oo.PoliticianKey AND a08.QuestionKey='ALLBio888888'" +
        " LEFT OUTER JOIN Answers a09 ON a09.PoliticianKey=oo.PoliticianKey AND a09.QuestionKey='ALLBio999999'" +
        " LEFT OUTER JOIN Answers a10 ON a10.PoliticianKey=oo.PoliticianKey AND a10.QuestionKey='ALLPersonal440785'" +
        " LEFT OUTER JOIN Answers a11 ON a11.PoliticianKey=oo.PoliticianKey AND a11.QuestionKey='ALLPersonal567191'" +
        " LEFT OUTER JOIN Answers a12 ON a12.PoliticianKey=oo.PoliticianKey AND a12.QuestionKey='ALLPersonal638630'" +
        " LEFT OUTER JOIN Answers a13 ON a13.PoliticianKey=oo.PoliticianKey AND a13.QuestionKey='ALLPersonal392763'" +
        " LEFT OUTER JOIN Answers a14 ON a14.PoliticianKey=oo.PoliticianKey AND a14.QuestionKey='ALLPersonal816076'" +
        " LEFT OUTER JOIN Answers a15 ON a15.PoliticianKey=oo.PoliticianKey AND a15.QuestionKey='ALLPersonal659866'" +
        " LEFT OUTER JOIN Answers a ON a.PoliticianKey = p.PoliticianKey" +
        "  AND (SELECT IsQuestionOmit FROM Questions q WHERE q.QuestionKey=a.QuestionKey)=0" +
        "  AND (SELECT IsIssueOmit FROM Issues i WHERE i.IssueKey=a.IssueKey)=0" +
        " WHERE oo.StateCode=@StateCode" +
        " GROUP BY PoliticianKey" +
        " ORDER BY o.OfficeLevel,o.DistrictCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "PartyKey", partyKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText = "SELECT COUNT(*) FROM OfficesOfficials WHERE OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    //public static int UpdateOfficeKeyByOfficeKey(String newValue, String officeKey)
    //{
    //  const string cmdText = "UPDATE OfficesOfficials SET OfficeKey=@newValue WHERE OfficeKey=@OfficeKey";
    //  var cmd = VoteDb.GetCommand(cmdText, -1);
    //  VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //  VoteDb.AddCommandParameter(cmd, "newValue", newValue);
    //  return VoteDb.ExecuteNonQuery(cmd);
    //}
  }
}