using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  public partial class MainForm
  {
    private static void ProcessStoredCandidateData(DataTable table)
    {
      const string insertCmdText =
        "INSERT INTO candidates_raw" +
          "(ballotName,candidateId,electionDate,electionDistrictId," +
          "electionDistrictName,electionOffice,electionOfficeId," +
          "electionOfficeTypeId,electionParties,electionSpecial," +
          "electionStateId,electionStatus,electionYear,fetch_id," +
          "fetch_method,fetch_parameters,fetch_time,firstName,lastName," +
          "middleName,nickName,officeDistrictId,officeDistrictName," +
          "officeId,officeName,officeParties,officeStateId,officeStatus," +
          "officeTypeId,preferredName,runningMateId,runningMateName," +
          "stateId,suffix,title) VALUES " +
          "(@ballotName,@candidateId,@electionDate,@electionDistrictId," +
          "@electionDistrictName,@electionOffice,@electionOfficeId," +
          "@electionOfficeTypeId,@electionParties,@electionSpecial," +
          "@electionStateId,@electionStatus,@electionYear,@fetchId," +
          "@fetchMethod,@fetchParameters,@fetchTime,@firstName," +
          "@lastName,@middleName,@nickName,@officeDistrictId," +
          "@officeDistrictName,@officeId,@officeName,@officeParties," +
          "@officeStateId,@officeStatus,@officeTypeId,@preferredName," +
          "@runningMateId,@runningMateName,@stateId,@suffix,@title);";

      var fetchErrors = 0;
      var candidatesAdded = 0;
      var duplicateCandidates = 0;
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
        foreach (var candidate in candidateArray.OfType<Dictionary<string, object>>())
        {
          var insertCmd = new MySqlCommand(insertCmdText);
          insertCmd.Parameters.AddWithValue("@ballotName", candidate["ballotName"]);
          insertCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
          insertCmd.Parameters.AddWithValue("@electionDate", candidate["electionDate"]);
          insertCmd.Parameters.AddWithValue("@electionDistrictId",
            candidate["electionDistrictId"]);
          insertCmd.Parameters.AddWithValue("@electionDistrictName",
            candidate["electionDistrictName"]);
          insertCmd.Parameters.AddWithValue("@electionOffice",
            candidate["electionOffice"]);
          insertCmd.Parameters.AddWithValue("@electionOfficeId",
            candidate["electionOfficeId"]);
          insertCmd.Parameters.AddWithValue("@electionOfficeTypeId",
            candidate["electionOfficeTypeId"]);
          insertCmd.Parameters.AddWithValue("@electionParties",
            candidate["electionParties"]);
          insertCmd.Parameters.AddWithValue("@electionSpecial",
            candidate["electionSpecial"]);
          insertCmd.Parameters.AddWithValue("@electionStateId",
            candidate["electionStateId"]);
          insertCmd.Parameters.AddWithValue("@electionStatus",
            candidate["electionStatus"]);
          insertCmd.Parameters.AddWithValue("@electionYear", candidate["electionYear"]);
          insertCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertCmd.Parameters.AddWithValue("@firstName", candidate["firstName"]);
          insertCmd.Parameters.AddWithValue("@lastName", candidate["lastName"]);
          insertCmd.Parameters.AddWithValue("@middleName", candidate["middleName"]);
          insertCmd.Parameters.AddWithValue("@nickName", candidate["nickName"]);
          insertCmd.Parameters.AddWithValue("@officeDistrictId",
            candidate["officeDistrictId"]);
          insertCmd.Parameters.AddWithValue("@officeDistrictName",
            candidate["officeDistrictName"]);
          insertCmd.Parameters.AddWithValue("@officeId", candidate["officeId"]);
          insertCmd.Parameters.AddWithValue("@officeName", candidate["officeName"]);
          insertCmd.Parameters.AddWithValue("@officeParties",
            candidate["officeParties"]);
          insertCmd.Parameters.AddWithValue("@officeStateId",
            candidate["officeStateId"]);
          insertCmd.Parameters.AddWithValue("@officeStatus", candidate["officeStatus"]);
          insertCmd.Parameters.AddWithValue("@officeTypeId", candidate["officeTypeId"]);
          insertCmd.Parameters.AddWithValue("@preferredName",
            candidate["preferredName"]);
          insertCmd.Parameters.AddWithValue("@runningMateId",
            candidate["runningMateId"]);
          insertCmd.Parameters.AddWithValue("@runningMateName",
            candidate["runningMateName"]);
          insertCmd.Parameters.AddWithValue("@stateId", stateId);
          insertCmd.Parameters.AddWithValue("@suffix", candidate["suffix"]);
          insertCmd.Parameters.AddWithValue("@title", candidate["title"]);
          using (var cn = GetOpenConnection())
          {
            insertCmd.Connection = cn;
            try
            {
              insertCmd.ExecuteNonQuery();
              candidatesAdded++;
            }
            catch (MySqlException ex)
            {
              if (ex.Message.StartsWith(("Duplicate entry"), StringComparison.Ordinal)) duplicateCandidates++;
              else throw;
            }
            GetCandidateBioInfo(candidate["candidateId"] as string);
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} candidates added. {3} duplicate candidates",
          table.Rows.Count - fetchErrors, fetchErrors, candidatesAdded,
          duplicateCandidates));
    }
  }
}
