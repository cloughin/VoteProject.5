using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using DB.Vote;

namespace Vote
{
  public class YouTubeInfo
  {
    public const string InvalidVideoUrlMessage = "Invalid YouTube URL";
    public const string VideoIdNotFoundMessage = "Video Id not found on YouTube";
    public const string VideoNotPublicMessage = "YouTube video is not publically viewable";
    public const string PlaylistIdNotFoundMessage = "Playlist Id not found on YouTube";
    public const string PlaylistNotPublicMessage = "YouTube playlist is not publically viewable";
    public const string ChannelIdNotFoundMessage = "Channel Id not found on YouTube";
    public const string ChannelNotPublicMessage = "YouTube channel is not publically viewable";
    public const string VideoUploadedByCandidateMessage = "Uploaded by candidate";
    public const int MaxVideoDescriptionLength = 300;

    // ReSharper disable once MemberCanBePrivate.Global
    public readonly string Id;
    public readonly bool IsValid;
    public readonly string Title;
    public readonly string Description;
    public readonly DateTime PublishedAt;
    public readonly TimeSpan Duration;
    public readonly bool IsPublic;

    public YouTubeInfo()
    {
    }

    public YouTubeInfo(string id)
    {
      Id = id;
    }

    public YouTubeInfo(string id, string title, string description, DateTime publishedAt,
      TimeSpan duration, bool isPublic)
    {
      Id = id;
      Title = title;
      Description = description;
      PublishedAt = publishedAt;
      Duration = duration;
      IsPublic = isPublic;
      IsValid = true;
    }

    public string ShortDescription
    {
      get
      {
        var description = Description.SafeString().Trim();
        if (string.IsNullOrWhiteSpace(description) || description.Length > MaxVideoDescriptionLength)
          description = Title.SafeString().Trim();
        return description;
      }
    }
  }

  public static class YouTubeUtility
  {
    // Extension methods

    public static bool IsValidYouTubeChannelUrl(this string str)
    {
      return YouTubeChannelIdRegex.IsMatch(str);
    }

    public static string GetYouTubeStandardChannelId(this string str)
    {
      if (str == null) return null;
      var match = YouTubeChannelIdRegex.Match(str);
      if (!match.Success || match.Groups["id"].Captures.Count != 1) return null;
      return match.Groups["id"].Captures[0].Value;
    }

    public static bool IsValidYouTubeCustomChannelUrl(this string str)
    {
      return YouTubeCustomChannelIdRegex.IsMatch(str);
    }

    public static string GetYouTubeCustomChannelId(this string str)
    {
      if (str == null) return null;
      var match = YouTubeCustomChannelIdRegex.Match(str);
      if (!match.Success || match.Groups["id"].Captures.Count != 1) return null;
      return match.Groups["id"].Captures[0].Value;
    }

    public static string GetYouTubeUrlDescription(this string url)
    {
      if (string.IsNullOrWhiteSpace(url)) return string.Empty;
      if (url.IsValidYouTubeVideoUrl()) return "Video";
      if (url.IsValidYouTubePlaylistUrl()) return "Playlist";
      return "Channel";
    }

    public static bool IsValidYouTubePlaylistUrl(this string str)
    {
      return YouTubePlaylistIdRegex.IsMatch(str);
    }

    public static string GetYouTubePlaylistId(this string str)
    {
      if (str == null) return null;
      var match = YouTubePlaylistIdRegex.Match(str);
      if (!match.Success || match.Groups["id"].Captures.Count != 1) return null;
      return match.Groups["id"].Captures[0].Value;
    }

    public static bool IsValidYouTubeUserChannelUrl(this string str)
    {
      return YouTubeUserChannelIdRegex.IsMatch(str);
    }

    public static string GetYouTubeUserChannelId(this string str)
    {
      if (str == null) return null;
      var match = YouTubeUserChannelIdRegex.Match(str);
      if (!match.Success || match.Groups["id"].Captures.Count != 1) return null;
      return match.Groups["id"].Captures[0].Value;
    }

    public static bool IsValidYouTubeVideoUrl(this string str)
    {
      return YouTubeVideoIdRegex.IsMatch(str);
    }

