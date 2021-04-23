using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DB.Vote;
using LumenWorks.Framework.IO.Csv;
using Vote;

namespace BallotPediaEncodeCsv
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private string GetIntroPageUrl(string id)
    {
      var uri = UrlManager.GetIntroPageUri(id);
      if (!LiveServerRadioButton.Checked) return uri.ToString();
      return new UriBuilder(uri)
      {
        Host = UrlManager.GetCanonicalLiveHostName(uri.Host)
      }.Uri.ToString();
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
      var candidates = new List<List<KeyValuePair<string, string>>>();

      // read CSV into memory structure
      var exactMatches = 0;
      var verifiedMatches = 0;
      var rejectedMatches = 0;
      var noMatches = 0;
      var multipleMatches = 0;
      using (var csvReader = new CsvReader(File.OpenText(InputTextBox.Text), true))
      {
        var headers = csvReader.GetFieldHeaders();
        if (!headers.Contains("FirstName"))
          throw new VoteException("FirstName column missing");
        if (!headers.Contains("LastName"))
          throw new VoteException("LastName column missing");
        if (!headers.Contains("State"))
          throw new VoteException("State column missing");
        while (csvReader.ReadNextRecord())
        {
          var data = new List<KeyValuePair<string, string>>();
          candidates.Add(data);
          var firstName = string.Empty;
          var lastName = string.Empty;
          var state = string.Empty;
          var id = string.Empty;
          var introPage = string.Empty;
          foreach (var header in headers)
          {
            var value = csvReader[header];
            switch (header)
            {
              case "FirstName":
                firstName = value;
                break;

              case "LastName":
                lastName = value;
                break;

              case "State":
                state = value;
                break;
            }
            data.Add(new KeyValuePair<string, string>(header, value));
          }
          var stateCode = StateCache.GetStateCode(state);
          if (!string.IsNullOrEmpty(stateCode))
          {
            var table = Politicians.GetNamesDataByStateCodeLastName(stateCode,
              lastName);
            var matches = table
              .Where(row => row.FirstName.IsEqIgnoreCase(firstName))
              .ToList();
            if (matches.Count == 1)
            {
              exactMatches++;
              id = matches[0].PoliticianKey;
              introPage = GetIntroPageUrl(id);
            }
            else switch (table.Count)
            {
              case 1:
              {
                var message = string.Format("BallotPedia: {0} {1}\n" + 
                  "Vote-USA: {2}\n\nUse?",
                  firstName, lastName, Politicians.GetFormattedName(table[0].PoliticianKey));
                if (
                  MessageBox.Show(message, "No Exact Match", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                  verifiedMatches++;
                  id = table[0].PoliticianKey;
                  introPage = GetIntroPageUrl(id); 
                }
                else rejectedMatches++;
              }
                break;

              case 0:
                noMatches++;
                break;

              default:
                multipleMatches++;
                break;
            }
          }
          data.Add(new KeyValuePair<string, string>("VoteUSA Id", id));
          data.Add(new KeyValuePair<string, string>("VoteUSA Url", introPage));
        }

        var summary =
          string.Format(
            "Candidates: {0}\nExact matches: {1}\n" +
              "Verified matches: {2}\nRejected: {3}\nNo matches: {4}\nMultiple matches: {5}",
            candidates.Count, exactMatches, verifiedMatches, rejectedMatches,
            noMatches, multipleMatches);
        MessageBox.Show(summary, "Summary");

        if (candidates.Count > 0)
        {
          var directory = Path.GetDirectoryName(InputTextBox.Text);
          var filename = Path.GetFileNameWithoutExtension(InputTextBox.Text);
          var extension = Path.GetExtension(InputTextBox.Text);
          Debug.Assert(directory != null, "directory != null");
          var outputPath = Path.Combine(directory, filename + ".coded" + extension);
          var textWriter = File.CreateText(outputPath);
          var csvWriter = new SimpleCsvWriter();
          // write headings
          foreach (var kvp in candidates[0]) 
            csvWriter.AddField(kvp.Key);
          csvWriter.Write(textWriter);
          foreach (var row in candidates)
          {
            foreach (var kvp in row)
              csvWriter.AddField(kvp.Value);
            csvWriter.Write(textWriter);
          }
          textWriter.Close();
        }
      }
    }

    #endregion Event handlers and overrides
  }
}