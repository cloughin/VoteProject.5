using System;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Vote
{
  public class PublicPage : VotePage
  {
    #region Private

    private (Uri RedirectUri, Uri CanonicalUri) NormalizeUrl()
    {
      //var redirectUri = NormalizeUrl(out var canonicalUri);
      //return (redirectUri, canonicalUri);

      var urlNormalizer = new UrlNormalizer();
      if (urlNormalizer.Normalize()) // no failure, possible redirect
        return (urlNormalizer.NormalizedUri, urlNormalizer.CanonicalUri);

      // could not normalize the url -- log and report it
      var logMessage = urlNormalizer.ErrorMessage;
      if (No404OnUrlNormalizeError)
        logMessage = $"[no 404] {logMessage}";
      Log404Error(logMessage);
      UrlError = urlNormalizer.ErrorMessage;
      if (!No404OnUrlNormalizeError)
        if (IsDebugging)
          throw new ApplicationException(urlNormalizer.ErrorMessage);
        else
          SafeTransferToError404();
      return (null, null);
    }

    //private Uri NormalizeUrl(out Uri canonicalUri)
    //{
    //  var urlNormalizer = new UrlNormalizer();
    //  if (urlNormalizer.Normalize()) // no failure, possible redirect
    //  {
    //    var redirectUri = urlNormalizer.NormalizedUri;
    //    canonicalUri = urlNormalizer.CanonicalUri;
    //    return redirectUri;
    //  }

    //  // could not normalize the url -- log and report it
    //  var logMessage = urlNormalizer.ErrorMessage;
    //  if (No404OnUrlNormalizeError)
    //    logMessage = $"[no 404] {logMessage}";
    //  Log404Error(logMessage);
    //  UrlError = urlNormalizer.ErrorMessage;
    //  if (!No404OnUrlNormalizeError)
    //    if (IsDebugging)
    //      throw new ApplicationException(urlNormalizer.ErrorMessage);
    //    else
    //      SafeTransferToError404();
    //  canonicalUri = null;
    //  return null;
    //}

    #endregion Private

    #region Protected

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    public Uri CanonicalUri { get; private set; }

    protected string UrlError { get; private set; }

    protected bool HasUrlError => UrlError != null;

    protected override void OnPreInit(EventArgs e)
    {
      var serverVariables = HttpContext.Current.Request.ServerVariables;
      var path = serverVariables["SCRIPT_NAME"];
      if (!NoUrlEdit &&
        !path.StartsWith("/test", StringComparison.OrdinalIgnoreCase))
      {
        // Fix a bad url
        var normalized = NormalizeUrl();
        CanonicalUri = normalized.CanonicalUri;
        //var redirectTo = NormalizeUrl(out _CanonicalUri);
        if (normalized.RedirectUri != null)
          Response.Redirect(normalized.RedirectUri.ToString());
      }
      base.OnPreInit(e);
    }

    protected bool No404OnUrlNormalizeError { get; set; }

    protected bool NoIndex { get; set; }

    protected bool NoUrlEdit { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      // Insert the noindex tag
      if (NoIndex)
      {
        if (Header == null)
          throw new VoteException("<head> tag missing runat=\"server\"");
        var noIndex = new HtmlMeta {Name = "robots", Content = "noindex"};
        Page.Header.Controls.Add(noIndex);
      }

      // Insert the canonical tag
      if (CanonicalUri != null)
      {
        if (Header == null)
          throw new VoteException("<head> tag missing runat=\"server\"");
        var canonical = new HtmlLink {Href = CanonicalUri.ToString()};
        canonical.Attributes["rel"] = "canonical";
        Page.Header.Controls.Add(canonical);
      }

      // Insert the ivn css
      //if (UrlManager.CurrentQuerySiteId == "ivn")
      //  this.IncludeCss("https://s3.amazonaws.com/ivn.us/ivn.css");
    }


    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public new PublicMasterPage Master => base.Master as PublicMasterPage;
    public Public2MasterPage Master2 => base.Master as Public2MasterPage;


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}