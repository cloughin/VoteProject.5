using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using Vote.Politician;

namespace Vote.Reports
{
  internal class ResponsiveIssuesReport : ResponsiveReport
  {
    #region Private

    private const int MoreMin = 250;
    private const int MoreMax = 350;
    private const int MoreMinForIntro = 750;
    private const int MoreMaxForIntro = 1000;
    private const int MoreMinForYouTube = 175;
    private const int MoreMaxForYouTube = 250;

    private const string SocialMediaYouTubeQuestionKey = "ALLPersonal440785";

    protected ResponsiveIssuesReport()
    {
      ReportContainer.AddCssClasses("issues-report");
    }

    //private static void EmbedVideo(Control container, string youTubeId, QuestionAndAnswer row,
    //  Control heading = null)
    //{
    //  if (!string.IsNullOrWhiteSpace(youTubeId))
    //  {
    //    var videoWrapper = new HtmlDiv().AddTo(container, "video-wrapper");
    //    var videoWrapperInner = new HtmlDiv().AddTo(videoWrapper, "video-wrapper-inner");
    //    var iframe = new HtmlGenericControl("iframe").AddTo(videoWrapperInner);
    //    iframe.Attributes.Add("width", "420");
    //    iframe.Attributes.Add("height", "236");
    //    iframe.Attributes.Add("src",
    //      "https://www.youtube.com/embed/" + youTubeId + "?rel=0&showinfo=0");
    //    iframe.Attributes.Add("frameborder", "0");
    //    iframe.Attributes.Add("allowfullscreen", "allowfullscreen");
    //  }
    //  if (heading != null) heading.AddTo(container);
    //  var description = FormatYouTubeDescription(row.YouTubeDescription, row.YouTubeRunningTime);
    //  var key = string.Format("{0}:{1}:{2}", row.PoliticianKey, row.QuestionKey, row.Sequence);
    //  VotePage.GetMorePart1(description, MoreMinForYouTube, MoreMaxForYouTube,
    //    "ytdesc", key).AddTo(container);
    //  if (!string.IsNullOrWhiteSpace(row.YouTubeSource) || row.YouTubeDate != VotePage.DefaultDbDate)
    //  {
    //    var sourceTag = new HtmlP().AddTo(container, "video-source");
    //    if (!string.IsNullOrWhiteSpace(row.YouTubeSource))
    //    {
    //      new HtmlSpan { InnerText = "Source: " }.AddTo(sourceTag);
    //      if (string.IsNullOrWhiteSpace(row.YouTubeSourceUrl))
    //        new LiteralControl(row.YouTubeSource).AddTo(sourceTag);
    //      else
    //        new HtmlAnchor
    //        {
    //          HRef = VotePage.NormalizeUrl(row.YouTubeSourceUrl),
    //          InnerHtml = row.YouTubeSource,
    //          Target = "view"
    //        }.AddTo(sourceTag);
    //      if (row.YouTubeDate != VotePage.DefaultDbDate)
    //        new LiteralControl(" ").AddTo(sourceTag);
    //    }
    //    if (row.YouTubeDate != VotePage.DefaultDbDate)
    //      new LiteralControl("(" + row.YouTubeDate.ToString("M/d/yyyy") + ")").AddTo(sourceTag);
    //  }
    //}

