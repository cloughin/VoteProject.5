using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DB;
using DB.Vote;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;
using Vote;
using static System.String;

namespace SetUpIssues
{
  public partial class MainForm : Form
  {
    private const int issuesIssueGroupId = 3;

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
    private readonly Dictionary<string, OneIssue> _IssueDictionary = new Dictionary<string, OneIssue>(StringComparer.OrdinalIgnoreCase);
    //private readonly Dictionary<int, OneIssue> _IssueIdDictionary = new Dictionary<int, OneIssue>();
    private readonly Dictionary<string, OneTopic> _TopicDictionary = new Dictionary<string, OneTopic>(StringComparer.OrdinalIgnoreCase);
    //private readonly Dictionary<int, OneTopic> _TopicIdDictionary = new Dictionary<int, OneTopic>();
    private readonly Dictionary<string, int> _QuestionDictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

    private void LoadOptions()
    {
      // get options from external json file
      var optionsFilename = OptionsFileTextBox.Text;
      if (IsNullOrWhiteSpace(optionsFilename))
        throw new Exception("Please select a JSON Options file");
      var optionsJson = File.ReadAllText(optionsFilename);
      var serializer = new JavaScriptSerializer();
      AllOptions = serializer.Deserialize<Options>(optionsJson);

      // create IDs for all issues
      var issueId = 1001;
      foreach (var issue in AllOptions.Issues)
      {
        var oneIssue = new OneIssue {Issue = issue.Trim(), IssueId = issueId};
        if (_IssueDictionary.ContainsKey(oneIssue.Issue))
          throw new VoteException($"Duplicate issue: \"{oneIssue.Issue}\"");
        _IssueDictionary.Add(oneIssue.Issue, oneIssue);
        //_IssueIdDictionary.Add(issueId, oneIssue);
        issueId++;
      }

      // create IDs for all topics
      var topicId = 1001;
      foreach (var topic in AllOptions.Topics)
      {
        var oneTopic = new OneTopic {Topic = topic.Topic.Trim(), TopicId = topicId};
        if (_TopicDictionary.ContainsKey(oneTopic.Topic))
          throw new VoteException($"Duplicate topic: \"{oneTopic.Topic}\"");
        if (topic.Issues.Count == 0)
          throw new VoteException($"No issues for topic: \"{oneTopic.Topic}\"");
        foreach (var issue in topic.Issues)
        {
          if (!_IssueDictionary.TryGetValue(issue.Trim(), out var oneIssue))
            throw new VoteException(
              $"Missing issue: \"{issue.Trim()}\" for topic \"{oneTopic.Topic}\"");
          oneIssue.TopicIds.Add(topicId);
          oneTopic.IssueIds.Add(oneIssue.IssueId);
        }
        _TopicDictionary.Add(topic.Topic.Trim(), oneTopic);
        //_TopicIdDictionary.Add(topicId, oneTopic);
        topicId++;
      }

      // make sure all Issues have at least one topic
      foreach (var kvp in _IssueDictionary)
        if (kvp.Value.TopicIds.Count == 0)
          throw new VoteException($"No topics for issue: \"{kvp.Value.Issue}\"");

      // load the answers file CSV
      var answersFilename = AnswersFileTextBox.Text;
      if (IsNullOrWhiteSpace(answersFilename))
        throw new Exception("Please select a CSV Answers Mapping file");
      var missingTopics = 0;
      var duplicateTopics = 0;
      using (var csvReader = new CsvReader(new StringReader(File.ReadAllText(answersFilename)), true))
      {
        var headers = csvReader.GetFieldHeaders();
        while (csvReader.ReadNextRecord())
        {
          var questions = csvReader[headers[0]].Split('\n');
          var topic = csvReader[headers[1]].Trim();
          if (!_TopicDictionary.TryGetValue(topic, out var topicEntry))
          {
            AppendStatusText($"Missing topic {topic} for answers {csvReader[headers[0]]}");
            missingTopics++;
          }
          else
          {
            // create question map entries
            foreach (var question in questions)
            {
              var key = Regex.Match(question, @"\((?<key>.+)\)$").Groups["key"].Value;
              if (IsNullOrWhiteSpace(key))
                throw new Exception($"Missing key in question {question}");
              if (_QuestionDictionary.TryGetValue(key, out var questionEntry))
              {
                if (questionEntry != topicEntry.TopicId)
                {
                  AppendStatusText($"Queston key {key} maps to different topics");
                  duplicateTopics++;
                }
              }
              else _QuestionDictionary.Add(key, topicEntry.TopicId); 
            }
          }
        }
      }
      if (missingTopics > 0)
        throw new VoteException($"There were {missingTopics} missing topics in answers mapping");
      if (duplicateTopics > 0)
        throw new VoteException($"There were {duplicateTopics} duplicate topics with conflicting mapping in answers mapping");
    }

