using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using static System.String;

namespace Vote
{
  public class FacebookVideoInfo : VideoInfo
  {
    public const string InvalidVideoUrlMessage = "Invalid Facebook video URL";
    public const string VideoIdNotFoundMessage = "Video Id not found on Facebook";
    public const string VideoNotPublicMessage = "Facebook video is not publically viewable";

    public FacebookVideoInfo()
    {
    }

    public FacebookVideoInfo(string id) : base(id)
    {
    }

    public FacebookVideoInfo(string id, string title, string description,
      DateTime publishedAt, TimeSpan duration, bool isPublic) : 
      base(id, title, description, publishedAt, duration, isPublic)
    {
    }
  }

  public static class FacebookVideoUtility
  {
    // Extension methods

    public static bool IsValidFacebookVideoUrl(this string str)
    {
      return FacebookVideoIdRegex.IsMatch(str);
    }

    public static string GetFacebookVideoId(this string str)
    {
      if (str == null) return null;
      var match = FacebookVideoIdRegex.Match(str);
      if (!match.Success || match.Groups[1].Captures.Count != 1) return null;
      return match.Groups[1].Captures[0].Value;
    }

    // end Extension methods

    private static readonly Regex FacebookVideoIdRegex = new Regex(
        @"^(?:(?:https?:)?\/\/)?(?:www\.)?facebook\.com\/(?:[a-z0-9\.]+\/videos\/(?:[a-z0-9\.]+\/)?|video.php\?v=)([0-9]+)\/?(?:\?.*)?$",
        RegexOptions.IgnoreCase);

    private static string _AccessToken;

    private static string AccessToken => _AccessToken ?? (_AccessToken =
      WebConfigurationManager.AppSettings["VoteFacebookAccessToken"]);

    private static string _WebProxyUrl;

    private static string WebProxyUrl => _WebProxyUrl ??
      (_WebProxyUrl = WebConfigurationManager.AppSettings["VoteWebProxyUrl"]);

    private const string VideoInfoUrlTemplate =
        "https://graph.facebook.com/v2.11/{0}?fields=length,description,title,live_status,updated_time,created_time,privacy&access_token={1}"
      ;

    private static FacebookVideoInfo DecodeFacebookVideoInfo(string facebookId, string json)
    {
      var serializer = new JavaScriptSerializer();
      var jsonObj = serializer.Deserialize<Dictionary<string, object>>(json);
      if (jsonObj == null) return null;
      jsonObj.TryGetValue("title", out var title);
      jsonObj.TryGetValue("description", out var description);
      jsonObj.TryGetValue("updated_time", out var updatedTime);
      DateTime publishedAt = default;
      if (updatedTime is string timeString) publishedAt = DateTime.Parse(timeString);
      jsonObj.TryGetValue("length", out var lengthObj);
      TimeSpan duration = default;
      if (lengthObj is decimal length)
      {
        var seconds = (int) length;
        var milliseconds = (int) (length % 1.0m * 1000);
        duration = new TimeSpan(0, 0, 0, seconds, milliseconds);
      }
      var privacy = jsonObj["privacy"] as Dictionary<string, object>;
      bool isPublic = default;
      if (privacy != null)
      {
        privacy.TryGetValue("description", out var privacyDescription);
        privacy.TryGetValue("value", out var privacyValue);
        isPublic = privacyDescription as string == "Public" &&
          privacyValue as string == "EVERYONE";
      }
      return new FacebookVideoInfo(facebookId, title as string, description as string,
        publishedAt, duration, isPublic);
    }

    public static string GetUrl(string id)
    {
      return $"https://www.facebook.com/video.php?v={id}";
    }

    public static FacebookVideoInfo GetVideoInfo(string facebookId, bool useProxy,
      int retries = 0)
    {
      try
      {
      var client = new WebClient();
      var url = Format(VideoInfoUrlTemplate, facebookId, AccessToken);
      if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
      var data = client.DownloadData(url);
      var json = System.Text.Encoding.UTF8.GetString(data);
      return DecodeFacebookVideoInfo(facebookId, json);
      }
      // ReSharper disable once UnusedVariable
      catch (Exception)
      {
        if (retries <= 0)
          return new FacebookVideoInfo(facebookId);
      }
      return GetVideoInfo(facebookId, useProxy, retries - 1);
    }
  }
}