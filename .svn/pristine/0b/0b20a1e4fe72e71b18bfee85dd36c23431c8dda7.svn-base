using System;

namespace Vote
{
  public partial class VotersPage : PublicPage
  {
    protected VotersPage()
    {
      NoUrlEdit = true;
    }

    private void Page_Load(object sender, EventArgs e)
    {
      Response.RedirectPermanent(
        UrlManager.GetForVotersPageUri(DomainData.FromQueryStringOrDomain)
          .ToString(), true);
    }
  }
}