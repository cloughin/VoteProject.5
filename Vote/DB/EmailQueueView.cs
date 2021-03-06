namespace DB.Vote
{
  public partial class EmailQueueViewRow
  {
  }

  public partial class EmailQueueView
  {
    public static EmailQueueViewTable GetPendingEmail(
      int maxEmails, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + " WHERE SentTime IS NULL" +
        "  AND IsClosed=0" + "  AND TRIM(Subject)<>''" + "  AND TRIM(Template)<>''";
      if (maxEmails > 0)
        cmdText = VoteDb.InjectSqlLimit(cmdText, maxEmails);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, EmailQueueViewTable.ColumnSet.All);
    }
  }
}