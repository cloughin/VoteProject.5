using System;

namespace DB.VoteCache
{
  public partial class CachePagesRow
  {
  }

  public partial class CachePages
  {
    public static CachePagesTable GetUnexpiredPageImage(string pageType,
      string pageKey, DateTime minDateStamp, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText +
        " WHERE PageType=@PageType AND PageKey=@PageKey AND DateStamp>=@DateStamp";
      var cmd = VoteCacheDb.GetCommand(cmdText, commandTimeout);
      VoteCacheDb.AddCommandParameter(cmd, "PageType", pageType);
      VoteCacheDb.AddCommandParameter(cmd, "PageKey", pageKey);
      VoteCacheDb.AddCommandParameter(cmd, "DateStamp", minDateStamp);
      return FillTable(cmd, CachePagesTable.ColumnSet.All);
    }

    public static void Upsert(string pageType, string pageKey,
      DateTime dateStamp, byte[] pageImage, int commandTimeout = -1)
    {
      const string cmdText =
        "INSERT INTO CachePages (PageType,PageKey,DateStamp,PageImage)" +
        " VALUES (@PageType,@PageKey,@DateStamp,@PageImage)" +
        " ON DUPLICATE KEY UPDATE DateStamp=VALUES(DateStamp),PageImage=VALUES(PageImage)";
      var cmd = VoteCacheDb.GetCommand(cmdText, commandTimeout);
      VoteCacheDb.AddCommandParameter(cmd, "PageType", pageType);
      VoteCacheDb.AddCommandParameter(cmd, "PageKey", pageKey);
      VoteCacheDb.AddCommandParameter(cmd, "DateStamp", dateStamp);
      VoteCacheDb.AddCommandParameter(cmd, "PageImage", pageImage);
      VoteCacheDb.ExecuteNonQuery(cmd);
    }
  }
}

namespace DB.VoteCacheLocal
{
  public partial class CachePagesRow
  {
  }

  public partial class CachePages
  {
    public static CachePagesTable GetUnexpiredPageImage(string pageType,
      string pageKey, DateTime minDateStamp, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText +
        " WHERE PageType=@PageType AND PageKey=@PageKey AND DateStamp>=@DateStamp";
      var cmd = VoteCacheLocalDb.GetCommand(cmdText, commandTimeout);
      VoteCacheLocalDb.AddCommandParameter(cmd, "PageType", pageType);
      VoteCacheLocalDb.AddCommandParameter(cmd, "PageKey", pageKey);
      VoteCacheLocalDb.AddCommandParameter(cmd, "DateStamp", minDateStamp);
      return FillTable(cmd, CachePagesTable.ColumnSet.All);
    }

    public static void Upsert(string pageType, string pageKey,
      DateTime dateStamp, byte[] pageImage, int commandTimeout = -1)
    {
      const string cmdText =
        "INSERT INTO CachePages (PageType,PageKey,DateStamp,PageImage)" +
        " VALUES (@PageType,@PageKey,@DateStamp,@PageImage)" +
        " ON DUPLICATE KEY UPDATE DateStamp=VALUES(DateStamp),PageImage=VALUES(PageImage)";
      var cmd = VoteCacheLocalDb.GetCommand(cmdText, commandTimeout);
      VoteCacheLocalDb.AddCommandParameter(cmd, "PageType", pageType);
      VoteCacheLocalDb.AddCommandParameter(cmd, "PageKey", pageKey);
      VoteCacheLocalDb.AddCommandParameter(cmd, "DateStamp", dateStamp);
      VoteCacheLocalDb.AddCommandParameter(cmd, "PageImage", pageImage);
      VoteCacheLocalDb.ExecuteNonQuery(cmd);
    }
  }
}