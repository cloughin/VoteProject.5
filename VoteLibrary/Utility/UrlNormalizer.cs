using System;
//using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Web;
using DB.Vote;
using JetBrains.Annotations;
using static System.String;

namespace Vote
{
  // The UriNormalizer class analyzes a url to determine if it is 
  // canonical (or canonical enough to keep search engines from 
  // flagging duplicate content). This logic was originally 
  // packaged as a single method tree. It was repackaged as a
  // class to facilitate decomposition.
  //
  // This version has a single public method with two overloads:
  // 
  //   public bool Normalize()
  //   public bool Normalize(uri originalUri)
  //
  // The first overload duplicates the functionality of the original,
  // using the current request to construct the originalUri.
  //
  // The second overload operates on a supplied url, independent of 
  // the current http context. It allows the class to be bulk
  // tested using the urls accumulated in the log files.
  //
  // The boolean result indicates success (true) or failure (false)
  // of the normalization. The following properties are then used to
  // obtain the specific result:
  //
  //   public string ErrorMessage { get; }
  //   public Uri NormalizedUri { get; }
  //
  // In the case of success, the NormalizedUri indicates whether
  // a redirect is necessary. If null, no redirect is necessary.
  // If non-null, the NormalizedUri gives the redirect target.
  //
  // In the case of failure, the ErrorMessage property gives an
  // explanation of the failure.
  //
  // Note that this class does no logging. The logging 
  // functionality is provided in a wrapper method in the db 
  // class with the same name as the original method (although 
  // the return type is now Uri):
  //
  //   public static Uri Url_Redirect_And_Log()
  //
  // Modified to pass through the NoCache and browser cache defeat
  // parameters. (3/2013)

  public class UrlNormalizer
  {
    #region legacy

    public static ElectoralClass Electoral_Class_Election(string electionKey)
    {
      switch (electionKey.Length)
      {
        case Elections.ElectionKeyLengthStateOrFederal:
          return Offices.GetElectoralClass(Offices.GetStateCodeFromKey(electionKey), Empty,
            Empty);
        case Elections.ElectionKeyLengthCounty:
          return ElectoralClass.County;
        case Elections.ElectionKeyLengthLocal:
          return ElectoralClass.Local;
        default:
          return ElectoralClass.Unknown;
      }
    }

    #endregion legacy

    #region Private data members

    // Private members (partially) exposed by public properties
    private bool _IsCanonicalUsa;

    // Completely private members
    private QueryStringCollection _OriginalQueryCollection;

    #endregion Private data members

    #region Private methods

    private Uri AdjustCanonicalUri(Uri uri)
    {
      uri = GetCanonicalPath(uri);
      // For info pages, use the site domain
      if (_IsCanonicalUsa)
      {
        var query = uri.Query;
        if (query.StartsWith("?", StringComparison.Ordinal))
          query = query.Substring(1);
        uri = UrlManager.GetSiteUri(uri.AbsolutePath, query);
      }
      else if (IsUsaPage(uri)) // eliminate query string
        uri = UrlManager.GetSiteUri(uri.AbsolutePath);
      else
        uri = CheckForPresidentialPrimary(uri);

      // If there's a site parameter, remove it
      if (!IsNullOrEmpty(UrlManager.CurrentQuerySiteId))
      {
        var ub = new UriBuilder(uri);
        var qsc = QueryStringCollection.Parse(ub.Query);
        qsc.Remove("site");
        ub.Query = qsc.ToString();
        uri = ub.Uri;
      }

      if (UrlManager.ForceSsl && uri.Scheme != Uri.UriSchemeHttps)
        uri = new UriBuilder(uri) {Scheme = Uri.UriSchemeHttps, Port = 443}.Uri;

      return uri;
    }

