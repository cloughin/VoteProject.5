using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote.Reports;
using Control = System.Web.UI.Control;
using static System.String;

namespace Vote
{
  public partial class BallotPage : CacheablePage
  {
    protected BallotPage()
    {
      //NoIndex = true;
    }

    #region Caching support

    protected override string GetCacheKey()
    {
      return
        $"{UrlManager.GetStateCodeFromHostName()}.{QueryState}.{QueryElection}.{QueryCongress}." +
        $"{QueryStateSenate}.{QueryStateHouse}.{QueryCounty}.{QueryDistrict}.{QueryPlace}." +
        $"{QueryElementary}.{QuerySecondary}.{QueryUnified}.{QueryCityCouncil}.{QueryCountySupervisors}." +
        $"{QuerySchoolDistrictDistrict}";
    }

    protected override string GetCacheType()
    {
      return "Ballot";
    }

    #endregion Caching support

    private readonly string _ElectionKey = QueryElection;
    private string _ElectionDescription;

    private const string TitleTag = "{1} | Interactive Ballot Choices for {0}";

    private void CreateHeading(int officeContestsCount , bool hasBallotMeasures)
    {
      // Link removed per Mantis 840
      //new HtmlAnchor
      //{
      //  HRef = UrlManager.GetElectionPageUri(_ElectionKey).ToString(),
      //  InnerText = _ElectionDescription
      //}.AddTo(ElectionTitle);
      ElectionTitle.InnerText = _ElectionDescription;
      LocationInfo1.InnerHtml = LocationInfo2.InnerHtml = 
        FormatLegislativeDistrictsFromQueryStringForHeading(true);

      var stateCode = Elections.GetStateCodeFromKey(QueryElection);

      if (officeContestsCount > 0 || hasBallotMeasures)
      {
      }
      else
      {
        InstructionsAccordion.Visible = false;
        Instructions.InnerText = "There are no office contests or ballot measures for your legislative districts.";
      }

      if (officeContestsCount > 0 || hasBallotMeasures)
      {
        var additionalInfo = PageCache.Elections.GetElectionAdditionalInfo(QueryElection)
          .ReplaceNewLinesWithParagraphs();
        if (IsNullOrWhiteSpace(additionalInfo))
          additionalInfo =
            ElectionsDefaults.GetElectionAdditionalInfo(
              Elections.GetDefaultElectionKeyFromKey(QueryElection));
        if (!IsNullOrWhiteSpace(additionalInfo))
        {
          new LiteralControl(additionalInfo).AddTo(AdditionalInformation);
        }

        var ballotInstructions = PageCache.Elections.GetBallotInstructions(QueryElection);
        if (IsNullOrWhiteSpace(ballotInstructions))
          ballotInstructions =
            ElectionsDefaults.GetBallotInstructions(
              Elections.GetDefaultElectionKeyFromKey(QueryElection));
        if (!IsNullOrWhiteSpace(ballotInstructions))
        {
          new LiteralControl(ballotInstructions.ReplaceNewLinesWithParagraphs()).AddTo(
            BallotInstructions);
        }

        var statesRow = States.GetData(stateCode).FirstOrDefault();
        if (statesRow != null)
        {
          var controlsToAdd = new List<Control>();

          if (!IsNullOrWhiteSpace(statesRow.Url))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.Url),
              Target = "_blank",
              InnerHtml = $"Official {statesRow.State} Election Website"
            });
            controlsToAdd.Add(new HtmlBreak());
          }

