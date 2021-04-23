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
    private static void ProcessStoredCommitteeMembers(DataTable table)
    {
      const string countCmdText = "SELECT COUNT(*) FROM committeemember_raw " +
        "WHERE member_candidateId=@candidateId AND committeeId=@committeeId";

      const string insertMemberCmdText =
        "INSERT INTO committeemember_raw" +
          "(committeeId,fetch_id,fetch_method,fetch_parameters,fetch_time," +
          "member_candidateId,member_firstName,member_lastName," +
          "member_middleName,member_party,member_position,member_suffix," +
          "member_title,name,parentId)" +
          " VALUES (@committeeId,@fetchId,@fetchMethod,@fetchParameters," +
          "@fetchTime,@memberCandidateId,@memberFirstName,@memberLastName," +
          "@memberMiddleName,@memberParty,@memberPosition,@memberSuffix," +
          "@memberTitle,@name,@parentId);";

      var fetchErrors = 0;
      var membersAdded = 0;
      var duplicates = 0;
      foreach (var row in table.Rows.OfType<DataRow>())
      {
        var committeeId = ParseIdFromParameters(row["fetch_parameters"], "committeeId");
        var jsonObj = GetDataAsJson(row);
        if (!jsonObj.ContainsKey("committeeMembers"))
        {
          fetchErrors++;
          continue;
        }
        var committeeMembers = jsonObj["committeeMembers"] as Dictionary<string, object>;
        if (committeeMembers == null || !committeeMembers.ContainsKey("committee") ||
         !committeeMembers.ContainsKey("member"))
        {
          fetchErrors++;
          continue;
        }
        var committee = committeeMembers["committee"] as Dictionary<string, object>;
        var member = AsArrayList(committeeMembers["member"]);
        if (committee == null || member == null)
        {
          fetchErrors++;
          continue;
        }
        foreach (var m in member.Cast<Dictionary<string, object>>())
        {

          // if there are any already there, skip
          var countCmd = new MySqlCommand(countCmdText);
          countCmd.Parameters.AddWithValue("@candidateId", m["candidateId"]);
          countCmd.Parameters.AddWithValue("@committeeId", committeeId);
          int count;
          using (var cn = GetOpenConnection())
          {
            countCmd.Connection = cn;
            count = Convert.ToInt32(countCmd.ExecuteScalar());
          }
          if (count != 0)
          {
            duplicates++;
            continue;
          }

          var insertMemberCmd = new MySqlCommand(insertMemberCmdText);
          insertMemberCmd.Parameters.AddWithValue("@committeeId", committeeId);
          insertMemberCmd.Parameters.AddWithValue("@fetchId", row["id"]);
          insertMemberCmd.Parameters.AddWithValue("@fetchMethod", row["fetch_method"]);
          insertMemberCmd.Parameters.AddWithValue("@fetchParameters", row["fetch_parameters"]);
          insertMemberCmd.Parameters.AddWithValue("@fetchTime", row["fetch_time"]);
          insertMemberCmd.Parameters.AddWithValue("@memberCandidateId", m["candidateId"]);
          insertMemberCmd.Parameters.AddWithValue("@memberFirstName", m["firstName"]);
          insertMemberCmd.Parameters.AddWithValue("@memberLastName", m["lastName"]);
          insertMemberCmd.Parameters.AddWithValue("@memberMiddleName", m["middleName"]);
          insertMemberCmd.Parameters.AddWithValue("@memberParty", m["party"]);
          insertMemberCmd.Parameters.AddWithValue("@memberPosition", m["position"]);
          insertMemberCmd.Parameters.AddWithValue("@memberSuffix", m["suffix"]);
          insertMemberCmd.Parameters.AddWithValue("@memberTitle", m["title"]);
          insertMemberCmd.Parameters.AddWithValue("@name", committee["name"]);
          insertMemberCmd.Parameters.AddWithValue("@parentId", committee["parentId"]);
          using (var cn = GetOpenConnection())
          {
            insertMemberCmd.Connection = cn;
            insertMemberCmd.ExecuteNonQuery();
            membersAdded++;
          }
        }
      }

      MessageBox.Show(
        String.Format(
          "{0} rows processed, {1} had fetch errors," +
            " {2} members added. {3} duplicates,",
          table.Rows.Count - fetchErrors, fetchErrors, membersAdded,
          duplicates));
    }
  }
}
