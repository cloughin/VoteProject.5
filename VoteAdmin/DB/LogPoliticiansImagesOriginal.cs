using System;
using DB.Vote;

namespace DB.VoteLog
{
  public partial class LogPoliticiansImagesOriginalRow
  {
  }

  public partial class LogPoliticiansImagesOriginal
  {
    public static LogPoliticiansImagesOriginalTable
      GetBillingDataByUserNameDateStampRange(string userName, DateTime lowDate,
        DateTime highDate, int commandTimeout = -1)
    {
      var cmdText = SelectBillingCommandText + " WHERE UserName=@UserName" +
        "    AND ProfileOriginalDate >= @LowDate" +
        "    AND ProfileOriginalDate <= @HighDate" +
        " ORDER BY ProfileOriginalDate ASC";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserName", userName);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogPoliticiansImagesOriginalTable.ColumnSet.Billing);
    }

    public static LogPoliticiansImagesOriginalTable GetTwoLatestImageDateAndUsers(
      string politicianKey)
    {
      var cmdText =
        "SELECT ProfileOriginalDate,UserName FROM LogPoliticiansImagesOriginal" +
        " WHERE PoliticianKey=@PoliticianKey ORDER BY ProfileOriginalDate DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 2);
      var cmd = VoteLogDb.GetCommand(cmdText, -1);
      VoteLogDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      var table = FillTable(cmd, LogPoliticiansImagesOriginalTable.ColumnSet.DateAndUser);
      return table;
    }
  }
}