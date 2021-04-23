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
    private static void ProcessStoredBioData(DataTable table)
    {
      const string insertCandidateCmdText =
        "INSERT INTO bio_candidate_raw" +
          "(birthDate,birthPlace,candidateId,congMembership,crpId,education," +
          "family,fetch_id,fetch_method,fetch_parameters,fetch_time,firstName," +
          "gender,homeCity,homeState,lastName,middleName,nickName,orgMembership," +
          "photo,political,profession,pronunciation,religion,specialMsg,suffix)" +
          " VALUES (@birthDate,@birthPlace,@candidateId,@congMembership,@crpId," +
          "@education,@family,@fetchId,@fetchMethod,@fetchParameters,@fetchTime," +
          "@firstName,@gender,@homeCity,@homeState,@lastName,@middleName," +
          "@nickName,@orgMembership,@photo,@political,@profession,@pronunciation," +
          "@religion,@specialMsg,@suffix);";

      const string insertOfficeCmdText =
        "INSERT INTO bio_office_raw" +
          "(candidateId,district,districtId,fetch_id,fetch_method," +
          "fetch_parameters,fetch_time,firstElect,lastElect,name,nextElect," +
          "parties,shortTitle,stateId,status,termEnd,termStart,title,type)" +
          " VALUES (@candidateId,@district,@districtId,@fetchId,@fetchMethod," +
          "@fetchParameters,@fetchTime,@firstElect,@lastElect,@name,@nextElect," +
          "@parties,@shortTitle,@stateId,@status,@termEnd,@termStart,@title,@type);";

      const string insertOfficeCommitteeCmdText =
        "INSERT INTO bio_office_committee_raw" +
          "(candidateId,committeeId,committeeName,fetch_id,fetch_method," +
          "fetch_parameters,fetch_time) VALUES (@candidateId,@committeeId," +
          "@committeeName,@fetchId,@fetchMethod,@fetchParameters,@fetchTime);";

      const string insertElectionCmdText =
        "INSERT INTO bio_election_raw" +
          "(ballotName,candidateId,district,districtId,fetch_id,fetch_method," +
          "fetch_parameters,fetch_time,office,officeType,parties,status)" +
          " VALUES (@ballotName,@candidateId,@district,@districtId,@fetchId," +
          "@fetchMethod,@fetchParameters,@fetchTime,@office," +
          "@officeType,@parties,@status);";

      var fetchErrors = 0;
      var biosAdded = 0;
      var bioOfficesAdded = 0;
      var bioOfficeCommitteesAdded = 0;
      var bioElectionsAdded = 0;
      var duplicateBios = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var stateId = ParseIdFromParameters(row["fetch_parameters"], "stateId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("bio"))
        {
          fetchErrors++;
          continue;
        }
        var bio = jsonObj["bio"] as Dictionary<string, object>;
        if (bio == null || !bio.ContainsKey("candidate"))
        {
          fetchErrors++;
          continue;
        }
        var candidate = bio["candidate"] as Dictionary<string, object>;
        if (candidate == null)
        {
          fetchErrors++;
          continue;
        }

        var insertCandidateCmd = new MySqlCommand(insertCandidateCmdText);
        insertCandidateCmd.Parameters.AddWithValue("@birthDate", candidate["birthDate"]);
        insertCandidateCmd.Parameters.AddWithValue("@birthPlace", candidate["birthPlace"]);
        insertCandidateCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
        insertCandidateCmd.Parameters.AddWithValue("@congMembership", candidate["congMembership"]);
        insertCandidateCmd.Parameters.AddWithValue("@crpId", candidate["crpId"]);
        insertCandidateCmd.Parameters.AddWithValue("@education", candidate["education"]);
        insertCandidateCmd.Parameters.AddWithValue("@family", candidate["family"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchId", row["id"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
        insertCandidateCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
        insertCandidateCmd.Parameters.AddWithValue("@firstName", candidate["firstName"]);
        insertCandidateCmd.Parameters.AddWithValue("@gender", candidate["gender"]);
        insertCandidateCmd.Parameters.AddWithValue("@homeCity", candidate["homeCity"]);
        insertCandidateCmd.Parameters.AddWithValue("@homeState", candidate["homeState"]);
        insertCandidateCmd.Parameters.AddWithValue("@lastName", candidate["lastName"]);
        insertCandidateCmd.Parameters.AddWithValue("@middleName", candidate["middleName"]);
        insertCandidateCmd.Parameters.AddWithValue("@nickName", candidate["nickName"]);
        insertCandidateCmd.Parameters.AddWithValue("@orgMembership", candidate["orgMembership"]);
        insertCandidateCmd.Parameters.AddWithValue("@photo", candidate["photo"]);
        insertCandidateCmd.Parameters.AddWithValue("@political", candidate["political"]);
        insertCandidateCmd.Parameters.AddWithValue("@profession", candidate["profession"]);
        insertCandidateCmd.Parameters.AddWithValue("@pronunciation", candidate["pronunciation"]);
        insertCandidateCmd.Parameters.AddWithValue("@religion", candidate["religion"]);
        insertCandidateCmd.Parameters.AddWithValue("@specialMsg", candidate["specialMsg"]);
        insertCandidateCmd.Parameters.AddWithValue("@suffix", candidate["suffix"]);
        using (var cn = GetOpenConnection())
        {
          insertCandidateCmd.Connection = cn;
          try
          {
            insertCandidateCmd.ExecuteNonQuery();
            biosAdded++;
          }
          catch (MySqlException ex)
          {
            if (ex.Message.StartsWith(("Duplicate entry"), StringComparison.Ordinal))
            {
              duplicateBios++;
              continue;
            }
            throw;
          }
        }

        if (bio.ContainsKey("office"))
        {
          var office = bio["office"] as Dictionary<string, object>;
          if (office != null)
          {
            var insertOfficeCmd = new MySqlCommand(insertOfficeCmdText);
            insertOfficeCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
            insertOfficeCmd.Parameters.AddWithValue("@district", office["district"]);
            insertOfficeCmd.Parameters.AddWithValue("@districtId", office["districtId"]);
            insertOfficeCmd.Parameters.AddWithValue("@fetchId", row["id"]);
            insertOfficeCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
            insertOfficeCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
            insertOfficeCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
            insertOfficeCmd.Parameters.AddWithValue("@firstElect", office["firstElect"]);
            insertOfficeCmd.Parameters.AddWithValue("@lastElect", office["lastElect"]);
            insertOfficeCmd.Parameters.AddWithValue("@name", office["name"]);
            insertOfficeCmd.Parameters.AddWithValue("@nextElect", office["nextElect"]);
            insertOfficeCmd.Parameters.AddWithValue("@parties", office["parties"]);
            insertOfficeCmd.Parameters.AddWithValue("@shortTitle", office["shortTitle"]);
            insertOfficeCmd.Parameters.AddWithValue("@stateId", office["stateId"]);
            insertOfficeCmd.Parameters.AddWithValue("@status", office["status"]);
            insertOfficeCmd.Parameters.AddWithValue("@termEnd", office["termEnd"]);
            insertOfficeCmd.Parameters.AddWithValue("@termStart", office["termStart"]);
            insertOfficeCmd.Parameters.AddWithValue("@title", office["title"]);
            insertOfficeCmd.Parameters.AddWithValue("@type", office["type"]);
            using (var cn = GetOpenConnection())
            {
              insertOfficeCmd.Connection = cn;
              insertOfficeCmd.ExecuteNonQuery();
              bioOfficesAdded++;
            }

            if (office.ContainsKey("committee"))
            {
              var committee = AsArrayList(office["committee"]);
              if (committee != null)
                foreach (var c in committee.Cast<Dictionary<string, object>>())
                {
                  var insertOfficeCommitteeCmd = new MySqlCommand(insertOfficeCommitteeCmdText);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@committeeId", c["committeeId"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@committeeName", c["committeeName"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@fetchId", row["id"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
                  insertOfficeCommitteeCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
                  using (var cn = GetOpenConnection())
                  {
                    insertOfficeCommitteeCmd.Connection = cn;
                    insertOfficeCommitteeCmd.ExecuteNonQuery();
                    bioOfficeCommitteesAdded++;
                  }
                }
            }
          }
        }
        if (bio.ContainsKey("election"))
        {
          var election = AsArrayList(bio["election"]);
          if (election != null)
            foreach (var e in election.Cast<Dictionary<string, object>>())
            {
              var insertElectionCmd = new MySqlCommand(insertElectionCmdText);
              insertElectionCmd.Parameters.AddWithValue("@ballotName", e["ballotName"]);
              insertElectionCmd.Parameters.AddWithValue("@candidateId", candidate["candidateId"]);
              insertElectionCmd.Parameters.AddWithValue("@district", e["district"]);
              insertElectionCmd.Parameters.AddWithValue("@districtId", e["districtId"]);
              insertElectionCmd.Parameters.AddWithValue("@fetchId", row["id"]);
              insertElectionCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
              insertElectionCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
              insertElectionCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
              insertElectionCmd.Parameters.AddWithValue("@office", e["office"]);
              // contrary to docs, officeId not in data
              //insertElectionCmd.Parameters.AddWithValue("@officeId", e["officeId"]);
              insertElectionCmd.Parameters.AddWithValue("@officeType", e["officeType"]);
              insertElectionCmd.Parameters.AddWithValue("@parties", e["parties"]);
              insertElectionCmd.Parameters.AddWithValue("@status", e["status"]);
              try
              {
                using (var cn = GetOpenConnection())
                {
                  insertElectionCmd.Connection = cn;
                  insertElectionCmd.ExecuteNonQuery();
                  bioElectionsAdded++;
                }
              }
              catch {}
            }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} bios added. {3} duplicate bios," +
            " {4} bio offices added, {5} bio office committees," +
            " {6} bio elections added.",
          table.Rows.Count - fetchErrors, fetchErrors, biosAdded,
          duplicateBios, bioOfficesAdded, bioOfficeCommitteesAdded,
          bioElectionsAdded));
    }
  }
}
