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
    private static void ProcessStoredElections(DataTable table)
    {
      const string countElectionCmdText = "SELECT COUNT(*) FROM election_raw " +
        "WHERE electionId=@electionId";

      const string countStageCmdText = "SELECT COUNT(*) FROM election_stage_raw " +
        "WHERE electionId=@electionId AND stageId=@stageId";

      const string countRosterCmdText = "SELECT COUNT(*) FROM fetches_raw " +
        "WHERE fetch_method=@method AND fetch_parameters=@parameters";

      const string insertElectionCmdText =
        "INSERT INTO election_raw" +
          "(electionId,electionYear,fetch_id,fetch_method,fetch_parameters," +
          "fetch_time,name,officeTypeId,special,stateId)" +
          " VALUES (@electionId,@electionYear,@fetchId,@fetchMethod," +
          "@fetchParameters,@fetchTime,@name,@officeTypeId," +
          "@special,@stateId);";

      const string insertStageCmdText =
        "INSERT INTO election_stage_raw" +
          "(electionDate,electionId,fetch_id,fetch_method,fetch_parameters," +
          "fetch_time,filingDeadline,name,npatMailed,stageId,stateId)" +
          " VALUES (@electionDate,@electionId,@fetchId,@fetchMethod," +
          "@fetchParameters,@fetchTime,@filingDeadline,@name,@npatMailed," +
          "@stageId,@stateId);";

      const string rosterMethod = "Election.getStageCandidates";

      var fetchErrors = 0;
      var electionsAdded = 0;
      var duplicateElections = 0;
      var stagesAdded = 0;
      var duplicateStages = 0;
      var rostersAdded = 0;
      var duplicateRosters = 0;
      var electionStatesAdded = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("elections"))
        {
          fetchErrors++;
          continue;
        }
        var elections = jsonObj["elections"] as Dictionary<string, object>;
        if (elections == null || !elections.ContainsKey("election"))
        {
          fetchErrors++;
          continue;
        }
        var election = AsArrayList(elections["election"]);
        if (election == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var e in election.Cast<Dictionary<string, object>>())
        {
          var electionId = Convert.ToInt32(e["electionId"]);
          if (!e.ContainsKey("stage"))
          {
            fetchErrors++;
            continue;
          }
          var stage = AsArrayList(e["stage"]);
          if (stage == null)
          {
            fetchErrors++;
            continue;
          }
          foreach (var s in stage.Cast<Dictionary<string, object>>())
          {
            var stageId = s["stageId"];
            // skip duplicate roster
            var rosterParameters = "electionId=" + electionId + "&stageId=" +
              stageId;
            var countRosterCmd = new MySqlCommand(countRosterCmdText);
            countRosterCmd.Parameters.AddWithValue("@method", rosterMethod);
            countRosterCmd.Parameters.AddWithValue("@parameters", rosterParameters);
            int rosterCount;
            using (var cn = GetOpenConnection())
            {
              countRosterCmd.Connection = cn;
              rosterCount = Convert.ToInt32(countRosterCmd.ExecuteScalar());
            }
            if (rosterCount != 0)
              duplicateRosters++;
            else
            {
              SaveRawData(rosterMethod, rosterParameters);
              rostersAdded++;
            }

            // skip duplicate stage
            var countStageCmd = new MySqlCommand(countStageCmdText);
            countStageCmd.Parameters.AddWithValue("@electionId", electionId);
            countStageCmd.Parameters.AddWithValue("@stageId", stageId);
            int stageCount;
            using (var cn = GetOpenConnection())
            {
              countStageCmd.Connection = cn;
              stageCount = Convert.ToInt32(countStageCmd.ExecuteScalar());
            }
            if (stageCount != 0)
            {
              duplicateStages++;
              continue;
            }

            var insertStageCmd = new MySqlCommand(insertStageCmdText);
            insertStageCmd.Parameters.AddWithValue("@electionDate", s["electionDate"]);
            insertStageCmd.Parameters.AddWithValue("@electionId", electionId);
            insertStageCmd.Parameters.AddWithValue("@fetchId", row["id"]);
            insertStageCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
            insertStageCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
            insertStageCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
            insertStageCmd.Parameters.AddWithValue("@filingDeadline", s["filingDeadline"]);
            insertStageCmd.Parameters.AddWithValue("@name", s["name"]);
            insertStageCmd.Parameters.AddWithValue("@npatMailed", s["npatMailed"]);
            insertStageCmd.Parameters.AddWithValue("@stageId", s["stageId"]);
            insertStageCmd.Parameters.AddWithValue("@stateId", s["stateId"]);
            using (var cn = GetOpenConnection())
            {
              insertStageCmd.Connection = cn;
              insertStageCmd.ExecuteNonQuery();
              stagesAdded++;
            }
          }

          const string insertElectionsStates =
            "INSERT INTO elections_states " +
            "(electionId,stateId) VALUES (@electionId,@stateId);";
          var insertElectionsStatesCmd = new MySqlCommand(insertElectionsStates);
          insertElectionsStatesCmd.Parameters.AddWithValue("@electionId", electionId);
          insertElectionsStatesCmd.Parameters.AddWithValue("@stateId", stateId);
          using (var cn = GetOpenConnection())
          {
            insertElectionsStatesCmd.Connection = cn;
            try
            {
              insertElectionsStatesCmd.ExecuteNonQuery();
              electionStatesAdded++;
            }
            catch (MySqlException ex)
            {
              if (!ex.Message.StartsWith(("Duplicate entry"),
                StringComparison.Ordinal)) throw;
            }
          }

          // skip duplicate election
          var countElectionCmd = new MySqlCommand(countElectionCmdText);
          countElectionCmd.Parameters.AddWithValue("@electionId", electionId);
          int electionCount;
          using (var cn = GetOpenConnection())
          {
            countElectionCmd.Connection = cn;
            electionCount = Convert.ToInt32(countElectionCmd.ExecuteScalar());
          }
          if (electionCount != 0)
          {
            duplicateElections++;
            continue;
          }

          var insertElectionCmd = new MySqlCommand(insertElectionCmdText);
          insertElectionCmd.Parameters.AddWithValue("@electionId", electionId);
          insertElectionCmd.Parameters.AddWithValue("@electionYear", e["electionYear"]);
          insertElectionCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertElectionCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertElectionCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertElectionCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertElectionCmd.Parameters.AddWithValue("@name", e["name"]);
          insertElectionCmd.Parameters.AddWithValue("@officeTypeId", e["officeTypeId"]);
          insertElectionCmd.Parameters.AddWithValue("@special", e["special"]);
          insertElectionCmd.Parameters.AddWithValue("@stateId", e["stateId"]);
          using (var cn = GetOpenConnection())
          {
            insertElectionCmd.Connection = cn;
            insertElectionCmd.ExecuteNonQuery();
            electionsAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} elections added, {3} duplicate elections," +
            " {4} stages added, {5} duplicate stages," +
            " {6} rosters added, {7} duplicate rosters," +
            " {8} elections_states added.",
          table.Rows.Count - fetchErrors, fetchErrors, electionsAdded,
          duplicateElections, stagesAdded, duplicateStages,
          rostersAdded, duplicateRosters, electionStatesAdded));
    }
  }
}
