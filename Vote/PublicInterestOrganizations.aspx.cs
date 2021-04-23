using System;
using static System.String;

namespace Vote
{
  public partial class PublicInterestOrganizations : PublicPage
  {
    private const string TitleTag =
      "{0} | Public Interest Organizations";

    private const string MetaDescriptionAllStatesDomain =
      "Public Interest Organizations";

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      MetaDescription = MetaDescriptionAllStatesDomain.Substitute();

      EmailForm.ToEmailAddress = "john.fleisch@vote-usa.org";
      EmailForm.SetItems("We are interested in working with Vote-USA", 
        "We would like our organization included on this page", 
        "We are interested in ways we could promote candidates on your website", 
        "We are interested in acquiring some of your candidate and/or election data");
    }
  }
}