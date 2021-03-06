using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class AnswersRow
  {
  }

  public partial class Answers
  {
    public static DataTable GetAnswers(string politicianKey, string officeKey,
      string questionKey = null,
      int commandTimeout = -1)
    {
      var cmdText =
        "SELECT a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl," +
        "a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.PoliticianKey,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" + "  AND q.IsQuestionOmit=0" +
        " WHERE a.PoliticianKey=@PoliticianKey" +
        $" {(questionKey == null ? string.Empty : "AND a.QuestionKey=@QuestionKey")}" +
        " AND (TRIM(a.Answer) <> '' OR TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL" +
        " AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
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

    public static DataTable GetVideoAnswers(string politicianKey, string officeKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT a.Answer,a.IssueKey,a.QuestionKey,a.Sequence,a.Source,a.DateStamp,a.YouTubeUrl," +
        "a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "a.PoliticianKey,i.Issue,i.IssueLevel,i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey" +
        "  AND q.IsQuestionOmit=0" +
        " WHERE a.PoliticianKey=@PoliticianKey" +
        "  AND NOT a.YouTubeUrl IS NULL AND TRIM(a.YouTubeUrl)<>''" +
        "  AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
        " AND i.IssueLevel IN ('A', GetIssueLevel(@OfficeKey))" +
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

    public static AnswersTable GetActiveDataByPoliticianKey(string politicianKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT a.PoliticianKey,a.QuestionKey,a.Sequence,a.StateCode,a.IssueKey," +
        "a.Answer,a.Source,a.DateStamp,a.UserName,a.YouTubeUrl,a.YouTubeSource,a.YouTubeSourceUrl," +
        "a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl,a.YouTubeRunningTime,a.YouTubeDate," +
        "a.YouTubeRefreshTime,a.YouTubeAutoDisable FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
        " WHERE PoliticianKey=@PoliticianKey AND" +
        " (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
        ")";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return FillTable(cmd, AnswersTable.ColumnSet.All);
    }

    public static int GetNextSequence(string politicianKey, string questionKey)
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
        if ((result == null) || (result == DBNull.Value)) return 0;
        return Convert.ToInt32(result) + 1;
      }
    }

    public static AnswersTable GetDataForYouTubeRefresh(DateTime refreshExpiration,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticianKey,QuestionKey,Sequence,StateCode,IssueKey,Answer," +
        "Source,DateStamp,UserName,YouTubeUrl,YouTubeDescription,YouTubeRunningTime,YouTubeSource," +
        "YouTubeSourceUrl,YouTubeDate,YouTubeRefreshTime,YouTubeAutoDisable" +
        " FROM Answers WHERE YouTubeUrl!='' AND NOT YouTubeUrl IS NULL AND YouTubeRefreshTime<=@RefreshExpiration";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "RefreshExpiration", refreshExpiration);
      return FillTable(cmd, AnswersTable.ColumnSet.All);
    }
  }
}