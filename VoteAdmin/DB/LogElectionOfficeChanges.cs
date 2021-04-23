using System;

namespace DB.VoteLog
{
  public partial class LogElectionOfficeChangesRow
  {
  }

  public partial class LogElectionOfficeChanges
  {
    public static LogElectionOfficeChangesTable
      GetBillingDataByUserNameDateStampRange(
        string userName, DateTime lowDate, DateTime highDate, int commandTimeout = -1)
    {
      var cmdText = SelectBillingCommandText + " WHERE UserName=@UserName" +
        "    AND DateStamp >= @LowDate" + "    AND DateStamp <= @HighDate" +
        " ORDER BY DateStamp ASC";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserName", userName);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogElectionOfficeChangesTable.ColumnSet.Billing);
    }
  }
}