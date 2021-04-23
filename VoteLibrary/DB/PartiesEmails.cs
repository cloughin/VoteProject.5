using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class PartiesEmails
  {
    public static DataTable GetPartiesEmailReportData(string partyCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PartyKey,PartyEmail,PartyPassword,PartyContactFName,PartyContactLName," +
        "PartyContactPhone,PartyContactTitle" +
        " FROM PartiesEmails" +
        " WHERE PartyKey=@PartyKey AND IsVolunteer = 0" +
        " ORDER BY PartyEmail";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PartyKey", partyCode);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}