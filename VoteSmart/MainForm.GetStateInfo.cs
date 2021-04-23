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
    private static void GetStateInfo(DataTable table)
    {
      var fetchErrors = 0;
      var statesFetched = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("stateList"))
        {
          fetchErrors++;
          continue;
        }
        var stateList = jsonObj["stateList"] as Dictionary<string, object>;
        if (stateList == null || !stateList.ContainsKey("list"))
        {
          fetchErrors++;
          continue;
        }
        var list = stateList["list"] as Dictionary<string, object>;
        if (list == null || !list.ContainsKey("state"))
        {
          fetchErrors++;
          continue;
        }
        var state = AsArrayList(list["state"]);
        if (state == null)
        {
          fetchErrors++;
          continue;
        }
        const string stateMethod = "State.getState";
        foreach (var s in state.OfType<Dictionary<string, object>>())
        {
          var stateId = s["stateId"];
          var parameters = "stateId=" + stateId;

          // only fetch state info if not already there
          const string countCmdText = "SELECT COUNT(*) FROM fetches_raw " +
            "WHERE fetch_method=@method AND fetch_parameters=@parameters";
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@method", stateMethod);
          countCmd.Parameters.AddWithValue("@parameters", parameters);
          int count;
          using (var cn = GetOpenConnection())
          {
            countCmd.Connection = cn;
            count = Convert.ToInt32(countCmd.ExecuteScalar());
          }
          if (count == 0)
          {
            SaveRawData(stateMethod, parameters);
            statesFetched++;
          }
          else
          {
            duplicates++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} states fetched, {3} duplicates.",
          table.Rows.Count - fetchErrors, fetchErrors, statesFetched,
          duplicates));
    }
  }
}
