using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using WebService = Vote.Admin.WebService;

namespace DB.Vote
{
  public partial class TempEmailBatches
  {
    private static int BuildContactsEmailBatch(int batchId, string cmdText,
      bool useBothContacts)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      // check for optional columns
      var countyCodeExists = table.Columns.Contains("CountyCode");
      var localCodeExists = table.Columns.Contains("LocalCode");
      var electionKeyExists = table.Columns.Contains("ElectionKey");
      var electionKeyToIncludeExists = table.Columns.Contains("ElectionKeyToInclude");

      var tempEmailTable = new TempEmailTable();
      foreach (var row in table.Rows.Cast<DataRow>())
      {
        var hadMain = false;
        if (Validation.IsValidEmailAddress(row.ContactEmail()))
        {
          hadMain = true;
          var email = row.ContactEmail();
          var contact = row.Contact();
          var title = row.ContactTitle();
          var phone = row.Phone();
          // We don't have first name, last name so we have to concoct a
          // SortName for the name we have
          var sortName = ConcoctSortName(row.Contact());
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (contact != null && contact.Length > TempEmail.ContactMaxLength)
            contact = contact.Substring(0, TempEmail.ContactMaxLength);
          if (title != null && title.Length > TempEmail.TitleMaxLength)
            title = title.Substring(0, TempEmail.TitleMaxLength);
          if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
            phone = phone.Substring(0, TempEmail.PhoneMaxLength);
          if (sortName != null && sortName.Length > TempEmail.SortNameMaxLength)
            sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
          tempEmailTable.AddRow(batchId, email, contact,
            title, phone, row.StateCode(),
            countyCodeExists ? row.CountyCode() : null,
            localCodeExists ? row.LocalCode() : null, null,
            electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null, null, null, 0, 0,
            sortName);
        }
        if ((useBothContacts || !hadMain) &&
          Validation.IsValidEmailAddress(row.AltEmail()))
        {
          var email = row.AltEmail();
          var contact = row.AltContact();
          var title = row.AltContactTitle();
          var phone = row.AltPhone();
          // We don't have first name, last name so we have to concoct a
          // SortName for the name we have
          var sortName = ConcoctSortName(row.AltContact());
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (contact != null && contact.Length > TempEmail.ContactMaxLength)
            contact = contact.Substring(0, TempEmail.ContactMaxLength);
          if (title != null && title.Length > TempEmail.TitleMaxLength)
            title = title.Substring(0, TempEmail.TitleMaxLength);
          if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
            phone = phone.Substring(0, TempEmail.PhoneMaxLength);
          if (sortName != null && sortName.Length > TempEmail.SortNameMaxLength)
            sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
          tempEmailTable.AddRow(batchId, email, contact,
            title, phone, row.StateCode(),
            countyCodeExists ? row.CountyCode() : null,
            localCodeExists ? row.LocalCode() : null, null,
            electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null, null, null,
            0, 0, sortName);
        }
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !string.IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildCandidatesEmailBatch(int batchId, string cmdText,
      string[] politicianEmailsToUse)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      // check for optional columns
      var countyCodeExists = table.Columns.Contains("CountyCode");
      var localCodeExists = table.Columns.Contains("LocalCode");
      var electionKeyExists = table.Columns.Contains("ElectionKey");
      var electionKeyToIncludeExists = table.Columns.Contains("ElectionKeyToInclude");
      var officeKeyExists = table.Columns.Contains("OfficeKey");

      var tempEmailTable = new TempEmailTable();
      foreach (var row in table.Rows.Cast<DataRow>())
      {
        var contact = Politicians.FormatName(row);
        var officeName = officeKeyExists ? Offices.FormatOfficeName(row) : null;
        var phone = row.PublicPhone();
        foreach (var emailKey in politicianEmailsToUse)
        {
          var email = row[emailKey] as string;
          var sortName = row.LastName() + " " + row.Suffix() + " " + row.LastName() +
            " " + row.MiddleName() + " " + row.Nickname();
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (contact != null && contact.Length > TempEmail.ContactMaxLength)
            contact = contact.Substring(0, TempEmail.ContactMaxLength);
          if (officeName != null && officeName.Length > TempEmail.TitleMaxLength)
            officeName = officeName.Substring(0, TempEmail.TitleMaxLength);
          if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
            phone = phone.Substring(0, TempEmail.PhoneMaxLength);
          if (sortName != null && sortName.Length > TempEmail.SortNameMaxLength)
            sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
          if (Validation.IsValidEmailAddress(email))
            tempEmailTable.AddRow(batchId, 
              email, contact, officeName, phone,
              row.StateCode(), countyCodeExists ? row.CountyCode() : null,
              localCodeExists ? row.LocalCode() : null, row.PoliticianKey(),
              electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, 
              officeKeyExists ? row.OfficeKey() : null, row.PartyKey(), 
              null, 0, 0, sortName);
        }
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !string.IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildDonorsEmailBatch(int batchId, string cmdText,
      DateTime fromDate, DateTime toDate)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "fromDate", fromDate);
      VoteDb.AddCommandParameter(cmd, "toDate", toDate);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var tempEmailTable = new TempEmailTable();

      // group by email and take only the first
      var grouped = table.Rows.Cast<DataRow>()
        .GroupBy(r => r.Email()
          .ToLowerInvariant())
          .Select(g => g.First());

      foreach (var row in grouped)
      {
        var email = row.Email();
        var contact = row.FullName();
        var phone = row.Phone();
        var sortName = row.LastName() + " " + row.FirstName();
        if (email != null && email.Length > TempEmail.EmailMaxLength)
          email = email.Substring(0, TempEmail.EmailMaxLength);
        if (contact != null && contact.Length > TempEmail.ContactMaxLength)
          contact = contact.Substring(0, TempEmail.ContactMaxLength);
        if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
          phone = phone.Substring(0, TempEmail.PhoneMaxLength);
        if (sortName.Length > TempEmail.SortNameMaxLength)
          sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
        if (Validation.IsValidEmailAddress(email))
          tempEmailTable.AddRow(batchId, email, contact, string.Empty, phone,
            row.StateCode(), row.CountyCode(), null, null, null, null, null, null, null,
            row.VisitorId(), row.Id(), sortName);
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !string.IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildPartiesEmailBatch(int batchId, string cmdText)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      // check for optional columns
      var electionKeyExists = table.Columns.Contains("ElectionKey");
      var electionKeyToIncludeExists = table.Columns.Contains("ElectionKeyToInclude");

      var tempEmailTable = new TempEmailTable();
      foreach (var row in table.Rows.Cast<DataRow>())
      {
        var email = row.PartyEmail();
        var contact = row.PartyContactFName() + " " + row.PartyContactLName();
        var title = row.PartyContactTitle();
        var phone = row.PartyContactPhone();
        var sortName = row.PartyContactLName() + " " + row.PartyContactFName();
        if (email != null && email.Length > TempEmail.EmailMaxLength)
          email = email.Substring(0, TempEmail.EmailMaxLength);
        if (contact != null && contact.Length > TempEmail.ContactMaxLength)
          contact = contact.Substring(0, TempEmail.ContactMaxLength);
        if (title != null && title.Length > TempEmail.TitleMaxLength)
          title = title.Substring(0, TempEmail.TitleMaxLength);
        if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
          phone = phone.Substring(0, TempEmail.PhoneMaxLength);
        if (sortName != null && sortName.Length > TempEmail.SortNameMaxLength)
          sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
        if (Validation.IsValidEmailAddress(email))
          tempEmailTable.AddRow(batchId, email, contact, title,
            phone, row.StateCode(), null, null, null,
            electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null, row.PartyKey(),
            email, 0, 0, sortName);
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !string.IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildVisitorsEmailBatch(int batchId, string cmdText,
      DateTime fromDate, DateTime toDate)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      VoteDb.AddCommandParameter(cmd, "fromDate", fromDate);
      VoteDb.AddCommandParameter(cmd, "toDate", toDate);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var tempEmailTable = new TempEmailTable();
      foreach (var row in table.Rows.Cast<DataRow>())
      {
        var email = row.Email();
        var contact = row.FirstName() + " " + row.LastName();
        var phone = row.Phone();
        var sortName = row.LastName() + " " + row.FirstName();
        if (email != null && email.Length > TempEmail.EmailMaxLength)
          email = email.Substring(0, TempEmail.EmailMaxLength);
        if (contact.Length > TempEmail.ContactMaxLength)
          contact = contact.Substring(0, TempEmail.ContactMaxLength);
        if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
          phone = phone.Substring(0, TempEmail.PhoneMaxLength);
        if (sortName.Length > TempEmail.SortNameMaxLength)
          sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
        if (Validation.IsValidEmailAddress(email))
          tempEmailTable.AddRow(batchId, email, contact, string.Empty, phone,
            row.StateCode(), row.CountyCode(), null, null, null, null, null, null, null,
            row.Id(), 0, sortName);
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !string.IsNullOrWhiteSpace(row.RowError));
    }

    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global
    // ReSharper disable UnusedParameter.Global
    // ReSharper disable UnusedParameter.Global

    #endregion ReSharper disable

    public static void CleanUpTempEmailBatches()
    {
      string message;

      try
      {
        VotePage.LogInfo("CleanUpTempEmailBatches", "Started");

        // Get the number of days to retain, default to 2
        var daysString =
          ConfigurationManager.AppSettings["VoteTempEmailBatchesRetentionDays"];
        int days;
        if (!int.TryParse(daysString, out days)) days = 2;

        // Convert to a past DateTime
        var expiration = DateTime.UtcNow - new TimeSpan(days, 0, 0, 0);

        // Do it
        var deletedBatches = 0;
        var deletedRows = 0;
        foreach (var expiredBatch in GetExpiredBatches(expiration, 0))
        {
          deletedRows += TempEmail.DeleteByBatchId(expiredBatch.Id, 0);
          deletedBatches += DeleteById(expiredBatch.Id, 0);
        }

        message =
          $"{deletedBatches} TempEmailBatches deleted, {deletedRows} TempEmail rows deleted";
      }
      catch (Exception ex)
      {
        VotePage.LogException("CleanUpTempEmailBatches", ex);
        message = $"Exception: {ex.Message} [see exception log for details]";
      }

      VotePage.LogInfo("CleanUpTempEmailBatches", message);
    }

    public static TempEmailBatchesTable GetExpiredBatches(DateTime expiration,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT Id,CreationTime,UserName FROM TempEmailBatches WHERE CreationTime<@Expiration";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "Expiration", expiration);
      return FillTable(cmd, TempEmailBatchesTable.ColumnSet.All);
    }

    public static string ConcoctSortName(string fullName)
    {
      fullName = fullName.StripRedundantWhiteSpace();
      if (fullName == string.Empty) return string.Empty;
      var names = fullName.ToLowerInvariant().Split(new []{' '}, 
        StringSplitOptions.RemoveEmptyEntries);
      if (names.Length == 1) return names[0];
      var lastNameIndex = names.Length - 1;
      if (Validation.IsValidNameSuffix(names[lastNameIndex])) lastNameIndex--;
      else if (fullName.Contains(",")) 
      {
        // assume it's lastname, firstname since we've ruled out a suffix
        lastNameIndex = 0;
      }
      return string.Join(" ", names.Skip(lastNameIndex)) + " " +
        string.Join(" ", names.Take(lastNameIndex));
    }

    public static int CreateAllContactsBatch(int batchId, string[] stateCodes,
      bool useBothContacts, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build States SELECT
      var statesCmdText =
        "SELECT StateCode,'' AS CountyCode,'' AS LocalCode,Contact,ContactEmail,ContactTitle,Phone," +
          "AltContact,AltEMail,AltContactTitle,AltPhone FROM States WHERE {0}";
      var stateWhereClause = "IsState=1";
      if (!allStates) stateWhereClause = "StateCode IN ('" + string.Join("','", stateCodes) + "')";
      statesCmdText = string.Format(statesCmdText, stateWhereClause);

      // build Counties SELECT
      var countiesWhereClause = string.Empty;
      if (!allStates)
        countiesWhereClause = "WHERE StateCode IN ('" + string.Join("','", stateCodes) + "')";
      var countiesCmdText =
        "SELECT StateCode,CountyCode,'' AS LocalCode,Contact,ContactEmail,ContactTitle,Phone," +
          "AltContact,AltEMail,AltContactTitle,AltPhone FROM Counties {0}";
      countiesCmdText = string.Format(countiesCmdText, countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = string.Empty;
      if (!allStates)
        localsWhereClause = "WHERE StateCode IN ('" + string.Join("','", stateCodes) + "')";
      var localsCmdText =
        "SELECT StateCode,CountyCode,LocalCode,Contact,ContactEmail,ContactTitle,Phone," +
          "AltContact,AltEMail,AltContactTitle,AltPhone FROM LocalDistricts {0}";
      localsCmdText = string.Format(localsCmdText, localsWhereClause);

      var cmdText = statesCmdText + " UNION " + countiesCmdText + " UNION " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateAllContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string stateCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalCode,s.Contact,s.ContactEmail,s.ContactTitle," +
          "s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle,s.AltPhone,e.ElectionKey,e.ElectionKeyToInclude" +
          " FROM States s INNER JOIN Elections e ON e.ElectionKey=" +
          " (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var statesCmdText = string.Format(stateCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC",
        "'" + string.Join("','", stateCodes) + "'");

      // build Counties SELECT
      var countiesWhereClause = string.Empty;
      if (!allStates)
        countiesWhereClause = "AND c.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalCode,c.Contact,c.ContactEmail,c.ContactTitle," +
          "c.Phone,c.AltContact,c.AltEMail,c.AltContactTitle,c.AltPhone,e.ElectionKey,e.ElectionKeyToInclude" +
          " FROM Counties c INNER JOIN Elections e" +
          " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=c.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var countiesCmdText = string.Format(countiesCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = string.Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,l.Contact,l.ContactEmail," +
          "l.ContactTitle,l.Phone,l.AltContact,l.AltEMail,l.AltContactTitle," +
          "l.AltPhone,e.ElectionKey,e.ElectionKeyToInclude FROM LocalDistricts l INNER JOIN Elections e" +
          " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=l.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var localsCmdText = string.Format(localsCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", localsWhereClause);

      var cmdText = statesCmdText + " UNION " + countiesCmdText + " UNION " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateAllContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string statesCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalCode,s.Contact,s.ContactEmail," +
          "s.ContactTitle,s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle," +
          "s.AltPhone FROM States s WHERE s.StateCode IN ({0}) AND" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=s.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({1}) AND" +
          " e.ElectionDate{2}UTC_DATE() {3} LIMIT 1) IS NULL";

      // Substitutions:
      // {0} = stateCodes (single-quoted, comma separated)
      // {1} = electionTypes (single-quoted, comma separated)
      // {2} = useFutureElections ? ">=" : "<"
      // {3} = viewableOnly ? "AND IsViewable=1" : string.Empty

      var statesCmdText = string.Format(statesCmdTemplate,
        "'" + string.Join("','", stateCodes) + "'",
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty);

      // build Counties SELECT
      var countiesWhereClause = string.Empty;
      if (!allStates)
        countiesWhereClause = "AND c.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalCode,c.Contact," +
          "c.ContactEmail,c.ContactTitle,c.Phone,c.AltContact,c.AltEMail," +
          "c.AltContactTitle,c.AltPhone FROM Counties c WHERE" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=c.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({0}) AND" +
          " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3] = WHERE clause

      var countiesCmdText = string.Format(countiesCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty, countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = string.Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,l.Contact," +
          "l.ContactEmail,l.ContactTitle,l.Phone,l.AltContact,l.AltEMail," +
          "l.AltContactTitle,l.AltPhone FROM LocalDistricts l WHERE" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=l.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({0}) AND" +
          " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3] = WHERE clause

      var localsCmdText = string.Format(localsCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty, localsWhereClause);

      var cmdText = statesCmdText + " UNION " + countiesCmdText + " UNION " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateStatesContactsBatch(int batchId, string[] stateCodes,
      bool useBothContacts, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      var all = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      var cmdText =
        "SELECT StateCode,Contact,ContactEmail,ContactTitle,Phone,AltContact," +
          "AltEMail,AltContactTitle,AltPhone FROM States WHERE {0}";

      // build WHERE clause
      var whereClause = "IsState=1";
      if (!all) whereClause = "StateCode IN ('" + string.Join("','", stateCodes) + "')";

      cmdText = string.Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateStatesContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate =
        "SELECT s.StateCode,s.Contact,s.ContactEmail,s.ContactTitle,s.Phone," +
          "s.AltContact,s.AltEMail,s.AltContactTitle,s.AltPhone,e.ElectionKey,e.ElectionKeyToInclude" +
          " FROM States s INNER JOIN Elections e ON e.ElectionKey=" +
          " (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC",
        "'" + string.Join("','", stateCodes) + "'");

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateStatesContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate =
        "SELECT s.StateCode,s.Contact,s.ContactEmail," +
          "s.ContactTitle,s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle," +
          "s.AltPhone FROM States s WHERE s.StateCode IN ({0}) AND" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=s.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({1}) AND" +
          " e.ElectionDate{2}UTC_DATE() {3} LIMIT 1) IS NULL";

      // Substitutions:
      // {0} = stateCodes (single-quoted, comma separated)
      // {1} = electionTypes (single-quoted, comma separated)
      // {2} = useFutureElections ? ">=" : "<"
      // {3} = viewableOnly ? "AND IsViewable=1" : string.Empty

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", stateCodes) + "'",
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateCountiesContactsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, bool useBothContacts, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND CountyCode IN ('" +
              string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "WHERE StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      var cmdText =
        "SELECT StateCode,CountyCode,Contact,ContactEmail,ContactTitle,Phone," +
          "AltContact,AltEMail,AltContactTitle,AltPhone FROM Counties {0}";

      cmdText = string.Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateCountiesContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, bool useBothContacts,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" +
              string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND c.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT c.StateCode,c.CountyCode,c.Contact,c.ContactEmail," +
          "c.ContactTitle,c.Phone,c.AltContact,c.AltEMail,c.AltContactTitle," +
          "c.AltPhone,e.ElectionKey,e.ElectionKeyToInclude FROM Counties c INNER JOIN Elections e" +
          " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=c.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateCountiesContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, bool useBothContacts,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" +
              string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND c.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT c.StateCode,c.CountyCode,c.Contact," +
          "c.ContactEmail,c.ContactTitle,c.Phone,c.AltContact,c.AltEMail," +
          "c.AltContactTitle,c.AltPhone FROM Counties c WHERE" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=c.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({0}) AND" +
          " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3] = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateLocalsContactsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, string[] localCodes, bool useBothContacts,
      int commandTimeout = -1)
    {
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allLocals = localCodes.Length == 1 &&
        localCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE StateCode='{stateCodes[0]}'";
          if (!allCounties)
            if (countyCodes.Length == 1)
            {
              whereClause += $" AND CountyCode='{countyCodes[0]}'";
              if (!allLocals)
                whereClause += " AND LocalCode IN ('" +
                  string.Join("','", localCodes) + "')";
            }
            else
              whereClause += " AND CountyCode IN ('" +
                string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "WHERE StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      var cmdText =
        "SELECT StateCode,CountyCode,LocalCode,Contact,ContactEmail,ContactTitle," +
          "Phone,AltContact,AltEMail,AltContactTitle,AltPhone FROM LocalDistricts {0}";

      cmdText = string.Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateLocalsContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localCodes,
      bool useBothContacts, string[] electionTypes, bool useFutureElections,
      bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allLocals = localCodes.Length == 1 &&
        localCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND l.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            if (countyCodes.Length == 1)
            {
              whereClause += $" AND l.CountyCode='{countyCodes[0]}'";
              if (!allLocals)
                whereClause += " AND l.LocalCode IN ('" +
                  string.Join("','", localCodes) + "')";
            }
            else
              whereClause += " AND l.CountyCode IN ('" +
                string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,l.Contact,l.ContactEmail," +
          "l.ContactTitle,l.Phone,l.AltContact,l.AltEMail,l.AltContactTitle," +
          "l.AltPhone,e.ElectionKey,e.ElectionKeyToInclude FROM LocalDistricts l INNER JOIN Elections e" +
          " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
          " WHERE e2.StateCode=l.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateLocalsContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localCodes,
      bool useBothContacts, string[] electionTypes, bool useFutureElections,
      bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allLocals = localCodes.Length == 1 &&
        localCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND l.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            if (countyCodes.Length == 1)
            {
              whereClause += $" AND l.CountyCode='{countyCodes[0]}'";
              if (!allLocals)
                whereClause += " AND l.LocalCode IN ('" +
                  string.Join("','", localCodes) + "')";
            }
            else
              whereClause += " AND l.CountyCode IN ('" +
                string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,l.Contact," +
          "l.ContactEmail,l.ContactTitle,l.Phone,l.AltContact,l.AltEMail," +
          "l.AltContactTitle,l.AltPhone FROM LocalDistricts l WHERE" +
          " (SELECT e.ElectionKey FROM Elections e" +
          " WHERE e.StateCode=l.StateCode AND e.CountyCode='' AND" +
          " e.LocalCode='' AND e.ElectionType IN ({0}) AND" +
          " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3] = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts);
    }

    public static int CreateAllCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] politicianEmailsToUse, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0) throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string statesCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
          "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
          "p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix,p.Phone,p.StatePhone,p.PartyKey" +
          " FROM States s INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
          " e.CountyCode='' AND e.LocalCode='' AND e.ElectionDate=" +
          " (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
          " INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
          " WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var statesCmdText = string.Format(statesCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC",
        "'" + string.Join("','", stateCodes) + "'");

