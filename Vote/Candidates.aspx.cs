namespace Vote
{
  public partial class CandidatesPage : PublicPage
  {
    public CandidatesPage()
    {
      NoUrlEdit = true;
    }

    private void Page_Load(object sender, System.EventArgs e)
    {
      Response.RedirectPermanent(
        UrlManager.GetForCandidatesPageUri(
         DomainData.FromQueryStringOrDomain).ToString(), true);
    }
  }
}
