namespace Vote
{
  public partial class ArchivesPage : PublicPage
  {
    public ArchivesPage()
    {
      NoUrlEdit = true;
    }

    private void Page_Load(object sender, System.EventArgs e)
    {
      Response.RedirectPermanent(
        UrlManager.GetForResearchPageUri(
         DomainData.FromQueryStringOrDomain).ToString(), true);
    }
  }
}
