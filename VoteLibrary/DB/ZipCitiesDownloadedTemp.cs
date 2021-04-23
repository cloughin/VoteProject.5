namespace DB.VoteZipNewLocal
{
  public class ZipCitiesDownloadedTempRow
  {
  }

  public class ZipCitiesDownloadedTemp
  {
    public static void Create()
    {
      Create(-1);
    }

    public static void Create(int commandTimeout)
    {
      const string cmdText = "CREATE TABLE `ZipCitiesDownloadedTemp` (" +
        " `ZipCode` char(5) DEFAULT NULL," + " `City` varchar(35) DEFAULT NULL," +
        " `State` char(2) DEFAULT NULL," + " `County` varchar(45) DEFAULT NULL," +
        " `AreaCode` varchar(55) DEFAULT NULL," + " `CityType` char(1) DEFAULT NULL," +
        " `CityAliasAbbreviation` varchar(13) DEFAULT NULL," +
        " `CityAliasName` varchar(35) DEFAULT NULL," +
        " `Latitude` decimal(18,0) DEFAULT NULL," +
        " `Longitude` decimal(18,0) DEFAULT NULL," +
        " `TimeZone` char(2) DEFAULT NULL," + " `Elevation` int(10) DEFAULT NULL," +
        " `CountyFIPS` char(3) DEFAULT NULL," +
        " `DayLightSaving` char(1) DEFAULT NULL," +
        " `PreferredLastLineKey` varchar(10) DEFAULT NULL," +
        " `ClassificationCode` char(1) DEFAULT NULL," +
        " `MultiCounty` char(1) DEFAULT NULL," + " `StateFIPS` char(2) DEFAULT NULL," +
        " `CityStateKey` char(6) DEFAULT NULL," +
        " `CityAliasCode` varchar(5) DEFAULT NULL," +
        " `PrimaryRecord` char(1) DEFAULT NULL," +
        " `CityMixedCase` varchar(35) DEFAULT NULL," +
        " `CityAliasMixedCase` varchar(35) DEFAULT NULL," +
        " `StateANSI` varchar(2) DEFAULT NULL," +
        " `CountyANSI` varchar(3) DEFAULT NULL," +
        " `FacilityCode` varchar(1) DEFAULT NULL," +
        " `CityDeliveryIndicator` varchar(1) DEFAULT NULL," +
        " `CarrierRouteRateSortation` varchar(1) DEFAULT NULL," +
        " `FinanceNumber` varchar(6) DEFAULT NULL," +
        " `UniqueZIPName` varchar(1) DEFAULT NULL," +
        " `MetaphoneAliasName` varchar(35) DEFAULT NULL," +
        " `MetaphoneAliasAbbreviation` varchar(13) DEFAULT NULL," +
        " UNIQUE KEY `index4` (`ZipCode`,`CityAliasName`)," +
        " KEY `index1` (`ZipCode`)," + " KEY `index2` (`State`,`CityAliasName`)," +
        " KEY `index3` (`CityAliasName`)," +
        " KEY `index5` (`State`,`MetaphoneAliasName`)," +
        " KEY `index6` (`State`,`CityAliasAbbreviation`)," +
        " KEY `index7` (`CityAliasAbbreviation`)," +
        " KEY `index8` (`State`,`MetaphoneAliasAbbreviation`)" +
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
      const string cmdText = "DROP TABLE IF EXISTS ZipCitiesDownloadedTemp";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }
  }
}