          if (!IsNullOrWhiteSpace(statesRow.PollHours))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlEm { InnerText = "Normal Polling Hours: " });
            controlsToAdd.Add(new LiteralControl(statesRow.PollHours));
          }

          if (!IsNullOrWhiteSpace(statesRow.PollHoursUrl))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.PollHoursUrl),
              Target = "_blank",
              InnerHtml = statesRow.State + " polling hours"
            });
          }

          if (!IsNullOrWhiteSpace(statesRow.PollPlacesUrl))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.PollPlacesUrl),
              Target = "_blank",
              InnerHtml = statesRow.State + " polling places"
            });
          }

          if (!IsNullOrWhiteSpace(statesRow.VoterRegistrationWebAddress))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.VoterRegistrationWebAddress),
              Target = "_blank",
              InnerHtml = statesRow.State + " voter registration page"
            });
          }

          if (!IsNullOrWhiteSpace(statesRow.EarlyVotingWebAddress))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.EarlyVotingWebAddress),
              Target = "_blank",
              InnerHtml = statesRow.State + " early voting page"
            });
          }

          if (!IsNullOrWhiteSpace(statesRow.VoteByMailWebAddress))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.VoteByMailWebAddress),
              Target = "_blank",
              InnerHtml = statesRow.State + " vote by mail page"
            });
          }

          if (!IsNullOrWhiteSpace(statesRow.VoteByAbsenteeBallotWebAddress))
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlAnchor
            {
              HRef = NormalizeUrl(statesRow.VoteByAbsenteeBallotWebAddress),
              Target = "_blank",
              InnerHtml = statesRow.State + " absentee ballot page"
            });
          }

          if (controlsToAdd.Count > 0)
          {
            var p = new HtmlP();
            VotingInformation.Controls.Add(p);
            foreach (var c in controlsToAdd)
              p.Controls.Add(c);
          }

          var howVotingIsDone = Elections.IsPrimaryElection(QueryElection)
            ? statesRow.HowPrimariesAreDone
            : statesRow.HowVotingIsDone;

          if (!IsNullOrWhiteSpace(howVotingIsDone))
            VotingInformation.Controls.Add(
              new LiteralControl(howVotingIsDone.ReplaceNewLinesWithParagraphs()));
        }

        var stateElectionKey = Elections.GetStateElectionKeyFromKey(QueryElection);
        var electionsRow = Elections.GetData(stateElectionKey).FirstOrDefault();
        var defRow = ElectionsDefaults
          .GetData(Elections.GetDefaultElectionKeyFromKey(stateElectionKey))
          .FirstOrDefault();
        if (electionsRow != null && defRow != null)
        {
          var controlsToAdd = new List<Control>();

          var registrationDeadline = electionsRow.RegistrationDeadline.IsDefaultDate()
            ? defRow.RegistrationDeadline
            : electionsRow.RegistrationDeadline;

          var earlyVotingBegin = electionsRow.EarlyVotingBegin.IsDefaultDate()
            ? defRow.EarlyVotingBegin
            : electionsRow.EarlyVotingBegin;

          var earlyVotingEnd = electionsRow.EarlyVotingEnd.IsDefaultDate()
            ? defRow.EarlyVotingEnd
            : electionsRow.EarlyVotingEnd;

          var mailBallotBegin = electionsRow.MailBallotBegin.IsDefaultDate()
            ? defRow.MailBallotBegin
            : electionsRow.MailBallotBegin;

          var mailBallotEnd = electionsRow.MailBallotEnd.IsDefaultDate()
            ? defRow.MailBallotEnd
            : electionsRow.MailBallotEnd;

          var mailBallotDeadline = electionsRow.MailBallotDeadline.IsDefaultDate()
            ? defRow.MailBallotDeadline
            : electionsRow.MailBallotDeadline;

          var absenteeBallotBegin = electionsRow.AbsenteeBallotBegin.IsDefaultDate()
            ? defRow.AbsenteeBallotBegin
            : electionsRow.AbsenteeBallotBegin;

          var absenteeBallotEnd = electionsRow.AbsenteeBallotEnd.IsDefaultDate()
            ? defRow.AbsenteeBallotEnd
            : electionsRow.AbsenteeBallotEnd;

          var absenteeBallotDeadline = electionsRow.AbsenteeBallotDeadline.IsDefaultDate()
            ? defRow.AbsenteeBallotDeadline
            : electionsRow.AbsenteeBallotDeadline;

          if (!registrationDeadline.IsDefaultDate())
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlEm {InnerText = "Registration Deadline: "});
            controlsToAdd.Add(new LiteralControl(registrationDeadline.ToShortDateString()));
          }

          if (!earlyVotingBegin.IsDefaultDate() || !earlyVotingEnd.IsDefaultDate())
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlEm {InnerText = "Early Voting: "});
            var strings = new List<string>();
            if (!earlyVotingBegin.IsDefaultDate())
              strings.Add($"begins {earlyVotingBegin.ToShortDateString()}");
            if (!earlyVotingEnd.IsDefaultDate())
              strings.Add($"ends {earlyVotingEnd.ToShortDateString()}");
            controlsToAdd.Add(new LiteralControl(Join(", ", strings)));
          }

          if (!mailBallotBegin.IsDefaultDate() || !mailBallotEnd.IsDefaultDate() ||
            !mailBallotDeadline.IsDefaultDate())
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlEm {InnerText = "Mail-In Ballots: "});
            var strings = new List<string>();
            if (!mailBallotBegin.IsDefaultDate())
              strings.Add($"begins {mailBallotBegin.ToShortDateString()}");
            if (!mailBallotEnd.IsDefaultDate())
              strings.Add($"last day to request {mailBallotEnd.ToShortDateString()}");
            if (!mailBallotDeadline.IsDefaultDate())
              strings.Add($"must be received by {mailBallotDeadline.ToShortDateString()}");
            controlsToAdd.Add(new LiteralControl(Join(", ", strings)));
          }

          if (!absenteeBallotBegin.IsDefaultDate() || !absenteeBallotEnd.IsDefaultDate() ||
            !absenteeBallotDeadline.IsDefaultDate())
          {
            if (controlsToAdd.Count > 0) controlsToAdd.Add(new HtmlBreak());
            controlsToAdd.Add(new HtmlEm {InnerText = "Absentee Ballots: "});
            var strings = new List<string>();
            if (!absenteeBallotBegin.IsDefaultDate())
              strings.Add($"begins {absenteeBallotBegin.ToShortDateString()}");
            if (!absenteeBallotEnd.IsDefaultDate())
              strings.Add($"last day to request {absenteeBallotEnd.ToShortDateString()}");
            if (!absenteeBallotDeadline.IsDefaultDate())
              strings.Add(
                $"must be received by {absenteeBallotDeadline.ToShortDateString()}");
            controlsToAdd.Add(new LiteralControl(Join(", ", strings)));
          }

          if (controlsToAdd.Count > 0)
          {
            var p = new HtmlP();
            VotingInformation.Controls.Add(p);
            foreach (var c in controlsToAdd)
              p.Controls.Add(c);
          }
        }

        // Mantis 349: Show Incumbent always
        //if (
        //  StateCache.GetIsIncumbentShownOnBallots(
        //    Elections.GetStateCodeFromKey(QueryElection)))
        var span = new HtmlSpan().AddTo(AdditionalInfo, "incumbent-note");
        new LiteralControl("* before a candidate&rsquo;s name denotes incumbent").AddTo(span);
      }
    }

    private string GetMetaTitle()
    {
      return
        $"Interactive Ballot Choices for {PageCache.Elections.GetElectionDesc(QueryElection)}," +
        $" {FormatLegislativeDistrictsFromQueryString(", ")}";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("~/js/ballot.js");

      _ElectionDescription = PageCache.Elections.GetElectionDesc(_ElectionKey);
      Title = Format(TitleTag, GetMetaTitle(), PublicMasterPage.SiteName);

      OgUrl.Attributes.Add("content", CanonicalUri.ToString());
      OgTitle.Attributes.Add("content", $"My choices for {_ElectionDescription}");
      OgDescription.Attributes.Add("content", "View the candidates I plan to vote for.");

      OgImage.Attributes.Add("content",
        UrlManager.GetSiteUri("/images/designs/Vote-USA/fbbanner2.png").ToString());
      OgImageWidth.Attributes.Add("content", "250");
      OgImageHeight.Attributes.Add("content", "250");

      try
      {
        var metaContent = GetMetaTitle();
        MetaDescription = metaContent;
        MetaKeywords = metaContent;

        BallotReportResponsive.GetReport(QueryElection, QueryCongress, QueryStateSenate,
          QueryStateHouse, QueryCounty, QueryDistrict, QueryPlace, QueryElementary,
          QuerySecondary, QueryUnified, QueryCityCouncil, QueryCountySupervisors,
          QuerySchoolDistrictDistrict, out var officeContests).AddTo(ReportPlaceHolder);
        var ballotMeasures = BallotReferendumReportResponsive.GetReport(QueryElection, QueryCounty,
            QueryDistrict, QueryPlace, QueryElementary, QuerySecondary, QueryUnified,
            QueryCityCouncil, QueryCountySupervisors, QuerySchoolDistrictDistrict)
          .AddTo(ReferendumReportPlaceHolder);
        ballotMeasures.AddTo(ReferendumReportPlaceHolder);
        CreateHeading(officeContests.Count, ballotMeasures.Controls.Count > 0);

        // tag body with key for persistent selections
        var body = Master.FindControl("Body") as HtmlGenericControl;
        var stateElectionKey = Elections.GetStateElectionKeyFromKey(QueryElection)
          .ToUpperInvariant();
        // ReSharper disable once PossibleNullReferenceException
        body.Attributes.Add("data-election", stateElectionKey);
        var electionDate = DateTime.ParseExact(Elections.GetElectionDateStringFromKey(stateElectionKey), "yyyyMMdd",
          CultureInfo.InvariantCulture);
        //var showAd = electionDate.AddDays(1) > DateTime.Today;
        var showAd = true;
        body.Attributes.Add("data-ad", showAd ? "Y" : "N");

        // for a single contest election we add a special class and also include the candidate comparison info
        if (officeContests.Count == 1 && ballotMeasures.Controls.Count == 0)
        {
          body.AddCssClasses("single-contest");
          body.Attributes.Add("data-adelection", QueryElection);
          body.Attributes.Add("data-adoffice", officeContests[0]);
          CompareCandidatesReportResponsive.GetReport(_ElectionKey, officeContests[0], true)
            .AddTo(ReportPlaceHolder);
        }
      }
      catch (Exception /*ex*/)
      {
        // ignored
      }
    }
  }
}