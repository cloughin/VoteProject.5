using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB.Vote;
using MySql.Data.MySqlClient;
using UtilityLibrary;
using Vote;
using System.Configuration;
using System.Text.RegularExpressions;

namespace NormalizeNewLines
{
  public partial class MainForm : Form
  {
    bool RepairLineBreaks;
    bool RepairRedundantSpaces;
    bool DoUpdate;

    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    internal void AppendStatusText(string text)
    {
      Form form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      form.Invoke(() => StatusTextBox.AppendText(text));
    }

    internal void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private List<string> GetPrimaryKeyColumns(string tableName)
    {
      var result = new List<string>();

      using (var cn = VoteDb.GetOpenConnection() as MySqlConnection)
      {
        DataTable table = new DataTable("KeyList");
        var command = new MySqlCommand("SHOW KEYS IN " + tableName, cn);
        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
          if ((row["Key_name"] as string) == "PRIMARY")
            result.Add(row["Column_name"] as string);
      }

      return result;
    }

    private List<TableInfo> GetTableInfo()
    {
      var result = new List<TableInfo>();

      using (var cn = VoteDb.GetOpenConnection() as MySqlConnection)
      {
        DataTable table = new DataTable("TableList");
        var command = new MySqlCommand("SHOW FULL TABLES", cn);
        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
          if ((row["Table_type"] as string) != "VIEW") // only real tables
          {
            string tableName = row["Tables_in_vote"] as string;
            var keyColumns = GetPrimaryKeyColumns(tableName);
            if (keyColumns.Count > 0) // must have a primary key
            {
              var textColumns = GetTextColumns(tableName, keyColumns);
              if (textColumns.Count > 0)
                result.Add(new TableInfo()
                {
                  Name = tableName,
                  PrimaryKeyColumns = keyColumns,
                  TextColumns = textColumns,
                  Enabled = true
                });
            }
          }
      }

      return result;
    }


    private List<ColumnInfo> GetTextColumns(string tableName, List<string> keyColumns)
    {
      var result = new List<ColumnInfo>();

      using (var cn = VoteDb.GetOpenConnection() as MySqlConnection)
      {
        DataTable table = new DataTable("ColumnList");
        var command = new MySqlCommand("SHOW COLUMNS FROM " + tableName, cn);
        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
        {
          int maxSize;
          Type type = SqlTypeToDotNetType(row["Type"] as string, out maxSize);
          string name = row["Field"] as string;
          if (type == typeof(string) && !keyColumns.Contains(name))
            result.Add(new ColumnInfo()
            {
              Name = name,
              Size = maxSize
            });
        }
      }

      return result;
    }

    class ColumnStatistics
    {
      public int HasUnnormalizedLineBreaks;
      public int HasRedundantSpaces;
      public int UnnormalizedLineBreaksRepaired;
      public int RedundantSpacesRepaired;
    }

    private void HandleRow(DataRow row, TableInfo tableInfo,
      Dictionary<string, ColumnStatistics> statistics)
    {
      foreach (var columnInfo in tableInfo.TextColumns)
      {
        bool hasUnnormalizedLineBreaks = false;
        bool hasRedundantSpaces = false;
        var name = columnInfo.Name;
        ColumnStatistics columnStats = statistics[name];
        var oldValue = row[name] as string;
        if (oldValue != null)
        {
          var newValue = oldValue.NormalizeNewLines();
          if (newValue != oldValue)
          {
            hasUnnormalizedLineBreaks = true;
            columnStats.HasUnnormalizedLineBreaks++;
          }
          oldValue = newValue;
          newValue = oldValue.StripRedundantSpaces();
          if (newValue != oldValue)
          {
            hasRedundantSpaces = true;
            columnStats.HasRedundantSpaces++;
          }
          if (hasUnnormalizedLineBreaks && RepairLineBreaks ||
            hasRedundantSpaces && RepairRedundantSpaces)
          {
            var updateValue = row[name] as string;
            if (hasUnnormalizedLineBreaks && RepairLineBreaks)
            {
              updateValue = updateValue.NormalizeNewLines();
              columnStats.UnnormalizedLineBreaksRepaired++;
            }
            if (hasRedundantSpaces && RepairRedundantSpaces)
            {
              updateValue = updateValue.StripRedundantSpaces();
              columnStats.RedundantSpacesRepaired++;
            }
            if (DoUpdate)
              UpdateValue(tableInfo, columnInfo, row, updateValue);
          }
        }
      }
    }

    private void SelectTables(List<TableInfo> tableInfoList)
    {
      using (var selectForm = new SelectTablesForm(tableInfoList))
        selectForm.ShowDialog();
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
    }

