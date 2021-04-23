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
      "a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp," +
      "a.YouTubeUrl,a.YouTubeSource,a.YouTubeDescription,a.YouTubeRunningTime," +
      "a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
      "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
      "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
      "i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder," +
      "ep.ElectionKey,ep.OfficeKey";

    private const string IssueColumnListNew =
      "a.Answer,Convert(i.IssueId, char) AS IssueKey,CONVERT(a.QuestionId,char) AS QuestionKey," +
      "a.Sequence,a.Source,a.DateStamp," +
      "a.YouTubeUrl,a.YouTubeSource,a.YouTubeDescription,a.YouTubeRunningTime," +
      "a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
      "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
      "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
      "i.Issue,qj.IssueLevel,i.IssueOrder,q.Question,(i.IssueId>1000) AS IsIssue,iq.QuestionOrder," +
      "ep.ElectionKey,ep.OfficeKey";

    private const string IssueColumnListNew3 =
      "a.Answer,Convert(i.IssueId, char) AS IssueKey,CONVERT(a.QuestionId,char) AS QuestionKey," +
      "a.Sequence,a.Source,a.DateStamp," +
      "a.YouTubeUrl,a.YouTubeSource,a.YouTubeDescription,a.YouTubeRunningTime," +
      "a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
      "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
      "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
      "i.Issue,qj.IssueLevel,igi.IssueOrder,q.Question,iq.QuestionOrder," +
      "ep.ElectionKey,ep.OfficeKey,ig.IssueGroupId,ig.IssueGroupOrder,ig.Heading,ig.SubHeading";

    public static DataTable GetAds(string electionKey, string officeKey, string adKey = null)
    {
      var cmdText =
        IsNullOrWhiteSpace(adKey)
          ? "SELECT ElectionKey,OfficeKey,PoliticianKey,AdType,AdUrl,AdThumbnailUrl," +
          "AdSponsor,AdSponsorUrl,AdIsCandidateSponsored" +
          " FROM ElectionsPoliticians WHERE ElectionKey=@ElectionKey AND OfficeKey=@OfficeKey AND AdEnabled=1" +
          " ORDER BY AdTimeStamp" 
          : "SELECT ElectionKey,OfficeKey,PoliticianKey,AdType,AdUrl,AdThumbnailUrl," +
          "AdSponsor,AdSponsorUrl,AdIsCandidateSponsored" +
          " FROM ElectionsPoliticians WHERE ElectionKey=@ElectionKey AND OfficeKey=@OfficeKey AND PoliticianKey=@AdKey AND AdType != ''" +
          " ORDER BY AdTimeStamp";

      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "AdKey", adKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCompareCandidateIssues(string electionKey, string officeKey,
      string questionKey = null, int commandTimeout = -1)
    {
      var cmdText =
        Format(
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
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
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
          questionKey == null ? Empty : "AND a.QuestionKey=@QuestionKey");

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

    public static DataTable GetCompareCandidateIssuesNew(string electionKey, string officeKey,
      int? questionId = null, int commandTimeout = -1)
    {
      var stagingTest = VotePage.ShowIssues ? Empty : " AND i.IssueId<1000 ";
      // This is used by the compare page and so if a quesion appears in more than one issue
      // we want to fetch it twice
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.PoliticianKey {1}" +
          "  AND (TRIM(a.Answer)<>''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          $" INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0 {stagingTest}" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND {2}" +
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.RunningMateKey {1}" +
          "  AND (TRIM(a.Answer) <> ''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          $" INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0 {stagingTest}" +
           " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND {2}" +
          " ORDER BY IsIssue,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnListNew,
          questionId == null ? Empty : "AND a.QuestionId=@QuestionId",
          Questions.QuestionsJurisdictionsWhereClause);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        if (questionId != null)
          VoteDb.AddCommandParameter(cmd, "QuestionId", questionId.Value);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCompareCandidateIssuesNew3(string electionKey, string officeKey,
      int? questionId = null, int commandTimeout = -1)
    {
      // This is used by the compare page and so if a quesion appears in more than one issue
      // we want to fetch it twice, unless a specific question is requested
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.PoliticianKey {1}" +
          "  AND (TRIM(a.Answer)<>''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
          " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
          " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND {2}" +
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.RunningMateKey {1}" +
          "  AND (TRIM(a.Answer) <> ''" +
          "   OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
          " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
          " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
           " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND {2}" +
          " {3} ORDER BY IssueLevel,IssueGroupOrder,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnListNew3,
          questionId == null ? Empty : "AND a.QuestionId=@QuestionId",
          Questions.QuestionsJurisdictionsWhereClause,
          questionId == null ? Empty : " GROUP BY PoliticianKey, QuestionKey, Sequence");

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        if (questionId != null)
          VoteDb.AddCommandParameter(cmd, "QuestionId", questionId.Value);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCompareCandidateVideos(string electionKey, string officeKey,
      string politicianKey, int commandTimeout = -1)
    {
      var cmdText =
        Format(
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
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
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

    public static DataTable GetCompareCandidateVideosNew(string electionKey, string officeKey,
      string politicianKey, int commandTimeout = -1)
    {
      // This is used by the compare page and so if a quesion appears in more than one issue
      // we want to fetch it twice
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.PoliticianKey" +
          "  AND (TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND ep.PoliticianKey=@PoliticianKey AND {1}" +
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.RunningMateKey" +
          "  AND (TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
           " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND ep.PoliticianKey=@PoliticianKey AND {1}" +
          " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnListNew,
          Questions.QuestionsJurisdictionsWhereClause);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetCompareCandidateVideosNew3(string electionKey, string officeKey,
      string politicianKey, int commandTimeout = -1)
    {
      // This is used by the compare page and so if a quesion appears in more than one issue
      // we want to fetch it twice
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        Format(
          "SELECT {0}, ep.PoliticianKey, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.PoliticianKey" +
          "  AND (TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
          " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
          " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND ep.PoliticianKey=@PoliticianKey AND {1}" +
          " UNION ALL SELECT {0},ep.RunningMateKey AS PoliticianKey, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Answers2 a ON a.PoliticianKey=ep.RunningMateKey" +
          "  AND (TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
          "    AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable=''))" +
          " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
          " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=a.QuestionId" +
          " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId" +
          "  AND q.IsQuestionOmit=0" +
          " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
          " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
          " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
           " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " AND ep.PoliticianKey=@PoliticianKey AND {1}" +
          " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC",
          IssueColumnListNew3,
          Questions.QuestionsJurisdictionsWhereClause);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    // ReSharper disable NotAccessedField.Global
    public class EmbeddedElection
    {
      public string Value;
      public string Text;
      public EmbeddedOffice[] Offices;
    }

    public class EmbeddedOffice
    {
      public string Value;
      public string Text;
      public EmbeddedPolitician[] Politicians;
    }

    public class EmbeddedPolitician
    {
      public string Value;
      public string Text;
    }
    // ReSharper restore NotAccessedField.Global

    public static EmbeddedElection[] GetEmbeddedKeyDataForState(string stateCode)
    {
      // currently only returns data for state elections

      const string cmdText = 
        "SELECT e.ElectionKey,e.ElectionDate,e.ElectionOrder,e.ElectionDesc," +
        "o.OfficeKey,o.OfficeLine1,o.OfficeLine2," +
        "p.PoliticianKey,p.FName,p.MName,p.LName,p.Nickname,p.Suffix" +
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Elections e ON e.ElectionKey = ep.ElectionKey" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
        " WHERE ep.StateCode=@StateCode AND e.CountyCode = '' AND e.LocalKey = ''";

      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .GroupBy(r => r.ElectionKey(), StringComparer.OrdinalIgnoreCase)
          .OrderByDescending(g => g.First().ElectionDate())
          .ThenBy(g => g.First().ElectionOrder())
          .ThenBy(g => g.First().ElectionDescription()).Select(g => new EmbeddedElection
          {
            Value = g.Key,
            Text = g.First().ElectionDescription(),
            Offices = g.GroupBy(r => r.OfficeKey(), StringComparer.OrdinalIgnoreCase)
              .Select(g2 => new EmbeddedOffice
              {
                Value = g2.Key,
                Text = Offices.FormatOfficeName(g2.First()),
                Politicians = g2
                  .OrderBy(r2 =>r2.LastName())
                  .ThenBy(r2 => r2.FirstName())
                  .ThenBy(r2 => r2.MiddleName())
                  .ThenBy(r2 => r2.Suffix())
                  .Select(r2 => new EmbeddedPolitician
                  {
                    Value = r2.PoliticianKey(),
                    Text = Politicians.FormatName(r2)
                  }).ToArray()
              }).ToArray()
          }).ToArray();
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

    public static IDictionary<string, int> GetOfficeCandidatesByYear()
    {
      const string cmdText = "SELECT SUBSTR(ElectionKey,3,4) AS Year,Count(*) AS Count" +
        " FROM ElectionsPoliticians GROUP BY Year";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .ToDictionary(r => r.Year(), r => r.Count());
      }
    }

    public static DataTable GetOfficesAndCandidatesReportData(DateTime fromDate,
      DateTime toDate, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT  NULL AS ReferendumKey,ep.ElectionKeyState,e.ElectionDesc,e.ElectionDate," +
        "e.StateCode,o.OfficeLevel,o.AlternateOfficeLevel,o.OfficeKey" +
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Elections e ON e.ElectionKey = ep.ElectionKeyState" +
        " INNER JOIN Offices o on o.OfficeKey = ep.OfficeKey" +
        " WHERE e.ElectionDate>=@FromDate AND e.ElectionDate<=@ToDate" +
        " UNION ALL SELECT r.ReferendumKey,r.ElectionKeyState,e2.ElectionDesc,e2.ElectionDate," +
        "e2.StateCode,NULL AS OfficeLevel,NULL AS AlternateOfficeLevel,NULL AS OfficeKey" +
        " FROM Referendums r" +
        " INNER JOIN Elections e2 ON e2.ElectionKey = r.ElectionKeyState" +
        " WHERE e2.ElectionDate>=@FromDate AND e2.ElectionDate<=@ToDate" +
        " ORDER BY ElectionDate,StateCode,ElectionDesc";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "FromDate", fromDate);
        VoteDb.AddCommandParameter(cmd, "ToDate", toDate);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      return table;
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
        "ep.ElectionKey,ep.OfficeKey,ep.PoliticianKey,ep.StateCode,ep.CountyCode," +
        "ep.LocalKey,o.OfficeLine1,o.OfficeLine2," +
        "p.DateOfBirth,p.Phone,p.StatePhone,p.Address,p.StateAddress,p.AddOn," +
        "p.FName AS FirstName,p.MName AS MiddleName,p.Nickname,p.LName AS LastName,p.Suffix," +
        "p.EmailAddr AS Email,p.StateEmailAddr AS StateEmail,p.WebAddr AS WebAddress," +
        "p.StateWebAddr AS StateWebAddress,p.BallotPediaWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
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

      cmdText = Format(cmdText, Join("','", electionKeys));
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

    public static DataTable GetPartyCandidatesInElectionsNew(IEnumerable<string> electionKeys,
      string partyKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT COUNT(a.PoliticianKey) AS AnswerCount," +
        "ep.ElectionKey,ep.OfficeKey,ep.PoliticianKey,ep.StateCode,ep.CountyCode," +
        "ep.LocalKey,o.OfficeLine1,o.OfficeLine2," +
        "p.DateOfBirth,p.Phone,p.StatePhone,p.Address,p.StateAddress,p.AddOn," +
        "p.FName AS FirstName,p.MName AS MiddleName,p.Nickname,p.LName AS LastName,p.Suffix," +
        "p.EmailAddr AS Email,p.StateEmailAddr AS StateEmail,p.WebAddr AS WebAddress," +
        "p.StateWebAddr AS StateWebAddress,p.BallotPediaWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.GooglePlusWebAddress,p.LinkedInWebAddress," +
        "p.PinterestWebAddress,p.RSSFeedWebAddress,p.TwitterWebAddress,p.VimeoWebAddress," +
        "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "p.CityStateZip,p.StateCityStateZip," +
        "NOT a01.QuestionId IS NULL AS BioGen," +
        "NOT a02.QuestionId IS NULL AS BioPer," +
        "NOT a03.QuestionId IS NULL AS BioPro," +
        "NOT a04.QuestionId IS NULL AS BioCiv," +
        "NOT a05.QuestionId IS NULL AS BioPol," +
        "NOT a06.QuestionId IS NULL AS BioRel," +
        "NOT a07.QuestionId IS NULL AS BioAcc," +
        "NOT a08.QuestionId IS NULL AS BioEdu," +
        "NOT a09.QuestionId IS NULL AS BioMil," +
        "NOT a10.QuestionId IS NULL AS PerWhy," +
        "NOT a11.QuestionId IS NULL AS PerGls," +
        "NOT a12.QuestionId IS NULL AS PerAch," +
        "NOT a13.QuestionId IS NULL AS PerCon," +
        "NOT a14.QuestionId IS NULL AS PerPub," +
        "NOT a14.QuestionId IS NULL AS PerOpi" +
        " FROM ElectionsPoliticians ep" +
        " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey AND o.OfficeKey != 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey AND p.PartyKey=@PartyKey" +
        $" LEFT OUTER JOIN Answers2 a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionId={Issues.QuestionId.GeneralPhilosophy.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionId={Issues.QuestionId.PersonalAndFamily.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionId={Issues.QuestionId.ProfessionalExperience.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionId={Issues.QuestionId.CivicInvolvement.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionId={Issues.QuestionId.PoliticalExperience.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionId={Issues.QuestionId.ReligiousAffiliation.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionId={Issues.QuestionId.AccomplishmentsAndAwards.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionId={Issues.QuestionId.EducationalBackground.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionId={Issues.QuestionId.MilitaryService.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a10 ON a10.PoliticianKey=p.PoliticianKey AND a10.QuestionId={Issues.QuestionId.WhyIAmRunning.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a11 ON a11.PoliticianKey=p.PoliticianKey AND a11.QuestionId={Issues.QuestionId.GoalsIfElected.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a12 ON a12.PoliticianKey=p.PoliticianKey AND a12.QuestionId={Issues.QuestionId.AchievementsIfElected.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a13 ON a13.PoliticianKey=p.PoliticianKey AND a13.QuestionId={Issues.QuestionId.AreasToConcentrateOn.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a14 ON a14.PoliticianKey=p.PoliticianKey AND a14.QuestionId={Issues.QuestionId.OnEnteringPublicService.ToInt()}" +
        $" LEFT OUTER JOIN Answers2 a15 ON a15.PoliticianKey=p.PoliticianKey AND a15.QuestionId={Issues.QuestionId.OpinionsOfOtherCandidates.ToInt()}" +
        " LEFT OUTER JOIN Answers2 a ON a.PoliticianKey = p.PoliticianKey" +
        "  AND (SELECT IsQuestionOmit FROM Questions2 q WHERE q.QuestionId=a.QuestionId)=0" +
        " WHERE ep.ElectionKeyState IN ('{0}')" +
        " GROUP BY PoliticianKey" +
        " ORDER BY o.OfficeLevel,o.DistrictCode ";

      cmdText = Format(cmdText, Join("','", electionKeys));
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
        if (result == null || result == DBNull.Value) return 0;
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
      if (IsNullOrWhiteSpace(electionKey)) return null;

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
      var stateCode = Elections.GetStateCodeFromKey(generalElectionKey);
      var searchElectionKey = stateCode + primaryDate.ToString("yyyyMMdd") + "__";
        /*any type, any party*/
      if (Elections.IsCountyElection(generalElectionKey))
        searchElectionKey += Elections.GetCountyCodeFromKey(generalElectionKey);
      else if (Elections.IsLocalElection(generalElectionKey))
        searchElectionKey += Elections.GetLocalKeyFromKey(generalElectionKey);

      var cmdText = SelectAllCommandText +
        " WHERE ElectionKey LIKE @ElectionKey AND IsWinner=1" +
        " AND SUBSTR(ElectionKey,11,1) IN ('" + Join("','", electionTypes) + "')";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", searchElectionKey);
      return FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.All);
    }

    public static IEnumerable<ElectionsPoliticiansRow> GetRunoffAdvancersForElection(
      string runoffElectionKey, DateTime electionDate)
    {
      var stateCode = Elections.GetStateCodeFromKey(runoffElectionKey);
      var searchElectionKey = stateCode + electionDate.ToString("yyyyMMdd") +
        Elections.GetElectionTypeForRunoff(
          Elections.GetElectionTypeFromKey(runoffElectionKey)) +
        Elections.GetNationalPartyCodeFromKey(runoffElectionKey);
      if (Elections.IsCountyElection(runoffElectionKey))
        searchElectionKey += Elections.GetCountyCodeFromKey(runoffElectionKey);
      else if (Elections.IsLocalElection(runoffElectionKey))
        searchElectionKey += Elections.GetLocalKeyFromKey(runoffElectionKey);

      var cmdText = SelectAllCommandText +
        " WHERE ElectionKey=@ElectionKey AND AdvanceToRunoff=1";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", searchElectionKey);
      return FillTable(cmd, ElectionsPoliticiansTable.ColumnSet.All);
    }

    public static DataTable GetSampleBallotData(string electionKey, string congress,
      string stateSenate, string stateHouse, string countyCode, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict, bool noData = false, int commandTimeout = -1)
    {
      // normalize congress length
      if (congress.Length == 2) congress = '0' + congress;
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var electionKeyToInclude = Elections.GetElectionKeyToInclude(electionKey);
      if (!IsNullOrWhiteSpace(electionKeyToInclude))
        electionKeyToInclude += "%";
      electionKey = Elections.GetStateElectionKeyFromKey(electionKey) + "%";

      var districtsClause = LocalIdsCodes.GetLocals(stateCode, countyCode, district, place,
        elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict)
        .SqlIn("ep.LocalKey");

      const string columnList =
        "ep.ElectionKey, ep.OfficeKey,ep.OrderOnBallot," +
        "ep.RunningMateKey,ep.CountyCode,ep.LocalKey,o.DistrictCode," +
        "o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice,o.OfficeLevel,o.OfficeLine1,o.OfficeLine2," +
        "o.OfficeOrderWithinLevel,o.WriteInLines," +
        "o.ElectionPositions,o.PrimaryPositions,o.PrimaryRunoffPositions,o.GeneralRunoffPositions," +
        "o.WriteInWording,p.AddOn,p.BallotPediaWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress,p.EmailAddr AS Email," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.FName AS FirstName," +
        "p.GooglePlusWebAddress," +
        "p.LinkedInWebAddress,p.LName AS LastName,p.MName AS MiddleName,p.Nickname,p.PartyKey," +
        "p.PinterestWebAddress,p.PoliticianKey,p.RSSFeedWebAddress," +
        "p.StateEmailAddr AS StateEmail,p.StateWebAddr AS StateWebAddress," +
        "p.Suffix,p.DateOfBirth,p.TwitterWebAddress,p.VimeoWebAddress," +
        "p.WebAddr AS WebAddress,p.WebstagramWebAddress,p.WikipediaWebAddress," +
        "p.YouTubeWebAddress,pt.PartyCode,pt.PartyName,pt.PartyUrl,pt.IsPartyMajor," +
        "bo.OfficeOrder,l.LocalDistrict," +
        "oo.PoliticianKey=ep.PoliticianKey AS IsIncumbent";

      var columns = noData ? "COUNT(*) AS Count" : columnList;

      var cmdText1 =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " INNER JOIN ElectionsBallotOrder bo ON bo.StateCode=ep.StateCode" +
          "  AND bo.OfficeClass=o.OfficeLevel" +
          " LEFT JOIN LocalDistricts l ON l.StateCode=ep.StateCode AND l.LocalKey=ep.LocalKey" +
          " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey=ep.OfficeKey" +
          "  AND oo.PoliticianKey=ep.PoliticianKey" +
          " WHERE (ep.ElectionKey LIKE @ElectionKey OR ep.ElectionKey LIKE @ElectionKeyToInclude)" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          $"  AND (ep.CountyCode='' AND ep.LocalKey='' OR ep.CountyCode=@CountyCode OR {districtsClause})", columns);

      var cmdText2 =
        Format(
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " INNER JOIN ElectionsBallotOrder bo ON bo.StateCode=ep.StateCode" +
          "  AND bo.OfficeClass=o.OfficeLevel" +
          " LEFT JOIN LocalDistricts l ON l.StateCode=ep.StateCode AND l.LocalKey=ep.LocalKey" +
          " LEFT JOIN OfficesOfficials oo ON oo.OfficeKey=ep.OfficeKey" +
          "  AND oo.PoliticianKey=ep.PoliticianKey" +
          " WHERE ep.ElectionKey LIKE @ElectionKey" +
          "  AND (o.OfficeLevel IN (1,2,4)" +
          "   OR o.OfficeLevel=3 AND o.DistrictCode=@Congress" +
          "   OR o.OfficeLevel=5 AND o.DistrictCode=@StateSenate" +
          "   OR o.OfficeLevel=6 AND o.DistrictCode=@StateHouse" +
          "   OR o.OfficeLevel>=7)" +
          $"  AND (ep.CountyCode='' AND ep.LocalKey='' OR ep.CountyCode=@CountyCode OR {districtsClause})", columns);

      var cmdText = cmdText1;
      if (!noData) cmdText += cmdText2;

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

    public static DataTable GetVotersEdgeData(string electionKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "ep.ElectionKey, ep.OfficeKey,ep.PoliticianKey,ep.RunningMateKey,o.DistrictCode," +
        "e.ElectionDesc,e.ElectionDate," +
        "o.OfficeLevel,o.OfficeLine1,o.OfficeLine2," +
        "p.Address,p.BallotPediaWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress,p.BloggerWebAddress,p.PodcastWebAddress," +
        "p.CityStateZip," +
        "p.DateOfBirth," +
        "p.EmailAddr," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.GooglePlusWebAddress,p.FName AS FirstName," +
        "p.LinkedInWebAddress,p.LName AS LastName," +
        "p.MName AS MiddleName,p.Nickname," +
        "p.Phone," +
        "p.PinterestWebAddress," +
        "a01.Answer as GeneralStatement," +
        "a02.Answer as Personal," +
        "a03.Answer as Profession," +
        "a04.Answer as Civic," +
        "a05.Answer as Political," +
        "a06.Answer as Religion," +
        "a07.Answer as Accomplishments," +
        "a08.Answer as Education," +
        "a09.Answer as Military," +
        "p.RSSFeedWebAddress,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr,p.StatePhone,p.StateWebAddr,p.Suffix," +
        "p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr," +
        "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "pt.PartyName,pt.PartyCode";

      var cmdText =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " LEFT OUTER JOIN Answers a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionKey='ALLBio111111' AND a01.Sequence=0" +
          " LEFT OUTER JOIN Answers a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionKey='ALLBio222222' AND a02.Sequence=0" +
          " LEFT OUTER JOIN Answers a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionKey='ALLBio333333' AND a03.Sequence=0" +
          " LEFT OUTER JOIN Answers a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionKey='ALLBio444444' AND a04.Sequence=0" +
          " LEFT OUTER JOIN Answers a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionKey='ALLBio555555' AND a05.Sequence=0" +
          " LEFT OUTER JOIN Answers a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionKey='ALLBio666666' AND a06.Sequence=0" +
          " LEFT OUTER JOIN Answers a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionKey='ALLBio777777' AND a07.Sequence=0" +
          " LEFT OUTER JOIN Answers a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionKey='ALLBio888888' AND a08.Sequence=0" +
          " LEFT OUTER JOIN Answers a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionKey='ALLBio999999' AND a09.Sequence=0" +
          " WHERE ep.ElectionKey=@ElectionKey" +
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " LEFT OUTER JOIN Answers a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionKey='ALLBio111111' AND a01.Sequence=0" +
          " LEFT OUTER JOIN Answers a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionKey='ALLBio222222' AND a02.Sequence=0" +
          " LEFT OUTER JOIN Answers a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionKey='ALLBio333333' AND a03.Sequence=0" +
          " LEFT OUTER JOIN Answers a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionKey='ALLBio444444' AND a04.Sequence=0" +
          " LEFT OUTER JOIN Answers a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionKey='ALLBio555555' AND a05.Sequence=0" +
          " LEFT OUTER JOIN Answers a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionKey='ALLBio666666' AND a06.Sequence=0" +
          " LEFT OUTER JOIN Answers a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionKey='ALLBio777777' AND a07.Sequence=0" +
          " LEFT OUTER JOIN Answers a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionKey='ALLBio888888' AND a08.Sequence=0" +
          " LEFT OUTER JOIN Answers a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionKey='ALLBio999999' AND a09.Sequence=0" +
          " WHERE ep.ElectionKey=@ElectionKey" +
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

    public static DataTable GetElectionCsvCandidateData(string electionKey, int commandTimeout = -1)
    {
      const string columnList =
        "ep.ElectionKey, ep.OfficeKey,ep.PoliticianKey,ep.RunningMateKey,ep.CountyCode," +
        "ep.LocalKey,ep.OrderOnBallot,ep.AdUrl,ep.AdType,ep.AdEnabled," +
        "o.StateCode,o.DistrictCode,e.ElectionDesc,e.ElectionDate,o.OfficeLevel," +
        "o.AlternateOfficeLevel,o.OfficeLine1,o.OfficeLine2,o.DistrictCode,o.OfficeOrderWithinLevel," +
        "p.Address,p.BallotPediaWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip,p.DateOfBirth,p.EmailAddr," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.GooglePlusWebAddress,p.FName AS FirstName," +
        "p.LinkedInWebAddress,p.LName AS LastName,p.MName AS MiddleName,p.Nickname,p.Phone," +
        "p.PinterestWebAddress,p.Password,p.YouTubeWebAddress," +
        "a01.Answer as GeneralStatement," +
        "a02.Answer as Personal," +
        "a03.Answer as Profession," +
        "a04.Answer as Civic," +
        "a05.Answer as Political," +
        "a06.Answer as Religion," +
        "a07.Answer as Accomplishments," +
        "a08.Answer as Education," +
        "a09.Answer as Military," +
        "p.RSSFeedWebAddress,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr,p.StatePhone,p.StateWebAddr,p.Suffix," +
        "p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr," +
        "p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "c.County,l.LocalDistrict,pt.PartyName,pt.PartyCode";

      var cmdText =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " LEFT OUTER JOIN Counties c ON c.StateCode = ep.StateCode AND c.CountyCode=ep.CountyCode" +
          " LEFT OUTER JOIN LocalDistricts l ON l.StateCode = ep.StateCode AND l.LocalKey=ep.LocalKey" +
          $" LEFT OUTER JOIN Answers2 a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionId={Issues.QuestionId.GeneralPhilosophy.ToInt()} AND a01.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionId={Issues.QuestionId.PersonalAndFamily.ToInt()} AND a02.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionId={Issues.QuestionId.ProfessionalExperience.ToInt()} AND a03.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionId={Issues.QuestionId.CivicInvolvement.ToInt()} AND a04.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionId={Issues.QuestionId.PoliticalExperience.ToInt()} AND a05.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionId={Issues.QuestionId.ReligiousAffiliation.ToInt()} AND a06.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionId={Issues.QuestionId.AccomplishmentsAndAwards.ToInt()} AND a07.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionId={Issues.QuestionId.EducationalBackground.ToInt()} AND a08.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionId={Issues.QuestionId.MilitaryService.ToInt()} AND a09.Sequence=0" +
          " WHERE ep.ElectionKeyState=@ElectionKey" +
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " LEFT OUTER JOIN Counties c ON c.StateCode = ep.StateCode AND c.CountyCode=ep.CountyCode" +
          " LEFT OUTER JOIN LocalDistricts l ON l.StateCode = ep.StateCode AND l.LocalKey=ep.LocalKey" +
          $" LEFT OUTER JOIN Answers2 a01 ON a01.PoliticianKey=p.PoliticianKey AND a01.QuestionId={Issues.QuestionId.GeneralPhilosophy.ToInt()} AND a01.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a02 ON a02.PoliticianKey=p.PoliticianKey AND a02.QuestionId={Issues.QuestionId.PersonalAndFamily.ToInt()} AND a02.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a03 ON a03.PoliticianKey=p.PoliticianKey AND a03.QuestionId={Issues.QuestionId.ProfessionalExperience.ToInt()} AND a03.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a04 ON a04.PoliticianKey=p.PoliticianKey AND a04.QuestionId={Issues.QuestionId.CivicInvolvement.ToInt()} AND a04.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a05 ON a05.PoliticianKey=p.PoliticianKey AND a05.QuestionId={Issues.QuestionId.PoliticalExperience.ToInt()} AND a05.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a06 ON a06.PoliticianKey=p.PoliticianKey AND a06.QuestionId={Issues.QuestionId.ReligiousAffiliation.ToInt()} AND a06.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a07 ON a07.PoliticianKey=p.PoliticianKey AND a07.QuestionId={Issues.QuestionId.AccomplishmentsAndAwards.ToInt()} AND a07.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a08 ON a08.PoliticianKey=p.PoliticianKey AND a08.QuestionId={Issues.QuestionId.EducationalBackground.ToInt()} AND a08.Sequence=0" +
          $" LEFT OUTER JOIN Answers2 a09 ON a09.PoliticianKey=p.PoliticianKey AND a09.QuestionId={Issues.QuestionId.MilitaryService.ToInt()} AND a09.Sequence=0" +
          " WHERE ep.ElectionKeyState=@ElectionKey" +
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
        foreach (var row in table)
        {
          var positionsData = Offices.GetPositionsDataByOfficeKey(row.OfficeKey)[0];
          var positions = Elections.GetOfficePositions(row.ElectionKey, positionsData) ?? 0;
          if (positions == 1)
          {
            row.IsWinner = true;
            if (!Elections.IsPrimaryElection(row.ElectionKey) && positionsData.Incumbents == 1)
              OfficesOfficials.UpdateIncumbents(row.OfficeKey, new[]{row.PoliticianKey},
                new List<(string key, string runningMateKey)>
                  { (key: row.PoliticianKey, runningMateKey: row.RunningMateKey)});
          }
        }
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

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}