using System;
using System.Data.Common;

namespace DB.Vote
{
  public partial class Donations
  {
    public static int DeleteByEmailDate(string email, DateTime date)
    {
      const string cmdText = "DELETE FROM Donations WHERE Email=@email AND Date=@date";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "Email", email);
      VoteDb.AddCommandParameter(cmd, "Date", date);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static decimal GetTotalAmountByEmail(string email)
    {
      const string cmdText = "SELECT SUM(Amount) FROM Donations WHERE Email=@Email";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "Email", email);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return 0;
      return (decimal) result;
    }
  }
}