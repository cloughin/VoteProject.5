using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DB.Vote;
using UtilityLibrary;
using Vote;
using static System.String;

namespace PreprocessIssues
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      var form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (IsNullOrWhiteSpace(text)) return;
      text = $"{DateTime.Now:HH:mm:ss.fff} {Format(text, arguments)}";
      form.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked)
        appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveDb";
      else
        throw new VoteException("Invalid server");
      var connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    private Options AllOptions;
    private Dictionary<string, OldIssueMapItem> _OldIssueMapDictionary;
    private Dictionary<string, OldQuestionMapItem> _OldQuestionMapDictionary;
    private Dictionary<string, NewIssueMapItem> _NewIssueMapDictionary;
    private Dictionary<string, NewQuestionMapItem> _NewQuestionMapDictionary;
    private Dictionary<string, string> _StatesDictionary;

    // Ideally these should not have to be dynamic, but defining them local to 
    // BackgroundWorker_DoWork led to closure issues. There are several work arounds.
    // This seemed the least undesireable.
    private readonly Dictionary<string, dynamic> _OldIssuesDictionary =
      new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, dynamic> _NewIssuesDictionary =
      new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, dynamic> _OldQuestionsDictionary =
      new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, dynamic> _NewQuestionsDictionary =
      new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<int, object> _QuestionsJurisdictionsDictionary =
      new Dictionary<int, dynamic>();

    private readonly List<dynamic> _QuestionsJurisdictionsList = new List<dynamic>();

    private readonly Dictionary<dynamic, object> _DuplicateVideoDictionary =
      new Dictionary<dynamic, object>();

    private readonly Dictionary<dynamic, object> _DuplicateTextDictionary =
      new Dictionary<dynamic, object>();

    private void AnalyzeAnswersAndCreateCsv(AnswersTable answersData)
    {
      // Code all answers with new Issue and Question. Ignore any non-matches -- they should all be
      // ones with the omit option.
      var codedAnswers = answersData
        .Where(r =>
          _NewIssuesDictionary.ContainsKey(r.IssueKey) &&
          _NewQuestionsDictionary.ContainsKey(r.QuestionKey)).Select(r => new
        {
          OldIssueKey = r.IssueKey.ToUpperInvariant(),
          OldQuestionKey = r.QuestionKey.ToUpperInvariant(),
          NewIssue = _NewIssuesDictionary[r.IssueKey],
          NewQuestion = _NewQuestionsDictionary[r.QuestionKey],
          PoliticianKey = r.PoliticianKey.ToUpperInvariant(),
          r.Sequence,
          r.Answer,
          r.YouTubeUrl,
          r.DateStamp
        }).OrderByDescending(r => r.DateStamp).ToList();

      // find merged answers with multiple references to the same video
      var duplicateVideoAnswers = codedAnswers.Where(a => !IsNullOrWhiteSpace(a.YouTubeUrl))
        .GroupBy(r => new
        {
          r.NewIssue.Issue,
          r.NewQuestion.Question,
          r.PoliticianKey,
          YouTubeId = r.YouTubeUrl.GetYouTubeVideoId()
        }).Where(g => g.Count() > 1).ToList();

      // the old answers to have their YouTubeUrls cleared on final output -- 
      // only keep the most recent
      var duplicateVideoDictionary = duplicateVideoAnswers
        // queue all but the most recent for deletion
        .SelectMany(g => g.OrderByDescending(g2 => g2.DateStamp).Skip(1)).ToDictionary(a =>
          new
          {
            PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
            a.OldQuestionKey,
            a.Sequence
          });
      foreach (var kvp in duplicateVideoDictionary)
        _DuplicateVideoDictionary.Add(kvp.Key, null);

      // find questions with exact duplicate long text answers
      var groupedTextAnswers = codedAnswers
        .Where(a =>
          !IsNullOrWhiteSpace(a.Answer.Trim()) &&
          a.Answer.Trim().Length >= AllOptions.LongAnswerMinimumLength).GroupBy(r =>
          new
          {
            r.NewIssue.Issue,
            r.NewQuestion.Question,
            r.PoliticianKey,
            Answer = r.Answer.Trim()
          }).ToList();
      var duplicateTextAnswers = groupedTextAnswers.Where(g => g.Count() > 1).ToList();

      var duplicateTextAnswersList = duplicateTextAnswers.Select(d => d.Key.Answer)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(a => a.Length)
        .ThenBy(a => a).ToList();

      // the old answers to have their Answers cleared on final output
      var duplicateTextDictionary = duplicateTextAnswers
        // queue all but the most recent for deletion
        .SelectMany(g => g.OrderByDescending(g2 => g2.DateStamp).Skip(1)).ToDictionary(a =>
          new
          {
            PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
            a.OldQuestionKey,
            a.Sequence
          });
      foreach (var kvp in duplicateTextDictionary)
        _DuplicateTextDictionary.Add(kvp.Key, null);

      // find all merged questions with multiple distinct answers
      var multipleTextAnswerQuestions = groupedTextAnswers
        .Select(g => new
        {
          g.First().OldIssueKey,
          g.First().OldQuestionKey,
          g.First().Sequence,
          g.First().NewIssue,
          g.First().NewQuestion,
          g.First().PoliticianKey,
          g.Key.Answer,
          g.First().DateStamp,
          g.First().YouTubeUrl
        }).GroupBy(r => new {r.NewIssue.Issue, r.NewQuestion.Question, r.PoliticianKey})
        .Where(g => g.Count() > 1).ToList();

      // create CSV from multiple text Answers
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write info rows
        csvWriter.AddField(
          $"{_DuplicateVideoDictionary.Count} duplicate video answers for the same new question will be eliminated from the final output.");
        csvWriter.Write(streamWriter);
        csvWriter.AddField(
          $"{duplicateTextDictionary.Count} exact duplicate text answers for the same new question will be eliminated from the final output.");
        csvWriter.Write(streamWriter);

        // write headers
        csvWriter.AddField("New Issue");
        csvWriter.AddField("New Question");
        csvWriter.AddField("Politician Key");
        csvWriter.AddField("Answers");
        csvWriter.AddField("Old Issue");
        csvWriter.AddField("Old Question");
        csvWriter.AddField("Old Sequence");
        csvWriter.AddField("Date");
        csvWriter.AddField("HasVideo");
        csvWriter.Write(streamWriter);

        foreach (var multiples in multipleTextAnswerQuestions)
        {
          var first = multiples.First();

          // these three fields are for display -- they are blanked after the first row
          // in each set of multiples
          var newIssue = $"{first.NewIssue.Issue} (Id={first.NewIssue.Id})";
          var newQuestion = $"{first.NewQuestion.Question} (Id={first.NewQuestion.Id})";
          var politicianKey = multiples.Key.PoliticianKey;

          foreach (var answer in multiples)
          {
            // must check if it will be blanked as a duplicate
            var hasVideo = !IsNullOrWhiteSpace(answer.YouTubeUrl) &&
              !duplicateVideoDictionary.ContainsKey(new
              {
                multiples.Key.PoliticianKey,
                answer.OldQuestionKey,
                answer.Sequence
              });

            csvWriter.AddField(newIssue);
            csvWriter.AddField(newQuestion);
            csvWriter.AddField(politicianKey);
            csvWriter.AddField(PrependOldQuestionToAnswerIfNecessary(answer.OldQuestionKey, answer.Answer));
            csvWriter.AddField(
              $"{_OldIssuesDictionary[answer.OldIssueKey].Issue} ({answer.OldIssueKey})");
            csvWriter.AddField(
              $"{_OldQuestionsDictionary[answer.OldQuestionKey].Question} ({answer.OldQuestionKey})");
            csvWriter.AddField(answer.Sequence.ToString());
            csvWriter.AddField(answer.DateStamp.ToShortDateString());
            csvWriter.AddField(hasVideo ? "Y" : "N");
            csvWriter.Write(streamWriter);

            newIssue = Empty;
            newQuestion = Empty;
            politicianKey = Empty;
          }
        }

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "multiple-answers.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }

      // create CSV with all eliminated duplicate answers, ordered by answer length
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write header
        csvWriter.AddField("Length");
        csvWriter.AddField("Answer");
        csvWriter.Write(streamWriter);

        foreach (var answer in duplicateTextAnswersList)
        {
          csvWriter.AddField(answer.Length.ToString());
          csvWriter.AddField(answer);
          csvWriter.Write(streamWriter);
        }

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "duplicate-answers.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }
    }

    private void CreateIssueGroupCsvs()
    {
      // validate the IssueGroups
      var duplicateIssueGroups = AllOptions.IssueGroups
        .GroupBy(ig => ig.IssueGroup, StringComparer.OrdinalIgnoreCase)
        .Where(g => g.Count() > 1).ToList();
      if (duplicateIssueGroups.Any())
      {
        var message =
          $"Duplicate IssueGroups\n{Join("\n", duplicateIssueGroups.Select(j => j.Key))}";
        throw new VoteException(message);
      }

      var issueGroupAssignments = AllOptions.IssueGroups.SelectMany(ig => ig.Issues)
        .GroupBy(i => i, StringComparer.OrdinalIgnoreCase).ToList();
      var duplicateIssueGroupAssignments = issueGroupAssignments.Where(g => g.Count() > 1).ToList();
      if (duplicateIssueGroupAssignments.Any())
      {
        var message =
          $"Duplicate IssueGroup assignments\n{Join("\n", duplicateIssueGroupAssignments.Select(j => j.Key))}";
        throw new VoteException(message);
      }

      var newIssuesDictionary = _NewIssuesDictionary.Select(kvp => kvp.Value.Issue as string)
        .Distinct().ToDictionary(i => i, StringComparer.OrdinalIgnoreCase);
      var assignedIssuesDictionary =
        issueGroupAssignments.ToDictionary(g => g.Key, StringComparer.OrdinalIgnoreCase);
      var assignedIssuesNotFound = assignedIssuesDictionary.Select(kvp => kvp.Key)
        .Where(i => !newIssuesDictionary.ContainsKey(i)).ToList();
      if (assignedIssuesNotFound.Any())
      {
        var message =
          $"Assigned Issues not found\n{Join("\n", assignedIssuesNotFound.Select(j => j))}";
        throw new VoteException(message);
      }
      var unassignedIssues = newIssuesDictionary.Select(kvp => kvp.Key)
        .Where(i => !assignedIssuesDictionary.ContainsKey(i)).ToList();
      if (unassignedIssues.Any())
      {
        var message =
          $"Issues not assigned to a group\n{Join("\n", assignedIssuesNotFound.Select(j => j))}";
        throw new VoteException(message);
      }

      // create the summary csv
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("IssueGroup");
        csvWriter.AddField("SubHeading");
        csvWriter.AddField("Issue");
        csvWriter.Write(streamWriter);

        foreach (var group in AllOptions.IssueGroups.OrderBy(g => g.IssueGroupOrder))
        {
          var issueGroup = group.IssueGroup;
          var subHeading = group.SubHeading.SafeString().ReplaceBreakTagsWithSpaces();
          foreach (var issue in group.Issues)
          {
            csvWriter.AddField(issueGroup);
            csvWriter.AddField(subHeading);
            csvWriter.AddField(issue);
            csvWriter.Write(streamWriter);
            issueGroup = Empty;
            subHeading = Empty;
          }
        }

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "issuegroups-issues.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }

      // create new issue dictionary of new questions...
      var issuesQuestionsDictionary = _OldQuestionsDictionary.Select(kvp => new
      {
        QuestionOrder = (int) kvp.Value.QuestionOrder,
        Issue = _NewIssuesDictionary[kvp.Value.IssueKey].Issue as string,
        Question = _NewQuestionsDictionary[kvp.Value.QuestionKey].Question as string
      }).GroupBy(q => new {q.Issue}).ToDictionary(g => g.Key.Issue,
        g => g.GroupBy(q => q.Question).OrderBy(g2 => g2.Average(g3 => g3.QuestionOrder))
          .Select(g2 => g2.Key).ToList(), StringComparer.OrdinalIgnoreCase);

      // create the detail csv
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("IssueGroup");
        csvWriter.AddField("SubHeading");
        csvWriter.AddField("Issue");
        csvWriter.AddField("Question");
        csvWriter.Write(streamWriter);

        foreach (var group in AllOptions.IssueGroups.OrderBy(g => g.IssueGroupOrder))
        {
          var issueGroup = group.IssueGroup;
          var subHeading = group.SubHeading.SafeString().ReplaceBreakTagsWithSpaces();
          foreach (var i in group.Issues)
          {
            var issue = i;
            foreach (var q in issuesQuestionsDictionary[i])
            {
              csvWriter.AddField(issueGroup);
              csvWriter.AddField(subHeading);
              csvWriter.AddField(issue);
              csvWriter.AddField(q);
              csvWriter.Write(streamWriter);
              issueGroup = Empty;
              subHeading = Empty;
              issue = Empty;
            }
          }
        }

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "issuegroups-issues-questions.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }
    }

    private void GetIssues()
    {
      // get all issues, apply mappings, apply omits and group by mapped name
      var issuesList = Issues.GetAllData().Select(r =>
      {
        var m = GetMappedIssue(r.Issue);
        return new
        {
          IssueKey = r.IssueKey.ToUpperInvariant(),
          r.IssueOrder,
          r.Issue,
          r.IssueLevel,
          r.StateCode,
          r.CountyCode,
          r.LocalKey,
          r.IsIssueOmit,
          r.IsTextSourceOptional,
          m.MappedIssue,
          m.MappedOmit
        };
      }).Where(r => !r.IsIssueOmit && !r.MappedOmit).GroupBy(r => r.MappedIssue).ToList();

      // create a list of these new, consolidated issues and assign an id and an order
      // equal to the average order of its constituents.
      var issueId = 1000;
      var tempNewIssues = issuesList.Select(i => new
      {
        Issue = i.Key,
        Group = i,
        Id = _NewIssueMapDictionary.ContainsKey(i.Key)
          ? _NewIssueMapDictionary[i.Key].Id
          : ++issueId,
        Order = Convert.ToInt32(i.Select(j => j.IssueOrder).Average())
      }).OrderBy(i => i.Order).ToList();

      // normalize the order
      var order = 0;
      var newIssues = tempNewIssues.OrderBy(i => i.Order)
        .Select(i => new {i.Issue, i.Group, i.Id, Order = order += 10}).ToList();

      // create dictionaries of old and new issues for all original IssueKeys
      foreach (var i in newIssues)
      foreach (var j in i.Group)
      {
        _OldIssuesDictionary.Add(j.IssueKey, j);
        _NewIssuesDictionary.Add(j.IssueKey, i);
      }
    }

    private OldIssueMapItem GetMappedIssue(string issue)
    {
      // if there's no mapped issue, return a reasonable default
      if (!_OldIssueMapDictionary.TryGetValue(issue, out var result))
        result = new OldIssueMapItem
        {
          Issue = issue, MappedIssue = issue, MappedOmit = false
        };
      return result;
    }

    private OldQuestionMapItem GetMappedQuestion(string question)
    {
      // if there's no mapped question, return a reasonable default
      if (!_OldQuestionMapDictionary.TryGetValue(question, out var result))
        result = new OldQuestionMapItem
        {
          Question = question,
          MappedQuestion = question,
          MappedOmit = false,
          NewOmit = false,
          SubstituteStateForName = false
        };
      return result;
    }

    private AnswersTable GetQuestionsAndCreateCsv()
    {
      // get all questions, apply mappings, apply omits and group by mapped name
      var questionsList = Questions.GetAllData().Select(r =>
        {
          string state = null;
          if (_OldIssuesDictionary.TryGetValue(r.IssueKey, out var oldIssue))
            _StatesDictionary.TryGetValue(oldIssue.StateCode, out state);
          var mappedQuestion = r.Question;
          if (!IsNullOrWhiteSpace(state))
          {
            if (state == "District of Columbia")
              state = "DC";
            mappedQuestion = mappedQuestion.Replace("in " + state, Empty);
            mappedQuestion = mappedQuestion.Replace(state + "'s", Empty);
            mappedQuestion = mappedQuestion.Replace(state, Empty);
          }

          mappedQuestion = mappedQuestion.StripRedundantSpaces();
          var originalQuestionWithoutState = mappedQuestion;
          var q = GetMappedQuestion(mappedQuestion);
          if (q.SubstituteStateForName && !IsNullOrWhiteSpace(state))
          {
            mappedQuestion =
              r.Question.Replace(state, state == "DC" ? "District" : "State");
            q = GetMappedQuestion(mappedQuestion);
          }

          return new
          {
            QuestionKey = r.QuestionKey.ToUpperInvariant(),
            IssueKey = r.IssueKey.ToUpperInvariant(),
            r.QuestionOrder,
            r.Question,
            OriginalQuestionWithoutState = originalQuestionWithoutState,
            r.IsQuestionOmit,
            q.MappedQuestion,
            q.MappedOmit,
            q.NewOmit,
            OldIssue = oldIssue
          };
        }).Where(r =>
          !r.IsQuestionOmit && !r.MappedOmit &&
          _OldIssuesDictionary.ContainsKey(r.IssueKey))
        .GroupBy(r => r.MappedQuestion).OrderBy(r => r.Key).ToList();

      // make sure all NewOmits in the questionsList agree
      var omitProblems = questionsList.Where(i => i.GroupBy(i2 => i2.NewOmit).Count() > 1)
        .ToList();
      if (omitProblems.Count > 0)
      {
        var message =
          $"Conflicting NewOmits\n{Join("\n", omitProblems.Select(i => $"{Join(", ", i.Select(j => j.Question))}"))}";
        throw new VoteException(message);
      }

      // create a list of these new, consolidated questions and assign an id
      // and an order equal to the average order of its constituents.
      var questionId = 1000;
      var newQuestions = questionsList.Select(i => new
      {
        Question = i.Key,
        Group = i,
        i.First().NewOmit,
        PrependOriginal =
          i.Select(q => q.OriginalQuestionWithoutState).GroupBy(q => q).Count() > 1,
        Id = _NewQuestionMapDictionary.ContainsKey(i.Key)
          ? _NewQuestionMapDictionary[i.Key].Id
          : ++questionId
      }).ToList();

      // create dictionaries of old and new questions for all original QuestionKeys
      foreach (var i in newQuestions)
      foreach (var j in i.Group)
      {
        _OldQuestionsDictionary.Add(j.QuestionKey, j);
        _NewQuestionsDictionary.Add(j.QuestionKey, i);
      }

      // Create dictionary of all answer counts
      var answersData = Answers.GetAllData();
      var answersDictionary = answersData.GroupBy(a => a.QuestionKey.ToUpperInvariant())
        .ToDictionary(g => g.Key, g => g.Count());

      // create CSV from questionsList
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("New Question");
        csvWriter.AddField("Answers");
        csvWriter.AddField("Original Questions");
        csvWriter.AddField("All");
        csvWriter.AddField("National");
        csvWriter.AddField("States");
        csvWriter.AddField("Old Issues");
        csvWriter.AddField("New Issues");
        csvWriter.AddField("Omit");
        csvWriter.Write(streamWriter);

        // put question list in new Issue order (there may be duplicate entries if new
        // question maps to multiple issues)
        questionsList = questionsList.Select(nq =>
            // get all old IssueKeys for the new Question
            nq.Select(oq => oq.IssueKey)
              // translate them to unique new Issues
              .Select(i => _NewIssuesDictionary[i].Issue).Distinct()
              // convert to object and flatten
              .Select(ni => new {Issue = ni, NewQuestion = nq})).SelectMany(o => o)
          // sort by Issue then discard Issue to restore to original shape
          .OrderBy(o => o.Issue)
          // secondary sort is most frequently occurring old issue to keep similar questions close
          .ThenBy(o =>
            o.NewQuestion.GroupBy(q => q.OldIssue.Issue as string)
              .OrderByDescending(g => g.Count()).First().Key).Select(o => o.NewQuestion)
          .ToList();

        var totalAnswers = 0;
        foreach (var question in questionsList)
        {
          // look for "ALL" at the beginning of the Question Key(
          var all = question.Any(q =>
            q.QuestionKey.Substring(0, 3).ToUpperInvariant() == "ALL");

          // look for "BUS" at the beginning of the Question Key
          var national = question.Any(q =>
            q.QuestionKey.Substring(0, 3).ToUpperInvariant() == "BUS");

          var statesCount = question
            // only select state-level questions
            .Where(q =>
              _StatesDictionary.ContainsKey(
                q.QuestionKey.Substring(1, 2).ToUpperInvariant())).Sum(q =>
              answersDictionary.ContainsKey(q.QuestionKey)
                ? answersDictionary[q.QuestionKey]
                : 0);

          var states = question
            // extract the StateCode from old IssueKey
            .Select(q => q.QuestionKey.Substring(1, 2).ToUpperInvariant())
            // old use valid, unique StateCodes
            .Where(s => _StatesDictionary.ContainsKey(s)).Distinct().OrderBy(s => s)
            .ToList();
          var statesMessage = Empty;

          // choose format for showing states to minimize length of list
          if (states.Count == 51)
          {
            statesMessage = "All";
          }
          else if (states.Count > 25)
          {
            // display as All except... with count
            var except = _StatesDictionary.Select(s => s.Key)
              .Where(s => !states.Contains(s)).OrderBy(s => s);
            statesMessage = $"({states.Count}) All except: {Join(", ", except)}";
          }
          else if (states.Count == 1)
          {
            statesMessage = states[0];
          }
          else if (states.Count != 0)
          {
            // display with count
            statesMessage = $"({states.Count}) {Join(", ", states)}";
          }

          // add juristiction entries
          var id = _NewQuestionsDictionary[question.First().QuestionKey].Id;
          if (!_QuestionsJurisdictionsDictionary.ContainsKey(id))
          {
            _QuestionsJurisdictionsDictionary.Add(id, null);
            if (all)
              _QuestionsJurisdictionsList.Add(new
              {
                QuestionId = id, Level = Issues.IssueLevelAll, StateCode = Empty
              });
            if (national)
              _QuestionsJurisdictionsList.Add(new
              {
                QuestionId = id, Level = Issues.IssueLevelNational, StateCode = Empty
              });
            if (states.Count == 51)
              _QuestionsJurisdictionsList.Add(new
              {
                QuestionId = id, Level = Issues.IssueLevelState, StateCode = Empty
              });
            else
              foreach (var state in states)
                _QuestionsJurisdictionsList.Add(new
                {
                  QuestionId = id, Level = Issues.IssueLevelState, StateCode = state
                });
          }

          // create list of all old issues referenced in the merged question
          var oldIssuesList = question.Select(q => q.IssueKey).Distinct()
            .Select(i => $"{_OldIssuesDictionary[i].Issue} ({i})").OrderBy(i => i).ToList();

          // create list of all new issues referenced in the merged question
          var newIssuesList = question.Select(q => q.IssueKey).Distinct()
            .Select(i =>
              $"{_NewIssuesDictionary[i].Issue} (Id={_NewIssuesDictionary[i].Id})")
            .Distinct().OrderBy(i => i).ToList();

          // add up answers for all old questions being merged
          var answers = question.Sum(q =>
            answersDictionary.ContainsKey(q.QuestionKey)
              ? answersDictionary[q.QuestionKey]
              : 0);

          totalAnswers += answers;

          csvWriter.AddField(question.Key);
          csvWriter.AddField(answers.ToString());
          csvWriter.AddField(Join("\n",
            question.Select(q =>
                $"{q.Question} [{(answersDictionary.ContainsKey(q.QuestionKey) ? answersDictionary[q.QuestionKey] : 0).ToString()}] ({q.QuestionKey})")
              .OrderBy(q => q)));
          csvWriter.AddField(all ? "X" : Empty);
          csvWriter.AddField(national ? "X" : Empty);
          csvWriter.AddField($"{statesMessage} [{statesCount}]");
          csvWriter.AddField(Join("\n", oldIssuesList));
          csvWriter.AddField(Join("\n", newIssuesList));
          csvWriter.AddField(question.First().NewOmit ? "Y" : Empty);

          csvWriter.Write(streamWriter);
        }

        // write an extra row with total answer count
        csvWriter.AddField(Empty);
        csvWriter.AddField(totalAnswers.ToString());
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.Write(streamWriter);

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "questions.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }

      // create topics CSV from questionsList
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("New Question");
        csvWriter.AddField("Question ID");
        csvWriter.AddField("Answers");
        csvWriter.AddField("Topics");
        csvWriter.AddField("New Issues");
        csvWriter.Write(streamWriter);

        var totalAnswers = 0;
        foreach (var question in questionsList.Where(q => !q.First().NewOmit))
        {
          // create list of all new issues referenced in the merged question
          var newIssuesList = question.Select(q => q.IssueKey).Distinct()
            .Select(i =>
              $"{_NewIssuesDictionary[i].Issue} (Id={_NewIssuesDictionary[i].Id})")
            .Distinct().OrderBy(i => i).ToList();

          // add up answers for all old questions being merged
          var answers = question.Sum(q =>
            answersDictionary.ContainsKey(q.QuestionKey)
              ? answersDictionary[q.QuestionKey]
              : 0);

          totalAnswers += answers;

          var issueId = _NewIssuesDictionary[question.First().IssueKey].Id;
          csvWriter.AddField(question.Key);
          csvWriter.AddField(_NewQuestionsDictionary[question.First().QuestionKey].Id
            .ToString());
          csvWriter.AddField(answers.ToString());
          csvWriter.AddField(Join("\n",
            question
              .GroupBy(q => q.OriginalQuestionWithoutState,
                StringComparer.OrdinalIgnoreCase)
              .Where(g =>
                g.Key.IsNeIgnoreCase(question.Key) &&
                g.Key.IndexOf("General Statement", StringComparison.OrdinalIgnoreCase) <
                0 && issueId != 1).OrderBy(g => g.Key).Select(g =>
                $"{g.Key} [{g.Sum(q => answersDictionary.ContainsKey(q.QuestionKey) ? answersDictionary[q.QuestionKey] : 0)}]")));
          csvWriter.AddField(Join("\n", newIssuesList));

          csvWriter.Write(streamWriter);
        }

        // write an extra row with total answer count
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.AddField(totalAnswers.ToString());
        csvWriter.AddField(Empty);
        csvWriter.AddField(Empty);
        csvWriter.Write(streamWriter);

        // write the csv
        streamWriter.Flush();
        ms.Position = 0;
        File.WriteAllText(Path.Combine(SaveFolderTextBox.Text, "topics.csv"),
          new StreamReader(ms).ReadToEnd(), Encoding.UTF8);
      }

      return answersData;
    }

    private void LoadOptions()
    {
      // get options from external json file
      var optionsFilename = OptionsFileTextBox.Text;
      if (IsNullOrWhiteSpace(optionsFilename))
        throw new Exception("Please select a JSON Options file");
      var optionsJson = File.ReadAllText(optionsFilename);
      var serializer = new JavaScriptSerializer();
      AllOptions = serializer.Deserialize<Options>(optionsJson);

      // load OldIssueMap options
      var oldIssueMapList = AllOptions.OldIssueMap.Select(i => new OldIssueMapItem
      {
        Issue = i.Issue,
        MappedIssue = IsNullOrWhiteSpace(i.MappedIssue) ? i.Issue : i.MappedIssue,
        MappedOmit = i.MappedOmit
      }).ToList();
      var oldIssueDuplicates = oldIssueMapList
        .GroupBy(i => i.Issue, StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1)
        .ToList();
      if (oldIssueDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Issues in OldIssueMap\n{Join("\n", oldIssueDuplicates.Select(i => $"{i.Key} [{Join(", ", i.Select(j => j.MappedIssue))}]"))}";
        throw new VoteException(message);
      }

      _OldIssueMapDictionary = oldIssueMapList.ToDictionary(i => i.Issue, i => i,
        StringComparer.OrdinalIgnoreCase);

      // load OldQuestionMap options
      var questionMapList = AllOptions.OldQuestionMap.Select(i => new OldQuestionMapItem
      {
        Question = i.Question,
        MappedQuestion =
          IsNullOrWhiteSpace(i.MappedQuestion) ? i.Question : i.MappedQuestion,
        MappedOmit = i.MappedOmit,
        NewOmit = i.NewOmit,
        SubstituteStateForName = i.SubstituteStateForName
      }).ToList();
      var questionDuplicates = questionMapList
        .GroupBy(i => i.Question, StringComparer.OrdinalIgnoreCase)
        .Where(g => g.Count() > 1).ToList();
      if (questionDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Questions in QuestionsMap\n{Join("\n", questionDuplicates.Select(i => $"{i.Key} [{Join(", ", i.Select(j => j.MappedQuestion))}]"))}";
        throw new VoteException(message);
      }

      _OldQuestionMapDictionary = questionMapList.ToDictionary(i => i.Question, i => i,
        StringComparer.OrdinalIgnoreCase);

      // load NewIssueMap options
      var newIssueMapList = AllOptions.NewIssueMap
        .Select(i => new NewIssueMapItem {Issue = i.Issue, Id = i.Id}).ToList();
      var newIssueDuplicates = newIssueMapList.GroupBy(i => i.Issue)
        .Where(g => g.Count() > 1).ToList();
      if (newIssueDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Issues in NewIssueMap\n{Join("\n", newIssueDuplicates.Select(i => $"{i.Key}"))}";
        throw new VoteException(message);
      }

      var newIssueIdDuplicates =
        newIssueMapList.GroupBy(i => i.Id).Where(g => g.Count() > 1).ToList();
      if (newIssueIdDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Ids in NewIssueMap\n{Join("\n", newIssueIdDuplicates.Select(i => $"{i.Key} [{Join(", ", i.Select(j => j.Issue))}]"))}";
        throw new VoteException(message);
      }

      _NewIssueMapDictionary = newIssueMapList.ToDictionary(i => i.Issue, i => i,
        StringComparer.OrdinalIgnoreCase);

      // load NewQuestionMap options
      var newQuestionMapList = AllOptions.NewQuestionMap
        .Select(i => new NewQuestionMapItem {Question = i.Question, Id = i.Id}).ToList();
      var newQuestionDuplicates = newQuestionMapList.GroupBy(i => i.Question)
        .Where(g => g.Count() > 1).ToList();
      if (newQuestionDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Questions in NewQuestionMap\n{Join("\n", newQuestionDuplicates.Select(i => $"{i.Key}"))}";
        throw new VoteException(message);
      }

      var newQuestionIdDuplicates = newQuestionMapList.GroupBy(i => i.Id)
        .Where(g => g.Count() > 1).ToList();
      if (newQuestionIdDuplicates.Count > 0)
      {
        var message =
          $"Duplicate Ids in NewQuestionMap\n{Join("\n", newQuestionIdDuplicates.Select(i => $"{i.Key} [{Join(", ", i.Select(j => j.Question))}]"))}";
        throw new VoteException(message);
      }

      _NewQuestionMapDictionary = newQuestionMapList.ToDictionary(i => i.Question, i => i,
        StringComparer.OrdinalIgnoreCase);
    }

    private string PrependOldQuestionToAnswerIfNecessary(string questionKey, string answer)
    {
      answer = answer.Trim();
      if (!IsNullOrWhiteSpace(answer) &&
        answer.Length < AllOptions.LongAnswerMinimumLength &&
        _NewQuestionsDictionary[questionKey].PrependOriginal)
        answer =
          $"{_OldQuestionsDictionary[questionKey].OriginalQuestionWithoutState}: {answer}";
      return answer;
    }

    private void Process()
    {
      _OldIssuesDictionary.Clear();
      _NewIssuesDictionary.Clear();
      _OldQuestionsDictionary.Clear();
      _NewQuestionsDictionary.Clear();
      _QuestionsJurisdictionsDictionary.Clear();
      _QuestionsJurisdictionsList.Clear();
      _DuplicateVideoDictionary.Clear();
      _DuplicateTextDictionary.Clear();

      try
      {
        // create state name dictionary
        _StatesDictionary = States.GetAllData().Where(r => r.IsState)
          .ToDictionary(r => r.StateCode, r => r.State, StringComparer.OrdinalIgnoreCase);

        LoadOptions();
        GetIssues();
        var answersData = GetQuestionsAndCreateCsv();
        CreateIssueGroupCsvs();
        AnalyzeAnswersAndCreateCsv(answersData);

        AppendStatusText("CSVs written");

        if (UpdateDatabaseCheckBox.Checked)
          UpdateDatabase(answersData);

        AppendStatusText("Task complete");
        AppendStatusText(Empty);
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
      }
    }

    private void UpdateDatabase(AnswersTable answersData)
    {
      bool IsBioOrReasonsIssue(int issueId)
      {
        return issueId == Issues.IssueId.Biographical.ToInt() ||
          issueId == Issues.IssueId.Reasons.ToInt();
      }

      bool IsBioOrReasonsQuestion(int questionId)
      {
        return questionId == Issues.QuestionId.GeneralPhilosophy.ToInt() ||
          questionId == Issues.QuestionId.PersonalAndFamily.ToInt() ||
          questionId == Issues.QuestionId.ProfessionalExperience.ToInt() ||
          questionId == Issues.QuestionId.CivicInvolvement.ToInt() ||
          questionId == Issues.QuestionId.PoliticalExperience.ToInt() ||
          questionId == Issues.QuestionId.ReligiousAffiliation.ToInt() ||
          questionId == Issues.QuestionId.AccomplishmentsAndAwards.ToInt() ||
          questionId == Issues.QuestionId.EducationalBackground.ToInt() ||
          questionId == Issues.QuestionId.MilitaryService.ToInt() ||
          questionId == Issues.QuestionId.WhyIAmRunning.ToInt() ||
          questionId == Issues.QuestionId.GoalsIfElected.ToInt() ||
          questionId == Issues.QuestionId.AchievementsIfElected.ToInt() ||
          questionId == Issues.QuestionId.AreasToConcentrateOn.ToInt() ||
          questionId == Issues.QuestionId.OnEnteringPublicService.ToInt() ||
          questionId == Issues.QuestionId.OpinionsOfOtherCandidates.ToInt();
      }

      AppendStatusText("Updating database");

      IssueGroups2.TruncateTable();
      IssueGroupsIssues2.TruncateTable();
      Issues2.TruncateTable();
      Questions2.TruncateTable();
      IssuesQuestions.TruncateTable();
      QuestionsJurisdictions.TruncateTable();
      Answers2.TruncateTable();
      AppendStatusText("Output tables truncated");

      // write IssueGroups2 and IssueGroupsIssues2
      var count = 0;
      foreach (var issueGroup in AllOptions.IssueGroups.OrderBy(ig => ig.IssueGroupOrder))
      {
        if (OnlyBioAndReasonsCheckbox.Checked)
        {
          var isOk = false;
          // only continue if group contains bio or reasons
          foreach (var issue in issueGroup.Issues)
          {
            var issueId = (int)_NewIssuesDictionary.First(kvp => issue == kvp.Value.Issue as string)
              .Value.Id;
            if (IsBioOrReasonsIssue(issueId))
              isOk = true;
          }
          if (!isOk) continue;
        }
        var issueGroupId = IssueGroups2.Insert(issueGroup.IssueGroupOrder, true, issueGroup.IssueGroup,
          issueGroup.SubHeading.SafeString());
        var order = 0;
        foreach (var issue in issueGroup.Issues)
        {
          var issueId = (int)_NewIssuesDictionary.First(kvp => issue == kvp.Value.Issue as string)
            .Value.Id;
          IssueGroupsIssues2.Insert(issueGroupId, issueId, order += 10);
          count++;
        }
      }
      AppendStatusText($"{ AllOptions.IssueGroups.Length} IssueGroups2 rows written");
      AppendStatusText($"{count} IssueGroupsIssues2 rows written");

      count = 0;
      // this dictionary is keyed by old IssueKey, so there will be duplicate new Issue
      // entries -- so we summarize first
      foreach (var issue in _NewIssuesDictionary.GroupBy(kvp => kvp.Value.Id)
        .Select(g => g.First().Value))
      {
        if (OnlyBioAndReasonsCheckbox.Checked && !IsBioOrReasonsIssue(issue.Id)) continue;
        // write Issues2 row for each new Issue
        Issues2.Insert(issue.Id, issue.Order, issue.Issue, false);
        count++;
        if (count % 100 == 0)
          AppendStatusText($"{count} Issues2 rows written");
      }

      AppendStatusText($"{count} Issues2 rows written");

      count = 0;
      // this dictionary is keyed by old QuestionKey, so there will be duplicate new 
      // Question entries -- so we summarize first
      foreach (var question in _NewQuestionsDictionary.GroupBy(kvp => kvp.Value.Id)
        .Select(g => g.First().Value))
      {
        if (OnlyBioAndReasonsCheckbox.Checked && !IsBioOrReasonsQuestion(question.Id)) continue;
        // write Questions2 row for each new Question
        Questions2.Insert(question.Id, question.Question, question.NewOmit);
        count++;
        if (count % 100 == 0)
          AppendStatusText($"{count} Questions2 rows written");
      }

      AppendStatusText($"{count} Questions2 rows written");

      // for each new issue we need a list of new questions, and for each of these a list
      // of old questions
      var issuesQuestionGroups = _OldQuestionsDictionary.Select(kvp => new
      {
        kvp.Value.QuestionOrder,
        IssueId = _NewIssuesDictionary[kvp.Value.IssueKey].Id,
        QuestionId = _NewQuestionsDictionary[kvp.Value.QuestionKey].Id
      }).GroupBy(q => new {q.IssueId, q.QuestionId}).Select(g => new
      {
        g.Key.IssueId, g.Key.QuestionId, QuestionOrder = g.Average(g2 => g2.QuestionOrder)
      }).GroupBy(q => q.IssueId).ToList();

      // use this to create IssuesQuestions entries
      count = 0;
      foreach (var issue in issuesQuestionGroups)
      {
        // normalize the question order
        var questionOrder = 0;
        var questions = issue.OrderBy(q => q.QuestionOrder).Select(q =>
          new {q.IssueId, q.QuestionId, QuestionOrder = questionOrder += 10}).ToList();
        foreach (var question in questions)
        {
          if (OnlyBioAndReasonsCheckbox.Checked && !IsBioOrReasonsIssue(question.IssueId)) continue;
          IssuesQuestions.Insert(question.IssueId, question.QuestionId,
            question.QuestionOrder);
          count++;
          if (count % 100 == 0)
            AppendStatusText($"{count} IssuesQuestions rows written");
        }
      }

      // write the QuestionsJurisdictions entries
      count = 0;
      foreach (var i in _QuestionsJurisdictionsList)
      {
        if (OnlyBioAndReasonsCheckbox.Checked && !IsBioOrReasonsQuestion(i.QuestionId)) continue;
        QuestionsJurisdictions.Insert(i.QuestionId, i.Level, i.StateCode, Empty);
        count++;
        if (count % 1000 == 0)
          AppendStatusText($"{count} QuestionsJurisdictions rows written");
      }

      AppendStatusText($"{count} QuestionsJurisdictions rows written");

      // write Answers2
      var answerGroups = answersData
        // eliminate duplicates and omits
        .Where(a =>
        {
          if (!_NewQuestionsDictionary.ContainsKey(a.QuestionKey))
            return false;
          var youTubeUrl = _DuplicateVideoDictionary.ContainsKey(new
          {
            PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
            OldQuestionKey = a.QuestionKey.ToUpperInvariant(),
            a.Sequence
          })
            ? Empty
            : a.YouTubeUrl;
          var answer = a.Answer.Trim().Length >= AllOptions.LongAnswerMinimumLength &&
            _DuplicateTextDictionary.ContainsKey(new
          {
            PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
            OldQuestionKey = a.QuestionKey.ToUpperInvariant(),
            a.Sequence
          })
            ? Empty
            : a.Answer;
          return !IsNullOrWhiteSpace(youTubeUrl) || !IsNullOrWhiteSpace(answer);
        })
        // group by Politician, QuestionID so we can assign a new 
        // sequence by date order
        .GroupBy(a => new
        {
          PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
          QuestionId = _NewQuestionsDictionary[a.QuestionKey].Id
        });

      count = 0;
      foreach (var g in answerGroups)
      {
        var sequence = 0;
        foreach (var a in g.OrderBy(r => r.DateStamp))
        {
          if (OnlyBioAndReasonsCheckbox.Checked && !IsBioOrReasonsQuestion(g.Key.QuestionId)) continue;
          var answer = PrependOldQuestionToAnswerIfNecessary(a.QuestionKey, a.Answer);
          Answers2.Insert(a.PoliticianKey, g.Key.QuestionId, sequence++, answer, a.Source,
            a.DateStamp, a.UserName, a.YouTubeUrl, a.YouTubeDescription,
            a.YouTubeRunningTime, a.YouTubeSource, a.YouTubeSourceUrl, a.YouTubeDate,
            a.YouTubeRefreshTime, a.YouTubeAutoDisable, a.FacebookVideoUrl,
            a.FacebookVideoDescription, a.FacebookVideoRunningTime, a.FacebookVideoDate,
            a.FacebookVideoRefreshTime, a.FacebookVideoAutoDisable);
          count++;
          if (count % 1000 == 0)
            AppendStatusText($"{count} Answers2 rows written");
        }
      }

      AppendStatusText($"{count} Answers2 rows written");
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Process();
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender,
      RunWorkerCompletedEventArgs e)
    {
      ServerGroupBox.Enabled = true;
      OptionsFileTextBox.Enabled = true;
      OptionsFileBrowseButton.Enabled = true;
      StartButton.Enabled = true;
      SaveFolderTextBox.Enabled = true;
      SaveFolderBrowseButton.Enabled = true;
      UpdateDatabaseCheckBox.Enabled = true;
    }

    private void OptionsFileBrowseButton_Click(object sender, EventArgs e)
    {
      OpenOptionsFileDialog.FileName = OptionsFileTextBox.Text;
      if (OpenOptionsFileDialog.ShowDialog() == DialogResult.OK)
        OptionsFileTextBox.Text = OpenOptionsFileDialog.FileName;
    }

    private void SaveFolderBrowseButton_Click(object sender, EventArgs e)
    {
      SaveFolderBrowserDialog.SelectedPath = SaveFolderTextBox.Text;
      if (SaveFolderBrowserDialog.ShowDialog() == DialogResult.OK)
        SaveFolderTextBox.Text = SaveFolderBrowserDialog.SelectedPath;
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        if (sender is RadioButton radioButton && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      ServerGroupBox.Enabled = false;
      OptionsFileTextBox.Enabled = false;
      OptionsFileBrowseButton.Enabled = false;
      StartButton.Enabled = false;
      SaveFolderTextBox.Enabled = false;
      SaveFolderBrowseButton.Enabled = false;
      UpdateDatabaseCheckBox.Enabled = false;
      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion

    // ReSharper disable ClassNeverInstantiated.Global
    // ReSharper disable UnassignedField.Global
    public class OldIssueMapItem
    {
      public string Issue;
      public string MappedIssue;
      public bool MappedOmit;
    }

    public class OldQuestionMapItem
    {
      public string Question;
      public string MappedQuestion;
      public bool MappedOmit;
      public bool NewOmit;
      public bool SubstituteStateForName;
    }

    public class NewIssueMapItem
    {
      public string Issue;
      public int Id;
    }

    public class NewQuestionMapItem
    {
      public string Question;
      public int Id;
    }

    public class IssueGroupItem
    {
      public string IssueGroup;
      public int Id;
      public int IssueGroupOrder;
      public string SubHeading;
      public string[] Issues;
    }

    public class Options
    {
      public int LongAnswerMinimumLength;
      public OldIssueMapItem[] OldIssueMap;
      public OldQuestionMapItem[] OldQuestionMap;
      public NewIssueMapItem[] NewIssueMap;
      public IssueGroupItem[] IssueGroups;

      public NewQuestionMapItem[] NewQuestionMap;
    }
    // ReSharper restore UnassignedField.Global
    // ReSharper restore ClassNeverInstantiated.Global
  }
}