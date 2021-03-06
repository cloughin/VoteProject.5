using System;
using System.Collections.Generic;
using DB.Vote;
using DB.VoteCache;
using DonationNags = DB.VoteCacheLocal.DonationNags;
using DonationNagsControl = DB.VoteCacheLocal.DonationNagsControl;
using DonationNagsTable = DB.VoteCacheLocal.DonationNagsTable;
using PoliticiansImagesData = DB.VoteImagesLocal.PoliticiansImagesData;
using PoliticiansImagesDataTable = DB.VoteImagesLocal.PoliticiansImagesDataTable;

namespace Vote
{
  public class ImageModDates
  {
    public DateTime ProfileDate;
    public DateTime HeadshotDate;
    public DateTime RefreshDate;
  }

  public static class MemCache
  {
    #region Private

    private static readonly KeyedCacheEntry<string> CanonicalElectionKeyCache =
      new KeyedCacheEntry<string>();

    //private static readonly KeyedCacheEntry<string>
    //  DomainInstructionsPoliticianIssuePageIssueListAnswersCache =
    //    new KeyedCacheEntry<string>();

    private static readonly CacheEntry<int> CacheExpirationCache =
      new CacheEntry<int>();

    private static readonly CacheEntry<int> CacheExpirationFuzzFactorCache =
      new CacheEntry<int>();

    private static readonly KeyedCacheEntry<ImageModDates> ImageModDatesCache =
      new KeyedCacheEntry<ImageModDates>(new TimeSpan(0, CacheExpiration, 0));

    private static readonly CacheEntry<bool> IsCachingPagesCache =
      new CacheEntry<bool>();

    private static readonly CacheEntry<bool> IsLoggingErrorsCache =
      new CacheEntry<bool>();

    private static readonly CacheEntry<bool> IsNaggingEnabledCache =
      new CacheEntry<bool>();

    private static readonly CacheValidationDictionary<string> IsValidElectionCache =
      new CacheValidationDictionary<string>();

    private static readonly CacheValidationDictionary<string> IsValidOfficeCache =
      new CacheValidationDictionary<string>();

    //private static readonly CacheEntry<string>
    //  MasterInstructionsPoliticianIssuePageIssueListAnswersCache =
    //    new CacheEntry<string>();

    //private static readonly CacheEntry<bool>
    //  MasterIsTextInstructionsPoliticianIssuePageIssueListAnswersCache =
    //    new CacheEntry<bool>();

    private static readonly CacheEntry<DonationNagsTable> DonationNagsCache =
      new CacheEntry<DonationNagsTable>();

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static string GetCanonicalElectionKey(string electionKey)
    {
      string result;
      lock (CanonicalElectionKeyCache)
        if (!CanonicalElectionKeyCache.TryGetValue(electionKey, out result))
          result = CanonicalElectionKeyCache.Refresh(
            electionKey, Elections.GetElectionKeyCanonical(electionKey, string.Empty));
      return result;
    }

    //public static string GetDomainInstructionsPoliticianIssuePageIssueListAnswers(
    //  string domainDesignCode)
    //{
    //  string result;
    //  lock (DomainInstructionsPoliticianIssuePageIssueListAnswersCache)
    //    if (
    //      !DomainInstructionsPoliticianIssuePageIssueListAnswersCache.TryGetValue(
    //        domainDesignCode, out result))
    //      result =
    //        DomainInstructionsPoliticianIssuePageIssueListAnswersCache.Refresh(
    //          domainDesignCode,
    //          DomainDesigns.GetInstructionsPoliticianIssuePageIssueListAnswers(
    //            domainDesignCode, string.Empty));
    //  return result;
    //}

