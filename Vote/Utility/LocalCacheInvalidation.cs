using DB.VoteCacheLocal;
using DB.VoteImagesLocal;
using CacheInvalidation = DB.VoteCache.CacheInvalidation;
using CacheInvalidationRow = DB.VoteCache.CacheInvalidationRow;
using DonationNags = DB.VoteCache.DonationNags;

namespace Vote
{
  public static class LocalCacheInvalidation
  {
    private static int GetLastProcessedId()
    {
      var id = LastProcessedId.GetId();
      if (id == null) // first time, initialize with -1
      {
        LastProcessedId.Insert(id: -1);
        id = -1;
      }
      return id.Value;
    }

    private static void InvalidateAll()
    {
      //InvalidateBallotAll();
      //InvalidateElectedAll();
      //InvalidateElectionAll();
      //InvalidateIntroAll();
      //InvalidateIssueAll();
      //InvalidateOfficialsAll();
      //InvalidatePoliticianIssueAll();
      //InvalidateReferendumAll();
      InvalidatePageCacheAll();
    }

    //private static void InvalidateBallotAll()
    //{
    //  CacheBallotPages.TruncateTable();
    //}

    //private static void InvalidateBallotByDomainDesignCodeElectionKey(
    //  string domainDesignCode, string electionKey)
    //{
    //  CacheBallotPages.DeleteByDomainDesignCodeElectionKey(
    //    domainDesignCode, electionKey);
    //}

    //private static void InvalidateBallotByElectionKey(string electionKey)
    //{
    //  CacheBallotPages.DeleteByElectionKey(electionKey);
    //}

    //private static void InvalidateElectedAll()
    //{
    //  CacheElectedPages.TruncateTable();
    //}

    //private static void InvalidateElectedByStateCode(string stateCode)
    //{
    //  CacheElectedPages.DeleteByStateCode(stateCode);
    //}

    //private static void InvalidateElectionAll()
    //{
    //  CacheElectionPages.TruncateTable();
    //}

    //private static void InvalidateElectionByElectionKey(string electionKey)
    //{
    //  CacheElectionPages.DeleteByElectionKey(electionKey);
    //}

    //private static void InvalidateIntroAll()
    //{
    //  CacheIntroPages.TruncateTable();
    //}

    //private static void InvalidateIntroByPoliticianKey(string politicianKey)
    //{
    //  CacheIntroPages.DeleteByPoliticianKey(politicianKey);
    //}

    //private static void InvalidateIssueAll()
    //{
    //  CacheIssuePages.TruncateTable();
    //}

    //private static void InvalidateIssueByIssueKey(string issueKey)
    //{
    //  CacheIssuePages.DeleteByIssueKey(issueKey);
    //}

    //private static void InvalidateIssueByOfficeKey(string officeKey)
    //{
    //  CacheIssuePages.DeleteByOfficeKey(officeKey);
    //}

    //private static void InvalidateIssueByElectionKeyOfficeKey(
    //  string electionKey, string officeKey)
    //{
    //  CacheIssuePages.DeleteByElectionKeyOfficeKey(electionKey, officeKey);
    //}

    //private static void InvalidateIssueByOfficeKeyIssueKey(
    //  string officeKey, string issueKey)
    //{
    //  CacheIssuePages.DeleteByOfficeKeyIssueKey(officeKey, issueKey);
    //}

    //private static void InvalidateIssueByElectionKeyOfficeKeyIssueKey(
    //  string electionKey, string officeKey, string issueKey)
    //{
    //  CacheIssuePages.DeleteByElectionKeyOfficeKeyIssueKey(
    //    electionKey, officeKey, issueKey);
    //}

    //private static void InvalidateOfficialsAll()
    //{
    //  CacheOfficialsPages.TruncateTable();
    //}

    //private static void InvalidateOfficialsByStateCode(string stateCode)
    //{
    //  CacheOfficialsPages.DeleteByStateCode(stateCode);
    //}

    //private static void InvalidateOfficialsByStateCodeCountyCode(
    //  string stateCode, string countyCode)
    //{
    //  CacheOfficialsPages.DeleteByStateCodeCountyCode(stateCode, countyCode);
    //}

