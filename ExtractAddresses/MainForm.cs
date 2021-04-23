using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.VoteLog;
using UtilityLibrary;
using System.IO;

namespace ExtractAddresses
{
  public partial class MainForm : Form
  {
    ExtractedAddresses ExtractedAddresses;
    static MainForm _Instance;

    public MainForm()
    {
      _Instance = this;
      InitializeComponent();
      StateComboBox.Items.Add(string.Empty);
      StateComboBox.Items.AddRange(StateCache.All51StateCodes.ToArray());
    }

    internal void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    internal void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private void ExtractMatchingAddresses()
    {
      ExtractedAddresses = new ExtractedAddresses();
      using (var reader = LogAddressesGoodNew.GetDataReaderByParsedStateCodeDateRange(
        StateComboBoxText, StartDateTimePickerValue, EndDateTimePickerValue, 0))
      {
        while (reader.Read())
        {
          ExtractedAddresses.AddAddress(reader);
        }
      }
      ExtractedAddresses.EliminateExactDuplicates();
      if (EditDuplicatesCheckBoxChecked)
        ExtractedAddresses.EliminateFuzzyDuplicates();
      ExtractedAddresses.NoFuzzyDuplicatesCount = 
        ExtractedAddresses.Addresses.Count;

      ExtractedAddresses.WriteToCsv(OutputFileTextBoxText);

      AppendStatusText("{0} addresses read from database",
        ExtractedAddresses.RawCount);
      AppendStatusText("{0} addresses not marked duplicate",
        ExtractedAddresses.NotMarkedDuplicateCount);
      AppendStatusText("{0} addresses meet minimum requirements",
        ExtractedAddresses.MinimumRequirementsCount);
      AppendStatusText("{0} addresses not exact duplicates",
        ExtractedAddresses.NoExactDuplicatesCount);
      AppendStatusText("{0} addresses not fuzzy duplicates",
        ExtractedAddresses.NoFuzzyDuplicatesCount);

      if (ExtractedAddresses.DuplicatedAddressIds.Count != 0)
      {
        if (MessageBox.Show(string.Format(
          "There were {0} new duplicates identified." +
          Environment.NewLine +
          "Do you want to mark these as duplicates in the database?",
          ExtractedAddresses.DuplicatedAddressIds.Count), "Mark Duplicates?",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          foreach (int id in ExtractedAddresses.DuplicatedAddressIds)
            LogAddressesGoodNew.UpdateIsDuplicateById(true, id);
        }
      }
    }

    #region Thread-safe control access

    private bool EditDuplicatesCheckBoxChecked
    {
      get
      {
        if (EditDuplicatesCheckBox.InvokeRequired)
          return this.Invoke(() => EditDuplicatesCheckBox.Checked);
        else
          return EditDuplicatesCheckBox.Checked;
      }
    }

    private DateTime EndDateTimePickerValue
    {
      get
      {
        if (EndDateTimePicker.InvokeRequired)
          return this.Invoke(() => EndDateTimePicker.Value);
        else
          return EndDateTimePicker.Value;
      }
    }

    public static MainForm Instance
    {
      get { return MainForm._Instance; }
    }

    private string OutputFileTextBoxText
    {
      get
      {
        if (OutputFileTextBox.InvokeRequired)
          return this.Invoke(() => OutputFileTextBox.Text);
        else
          return OutputFileTextBox.Text;
      }
    }

    private DateTime StartDateTimePickerValue
    {
      get
      {
        if (StartDateTimePicker.InvokeRequired)
          return this.Invoke(() => StartDateTimePicker.Value);
        else
          return StartDateTimePicker.Value;
      }
    }

    private string StateComboBoxText
    {
      get
      {
        if (StateComboBox.InvokeRequired)
          return this.Invoke(() => StateComboBox.Text);
        else
          return StateComboBox.Text;
      }
    }

    #endregion Thread-safe control access

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ExtractMatchingAddresses();
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
      StateComboBox.Enabled = true;
      StartDateTimePicker.Enabled = true;
      EndDateTimePicker.Enabled = true;
      EditDuplicatesCheckBox.Enabled = true;
      OutputFileTextBox.Enabled = true;
      BrowseOutputFileButton.Enabled = true;
    }

