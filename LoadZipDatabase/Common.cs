using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vote;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;

namespace LoadZipDatabase
{
  class Common
  {
    readonly TextBox _StatusTextBox;
    TextWriter _TextWriter;

    public Common(TextBox statusTextBox)
    {
      _StatusTextBox = statusTextBox;
    }

    internal void AppendStatusText(string text)
    {
      if (text == null) throw new ArgumentNullException("text");
      var form = _StatusTextBox.Parent as Form;
      if (_StatusTextBox.Text.Length != 0)
        form.Invoke(() => _StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      form.Invoke(() => _StatusTextBox.AppendText(text));
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    internal void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private string FixNumericPrimaryAddress(string field, string address)
    {
      if (!string.IsNullOrEmpty(address) &&
       address.IsDigits() &&
       address.Length < 10)
        return address.ZeroPad(10);
      if (address.Length <= 10) return address;
      AppendStatusText("{0}: \"{1}\" left truncated to 10 characters", field, address);
      address = address.Substring(address.Length - 10);
      return address;
    }

    private string FixupField(string field, string value)
    {
      switch (field)
      {
        case "AddressPrimaryLowNumber":
        case "AddressPrimaryHighNumber":
          value = FixNumericPrimaryAddress(field, value);
          break;
        case "AddressPrimaryEvenOdd":
          value = TruncateTo(field, value, 1);
          break;
        case "AddressSecAbbr":
          value = TruncateTo(field, value, 4);
          break;
        case "AddressSecHighNumber":
          value = TruncateTo(field, value, 10);
          break;
        case "AddressSecLowNumber":
          value = TruncateTo(field, value, 10);
          break;
        case "AddressSecOddEven":
          value = TruncateTo(field, value, 1);
          break;
        case "BaseAlternateCode":
          value = TruncateTo(field, value, 1);
          break;
        case "BuildingName":
          value = TruncateTo(field, value, 40);
          break;
        case "CarrierRoute":
          value = TruncateTo(field, value, 4);
          break;
        case "FinanceNumber":
          value = TruncateTo(field, value, 6);
          break;
        case "GovernmentBuilding":
          value = TruncateTo(field, value, 1);
          break;
        case "LACSStatus":
          value = TruncateTo(field, value, 1);
          break;
        case "LatLonMultiMatch":
          value = TruncateTo(field, value, 50);
          break;
        case "StName":
          value = TruncateTo(field, value, 28);
          break;
        case "StPostDirAbbr":
          value = TruncateTo(field, value, 2);
          break;
        case "StPreDirAbbr":
          value = TruncateTo(field, value, 2);
          break;
        case "StSuffixAbbr":
          value = TruncateTo(field, value, 4);
          break;
        case "TLID":
          value = TruncateTo(field, value, 10);
          break;
      }
      return value;
    }

    private int LoadState(TextReader textReader, int skipRows)
    {
      var rowCount = 0;
      var updateErrorCount = 0;
      using (var csvReader = new CsvReader(textReader, true))
      {
        var headers = csvReader.GetFieldHeaders();

        // process skips
        if (skipRows > 0)
        {
          AppendStatusText("Skipping {0} rows", skipRows);
          while (skipRows > 0 && csvReader.ReadNextRecord())
          {
            skipRows--;
            rowCount++;
          }
        }

        if (ToCsv)
        {
          var csvWriter = new SimpleCsvWriter();
          while (csvReader.ReadNextRecord())
          {
            var error = false;
            rowCount++;
            try
            {
              foreach (var field in headers)
              {
                var value = csvReader[field];
                // The incoming data has a few anomolies...
                value = FixupField(field, value);
                //if (field == "AddressPrimaryLowNumber" ||
                //  field == "AddressPrimaryHighNumber")
                //  value = FixNumericPrimaryAddress(value);
                csvWriter.AddField(value);
              }
            }
            catch (Exception ex)
            {
              error = true;
              AppendStatusText("Row {0} skipped due to {1}", rowCount, ex.Message);
            }
            if (!error)
              csvWriter.Write(_TextWriter);
            else
              csvWriter.Clear();
            if (rowCount % 50000 == 0) // report every 50000 rows
              AppendStatusText("{0} rows written", rowCount);
          }
        }
        else
        {
          var table = new DB.VoteZipNew.ZipStreetsDownloadedTable();
          while (csvReader.ReadNextRecord())
          {
            var row = table.NewRow();
            var error = false;
            rowCount++;
            try
            {
              foreach (var field in headers)
              {
                var value = csvReader[field];
                // The incoming data has a few anomolies...
                value = FixupField(field, value);
                //if (field == "AddressPrimaryLowNumber" ||
                //  field == "AddressPrimaryHighNumber")
                //  value = FixNumericPrimaryAddress(field, value);
                row[field] = value;
              }
            }
            catch (Exception ex)
            {
              error = true;
              AppendStatusText("Row {0} skipped due to {1}", rowCount, ex.Message);
            }
            if (!error)
              table.AddRow(row);
            if (rowCount % 1000 == 0) // flush every 1000 rows
            {
              UpdateZipStreetsDownloaded(table, ref updateErrorCount);
              table = new DB.VoteZipNew.ZipStreetsDownloadedTable();
            }
            if (rowCount % 50000 == 0) // report every 50000 rows
              AppendStatusText("{0} rows written", rowCount);
          }
          UpdateZipStreetsDownloaded(table, ref updateErrorCount);
        }
      }
      return rowCount;
    }

    internal void ProcessOneState(DirectoryInfo directoryInfo, string stateCode,
      TextWriter writer)
    {
      _TextWriter = writer;
      ProcessOneState(directoryInfo, stateCode, 0);
    }

    internal  void ProcessOneState(DirectoryInfo directoryInfo, string stateCode, int skipRows)
    {
      AppendStatusText("Processing state {0}", stateCode);
      var zipArchiveName = string.Format("ZIP4-{0}.zip", stateCode);
      var zipCsvFileName = string.Format("ZIP4-{0}.csv", stateCode);
      var zipArchiveFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, zipArchiveName));
      if (!zipArchiveFileInfo.Exists)
      {
        //throw new VoteException("File \"{0}\" does not exist.", zipArchiveFileInfo.FullName);
        AppendStatusText("**>>State {0} is missing", stateCode);
        return;
      }
      using (var zipStream = new ZipInputStream(zipArchiveFileInfo.FullName))
      {
        // Find the proper entry
        ZipEntry zipEntry;
        do
        {
          zipEntry = zipStream.GetNextEntry();
        } while (zipEntry != null && zipEntry.FileName != zipCsvFileName);
        if (zipEntry == null)
          throw new VoteException("CSV file \"{0}\" not found in archive \"{1}\".",
            zipCsvFileName, zipArchiveFileInfo.FullName);
        TextReader textReader = new StreamReader(zipStream, zipEntry.ActualEncoding);
        var rowCount = LoadState(textReader, skipRows);
        AppendStatusText("Processed state {0}, {1} rows", stateCode, rowCount);
      }
    }

    private bool ToCsv
    {
      get
      {
        return _TextWriter != null;
      }
    }

    private string TruncateTo(string field, string value, int max)
    {
      if (value.Length > max)
      {
        AppendStatusText("{0}: \"{1}\" truncated to {2} characters", field, value, max);
        value = value.Substring(0, max);
      }
      return value;
    }

    private void UpdateZipStreetsDownloaded(DB.VoteZipNew.ZipStreetsDownloadedTable table,
      ref int updateErrorCount)
    {
      DB.VoteZipNew.ZipStreetsDownloaded.UpdateTable(table, 0,
        ConflictOption.CompareAllSearchableValues, true);
      var errors = table.Count(row => row.HasErrors);
      if (errors > 0)
      {
        updateErrorCount += errors;
        AppendStatusText("{0} update errors", updateErrorCount);
      }
    }
  }
}
