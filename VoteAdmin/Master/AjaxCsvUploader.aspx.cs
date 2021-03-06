using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DB.Vote;
using Jayrock.Json.Conversion;
using LumenWorks.Framework.IO.Csv;
using static System.String;

namespace Vote.Master
{
  public partial class AjaxCsvUploaderPage : SecurePage
  {
    #region Private

    private void WriteJsonResultToResponse(AjaxResponse result)
    {
      Response.Write(JsonConvert.ExportToString(result));
      Response.End();
    }

    #endregion Private

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    // This must be public so that the JsonConvert class can see it
    // ReSharper disable NotAccessedField.Global
    public class AjaxResponse
    {
      public bool Success;
      public string Message;
      public string GroupName;
      public string UploadId;
    }

    // ReSharper restore NotAccessedField.Global

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public

    // This setup is used to handle inconsistent column naming by BallotPedia
    private readonly Dictionary<string, string[]> _NameDictionary =
      new Dictionary<string, string[]>
      {
        {"FirstName", new string[0]},
        {"LastName", new string[0]},
        {"Page Title", new string[0]},
        {"State", new string[0]},
        {"Name", new string[0]},
        {"Status", new string[0]},
        {"Office", new[] {"Position"}},
        {"Party", new string[0]}
      };

    public string[] NormalizeHeaders(IEnumerable<string> headers)
    {
      var normalized = new List<string>(headers);

      // make sure all required headers are present
      foreach (var kvp in _NameDictionary)
      {
        var ok = false;
        if (normalized.Contains(kvp.Key)) ok = true;
        else
        {
          foreach (var alt in kvp.Value)
          {
            var index = normalized.FindIndex(s => s == alt);
            if (index < 0) continue;
            normalized[index] = kvp.Key;
            ok = true;
          }
        }
        if (!ok) throw new VoteException(kvp.Key + " column missing");
      }
      return normalized.ToArray();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var result = new AjaxResponse();

      try
      {
        // assume success unless an exception is thrown
        result.Success = true;

        // copy back the groupname and uploadid
        result.GroupName = Request.Form["groupname"];
        result.UploadId = Request.Form["uploadid"];

        // there should always be exactly one file
        if (Request.Files.Count == 0) throw new VoteException("The upload file is missing");
        if (Request.Files.Count > 1)
          throw new VoteException("Unexpected files in the upload package");

        // Test error handling
        //throw new VoteException("Some weird-ass error");

        // get the file
        var postedFile = Request.Files[0];
        var filename = postedFile.FileName;
        var saveAs = Request.Form["saveas"];
        if (!IsNullOrWhiteSpace(saveAs)) filename = saveAs;
        if (!filename.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
          filename += ".csv";
        var duplicate = BallotPediaCsvs.FilenameExists(filename);
        if (duplicate && Request.Form["overwrite"] != "true")
          throw new VoteException(
            "A CSV with this filename has already been uploaded. Either rename the file or check the 'overwrite existing file' box.");

        // save the timestamp so the time we post to all the various tables matches
        // exactly...
        var uploadTime = DateTime.UtcNow;

        string csv;
        var coded = 0;
        var candidateCount = 0;
        using (var csvReader =
          new CsvReader(new StreamReader(postedFile.InputStream, Encoding.UTF8), true))
        {
          var headers = csvReader.GetFieldHeaders();
          var normalizedHeaders = NormalizeHeaders(headers);
          var columnsToAdd = new List<string>();
          if (!headers.Contains("VoteUSA Id")) columnsToAdd.Add("VoteUSA Id");
          if (!headers.Contains("VoteUSA Url")) columnsToAdd.Add("VoteUSA Url");

          using (var ms = new MemoryStream())
          {
            var streamWriter = new StreamWriter(ms);
            var csvWriter = new SimpleCsvWriter();

            // write headers
            foreach (var col in normalizedHeaders) csvWriter.AddField(col);
            foreach (var col in columnsToAdd) csvWriter.AddField(col);
            csvWriter.Write(streamWriter);
            while (csvReader.ReadNextRecord())
            {
              foreach (var col in headers)
              {
                var value = csvReader[col];
                csvWriter.AddField(value);
                if (col == "VoteUSA Id" && !IsNullOrWhiteSpace(value)) coded++;
              }
              // ReSharper disable once UnusedVariable
              foreach (var col in columnsToAdd) csvWriter.AddField(Empty);
              csvWriter.Write(streamWriter);
              candidateCount++;
            }
            streamWriter.Flush();
            ms.Position = 0;
            csv = new StreamReader(ms).ReadToEnd();
          }
        }

        if (duplicate) BallotPediaCsvs.DeleteByFilename(filename);
        BallotPediaCsvs.Insert(filename, uploadTime, csv, candidateCount, coded, false);

        result.Message = "The CSV uploaded successfully.";
      }
      catch (Exception ex)
      {
        result.Success = false;
        result.Message = ex.Message;
      }

      WriteJsonResultToResponse(result);
    }
  }
}