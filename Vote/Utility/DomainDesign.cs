using System;
using System.IO;
using System.Web;
using static System.String;

namespace Vote
{
  // These classes might be reorganized and perhaps renamed later.
  //
  public static class DomainDesign
  {
    private const string ImagePathStringTemplate = "/images/designs/{0}/";

    public static string FromQueryStringOrDomain
    {
      get
      {
        var designCode = HttpContext.Current.Request.QueryString["Design"];
        return IsNullOrWhiteSpace(designCode)
          ? UrlManager.CurrentDomainDesignCode
          : designCode.ToUpperInvariant();
      }
    }

    public static Uri GetImageUri(string imageName)
    {
      return GetImageUri(FromQueryStringOrDomain, imageName);
    }

    private static Uri GetImageUri(string designCode, string imageName)
    {
      return UrlManager.GetDomainDesignCodeUri(
        designCode, ImagePath(designCode, imageName));
    }

    private static string ImagePath(string designCode, string imageName)
    {
      var folder = Format(ImagePathStringTemplate, designCode);
      return Path.Combine(folder, imageName);
    }
  }
}