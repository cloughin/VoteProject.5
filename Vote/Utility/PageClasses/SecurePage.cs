using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Windows.Forms;
using DB.Vote;
using Vote.Controls;
using Vote.Master;

namespace Vote
{
  //  For new-style admin pages. Subclassed for various
  //  security levels.
  //
  public partial class SecurePage : VotePage
  {
    #region Private

    private OfficesRow _OfficeKeyInfo;

    private void GetOfficeKeyInfo(string officeKey)
    {
      if ((_OfficeKeyInfo != null) && (_OfficeKeyInfo.OfficeKey == officeKey)) return;
      var table = Offices.GetKeyInfoData(officeKey);
      _OfficeKeyInfo = table.Count == 1 ? table[0] : null;
    }

    private static string GetStateCodeFromQueryString()
    {
      var result = QueryState;
      if (!string.IsNullOrWhiteSpace(result)) return result;
      result = Politicians.GetStateCodeFromKey(QueryId);
      if (!string.IsNullOrWhiteSpace(result)) return result;
      result = Elections.GetStateCodeFromKey(QueryElection);
      return !string.IsNullOrWhiteSpace(result)
        ? result
        : Offices.GetStateCodeFromKey(QueryOffice);
    }

    private string GetCountyCodeFromQueryString()
    {
      var result = QueryCounty;
      if (!string.IsNullOrWhiteSpace(result)) return result;
      result = Elections.GetCountyCodeFromKey(QueryElection);
      if (!string.IsNullOrWhiteSpace(result)) return result;
      var officeKey = QueryOffice;
      if (string.IsNullOrWhiteSpace(officeKey)) return string.Empty;
      GetOfficeKeyInfo(officeKey);
      return _OfficeKeyInfo == null ? string.Empty : _OfficeKeyInfo.CountyCode;
    }

    private string GetLocalCodeFromQueryString()
    {
      var result = QueryLocal;
      if (!string.IsNullOrWhiteSpace(result)) return result;
      result = Elections.GetLocalCodeFromKey(QueryElection);
      if (!string.IsNullOrWhiteSpace(result)) return result;
      var officeKey = QueryOffice;
      if (string.IsNullOrWhiteSpace(officeKey)) return string.Empty;
      GetOfficeKeyInfo(officeKey);
      return _OfficeKeyInfo == null ? string.Empty : _OfficeKeyInfo.LocalCode;
    }

    private void PrePopulateViewState()
    {
      switch (PageSecurityClass)
      {
        case MasterSecurityClass:
          break;

        case AdminSecurityClass:
          switch (UserSecurityClass)
          {
            case StateAdminSecurityClass:
              ViewState["_StateCode"] = UserStateCode;
              ViewState["_CountyCode"] = GetCountyCodeFromQueryString();
              ViewState["_LocalCode"] = string.IsNullOrWhiteSpace(QueryCounty)
                ? string.Empty
                : GetLocalCodeFromQueryString();
              break;

            case CountyAdminSecurityClass:
              ViewState["_StateCode"] = UserStateCode;
              ViewState["_CountyCode"] = UserCountyCode;
              ViewState["_LocalCode"] = GetLocalCodeFromQueryString();
              break;

            case LocalAdminSecurityClass:
              ViewState["_StateCode"] = UserStateCode;
              ViewState["_CountyCode"] = UserCountyCode;
              ViewState["_LocalCode"] = UserLocalCode;
              break;

            default:
              ViewState["_StateCode"] = GetStateCodeFromQueryString();
              ViewState["_CountyCode"] = GetCountyCodeFromQueryString();
              ViewState["_LocalCode"] = string.IsNullOrWhiteSpace(QueryCounty)
                ? string.Empty
                : GetLocalCodeFromQueryString();
              break;
          }
          break;

        case PartySecurityClass:
          if (!string.IsNullOrWhiteSpace(QueryParty)) ViewState["_PartyKey"] = QueryParty;
          else ViewState["_PartyKey"] = UserPartyKey;
          break;

        case PoliticianSecurityClass:
          if (!string.IsNullOrWhiteSpace(QueryId)) ViewState["_PoliticianKey"] = QueryId;
          else ViewState["_PoliticianKey"] = UserPoliticianKey;
          break;

        case DesignSecurityClass:
          if (!string.IsNullOrWhiteSpace(QueryDesign)) ViewState["_DesignCode"] = QueryDesign;
          else ViewState["_DesignCode"] = UserDesignCode;
          break;

        case OrganizationSecurityClass:
          if (!string.IsNullOrWhiteSpace(QueryOrganization))
            ViewState["_QueryOrganization"] = QueryOrganization;
          else ViewState["_QueryOrganization"] = UserOrganizationCode;
          break;
      }
    }

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

