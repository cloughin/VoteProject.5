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
    private static void ProcessStoredState(DataTable table)
    {
      const string insertCmdText =
        "INSERT INTO state_raw" +
          "(area,bicameral,billUrl," +
          "bird,capital,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "flower,generalDate,highPoint,largestCity,lowerLegis," +
          "lowPoint,ltGov,motto,name,nickName,population," +
          "primaryDate,reps,rollLower,rollUpper,senators,statehood," +
          "stateId,stateType,termLimit,tree,upperLegis,usCircuit," +
          "voterReg,voteUrl)" +
          " VALUES (@area,@bicameral,@billUrl," +
          "@bird,@capital,@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@flower,@generalDate,@highPoint,@largestCity,@lowerLegis," +
          "@lowPoint,@ltGov,@motto,@name,@nickName,@population," +
          "@primaryDate,@reps,@rollLower,@rollUpper,@senators,@statehood," +
          "@stateId,@stateType,@termLimit,@tree,@upperLegis,@usCircuit," +
          "@voterReg,@voteUrl);";

      var fetchErrors = 0;
      var added = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("state"))
        {
          fetchErrors++;
          continue;
        }
        var state = jsonObj["state"] as Dictionary<string, object>;
        if (state == null || !state.ContainsKey("details"))
        {
          fetchErrors++;
          continue;
        }
        var details = state["details"] as Dictionary<string, object>;
        if (details == null)
        {
          fetchErrors++;
          continue;
        }

        var insertCmd = new MySqlCommand(insertCmdText);
        insertCmd.Parameters.AddWithValue("@area", details["area"]);
        insertCmd.Parameters.AddWithValue("@bicameral", details["bicameral"]);
        insertCmd.Parameters.AddWithValue("@billUrl", details["billUrl"]);
        insertCmd.Parameters.AddWithValue("@bird", details["bird"]);
        insertCmd.Parameters.AddWithValue("@capital", details["capital"]);
        insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
        insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
        insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
        insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
        insertCmd.Parameters.AddWithValue("@flower", details["flower"]);
        insertCmd.Parameters.AddWithValue("@generalDate", details["generalDate"]);
        insertCmd.Parameters.AddWithValue("@highPoint", details["highPoint"]);
        insertCmd.Parameters.AddWithValue("@largestCity", details["largestCity"]);
        insertCmd.Parameters.AddWithValue("@lowerLegis", details["lowerLegis"]);
        insertCmd.Parameters.AddWithValue("@lowPoint", details["lowPoint"]);
        insertCmd.Parameters.AddWithValue("@ltGov", details["ltGov"]);
        insertCmd.Parameters.AddWithValue("@motto", details["motto"]);
        insertCmd.Parameters.AddWithValue("@name", details["name"]);
        insertCmd.Parameters.AddWithValue("@nickName", details["nickName"]);
        insertCmd.Parameters.AddWithValue("@population", details["population"]);
        insertCmd.Parameters.AddWithValue("@primaryDate", details["primaryDate"]);
        insertCmd.Parameters.AddWithValue("@reps", details["reps"]);
        insertCmd.Parameters.AddWithValue("@rollLower", details["rollLower"]);
        insertCmd.Parameters.AddWithValue("@rollUpper", details["rollUpper"]);
        insertCmd.Parameters.AddWithValue("@senators", details["senators"]);
        insertCmd.Parameters.AddWithValue("@statehood", details["statehood"]);
        insertCmd.Parameters.AddWithValue("@stateId", details["stateId"]);
        insertCmd.Parameters.AddWithValue("@stateType", details["stateType"]);
        insertCmd.Parameters.AddWithValue("@termLimit", details["termLimit"]);
        insertCmd.Parameters.AddWithValue("@tree", details["tree"]);
        insertCmd.Parameters.AddWithValue("@upperLegis", details["upperLegis"]);
        insertCmd.Parameters.AddWithValue("@usCircuit", details["usCircuit"]);
        insertCmd.Parameters.AddWithValue("@voterReg", details["voterReg"]);
        insertCmd.Parameters.AddWithValue("@voteUrl", details["voteUrl"]);
        using (var cn = GetOpenConnection())
        {
          insertCmd.Connection = cn;
          try
          {
            insertCmd.ExecuteNonQuery();
            added++;
          }
          catch (MySqlException ex)
          {
            if (ex.Message.StartsWith(("Duplicate entry"), StringComparison.Ordinal))
            {
              duplicates++;
              continue;
            }
            throw;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} states added. {3} duplicate states.",
          table.Rows.Count - fetchErrors, fetchErrors, added, duplicates));
    }
  }
}
