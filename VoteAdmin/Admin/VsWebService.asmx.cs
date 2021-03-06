using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Services;
using DB;
using DB.Vote;
using MySql.Data.MySqlClient;

namespace Vote.Admin
{
  [WebService(Namespace = "http://tempuri.org/")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  [ToolboxItem(false)]
  [ScriptService]
  public class VsWebService : System.Web.Services.WebService
  {
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class CandidateInfo
    {
      public DateTime LastRefreshDate;
      public string FirstName;
      public string MiddleName;
      public string Nickname;
      public string LastName;
      public string Suffix;
      public DateTime BirthDate;
      public string BirthPlace;
      public string Pronunciation;
      public string Gender;
      public string Family;
      public string Photo;
      public string HomeCity;
      public string HomeState;
      public string Education;
      public string Profession;
      public string Political;
      public string Religion;
      public string CongMembership;
      public string OrgMembership;
      public string SpecialMsg;
      public List<SimpleListItem> Items;
      public List<SimpleListItem> WebAddresses;
    }

    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class CandidatesInfo
    {
      public DateTime LastRefreshDate;
      public List<SimpleListItem> Candidates;
    }

    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class ElectionsInfo
    {
      public DateTime LastRefreshDate;
      public List<SimpleListItem> Elections;
    }

    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class VoteUsaCandidate
    {
      public string Text;
      public string Value;
      public string FirstThree;
    }

    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VoteUsaCandidateInfo
    {
      public string PoliticianKey;
      public string Personal;
      public string Education;
      public string Profession;
      public string Military;
      public string Civic;
      public string Political;
      public string Religion;
      public string Accomplishments;
      public DateTime BirthDate;
      public string Email;
      public string Website;
      public string Facebook;
      public string Twitter;
      public string YouTube;
    }

    private static string VsDecode(object field) => 
      // ReSharper disable once AssignNullToNotNullAttribute
      Regex.Replace(field as string, ".", c =>
      {
        switch (c.Value[0])
        {
          case '\u0093':
            return "\u201c"; // ldquo

          case '\u0094':
            return "\u201d"; // rdquo

          case '\u0096':
            return "\u2013"; // ndash

          default:
            return c.Value;
        }
      });

    [WebMethod]
    public CandidateInfo GetCandidate(string vsCandidateId)
    {
      int id;
      VsCandidatesTable candidate = null;
      if (int.TryParse(vsCandidateId, out id))
        candidate = VsCandidates.GetData(id);
      if (candidate == null || candidate.Count == 0) return null;
      var candidateItems = VsCandidatesItems.GetData(id);
      var candidateWebAddresses = VsCandidatesWebAddresses.GetData(id);
      var webAddressTypes = VsWebAddressTypes.GetAllData()
        .ToDictionary(r => r.WebAddressTypeId, r => r.Name);

      var info = new CandidateInfo
      {
        LastRefreshDate =
          candidate[0].LastRefreshDate.AsUtc(),
        FirstName = candidate[0].FirstName,
        MiddleName = candidate[0].MiddleName,
        Nickname = candidate[0].Nickname,
        LastName = candidate[0].LastName,
        Suffix = candidate[0].Suffix,
        BirthDate = candidate[0].BirthDate,
        BirthPlace = candidate[0].BirthPlace,
        Pronunciation = candidate[0].Pronunciation,
        Gender = candidate[0].Gender,
        Family = candidate[0].Family,
        Photo = candidate[0].Photo,
        HomeCity = candidate[0].HomeCity,
        HomeState = candidate[0].HomeState,
        Education = candidate[0].Education,
        Profession = candidate[0].Profession,
        Political = candidate[0].Political,
        Religion = candidate[0].Religion,
        CongMembership = candidate[0].CongMembership,
        OrgMembership = candidate[0].OrgMembership,
        SpecialMsg = candidate[0].SpecialMsg,
        Items = candidateItems.OrderBy(r => r.ItemName)
          .Select(r => new SimpleListItem(r.ItemName, r.ItemData))
          .ToList(),
        WebAddresses = candidateWebAddresses
        .OrderBy(r => r.WebAddressTypeId)
        .Select(r => new SimpleListItem(webAddressTypes.ContainsKey(r.WebAddressTypeId) ?
         webAddressTypes[r.WebAddressTypeId] : "Code " + r.WebAddressTypeId, 
         Validation.StripWebProtocol(r.WebAddress)))
         .ToList()
      };
      return info;
    }

    private static string FormatCandidateName(VsElectionsCandidatesViewRow r)
    {
      if (r.LastName == null) return r.ElectionsLastName + ", " + r.ElectionsFirstName;
      var result = r.LastName;
      if (!string.IsNullOrWhiteSpace(r.Suffix)) result += " " + r.Suffix;
      result += ", " + r.FirstName;
      if (!string.IsNullOrWhiteSpace(r.MiddleName)) result += " " + r.MiddleName;
      if (!string.IsNullOrWhiteSpace(r.Nickname)) result += " (" + r.Nickname + ")";
      return result;
    }

    [WebMethod]
    public CandidatesInfo GetCandidates(string vsElectionKey)
    {
      var electionId =
        int.Parse(vsElectionKey.Substring(0, vsElectionKey.Length - 1));
      var stageId = vsElectionKey.Substring(vsElectionKey.Length - 1, 1);
      var info = new CandidatesInfo
      {
        LastRefreshDate =
          VsElections.GetCandidateListRefreshTime(electionId, stageId,
            VotePage.DefaultDbDate).AsUtc(),
        Candidates = Enumerable.Select(VsElectionsCandidatesView.GetData(electionId, stageId), 
          // ReSharper disable once PossibleInvalidOperationException
          r => new SimpleListItem(r.CandidateId.Value.ToString(CultureInfo.InvariantCulture),
          FormatCandidateName(r)))
          .OrderBy(i => i.Text)
        .ToList()
      };
      return info;
    }

    [WebMethod]
    public ElectionsInfo GetElections(string electionYear, string stateCode)
    {
      var info = new ElectionsInfo
      {
        LastRefreshDate =
          VsElectionYearState.GetLastRefreshTime(electionYear, stateCode,
            VotePage.DefaultDbDate).AsUtc(),
        Elections = Enumerable.Select(VsElections.GetDataByElectionYearStateCode(electionYear,
          stateCode)
          .OrderBy(r => r.StageDate)
          .ThenBy(r => r.StageId), r => new SimpleListItem(r.ElectionId + r.StageId,
            $"{r.ElectionName} - {r.StageName} ({r.StageDate.ToShortDateString()})"))
        .ToList()
      };
      return info;
    }

    [WebMethod]
    public VoteUsaCandidateInfo GetVoteUsaCandidate(string politicianKey)
    {
      return
        Politicians.GetData(politicianKey)
          .Select(row => new VoteUsaCandidateInfo
          {
            PoliticianKey = row.PoliticianKey,
            //Personal = row.Personal,
            //Education = row.Education,
            //Profession = row.Profession,
            //Military = row.Military,
            //Civic = row.Civic,
            //Political = row.Political,
            //Religion = row.Religion,
            //Accomplishments = row.Accomplishments,
            BirthDate = row.DateOfBirth,
            Email = row.PublicEmail,
            Website = row.WebAddress,
            Facebook = row.FacebookWebAddress,
            Twitter = row.TwitterWebAddress,
            YouTube = row.YouTubeWebAddress
          }).FirstOrDefault();
    }

    [WebMethod]
    public List<VoteUsaCandidate> GetVoteUsaCandidates(string electionKey,
      string officeKey)
    {
      return
        ElectionsPoliticians.GetPoliticiansForOfficeInElection(electionKey, officeKey)
          .Rows.Cast<DataRow>()
          .Select(
            row =>
              new VoteUsaCandidate
              {
                Value = row.PoliticianKey(),
                Text = Politicians.FormatName(row),
                FirstThree =
                  row.LastName()
                    .Substring(0, Math.Min(3, row.LastName().Length))
                    .ToLowerInvariant()
              })
          .ToList();
    }

    [WebMethod]
    public List<SimpleListItem> GetVoteUsaOffices(string electionKey)
    {
      return
        ElectionsOffices.GetElectionOffices(electionKey)
          .Rows.Cast<DataRow>()
          .Select(row => new SimpleListItem(row.OfficeKey(), Offices.FormatOfficeName(row)))
          .ToList();
    }

    [WebMethod]
    public string RefreshCandidate(string vsCandidateId)
    {
      var candidateId = int.Parse(vsCandidateId);

      VsCandidates.DeleteByCandidateId(candidateId);
      VsCandidatesItems.DeleteByCandidateId(candidateId);
      VsCandidatesWebAddresses.DeleteByCandidateId(candidateId);

      var jsonObj = VoteSmart.GetJsonAsDictionary("CandidateBio.getBio",
        "candidateId=" + candidateId);
      if (jsonObj.ContainsKey("bio"))
      {
        var bio = jsonObj["bio"] as Dictionary<string, object>;
        if (bio?.ContainsKey("candidate") == true)
        {
          var candidate = bio["candidate"] as Dictionary<string, object>;
          DateTime birthDate;
          Debug.Assert(candidate != null, "candidate != null");
          if (!DateTime.TryParse(candidate["birthDate"] as string, out birthDate)) birthDate = VotePage.DefaultDbDate;
          VsCandidates.Insert(candidateId, DateTime.UtcNow, 
            candidate["firstName"] as string,
            candidate["middleName"] as string,
            candidate["nickName"] as string,
            candidate["lastName"] as string,
            candidate["suffix"] as string,
            birthDate,
            candidate["birthPlace"] as string,
            candidate["pronunciation"] as string,
            candidate["gender"] as string,
            candidate["family"] as string,
            candidate["photo"] as string,
            candidate["homeCity"] as string,
            candidate["homeState"] as string,
            candidate["education"] as string,
            candidate["profession"] as string,
            candidate["political"] as string,
            candidate["religion"] as string,
            candidate["congMembership"] as string,
            candidate["orgMembership"] as string,
            candidate["specialMsg"] as string);
        }
      }

      var itemsJsonObj = VoteSmart.GetJsonAsDictionary("CandidateBio.getAddlBio",
        "candidateId=" + candidateId);
      if (itemsJsonObj.ContainsKey("addlBio"))
      {
        var addlBio = itemsJsonObj["addlBio"] as Dictionary<string, object>;
        if (addlBio?.ContainsKey("additional") == true)
        {
          var additional = addlBio["additional"] as Dictionary<string, object>;
          if (additional != null)
          {
            var item = VoteSmart.AsArrayList(additional["item"]);
            if (item != null)
              foreach (var i in item.Cast<Dictionary<string, object>>())
                VsCandidatesItems.Insert(candidateId, i["name"] as string,
                  i["data"] as string);
          }
        }
      }

      var wasonObj = VoteSmart.GetJsonAsDictionary("Address.getOfficeWebAddress",
        "candidateId=" + candidateId);
      if (wasonObj.ContainsKey("webaddress"))
      {
        var webAddress = wasonObj["webaddress"] as Dictionary<string, object>;
        if (webAddress?.ContainsKey("address") == true)
        {
          var address = VoteSmart.AsArrayList(webAddress["address"]);
          if (address != null)
            foreach (var a in address.Cast<Dictionary<string, object>>())
              VsCandidatesWebAddresses.Insert(candidateId,
                a["webAddressTypeId"] as string, a["webAddress"] as string);
        }
      }

      return string.Empty;
    }

    [WebMethod]
    public string RefreshCandidates(string vsElectionKey)
    {
      var electionId =
        int.Parse(vsElectionKey.Substring(0, vsElectionKey.Length - 1));
      var stageId = vsElectionKey.Substring(vsElectionKey.Length - 1, 1);

      VsElectionsCandidates.DeleteByElectionIdStageId(electionId, stageId);
      VsElections.UpdateCandidateListRefreshTime(DateTime.UtcNow, electionId, stageId);

      var jsonObj = VoteSmart.GetJsonAsDictionary("Candidates.getByElection",
        "electionId=" + electionId + "&stageId=" + stageId);
      if (jsonObj.ContainsKey("candidateList"))
      {
        var candidateList = jsonObj["candidateList"] as Dictionary<string, object>;
        // ReSharper disable once PossibleNullReferenceException
        var candidateArray = VoteSmart.AsArrayList(candidateList["candidate"]);
        foreach (
          var candidate in candidateArray.OfType<Dictionary<string, object>>())
          try
          {
            VsElectionsCandidates.Insert(electionId, stageId,
              Convert.ToInt32(candidate["candidateId"]),
              VsDecode(candidate["ballotName"]), candidate["lastName"] as string,
              candidate["firstName"] as string);
          }
          catch (MySqlException e)
          {
            if (!e.Message.StartsWith("Duplicate entry", 
              StringComparison.OrdinalIgnoreCase)) throw;
          }
      }

      return string.Empty;
    }

    [WebMethod]
    public string RefreshElections(string electionYear, string stateCode)
    {
      var jsonObj = VoteSmart.GetJsonAsDictionary("Election.getElectionByYearState", 
        "year=" + electionYear + "&stateId=" + stateCode);
      if (!jsonObj.ContainsKey("elections"))
        return "No elections found in " + electionYear + " for " +
          StateCache.GetStateName(stateCode);

      VsElections.DeleteByElectionYearStateCode(electionYear, stateCode);

      var elections = jsonObj["elections"] as Dictionary<string, object>;
      Debug.Assert(elections != null, "elections != null");
      var election = VoteSmart.AsArrayList(elections["election"]);
      foreach (var e in election.Cast<Dictionary<string, object>>())
      {
        var electionId = Convert.ToInt32(e["electionId"]);
        var stage = VoteSmart.AsArrayList(e["stage"]);
        foreach (var s in stage.Cast<Dictionary<string, object>>())
        {
          var stageId = s["stageId"] as string;
          VsElectionsCandidates.DeleteByElectionIdStageId(electionId, stageId);
          VsElections.Insert(electionId, stageId, electionYear,
            stateCode, e["officeTypeId"] as string, e["special"] as string, 
            e["name"] as string, s["name"] as string,
            DateTime.Parse(s["electionDate"] as string),
            VotePage.DefaultDbDate);
        }
      }

      if (VsElectionYearState.ElectionYearStateCodeExists(electionYear,stateCode))
        VsElectionYearState.UpdateLastRefreshTime(DateTime.UtcNow, electionYear,
          stateCode);
      else
        VsElectionYearState.Insert(electionYear, stateCode, DateTime.UtcNow);

      return string.Empty;
    }

    [WebMethod(EnableSession = true)]
    public void UpdateVoteUsaCandidate(string politicianKey, string family,
      string education, string professional, string military, string civic, 
      string political, string religion, string accomplishments, 
      string birthdate, string email, string website, string facebook,
      string twitter,string youtube)
    {
      DateTime dateOfBirth;
      if (!DateTime.TryParse(birthdate, out dateOfBirth))
        dateOfBirth = Politicians.GetDateOfBirth(politicianKey,
          VotePage.DefaultDbDate).Date;
      //Politicians.UpdatePersonal(family, politicianKey);
      //Politicians.UpdateEducation(education, politicianKey);
      //Politicians.UpdateProfession(professional, politicianKey);
      //Politicians.UpdateMilitary(military, politicianKey);
      //Politicians.UpdateCivic(civic, politicianKey);
      //Politicians.UpdatePolitical(political, politicianKey);
      //Politicians.UpdateReligion(religion, politicianKey);
      //Politicians.UpdateAccomplishments(accomplishments, politicianKey);
      Politicians.UpdateDateOfBirth(dateOfBirth, politicianKey);
      Politicians.UpdatePublicEmail(Validation.StripWebProtocol(email), politicianKey);
      Politicians.UpdateWebAddress(Validation.StripWebProtocol(website), politicianKey);
      Politicians.UpdateFacebookWebAddress(Validation.StripWebProtocol(facebook), politicianKey);
      Politicians.UpdateTwitterWebAddress(Validation.StripWebProtocol(twitter), politicianKey);
      Politicians.UpdateYouTubeWebAddress(Validation.StripWebProtocol(youtube), politicianKey);
    }

    [WebMethod(EnableSession = true)]
    public string UpdateVoteUsaImage(string politicianKey, string url)
    {
      var request = (HttpWebRequest) WebRequest.Create(url);
      var response = (HttpWebResponse) request.GetResponse();
      using (var inputStream = response.GetResponseStream())
      using (var memoryStream = new MemoryStream())
      {
        Debug.Assert(inputStream != null, "inputStream != null");
        inputStream.CopyTo(memoryStream);
        byte[] originalBlob;
        Size originalSize;
        if (PoliticiansImagesBlobs.GetHeadshot100(politicianKey) == null)
          ImageManager.UpdateAllPoliticianImages(politicianKey,
            memoryStream, DateTime.UtcNow, true, out originalSize,
            out originalBlob);
        else
          ImageManager.UpdatePoliticianProfileImages(politicianKey,
            memoryStream, DateTime.UtcNow, true, out originalSize,
            out originalBlob);
      }
      return VotePage.GetPoliticianImageUrl(politicianKey, 300, true);
    }
  }
}