    private static void EmbedVideo2(Control container, string youTubeId, QuestionAndAnswer row,
      Control heading = null)
    {
      // uses fake YouTube player
      if (!string.IsNullOrWhiteSpace(youTubeId))
      {
        var videoWrapper = new HtmlDiv().AddTo(container, "youtube-container");
        new HtmlDiv().AddTo(videoWrapper, "youtube-player")
          .Attributes.Add("data-id", youTubeId);
        //var iframe = new HtmlGenericControl("iframe").AddTo(videoWrapperInner);
        //iframe.Attributes.Add("width", "420");
        //iframe.Attributes.Add("height", "236");
        //iframe.Attributes.Add("src",
        //  "https://www.youtube.com/embed/" + youTubeId + "?rel=0&showinfo=0");
        //iframe.Attributes.Add("frameborder", "0");
        //iframe.Attributes.Add("allowfullscreen", "allowfullscreen");
      }
      heading?.AddTo(container);
      var description = FormatYouTubeDescription(row.YouTubeDescription, row.YouTubeRunningTime);
      var key = $"{row.PoliticianKey}:{row.QuestionKey}:{row.Sequence}";
      VotePage.GetMorePart1(description, MoreMinForYouTube, MoreMaxForYouTube,
        "ytdesc", key).AddTo(container);
      if (!string.IsNullOrWhiteSpace(row.YouTubeSource) ||
        (row.YouTubeDate != VotePage.DefaultDbDate))
      {
        var sourceTag = new HtmlP().AddTo(container, "video-source");
        if (!string.IsNullOrWhiteSpace(row.YouTubeSource))
        {
          new HtmlSpan {InnerText = "Source: "}.AddTo(sourceTag);
          if (string.IsNullOrWhiteSpace(row.YouTubeSourceUrl))
            new LiteralControl(row.YouTubeSource).AddTo(sourceTag);
          else
            new HtmlAnchor
            {
              HRef = VotePage.NormalizeUrl(row.YouTubeSourceUrl),
              InnerHtml = row.YouTubeSource,
              Target = "view"
            }.AddTo(sourceTag);
          if (row.YouTubeDate != VotePage.DefaultDbDate)
            new LiteralControl(" ").AddTo(sourceTag);
        }
        if (row.YouTubeDate != VotePage.DefaultDbDate)
          new LiteralControl("(" + row.YouTubeDate.ToString("M/d/yyyy") + ")").AddTo(sourceTag);
      }
    }

    private static Control FormatAnswerForDisplay(QuestionAndAnswer qa, bool forIntro)
    {
      if ((qa == null) || (string.IsNullOrWhiteSpace(qa.Answer) &&
      (string.IsNullOrWhiteSpace(qa.YouTubeUrl) ||
        !string.IsNullOrWhiteSpace(qa.YouTubeAutoDisable))))
        return new LiteralControl("<p><em>Not available</em></p>");
      if (string.IsNullOrWhiteSpace(qa.Answer))
        return new PlaceHolder();
      var min = forIntro
        ? MoreMinForIntro
        : MoreMin;
      var max = forIntro
        ? MoreMaxForIntro
        : MoreMax;
      var key = $"{qa.PoliticianKey}:{qa.QuestionKey}:{qa.Sequence}";
      return VotePage.GetMorePart1(qa.Answer, min, max, "answer", key);
    }

    public static string FormatYouTubeDescription(string description, TimeSpan duration)
    {
      if (duration != default(TimeSpan))
        description = "<span class=\"duration\">[" + duration.FormatRunningTime() +
          "]</span> " + description;
      return description;
    }

