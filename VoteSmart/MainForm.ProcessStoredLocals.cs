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
    private static void ProcessStoredLocals(DataTable table, string name1, string name2)
    {
      const string countLocalsCmdText = "SELECT COUNT(*) FROM local_raw " +
        "WHERE stateId=@stateId AND localId=@localId";

      const string countOfficialsCmdText = "SELECT COUNT(*) FROM fetches_raw " +
        "WHERE fetch_method=@method AND fetch_parameters=@parameters";

      const string insertCmdText =
        "INSERT INTO local_raw" +
          "(fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "localId,name,stateId,url)" +
          " VALUES (@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@localId,@name,@stateId,@url);";

      const string officialsMethod = "Local.getOfficials";

      var fetchErrors = 0;
      var localsAdded = 0;
      var duplicateLocals = 0;
      var officialsAdded = 0;
      var duplicateOfficials = 0;
      var localsStatesAdded = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey(name1))
        {
          fetchErrors++;
          continue;
        }
        var dictionary = jsonObj[name1] as Dictionary<string, object>;
        if (dictionary == null || !dictionary.ContainsKey(name2))
        {
          fetchErrors++;
          continue;
        }
        var list = AsArrayList(dictionary[name2]);
        if (list == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var l in list.Cast<Dictionary<string, object>>())
        {
          var localId = Convert.ToInt32(l["localId"]);
          // skip duplicate locals
          var officialsParameters = "localId=" + localId;
          var countOfficialsCmd = new MySqlCommand(countOfficialsCmdText);
          countOfficialsCmd.Parameters.AddWithValue("@method", officialsMethod);
          countOfficialsCmd.Parameters.AddWithValue("@parameters", officialsParameters);
          int officialsCount;
          using (var cn = GetOpenConnection())
          {
            countOfficialsCmd.Connection = cn;
            officialsCount = Convert.ToInt32(countOfficialsCmd.ExecuteScalar());
          }
          if (officialsCount != 0)
            duplicateOfficials++;
          else
          {
            SaveRawData(officialsMethod, officialsParameters);
            officialsAdded++;
          }

          const string insertLocalsStates =
            "INSERT INTO locals_states " +
            "(localId,stateId) VALUES (@localId,@stateId);";
          var insertLocalsStatesCmd = new MySqlCommand(insertLocalsStates);
          insertLocalsStatesCmd.Parameters.AddWithValue("@localId", localId);
          insertLocalsStatesCmd.Parameters.AddWithValue("@stateId", stateId);
          using (var cn = GetOpenConnection())
          {
            insertLocalsStatesCmd.Connection = cn;
            try
            {
              insertLocalsStatesCmd.ExecuteNonQuery();
              localsStatesAdded++;
            }
            catch (MySqlException ex)
            {
              if (!ex.Message.StartsWith(("Duplicate entry"),
                StringComparison.Ordinal)) throw;
            }
          }

          // skip duplicate local
          var countLocalsCmd = new MySqlCommand(countLocalsCmdText);
          countLocalsCmd.Parameters.AddWithValue("@stateId", stateId);
          countLocalsCmd.Parameters.AddWithValue("@localId", localId);
          int localCount;
          using (var cn = GetOpenConnection())
          {
            countLocalsCmd.Connection = cn;
            localCount = Convert.ToInt32(countLocalsCmd.ExecuteScalar());
          }
          if (localCount != 0)
          {
            duplicateLocals++;
            continue;
          }

          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@localId", localId);
          insertCmd.Parameters.AddWithValue("@name", l["name"]);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          insertCmd.Parameters.AddWithValue("@url", l["url"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            insertCmd.ExecuteNonQuery();
            localsAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} locals added, {3} duplicate locals," +
            " {4} officials lists added. {5} duplicate officials lists," +
            " {6} locals_states added.",
          table.Rows.Count - fetchErrors, fetchErrors, localsAdded,
          duplicateLocals, officialsAdded, duplicateOfficials,
          localsStatesAdded));
    }
  }
}
