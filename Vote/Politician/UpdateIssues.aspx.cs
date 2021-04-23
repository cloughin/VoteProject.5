using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Politician
{
  public partial class UpdateIssuesPage : SecurePoliticianPage
  {
    #region Private Members

    private int _UpdateCount;
    private readonly UpdateAnswer _UpdateAnswer = new UpdateAnswer(UpdateAnswer.Usage.ForIssues);

    private const string MonitorPrefix = "mc";
    private const string MonitorTabPrefix = "mt";
    private const string MonitorSuperTabPrefix = "ms";

    private readonly MonitorFactory _MonitorFactory =
      new MonitorFactory(MonitorPrefix);

    private const int LinesInCategoryHeading = 1;
    private const int LinesInCategorySubHeading = 3;

    // for testing
    private const int MaxCategories = int.MaxValue;
    private const int MaxIssuesPerCategory = int.MaxValue;
    private const int MaxQuestionsPerIssue = int.MaxValue;

    #endregion Private Members

    #region Private Members to Build the Tabs

    private Font _IssueTabFont;

    // specs for breaking the vertical tabs
    private const string IssueTabFontFamily = "Arial";
    private const float IssueTabFontSize = 8.0F; // 8pt
    private const int IssueTabMaxWidth = 120; // 120px

    private void BuildTabTable()
    {
      var table = _UpdateAnswer.GetDataTable();
      var issueGroups = table
        .GroupBy(g => g.First().IssueGroupKey)
        .OrderBy(group => group.First().First().IssueGroupOrder);
      ProcessCategories(issueGroups);
    }

    private HtmlContainerControl CreateCategory(AnswersViewRow row,
      Control horizontalTabs)
    {
      // create panel
      var panel = AddContainer(MainTabsPlaceholder, "tab-" + row.IssueGroupKey,
        "content-panel tab-panel htab-panel");

      // create vertical tab control
      var verticalTabs = AddVertTabControl(panel, null, "tabs-nested");

      // create horizontal tab
      var superTabKey = MonitorSuperTabPrefix + "-igroup" + row.IssueGroupKey;
      var horizontalTab = AddHorzTab(horizontalTabs, "#" + panel.ClientID,
        GetMainTabLabel(row), superTabKey);

      AddAsteriskIndicator(horizontalTab, null, null,
        $"There are unsaved changes to the {row.IssueGroupHeading} category");

      AddStarIndicator(horizontalTab, null, null,
        $"We have at least one response from you for the {row.IssueGroupHeading} category");

      return verticalTabs;
    }

    private HtmlContainerControl CreateIssue(AnswersViewRow row, Control tabs,
      Control container)
    {
      if (_IssueTabFont == null)
        // We use bold as worst-case
        _IssueTabFont = new Font(IssueTabFontFamily, IssueTabFontSize,
          FontStyle.Bold);

      // create panel
      var panel = AddContainer(container, "tab-" + row.IssueKey,
        "content-panel tab-panel vtab-panel");

      // create accordion control
      var accordion = AddAccordionControl(panel, null, null, "accordion-deferred");

      // create vertical tab
      var tabKey = MonitorTabPrefix + "-issue" + row.IssueKey;
      var verticalTab = AddVertTab(tabs, "#" + panel.ClientID,
        BreakForTab(row.Issue, _IssueTabFont, IssueTabMaxWidth), tabKey + " vcentered-tab");

      // center the a tag -- the first child of the tab always
      Center(verticalTab.Controls[0], false, true);

      AddAsteriskIndicator(verticalTab, null, null,
        $"There are unsaved changes to {row.Issue}");

      AddStarIndicator(verticalTab, null, null,
        $"We have at least one response from you for {row.Issue}");

      return accordion;
    }

    private void CreateQuestion(IList<AnswersViewRow> responses, Control parent)
    {
      var row = responses.First();
      var monitor = _MonitorFactory.GetMonitorInstance(row.QuestionKey);

      // create the accordion header
      var accordionHeader = AddAccordionHeader(parent, row.Question);

      // create the accordion panel
      var panel = AddContainer(parent, "tab-answer-" + row.QuestionKey,
        "content-panel accordion-panel");

      AddAsteriskIndicator(accordionHeader, "Ast" + row.QuestionKey,
        monitor.GetAsteriskClass(null),
        $"There are unsaved changes to \"{row.Question}\"");

      AddStarIndicator(accordionHeader, "Star" + row.QuestionKey,
        monitor.GetStarClass(null, "hasvalue"),
        $"We have a response from you for \"{row.Question}\"");

      _UpdateAnswer.CreateControls(panel, responses, monitor);
    }

    private static PlaceHolder GetMainTabLabel(AnswersViewRow row)
    {
      // The database entry has pre-defined breaks. Here we verify the count and
      // pad with blank lines at the end, if necessary

      var container = new PlaceHolder {EnableViewState = false};

      var formattedHeading = ValidateBreaks(row.IssueGroupHeading,
        LinesInCategoryHeading, true,
        "Too many lines in category heading: " + row.IssueGroupKey);
      new LiteralControl(formattedHeading) {EnableViewState = false}.AddTo(
        container);

      var formattedSubHeading = ValidateBreaks(row.IssueGroupSubHeading,
        LinesInCategorySubHeading, true,
        "Too many lines in category subheading: " + row.IssueGroupKey);
      new HtmlSpan
      {
        EnableViewState = false,
        InnerHtml = formattedSubHeading
      }.AddTo(container);

      return container;
    }

    private void ProcessCategories(
      IEnumerable<IGrouping<string, IGrouping<string, AnswersViewRow>>> categories)
    {
      var horizontalTabs = AddTabContainer(MainTabsPlaceholder, null, "tabs htabs unselectable");

      foreach (var category in categories)
      {
        var first = category.First();
        var tabs = CreateCategory(first.First(), horizontalTabs);

        var issues = category.GroupBy(g => g.First().IssueKey)
          .OrderBy(group => group.First().First().IssueOrder);
        ProcessIssues(issues, tabs);
        if (horizontalTabs.Controls.Count >= MaxCategories) return;
      }
    }

    private void ProcessIssues(
      IEnumerable<IGrouping<string, IGrouping<string, AnswersViewRow>>> issues, Control container)
    {
      var verticalTabs = AddTabContainer(container, null, "tabs vtabs unselectable");

      foreach (var issue in issues)
      {
        var first = issue.First();
        var accordion = CreateIssue(first.First(), verticalTabs, container);

        var questions = issue.OrderBy(g => g.First().QuestionOrder);
        ProcessQuestions(questions, accordion);
        if (verticalTabs.Controls.Count >= MaxIssuesPerCategory) return;
      }
    }

    private void ProcessQuestions(IEnumerable<IGrouping<string, AnswersViewRow>> questions,
      Control container)
    {
      foreach (var question in questions)
      {
        CreateQuestion(question.ToList(), container);
        if (container.Controls.Count / 2 >= MaxQuestionsPerIssue) return;
      }
    }

    private static string ValidateBreaks(string input, int linesExpected, bool pad,
      string errorMessage)
    {
      ICollection<string> lines = input.SplitOnBreakTags();
      if (lines.Count > linesExpected)
      {
        if (string.IsNullOrWhiteSpace(errorMessage))
          errorMessage = "More lines than expected";
        throw new ApplicationException(errorMessage);
      }
      if (pad && (lines.Count < linesExpected))
      {
        lines = new List<string>(lines);
        while (linesExpected-- > lines.Count)
          lines.Add("&nbsp;");
      }
      return string.Join("<br />", lines);
    }

    #endregion Private Members to Build the Tabs

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
      BuildTabTable();

      if (!IsPostBack)
      {
        Page.Title = PoliticianName + " Update Issues";
        H1.InnerHtml = PoliticianName + "<span>Update Issues</span>";
        H2.InnerHtml = OfficeAndStatus;
        UpdateIntroLink.Attributes["href"] = UpdateIntroPageUrl;
        ShowIntroLink.Attributes["href"] = IntroPageUrl;
        ShowPoliticianIssueLink.Attributes["href"] = PoliticianIssuePageUrl;
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // This isn't available at Page_Load time
      var viewIssuesLink =
        PageCachingSubHeadingWithHelp.FindTemplateControl("ViewIssuesLink") as
          HtmlAnchor;
      if (viewIssuesLink != null) viewIssuesLink.HRef = PoliticianIssuePageUrl;

      // We handle post-update processing here so we only have to do it once per postback,
      // not on each field.
      if (_UpdateCount <= 0) return;
      Politicians.IncrementDataUpdatedCount(PoliticianKey);
    }

    protected void ButtonUpdateAll_OnClick(object sender, EventArgs e)
    {
      try
      {
        var errorCount = _UpdateAnswer.UpdateAllAnswers(false, ref _UpdateCount);

        FeedbackUpdateAll.AddInfo(
          _UpdateCount.ToString(CultureInfo.InvariantCulture) +
          _UpdateCount.Plural(" item was", " items were") + " updated.");
        if (errorCount > 0)
          FeedbackUpdateAll.AddError(
            errorCount.ToString(CultureInfo.InvariantCulture) +
            errorCount.Plural(" item was", " items were") +
            " not updated due to errors.");

        InvalidatePageCache();
      }
      catch (Exception ex)
      {
        FeedbackUpdateAll.HandleException(ex);
      }
    }

    #endregion Event Handlers
  }
}