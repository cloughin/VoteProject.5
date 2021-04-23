using System;
using System.Linq;
using DB.Vote;
using Vote;
using static System.String;

namespace FixYouTubeUrls
{
  internal static class Program
  {
    private const string IssueKey = "ALLPersonal";
    private const string QuestionKey = "ALLPersonal440785"; // Why I'm Running

    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
      var withVideoUrls = Politicians.GetAllCacheData().Where(row =>
        !IsNullOrWhiteSpace(row.YouTubeWebAddress.GetYouTubeVideoId())).ToList();
      var count = 0;
      var duplicate = 0;
      foreach (var politician in withVideoUrls)
      {
        count = MoveVideoToAnswers(politician, count, ref duplicate);
      }
      Console.WriteLine("With Video URL: {0} -- Invalid: {1} -- Duplicate: {2}",
        withVideoUrls.Count, withVideoUrls.Count - count, duplicate);
      Console.Write("Press any key to exit");
      Console.ReadKey();
    }

    private static int MoveVideoToAnswers(PoliticiansRow politician, int count,
      ref int duplicate)
    {
      var youTubeId = politician.YouTubeWebAddress.GetYouTubeVideoId();
      VideoInfo videoInfo = null;
      if (!IsNullOrWhiteSpace(youTubeId))
        videoInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, false, 1);
      if (videoInfo != null && videoInfo.IsValid)
      {
        count++;

        // get Why I'm Running answers
        var answers =
          Answers.GetDataByPoliticianKeyQuestionKey(politician.PoliticianKey, QuestionKey);
        if (answers.All(row => row.YouTubeUrl.GetYouTubeVideoId() != youTubeId))
        {
          // doesn't exist -- add to first without a YouTubeUrl
          var rowToUpdate =
            answers.FirstOrDefault(row => IsNullOrWhiteSpace(row.YouTubeUrl));
          if (rowToUpdate == null)
          {
            // new to add a row
            {
              rowToUpdate = answers.NewRow(politician.PoliticianKey, QuestionKey,
                Answers.GetNextSequence(politician.PoliticianKey, QuestionKey),
                Politicians.GetStateCodeFromKey(politician.PoliticianKey), IssueKey, Empty,
                Empty, VotePage.DefaultDbDate, "curt", Empty, Empty, default, Empty, Empty,
                VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty, Empty, Empty,
                default, VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty);
              answers.AddRow(rowToUpdate);
            }
          }

          // update the row

          var description = videoInfo.Description.Trim();
          if (IsNullOrWhiteSpace(description) ||
            description.Length > VideoInfo.MaxVideoDescriptionLength)
            description = videoInfo.Title.Trim();
          rowToUpdate.YouTubeUrl = politician.YouTubeWebAddress;
          rowToUpdate.YouTubeDescription = description;
          rowToUpdate.YouTubeRunningTime = videoInfo.Duration;
          rowToUpdate.YouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
          rowToUpdate.YouTubeSourceUrl = Empty;
          rowToUpdate.YouTubeDate = videoInfo.PublishedAt;
          rowToUpdate.YouTubeRefreshTime = VotePage.DefaultDbDate;
          rowToUpdate.YouTubeAutoDisable = Empty;
        }
        else duplicate++;
        if (count % 100 == 0)
          Console.WriteLine("Processed {0}", count);
      }
      return count;
    }
  }
}