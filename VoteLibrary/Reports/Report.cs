using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Reports
{
  public class Report
  {
    #region Private

    private static Control CreatePoliticianImageAnchorPrivate(string href,
      string politicianKey, int imageWidth, string align, string title, string target,
      bool noCache = false)
    {
      var placeHolder = new PlaceHolder();
      if (politicianKey == null || politicianKey == "NoRunningMate")
      {
        new HtmlSpan
        {
          InnerHtml = "No Running Mate Selected"
        }.AddTo(placeHolder, "no-running-mate");
      }
      else
      {
        var a = new HtmlAnchor
        {
          HRef = href,
          Title = title,
          Target = target
        }.AddTo(placeHolder);
        CreatePoliticianImageTag(politicianKey, imageWidth, noCache, align).AddTo(a);
      }

      return placeHolder;
    }

    private static Control CreatePoliticianWebsiteAnchor(string webAddress,
      string anchorText, string title)
    {
      Control control;
      if (IsNullOrEmpty(webAddress))
        control = new Literal();
      else
      {
        var a = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(webAddress),
          Target = "view",
          Title = title,
          InnerHtml = anchorText ?? webAddress
        };
        control = a;
      }
      return control;
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    protected ReportUser ReportUser = ReportUser.Public;

    protected static HtmlAnchor CreateAdminOfficeAnchor(string officeKey,
      string anchorText, string target = "office")
    {
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetOfficePageEditUrl(officeKey),
        Target = target,
        InnerHtml = anchorText
      };
      return a;
    }

    protected static HtmlAnchor CreateAdminOfficeCandidateListAnchor(string electionKey,
      string officeKey, string anchorText, string title = "",
      string target = "officecontest")
    {
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetOfficeCandidateListUrl(electionKey, officeKey),
        Target = target,
        Title = title,
        InnerHtml = anchorText
      };
      return a;
    }

    protected static HtmlAnchor CreateAdminOfficeWinnerAnchor(string electionKey,
      string officeKey, string anchorText, string target = "officewinner")
    {
      var a = new HtmlAnchor
      {
        HRef =
          SecureAdminPage.GetOfficeWinnerPageUrl(electionKey, officeKey),
        Target = target,
        InnerHtml = anchorText
      };

      return a;
    }

    protected static HtmlAnchor CreateAdminPoliticianAnchor(DataRow politician,
      string anchorText, string target = "politician")
    {
      var href = SecurePoliticianPage.GetUpdateIntroPageUrl(politician.PoliticianKey());
      if (VotePage.IsPublicPage)
        href = UrlManager.GetAdminUri(href).ToString();
      var a = new HtmlAnchor
      {
        HRef = href,
        Target = target,
        Title = "Edit Links, Picture, Bio & Reasons",
        InnerHtml = anchorText
      };
      return a;
    }

    protected static HtmlAnchor CreateAdminReferendumAnchor(string electionKey,
      // ReSharper disable once UnusedParameter.Global
      string referendumKey, string anchorText)
    {
      //var a = new HtmlAnchor
      //{
      //  HRef = SecureAdminPage.GetReferendumPageUrl(electionKey, referendumKey),
      //  Target = "edit",
      //  Title = "Edit Description, Details and Full Text",
      //  InnerHtml = anchorText
      //};
      var a = new HtmlAnchor
      {
        HRef = SecureAdminPage.GetUpdateElectionsPageUrlByElection(electionKey, "addballotmeasures"),
        Target = "edit",
        Title = "Edit Description, Details and Full Text",
        InnerHtml = anchorText
      };

      return a;
    }

    protected static HtmlAnchor CreateCompareTheCandidatesAnchor(string electionKey,
      string officeKey)
    {
      var a = new HtmlAnchor
      {
        HRef =
          UrlManager.GetCompareCandidatesPageUri(Offices.GetStateCodeFromKey(officeKey),
              electionKey, officeKey)
            .ToString(),
        Title = "Compare candidates' bios and views and positions on the issues",
        InnerHtml = "Compare the candidates"
      };
      a.AddCssClasses("compare-candidates no-print");
      return a;
    }

    protected static HtmlAnchor CreateCompareTheCandidatesAnchorTable(string electionKey,
      string officeKey, string issueKey = "ALLBio")
    {
      var a = new HtmlAnchor
      {
        HRef =
          UrlManager.GetIssuePageUri(Offices.GetStateCodeFromKey(officeKey),
              electionKey, officeKey, issueKey)
            .ToString(),
        Title = "Compare candidates' bios and views and positions on the issues",
        InnerHtml = "Compare the candidates"
      };
      a.AddCssClasses("compareCandidates");
      return a;
    }

    //protected static HtmlAnchor CreateCompareViaExplorerAnchor(string electionKey,
    //  string officeKey, string congress, string stateSenate, string stateHouse,
    //  string county)
    //{
    //  var a = new HtmlAnchor
    //  {
    //    HRef =
    //      UrlManager.GetIssue2PageUri(Offices.GetStateCodeFromKey(officeKey),
    //        electionKey, congress, stateSenate, stateHouse, county, officeKey)
    //        .ToString(),
    //    Title = "Compare candidates' bios and views and positions on the issues",
    //    InnerHtml = "Compare the candidates"
    //  };
    //  a.AddCssClasses("compareCandidates");
    //  return a;
    //}

    protected static HtmlAnchor CreateIssuePageAnchor(string electionKey,
      string officeKey, string issueKey, string anchorText, string title,
      string target = "")
    {
      var a = new HtmlAnchor
      {
        HRef =
          UrlManager.GetIssuePageUri(Offices.GetStateCodeFromKey(officeKey),
              electionKey, officeKey, issueKey)
            .ToString(),
        Title = title,
        Target = target,
        InnerHtml = anchorText
      };

      return a;
    }

    //protected static Control CreateCenteredPoliticianImageAnchor(string href,
    //  string politicianKey, int imageWidth, string title, string target = "_self")
    //{
    //  return CreatePoliticianImageAnchorPrivate(href, politicianKey, imageWidth, "center",
    //    title, target);
    //}

    protected static Control CreatePoliticianImageAnchor(string href,
      string politicianKey, int imageWidth, string title, string target = "_self", bool noCache = false)
    {
      return CreatePoliticianImageAnchorPrivate(href, politicianKey, imageWidth, "left",
        title, target, noCache);
    }

    protected static HtmlAnchor CreatePoliticianIntroAnchor(DataRow politician,
      string anchorText = "", string title = "", string target = "_self")
    {
      var politicianName = Politicians.FormatName(politician, true);

      if (IsNullOrEmpty(anchorText))
        anchorText = politicianName;

      if (IsNullOrEmpty(title))
        title = politicianName +
          "'s biographical information and positions and views on the issues";

      return new HtmlAnchor
      {
        HRef = UrlManager.GetIntroPageUri(politician.PoliticianKey())
          .ToString(),
        Title = title,
        Target = target,
        InnerHtml = anchorText
      };
    }

    private const bool DoFilterUncontestedOffices = false;

    protected static IEnumerable<IGrouping<string, DataRow>>
      FilterUncontestedOffices(IEnumerable<IGrouping<string, DataRow>> offices)
    {
      // ReSharper disable once CSharpWarnings::CS0162
      // ReSharper disable once ConditionIsAlwaysTrueOrFalse
      return offices.Where(o => !DoFilterUncontestedOffices || o.Count() > 1);
    }

    protected static void FormatAnswer(DataRow answerRow, Control td, bool prependName)
    {
      const int maxLengthSource = 45;
      if (answerRow != null)
      {
        var div = new HtmlDiv().AddTo(td);
        if (!IsNullOrWhiteSpace(answerRow.Answer()))
        {
          var answer = TruncateAnswer(answerRow.Answer());
          if (prependName)
            answer = PrependName(answerRow.LastName(), answer);
          new LiteralControl(answer.ReplaceNewLinesWithParagraphs()).AddTo(div);
          var answerSource = answerRow.AnswerSource()
            .SafeString();
          var answerDate = answerRow.AnswerDate(VotePage.DefaultDbDate);
          if (!IsNullOrWhiteSpace(answerSource) || !answerDate.IsDefaultDate())
          {
            var p = new HtmlP().AddTo(div);
            if (!IsNullOrWhiteSpace(answerSource))
            {
              Debug.Assert(answerSource != null, "answerSource != null");
              if (answerSource.Length > maxLengthSource)
              {
                var slashLocation = answerSource.IndexOf("/",
                  StringComparison.Ordinal);
                answerSource = slashLocation != -1
                  ? answerSource.Insert(slashLocation + 1, "<br />")
                  : answerSource.Insert(maxLengthSource, "<br />");
              }
              var span = new HtmlSpan().AddTo(p, "source");
              new HtmlSpan {InnerHtml = "Source: "}.AddTo(span);
              new LiteralControl(answerSource).AddTo(span);
            }
            if (!answerDate.IsDefaultDate())
              new LiteralControl(" (" + answerDate.ToString("MM/dd/yyyy") + ")").AddTo(p);
          }
        }
        if (!IsNullOrWhiteSpace(answerRow.YouTubeUrl()))
        {
          var p = new HtmlP().AddTo(div);
          var anchor = new HtmlAnchor
          {
            HRef = answerRow.YouTubeUrl(),
            Target = "YouTube"
          }.AddTo(p);
          new HtmlImage {Src = "/images/youTubeWide.jpg"}.AddTo(anchor);
          var youTubeDate = answerRow.YouTubeDate(VotePage.DefaultDbDate);
          if (!youTubeDate.IsDefaultDate())
          {
            new HtmlSpan
            {
              InnerHtml = "(" + youTubeDate.ToString("MM/dd/yyyy") + ")"
            }.AddTo(p, "youtubedate");
          }
        }
      }
      if (td.Controls.Count == 0)
        new HtmlNbsp().AddTo(td);
    }

    //protected Control FormatNameAndParty(DataRow politician, bool showParty = true)
    //{
    //  var span = new HtmlSpan();

    //  var politicianName = Politicians.FormatName(politician, true);
    //  var politicianAnchor = ReportUser == ReportUser.Public
    //    ? CreatePoliticianIntroAnchor(politician, politicianName)
    //    : CreateAdminPoliticianAnchor(politician, politicianName);
    //  politicianAnchor.AddTo(span);

    //  if (showParty && !IsNullOrWhiteSpace(politician.PartyCode()))
    //  {
    //    new Literal { Text = " - " }.AddTo(span);
    //    CreatePartyAnchor(politician, "view").AddTo(span);
    //  }

    //  return span;
    //}

    protected Control FormatNameAndPartyTable(DataRow politician, bool showParty = true)
    {
      var placeHolder = new PlaceHolder();
      var span = new HtmlSpan().AddTo(placeHolder, "TName");

      var politicianName = Politicians.FormatName(politician, true);
      var politicianAnchor = ReportUser == ReportUser.Public
        ? CreatePoliticianIntroAnchor(politician, politicianName)
        : CreateAdminPoliticianAnchor(politician, politicianName);
      politicianAnchor.AddTo(span);

      if (showParty && !IsNullOrWhiteSpace(politician.PartyCode()))
      {
        new Literal {Text = " - "}.AddTo(span);
        CreatePartyAnchor(politician, "view").AddTo(span);
      }

      return placeHolder;
    }

    protected static Control FormatPoliticianWebsite(DataRow politician, string text = "&#x25BA;Website")
    {
      Control control;

      var publicWebAddress = politician.PublicWebAddress();
      if (!IsNullOrEmpty(publicWebAddress))
      {
        control = new PlaceHolder();
        var span = new HtmlSpan().AddTo(control);
        var anchor = CreatePoliticianWebsiteAnchor(publicWebAddress, text,
          publicWebAddress);
        anchor.AddTo(span);
        if (anchor is HtmlAnchor) (anchor as HtmlAnchor).Attributes["rel"] = "nofollow";
      }
      else control = new Literal();

      return control;
    }

    protected static Control FormatPoliticianWebsiteTable(DataRow politician, int breakTags = 2)
    {
      Control control;

      var publicWebAddress = politician.PublicWebAddress();
      if (!IsNullOrEmpty(publicWebAddress))
      {
        control = new PlaceHolder();
        if (breakTags > 0)
          new HtmlBreak(breakTags).AddTo(control);
        var span = new HtmlSpan().AddTo(control, "TWebsite");
        CreatePoliticianWebsiteAnchor(publicWebAddress, "Website",
          Politicians.FormatName(politician, true, 30) + "'s Website").AddTo(span);
      }
      else control = new Literal();

      return control;
    }

    //protected static void PrependName(string nameToPrepend, Control td)
    //{
    //  if (IsNullOrWhiteSpace(nameToPrepend)) return;
    //  new HtmlSpan { InnerHtml = nameToPrepend + ": " }.AddTo(td, "last-name");
    //}

    protected static string PrependName(string nameToPrepend, string prependTo)
    {
      if (IsNullOrWhiteSpace(nameToPrepend)) return prependTo;
      return
        new HtmlSpan {InnerHtml = nameToPrepend + ": "}.AddCssClasses("last-name").RenderToString() +
        prependTo;
    }

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Public

    protected const int ImageSize100 = 100;
    protected const int ImageSize300 = 300;

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static Control CreatePartyAnchor(DataRow politician, string target = "")
    {
      Control control;
      if (IsNullOrWhiteSpace(politician.PartyCode()))
        control = new Literal();
      else if (IsNullOrWhiteSpace(politician.PartyUrl()))
        control = new Literal {Text = politician.PartyCode()};
      else
      {
        var a = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(politician.PartyUrl()),
          Title = politician.PartyName() + " Website",
          Target = target,
          InnerHtml = politician.PartyCode()
        };
        control = a;
      }

      return control;
    }

    public static HtmlImage CreatePoliticianImageTag(string politicianKey, int imageWidth,
      bool noCache = false, string align = "left")
    {
      var img = new HtmlImage
      {
        Src = VotePage.GetPoliticianImageUrl(politicianKey, imageWidth, noCache),
        Align = align
      };

      return img;
    }

    public static ReportUser SignedInReportUser
    {
      get
      {
        if (SecurePage.IsMasterUser)
          return ReportUser.Master;
        return SecurePage.IsAdminUser
          ? ReportUser.Admin
          : ReportUser.Public;
      }
    }

    public static string TruncateAnswer(string answer)
    {
      const int maxAnswerLength = 2000;
      const int fudge = 100;
      if (answer.Length > maxAnswerLength + fudge)
      {
        answer = answer.Substring(0, maxAnswerLength + fudge);
        answer += " [Response was truncated to maximum response length of " +
          maxAnswerLength + " characters.] ";
      }
      return answer;
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

  public class ReportDataManager<T> where T : DataRow
  {
    #region Protected

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    protected List<T> DataList;

    protected DataTable DataTable
    {
      set
      {
        DataList = value.Rows.OfType<T>().ToList();
      }
    }

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public abstract class FilterBy
    {
      public abstract bool Filter(T row);
    }

    public abstract class OrderBy : IComparer<T>
    {
      public abstract int Compare(T row1, T row2);
    }

    public IList<T> GetDataSubset(FilterBy filterBy = null, OrderBy orderBy = null)
    {
      IEnumerable<T> result = DataList;
      if (filterBy != null)
        result = result.Where(filterBy.Filter);
      if (orderBy != null)
        result = result.OrderBy(row => row, orderBy);
      return result.ToList();
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public enum ReportUser
  {
    Public,
    Admin,
    Master
  }
}