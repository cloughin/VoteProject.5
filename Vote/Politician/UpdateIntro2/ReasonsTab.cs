using System.Linq;
using System.Web.UI.WebControls;

namespace Vote.Politician
{
  public partial class UpdateIntro2Page
  {
    #region Private

    #region DataItem object

    // ReSharper disable UnusedMember.Local
    // No updating here, only to provide initialization services
    [PageInitializer]
    private class ReasonsTabItem : PoliticianDataItem
      // ReSharper restore UnusedMember.Local
    {
      private const string GroupName = "Reasons";

      public ReasonsTabItem(UpdateIntro2Page page, string groupName)
        : base(page, groupName) {}

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateIntro2Page page)
        // ReSharper restore UnusedMember.Local
      {
        page.BuildReasonsAndObjectivesTab();
        page.RegisterUpdateAll(page.UpdateAllReasons);
        new MainTabItem { TabName = GroupName }.Initialize(page);
        if (!page.IsPostBack)
        {
          page.LoadReasonsTabData();
        }
      }
    }

    #endregion DataItem object

    #region Private Members to Build the Reasons and Objectives tab

    //private Font _QuestionTabFont;
    private readonly UpdateAnswer _UpdateAnswer = new UpdateAnswer(UpdateAnswer.Usage.ForIntroReasons);

    // specs for breaking the vertical tabs
    //private const string QuestionTabFontFamily = "Arial";
    //private const float QuestionTabFontSize = 8.0F; // 8pt
    //private const int QuestionTabMaxWidth = 120; // 120px

    private void BuildReasonsAndObjectivesTab()
    {
      var table = _UpdateAnswer.GetDataTable();

      var questions = table
        .OrderBy(g => g.First().QuestionOrder);

      ProcessQuestions(PlaceHolderReasons, "reasons", questions, _UpdateAnswer);
      //ProcessQuestions(questions);
    }

    //private void CreateQuestion(IList<AnswersViewRow> responses, Control tabs)
    //{
    //  var row = responses.First();

    //  if (_QuestionTabFont == null)
    //    // We use bold as worst-case
    //    _QuestionTabFont = new Font(QuestionTabFontFamily, QuestionTabFontSize,
    //      FontStyle.Bold);

    //  var monitor = MonitorFactory.GetMonitorInstance(row.QuestionKey);

    //  var panel = AddContainer(PlaceHolderReasons, "tab-reasons-" + row.QuestionKey,
    //    "content-panel tab-panel vtab-panel");

    //  // create vertical tab
    //  var verticalTab = AddVertTab(tabs, "#" + panel.ClientID,
    //    BreakForTab(row.Question, _QuestionTabFont, QuestionTabMaxWidth), " vcentered-tab");

    //  // center the a tag -- the first child of the tab always
    //  Center(verticalTab.Controls[0], false, true);

    //  AddAsteriskIndicator(verticalTab, "Ast" + row.QuestionKey,
    //    monitor.GetAsteriskClass(null),
    //    string.Format("There are unsaved changes to \"{0}\"", row.Question));

    //  AddStarIndicator(verticalTab, "Star" + row.QuestionKey,
    //    monitor.GetStarClass(null, "answer"),
    //    string.Format("We have a response from you for \"{0}\"", row.Question));

    //  _UpdateAnswer.CreateControls(panel, responses, monitor);
    //}

    //private void ProcessQuestions(IEnumerable<IGrouping<string, AnswersViewRow>> questions)
    //{
    //  var tabs = AddTabContainer(PlaceHolderReasons, null, "tabs vtabs unselectable");

    //  foreach (var question in questions)
    //    CreateQuestion(question.ToList(), tabs);
    //}

    #endregion Private Members to Build the Reasons and Objectives tab

    private void LoadReasonsTabData()
    {
      var table = _UpdateAnswer.GetDataTable();
      foreach (var row in table.Select(g => g.First()))
        if (!string.IsNullOrWhiteSpace(row.Answer))
        {
          var textbox =
            Master.FindMainContentControl("TextBox" + row.QuestionKey) as TextBox;
          if (textbox != null)
            textbox.Text = row.Answer.Trim();
        }
    }

    private int UpdateAllReasons(bool showSummary)
    {
      return _UpdateAnswer.UpdateAllAnswers(showSummary, ref _UpdateCount);
    }

    #endregion Private
  }
}