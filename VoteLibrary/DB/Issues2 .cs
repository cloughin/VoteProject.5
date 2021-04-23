using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global
  public sealed class IssuesData
  {
    public IssuesDataIssue[] Issues;
    public IssuesDataIssueGroup[] IssueGroups;
    public IssuesDataQuestion[] Questions;
  }

  public sealed class IssuesDataIssue
  {
    public int IssueId;
    public int IssueGroupId;
    public bool IsEnabled;
    public string Issue;
    public int[] QuestionIds;
    public int Answers;
  }

  public sealed class IssuesDataIssueGroup
  {
    public int IssueGroupId;
    public string Heading;
  }

  public sealed class IssuesDataQuestion
  {
    public int QuestionId;
    public bool IsEnabled;
    public string Question;
    public int Answers;
  }
  // ReSharper restore NotAccessedField.Global

  public partial class Issues2
  {
    public static IssuesData GetIssuesData(DateTime? minDate = null, DateTime? maxDate = null)
    {
      var result = new IssuesData();

      if (minDate != null && (minDate.Value == DateTime.MinValue ||
        minDate.Value == VotePage.DefaultDbDate))
        minDate = null;

      if (maxDate != null && (maxDate.Value == DateTime.MaxValue ||
        maxDate.Value == VotePage.DefaultDbDate))
        maxDate = null;

      // get the Issues
      var issuesCmdText =
        "SELECT i.IssueId,i.Issue,i.IsIssueOmit,igi.IssueGroupId,iq.QuestionId," +
        "IF(a.QuestionId IS NULL,0,COUNT(*)) AS Count FROM Issues2 i" + 
        " INNER JOIN IssueGroupsIssues2 igi ON igi.IssueId=i.IssueId" +
        " LEFT OUTER JOIN IssuesQuestions iq ON iq.IssueId=i.IssueId" +
        " LEFT OUTER JOIN Answers2 a ON a.QuestionId=iq.questionId" +
        $"{(minDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)>=@MinDate")}" +
        $"{(maxDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)<=@MaxDate")}" +
        " GROUP BY i.IssueId,iq.QuestionId" +
        " ORDER BY i.IssueOrder,iq.QuestionOrder";

      var issuesCmd = VoteDb.GetCommand(issuesCmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        issuesCmd.Connection = cn;
        if (minDate != null)
          VoteDb.AddCommandParameter(issuesCmd, "MinDate", minDate.Value);
        if (maxDate != null)
          VoteDb.AddCommandParameter(issuesCmd, "MaxDate", maxDate.Value);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(issuesCmd as MySqlCommand);
        adapter.Fill(table);
        result.Issues = table.Rows.OfType<DataRow>().GroupBy(r => r.IssueId()).Select(g =>
          new IssuesDataIssue
          {
            IssueId = g.First().IssueId(),
            IssueGroupId = g.First().IssueGroupId(),
            IsEnabled = !g.First().IsIssueOmit(),
            Issue = g.First().Issue(),
            QuestionIds = g.Where(q => q.QuestionIdOrNull() != null)
              .Select(q => q.QuestionId()).ToArray(),
            Answers = g.Select(q => q.Count()).Sum()
          }).ToArray();
      }

      // get the IssueGroups
      const string issueGroupsCmdText = "SELECT IssueGroupId,Heading FROM IssueGroups2";

      var issueGroupsCmd = VoteDb.GetCommand(issueGroupsCmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        issueGroupsCmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(issueGroupsCmd as MySqlCommand);
        adapter.Fill(table);
        result.IssueGroups = table.Rows.OfType<DataRow>().Select(r =>
          new IssuesDataIssueGroup
          {
            IssueGroupId = r.IssueGroupId(),
            Heading = r.Heading()
          }).ToArray();
      }

      // get the questions
      var questionsCmdText = "SELECT q.QuestionId,q.Question,q.IsQuestionOmit," +
        "IF(a.QuestionId IS NULL,0,COUNT(*)) AS Count" +
        " FROM Questions2 q" +
        " LEFT OUTER JOIN Answers2 a ON a.QuestionId=q.questionId" +
        $"{(minDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)>=@MinDate")}" +
        $"{(maxDate == null ? Empty : " AND GREATEST(DateStamp,YouTubeDate)<=@MaxDate")}" +
        " GROUP BY q.QuestionId" +
        " ORDER BY Question";

      var questionsCmd = VoteDb.GetCommand(questionsCmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        questionsCmd.Connection = cn;
        if (minDate != null)
          VoteDb.AddCommandParameter(questionsCmd, "MinDate", minDate.Value);
        if (maxDate != null)
          VoteDb.AddCommandParameter(questionsCmd, "MaxDate", maxDate.Value);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(questionsCmd as MySqlCommand);
        adapter.Fill(table);
        result.Questions = table.Rows.OfType<DataRow>().Select(r =>
          new IssuesDataQuestion
          {
            QuestionId = r.QuestionId(),
            IsEnabled = !r.IsQuestionOmit(),
            Question = r.Question(),
            Answers = r.Count()
          }).ToArray();
      }

      return result;
    }

    public static void SaveIssuesData(IssuesDataIssue[] data)
    {
      var issuesTable = GetAllData();
      var issueGroupsIssuesTable = IssueGroupsIssues2.GetAllData();
      var issuesQuestionsTable = IssuesQuestions.GetAllData();

      // this dictionary is used for adding IssueGroupsIssues entries for new issues
      var igiMaxOrderByIssueGroup = issueGroupsIssuesTable.GroupBy(igi => igi.IssueGroupId)
        .Select(g => new {IssueGroupId = g.Key, MaxOrder = g.Max(igi => igi.IssueOrder)})
        .ToDictionary(ig => ig.IssueGroupId, ig => ig.MaxOrder);

      // delete any missing Issues rows
      foreach (var iRow in issuesTable)
        if (data.All(i => iRow.IssueId != i.IssueId))
          iRow.Delete();

      // delete any missing IssueGroupsIssues rows
      foreach (var igiRow in issueGroupsIssuesTable)
      {
        var issue =
          data.FirstOrDefault(i => i.IssueId == igiRow.IssueId);
        if (issue == null)
          igiRow.Delete();
      }

      // delete any missing IssuesQuestions rows  
      foreach (var iqRow in issuesQuestionsTable)
      {
        var issue =
          data.FirstOrDefault(i => i.IssueId == iqRow.IssueId);
        if (issue == null || issue.QuestionIds.All(iq => iq != iqRow.QuestionId))
          iqRow.Delete();
      }

      // update or add remaining entries
      var iOrder = 0;
      foreach (var i in data)
      {
        iOrder += 10;
        var iRow = issuesTable.Where(r => r.RowState != DataRowState.Deleted)
          .FirstOrDefault(r => r.IssueId == i.IssueId);

        // Issues
        if (iRow == null)
        {
          // insert directly because of auto increment column
          Insert(i.IssueId, iOrder, i.Issue, !i.IsEnabled);

          // new issues are added to IssueGroupsIssues at the end of the sequence by issue group
          var maxOrder = igiMaxOrderByIssueGroup[i.IssueGroupId] + 10;
          igiMaxOrderByIssueGroup[i.IssueGroupId] = maxOrder;
          var igiRow = issueGroupsIssuesTable.NewRow(i.IssueGroupId, i.IssueId, maxOrder);
          issueGroupsIssuesTable.AddRow(igiRow);
        }
        else
        {
          if (iRow.IssueOrder != iOrder)
            iRow.IssueOrder = iOrder;
          if (iRow.Issue != i.Issue)
            iRow.Issue = i.Issue;
          if (iRow.IsIssueOmit != !i.IsEnabled)
            iRow.IsIssueOmit = !i.IsEnabled;
        }

        var iqOrder = 0;
        foreach (var iq in i.QuestionIds)
        {
          iqOrder += 10;
          var iqRow = issuesQuestionsTable.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.IssueId == i.IssueId && r.QuestionId == iq);
          if (iqRow == null)
          {
            iqRow = issuesQuestionsTable.NewRow(i.IssueId, iq, iqOrder);
            issuesQuestionsTable.AddRow(iqRow);
          }
          else
          if (iqRow.QuestionOrder != iqOrder)
            iqRow.QuestionOrder = iqOrder;
        }
      }

      UpdateTable(issuesTable);
      IssueGroupsIssues2.UpdateTable(issueGroupsIssuesTable);
      IssuesQuestions.UpdateTable(issuesQuestionsTable);
    }
  }
}