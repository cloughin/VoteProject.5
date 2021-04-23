using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Configuration;
using DB.VoteLog;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class TempEmailBatches
  {
    // ReSharper disable UnassignedField.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DonorBatchOptions
    {
      // uses district filtering from visitors
      public string StartDate;
      public string EndDate;
      public string DistrictCoding;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class VisitorBatchOptions
    {
      public string DistrictFiltering;
      public string[] Districts;
      public bool SampleBallots;
      public bool SharedChoices;
      public bool Donated;
      public bool WithNames;
      public bool WithoutNames;
      public bool WithDistrictCoding;
      public bool WithoutDistrictCoding;
      public string StartDate;
      public string EndDate;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class OrgBatchOptions
    {
      public int OrgTypeId;
      public int OrgSubTypeId;
      public int IdeologyId;
      public int[] EmailTagIds;
      public bool UseAllContacts;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EmailTypeBatchOptions
    {
      public bool Checked;
      public bool DidReceive;
      public string EmailType;
      public string StartDate;
      public string EndDate;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class LoginDateBatchOptions
    {
      public bool Checked;
      public bool DidLogin;
      public string StartDate;
      public string EndDate;
    }
    // ReSharper restore UnassignedField.Global

    private class EmailTypeValidator
    {
      private readonly Dictionary<string, bool> EmailTypeDictionary;
      private readonly EmailTypeBatchOptions EmailTypeOptions;

      public EmailTypeValidator(EmailTypeBatchOptions emailTypeOptions,
        IList<string> emails)
      {
        EmailTypeOptions = emailTypeOptions;
        if (!emails.Any()) return;
        var logCmdText = "SELECT ToEmail FROM LogEmail" +
          $" WHERE EmailTypeCode='{emailTypeOptions.EmailType}' AND" +
          $" {emails.Distinct(StringComparer.OrdinalIgnoreCase).SqlIn("ToEmail")}";
        if (!IsNullOrWhiteSpace(emailTypeOptions.StartDate))
          logCmdText += " AND SentTime>=@StartDate";
        if (!IsNullOrWhiteSpace(emailTypeOptions.EndDate))
          logCmdText += " AND SentTime<@EndDate";
        logCmdText += " GROUP BY ToEmail";

        var logCmd = VoteLogDb.GetCommand(logCmdText, 0);
        if (!IsNullOrWhiteSpace(emailTypeOptions.StartDate))
          VoteLogDb.AddCommandParameter(logCmd, "StartDate",
            DateTime.Parse(emailTypeOptions.StartDate));
        if (!IsNullOrWhiteSpace(emailTypeOptions.EndDate))
          VoteLogDb.AddCommandParameter(logCmd, "EndDate",
            DateTime.Parse(emailTypeOptions.EndDate).AddDays(1));
        var logTable = new DataTable();
        using (var cn = VoteLogDb.GetOpenConnection())
        {
          logCmd.Connection = cn;
          DbDataAdapter adapter = new MySqlDataAdapter(logCmd as MySqlCommand);
          adapter.Fill(logTable);
        }

        EmailTypeDictionary = logTable.Rows.Cast<DataRow>().Select(r => r.ToEmail())
          .ToDictionary(a => a, a => true, StringComparer.OrdinalIgnoreCase);
      }

      public bool Validate(string email)
      {
        if (EmailTypeDictionary == null) return true;
        var isInDictionary = EmailTypeDictionary.ContainsKey(email);
        return EmailTypeOptions.DidReceive == isInDictionary;
      }
    }

    private class LoginDateValidator
    {
      private readonly Dictionary<string, bool> LoginDictionary;
      private readonly LoginDateBatchOptions LogonDateOptions;

      public LoginDateValidator(LoginDateBatchOptions loginDateOptions, IList<string> users)
      {
        LogonDateOptions = loginDateOptions;
        if (!users.Any()) return;
        var logCmdText = "SELECT UserName FROM LogLogins" +
          $" WHERE {users.Distinct(StringComparer.OrdinalIgnoreCase).SqlIn("UserName")}";
        if (!IsNullOrWhiteSpace(loginDateOptions.StartDate))
          logCmdText += " AND DateStamp>=@StartDate";
        if (!IsNullOrWhiteSpace(loginDateOptions.EndDate))
          logCmdText += " AND DateStamp<@EndDate";
        logCmdText += " GROUP BY UserName";

        var logCmd = VoteLogDb.GetCommand(logCmdText, 0);
        if (!IsNullOrWhiteSpace(loginDateOptions.StartDate))
          VoteLogDb.AddCommandParameter(logCmd, "StartDate",
            DateTime.Parse(loginDateOptions.StartDate));
        if (!IsNullOrWhiteSpace(loginDateOptions.EndDate))
          VoteLogDb.AddCommandParameter(logCmd, "EndDate",
            DateTime.Parse(loginDateOptions.EndDate).AddDays(1));
        var logTable = new DataTable();
        using (var cn = VoteLogDb.GetOpenConnection())
        {
          logCmd.Connection = cn;
          DbDataAdapter adapter = new MySqlDataAdapter(logCmd as MySqlCommand);
          adapter.Fill(logTable);
        }

        LoginDictionary = logTable.Rows.Cast<DataRow>().Select(r => r.UserName())
          .ToDictionary(a => a, a => true, StringComparer.OrdinalIgnoreCase);
      }

      public bool Validate(string email)
      {
        if (LoginDictionary == null) return true;
        var isInDictionary = LoginDictionary.ContainsKey(email);
        return LogonDateOptions.DidLogin == isInDictionary;
      }
    }

    private static int BuildContactsEmailBatch(int batchId, string cmdText,
      bool useBothContacts, EmailTypeBatchOptions emailTypeOptions)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var emailAddressList = new List<string>();
      if (emailTypeOptions?.Checked == true)
      {
        // need a pre-pass to get email list
        foreach (var row in table.Rows.Cast<DataRow>())
        {
          var hadMain = false;
          if (Validation.IsValidEmailAddress(row.ContactEmail()))
          {
            hadMain = true;
            emailAddressList.Add(row.ContactEmail());
          }

          if ((useBothContacts || !hadMain) &&
            Validation.IsValidEmailAddress(row.AltEmail()))
            emailAddressList.Add(row.ContactEmail());
        }
      }

      var emailTypeValidator = new EmailTypeValidator(emailTypeOptions, emailAddressList);

      // check for optional columns
      var countyCodeExists = table.Columns.Contains("CountyCode");
      var localKeyExists = table.Columns.Contains("LocalKey");
      var electionKeyExists = table.Columns.Contains("ElectionKey");
      var electionKeyToIncludeExists = table.Columns.Contains("ElectionKeyToInclude");

      var tempEmailTable = new TempEmailTable();
      foreach (var row in table.Rows.Cast<DataRow>())
      {
        var hadMain = false;
        if (Validation.IsValidEmailAddress(row.ContactEmail()) &&
          emailTypeValidator.Validate(row.ContactEmail()))
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
          tempEmailTable.AddRow(batchId, email, contact, title, phone, row.StateCode(),
            countyCodeExists ? row.CountyCode() : null,
            localKeyExists ? row.LocalKey() : null, null,
            electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null, null,
            null, 0, 0, 0, sortName, row.SourceCode().Replace('?', 'P'));
        }

        if ((useBothContacts || !hadMain) &&
          Validation.IsValidEmailAddress(row.AltEmail()) &&
          emailTypeValidator.Validate(row.AltEmail()))
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
          tempEmailTable.AddRow(batchId, email, contact, title, phone, row.StateCode(),
            countyCodeExists ? row.CountyCode() : null,
            localKeyExists ? row.LocalKey() : null, null,
            electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null, null,
            null, 0, 0, 0, sortName, row.SourceCode().Replace('?', 'A'));
        }
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
    }

    private static Dictionary<string, char> PoliticianSourceCodeSubtypes =
      new Dictionary<string, char>
      {
        {"EmailAddr", 'M'},
        {"CampaignEmail", 'M'},
        {"StateEmailAddr", 'S'},
        {"EmailAddrVoteUSA", 'V'}
      };

    private static int BuildCandidatesEmailBatch(int batchId, string cmdText,
      string[] politicianEmailsToUse, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var emailAddressList = new List<string>();
      var politicianKeyList = new List<string>();
      if (emailTypeOptions?.Checked == true || loginDateOptions?.Checked == true)
      {
        // need a pre-Pass to get email list
        foreach (var row in table.Rows.Cast<DataRow>())
        {
          foreach (var emailKey in politicianEmailsToUse)
          {
            var email = row[emailKey] as string;
            if (email != null && email.Length > TempEmail.EmailMaxLength)
              email = email.Substring(0, TempEmail.EmailMaxLength);
            if (Validation.IsValidEmailAddress(email))
            {
              if (emailTypeOptions?.Checked == true)
                emailAddressList.Add(email);
              if (loginDateOptions?.Checked == true)
                politicianKeyList.Add(row.PoliticianKey());
            }
          }
        }
      }

      var emailTypeValidator = new EmailTypeValidator(emailTypeOptions, emailAddressList);
      var loginDateValidator = new LoginDateValidator(loginDateOptions, politicianKeyList);

      // check for optional columns
      var countyCodeExists = table.Columns.Contains("CountyCode");
      var localKeyExists = table.Columns.Contains("LocalKey");
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
          var sortName = row.LastName() + " " + row.Suffix() + " " + row.LastName() + " " +
            row.MiddleName() + " " + row.Nickname();
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
          if (Validation.IsValidEmailAddress(email) && emailTypeValidator.Validate(email) &&
            loginDateValidator.Validate(row.PoliticianKey()))
            tempEmailTable.AddRow(batchId, email, contact, officeName, phone,
              row.StateCode(), countyCodeExists ? row.CountyCode() : null,
              localKeyExists ? row.LocalKey() : null, row.PoliticianKey(),
              electionKeyExists ? row.ElectionKey() : null,
              electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null,
              officeKeyExists ? row.OfficeKey() : null, row.PartyKey(), null, 0, 0, 0,
              sortName, row.SourceCode().Replace('?', PoliticianSourceCodeSubtypes[emailKey]));
        }
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildDonorsEmailBatch(int batchId, string cmdText, DateTime fromDate,
      DateTime toDate, EmailTypeBatchOptions emailTypeOptions)
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
      var grouped = table.Rows.Cast<DataRow>().GroupBy(r => r.Email().ToLowerInvariant())
        .Select(g => g.First()).ToList();

      var emailAddressList = new List<string>();
      if (emailTypeOptions?.Checked == true)
      {
        // need a pre-PassportAuthentication to get email list
        foreach (var row in grouped)
        {
          var email = row.Email();
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (Validation.IsValidEmailAddress(email))
            emailAddressList.Add(email);
        }
      }

      var emailTypeValidator = new EmailTypeValidator(emailTypeOptions, emailAddressList);

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
        if (Validation.IsValidEmailAddress(email) && emailTypeValidator.Validate(email))
          tempEmailTable.AddRow(batchId, email, contact, Empty, phone, row.StateCode(),
            row.CountyCode(), null, null, null, null, null, null, null, row.VisitorId(),
            row.Id(), 0, sortName, $"A-{row.Id()}");
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildOrganizationsEmailBatch(int batchId, string cmdText, bool useAllContacts,
      EmailTypeBatchOptions emailTypeOptions)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var tempEmailTable = new TempEmailTable();

      // convert to list
      var rows = table.Rows.OfType<DataRow>().ToList();

      // select only the first for org unless useAllContacts
      if (!useAllContacts)
        rows = rows.GroupBy(r => r.OrgId()).Select(g => g.First()).ToList();

      var emailAddressList = new List<string>();
      if (emailTypeOptions?.Checked == true)
      {
        // need a pre-pass to get email list
        foreach (var row in rows)
        {
          var email = row.Email();
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (Validation.IsValidEmailAddress(email) && !emailAddressList.Contains(email))
            emailAddressList.Add(email);
        }
      }

      var emailTypeValidator = new EmailTypeValidator(emailTypeOptions, emailAddressList);

      foreach (var row in rows)
      {
        var email = row.Email();
        var contact = row.Contact();
        var phone = row.Phone();

        var sortName = Empty;
        if (!IsNullOrWhiteSpace(contact))
        {
          // try to decontruct name
          var parsedName = contact.ParseName();
          var components = new List<string>();
          if (!IsNullOrWhiteSpace(parsedName.Last))
            components.Add(parsedName.Last);
          if (!IsNullOrWhiteSpace(parsedName.Suffix))
            components.Add(parsedName.Suffix);
          if (!IsNullOrWhiteSpace(parsedName.First))
            components.Add(parsedName.First);
          if (!IsNullOrWhiteSpace(parsedName.Middle))
            components.Add(parsedName.Middle);
          sortName = Join(" ", components);
        }

        if (email != null && email.Length > TempEmail.EmailMaxLength)
          email = email.Substring(0, TempEmail.EmailMaxLength);
        if (contact != null && contact.Length > TempEmail.ContactMaxLength)
          contact = contact.Substring(0, TempEmail.ContactMaxLength);
        if (phone != null && phone.Length > TempEmail.PhoneMaxLength)
          phone = phone.Substring(0, TempEmail.PhoneMaxLength);
        if (sortName.Length > TempEmail.SortNameMaxLength)
          sortName = sortName.Substring(0, TempEmail.SortNameMaxLength);
        if (Validation.IsValidEmailAddress(email) && emailTypeValidator.Validate(email))
          tempEmailTable.AddRow(batchId, email, contact, row.Name(), phone, row.StateCode(),
            null, null, null, null, null, null, null, null, 0,
            0, row.ContactId(), sortName, $"O-{row.ContactId()}");
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildPartiesEmailBatch(int batchId, string cmdText,
      EmailTypeBatchOptions emailTypeOptions, LoginDateBatchOptions loginDateOptions)
    {
      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      var emailTypeAddressList = new List<string>();
      var loginDateAddressList = new List<string>();
      if (emailTypeOptions?.Checked == true || loginDateOptions?.Checked == true)
      {
        // need a pre-PassportAuthentication to get email list
        foreach (var row in table.Rows.Cast<DataRow>())
        {
          var email = row.PartyEmail();
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (Validation.IsValidEmailAddress(email))
          {
            if (emailTypeOptions?.Checked == true)
              emailTypeAddressList.Add(email);
            if (loginDateOptions?.Checked == true)
              loginDateAddressList.Add(email);
          }
        }
      }

      var emailTypeValidator =
        new EmailTypeValidator(emailTypeOptions, emailTypeAddressList);
      var loginDateValidator =
        new LoginDateValidator(loginDateOptions, loginDateAddressList);

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
        if (Validation.IsValidEmailAddress(email) && emailTypeValidator.Validate(email) &&
          loginDateValidator.Validate(email))
          tempEmailTable.AddRow(batchId, email, contact, title, phone, row.StateCode(),
            null, null, null, electionKeyExists ? row.ElectionKey() : null,
            electionKeyToIncludeExists ? row.ElectionKeyToInclude() : null, null,
            row.PartyKey(), email, 0, 0, 0, sortName, $"Z-{row.Id()}");
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
    }

    private static int BuildVisitorsEmailBatch(int batchId, string cmdText,
      DateTime fromDate, DateTime toDate, EmailTypeBatchOptions emailTypeOptions)
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

      var emailAddressList = new List<string>();
      if (emailTypeOptions?.Checked == true)
      {
        // need a pre-PassportAuthentication to get email list
        foreach (var row in table.Rows.Cast<DataRow>())
        {
          var email = row.Email();
          if (email != null && email.Length > TempEmail.EmailMaxLength)
            email = email.Substring(0, TempEmail.EmailMaxLength);
          if (Validation.IsValidEmailAddress(email))
            emailAddressList.Add(email);
        }
      }

      var emailTypeValidator = new EmailTypeValidator(emailTypeOptions, emailAddressList);

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
        if (Validation.IsValidEmailAddress(email) && emailTypeValidator.Validate(email))
          tempEmailTable.AddRow(batchId, email, contact, Empty, phone, row.StateCode(),
            row.CountyCode(), null, null, null, null, null, null, null, row.Id(), 0, 0,
            sortName, $"A-{row.Id()}");
      }

      TempEmail.UpdateTable(tempEmailTable, TempEmailTable.ColumnSet.All, 0,
        ConflictOption.CompareAllSearchableValues, true);

      return tempEmailTable.Count(row => !IsNullOrWhiteSpace(row.RowError));
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
          WebConfigurationManager.AppSettings["VoteTempEmailBatchesRetentionDays"];
        if (!int.TryParse(daysString, out var days)) days = 2;

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
      var cmdText = SelectAllCommandText + " WHERE CreationTime<@Expiration";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "Expiration", expiration);
      return FillTable(cmd, TempEmailBatchesTable.ColumnSet.All);
    }

    public static string ConcoctSortName(string fullName)
    {
      fullName = fullName.StripRedundantWhiteSpace();
      if (fullName == Empty) return Empty;
      var names = fullName.ToLowerInvariant()
        .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
      if (names.Length == 1) return names[0];
      var lastNameIndex = names.Length - 1;
      if (Validation.IsValidNameSuffix(names[lastNameIndex])) lastNameIndex--;
      else if (fullName.Contains(","))
      {
        // assume it's lastname, firstname since we've ruled out a suffix
        lastNameIndex = 0;
      }

      return Join(" ", names.Skip(lastNameIndex)) + " " +
        Join(" ", names.Take(lastNameIndex));
    }

    public static int CreateAllContactsBatch(int batchId, string[] stateCodes,
      bool useBothContacts, EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build States SELECT
      var statesCmdText =
        "SELECT StateCode,'' AS CountyCode,'' AS LocalKey,Contact,ContactEmail,ContactTitle,Phone," +
        "AltContact,AltEMail,AltContactTitle,AltPhone,CONCAT('S?-',StateCode) AS SourceCode" + 
        " FROM States WHERE {0}";
      var stateWhereClause = "IsState=1";
      if (!allStates) stateWhereClause = "StateCode IN ('" + Join("','", stateCodes) + "')";
      statesCmdText = Format(statesCmdText, stateWhereClause);

      // build Counties SELECT
      var countiesWhereClause = Empty;
      if (!allStates)
        countiesWhereClause = "WHERE IsCountyTagForDeletion=0 AND StateCode IN ('" +
          Join("','", stateCodes) + "')";
      var countiesCmdText =
        "SELECT StateCode,CountyCode,'' AS LocalKey,Contact,ContactEmail,ContactTitle,Phone," +
        "AltContact,AltEMail,AltContactTitle,AltPhone,CONCAT('C?-',StateCode,CountyCode) AS SourceCode" + 
        " FROM Counties {0}";
      countiesCmdText = Format(countiesCmdText, countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = Empty;
      if (!allStates)
        localsWhereClause = "WHERE IsLocalDistrictTagForDeletion=0 AND StateCode IN ('" +
          Join("','", stateCodes) + "')";
      var localsCmdText =
        "SELECT StateCode,'' AS CountyCode,LocalKey,Contact,ContactEmail,ContactTitle,Phone," +
        "AltContact,AltEMail,AltContactTitle,AltPhone,CONCAT('L?-',StateCode,LocalKey) AS SourceCode" + 
        " FROM LocalDistricts {0}";
      localsCmdText = Format(localsCmdText, localsWhereClause);

      var cmdText = statesCmdText + " UNION ALL " + countiesCmdText + " UNION ALL " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateAllContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string stateCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalKey,s.Contact,s.ContactEmail," +
        "s.ContactTitle,s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle,s.AltPhone," +
        "e.ElectionKey,e.ElectionKeyToInclude,CONCAT('S?-',s.StateCode) AS SourceCode FROM States s" +

        " INNER JOIN Elections e ON e.ElectionKey=" +
        " (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
        " LIMIT 1)" + 

        " WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var statesCmdText = Format(stateCmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", "'" + Join("','", stateCodes) + "'");

      // build Counties SELECT
      var countiesWhereClause = Empty;
      if (!allStates)
        countiesWhereClause = "AND c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalKey,c.Contact,c.ContactEmail,c.ContactTitle," +
        "c.Phone,c.AltContact,c.AltEMail,c.AltContactTitle,c.AltPhone,e.ElectionKey," +
        "e.ElectionKeyToInclude,CONCAT('C?-',c.StateCode,c.CountyCode) AS SourceCode" +
        " FROM Counties c INNER JOIN Elections e" +
        " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=c.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" + " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var countiesCmdText = Format(countiesCmdTemplate,
        "'" + Join("','", electionTypes) + "'", useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : Empty, useFutureElections ? "ASC" : "DESC",
        countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.LocalKey,l.Contact,l.ContactEmail," +
        "l.ContactTitle,l.Phone,l.AltContact,l.AltEMail,l.AltContactTitle," +
        "l.AltPhone,e.ElectionKey,e.ElectionKeyToInclude," +
        "CONCAT('L?-',l.StateCode,l.LocalKey) AS SourceCode" +
        " FROM LocalDistricts l INNER JOIN Elections e" +
        " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=l.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" + " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var localsCmdText = Format(localsCmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", localsWhereClause);

      var cmdText = statesCmdText + " UNION ALL " + countiesCmdText + " UNION ALL " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateAllContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string statesCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalKey,s.Contact,s.ContactEmail," +
        "s.ContactTitle,s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle," +
        "s.AltPhone,CONCAT('S?-',s.StateCode) AS SourceCode" + 
        " FROM States s WHERE s.StateCode IN ({0}) AND" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=s.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({1}) AND" +
        " e.ElectionDate{2}UTC_DATE() {3} LIMIT 1) IS NULL";

      // Substitutions:
      // {0} = stateCodes (single-quoted, comma separated)
      // {1} = electionTypes (single-quoted, comma separated)
      // {2} = useFutureElections ? ">=" : "<"
      // {3} = viewableOnly ? "AND IsViewable=1" : Empty

      var statesCmdText = Format(statesCmdTemplate, "'" + Join("','", stateCodes) + "'",
        "'" + Join("','", electionTypes) + "'", useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : Empty);

      // build Counties SELECT
      var countiesWhereClause = Empty;
      if (!allStates)
        countiesWhereClause = "AND c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalKey,c.Contact," +
        "c.ContactEmail,c.ContactTitle,c.Phone,c.AltContact,c.AltEMail," +
        "c.AltContactTitle,c.AltPhone,CONCAT('C?-',c.StateCode,c.CountyCode) AS SourceCode" +
        " FROM Counties c WHERE" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=c.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({0}) AND" +
        " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3] = WHERE clause

      var countiesCmdText = Format(countiesCmdTemplate,
        "'" + Join("','", electionTypes) + "'", useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : Empty, countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.LocalKey,l.Contact," +
        "l.ContactEmail,l.ContactTitle,l.Phone,l.AltContact,l.AltEMail," +
        "l.AltContactTitle,l.AltPhone,CONCAT('L?-',l.StateCode,l.LocalKey) AS SourceCode" +
        " FROM LocalDistricts l WHERE" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=l.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({0}) AND" +
        " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3] = WHERE clause

      var localsCmdText = Format(localsCmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        localsWhereClause);

      var cmdText = statesCmdText + " UNION ALL " + countiesCmdText + " UNION ALL " + localsCmdText;

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateStatesContactsBatch(int batchId, string[] stateCodes,
      bool useBothContacts, EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      var all = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      var cmdText = "SELECT StateCode,Contact,ContactEmail,ContactTitle,Phone,AltContact," +
        "AltEMail,AltContactTitle,AltPhone,CONCAT('S?-',StateCode) AS SourceCode" + 
        " FROM States WHERE {0}";

      // build WHERE clause
      var whereClause = "IsState=1";
      if (!all) whereClause = "StateCode IN ('" + Join("','", stateCodes) + "')";

      cmdText = Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateStatesContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate =
        "SELECT s.StateCode,s.Contact,s.ContactEmail,s.ContactTitle,s.Phone," +
        "s.AltContact,s.AltEMail,s.AltContactTitle,s.AltPhone,e.ElectionKey,e.ElectionKeyToInclude," +
        "CONCAT('S?-',s.StateCode) AS SourceCode" +
        " FROM States s INNER JOIN Elections e ON e.ElectionKey=" +
        " (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
        " LIMIT 1) WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", "'" + Join("','", stateCodes) + "'");

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateStatesContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, bool useBothContacts, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate = "SELECT s.StateCode,s.Contact,s.ContactEmail," +
        "s.ContactTitle,s.Phone,s.AltContact,s.AltEMail,s.AltContactTitle," +
        "s.AltPhone,CONCAT('S?-',s.StateCode) AS SourceCode" +
        " FROM States s WHERE s.StateCode IN ({0}) AND" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=s.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({1}) AND" +
        " e.ElectionDate{2}UTC_DATE() {3} LIMIT 1) IS NULL";

      // Substitutions:
      // {0} = stateCodes (single-quoted, comma separated)
      // {1} = electionTypes (single-quoted, comma separated)
      // {2} = useFutureElections ? ">=" : "<"
      // {3} = viewableOnly ? "AND IsViewable=1" : Empty

      var cmdText = Format(cmdTemplate, "'" + Join("','", stateCodes) + "'",
        "'" + Join("','", electionTypes) + "'", useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : Empty);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateCountiesContactsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, bool useBothContacts, EmailTypeBatchOptions emailTypeOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND CountyCode IN ('" + Join("','", countyCodes) + "')";
        }
        else
          whereClause = "WHERE StateCode IN ('" + Join("','", stateCodes) + "')";

      var cmdText = "SELECT StateCode,CountyCode,Contact,ContactEmail,ContactTitle,Phone," +
        "AltContact,AltEMail,AltContactTitle,AltPhone,CONCAT('C?-',StateCode,CountyCode) AS SourceCode" +
        " FROM Counties {0}";

      cmdText = Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateCountiesContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, bool useBothContacts,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" + Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string cmdTemplate =
        "SELECT c.StateCode,c.CountyCode,c.Contact,c.ContactEmail," +
        "c.ContactTitle,c.Phone,c.AltContact,c.AltEMail,c.AltContactTitle," +
        "c.AltPhone,e.ElectionKey,e.ElectionKeyToInclude,CONCAT('C?-'," +
        "c.StateCode,c.CountyCode) AS SourceCode" +
        " FROM Counties c INNER JOIN Elections e" +
        " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=c.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" + " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateCountiesContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, bool useBothContacts,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"AND c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" + Join("','", countyCodes) + "')";
        }
        else
          whereClause = "AND c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string cmdTemplate = "SELECT c.StateCode,c.CountyCode,c.Contact," +
        "c.ContactEmail,c.ContactTitle,c.Phone,c.AltContact,c.AltEMail," +
        "c.AltContactTitle,c.AltPhone,CONCAT('C?-',c.StateCode,c.CountyCode) AS SourceCode" +
        " FROM Counties c WHERE" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=c.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({0}) AND" +
        " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL {3}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3] = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    private static string BuildWhereClauseJurisdictions(string[] stateCodes,
      IReadOnlyList<string> countyCodes, string[] localKeysOrCodes)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Count == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allLocals = localKeysOrCodes.Length == 1 && localKeysOrCodes[0]
        .Equals("all", StringComparison.OrdinalIgnoreCase);
      var whereClause = Empty;

      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE l.StateCode='{stateCodes[0]}'";
          if (!allCounties)
          {
            if (countyCodes.Count == 1 && !allLocals)
              whereClause += " AND l.LocalKey IN ('" + Join("','", localKeysOrCodes) + "')";
            else
            {
              // we need to do a pre-query to get all the locals in the selected counties
              var localKeys =
                LocalDistricts.GetLocalKeysForCounties(stateCodes[0], countyCodes);
              whereClause += " AND l.LocalKey IN ('" + Join("','", localKeys) + "')";
            }
          }
        }
        else
          whereClause = "WHERE l.StateCode IN ('" + Join("','", stateCodes) + "')";

      return whereClause;
    }

    public static int CreateLocalsContactsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, string[] localKeysOrCodes, bool useBothContacts,
      EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      var whereClause =
        BuildWhereClauseJurisdictions(stateCodes, countyCodes, localKeysOrCodes);

      var cmdText =
        "SELECT StateCode,'' AS CountyCode,LocalKey,Contact,ContactEmail,ContactTitle," +
        "Phone,AltContact,AltEMail,AltContactTitle,AltPhone," +
        "CONCAT('L?-',StateCode,LocalKey) AS SourceCode" +
        " FROM LocalDistricts l {0}";

      cmdText = Format(cmdText, whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateLocalsContactsBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localKeysOrCodes,
      bool useBothContacts, string[] electionTypes, bool useFutureElections,
      bool viewableOnly, EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      var whereClause =
        BuildWhereClauseJurisdictions(stateCodes, countyCodes, localKeysOrCodes);

      const string cmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.Contact,l.ContactEmail," +
        "l.ContactTitle,l.Phone,l.AltContact,l.AltEMail,l.AltContactTitle," +
        "l.AltPhone,e.ElectionKey,e.ElectionKeyToInclude," +
        "CONCAT('L?-',l.StateCode,l.LocalKey) AS SourceCode" +
        " FROM LocalDistricts l INNER JOIN Elections e" +
        " ON e.ElectionKey= (SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=l.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" + " LIMIT 1) {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateLocalsContactsBatchWithoutElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localKeysOrCodes,
      bool useBothContacts, string[] electionTypes, bool useFutureElections,
      bool viewableOnly, EmailTypeBatchOptions emailTypeOptions, int commandTimeout = -1)
    {
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      var whereClause =
        BuildWhereClauseJurisdictions(stateCodes, countyCodes, localKeysOrCodes);

      const string cmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.LocalKey,l.Contact," +
        "l.ContactEmail,l.ContactTitle,l.Phone,l.AltContact,l.AltEMail," +
        "l.AltContactTitle,l.AltPhone,CONCAT('L?-',l.StateCode,l.LocalKey) AS SourceCode" +
        " FROM LocalDistricts l {3} AND" +
        " (SELECT e.ElectionKey FROM Elections e" +
        " WHERE e.StateCode=l.StateCode AND e.CountyCode='' AND" +
        " e.LocalKey='' AND e.ElectionType IN ({0}) AND" +
        " e.ElectionDate{1}UTC_DATE() {2} LIMIT 1) IS NULL";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        whereClause);

      return BuildContactsEmailBatch(batchId, cmdText, useBothContacts, emailTypeOptions);
    }

    public static int CreateAllCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] politicianEmailsToUse, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0)
        throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");
      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      // build States SELECT
      const string statesCmdTemplate =
        "SELECT s.StateCode,'' AS CountyCode,'' AS LocalKey,e.ElectionKey," +
        "e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2," +
        "p.EmailAddrVoteUSA,p.EmailAddr,p.StateEmailAddr,p.CampaignEmail,p.Fname,p.Mname," +
        "p.Lname,p.Nickname,p.Suffix,p.Phone,p.StatePhone,p.PartyKey," +
        "CONCAT('P?-',p.Id) AS SourceCode FROM States s" +

        //" INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
        //" e.CountyCode='' AND e.LocalKey='' AND e.ElectionDate=" +
        //" (SELECT e2.ElectionDate FROM Elections e2" +
        //" WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        //" e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        //" e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +

        // cannot understand why above did not work [it returned the wrong election's date,
        // seemed to ignore ElectionType IN ('P')]
        // below is kludgy, but works
        " INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
        " e.CountyCode='' AND e.LocalKey='' AND e.ElectionYYYYMMDD=" +
        " SUBSTR((SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1),3,8)" +

        " INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
        " WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var statesCmdText = Format(statesCmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", "'" + Join("','", stateCodes) + "'");

      // build Counties SELECT
      var countiesWhereClause = Empty;
      if (!allStates)
        countiesWhereClause = "WHERE c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string countiesCmdTemplate =
        "SELECT c.StateCode,c.CountyCode,'' AS LocalKey,e.ElectionKey,e.ElectionKeyToInclude," +
        "ep.OfficeKey,ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr," +
        "p.StateEmailAddr,p.CampaignEmail,p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix,p.Phone," +
        "p.StatePhone,p.PartyKey,CONCAT('P?-',p.Id) AS SourceCode" +
        " FROM Counties c INNER JOIN Elections e ON e.StateCode = c.StateCode" +
        " AND e.CountyCode=c.CountyCode AND e.LocalKey='' AND e.ElectionDate=" +
        " (SELECT e2.ElectionDate FROM Elections e2" +
        " WHERE e2.StateCode=c.StateCode AND e2.CountyCode=c.CountyCode AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
        " INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var countiesCmdText = Format(countiesCmdTemplate,
        "'" + Join("','", electionTypes) + "'", useFutureElections ? ">=" : "<",
        viewableOnly ? "AND IsViewable=1" : Empty, useFutureElections ? "ASC" : "DESC",
        countiesWhereClause);

      // build Locals SELECT
      var localsWhereClause = Empty;
      if (!allStates)
        localsWhereClause = "AND l.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string localsCmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.LocalKey,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
        "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
        "p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix,p.Phone,p.StatePhone,p.PartyKey," +
        "CONCAT('P?-',p.Id) AS SourceCode" +
        " FROM LocalDistricts l INNER JOIN Elections e ON e.StateCode = l.StateCode" +
        " AND e.LocalKey=l.LocalKey" +
        " AND e.ElectionDate= (SELECT e2.ElectionDate FROM Elections e2" +
        " WHERE e2.StateCode=l.StateCode AND" +
        " e2.LocalKey=l.LocalKey AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
        " LIMIT 1) INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var localsCmdText = Format(localsCmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", localsWhereClause);

      var cmdText = statesCmdText + " UNION ALL " + countiesCmdText + " UNION ALL " + localsCmdText;

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateStatesCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] politicianEmailsToUse, string[] electionTypes,
      bool useFutureElections, bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0)
        throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      const string cmdTemplate =
        "SELECT s.StateCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
        "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA,p.EmailAddr," +
        "p.StateEmailAddr,p.CampaignEmail," +
        "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," + "p.StatePhone, " +
        "p.PartyKey,CONCAT('P?-',p.Id) AS SourceCode FROM States s" +

        //" INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
        //" e.CountyCode='' AND e.LocalKey='' AND e.ElectionDate=" +
        //" (SELECT e2.ElectionDate FROM Elections e2" +
        //" WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        //" e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        //" e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +

        // cannot understand why above did not work [it returned the wrong election's date,
        // seemed to ignore ElectionType IN ('P')]
        // below is kludgy, but works
        " INNER JOIN Elections e ON e.StateCode = s.StateCode AND" +
        " e.CountyCode='' AND e.LocalKey='' AND e.ElectionYYYYMMDD=" +
        " SUBSTR((SELECT e2.ElectionKey FROM Elections e2" +
        " WHERE e2.StateCode=s.StateCode AND e2.CountyCode='' AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1),3,8)" +

        " INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" +
        " WHERE s.StateCode IN ({4})";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = stateCodes (single-quoted, comma separated)

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", "'" + Join("','", stateCodes) + "'");

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateCountiesCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] politicianEmailsToUse,
      string[] electionTypes, bool useFutureElections, bool viewableOnly,
      EmailTypeBatchOptions emailTypeOptions, LoginDateBatchOptions loginDateOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0)
        throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      var allStates = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);
      var allCounties = countyCodes.Length == 1 &&
        countyCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      // build WHERE clause
      var whereClause = Empty;
      if (!allStates)
        if (stateCodes.Length == 1)
        {
          whereClause = $"WHERE c.StateCode='{stateCodes[0]}'";
          if (!allCounties)
            whereClause += " AND c.CountyCode IN ('" + Join("','", countyCodes) + "')";
        }
        else
          whereClause = "WHERE c.StateCode IN ('" + Join("','", stateCodes) + "')";

      const string cmdTemplate =
        "SELECT c.StateCode,c.CountyCode,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey," +
        "ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
        "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
        "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," + "p.StatePhone, " +
        "p.PartyKey,CONCAT('P?-',p.Id) AS SourceCode" +
        " FROM Counties c INNER JOIN Elections e ON e.StateCode = c.StateCode" +
        " AND e.CountyCode=c.CountyCode AND e.LocalKey='' AND e.ElectionDate=" +
        " (SELECT e2.ElectionDate FROM Elections e2" +
        " WHERE e2.StateCode=c.StateCode AND e2.CountyCode=c.CountyCode AND" +
        " e2.LocalKey='' AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3} LIMIT 1)" +
        " INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateLocalsCandidatesBatchWithElectionTypes(int batchId,
      string[] stateCodes, string[] countyCodes, string[] localKeysOrCodes,
      string[] politicianEmailsToUse, string[] electionTypes, bool useFutureElections,
      bool viewableOnly, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      if (politicianEmailsToUse == null || politicianEmailsToUse.Length == 0)
        throw new VoteException("No email types were supplied");
      if (electionTypes == null || electionTypes.Length == 0)
        throw new VoteException("No election types were supplied");

      var whereClause =
        BuildWhereClauseJurisdictions(stateCodes, countyCodes, localKeysOrCodes);

      const string cmdTemplate =
        "SELECT l.StateCode,'' AS CountyCode,l.LocalKey,e.ElectionKey,e.ElectionKeyToInclude,ep.OfficeKey," +
        "ep.PoliticianKey,o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
        "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail," +
        "p.Fname, p.Mname, p.Lname, p.Nickname, p.Suffix, p.Phone," + "p.StatePhone, " +
        "p.PartyKey,CONCAT('P?-',p.Id) AS SourceCode FROM LocalDistricts l" +
        " INNER JOIN Elections e ON e.StateCode = l.StateCode" +
        " AND e.LocalKey=l.LocalKey" +
        " AND e.ElectionDate= (SELECT e2.ElectionDate FROM Elections e2" +
        " WHERE e2.StateCode=l.StateCode AND" +
        " e2.LocalKey=l.LocalKey AND e2.ElectionType IN ({0}) AND" +
        " e2.ElectionDate{1}UTC_DATE() {2} ORDER BY e2.ElectionDate {3}" +
        " LIMIT 1) INNER JOIN ElectionsPoliticians ep" +
        "  ON ep.ElectionKey=e.ElectionKey AND ep.OfficeKey <> 'USPresident'" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey {4}";

      // Substitutions:
      // {0} = electionTypes (single-quoted, comma separated)
      // {1} = useFutureElections ? ">=" : "<"
      // {2} = viewableOnly ? "AND IsViewable=1" : Empty
      // {3} = useFutureElections ? "ASC" : "DESC"
      // {4} = WHERE clause

      var cmdText = Format(cmdTemplate, "'" + Join("','", electionTypes) + "'",
        useFutureElections ? ">=" : "<", viewableOnly ? "AND IsViewable=1" : Empty,
        useFutureElections ? "ASC" : "DESC", whereClause);

      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateStateCandidatesBatchWithNoElectionFiltering(int batchId,
      string[] stateCodes, string[] politicianEmailsToUse,
      EmailTypeBatchOptions emailTypeOptions, LoginDateBatchOptions loginDateOptions,
      int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");
      var all = stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase);

      const string cmdTemplate =
        "SELECT StateCode,PoliticianKey,EmailAddrVoteUSA,EmailAddr,StateEmailAddr," +
        "CampaignEmail," +
        //"LDSEmailAddr," + 
        "Fname,Mname,Lname,Nickname,Suffix,Phone," +
        //"StatePhone,LDSPhone,PartyKey FROM Politicians" +
        "StatePhone,PartyKey,CONCAT('P?-',Id) AS SourceCode FROM Politicians" + " {0}";

      var whereClause =
        all ? Empty : "WHERE StateCode IN ('" + Join("','", stateCodes) + "') AND OptOut=0";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);
      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateCandidatesBatchByElection(int batchId,
      string[] politicianEmailsToUse, string electionKey, bool includeAllElectionsOnDate,
      bool includeAllJurisdictions, EmailTypeBatchOptions emailTypeOptions, string adFiltering,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      const string cmdTemplate =
        "SELECT e.StateCode,e.CountyCode,e.LocalKey,e.ElectionKey,e.ElectionKeyToInclude,e.ElectionKeyToInclude,ep.OfficeKey,ep.PoliticianKey," +
        "o.OfficeLine1,o.OfficeLine2,p.EmailAddrVoteUSA," +
        "p.EmailAddr,p.StateEmailAddr,p.CampaignEmail,p.Fname,p.Mname,p.Lname,p.Nickname,p.Suffix," +
        "p.Phone,p.StatePhone,p.PartyKey,CONCAT('P?-',p.Id) AS SourceCode FROM ElectionsPoliticians ep" +
        " INNER JOIN Elections e ON e.ElectionKey = ep.ElectionKey" +
        " INNER JOIN Politicians p ON p.PoliticianKey = ep.PoliticianKey AND p.OptOut=0" +
        " INNER JOIN Offices o ON o.OfficeKey = ep.OfficeKey" + 
        " WHERE ep.ElectionKey{0}";

      var whereConditions = new List<string>();
      if (includeAllJurisdictions)
        whereConditions.Add(includeAllElectionsOnDate
          ? " LIKE '" + electionKey.Substring(0, 10) + "%'"
          : " LIKE '" + electionKey.Substring(0, 12) + "%'");
      else
        whereConditions.Add(includeAllElectionsOnDate
          ? " LIKE '" + electionKey.Substring(0, 10) + "__" + electionKey.Substring(12) +
          "'"
          : "='" + electionKey + "'");
      switch (adFiltering)
      {
        case "NoFiltering":
          break;

        case "HasEnabled":
          whereConditions.Add("(ep.AdType!='' AND ep.AdEnabled=1)");
          break;

        case "HasDisabled":
          whereConditions.Add("(ep.AdType!='' AND ep.AdEnabled=0)");
          break;

        case "HasNone":
          whereConditions.Add("ep.AdType=''");
          break;
      }
      var whereCondition = Join(" AND ", whereConditions);

      // Substitutions:
      // {0} = WHERE condition

      var cmdText = Format(cmdTemplate, whereCondition);
      return BuildCandidatesEmailBatch(batchId, cmdText, politicianEmailsToUse,
        emailTypeOptions, loginDateOptions);
    }

    public static int CreateStatesPartiesBatchByState(int batchId, string[] stateCodes,
      string[] partyKeys, bool majorParties, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      // build WHERE clause
      if (stateCodes.Length == 0) stateCodes = new[] {Empty};
      var whereClause = stateCodes.Length == 1
        ? "WHERE p.StateCode = '" + stateCodes[0] + "'"
        : "WHERE " + stateCodes.SqlIn("p.StateCode");

      var allParties = partyKeys.Length == 1 && partyKeys[0] == "all";
      if (partyKeys.Length == 0) partyKeys = new[] {Empty};
      if (!allParties && !majorParties)
        whereClause += " AND " + partyKeys.SqlIn("p.PartyKey");

      if (majorParties) whereClause += " AND  p.IsPartyMajor=1";
      whereClause += " AND pe.IsVolunteer=0 AND pe.OptOut=0";

      const string cmdTemplate =
        "SELECT pe.Id,pe.PartyContactFName,pe.PartyContactLName,pe.PartyContactPhone," +
        "pe.PartyContactTitle,pe.PartyEmail,pe.PartyKey,p.StateCode FROM Parties p" +
        " INNER JOIN PartiesEmails pe ON pe.PartyKey=p.PartyKey {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText, emailTypeOptions, loginDateOptions);
    }

    public static int CreateVolunteersBatch(int batchId, string[] stateCodes,
      string[] partyKeys, bool majorParties, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      if (stateCodes == null || stateCodes.Length == 0)
        throw new VoteException("No states were supplied");

      // expand "all" states
      if (stateCodes.Length == 1 &&
        stateCodes[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        stateCodes = StateCache.All51StateCodes.ToArray();

      // build WHERE clause
      if (stateCodes.Length == 0) stateCodes = new[] {Empty};
      var whereClause = stateCodes.Length == 1
        ? "WHERE p.StateCode = '" + stateCodes[0] + "'"
        : "WHERE " + stateCodes.SqlIn("p.StateCode");

      var allParties = partyKeys.Length == 1 && partyKeys[0] == "all";
      if (partyKeys.Length == 0) partyKeys = new[] {Empty};
      if (!allParties && !majorParties)
        whereClause += " AND " + partyKeys.SqlIn("p.PartyKey");

      if (majorParties) whereClause += " AND  p.IsPartyMajor=1";
      whereClause += " AND pe.IsVolunteer=1";

      const string cmdTemplate =
        "SELECT pe.Id,pe.PartyContactFName,pe.PartyContactLName,pe.PartyContactPhone," +
        "pe.PartyContactTitle,pe.PartyEmail,pe.PartyKey,p.StateCode FROM Parties p" +
        " INNER JOIN PartiesEmails pe ON pe.PartyKey=p.PartyKey {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText, emailTypeOptions, loginDateOptions);
    }

    public static int CreateStatesPartiesBatchByElection(int batchId, string electionKey,
      bool includeAllElectionsOnDate, EmailTypeBatchOptions emailTypeOptions,
      LoginDateBatchOptions loginDateOptions, int commandTimeout = -1)
    {
      // build WHERE clause
      var whereClause = includeAllElectionsOnDate
        ? "WHERE e.ElectionKey LIKE '" + electionKey.Substring(0, 11) + "_" +
        electionKey.Substring(12) + "'"
        : "WHERE e.ElectionKey = '" + electionKey + "'";
      whereClause += " AND pe.IsVolunteer=0 AND pe.OptOut=0";

      const string cmdTemplate =
        "SELECT e.ElectionKey,e.ElectionKeyToInclude,e.StateCode,pe.Id,pe.PartyContactFName," +
        "pe.PartyContactLName,pe.PartyContactPhone,pe.PartyContactTitle," +
        "pe.PartyEmail,pe.PartyKey FROM Elections e" +
        " INNER JOIN PartiesEmails pe ON pe.PartyKey=" +
        "CONCAT(SUBSTRING(e.ElectionKey, 1, 2),SUBSTRING(e.ElectionKey, 12, 1))" + " {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);

      return BuildPartiesEmailBatch(batchId, cmdText, emailTypeOptions, loginDateOptions);
    }

    public static int CreateVisitorsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, VisitorBatchOptions visitorOptions,
      EmailTypeBatchOptions emailTypeOptions)
    {
      var beginTime = IsNullOrWhiteSpace(visitorOptions.StartDate)
        ? DateTime.MinValue
        : DateTime.Parse(visitorOptions.StartDate);
      var endTime = IsNullOrWhiteSpace(visitorOptions.EndDate)
        ? DateTime.MaxValue
        : DateTime.Parse(visitorOptions.EndDate) + new TimeSpan(1, 0, 0, 0); // add a day

      // build WHERE clause
      var whereClause = Addresses.FormatEmailBatchesWhereClause(stateCodes, countyCodes,
        visitorOptions.SampleBallots, visitorOptions.SharedChoices, visitorOptions.Donated,
        visitorOptions.WithNames, visitorOptions.WithoutNames,
        visitorOptions.WithDistrictCoding, visitorOptions.WithoutDistrictCoding, beginTime,
        endTime, visitorOptions.DistrictFiltering, visitorOptions.Districts);

      const string cmdTemplate =
        "SELECT Id,FirstName,LastName,StateCode,Email,County AS CountyCode,Phone" +
        " FROM Addresses {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);

      return BuildVisitorsEmailBatch(batchId, cmdText, beginTime, endTime,
        emailTypeOptions);
    }

    public static int CreateDonorsBatch(int batchId, string[] stateCodes,
      string[] countyCodes, VisitorBatchOptions visitorOptions,
      DonorBatchOptions donorOptions, EmailTypeBatchOptions emailTypeOptions)
    {
      var beginTime = IsNullOrWhiteSpace(donorOptions.StartDate)
        ? DateTime.MinValue
        : DateTime.Parse(donorOptions.StartDate);
      var endTime = IsNullOrWhiteSpace(donorOptions.EndDate)
        ? DateTime.MaxValue
        : DateTime.Parse(donorOptions.EndDate) + new TimeSpan(1, 0, 0, 0); // add a day

      // build WHERE clause
      var whereClause = DonorsView.FormatEmailBatchesWhereClause(stateCodes, countyCodes,
        beginTime, endTime, visitorOptions.DistrictFiltering, visitorOptions.Districts,
        donorOptions.DistrictCoding);

      const string cmdTemplate = "SELECT * FROM DonorsView {0}";

      // Substitutions:
      // {0} = WHERE clause

      var cmdText = Format(cmdTemplate, whereClause);

      return BuildDonorsEmailBatch(batchId, cmdText, beginTime, endTime, emailTypeOptions);
    }

    public static int CreateOrganizationsBatch(int batchId, string[] stateCodes,
      OrgBatchOptions orgOptions, EmailTypeBatchOptions emailTypeOptions)
    {
      var emailTagsJoin = Empty;
      if (orgOptions.EmailTagIds.Length > 0)
        emailTagsJoin = "INNER JOIN OrganizationAssignedEmailTags ot" + 
          $" ON ot.OrgId=o.OrgId AND {orgOptions.EmailTagIds.SqlIn("ot.EmailTagId")}";

      var whereElements = new List<string> {$"o.OrgTypeId={orgOptions.OrgTypeId}" };
      if (orgOptions.OrgSubTypeId != 0)
        whereElements.Add($"o.OrgSubTypeId={orgOptions.OrgSubTypeId}");
      if (orgOptions.IdeologyId != 0)
        whereElements.Add($"o.IdeologyId={orgOptions.IdeologyId}");
      if (stateCodes != null && stateCodes.Length > 0 && stateCodes[0] != "all")
        whereElements.Add(stateCodes.SqlIn("o.StateCode"));

      var cmdText = "SELECT o.OrgId,o.Name,o.OrgAbbreviation,o.StateCode,oc.Phone,oc.ContactId," + 
        "oc.Contact,oc.Email,oc.Title FROM Organizations o" +
        $" INNER JOIN OrganizationContacts oc ON oc.OrgId=o.OrgId {emailTagsJoin}" + 
        $" WHERE {Join(" AND ", whereElements)}" + 
        " GROUP BY o.OrgId,oc.ContactId ORDER BY o.Name,o.OrgId,oc.ContactOrder";

      return BuildOrganizationsEmailBatch(batchId, cmdText, orgOptions.UseAllContacts,
        emailTypeOptions);
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