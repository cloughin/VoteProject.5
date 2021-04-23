using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteLog;
using JetBrains.Annotations;
using static System.String;

namespace Vote
{
  public class SecureAdminPage : SecurePage
  {
    #region Private

    public static string GetAdminFolderPageUrl(string pageName,
      params string[] queryParametersAndValues)
    {
      return GetAdminFolderPageUrl(pageName, false, queryParametersAndValues);
    }

    private static string GetAdminFolderPageUrl(string pageName, bool addAdminCodes,
      params string[] queryParametersAndValues)
    {
      IEnumerable<string> queries = queryParametersAndValues;
      if (addAdminCodes)
      {
        var page = GetPage<SecureAdminPage>();
        var list = new List<string>(queryParametersAndValues);
        queries = list;
        list.Add("state", page.StateCode);
        if (page.CountyCodeExists)
          list.Add("county", page.CountyCode);
        if (page.LocalKeyExists)
          list.Add("local", page.LocalKey);
      }
      return UrlManager.ToAdminUrl(QueryStringCollection.FromPairs(queries)
        .AddToPath("/admin/" + pageName + ".aspx"));
    }

    #endregion Private

    #region Protected

    // ReSharper disable VirtualMemberNeverOverriden.Global

    protected void FormatMultiCountiesMessage(HtmlContainerControl control)
    {
      var allCounties =
        LocalIdsCodes.FormatMultiCountyNames(StateCode, LocalKey, true);
      if (!IsNullOrWhiteSpace(allCounties))
      {
        control.RemoveCssClass("hidden");
        control.InnerText = $"Parts of this local district are in {allCounties}";
      }
    }

    protected int FormatOtherCountiesMessage(HtmlContainerControl control)
    {
      var otherCounties =
        LocalIdsCodes.FormatOtherCountyNames(StateCode, CountyCode, LocalKey, out var countyCount, true);
      if (!IsNullOrWhiteSpace(otherCounties))
      {
        control.RemoveCssClass("hidden");
        control.InnerText = $"Parts of this local district are also in {otherCounties}";
      }
      return countyCount;
    }

    protected static void LogElectionsInsert(string electionKey)
    {
      LogDataChange.LogInsert(Elections.TableName, UserName, UserSecurityClass,
        DateTime.UtcNow, electionKey);
    }

    protected static void LogElectionsDataChange(string electionKey, string column,
      string oldValue, string newValue)
    {
      LogDataChange.LogUpdate(Elections.TableName, column, oldValue, newValue,
        UserName, UserSecurityClass, DateTime.UtcNow, electionKey);
    }

    protected static void LogElectionsOfficesDelete(string electionKey, string officeKey)
    {
      LogDataChange.LogDelete(ElectionsOffices.TableName, UserName, UserSecurityClass,
        DateTime.UtcNow, electionKey, officeKey);
    }

    protected static void LogElectionsOfficesInsert(string electionKey, string officeKey)
    {
      LogDataChange.LogInsert(ElectionsOffices.TableName, UserName, UserSecurityClass,
        DateTime.UtcNow, electionKey, officeKey);
    }

    public virtual IEnumerable<string> NonStateCodesAllowed => null;

    public virtual IEnumerable<string> NonStateCodesRequireSuperUser => null;

    // ReSharper restore VirtualMemberNeverOverriden.Global

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    #region Public Properties

    public AdminPageLevel AdminPageLevel { get; private set; }

    public bool CodesAreValid { get; private set; }

    public string CountyCode { get; private set; }

    public bool CountyCodeExists { get; private set; }

    public string LocalKey { get; private set; }

    public bool LocalKeyExists { get; private set; }

    public string StateCode { get; private set; }

    public bool StateCodeExists { get; private set; }

