using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using DB.Vote;
using static System.String;

namespace Vote
{
  public static class UrlManager
  {
    // This class caches the host name translation information for canonicalization
    // and enables a separate set of "host names" (based on localhost and
    // using the hosts file) for test and debugging.

    #region Private

    #region Private data members

    // For now, we only look at rows with the following DomainOrganizationCode
    //private const string BlessedDomainOrganizationCode = "VoteUSA";

    // This is the machine.config key that controls live/test mode
    private const string UseLiveHostNamesKey = "VoteUseLiveDomains";
    private const string IsStagingKey = "IsStaging";
    private const string IsLiveKey = "IsLive";

    // This is the machine.config key that controls live/test mode
    private const string AdminHostAndPortKey = "AdminHostAndPort";

    // This is the StateCode for all-states
    private const string AllStatesStateCode = "US";

    // We create dictionaries of the relevant host name info, packaged in instances of
    // the following private class
    private class HostNameInfo
    {
      public string Name;
      public string LiveName;
      public bool IsCanonical;
      public string StateCode;
      public string DesignCode;
    }

    // If false, a test server (including port name) is assumed
    private static readonly bool UseLiveHostNames;
    public static readonly bool IsStaging;
    public static readonly bool IsLive;
    //private static readonly int CurrentPort = 80;

    // This dictionary is indexed by host name -- either DomainServerName 
    // or TestServerName (case insensitive)
    private static readonly Dictionary<string, HostNameInfo> HostNameDictionary;

    // This dictionary is indexed by StateCode (case insensitive)
    // and only contains canonical entries.
    private static readonly Dictionary<string, HostNameInfo> StateCodeDictionary;

    // This dictionary is indexed by DomainDesignCode (case insensitive)
    // and only contains canonical entries.
    private static readonly Dictionary<string, HostNameInfo> DesignCodeDictionary;

    // This is the entry for the all-inclusive site name (currently Vote-USA.org)
    private static readonly HostNameInfo CanonicalSiteNameInfo;

    // Cache for CurrentDomainDataCode
    private static readonly Dictionary<string, string> CurrentDomainDataCodeCache;

    // Cache for CurrentDomainOrgainzationCode
    //private static readonly Dictionary<string, string>
    //  CurrentDomainOrganizationCodeCache;

    #endregion Private data members

    #region Static constructor

    static UrlManager()
    {
      // Create the dictionaries
      HostNameDictionary =
        new Dictionary<string, HostNameInfo>(StringComparer.OrdinalIgnoreCase);
      StateCodeDictionary =
        new Dictionary<string, HostNameInfo>(StringComparer.OrdinalIgnoreCase);
      DesignCodeDictionary =
        new Dictionary<string, HostNameInfo>(StringComparer.OrdinalIgnoreCase);
      CurrentDomainDataCodeCache =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      //CurrentDomainOrganizationCodeCache =
      //  new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      // Get the live host name / test host name mode
      if (
        !bool.TryParse(
          WebConfigurationManager.AppSettings[UseLiveHostNamesKey], out UseLiveHostNames))
        UseLiveHostNames = false;
      if (
        !bool.TryParse(
          WebConfigurationManager.AppSettings[IsStagingKey], out IsStaging))
        IsStaging = false;
      if (
        !bool.TryParse(
          WebConfigurationManager.AppSettings[IsLiveKey], out IsLive))
        IsLive = false;

      // AdminHostAndPort is empty if this is the Admin site
      AdminHostAndPort = WebConfigurationManager.AppSettings[AdminHostAndPortKey];


      //if (HttpContext.Current != null)
      //{
      //  var port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
      //  if (!int.TryParse(port, out CurrentPort))
      //    CurrentPort = 80;
      //}

      var table = Domains.GetAllUrlManagerData();
      foreach (var row in table)
        // We only process entries with the blessed DomainOrganizationCode
        // and that have a non-empty TestServer name
        if (/*row.DomainOrganizationCode == BlessedDomainOrganizationCode &&*/
          !IsNullOrWhiteSpace(row.TestServerName))
        {
          // Create a HostNameInfo object
          var hostNameInfo = new HostNameInfo
            {
              Name = UseLiveHostNames ? row.DomainServerName : row.TestServerName,
              LiveName = row.DomainServerName,
              IsCanonical = row.IsCanonical,
              StateCode = row.StateCode,
              DesignCode = row.DomainDesignCode
            };

          if (IsStaging)
          {
            hostNameInfo.LiveName = "stage." + hostNameInfo.LiveName;
            if (UseLiveHostNames)
              hostNameInfo.Name = hostNameInfo.LiveName;
          }

          // Add to the DomainNameDictionary
          if (HostNameDictionary.ContainsKey(hostNameInfo.Name))
            throw new VoteException(
              "UrlManager: duplicate host name \"{0\"", hostNameInfo.Name);
          HostNameDictionary.Add(hostNameInfo.Name, hostNameInfo);

          // If canonical, add to the StateCodeDictionary and DesignCodeDictionary
          if (hostNameInfo.IsCanonical)
          {
            if (StateCodeDictionary.ContainsKey(hostNameInfo.StateCode))
              throw new VoteException(
                "UrlManager: duplicate canonical host name \"{0\" for state \"{1}\"",
                hostNameInfo.Name, hostNameInfo.StateCode);
            StateCodeDictionary.Add(hostNameInfo.StateCode, hostNameInfo);

            if (DesignCodeDictionary.ContainsKey(hostNameInfo.DesignCode))
              throw new VoteException(
                "UrlManager: duplicate canonical host name \"{0\" for design code \"{1}\"",
                hostNameInfo.Name, hostNameInfo.DesignCode);
            DesignCodeDictionary.Add(hostNameInfo.DesignCode, hostNameInfo);
          }
        }

      if (!StateCodeDictionary.ContainsKey(AllStatesStateCode))
        throw new VoteException(
          "UrlManager: there is no canonical host name for the all-states state code (\"{0}\")",
          AllStatesStateCode);
      CanonicalSiteNameInfo = StateCodeDictionary[AllStatesStateCode];
    }

#endregion Static constructor

#region Private methods

