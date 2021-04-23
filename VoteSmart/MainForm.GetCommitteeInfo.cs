using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  public partial class MainForm
  {
    private static void GetCommitteeInfo(DataTable table)
    {
      var fetchErrors = 0;
      var committeesFetched = 0;
      var committeeRostersFetched = 0;
      var committeeStatesAdded = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("committees"))
        {
          fetchErrors++;
          continue;
        }
        var committees = jsonObj["committees"] as Dictionary<string, object>;
        if (committees == null || !committees.ContainsKey("committee"))
        {
          fetchErrors++;
          continue;
        }
        var committee = AsArrayList(committees["committee"]);
        if (committee == null)
        {
          fetchErrors++;
          continue;
        }
        const string committeeMethod = "Committee.getCommittee";
        const string committeeMembersMethod = "Committee.getCommitteeMembers";
        foreach (var c in committee.OfType<Dictionary<string, object>>())
        {
          var committeeId = Convert.ToInt32(c["committeeId"]);
          var parameters = "committeeId=" + committeeId;

          // only fetch committee info if not already there
          const string countCmdText = "SELECT COUNT(*) FROM fetches_raw " +
            "WHERE fetch_method=@method AND fetch_parameters=@parameters";
          var committeeCountCmd = new MySqlCommand(countCmdText);
          committeeCountCmd.Parameters.AddWithValue("@method", committeeMethod);
          committeeCountCmd.Parameters.AddWithValue("@parameters", parameters);
          int committeeCount;
          using (var cn = GetOpenConnection())
          {
            committeeCountCmd.Connection = cn;
            committeeCount = Convert.ToInt32(committeeCountCmd.ExecuteScalar());
          }
          if (committeeCount == 0)
          {
            SaveRawData(committeeMethod, parameters);
            committeesFetched++;
          }

          var committeeMembersCountCmd = new MySqlCommand(countCmdText);
          committeeMembersCountCmd.Parameters.AddWithValue("@method", committeeMembersMethod);
          committeeMembersCountCmd.Parameters.AddWithValue("@parameters", parameters);
          int committeeMembersCount;
          using (var cn = GetOpenConnection())
          {
            committeeMembersCountCmd.Connection = cn;
            committeeMembersCount = Convert.ToInt32(committeeMembersCountCmd.ExecuteScalar());
          }
          if (committeeMembersCount == 0)
          {
            SaveRawData(committeeMembersMethod, parameters);
            committeeRostersFetched++;
          }

          const string insertCommitteesStates =
            "INSERT INTO committees_states " + 
            "(committeeId,stateId) VALUES (@committeeId,@stateId);";
          var insertCmd = new MySqlCommand(insertCommitteesStates);
          insertCmd.Parameters.AddWithValue("@committeeId", committeeId);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            try
            {
              insertCmd.ExecuteNonQuery();
              committeeStatesAdded++;
            }
            catch (MySqlException ex)
            {
              if (!ex.Message.StartsWith(("Duplicate entry"),
                StringComparison.Ordinal)) throw;
            }
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} committees fetched, {3} committee rosters fetched," +
            " {4} committees_states added.",
          table.Rows.Count - fetchErrors, fetchErrors, committeesFetched,
          committeeRostersFetched, committeeStatesAdded));
    }
  }
}
