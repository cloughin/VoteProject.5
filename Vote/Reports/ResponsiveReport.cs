using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal class ResponsiveReport : Report
  {
    #region Private

    private static void CreateCandidateCell(Control container, DataRow politician, bool includeKey = false,
      bool includeCheck = false, bool nameAsUpdateLink = false)
    {
      if (politician != null)
        ReportPolitician(container, politician, politician.IsWinner(), politician.IsIncumbent(), null, 
          includeKey, includeCheck, nameAsUpdateLink);
      else // create empty cell
      {
        var cell = new HtmlDiv().AddTo(container, "candidate-cell empty-candidate-cell");
        new HtmlDiv().AddTo(cell, "candidate-cell-inner");
      }
    }

    #endregion

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected readonly Control ReportContainer =
      new HtmlDiv().AddCssClasses("responsive-report");

    protected abstract class RunningMateDataManager : ReportDataManager<DataRow>
    {
      public DataRow GetRunningMate(string officeKey, string politicianKey)
      {
        return DataTable.Rows.OfType<DataRow>()
          .FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
              .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }
    }

    protected static void CreateCompareOrIntroAnchor(Control container, IList<DataRow> politicians,
      string electionKey, string officeKey)
    {
      if (politicians.Count > 1)
        CreateCompareTheCandidatesAnchor(electionKey, officeKey)
          .AddTo(container);
      else
      {
        var div = new HtmlDiv().AddTo(container, "candidate-views no-print");
        CreatePoliticianIntroAnchor(politicians[0], "Candidate Bio & Views")
          .AddTo(div, "link-button");
      }
    }

    private static HtmlAnchor CreateReferendumPageAnchor(string electionKey,
      string referendumKey, string anchorText)
    {
      var a = new HtmlAnchor
      {
        HRef = UrlManager.GetReferendumPageUri(electionKey, referendumKey)
          .ToString(),
        Title = "Referendum Description, Details and Full Text",
        InnerHtml = anchorText
      };
      return a;
    }

    protected static void FormatAge(Control infoContainer, DataRow politician)
    {
      var age = politician.Age();
      if (!string.IsNullOrEmpty(age))
        new HtmlDiv { InnerText = age }.AddTo(infoContainer,
          "candidate-age");
    }

    protected static void FormatCandidateNameAndParty(Control container, DataRow politician,
      bool isIncumbent = false, bool showParty = true, bool isRunningMate = false, 
      bool nameAsUpdateLink = false, string spanClass = null)
    {
      if (isIncumbent)
        container.AddCssClasses("incumbent");
      var politicianName = Politicians.FormatName(politician, true);
      var span = new HtmlSpan();
      span.AddTo(container, spanClass);
      if (nameAsUpdateLink)
        CreateAdminPoliticianAnchor(politician, politicianName).AddTo(span);
      else
        span.InnerText = politicianName;
      if (isRunningMate) span.AddCssClasses("running-mate");
      if (showParty && !string.IsNullOrWhiteSpace(politician.PartyCode()))
      {
        new Literal { Text = " - " }.AddTo(span);
        CreatePartyAnchor(politician, "view").AddTo(span);
      }
    }

    protected static void FormatMoreInfoLink(Control infoContainer, DataRow politician)
    {
      var moreInfoDiv = new HtmlDiv().AddTo(infoContainer, "candidate-more-info no-print");
      CreatePoliticianIntroAnchor(politician, "More information").AddTo(moreInfoDiv);
    }

    protected static void FormatPhone(Control infoContainer, DataRow politician)
    {
      var phone = politician.PublicPhone();

      if (!string.IsNullOrEmpty(phone))
        new HtmlDiv { InnerText = phone.NormalizePhoneNumber() }.AddTo(infoContainer,
          "candidate-phone");
    }

    protected static void FormatPostalAddress(Control infoContainer, DataRow politician)
    {
      var addressLines = new List<string>();
      var streetAddress = politician.PublicAddress();
      var cityStateZip = politician.PublicCityStateZip();
      if (!string.IsNullOrEmpty(streetAddress) &&
          !string.IsNullOrEmpty(cityStateZip))
      {
        addressLines.Add(streetAddress);
        addressLines.Add(cityStateZip);
      }
      else addressLines.Add("no address");

      var addressContainer = new HtmlDiv().AddTo(infoContainer,
        "candidate-address");

      foreach (var line in addressLines)
        new HtmlSpan { InnerText = line }.AddTo(addressContainer);
    }

    protected static void FormatSocialMedia(Control container, DataRow politician)
    {
      var socalMediaAnchors = SocialMedia.GetAnchors(politician, false, true);
      if (socalMediaAnchors.Controls.Count > 0)
        socalMediaAnchors.AddTo(container, "candidate-social-media clearfixleft");
    }

    protected static void FormatWebAddress(Control container, DataRow politician)
    {
      var webAddress = politician.PublicWebAddress();
      var website = FormatPoliticianWebsite(politician/*, webAddress*/);
      var isEmptyWebsite = website as Literal;
      if (string.IsNullOrWhiteSpace(isEmptyWebsite?.Text))
      {
        var websiteContainer = new HtmlDiv().AddTo(container,
          "candidate-website");
        var wrapper = new HtmlSpan().AddTo(websiteContainer, "no-print");
        website.AddTo(wrapper);
        new HtmlSpan { InnerText = webAddress }
          .AddTo(websiteContainer, "only-print");
      }
    }

    protected static int GetElectionPositions(string electionKey, DataRow officeInfo)
    {
      var positions = Elections.IsPrimaryElection(electionKey)
        ? (Elections.IsRunoffElection(electionKey)
          ? officeInfo.PrimaryRunoffPositions()
          : officeInfo.PrimaryPositions())
        : (Elections.IsRunoffElection(electionKey)
          ? officeInfo.GeneralRunoffPositions()
          : officeInfo.ElectionPositions());
      return positions;
    }

    protected static void ReportOffice(Control container, bool isRunningMateOffice,
      IList<DataRow> politicians, RunningMateDataManager dataManager, bool includePoliticianKey = false,
      bool includeCheck = false, bool nameAsUpdateLink = false)
    {
      var officeInfo = politicians[0];
      container.AddCssClasses("candidates-" + politicians.Count);

      foreach (
        var row in politicians.Where(p => p.ElectionsPoliticianKey() != null))
      {
        var ticketContainer = isRunningMateOffice
          ? new HtmlDiv().AddTo(container, "office-ticket")
          : container;

        CreateCandidateCell(ticketContainer, row, includePoliticianKey, includeCheck, nameAsUpdateLink);

        if (!isRunningMateOffice) continue;

        var runningMate = dataManager.GetRunningMate(officeInfo.OfficeKey(),
          row.RunningMateKey());

        CreateCandidateCell(ticketContainer, runningMate, false, false, nameAsUpdateLink);
      }
    }

    protected void ReportOneReferendum(Control container, DataRow row, bool forBallotPage = false)
    {
      var formattedTitle = row.ReferendumTitle().ReplaceNewLinesWithSpans();
      new HtmlDiv { InnerHtml = formattedTitle }.AddTo(container, "referendum-title accordion-header");

      var contentDiv = new HtmlDiv().AddTo(container, "referendum-content accordion-content");
      contentDiv.Attributes.Add("data-key", row.ReferendumKey().ToUpperInvariant());

      if (forBallotPage)
      {
        var yesNo = new HtmlDiv().AddTo(contentDiv, "yes-no");
        var yes = new HtmlSpan().AddTo(yesNo, "yes");
        new HtmlInputCheckBox().AddTo(yes, "kalypto referendum-checkbox");
        new HtmlSpan { InnerText = "Yes" }.AddTo(yes, "label clicker");
        var no = new HtmlSpan().AddTo(yesNo, "no");
        new HtmlInputCheckBox().AddTo(no, "kalypto referendum-checkbox");
        new HtmlSpan { InnerText = "No" }.AddTo(no, "label clicker");
        contentDiv = new HtmlDiv().AddTo(contentDiv, "referendum-right-content");
      }

      if (!forBallotPage && row.IsResultRecorded())
        new HtmlDiv().AddTo(contentDiv, row.IsPassed() ? "referendum-passed" : "referendum-failed");

      var formattedDescription = row.ReferendumDescription().ReplaceNewLinesWithSpans();
      var referendumDetailDesc = row.ReferendumDescription() != string.Empty
        ? formattedDescription
        : formattedTitle;

      new HtmlDiv { InnerHtml = referendumDetailDesc }.AddTo(contentDiv, "referendum-description");

      var anchorDiv = new HtmlDiv().AddTo(contentDiv, "referendum-details no-print");

      if (ReportUser == ReportUser.Master)
        CreateAdminReferendumAnchor(row.ElectionKey(), row.ReferendumKey(),
          "Edit description, details and full text")
          .AddTo(anchorDiv);
      else
        CreateReferendumPageAnchor(row.ElectionKey(), row.ReferendumKey(), 
          "Description, details and full text")
          .AddTo(anchorDiv);
      }

    protected static void ReportPolitician(Control container, DataRow politician, bool isWinner,
      bool isIncumbent, string heading = null, bool includeKey = false, bool includeCheck = false,
      bool nameAsUpdateLink = false)
    {
      var politicianKey = politician.PoliticianKey();
      var content = new HtmlDiv().AddTo(container, "candidate-cell");
      if (politician.IsRunningMate()) content.AddCssClasses("running-mate-cell");
      if (includeKey) content.Attributes.Add("data-key", politicianKey.ToUpperInvariant());
      content = new HtmlDiv().AddTo(content, "candidate-cell-inner");

      if (!string.IsNullOrWhiteSpace(heading))
        new HtmlDiv { InnerText = heading }.AddTo(content, "cell-heading");

      var nameContainer = new HtmlDiv().AddTo(content,
        "candidate-name");
      includeCheck = includeCheck && !politician.IsRunningMate();
      if (includeCheck)
        new HtmlInputCheckBox().AddTo(nameContainer, "kalypto candidate-checkbox");

      FormatCandidateNameAndParty(nameContainer, politician, isIncumbent, true, false, nameAsUpdateLink,
        includeCheck ? "clicker" : null);

      var imageContainer = new HtmlDiv().AddTo(content, "candidate-image");
      if (nameAsUpdateLink)
        CreatePoliticianImageAnchor(SecurePoliticianPage.GetUpdateIntroPageUrl(politicianKey),
          politicianKey, ImageSize100, "Update candidate's intro", "intro").AddTo(imageContainer);
      else
        CreatePoliticianImageTag(politicianKey, ImageSize100, false, string.Empty).AddTo(imageContainer);

      var infoContainer = new HtmlDiv().AddTo(content, "candidate-info");

      if (isWinner)
        new HtmlDiv().AddTo(infoContainer, "candidate-is-winner");

      FormatWebAddress(infoContainer, politician);
      FormatSocialMedia(infoContainer, politician);
      //FormatPostalAddress(infoContainer, politician);
      //FormatPhone(infoContainer, politician);
      FormatAge(infoContainer, politician);
      FormatMoreInfoLink(infoContainer, politician);
    }

    protected sealed class OfficialsSort : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.OfficeLevel()
          .CompareTo(row2.OfficeLevel());
        if (result != 0) return result;
        result = string.Compare(row1.DistrictCode(), row2.DistrictCode(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeLine1(), row2.OfficeLine1(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = string.Compare(row1.AddOn(), row2.AddOn(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.LastName(), row2.LastName(),
          StringComparison.OrdinalIgnoreCase);
      }
    }

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }
}