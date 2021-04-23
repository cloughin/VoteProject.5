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

namespace UpdateZipCitiesDatabase
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();

      SetConnectionString();
    }

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

    private void ApplyUpdates()
    {
      AppendStatusText("Starting...");

      FileInfo fileInfo = new FileInfo(ZipPathTextBox.Text);
      if (!fileInfo.Exists)
        throw new VoteException("File \"{0}\" does not exist.", fileInfo.FullName);

      using (ZipInputStream zipStream = new ZipInputStream(fileInfo.FullName))
      {
        string xlsFileName = fileInfo.Name;
        // Replace zip extension with "xls"
        xlsFileName = xlsFileName.Replace(".zip", ".xls");

        // Find the proper entry
        ZipEntry zipEntry = null;
        do
        {
          zipEntry = zipStream.GetNextEntry();
        } while (zipEntry != null && 
          !string.Equals(zipEntry.FileName, xlsFileName, StringComparison.OrdinalIgnoreCase));
        if (zipEntry == null)
          throw new VoteException("XLS file \"{0}\" not found in archive \"{1}\".",
            xlsFileName, fileInfo.FullName);
        TextReader textReader = new StreamReader(zipStream, zipEntry.ActualEncoding);
        ApplyUpdates(textReader);
      }

      AppendStatusText("Completed.");
    }

    private void ApplyUpdates(TextReader textReader)
    {
      // In suppressUpdate mode, this keeps track of rows that would have been deleted
      Dictionary<string, object> phantomDeletes = new Dictionary<string, object>();

      int adds = 0;
      int deletes = 0;
      int couldNotAdd = 0;
      int couldNotDelete = 0;
      int rowCount = 0;
      bool suppressUpdate = this.Invoke(() => SuppressUpdateCheckBox.Checked);

      string actionField = "Type";

      using (var csvReader = new CsvReader(textReader, true, '\t'))
      {
        int fieldCount = csvReader.FieldCount;
        string[] headers = csvReader.GetFieldHeaders();

        while (csvReader.ReadNextRecord())
        {
          string action = csvReader[actionField].ToString();
          string zipCode = csvReader["ZipCode"].ToString();
          string cityAliasName = csvReader["CityAliasName"].ToString();
          string key = zipCode + cityAliasName;

          switch (action)
          {
            case "A":
              if (suppressUpdate)
              {
                if (!phantomDeletes.ContainsKey(key) &&
                  DB.VoteZipNew.ZipCitiesDownloaded.PrimaryKeyExists(zipCode, cityAliasName))
                  couldNotAdd++;
                else
                  adds++;
              }
              else
              {
                try
                {
                  var table = new DB.VoteZipNew.ZipCitiesDownloadedTable();
                  var row = table.NewRow();
                  foreach (string field in headers)
                    if (field != actionField)
                      row[field] = csvReader[field];
                  string metaphoneAliasName = DoubleMetaphone.EncodePhrase(row.CityAliasName);
                  if (metaphoneAliasName.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength)
                    metaphoneAliasName =
                      metaphoneAliasName.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength);
                  string metaphoneAliasAbbreviation = DoubleMetaphone.EncodePhrase(row.CityAliasAbbreviation);
                  if (metaphoneAliasAbbreviation.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength)
                    metaphoneAliasAbbreviation =
                      metaphoneAliasAbbreviation.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength);
                  row.MetaphoneAliasName = metaphoneAliasName;
                  row.MetaphoneAliasAbbreviation = metaphoneAliasAbbreviation;
                  table.AddRow(row);
                  DB.VoteZipNew.ZipCitiesDownloaded.UpdateTable(table);
                  adds++;
                }
                catch
                {
                  couldNotAdd++;
                }
              }
              break;

            case "D":
              if (suppressUpdate)
              {
                if (DB.VoteZipNew.ZipCitiesDownloaded.PrimaryKeyExists(zipCode, cityAliasName))
                {
                  deletes++;
                  if (!phantomDeletes.ContainsKey(key))
                    phantomDeletes.Add(key, null);
                }
                else
                  couldNotDelete++;
              }
              else
              {
                try
                {
                  int deleted = DB.VoteZipNew.ZipCitiesDownloaded.DeleteByPrimaryKey(zipCode, cityAliasName);
                  if (deleted != 1)
                    throw new VoteException();
                  deletes++;
                }
                catch
                {
                  couldNotDelete++;
                }
              }
             break;

            default:
              AppendStatusText("Invalid action: {0}", csvReader[actionField]);
              break;
          }
          rowCount++;
        }
      }

      AppendStatusText("Processed data,  {0} rows", rowCount);
      AppendStatusText("Adds: {0}", adds);
      AppendStatusText("Deletes: {0}", deletes);
      AppendStatusText("Could not add: {0}", couldNotAdd);
      AppendStatusText("Could not delete: {0}", couldNotDelete);
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
      DB.VoteZipNew.VoteZipNewDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ApplyUpdates();
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
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      BrowseZipPathButton.Enabled = true;
      ZipPathTextBox.Enabled = true;
      ServerGroupBox.Enabled = true;
      SuppressUpdateCheckBox.Enabled = true;
    }

    private void BrowseZipPathButton_Click(object sender, EventArgs e)
    {
      OpenZipFileDialog.FileName = ZipPathTextBox.Text;
      if (OpenZipFileDialog.ShowDialog() == DialogResult.OK)
        ZipPathTextBox.Text = OpenZipFileDialog.FileName;
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
      BrowseZipPathButton.Enabled = false;
      ZipPathTextBox.Enabled = false;
      ServerGroupBox.Enabled = false;
      SuppressUpdateCheckBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
