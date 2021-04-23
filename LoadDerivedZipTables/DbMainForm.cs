using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using UtilityLibrary;

namespace LoadDerivedZipTables
{
  public partial class DbMainForm : Form
  {
    Common Common;

    bool Running = false;
    bool StopPending = false;
    bool SuppressUpdate = false;

    public DbMainForm()
    {
      InitializeComponent();
      Common = new Common(this.StatusTextBox, this.ErrorsTextBox);
      SetConnectionString();
    }

    private void DoWork()
    {
      int startAt = 0;
      if (!string.IsNullOrWhiteSpace(StartAtTextBox.Text))
        if (!int.TryParse(StartAtTextBox.Text.Trim(), out startAt) ||
          startAt < 0 || startAt > 99999)
        {
          Common.AppendStatusText("Invalid starting number");
          return;
        }

      for (int zip = startAt; zip <= 99999; zip++)
        if (StopPending)
        {
          this.Invoke(() => StartAtTextBox.Text = zip.ToString().ZeroPad(5));
          break;
        }
        else
          Common.DoOneZipCode(zip.ToString().ZeroPad(5));
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
      VoteDb.ConnectionString = connectionString;

    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        DoWork();
        if (!StopPending)
          Common.AppendStatusText("Completed: {0} rows", Common.RowsWritten);
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
      if (!StopPending)
        StartAtTextBox.Text = string.Empty;

      StartButton.Text = "Start";
      ServerGroupBox.Enabled = false;
      StartAtLabel.Enabled = true;
      StartAtTextBox.Enabled = true;
      SuppressUpdateCheckBox.Enabled = true;
      Running = false;
      StopPending = false;
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
      if (Running)
      {
        StopPending = true;
      }
      else
      {
        StartButton.Text = "Stop";
        ServerGroupBox.Enabled = false;
        StartAtLabel.Enabled = false;
        StartAtTextBox.Enabled = false;
        SuppressUpdateCheckBox.Enabled = false;
        Running = true;
        StopPending = false;
        SuppressUpdate = SuppressUpdateCheckBox.Checked;
        Common.SuppressUpdate = SuppressUpdate;
        StatusTextBox.Clear();
        ErrorsTextBox.Clear();
        BackgroundWorker.RunWorkerAsync();
      }
    }

    #endregion Event handlers
  }
}
