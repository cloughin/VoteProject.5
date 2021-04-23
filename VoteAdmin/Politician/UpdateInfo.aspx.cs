using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin.Politician
{
  public partial class UpdateInfoPage : SecurePoliticianPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Master.NoHeading = true;
      MainBannerHomeLink.HRef = UrlManager.GetSiteUri().ToString();
      Title = "Vote-USA - Manage Issue Responses";

      if (UserSecurityClass != PoliticianSecurityClass)
      {
        var mainContent = Master.MainContentControl;
        mainContent.Controls.Clear();
        new HtmlP
        {
          InnerText = "This page is only available to signed-in candidates"
        }.AddTo(mainContent, "user-error");
        return;
      }

      PoliticianName.InnerText = base.PoliticianName;
      var issuesAndTopics = Questions2.GetIssuesAndTopics();

      // prune to match the politician
      var liveOfficeKey = Politicians.GetLiveOfficeKey(PoliticianKey, Empty);
      var electoralClass = Offices.GetElectoralClass(Offices.GetOfficeClass(liveOfficeKey));
      var stateCode = Offices.GetStateCodeFromKey(liveOfficeKey);
      var issueLevel = Empty;
      var countyOrLocal = Empty;
      switch (electoralClass)
      {
        case ElectoralClass.USPresident:
        case ElectoralClass.USSenate:
        case ElectoralClass.USHouse:
          issueLevel = "B";
          break;

        case ElectoralClass.USGovernors:
        case ElectoralClass.State:
          issueLevel = "C";
          break;

        case ElectoralClass.County:
          issueLevel = "D";
          countyOrLocal = Offices.GetCountyCodeFromKey(liveOfficeKey);
          break;

        case ElectoralClass.Local:
          issueLevel = "E";
          countyOrLocal = Offices.GetLocalKeyFromKey(liveOfficeKey);
          break;
      }

      foreach (var issue in issuesAndTopics)
      {
        foreach (var topic in issue.Questions)
        {
          if (topic.Jurisdictions.Any(j => j.IssueLevel == "A"))
            topic.Pruned = false;
          else
            switch (issueLevel)
            {
              case "B":
                topic.Pruned = topic.Jurisdictions.All(j => j.IssueLevel != "B");
                break;

              case "C":
                topic.Pruned = !topic.Jurisdictions.Any(j => j.IssueLevel == "C" &&
                  (j.StateCode == Empty || j.StateCode == stateCode));
                break;

              case "D":
                topic.Pruned = !topic.Jurisdictions.Any(j => j.IssueLevel == "D" &&
                  (j.StateCode == Empty || j.StateCode == stateCode &&
                    (j.CountyOrLocal == Empty || j.CountyOrLocal == countyOrLocal)));
                break;

              case "E":
                topic.Pruned = !topic.Jurisdictions.Any(j => j.IssueLevel == "E" &&
                  (j.StateCode == Empty || j.StateCode == stateCode &&
                    (j.CountyOrLocal == Empty || j.CountyOrLocal == countyOrLocal)));
                break;

              default:
                topic.Pruned = true;
                break;
            }
        }

        issue.Pruned = issue.Questions.All(q => q.Pruned);
      }

      var prunedIssuesAndTopics = issuesAndTopics.Where(i => !i.Pruned)
        .Select(i => new IssuesAndTopicsPrunedIssue
        {
          I = i.IssueId,
          Issue = i.Issue,
          Q = i.Questions.Where(q => !q.Pruned).Select(q => new IssuesAndTopicsPrunedQuestion
          {
            I = q.QuestionId,
            Q = q.Question
          }).ToList()
        }).ToList();


      // load the topics
      var options = prunedIssuesAndTopics.Select(i =>
          $"<option value=\"{i.I}\">{i.Issue}</option>");
      IssuesSelectOptions.Text = "<option value=\"\">&lt; select an issue &gt;</option>" +
        Join(Empty, options);

      // second pruning to reduce bandwidth
      var rePrunedIssuesAndTopics = prunedIssuesAndTopics
        .Select(i => new IssuesAndTopicsRePrunedIssue
        {
          I = i.I,
          Q = i.Q
        }).ToList();

      // make data available client side
      var body = Master.FindControl("body") as HtmlGenericControl;
      var json = new JavaScriptSerializer();
      // ReSharper disable once PossibleNullReferenceException
      body.Attributes.Add("data-issues-and-topics", json.Serialize(rePrunedIssuesAndTopics));
      body.Attributes.Add("data-politician-key", PoliticianKey);
    }
  }
}
