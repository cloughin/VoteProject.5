using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.VoteLog;

namespace Vote
{
  // Public methods and properties relation to politicians
  public partial class VotePage
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static DateTime DefaultDbDate => Convert.ToDateTime("1900-01-01 00:00:00");

    public static string FormatLegislativeDistrictsFromQueryString(
      string separator = "<br />")
    {
      var lines = new List<string>();
      if (QueryState != "DC")
      {
        if (QueryCongress != "000")
          lines.Add("US House District " + QueryCongress.TrimStart('0') + " " +
            StateCache.GetStateName(QueryState));
        if (QueryStateSenate != "000")
          lines.Add(StateCache.GetStateName(QueryState) + " Senate District " +
            QueryStateSenate.TrimStart('0'));
        if (QueryStateHouse != "000")
          lines.Add(StateCache.GetStateName(QueryState) + " House District " +
            QueryStateHouse.TrimStart('0'));
        if (QueryCounty != string.Empty)
          lines.Add(CountyCache.GetCountyName(QueryState, QueryCounty));
      }
      else if (QueryStateSenate != "000")
        lines.Add("Ward " + QueryStateSenate.TrimStart('0'));

      if (separator == null)
        return "<span>" + string.Join("</span><span>", lines) + "</span>";
      return string.Join(separator, lines);
    }

    public static string FormatLegislativeDistrictsFromQueryStringForHeading(bool useSpans = false)
    {
      string subtitle;
      if (StateCache.IsValidStateCode(QueryState))
      {
        subtitle = (useSpans
            ? "<span class=\"districts-head\">For address in:</span>"
            : "<span class=\"districtsHead\">For address in:</span><br />") +
          FormatLegislativeDistrictsFromQueryString(useSpans ? null : "<br /");
      }
      else subtitle = "for any address in the United States";
      return subtitle;
    }

    public static Control GetMorePart1(string text, int min, int max, string type, string key,
      bool linkify = true)
    {
      key = $"{type}:{min}:{max}:{key}";
      var split = GetMoreSplit(text, min, max);
      var p = new HtmlP();
      var s = split[0];
      if (linkify) s = s.Linkify();
      s = s.ReplaceNewLinesWithParagraphs(false);
      new LiteralControl(s).AddTo(p);
      if (!string.IsNullOrEmpty(split[1]))
      {
        var span = new HtmlSpan().AddTo(p, "more-text");
        span.Attributes.Add("data-key", key);
      }
      return p;
    }

    public static string GetMorePart2(string text, int min, int max, bool linkify = true)
    {
      var s = GetMoreSplit(text, min, max)[1];
      if (linkify) s = s.Linkify();
      s = s.ReplaceNewLinesWithParagraphs(false);
      return s;
    }

    private static string[] GetMoreSplit(string text, int min, int max)
    {
      if (text.Length <= max)
        return new[]
        {
          text,
          string.Empty
        };
      var splitAt = text.IndexOf(' ', min);
      splitAt = splitAt == -1
        ? max
        : Math.Min(splitAt, max);
      return new[]
      {
        text.Substring(0, splitAt),
        text.Substring(splitAt)
      };
    }

    private static readonly Regex IsUrlRegex =
      new Regex(@"^(?:(http(?:s)?\:\/\/)?\S+(?:(?:\.)\S+)+(?:\/S+)*)$", RegexOptions.IgnoreCase);

    public static bool IsValidUrl(string candidate)
    {
      return IsUrlRegex.IsMatch(candidate);
    }

    public static void Log301Redirect(string redirectPage)
    {
      if (MemCache.IsLoggingErrors)
        DB.VoteLog.Log301Redirect.Insert(DateTime.UtcNow, CurrentUrl, redirectPage);
    }

    public static void Log404Error(string msg)
    {
      if (MemCache.IsLoggingErrors)
        Log404PageNotFound.Insert(DateTime.UtcNow, CurrentUrl, msg);
    }

    public static void LogInfo(string source, string message)
    {
      DB.VoteLog.LogInfo.Insert(DateTime.UtcNow, Environment.MachineName, source,
        message);
    }

    public static void LogException(string errorType, Exception ex,
      string message = null)
    {
      var sb = new StringBuilder();

      while (ex != null)
      {
        if (sb.Length > 0)
        {
          sb.Append(Environment.NewLine);
          sb.Append(Environment.NewLine);
        }
        sb.Append(ex.Message);
        sb.Append(Environment.NewLine);
        sb.Append(ex.StackTrace);
        ex = ex.InnerException;
      }

      var currentUrl = HttpContext.Current != null
        ? CurrentUrl
        : "CurrentUrl not available";

      LogExceptions.Insert(DateTime.UtcNow, Environment.MachineName, errorType,
        currentUrl, message, sb.ToString());
    }

    public static void LogException(string errorType, string message)
    {
      LogException(errorType, null, message);
    }

