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

namespace UpdateZip4Database
{
  public partial class MainForm : Form
  {
    Dictionary<StreetInfo, object> UpdatedStreets;

    public MainForm()
    {
      InitializeComponent();

      SetConnectionString();
    }

    private void AddToList(CsvReader csvReader)
    {
      StreetInfo streetInfo = new StreetInfo(
        csvReader["ZipCode"] as string,
        csvReader["StPreDirAbbr"] as string,
        csvReader["StName"] as string,
        csvReader["StSuffixAbbr"] as string,
        csvReader["StPostDirAbbr"] as string);
      UpdatedStreets[streetInfo] = null;
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

      UpdatedStreets = new Dictionary<StreetInfo, object>();

      FileInfo fileInfo = new FileInfo(ZipPathTextBox.Text);
      if (!fileInfo.Exists)
        throw new VoteException("File \"{0}\" does not exist.", fileInfo.FullName);

      switch (fileInfo.Extension.ToLowerInvariant())
      {
        case ".zip":
          Encoding encoding;
          using (ZipInputStream zipStream = GetZipInputStream(fileInfo, out encoding))
          {
            TextReader textReader = new StreamReader(zipStream, encoding);
            if (!ReportRowCount(textReader)) return;
          }

          using (ZipInputStream zipStream = GetZipInputStream(fileInfo, out encoding))
          {
            TextReader textReader = new StreamReader(zipStream, encoding);
            ApplyUpdates(textReader);
          }
          break;

        case ".csv":
          {
            TextReader textReader = new StreamReader(fileInfo.FullName);
            if (!ReportRowCount(textReader)) return;
            textReader = new StreamReader(fileInfo.FullName);
            ApplyUpdates(textReader);
          }
          break;

        default:
          AppendStatusText("Invalid file type {0}", fileInfo.Extension);
          return;
      }

      if (!this.Invoke(() => SuppressUpdateCheckBox.Checked))
      {
        AppendStatusText("Updating ZipStreetsUpdatesNeeded");
        foreach (StreetInfo street in UpdatedStreets.Keys)
        {
          if (!DB.VoteZipNew.ZipStreetsUpdatesNeeded.FullStreetNameExists(zipCode: street.ZipCode, directionPrefix: street.DirectionPrefix, streetName: street.StreetName, streetSuffix: street.StreetSuffix, directionSuffix: street.DirectionSuffix))
            DB.VoteZipNew.ZipStreetsUpdatesNeeded.Insert(zipCode: street.ZipCode, directionPrefix: street.DirectionPrefix, streetName: street.StreetName, streetSuffix: street.StreetSuffix, directionSuffix: street.DirectionSuffix);
        }
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

      string actionField = "Action";

      using (var csvReader = new CsvReader(textReader, true))
      {
        int fieldCount = csvReader.FieldCount;
        string[] headers = csvReader.GetFieldHeaders();

        while (csvReader.ReadNextRecord())
        {
          string action = csvReader[actionField].ToString();
          string updateKey = csvReader["UpdateKey"].ToString();

          UpdatedStreets[new StreetInfo(
            csvReader["ZipCode"],
            csvReader["StPreDirAbbr"],
            csvReader["StName"],
            csvReader["StSuffixAbbr"],
            csvReader["StPostDirAbbr"])] = null;

          switch (action)
          {
            case "A":
              if (suppressUpdate)
              {
                if (!phantomDeletes.ContainsKey(updateKey) &&
                  DB.VoteZipNew.ZipStreetsDownloaded.UpdateKeyExists(updateKey))
                  couldNotAdd++;
                else
                  adds++;
              }
              else
              {
                try
                {
                  var table = new DB.VoteZipNew.ZipStreetsDownloadedTable();
                  var row = table.NewRow();
                  foreach (string field in headers)
                  {
                    string value = csvReader[field];
                    // The incoming data has a few anomolies...
                    if (field == "AddressPrimaryLowNumber" ||
                      field == "AddressPrimaryHighNumber")
                      value = FixNumericPrimaryAddress(value);
                    row[field] = value;
                  }
                  table.AddRow(row);
                  DB.VoteZipNew.ZipStreetsDownloaded.UpdateTable(table, 0);
                  adds++;
                  AddToList(csvReader);
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
                if (DB.VoteZipNew.ZipStreetsDownloaded.UpdateKeyExists(updateKey))
                {
                  deletes++;
                  if (!phantomDeletes.ContainsKey(updateKey))
                    phantomDeletes.Add(updateKey, null);
                }
                else
                  couldNotDelete++;
              }
              else
              {
                try
                {
                  int deleted = DB.VoteZipNew.ZipStreetsDownloaded.DeleteByUpdateKey(updateKey, 0);
                  if (deleted != 1)
                    throw new VoteException();
                  deletes++;
                  AddToList(csvReader);
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
          if (rowCount % 1000 == 0)
            AppendStatusText("{0} rows processed", rowCount);
        }
      }

      AppendStatusText("Processed data,  {0} rows", rowCount);
      AppendStatusText("Adds: {0}", adds);
      AppendStatusText("Deletes: {0}", deletes);
      AppendStatusText("Could not add: {0}", couldNotAdd);
      AppendStatusText("Could not delete: {0}", couldNotDelete);
    }

    string FixNumericPrimaryAddress(string address)
    {
      if (!string.IsNullOrEmpty(address) &&
        address.IsDigits() &&
        address.Length < 10)
        return address.ZeroPad(10);
      else
        return address;
    }

    static ZipInputStream GetZipInputStream(FileInfo fileInfo, out Encoding encoding)
    {
      ZipInputStream zipStream = new ZipInputStream(fileInfo.FullName);

      string csvFileName = fileInfo.Name;
      // Replace zip extension with "csv"
      csvFileName = csvFileName.Replace(".zip", ".csv");

      // Find the proper entry
      ZipEntry zipEntry = null;
      do
      {
        zipEntry = zipStream.GetNextEntry();
      } while (zipEntry != null &&
         !string.Equals(Path.GetExtension(zipEntry.FileName), ".csv", 
         StringComparison.OrdinalIgnoreCase));

      if (zipEntry == null)
        throw new VoteException("No CSV file found in archive \"{0}\".",
          fileInfo.FullName);

      encoding = zipEntry.ActualEncoding;
      return zipStream;
    }

    private bool ReportRowCount(TextReader textReader)
    {
      int count = 0;

      try
      {
        using (var csvReader = new CsvReader(textReader, true))
          while (csvReader.ReadNextRecord())
            count++;
      }
      catch (Exception ex)
      {
        AppendStatusText("Error attempting to read row {0}", count + 1);
        AppendStatusText(ex.ToString());
        AppendStatusText("Terminated.");
        return false;
      }

      AppendStatusText("Reading {0} rows", count);
      return true;
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
