using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  public partial class MainForm : Form
  {
    private static readonly Regex MethodRegex =
      new Regex(
        @"^(?<method>[a-z]+\.[a-z]+)\((?:(?<parameter>[a-z]+)\*?)?(?:,(?<parameter>[a-z]+)\*?)*\)$",
        RegexOptions.IgnoreCase);

    public MainForm()
    {
      InitializeComponent();
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      if (GetRawRadioButton.Checked) GetRawData();
      if (ProcessStoredRadioButton.Checked) ProcessStoredData();
      if (IterateRadioButton.Checked) Iterate();
      if (MissingCandidatesRadioButton.Checked) GetMissingCandidates();
      if (CreateCandidatesStatesRadioButton.Checked) CreateCandidatesStates();
    }

    private static void GetMissingCandidates()
    {
      var candidateDictionary = new Dictionary<int, string>();
      foreach (
        var c in
          GetCandidatesFromTable("committeemember_raw", "member_candidateId", 
           "committees_states", "committeeId")
            .Where(c => !candidateDictionary.ContainsKey(c.Key))) candidateDictionary[c.Key] = c.Value;
      foreach (
        var c in
          GetCandidatesFromTable("election_stage_candidates_raw", "candidateId",
           "elections_states", "electionId")
            .Where(c => !candidateDictionary.ContainsKey(c.Key))) candidateDictionary[c.Key] = c.Value;
      foreach (
        var c in
          GetCandidatesFromTable("leadership_officials_raw")
            .Where(c => !candidateDictionary.ContainsKey(c.Key))) candidateDictionary[c.Key] = c.Value;
      foreach (
        var c in
          GetCandidatesFromTable("local_officials_raw", "candidateId",
           "locals_states", "localId")
            .Where(c => !candidateDictionary.ContainsKey(c.Key))) candidateDictionary[c.Key] = c.Value;
      foreach (
        var c in
          GetCandidatesFromTable("state_officials_raw")
            .Where(c => !candidateDictionary.ContainsKey(c.Key))) candidateDictionary[c.Key] = c.Value;

      const string insertCmdText =
        "INSERT INTO candidates_states" +
          "(candidateId,stateId) VALUES (@candidateId,@stateId);";

      foreach (var kvp in candidateDictionary)
      {
        var insertCmd = new MySqlCommand(insertCmdText);
        insertCmd.Parameters.AddWithValue("@candidateId", kvp.Key);
        insertCmd.Parameters.AddWithValue("@stateId", kvp.Value);
        using (var cn = GetOpenConnection())
        {
          insertCmd.Connection = cn;
          try
          {
            insertCmd.ExecuteNonQuery();
          }
          catch (MySqlException ex)
          {
            if (!ex.Message.StartsWith(("Duplicate entry"),
              StringComparison.Ordinal)) throw;
          }
        }
        GetCandidateBioInfo(kvp.Key.ToString(CultureInfo.InvariantCulture));
      }
    }

    private static void CreateCandidatesStates()
    {
      const string insertCmdText =
        "INSERT INTO candidates_states" +
          "(candidateId,stateId) VALUES (@candidateId,@stateId);";

      var table = GetRawFetches("Candidates.getByOfficeTypeState", "%%");

      var fetchErrors = 0;
      var rowsAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("candidateList"))
        {
          fetchErrors++;
          continue;
        }
        var candidateList = jsonObj["candidateList"] as Dictionary<string, object>;
        if (candidateList == null || !candidateList.ContainsKey("candidate"))
        {
          fetchErrors++;
          continue;
        }
        var candidateArray = AsArrayList(candidateList["candidate"]);
        if (candidateArray == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (
          var candidate in candidateArray.OfType<Dictionary<string, object>>())
        {
          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            try
            {
              insertCmd.ExecuteNonQuery();
              rowsAdded++;
            }
            catch (MySqlException ex)
            {
              if (ex.Message.StartsWith(("Duplicate entry"),
                StringComparison.Ordinal)) duplicates++;
              else throw;
            }
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} candidates added. {3} duplicate candidates",
          table.Rows.Count - fetchErrors, fetchErrors, rowsAdded,
          duplicates));
    }

    private void GetRawData()
    {
      var method = MethodComboBox.SelectedItem.ToString();
      var match = MethodRegex.Match(method);
      if (!match.Success) throw new Exception("Could not parse method signature");
      var methodName = match.Groups["method"].Value;
      var parameters = GetNormalizedParameters();
      SaveRawData(methodName, parameters);
      MessageBox.Show("Success");
    }

    private void Iterate()
    {
      var method = MethodComboBox.SelectedItem.ToString();
      var match = MethodRegex.Match(method);
      if (!match.Success) throw new Exception("Could not parse method signature");
      var methodName = match.Groups["method"].Value;
      var parameters = GetNormalizedParameters();
      switch (methodName)
      {
        case "District.getByOfficeState":
          // iterate for all officeIds
          var stateId = ParseIdFromParameters(parameters, "stateId");
          foreach (var officeId in GetOfficeIds())
          {
            var parms = "officeId=" + officeId + "&stateId=" + stateId;
            SaveRawData(methodName, parms);
          }
          break;
      }
    }

    private static void SaveRawData(string methodName, string parameters)
    {
      var json = GetVoteSmartJson(methodName, parameters);
      var table = CreateFetchesRawTable();
      var row = table.NewRow();
      row["fetch_time"] = DateTime.UtcNow;
      row["fetch_method"] = methodName;
      row["fetch_parameters"] = parameters;
      row["fetch_type"] = "JSON";
      row["fetch_data"] = json;
      table.Rows.Add(row);
      using (var cn = GetOpenConnection())
      {
        const string cmdText =
          "SELECT fetch_time,fetch_method," +
            "fetch_parameters,fetch_type,fetch_data FROM fetches_raw";
        var cmd = new MySqlCommand(cmdText, cn) {CommandTimeout = 0};
        var adapter = new MySqlDataAdapter(cmd);
        // ReSharper disable once ObjectCreationAsStatement
        new MySqlCommandBuilder(adapter);
        adapter.Update(table);
      }
    }

    private void ProcessStoredData()
    {
      var method = MethodComboBox.SelectedItem.ToString();
      var match = MethodRegex.Match(method);
      if (!match.Success) throw new Exception("Could not parse method signature");
      var methodName = match.Groups["method"].Value;
      var parameters = GetNormalizedParameters(true);

      var table = GetRawFetches(methodName, parameters);

      switch (methodName)
      {
        case "Address.getCampaign":
        case "Address.getOffice":
          ProcessStoredAddressOffice(table);
          break;

        case "Address.getCampaignWebAddress":
        case "Address.getOfficeWebAddress":
          ProcessStoredWebAddress(table);
          break;

        case "Candidates.getByOfficeTypeState":
          ProcessStoredCandidateData(table);
          break;

        case "CandidateBio.getBio":
          ProcessStoredBioData(table);
          break;

        case "CandidateBio.getAddlBio":
          ProcessStoredAddlBioData(table);
          break;

        case "Committee.getCommittee":
          ProcessStoredCommittees(table);
          break;

        case "Committee.getCommitteeMembers":
          ProcessStoredCommitteeMembers(table);
          break;

        case "Committee.getCommitteesByTypeState":
          GetCommitteeInfo(table);
          break;

        case "District.getByOfficeState":
          ProcessStoredDistricts(table);
          break;

        case "Election.getElectionByYearState":
          ProcessStoredElections(table);
          break;

        case "Election.getStageCandidates":
          ProcessStoredElectionRosters(table);
          break;

        case "Leadership.getOfficials":
          ProcessStoredLeadershipOfficials(table);
          break;

        case "Leadership.getPositions":
          ProcessStoredLeadership(table);
          break;

        case "Local.getCities":
          ProcessStoredLocals(table, "cities", "city");
          break;

        case "Local.getCounties":
          ProcessStoredLocals(table, "counties", "county");
          break;

        case "Local.getOfficials":
          ProcessStoredLocalOfficials(table);
          break;

        case "Measure.getMeasure":
          ProcessStoredBallotMeasures(table);
          break;

        case "Measure.getMeasuresByYearState":
          ProcessStoredBallotMeasuresByYearState(table);
          break;

        case "Office.getOfficesByType":
          ProcessStoredOffices(table);
          break;

        case "Officials.getStatewide":
          ProcessStoredStateOfficials(table);
          break;

        case "State.getState":
          ProcessStoredState(table);
          break;

        case "State.getStateIDs":
          GetStateInfo(table);
          break;
      }
    }

    private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      var method = MethodComboBox.SelectedItem.ToString();
      var match = MethodRegex.Match(method);
      if (match.Success)
        ParametersTextBox.Text = String.Join("&",
          match.Groups["parameter"].Captures.Cast<Capture>()
            .Select(c => c.Value + "="));
    }
  }
}