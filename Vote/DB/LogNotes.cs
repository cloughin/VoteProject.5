using DB.Vote;

namespace DB.VoteLog
{
  public partial class LogNotesRow
  {
  }

  public partial class LogNotes
  {
    public static LogNotesTable GetLatestDataByUserStateCode(
      string userStateCode, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + " WHERE UserStateCode=@UserStateCode" +
        " ORDER BY DateStamp DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserStateCode", userStateCode);
      return FillTable(cmd, LogNotesTable.ColumnSet.All);
    }
  }
}