    public static Uri BuildCurrentUri()
    {
      var ub = new UriBuilder();
      var serverVariables = HttpContext.Current.Request.ServerVariables;
      ub.Scheme = UrlManager.GetCurrentScheme();
      var hostAndPort = serverVariables["HTTP_HOST"];
      var colonIndex = hostAndPort.IndexOf(':');
      if (colonIndex < 0) // no port, let it default
        ub.Host = hostAndPort;
      else
      {
        ub.Host = hostAndPort.Substring(0, colonIndex);
        ub.Port = int.Parse(hostAndPort.Substring(colonIndex + 1));
      }
      ub.Path = serverVariables["SCRIPT_NAME"];
      ub.Query = serverVariables["QUERY_STRING"];
      return ub.Uri;
    }

    private static Uri CheckForPresidentialPrimary(Uri uri)
    {
      var path = uri.AbsolutePath.ToLower();
      if (path == "/election.aspx" || path == "/election2.aspx" || 
        path == "/electionforiframe.aspx" || path == "/issue.aspx" || path == "/issue2.aspx")
      {
        var qsc = QueryStringCollection.Parse(uri.Query);
        var newElectionKey = MemCache.GetCanonicalElectionKey(qsc["election"]);
        // if we have a canonical key, substitute it 
        // and switch to the non-state domain
        if (!IsNullOrWhiteSpace(newElectionKey))
        {
          qsc["election"] = newElectionKey;
          // Remove state from query string
          qsc.Remove("state");
          uri = UrlManager.GetSiteUri(path, qsc);
        }
      }
      return uri;
    }

    [NotNull]
    private string FixElectionKeyFromQueryString()
    {
      if (IsNullOrEmpty(GetQueryParm("Election")))
        return GetLatestViewableElectionKey();

      var electionKeyFixed = GetQueryParm("Election");

      return IsValidElectionKey(electionKeyFixed)
        ? electionKeyFixed
        : GetLatestViewableElectionKey();
    }

    private static Uri GetCanonicalPath(Uri uri)
    {
      var canonicalPath = uri.AbsolutePath.ToLower();
      switch (canonicalPath)
      {
        case "/default.aspx":
          canonicalPath = "/";
          break;
      }

      var ub = new UriBuilder(uri) {Path = canonicalPath};
      return ub.Uri;
    }

    private string GetLatestViewableElectionKey()
    {
      if (!IsNullOrEmpty(GetQueryParm("Office")))
        return
          ElectionsOffices.GetLatestViewableElectionKeyStateByOfficeKey(
            GetQueryParm("Office"), Empty);

      return
        Elections.GetLatestViewableElectionKeyByStateCode(
          UrlManager.FindStateCode(), Empty);
    }

    private QueryStringCollection GetOriginalQueryCollectionCopy()
    {
      return QueryStringCollection.Parse(OriginalUri.Query);
    }

    [NotNull]
    private string GetQueryParm(string name)
    {
      var value = _OriginalQueryCollection[name];
      return value ?? Empty;
    }

    private void HandleBallotPage()
    {
      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();

      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var path = OriginalUri.AbsolutePath.ToLower();
      if (StateCache.IsValidStateCode(stateCode) && !IsNullOrEmpty(electionKey))
        NormalizedUri = path == "/ballot2.aspx"
          ? UrlManager.GetBallot2PageUri(stateCode, electionKey,
            GetQueryParm("Congress"), GetQueryParm("StateSenate"),
            GetQueryParm("StateHouse"), GetQueryParm("County"),
            GetQueryParm("District"), GetQueryParm("Place"),
            GetQueryParm("Esd"), GetQueryParm("Ssd"), GetQueryParm("Usd"),
            GetQueryParm("Cc"), GetQueryParm("Cs"), GetQueryParm("Sdd"), null)
          : UrlManager.GetBallotPageUri(stateCode, electionKey,
            GetQueryParm("Congress"), GetQueryParm("StateSenate"),
            GetQueryParm("StateHouse"), GetQueryParm("County"),
            GetQueryParm("District"), GetQueryParm("Place"),
            GetQueryParm("Esd"), GetQueryParm("Ssd"), GetQueryParm("Usd"),
            GetQueryParm("Cc"), GetQueryParm("Cs"), GetQueryParm("Sdd"), null);
      else
        ErrorMessage =
          "Invalid_ElectionKey|State|Congress|StateSenate|StateHouse|County";
    }

