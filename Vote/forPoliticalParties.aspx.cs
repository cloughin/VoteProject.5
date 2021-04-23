using System;

namespace Vote
{
  public partial class ForPoliticalPartiesPage : PublicPage // not CacheablePage due to Captcha
  {
    private const string TitleTagAllStatesDomain =
      "Free Promotion of Political Party Candidates";

    private const string MetaDescriptionAllStatesDomain =
      "Helping political parties to promote their candidates in elections, free of charge.";

    private const string TitleTagSingleStateDomain =
      "Free Promotion of [[state]] Political Party Candidates";

    private const string MetaDescriptionSingleStateDomain =
      "Helping [[state]] political parties to promote their candidates in elections, free of charge.";

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
      EmailForm.SetItems("Request from a political party for username and password");
    }
  }
}