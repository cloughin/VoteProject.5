using DB.Vote;

namespace DB.VoteLog
{
  public partial class LogPoliticiansImagesHeadshotRow
  {
  }

  public partial class LogPoliticiansImagesHeadshot
  {
    //public static LogPoliticiansImagesHeadshotTable GetLatestData(
    //  string politicianKey)
    //{
    //  return GetLatestData(politicianKey, -1);
    //}

    public static LogPoliticiansImagesHeadshotTable GetLatestData(
      string politicianKey, int commandTimeout)
    {
      var cmdText =
        "SELECT PrimaryKey,PoliticianKey,HeadshotOriginal,HeadshotDate,UserSecurity,UserName" +
        " FROM LogPoliticiansImagesHeadshot" +
        " WHERE PoliticianKey=@PoliticianKey" + " ORDER BY HeadshotDate DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      VoteLogDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      return FillTable(cmd, LogPoliticiansImagesHeadshotTable.ColumnSet.All);
    }
  }
}