using System;
using DB.Vote;
using System.IO;
using System.Configuration;
using Google.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using Google.GData.Client;
using System.Threading;

namespace Vote.Politician
{
  public partial class YouTubeUpload : SecurePoliticianPage
  {
    //private void HandleException(Exception ex)
    //{
    //  string message;

    //  Title = H1.InnerText = "YouTube Upload";

    //  try
    //  {
    //    message = db.Fail(ex.Message);
    //    // We don't log routine UI exceptions
    //    if (!(ex is VoteUIException))
    //      db.Log_Error_Admin(ex);
    //  }
    //  catch (Exception ex2)
    //  {
    //    message = "Unexpected failure in exception handler: " +
    //      ex2 + Environment.NewLine +
    //      "Original exception: " + ex;
    //  }

    //  Msg.Text = message;
    //}
    
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    try
    //    {
    //      // Get the PoliticianKey from the query string and validate it
    //      var politicianKey = QueryId;
    //      if (!Politicians.PoliticianKeyExists(politicianKey))
    //        throw new VoteUIException("Invalid PoliticianKey (Id) in query string");
    //      PoliticianName.Text = Politicians.GetFormattedName(politicianKey);

    //      // Get the QuestionKey from the query string and validate it
    //      var questionKey = QueryQuestion;
    //      if (!Questions.QuestionKeyExists(questionKey))
    //        throw new VoteUIException("Invalid QuestionKey in query string");
    //      QuestionDescription.Text = Questions.GetQuestionByQuestionKey(questionKey);
    //    }
    //    catch (Exception ex)
    //    {
    //      HandleException(ex);
    //    }
    //  }
    //}

    //protected void ButtonUploadFile_ServerClick(object sender, EventArgs e)
    //{
    //  string tempFileName = null;
    //  try
    //  {
    //    // Get the PoliticianKey from the query string and validate it
    //    var politicianKey = QueryId;
    //    if (!Politicians.PoliticianKeyExists(politicianKey))
    //      throw new VoteUIException("Invalid PoliticianKey (Id) in query string");

    //    // Get the QuestionKey from the query string and validate it
    //    var questionKey = QueryQuestion;
    //    if (!Questions.QuestionKeyExists(questionKey))
    //      throw new VoteUIException("Invalid QuestionKey in query string");

    //    // Make sure there is a title, and that it is 100 characters or less
    //    var videoTitle = YouTubeTitleTextBox.Text.Trim();
    //    if (string.IsNullOrEmpty(videoTitle))
    //      throw new VoteUIException("The YouTube Title is required");
    //    if (videoTitle.Length > 100)
    //      throw new VoteUIException(
    //        "The YouTube Title is {0} characters long. The maximum is 100 characters.",
    //        videoTitle.Length);

    //    // If there is a description, make sure that it is 2000 characters or less
    //    var videoDescription = YouTubeDescriptionTextBox.Text.Trim();
    //    if (videoDescription.Length > 2000)
    //      throw new VoteUIException(
    //        "The YouTube Description is {0} characters long. The maximum is 2000 characters.",
    //        videoDescription.Length);

    //    // If there is a date, parse and validate it. Otherwise use today.
    //    var youTubeDate = DateTime.Today;
    //    var youTubeDateString = YouTubeDateTextBox.Text.Trim();
    //    if (!string.IsNullOrWhiteSpace(youTubeDateString))
    //      if (DateTime.TryParse(youTubeDateString, out youTubeDate))
    //        youTubeDate = youTubeDate.Date;
    //      else
    //        throw new VoteUIException("The YouTube Date is invalid");

    //    // Make sure we have a file and save it under a temporary name
    //    var postedFile = Request.Files["UploadFile"];
    //    if (postedFile.ContentLength == 0)
    //      throw new VoteUIException("There is no file to upload");
    //    tempFileName = Path.GetTempFileName();
    //    postedFile.SaveAs(tempFileName);

