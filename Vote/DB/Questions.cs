using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class QuestionsRow
  {
  }

  public partial class Questions
  {
    public static QuestionsTable GetNonOmittedDataByIssueKey(string issueKey,
      int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + " WHERE IssueKey=@IssueKey" +
        "  AND IsQuestionOmit = 0" + " ORDER BY QuestionOrder";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
      return FillTable(cmd, QuestionsTable.ColumnSet.All);
    }

    //public static DataTable GetQuestionListByOfficeKey(string officeKey,
    //  string electionKey = null, int commandTimeout = -1)
    //{
    //  const string cmdText = "CALL GetQuestionListByOfficeKey(@OfficeKey,@ElectionKey)";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
    //    VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    //public static DataTable GetQuestionListByPoliticianeKey(string politicianKey,
    //  string electionKey = null, int commandTimeout = -1)
    //{
    //  const string cmdText = "CALL GetQuestionListByPoliticianKey(@PoliticianKey,@ElectionKey)";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //    VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    public static DataTable GetQuestionListByStateCode(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT i.IssueKey,i.Issue,i.IssueLevel AS SortIssueLevel,i.IssueOrder," +
        " q.QuestionKey,q.Question,q.QuestionOrder FROM Issues i" +
        " INNER JOIN Questions q ON q.IssueKey=i.IssueKey AND q.IsQuestionOmit=0" +
        " WHERE (i.StateCode='LL' AND i.IssueLevel='A'" +
        " OR StateCode='US' AND IssueLevel='B'" +
        " OR StateCode=@StateCode AND IssueLevel='C') AND IsIssueOmit=0" +
        " ORDER BY SortIssueLevel,IssueOrder,QuestionOrder";

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