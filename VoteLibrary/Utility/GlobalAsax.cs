using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using DB.VoteLog;
using static System.String;

namespace Vote
{
  public static class GlobalAsax
  {
    private static readonly Regex EmbeddedUrlRegex =
      new Regex(
        @"^/.*(?:\.biz|\.com|\.info|\.name|\.net|\.org|\.edu|\.gov)(?:$|/.*)",
        RegexOptions.IgnoreCase);

    private static XmlElement GetVoteConfiguration()
    {
      XmlElement documentElement = null;
      var documentsFolder =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      var voteConfigPath = Path.Combine(documentsFolder, @"Vote\VoteConfig.xml");
      if (!File.Exists(voteConfigPath))
        // Try c drive
        voteConfigPath = @"C:\VoteConfig\VoteConfig.xml";
      if (File.Exists(voteConfigPath))
      {
        var document = new XmlDocument();
        document.Load(voteConfigPath);
        documentElement = document.DocumentElement;
        Debug.Assert(documentElement != null, "documentElement != null");
        if (!bool.TryParse(documentElement.GetAttribute("enabled"), out var enabled))
          enabled = false;
        if (!enabled) documentElement = null; // only return an enabled document
      }
      return documentElement;
    }

    public static void OnApplicationError(HttpApplication application)
    {
      try
      {
        var path = HttpContext.Current.Request.Path;
        var url = path;
        var query = HttpContext.Current.Request.QueryString.ToString();
        if (!IsNullOrEmpty(query))
          url = url + '?' + query;

        // check for embedded url
        if (EmbeddedUrlRegex.Match(path)
          .Success) // redirect to real url
        {
          application.Response.Redirect(Uri.UriSchemeHttp + "://" + url.Substring(1), true);
          return;
        }

        if (application.Context.Error != null)
          if (
            application.Context.Error.Message.StartsWith("A potentially dangerous Request.Path value was detected from the client", StringComparison.Ordinal) &&
              HttpUtility.UrlDecode(path) == path)
          {
            application.Response.RedirectPermanent(
              HttpContext.Current.Request.Url.AbsoluteUri, true);
          }
          else
            Log404PageNotFound.Log("Global.asax Application_Error: " + application.Context.Error.Message);
        else
          Log404PageNotFound.Log("Global.asax Application_Error");
      }
      catch
      {
        // ignored
      } // no exceptions may be thrown here
    }

    public static void ProcessVoteConfigForBeginRequest(HttpApplication application)
    {
      // we only do it for aspx pages with no query string
      var path = HttpContext.Current.Request.Path;
      var pathLowered = path.ToLowerInvariant();
      var queryString = HttpContext.Current.Request.QueryString.ToString();
      if (!IsNullOrEmpty(queryString) ||
        Path.GetExtension(pathLowered) != ".aspx") return;
      //this prevents infinite recursion on admin pages
      if (VotePage.IsSessionStateEnabled && SecurePage.QueryStringMightNotBeNeeded)
        return;
      var documentElement = GetVoteConfiguration();
      if (documentElement == null) return;
      var pathXPath =
        "queryStrings/queryString[@enabled=\"true\" and translate(@path, " +
          // the translation enables a case-insensitive search
          "'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=\"" +
          pathLowered + "\"]";
      if (!(documentElement.SelectSingleNode(pathXPath) is XmlElement queryStringElement)) return;
      queryString = queryStringElement.GetAttribute("queryString");
      if (!IsNullOrEmpty(queryString))
        //Context.RewritePath(path, null, queryString);
        application.Response.Redirect(path + "?" + queryString);
    }

    public static void ProcessVoteConfigForSessionStart(HttpApplication application)
    {
      var documentElement = GetVoteConfiguration();
      if (!(documentElement?.SelectSingleNode("options") is XmlElement optionsElement)) return;
      var sessionVariablesSet = optionsElement.GetAttribute(
        "sessionVariablesSet");
      if (!bool.TryParse(optionsElement.GetAttribute("autoLogin"), out var autoLogin))
        autoLogin = false;
      if (IsNullOrEmpty(sessionVariablesSet)) return;
      var sessionVariablesXPath =
        "sessionVariablesSets/sessionVariablesSet[@name=\"" +
          sessionVariablesSet + "\"]/sessionVariable";
      var sessionVariables = documentElement.SelectNodes(sessionVariablesXPath);
      Debug.Assert(sessionVariables != null, "sessionVariables != null");
      foreach (XmlElement sessionVariable in sessionVariables)
      {
        var name = sessionVariable.GetAttribute("name");
        var value = sessionVariable.GetAttribute("value");
        if (IsNullOrEmpty(name)) continue;
        application.Session[name] = value;
        if (name == "UserName" && autoLogin)
          application.Session["AutoLoginUserName"] = value;
      }
    }
  }
}