    private void BrowseOutputFileButton_Click(object sender, EventArgs e)
    {
      OutputFileDialog.FileName = OutputFileTextBox.Text;
      if (OutputFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = OutputFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(StateComboBoxText))
      {
        MessageBox.Show("Please select a state");
        return;
      }
      StartButton.Enabled = false;
      StateComboBox.Enabled = false;
      StartDateTimePicker.Enabled = false;
      EndDateTimePicker.Enabled = false;
      EditDuplicatesCheckBox.Enabled = false;
      OutputFileTextBox.Enabled = false;
      BrowseOutputFileButton.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }

  internal class ExtractedAddress
  {
    public string Address;
    public string City;
    public string State;
    public string Zip5;
    public string Zip4;
    public int Id;
    public DateTime DateStamp;

    public ExtractedAddress(LogAddressesGoodNewReader reader)
    {
      Address = reader.ParsedAddress.SafeString().Trim().ToUpperInvariant();
      City = reader.ParsedCity.SafeString().Trim().ToUpperInvariant();
      State = reader.ParsedStateCode.SafeString().Trim();
      Zip5 = reader.ParsedZip5.SafeString().Trim();
      Zip4 = reader.ParsedZip4.SafeString().Trim();
      Id = reader.Id;
      DateStamp = reader.DateStamp;
    }

    public string LeadingNumericsInAddress 
    {
      get
      {
        int len = 0;
        while (len < Address.Length && char.IsDigit(Address[len]))
          len++;
        return Address.Substring(0, len);
      }
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      if (!string.IsNullOrWhiteSpace(Address))
      {
        sb.Append(Address);
        sb.Append(", ");
      }
      sb.Append(City);
      sb.Append(" ");
      sb.Append(State);
      sb.Append(" ");
      sb.Append(Zip5);
      if (!string.IsNullOrWhiteSpace(Zip4))
      {
        sb.Append("-");
        sb.Append(Zip4);
      }
      return sb.ToString();
    }
  }

  internal class ExtractedAddresses
  {
    public int RawCount;
    public int NotMarkedDuplicateCount;
    public int MinimumRequirementsCount;
    public int NoExactDuplicatesCount;
    public int NoFuzzyDuplicatesCount;
    public List<ExtractedAddress> Addresses = new List<ExtractedAddress>();
    public List<int> DuplicatedAddressIds = new List<int>();
    public bool StopPresentingPossibleDuplicates;

    public void AddAddress(LogAddressesGoodNewReader reader)
    {
      RawCount++;
      if (!reader.IsDuplicate)
      {
        NotMarkedDuplicateCount++;
        bool validZip = string.IsNullOrWhiteSpace(reader.ParsedZip5) ||
          reader.ParsedZip5.Length == 5;
        if (validZip)
          validZip = string.IsNullOrWhiteSpace(reader.ParsedZip4) ||
           reader.ParsedZip4.Length == 4;
        else
          validZip = string.IsNullOrWhiteSpace(reader.ParsedZip4);
        // need address, city, state at a minimum (and valid zip)
        if (string.IsNullOrWhiteSpace(reader.ParsedAddress) ||
         string.IsNullOrWhiteSpace(reader.ParsedCity) ||
         string.IsNullOrWhiteSpace(reader.ParsedStateCode) ||
         !validZip)
          DuplicatedAddressIds.Add(reader.Id);
        else
        {
          Addresses.Add(new ExtractedAddress(reader));
          MinimumRequirementsCount++;
        }
      }
    }

    private List<int> AnalyzeDistances(List<ExtractedAddress> zipGroup, 
      int inx, int[,] distances, bool[] deleted)
    {
      string baseNumerics = zipGroup[inx].LeadingNumericsInAddress;
      List<int> possibleDups = new List<int>();
      // check lower addresses
      for (int inx2 = 0; inx2 < zipGroup.Count; inx2++)
        if (inx2 == inx)
          possibleDups.Add(inx); // always add the base address
        else if (!deleted[inx2])
        {
          int distance = 0;
          if (inx2 < inx)
            distance = int.MaxValue; //distances[inx2, inx];
          else
            distance = distances[inx, inx2];
          if (distance <= 4) // arbitrary, subject to adjustment
            if (baseNumerics == zipGroup[inx2].LeadingNumericsInAddress)
              possibleDups.Add(inx2);
        }
      return possibleDups;
    }

