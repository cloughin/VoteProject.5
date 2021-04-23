using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DB.Vote;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.VoteLog
{
  public partial class LogEmailBatchesRow {}

  public partial class LogEmailBatches
  {
    public static LogEmailBatchesTable GetDataForSearchCriteria(DateTime beginTime, 
      DateTime endTime, bool reportSuccess, bool reportFailure, string[] from, 
      string[] user, string[] searchStrings, string joinOption, 
      int commandTimeout = -1)
    {
      const string cmdTemplate =
        "SELECT Id,CreationTime,ContactType,SelectionCriteria," +
          "Description,Found,Skipped,Sent,Failed,UserName,FromEmail,CcEmails," +
          "BccEmails FROM LogEmailBatches {0} ORDER BY CreationTime DESC";

      // build WHERE clause
      joinOption = joinOption.IsEqIgnoreCase("and") ? "AND" : "OR";
      var wheres = new List<string>();
      if (beginTime != DateTime.MinValue) wheres.Add("CreationTime>=@BeginTime");
      if (endTime != DateTime.MaxValue) wheres.Add("CreationTime<@EndTime");
      if (reportSuccess != reportFailure)
        wheres.Add(reportSuccess
          ? "(`Found`-Skipped)=(Sent+Failed) AND Failed=0"
          : "((`Found`-Skipped)<>(Sent+Failed) OR Failed<>0)");
      if (from.Length > 0) wheres.Add($"FromEmail IN ('{string.Join("','", from)}')");
      if (user.Length > 0) wheres.Add($"UserName IN ('{string.Join("','", user)}')");
      if (searchStrings.Length > 0)
        wheres.Add(
          $"(Description LIKE '%{string.Join("%' " + joinOption + " Description LIKE '%", searchStrings.Select(s => s.SqlEscapeLike()))}%')");
      var whereClause = wheres.Count == 0
        ? string.Empty
        : "WHERE " + string.Join(" AND ", wheres);

      var cmdText = string.Format(cmdTemplate, whereClause);

      var cmd = VoteLogDb.GetCommand(cmdText, commandTimeout);
      if (beginTime != DateTime.MinValue) VoteLogDb.AddCommandParameter(cmd, "BeginTime", beginTime);
      if (endTime != DateTime.MaxValue) VoteLogDb.AddCommandParameter(cmd, "EndTime", endTime);
      return FillTable(cmd, LogEmailBatchesTable.ColumnSet.All);
    }

    public static DataTable GetLoggedEmails(string[] contactTypes, string jurisdictionLevel, 
      string[] stateCodes, string[] countyCodes, string[] localCodes, 
      DateTime beginTime, DateTime endTime, bool reportSuccess, bool reportFailure, 
      bool reportFlagged, bool reportUnflagged, int maximumResults, string[] froms, string[] tos, 
      string[] users, string[] electionKeys, string[] officeKeys, string[] candidateKeys, 
      string[] politicianKeys, int[] batchIds, int commandTimeout = -1)
    {
      const string cmdTemplate =
        "SELECT le.`Subject`,le.Contact,le.CountyCode,le.ElectionKey,le.Id," +
          "le.LocalCode,le.LogBatchId,le.OfficeKey,le.PoliticianKey,le.ErrorMessage," +
          "le.SentTime,le.StateCode,le.ToEmail,le.WasSent,le.ForwardedCount," +
          "le.IsFlagged,lb.ContactType,lb.FromEmail,lb.UserName,l.LocalDistrict," +
          "e.ElectionDesc,o.OfficeLine1,o.OfficeLine2,p.Fname,p.Mname," +
          "p.Lname,p.Nickname,p.Suffix,pt.PartyCode FROM votelog.LogEmail le" +
          " INNER JOIN votelog.LogEmailBatches lb ON lb.Id=le.LogBatchId" +
          " LEFT OUTER JOIN Elections e ON e.ElectionKey=le.ElectionKey" +
          " LEFT OUTER JOIN Offices o ON o.OfficeKey=le.OfficeKey" +
          " LEFT OUTER JOIN Politicians p ON p.PoliticianKey=le.PoliticianKey" +
          " LEFT OUTER JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN LocalDistricts l ON l.LocalCode=le.LocalCode" + " {0} {1}";

      // Build the where clause
      var outerAnds = new List<string>();
      var outerOrs = new List<string>();
      var middleAnds = new List<string>();
      var middleOrs = new List<string>();
      var innerAnds = new List<string>();

      var allContacts = contactTypes.Length == 1 && contactTypes[0] == "all";
      var allStates = stateCodes.Length == 1 && stateCodes[0] == "all";
      var allCounties = countyCodes.Length == 1 && countyCodes[0] == "all";
      var allLocals = localCodes.Length == 1 && localCodes[0] == "all";
      var allElections = electionKeys.Length == 1 && electionKeys[0] == "all";
      var allOffices = officeKeys.Length == 1 && officeKeys[0] == "all";
      var allCandidates = candidateKeys.Length == 1 && candidateKeys[0] == "all";

      // success clause (applies to everything)
      if (reportSuccess != reportFailure) outerAnds.Add("le.WasSent=" + (reportSuccess ? "1" : "0"));

      // flagged clause (applies to everything)
      if (reportFlagged != reportUnflagged) outerAnds.Add("le.IsFlagged=" + (reportSuccess ? "1" : "0"));

      // tos  (applies to everything)
      if (tos.Length > 0) outerAnds.Add(tos.SqlIn("le.ToEmail"));

      // batchIds
      if (batchIds.Length > 0) outerOrs.Add(batchIds.SqlIn("lb.Id"));

      // dates
      if (beginTime != DateTime.MinValue) middleAnds.Add("le.SentTime>=@BeginTime");
      if (endTime != DateTime.MaxValue) middleAnds.Add("le.SentTime<@EndTime");

      // froms (a single address in the db)
      if (froms.Length > 0) middleAnds.Add(froms.SqlIn("lb.FromEmail"));

      // users
      if (users.Length > 0) middleAnds.Add(users.SqlIn("lb.UserName"));

      // politicianKeys
      if (politicianKeys.Length > 0) middleOrs.Add(politicianKeys.SqlIn("le.PoliticianKey"));

      // anaylyze states, counties, locals, elections, offices and
      // candidates based on specificity
      if (contactTypes.Length > 0 && (allElections || electionKeys.Length == 0))
      {
        switch (jurisdictionLevel)
        {
          case "states":
            if (stateCodes.Length == 0) // force no results from this section
              innerAnds.Add("le.StateCode=''");
            else if (!allStates) innerAnds.Add(stateCodes.SqlIn("le.StateCode"));
            break;

          case "counties":
            if (stateCodes.Length == 0 || countyCodes.Length == 0) // force no results from this section
              innerAnds.Add("le.StateCode=''");
            else if (allCounties)
            {
              if (!allStates) innerAnds.Add(stateCodes.SqlIn("le.StateCode"));
              innerAnds.Add("le.CountyCode<>''");
              innerAnds.Add("le.LocalCode=''");
            }
            else if (countyCodes.Length > 0)
            {
              Debug.Assert(stateCodes.Length == 1, "Expecting a single state");
              innerAnds.Add("le.StateCode=" + stateCodes[0].SqlLit());
              innerAnds.Add(countyCodes.SqlIn("le.CountyCode"));
              innerAnds.Add("le.LocalCode=''");
            }
            break;

          case "locals":
            if (stateCodes.Length == 0 || countyCodes.Length == 0 ||
              localCodes.Length == 0) // force no results from this section
              innerAnds.Add("le.StateCode=''");
            else if (allLocals)
            {
              if (!allStates) innerAnds.Add(stateCodes.SqlIn("le.StateCode"));
              if (!allCounties) innerAnds.Add(countyCodes.SqlIn("le.CountyCode"));
              innerAnds.Add("le.LocalCode<>''");
            }
            else if (countyCodes.Length > 0)
            {
              Debug.Assert(stateCodes.Length == 1, "Expecting a single state");
              Debug.Assert(countyCodes.Length == 1, "Expecting a single county");
              innerAnds.Add("le.StateCode=" + stateCodes[0].SqlLit());
              innerAnds.Add("le.CountyCode=" + countyCodes[0].SqlLit());
              innerAnds.Add(localCodes.SqlIn("le.LocalCode"));
            }
            break;
        }
      }
      else if (electionKeys.Length == 0) // exclude election-coded emails
        innerAnds.Add("le.ElectionKey=''");
      else if (allOffices) innerAnds.Add(electionKeys.SqlIn("le.ElectionKey"));
      else
      {
        Debug.Assert(electionKeys.Length == 1, "Expecting a single electionKey");
        innerAnds.Add("le.ElectionKey=" + electionKeys[0].SqlLit());
        if (officeKeys.Length == 0) innerAnds.Add("le.OfficeKey=''");
        else if (allCandidates) innerAnds.Add(officeKeys.SqlIn("le.OfficeKey"));
        else
        {
          Debug.Assert(officeKeys.Length == 1, "Expecting a single officeKey");
          innerAnds.Add("le.OfficeKey=" + officeKeys[0].SqlLit());
          innerAnds.Add(candidateKeys.Length == 0
            ? "le.PoliticianKey=''"
            : candidateKeys.SqlIn("le.PoliticianKey"));
        }
      }

      if (!allContacts) innerAnds.Add(contactTypes.SqlIn("lb.ContactType"));

      if (innerAnds.Count > 0) // combine into an OR
        middleOrs.Add(string.Join(" AND ", innerAnds));

      if (middleOrs.Count > 0) // combine into an AND
        middleAnds.Add("(" + string.Join(" OR ", middleOrs) + ")");

      if (middleAnds.Count > 0) // combine into an OR
        outerOrs.Add(string.Join(" AND ", middleAnds));

      if (outerOrs.Count > 0) // combine into an AND
        outerAnds.Add("(" + string.Join(" OR ", outerOrs) + ")");

      var whereClause = outerAnds.Count > 0
        ? "WHERE " + string.Join(" AND ", outerAnds)
        : string.Empty;

      var limitClause = maximumResults > 0
        ? "LIMIT " + maximumResults
        : string.Empty;

      var cmdText = string.Format(cmdTemplate, whereClause, limitClause);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        if (beginTime != DateTime.MinValue) VoteLogDb.AddCommandParameter(cmd, "BeginTime", beginTime);
        if (endTime != DateTime.MaxValue) VoteLogDb.AddCommandParameter(cmd, "EndTime", endTime);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      return table;
    }

    public static DataRow GetLoggedEmailDetail(int id, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT le.Id,le.Body,lb.CcEmails,lb.BccEmails,lb.Description," +
        "lb.SelectionCriteria FROM votelog.LogEmail le " +
        " INNER JOIN votelog.LogEmailBatches lb ON lb.Id=le.LogBatchId" +
        " WHERE le.Id=@Id";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteLogDb.AddCommandParameter(cmd, "Id", id);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      return table.Rows.Count == 0 ? null : table.Rows[0];
    }
  }
}