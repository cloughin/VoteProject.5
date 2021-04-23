using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;

namespace LoadZipCitiesDatabase
{
  public partial class DbMainForm : Form
  {
    Common Common;

    public DbMainForm()
    {
      InitializeComponent();
      Common = new Common(this.StatusTextBox);

      SetConnectionString();
    }

    //private void LoadCities()
    //{
    //  Common.AppendStatusText("Starting...");

    //  FileInfo fileInfo = new FileInfo(CsvPathTextBox.Text);
    //  if (!fileInfo.Exists)
    //    throw new VoteException("File \"{0}\" does not exist.", fileInfo.FullName);

    //  if (MessageBox.Show("OK to truncate ZipCitiesDownloaded table?", "Confirm Truncation",
    //    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
    //    DialogResult.OK)
    //    throw new VoteException("Cancelled by user.");
    //  DB.VoteZipNew.ZipCitiesDownloaded.TruncateTable();

    //  using (ZipInputStream zipStream = new ZipInputStream(fileInfo.FullName))
    //  {
    //    string zipCsvFileName = "zip-codes-database-STANDARD.csv";

    //    // Find the proper entry
    //    ZipEntry zipEntry = null;
    //    do
    //    {
    //      zipEntry = zipStream.GetNextEntry();
    //    } while (zipEntry != null && zipEntry.FileName != zipCsvFileName);
    //    if (zipEntry == null)
    //      throw new VoteException("CSV file \"{0}\" not found in archive \"{1}\".",
    //        zipCsvFileName, fileInfo.FullName);
    //    TextReader reader = new StreamReader(zipStream, zipEntry.ActualEncoding);
    //    int rowCount = LoadData(reader);
    //    Common.AppendStatusText("Processed data,  {0} rows", rowCount);
    //  }

    //  Common.AppendStatusText("Completed.");
    //}

    //private int LoadData(TextReader textReader)
    //{
    //  int rowCount = 0;
    //  using (var csvReader = new CsvReader(textReader, true))
    //  {
    //    int fieldCount = csvReader.FieldCount;
    //    string[] headers = csvReader.GetFieldHeaders();

    //    var table = new DB.VoteZipNew.ZipCitiesDownloadedTable();
    //    while (csvReader.ReadNextRecord())
    //    {
    //      var row = table.NewRow();
    //      foreach (string field in headers)
    //        row[field] = csvReader[field];
    //      string metaphoneAliasName = DoubleMetaphone.EncodePhrase(row.CityAliasName);
    //      if (metaphoneAliasName.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength)
    //        metaphoneAliasName =
    //          metaphoneAliasName.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength);
    //      string metaphoneAliasAbbreviation = DoubleMetaphone.EncodePhrase(row.CityAliasAbbreviation);
    //      if (metaphoneAliasAbbreviation.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength)
    //        metaphoneAliasAbbreviation =
    //          metaphoneAliasAbbreviation.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength);
    //      row.MetaphoneAliasName = metaphoneAliasName;
    //      row.MetaphoneAliasAbbreviation = metaphoneAliasAbbreviation;
    //      table.AddRow(row);
    //      rowCount++;
    //      if (rowCount % 1000 == 0) // flush every 1000 rows
    //      {
    //        DB.VoteZipNew.ZipCitiesDownloaded.UpdateTable(table, 0);
    //        table = new DB.VoteZipNew.ZipCitiesDownloadedTable();
    //      }
    //    }
    //    DB.VoteZipNew.ZipCitiesDownloaded.UpdateTable(table);
    //  }
    //  return rowCount;
    //}

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
      DB.VoteZipNew.VoteZipNewDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        Common.LoadCities(CsvPathTextBox.Text);
      }
      catch (VoteException ex)
      {
        Common.AppendStatusText(ex.Message);
        Common.AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        Common.AppendStatusText(ex.ToString());
        Common.AppendStatusText("Terminated.");
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      BrowseCsvPathButton.Enabled = true;
      CsvPathTextBox.Enabled = true;
      ServerGroupBox.Enabled = true;
    }

    private void BrowseCsvPathButton_Click(object sender, EventArgs e)
    {
      OpenCsvFileDialog.FileName = CsvPathTextBox.Text;
      if (OpenCsvFileDialog.ShowDialog() == DialogResult.OK)
        CsvPathTextBox.Text = OpenCsvFileDialog.FileName;
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
        Common.AppendStatusText(ex.Message);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseCsvPathButton.Enabled = false;
      CsvPathTextBox.Enabled = false;
      ServerGroupBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