    public void EliminateExactDuplicates()
    {
      // Group by matching addresses
      var groups = Addresses
        .GroupBy(a => a.ToString())
        .ToList();

      // Keep the first of each
      Addresses = groups
        .Select(g => g.First())
        .ToList();
      NoExactDuplicatesCount = Addresses.Count;

      // Add all but the first's to the Duplicates
      foreach (var g in groups)
      {
        var group = g.ToList();
        group.RemoveAt(0);
        DuplicatedAddressIds.AddRange(group.Select(a => a.Id));
      }
    }

    public void EliminateFuzzyDuplicates()
    {
      // Group by 1st 3 digits of zip and city metaphone
      var zipGroups = Addresses
        .GroupBy(addr => 
          addr.Zip5.Substring(0, Math.Min(addr.Zip5.Length, 3)) + "|" +
          DoubleMetaphone.EncodePhrase(addr.City))
        .OrderBy(group => group.Key)
        .ToList();

      List<ExtractedAddress> newAddresses = new List<ExtractedAddress>();

      foreach (var group in zipGroups)
      {
        var zipGroup = group.ToList();
        if (!StopPresentingPossibleDuplicates)
          EliminateFuzzyDuplicatesInOneGroup(zipGroup);
        newAddresses.AddRange(zipGroup);
      }

      Addresses = newAddresses;
    }

    private void EliminateFuzzyDuplicatesInOneGroup(List<ExtractedAddress> zipGroup)
    {
      if (zipGroup.Count < 2) return; // nothing to compare

      // Set up a deletion flag array
      bool[] deleted = new bool[zipGroup.Count];

      // Compute the distance matrix -- we only populate where inx1 < inx2
      // For inx1 == inx2, distance = 0
      // For inx1 > inx2, distance[inx1, inx2] = distance[inx2, inx1]
      int[,] distances = new int[zipGroup.Count, zipGroup.Count];
      for (int inx1 = 0; inx1 < zipGroup.Count - 1; inx1++)
        for (int inx2 = inx1 + 1; inx2 < zipGroup.Count; inx2++)
          distances[inx1, inx2] = 
            zipGroup[inx1].Address.LevenshteinDistance(zipGroup[inx2].Address);

      // Go through each address in the group and look for others that have
      // distance <= 4. For each, scan for leading numerics and disqualify
      // if the leading numerics don't match the focused address.
      for (int inx = 0; inx < zipGroup.Count; inx++)
        if (!deleted[inx])
        {
          List<int> possibleDups = AnalyzeDistances(zipGroup, inx, distances, deleted);
          if (possibleDups.Count > 1 && !StopPresentingPossibleDuplicates) // present
          {
            PossibleDuplicatesForm dupsForm = new PossibleDuplicatesForm();
            foreach (int dupInx in possibleDups)
              dupsForm.AddAddress(zipGroup[dupInx].ToString());
            dupsForm.ShowDialog();
            bool[] checks = dupsForm.GetChecks();
            for (int checkInx = 0; checkInx < checks.Length; checkInx++)
              if (!checks[checkInx]) // mark deleted
                deleted[possibleDups[checkInx]] = true;
            StopPresentingPossibleDuplicates = dupsForm.StopPresenting;
          }
        }

      // Actually delete the deletions, in reverse order
      for (int inx = deleted.Length - 1; inx >= 0; inx--)
        if (deleted[inx])
        {
          DuplicatedAddressIds.Add(zipGroup[inx].Id);
          zipGroup.RemoveAt(inx);
        }
    }

    internal void WriteToCsv(string outputFilePath)
    {
      // Sort by zip
      var sortedAddresses = Addresses
        .OrderBy(a => a.Zip5 + a.Zip4);

      TextWriter writer = null;

      try
      {
        int count = 0;
        writer = new StreamWriter(outputFilePath);
        SimpleCSVWriter csvWriter = new SimpleCSVWriter();
        foreach (var address in sortedAddresses)
        {
          csvWriter.AddField(address.Address);
          csvWriter.AddField(address.City);
          csvWriter.AddField(address.State);
          csvWriter.AddField(address.Zip5);
          csvWriter.AddField(address.Zip4);
          csvWriter.Write(writer);
          count++;
        }
        MainForm.Instance.AppendStatusText("{0} addresses written to csv {1}",
          count, outputFilePath);
      }
      catch (VoteException ex)
      {
        MainForm.Instance.AppendStatusText(ex.Message);
        MainForm.Instance.AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        MainForm.Instance.AppendStatusText(ex.ToString());
        MainForm.Instance.AppendStatusText("Terminated.");
      }
      finally
      {
        if (writer != null)
          writer.Close();
      }
    }
  }
}
