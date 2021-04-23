using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using UtilityLibrary;

namespace TestRedirect
{
  public partial class FixPoliticianKeys : Form
  {
    List<string> TableNames;

    string[] PoliticianKeyColumns = new string[]
    {
      "PoliticianKey",
      "RunningMateKey"
    };

    string[] ExcludeTables = new string[]
    {
      "LogPageVisits",
      "LogElectionPoliticianAddsDeletes",
      "LogEmails",
      "LogOfficeOfficialAddsDeletes",
      "LogOfficeOfficialChanges",
      "LogPoliticianAdds",
      "LogPoliticianAnswers",
      "LogPoliticianChanges",
      "LogPoliticiansImagesHeadshot",
      "LogPoliticiansImagesOriginal"
    };

    class UpdateInfo
    {
      public string TableName;
      public string ColumnName;
    }

    List<UpdateInfo> Updates;

    public FixPoliticianKeys()
    {
      InitializeComponent();

      SetConnectionString();
    }

    private void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private void Initialize()
    {
      GetTableInfo();
      GetUpdateInfo();
    }

    public void GetTableInfo()
    {
      TableNames = new List<String>();

      using (var cn = VoteDb.GetOpenConnection())
      {
        DataTable table = new DataTable("TableList");
        var command = VoteDb.GetCommand("SHOW FULL TABLES", cn);
        var adapter = VoteDb.GetDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
        {
          string tableName = row[0] as string;
          if ((row[1] as string) != "VIEW" && !ExcludeTables.Contains(tableName))
            TableNames.Add(tableName);
        }
      }

      AppendStatusText("Found {0} tables", TableNames.Count);
    }

    public void GetUpdateInfo()
    {
      Updates = new List<UpdateInfo>();

      foreach (string tableName in TableNames)
      {
        using (var cn = VoteDb.GetOpenConnection())
        {
          DataTable table = new DataTable("ColumnList");
          var command = VoteDb.GetCommand("SHOW COLUMNS FROM " + tableName, cn);
          var adapter = VoteDb.GetDataAdapter(command);
          adapter.Fill(table);

          foreach (DataRow row in table.Rows)
          {
            string columnName = row[0] as string;
            if (PoliticianKeyColumns.Contains(columnName))
              Updates.Add(
                new UpdateInfo()
                { 
                  TableName = tableName, 
                  ColumnName = columnName 
                });
          }
        }
      }

      AppendStatusText("Found {0} updateable columns", Updates.Count);
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked)
        appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveDb";
      else
        throw new VoteException("Invalid server");
      string connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
      Initialize();
    }

    #region Event handlers

    private void FixButton_Click(object sender, EventArgs e)
    {
      AppendStatusText(string.Empty);
      foreach (UpdateInfo updateInfo in Updates)
      {
        // This could be better done with a single update statement
        string sqlText =
          "SELECT " + updateInfo.ColumnName +
          " FROM " + updateInfo.TableName +
          " WHERE " + updateInfo.ColumnName + "=@OriginalKey";

        using (var cn = VoteDb.GetOpenConnection())
        {
          DataTable table = new DataTable("Updates");
          var command = VoteDb.GetCommand(sqlText, cn);
          VoteDb.AddCommandParameter(command, "OriginalKey", OriginalKeyTextBox.Text);
          var adapter = VoteDb.GetDataAdapter(command);
          adapter.Fill(table);

          AppendStatusText("Table {0}, Column {1}, Found {2}",
            updateInfo.TableName, updateInfo.ColumnName, table.Rows.Count);

          if (table.Rows.Count > 0)
          {
            var builder = VoteDb.GetCommandBuilder(adapter);
            foreach (DataRow row in table.Rows)
              row[updateInfo.ColumnName] = NewKeyTextBox.Text;
            adapter.Update(table);
          }
        }
      }
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        RadioButton radioButton = sender as RadioButton;
        if (radioButton != null && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    #endregion Event handlers
  }
}
