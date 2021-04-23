using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UtilityLibrary;
using Vote;
using DB.Vote;

namespace ApostropheS
{
  public partial class MainForm : Form
  {
    public const char BadCharacter = (char) 65533;
    
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

    private void HandleRow(DataRow row, TableInfo tableInfo)
    {
      foreach (var columnInfo in tableInfo.TextColumns)
      {
        string value = row[columnInfo.Name] as string;
        if (value != null)
        {
          string newValue = Regex.Replace(value, 
            @"(?:^|[^a-z0-9])(?:[a-z0-9]+�s|aren�t|can�t|couldn�t|couldn�t|didn�t|doesn�t|don�t|hadn�t|hasn�t|haven�t|he�d|he�ll|he�s|i�d|i�ll|i�m|i�ve|isn�t|mightn�t|mustn�t|shan�t|she�d|she�ll|she�s|shouldn�t|they�d|they�ll|they�re|they�ve|we�d|we�re|we�ve|weren�t|what�ll|what�re|what�ve|who�d|who�ll|who�re|who�ve|won�t|wouldn�t|you�d|you�ll|you�re|you�ve)(?:$|[^a-z0-9])",
            match => match.Value.Replace('�', '\''),
            RegexOptions.IgnoreCase);
          if (newValue != value)
            UpdateValue(tableInfo, columnInfo, row, newValue);
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
          AppendStatusText("Reading table {0}", tableInfo.Name);
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
          AppendStatusText("Looking for invalid characters in table {0}", tableInfo.Name);
          foreach (DataRow row in table.Rows)
          {
            HandleRow(row, tableInfo);
          }
        }
      AppendStatusText("Process is complete.");
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {

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
      StartButton.Enabled = false;
      ServerGroupBox.Enabled = false;
      StatusTextBox.Clear();

      var tableInfoList = GetTableInfo();
      SelectTables(tableInfoList);
      BackgroundWorker.RunWorkerAsync(tableInfoList);
    }

    #endregion Event handlers

  }
}
