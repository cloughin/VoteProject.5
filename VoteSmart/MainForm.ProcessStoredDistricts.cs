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
    private static void ProcessStoredDistricts(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM district_raw " +
        "WHERE stateId=@stateId AND officeId=@officeId AND districtId=@districtId";

      const string insertCmdText =
        "INSERT INTO district_raw" +
          "(districtId,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "name,officeId,stateId)" +
          " VALUES (@districtId,@fetchId,@fetchMethod,@fetchParameters," +
          "@fetchTime,@name,@officeId,@stateId);";

      var fetchErrors = 0;
      var districtsAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("districtList"))
        {
          fetchErrors++;
          continue;
        }
        var districtList = jsonObj["districtList"] as Dictionary<string, object>;
        if (districtList == null || !districtList.ContainsKey("district"))
        {
          fetchErrors++;
          continue;
        }
        var district = AsArrayList(districtList["district"]);
        if (district == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var d in district.Cast<Dictionary<string, object>>())
        {
          var stateId = d["stateId"] as string;
          var officeId = Convert.ToInt32(d["officeId"]);
          var districtId = Convert.ToInt32(d["districtId"]);
          // if there is one already there, skip
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@stateId", stateId);
          countCmd.Parameters.AddWithValue("@officeId", officeId);
          countCmd.Parameters.AddWithValue("@districtId", districtId);
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
          insertCmd.Parameters.AddWithValue("@districtId", districtId);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@name", d["name"]);
          insertCmd.Parameters.AddWithValue("@officeId", d["officeId"]);
          insertCmd.Parameters.AddWithValue("@stateId", d["stateId"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            insertCmd.ExecuteNonQuery();
            districtsAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} districts added. {3} duplicates,",
          table.Rows.Count - fetchErrors, fetchErrors, districtsAdded,
          duplicates));
    }
  }
}