    private static string AppendPortNumber(string hostName)
    {
      // after SSL conversion we no longer use port numbers
      //if (!UseLiveHostNames && CurrentPort != 80)
      //  hostName = hostName + ":" +
      //    CurrentPort.ToString(CultureInfo.InvariantCulture);
      return hostName;
    }

    private static bool? _ForceSsl;

    public static bool ForceSsl
    {
      get
      {
        return (_ForceSsl ??
          (_ForceSsl = ConfigurationManager.AppSettings["ForceSsl"] == "true" && ConfigurationManager.AppSettings["IsStaging"] == "false")).Value;
      }
    }

    private static HostNameInfo GetCanonicalHostNameInfo(string hostName)
    {
      var result = CanonicalSiteNameInfo; // if all else fails

      hostName = StripPortNumber(hostName);
      if (HostNameDictionary.TryGetValue(hostName, out var hostNameInfo))
        result = hostNameInfo.IsCanonical
          ? hostNameInfo
          : GetStateHostNameInfo(hostNameInfo.StateCode);

      return result;
    }

    private static string GetDestinationScheme()
    {
      return IsHttpsPage || ForceSsl
        ? Uri.UriSchemeHttps
        : Uri.UriSchemeHttp;
    }

    private static string StripPortNumber(string hostName)
    {
      var index = hostName.IndexOf(':');
      if (index >= 0)
        hostName = hostName.Substring(0, index);
      return hostName;
    }

#endregion Private methods

#endregion Private

#region Public

#region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

#endregion ReSharper disable

#region Methods to return the crawler verification codes

    public static string CurrentLiveName => GetCanonicalLiveHostName(CurrentHostName);

    public static string CurrentBingVerificationCode
    {
      get
      {
        var hostName = GetCanonicalHostName(CurrentHostName);
        return UseLiveHostNames
          ? Domains.GetBingSiteVerificationCodeByDomainServerName(hostName)
          : Domains.GetBingSiteVerificationCodeByTestServerName(hostName);
      }
    }

    public static string CurrentGoogleVerificationCode
    {
      get
      {
        var hostName = GetCanonicalHostName(CurrentHostName);
        return UseLiveHostNames
          ? Domains.GetGoogleSiteVerificationCodeByDomainServerName(hostName)
          : Domains.GetGoogleSiteVerificationCodeByTestServerName(hostName);
      }
    }

    public static string CurrentYahooVerificationCode
    {
      get
      {
        var hostName = GetCanonicalHostName(CurrentHostName);
        return UseLiveHostNames
          ? Domains.GetYahooSiteVerificationCodeByDomainServerName(hostName)
          : Domains.GetYahooSiteVerificationCodeByTestServerName(hostName);
      }
    }

#endregion Methods to return the crawler verification codes

#region Methods and properties to return current request info

    public static string CurrentDomainDataCode
    {
      get
      {
        var hostName = GetCanonicalHostName(CurrentHostName);
        string result;
        lock (CurrentDomainDataCodeCache)
          if (!CurrentDomainDataCodeCache.TryGetValue(hostName, out result))
          {
            var lookupName = hostName;
            if (IsStaging && UseLiveHostNames)
              lookupName = lookupName.Substring(6); // remove stage.
            result = UseLiveHostNames
              ? Domains.GetDomainDataCodeByDomainServerName(lookupName)
              : Domains.GetDomainDataCodeByTestServerName(lookupName);
            CurrentDomainDataCodeCache[lookupName] = result;
          }
        return result;
      }
    }

    public static string CurrentDomainDesignCode => GetCanonicalHostNameInfo(CurrentHostName)
      .DesignCode;

    public static string CurrentHostName => GetCurrentHostName();

    public static string CurrentPath => GetCurrentPath();

    public static Uri CurrentPathUri => GetCurrentPathUri();

    public static QueryStringCollection CurrentQueryCollection => GetCurrentQueryCollection();

