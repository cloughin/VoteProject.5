using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NormalizeNewLines
{
  public partial class SelectTablesForm : Form
  {
    List<TableInfo> TableInfoList;

    public SelectTablesForm(List<TableInfo> tableInfoList)
    {
      InitializeComponent();

      TableInfoList = tableInfoList;
      TablesCheckedListBox.Items.Clear();
      foreach (var tableInfo in tableInfoList)
      {
        int inx = TablesCheckedListBox.Items.Add(tableInfo);
        TablesCheckedListBox.SetItemChecked(inx, tableInfo.Enabled);
      }
    }

    private void SetAllItems(bool selected)
    {
      for (int inx = 0; inx < TablesCheckedListBox.Items.Count; inx++)
        TablesCheckedListBox.SetItemChecked(inx, selected);
    }

    #region Event handlers

    private void SelectTablesSelectAllButton_Click(object sender, EventArgs e)
    {
      SetAllItems(true);
    }

    private void SelectTablesSelectNoneButton_Click(object sender, EventArgs e)
    {
      SetAllItems(false);
    }

    private void SelectTablesOkButton_Click(object sender, EventArgs e)
    {
      for (int inx = 0; inx < TablesCheckedListBox.Items.Count; inx++)
      {
        TableInfo tableInfo = TablesCheckedListBox.Items[inx] as TableInfo;
        tableInfo.Enabled = TablesCheckedListBox.GetItemChecked(inx);
      }
      Hide();
    }

    #endregion Event handlers
  }
}