    public static string GetYouTubeVideoId(this string str)
    {
      if (str == null) return null;
      var match = YouTubeVideoIdRegex.Match(str);
      if (!match.Success || match.Groups["id"].Captures.Count != 1) return null;
      return match.Groups["id"].Captures[0].Value;
    }

    // end Extension methods

    private static readonly Regex YouTubeChannelIdRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?youtube\.com/channel/(?<id>(?:\w|-)+)(?:\S+)?$");
    private static readonly Regex YouTubeCustomChannelIdRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?youtube\.com/c/(?<id>(?:\w|-)+)(?:\S+)?$");
    private static readonly Regex YouTubeUserChannelIdRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?youtube\.com/(?:user/)?(?!channel/|c/)(?<id>(?:\w|-)+)(?:\S+)?$");
    private static readonly Regex YouTubeVideoIdRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))(?<id>(?:\w|-)+(?!\w|-))(?:\S+)?$");
    private static readonly Regex YouTubePlaylistIdRegex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?youtube\.com/playlist\?list=(?<id>(?:\w|-)+(?!\w|-))(?:\S+)?$");
    private static readonly Regex ChannelIdRegex1 = new Regex("data-channel-external-id=\"(?<id>[^\"]+)\"");
    private static readonly Regex ChannelIdRegex2 = new Regex("<link rel=\"canonical\" href=\"https://www.youtube.com/channel/(?<id>[^\"]+)\">");

    private static string _ApiKey;
    private static string ApiKey => _ApiKey ?? (_ApiKey = ConfigurationManager.AppSettings["VoteYouTubeApiKey"]);

    private static string _WebProxyUrl;
    private static string WebProxyUrl => _WebProxyUrl ?? (_WebProxyUrl = ConfigurationManager.AppSettings["VoteWebProxyUrl"]);

    private const string ChannelIdUrlTemplate =
      "https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&part=snippet&fields=items(snippet/channelId)";

    private const string ChannelInfoUrlTemplate =
      "https://www.googleapis.com/youtube/v3/channels?id={0}&key={1}&part=snippet,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,status/privacyStatus)";

    private const string PlaylistInfoUrlTemplate =
      "https://www.googleapis.com/youtube/v3/playlists?id={0}&key={1}&part=snippet,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,status/privacyStatus)";

    private const string VideoInfoUrlTemplate =
      "https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&part=snippet,contentDetails,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,contentDetails/duration,status/privacyStatus)";

    private static YouTubeInfo DecodeYouTubeInfo(string youTubeId, string json)
    {
      // ReSharper disable AssignNullToNotNullAttribute
      var serializer = new JavaScriptSerializer();
      var jsonObj = serializer.Deserialize<Dictionary<string, object>>(json);
      var items = jsonObj["items"] as ArrayList;
      var dictionary = items?[0] as Dictionary<string, object>;
      var snippet = dictionary?["snippet"] as Dictionary<string, object>;
      var title = snippet?["title"] as string;
      var description = snippet?["description"] as string;
      var publishedAt = snippet?["publishedAt"] as string;
      var duration = new TimeSpan();
      if (dictionary != null && dictionary.ContainsKey("contentDetails"))
      {
        var contentDetails = dictionary["contentDetails"] as Dictionary<string, object>;
        if (contentDetails != null && contentDetails.ContainsKey("duration"))
          duration = XmlConvert.ToTimeSpan(contentDetails["duration"] as string);
      }
      var status = dictionary?["status"] as Dictionary<string, object>;
      var privacyStatus = status?["privacyStatus"] as string;

      return new YouTubeInfo(youTubeId, title, description, DateTime.Parse(publishedAt).Date,
        duration, privacyStatus != "private");
      // ReSharper restore AssignNullToNotNullAttribute
    }

    private static string DecodeVideoChannelId(string json)
    {
      // ReSharper disable AssignNullToNotNullAttribute
      var serializer = new JavaScriptSerializer();
      var jsonObj = serializer.Deserialize<Dictionary<string, object>>(json);
      var items = jsonObj["items"] as ArrayList;
      var dictionary = items?[0] as Dictionary<string, object>;
      var snippet = dictionary?["snippet"] as Dictionary<string, object>;
      return snippet?["channelId"] as string;
      // ReSharper restore AssignNullToNotNullAttribute
    }

    public static YouTubeInfo GetChannelInfo(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = string.Format(ChannelInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeInfo(youTubeId);
      }
      return GetChannelInfo(youTubeId, useProxy, retries - 1);
    }

    public static YouTubeInfo GetPlaylistInfo(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = string.Format(PlaylistInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeInfo(youTubeId);
      }
      return GetPlaylistInfo(youTubeId, useProxy, retries - 1);
    }

    public static string GetVideoChannelId(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = string.Format(ChannelIdUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeVideoChannelId(json);
      }
      catch
      {
        if (retries <= 0)
          return null;
      }
      return GetVideoChannelId(youTubeId, useProxy, retries - 1);
    }

    public static YouTubeInfo GetVideoChannelInfo(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var channelId = GetVideoChannelId(youTubeId, useProxy);
        if (string.IsNullOrWhiteSpace(channelId)) throw new Exception();
        return GetChannelInfo(channelId, useProxy);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeInfo(youTubeId);
      }
      return GetVideoChannelInfo(youTubeId, useProxy, retries - 1);
    }

    public static YouTubeInfo GetVideoInfo(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = string.Format(VideoInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeInfo(youTubeId);
      }
      return GetVideoInfo(youTubeId, useProxy, retries - 1);
    }

    public static string LookupChannelId(string url, int retries = 0)
    {
      // There's gotta be a better way...
      // Accepts a "friendly" channel url like:
      //   [www.]youtube.com/c/{name}[?...]
      //   [www.]youtube.com/cuser/{name}[?...]
      //   [www.]youtube.com/{name}[?...]
      //Fetches the page and looks for either
      // 1. data-channel-external-id="{channelId"
      // 2. <link rel="canonical" href="https://www.youtube.com/channel/{channelId}">
      try
      {
        var page = new WebClient().DownloadString(VotePage.NormalizeUrl(url));
        var match = ChannelIdRegex1.Match(page);
        if (match.Success)
          return match.Groups["id"].Captures[0].Value;
        match = ChannelIdRegex2.Match(page);
        if (match.Success)
          return match.Groups["id"].Captures[0].Value;
        return null;
      }
      catch
      {
        if (retries <= 0)
          return null;
      }
      return LookupChannelId(url, retries - 1);
    }

    public static void RefreshYouTubeAnswers()
    {
      string message;
      var expiration = new TimeSpan(3, 0, 0, 0); // 3 days
      // fudge is to account for small differences in run time
      var fudge = new TimeSpan(1, 0, 0); // 1 hour
      var now = DateTime.UtcNow;

      try
      {
        VotePage.LogInfo("UpdatePoliticiansLiveOfficeKey", "Started");

        var answersUpdated = 0;
        var table = Answers.GetDataForYouTubeRefresh(now - expiration + fudge);

        foreach (var row in table)
        {
          row.YouTubeRefreshTime = now;

          var youTubeId = row.YouTubeUrl.GetYouTubeVideoId();
          if (string.IsNullOrWhiteSpace(youTubeId))
          {
            if (row.YouTubeAutoDisable != YouTubeInfo.InvalidVideoUrlMessage)
            {
              row.YouTubeAutoDisable = YouTubeInfo.InvalidVideoUrlMessage;
              answersUpdated++;
            }
            continue;
          }

          var videoInfo = GetVideoInfo(youTubeId, false, 1);
          if (!videoInfo.IsValid)
          {
            if (row.YouTubeAutoDisable != YouTubeInfo.VideoIdNotFoundMessage)
            {
              row.YouTubeAutoDisable = YouTubeInfo.VideoIdNotFoundMessage;
              answersUpdated++;
            }
            continue;
          }

          if (!videoInfo.IsPublic)
          {
            if (row.YouTubeAutoDisable != YouTubeInfo.VideoNotPublicMessage)
            {
              row.YouTubeAutoDisable = YouTubeInfo.VideoNotPublicMessage;
              answersUpdated++;
            }
            continue;
          }

          var description = videoInfo.Description.Trim();
          if (string.IsNullOrWhiteSpace(description) || description.Length > YouTubeInfo.MaxVideoDescriptionLength)
            description = videoInfo.Title.Trim();

          if (description != row.YouTubeDescription ||
            videoInfo.Duration != row.YouTubeRunningTime ||
            row.YouTubeAutoDisable != null ||
            row.YouTubeSource == YouTubeInfo.VideoUploadedByCandidateMessage &&
              videoInfo.PublishedAt != row.YouTubeDate)
          {
            row.YouTubeDescription = description;
            row.YouTubeRunningTime = videoInfo.Duration;
            row.YouTubeAutoDisable = null;
            if (row.YouTubeSource == YouTubeInfo.VideoUploadedByCandidateMessage)
              row.YouTubeDate = videoInfo.PublishedAt;
            answersUpdated++;
          }
        }

        Answers.UpdateTable(table);

        message =
          $"{table.Count} Expired YouTube answers found, {answersUpdated} YouTube Answers updated";

      }
      catch (Exception ex)
      {
        VotePage.LogException("RefreshYouTubeAnswers", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("RefreshYouTubeAnswers", message);
    }

    public static void RefreshYouTubePoliticians()
    {
      // this is so we only do it every three days
      if (DateTime.UtcNow.Day % 3 != 0) return;

      string message;

      try
      {
        VotePage.LogInfo("RefreshYouTubePoliticians", "Started");

        var politiciansUpdated = 0;
        var table = Politicians.GetYouTubeRefreshData();

        foreach (var row in table)
        {
          YouTubeInfo info = null;
          var url = row.YouTubeWebAddress;
          var videoType = string.Empty;
          if (url.IsValidYouTubeVideoUrl())
          {
            var id = url.GetYouTubeVideoId();
            info = GetVideoInfo(id, false, 1);
            videoType = "video";
          }
          else if (url.IsValidYouTubePlaylistUrl())
          {
            var id = url.GetYouTubePlaylistId();
            info = GetPlaylistInfo(id, false, 1);
            videoType = "playlist";
          }
          else if (url.IsValidYouTubeChannelUrl() || url.IsValidYouTubeCustomChannelUrl() ||
            url.IsValidYouTubeUserChannelUrl())
          {
            var id = LookupChannelId(url, 1);
            if (!string.IsNullOrWhiteSpace(id))
              info = GetChannelInfo(id, false, 1);
            videoType = "channel";
          }

          var error = string.Empty;
          if (info == null || !info.IsValid)
          {
            switch (videoType)
            {
              case "video":
                error = YouTubeInfo.VideoIdNotFoundMessage;
                break;

              case "playlist":
                error = YouTubeInfo.PlaylistIdNotFoundMessage;
                break;

              case "channel":
                error = YouTubeInfo.ChannelIdNotFoundMessage;
                break;

              default:
                error = YouTubeInfo.InvalidVideoUrlMessage;
                break;
            }

            if (row.YouTubeAutoDisable != error)
            {
              row.YouTubeAutoDisable = error;
              politiciansUpdated++;
            }
            continue;
          }

          if (!info.IsPublic)
          {
            switch (videoType)
            {
              case "video":
                error = YouTubeInfo.VideoNotPublicMessage;
                break;

              case "playlist":
                error = YouTubeInfo.PlaylistNotPublicMessage;
                break;

              case "channel":
                error = YouTubeInfo.ChannelNotPublicMessage;
                break;
            }
            if (row.YouTubeAutoDisable != error)
            {
              row.YouTubeAutoDisable = error;
              politiciansUpdated++;
            }
            continue;
          }

          if (info.ShortDescription != row.YouTubeDescription ||
            info.Duration != row.YouTubeRunningTime ||
            info.PublishedAt != row.YouTubeDate ||
            row.YouTubeAutoDisable != null)
          {
            row.YouTubeDescription = info.ShortDescription;
            row.YouTubeRunningTime = info.Duration;
            row.YouTubeAutoDisable = null;
            row.YouTubeDate = info.PublishedAt;
            politiciansUpdated++;
          }
        }

        Politicians.UpdateTable(table, PoliticiansTable.ColumnSet.YouTubeRefresh);

        message =
          $"{table.Count} Expired YouTube Politicians found, {politiciansUpdated} YouTube Politicians updated";

      }
      catch (Exception ex)
      {
        VotePage.LogException("RefreshYouTubePoliticians", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("RefreshYouTubePoliticians", message);
    }
  }
}