using System;
using System.Data;
using System.Linq;
using System.Web.ModelBinding;
using DB;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateIssuesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    public static void ConsolidateTopics(int toId, int fromId)
    {
      var politicians = Answers.GetAnswersForConsolidation(toId, fromId).Rows.OfType<DataRow>()
        .GroupBy(r => r.PoliticianKey(), StringComparer.OrdinalIgnoreCase);
      foreach (var politician in politicians)
      {
        // delete old
        Answers2.DeleteByPoliticianKeyQuestionId(politician.Key, toId);
        Answers2.DeleteByPoliticianKeyQuestionId(politician.Key, fromId);

        // insert new
        var answers = politician.OrderBy(r =>
          IsNullOrWhiteSpace(r.YouTubeUrl()) ? r.AnswerDate() : r.YouTubeDate());
        var sequence = 0;
        foreach (var a in answers)
          Answers2.Insert(politician.Key, toId, sequence++, a.Answer(), a.Source(), a.DateStamp(),
            a.UserName(), a.YouTubeUrl(), a.YouTubeDescription(), a.YouTubeRunningTime(),
            a.YouTubeSource(), a.YouTubeSourceUrl(), a.YouTubeDate(), a.YouTubeRefreshTime(),
            a.YouTubeAutoDisable(), a.FacebookVideoUrl(), a.FacebookVideoDescription(),
            a.FacebookVideoRunningTime(), a.FacebookVideoDate(), a.FacebookVideoRefreshTime(),
            a.FacebookVideoAutoDisable());
      }

      // delete the topic
      Questions2.DeleteByQuestionId(fromId);
      IssuesQuestions.DeleteByQuestionId(fromId);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        const string title = "Update Issues";
        Page.Title = title;
        H1.InnerHtml = title;
        SelectJurisdictions.Initialize();
      }
    }
  }
}