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
    private static void ProcessStoredAddressOffice(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM address_office_raw " +
        "WHERE candidateId=@candidateId AND isCampaign=@isCampaign";

      const string insertAddressCmdText =
        "INSERT INTO address_office_raw" +
          "(candidateId,cellphone,city,contactName,contactTitle,fax1,fax2,fetch_id," +
          "fetch_method,fetch_parameters,fetch_time,isCampaign,phone1,phone2," +
          "state,street,tollFree,ttyd,type,typeId,zip)" +
          " VALUES (@candidateId,@cellphone,@city,@contactName,@contactTitle,@fax1," +
          "@fax2,@fetchId,@fetchMethod,@fetchParameters,@fetchTime,@isCampaign," +
          "@phone1,@phone2,@state,@street,@tollFree,@ttyd,@type,@typeId,@zip);";

      var isCampaign = table.Rows.Count > 0 &&
        table.Rows[0]["fetch_method"].ToString() == "Address.getCampaign";

      var fetchErrors = 0;
      var addressesAdded = 0;
      var officeErrors = 0;
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
        if (!jsonObj.ContainsKey("address"))
        {
          fetchErrors++;
          continue;
        }
        var address = jsonObj["address"] as Dictionary<string, object>;
        if (address == null || !address.ContainsKey("office"))
        {
          fetchErrors++;
          continue;
        }
        var office = AsArrayList(address["office"]);
        foreach (var o in office.Cast<Dictionary<string, object>>())
        {
          if (!o.ContainsKey("address") || !o.ContainsKey("phone") ||
            !o.ContainsKey("notes"))
          {
            officeErrors++;
            continue;
          }
          var address2 = o["address"] as Dictionary<string, object>;
          var phone = o["phone"] as Dictionary<string, object>;
          var notes = o["notes"] as Dictionary<string, object>;
          if (address2 == null || phone == null || notes == null)
          {
            officeErrors++;
            continue;
          }

          var insertAddressCmd = new MySqlCommand(insertAddressCmdText);
          insertAddressCmd.Parameters.AddWithValue("@candidateId", candidateId);
          insertAddressCmd.Parameters.AddWithValue("@city", address2["city"]);
          insertAddressCmd.Parameters.AddWithValue("@cellphone", phone["cellphone"]);
          insertAddressCmd.Parameters.AddWithValue("@contactName", notes["contactName"]);
          insertAddressCmd.Parameters.AddWithValue("@contactTitle", notes["contactTitle"]);
          insertAddressCmd.Parameters.AddWithValue("@fax1", phone["fax1"]);
          insertAddressCmd.Parameters.AddWithValue("@fax2", phone["fax2"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertAddressCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertAddressCmd.Parameters.AddWithValue("@isCampaign", isCampaign);
          insertAddressCmd.Parameters.AddWithValue("@phone1", phone["phone1"]);
          insertAddressCmd.Parameters.AddWithValue("@phone2", phone["phone2"]);
          insertAddressCmd.Parameters.AddWithValue("@state", address2["state"]);
          insertAddressCmd.Parameters.AddWithValue("@street", address2["street"]);
          insertAddressCmd.Parameters.AddWithValue("@tollFree", phone["tollFree"]);
          insertAddressCmd.Parameters.AddWithValue("@ttyd", phone["ttyd"]);
          insertAddressCmd.Parameters.AddWithValue("@type", address2["type"]);
          insertAddressCmd.Parameters.AddWithValue("@typeId", address2["typeId"]);
          insertAddressCmd.Parameters.AddWithValue("@zip", address2["zip"]);
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
            " {2} addresses added. {3} duplicates," +
            " {4} office errors",
          table.Rows.Count - fetchErrors, fetchErrors, addressesAdded,
          duplicates, officeErrors));
    }
  }
}