    public static string CurrentQuerySiteId => GetCurrentQuerySiteId();

    public static string CurrentQueryString => GetCurrentQueryString();

    public static Uri CurrentSiteUri => GetCurrentSiteUri();

    public static string FindStateCode()
    {
      // This is a replacement for db.State_Code() because in this context
      // we are only dealing with public pages. It is a rework of db.Domain_DataCode_This()
      var stateCode = HttpContext.Current.Request.QueryString["State"];
      if (IsNullOrEmpty(stateCode))
        stateCode = HttpContext.Current.Request.QueryString["Data"];
      if (IsNullOrEmpty(stateCode))
        stateCode = GetStateCodeFromHostName(GetCurrentHostName());
      stateCode = stateCode.ToUpper();
      if (!StateCache.IsValidStateCode(stateCode))
        stateCode = Empty;
      return stateCode;
    }

    public static string GetCurrentHostName()
    {
      if (HttpContext.Current == null) return Empty;
      var host =
        HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLowerInvariant();
      return StripPortNumber(host);
    }

    public static string GetCurrentPath()
    {
      return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
    }

    //public static Uri GetCurrentPathUri()
    //{
    //  return GetCurrentPathUri(null as string);
    //}

    public static Uri GetCurrentPathUri(bool withCurrentQueryString = false)
    {
      return GetCurrentPathUri(withCurrentQueryString ? GetCurrentQueryString() : null);
    }

    public static Uri GetCurrentPathUri(QueryStringCollection qsc)
    {
      return GetCurrentPathUri(qsc.ToString());
    }

    public static Uri GetCurrentPathUri(string query)
    {
      return GetCurrentSiteUri(CurrentPath, query);
    }

    public static QueryStringCollection GetCurrentQueryCollection()
    {
      return QueryStringCollection.Parse(GetCurrentQueryString());
    }

    public static string GetCurrentQuerySiteId()
    {
      if (HttpContext.Current == null) return Empty;
      var site = HttpContext.Current.Request.QueryString["site"] ?? Empty;
      site = site.ToLowerInvariant();
      switch (site)
      {
          // only return valid sites
        case "ivn":
          return site;

        default:
          return Empty;
      }
    }

    public static string GetCurrentQueryString()
    {
      return HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
    }

    public static string GetCurrentScheme()
    {
      return IsHttpsPage
        ? Uri.UriSchemeHttps
        : Uri.UriSchemeHttp;
    }

    public static Uri GetCurrentSiteUri()
    {
      return GetCurrentSiteUri(null, null as string);
    }

    public static Uri GetCurrentSiteUri(string path)
    {
      return GetCurrentSiteUri(path, null as string);
    }

    public static Uri GetCurrentSiteUri(QueryStringCollection qsc)
    {
      return GetCurrentSiteUri(null, qsc.ToString());
    }

    public static Uri GetCurrentSiteUri(string path, QueryStringCollection qsc)
    {
      return GetCurrentSiteUri(path, qsc.ToString());
    }

    public static Uri GetCurrentSiteUri(string path, string query)
    {
      var uriBuilder = new UriBuilder(
        GetDestinationScheme(), CurrentHostName/*, CurrentPort*/);
      if (!IsNullOrWhiteSpace(path))
        uriBuilder.Path = path;
      if (!IsNullOrWhiteSpace(query))
        uriBuilder.Query = query;
      //PropagateCurrentSiteId(uriBuilder);
      return uriBuilder.Uri;
    }

#endregion Methods and properties to return current request info

#region Methods and properties to perform host name canonicalization

    public static string AdminHostAndPort { get; }

    public static string GetCanonicalHostName(string hostName)
    {
      return GetCanonicalHostNameInfo(hostName)
        .Name;
    }

    public static string GetCanonicalLiveHostName(string hostName)
    {
      return GetCanonicalHostNameInfo(hostName)
        .LiveName;
    }

    public static string GetCanonicalHostNameAndPort(string hostName)
    {
      return AppendPortNumber(GetCanonicalHostName(hostName));
    }

    public static bool IsAdminSite
    {
      get { return IsNullOrWhiteSpace(AdminHostAndPort); }
    }

    public static bool IsCanonicalHostName(string hostName)
    {
      var result = false;

      hostName = StripPortNumber(hostName);
      if (HostNameDictionary.TryGetValue(hostName, out var hostNameInfo))
        result = hostNameInfo.IsCanonical;

      return result;
    }

    public static bool IsHttpsPage
    {
      get
      {
        // if we have no HttpContent, treat as https
        var currentContext = HttpContext.Current;
        if (currentContext == null) return true;
        return currentContext.Request.ServerVariables["SERVER_PORT_SECURE"] == "1" ||
          currentContext.Request.Headers["X-Forwarded-Proto"].IsEqIgnoreCase("https");
      }
    }

    public static string ToAdminUrl(string url)
    {
      if (IsAdminSite) return url;
      return $"https://{AdminHostAndPort}{url}";
    }

#endregion Methods and properties to perform host name canonicalization

#region Methods to return url info based on design code