          case DesignSecurityClass:
            if (IsDesignUser) return new[] {"design"};
            break;

          case OrganizationSecurityClass:
            if (IsOrganizationUser) return new[] {"organization"};
            break;
        }

        return null;
      }
    }

    private void RedirectIfSuperfluousQueryString()
    {
      var hasQueryString =
        !string.IsNullOrWhiteSpace(Request.QueryString.ToString());
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
      while ((pageType != null) && (pageType != typeof (SecurePage)))
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

    protected static FeedbackContainerControl LoadFeedbackContainerControl()
    {
      var page = GetPage<Page>();
      return
        page.LoadControl("/controls/feedbackcontainer.ascx") as
          FeedbackContainerControl;
    }

    public new AdminMaster Master => base.Master as AdminMaster;

    protected virtual void SecurityExceptionHandler()
    {
      // To give subclasses an opportunity to override, since
      // HandleSecurityException is static
      HandleSecurityException();
    }

    protected virtual void ScriptManager_AsyncPostBackError(object sender,
      AsyncPostBackErrorEventArgs e)
    {
      LogException("AsyncPostBackError", e.Exception);
    }

    internal static void ThrowRandomException()
    {
#if DEBUG
      // use ?test=anything to test exception handling during update
      if (!string.IsNullOrWhiteSpace(GetQueryString("testex")))
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

    public enum UpdateStatus
    {
      Unchanged,
      Success,
      Failure
    }

    public const string MasterSecurityClass = "MASTER";
    public const string AdminSecurityClass = "ADMIN";
    public const string StateAdminSecurityClass = "ADMIN";
    public const string CountyAdminSecurityClass = "COUNTY";
    public const string LocalAdminSecurityClass = "LOCAL";
    public const string PartySecurityClass = "PARTY";
    public const string PoliticianSecurityClass = "POLITICIAN";
    public const string DesignSecurityClass = "DESIGN";
    public const string OrganizationSecurityClass = "ORGANIZATION";

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
          while ((wordsBeforeBreak < words.Length) &&
          (TextRenderer.MeasureText(candidate, font)
            .Width <= maxPixelWidth))
          {
            wordsBeforeBreak++;
            if (wordsBeforeBreak < words.Length) candidate += " " + words[wordsBeforeBreak];
          }
          if (wordsBeforeBreak < words.Length)
            output = string.Join(" ", words.Take(wordsBeforeBreak)) + "<br />" +
              string.Join(" ", words.Skip(wordsBeforeBreak));
          else output = candidate;
        }
      }
      else output = string.Empty;

      return output;
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

    public static string CreateUniquePassword()
    {
      var password = string.Empty;
      //Make a AAADDD password
      for (var n = 0; n < 3; n++) password += GetRandomAlpha24();
      for (var n = 0; n < 3; n++) password += GetRandomDigit();
      return password;
    }

    public static string GetSecurityClass(string path)
    {
      var match = Regex.Match(path, @"^/(?<folder>[a-z0-9]+)/",
        RegexOptions.IgnoreCase);
      return match.Success
        ? match.Groups["folder"].Captures[0].Value.ToUpperInvariant()
        : string.Empty;
    }

    public bool GetUserSecurityIsOk()
    {
      var ok = IsSuperUser || ((UserSecurityClass == MasterSecurityClass) && !IsSuperUserPage);

      if (!ok)
        switch (PageSecurityClass)
        {
          case StateAdminSecurityClass:
            switch (UserSecurityClass)
            {
              case StateAdminSecurityClass:
                ok = ViewStateStateCode == UserStateCode;
                break;

              case CountyAdminSecurityClass:
                ok = (ViewStateStateCode == UserStateCode) &&
                  (ViewStateCountyCode == UserCountyCode);
                break;

              case LocalAdminSecurityClass:
                ok = (ViewStateStateCode == UserStateCode) &&
                  (ViewStateCountyCode == UserCountyCode) &&
                  (ViewStateLocalCode == UserLocalCode);
                break;
            }
            break;

          case PartySecurityClass:
            ok = IsPartyUser && (ViewStatePartyKey == UserPartyKey);
            break;

          case PoliticianSecurityClass:
            ok = ViewStatePoliticianKey == UserPoliticianKey;
            // allow access to the party's politician pages
            if (!ok && IsPartyUser)
            {
              var partyKey = Politicians.GetPartyKey(ViewStatePoliticianKey);
              if (!string.IsNullOrWhiteSpace(partyKey)) ok = partyKey.IsEqIgnoreCase(UserPartyKey);
            }
            break;

          case DesignSecurityClass:
            ok = IsDesignUser && (ViewStateDesignCode == UserDesignCode);
            break;

          case OrganizationSecurityClass:
            ok = IsOrganizationUser &&
              (ViewStateOrganizationCode == UserOrganizationCode);
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

          case DesignSecurityClass:
            isSuperfluous = IsDesignUser;
            break;

          case OrganizationSecurityClass:
            isSuperfluous = IsOrganizationUser;
            break;
        }
        return isSuperfluous;
      }
    }

    public static void RedirectToSignInPage(string returnUrl, string queryString)
    {
      var signInUrl = FormsAuthentication.LoginUrl;

      if (!string.IsNullOrWhiteSpace(queryString))
      {
        signInUrl += signInUrl.IndexOf('?') < 0 ? '?' : '&';
        if (queryString.StartsWith("?", StringComparison.Ordinal))
          queryString = queryString.Substring(1);
        signInUrl += queryString;
      }

      if (!string.IsNullOrWhiteSpace(returnUrl))
      {
        signInUrl += signInUrl.IndexOf('?') < 0 ? '?' : '&';
        signInUrl += "ReturnUrl=" + HttpUtility.HtmlEncode(returnUrl);
      }

      HttpContext.Current.Response.Redirect(signInUrl);
    }

    public static void RedirectToSignInPage(string returnUrl,
      QueryStringCollection qsc)
    {
      string queryString = null;
      if ((qsc != null) && (qsc.Count > 0)) queryString = qsc.ToString();
      RedirectToSignInPage(returnUrl, queryString);
    }

    public static void RedirectToSignInPage(string returnUrl)
    {
      RedirectToSignInPage(returnUrl, null as string);
    }

    public static void RedirectToSignInPage(QueryStringCollection qsc)
    {
      RedirectToSignInPage(null, qsc);
    }

    public static void RedirectToSignInPage()
    {
      RedirectToSignInPage(null, null as string);
    }

    #region Security Properties

    public static bool IsAdminPage => PageSecurityClass == AdminSecurityClass;

    public static bool IsAdminUser => IsStateAdminUser || IsCountyAdminUser || IsLocalAdminUser;

    public static bool IsAnonymousUser => string.IsNullOrWhiteSpace(UserSecurityClass);

    public static bool IsCountyAdminPage => (PageSecurityClass == AdminSecurityClass) &&
      !string.IsNullOrWhiteSpace(QueryState) &&
      !string.IsNullOrWhiteSpace(QueryCounty) &&
      string.IsNullOrWhiteSpace(QueryLocal);

    public static bool IsCountyAdminUser => UserSecurityClass == CountyAdminSecurityClass;

    public static bool IsDesignPage => PageSecurityClass == DesignSecurityClass;

    public static bool IsDesignUser => UserSecurityClass == DesignSecurityClass;

    public static bool IsLocalAdminPage => (PageSecurityClass == AdminSecurityClass) &&
      !string.IsNullOrWhiteSpace(QueryState) &&
      !string.IsNullOrWhiteSpace(QueryCounty) &&
      !string.IsNullOrWhiteSpace(QueryLocal);

    public static bool IsLocalAdminUser => UserSecurityClass == LocalAdminSecurityClass;

    public static bool IsMasterPage => PageSecurityClass == MasterSecurityClass;

    public static bool IsMasterUser => UserSecurityClass == MasterSecurityClass;

    public static bool IsOrganizationPage => PageSecurityClass == OrganizationSecurityClass;

    public static bool IsOrganizationUser => UserSecurityClass == OrganizationSecurityClass;

    public static bool IsPartyUser => UserSecurityClass == PartySecurityClass;

    public static bool IsPartyPage => PageSecurityClass == PartySecurityClass;

    public static bool IsPoliticianPage => PageSecurityClass == PoliticianSecurityClass;

    public static bool IsPoliticianUser => UserSecurityClass == PoliticianSecurityClass;

    public static bool IsSecurePage => !string.IsNullOrWhiteSpace(PageSecurityClass);

    public static bool IsSignedIn => !string.IsNullOrWhiteSpace(UserName);

    public bool IsSignInPage => this is ISignInPage;

    public static bool IsStateAdminPage => (PageSecurityClass == AdminSecurityClass) &&
      !string.IsNullOrWhiteSpace(QueryState) &&
      string.IsNullOrWhiteSpace(QueryCounty) &&
      string.IsNullOrWhiteSpace(QueryLocal);

    public static bool IsStateAdminUser => UserSecurityClass == StateAdminSecurityClass;

    public bool IsSuperUserPage => this is ISuperUser;

    public static bool IsSuperUser => GetSessionString("_SuperUser") == "Y";

    public static string PageSecurityClass => HttpContext.Current == null
      ? string.Empty
      : GetSecurityClass(HttpContext.Current.Request.Path);

    public static string UserDesignCode => GetSessionString("_UserDesignCode");

    public static string UserCountyCode => GetSessionString("_UserCountyCode");

    public static string UserIssuesCode => GetSessionString("_UserIssuesCode");

    public static string UserLocalCode => GetSessionString("_UserLocalCode");

    public static string UserOrganizationCode => GetSessionString("_UserOrganizationCode");

    public static string UserPartyKey => GetSessionString("_UserPartyKey");

    public static string UserPoliticianKey => GetSessionString("_PoliticianKey");

    public static string UserStateCode => GetSessionString("_UserStateCode");

    public static string UserSecurityClass => GetSessionString("_UserSecurity");

    #endregion Security Properties

    #region ViewState properties and methods

    public static string GetViewStateCountyCode()
    {
      var vs = GetViewState();
      return vs?["_CountyCode"] as string;
    }

    public static string GetViewStateDesignCode()
    {
      var vs = GetViewState();
      return vs?["_DesignCode"] as string;
    }

    public static string GetViewStateLocalCode()
    {
      var vs = GetViewState();
      return vs?["_LocalCode"] as string;
    }

    public static string GetViewStateOrganizationCode()
    {
      var vs = GetViewState();
      return vs?["_OrganizationCode"] as string;
    }

    public static string GetViewStatePartyKey()
    {
      var vs = GetViewState();
      return vs?["_PartyKey"] as string;
    }

    public static string GetViewStatePoliticianKey()
    {
      var vs = GetViewState();
      return vs?["_PoliticianKey"] as string;
    }

    public static string GetViewStateStateCode()
    {
      var vs = GetViewState();
      return vs?["_StateCode"] as string;
    }

    public string ViewStateCountyCode => (ViewState["_CountyCode"] as string).SafeString();

    public string ViewStateDesignCode => (ViewState["_DesignCode"] as string).SafeString();

    public string ViewStateLocalCode => (ViewState["_LocalCode"] as string).SafeString();

    public string ViewStateOrganizationCode
      => (ViewState["_OrganizationCode"] as string).SafeString();

    public string ViewStatePartyKey => (ViewState["_PartyKey"] as string).SafeString();

    public string ViewStatePoliticianKey => (ViewState["_PoliticianKey"] as string).SafeString();

    public string ViewStateStateCode => (ViewState["_StateCode"] as string).SafeString();

    #endregion ViewState properties and methods

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e);
      UrlManager.ForceUsaDomain();
    }

    protected override void OnPreLoad(EventArgs e)
    {
      if (!IsPostBack) PrePopulateViewState();
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
  internal interface ISuperUser
  {
  }

  // marker for SignIn page
  internal interface ISignInPage
  {
  }

  // initializer attributes
  [AttributeUsage(AttributeTargets.Class)]
  internal class PageInitializersAttribute : Attribute
  {
  }

  [AttributeUsage(AttributeTargets.Class)]
  internal class PageInitializerAttribute : Attribute
  {
  }
}