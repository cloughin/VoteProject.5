using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DB.Vote;
using UtilityLibrary;
using Vote;
using static System.String;
using ExcelDataReader;

namespace LoadDonations
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void AppendStatusText(string text)
    {
      var form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (IsNullOrWhiteSpace(text)) return;
      text = $"{DateTime.Now:HH:mm:ss.fff} {text}";
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

    private void Process()
    {
      var rowNumber = 1;
      var donationsAdded = 0;
      var addressesAdded = 0;
      var addressesModified = 0;
      var now = DateTime.UtcNow;

      void PostError(string msg)
      {
        AppendStatusText($"Row {rowNumber}: {msg}");
      }

      var fileToRead = new FileInfo(SpreadsheetTextBox.Text);
      using (var stream = fileToRead.OpenRead())
      {
        var reader = ExcelReaderFactory.CreateReader(stream);
        using (reader)
        {
          var config = new ExcelDataSetConfiguration
          {
            ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
            {
              UseHeaderRow = true
            }
          };
          var result = reader.AsDataSet(config);
          if (result.Tables.Count != 1)
            throw new Exception($"Cannot handle an Excel File with {result.Tables.Count} sheets");
          var dataTable = result.Tables[0];
          foreach (var row in dataTable.Rows.OfType<DataRow>())
          {
            rowNumber++;
            var date = row.ExcelDate();
            var fullName = row.ExcelFullName().SafeString().Trim();
            var address = row.ExcelAddress().SafeString().Trim();
            var cityStateZip = row.ExcelCityStateZip().SafeString().Trim();
            var email = row.ExcelEmail().Trim().SafeString();
            var donation = row.ExcelDonation();

            if (date == null || date > DateTime.Now || date < DateTime.Now.Subtract(new TimeSpan(730, 0, 0, 0) /* 2 years */))
            {
              PostError("Missing or invalid Date ({date}");
              continue;
            }

            if (IsNullOrWhiteSpace(fullName))
            {
              PostError("Missing Full Name");
              continue;
            }

            if (IsNullOrWhiteSpace(address))
            {
              PostError("Missing Address");
              continue;
            }

            if (IsNullOrWhiteSpace(cityStateZip))
            {
              PostError("Missing City State Zip");
              continue;
            }

            if (!Validation.IsValidEmailAddress(email))
            {
              PostError("Missing or invalid Email ({email}");
              continue;
            }

            if (donation == null || donation <= 0)
            {
              PostError("Missing or invalid Donation");
              continue;
            }

            // decompose name
            var pos = fullName.LastIndexOf(' ');
            if (pos < 1)
            {
              PostError($"Cannot decompose name: {fullName}");
              continue;
            }

            var firstName = fullName.Substring(0, pos).Trim();
            var lastName = fullName.Substring(pos + 1).Trim();
            if (IsNullOrWhiteSpace(firstName) || IsNullOrWhiteSpace(lastName))
            {
              PostError($"Cannot decompose name: {fullName}");
              continue;
            }

            // decompose cityStateZip
            var match = Regex.Match(cityStateZip, @"^\s*(?<city>[^,]+),\s*(?<state>[^\d]+)(?<zip5>\d{5})?(?:-(?<zip4>\d{4}))?\s*$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
              PostError($"Cannot decompose cityStateZip: {cityStateZip}");
              continue;
            }

            var city = match.Groups["city"].Value.Trim();
            var state = match.Groups["state"].Value.Trim();
            var zip5 = match.Groups["zip5"].Value.Trim();
            var zip4 = match.Groups["zip4"].Value.Trim();

            var stateCode = StateCache.GetStateCode(state);
            if (IsNullOrWhiteSpace(stateCode))
            {
              PostError($"Invalid State: {state}");
              continue;
            }

            //if (Donations.EmailDateExists(email, date.Value))
            //{
            //  PostError("A donation for this email, date and time already exists" +
            //    $" ({email}, {date.Value:G}, {donation:C})");
            //  continue;
            //}

            if (UpdateDatabaseCheckBox.Checked)
              Donations.Insert(email, date.Value, firstName, lastName, fullName,
                address, city, stateCode, zip5, zip4, Empty, Convert.ToDecimal(donation.Value), false, null);
            donationsAdded++;

            // Get the encoding
            var googleResult = GoogleMapsLookup.Lookup(address, city, stateCode, zip5);
            TigerLookupData tigerResult = null;
            if (googleResult.Status == "OK")
              tigerResult =
                TigerLookup.Lookup(googleResult.Latitude, googleResult.Longitude);
            var table = Addresses.GetDataByEmail(email);
            if (table.Count == 0) // Insert
            {
              if (UpdateDatabaseCheckBox.Checked)
                Addresses.Insert(firstName, lastName, address, city, stateCode, zip5, zip4,
                  email, Empty, now, "DONR", false, false, false, VotePage.DefaultDbDate,
                  Empty, tigerResult?.Congress.SafeString(),
                  tigerResult?.Upper.SafeString(), tigerResult?.Lower.SafeString(),
                  tigerResult?.County.SafeString(), tigerResult?.District.SafeString(),
                  tigerResult?.Place.SafeString(), tigerResult?.Elementary.SafeString(),
                  tigerResult?.Secondary.SafeString(), tigerResult?.Unified.SafeString(),
                  tigerResult?.CityCouncil.SafeString(),
                  tigerResult?.CountySupervisors.SafeString(),
                  tigerResult?.SchoolDistrictDistrict.SafeString(), googleResult.Latitude,
                  googleResult.Longitude, googleResult.Status == "OK" ? now : VotePage.DefaultDbDate,
                  0, VotePage.DefaultDbDate, true);
              addressesAdded++;
            }
            else if (googleResult.Status == "OK") // update all matching Addresses
            {
              foreach (var r in table)
              {
                if (!IsNullOrWhiteSpace(firstName)) r.FirstName = firstName;
                if (!IsNullOrWhiteSpace(lastName)) r.LastName = lastName;
                r.IsDonor = true;
                if (tigerResult != null)
                {
                  r.Address = address;
                  r.City = city;
                  r.StateCode = stateCode;
                  r.Zip5 = zip5;
                  r.Zip4 = zip4;
                  r.CongressionalDistrict = tigerResult.Congress;
                  r.StateSenateDistrict = tigerResult.Upper;
                  r.StateHouseDistrict = tigerResult.Lower;
                  r.County = tigerResult.County;
                  r.District = tigerResult.District;
                  r.Place = tigerResult.Place;
                  r.Latitude = googleResult.Latitude;
                  r.Longitude = googleResult.Longitude;
                  r.DistrictLookupDate = now;
                }

                addressesModified++;
              }
              if (UpdateDatabaseCheckBox.Checked)
                Addresses.UpdateTable(table);
            }
          }
          AppendStatusText($"{rowNumber - 1} rows processed, {donationsAdded} donations added, {addressesAdded} addresses added, {addressesModified} addresses modified");
        }
      }
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        Process();
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch (Exception ex)
      {
        AppendStatusText($"An exception occurred: {ex.Message}");
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender,
      RunWorkerCompletedEventArgs e)
    {
      ServerGroupBox.Enabled = true;
      SpreadsheetTextBox.Enabled = true;
      OptionsFileBrowseButton.Enabled = true;
      StartButton.Enabled = true;
      UpdateDatabaseCheckBox.Enabled = true;
    }

    private void SpreadsheetBrowseButton_Click(object sender, EventArgs e)
    {
      OpenOptionsFileDialog.FileName = SpreadsheetTextBox.Text;
      if (OpenOptionsFileDialog.ShowDialog() == DialogResult.OK)
        SpreadsheetTextBox.Text = OpenOptionsFileDialog.FileName;
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
      SpreadsheetTextBox.Enabled = false;
      OptionsFileBrowseButton.Enabled = false;
      StartButton.Enabled = false;
      UpdateDatabaseCheckBox.Enabled = false;
      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion
  }

  #region Database access

  static class DatabaseExtensions
  {
    public static string ExcelAddress(this DataRow row)
    {
      return row["Address"] as string;
    }

    public static string ExcelCityStateZip(this DataRow row)
    {
      return row["City State Zip"] as string;
    }

    public static DateTime? ExcelDate(this DataRow row)
    {
      return row["Date"] as DateTime?;
    }

    public static double? ExcelDonation(this DataRow row)
    {
      if (row["Donation"] is double)
        return Math.Round((double)row["Donation"], 2);
      return null;
    }

    public static string ExcelEmail(this DataRow row)
    {
      return row["Email"] as string;
    }

    public static string ExcelFullName(this DataRow row)
    {
      return row["Full Name"] as string;
    }
  }
  #endregion
}
