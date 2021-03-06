namespace DB.VoteZipNewLocal
{
  public partial class ZipStreetsTempRow
  {
  }

  public partial class ZipStreetsTemp
  {
    public static void Create()
    {
      Create(-1);
    }

    public static void Create(int commandTimeout)
    {
      const string cmdText = "CREATE TABLE `ZipStreetsTemp` (" +
        " `ZipCode` char(5) NOT NULL," + " `UpdateKey` varchar(14) NOT NULL," +
        " `StPreDirAbbr` varchar(2) NOT NULL," + " `StName` varchar(28) NOT NULL," +
        " `StSuffixAbbr` varchar(4) NOT NULL," +
        " `StPostDirAbbr` varchar(2) NOT NULL," +
        " `AddressPrimaryLowNumber` varchar(10) NOT NULL," +
        " `AddressPrimaryHighNumber` varchar(10) NOT NULL," +
        " `AddressPrimaryEvenOdd` varchar(1) NOT NULL," +
        " `BuildingName` varchar(40) NOT NULL," +
        " `AddressSecAbbr` varchar(4) NOT NULL," +
        " `AddressSecLowNumber` varchar(10) NOT NULL," +
        " `AddressSecHighNumber` varchar(10) NOT NULL," +
        " `AddressSecOddEven` varchar(1) NOT NULL," +
        " `Metaphone` varchar(28) NOT NULL," + " `StateCode` char(2) NOT NULL," +
        " `Congress` char(3) NOT NULL," + " `StateSenate` char(3) NOT NULL," +
        " `StateHouse` char(3) NOT NULL," + " `County` char(3) NOT NULL," +
        " PRIMARY KEY (`UpdateKey`)," + " KEY `index2` (`ZipCode`,`StName`)," +
        " KEY `index3` (`ZipCode`,`Metaphone`)" +
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
      const string cmdText = "DROP TABLE IF EXISTS ZipStreetsTemp";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }
  }
}