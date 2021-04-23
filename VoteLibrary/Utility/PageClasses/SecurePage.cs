using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote
{
  //  For new-style admin pages. Subclassed for various
  //  security levels.
  //
  public partial class SecurePage : VotePage
  {
    #region Private

    private static IEnumerable<string> SuperfluousQueryParameters
    {
      get
      {
        switch (PageSecurityClass)
        {
          case AdminSecurityClass:
            if (IsStateAdminUser) return new[] {"state"};
            if (IsCountyAdminUser) return new[] {"state", "county"};
            if (IsLocalAdminUser) return new[] {"state", "county", "local"};
            break;

          case PartySecurityClass:
            if (IsPartyUser) return new[] {"party"};
            break;

          case PoliticianSecurityClass:
            if (IsPoliticianUser) return new[] {"id"};
            break;
        }

        return null;
      }
    }

    private void RedirectIfSuperfluousQueryString()
    {

      var hasQueryString =
        !IsNullOrWhiteSpace(Request.QueryString.ToString());
      if (!hasQueryString) return;
      var superfluousParameters = SuperfluousQueryParameters;
      if (superfluousParameters == null) return;
      var qsc = QueryStringCollection.Parse(Request.QueryString.ToString());
      var startingCount = qsc.Count;
      foreach (var superfluousParameter in superfluousParameters) qsc.Remove(superfluousParameter);

      if (qsc.Count != startingCount) Response.Redirect(qsc.AddToPath(Request.Path));
    }

    #endregion Private

    #region Protected

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void CheckSecurity()
    {
      var ok = IsSignInPage || GetUserSecurityIsOk();

      if (!ok) SecurityExceptionHandler();

      RedirectIfSuperfluousQueryString();
    }

    protected void InvokeInitializers()
    {
      // get list of derived-from classes that have PageInitializersAttribute
      var pageType = GetType();
      var pageTypes = new List<Type>();
      while (pageType != null && pageType != typeof (SecurePage))
      {
        if (pageType.IsDefined(typeof (PageInitializersAttribute), false)) pageTypes.Add(pageType);
        pageType = pageType.BaseType;
      }

      // for each of these classes, call the "Initialize(page)" method for
      // any sub-classes that have PageInitializerAttribute
      foreach (var oneType in pageTypes)
      {
        var type = oneType;
        foreach (var method in
          type.GetNestedTypes(BindingFlags.NonPublic)
            .Where(t => t.IsDefined(typeof (PageInitializerAttribute), false))
            .Select(
              t =>
                t.GetMethod("Initialize",
                  BindingFlags.Public | BindingFlags.NonPublic |
                  BindingFlags.Static, null, new[] {type}, null))
            .Where(m => m != null)) method.Invoke(null, new object[] {this});
      }
    }

    protected static void LogImageChange(string politicianKey, byte[] oldImageBlob, byte[] newImageBlob,
      DateTime uploadTime)
    {
      LogDataChange.LogUpdate(PoliticiansImagesBlobs.Column.ProfileOriginal,
        oldImageBlob, newImageBlob, UserName, UserSecurityClass, uploadTime,
        politicianKey);
    }

    protected static void LogImageHeadshotChange(string politicianKey, byte[] oldImageBlob, byte[] newImageBlob,
      DateTime uploadTime)
    {
      LogDataChange.LogUpdate(PoliticiansImagesBlobs.Column.Headshot100,
        oldImageBlob, newImageBlob, UserName, UserSecurityClass, uploadTime,
        politicianKey);
    }

    public new IAdminMaster Master
    {
      get { return base.Master as IAdminMaster; }
    }

    protected virtual void SecurityExceptionHandler()
    {
      // To give subclasses an opportunity to override, since
      // HandleSecurityException is static
      HandleSecurityException();
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void ScriptManager_AsyncPostBackError(object sender,
      AsyncPostBackErrorEventArgs e)
    {
      LogException("AsyncPostBackError", e.Exception);
    }

    protected void SetNoCacheForState()
    {
      //if (IsNullOrWhiteSpace(stateCode)) return;
      //var uri = StateCache.IsValidStateCode(stateCode) 
      //  ? UrlManager.GetStateUri(stateCode, "/nocache.aspx") 
      //  : UrlManager.GetSiteUri("/nocache.aspx");
      //Master.SetNoCacheUrl(uri.ToString());
      Master.SetNoCacheUrl("/nocache.aspx");
    }

    public static void ThrowRandomException()
    {
#if DEBUG
      // use ?test=anything to test exception handling during update
      if (!IsNullOrWhiteSpace(GetQueryString("testex")))
        if (DateTime.Now.Ticks % 2 == 1) throw new VoteException("Some random error");
#endif
    }

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public interface IAdminMaster
    {
      System.Web.UI.Control FindControl(string id);
      System.Web.UI.Control FindMainContentControl(string id);
      bool HasMenu { get; }
      ContentPlaceHolder MainContentControl { get; }
      HtmlForm MasterForm { get; }
      bool NoHeading { set; }
      bool NoMenu { set; }
      void SetJavascriptNotNeeded();
      void SetNoCacheUrl(string url);
    }

    public enum UpdateStatus
    {
      Unchanged,
      Success,
      Failure
    }

    private static bool? _IsIsolated;

    public static bool IsIsolated
    {
      get
      {
        if (_IsIsolated == null)
          _IsIsolated = WebConfigurationManager.AppSettings["IsIsolated"] == "true";
        return _IsIsolated.Value;
      }
    }

    public const string MasterSecurityClass = "MASTER";
    public const string AdminSecurityClass = "ADMIN";
    public const string StateAdminSecurityClass = "ADMIN";
    public const string CountyAdminSecurityClass = "COUNTY";
    public const string LocalAdminSecurityClass = "LOCAL";
    public const string PartySecurityClass = "PARTY";
    public const string PoliticianSecurityClass = "POLITICIAN";

    public static string BreakForTab(string input, Font font, int maxPixelWidth)
    {
      // breaks into two lines given a font and max width
      string output;
      var words = input.SplitOnWhiteSpace();
      if (words.Length > 0)
      {
        output = words[0];
        if (words.Length > 1)
        {
          var wordsBeforeBreak = 1;
          var candidate = words[0] + " " + words[1];
          while (wordsBeforeBreak < words.Length &&
            TextRenderer.MeasureText(candidate, font)
              .Width <= maxPixelWidth)
          {
            wordsBeforeBreak++;
            if (wordsBeforeBreak < words.Length) candidate += " " + words[wordsBeforeBreak];
          }
          if (wordsBeforeBreak < words.Length)
            output = Join(" ", words.Take(wordsBeforeBreak)) + "<br />" +
              Join(" ", words.Skip(wordsBeforeBreak));
          else output = candidate;
        }
      }
      else output = Empty;

      return output;
    }

    public static string BreakForTab2(string input, Font font, int maxPixelWidth)
    {
      // breaks into two lines given a font and max width
      var words = input.SplitOnWhiteSpace();
      if (words.Length <= 0) return Empty;
      if (words.Length == 1) return words[0];

      // measure words
      var wordWidths = words.Select(w => TextRenderer.MeasureText(w, font).Width).ToList();
      var spaceWidth = TextRenderer.MeasureText(" ", font).Width;
      var output = new List<string>();
      var wordInx = 0;
      while (wordInx < words.Length)
      {
        var lineWidth = wordWidths[wordInx];
        output.Add(words[wordInx]);
        wordInx++;
        while (wordInx < words.Length &&
          lineWidth + spaceWidth + wordWidths[wordInx] <= maxPixelWidth)
        {
          lineWidth += spaceWidth + wordWidths[wordInx];
          output.Add(" ");
          output.Add(words[wordInx]);
          wordInx++;
        }
        if (wordInx < words.Length)
          output.Add("<br />");
      }

      return Join(Empty, output);
    }

    public static string CreateUniquePassword()
    {
      var password = Empty;
      //Make a AAADDD password
      for (var n = 0; n < 3; n++) password += GetRandomAlpha24();
      for (var n = 0; n < 3; n++) password += GetRandomDigit();
      return password;
    }

    public static string FindCountyCode()
    {
      // reworked to eliminate ViewState references
      var stateCode = FindStateCode();
      if (IsNullOrWhiteSpace(stateCode)) return Empty;

      var countyCode = QueryCounty;
      if (IsNullOrWhiteSpace(countyCode))
        if (IsMasterUser || IsAdminUser)
        {
          countyCode = UserCountyCode;
          if (IsNullOrWhiteSpace(countyCode))
          {
            var localKey = FindLocalKey();
            if (!IsNullOrEmpty(localKey))
            {
              // if there is a local key and there is no county in the query string, 
              // we must look up a county. If there's more than 1, we just use the first we find.
              var counties = LocalIdsCodes.FindCounties(stateCode, localKey);
              countyCode = counties.Length == 0 ? Empty : counties[0];
            }
            else
            {
              countyCode = Elections.GetCountyCodeFromKey(QueryElection);
              if (IsNullOrWhiteSpace(countyCode))
                countyCode = Offices.GetCountyCodeFromKey(QueryOffice);
            }
          }
        }
      return !IsNullOrWhiteSpace(countyCode) &&
        CountyCache.CountyExists(stateCode, countyCode)
        ? countyCode
        : Empty;
    }

    public static string FindLocalKey()
    {
      var stateCode = FindStateCode();
      if (IsNullOrWhiteSpace(stateCode))
        return Empty;

      var localKey = QueryLocal;
      if (IsNullOrWhiteSpace(localKey))
        if (IsMasterUser || IsAdminUser)
        {
          localKey = UserLocalKey;
          if (IsNullOrWhiteSpace(localKey))
            localKey = Elections.GetLocalKeyFromKey(QueryElection);
          if (IsNullOrWhiteSpace(localKey))
            localKey = Offices.GetLocalKeyFromKey(QueryOffice);
        }
      return !IsNullOrWhiteSpace(localKey) &&
        LocalDistricts.IsValidKey(stateCode, localKey)
        ? localKey
        : Empty;
    }

    public static string FindPartyKey()
    {
      // reworked to eliminate ViewState references
      var partyKey = QueryParty;
      if (IsNullOrWhiteSpace(partyKey) && IsPartyUser)
        partyKey = UserPartyKey;
      return partyKey;
    }

    public static string FindPoliticianKey()
    {
      // reworked to eliminate ViewState references
      var politicianKey = QueryId;
      if (IsNullOrWhiteSpace(politicianKey) && IsPoliticianUser)
        politicianKey = UserPoliticianKey;
      return politicianKey;
    }

    public static string FindStateCode()
    {
      // reworked to eliminate ViewState references
      var stateCode = QueryState;
      if (IsNullOrWhiteSpace(stateCode))
        if (IsPublicPage)
          stateCode = UrlManager.CurrentDomainDataCode;
        else if (IsMasterUser || IsAdminUser)
        {
          stateCode = UserStateCode;
          if (IsNullOrWhiteSpace(stateCode))
            stateCode = Elections.GetStateCodeFromKey(QueryElection);
          if (IsNullOrWhiteSpace(stateCode))
            stateCode = Offices.GetStateCodeFromKey(QueryOffice);
          if (IsNullOrWhiteSpace(stateCode))
            stateCode = Politicians.GetStateCodeFromKey(QueryId);
          if (IsNullOrWhiteSpace(stateCode))
            stateCode = Issues.GetStateCodeFromKey(QueryIssue);
          if (IsNullOrWhiteSpace(stateCode))
            stateCode = Referendums.GetStateCodeFromKey(QueryReferendum);
        }
        else if (IsPoliticianUser)
          stateCode = Politicians.GetStateCodeFromKey(UserPoliticianKey);
        else if (IsPartyUser)
          stateCode = Parties.GetStateCodeFromKey(UserPartyKey);

      return !IsNullOrWhiteSpace(stateCode) &&
        (StateCache.IsValidStateOrFederalCode(stateCode)
          || GetPage<SecureAdminPage>()?.NonStateCodesAllowed.Contains(stateCode) == true)
        ? stateCode
        : Empty;
    }

    private static Random _RandomPasswordObject;

    private static char GetRandomAlpha24()
    {
      if (_RandomPasswordObject == null) _RandomPasswordObject = new Random();
      var n = _RandomPasswordObject.Next(24);
      if (n < 8) return Convert.ToChar(n + Convert.ToInt32('A'));
      return n < 13
        ? Convert.ToChar(n + Convert.ToInt32('B'))
        : Convert.ToChar(n + Convert.ToInt32('C'));
    }

    private static char GetRandomDigit()
    {
      if (_RandomPasswordObject == null) _RandomPasswordObject = new Random();
      return Convert.ToChar(_RandomPasswordObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string GetSecurityClass(string path)
    {
      var match = Regex.Match(path, @"^/(?<folder>[a-z0-9]+)/",
        RegexOptions.IgnoreCase);
      return match.Success
        ? match.Groups["folder"].Captures[0].Value.ToUpperInvariant()
        : Empty;
    }

    public bool GetUserSecurityIsOk()
    {
      var ok = IsSuperUser || IsMasterUser && !IsSuperUserPage;

      if (!ok)
        switch (PageSecurityClass)
        {
          case StateAdminSecurityClass:
            switch (UserSecurityClass)
            {
              case StateAdminSecurityClass:
                ok = FindStateCode() == UserStateCode;
                break;

              case CountyAdminSecurityClass:
                ok = FindStateCode() == UserStateCode &&
                  FindCountyCode() == UserCountyCode;
                break;

              case LocalAdminSecurityClass:
                ok = FindStateCode() == UserStateCode &&
                  FindCountyCode() == UserCountyCode &&
                  FindLocalKey() == UserLocalKey;
                break;
            }
            break;

          case PartySecurityClass:
            ok = IsPartyUser && FindPartyKey() == UserPartyKey;
            break;

          case PoliticianSecurityClass:
            ok = IsPoliticianUser && FindPoliticianKey() == UserPoliticianKey;
            if (!ok)
              if (IsPartyUser)
              {
                // allow access to the party's politician pages
                var partyKey = Politicians.GetPartyKey(FindPoliticianKey());
                if (!IsNullOrWhiteSpace(partyKey)) ok = partyKey.IsEqIgnoreCase(UserPartyKey);
              }
              else if (IsStateAdminUser)
              {
                // allow access to the state's politician pages
                ok = Politicians.GetStateCodeFromKey(FindPoliticianKey()) == UserStateCode;
              }
            break;
        }

      return ok;
    }

    public static void HandleSecurityException()
    {
      var queryString = IsSignedIn ? "Security=Y" : "Timeout=Y";
      RedirectToSignInPage(CurrentPathWithQuery, queryString);
    }

    public static bool QueryStringMightNotBeNeeded
    {
      get
      {
        var isSuperfluous = false;
        switch (PageSecurityClass)
        {
          case AdminSecurityClass:
            isSuperfluous = IsAdminUser;
            break;

          case PartySecurityClass:
            isSuperfluous = IsPartyUser;
            break;

          case PoliticianSecurityClass:
            isSuperfluous = IsPoliticianUser;
            break;
        }
        return isSuperfluous; 
      }
    }

    public static void RedirectToSignInPage(string returnUrl = null, string queryString = null)
    {
      var signInUrl = FormsAuthentication.LoginUrl; 

      if (!IsNullOrWhiteSpace(queryString))
      {
        signInUrl += signInUrl.IndexOf('?') < 0 ? '?' : '&';
        if (queryString.StartsWith("?", StringComparison.Ordinal))
          queryString = queryString.Substring(1);
        signInUrl += queryString;
      }

      if (!IsNullOrWhiteSpace(returnUrl))
      {
        signInUrl += signInUrl.IndexOf('?') < 0 ? '?' : '&';
        signInUrl += "ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
        // append original query paramenets, because this is what FormsAuthentication wants
        // ** maybe not needed
        //var queryPos = returnUrl.IndexOf("?", StringComparison.Ordinal);
        //if (queryPos >= 0)
        //  signInUrl += "&" + returnUrl.Substring(queryPos + 1);
      }

      HttpContext.Current.Response.Redirect(signInUrl);
    }

    #region Security Properties

    public static bool IsAdminPage => PageSecurityClass == AdminSecurityClass;

    public static bool IsAdminUser => IsStateAdminUser || IsCountyAdminUser || IsLocalAdminUser;

    public static bool IsAnonymousUser => IsNullOrWhiteSpace(UserSecurityClass);

    public static bool IsCountyAdminPage => PageSecurityClass == AdminSecurityClass &&
      !IsNullOrWhiteSpace(FindStateCode()) &&
      !IsNullOrWhiteSpace(FindCountyCode()) &&
      IsNullOrWhiteSpace(FindLocalKey());

    public static bool IsCountyAdminUser => UserSecurityClass == CountyAdminSecurityClass;

    public static bool IsLocalAdminPage => PageSecurityClass == AdminSecurityClass &&
      !IsNullOrWhiteSpace(FindStateCode()) &&
      !IsNullOrWhiteSpace(FindCountyCode()) &&
      !IsNullOrWhiteSpace(FindLocalKey());

    public static bool IsLocalAdminUser => UserSecurityClass == LocalAdminSecurityClass;

    public static bool IsMasterPage => PageSecurityClass == MasterSecurityClass;

    public static bool IsMasterUser => UserSecurityClass == MasterSecurityClass;
    public static bool IsPartyUser => UserSecurityClass == PartySecurityClass;

    public static bool IsPartyPage => PageSecurityClass == PartySecurityClass;

    public static bool IsPoliticianPage => PageSecurityClass == PoliticianSecurityClass;

    public static bool IsPoliticianUser => UserSecurityClass == PoliticianSecurityClass;

    public static bool IsSecurePage => !IsNullOrWhiteSpace(PageSecurityClass);

    public static bool IsSignedIn => !IsNullOrWhiteSpace(UserName);

    public bool IsSignInPage => this is ISignInPage;

    public static bool IsStateAdminPage => PageSecurityClass == AdminSecurityClass &&
      !IsNullOrWhiteSpace(FindStateCode()) &&
      IsNullOrWhiteSpace(FindCountyCode()) &&
      IsNullOrWhiteSpace(FindLocalKey());

    public static bool IsStateAdminUser => UserSecurityClass == StateAdminSecurityClass;

    public bool IsSuperUserPage => this is ISuperUser;

    public static bool IsSuperUser => GetSessionString("_SuperUser") == "Y";

    public static string PageSecurityClass
    {
      get
      {
        return HttpContext.Current == null
          ? Empty
          : GetSecurityClass(HttpContext.Current.Request.Path);
      }
    }

    public static void LogAdminError(Exception ex, string message = null)
    {
      var logMessage = Empty;
      var stackTrace = Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!IsNullOrWhiteSpace(message))
      {
        if (!IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    public static string UserCountyCode => GetSessionString("_UserCountyCode");

    public static string UserLocalKey => GetSessionString("_UserLocalKey");

    public static string UserPartyKey => GetSessionString("_UserPartyKey");

    public static string UserPoliticianKey => GetSessionString("_PoliticianKey");

    public static string UserStateCode => GetSessionString("_UserStateCode");

    public static string UserSecurityClass => GetSessionString("_UserSecurity");

    #endregion Security Properties

    #endregion Public

    #region Event handlers and overrides

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);
      UrlManager.ForceUsaDomain();
    }

    protected override void OnPreLoad(EventArgs e)
    {
      CheckSecurity();

      base.OnPreLoad(e);
    }

    protected override void OnLoad(EventArgs e)
    {
      var scriptManager = ScriptManager.GetCurrent(this);
      if (scriptManager != null)
        scriptManager.AsyncPostBackError += ScriptManager_AsyncPostBackError;

      InvokeInitializers();

      base.OnLoad(e);
    }

    #endregion Event handlers and overrides
  }

  // marker for SuperUser pages
  public interface ISuperUser
  {
  }

  // marker for SignIn page
  public interface ISignInPage
  {
  }

  // initializer attributes
  [AttributeUsage(AttributeTargets.Class)]
  public class PageInitializersAttribute : Attribute
  {
  }

  [AttributeUsage(AttributeTargets.Class)]
  public class PageInitializerAttribute : Attribute
  {
  }
}