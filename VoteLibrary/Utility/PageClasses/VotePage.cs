using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using static System.String;

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

    private static bool? _AllowFacebookVideos;

    public static bool AllowFacebookVideos
    {
      get
      {
        return (_AllowFacebookVideos ??
          (_AllowFacebookVideos = ConfigurationManager.AppSettings["AllowFacebookVideos"] == "true")).Value;
      }
    }

    public static string CurrentPath => HttpContext.Current.Request.Path;

    public static string CurrentPathWithQuery
    {
      get
      {
        var path = CurrentPath;
        var query = HttpContext.Current.Request.QueryString.ToString();
        if (!IsNullOrWhiteSpace(query))
          path += "?" + query;
        return path;
      }
    }

    public static string CurrentUrl => Uri.UriSchemeHttps + "://" + UrlManager.CurrentHostName +
      CurrentPathWithQuery;

    public static string DonateUrl
      =>
      "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=7KVMAE3D2XAYL&source=url"
      ;

    private static bool? _EnableIssueGroups;

    public static bool EnableIssueGroups
    {
      get
      {
        return (_EnableIssueGroups ??
          (_EnableIssueGroups = ConfigurationManager.AppSettings["EnableIssueGroups"] == "true")).Value;
      }
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

    public static bool IsPublicPage
    {
      get { return IsNullOrWhiteSpace(SecurePage.PageSecurityClass) &&
        GetPage<SecurePage>()?.IsSignInPage != true; }
    }

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

    public bool NoCacheViaCookie
    {
      get { return Request.Cookies["nocache"] != null || Request.Cookies["pnocache"] != null; }
    }

    public bool NoSocialMedia { get; protected set; }

    public PageCache PageCache => _PageCache ?? (_PageCache = new PageCache());

    public void ResetPageCache()
    {
      _PageCache = null;
    }

    public static string QueryCityCouncil
    {
      get
      {
        return GetQueryString("Cc");
      }
    }

    public static string QueryCongress
    {
      get
      {
        var result = GetQueryString("Congress");
        if (!IsNullOrWhiteSpace(result))
          result = result.TrimStart('0').ZeroPad(3);
        return result;
      }
    }

    public static string QueryCounty
    {
      get
      {
        var result = GetQueryString("County");
        if (!IsNullOrWhiteSpace(result))
          result = result.TrimStart('0').ZeroPad(3);
        return result;
      }
    }

    public static string QueryCountySupervisors
    {
      get
      {
        return GetQueryString("Cs");
      }
    }

    public static string QueryDistrict
    {
      get
      {
        return GetQueryString("District");
      }
    }

    public static string QueryDesign
    {
      get { return GetQueryString("Design").ToUpperInvariant(); }
    }

    public static string QueryElection
    {
      get { return GetQueryString("Election").ToUpperInvariant(); }
    }

    public static string QueryElementary
    {
      get
      {
        return GetQueryString("Esd");
      }
    }

    public static string QueryId
    {
      get { return GetQueryString("Id").ToUpperInvariant(); }
    }

    public static string QueryIssue
    {
      get { return GetQueryString("Issue").ToUpperInvariant(); }
    }

    public static string QueryLocal
    {
      get
      {
        var result = GetQueryString("Local");
        // check for localKey
        if (result.Length == 5) return result;
        if (!IsNullOrWhiteSpace(result))
          result = result.TrimStart('0').ZeroPad(2);
        return result;
      }
    }

    public static string QueryOffice
    {
      get { return GetQueryString("Office").ToUpperInvariant(); }
    }

    public static string QueryOrganization
    {
      get { return GetQueryString("Organization").ToUpperInvariant(); }
    }

    public static string QueryParty
    {
      get { return GetQueryString("Party").ToUpperInvariant(); }
    }

    public static string QueryPlace
    {
      get
      {
        return GetQueryString("Place");
      }
    }

    public static string QueryQuestion
    {
      get { return GetQueryString("Question").ToUpperInvariant(); }
    }

    public static string QueryReferendum
    {
      get { return GetQueryString("Referendum").ToUpperInvariant(); }
    }

    public static string QueryReport
    {
      get { return GetQueryString("Report").ToUpperInvariant(); }
    }

    public static string QuerySchoolDistrictDistrict
    {
      get
      {
        return GetQueryString("Sdd");
      }
    }

    public static string QuerySecondary
    {
      get
      {
        return GetQueryString("Ssd");
      }
    }

    public static string QueryState
    {
      get { return GetQueryString("State").ToUpperInvariant(); }
    }

    public static string QueryStateHouse
    {
      get
      {
        var result = GetQueryString("StateHouse");
        if (!IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string QueryStateSenate
    {
      get
      {
        var result = GetQueryString("StateSenate");
        if (!IsNullOrWhiteSpace(result))
          result = result.ZeroPad(3);
        return result;
      }
    }

    public static string QueryUnified
    {
      get
      {
        return GetQueryString("Usd");
      }
    }

#pragma warning disable 169
    private static bool? _ShowIssues;
#pragma warning restore 169

    public static bool ShowIssues
    {
      get
      {
        //return (_ShowIssues ??
        //  (_ShowIssues = ConfigurationManager.AppSettings["ShowIssues"] == "true")).Value;
        return true;
      }
    }

    private static bool? _ShowPresidentialComparisons;

    public static bool ShowPresidentialComparisons
    {
      get
      {
        return (_ShowPresidentialComparisons ??
          (_ShowPresidentialComparisons = ConfigurationManager.AppSettings["ShowPresidentialComparisons"] == "true")).Value;
      }
    }

    //private static bool? _UseFlexAds;

    //public static bool UseFlexAds
    //{
    //  get
    //  {
    //    return (_UseFlexAds ??
    //      (_UseFlexAds = ConfigurationManager.AppSettings["UseFlexAds"] == "true")).Value;
    //  }
    //}

    public static string UserName
    {
      get { return GetSessionString("_UserName"); }
    }

    #endregion Public Properties

    #region Public Methods

    private static readonly Regex CheckForDangerousInputRegex = new Regex("<script|<.+>", RegexOptions.IgnoreCase);
    public static void CheckForDangerousInput(params ITextControl[] textboxes)
    {
      foreach (var textbox in textboxes)
        if (CheckForDangerousInputRegex.Match(textbox.Text).Success)
          throw new ApplicationException("Text in a textbox appears to be HTML");
    }

    public static VotePage Current
    {
      get { return GetPage<VotePage>(); }
    }

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
      if (HttpContext.Current == null) return Empty;
      var sessionState = HttpContext.Current.Session;
      return sessionState == null
        ? Empty
        : (sessionState[name] as string).SafeString();
    }

    public static string GetQueryString(string name = null)
    {
      if (HttpContext.Current == null) return Empty;
      return IsNullOrWhiteSpace(name)
        ? HttpContext.Current.Request.QueryString.ToString()
        : HttpContext.Current.Request.QueryString[name].SafeString().Trim();
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

    public static void PutSessionString(string name, string value)
    {
      if (HttpContext.Current != null)
        HttpContext.Current.Session[name] = value;
    }

    #endregion Public Methods

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}