using Vote;

namespace TestGetYouTubeVideoInfo
{
  static class Program
  {
    static void Main(string[] args)
    {
      //var id = YouTubeVideoUtility.GetYouTubeVideoId("www.youtube.com/watch?v=WAWqjONi4TE");
      //var info = YouTubeVideoUtility.GetVideoInfo(id, false);
      var cid = YouTubeVideoUtility.LookupChannelId("www.youtube.com/user/senatormurkowski");
      var cinfo = YouTubeVideoUtility.GetChannelInfo(cid, false);
    }
  }
}
