using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using UtilityLibrary;

namespace TestRedirect
{
  public partial class POBoxes : Form
  {
    Dictionary<string, object> Patterns;
    int RowsProcessed;

    Regex PatternRegex = new Regex(@"(?<part>[0-9]+/[0-9]+)|(?<part>[A-Z]+)|(?<part>[0-9]+)", RegexOptions.IgnoreCase);

    public POBoxes()
    {
      InitializeComponent();
      SetConnectionString();
      Test();
    }

    //private void Test()
    //{
    //  var parts = new List<string>();

    //  parts.Add("PObox", "1001", "5bb", "cc");

    //  foreach (var info in AddressFinder2.PossibleHouseNumbers2(parts))
    //  {
    //  }
    //}

    private void Test()
    {
      //AddressFinder.Test("123sw Folkstone");
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

    private void DoWork()
    {
      Patterns = new Dictionary<string, object>();
      RowsProcessed = 0;

      using (var reader = GetDataReader(0))
        while (reader.Read())
        {
          RowsProcessed++;

          string pattern = PatternRegex.Replace(reader.PrimaryHighNumber,
            SubstitutePatternCharacter);
          Patterns[pattern] = null;

          if (RowsProcessed % 1000 == 0)
            Report();
        }

      Report();
      AppendStatusText("Complete");
    }

    private string SubstitutePatternCharacter(Match match)
    {
      if (match.Value.IsDigits())
        return "9";
      else if (match.Value.IsLetters())
        return "X";
      else
        return "F";
    }

    public static DB.VoteZipNew.ZipStreetsReader GetDataReader(int commandTimeout)
    {
      string cmdText = DB.VoteZipNew.ZipStreets.SelectAllCommandText;
      cmdText += " WHERE StName='PO BOX' AND Length(AddressPrimaryHighNumber) <> 10";
      DbConnection cn = DB.VoteTemp.VoteTempDb.GetOpenConnection();
      DbCommand cmd = DB.VoteTemp.VoteTempDb.GetCommand(cmdText, cn, commandTimeout);
      return new DB.VoteZipNew.ZipStreetsReader(cmd.ExecuteReader(), cn);
    }

    private void Report()
    {
      this.Invoke(() => StatusTextBox.Clear());
      AppendStatusText("Patterns");

      foreach (string pattern in Patterns.Keys.OrderBy(pattern => pattern))
        AppendStatusText(pattern);

      AppendStatusText("{0} rows processed", RowsProcessed);
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

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
