using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Vote;
using Vote.Reports;
using static System.String;

namespace DB.Vote
{
  public enum PoliticianStatus
  {
    Unknown,
    InFutureViewableElection,
    InFutureNonviewableElection, // might not be used now
    InFutureViewableElectionAsRunningMate,
    InFutureNonviewableElectionAsRunningMate, // might not be used now
    InFutureUncreatedElection,
    Incumbent,
    IncumbentRunningMate,
    InPreviousElection,
    InPreviousElectionAsRunningMate
  }

  public static class PoliticianStatusExtensions
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static bool IsInFutureViewableElection(this PoliticianStatus politicianStatus)
    {
      return politicianStatus == PoliticianStatus.InFutureViewableElection ||
        politicianStatus == PoliticianStatus.InFutureViewableElectionAsRunningMate;
    }

    public static PoliticianStatus ToPoliticianStatus(this object value)
    {
      try
      {
        return (PoliticianStatus) Convert.ToInt32(value);
      }
      catch (Exception)
      {
        return PoliticianStatus.Unknown;
      }
    }

    public static PoliticianStatus ToPoliticianStatus(this string value)
    {
      if (!Enum.TryParse(value, out PoliticianStatus result))
        result = PoliticianStatus.Unknown;
      return result;
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public class PoliticianOfficeStatus
  {
    public PoliticianStatus PoliticianStatus;
    public string OfficeKey;
    public string ElectionKey;

    public static PoliticianOfficeStatus FromLiveOfficeKeyAndStatus(
      string liveOfficeKeyAndStatus)
    {
      var result = new PoliticianOfficeStatus();
      var split = liveOfficeKeyAndStatus.Split('|');
      if (split.Length >= 2)
      {
        result.OfficeKey = split[0];
        if (!Enum.TryParse(split[1], out result.PoliticianStatus))
          result.PoliticianStatus = PoliticianStatus.Unknown;
        result.ElectionKey = split.Length > 2 ? split[2] : Empty;
      }
      else
      {
        result.OfficeKey = Empty;
        result.ElectionKey = Empty;
        result.PoliticianStatus = PoliticianStatus.Unknown;
      }
      return result;
    }
  }

  public partial class Politicians
  {
    #region private

    private static int MatchCandidateQuality(string alphaName, string vowelStrippedName,
      DataRow politician)
    {
      if (alphaName == politician.AlphaName()) return 1;
      if (politician.AlphaName().StartsWith(alphaName, StringComparison.Ordinal)) return 2;
      return vowelStrippedName == politician.VowelStrippedName() ? 3 : 4;
    }

    #endregion private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static string FormatName(PoliticiansRow row, bool includeAddOn = false,
      int breakPosition = 0)
    {
      if (row == null) return Empty;
      return FormatName(row.FirstName, row.MiddleName, row.Nickname, row.LastName,
        row.Suffix, includeAddOn ? row.AddOn : null, GetStateCodeFromKey(row.PoliticianKey),
        breakPosition);
    }

    public static string FormatName(DataRow row, bool includeAddOn = false,
      int breakPosition = 0)
    {
      if (row == null) return Empty;
      return FormatName(row.FirstName(), row.MiddleName(), row.Nickname(), row.LastName(),
        row.Suffix(), includeAddOn ? row.AddOn() : null,
        GetStateCodeFromKey(row.PoliticianKey()), breakPosition);
    }

    public static string FormatName(string firstName, string middleName, string nickname,
      string lastName, string suffix, string addon = null, string stateCode = null,
      int breakPosition = 0)
    {
      var formattedName = Empty;

      if (!IsNullOrWhiteSpace(firstName)) formattedName += firstName;

      if (!IsNullOrWhiteSpace(middleName)) formattedName += " " + middleName;

      if (!IsNullOrWhiteSpace(nickname))
        formattedName += " " + GetEnquotedNicknameForState(nickname, stateCode);

      if (!IsNullOrWhiteSpace(lastName)) formattedName += " " + lastName;

      if (!IsNullOrWhiteSpace(suffix)) formattedName += ", " + suffix;

      if (!IsNullOrWhiteSpace(addon)) formattedName += " " + addon;

      if (breakPosition > 0)
        formattedName = formattedName.SplitLinesWithBreakTags(breakPosition);
      return formattedName;
    }

    public static string FormatOfficeAndStatus(DataRow politicianInfo)
    {
      var officeKey = politicianInfo.LiveOfficeKey();
      if (IsNullOrWhiteSpace(officeKey)) return Empty;

      var politicianStatus = politicianInfo.LivePoliticianStatus();
      var officeName = Offices.GetLocalizedOfficeName(politicianInfo);
      var nameElectoral = Offices.GetElectoralClassDescriptionFromOfficeKey(officeKey);
      if (!IsNullOrEmpty(nameElectoral) && !officeName.Contains(nameElectoral))
        officeName += ", " + nameElectoral;

      return politicianStatus.GetOfficeStatusDescription(officeName);
    }

    public static IEnumerable<DataRow> GetCandidateListRows(string name, string stateCode)
    {
      name = name.Trim();
      if (name.Length < 2) return new List<DataRow>();

      return GetSearchCandidates(name, null, new[] {stateCode}).Rows.Cast<DataRow>()
        .OrderBy(row => row.LastName()).ThenBy(row => row.FirstName())
        .GroupBy(row => MatchCandidateQuality(name.StripAccents(), name.StripVowels(), row))
        .OrderBy(g => g.Key).SelectMany(g => g).ToList();
    }

    public static Control GetCandidateList(string partialName, string[] selectedPoliticians,
      string officeKeyOrStateCode, string[] currentCandidates,
      bool fullAlphaNameOnly = false)
    {
      var placeHolder = new PlaceHolder();
      var idPrefix = fullAlphaNameOnly ? "addpolitician-" : "searchpolitician-";
      partialName = partialName.Trim();
      if (partialName.Length < 2) return placeHolder;
      var alphaName = partialName.StripAccents();
      var vowelStrippedName = partialName.StripVowels();

      // The officeKeyOrStateCode can be either empty/null, a single officeKey,
      // a single stateCode, or a comma separated list of stateCodes
      officeKeyOrStateCode = officeKeyOrStateCode ?? Empty;
      var stateCodes =
        officeKeyOrStateCode.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
      if (stateCodes.Length == 1)
        stateCodes[0] = Offices.GetStateCodeFromKey(stateCodes[0]);

      var matches =
        GetSearchCandidates(partialName, currentCandidates, stateCodes, fullAlphaNameOnly)
          .Rows.Cast<DataRow>().OrderBy(row => row.LastName())
          .ThenBy(row => row.FirstName())
          .GroupBy(row => MatchCandidateQuality(alphaName, vowelStrippedName, row))
          .OrderBy(g => g.Key);
      foreach (var group in matches)
      foreach (var politician in group)
      {
        var selected = selectedPoliticians?.Contains(politician.PoliticianKey(),
          StringComparer.OrdinalIgnoreCase) == true;
        var className = selected ? "selected" : null;
        GetCandidateListItem(politician, idPrefix, selected).AddTo(placeHolder, className)
          .Attributes.Add("sort-key",
            (politician.LastName() + " " + politician.FirstName() + " " +
              politician.MiddleName() + " " + politician.Nickname() + " " +
              politician.Suffix()).StripRedundantSpaces().ToLowerInvariant());
      }
      return placeHolder;
    }

    public static HtmlGenericControl GetCandidateListItem(DataRow politician,
      string idPrefix = null, bool noCache = false)
    {
      string OfficeDescription()
      {
        var officeKey = politician.LiveOfficeKey();
        if (IsNullOrWhiteSpace(officeKey)) return Empty;

        var result = politician.OfficeLine1();
        if (!IsNullOrWhiteSpace(politician.OfficeLine2()) &&
          politician.OfficeClass() != OfficeClass.USPresident)
          result += " " + politician.OfficeLine2();
        if (politician.OfficeClass() != OfficeClass.USPresident)
        {
          var stateCode = Offices.GetStateCodeFromKey(officeKey);
          var countyCode = Offices.GetCountyCodeFromKey(officeKey);
          var localKey = Offices.GetLocalKeyFromKey(officeKey);
          if (IsNullOrWhiteSpace(countyCode))
            result = StateCache.GetStateName(stateCode) + " " + result;
          else if (IsNullOrWhiteSpace(localKey))
            result = CountyCache.GetCountyName(stateCode, countyCode) + ", " +
              StateCache.GetStateName(stateCode) + " " + result;
          else
            result = CountyCache.GetCountyDescription(stateCode, localKey) + ", " +
              StateCache.GetStateName(stateCode) + ", " + politician.LocalDistrict() + " " +
              result;
        }
        return politician.LivePoliticianStatus().GetOfficeStatusDescription() + result;
      }

      string AddressLine()
      {
        var result = politician.PublicAddress();
        var cityStateZip = politician.PublicCityStateZip();
        if (!IsNullOrWhiteSpace(result) && !IsNullOrWhiteSpace(cityStateZip))
          result += ", ";
        result += cityStateZip;
        return result;
      }

      var div = new HtmlDiv();
      div.AddCssClasses("search-politician unselectable clearfix");
      if (!IsNullOrWhiteSpace(idPrefix))
        div.ID = idPrefix + politician.PoliticianKey();
      Report.CreatePoliticianImageTag(politician.PoliticianKey(), 35, noCache).AddTo(div);
      var text = FormatName(politician);
      if (!IsNullOrWhiteSpace(politician.PartyCode()))
        text += " (" + politician.PartyCode() + ")";
      new HtmlDiv {InnerHtml = text}.AddTo(div, "name");
      text = OfficeDescription();
      if (!IsNullOrWhiteSpace(text))
        new HtmlDiv {InnerHtml = text}.AddTo(div, "office");
      text = AddressLine();
      if (!IsNullOrWhiteSpace(text))
        new HtmlDiv {InnerHtml = text}.AddTo(div, "address");
      return div;
    }

    public static string GetCandidateListHtml(string partialName,
      string[] selectedPoliticians, string officeKeyOrStateCode, string[] currentCandidates,
      bool fullAlphaNameOnly = false)
    {
      return GetCandidateList(partialName, selectedPoliticians, officeKeyOrStateCode,
        currentCandidates, fullAlphaNameOnly).RenderToString();
    }

    public static string GetFormattedName(string politicianKey, bool includeAddOn = false,
      int breakAfterPosition = 0)
    {
      return VotePage.GetPageCache().Politicians
        .GetPoliticianName(politicianKey, includeAddOn, breakAfterPosition);
    }

    public static string GetFormattedNameUncached(string politicianKey,
      bool includeAddOn = false, int breakAfterPosition = 0)
    {
      return PageCache.GetTemporary().Politicians
        .GetPoliticianName(politicianKey, includeAddOn, breakAfterPosition);
    }

    public static OfficeClass GetOfficeClass(string politicianKey)
    {
      var cache = VotePage.GetPageCache();
      return Offices.GetOfficeClass(cache,
        cache.Politicians.GetLiveOfficeKey(politicianKey));
    }

    public static string GetOfficeStatusDescription(this PoliticianStatus officeStatus)
    {
      switch (officeStatus)
      {
        case PoliticianStatus.InFutureViewableElection:
        case PoliticianStatus.InFutureNonviewableElection:
        case PoliticianStatus.InFutureUncreatedElection:
          return "Candidate for ";

        case PoliticianStatus.InFutureViewableElectionAsRunningMate:
        case PoliticianStatus.InFutureNonviewableElectionAsRunningMate:
          return "Running Mate for ";

        case PoliticianStatus.Incumbent:
          return "Currently Elected ";

        case PoliticianStatus.IncumbentRunningMate:
          return "Currently Elected Running Mate for ";

        case PoliticianStatus.InPreviousElection:
          return "Previous Candidate for ";

        case PoliticianStatus.InPreviousElectionAsRunningMate:
          return "Previous Candidate Running Mate for ";

        default:
          return Empty;
      }
    }

    public static string GetOfficeStatusDescription(this PoliticianStatus officeStatus,
      string officeName)
    {
      return officeStatus.GetOfficeStatusDescription() + officeName;
    }

    public static string GetStateCodeFromKey(string politicianKey)
    {
      if (politicianKey == null || politicianKey.Length < 2) return Empty;
      return politicianKey.Substring(0, 2).ToUpperInvariant();
    }

    public static bool IsValid(string politicianKey)
    {
      return VotePage.GetPageCache().Politicians.Exists(politicianKey);
    }

    public static void MoveVideoToAnswers(string politicianKey)
    {
      var politician = GetData(politicianKey)[0];
      const string issueKey = "ALLPersonal";
      const string questionKey = "ALLPersonal440785"; // Why I'm Running
      var youTubeId = politician.YouTubeWebAddress.GetYouTubeVideoId();
      VideoInfo videoInfo = null;
      if (!IsNullOrWhiteSpace(youTubeId))
        videoInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
      if (videoInfo?.IsValid == true)
      {
        // get Why I'm Running answers
        var answers =
          Answers.GetDataByPoliticianKeyQuestionKey(politician.PoliticianKey, questionKey);
        if (answers.All(row => row.YouTubeUrl.GetYouTubeVideoId() != youTubeId))
        {
          // doesn't exist -- add to first without a YouTubeUrl
          var rowToUpdate =
            answers.FirstOrDefault(row => IsNullOrWhiteSpace(row.YouTubeUrl));
          if (rowToUpdate == null)
          {
            // new to add a row
            {
              rowToUpdate = answers.NewRow(politician.PoliticianKey, questionKey,
                Answers.GetNextSequence(politician.PoliticianKey, questionKey),
                GetStateCodeFromKey(politician.PoliticianKey), issueKey, Empty, Empty,
                VotePage.DefaultDbDate, "curt", Empty, Empty, default, Empty, Empty,
                VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty, Empty, Empty,
                default, VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty);
              answers.AddRow(rowToUpdate);
            }
          }

          // update the row

          var description = videoInfo.Description.Trim();
          if (IsNullOrWhiteSpace(description) ||
            description.Length > VideoInfo.MaxVideoDescriptionLength)
            description = videoInfo.Title.Trim();
          rowToUpdate.YouTubeUrl = politician.YouTubeWebAddress;
          rowToUpdate.YouTubeDescription = description;
          rowToUpdate.YouTubeRunningTime = videoInfo.Duration;
          rowToUpdate.YouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
          rowToUpdate.YouTubeSourceUrl = Empty;
          rowToUpdate.YouTubeDate = videoInfo.PublishedAt;
          rowToUpdate.YouTubeRefreshTime = VotePage.DefaultDbDate;
          rowToUpdate.YouTubeAutoDisable = Empty;
          Answers.UpdateTable(answers);
        }
      }
      UpdateYouTubeWebAddress(Empty, politician.PoliticianKey);
    }

    public static void MoveVideoToAnswersNew(string politicianKey)
    {
      var politician = GetData(politicianKey)[0];
      var questionId = Issues.QuestionId.WhyIAmRunning.ToInt();
      var youTubeId = politician.YouTubeWebAddress.GetYouTubeVideoId();
      VideoInfo videoInfo = null;
      if (!IsNullOrWhiteSpace(youTubeId))
        videoInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
      if (videoInfo?.IsValid == true)
      {
        // get Why I'm Running answers
        var answers = Answers2.GetDataByPoliticianKeyQuestionId(politician.PoliticianKey,
          questionId);
        if (answers.All(row => row.YouTubeUrl.GetYouTubeVideoId() != youTubeId))
        {
          // doesn't exist -- add to first without a YouTubeUrl
          var rowToUpdate =
            answers.FirstOrDefault(row => IsNullOrWhiteSpace(row.YouTubeUrl));
          if (rowToUpdate == null)
          {
            // new to add a row
            {
              rowToUpdate = answers.NewRow(politician.PoliticianKey, questionId,
                Answers.GetNextSequenceNew(politician.PoliticianKey, questionId.ToString()),
                Empty, Empty, VotePage.DefaultDbDate, "curt", Empty, Empty, default, Empty, 
                Empty, VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty, Empty, Empty,
                default, VotePage.DefaultDbDate, VotePage.DefaultDbDate, Empty);
              answers.AddRow(rowToUpdate);
            }
          }

          // update the row

          var description = videoInfo.Description.Trim();
          if (IsNullOrWhiteSpace(description) ||
            description.Length > VideoInfo.MaxVideoDescriptionLength)
            description = videoInfo.Title.Trim();
          rowToUpdate.YouTubeUrl = politician.YouTubeWebAddress;
          rowToUpdate.YouTubeDescription = description;
          rowToUpdate.YouTubeRunningTime = videoInfo.Duration;
          rowToUpdate.YouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
          rowToUpdate.YouTubeSourceUrl = Empty;
          rowToUpdate.YouTubeDate = videoInfo.PublishedAt;
          rowToUpdate.YouTubeRefreshTime = VotePage.DefaultDbDate;
          rowToUpdate.YouTubeAutoDisable = Empty;
          Answers2.UpdateTable(answers);
        }
      }
      UpdateYouTubeWebAddress(Empty, politician.PoliticianKey);
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}