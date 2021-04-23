using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using DB.VoteCacheLocal;

namespace Vote
{
  public abstract class CacheablePage : PublicPage
  {
    #region Private

    private class Statistics
    {
      public DateTime SnapshotTime;
      public int PagesServedFromMemoryCache;
      public int PagesServedFromLocalCache;
      public int PagesServedFromCommonCache;
      public int PagesCreated;

      public Statistics Clone()
      {
        return new Statistics
        {
          SnapshotTime = SnapshotTime,
          PagesServedFromMemoryCache = PagesServedFromMemoryCache,
          PagesServedFromLocalCache = PagesServedFromLocalCache,
          PagesServedFromCommonCache = PagesServedFromCommonCache,
          PagesCreated = PagesCreated
        };
      }
    }

    private static readonly TimeSpan StatisticsSnapshotInterval = new TimeSpan(1, 0,
      0); // 1 hour
    private static readonly Statistics CurrentStatistics = new Statistics();
    private static Statistics _LastStatisticsSnapshot = new Statistics
    {
      SnapshotTime = DateTime.UtcNow
    };
    private static int _SnapshottingStatistics;

    private static readonly Random Random = new Random();

    private string GetCacheImage()
    {
      var cacheType = GetCacheType();
      var cacheKey = GetCacheKey();
      var expiration = MemCache.CacheExpiration;
      var fuzzFactor = MemCache.CacheFuzzFactor;

      // apply fuzz factor
      var fuzz = Convert.ToInt32(Math.Round(expiration * (fuzzFactor / 100.0)));
      if (fuzz != 0)
        lock (Random)
          expiration += Random.Next(-fuzz, fuzz);
      var minDateStamp = DateTime.UtcNow - new TimeSpan(0, expiration, 0);

      byte[] cacheBlob = null;
      var localBlobTable = CachePages.GetUnexpiredPageImage(cacheType, cacheKey,
        minDateStamp);

      if (localBlobTable.Count == 1)
      {
        cacheBlob = localBlobTable[0].PageImage;
        Interlocked.Increment(ref CurrentStatistics.PagesServedFromLocalCache);
      }
      else
      {
        var commonBlobTable =
          DB.VoteCache.CachePages.GetUnexpiredPageImage(cacheType, cacheKey,
            minDateStamp);
        if (commonBlobTable.Count == 1)
        {
          cacheBlob = commonBlobTable[0].PageImage;
          SaveLocalCacheBlob(cacheType, cacheKey, commonBlobTable[0].DateStamp,
            cacheBlob);
          Interlocked.Increment(ref CurrentStatistics.PagesServedFromCommonCache);
        }
        else
          Interlocked.Increment(ref CurrentStatistics.PagesCreated);
      }

      if (_LastStatisticsSnapshot.SnapshotTime + StatisticsSnapshotInterval <
        DateTime.UtcNow)
        // May need to snapshot statistics, but make sure only one thread does it
        if (Interlocked.Exchange(ref _SnapshottingStatistics, 1) == 0)
        // it's our job
        {
          var previousSnapshot = _LastStatisticsSnapshot;
          _LastStatisticsSnapshot = CurrentStatistics.Clone();
          _LastStatisticsSnapshot.SnapshotTime = DateTime.UtcNow;
          LogInfo("PageCaching",
            $"Page caching from {previousSnapshot.SnapshotTime} to {_LastStatisticsSnapshot.SnapshotTime}:" +
            $" {_LastStatisticsSnapshot.PagesServedFromMemoryCache - previousSnapshot.PagesServedFromMemoryCache} from memory cache," +
            $" {_LastStatisticsSnapshot.PagesServedFromLocalCache - previousSnapshot.PagesServedFromLocalCache} from local cache," +
            $" {_LastStatisticsSnapshot.PagesServedFromCommonCache - previousSnapshot.PagesServedFromCommonCache} from common cache," +
            $" {_LastStatisticsSnapshot.PagesCreated - previousSnapshot.PagesCreated} created");
          Interlocked.Exchange(ref _SnapshottingStatistics, 0);
        }

      return cacheBlob == null ? null : Encoding.UTF8.GetString(cacheBlob);
    }

    private static string InsertNoCacheIntoRenderedPage(string renderedPage)
    {
      // to add an additional nocache page, add it to the regex in this
      // method and override its SuppressCaching property
      renderedPage = Regex.Replace(renderedPage,
        @"/(?:image|intro|politicianissue).aspx\?",
        match => InsertNoCacheIntoUrl(match.Value), RegexOptions.IgnoreCase);
      return renderedPage;
    }

    private void SaveCacheImage(string cacheImage)
    {
      var cacheType = GetCacheType();
      var cacheKey = GetCacheKey();
      var dateStamp = DateTime.UtcNow;
      var cacheBlob = Encoding.UTF8.GetBytes(cacheImage);
      SaveCommonCacheBlob(cacheType, cacheKey, dateStamp, cacheBlob);
      SaveLocalCacheBlob(cacheType, cacheKey, dateStamp, cacheBlob);
    }

    private static void SaveCommonCacheBlob(string cacheType, string cacheKey,
      DateTime dateStamp, byte[] cacheBlob)
    {
      // Save the rendered page in the common cache store.
      // We ignore errors because parallel threads may try to create an
      // identical cache entry
      try
      {
        DB.VoteCache.CachePages.Upsert(cacheType, cacheKey, dateStamp, cacheBlob);
      }
      catch
      {
        // ignored
      }
    }

    private static void SaveLocalCacheBlob(string cacheType, string cacheKey,
      DateTime dateStamp, byte[] cacheBlob)
    {
      // Save the rendered page in the local cache store.
      // We ignore errors because parallel threads may try to create an
      // identical cache entry
      try
      {
        CachePages.Upsert(cacheType, cacheKey, dateStamp, cacheBlob);
      }
      catch
      {
        // ignored
      }
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    protected abstract string GetCacheKey();

    public string CacheKey
    {
      get
      {
        return GetCacheKey();
      }
    }

    protected abstract string GetCacheType();

    protected virtual bool SuppressCaching { get { return false; } }

    protected static bool UrlContainsNoCache { get { return GetQueryString(NoCacheParameter) == "1"; } }

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Event handlers and overrides

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);
      if (MemCache.CacheExpiration == 0 || SecurePage.IsSignedIn || Request.HttpMethod == "POST" || SuppressCaching)
        return;
      var cacheImage = GetCacheImage();
      if (string.IsNullOrEmpty(cacheImage)) return;
      Response.Write(cacheImage);
      Response.End();
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if ((MemCache.CacheExpiration != 0 && !SecurePage.IsSignedIn ||
        SuppressCaching) && Request.HttpMethod != "POST")
      {
        // Render the page to a string
        var stringWriter = new StringWriter();
        var textWriter = new HtmlTextWriter(stringWriter);
        base.Render(textWriter);
        var renderedPage = stringWriter.ToString();
        if (SuppressCaching)
          // insert the no-cache code into all cached images
          renderedPage = InsertNoCacheIntoRenderedPage(renderedPage);
        else
          SaveCacheImage(renderedPage);
        writer.Write(renderedPage);
      }
      else
        base.Render(writer);
    }

    #endregion Event handlers and overrides
  }
}