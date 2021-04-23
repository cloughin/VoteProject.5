using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DB.VoteZipNew;
using UtilityLibrary;
using Vote;

namespace LoadDerivedZipTables
{
  internal class Common
  {
    private readonly TextBox _ErrorsTextBox;
    private readonly TextBox _StatusTextBox;

    public Common(TextBox statusTextBox, TextBox errorsTextBox)
    {
      _StatusTextBox = statusTextBox;
      _ErrorsTextBox = errorsTextBox;
    }

    private int AnalyzeStreet(List<StreetAnalysisData> dataList)
    {
      var streetAnalyzer = new StreetAnalyzer(TextWriter, SuppressUpdate)
      {
        Feedback = AppendErrorsText
      };
      return streetAnalyzer.Analyze(dataList);
    }

    internal void AppendErrorsText(string text)
    {
      var form = _ErrorsTextBox.Parent as Form;
      if (_ErrorsTextBox.Text.Length != 0) form.Invoke(() => _ErrorsTextBox.AppendText(Environment.NewLine));
      form.Invoke(() => _ErrorsTextBox.AppendText(text));
    }

    internal void AppendStatusText(string text)
    {
      var form = _StatusTextBox.Parent as Form;
      if (_StatusTextBox.Text.Length != 0) form.Invoke(() => _StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text)) text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      form.Invoke(() => { if (text != null) _StatusTextBox.AppendText(text); });
    }

    internal void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    internal void DoOneZipCode(string zipCode)
    {
      if (!SuppressUpdate && TextWriter == null) ZipStreets.DeleteByZipCode(zipCode, 0);

      using (
        var reader = ZipStreetsDownloaded.GetAnalysisDataReaderByZipCode(zipCode, 0)
        )
      {
        var first = true;

        string currentDirectionPrefix = null;
        string currentStreetName = null;
        string currentStreetSuffix = null;
        string currentDirectionSuffix = null;

        List<StreetAnalysisData> dataList = null;

        while (reader.Read())
        {
          if (!StateCache.IsValidStateCode(reader.State)) continue; // skip PR etc

          if (reader.Plus4Low.EndsWith("ND")) // no delivery
            continue;

          if (first)
          {
            AppendStatusText("{1} Beginning zipCode {0}", zipCode, RowsWritten);
            first = false;
          }
          var directionPrefix = reader.DirectionPrefix;
          var streetName = reader.StreetName;
          var streetSuffix = reader.StreetSuffix;
          var directionSuffix = reader.DirectionSuffix;
          if (directionPrefix != currentDirectionPrefix ||
            streetName != currentStreetName || streetSuffix != currentStreetSuffix ||
            directionSuffix != currentDirectionSuffix)
          {
            if (dataList != null) RowsWritten += AnalyzeStreet(dataList);
            dataList = new List<StreetAnalysisData>();
            currentDirectionPrefix = directionPrefix;
            currentStreetName = streetName;
            currentStreetSuffix = streetSuffix;
            currentDirectionSuffix = directionSuffix;
          }
          if (dataList != null) dataList.Add(new StreetAnalysisData(reader));
        }
        if (dataList != null) RowsWritten += AnalyzeStreet(dataList);
      }
    }

    public int RowsWritten { get; private set; }

    public bool SuppressUpdate { get; set; }

    public TextWriter TextWriter { get; set; }
  }
}