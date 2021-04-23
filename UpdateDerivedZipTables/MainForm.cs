using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using UtilityLibrary;


namespace UpdateDerivedZipTables
{
  public partial class MainForm : Form
  {
    Dictionary<string, object> ZipCodesToUpdate;
    bool SuppressUpdate;
    int StreetCount;
    int SingleZipCodesAdded;
    int SingleZipCodesRemoved;

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

    private void DoWork()
    {
      // We read the ZipStreetsUpdatesNeeded table, which contains ZipCode/Street
      // combinations that have been updated in the source tables.
      //
      // For each row in the table we delete then recreate the ZipStreets entries.
      // As we go, we collect all the changed ZipCodes. When we are through, we
      // delete then recreate the ZipSingleUSZD entries for all modified zip codes.

      AppendStatusText("Starting...");
      ZipCodesToUpdate = new Dictionary<string, object>();

      int skip = 0;
      string skipText = SkipTextBox.Text.Trim();
      if (!string.IsNullOrWhiteSpace(skipText) && !int.TryParse(skipText, out skip))
      {
        AppendStatusText("Invalid Skip value");
        return;
      }

      // We use a reader because who knows how big this might be...
      // Build in error recovery, 'cause stuff happens...
      int count = 0;
      bool done = false;
      while (!done)
        using (var reader = DB.VoteZipNew.ZipStreetsUpdatesNeeded.GetAllDataReaderAt(count, 0))
        {
          try
          {
            while (reader.Read())
            {
              // Save the ZipCode (only new entries are actually saved)
              ZipCodesToUpdate[reader.ZipCode] = null;

              if (StreetCount >= skip)
              {
                ProcessOneStreet(reader);
                StreetCount++;
                count++;
                if (StreetCount % 100 == 0)
                  AppendStatusText("Processed {0} streets", StreetCount);
              }
            }
            done = true;
          }
          catch { }
        }

      // Report final street results
      AppendStatusText("Finished {0} streets", StreetCount);

      // Now do zip codes
      ProcessZipCodes();

      // Truncate the table (if updating)
      if (!SuppressUpdate)
        DB.VoteZipNew.ZipStreetsUpdatesNeeded.TruncateTable();
    }

    private void DeletePastedVariants(string zipCode, string directionPrefix, 
      string streetName, string streetSuffix, string directionSuffix)
    {
      // We only paste adjacent pairs for now.
      if (streetName == StreetAnalyzer.PoBox) return;
      string[] splitName = streetName.Split(' ');
      if (splitName.Length > 1)
        for (int pasteIndex = 1; pasteIndex < splitName.Length; pasteIndex++)
        {
          var pastedParts = new List<string>();
          string pastedPart = splitName[pasteIndex - 1] + splitName[pasteIndex];
          pastedParts.AddRange(splitName.Take(pasteIndex - 1));
          pastedParts.Add(pastedPart);
          pastedParts.AddRange(splitName.Skip(pasteIndex + 1));
          string pastedStreetName = string.Join(" ", pastedParts);
          DB.VoteZipNew.ZipStreets.DeleteByFullStreetName(zipCode, directionPrefix, pastedStreetName,
            streetSuffix, directionSuffix, 0);
        }
    }

    private void ProcessOneStreet(DB.VoteZipNew.ZipStreetsUpdatesNeededReader updatesNeededReader)
    {
      string zipCode = updatesNeededReader.ZipCode;
      string directionPrefix = updatesNeededReader.DirectionPrefix;
      string streetName = updatesNeededReader.StreetName;
      string streetSuffix = updatesNeededReader.StreetSuffix;
      string directionSuffix = updatesNeededReader.DirectionSuffix;

      if (!SuppressUpdate)
      {
        DB.VoteZipNew.ZipStreets.DeleteByFullStreetName(zipCode, directionPrefix, streetName,
          streetSuffix, directionSuffix, 0);
        // Because we now create entries with adjacent street name parts pasted,
        // we must delete these entries also.
        DeletePastedVariants(zipCode, directionPrefix, streetName,
          streetSuffix, directionSuffix);
      }

      // Collect data for the street in a list
      using (var streetsReader =
        DB.VoteZipNew.ZipStreetsDownloaded.GetAnalysisDataReaderByFullStreetName(zipCode,
         directionPrefix, streetName, streetSuffix, directionSuffix, 0))
      {
        var dataList = new List<StreetAnalysisData>();

        while (streetsReader.Read())
        {
          if (!StateCache.IsValidStateCode(streetsReader.State))
            continue; // skip PR etc

          if (streetsReader.Plus4Low.EndsWith("ND")) // no delivery
            continue;

          dataList.Add(new StreetAnalysisData(streetsReader));
        }

        if (dataList.Count > 0)
        {
          StreetAnalyzer streetAnalyzer = new StreetAnalyzer(SuppressUpdate);
          streetAnalyzer.Feedback = AppendStatusText;
          streetAnalyzer.Analyze(dataList);
        }
      }
    }

    private void ProcessZipCodes()
    {
      AppendStatusText("Processing {0} zip codes", ZipCodesToUpdate.Count);
      foreach (string zipCode in ZipCodesToUpdate.Keys)
      {
        bool alreadyExists;

        if (SuppressUpdate)
          alreadyExists = DB.VoteZipNew.ZipSingleUszd.ZipCodeExists(zipCode);
        else
          alreadyExists = DB.VoteZipNew.ZipSingleUszd.DeleteByZipCode(zipCode) > 0;

        ZipSingleUszdWriter writer = new ZipSingleUszdWriter(SuppressUpdate);
        writer.Feedback = AppendStatusText;
        bool single = writer.DoOneZipCode(zipCode);

        if (alreadyExists && !single)
          SingleZipCodesRemoved++;
        else if (!alreadyExists && single)
          SingleZipCodesAdded++;
      }
      AppendStatusText("Finished {0} zip codes", ZipCodesToUpdate.Count);
      AppendStatusText("{0} added, {1} removed", SingleZipCodesAdded, SingleZipCodesRemoved);
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
        DoWork();
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
      ServerGroupBox.Enabled = true;
      SuppressUpdateCheckBox.Enabled = true;
      SkipTextBox.Enabled = true;
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
      ServerGroupBox.Enabled = false;
      SuppressUpdate = SuppressUpdateCheckBox.Checked;
      SuppressUpdateCheckBox.Enabled = false;
      SkipTextBox.Enabled = false;
      StreetCount = 0;
      SingleZipCodesAdded = 0;
      SingleZipCodesRemoved = 0;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
