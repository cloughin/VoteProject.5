using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB;
using DB.Vote;

namespace Vote.Admin
{
  public partial class DownloadVotersEdgeCsvPage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var electionKey = Request.QueryString["election"];
      if (string.IsNullOrWhiteSpace(electionKey))
        throw new VoteException("Election key is missing.");
      var electionDescription = Elections.GetElectionDesc(electionKey);
      if (string.IsNullOrWhiteSpace(electionDescription))
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
      var table = ElectionsPoliticians.GetVotersEdgeData(electionKey);

      // create the csv
      string csv;
      using (var ms = new MemoryStream())
      {
        var streamWriter = new StreamWriter(ms);
        var csvWriter = new SimpleCsvWriter();

        // write headers
        csvWriter.AddField("Jurisdiction");
        csvWriter.AddField("State Code");
        csvWriter.AddField("Office");
        csvWriter.AddField("District");
        csvWriter.AddField("Running Mate?");
        csvWriter.AddField("Candidate");
        csvWriter.AddField("First Name");
        csvWriter.AddField("Middle Name");
        csvWriter.AddField("Nickname");
        csvWriter.AddField("Last Name");
        csvWriter.AddField("Suffix");
        csvWriter.AddField("Party");
        csvWriter.AddField("Photo Url");
        csvWriter.AddField("VoteUSA Id");
        csvWriter.AddField("Postal Street Address");
        csvWriter.AddField("Postal City, State Zip");
        csvWriter.AddField("Phone");
        csvWriter.AddField("Email");
        csvWriter.AddField("Date of Birth");
        //csvWriter.AddField("General Philosophy");
        //csvWriter.AddField("Personal and Family");
        //csvWriter.AddField("Education");
        //csvWriter.AddField("Profession");
        //csvWriter.AddField("Military");
        //csvWriter.AddField("Civic");
        //csvWriter.AddField("Political Experience");
        //csvWriter.AddField("Religious Affiliation");
        //csvWriter.AddField("Accomplishment and Awards");
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
        csvWriter.AddField("Instagram Url");
        csvWriter.Write(streamWriter);

        foreach (var row in table.Rows.Cast<DataRow>())
        {
          string jurisdiction;
          var politicianKey = row.IsRunningMate()
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
              jurisdiction = string.Empty;
              break;
          }

          var stateCode = Elections.GetStateCodeFromKey(electionKey);

          var qsc = new QueryStringCollection
          {
            {"id", politicianKey}
          };
          var photoUri = UrlManager.GetStateUri(stateCode, "image.aspx", qsc);
          var photoUrl = new UriBuilder(photoUri)
          {
            Host = UrlManager.GetCanonicalLiveHostName(photoUri.Host),
            Port = 80
          }.Uri.ToString();

          var district = string.Empty;
          int distictNumber;
          if (int.TryParse(row.DistrictCode(), out distictNumber))
            district = distictNumber.ToString(CultureInfo.InvariantCulture);

          // convert to simple name if national
          var partyName = Parties.GetNationalPartyDescription(row.PartyCode(),
            row.PartyName());

          csvWriter.AddField(jurisdiction);
          csvWriter.AddField(stateCode);
          csvWriter.AddField(Offices.FormatOfficeName(row));
          csvWriter.AddField(district);
          csvWriter.AddField(row.IsRunningMate() ? row.PoliticianKey() : string.Empty);
          csvWriter.AddField(Politicians.FormatName(row));
          csvWriter.AddField(row.FirstName());
          csvWriter.AddField(row.MiddleName());
          csvWriter.AddField(row.Nickname());
          csvWriter.AddField(row.LastName());
          csvWriter.AddField(row.Suffix());
          csvWriter.AddField(partyName);
          csvWriter.AddField(photoUrl);
          csvWriter.AddField(politicianKey);
          csvWriter.AddField(row.PublicAddress());
          csvWriter.AddField(row.PublicCityStateZip());
          csvWriter.AddField(row.PublicPhone());
          csvWriter.AddField(row.PublicEmail());
          csvWriter.AddField(row.DateOfBirth().ToString("d"));
          //csvWriter.AddField(row.GeneralStatement());
          //csvWriter.AddField(row.Personal());
          //csvWriter.AddField(row.Education());
          //csvWriter.AddField(row.Profession());
          //csvWriter.AddField(row.Military());
          //csvWriter.AddField(row.Civic());
          //csvWriter.AddField(row.Political());
          //csvWriter.AddField(row.Religion());
          //csvWriter.AddField(row.Accomplishments());
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
          csvWriter.AddField(row.WebstagramWebAddress());

          csvWriter.Write(streamWriter);
        }
        streamWriter.Flush();
        ms.Position = 0;
        csv = new StreamReader(ms).ReadToEnd();
      }

      // download
      Response.Clear();
      Response.ContentType = "application/vnd.ms-excel";
      Response.AddHeader("Content-Disposition",
        "attachment;filename=\"" + electionDescription + ".csv\"");
      Response.Write("\xfeff"); // BOM
      Response.Write(csv);
      Response.End();
    }
  }
}