      // build Counties SELECT
      var countiesWhereClause = string.Empty;
      if (!allStates)
        countiesWhereClause = "WHERE c.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
          "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
          "p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix,p.Phone,p.StatePhone,p.PartyKey" +
          " FROM Counties c INNER JOIN Elections e ON e.StateCode = c.StateCode" +
          " AND e.CountyCode=c.CountyCode AND e.LocalCode='' AND e.ElectionDate=" +
          " (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=c.StateCode AND e2.CountyCode=c.CountyCode AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
          " INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var countiesCmdText = string.Format(countiesCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = string.Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
          "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
          "p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix,p.Phone,p.StatePhone,p.PartyKey" +
          " FROM LocalDistricts l INNER JOIN Elections e ON e.StateCode = l.StateCode" +
          " AND e.CountyCode=l.CountyCode AND e.LocalCode=l.LocalCode" +
          " AND e.ElectionDate= (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=l.StateCode AND e2.CountyCode=l.CountyCode AND" +
          " e2.LocalCode=l.LocalCode AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var localsCmdText = string.Format(localsCmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", localsWhereClause);

      var cmdText = statesCmdText + " UNION " + countiesCmdText + " UNION " + localsCmdText;

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateStatesCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] politicianEmailsToUse, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0) throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate =
        "SELECT s.StateCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
          "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr," +
          "p.StateEmailAddr,p.CampaignEmail," + 
          //"p.LDSEmailAddr," +
          "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," +
          "p.StatePhone, " +
          //"p.LDSPhone," +
          "p.PartyKey" +
          " FROM States s INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
          " e.CountyCode='' AND e.LocalCode='' AND e.ElectionDate=" +
          " (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
          " INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
          " WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC",
        "'" + string.Join("','", stateCodes) + "'");

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateCountiesCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] politicianEmailsToUse,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0) throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");

      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" +
              string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "WHERE c.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT c.StateCode,c.CountyCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey," +
          "ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
          "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," + 
          //"p.LDSEmailAddr," +
          "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," +
          "p.StatePhone, " +
          //"p.LDSPhone," +
          "p.PartyKey" +
          " FROM Counties c INNER JOIN Elections e ON e.StateCode = c.StateCode" +
          " AND e.CountyCode=c.CountyCode AND e.LocalCode='' AND e.ElectionDate=" +
          " (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=c.StateCode AND e2.CountyCode=c.CountyCode AND" +
          " e2.LocalCode='' AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
          " INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateLocalsCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localCodes,
      string[] politicianEmailsToUse, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0) throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0) throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allLocals = localCodes.Length == 1 &&
        localCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = string.Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND l.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            if (countyCodes.Length == 1)
            {
              whereClause += $" AND l.CountyCode='{countyCodes[0]}'";
              if (!allLocals)
                whereClause += " AND l.LocalCode IN ('" +
                  string.Join("','", localCodes) + "')";
            }
            else
              whereClause += " AND l.CountyCode IN ('" +
                string.Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND l.StateCode IN ('" + string.Join("','", stateCodes) +
            "')";

      const string cmdTemplate =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey," +
          "ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
          "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," + 
          //",p.LDSEmailAddr," +
          "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," +
          "p.StatePhone, " +
          //"p.LDSPhone," +
          "p.PartyKey FROM LocalDistricts l" +
          " INNER JOIN Elections e ON e.StateCode = l.StateCode" +
          " AND e.CountyCode=l.CountyCode AND e.LocalCode=l.LocalCode" +
          " AND e.ElectionDate= (SELECT e2.ElectionDate FROM Elections e2" +
          " WHERE e2.StateCode=l.StateCode AND e2.CountyCode=l.CountyCode AND" +
          " e2.LocalCode=l.LocalCode AND e2.ElectionType IN ({0}) AND" +
          " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
          " LIMIT 1) INNER JOIN ElectionsPoliticians ep" +
          "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : string.Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = string.Format(cmdTemplate,
        "'" + string.Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : string.Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateStateCandidatesBatchWithNoElectionFiltering(int batchId, string[] stateCodes, string[] politicianEmailsToUse, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");
      var all = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      const string cmdTemplate =
        "SELECT StateCode,PoliticianKey,EmailAddrVoteUSA,EmailAddr,StateEmailAddr," +
          "CampaignEmail," + 
          //"LDSEmailAddr," + 
          "Fname,Mname,Lname,Nickname,Suffix,Phone," +
          //"StatePhone,LDSPhone,PartyKey FROM Politicians" +
          "StatePhone,PartyKey FROM Politicians" +
          " {0}";

      var whereClause = all ? string.Empty : "WHERE StateCode IN ('" + string.Join("','", stateCodes) + "') AND OptOut=0";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);
      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateCandidatesBatchByElection(int batchId, string[] politicianEmailsToUse, 
      string electionKey, bool includeAllElectionsOnDate, bool includeAllJurisdictions = false, 
      int commandTimeout = -1)
    {
      const string cmdTemplate =
        "SELECT e.StateCode,e.CountyCode,e.LocalCode,e.ElectionKey,e.ElectionKeyToInclude,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
          "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
          "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail,p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix," +
          "p.Phone,p.StatePhone,p.PartyKey FROM ElectionsPoliticians ep" +
          " INNER JOIN Elections e ON e.ElectionKey = ep.ElectionKey" +
          " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
          " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
          " WHERE ep.ElectionKey{0}";

      string whereCondition;
      if (includeAllJurisdictions)
        whereCondition = includeAllElectionsOnDate
          ? " LIKE '" + electionKey.Substring(0, 10) + "%'"
          : " LIKE '" + electionKey.Substring(0, 12) + "%'";
      else
        whereCondition = includeAllElectionsOnDate
          ? " LIKE '" + electionKey.Substring(0, 10) + "__" +
            electionKey.Substring(12) + "'"
          : "='" + electionKey + "'";

      // Substitutions:
      // {0} = WHERE condition

      var cmdText = string.Format(cmdTemplate, whereCondition);
      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse);
    }

    public static int CreateStatesPartiesBatchByState(int batchId,
      string[] stateCodes, string[] partyKeys, bool majorParties,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      // build WHERE clause
      if (stateCodes.Length == 0) stateCodes = new[] { string.Empty };
      var whereClause = stateCodes.Length == 1
        ? "WHERE p.StateCode = '" + stateCodes[0] + "'"
        : "WHERE " + stateCodes.SqlIn("p.StateCode");

      var allParties = partyKeys.Length == 1 && partyKeys[0] == "all";
      if (partyKeys.Length == 0) partyKeys = new[] { string.Empty };
      if (!allParties && !majorParties) whereClause += " AND " + partyKeys.SqlIn("p.PartyKey");

      if (majorParties) whereClause += " AND  p.IsPartyMajor=1";
      whereClause += " AND pe.IsVolunteer=0 AND pe.OptOut=0";

      const string cmdTemplate =
        "SELECT pe.PartyContactFName,pe.PartyContactLName,pe.PartyContactPhone," +
          "pe.PartyContactTitle,pe.PartyEmail,pe.PartyKey,p.StateCode FROM Parties p" +
          " INNER JOIN PartiesEmails pe ON pe.PartyKey=p.PartyKey {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText);
    }

    public static int CreateVolunteersBatch(int batchId,
      string[] stateCodes, string[] partyKeys, bool majorParties,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0) throw new VoteException("No states were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase)) stateCodes = StateCache.All51StateCodes.ToArray();

      // build WHERE clause
      if (stateCodes.Length == 0) stateCodes = new[] { string.Empty };
      var whereClause = stateCodes.Length == 1
        ? "WHERE p.StateCode = '" + stateCodes[0] + "'"
        : "WHERE " + stateCodes.SqlIn("p.StateCode");

      var allParties = partyKeys.Length == 1 && partyKeys[0] == "all";
      if (partyKeys.Length == 0) partyKeys = new[] { string.Empty };
      if (!allParties && !majorParties) whereClause += " AND " + partyKeys.SqlIn("p.PartyKey");

      if (majorParties) whereClause += " AND  p.IsPartyMajor=1";
      whereClause += " AND pe.IsVolunteer=1";

      const string cmdTemplate =
        "SELECT pe.PartyContactFName,pe.PartyContactLName,pe.PartyContactPhone," +
          "pe.PartyContactTitle,pe.PartyEmail,pe.PartyKey,p.StateCode FROM Parties p" +
          " INNER JOIN PartiesEmails pe ON pe.PartyKey=p.PartyKey {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText);
    }

    public static int CreateStatesPartiesBatchByElection(int batchId, 
      string electionKey, bool includeAllElectionsOnDate, int commandTimeout = -1)
    {
      // build WHERE clause
      var whereClause = includeAllElectionsOnDate
        ? "WHERE e.ElectionKey LIKE '" + electionKey.Substring(0, 11) + "_" + electionKey.Substring(12) + "'"
        : "WHERE e.ElectionKey = '" + electionKey + "'";
      whereClause += " AND pe.IsVolunteer=0 AND pe.OptOut=0";

      const string cmdTemplate =
        "SELECT e.ElectionKey,e.ElectionKeyToInclude,e.StateCode,pe.PartyContactFName," +
        "pe.PartyContactLName,pe.PartyContactPhone,pe.PartyContactTitle," +
        "pe.PartyEmail,pe.PartyKey FROM Elections e" +
        " INNER JOIN PartiesEmails pe ON pe.PartyKey=" +
        "CONCAT(SUBSTRING(e.ElectionKey, 1, 2),SUBSTRING(e.ElectionKey, 12, 1))" +
        " {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText);
    }

    public static int CreateVisitorsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, WebService.VisitorBatchOptions visitorOptions)
    {
      var beginTime = string.IsNullOrWhiteSpace(visitorOptions.StartDate)
      ? DateTime.MinValue
      : DateTime.Parse(visitorOptions.StartDate);
      var endTime = string.IsNullOrWhiteSpace(visitorOptions.EndDate)
       ? DateTime.MaxValue
       : DateTime.Parse(visitorOptions.EndDate) + new TimeSpan(1, 0, 0, 0); // add a day

      // build WHERE clause
      var whereClause = Addresses.FormatEmailBatchesWhereClause(stateCodes,
        countyCodes, visitorOptions.SampleBallots, visitorOptions.SharedChoices, 
        visitorOptions.Donated, visitorOptions.WithNames, visitorOptions.WithoutNames,
        visitorOptions.WithDistrictCoding, visitorOptions.WithoutDistrictCoding,
        beginTime, endTime, visitorOptions.DistrictFiltering,
        visitorOptions.Districts);

      const string cmdTemplate =
        "SELECT Id,FirstName,LastName,StateCode,Email,County AS CountyCode,Phone" +
        " FROM Addresses {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);

      return BuildVisitorsEmailBatch(batchId, cmdText, beginTime, endTime);
    }

    public static int CreateDonorsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, WebService.VisitorBatchOptions visitorOptions,
      WebService.DonorBatchOptions donorOptions)
    {
      var beginTime = string.IsNullOrWhiteSpace(donorOptions.StartDate)
      ? DateTime.MinValue
      : DateTime.Parse(donorOptions.StartDate);
      var endTime = string.IsNullOrWhiteSpace(donorOptions.EndDate)
       ? DateTime.MaxValue
       : DateTime.Parse(donorOptions.EndDate) + new TimeSpan(1, 0, 0, 0); // add a day

      // build WHERE clause
      var whereClause = DonorsView.FormatEmailBatchesWhereClause(stateCodes,
        countyCodes, beginTime, endTime, visitorOptions.DistrictFiltering,
        visitorOptions.Districts);

      const string cmdTemplate =
        "SELECT * FROM DonorsView {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = string.Format(cmdTemplate, whereClause);

      return BuildDonorsEmailBatch(batchId, cmdText, beginTime, endTime);
    }

    #region ReSharper restore

    // ReSharper restore UnusedParameter.Global
    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}