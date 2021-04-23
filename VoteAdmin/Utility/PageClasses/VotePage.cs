using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Vote
{
  public partial class VotePage : Page
  {
    #region Private

    private PageCache _PageCache;

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    #region Public Properties

    public static string CurrentPath => HttpContext.Current.Request.Path;

    public static string CurrentPathWithQuery
    {
      get
      {
        var path = CurrentPath;
        var query = HttpContext.Current.Request.QueryString.ToString();
        if (!string.IsNullOrWhiteSpace(query))
          path += "?" + query;
        return path;
      }
    }

    public static string CurrentUrl => Uri.UriSchemeHttp + "://" + UrlManager.CurrentHostName +
      CurrentPathWithQuery;

    public static string DonateUrl
      =>
      "https://connect.clickandpledge.com/w/Organization/vote-usa/paymentwidget/883bcac9-5998-4ecb-a45f-c4bf0a7fd519"
      ;

    public static string GetServerPath()
    {
      //returns like: d:\inetpub\wwwroot\
      // HttpContext.Current is only valid if running in a web context.
      // Because the CreateSitemaps app (and possibly others) run in a
      // Windows application context, we need to get the web path from
      // AppSettings when HttpContext.Current is null. We throw an
      // exception if there is no "VoteWebPath" app setting defined.
      if (HttpContext.Current == null)
      {
        var voteWebPath = ConfigurationManager.AppSettings["VoteWebPath"];
        if (string.IsNullOrWhiteSpace(voteWebPath))
          throw new ApplicationException("'VoteWebPath' app setting is missing.");
        return voteWebPath;
      }
      return HttpContext.Current.Server.MapPath(@"\");
    }

    protected static void HandleFatalError(string message)
    {
      if (IsDebugging)
        throw new ApplicationException(message);
      HttpContext.Current.Response.Redirect("/500.aspx");
    }

    public static bool IsDebugging
    {
      get
      {
#if DEBUG
        return true;
#else
        return false;
#endif
      }
    }

    public static bool IsPublicPage => string.IsNullOrWhiteSpace(SecurePage.PageSecurityClass);

    public static bool IsSessionStateEnabled
    {
      get
      {
        var context = HttpContext.Current;
        return context?.Session != null;
      }
    }

    protected void LegacyRedirect(string redirectTo)
    {
      if (GetQueryString("Legacy").ToUpperInvariant() != "TRUE")
        Response.Redirect(redirectTo);
    }

    public static string NoCacheParameter => "nc";

    public bool NoSocialMedia { get; protected set; }

    public PageCache PageCache => _PageCache ?? (_PageCache = new PageCache());

    public void ResetPageCache()
    {
      _PageCache = null;
    }

    public StateBag PublicViewState => ViewState;

    public static string QueryCongress
    {
      get
      {
        var result = GetQueryString("Congress");
        if (!string.IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string QueryCounty
    {
      get
      {
        var result = GetQueryString("County");
        if (!string.IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string QueryDesign => GetQueryString("Design")
      .ToUpperInvariant();

    public static string QueryElection => GetQueryString("Election")
      .ToUpperInvariant();

    public static string QueryId => GetQueryString("Id")
      .ToUpperInvariant();

    public static string QueryIssue => GetQueryString("Issue")
      .ToUpperInvariant();

    public static string QueryLocal
    {
      get
      {
        var result = GetQueryString("Local");
        if (!string.IsNullOrWhiteSpace(result))
          result = result.ZeroPad(2);
        return result;
      }
    }

    public static string QueryOffice => GetQueryString("Office")
      .ToUpperInvariant();

    public static string QueryOrganization => GetQueryString("Organization")
      .ToUpperInvariant();

    public static string QueryParty => GetQueryString("Party")
      .ToUpperInvariant();

    public static string QueryQuestion => GetQueryString("Question")
      .ToUpperInvariant();

    public static string QueryReferendum => GetQueryString("Referendum")
      .ToUpperInvariant();

    public static string QueryReport => GetQueryString("Report")
      .ToUpperInvariant();

    public static string QueryState => GetQueryString("State")
      .ToUpperInvariant();

    public static string QueryStateHouse
    {
      get
      {
        var result = GetQueryString("StateHouse");
        if (!string.IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string QueryStateSenate
    {
      get
      {
        var result = GetQueryString("StateSenate");
        if (!string.IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string UserName => GetSessionString("_UserName");

    #endregion Public Properties

    #region Public Methods

    public static VotePage Current => GetPage<VotePage>();

    public static T GetPage<T>() where T : Page
    {
      var context = HttpContext.Current;
      return context?.CurrentHandler as T;
    }

    public static PageCache GetPageCache()
    {
      var votePage = GetPage<VotePage>();
      return votePage == null ? PageCache.GetTemporary() : votePage.PageCache;
    }

    public static string GetSessionString(string name)
    {
      if (HttpContext.Current == null) return string.Empty;
      var sessionState = HttpContext.Current.Session;
      return sessionState == null
        ? string.Empty
        : (sessionState[name] as string).SafeString();
    }

    public static string GetQueryString(string name = null)
    {
      if (HttpContext.Current == null) return string.Empty;
      return string.IsNullOrWhiteSpace(name)
        ? HttpContext.Current.Request.QueryString.ToString()
        : HttpContext.Current.Request.QueryString[name].SafeString().Trim();
    }

    public static StateBag GetViewState()
    {
      var votePage = GetPage<VotePage>();
      return votePage?.PublicViewState;
    }

    public static string InsertNoCacheIntoUrl(string url)
    {
      var q = url.IndexOf('?');
      var insert = NoCacheParameter + "=1&X=" +
        (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond).ToString(
          CultureInfo.InvariantCulture);
      if (q < 0)
        return url + "?" + insert;
      return url.Substring(0, q + 1) + insert + "&" + url.Substring(q + 1);
    }

    public void InvalidatePageCache()
    {
      _PageCache = null;
    }

    #endregion Public Methods

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}