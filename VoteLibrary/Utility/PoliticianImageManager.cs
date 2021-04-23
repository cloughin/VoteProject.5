using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using DB.Vote;
using static System.String;

namespace Vote
{
  public static class ImageManager
  {
    #region Local caching methods

    private static void CopyCommonBlobsToLocal(string politicianKey)
    {
      PoliticiansImagesBlobsTable table = null;

      try
      {
        table = PoliticiansImagesBlobs.GetCacheDataByPoliticianKey(politicianKey);
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch {}
      // suppress errors on Insert because a parallel thread may have already
      // copied the row into the local database
      try
      {
        if (table != null && table.Count == 1)
        {
          var row = table[0];
          DB.VoteImagesLocal.PoliticiansImagesBlobs.Upsert(politicianKey, null,
            row.Profile300, row.Profile200, row.Headshot100, row.Headshot75,
            row.Headshot50, row.Headshot35, row.Headshot25, row.Headshot20,
            row.Headshot15);
        }
        else if (table != null && table.Count == 0) // negative caching
          DB.VoteImagesLocal.PoliticiansImagesBlobs.Upsert(politicianKey, null, null,
            null, null, null, null, null, null, null, null);
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch {}
    }

    public static void CopyCommonDataToLocal(string politicianKey)
    {
      CopyCommonBlobsToLocal(politicianKey);
      var table = PoliticiansImagesData.GetDataByPoliticianKey(politicianKey);
      // suppress errors on Insert because a parallel thread may have already
      // copied the row into the local database
      try
      {
        switch (table.Count)
        {
          case 1:
            {
              var row = table[0];
              DB.VoteImagesLocal.PoliticiansImagesData.Upsert(politicianKey,
                row.ProfileOriginalDate, row.HeadshotDate, row.HeadshotResizeDate,
                DateTime.UtcNow);
            }
            break;

          case 0:
            DB.VoteImagesLocal.PoliticiansImagesData.Upsert(politicianKey, 
              VoteDb.DateTimeMin, VoteDb.DateTimeMin, VoteDb.DateTimeMin,
              VoteDb.DateTimeMin);
            break;
        }
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch {}
    }

    private static DateTime GetHeadshotResizeDate(string politicianKey,
      bool noCache)
    {
      if (noCache)
        return PoliticiansImagesData.GetHeadshotResizeDate(politicianKey,
          DateTime.MinValue);

      var result =
        DB.VoteImagesLocal.PoliticiansImagesData.GetHeadshotResizeDate(
          politicianKey);

      if (result != null)
        return result.Value;

      CopyCommonDataToLocal(politicianKey);
      return
        DB.VoteImagesLocal.PoliticiansImagesData.GetHeadshotResizeDate(
          politicianKey, DateTime.MinValue);
    }

    private static DateTime GetProfileOriginalDate(string politicianKey,
      bool noCache)
    {
      if (noCache)
        return PoliticiansImagesData.GetProfileOriginalDate(politicianKey,
          DateTime.MinValue);

      var result =
        DB.VoteImagesLocal.PoliticiansImagesData.GetProfileOriginalDate(
          politicianKey);

      if (result != null)
        return result.Value;

      CopyCommonDataToLocal(politicianKey);
      return
        DB.VoteImagesLocal.PoliticiansImagesData.GetProfileOriginalDate(
          politicianKey, DateTime.MinValue);
    }

    #endregion Local caching methods

    #region Private data members

    private class ApplicationCacheEntry
    {
      public DateTime Expiration;
      public byte[] Blob;
      public string ContentType;
    }

    private class Statistics
    {
      public DateTime SnapshotTime;
      public int ImagesServedFromBrowserCache;
      public int ImagesServedFromMemoryCache;
      public int ImagesServedFromDisc;

      public Statistics Clone()
      {
        return new Statistics
          {
            SnapshotTime = SnapshotTime,
            ImagesServedFromBrowserCache = ImagesServedFromBrowserCache,
            ImagesServedFromMemoryCache = ImagesServedFromMemoryCache,
            ImagesServedFromDisc = ImagesServedFromDisc
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

    // This table allows iteration through the various profile and headshot
    // image sizes
    // Except for the ProfileOriginal, the height limits are to prevent silliness.
    private static readonly List<PoliticianImageInfo> PoliticianImageInfoList =
      new List<PoliticianImageInfo>
        {
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.ProfileOriginal, true,
            true, 1600, 1600, true),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Profile300, false,
            true, 300, 900),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Profile200, false,
            true, 200, 600),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot100, false,
            false, 100, 300),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot75, false,
            false, 75, 225),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot50, false,
            false, 50, 150),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot35, false,
            false, 35, 105),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot25, false,
            false, 25, 75),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot20, false,
            false, 20, 60),
          new PoliticianImageInfo(PoliticiansImagesBlobs.Column.Headshot15, false,
            false, 15, 45)
        };

    #endregion Private data members

    #region General public methods

    public static string GetPoliticianImageColumnNameByWidth(int width)
    {
      var politicianImageInfo =
        PoliticianImageInfoList.SingleOrDefault(info => info.MaxWidth == width);

      return politicianImageInfo == null
        ? Empty
        : PoliticiansImagesBlobs.GetColumnName(politicianImageInfo.BlobsColumn);
    }

    #endregion General public methods

    #region Methods to retrieve and serve a politician image

    public static string GetContentType(Image image)
    {
      string contentType = null;

      if (image.RawFormat.Equals(ImageFormat.Bmp))
        contentType = "image/bmp";
      else if (image.RawFormat.Equals(ImageFormat.Gif))
        contentType = "image/gif";
      else if (image.RawFormat.Equals(ImageFormat.Jpeg))
        contentType = "image/jpeg";
      else if (image.RawFormat.Equals(ImageFormat.Png))
        contentType = "image/png";
      else if (image.RawFormat.Equals(ImageFormat.Tiff))
        contentType = "image/tiff";

      return contentType;
    }

    public static byte[] GetPoliticianImage(string politicianKey,
      string columnName, string defaultColumnName, bool noCache)
    {
      byte[] blob;

      if (noCache)
      {
        // Validate the columnName and get the column enum from the columnName

        if (columnName == null ||
          !PoliticiansImagesBlobs.TryGetColumn(columnName, out var column))
          return null;

        // The following will be null if the politicianKey is bad, if the image doesn't
        // exist, or if a valid but non-image column name was passed
        blob = PoliticiansImagesBlobs.GetColumn(column, politicianKey) as byte[];

        // Optional recursive call to return the appropriate NoPhoto image if 
        // the requested image not found
        if (!IsNullOrWhiteSpace(defaultColumnName) && blob == null)
        {
          // if the defaultColumnName is invalid, use the original column
          if (!PoliticiansImagesBlobs.TryGetColumn(defaultColumnName, out column))
            defaultColumnName = columnName;
          blob = GetPoliticianImage("NoPhoto", defaultColumnName, null, true);
        }
      }
      else // normal caching
      {
        // Validate the columnName and get the column enum from the columnName
        if (columnName == null ||
          !DB.VoteImagesLocal.PoliticiansImagesBlobs.TryGetColumn(columnName,
            out var column))
          return null;

        // The following will be null if the politicianKey is bad, if the image doesn't
        // exist, or if a valid but non-image column name was passed
        blob =
          DB.VoteImagesLocal.PoliticiansImagesBlobs.GetColumn(column, politicianKey)
            as byte[];

        // Optional recursive call to return the appropriate NoPhoto image if 
        // the requested image not found
        if (!IsNullOrWhiteSpace(defaultColumnName) && blob == null)
        {
          // make sure the NoPhoto image has been cached
          if (
            !DB.VoteImagesLocal.PoliticiansImagesData.PoliticianKeyExists("NoPhoto"))
            CopyCommonDataToLocal("NoPhoto");
          // if the defaultColumnName is invalid, use the original column
          if (
            !DB.VoteImagesLocal.PoliticiansImagesBlobs.TryGetColumn(
              defaultColumnName, out column))
            defaultColumnName = columnName;
          blob = GetPoliticianImage("NoPhoto", defaultColumnName, null, false);
        }
      }

      return blob;
    }

    private static bool IsColumnNameProfileImage(string columnName)
    {
      var result = false;

      if (PoliticiansImagesBlobs.TryGetColumn(columnName, out var column))
      {
        var politicianImageInfo =
          PoliticianImageInfoList.SingleOrDefault(
            info => info.BlobsColumn == column);
        if (politicianImageInfo != null)
          result = politicianImageInfo.IsProfile;
      }

      return result;
    }

    public static void ServeImagePage(VotePage votePage)
    {
      var politicianKey = VotePage.QueryId;
      var column = VotePage.GetQueryString("Col");
      if (IsNullOrWhiteSpace(column)) column = "Profile300";
      // Set &Def=ColumnName to show the NoPhoto if requested image is missing
      // and to use the ColumnName for the size. If Def= is present but the
      // column name is invalid, the columnName from Col= will be used
      var defaultColumn = VotePage.GetQueryString("Def");
      // Include <NoCacheParameter>=1 (meaning nocache=true) to force the image to be be served
      // from the common server to avoid cache invalidation latency. This should
      // ONLY be used on master and admin maintenance forms.
      // Use the NoCacheParameter property so we can change the value if need be.
      var noCacheParameter = VotePage.GetQueryString(VotePage.NoCacheParameter);
      int.TryParse(noCacheParameter, out var noCacheValue);
      var noCache = noCacheValue == 1 || votePage.NoCacheViaCookie;

      // Added 09/10/2012 to prevent serving an empty image
      if (IsNullOrWhiteSpace(defaultColumn))
        defaultColumn = column;

      // Added 09/10/2012 -- we no longer serve Profile500 & Profile400

      if (column == "Profile500" || column == "Profile400")
        column = "Profile300";
      if (defaultColumn == "Profile500" || defaultColumn == "Profile400")
        defaultColumn = "Profile300";

      ServePoliticianImage(
        politicianKey, column, defaultColumn, noCache);
    }

    public static void ServePoliticianImage(string politicianKey, string column,
      string defaultColumn, bool noCache)
    {
      if (SecurePage.IsSignedIn) noCache = true;
      var request = HttpContext.Current.Request;

      // Get the appropriate modification date
      DateTime lastModDate;
      if (noCache)
        lastModDate = IsColumnNameProfileImage(column)
          ? GetProfileOriginalDate(politicianKey, true)
          : GetHeadshotResizeDate(politicianKey, true);
      else
      {
        // This triggers a copy to local is it's not there
        var modDates = MemCache.GetImageModDates(politicianKey);
        lastModDate = IsColumnNameProfileImage(column)
          ? modDates.ProfileDate
          : modDates.HeadshotDate;
      }
      lastModDate = new DateTime(lastModDate.Ticks, DateTimeKind.Utc);

      // To force all images older than this date to refresh
      var minimumModDate = new DateTime(2013, 05, 21, 14, 0, 0, DateTimeKind.Utc);
      if (minimumModDate > lastModDate)
        lastModDate = minimumModDate;

      // First check if the image has been modified
      var ifModifiedSinceHeader = request.Headers["If-Modified-Since"];

      var isModified = true; // assume modified unless we prove otherwise
      if (!IsNullOrWhiteSpace(ifModifiedSinceHeader) &&
        DateTime.TryParse(ifModifiedSinceHeader, null,
          DateTimeStyles.AdjustToUniversal, out var ifModifiedSince))
      {
        isModified = false; // change our assumption
        // If mod date is greater, we need to check for insignificant (< 1 sec)
        // difference, because of lossy date conversions.
        if (lastModDate > ifModifiedSince)
          isModified = lastModDate - ifModifiedSince > TimeSpan.FromSeconds(1);
      }

      ServeImageContent(politicianKey, column, defaultColumn, lastModDate,
        isModified, noCache);
    }

    private static void ServeImageContent(string politicianKey, string column,
      string defaultColumn, DateTime lastModDate, bool isModified, bool noCache)
    {
      var response = HttpContext.Current.Response;
      //var maxAge = new TimeSpan(24, 0, 0); // 24 hours -- used in headers
      var maxAge = new TimeSpan(0, 0, 0); // force a server query always
      var expiration = DateTime.UtcNow + maxAge;
      var isProfileOriginal =
        column.IsEqIgnoreCase(
          PoliticiansImagesBlobs.GetColumnName(
            PoliticiansImagesBlobs.Column.ProfileOriginal));

      if (!isModified)
      {
        response.StatusCode = 304;
        response.StatusDescription = "Not Modified";
      }

      response.Cache.SetCacheability(HttpCacheability.Public);
      response.Cache.SetETag('"' +
        lastModDate.Ticks.ToString(CultureInfo.InvariantCulture) + '"');
      response.Cache.SetLastModified(lastModDate);
      response.Cache.SetMaxAge(maxAge);
      response.Cache.SetExpires(expiration);
      response.Cache.SetSlidingExpiration(false);
      response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
      if (isModified) // serve actual image
      {
        var cache = HttpContext.Current.Cache;
        var cacheKey = "politicianImage." + politicianKey + "." + column;

        ApplicationCacheEntry cacheEntry = null;
        // We don't use memory cache for profile originals
        if (!noCache && !isProfileOriginal)
          cacheEntry = cache.Get(cacheKey) as ApplicationCacheEntry;
        if (cacheEntry != null && cacheEntry.Expiration > DateTime.UtcNow)
        {
          response.ContentType = cacheEntry.ContentType;
          response.BinaryWrite(cacheEntry.Blob);
          Interlocked.Increment(ref CurrentStatistics.ImagesServedFromMemoryCache);
        }
        else
        {
          var blob = GetPoliticianImage(politicianKey, column, defaultColumn,
            noCache);
          if (blob != null)
          {
            // we get the content type from the actual image
            var contentType = GetContentType(blob);
            response.ContentType = contentType;
            response.BinaryWrite(blob);

            if (!isProfileOriginal)
            {
              var cacheExpiration =
                DateTime.UtcNow.AddMinutes(MemCache.CacheExpiration);
              cacheEntry = new ApplicationCacheEntry
                {
                  Expiration = cacheExpiration,
                  Blob = blob,
                  ContentType = contentType
                };
              cache.Add(cacheKey, cacheEntry, null, cacheExpiration,
                Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
              
            }
            Interlocked.Increment(ref CurrentStatistics.ImagesServedFromDisc);
          }
        }
      }
      else // tell browser to use cached version
      {
        response.AddHeader("Content-Length", "0");
        Interlocked.Increment(ref CurrentStatistics.ImagesServedFromBrowserCache);
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
          VotePage.LogInfo("ImageCaching",
            $"Image caching from {previousSnapshot.SnapshotTime} to {_LastStatisticsSnapshot.SnapshotTime}:" +
            $" {_LastStatisticsSnapshot.ImagesServedFromBrowserCache - previousSnapshot.ImagesServedFromBrowserCache} from browser cache," +
            $" {_LastStatisticsSnapshot.ImagesServedFromMemoryCache - previousSnapshot.ImagesServedFromMemoryCache} from memory cache," +
            $" {_LastStatisticsSnapshot.ImagesServedFromDisc - previousSnapshot.ImagesServedFromDisc} from disc");
          Interlocked.Exchange(ref _SnapshottingStatistics, 0);
        }

      response.End();
    }

    #endregion Methods to retrieve and serve a politician image

    #region Methods for enumerating through the PoliticianImageInfoList

    // Do not call the private method directly, even from within this class. Use one
    // of the speciifically-named wrappers, or create a new wrapper.

    private static PoliticianImageInfo GetImageToUseForDuplicateTesting()
    {
      var matches =
        PoliticianImageInfoList.Where(info => info.UseForDuplicateTesting)
          .ToArray();
      if (matches.Length > 1)
        throw new VoteException(
          "PoliticianImageInfoList: multiple duplicate testers");
      return matches.Length == 0 ? null : matches[0];
    }

    private static IEnumerable<PoliticianImageInfo>
      GetAllProfilePoliticianImageInfos()
    {
      return GetPoliticianImageInfos(true, true, true, false);
    }

    //public static IEnumerable<PoliticianImageInfo> GetResizedProfilePoliticianImageInfos()
    //{
    //  return GetPoliticianImageInfos(false, true, true, false);
    //}

    private static IEnumerable<PoliticianImageInfo>
      GetResizedHeadshotPoliticianImageInfos()
    {
      return GetPoliticianImageInfos(false, true, false, true);
    }

    private static IEnumerable<PoliticianImageInfo> GetPoliticianImageInfos(
      bool includeOriginals, bool includeResizes, bool includeProfiles,
      bool includeHeadshots)
    {
      return PoliticianImageInfoList.Where(info => // The filter function
        (includeOriginals && info.IsOriginal || includeResizes && !info.IsOriginal) &&
          (includeProfiles && info.IsProfile || includeHeadshots && !info.IsProfile));
    }

    #endregion Methods for enumerating through the PoliticianImageInfoList

    #region Methods to update the politician profile and headshot images

    private static ImageCodecInfo GetCodecInfo(ImageFormat format)
    {
      var codecs = ImageCodecInfo.GetImageDecoders();

      return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }

    public static string GetContentType(byte[] blob)
    {
      var memoryStream = new MemoryStream(blob);
      var image = Image.FromStream(memoryStream);

      return GetContentType(image);
    }

    //public static byte[] GetResizedImageBlob(Image image, int maxWidth,
    //  int maxHeight)
    //{
    //  Bitmap originalBitmap = null;
    //  Bitmap tempBitmap = null;

    //  try
    //  {
    //    originalBitmap = new Bitmap(image);
    //    // necessary to handle indexed pixel formats

    //    // Check if any sort of resizing is needed. Note: a max of 0 means no limit.
    //    var newWidth = originalBitmap.Width;
    //    var newHeight = originalBitmap.Height;
    //    if (maxWidth != 0 && newWidth > maxWidth) // too wide
    //    {
    //      newWidth = maxWidth;
    //      newHeight =
    //        Convert.ToInt32(
    //          Math.Round(Convert.ToDouble(originalBitmap.Height)*
    //            Convert.ToDouble(maxWidth)/Convert.ToDouble(originalBitmap.Width)));
    //    }
    //    if (maxHeight != 0 && newHeight > maxHeight) // too tall
    //    {
    //      newHeight = maxHeight;
    //      newWidth =
    //        Convert.ToInt32(
    //          Math.Round(Convert.ToDouble(originalBitmap.Width)*
    //            Convert.ToDouble(maxHeight)/Convert.ToDouble(originalBitmap.Height)));
    //    }

    //    Bitmap bitmapToReturn;
    //    if (newWidth != originalBitmap.Width || newHeight != originalBitmap.Height)
    //    { // Must resize
    //      tempBitmap = new Bitmap(newWidth, newHeight, originalBitmap.PixelFormat);
    //      var graphics = Graphics.FromImage(tempBitmap);
    //      graphics.SmoothingMode = SmoothingMode.HighQuality;
    //      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    //      graphics.DrawImage(originalBitmap, 0, 0, tempBitmap.Width,
    //        tempBitmap.Height);
    //      bitmapToReturn = tempBitmap;
    //    }
    //    else
    //      bitmapToReturn = originalBitmap;

    //    // Save as png and extract blob
    //    Byte[] blob;
    //    using (var memoryStream = new MemoryStream())
    //    {
    //      bitmapToReturn.Save(memoryStream, ImageFormat.Png);
    //      // always convert to png
    //      blob = memoryStream.GetBuffer();
    //    }

    //    return blob;
    //  }
    //  finally
    //  {
    //    // Clean up no matter what
    //    originalBitmap?.Dispose();
    //    tempBitmap?.Dispose();
    //  }
    //}

    public static byte[] GetResizedImageBlobAsJpg(Image image, int maxWidth,
      int maxHeight)
    {
      return GetResizedImageBlobAsJpg(image, maxWidth, maxHeight, 80);
    }

    private static byte[] GetResizedImageBlobAsJpg(Image image, int maxWidth,
      int maxHeight, int quality /*0-100*/)
    {
      Bitmap originalBitmap = null;
      Bitmap tempBitmap = null;

      try
      {
        originalBitmap = new Bitmap(image);
        // necessary to handle indexed pixel formats

        // Check if any sort of resizing is needed. Note: a max of 0 means no limit.
        var newWidth = originalBitmap.Width;
        var newHeight = originalBitmap.Height;
        if (maxWidth != 0 && newWidth > maxWidth) // too wide
        {
          newWidth = maxWidth;
          newHeight =
            Convert.ToInt32(
              Math.Round(Convert.ToDouble(originalBitmap.Height)*
                Convert.ToDouble(maxWidth)/Convert.ToDouble(originalBitmap.Width)));
        }
        if (maxHeight != 0 && newHeight > maxHeight) // too tall
        {
          newHeight = maxHeight;
          newWidth =
            Convert.ToInt32(
              Math.Round(Convert.ToDouble(originalBitmap.Width)*
                Convert.ToDouble(maxHeight)/Convert.ToDouble(originalBitmap.Height)));
        }

        Bitmap bitmapToReturn;
        if (newWidth != originalBitmap.Width || newHeight != originalBitmap.Height)
        { // Must resize
          tempBitmap = new Bitmap(newWidth, newHeight, originalBitmap.PixelFormat);
          var graphics = Graphics.FromImage(tempBitmap);
          graphics.SmoothingMode = SmoothingMode.HighQuality;
          graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
          graphics.DrawImage(originalBitmap, 0, 0, tempBitmap.Width,
            tempBitmap.Height);
          bitmapToReturn = tempBitmap;
        }
        else
          bitmapToReturn = originalBitmap;

        // Save as jpg and extract blob
        byte[] blob;
        using (var memoryStream = new MemoryStream())
        {
          var codecInfo = GetCodecInfo(ImageFormat.Jpeg);
          var encoder = Encoder.Quality;
          var encoderParameter = new EncoderParameter(encoder, quality);
          var encoderParameters = new EncoderParameters(1) {Param = {[0] = encoderParameter}};
          bitmapToReturn.Save(memoryStream, codecInfo, encoderParameters);
          blob = memoryStream.GetBuffer();
        }

        return blob;
      }
      finally
      {
        // Clean up no matter what
        originalBitmap?.Dispose();
        tempBitmap?.Dispose();
      }
    }

    public static void UpdateAllPoliticianHeadshotImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, out Size originalSize)
    {
      UpdatePoliticianImages(politicianKey, imageStream, uploadTime, false, true,
        true, out originalSize);
    }

    public static void UpdateResizedPoliticianHeadshotImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, out Size originalSize)
    {
      UpdatePoliticianImages(politicianKey, imageStream, uploadTime, false, false,
        true, out originalSize);
    }

    public static byte[] UpdateAllPoliticianImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, bool checkForDuplicates,
      out Size originalSize, out byte[] originalBlob)
    {
      return UpdatePoliticianImages(politicianKey, imageStream, uploadTime, true,
        false, true, checkForDuplicates, out originalSize, out originalBlob);
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static byte[] UpdatePoliticianProfileImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, out Size originalSize)
    {
      return UpdatePoliticianImages(politicianKey, imageStream, uploadTime, true,
        false, false, out originalSize);
    }

    public static byte[] UpdatePoliticianProfileImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, bool checkForDuplicates,
      out Size originalSize, out byte[] originalBlob)
    {
      return UpdatePoliticianImages(politicianKey, imageStream, uploadTime, true,
        false, false, checkForDuplicates, out originalSize, out originalBlob);
    }

    private static byte[] UpdatePoliticianImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, bool includeAllProfileImages,
      bool includeHeadshotOriginal, bool includeHeadshotResizedImages,
      out Size originalSize)
    {
      return UpdatePoliticianImages(politicianKey, imageStream, uploadTime,
        includeAllProfileImages, includeHeadshotOriginal,
        includeHeadshotResizedImages, false, out originalSize, out _);
    }

    private static byte[] UpdatePoliticianImages(string politicianKey,
      Stream imageStream, DateTime uploadTime, bool includeAllProfileImages,
      bool includeHeadshotOriginal, bool includeHeadshotResizedImages,
      bool checkForDuplicates, out Size originalSize, out byte[] originalBlob)
    {
      var memoryStream = new MemoryStream();
      imageStream.Position = 0;
      imageStream.CopyTo(memoryStream);
      imageStream.Position = 0;
      var image = Image.FromStream(imageStream);
      originalSize = image.Size;
      var blob = memoryStream.ToArray();

      if (!image.RawFormat.Equals(ImageFormat.Jpeg)) // could be transparent
      {
        // draw on a white background
        var b = new Bitmap(image.Width, image.Height);
        b.SetResolution(image.HorizontalResolution, image.VerticalResolution);
        using (var g = Graphics.FromImage(b))
        {
          g.Clear(Color.White);
          g.DrawImageUnscaled(image, 0, 0);
          //b.Save(@"c:\temp\zz.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        image = b;
      }

      // This is so all subsequent db operations can be updates
      PoliticiansImagesData.GuaranteePoliticianKeyExists(politicianKey);
      PoliticiansImagesBlobs.GuaranteePoliticianKeyExists(politicianKey);

      originalBlob = null;
      if (checkForDuplicates)
      {
        // We fetch this image to see if its a duplicate
        var duplicateTester = GetImageToUseForDuplicateTesting();
        if (duplicateTester != null)
        {
          var duplicateTesterBlob = GetResizedImageBlobAsJpg(image,
            duplicateTester.MaxWidth, duplicateTester.MaxHeight);
          originalBlob = duplicateTesterBlob;
          if (PoliticiansImagesBlobs.GetColumn(duplicateTester.BlobsColumn,
            politicianKey) is byte[] dbBlob && dbBlob.Length == duplicateTesterBlob.Length)
          {
            var isDuplicate =
              !dbBlob.Where((t, inx) => t != duplicateTesterBlob[inx])
                .Any();
            if (isDuplicate)
              return null; // signals duplicate
          }
        }
      }

      if (includeAllProfileImages)
      {
        PoliticiansImagesData.UpdateProfileOriginalDate(uploadTime, politicianKey);
        //PoliticiansImagesBlobs.UpdateProfileOriginal(blob, politicianKey);

        foreach (var info in GetAllProfilePoliticianImageInfos())
        {
          var resizedBlob = GetResizedImageBlobAsJpg(image, info.MaxWidth,
            info.MaxHeight);
          PoliticiansImagesBlobs.UpdateColumn(info.BlobsColumn, resizedBlob,
            politicianKey);
        }
      }

      if (includeHeadshotOriginal)
        PoliticiansImagesData.UpdateHeadshotDate(uploadTime, politicianKey);

      if (includeHeadshotResizedImages)
      {
        foreach (var info in GetResizedHeadshotPoliticianImageInfos())
        {
          var resizedBlob = GetResizedImageBlobAsJpg(image, info.MaxWidth,
            info.MaxHeight);
          PoliticiansImagesBlobs.UpdateColumn(info.BlobsColumn, resizedBlob,
            politicianKey);
        }
        PoliticiansImagesData.UpdateHeadshotResizeDate(uploadTime, politicianKey);
      }

      // Don't do this here. It belongs as a separate call.
      //CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

      // We return the blob for logging purposes
      return blob;
    }

    #endregion Methods to update the politician profile and headshot images
  }

  #region PoliticianImageInfo (helper class)

  public class PoliticianImageInfo
  {
    public PoliticianImageInfo(PoliticiansImagesBlobs.Column blobsColumn,
      bool isOriginal, bool isProfile, int maxWidth, int maxHeight,
      bool useForDuplicateTesting = false)
    {
      BlobsColumn = blobsColumn;
      IsOriginal = isOriginal;
      IsProfile = isProfile;
      MaxWidth = maxWidth;
      MaxHeight = maxHeight;
      UseForDuplicateTesting = useForDuplicateTesting;
    }

    public PoliticiansImagesBlobs.Column BlobsColumn { get; }

    public bool IsOriginal { get; }

    public bool IsProfile { get; }

    public int MaxHeight { get; }

    public int MaxWidth { get; }

    public bool UseForDuplicateTesting { get; }
  }

  #endregion PoliticianImageInfo (helper class)
}