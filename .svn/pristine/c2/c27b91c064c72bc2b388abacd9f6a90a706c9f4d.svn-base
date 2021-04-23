using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DB.Vote;
using LumenWorks.Framework.IO.Csv;
using Vote;

namespace BallotPediaUpdateLinks
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
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

      var connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    #region Event handlers and overrides

    private void InputBrowseButton_Click(object sender, EventArgs e)
    {
      InputOpenFileDialog.FileName = InputTextBox.Text;
      if (InputOpenFileDialog.ShowDialog() == DialogResult.OK) InputTextBox.Text = InputOpenFileDialog.FileName;
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null && radioButton.Checked)
        SetConnectionString();
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      //var candidates = new List<List<KeyValuePair<string, string>>>();

      var rowsRead = 0;
      var updated = 0;
      var missingIdOrLink = 0;
      var noPolitician = 0;
      using (var csvReader = new CsvReader(File.OpenText(InputTextBox.Text), true))
      {
        var headers = csvReader.GetFieldHeaders();
        if (!headers.Contains("URL"))
          throw new VoteException("BallotPedia URL column missing");
        if (!headers.Contains("VoteUSA Id"))
          throw new VoteException("VoteUSA Id column missing");
        while (csvReader.ReadNextRecord())
        {
          rowsRead++;
          var url = Validation.StripWebProtocol(csvReader["URL"]);
          var id = csvReader["VoteUSA Id"];
          if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(id)) 
            missingIdOrLink++;
          else if (Politicians.UpdateBallotPediaWebAddress(url, id) == 0) 
            noPolitician++;
          else updated++;
        }

        var summary =
        string.Format(
          "Rows read: {0}\nUpdated: {1}\n" +
            "Missing Id or Link in CSV: {2}\nMissing politician in DB: {3}",
          rowsRead, updated, missingIdOrLink, noPolitician);
        MessageBox.Show(summary, "Summary");
      }
    }

    #endregion Event handlers and overrides
  }
}
