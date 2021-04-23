using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace ExcelTest
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void ButtonBrowseFileToRead_Click(object sender, EventArgs e)
    {
      OpenFileToReadDialog.FileName = TextBoxFileToRead.Text;
      if (OpenFileToReadDialog.ShowDialog() == DialogResult.OK)
        TextBoxFileToRead.Text = OpenFileToReadDialog.FileName;
    }

    private void ButtonStart_Click(object sender, EventArgs e)
    {
      try
      {
        var fileToRead = new FileInfo(TextBoxFileToRead.Text);
        var isExcel = new[]{".xls", ".xlsx"}.Contains(fileToRead.Extension, StringComparer.OrdinalIgnoreCase);
        using (var stream = fileToRead.OpenRead())
        {
          var reader = isExcel
            ? ExcelReaderFactory.CreateReader(stream)
            : ExcelReaderFactory.CreateCsvReader(stream);
          using (reader)
          {
            var config = new ExcelDataSetConfiguration
            {
              ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
              {
                UseHeaderRow = true
              }
            };
            var result = reader.AsDataSet(config);
            if (result.Tables.Count != 1)
              throw new Exception($"Cannot handle an Excel File with {result.Tables.Count} sheets");
            var dataTable = result.Tables[0];
            MessageBox.Show(string.Join("\n", dataTable.Columns.OfType<DataColumn>().Select(c => c.ColumnName)));
            MessageBox.Show(dataTable.Rows.Count.ToString());
            foreach (var row in dataTable.Rows.OfType<DataRow>())
            {
              MessageBox.Show(row["Cand Ballot Name Txt"] as string);
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        return;
      }
    }
  }
}
