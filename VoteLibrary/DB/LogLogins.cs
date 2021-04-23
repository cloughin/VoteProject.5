using System;
using System.Data;
using System.Data.Common;
using DB.Vote;
using MySql.Data.MySqlClient;

namespace DB.VoteLog
{
  public partial class LogLogins
  {
    public static LogLoginsTable GetDataByUserNameDateStampRange(
      string userName, DateTime lowDate, DateTime highDate, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + " WHERE UserName=@UserName" +
        " AND DateStamp >= @LowDate AND DateStamp <= @HighDate" +
        " ORDER BY DateStamp ASC";
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "UserName", userName);
      VoteLogDb.AddCommandParameter(cmd, "LowDate", lowDate);
      VoteLogDb.AddCommandParameter(cmd, "HighDate", highDate);
      return FillTable(cmd, LogLoginsTable.ColumnSet.All);
    }

    public static DataTable GetPoliticianLoginsByDateStampRange(DateTime lowDate, 
      DateTime? highDate = null, int commandTimeout = -1)
    {
      lowDate = lowDate.Date;
      if (highDate == null) highDate = lowDate;
      highDate = highDate.Value.AddDays(1).Date;

      const string cmdText = "SELECT l.DateStamp,p.StateCode,p.PoliticianKey,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LName AS LastName,p.Nickname,p.Suffix,p.LiveOfficeStatus," +
        "o.OfficeKey,o.OfficeLine1,o.OfficeLine2,e.ElectionDesc FROM votelog.LogLogins l" +
        " INNER JOIN vote.Politicians p ON p.PoliticianKey=l.UserPoliticianKey" +
        " LEFT OUTER JOIN vote.Offices o ON o.OfficeKey=p.LiveOfficeKey" +
        " LEFT OUTER JOIN vote.Elections e ON e.ElectionKey=p.LiveElectionKey" +
        " WHERE l.UserSecurity='POLITICIAN'" +
        " AND l.DateStamp>=@LowDate AND l.DateStamp<@HighDate" +
        " GROUP BY DATE(l.DateStamp),p.PoliticianKey" +
        " ORDER BY DATE(l.DateStamp),p.StateCode,p.LName,p.FName,p.MName,p.Suffix";

      using (var cn = VoteDb.GetOpenConnection())
      {
        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "LowDate", lowDate);
        VoteDb.AddCommandParameter(cmd, "HighDate", highDate);
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }
  }
}