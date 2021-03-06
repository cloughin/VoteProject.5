using System;

namespace DB.VoteLog
{
  public partial class LogInfo
  {
    public static LogInfoTable GetDataByDate(DateTime date, int commandTimeout = -1)
    {
      var lowDate = date.Date;
      var highDate = lowDate.AddDays(1);
      var cmdText = SelectAllCommandText +
        " WHERE DateStamp>=@LowDate AND DateStamp < @HighDate ORDER BY DateStamp";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogInfoTable.ColumnSet.All);
    }
  }
}