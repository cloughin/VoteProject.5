using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DB.Vote;
using MySql.Data.MySqlClient;
using UtilityLibrary;
using Vote;

namespace RepairDB
{
  public partial class MainForm : Form
  {
    public const char BadCharacter = (char) 65533;

    Dictionary<string, string> Duplicates = new Dictionary<string, string>();

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

    private void HandleRow(DataRow row, EditForm editForm, TableInfo tableInfo,
      char badCharacter)
    {
      foreach (var columnInfo in tableInfo.TextColumns)
      {
        string value = row[columnInfo.Name] as string;
        //if (value != null && value.Contains(badCharacter))
        if (ValueNeedsInspection(value, badCharacter))
        {
          // Try paragraph by paragraph substitution
          var paragraphs = value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
          for (int n = 0; n < paragraphs.Length; n++)
          {
            string newParagraph;
            if (Duplicates.TryGetValue(paragraphs[n], out newParagraph))
              paragraphs[n] = newParagraph;
          }
          value = string.Join("\r\n", paragraphs);
          // now re-check
          if (!ValueNeedsInspection(value, badCharacter))
          {
            // use edited value automatically
            EditForm.UpdateValue(tableInfo, columnInfo, row, value, this);
          }
          else
          {
            row[columnInfo.Name] = value;
            editForm.Initialize(row, tableInfo, columnInfo, badCharacter);
            editForm.ShowDialog();
            if (editForm.SkippingTable)
              break;
            //if (editForm.Updated && !Duplicates.ContainsKey(value))
            //  Duplicates.Add(value, editForm.UpdatedValue);
            if (editForm.Updated)
            {
              var oldParagraphs = value.Split(
                new string[] { "\r\n" }, StringSplitOptions.None);
              var newParagraphs = editForm.UpdatedValue.Split(
                new string[] { "\r\n" }, StringSplitOptions.None);
              if (oldParagraphs.Length == newParagraphs.Length)
              for (int n = 0; n < oldParagraphs.Length; n++)
                if (oldParagraphs[n] != newParagraphs[n] &&
                 !Duplicates.ContainsKey(oldParagraphs[n]))
                  Duplicates.Add(oldParagraphs[n], newParagraphs[n]);
            }
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

    private bool ValueNeedsInspection(string value, char badCharacter)
    {
      if (value == null) return false;
      if (!ApplyHeuristicsCheckBox.Checked)
        return value.Contains(badCharacter);
      switch (badCharacter)
      {
        case '?':
          // ignore if / in "word" (probably url)
          // ignore if followed by space and preceeded by letter (probably real question mark)
          var matches = Regex.Matches(value, @"\?");
          foreach (Match match in matches)
          {
            int position = match.Index;
            if (position > 0)
              if ((position == (value.Length - 1) ||
                value[position + 1] == ' ' ||
                value[position + 1] == '"' ||
                value[position + 1] == '?' ||
                value[position + 1] == '\'' ||
                value[position + 1] == '\r' ||
                value[position + 1] == '\n')
               && (value[position - 1] != ' ' &&
                value[position - 1] != '\r' &&
                value[position - 1] != '\n'))
                continue;
            bool hasSlash = false;
            for (int n = position - 1; n >= 0; n--)
              if (value[n] == '/' || value[n] == '\\')
              {
                hasSlash = true;
                break;
              }
              else if (value[n] == ' ' || value[n] == '\r' || value[n] == '\n')
                break;
            if (hasSlash)
              continue;
            return true;
          }
          return false;

        default:
          return value.Contains(badCharacter);
      }
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      var tableInfoList = e.Argument as List<TableInfo>;

      char badCharacter = BadCharacter;
      foreach (var control in SearchCharacterGroupBox.Controls)
      {
        var radioButton = control as RadioButton;
        if (radioButton != null && radioButton.Checked)
        {
          string tag = radioButton.Tag as string;
          if (tag != null && tag.Length > 0)
          {
            badCharacter = tag[0];
            break;
          }
        }
      }

      using (var editForm = new EditForm(this))
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
          bool hasPoliticianKey =
            tableInfo.TextColumns
             .FirstOrDefault(info => info.Name == "PoliticianKey") != null ||
            tableInfo.PrimaryKeyColumns
             .FirstOrDefault(name => name == "PoliticianKey") != null;
          AppendStatusText("Looking for invalid characters in table {0}", tableInfo.Name);
          foreach (DataRow row in table.Rows)
          {
            //if (
            //  row["PoliticianKey"].ToString() != "MARomneyMitt" &&
            //  row["PoliticianKey"].ToString() != "WIRyanPaul" &&
            //  row["PoliticianKey"].ToString() != "ILObamaBarack" &&
            //  row["PoliticianKey"].ToString() != "DEBidenJosephRJr")
            //  continue;
            if (!hasPoliticianKey ||
              row["PoliticianKey"].ToString().IsGeIgnoreCase(StartAtTextBox.Text))
            {
              HandleRow(row, editForm, tableInfo, badCharacter);
              if (editForm.SkippingTable)
              {
                AppendStatusText("Skipping rest of table {0}", tableInfo.Name);
                editForm.SkippingTable = false;
                break;
              }
            }
            if (editForm.Exiting)
            {
              AppendStatusText("Exiting");
              break;
            }
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
