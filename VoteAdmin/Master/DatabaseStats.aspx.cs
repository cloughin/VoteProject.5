using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote;

namespace VoteAdmin
{
  public partial class DatabaseStats : SecurePage, ISuperUser
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPoliticianUser)
      {
        Page.Title = H1.InnerHtml = "Database Statistics";
        CandidatesCount.InnerText = Politicians.CountTable().ToString("N0");
        CandidatePicturesCount.InnerText =
          PoliticiansImagesData.CountTable().ToString("N0");
        OfficesCount.InnerText = Offices.CountAllActualOffices().ToString("N0");

        var officeCandidates = ElectionsPoliticians.GetOfficeCandidatesByYear();
        OfficeCandidatesCount.InnerText = officeCandidates.Sum(y => y.Value).ToString("N0");
        var officeCandidatesRowIndex =
          OfficeCandidatesRow.Parent.Controls.IndexOf(OfficeCandidatesRow);
        foreach (var kvp in officeCandidates.OrderBy(i => i.Key))
        {
          var newRow = new HtmlTableRow();
          newRow.AddCssClasses("office-candidates-year");
          newRow.AddCssClasses(OfficeCandidatesRow.Attributes["class"].SafeString());
          OfficeCandidatesRow.Parent.Controls.AddAt(++officeCandidatesRowIndex, newRow);
          new Literal
          {
            Text = kvp.Key
          }.AddTo(new HtmlTableCell().AddTo(newRow, "year"));
          new Literal
          {
            Text = kvp.Value.ToString("N0")
          }.AddTo(new HtmlTableCell().AddTo(newRow, "value"));
        }

        var officeContests = ElectionsOffices.GetOfficeContestsByYear();
        OfficeContestCount.InnerText = officeContests.Sum(y => y.Value).ToString("N0");
        var officeContestsRowIndex =
          OfficeContestsRow.Parent.Controls.IndexOf(OfficeContestsRow);
        foreach (var kvp in officeContests.OrderBy(i => i.Key))
        {
          var newRow = new HtmlTableRow();
          newRow.AddCssClasses("office-contests-year");
          newRow.AddCssClasses(OfficeContestsRow.Attributes["class"].SafeString());
          OfficeContestsRow.Parent.Controls.AddAt(++officeContestsRowIndex, newRow);
          new Literal
          {
            Text = kvp.Key
          }.AddTo(new HtmlTableCell().AddTo(newRow, "year"));
          new Literal
          {
            Text = kvp.Value.ToString("N0")
          }.AddTo(new HtmlTableCell().AddTo(newRow, "value"));
        }

        var electionsCount = Elections.CountStateElectionsByYear();
        ElectionsCount.InnerText = electionsCount.Sum(y => y.Value).ToString("N0");
        var electionsRowIndex =
          ElectionsRow.Parent.Controls.IndexOf(ElectionsRow);
        foreach (var kvp in electionsCount.OrderBy(i => i.Key))
        {
          var newRow = new HtmlTableRow();
          newRow.AddCssClasses("elections-year");
          newRow.AddCssClasses(ElectionsRow.Attributes["class"].SafeString());
          ElectionsRow.Parent.Controls.AddAt(++electionsRowIndex, newRow);
          new Literal
          {
            Text = kvp.Key
          }.AddTo(new HtmlTableCell().AddTo(newRow, "year"));
          new Literal
          {
            Text = kvp.Value.ToString("N0")
          }.AddTo(new HtmlTableCell().AddTo(newRow, "value"));
        }

        SocialMediaLinksCount.InnerText =
          Politicians.CountAllSocialMediaLinks().ToString("N0");
        BioInfoCount.InnerText = Answers.CountBioAnswersNew().ToString("N0");
        PersonalCount.InnerText = Answers.CountPersonalAnswersNew().ToString("N0");
        IssueResponsesCount.InnerText = Answers.CountActiveIssueAnswersNew().ToString("N0");
      }
    }
  }
}