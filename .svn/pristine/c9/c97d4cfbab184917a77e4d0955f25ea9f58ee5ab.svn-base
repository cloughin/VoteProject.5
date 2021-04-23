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

namespace TestRedirect
{
  public partial class BuildZipSingleUSZD : Form
  {
    bool SuppressUpdate; 

    public BuildZipSingleUSZD()
    {
      InitializeComponent();

      SetConnectionString();
    }

    private void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    //private static IEnumerable<string> CreateZipPlus4List(ZipStreetsDownloadedTable table)
    //{
    //  Dictionary<string, object> zip4Dictionary = new Dictionary<string, object>();
    //  foreach (var row in table)
    //  {
    //    if (row.Plus4Low == row.Plus4High)
    //      zip4Dictionary[row.Plus4Low] = null;
    //    else
    //    {
    //      int low = int.Parse(row.Plus4Low);
    //      int high = int.Parse(row.Plus4High);
    //      // high = Math.Min(high, low + 199);
    //      for (int n = low; n <= high; n++)
    //        zip4Dictionary[n.ToString().ZeroPad(4)] = null;
    //    }
    //  }
    //  return zip4Dictionary.Keys;
    //}

    private bool DoOneZipCode(string zipCode)
    {
      ZipSingleUszdWriter writer = new ZipSingleUszdWriter(SuppressUpdate);
      writer.Feedback = AppendStatusText;
      return writer.DoOneZipCode(zipCode);
    }

    //private bool DoOneZipCode(string zipCode)
    //{
    //  // Get all zip rows from ZipStreetsDownloaded
    //  bool isSingle = false;
    //  bool found = false;
    //  LdsInfo singleLdsInfo = null;
    //  string singleLdsStateCode = null;
    //  var streetsTable = ZipStreetsDownloaded.GetLookupDataByZipCode(zipCode, 0);
    //  if (streetsTable.Count > 0)
    //  {
    //    found = true;

    //    // Fetch and summarize the USZD data
    //    Dictionary<LdsInfo, object> LdsDictionary = 
    //      new Dictionary<LdsInfo, object>();
    //    IEnumerable<string> zip4s = CreateZipPlus4List(streetsTable);
    //    var uszdTable = Uszd.GetDataByZip4s(zipCode, zip4s, 0);
    //    foreach (var row in uszdTable)
    //    {
    //      string stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
    //      singleLdsStateCode = row.LdsStateCode;
    //      LdsInfo ldsInfo =
    //        new LdsInfo(stateCode, row.Congress, row.StateSenate,
    //          row.StateHouse, row.County);
    //      LdsDictionary[ldsInfo] = null;
    //    }
    //    if (LdsDictionary.Count == 1)
    //    {
    //      isSingle = true;
    //      singleLdsInfo = LdsDictionary.Keys.Single();
    //      if (!SuppressUpdateCheckBox.Checked)
    //        ZipSingleUszd.Insert(
    //          zipCode, 
    //          singleLdsInfo.Congress.Substring(1), // LdsInfo padds it to 3
    //          singleLdsInfo.StateSenate, 
    //          singleLdsInfo.StateHouse,
    //          singleLdsStateCode, 
    //          singleLdsInfo.County, 
    //          singleLdsInfo.StateCode);
    //    }
    //  }

    //  if (found)
    //    AppendStatusText("{0}: {1}", zipCode, isSingle ? "Single" : "Multiple");

    //  return isSingle;
    //}

    private void DoWork()
    {
      int startAt = 0;
      if (!string.IsNullOrWhiteSpace(StartAtTextBox.Text))
        if (!int.TryParse(StartAtTextBox.Text.Trim(), out startAt) ||
          startAt < 0 || startAt > 99999)
        {
          AppendStatusText("Invalid starting number");
          return;
        }

      if (!SuppressUpdateCheckBox.Checked && startAt == 0)
        if (MessageBox.Show("Is it OK to truncate the existing ZipSingleUSZD table?",
        "Confirm Truncation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2) == DialogResult.OK)
        {
          DB.VoteZipNew.ZipSingleUszd.TruncateTable(0);
          AppendStatusText("ZipSingleUSZD truncated");
        }
        else
        {
          AppendStatusText("Cancelled by user");
          return;
        }

      int singleCount = 0;
      // We go through every possible zipcode, all 100000
      for (int zip = startAt; zip <= 99999; zip++)
        if (DoOneZipCode(zip.ToString().ZeroPad(5)))
          singleCount++;
      AppendStatusText("Total singles: {0}", singleCount);
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

      if (TestServerRadioButton.Checked)
        appSettingskey = "TestTempDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveTempDb";
      else
        throw new VoteException("Invalid server");

      connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      DB.VoteTemp.VoteTempDb.ConnectionString = connectionString;
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
      StartAtLabel.Enabled = true;
      StartAtTextBox.Enabled = true;
      SuppressUpdateCheckBox.Enabled = true;
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
      StartAtLabel.Enabled = false;
      StartAtTextBox.Enabled = false;
      SuppressUpdate = SuppressUpdateCheckBox.Checked;
      SuppressUpdateCheckBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
