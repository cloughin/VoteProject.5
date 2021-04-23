using System;

namespace Vote
{
  public partial class IssuePage : PublicPage
  {
    protected IssuePage()
    {
      NoUrlEdit = true;
    }

   protected void Page_Load(object sender, EventArgs e)
    {
      Response.RedirectPermanent(
        UrlManager.GetCompareCandidatesPageUri(DomainData.FromQueryStringOrDomain,
         QueryElection, QueryOffice)
          .ToString(), true);
    }
  }
}