    public static string GetDomainDesignCodeHostName(string designCode)
    {
      var result = DesignCodeDictionary.TryGetValue(designCode, out var hostNameInfo)
        ? hostNameInfo.Name
        : CanonicalSiteNameInfo.Name;

      return result;
    }

    public static string GetDomainDesignCodeHostNameAndPort(string designCode)
    {
      return AppendPortNumber(GetDomainDesignCodeHostName(designCode));
    }

    public static Uri GetDomainDesignCodeUri(string designCode)
    {
      return GetDomainDesignCodeUri(designCode, null, null as string);
    }

    public static Uri GetDomainDesignCodeUri(string designCode, string path)
    {
      return GetDomainDesignCodeUri(designCode, path, null as string);
    }

    public static Uri GetDomainDesignCodeUri(
      string designCode, string path, QueryStringCollection qsc)
    {
      return GetDomainDesignCodeUri(designCode, path, qsc.ToString());
    }

    public static Uri GetDomainDesignCodeUri(
      string designCode, string path, string query)
    {
      var uriBuilder = new UriBuilder(
        GetDestinationScheme(), GetDomainDesignCodeHostName(designCode)/*, CurrentPort*/);
      if (!IsNullOrWhiteSpace(path))
        uriBuilder.Path = path;
      if (!IsNullOrWhiteSpace(query))
        uriBuilder.Query = query;
      //PropagateCurrentSiteId(uriBuilder);
      return uriBuilder.Uri;
    }

#endregion Methods to return url info based on design code

#region Methods to return url info based on state code

    public static string AdminHostName
    {
      get { return "admin." + SiteHostName; }
    }

    public static string AdminHostNameAndPort
    {
      get { return AppendPortNumber(AdminHostName); }
    }

    public static Uri GetAdminUri(string path, string query = null)
    {
      var scheme = SiteHostName.Contains("localhost")
        ? Uri.UriSchemeHttp
        : Uri.UriSchemeHttps;
      var uriBuilder = new UriBuilder(
        scheme, AdminHostName/*, CurrentPort*/);
      if (!IsNullOrWhiteSpace(path))
      {
        if (IsNullOrWhiteSpace(query) && path.Contains("?"))
        {
          var queryPos = path.IndexOf("?", StringComparison.Ordinal);
          query = path.Substring(queryPos + 1);
          path = path.Substring(0, queryPos);
        }
        uriBuilder.Path = path;
      }
      if (!IsNullOrWhiteSpace(query))
        uriBuilder.Query = query;
      //PropagateCurrentSiteId(uriBuilder);
      return uriBuilder.Uri;
    }

    public static string GetStateCodeFromHostName()
    {
      return GetStateCodeFromHostName(GetCurrentHostName());
    }

    public static string GetStateCodeFromHostName(string hostName)
    {
      return GetCanonicalHostNameInfo(hostName)
        .StateCode;
    }

    public static string GetStateHostName(string stateCode)
    {
      return GetStateHostNameInfo(stateCode)
        .Name;
    }

    private static HostNameInfo GetStateHostNameInfo(string stateCode)
    {
      HostNameInfo result;

      if (stateCode != null &&
        StateCodeDictionary.TryGetValue(stateCode, out var hostNameInfo))
        result = hostNameInfo;
      else
        result = CanonicalSiteNameInfo;

      return result;
    }

    public static string GetStateHostNameAndPort(string stateCode)
    {
      return AppendPortNumber(GetStateHostName(stateCode));
    }

    //public static Uri GetStateUri(string stateCode)
    //{
    //  return GetStateUri(stateCode, null, null as string);
    //}

    //public static Uri GetStateUri(string stateCode, string path)
    //{
    //  return GetStateUri(stateCode, path, null as string);
    //}

    //public static Uri GetStateUri(
    //  string stateCode, string path, QueryStringCollection qsc)
    //{
    //  return GetStateUri(stateCode, path, qsc.ToString());
    //}

    //public static Uri GetStateUri(string stateCode, string path, string query)
    //{
    //  var uriBuilder = new UriBuilder(
    //    GetDestinationScheme(), GetStateHostName(stateCode)/*, CurrentPort*/);
    //  if (!IsNullOrWhiteSpace(path))
    //    uriBuilder.Path = path;
    //  if (!IsNullOrWhiteSpace(query))
    //    uriBuilder.Query = query;
    //  //PropagateCurrentSiteId(uriBuilder);
    //  return uriBuilder.Uri;
    //}

#endregion Methods to return url info based on state code

#region Methods and properties to return url info for the primary (all states) domain

    public static Uri GetSiteUri()
    {
      return GetSiteUri(null, null as string);
    }

    public static Uri GetSiteUri(string path)
    {
      return GetSiteUri(path, null as string);
    }

    public static Uri GetSiteUri(string path, QueryStringCollection qsc)
    {
      return GetSiteUri(path, qsc.ToString());
    }

