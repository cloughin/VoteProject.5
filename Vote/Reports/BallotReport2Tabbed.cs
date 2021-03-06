using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal class BallotReport2Tabbed : BallotReport2
  {
    internal BallotReport2Tabbed() : base(true, true) {}

    #region private

    private readonly StaticClientIdPlaceHolder _ReportPlaceHolder =
      new StaticClientIdPlaceHolder();

    private Control _ReportContainer;
    private Control _Outer;
    private Control _MainTabControl;
    private Control _MainTabControlTabs;
    private Control _CurrentOfficeTabControl;
    private Control _CurrentOfficeTabControlTabs;
    private Control _CurrentOfficeInfoContent;
    private Control _CurrentOfficeBioBar;
    private Control _CurrentOfficeIssuesBar;
    private HtmlControl _IssuesBioBarItem;
    private bool _MultiLocalMode;
    private Control _MultiLocalAccordion;
    private Control _CurrentMultiLocalAccordionList;
    private BioInfo[] _BioInfos;
    private DataTable _Issues;
    private DataTable _Referendums;
    private List<DataRow> _ReferendumsInElection; 

    private List<IGrouping<string, QuestionEntry>> _OfficeIssues;

    private class BioInfo
    {
      public string Column { get; private set; }
      public string Label { get; private set; }
      public string CssClass { get; private set; }
      public string ToolTip { get; private set; }
      public bool HasValue;
      public HtmlControl Container;
      public HtmlControl BioBarItem;

      public BioInfo(string column, string toolTip, string label = null,
        string cssClass = null)
      {
        Column = column;
        ToolTip = toolTip;
        Label = label ?? Column;
        CssClass = cssClass ?? Label.ToLowerInvariant();
      }
    }

    private class QuestionEntry
    {
      public readonly IGrouping<string, DataRow> QuestionGroup;
      public Control Container;

      public QuestionEntry(IGrouping<string, DataRow> questionGroup)
      {
        QuestionGroup = questionGroup;
      }
    }

    private static void AddBioInfo(DataRow candidate, BioInfo bioInfo)
    {
      var value = candidate == null 
        ? string.Empty 
        : (candidate[bioInfo.Column] as string).SafeString().Trim();
      var item =
        new HtmlDiv { InnerText = value }.AddTo(bioInfo.Container,
          "info-item bio-item");
      if (value == string.Empty) item.AddCssClasses("empty-item");
      else bioInfo.HasValue = true;
    }

    private static void AddIssue(DataRow candidate, QuestionEntry question)
    {
      var item = new HtmlDiv();
      item.AddTo(question.Container, "info-item issue-item");
      var row =
        question.QuestionGroup.FirstOrDefault(
          r => r.PoliticianKey().IsEqIgnoreCase(candidate.PoliticianKey()));
      if (row == null) // no answer
        item.AddCssClasses("empty-item");
      else item.InnerText = row.Answer();
    }

    private void BeginJurisdictionLevel(string tabName, string tabId)
    {
      var jQueryTabId = "tab-" + tabId.ToLowerInvariant();

      var tab =
        new HtmlLi {EnableViewState = false}.AddTo(
          _MainTabControlTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#" + jQueryTabId,
        ID = tabId,
        EnableViewState = false,
        InnerText = tabName
      }.AddTo(tab)
        .Attributes["onclick"] = "this.blur()";

      var mainTabContent =
        new HtmlDiv {ID = jQueryTabId}.AddTo(_MainTabControl,
          "main-tab content-panel tab-panel htab-panel");

      _CurrentOfficeTabControl =
        new HtmlDiv {ID = tabId + "-tabs"}.AddTo(mainTabContent,
          tabId + "-tabs tab-control vtab-control jqueryui-tabs");

      _CurrentOfficeTabControlTabs =
        new HtmlUl().AddTo(_CurrentOfficeTabControl,
          "tabs vtabs unselectable");
    }

    private HtmlControl ReportCandidate(DataRow candidate)
    {
      var info = new HtmlDiv().AddCssClasses("info");

      var name = new HtmlP().AddTo(info, "name");
      var isPrimary = Elections.IsPrimaryElection(candidate.ElectionKey());
      var isNonPartisan =
        Elections.GetNationalPartyCode(candidate.ElectionKey()) == "X";
      FormatCandidate(candidate, true, (!isPrimary || isNonPartisan) && 
        !candidate.IsRunningMate())
        .AddTo(name);

      if (candidate.IsWinner() && !candidate.IsRunningMate())
      {
        info.AddCssClasses("winner");
        new HtmlDiv()
         .Attribute("title", "Winner")
          .AddTo(info, "winner-icon");
      }

      new HtmlImage
      {
        Src = VotePage.GetPoliticianImageUrl(candidate.PoliticianKey(), 75)
      }.AddTo(
        info, "rounded-border headshot");

      var rightSide = new HtmlDiv().AddTo(info, "right-side");

      FormatCandidatePhone(candidate)
        .AddTo(rightSide);

      FormatCandidateWebsite(candidate)
        .AddTo(rightSide);

      SocialMedia.GetAnchors(candidate)
        .AddCssClasses("social")
        .AddTo(rightSide);

      var address = candidate.PublicAddress();
      var cityStateZip = candidate.PublicCityStateZip();
      if (!string.IsNullOrWhiteSpace(address) &&
        !string.IsNullOrWhiteSpace(cityStateZip))
      {
        var addressTag = new HtmlDiv().AddTo(rightSide, "address");
        new HtmlP { InnerText = address }.AddTo(addressTag);
        new HtmlP { InnerText = cityStateZip }.AddTo(addressTag);
      }

      var age = candidate.Age();
      if (!string.IsNullOrWhiteSpace(age))
      {
        var ageTag = new HtmlDiv().AddTo(rightSide, "age");
        new HtmlSpan { InnerText = "Age: " }.AddTo(ageTag);
        new LiteralControl(age).AddTo(ageTag);
      }

      ClearBoth()
        .AddTo(info);

      return info;
    }

    private void ReportReferendums(string electionKey)
    {
      if (_ReferendumsInElection.Count <= 0) return;

      var electionKeyLower = electionKey.ToLowerInvariant();
      var jQueryTabId = "tab-bms-" + electionKeyLower;
      var tabId = "Tab_Bms_" + electionKeyLower;

      var tab =
        new HtmlLi { EnableViewState = false }.AddTo(
          _CurrentOfficeTabControlTabs, "tab vtab");
      new HtmlAnchor
      {
        HRef = "#" + jQueryTabId,
        ID = tabId,
        EnableViewState = false,
        InnerText = "Ballot Measures"
      }.AddTo(tab)
        .Attribute("onclick", "this.blur()");

      if (_MultiLocalMode)
      {
        tab = new HtmlLi { EnableViewState = false }.AddTo(
          _CurrentMultiLocalAccordionList, "tab vtab accordion-tab")
          .Attribute("tabid", "Tab_Bms_" + electionKeyLower);
        new HtmlAnchor { EnableViewState = false, InnerText = "Ballot Measures" }.AddTo(tab);
      }

      var tabContent =
        new HtmlDiv {ID = jQueryTabId}.AddTo(
          _CurrentOfficeTabControl,
          "content-panel tab-panel vtab-panel office-panel referendums-panel");

      var accordion = new HtmlDiv().AddTo(tabContent, "accordion");

      foreach (var referendum in _ReferendumsInElection)
      {
        new HtmlH3
        {
          InnerText = referendum.ReferendumTitle()
        }.AddTo(accordion, "accordion-header");
        var content = new HtmlDiv()
          .AddTo(accordion);
        if (referendum.IsResultRecorded())
          new HtmlDiv()
            .Attribute("title", referendum.IsPassed() 
              ? "Ballot measure passed" : "Ballot measure was defeated")
            .AddTo(content,
              "referendum-result " +
                (referendum.IsPassed() ? "passed" : "defeated"));
        new HtmlP
        {
          InnerText = referendum.ReferendumDescription()
        }.AddTo(content);
      }
    }

    #endregion private

    #region private (but should become protected)

    private const string OffsiteTarget = "view";

    protected Control ClearBoth()
    {
      return new HtmlDiv().AddCssClasses("clear-both");
    }

    private void ExtractReferendums(string electionKey)
    {
      _ReferendumsInElection = _Referendums.Rows.Cast<DataRow>()
        .Where(row => row.ElectionKey()
          .IsEqIgnoreCase(electionKey))
        .OrderBy(row => row.OrderOnBallot())
        .ToList();
    }

    protected Control FormatCandidate(DataRow candidate, bool showIncumbent,
      bool showParty)
    {
      var placeHolder = new PlaceHolder();

      new LiteralControl(Politicians.FormatName(candidate, true)).AddTo(placeHolder);
      if (showIncumbent && candidate.IsIncumbent())
        new LiteralControl("&nbsp;*").AddTo(
          new HtmlSpan().AddTo(placeHolder, "incumbent"));

      if (!showParty || string.IsNullOrWhiteSpace(candidate.PartyCode())) return placeHolder;

      var span = new HtmlSpan().AddTo(placeHolder, "party");
      new Literal {Text = " - "}.AddTo(span);
      FormatPartyAnchor(candidate)
        .AddTo(span);

      return placeHolder;
    }

    protected Control FormatCandidatePhone(DataRow candidate)
    {
      Control control = new PlaceHolder();

      var publicPhone = candidate.PublicPhone();
      if (!string.IsNullOrEmpty(publicPhone))
      {
        var div = new HtmlDiv().AddTo(control, "phone");
        new HtmlImage {Src = "/images/phone.png"}.AddTo(div);
        new HtmlSpan {InnerText = FormatPhone(publicPhone)}.AddTo
          (div);
      }

      return control;
    }

    protected Control FormatCandidateWebsite(DataRow candidate)
    {
      Control control = new PlaceHolder();

      var publicWebAddress = candidate.PublicWebAddress();
      if (!string.IsNullOrEmpty(publicWebAddress))
      {
        var div = new HtmlDiv().AddTo(control, "website");
        new HtmlImage {Src = "/images/website.png"}.AddTo(div);
        new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(publicWebAddress),
          Target = OffsiteTarget,
          Title = Politicians.FormatName(candidate) + "'s Website",
          InnerText = "Website"
        }.AddTo(new HtmlSpan().AddTo(div));
      }

      return control;
    }

    public static Control FormatPartyAnchor(DataRow politician)
    {
      Control control;
      if (string.IsNullOrWhiteSpace(politician.PartyCode())) control = new Literal();
      else if (string.IsNullOrWhiteSpace(politician.PartyUrl())) control = new Literal {Text = politician.PartyCode()};
      else
      {
        var a = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(politician.PartyUrl()),
          Title = politician.PartyName() + " Website",
          Target = OffsiteTarget,
          InnerText = politician.PartyCode()
        };
        control = a;
      }

      return control;
    }

    public static string FormatPhone(string phone)
    {
      // if there are exactly 10 digits, excluding a leading 1, we format
      // it as (xxx) xxx-xxxx. Otherwise return it unchanged.
      var digits = Regex.Replace(phone, "^1|[^0-9]", "");
      if (digits.Length == 10)
        return "(" + digits.Substring(0, 3) + ") " + digits.Substring(3, 3) + "-" +
          digits.Substring(6);
      return phone;
    }

    #endregion private (but should become protected)

    #region Event handlers and overrides

    protected override void OnBeginCounty(string electionKey, string countyName, int officeCount)
    {
      ExtractReferendums(electionKey);
      if (officeCount > 0)
      {
        BeginJurisdictionLevel(countyName, "CountyTab");
        _Outer.RemoveCssClass("state-only");
      }
      else ReportReferendums(electionKey);
    }

    protected override void OnBeginLocal(string electionKey, string localCode, string localName, int officeCount)
    {
      ExtractReferendums(electionKey);
      if (officeCount > 0 && _MultiLocalMode)
      {
        // create accordion entry
        new HtmlH3 { InnerText = localName }.AddTo(
          _MultiLocalAccordion, "accordion-header");
        _CurrentMultiLocalAccordionList =
          new HtmlUl().AddTo(_MultiLocalAccordion);
      }
      else ReportReferendums(electionKey);
    }

    protected override void OnBeginLocals(int localCount)
    {
      if (localCount > 1)
      {
        BeginJurisdictionLevel("Local Districts", "LocalsTab");
        _Outer.RemoveCssClass("state-only");
        _MultiLocalMode = true;
        _MultiLocalAccordion =
          new HtmlDiv().AddTo(_CurrentOfficeTabControl, "accordion");
      }
    }

    protected override void OnBeginOffice(string electionKey, DataRow officeInfo, 
      IList<string> candidateKeys, IList<string> runningMateKeys)
    {
      _OfficeIssues = _Issues.Rows.Cast<DataRow>()
        .Where(
          row =>
            candidateKeys.Contains(row.PoliticianKey(),
              StringComparer.OrdinalIgnoreCase) ||
              runningMateKeys.Contains(row.PoliticianKey(),
              StringComparer.OrdinalIgnoreCase))
        .GroupBy(row => row.QuestionKey())
        .Select(g => new QuestionEntry(g))
        .ToList()
        .GroupBy(q => q.QuestionGroup.First().IssueKey())
        .ToList();

      var officeKeyLower = officeInfo.OfficeKey()
        .ToLowerInvariant();
      var officeName = Offices.FormatOfficeName(officeInfo);

      var jQueryTabId = "tab-" + officeKeyLower;

      var tabId = "Tab_" + officeKeyLower;
      var tab =
        new HtmlLi {EnableViewState = false}.AddTo(
          _CurrentOfficeTabControlTabs, "tab vtab");
      new HtmlAnchor
      {
        HRef = "#" + jQueryTabId,
        ID = tabId,
        EnableViewState = false,
        InnerText = officeName
      }.AddTo(tab)
        .Attribute("onclick", "this.blur()");

      if (_MultiLocalMode)
      {
        tab = new HtmlLi {EnableViewState = false}.AddTo(
          _CurrentMultiLocalAccordionList, "tab vtab accordion-tab")
          .Attribute("tabid", "Tab_" + officeKeyLower);
        new HtmlAnchor {EnableViewState = false, InnerText = officeName}.AddTo(tab);
      }

      var tabContent =
        new HtmlDiv {ID = jQueryTabId}.AddTo(
          _CurrentOfficeTabControl,
          "content-panel tab-panel vtab-panel office-panel" + 
          (officeInfo.IsRunningMateOffice() ? " running-mate-office" : string.Empty));

      var maxCandidatesShowing = officeInfo.IsRunningMateOffice() ? 2 : 4;
      var outer = new HtmlDiv().AddTo(tabContent,
        "many-candidates-outer");
      new LiteralControl("Select candidates to compare &#x25bc;").AddTo(
        new HtmlDiv().AddTo(outer, "select-candidates"));
      var manyCandidates = new HtmlDiv
      {
        InnerText =
          string.Format(
            "{0} total {1}, ",
            candidateKeys.Count, officeInfo.IsRunningMateOffice() ? "tickets" : "candidates"),
      }.AddTo(outer, "many-candidates");
      new HtmlSpan
      {
        InnerText = " all are selected."
      }.AddTo(manyCandidates, "number-selected");
      if (candidateKeys.Count > maxCandidatesShowing)
      {
        new HtmlSpan
        {
          InnerText = " Click the arrows or drag to see more candidates."
        }.AddTo(manyCandidates, "scroll-message");
        new HtmlDiv().AddTo(tabContent, "scroll-arrow left disabled");
        new HtmlDiv().AddTo(tabContent,
          "scroll-arrow right");
      }

      var candidateFrame = new HtmlDiv().AddTo(tabContent,
        "candidate-frame");

      _CurrentOfficeInfoContent =
        new HtmlDiv().AddTo(candidateFrame, "candidates");

      // only put out votefor if its not "one"
      if (!Regex.IsMatch(officeInfo.VoteForWording(), 
        @"(?:(?:\A|\D)1(?:\D|\z))|(?:(?:\A|[^a-z])one(?:[^a-z]|\z))", 
        RegexOptions.IgnoreCase))
        new HtmlDiv {InnerText = officeInfo.VoteForWording()}
          .AddTo(tabContent, "vote-for");

      _CurrentOfficeBioBar = new HtmlUl().AddTo(tabContent,
        "bio-bar rounded-border unselectable");

      // only create issues bar if there are issue responses for the office
      if (_OfficeIssues.Count > 0)
      {
        _CurrentOfficeIssuesBar = new HtmlUl().AddTo(tabContent,
          "issue-bar rounded-border unselectable hidden");
        new HtmlLi().AddTo(_CurrentOfficeIssuesBar,
          "issue-bar-item issue-bar-arrow issue-arrow left")
          .Attribute("title", "Previous issue");
        new HtmlLi().AddTo(_CurrentOfficeIssuesBar,
          "issue-bar-item issue-bar-arrow issue-arrow right")
          .Attribute("title", "Next issue");
        var textContainer = new HtmlLi()
          .AddTo(_CurrentOfficeIssuesBar, "issue-bar-item text-container");
        new HtmlDiv().AddTo(textContainer,
          "text-menu text-issue-menu");
        new HtmlDiv { InnerText = "Issue" }.AddTo(
          textContainer, "issue-bar-text issue-text");
        new HtmlLi().AddTo(_CurrentOfficeIssuesBar,
          "issue-bar-item issue-bar-arrow question-arrow left")
          .Attribute("title", "Previous question for the issue");
        new HtmlLi().AddTo(_CurrentOfficeIssuesBar,
          "issue-bar-item issue-bar-arrow question-arrow right")
          .Attribute("title", "Next question for the issue");
        textContainer = new HtmlLi()
          .AddTo(_CurrentOfficeIssuesBar, "issue-bar-item text-container");
        new HtmlDiv().AddTo(textContainer,
         "text-menu text-question-menu");
        new HtmlLi { InnerText = "Question" }.AddTo(
          textContainer, "issue-bar-text question-text");
      }
      else _CurrentOfficeIssuesBar = null;

      var infoContent = new HtmlDiv().AddTo(tabContent,
        "info-group-frame");

      foreach (var info in _BioInfos)
      {
        info.HasValue = false;
        info.Container = new HtmlDiv().AddTo(infoContent,
          "info-group " + info.CssClass);
        info.BioBarItem =
          new HtmlLi {InnerText = info.Label}.AddTo(
            _CurrentOfficeBioBar, "bio-bar-item bio-class-" + info.CssClass);
      }

      // add the issues bio-bar-item
      _IssuesBioBarItem =
        new HtmlLi {InnerText = "Issues"}.AddTo(
          _CurrentOfficeBioBar, "bio-bar-item bio-class-issues");

      // create an info group for each question in the office
      foreach (var issue in _OfficeIssues)
        foreach (var question in issue)
          question.Container = new HtmlDiv().AddTo(infoContent,
            "hidden info-group issue-group q-" +
              question.QuestionGroup.Key.ToLowerInvariant() + " i-" +
              question.QuestionGroup.First().IssueKey().ToLowerInvariant())
            .Attribute("issue", question.QuestionGroup.First()
              .Issue())
            .Attribute("question", question.QuestionGroup.First()
              .Question());
    }

    protected override void OnBeginReport()
    {
      // get the issue data
      _Issues = ElectionsPoliticians.GetSampleBallotIssues(ElectionKey, Congress,
        StateSenate, StateHouse, CountyCode);

      // get the referendum data
      _Referendums = Referendums.GetExplorerData(ElectionKey, CountyCode);

      _Outer =
        new HtmlDiv {ID = "VoteReport"}.AddTo(
          _ReportPlaceHolder, "vote-report state-only");

      _ReportContainer =
        new HtmlDiv { ID = "VoteBallotReportTabbed" }.AddTo(_Outer,
          "vote-ballot-report-tabbed");

      _MainTabControl =
        new HtmlDiv {ID = "MainTabs"}.AddTo(_ReportContainer,
          "tab-control htab-control jqueryui-tabs start-hidden shadow");

      _MainTabControlTabs = new HtmlUl().AddTo(_MainTabControl,
        "tabs htabs unselectable");

      //_BioInfos = new[]
      //{
      //  new BioInfo("GeneralStatement",
      //    "political statement of goals, objectives, views, philosophies",
      //    "General"),
      //  new BioInfo("Personal",
      //    "gender, age, marital status, spouse's name and age, children's name and ages, home town, current residence"),
      //  new BioInfo("Education",
      //    "times and places of schools, colleges, major, degrees, activities, sports"),
      //  new BioInfo("Profession",
      //    "professional and work experience outside politics"),
      //  new BioInfo("Military",
      //    "branch, years of service, active duty experience, highest rank, medals, honors, type and date of discharge"),
      //  new BioInfo("Civic",
      //    "past and present organizations, charities involvement"),
      //  new BioInfo("Political",
      //    "dates and titles of previously held political offices"),
      //  new BioInfo("Religion", "current and past religious affiliations, beliefs"),
      //  new BioInfo("Accomplishments",
      //    "significant accomplishments, awards, achievements")
      //};
    }

    protected override void OnBeginState(string electionKey, string stateName, int officeCount)
    {
      ExtractReferendums(electionKey);
      if (officeCount > 0) BeginJurisdictionLevel(stateName, "StateTab");
      else ReportReferendums(electionKey);
    }

    protected override void OnEndCounty(string electionKey)
    {
      ReportReferendums(electionKey);
    }

    protected override void OnEndLocal(string electionKey)
    {
      ReportReferendums(electionKey);
    }

    protected override void OnEndLocals()
    {
      _MultiLocalMode = false;
    }

    protected override void OnEndOffice(string electionKey, DataRow officeInfo)
    {
      var isFirstEnabled = true;
      new HtmlDiv().AddTo(_CurrentOfficeInfoContent, "clear-both"); 
      foreach (var info in _BioInfos)
      {
        info.Container.AddCssClasses("hidden");
        if (info.HasValue)
        {
          info.BioBarItem.Attribute("title", info.ToolTip);
          if (isFirstEnabled)
          {
            isFirstEnabled = false;
            info.Container.RemoveCssClass("hidden");
            info.BioBarItem.AddCssClasses("selected");
          }
        }
        else
        {
          info.BioBarItem.AddCssClasses("disabled");
          info.BioBarItem.Attribute("title",
            "We have no responses for " + info.Label +
              " from the candidates for this office");
        }
      }
      if (_OfficeIssues.Count > 0)
        _IssuesBioBarItem.Attribute("title",
          "Click to compare the candidates' statements on the issues");
      else
      {
        _IssuesBioBarItem.AddCssClasses("disabled");
        _IssuesBioBarItem.Attribute("title",
          "We have no responses for nay of the issues from the candidates" +
            " for this office");
      }
    }

    protected override Control OnEndReport(out int ballotMeasures)
    {
      ballotMeasures = _Referendums.Rows.Count;
      return _ReportPlaceHolder;
    }

    protected override void OnEndState(string electionKey)
    {
      ReportReferendums(electionKey);
    }

    protected override void OnReportCandidate(DataRow candidate)
    {
      var div = new HtmlDiv().AddTo(_CurrentOfficeInfoContent,
        "rounded-border candidate");
      ReportCandidate(candidate).AddTo(div);

      DataRow runningMate = null;
      if (candidate.IsRunningMateOffice())
      {
        runningMate = GetRunningMate(candidate.OfficeKey(),
          candidate.RunningMateKey());
        if (runningMate != null)
          ReportCandidate(runningMate).AddTo(div);
      }

      // add bio info
      foreach (var bioInfo in _BioInfos)
      {
        AddBioInfo(candidate, bioInfo);
        if (candidate.IsRunningMateOffice())
          AddBioInfo(runningMate, bioInfo);
      }

      // add answers
      foreach (var issue in _OfficeIssues)
        foreach (var question in issue)
        {
          AddIssue(candidate, question);
          if (candidate.IsRunningMateOffice())
            AddIssue(runningMate, question); 
        }
    }

    #endregion Event handlers and overrides

    public static Control GetReport(string electionKey, string congress, 
      string stateSenate, string stateHouse, string countyCode, 
      out int officeContests, out int ballotMeasures)
    {
      var reportObject = new BallotReport2Tabbed();
      return reportObject.GenerateReport(electionKey, congress, stateSenate,
        stateHouse, countyCode, out officeContests, out ballotMeasures);
    }

    //public static Control GetReport(string electionKey, string congress,
    //  string stateSenate, string stateHouse, string countyCode)
    //{
    //  int officeContests;
    //  int ballotMeasures;
    //  return GetReport(electionKey, congress, stateSenate, stateHouse, countyCode,
    //    out officeContests, out ballotMeasures);
    //}
  }
}