using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global
  public sealed class QuestionsData
  {
    public QuestionsDataIssue[] Issues;
    public QuestionsDataQuestion[] Questions;
  }

  public sealed class QuestionsDataIssue
  {
    public int IssueId;
    public string Issue;
    public int[] QuestionIds;
  }

  public sealed class QuestionsDataQuestion
  {
    public int QuestionId;
    public bool IsEnabled;
    public string Question;
    public int Answers;
    public QuestionsDataJurisdiction[] Jurisdictions;
  }

  public sealed class QuestionsDataJurisdiction
  {
    public string IssueLevel;
    public string StateCode;
    public string CountyOrLocal;
  }

  public sealed class IssuesAndTopicsIssue
  {
    public int IssueId;
    public string Issue;
    public IList<IssuesAndTopicsQuestion> Questions;
    public bool Pruned;
  }

  public sealed class IssuesAndTopicsQuestion
  {
    public int QuestionId;
    public string Question;
    public IList<IssuesAndTopicsJurisdictions> Jurisdictions;
    public bool Pruned;
  }

  public sealed class IssuesAndTopicsJurisdictions
  {
    public string IssueLevel;
    public string StateCode;
    public string CountyOrLocal;
  }

  public sealed class IssuesAndTopicsPrunedIssue
  {
    public int I;
    public string Issue;
    public IList<IssuesAndTopicsPrunedQuestion> Q;
  }

  public sealed class IssuesAndTopicsRePrunedIssue
  {
    public int I;
    public IList<IssuesAndTopicsPrunedQuestion> Q;
  }

  public sealed class IssuesAndTopicsPrunedQuestion
  {
    public int I;
    public string Q;
  }

  // ReSharper restore NotAccessedField.Global

  public partial class Questions2
  {
    public static IList<IssuesAndTopicsIssue> GetIssuesAndTopics()
    {
      const string cmdText = "SELECT i.IssueId,i.Issue,q.QuestionId,q.Question,qj.IssueLevel,qj.StateCode,qj.CountyOrLocal" +
        " FROM Issues2 i" +
        " INNER JOIN IssuesQuestions iq ON iq.IssueId = i.IssueId " +
        " INNER JOIN Questions2 q ON q.QuestionId = iq.QuestionId" +
        " INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=q.QuestionId" +
        " WHERE i.IsIssueOmit=0 AND q.IsQuestionOmit = 0" +
        " ORDER BY i.IssueOrder,iq.QuestionOrder";

      var cmd = VoteDb.GetCommand(cmdText);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);

        return table.Rows.OfType<DataRow>()
          .GroupBy(r => r.IssueId())
          .Select(g =>
          new IssuesAndTopicsIssue
          {
            IssueId = g.Key,
            Issue = g.First().Issue(),
            Questions = g.GroupBy(r => r.QuestionId())
              .Select(g2 => 
                new IssuesAndTopicsQuestion
                {
                  QuestionId = g2.Key,
                  Question = g2.First().Question(),
                  Jurisdictions = g2.Select(r =>
                    new IssuesAndTopicsJurisdictions
                    {
                      IssueLevel = r.IssueLevel(),
                      StateCode = r.StateCode(),
                      CountyOrLocal = r.CountyOrLocal()
                    }).ToList()
                }).ToList()
          }).ToList();
      }
    }

    public static QuestionsData GetTopicsData(DateTime? minDate = null, DateTime? maxDate = null)
    {
      var result = new QuestionsData();

      if (minDate != null && (minDate.Value == DateTime.MinValue ||
        minDate.Value == VotePage.DefaultDbDate))
        minDate = null;

      if (maxDate != null && (maxDate.Value == DateTime.MaxValue ||
        maxDate.Value == VotePage.DefaultDbDate))
        maxDate = null;

      // get the topics
      var topicsCmdText =
        "SELECT q.QuestionId,q.Question,q.IsQuestionOmit,iq.IssueId,qj.IssueLevel,qj.StateCode,qj.CountyOrLocal," + 
        "IF(a.QuestionId IS NULL,0,COUNT(*)) AS Count FROM Questions2 q" + 
        " INNER JOIN IssuesQuestions iq ON iq.QuestionId=q.QuestionId" +
        " LEFT OUTER JOIN QuestionsJurisdictions qj ON qj.QuestionId=q.QuestionId" +
        //" INNER JOIN QuestionsJurisdictions qj ON qj.QuestionId=q.QuestionId" +
        " LEFT OUTER JOIN Answers2 a ON a.QuestionId=q.QuestionId" +
        $"{(minDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)>=@MinDate")}" +
        $"{(maxDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)<=@MaxDate")}" +
        " GROUP BY q.QuestionId,q.Question,q.IsQuestionOmit,iq.IssueId,qj.IssueLevel,qj.StateCode,qj.CountyOrLocal";
      var topicsCmd = VoteDb.GetCommand(topicsCmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        topicsCmd.Connection = cn;
        if (minDate != null)
          VoteDb.AddCommandParameter(topicsCmd, "MinDate", minDate.Value);
        if (maxDate != null)
          VoteDb.AddCommandParameter(topicsCmd, "MaxDate", maxDate.Value);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(topicsCmd as MySqlCommand);
        adapter.Fill(table);
        result.Questions = table.Rows.OfType<DataRow>()
          .GroupBy(r => r.QuestionId())
          .OrderBy(g => g.First().Question())
          .Select(g =>
          new QuestionsDataQuestion
          {
            QuestionId = g.First().QuestionId(),
            Question = g.First().Question(),
            IsEnabled = !g.First().IsQuestionOmit(),
            Answers = g.First().Count(),
            Jurisdictions = g
              .Where(i => i.IssueLevel() != null)
              .OrderBy(i => i.IssueLevel())
              .ThenBy(i => i.StateCode())
              .ThenBy(i => i.CountyOrLocal())
              .Select(j => new QuestionsDataJurisdiction
              {
                IssueLevel = j.IssueLevel(),
                StateCode = j.StateCode(),
                CountyOrLocal = j.CountyOrLocal()
              })
              .ToArray()
          }).ToArray();
      }

      // get the issues for the filter
      const string issuesCmdText = "SELECT i.IssueId,i.Issue,iq.QuestionId FROM Issues2 i" +
        " LEFT OUTER JOIN IssuesQuestions iq ON iq.IssueId=i.IssueId" +
        " ORDER BY IssueOrder";
      var issuesCmd = VoteDb.GetCommand(issuesCmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        issuesCmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(issuesCmd as MySqlCommand);
        adapter.Fill(table);
        result.Issues = table.Rows.OfType<DataRow>()
          .GroupBy(r => r.IssueId())
          .Select(g =>
          new QuestionsDataIssue
          {
            IssueId = g.First().IssueId(),
            Issue = g.First().Issue(),
            QuestionIds = g.Where(i => i.QuestionIdOrNull() != null).Select(i => i.QuestionId()).ToArray()
          }).ToArray();
      }

      return result;
    }

    //public static QuestionsData GetQuestionsData()
    //{
    //  var result = new QuestionsData();

    //  // get the Issues
    //  const string issuesCmdText =
    //    "SELECT i.IssueId,i.Issue,iq.QuestionId FROM Issues2 i" +
    //    " LEFT OUTER JOIN IssuesQuestions iq ON iq.IssueId=i.IssueId" +
    //    " LEFT OUTER JOIN Questions2 q ON q.QuestionId=iq.QuestionId" +
    //    " ORDER BY i.Issue,i.IssueId,iq.QuestionOrder";

    //  var issuesCmd = VoteDb.GetCommand(issuesCmdText);
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    issuesCmd.Connection = cn;
    //    var table = new DataTable();
    //    DbDataAdapter adapter = new MySqlDataAdapter(issuesCmd as MySqlCommand);
    //    adapter.Fill(table);
    //    //    result.Issues = table.Rows.OfType<DataRow>().GroupBy(r => r.IssueId()).Select(g =>
    //    //      new IssuesDataIssue
    //    //      {
    //    //        IssueId = g.First().IssueId(),
    //    //        IssueGroupId = g.First().IssueGroupId(),
    //    //        IsEnabled = !g.First().IsIssueOmit(),
    //    //        Issue = g.First().Issue(),
    //    //        QuestionIds = g.Where(q => q.QuestionIdOrNull() != null)
    //    //          .Select(q => q.QuestionId()).ToArray()
    //    //      }).ToArray();
    //  }

    //  //  // get the IssueGroups
    //  //  const string issueGroupsCmdText = "SELECT IssueGroupId,Heading FROM IssueGroups2";

    //  //  var issueGroupsCmd = VoteDb.GetCommand(issueGroupsCmdText);
    //  //  using (var cn = VoteDb.GetOpenConnection())
    //  //  {
    //  //    issueGroupsCmd.Connection = cn;
    //  //    var table = new DataTable();
    //  //    DbDataAdapter adapter = new MySqlDataAdapter(issueGroupsCmd as MySqlCommand);
    //  //    adapter.Fill(table);
    //  //    result.IssueGroups = table.Rows.OfType<DataRow>().Select(r =>
    //  //      new IssuesDataIssueGroup
    //  //      {
    //  //        IssueGroupId = r.IssueGroupId(),
    //  //        Heading = r.Heading()
    //  //      }).ToArray();
    //  //  }

    //  //  // get the questions
    //  //  const string questionsCmdText = "SELECT QuestionId,Question,IsQuestionOmit FROM Questions2" +
    //  //    " ORDER BY Question";

    //  //  var questionsCmd = VoteDb.GetCommand(questionsCmdText);
    //  //  using (var cn = VoteDb.GetOpenConnection())
    //  //  {
    //  //    questionsCmd.Connection = cn;
    //  //    var table = new DataTable();
    //  //    DbDataAdapter adapter = new MySqlDataAdapter(questionsCmd as MySqlCommand);
    //  //    adapter.Fill(table);
    //  //    result.Questions = table.Rows.OfType<DataRow>().Select(r =>
    //  //      new IssuesDataQuestion
    //  //      {
    //  //        QuestionId = r.QuestionId(),
    //  //        IsEnabled = !r.IsQuestionOmit(),
    //  //        Question = r.Question()
    //  //      }).ToArray();
    //  //  }

    //  return result;
    //}

    public static void SaveTopicsData(QuestionsData data)
    {
      var questionsTable = GetAllData();
      var questionsJurisdictionsTable = QuestionsJurisdictions.GetAllData();
      var issuesQuestionsTable = IssuesQuestions.GetAllData();

      // collect all QuestionsJurisdictions data 
      var questionsJurisdictionsData = data.Questions.SelectMany(q => q.Jurisdictions, (q, j) => new
      {
        q.QuestionId,
        j.IssueLevel,
        j.StateCode,
        j.CountyOrLocal
      }).ToArray();

      // this dictionary is used for adding IssuesQuestions entries for new questions
      var iqMaxOrderByIssue = issuesQuestionsTable.GroupBy(iq => iq.IssueId)
        .Select(g => new { IssueId = g.Key, MaxOrder = g.Max(iq => iq.QuestionOrder) })
        .ToDictionary(i => i.IssueId, i => i.MaxOrder);

      // delete any missing Questions rows
      foreach (var qRow in questionsTable)
        if (data.Questions.All(i => qRow.QuestionId != i.QuestionId))
          qRow.Delete();

      // delete any missing QuestionsJurisdictions rows
      foreach (var qjRow in questionsJurisdictionsTable)
      {
        var val = new
        {
          qjRow.QuestionId,
          qjRow.IssueLevel,
          qjRow.StateCode,
          qjRow.CountyOrLocal
        };
        if (questionsJurisdictionsData.All(i => i.QuestionId != val.QuestionId || i.IssueLevel != val.IssueLevel || i.StateCode != val.StateCode || i.CountyOrLocal != val.CountyOrLocal))
          qjRow.Delete();
      }

      // delete any missing IssuesQuestions rows
      foreach (var iqRow in issuesQuestionsTable)
      {
        var question =
          data.Questions.FirstOrDefault(i => i.QuestionId == iqRow.QuestionId);
        if (question == null)
          iqRow.Delete();
      }

      // update or add remaining Questions rows -- if new add an IssuesQuestions row too
      //var qOrder = 0;
      foreach (var q in data.Questions)
      {
        var qRow = questionsTable.Where(r => r.RowState != DataRowState.Deleted)
          .FirstOrDefault(r => r.QuestionId == q.QuestionId);

        if (qRow == null)
        {
          // new question -- insert directly because of auto increment column
          Insert(q.QuestionId, q.Question, !q.IsEnabled);

          // find the Issue with this QuestionId and add a row to IssuesQuestions
          foreach (var i in data.Issues)
            if (i.QuestionIds.Contains(q.QuestionId))
            {
              var maxOrder = iqMaxOrderByIssue[i.IssueId] + 10;
              iqMaxOrderByIssue[i.IssueId] = maxOrder;
              issuesQuestionsTable.AddRow(i.IssueId, q.QuestionId, maxOrder);
            }
        }
        else
        {
          if (qRow.Question != q.Question)
            qRow.Question = q.Question;
          if (qRow.IsQuestionOmit != !q.IsEnabled)
            qRow.IsQuestionOmit = !q.IsEnabled;
        }
      }

      // add new QuestionsJurisdictions rows
      foreach (var qj in questionsJurisdictionsData)
        if (questionsJurisdictionsTable.Where(r => r.RowState != DataRowState.Deleted).All(
          r => qj.QuestionId != r.QuestionId || qj.IssueLevel != r.IssueLevel || qj.StateCode != r.StateCode || qj.CountyOrLocal != r.CountyOrLocal))
          questionsJurisdictionsTable.AddRow(qj.QuestionId, qj.IssueLevel, qj.StateCode,
            qj.CountyOrLocal);

      UpdateTable(questionsTable);
      QuestionsJurisdictions.UpdateTable(questionsJurisdictionsTable);
      IssuesQuestions.UpdateTable(issuesQuestionsTable);
    }
  }
}