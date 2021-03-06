using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using static System.String;

namespace Vote.Politician
{
  public partial class UpdateIntroPage
  {
    #region Private

    private static readonly Dictionary<string, string> AlternateHeadings =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
      {
        {"ALLBio111111", "Statement of goals, objectives, views, philosophies"},
        {
          "ALLBio222222", "Age, marital status, spouse and children's names and ages,<br />" +
          "hometown, current residence, family background"
        },
        {"ALLBio333333", "Occupation or profession and work experience outside politics"},
        {"ALLBio444444", "Past and present organizations, involvement with charities"},
        {
          "ALLBio555555", "Dates and titles of previously held political offices,<br />" +
          "sponsored and co-sponsored legislation"
        },
        {"ALLBio666666", "Current and past religious affiliations and beliefs"},
        {"ALLBio777777", "Significant accomplishments, awards, achievements"},
        {
          "ALLBio888888", "Dates and locations of schools, colleges, major,<br />" +
          "degrees, activities, athletics"
        },
        {
          "ALLBio999999", "Branch, years of service, active duty experience, highest rank,<br />" +
          "medals, honors, discharge date and type"
        }
      };

    #region DataItem object

    // ReSharper disable UnusedMember.Local
    // No updating here, only to provide initialization services
    [PageInitializer]
    private class Bio2TabItem : PoliticianDataItem
      // ReSharper restore UnusedMember.Local
    {
      private const string GroupName = "Bio2";

      public Bio2TabItem(UpdateIntroPage page, string groupName)
        : base(page, groupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateIntroPage page)
        // ReSharper restore UnusedMember.Local
      {
        page.BuildBio2Tab();
        page.RegisterUpdateAll(page.UpdateAllBio2);
        new MainTabItem {TabName = GroupName}.Initialize(page);
        if (!page.IsPostBack)
        {
          page.LoadBio2TabData();
        }
      }
    }

    #endregion DataItem object

    #region Private Members to Build the Bio2 tab

    private Font _QuestionTabFont;

    private readonly UpdateAnswer _UpdateAnswerBio2 =
      new UpdateAnswer(UpdateAnswer.Usage.ForIntroBio);

    // specs for breaking the vertical tabs
    private const string QuestionTabFontFamily = "Arial";
    private const float QuestionTabFontSize = 8.0F; // 8pt
    private const int QuestionTabMaxWidth = 120; // 120px

    private void BuildBio2Tab()
    {
      var table = _UpdateAnswerBio2.GetDataTable();

      var questions = table
        .OrderBy(g => g.First().QuestionOrder());

      var alternatePoliticianLabels = AlternatePoliticianTabLabels2;

      ProcessQuestions(PlaceHolderBio2, "bio2", questions,
        _UpdateAnswerBio2, alternatePoliticianLabels, AlternateHeadings);
    }

    private void CreateQuestion(Control parent, IList<DataRow> responses,
      Control tabs, string tabName, UpdateAnswer updateAnswer,
      IDictionary<string, string> alternateTabLabels = null,
      Dictionary<string, string> alternateHeadings = null)
    {
      var row = responses.First();

      if (_QuestionTabFont == null)
        _QuestionTabFont = new Font(QuestionTabFontFamily, QuestionTabFontSize,
          FontStyle.Bold);

      var monitor = MonitorFactory.GetMonitorInstance(row.QuestionKey());

      var panel = AddContainer(parent, "tab-" + tabName + "-" + row.QuestionKey().ToLowerInvariant(),
        "content-panel tab-panel vtab-panel");
      panel.ClientIDMode = ClientIDMode.Static;

      // create vertical tab
      var tabLabel = alternateTabLabels?.ContainsKey(row.QuestionKey()) == true
        ? alternateTabLabels[row.QuestionKey()]
        : BreakForTab(row.Question(), _QuestionTabFont, QuestionTabMaxWidth);
      var verticalTab = AddVertTab(tabs, "#" + panel.ClientID, tabLabel, " vcentered-tab");

      // center the a tag -- the first child of the tab always
      Center(verticalTab.Controls[0], false, true);

      AddAsteriskIndicator(verticalTab, "Ast" + row.QuestionKey(),
        monitor.GetAsteriskClass(null),
        $"There are unsaved changes to \"{row.Question()}\"");

      AddStarIndicator(verticalTab, "Star" + row.QuestionKey(),
        monitor.GetStarClass(null, "hasvalue"),
        $"We have a response from you for \"{row.Question()}\"");

      updateAnswer.CreateControls(panel, responses, monitor, alternateHeadings);
    }

    // shared by Reasons tab
    private void ProcessQuestions(Control parent, string tabName,
      IEnumerable<IGrouping<string, DataRow>> questions,
      UpdateAnswer updateAnswer,
      IDictionary<string, string> alternateTabLabels = null,
      Dictionary<string, string> alternateHeadings = null)
    {
      var tabs = AddTabContainer(parent, null, "tabs vtabs unselectable");

      foreach (var question in questions)
        CreateQuestion(parent, question.ToList(), tabs, tabName, updateAnswer,
          alternateTabLabels, alternateHeadings);
    }

    #endregion Private Members to Build the Bio2 tab

    private void LoadBio2TabData()
    {
      var table = _UpdateAnswerBio2.GetDataTable();
      foreach (var row in table.Select(g => g.First()))
        if (!IsNullOrWhiteSpace(row.Answer()))
        {
          if (Master.FindMainContentControl("TextBox" + row.QuestionKey()) is TextBox textbox)
            textbox.Text = row.Answer().Trim();
        }
    }

    private int UpdateAllBio2(bool showSummary)
    {
      return _UpdateAnswerBio2.UpdateAllAnswers(showSummary, ref _UpdateCount);
    }

    #endregion Private
  }
}