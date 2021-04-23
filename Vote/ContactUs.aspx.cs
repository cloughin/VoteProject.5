using System;
using static System.String;

namespace Vote
{
  public partial class ContactUsPage : PublicPage // not CacheablePage due to Captcha
  {
    private const string TitleTag = "{0} | Contact Us";
    private const string MetaDescriptionTag = "Contact {0}.org by email or traditional mail.";

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, PublicMasterPage.SiteName);

      EmailForm.ToEmailAddress = "john.fleisch@vote-usa.org";

      EmailForm.SetItems(
        "Privacy issues", "Donation issues", "Volunteer My Help",
        "I’m a Candidate and Need My Login Credentials");
    }
  }
}