namespace DB.Vote
{
  public partial class EmailQueue
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static EmailQueueTable GetPendingEmail(int maxEmails, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + " WHERE SentTime IS NULL";
      if (maxEmails > 0)
        cmdText = VoteDb.InjectSqlLimit(cmdText, maxEmails);
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, EmailQueueTable.ColumnSet.All);
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}