    private static void DeleteNonPermanentAnswers()
    {
      const string cmdText = "DELETE FROM Answers2 WHERE QuestionId > 1000";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentIssueGroups()
    {
      const string cmdText = "DELETE FROM IssueGroups2 WHERE NOT IssueGroupId IN (1,2)";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentIssues()
    {
      const string cmdText = "DELETE FROM Issues2 WHERE NOT IssueId IN (1,2)";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentIssueGroupsIssues()
    {
      const string cmdText = "DELETE FROM IssueGroupsIssues2 WHERE NOT IssueId IN (1,2)";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentQuestions()
    {
      const string cmdText = "DELETE FROM Questions2 WHERE QuestionId > 1000";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentQuestionsJurisdictions()
    {
      const string cmdText = "DELETE FROM QuestionsJurisdictions WHERE QuestionId > 1000";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private static void DeleteNonPermanentIssuesQuestions()
    {
      const string cmdText = "DELETE FROM IssuesQuestions WHERE NOT IssueId IN (1,2)";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.ExecuteNonQuery(cmd);
    }

    private void UpdateDatabase()
    {
      // For now we just use a single Issue Group "Issues"
      // Create it, initially disabled
      DeleteNonPermanentIssueGroups();
      IssueGroups2.Insert(issuesIssueGroupId, 10, false, "Issues", Empty);
      AppendStatusText("Global Issue Group created");

      // Add the Issues
      DeleteNonPermanentIssues();
      foreach (var kvp in _IssueDictionary)
        Issues2.Insert(kvp.Value.IssueId, kvp.Value.IssueId, kvp.Value.Issue, false);
      AppendStatusText($"{_IssueDictionary.Count} Issues created");

      // Add the IssueGroupsIssues
      DeleteNonPermanentIssueGroupsIssues();
      foreach (var kvp in _IssueDictionary)
        IssueGroupsIssues2.Insert(issuesIssueGroupId, kvp.Value.IssueId, kvp.Value.IssueId);
      AppendStatusText($"{_IssueDictionary.Count} IssueGroupsIssues created");

      // Add the Questions
      DeleteNonPermanentQuestions();
      foreach (var kvp in _TopicDictionary)
        Questions2.Insert(kvp.Value.TopicId, kvp.Value.Topic, false);
      AppendStatusText($"{_TopicDictionary.Count} Questions created");

      // Add the QuestionJurisdictions
      // All are federal for now
      DeleteNonPermanentQuestionsJurisdictions();
      foreach (var kvp in _TopicDictionary)
        QuestionsJurisdictions.Insert(kvp.Value.TopicId, "B", Empty, Empty);
      AppendStatusText($"{_TopicDictionary.Count} QuestionsJurisdictions created");

      // Add the IssuesQuestions
      DeleteNonPermanentIssuesQuestions();
      var iqCount = 0;
      foreach (var kvp in _IssueDictionary)
      {
        var value = kvp.Value;
        foreach (var topicId in value.TopicIds)
        {
          IssuesQuestions.Insert(value.IssueId, topicId, topicId);
          iqCount++;
        }
      }
      AppendStatusText($"{iqCount} IssuesQuestions created");

      // remap existing answers
      RemapExistingAnswers();
    }

    private readonly Dictionary<dynamic, object> _DuplicateVideoDictionary =
      new Dictionary<dynamic, object>();

    private readonly Dictionary<dynamic, object> _DuplicateTextDictionary =
      new Dictionary<dynamic, object>();

    private Dictionary<string, string> _StatesDictionary;

    //private readonly Dictionary<string, dynamic> _OldQuestionsDictionary =
    //  new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    //private readonly Dictionary<string, dynamic> _NewQuestionsDictionary =
    //  new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

    private string PrependOldQuestionToAnswerIfNecessary(string questionKey, AnswersRow row)
    {
      var answer = row.Answer.Trim();
      if (!IsNullOrWhiteSpace(answer) &&
        answer.Length < AllOptions.LongAnswerMinimumLength
        //&& _NewQuestionsDictionary[questionKey].PrependOriginal
        )
      {
        _StatesDictionary.TryGetValue(row.StateCode, out var state);
        var question = Questions.GetQuestion(questionKey);
        if (!IsNullOrWhiteSpace(state))
        {
          if (state == "District of Columbia")
            state = "DC";
          question = question.Replace("in " + state, Empty);
          question = question.Replace(state + "'s", Empty);
          question = question.Replace(state, Empty);
        }

        question = question.StripRedundantSpaces();
        answer =
          $"{question}: {answer}";}
      return answer;
    }


    private void RemapExistingAnswers()
    {
      var oldAnswers = Answers.GetAllData();

      // Code all answers with new Question. 
      var codedAnswers = oldAnswers
        .Where(r =>
          _QuestionDictionary.ContainsKey(r.QuestionKey)).Select(r => new
        {
          QuestionKey = r.QuestionKey.ToUpperInvariant(),
          QuestionId = _QuestionDictionary[r.QuestionKey],
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
          r.QuestionId,
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
            a.QuestionKey,
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
            QuestionKey = r.QuestionKey.ToUpperInvariant(),
            QuestionId = _QuestionDictionary[r.QuestionKey],
            PoliticianKey = r.PoliticianKey.ToUpperInvariant(),
            Answer = r.Answer.Trim()
          }).ToList();
      var duplicateTextAnswers = groupedTextAnswers.Where(g => g.Count() > 1).ToList();

      // the old answers to have their Answers cleared on final output
      var duplicateTextDictionary = duplicateTextAnswers
        // queue all but the most recent for deletion
        .SelectMany(g => g.OrderByDescending(g2 => g2.DateStamp).Skip(1)).ToDictionary(a =>
          new
          {
            PoliticianKey = a.PoliticianKey.ToUpperInvariant(),
            QuestionKey = a.QuestionKey.ToUpperInvariant(),
            a.Sequence
          });
      foreach (var kvp in duplicateTextDictionary)
        _DuplicateTextDictionary.Add(kvp.Key, null);

      var oldFilteredAnswers = oldAnswers
        .Where(a =>
        {
          if (!_QuestionDictionary.ContainsKey(a.QuestionKey) ||
            a.QuestionKey.StartsWith("ALL", StringComparison.Ordinal)) return false;
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
        }).ToArray();

      AppendStatusText($"{oldAnswers.Count} existing answers read");
      AppendStatusText($"{oldFilteredAnswers.Length} answers mapped to new topics");

      var oldMappedAnswers = oldFilteredAnswers
        .Select(a => new { TopicId = _QuestionDictionary[a.QuestionKey()], AnswerRow = a })
        .GroupBy(a => new { a.TopicId, PoliticianKey = a.AnswerRow.PoliticianKey.ToUpperInvariant() }).ToList();

      // delete old answers
      DeleteNonPermanentAnswers();

      // write new
      var count = 0;
      foreach (var g in oldMappedAnswers)
      {
        var sequence = 0;
        foreach (var a in g.OrderBy(r => r.AnswerRow.DateStamp))
        {
          var row = a.AnswerRow;
          var answer = PrependOldQuestionToAnswerIfNecessary(row.QuestionKey, row);
          Answers2.Insert(row.PoliticianKey, g.Key.TopicId, sequence++, answer, row.Source,
            row.DateStamp, row.UserName, row.YouTubeUrl, row.YouTubeDescription,
            row.YouTubeRunningTime, row.YouTubeSource, row.YouTubeSourceUrl, row.YouTubeDate,
            row.YouTubeRefreshTime, row.YouTubeAutoDisable, row.FacebookVideoUrl,
            row.FacebookVideoDescription, row.FacebookVideoRunningTime, row.FacebookVideoDate,
            row.FacebookVideoRefreshTime, row.FacebookVideoAutoDisable);
          count++;
          if (count % 1000 == 0)
            AppendStatusText($"{count} Answers2 rows written");
        }
      }

      AppendStatusText($"{count} Answers2 rows written");
    }

    private void Process()
    {
      try
      {
        _IssueDictionary.Clear();
        //_IssueIdDictionary.Clear();
        _TopicDictionary.Clear();
        //_TopicIdDictionary.Clear();
        _QuestionDictionary.Clear();
        _DuplicateVideoDictionary.Clear();
        _DuplicateTextDictionary.Clear();

        //_OldQuestionsDictionary.Clear();
        //_NewQuestionsDictionary.Clear();

        // create state name dictionary
        _StatesDictionary = States.GetAllData().Where(r => r.IsState)
          .ToDictionary(r => r.StateCode, r => r.State, StringComparer.OrdinalIgnoreCase);

        LoadOptions();

        if (UpdateDatabaseCheckBox.Checked)
        {
          UpdateDatabase();
        }

        AppendStatusText("Complete");

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

    private void AnswersFileBrowseButton_Click(object sender, EventArgs e)
    {
      OpenAnswersFileDialog.FileName = AnswersFileTextBox.Text;
      if (OpenAnswersFileDialog.ShowDialog() == DialogResult.OK)
        AnswersFileTextBox.Text = OpenAnswersFileDialog.FileName;
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

    public class OneIssue
    {
      public string Issue;
      public int IssueId;
      public readonly List<int> TopicIds =  new List<int>();
    }

    public class OneTopic
    {
      public string Topic;
      public int TopicId;
      public List<int> IssueIds = new List<int>();
  }

    public class TopicOption
    {
      public string Topic;
      // ReSharper disable once CollectionNeverUpdated.Global
      public readonly List<string> Issues = new List<string>();
    }

    public class Options
    {
      public int LongAnswerMinimumLength;
      public string[] Issues;
      public TopicOption[] Topics;
    }
    // ReSharper restore UnassignedField.Global
    // ReSharper restore ClassNeverInstantiated.Global

  }
}
