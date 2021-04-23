using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;

namespace Vote
{
  /// <summary>
  /// Summary description for Global.
  /// </summary>
  public class Global : HttpApplication
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
// ReSharper disable NotAccessedField.Local
    private IContainer components;
    // ReSharper restore NotAccessedField.Local

    protected Global()
    {
      InitializeComponent();
    }

    protected void Application_Start(object sender, EventArgs e)
    {
#if !DEBUG
      // prime the Tiger lookup (with an arbitrary location) because of long start time
      TigerLookup.LookupAll(38.976657, -77.373288);
#endif
    }

    protected void Session_Start(object sender, EventArgs e)
    {
#if DEBUG
      GlobalAsax.ProcessVoteConfigForSessionStart(this);
#endif
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
#if DEBUG
      GlobalAsax.ProcessVoteConfigForBeginRequest(this);
#endif
    }

    private static readonly Regex SitemapPathRegex =
      new Regex(@"/sitemap(?<dataCode>[a-z]{2})\.xml", RegexOptions.IgnoreCase);
    private static readonly Regex UploadedImagesRegex =
      new Regex(@"^/image/(?<name>.+)\.[^.]+$", RegexOptions.IgnoreCase);

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
      var path = HttpContext.Current.Request.Path;

      // We intercept requests for sitemaps and handle them here
      var match = SitemapPathRegex.Match(path);
      if (match.Success)
      {
        var domainDataCode = match.Groups["dataCode"].Captures[0].Value;
        if (domainDataCode.ToUpper() != "US") return;
        SitemapManager.ServeSitemap(domainDataCode);
        CompleteRequest();
        return;
      }

      // ... and uploaded images
      match = UploadedImagesRegex.Match(path);
      if (match.Success)
      {
        var externalName = match.Groups["name"].Captures[0].Value;
        ImageUtility.ServeImage(externalName);
        CompleteRequest();
        return;
      }
    }

    protected void Application_PostMapRequestHandler(object sender, EventArgs e) {}

    protected void Application_EndRequest(object sender, EventArgs e) {}

    protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

    protected void Application_Error(object sender, EventArgs e)
    {
      GlobalAsax.OnApplicationError(this);
    }

    protected void Session_End(object sender, EventArgs e) {}

    protected void Application_End(object sender, EventArgs e) {}

#region Web Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new Container();
    }

#endregion
  }
}