using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class AnswersViewRow
  {
  }

  public partial class AnswersView
  {
    //public static DataTable GetAllDataByIssueLevelStateCodePoliticianKey(string issueLevel,
    //  string stateCode, string politicianKey, int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT ig.IssueGroupKey,ig.IssueGroupOrder,ig.Heading AS IssueGroupHeading," +
    //    "ig.SubHeading AS IssueGroupSubHeading,i.IssueKey,igi.IssueOrder,i.IssueLevel,i.Issue,i.IsTextSourceOptional," +
    //    "i.StateCode AS StateCode,q.QuestionKey,q.QuestionOrder,q.Question,a.PoliticianKey,a.Source," +
    //    "a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime," +
    //    "a.YouTubeSourceUrl,a.YouTubeDate,a.Sequence,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
    //    "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
    //    "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable FROM Issues i" +
    //    " INNER JOIN IssueGroupsIssues ON IssueGroupsIssues.IssueKey = i.IssueKey" +
    //    " INNER JOIN IssueGroups ig ON ig.IssueGroupKey = IssueGroupsIssues.IssueGroupKey" +
    //    " INNER JOIN Questions q ON q.IssueKey = i.IssueKey" +
    //    " LEFT JOIN Answers a on a.QuestionKey = q.QuestionKey AND a.PoliticianKey=@PoliticianKey" +
    //    " WHERE i.StateCode=@StateCode AND i.IssueLevel=@IssueLevel" +
    //    "  AND i.IsIssueOmit=0 AND q.IsQuestionOmit=0" +
    //    " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC,Sequence DESC";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "IssueLevel", issueLevel);
    //    VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
    //    VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    public static DataTable GetAllDataByPoliticianKeyNew(string politicianKey, string officeKey)
    {
      // Group to eliminate duplicates caused by questions in multiple issues
      if (IsNullOrWhiteSpace(officeKey))
        officeKey = VotePage.GetPageCache().Politicians.GetLiveOfficeKey(politicianKey);
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText = "SELECT CONVERT(ig.IssueGroupId, CHAR) AS IssueGroupKey," +
        "ig.IssueGroupOrder,ig.Heading AS IssueGroupHeading," +
        "ig.SubHeading AS IssueGroupSubHeading,CONVERT(i.IssueId, CHAR) AS IssueKey,i.IssueId," +
        "igi.IssueOrder,qj.IssueLevel,i.Issue," +
        "IF(qj.IssueLevel = 'A', 1, 0) AS IsTextSourceOptional,qj.StateCode AS StateCode," +
        "CONVERT(q.QuestionId, CHAR) AS QuestionKey,q.QuestionId,iq.QuestionOrder,q.Question," +
        "@PoliticianKey AS PoliticianKey,a.Source,a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription," +
        "a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate,a.Sequence," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.FacebookVideoUrl," +
        "a.FacebookVideoDescription,a.FacebookVideoRunningTime,a.FacebookVideoDate," +
        "a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable FROM Issues2 i" +
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
        " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId" +
        " INNER JOIN IssuesQuestions iq ON iq.IssueId = i.IssueId" +
        " INNER JOIN Questions2 q ON q.QuestionId = iq.QuestionId" +
        " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId = q.QuestionId" +
        " LEFT JOIN Answers2 a on a.QuestionId = q.QuestionId" +
        "  AND a.PoliticianKey = @PoliticianKey" +
        " WHERE i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" + 
        $" AND {Questions.QuestionsJurisdictionsWhereClause}" +
        " GROUP BY q.QuestionId,a.Sequence" + 
        " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC, Sequence DESC";
      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    //public static DataTable GetAllDataByIssueKeyPoliticianKey(string issueKey,
    //  string politicianKey)
    //{
    //  const string cmdText =
    //    "SELECT ig.IssueGroupKey,ig.IssueGroupOrder,ig.Heading AS IssueGroupHeading," +
    //    "ig.SubHeading AS IssueGroupSubHeading,i.IssueKey,igi.IssueOrder,i.IssueLevel,i.Issue,i.IsTextSourceOptional," +
    //    "i.StateCode AS StateCode,q.QuestionKey,q.QuestionOrder,q.Question,a.PoliticianKey,a.Source," +
    //    "a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
    //    "a.YouTubeSource,a.YouTubeDate,a.Sequence,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
    //    "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
    //    "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable FROM Issues i" +
    //    " INNER JOIN IssueGroupsIssues igi ON igi.IssueKey = i.IssueKey" +
    //    " INNER JOIN IssueGroups ig ON ig.IssueGroupKey = igi.IssueGroupKey" +
    //    " INNER JOIN Questions q ON q.IssueKey = i.IssueKey" +
    //    " LEFT JOIN Answers a on a.QuestionKey = q.QuestionKey AND a.PoliticianKey=@PoliticianKey" +
    //    " WHERE i.IssueKey=@IssueKey" + "  AND i.IsIssueOmit=0 AND q.IsQuestionOmit=0" +
    //    " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC,Sequence DESC";
    //  var cmd = VoteDb.GetCommand(cmdText);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
    //    VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    public static DataTable GetAllDataByIssueKeyPoliticianKeyNew(int issueId,
      string politicianKey, string officeKey = null)
    {
      // This is keyed by specificIssueId
      // and so will not double-fetch questions in more than one issue
      if (IsNullOrWhiteSpace(officeKey))
        officeKey = VotePage.GetPageCache().Politicians.GetLiveOfficeKey(politicianKey);
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText =
        "SELECT CONVERT(ig.IssueGroupId, CHAR) AS IssueGroupKey, ig.IssueGroupOrder," +
        "ig.Heading AS IssueGroupHeading,ig.SubHeading AS IssueGroupSubHeading," +
        "CONVERT(i.IssueId, CHAR) AS IssueKey, igi.IssueOrder,@Level AS IssueLevel,i.Issue," +
        "CONVERT(q.QuestionId, CHAR) AS QuestionKey, iq.QuestionOrder,q.Question," +
        "a.PoliticianKey,a.Source,a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription," +
        "a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeSource,a.YouTubeDate," +
        "a.Sequence,a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.FacebookVideoUrl," +
        "a.FacebookVideoDescription,a.FacebookVideoRunningTime,a.FacebookVideoDate," +
        "a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        "@IsTextSourceOptional AS IsTextSourceOptional FROM Issues2 i" +
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
        " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId" +
        " INNER JOIN IssuesQuestions iq ON iq.IssueId = i.IssueId" +
        " INNER JOIN Questions2 q ON q.QuestionId = iq.QuestionId" +
        " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId = q.QuestionId" +
        " LEFT JOIN Answers2 a on a.QuestionId = q.QuestionId" +
        "  AND a.PoliticianKey = @PoliticianKey" +
        " WHERE i.IssueId = @IssueId  AND i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" +
        $" AND {Questions.QuestionsJurisdictionsWhereClause}" +
        " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC, Sequence DESC";
      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "IssueId", issueId);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "IsTextSourceOptional", issueId == Issues.IssueId.Biographical.ToInt());
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    //public static DataTable GetDataForConsolidation(string politicianKey1,
    //  string politicianKey2, int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT a.PoliticianKey,a.Answer,a.IssueKey,a.QuestionKey,a.Sequence," +
    //    "a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
    //    "a.YouTubeSource,a.YouTubeDate,i.Issue,i.IssueLevel,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
    //    "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
    //    "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
    //    "i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
    //    " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
    //    " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
    //    " WHERE PoliticianKey IN (@PoliticianKey1,@PoliticianKey2) AND" +
    //    " (TRIM(a.Answer) <> '' OR" +
    //    " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
    //    " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
    //    ")" +
    //    " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC,PoliticianKey";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "PoliticianKey1", politicianKey1);
    //    VoteDb.AddCommandParameter(cmd, "PoliticianKey2", politicianKey2);
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    public static DataTable GetDataForConsolidationNew(string politicianKey1,
      string politicianKey2, int commandTimeout = -1)
    {
      // Group to eliminate duplicates caused by questions in multiple issues
      const string cmdText =
        "SELECT a.PoliticianKey,a.Answer,CONVERT(a.QuestionId,CHAR) AS QuestionKey,a.Sequence," +
        "a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
        "a.YouTubeSource,a.YouTubeDate,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.FacebookVideoUrl,a.FacebookVideoDescription,a.FacebookVideoRunningTime," +
        "a.FacebookVideoDate,a.FacebookVideoRefreshTime,a.FacebookVideoAutoDisable," +
        "q.Question," +
        "CONVERT((SELECT IssueId FROM Issues2 i2 WHERE i2.IssueId = i.IssueId AND i2.IsIssueOmit = 0 ORDER BY i2.IssueOrder LIMIT 1),CHAR) AS IssueKey," +
        "(SELECT Issue FROM Issues2 i2 WHERE i2.IssueId = i.IssueId AND i2.IsIssueOmit = 0 ORDER BY i2.IssueOrder LIMIT 1) AS Issue" + 
        " FROM Answers2 a" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = a.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId=iq.IssueId AND i.IsIssueOmit=0" +
        " INNER JOIN Questions2 q ON q.QuestionId=a.QuestionId AND q.IsQuestionOmit=0" +
        " WHERE PoliticianKey IN (@PoliticianKey1,@PoliticianKey2) AND" +
        " (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='') OR" +
        " TRIM(a.FacebookVideoUrl)<>'' AND NOT a.FacebookVideoUrl IS NULL AND (a.FacebookVideoAutoDisable IS NULL OR a.FacebookVideoAutoDisable='')" +
        ")" +
        " GROUP BY a.PoliticianKey,a.QuestionId,a.Sequence" +
        " ORDER BY QuestionOrder,Question,DateStamp DESC,PoliticianKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey1", politicianKey1);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey2", politicianKey2);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetIssueTopicsByPoliticianKey(string politicianKey, string officeKey = null)
    {
      if (IsNullOrWhiteSpace(officeKey))
        officeKey = VotePage.GetPageCache().Politicians.GetLiveOfficeKey(politicianKey);
      var (stateCode, countyCode, localKey, level) = Offices.GetIssuesCoding(officeKey);
      var cmdText = "SELECT i.IssueId,i.Issue,q.QuestionId,q.Question FROM Issues2 i" +
        " INNER JOIN IssuesQuestions iq ON iq.IssueId = i.IssueId" +
        " INNER JOIN Questions2 q ON q.QuestionId = iq.QuestionId" +
        " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId = q.QuestionId" +
        " WHERE i.IssueId > 1000 AND i.IsIssueOmit = 0 AND q.IsQuestionOmit = 0" +
        $" AND {Questions.QuestionsJurisdictionsWhereClause}" +
        " GROUP BY i.issueId,q.QuestionId" +
        " ORDER BY IssueOrder,QuestionOrder";
      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "Level", level);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
        VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}