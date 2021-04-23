using System;
using System.Configuration;
using System.Linq;
using DB.VoteCache;

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
        int days;
        if (!int.TryParse(daysString, out days))
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
      //count += InvalidateBallotAll();
      //count += InvalidateElectedAll();
      //count += InvalidateElectionAll();
      //count += InvalidateIntroAll();
      //count += InvalidateIssueAll();
      //count += InvalidateOfficialsAll();
      //count += InvalidatePoliticianIssueAll();
      //count += InvalidateReferendumAll();
      count += InvalidatePageCacheAll();
      return count;
    }

    //private static int InvalidateBallotAll()
    //{
    //  var count = CacheBallotPages.CountTable(0);
    //  CacheBallotPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateBallotByDomainDesignCodeElectionKey(
    //  string domainDesignCode, string electionKey)
    //{
    //  return CacheBallotPages.DeleteByDomainDesignCodeElectionKey(
    //    domainDesignCode, electionKey);
    //}

    //private static int InvalidateBallotByElectionKey(string electionKey)
    //{
    //  return CacheBallotPages.DeleteByElectionKey(electionKey);
    //}

    //private static int InvalidateElectedAll()
    //{
    //  var count = CacheElectedPages.CountTable(0);
    //  CacheElectedPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateElectedByStateCode(string stateCode)
    //{
    //  return CacheElectedPages.DeleteByStateCode(stateCode);
    //}

    //private static int InvalidateElectionAll()
    //{
    //  var count = CacheElectionPages.CountTable(0);
    //  CacheElectionPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateElectionByElectionKey(string electionKey)
    //{
    //  return CacheElectionPages.DeleteByElectionKey(electionKey);
    //}

    //private static int InvalidateIntroAll()
    //{
    //  var count = CacheIntroPages.CountTable(0);
    //  CacheIntroPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateIntroByPoliticianKey(string politicianKey)
    //{
    //  return CacheIntroPages.DeleteByPoliticianKey(politicianKey);
    //}

    //private static int InvalidateIssueAll()
    //{
    //  var count = CacheIssuePages.CountTable(0);
    //  CacheIssuePages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateIssueByIssueKey(string issueKey)
    //{
    //  return CacheIssuePages.DeleteByIssueKey(issueKey);
    //}

    //private static int InvalidateIssueByOfficeKey(string officeKey)
    //{
    //  return CacheIssuePages.DeleteByOfficeKey(officeKey);
    //}

    //private static int InvalidateIssueByElectionKeyOfficeKey(
    //  string electionKey, string officeKey)
    //{
    //  return CacheIssuePages.DeleteByElectionKeyOfficeKey(electionKey, officeKey);
    //}

    //private static int InvalidateIssueByOfficeKeyIssueKey(
    //  string officeKey, string issueKey)
    //{
    //  return CacheIssuePages.DeleteByOfficeKeyIssueKey(officeKey, issueKey);
    //}

    //private static int InvalidateIssueByElectionKeyOfficeKeyIssueKey(
    //  string electionKey, string officeKey, string issueKey)
    //{
    //  return CacheIssuePages.DeleteByElectionKeyOfficeKeyIssueKey(
    //    electionKey, officeKey, issueKey);
    //}

    //private static int InvalidateOfficialsAll()
    //{
    //  var count = CacheOfficialsPages.CountTable(0);
    //  CacheOfficialsPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateOfficialsByStateCode(string stateCode)
    //{
    //  return CacheOfficialsPages.DeleteByStateCode(stateCode);
    //}

    //private static int InvalidateOfficialsByStateCodeCountyCode(
    //  string stateCode, string countyCode)
    //{
    //  return CacheOfficialsPages.DeleteByStateCodeCountyCode(stateCode, countyCode);
    //}

    //private static int InvalidateOfficialsByStateCodeCountyCodeLocalCode(
    //  string stateCode, string countyCode, string localCode)
    //{
    //  return CacheOfficialsPages.DeleteByStateCodeCountyCodeLocalCode(
    //    stateCode, countyCode, localCode);
    //}

    private static int InvalidatePageCacheAll()
    {
      var count = CachePages.CountTable(0);
      CachePages.TruncateTable();
      return count;
    }

    //private static int InvalidatePoliticianIssueAll()
    //{
    //  var count = CachePoliticianIssuePages.CountTable(0);
    //  CachePoliticianIssuePages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidatePoliticianIssueByIssuekey(string issueKey)
    //{
    //  return CachePoliticianIssuePages.DeleteByIssueKey(issueKey);
    //}

    //private static int InvalidatePoliticianIssueByPoliticianKey(
    //  string politicianKey)
    //{
    //  return CachePoliticianIssuePages.DeleteByPoliticianKey(politicianKey);
    //}

    //private static int InvalidatePoliticianIssueByPoliticianKeyIssueKey(
    //  string politicianKey, string issueKey)
    //{
    //  return CachePoliticianIssuePages.DeleteByPoliticianKeyIssueKey(
    //    politicianKey, issueKey);
    //}

    //private static int InvalidateReferendumAll()
    //{
    //  var count = CacheReferendumPages.CountTable(0);
    //  CacheReferendumPages.TruncateTable();
    //  return count;
    //}

    //private static int InvalidateReferendumByElectionkey(string electionKey)
    //{
    //  return CacheReferendumPages.DeleteByElectionKey(electionKey);
    //}

    private static int ProcessTransaction(CacheInvalidationRow row)
    {
      var count = 0;

      switch (row.TransactionType.ToLowerInvariant())
      {
        case "all":
          count += InvalidateAll();
          break;

        //case "ballotall":
        //  count += InvalidateBallotAll();
        //  break;

        //case "ballotbydomaindesigncodeelectionkey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      count += InvalidateBallotByDomainDesignCodeElectionKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "ballotbyelectionkey":
        //  count += InvalidateBallotByElectionKey(row.CacheKey);
        //  break;

        //case "electedall":
        //  count += InvalidateElectedAll();
        //  break;

        //case "electedbystatecode":
        //  count += InvalidateElectedByStateCode(row.CacheKey);
        //  break;

        //case "electionall":
        //  count += InvalidateElectionAll();
        //  break;

        //case "electionbyelectionkey":
        //  count += InvalidateElectionByElectionKey(row.CacheKey);
        //  break;

        //case "introall":
        //  count += InvalidateIntroAll();
        //  break;

        //case "introbypoliticiankey":
        //  count += InvalidateIntroByPoliticianKey(row.CacheKey);
        //  break;

        //case "issueall":
        //  count += InvalidateIssueAll();
        //  break;

        //case "issuebyissuekey":
        //  count += InvalidateIssueByIssueKey(row.CacheKey);
        //  break;

        //case "issuebyofficekey":
        //  count += InvalidateIssueByOfficeKey(row.CacheKey);
        //  break;

        //case "issuebyelectionkeyofficekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      count += InvalidateIssueByElectionKeyOfficeKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "issuebyofficekeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      count += InvalidateIssueByOfficeKeyIssueKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "issuebyelectionkeyofficekeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 3)
        //      count += InvalidateIssueByElectionKeyOfficeKeyIssueKey(
        //        keys[0], keys[1], keys[2]);
        //  }
        //  break;

        case "nagsall":
          // no action needed on common db apart from 
          // updating IsCommonCacheInvalidated
          break;

        //case "officialsall":
        //  count += InvalidateOfficialsAll();
        //  break;

        //case "officialsbystatecode":
        //  count += InvalidateOfficialsByStateCode(row.CacheKey);
        //  break;

        //case "officialsbystatecodecountycode":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      count += InvalidateOfficialsByStateCodeCountyCode(keys[0], keys[1]);
        //  }
        //  break;

        //case "officialsbystatecodecountycodelocalcode":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 3)
        //      count += InvalidateOfficialsByStateCodeCountyCodeLocalCode(
        //        keys[0], keys[1], keys[2]);
        //  }
        //  break;

        case "politicianimage":
          // no action needed on common db apart from 
          // updating IsCommonCacheInvalidated
          break;

        //case "politicianissueall":
        //  count += InvalidatePoliticianIssueAll();
        //  break;

        //case "politicianissuebyissuekey":
        //  count += InvalidatePoliticianIssueByIssuekey(row.CacheKey);
        //  break;

        //case "politicianissuebypoliticiankey":
        //  count += InvalidatePoliticianIssueByPoliticianKey(row.CacheKey);
        //  break;

        //case "politicianissuebypoliticiankeyissuekey":
        //  {
        //    var keys = row.CacheKey.Split('|');
        //    if (keys.Length == 2)
        //      count += InvalidatePoliticianIssueByPoliticianKeyIssueKey(keys[0], keys[1]);
        //  }
        //  break;

        //case "referendumall":
        //  count += InvalidateReferendumAll();
        //  break;

        //case "referendumbyelectionkey":
        //  count += InvalidateReferendumByElectionkey(row.CacheKey);
        //  break;

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
      ScheduleInvalidation("all", string.Empty);
      //DB.Vote.Master.UpdateCacheRemovedAll(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedBallot(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedElected(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedIntro(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedPoliticianIssue(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedIssue(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedReferendum(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedElectionReport(DateTime.UtcNow);
      //DB.Vote.Master.UpdateCacheRemovedOfficialsReport(DateTime.UtcNow);
      CacheControl.UpdateWhenCleared(DateTime.UtcNow);
    }

    //public static void ScheduleInvalidateBallotAll()
    //{
    //  ScheduleInvalidation("ballotall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedBallot(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateBallotByDomainDesignCodeElectionKey(
    //  string domainDesignCode, string electionKey)
    //{
    //  ScheduleInvalidation(
    //    "ballotbydomaindesigncodeelectionkey", domainDesignCode + "|" + electionKey);
    //}

    //public static void ScheduleInvalidateBallotByElectionKey(string electionKey)
    //{
    //  ScheduleInvalidation("ballotbyelectionkey", electionKey);
    //}

    //public static void ScheduleInvalidateElectedAll()
    //{
    //  ScheduleInvalidation("electedall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedElected(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateElectedByStateCode(string stateCode)
    //{
    //  ScheduleInvalidation("electedbystatecode", stateCode);
    //}

    //public static void ScheduleInvalidateElectionAll()
    //{
    //  ScheduleInvalidation("electionall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedElectionReport(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateElectionByElectionKey(string electionKey)
    //{
    //  ScheduleInvalidation("electionbyelectionkey", electionKey);
    //}

    //public static void ScheduleInvalidateIntroAll()
    //{
    //  ScheduleInvalidation("introall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedIntro(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateIntroByPoliticianKey(string politicianKey)
    //{
    //  ScheduleInvalidation("introbypoliticiankey", politicianKey);
    //}

    //public static void ScheduleInvalidateIssueAll()
    //{
    //  ScheduleInvalidation("issueall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedIssue(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateIssueByIssueKey(string issueKey)
    //{
    //  ScheduleInvalidation("issuebyissuekey", issueKey);
    //}

    //public static void ScheduleInvalidateIssueByOfficeKey(string officeKey)
    //{
    //  ScheduleInvalidation("issuebyofficekey", officeKey);
    //}

    //public static void ScheduleInvalidateIssueByElectionKeyOfficeKey(
    //  string electionKey, string officeKey)
    //{
    //  ScheduleInvalidation(
    //    "issuebyelectionkeyofficekey", electionKey + "|" + officeKey);
    //}

    //public static void ScheduleInvalidateIssueByOfficeKeyIssueKey(
    //  string officeKey, string issueKey)
    //{
    //  ScheduleInvalidation("issuebyofficekeyissuekey", officeKey + "|" + issueKey);
    //}

    //public static void ScheduleInvalidateIssueByElectionKeyOfficeKeyIssueKey(
    //  string electionKey, string officeKey, string issueKey)
    //{
    //  ScheduleInvalidation(
    //    "issuebyelectionkeyofficekeyissuekey",
    //    electionKey + "|" + officeKey + "|" + issueKey);
    //}

    public static void ScheduleInvalidateNagsAll()
    {
      ScheduleInvalidation("nagsall", string.Empty);
    }

    //public static void ScheduleInvalidateOfficialsAll()
    //{
    //  ScheduleInvalidation("officialsall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedOfficialsReport(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateOfficialsByStateCode(string stateCode)
    //{
    //  ScheduleInvalidation("officialsbystatecode", stateCode);
    //}

    //public static void ScheduleInvalidateOfficialsByStateCodeCountyCode(
    //  string stateCode, string countyCode)
    //{
    //  ScheduleInvalidation(
    //    "officialsbystatecodecountycode", stateCode + "|" + countyCode);
    //}

    //public static void ScheduleInvalidateOfficialsByStateCodeCountyCodeLocalCode(
    //  string stateCode, string countyCode, string localCode)
    //{
    //  ScheduleInvalidation(
    //    "officialsbystatecodecountycodelocalcode",
    //    stateCode + "|" + countyCode + "|" + localCode);
    //}

    //public static void ScheduleInvalidateOfficials(string stateCode,
    //  string countyCode = "", string localCode = "")
    //{
    //  if (string.IsNullOrWhiteSpace(localCode))
    //    if (string.IsNullOrWhiteSpace(countyCode))
    //      ScheduleInvalidateOfficialsByStateCode(stateCode);
    //    else
    //      ScheduleInvalidateOfficialsByStateCodeCountyCode(stateCode, countyCode);
    //  else
    //    ScheduleInvalidateOfficialsByStateCodeCountyCodeLocalCode(stateCode,
    //      countyCode, localCode);
    //}

    //public static void ScheduleInvalidatePoliticianIssueAll()
    //{
    //  ScheduleInvalidation("politicianissueall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedPoliticianIssue(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidatePoliticianIssueByIssuekey(string issueKey)
    //{
    //  ScheduleInvalidation("politicianissuebyissuekey", issueKey);
    //}

    //public static void ScheduleInvalidatePoliticianIssueByPoliticianKey(
    //  string politicianKey)
    //{
    //  ScheduleInvalidation("politicianissuebypoliticiankey", politicianKey);
    //}

    //public static void ScheduleInvalidatePoliticianIssueByPoliticianKeyIssueKey(
    //  string politicianKey, string issueKey)
    //{
    //  ScheduleInvalidation(
    //    "politicianissuebypoliticiankeyissuekey", politicianKey + "|" + issueKey);
    //}

    //public static void ScheduleInvalidateReferendumAll()
    //{
    //  ScheduleInvalidation("referendumall", string.Empty);
    //  DB.Vote.Master.UpdateCacheRemovedReferendum(DateTime.UtcNow);
    //}

    //public static void ScheduleInvalidateReferendumByElectionkey(string electionKey)
    //{
    //  ScheduleInvalidation("referendumbyelectionkey", electionKey);
    //}

    #endregion Public low-level scheduling methods

    #region Public high-level scheduling methods

    //public static void ScheduleInvalidateAllCacheForPolitician(
    //  string politicianKey, PageCache pageCache)
    //{
    //  var politicianStateCode = Politicians.GetStateCodeFromKey(politicianKey);
    //  var officeKey = pageCache.Politicians.GetOfficeKey(politicianKey);
    //  var officeClass = pageCache.Offices.GetOfficeClass(officeKey);
    //  var officeStateCode = Offices.GetStateCodeFromKey(officeKey);
    //  var countyCode = pageCache.Offices.GetCountyCode(officeKey);
    //  var localCode = pageCache.Offices.GetLocalCode(officeKey);

    //  // Invalidate the politician's intro and politician issue pages
    //  ScheduleInvalidateIntroAndPoliticianIssuePagesForPolitician(politicianKey);

    //  // Invalidate the issue pages for all politician's offices in all elections
    //  ScheduleInvalidateAllIssuePagesForPolitician(politicianKey);

    //  // Invalidate election report, election pages and ballot pages
    //  ScheduleInvalidateAllElectionCacheForPolitician(politicianKey);

    //  // Invalidate elected officials pages for the politicians state and
    //  // the election state code, if different ("US")
    //  ScheduleInvalidateAllElectedPagesForPoliticianOffice(politicianKey, officeKey);

    //  // Invalidate officials pages and reports
    //  ScheduleInvalidateAllOfficialsCacheForPoliticianOfficeClass(
    //    politicianKey, officeClass);

    //  // Invalidate the politicians reports
    //  //ReportsPoliticians.SetNotCurrent(
    //  //  officeClass, officeStateCode, countyCode, localCode);
    //  //ReportsOffices.SetNotCurrent(
    //  //  officeClass, politicianStateCode, countyCode, localCode);
    //}

    //private static void ScheduleInvalidateAllElectedPagesForPoliticianOffice(
    //  string politicianKey, string officeKey)
    //{
    //  var politicianStateCode = Politicians.GetStateCodeFromKey(politicianKey);
    //  var officeStateCode = Offices.GetStateCodeFromKey(officeKey);
    //  ScheduleInvalidateElectedByStateCode(officeStateCode);
    //  if (politicianStateCode != officeStateCode)
    //    ScheduleInvalidateElectedByStateCode(politicianStateCode);
    //}

    //private static void ScheduleInvalidateAllElectionCacheForPolitician(
    //  string politicianKey)
    //{
    //  // for every election that the politicial has been involved in as a candidate
    //  // invalidate the election reports, the election pages, and the ballot pages
    //  var tableCandidate =
    //    ElectionsPoliticians.GetOfficeKeyDataByPoliticianKey(politicianKey);
    //  foreach (var row in tableCandidate)
    //    ScheduleInvalidateOneElection(row.ElectionKey);

    //  // and again when running mate
    //  var tableRunningMate =
    //    ElectionsPoliticians.GetOfficeKeyDataByRunningMateKey(politicianKey);
    //  foreach (var row in tableRunningMate)
    //    ScheduleInvalidateOneElection(row.ElectionKey);
    //}

    //private static void ScheduleInvalidateAllIssuePagesForPolitician(
    //  string politicianKey)
    //{
    //  var officeKeys = ElectionsPoliticians.GetOfficeKeyData(politicianKey);
    //  foreach (var officeKeyData in officeKeys)
    //    ScheduleInvalidateIssueByElectionKeyOfficeKey(
    //      officeKeyData.ElectionKey, officeKeyData.OfficeKey);
    //}

    //private static void ScheduleInvalidateAllOfficialsCacheForPoliticianOfficeClass(
    //  string politicianKey, OfficeClass officeClass)
    //{
    //  var stateCode = Politicians.GetStateCodeFromKey(politicianKey);
    //  ScheduleInvalidateAllOfficialsCacheForState(stateCode);

    //  if (!officeClass.IsFederal()) return;

    //  var stateCodeProxy = officeClass.GetStateCodeProxy();
    //  if (!string.IsNullOrWhiteSpace(stateCodeProxy))
    //    ScheduleInvalidateAllOfficialsCacheForState(stateCodeProxy);
    //}

    //private static void ScheduleInvalidateAllOfficialsCacheForState(string stateCode)
    //{
    //  //ReportsOfficials.SetNotCurrent(stateCode);
    //  ScheduleInvalidateOfficialsByStateCode(stateCode);
    //}

    //private static void ScheduleInvalidateIntroAndPoliticianIssuePagesForPolitician(
    //  string politicianKey)
    //{
    //  ScheduleInvalidateIntroByPoliticianKey(politicianKey);
    //  ScheduleInvalidatePoliticianIssueByPoliticianKey(politicianKey);
    //}

    //private static void ScheduleInvalidateOneElection(string electionKey)
    //{
    //  //ReportsElections.UpdateIsReportCurrent(false, electionKey);
    //  //ReportsElections.UpdateReportLastUpdated(DateTime.UtcNow, electionKey);
    //  ScheduleInvalidateElectionByElectionKey(electionKey);
    //  ScheduleInvalidateBallotByElectionKey(electionKey);
    //}

    #endregion Public high-level scheduling methods
  }
}