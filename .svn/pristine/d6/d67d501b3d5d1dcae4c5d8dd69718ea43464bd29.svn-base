using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote
{
  //internal class IssuePositions
  //{
  //  private readonly string _IssueKey;
  //  internal readonly string IssueDescription;

  //  private readonly List<IssuePolitician> _IssuePoliticians =
  //    new List<IssuePolitician>();

  //  internal readonly List<IssueQuestion> IssueQuestions = new List<IssueQuestion>();

  //  private IssuePositions(PageCache cache, string issueKey)
  //  {
  //    _IssueKey = issueKey;
  //    IssueDescription = cache.Issues.GetIssue(_IssueKey);
  //  }

  //  private void AddPolitician(PageCache cache, string politicianKey)
  //  {
  //    //string politicianLastName = Politicians.GetLastNameByPoliticianKey(politicianKey);
  //    var politicianLastName = cache.Politicians.GetLastName(politicianKey);
  //    var issuePolitician = new IssuePolitician
  //      {
  //        PoliticianKey = politicianKey,
  //        PoliticianLastName = politicianLastName
  //      };
  //    _IssuePoliticians.Add(issuePolitician);
  //  }

  //  internal PoliticianAnswer GetPoliticianAnswer(
  //    string questionKey, string politicianKey)
  //  {
  //    return IssueQuestions.Single(iq => iq.QuestionKey == questionKey)
  //      .PoliticianAnswers[politicianKey];
  //  }

  //  private void GetQuestions(PageCache cache)
  //  {
  //    //DataTable issueQuestionsTable = db.Table(sql.Questions4Issue(IssueKey));
  //    DataTable issueQuestionsTable =
  //      cache.Questions.GetNonOmittedDataByIssueKey(_IssueKey);
  //    foreach (DataRow issueQuestionsrow in issueQuestionsTable.Rows)
  //    {
  //      var questionKey = issueQuestionsrow["QuestionKey"].ToString();
  //      var questionDescription = issueQuestionsrow["Question"].ToString()
  //        .Trim();
  //      var question = new IssueQuestion
  //        {
  //          QuestionKey = questionKey,
  //          QuestionDescription = questionDescription
  //        };
  //      IssueQuestions.Add(question);

  //      foreach (
  //        var politicianKey in _IssuePoliticians.Select(ip => ip.PoliticianKey))
  //      {
  //        var politicianAnswer = new PoliticianAnswer();
  //        question.AddPolitician(politicianKey, politicianAnswer);
  //        var answersTable = Answers.GetDataByPoliticianKeyQuestionKey(
  //          politicianKey, questionKey);
  //        if (answersTable.Count > 0)
  //          politicianAnswer.Init(answersTable[0]);
  //      }
  //    }
  //  }

  //  //// for single politician
  //  //internal static IssuePositions GetIssuePositionsInfo(string politicianKey,
  //  //  string issueKey)
  //  //{
  //  //  return GetIssuePositionsInfo(PageCache.GetTemporary(), politicianKey, issueKey);
  //  //}

  //  internal static IssuePositions GetIssuePositionsInfo(
  //    PageCache cache, string politicianKey, string issueKey)
  //  {
  //    var keyList = new List<string> {politicianKey};
  //    return GetIssuePositionsInfo(cache, keyList, issueKey);
  //  }

  //  // for multiple politicians
  //  //internal static IssuePositions GetIssuePositionsInfo(
  //  //  IEnumerable<string> politicianKeys, string issueKey)
  //  //{
  //  //  return GetIssuePositionsInfo(null, null, null, politicianKeys, issueKey);
  //  //}

  //  internal static IssuePositions GetIssuePositionsInfo(
  //    PageCache cache, IEnumerable<string> politicianKeys, string issueKey)
  //  {
  //    var ip = new IssuePositions(cache, issueKey);
  //    foreach (var politicianKey in politicianKeys)
  //      ip.AddPolitician(cache, politicianKey);
  //    ip.GetQuestions(cache);

  //    return ip;
  //  }

  //  internal string GetPoliticianLastName(string politicianKey)
  //  {
  //    return _IssuePoliticians.Single(ip => ip.PoliticianKey == politicianKey)
  //      .PoliticianLastName;
  //  }

  //  // Only to be called when there is exactly one politician
  //  internal string PoliticianLastName
  //  {
  //    get
  //    {
  //      return _IssuePoliticians.Single()
  //        .PoliticianLastName;
  //    }
  //  }
  //}

  //internal class IssueQuestion
  //{
  //  internal string QuestionKey;
  //  internal string QuestionDescription;

  //  internal readonly Dictionary<string, PoliticianAnswer> PoliticianAnswers =
  //    new Dictionary<string, PoliticianAnswer>();

  //  internal void AddPolitician(
  //    string politicianKey, PoliticianAnswer politicianAnswer)
  //  {
  //    PoliticianAnswers.Add(politicianKey, politicianAnswer);
  //  }

  //  internal bool HasAnswer { get
  //  {
  //    return
  //      PoliticianAnswers.Values.Any(
  //        politicianAnswer => politicianAnswer.HasAnswer);
  //  } }

  //  // Only for when there is a single politician
  //  internal string GetAnswerHtml(string politicianLastName, bool answerOnly)
  //  {
  //    return PoliticianAnswers.Single()
  //      .Value.GetAnswerHtml(politicianLastName, answerOnly);
  //  }
  //}

  //internal class IssuePolitician
  //{
  //  internal string PoliticianKey;
  //  internal string PoliticianLastName;
  //}

  //internal class PoliticianAnswer
  //{
  //  private string _PoliticianKey;
  //  private string _Answer;
  //  private string _AnswerSource;
  //  private DateTime _AnswerDate;
  //  private Uri _YouTubeUri;
  //  private DateTime _YouTubeDate;

  //  internal string GetAnswerHtml(string politicianLastName, bool answerOnly)
  //  {
  //    const int maxLengthSource = 45;
  //    var result = string.Empty;
  //    if (!string.IsNullOrWhiteSpace(_Answer))
  //      if (!answerOnly)
  //      {
  //        result = db.Answer_Truncate_To_Max_Chars(_Answer);
  //        result = "<span class=\"last-name\">" + politicianLastName + ":</span> " +
  //          result;

  //        if (!string.IsNullOrWhiteSpace(_AnswerSource) ||
  //          _AnswerDate != VoteDb.DateTimeMin) // second line
  //          result += "<br />";

  //        // source
  //        if (!string.IsNullOrWhiteSpace(_AnswerSource))
  //        {
  //          var source = _AnswerSource;
  //          if (source.Length > maxLengthSource)
  //          {
  //            var slashLocation = source.IndexOf("/", StringComparison.Ordinal);
  //            source = slashLocation != -1
  //              ? source.Insert(slashLocation + 1, "<br>")
  //              : source.Insert(maxLengthSource, "<br>");
  //          }
  //          result += "<span class=\"source\"><span>Source:</span> " + source +
  //            "</span> ";
  //        }

  //        if (_AnswerDate != VoteDb.DateTimeMin)
  //          result += " (" + _AnswerDate.ToString("MM/dd/yyyy") + ")";
  //      }
  //      else
  //        result = _Answer;
  //    else // no answer
  //      if (!answerOnly && _YouTubeUri == null && _PoliticianKey != null)
  //        result = "<span class=\"last-name\">" + politicianLastName + ":</span> " +
  //          db.No_Response;

  //    if (_YouTubeUri != null)
  //    {
  //      if (!string.IsNullOrWhiteSpace(result))
  //        result += "<br />";
  //      var anchor = new HtmlAnchor
  //        {
  //          HRef = _YouTubeUri.ToString(),
  //          Target = "YouTube"
  //        };
  //      var image = new HtmlImage {Src = "/images/youTubeWide.jpg"};
  //      anchor.Controls.Add(image);
  //      result += db.RenderToString(anchor);
  //      if (_YouTubeDate != VoteDb.DateTimeMin)
  //        result += "<span class=\"youtubedate\">(" +
  //          _YouTubeDate.ToString("MM/dd/yyyy") + ")</span>";
  //    }

  //    return result;
  //  }

  //  internal bool HasAnswer { get { return !string.IsNullOrWhiteSpace(_Answer) || _YouTubeUri != null; } }

  //  internal void Init(AnswersRow answersRow)
  //  {
  //    _PoliticianKey = answersRow.PoliticianKey;
  //    _Answer = answersRow.Answer;
  //    _AnswerSource = answersRow.Source;
  //    _AnswerDate = answersRow.DateStamp;
  //    _YouTubeUri = answersRow.YouTubeUrl.ToUri();
  //    _YouTubeDate = answersRow.YouTubeDate;
  //  }
  //}
}