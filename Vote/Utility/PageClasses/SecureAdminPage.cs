using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DB.Vote;
using DB.VoteLog;

namespace Vote
{
  public class SecureAdminPage : SecurePage
  {
    #region Private

    private static string GetAdminFolderPageUrl(string pageName,
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
        {
          list.Add("county", page.CountyCode);
          if (page.LocalCodeExists)
            list.Add("local", page.LocalCode);
        }
      }
      return QueryStringCollection.FromPairs(queries)
        .AddToPath("/admin/" + pageName + ".aspx");
    }

    #endregion Private

    #region Protected

    // ReSharper disable VirtualMemberNeverOverriden.Global

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

    internal virtual IEnumerable<string> NonStateCodesAllowed => null;

    internal virtual IEnumerable<string> NonStateCodesRequireSuperUser => null;

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

    public string LocalCode { get; private set; }

    public bool LocalCodeExists { get; private set; }

    public string StateCode { get; private set; }

    public bool StateCodeExists { get; private set; }

    public bool StateCodeIsNonState { get; private set; }

    #endregion Public Properties

    #region Public Methods

    //public static AdminPageCodes GetAdminPageCodes()
    //{
    //  var page = GetPage<SecureAdminPage>();
    //  return page == null ? null : page.AdminPageCodes;
    //}

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
      string countyCode = "", string localCode = "")
    {
      return GetAdminFolderPageUrl("bulkemail", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    public static string GetCountyCode()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? string.Empty : page.CountyCode;
    }

    public static string GetDefaultPageUrl(string stateCode,
      string countyCode = "", string localCode = "")
    {
      if (string.IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("default", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    public static string GetDistrictsPageUrl(string stateCode, string countyCode,
      string localCode)
    {
      return GetAdminFolderPageUrl("districts", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    //public static string GetElectionReportPageUrl(string electionKey)
    //{
    //  var parameters = new List<string> {"election", electionKey};
    //  parameters.AddRange(Elections.GetQueryParametersFromKey(electionKey));
    //  return GetAdminFolderPageUrl("electionreport", parameters.ToArray());
    //}

    //public static string GetElectionOfficesPageUrl(string electionKey)
    //{
    //  var parameters = new List<string> {"election", electionKey};
    //  parameters.AddRange(Elections.GetQueryParametersFromKey(electionKey));
    //  return GetAdminFolderPageUrl("electionoffices", parameters.ToArray());
    //}

    public static string GetLocalCode()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? string.Empty : page.LocalCode;
    }

    //public static string GetOfficePageUrl(string officeKey)
    //{
    //  return GetAdminFolderPageUrl("office", "state",
    //    Offices.GetStateCodeFromKey(officeKey), "office", officeKey);
    //}

    public static string GetOfficePageEditUrl(string officeKey)
    {
      return GetAdminFolderPageUrl("updateoffices", "office", officeKey) /* + "#changeinfo"*/;
    }

    public static string GetOfficePageUrl(string stateCode, OfficeClass officeClass,
      string countyCode = "", string localCode = "")
    {
      return GetAdminFolderPageUrl("office", "state", stateCode,
        "county", countyCode, "local", localCode,
        "class", officeClass.ToInt().ToString(CultureInfo.InvariantCulture));
    }

    public static string GetOfficeCandidateListUrl(string electionKey,
      string officeKey)
    {
      return GetAdminFolderPageUrl("updateelections", "election", electionKey,
        "office", officeKey);
    }

    //public static string GetOfficeIncumbentsPageUrl(string officeKey)
    //{
    //  return GetAdminFolderPageUrl("updateoffices", "office", officeKey, "tab", "addincumbents") + "#addincumbents";
    //}

    public static string GetOfficeWinnerPageUrl(string electionKey, string officeKey)
    {
      return GetAdminFolderPageUrl("updateelections", "election", electionKey,
        "office", officeKey) + "#identifywinners";
    }

    public static string GetOfficialsPageUrl(string stateCode,
      string countyCode = "", string localCode = "")
    {
      if (string.IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("officials", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    public static string GetPoliticianPageUrl(string politicianId)
    {
      //return GetAdminFolderPageUrl("politician", "id", politicianId);
      return SecurePoliticianPage.GetUpdateIntroPageUrl(politicianId);
    }

    //public static string GetPoliticianPageUrlByOfficeKey(string officeKey)
    //{
    //  return GetAdminFolderPageUrl("politician", "office", officeKey);
    //}

    //public static string GetReferendumPageUrl(string electionKey,
    //  string referendumKey)
    //{
    //  var parameters = new List<string>();
    //  parameters.Add("election", electionKey);
    //  parameters.Add("referendum", referendumKey);
    //  parameters.AddRange(Elections.GetQueryParametersFromKey(electionKey));
    //  return GetAdminFolderPageUrl("referendum", parameters.ToArray());
    //}

    public static string GetStateCode()
    {
      var page = GetPage<SecureAdminPage>();
      return page == null ? string.Empty : page.StateCode;
    }

    public static string GetUpdateJurisdictionsPageUrl(string stateCode,
      string countyCode, string localCode)
    {
      return GetAdminFolderPageUrl("updatejurisdictions", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    public static string GetUpdateElectionsPageUrl(string stateCode,
      string countyCode = "", string localCode = "")
    {
      if (string.IsNullOrWhiteSpace(stateCode))
        throw new ArgumentException("stateCode is required");
      return GetAdminFolderPageUrl("updateelections", "state", stateCode, "county",
        countyCode, "local", localCode);
    }

    public static string GetUpdateElectionsPageUrlByElection(string electionKey,
      string tab = null)
    {
      if (string.IsNullOrWhiteSpace(electionKey))
        throw new ArgumentException("stateCode is required");
      var url = GetAdminFolderPageUrl("updateelections", "election", electionKey);
      if (!string.IsNullOrWhiteSpace(tab)) url += "#" + tab;
      return url;
    }

    public static string GetUpdateOfficesPageUrl(string stateCode,
      string countyCode, string localCode)
    {
      return GetAdminFolderPageUrl("updateoffices", "state", stateCode, "county",
        countyCode, "local", localCode);
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
      if (CodesAreValid || (string.IsNullOrWhiteSpace(StateCode) && this is IAllowEmptyStateCode))
        base.OnLoad(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
      base.OnPreLoad(e);
      StateCode = ViewStateStateCode;
      CountyCode = ViewStateCountyCode;
      LocalCode = ViewStateLocalCode.TwoCharLocalCode();
      StateCodeExists = StateCache.IsValidStateCode(StateCode);
      if (StateCodeExists)
      {
        StateCodeIsNonState = false;
        if (string.IsNullOrWhiteSpace(CountyCode))
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
            if (string.IsNullOrWhiteSpace(LocalCode))
              CodesAreValid = true;
            else
            {
              AdminPageLevel = AdminPageLevel.Local;
              LocalCodeExists =
                LocalDistricts.StateCodeCountyCodeLocalCodeExists(StateCode,
                  CountyCode, LocalCode.TwoCharLocalCode());
              CodesAreValid = LocalCodeExists;
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

      if (CodesAreValid || (string.IsNullOrWhiteSpace(StateCode) && this is IAllowEmptyStateCode))
        return;
      var mainContent = Master.MainContentControl;
      mainContent.Controls.Clear();
      var p = new HtmlP();
      if (string.IsNullOrWhiteSpace(StateCode))
        p.InnerHtml = "StateCode is missing";
      else if (!StateCodeExists)
        p.InnerHtml = "StateCode [" + StateCode + "] is invalid";
      else if (!CountyCodeExists)
        p.InnerHtml = "CountyCode [" + CountyCode + "] is invalid for state + " +
          StateCode;
      else if (!LocalCodeExists)
        p.InnerHtml = "LocalCode [" + LocalCode + "] is invalid for state + " +
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
}

// marker for empty state code pages
internal interface IAllowEmptyStateCode
{
}