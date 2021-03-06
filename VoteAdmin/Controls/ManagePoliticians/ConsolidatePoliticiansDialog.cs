using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using Jayrock.Json.Conversion;
using JetBrains.Annotations;
using static System.String;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel
  {
    #region Private

    [Flags]
    private enum CategoryOptions
    {
      None = 0,
      AsLink = 1,
      Scrollable = 2,
      Paragraphs = 4,
      Headshot = 8,
      Profile = 16
    }

    private class PoliticianUpdateField
    {
      public PoliticianUpdateField(string key, Politicians.Column column)
      {
        Key = key;
        Columns = new[] {column};
      }

      public PoliticianUpdateField(string key, Politicians.Column[] columns)
      {
        Key = key;
        Columns = columns;
      }

      public readonly string Key;
      public readonly Politicians.Column[] Columns;
    }

    private static IEnumerable<PoliticianUpdateField> GetPoliticianUpdateFields() => new[]
    {
      new PoliticianUpdateField("name",
        new[]
        {
          Politicians.Column.FirstName, Politicians.Column.MiddleName,
          Politicians.Column.Nickname, Politicians.Column.LastName,
          Politicians.Column.Suffix, Politicians.Column.AddOn
        }),
      new PoliticianUpdateField("party", Politicians.Column.PartyKey),
      new PoliticianUpdateField("birthday", Politicians.Column.DateOfBirth),
      new PoliticianUpdateField("password", Politicians.Column.Password),
      new PoliticianUpdateField("passwordhint", Politicians.Column.PasswordHint),
      new PoliticianUpdateField("publicaddress",
        new[] {Politicians.Column.Address, Politicians.Column.CityStateZip}),
      new PoliticianUpdateField("publicphone", Politicians.Column.Phone),
      new PoliticianUpdateField("stateaddress",
        new[] {Politicians.Column.StateAddress, Politicians.Column.StateCityStateZip}),
      new PoliticianUpdateField("statephone", Politicians.Column.StatePhone),
      new PoliticianUpdateField("campaign",
        new[]
        {
          Politicians.Column.CampaignName, Politicians.Column.CampaignAddress,
          Politicians.Column.CampaignCityStateZip, Politicians.Column.CampaignPhone,
          Politicians.Column.CampaignEmail
        }),
      new PoliticianUpdateField("statephone", Politicians.Column.StatePhone),
      new PoliticianUpdateField("webaddress", Politicians.Column.WebAddress),
      new PoliticianUpdateField("statewebaddress", Politicians.Column.StateWebAddress),
      new PoliticianUpdateField("email", Politicians.Column.Email),
      new PoliticianUpdateField("stateemail", Politicians.Column.StateEmail),
      new PoliticianUpdateField("voteusaemail", Politicians.Column.EmailVoteUSA),
      new PoliticianUpdateField("ballotpedia", Politicians.Column.BallotPediaWebAddress),
      new PoliticianUpdateField("blogger", Politicians.Column.BloggerWebAddress),
      new PoliticianUpdateField("podcast", Politicians.Column.PodcastWebAddress),
      new PoliticianUpdateField("facebook", Politicians.Column.FacebookWebAddress),
      new PoliticianUpdateField("flickr", Politicians.Column.FlickrWebAddress),
      new PoliticianUpdateField("googleplus", Politicians.Column.GooglePlusWebAddress),
      new PoliticianUpdateField("instagram", Politicians.Column.WebstagramWebAddress),
      new PoliticianUpdateField("linkedin", Politicians.Column.LinkedInWebAddress),
      new PoliticianUpdateField("pinterest", Politicians.Column.PinterestWebAddress),
      new PoliticianUpdateField("rssfeed", Politicians.Column.RSSFeedWebAddress),
      new PoliticianUpdateField("vimeo", Politicians.Column.VimeoWebAddress),
      new PoliticianUpdateField("wikipedia", Politicians.Column.WikipediaWebAddress),
      new PoliticianUpdateField("youtube", Politicians.Column.YouTubeWebAddress)
    };

    private void DoConsolidation()
    {
      try
      {
        var selectedIndex = ConsolidateSelectedIndex.Value;
        string selectedKey;
        string unselectedKey;
        switch (selectedIndex)
        {
          case "1":
            selectedKey = ConsolidateKey1.Value;
            unselectedKey = ConsolidateKey2.Value;
            break;

          case "2":
            selectedKey = ConsolidateKey2.Value;
            unselectedKey = ConsolidateKey1.Value;
            break;

          default: throw new VoteException("Index not 1 or 2");
        }

        // Politicians
        var selectedPolitician = Politicians.GetData(selectedKey);
        var unselectedPolitician = Politicians.GetData(unselectedKey);
        if (selectedPolitician.Count != 1)
          throw new VoteException("Politician " + selectedPolitician + " not found");
        if (unselectedPolitician.Count != 1)
          throw new VoteException("Politician " + unselectedKey + " not found");
        var selectedData = UpdatePoliticians(selectedIndex, selectedPolitician,
          unselectedPolitician);

        // PoliticiansImagesData and PoliticiansImagesBlobs
        var selectedImagesData = PoliticiansImagesData.GetData(selectedKey);
        var unselectedImagesData = PoliticiansImagesData.GetData(unselectedKey);
        var selectedImagesBlobs = PoliticiansImagesBlobs.GetData(selectedKey);
        var unselectedImagesBlobs = PoliticiansImagesBlobs.GetData(unselectedKey);
        UpdateImages(selectedIndex, selectedData, selectedKey, selectedImagesData,
          selectedImagesBlobs, unselectedImagesData, unselectedImagesBlobs);

        // Answers
        //AnswersTable selectedAnswers = null;
        //AnswersTable unselectedAnswers = null;
        var selectedAnswers2 = Answers.GetActiveDataByPoliticianKeyNew(selectedKey);
        var unselectedAnswers2 = Answers2.GetDataByPoliticianKey(unselectedKey);
        UpdateAnswers2(selectedKey, selectedAnswers2, unselectedAnswers2);

        // ElectionsIncumbentsRemoved
        var selectedIncumbentsRemoved =
          ElectionsIncumbentsRemoved.GetDataByPoliticianKey(selectedKey);
        var unselectedIncumbentsRemoved =
          ElectionsIncumbentsRemoved.GetDataByPoliticianKey(unselectedKey);
        UpdateIncumbentsRemoved(selectedKey, unselectedIncumbentsRemoved,
          selectedIncumbentsRemoved);

        // ElectionsPoliticians
        var selectedElectionsPoliticians =
          ElectionsPoliticians.GetDataByPoliticianKey(selectedKey);
        var unselectedElectionsPoliticians =
          ElectionsPoliticians.GetDataByPoliticianKey(unselectedKey);
        UpdateElectionsPoliticians(selectedKey, unselectedElectionsPoliticians,
          selectedElectionsPoliticians);

        // OfficesOfficials
        var selectedOfficesOfficials = OfficesOfficials.GetDataByPoliticianKey(selectedKey);
        var unselectedOfficesOfficials =
          OfficesOfficials.GetDataByPoliticianKey(unselectedKey);
        UpdateOfficesOfficials(selectedKey, unselectedOfficesOfficials,
          selectedOfficesOfficials);

        // Update everything as one transaction, politicians last
        PoliticiansImagesData.UpdateTable(selectedImagesData);
        PoliticiansImagesData.UpdateTable(unselectedImagesData);
        PoliticiansImagesBlobs.UpdateTable(selectedImagesBlobs);
        PoliticiansImagesBlobs.UpdateTable(unselectedImagesBlobs);
        Answers2.UpdateTable(selectedAnswers2);
        Answers2.UpdateTable(unselectedAnswers2);
        ElectionsIncumbentsRemoved.UpdateTable(unselectedIncumbentsRemoved);
        ElectionsPoliticians.UpdateTable(unselectedElectionsPoliticians);
        OfficesOfficials.UpdateTable(unselectedOfficesOfficials);
        Politicians.UpdateTable(selectedPolitician);
        Politicians.UpdateTable(unselectedPolitician);

        // Log
        LogDataChange.LogUpdate("*ConsolidatePoliticians", "*Various", unselectedKey,
          selectedKey, VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow,
          selectedKey);

        // After the main update, refresh the LiveOfficeKey, LiveOfficeStatus and LiveElectionKey
        var view = PoliticiansLiveOfficeKeyView.GetData(selectedKey);
        if (view.Count == 1)
        {
          var keyAndStatus =
            PoliticianOfficeStatus.FromLiveOfficeKeyAndStatus(
              view[0].LiveOfficeKeyAndStatus);
          selectedPolitician[0].LiveOfficeKey = keyAndStatus.OfficeKey;
          selectedPolitician[0].LiveOfficeStatus = keyAndStatus.PoliticianStatus.ToString();
          selectedPolitician[0].LiveElectionKey = keyAndStatus.ElectionKey;
          Politicians.UpdateTable(selectedPolitician);
        }

        ConsolidateReloaded.Value = "ok";
      }
      catch (Exception ex)
      {
        FeedbackConsolidate.AddError("There was an unexpected error: " + ex.Message);
      }
    }

    [NotNull]
    private static string FormatAddress([NotNull] string address, string cityStateZip)
    {
      if (!IsNullOrWhiteSpace(address))
        return address.Trim() + ", " + cityStateZip.Trim();
      return cityStateZip.Trim();
    }

    [NotNull]
    private static string FormatCampaign(PoliticiansRow p)
    {
      var lines = new List<string>();
      var address = FormatAddress(p.CampaignAddress, p.CampaignCityStateZip);
      if (!IsNullOrWhiteSpace(p.CampaignName)) lines.Add("Name: " + p.CampaignName);
      if (!IsNullOrWhiteSpace(address)) lines.Add("Address: " + address);
      if (!IsNullOrWhiteSpace(p.CampaignPhone))
        lines.Add("Phone: " + p.CampaignPhone);
      if (!IsNullOrWhiteSpace(p.CampaignEmail))
        lines.Add("Email: " + p.CampaignEmail);
      return Join("<br />", lines);
    }

    private static void MakeCategory(Control parent, string title, string classNamePrefix,
      string content1, string content2, CategoryOptions options = CategoryOptions.None)
    {
      var asLink = (options & CategoryOptions.AsLink) != 0;
      var isScrollable = (options & CategoryOptions.Scrollable) != 0;
      var createParagraphs = (options & CategoryOptions.Paragraphs) != 0;
      var isHeadshot = (options & CategoryOptions.Headshot) != 0;
      var isProfile = (options & CategoryOptions.Profile) != 0;
      var isPicture = isHeadshot || isProfile;
      var heading = new HtmlDiv().AddTo(parent, "category-heading", true);
      new HtmlSpan {InnerText = title}.AddTo(heading);
      var hasContent1 = isPicture
        ? PoliticiansImagesBlobs.PoliticianKeyExists(content1)
        : !IsNullOrWhiteSpace(content1);
      var hasContent2 = isPicture
        ? PoliticiansImagesBlobs.PoliticianKeyExists(content2)
        : !IsNullOrWhiteSpace(content2);
      var category = new HtmlDiv().AddTo(parent,
        classNamePrefix + "-databoxes databoxes clearfix", true);
      var d1 =
        new HtmlDiv().AddTo(category, "databox databox-1", true) as HtmlContainerControl;
      if (!hasContent1) d1.AddCssClasses("no-content");
      var d2 =
        new HtmlDiv().AddTo(category, "databox databox-2", true) as HtmlContainerControl;
      if (!hasContent2) d2.AddCssClasses("no-content");
      if (isScrollable)
      {
        d1.AddCssClasses("scrollable");
        d2.AddCssClasses("scrollable");
      }
      if (asLink)
      {
        if (!IsNullOrWhiteSpace(content1))
          new HtmlAnchor
          {
            InnerText = content1,
            HRef = VotePage.NormalizeUrl(content1),
            Target = "content"
          }.AddTo(d1);
        if (!IsNullOrWhiteSpace(content2))
          new HtmlAnchor
          {
            InnerText = content2,
            HRef = VotePage.NormalizeUrl(content2),
            Target = "content"
          }.AddTo(d2);
      }
      else if (isPicture)
      {
        d1.AddCssClasses("picture");
        d2.AddCssClasses("picture");
        var width = isHeadshot ? 100 : 300;
        if (hasContent1)
          new HtmlImage {Src = VotePage.GetPoliticianImageUrl(content1, width, true)}
            .AddTo(d1);
        if (hasContent2)
          new HtmlImage {Src = VotePage.GetPoliticianImageUrl(content2, width, true)}
            .AddTo(d2);
      }
      else
      {
        if (createParagraphs)
        {
          content1 = content1.ReplaceNewLinesWithParagraphs();
          content2 = content2.ReplaceNewLinesWithParagraphs();
        }
        Debug.Assert(d1 != null, "d1 != null");
        d1.InnerHtml = content1;
        Debug.Assert(d2 != null, "d2 != null");
        d2.InnerHtml = content2;
      }
    }

    private void LoadAnswersTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidateAnswersTabContent;
      content.Controls.Clear();
      var table = AnswersView.GetDataForConsolidationNew(p1.PoliticianKey, p2.PoliticianKey);
      new HtmlDiv
      {
        InnerText = table.Rows.Count == 0
          ? "No responses for either politician"
          : "All responses are selected by default. Click to deselect."
      }.AddTo(content, "question-title", true);
      foreach (var issue in table.Rows.OfType<DataRow>().GroupBy(r => r.IssueKey()))
      {
        var heading = new HtmlDiv().AddTo(content, "category-heading", true);
        new HtmlSpan {InnerText = issue.First().Issue() }.AddTo(heading);
        foreach (var question in issue.GroupBy(i => i.QuestionKey()))
        {
          new HtmlDiv {InnerText = question.First().Question() }.AddTo(content,
            "question-title", true);
          var answerBoxes = new HtmlDiv().AddTo(content, "answerboxes clearfix", true);
          var answerBox1 = new HtmlDiv().AddTo(answerBoxes, "answerbox answerbox-1", true);
          var answerBox2 = new HtmlDiv().AddTo(answerBoxes, "answerbox answerbox-2", true);
          foreach (var response in question)
          {
            var haveText = !IsNullOrWhiteSpace(response.Answer());
            var haveYouTube = !IsNullOrWhiteSpace(response.YouTubeUrl());
            var answerbox = response.PoliticianKey().IsEqIgnoreCase(p1.PoliticianKey)
              ? answerBox1
              : answerBox2;
            var responseBox =
              new HtmlDiv().AddTo(answerbox, "response-box selected", true) as
                HtmlContainerControl;

            if (haveYouTube)
            {
              var youTube =
                new HtmlDiv().AddTo(responseBox, "response-youtube clearfix", true);
              var anchor = new HtmlAnchor
              {
                HRef = VotePage.NormalizeUrl(response.YouTubeUrl()),
                Target = "YouTube"
              }.AddTo(youTube);
              new HtmlImage {Src = "/images/yt-icon.png"}.AddTo(anchor);
              var description =
                response.YouTubeDescription().ReplaceNewLinesWithParagraphs(false);
              if (!response.YouTubeDateOrNull().IsDefaultDate())
                description += " (" + response.YouTubeDate().ToString("MM/dd/yyyy") +
                  ")";
              new HtmlP {InnerHtml = description}.AddTo(youTube);
            }

            if (haveText && haveYouTube)
              new HtmlHr().AddTo(responseBox, "separator-rule", true);

            if (haveText || !haveYouTube)
            {
              new HtmlDiv {InnerHtml = response.Answer().ReplaceNewLinesWithParagraphs()}
                .AddTo(responseBox, "response-answer", true);

              if (!IsNullOrWhiteSpace(response.Source()) || 
                !response.DateStampOrNull().IsDefaultDate())
              {
                var p = new HtmlP().AddTo(responseBox, "response-source", true);
                if (!IsNullOrWhiteSpace(response.Answer()))
                  new HtmlSpan {InnerHtml = "Source: " + response.Source() }.AddTo(p);
                if (!response.DateStampOrNull().IsDefaultDate())
                  new LiteralControl(" (" +
                    response.DateStamp().ToString("MM/dd/yyyy") + ")").AddTo(p);
              }
            }

            Debug.Assert(responseBox != null, "responseBox != null");
            responseBox.Attributes.Add("data-key",
              response.PoliticianKey() + ":" + response.QuestionKey() + ":" +
              response.Sequence());
          }
        }
      }
    }

    private void LoadContactTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidateContactTabContent;
      content.Controls.Clear();
      MakeCategory(content, "Public Address", "publicaddress",
        FormatAddress(p1.PublicAddress, p1.PublicCityStateZip),
        FormatAddress(p2.PublicAddress, p2.PublicCityStateZip));
      MakeCategory(content, "Public Phone", "publicphone", p1.PublicPhone, p2.PublicPhone);
      MakeCategory(content, "State Address", "stateaddress",
        FormatAddress(p1.StateAddress, p1.StateCityStateZip),
        FormatAddress(p2.StateAddress, p2.StateCityStateZip));
      MakeCategory(content, "State Phone", "statephone", p1.StatePhone, p2.StatePhone);
      MakeCategory(content, "Campaign", "campaign", FormatCampaign(p1), FormatCampaign(p2));
    }

    private void LoadPersonalTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidatePersonalTabContent;
      content.Controls.Clear();
      MakeCategory(content, "Politician Name", "name", Politicians.FormatName(p1, true),
        Politicians.FormatName(p2, true));
      MakeCategory(content, "Party", "party", Parties.GetPartyName(p1.PartyKey),
        Parties.GetPartyName(p2.PartyKey));
      MakeCategory(content, "Date of Birth", "birthday", p1.DateOfBirthAsString,
        p2.DateOfBirthAsString);
      MakeCategory(content, "Password", "password", p1.Password, p2.Password);
      MakeCategory(content, "Password Hint", "passwordhint", p1.PasswordHint,
        p2.PasswordHint);
    }

    private void LoadPicturesTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidatePicturesTabContent;
      content.Controls.Clear();
      MakeCategory(content, "Headshot", "headshot", p1.PoliticianKey, p2.PoliticianKey,
        CategoryOptions.Headshot);
      MakeCategory(content, "Profile", "profile", p1.PoliticianKey, p2.PoliticianKey,
        CategoryOptions.Profile);
    }

    private void LoadSocialTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidateSocialTabContent;
      content.Controls.Clear();
      MakeCategory(content, "BallotPedia", "ballotpedia", p1.BallotPediaWebAddress,
        p2.BallotPediaWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Blogger", "blogger", p1.BloggerWebAddress,
        p2.BloggerWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Facebook", "facebook", p1.FacebookWebAddress,
        p2.FacebookWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Flickr", "flickr", p1.FlickrWebAddress, p2.FlickrWebAddress,
        CategoryOptions.AsLink);
      MakeCategory(content, "GooglePlus", "googleplus", p1.GooglePlusWebAddress,
        p2.GooglePlusWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "InstaGram", "instagram", p1.WebstagramWebAddress,
        p2.WebstagramWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "LinkedIn", "linkedin", p1.LinkedInWebAddress,
        p2.LinkedInWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Pinterest", "pinterest", p1.PinterestWebAddress,
        p2.PinterestWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Podcast", "podcast", p1.PodcastWebAddress,
        p2.PodcastWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "RSS Feed", "rssfeed", p1.RSSFeedWebAddress,
        p2.RSSFeedWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Vimeo", "vimeo", p1.VimeoWebAddress, p2.VimeoWebAddress,
        CategoryOptions.AsLink);
      MakeCategory(content, "Wikipedia", "wikipedia", p1.WikipediaWebAddress,
        p2.WikipediaWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "YouTube", "youtube", p1.YouTubeWebAddress,
        p2.YouTubeWebAddress, CategoryOptions.AsLink);
    }

    private void LoadWebEmailTab(PoliticiansRow p1, PoliticiansRow p2)
    {
      var content = ConsolidateWebEmailTabContent;
      content.Controls.Clear();
      MakeCategory(content, "Web Address", "webaddress", p1.PublicWebAddress,
        p2.PublicWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "State Web Address", "statewebaddress", p1.StateWebAddress,
        p2.StateWebAddress, CategoryOptions.AsLink);
      MakeCategory(content, "Email Address", "email", p1.PublicEmail, p2.PublicEmail);
      MakeCategory(content, "State Email Address", "stateemail", p1.StateEmail,
        p2.StateEmail);
      MakeCategory(content, "Vote-USA Email Address", "voteusaemail", p1.EmailVoteUSA,
        p2.EmailVoteUSA);
    }

    //private void UpdateAnswers(string selectedKey, AnswersTable selectedAnswers,
    //  AnswersTable unselectedAnswers)
    //{
    //  // to update the Answers:
    //  // 1. get both sets of answers
    //  // 2. any answers from the selected politician that aren't in the list are marked for deletion
    //  // 3. any answers from the unselected politician that are in the list are copied to the selected
    //  //    politition, being careful to assign a unique Sequence.
    //  // 4. unselected Answers are deleted
    //  var selectedResponses =
    //    // ReSharper disable once AssignNullToNotNullAttribute
    //    (JsonConvert.Import(ConsolidateSelectedResponses.Value) as IList).Cast<string>()
    //    .ToDictionary(s =>
    //    {
    //      var split = s.Split(':');
    //      return new
    //      {
    //        PoliticianKey = split[0].ToUpperInvariant(),
    //        QuestionKey = split[1].ToUpperInvariant(),
    //        Sequence = int.Parse(split[2])
    //      };
    //    }, s => null as object);

    //  // any answers from the selected politician that aren't in the list are marked for deletion
    //  // we have to defer deletion because data must be available for next step
    //  var rowsToDelete = new List<AnswersRow>();
    //  foreach (var row in Enumerable.Where(selectedAnswers, row => !selectedResponses.ContainsKey(new
    //  {
    //    PoliticianKey = row.PoliticianKey.ToUpperInvariant(),
    //    QuestionKey = row.QuestionKey.ToUpperInvariant(),
    //    row.Sequence
    //  }))) rowsToDelete.Add(row);

    //  foreach (var row in Enumerable.Where(unselectedAnswers, row => selectedResponses.ContainsKey(new
    //  {
    //    PoliticianKey = row.PoliticianKey.ToUpperInvariant(),
    //    QuestionKey = row.QuestionKey.ToUpperInvariant(),
    //    row.Sequence
    //  })))
    //  {
    //    var responses = Enumerable.Where(selectedAnswers, r => r.QuestionKey.IsEqIgnoreCase(row.QuestionKey)).ToList();
    //    var newSequence = responses.Any() ? responses.Max(r => r.Sequence) + 1 : 0;
    //    selectedAnswers.AddRow(selectedKey, row.QuestionKey, newSequence,
    //      Politicians.GetStateCodeFromKey(selectedKey), row.IssueKey, row.Answer,
    //      row.Source, row.DateStamp, row.UserName, row.YouTubeUrl, row.YouTubeDescription,
    //      row.YouTubeRunningTime, row.YouTubeSource, row.YouTubeSourceUrl, row.YouTubeDate,
    //      row.YouTubeRefreshTime, row.YouTubeAutoDisable, row.FacebookVideoUrl,
    //      row.FacebookVideoDescription, row.FacebookVideoRunningTime, row.FacebookVideoDate,
    //      row.FacebookVideoRefreshTime, row.FacebookVideoAutoDisable);
    //  }

    //  foreach (var row in rowsToDelete) row.Delete();

    //  foreach (var row in unselectedAnswers) row.Delete();
    //}

    private void UpdateAnswers2(string selectedKey, Answers2Table selectedAnswers,
      Answers2Table unselectedAnswers)
    {
      // to update the Answers:
      // 1. get both sets of answers
      // 2. any answers from the selected politician that aren't in the list are marked for deletion
      // 3. any answers from the unselected politician that are in the list are copied to the selected
      //    politition, being careful to assign a unique Sequence.
      // 4. unselected Answers are deleted
      var selectedResponses =
        // ReSharper disable once AssignNullToNotNullAttribute
        (JsonConvert.Import(ConsolidateSelectedResponses.Value) as IList).Cast<string>()
        .ToDictionary(s =>
        {
          var split = s.Split(':');
          return new
          {
            PoliticianKey = split[0].ToUpperInvariant(),
            QuestionId = int.Parse(split[1]),
            Sequence = int.Parse(split[2])
          };
        }, s => null as object);

      // any answers from the selected politician that aren't in the list are marked for deletion
      // we have to defer deletion because data must be available for next step
      var rowsToDelete = new List<Answers2Row>();
      foreach (var row in Enumerable.Where(selectedAnswers, row => !selectedResponses.ContainsKey(new
      {
        PoliticianKey = row.PoliticianKey.ToUpperInvariant(),
        row.QuestionId,
        row.Sequence
      }))) rowsToDelete.Add(row);

      foreach (var row in Enumerable.Where(unselectedAnswers, row => selectedResponses.ContainsKey(new
      {
        PoliticianKey = row.PoliticianKey.ToUpperInvariant(),
        row.QuestionId,
        row.Sequence
      })))
      {
        var responses = Enumerable.Where(selectedAnswers, r => r.QuestionId == row.QuestionId).ToList();
        var newSequence = responses.Any() ? responses.Max(r => r.Sequence) + 1 : 0;
        selectedAnswers.AddRow(selectedKey, row.QuestionId, newSequence, row.Answer,
          row.Source, row.DateStamp, row.UserName, row.YouTubeUrl, row.YouTubeDescription,
          row.YouTubeRunningTime, row.YouTubeSource, row.YouTubeSourceUrl, row.YouTubeDate,
          row.YouTubeRefreshTime, row.YouTubeAutoDisable, row.FacebookVideoUrl,
          row.FacebookVideoDescription, row.FacebookVideoRunningTime, row.FacebookVideoDate,
          row.FacebookVideoRefreshTime, row.FacebookVideoAutoDisable);
      }

      foreach (var row in rowsToDelete) row.Delete();

      foreach (var row in unselectedAnswers) row.Delete();
    }

    private static void UpdateElectionsPoliticians(string selectedKey,
      IEnumerable<ElectionsPoliticiansRow> unselectedElectionsPoliticians,
      ElectionsPoliticiansTable selectedElectionsPoliticians)
    {
      // change PoliticianKey in unselected entries unless it would be a duplicate, then delete
      foreach (var row in unselectedElectionsPoliticians)
        if (selectedElectionsPoliticians.Any(
          r => r.ElectionKey.IsEqIgnoreCase(row.ElectionKey) &&
            r.OfficeKey.IsEqIgnoreCase(row.OfficeKey))) row.Delete();
        else row.PoliticianKey = selectedKey;
    }

    private static void UpdateImages([NotNull] string selectedIndex,
      IDictionary selectedData, string selectedKey,
      PoliticiansImagesDataTable selectedImagesData,
      PoliticiansImagesBlobsTable selectedImagesBlobs,
      PoliticiansImagesDataTable unselectedImagesData,
      PoliticiansImagesBlobsTable unselectedImagesBlobs)
    {
      var copyProfile = selectedData["profile"] as string != selectedIndex;
      var copyHeadshot = selectedData["headshot"] as string != selectedIndex;
      var haveUnselectedImages = unselectedImagesData.Count == 1 &&
        unselectedImagesBlobs.Count == 1;
      if (haveUnselectedImages && (copyProfile || copyHeadshot))
      {
        // make sure we have selected rows
        if (selectedImagesData.Count == 0)
        {
          var newRow = selectedImagesData.NewRow();
          newRow.PoliticianKey = selectedKey;
          newRow.ProfileOriginalDate = VotePage.DefaultDbDate;
          newRow.HeadshotDate = VotePage.DefaultDbDate;
          newRow.HeadshotResizeDate = VotePage.DefaultDbDate;
          selectedImagesData.AddRow(newRow);
        }
        if (selectedImagesBlobs.Count == 0)
        {
          var newRow = selectedImagesBlobs.NewRow();
          newRow.PoliticianKey = selectedKey;
          selectedImagesBlobs.AddRow(newRow);
        }
      }
      if (haveUnselectedImages && copyProfile)
      {
        selectedImagesData[0].ProfileOriginalDate =
          unselectedImagesData[0].ProfileOriginalDate;
        selectedImagesBlobs[0].ProfileOriginal = unselectedImagesBlobs[0].ProfileOriginal;
        selectedImagesBlobs[0].Profile200 = unselectedImagesBlobs[0].Profile200;
        selectedImagesBlobs[0].Profile300 = unselectedImagesBlobs[0].Profile300;
      }
      if (haveUnselectedImages && copyHeadshot)
      {
        selectedImagesData[0].HeadshotDate = unselectedImagesData[0].HeadshotDate;
        selectedImagesData[0].HeadshotResizeDate =
          unselectedImagesData[0].HeadshotResizeDate;
        selectedImagesBlobs[0].Headshot15 = unselectedImagesBlobs[0].Headshot15;
        selectedImagesBlobs[0].Headshot20 = unselectedImagesBlobs[0].Headshot20;
        selectedImagesBlobs[0].Headshot25 = unselectedImagesBlobs[0].Headshot25;
        selectedImagesBlobs[0].Headshot35 = unselectedImagesBlobs[0].Headshot35;
        selectedImagesBlobs[0].Headshot50 = unselectedImagesBlobs[0].Headshot50;
        selectedImagesBlobs[0].Headshot75 = unselectedImagesBlobs[0].Headshot75;
        selectedImagesBlobs[0].Headshot100 = unselectedImagesBlobs[0].Headshot100;
      }
      if (unselectedImagesData.Count == 1) unselectedImagesData[0].Delete();
      if (unselectedImagesBlobs.Count == 1) unselectedImagesBlobs[0].Delete();
    }

    private static void UpdateIncumbentsRemoved(string selectedKey,
      IEnumerable<ElectionsIncumbentsRemovedRow> unselectedIncumbentsRemoved,
      ElectionsIncumbentsRemovedTable selectedIncumbentsRemoved)
    {
      // change PoliticianKey in unselected entries unless it would be a duplicate, then delete
      foreach (var row in unselectedIncumbentsRemoved)
        if (selectedIncumbentsRemoved.Any(
          r => r.ElectionKey.IsEqIgnoreCase(row.ElectionKey) &&
            r.OfficeKey.IsEqIgnoreCase(row.OfficeKey))) row.Delete();
        else row.PoliticianKey = selectedKey;
    }

    private static void UpdateOfficesOfficials(string selectedKey,
      IEnumerable<OfficesOfficialsRow> unselectedOfficesOfficials,
      OfficesOfficialsTable selectedOfficesOfficials)
    {
      // change PoliticianKey in unselected entries unless it would be a duplicate, then delete
      foreach (var row in unselectedOfficesOfficials)
        if (selectedOfficesOfficials.Any(r => r.OfficeKey.IsEqIgnoreCase(row.OfficeKey)))
          row.Delete();
        else row.PoliticianKey = selectedKey;
    }

    private IDictionary UpdatePoliticians(string selectedIndex,
      PoliticiansTable selectedPolitician, PoliticiansTable unselectedPolitician)
    {
      // update the Politicians table
      // transfer any values that were chosen from the unselected politician to the selected,
      // then mark the unselected for deletion
      var selectedData = JsonConvert.Import(ConsolidateSelectedData.Value) as IDictionary;
      foreach (var field in GetPoliticianUpdateFields())
      {
        Debug.Assert(selectedData != null, "selectedData != null");
        var index = selectedData[field.Key] as string;
        if (index != selectedIndex)
          foreach (var column in field.Columns)
          {
            var columnName = Politicians.GetColumnName(column);
            selectedPolitician[0][columnName] = unselectedPolitician[0][columnName];
          }
      }
      unselectedPolitician[0].Delete();

      // in case name changed, update the AlphaName and VowelStrippedName columns
      selectedPolitician[0].AlphaName = selectedPolitician[0].LastName.StripAccents();
      selectedPolitician[0].VowelStrippedName =
        selectedPolitician[0].LastName.StripVowels();

      return selectedData;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonConsolidate_OnClick(object sender, EventArgs e)
    {
      switch (ConsolidateReloading.Value)
      {
        case "reloading":
        {
          ConsolidateReloading.Value = Empty;
          ConsolidateReloaded.Value = "reloaded";
          var listData1 = Politicians.GetListItemData(ConsolidateKey1.Value);
          if (listData1 == null)
            throw new VoteException($"Politician key '{ConsolidateKey1.Value}' not found.");
          var listData2 = Politicians.GetListItemData(ConsolidateKey2.Value);
          if (listData2 == null)
            throw new VoteException($"Politician key '{ConsolidateKey2.Value}' not found.");
          Politicians.GetCandidateListItem(listData1).AddTo(ConsolidateItem1);
          Politicians.GetCandidateListItem(listData2).AddTo(ConsolidateItem2);
          //var incumbency1 = Politicians.GetCandidateList_OfficeDescription(listData1);
          //var incumbency2 = Politicians.GetCandidateList_OfficeDescription(listData2);
          var p1 = Politicians.GetData(ConsolidateKey1.Value)[0];
          var p2 = Politicians.GetData(ConsolidateKey2.Value)[0];
          LoadPersonalTab(p1, p2 /*, incumbency1, incumbency2*/);
          LoadContactTab(p1, p2);
          LoadWebEmailTab(p1, p2);
          LoadSocialTab(p1, p2);
          LoadPicturesTab(p1, p2);
          //LoadBioTab(p1, p2);
          LoadAnswersTab(p1, p2);
        }
          break;

        case "":
        {
          // normal update
          ConsolidateReloaded.Value = "error"; // for now
          DoConsolidation();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{ConsolidateReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}