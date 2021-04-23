using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Xml;
using DB.Vote;
using static System.String;

namespace Vote
{
  public class YouTubeVideoInfo : VideoInfo
  {
    public const string InvalidVideoUrlMessage = "Invalid YouTube URL";
    public const string VideoIdNotFoundMessage = "Video Id not found on YouTube";
    public const string VideoNotPublicMessage = "YouTube video is not publically viewable";
    public const string PlaylistIdNotFoundMessage = "Playlist Id not found on YouTube";

    public const string PlaylistNotPublicMessage =
      "YouTube playlist is not publically viewable";

    public const string ChannelIdNotFoundMessage = "Channel Id not found on YouTube";

    public const string ChannelNotPublicMessage =
      "YouTube channel is not publically viewable";

    public const string VideoUploadedByCandidateMessage = "Uploaded by candidate";

    public YouTubeVideoInfo()
    {
    }

    public YouTubeVideoInfo(string id) : base(id)
    {
    }

    public YouTubeVideoInfo(string id, string title, string description,
      DateTime publishedAt, TimeSpan duration, bool isPublic) : base(id, title, description,
      publishedAt, duration, isPublic)
    {
    }
  }

  public static class YouTubeVideoUtility
  {
    // Extension methods

    public static bool IsValidYouTubeChannelUrl(this string str)
    {
      return YouTubeChannelIdRegex.IsMatch(str);
    }

    public static bool IsValidYouTubeCustomChannelUrl(this string str)
    {
      return YouTubeCustomChannelIdRegex.IsMatch(str);
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

    private static readonly Regex YouTubeChannelIdRegex =
      new Regex(
        @"^(?:https?:\/\/)?(?:www\.)?youtube\.com/channel/(?<id>(?:\w|-)+)(?:\S+)?$");

    private static readonly Regex YouTubeCustomChannelIdRegex =
      new Regex(@"^(?:https?:\/\/)?(?:www\.)?youtube\.com/c/(?<id>(?:\w|-)+)(?:\S+)?$");

    private static readonly Regex YouTubeUserChannelIdRegex =
        new Regex(
          @"^(?:https?:\/\/)?(?:www\.)?youtube\.com/(?:user/)?(?!channel/|c/)(?<id>(?:\w|-)+)(?:\S+)?$")
      ;

    private static readonly Regex YouTubeVideoIdRegex = new Regex(
        @"^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))(?<id>(?:\w|-)+(?!\w|-))(?:\S+)?$")
      ;

    private static readonly Regex YouTubePlaylistIdRegex =
        new Regex(
          @"^(?:https?:\/\/)?(?:www\.)?youtube\.com/playlist\?list=(?<id>(?:\w|-)+(?!\w|-))(?:\S+)?$")
      ;

    private static readonly Regex ChannelIdRegex1 =
      new Regex("data-channel-external-id=\"(?<id>[^\"]+)\"");

    private static readonly Regex ChannelIdRegex2 =
      new Regex(
        "<link rel=\"canonical\" href=\"https://www.youtube.com/channel/(?<id>[^\"]+)\">");

    private static readonly Regex ChannelIdRegex3 =
      new Regex(
        "<meta itemprop=\"channelId\" content=\"(?<id>[^\"]+)\">");

    private static string _ApiKey;

    private static string ApiKey => _ApiKey ??
      (_ApiKey = WebConfigurationManager.AppSettings["VoteYouTubeApiKey"]);

    private static string _WebProxyUrl;

    private static string WebProxyUrl => _WebProxyUrl ??
      (_WebProxyUrl = WebConfigurationManager.AppSettings["VoteWebProxyUrl"]);

    private const string ChannelIdUrlTemplate =
        "https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&part=snippet&fields=items(snippet/channelId)"
      ;

    private const string ChannelInfoUrlTemplate =
        //"https://www.googleapis.com/youtube/v3/channels?id={0}&key={1}&part=snippet,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,snippet/thumbnails,status/privacyStatus)"
        "https://www.googleapis.com/youtube/v3/channels?id={0}&key={1}&part=snippet,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,status/privacyStatus)"
      ;

    private const string PlaylistInfoUrlTemplate =
        "https://www.googleapis.com/youtube/v3/playlists?id={0}&key={1}&part=snippet,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,status/privacyStatus)"
      ;

    private const string VideoInfoUrlTemplate =
        "https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&part=snippet,contentDetails,status&fields=items(snippet/title,snippet/description,snippet/publishedAt,contentDetails/duration,status/privacyStatus)"
      ;

    private static VideoInfo DecodeYouTubeInfo(string youTubeId, string json)
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
        if (dictionary["contentDetails"] is Dictionary<string, object> contentDetails &&
          contentDetails.ContainsKey("duration"))
          duration = XmlConvert.ToTimeSpan(contentDetails["duration"] as string);
      }
      var status = dictionary?["status"] as Dictionary<string, object>;
      var privacyStatus = status?["privacyStatus"] as string;