    public static ImageModDates GetImageModDates(string politicianKey)
    {
      ImageModDates result;
      lock (ImageModDatesCache)
        if (!ImageModDatesCache.TryGetValue(politicianKey, out result))
        {
          var table = PoliticiansImagesData.GetData(politicianKey);
          if (table.Count == 0)
          {
            ImageManager.CopyCommonDataToLocal(politicianKey);
            table = PoliticiansImagesData.GetData(politicianKey);
          }
          result = RefreshImageModDates(table);
        }
      // check for cache expiration
      if (result.RefreshDate + new TimeSpan(0, CacheExpiration, 0) < DateTime.UtcNow)
        lock (ImageModDatesCache)
        {
          // expired
          ImageManager.CopyCommonDataToLocal(politicianKey);
          var table = PoliticiansImagesData.GetData(politicianKey);
          result = RefreshImageModDates(table);
        }
      return result ?? new ImageModDates();
    }

    private static ImageModDates RefreshImageModDates(PoliticiansImagesDataTable table)
    {
      ImageModDates result = null;
      if (table.Count == 1)
      {
        var row = table[0];
        ImageModDatesCache.MaxAge = new TimeSpan(0, CacheExpiration, 0);
        result = ImageModDatesCache.Refresh(row.PoliticianKey, new ImageModDates
        {
          ProfileDate = row.ProfileOriginalDate.AsUtc(),
          HeadshotDate = row.HeadshotDate.AsUtc(),
          RefreshDate = row.RefreshDate.AsUtc()
        });
      }
      return result;
    }

    public static int CacheExpiration
    {
      get
      {
        int result;
        lock (CacheExpirationCache)
          if (!CacheExpirationCache.TryGetValue(out result))
            result =
              CacheExpirationCache.Refresh(CacheControl.GetExpiration(0));
        return result;
      }
    }

    public static int CacheFuzzFactor
    {
      get
      {
        int result;
        lock (CacheExpirationFuzzFactorCache)
          if (!CacheExpirationFuzzFactorCache.TryGetValue(out result))
            result =
              CacheExpirationFuzzFactorCache.Refresh(CacheControl.GetFuzzFactor(0));
        return result;
      }
    }

    public static bool IsCachingPages
    {
      get
      {
        bool result;
        lock (IsCachingPagesCache)
          if (!IsCachingPagesCache.TryGetValue(out result))
            result =
              IsCachingPagesCache.Refresh(DB.Vote.Master.GetIsCachePages(false));
        return result;
      }
    }

    public static bool IsLoggingErrors
    {
      get
      {
        bool result;
        lock (IsLoggingErrorsCache)
          if (!IsLoggingErrorsCache.TryGetValue(out result))
            result =
              IsLoggingErrorsCache.Refresh(
                DB.Vote.Master.GetIsLog301And404Errors(false));
        return result;
      }
    }

    public static bool IsNaggingEnabled
    {
      get
      {
        bool result;
        lock (IsNaggingEnabledCache)
          if (!IsNaggingEnabledCache.TryGetValue(out result))
            result =
              IsNaggingEnabledCache.Refresh(
                DonationNagsControl.GetIsNaggingEnabled(true));
        return result;
      }
    }

    public static bool IsValidElection(string electionKey)
    {
      bool result;
      lock (IsValidElectionCache)
        if (!IsValidElectionCache.TryGetValue(electionKey, out result))
        {
          var exists = Elections.ElectionKeyExists(electionKey);
          // for county elections, we consider it to exist of there are any local elections
          if (!exists && Elections.IsCountyElection(electionKey))
            exists = Elections.LocalElectionsExistForCountyElection(electionKey);
          result = IsValidElectionCache.Refresh(electionKey, exists);
        }
      return result;
    }

    // prefer Offices.IsValid()
    public static bool IsValidOffice(string officeKey)
    {
      if (string.IsNullOrEmpty(officeKey)) return false;
      bool result;
      lock (IsValidOfficeCache)
        if (!IsValidOfficeCache.TryGetValue(officeKey, out result))
          result = IsValidOfficeCache.Refresh(
            officeKey, Offices.OfficeKeyExists(officeKey));
      return result;
    }

    //public static string MasterInstructionsPoliticianIssuePageIssueListAnswers
    //{
    //  get
    //  {
    //    string result;
    //    lock (MasterInstructionsPoliticianIssuePageIssueListAnswersCache)
    //      if (
    //        !MasterInstructionsPoliticianIssuePageIssueListAnswersCache.TryGetValue(
    //          out result))
    //        result =
    //          MasterInstructionsPoliticianIssuePageIssueListAnswersCache.Refresh(
    //            MasterDesign.GetInstructionsPoliticianIssuePageIssueListAnswers(
    //              string.Empty));
    //    return result;
    //  }
    //}

