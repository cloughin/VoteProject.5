using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace DB.Vote
{
  // ReSharper disable NotAccessedField.Global
  public sealed class IssueGroupsData
  {
    public int IssueGroupId;
    public bool IsEnabled;
    public string Heading;
    public string SubHeading;
    public IssueGroupsDataIssues[] Issues;
  }

  public sealed class IssueGroupsDataIssues
  {
    public int IssueId;
    public bool IsEnabled;
    public string Issue;
    public int Topics;
  }
  // ReSharper restore NotAccessedField.Global

  public partial class IssueGroups2
  {
    public static IssueGroupsData[] GetIssueGroupsData()
    {
      const string cmdText =
        "SELECT ig.IssueGroupId,ig.IsEnabled,ig.Heading,ig.SubHeading,i.IssueId,i.IsIssueOmit,i.Issue," +
        "IF(iq.IssueId IS NULL,0,COUNT(*)) AS Count" +
        " FROM IssueGroups2 ig" +
        " LEFT OUTER JOIN IssueGroupsIssues2 igi ON igi.IssueGroupId=ig.IssueGroupId" +
        " LEFT OUTER JOIN Issues2 i ON i.IssueId=igi.IssueId" +
        " LEFT OUTER JOIN IssuesQuestions iq on iq.IssueId=i.IssueId" +
        " GROUP BY ig.IssueGroupId,i.IssueId" +
        " ORDER BY ig.IssueGroupOrder,igi.IssueOrder";

      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().GroupBy(r => r.IssueGroupId()).Select(g =>
          new IssueGroupsData
          {
            IssueGroupId = g.First().IssueGroupId(),
            IsEnabled = g.First().IsEnabled(),
            Heading = g.First().Heading(),
            SubHeading = g.First().SubHeading(),
            Issues = g.Where(i => i.Issue() != null).Select(i => new IssueGroupsDataIssues
            {
              IssueId = i.IssueId(),
              IsEnabled = !i.IsIssueOmit(),
              Issue = i.Issue(),
              Topics = i.Count()
            }).ToArray()
          }).ToArray();
      }
    }

    public static void SaveIssueGroupsData(IssueGroupsData[] data)
    {
      var issueGroupsTable = GetAllData();
      var issueGroupsIssuesTable = IssueGroupsIssues2.GetAllData();

      // delete any missing IssueGroups rows  
      foreach (var igRow in issueGroupsTable)
        if (data.All(ig => igRow.IssueGroupId != ig.IssueGroupId))
          igRow.Delete();

      // delete any missing IssueGroupsIssues rows  
      foreach (var igiRow in issueGroupsIssuesTable)
      {
        var issueGroup =
          data.FirstOrDefault(ig => ig.IssueGroupId == igiRow.IssueGroupId);
        if (issueGroup == null || issueGroup.Issues.All(igi => igi.IssueId != igiRow.IssueId))
          igiRow.Delete();
      }

      // update or add remaining entries
      var igOrder = 0;
      foreach (var ig in data)
      {
        igOrder += 10;
        var igRow = issueGroupsTable.Where(r => r.RowState != DataRowState.Deleted)
          .FirstOrDefault(r => r.IssueGroupId == ig.IssueGroupId);
        if (igRow == null)
        {
          //igRow = issueGroupsTable.NewRow(ig.IssueGroupId, igOrder, ig.IsEnabled,
          //  ig.Heading, ig.SubHeading);
          //issueGroupsTable.AddRow(igRow);
          // insert directly because of auto increment column
          Insert(ig.IssueGroupId, igOrder, ig.IsEnabled, ig.Heading,
            ig.SubHeading);
        }
        else
        {
          if (igRow.IssueGroupOrder != igOrder)
            igRow.IssueGroupOrder = igOrder;
          if (igRow.IsEnabled != ig.IsEnabled)
            igRow.IsEnabled = ig.IsEnabled;
          if (igRow.Heading != ig.Heading)
            igRow.Heading = ig.Heading;
          if (igRow.SubHeading != ig.SubHeading)
            igRow.SubHeading = ig.SubHeading;
        }

        var igiOrder = 0;
        foreach (var igi in ig.Issues)
        {
          igiOrder += 10;
          var igiRow = issueGroupsIssuesTable.Where(r => r.RowState != DataRowState.Deleted)
            .FirstOrDefault(r => r.IssueGroupId == ig.IssueGroupId && r.IssueId == igi.IssueId);
          if (igiRow == null)
          {
            igiRow = issueGroupsIssuesTable.NewRow(ig.IssueGroupId, igi.IssueId, igiOrder);
            issueGroupsIssuesTable.AddRow(igiRow);
          }
          else
            if (igiRow.IssueOrder != igiOrder)
            igiRow.IssueOrder = igiOrder;
        }
      }

      UpdateTable(issueGroupsTable);
      IssueGroupsIssues2.UpdateTable(issueGroupsIssuesTable);
    }
  }
}