namespace DB.Vote
{
  public partial class AnswersViewRow
  {
  }

  public partial class AnswersView
  {
    public static AnswersViewTable GetAllDataByIssueLevelStateCodePoliticianKey(
      string issueLevel, string stateCode, string politicianKey, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT ig.IssueGroupKey,ig.IssueGroupOrder,ig.Heading AS IssueGroupHeading," +
        "ig.SubHeading AS IssueGroupSubHeading,i.IssueKey,i.IssueOrder,i.IssueLevel,i.Issue,i.IsTextSourceOptional," +
        "i.StateCode AS StateCode,q.QuestionKey,q.QuestionOrder,q.Question,a.PoliticianKey,a.Source," +
        "a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeSource,a.YouTubeRunningTime," +
        "a.YouTubeSourceUrl,a.YouTubeDate,a.Sequence,a.YouTubeRefreshTime,a.YouTubeAutoDisable FROM Issues i" +
        " INNER JOIN IssueGroupsIssues ON IssueGroupsIssues.IssueKey = i.IssueKey" +
        " INNER JOIN IssueGroups ig ON ig.IssueGroupKey = IssueGroupsIssues.IssueGroupKey" +
        " INNER JOIN Questions q ON q.IssueKey = i.IssueKey" +
        " LEFT JOIN Answers a on a.QuestionKey = q.QuestionKey AND a.PoliticianKey=@PoliticianKey" +
        " WHERE i.StateCode=@StateCode AND i.IssueLevel=@IssueLevel" +
        "  AND i.IsIssueOmit=0 AND q.IsQuestionOmit=0" +
        " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC,Sequence DESC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "IssueLevel", issueLevel);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return FillTable(cmd, AnswersViewTable.ColumnSet.All);
    }

    public static AnswersViewTable GetAllDataByIssueKeyPoliticianKey(
      string issueKey, string politicianKey, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT ig.IssueGroupKey,ig.IssueGroupOrder,ig.Heading AS IssueGroupHeading," +
        "ig.SubHeading AS IssueGroupSubHeading,i.IssueKey,i.IssueOrder,i.IssueLevel,i.Issue,i.IsTextSourceOptional," +
        "i.StateCode AS StateCode,q.QuestionKey,q.QuestionOrder,q.Question,a.PoliticianKey,a.Source," +
        "a.DateStamp,a.Answer,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
        "a.YouTubeSource,a.YouTubeDate,a.Sequence,a.YouTubeRefreshTime,a.YouTubeAutoDisable FROM Issues i" +
        " INNER JOIN IssueGroupsIssues ON IssueGroupsIssues.IssueKey = i.IssueKey" +
        " INNER JOIN IssueGroups ig ON ig.IssueGroupKey = IssueGroupsIssues.IssueGroupKey" +
        " INNER JOIN Questions q ON q.IssueKey = i.IssueKey" +
        " LEFT JOIN Answers a on a.QuestionKey = q.QuestionKey AND a.PoliticianKey=@PoliticianKey" +
        " WHERE i.IssueKey=@IssueKey" +
        "  AND i.IsIssueOmit=0 AND q.IsQuestionOmit=0" +
        " ORDER BY IssueGroupOrder,IssueOrder,QuestionOrder,DateStamp DESC,Sequence DESC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "IssueKey", issueKey);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return FillTable(cmd, AnswersViewTable.ColumnSet.All);
    }


    public static AnswersViewTable GetDataForConsolidation(string politicianKey1,
      string politicianKey2, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT a.PoliticianKey,a.Answer,a.IssueKey,a.QuestionKey,a.Sequence," +
        "a.Source,a.DateStamp,a.YouTubeUrl,a.YouTubeDescription,a.YouTubeRunningTime,a.YouTubeSourceUrl," +
        "a.YouTubeSource,a.YouTubeDate,i.Issue,i.IssueLevel,a.YouTubeRefreshTime,a.YouTubeAutoDisable," +
        "i.IssueOrder,q.Question,q.QuestionOrder FROM Answers a" +
        " INNER JOIN Issues i ON i.IssueKey=a.IssueKey AND i.IsIssueOmit=0" +
        " INNER JOIN Questions q ON q.QuestionKey=a.QuestionKey AND q.IsQuestionOmit=0" +
        " WHERE PoliticianKey IN (@PoliticianKey1,@PoliticianKey2) AND" +
        " (TRIM(a.Answer) <> '' OR" +
        " TRIM(a.YouTubeUrl)<>'' AND NOT a.YouTubeUrl IS NULL AND (a.YouTubeAutoDisable IS NULL OR a.YouTubeAutoDisable='')" +
        ")" +
        " ORDER BY IssueLevel,IssueOrder,Issue,QuestionOrder,Question,DateStamp DESC,PoliticianKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey1", politicianKey1);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey2", politicianKey2);
      return FillTable(cmd, AnswersViewTable.ColumnSet.All);
    }

    //public static AnswersViewTable GetAllResponsesByQuestionKey(
    //  string questionKey, int commandTimeout = -1)
    //{
    //  const string cmdText = "SELECT a.Source,a.DateStamp,a.Answer,a.Sequence FROM Answers a" +
    //    " WHERE a.QuestionKey=@QuestionKey" +
    //    " ORDER BY DateStamp DESC,Sequence DESC";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "QuestionKey", questionKey);
    //  return FillTable(cmd, AnswersViewTable.ColumnSet.All);
    //}
  }
}