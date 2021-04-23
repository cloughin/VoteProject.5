using System.Linq;
using System.Web.UI.WebControls;
using DB;
using static System.String;

namespace Vote.Politician
{
  public partial class UpdateIntroPage
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

      public ReasonsTabItem(UpdateIntroPage page, string groupName)
        : base(page, groupName)
      {
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateIntroPage page)
        // ReSharper restore UnusedMember.Local
      {
        page.BuildReasonsAndObjectivesTab();
        page.RegisterUpdateAll(page.UpdateAllReasons);
        new MainTabItem {TabName = GroupName}.Initialize(page);
        if (!page.IsPostBack)
        {
          page.LoadReasonsTabData();
        }
      }
    }

    #endregion DataItem object

    #region Private Members to Build the Reasons and Objectives tab

    private readonly UpdateAnswer _UpdateAnswer =
      new UpdateAnswer(UpdateAnswer.Usage.ForIntroReasons);

    private void BuildReasonsAndObjectivesTab()
    {
      var table = _UpdateAnswer.GetDataTable();

      var questions = table
        .OrderBy(g => g.First().QuestionOrder());

      ProcessQuestions(PlaceHolderReasons, "reasons", questions, _UpdateAnswer);
    }

    #endregion Private Members to Build the Reasons and Objectives tab

    private void LoadReasonsTabData()
    {
      var table = _UpdateAnswer.GetDataTable();
      foreach (var row in table.Select(g => g.First()))
        if (!IsNullOrWhiteSpace(row.Answer()))
        {
          if (Master.FindMainContentControl("TextBox" + row.QuestionKey()) is TextBox textbox)
            textbox.Text = row.Answer().Trim();
        }
    }

    private int UpdateAllReasons(bool showSummary)
    {
      return _UpdateAnswer.UpdateAllAnswers(showSummary, ref _UpdateCount);
    }

    #endregion Private
  }
}