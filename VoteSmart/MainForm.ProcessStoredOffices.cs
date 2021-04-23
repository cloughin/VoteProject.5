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
    private static void ProcessStoredOffices(DataTable table)
    {
      const string insertCmdText =
        "INSERT INTO offices" +
          "(name,officeId,officeTypeId,shortTitle,title) VALUES " +
          "(@name,@officeId,@officeTypeId,@shortTitle,@title);";

      var fetchErrors = 0;
      var officesAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("offices"))
        {
          fetchErrors++;
          continue;
        }
        var offices = jsonObj["offices"] as Dictionary<string, object>;
        if (offices == null || !offices.ContainsKey("office"))
        {
          fetchErrors++;
          continue;
        }
        var office = AsArrayList(offices["office"]);
        if (office == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var o in office.OfType<Dictionary<string, object>>())
        {
          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@name", o["name"]);
          insertCmd.Parameters.AddWithValue("@officeId", o["officeId"]);
          insertCmd.Parameters.AddWithValue("@officeTypeId", o["officeTypeId"]);
          insertCmd.Parameters.AddWithValue("@shortTitle", o["shortTitle"]);
          insertCmd.Parameters.AddWithValue("@title", o["title"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            try
            {
              insertCmd.ExecuteNonQuery();
              officesAdded++;
            }
            catch (MySqlException ex)
            {
              if (ex.Message.StartsWith(("Duplicate entry"), StringComparison.Ordinal)) duplicates++;
              else throw;
            }
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} offices added. {3} duplicate offices",
          table.Rows.Count - fetchErrors, fetchErrors, officesAdded,
          duplicates));
    }
  }
}