    public bool StateCodeIsNonState { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public static AdminPageLevel GetAdminPageLevel()
    {
      var page = GetPage<SecureAdminPage>();
      return page?.AdminPageLevel ?? AdminPageLevel.Unknown;
    }

    public static string GetBallotPageUrl(string stateCode)
    {
      return GetAdminFolderPageUrl("updatejurisdictions", "state", stateCode) + "#ballot";
    }

    public static string GetBulkEmailPageUrl(string stateCode,
      string countyCode = "", string localKey = "")
    {
      return GetAdminFolderPageUrl("bulkemail", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetCountyCode()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? Empty : page.CountyCode;
    }

    public static string GetDefaultPageUrl(string stateCode,
      string countyCode = "", string localKey = "")
    {
      if (IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("default", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetIssuesPageUrl(string stateCode, [CanBeNull] string issueKey = null,
      [CanBeNull] string questionKey = null, bool? isOmit = null)
    {
      return GetAdminFolderPageUrl("issues", "state", stateCode, "issue", issueKey, 
        "question", questionKey, "omit", isOmit?.ToString());
    }

    public static string GetLocalKey()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? Empty : page.LocalKey;
    }

    public static string GetOfficePageEditUrl(string officeKey)
    {
      return GetAdminFolderPageUrl("updateoffices", "office", officeKey);
    }

    public static string GetOfficePageUrl(string stateCode, OfficeClass officeClass,
      string countyCode = "", string localKey = "")
    {
      return GetAdminFolderPageUrl("office", "state", stateCode,
        "county", countyCode, "local", localKey,
        "class", officeClass.ToInt().ToString(CultureInfo.InvariantCulture));
    }

    public static string GetOfficeCandidateListUrl(string electionKey,
      string officeKey)
    {
      return GetAdminFolderPageUrl("updateelections", "election", electionKey,
        "office", officeKey);
    }

    public static string GetOfficeWinnerPageUrl(string electionKey, string officeKey)
    {
      return GetAdminFolderPageUrl("updateelections", "election", electionKey,
        "office", officeKey) + "#identifywinners";
    }

    public static string GetOfficesPageUrl(string stateCode,
      string countyCode = "", string localKey = "")
    {
      if (IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("offices", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetOfficialsPageUrl(string stateCode,
      string countyCode = "", string localKey = "")
    {
      if (IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("officials", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetPartiesPageUrl([CanBeNull] string stateCode = null,
      [CanBeNull] string partyKey = null, [CanBeNull] string email = null)
    {
      return GetAdminFolderPageUrl("parties", "state", stateCode,
        "party", partyKey, "email", email);
    }

    public static string GetStateCode()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? Empty : page.StateCode;
    }

    public static string GetUpdateJurisdictionsPageUrl([CanBeNull] string stateCode = null,
      [CanBeNull] string countyCode = null, [CanBeNull] string localKey = null)
    {
      return GetAdminFolderPageUrl("updatejurisdictions", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetUpdateElectionsPageUrl([CanBeNull] string stateCode = null,
      string countyCode = "", string localKey = "")
    {
      return GetAdminFolderPageUrl("updateelections", "state", stateCode, "county",
        countyCode, "local", localKey);
    }

    public static string GetUpdateElectionsPageUrlByElection(string electionKey,
      [CanBeNull] string tab = null)
    {
      if (IsNullOrWhiteSpace(electionKey))
        throw new ArgumentException("stateCode is required");
      var url = GetAdminFolderPageUrl("updateelections", "election", electionKey);
      if (!IsNullOrWhiteSpace(tab)) url += "#" + tab;
      return url;
    }

    public static string GetUpdateOfficesPageUrl(string stateCode,
      string countyCode, string localKey, OfficeClass officeClass = OfficeClass.Undefined)
    {
      var oClass = officeClass == OfficeClass.Undefined
        ? Empty
        : officeClass.ToInt().ToString(CultureInfo.InvariantCulture);
      return GetAdminFolderPageUrl("updateoffices", "state", stateCode, "county",
        countyCode, "local", localKey, "class", oClass);
    }

    public static string GetUpdatePoliticiansPageUrl(string stateCode)
    {
      return GetAdminFolderPageUrl("updatepoliticians", "state", stateCode);
    }

    #endregion Public Methods

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected override void OnLoad(EventArgs e)
    {
      // Skip OnLoad processing if invalid codes, handled in OnPreRender
      if (CodesAreValid || IsNullOrWhiteSpace(StateCode) && this is IAllowEmptyStateCode)
        base.OnLoad(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
      base.OnPreLoad(e); 
      StateCode = FindStateCode();
      CountyCode = FindCountyCode();
      LocalKey = FindLocalKey();
      StateCodeExists = StateCache.IsValidStateCode(StateCode);
      if (StateCodeExists)
      {
        StateCodeIsNonState = false;
        if (IsNullOrWhiteSpace(CountyCode))
        {
          AdminPageLevel = AdminPageLevel.State;
          CodesAreValid = true;
        }
        else // we have a CountyCode
        {
          AdminPageLevel = AdminPageLevel.County;
          CountyCodeExists = Counties.StateCodeCountyCodeExists(StateCode,
            CountyCode);
          if (CountyCodeExists)
            if (IsNullOrWhiteSpace(LocalKey))
              CodesAreValid = true;
            else
            {
              AdminPageLevel = AdminPageLevel.Local;
              LocalKeyExists = LocalDistricts.StateCodeLocalKeyExists(StateCode, LocalKey);
              CodesAreValid = LocalKeyExists;
            }
        }
      }
      else
      {
        var nonStateCodesAllowed = NonStateCodesAllowed;
        var nonStateCodesRequireSuperUser = NonStateCodesRequireSuperUser;
        if (nonStateCodesAllowed != null &&
          nonStateCodesAllowed.Contains(StateCode) &&
          (IsSuperUser || nonStateCodesRequireSuperUser == null ||
           !nonStateCodesRequireSuperUser.Contains(StateCode)))
        {
          StateCodeExists = true;
          CodesAreValid = true;
          switch (StateCode)
          {
            case "US":
              AdminPageLevel = AdminPageLevel.President;
              break;

            case "PP":
              AdminPageLevel = AdminPageLevel.PresidentTemplate;
              break;

            case "U1":
            case "U2":
            case "U3":
            case "U4":
              AdminPageLevel = AdminPageLevel.Federal;
              break;

            case "":
            case "LL":
              AdminPageLevel = AdminPageLevel.AllStates;
              break;

            default:
              StateCodeExists = false;
              CodesAreValid = false;
              break;
          }
        }
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      if (CodesAreValid || IsNullOrWhiteSpace(StateCode) && this is IAllowEmptyStateCode)
        return;
      var mainContent = Master.MainContentControl;
      mainContent.Controls.Clear();
      var p = new HtmlP();
      if (IsNullOrWhiteSpace(StateCode))
        p.InnerHtml = "StateCode is missing";
      else if (!StateCodeExists)
        p.InnerHtml = "StateCode [" + StateCode + "] is invalid";
      else if (!CountyCodeExists)
        p.InnerHtml = "CountyCode [" + CountyCode + "] is invalid for state + " +
          StateCode;
      else if (!LocalKeyExists)
        p.InnerHtml = "LocalKey [" + LocalKey + "] is invalid for state + " +
          StateCode + " and county " + CountyCode;
      else
        // should never happen
        p.InnerHtml = "There was a problem interpreting the query string";
      p.Attributes["class"] = "missing-key";
      mainContent.Controls.Add(p);
    }

    #endregion Event handlers and overrides
  }

  public enum AdminPageLevel
  {
    Unknown,
    AllStates,
    President, // US
    PresidentTemplate, // PP
    Federal, // U1..U4
    State,
    County,
    Local
  }

  // marker for empty state code pages
  public interface IAllowEmptyStateCode
  {
  }
}
