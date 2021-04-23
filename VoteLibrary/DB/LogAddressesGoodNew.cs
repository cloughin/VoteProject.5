using System;

namespace DB.VoteLog
{
  //public partial class LogAddressesGoodNewRow
  //{
  //}

  //public partial class LogAddressesGoodNew
  //{
  //  public static LogAddressesGoodNewReader GetDataReaderByNotTransferredToAddresses(
  //    int commandTimeout = -1)
  //  {
  //    var cmdText = GetSelectCommandText(LogAddressesGoodNewTable.ColumnSet.All) +
  //      " WHERE TransferredToAddresses='0'";
  //    var cn = VoteLogDb.GetOpenConnection();
  //    var cmd = VoteLogDb.GetCommand(cmdText, cn, commandTimeout);
  //    return new LogAddressesGoodNewReader(cmd.ExecuteReader(), cn);
  //  }

  //  //public static LogAddressesGoodNewReader GetDataReaderByParsedStateCodeDateRange(
  //  //  string parsedStateCode, DateTime startDate, DateTime endDate,
  //  //  int commandTimeout = -1)
  //  //{
  //  //  var cmdText = GetSelectCommandText(LogAddressesGoodNewTable.ColumnSet.All) +
  //  //    " WHERE ParsedStateCode=@ParsedStateCode" +
  //  //    "   AND DATE(DateStamp)>=@StartDate" + "   AND DATE(DateStamp)<=@EndDate";
  //  //  var cn = VoteLogDb.GetOpenConnection();
  //  //  var cmd = VoteLogDb.GetCommand(cmdText, cn, commandTimeout);
  //  //  VoteLogDb.AddCommandParameter(cmd, "ParsedStateCode", parsedStateCode);
  //  //  VoteLogDb.AddCommandParameter(cmd, "StartDate", startDate);
  //  //  VoteLogDb.AddCommandParameter(cmd, "EndDate", endDate);
  //  //  return new LogAddressesGoodNewReader(cmd.ExecuteReader(), cn);
  //  //}
  //}
}