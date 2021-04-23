using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public abstract class PartiesReport : TableBasedReport
  {
    #region Private

    [Flags]
    private enum Options
    {
      None = 0,
      Strong = 1,
      Em = 2,
      NoBreak = 4
    }

    private class IssueQuestion
    {
      public string Column;
      public string Description;
    }

    private readonly IssueQuestion[] _IssueQuestions =
    {
      new IssueQuestion {Column = "BioGen", Description = "Bio: General"},
      new IssueQuestion {Column = "BioPer", Description = "Bio: Personal"},
      new IssueQuestion {Column = "BioPro", Description = "Bio: Professional"},
      new IssueQuestion {Column = "BioCiv", Description = "Bio: Civic"},
      new IssueQuestion {Column = "BioPol", Description = "Bio: Political"},
      new IssueQuestion {Column = "BioRel", Description = "Bio: Religion"},
      new IssueQuestion {Column = "BioAcc", Description = "Bio: Accomplishments"},
      new IssueQuestion {Column = "BioEdu", Description = "Bio: Education"},
      new IssueQuestion {Column = "BioMil", Description = "Bio: Military"},
      new IssueQuestion {Column = "PerWhy", Description = "R&O: Why Running"},
      new IssueQuestion {Column = "PerGls", Description = "R&O: Goals"},
      new IssueQuestion {Column = "PerAch", Description = "R&O: Achievements"},
      new IssueQuestion {Column = "PerCon", Description = "R&O: Areas of Focus"},
      new IssueQuestion {Column = "PerPub", Description = "R&O: Public Service"},
      new IssueQuestion {Column = "PerOpi", Description = "R&O: Other Candidates"}
    };

    private static void AddItem(Control cell, string text, Options options = Options.None)
    {
      Control control = new Literal {Text = text};
      if ((options & Options.NoBreak) != 0)
      {
        var temp = control;
        (control = new HtmlSpan()).Controls.Add(temp);
      }
      if ((options & Options.Em) != 0)
      {
        var temp = control;
        (control = new HtmlEm()).Controls.Add(temp);
      }
      if ((options & Options.Strong) != 0)
      {
        var temp = control;
        (control = new HtmlStrong()).Controls.Add(temp);
      }
      control.AddTo(cell);
    }

    private void AddIssueItems(Control cell, DataRow row)
    {
      foreach (var q in _IssueQuestions)
      {
        if (Convert.ToInt32(row[q.Column]) == 0)
          AddItem(cell, "No " + q.Description, Options.Strong | Options.Em | Options.NoBreak);
        else
          AddItem(cell, "Have " + q.Description, Options.NoBreak);
        new HtmlBreak().AddTo(cell);
      }
    }

    #endregion Private

    #region Protected

    protected void CreateHeadingRow()
    {
      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading");
      new HtmlTableCell
      {
        InnerHtml = "Picture",
        RowSpan = 2
      }.AddTo(tr, "tdReportGroupHeading");
      new HtmlTableCell
      {
        InnerHtml = "Info We Have or Do Not Have",
        ColSpan = 2
      }.AddTo(tr, "tdReportGroupHeading");
      new HtmlTableCell
      {
        InnerHtml = "Links to Enter Missing Info or View Existing Info",
        RowSpan = 2
      }.AddTo(tr, "tdReportGroupHeading");

      tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportGroupHeading");
      new HtmlTableCell
      {
        InnerHtml = "General"
      }.AddTo(tr, "tdReportGroupHeading");
      new HtmlTableCell
      {
        InnerHtml = "Bio Info and Reasons &amp; Objectives (R&amp;O)"
      }.AddTo(tr, "tdReportGroupHeading");
    }

    protected void FormatPoliticianRow(DataRow row)
    {
      var politicianKey = row.PoliticianKey();

      var tr = new HtmlTableRow().AddTo(CurrentHtmlTable, "trReportDetail");

      CreatePoliticianImageTag(politicianKey, ImageSize100).AddTo(
        new HtmlTableCell().AddTo(tr, "tdReportDetailLargeBold"));

      // Information We Have
      //var infoLines = new List<string>();

      var tdInfo = new HtmlTableCell().AddTo(tr, "tdReportDetailSmall");

      // Age, Phone, Address, Social Media
      var politicianAge = row.Age();
      if (IsNullOrWhiteSpace(politicianAge))
      {
        AddItem(tdInfo, "No DOB to compute age", Options.Strong | Options.Em);
        new HtmlBreak().AddTo(tdInfo);
      }
      else
        AddItem(tdInfo, "Age " + politicianAge);

      new HtmlBreak().AddTo(tdInfo);
      var politicianPhone = row.PublicPhone();
      if (IsNullOrWhiteSpace(politicianPhone))
        AddItem(tdInfo, "No Phone", Options.Strong);
      else
        AddItem(tdInfo, politicianPhone.NormalizePhoneNumber(), Options.NoBreak);

      new HtmlBreak().AddTo(tdInfo);
      var politicianAddress = row.PublicAddress();
      if (IsNullOrWhiteSpace(politicianAddress))
        AddItem(tdInfo, "No Address", Options.Strong);
      else
      {
        AddItem(tdInfo, politicianAddress);
        new HtmlBreak().AddTo(tdInfo);
        AddItem(tdInfo, row.PublicCityStateZip());
      }

      var socialMediaAnchors = SocialMedia.GetAnchors(row, out var socialMediaCount);
      new HtmlBreak(2).AddTo(tdInfo);
      if (socialMediaCount == 0)
      {
        AddItem(tdInfo, "No social media links or email address", Options.Strong | Options.Em);
        new HtmlBreak().AddTo(tdInfo);
      }
      else
        socialMediaAnchors.AddTo(tdInfo);

      // Web address
      new HtmlBreak().AddTo(tdInfo);
      var webAddress = row.PublicWebAddress();
      if (IsNullOrWhiteSpace(webAddress))
      {
        AddItem(tdInfo, "No web address", Options.Strong | Options.Em);
      }
      else
        new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(webAddress),
          Target = "website",
          InnerHtml = "Have web address"
        }.AddTo(tdInfo);

      new HtmlBreak(2).AddTo(tdInfo);
      //if (row.AnswerCount() > 0)
      //  AddItem(tdInfo, "Have " + row.AnswerCount() + " responses to issue questions");
      //else
      //  AddItem(tdInfo, "No responses to issue questions", Options.Strong | Options.Em);

      AddIssueItems(new HtmlTableCell().AddTo(tr, "tdReportDetailSmall"), row);

      var tdAnchors = new HtmlTableCell().AddTo(tr, "tdReportDetailLarge");
      var politicianName = Politicians.FormatName(row, true, 30);
      new HtmlStrong {InnerText = Politicians.FormatName(row, true, 30)}.AddTo(tdAnchors, "head");

      // Office
      new HtmlBreak().AddTo(tdAnchors);
      new HtmlStrong {InnerText = Offices.FormatOfficeName(row)}.AddTo(tdAnchors, "head");

      // Jurisdiction
      new HtmlBreak().AddTo(tdAnchors);
      new HtmlStrong
      {
        InnerText =
          Offices.GetElectoralClassDescription(row.StateCode(), row.CountyCode(),
            row.LocalKey())
      }.AddTo(tdAnchors, "head");

      // UpdateIntro Page
      var title = "<strong>Enter</strong> info including:" +
        " <span>&#x25ba; candidate</span> photo" +
        " <span>&#x25ba; email</span> and website address" +
        " <span>&#x25ba; social</span> media and video links" +
        " <span>&#x25ba; biographical</span> info" +
        " <span>&#x25ba; reasons</span> and objectives";
      new HtmlBreak(2).AddTo(tdAnchors);
      new HtmlAnchor
      {
        HRef = SecurePoliticianPage.GetUpdateIntroPageUrl(politicianKey),
        Target = "politician",
        InnerHtml = title
      }.AddTo(tdAnchors);

      // UpdateIssues
      //title = "<strong>Enter</strong> responses to issue questions";
      //new HtmlBreak(2).AddTo(tdAnchors);
      //new HtmlAnchor
      //{
      //  HRef = SecurePoliticianPage.GetUpdateIssuesPageUrl(politicianKey),
      //  Target = "politician",
      //  InnerHtml = title
      //}.AddTo(tdAnchors);

      // Introduction Page
      title = "<strong>View</strong> our Introduction page for " + politicianName;
      new HtmlBreak(2).AddTo(tdAnchors);
      new HtmlAnchor
      {
        HRef = UrlManager.GetIntroPageUri(politicianKey).ToString(),
        Target = "politician",
        InnerHtml = title
      }.AddTo(tdAnchors);

      // Comparison Page (PartiesElectionReport only)
      var electionKey = row.Table.Columns.Contains("ElectionKey")
        ? row.ElectionKey()
        : null;
      if (!IsNullOrWhiteSpace(electionKey))
      {
        title = "<strong>View</strong> our Candidate Comparison page for " + politicianName;
        new HtmlBreak(2).AddTo(tdAnchors);
        new HtmlAnchor
        {
          HRef = UrlManager.GetCompareCandidatesPageUri(electionKey, row.OfficeKey()).ToString(),
          Target = "politician",
          InnerHtml = title
        }.AddTo(tdAnchors);
      }
    }

    #endregion Protected
  }
}