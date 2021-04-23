using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.api
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [System.ComponentModel.ToolboxItem(false)]
  [System.Web.Script.Services.ScriptService]

  public class ApiV1 : System.Web.Services.WebService
  {
    public class ElectionInfo
    {
      public string electionDay;
      public string name;
      public string id;
    }

    public class Candidate
    {
      public string id;
      public string name;
    }

    public class Contest
    {
      public string office;
      public string id;
      public List<Candidate> candidates;
    }

    public class VoterBallotIdentifiersRetrieveData
    {
      public string kind;
      public double latitude;
      public double longitude;
      public string electionDay;
      public string state;
      public ElectionInfo election;
      public List<ElectionInfo> otherElections;
      public List<Contest> contests;
      public string status;
      public bool success;
    }

    [WebMethod]
    public void voterInfoQuery(double latitude, double longitude,
      string electionDay, string state, string accessKey)
    {
      var result = new VoterBallotIdentifiersRetrieveData
      {
        status = "OK", success = true, latitude = latitude, longitude = longitude, kind = "voteusa#voterInfoQuery"
      };
      state = state.ToUpperInvariant();
      if (StateCache.IsValidStateCode(state))
        result.state = state;
      else
      {
        result.status = "Invalid state";
        result.success = false;
      }

      if (DateTime.TryParseExact(electionDay, "yyyy-MM-dd", new CultureInfo("en-US"),
        DateTimeStyles.None, out var electionDate))
        result.electionDay = electionDay;
      else
      {
        result.status = "Invalid electionDay";
        result.success = false;
      }

      if (accessKey != "M@gicWord")
      {
        result.status = "Unauthorized";
        result.success = false;
      }

      // There could be multiple election keys if it's a primary day
      var keys = Elections.GetStateElectionKeysForDate(state, electionDate);
      if (keys.Count < 1)
      {
        result.status = $"No elections found for {electionDay} in {state}";
        result.success = false;
      }

      if (result.success) // all validation passed
      {
        var tiger = TigerLookup.LookupAll(latitude, longitude);
        // handle the keys -- first one is arbitrarily the main election
        result.election = new ElectionInfo
        {
          electionDay = electionDay,
          name = Elections.GetElectionDesc(keys[0]),
          id = keys[0]
        };
        // if there are additional elections, post them
        result.otherElections = new List<ElectionInfo>();
        foreach (var key in keys.Skip(1))
          result.otherElections.Add(new ElectionInfo
          {
            electionDay = electionDay,
            name = Elections.GetElectionDesc(key),
            id = key
          });
        result.contests = new List<Contest>();
        foreach (var key in keys)
        {
          var data = ElectionsPoliticians.GetSampleBallotData(key, tiger.Congress,
            tiger.Upper, tiger.Lower, tiger.County, tiger.District, tiger.Place,
            tiger.Elementary, tiger.Secondary, tiger.Unified, tiger.CityCouncil,
            tiger.CountySupervisors, Empty);
          var contests = data.Rows.OfType<DataRow>().GroupBy(r =>
            new {ElectionKey = r.ElectionKey(), OfficeKey = r.OfficeKey()}).ToList();
          foreach (var contest in contests)
          {
            var candidates = new List<Candidate>();
            foreach (var candidate in contest)
              candidates.Add(new Candidate
              {
                id = candidate.PoliticianKey(),
                name = Politicians.FormatName(candidate)
              });
            result.contests.Add(new Contest
            {
              office = Offices.FormatOfficeName(contest.First()),
              id = $"{contest.Key.ElectionKey}|{contest.Key.OfficeKey}",
              candidates = candidates
            });
          }
        }
      }

      var javaScriptSerializer = new JavaScriptSerializer();
      var jsonString = javaScriptSerializer.Serialize(result);
      Context.Response.Write(jsonString);
    }
  }
}