    public static Uri GetSiteUri(string path, string query)
    {
      var uriBuilder = new UriBuilder(
        GetDestinationScheme(), CanonicalSiteNameInfo.Name/*, CurrentPort*/);
      if (!IsNullOrWhiteSpace(path))
        uriBuilder.Path = path;
      if (!IsNullOrWhiteSpace(query))
        uriBuilder.Query = query;
      //PropagateCurrentSiteId(uriBuilder);
      return uriBuilder.Uri;
    }

    public static string SiteHostName
    {
      get { return CanonicalSiteNameInfo.Name; }
    }

    public static string SiteHostNameAndPort
    {
      get { return AppendPortNumber(SiteHostName); }
    }

    public static Uri SiteUri
    {
      get { return GetSiteUri(); }
    }

    #endregion Methods and properties to return url info for the primary (all states) domain

    #region Methods to create canonical urls for public pages

    private static void AddNonEmptyParm(
      NameValueCollection qsc, string name, string value)
    {
      if (!IsNullOrEmpty(value))
        qsc.Add(name, value);
    }

    private static void AddNonNullParm(
      NameValueCollection qsc, string name, string value)
    {
      if (value != null)
        qsc.Add(name, value);
    }

    public static Uri GetAboutUsPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("AboutUs.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("AboutUs.aspx", qsc);
      //return GetStateUri(state, "AboutUs.aspx", qsc);
    }

    public static Uri GetAboutUsPageUri()
    {
      return GetAboutUsPageUri(Empty);
    }

    public static Uri GetBallotPageUri(
      string election, string congress, string stateSenate, string stateHouse, string county, 
      string district, string place, string elementary, string secondary, string unified, 
      string cityCouncil, string countySupervisors, string schoolDistrictDistrict, string sampleBallotEmail)
    {
      var state = Elections.GetStateCodeFromKey(election);
      return GetBallotPageUri(state, election, congress, stateSenate, stateHouse, county, district, 
        place, elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict,
        sampleBallotEmail);
    }

    public static Uri GetBallotPageUri(
      string state, string election, string congress, string stateSenate,
      string stateHouse, string county, string district, string place, string elementary,
      string secondary, string unified, string cityCouncil, string countySupervisors,
      string schoolDistrictDistrict, string sampleBallotEmail)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Congress", congress);
      AddNonEmptyParm(qsc, "StateSenate", stateSenate);
      AddNonEmptyParm(qsc, "StateHouse", stateHouse);
      AddNonEmptyParm(qsc, "County", county);
      AddNonNullParm(qsc, "District", district);
      AddNonNullParm(qsc, "Place", place);
      AddNonEmptyParm(qsc, "Esd", elementary);
      AddNonEmptyParm(qsc, "Ssd", secondary);
      AddNonEmptyParm(qsc, "Usd", unified);
      AddNonEmptyParm(qsc, "Cc", cityCouncil);
      AddNonEmptyParm(qsc, "Cs", countySupervisors);
      AddNonEmptyParm(qsc, "Sdd", schoolDistrictDistrict);
      AddNonEmptyParm(qsc, "sbe", sampleBallotEmail);
      return GetSiteUri("Ballot.aspx", qsc);
      //return GetStateUri(state, "Ballot.aspx", qsc);
    }

    public static Uri GetBallot2PageUri(
      string election, string congress, string stateSenate, string stateHouse, string county,
      string district, string place, string elementary, string secondary, string unified,
      string cityCouncil, string countySupervisors, string schoolDistrictDistrict, string sampleBallotEmail)
    {
      var state = Elections.GetStateCodeFromKey(election);
      return GetBallot2PageUri(state, election, congress, stateSenate, stateHouse, county, district,
        place, elementary, secondary, unified, cityCouncil, countySupervisors, schoolDistrictDistrict,
        sampleBallotEmail);
    }

