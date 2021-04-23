using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
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

    public static DataTable GetJurisdictionsReportData(string stateCode)
    {
      const string cmdText = "SELECT StateCode,CountyCode,County,URL as Url,Contact," +
        " ContactEmail,AltContact,AltEmail FROM Counties" +
        " WHERE StateCode=@StateCode" +
        " ORDER BY County";

      var cmd = VoteDb.GetCommand(cmdText, 0);
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

    public static DataTable GetOfficesForCsv(string stateCode)
    {
      const string cmdText =
        "SELECT c.StateCode,c.CountyCode,c.County,o.OfficeKey,o.OfficeLine1," +
        "o.OfficeLine2,p.PoliticianKey,p.FName AS FirstName,p.MName AS MiddleName,p.Nickname," +
        "p.LName AS LastName,p.Suffix," + 
        "(SELECT ElectionKey FROM ElectionsPoliticians ep" +
        " WHERE ep.OfficeKey = o.OfficeKey" +
        "  AND NOT SUBSTR(ep.ElectionKey, 11, 1) IN ('A', 'B', 'P', 'Q')" +
        " ORDER BY ep.ElectionKey DESC Limit 1) AS ElectionKey" + 
        " FROM Counties c" +
        " LEFT OUTER JOIN Offices o ON o.StateCode = c.StateCode AND o.CountyCode = c.CountyCode" +
        "  AND o.IsVirtual = 0 AND o.IsInactive = 0" +
        " LEFT OUTER JOIN OfficesOfficials oo ON oo.OfficeKey = o.OfficeKey" +
        " LEFT OUTER JOIN Politicians p ON p.PoliticianKey = oo.PoliticianKey" +
        " WHERE c.IsCountyTagForDeletion = 0 AND c.StateCode = @StateCode" +
        " ORDER BY c.County,o.OfficeLine1,o.OfficeLine2,LastName,FirstName";

      var cmd = VoteDb.GetCommand(cmdText, 0);
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

    public static List<SimpleListItem> GetSearchCountiesInState(string stateCode,
      string searchString)
    {
      searchString = searchString.Trim();
      const string cmdText = "SELECT CountyCode,County FROM Counties" +
        " WHERE StateCode=@StateCode AND County LIKE @CountyMatchAny" +
        " AND IsCountyTagForDeletion=0" +
        " ORDER BY County LIKE @CountyMatchStart DESC,County";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyMatchAny", $"%{searchString}%");
      VoteDb.AddCommandParameter(cmd, "CountyMatchStart", $"{searchString}%");
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().Select(r => new SimpleListItem
        {
          Text = r.County(),
          Value = r.CountyCode()
        }).ToList();
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