    private static void ReportAnswers(Control container, DataRow candidate,
      IEnumerable<DataRow> qaRows,
      string questionKey, bool forIntro = false)
    {
      var qas = GetQuestionAndAnswerList(qaRows, candidate,
        questionKey.IsEqIgnoreCase(SocialMediaYouTubeQuestionKey));

      //// sort qas on date descending: AnswerDate if there's an answer else YouTubeDate
      //qas = qas.OrderByDescending(a => string.IsNullOrWhiteSpace(a.Answer())
      //  ? a.YouTubeDate()
      //  : a.AnswerDate()).ToList();
      // sort qas on date descending: YouTubeDate if there's a video else AnswerDate
      qas = qas.OrderByDescending(a => string.IsNullOrWhiteSpace(a.YouTubeUrl)
        ? a.AnswerDate
        : a.YouTubeDate).ToList();
      if (qas.Count == 0) qas.Add(null); // To force empty cell for running mate with no answers
      var content = new HtmlDiv().AddTo(container, "answer-cell");
      var inner = new HtmlDiv().AddTo(content, "answer-cell-inner");

      var firstAnswer = true;
      foreach (var qa in qas)
      {
        content = new HtmlDiv().AddTo(inner, "answer-cell-answer clearfix");
        var haveText = !string.IsNullOrWhiteSpace(qa?.Answer);
        var haveYouTube = !string.IsNullOrWhiteSpace(qa?.YouTubeUrl) &&
          string.IsNullOrWhiteSpace(qa.YouTubeAutoDisable);

        if (firstAnswer && (candidate != null) && !forIntro)
        {
          var nameContainer = new HtmlDiv().AddTo(content, "answer-name");
          FormatCandidateNameAndParty(nameContainer, candidate, false, false);

          var imageContainer = new HtmlDiv().AddTo(content, "answer-image");
          CreatePoliticianImageTag(candidate.PoliticianKey(), ImageSize100, false, string.Empty)
            .AddTo(imageContainer);
        }

        if (haveYouTube)
        {
          var youTube = new HtmlDiv().AddTo(content, "answer-youtube clearfix");
          var anchor = new HtmlAnchor
          {
            HRef = VotePage.NormalizeUrl(qa.YouTubeUrl),
            Target = "YouTube"
          }.AddTo(youTube, "yt-icon");
          new HtmlImage {Src = "/images/yt-icon.png"}.AddTo(anchor);

          var youTubeId = qa.YouTubeUrl.GetYouTubeVideoId();
          EmbedVideo2(youTube, youTubeId, qa);
        }

        if (haveText && haveYouTube)
          new HtmlHr().AddTo(content, "separator-rule");

        if (haveText || !haveYouTube)
        {
          var answerDiv = new HtmlDiv()
            .AddTo(content, "answer-answer");
          FormatAnswerForDisplay(qa, forIntro).AddTo(answerDiv);

          if ((qa != null) && (!string.IsNullOrWhiteSpace(qa.AnswerSource) ||
            (qa.AnswerDate != VotePage.DefaultDbDate)))
          {
            var p = new HtmlP().AddTo(content, "answer-source");
            if (!string.IsNullOrWhiteSpace(qa.AnswerSource))
            {
              if (VotePage.IsValidUrl(qa.AnswerSource))
              {
                var anchor = new HtmlAnchor
                {
                  HRef = VotePage.NormalizeUrl(qa.AnswerSource),
                  Target = "_blank"
                }.AddTo(p);
                new LiteralControl(Validation.StripWebProtocol(qa.AnswerSource)).AddTo(anchor);
              }
              else
                new LiteralControl( /*"Source: " +*/ qa.AnswerSource).AddTo(p);
            }
            if (qa.AnswerDate != VotePage.DefaultDbDate)
              new LiteralControl(" (" + qa.AnswerDate.ToString("MM/dd/yyyy") + ")").AddTo(p);
          }
        }

        firstAnswer = false;
      }
    }

