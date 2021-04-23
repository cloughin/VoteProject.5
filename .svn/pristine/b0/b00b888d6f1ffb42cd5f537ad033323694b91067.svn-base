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
using DoubleMetaphone;
using UtilityLibrary;

namespace TestRedirect
{
  public partial class AnalyzeStreets : Form
  {
    bool Running = false;
    bool StopPending = false;
    bool SuppressUpdate = false;

    int RecordCount;
    int NonUS;
    int NonDelivery;
    int RowsWritten = 0;

    public AnalyzeStreets()
    {
      InitializeComponent();

      SetConnectionString();
    }

    private int AnalyzeStreet(List<StreetAnalysisData> dataList)
    {
      StreetAnalyzer streetAnalyzer = new StreetAnalyzer(SuppressUpdate);
      streetAnalyzer.Feedback = AppendErrorsText;
      return streetAnalyzer.Analyze(dataList);
    }

    private void AppendErrorsText(string text)
    {
      if (ErrorsTextBox.Text.Length != 0)
        this.Invoke(() => ErrorsTextBox.AppendText(Environment.NewLine));
      this.Invoke(() => ErrorsTextBox.AppendText(text));
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

    private void DoOneZipCode(string zipCode)
    {
      if (!SuppressUpdate)
        DB.VoteZipNew.ZipStreets.DeleteByZipCode(zipCode, 0);

      using (var reader = DB.VoteZipNew.ZipStreetsDownloaded.GetAnalysisDataReaderByZipCode(zipCode, 0))
      {
        bool first = true;

        string currentDirectionPrefix = null;
        string currentStreetName = null;
        string currentStreetSuffix = null;
        string currentDirectionSuffix = null;

        List<StreetAnalysisData> dataList = null;

        while (reader.Read())
        {
          RecordCount++;

          if (!StateCache.IsValidStateCode(reader.State))
          {
            NonUS++;
            continue; // skip PR etc
          }

          if (reader.Plus4Low.EndsWith("ND")) // no delivery
          {
            NonDelivery++;
            continue;
          }

          if (first)
          {
            AppendStatusText("{1} Beginning zipCode {0}", zipCode,
              RowsWritten);
            first = false;
          }
          string directionPrefix = reader.DirectionPrefix;
          string streetName = reader.StreetName;
          string streetSuffix = reader.StreetSuffix;
          string directionSuffix = reader.DirectionSuffix;
          if (directionPrefix != currentDirectionPrefix || streetName != currentStreetName ||
            streetSuffix != currentStreetSuffix || directionSuffix != currentDirectionSuffix)
          {
            if (dataList != null)
              RowsWritten += AnalyzeStreet(dataList);
            dataList = new List<StreetAnalysisData>();
            currentDirectionPrefix = directionPrefix;
            currentStreetName = streetName;
            currentStreetSuffix = streetSuffix;
            currentDirectionSuffix = directionSuffix;
          }
          dataList.Add(new StreetAnalysisData(reader));
        }
        if (dataList != null)
          RowsWritten += AnalyzeStreet(dataList);
      }
    }

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

      RecordCount = 0;
      NonUS = 0;
      NonDelivery = 0;
      RowsWritten = 0;

      for (int zip = startAt; zip <= 99999; zip++)
        if (StopPending)
        {
          this.Invoke(() => StartAtTextBox.Text = zip.ToString().ZeroPad(5));
          break;
        }
        else
          DoOneZipCode(zip.ToString().ZeroPad(5));
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
        if (!StopPending)
          AppendStatusText("Completed: {0} rows", RowsWritten);
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
      if (!StopPending)
        StartAtTextBox.Text = string.Empty;

      //StartButton.Enabled = true;
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
        AppendStatusText(ex.Message);
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
        //StartButton.Enabled = false;
        StartButton.Text = "Stop";
        ServerGroupBox.Enabled = false;
        StartAtLabel.Enabled = false;
        StartAtTextBox.Enabled = false;
        SuppressUpdateCheckBox.Enabled = false;
        Running = true;
        StopPending = false;
        SuppressUpdate = SuppressUpdateCheckBox.Checked;

        StatusTextBox.Clear();
        ErrorsTextBox.Clear();
        BackgroundWorker.RunWorkerAsync();
      }
    }

    #endregion Event handlers
  }
}
