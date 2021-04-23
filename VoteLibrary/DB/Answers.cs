using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class Answers
  {
    public static int CountActiveIssueAnswers(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
        " WHERE NOT a.IssueKey IN ('ALLBio','ALLPersonal')" +
        " AND i.IsIssueOmit=0 AND q.IsQuestionOmit=0";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountActiveIssueAnswersNew(int commandTimeout = -1)
    {
      // the double select is to eliminate double counting
      // when a question is assigned to more than one issue
      var cmdText =
        "SELECT COUNT(*) FROM" + 
        " (SELECT COUNT(*) FROM Answers2 a" +
        " INNER JOIN Questions2 q ON q.QuestionId = a.QuestionId" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = a.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId" + 
        $" WHERE NOT i.IssueId IN({Issues.IssueId.Biographical.ToInt()},{Issues.IssueId.Reasons.ToInt()})" +
        "  AND i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" +
        " GROUP BY a.PoliticianKey, a.QuestionId, a.Sequence) as q";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountBioAnswers(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Answers" +
        " WHERE IssueKey = 'ALLBio'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountBioAnswersNew(int commandTimeout = -1)
    {
      // the double select is to eliminate double counting
      // when a question is assigned to more than one issue
      var cmdText =
        "SELECT COUNT(*) FROM" +
        " (SELECT COUNT(*) FROM Answers2 a" +
        " INNER JOIN Questions2 q ON q.QuestionId = a.QuestionId" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = a.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId" +
        $" WHERE i.IssueId={Issues.IssueId.Biographical.ToInt()}" +
        "  AND i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" +
        " GROUP BY a.PoliticianKey, a.QuestionId, a.Sequence) as q";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountPersonalAnswers(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Answers" +
        " WHERE IssueKey = 'ALLPersonal'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static int CountPersonalAnswersNew(int commandTimeout = -1)
    {
      // the double select is to eliminate double counting
      // when a question is assigned to more than one issue
      var cmdText =
        "SELECT COUNT(*) FROM" +
        " (SELECT COUNT(*) FROM Answers2 a" +
        " INNER JOIN Questions2 q ON q.QuestionId = a.QuestionId" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = a.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId" +
        $" WHERE i.IssueId={Issues.IssueId.Reasons.ToInt()}" +
        "  AND i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" +
        " GROUP BY a.PoliticianKey, a.QuestionId, a.Sequence) as q";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    [NotNull]
    public static DataTable GetAnswerIssuesNew([NotNull] string politicianKey, [NotNull] string officeKey, 
      int commandTimeout = -1)
    {
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        "SELECT i.IssueId,i.Issue,(i.IssueId>1000) AS IsIssue FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0" +
        $" WHERE {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY IsIssue,i.IssueOrder,i.Issue";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetAnswerQuestionsNew([NotNull] string politicianKey,
      [NotNull] string officeKey, int issueId, int commandTimeout = -1)
    {
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        "SELECT q.QuestionId,q.Question FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId AND iq.IssueId=@IssueId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0" +
        $" WHERE {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY iq.QuestionOrder,q.Question";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        VoteDb.AddCommandParameter(cmd, "IssueId", issueId);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetAnswers([NotNull] string politicianKey, [NotNull] string officeKey,
      [CanBeNull] string questionKey = null, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl," +
        "a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        "a.PoliticianKey,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" + "  AND q.IsQuestionOmit=0" +
        " WHERE a.PoliticianKey=@PoliticianKey" +
        $" {(questionKey == null ? Empty : "AND a.QuestionKey=@QuestionKey")}" +
        " AND (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
        " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
        " ) AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
        " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC,Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        if (questionKey != null)
          VoteDb.AddCommandParameter(cmd, "QuestionKey", questionKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetAnswersForConsolidation(int questionIdTo, int questionIdFor)
    {
      const string cmdText = "SELECT * FROM Answers2 WHERE questionId IN (@QuestionIdTo,@QuestionIdFrom)";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "QuestionIdTo", questionIdTo);
        VoteDb.AddCommandParameter(cmd, "QuestionIdFrom", questionIdFor);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetAnswersNew([NotNull] string politicianKey,
      [NotNull] string officeKey, [CanBeNull] int? questionId = null, bool forUpdate = false, int commandTimeout = -1)
    {
      var stagingTest = VotePage.ShowIssues || forUpdate ? Empty : " AND i.IssueId<1000 ";
      // If question is assigned to multiple issues, it is returned multiple times
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        "SELECT a.Answer,q.QuestionId,CONVERT(q.QuestionId,char) AS QuestionKey,a.Sequence,a.Source," +
        " a.DateStamp,a.YouTubeUrl," +
        " a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        " a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        " a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        " a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        " @PoliticianKey AS PoliticianKey,i.IssueId,(i.IssueId>1000) AS IsIssue,Convert(i.IssueId, char) AS IssueKey,i.Issue," +
        " IF(qj.IssueLevel = 'A', 1, 0) AS IsTextSourceOptional,qj.IssueLevel,i.IssueOrder,q.Question,iq.QuestionOrder" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        $" INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0 {stagingTest}" +
        $" {(forUpdate ? "LEFT OUTER" : "INNER")} JOIN Answers2 a ON a.PoliticianKey = @PoliticianKey AND a.QuestionId = qj.QuestionId" +
        " AND(TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl) <> '' AND NOT a.YouTubeUrl IS NULL AND(a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable = '') OR" +
        " TRIM(a.FacebookVideoUrl) <> '' AND NOT a.FacebookVideoUrl IS NULL AND(a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable = ''))" +
        " WHERE" +
        $" {(questionId == null ? Empty : "qj.QuestionId=@QuestionId AND")}" +
        $" {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY IsIssue,i.IssueOrder,i.Issue,iq.QuestionOrder,q.Question,a.DateStamp DESC, a.Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        if (questionId != null)
          VoteDb.AddCommandParameter(cmd, "QuestionId", questionId.Value);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetAnswersNew3([NotNull] string politicianKey,
      [NotNull] string officeKey, [CanBeNull] int? questionId = null, int commandTimeout = -1)
    {
      // If question is assigned to multiple issues, it is returned multiple times
      // unless a specific question is requested
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        "SELECT a.Answer,CONVERT(a.QuestionId,char) AS QuestionKey,a.Sequence,a.Source," +
        " a.DateStamp,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime," +
        " a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        " a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.FacebookVideoUrl," +
        " a.FacebookVideoDescription,a.FacebookVideoRunningTime,a.FacebookVideoDate," +
        " a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable,a.PoliticianKey," +
        " Convert(i.IssueId, char) AS IssueKey,(i.IssueId>1000) AS IsIssue,i.Issue,qj.IssueLevel,igi.IssueOrder," +
        " q.Question,iq.QuestionOrder,ig.IssueGroupId,ig.Heading,ig.SubHeading" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0" +
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
        " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
        " INNER JOIN Answers2 a ON a.PoliticianKey = @PoliticianKey AND a.QuestionId = qj.QuestionId" +
        " AND(TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl) <> '' AND NOT a.YouTubeUrl IS NULL AND(a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable = '') OR" +
        " TRIM(a.FacebookVideoUrl) <> '' AND NOT a.FacebookVideoUrl IS NULL AND(a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable = ''))" +
        " WHERE" +
        $" {(questionId == null ? Empty : " qj.QuestionId=@QuestionId AND")}" +
        $" {Questions.QuestionsJurisdictionsWhereClause}" +
        $" {(questionId == null ? Empty : " GROUP BY a.PoliticianKey, a.QuestionId, a.Sequence")}" +
        " ORDER BY IsIssue,ig.IssueGroupOrder,igi.IssueOrder,i.Issue,iq.QuestionOrder,q.Question,a.DateStamp DESC, a.Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        if (questionId != null)
          VoteDb.AddCommandParameter(cmd, "QuestionId", questionId.Value);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetVideoAnswers([NotNull] string politicianKey, [NotNull] string officeKey,
      int commandTimeout = -1)
    {
      const string cmdText = "SELECT a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl," +
        "a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        "a.PoliticianKey,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
        "  AND q.IsQuestionOmit=0" +
        " WHERE a.PoliticianKey=@PoliticianKey AND (" +
        " NOT a.YouTubeUrl IS NULL AND TRIM(a.YouTubeUrl)<>'' AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
        " NOT a.FacebookVideoUrl IS NULL AND TRIM(a.FacebookVideoUrl)<>'' AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
        " ) AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
        " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC,Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetVideoAnswersNew([NotNull] string politicianKey,
      [NotNull] string officeKey, int commandTimeout = -1)
    {
      var stagingTest = VotePage.ShowIssues ? Empty : " AND i.IssueId<1000 ";
      // If question is assigned to multiple issues, it is returned multiple times
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText = "SELECT a.Answer,CONVERT(a.QuestionId,char) AS QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl," +
        " a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        " a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        " a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        " a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        " a.PoliticianKey,Convert(i.IssueId, char) AS IssueKey,(i.IssueId>1000) AS IsIssue, i.Issue,qj.IssueLevel,i.IssueOrder,q.Question,iq.QuestionOrder" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        $" INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0 {stagingTest}" +
        " INNER JOIN Answers2 a ON a.PoliticianKey = @PoliticianKey AND a.QuestionId = qj.QuestionId" +
        " AND (TRIM(a.YouTubeUrl) <> '' AND NOT a.YouTubeUrl IS NULL AND(a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable = '') OR" +
        " TRIM(a.FacebookVideoUrl) <> '' AND NOT a.FacebookVideoUrl IS NULL AND(a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable = ''))" +
        " WHERE" +
        $" {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY IsIssue,i.IssueOrder,i.Issue,iq.QuestionOrder,q.Question,a.DateStamp DESC, a.Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static DataTable GetVideoAnswersNew3([NotNull] string politicianKey,
      [NotNull] string officeKey, int commandTimeout = -1)
    {
      // If question is assigned to multiple issues, it is returned multiple times
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText = "SELECT a.Answer,CONVERT(a.QuestionId,char) AS QuestionKey,a.Sequence,a.Source," +
        " a.DateStamp,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime," +
        " a.YouTubeSourceUrl,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        " a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.FacebookVideoUrl,a.FacebookVideoDescription," +
        " a.FacebookVideoRunningTime,a.FacebookVideoDate,a.FacebookVideoRefreshTime," +
        " a.FacebookVideoAutoDisable,a.PoliticianKey,Convert(i.IssueId, char) AS IssueKey,(i.IssueId>1000) AS IsIssue," +
        " i.Issue,qj.IssueLevel,igi.IssueOrder,q.Question,iq.QuestionOrder,ig.Heading" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0" +
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
        " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
        " INNER JOIN Answers2 a ON a.PoliticianKey = @PoliticianKey AND a.QuestionId = qj.QuestionId" +
        " AND (TRIM(a.YouTubeUrl) <> '' AND NOT a.YouTubeUrl IS NULL AND(a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable = '') OR" +
        " TRIM(a.FacebookVideoUrl) <> '' AND NOT a.FacebookVideoUrl IS NULL AND(a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable = ''))" +
        " WHERE" +
        $" {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY IsIssue,ig.IssueGroupOrder,igi.IssueOrder,i.Issue,iq.QuestionOrder,q.Question,a.DateStamp DESC, a.Sequence DESC";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    [NotNull]
    public static AnswersTable GetActiveDataByPoliticianKey([NotNull] string politicianKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT a.PoliticianKey,a.QuestionKey,a.Sequence,a.StateCode,a.IssueKey," +
        "a.Answer,a.Source,a.DateStamp,a.UserName,a.YouTubeUrl,a.YouTubeSource,a.YouTubeSourceUrl," +
        "a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeRunningTime,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
        " WHERE PoliticianKey=@PoliticianKey AND" +
        " (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
        " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
        ")";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return FillTable(cmd, AnswersTable.ColumnSet.All);
    }

    [NotNull]
    public static Answers2Table GetActiveDataByPoliticianKeyNew([NotNull] string politicianKey,
      int commandTimeout = -1)
    {
      // Group to eliminate duplicates caused by questions in multiple issues
      const string cmdText =
        "SELECT a.PoliticianKey,a.QuestionId,a.Sequence," +
        "a.Answer,a.Source,a.DateStamp,a.UserName," +
        "a.YouTubeUrl,a.YouTubeSource,a.YouTubeSourceUrl,a.YouTubeDescription," +
        "a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeRunningTime,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.FacebookVideoUrl," +
        "a.FacebookVideoDescription,a.FacebookVideoRunningTime,a.FacebookVideoDate," +
        "a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable FROM Answers2 a" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId=a.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
        " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId AND q.IsQuestionOmit=0" +
        " WHERE PoliticianKey=@PoliticianKey AND" +
        " (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
        " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
        ") GROUP BY a.PoliticianKey,a.QuestionId,a.Sequence";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return Answers2.FillTable(cmd, Answers2Table.ColumnSet.All);
    }

    //[NotNull]
    //public static AnswersTable GetActiveDataByPoliticianKeys([NotNull] IEnumerable<string> politicianKeys,
    //  int commandTimeout = -1)
    //{
    //  var cmdText =
    //    "SELECT a.PoliticianKey,a.QuestionKey,a.Sequence,a.StateCode,a.IssueKey," +
    //    "a.Answer,a.Source,a.DateStamp,a.UserName,a.YouTubeUrl,a.YouTubeSource," +
    //    "a.YouTubeSourceUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
    //    "a.YouTubeRunningTime,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
    //    "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
    //    "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
    //    "i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder" +
    //    " FROM Answers a" +
    //    " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
    //    " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
    //    $" WHERE PoliticianKey IN ('{Join("','", politicianKeys)}') AND" +
    //    " (TRIM(a.Answer) <> '' OR" +
    //    " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
    //    " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
    //    ")";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  return FillTable(cmd, AnswersTable.ColumnSet.All);
    //}

    public static int GetNextSequence([NotNull] string politicianKey, [NotNull] string questionKey)
    {
      const string cmdText =
        "SELECT MAX(Sequence) FROM Answers WHERE PoliticianKey=@PoliticianKey AND QuestionKey=@QuestionKey";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "QuestionKey", questionKey);
        var result = VoteDb.ExecuteScalar(cmd);
        if (result == null || result == DBNull.Value) return 0;
        return Convert.ToInt32(result) + 1;
      }
    }

    public static int GetNextSequenceNew([NotNull] string politicianKey, [NotNull] string questionKey)
    {
      return GetNextSequenceNew(politicianKey, int.Parse(questionKey));
      //const string cmdText =
      //  "SELECT MAX(Sequence) FROM Answers2 WHERE PoliticianKey=@PoliticianKey AND QuestionId=@QuestionId";
      //var cmd = VoteDb.GetCommand(cmdText);
      //using (var cn = VoteDb.GetOpenConnection())
      //{
      //  cmd.Connection = cn;
      //  VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      //  VoteDb.AddCommandParameter(cmd, "QuestionId", int.Parse(questionKey));
      //  var result = VoteDb.ExecuteScalar(cmd);
      //  if (result == null || result == DBNull.Value) return 0;
      //  return Convert.ToInt32(result) + 1;
      //}
    }

    public static int GetNextSequenceNew([NotNull] string politicianKey, int questionId)
    {
      const string cmdText =
        "SELECT MAX(Sequence) FROM Answers2 WHERE PoliticianKey=@PoliticianKey AND QuestionId=@QuestionId";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "QuestionId", questionId);
        var result = VoteDb.ExecuteScalar(cmd);
        if (result == null || result == DBNull.Value) return 0;
        return Convert.ToInt32(result) + 1;
      }
    }

    [NotNull]
    public static AnswersTable GetDataForYouTubeRefresh(DateTime refreshExpiration,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticianKey,QuestionKey,Sequence,StateCode,IssueKey,Answer," +
        "Source,DateStamp,UserName,YouTubeUrl,YouTubeDescription,YouTubeRunningTime,YouTubeSource," +
        "YouTubeSourceUrl,YouTubeDate,YouTubeRefreshTime,YouTubeAutoDisable,FacebookVideoUrl," +
        "FacebookVideoDescription,FacebookVideoRunningTime,FacebookVideoDate," +
        "FacebookVideoRefreshTime,FacebookVideoAutoDisable" +
        " FROM Answers WHERE YouTubeUrl!='' AND NOT YouTubeUrl IS NULL AND YouTubeRefreshTime<=@RefreshExpiration";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "RefreshExpiration", refreshExpiration);
      return FillTable(cmd, AnswersTable.ColumnSet.All);
    }

    //[NotNull]
    //public static Answers2Table GetDataForYouTubeRefreshNew(DateTime refreshExpiration,
    //  int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT PoliticianKey,QuestionId,Sequence,Answer," +
    //    "Source,DateStamp,UserName,YouTubeUrl,YouTubeDescription,YouTubeRunningTime,YouTubeSource," +
    //    "YouTubeSourceUrl,YouTubeDate,YouTubeRefreshTime,YouTubeAutoDisable,FacebookVideoUrl," +
    //    "FacebookVideoDescription,FacebookVideoRunningTime,FacebookVideoDate," +
    //    "FacebookVideoRefreshTime,FacebookVideoAutoDisable" +
    //    " FROM Answers2 WHERE YouTubeUrl!='' AND NOT YouTubeUrl IS NULL AND YouTubeRefreshTime<=@RefreshExpiration";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "RefreshExpiration", refreshExpiration);
    //  return Answers2.FillTable(cmd, Answers2Table.ColumnSet.All);
    //}

    [NotNull]
    public static Answers2Table GetDataForYouTubeRefreshNew2(int maxRows,
      int commandTimeout = -1)
    {
      // We now do the oldest maxRows only
      var cmdText =
        "SELECT PoliticianKey,QuestionId,Sequence,Answer," +
        "Source,DateStamp,UserName,YouTubeUrl,YouTubeDescription,YouTubeRunningTime,YouTubeSource," +
        "YouTubeSourceUrl,YouTubeDate,YouTubeRefreshTime,YouTubeAutoDisable,FacebookVideoUrl," +
        "FacebookVideoDescription,FacebookVideoRunningTime,FacebookVideoDate," +
        "FacebookVideoRefreshTime,FacebookVideoAutoDisable" +
        $" FROM Answers2 WHERE YouTubeUrl!='' AND NOT YouTubeUrl IS NULL LIMIT {maxRows}";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return Answers2.FillTable(cmd, Answers2Table.ColumnSet.All);
    }
  }
}