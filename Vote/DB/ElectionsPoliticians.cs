using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class ElectionsPoliticians
  {
    #region Private

    private const int DaysAfterElectionToShowAsUpcoming = 30;

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    private const string IssueColumnList =
      "a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeSource," +
      "a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime," +
      "a.YouTubeAutoDisable,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder," +
      "ep.ElectionKey,ep.OfficeKey";

    public static DataTable GetCompareCandidateIssues(string electionKey, string officeKey,
      string questionKey = null, int commandTimeout = -1)
    {
      //const string columnList =
      //  "a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeSource," +
      //  "a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime," +
      //  "a.YouTubeAutoDisable,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder";

      var cmdText =
        string.Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.PoliticianKey {1}" +
          "  AND (TRIM(a.Answer)<>''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          "  AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
          " UNION SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.RunningMateKey {1}" +
          "  AND (TRIM(a.Answer) <> ''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          "  AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
          " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnList,
          questionKey == null ? string.Empty : "AND a.QuestionKey=@QuestionKey");

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        if (questionKey != null)
          VoteDb.AddCommandParameter(cmd, "QuestionKey", questionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCompareCandidateVideos(string electionKey, string officeKey,
      string politicianKey, int commandTimeout = -1)
    {
      //const string columnList =
      //  "a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeSource," +
      //  "a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime," +
      //  "a.YouTubeAutoDisable,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder";

      var cmdText =
        string.Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          "  AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
          "  AND ep.PoliticianKey=@PoliticianKey" +
          "  AND NOT a.YouTubeUrl IS NULL AND a.YouTubeUrl!=''" +
          "  AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
          " UNION SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          "  AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
          "  AND ep.PoliticianKey=@PoliticianKey" +
          "  AND NOT a.YouTubeUrl IS NULL AND a.YouTubeUrl!=''" +
          "  AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
          " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnList);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static OfficeKeyInfo GetFutureOfficeKeyInfoByPoliticianKey(
      string politicianKey, bool isViewable, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionsPoliticians.ElectionKey,ElectionsPoliticians.OfficeKey" +
        " FROM ElectionsPoliticians,Elections" +
        " WHERE ElectionsPoliticians.PoliticianKey=@PoliticianKey" +
        "  AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey" +
        "  AND Elections.ElectionDate>=@Today" +
        "  AND Elections.IsViewable=@IsViewable" +
        " ORDER BY Elections.ElectionDate";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      VoteDb.AddCommandParameter(cmd, "Today",
        DateTime.Today.AddDays(-DaysAfterElectionToShowAsUpcoming));
      VoteDb.AddCommandParameter(cmd, "IsViewable", isViewable);
      var table = FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.OfficeKey);
      if (table.Count == 0) return OfficeKeyInfo.Empty;
      return new OfficeKeyInfo
      {
        ElectionKey = table[0].ElectionKey,
        OfficeKey = table[0].OfficeKey
      };
    }

    public static OfficeKeyInfo GetFutureOfficeKeyInfoByRunningMateKey(
      string runningMateKey, bool isViewable, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionsPoliticians.ElectionKey,ElectionsPoliticians.OfficeKey" +
        " FROM ElectionsPoliticians,Elections" +
        " WHERE ElectionsPoliticians.RunningMateKey=@RunningMateKey" +
        "  AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey" +
        "  AND Elections.ElectionDate>=@Today" +
        "  AND Elections.IsViewable=@IsViewable" +
        " ORDER BY Elections.ElectionDate";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "RunningMateKey", runningMateKey);
      VoteDb.AddCommandParameter(cmd, "Today",
        DateTime.Today.AddDays(-DaysAfterElectionToShowAsUpcoming));
      VoteDb.AddCommandParameter(cmd, "IsViewable", isViewable);
      var table = FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.OfficeKey);
      if (table.Count == 0) return OfficeKeyInfo.Empty;
      return new OfficeKeyInfo
      {
        ElectionKey = table[0].ElectionKey,
        OfficeKey = table[0].OfficeKey
      };
    }

    public static List<IGrouping<string, DataRow>> GetOfficesWithCandidatesToEliminate(
      string electionKey, int commandTimeout = -1)
    {
      const string cmdText = "SELECT ep.OfficeKey,ep.PoliticianKey,o.OfficeLine1," +
        "o.OfficeLine2,o.Incumbents,o.ElectionPositions,p.FName as FirstName," +
        "p.MName as MiddleName,p.Nickname,p.LName as LastName,p.Suffix" +
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey AND" +
        "  o.Incumbents>o.ElectionPositions" +
        " INNER JOIN OfficesOfficials oo ON oo.OfficeKey=ep.OfficeKey AND oo.PoliticianKey=ep.PoliticianKey" +
        " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
        " WHERE ep.ElectionKey=@ElectionKey" +
        " ORDER BY ep.OfficeKey,ep.OrderOnBallot";

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
        .Where(g => g.First().Incumbents() - g.Count() < g.First().ElectionPositions())
        .ToList();
    }

    public static DataTable GetPartyCandidatesInElections(IEnumerable<string> electionKeys,
      string partyKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT COUNT(a.PoliticianKey) AS AnswerCount," +
        "ep.ElectionKey,ep.OfficeKey,ep.PoliticianKey,ep.StateCode,ep.CountyCode,ep.LocalCode," +
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
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey AND o.OfficeKey != 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey AND p.PartyKey=@PartyKey" +
        " LEFT OUTER JOIN Answers a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionKey='ALLBio111111'" +
        " LEFT OUTER JOIN Answers a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionKey='ALLBio222222'" +
        " LEFT OUTER JOIN Answers a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionKey='ALLBio333333'" +
        " LEFT OUTER JOIN Answers a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionKey='ALLBio444444'" +
        " LEFT OUTER JOIN Answers a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionKey='ALLBio555555'" +
        " LEFT OUTER JOIN Answers a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionKey='ALLBio666666'" +
        " LEFT OUTER JOIN Answers a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionKey='ALLBio777777'" +
        " LEFT OUTER JOIN Answers a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionKey='ALLBio888888'" +
        " LEFT OUTER JOIN Answers a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionKey='ALLBio999999'" +
        " LEFT OUTER JOIN Answers a10 ON a10.PoliticianKey=p.PoliticianKey AND a10.QuestionKey='ALLPersonal440785'" +
        " LEFT OUTER JOIN Answers a11 ON a11.PoliticianKey=p.PoliticianKey AND a11.QuestionKey='ALLPersonal567191'" +
        " LEFT OUTER JOIN Answers a12 ON a12.PoliticianKey=p.PoliticianKey AND a12.QuestionKey='ALLPersonal638630'" +
        " LEFT OUTER JOIN Answers a13 ON a13.PoliticianKey=p.PoliticianKey AND a13.QuestionKey='ALLPersonal392763'" +
        " LEFT OUTER JOIN Answers a14 ON a14.PoliticianKey=p.PoliticianKey AND a14.QuestionKey='ALLPersonal816076'" +
        " LEFT OUTER JOIN Answers a15 ON a15.PoliticianKey=p.PoliticianKey AND a15.QuestionKey='ALLPersonal659866'" +
        " LEFT OUTER JOIN Answers a ON a.PoliticianKey = p.PoliticianKey" +
        "  AND (SELECT IsQuestionOmit FROM Questions q WHERE q.QuestionKey=a.QuestionKey)=0" +
        "  AND (SELECT IsIssueOmit FROM Issues i WHERE i.IssueKey=a.IssueKey)=0" +
        " WHERE ep.ElectionKeyState IN ('{0}')" +
        " GROUP BY PoliticianKey" +
        " ORDER BY o.OfficeLevel,o.DistrictCode ";

      cmdText = string.Format(cmdText, string.Join("','", electionKeys));
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
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

    public static int GetPoliticianCountForOfficeInElection(string electionKey,
      string officeKey, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM ElectionsPoliticians" +
        " WHERE ElectionKey=@ElectionKey AND OfficeKey=@OfficeKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        var result = VoteDb.ExecuteScalar(cmd);
        if ((result == null) || (result == DBNull.Value)) return 0;
        return Convert.ToInt32(result);
      }
    }

    public static string GetPoliticianListForOfficeInElection(string electionKey,
      string officeKey, int commandTimeout = -1)
    {
      var nameList = GetPoliticiansForOfficeInElection(electionKey,
          officeKey, commandTimeout).Rows.OfType<DataRow>()
        .Select(row => Politicians.FormatName(row))
        .ToList();
      return nameList.JoinText();
    }

    public static DataTable GetPoliticiansForOfficeInElection(string electionKey,
      string officeKey, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT p.PoliticianKey,p.FName AS FirstName,p.MName AS MiddleName," +
        " p.NickName,p.LName AS LastName,p.Suffix FROM ElectionsPoliticians ep" +
        " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
        " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
        " ORDER BY ep.OrderOnBallot,p.LName,p.FName";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      return table;
    }

    public static DataTable GetPresidentialCandidatesFromTemplate(
      DateTime electionDate, string nationalPartyCode, int commandTimeout = -1)
    {
      const string cmdText1 = "SELECT ElectionKey FROM Elections" +
        " WHERE ElectionType='A' AND StateCode='PP'" +
        " AND NationalPartyCode=@NationalPartyCode" +
        " AND ElectionDate>=@ElectionDate ORDER BY ElectionDate LIMIT 1";

      var cmd1 = VoteDb.GetCommand(cmdText1, commandTimeout);
      string electionKey;
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd1.Connection = cn;
        VoteDb.AddCommandParameter(cmd1, "ElectionDate", electionDate);
        VoteDb.AddCommandParameter(cmd1, "NationalPartyCode", nationalPartyCode);
        electionKey = VoteDb.ExecuteScalar(cmd1) as string;
      }
      if (string.IsNullOrWhiteSpace(electionKey)) return null;

      const string cmdText2 = "SELECT OfficeKey,PoliticianKey,OrderOnBallot" +
        " FROM ElectionsPoliticians" +
        " WHERE ElectionKey=@ElectionKey";
      var cmd2 = VoteDb.GetCommand(cmdText2, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd2.Connection = cn;
        VoteDb.AddCommandParameter(cmd2, "ElectionKey", electionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd2 as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static OfficeKeyInfo GetPreviousOfficeKeyInfoByPoliticianKey(
      string politicianKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionsPoliticians.ElectionKey,ElectionsPoliticians.OfficeKey" +
        " FROM ElectionsPoliticians,Elections" +
        " WHERE ElectionsPoliticians.PoliticianKey=@PoliticianKey" +
        "  AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey" +
        "  AND Elections.ElectionDate<@Today" +
        " ORDER BY Elections.ElectionDate DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      var table = FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.OfficeKey);
      if (table.Count == 0) return OfficeKeyInfo.Empty;
      return new OfficeKeyInfo
      {
        ElectionKey = table[0].ElectionKey,
        OfficeKey = table[0].OfficeKey
      };
    }

    public static OfficeKeyInfo GetPreviousOfficeKeyInfoByRunningMateKey(
      string runningMateKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionsPoliticians.ElectionKey,ElectionsPoliticians.OfficeKey" +
        " FROM ElectionsPoliticians,Elections" +
        " WHERE ElectionsPoliticians.RunningMateKey=@RunningMateKey" +
        "  AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey" +
        "  AND Elections.ElectionDate<@Today" +
        " ORDER BY Elections.ElectionDate DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "RunningMateKey", runningMateKey);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      var table = FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.OfficeKey);
      if (table.Count == 0) return OfficeKeyInfo.Empty;
      return new OfficeKeyInfo
      {
        ElectionKey = table[0].ElectionKey,
        OfficeKey = table[0].OfficeKey
      };
    }

    public static DataTable GetPrimaryCandidatesToCopy(string electionKey,
      int commandTimeout = -1)
    {
      const string cmdText = "SELECT OfficeKey,PoliticianKey,OrderOnBallot" +
        " FROM ElectionsPoliticians" +
        " WHERE ElectionKey=@ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static IEnumerable<ElectionsPoliticiansRow> GetPrimaryWinnersForGeneralElection(
      string generalElectionKey, DateTime primaryDate, bool isRunoff = false)
    {
      var electionTypes = new List<string>();
      if (isRunoff)
        electionTypes.Add(Elections.ElectionTypeStatePrimaryRunoff);
      else
      {
        electionTypes.Add(Elections.ElectionTypeStatePresidentialPrimary);
        electionTypes.Add(Elections.ElectionTypeStatePrimary);
      }
      var searchElectionKey = Elections.GetStateCodeFromKey(generalElectionKey) +
        primaryDate.ToString("yyyyMMdd") +
        //(isRunoff ? Elections.ElectionTypeStatePrimaryRunoff : Elections.ElectionTypeStatePrimary) + 
        "__" /*any type, any party*/+
        Elections.GetCountyCodeFromKey(generalElectionKey) +
        Elections.GetLocalCodeFromKey(generalElectionKey);

      var cmdText = SelectAllCommandText + " WHERE ElectionKey LIKE @ElectionKey AND IsWinner=1" +
        " AND SUBSTR(ElectionKey,11,1) IN ('" + string.Join("','", electionTypes) + "')";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", searchElectionKey);
      return FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.All);
    }

    public static IEnumerable<ElectionsPoliticiansRow> GetRunoffAdvancersForElection(
      string runoffElectionKey, DateTime electionDate)
    {
      var searchElectionKey = Elections.GetStateCodeFromKey(runoffElectionKey) +
        electionDate.ToString("yyyyMMdd") +
        Elections.GetElectionTypeForRunoff(Elections.GetElectionTypeFromKey(runoffElectionKey)) +
        Elections.GetNationalPartyCodeFromKey(runoffElectionKey) +
        Elections.GetCountyCodeFromKey(runoffElectionKey) +
        Elections.GetLocalCodeFromKey(runoffElectionKey);

      var cmdText = SelectAllCommandText + " WHERE ElectionKey=@ElectionKey AND AdvanceToRunoff=1";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", searchElectionKey);
      return FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.All);
    }

    public static DataTable GetSampleBallotData(string electionKey,
      string congress, string stateSenate, string stateHouse,
      string countyCode, bool includeOptionalData = false, int commandTimeout = -1)
    {
      var electionKeyToInclude = Elections.GetElectionKeyToInclude(electionKey);
      if (!string.IsNullOrWhiteSpace(electionKeyToInclude))
        electionKeyToInclude += "%";
      electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

      const string columnList =
        "ep.ElectionKey, ep.OfficeKey,ep.OrderOnBallot," +
        "ep.RunningMateKey,ep.CountyCode,ep.LocalCode,o.DistrictCode," +
        "o.IsRunningMateOffice,o.OfficeLevel,o.OfficeLine1,o.OfficeLine2," +
        "o.OfficeOrderWithinLevel,o.VoteForWording,o.WriteInLines," +
        "o.ElectionPositions,o.PrimaryPositions,o.PrimaryRunoffPositions,o.GeneralRunoffPositions," +
        "o.WriteInWording,p.AddOn,p.BallotPediaWebAddress,p.BloggerWebAddress,p.EmailAddr AS Email," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.FName AS FirstName," +
        "p.GooglePlusWebAddress," +
        "p.LinkedInWebAddress,p.LName AS LastName,p.MName AS MiddleName,p.Nickname,p.PartyKey," +
        "p.PinterestWebAddress,p.PoliticianKey,p.RSSFeedWebAddress," +
        "p.StateEmailAddr AS StateEmail,p.StateWebAddr AS StateWebAddress," +
        "p.Suffix,p.DateOfBirth,p.TwitterWebAddress,p.VimeoWebAddress," +
        "p.WebAddr AS WebAddress,p.WebstagramWebAddress,p.WikipediaWebAddress," +
        "p.YouTubeWebAddress,pt.PartyCode,pt.PartyName,pt.PartyUrl,pt.IsPartyMajor," +
        "bo.OfficeOrder,bo.DemographicClass,l.LocalDistrict," +
        "oo.PoliticianKey=ep.PoliticianKey AS IsIncumbent";

      const string columns = columnList;

      var cmdText =
        string.Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " INNER JOIN ElectionsBallotOrder bo ON bo.StateCode=ep.StateCode" +
          "  AND bo.OfficeClass=o.OfficeLevel" +
          " LEFT JOIN LocalDistricts l ON l.StateCode=ep.StateCode" +
          "  AND l.CountyCode=ep.CountyCode AND l.LocalCode=ep.LocalCode" +
          " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey=ep.OfficeKey" +
          "  AND oo.PoliticianKey=ep.PoliticianKey" +
          " WHERE (ep.ElectionKey LIKE @ElectionKey OR ep.ElectionKey LIKE @ElectionKeyToInclude)" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          "  AND (ep.CountyCode='' OR ep.CountyCode=@CountyCode)" +
          " UNION SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " INNER JOIN ElectionsBallotOrder bo ON bo.StateCode=ep.StateCode" +
          "  AND bo.OfficeClass=o.OfficeLevel" +
          " LEFT JOIN LocalDistricts l ON l.StateCode=ep.StateCode" +
          "  AND l.CountyCode=ep.CountyCode AND l.LocalCode=ep.LocalCode" +
          " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey=ep.OfficeKey" +
          "  AND oo.PoliticianKey=ep.PoliticianKey" +
          " WHERE ep.ElectionKey LIKE @ElectionKey" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          "  AND (ep.CountyCode='' OR ep.CountyCode=@CountyCode)", columns);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "ElectionKeyToInclude", electionKeyToInclude);
        VoteDb.AddCommandParameter(cmd, "Congress", congress);
        VoteDb.AddCommandParameter(cmd, "StateSenate", stateSenate);
        VoteDb.AddCommandParameter(cmd, "StateHouse", stateHouse);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);

        return table;
      }
    }

    public static DataTable GetSampleBallotIssues(string electionKey, string congress,
      string stateSenate, string stateHouse, string countyCode, int commandTimeout = -1)
    {
      electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

      const string columnList =
        "a.Answer,a.IssueKey,a.QuestionKey,i.Issue,i.IssueLevel,i.IssueOrder," +
        "q.Question,q.QuestionOrder";

      var cmdText =
        string.Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.PoliticianKey" +
          "  AND TRIM(a.Answer) <> ''" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey LIKE @ElectionKey" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          "  AND (ep.CountyCode='' OR ep.CountyCode=@CountyCode)" +
          " UNION SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers a ON a.PoliticianKey=ep.RunningMateKey" +
          "  AND TRIM(a.Answer) <> ''" +
          " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
          " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
          "  AND q.IsQuestionOmit=0" +
          " WHERE ep.ElectionKey LIKE @ElectionKey" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          "  AND (ep.CountyCode='' OR ep.CountyCode=@CountyCode)" +
          " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question", columnList);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "Congress", congress);
        VoteDb.AddCommandParameter(cmd, "StateSenate", stateSenate);
        VoteDb.AddCommandParameter(cmd, "StateHouse", stateHouse);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetVotersEdgeData(string electionKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "ep.ElectionKey, ep.OfficeKey,ep.PoliticianKey,ep.RunningMateKey,o.DistrictCode," +
        "o.OfficeLevel,o.OfficeLine1,o.OfficeLine2," +
        //"p.Accomplishments," +
        "p.Address,p.BallotPediaWebAddress,p.BloggerWebAddress," +
        "p.CityStateZip," +
        //"p.Civic," +
        "p.DateOfBirth," +
        //"p.Education," +
        "p.EmailAddr," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.FName AS FirstName," +
        "p.GeneralStatement," +
        //"p.GooglePlusWebAddress," + 
        //"p.LDSAddress,p.LDSCityStateZip,p.LDSEmailAddr,p.LDSPhone,p.LDSWebAddr," +
        "p.LinkedInWebAddress,p.LName AS LastName," +
        //"p.Military," +
        "p.MName AS MiddleName,p.Nickname," +
        //"p.Personal," +
        "p.Phone," +
        "p.PinterestWebAddress," +
        //"p.Political,p.Profession,p.Religion," +
        "p.RSSFeedWebAddress,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr,p.StatePhone,p.StateWebAddr,p.Suffix," +
        "p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr," +
        "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "pt.PartyName,pt.PartyCode";

      var cmdText =
        string.Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " WHERE ElectionKey=@ElectionKey" +
          " UNION SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " WHERE ElectionKey=@ElectionKey" +
          " ORDER BY OfficeLevel,OfficeLine1,OfficeLine2,LastName,FirstName", columnList);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);

        return table;
      }
    }

    public static int
      MarkWinnersForSingleCandidatePastContests(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT ep1.ElectionKey,ep1.OfficeKey,ep1.PoliticianKey,ep1.RunningMateKey,ep1.IsWinner," +
        "ep1.AdvanceToRunoff" +
        " FROM ElectionsPoliticians ep1" +
        " INNER JOIN Elections e ON e.ElectionKey=ep1.ElectionKey" +
        " WHERE e.ElectionDate < CURDATE() AND ep1.IsWinner=0 AND" +
        " (SELECT COUNT(*) FROM ElectionsPoliticians ep2 " +
        " WHERE ep2.ElectionKey=ep1.ElectionKey AND" +
        "  ep2.OfficeKey=ep1.OfficeKey)=1";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.Winners);
      if (table.Count > 0)
      {
        foreach (var row in table) row.IsWinner = true;
        UpdateTable(table, ElectionsPoliticiansTable.ColumnSet.Winners, commandTimeout);
      }
      return table.Count;
    }

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText = "SELECT COUNT(*) FROM ElectionsPoliticians WHERE OfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    //public static int UpdateOfficeKeyByOfficeKey(String newValue, String officeKey)
    //{
    //  const string cmdText = "UPDATE ElectionsPoliticians SET OfficeKey=@newValue WHERE OfficeKey=@OfficeKey";
    //  var cmd = VoteDb.GetCommand(cmdText, -1);
    //  VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //  VoteDb.AddCommandParameter(cmd, "newValue", newValue);
    //  return VoteDb.ExecuteNonQuery(cmd);
    //}

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}