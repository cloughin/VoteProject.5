using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Vote;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;
using DB.Vote;
using DB.VoteZipNewLocal;

namespace AddressUtility
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
      DB.Vote.VoteDb.ConnectionString = connectionString;
    }

    #region ConnectDotNet

    const string ConnectDotNetSourceCode = "CNDN";
    const string ConnectDotNetColumnDefinitionVersion = "1.0";

    string ConnectDotNetLoadedColumnDefinitionPath = string.Empty;
    bool ConnectDotNetColumnDefinitionsChanged = false;

    private void ApplyConnectDotNetNamesAndEmails()
    {
      AppendStatusText("Starting...");

      FileInfo fileInfo = new FileInfo(ConnectDotNetInputFileTextBox.Text);
      if (!fileInfo.Exists)
        throw new VoteException("File \"{0}\" does not exist.", fileInfo.FullName);

      bool hasHeaders = ConnectDotNetHasHeadersCheckBox.Checked;
      int firstNameIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetFirstNameNumericUpDown) - 1;
      int lastNameIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetLastNameNumericUpDown) - 1;
      int addressIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetAddressNumericUpDown) - 1;
      int cityIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetCityNumericUpDown) - 1;
      int stateCodeIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetStateCodeNumericUpDown) - 1;
      int zip5Index =
        GetConnectDotNetColumnIndex(ConnectDotNetZip5NumericUpDown) - 1;
      int zip4Index =
        GetConnectDotNetColumnIndex(ConnectDotNetZip4NumericUpDown) - 1;
      int emailIndex =
        GetConnectDotNetColumnIndex(ConnectDotNetEmailNumericUpDown) - 1;

      int rowCount = 0;
      int noUsefulInfo = 0;
      int nameApplied = 0;
      int emailApplied = 0;
      int nameAdded = 0;
      int emailAdded = 0;
      int nameAndEmailApplied = 0;
      int nameAndEmailAdded = 0;
      int invalidData = 0;
      using (var csvReader = new CsvReader(fileInfo.OpenText(), hasHeaders))
      {
        int fieldCount = csvReader.FieldCount;

        try
        {
          while (csvReader.ReadNextRecord())
          {
            rowCount++;

            string firstName = csvReader[firstNameIndex].ToString().Trim();
            string lastName = csvReader[lastNameIndex].ToString().Trim();
            string address = csvReader[addressIndex].ToString().Trim();
            string city = csvReader[cityIndex].ToString().Trim();
            string stateCode = csvReader[stateCodeIndex].ToString().Trim();
            string zip5 = csvReader[zip5Index].ToString().Trim().ZeroPad(5);
            // this column is optional
            string zip4 = string.Empty;
            if (zip4Index >= 0)
              zip4 = csvReader[zip4Index].ToString().Trim().ZeroPad(4);
            else // try to parse zip4 from zip5
            {
              if (zip5.Length == 10 && zip5[5] == '-')
              {
                zip4 = zip5.Substring(6);
                zip5 = zip5.Substring(0, 5);
              }
              else if (zip5.Length == 9)
              {
                zip4 = zip5.Substring(5);
                zip5 = zip5.Substring(0, 5);
              }
            }
            string email = csvReader[emailIndex].ToString().Trim();

            if (zip5 == "00000") zip5 = string.Empty;
            if (zip4 == "0000") zip4 = string.Empty;

            bool haveName = !string.IsNullOrWhiteSpace(firstName) ||
             !string.IsNullOrWhiteSpace(lastName);
            bool haveEmail = !string.IsNullOrWhiteSpace(email);

            if (
              address.Length > Addresses.AddressMaxLength ||
              address.Length == 0 || // Address required
              city.Length > Addresses.CityMaxLength ||
              city.Length == 0 || // City required
              !StateCache.IsValidStateCode(stateCode) ||
              zip5.Length != Addresses.Zip5MaxLength || // Zip5 required, must be 5 chars
              (zip4.Length != Addresses.Zip4MaxLength && zip4.Length != 0) || // Zip4 either 4 chars or empty
              firstName.Length > Addresses.FirstNameMaxLength ||
              lastName.Length > Addresses.LastNameMaxLength ||
              email.Length > Addresses.EmailMaxLength)
              invalidData++;
            else if (haveName || haveEmail)
            {
              // get all db rows with matching address
              // mod: we don't use zip4 in the match because it is not
              // always present or could be incorrect
              //var table = Addresses.GetDataByAddressCityStateCodeZip5Zip4(
              //  address, city, state, zip5, zip4);
              var table = Addresses.GetDataByAddressCityStateCodeZip5(
                address, city, stateCode, zip5);
              bool addRow = false;

              // get rows that match each component
              var nameMatches = table.Rows.Cast<AddressesRow>()
                  .Where(row => row.FirstName.IsEqIgnoreCase(firstName) &&
                    row.LastName.IsEqIgnoreCase(lastName))
                  .ToList();
              var nameBlank = table.Rows.Cast<AddressesRow>()
                  .Where(row => string.IsNullOrWhiteSpace(row.FirstName) &&
                    string.IsNullOrWhiteSpace(row.LastName))
                  .ToList();
              int emailMatchesCount = table.Rows.Cast<AddressesRow>()
                  .Where(row => row.Email.IsEqIgnoreCase(email))
                  .Count();
              var emailBlank = table.Rows.Cast<AddressesRow>()
                  .Where(row => string.IsNullOrWhiteSpace(row.Email))
                  .ToList();

              if (!haveEmail) // name only
              {
                if (nameMatches.Count > 0) // nothing to do
                  noUsefulInfo++;
                else if (nameBlank.Count > 0) // update name
                {
                  nameBlank[0].FirstName = firstName;
                  nameBlank[0].LastName = lastName;
                  nameApplied++;
                }
                else // add new row
                {
                  addRow = true;
                  nameAdded++;
                }
              }
              else if (!haveName) // email only
              {
                if (emailMatchesCount > 0)
                  noUsefulInfo++;
                else if (emailBlank.Count > 0) // apply email
                {
                  foreach (var row in emailBlank)
                  {
                    row.Email = email;
                    row.EmailAttachedSource = ConnectDotNetSourceCode;
                    row.EmailAttachedDate = DateTime.Now;
                  }
                  emailApplied++;
                }
                else // add a new email
                {
                  addRow = true;
                  emailAdded++;
                }
              }
              else // both name and email present
              {
                if (nameMatches.Count > 0 && emailMatchesCount > 0)
                  noUsefulInfo++;
                else if (nameMatches.Count > 0) // no email matches
                {
                  var nameMatchesEmailBlank = nameMatches
                    .Where(row => string.IsNullOrWhiteSpace(row.Email))
                    .ToList();
                  if (nameMatchesEmailBlank.Count > 0)
                  {
                    foreach (var row in nameMatchesEmailBlank)
                    {
                      row.Email = email;
                      row.EmailAttachedSource = ConnectDotNetSourceCode;
                      row.EmailAttachedDate = DateTime.Now;
                    }
                    emailApplied++;
                  }
                  else // new name/email combo
                  {
                    addRow = true;
                    nameAndEmailAdded++;
                  }
                }
                else if (emailMatchesCount > 0) // no name matches
                {
                  var emailMatchesNameBlank = nameBlank
                    .Where(row => row.Email.IsEqIgnoreCase(email))
                    .ToList();
                  if (emailMatchesNameBlank.Count > 0)
                  {
                    foreach (var row in emailMatchesNameBlank)
                    {
                      row.FirstName = firstName;
                      row.LastName = lastName;
                    }
                    nameApplied++;
                  }
                  else
                  {
                    addRow = true;
                    nameAndEmailAdded++;
                  }
                }
                else // no matches at all
                {
                  if (nameBlank.Count == 0 || emailBlank.Count == 0)
                  {
                    addRow = true;
                    nameAndEmailAdded++;
                  }
                  else
                  {
                    foreach (var row in nameBlank)
                    {
                      row.FirstName = firstName;
                      row.LastName = lastName;
                    }
                    foreach (var row in emailBlank)
                    {
                      row.Email = email;
                      row.EmailAttachedSource = ConnectDotNetSourceCode;
                      row.EmailAttachedDate = DateTime.Now;
                    }
                    if (nameBlank.Count == 0)
                      emailApplied++;
                    else if (emailBlank.Count == 0)
                      nameApplied++;
                    else
                      nameAndEmailApplied++;
                  }
                }
              }

              if (addRow)
              {
                table.AddRow(
                  firstName,
                  lastName,
                  address,
                  city,
                  stateCode,
                  zip5,
                  zip4,
                  email,
                  string.Empty,
                  DateTime.Now.Date,
                  ConnectDotNetSourceCode,
                  false,
                  false,
                  DateTime.Now,
                  ConnectDotNetSourceCode,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  VoteDb.DateTimeMin);
              }

              // See if we need to update the table
              bool mustUpdate = table.Rows.Cast<AddressesRow>()
                .Where(row => row.RowState != DataRowState.Unchanged)
                .Count() > 0;
              if (mustUpdate)
                Addresses.UpdateTable(table);
            }
            else
              noUsefulInfo++;
            if (rowCount % 1000 == 0)
              AppendStatusText("{0} rows processed", rowCount);
          }
        }
        catch
        {
          AppendStatusText("Exception at row {}", rowCount);
          throw;
        }
      }

      AppendStatusText("{0} rows processed", rowCount);
      AppendStatusText("{0} rows had no useful information", noUsefulInfo);
      AppendStatusText("{0} rows had invalid data", invalidData);
      AppendStatusText("{0} names applied", nameApplied);
      AppendStatusText("{0} names added", nameAdded);
      AppendStatusText("{0} emails applied", emailApplied);
      AppendStatusText("{0} emails added", emailAdded);
      AppendStatusText("{0} name/emails applied", nameAndEmailApplied);
      AppendStatusText("{0} name/emails added", nameAndEmailAdded);
      AppendStatusText("Completed.");
    }

    private bool ConnectDotNetCheckForUnsavedChanges()
    {
      bool cancelled = false;
      if (ConnectDotNetColumnDefinitionsChanged)
      {
        switch (MessageBox.Show(
          "You have unsaved changes to the column definitions. Do you want to save them?",
          "Save column definitions?",
          MessageBoxButtons.YesNoCancel,
          MessageBoxIcon.Hand))
        {
          case DialogResult.Yes:
            if (ConnectDotNetColumnDefinitionsAreValid())
            {
              if (!SaveConnectDotNetColumnDefinitions(false))
                cancelled = true;
            }
            else
              cancelled = true;
            break;

          case DialogResult.No:
            break;

          case DialogResult.Cancel:
            cancelled = true;
            break;
        }
      }
      return cancelled;
    }

    private bool ConnectDotNetColumnDefinitionsAreValid()
    {
      // The controls enforce the optional/required values.
      // All we need to be sure is that there are no duplicate
      // values.
      List<int> values = GetConnectDotNetColumnIndexes();
      values = values // remove omitted values as they are allowed to be duplicated
        .Where(value => value != 0)
        .ToList();
      var grouped = values
        .GroupBy(value => value)
        .ToList();
      if (values.Count != grouped.Count) // there were dups
      {
        var dups = grouped
          .Where(group => group.Count() > 1)
          .Select(group => group.Key)
          .OrderBy(index => index)
          .ToList();
        MessageBox.Show(string.Format(
          "The following column indexes appear more than once: {0}",
          string.Join(", ", dups)), "Invalid Column Definitions",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    private int GetConnectDotNetColumnIndex(NumericUpDown numericUpDown)
    {
      return Convert.ToInt32(numericUpDown.Value);
    }

    private List<int> GetConnectDotNetColumnIndexes()
    {
      List<int> values = new List<int>();
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetFirstNameNumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetLastNameNumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetAddressNumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetCityNumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetStateCodeNumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetZip5NumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetZip4NumericUpDown));
      values.Add(GetConnectDotNetColumnIndex(ConnectDotNetEmailNumericUpDown));
      return values;
    }

    private string GetConnectDotNetDefaultPath()
    {
      string path = ConnectDotNetLoadedColumnDefinitionPath;
      if (string.IsNullOrWhiteSpace(path))
      {
        string folder =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        path = Path.Combine(folder, "connect.net.columns.xml");
      }
      return path;
    }

    private bool SaveConnectDotNetColumnDefinitions(bool saveAs)
    {
      if (string.IsNullOrWhiteSpace(ConnectDotNetLoadedColumnDefinitionPath))
        saveAs = true;
      string path = GetConnectDotNetDefaultPath();
      if (saveAs)
      {
        SaveConnectDotNetColumnsFileDialog.FileName = path;
        switch (SaveConnectDotNetColumnsFileDialog.ShowDialog())
        {
          case DialogResult.OK:
            path = SaveConnectDotNetColumnsFileDialog.FileName;
            break;

          default:
            return false;
        }
      }
      SaveConnectDotNetColumnDefinitions(path);
      ConnectDotNetLoadedColumnDefinitionPath = path;
      ConnectDotNetColumnDefinitionsChanged = false;
      return true;
    }

    private void SaveConnectDotNetColumnDefinitions(string path)
    {
      XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
      writer.WriteStartDocument();
      writer.WriteStartElement("ConnectDotNetColumns");
      writer.WriteStartElement("Version");
      writer.WriteString(ConnectDotNetColumnDefinitionVersion);
      writer.WriteEndElement();
      writer.WriteStartElement("HasHeaders");
      writer.WriteString(ConnectDotNetHasHeadersCheckBox.Checked.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("Columns");
      writer.WriteString(string.Join(",", GetConnectDotNetColumnIndexes()));
      writer.WriteEndElement();
      writer.Close();
    }

    private void StartConnectDotNet()
    {
      if (ConnectDotNetColumnDefinitionsAreValid())
      {
        StartButton.Enabled = false;
        OperationGroupBox.Enabled = false;
        ServerGroupBox.Enabled = false;
        ConnectDotNetPanel.Enabled = false;

        StatusTextBox.Clear();
        ConnectDotNetBackgroundWorker.RunWorkerAsync();
      }
    }

    //
    // Event handlers
    //

    private void BrowseConnectDotNetInputFileButton_Click(object sender, EventArgs e)
    {
      OpenConnectDotNetInputFileDialog.FileName = ConnectDotNetInputFileTextBox.Text;
      if (OpenConnectDotNetInputFileDialog.ShowDialog() == DialogResult.OK)
        ConnectDotNetInputFileTextBox.Text = OpenConnectDotNetInputFileDialog.FileName;
    }

    private void ConnectDotNetBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ApplyConnectDotNetNamesAndEmails();
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

    private void ConnectDotNetBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      OperationGroupBox.Enabled = true;
      ServerGroupBox.Enabled = true;
      ConnectDotNetPanel.Enabled = true;
    }

    private void ConnectDotNetColumnIndex_ValueChanged(object sender, EventArgs e)
    {
      ConnectDotNetColumnDefinitionsChanged = true;
    }

    private void ConnectDotNetLoadButton_Click(object sender, EventArgs e)
    {
      if (!ConnectDotNetCheckForUnsavedChanges())
      {
        OpenConnectDotNetColumnsFileDialog.FileName = GetConnectDotNetDefaultPath();
        if (OpenConnectDotNetColumnsFileDialog.ShowDialog() == DialogResult.OK)
        {
          XmlReader reader =
            new XmlTextReader(OpenConnectDotNetColumnsFileDialog.FileName);
          reader.ReadToFollowing("Version");
          string version = reader.ReadString();
          reader.ReadToFollowing("HasHeaders");
          ConnectDotNetHasHeadersCheckBox.Checked =
            bool.Parse(reader.ReadString());
          reader.ReadToFollowing("Columns");
          int[] indexes = reader.ReadString().Split(',')
            .Select(column => int.Parse(column))
            .ToArray();
          ConnectDotNetFirstNameNumericUpDown.Value = indexes[0];
          ConnectDotNetLastNameNumericUpDown.Value = indexes[1];
          ConnectDotNetAddressNumericUpDown.Value = indexes[2];
          ConnectDotNetCityNumericUpDown.Value = indexes[3];
          ConnectDotNetStateCodeNumericUpDown.Value = indexes[4];
          ConnectDotNetZip5NumericUpDown.Value = indexes[5];
          ConnectDotNetZip4NumericUpDown.Value = indexes[6];
          ConnectDotNetEmailNumericUpDown.Value = indexes[7];
          ConnectDotNetLoadedColumnDefinitionPath =
            OpenConnectDotNetColumnsFileDialog.FileName;
          ConnectDotNetColumnDefinitionsChanged = false;
        }
      }
    }

    private void ConnectDotNetRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      ConnectDotNetPanel.Visible = ConnectDotNetRadioButton.Checked;
    }

    private void ConnectDotNetSaveButton_Click(object sender, EventArgs e)
    {
      if (ConnectDotNetColumnDefinitionsAreValid())
        SaveConnectDotNetColumnDefinitions(false);
    }

    private void ConnectDotNetSaveAsButton_Click(object sender, EventArgs e)
    {
      if (ConnectDotNetColumnDefinitionsAreValid())
        SaveConnectDotNetColumnDefinitions(true);
    }

    #endregion ConnectDotNet

    #region FillInLegislativeDistricts

    private void StartFillInLegislativeDistricts()
    {
      StartButton.Enabled = false;
      OperationGroupBox.Enabled = false;
      ServerGroupBox.Enabled = false;
      FillInLegislativeDistrictsPanel.Enabled = false;

      StatusTextBox.Clear();
      FillInLegislativeDistrictsBackgroundWorker.RunWorkerAsync();
    }

    private void FillInLegislativeDistricts()
    {
      AppendStatusText("Starting...");

      int rowCount = 0;
      int fromUszd = 0;
      int successfulLookups = 0;
      int unsuccessfulLookups = 0;
      bool done = false;
      while (!done)
        using (var reader = Addresses.GetDataReaderForLegislativeCoding(
          FillInLegislativeDistrictsOnlyEmailsCheckBox.Checked,
          FillInLegislativeDistrictsOnlyUncodedCheckBox.Checked, 0))
        {
          try
          {
            while (reader.Read())
            {
              rowCount++;
              string congressionalDistrict = string.Empty.ZeroPad(
                Addresses.CongressionalDistrictMaxLength);
              string stateSenateDistrict = string.Empty.ZeroPad(
                Addresses.StateSenateDistrictMaxLength);
              string stateHouseDistrict = string.Empty.ZeroPad(
                Addresses.StateHouseDistrictMaxLength);
              string county = string.Empty.ZeroPad(
                Addresses.CountyMaxLength);
              bool lookup = true;
              if (reader.Zip5.Length == 5 && reader.Zip4.Length == 4)
              {
                // use USZD
                var uszdTable = Uszd.GetDataByZip5Zip4(reader.Zip5, reader.Zip4);
                if (uszdTable.Count > 0)
                {
                  var uszdRow = uszdTable[0];
                  congressionalDistrict = uszdRow.Congress;
                  stateSenateDistrict = uszdRow.StateSenate;
                  stateHouseDistrict = uszdRow.StateHouse;
                  county = uszdRow.County;
                  lookup = false;
                  fromUszd++;
                }
              }
              if (lookup)
              {
                var result = AddressFinder.Find(reader.Address + ' ' +
                  reader.City + ' ' + reader.StateCode, null);
                if (result.Success)
                {
                  congressionalDistrict = result.Congress;
                  stateSenateDistrict = result.StateSenate;
                  stateHouseDistrict = result.StateHouse;
                  county = result.County;
                  successfulLookups++;
                }
                else
                  unsuccessfulLookups++;
              }
              // congresionalDistrictFixup
              if (congressionalDistrict.Length == 3 && congressionalDistrict[0] == '0')
                congressionalDistrict = congressionalDistrict.Substring(1);
              var addressesTable = Addresses.GetDataById(reader.Id);
              addressesTable[0].CongressionalDistrict = congressionalDistrict;
              addressesTable[0].StateSenateDistrict = stateSenateDistrict;
              addressesTable[0].StateHouseDistrict = stateHouseDistrict;
              addressesTable[0].County = county;
              addressesTable[0].DistrictLookupDate = DateTime.Now.Date;
              Addresses.UpdateTable(addressesTable);
              if (rowCount % 1000 == 0)
                AppendStatusText("{0} rows processed", rowCount);
            }
            done = true;
          }
          catch { }
        }

      AppendStatusText("{0} rows with email and missing legislative districts found", rowCount);
      AppendStatusText("{0} filled in from USZD", fromUszd);
      AppendStatusText("{0} successful lookups", successfulLookups);
      AppendStatusText("{0} unsuccessful lookups", unsuccessfulLookups);
      AppendStatusText("Completed.");
    }

    //
    // Event handlers
    //

    private void FillInLegislativeDistrictsBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        FillInLegislativeDistricts();
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

    private void FillInLegislativeDistrictsBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      OperationGroupBox.Enabled = true;
      ServerGroupBox.Enabled = true;
      FillInLegislativeDistrictsPanel.Enabled = true;
    }

    private void FillInLegislativeDistrictsRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      FillInLegislativeDistrictsPanel.Visible = FillInLegislativeDistrictsRadioButton.Checked;
    }

    #endregion LegislativeDistricts

    //
    // Event handlers
    //

    private void MainForm_FormClosing(object sender,
      FormClosingEventArgs e)
    {
      e.Cancel = ConnectDotNetCheckForUnsavedChanges();
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
      if (ConnectDotNetRadioButton.Checked)
        StartConnectDotNet();
      else if (FillInLegislativeDistrictsRadioButton.Checked)
        StartFillInLegislativeDistricts();
    }
  }
}