    private void HandleCompareCandidatesPage()
    {
      var officeKey = Empty;

      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();

      if (!IsNullOrEmpty(electionKey))
      {
        if (Offices.IsInElection(GetQueryParm("Office"), electionKey))
          officeKey = GetQueryParm("Office");
      }

      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var path = OriginalUri.AbsolutePath.ToLower();
      if (!IsNullOrEmpty(electionKey) && !IsNullOrEmpty(officeKey))
      {
        NormalizedUri = path == "/comparecandidates2.aspx"
          ? UrlManager.GetCompareCandidates2PageUri(stateCode, electionKey,
            officeKey) 
          : UrlManager.GetCompareCandidatesPageUri(stateCode, electionKey,
            officeKey);
      }
      else
        ErrorMessage =
          "Cannot find this combination of State, Election and Office";
    }

    private void HandleElectedPage()
    {
      var stateCode = UrlManager.FindStateCode();
      if (StateCache.IsValidStateCode(stateCode))
        NormalizedUri = UrlManager.GetElectedPageUri(stateCode,
          GetQueryParm("Congress"), GetQueryParm("StateSenate"),
          GetQueryParm("StateHouse"), GetQueryParm("County"), 
          GetQueryParm("District"), GetQueryParm("Place"),
          GetQueryParm("Esd"), GetQueryParm("Ssd"), GetQueryParm("Usd"), 
          GetQueryParm("Cc"), GetQueryParm("Cs"), GetQueryParm("Sdd"));
      else
        ErrorMessage = "Invalid_StateCode";
    }

    private void HandleElectionPage()
    {
      var path = OriginalUri.AbsolutePath.ToLower();
      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();

      var forIFrame = path == "/electionforiframe.aspx";

      if (!IsNullOrEmpty(electionKey))
      {
        var stateCode = Elections.GetValidatedStateCodeFromKey(electionKey);
        if (IsNullOrEmpty(stateCode))
        {
          // Build normalized uri without state
          NormalizedUri = UrlManager.GetElectionPageUri(electionKey, false, forIFrame);
          _IsCanonicalUsa = true;
        }
        else if (StateCache.IsValidStateCode(stateCode))
          // Build normalized uri with state
          NormalizedUri = path == "/election2.aspx"
            ? UrlManager.GetElection2PageUri(stateCode, electionKey)
            : UrlManager.GetElectionPageUri(stateCode, electionKey, false, forIFrame);
        else
          ErrorMessage = "Invalid_StateCode";
      }
      else
        ErrorMessage = "Invalid_ElectionKey|StateCode";
    }

    private void HandleIntroPage()
    {
      var politicianKey = GetQueryParm("Id");
      var path = OriginalUri.AbsolutePath.ToLower();
      NormalizedUri = path == "/intro2.aspx" 
        ? UrlManager.GetIntro2PageUri(politicianKey) 
        : UrlManager.GetIntroPageUri(politicianKey);
    }

    // obsolete
    private void HandleIssuePage()
    {
      var officeKey = Empty;
      var issueKey = Empty;

      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();

      if (!IsNullOrEmpty(electionKey))
      {
        if (Offices.IsInElection(GetQueryParm("Office"), electionKey))
          officeKey = GetQueryParm("Office");

        if (!IsNullOrEmpty(officeKey))
          if (IsValidIssue(GetQueryParm("Issue"),
            Elections.GetStateCodeFromKey(electionKey)))
            issueKey = GetQueryParm("Issue");
      }

      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      if (!IsNullOrEmpty(electionKey) && !IsNullOrEmpty(officeKey) &&
        !IsNullOrEmpty(issueKey))
      {
        if (OriginalUri.AbsolutePath.ToLower() == "/issue2.aspx")
          NormalizedUri = UrlManager.GetIssue2PageUri(stateCode, electionKey,
            officeKey, issueKey);
        else
          NormalizedUri = UrlManager.GetIssuePageUri(stateCode, electionKey,
            officeKey, issueKey);
      }
      else
        ErrorMessage =
          "Cannot find this combination of State, Election, Office and Issue";
    }

