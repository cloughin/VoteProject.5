using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class DownloadVotersEdgeCsvPage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Server.ScriptTimeout = 1800;
      var electionKey = Request.QueryString["election"];
      var csvType = Request.QueryString["type"];
      var includeCandidates = csvType != "BM";
      var includeCandidateInfo = csvType == "NA" || csvType == "WA";
      var includeAnswers = csvType == "OA" || csvType == "WA";
      var includeBallotMeasures = csvType == "BM"; 
      if (IsNullOrWhiteSpace(electionKey))
        throw new VoteException("Election key is missing.");
      var electionDescription = Elections.GetElectionDesc(electionKey);
      if (IsNullOrWhiteSpace(electionDescription))
        throw new VoteException("Election key is invalid.");

      // make sure it's a valid filename
      var invalidFileChars = Path.GetInvalidFileNameChars();
      electionDescription = Regex.Replace(electionDescription, ".",
        match => new[] {' ', ',', '"', '\''}.Contains(match.Value[0]) ||
          invalidFileChars.Contains(match.Value[0])
            ? "_"
            : match.Value);
      electionDescription = Regex.Replace(electionDescription, "__+", "_");

      // get the data
      var table = includeBallotMeasures
        ? Referendums.GetElectionCsvReferendumData(electionKey)
        : ElectionsPoliticians.GetElectionCsvCandidateData(electionKey);

      // if we're including answers, get the answers data for each office in the election
      var qas = new List<QuestionAndAnswer>();
      var columns = new List<dynamic>();
      if (includeAnswers)
      {
        var answers = new List<DataRow>();
        // get a list of all offices in the election
        var allOfficeKeys = table.Rows.OfType<DataRow>().Select(r => r.OfficeKey()).Distinct()
          .ToList();
        // collect all the answers
        foreach (var officeKey in allOfficeKeys)
        {
          var oldAnswerCutoff =
            ElectionsOffices.GetOldAnswerCutoffDate(electionKey, officeKey);
          // the GroupBy is to eliminate duplicate answers if a question is in more than one issue
          answers.AddRange(ElectionsPoliticians.GetCompareCandidateIssuesNew(electionKey, officeKey)
            .Rows.OfType<DataRow>()
            .GroupBy(r => new
            {
              PoliticianKey = r.PoliticianKey(),
              QuestionKey = r.QuestionKey(),
              Sequence = r.Sequence()
            })
            .Select(g => g.First()));
          // convert the answers to QuestionAndAnswer format
          foreach (var p in answers.GroupBy(r => r.PoliticianKey()))
          {
            qas.AddRange(ResponsiveIssuesReport.SplitOutVideos(ResponsiveIssuesReport.GetQuestionAndAnswerList(p, 
              Politicians.GetData(p.Key)[0], false)).Where(qa => qa.ResponseDate > oldAnswerCutoff));
          }
        }
        // analyze qas to determine which topic columns we need to include
        columns.AddRange(qas
          .GroupBy(qa =>
            new { qa.QuestionKey, IsYouTube = !IsNullOrWhiteSpace(qa.YouTubeUrl) }).Select(
            g => new
            {
              g.Key.QuestionKey,
              g.Key.IsYouTube,
              Topic = $"{g.First().Question}{(g.Key.IsYouTube ? " Video" : Empty)}"
            }).OrderBy(q => q.Topic));
      }

      // create the csv
      string csv;
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("Jurisdiction");
        csvWriter.AddField("State Code");
        if (csvType != "OK")
        {
          csvWriter.AddField("County");
          csvWriter.AddField("City or District");
          csvWriter.AddField("Election Name");
          csvWriter.AddField("Election Date");
          csvWriter.AddField("VoteUSA Election Id");
        }

        if (includeCandidates && csvType != "OK")
        {
          csvWriter.AddField("Office");
          csvWriter.AddField("Office Class");
          csvWriter.AddField("District");
          csvWriter.AddField("VoteUSA Office Id");
          csvWriter.AddField("Running Mate?");
          csvWriter.AddField("Candidate");
          csvWriter.AddField("First Name");
          csvWriter.AddField("Middle Name");
          csvWriter.AddField("Nickname");
          csvWriter.AddField("Last Name");
          csvWriter.AddField("Suffix");
          csvWriter.AddField("Party");
          csvWriter.AddField("VoteUSA Id");
        }

        if (csvType == "OK")
        {
          csvWriter.AddField("County Code");
          csvWriter.AddField("County");
          csvWriter.AddField("Local Key");
          csvWriter.AddField("Local Name");
          csvWriter.AddField("Election Key");
          csvWriter.AddField("Office Key");
          csvWriter.AddField("Office");
          csvWriter.AddField("Politician Key");
          csvWriter.AddField("Politician Password");
          csvWriter.AddField("Candidate");
          csvWriter.AddField("Party Code");
          csvWriter.AddField("Ad Enabled");
          csvWriter.AddField("YouTube Video Url");
          csvWriter.AddField("YouTube Channel or Playlist Url");
          csvWriter.AddField("Compare Candidates Url");
          csvWriter.AddField("Type");
          csvWriter.AddField("Date");
          csvWriter.AddField("Amount");
          csvWriter.AddField("Email");
          csvWriter.AddField("Banner Ad Url");
        }

        if (includeCandidateInfo)
        {
          csvWriter.AddField("Intro Url");
          csvWriter.AddField("Photo100 Url");
          csvWriter.AddField("Photo200 Url");
          csvWriter.AddField("Photo300 Url");
          csvWriter.AddField("Postal Street Address");
          csvWriter.AddField("Postal City, State Zip");
          csvWriter.AddField("Phone");
          csvWriter.AddField("Email");
          csvWriter.AddField("Date of Birth");
        }

        if (!includeAnswers && !includeBallotMeasures && csvType != "OK")
        {
          csvWriter.AddField("General Philosophy");
          csvWriter.AddField("Personal and Family");
          csvWriter.AddField("Education");
          csvWriter.AddField("Profession");
          csvWriter.AddField("Military");
          csvWriter.AddField("Civic");
          csvWriter.AddField("Political Experience");
          csvWriter.AddField("Religious Affiliation");
          csvWriter.AddField("Accomplishment and Awards");
        }

        if (includeCandidateInfo)
        {
          csvWriter.AddField("Website Url");
          csvWriter.AddField("Facebook Url");
          csvWriter.AddField("YouTube Url");
          csvWriter.AddField("Flickr Url");
          csvWriter.AddField("Twitter Url");
          csvWriter.AddField("RSS Feed Url");
          csvWriter.AddField("Wikipedia Url");
          csvWriter.AddField("BallotPedia Url");
          csvWriter.AddField("Vimeo Url");
          csvWriter.AddField("Google+ Url");
          csvWriter.AddField("LinkedIn Url");
          csvWriter.AddField("Pinterest Url");
          csvWriter.AddField("Blogger Url");
          csvWriter.AddField("Podcast Url");
          csvWriter.AddField("Instagram Url");
          csvWriter.AddField("GoFundMe Url");
          csvWriter.AddField("Crowdpac Url");
        }

        if (includeAnswers)
        {
          foreach (var column in columns)
            csvWriter.AddField(column.Topic);

        }

        if (includeBallotMeasures)
        {
          csvWriter.AddField("Ballot Measure Title");
          csvWriter.AddField("Ballot Measure Description");
          csvWriter.AddField("Ballot Measure Detail");
          csvWriter.AddField("Ballot Measure Detail URL");
          csvWriter.AddField("Ballot Measure Full Text");
          csvWriter.AddField("Ballot Measure Full Text URL");
          csvWriter.AddField("Ballot Measure Passed");
        }

        csvWriter.Write(streamWriter);

        var stateCode = Elections.GetStateCodeFromKey(electionKey);

        // do a first pass to get counties for all locals
        var allLocals = new List<string>();
        foreach (var row in table.Rows.Cast<DataRow>())
        {
          if (!IsNullOrWhiteSpace(row.LocalKey()))
            allLocals.Add(row.LocalKey());
        }

        var countiesForLocals =
          LocalIdsCodes.FindCountiesWithNames(stateCode, allLocals.Distinct());

        var rows = table.Rows.Cast<DataRow>();

        if (csvType == "OK")
          rows = rows.OrderBy(r => r.OfficeLevel())
            //.ThenBy(r => r.DistrictCode())
            //.ThenBy(r => r.OfficeOrderWithinLevel())
            //.ThenBy(r => r.OfficeLine1())
            //.ThenBy(r => r.OfficeLine2())
            .ThenBy(r => Offices.FormatOfficeName(r),
              MixedNumericComparer.Instance)
            .ThenBy(r => r.OrderOnBallot())
            .ThenBy(r => r.PoliticianKey(), StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.IsRunningMate());

        foreach (var row in rows)
        {
          string jurisdiction;
          var politicianKey = Empty;
          if (includeBallotMeasures)
          {
            if (!IsNullOrWhiteSpace(row.LocalKey()))
              jurisdiction = "Local";
            else if (!IsNullOrWhiteSpace(row.CountyCode()))
              jurisdiction = "County";
            else
              jurisdiction = "State";
          }
          else
          {
            politicianKey = row.IsRunningMate()
              ? row.RunningMateKey()
              : row.PoliticianKey();
            switch (Offices.GetElectoralClass(row.OfficeClass()))
            {
              case ElectoralClass.USPresident:
              case ElectoralClass.USSenate:
              case ElectoralClass.USHouse:
                jurisdiction = "Federal";
                break;

              case ElectoralClass.USGovernors:
              case ElectoralClass.State:
                jurisdiction = "State";
                break;

              case ElectoralClass.County:
                jurisdiction = "County";
                break;

              case ElectoralClass.Local:
                jurisdiction = "Local";
                break;

              default:
                jurisdiction = Empty;
                break;
            }
          }

          var photo100Url = Empty;
          if (includeCandidateInfo)
          {
            var qsc100 = new QueryStringCollection
            {
              {
                "id", politicianKey
              },
              {
                "Col", "Headshot100"
              }
            };
            var photo100Uri = UrlManager.GetSiteUri("image.aspx", qsc100);
            photo100Url = new UriBuilder(photo100Uri)
            {
              Scheme = Uri.UriSchemeHttps,
              Host = UrlManager.GetCanonicalLiveHostName(photo100Uri.Host),
              Port = 443
            }.Uri.ToString();
          }

          var photo200Url = Empty;
          if (includeCandidateInfo)
          {
            var qsc200 = new QueryStringCollection
            {
              {"id", politicianKey}, {"Col", "Profile200"}
            };
            var photo200Uri = UrlManager.GetSiteUri("image.aspx", qsc200);
            photo200Url = new UriBuilder(photo200Uri)
            {
              Scheme = Uri.UriSchemeHttps,
              Host = UrlManager.GetCanonicalLiveHostName(photo200Uri.Host),
              Port = 443
            }.Uri.ToString();
          }

          var photo300Url = Empty;
          if (includeCandidateInfo)
          {
            var qsc300 = new QueryStringCollection
            {
              {"id", politicianKey}, {"Col", "Profile300"}
            };
            var photo300Uri = UrlManager.GetSiteUri("image.aspx", qsc300);
            photo300Url = new UriBuilder(photo300Uri)
            {
              Scheme = Uri.UriSchemeHttps,
              Host = UrlManager.GetCanonicalLiveHostName(photo300Uri.Host),
              Port = 443
            }.Uri.ToString();
          }

          var introUrl = Empty;
          if (includeCandidateInfo)
          {
            var introUri = UrlManager.GetIntroPageUri(politicianKey);
            introUrl = new UriBuilder(introUri)
            {
              Scheme = Uri.UriSchemeHttps,
              Host = UrlManager.GetCanonicalLiveHostName(introUri.Host),
              Port = 443
            }.Uri.ToString();
          }

          var district = Empty;
          if (includeCandidates)
          {
            if (int.TryParse(row.DistrictCode(), out var districtNumber))
              district = districtNumber.ToString(CultureInfo.InvariantCulture);
          }

          // convert to simple name if national
          var partyName = Empty;
          if (includeCandidates)
          {
            partyName = Parties.GetNationalPartyDescription(row.PartyCode(),
              row.PartyName());
          }

          var county = IsNullOrWhiteSpace(row.County()) ? Empty : row.County();
          var local = Empty;
          if (!IsNullOrWhiteSpace(row.LocalKey()))
          {
            local = row.LocalDistrict();
            county = Join(", ", countiesForLocals[row.LocalKey()].Select(c => c.Text));
          }

          csvWriter.AddField(jurisdiction);
          csvWriter.AddField(stateCode);
          if (csvType != "OK")
          {
            csvWriter.AddField(county);
            csvWriter.AddField(local);
            csvWriter.AddField(row.ElectionDescription());
            csvWriter.AddField(row.ElectionDate().ToString("d"));
            csvWriter.AddField(row.ElectionKey());
          }

          if (includeCandidates && csvType != "OK")
          {
            csvWriter.AddField(Offices.FormatOfficeName(row));
            csvWriter.AddField(Offices.GetOfficeClassShortDescriptionExtended(row));
            csvWriter.AddField(district);
            csvWriter.AddField(row.OfficeKey());
            csvWriter.AddField(row.IsRunningMate() ? row.PoliticianKey() : Empty);
            csvWriter.AddField(Politicians.FormatName(row));
            csvWriter.AddField(row.FirstName());
            csvWriter.AddField(row.MiddleName());
            csvWriter.AddField(row.Nickname());
            csvWriter.AddField(row.LastName());
            csvWriter.AddField(row.Suffix());
            csvWriter.AddField(partyName);
            csvWriter.AddField(politicianKey);
          }

          if (csvType == "OK")
          {

            var youTubeVideoUrl = Empty;
            var youTubeAdWebAddress = row.AdUrl();
            if (!IsNullOrWhiteSpace(youTubeAdWebAddress))
              youTubeVideoUrl = youTubeAdWebAddress.IsValidYouTubeVideoUrl()
                ? youTubeAdWebAddress
                : "channel or playlist";

            var adEnabled = Empty;
            if (!IsNullOrWhiteSpace(row.AdType()))
              adEnabled = row.AdEnabled() ? "E" : "D";

            var compareUrl =
              UrlManager.GetCompareCandidatesPageUri(row.ElectionKey(), row.OfficeKey()) +
               $"&ad={politicianKey}";

            csvWriter.AddField(row.CountyCode());
            csvWriter.AddField(row.County().SafeString());
            csvWriter.AddField(row.LocalKey());
            csvWriter.AddField(row.LocalDistrict().SafeString());
            csvWriter.AddField(row.ElectionKey());
            csvWriter.AddField(row.OfficeKey());
            csvWriter.AddField(Offices.FormatOfficeName(row.OfficeLine1(),
              row.OfficeLine2(), row.OfficeKey()));
            csvWriter.AddField(politicianKey);
            csvWriter.AddField(row.Password());
            csvWriter.AddField(Politicians.FormatName(row));
            csvWriter.AddField(row.PartyCode().SafeString());
            csvWriter.AddField(adEnabled);
            csvWriter.AddField(youTubeVideoUrl);
            csvWriter.AddField(row.YouTubeWebAddress());
            csvWriter.AddField($"=HYPERLINK(\"{compareUrl}\",\"{compareUrl}\")");
            csvWriter.AddField(Empty);
            csvWriter.AddField(Empty);
            csvWriter.AddField(Empty);
            csvWriter.AddField(Empty);
            csvWriter.AddField(Empty);
          }

          if (includeCandidateInfo)
          {
            csvWriter.AddField(introUrl);
            csvWriter.AddField(photo100Url);
            csvWriter.AddField(photo200Url);
            csvWriter.AddField(photo300Url);
            csvWriter.AddField(row.PublicAddress());
            csvWriter.AddField(row.PublicCityStateZip());
            csvWriter.AddField(row.PublicPhone());
            csvWriter.AddField(row.PublicEmail());
            csvWriter.AddField(row.DateOfBirth().ToString("d"));
          }

          if (!includeAnswers && !includeBallotMeasures && csvType != "OK")
          {
            csvWriter.AddField(row.GeneralStatement().SafeString());
            csvWriter.AddField(row.Personal().SafeString());
            csvWriter.AddField(row.Education().SafeString());
            csvWriter.AddField(row.Profession().SafeString());
            csvWriter.AddField(row.Military().SafeString());
            csvWriter.AddField(row.Civic().SafeString());
            csvWriter.AddField(row.Political().SafeString());
            csvWriter.AddField(row.Religion().SafeString());
            csvWriter.AddField(row.Accomplishments().SafeString());
          }

          if (includeCandidateInfo)
          {
            csvWriter.AddField(row.PublicWebAddress());
            csvWriter.AddField(row.FacebookWebAddress());
            csvWriter.AddField(row.YouTubeWebAddress());
            csvWriter.AddField(row.FlickrWebAddress());
            csvWriter.AddField(row.TwitterWebAddress());
            csvWriter.AddField(row.RssFeedWebAddress());
            csvWriter.AddField(row.WikipediaWebAddress());
            csvWriter.AddField(row.BallotPediaWebAddress());
            csvWriter.AddField(row.VimeoWebAddress());
            csvWriter.AddField(row.GooglePlusWebAddress());
            csvWriter.AddField(row.LinkedInWebAddress());
            csvWriter.AddField(row.PinterestWebAddress());
            csvWriter.AddField(row.BloggerWebAddress());
            csvWriter.AddField(row.PodcastWebAddress());
            csvWriter.AddField(row.WebstagramWebAddress());
            csvWriter.AddField(row.GoFundMeWebAddress());
            csvWriter.AddField(row.CrowdpacWebAddress());
          }

          if (includeAnswers)
          {
            var data = qas.Where(qa => qa.PoliticianKey == politicianKey)
              .OrderByDescending(qa => qa.ResponseDate)
              .ToList();
            foreach (var column in columns)
            {
              var response = data.FirstOrDefault(d => d.QuestionKey == column.QuestionKey &&
                d.HasVideo == column.IsYouTube);
              var field = Empty;
              if (response != null)
                if (response.HasVideo)
                  field = response.YouTubeUrl;
                else
                  field = $"{response.Answer}" +
                    $"\n\n{(IsNullOrWhiteSpace(response.AnswerSource) ? Empty : $" Source: {response.AnswerSource}")}" +
                    $" ({response.AnswerDate:M/d/yyyy})";
              csvWriter.AddField(field);
            }
          }

          if (includeBallotMeasures)
          {
            csvWriter.AddField(row.ReferendumTitle());
            csvWriter.AddField(row.ReferendumDescription());
            csvWriter.AddField(row.ReferendumDetail());
            csvWriter.AddField(row.ReferendumDetailUrl());
            csvWriter.AddField(row.ReferendumFullText());
            csvWriter.AddField(row.ReferendumFullTextUrl());
            csvWriter.AddField(row.IsPassed() ? "Y" : Empty);
          }

          csvWriter.Write(streamWriter);
        }
        streamWriter.Flush();
        ms.Position = 0;
        csv = new StreamReader(ms).ReadToEnd();
      }

      // download
      var filename = electionDescription;
      switch (csvType)
      {
        case "NA":
          filename += " without topics";
          break;
        case "OA":
          filename += " - topics only";
          break;
        case "WA":
          filename += " - all data including topics";
          break;
        case "OK":
          filename += " - names and keys only";
          break;
        case "BM":
          filename += " - Ballot Measures";
          break;
      }
      Response.Clear();
      Response.ContentType = "application/vnd.ms-excel";
      Response.AddHeader("Content-Disposition",
        $"attachment;filename=\"{filename}.csv\"");
      Response.Write("\xfeff"); // BOM
      Response.Write(csv);
      Response.End();
    }
  }
}