    //private static void InvalidateOfficialsByStateCodeCountyCodeLocalCode(
    //  string stateCode, string countyCode, string localCode)
    //{
    //  CacheOfficialsPages.DeleteByStateCodeCountyCodeLocalCode(
    //    stateCode, countyCode, localCode);
    //}

    private static void InvalidateNagsAll()
    {
      // copy the DonationNagsControl singleton from common to local
      DonationNagsControl.UpdateIsNaggingEnabled(
        DB.VoteCache.DonationNagsControl.GetIsNaggingEnabled(true));

      // copy the DonationNags table from common to local
      var table = DonationNags.GetAllData();
      DB.VoteCacheLocal.DonationNags.TruncateTable();
      foreach (var row in table)
        DB.VoteCacheLocal.DonationNags.Insert(
          messageNumber: row.MessageNumber, messageHeading: row.MessageHeading,
          messageText: row.MessageText, nextMessageNumber: row.NextMessageNumber);
    }

    private static void InvalidatePageCacheAll()
    {
      var count = CachePages.CountTable(0);
      VotePage.LogInfo("LocalCacheInvalidation", "Begin InvalidatePageCacheAll:" +
        count);
      CachePages.TruncateTable();
      VotePage.LogInfo("LocalCacheInvalidation", "End InvalidatePageCacheAll:" +
        count);
    }

