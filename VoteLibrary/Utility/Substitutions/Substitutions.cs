using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  // ReSharper disable UnusedMember.Global
  // ReSharper disable UnusedMember.Local
  // ReSharper disable StringIndexOfIsCultureSpecific.1
  public sealed partial class Substitutions
  {
    #region Private

    private static readonly Dictionary<string, Requirements> _ValidEmbeddedSubstitutions =
      new Dictionary<string, Requirements>(StringComparer.OrdinalIgnoreCase)
      {
        {"StateCode", Requirements.StateCode },
        {"ElectionKey", Requirements.ElectionKey },
        {"OfficeKey", Requirements.OfficeKey },
        {"PoliticianKey", Requirements.PoliticianKey },
        {"CountyCode", Requirements.CountyCode },
        {"LocalKey", Requirements.LocalKey }
      };

    private PageCache _PageCache;
    private Dictionary<string, object> _DataCache;
    private Dictionary<string, string> _AdHocSubstitutions;
    private Dictionary<string, string> _EmbeddedSubstitutions;
    private string _CountyCode;
    private string _ElectionKey;
    private string _IssueKey;
    private string _LocalKey;
    private string _OfficeKey;
    private string _PartyEmail;
    private string _PartyKey;
    private string _PoliticianKey;
    private string _StateCode;
    private int? _VisitorId;
    private int? _DonorId;
    private int? _OrgContactId;
    private DateTime? _Now;

    private static readonly Dictionary<string, Substitution>
      SubstitutionsDictionary;

    private static readonly Regex ParserRegex =
      new Regex(@"(?:\[\[.+?]])|(?:@@.+?@@)|(?:##.+?##)", RegexOptions.IgnoreCase);

    private static readonly Regex ParserRegexPass1 =
      new Regex(@"(?:\[\[.+?]])", RegexOptions.IgnoreCase);

    private static readonly Regex ParserRegexPass2 =
      new Regex(@"(?:@@.+?@@)|(?:##.+?##)", RegexOptions.IgnoreCase);

    private static readonly Regex EmbeddedSubstitutionRegex =
      new Regex(@"^\[\[(?<parm>[\da-z]+)=(?<value>[\da-z]+%?)\]\]$", RegexOptions.IgnoreCase);

    private static readonly Regex ParseOptionsRegex =
      new Regex(
        @"\((?<options>(?:(?:\s*(?<option>[^,)\s]+)\s*,)*\s*(?<option>[^,)\s]+))?\s*)\)\s*..\z");

    private static readonly Regex CheckerRegex = new Regex(@"\[\[|]]|@@|##");

    private static string AddStyleToTag(string tag, string style)
    {
      return Regex.Replace(tag, @"^(<[a-z]\s)", "$1style=\"" + style + "\" ",
        RegexOptions.IgnoreCase);
    }

    private static string CreateMailToAnchor(string email, string anchorText = null)
    {
      if (IsNullOrWhiteSpace(email)) return Empty;
      if (IsNullOrWhiteSpace(anchorText)) anchorText = email;
      return
        new HtmlAnchor {HRef = "mailto:" + email, InnerHtml = anchorText}
          .RenderToString();
    }

    public static string CreateWebAnchor(Uri uri, string anchorText = "",
      string title = "", string target = "")
    {
      return uri == null
        ? Empty
        : CreateWebAnchor(uri.ToString(), anchorText, title, target);
    }

    public static string CreateWebAnchor(string href, string anchorText = "",
      string title = "", string target = "")
    {
      if (IsNullOrWhiteSpace(href)) return Empty;
      anchorText = anchorText.SafeString();
      var index = anchorText.IndexOf("Image:", StringComparison.OrdinalIgnoreCase);
      if (index >= 0) // an uploaded image anchor
      {
        var externalName = anchorText.Substring(index + 6);
        var path = $"/image/{externalName}{UploadedImages.GetImageTypeByExternalName(externalName, Empty)}";
        return CreateImageAnchor(href, UrlManager.GetSiteUri(path).ToString(), title, target);
      }
      if (IsNullOrWhiteSpace(anchorText)) anchorText = href;
      href = VotePage.NormalizeUrl(href);
      var a = new HtmlAnchor
      {
        HRef = href,
        InnerHtml = anchorText,
        Title = title,
        Target = target
      };
      return a.RenderToString();
    }

    private static string CreateImageAnchor(string href, string src,
      string title = "", string target = "")
    {
      if (IsNullOrWhiteSpace(href)) return Empty;
      if (IsNullOrWhiteSpace(src)) return Empty;
      href = VotePage.NormalizeUrl(href);
      src = VotePage.NormalizeUrl(src);
      var a = new HtmlAnchor
      {
        HRef = href,
        Title = title,
        Target = target
      };
      new HtmlImage {Src = src}.AddTo(a);
      return a.RenderToString();
    }

    public static string Do(string template)
    {
      return new Substitutions().Substitute(template);
    }

    private string GetAdHocSubstitution(string substitution)
    {
      if (_AdHocSubstitutions != null &&
        _AdHocSubstitutions.TryGetValue(substitution, out var result)) return result;
      return Empty;
    }

    private object GetDataCache(string key)
    {
      object data = null;
      _DataCache?.TryGetValue(key, out data);
      return data;
    }

    private string GetLocalizedOfficeName(string officeKey)
    {
      var officeTitle = Empty;

      if (!Offices.IsStateOrFederalOffice(officeKey))
      {
        if (Offices.IsCountyOffice(officeKey))
          officeTitle +=
            CountyCache.GetCountyName(Offices.GetStateCodeFromKey(officeKey),
              Offices.GetCountyCodeFromKey(officeKey));
        else
        {
          var stateCode = Offices.GetStateCodeFromKey(officeKey);
          officeTitle +=
            VotePage.GetPageCache()
              .LocalDistricts.GetLocalDistrict(stateCode,
                Offices.GetLocalKeyFromKey(officeKey));
        }
        officeTitle += " ";
      }

      officeTitle +=
        Offices.FormatOfficeName(PageCache.Offices.GetOfficeLine1(officeKey),
          PageCache.Offices.GetOfficeLine2(officeKey), officeKey);

      return officeTitle;
    }

    private IEnumerable<ElectionsRow> GetFuturePrimaryElections(string stateCode)
    {
      var key = stateCode + "|FuturePrimaryElectionsByState";
      var data = GetDataCache(key);
      if (data == null)
      {
        var table =
          Elections.GetFutureViewablePrimaryDisplayDataByStateCode(stateCode);
        // only keep closest dated elections
        if (table.Count > 0)
          data = table.Where(row => row.ElectionDate == table[0].ElectionDate)
            .ToList();
        else data = table;
        PutDataCache(key, data);
      }
      return data as IEnumerable<ElectionsRow>;
    }

    private string GetMajorPartyCodeWithOption(Options o)
    {
      switch (o & Options.MajorPartyTypes)
      {
        case Options.DemocraticParty:
          return "D";

        case Options.RepublicanParty:
          return "R";

        case Options.LibertarianParty:
          return "L";

        case Options.GreenParty:
          return "G";
      }
      var partyKey = PartyKey;
      if (partyKey.Length == 3 && partyKey.Substring(0, 2) == StateCode)
      {
        var partyCode = partyKey.Substring(2);
        if (new[] {"D", "R", "L", "G"}.Contains(partyCode)) return partyCode;
      }
      throw new VoteSubstitutionException("Could not determine major party code");
    }

    private void GetPartyKey()
    {
      if (IsNullOrWhiteSpace(_PartyKey))
        PartyKey = VotePage.QueryParty;
      if (IsNullOrWhiteSpace(_PartyKey))
      {
        GetPoliticianKey();
        if (IsNullOrWhiteSpace(_PoliticianKey))
          PartyKey = PageCache.Politicians.GetPartyKey(_PoliticianKey);
      }
    }

    private void GetPoliticianKey()
    {
      if (IsNullOrWhiteSpace(_PoliticianKey))
        PoliticianKey = VotePage.QueryId;
    }

    private static string ParseOptions(string parameter, out Options options)
    {
      options = Options.None;
      var match = ParseOptionsRegex.Match(parameter);
      if (match.Success)
      {
        var optionList = new List<KeyValuePair<string, Options>>();
        var opts = Options.None;
        foreach (var capture in match.Groups["option"].Captures.Cast<Capture>())
        {
          // collect and check for validity and duplicates
          var option = capture.Value.ToLowerInvariant();
          if (!OptionsDictionary.TryGetValue(option, out var o))
            throw new VoteSubstitutionException("Invalid option " + capture.Value +
              ": " + parameter);
          if (optionList.Exists(i => i.Key == option))
            throw new VoteSubstitutionException("Duplicate option " + capture.Value +
              ": " + parameter);
          opts |= o;
          optionList.Add(new KeyValuePair<string, Options>(option, o));
        }
        // Make sure no more than one of each type is present
        foreach (var info in OptionTypeInfos)
        {
          if (info.Multiple) continue;
          var has = new List<string>();
          foreach (var o in info.Members)
          {
            var item = optionList.Find(l => l.Value == o);
            if (item.Key != null) has.Add(item.Key);
          }
          if (has.Count > 1)
            throw new VoteSubstitutionException("Conflicting options " +
              string.Join(", ", has) + ": " + parameter);
        }
        // remove the options to get the basic name for matching
        var optionsGroup = match.Groups["options"];
        Debug.Assert(optionsGroup.Captures.Count == 1, "Missing options Capture");
        var optionsCapture = optionsGroup.Captures[0];
        parameter = parameter.Substring(0, optionsCapture.Index) +
          parameter.Substring(optionsCapture.Index + optionsCapture.Length);
        options = opts;
      }
      else if (Regex.IsMatch(parameter, @"\(.*\)\s*..\z"))
        throw new VoteSubstitutionException("Invalid options syntax: " + parameter);

      return parameter;
    }

    private string OneSubstitution(string parameter)
    {
      try
      {
        // first check for an embedded substitution assignment
        var embeddedMatch = EmbeddedSubstitutionRegex.Match(parameter);
        if (embeddedMatch.Success)
        {
          var parm = embeddedMatch.Groups["parm"].Value;
          var parmValue = embeddedMatch.Groups["value"].Value;
          if (parm.Equals("addiscount", StringComparison.OrdinalIgnoreCase))
          {
            var pctMatch = Regex.Match(parmValue, @"^(?<percent>\d{1,2})%$");
            if (pctMatch.Success)
              AdDiscount = Convert.ToDecimal(pctMatch.Groups["percent"].Value) / 100m;
            return Empty;
          }
          if (!_ValidEmbeddedSubstitutions.ContainsKey(parm))
            throw new VoteSubstitutionException("Unknown embedded substitution: " + parameter);
          if (_EmbeddedSubstitutions == null)
            _EmbeddedSubstitutions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
          _EmbeddedSubstitutions[parm] = parmValue;
          return Empty;
        }

        parameter = ParseOptions(parameter, out var options);
        if (_AdHocSubstitutions != null &&
          _AdHocSubstitutions.TryGetValue(parameter, out var adHoc)) return adHoc;

        // this is a bit of a kludge to allow custom link text to be embedded in a link-type (##) parameter
        // as ##<substitution-or-literal-link>{link text}##
        string linkText = null;
        var match = Regex.Match(parameter, @"^##.+?\{(?<text>.*?)\}##|\[\[.+?\{(?<text>.*?)\}\]\]$");
        if (match.Success)
        {
          // recover linkText and remove from parameter
          linkText = match.Groups["text"].Value;
          parameter = parameter.Replace($"{{{linkText}}}", Empty);
          if (linkText == Empty) linkText = null;
        }

        if (SubstitutionsDictionary.TryGetValue(parameter, out var substitution))
          return substitution.Do(this, options, linkText);

        var value = parameter.Substring(2, parameter.Length - 4).Trim();
        switch (parameter[0])
        {
          case '[': // check for miscoding
            SubstitutionsDictionary.TryGetValue("@@" + value + "@@", out substitution);
            if (substitution != null) return substitution.Do(this, options);
            SubstitutionsDictionary.TryGetValue("##" + value + "##", out substitution);
            if (substitution != null) return substitution.Do(this, options, linkText);
            break;

          case '@': // handle immediate
            return CreateMailToAnchor(value);

          case '#': // handle immediate
            return CreateWebAnchor(value, linkText);
        }
        throw new VoteSubstitutionException("Unknown substitution: " + parameter);
      }
      catch (VoteSubstitutionException ex)
      {
        // defer a VoteSubstitutionException by substituting an {{exception}}
        return $"{{{{exception {ex.Message}}}}}";
      }
    }

    private void PutDataCache(string key, object data)
    {
      if (_DataCache == null)
        _DataCache =
          new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
      _DataCache[key] = data;
    }

    private class Substitution : SubstitutionInfo
    {
      public Func<Substitutions, Options, string, string> Fn;

      public string Do(Substitutions s, Options o, string linkText = null)
      {
        return Fn(s, o, linkText)
          .SafeString();
      }

      //public SubstitutionInfo GetInfo()
      //{
      //  return new SubstitutionInfo
      //  {
      //    HtmlDescription = HtmlDescription,
      //    Requirements = Requirements
      //  };
      //}
    }

    //private class SimpleSubstitution : Substitution
    //{
    //}

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

    [Flags]
    public enum Requirements
    {
      None = 0x0000,
      StateCode = 0x0001,
      CountyCode = 0x0002,
      LocalKey = 0x0004,
      PoliticianKey = 0x0008,
      ElectionKey = 0x0010,
      OfficeKey = 0x0020,
      IssueKey = 0x0040,
      PartyKey = 0x0080,
      PartyEmail = 0x0100,
      //OrganizationCode = 0x0200,
      VisitorId = 0x0400,
      DonorId = 0x0800,
      OrgContactId = 0x1000
    }

    public enum Class
    {
      None,
      Text,
      Email,
      Web
    }

    public enum Type
    {
      State,
      County,
      Local,
      Election,
      Office,
      Politician,
      Issue,
      Party,
      Volunteer,
      Visitor,
      Donor,
      Organization,
      EmailList,
      Miscellaneous,
      Generic,
      Undocumented,
      Deprecated
    }

    public static readonly Dictionary<Type, string> TypeNameDictionary =
      new Dictionary<Type, string>
      {
        {Type.State, "State Substitutions"},
        {Type.County, "County Substitutions"},
        {Type.Local, "Local Substitutions"},
        {Type.Election, "Election Substitutions"},
        {Type.Office, "Office Substitutions"},
        {Type.Politician, "Politician Substitutions"},
        {Type.Issue, "Issue Substitutions"},
        {Type.Party, "Party Substitutions"},
        {Type.Volunteer, "Volunteer Substitutions"},
        {Type.Visitor, "Website Visitor Substitutions"},
        {Type.Donor, "Donor Substitutions"},
        {Type.Organization, "Organization Substitutions"},
        {Type.EmailList, "Email List Substitutions"},
        {Type.Miscellaneous, "Miscellaneous Substitutions"},
        {Type.Generic, "Generic Substitutions"},
        {Type.Undocumented, "Undocumented Substitutions"},
        {Type.Deprecated, "Deprecated Substitutions"}
      };

    [Flags]
    public enum Options
    {
      None = 0x00000000,

      // Date Type
      XmdDate = 0x00000001,
      XmmddDate = 0x00000002,
      XmmmdDate = 0x00000004,
      XmmmmdDate = 0x00000008,
      XdddmmmdDate = 0x00000010,
      XddddmmmmdDate = 0x00000020,
      DateTypes = 0x0000003f,

      // List Type
      CommaList = 0x00000040,
      ParagraphList = 0x00000080,
      UnorderedList = 0x00000100,
      LinefeedList = 0x00000200,
      BreakList = 0x00000400,
      ListTypes = 0x000007c0,

      // Election Types
      GeneralElection = 0x00000800,
      OffYearElection = 0x00001000,
      PrimaryElection = 0x00002000,
      SpecialElection = 0x00004000,
      PresidentialPrimaryElection = 0x00008000,
      ElectionTypes = 0x0000f800,

      // Viewability Types
      Viewable = 0x00010000,
      NotViewable = 0x00020000,
      ViewabilityTypes = 0x00030000,

      // Past/Future Types
      Past = 0x00040000,
      Future = 0x00080000,
      PastFutureTypes = 0x000c0000,

      // Major Party Types
      DemocraticParty = 0x00100000,
      RepublicanParty = 0x00200000,
      LibertarianParty = 0x00400000,
      GreenParty = 0x00800000,
      MajorPartyTypes = 0x00f00000,

      All = 0x00ffffff
    }

    [Flags]
    public enum OptionTypes
    {
      None = 0x0000,
      Date = 0x0001,
      List = 0x0002,
      Election = 0x0004,
      Viewability = 0x0008,
      PastFuture = 0x0010,
      MajorParty = 0x0020
    }

    public class OptionTypeInfo
    {
      public OptionTypes OptionType;
      public string Name;
      public string ShortName;
      public string HtmlDescription;
      // ReSharper disable once NotAccessedField.Global
      public Options Options;
      public Options[] Members;
      public bool Multiple;
    }

    public static readonly OptionTypeInfo[] OptionTypeInfos =
    {
      new OptionTypeInfo
      {
        OptionType = OptionTypes.Date,
        Name = "Date Options",
        ShortName = "Date",
        HtmlDescription = "<p>Controls how date values are formatted." +
          " Options are:</p><ul>" +
          "<li><em>md</em> = like 1/1/2014</li>" +
          "<li><em>mmdd</em> = like 01/01/2014</li>" +
          "<li><em>mmmd</em> = like Jan 1, 2014" +
          "<li><em>mmmmd</em> = like January 1,2014 (default)</li>" +
          "<li><em>dddmmmd</em> = like Wed, Jan 1, 2014</li>" +
          "<li><em>ddddmmmmd</em> = like Wednesday, January 1, 2014</p>",
        Options = Options.DateTypes,
        Members = new[]
        {
          Options.XmdDate,
          Options.XmmddDate,
          Options.XmmmdDate,
          Options.XmmmmdDate,
          Options.XdddmmmdDate,
          Options.XddddmmmmdDate
        }
      },
      new OptionTypeInfo
      {
        OptionType = OptionTypes.List,
        Name = "List Options",
        ShortName = "List",
        HtmlDescription = "<p>Controls how the elements in a list are separated." +
          " Options are:</p><ul>" +
          "<li><em>comma</em> = separate by commas using <i>and</i> for the" +
          " last separator (default)</li>" +
          "<li><em>ptag</em> = enclose each element in a &lt;p&gt; tag</li>" +
          "<li><em>ul</em> = enclosed each element in a &lt;li&gt;" +
          " tag and enclosed the whole list in a &lt;ul&gt; tag</li>" +
          "<li><em>lf</em> = separate by a linefeed character</li>" +
          "<li><em>br</em> = separate by &lt;br/ &gt; tags</li>" +
          "</ul><p>Only one List option may be specified.</p>",
        Options = Options.ListTypes,
        Members = new[]
        {
          Options.CommaList,
          Options.ParagraphList,
          Options.UnorderedList,
          Options.LinefeedList,
          Options.BreakList
        }
      },
      new OptionTypeInfo
      {
        OptionType = OptionTypes.Election,
        Name = "Election Options",
        ShortName = "Election",
        HtmlDescription = "<p>Controls which election types are included" +
          " in the list. Options are:</p><ul>" +
          "<li><em>g</em> = General elections</li>" +
          "<li><em>o</em> = Off-year elections</li>" +
          "<li><em>p</em> = Primary elections (state)</li>" +
          "<li><em>s</em> = Special elections</li>" +
          "<li><em>b</em> = Presidential primaries</li>" +
          "</ul><p>More than one Election option may be specified. If omitted" +
          " all election types are used.</p>",
        Options = Options.ElectionTypes,
        Members = new[]
        {
          Options.GeneralElection,
          Options.OffYearElection,
          Options.PrimaryElection,
          Options.SpecialElection,
          Options.PresidentialPrimaryElection
        },
        Multiple = true
      },
      new OptionTypeInfo
      {
        OptionType = OptionTypes.Viewability,
        Name = "Viewability Options",
        ShortName = "Viewability",
        HtmlDescription = "<p>Controls whether publicly viewable elections are" +
          " included or excluded. Options are:</p><ul>" +
          "<li><em>v</em> = viewable only</li>" +
          "<li><em>nv</em> = not viewable only</li>" +
          "</ul><p>Only one Viewability option may be specified. If omitted" +
          " all elections are used.</p>",
        Options = Options.ViewabilityTypes,
        Members = new[]
        {
          Options.Viewable,
          Options.NotViewable
        }
      },
      new OptionTypeInfo
      {
        OptionType = OptionTypes.PastFuture,
        Name = "Past/Future Options",
        ShortName = "Past/Future",
        HtmlDescription = "<p>Controls whether past or future elections are" +
          " included. Options are:</p><ul>" +
          "<li><em>past</em> = past elections only</li>" +
          "<li><em>future</em> = future elections only</li>" +
          "</ul><p>Only one Past/Future option may be specified. If omitted" +
          " all elections are used.</p>",
        Options = Options.PastFutureTypes,
        Members = new[]
        {
          Options.Past,
          Options.Future
        }
      },
      new OptionTypeInfo
      {
        OptionType = OptionTypes.MajorParty,
        Name = "Major Party Options",
        ShortName = "Major Party",
        HtmlDescription = "<p>Targets a specific major party primary. " +
          "Options are:</p><ul>" +
          "<li><em>dem</em> = Democratic Party</li>" +
          "<li><em>rep</em> = Republican Party</li>" +
          "<li><em>lib</em> = Libertarian Party</li>" +
          "<li><em>grn</em> = Green Party</li>" +
          "</ul><p>Only one Major Party may be specified. If omitted the party key will be used if available.</p>",
        Options = Options.MajorPartyTypes,
        Members = new[]
        {
          Options.DemocraticParty,
          Options.RepublicanParty,
          Options.LibertarianParty,
          Options.GreenParty
        }
      }
    };

    private static readonly Dictionary<string, Options> OptionsDictionary =
      new Dictionary<string, Options>(StringComparer.OrdinalIgnoreCase)
      {
        {"md", Options.XmdDate},
        {"mmdd", Options.XmmddDate},
        {"mmmd", Options.XmmmdDate},
        {"mmmmd", Options.XmmmmdDate},
        {"dddmmmd", Options.XdddmmmdDate},
        {"ddddmmmmd", Options.XddddmmmmdDate},
        {"comma", Options.CommaList},
        {"ptag", Options.ParagraphList},
        {"ul", Options.UnorderedList},
        {"lf", Options.LinefeedList},
        {"br", Options.BreakList},
        {"g", Options.GeneralElection},
        {"o", Options.OffYearElection},
        {"p", Options.PrimaryElection},
        {"s", Options.SpecialElection},
        {"b", Options.PresidentialPrimaryElection},
        {"v", Options.Viewable},
        {"nv", Options.NotViewable},
        {"past", Options.Past},
        {"future", Options.Future},
        {"dem", Options.DemocraticParty},
        {"rep", Options.RepublicanParty},
        {"lib", Options.LibertarianParty},
        {"grn", Options.GreenParty}
      };

    public Substitutions(params string[] parms)
    {
      AddSubstitutions(parms);
    }

    public void AddSubstitutions(params string[] parms)
    {
      if (parms.Length == 0) return;
      var inx = 0;
      if (_AdHocSubstitutions == null)
        _AdHocSubstitutions =
          new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      while (inx < parms.Length)
      {
        var name = parms[inx++];
        if (IsNullOrWhiteSpace(name))
          throw new VoteSubstitutionException("Empty parameter not allowed");
        if (inx >= parms.Length) throw new VoteSubstitutionException("Missing parameter value");
        var value = parms[inx++];
        if (name.Length >= 5)
          switch (name[0])
          {
            case '[':
              if (name.StartsWith("[[", StringComparison.Ordinal) &&
                name.EndsWith("]]", StringComparison.Ordinal))
              {
                _AdHocSubstitutions.Add(name, value);
                continue;
              }
              break;

            case '@':
            case '#':
              var delimiter = new string(name[0], 2);
              if (name.StartsWith(delimiter, StringComparison.Ordinal) &&
                name.EndsWith(delimiter, StringComparison.Ordinal))
              {
                var anchorText = value;
                var delimiters = new[] {'[', '@', '#'};
                if (inx < parms.Length && !delimiters.Contains(parms[inx][0]))
                  anchorText = parms[inx++];
                if (name[0] == '@') // email
                  value = CreateMailToAnchor(value, anchorText);
                else // anchor
                {
                  var title = Empty;
                  var target = Empty;
                  if (inx < parms.Length &&
                  (IsNullOrWhiteSpace(parms[inx]) ||
                    !delimiters.Contains(parms[inx][0])))
                  {
                    title = parms[inx++].SafeString();
                    if (inx < parms.Length &&
                    (IsNullOrWhiteSpace(parms[inx]) ||
                      !delimiters.Contains(parms[inx][0]))) target = parms[inx++];
                  }
                  value = CreateWebAnchor(value, anchorText, title, target);
                }
                _AdHocSubstitutions.Add(name, value);
                continue;
              }
              break;
          }
        throw new VoteSubstitutionException("Invalid substitution name: " + name);
      }
    }

    public static void CheckForIncompleteSubstitutions(string input)
    {
      var match = CheckerRegex.Match(input);
      if (match.Success)
        switch (match.Value)
        {
          case "]]":
            throw new VoteSubstitutionException(
              "There is an incomplete substitution ending with '{0}'",
              input.SafeSubstring(match.Index - 20, 22));

          case "[[":
            throw new VoteSubstitutionException(
              "There is an incomplete substitution starting with '{0}'",
              input.SafeSubstring(match.Index, 22));

          default:
            throw new VoteSubstitutionException(
              "There is an incomplete substitution containing '{0}'",
              input.SafeSubstring(match.Index - 10, 22));
        }
    }

    public decimal? AdDiscount { get; private set; }

    public string CountyCode
    {
      get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("CountyCode", out var countyCode))
          return countyCode;
        if (IsNullOrWhiteSpace(_CountyCode)) CountyCode = VotePage.QueryCounty;
        if (IsNullOrWhiteSpace(_CountyCode))
          CountyCode = Elections.GetCountyCodeFromKey(_ElectionKey);
        if (IsNullOrWhiteSpace(_CountyCode))
          CountyCode = Offices.GetCountyCodeFromKey(_OfficeKey);
        if (IsNullOrWhiteSpace(_CountyCode))
          throw new VoteSubstitutionException("County Code is missing");
        return _CountyCode;
      }
      set
      {
        _CountyCode = value.SafeString()
          .ZeroPad(3);
      }
    }

    public int DonorId
    {
      private get
      {
        if (_DonorId != null && _DonorId != 0) return _DonorId.Value;
        throw new VoteSubstitutionException("Unable to determine a donor id for substitutions");
      }
      set { _DonorId = value; }
    }

    public string ElectionKey
    {
      get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("ElectionKey", out var electionKey))
          return electionKey;
        if (IsNullOrWhiteSpace(_ElectionKey)) ElectionKey = VotePage.QueryElection;
        if (IsNullOrWhiteSpace(_ElectionKey))
          throw new VoteSubstitutionException("Election key is missing");
        return _ElectionKey;
      }
      set
      {
        _ElectionKey = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public string IssueKey
    {
      get
      {
        if (IsNullOrWhiteSpace(_IssueKey)) IssueKey = VotePage.QueryIssue;
        if (IsNullOrWhiteSpace(_IssueKey))
          throw new VoteSubstitutionException("Issue key is missing");
        return _IssueKey;
      }
      set
      {
        _IssueKey = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public string LocalKey
    {
      get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("LocalKey", out var localKey))
          return localKey;
        if (IsNullOrWhiteSpace(_LocalKey)) LocalKey = VotePage.QueryLocal;
        if (IsNullOrWhiteSpace(_LocalKey))
          LocalKey = Elections.GetLocalKeyFromKey(_ElectionKey);
        if (IsNullOrWhiteSpace(_LocalKey))
          LocalKey = Offices.GetLocalKeyFromKey(_OfficeKey);
        if (IsNullOrWhiteSpace(_LocalKey))
          throw new VoteSubstitutionException("Local Key is missing");
        return _LocalKey;
      }
      set
      {
        _LocalKey = value.SafeString()
          .ZeroPad(2);
      }
    }

    public DateTime Now
    {
      private get
      {
        if (_Now == null) _Now = DateTime.UtcNow;
        return _Now.Value;
      }
      set { _Now = value; }
    }

    public string OfficeKey
    {
      get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("OfficeKey", out var officeKey))
          return officeKey;
        if (IsNullOrWhiteSpace(_OfficeKey)) OfficeKey = VotePage.QueryOffice;
        if (IsNullOrWhiteSpace(_OfficeKey))
          throw new VoteSubstitutionException("Office key is missing");
        return _OfficeKey;
      }
      set
      {
        _OfficeKey = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public int OrgContactId
    {
      private get
      {
        if (_OrgContactId != null && _OrgContactId != 0) return _OrgContactId.Value;
        throw new VoteSubstitutionException("Unable to determine an organization contact id for substitutions");
      }
      set
      {
        _OrgContactId = value;
      }
    }

    public PageCache PageCache
    {
      private get
      {
        if (_PageCache != null) return _PageCache;
        _PageCache = VotePage.GetPageCache();
        return _PageCache;
      }
      set { _PageCache = value; }
    }

    public string PartyEmail
    {
      get
      {
        if (IsNullOrWhiteSpace(_PartyEmail))
          throw new VoteSubstitutionException("Party email is missing");
        return _PartyEmail;
      }
      set { _PartyEmail = value.SafeString(); }
    }

    public string PartyKey
    {
      get
      {
        GetPartyKey();
        if (!IsNullOrWhiteSpace(_PartyKey)) return _PartyKey;
        if (!IsNullOrWhiteSpace(_PartyEmail))
          PartyKey = PartiesEmails.GetPartyKey(_PartyEmail);
        if (IsNullOrWhiteSpace(_PartyKey))
          throw new VoteSubstitutionException("Party key is missing");
        return _PartyKey;
      }
      set
      {
        _PartyKey = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public string PoliticianKey
    {
      get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("PoliticianKey", out var politicianKey))
          return politicianKey;
        GetPoliticianKey();
        if (IsNullOrWhiteSpace(_PoliticianKey))
          throw new VoteSubstitutionException("Politician key is missing");
        return _PoliticianKey;
      }
      set
      {
        _PoliticianKey = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public string SourceCode { get; set; }

    public string StateCode
    {
      private get
      {
        if (_EmbeddedSubstitutions != null && _EmbeddedSubstitutions.TryGetValue("StateCode", out var stateCode))
          return stateCode;
        if (!IsNullOrWhiteSpace(_StateCode)) return _StateCode;
        StateCode = SecurePage.FindStateCode();
        if (!IsNullOrWhiteSpace(_StateCode)) return _StateCode;
        if (!SecurePage.IsSecurePage) StateCode = UrlManager.CurrentDomainDataCode;
        if (!IsNullOrWhiteSpace(_StateCode)) return _StateCode;
        if (!IsNullOrWhiteSpace(_PoliticianKey))
          _StateCode = Politicians.GetStateCodeFromKey(_PoliticianKey);
        if (!IsNullOrWhiteSpace(_StateCode)) return _StateCode;
        throw new VoteSubstitutionException(
          "Unable to determine a state code for substitutions");
      }
      set
      {
        _StateCode = value.SafeString()
          .ToUpperInvariant();
      }
    }

    public int VisitorId
    {
      private get
      {
        if (_VisitorId != null && _VisitorId != 0) return _VisitorId.Value;
        throw new VoteSubstitutionException("Unable to determine a visitor id for substitutions");
      }
      set { _VisitorId = value; }
    }

    public static Dictionary<string, string> GetAvailableSubstitutions(
      Requirements available, params string[] customSubstitutions)
    {
      var dictionary = SubstitutionsDictionary.Where(
          kvp => (kvp.Value.Requirements & available) == kvp.Value.Requirements)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.HtmlDescription);

      for (var n = 0; n < customSubstitutions.Length - 1; n += 2)
      {
        var name = customSubstitutions[n].Trim()
          .ToLowerInvariant();
        if (!dictionary.ContainsKey(name)) dictionary.Add(name, customSubstitutions[n + 1]);
      }

      return dictionary;
    }

    //public static Requirements GetRequirements(string template)
    //{
    //  var matches = ParserRegex.Matches(template);
    //  var requirements = Requirements.None;
    //  var embedded = Requirements.None;
    //  foreach (var match in matches.OfType<Match>())
    //  {
    //    // check for embedded substitution -- these will be removed from the requirements
    //    var embeddedMatch = EmbeddedSubstitutionRegex.Match(match.Value);
    //    if (embeddedMatch.Success)
    //    {
    //      var parm = embeddedMatch.Groups["parm"].Value;
    //      if (_ValidEmbeddedSubstitutions.TryGetValue(parm, out var requirement))
    //        embedded |= requirement;
    //      continue;
    //    }
    //    if (!SubstitutionsDictionary.TryGetValue(match.Value, out var substitution))
    //      if (match.Value[0] == '[')
    //      {
    //        // Check for miscoding
    //        var value = match.Value.Substring(2, match.Value.Length - 4);
    //        if (
    //          !SubstitutionsDictionary.TryGetValue("@@" + value + "@@",
    //            out substitution))
    //          SubstitutionsDictionary.TryGetValue("##" + value + "##",
    //            out substitution);
    //      }
    //    if (substitution != null) requirements |= substitution.Requirements;
    //  }

    //  // remove embedded requirements
    //  requirements = requirements & ~embedded;

    //  return requirements;
    //}

    public static Requirements GetRequirements(string template)
    {
      // We must do this in two passes now because of nesting of [[ ]] substitutiona
      var requirements = Requirements.None;
      var embedded = Requirements.None;

      var matches1 = ParserRegexPass1.Matches(template);
      foreach (var match in matches1.OfType<Match>())
      {
        // check for embedded substitution -- these will be removed from the requirements
        var embeddedMatch = EmbeddedSubstitutionRegex.Match(match.Value);
        if (embeddedMatch.Success)
        {
          var parm = embeddedMatch.Groups["parm"].Value;
          if (_ValidEmbeddedSubstitutions.TryGetValue(parm, out var requirement))
            embedded |= requirement;
          continue;
        }
        if (!SubstitutionsDictionary.TryGetValue(match.Value, out var substitution))
          if (match.Value[0] == '[')
          {
            // Check for miscoding
            var value = match.Value.Substring(2, match.Value.Length - 4);
            if (
              !SubstitutionsDictionary.TryGetValue("@@" + value + "@@",
                out substitution))
              SubstitutionsDictionary.TryGetValue("##" + value + "##",
                out substitution);
          }
        if (substitution != null) requirements |= substitution.Requirements;
      }

      var matches2 = ParserRegexPass2.Matches(template);
      foreach (var match in matches2.OfType<Match>())
      {
        if (SubstitutionsDictionary.TryGetValue(match.Value, out var substitution))
          requirements |= substitution.Requirements;
      }

      // remove embedded requirements
      requirements = requirements & ~embedded;

      return requirements;
    }

    public string Substitute(string template)
    {
      //var replaced = ParserRegex.Replace(template.SafeString(),
      //  p => OneSubstitution(p.Value));
      // split into two passes to allow [[]] to be nested inside of ## ##
      var replaced = ParserRegexPass1.Replace(template.SafeString(),
        p => OneSubstitution(p.Value));
      replaced = ParserRegexPass2.Replace(replaced,
        p => OneSubstitution(p.Value));
      return Conditionals.Evaluate(replaced);
    }

    public static Class GetClass(string substitution)
    {
      if (IsNullOrWhiteSpace(substitution)) return Class.None;
      switch (substitution[0])
      {
        case '[':
          return Class.Text;

        case '@':
          return Class.Email;

        case '#':
          return Class.Web;

        default:
          return Class.None;
      }
    }

    public static string GetClassDescription(Class @class, bool plural = false)
    {
      switch (@class)
      {
        case Class.Text:
          return plural ? "Text Substitutions" : "Text Substitution";

        case Class.Email:
          return plural ? "Email Substitutions" : "Email Substitution";

        case Class.Web:
          return plural ? "Web Substitutions" : "Web Substitution";

        default:
          return "Unknown Substitution Class";
      }
    }

    public static Dictionary<string, SubstitutionInfo> GetAllInfo()
    {
      return SubstitutionsDictionary.Select(
          kvp => new {kvp.Key, Info = kvp.Value.Clone()})
        .ToDictionary(i => i.Key, i => i.Info);
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

  public class SubstitutionInfo
  {
    public string HtmlDescription;
    public Substitutions.Requirements Requirements;
    public Substitutions.Type Type;
    public Substitutions.OptionTypes OptionTypes;
    // ReSharper disable once NotAccessedField.Global
    public int DisplayOrder;

    public SubstitutionInfo Clone()
    {
      return MemberwiseClone() as SubstitutionInfo;
    }
  }

  [Serializable]
  public class VoteSubstitutionException : VoteException
  {
    public VoteSubstitutionException()
    {
    }

    public VoteSubstitutionException(string message) : base(message)
    {
    }

    public VoteSubstitutionException(string message, params object[] args)
      : base(Format(message, args))
    {
    }

    public VoteSubstitutionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected VoteSubstitutionException(
      SerializationInfo serializationInfo, StreamingContext streamingContent)
      : base(serializationInfo, streamingContent)
    {
    }
  }
}