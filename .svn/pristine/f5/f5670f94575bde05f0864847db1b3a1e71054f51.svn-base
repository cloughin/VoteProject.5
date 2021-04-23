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
    private static void ProcessStoredElectionRosters(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM election_stage_candidates_raw " +
        "WHERE electionId=@electionId AND stageId=@stageId" +
        " AND candidateId=@candidateId";

      const string insertCmdText =
        "INSERT INTO election_stage_candidates_raw" +
          "(candidateId,district,electionId,fetch_id,fetch_method," +
          "fetch_parameters,fetch_time,firstName,lastName,middleName," +
          "officeId,party,stageId,status,suffix,voteCount,votePercent)" +
          " VALUES (@candidateId,@district,@electionId,@fetchId," +
          "@fetchMethod,@fetchParameters,@fetchTime,@firstName,@lastName," +
          "@middleName,@officeId,@party,@stageId,@status,@suffix," +
          "@voteCount,@votePercent);";

      var stageDictionary = GetReverseStageDictionary();

      var fetchErrors = 0;
      var candidatesAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("stageCandidates"))
        {
          fetchErrors++;
          continue;
        }
        var stageCandidates = jsonObj["stageCandidates"] as Dictionary<string, object>;
        if (stageCandidates == null || !stageCandidates.ContainsKey("election") ||
          !stageCandidates.ContainsKey("candidate"))
        {
          fetchErrors++;
          continue;
        }
        var election = stageCandidates["election"] as Dictionary<string, object>;
        var candidate = AsArrayList(stageCandidates["candidate"]);
        if (election == null || candidate == null)
        {
          fetchErrors++;
          continue;
        }
        var electionId = Convert.ToInt32(election["electionId"]);
        // ReSharper disable once AssignNullToNotNullAttribute
        var stageId = stageDictionary[election["stage"] as string];
        foreach (var c in candidate.Cast<Dictionary<string, object>>())
        {
          var candidateId = Convert.ToInt32(c["candidateId"]);

          // skip duplicate candidate
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@electionId", electionId);
          countCmd.Parameters.AddWithValue("@stageId", stageId);
          countCmd.Parameters.AddWithValue("@candidateId", candidateId);
          int count;
          using (var cn = GetOpenConnection())
          {
            countCmd.Connection = cn;
            count = Convert.ToInt32(countCmd.ExecuteScalar());
          }
          if (count != 0)
          {
            duplicates++;
            continue;
          }

          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@candidateId", candidateId);
          insertCmd.Parameters.AddWithValue("@district", c["district"]);
          insertCmd.Parameters.AddWithValue("@electionId", electionId);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@firstName", c["firstName"]);
          insertCmd.Parameters.AddWithValue("@lastName", c["lastName"]);
          insertCmd.Parameters.AddWithValue("@middleName", c["middleName"]);
          insertCmd.Parameters.AddWithValue("@officeId", c["officeId"]);
          insertCmd.Parameters.AddWithValue("@party", c["party"]);
          insertCmd.Parameters.AddWithValue("@stageId", stageId);
          insertCmd.Parameters.AddWithValue("@status", c["status"]);
          insertCmd.Parameters.AddWithValue("@suffix", c["suffix"]);
          insertCmd.Parameters.AddWithValue("@voteCount", c["voteCount"]);
          insertCmd.Parameters.AddWithValue("@votePercent", c["votePercent"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            insertCmd.ExecuteNonQuery();
            candidatesAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} candidates added, {3} duplicates.",
          table.Rows.Count - fetchErrors, fetchErrors, candidatesAdded,
          duplicates));
    }
  }
}
