namespace DB.Vote
{
  public partial class Sitemap
  {
    public static SitemapTable GetDomainDataSorted(int commandTimeout = -1)
    {
      const string cmdText = "SELECT DomainDataCode,DomainName FROM Sitemap" +
        " ORDER BY DomainDataCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, SitemapTable.ColumnSet.Domain);
    }

    public static int UpdateColumnAllRows(Column column, object newValue)
    {
      var cmdText = "UPDATE Sitemap SET {0}=@newValue";
      cmdText = string.Format(cmdText, GetColumnName(column));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateElectionDirectoriesAllRows(string newValue)
    {
      const string cmdText = "UPDATE Sitemap SET ElectionDirectories=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateFactorIssueAllRows(int newValue)
    {
      const string cmdText = "UPDATE Sitemap SET FactorIssue=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateMinimumAnswersAllRows(int newValue)
    {
      const string cmdText = "UPDATE Sitemap SET MinimumAnswers=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateMinimumCandidatesAllRows(int newValue)
    {
      const string cmdText = "UPDATE Sitemap SET MinimumCandidates=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateMustHavePictureAllRows(bool newValue)
    {
      const string cmdText = "UPDATE Sitemap SET MustHavePicture=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdateMustHaveStatementAllRows(bool newValue)
    {
      const string cmdText = "UPDATE Sitemap SET MustHaveStatement=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static int UpdatePoliticianElectionsAllRows(string newValue)
    {
      const string cmdText = "UPDATE Sitemap SET PoliticianElections=@newValue";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }
  }
}