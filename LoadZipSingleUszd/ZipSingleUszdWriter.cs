using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DB.VoteZipNewLocal;
using Vote;

namespace LoadZipSingleUszd
{
  public class ZipSingleUszdWriter
  {
    private Action<string> _FeedbackDelegate;
    private readonly TextWriter _TextWriter;

    public ZipSingleUszdWriter(TextWriter writer)
    {
      _TextWriter = writer;
    }

    //private static void AddField(List<string> fields, string field)
    //{
    //  field = field.Replace("\"", "\"\""); // double up quotes
    //  fields.Add("\"" + field + "\""); // enclose in quotes
    //}

    private static IEnumerable<string> CreateZipPlus4List(
      IEnumerable<ZipStreetsDownloadedRow> table)
    {
      var zip4Dictionary = new Dictionary<string, object>();
      foreach (var row in table)
        if (row.Plus4Low == row.Plus4High)
          zip4Dictionary[row.Plus4Low] = null;
        else
        {
          var low = int.Parse(row.Plus4Low);
          var high = int.Parse(row.Plus4High);
          for (var n = low; n <= high; n++)
            zip4Dictionary[n.ToString(CultureInfo.InvariantCulture)
              .ZeroPad(4)] = null;
        }
      return zip4Dictionary.Keys;
    }

    public void DoOneZipCode(string zipCode)
    {
      // Get all zip rows from ZipStreetsDownloaded
      var isSingle = false;
      var found = false;
      string singleLdsStateCode = null;
      var streetsTable = ZipStreetsDownloaded.GetLookupDataByZipCode(zipCode, 0);
      if (streetsTable.Count > 0)
      {
        found = true;

        // Fetch and summarize the USZD data
        var ldsDictionary = new Dictionary<LdsInfo, object>();
// ReSharper disable InconsistentNaming
        var zip4s = CreateZipPlus4List(streetsTable);
// ReSharper restore InconsistentNaming
        var uszdTable = Uszd.GetDataByZip4List(zipCode, zip4s, 0);
        foreach (var row in uszdTable)
        {
          var stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
          singleLdsStateCode = row.LdsStateCode;
          var ldsInfo = new LdsInfo(stateCode, row.Congress, row.StateSenate,
            row.StateHouse, row.County);
          ldsDictionary[ldsInfo] = null;
        }
        if (ldsDictionary.Count == 1)
        {
          isSingle = true;
          var singleLdsInfo = ldsDictionary.Keys.Single();
          //List<string> fields = new List<string>();
          var csvWriter = new SimpleCsvWriter();
          //AddField(fields, zipCode);
          //AddField(fields, singleLdsInfo.Congress.Substring(1));
          //AddField(fields, singleLdsInfo.StateSenate);
          //AddField(fields, singleLdsInfo.StateHouse);
          //AddField(fields, singleLdsStateCode);
          //AddField(fields, singleLdsInfo.County);
          //AddField(fields, singleLdsInfo.StateCode);
          //TextWriter.WriteLine(string.Join(",", fields));
          csvWriter.AddField(zipCode);
          csvWriter.AddField(singleLdsInfo.Congress.Substring(1));
          csvWriter.AddField(singleLdsInfo.StateSenate);
          csvWriter.AddField(singleLdsInfo.StateHouse);
          csvWriter.AddField(singleLdsStateCode);
          csvWriter.AddField(singleLdsInfo.County);
          csvWriter.AddField(singleLdsInfo.StateCode);
          csvWriter.Write(_TextWriter);
        }
      }

      if (found)
        Report("{0}: {1}", zipCode, isSingle ? "Single" : "Multiple");
    }

    public Action<string> Feedback { set { _FeedbackDelegate = value; } }

    private void Report(string text, params object[] arguments)
    {
      if (_FeedbackDelegate != null)
        _FeedbackDelegate(string.Format(text, arguments));
    }
  }
}