    public static string NormalizeEmailHRef(string email)
    {
      if (email.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
        return email;
      return "mailto:" + email;
    }

    public static string NormalizeUrl(string url)
    {
      if (string.IsNullOrWhiteSpace(url)) return url;
      var httpProtocol = Uri.UriSchemeHttp + "://";
      var httpsProtocol = Uri.UriSchemeHttps + "://";
      if (url.StartsWith(httpProtocol, StringComparison.OrdinalIgnoreCase) ||
        url.StartsWith(httpsProtocol, StringComparison.OrdinalIgnoreCase))
        return url;
      return httpProtocol + url;
    }

    // This is to prevent recursion or exceptions when transferring
    // to /404.aspx
    public static void SafeTransferToError404()
    {
      if (!IsDebugging)
        if (HttpContext.Current != null)
          if (
            !string.Equals(HttpContext.Current.Request.CurrentExecutionFilePath,
              "/404.aspx", StringComparison.OrdinalIgnoreCase))
            HttpContext.Current.Server.Transfer("/404.aspx");
      // If Debugging, return and let the exception be reported in all its gory detail
    }

    // This is to prevent recursion or exceptions when transferring
    // to /500.aspx
    public static void SafeTransferToError500()
    {
      if (!IsDebugging)
        if (HttpContext.Current != null)
          if (
            !string.Equals(HttpContext.Current.Request.CurrentExecutionFilePath,
              "/500.aspx", StringComparison.OrdinalIgnoreCase))
            HttpContext.Current.Server.Transfer("/500.aspx");
      // If Debugging, return and let the exception be reported in all its gory detail
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  // Vote-specific ExtensionMethods
  public static class VotePageExtensionMethods
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static string DbDateToShortDate(this DateTime dateTime)
    {
      return dateTime == VotePage.DefaultDbDate
        ? string.Empty
        : dateTime.ToShortDateString();
    }

    public static string DbDateToShortDateTime(this DateTime dateTime,
      bool utc = false)
    {
      return dateTime == VotePage.DefaultDbDate
        ? string.Empty
        : dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString() +
        (utc || (dateTime.Kind == DateTimeKind.Utc) ? " UTC" : string.Empty);
    }

    public static string DbDateToShortDate(this DateTime? dateTime)
    {
      return dateTime == null ? string.Empty : dateTime.Value.DbDateToShortDate();
    }

    public static void IncludeCss(this Page page, string path,
      params string[] ieOverrides)
    {
      page.IncludeCss(path, false);
      for (var n = 0; n < ieOverrides.Length - 1; n += 2)
      {
        var ieFilename = Path.GetDirectoryName(path) + "/" +
          Path.GetFileNameWithoutExtension(path) + "." +
          ieOverrides[n + 1] + Path.GetExtension(path);
        page.IncludeCss(ieFilename, false, ieOverrides[n]);
      }
    }

    public static void IncludeCss(this Page page, string path, bool first,
      string ieCondition = null)
    {
      if (page.Header == null) return;
      path = path.Replace('\\', '/');

      string resolvedPath;
      if (path.Contains("://")) resolvedPath = path;
      else
      {
        // strip query buster
        int pos;
        if ((pos = path.IndexOf("?", StringComparison.Ordinal)) >= 0)
          path = path.Substring(0, pos);
        resolvedPath = page.ResolveClientUrl(path);
        var file = new FileInfo(page.Server.MapPath(resolvedPath));

        // skip if no such file
        if (!file.Exists) return;

        // append mod time to defeat browser caching
        resolvedPath += "?" + file.LastWriteTimeUtc.Ticks;
      }

      // skip if already added
      HtmlLink firstStylesheet = null;
      foreach (Control control in page.Header.Controls)
      {
        var htmlLink = control as HtmlLink;
        if (htmlLink != null)
          if ( //htmlLink.Attributes["type"].IsEqIgnoreCase("text/css") &&
            htmlLink.Attributes["rel"].IsEqIgnoreCase("stylesheet"))
          {
            if (firstStylesheet == null)
              firstStylesheet = htmlLink;
            if (htmlLink.Href.IsEqIgnoreCase(resolvedPath))
              return;
          }
      }
      var newLink = new HtmlLink {Href = resolvedPath};
      newLink.Attributes.Add("type", "text/css");
      newLink.Attributes.Add("rel", "stylesheet");
      Control toAdd;
      if (string.IsNullOrWhiteSpace(ieCondition)) toAdd = newLink;
      else
      {
        toAdd = new PlaceHolder();
        new LiteralControl("\n<!--[" + ieCondition + "]>\n").AddTo(toAdd);
        newLink.AddTo(toAdd);
        new LiteralControl("\n<![endif]-->\n").AddTo(toAdd);
      }
      if (first && (firstStylesheet != null))
      {
        var index = page.Header.Controls.IndexOf(firstStylesheet);
        page.Header.Controls.AddAt(index, toAdd);
      }
      else
        toAdd.AddTo(page.Header);
    }

    public static void IncludeJs(this Page page, string path)
    {
      string resolvedPath;
      if (path.Contains("://")) resolvedPath = path;
      else
      {
        // strip query buster
        int pos;
        if ((pos = path.IndexOf("?", StringComparison.Ordinal)) >= 0)
          path = path.Substring(0, pos);
        resolvedPath = page.ResolveClientUrl(path);
        var file = new FileInfo(page.Server.MapPath(resolvedPath));

        // skip if no such file
        if (!file.Exists) return;

        // append mod time to defeat browser caching
        resolvedPath += "?" + file.LastWriteTimeUtc.Ticks;
      }

      page.ClientScript.RegisterClientScriptInclude(resolvedPath, resolvedPath);
    }

    public static DateTime SafeDbDate(this DateTime? dateTime)
    {
      return dateTime ?? VotePage.DefaultDbDate;
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}