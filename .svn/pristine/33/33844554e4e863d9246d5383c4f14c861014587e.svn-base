using System;
using System.Collections.Generic;

namespace DB.Vote
{
  public partial class DistrictAddressesView
  {
    public static DistrictAddressesViewReader GetDataReaderForAddressExtraction(
      ICollection<string> stateCodes, bool nameRequired, bool addressRequired,
      bool emailRequired, bool noEmailRequired, bool phoneRequired, DateTime fromDate,
      DateTime toDate, string districtColumnName, ICollection<string> districts,
      int commandTimeout = -1)
    {
      var whereClause = Addresses.FormatAddressExtractionWhereClause(
        stateCodes, nameRequired, addressRequired, emailRequired, noEmailRequired,
        phoneRequired, fromDate, toDate, districtColumnName, districts);
      var cmdText = GetSelectCommandText(DistrictAddressesViewTable.ColumnSet.All) +
        whereClause + " ORDER BY Zip5, Zip4";

      var cn = VoteDb.GetOpenConnection();
      var cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
      if (fromDate != DateTime.MinValue.Date)
        VoteDb.AddCommandParameter(cmd, "fromDate", fromDate);
      if (toDate != DateTime.MaxValue.Date)
        VoteDb.AddCommandParameter(cmd, "toDate", toDate);
      return new DistrictAddressesViewReader(cmd.ExecuteReader(), cn);
    }
  }
}