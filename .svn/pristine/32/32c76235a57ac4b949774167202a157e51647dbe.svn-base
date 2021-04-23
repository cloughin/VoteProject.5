using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class MainFooter : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeCss("~/css/MainFooter.css");

      if (!IsPostBack)
        if (!string.IsNullOrEmpty(UrlManager.CurrentQuerySiteId))
        {
          var stateCode = UrlManager.FindStateCode();
          HomeLink.HRef = UrlManager.GetDefaultPageUri(stateCode)
            .ToString();
          AboutUsLink.HRef = UrlManager.GetAboutUsPageUri(stateCode)
            .ToString();
          ContactUsLink.HRef = UrlManager.GetContactUsPageUri(stateCode)
            .ToString();
          PrivacyLink.HRef = UrlManager.GetPrivacyPageUri(stateCode)
            .ToString();
          DonateLink.HRef = UrlManager.GetDonatePageUri(stateCode)
            .ToString();
          ForVotersLink.HRef = UrlManager.GetForVotersPageUri(stateCode)
            .ToString();
          ForCandidatesLink.HRef = UrlManager.GetForCandidatesPageUri(stateCode)
            .ToString();
          ForVolunteersLink.HRef = UrlManager.GetForVolunteersPageUri(stateCode)
            .ToString();
          ForPartersLink.HRef = UrlManager.GetForPartnersPageUri(stateCode)
            .ToString();
          ForPoliticalPartiesLink.HRef =
            UrlManager.GetForPoliticalPartiesPageUri(stateCode)
              .ToString();
          ForResearchLink.HRef = UrlManager.GetForResearchPageUri(stateCode)
            .ToString();
          ForElectionAuthoritiesLink.HRef =
            UrlManager.GetForElectionAuthoritiesPageUri(stateCode)
              .ToString();
        }
    }
  }
}