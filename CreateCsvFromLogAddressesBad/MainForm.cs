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

namespace CreateCsvFromLogAddressesBad
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
        LogAddressesBadNewTable newTable = new LogAddressesBadNewTable();
        using (var reader = LogAddressesBad.GetAllDataReader(0))
        {
          while (reader.Read())
          {
            //fields.Clear();
            //fields.Add("NULL"); // Id (will auto-increment)
            csvWriter.AddNull(); // Id (will auto-increment)
            //AddField(fields, reader.DateStamp.ToString("yyyy-MM-dd HH:mm:ss")); // DateStamp
            //AddField(fields, ""); // Email
            csvWriter.AddField(reader.DateStamp.ToString("yyyy-MM-dd HH:mm:ss")); // DateStamp
            csvWriter.AddField(""); // Email
            // concatenate the address fields using pipes, to indicate old fixed format
            string address = 
              reader.Address1.Trim() + "|" +
              reader.Address2.Trim() + "|" +
              reader.City.Trim() + "|" +
              reader.StateCode.Trim();
            //AddField(fields, newTable.RawAddressColumn.Truncate(address)); // RawAddress
            //AddField(fields, ""); // ErrorMessage
            csvWriter.AddField(newTable.RawAddressColumn.Truncate(address)); // RawAddress
            csvWriter.AddField(""); // ErrorMessage
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
        SaveOutputFileDialog.FileName = "AddressesBad.csv";
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
