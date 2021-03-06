using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DB.Vote;
using DB.VoteLog;
using Vote.Controls;
using static System.String;

namespace Vote
{
  public partial class SignInPage : SecurePage, ISignInPage
  {
    #region Private

    private static void CheckForFatalCredentialingProblems(string usernameEntered)
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          // no additional info needed
          break;

        case StateAdminSecurityClass:
          if (!StateCache.IsValidStateCode(UserStateCode))
            HandleCredentialingInconsistency(usernameEntered, "Invalid StateCode");
          break;

        case CountyAdminSecurityClass:
          if (!Counties.StateCodeCountyCodeExists(UserStateCode, UserCountyCode))
            HandleCredentialingInconsistency(usernameEntered, "Invalid CountyCode");
          break;

        case LocalAdminSecurityClass:
          if (!LocalDistricts.StateCodeLocalKeyExists(UserStateCode, UserLocalKey))
            HandleCredentialingInconsistency(usernameEntered, "Invalid LocalKey");
          break;

        case PartySecurityClass:
          if (!Parties.PartyKeyExists(UserPartyKey))
            HandleCredentialingInconsistency(usernameEntered, "Invalid PartyKey");
          break;

        case PoliticianSecurityClass:
          // no additional info needed --
          // they couldn't get this far without a valid politician key
          break;
      }
    }

    private static void CorrectCredentialingSessionVariable(
      string name, IEnumerable<string> exceptions)
    {
      if (!exceptions.Contains(name))
        HttpContext.Current.Session[name] = Empty;
    }

    private static void CorrectCredentialingSessionVariables(
      params string[] exceptions)
    {
      CorrectCredentialingSessionVariable("_PoliticianKey", exceptions);
      CorrectCredentialingSessionVariable("_UserPartyKey", exceptions);
      CorrectCredentialingSessionVariable("_UserStateCode", exceptions);
      CorrectCredentialingSessionVariable("_UserCountyCode", exceptions);
      CorrectCredentialingSessionVariable("_UserLocalKey", exceptions);
      //CorrectCredentialingSessionVariable("_UserIssuesCode", exceptions);
    }

    private static void CorrectCredentialingInconsistencies()
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          CorrectCredentialingSessionVariables();
          break;

        case StateAdminSecurityClass:
          CorrectCredentialingSessionVariables("_UserStateCode");
          break;

        case CountyAdminSecurityClass:
          CorrectCredentialingSessionVariables("_UserStateCode", "_UserCountyCode");
          break;

        case LocalAdminSecurityClass:
          CorrectCredentialingSessionVariables(
            "_UserStateCode", "_UserCountyCode", "_UserLocalKey");
          break;

        case PartySecurityClass:
          CorrectCredentialingSessionVariables("_UserPartyKey");
          break;

        case PoliticianSecurityClass:
          CorrectCredentialingSessionVariables("_PoliticianKey");
          break;
      }
    }

    private static void HandleCredentialingInconsistency(
      string username, string message)
    {
      InitializeSessionVariables();
      LogException("Credentialing", username + ": " + message);
      throw new VoteException(
        "We had a problem checking your sign in " +
          "credentials. Please <a href=\"/Contact.aspx\">contact us</a>.");
    }

    private static void HandleSignin(string usernameEntered, string passwordEntered)
    {
      // a null passwordEntered signals auto login
      var generalPassword = Security.GetUserPassword(usernameEntered);
      var politiciansPassword = Politicians.GetPassword(usernameEntered);
      var partiesPassword = PartiesEmails.GetPartyPassword(usernameEntered);

      // Get an array of all valid (non-null) passwords
      var validPasswords =
        new[] {generalPassword, politiciansPassword, partiesPassword}.Where(
          pw => pw != null)
          .ToArray();

      if (validPasswords.Length > 1)
        // the username appeared in more than one security context
        HandleCredentialingInconsistency(usernameEntered, "Ambiguous usernname");

      var validPassword = passwordEntered == null ||
        validPasswords.Length > 0 && validPasswords[0] == passwordEntered;
      if (validPasswords.Length == 0 || !validPassword)
        throw new VoteException(
          "Your sign in failed. Please check your username and " +
            "password. If you believe they are correct and still cannot sign in, please " +
            "<a href=\"/Contact.aspx\">contact us</a>.");

      // We now base the saved security on the user, not the page requested
      string userSecurityClass;
      if (politiciansPassword != null)
        userSecurityClass = PoliticianSecurityClass;
      else if (partiesPassword != null)
        userSecurityClass = PartySecurityClass;
      else
        userSecurityClass = Security.GetUserSecurity(usernameEntered)
          .ToUpperInvariant();
      var isSuperUser = userSecurityClass == MasterSecurityClass && Security.GetIsSuperUser(usernameEntered, false);

      InitializeSessionVariables();

      SetSessionVariables(usernameEntered, userSecurityClass, isSuperUser);

      CheckForFatalCredentialingProblems(usernameEntered);
      CorrectCredentialingInconsistencies();

      if (passwordEntered != null)
        LogSignIn();

      FormsAuthentication.RedirectFromLoginPage(usernameEntered, false);
    }

    private static void InitializeSessionVariables()
    {
      var session = HttpContext.Current.Session;
      session["_UserName"] = Empty;
      session["_UserSecurity"] = Empty;
      session["_SuperUser"] = Empty;
      session["_UserStateCode"] = Empty;
      session["_UserCountyCode"] = Empty;
      session["_UserLocalKey"] = Empty;
      session["_UserPartyKey"] = Empty;
      session["_PoliticianKey"] = Empty;
    }

    private static void LogSignIn()
    {
      LogLogins.Insert(DateTime.UtcNow, UserName, UserSecurityClass, UserStateCode, UserCountyCode,
        UserLocalKey, Empty, Empty, Empty, UserPartyKey, 
        UserPoliticianKey);
    }

    private static void SetSessionVariables(
      string usernameEntered, string userSecurityClass, bool superUser)
    {
      var session = HttpContext.Current.Session;
      // Note: we fetch the UserName from the db to normalize casing
      session["_UserSecurity"] = userSecurityClass;
      session["_SuperUser"] = superUser ? "Y" : "N";

      switch (userSecurityClass)
      {
        case PoliticianSecurityClass:
          session["_UserName"] = Politicians.GetPoliticianKey(usernameEntered);
          session["_PoliticianKey"] = usernameEntered.ToUpperInvariant();
          break;

        case PartySecurityClass:
          session["_UserName"] = PartiesEmails.GetPartyEmail(usernameEntered);
          session["_UserPartyKey"] = PartiesEmails.GetPartyKey(usernameEntered)
            .ToUpperInvariant();
          break;

        default:
          {
            var row = Security.GetData(usernameEntered)[0];
            session["_UserName"] = Security.GetUserName(usernameEntered);
            session["_UserStateCode"] = row.UserStateCode.ToUpperInvariant();
            session["_UserCountyCode"] = IsNullOrWhiteSpace(row.UserCountyCode)
              ? Empty
              : row.UserCountyCode.ZeroPad(3);
            session["_UserLocalKey"] = IsNullOrWhiteSpace(row.UserLocalKey)
              ? Empty
              : row.UserLocalKey;
          }
          break;
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected override void OnPreInit(EventArgs e)
    {
      base.OnPreInit(e); 
      UrlManager.ForceUsaDomain();
    }

    private void Page_Load(object sender, EventArgs e)
    {
      Master.SetJavascriptNotNeeded();

      if (!IsPostBack)
        try
        {
          if (GetQueryString("signout")
            .IsEqIgnoreCase("Y"))
          {
            FormsAuthentication.SignOut();
            // Get rid of menus
            InitializeSessionVariables();
            Response.Redirect(FormsAuthentication.LoginUrl);
          }

          if (HttpContext.Current.User.Identity.IsAuthenticated && 
            !IsNullOrWhiteSpace(UserSecurityClass) &&
            GetQueryString("Security") != "Y")
            Response.Redirect(FormsAuthentication.GetRedirectUrl(HttpContext.Current.User.Identity.Name, true));

          Page.Title = "Sign In";

          Page.IncludeCss("/css/vote/SignIn.css", "if IE 7", "ie7");

          if (Request.QueryString["Timeout"] == "Y")
          {
            InfoMessage1.InnerText =
              "You were signed out due to inactivity. Please sign in again. ";
            InfoMessage1.AddCssClasses("warn");
          }
          else if (Request.QueryString["Security"] == "Y")
          {
            InfoMessage1.InnerText =
              "Your sign in does not allow access to the page you requested.";
            InfoMessage1.AddCssClasses("error");
            InfoMessage2.InnerText =
              "If you have different credentials you can use them to sign in again.";
          }

          Session.Timeout = 60;

          var autoLoginUserName = Session["AutoLoginUserName"] as string;
          if (!IsNullOrEmpty(autoLoginUserName))
            //Session.Remove("AutoLoginUserName");
            try
            {
              HandleSignin(autoLoginUserName, null);
            }
            catch
            {
              // ignored
              // if it doesn't redirect, fall thru
            }
        }
        catch (Exception ex)
        {
          SignInFeedback.AddError("Your signin failed due to an unexpected error.");
          SignInFeedback.HandleException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
      // If we have a security violation page, modify the message if there
      // are menu selections available.
      if (!IsPostBack && Request.QueryString["Security"] == "Y" && Master.HasMenu)
        InfoMessage2.InnerText += " Or make a selection from the menu on this page.";

      base.OnPreRender(e);
    }

    protected void ButtonSignin_Click(object sender, EventArgs e)
    {
      try
      {
        FeedbackContainerControl.ClearValidationErrors(
          TextBoxUserName, TextBoxPassword);
        SignInFeedback.ValidateRequired(TextBoxUserName, "Username", out _);
        SignInFeedback.ValidateRequired(TextBoxPassword, "Password", out _);

        if (SignInFeedback.ValidationErrorCount != 0) return;
        var usernameEntered = TextBoxUserName.Text.Trim();
        var passwordEntered = TextBoxPassword.Text.Trim();

        HandleSignin(usernameEntered, passwordEntered);
      }
      catch (VoteException ex)
      {
        SignInFeedback.AddError(ex.Message);
      }
      catch (Exception ex)
      {
        SignInFeedback.AddError("Your signin failed due to an unexpected error.");
        SignInFeedback.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}