    protected static IList<QuestionAndAnswer> GetQuestionAndAnswerList(IEnumerable<DataRow> rows,
      DataRow candidate, bool addSocialMediaVideo, bool addNonVideoPlaceholder = false)
    {
      var list = rows.Select(r => new QuestionAndAnswer
      {
        PoliticianKey = candidate.PoliticianKey(),
        IsRunningMate = candidate.ContainsColumn("IsRunningMate") && candidate.IsRunningMate(),
        QuestionKey = r.QuestionKey(),
        Issue = r.Issue(),
        Question = r.Question(),
        Sequence = r.Sequence(),
        Answer = r.Answer(),
        AnswerSource = r.AnswerSource(),
        AnswerDate = r.AnswerDate(),
        YouTubeUrl = r.YouTubeUrl(),
        YouTubeDate = r.YouTubeDate(VotePage.DefaultDbDate),
        YouTubeAutoDisable = r.YouTubeAutoDisable(),
        YouTubeSource = r.YouTubeSource(),
        YouTubeSourceUrl = r.YouTubeSourceUrl(),
        YouTubeDescription = r.YouTubeDescription(),
        YouTubeRunningTime = r.YouTubeRunningTime()
      }).ToList();

      // if the question key is SocialMediaYouTubeQuestionKey and the candidate YouTubeWebAddress
      // is a YouTobeVideo, then make sure it is included in the list.
      //
      //  Cases:
      //    1. It's already there -- don't add anything
      //    2. There is already an answer with no video -- insert it
      //    3. Create a new answer for it with sequence 0
      // 

      if (addSocialMediaVideo &&
        (addNonVideoPlaceholder || candidate.YouTubeWebAddress().IsValidYouTubeVideoUrl()) &&
        string.IsNullOrWhiteSpace(candidate.YouTubeAutoDisable()))
      {
        var url = candidate.YouTubeWebAddress();
        var videoId = url.GetYouTubeVideoId();
        if (videoId != null) // add in video
        {
          if (list.All(r => r.YouTubeUrl.GetYouTubeVideoId() != videoId))
          {
            var emptyAnswer = list.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.YouTubeUrl) &&
              r.QuestionKey.IsEqIgnoreCase(SocialMediaYouTubeQuestionKey));
            if (emptyAnswer == null) // create one and add it
            {
              emptyAnswer = new QuestionAndAnswer
              {
                Sequence = -1,
                PoliticianKey = candidate.PoliticianKey(),
                IsRunningMate =
                  candidate.ContainsColumn("IsRunningMate") && candidate.IsRunningMate(),
                QuestionKey = SocialMediaYouTubeQuestionKey,
                Issue = "Reasons & Objectives",
                Question = "Why I Am Running for Public Office",
                AnswerDate = VotePage.DefaultDbDate,
                YouTubeDate = VotePage.DefaultDbDate
              };
              list.Add(emptyAnswer);
            }
            emptyAnswer.YouTubeUrl = url;
            emptyAnswer.YouTubeSource = YouTubeInfo.VideoUploadedByCandidateMessage;
            emptyAnswer.YouTubeDescription = candidate.YouTubeDescription().SafeString();
            emptyAnswer.YouTubeRunningTime = candidate.YouTubeRunningTime();
            emptyAnswer.YouTubeDate = candidate.YouTubeDate();
          }
        }
        else if (addNonVideoPlaceholder &&
        (url.IsValidYouTubePlaylistUrl() || url.IsValidYouTubeChannelUrl() ||
          url.IsValidYouTubeCustomChannelUrl() || url.IsValidYouTubeUserChannelUrl()))
        {
          // to force a candidate heading when the only video entry is a socual media playlist or channel
          list.Add(new QuestionAndAnswer
          {
            PoliticianKey = candidate.PoliticianKey(),
            IsRunningMate = candidate.ContainsColumn("IsRunningMate") && candidate.IsRunningMate(),
            YouTubeUrl = "dummy"
          });
        }
      }

