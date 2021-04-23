using DB.VoteZipNewLocal;

namespace LoadUszdSingleState
{
  public static class UszdNew
  {
    public static void DeleteByLdsStateCode(string ldsStateCode, int commandTimeout)
    {
      const string cmdText = "DELETE FROM USZDNew WHERE ST=@LdsStateCode";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "LdsStateCode", ldsStateCode);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }
  }
}
