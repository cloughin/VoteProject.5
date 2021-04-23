using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class Issues
  {
    public static DataTable GetIssuesReportData(string issueLevel, string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT i.IssueKey,i.IssueOrder,i.Issue,i.IsIssueOmit," +
        "q.QuestionKey,q.Question,q.IsQuestionOmit,COUNT(*) AS Count" +
        " FROM Issues i" +
        " LEFT JOIN Questions q ON q.IssueKey = i.IssueKey" +
        " LEFT JOIN Answers a ON a.QuestionKey=q.QuestionKey" +
        " WHERE i.StateCode=@StateCode AND i.IssueLevel=@IssueLevel" +
        " GROUP BY i.IssueKey,q.QuestionKey" +
        " ORDER BY i.IssueOrder,q.QuestionOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "IssueLevel", issueLevel);
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetIssuesSummaryReportData(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT i.StateCode,i.IssueKey,i.IssueOrder,i.Issue,i.IsIssueOmit,COUNT(*) AS Count" +
        " FROM Issues i" +
        " LEFT JOIN Questions q ON q.IssueKey = i.IssueKey" +
        " WHERE i.StateCode=@StateCode" +
        " GROUP BY i.IssueKey" +
        " ORDER BY i.IsIssueOmit,IssueOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}