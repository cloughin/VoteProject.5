using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using DB.Vote;
using UtilityLibrary;
using Vote;
using static System.String;

namespace Geocode_Addresses
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      var form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (IsNullOrWhiteSpace(text)) return;
      text = DateTime.Now.ToString("HH:mm:ss.fff") + " " +
        Format(text, arguments);
      form.Invoke(() => StatusTextBox.AppendText(text));
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
      if (IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    public static AddressesTable GetData(int limit, int commandTimeout = -1)
    {
      var cmdText = "SELECT Id,FirstName,LastName,Address,City,StateCode,Zip5,Zip4,Email,Phone," +
        "DateStamp,SourceCode,OptOut,SendSampleBallots,SentBallotChoices,EmailAttachedDate," +
        "EmailAttachedSource,CongressionalDistrict,StateSenateDistrict,StateHouseDistrict," +
        "County,District,Place,Latitude,Longitude,DistrictLookupDate,CommentCount,LastCommentDate," +
        "IsDonor" + 
        $" FROM Addresses WHERE Address!='' AND Latitude IS Null LIMIT {limit}";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return Addresses.FillTable(cmd, AddressesTable.ColumnSet.All);
    }

    #region Event Handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        AppendStatusText("Started");
        if (!int.TryParse(MaxAttemptsTextBox.Text.Trim(), out var maxAttempts))
        {
          AppendStatusText("Invalid Max Attempts");
          return;
        }
        var table = GetData(maxAttempts);
        var coded = 0;
        var notCoded = 0;
        foreach (var row in table)
        {
          var coding = GoogleMapsLookup.Lookup(row.Address, row.City, row.StateCode,
            row.Zip5);
          if (coding.Status == "OK")
          {
            row.Address = $"{coding.StreetNumber.SafeString()} {coding.Street.SafeString()}".Trim();
            row.City = coding.City.SafeString();
            row.StateCode = coding.State.SafeString();
            row.Zip5 = coding.Zip5.SafeString();
            row.Zip4 = coding.Zip4.SafeString();
            row.Latitude = coding.Latitude;
            row.Longitude = coding.Longitude;
            coded++;
          }
          else
          {
            AppendStatusText($"Could not code {row.Address}, {row.City}, {row.StateCode}, {row.Zip4}:" +
              $" {coding.Status}");
            switch (coding.Status)
            {
              case "ambiguous":
              case "ZERO_RESULTS":
                row.Latitude = 0.0;
                row.Longitude = 0.0;
                break;
            }
            notCoded++;
          }
        }
        Addresses.UpdateTable(table);
        AppendStatusText($"Task complete: coded={coded}, not coded={notCoded}");
        AppendStatusText(Empty);
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender,
      RunWorkerCompletedEventArgs e)
    {
      ServerGroupBox.Enabled = true;
      MaxAttemptsTextBox.Enabled = true;
      StartButton.Enabled = true;
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        if (sender is RadioButton radioButton && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      ServerGroupBox.Enabled = false;
      MaxAttemptsTextBox.Enabled = false;
      StartButton.Enabled = false;
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion
  }
}
