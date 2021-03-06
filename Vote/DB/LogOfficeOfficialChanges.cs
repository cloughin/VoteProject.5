using System;

namespace DB.VoteLog
{
  public partial class LogOfficeOfficialChangesRow
  {
  }

  public partial class LogOfficeOfficialChanges
  {
    public static LogOfficeOfficialChangesTable
      GetBillingDataByUserNameDateStampRange(
        string userName, DateTime lowDate, DateTime highDate)
    {
      return GetBillingDataByUserNameDateStampRange(userName, lowDate, highDate, -1);
    }

    public static LogOfficeOfficialChangesTable
      GetBillingDataByUserNameDateStampRange(
        string userName, DateTime lowDate, DateTime highDate, int commandTimeout)
    {
      var cmdText = SelectBillingCommandText + " WHERE UserName=@UserName" +
        "    AND DateStamp >= @LowDate" + "    AND DateStamp <= @HighDate" +
        " ORDER BY DateStamp ASC";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserName", userName);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogOfficeOfficialChangesTable.ColumnSet.Billing);
    }
  }
}