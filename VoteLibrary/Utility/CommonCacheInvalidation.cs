using System;
using System.Configuration;
using System.Linq;
using DB.VoteCache;
using static System.String;

namespace Vote
{
  public static class CommonCacheInvalidation
  {
    #region CleanUpCacheInvalidation

    // This method is only used by the CommonCacheCleanup which is run via the
    // Windows task scheduler
    public static void CleanUpCacheInvalidation()
    {
      string message;

      try
      {
        VotePage.LogInfo("CleanUpCacheInvalidation", "Started");

        // Get the number of days to retain, default to 7
        var daysString =
          ConfigurationManager.AppSettings["VoteCommonCacheInvalidationRetentionDays"];
        if (!int.TryParse(daysString, out var days))
          days = 7;

        // Convert to a past DateTime
        var expiration = DateTime.UtcNow - new TimeSpan(days, 0, 0, 0);

        // Do it
        var deleted = CacheInvalidation.DeleteExpiredTransactions(expiration);

        message =
          $"{deleted} CacheInvalidation rows deleted";
      }
      catch (Exception ex)
      {
        VotePage.LogException("CleanUpCacheInvalidation", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("CleanUpCacheInvalidation", message);
    }

    #endregion CleanUpCacheInvalidation

    #region ProcessPendingTransactions

    // This method is only used by the CommonCacheInvalidation which is run via the
    // Windows task scheduler
    public static void ProcessPendingTransactions()
    {
      string message;

      try
      {
        VotePage.LogInfo("CommonCacheInvalidation", "Started");

        var table = CacheInvalidation.GetUnprocessedData();
        var count = table.Sum(ProcessTransaction);

        message =
          $"{table.Count()} CacheInvalidation rows processed, {count} Page rows deleted";
      }
      catch (Exception ex)
      {
        VotePage.LogException("CommonCacheInvalidation", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }
      VotePage.LogInfo("CommonCacheInvalidation", message);
    }

    #region ProcessPendingTransactions private helpers

    private static int InvalidateAll()
    {
      var count = 0;
      count += InvalidatePageCacheAll();
      return count;
    }

    private static int InvalidatePageCacheAll()
    {
      var count = CachePages.CountTable(0);
      CachePages.TruncateTable();
      return count;
    }

    private static int ProcessTransaction(CacheInvalidationRow row)
    {
      var count = 0;

      switch (row.TransactionType.ToLowerInvariant())
      {
        case "all":
          count += InvalidateAll();
          break;

        case "nagsall":
          // no action needed on common db apart from 
          // updating IsCommonCacheInvalidated
          break;

        case "politicianimage":
          // no action needed on common db apart from 
          // updating IsCommonCacheInvalidated
          break;

        default:
          throw new VoteException(
            "Unidentified invalidation transaction type" + row.TransactionType);
      }
      CacheInvalidation.UpdateIsCommonCacheInvalidatedById(true, row.Id);
      return count;
    }

    #endregion ProcessPendingTransactions private helpers

    #endregion ProcessPendingTransactions

    #region Public low-level scheduling methods

    public static void ScheduleInvalidation(string transactionType, string key)
    {
      CacheInvalidation.Insert(
        transactionType: transactionType, cacheKey: key,
        cacheTimeStamp: DateTime.UtcNow, isCommonCacheInvalidated: false);
    }

    public static void ScheduleInvalidateAll()
    {
      ScheduleInvalidation("all", Empty);
      CacheControl.UpdateWhenCleared(DateTime.UtcNow);
    }

    public static void ScheduleInvalidateNagsAll()
    {
      ScheduleInvalidation("nagsall", Empty);
    }

    #endregion Public low-level scheduling methods
  }
}