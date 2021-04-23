using System;

namespace Vote
{
  public partial class ForElectionAuthoritiesPage : PublicPage // not CacheablePage due to Captcha
  {
    private const string TitleTagAllStatesDomain =
      "State Election Authorities Providing Election Rosters and Candidate Information";

    private const string MetaDescriptionAllStatesDomain =
      "Since 2004 state election authorities have been providing Vote USA LLC with election rosters and candidate information.";

    private const string TitleTagSingleStateDomain =
      "[[StateElectionAuthority]] Providing Election Rosters and Candidate Information";

    private const string MetaDescriptionSingleStateDomain =
      "Since 2004 [[StateElectionAuthority]] has been providing Vote USA LLC with election rosters and candidate information.";

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
      EmailForm.SetItems(
        "We are interested in the tools to manager our election rosters",
        "We are interested in incorporating interactive ballot choices");
    }
  }
}