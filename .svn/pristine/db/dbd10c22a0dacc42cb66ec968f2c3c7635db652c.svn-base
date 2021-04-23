using System;

namespace Vote
{
  public partial class PoliticianIssuePage : PublicPage
  {
    protected PoliticianIssuePage()
    {
      NoUrlEdit = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      Response.RedirectPermanent(UrlManager.GetIntroPageUri(QueryId)
          .ToString(), true);
    }
  }
}