    public static Uri GetBallot2PageUri(
      string state, string election, string congress, string stateSenate,
      string stateHouse, string county, string district, string place, string elementary,
      string secondary, string unified, string cityCouncil, string countySupervisors,
      string schoolDistrictDistrict, string sampleBallotEmail)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Congress", congress);
      AddNonEmptyParm(qsc, "StateSenate", stateSenate);
      AddNonEmptyParm(qsc, "StateHouse", stateHouse);
      AddNonEmptyParm(qsc, "County", county);
      AddNonNullParm(qsc, "District", district);
      AddNonNullParm(qsc, "Place", place);
      AddNonEmptyParm(qsc, "Esd", elementary);
      AddNonEmptyParm(qsc, "Ssd", secondary);
      AddNonEmptyParm(qsc, "Usd", unified);
      AddNonEmptyParm(qsc, "Cc", cityCouncil);
      AddNonEmptyParm(qsc, "Cs", countySupervisors);
      AddNonEmptyParm(qsc, "Sdd", schoolDistrictDistrict);
      AddNonEmptyParm(qsc, "sbe", sampleBallotEmail);
      return GetSiteUri("Ballot2.aspx", qsc);
    }

    public static Uri GetCompareCandidatesPageUri(
      string state, string election, string office)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Office", office);
      //AddNonEmptyParm(qsc, "Issue", issue);
      var uri = GetSiteUri("CompareCandidates.aspx", qsc);
      //var uri = GetStateUri(state, "CompareCandidates.aspx", qsc);
      //if (!IsNullOrEmpty(issue))
      //  uri = new UriBuilder(uri) {Fragment = "Issue_" + issue}.Uri;
      return uri;
    }

    public static Uri GetCompareCandidatesPageUri(string election, 
      string office)
    {
      return GetCompareCandidatesPageUri(Elections.GetStateCodeFromKey(election),
        election, office);
    }

    public static Uri GetCompareCandidates2PageUri(
      string state, string election, string office)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Office", office);
      //AddNonEmptyParm(qsc, "Issue", issue);
      var uri = GetSiteUri("CompareCandidates2.aspx", qsc);
      //var uri = GetStateUri(state, "CompareCandidates2.aspx", qsc);
      //if (!IsNullOrEmpty(issue))
      //  uri = new UriBuilder(uri) {Fragment = "Issue_" + issue}.Uri;
      return uri;
    }

    public static Uri GetCompareCandidates2PageUri(string election,
      string office)
    {
      return GetCompareCandidates2PageUri(Elections.GetStateCodeFromKey(election),
        election, office);
    }

    public static Uri GetContactUsPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("ContactUs.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("ContactUs.aspx", qsc);
      //return GetStateUri(state, "ContactUs.aspx", qsc);
    }

    public static Uri GetContactUsPageUri()
    {
      return GetContactUsPageUri(Empty);
    }

    //public static Uri GetDefaultPageUri(string state)
    //{
    //  return GetSiteUri();
    //  //return IsNullOrWhiteSpace(state) ? GetSiteUri() : GetStateUri(state);
    //}

    //public static Uri GetDefaultPageUri()
    //{
    //  return GetDefaultPageUri(Empty);
    //}

    //public static Uri GetDefault2PageUri(string state)
    //{
    //  return GetSiteUri("default2.aspx");
    //  //return IsNullOrWhiteSpace(state) ? GetSiteUri("default2.aspx") : GetStateUri(state, "default2.aspx");
    //}

    //public static Uri GetDefault2PageUri()
    //{
    //  return GetDefault2PageUri(Empty);
    //}

    public static Uri GetDonatePageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("Donate.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("Donate.aspx", qsc);
      //return GetStateUri(state, "Donate.aspx", qsc);
    }

    public static Uri GetDonatePageUri()
    {
      return GetDonatePageUri(Empty);
    }


    public static Uri GetElectedPageUri(
      string state, string congress, string stateSenate, string stateHouse,
      string county, string district, string place, string elementary, string secondary, 
      string unified, string cityCouncil, string countySupervisors, string schoolDistrictDistrict)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Congress", congress);
      AddNonEmptyParm(qsc, "StateSenate", stateSenate);
      AddNonEmptyParm(qsc, "StateHouse", stateHouse);
      AddNonEmptyParm(qsc, "County", county);
      AddNonNullParm(qsc, "District", district);
      AddNonNullParm(qsc, "Place", place);
      AddNonEmptyParm(qsc, "Esd", elementary);
      AddNonEmptyParm(qsc, "Ssd", secondary);
      AddNonEmptyParm(qsc, "Usd", unified);
      AddNonEmptyParm(qsc, "Cc", cityCouncil);
      AddNonEmptyParm(qsc, "Cs", countySupervisors);
      AddNonEmptyParm(qsc, "Sdd", schoolDistrictDistrict);
      return GetSiteUri("Elected.aspx", qsc);
      //return GetStateUri(state, "Elected.aspx", qsc);
    }

    public static Uri GetElectionPageUri(string election, bool openAll = false, bool forIFrame = false, bool complete = false)
    {
      var stateCode = Elections.GetStateCodeFromKey(election);
      return GetElectionPageUri(stateCode, election, openAll, forIFrame, complete);
    }

    public static Uri GetElectionPageUri(string state, string election, bool openAll = false, bool forIFrame = false, bool complete = false)
    {
      var page = forIFrame
        ? "ElectionForIFrame.aspx"
        : "Election.aspx";
      if (StateCache.IsValidStateCode(state))
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "State", state);
        AddNonEmptyParm(qsc, "Election", election);
        if (openAll) AddNonEmptyParm(qsc, "openall", "Y");
        if (complete) AddNonEmptyParm(qsc, "complete", "1");
        return GetSiteUri(page, qsc);
        //return GetStateUri(state, page, qsc);
      }
      else
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "Election", election);
        if (openAll) AddNonEmptyParm(qsc, "openall", "Y");
        if (complete) AddNonEmptyParm(qsc, "complete", "1");
        return GetSiteUri(page, qsc);
        //return GetStateUri(SecurePage.FindStateCode(), page, qsc);
      }
    }

    public static Uri GetElection2PageUri(string state, string election, bool openAll = false)
    {
      if (StateCache.IsValidStateCode(state))
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "State", state);
        AddNonEmptyParm(qsc, "Election", election);
        if (openAll) AddNonEmptyParm(qsc, "openall", "Y");
        return GetSiteUri( "Election2.aspx", qsc);
        //return GetStateUri(state, "Election2.aspx", qsc);
      }
      else
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "Election", election);
        if (openAll) AddNonEmptyParm(qsc, "openall", "Y");
        return GetSiteUri("Election2.aspx", qsc);
        //return GetStateUri(SecurePage.FindStateCode(), "Election2.aspx", qsc);
      }
    }

    public static Uri GetForCandidatesPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forCandidates.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forCandidates.aspx", qsc);
      //return GetStateUri(state, "forCandidates.aspx", qsc);
    }

    public static Uri GetForCandidatesPageUri()
    {
      return GetForCandidatesPageUri(Empty);
    }

    public static Uri GetForResearchPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forResearch.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forResearch.aspx", qsc);
      //return GetStateUri(state, "forResearch.aspx", qsc);
    }

    public static Uri GetForResearchPageUri()
    {
      return GetForResearchPageUri(Empty);
    }

    public static Uri GetForPartnersPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forPartners.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forPartners.aspx", qsc);
      //return GetStateUri(state, "forPartners.aspx", qsc);
    }

    public static Uri GetForPartnersPageUri()
    {
      return GetForPartnersPageUri(Empty);
    }

    public static Uri GetForPoliticalPartiesPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forPoliticalParties.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forPoliticalParties.aspx", qsc);
      //return GetStateUri(state, "forPoliticalParties.aspx", qsc);
    }

    public static Uri GetForPoliticalPartiesPageUri()
    {
      return GetForPoliticalPartiesPageUri(Empty);
    }

    public static Uri GetForElectionAuthoritiesPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forElectionAuthorities.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forElectionAuthorities.aspx", qsc);
      //return GetStateUri(state, "forElectionAuthorities.aspx", qsc);
    }

    public static Uri GetForElectionAuthoritiesPageUri()
    {
      return GetForElectionAuthoritiesPageUri(Empty);
    }

    public static Uri GetForVolunteersPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forVolunteers.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forVolunteers.aspx", qsc);
      //return GetStateUri(state, "forVolunteers.aspx", qsc);
    }

    public static Uri GetForVolunteersPageUri()
    {
      return GetForVolunteersPageUri(Empty);
    }

    public static Uri GetForVotersPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("forVoters.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("forVoters.aspx", qsc);
      //return GetStateUri(state, "forVoters.aspx", qsc);
    }

    public static Uri GetForVotersPageUri()
    {
      return GetForVotersPageUri(Empty);
    }

    public static Uri GetIntroPageUri(string id)
    {
      var stateCode = Politicians.GetStateCodeFromKey(id);
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", stateCode);
      AddNonEmptyParm(qsc, "Id", id);
      return GetSiteUri("Intro.aspx", qsc);
      //return GetStateUri(stateCode, "Intro.aspx", qsc);
    }

    public static Uri GetIntro2PageUri(string id)
    {
      var stateCode = Politicians.GetStateCodeFromKey(id);
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", stateCode);
      AddNonEmptyParm(qsc, "Id", id);
      return GetSiteUri("Intro2.aspx", qsc);
      //return GetStateUri(stateCode, "Intro2.aspx", qsc);
    }

    public static Uri GetIssuePageUri(
      string state, string election, string office, string issue)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Office", office);
      AddNonEmptyParm(qsc, "Issue", issue);
      return GetSiteUri("Issue.aspx", qsc);
      //return GetStateUri(state, "Issue.aspx", qsc);
    }

    public static Uri GetIssue2PageUri(
      string state, string election, string office, string issue)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Office", office);
      AddNonEmptyParm(qsc, "Issue", issue);
      return GetSiteUri("Issue2.aspx", qsc);
      //return GetStateUri(state, "Issue2.aspx", qsc);
    }

    public static Uri GetIssue2PageUri(
      string state, string election, string congress, string stateSenate,
      string stateHouse, string county, string office = null)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Congress", congress);
      AddNonEmptyParm(qsc, "StateSenate", stateSenate);
      AddNonEmptyParm(qsc, "StateHouse", stateHouse);
      AddNonEmptyParm(qsc, "County", county);
      AddNonEmptyParm(qsc, "Office", office);
      return GetSiteUri("Issue2.aspx", qsc);
      //return GetStateUri(state, "Issue2.aspx", qsc);
    }

    public static Uri GetIssueListPageUri(string state, string county = null, string local = null)
    {
      if (!StateCache.IsValidStateCode(state) && !StateCache.IsUS(state))
        return GetSiteUri("IssueList.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "County", county);
      AddNonEmptyParm(qsc, "Local", local);
      return GetSiteUri("IssueList.aspx", qsc);
      //return GetStateUri(state, "IssueList.aspx", qsc);
    }

    public static Uri GetOfficialsPageUri(string report, string county = null, string local = null)
    {
      if (report != null && StateCache.IsValidStateCode(report))
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "State", report);
        AddNonEmptyParm(qsc, "Report", report);
        AddNonEmptyParm(qsc, "County", county);
        AddNonEmptyParm(qsc, "Local", local);
        return GetSiteUri("Officials.aspx", qsc);
        //return GetStateUri(report, "Officials.aspx", qsc);
      }
      else
      {
        var qsc = new QueryStringCollection();
        AddNonEmptyParm(qsc, "Report", report);
        return /*HttpContext.Current == null
          ? GetSiteUri("Officials.aspx", qsc)
          : */GetSiteUri("Officials.aspx", qsc);
          //: GetStateUri(SecurePage.FindStateCode(), "Officials.aspx", qsc);
      }
    }

    public static Uri GetPaymentsPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("Payments.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("Payments.aspx", qsc);
      //return GetStateUri(state, "Payments.aspx", qsc);
    }

    public static Uri GetPoliticianIssuePageUri(
      string state, string id, string issue)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Id", id);
      AddNonEmptyParm(qsc, "Issue", issue);
      return GetSiteUri("PoliticianIssue.aspx", qsc);
      //return GetStateUri(state, "PoliticianIssue.aspx", qsc);
    }

    public static Uri GetPublicInterestOrganizationsPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("PublicInterestOrganizations.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("PublicInterestOrganizations.aspx", qsc);
      //return GetStateUri(state, "PublicInterestOrganizations.aspx", qsc);
    }

    public static Uri GetPrivacyPageUri(string state)
    {
      if (!StateCache.IsValidStateCode(state))
        return GetSiteUri("Privacy.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      return GetSiteUri("Privacy.aspx", qsc);
      //return GetStateUri(state, "Privacy.aspx", qsc);
    }

    public static Uri GetPrivacyPageUri()
    {
      return GetPrivacyPageUri(Empty);
    }

    public static Uri GetReferendumPageUri(string election, string referendum)
    {
      var stateCode = Elections.GetStateCodeFromKey(election);
      return GetReferendumPageUri(stateCode, election, referendum);
    }

    public static Uri GetReferendumPageUri(
      string state, string election, string referendum)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Referendum", referendum);
      return GetSiteUri("Referendum.aspx", qsc);
      //return GetStateUri(state, "Referendum.aspx", qsc);
    }

    public static Uri GetReferendum2PageUri(string election, string referendum)
    {
      var stateCode = Elections.GetStateCodeFromKey(election);
      return GetReferendum2PageUri(stateCode, election, referendum);
    }

    public static Uri GetReferendum2PageUri(
      string state, string election, string referendum)
    {
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "State", state);
      AddNonEmptyParm(qsc, "Election", election);
      AddNonEmptyParm(qsc, "Referendum", referendum);
      return GetSiteUri("Referendum2.aspx", qsc);
      //return GetStateUri(state, "Referendum2.aspx", qsc);
    }

    public static Uri GetSampleBallotEnrolledPageUri(string email)
    {
      if (!Validation.IsValidEmailAddress(email))
        return GetSiteUri("SampleBallotEnrollment.aspx");
      var qsc = new QueryStringCollection();
      AddNonEmptyParm(qsc, "Email", email);
      return GetSiteUri("SampleBallotEnrolled.aspx", qsc);
    }

    public static Uri GetSampleBallotEnrollmentPageUri(string email)
    {
      var qsc = new QueryStringCollection();
      if (Validation.IsValidEmailAddress(email))
        AddNonEmptyParm(qsc, "Email", email);
      return GetSiteUri("SampleBallotEnrollment.aspx", qsc);
    }

    //public static Uri PropagateCurrentSiteId(Uri uri)
    //{
    //  var siteId = CurrentQuerySiteId;
    //  if (IsNullOrEmpty(siteId)) return uri;
    //  var ub = new UriBuilder(uri);
    //  //PropagateCurrentSiteId(ub, siteId);
    //  return ub.Uri;
    //}

    //private static void PropagateCurrentSiteId(UriBuilder ub)
    //{
    //  //PropagateCurrentSiteId(ub, CurrentQuerySiteId);
    //}

    //private static void PropagateCurrentSiteId(UriBuilder ub, string siteId)
    //{
    //  if (!IsNullOrEmpty(siteId))
    //  {
    //    var qsc = QueryStringCollection.Parse(ub.Query);
    //    qsc["Site"] = siteId;
    //    ub.Query = qsc.ToString();
    //  }
    //}

#endregion Methods to create canonical urls for public pages

    public static void ForceUsaDomain() 
    {
      if (SecurePage.IsIsolated) return;
      var uri = UrlNormalizer.BuildCurrentUri();
      if (uri.Host.IsNeIgnoreCase(CanonicalSiteNameInfo.Name))
      {
        var builder = new UriBuilder(uri) {Host = CanonicalSiteNameInfo.Name};
        HttpContext.Current.Response.Redirect(builder.ToString(), true);
      }
    }

#region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

#endregion ReSharper restore

#endregion Public
  }
}