      return new YouTubeVideoInfo(youTubeId, title, description,
        DateTime.Parse(publishedAt).Date, duration, privacyStatus != "private");
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

    public static VideoInfo GetChannelInfo(string youTubeId, bool useProxy,
      int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = Format(ChannelInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
      // ReSharper disable once UnusedVariable
      catch (Exception)
      {
        if (retries <= 0)
          return new YouTubeVideoInfo(youTubeId);
      }
      return GetChannelInfo(youTubeId, useProxy, retries - 1);
    }

    public static VideoInfo GetPlaylistInfo(string youTubeId, bool useProxy,
      int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = Format(PlaylistInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeVideoInfo(youTubeId);
      }
      return GetPlaylistInfo(youTubeId, useProxy, retries - 1);
    }

    public static string GetVideoChannelId(string youTubeId, bool useProxy, int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = Format(ChannelIdUrlTemplate, youTubeId, ApiKey);
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

    public static VideoInfo GetVideoChannelInfo(string youTubeId, bool useProxy,
      int retries = 0)
    {
      try
      {
        var channelId = GetVideoChannelId(youTubeId, useProxy);
        if (IsNullOrWhiteSpace(channelId)) throw new Exception();
        return GetChannelInfo(channelId, useProxy);
      }
      catch
      {
        if (retries <= 0)
          return new YouTubeVideoInfo(youTubeId);
      }
      return GetVideoChannelInfo(youTubeId, useProxy, retries - 1);
    }

    public static VideoInfo GetVideoInfo(string youTubeId, bool useProxy,
      int retries = 0)
    {
      try
      {
        var client = new WebClient();
        var url = Format(VideoInfoUrlTemplate, youTubeId, ApiKey);
        if (useProxy) url = WebProxyUrl + "?" + HttpUtility.UrlEncode(url);
        //var json = client.DownloadString(url);
        var data = client.DownloadData(url);
        var json = System.Text.Encoding.UTF8.GetString(data);
        return DecodeYouTubeInfo(youTubeId, json);
      }
#pragma warning disable 168
      catch (Exception ex)
#pragma warning restore 168
      {
        if (retries <= 0)
          return new YouTubeVideoInfo(youTubeId);
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
        match = ChannelIdRegex3.Match(page);
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
      RefreshYouTubeAnswersNew();
      //return;

      //string message;
      //var expiration = new TimeSpan(3, 0, 0, 0); // 3 days
      //// fudge is to account for small differences in run time
      //var fudge = new TimeSpan(1, 0, 0); // 1 hour
      //var now = DateTime.UtcNow;

      //try
      //{
      //  VotePage.LogInfo("RefreshYouTubeAnswers", "Started");

      //  var answersUpdated = 0;
      //  var table = Answers.GetDataForYouTubeRefresh(now - expiration + fudge);

      //  foreach (var row in table)
      //  {
      //    row.YouTubeRefreshTime = now;

      //    var youTubeId = row.YouTubeUrl.GetYouTubeVideoId();
      //    if (IsNullOrWhiteSpace(youTubeId))
      //    {
      //      if (row.YouTubeAutoDisable != YouTubeVideoInfo.InvalidVideoUrlMessage)
      //      {
      //        row.YouTubeAutoDisable = YouTubeVideoInfo.InvalidVideoUrlMessage;
      //        answersUpdated++;
      //      }

      //      continue;
      //    }

      //    var videoInfo = GetVideoInfo(youTubeId, false, 1);
      //    if (!videoInfo.IsValid)
      //    {
      //      if (row.YouTubeAutoDisable != YouTubeVideoInfo.VideoIdNotFoundMessage)
      //      {
      //        row.YouTubeAutoDisable = YouTubeVideoInfo.VideoIdNotFoundMessage;
      //        answersUpdated++;
      //      }

      //      continue;
      //    }

      //    if (!videoInfo.IsPublic)
      //    {
      //      if (row.YouTubeAutoDisable != YouTubeVideoInfo.VideoNotPublicMessage)
      //      {
      //        row.YouTubeAutoDisable = YouTubeVideoInfo.VideoNotPublicMessage;
      //        answersUpdated++;
      //      }

      //      continue;
      //    }

      //    var description = videoInfo.Description.Trim();
      //    if (IsNullOrWhiteSpace(description) ||
      //      description.Length > VideoInfo.MaxVideoDescriptionLength)
      //      description = videoInfo.Title.Trim();

      //    if (description != row.YouTubeDescription ||
      //      videoInfo.Duration != row.YouTubeRunningTime ||
      //      row.YouTubeAutoDisable != null ||
      //      row.YouTubeSource == YouTubeVideoInfo.VideoUploadedByCandidateMessage &&
      //      videoInfo.PublishedAt != row.YouTubeDate)
      //    {
      //      row.YouTubeDescription = description;
      //      row.YouTubeRunningTime = videoInfo.Duration;
      //      row.YouTubeAutoDisable = null;
      //      if (row.YouTubeSource == YouTubeVideoInfo.VideoUploadedByCandidateMessage)
      //        row.YouTubeDate = videoInfo.PublishedAt;
      //      answersUpdated++;
      //    }
      //  }

      //  Answers.UpdateTable(table);

      //  message =
      //    $"{table.Count} Expired YouTube answers found, {answersUpdated} YouTube Answers updated";
      //}
      //catch (Exception ex)
      //{
      //  VotePage.LogException("RefreshYouTubeAnswers", ex);
      //  message = $"Exception: {ex.Message} [see exception log for details]";
      //}

      //VotePage.LogInfo("RefreshYouTubeAnswers", message);
    }

    public static void RefreshYouTubeAnswersNew()
    {
      string message;
      //var expiration = new TimeSpan(3, 0, 0, 0); // 3 days
      // fudge is to account for small differences in run time
      //var fudge = new TimeSpan(1, 0, 0); // 1 hour
      var now = DateTime.UtcNow;

      try
      {
        VotePage.LogInfo("RefreshYouTubeAnswersNew", "Started");

        var answersUpdated = 0;
        //var table = Answers.GetDataForYouTubeRefreshNew(now - expiration + fudge);
        var table = Answers.GetDataForYouTubeRefreshNew2(450);

        foreach (var row in table)
        {
          row.YouTubeRefreshTime = now;

          var youTubeId = row.YouTubeUrl.GetYouTubeVideoId();
          if (IsNullOrWhiteSpace(youTubeId))
          {
            if (row.YouTubeAutoDisable != YouTubeVideoInfo.InvalidVideoUrlMessage)
            {
              row.YouTubeAutoDisable = YouTubeVideoInfo.InvalidVideoUrlMessage;
              answersUpdated++;
            }
            continue;
          }

          var videoInfo = GetVideoInfo(youTubeId, false, 1);
          if (!videoInfo.IsValid)
          {
            if (row.YouTubeAutoDisable != YouTubeVideoInfo.VideoIdNotFoundMessage)
            {
              row.YouTubeAutoDisable = YouTubeVideoInfo.VideoIdNotFoundMessage;
              answersUpdated++;
            }
            continue;
          }

          if (!videoInfo.IsPublic)
          {
            if (row.YouTubeAutoDisable != YouTubeVideoInfo.VideoNotPublicMessage)
            {
              row.YouTubeAutoDisable = YouTubeVideoInfo.VideoNotPublicMessage;
              answersUpdated++;
            }
            continue;
          }

          var description = videoInfo.Description.Trim();
          if (IsNullOrWhiteSpace(description) ||
            description.Length > VideoInfo.MaxVideoDescriptionLength)
            description = videoInfo.Title.Trim();

          if (description != row.YouTubeDescription ||
            videoInfo.Duration != row.YouTubeRunningTime ||
            row.YouTubeAutoDisable != null ||
            row.YouTubeSource == YouTubeVideoInfo.VideoUploadedByCandidateMessage &&
            videoInfo.PublishedAt != row.YouTubeDate)
          {
            row.YouTubeDescription = description;
            row.YouTubeRunningTime = videoInfo.Duration;
            row.YouTubeAutoDisable = null;
            if (row.YouTubeSource == YouTubeVideoInfo.VideoUploadedByCandidateMessage)
              row.YouTubeDate = videoInfo.PublishedAt;
            answersUpdated++;
          }
        }

        Answers2.UpdateTable(table);

        message =
          $"{table.Count} Expired YouTube answers found, {answersUpdated} YouTube Answers updated";
      }
      catch (Exception ex)
      {
        VotePage.LogException("RefreshYouTubeAnswersNew", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("RefreshYouTubeAnswersNew", message);
    }

    public static void RefreshYouTubePoliticians()
    {
      //// this is so we only do it every three days
      //if (DateTime.UtcNow.Day % 3 != 0) return;

      string message;

      try
      {
        VotePage.LogInfo("RefreshYouTubePoliticians", "Started");

        var politiciansUpdated = 0;
        //var table = Politicians.GetYouTubeRefreshData();
        var table = Politicians.GetYouTubeRefreshData2(450);
        var now = DateTime.UtcNow;

        foreach (var row in table)
        {
          row.YouTubeRefreshDate = now;
          VideoInfo info = null;
          var url = row.YouTubeWebAddress;
          var videoType = Empty;
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
            if (!IsNullOrWhiteSpace(id))
              info = GetChannelInfo(id, false, 1);
            videoType = "channel";
          }

          var error = Empty;
          if (info == null || !info.IsValid)
          {
            switch (videoType)
            {
              case "video":
                error = YouTubeVideoInfo.VideoIdNotFoundMessage;
                break;

              case "playlist":
                error = YouTubeVideoInfo.PlaylistIdNotFoundMessage;
                break;

              case "channel":
                error = YouTubeVideoInfo.ChannelIdNotFoundMessage;
                break;

              default:
                error = YouTubeVideoInfo.InvalidVideoUrlMessage;
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
                error = YouTubeVideoInfo.VideoNotPublicMessage;
                break;

              case "playlist":
                error = YouTubeVideoInfo.PlaylistNotPublicMessage;
                break;

              case "channel":
                error = YouTubeVideoInfo.ChannelNotPublicMessage;
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
            info.PublishedAt != row.YouTubeDate || row.YouTubeAutoDisable != null)
          {
            row.YouTubeDescription = info.ShortDescription;
            row.YouTubeRunningTime = info.Duration;
            row.YouTubeAutoDisable = null;
            row.YouTubeDate = info.PublishedAt;
            row.YouTubeRefreshDate = now;
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