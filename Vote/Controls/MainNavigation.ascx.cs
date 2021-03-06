using System;
using System.Web.UI;

namespace Vote.Controls
{
  public partial class MainNavigation : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.IncludeJs("http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js");
      Page.IncludeJs(
        "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js");
      Page.IncludeJs("~/js/vote.js");
      Page.IncludeJs("~/js/donate.js");
      Page.IncludeCss("~/js/jq/jquery-ui.css");
      Page.IncludeCss("~/css/MainNavigation.css");
      DonateLink.HRef = VotePage.DonateUrl;

      if (!IsPostBack)
        if (!string.IsNullOrEmpty(UrlManager.CurrentQuerySiteId))
        {
          var stateCode = UrlManager.FindStateCode();
          HomeLink.HRef = UrlManager.GetDefaultPageUri(stateCode)
            .ToString();
          ForVotersLink.HRef = UrlManager.GetForVotersPageUri(stateCode)
            .ToString();
          ForCandidatesLink.HRef = UrlManager.GetForCandidatesPageUri(stateCode)
            .ToString();
          ForVolunteersLink.HRef = UrlManager.GetForVolunteersPageUri(stateCode)
            .ToString();
        }
    }
  }
}