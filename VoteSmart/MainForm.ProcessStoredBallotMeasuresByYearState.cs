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
    private static void ProcessStoredBallotMeasuresByYearState(DataTable table)
    {
      var fetchErrors = 0;
      var measuresQueried = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("measures"))
        {
          fetchErrors++;
          continue;
        }
        var measures = jsonObj["measures"] as Dictionary<string, object>;
        if (measures == null || !measures.ContainsKey("measure"))
        {
          fetchErrors++;
          continue;
        }
        var measure = AsArrayList(measures["measure"]);
        foreach (var m in measure.OfType<Dictionary<string, object>>())
        {
          // Only query if not there already
          const string method = "Measure.getMeasure";
          var parameters = "measureId=" + m["measureId"];
          const string countCmdText = "SELECT COUNT(*) FROM fetches_raw " +
            "WHERE fetch_method=@method AND fetch_parameters=@parameters";
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@method", method);
          countCmd.Parameters.AddWithValue("@parameters", parameters);
          int count;
          using (var cn = GetOpenConnection())
          {
            countCmd.Connection = cn;
            count = Convert.ToInt32(countCmd.ExecuteScalar());
          }
          if (count == 0)
          {
            SaveRawData(method, parameters);
            measuresQueried++;
          }
          else duplicates++;
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} measures queried. {3} duplicate measures",
          table.Rows.Count - fetchErrors, fetchErrors, measuresQueried,
          duplicates));
    }
  }
}
