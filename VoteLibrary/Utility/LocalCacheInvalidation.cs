using CacheInvalidationGlobal = DB.VoteCache.CacheInvalidation;
using CacheInvalidationRowGlobal = DB.VoteCache.CacheInvalidationRow;
using DonationNagsGlobal = DB.VoteCache.DonationNags;
using DonationNagsControlGlobal = DB.VoteCache.DonationNagsControl;

using PoliticiansImagesBlobsGlobal = DB.Vote.PoliticiansImagesBlobs;
using PoliticiansImagesDataGlobal = DB.Vote.PoliticiansImagesData;

using LastProcessedIdLocal = DB.VoteCacheLocal.LastProcessedId;
using DonationNagsLocal = DB.VoteCacheLocal.DonationNags;
using DonationNagsControlLocal = DB.VoteCacheLocal.DonationNagsControl;
using CachePagesLocal = DB.VoteCacheLocal.CachePages;

using PoliticiansImagesBlobsLocal = DB.VoteImagesLocal.PoliticiansImagesBlobs;
using PoliticiansImagesDataLocal = DB.VoteImagesLocal.PoliticiansImagesData;

namespace Vote
{
  public static class LocalCacheInvalidation
  {
    private static void Debug(string message)
    {
      DB.Vote.DebugLog.Insert("LocalCacheInvalidation", message);
    }

    private static int GetLastProcessedId()
    {
      var id = LastProcessedIdLocal.GetId();
      if (id == null) // first time, initialize with -1
      {
        LastProcessedIdLocal.Insert(id: -1);
        id = -1;
      }
      return id.Value;
    }

    private static void InvalidateAll()
    {
      InvalidatePageCacheAll();
    }

    private static void InvalidateNagsAll(bool debugging = false)
    {
      // copy the DonationNagsControl singleton from common to local
      if (debugging)
      {
        Debug($"DonationNagsControlGlobal.GetIsNaggingEnabled(true) = {DonationNagsControlGlobal.GetIsNaggingEnabled(true)}");
        Debug($"DonationNagsControlLocal.GetIsNaggingEnabled(true) = {DonationNagsControlLocal.GetIsNaggingEnabled(true)}");
      }
      DonationNagsControlLocal.UpdateIsNaggingEnabled(
        DonationNagsControlGlobal.GetIsNaggingEnabled(true));
      Debug($"DonationNagsControlLocal.GetIsNaggingEnabled(true) = {DonationNagsControlLocal.GetIsNaggingEnabled(true)}");

      // copy the DonationNags table from common to local
      var table = DonationNagsGlobal.GetAllData();
      DonationNagsLocal.TruncateTable();
      foreach (var row in table)
        DonationNagsLocal.Insert(row.MessageNumber, row.MessageHeading,
          row.MessageText, row.NextMessageNumber);
    }

    private static void InvalidatePageCacheAll()
    {
      var count = CachePagesLocal.CountTable(0);
      VotePage.LogInfo("LocalCacheInvalidation", "Begin InvalidatePageCacheAll:" +
        count);
      CachePagesLocal.TruncateTable();
      VotePage.LogInfo("LocalCacheInvalidation", "End InvalidatePageCacheAll:" +
        count);
    }

    private static void InvalidatePoliticianImage(string politicianKey)
    {
      // We do this via updates to avoid synchronization problems
      var oldBlobs = PoliticiansImagesBlobsLocal.GetDataByPoliticianKey(politicianKey);
      if (oldBlobs.Count == 1)
      {
        var newBlobs =
          PoliticiansImagesBlobsGlobal.GetCacheDataByPoliticianKey(politicianKey);
        switch (newBlobs.Count)
        {
          case 1:
          {
            var oldRow = oldBlobs[0];
            var newRow = newBlobs[0];
            oldRow.Profile300 = newRow.Profile300;
            oldRow.Profile200 = newRow.Profile200;
            oldRow.Headshot100 = newRow.Headshot100;
            oldRow.Headshot75 = newRow.Headshot75;
            oldRow.Headshot50 = newRow.Headshot50;
            oldRow.Headshot35 = newRow.Headshot35;
            oldRow.Headshot25 = newRow.Headshot25;
            oldRow.Headshot20 = newRow.Headshot20;
            oldRow.Headshot15 = newRow.Headshot15;
            PoliticiansImagesBlobsLocal.UpdateTable(oldBlobs);
          }
            break;

          case 0:
            PoliticiansImagesBlobsLocal.DeleteByPoliticianKey(politicianKey);
            break;
        }
      }
      var oldData = PoliticiansImagesDataLocal.GetDataByPoliticianKey(politicianKey);
      if (oldData.Count != 1) return;
      var newData =
        PoliticiansImagesDataGlobal.GetDataByPoliticianKey(politicianKey);
      switch (newData.Count)
      {
        case 1:
        {
          var oldRow = oldData[0];
          var newRow = newData[0];
          oldRow.ProfileOriginalDate = newRow.ProfileOriginalDate;
          oldRow.HeadshotDate = newRow.HeadshotDate;
          oldRow.HeadshotResizeDate = newRow.HeadshotResizeDate;
          PoliticiansImagesDataLocal.UpdateTable(oldData);
        }
          break;

        case 0:
          PoliticiansImagesDataLocal.DeleteByPoliticianKey(politicianKey);
          break;
      }
    }

    public static void ProcessPendingTransactions(bool debugging = false)
    {
      if (debugging) Debug("ProcessPendingTransactions");
      var lastId = GetLastProcessedId();
      if (debugging) Debug($"GetLastProcessedId = {lastId}");
      var table = CacheInvalidationGlobal.GetNewTransactions(lastId);
      if (debugging) Debug($"table.Count = {table.Count}");
      foreach (var row in table)
      {
        ProcessTransaction(row, debugging);
        lastId = row.Id;
      }
      LastProcessedIdLocal.UpdateId(lastId);
    }

    private static void ProcessTransaction(CacheInvalidationRowGlobal row, bool debugging = false)
    {
      if (debugging) Debug($"row.TransactionType.ToLowerInvariant() = {row.TransactionType.ToLowerInvariant()}");
      switch (row.TransactionType.ToLowerInvariant())
      {
        case "all":
          VotePage.LogInfo("LocalCacheInvalidation", "Begin InvalidateAll");
          InvalidateAll();
          break;

        case "nagsall":
          InvalidateNagsAll(debugging);
          break;

        case "politicianimage":
          InvalidatePoliticianImage(row.CacheKey);
          break;

        default:
          throw new VoteException(
            "Unidentified invalidation transaction type" + row.TransactionType);
      }
    }
  }
}