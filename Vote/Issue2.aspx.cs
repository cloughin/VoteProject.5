using System;

namespace Vote
{
  public partial class Issue2Page : PublicPage
  {
    protected Issue2Page()
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