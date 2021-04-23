using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.VoteLog;
using UtilityLibrary;

namespace CreateCsvFromLogAddressesGood
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    //private void AddField(List<string> fields, string field)
    //{
    //  field = field.Replace("\"", "\"\""); // double up quotes
    //  fields.Add("\"" + field + "\""); // enclose in quotes
    //}

    private void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    //
    // Event handlers
    //

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      TextWriter writer = null;
      try
      {
        writer = new StreamWriter(OutputFileTextBox.Text);
        int count = 0;
        //List<string> fields = new List<string>();
        SimpleCsvWriter csvWriter = new SimpleCsvWriter();
        // newTable used only to get at column info
        LogAddressesGoodNewTable newTable = new LogAddressesGoodNewTable();
        using (var reader = LogAddressesGood.GetAllDataReader(0))
        {
          while (reader.Read())
          {
            //fields.Clear();
            //fields.Add("NULL"); // Id (will auto-increment)
            //AddField(fields, reader.DateStamp.ToString("yyyy-MM-dd HH:mm:ss")); // DateStamp
            //AddField(fields, ""); // Email
            //AddField(fields, ""); // RawAddress
            //AddField(fields, ""); // SelectedStateCode
            csvWriter.AddNull(); // Id (will auto-increment)
            csvWriter.AddField(reader.DateStamp.ToString("yyyy-MM-dd HH:mm:ss")); // DateStamp
            csvWriter.AddField(""); // Email
            csvWriter.AddField(""); // RawAddress
            csvWriter.AddField(""); // SelectedStateCode
            // concatenate the address fields
            string address = reader.Address1.Trim();
            string address2 = reader.Address2.Trim();
            if (address.Length == 0)
              address = address2;
            else if (address2.Length > 0)
              address = address + ", " + address2;
            //AddField(fields, newTable.ParsedAddressColumn.Truncate(address));
            //AddField(fields, newTable.ParsedCityColumn.Truncate(reader.City));
            //AddField(fields, newTable.ParsedStateCodeColumn.Truncate(reader.StateCode));
            //AddField(fields, newTable.ParsedZip5Column.Truncate(reader.Zip5));
            //AddField(fields, newTable.ParsedZip4Column.Truncate(reader.Zip4));
            csvWriter.AddField(newTable.ParsedAddressColumn.Truncate(address));
            csvWriter.AddField(newTable.ParsedCityColumn.Truncate(reader.City));
            csvWriter.AddField(newTable.ParsedStateCodeColumn.Truncate(reader.StateCode));
            csvWriter.AddField(newTable.ParsedZip5Column.Truncate(reader.Zip5));
            csvWriter.AddField(newTable.ParsedZip4Column.Truncate(reader.Zip4));
            //writer.WriteLine(string.Join(",", fields));
            csvWriter.Write(writer);
            count++;
            if ((count % 10000) == 0)
              AppendStatusText("{0} rows written", count);
          }
          AppendStatusText("{0} rows written", count);
          AppendStatusText("Finished", count);
        }
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
        AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
        AppendStatusText("Terminated.");
      }
      finally
      {
        if (writer != null)
          writer.Close();
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      BrowseOutputFileButton.Enabled = true;
      OutputFileTextBox.Enabled = true;
    }

    private void BrowseOutputFileButton_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(OutputFileTextBox.Text))
        SaveOutputFileDialog.FileName = "AddressesGood.csv";
      else
        SaveOutputFileDialog.FileName = OutputFileTextBox.Text;
      if (SaveOutputFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = SaveOutputFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseOutputFileButton.Enabled = false;
      OutputFileTextBox.Enabled = false;
      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }
  }
}