    private static void InvalidatePoliticianImage(string politicianKey)
    {
      // We do this via updates to avoid synchronization problems
      var oldBlobs = PoliticiansImagesBlobs.GetDataByPoliticianKey(politicianKey);
      if (oldBlobs.Count == 1)
      {
        var newBlobs =
          DB.Vote.PoliticiansImagesBlobs.GetCacheDataByPoliticianKey(politicianKey);
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
            PoliticiansImagesBlobs.UpdateTable(oldBlobs);
          }
            break;

          case 0:
            PoliticiansImagesBlobs.DeleteByPoliticianKey(politicianKey);
            break;
        }
      }
      var oldData = PoliticiansImagesData.GetDataByPoliticianKey(politicianKey);
      if (oldData.Count != 1) return;
      var newData =
        DB.Vote.PoliticiansImagesData.GetDataByPoliticianKey(politicianKey);
      switch (newData.Count)
      {
        case 1:
        {
          var oldRow = oldData[0];
          var newRow = newData[0];
          //oldRow.ProfileOriginalContentType = newRow.ProfileOriginalContentType;
          oldRow.ProfileOriginalDate = newRow.ProfileOriginalDate;
          //oldRow.ProfileOriginalWidth = newRow.ProfileOriginalWidth;
          //oldRow.ProfileOriginalHeight = newRow.ProfileOriginalHeight;
          //oldRow.HeadshotContentType = newRow.HeadshotContentType;
          oldRow.HeadshotDate = newRow.HeadshotDate;
          //oldRow.HeadshotWidth = newRow.HeadshotWidth;
          //oldRow.HeadshotHeight = newRow.HeadshotHeight;
          oldRow.HeadshotResizeDate = newRow.HeadshotResizeDate;
          PoliticiansImagesData.UpdateTable(oldData);
        }
          break;

        case 0:
          PoliticiansImagesData.DeleteByPoliticianKey(politicianKey);
          break;
      }
    }

    //private static void InvalidatePoliticianIssueAll()
    //{
    //  CachePoliticianIssuePages.TruncateTable();
    //}

    //private static void InvalidatePoliticianIssueByIssuekey(string issueKey)
    //{
    //  CachePoliticianIssuePages.DeleteByIssueKey(issueKey);
    //}

    //private static void InvalidatePoliticianIssueByPoliticianKey(
    //  string politicianKey)
    //{
    //  CachePoliticianIssuePages.DeleteByPoliticianKey(politicianKey);
    //}

    //private static void InvalidatePoliticianIssueByPoliticianKeyIssueKey(
    //  string politicianKey, string issueKey)
    //{
    //  CachePoliticianIssuePages.DeleteByPoliticianKeyIssueKey(
    //    politicianKey, issueKey);
    //}

    //private static void InvalidateReferendumAll()
    //{
    //  CacheReferendumPages.TruncateTable();
    //}

    //private static void InvalidateReferendumByElectionkey(string electionKey)
    //{
    //  CacheReferendumPages.DeleteByElectionKey(electionKey);
    //}

    public static void ProcessPendingTransactions()
    {
      var lastId = GetLastProcessedId();
      var table = CacheInvalidation.GetNewTransactions(lastId);
      foreach (var row in table)
      {
        ProcessTransaction(row);
        lastId = row.Id;
      }
      LastProcessedId.UpdateId(lastId);
    }

    private static void ProcessTransaction(CacheInvalidationRow row)
    {
      switch (row.TransactionType.ToLowerInvariant())
      {
        case "all":
          VotePage.LogInfo("LocalCacheInvalidation", "Begin InvalidateAll");
          InvalidateAll();
          break;

        //case "ballotall":
        //  InvalidateBallotAll();
        //  break;

        //case "ballotbydomaindesigncodeelectionkey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      InvalidateBallotByDomainDesignCodeElectionKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "ballotbyelectionkey":
        //  InvalidateBallotByElectionKey(row.CacheKey);
        //  break;

        //case "electedall":
        //  InvalidateElectedAll();
        //  break;

        //case "electedbystatecode":
        //  InvalidateElectedByStateCode(row.CacheKey);
        //  break;

        //case "electionall":
        //  InvalidateElectionAll();
        //  break;

        //case "electionbyelectionkey":
        //  InvalidateElectionByElectionKey(row.CacheKey);
        //  break;

        //case "introall":
        //  InvalidateIntroAll();
        //  break;

        //case "introbypoliticiankey":
        //  InvalidateIntroByPoliticianKey(row.CacheKey);
        //  break;

        //case "issueall":
        //  InvalidateIssueAll();
        //  break;

        //case "issuebyissuekey":
        //  InvalidateIssueByIssueKey(row.CacheKey);
        //  break;

        //case "issuebyofficekey":
        //  InvalidateIssueByOfficeKey(row.CacheKey);
        //  break;

        //case "issuebyelectionkeyofficekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      InvalidateIssueByElectionKeyOfficeKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "issuebyofficekeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      InvalidateIssueByOfficeKeyIssueKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "issuebyelectionkeyofficekeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 3)
        //      InvalidateIssueByElectionKeyOfficeKeyIssueKey(
        //        keys[0], keys[1], keys[2]);
        //  }
        //  break;

        case "nagsall":
          InvalidateNagsAll();
          break;

        //case "officialsall":
        //  InvalidateOfficialsAll();
        //  break;

        //case "officialsbystatecode":
        //  InvalidateOfficialsByStateCode(row.CacheKey);
        //  break;

        //case "officialsbystatecodecountycode":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      InvalidateOfficialsByStateCodeCountyCode(keys[0], keys[1]);
        //  }
        //  break;

        //case "officialsbystatecodecountycodelocalcode":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 3)
        //      InvalidateOfficialsByStateCodeCountyCodeLocalCode(
        //        keys[0], keys[1], keys[2]);
        //  }
        //  break;

        case "politicianimage":
          InvalidatePoliticianImage(row.CacheKey);
          break;

        //case "politicianissueall":
        //  InvalidatePoliticianIssueAll();
        //  break;

        //case "politicianissuebyissuekey":
        //  InvalidatePoliticianIssueByIssuekey(row.CacheKey);
        //  break;

        //case "politicianissuebypoliticiankey":
        //  InvalidatePoliticianIssueByPoliticianKey(row.CacheKey);
        //  break;

        //case "politicianissuebypoliticiankeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      InvalidatePoliticianIssueByPoliticianKeyIssueKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "referendumall":
        //  InvalidateReferendumAll();
        //  break;

        //case "referendumbyelectionkey":
        //  InvalidateReferendumByElectionkey(row.CacheKey);
        //  break;

        default:
          throw new VoteException(
            "Unidentified invalidation transaction type" + row.TransactionType);
      }
    }
  }
}