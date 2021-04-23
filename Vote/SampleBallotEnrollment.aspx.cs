using System;
using System.Web.UI;
using static System.String;

namespace Vote
{
  public partial class SampleBallotEnrollmentPage : PublicPage
  {
    private const string TitleTag = "{0} | Automatic Ballot Choices Enrollment";
    private const string MetaDescriptionTag = "Sign up for automatic future ballot choices via email.";
    private const string H1Tag = "Sign Up for Automatic Future Ballot Choices via Email";

    protected SampleBallotEnrollmentPage()
    {
      NoUrlEdit = true;
    }

    protected override void OnPreInit(EventArgs e)
    {
      var uri = UrlNormalizer.BuildCurrentUri();
      if (uri.Host.IsNeIgnoreCase(UrlManager.SiteHostName))
      {
        var query = uri.Query;
        if (query.StartsWith("?")) query = query.Substring(1);
        Response.Redirect(UrlManager.GetSiteUri(uri.AbsolutePath, query).ToString());
        Response.End();
      }
      base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Title = Format(TitleTag, PublicMasterPage.SiteName);
      H1.InnerHtml = Format(H1Tag, PublicMasterPage.SiteName);
      MetaDescription = Format(MetaDescriptionTag);
      var email = GetQueryString("email");
      if (Validation.IsValidEmailAddress(email))
      {
        EmailEntry.Visible = false;
        EmailFixedAddress.InnerText = email;
      }
      else
      {
        EmailFixed.Visible = false;
        AddressEntryBlock.Style.Add(HtmlTextWriterStyle.Display,"none");
      }
    }
  }
}