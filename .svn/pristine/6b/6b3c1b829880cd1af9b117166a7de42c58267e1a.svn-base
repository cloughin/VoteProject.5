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
    private static void ProcessStoredLocalOfficials(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM local_officials_raw " +
        "WHERE localId=@localId AND officeId=@officeId AND " +
        "candidateId=@candidateId";

      const string insertCmdText =
        "INSERT INTO local_officials_raw" +
          "(candidateId,electionDistrictId,electionParties,fetch_id," +
          "fetch_method,fetch_parameters,fetch_time," +
          "firstName,lastName,localId,middleName,nickName,officeDistrictId," +
          "officeDistrictName,officeId,officeName,officeParties," +
          "officeStateId,officeTypeId,suffix,title)" +
          " VALUES (@candidateId,@electionDistrictId,@electionParties," +
          "@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@firstName,@lastName,@localId,@middleName,@nickName,@officeDistrictId," +
          "@officeDistrictName,@officeId,@officeName,@officeParties," +
          "@officeStateId,@officeTypeId,@suffix,@title);";

      var fetchErrors = 0;
      var officialsAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var localId = ParseIdFromParameters(row["fetch_parameters"], "localId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("candidateList"))
        {
          fetchErrors++;
          continue;
        }
        var candidateList = jsonObj["candidateList"] as Dictionary<string, object>;
        if (candidateList == null || !candidateList.ContainsKey("candidate"))
        {
          fetchErrors++;
          continue;
        }
        var candidate = AsArrayList(candidateList["candidate"]);
        if (candidate == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var c in candidate.Cast<Dictionary<string, object>>())
        {
          var officeId = Convert.ToInt32(c["officeId"]);
          var candidateId = Convert.ToInt32(c["candidateId"]);
          var officeDistrictId = c["officeDistrictId"];
          if (string.IsNullOrWhiteSpace(officeDistrictId as string)) officeDistrictId = null;
          var electionDistrictId = c["electionDistrictId"];
          if (string.IsNullOrWhiteSpace(electionDistrictId as string)) electionDistrictId = null;
          // skip duplicate official
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@localId", localId);
          countCmd.Parameters.AddWithValue("@officeId", officeId);
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
          insertCmd.Parameters.AddWithValue("@electionDistrictId", electionDistrictId);
          insertCmd.Parameters.AddWithValue("@electionParties", c["electionParties"]);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@firstName", c["firstName"]);
          insertCmd.Parameters.AddWithValue("@lastName", c["lastName"]);
          insertCmd.Parameters.AddWithValue("@localId", localId);
          insertCmd.Parameters.AddWithValue("@middleName", c["middleName"]);
          insertCmd.Parameters.AddWithValue("@nickName", c["nickName"]);
          insertCmd.Parameters.AddWithValue("@officeDistrictId", officeDistrictId);
          insertCmd.Parameters.AddWithValue("@officeDistrictName", c["officeDistrictName"]);
          insertCmd.Parameters.AddWithValue("@officeId", officeId);
          insertCmd.Parameters.AddWithValue("@officeName", c["officeName"]);
          insertCmd.Parameters.AddWithValue("@officeParties", c["officeParties"]);
          insertCmd.Parameters.AddWithValue("@officeStateId", c["officeStateId"]);
          insertCmd.Parameters.AddWithValue("@officeTypeId", c["officeTypeId"]);
          insertCmd.Parameters.AddWithValue("@suffix", c["suffix"]);
          insertCmd.Parameters.AddWithValue("@title", c["title"]);
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
