using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class Questions
  {
    public const string QuestionsJurisdictionsWhereClause = 
      "(qj.IssueLevel = 'A'" + 
      " OR @Level = 'B' AND qj.IssueLevel = 'B'" + 
      " OR @Level = 'C' AND qj.IssueLevel = 'C' AND qj.StateCode IN(@StateCode,'')" + 
      " OR @Level = 'D' AND qj.IssueLevel = 'D' AND qj.StateCode IN(@StateCode,'') AND qj.CountyOrLocal IN(@CountyCode,'')" + 
      " OR @Level = 'E' AND qj.IssueLevel = 'E' AND qj.StateCode IN(@StateCode,'') AND qj.CountyOrLocal IN(@LocalKey,''))";

    //public static QuestionsTable GetNonOmittedDataByIssueKey(string issueKey,
    //  int commandTimeout = -1)
    //{
    //  var cmdText = SelectAllCommandText + " WHERE IssueKey=@IssueKey" +
    //    "  AND IsQuestionOmit = 0" + " ORDER BY QuestionOrder";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
    //  return FillTable(cmd, QuestionsTable.ColumnSet.All);
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

    public static DataTable GetQuestionListByJurisdictionNew(string stateCode,
      string countyCode = null, string localKey = null, int commandTimeout = -1)
    {
      var stagingTest = VotePage.ShowIssues ? Empty : " AND i.IssueId<1000 ";
      // If a quesion appears in more than one issue we want to fetch it twice
      var level = "A";
      if (!IsNullOrWhiteSpace(stateCode))
        if (StateCache.IsUS(stateCode)) level = "B";
        else if (!IsNullOrWhiteSpace(countyCode)) level = "D";
        else if (!IsNullOrWhiteSpace(localKey)) level = "E";
        else level = "C";
      var cmdText = "SELECT qj.IssueLevel,qj.StateCode,q.QuestionId,q.Question,iq.QuestionOrder," +
        " i.IssueId,i.IssueOrder,i.Issue" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        $" INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0 {stagingTest}" +
        $" WHERE {QuestionsJurisdictionsWhereClause}" +
        " ORDER BY i.IssueOrder,iq.QuestionOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
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

    public static DataTable GetQuestionListByJurisdictionNew3(string stateCode,
      string countyCode = null, string localKey = null, int commandTimeout = -1)
    {
      // If a quesion appears in more than one issue we want to fetch it twice
      var level = "A";
      if (!IsNullOrWhiteSpace(stateCode))
        if (StateCache.IsUS(stateCode)) level = "B";
        else if (!IsNullOrWhiteSpace(countyCode)) level = "D";
        else if (!IsNullOrWhiteSpace(localKey)) level = "E";
        else level = "C";
      var cmdText = "SELECT qj.IssueLevel,qj.StateCode,q.QuestionId,q.Question," +
        " iq.QuestionOrder,i.IssueId,i.IssueOrder,i.Issue,ig.IssueGroupId,ig.Heading,ig.SubHeading" +
        " FROM QuestionsJurisdictions qj" +
        " INNER JOIN Questions2 q ON q.QuestionId = qj.QuestionId AND q.IsQuestionOmit = 0" +
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId = q.QuestionId" +
        " INNER JOIN Issues2 i ON i.IssueId = iq.IssueId AND i.IsIssueOmit = 0" +
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId = i.IssueId" +
        " INNER JOIN IssueGroups2 ig ON ig.IssueGroupId = igi.IssueGroupId AND ig.IsEnabled=1" +
        $" WHERE {QuestionsJurisdictionsWhereClause}" +
        " ORDER BY ig.IssueGroupOrder,i.IssueOrder,iq.QuestionOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
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

    public static DataTable GetQuestionsSummaryReportData(string issueKey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT q.QuestionKey,q.QuestionOrder,q.Question,q.IsQuestionOmit,q.IssueKey,COUNT(*) AS Count" +
        " FROM Questions q" +
        " LEFT JOIN Answers a ON a.QuestionKey = q.QuestionKey" +
        " WHERE q.IssueKey=@IssueKey" +
        " GROUP BY q.QuestionKey" +
        " ORDER BY q.IsQuestionOmit,q.QuestionOrder";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static bool QuestionExists(string question, string issueLevel)
    {
      const string cmdText = "SELECT COUNT(*) FROM Questions WHERE Question=@Question AND" +
        "SUBSTRING(QuestionKey,1,1)=@IssueLevel";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "Question", question);
      VoteDb.AddCommandParameter(cmd, "IssueLevel", issueLevel);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }
  }
}