using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace DB.Vote
{
  // ReSharper disable once ClassNeverInstantiated.Global
  public partial class PoliticiansRow
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public string Age
    {
      get { return Politicians.GetAgeFromDateTime(DateOfBirth); }
    }

    public string DateOfBirthAsString
    {
      get { return Politicians.GetDateOfBirthFromDateTime(DateOfBirth); }
    }

    public string EnquotedNickname
    {
      get
      {
        return Politicians.GetEnquotedNicknameForState(Nickname,
          Politicians.GetStateCodeFromKey(PoliticianKey));
      }
    }

    //public bool HasCompleteLDSAddress
    //{
    //  get
    //  {
    //    //return !IsNullOrWhiteSpace(LDSAddress) &&
    //    //  !IsNullOrWhiteSpace(LDSCityStateZip);
    //    return false;
    //  }
    //}

    public bool HasCompleteStateAddress
    {
      get
      {
        return !IsNullOrWhiteSpace(StateAddress) &&
          !IsNullOrWhiteSpace(StateCityStateZip);
      }
    }

    public string PublicAddress
    {
      get
      {
        var result = Address;
        if (IsNullOrWhiteSpace(result)) result = StateAddress;
        return result;
      }
    }

    public string StateOrCandidateAddress
    {
      get
      {
        if (HasCompleteStateAddress)
          return StateAddress;
        return Address;
      }
    }

    public string PublicCityStateZip
    {
      get
      {
        var result = CityStateZip;
        if (IsNullOrWhiteSpace(result)) result = StateCityStateZip;
        return result;
      }
    }

    public string StateOrCandidateCityStateZip
    {
      get
      {
        if (HasCompleteStateAddress)
          return StateCityStateZip;
        return CityStateZip;
      }
    }

    public string PublicEmail
    {
      get
      {
        var result = Email;
        if (IsNullOrWhiteSpace(result)) result = StateEmail;
        return result;
      }
    }

    public string StateOrCandidateEmail
    {
      get
      {
        var result = StateEmail;
        if (IsNullOrWhiteSpace(result))
          result = Email;
        return result;
      }
    }

    public string PublicPhone
    {
      get
      {
        var result = Phone;
        if (IsNullOrWhiteSpace(result)) result = StatePhone;
        return result;
      }
    }

    public string StateOrCandidatePhone
    {
      get
      {
        var result = StatePhone;
        if (IsNullOrWhiteSpace(result))
          result = Phone;
        return result;
      }
    }

    public string PublicWebAddress
    {
      get
      {
        var result = WebAddress;
        if (IsNullOrWhiteSpace(result)) result = StateWebAddress;
        return result;
      }
    }

    public string StateOrCandidateWebAddress
    {
      get
      {
        var result = StateWebAddress;
        if (IsNullOrWhiteSpace(result))
          result = WebAddress;
        return result;
      }
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public partial class Politicians
  {
    #region Private

    //private static bool? _GetHasCompleteLDSAddress(string politicianKey,
    //  bool? defaultValue)
    //{
    //  bool? result = null;

    //  var table = GetAddressesData(politicianKey);
    //  if (table.Count != 0)
    //    result = table[0].HasCompleteLDSAddress;

    //  return result ?? defaultValue;
    //}

    private static bool? _GetHasCompleteStateAddress(string politicianKey,
      bool? defaultValue)
    {
      bool? result = null;

      var table = GetAddressesData(politicianKey);
      if (table.Count != 0)
        result = table[0].HasCompleteStateAddress;

      return result ?? defaultValue;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static void AddPolitician(string politicianKey, string firstName,
      string middleName, string nickname, string lastName, string suffix,
      string partyKey, string password)
    {
      Insert(politicianKey, password, Empty, Empty, Empty,
        Empty, GetStateCodeFromKey(politicianKey), firstName, middleName, nickname,
        lastName, lastName.StripAccents(), lastName.StripVowels(), suffix, Empty,
        Empty, Empty, Empty, Empty, Empty, Empty,
        Empty, Empty, Empty, partyKey, Empty, Empty,
        Empty, Empty, Empty, Empty, Empty, Empty,
        Empty, /*Empty, VotePage.DefaultDbDate, false,*/ DateTime.UtcNow, 0,
        VotePage.DefaultDbDate, /*0,*/ VotePage.DefaultDbDate, Empty, Empty,
        Empty, Empty, Empty, Empty, Empty, Empty, Empty,
        Empty, Empty, Empty, Empty, Empty, Empty, Empty, Empty,
        default, VotePage.DefaultDbDate, Empty, false, false, VotePage.DefaultDbDate);
    }

    public static int CountAllSocialMediaLinks(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT SUM(" +
        "(NOT EmailAddr IS NULL AND EmailAddr != '' OR StateEmailAddr != '') +" +
        "(NOT WebAddr IS NULL AND WebAddr != '' OR StateWebAddr!= '') +" +
        "(YouTubeWebAddress != '') +" +
        "(FacebookWebAddress != '') +" +
        "(FlickrWebAddress != '') +" +
        "(TwitterWebAddress != '') +" +
        "(RSSFeedWebAddress != '') +" +
        "(WikipediaWebAddress != '') +" +
        "(BallotpediaWebAddress != '') +" +
        "(VimeoWebAddress != '') +" +
        "(GooglePlusWebAddress != '') +" +
        "(LinkedInWebAddress != '') +" +
        "(PinterestWebAddress != '') +" +
        "(BloggerWebAddress != '') +" +
        "(PodcastWebAddress != '') +" +
        "(WebstagramWebAddress != ''))" +
        " FROM vote.Politicians";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static string GetAge(string politicianKey)
    {
      return
        GetAgeFromDateTime(GetDateOfBirth(politicianKey, VotePage.DefaultDbDate));
    }

    public static string GetAgeFromDateTime(DateTime dateTime)
    {
      if (dateTime.IsDefaultDate())
        return Empty;
      var now = DateTime.UtcNow;
      var years = now.Year - dateTime.Year;
      if (dateTime.DayOfYear > now.DayOfYear) years--;
      return years.ToString(CultureInfo.InvariantCulture);
    }

    public static string GetColumnExtended(string columnName, string politicianKey)
    {
      switch (columnName.ToLowerInvariant())
      {
        case "age":
          return GetAge(politicianKey);

        case "dateofbirth":
          return GetDateOfBirthAsString(politicianKey);

        case "publicaddress":
          return GetPublicAddress(politicianKey);

        case "publiccitystatezip":
          return GetPublicCityStateZip(politicianKey);

        case "publicemail":
          return GetPublicEmail(politicianKey);

        case "publicphone":
          return GetPublicPhone(politicianKey);

        case "publicwebaddress":
          return GetPublicWebAddress(politicianKey);

        default:
        {
          var result = GetColumn(GetColumn(columnName), politicianKey);
          if (result is DateTime dateTime && dateTime.IsDefaultDate())
            return Empty;
          return result?.ToString();
        }
      }
    }

    public static string GetDateOfBirthAsString(string politicianKey)
    {
      return
        GetDateOfBirthFromDateTime(GetDateOfBirth(politicianKey,
          VotePage.DefaultDbDate));
    }

    public static string GetDateOfBirthFromDateTime(DateTime dateTime)
    {
      return dateTime == VotePage.DefaultDbDate
        ? Empty
        : dateTime.ToShortDateString();
    }

    public static PoliticiansTable GetDuplicateNamesDataLikeLastNameStateCode(
      string lastName, string stateCode, int commandTimeout = -1)
    {
      var cmdText = SelectDuplicateNamesCommandText;
      if (stateCode == "US")
      {
        cmdText += "   WHERE (LName=@LastName";
        cmdText += "     OR LName LIKE @Pattern1";
        cmdText += "     OR LName LIKE @Pattern2";
        cmdText += "     OR LName LIKE @Pattern3";
        cmdText += "     OR LName LIKE @Pattern4)";
        cmdText += " ORDER BY LName,FName,MName";
      }
      else
      {
        cmdText += " WHERE StateCode=@StateCode";
        cmdText += "   AND (LName=@LastName";
        cmdText += "     OR LName LIKE @Pattern1";
        cmdText += "     OR LName LIKE @Pattern2";
        cmdText += "     OR LName LIKE @Pattern3";
        cmdText += "     OR LName LIKE @Pattern4)";
        cmdText += " ORDER BY LName,FName,MName";
      }
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);

      VoteDb.AddCommandParameter(cmd, "LastName", lastName);
      VoteDb.AddCommandParameter(cmd, "Pattern1", "% " + lastName);
      VoteDb.AddCommandParameter(cmd, "Pattern2", "%-" + lastName);
      VoteDb.AddCommandParameter(cmd, "Pattern3", lastName + " %");
      VoteDb.AddCommandParameter(cmd, "Pattern4", lastName + "-%");
      if (stateCode != "US")
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);

      return FillTable(cmd, PoliticiansTable.ColumnSet.DuplicateNames);
    }

    public static string GetEnquotedNickname(string nickname, string quote1 = "\"",
      string quote2 = null)
    {
      quote2 = quote2 ?? quote1;
      return IsNullOrWhiteSpace(nickname)
        ? Empty
        : quote1 + nickname + quote2;
    }

    public static string GetEnquotedNicknameForState(string nickname,
      string stateCode)
    {
      string quote1;
      string quote2 = null;
      switch (StateCache.GetEncloseNicknameCode(stateCode))
      {
        case "D":
        case null:
          quote1 = "\"";
          break;

        case "S":
          quote1 = "'";
          break;

        case "P":
          quote1 = "(";
          quote2 = ")";
          break;

        default:
          quote1 = Empty;
          break;
      }

      return GetEnquotedNickname(nickname, quote1, quote2);
    }

    public static bool? GetHasCompleteStateAddress(string politicianKey)
    {
      return _GetHasCompleteStateAddress(politicianKey, null);
    }

    public static bool GetHasCompleteStateAddress(string politicianKey,
      bool defaultValue)
    {
      var hasCompleteStateAddress = _GetHasCompleteStateAddress(politicianKey,
        defaultValue);
      return hasCompleteStateAddress == true;
    }

    public static PoliticianOfficeStatus GetOfficeStatus(string politicianKey)
    {
      const string cmdText = "CALL OfficeStatus(@PoliticianKey)";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var adapter = VoteDb.GetDataAdapter(cmd);
        adapter.Fill(table);
      }

      var status = table.Rows[0]["Status"].ToString();
      if (!Enum.TryParse(status, out PoliticianStatus policicianStatus))
        policicianStatus = PoliticianStatus.Unknown;

      var result = new PoliticianOfficeStatus
      {
        OfficeKey = table.Rows[0].OfficeKey(),
        PoliticianStatus = policicianStatus
      };

      return result;
    }

    public static DateTime? GetLatestSignin(string politicianKey)
    {
      const string cmdText = "SELECT MAX(l.DateStamp) FROM vote.Politicians p" +
        " INNER JOIN votelog.LogLogins l ON l.UserSecurity = 'POLITICIAN' AND l.UserName = p.PoliticianKey" +
        " WHERE p.PoliticianKey = @politicianKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "politicianKey", politicianKey);
      return VoteDb.ExecuteScalar(cmd) as DateTime?;
    }

    public static DataRow GetPoliticianIntroReportData(string politiciankey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT p.AddOn,p.Address,p.BallotPediaWebAddress,p.GoFundMeWebAddress," +
        "p.CrowdpacWebAddress,p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip," +
        "p.DateOfBirth," +
        " p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
        " p.FName AS FirstName,p.GooglePlusWebAddress," +
        //"p.LDSAddress,p.LDSCityStateZip,p.LDSEmailAddr AS LDSEmail,p.LDSPhone,p.LDSWebAddr AS LDSWebAddress," +
        "p.LinkedInWebAddress," +
        " p.LiveElectionKey,p.LiveOfficeKey,p.LiveOfficeStatus,p.LName AS LastName," +
        " p.MName AS MiddleName,p.Nickname,p.PartyKey,p.Phone," +
        " p.PinterestWebAddress,p.PoliticianKey,p.RSSFeedWebAddress," +
        " p.StateAddress,p.StateCityStateZip,p.StateEmailAddr AS StateEmail," +
        " p.StatePhone,p.StateWebAddr AS StateWebAddress,p.Suffix," +
        " p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr AS WebAddress," +
        " p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        "p.YouTubeDescription,p.YouTubeRunningTime,p.YouTubeDate,p.YouTubeAutoDisable," +
        //" p.GeneralStatement,p.Personal,p.Education,p.Profession,p.Military," +
        //" p.Civic,p.Political,p.Religion,p.Accomplishments," +
        " pt.PartyCode,pt.PartyName,pt.PartyUrl,o.OfficeLevel,o.OfficeLine1," +
        " o.OfficeLine2,e.ElectionDesc FROM Politicians p" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
        " LEFT JOIN Offices o ON o.OfficeKey=p.LiveOfficeKey" +
        " LEFT JOIN Elections e ON e.ElectionKey=p.LiveElectionKey" +
        " WHERE p.PoliticianKey=@PoliticianKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politiciankey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static DataRow GetPoliticianIssueReportData(string politiciankey,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT p.AddOn,p.Address,p.BallotPediaWebAddress,p.GoFundMeWebAddress," +
        "p.CrowdpacWebAddress,p.BloggerWebAddress,p.PodcastWebAddress,p.CityStateZip,p.DateOfBirth," +
        " p.EmailAddr AS Email,p.FacebookWebAddress,p.FlickrWebAddress," +
        " p.FName AS FirstName,p.GooglePlusWebAddress," +
        //"p.LDSAddress,p.LDSCityStateZip,p.LDSEmailAddr AS LDSEmail,p.LDSPhone,p.LDSWebAddr AS LDSWebAddress," +
        "p.LinkedInWebAddress," +
        " p.LiveElectionKey,p.LiveOfficeKey,p.LiveOfficeStatus,p.LName AS LastName," +
        " p.MName AS MiddleName,p.Nickname,p.PartyKey,p.Phone," +
        " p.PinterestWebAddress,p.PoliticianKey,p.RSSFeedWebAddress," +
        " p.StateAddress,p.StateCityStateZip,p.StateEmailAddr AS StateEmail," +
        " p.StatePhone,p.StateWebAddr AS StateWebAddress,p.Suffix," +
        " p.TwitterWebAddress,p.VimeoWebAddress,p.WebAddr AS WebAddress," +
        " p.WebstagramWebAddress,p.WikipediaWebAddress,p.YouTubeWebAddress," +
        " pt.PartyCode,pt.PartyName,pt.PartyUrl,o.OfficeLevel,o.OfficeLine1," +
        " o.OfficeLine2,e.ElectionDesc FROM Politicians p" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
        " LEFT JOIN Offices o ON o.OfficeKey=p.LiveOfficeKey" +
        " LEFT JOIN Elections e ON e.ElectionKey=p.LiveElectionKey" +
        " WHERE p.PoliticianKey=@PoliticianKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "PoliticianKey", politiciankey);
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static string GetPublicAddress(string politicianKey)
    {
      var table = GetAddressesData(politicianKey);
      return table.Count == 0 ? Empty : table[0].PublicAddress;
    }

    public static string GetPublicCityStateZip(string politicianKey)
    {
      var table = GetAddressesData(politicianKey);
      return table.Count == 0 ? Empty : table[0].PublicCityStateZip;
    }

    public static string GetPublicEmail(string politicianKey)
    {
      var table = GetEmailsData(politicianKey);
      return table.Count == 0 ? Empty : table[0].PublicEmail;
    }

    public static string GetPublicPhone(string politicianKey)
    {
      var table = GetPhonesData(politicianKey);
      return table.Count == 0 ? Empty : table[0].PublicPhone;
    }

    public static string GetPublicWebAddress(string politicianKey)
    {
      var table = GetWebAddressesData(politicianKey);
      return table.Count == 0 ? Empty : table[0].PublicWebAddress;
    }

    public static DataRow GetCandidateData(string electionKey, string politicianKey,
      bool isRunningMate,
      int commandTimeout = -1)
    {
      var cmdText =
        "SELECT p.AddOn,p.Address,p.CityStateZip,p.EmailAddr AS Email,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LName AS LName,p.Nickname,p.Phone," +
        "p.PoliticianKey,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.WebAddr AS WebAddress," +
        "pt.PartyCode,pt.PartyName,pt.PartyUrl,ep.IsIncumbent," +
        $"{(isRunningMate ? 1 : 0)} AS IsRunningMate FROM Politicians p" +
        " LEFT JOIN ElectionsPoliticians ep ON ep.ElectionKey=@ElectionKey AND ep.PoliticianKey=@PoliticianKey" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" + " WHERE p.PoliticianKey=@PoliticianKey GROUP BY p.PoliticianKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "ElectionKey", electionKey);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static DataRow GetCandidateData(string politicianKey, bool isRunningMate,
      int commandTimeout = -1)
    {
      var cmdText =
        "SELECT p.AddOn,p.Address,p.CityStateZip,p.EmailAddr AS Email,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LName AS LName,p.Nickname,p.Phone," +
        "p.PoliticianKey,p.StateAddress,p.StateCityStateZip," +
        "p.StateEmailAddr AS StateEmail,p.StatePhone," +
        "p.StateWebAddr AS StateWebAddress,p.Suffix,p.WebAddr AS WebAddress," +
        "pt.PartyCode,pt.PartyName,pt.PartyUrl," +
        $"{(isRunningMate ? 1 : 0)} AS IsRunningMate FROM Politicians p" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" + " WHERE p.PoliticianKey=@PoliticianKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static DataRow GetListItemData(string politicianKey, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT p.Address,p.CityStateZip,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LiveOfficeKey," +
        "p.LiveOfficeStatus,p.LName AS LName,p.Nickname,p.PoliticianKey," +
        "p.StateAddress,p.StateCityStateZip,p.Suffix,p.AlphaName," +
        "p.VowelStrippedName,pt.PartyCode,o.OfficeLine1," +
        "o.OfficeLine2,o.OfficeLevel,l.LocalDistrict FROM Politicians p" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
        " LEFT JOIN Offices o ON o.OfficeKey=p.LiveOfficeKey" +
        " LEFT JOIN LocalDistricts l ON l.StateCode=o.StateCode" +
        "  AND l.LocalKey=o.LocalKey" +
        " WHERE p.PoliticianKey=@PoliticianKey";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table.Rows.Count == 1 ? table.Rows[0] : null;
      }
    }

    public static DataTable GetSearchCandidates(string lastname,
      IList<string> keysToSkip, IEnumerable<string> stateCodes,
      bool fullAlphaNameOnly = false, int commandTimeout = -1)
    {
      // if stateCode is not supplied, do not search on VowelStrippedName --
      // these are presidential candidates (or if it is a single character)
      var validStateCodes = stateCodes.Where(StateCache.IsValidStateCode).ToArray();
      var haveStateCode = validStateCodes.Length > 0;
      var alphaName = lastname.StripAccents();
      if (!fullAlphaNameOnly) alphaName += "%";
      var vowelStrippedName = lastname.StripVowels() + "%";

      var stateCodeClause = haveStateCode
        ? "p.StateCode IN ('" + Join("','", validStateCodes) + "') AND"
        : Empty;
      var excludeClause = keysToSkip == null || keysToSkip.Count == 0
        ? Empty
        : "p.PoliticianKey NOT IN ('" + Join("','", keysToSkip) + "') AND";
      var vowelStrippedClause = haveStateCode && vowelStrippedName.Length > 2 &&
        !fullAlphaNameOnly
          ? "OR p.VowelStrippedName LIKE @VowelStrippedName"
          : Empty;

      var cmdText =
        "SELECT p.Address,p.CityStateZip,p.FName AS FirstName," +
        "p.MName as MiddleName,p.LiveOfficeKey," +
        "p.LiveOfficeStatus,p.LName AS LName,p.Nickname,p.PoliticianKey," +
        "p.StateAddress,p.StateCityStateZip,p.Suffix,p.AlphaName," +
        "p.VowelStrippedName,pt.PartyCode,o.OfficeLine1," +
        "o.OfficeLine2,o.OfficeLevel,l.LocalDistrict FROM Politicians p" +
        " LEFT JOIN Parties pt ON pt.PartyKey=p.PartyKey" +
        " LEFT JOIN Offices o ON o.OfficeKey=p.LiveOfficeKey" +
        " LEFT JOIN LocalDistricts l ON l.StateCode=o.StateCode" +
        " AND l.LocalKey=o.LocalKey" +
        $" WHERE {stateCodeClause} {excludeClause} (p.AlphaName LIKE @AlphaName {vowelStrippedClause})";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "AlphaName", alphaName);
      if (haveStateCode && vowelStrippedName.Length > 2 && !fullAlphaNameOnly)
        VoteDb.AddCommandParameter(cmd, "VowelStrippedName", vowelStrippedName);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    public static string GetUniqueKey(string stateCode, string lastName,
      string firstName, string middleName, string suffix)
    {
      stateCode = stateCode.ToUpperInvariant();
      lastName = lastName.StripAccents(true);
      firstName = firstName.StripAccents(true);
      middleName = middleName.StripAccents(true);
      suffix = suffix.StripAccents(true);
      var politicianKey = stateCode + lastName + firstName + middleName + suffix;

      // Get all existing keys that match
      const string cmdText =
        "SELECT PoliticianKey FROM Politicians WHERE PoliticianKey LIKE @PoliticianKey";
      var cmd = VoteDb.GetCommand(cmdText);
      VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey + "%");
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }

      // If there's no exact match, no adjustment is necessary
      // Otherwise, append integers until it's unique
      var sequence = 1;
      var uniqueKey = politicianKey;
      while (table.Rows.Cast<DataRow>()
        .FirstOrDefault(row => row.PoliticianKey()
          .IsEqIgnoreCase(uniqueKey)) != null)
        uniqueKey = politicianKey + sequence++;

      return uniqueKey;
    }

    public static IList<DataRow> GetUnverifiedYouTubeVideos(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticianKey,YouTubeWebAddress,FName AS FirstName,MName AS MiddleName," +
        "Nickname,LName AS LastName,Suffix,StateCode" +
        " FROM Politicians" +
        " WHERE YouTubeWebAddress!='' AND YouTubeVideoVerified=0" +
        " AND (YouTubeAutoDisable IS NULL OR YouTubeAutoDisable='')" +
        " ORDER BY LastName,FirstName,MiddleName,Suffix,StateCode";

      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }
      return table.Rows.Cast<DataRow>()
        .Where(r => r.YouTubeWebAddress().IsValidYouTubeVideoUrl())
        .ToList();
    }

    public static PoliticiansTable GetYouTubeRefreshData(int commandTimeout = -1)
    {
      var cmdText = SelectYouTubeRefreshCommandText + " WHERE YouTubeWebAddress!=''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, PoliticiansTable.ColumnSet.YouTubeRefresh);
    }

    public static PoliticiansTable GetYouTubeRefreshData2(int maxRows, int commandTimeout = -1)
    {
      // We now do the oldest maxRows only
      var cmdText = SelectYouTubeRefreshCommandText + 
        $" WHERE YouTubeWebAddress!='' ORDER BY YouTubeRefreshDate LIMIT {maxRows}";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, PoliticiansTable.ColumnSet.YouTubeRefresh);
    }

    public static void IncrementDataUpdatedCount(string politicianKey)
    {
      var dataUpdatedCount = GetDataUpdatedCount(politicianKey, 0) + 1;
      UpdateDataUpdatedCount(dataUpdatedCount, politicianKey);
      UpdateDataLastUpdated(DateTime.UtcNow, politicianKey);
    }

    public static bool OfficeKeyExists(string officeKey)
    {
      const string cmdText =
        "SELECT COUNT(*) FROM Politicians WHERE LiveOfficeKey=@OfficeKey";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "OfficeKey", officeKey);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result) != 0;
    }

    public static void UpdateColumnExtended(string columnName, object newValue,
      string politicianKey)
    {
      var stringValue = (newValue as string).SafeString();
      switch (columnName.ToLowerInvariant())
      {
        case "dateofbirth":
        case "datelastupdated":
        case "datepictureuploaded":
        case "introlettersent":
        case "isldsincumbent":
        {
          var value = VotePage.DefaultDbDate;
          stringValue = stringValue.Trim();
          if (!IsNullOrEmpty(stringValue))
            value = DateTime.Parse(stringValue);
          UpdateColumn(GetColumn(columnName), value, politicianKey);
        }
          break;

        case "isnotresponedeemailsent":
        {
          var value = false;
          stringValue = stringValue.Trim();
          if (!IsNullOrEmpty(stringValue))
          {
            if (int.TryParse(stringValue, out var intValue))
              value = intValue != 0;
            else
              value = bool.Parse(stringValue);
          }
          UpdateColumn(GetColumn(columnName), value, politicianKey);
        }
          break;

        case "answers":
        case "dataupdatedcount":
        {
          var value = 0;
          stringValue = stringValue.Trim();
          if (!IsNullOrEmpty(stringValue))
            value = int.Parse(stringValue);
          UpdateColumn(GetColumn(columnName), value, politicianKey);
        }
          break;

        case "publicaddress":
          UpdatePublicAddress(stringValue.Trim(), politicianKey);
          break;

        case "publiccitystatezip":
          UpdatePublicCityStateZip(stringValue.Trim(), politicianKey);
          break;

        case "publicemail":
          UpdatePublicEmail(stringValue.Trim(), politicianKey);
          break;

        case "publicphone":
          UpdatePublicPhone(stringValue.Trim(), politicianKey);
          break;

        case "publicwebaddress":
          UpdatePublicWebAddress(stringValue.Trim(), politicianKey);
          break;

        case "lname":
          UpdateLastName(stringValue.Trim(), politicianKey);
          UpdateSearchKeys(politicianKey);
          break;

        default:
          UpdateColumn(GetColumn(columnName), newValue, politicianKey);
          break;
      }
    }

    public static int UpdateOfficeKeyByOfficeKey(string newValue, string officeKey)
    {
      const string cmdText1 =
        "UPDATE Politicians SET LiveOfficeKey=@newValue WHERE LiveOfficeKey=@OfficeKey";
      var cmd1 = VoteDb.GetCommand(cmdText1, -1);
      VoteDb.AddCommandParameter(cmd1, "OfficeKey", officeKey);
      VoteDb.AddCommandParameter(cmd1, "newValue", newValue);
      var count = VoteDb.ExecuteNonQuery(cmd1);
      return count;
    }

    public static void UpdatePublicAddress(string newValue, string politicianKey)
    {
      //if (newValue == GetStateAddress(politicianKey))
      //  newValue = null;
      UpdateAddress(newValue, politicianKey);
      UpdateStateAddress(Empty, politicianKey);
      var cityStateZip = GetCityStateZip(politicianKey);
      if (IsNullOrWhiteSpace(cityStateZip))
        UpdateCityStateZip(GetStateCityStateZip(politicianKey), politicianKey);
      UpdateStateCityStateZip(Empty, politicianKey);
    }

    public static void UpdatePublicCityStateZip(string newValue,
      string politicianKey)
    {
      //if (newValue == GetStateCityStateZip(politicianKey))
      //  newValue = null;
      UpdateCityStateZip(newValue, politicianKey);
      UpdateStateCityStateZip(Empty, politicianKey);
      var address = GetAddress(politicianKey);
      if (IsNullOrWhiteSpace(address))
        UpdateAddress(GetStateAddress(politicianKey), politicianKey);
      UpdateStateAddress(Empty, politicianKey);
    }

    public static void UpdatePublicEmail(string newValue, string politicianKey)
    {
      //if (newValue == GetStateEmail(politicianKey))
      //  newValue = null;
      UpdateEmail(newValue, politicianKey);
      UpdateStateEmail(Empty, politicianKey);
    }

    public static void UpdatePublicPhone(string newValue, string politicianKey)
    {
      //if (newValue == GetStatePhone(politicianKey))
      //  newValue = null;
      UpdatePhone(newValue, politicianKey);
      UpdateStatePhone(Empty, politicianKey);
    }

    public static void UpdatePublicWebAddress(string newValue, string politicianKey)
    {
      //if (newValue == GetStateWebAddress(politicianKey))
      //  newValue = null;
      UpdateWebAddress(newValue, politicianKey);
      UpdateStateWebAddress(Empty, politicianKey);
    }

    public static void UpdateSearchKeys(string politicianKey)
    {
      var table = GetSearchKeyUpdateData(politicianKey);
      if (table.Count == 1)
      {
        var value = table[0].LastName.StripAccents();
        // ReSharper disable RedundantCheckBeforeAssignment
        if (table[0].AlphaName != value)
          table[0].AlphaName = value;
        value = table[0].LastName.StripVowels();
        if (table[0].VowelStrippedName != value)
          table[0].VowelStrippedName = value;
        // ReSharper restore RedundantCheckBeforeAssignment
        if (table.Rows[0].RowState == DataRowState.Modified)
          UpdateTable(table, PoliticiansTable.ColumnSet.SearchKeyUpdate);
      }
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}