    static Regex SqlTypeToDotNetTypeRegex =
      new Regex(@"^(?<type>[a-z]+)(?:\((?<size>\d+)(?:,(?<decimals>\d+))?\))?$");
    private Type SqlTypeToDotNetType(string sqlType, out int maxSize)
    {
      Type result = null;
      maxSize = -1;

      Match match = SqlTypeToDotNetTypeRegex.Match(sqlType);
      if (!match.Success)
        throw new ApplicationException("Unknown MySql data type '" + sqlType + "'");
      string typeName = match.Groups["type"].Captures[0].Value;
      int size = -1;
      var sizeCaptures = match.Groups["size"].Captures;
      if (sizeCaptures.Count != 0)
        int.TryParse(sizeCaptures[0].Value, out size);

      switch (typeName)
      {
        case "char":
        case "longtext":
        case "text":
        case "varchar":
          result = typeof(string);
          maxSize = size;
          break;

        case "date":
        case "datetime":
          result = typeof(DateTime);
          break;

        case "tinyint":
          if (size == 1) // conventional
            result = typeof(bool);
          else
            result = typeof(byte);
          break;

        case "smallint":
          result = typeof(short);
          break;

        case "int":
          result = typeof(int);
          break;

        case "bigint":
          result = typeof(long);
          break;

        case "longblob":
          result = typeof(byte[]);
          break;

        case "decimal":
          result = typeof(decimal);
          break;

        default:
          throw new ApplicationException("Unknown MySql data type '" + sqlType + "'");
      }

      return result;
    }

    public void UpdateValue(TableInfo tableInfo, ColumnInfo columnInfo,
     DataRow row, string newValue)
    {
      try
      {
        using (var cn = VoteDb.GetOpenConnection() as MySqlConnection)
        {
          string template = "UPDATE {0} SET {1} = @NewValue WHERE {2}";
          string where = string.Join(" AND ",
            tableInfo.PrimaryKeyColumns.Select(col => col + "=@" + col));
          string sqlText = string.Format(template, tableInfo.Name,
            columnInfo.Name, where);
          var command = new MySqlCommand(sqlText, cn);
          VoteDb.AddCommandParameter(command, "NewValue", newValue);
          foreach (string keyColumn in tableInfo.PrimaryKeyColumns)
            VoteDb.AddCommandParameter(command, keyColumn, row[keyColumn]);
          VoteDb.ExecuteNonQuery(command);
        }
      }
      catch (Exception ex)
      {
        AppendStatusText("An error occurred updating value: {0}",
          ex.Message);
      }
    }
   
    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      var tableInfoList = e.Argument as List<TableInfo>;

      foreach (var tableInfo in tableInfoList.Where(info => info.Enabled))
      {
        int rows = 0;
        AppendStatusText("Reading table {0} with {1} text columns", tableInfo.Name,
          tableInfo.TextColumns.Count);
        DataTable table = new DataTable("ColumnList");
        // get all text and key fields
        using (var cn = VoteDb.GetOpenConnection() as MySqlConnection)
        {
          List<string> allColumns = new List<string>();
          allColumns.AddRange(tableInfo.PrimaryKeyColumns);
          allColumns.AddRange(tableInfo.TextColumns.Select(info => info.Name));
          string columns = string.Join(",", allColumns);
          var command = VoteDb.GetCommand(
            string.Format("SELECT {0} FROM {1}", columns, tableInfo.Name), cn, 0)
            as MySqlCommand;
          MySqlDataAdapter adapter = new MySqlDataAdapter(command);
          adapter.Fill(table);
        }

        Dictionary<string, ColumnStatistics> statistics = 
          new Dictionary<string, ColumnStatistics>();
        foreach (var columnInfo in tableInfo.TextColumns)
          statistics.Add(columnInfo.Name, new ColumnStatistics());

        foreach (DataRow row in table.Rows)
        {
          HandleRow(row, tableInfo, statistics);
          rows++;
        }
        foreach (var columnInfo in tableInfo.TextColumns)
        {
          AppendStatusText("Column {0}: {1} with unnormalized line breaks, {2} with redundant spaces",
            columnInfo.Name, statistics[columnInfo.Name].HasUnnormalizedLineBreaks,
            statistics[columnInfo.Name].HasRedundantSpaces);
          if (RepairLineBreaks || RepairRedundantSpaces)
            AppendStatusText("Column {0}: {1} unnormalized line breaks repaired, {2} redundant spaces repaired",
              columnInfo.Name, statistics[columnInfo.Name].HasUnnormalizedLineBreaks,
              statistics[columnInfo.Name].HasRedundantSpaces);

        }
        AppendStatusText("{0} rows examined", rows);
      }

      AppendStatusText("Process is complete.");
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      ServerGroupBox.Enabled = true;
      RepairLineBreaksCheckBox.Enabled = true;
      RepairRedundantSpacesCheckBox.Enabled = true;
      UpdateCheckBox.Checked = false;
      UpdateCheckBox.Enabled = true;
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

    private void StartButton_Click(object sender, EventArgs e)
    {
      RepairLineBreaks = RepairLineBreaksCheckBox.Checked;
      RepairRedundantSpaces = RepairRedundantSpacesCheckBox.Checked;
      DoUpdate = UpdateCheckBox.Checked;

      StartButton.Enabled = false;
      ServerGroupBox.Enabled = false;
      RepairLineBreaksCheckBox.Enabled = false;
      RepairRedundantSpacesCheckBox.Enabled = false;
      UpdateCheckBox.Enabled = false;
      StatusTextBox.Clear();

      var tableInfoList = GetTableInfo();
      SelectTables(tableInfoList);
      BackgroundWorker.RunWorkerAsync(tableInfoList);
    }

    #endregion Event handlers

  }
}
