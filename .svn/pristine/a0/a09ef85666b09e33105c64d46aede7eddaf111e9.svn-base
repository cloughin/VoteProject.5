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
    private static void ProcessStoredCommittees(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM committee_raw " +
        "WHERE committeeId=@committeeId";

      const string insertCommitteeCmdText =
        "INSERT INTO committee_raw" +
          "(committeeId,committeeTypeId,contact_address1,contact_address2," +
          "contact_city,contact_email,contact_fax,contact_phone," +
          "contact_staffContact,contact_state,contact_url,contact_zip," +
          "fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "jurisdiction,name,parentId,stateId)" +
          " VALUES (@committeeId,@committeeTypeId,@contactAddress1," +
          "@contactAddress2,@contactCity,@contactEmail,@contactFax," +
          "@contactPhone,@contactStaffContact,@contactState,@contacUrl," +
          "@contactZip,@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@jurisdiction,@name,@parentId,@stateId);";

      var fetchErrors = 0;
      var committeesAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var committeeId = ParseIdFromParameters(row["fetch_parameters"], "committeeId");
        // if there are any already there, skip
        var countCmd = new MySqlCommand(countCmdText);
        countCmd.Parameters.AddWithValue("@committeeId", committeeId);
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

        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("committee"))
        {
          fetchErrors++;
          continue;
        }
        var committee = jsonObj["committee"] as Dictionary<string, object>;
        if (committee == null || !committee.ContainsKey("contact"))
        {
          fetchErrors++;
          continue;
        }
        var contact = committee["contact"] as Dictionary<string, object>;
        if (contact == null)
        {
          fetchErrors++;
          continue;
        }
        var insertCommitteeCmd = new MySqlCommand(insertCommitteeCmdText);
        insertCommitteeCmd.Parameters.AddWithValue("@committeeId", committee["committeeId"]);
        insertCommitteeCmd.Parameters.AddWithValue("@committeeTypeId", committee["committeeTypeId"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactAddress1", contact["address1"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactAddress2", contact["address1"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactCity", contact["city"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactEmail", contact["email"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactFax", contact["fax"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactPhone", contact["phone"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactStaffContact", contact["staffContact"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactState", contact["state"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contacUrl", contact["url"]);
        insertCommitteeCmd.Parameters.AddWithValue("@contactZip", contact["zip"]);
        insertCommitteeCmd.Parameters.AddWithValue("@fetchId", row["id"]);
        insertCommitteeCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
        insertCommitteeCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
        insertCommitteeCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
        insertCommitteeCmd.Parameters.AddWithValue("@jurisdiction", committee["jurisdiction"]);
        insertCommitteeCmd.Parameters.AddWithValue("@name", committee["name"]);
        insertCommitteeCmd.Parameters.AddWithValue("@parentId", committee["parentId"]);
        insertCommitteeCmd.Parameters.AddWithValue("@stateId", committee["stateId"]);
        using (var cn = GetOpenConnection())
        {
          insertCommitteeCmd.Connection = cn;
          insertCommitteeCmd.ExecuteNonQuery();
          committeesAdded++;
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} committees added. {3} duplicates,",
          table.Rows.Count - fetchErrors, fetchErrors, committeesAdded,
          duplicates));
    }
  }
}
