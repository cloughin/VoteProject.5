namespace DB.VoteZipNewLocal
{
  public partial class ZipSingleUszdTempRow
  {
  }

  public partial class ZipSingleUszdTemp
  {
    public static void Create()
    {
      Create(-1);
    }

    public static void Create(int commandTimeout)
    {
      const string cmdText = "CREATE TABLE `ZipSingleUSZDTemp` (" +
        " `ZipCode` char(5) NOT NULL," + " `Congress` char(2) NOT NULL," +
        " `StateSenate` char(3) NOT NULL," + " `StateHouse` char(3) NOT NULL," +
        " `LdsStateCode` char(2) NOT NULL," + " `County` char(3) NOT NULL," +
        " `StateCode` char(2) NOT NULL," + " PRIMARY KEY (`ZipCode`)" +
        " ) ENGINE=InnoDB DEFAULT CHARSET=utf8";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }

    public static void Drop()
    {
      Drop(-1);
    }

    public static void Drop(int commandTimeout)
    {
      const string cmdText = "DROP TABLE IF EXISTS ZipSingleUszdTemp";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }
  }
}