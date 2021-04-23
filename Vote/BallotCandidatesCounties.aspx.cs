using System;

namespace Vote
{
  public partial class BallotCandidatesCountiesPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // Obsolete page
      string electionKey = db.ElectionKey_New_Format(VotePage.QueryElection);
      Response.RedirectPermanent(UrlManager.GetElectionPageUri(electionKey).ToString());
    }
  }
}