      return list;
    }

    private static void ReportVideoSummary(Control container, bool isRunningMateOffice,
      IList<DataRow> issuesDataRows, IList<DataRow> keys, bool forIntro)
    {
      // add in the Social Media Videos
      var issuesData = new List<QuestionAndAnswer>();
      foreach (var c in keys.Where(k => k != null))
      {
        var candidate = c;
        var candidateIssues =
          issuesDataRows.Where(r => r.PoliticianKey().IsEqIgnoreCase(candidate.PoliticianKey()));
        var qaList = GetQuestionAndAnswerList(candidateIssues, candidate, true, true);
        issuesData.AddRange(qaList);
      }

      var videoGroups = issuesData
        .Where(r => !r.IsRunningMate &&
          ((r.YouTubeUrl == "dummy") || !string.IsNullOrWhiteSpace(r.YouTubeUrl.GetYouTubeVideoId())) &&
          string.IsNullOrWhiteSpace(r.YouTubeAutoDisable))
        .GroupBy(r => r.PoliticianKey.ToUpper())
        .ToList();

      var runningMateGroups = issuesData
        .Where(r => isRunningMateOffice && r.IsRunningMate &&
          ((r.YouTubeUrl == "dummy") || !string.IsNullOrWhiteSpace(r.YouTubeUrl.GetYouTubeVideoId())) &&
          string.IsNullOrWhiteSpace(r.YouTubeAutoDisable))
        .GroupBy(r => r.PoliticianKey.ToUpper())
        .ToList();

      if (videoGroups.Any() || runningMateGroups.Any())
      {
        var title = "<span class=\"logo\"></span>";
        title += forIntro
          ? "All Videos"
          : "Videos by Candidate";
        new HtmlDiv {InnerHtml = title}.AddTo(container,
          "issue-header youtube-header accordion-header");
        var content = new HtmlDiv().AddTo(container, "youtube-content accordion-content");
        if (!forIntro) content.AddCssClasses("issues-content");
        foreach (var candidate in keys.Where(k => (k != null) &&
          (!k.ContainsColumn("IsRunningMate") || !k.IsRunningMate())))
        {
          var videos = videoGroups.FirstOrDefault(g => g.Key == candidate.PoliticianKey().ToUpper());
          var runningMateVideos =
            runningMateGroups.FirstOrDefault(
              g => g.Key == candidate.RunningMateKey().SafeString().ToUpper());
          if ((videos != null) || (runningMateVideos != null))
          {
            var name = Politicians.FormatName(candidate);
            var headerName = name;
            var runningMate = isRunningMateOffice
              ? keys.FirstOrDefault(
                k => k?.PoliticianKey().IsEqIgnoreCase(candidate.RunningMateKey()) == true)
              : null;
            if (runningMate != null)
            {
              var runningMateName = Politicians.FormatName(runningMate);
              headerName += " / " + runningMateName;
            }
            var videoContent = content;
            if (!forIntro)
            {
              // add candidate-level accordion
              new HtmlDiv {InnerHtml = headerName}.AddTo(content, "issue-header accordion-header");
              videoContent = new HtmlDiv().AddTo(content, "accordion-content");
            }
            var query = forIntro
              ? $"Content=INTROVIDEOS&Id={candidate.PoliticianKey()}"
              : $"Content=COMPAREVIDEOS&Election={issuesDataRows.First().ElectionKey()}&Office={issuesDataRows.First().OfficeKey()}&Id={candidate.PoliticianKey()}";
            videoContent.Attributes.Add("data-ajax", query);
          }
        }
      }
    }

    private static IEnumerable<DataRow> GetPoliticianAnswers(IEnumerable<DataRow> answers,
      string politicianKey)
    {
      return answers.Where(a => a.PoliticianKey().IsEqIgnoreCase(politicianKey))
        .OrderByDescending(a => a.AnswerDate());
    }

    private static void ReportOneIssue(Control container, string issue,
      IEnumerable<DataRow> qAndAs, IList<DataRow> candidates, bool forIntro = false)
    {
      new HtmlDiv {InnerHtml = issue}.AddTo(container, "issue-header accordion-header");
      var content = new HtmlDiv().AddTo(container, "issues-content accordion-content");
      var questions = qAndAs.GroupBy(qa => qa.QuestionKey());
      foreach (var question in questions)
      {
        var answers = question.ToList();
        var q = answers.First();
        new HtmlDiv {InnerHtml = q.Question()}.AddTo(content, "question-header accordion-header");
        var answerContainer = new HtmlDiv().AddTo(content, "question-content accordion-content");
        var query = forIntro
          ? $"Content=INTRO&Id={candidates.First().PoliticianKey()}&Question={q.QuestionKey()}"
          : $"Content=COMPARE&Election={q.ElectionKey()}&Office={q.OfficeKey()}&Question={q.QuestionKey()}";
        answerContainer.Attributes.Add("data-ajax", query);
      }
    }

    #endregion

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected static void CreateOneAnswerContent(Control container, IList<DataRow> candidates,
      IList<DataRow> answers, bool isRunningMateOffice, string questionKey, bool forIntro)
    {
      foreach (var candidate in candidates.Where(k => (k != null) &&
        (!k.ContainsColumn("IsRunningMate") || !k.IsRunningMate())))
      {
        var c = candidate;
        var cont = container;
        if (isRunningMateOffice)
          cont = new HtmlDiv().AddTo(cont, "answer-ticket");
        ReportAnswers(cont, c, GetPoliticianAnswers(answers, c.PoliticianKey()).ToList(),
          questionKey, forIntro);
        if (isRunningMateOffice)
        {
          var runningMate =
            candidates.FirstOrDefault(
              k => k?.PoliticianKey().IsEqIgnoreCase(c.RunningMateKey()) == true);
          ReportAnswers(cont, runningMate,
            GetPoliticianAnswers(answers, c.RunningMateKey()).ToList(),
            questionKey, forIntro);
        }
      }
    }

    protected void ReportIssues(List<DataRow> candidates, IList<DataRow> issuesData,
      bool isRunningMateOffice = false, int candidateCount = 0, bool forIntro = false)
    {
      var issues = issuesData.GroupBy(r => r.IssueKey()).ToList();
      if (candidateCount == 0) candidateCount = candidates.Count;
      var issuesContainer = new HtmlDiv()
        .AddTo(ReportContainer, "issues-container candidates-" + candidateCount);
      if (!issues.Any())
      {
        var messageContainer = new HtmlDiv().AddTo(ReportContainer,
          "issues-message") as HtmlGenericControl;
        var appendText = forIntro ? "" : " for any of the candidates";
        // ReSharper disable once PossibleNullReferenceException
        messageContainer.InnerHtml = "We have no biographical information or issue responses" +
          appendText + ".";
      }
      else
      {
        ReportVideoSummary(issuesContainer, isRunningMateOffice, issuesData, candidates, forIntro);
        foreach (var issue in issues)
          ReportOneIssue(issuesContainer, issue.First().Issue(), issue,
            candidates, forIntro);
      }
    }

    protected static void ReportCandidateVideos(Control content, DataRow candidate,
      IEnumerable<QuestionAndAnswer> videos, string name, bool isRunningMateOffice)
    {
      // We group by YouTubeId, so we don't present duplicate videos. It is used in the 
      // video aggregation sections.
      if (isRunningMateOffice)
        new HtmlDiv {InnerText = name}.AddTo(content, "answer-name");
      var uniqueVideos = videos.Where(v => v.YouTubeUrl != "dummy")
        .GroupBy(v => v.YouTubeUrl.GetYouTubeVideoId());

      // add a link for a channel or playlist
      if (!string.IsNullOrWhiteSpace(candidate.YouTubeWebAddress()) &&
        !candidate.YouTubeWebAddress().IsValidYouTubeVideoUrl())
      {
        var linkDesc = "&#x25ba " + name +
        (candidate.YouTubeWebAddress().IsValidYouTubePlaylistUrl()
          ? "'s YouTube playlist"
          : "'s YouTube channel");
        var channelDiv = new HtmlDiv().AddTo(content, "channel-link");
        new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(candidate.YouTubeWebAddress()),
          InnerHtml = linkDesc,
          Target = "view"
        }.AddTo(channelDiv);
      }

      foreach (var videoGroup in uniqueVideos.OrderByDescending(g => g.First().YouTubeDate))
      {
        var video = videoGroup.First();
        var issuesInVideo = videoGroup.GroupBy(vg => vg.Issue);
        // for each issue we create a heading line
        var headings = new PlaceHolder();
        foreach (var issueInVideo in issuesInVideo)
        {
          var first = issueInVideo.First();
          string question;
          if (issueInVideo.Count() > 1)
            question = "multiple topics";
          else
            question = UpdateIntroPage.AlternateTabLabels.ContainsKey(first.QuestionKey)
              ? UpdateIntroPage.AlternateTabLabels[first.QuestionKey].ReplaceBreakTagsWithSpaces()
              : first.Question;
          new HtmlP {InnerText = first.Issue + ": " + question}.AddTo(headings, "heading");
        }
        var outerWrapper = new HtmlDiv().AddTo(content, "video-wrapper-outer answer-youtube");
        EmbedVideo2(outerWrapper, videoGroup.Key, video, headings);
      }
    }

    protected static void ReportCandidateVideos(Control content, DataRow candidate,
      IEnumerable<DataRow> videoRows, string name, bool isRunningMateOffice)
    {
      var videos = GetQuestionAndAnswerList(videoRows, candidate, true);
      ReportCandidateVideos(content, candidate, videos, name, isRunningMateOffice);
    }

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }

  public sealed class QuestionAndAnswer
  {
    public string PoliticianKey;
    public bool IsRunningMate;
    public string QuestionKey;
    public string Issue;
    public string Question;
    public int Sequence;
    public string Answer;
    public string AnswerSource;
    public DateTime AnswerDate = VotePage.DefaultDbDate;
    public string YouTubeUrl;
    public DateTime YouTubeDate;
    public string YouTubeAutoDisable;
    public string YouTubeSource;
    public string YouTubeSourceUrl;
    public string YouTubeDescription;
    public TimeSpan YouTubeRunningTime;
  }
}