    //public static bool MasterIsTextInstructionsPoliticianIssuePageIssueListAnswers
    //{
    //  get
    //  {
    //    bool result;
    //    lock (MasterIsTextInstructionsPoliticianIssuePageIssueListAnswersCache)
    //      if (
    //        !MasterIsTextInstructionsPoliticianIssuePageIssueListAnswersCache
    //          .TryGetValue(out result))
    //        result =
    //          MasterIsTextInstructionsPoliticianIssuePageIssueListAnswersCache
    //            .Refresh(
    //              MasterDesign
    //                .GetIsTextInstructionsPoliticianIssuePageIssueListAnswers(false));
    //    return result;
    //  }
    //}

    public static DonationNagsTable CachedDonationNagsTable
    {
      get
      {
        DonationNagsTable result;
        lock (DonationNagsCache)
          if (!DonationNagsCache.TryGetValue(out result))
            result = DonationNagsCache.Refresh(DonationNags.GetAllData());
        return result;
      }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  public class CacheBase
  {
    protected static readonly TimeSpan DefaultMaxAge = new TimeSpan(0, 5, 0);
    // 5 minutes
  }

  // For a single-valued cache entry
  public class CacheEntry<T> : CacheBase
  {
    #region Private

    private DateTime _Expiration = DateTime.MinValue;
    private readonly TimeSpan _MaxAge = DefaultMaxAge;
    private T _Value;

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public CacheEntry()
    {
    }

    public CacheEntry(TimeSpan maxAge)
    {
      _MaxAge = maxAge;
    }

    public T Refresh(T value)
    {
      _Value = value;
      _Expiration = DateTime.UtcNow + _MaxAge;
      return value;
    }

    public bool TryGetValue(out T value)
    {
      if (DateTime.UtcNow >= _Expiration)
      {
        value = default(T);
        return false;
      }

      value = _Value;
      return true;
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  // For a keyed cache entry
  public class KeyedCacheEntry<T> : CacheBase
  {
    #region Private

    private readonly Dictionary<string, Item> _Dictionary =
      new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);

    private class Item
    {
      public T Value;
// ReSharper disable UnusedAutoPropertyAccessor.Local
      public DateTime Expiration { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public KeyedCacheEntry()
    {
    }

    public KeyedCacheEntry(TimeSpan maxAge)
    {
      MaxAge = maxAge;
    }

    public TimeSpan MaxAge { get; set; } = DefaultMaxAge;

    public T Refresh(string key, T value)
    {
      _Dictionary[key] = new Item
      {
        Value = value,
        Expiration = DateTime.UtcNow + MaxAge
      };
      return value;
    }

    public bool TryGetValue(string key, out T value)
    {
      Item item;
      if (!_Dictionary.TryGetValue(key, out item) ||
        (item.Expiration <= DateTime.UtcNow))
      {
        value = default(T);
        return false;
      }

      value = item.Value;
      return true;
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  // For validation, implements both positive and negative caching
  public class CacheValidationDictionary<T> : CacheBase
  {
    #region Private

    private DateTime _Expiration = DateTime.MinValue;
    private readonly TimeSpan _MaxAge = DefaultMaxAge;
    private readonly Dictionary<T, bool> _Dictionary = new Dictionary<T, bool>();

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public CacheValidationDictionary()
    {
    }

    public CacheValidationDictionary(TimeSpan maxAge)
    {
      _MaxAge = maxAge;
    }

    public bool Refresh(T key, bool value)
    {
      _Dictionary[key] = value;
      return value;
    }

    public bool TryGetValue(T key, out bool value)
    {
      if (DateTime.UtcNow < _Expiration)
        return _Dictionary.TryGetValue(key, out value);

      _Dictionary.Clear();
      _Expiration = DateTime.UtcNow + _MaxAge;
      value = default(bool);
      return false;
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}