namespace Vote
{
  public partial class InternsPage : PublicPage
  {
    public InternsPage()
    {
      NoUrlEdit = true;
    }

    private void Page_Load(object sender, System.EventArgs e)
    {
      Response.RedirectPermanent(
        UrlManager.GetForVolunteersPageUri(
         DomainData.FromQueryStringOrDomain).ToString(), true);
    }
  }
}
