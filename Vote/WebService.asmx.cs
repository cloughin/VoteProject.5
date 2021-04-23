using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using DB.VoteCache;
using DB.VoteLog;
using Vote.Reports;
using DonationNagsRow = DB.VoteCacheLocal.DonationNagsRow;
using static System.String;

namespace Vote
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [ToolboxItem(false)]
  [ScriptService]
  public class WebService : System.Web.Services.WebService
  {
    private static string EncodeBallotChoices(object choices, int logId)
    {
      // offices separated by $
      // candidates separated by |
      // writeIn indicated by *
      string result = null;
      if (choices is Dictionary<string, object> officeDictionary)
      {
        var offices = new List<string> {logId.ToString(CultureInfo.InvariantCulture)};
        foreach (var officeKey in officeDictionary.Keys)
        {
          if (!(officeDictionary[officeKey] is Dictionary<string, object>
            candidateDictionary)) // it's a ballot measure
          {
            var key = Referendums.GetIdByReferendumKey(officeKey);
            var val = officeDictionary[officeKey] as string;
            // > indicates ballot measure
            offices.Add($"{key}|>{val}");
          }
          else
          {
            var keys = new List<string> {Offices.GetId(officeKey).ToString()};
            foreach (var politicianKey in candidateDictionary.Keys)
            {
              if (candidateDictionary[politicianKey] is int)
              {
                // politician key
                keys.Add(Politicians.GetId(politicianKey).ToString());
              }
              else
              {
                if (candidateDictionary[politicianKey] is string writeIn)
                {
                  writeIn = Uri.EscapeUriString(Regex.Replace(writeIn, "[$|]", "-"));
                  keys.Add($"*{writeIn}");
                }
              }
            }

            offices.Add(Join("|", keys));
          }
        }

        result = Join("$", offices);

        // prepend a simple checksum
        result = $"{Math.Abs(result.GetMd5HashString(true))}${result}";
      }

      return result;
    }

    public static string FormatShareUrl(string url, object choices)
    {
      var uri = new Uri(url);
      var qsc = QueryStringCollection.Parse(uri.Query);

      const int logId = 0; // for now
      var encodedChoices = EncodeBallotChoices(choices, logId);

      // remove any old friend and choices
      qsc.Remove("friend");
      qsc.Remove("choices");

      // add in new
      qsc.Add("friend", "Facebook-friend"); // for now
      qsc.Add("choices", encodedChoices);

      var newUri = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath)
      {
        Query = qsc.ToString()
      };
      return newUri.ToString();
    }

    public static string[] GetComponentsFromCookies()
    {
      var components = HttpContext.Current.Request.Cookies["Components"]?.Value;
      if (IsNullOrWhiteSpace(components)) return null;
      // ReSharper disable once PossibleNullReferenceException
      var array = components.Split('|').Select(HttpUtility.UrlDecode).ToArray();
      // address, city, state required
      if (array.Length != 5 || IsNullOrWhiteSpace(array[0]) &&
        IsNullOrWhiteSpace(array[1]) && IsNullOrWhiteSpace(array[2]))
        return null;
      return array;
    }

    public static (double? Latitude, double? Longitude) GetGeoFromCookies()
    {
      double? latitude = null;
      double? longitude = null;
      var geo = HttpContext.Current.Request.Cookies["Geo"]?.Value;
      if (!IsNullOrWhiteSpace(geo) && PublicMasterPage.IsCookieTrusted)
      {
        var coords = geo.Split(',');
        if (coords.Length == 2)
        {
          if (double.TryParse(coords[0], out var lat) &&
            double.TryParse(coords[1], out var lng))
          {
            latitude = Math.Round(lat, 6);
            longitude = Math.Round(lng, 6);
          }
        }
      }

      return (latitude, longitude);
    }

    //private static string MakeSetCookiesUrl(Uri uri, string stateCode, string county, string congress,
    //  string upper, string lower, string district, string place, string elementary, string secondary, 
    //  string unified, string cityCouncil, string countySupervisors, string schoolDistrictDistrict, 
    //  string geo, string address, string components)
    //{
    //  var qsc = new QueryStringCollection
    //  {
    //    {"State", stateCode},
    //    {"Congress", congress},
    //    {"StateSenate", upper},
    //    {"StateHouse", lower},
    //    {"County", county},
    //    {"District", district},
    //    {"Place", place},
    //    {"Esd", elementary},
    //    {"Ssd", secondary},
    //    {"Usd", unified},
    //    {"Cc", cityCouncil},
    //    {"Cs", countySupervisors},
    //    {"Sdd", schoolDistrictDistrict},
    //    {"Geo", geo},
    //    {"Page", HttpUtility.UrlEncode(uri.ToString())},
    //    {"Address", address},
    //    {"Components", components}
    //  };
    //  return UrlManager.GetStateUri(UrlManager.CurrentDomainDataCode == "US"
    //    ? stateCode
    //    : Empty, "SetCookies.aspx", qsc).ToString();
    //}

    private static string StripUsa(string input)
    {
      input = input.Trim();
      if (input.EndsWith(", USA", StringComparison.OrdinalIgnoreCase))
        input = input.Substring(0, input.Length - 5);
      else if (input.EndsWith(", United States", StringComparison.OrdinalIgnoreCase))
        input = input.Substring(0, input.Length - 15);
      return input;
    }

    public static void UpdateAddresses(string email, string type, string firstname,
      string lastname, IReadOnlyList<string> components, string stateCode, string congress,
      string stateSenate, string stateHouse, string county, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict, double? latitude,
      double? longitude)
    {
      var sendSampleBallots = type == "SBRL";
      var sentBallotChoices = type == "SHRB";
      // update addresses record(s) or add new
      if (congress.Length == 3) congress = congress.Substring(1);
      var addresses = Addresses.GetDataByEmail(email);
      if (addresses.Count == 0)
      {
        // create new
        Addresses.Insert(firstname, lastname, components?[0].SafeString(),
          components?[1].SafeString(), stateCode, components?[3].SafeString(),
          components?[4].SafeString(), email, Empty, DateTime.UtcNow, type, false,
          sendSampleBallots, sentBallotChoices, VotePage.DefaultDbDate, Empty, congress,
          stateSenate, stateHouse, county, district.SafeString(), place.SafeString(),
          elementary.SafeString(), secondary.SafeString(), unified.SafeString(),
          cityCouncil.SafeString(), countySupervisors.SafeString(),
          schoolDistrictDistrict.SafeString(), latitude, longitude, DateTime.UtcNow, 0,
          VotePage.DefaultDbDate, false);
      }
      else
      {
        foreach (var address in addresses)
        {
          if (!IsNullOrWhiteSpace(firstname) && !IsNullOrWhiteSpace(lastname))
          {
            address.FirstName = firstname;
            address.LastName = lastname;
          }

          if (!IsNullOrWhiteSpace(stateCode))
            address.StateCode = stateCode;
          if (!address.SendSampleBallots && sendSampleBallots)
            address.SendSampleBallots = true;
          if (!address.SentBallotChoices && sentBallotChoices)
            address.SentBallotChoices = true;
          if (latitude != null && longitude != null)
          {
            address.Latitude = latitude;
            address.Longitude = longitude;
            address.DistrictLookupDate = DateTime.UtcNow;
          }

          if (components != null)
          {
            address.Address = components[0];
            address.City = components[1];
            address.Zip5 = components[3];
            address.Zip4 = components[4];
          }

          if (!IsNullOrWhiteSpace(congress) && !IsNullOrWhiteSpace(stateSenate) &&
            Offices.IsValidStateHouseDistrict(stateHouse, stateCode) &&
            !IsNullOrWhiteSpace(county))
          {
            address.CongressionalDistrict = congress;
            address.StateSenateDistrict = stateSenate;
            address.StateHouseDistrict = stateHouse;
            address.County = county;
            address.District = district.SafeString();
            address.Place = place.SafeString();
            address.Elementary = elementary.SafeString();
            address.Secondary = secondary.SafeString();
            address.Unified = unified.SafeString();
            address.CityCouncil = cityCouncil.SafeString();
            address.CountySupervisors = countySupervisors.SafeString();
            address.SchoolDistrictDistrict = schoolDistrictDistrict.SafeString();
          }
        }

        Addresses.UpdateTable(addresses);
      }
    }

    private static void ValidateReferrer()
    {
      if (!UrlManager.IsCanonicalHostName(HttpContext.Current.Request.UrlReferrer?.Host))
        throw new Exception("Unauthorized");
    }

    // ReSharper disable NotAccessedField.Global
    public class DonationNag
    {
      public string MessageHeading;
      public string MessageText;
      public int NextMessageNumber;
    }

    public class FindLocationResult
    {
      public string FormattedAddress;
      public string Congress;
      public string StateSenate;
      public string StateHouse;
      public string District;
      public string Place;
      public string County;
      public string StateCode;
      public string Elementary;
      public string Secondary;
      public string Unified;
      public string CityCouncil;
      public string CountySupervisors;
      public string SchoolDistrictDistrict;
      public string RedirectUrl;
      public string DomainCode;
      public string ErrorMessage;
      public bool Success;
    }

    public class UpcomingElectionInfo
    {
      public readonly List<UpcomingElection> Elections = new List<UpcomingElection>();
      public bool IsPast;
    }

    public class UpcomingElection
    {
      public string Description;
      public string Href;
      public bool HasContests;
    }

    // ReSharper restore NotAccessedField.Global

    [WebMethod]
    public void BallotShareAbandoned()
    {
      ValidateReferrer();
      // non-critical -- no concurrency worries
      Master.UpdateShareBallotAbandons(Master.GetShareBallotAbandons(0) + 1);
    }

    [WebMethod]
    public object DecodeBallotChoices(string choices)
    {
      ValidateReferrer();
      try
      {
        var officeDictionary = new Dictionary<string, object>();
        var officeSplit = choices.Split('$');
        if (officeSplit.Length < 3) // need at least checksum, id, and one office
          throw new VoteException();

        // validate checksum
        if (!int.TryParse(officeSplit[0], out var checksum))
          throw new VoteException();
        if (Math.Abs(choices.Substring(choices.IndexOf('$') + 1).GetMd5HashString(true)) != checksum)
          throw new VerificationException();

        // update references
        if (!int.TryParse(officeSplit[1], out var logId))
          throw new VoteException();
        // don't worry about concurrency -- not critical
        LogBallotSharing.UpdateSharesById(LogBallotSharing.GetSharesById(logId, 0) + 1,
          logId);

        for (var inx1 = 1; inx1 < officeSplit.Length; inx1++)
        {
          var office = officeSplit[inx1];
          var candidateSplit = office.Split('|');
          if (candidateSplit.Length > 1)
          {
            if (!int.TryParse(candidateSplit[0], out var id)) id = -1;

            if (candidateSplit[1].StartsWith(">")) // ballot measure
            {
              var referendumKey = Referendums.GetReferendumKeyById(id);
              if (!IsNullOrWhiteSpace(referendumKey))
                officeDictionary.Add(referendumKey.ToUpperInvariant(),
                  candidateSplit[1].Substring(1));
            }
            else
            {
              var officeKey = Offices.GetOfficeKeyById(id);
              if (!IsNullOrWhiteSpace(officeKey))
              {
                var candidateDictionary = new Dictionary<string, object>();
                for (var inx2 = 1; inx2 < candidateSplit.Length; inx2++)
                {
                  var candidate = candidateSplit[inx2];
                  if (candidate.Length > 1 && candidate.StartsWith("*")) // writeIn
                  {
                    var writeIn = Uri.UnescapeDataString(candidate.Substring(1)).Trim();
                    if (!IsNullOrWhiteSpace(writeIn))
                      candidateDictionary.Add("writeIn", writeIn);
                  }
                  else
                  {
                    if (!int.TryParse(candidate, out id)) id = -1;
                    var politicianKey = Politicians.GetPoliticianKeyById(id);
                    if (!IsNullOrWhiteSpace(politicianKey))
                      candidateDictionary.Add(politicianKey.ToUpperInvariant(), 1);
                  }
                }

                if (candidateDictionary.Count > 0)
                  officeDictionary.Add(officeKey.ToUpperInvariant(), candidateDictionary);
              }
            }
          }
        }

        return officeDictionary.Count == 0 ? null : officeDictionary;
      }
      catch (Exception /*ex*/)
      {
        return null;
      }
    }

    [WebMethod]
    public FindLocationResult FindLocation(string formattedAddress, string components,
      string stateCode, double latitude, double longitude, bool forEnrollment,
      string forEnrollmentEmail)
    {
      ValidateReferrer();

      try
      {
        var redirectUrl = forEnrollment
          ? UrlManager.GetSampleBallotEnrolledPageUri(forEnrollmentEmail).ToString()
          : null;
        var tigerData = TigerLookup.Lookup(latitude, longitude);
        formattedAddress = StripUsa(formattedAddress);
        var result = new FindLocationResult
        {
          FormattedAddress = formattedAddress,
          DomainCode = UrlManager.CurrentDomainDataCode,
          StateCode = stateCode,
          Congress = tigerData.Congress,
          County = tigerData.County,
          StateSenate = tigerData.Upper,
          StateHouse = tigerData.Lower,
          District = tigerData.District,
          Place = tigerData.Place,
          Elementary = tigerData.Elementary,
          Secondary = tigerData.Secondary,
          Unified = tigerData.Unified,
          CityCouncil = tigerData.CityCouncil,
          CountySupervisors = tigerData.CountySupervisors,
          SchoolDistrictDistrict = tigerData.SchoolDistrictDistrict,
          Success = true,
          RedirectUrl = redirectUrl
          //RedirectUrl = MakeSetCookiesUrl(uri, stateCode,
          //  tigerData.County, tigerData.Congress, tigerData.Upper, tigerData.Lower,
          //  tigerData.District, tigerData.Place, tigerData.Elementary, tigerData.Secondary,
          //  tigerData.Unified, tigerData.CityCouncil, tigerData.CountySupervisors,
          //  tigerData.SchoolDistrictDistrict, $"{Math.Round(latitude, 6)},{Math.Round(longitude, 6)}", 
          //  formattedAddress, components)
        };

        return result;
      }
      catch (Exception ex)
      {
        return new FindLocationResult {ErrorMessage = $"An error occurred: {ex.Message}"};
      }
    }

    [WebMethod]
    public string GetAds(string electionKey, string officeKey, string adKey)
    {
      ValidateReferrer();
      try
      {
        int.TryParse(adKey, out var orgId);
        return Utility.RenderBannerAd("C", Empty, electionKey, officeKey, adKey == "show", orgId) +
          Utility.RenderAds(electionKey, officeKey, adKey);
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetAds", e);
        throw;
      }
    }

    [WebMethod]
    public string GetBannerAd(string adType, string stateCode, string electionKey, string officeKey, string adKey)
    {
      ValidateReferrer();
      try
      {
        int.TryParse(adKey, out var orgId);
        return Utility.RenderBannerAd(adType, stateCode, electionKey, officeKey, adKey == "show", orgId);
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetBannerAd", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetCongressionalDistricts(string stateCode)
    {
      ValidateReferrer();
      try
      {
        // reduce CongressionalDistrict codes from 3 to 2 characters
        return Offices.GetDistrictItems(stateCode, OfficeClass.USHouse).Select(i =>
        {
          i.Value = i.Value.Substring(1);
          return i;
        }).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetCongressionalDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public SimpleListItem[] GetCounties(string stateCode)
    {
      ValidateReferrer();
      try
      {
        return CountyCache.GetCountiesByState(stateCode).Select(countyCode =>
          new SimpleListItem
          {
            Text = Counties.GetName(stateCode, countyCode), Value = countyCode
          }).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetCounties", e);
        throw;
      }
    }

    [WebMethod]
    public DonationNag GetDonationNag(int cookieIndex)
    {
      ValidateReferrer();
      var result = new DonationNag();
      if (MemCache.IsNaggingEnabled)
      {
        var donationNagsTable = MemCache.CachedDonationNagsTable;
        var donationNagsRow = donationNagsTable.Cast<DonationNagsRow>()
          .FirstOrDefault(row => row.MessageNumber == cookieIndex);
        var maxIndex = Enumerable.Select(donationNagsTable.Cast<DonationNagsRow>(),
          row => row.MessageNumber).Max();
        result.NextMessageNumber = Math.Min(cookieIndex + 1, maxIndex);
        if (donationNagsRow != null) // no dialog if null
        {
          result.MessageHeading = donationNagsRow.MessageHeading;
          result.MessageText = donationNagsRow.MessageText;
          if (donationNagsRow.NextMessageNumber != null)
            result.NextMessageNumber = donationNagsRow.NextMessageNumber.Value;
        }
      }
      else
      {
        result.MessageHeading = Empty;
        result.MessageText = Empty;
        result.NextMessageNumber = cookieIndex;
      }

      return result;
    }

    [WebMethod]
    public DonationNag GetDonationNagNew(int cookieIndex)
    {
        ValidateReferrer();
      var result = new DonationNag();
      if (MemCache.IsNaggingEnabled)
      {
        var donationNagsTable = DonationNags2.GetAllData();
        var donationNagsRow = donationNagsTable.FirstOrDefault(row => row.Id == cookieIndex) 
          ?? donationNagsTable.FirstOrDefault(row => row.Id == 1);
        if (donationNagsRow != null)
        {
          result.MessageHeading = "Pardon the interruption, but &hellip;";
          result.MessageText = donationNagsRow.Message;
          result.NextMessageNumber = cookieIndex < donationNagsTable.Max(row => row.Id)
            ? cookieIndex + 1
            : 1;
          return result;
        }
      }

      result.MessageHeading = Empty;
      result.MessageText = Empty;
      result.NextMessageNumber = cookieIndex;

      return result;
    }

    //[WebMethod]
    //public LdsInfo GetLdsInfo(string zip5, string zip4)
    //{
    //  ValidateReferrer();
    //  return AddressFinder.GetLdsInfo(zip5, zip4);
    //}

    [WebMethod]
    public string GetMoreText(string key)
    {
      ValidateReferrer();
      var keyPart = key.Split(':');
      var result = Empty;
      if (keyPart.Length >= 3) // need at least type, min and max
      {
        var type = keyPart[0];
        if (int.TryParse(keyPart[1], out var min) && int.TryParse(keyPart[2], out var max))
          switch (type)
          {
            case "answer":
            case "ytdesc":
            case "fbdesc":
              if (keyPart.Length == 6) // need PoliticianKey, QuestionKey and Sequence
              {
                var politicianKey = keyPart[3];
                var questionKey = keyPart[4];
                string text = null;
                if (int.TryParse(keyPart[5], out var sequence))
                  switch (type)
                  {
                    case "answer":
                      int.TryParse(questionKey, out var questionId);
                      text = Answers2
                        .GetAnswerByPoliticianKeyQuestionIdSequence(politicianKey,
                          questionId, sequence).SafeString();

                      break;

                    case "ytdesc":
                    case "fbdesc":
                    {
                      DataRow row = null;
                      if (sequence == -1) // from politicians
                        row = Politicians.GetPoliticianIntroReportData(politicianKey);
                      else
                      {
                        var table = Answers2.GetVideoDescData(politicianKey,
                          int.Parse(questionKey), sequence);
                        if (table.Count == 1)
                          row = table[0];
                      }

                      if (row != null)
                        if (type == "ytdesc")
                          text = ResponsiveIssuesReport.FormatVideoDescription(
                            row.YouTubeDescription().SafeString(),
                            row.YouTubeRunningTime());
                        else
                          text = ResponsiveIssuesReport.FormatVideoDescription(
                            row.FacebookVideoDescription().SafeString(),
                            row.FacebookVideoRunningTime());
                      break;
                    }
                  }
                if (!IsNullOrEmpty(text))
                  result = VotePage.GetMorePart2(text, min, max);
              }

              break;
          }
      }

      return result;
    }

    [WebMethod]
    public string GetShareUrl(string url, object choices)
    {
      ValidateReferrer();
      return FormatShareUrl(url, choices);
    }

    [WebMethod]
    public SimpleListItem[] GetStateHouseDistricts(string stateCode)
    {
      ValidateReferrer();
      try
      {
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateHouse).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetStateHouseDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public string GetStateLinks(string input)
    {
      ValidateReferrer();
      var div = new HtmlDiv();

      // Add the state election authority first
      var p = new HtmlP().AddTo(div);
      new HtmlAnchor
      {
        HRef = StateCache.GetUri(input).ToString(),
        InnerText =
          $"Official {StateCache.GetStateName(input)} Election Authority Website",
        Target = "_blank"
      }.AddTo(p);

      ForResearchPage.AddStateLinksToDiv(div, input, true, true);

      var htmlArray = div.Controls.OfType<Control>().Select(c => c.RenderToString())
        .ToArray();

      return Join("", htmlArray);
    }

    [WebMethod]
    public SimpleListItem[] GetStateSenateDistricts(string stateCode)
    {
      ValidateReferrer();
      try
      {
        return Offices.GetDistrictItems(stateCode, OfficeClass.StateSenate).ToArray();
      }
      catch (Exception e)
      {
        VotePage.LogException("WebService/GetStateSenateDistricts", e);
        throw;
      }
    }

    [WebMethod]
    public TigerLookupAllData GetTigerData(double latitude, double longitude)
    {
      ValidateReferrer();
      return TigerLookup.LookupAll(latitude, longitude);
    }

    //[WebMethod]
    //public UpcomingElectionInfo GetUpcomingElections(string stateCode, string congress,
    //  string stateSenate, string stateHouse, string county, string district, string place,
    //  string elementary, string secondary, string unified, string cityCouncil,
    //  string countySupervisors, string schoolDistrictDistrict)
    //{
    //  ValidateReferrer();

    //  var futureElectionsTable =
    //    Elections.GetFutureViewableDisplayDataByStateCode(stateCode);

    //  var upcomingElectionInfo = new UpcomingElectionInfo();
    //  foreach (var row in futureElectionsTable)
    //  {
    //    if (Elections.IsRelevantElection(row.ElectionKey, congress, stateSenate, stateHouse,
    //      county, district, place, elementary, secondary, unified, cityCouncil, countySupervisors,
    //      schoolDistrictDistrict))
    //      upcomingElectionInfo.Elections.Add(new UpcomingElection
    //      {
    //        Description = row.ElectionDesc,
    //        Href = UrlManager.GetBallotPageUri(row.ElectionKey, congress, stateSenate,
    //          stateHouse, county, district, place, elementary, secondary, unified,
    //          cityCouncil, countySupervisors, schoolDistrictDistrict, null).ToString(),
    //        HasContests = Elections.HasContestsOrBallotMeasures(row.ElectionKey, congress,
    //          stateSenate, stateHouse, county, district, place, elementary, secondary,
    //          unified, cityCouncil, countySupervisors, schoolDistrictDistrict)
    //      });
    //  }

    //  // mod 1/24/2013
    //  // if there are no future elections, return the most recent past election(s)
    //  if (upcomingElectionInfo.Elections.Count == 0)
    //  {
    //    upcomingElectionInfo.IsPast = true;
    //    var pastElectionsTable = Elections.GetPastViewableDisplayDataByStateCode(stateCode);

    //    if (pastElectionsTable.Count != 0)
    //    {
    //      DateTime? date = null;
    //      foreach (var row in pastElectionsTable.Where(r => Elections.IsRelevantElection(r.ElectionKey, 
    //        congress, stateSenate, stateHouse, county, district, place, elementary, secondary, 
    //        unified, cityCouncil, countySupervisors, schoolDistrictDistrict)))
    //      {
    //        if (date == null) date = row.ElectionDate;
    //        if (row.ElectionDate != date) break;
    //        upcomingElectionInfo.Elections.Add(new UpcomingElection
    //        {
    //          Description = row.ElectionDesc,
    //          Href = UrlManager.GetBallotPageUri(row.ElectionKey, congress, stateSenate,
    //            stateHouse, county, district, place, elementary, secondary, unified,
    //            cityCouncil, countySupervisors, schoolDistrictDistrict, null).ToString(),
    //          HasContests = Elections.HasContestsOrBallotMeasures(row.ElectionKey,
    //            congress, stateSenate, stateHouse, county, district, place, elementary,
    //            secondary, unified, cityCouncil, countySupervisors,
    //            schoolDistrictDistrict)
    //        });
    //      }
    //    }
    //  }

    //  return upcomingElectionInfo;
    //}

    [WebMethod]
    public UpcomingElectionInfo GetUpcomingElections(string stateCode, string congress,
      string stateSenate, string stateHouse, string county, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict)
    {
      ValidateReferrer();

      var futureElectionsTable =
        Elections.GetFutureViewableDisplayDataByStateCode(stateCode);

      var upcomingElectionInfo = new UpcomingElectionInfo();
      foreach (var row in futureElectionsTable)
      {
        if (Elections.HasContestsOrBallotMeasures(row.ElectionKey, congress, stateSenate, stateHouse,
          county, district, place, elementary, secondary, unified, cityCouncil, countySupervisors,
          schoolDistrictDistrict))
          upcomingElectionInfo.Elections.Add(new UpcomingElection
          {
            Description = row.ElectionDesc,
            Href = UrlManager.GetBallotPageUri(row.ElectionKey, congress, stateSenate,
              stateHouse, county, district, place, elementary, secondary, unified,
              cityCouncil, countySupervisors, schoolDistrictDistrict, null).ToString(),
            HasContests = true // no longer relevant
          });
      }

      // mod 1/24/2013
      // if there are no future elections, return the most recent past election(s)
      if (upcomingElectionInfo.Elections.Count == 0)
      {
        upcomingElectionInfo.IsPast = true;
        var pastElectionsTable = Elections.GetPastViewableDisplayDataByStateCode(stateCode);

        if (pastElectionsTable.Count != 0)
        {
          DateTime? date = null;
          foreach (var row in pastElectionsTable)
          {
            if (date != null && row.ElectionDate != date) break;
            if (!Elections.HasContestsOrBallotMeasures(row.ElectionKey, congress,
              stateSenate, stateHouse, county, district, place, elementary, secondary,
              unified, cityCouncil, countySupervisors, schoolDistrictDistrict)) continue;
            if (date == null) date = row.ElectionDate;
            upcomingElectionInfo.Elections.Add(new UpcomingElection
            {
              Description = row.ElectionDesc,
              Href = UrlManager.GetBallotPageUri(row.ElectionKey, congress, stateSenate,
                stateHouse, county, district, place, elementary, secondary, unified,
                cityCouncil, countySupervisors, schoolDistrictDistrict, null).ToString(),
              HasContests = true // no longer relevant
            });
          }
        }
      }

      return upcomingElectionInfo;
    }

    [WebMethod]
    public bool IsCookieTrusted()
    {
      return PublicMasterPage.IsCookieTrusted;
    }

    [WebMethod]
    public void LogDebug(string type, string message)
    {
      ValidateReferrer();
      DebugLog.Insert(type, message);
    }

    [WebMethod]
    public string SendBallotChoices(string name, string email, string url, object choices)
    {
      ValidateReferrer();
      var uri = new Uri(url);
      var qsc = QueryStringCollection.Parse(uri.Query);
      var electionKey = qsc["election"];

      // create log record
      var logId = LogBallotSharing.Insert(name, email, DateTime.UtcNow, 0, Empty, // for now
        electionKey, null, qsc["congress"], qsc["statesenate"], qsc["statehouse"],
        qsc["county"], qsc["district"].SafeString(), qsc["place"].SafeString(),
        new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(choices));

      // try to split the name
      var nameSplit = name.Split(' ');
      var first = Empty;
      var last = name;
      if (nameSplit.Length > 1)
      {
        first = Join(" ", nameSplit.Take(nameSplit.Length / 2));
        last = Join(" ", nameSplit.Skip(nameSplit.Length / 2));
      }

      // get and parse the geo from cookies
      var geo = GetGeoFromCookies();
      UpdateAddresses(email, "SHRB", first, last, GetComponentsFromCookies(),
        Elections.GetStateCodeFromKey(electionKey), qsc["congress"], qsc["statesenate"],
        qsc["statehouse"], qsc["county"], qsc["district"], qsc["place"], qsc["esd"],
        qsc["ssd"], qsc["usd"], qsc["cc"], qsc["cs"], qsc["sdd"], geo.Latitude,
        geo.Longitude);

      var encodedChoices = EncodeBallotChoices(choices, logId);

      // remove any old friend and choices
      qsc.Remove("friend");
      qsc.Remove("choices");

      // add in new
      qsc.Add("friend", name);
      qsc.Add("choices", encodedChoices);

      var newUri = new UriBuilder(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath)
      {
        Query = qsc.ToString()
      };

      // update log with url
      LogBallotSharing.UpdateUrlById(newUri.Uri.ToString(), logId);

      // get the template
      var template =
        EmailTemplates.GetDataByNameOwnerTypeOwner("Share Ballot Choices", "U",
          "SpecialTemplates")[0];

      var substitutions =
        new Substitutions("[[ShareChoicesName]]", name, "[[ShareChoicesLink]]",
          Substitutions.CreateWebAnchor(newUri.Uri, "View My Choices"))
        {
          ElectionKey = electionKey
        };

      var subject = substitutions.Substitute(template.Subject);
      var body = substitutions.Substitute(template.Body);

      EmailTemplates.SendEmail(subject, body, "info@vote-usa.org", new[] {email}, null,
        null, 2);

      return null;
    }

    [WebMethod]
    public void SubmitSampleBallotEmail(string email, string siteId, string stateCode,
      string electionKey, string congressionalDistrict, string stateSenateDistrict,
      string stateHouseDistrict, string county, string district, string place,
      string elementary, string secondary, string unified, string cityCouncil,
      string countySupervisors, string schoolDistrictDistrict)
    {
      ValidateReferrer();
      try
      {
        var geo = GetGeoFromCookies();
        UpdateAddresses(email, "SBRL", Empty, Empty, GetComponentsFromCookies(), stateCode,
          congressionalDistrict, stateSenateDistrict, stateHouseDistrict, county, district,
          place, elementary, secondary, unified, cityCouncil, countySupervisors,
          schoolDistrictDistrict, geo.Latitude, geo.Longitude);
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch
      {
      } // tolerate errors
    }

    [WebMethod]
    public string UseThisAddress(bool forEnrollment, string forEnrollmentEmail)
    {
      ValidateReferrer();
      var request = HttpContext.Current.Request;
      var address = request.Cookies["Address"]?.Value.SafeString();
      //var components = request.Cookies["Components"]?.Value.SafeString();
      var stateCode = request.Cookies["State"]?.Value.SafeString();
      var congress = request.Cookies["Congress"]?.Value.SafeString();
      var stateSenate = request.Cookies["StateSenate"]?.Value.SafeString();
      //var stateHouse = request.Cookies["StateHouse"]?.Value.SafeString();
      //var district = request.Cookies["District"]?.Value.SafeString();
      //var place = request.Cookies["Place"]?.Value.SafeString();
      //var elementary = request.Cookies["Elementary"]?.Value.SafeString();
      //var secondary = request.Cookies["Secondary"]?.Value.SafeString();
      //var unified = request.Cookies["Unified"]?.Value.SafeString();
      //var cityCouncil = request.Cookies["CityCouncil"]?.Value.SafeString();
      //var countySupervisors = request.Cookies["CountySupervisors"]?.Value.SafeString();
      //var schoolDistrictDistrict = request.Cookies["SchoolDistrictDistrict"]?.Value.SafeString();
      var geo = request.Cookies["Geo"]?.Value.SafeString();
      var county = request.Cookies["County"]?.Value.SafeString();
      if (IsNullOrWhiteSpace(address) || IsNullOrWhiteSpace(stateCode) ||
        IsNullOrWhiteSpace(congress) || IsNullOrWhiteSpace(stateSenate) ||
        IsNullOrWhiteSpace(county) || IsNullOrWhiteSpace(geo))
        return Empty;
      return UrlManager.GetSampleBallotEnrolledPageUri(forEnrollmentEmail).ToString();
    }
  }
}