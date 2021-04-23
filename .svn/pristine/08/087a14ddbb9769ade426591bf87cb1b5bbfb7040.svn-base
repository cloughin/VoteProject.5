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
    private static void ProcessStoredBallotMeasures(DataTable table)
    {
      const string insertCmdText =
        "INSERT INTO measures_raw" +
          "(conUrl,electionDate,electionType,fetch_id,fetch_method," +
          "fetch_parameters,fetch_time,measureCode,measureId,measureText," +
          "no,outcome,proUrl,source,summary,summaryUrl,textUrl,title,url," +
          "yes)" +
          " VALUES (@conUrl,@electionDate,@electionType,@fetchId," +
          "@fetchMethod,@fetchParameters,@fetchTime,@measureCode," +
          "@measureId,@measureText,@no,@outcome,@proUrl,@source," +
          "@summary,@summaryUrl,@textUrl,@title,@url,@yes);";

      var fetchErrors = 0;
      var added = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("measure"))
        {
          fetchErrors++;
          continue;
        }
        var measure = jsonObj["measure"] as Dictionary<string, object>;
        if (measure == null)
        {
          fetchErrors++;
          continue;
        }

        var insertCmd = new MySqlCommand(insertCmdText);
        insertCmd.Parameters.AddWithValue("@conUrl", measure["conUrl"]);
        insertCmd.Parameters.AddWithValue("@electionDate", measure["electionDate"]);
        insertCmd.Parameters.AddWithValue("@electionType", measure["electionType"]);
        insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
        insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
        insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
        insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
        insertCmd.Parameters.AddWithValue("@measureCode", measure["measureCode"]);
        insertCmd.Parameters.AddWithValue("@measureId", measure["measureId"]);
        insertCmd.Parameters.AddWithValue("@measureText", measure["measureText"]);
        insertCmd.Parameters.AddWithValue("@no", measure["no"]);
        insertCmd.Parameters.AddWithValue("@outcome", measure["outcome"]);
        insertCmd.Parameters.AddWithValue("@proUrl", measure["proUrl"]);
        insertCmd.Parameters.AddWithValue("@source", measure["source"]);
        insertCmd.Parameters.AddWithValue("@summary", measure["summary"]);
        insertCmd.Parameters.AddWithValue("@summaryUrl", measure["summaryUrl"]);
        insertCmd.Parameters.AddWithValue("@textUrl", measure["textUrl"]);
        insertCmd.Parameters.AddWithValue("@title", measure["title"]);
        insertCmd.Parameters.AddWithValue("@url", measure["url"]);
        insertCmd.Parameters.AddWithValue("@yes", measure["yes"]);
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
            " {2} measures added. {3} duplicate measures.",
          table.Rows.Count - fetchErrors, fetchErrors, added, duplicates));
    }
  }
}
