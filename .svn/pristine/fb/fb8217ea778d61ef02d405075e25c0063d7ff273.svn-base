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
    private static void ProcessStoredLeadership(DataTable table)
    {
      const string countLeadershipCmdText = "SELECT COUNT(*) FROM leadership_raw " +
        "WHERE stateId=@stateId AND leadershipId=@leadershipId AND" +
        " officeId=@officeId";

      const string countOfficialsCmdText = "SELECT COUNT(*) FROM fetches_raw " +
        "WHERE fetch_method=@method AND fetch_parameters=@parameters";

      const string insertCmdText =
        "INSERT INTO leadership_raw" +
          "(fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "leadershipId,name,officeId,officeName,stateId)" +
          " VALUES (@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@leadershipId,@name,@officeId,@officeName,@stateId);";

      const string officialsMethod = "Leadership.getOfficials";

      var fetchErrors = 0;
      var positionsAdded = 0;
      var duplicatePositions = 0;
      var officialsAdded = 0;
      var duplicateOfficials = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("leadership"))
        {
          fetchErrors++;
          continue;
        }
        var leadership = jsonObj["leadership"] as Dictionary<string, object>;
        if (leadership == null || !leadership.ContainsKey("position"))
        {
          fetchErrors++;
          continue;
        }
        var position = AsArrayList(leadership["position"]);
        if (position == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var p in position.Cast<Dictionary<string, object>>())
        {
          var leadershipId = Convert.ToInt32(p["leadershipId"]);
          var officeId = Convert.ToInt32(p["officeId"]);
          // skip duplicate officials
          var officialsParameters = "leadershipId=" + leadershipId + "&stateId=" +
            stateId;
          var countOfficialsCmd = new MySqlCommand(countOfficialsCmdText);
          countOfficialsCmd.Parameters.AddWithValue("@method", officialsMethod);
          countOfficialsCmd.Parameters.AddWithValue("@parameters", officialsParameters);
          int rosterCount;
          using (var cn = GetOpenConnection())
          {
            countOfficialsCmd.Connection = cn;
            rosterCount = Convert.ToInt32(countOfficialsCmd.ExecuteScalar());
          }
          if (rosterCount != 0)
            duplicateOfficials++;
          else
          {
            SaveRawData(officialsMethod, officialsParameters);
            officialsAdded++;
          }

          // skip duplicate position
          var countLeadershipCmd = new MySqlCommand(countLeadershipCmdText);
          countLeadershipCmd.Parameters.AddWithValue("@stateId", stateId);
          countLeadershipCmd.Parameters.AddWithValue("@leadershipId", leadershipId);
          countLeadershipCmd.Parameters.AddWithValue("@officeId", officeId);
          int leadershipCount;
          using (var cn = GetOpenConnection())
          {
            countLeadershipCmd.Connection = cn;
            leadershipCount = Convert.ToInt32(countLeadershipCmd.ExecuteScalar());
          }
          if (leadershipCount != 0)
          {
            duplicatePositions++;
            continue;
          }

          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@leadershipId", leadershipId);
          insertCmd.Parameters.AddWithValue("@name", p["name"]);
          insertCmd.Parameters.AddWithValue("@officeId", officeId);
          insertCmd.Parameters.AddWithValue("@officeName", p["officeName"]);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            insertCmd.ExecuteNonQuery();
            positionsAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} positions added, {3} duplicate positions," +
            " {4} officials lists added. {5} duplicate officials lists.",
          table.Rows.Count - fetchErrors, fetchErrors, positionsAdded,
          duplicatePositions, officialsAdded, duplicateOfficials));
    }
  }
}
