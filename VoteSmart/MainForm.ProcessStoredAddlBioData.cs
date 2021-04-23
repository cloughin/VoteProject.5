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
    private static void ProcessStoredAddlBioData(DataTable table)
    {
      const string insertCandidateCmdText =
        "INSERT INTO addlbio_candidate_raw" +
          "(candidateId,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "firstName,lastName,middleName,nickName,shortTitle,suffix)" +
          " VALUES (@candidateId,@fetchId,@fetchMethod,@fetchParameters," +
          "@fetchTime,@firstName,@lastName,@middleName,@nickName," +
          "@shortTitle,@suffix);";

      const string insertCandidateItemCmdText =
        "INSERT INTO addlbio_additional_item_raw" +
          "(candidateId,data,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "name) VALUES (@candidateId,@data,@fetchId,@fetchMethod," +
          "@fetchParameters,@fetchTime,@name);";

      var fetchErrors = 0;
      var biosAdded = 0;
      var duplicateBios = 0;
      var itemsAdded = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var candidateId = ParseIdFromParameters(row["fetch_parameters"], "candidateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("addlBio"))
        {
          fetchErrors++;
          continue;
        }
        var addlBio = jsonObj["addlBio"] as Dictionary<string, object>;
        if (addlBio == null || !addlBio.ContainsKey("candidate"))
        {
          fetchErrors++;
          continue;
        }
        var candidate = addlBio["candidate"] as Dictionary<string, object>;
        if (candidate == null || !addlBio.ContainsKey("additional"))
        {
          fetchErrors++;
          continue;
        }

        var insertCandidateCmd = new MySqlCommand(insertCandidateCmdText);
        insertCandidateCmd.Parameters.AddWithValue("@candidateId", candidateId);
        insertCandidateCmd.Parameters.AddWithValue("@fetchId", row["id"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
        insertCandidateCmd.Parameters.AddWithValue("@firstName", candidate["firstName"]);
        insertCandidateCmd.Parameters.AddWithValue("@lastName", candidate["lastName"]);
        insertCandidateCmd.Parameters.AddWithValue("@middleName", candidate["middleName"]);
        insertCandidateCmd.Parameters.AddWithValue("@nickName", candidate["nickName"]);
        insertCandidateCmd.Parameters.AddWithValue("@shortTitle", candidate["shortTitle"]);
        insertCandidateCmd.Parameters.AddWithValue("@suffix", candidate["suffix"]);
        using (var cn = GetOpenConnection())
        {
          insertCandidateCmd.Connection = cn;
          try
          {
            insertCandidateCmd.ExecuteNonQuery();
            biosAdded++;
          }
          catch (MySqlException ex)
          {
            if (ex.Message.StartsWith(("Duplicate entry"), StringComparison.Ordinal))
            {
              duplicateBios++;
              continue;
            }
            throw;
          }
        }

        if (addlBio.ContainsKey("additional"))
        {
          var additional = addlBio["additional"] as Dictionary<string, object>;
          if (additional != null)
          {
            var item = AsArrayList(additional["item"]);
            if (item != null)
              foreach (var i in item.Cast<Dictionary<string, object>>())
              {
                var insertCandidateItemCmd = new MySqlCommand(insertCandidateItemCmdText);
                insertCandidateItemCmd.Parameters.AddWithValue("@candidateId", candidateId);
                insertCandidateItemCmd.Parameters.AddWithValue("@data", i["data"]);
                insertCandidateItemCmd.Parameters.AddWithValue("@fetchId", row["id"]);
                insertCandidateItemCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
                insertCandidateItemCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
                insertCandidateItemCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
                insertCandidateItemCmd.Parameters.AddWithValue("@name", i["name"]);
                using (var cn = GetOpenConnection())
                {
                  insertCandidateItemCmd.Connection = cn;
                  insertCandidateItemCmd.ExecuteNonQuery();
                  itemsAdded++;
                }
              }
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} additional bios added. {3} duplicate additional bios," +
            " {4} additional bio items added.",
          table.Rows.Count - fetchErrors, fetchErrors, biosAdded,
          duplicateBios, itemsAdded));
    }
  }
}
