using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class PartiesRow
  {
  }

  public partial class Parties
  {
    public static PartiesTable GetMajorPartyDataByStateCode(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PartyKey,PartyCode,StateCode,PartyOrder,PartyName,PartyURL,PartyAddressLine1," +
        "PartyAddressLine2,PartyCityStateZip,IsPartyMajor FROM Parties" +
        " WHERE StateCode=@StateCode AND IsPartyMajor=1 AND LENGTH(PartyCode)=1" +
        " ORDER BY PartyOrder";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      return FillTable(cmd, PartiesTable.ColumnSet.All);
    }

    public static DataTable GetPartiesSummaryReportData(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PartyKey,PartyCode,StateCode,PartyOrder,PartyName," +
        "PartyURL AS PartyUrl,IsPartyMajor" +
        " FROM Parties" +
        " WHERE StateCode=@StateCode" +
        " ORDER BY IsPartyMajor DESC, PartyOrder, PartyName";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}