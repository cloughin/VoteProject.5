using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public class ResponsiveIssuesReport : ResponsiveReport
  {
    #region Private

    private const int MoreMin = 250;
    private const int MoreMax = 350;
    private const int MoreMinForIntro = 750;
    private const int MoreMaxForIntro = 1000;
    private const int MoreMinForVideo = 175;
    private const int MoreMaxForVideo = 250;

    private const string SocialMediaYouTubeQuestionKey = "ALLPersonal440785";

    protected ResponsiveIssuesReport()
    {
      ReportContainer.AddCssClasses("issues-report");
      ReportContainer.ID = "new-accordions";
      ReportContainer.ClientIDMode = ClientIDMode.Static;
    }

    private static void EmbedFacebookVideo(Control container, string facebookVideoId, QuestionAndAnswer row,
      Control heading = null)
    {
      // uses fake Facebook player
      if (!IsNullOrWhiteSpace(facebookVideoId))
      {
        var videoWrapper = new HtmlDiv().AddTo(container, "video-container facebookvideo-container");
        var videoPlayer = new HtmlDiv().AddTo(videoWrapper, "video-player facebookvideo-player");
        videoPlayer.Attributes.Add("data-type", "fb");
        videoPlayer.Attributes.Add("data-id", facebookVideoId);
      }
      heading?.AddTo(container);
      var description = FormatVideoDescription(row.FacebookVideoDescription, row.FacebookVideoRunningTime);
      var key = $"{row.PoliticianKey}:{row.QuestionKey}:{row.Sequence}";
      VotePage.GetMorePart1(description, MoreMinForVideo, MoreMaxForVideo,
        "fbdesc", key).AddTo(container);
      var sourceTag = new HtmlP().AddTo(container, "video-source");
      new HtmlSpan { InnerText = "Source: Uploaded by candidate " }.AddTo(sourceTag);
      if (!row.FacebookVideoDate.IsDefaultDate())
        new LiteralControl("(" + row.FacebookVideoDate.ToString("M/d/yyyy") + ")").AddTo(
          sourceTag);
    }

    private static void EmbedYouTube(Control container, string youTubeId, QuestionAndAnswer row,
      Control heading = null)
    {
      // uses fake YouTube player
      if (!IsNullOrWhiteSpace(youTubeId))
      {
        var videoWrapper = new HtmlDiv().AddTo(container, "video-container youtube-container");
        var videoPlayer = new HtmlDiv().AddTo(videoWrapper, "video-player youtube-player");
        videoPlayer.Attributes.Add("data-type", "yt");
        videoPlayer.Attributes.Add("data-id", youTubeId);
      }
      heading?.AddTo(container);
      var description = FormatVideoDescription(row.YouTubeDescription, row.YouTubeRunningTime);
      var key = $"{row.PoliticianKey}:{row.QuestionKey}:{row.Sequence}";
      VotePage.GetMorePart1(description, MoreMinForVideo, MoreMaxForVideo,
        "ytdesc", key).AddTo(container);
      if (!IsNullOrWhiteSpace(row.YouTubeSource) || !row.YouTubeDate.IsDefaultDate())
      {
        var sourceTag = new HtmlP().AddTo(container, "video-source");
        if (!IsNullOrWhiteSpace(row.YouTubeSource))
        {
          new HtmlSpan { InnerText = "Source: " }.AddTo(sourceTag);
          if (IsNullOrWhiteSpace(row.YouTubeSourceUrl))
            new LiteralControl(row.YouTubeSource).AddTo(sourceTag);
          else
            new HtmlAnchor
            {
              HRef = VotePage.NormalizeUrl(row.YouTubeSourceUrl),
              InnerHtml = row.YouTubeSource,
              Target = "view"
            }.AddTo(sourceTag);
          if (!row.YouTubeDate.IsDefaultDate())
            new LiteralControl(" ").AddTo(sourceTag);
        }
        if (!row.YouTubeDate.IsDefaultDate())
          new LiteralControl("(" + row.YouTubeDate.ToString("M/d/yyyy") + ")").AddTo(sourceTag);
      }
    }

    private static Control FormatAnswerForDisplay(QuestionAndAnswer qa, bool forIntro, bool haveOldAnswers)
    {
      if (qa == null || IsNullOrWhiteSpace(qa.Answer) &&
      (IsNullOrWhiteSpace(qa.YouTubeUrl) ||
        !IsNullOrWhiteSpace(qa.YouTubeAutoDisable)) &&
        (IsNullOrWhiteSpace(qa.FacebookVideoUrl) ||
          !IsNullOrWhiteSpace(qa.FacebookVideoAutoDisable) ||
          !VotePage.AllowFacebookVideos))
        return new LiteralControl($"<p><em>{(haveOldAnswers ? "No recent responses available" : "No responses available")}</em></p>");
      if (IsNullOrWhiteSpace(qa.Answer))
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

    public static string FormatVideoDescription(string description, TimeSpan duration)
    {
      if (duration != default)
        description = "<span class=\"duration\">[" + duration.FormatRunningTime() +
          "]</span> " + description;
      return description;
    }

    public static List<QuestionAndAnswer> SplitOutVideos(IEnumerable<QuestionAndAnswer> qas)
    {
      var result = new List<QuestionAndAnswer>();
      foreach (var qa in qas)
      {
        if (!IsNullOrWhiteSpace(qa.Answer) && !IsNullOrWhiteSpace(qa.YouTubeUrl))
        {
          result.Add(new QuestionAndAnswer
          {
            PoliticianKey = qa.PoliticianKey,
            IsRunningMate = qa.IsRunningMate,
            QuestionKey = qa.QuestionKey,
            Heading = qa.Heading,
            Issue = qa.Issue,
            Question = qa.Question,
            Sequence = qa.Sequence,
            Answer = qa.Answer,
            AnswerSource = qa.AnswerSource,
            AnswerDate = qa.AnswerDate,
            YouTubeUrl = Empty,
            YouTubeDate = VotePage.DefaultDbDate,
            YouTubeAutoDisable = null,
            YouTubeSource = Empty,
            YouTubeSourceUrl = Empty,
            YouTubeDescription = null,
            YouTubeRunningTime = default
          });
          result.Add(new QuestionAndAnswer
          {
            PoliticianKey = qa.PoliticianKey,
            IsRunningMate = qa.IsRunningMate,
            QuestionKey = qa.QuestionKey,
            Heading = qa.Heading,
            Issue = qa.Issue,
            Question = qa.Question,
            Sequence = qa.Sequence,
            Answer = Empty,
            AnswerSource = Empty,
            AnswerDate = VotePage.DefaultDbDate,
            YouTubeUrl = qa.YouTubeUrl,
            YouTubeDate = qa.YouTubeDate,
            YouTubeAutoDisable = qa.YouTubeAutoDisable,
            YouTubeSource = qa.YouTubeSource,
            YouTubeSourceUrl = qa.YouTubeSourceUrl,
            YouTubeDescription = qa.YouTubeDescription,
            YouTubeRunningTime = qa.YouTubeRunningTime
          });
        }
        else
          result.Add(qa);
      }

      return result;
    }

    private static void ReportAnswers(Control container, DataRow candidate,
      IEnumerable<DataRow> qaRows, string questionKey, bool forIntro, 
      DateTime oldAnswerCutoff)
    {
      var qas = GetQuestionAndAnswerList(qaRows, candidate,
          questionKey == Issues.QuestionId.WhyIAmRunning.ToInt().ToString());

      qas = SplitOutVideos(qas);

      qas = qas.OrderByDescending(a => a.ResponseDate).ToList();
      var content = new HtmlDiv().AddTo(container, "answer-cell");
      var inner = new HtmlDiv().AddTo(content, "answer-cell-inner");

      var firstAnswer = true;
      //if (UrlManager.IsStaging)
      {
        // separate into two groups -- new and old
        var newqas = qas.Where(qa => qa.ResponseDate > oldAnswerCutoff).ToList();
        var oldqas = qas.Where(qa => qa.ResponseDate <= oldAnswerCutoff).ToList();

        if (newqas.Count == 0) newqas.Add(null); // To force empty cell for no answers
        foreach (var qa in newqas)
          ReportOneAnswer(qa, candidate, forIntro, inner, ref firstAnswer, oldqas.Count > 0);
        if (oldqas.Count > 0)
        {
          new HtmlP { InnerText = "Show older answers" }.AddTo(inner, "show-older-answers");
          var oldContainer = new HtmlDiv().AddTo(inner, "old-answers hidden");
          foreach (var qa in oldqas)
            ReportOneAnswer(qa, candidate, forIntro, oldContainer, ref firstAnswer);
        }
      }
      //else
      //{
      //  if (qas.Count == 0) qas.Add(null); // To force empty cell for running mate with no answers
      //  foreach (var qa in qas)
      //    ReportOneAnswer(qa, candidate, forIntro, inner, ref firstAnswer);
      //}
    }

    private static void ReportOneAnswer(QuestionAndAnswer qa, DataRow candidate,
      bool forIntro, Control inner, ref bool firstAnswer, bool haveOldAnswers = false)
    {
      var content = new HtmlDiv().AddTo(inner, "answer-cell-answer clearfix");
      var haveText = !IsNullOrWhiteSpace(qa?.Answer);
      var haveYouTube = !IsNullOrWhiteSpace(qa?.YouTubeUrl) &&
        IsNullOrWhiteSpace(qa.YouTubeAutoDisable);
      var haveFacebookVideo = !IsNullOrWhiteSpace(qa?.FacebookVideoUrl) &&
        IsNullOrWhiteSpace(qa.FacebookVideoAutoDisable) &&
        VotePage.AllowFacebookVideos;

      if (firstAnswer && candidate != null && !forIntro)
      {
        var nameContainer = new HtmlDiv().AddTo(content, "answer-name");
        //FormatCandidateNameAndParty(nameContainer, candidate, false, false);
        CreatePoliticianIntroAnchor(candidate).AddTo(nameContainer);
        var imageContainer = new HtmlDiv().AddTo(content, "answer-image");
        //CreatePoliticianImageTag(candidate.PoliticianKey(), ImageSize100, false, Empty)
        //  .AddTo(imageContainer);
        CreatePoliticianImageAnchor(UrlManager.GetIntroPageUri(candidate.PoliticianKey())
            .ToString(), candidate.PoliticianKey(), ImageSize100,
          Politicians.FormatName(candidate) +
          " biographical information and positions and views on the issues").AddTo(imageContainer);
      }

      if (haveYouTube)
      {
        var youTube = new HtmlDiv().AddTo(content, "answer-video answer-youtube clearfix");
        var anchor = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(qa.YouTubeUrl),
          Target = "Video"
        }.AddTo(youTube, "video-icon yt-icon");
        new HtmlImage { Src = "/images/yt-icon-new.png" }.AddTo(anchor);

        var youTubeId = qa.YouTubeUrl.GetYouTubeVideoId();
        EmbedYouTube(youTube, youTubeId, qa);

        if (haveText || haveFacebookVideo)
          new HtmlHr().AddTo(content, "separator-rule");
      }

      if (haveFacebookVideo)
      {
        var facebookVideo = new HtmlDiv().AddTo(content, "answer-video answer-facebookvideo clearfix");
        var anchor = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(qa.FacebookVideoUrl),
          Target = "Video"
        }.AddTo(facebookVideo, "video-icon fb-icon");
        new HtmlImage { Src = "/images/fb-video-icon.png" }.AddTo(anchor);

        var facebookVideoId = qa.FacebookVideoUrl.GetFacebookVideoId();
        EmbedFacebookVideo(facebookVideo, facebookVideoId, qa);

        if (haveText)
          new HtmlHr().AddTo(content, "separator-rule");
      }

      if (haveText || !haveYouTube && !haveFacebookVideo)
      {
        var answerDiv = new HtmlDiv()
          .AddTo(content, "answer-answer");
        FormatAnswerForDisplay(qa, forIntro, haveOldAnswers).AddTo(answerDiv);

        if (qa != null && (!IsNullOrWhiteSpace(qa.AnswerSource) ||
          !qa.AnswerDate.IsDefaultDate()))
        {
          var p = new HtmlP().AddTo(content, "answer-source");
          if (!IsNullOrWhiteSpace(qa.AnswerSource))
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
          if (!qa.AnswerDate.IsDefaultDate())
            new LiteralControl(" (" + qa.AnswerDate.ToString("MM/dd/yyyy") + ")").AddTo(p);
        }
      }

      firstAnswer = false;
      //return content;
    }

    public static IList<QuestionAndAnswer> GetQuestionAndAnswerList(IEnumerable<DataRow> rows,
      DataRow candidate, bool addSocialMediaVideo, bool addNonVideoPlaceholder = false)
    {
      var list = rows.Select(r => new QuestionAndAnswer
      {
        PoliticianKey = candidate.PoliticianKey(),
        IsRunningMate = candidate.ContainsColumn("IsRunningMate") && candidate.IsRunningMate(),
        QuestionKey = r.QuestionKey(),
        Heading = VotePage.EnableIssueGroups ? r.Heading() : Empty,
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
        YouTubeRunningTime = r.YouTubeRunningTime(),
        FacebookVideoUrl = r.FacebookVideoUrl(),
        FacebookVideoDate = r.FacebookVideoDate(VotePage.DefaultDbDate),
        FacebookVideoAutoDisable = r.FacebookVideoAutoDisable(),
        FacebookVideoDescription = r.FacebookVideoDescription(),
        FacebookVideoRunningTime = r.FacebookVideoRunningTime()
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
        IsNullOrWhiteSpace(candidate.YouTubeAutoDisable()))
      {
        var url = candidate.YouTubeWebAddress();
        var videoId = url.GetYouTubeVideoId();
        if (videoId != null) // add in video
        {
          if (list.All(r => r.YouTubeUrl.GetYouTubeVideoId() != videoId))
          {
            var emptyAnswer = list.FirstOrDefault(r => IsNullOrWhiteSpace(r.YouTubeUrl) &&
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
                Heading = "Reasons & Objectives",
                Issue = "Reasons & Objectives",
                Question = "Why I Am Running for Public Office",
                AnswerDate = VotePage.DefaultDbDate,
                YouTubeDate = VotePage.DefaultDbDate
              };
              list.Add(emptyAnswer);
            }
            emptyAnswer.YouTubeUrl = url;
            emptyAnswer.YouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
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
      IList<DataRow> issuesDataRows, IList<DataRow> keys, DateTime oldAnswerCutoff, bool forIntro)
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
          (r.YouTubeUrl == "dummy" || !IsNullOrWhiteSpace(r.YouTubeUrl.GetYouTubeVideoId())) &&
          IsNullOrWhiteSpace(r.YouTubeAutoDisable) && r.YouTubeDate >= oldAnswerCutoff)
        .GroupBy(r => r.PoliticianKey.ToUpper())
        .ToList();

      var runningMateGroups = issuesData
        .Where(r => isRunningMateOffice && r.IsRunningMate &&
          (r.YouTubeUrl == "dummy" || !IsNullOrWhiteSpace(r.YouTubeUrl.GetYouTubeVideoId())) &&
          IsNullOrWhiteSpace(r.YouTubeAutoDisable) && r.YouTubeDate >= oldAnswerCutoff)
        .GroupBy(r => r.PoliticianKey.ToUpper())
        .ToList();

      if (videoGroups.Any() || runningMateGroups.Any())
      {
        var title = "<span class=\"logo\"></span>";
        title += forIntro
          ? "All Videos"
          : "Videos by Candidate";
        new HtmlDiv {InnerHtml = title}.AddTo(container,
          "youtube-header accordion-header top");
        var content = new HtmlDiv().AddTo(container, "youtube-content accordion-content top");
        /*if (!forIntro) */content.AddCssClasses(forIntro ? "bottom" : "accordion-container");
        foreach (var candidate in keys.Where(k => k != null &&
          (!k.ContainsColumn("IsRunningMate") || !k.IsRunningMate())))
        {
          var videos = videoGroups.FirstOrDefault(g => g.Key == candidate.PoliticianKey().ToUpper());
          var runningMateVideos =
            runningMateGroups.FirstOrDefault(
              g => g.Key == candidate.RunningMateKey().SafeString().ToUpper());
          if (videos != null || runningMateVideos != null)
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
              new HtmlDiv {InnerHtml = headerName}.AddTo(content, "accordion-header accordion-header-2 bottom");
              videoContent = new HtmlDiv().AddTo(content, "accordion-content accordion-content-2 bottom");
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
      return  answers.Where(a => a.PoliticianKey().IsEqIgnoreCase(politicianKey))
        .GroupBy(a => a.Sequence()).Select(g => g.First())
        .OrderByDescending(a => a.AnswerDate());
    }

    private static void ReportOneIssue(Control container, string issue,
      IList<DataRow> qAndAs, IList<DataRow> candidates, DateTime oldAnswerCutoff, bool forIntro = false)
    {
      var content = container;
      int.TryParse(qAndAs.First().IssueKey(), out var issueId);
      if (!VotePage.EnableIssueGroups || issueId != Issues.IssueId.Biographical.ToInt() &&
        issueId != Issues.IssueId.Reasons.ToInt())
      {
        var header = new HtmlDiv {InnerHtml = issue}.AddTo(container, "accordion-header accordion-header-2");
        content = new HtmlDiv().AddTo(container, "accordion-container accordion-content accordion-content-2");
        if (!VotePage.EnableIssueGroups)
        {
          header.AddCssClasses("top");
          content.AddCssClasses("top");
        }
      }
      var questions = qAndAs.GroupBy(qa => qa.QuestionKey());
      foreach (var question in questions)
        if (question.Any(r => r.ResponseDate() >= oldAnswerCutoff))
        {
          var answers = question.ToList();
          var q = answers.First();
          new HtmlDiv {InnerHtml = q.Question()}.AddTo(content, "accordion-header accordion-header-3 bottom indent");
          var answerContainer = new HtmlDiv().AddTo(content, "accordion-content accordion-content-3 bottom");
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

    protected void CreateIssueListLink(string officeKey)
    {
      var issueListLink = new HtmlDiv().AddTo(ReportContainer, "issue-list-link");
      var (stateCode, countyCode, localKey, _) = Offices.GetIssuesCoding(officeKey);
      new HtmlAnchor
      {
        HRef = UrlManager.GetIssueListPageUri(stateCode, countyCode, localKey).ToString(),
        InnerText = "View a list of all questions available to the candidates"
      }.AddTo(issueListLink);
      new LiteralControl(
        ". Questions are included below only if there is at least one response.").AddTo(
        issueListLink);
    }

    protected static void CreateOneAnswerContent(Control container, IList<DataRow> candidates,
      IList<DataRow> answers, bool isRunningMateOffice, string questionKey, bool forIntro,
      DateTime oldAnswerCutoff)
    {
      foreach (var candidate in candidates.Where(k => k != null &&
        (!k.ContainsColumn("IsRunningMate") || !k.IsRunningMate())))
      {
        var c = candidate;
        var cont = container;
        if (isRunningMateOffice)
          cont = new HtmlDiv().AddTo(cont, "answer-ticket");
        ReportAnswers(cont, c, GetPoliticianAnswers(answers, c.PoliticianKey()).ToList(),
          questionKey, forIntro, oldAnswerCutoff);
        if (isRunningMateOffice)
        {
          var runningMate =
            candidates.FirstOrDefault(
              k => k?.PoliticianKey().IsEqIgnoreCase(c.RunningMateKey()) == true);
          ReportAnswers(cont, runningMate,
            GetPoliticianAnswers(answers, c.RunningMateKey()).ToList(),
            questionKey, forIntro, oldAnswerCutoff);
        }
      }
    }

    protected void ReportIssues(List<DataRow> candidates, IList<DataRow> issuesData,
      DateTime oldAnswerCutoff, bool isRunningMateOffice = false, int candidateCount = 0,
      bool forIntro = false)
    {
      var issues = issuesData.GroupBy(r => r.IssueKey()).ToList();
      if (candidateCount == 0) candidateCount = candidates.Count;
      var issuesContainer = new HtmlDiv()
        .AddTo(ReportContainer, "issues-container accordion-container candidates-" + candidateCount);

      //foreach (var x in issues)
      //{
      //  foreach (var y in x)
      //  {
      //    var a = y.ResponseDate() >= oldAnswerCutoff;
      //  }
      //  var any = x.Any(r2 => r2.ResponseDate() >= oldAnswerCutoff);
      //}

      if (!issues.Any(r => r.Any(r2 => r2.ResponseDate() >= oldAnswerCutoff)))
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
        ReportVideoSummary(issuesContainer, isRunningMateOffice, issuesData, candidates, oldAnswerCutoff, forIntro);
        foreach (var issue in issues)
          if (issue.Any(r => r.ResponseDate() >= oldAnswerCutoff))
            ReportOneIssue(issuesContainer, issue.First().Issue(), issue.ToList(),
              candidates, oldAnswerCutoff, forIntro);
      }
    }

    protected void ReportIssues3(List<DataRow> candidates, IList<DataRow> issuesData,
      bool isRunningMateOffice = false, int candidateCount = 0, bool forIntro = false)
    {
      var issueGroups = issuesData.GroupBy(r => r.IssueGroupId()).ToList();
      if (candidateCount == 0) candidateCount = candidates.Count;
      var issuesContainer = new HtmlDiv()
        .AddTo(ReportContainer, "issues-container accordion-container candidates-" + candidateCount);
      if (!issueGroups.Any())
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
        ReportVideoSummary(issuesContainer, isRunningMateOffice, issuesData, candidates, DateTime.MinValue, forIntro);
        foreach (var issueGroup in issueGroups)
        {
          var groupInfo = issueGroup.First();
          var groupHeader =
            new HtmlDiv().AddTo(issuesContainer, "accordion-header accordion-header-1 top");
          new HtmlDiv {InnerHtml = groupInfo.Heading()}.AddTo(groupHeader);
          if (!IsNullOrWhiteSpace(groupInfo.SubHeading()))
            new HtmlDiv {InnerHtml = groupInfo.SubHeading().ReplaceBreakTagsWithSpaces()}
              .AddTo(groupHeader, "sub-heading");
          var groupContent = new HtmlDiv().AddTo(issuesContainer, "accordion-content accordion-content=1 accordion-container top");
          ReportIssues3a(groupContent, candidates, issueGroup.ToList(),
            forIntro);
        }
      }
    }

    protected void ReportIssues3a(HtmlControl issuesContainer, List<DataRow> candidates, 
      IEnumerable<DataRow> issuesData, bool forIntro = false)
    {
      var issues = issuesData.GroupBy(r => r.IssueKey()).ToList();
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
        foreach (var issue in issues)
          ReportOneIssue(issuesContainer, issue.First().Issue(), issue.ToList(),
            candidates, DateTime.MinValue, forIntro);
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
      if (!IsNullOrWhiteSpace(candidate.YouTubeWebAddress()) &&
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
        var alternatePoliticianLabels = VotePage.AlternatePoliticianTabLabels2;
        var newQuestion = true;
        foreach (var issueInVideo in issuesInVideo)
        {
          var first = issueInVideo.First();
          string question;
          if (issueInVideo.Count() > 1)
            question = "multiple topics";
          else
            question = alternatePoliticianLabels.ContainsKey(first.QuestionKey)
              ? alternatePoliticianLabels[first.QuestionKey].ReplaceBreakTagsWithSpaces()
              : first.Question;
          if (VotePage.EnableIssueGroups)
          {
            if (newQuestion)
              new HtmlP { InnerText = question }.AddTo(headings, "heading");
            var sub = $"in {first.Heading}";
            if (first.Heading != first.Issue)
              sub += $": {first.Issue}";
            new HtmlP { InnerText = sub }.AddTo(headings, "heading sub-heading");
          }
          else
            new HtmlP {InnerText = first.Issue + ": " + question}.AddTo(headings, "heading");
          newQuestion = false;
        }
        var outerWrapper = new HtmlDiv().AddTo(content, "video-wrapper-outer answer-youtube");
        EmbedYouTube(outerWrapper, videoGroup.Key, video, headings);
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
    public string Heading;
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
    public string FacebookVideoUrl;
    public DateTime FacebookVideoDate;
    public string FacebookVideoAutoDisable;
    public string FacebookVideoDescription;
    public TimeSpan FacebookVideoRunningTime;

    public bool HasVideo
    {
      get { return !IsNullOrWhiteSpace(YouTubeUrl); }
    }

    public DateTime ResponseDate
    {
      get { return IsNullOrWhiteSpace(YouTubeUrl) ? AnswerDate : YouTubeDate; }
    }
  }
}