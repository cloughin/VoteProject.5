using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DB.Vote;

namespace RepairDB
{
  public partial class EditForm : Form
  {
    bool _Exiting;
    bool _SkippingTable;
    bool _Updated;
    string _UpdatedValue;
    MainForm MainForm;

    // from last call to Initialize
    DataRow Row;
    TableInfo TableInfo;
    ColumnInfo ColumnInfo;
    char BadCharacter;
    string OriginalValueToRepair;
    bool IsChanged;

    public EditForm(MainForm mainForm)
    {
      InitializeComponent();
      MainForm = mainForm;
    }

    public bool Exiting
    {
      get { return _Exiting; }
    }

    private bool HighlightNextError()
    {
      int start = ValueToRepairTextBox.SelectionStart;
      if (ValueToRepairTextBox.SelectionLength > 0)
        start++;
      if (BadCharacter == MainForm.BadCharacter)
        start = 0; // never let a true bad character pass
      int inx = ValueToRepairTextBox.Text.IndexOf(BadCharacter, start);
      if (inx < 0) return false;
      ValueToRepairTextBox.SelectionStart = inx;
      ValueToRepairTextBox.SelectionLength = 1;
      ValueToRepairTextBox.ScrollToCaret();
      ValueToRepairTextBox.Focus();
      return true;
    }

    public void Initialize(DataRow row, TableInfo tableInfo, ColumnInfo columnInfo,
      char badCharacter)
    {
      Row = row;
      TableInfo = tableInfo;
      ColumnInfo = columnInfo;
      BadCharacter = badCharacter;
    }

    public bool SkippingTable
    {
      get { return _SkippingTable || _Exiting; }
      set { _SkippingTable = value; }
    }

    public bool Updated
    {
      get { return _Updated; }
    }

    public string UpdatedValue
    {
      get { return _UpdatedValue; }
    }

    public static void UpdateValue(TableInfo tableInfo, ColumnInfo columnInfo, 
      DataRow row, string newValue, MainForm mainForm)
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
        mainForm.AppendStatusText("An error occurred updating value: {0}",
          ex.Message);
      }
    }

    #region Event handlers

    private void EditForm_Shown(object sender, EventArgs e)
    {
      _Exiting = false;
      _SkippingTable = false;
      _Updated = false;
      TableNameLabel.Text = TableInfo.Name;
      PrimaryKeyColumnsLabel.Text = string.Join(" | ", TableInfo.PrimaryKeyColumns);
      PrimaryKeyValuesLabel.Text = string.Join(" | ",
        TableInfo.PrimaryKeyColumns
        .Select(col => Row[col].ToString())
        .ToList());
      EditColumnLabel.Text = ColumnInfo.Name;
      OriginalValueToRepair = Row[ColumnInfo.Name].ToString();
      ValueToRepairTextBox.Text = OriginalValueToRepair;
      IsChanged = false;
      NextErrorButton.Enabled = true;
      UndoAllButton.Enabled = true;
      UpdateButton.Enabled = true;
      HighlightNextError();
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
      _Exiting = true;
      Hide();
    }

    private void NextErrorButton_Click(object sender, EventArgs e)
    {
      NextErrorButton.Enabled = HighlightNextError();

    }

    private void OkButton_Click(object sender, EventArgs e)
    {
      Hide();
    }

    private void ReplaceButton_Click(object sender, EventArgs e)
    {
      ValueToRepairTextBox.Paste((sender as Button).Tag as string);
      ValueToRepairTextBox.Focus();
    }

    private void SkipTableButton_Click(object sender, EventArgs e)
    {
      _SkippingTable = true;
      Hide();
    }

    private void UndoAllButton_Click(object sender, EventArgs e)
    {
      ValueToRepairTextBox.Text = OriginalValueToRepair;
      HighlightNextError();
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
      if (ColumnInfo.Size > 0 && ValueToRepairTextBox.Text.Length > ColumnInfo.Size)
        MessageBox.Show(string.Format("The new text length exceeds the maximum for this column of {0}",
          ColumnInfo.Size), "Cannot Update New Value", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
      else
      {
        UpdateValue(TableInfo, ColumnInfo, Row, ValueToRepairTextBox.Text,
          MainForm);
        OriginalValueToRepair = ValueToRepairTextBox.Text;
        IsChanged = false;
        UpdateButton.Enabled = false;
        UndoAllButton.Enabled = false;
        _Updated = true;
        _UpdatedValue = ValueToRepairTextBox.Text;
      }
    }

    private void ValueToRepairTextBox_TextChanged(object sender, EventArgs e)
    {
      IsChanged = ValueToRepairTextBox.Text != OriginalValueToRepair;
      UpdateButton.Enabled = IsChanged;
      UndoAllButton.Enabled = IsChanged;
    }

    #endregion Event handlers
  }
}