    //    // Get the YouTube credentials from config
    //    var youTubeApplication = 
    //      ConfigurationManager.AppSettings["VoteYouTubeApplication"];
    //    var youTubeDeveloperKey =
    //      ConfigurationManager.AppSettings["VoteYouTubeDeveloperKey"];
    //    var youTubeUserName =
    //      ConfigurationManager.AppSettings["VoteYouTubeUserName"];
    //    var youTubePassword =
    //      ConfigurationManager.AppSettings["VoteYouTubePassword"];
    //    if (string.IsNullOrWhiteSpace(youTubeApplication) ||
    //      string.IsNullOrWhiteSpace(youTubeDeveloperKey) ||
    //      string.IsNullOrWhiteSpace(youTubeUserName) ||
    //      string.IsNullOrWhiteSpace(youTubePassword))
    //      throw new VoteUIException("The YouTube configuration settings are incomplete");

    //    // Create the YouTube request object
    //    var settings = new YouTubeRequestSettings(
    //      youTubeApplication, youTubeDeveloperKey, youTubeUserName, youTubePassword);
    //    var request = new YouTubeRequest(settings);

    //    // Create and initialize the YouTube Video object
    //    var newVideo = new Video {Title = videoTitle};
    //    newVideo.Tags.Add(new MediaCategory("News", YouTubeNameTable.CategorySchema));
    //    //newVideo.Keywords = "cars, funny";
    //    if (!string.IsNullOrEmpty(videoDescription))
    //      newVideo.Description = videoDescription;
    //    newVideo.YouTubeEntry.Private = true;
    //    newVideo.Tags.Add(new MediaCategory(string.Concat(politicianKey, ", ", questionKey),
    //      YouTubeNameTable.DeveloperTagSchema));
    //    newVideo.YouTubeEntry.MediaSource = new MediaFileSource(tempFileName,
    //      postedFile.ContentType);

    //    // Upload to YouTube
    //    var createdVideo = request.Upload(newVideo);

    //    // Check for any error
    //    var videoIsOk = false;
    //    var videoId = createdVideo.VideoId;
    //    while (!videoIsOk)
    //    {
    //      if (!createdVideo.IsDraft)
    //        videoIsOk = true;
    //      else
    //      {
    //        var stateName = createdVideo.Status.Name;
    //        if (stateName == "processing") // wait a bit
    //        {
    //          Thread.Sleep(new TimeSpan(0, 0, 2)); // 2 seconds
    //          // re-fetch the video
    //          var videoUri = 
    //            new Uri("http://gdata.youtube.com/feeds/api/videos/" + videoId);
    //          createdVideo = request.Retrieve<Video>(videoUri);
    //        }
    //        else if (stateName == "rejected")
    //          throw new VoteUIException("Video was rejected because {0}",
    //            createdVideo.Status.Value);
    //        else if (stateName == "failed")
    //          throw new VoteUIException("Video failed to upload because {0}",
    //            createdVideo.Status.Value);
    //        else
    //          throw new VoteUIException("Unknown video status {0}::{1}",
    //            stateName, createdVideo.Status.Value);
    //      }
    //    }

    //    // Create a database entries for the video
    //    var uploadDate = DateTime.Now;
    //    PoliticiansVideosData.Insert(
    //      politicianKey,
    //      questionKey,
    //      uploadDate,
    //      youTubeDate,
    //      videoTitle,
    //      videoId,
    //      UserName,
    //      false);

    //    // Blob storage is commented because they can be FRIGGIN HUGE!
    //    //string extension = Path.GetExtension(postedFile.FileName);
    //    //MemoryStream memoryStream = new MemoryStream();
    //    //postedFile.InputStream.Position = 0;
    //    //postedFile.InputStream.CopyTo(memoryStream);
    //    //byte[] blob = memoryStream.ToArray();
    //    //PoliticiansVideosBlobs.Insert(
    //    //  politicianKey,
    //    //  questionKey,
    //    //  uploadDate,
    //    //  blob,
    //    //  extension);

    //    // Report success
    //    Msg.Text = db.Ok("Video was successfully uploaded with Id=" + videoId +
    //      ". The video will not be accessible to the general public until we " +
    //      "have reviewed and approved it.");
    //  }
    //  catch (Exception ex)
    //  {
    //    HandleException(ex);
    //  }
    //  finally
    //  {
    //    if (!string.IsNullOrEmpty(tempFileName))
    //      File.Delete(tempFileName);
    //  }
    //}
  }
}