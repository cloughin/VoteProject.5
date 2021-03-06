using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  public partial class CountiesRow
  {
  }

  public partial class Counties
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static DataTable GetCountiesReportData(string stateCode, int commandTimeout = -1)
    {
      const string cmdText = "SELECT County,Contact,ContactEmail,AltContact,AltEMail" +
        " FROM Counties WHERE StateCode=@StateCode" +
        " ORDER BY County";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}