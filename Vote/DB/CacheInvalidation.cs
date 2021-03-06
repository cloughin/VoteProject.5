using System;

namespace DB.VoteCache
{
  public partial class CacheInvalidationRow
  {
  }

  public partial class CacheInvalidation
  {
    public static int DeleteExpiredTransactions(
      DateTime expiration, int commandTimeout = -1)
    {
      const string cmdText =
        "DELETE FROM CacheInvalidation WHERE CacheTimeStamp<@Expiration";
      var cmd = VoteCacheDb.GetCommand(cmdText, commandTimeout);
      VoteCacheDb.AddCommandParameter(cmd, "Expiration", expiration);
      return VoteCacheDb.ExecuteNonQuery(cmd);
    }

    public static CacheInvalidationTable GetNewTransactions(
      int lastId, int commandTimeout = -1)
    {
      var cmdText = GetSelectCommandText(CacheInvalidationTable.ColumnSet.All) +
        " WHERE IsCommonCacheInvalidated=1" + "  AND Id > @LastProcessedId" +
        " ORDER BY Id";
      var cmd = VoteCacheDb.GetCommand(cmdText, commandTimeout);
      VoteCacheDb.AddCommandParameter(cmd, "LastProcessedId", lastId);
      return FillTable(cmd, CacheInvalidationTable.ColumnSet.All);
    }

    public static CacheInvalidationTable GetUnprocessedData(int commandTimeout = -1)
    {
      var cmdText = GetSelectCommandText(CacheInvalidationTable.ColumnSet.All) +
        " WHERE IsCommonCacheInvalidated=0" + " ORDER BY Id";
      var cmd = VoteCacheDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, CacheInvalidationTable.ColumnSet.All);
    }
  }
}