    private void HandleIssue2Page()
    {
      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();

      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      if (StateCache.IsValidStateCode(stateCode) && !IsNullOrEmpty(electionKey))
        NormalizedUri = UrlManager.GetIssue2PageUri(stateCode, electionKey,
          GetQueryParm("Congress"), GetQueryParm("StateSenate"),
          GetQueryParm("StateHouse"), GetQueryParm("County"), GetQueryParm("Office"));
      else
        ErrorMessage =
          "Invalid_ElectionKey|State|Congress|StateSenate|StateHouse|County";
    }

    private void HandleOfficialsPage()
    {
      var reportCode = Empty;
      if (StateCache.IsValidStateOrFederalCode(GetQueryParm("Report"), false))
        reportCode = GetQueryParm("Report")
          .ToUpper();
      else
      {
        var stateCode = UrlManager.FindStateCode();
        if (StateCache.IsValidStateCode(stateCode) ||
          StateCache.IsValidFederalCode(stateCode, false))
          reportCode = stateCode;
      }

      if (!IsNullOrEmpty(reportCode))
      {
        NormalizedUri = UrlManager.GetOfficialsPageUri(reportCode,
          GetQueryParm("County"), GetQueryParm("Local"));
        _IsCanonicalUsa = !StateCache.IsValidStateCode(reportCode);
      }
      else
        ErrorMessage = "The Report code could not be identified.";
    }

    private static bool IsUsaPage(Uri uri)
    {
      switch (uri.AbsolutePath.ToLower())
      {
        case "/":
        case "/aboutus.aspx":
        case "/contactus.aspx":
        case "/default.aspx":
        case "/default2.aspx":
        case "/donate.aspx":
        case "/payments.aspx":
        case "/privacy.aspx":
        case "/forcandidates.aspx":
        case "/forvolunteers.aspx":
        case "/forpartners.aspx":
        case "/forpoliticalparties.aspx":
        case "/forelectionauthorities.aspx":
        case "/publicinterestorganizations.aspx":
          return true;
      }
      return false;
    }

    private void HandlePagesWithNoStateCode()
    {
      // Build normalized uri
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/aboutus.aspx":
          NormalizedUri = UrlManager.GetAboutUsPageUri();
          break;

        case "/candidates.aspx":
          NormalizedUri = UrlManager.GetForCandidatesPageUri();
          break;

        case "/contactus.aspx":
          NormalizedUri = UrlManager.GetContactUsPageUri();
          break;

        case "/interns.aspx":
          NormalizedUri = UrlManager.GetForVolunteersPageUri();
          break;

        case "/parties.aspx":
          NormalizedUri = UrlManager.GetForPoliticalPartiesPageUri();
          break;

        case "/voters.aspx":
          NormalizedUri = UrlManager.GetForVotersPageUri();
          break;
      }
    }

