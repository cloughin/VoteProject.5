namespace DB.VoteLog
{
  public partial class LogSampleBallotRequestsRow
  {
  }

  public partial class LogSampleBallotRequests
  {
    public static LogSampleBallotRequestsReader GetDataReaderByNotTransferredToAddresses(
      int commandTimeout = -1)
    {
      var cmdText = GetSelectCommandText(LogSampleBallotRequestsTable.ColumnSet.All) +
        " WHERE TransferredToAddresses<>1 OR TransferredToAddresses IS NULL";
      var cn = VoteLogDb.GetOpenConnection();
      var cmd = VoteLogDb.GetCommand(cmdText, cn, commandTimeout);
      return new LogSampleBallotRequestsReader(cmd.ExecuteReader(), cn);
    }
  }
}