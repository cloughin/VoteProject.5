using System;
using static System.String;

namespace Vote
{
  public partial class ForCandidatesPage : PublicPage // not CacheablePage due to Captcha
  {
    private const string TitleTag = "{0} | Free Candidate Advertising & Promotion";
    private const string MetaDescriptionTag = "Free promotion for candidates in {0}elections including pictures, bios, position statements and social media links.";

    protected void Page_Load(object sender, EventArgs e)
    {
      var stateName = PublicMasterPage.StateName + " ";
      if (stateName == "US ") stateName = Empty;
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag, stateName);

      //VoteUsaAddress.NoBreak = true;
      EmailForm.ToEmailAddress = "john.fleisch@vote-usa.org";
      EmailForm.SetItems(
        "I am a candidate -- I need my logon credentials and instructions");
    }
  }
}