using System;

namespace DB.VoteImagesLocal
{
  public partial class PoliticiansImagesDataRow
  {
  }

  public partial class PoliticiansImagesData
  {
    public static void Upsert(string politicianKey, DateTime profileOriginalDate,
      DateTime headshotDate, DateTime headshotResizeDate, DateTime refreshDate,
      int commandTimeout = -1)
    {
      const string cmdText =
        "INSERT INTO PoliticiansImagesData" +
        " (PoliticianKey,ProfileOriginalDate,HeadshotDate,HeadshotResizeDate,RefreshDate)" +
        " VALUES (@PoliticianKey,@ProfileOriginalDate,@HeadshotDate,@HeadshotResizeDate,@RefreshDate)" +
        " ON DUPLICATE KEY UPDATE ProfileOriginalDate=VALUES(ProfileOriginalDate)," +
        "HeadshotDate=VALUES(HeadshotDate),HeadshotResizeDate=VALUES(HeadshotResizeDate)," +
        "RefreshDate=VALUES(RefreshDate)";
      var cmd = VoteImagesLocalDb.GetCommand(cmdText, commandTimeout);
      VoteImagesLocalDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      VoteImagesLocalDb.AddCommandParameter(cmd, "ProfileOriginalDate", profileOriginalDate);
      VoteImagesLocalDb.AddCommandParameter(cmd, "HeadshotDate", headshotDate);
      VoteImagesLocalDb.AddCommandParameter(cmd, "HeadshotResizeDate", headshotResizeDate);
      VoteImagesLocalDb.AddCommandParameter(cmd, "RefreshDate", refreshDate);
      VoteImagesLocalDb.ExecuteNonQuery(cmd);
    }
  }
}