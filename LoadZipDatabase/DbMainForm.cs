using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Vote;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;

namespace LoadZipDatabase
{
  public partial class DbMainForm : Form
  {
    Common Common;

    public DbMainForm()
    {
      InitializeComponent();
      Common = new Common(this.StatusTextBox);

      StateComboBox.Items.AddRange(StateCache.All51StateCodes.ToArray());
      CountRowsComboBox.Items.AddRange(StateCache.All51StateCodes.ToArray());
      SetConnectionString();
    }

    private void ProcessAllStates()
    {
      Common.AppendStatusText("Starting...");

      DirectoryInfo directoryInfo = new DirectoryInfo(ZipPlus4PathTextBox.Text);
      if (!directoryInfo.Exists)
        throw new VoteException("Directory \"{0}\" does not exist.", directoryInfo.FullName);

      string startWithState = null;
      int skipRows = 0;
      if (StartWithCheckBox.Checked)
      {
        object selectedObject = this.Invoke(() => StateComboBox.SelectedItem);
        if (selectedObject == null)
          throw new VoteException("No starting state selected.");
        startWithState = selectedObject.ToString();
        if (DeleteStartingStateCheckBox.Checked)
        {
          if (MessageBox.Show(string.Format("OK to delete existing entries for {0}?", startWithState),
            "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) != DialogResult.OK)
            throw new VoteException("Cancelled by user.");
          Common.AppendStatusText("Deleting existing entries for {0}", startWithState);
          int entriesDeleted = DB.VoteZipNew.ZipStreetsDownloaded.DeleteByState(startWithState, 0);
          Common.AppendStatusText("{0} entries deleted", entriesDeleted);
        }
        else
        {
          string skipRowsText = SkipRowsTextBox.Text.Trim();
          if (!string.IsNullOrWhiteSpace(skipRowsText))
            if (!int.TryParse(skipRowsText, out skipRows))
              throw new VoteException("Invalid skip rows count");
        }
      }
      else
      {
        if (MessageBox.Show("OK to truncate ZipStreetsDownloaded table?", "Confirm Truncation",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
          DialogResult.OK)
          throw new VoteException("Cancelled by user.");
        DB.VoteZipNew.ZipStreetsDownloaded.TruncateTable();
      }

      bool skipping = startWithState != null;
      foreach (string stateCode in StateCache.All51StateCodes)
        if (!skipping || stateCode == startWithState)
        {
          Common.ProcessOneState(directoryInfo, stateCode, skipRows);
          skipping = false;
          skipRows = 0;
        }

      Common.AppendStatusText("Completed.");
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

    private void SynchronizeStartStateControls()
    {
      StateComboBox.Enabled = StartWithCheckBox.Checked;
      DeleteStartingStateCheckBox.Enabled = StartWithCheckBox.Checked;
      bool skipRowsEnabled = StartWithCheckBox.Checked && !DeleteStartingStateCheckBox.Checked;
      SkipRowsLabel.Enabled = skipRowsEnabled;
      SkipRowsTextBox.Enabled = skipRowsEnabled;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ProcessAllStates();
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
      BrowseZipPlus4PathButton.Enabled = true;
      ZipPlus4PathTextBox.Enabled = true;
      StartWithCheckBox.Enabled = true;
      ServerGroupBox.Enabled = true;
      CountRowsButton.Enabled = true;
      CountRowsComboBox.Enabled = true;
      SynchronizeStartStateControls();
    }

    private void BrowseZipPlus4PathButton_Click(object sender, EventArgs e)
    {
      ZipPlus4FolderBrowserDialog.SelectedPath = ZipPlus4PathTextBox.Text;
      if (ZipPlus4FolderBrowserDialog.ShowDialog() == DialogResult.OK)
        ZipPlus4PathTextBox.Text = ZipPlus4FolderBrowserDialog.SelectedPath;
    }

    private void CountRowsButton_Click(object sender, EventArgs e)
    {
      string state = CountRowsComboBox.SelectedItem.ToString();
      if (!StateCache.IsValidStateCode(state))
      {
        Common.AppendStatusText("Invalid state code");
        return;
      }
      Common.AppendStatusText("Counting rows for {0}", state);
      int count = DB.VoteZipNew.ZipStreetsDownloaded.CountByState(state, 0);
      Common.AppendStatusText("Found {0} rows for {1}", count, state);
    }

    private void DeleteStartingStateCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SynchronizeStartStateControls();
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
      BrowseZipPlus4PathButton.Enabled = false;
      ZipPlus4PathTextBox.Enabled = false;
      StartWithCheckBox.Enabled = false;
      StateComboBox.Enabled = false;
      DeleteStartingStateCheckBox.Enabled = false;
      SkipRowsLabel.Enabled = false;
      SkipRowsTextBox.Enabled = false;
      ServerGroupBox.Enabled = false;
      CountRowsButton.Enabled = false;
      CountRowsComboBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    private void StartWithCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SynchronizeStartStateControls();
    }

    #endregion Event handlers
  }
}
