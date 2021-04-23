using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using Vote;

namespace VoteNew
{
  public static class Transition
  {
    public const int LegacyPort = 23232;

    public static Uri SetLegacyPort(Uri uri)
    {
      UriBuilder ub = new UriBuilder(uri);
      ub.Port = LegacyPort;
      return ub.Uri;
    }
  }

  public class NewVotePublicPage : System.Web.UI.Page
  {
    private Uri CanonicalUri;

    protected override void OnPreLoad(EventArgs e)
    {
      this.IncludeCss("~/css/Reset.css", true);

      base.OnPreLoad(e);

      // Fix a bad url
      Uri Url_Redirect = db.Url_Redirect_And_Log(out CanonicalUri);
      if (Url_Redirect != null)
        Response.RedirectPermanent(Url_Redirect.ToString());
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // Insert the canonical tag
      if (CanonicalUri != null)
      {
        if (Header == null)
          throw new VoteException("<head> tag missing runat=\"server\"");
        HtmlLink canonical = new HtmlLink();
        canonical.Href = CanonicalUri.ToString();
        canonical.Attributes["rel"] = "canonical";
        Page.Header.Controls.Add(canonical);
      }
    }
  }
}