using System;

namespace DB.Vote
{
  public partial class Donations
  {
    public static decimal GetTotalAmountByEmail(string email)
    {
      const string cmdText = "SELECT SUM(Amount) FROM Donations WHERE Email=@Email";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "Email", email);
      var result = VoteDb.ExecuteScalar(cmd);
      if ((result == null) || (result == DBNull.Value)) return 0;
      return (decimal) result;
    }
  }
}