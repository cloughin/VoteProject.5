using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class AdImageDefaultPage : VotePage
  {
    public static void ServeImage(byte[] blob)
    {
      var request = HttpContext.Current.Request;

      // First check if the image has been modified
      var ifModifiedSinceHeader = request.Headers["If-Modified-Since"];

      var isModified = !(!IsNullOrWhiteSpace(ifModifiedSinceHeader) &&
        DateTime.TryParse(ifModifiedSinceHeader, null, DateTimeStyles.AdjustToUniversal,
          out var ifModifiedSince) &&
        ifModifiedSince.AddDays(1) > DateTime.UtcNow); // assume modified unless we prove otherwise

      ServeImageContent(blob, isModified);
    }

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

    private static void ServeImageContent(byte[] blob, bool isModified)
    {
      var response = HttpContext.Current.Response;
      var maxAge = new TimeSpan(24, 0, 0); // 24 hours -- used in headers
      var expiration = DateTime.UtcNow + maxAge;
      var lastModDate = DateTime.UtcNow;

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
        // we get the content type from the actual image
        var contentType = GetContentType(blob);
        response.ContentType = contentType;
        response.BinaryWrite(blob);
      }
      else // tell browser to use cached version
      {
        response.AddHeader("Content-Length", "0");
      }

      response.End();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var query = Request.Url.Query;
      if (IsNullOrWhiteSpace(query))
        Utility.Signal404();
      var info = Server.UrlDecode(query.Substring(1))?.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      Debug.Assert(info != null, nameof(info) + " != null");
      if (info.Length < 3) Utility.Signal404();
      var electionKey = info[0];
      var officeKey = info[1];
      var politicianKey = info[2];

      var blob = ElectionsPoliticians.GetAdImage(electionKey, officeKey, politicianKey);
      if (blob == null) Utility.Signal404();
      ServeImage(blob);
    }
  }
}