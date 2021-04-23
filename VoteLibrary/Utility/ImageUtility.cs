using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using DB.Vote;
using static System.String;

namespace Vote
{
  public static class ImageUtility
  {

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

    public static string GetContentType(byte[] blob)
    {
      var memoryStream = new MemoryStream(blob);
      var image = Image.FromStream(memoryStream);

      return GetContentType(image);
    }

    public static void ServeImage(string externalName)
    {
      var request = HttpContext.Current.Request;

      var lastModDate = UploadedImages.GetImageChangeTimeByExternalName(externalName);
      if (lastModDate == null)
      {
        Utility.Signal404();
        return;
      }
      lastModDate = new DateTime(lastModDate.Value.Ticks, DateTimeKind.Utc);

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

      ServeImageContent(externalName, lastModDate.Value,
        isModified);
    }

    private static void ServeImageContent(string externalName, DateTime lastModDate, bool isModified)
    {
      var response = HttpContext.Current.Response;
      var maxAge = new TimeSpan(0, 0, 0); // force a server query always
      var expiration = DateTime.UtcNow + maxAge;

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
        var blob = UploadedImages.GetImageByExternalName(externalName);
        if (blob == null)
          Utility.Signal404();
        else
        {
          // we get the content type from the actual image
          var contentType = GetContentType(blob);
          response.ContentType = contentType;
          response.BinaryWrite(blob);
        }
      }
      else // tell browser to use cached version
        response.AddHeader("Content-Length", "0");

      response.End();
    }
  }
}
