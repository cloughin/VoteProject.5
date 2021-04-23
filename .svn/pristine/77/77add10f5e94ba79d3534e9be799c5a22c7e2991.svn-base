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
    private static void ProcessStoredWebAddress(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM webaddress_raw " +
        "WHERE candidateId=@candidateId AND isCampaign=@isCampaign";

      const string insertWebAddressCmdText =
        "INSERT INTO webaddress_raw" +
          "(candidateId,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "isCampaign,webAddress,webAddressType,webAddressTypeId)" +
          " VALUES (@candidateId,@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@isCampaign,@webAddress,@webAddressType,@webAddressTypeId);";

      var isCampaign = table.Rows.Count > 0 &&
        table.Rows[0]["fetch_method"].ToString() == "Address.getCampaignWebAddress";

      var fetchErrors = 0;
      var addressesAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var candidateId = ParseIdFromParameters(row["fetch_parameters"], "candidateId");
        // if there are any already there, skip
        var countCmd = new MySqlCommand(countCmdText);
        countCmd.Parameters.AddWithValue("@candidateId", candidateId);
        countCmd.Parameters.AddWithValue("@isCampaign", isCampaign);
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
        if (!jsonObj.ContainsKey("webaddress"))
        {
          fetchErrors++;
          continue;
        }
        var webAddress = jsonObj["webaddress"] as Dictionary<string, object>;
        if (webAddress == null || !webAddress.ContainsKey("address"))
        {
          fetchErrors++;
          continue;
        }
        var address = AsArrayList(webAddress["address"]);
        foreach (var a in address.Cast<Dictionary<string, object>>())
        {
          var insertAddressCmd = new MySqlCommand(insertWebAddressCmdText);
          insertAddressCmd.Parameters.AddWithValue("@candidateId", candidateId);
          insertAddressCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertAddressCmd.Parameters.AddWithValue("@isCampaign", isCampaign);
          insertAddressCmd.Parameters.AddWithValue("@webAddressTypeId", a["webAddressTypeId"]);
          insertAddressCmd.Parameters.AddWithValue("@webAddressType", a["webAddressType"]);
          insertAddressCmd.Parameters.AddWithValue("@webAddress", a["webAddress"]);
          using (var cn = GetOpenConnection())
          {
            insertAddressCmd.Connection = cn;
            insertAddressCmd.ExecuteNonQuery();
            addressesAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} addresses added. {3} duplicates,",
          table.Rows.Count - fetchErrors, fetchErrors, addressesAdded,
          duplicates));
    }
  }
}
