using System;
using System.Data;
using DB.Vote;

namespace DB
{
  public static class DataRowExtensions
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static bool ContainsColumn(this DataRow row, string columnName)
    {
      return row.Table.Columns.Contains(columnName);
    }

    //public static string Accomplishments(this DataRow row)
    //{
    //  return row["Accomplishments"] as string;
    //}

    public static string AddOn(this DataRow row)
    {
      return row["AddOn"] as string;
    }

    public static string Address(this DataRow row)
    {
      return row["Address"] as string;
    }

    public static bool AdvanceToRunoff(this DataRow row, bool defaultValue = default(bool))
    {
      if (row.IsNull("AdvanceToRunoff")) return defaultValue;
      return Convert.ToInt32(row["AdvanceToRunoff"]) != 0;
    }

    public static string Age(this DataRow row)
    {
      return Politicians.GetAgeFromDateTime(row.DateOfBirth());
    }

    public static string AlphaName(this DataRow row)
    {
      return row["AlphaName"] as string;
    }

    public static string AltContact(this DataRow row)
    {
      return row["AltContact"] as string;
    }

    public static string AltContactTitle(this DataRow row)
    {
      return row["AltContactTitle"] as string;
    }

    public static string AltEmail(this DataRow row)
    {
      return row["AltEmail"] as string;
    }

    public static string AltPhone(this DataRow row)
    {
      return row["AltPhone"] as string;
    }

    public static OfficeClass AlternateOfficeClass(this DataRow row,
      OfficeClass defaultValue = Vote.OfficeClass.Undefined)
    {
      return row.IsNull("AlternateOfficeLevel")
        ? defaultValue
        : Convert.ToInt32(row["AlternateOfficeLevel"]).ToOfficeClass();
    }

    public static int AlternateOfficeLevel(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("AlternateOfficeLevel")
        ? defaultValue
        : Convert.ToInt32(row["AlternateOfficeLevel"]);
    }

    public static string Answer(this DataRow row)
    {
      return row["Answer"] as string;
    }

    public static int AnswerCount(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("AnswerCount")
        ? defaultValue
        : Convert.ToInt32(row["AnswerCount"]);
    }

