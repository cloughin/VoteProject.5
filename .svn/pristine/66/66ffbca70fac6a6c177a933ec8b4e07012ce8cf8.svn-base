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
    private static void ProcessStoredLeadershipOfficials(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM leadership_officials_raw " +
        "WHERE stateId=@stateId AND leadershipId=@leadershipId AND" +
        " officeId=@officeId AND candidateId=@candidateId";

      const string insertCmdText =
        "INSERT INTO leadership_officials_raw" +
          "(candidateId,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "firstName,lastName,leadershipId,middleName,officeId,position," +
          "stateId,suffix,title)" +
          " VALUES (@candidateId,@fetchId,@fetchMethod,@fetchParameters," +
          "@fetchTime,@firstName,@lastName,@leadershipId,@middleName," +
          "@officeId,@position,@stateId,@suffix,@title);";

      var fetchErrors = 0;
      var officialsAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var leadershipId = ParseIdFromParameters(row["fetch_parameters"], "leadershipId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("leaders"))
        {
          fetchErrors++;
          continue;
        }
        var leaders = jsonObj["leaders"] as Dictionary<string, object>;
        if (leaders == null || !leaders.ContainsKey("leader"))
        {
          fetchErrors++;
          continue;
        }
        var leader = AsArrayList(leaders["leader"]);
        if (leader == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var l in leader.Cast<Dictionary<string, object>>())
        {
          var officeId = Convert.ToInt32(l["officeId"]);
          var candidateId = Convert.ToInt32(l["candidateId"]);
          // skip duplicate official
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@stateId", stateId);
          countCmd.Parameters.AddWithValue("@leadershipId", leadershipId);
          countCmd.Parameters.AddWithValue("@officeId", officeId);
          countCmd.Parameters.AddWithValue("@candidateId", candidateId);
          int leadershipCount;
          using (var cn = GetOpenConnection())
          {
            countCmd.Connection = cn;
            leadershipCount = Convert.ToInt32(countCmd.ExecuteScalar());
          }
          if (leadershipCount != 0)
          {
            duplicates++;
            continue;
          }

          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@candidateId", candidateId);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@firstName", l["firstName"]);
          insertCmd.Parameters.AddWithValue("@lastName", l["lastName"]);
          insertCmd.Parameters.AddWithValue("@leadershipId", leadershipId);
          insertCmd.Parameters.AddWithValue("@middleName", l["middleName"]);
          insertCmd.Parameters.AddWithValue("@officeId", officeId);
          insertCmd.Parameters.AddWithValue("@position", l["position"]);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          insertCmd.Parameters.AddWithValue("@suffix", l["suffix"]);
          insertCmd.Parameters.AddWithValue("@title", l["title"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            insertCmd.ExecuteNonQuery();
            officialsAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} officials added, {3} duplicates.",
          table.Rows.Count - fetchErrors, fetchErrors, officialsAdded,
          duplicates));
    }
  }
}
