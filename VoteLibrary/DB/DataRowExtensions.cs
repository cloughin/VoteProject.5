using System;
using System.Data;
using System.Linq;
using DB.Vote;
using static System.String;

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

    public static string Accomplishments(this DataRow row)
    {
      return row["Accomplishments"] as string;
    }

    public static string AdDescription1(this DataRow row)
    {
      return row["AdDescription1"] as string;
    }

    public static string AdDescription2(this DataRow row)
    {
      return row["AdDescription2"] as string;
    }

    public static string AdDescriptionUrl(this DataRow row)
    {
      return row["AdDescriptionUrl"] as string;
    }

    public static string AddOn(this DataRow row)
    {
      return row["AddOn"] as string;
    }

    public static string Address(this DataRow row)
    {
      return row["Address"] as string;
    }

    public static string Address1(this DataRow row)
    {
      return row["Address1"] as string;
    }

    public static string Address2(this DataRow row)
    {
      return row["Address2"] as string;
    }

    public static bool AdEnabled(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("AdEnabled")) return defaultValue;
      return Convert.ToInt32(row["AdEnabled"]) != 0;
    }

    public static string AdImageName(this DataRow row)
    {
      return row["AdImageName"] as string;
    }

    public static bool AdIsCandidateSponsored(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("AdIsCandidateSponsored")) return defaultValue;
      return Convert.ToInt32(row["AdIsCandidateSponsored"]) != 0;
    }

    public static bool AdIsPaid(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("AdIsPaid")) return defaultValue;
      return Convert.ToInt32(row["AdIsPaid"]) != 0;
    }

    public static string AdMediaType(this DataRow row)
    {
      return row["AdMediaType"] as string;
    }

    public static string AdSponsor(this DataRow row)
    {
      return row["AdSponsor"] as string;
    }

    public static string AdSponsorUrl(this DataRow row)
    {
      return row["AdSponsorUrl"] as string;
    }

    public static bool AdvanceToRunoff(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("AdvanceToRunoff")) return defaultValue;
      return Convert.ToInt32(row["AdvanceToRunoff"]) != 0;
    }

    public static string AdThumbnailUrl(this DataRow row)
    {
      return row["AdThumbnailUrl"] as string;
    }

    public static string AdType(this DataRow row)
    {
      return row["AdType"] as string;
    }

    public static string AdUrl(this DataRow row)
    {
      return row["AdUrl"] as string;
    }

    public static string AdYouTubeUrl(this DataRow row)
    {
      return row["AdYouTubeUrl"] as string;
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
      int defaultValue = default)
    {
      return row.IsNull("AlternateOfficeLevel")
        ? defaultValue
        : Convert.ToInt32(row["AlternateOfficeLevel"]);
    }

    public static string Answer(this DataRow row)
    {
      return row["Answer"] as string;
    }

    public static int AnswerCount(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("AnswerCount")
        ? defaultValue
        : Convert.ToInt32(row["AnswerCount"]);
    }

    public static DateTime AnswerDate(this DataRow row,
      DateTime defaultValue = default)
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
      int defaultValue = default)
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

    public static string City(this DataRow row)
    {
      return row["City"] as string;
    }

    public static string CityCouncilCode(this DataRow row)
    {
      return row["CityCouncilCode"] as string;
    }

    public static string CityStateZip(this DataRow row)
    {
      return row["CityStateZip"] as string;
    }

    public static string Civic(this DataRow row)
    {
      return row["Civic"] as string;
    }

    public static string Contact(this DataRow row)
    {
      return row["Contact"] as string;
    }

    public static string ContactEmail(this DataRow row)
    {
      return row["ContactEmail"] as string;
    }

    public static int ContactId(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("ContactId")
        ? defaultValue
        : Convert.ToInt32(row["ContactId"]);
    }

    public static string ContactTitle(this DataRow row)
    {
      return row["ContactTitle"] as string;
    }

    public static string ContactType(this DataRow row)
    {
      return row["ContactType"] as string;
    }

    public static int Count(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("Count")
        ? defaultValue
        : Convert.ToInt32(row["Count"]);
    }

    public static string County(this DataRow row)
    {
      return row["County"] as string;
    }

    public static string CountyCode(this DataRow row)
    {
      return row["CountyCode"] as string;
    }

    public static string CountyOrLocal(this DataRow row)
    {
      return row["CountyOrLocal"] as string;
    }

    public static string CountySupervisorsCode(this DataRow row)
    {
      return row["CountySupervisorsCode"] as string;
    }

    public static string CrowdpacWebAddress(this DataRow row)
    {
      return row["CrowdpacWebAddress"] as string;
    }

    public static DateTime DataLastUpdated(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("DataLastUpdated")) return defaultValue;
      return (DateTime) row["DataLastUpdated"];
    }

    public static int DataUpdatedCount(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("DataUpdatedCount")
        ? defaultValue
        : Convert.ToInt32(row["DataUpdatedCount"]);
    }

    public static DateTime DateOfBirth(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("DateOfBirth")) return defaultValue;
      return (DateTime) row["DateOfBirth"];
    }

    public static string DateOfBirthAsString(this DataRow row)
    {
      return Politicians.GetDateOfBirthFromDateTime(row.DateOfBirth());
    }

    public static DateTime DatePictureUploaded(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("DatePictureUploaded")) return defaultValue;
      return (DateTime) row["DatePictureUploaded"];
    }

    public static DateTime DateStamp(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("DateStamp")) return defaultValue;
      return (DateTime)row["DateStamp"];
    }

    public static DateTime? DateStampOrNull(this DataRow row)
    {
      if (row.IsNull("DateStamp")) return null;
      return (DateTime)row["DateStamp"];
    }

    public static string Description(this DataRow row)
    {
      return row["Description"] as string;
    }

    public static string District(this DataRow row)
    {
      return row["District"] as string;
    }

    public static string DistrictCode(this DataRow row)
    {
      return row["DistrictCode"] as string;
    }

    //public static string DistrictCodeAlpha(this DataRow row)
    //{
    //  return row["DistrictCodeAlpha"] as string;
    //}

    public static string Education(this DataRow row)
    {
      return row["Education"] as string;
    }

    public static DateTime ElectionDate(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("ElectionDate")) return defaultValue;
      return (DateTime)row["ElectionDate"];
    }

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

    public static string ElectionKeyState(this DataRow row)
    {
      return row["ElectionKeyState"] as string;
    }

    public static string ElectionKeyToInclude(this DataRow row)
    {
      return row["ElectionKeyToInclude"] as string;
    }

    public static int ElectionOrder(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("ElectionOrder")
        ? defaultValue
        : Convert.ToInt32(row["ElectionOrder"]);
    }

    public static int ElectionPositions(this DataRow row,
      int defaultValue = default)
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

    public static string EmailMission(this DataRow row)
    {
      return row["EmailMission"] as string;
    }

    public static string EmailTag(this DataRow row)
    {
      return row["EmailTag"] as string;
    }

    public static int EmailTagId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("EmailTagId")
        ? defaultValue
        : Convert.ToInt32(row["EmailTagId"]);
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

    public static string FacebookVideoAutoDisable(this DataRow row)
    {
      return row["FacebookVideoAutoDisable"] as string;
    }

    public static DateTime FacebookVideoDate(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("FacebookVideoDate")) return defaultValue;
      return (DateTime) row["FacebookVideoDate"];
    }

    public static string FacebookVideoDescription(this DataRow row)
    {
      return row["FacebookVideoDescription"] as string;
    }

    public static DateTime FacebookVideoRefreshTime(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("FacebookVideoRefreshTime")) return defaultValue;
      return (DateTime) row["FacebookVideoRefreshTime"];
    }

    public static TimeSpan FacebookVideoRunningTime(this DataRow row,
      TimeSpan defaultValue = default)
    {
      if (row.IsNull("FacebookVideoRunningTime")) return defaultValue;
      return (TimeSpan) row["FacebookVideoRunningTime"];
    }

    public static string FacebookVideoUrl(this DataRow row)
    {
      return row["FacebookVideoUrl"] as string;
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

    public static int ForwardedCount(this DataRow row, int defaultValue = default)
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

    public static string FuncStat(this DataRow row)
    {
      return row["FuncStat"] as string;
    }

    public static string Gender(this DataRow row)
    {
      return row["Gender"] as string;
    }

    public static decimal GeneralAdRate(this DataRow row)
    {
      return (decimal) row["GeneralAdRate"];
    }

    public static string GeneralStatement(this DataRow row)
    {
      return row["GeneralStatement"] as string;
    }

    public static int GeneralRunoffPositions(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("GeneralRunoffPositions")
        ? defaultValue
        : Convert.ToInt32(row["GeneralRunoffPositions"]);
    }

    //public static string GeoPhase(this DataRow row)
    //{
    //  return row["GeoPhase"] as string;
    //}

    public static string GoFundMeWebAddress(this DataRow row)
    {
      return row["GoFundMeWebAddress"] as string;
    }

    public static string GooglePlusWebAddress(this DataRow row)
    {
      return row["GooglePlusWebAddress"] as string;
    }

    public static bool HasAdImage(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("HasAdImage")) return defaultValue;
      return Convert.ToInt32(row["HasAdImage"]) != 0;
    }

    public static bool HasCompleteStateAddress(this DataRow row)
    {
      return !IsNullOrWhiteSpace(row.StateAddress()) &&
        !IsNullOrWhiteSpace(row.StateCityStateZip());
    }

    public static string Heading(this DataRow row)
    {
      return row["Heading"] as string;
    }

    public static int Id(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("Id")
        ? defaultValue
        : Convert.ToInt32(row["Id"]);
    }

    public static string Ideology(this DataRow row)
    {
      return row["Ideology"] as string;
    }

    public static int Incumbents(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("Incumbents")
        ? defaultValue
        : Convert.ToInt32(row["Incumbents"]);
    }

    public static int IdeologyId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("IdeologyId")
        ? defaultValue
        : Convert.ToInt32(row["IdeologyId"]);
    }

    //public static DateTime IntroLetterSent(this DataRow row,
    //  DateTime defaultValue = default)
    //{
    //  if (row.IsNull("IntroLetterSent")) return defaultValue;
    //  return (DateTime) row["IntroLetterSent"];
    //}

    public static bool IsEnabled(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsEnabled")) return defaultValue;
      return Convert.ToInt32(row["IsEnabled"]) != 0;
    }

    public static bool IsFlagged(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsFlagged")) return defaultValue;
      return Convert.ToInt32(row["IsFlagged"]) != 0;
    }

    public static bool IsInactive(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsInactive")) return defaultValue;
      return Convert.ToInt32(row["IsInactive"]) != 0;
    }

    public static bool IsInactiveOptional(this DataRow row,
      bool defaultValue = default)
    {
      if (!row.ContainsColumn("IsInactive") || row.IsNull("IsInactive")) return defaultValue;
      return Convert.ToInt32(row["IsInactive"]) != 0;
    }

    public static bool IsIncumbent(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsIncumbent")) return defaultValue;
      return Convert.ToInt32(row["IsIncumbent"]) != 0;
    }

    public static bool IsIncumbentRow(this DataRow row,
      bool defaultValue = default)
    {
      // Pseudo colummn on some queries
      if (row.IsNull("IsIncumbentRow")) return defaultValue;
      return Convert.ToInt32(row["IsIncumbentRow"]) != 0;
    }

    public static bool IsInShapefile(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsInShapefile")) return defaultValue;
      return Convert.ToInt32(row["IsInShapefile"]) != 0;
    }

    public static bool IsIssueOmit(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsIssueOmit")) return defaultValue;
      return Convert.ToInt32(row["IsIssueOmit"]) != 0;
    }

    public static bool IsNull(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsNull")) return defaultValue;
      return Convert.ToInt32(row["IsNull"]) != 0;
    }

    public static bool IsOfficeInElection(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsOfficeInElection")) return defaultValue;
      return Convert.ToInt32(row["IsOfficeInElection"]) == 1;
    }

    public static bool IsOfficeTagForDeletion(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsOfficeTagForDeletion")) return defaultValue;
      return Convert.ToInt32(row["IsOfficeTagForDeletion"]) != 0;
    }

    public static bool IsOnlyForPrimaries(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsOnlyForPrimaries")) return defaultValue;
      return Convert.ToInt32(row["IsOnlyForPrimaries"]) != 0;
    }

    public static bool IsPartyMajor(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsPartyMajor")) return defaultValue;
      return Convert.ToInt32(row["IsPartyMajor"]) != 0;
    }

    public static bool IsPassed(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsPassed")) return defaultValue;
      return Convert.ToInt32(row["IsPassed"]) != 0;
    }

    public static bool IsPrimaryRunningMateOffice(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsPrimaryRunningMateOffice")) return defaultValue;
      return Convert.ToInt32(row["IsPrimaryRunningMateOffice"]) != 0;
    }

    public static bool IsQuestionOmit(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsQuestionOmit")) return defaultValue;
      return Convert.ToInt32(row["IsQuestionOmit"]) != 0;
    }

    public static bool IsResultRecorded(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsResultRecorded")) return defaultValue;
      return Convert.ToInt32(row["IsResultRecorded"]) != 0;
    }

    public static bool IsRunningMate(this DataRow row,
      bool defaultValue = default)
    {
      // Pseudo colummn on some queries
      if (row.IsNull("IsRunningMate")) return defaultValue;
      return Convert.ToInt32(row["IsRunningMate"]) != 0;
    }

    public static bool IsRunningMateOffice(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsRunningMateOffice")) return defaultValue;
      return Convert.ToInt32(row["IsRunningMateOffice"]) != 0;
    }

    public static bool IsSpecialOffice(this DataRow row,
      bool defaultValue = default)
    {
      if (row.IsNull("IsSpecialOffice")) return defaultValue;
      return Convert.ToInt32(row["IsSpecialOffice"]) != 0;
    }

    public static string Issue(this DataRow row)
    {
      return row["Issue"] as string;
    }

    public static int IssueGroupId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("IssueGroupId")
        ? defaultValue
        : Convert.ToInt32(row["IssueGroupId"]);
    }

    public static int IssueId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("IssueId")
        ? defaultValue
        : Convert.ToInt32(row["IssueId"]);
    }

    public static string IssueGroupHeading(this DataRow row)
    {
      return row["IssueGroupHeading"] as string;
    }

    public static string IssueGroupKey(this DataRow row)
    {
      return row["IssueGroupKey"] as string;
    }

    public static int IssueGroupOrder(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("IssueGroupOrder")
        ? defaultValue
        : Convert.ToInt32(row["IssueGroupOrder"]);
    }

    public static string IssueGroupSubHeading(this DataRow row)
    {
      return row["IssueGroupSubHeading"] as string;
    }

    public static string IssueKey(this DataRow row)
    {
      return row["IssueKey"] as string;
    }

    public static string IssueLevel(this DataRow row)
    {
      return row["IssueLevel"] as string;
    }

    public static int IssueOrder(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("IssueOrder")
        ? defaultValue
        : Convert.ToInt32(row["IssueOrder"]);
    }

    public static bool IsTextSourceOptional(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsTextSourceOptional")) return defaultValue;
      return Convert.ToInt32(row["IsTextSourceOptional"]) != 0;
    }

    public static bool IsVacant(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsVacant")) return defaultValue;
      return Convert.ToInt32(row["IsVacant"]) != 0;
    }

    public static bool IsVirtual(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsVirtual")) return defaultValue;
      return Convert.ToInt32(row["IsVirtual"]) != 0;
    }

    public static bool IsWinner(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsWinner")) return defaultValue;
      return Convert.ToInt32(row["IsWinner"]) != 0;
    }

    public static bool IsWinnersIdentified(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("IsWinnersIdentified")) return defaultValue;
      return Convert.ToInt32(row["IsWinnersIdentified"]) != 0;
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

    public static string LdsStateCode(this DataRow row)
    {
      return row["LDSStateCode"] as string;
    }

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

    public static string LocalKey(this DataRow row)
    {
      return row["LocalKey"] as string;
    }

    public static string LocalDistrict(this DataRow row)
    {
      return row["LocalDistrict"] as string;
    }

    public static string LocalId(this DataRow row)
    {
      return row["LocalId"] as string;
    }

    public static string LocalType(this DataRow row)
    {
      return row["LocalType"] as string;
    }

    public static string LongMission(this DataRow row)
    {
      return row["LongMission"] as string;
    }

    public static string LongName(this DataRow row)
    {
      return row["LongName"] as string;
    }

    public static string Lsad(this DataRow row)
    {
      return row["Lsad"] as string;
    }

    public static string MiddleName(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("MiddleName")
        ? row["MiddleName"]
        : row["MName"]) as string;
    }

    public static string Military(this DataRow row)
    {
      return row["Military"] as string;
    }

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

    public static int OfficeLevel(this DataRow row, int defaultValue = default)
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
      int defaultValue = default)
    {
      return row.IsNull("OfficeOrder")
        ? defaultValue
        : Convert.ToInt32(row["OfficeOrder"]);
    }

    public static int OfficeOrderWithinLevel(this DataRow row,
      int defaultValue = default)
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
      int defaultValue = default)
    {
      return row.IsNull("OrderOnBallot")
        ? defaultValue
        : Convert.ToInt32(row["OrderOnBallot"]);
    }

    public static string OrgAbbreviation(this DataRow row)
    {
      return row["OrgAbbreviation"] as string;
    }

    public static int OrgId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("OrgId")
        ? defaultValue
        : Convert.ToInt32(row["OrgId"]);
    }

    public static string OrgSubType(this DataRow row)
    {
      return row["OrgSubType"] as string;
    }

    public static string OrgType(this DataRow row)
    {
      return row["OrgType"] as string;
    }

    public static int OrgSubTypeId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("OrgSubTypeId")
        ? defaultValue
        : Convert.ToInt32(row["OrgSubTypeId"]);
    }

    public static int OrgTypeId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("OrgTypeId")
        ? defaultValue
        : Convert.ToInt32(row["OrgTypeId"]);
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

    public static int PartyOrder(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("PartyOrder")
        ? defaultValue
        : Convert.ToInt32(row["PartyOrder"]);
    }

    public static string PartyPassword(this DataRow row)
    {
      return row["PartyPassword"] as string;
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

    public static string Personal(this DataRow row)
    {
      return row["Personal"] as string;
    }

    public static string Phone(this DataRow row)
    {
      return row["Phone"] as string;
    }

    public static string PinterestWebAddress(this DataRow row)
    {
      return row["PinterestWebAddress"] as string;
    }

    public static string PodcastWebAddress(this DataRow row)
    {
      return row["PodcastWebAddress"] as string;
    }

    public static string Political(this DataRow row)
    {
      return row["Political"] as string;
    }

    public static string PoliticianKey(this DataRow row)
    {
      return row["PoliticianKey"] as string;
    }

    public static string Prefix(this DataRow row)
    {
      return row["Prefix"] as string;
    }

    public static decimal PrimaryAdRate(this DataRow row)
    {
      return (decimal)row["PrimaryAdRate"];
    }

    public static int PrimaryPositions(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("PrimaryPositions")
        ? defaultValue
        : Convert.ToInt32(row["PrimaryPositions"]);
    }

    public static int PrimaryRunoffPositions(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("PrimaryRunoffPositions")
        ? defaultValue
        : Convert.ToInt32(row["PrimaryRunoffPositions"]);
    }

    public static string Profession(this DataRow row)
    {
      return row["Profession"] as string;
    }

    public static string PublicAddress(this DataRow row)
    {
      var result = row.Address();
      if (IsNullOrWhiteSpace(result)) result = row.StateAddress();
      return result;
    }

    public static string PublicCityStateZip(this DataRow row)
    {
      var result = row.CityStateZip();
      if (IsNullOrWhiteSpace(result)) result = row.StateCityStateZip();
      return result;
    }

    public static string PublicEmail(this DataRow row)
    {
      var result = row.Email();
      if (IsNullOrWhiteSpace(result)) result = row.StateEmail();
      return result;
    }

    public static string PublicPhone(this DataRow row)
    {
      var result = row.Phone();
      if (IsNullOrWhiteSpace(result)) result = row.StatePhone();
      return result;
    }

    public static string PublicWebAddress(this DataRow row)
    {
      var result = row.WebAddress();
      if (IsNullOrWhiteSpace(result)) result = row.StateWebAddress();
      return result;
    }

    public static string Question(this DataRow row)
    {
      return row["Question"] as string;
    }

    public static int QuestionId(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("QuestionId")
        ? defaultValue
        : Convert.ToInt32(row["QuestionId"]);
    }

    public static int? QuestionIdOrNull(this DataRow row)
    {
      return row.IsNull("QuestionId")
        ? (int?)null
        : Convert.ToInt32(row["QuestionId"]);
    }

    public static string QuestionKey(this DataRow row)
    {
      return row["QuestionKey"] as string;
    }

    public static int QuestionOrder(this DataRow row, int defaultValue = default)
    {
      return row.IsNull("QuestionOrder")
        ? defaultValue
        : Convert.ToInt32(row["QuestionOrder"]);
    }

    public static string ReferendumDescription(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("ReferendumDescription")
        ? row["ReferendumDescription"]
        : row["ReferendumDesc"]) as string;
    }

    public static string ReferendumDetail(this DataRow row)
    {
      return row["ReferendumDetail"] as string;
    }

    public static string ReferendumDetailUrl(this DataRow row)
    {
      return row["ReferendumDetailUrl"] as string;
    }

    public static string ReferendumFullText(this DataRow row)
    {
      return row["ReferendumFullText"] as string;
    }

    public static string ReferendumFullTextUrl(this DataRow row)
    {
      return row["ReferendumFullTextUrl"] as string;
    }

    public static string ReferendumKey(this DataRow row)
    {
      return row["ReferendumKey"] as string;
    }

    public static string ReferendumTitle(this DataRow row)
    {
      return row["ReferendumTitle"] as string;
    }

    public static string Religion(this DataRow row)
    {
      return row["Religion"] as string;
    }

    public static DateTime ResponseDate(this DataRow row)
    {
      if (IsNullOrWhiteSpace(row.YouTubeUrl())) return row.AnswerDate();
      return new[]{row.AnswerDate(), row.YouTubeDate()}.Max();
    }

    public static string RssFeedWebAddress(this DataRow row)
    {
      return row["RSSFeedWebAddress"] as string;
    }

    public static string RunningMateKey(this DataRow row)
    {
      return row["RunningMateKey"] as string;
    }

    public static string SchoolDistrictDistrictCode(this DataRow row)
    {
      return row["SchoolDistrictDistrictCode"] as string;
    }

    public static string SelectionCriteria(this DataRow row)
    {
      return row["SelectionCriteria"] as string;
    }

    public static DateTime SentTime(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("SentTime")) return defaultValue;
      return (DateTime) row["SentTime"];
    }

    public static int Sequence(this DataRow row,
      int defaultValue = default)
    {
      return row.IsNull("Sequence")
        ? defaultValue
        : Convert.ToInt32(row["Sequence"]);
    }

    public static int? SequenceOrNull(this DataRow row)
    {
      return row.IsNull("Sequence")
        ? (int?) null
        : Convert.ToInt32(row["Sequence"]);
    }

    public static string ShortMission(this DataRow row)
    {
      return row["ShortMission"] as string;
    }

    public static string SortIssueLevel(this DataRow row)
    {
      return row["SortIssueLevel"] as string;
    }

    public static string SourceCode(this DataRow row)
    {
      return row["SourceCode"] as string;
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

    //public static string StateData(this DataRow row)
    //{
    //  return row["StateData"] as string;
    //}

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
      if (IsNullOrWhiteSpace(result))
        result = row.Email();
      return result;
    }

    public static string StateOrCandidateWebAddress(this DataRow row)
    {
      var result = row.StateWebAddress();
      if (IsNullOrWhiteSpace(result))
        result = row.WebAddress();
      return result;
    }

    public static string StateOrCandidatePhone(this DataRow row)
    {
      var result = row.StatePhone();
      if (IsNullOrWhiteSpace(result))
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

    public static string SubHeading(this DataRow row)
    {
      return row["SubHeading"] as string;
    }

    public static string Subject(this DataRow row)
    {
      return row["Subject"] as string;
    }

    public static string Suffix(this DataRow row)
    {
      return row["Suffix"] as string;
    }

    public static string TigerCode(this DataRow row)
    {
      return row["TigerCode"] as string;
    }

    public static string Title(this DataRow row)
    {
      return row["Title"] as string;
    }

    public static string TigerType(this DataRow row)
    {
      return row["TigerType"] as string;
    }

    public static string ToEmail(this DataRow row)
    {
      return row["ToEmail"] as string;
    }

    public static string TwitterWebAddress(this DataRow row)
    {
      return row["TwitterWebAddress"] as string;
    }

    public static bool Undeletable(this DataRow row, bool defaultValue = default)
    {
      if (row.IsNull("Undeletable")) return defaultValue;
      return Convert.ToInt32(row["Undeletable"]) != 0;
    }

    public static string Url(this DataRow row)
    {
      return
      (row.Table.Columns.Contains("Url")
        ? row["Url"]
        : row["URL"]) as string;
    }

    public static string UserCountyCode(this DataRow row)
    {
      return row["UserCountyCode"] as string;
    }

    public static string UserLocalKey(this DataRow row)
    {
      return row["UserLocalKey"] as string;
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
      int defaultValue = default)
    {
      return row.IsNull("VisitorId")
        ? defaultValue
        : Convert.ToInt32(row["VisitorId"]);
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
      bool defaultValue = default)
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
      int defaultValue = default)
    {
      return row.IsNull("WriteInLines")
        ? defaultValue
        : Convert.ToInt32(row["WriteInLines"]);
    }

    public static string WriteInWording(this DataRow row)
    {
      return row["WriteInWording"] as string;
    }

    public static string Year(this DataRow row)
    {
      return row["Year"] as string;
    }

    public static string YouTubeAutoDisable(this DataRow row)
    {
      return row["YouTubeAutoDisable"] as string;
    }

    public static DateTime YouTubeDate(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("YouTubeDate")) return defaultValue;
      return (DateTime)row["YouTubeDate"];
    }

    public static DateTime? YouTubeDateOrNull(this DataRow row)
    {
      if (row.IsNull("YouTubeDate")) return null;
      return (DateTime)row["YouTubeDate"];
    }

    public static string YouTubeDescription(this DataRow row)
    {
      return row["YouTubeDescription"] as string;
    }

    public static DateTime YouTubeRefreshTime(this DataRow row,
      DateTime defaultValue = default)
    {
      if (row.IsNull("YouTubeRefreshTime")) return defaultValue;
      return (DateTime) row["YouTubeRefreshTime"];
    }

    public static TimeSpan YouTubeRunningTime(this DataRow row,
      TimeSpan defaultValue = default)
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

    public static string Zip(this DataRow row)
    {
      return row["Zip"] as string;
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