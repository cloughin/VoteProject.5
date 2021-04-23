using System;
using System.Collections.Generic;
using System.Data;
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
  class Common
  {
    TextBox StatusTextBox;
    string OutputFilePath;

    public Common(TextBox statusTextBox)
    {
      StatusTextBox = statusTextBox;
    }

    //private static void AddField(List<string> fields, string field)
    //{
    //  field = field.Replace("\"", "\"\""); // double up quotes
    //  fields.Add("\"" + field + "\""); // enclose in quotes
    //}

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

    internal void LoadCities(string inputFilePath)
    {
      LoadCities(inputFilePath, null);
    }

    internal void LoadCities(string inputFilePath, string outputFilePath)
    {
      OutputFilePath = outputFilePath;
      AppendStatusText("Starting...");

      FileInfo fileInfo = new FileInfo(inputFilePath);
      if (!fileInfo.Exists)
        throw new VoteException("File \"{0}\" does not exist.", fileInfo.FullName);

      if (!ToCsv)
      {
        if (MessageBox.Show("OK to truncate ZipCitiesDownloaded table?", "Confirm Truncation",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
          DialogResult.OK)
          throw new VoteException("Cancelled by user.");
        DB.VoteZipNew.ZipCitiesDownloaded.TruncateTable();
      }

      using (ZipInputStream zipStream = new ZipInputStream(fileInfo.FullName))
      {
        string zipCsvFileName = "zip-codes-database-STANDARD.csv";

        // Find the proper entry
        ZipEntry zipEntry = null;
        do
        {
          zipEntry = zipStream.GetNextEntry();
        } while (zipEntry != null && zipEntry.FileName != zipCsvFileName);
        if (zipEntry == null)
          throw new VoteException("CSV file \"{0}\" not found in archive \"{1}\".",
            zipCsvFileName, fileInfo.FullName);
        TextReader reader = new StreamReader(zipStream, zipEntry.ActualEncoding);
        int rowCount = LoadData(reader);
        AppendStatusText("Processed data,  {0} rows", rowCount);
      }

      AppendStatusText("Completed.");
    }

    private int LoadData(TextReader textReader)
    {
      TextWriter writer = null;
      int rowCount = 0;
      try
      {
        if (ToCsv)
          writer = new StreamWriter(OutputFilePath);
        using (var csvReader = new CsvReader(textReader, true))
        {
          int fieldCount = csvReader.FieldCount;
          string[] headers = csvReader.GetFieldHeaders();

          DB.VoteZipNew.ZipCitiesDownloadedTable table = null;
          if (!ToCsv)
            table = new DB.VoteZipNew.ZipCitiesDownloadedTable();
          DB.VoteZipNew.ZipCitiesDownloadedRow row;
          //List<string> fields = new List<string>();
          SimpleCsvWriter csvWriter = new SimpleCsvWriter();
          while (csvReader.ReadNextRecord())
          {
            string cityAliasName = csvReader["CityAliasName"];
            string cityAliasAbbreviation = csvReader["CityAliasAbbreviation"];
            string metaphoneAliasName = DoubleMetaphone.EncodePhrase(cityAliasName);
            if (metaphoneAliasName.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength)
              metaphoneAliasName =
                metaphoneAliasName.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasNameMaxLength);
            string metaphoneAliasAbbreviation = DoubleMetaphone.EncodePhrase(cityAliasAbbreviation);
            if (metaphoneAliasAbbreviation.Length > DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength)
              metaphoneAliasAbbreviation =
                metaphoneAliasAbbreviation.Substring(0, DB.VoteZipNew.ZipCitiesDownloaded.MetaphoneAliasAbbreviationMaxLength);
            rowCount++;
            if (ToCsv)
            {
              //fields.Clear();
              foreach (string field in headers)
                //AddField(fields, csvReader[field]);
                csvWriter.AddField(csvReader[field]);
              //AddField(fields, metaphoneAliasName);
              //AddField(fields, metaphoneAliasAbbreviation);
              csvWriter.AddField(metaphoneAliasName);
              csvWriter.AddField(metaphoneAliasAbbreviation);
              //writer.WriteLine(string.Join(",", fields));
              csvWriter.Write(writer);
            }
            else
            {
              row = table.NewRow();
              foreach (string field in headers)
                row[field] = csvReader[field];
              row.MetaphoneAliasName = metaphoneAliasName;
              row.MetaphoneAliasAbbreviation = metaphoneAliasAbbreviation;
              table.AddRow(row);
              if (rowCount % 1000 == 0) // flush every 1000 rows
              {
                DB.VoteZipNew.ZipCitiesDownloaded.UpdateTable(table, 0);
                table = new DB.VoteZipNew.ZipCitiesDownloadedTable();
              }
            }
          }
          if (!ToCsv)
            DB.VoteZipNew.ZipCitiesDownloaded.UpdateTable(table);
        }
      }
      finally
      {
        if (writer != null)
          writer.Close();
      }
      return rowCount;
    }

    private bool ToCsv
    {
      get
      {
        return OutputFilePath != null;
      }
    }
  }
}