    public static DateTime AnswerDate(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.Table.Columns.Contains("AnswerDate"))
      {
        if (row.IsNull("AnswerDate")) return defaultValue;
        return (DateTime) row["AnswerDate"];
      }
      if (row.Table.Columns.Contains("DateStamp"))
      {
        if (row.IsNull("DateStamp")) return defaultValue;
        return (DateTime) row["DateStamp"];
      }
      return defaultValue;
    }

    public static int Answers(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("Answers")
        ? defaultValue
        : Convert.ToInt32(row["Answers"]);
    }

    public static string AnswerSource(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("AnswerSource")
        ? row["AnswerSource"]
        : row["Source"]) as string;
    }

    public static string BallotPediaWebAddress(this DataRow row)
    {
      return row["BallotPediaWebAddress"] as string;
    }

    public static string[] BccEmails(this DataRow row)
    {
      var value = row["BccEmails"] as string;
      return value?.Split(new[] {","},
        StringSplitOptions.RemoveEmptyEntries);
    }

    public static string BloggerWebAddress(this DataRow row)
    {
      return row["BloggerWebAddress"] as string;
    }

    public static string Body(this DataRow row)
    {
      return row["Body"] as string;
    }

    public static string CampaignAddress(this DataRow row)
    {
      return row["CampaignAddress"] as string;
    }

    public static string CampaignCityStateZip(this DataRow row)
    {
      return row["CampaignCityStateZip"] as string;
    }

    public static string CampaignEmail(this DataRow row)
    {
      return row["CampaignEmail"] as string;
    }

    public static string CampaignName(this DataRow row)
    {
      return row["CampaignName"] as string;
    }

    public static string CampaignPhone(this DataRow row)
    {
      return row["CampaignPhone"] as string;
    }

    public static int CandidateCountForOffice(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("CandidateCountForOffice")
        ? defaultValue
        : Convert.ToInt32(row["CandidateCountForOffice"]);
    }

    public static string[] CcEmails(this DataRow row)
    {
      var value = row["CcEmails"] as string;
      return value?.Split(new[] {","},
        StringSplitOptions.RemoveEmptyEntries);
    }

    public static string CityStateZip(this DataRow row)
    {
      return row["CityStateZip"] as string;
    }

    //public static string Civic(this DataRow row)
    //{
    //  return row["Civic"] as string;
    //}

    public static string Contact(this DataRow row)
    {
      return row["Contact"] as string;
    }

    public static string ContactEmail(this DataRow row)
    {
      return row["ContactEmail"] as string;
    }

    public static string ContactTitle(this DataRow row)
    {
      return row["ContactTitle"] as string;
    }

    public static string ContactType(this DataRow row)
    {
      return row["ContactType"] as string;
    }

    public static string County(this DataRow row)
    {
      return row["County"] as string;
    }

    public static string CountyCode(this DataRow row)
    {
      return row["CountyCode"] as string;
    }

    public static DateTime DataLastUpdated(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("DataLastUpdated")) return defaultValue;
      return (DateTime) row["DataLastUpdated"];
    }

    public static int DataUpdatedCount(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("DataUpdatedCount")
        ? defaultValue
        : Convert.ToInt32(row["DataUpdatedCount"]);
    }

    public static DateTime DateOfBirth(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("DateOfBirth")) return defaultValue;
      return (DateTime) row["DateOfBirth"];
    }

    public static string DateOfBirthAsString(this DataRow row)
    {
      return Politicians.GetDateOfBirthFromDateTime(row.DateOfBirth());
    }

    public static DateTime DatePictureUploaded(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("DatePictureUploaded")) return defaultValue;
      return (DateTime) row["DatePictureUploaded"];
    }

    public static DateTime DateStamp(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("DateStamp")) return defaultValue;
      return (DateTime) row["DateStamp"];
    }

    public static int DemographicClass(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("DemographicClass")
        ? defaultValue
        : Convert.ToInt32(row["DemographicClass"]);
    }

    public static string Description(this DataRow row)
    {
      return row["Description"] as string;
    }

    public static string DistrictCode(this DataRow row)
    {
      return row["DistrictCode"] as string;
    }

    public static string DistrictCodeAlpha(this DataRow row)
    {
      return row["DistrictCodeAlpha"] as string;
    }

    //public static string Education(this DataRow row)
    //{
    //  return row["Education"] as string;
    //}

    public static string ElectionDescription(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("ElectionDescription")
        ? row["ElectionDescription"]
        : row["ElectionDesc"]) as string;
    }

    public static string ElectionKey(this DataRow row)
    {
      return row["ElectionKey"] as string;
    }

    public static string ElectionKeyToInclude(this DataRow row)
    {
      return row["ElectionKeyToInclude"] as string;
    }

    public static int ElectionPositions(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("ElectionPositions")
        ? defaultValue
        : Convert.ToInt32(row["ElectionPositions"]);
    }

    public static string ElectionsPoliticianKey(this DataRow row)
    {
      return row["ElectionsPoliticianKey"] as string;
    }

    public static string ElectionType(this DataRow row)
    {
      return row["ElectionType"] as string;
    }

    public static string Email(this DataRow row)
    {
      object obj;
      if (row.Table.Columns.Contains("Email"))
        obj = row["Email"];
      else if (row.Table.Columns.Contains("EmailAddr"))
        obj = row["EmailAddr"];
      else
        obj = row["EmailAddress"];
      return obj as string;
    }

    public static string EmailVoteUsa(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("EmailVoteUSA")
        ? row["EmailVoteUSA"]
        : row["EmailAddrVoteUSA"]) as string;
    }

    public static string EnquotedNickname(this DataRow row)
    {
      return Politicians.GetEnquotedNicknameForState(row.Nickname(),
        Politicians.GetStateCodeFromKey(row.PoliticianKey()));
    }

    public static string ErrorMessage(this DataRow row)
    {
      return row["ErrorMessage"] as string;
    }

    public static string FacebookWebAddress(this DataRow row)
    {
      return row["FacebookWebAddress"] as string;
    }

    public static string FirstName(this DataRow row)
    {
      return
        (row.Table.Columns.Contains("FirstName") ? row["FirstName"] : row["FName"])
          as string;
    }

    public static string FlickrWebAddress(this DataRow row)
    {
      return row["FlickrWebAddress"] as string;
    }

    public static int ForwardedCount(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("ForwardedCount")
        ? defaultValue
        : Convert.ToInt32(row["ForwardedCount"]);
    }

    public static string FromEmail(this DataRow row)
    {
      return row["FromEmail"] as string;
    }

    public static string FullName(this DataRow row)
    {
      return row["FullName"] as string;
    }

    public static string Gender(this DataRow row)
    {
      return row["Gender"] as string;
    }

    public static int GeneralRunoffPositions(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("GeneralRunoffPositions")
        ? defaultValue
        : Convert.ToInt32(row["GeneralRunoffPositions"]);
    }

    //public static string GeneralStatement(this DataRow row)
    //{
    //  return row["GeneralStatement"] as string;
    //}

    public static string GooglePlusWebAddress(this DataRow row)
    {
      return row["GooglePlusWebAddress"] as string;
    }

    //public static bool HasBioData(this DataRow row,
    //  bool defaultValue = default(bool))
    //{
    //  if (row.IsNull("IsHasBioData")) return defaultValue;
    //  return Convert.ToInt32(row["IsHasBioData"]) != 0;
    //}

    //public static bool HasCompleteLDSAddress(this DataRow row)
    //{
    //  return !string.IsNullOrWhiteSpace(row.LDSAddress()) &&
    //    !string.IsNullOrWhiteSpace(row.LDSCityStateZip());
    //}

    public static bool HasCompleteStateAddress(this DataRow row)
    {
      return !string.IsNullOrWhiteSpace(row.StateAddress()) &&
        !string.IsNullOrWhiteSpace(row.StateCityStateZip());
    }

    public static int Id(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("Id")
        ? defaultValue
        : Convert.ToInt32(row["Id"]);
    }

    public static int Incumbents(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("Incumbents")
        ? defaultValue
        : Convert.ToInt32(row["Incumbents"]);
    }

    public static DateTime IntroLetterSent(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("IntroLetterSent")) return defaultValue;
      return (DateTime) row["IntroLetterSent"];
    }

    public static bool IsFlagged(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsFlagged")) return defaultValue;
      return Convert.ToInt32(row["IsFlagged"]) != 0;
    }

    public static bool IsInactive(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsInactive")) return defaultValue;
      return Convert.ToInt32(row["IsInactive"]) != 0;
    }

    public static bool IsInactiveOptional(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (!row.ContainsColumn("IsInactive") || row.IsNull("IsInactive")) return defaultValue;
      return Convert.ToInt32(row["IsInactive"]) != 0;
    }

    public static bool IsIncumbent(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsIncumbent")) return defaultValue;
      return Convert.ToInt32(row["IsIncumbent"]) != 0;
    }

    public static bool IsIncumbentRow(this DataRow row,
      bool defaultValue = default(bool))
    {
      // Pseudo colummn on some queries
      if (row.IsNull("IsIncumbentRow")) return defaultValue;
      return Convert.ToInt32(row["IsIncumbentRow"]) != 0;
    }

    //public static bool IsLDSIncumbent(this DataRow row,
    //  bool defaultValue = default(bool))
    //{
    //  if (row.IsNull("IsLDSIncumbent")) return defaultValue;
    //  return Convert.ToInt32(row["IsLDSIncumbent"]) != 0;
    //}

    public static bool IsNotRespondedEmailSent(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsNotRespondedEmailSent")) return defaultValue;
      return Convert.ToInt32(row["IsNotRespondedEmailSent"]) != 0;
    }

    public static bool IsOfficeInElection(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsOfficeInElection")) return defaultValue;
      return Convert.ToInt32(row["IsOfficeInElection"]) == 1;
    }

    public static bool IsOfficeTagForDeletion(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsOfficeTagForDeletion")) return defaultValue;
      return Convert.ToInt32(row["IsOfficeTagForDeletion"]) != 0;
    }

    public static bool IsOnlyForPrimaries(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsOnlyForPrimaries")) return defaultValue;
      return Convert.ToInt32(row["IsOnlyForPrimaries"]) != 0;
    }

    public static bool IsPartyMajor(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsPartyMajor")) return defaultValue;
      return Convert.ToInt32(row["IsPartyMajor"]) != 0;
    }

    public static bool IsPassed(this DataRow row, bool defaultValue = default(bool))
    {
      if (row.IsNull("IsPassed")) return defaultValue;
      return Convert.ToInt32(row["IsPassed"]) != 0;
    }

    public static bool IsResultRecorded(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsResultRecorded")) return defaultValue;
      return Convert.ToInt32(row["IsResultRecorded"]) != 0;
    }

    public static bool IsRunningMate(this DataRow row,
      bool defaultValue = default(bool))
    {
      // Pseudo colummn on some queries
      if (row.IsNull("IsRunningMate")) return defaultValue;
      return Convert.ToInt32(row["IsRunningMate"]) != 0;
    }

    public static bool IsRunningMateOffice(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsRunningMateOffice")) return defaultValue;
      return Convert.ToInt32(row["IsRunningMateOffice"]) != 0;
    }

    public static bool IsSpecialOffice(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("IsSpecialOffice")) return defaultValue;
      return Convert.ToInt32(row["IsSpecialOffice"]) != 0;
    }

    public static string Issue(this DataRow row)
    {
      return row["Issue"] as string;
    }

    public static string IssueKey(this DataRow row)
    {
      return row["IssueKey"] as string;
    }

    public static string IssueLevel(this DataRow row)
    {
      return row["IssueLevel"] as string;
    }

    public static bool IsVacant(this DataRow row, bool defaultValue = default(bool))
    {
      if (row.IsNull("IsVacant")) return defaultValue;
      return Convert.ToInt32(row["IsVacant"]) != 0;
    }

    public static bool IsVirtual(this DataRow row, bool defaultValue = default(bool))
    {
      if (row.IsNull("IsVirtual")) return defaultValue;
      return Convert.ToInt32(row["IsVirtual"]) != 0;
    }

    public static bool IsWinner(this DataRow row, bool defaultValue = default(bool))
    {
      if (row.IsNull("IsWinner")) return defaultValue;
      return Convert.ToInt32(row["IsWinner"]) != 0;
    }

    public static string LastEmailCode(this DataRow row)
    {
      return row["LastEmailCode"] as string;
    }

    public static string LastName(this DataRow row)
    {
      return
        (row.Table.Columns.Contains("LastName") ? row["LastName"] : row["LName"])
          as string;
    }

    //public static string LDSAddress(this DataRow row)
    //{
    //  return row["LDSAddress"] as string;
    //}

    //public static string LDSCityStateZip(this DataRow row)
    //{
    //  return row["LDSCityStateZip"] as string;
    //}

    //public static string LDSDistrictCode(this DataRow row)
    //{
    //  return row["LDSDistrictCode"] as string;
    //}

    //public static string LDSEmail(this DataRow row)
    //{
    //  return
    //    (row.Table.Columns.Contains("LDSEmail")
    //      ? row["LDSEmail"]
    //      : row["LDSEmailAddr"]) as string;
    //}

    //public static string LDSGender(this DataRow row)
    //{
    //  return row["LDSGender"] as string;
    //}

    //public static string LDSLegIDNum(this DataRow row)
    //{
    //  return row["LDSLegIDNum"] as string;
    //}

    //public static string LDSOffice(this DataRow row)
    //{
    //  return row["LDSOffice"] as string;
    //}

    //public static string LDSPartyCode(this DataRow row)
    //{
    //  return row["LDSPartyCode"] as string;
    //}

    //public static string LDSPhone(this DataRow row)
    //{
    //  return row["LDSPhone"] as string;
    //}

    //public static string LDSPoliticianName(this DataRow row)
    //{
    //  return row["LDSPoliticianName"] as string;
    //}

    public static string LdsStateCode(this DataRow row)
    {
      return row["LDSStateCode"] as string;
    }

    //public static string LDSTypeCode(this DataRow row)
    //{
    //  return row["LDSTypeCode"] as string;
    //}

    //public static DateTime LDSUpdateDate(this DataRow row,
    //  DateTime defaultValue = default(DateTime))
    //{
    //  if (row.IsNull("LDSUpdateDate")) return defaultValue;
    //  return (DateTime) row["LDSUpdateDate"];
    //}

    //public static string LDSVersion(this DataRow row)
    //{
    //  return row["LDSVersion"] as string;
    //}

    //public static string LDSWebAddress(this DataRow row)
    //{
    //  return
    //    (row.Table.Columns.Contains("LDSWebAddress")
    //      ? row["LDSWebAddress"]
    //      : row["LDSWebAddr"]) as string;
    //}

    public static string LinkedInWebAddress(this DataRow row)
    {
      return row["LinkedInWebAddress"] as string;
    }

    public static string LiveElectionKey(this DataRow row)
    {
      return row["LiveElectionKey"] as string;
    }

    public static string LiveOfficeKey(this DataRow row)
    {
      return row["LiveOfficeKey"] as string;
    }

    public static string LiveOfficeStatus(this DataRow row)
    {
      return row["LiveOfficeStatus"] as string;
    }

    public static PoliticianStatus LivePoliticianStatus(this DataRow row)
    {
      return row.LiveOfficeStatus().ToPoliticianStatus();
    }

    public static string LocalCode(this DataRow row)
    {
      return row["LocalCode"] as string;
    }

    public static string LocalDistrict(this DataRow row)
    {
      return row["LocalDistrict"] as string;
    }

    public static string MiddleName(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("MiddleName")
        ? row["MiddleName"]
        : row["MName"]) as string;
    }

    //public static string Military(this DataRow row)
    //{
    //  return row["Military"] as string;
    //}

    public static string Name(this DataRow row)
    {
      return row["Name"] as string;
    }

    public static string NationalPartyCode(this DataRow row)
    {
      return row["NationalPartyCode"] as string;
    }

    public static string Nickname(this DataRow row)
    {
      return row["Nickname"] as string;
    }

    public static OfficeClass OfficeClass(this DataRow row,
      OfficeClass defaultValue = Vote.OfficeClass.Undefined)
    {
      var column = row.Table.Columns.Contains("OfficeLevel")
        ? "OfficeLevel"
        : "OfficeClass";
      return row.IsNull(column)
        ? defaultValue
        : Convert.ToInt32(row[column]).ToOfficeClass();
    }

    public static string OfficeKey(this DataRow row)
    {
      return row["OfficeKey"] as string;
    }

    public static int OfficeLevel(this DataRow row, int defaultValue = default(int))
    {
      var column = row.Table.Columns.Contains("OfficeLevel")
        ? "OfficeLevel"
        : "OfficeClass";
      return row.IsNull(column)
        ? defaultValue
        : Convert.ToInt32(row[column]);
    }

    public static string OfficeLine1(this DataRow row)
    {
      return row["OfficeLine1"] as string;
    }

    public static string OfficeLine2(this DataRow row)
    {
      return row["OfficeLine2"] as string;
    }

    public static int OfficeOrder(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("OfficeOrder")
        ? defaultValue
        : Convert.ToInt32(row["OfficeOrder"]);
    }

    public static int OfficeOrderWithinLevel(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("OfficeOrderWithinLevel")
        ? defaultValue
        : Convert.ToInt32(row["OfficeOrderWithinLevel"]);
    }

    public static string OfficialsPoliticianKey(this DataRow row)
    {
      // pseudo column on some queries
      return row["OfficialsPoliticianKey"] as string;
    }

    public static int OrderOnBallot(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("OrderOnBallot")
        ? defaultValue
        : Convert.ToInt32(row["OrderOnBallot"]);
    }

    public static string PartyAddressLine1(this DataRow row)
    {
      return row["PartyAddressLine1"] as string;
    }

    public static string PartyAddressLine2(this DataRow row)
    {
      return row["PartyAddressLine2"] as string;
    }

    public static string PartyCityStateZip(this DataRow row)
    {
      return row["PartyCityStateZip"] as string;
    }

    public static string PartyCode(this DataRow row)
    {
      return row["PartyCode"] as string;
    }

    public static string PartyContactFName(this DataRow row)
    {
      return row["PartyContactFName"] as string;
    }

    public static string PartyContactLName(this DataRow row)
    {
      return row["PartyContactLName"] as string;
    }

    public static string PartyContactPhone(this DataRow row)
    {
      return row["PartyContactPhone"] as string;
    }

    public static string PartyContactTitle(this DataRow row)
    {
      return row["PartyContactTitle"] as string;
    }

    public static string PartyEmail(this DataRow row)
    {
      return row["PartyEmail"] as string;
    }

    public static string PartyKey(this DataRow row)
    {
      return row["PartyKey"] as string;
    }

    public static string PartyName(this DataRow row)
    {
      return row["PartyName"] as string;
    }

    public static int PartyOrder(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("PartyOrder")
        ? defaultValue
        : Convert.ToInt32(row["PartyOrder"]);
    }

    public static string PartyUrl(this DataRow row)
    {
      return row["PartyUrl"] as string;
    }

    public static string Password(this DataRow row)
    {
      return row["Password"] as string;
    }

    public static string PasswordHint(this DataRow row)
    {
      return row["PasswordHint"] as string;
    }

    //public static string Personal(this DataRow row)
    //{
    //  return row["Personal"] as string;
    //}

    public static string Phone(this DataRow row)
    {
      return row["Phone"] as string;
    }

    public static string PinterestWebAddress(this DataRow row)
    {
      return row["PinterestWebAddress"] as string;
    }

    //public static string Political(this DataRow row)
    //{
    //  return row["Political"] as string;
    //}

    public static string PoliticianKey(this DataRow row)
    {
      return row["PoliticianKey"] as string;
    }

    public static int PrimaryPositions(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("PrimaryPositions")
        ? defaultValue
        : Convert.ToInt32(row["PrimaryPositions"]);
    }

    public static int PrimaryRunoffPositions(this DataRow row, int defaultValue = default(int))
    {
      return row.IsNull("PrimaryRunoffPositions")
        ? defaultValue
        : Convert.ToInt32(row["PrimaryRunoffPositions"]);
    }

    //public static string Profession(this DataRow row)
    //{
    //  return row["Profession"] as string;
    //}

    public static string PublicAddress(this DataRow row)
    {
      var result = row.Address();
      if (string.IsNullOrWhiteSpace(result)) result = row.StateAddress();
      return result;
    }

    public static string PublicCityStateZip(this DataRow row)
    {
      var result = row.CityStateZip();
      if (string.IsNullOrWhiteSpace(result)) result = row.StateCityStateZip();
      return result;
    }

    public static string PublicEmail(this DataRow row)
    {
      var result = row.Email();
      if (string.IsNullOrWhiteSpace(result)) result = row.StateEmail();
      return result;
    }

    public static string PublicPhone(this DataRow row)
    {
      var result = row.Phone();
      if (string.IsNullOrWhiteSpace(result)) result = row.StatePhone();
      return result;
    }

    public static string PublicWebAddress(this DataRow row)
    {
      var result = row.WebAddress();
      if (string.IsNullOrWhiteSpace(result)) result = row.StateWebAddress();
      return result;
    }

    public static string Question(this DataRow row)
    {
      return row["Question"] as string;
    }

    public static string QuestionKey(this DataRow row)
    {
      return row["QuestionKey"] as string;
    }

    public static string ReferendumDescription(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("ReferendumDescription")
        ? row["ReferendumDescription"]
        : row["ReferendumDesc"]) as string;
    }

    public static string ReferendumKey(this DataRow row)
    {
      return row["ReferendumKey"] as string;
    }

    public static string ReferendumTitle(this DataRow row)
    {
      return row["ReferendumTitle"] as string;
    }

    //public static string Religion(this DataRow row)
    //{
    //  return row["Religion"] as string;
    //}

    public static string RssFeedWebAddress(this DataRow row)
    {
      return row["RSSFeedWebAddress"] as string;
    }

    public static string RunningMateKey(this DataRow row)
    {
      return row["RunningMateKey"] as string;
    }

    public static string SelectionCriteria(this DataRow row)
    {
      return row["SelectionCriteria"] as string;
    }

    public static DateTime SentTime(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("SentTime")) return defaultValue;
      return (DateTime) row["SentTime"];
    }

    public static int Sequence(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("Sequence")
        ? defaultValue
        : Convert.ToInt32(row["Sequence"]);
    }

    public static string SortIssueLevel(this DataRow row)
    {
      return row["SortIssueLevel"] as string;
    }

    public static string Source(this DataRow row)
    {
      return row["Source"] as string;
    }

    public static string StateAddress(this DataRow row)
    {
      return row["StateAddress"] as string;
    }

    public static string StateCityStateZip(this DataRow row)
    {
      return row["StateCityStateZip"] as string;
    }

    public static string StateCode(this DataRow row)
    {
      return row["StateCode"] as string;
    }

    public static string StateData(this DataRow row)
    {
      return row["StateData"] as string;
    }

    public static string StateEmail(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("StateEmail")
        ? row["StateEmail"]
        : row["StateEmailAddr"]) as string;
    }

    public static string StateOrCandidateAddress(this DataRow row)
    {
      if (row.HasCompleteStateAddress())
        return row.StateAddress();
      return row.Address();
    }

    public static string StateOrCandidateCityStateZip(this DataRow row)
    {
      if (row.HasCompleteStateAddress())
        return row.StateCityStateZip();
      return row.CityStateZip();
    }

    public static string StateOrCandidateEmail(this DataRow row)
    {
      var result = row.StateEmail();
      if (string.IsNullOrWhiteSpace(result))
        result = row.Email();
      return result;
    }

    public static string StateOrCandidateWebAddress(this DataRow row)
    {
      var result = row.StateWebAddress();
      if (string.IsNullOrWhiteSpace(result))
        result = row.WebAddress();
      return result;
    }

    public static string StateOrCandidatePhone(this DataRow row)
    {
      var result = row.StatePhone();
      if (string.IsNullOrWhiteSpace(result))
        result = row.Phone();
      return result;
    }

    public static string StatePhone(this DataRow row)
    {
      return row["StatePhone"] as string;
    }

    public static string StateWebAddress(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("StateWebAddress")
        ? row["StateWebAddress"]
        : row["StateWebAddr"]) as string;
    }

    public static string Subject(this DataRow row)
    {
      return row["Subject"] as string;
    }

    public static string Suffix(this DataRow row)
    {
      return row["Suffix"] as string;
    }

    public static string TemporaryOfficeKey(this DataRow row)
    {
      return row["TemporaryOfficeKey"] as string;
    }

    public static string ToEmail(this DataRow row)
    {
      return row["ToEmail"] as string;
    }

    public static string TwitterWebAddress(this DataRow row)
    {
      return row["TwitterWebAddress"] as string;
    }

    public static string UserCountyCode(this DataRow row)
    {
      return row["UserCountyCode"] as string;
    }

    public static string UserLocalCode(this DataRow row)
    {
      return row["UserLocalCode"] as string;
    }

    public static string UserName(this DataRow row)
    {
      return row["UserName"] as string;
    }

    public static string UserPassword(this DataRow row)
    {
      return row["UserPassword"] as string;
    }

    public static string UserSecurity(this DataRow row)
    {
      return row["UserSecurity"] as string;
    }

    public static string UserStateCode(this DataRow row)
    {
      return row["UserStateCode"] as string;
    }

    public static string VimeoWebAddress(this DataRow row)
    {
      return row["VimeoWebAddress"] as string;
    }

    public static int VisitorId(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("VisitorId")
        ? defaultValue
        : Convert.ToInt32(row["VisitorId"]);
    }

    public static string VoteForWording(this DataRow row)
    {
      return row["VoteForWording"] as string;
    }

    public static string VoteInstructions(this DataRow row)
    {
      return row["VoteInstructions"] as string;
    }

    public static string VowelStrippedName(this DataRow row)
    {
      return row["VowelStrippedName"] as string;
    }

    public static bool WasSent(this DataRow row,
      bool defaultValue = default(bool))
    {
      if (row.IsNull("WasSent")) return defaultValue;
      return Convert.ToInt32(row["WasSent"]) != 0;
    }

    public static string WebAddress(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("WebAddress")
        ? row["WebAddress"]
        : row["WebAddr"]) as string;
    }

    public static string WebstagramWebAddress(this DataRow row)
    {
      return row["WebstagramWebAddress"] as string;
    }

    public static string WikipediaWebAddress(this DataRow row)
    {
      return row["WikipediaWebAddress"] as string;
    }

    public static string WriteInInstructions(this DataRow row)
    {
      return row["WriteInInstructions"] as string;
    }

    public static int WriteInLines(this DataRow row,
      int defaultValue = default(int))
    {
      return row.IsNull("WriteInLines")
        ? defaultValue
        : Convert.ToInt32(row["WriteInLines"]);
    }

    public static string WriteInWording(this DataRow row)
    {
      return row["WriteInWording"] as string;
    }

    public static string YouTubeAutoDisable(this DataRow row)
    {
      return row["YouTubeAutoDisable"] as string;
    }

    public static DateTime YouTubeDate(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("YouTubeDate")) return defaultValue;
      return (DateTime) row["YouTubeDate"];
    }

    public static string YouTubeDescription(this DataRow row)
    {
      return row["YouTubeDescription"] as string;
    }

    public static DateTime YouTubeRefreshTime(this DataRow row,
      DateTime defaultValue = default(DateTime))
    {
      if (row.IsNull("YouTubeRefreshTime")) return defaultValue;
      return (DateTime) row["YouTubeRefreshTime"];
    }

    public static TimeSpan YouTubeRunningTime(this DataRow row,
      TimeSpan defaultValue = default(TimeSpan))
    {
      if (row.IsNull("YouTubeRunningTime")) return defaultValue;
      return (TimeSpan) row["YouTubeRunningTime"];
    }

    public static string YouTubeSource(this DataRow row)
    {
      return row["YouTubeSource"] as string;
    }

    public static string YouTubeSourceUrl(this DataRow row)
    {
      return row["YouTubeSourceUrl"] as string;
    }

    public static string YouTubeUrl(this DataRow row)
    {
      return row["YouTubeUrl"] as string;
    }

    public static string YouTubeWebAddress(this DataRow row)
    {
      return row["YouTubeWebAddress"] as string;
    }

    #region ReSharper restore

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