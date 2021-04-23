using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class Elections
  {
    #region Private

    private static string BuildWhereForGetCompareCandidatesReportData(string electionKey,
      string officeKey)
    {
      var stateOrFederalCode = GetStateCodeFromKey(electionKey);
      string where;

      if (GetElectionTypeFromKey(electionKey) == ElectionTypeUSPresidentialPrimary ||
          stateOrFederalCode == "US" && GetElectionTypeFromKey(electionKey) == "G")
        // presidential comparison
        where = $"ep.ElectionKey='{electionKey}'";
      else if (StateCache.IsValidFederalCode(stateOrFederalCode))
        if (StateCache.IsUSGovernors(stateOrFederalCode))
          // no federal key for governors, unfortunately
          where =
            $"o.AlternateOfficeLevel IN ({OfficeClass.USGovernors.ToInt()},{OfficeClass.USLtGovernor.ToInt()})" +
            $" AND ep.ElectionKey LIKE '__{electionKey.Substring(2)}'";
        else
          // search on federal key
          where = $"ep.ElectionKeyFederal='{electionKey}'";
      else
        // just use the election key
        where = $"ep.ElectionKey='{electionKey}'";

      return where + $" AND o.IsInactive = 0 AND ep.OfficeKey='{officeKey}'";
    }

    private static string BuildWhereForGetElectionReportData(string electionKey)
    {
      var stateOrFederalCode = GetStateCodeFromKey(electionKey);
      string where;

      //if (stateOrFederalCode == "US")
      //  electionKey = $"U1{electionKey.Substring(2)}";

      if (GetElectionTypeFromKey(electionKey) == ElectionTypeUSPresidentialPrimary)
        // presidential comparison
        where = $"eo.ElectionKey='{electionKey}'";
      else if (StateCache.IsValidFederalCode(stateOrFederalCode))
        where = StateCache.IsUSGovernors(stateOrFederalCode)
          ? $"o.AlternateOfficeLevel IN ({OfficeClass.USGovernors.ToInt()},{OfficeClass.USLtGovernor.ToInt()})" +
          $" AND eo.ElectionKey LIKE '__{electionKey.Substring(2)}'"
          : $"eo.ElectionKeyFederal='{electionKey}'";
      else
      {
        var altElectionKey = GetElectionKeyToInclude(electionKey);
        where = IsNullOrWhiteSpace(altElectionKey)
          ? $"eo.ElectionKey='{electionKey}'"
          : $"eo.ElectionKey IN ('{electionKey}','{altElectionKey}')";
      }

      return where + " AND o.IsInactive = 0";
    }

    private static string BuildWhereForFutureElectionsWithSubstitutionOptions(
      string stateCode, Substitutions.Options options)
    {
      var where = new List<string>();

      // These are in all wheres
      where.Add("StateCode='" + stateCode + "'");
      where.Add("CountyCode=''");
      where.Add("LocalKey=''");
      where.Add("ElectionDate>=NOW()");

      // handle the election types
      var electionTypes = new List<string>();
      if ((options & Substitutions.Options.GeneralElection) != 0)
        electionTypes.Add("G");
      if ((options & Substitutions.Options.OffYearElection) != 0)
        electionTypes.Add("O");
      if ((options & Substitutions.Options.PrimaryElection) != 0)
        electionTypes.Add("P");
      if ((options & Substitutions.Options.SpecialElection) != 0)
        electionTypes.Add("S");
      if ((options & Substitutions.Options.PresidentialPrimaryElection) != 0)
        electionTypes.Add("B");
      if (electionTypes.Count > 0)
        where.Add(electionTypes.SqlIn("ElectionType"));

      // handle viewability
      if ((options & Substitutions.Options.Viewable) != 0)
        where.Add("IsViewable=1");
      else if ((options & Substitutions.Options.NotViewable) != 0)
        where.Add("IsViewable=0");

      // put it all together
      return "WHERE " + Join(" AND ", where);
    }

    private static string BuildWhereAndOrderByForSubstitutionOptions(string stateCode,
      Substitutions.Options options, string nationalPartyCode = "")
    {
      var where = new List<string>();
      var orderBy = new List<string>();

      // These are in all wheres
      where.Add("StateCode='" + stateCode + "'");
      where.Add("CountyCode=''");
      where.Add("LocalKey=''");

      // The Past/Future options are straightforward unless neother is specified.
      // Then we want the soonest future election if one exists. Otherwise we want
      // the latest past election. We accomplish that with a two level ORDER BY
      // The first level, ElectionDate>=NOW() DESC insures that all future
      // elections sort before any past elections, so a past election is only used
      // if there are no futures ones. The second level, 
      // ABS(TIMESTAMPDIFF(Second,NOW(),ElectionDate)) orders the elections with the
      // closest to the current date first.
      if ((options & Substitutions.Options.Future) != 0)
      {
        where.Add("ElectionDate>=NOW()");
        orderBy.Add("ElectionDate");
      }
      else if ((options & Substitutions.Options.Past) != 0)
      {
        where.Add("ElectionDate<NOW()");
        orderBy.Add("ElectionDate DESC");
      }
      else
      {
        orderBy.Add("ElectionDate>=NOW() DESC");
        orderBy.Add("ABS(TIMESTAMPDIFF(Second,NOW(),ElectionDate))");
      }

      // handle the election types
      var electionTypes = new List<string>();
      if ((options & Substitutions.Options.GeneralElection) != 0)
        electionTypes.Add("G");
      if ((options & Substitutions.Options.OffYearElection) != 0)
        electionTypes.Add("O");
      if ((options & Substitutions.Options.PrimaryElection) != 0)
        electionTypes.Add("P");
      if ((options & Substitutions.Options.SpecialElection) != 0)
        electionTypes.Add("S");
      if ((options & Substitutions.Options.PresidentialPrimaryElection) != 0)
        electionTypes.Add("B");
      if (electionTypes.Count > 0)
        where.Add(electionTypes.SqlIn("ElectionType"));

      // handle viewability
      if ((options & Substitutions.Options.Viewable) != 0)
        where.Add("IsViewable=1");
      else if ((options & Substitutions.Options.NotViewable) != 0)
        where.Add("IsViewable=0");

      // handle nationalPartyCode
      if (!IsNullOrWhiteSpace(nationalPartyCode))
        where.Add("NationalPartyCode='" + nationalPartyCode + "'");

      // put it all together
      return "WHERE " + Join(" AND ", where) + " ORDER BY " +
        Join(",", orderBy);
    }

    private static string BuildWhereForPrimariesWithSubstitutionOptions(string stateCode,
      Substitutions.Options options, bool allElectionTypes = false)
    {
      // This is similar to the above, but uses a subquery to get all primaries on
      // the same date. The Election Type options are ignored, Primary is assumed.

      // We start with the where conditions used in both main and subqueries
      var mainWhere = new List<string>
      {
// These are in all wheres
        "StateCode='" + stateCode + "'",
        "CountyCode=''",
        "LocalKey=''"
      };

      if (!allElectionTypes) mainWhere.Add("ElectionType='P'");

      // handle viewability
      if ((options & Substitutions.Options.Viewable) != 0)
        mainWhere.Add("IsViewable=1");
      else if ((options & Substitutions.Options.NotViewable) != 0)
        mainWhere.Add("IsViewable=0");

      // clone this for the subquery
      var subqueryWhere = new List<string>(mainWhere);

      // add extra condition to subquery for Past or Future
      if ((options & Substitutions.Options.Future) != 0)
      {
        subqueryWhere.Add("ElectionDate>=NOW()");
      }
      else if ((options & Substitutions.Options.Past) != 0)
      {
        subqueryWhere.Add("ElectionDate<NOW()");
      }

      // assign the subquery ORDER BY
      string subqueryOrderBy;
      if ((options & Substitutions.Options.Future) != 0)
      {
        subqueryOrderBy = "ElectionDate";
      }
      else if ((options & Substitutions.Options.Past) != 0)
      {
        subqueryOrderBy = "ElectionDate DESC";
      }
      else
        subqueryOrderBy =
          "ElectionDate>=NOW() DESC,ABS(TIMESTAMPDIFF(Second,NOW(),ElectionDate))";

      // add the subquery condition
      mainWhere.Add("ElectionDate=(SELECT ElectionDate FROM Elections" +
        $" WHERE {Join(" AND ", subqueryWhere)} ORDER BY {subqueryOrderBy} LIMIT 1)");

      // build the main WHERE
      return "WHERE " + Join(" AND ", mainWhere);
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable ReturnTypeCanBeEnumerable.Global

    public static void ActualizeElection(string electionKey)
    {
      if (IsStateElection(electionKey)) return;
      if (ElectionKeyExists(electionKey)) return;
      var table = GetDataByElectionKey(GetStateElectionKeyFromKey(electionKey));
      if (table.Count == 0) return;
      table.Rows[0].SetAdded();
      table.Rows[0]["ElectionKey"] = electionKey;
      table.Rows[0]["CountyCode"] = GetCountyCodeFromKey(electionKey);
      table.Rows[0]["LocalKey"] = GetLocalKeyFromKey(electionKey);
      UpdateTable(table);
    }

    public static string ChangeElectionDate(string stateElectionKey, DateTime newDate,
      int commandTimeout = -1)
    {
      DbConnection cn = null;
      DbTransaction transaction = null;
      var newStateElectionKey = ReplaceDateInKey(stateElectionKey, newDate);

      if (!IsStateElection(stateElectionKey))
        throw new VoteException("Can only change date at the state level");

      if (ElectionKeyExists(newStateElectionKey))
        throw new VoteException("There is already an election of this type on this date");

      try
      {
        cn = VoteDb.GetOpenConnection();
        transaction = cn.BeginTransaction();

        var keyFamily = GetElectionKeyFamily(stateElectionKey);

        foreach (var electionKey in keyFamily)
        {
          // ElectionsOffices
          var electionsOffices = ElectionsOffices.GetDataByElectionKey(electionKey,
            commandTimeout);
          foreach (var office in electionsOffices)
          {
            office.ElectionKey = ReplaceDateInKey(office.ElectionKey, newDate);
            office.ElectionKeyFederal = ReplaceDateInKey(office.ElectionKeyFederal, newDate);
            office.ElectionKeyState = ReplaceDateInKey(office.ElectionKeyState, newDate);
          }
          ElectionsOffices.UpdateTable(electionsOffices, commandTimeout);

          // ElectionsPoliticians
          var electionsPoliticians = ElectionsPoliticians.GetDataByElectionKey(electionKey,
            commandTimeout);
          foreach (var politician in electionsPoliticians)
          {
            politician.ElectionKey = ReplaceDateInKey(politician.ElectionKey, newDate);
            politician.ElectionKeyFederal = ReplaceDateInKey(politician.ElectionKeyFederal,
              newDate);
            politician.ElectionKeyState = ReplaceDateInKey(politician.ElectionKeyState,
              newDate);
          }
          ElectionsPoliticians.UpdateTable(electionsPoliticians, commandTimeout);

          // Referendums
          var referendums = Referendums.GetDataByElectionKey(electionKey, commandTimeout);
          foreach (var referendum in referendums)
          {
            if (referendum.ReferendumKey.StartsWith(referendum.ElectionKey,
              StringComparison.OrdinalIgnoreCase))
              referendum.ReferendumKey = ReplaceDateInKey(referendum.ReferendumKey, newDate);
            referendum.ElectionKey = ReplaceDateInKey(referendum.ElectionKey, newDate);
            referendum.ElectionKeyState = ReplaceDateInKey(referendum.ElectionKeyState,
              newDate);
          }
          Referendums.UpdateTable(referendums, commandTimeout);

          // Elections
          var elections = GetDataByElectionKey(electionKey, commandTimeout);
          foreach (var election in elections)
          {
            election.ElectionDate = newDate;
            election.ElectionDesc = FormatElectionDescription(newStateElectionKey);
            election.ElectionKey = ReplaceDateInKey(election.ElectionKey, newDate);
            election.ElectionKeyCanonical = ReplaceDateInKey(election.ElectionKeyCanonical,
              newDate);
            election.ElectionYYYYMMDD = newDate.ToString("yyyyMMdd");
          }
          UpdateTable(elections, commandTimeout);
        }

        // Elections.ElectionKeyToInclude
        UpdateElectionKeyToIncludeByElectionKeyToInclude(newStateElectionKey,
          stateElectionKey);

        // Create new ElectionsDefaults row
        var def =
          ElectionsDefaults.GetDataByDefaultElectionKey(
            GetDefaultElectionKeyFromKey(stateElectionKey)).FirstOrDefault();
        if (def != null)
          ElectionsDefaults.Insert(GetDefaultElectionKeyFromKey(newStateElectionKey),
            def.ElectionAdditionalInfo, def.BallotInstructions, def.RegistrationDeadline,
            def.EarlyVotingBegin, def.EarlyVotingEnd, def.MailBallotBegin, def.MailBallotEnd,
            def.MailBallotDeadline, def.AbsenteeBallotBegin, def.AbsenteeBallotEnd,
            def.AbsenteeBallotDeadline);
        else
          ElectionsDefaults.CreateEmptyRow(newStateElectionKey);

        ElectionsDefaults.RemoveOrphanedRow(stateElectionKey);

        transaction.Commit();
        cn.Close();
      }
      catch
      {
        transaction?.Rollback();
        cn?.Close();
        throw;
      }

      return newStateElectionKey;
    }

    public static int CountAllStateElections(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Elections" + " WHERE CountyCode='' AND LocalKey=''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static IDictionary<string, int> CountStateElectionsByYear(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT SUBSTR(e.ElectionKey,3,4) AS Year FROM Elections e" +
        " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey = e.ElectionKey" +
        " GROUP BY SUBSTR(e.ElectionKey, 1, 12)";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>()
          .GroupBy(r => r.Year())
          .ToDictionary(g => g.Key, g => g.Count());
      }
    }

    public static void CreateOffYearGovernorsPseudoElections(int commandTimeout = -1)
    {
      string message;

      try
      {
        VotePage.LogInfo("CreateOffYearGovernorsPseudoElections", "Started");

        var cmdText = "SELECT CONCAT('U4', SUBSTR(e.ElectionKey,3,10)) AS ElectionKey FROM Elections e" +
          " INNER JOIN ElectionsOffices eo ON eo.ElectionKey = e.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey = eo.OfficeKey" +
          " LEFT OUTER JOIN Elections e2 ON e2.ElectionKey = CONCAT('U4', SUBSTR(e.ElectionKey, 3, 10))" +
          " WHERE e.ElectionType = 'O' AND e.IsViewable = 1 AND e2.ElectionKey IS NULL" +
          $" AND o.AlternateOfficeLevel='{OfficeClass.USGovernors.ToInt()}'" +
          " GROUP BY ElectionKey" +
          " ORDER BY ElectionKey";

        var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
        using (var cn = VoteDb.GetOpenConnection())
        {
          cmd.Connection = cn;
          var table = new DataTable();
          DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
          adapter.Fill(table);
          var keysToCreate = table.Rows.Cast<DataRow>().Select(r => r.ElectionKey()).ToList();
          foreach (var electionKey in keysToCreate)
          {
            Insert(electionKey, GetStateCodeFromKey(electionKey), Empty, Empty,
              GetElectionDateFromKey(electionKey), GetElectionDateStringFromKey(electionKey),
              GetElectionTypeFromKey(electionKey), Parties.NationalPartyAll,
              Parties.FormatPartyKey(GetStateCodeFromKey(electionKey),
                Parties.NationalPartyAll), Empty,
              $"{GetElectionDateFromKey(electionKey):MMMM d, yyyy} Off-Year Election of Governors State-By-State",
              Empty, Empty, VotePage.DefaultDbDate, Empty, true,
              Empty, 0, false, false, VotePage.DefaultDbDate, VotePage.DefaultDbDate,
              VotePage.DefaultDbDate, VotePage.DefaultDbDate, VotePage.DefaultDbDate,
              VotePage.DefaultDbDate, VotePage.DefaultDbDate, VotePage.DefaultDbDate,
              VotePage.DefaultDbDate, null);
          }

          message =
            $"{keysToCreate.Count} keys created";
        }

      }
      catch (Exception ex)
      {
        VotePage.LogException("CreateOffYearGovernorsPseudoElections", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("CreateOffYearGovernorsPseudoElections", message);
    }

    private static void DeleteElectionPrivate(string electionKey, bool deleteFamily,
      int commandTimeout = -1)
    {
      DbConnection cn = null;
      DbTransaction transaction = null;

      if (deleteFamily)
      {
        if (!IsStateElection(electionKey))
          throw new VoteException("Expected a state election key");
        electionKey += "%";
      }

      const string deleteElectionsCmdText =
        "DELETE FROM Elections WHERE ElectionKey LIKE @ElectionKey";

      const string deleteElectionsOfficesCmdText =
        "DELETE FROM ElectionsOffices WHERE ElectionKey LIKE @ElectionKey";

      const string deleteElectionsPoliticiansCmdText =
        "DELETE FROM ElectionsPoliticians WHERE ElectionKey LIKE @ElectionKey";

      const string deleteReferendumsCmdText =
        "DELETE FROM Referendums WHERE ElectionKey LIKE @ElectionKey";

      const string removeElectionKeyToIncludeReference =
        "UPDATE Elections SET ElectionKeyToInclude='' WHERE ElectionKeyToInclude LIKE @ElectionKey";

      try
      {
        cn = VoteDb.GetOpenConnection();
        transaction = cn.BeginTransaction();

        var cmd = VoteDb.GetCommand(deleteElectionsCmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.ExecuteNonQuery(cmd);

        cmd = VoteDb.GetCommand(deleteElectionsOfficesCmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.ExecuteNonQuery(cmd);

        cmd = VoteDb.GetCommand(deleteElectionsPoliticiansCmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.ExecuteNonQuery(cmd);

        cmd = VoteDb.GetCommand(deleteReferendumsCmdText, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.ExecuteNonQuery(cmd);

        cmd = VoteDb.GetCommand(removeElectionKeyToIncludeReference, commandTimeout);
        VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
        VoteDb.ExecuteNonQuery(cmd);

        ElectionsDefaults.RemoveOrphanedRow(electionKey);

        transaction.Commit();
        cn.Close();
      }
      catch
      {
        transaction?.Rollback();
        cn?.Close();
        throw;
      }
    }

    public static void DeleteElection(string electionKey, int commandTimeout = -1)
    {
      DeleteElectionPrivate(electionKey, false, commandTimeout);
    }

    public static void DeleteElectionFamily(string electionKey, int commandTimeout = -1)
    {
      DeleteElectionPrivate(electionKey, true, commandTimeout);
    }

    public static IList<DataRow> GetActiveElectionOfficeData(string electionKey,
      string stateCode, string countyCode, string localKey,
      int commandTimeout = -1)
    {
      List<DataRow> result;
      const string cmdText =
        "SELECT o.OfficeKey,o.OfficeLevel,o.DistrictCode,o.OfficeLine1,o.OfficeLine2," +
        "o.OfficeOrderWithinLevel," + 
        //"o.DistrictCodeAlpha," + 
        "o.IsVirtual,o.Undeletable," +
        " COUNT(ep.PoliticianKey) AS CandidateCountForOffice" + 
        " FROM ElectionsOffices eo" +
        " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
        " LEFT JOIN ElectionsPoliticians ep ON ep.ElectionKey=@ElectionKey" +
        "  AND ep.OfficeKey=eo.OfficeKey" +
        " WHERE o.IsInactive=0 AND eo.ElectionKey=@ElectionKey GROUP BY o.OfficeKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        result = table.Rows.Cast<DataRow>().ToList();
      }

      string cmdText2 = null;
      {
        if (!IsNullOrWhiteSpace(localKey))
          cmdText2 = "SELECT OfficeKey,OfficeLevel,DistrictCode,OfficeLine1,OfficeLine2," +
            "OfficeOrderWithinLevel," + 
            //"DistrictCodeAlpha," + 
            "IsVirtual,Undeletable," +
            " 0 AS CandidateCountForOffice FROM Offices" +
            " WHERE IsInactive=0 AND StateCode=@StateCode AND LocalKey='#####'";
        else if (!IsNullOrWhiteSpace(countyCode))
          cmdText2 = "SELECT OfficeKey,OfficeLevel,DistrictCode,OfficeLine1,OfficeLine2," +
            "OfficeOrderWithinLevel," + 
            //"DistrictCodeAlpha," + 
            "IsVirtual,Undeletable," +
            " 0 AS CandidateCountForOffice FROM Offices" +
            " WHERE IsInactive=0 AND StateCode=@StateCode AND CountyCode='###'";
      }

      if (cmdText2 != null)
      {
        var cmd2 = VoteDb.GetCommand(cmdText2, commandTimeout);
        VoteDb.AddCommandParameter(cmd2, "StateCode", stateCode);
        using (var cn2 = VoteDb.GetOpenConnection())
        {
          cmd2.Connection = cn2;
          var table2 = new DataTable();
          DbDataAdapter adapter2 = new MySqlDataAdapter(cmd2 as MySqlCommand);
          adapter2.Fill(table2);
          // filter out any that are already actualized and add to result
          result.AddRange(table2.Rows.Cast<DataRow>()
            .Where(r => !result.Any(r2 => Offices.MatchesOfficeCode(r.OfficeKey(), r2.OfficeKey())))
            .ToList());
        }
      }

      // sort and return
      return result
        .OrderBy(r => r.OfficeLevel())
        .ThenBy(r => r.DistrictCode())
        .ThenBy(r => r.OfficeOrderWithinLevel())
        //.ThenBy(r => r.DistrictCodeAlpha())
        .ThenBy(r => r.OfficeLine1())
        .ThenBy(r => r.OfficeLine2())
        .ToList();
    }

    public static List<SimpleListItem> GetElectionItemsByStateCode(string stateCode, 
      string defaultText = "<select>")
    {
      return new List<SimpleListItem> { new SimpleListItem(Empty, defaultText) }.Union(
        Enumerable.Select(
          GetControlDataByStateCodeCountyCodeLocalKey(stateCode, Empty, Empty),
          row => new SimpleListItem(row.ElectionKey, row.ElectionDesc))).ToList();
    }

    public static IList<DataRow> GetJurisdictionOfficeData(string stateCode, string countyCode,
      string localKey, int commandTimeout = -1)
    {
      var table = new DataTable();

      // include virtuals offices for state level
      var isForState = StateCache.IsValidStateCode(stateCode) &&
        IsNullOrWhiteSpace(countyCode) && IsNullOrWhiteSpace(localKey);
      
      string cmdText;
      if (isForState)
      {
        cmdText =
          "SELECT OfficeKey,OfficeLevel,OfficeLine1,OfficeLine2,IsVirtual,IsInactive,Undeletable" +
          " FROM Offices" +
          " WHERE StateCode=@StateCode" +
          " AND (CountyCode = '' AND LocalKey = ''  OR CountyCode='###' OR LocalKey='#####')" +
          " ORDER BY OfficeLevel,DistrictCode,OfficeOrderWithinLevel," +
          //" DistrictCodeAlpha," + 
          "OfficeLine1";
      }
      else
      {
        cmdText =
          "SELECT OfficeKey,OfficeLevel,OfficeLine1,OfficeLine2,IsVirtual,IsInactive,Undeletable," +
          "DistrictCode,OfficeOrderWithinLevel" + 
          //",DistrictCodeAlpha" + 
          " FROM Offices" +
          " WHERE StateCode=@StateCode" +
          (IsNullOrWhiteSpace(localKey)
            ? IsNullOrWhiteSpace(countyCode)
              ? " AND CountyCode=@CountyCode"
              : " AND CountyCode IN ('###',@CountyCode)"
            : " AND LocalKey IN ('#####',@LocalKey)") + " ORDER BY IsVirtual";
      }

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var list = table.Rows.Cast<DataRow>().ToList();
      if (!isForState) list = PruneRedundantVirtualOffices(list, countyCode, localKey);
      return list;
    }

    public static IList<DataRow> GetAvailableElectionOfficeData(string electionKey,
      string stateCode, string countyCode, string localKey,
      int commandTimeout = -1)
    {
      var cmdText =
        "SELECT o.OfficeKey,o.OfficeLevel,o.Undeletable,o.OfficeLine1,o.OfficeLine2," +
          "o.DistrictCode,o.OfficeOrderWithinLevel," + 
          //"o.DistrictCodeAlpha," +
          "o.IsVirtual," + 
          "o.ElectionPositions,o.PrimaryPositions,o.PrimaryRunoffPositions," +
          "o.GeneralRunoffPositions,e.ElectionType," +
          " NOT eo.ElectionKey IS NULL AS IsOfficeInElection," +
          " COUNT(ep.PoliticianKey) AS CandidateCountForOffice" + 
          " FROM Offices o" +
          " LEFT JOIN Elections e ON e.ElectionKey=@ElectionKey" +
          " LEFT JOIN ElectionsOffices eo ON eo.ElectionKey=@ElectionKey" +
          "  AND eo.OfficeKey=o.OfficeKey" +
          " LEFT JOIN ElectionsPoliticians ep ON ep.ElectionKey=@ElectionKey" +
          "  AND ep.OfficeKey=o.OfficeKey" +
          " WHERE o.IsInactive=0 AND (o.StateCode=@StateCode" +
          (IsNullOrWhiteSpace(localKey)
            ? IsNullOrWhiteSpace(countyCode)
              // no CountyCode or LocalKey => state election
              ? " AND o.CountyCode='' AND o.LocalKey=''"
              // has CountyCode (no LocalKey) => county election
              : " AND o.CountyCode IN ('###',@CountyCode)"
           // has LocalKey (no CountyCode) => local election
            : " AND o.LocalKey IN ('#####',@LocalKey)") +
          // presidential
          $" OR (SUBSTR(@ElectionKey,11,1) NOT IN ('{ElectionTypeOffYear}','{ElectionTypeSpecial}')" +
          $"  AND o.OfficeLevel={OfficeClass.USPresident.ToInt()} AND @CountyCode='' AND @LocalKey=''))" +
          " GROUP BY o.OfficeKey" +
          " ORDER BY o.IsVirtual ";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        var list = table.Rows.Cast<DataRow>().ToList();
        return PruneRedundantVirtualOffices(list, countyCode, localKey);
      }
    }

    private static List<DataRow> PruneRedundantVirtualOffices(IEnumerable<DataRow> list,
      string countyCode, string localKey)
    {
      var keyPrefixLength =
        localKey.SafeString().Length == 5
        ? 7
        : IsNullOrWhiteSpace(countyCode)
          ? 2
          : IsNullOrWhiteSpace(localKey)
            ? 5
            : 7;

      // drop any virtuals that are actualized
      // actual will be first in group due to sort
      // and re-sort
      var pruned = list.GroupBy(r => r.OfficeKey().Substring(keyPrefixLength), StringComparer.OrdinalIgnoreCase)
        .Select(g => g.First())
        .OrderBy(r => r.OfficeLevel())
        .ThenBy(r => r.DistrictCode())
        .ThenBy(r => r.OfficeOrderWithinLevel())
        //.ThenBy(r => r.DistrictCodeAlpha())
        .ThenBy(r => r.OfficeLine1())
        .ThenBy(r => r.OfficeLine2())
        .ToList();
      return pruned;
    }

    public static DataTable GetCompareCandidatesReportData(string electionKey, string officeKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "ep.ElectionKey,ep.OfficeKey,ep.StateCode,o.AlternateOfficeLevel," +
        "o.CountyCode,o.DistrictCode,o.Incumbents,o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice," +
        "o.IsVacant,o.LocalKey,o.OfficeKey,o.OfficeLevel,o.OfficeLine1," +
        "o.OfficeLine2,o.OfficeOrderWithinLevel," +
        "o.ElectionPositions,o.GeneralRunoffPositions,o.PrimaryPositions,o.PrimaryRunoffPositions," +
        "ep.PoliticianKey AS ElectionsPoliticianKey,ep.RunningMateKey," +
        "ep.OrderOnBallot,ep.IsIncumbent,ep.IsWinner,p.AddOn,p.Address," +
        "p.BallotPediaWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip,p.DateOfBirth,p.EmailAddr AS Email," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.FName AS FirstName," +
        "p.GooglePlusWebAddress," +
        "p.LinkedInWebAddress,p.LName AS LastName,p.MName AS MiddleName," +
        "p.Nickname,p.PartyKey,p.Phone,p.PinterestWebAddress," +
        "p.PoliticianKey,p.RSSFeedWebAddress,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.TwitterWebAddress," +
        "p.VimeoWebAddress,p.WebAddr AS WebAddress,p.WebstagramWebAddress," +
        "p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "p.YouTubeDescription,p.YouTubeRunningTime,p.YouTubeDate,p.YouTubeAutoDisable," +
        "pt.PartyCode,pt.PartyName," +
        "pt.PartyUrl,e.ElectionType,e.NationalPartyCode";

      var cmdText =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " LEFT JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" + " WHERE {1}" +
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Elections e ON e.ElectionKey=ep.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" +
          " WHERE {1} ORDER BY OrderOnBallot",
          columnList, BuildWhereForGetCompareCandidatesReportData(electionKey, officeKey));

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static ElectionsTable GetControlDataByStateCodeCountyCodeLocalKey(string stateCode,
      string countyCode, string localKey, int commandTimeout = -1)
    {
      var cmdText = SelectControlCommandText +
      (IsNullOrEmpty(localKey)
        ? " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND LocalKey=''" +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC,ElectionDesc ASC"
        : " WHERE StateCode=@StateCode AND CountyCode='' AND LocalKey=@LocalKey" +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC,ElectionDesc ASC");
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode.SafeString());
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey.SafeString());
      return FillTable(cmd, ElectionsTable.ColumnSet.Control);
    }

    public static ElectionsTable GetDisplayDataByStateCodeElectionTypeIsViewable(string stateCode, 
      string electionType, bool isViewable, int commandTimeout = -1)
    {
      var cmdText = SelectDisplayCommandText +
        " WHERE StateCode=@StateCode" +
        (IsNullOrWhiteSpace(electionType) ? Empty : $" AND ElectionType='{electionType}'") + 
        " AND IsViewable=@IsViewable" +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionType", electionType);
      VoteDb.AddCommandParameter(cmd, "IsViewable", isViewable);
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static IList<IGrouping<DateTime, ElectionsRow>> GetElectionControlData(string stateCode,
      string countyCode, string localKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionKey,ElectionDate,ElectionDesc,ElectionOrder FROM Elections" +
        " WHERE StateCode=@StateCode" +
        (IsNullOrWhiteSpace(localKey)
          ? " AND CountyCode=@CountyCode"
          : Empty) +" AND LocalKey=@LocalKey" +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC,ElectionDesc ASC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode.SafeString());
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey.SafeString());
      return
        FillTable(cmd, ElectionsTable.ColumnSet.Control)
          .GroupBy(row => row.ElectionDate)
          .ToList();
    }

    public static DateTime GetElectionDateForSubstitutions(string stateCode,
      Substitutions.Options options, string majorPartyCode = "")
    {
      const string cmdTemplate = "SELECT ElectionDate FROM Elections {0} LIMIT 1";

      var cmdText = Format(cmdTemplate,
        BuildWhereAndOrderByForSubstitutionOptions(stateCode, options, majorPartyCode));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return VotePage.DefaultDbDate;
      return (DateTime) result;
    }

    public static string GetElectionDescriptionForSubstitutions(string stateCode,
      Substitutions.Options options, string majorPartyCode = "")
    {
      const string cmdTemplate = "SELECT ElectionDesc FROM Elections {0} LIMIT 1";

      var cmdText = Format(cmdTemplate,
        BuildWhereAndOrderByForSubstitutionOptions(stateCode, options, majorPartyCode));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      var result = VoteDb.ExecuteScalar(cmd);
      return (result as string).SafeString();
    }

    public static Dictionary<string, string> GetElectionDescriptions(IEnumerable<string> electionKeys)
    {
      var cmdText = "SELECT ElectionKey,ElectionDesc FROM Elections" + 
        $" WHERE {electionKeys.SqlIn("ElectionKey")}";

      var cmd = VoteDb.GetCommand(cmdText, -1);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.OfType<DataRow>().ToDictionary(r => r.ElectionKey(), r => r.ElectionDescription());
      }
    }

    public static DataTable GetElectionsForSitemap(string stateCode, int commandTimeout = -1)
    {
      // stateCode can be "US"
      const string cmdText =
        "SELECT e.ElectionKey,e.ElectionDate,ep.OfficeKey,ep.PoliticianKey,ep.RunningMateKey" +
        " FROM Elections e" +
        " INNER JOIN ElectionsPoliticians ep ON ep.ElectionKey=e.ElectionKey" +
        " WHERE e.StateCode IN (@StateCode,'US') AND e.CountyCode=''" +
        " AND e.LocalKey='' AND e.IsViewable=1";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static List<string> GetElectionKeyFamily(string electionKey, int commandTimeout = -1)
    {
      if (!IsStateElection(electionKey)) return null;
      electionKey += "%";
      const string cmdText = "SELECT ElectionKey FROM Elections WHERE ElectionKey LIKE @ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>().Select(r => r.ElectionKey()).ToList();
      }
    }

    public static List<string> GetVirtualElectionKeyFamily(string electionKey)
    {
      if (!IsStateElection(electionKey)) return null;
      var stateCode = GetStateCodeFromKey(electionKey);
      var result = new List<string> { electionKey };
      // add all counties
      result.AddRange(Counties.GetCacheDataByStateCode(stateCode)
        .Select(c => electionKey + c.CountyCode));
      // add all locals
      result.AddRange(LocalDistricts.GetCacheDataByStateCode(stateCode)
        .Select(l => electionKey + l.LocalKey));
      return result;
    }

    public static DataTable GetElectionReportData(string electionKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "eo.ElectionKey,eo.OfficeKey,eo.StateCode,o.AlternateOfficeLevel," +
        "o.CountyCode,o.DistrictCode,o.Incumbents,o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice," +
        "o.IsVacant,o.LocalKey,o.OfficeKey,o.OfficeLevel,o.OfficeLine1," +
        "o.OfficeLine2,o.OfficeOrderWithinLevel," + 
        //"o.DistrictCodeAlpha," +
        "o.ElectionPositions,o.PrimaryPositions,o.GeneralRunoffPositions,o.PrimaryRunoffPositions," +
        "ep.PoliticianKey AS ElectionsPoliticianKey,ep.RunningMateKey," +
        "ep.OrderOnBallot,ep.IsIncumbent,ep.IsWinner,p.AddOn,p.Address," +
        "p.BallotPediaWebAddress,p.GoFundMeWebAddress,p.CrowdpacWebAddress," +
        "p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip,p.DateOfBirth,p.EmailAddr AS Email," +
        "p.FacebookWebAddress,p.FlickrWebAddress,p.FName AS FirstName," +
        "p.GooglePlusWebAddress," +
        "p.LinkedInWebAddress,p.LName AS LastName,p.MName AS MiddleName," +
        "p.Nickname,p.PartyKey,p.Phone,p.PinterestWebAddress," +
        "p.PoliticianKey,p.RSSFeedWebAddress,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.TwitterWebAddress," +
        "p.VimeoWebAddress,p.WebAddr AS WebAddress,p.WebstagramWebAddress," +
        "p.WikipediaWebAddress,p.YouTubeWebAddress,pt.PartyCode,pt.PartyName," +
        "pt.PartyUrl,e.ElectionType,e.NationalPartyCode,pt.PartyName";

      var cmdText =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsOffices eo" +
          " INNER JOIN Elections e ON e.ElectionKey= eo.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
          " LEFT JOIN ElectionsPoliticians ep" + "" +
          "  ON ep.ElectionKey=eo.ElectionKey AND ep.OfficeKey=eo.OfficeKey" +
          " LEFT JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" + " WHERE {1}" +
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsOffices eo" +
          " INNER JOIN Elections e ON e.ElectionKey= eo.ElectionKey" +
          " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
          " LEFT JOIN ElectionsPoliticians ep" + "" +
          "  ON ep.ElectionKey=eo.ElectionKey AND ep.OfficeKey=eo.OfficeKey" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey = p.PartyKey" + " WHERE {1}",
          columnList, BuildWhereForGetElectionReportData(electionKey));

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static ElectionsTable GetElectionsOnDate(string stateCode,
      string countyCode, string localKey, DateTime electionDate,
      int commandTimeout = -1)
    {
      if (!IsNullOrWhiteSpace(localKey))
        countyCode = Empty;
      var cmdText = 
        SelectDisplayCommandText +
        " WHERE StateCode=@StateCode AND CountyCode=@CountyCode AND" +
        "  LocalKey=@LocalKey AND ElectionDate=@ElectionDate" +
        " ORDER BY ElectionOrder,ElectionDesc";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
      VoteDb.AddCommandParameter(cmd, "ElectionDate", electionDate);
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static ElectionsTable GetElectionsOnSameDate(string electionKey,
      int commandTimeout = -1)
    {
      var cmdText = SelectDisplayCommandText +
        " WHERE ElectionKey LIKE @Selector";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      // ignore party
      VoteDb.AddCommandParameter(cmd, "Selector",
        electionKey.Substring(0, 10) + "__" + electionKey.Substring(12));
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static IEnumerable<string> GetFutureElectionDescriptionsForSubstitutions(
      string stateCode,
      Substitutions.Options options)
    {
      const string cmdTemplate = "SELECT ElectionDesc FROM Elections {0}";

      var cmdText = Format(cmdTemplate,
        BuildWhereForFutureElectionsWithSubstitutionOptions(stateCode, options));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(row => row.ElectionDescription())
          .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);
      }
    }

    public static ElectionsTable GetFutureViewableDisplayDataByStateCode(
      string stateCode, int commandTimeout = -1)
    {
      var cmdText = SelectDisplayCommandText + " WHERE StateCode=@StateCode" +
        "  AND CountyCode='' AND LocalKey=''  AND IsViewable=1 " +
        "  AND ElectionDate >= @Today " +
        " ORDER BY ElectionDate ASC,ElectionOrder ASC,NationalPartyCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static ElectionsTable GetFutureViewablePrimaryDisplayDataByStateCode(
      string stateCode, int commandTimeout = -1)
    {
      var cmdText = SelectDisplayCommandText + " WHERE StateCode=@StateCode" +
        "  AND CountyCode='' AND LocalKey=''  AND IsViewable=1 " +
        "  AND ElectionType IN ('P','B') " +
        "  AND ElectionDate >= @Today " +
        " ORDER BY ElectionDate ASC,ElectionOrder ASC,NationalPartyCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      //VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      VoteDb.AddCommandParameter(cmd, "Today", new DateTime(2012, 1, 1));
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static string GetGeneralElectionDescriptionTemplate(DateTime date)
    {
      var electionKey = FormatElectionKey(date,
        ElectionTypeGeneralElection, Parties.NationalPartyAll, "IA");
      var desc = FormatElectionDescription(electionKey);
      // Replace " Iowa " with " {StateName} "
      desc = desc.Replace(" Iowa ", " {StateName} ");
      return desc;
    }

    public static IList<string> GetLatestElectionsByStateCode(string stateCode)
    {
      var cmdText = "SELECT ElectionKey,ElectionDate FROM Elections" +
        " WHERE ElectionDate<@Today" + "  AND StateCode=@StateCode" +
        " ORDER BY ElectionDate DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 4); // up to 4 primaries on 1 date
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var table = FillTable(cmd, ElectionsTable.ColumnSet.Date);
      if (table.Count == 0) return null;
      var date = table[0].ElectionDate;
      return table.OfType<ElectionsRow>()
        .Where(row => row.ElectionDate == date)
        .Select(row => row.ElectionKey)
        .ToList();
    }

    public static IList<string> GetLatestElectionsByStateCodeElectionType(string stateCode, 
      string electionType)
    {
      const string cmdText = "SELECT ElectionKey,ElectionDate FROM Elections" +
        " WHERE ElectionDate<@Today AND StateCode=@StateCode AND CountyCode='' AND LocalKey='' AND ElectionType=@ElectionType" +
        " ORDER BY ElectionDate DESC";
      //cmdText = VoteDb.InjectSqlLimit(cmdText, 4); // up to 4 primaries on 1 date
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionType", electionType);
      var table = FillTable(cmd, ElectionsTable.ColumnSet.Date);
      if (table.Count == 0) return null;
      var date = table[0].ElectionDate;
      return table.OfType<ElectionsRow>()
        .Where(row => row.ElectionDate == date)
        .Select(row => row.ElectionKey)
        .ToList();
    }

    public static string GetLatestViewableElectionKeyByStateCode(string stateCode,
      string defaultValue = null)
    {
      var cmdText = "SELECT ElectionKey" + " FROM Elections" +
        " WHERE StateCode=@StateCode" + "  AND CountyCode='' AND LocalKey=''" +
        "  AND IsViewable=1" + " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static DateTime? GetLatestPastGeneralElectionDateByStateCode(
      string stateCode)
    {
      var cmdText = "SELECT ElectionDate FROM Elections" +
        " WHERE StateCode=@StateCode  AND CountyCode='' AND LocalKey=''" +
        "  AND ElectionDate<CURDATE()  AND ElectionType='G'" +
        " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      return result as DateTime?;
    }

    public static string GetLatestViewableGeneralElectionKeyByStateCode(
      string stateCode, string defaultValue = null)
    {
      var cmdText = "SELECT ElectionKey FROM Elections" +
        " WHERE StateCode=@StateCode  AND CountyCode='' AND LocalKey=''" +
        "  AND IsViewable=1  AND ElectionType='G'" +
        " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static string GetLatestViewableOffYearElectionKeyByStateCode(
      string stateCode, string defaultValue = null)
    {
      var cmdText = "SELECT ElectionKey" + " FROM Elections" +
        " WHERE StateCode=@StateCode" + "  AND CountyCode='' AND LocalKey=''" +
        "  AND IsViewable=1" + "  AND ElectionType='O'" +
        " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static string GetLatestViewablePrimaryElectionKeyByStateCode(
      string stateCode, string defaultValue = null)
    {
      var cmdText = "SELECT ElectionKey" + " FROM Elections" +
        " WHERE StateCode=@StateCode" + "  AND CountyCode='' AND LocalKey=''" +
        "  AND IsViewable=1" +
        "  AND (ElectionType='P' OR ElectionType='B')" +
        " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static string GetLatestViewableSpecialElectionKeyByStateCode(
      string stateCode, string defaultValue = null)
    {
      var cmdText = "SELECT ElectionKey" + " FROM Elections" +
        " WHERE StateCode=@StateCode" + "  AND CountyCode='' AND LocalKey=''" +
        "  AND IsViewable=1" + "  AND ElectionType='S'" +
        " ORDER BY ElectionKey DESC";
      cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      var result = VoteDb.ExecuteScalar(cmd);
      if (result == null || result == DBNull.Value) return defaultValue;
      return result as string;
    }

    public static string GetNextViewableElectionForParty(string partyKey, string stateCode)
    {
      var nationalPartyCode = Parties.GetPartyCode(partyKey);
      const string cmdText = "SELECT  ElectionKey" +
        " FROM Elections " +
        " WHERE Elections.StateCode=@StateCode" +
        "  AND (NOT ElectionType IN ('B','P') OR " +
        " NationalPartyCode IN (@NationalPartyCode,'X'))" +
        " AND ElectionType != 'A'" +
        " AND CountyCode = '' AND LocalKey=''  AND IsViewable = 1 AND ElectionDate>=CURDATE()" +
        " ORDER BY ElectionDate DESC" +
        " LIMIT 1";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "NationalPartyCode", nationalPartyCode);
      return (VoteDb.ExecuteScalar(cmd) as string).SafeString();
    }

    public static bool GetNonPartisanPrimaryExists(string stateCode,
      DateTime electionDate, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Elections" +
        " WHERE StateCode=@StateCode AND CountyCode='' AND LocalKey=''" +
        " AND ElectionType='P' AND ElectionDate=@ElectionDate" +
        " AND NationalPartyCode IN ('A','X')";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionDate", electionDate);
      return (long) VoteDb.ExecuteScalar(cmd) != 0;
    }

    public static DataTable GetOneElectionOffice(string electionKey,
      string officeKey, string politicianKey = null, int commandTimeout = -1)
    {
      const string columnList =
        "ep.RunningMateKey,ep.OrderOnBallot,ep.IsIncumbent,p.AddOn,p.Address," +
        "p.CityStateZip,p.EmailAddr AS Email,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LName AS LastName," +
        "p.Nickname,p.Phone,p.PoliticianKey,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.WebAddr AS WebAddress," +
        "o.IsRunningMateOffice,o.IsPrimaryRunningMateOffice,pt.PartyCode,pt.PartyName,pt.PartyUrl";

      var cmdText =
        Format(
          "SELECT {0}, 0 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey {1}" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=@OfficeKey" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey" +
          " UNION ALL SELECT {0}, 1 AS IsRunningMate FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.RunningMateKey {2}" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=@OfficeKey" +
          " WHERE ep.ElectionKey=@ElectionKey AND ep.OfficeKey=@OfficeKey",
          columnList,
          politicianKey == null
            ? Empty
            : "AND (p.PoliticianKey=@PoliticianKey OR ep.RunningMateKey=@PoliticianKey)",
          politicianKey == null
            ? Empty
            : "AND p.PoliticianKey=@PoliticianKey");

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      if (politicianKey != null)
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static ElectionsTable GetPartyPrimariesWithOffices(string stateCode,
      DateTime electionDate, int commandTimeout = -1)
    {
      const string cmdText = "SELECT e.ElectionKey,e.ElectionDate,e.ElectionDesc,e.ElectionOrder" +
        " FROM Elections e INNER JOIN ElectionsOffices eo ON e.ElectionKey=eo.ElectionKey" +
        " WHERE e.StateCode=@StateCode AND e.CountyCode='' AND LocalKey=''" +
        " AND e.ElectionType='P' AND e.ElectionDate=@ElectionDate" +
        //" AND e.NationalPartyCode NOT IN ('A','X')" +
        " AND e.NationalPartyCode <> 'A'" +
        " AND eo.OfficeKey<>'USPresident'" +
        " GROUP BY e.ElectionKey ORDER BY ElectionOrder ASC,ElectionDesc ASC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionDate", electionDate);
      return FillTable(cmd, ElectionsTable.ColumnSet.Control);
    }

    public static bool GetPartyPrimaryExists(string stateCode,
      DateTime electionDate, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Elections" +
        " WHERE StateCode=@StateCode AND CountyCode='' AND LocalKey=''" +
        " AND ElectionType='P' AND ElectionDate=@ElectionDate" +
        " AND NationalPartyCode NOT IN ('A','X')";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "ElectionDate", electionDate);
      return (long) VoteDb.ExecuteScalar(cmd) != 0;
    }

    public static ElectionsTable GetPastViewableDisplayDataByStateCode(
      string stateCode, int commandTimeout = -1)
    {
      var cmdText = SelectDisplayCommandText + " WHERE StateCode=@StateCode" +
        "  AND CountyCode='' AND LocalKey=''  AND IsViewable=1 " +
        "  AND ElectionDate < @Today " +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC,NationalPartyCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "Today", DateTime.Today);
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static DateTime GetElectionDateForRunoffElection(string electionKey)
    {
      var result = VotePage.DefaultDbDate;
      var cmdText = "SELECT ElectionDate FROM Elections WHERE StateCode=@StateCode AND " +
        (IsStateElection(electionKey)
          ? " CountyCode='' AND LocalKey=''"
          : IsCountyElection(electionKey)
            ? "CountyCode=@CountyCode"
            : "LocalKey=@LocalKey") +
        " AND ElectionYYYYMMDD<@YYYYMMDD AND ElectionType=@ElectionType" +
        " AND NationalPartyCode=@NationalPartyCode" +
        " ORDER BY ElectionYYYYMMDD DESC LIMIT 1";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", GetStateCodeFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "CountyCode", GetCountyCodeFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "LocalKey", GetLocalKeyFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "YYYYMMDD", GetYyyyMmDdFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "ElectionType",
        GetElectionTypeForRunoff(GetElectionTypeFromKey(electionKey)));
      VoteDb.AddCommandParameter(cmd, "NationalPartyCode",
        GetNationalPartyCodeFromKey(electionKey));
      var date = VoteDb.ExecuteScalar(cmd);
      if (date != null) result = (DateTime) date;
      return result;
    }

    public static DateTime GetPrimaryDateForGeneralElection(string electionKey)
    {
      var result = VotePage.DefaultDbDate;
      var cmdText = "SELECT ElectionDate FROM Elections" + 
        " WHERE StateCode=@StateCode AND" + 
        (IsStateElection(electionKey)
          ? " CountyCode='' AND LocalKey=''"
          : IsCountyElection(electionKey)
            ? " CountyCode=@CountyCode"
            : " LocalKey=@LocalKey") +
        " AND ElectionYYYYMMDD<@YYYYMMDD AND ElectionType IN ('B','P')" +
        " ORDER BY ElectionYYYYMMDD DESC LIMIT 1";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", GetStateCodeFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "CountyCode", GetCountyCodeFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "LocalKey", GetLocalKeyFromKey(electionKey));
      VoteDb.AddCommandParameter(cmd, "YYYYMMDD", GetYyyyMmDdFromKey(electionKey));
      var date = VoteDb.ExecuteScalar(cmd);
      if (date != null) result = (DateTime) date;
      return result;
    }

    public static IEnumerable<string> GetPrimaryElectionDescriptionsForSubstitutions(
      string stateCode,
      Substitutions.Options options)
    {
      const string cmdTemplate = "SELECT ElectionDesc FROM Elections {0}";

      var cmdText = Format(cmdTemplate,
        BuildWhereForPrimariesWithSubstitutionOptions(stateCode, options));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(row => row.ElectionDescription())
          .OrderBy(s => s, StringComparer.OrdinalIgnoreCase);
      }
    }

    public static IEnumerable<SimpleListItem> GetElectionsForSampleBallotsSubstitutions(
      string stateCode, Substitutions.Options options)
    {
      const string cmdTemplate = "SELECT ElectionDesc,ElectionKey FROM Elections {0}";

      // force viewable and future
      options &= ~Substitutions.Options.NotViewable;
      options |= Substitutions.Options.Viewable;

      // force future
      //options &= ~Substitutions.Options.Past;
      //options |= Substitutions.Options.Future;

      var cmdText = Format(cmdTemplate,
        BuildWhereForPrimariesWithSubstitutionOptions(stateCode, options, true));
      var cmd = VoteDb.GetCommand(cmdText, -1);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(row => new SimpleListItem
          {
            Text = row.ElectionDescription(),
            Value = row.ElectionKey()
          })
          .OrderBy(i => i.Text, StringComparer.OrdinalIgnoreCase);
      }
    }

    public static List<string> GetStateElectionKeysForDate(string stateCode, DateTime date)
    {
      var cmdText = "SELECT ElectionKey FROM Elections WHERE IsViewable=1 AND" +
        $" ElectionKey LIKE '{stateCode}{date:yyyyMMdd}%'";
      var cmd = VoteDb.GetCommand(cmdText);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(row => Elections.GetStateElectionKeyFromKey(row.ElectionKey()))
          .Distinct()
          .ToList();
      }
    }

    public static List<string> GetStateGeneralElectionsByDate(DateTime electionDate,
      int commandTimeout = -1)
    {
      const string cmdText = "SELECT ElectionKey FROM Elections" +
        " WHERE ElectionDate=@ElectionDate AND ElectionType='G' AND NationalPartyCode='A'" +
        " AND CountyCode='' AND LocalKey=''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionDate", electionDate);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Cast<DataRow>()
          .Select(row => row.ElectionKey())
          .ToList();
      }
    }

    public static IList<IGrouping<DateTime, ElectionsRow>> GetVirtualElectionControlData(
      string stateCode,
      string countyCode, string localKey, int commandTimeout = -1)
    {
      var cmdText =
        "SELECT ElectionKey,ElectionDate,ElectionDesc,ElectionOrder FROM Elections" +
        " WHERE StateCode=@StateCode AND" +
        (IsNullOrWhiteSpace(localKey)
          ? IsNullOrWhiteSpace(countyCode) 
            // no countyCode or localKey -- state only
            ? " CountyCode='' AND LocalKey=''"
            // has countyCode -- state and county
            : " CountyCode IN ('',@CountyCode) AND LocalKey=''"
          // has localKey -- state and local
          : " CountyCode='' AND LocalKey IN ('',@LocalKey)") +
        " ORDER BY ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalKey", localKey);
      // Group results by StateElectionKey
      // All groups should contain either 1 or two rows.
      // If 2, use second.
      // If 1, adjust key and use it
      return
        FillTable(cmd, ElectionsTable.ColumnSet.Control)
          .GroupBy(row => GetStateElectionKeyFromKey(row.ElectionKey))
          .Select(g =>
          {
            if (g.Count() == 1)
            {
              var row = g.First();
              if (IsStateElection(row.ElectionKey))
                row.ElectionKey += IsNullOrEmpty(localKey)
                  ? countyCode
                  : localKey;
              return row;
            }
            return g.ElementAt(1);
          })
          .OrderByDescending(row => row.ElectionDate)
          .ThenBy(row => row.ElectionOrder)
          .ThenBy(row => row.ElectionDesc)
          .GroupBy(row => row.ElectionDate)
          .ToList();
    }

    public static DataTable GetWinnersData(string electionKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "p.FName AS FirstName,p.MName as MiddleName,p.LName AS LastName,p.Nickname," +
        "p.PoliticianKey,p.Suffix,o.DistrictCode," + 
        //"o.DistrictCodeAlpha," +
        "o.Incumbents," +
        "o.OfficeKey,o.OfficeLevel,o.OfficeLine1,o.OfficeLine2,o.ElectionPositions," +
        "o.OfficeOrderWithinLevel,o.GeneralRunoffPositions,pt.PartyCode";

      var cmdText =
        Format(
          "SELECT {0}, ep.IsWinner,ep.AdvanceToRunoff, 0 AS IsIncumbentRow FROM ElectionsPoliticians ep" +
          " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" +
          " WHERE ep.ElectionKey=@ElectionKey" +
          " UNION ALL SELECT {0}, 0 AS IsWinner, 0 AS AdvanceToRunoff, 1 AS IsIncumbentRow" +
          " FROM ElectionsOffices eo" +
          " INNER JOIN OfficesOfficials oo ON oo.OfficeKey=eo.OfficeKey" +
          " INNER JOIN Politicians p ON p.PoliticianKey=oo.PoliticianKey" +
          " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
          " INNER JOIN Offices o ON o.OfficeKey=eo.OfficeKey" +
          " WHERE eo.ElectionKey=@ElectionKey" +
          " ORDER BY OfficeLevel,DistrictCode,OfficeOrderWithinLevel," +
          //" DistrictCodeAlpha," + 
          "OfficeLine1,OfficeLine2", columnList);

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static DataTable GetPrimaryWinnersData(string electionKey,
      int commandTimeout = -1)
    {
      const string columnList =
        "ep.IsWinner,ep.AdvanceToRunoff," +
        "p.FName AS FirstName,p.MName as MiddleName,p.LName AS LastName,p.Nickname," +
        "p.PoliticianKey,p.Suffix,o.DistrictCode," +
        //"o.DistrictCodeAlpha," + 
        "o.PrimaryPositions," +
        "o.OfficeKey,o.OfficeLevel,o.OfficeLine1,o.OfficeLine2," +
        "o.OfficeOrderWithinLevel,o.PrimaryRunoffPositions,pt.PartyCode";

      var cmdText =
        $"SELECT {columnList} FROM ElectionsPoliticians ep" +
        " INNER JOIN Politicians p ON p.PoliticianKey=ep.PoliticianKey" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
        " INNER JOIN Offices o ON o.OfficeKey=ep.OfficeKey" + " WHERE ep.ElectionKey=@ElectionKey" +
        " ORDER BY OfficeLevel,DistrictCode,OfficeOrderWithinLevel," +
        //" DistrictCodeAlpha," + 
        "OfficeLine1,OfficeLine2";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static ElectionsTable GetViewableDisplayDataByStateCode(string stateCode, int commandTimeout = -1)
    {
      const string cmdText = 
        "SELECT ElectionKey,StateCode,CountyCode,LocalKey,ElectionDate,ElectionType," +
        "PartyCode,ElectionDesc,IsViewable FROM Elections" +
        " WHERE StateCode=@StateCode AND CountyCode=''" +
        "  AND LocalKey='' AND IsViewable=1" +
        " ORDER BY ElectionDate DESC,ElectionOrder ASC";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      return FillTable(cmd, ElectionsTable.ColumnSet.Display);
    }

    public static DataTable GetWinnersIdentified(string stateCode,
      DateTime lastGeneralElectionDate)
    {
      const string cmdText = "SELECT ElectionDesc,IsWinnersIdentified FROM Elections" +
        " WHERE StateCode=@StateCode AND CountyCode = '' AND LocalKey=''" +
        "  AND ElectionDate>=@LastGeneralElectionDate AND ElectionDate<CURDATE() AND IsViewable=1";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "LastGeneralElectionDate", lastGeneralElectionDate);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    //public static bool IsRelevantElection(string electionKey, string congress,
    //  string stateSenate, string stateHouse, string county, string district, string place,
    //  string elementary, string secondary, string unified, string cityCouncil,
    //  string countySupervisors, string schoolDistrictDistrict)
    //{
    //  // Generals are always relevant
    //  if (GetElectionTypeFromKey(electionKey) == ElectionTypeGeneralElection)
    //    return true;

    //  // for all others, check for contents
    //  return HasContestsOrBallotMeasures(electionKey, congress, stateSenate, stateHouse,
    //    county, district, place, elementary, secondary, unified, cityCouncil,
    //    countySupervisors, schoolDistrictDistrict);
    //}

    public static bool LocalElectionsExistForCountyElection(string countyElectionKey)
    {
      var stateElectionKey = GetStateElectionKeyFromKey(countyElectionKey);
      var localElectionKeysClause =
        LocalDistricts.GetLocalKeysForCounty(GetStateCodeFromKey(countyElectionKey),
          GetCountyCodeFromKey(countyElectionKey))
          .Select(l => stateElectionKey + l)
          .SqlIn("ElectionKey");
      var cmdText = $"SELECT COUNT(*) FROM Elections WHERE {localElectionKeysClause}";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    public static int UpdateElectionDescForElectionFamily(string newValue, string stateElectionKey)
    {
      if (!IsStateElection(stateElectionKey)) return 0;
      stateElectionKey = stateElectionKey + "%";
      const string cmdText =
        "UPDATE Elections SET ElectionDesc=@newValue WHERE ElectionKey LIKE @ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", stateElectionKey);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    public static void UpdateElectionsAndOffices(ElectionsTable electionsTable,
      ElectionsOfficesTable officesTable, ElectionsPoliticiansTable politiciansTable,
      int commandTimeout = -1)
    {
      DbConnection cn = null;
      DbTransaction transaction = null;

      try
      {
        cn = VoteDb.GetOpenConnection();
        transaction = cn.BeginTransaction();

        var cmdText = GetSelectCommandText(ElectionsTable.ColumnSet.All);
        var cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
        var adapter = VoteDb.GetDataAdapter(cmd);
        adapter.ContinueUpdateOnError = false;
        var builder = VoteDb.GetCommandBuilder(adapter);
        builder.ConflictOption = ConflictOption.CompareAllSearchableValues;
        adapter.Update(electionsTable);

        // create the defaults
        foreach (var row in electionsTable)
          ElectionsDefaults.CreateEmptyRow(row.ElectionKey);

        if (officesTable != null)
        {
          cmdText =
            ElectionsOffices.GetSelectCommandText(
              ElectionsOfficesTable.ColumnSet.All);
          cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
          adapter = VoteDb.GetDataAdapter(cmd);
          adapter.ContinueUpdateOnError = false;
          builder = VoteDb.GetCommandBuilder(adapter);
          builder.ConflictOption = ConflictOption.CompareAllSearchableValues;
          adapter.Update(officesTable);
        }

        if (politiciansTable != null)
        {
          cmdText =
            ElectionsPoliticians.GetSelectCommandText(
              ElectionsPoliticiansTable.ColumnSet.All);
          cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
          adapter = VoteDb.GetDataAdapter(cmd);
          adapter.ContinueUpdateOnError = false;
          builder = VoteDb.GetCommandBuilder(adapter);
          builder.ConflictOption = ConflictOption.CompareAllSearchableValues;
          adapter.Update(politiciansTable);
        }

        transaction.Commit();
        cn.Close();
      }
      catch
      {
        transaction?.Rollback();
        cn?.Close();
        throw;
      }
    }

    public static int UpdateIsViewableForElectionFamily(bool newValue, string stateElectionKey)
    {
      //if (!IsStateElection(stateElectionKey)) return 0;
      stateElectionKey = stateElectionKey + "%";
      const string cmdText =
        "UPDATE Elections SET IsViewable=@newValue WHERE ElectionKey LIKE @ElectionKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", stateElectionKey);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }

    // ReSharper restore ReturnTypeCanBeEnumerable.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}