    private void HandlePagesWithJurisdictionParms()
    {
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/issuelist.aspx":
          NormalizedUri = UrlManager.GetIssueListPageUri(GetQueryParm("State"), GetQueryParm("County"), 
            GetQueryParm("Local"));
          break;
      }
    }

    private void HandlePagesWithOnlyStateCode()
    {
      var stateCode = UrlManager.FindStateCode();
      if (IsNullOrEmpty(stateCode) || StateCache.IsValidStateCode(stateCode))
        switch (OriginalUri.AbsolutePath.ToLower())
        {
          case "/":
          case "/default.aspx":
            NormalizedUri = UrlManager.GetSiteUri();
            //NormalizedUri = UrlManager.GetDefaultPageUri(stateCode);
            break;

          case "/default2.aspx":
            NormalizedUri = UrlManager.GetSiteUri("default2.aspx");
            break;

          case "/aboutus.aspx":
            NormalizedUri = UrlManager.GetAboutUsPageUri(stateCode);
            break;

          case "/contactus.aspx":
            NormalizedUri = UrlManager.GetContactUsPageUri(stateCode);
            break;

          case "/donate.aspx":
            NormalizedUri = UrlManager.GetDonatePageUri(stateCode);
            break;

          case "/forcandidates.aspx":
            NormalizedUri = UrlManager.GetForCandidatesPageUri(stateCode);
            break;

          case "/forresearch.aspx":
            NormalizedUri = UrlManager.GetForResearchPageUri(stateCode);
            break;

          case "/forpartners.aspx":
            NormalizedUri = UrlManager.GetForPartnersPageUri(stateCode);
            break;

          case "/forpoliticalparties.aspx":
            NormalizedUri = UrlManager.GetForPoliticalPartiesPageUri(stateCode);
            break;

          case "/forelectionauthorities.aspx":
            NormalizedUri = UrlManager.GetForElectionAuthoritiesPageUri(stateCode);
            break;

          case "/forvolunteers.aspx":
            NormalizedUri = UrlManager.GetForVolunteersPageUri(stateCode);
            break;

          case "/forvoters.aspx":
            NormalizedUri = UrlManager.GetForVotersPageUri(stateCode);
            break;

          case "/issuelist.aspx":
            NormalizedUri = UrlManager.GetIssueListPageUri(stateCode);
            break;

          case "/payments.aspx":
            NormalizedUri = UrlManager.GetPaymentsPageUri(stateCode);
            break;

          case "/privacy.aspx":
            NormalizedUri = UrlManager.GetPrivacyPageUri(stateCode);
            break;

          case "/publicinterestorganizations.aspx":
            NormalizedUri = UrlManager.GetPublicInterestOrganizationsPageUri(stateCode);
            break;
        }
      else
        ErrorMessage = "Invalid_StateCode";
    }

    private void HandlePoliticianIssuePage()
    {
      var stateCode = Politicians.GetStateCodeFromKey(GetQueryParm("Id"));
      if (StateCache.IsValidStateCode(stateCode))
        NormalizedUri = UrlManager.GetPoliticianIssuePageUri(stateCode,
          GetQueryParm("Id"), GetQueryParm("Issue"));
      else
        ErrorMessage = "Invalid StateCode";
    }

    private void HandleReferendumPage()
    {
      var electionKey = MemCache.IsValidElection(GetQueryParm("Election"))
        ? GetQueryParm("Election")
        : FixElectionKeyFromQueryString();
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var referendumKey = GetQueryParm("Referendum");

      var path = OriginalUri.AbsolutePath.ToLower();
      if (!IsNullOrEmpty(electionKey) && !IsNullOrEmpty(stateCode) &&
        Referendums.IsValidReferendumKey(electionKey, referendumKey))
        NormalizedUri = path == "/referendum.aspx"
          ? UrlManager.GetReferendumPageUri(stateCode, electionKey,
            referendumKey)
          : UrlManager.GetReferendum2PageUri(stateCode, electionKey,
            referendumKey);
      else
        ErrorMessage = "Invalid_ElectionKey|ReferendumKey|StateCode";
    }

    private void Initialize(Uri originalUri)
    {
      OriginalUri = originalUri;
      _OriginalQueryCollection = GetOriginalQueryCollectionCopy();
      NormalizedUri = null;
      CanonicalUri = null;
      ErrorMessage = "All_rebuild_options_failed";
    }

    private bool IsIPAddress
    {
      get
      {
        return IPAddress.TryParse(OriginalUri.Host, out _);
      }
    }

    private static bool IsValidElectionKey(string electionKey)
    {
      if (MemCache.IsValidElection(electionKey))
        if (Electoral_Class_Election(electionKey) == ElectoralClass.State ||
          Electoral_Class_Election(electionKey) == ElectoralClass.County ||
          Electoral_Class_Election(electionKey) == ElectoralClass.Local)
          return true;
        else if (Electoral_Class_Election(electionKey) == ElectoralClass.County &&
          Elections.GetCountyCodeFromKey(electionKey) == "000")
          //Report of County Links to County Elections
          return true;
        else if (Electoral_Class_Election(electionKey) == ElectoralClass.Local &&
          Elections.GetLocalKeyFromKey(electionKey) == "00")
          //Report of Local Links to Local Elections
          return true;
        else
          return false;
      return false;
    }

    private static bool IsValidIssue(string issueKey, string stateCode)
    {
      if (!Issues.IssueKeyExists(issueKey)) return false;
      // Check StateCode in IssueKey match StateCode for page
      switch (Issues.GetIssueLevelFromKey(issueKey.ToUpper()))
      {
        case "A": //All Offices
          return Issues.GetStateCodeFromKey(issueKey) == "LL";

        case "B": //National issues
          return Issues.GetStateCodeFromKey(issueKey) == "US";

        case "C": //State Issues
        case "D": //County Issues
        case "E": //Local Issues
          return Issues.GetStateCodeFromKey(issueKey) == stateCode;

        default: return false;
      }
    }

    private bool Normalize(Uri originalUri)
    {
      #region Note

      // This method rebuilds the old requested url
      // and a new correct url based on the StateCode for the page.
      // Both are reconstructed so that the QueryString 
      // parameters are in the same order.
      // Then they are compared (case insensitive).
      //
      // If there is any sort of failure, the method returns false.
      // The ErrorMessage property contains an explanation.
      //
      // If the method returns true (success), the
      // normalized Uri is available through the NormalizedUri
      // property. If it is null, no redirect is required.

      #endregion Note

      Initialize(originalUri);

      if (IsIPAddress) // any explicit IP address redirects to main site home page
      {
        NormalizedUri = UrlManager.SiteUri;
      }
      else
      {
        // Save the caching values and remove them so they don't trigger a
        // redirect
        var noCacheValue = GetQueryParm(VotePage.NoCacheParameter);
        var cacheDefeatValue = GetQueryParm("X");
        var openAllValue = GetQueryParm("openall");
        var completeValue = GetQueryParm("complete");
        var publicValue = GetQueryParm("public");
        var sampleBallotEmailValue = GetQueryParm("sbe");
        var adValue = GetQueryParm("ad");
        if (!IsNullOrWhiteSpace(noCacheValue) ||
          !IsNullOrWhiteSpace(cacheDefeatValue) ||
          !IsNullOrWhiteSpace(openAllValue) ||
          !IsNullOrWhiteSpace(completeValue) ||
          !IsNullOrWhiteSpace(publicValue) ||
          !IsNullOrWhiteSpace(sampleBallotEmailValue) ||
          !IsNullOrWhiteSpace(adValue))
        {
          if (!IsNullOrWhiteSpace(noCacheValue))
            _OriginalQueryCollection.Remove(VotePage.NoCacheParameter);
          if (!IsNullOrWhiteSpace(cacheDefeatValue))
            _OriginalQueryCollection.Remove("X");
          if (!IsNullOrWhiteSpace(openAllValue))
            _OriginalQueryCollection.Remove("openall");
          if (!IsNullOrWhiteSpace(completeValue))
            _OriginalQueryCollection.Remove("complete");
          if (!IsNullOrWhiteSpace(publicValue))
            _OriginalQueryCollection.Remove("public");
          if (!IsNullOrWhiteSpace(sampleBallotEmailValue))
            _OriginalQueryCollection.Remove("sbe");
          if (!IsNullOrWhiteSpace(adValue))
            _OriginalQueryCollection.Remove("ad");
          var ub = new UriBuilder(OriginalUri)
          {
            Query = _OriginalQueryCollection.ToString()
          };
          OriginalUri = ub.Uri;
        }

        // for ballot page, save any friend and choices parameters and remove them
        string friendValue = null;
        string choicesValue = null;
        if (OriginalUri.AbsolutePath.ToLower() == "/ballot.aspx")
        {
          friendValue = GetQueryParm("friend");
          choicesValue = GetQueryParm("choices");
          if (!IsNullOrWhiteSpace(friendValue) ||
            !IsNullOrWhiteSpace(choicesValue))
          {
            if (!IsNullOrWhiteSpace(friendValue))
              _OriginalQueryCollection.Remove("friend");
            if (!IsNullOrWhiteSpace(choicesValue))
              _OriginalQueryCollection.Remove("choices");
            var ub = new UriBuilder(OriginalUri)
            {
              Query = _OriginalQueryCollection.ToString()
            };
            OriginalUri = ub.Uri;
          }
        }

        if (ScriptHasQueryStringParms())
          if (!IsNullOrEmpty(GetQueryParm("Election")))
            if (!IsNullOrEmpty(GetQueryParm("Office")) &&
              !IsNullOrEmpty(GetQueryParm("Issue")))
              HandleIssuePage();
            else if (!IsNullOrEmpty(GetQueryParm("Referendum")))
              HandleReferendumPage();
            else if (!IsNullOrEmpty(GetQueryParm("Congress")))
              if (OriginalUri.AbsolutePath.ToLower() == "/issue2.aspx")
                HandleIssue2Page();
              else
                HandleBallotPage();
            else if (!IsNullOrEmpty(GetQueryParm("Office")))
              HandleCompareCandidatesPage();
            else
              HandleElectionPage();
          else if (!IsNullOrEmpty(GetQueryParm("Id")))
            if (!IsNullOrEmpty(GetQueryParm("Issue")))
              HandlePoliticianIssuePage();
            else
              HandleIntroPage();
          else if (!IsNullOrEmpty(GetQueryParm("Congress")))
            HandleElectedPage();
          else if (!IsNullOrEmpty(GetQueryParm("Report")) ||
            !IsNullOrEmpty(UrlManager.FindStateCode()))
            HandleOfficialsPage();

        if (NormalizedUri == null) // nothing yet, keep trying
          if (ScriptHasJurisdictionParms())
            HandlePagesWithJurisdictionParms();

        if (NormalizedUri == null) // nothing yet, keep trying
          if (ScriptCanHaveStateParm())
            HandlePagesWithOnlyStateCode();

        if (NormalizedUri == null) // nothing yet, keep trying
          if (ScriptHasNoParms())
            HandlePagesWithNoStateCode();

        // Ensure a canonical host name
        if (NormalizedUri != null)
          if (!UrlManager.IsCanonicalHostName(NormalizedUri.Host)) // replace host
          {
            var ub = new UriBuilder(NormalizedUri)
            {
              Host = UrlManager.GetCanonicalHostName(NormalizedUri.Host)
            };
            NormalizedUri = ub.Uri;
          }

        // Note: logging has been moved out of the mainstream processing so that
        // this class can be easily tested without logging

        if (NormalizedUri == null) // We couldn't do anything with it, error return
          return false;

        // The canonical Uri is all lower case
        CanonicalUri = new Uri(NormalizedUri.ToString()
          .ToLowerInvariant());
        CanonicalUri = AdjustCanonicalUri(CanonicalUri);

        Debug.Assert(NormalizedUri != null, nameof(NormalizedUri) + " != null");
        if (UrlManager.ForceSsl && NormalizedUri.Scheme != Uri.UriSchemeHttps)
        {
          NormalizedUri = new UriBuilder(NormalizedUri) { Scheme = Uri.UriSchemeHttps, Port = 443 }.Uri;
        }

        if (
          Uri.Compare(NormalizedUri, OriginalUri, UriComponents.AbsoluteUri | UriComponents.Fragment,
            UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
          NormalizedUri = null; // meaning no redirect is necessary

        // suppress redirect for /default.aspx if everything else is the same
        if (NormalizedUri != null &&
          OriginalUri.AbsolutePath.Equals("/default.aspx",
            StringComparison.OrdinalIgnoreCase))
        {
          var ub = new UriBuilder(OriginalUri) {Path = "/"};
          if (
            Uri.Compare(NormalizedUri, ub.Uri, UriComponents.AbsoluteUri,
              UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
            NormalizedUri = null; // meaning no redirect is necessary
        }

        // Restore NoCache and browser cache defeat parameters.
        // Note: we do not restore browser cache defeat unless NoCache is present too.
        if (NormalizedUri != null &&
          (!IsNullOrWhiteSpace(noCacheValue) || !IsNullOrWhiteSpace(openAllValue) ||
            !IsNullOrWhiteSpace(publicValue) ||
            !IsNullOrWhiteSpace(sampleBallotEmailValue) ||
            !IsNullOrWhiteSpace(adValue)))
        {
          var ub = new UriBuilder(NormalizedUri);
          var qsc = QueryStringCollection.Parse(ub.Query);
          if (!IsNullOrWhiteSpace(noCacheValue))
            qsc.Add(VotePage.NoCacheParameter, noCacheValue);
          if (!IsNullOrWhiteSpace(noCacheValue) && !IsNullOrWhiteSpace(cacheDefeatValue))
            qsc.Add("X", cacheDefeatValue);
          if (!IsNullOrWhiteSpace(openAllValue))
            qsc.Add("openall", openAllValue);
          if (!IsNullOrWhiteSpace(completeValue))
            qsc.Add("complete", completeValue);
          if (!IsNullOrWhiteSpace(publicValue))
            qsc.Add("public", publicValue);
          if (!IsNullOrWhiteSpace(sampleBallotEmailValue))
            qsc.Add("sbe", sampleBallotEmailValue);
          if (!IsNullOrWhiteSpace(adValue))
            qsc.Add("ad", adValue);
          ub.Query = qsc.ToString();
          NormalizedUri = ub.Uri;
        }

        // restore friend and choices (ballot page only)
        // must both be present
        if (NormalizedUri != null && !IsNullOrWhiteSpace(friendValue) && !IsNullOrWhiteSpace(choicesValue))
        {
          var ub = new UriBuilder(NormalizedUri);
          var qsc = QueryStringCollection.Parse(ub.Query);
          qsc.Add("friend", friendValue);
          qsc.Add("choices", choicesValue);
          ub.Query = qsc.ToString();
          NormalizedUri = ub.Uri;
        }
      }

      return true;
    }

    private Uri OriginalUri { get; set; }

    private bool ScriptCanHaveStateParm()
    {
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/":
        case "/donate.aspx":
        case "/default.aspx":
        case "/default2.aspx":
        case "/forcandidates.aspx":
        case "/forresearch.aspx":
        case "/forpartners.aspx":
        case "/forpoliticalparties.aspx":
        case "/forelectionauthorities.aspx":
        case "/forvolunteers.aspx":
        case "/forvoters.aspx":
        case "/payments.aspx":
        case "/publicinterestorganizations.aspx":
        case "/privacy.aspx":
          return true;

        default:
          return false;
      }
    }

    private bool ScriptHasNoParms()
    {
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/aboutus.aspx":
        case "/contactus.aspx":
          return true;

        default:
          return false;
      }
    }

    private bool ScriptHasJurisdictionParms()
    {
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/issuelist.aspx":
          return true;

        default:
          return false;
      }
    }

    private bool ScriptHasQueryStringParms()
    {
      switch (OriginalUri.AbsolutePath.ToLower())
      {
        case "/ballot.aspx":
        case "/ballot2.aspx":
        case "/comparecandidates.aspx":
        case "/comparecandidates2.aspx":
        case "/elected.aspx":
        case "/election.aspx":
        case "/election2.aspx":
        case "/electionforiframe.aspx":
        case "/intro.aspx":
        case "/intro2.aspx":
        case "/issue.aspx":
        case "/issue2.aspx":
        case "/officials.aspx":
        case "/politicianissue.aspx":
        case "/referendum.aspx":
        case "/referendum2.aspx":
          return true;

        default:
          return false;
      }
    }

    #endregion Private methods

    #region Public properties

    public Uri CanonicalUri { get; private set; }

    public string ErrorMessage { get; private set; }

    public Uri NormalizedUri { get; private set; }

    #endregion Public properties

    #region Public methods

    public bool Normalize()
    {
      return Normalize(BuildCurrentUri());
    }

    #endregion Public methods
  }
}