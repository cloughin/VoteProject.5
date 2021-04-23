using System;

namespace Vote
{
  public partial class ForPartnersPage : PublicPage // not CacheablePage due to Captcha
  {
    protected override void OnPreInit(EventArgs e)
    {
      Response.Redirect(UrlManager.GetSiteUri().ToString());
      Response.End();
    }

    private const string TitleTagAllStatesDomain =
      "Partnering with Political Organizations, Associations and News Media";

    private const string MetaDescriptionAllStatesDomain =
      "Partnering with politically engaged organizations, political parties, associations and news media to share candidate and election information.";

    private const string TitleTagSingleStateDomain =
      "Partnering with [[state]] Political Organizations, Associations and News Media";

    private const string MetaDescriptionSingleStateDomain =
      "Partnering with [[state]]’s politically engaged organizations, political parties, associations and news media to share candidate and election information.";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (DomainData.IsValidStateCode) // Single state
      {
        Title = $"{PublicMasterPage.SiteName} | {TitleTagSingleStateDomain.Substitute()}";
        MetaDescription = MetaDescriptionSingleStateDomain.Substitute();
      }
      else
      {
        Title = $"{PublicMasterPage.SiteName} | {TitleTagAllStatesDomain.Substitute()}";
        MetaDescription = MetaDescriptionAllStatesDomain.Substitute();
      }

      EmailForm.ToEmailAddress = "john.fleisch@vote-usa.org";
      EmailForm.SetItems("We are interested in partnering with Vote-USA");
    }
  }
}