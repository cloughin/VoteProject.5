using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DB.VoteLog;
using DB.VoteZipNewLocal;

namespace Vote
{

  #region Public result class

  public class AddressFinderResult
  {
    public string OriginalInput;
    public string Congress;
    public string StateSenate;
    public string StateHouse;
    public string County;
    public string ParsedAddress;
    public string ParsedCity;
    public string State;
    // ReSharper disable once NotAccessedField.Global
    public string StateName;
    public string[] Zip5S;
    public string Zip4;
    public string RedirectUrl;
    // ReSharper disable once NotAccessedField.Global
    public string RedirectHost;
    public string SuccessMessage;
    public bool Remember = true;
    // ReSharper disable once NotAccessedField.Global
    public string DomainCode;
    public readonly List<string> ErrorMessages = new List<string>();

    public override string ToString()
    {
      var sb = new StringBuilder();

      sb.Append("OriginalInput: ");
      sb.AppendLine(OriginalInput.ToStringOrNull());
      sb.Append("Congress: ");
      sb.AppendLine(Congress.ToStringOrNull());
      sb.Append("StateSenate: ");
      sb.AppendLine(StateSenate.ToStringOrNull());
      sb.Append("StateHouse: ");
      sb.AppendLine(StateHouse.ToStringOrNull());
      sb.Append("County: ");
      sb.AppendLine(County.ToStringOrNull());
      sb.Append("Address: ");
      sb.AppendLine(ParsedAddress.ToStringOrNull());
      sb.Append("City: ");
      sb.AppendLine(ParsedCity.ToStringOrNull());
      sb.Append("State: ");
      sb.AppendLine(State.ToStringOrNull());
      sb.Append("Zip: ");
      if (Zip5S == null)
        sb.Append("<null>");
      else
      {
        sb.Append(string.Join(", ", Zip5S));
        if (Zip5S.Length == 1 && Zip4 != null)
        {
          sb.Append("-");
          sb.Append(Zip4);
        }
      }
      sb.AppendLine();
      sb.Append("RedirectUrl: ");
      sb.AppendLine(RedirectUrl.ToStringOrNull());
      sb.Append("SuccessMessage: ");
      sb.AppendLine(SuccessMessage.ToStringOrNull());
      sb.AppendLine("ErrorMessages: ");
      foreach (var message in ErrorMessages)
        sb.AppendLine(message.ToStringOrNull());

      return sb.ToString();
    }

    public string ErrorMessage => string.Join(Environment.NewLine, ErrorMessages);

    public void FillHostAndState()
    {
      if (!string.IsNullOrWhiteSpace(RedirectUrl))
      {
        var tempUri = new Uri(RedirectUrl);
        var port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
        port = port == "80" ? string.Empty : ":" + port;
        RedirectHost = tempUri.Host + port;
      }
      StateName = StateCache.GetStateName(State);
    }

    public bool Success => !string.IsNullOrWhiteSpace(SuccessMessage);
  }

  #endregion

  public class AddressFinder
  {
    #region Private classes

    // Some of these are marked public so they are
    // accessible from external test code.

    #region CityZip5Codes

    // Used to store results of parsing out the city name
    private class CityZip5Codes
    {
      public readonly string StateCode;
      public readonly string[] ZipCodes;
      public bool ViaMetaphone;

      //public CityZip5Codes()
      //{
      //}

      public CityZip5Codes(IList<FilteredCityRow> cityRows)
      {
        // We assume there is at least one row, and that all
        // state codes match.

        // Since all states are the same, first will do
        StateCode = cityRows[0].State;

        // Eliminate duplicate zip codes
        ZipCodes = cityRows.GroupBy(row => row.ZipCode)
          .Select(group => group.Key)
          .ToArray();
      }

      //internal bool MatchesZip5(string Zip5)
      //{
      //  if (Zip5 == null) return false;
      //  return ZipCodes.Contains(Zip5);
      //}
    }

    #endregion CityZip5Codes

    #region DirectionalInfo

    // Used by the PossibleDirections iterator to return
    // possibilities
    private class DirectionalInfo
    {
      public string Direction;
      public int PartsUsed;
    }

    #endregion DirectionalInfo

    #region FilteredCityRow

    // Used to return the results of filtering a ZipCities
    // query for true matches
    private class FilteredCityRow
    {
      public string State;
      public string ZipCode;
    }

    #endregion FilteredCityRow

    #region HouseNumberInfo

    public class HouseNumberInfo
    {
      public string HouseNumber;
      public bool IsPoBox;
      public int HouseNumberParts;
      public string DirectionalPrefix;
      public string SecondaryPart;

      public override string ToString()
      {
        if (HouseNumber == null)
          return "<null>";
        if (IsPoBox)
          return "PO Box " + HouseNumber;

        var result = HouseNumber;
        if (!string.IsNullOrWhiteSpace(DirectionalPrefix))
          result += " " + DirectionalPrefix;
        if (!string.IsNullOrWhiteSpace(SecondaryPart))
          result += " [" + SecondaryPart + "]";
        return result;
      }
    }

    #endregion HouseNumberInfo

    #region StreetAddressParseInfo

    // Used to manage the various permutations tried during the parse and lookup
    // phase.
    public class StreetAddressParseInfo
    {
      #region Cache class

      // Because queries that differ only in DirectionalPrefix, StreetSuffix or
      // DirectionalSuffix usually return essentially the same data, we convert all
      // queries to ignore those three values, then filter the results to simulate
      // the requested results. The initial results are cached and re-used for
      // subsequent queries.

      // A cache instance includes the list of zip codes it applies to.

      // The public "Black Box" for the cache 
      public class Cache {}

      // The private true implementation 
      private class PrivateCache : Cache
      {
        private readonly Dictionary<string, List<StreetAddressDatabaseInfo>>
          _Dictionary = new Dictionary<string, List<StreetAddressDatabaseInfo>>();

        private readonly string[] _Zip5S;

        internal PrivateCache(string[] zip5S)
        {
          _Zip5S = zip5S;
        }

        private static bool Filter(StreetAddressDatabaseInfo item,
          StreetAddressParseInfo info)
        {
          if (info.DirectionalPrefix != null)
            if (
              !info.DirectionalPrefix.Equals(item.DirectionPrefix,
                StringComparison.OrdinalIgnoreCase))
              return false;

          if (info.DirectionalSuffix != null)
            if (
              !info.DirectionalSuffix.Equals(item.DirectionSuffix,
                StringComparison.OrdinalIgnoreCase))
              return false;

          if (info.StreetSuffix != null)
            if (
              !info.StreetSuffix.Equals(item.StreetSuffix,
                StringComparison.OrdinalIgnoreCase))
              return false;

          return true;
        }

        private static string GetCacheKey(StreetAddressParseInfo info, bool useMetaphone) => 
          (useMetaphone ? "@" : string.Empty) + (info.HouseNumber ?? string.Empty) + ":" +
          string.Join(".", info.StreetParts);

        internal List<StreetAddressDatabaseInfo> GetData(StreetAddressParseInfo info,
          bool useMetaphone)
        {
          // For metaphone queries, if there is any numeric content in the street parts
          // return empty result set
          if (useMetaphone)
            foreach (var part in info.StreetParts)
              if (!part.Value.IsLetters())
                return new List<StreetAddressDatabaseInfo>();

          var cacheKey = GetCacheKey(info, useMetaphone);
          List<StreetAddressDatabaseInfo> infoList;
          if (!_Dictionary.TryGetValue(cacheKey, out infoList))
          {
            // Include trailing % for partial match on last word
            string streetNamePattern;
            if (useMetaphone)
              streetNamePattern =
                DoubleMetaphone.EncodePhrase(string.Join(" ", info.StreetParts));
            else
              streetNamePattern = string.Join("%", info.StreetParts) + "%";
            var table = ZipStreets.GetDataByZipCodes(_Zip5S, info.HouseNumber, null,
              streetNamePattern, null, null, useMetaphone, 0);

            // Filter for true matches 
            infoList =
              table.Where(
                row =>
                  useMetaphone || NameMatchesParts(row.StreetName, info.StreetParts))
                .Select(row => new StreetAddressDatabaseInfo(row))
                .ToList();

            _Dictionary[cacheKey] = infoList;
          }

          // If any of the three minor field is non-null, further filtering is needed
          if (infoList.Count > 0)
            if (info.DirectionalPrefix != null || info.DirectionalSuffix != null ||
              info.StreetSuffix != null)
              infoList = infoList.Where(item => Filter(item, info))
                .ToList();

          return infoList;
        }
      }

      #endregion Cache class

      public string HouseNumber;
      public string DirectionalPrefix;
      public List<Part> StreetParts;
      public string StreetSuffix;
      public string DirectionalSuffix;
      public int PartsUsed;

      public StreetAddressParseInfo Clone() => 
        new StreetAddressParseInfo
        {
          HouseNumber = HouseNumber,
          DirectionalPrefix = DirectionalPrefix,
          StreetParts = new List<Part>(StreetParts),
          StreetSuffix = StreetSuffix,
          DirectionalSuffix = DirectionalSuffix
        };

      public static Cache CreateCache(string[] zip5S) => new PrivateCache(zip5S);

      //public List<StreetAddressDatabaseInfo> GetDatabaseData(string zipCode)
      //{
      //  if (StreetParts == null) // special for town only
      //  {
      //    ZipStreetsTable table = ZipStreets.GetDataByZipCode(zipCode);
      //    return table
      //    .Select(row => new StreetAddressDatabaseInfo(row))
      //    .ToList();
      //  }
      //  else
      //  {
      //    // Include trailing % for partial match on last word, but only on a multi word streets
      //    string streetNamePattern = string.Join("%", StreetParts) + 
      //      (StreetParts.Count > 1 ? "%" : string.Empty);
      //    ZipStreetsTable table = ZipStreets.GetDataByZipCode
      //      (zipCode, HouseNumber, DirectionalPrefix, streetNamePattern,
      //        StreetSuffix, DirectionalSuffix);

      //    // Filter for true matches and return custom object list
      //    return table
      //      .Where(row => NameMatchesParts(row.StreetName, StreetParts))
      //      .Select(row => new StreetAddressDatabaseInfo(row))
      //      .ToList();
      //  }
      //}

      //public List<StreetAddressDatabaseInfo> GetDatabaseData(string[] zipCodes,
      //  bool useMetaphone)
      //{
      //  if (StreetParts == null) // special for town only
      //  {
      //    if (useMetaphone) // nothing to use metaphone on
      //      return new List<StreetAddressDatabaseInfo>();
      //    ZipStreetsTable table = ZipStreets.GetDataByZipCodes(zipCodes);
      //    return table
      //    .Select(row => new StreetAddressDatabaseInfo(row))
      //    .ToList();
      //  }
      //  else
      //  {
      //    // Include trailing % for partial match on last word
      //    string streetNamePattern = string.Join("%", StreetParts) + "%";
      //    ZipStreetsTable table = ZipStreets.GetDataByZipCodes
      //      (zipCodes, HouseNumber, DirectionalPrefix, streetNamePattern,
      //      StreetSuffix, DirectionalSuffix, useMetaphone);

      //    // Filter for true matches and return custom object list
      //    return table
      //      .Where(row => useMetaphone || NameMatchesParts(row.StreetName, StreetParts))
      //      .Select(row => new StreetAddressDatabaseInfo(row))
      //      .ToList();
      //  }
      //}

      public List<StreetAddressDatabaseInfo> GetDatabaseData(Cache cache,
        bool useMetaphone) => (cache as PrivateCache)?.GetData(this, useMetaphone);

      public static bool NameMatchesParts(string name, List<Part> parts)
      {
        var match = PartsRegex.Match(name);
        if (match.Success) // match should always succeed
        {
          var partCaptures = match.Groups["part"].Captures;
          // The part count must match
          if (partCaptures.Count == parts.Count)
          {
            var isEqual = true;
            // Each part must match
            for (var n = 0; n < partCaptures.Count && isEqual; n++)
            {
              // To better process ad hoc abbreviations, we consider a shorter 
              // input part to match a longer db part if it matches the truncated 
              // db part, but only if its at least 5 chars long
              var dbPart = partCaptures[n].Value;
              var inputPart = parts[n].Value;
              // Partial match disabled for now (reenabled)
              if (inputPart.Length >= 5)
                isEqual = dbPart.Length >= inputPart.Length &&
                  dbPart.StartsWith(inputPart, StringComparison.OrdinalIgnoreCase);
              else
                isEqual = dbPart.Equals(inputPart, StringComparison.OrdinalIgnoreCase);
            }
            return isEqual;
          }
        }
        return false;
      }

      public override string ToString() => 
        $"[{HouseNumber}] [{DirectionalPrefix} | {string.Join(" ", StreetParts)}" +
        $" | {StreetSuffix} | {DirectionalSuffix}]";
    }

    #endregion StreetAddressParseInfo

    #region StreetAddressDatabaseInfo

    // Used to return database street info that has been filtered 
    // for true matches
    public class StreetAddressDatabaseInfo
    {
      public readonly string ZipCode;
      public readonly string DirectionPrefix;
      // ReSharper disable once NotAccessedField.Global
      public string StreetName;
      public readonly string StreetSuffix;
      public readonly string DirectionSuffix;
      public readonly string PrimaryLowNumber;
      public readonly string PrimaryHighNumber;
      public readonly string PrimaryOddEven;
      public readonly string BuildingName;
      // ReSharper disable once NotAccessedField.Global
      public string SecondaryType;
      public readonly string SecondaryLowNumber;
      public readonly string SecondaryHighNumber;
      public readonly string SecondaryOddEven;
      public readonly LdsInfo LdsInfo;
      //public string Plus4Low;
      //public string Plus4High;
      //public string State;

      public StreetAddressDatabaseInfo(ZipStreetsRow row)
      {
        ZipCode = row.ZipCode;
        DirectionPrefix = row.DirectionPrefix;
        StreetName = row.StreetName;
        StreetSuffix = row.StreetSuffix;
        DirectionSuffix = row.DirectionSuffix;
        PrimaryLowNumber = row.PrimaryLowNumber;
        PrimaryHighNumber = row.PrimaryHighNumber;
        PrimaryOddEven = row.PrimaryOddEven;
        BuildingName = row.BuildingName;
        SecondaryType = row.SecondaryType;
        SecondaryLowNumber = row.SecondaryLowNumber;
        SecondaryHighNumber = row.SecondaryHighNumber;
        SecondaryOddEven = row.SecondaryOddEven;
        LdsInfo = new LdsInfo(row.StateCode, row.Congress, row.StateSenate,
          row.StateHouse, row.County);
        //Plus4Low = row.Plus4Low;
        //Plus4High = row.Plus4High;
        //State = row.State;
      }

      public bool IsBoth => PrimaryOddEven != "O" && PrimaryOddEven != "E";

      public bool IsWildcard => PrimaryLowNumber.Length == 0 && PrimaryHighNumber.Length == 0;
    }

    #endregion StreetAddressDatabaseInfo

    #region Part

    // Immutable
    public class Part
    {
      public Part(int index, int length, string value)
      {
        Index = index;
        Length = length;
        Value = value;
      }

      public Part(Capture capture)
      {
        Index = capture.Index;
        Length = capture.Length;
        Value = capture.Value;
      }

      // Replace value
      public Part(Part part, string value)
      {
        Index = part.Index;
        Length = part.Length;
        Value = value;
      }

      public static implicit operator string(Part part) => part.ToString();

      public int Index { get; }

      public int Length { get; }

      public Part ReplaceValue(string value) => new Part(this, value);

      public override string ToString() => Value;

      public string Value { get; }
    }

    #endregion Part

    #endregion Private classes

    #region Regular expressions

    // For parsing address input
    private static readonly Regex AddressRegex =
      new Regex(
        @"^[^A-Z0-9]*(?:(?<part>[A-Z0-9]+)(?:[^A-Z0-9]+|$))*?(?:(?:(?<zip5>[0-9]{5})(?:-?(?<zip4>[0-9]{4})(?:\s*[A-Z]{2})?)?)|(?<badzip>[-0-9]+))?\s*$",
        RegexOptions.IgnoreCase);

    // For parsing a database city or street into parts
    private static readonly Regex PartsRegex =
      new Regex(@"^[^A-Z0-9]*(?:(?<part>[A-Z0-9]+)(?:[^A-Z0-9]+|$))*$",
        RegexOptions.IgnoreCase);

    // For testing street part for beginning directional
    private static readonly Regex StreetStartingDirectionalRegex =
      new Regex(@"^(?<directional>N|S|E|W|NE|NW|SE|SW)(?<name>[0-9]+.*)$",
        RegexOptions.IgnoreCase);

    // For the house number reparse: handles fractions and splits alphas from numerics
    private static readonly Regex HouseNumberRegex =
      new Regex(@"(?<part>[0-9]+/[0-9]+)|(?<part>[A-Z]+)|(?<part>[0-9]+)",
        RegexOptions.IgnoreCase);

    // For breaking parts into subparts when parsing PO Box numbers
    private static readonly Regex RegexSubparts =
      new Regex(@"^(?<subpart>[A-Z]+|[0-9]+)+$", RegexOptions.IgnoreCase);

    // For testing house number for ending directional
    private static readonly Regex PartEndingDirectionalRegex =
      new Regex(@"^.*?[0-9](?<directional>N|S|E|W|NE|NW|SE|SW)$",
        RegexOptions.IgnoreCase);

    #endregion Regular expressions

    #region Private data members

    private AddressFinderResult _Result;
    private string _ParsedInput;
    private Match _Match;
    private string _Zip5;
    private string _Zip4;
    private LdsInfo _LdsInfoFromEnteredZipPlus4;
    private List<List<Part>> _OriginalParts;
    private string _StateFromParts;
    private string _StateFromEnteredZip5;
    private string _DefaultState;
    private string _StateFromCity;
    private CityZip5Codes _CityZip5CodesFromParse;
    private bool _ResultIsComplete;

    public static readonly int MaxHouseNumberLength = // 10
      ZipStreetsDownloaded.PrimaryLowNumberMaxLength;

    public static readonly int MaxStreetNameLength = // 28
      ZipStreetsDownloaded.StreetNameMaxLength;

    //public static readonly int MaxStreetSuffixLength = // 4
    //  ZipStreetsDownloaded.StreetSuffixMaxLength;
    public static readonly int MaxCityNameLength = // 35
      ZipCitiesDownloaded.CityAliasNameMaxLength;

    //public static readonly int MaxCityAbbreviationLength = // 13
    //  ZipCitiesDownloaded.CityAliasAbbreviationMaxLength;

    private const int MaxWordsInStreet = 7;
    private const int MaxWordsInCity = 6;
    private const int MaxWordsInState = 3;

    #region CityInterchangeableNames

    private static readonly Dictionary<string, string> CityInterchangeableNames =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
          {"FORT", "FT"},
          {"FT", "FORT"},
          {"SAINT", "ST"},
          {"ST", "SAINT"}
        };

    #endregion CityInterchangeableNames

    #region StreetNumericNames

    private static readonly Dictionary<string, string> StreetNumericNames =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
          {"FIRST", "1ST"},
          {"SECOND", "2ND"},
          {"THIRD", "3RD"},
          {"FOURTH", "4TH"},
          {"FIFTH", "5TH"},
          {"SIXTH", "6TH"},
          {"SEVENTH", "7TH"},
          {"EIGHTH", "8TH"},
          {"NINTH", "9TH"},
          {"TENTH", "10TH"},
          {"ELEVENTH", "11TH"},
          {"TWELFTH", "12TH"}
        };

    #endregion StreetNumericNames

    #region StreetAlternateNames

    private static readonly Dictionary<string, string[]> StreetAlternateNames =
      new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
          {"BOULEVARD", new[] {"BLVD"}},
          {"BLVD", new[] {"BOULEVARD"}},
          {"CO", new[] {"COUNTY"}},
          {"CORD", new[] {"COUNTY ROAD"}},
          {"DOCTOR", new[] {"DR"}},
          {"DR", new[] {"DOCTOR"}},
          {"FORT", new[] {"FT"}},
          {"FT", new[] {"FORT"}},
          {"HWY", new[] {"HIGHWAY", "STATE ROUTE"}},
          {"HIGHWAY", new[] {"STATE ROUTE"}},
          {"RD", new[] {"ROAD"}},
          {"SAINT", new[] {"ST"}},
          {"ST", new[] {"SAINT"}}
        };

    private static readonly Dictionary<string, object> LimitedAlternameNames =
      new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
          {"DR", null},
          {"ST", null}
        };

    #endregion StreetAlternateNames

    #region StreetSuffixAbbreviations

    private static readonly Dictionary<string, string> StreetSuffixAbbreviations =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
          {"ALLEE", "ALY"},
          {"ALLEY", "ALY"},
          {"ALLY", "ALY"},
          {"ALY", "ALY"},
          {"ANEX", "ANX"},
          {"ANNEX", "ANX"},
          {"ANNX", "ANX"},
          {"ANX", "ANX"},
          {"ARC", "ARC"},
          {"ARCADE", "ARC"},
          {"AV", "AVE"},
          {"AVE", "AVE"},
          {"AVEN", "AVE"},
          {"AVENU", "AVE"},
          {"AVENUE", "AVE"},
          {"AVN", "AVE"},
          {"AVNUE", "AVE"},
          {"BAYOO", "BYU"},
          {"BAYOU", "BYU"},
          {"BCH", "BCH"},
          {"BEACH", "BCH"},
          {"BEND", "BND"},
          {"BND", "BND"},
          {"BLF", "BLF"},
          {"BLUF", "BLF"},
          {"BLUFF", "BLF"},
          {"BLUFFS", "BLFS"},
          {"BOT", "BTM"},
          {"BOTTM", "BTM"},
          {"BOTTOM", "BTM"},
          {"BTM", "BTM"},
          {"BLVD", "BLVD"},
          {"BOUL", "BLVD"},
          {"BOULEVARD", "BLVD"},
          {"BOULV", "BLVD"},
          {"BR", "BR"},
          {"BRANCH", "BR"},
          {"BRNCH", "BR"},
          {"BRDGE", "BRG"},
          {"BRG", "BRG"},
          {"BRIDGE", "BRG"},
          {"BRK", "BRK"},
          {"BROOK", "BRK"},
          {"BROOKS", "BRKS"},
          {"BURG", "BG"},
          {"BURGS", "BGS"},
          {"BYP", "BYP"},
          {"BYPA", "BYP"},
          {"BYPAS", "BYP"},
          {"BYPASS", "BYP"},
          {"BYPS", "BYP"},
          {"CAMP", "CP"},
          {"CMP", "CP"},
          {"CP", "CP"},
          {"CANYN", "CYN"},
          {"CANYON", "CYN"},
          {"CNYN", "CYN"},
          {"CYN", "CYN"},
          {"CAPE", "CPE"},
          {"CPE", "CPE"},
          {"CAUSEWAY", "CSWY"},
          {"CAUSWAY", "CSWY"},
          {"CSWY", "CSWY"},
          {"CEN", "CTR"},
          {"CENT", "CTR"},
          {"CENTER", "CTR"},
          {"CENTR", "CTR"},
          {"CENTRE", "CTR"},
          {"CNTER", "CTR"},
          {"CNTR", "CTR"},
          {"CTR", "CTR"},
          {"CENTERS", "CTRS"},
          {"CIR", "CIR"},
          {"CIRC", "CIR"},
          {"CIRCL", "CIR"},
          {"CIRCLE", "CIR"},
          {"CRCL", "CIR"},
          {"CRCLE", "CIR"},
          {"CIRCLES", "CIRS"},
          {"CLF", "CLF"},
          {"CLIFF", "CLF"},
          {"CLFS", "CLFS"},
          {"CLIFFS", "CLFS"},
          {"CLB", "CLB"},
          {"CLUB", "CLB"},
          {"COMMON", "CMN"},
          {"COR", "COR"},
          {"CORNER", "COR"},
          {"CORNERS", "CORS"},
          {"CORS", "CORS"},
          {"COURSE", "CRSE"},
          {"CRSE", "CRSE"},
          {"COURT", "CT"},
          {"CRT", "CT"},
          {"CT", "CT"},
          {"COURTS", "CTS"},
          {"CTS", "CTS"},
          {"COVE", "CV"},
          {"CV", "CV"},
          {"COVES", "CVS"},
          {"CK", "CRK"},
          {"CR", "CRK"},
          {"CREEK", "CRK"},
          {"CRK", "CRK"},
          {"CRECENT", "CRES"},
          {"CRES", "CRES"},
          {"CRESCENT", "CRES"},
          {"CRESENT", "CRES"},
          {"CRSCNT", "CRES"},
          {"CRSENT", "CRES"},
          {"CRSNT", "CRES"},
          {"CREST", "CRST"},
          {"CROSSING", "XING"},
          {"CRSSING", "XING"},
          {"CRSSNG", "XING"},
          {"XING", "XING"},
          {"CROSSROAD", "XRD"},
          {"CURVE", "CURV"},
          {"DALE", "DL"},
          {"DL", "DL"},
          {"DAM", "DM"},
          {"DM", "DM"},
          {"DIV", "DV"},
          {"DIVIDE", "DV"},
          {"DV", "DV"},
          {"DVD", "DV"},
          {"DR", "DR"},
          {"DRIV", "DR"},
          {"DRIVE", "DR"},
          {"DRV", "DR"},
          {"DRIVES", "DRS"},
          {"EST", "EST"},
          {"ESTATE", "EST"},
          {"ESTATES", "ESTS"},
          {"ESTS", "ESTS"},
          {"EXP", "EXPY"},
          {"EXPR", "EXPY"},
          {"EXPRESS", "EXPY"},
          {"EXPRESSWAY", "EXPY"},
          {"EXPW", "EXPY"},
          {"EXPY", "EXPY"},
          {"EXT", "EXT"},
          {"EXTENSION", "EXT"},
          {"EXTN", "EXT"},
          {"EXTNSN", "EXT"},
          {"EXTENSIONS", "EXTS"},
          {"EXTS", "EXTS"},
          {"FALL", "FALL"},
          {"FALLS", "FLS"},
          {"FLS", "FLS"},
          {"FERRY", "FRY"},
          {"FRRY", "FRY"},
          {"FRY", "FRY"},
          {"FIELD", "FLD"},
          {"FLD", "FLD"},
          {"FIELDS", "FLDS"},
          {"FLDS", "FLDS"},
          {"FLAT", "FLT"},
          {"FLT", "FLT"},
          {"FLATS", "FLTS"},
          {"FLTS", "FLTS"},
          {"FORD", "FRD"},
          {"FRD", "FRD"},
          {"FORDS", "FRDS"},
          {"FOREST", "FRST"},
          {"FORESTS", "FRST"},
          {"FRST", "FRST"},
          {"FORG", "FRG"},
          {"FORGE", "FRG"},
          {"FRG", "FRG"},
          {"FORGES", "FRGS"},
          {"FORK", "FRK"},
          {"FRK", "FRK"},
          {"FORKS", "FRKS"},
          {"FRKS", "FRKS"},
          {"FORT", "FT"},
          {"FRT", "FT"},
          {"FT", "FT"},
          {"FREEWAY", "FWY"},
          {"FREEWY", "FWY"},
          {"FRWAY", "FWY"},
          {"FRWY", "FWY"},
          {"FWY", "FWY"},
          {"GARDEN", "GDN"},
          {"GARDN", "GDN"},
          {"GDN", "GDN"},
          {"GRDEN", "GDN"},
          {"GRDN", "GDN"},
          {"GARDENS", "GDNS"},
          {"GDNS", "GDNS"},
          {"GRDNS", "GDNS"},
          {"GATEWAY", "GTWY"},
          {"GATEWY", "GTWY"},
          {"GATWAY", "GTWY"},
          {"GTWAY", "GTWY"},
          {"GTWY", "GTWY"},
          {"GLEN", "GLN"},
          {"GLN", "GLN"},
          {"GLENS", "GLNS"},
          {"GREEN", "GRN"},
          {"GRN", "GRN"},
          {"GREENS", "GRNS"},
          {"GROV", "GRV"},
          {"GROVE", "GRV"},
          {"GRV", "GRV"},
          {"GROVES", "GRVS"},
          {"HARB", "HBR"},
          {"HARBOR", "HBR"},
          {"HARBR", "HBR"},
          {"HBR", "HBR"},
          {"HRBOR", "HBR"},
          {"HARBORS", "HBRS"},
          {"HAVEN", "HVN"},
          {"HAVN", "HVN"},
          {"HVN", "HVN"},
          {"HEIGHT", "HTS"},
          {"HEIGHTS", "HTS"},
          {"HGTS", "HTS"},
          {"HT", "HTS"},
          {"HTS", "HTS"},
          {"HIGHWAY", "HWY"},
          {"HIGHWY", "HWY"},
          {"HIWAY", "HWY"},
          {"HIWY", "HWY"},
          {"HWAY", "HWY"},
          {"HWY", "HWY"},
          {"HILL", "HL"},
          {"HL", "HL"},
          {"HILLS", "HLS"},
          {"HLS", "HLS"},
          {"HLLW", "HOLW"},
          {"HOLLOW", "HOLW"},
          {"HOLLOWS", "HOLW"},
          {"HOLW", "HOLW"},
          {"HOLWS", "HOLW"},
          {"INLET", "INLT"},
          {"INLT", "INLT"},
          {"IS", "IS"},
          {"ISLAND", "IS"},
          {"ISLND", "IS"},
          {"ISLANDS", "ISS"},
          {"ISLNDS", "ISS"},
          {"ISS", "ISS"},
          {"ISLE", "ISLE"},
          {"ISLES", "ISLE"},
          {"JCT", "JCT"},
          {"JCTION", "JCT"},
          {"JCTN", "JCT"},
          {"JUNCTION", "JCT"},
          {"JUNCTN", "JCT"},
          {"JUNCTON", "JCT"},
          {"JCTNS", "JCTS"},
          {"JCTS", "JCTS"},
          {"JUNCTIONS", "JCTS"},
          {"KEY", "KY"},
          {"KY", "KY"},
          {"KEYS", "KYS"},
          {"KYS", "KYS"},
          {"KNL", "KNL"},
          {"KNOL", "KNL"},
          {"KNOLL", "KNL"},
          {"KNLS", "KNLS"},
          {"KNOLLS", "KNLS"},
          {"LAKE", "LK"},
          {"LK", "LK"},
          {"LAKES", "LKS"},
          {"LKS", "LKS"},
          {"LAND", "LAND"},
          {"LANDING", "LNDG"},
          {"LNDG", "LNDG"},
          {"LNDNG", "LNDG"},
          {"LA", "LN"},
          {"LANE", "LN"},
          {"LANES", "LN"},
          {"LN", "LN"},
          {"LGT", "LGT"},
          {"LIGHT", "LGT"},
          {"LIGHTS", "LGTS"},
          {"LF", "LF"},
          {"LOAF", "LF"},
          {"LCK", "LCK"},
          {"LOCK", "LCK"},
          {"LCKS", "LCKS"},
          {"LOCKS", "LCKS"},
          {"LDG", "LDG"},
          {"LDGE", "LDG"},
          {"LODG", "LDG"},
          {"LODGE", "LDG"},
          {"LOOP", "LOOP"},
          {"LOOPS", "LOOP"},
          {"MALL", "MALL"},
          {"MANOR", "MNR"},
          {"MNR", "MNR"},
          {"MANORS", "MNRS"},
          {"MNRS", "MNRS"},
          {"MDW", "MDW"},
          {"MEADOW", "MDW"},
          {"MDWS", "MDWS"},
          {"MEADOWS", "MDWS"},
          {"MEDOWS", "MDWS"},
          {"MEWS", "MEWS"},
          {"MILL", "ML"},
          {"ML", "ML"},
          {"MILLS", "MLS"},
          {"MLS", "MLS"},
          {"MISSION", "MSN"},
          {"MISSN", "MSN"},
          {"MSN", "MSN"},
          {"MSSN", "MSN"},
          {"MOTORWAY", "MTWY"},
          {"MNT", "MT"},
          {"MOUNT", "MT"},
          {"MT", "MT"},
          {"MNTAIN", "MTN"},
          {"MNTN", "MTN"},
          {"MOUNTAIN", "MTN"},
          {"MOUNTIN", "MTN"},
          {"MTIN", "MTN"},
          {"MTN", "MTN"},
          {"MNTNS", "MTNS"},
          {"MOUNTAINS", "MTNS"},
          {"NCK", "NCK"},
          {"NECK", "NCK"},
          {"ORCH", "ORCH"},
          {"ORCHARD", "ORCH"},
          {"ORCHRD", "ORCH"},
          {"OVAL", "OVAL"},
          {"OVL", "OVAL"},
          {"OVERPASS", "OPAS"},
          {"PARK", "PARK"},
          {"PK", "PARK"},
          {"PRK", "PARK"},
          {"PARKS", "PARK"},
          {"PARKWAY", "PKWY"},
          {"PARKWY", "PKWY"},
          {"PKWAY", "PKWY"},
          {"PKWY", "PKWY"},
          {"PKY", "PKWY"},
          {"PARKWAYS", "PKWY"},
          {"PKWYS", "PKWY"},
          {"PASS", "PASS"},
          {"PASSAGE", "PSGE"},
          {"PATH", "PATH"},
          {"PATHS", "PATH"},
          {"PIKE", "PIKE"},
          {"PIKES", "PIKE"},
          {"PINE", "PNE"},
          {"PINES", "PNES"},
          {"PNES", "PNES"},
          {"PL", "PL"},
          {"PLACE", "PL"},
          {"PLAIN", "PLN"},
          {"PLN", "PLN"},
          {"PLAINES", "PLNS"},
          {"PLAINS", "PLNS"},
          {"PLNS", "PLNS"},
          {"PLAZA", "PLZ"},
          {"PLZ", "PLZ"},
          {"PLZA", "PLZ"},
          {"POINT", "PT"},
          {"PT", "PT"},
          {"POINTS", "PTS"},
          {"PTS", "PTS"},
          {"PORT", "PRT"},
          {"PRT", "PRT"},
          {"PORTS", "PRTS"},
          {"PRTS", "PRTS"},
          {"PR", "PR"},
          {"PRAIRIE", "PR"},
          {"PRARIE", "PR"},
          {"PRR", "PR"},
          {"RAD", "RADL"},
          {"RADIAL", "RADL"},
          {"RADIEL", "RADL"},
          {"RADL", "RADL"},
          {"RAMP", "RAMP"},
          {"RANCH", "RNCH"},
          {"RANCHES", "RNCH"},
          {"RNCH", "RNCH"},
          {"RNCHS", "RNCH"},
          {"RAPID", "RPD"},
          {"RPD", "RPD"},
          {"RAPIDS", "RPDS"},
          {"RPDS", "RPDS"},
          {"REST", "RST"},
          {"RST", "RST"},
          {"RDG", "RDG"},
          {"RDGE", "RDG"},
          {"RIDGE", "RDG"},
          {"RDGS", "RDGS"},
          {"RIDGES", "RDGS"},
          {"RIV", "RIV"},
          {"RIVER", "RIV"},
          {"RIVR", "RIV"},
          {"RVR", "RIV"},
          {"RD", "RD"},
          {"ROAD", "RD"},
          {"RDS", "RDS"},
          {"ROADS", "RDS"},
          {"ROUTE", "RTE"},
          {"ROW", "ROW"},
          {"RUE", "RUE"},
          {"RUN", "RUN"},
          {"SHL", "SHL"},
          {"SHOAL", "SHL"},
          {"SHLS", "SHLS"},
          {"SHOALS", "SHLS"},
          {"SHOAR", "SHR"},
          {"SHORE", "SHR"},
          {"SHR", "SHR"},
          {"SHOARS", "SHRS"},
          {"SHORES", "SHRS"},
          {"SHRS", "SHRS"},
          {"SKYWAY", "SKWY"},
          {"SPG", "SPG"},
          {"SPNG", "SPG"},
          {"SPRING", "SPG"},
          {"SPRNG", "SPG"},
          {"SPGS", "SPGS"},
          {"SPNGS", "SPGS"},
          {"SPRINGS", "SPGS"},
          {"SPRNGS", "SPGS"},
          {"SPUR", "SPUR"},
          {"SPURS", "SPUR"},
          {"SQ", "SQ"},
          {"SQR", "SQ"},
          {"SQRE", "SQ"},
          {"SQU", "SQ"},
          {"SQUARE", "SQ"},
          {"SQRS", "SQS"},
          {"SQUARES", "SQS"},
          {"STA", "STA"},
          {"STATION", "STA"},
          {"STATN", "STA"},
          {"STN", "STA"},
          {"STRA", "STRA"},
          {"STRAV", "STRA"},
          {"STRAVE", "STRA"},
          {"STRAVEN", "STRA"},
          {"STRAVENUE", "STRA"},
          {"STRAVN", "STRA"},
          {"STRVN", "STRA"},
          {"STRVNUE", "STRA"},
          {"STREAM", "STRM"},
          {"STREME", "STRM"},
          {"STRM", "STRM"},
          {"ST", "ST"},
          {"STR", "ST"},
          {"STREET", "ST"},
          {"STRT", "ST"},
          {"STREETS", "STS"},
          {"SMT", "SMT"},
          {"SUMIT", "SMT"},
          {"SUMITT", "SMT"},
          {"SUMMIT", "SMT"},
          {"TER", "TER"},
          {"TERR", "TER"},
          {"TERRACE", "TER"},
          {"THROUGHWAY", "TRWY"},
          {"TRACE", "TRCE"},
          {"TRACES", "TRCE"},
          {"TRCE", "TRCE"},
          {"TRACK", "TRAK"},
          {"TRACKS", "TRAK"},
          {"TRAK", "TRAK"},
          {"TRK", "TRAK"},
          {"TRKS", "TRAK"},
          {"TRAFFICWAY", "TRFY"},
          {"TRFY", "TRFY"},
          {"TR", "TRL"},
          {"TRAIL", "TRL"},
          {"TRAILS", "TRL"},
          {"TRL", "TRL"},
          {"TRLS", "TRL"},
          {"TUNEL", "TUNL"},
          {"TUNL", "TUNL"},
          {"TUNLS", "TUNL"},
          {"TUNNEL", "TUNL"},
          {"TUNNELS", "TUNL"},
          {"TUNNL", "TUNL"},
          {"TPK", "TPKE"},
          {"TPKE", "TPKE"},
          {"TRNPK", "TPKE"},
          {"TRPK", "TPKE"},
          {"TURNPIKE", "TPKE"},
          {"TURNPK", "TPKE"},
          {"UNDERPASS", "UPAS"},
          {"UN", "UN"},
          {"UNION", "UN"},
          {"UNIONS", "UNS"},
          {"VALLEY", "VLY"},
          {"VALLY", "VLY"},
          {"VLLY", "VLY"},
          {"VLY", "VLY"},
          {"VALLEYS", "VLYS"},
          {"VLYS", "VLYS"},
          {"VDCT", "VIA"},
          {"VIA", "VIA"},
          {"VIADCT", "VIA"},
          {"VIADUCT", "VIA"},
          {"VIEW", "VW"},
          {"VW", "VW"},
          {"VIEWS", "VWS"},
          {"VWS", "VWS"},
          {"VILL", "VLG"},
          {"VILLAG", "VLG"},
          {"VILLAGE", "VLG"},
          {"VILLG", "VLG"},
          {"VILLIAGE", "VLG"},
          {"VLG", "VLG"},
          {"VILLAGES", "VLGS"},
          {"VLGS", "VLGS"},
          {"VILLE", "VL"},
          {"VL", "VL"},
          {"VIS", "VIS"},
          {"VIST", "VIS"},
          {"VISTA", "VIS"},
          {"VST", "VIS"},
          {"VSTA", "VIS"},
          {"WALK", "WALK"},
          {"WALKS", "WALK"},
          {"WALL", "WALL"},
          {"WAY", "WAY"},
          {"WY", "WAY"},
          {"WAYS", "WAYS"},
          {"WELL", "WL"},
          {"WELLS", "WLS"},
          {"WLS", "WLS"}
        };

    #endregion StreetSuffixAbbreviations

    #region ValidAddressPatterns and ValidPOBoxPatterns

    // For these patterns 9 represents and sequence of digits and X represents any
    // sequence of letters. XX or 99 signify consecutive sequences of digits or
    // letters. For the XX and 99 case, a hyphen is inserted in the final string. 
    // F represents any fraction (1/2, 3/7 etc). A space is inserted before the
    // fraction unless it is the first component.

    private static readonly string[] ValidPoBoxPatterns = 
      {"9", "99", "99X", "9X", "9X9", "X", "X9", "X9X"};

    private static readonly string[] ValidAddressPatterns = 
      { // These have consecutive numeric parts and will be hyphenated
        "99", "99X", "X99", "X99X",
        // These have consecutive alpha parts and will be hyphenated
        "XX", "9XX", "XX9", "9XX9",
        // These have no consecutive alpha or numeric parts
        "X", "X9", "X9X", "X9X9", "X9X9X", "X9X9X9", "9", "9X", "9X9", "9X9X",
        "9X9X9",
        // These have fractional parts
        "F", "9F", "9XF", "99F", "9X9F", "XF", "X9F", "FX", "9FX", "9XFX", "99FX",
        "9X9FX", "XFX", "X9FX"
      };

    #endregion ValidAddressPatterns and ValidPOBoxPatterns

    #endregion Private data members

    #region Constructor

    private AddressFinder() {}

    #endregion Constructor

    #region Utility methods

    public static bool IsNumericHouseNumber(string houseNumber) => 
      houseNumber.Length == MaxHouseNumberLength && houseNumber.IsDigits();

    private static int Length(List<Part> parts)
    {
      if (parts == null) return 0;
      return string.Join(" ", parts)
        .Length;
    }

    private static bool IsSingleLdsInfo(IList<StreetAddressDatabaseInfo> list)
    {
      // Returns true if the list contains exactly 1 unique LdsInfo
      if (list.Count == 0) return false;
      if (list.Count == 1) return true;
      var firstLdsInfo = list[0].LdsInfo;
      for (var n = 1; n < list.Count; n++)
        if (list[n].LdsInfo != firstLdsInfo) return false;
      return true;
    }

    private static string SpellOutNumber(int number)
    {
      if (number < 1 || number > 99)
        throw new ArgumentException("Only 1 to 99 is supported", nameof(number));

      switch (number)
      {
        case 10:
          return "TEN";

        case 11:
          return "ELEVEN";

        case 12:
          return "TWELVE";

        case 13:
          return "THIRTEEN";

        case 14:
          return "FOURTEEN";

        case 15:
          return "FIFTEEN";

        case 16:
          return "SIXTEEN";

        case 17:
          return "SEVENTEEN";

        case 18:
          return "EIGHTEEN";

        case 19:
          return "NINETEEN";

        default:
          {
            var units = SpellOutUnits(number);
            var tens = SpellOutTens(number);
            if (string.IsNullOrWhiteSpace(units))
              return tens;
            if (string.IsNullOrWhiteSpace(tens))
              return units;
            return tens + " " + units;
          }
      }
    }

    private static string SpellOutTens(int number)
    {
      switch (number/10)
      {
        case 2:
          return "TWENTY";

        case 3:
          return "THIRTY";

        case 4:
          return "FORTY";

        case 5:
          return "FIFTY";

        case 6:
          return "SIXTY";

        case 7:
          return "SEVENTY";

        case 8:
          return "EIGHTY";

        case 9:
          return "NINETY";

        default:
          return string.Empty;
      }
    }

    private static string SpellOutUnits(int number)
    {
      switch (number%10)
      {
        case 1:
          return "ONE";

        case 2:
          return "TWO";

        case 3:
          return "THREE";

        case 4:
          return "FOUR";

        case 5:
          return "FIVE";

        case 6:
          return "SIX";

        case 7:
          return "SEVEN";

        case 8:
          return "EIGHT";

        case 9:
          return "NINE";

        default:
          return string.Empty;
      }
    }

    #endregion Utility methods

    #region ZipCode analysis

    // If the entered address contains a Zip5 or a Zip+4, it is analyzed first.
    // The initial Regex parse treats the Zip specially -- it is never included
    // in the general parts list.
    //
    // A complete Zip+4 is taken to be definitive, and ends the parse be it 
    // good or bad.

    private void AnalyzeZipCode()
    {
      // Extract zip components
      var group = _Match.Groups["zip5"];
      if (group.Captures.Count == 1)
      {
        _Zip5 = group.Captures[0].Value;
        group = _Match.Groups["zip4"];
        if (group.Captures.Count == 1)
          _Zip4 = group.Captures[0].Value;
      }

      // Get the Zip+4 info if available
      _LdsInfoFromEnteredZipPlus4 = GetLdsInfo(_Zip5, _Zip4);

      // Get state from Zip5
      if (_Zip5 != null)
      {
        var state = ZipCitiesDownloaded.GetFirstStateByZipCode(_Zip5);
        if (StateCache.IsValidStateCode(state))
          _StateFromEnteredZip5 = state;
      }

      // If there is a bad zip, extract it.
      // A bad zip is any ending sequence of digits (plus hyphens) that
      // does not contain either 5 or 9 digits, or that has a misplaced hyphen.
      group = _Match.Groups["badzip"];
      string badZip = null;
      if (group != null && group.Captures.Count == 1)
        badZip = group.Captures[0].Value;

      if (badZip != null)
      {
        var message = $"This zipcode does not appear to be valid: {badZip}";
        _Result.ErrorMessages.Add(message);
        _ResultIsComplete = true;
      }
      else // if we have a full ZIP+4 and no parts we are good to go
        if (_Zip5 != null && _Zip4 != null)
        {
          if (_LdsInfoFromEnteredZipPlus4 == null)
          {
            var message = $"We could not find this ZIP+4: {_Zip5}-{_Zip4}";
            _Result.ErrorMessages.Add(message);
          }
          else
            _LdsInfoFromEnteredZipPlus4.ApplyToResult(_Result);
          _ResultIsComplete = true;
        }
    }

    public static LdsInfo GetLdsInfo(string zip5, string zip4)
    {
      if (zip5 == null || zip4 == null) return null;

      var table = Uszd.GetDataByZip5Zip4(zip5, zip4, 0);
      if (table.Count == 0) return null;
      var row = table[0];
      var stateCode = StateCache.StateCodeFromLdsStateCode(row.LdsStateCode);
      return new LdsInfo(stateCode, row.Congress, row.StateSenate, row.StateHouse,
        row.County);
    }

    public static LdsInfo GetLdsInfoOrMissing(string zip5, string zip4)
    {
      var ldsInfo = GetLdsInfo(zip5, zip4) ?? LdsInfo.Missing;
      return ldsInfo;
    }

    #endregion ZipCode analysis

    #region Parse setup

    // If there is no explicit Zip+4, the parse proceeds by extracting the 
    // parts list, performing a special front-end parse for the house number,
    // and then extracting the state and city, working from the end forward.

    private void ExtractPreZipParts()
    {
      var group = _Match.Groups["part"];
      _OriginalParts = new List<List<Part>>
      {
        group.Captures.OfType<Capture>()
          .Select(capture => new Part(capture))
          .ToList()
      };
    }

    private void InitParse(string input, string defaultState, bool remember = true)
    {
      _Result = null;

      input = input.Trim();
      if (input.Length == 0)
        _ResultIsComplete = true;
      else
      {
        _DefaultState = defaultState;

        // Create and initialize the result object
        _Result = new AddressFinderResult
        {
          OriginalInput = input,
          Remember = remember
        };

        // One special pre-parse transformation
        input = input.Replace("&", " AND ");
        _ParsedInput = input;

        // Do the initial parse -- it should never fail no matter what the input
        _Match = AddressRegex.Match(input);
        if (!_Match.Success)
        {
          _Result.ErrorMessages.Add("Match failure");
          _ResultIsComplete = true;
        }
      }

      if (!_ResultIsComplete)
      {
        AnalyzeZipCode();
        if (!_ResultIsComplete)
        {
          ExtractPreZipParts();
          FindStateFromParts();
          FindCityFromParts();
        }
      }
    }

    #endregion Parse setup

    #region Parse state

    private void FindStateFromParts()
    {
      if (_OriginalParts == null || _OriginalParts.Count < 1 ||
        _OriginalParts[0].Count < 1)
        return;

      var parts = _OriginalParts[0];
      var partsUsed = 0;
      string stateCode = null;

      // We try the last "part" (everything up to the zip) as a state
      // name or abbreviation, then keep trying by concatenating up to
      // MaxWordsInState (space separated).
      var valueToTry = string.Empty;
      for (var nParts = 1; nParts <= MaxWordsInState; nParts++)
        if (parts.Count >= nParts)
        {
          var spacer = nParts == 1 ? string.Empty : " ";
          valueToTry = parts[parts.Count - nParts].Value + spacer + valueToTry;
          // This handles a wide variety of abbreviations
          stateCode = StateCache.GetStateCode(valueToTry);
          if (stateCode != null)
          {
            partsUsed = nParts;
            break;
          }
        }
        else break;

      // If there is a StateFromEnteredZip5 and it doesn't match what we found here,
      // ignore what we found here -- it's probably spurious
      if (_StateFromEnteredZip5 != null && _StateFromEnteredZip5 != stateCode)
      {
        partsUsed = 0;
        stateCode = null;
      }

      // Remove used parts from parts list
      while (partsUsed-- > 0)
        parts.RemoveAt(parts.Count - 1);

      _StateFromParts = stateCode;
    }

    #endregion Parse state

    #region Parse city

    private void FindCityFromParts()
    {
      if (_OriginalParts == null || _OriginalParts.Count < 1 ||
        _OriginalParts[0].Count < 1)
        return;

      var parts = _OriginalParts[0];
      var partsUsed = 0;
      CityZip5Codes zip5Codes = null;

      // We determine the state code to be used according to this priority list:
      //  1. The state code we determined from parsing the text
      //  2. The state code from the Zip5
      //  3. The default state code based on the current site context (vote-xx.org)
      // If there is no state code, we try a lookup on just the city, and if
      // all results are from a single state, we use it.
      var stateCode = BestState;

      // After the zip and state have been extracted, we try the remaining parts 
      // as a city name. We start the longest possible list of parts (up to
      // MaxWordsInCity, but no parts with numerics). We separate them by '%' 
      // for the 'LIKE' database search. 

      // Create the (possible) city parts list. We start with the last part
      // and work forward. Parts of a city name never contain numerics, so we 
      // stop if we encounter any such parts.
      var cityParts = new List<string>();
      var maxParts = Math.Min(parts.Count, MaxWordsInCity);
      for (var nParts = 1; nParts <= maxParts; nParts++)
      {
        var part = parts[parts.Count - nParts].Value;
        if (!part.IsLetters()) break; // no numbers allowed in city names
        cityParts.Insert(0, part);
      }

      var done = false;
      for (var useMetaphone = 0; useMetaphone <= 1; useMetaphone++)
      {
        // We go from longest to shortest to take longest match
        for (var nParts = cityParts.Count; nParts > 0 && !done; nParts--)
        {
          var usedParts = cityParts.Skip(cityParts.Count - nParts)
            .ToList();
          // ReSharper disable once UnusedVariable
          // variant is a dummy variable
          foreach (var variant in PossibleCityPartsVariants(usedParts))
          {
            var separator = useMetaphone == 0 ? "%" : " ";
            var valueToTry = string.Join(separator, usedParts);
            if (valueToTry.Length > MaxCityNameLength) continue;
            bool abbreviationMatch;

            var table = GetZipCitiesData(stateCode, valueToTry, useMetaphone == 1,
              out abbreviationMatch);

            List<FilteredCityRow> filteredCityRows = null;
            if (table.Count > 0)
              filteredCityRows = FilterTrueCityMatches(usedParts, table,
                useMetaphone == 1 || abbreviationMatch);

            if (filteredCityRows != null && filteredCityRows.Count > 0)
              // found match(es)
            {
              // For stateCode == null, we need to make sure the results are from a
              // single state to safely infer the state from the city (e.g. Chicago)
              if (stateCode == null && GetStateCount(filteredCityRows) != 1)
                continue;

              // We have a good state. If we have no Zip5, we go with this one. 
              // If we do have a Zip5, make sure it's a match
              if (_Zip5 != null)
                if (filteredCityRows.All(row => row.ZipCode != _Zip5))
                  continue;

              zip5Codes = new CityZip5Codes(filteredCityRows) {ViaMetaphone = useMetaphone == 1};
              partsUsed = nParts;
              done = true;
              break;
            }
          }
        }

        if (zip5Codes == null && stateCode == "DC")
          zip5Codes = GetImpliedCityForDc();

        if (zip5Codes != null)
        {
          // We deal with the case of an explicit Zip5 where the apparent
          // city name might actually be a street name. To account for this, 
          // create a second copy of the parts list and do not remove the
          // city parts from it. This will only be used if the main parts list
          // does not produce a match.
          if (_Zip5 != null)
            _OriginalParts.Add(new List<Part>(parts));

          // Remove used parts from (the original) parts list
          while (partsUsed-- > 0)
            parts.RemoveAt(parts.Count - 1);

          // Save the one we found
          _CityZip5CodesFromParse = zip5Codes;
          _StateFromCity = zip5Codes.StateCode;
          return;
        }

        // We skip metaphone if there's no state, or if we have a Zip5
        if (stateCode == null || _Zip5 != null) return;
      }
    }

    private static IEnumerable<int> PossibleCityPartsVariants(List<string> parts)
    {
      // This iterator takes care of interchangeables in the city name, like 
      // ST <-> SAINT. The iterator return is a dummy variable.
      yield return 1;

      for (var n = 0; n < parts.Count; n++)
      {
        string substitute;
        if (CityInterchangeableNames.TryGetValue(parts[n], out substitute))
        {
          var original = parts[n];
          parts[n] = substitute;
          yield return 1;
          parts[n] = original;
        }
      }

      // Try pasting adjacent pairs
      if (parts.Count > 1)
      {
        var originalParts = new List<string>(parts);
        for (var n = 1; n < parts.Count; n++)
        {
          parts.Clear();
          parts.AddRange(originalParts);
          parts[n - 1] += parts[n];
          parts.RemoveAt(n);
          yield return 1;
        }
        parts.Clear();
        parts.AddRange(originalParts);
      }
    }

    private string BestState
    {
      get
      {
        // Return stateCode from "best" source
        if (_StateFromParts != null)
          return _StateFromParts;
        if (_StateFromEnteredZip5 != null)
          return _StateFromEnteredZip5;
        if (_DefaultState != null)
          return _DefaultState;
        return _StateFromCity;
      }
    }

    private static List<FilteredCityRow> FilterTrueCityMatches(IList<string> parts,
      ZipCitiesDownloadedTable table, bool exactMatch)
    {
      // Because we use wildcards when fetching the city names (to render special 
      // characters insignificant), we need to split into parts and compare to make 
      // sure we don't have a spurious match.
      //
      // To better process ad hoc abbreviations, we consider a shorter input part
      // to match a longer db part if it matches the truncated db part.
      //
      // After an exact match, we just pass everything
      if (exactMatch)
        return
          table.Select(
            row => new FilteredCityRow {State = row.State, ZipCode = row.ZipCode})
            .ToList();
      var list = new List<FilteredCityRow>();
      foreach (var row in table)
      {
        // Split the city name from the DB into parts
        var match = PartsRegex.Match(row.CityAliasName);
        if (match.Success) // match should always succeed
        {
          var cityPartCaptures = match.Groups["part"].Captures;
          // The part count must match and so must the Zip5 if present
          if (cityPartCaptures.Count == parts.Count)
          {
            // ** Test: remove the Zip5 match requirement to better handle
            // bad Zip5's
            //bool isEqual = Zip5 == null || Zip5 == row.ZipCode;
            var isEqual = true;
            // Each part must match
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (isEqual)
              for (var n = 0; n < cityPartCaptures.Count && isEqual; n++)
              {
                var cityPart = cityPartCaptures[n].Value;
                var inputPart = parts[n];
                isEqual &= cityPart.Length >= inputPart.Length &&
                  cityPart.StartsWith(inputPart, StringComparison.OrdinalIgnoreCase);
              }
            if (isEqual)
              list.Add(new FilteredCityRow
              {
                State = row.State,
                ZipCode = row.ZipCode
              });
          }
        }
      }
      return list;
    }

    private static CityZip5Codes GetImpliedCityForDc()
    {
      CityZip5Codes parseInfo;
      // Test: return all DC zips even if we have one entered
      //if (Zip5 == null)
      {
        // Get all DC zip codes
        bool abbreviationMatch;
        var table = GetZipCitiesData("DC", "Washington", false, out abbreviationMatch);
        // convert the table to a list -- no need to filter
        var list =
          table.Select(
            row => new FilteredCityRow {State = row.State, ZipCode = row.ZipCode})
            .ToList();
        parseInfo = new CityZip5Codes(list);
      }
      //else
      //  parseInfo = new CityZip5Codes()
      //  {
      //    // Use the one zip code we have
      //    StateCode = "DC",
      //    //Names = new string[] { "Washington" },
      //    ZipCodes = new string[] { Zip5 }
      //  };

      return parseInfo;
    }

    private static int GetStateCount(IEnumerable<FilteredCityRow> cityRow) => 
      cityRow.GroupBy(row => row.State)
      .Count();

    private static ZipCitiesDownloadedTable GetZipCitiesData(string stateCode,
      string valueToTry, bool useMetaphone, out bool abbreviationMatch)
    {
      abbreviationMatch = false;
      ZipCitiesDownloadedTable table;

      // If we have no state code, we look up the city for any state. If the
      // result is only found in one state, we can infer the state, but not with
      // metaphone.
      if (stateCode == null) // no state available
        table = useMetaphone ? null : ZipCitiesDownloaded.GetCityAliasesDataLikeCityAliasName(valueToTry);
      else // use the state we have
        if (useMetaphone)
        {
          var metaphone = DoubleMetaphone.EncodePhrase(valueToTry);
          table =
            ZipCitiesDownloaded.GetCityAliasesDataByStateMetaphoneAliasName(
              stateCode, metaphone);
        }
        else
          table =
            ZipCitiesDownloaded.GetCityAliasesDataByStateLikeCityAliasName(
              stateCode, valueToTry);

      return table;
    }

    #endregion Parse city

    #region House number parsing

    private IEnumerable<HouseNumberInfo> PossibleHouseNumbers(IList<Part> parts)
    {
      if (parts.Count == 0) yield break;

      // First, see if we begin with a POBox, either P O BOX, PO BOX POBOX or BOX
      var poBoxParts = 0;
      if (parts[0].Value.IsEqIgnoreCase("POBOX") ||
        parts[0].Value.IsEqIgnoreCase("BOX"))
        poBoxParts = 1;
      else if (parts.Count >= 2 && parts[0].Value.IsEqIgnoreCase("PO") &&
        parts[1].Value.IsEqIgnoreCase("BOX"))
        poBoxParts = 2;
      else if (parts.Count >= 3 && parts[0].Value.IsEqIgnoreCase("P") &&
        parts[1].Value.IsEqIgnoreCase("O") &&
        parts[2].Value.IsEqIgnoreCase("BOX"))
        poBoxParts = 3;

      if (poBoxParts > 0) // we have a POBox
      {
        foreach (var info in PossiblePoBoxes(parts, poBoxParts))
          yield return info;
        yield break;
      }

      // We have a number of special issues
      //  * Fractions - these require a reparse of the subset of the input not used
      //    by city, state and zip.
      //  * a directional appended to the house number
      //  * a possible apartment or suite number appended to the house number by a hyphen

      // Here we apply a heuristic. If the first part is all alpha, we try the
      // missing house number first, otherwise last

      var tryMissingFirst = parts[0].Value.IsLetters();

      if (tryMissingFirst)
        yield return
          new HouseNumberInfo
            {
              HouseNumber = string.Empty,
              IsPoBox = false,
              HouseNumberParts = 0,
              DirectionalPrefix = null
            };

      // We try house numbers from shortest to longest.
      for (var lastPartNumber = 0; lastPartNumber < parts.Count; lastPartNumber++)
        //for (int lastPartNumber = parts.Count - 1; lastPartNumber >= 0; lastPartNumber--)
      {
        var lastPart = parts[lastPartNumber];
        var originalLength = lastPart.Index + lastPart.Length;
        var originalString = _ParsedInput.Substring(0, originalLength);

        // Now we reparse with a special regex that captures fractions and breaks
        // out the alphas and the numerics separately
        var partPatterns = new List<string>();
        var subParts = new List<string>();
        HouseNumberRegex.Replace(originalString, match =>
          {
            subParts.Add(match.Value);
            if (match.Value.IsDigits())
              partPatterns.Add("9");
            else if (match.Value.IsLetters())
              partPatterns.Add("X");
            else
              partPatterns.Add("F");
            return match.Value;
          });

        var partPattern = string.Join(string.Empty, partPatterns);
        if (ValidAddressPatterns.Contains(partPattern)) // good pattern
        {
          // Is a hyphen needed?
          var doubleUp = partPattern.IndexOf("99", StringComparison.Ordinal);
          if (doubleUp == -1) doubleUp = partPattern.IndexOf("XX", StringComparison.Ordinal);
          if (doubleUp >= 0) // insert hyphen at location + 1
          {
            subParts.Insert(doubleUp + 1, "-");
            // Insert a place holder to maintain sync for the next test below
            partPattern = partPattern.Insert(doubleUp + 1, "-");
          }

          // Do we need a space before the F?
          var fractionIndex = partPattern.IndexOf("F", StringComparison.Ordinal);
          if (fractionIndex > 0) // not if at 0
            subParts.Insert(fractionIndex, " ");

          var houseNumber = string.Join(string.Empty, subParts);
          // No point if houseNumber is too long
          if (houseNumber.Length <= MaxHouseNumberLength)
          {
            if (houseNumber.IsDigits())
              houseNumber = houseNumber.ZeroPad(MaxHouseNumberLength);
            yield return
              new HouseNumberInfo
                {
                  HouseNumber = houseNumber,
                  IsPoBox = false,
                  HouseNumberParts = lastPartNumber + 1,
                  DirectionalPrefix = null
                };
          }

          // check for a trailing directional embedded in last part
          if (subParts.Count > 1)
          {
            var subPartsCopy = new List<string>(subParts);
            var match = PartEndingDirectionalRegex.Match(lastPart.Value);
            if (match.Success) // the directional will be the last sub-part
            {
              subPartsCopy.RemoveAt(subPartsCopy.Count - 1);
              partPattern = partPattern.Substring(0, partPattern.Length - 1);
              // There could be a hyphen to remove
              if (subPartsCopy.Count > 1 &&
                partPattern[partPattern.Length - 1] == '-')
              {
                subPartsCopy.RemoveAt(subPartsCopy.Count - 1);
                partPattern = partPattern.Substring(0, partPattern.Length - 1);
              }
              if (ValidAddressPatterns.Contains(partPattern)) // good pattern
              {
                houseNumber = string.Join(string.Empty, subPartsCopy);
                // No point if houseNumber is too long
                if (houseNumber.Length <= MaxHouseNumberLength)
                {
                  if (houseNumber.IsDigits())
                    houseNumber = houseNumber.ZeroPad(MaxHouseNumberLength);
                  yield return
                    new HouseNumberInfo
                      {
                        HouseNumber = houseNumber,
                        IsPoBox = false,
                        HouseNumberParts = lastPartNumber + 1,
                        DirectionalPrefix = match.FirstCapture("directional")
                      };
                }
              }
            }
          }

          // If we are hyphenated and no fraction, try the post-hyphen part as
          // an apartment number
          if (doubleUp >= 0 && fractionIndex < 0)
          {
            houseNumber = string.Join(string.Empty, subParts.Take(doubleUp + 1));
            if (houseNumber.IsDigits())
              houseNumber = houseNumber.ZeroPad(MaxHouseNumberLength);
            yield return
              new HouseNumberInfo
                {
                  HouseNumber = houseNumber,
                  IsPoBox = false,
                  HouseNumberParts = lastPartNumber + 1,
                  SecondaryPart =
                    string.Join(string.Empty, subParts.Skip(doubleUp + 2))
                };
          }
        }
      }

      if (!tryMissingFirst)
        yield return
          new HouseNumberInfo
            {
              HouseNumber = string.Empty,
              IsPoBox = false,
              HouseNumberParts = 0,
              DirectionalPrefix = null
            };
    }

    private static IEnumerable<HouseNumberInfo> PossiblePoBoxes(IList<Part> parts,
      int poBoxParts)
    {
      if (parts.Count <= poBoxParts)
      {
        // Bare POBox
        yield return
          new HouseNumberInfo
            {
              HouseNumber = string.Empty,
              IsPoBox = true,
              HouseNumberParts = poBoxParts,
              DirectionalPrefix = null
            };
        yield break;
      }

      // To generate the remaining possibilities, we examine the remaining
      // parts and break them into subparts, then create a pattern. If the
      // pattern is valid, we try it. We go largest to smallest. Since the 
      // longest POBox pattern is 3, we start with no more than 3 parts.

      var startPart = poBoxParts;
      var maxPart = Math.Min(poBoxParts + 2, parts.Count - 1);

      for (var endPart = maxPart; endPart >= startPart; endPart--)
      {
        var partPatterns = new List<string>();
        var subParts = new List<string>();

        // Collect the subParts
        for (var n = startPart; n <= endPart; n++)
        {
          var match = RegexSubparts.Match(parts[n].Value);
          var captures = match.Groups["subpart"].Captures;
          foreach (Capture capture in captures)
          {
            var value = capture.Value;
            partPatterns.Add(value.IsDigits() ? "9" : "X");
            subParts.Add(value);
          }
        }

        var partPattern = string.Join(string.Empty, partPatterns);
        if (ValidPoBoxPatterns.Contains(partPattern)) // good pattern
        {
          // Is a hyphen needed?
          var doubleUp = partPattern.IndexOf("99", StringComparison.Ordinal);
          if (doubleUp == -1) doubleUp = partPattern.IndexOf("XX", StringComparison.Ordinal);
          if (doubleUp >= 0) // insert hyphen at location + 1
            subParts.Insert(doubleUp + 1, "-");

          var houseNumber = string.Join(string.Empty, subParts);
          // No point if houseNumber is too long
          if (houseNumber.Length <= MaxHouseNumberLength)
          {
            if (houseNumber.IsDigits())
              houseNumber = houseNumber.ZeroPad(MaxHouseNumberLength);
            yield return
              new HouseNumberInfo
                {
                  HouseNumber = houseNumber,
                  IsPoBox = true,
                  HouseNumberParts = endPart + 1,
                  DirectionalPrefix = null
                };
          }
        }
      }
    }

    #endregion House number parsing

    #region Street address parsing

    private void TryParsePossibilities()
    {
      var ambiguous = false;
      var ok = false;

      var zip5S = GetZip5S();

      if (zip5S.Length > 0)
      {
        // First check if there is enough info without parsing the address,
        // but not for a metaphone city match -- too much chance of a false match.
        // ReSharper disable once MergeSequentialChecksWhenPossible
        if (_CityZip5CodesFromParse == null || !_CityZip5CodesFromParse.ViaMetaphone)
          CheckForSingleUszd(zip5S);
        if (_ResultIsComplete) return;

        var infoCache = StreetAddressParseInfo.CreateCache(zip5S);
        for (var useMetaphone = 0;
          useMetaphone <= 1 && !ok && !ambiguous;
          useMetaphone++)
          foreach (var parts in _OriginalParts)
          {
            if (parts.Count == 0) ambiguous = true;
            foreach (var info in PossibleStreetAddressPatterns(parts))
            {
              // Get the filtered data from the ZipStreets table
              var dbList = info.GetDatabaseData(infoCache, useMetaphone == 1);
              ResolveAmbiguity(dbList, parts, info);
              if (IsSingleLdsInfo(dbList))
              {
                dbList[0].LdsInfo.ApplyToResult(_Result);
                _Result.Zip5S = dbList.Select(i => i.ZipCode)
                  .ToArray();
                ok = true; // we have a good result
                // For now we use the address portion of the raw
                // input as the parsed address -- later we can try
                // to clean it up, or clean it up when we extract it
                // from the database
                if (parts.Count == 0)
                  _Result.ParsedAddress = string.Empty;
                else
                {
                  var lastPart = parts[parts.Count - 1];
                  var start = parts[0].Index;
                  var end = lastPart.Index + lastPart.Length;
                  _Result.ParsedAddress =
                    _Result.OriginalInput.Substring(start, end - start)
                      .ToUpper();
                }
                break;
              }
              if (dbList.Count > 1)
                ambiguous = true;
            }
            if (ok) break;
          }
      }

      if (!ok)
        _Result.ErrorMessages.Add(
          ambiguous
            ? "We found more than one matching legislative district. Could you supply a bit more information?"
            : "We could not find the address you entered. Please check your spelling and try again.");
    }

    private string[] GetZip5S()
    {
      var zip5SList = new List<string>();
      if (_CityZip5CodesFromParse != null)
        zip5SList.AddRange(_CityZip5CodesFromParse.ZipCodes);
      // Make sure any entered Zip5 is at the top of the list
      if (_Zip5 != null)
      {
        var inx = zip5SList.IndexOf(_Zip5);
        if (inx > 0)
          zip5SList.RemoveAt(inx);
        if (inx != 0)
          zip5SList.Insert(0, _Zip5);
      }
      var zip5S = zip5SList.ToArray();
      return zip5S;
    }

    private void CheckForSingleUszd(string[] zip5S)
    {
      var table = ZipSingleUszd.GetDataByZipCodes(zip5S);

      // First check that every zip had a row in the table
      if (table.Count == zip5S.Length)
      {
        // Now summarize and see if we wind up with exactly one
        var dictionary = new Dictionary<LdsInfo, object>();
        foreach (var row in table)
        {
          var ldsInfo = new LdsInfo(row.StateCode, row.Congress, row.StateSenate,
            row.StateHouse, row.County);
          dictionary[ldsInfo] = null;
        }
        if (dictionary.Count == 1) // we are good
        {
          dictionary.Keys.Single()
            .ApplyToResult(_Result);
          _Result.ErrorMessages.Add("Zip5 match");
          foreach (var zip5 in zip5S)
            _Result.ErrorMessages.Add(zip5);
          _ResultIsComplete = true;
        }
      }
    }

    private AddressFinderResult FindPrivate(string input, string defaultState, bool remember = true)
    {
      // The main line, private version.The public static methods are thin wrappers
      // for this method
      InitParse(input, defaultState, remember);
      if (_Zip5 != null)
        _Result.Zip5S = new[] {_Zip5};
      _Result.Zip4 = _Zip4;
      if (!_ResultIsComplete)
        TryParsePossibilities();

      if (_Result.Success && _Result.ParsedCity == null && _Result.Zip5S != null &&
        _Result.Zip5S.Length > 0) // success with zip
        _Result.ParsedCity =
          ZipCitiesDownloaded.GetPrimaryCityByZipCode(_Result.Zip5S[0]);

      return _Result;
    }

    private static string StandardizeStreetSuffix(string streetSuffix)
    {
      if (streetSuffix == null) return null;
      string abbreviation;
      StreetSuffixAbbreviations.TryGetValue(streetSuffix, out abbreviation);
      return abbreviation;
    }

    #endregion Street address parsing

    #region Ambiguity resolution

    private static void ResolveAmbiguity(List<StreetAddressDatabaseInfo> list,
      ICollection<Part> parts, StreetAddressParseInfo info)
    {
      if (list.Count == 0 || IsSingleLdsInfo(list)) return;

      // First, if there are a mix of wildcard and non-wildcard entries, remove the
      // wildcards, but only if there is a house number
      if (!string.IsNullOrWhiteSpace(info.HouseNumber))
      {
        RemoveSuperfluousWildcards(list);
        SelectTightestNumericRange(list);
        if (IsSingleLdsInfo(list))
          return;
      }

      // If we have some left over parts, see if any of them match the secondary
      // address data.
      if (parts.Count <= info.PartsUsed)
        return; // nothing to match with

      // Create a list of remaining parts
      var remainingParts = parts.Skip(info.PartsUsed)
        .ToList();

      // If we have a secondary range, see if any of the parts match
      if (MatchSecondaryRange(list, remainingParts))
        return;

      // Try to match the parts against the Building
      if (MatchBuilding(list, remainingParts))
        return;

      // Try to match the parts against the Building using metaphone
      if (MatchBuildingMetaphone(list, remainingParts))
        // ReSharper disable once RedundantJumpStatement
        return;
    }

    private static void SelectTightestNumericRange(
      List<StreetAddressDatabaseInfo> list)
    {
      // Make sure they are all numerics
      var nonNumerics =
        list.Count(
          info =>
            !info.PrimaryLowNumber.IsDigits() || !info.PrimaryHighNumber.IsDigits());
      if (nonNumerics != 0) return;

      // Group by tightness and select the tightest group
      var newList =
        list.GroupBy(
          info =>
            int.Parse(info.PrimaryHighNumber) - int.Parse(info.PrimaryLowNumber))
          .OrderBy(group => group.Key)
          .First()
          .ToList();

      // Only bother replacing if there were exclusions
      if (newList.Count < list.Count)
      {
        list.Clear();
        list.AddRange(newList);
      }
    }

    public static IEnumerable<string> PossibleSecondaryParts(List<Part> parts,
      bool joinWithSpaces)
    {
      // Combines the remaining parts every which way
      for (var partCount = 1; partCount <= parts.Count; partCount++)
        for (var startIndex = 0; startIndex + partCount <= parts.Count; startIndex++)
          if (joinWithSpaces)
            yield return string.Join(" ", parts.Skip(startIndex)
              .Take(partCount));
          else
          {
            // just paste together unless we have adjacent letters or numbers, then
            // use a hyphen
            string joined = parts[startIndex];
            for (var n = 1; n < partCount; n++)
            {
              var nextValue = parts[startIndex + n].Value;
              if (char.IsDigit(joined[joined.Length - 1]) ==
                char.IsDigit(nextValue[0]))
                joined += "-";
              joined += nextValue;
            }
            yield return joined;
          }
    }

    private static bool MatchSecondaryRange(List<StreetAddressDatabaseInfo> list,
      List<Part> remainingParts)
    {
      foreach (var s in PossibleSecondaryParts(remainingParts, false))
      {
        var upper = s.ToUpperInvariant();
        var matches =
          list.Where(
            i =>
              MatchesOddEven(i.SecondaryOddEven, s) &&
                i.SecondaryLowNumber.Length > 0 && i.SecondaryHighNumber.Length > 0 &&
                i.SecondaryLowNumber.ToUpperInvariant()
                  .CompareAlphanumeric(upper) <= 0 &&
                i.SecondaryHighNumber.ToUpperInvariant()
                  .CompareAlphanumeric(upper) >= 0)
            .ToList();
        if (IsSingleLdsInfo(matches)) // bingo
        {
          list.Clear();
          list.AddRange(matches);
          return true;
        }
      }
      return false;
    }

    private static bool MatchBuilding(List<StreetAddressDatabaseInfo> list,
      List<Part> remainingParts)
    {
      foreach (var s in PossibleSecondaryParts(remainingParts, true))
      {
        var matches = list.Where(i => i.BuildingName.IsEqIgnoreCase(s))
          .ToList();
        if (IsSingleLdsInfo(matches)) // bingo
        {
          list.Clear();
          list.AddRange(matches);
          return true;
        }
      }
      return false;
    }

    private static bool MatchBuildingMetaphone(List<StreetAddressDatabaseInfo> list,
      List<Part> remainingParts)
    {
      foreach (var s in PossibleSecondaryParts(remainingParts, true))
      {
        var metaphone = DoubleMetaphone.EncodePhrase(s);
        var matches = list.Where(i => DoubleMetaphone.EncodePhrase(i.BuildingName)
          .IsEqIgnoreCase(metaphone))
          .ToList();
        if (IsSingleLdsInfo(matches)) // bingo
        {
          list.Clear();
          list.AddRange(matches);
          return true;
        }
      }
      return false;
    }

    private static bool MatchesOddEven(string oddEven, string value)
    {
      if (oddEven != "O" && oddEven != "E") return true;
      for (var n = value.Length - 1; n >= 0; n--)
      {
        var lastDigit = value[n];
        if (char.IsDigit(lastDigit))
          if (lastDigit%2 == 0) // is even so exclude odd
            return oddEven == "E";
          else
            return oddEven == "O";
      }
      return true;
    }

    private static void RemoveSuperfluousWildcards(
      IList<StreetAddressDatabaseInfo> list)
    {
      // Make sure its not all or no windcards
      var wildcardCount = list.Count(info => info.IsWildcard);
      if (wildcardCount > 0 && wildcardCount < list.Count)
      {
        var n = 0;
        while (n < list.Count)
          if (list[n].IsWildcard)
            list.RemoveAt(n);
          else
            n++;
      }

      // We also remove superfluous "B" (both) entries
      var bothCount = list.Count(info => info.IsBoth);
      if (bothCount > 0 && bothCount < list.Count)
      {
        var n = 0;
        while (n < list.Count)
          if (list[n].IsBoth)
            list.RemoveAt(n);
          else
            n++;
      }
    }

    #endregion Ambiguity resolution

    #region Iterator methods for trying various possibilities

    private static IEnumerable<List<Part>> PossibleAlternateStreetNames(List<Part> parts,
      bool paste)
    {
      if (Length(parts) <= MaxStreetNameLength)
        yield return parts;

      // Here is where we translate from things like seventh to 7th
      if (parts.Count == 1) // only 1 word names are translated
      {
        var originalName = parts[0].Value;
        string alternateName;
        if (StreetNumericNames.TryGetValue(originalName, out alternateName))
        {
          var result = new List<Part> {parts[0].ReplaceValue(alternateName)};
          if (Length(result) <= MaxStreetNameLength)
            yield return result;
        }

        // We add an ordinal suffix to a bare number < 1000
        var suffix = GetNumericSuffix(originalName);
        if (suffix != null)
        {
          var result = new List<Part> {parts[0].ReplaceValue(originalName + suffix)};
          if (Length(result) <= MaxStreetNameLength)
            yield return result;
        }
      }

      // All numeric components from 1 to 99 are tried spelled out
      for (var n = 0; n < parts.Count; n++)
        if (parts[n].Value.IsDigits())
        {
          var number = int.Parse(parts[n].Value);
          if (number > 0 && number < 100)
          {
            var substitute = SpellOutNumber(number)
              .Split(' ');
            var substituteList = new List<Part>(parts) {[n] = parts[n].ReplaceValue(substitute[0])};
            if (substitute.Length == 2) // no more than 2
              substituteList.Insert(n + 1,
                new Part(substituteList[n].Index + substituteList[n].Length, 0,
                  substitute[1]));
            if (Length(substituteList) <= MaxStreetNameLength)
              yield return substituteList;
          }
        }

      // If any component has an alternate name, we make the substitution.
      // We don't translate limited interchangeables (like ST -> SAINT) unless 
      // the context is multi word and it's not the last word
      for (var n = 0; n < parts.Count; n++)
      {
        var ok = true;
        if (LimitedAlternameNames.ContainsKey(parts[n].Value))
          if (parts.Count == 1 || n == parts.Count - 1)
            ok = false;
        if (ok)
        {
          string[] substitutes;
          if (StreetAlternateNames.TryGetValue(parts[n].Value, out substitutes))
            foreach (var substitute in substitutes)
            {
              var substituteParts = substitute.Split(' ');
              if (substituteParts.Length > 2)
                throw new VoteException("More than 2 parts in street alternate name");
              var substituteList = new List<Part>(parts)
              {
                [n] = parts[n].ReplaceValue(substituteParts[0])
              };
              if (substituteParts.Length == 2) // no more than 2
                substituteList.Insert(n + 1,
                  new Part(substituteList[n].Index + substituteList[n].Length, 0,
                    substituteParts[1]));
              if (Length(substituteList) <= MaxStreetNameLength)
                yield return substituteList;
            }
        }
      }

      // If there is more than one part we try pasting them together. We only paste
      // adjacent pairs for now.
      if (paste && parts.Count > 1)
        for (var pasteIndex = 1; pasteIndex < parts.Count; pasteIndex++)
        {
          var pastedParts = new List<Part>();
          var pastedPart = new Part(parts[pasteIndex - 1].Index,
            parts[pasteIndex].Length + parts[pasteIndex].Index -
              parts[pasteIndex - 1].Index,
            parts[pasteIndex - 1].Value + parts[pasteIndex].Value);
          pastedParts.AddRange(parts.Take(pasteIndex - 1));
          pastedParts.Add(pastedPart);
          pastedParts.AddRange(parts.Skip(pasteIndex + 1));
          foreach (var alternate in PossibleAlternateStreetNames(pastedParts, false))
            yield return alternate;
        }
    }

    private static string GetNumericSuffix(string originalName)
    {
      string suffix = null;
      int numericStreet;
      if (int.TryParse(originalName, out numericStreet) && numericStreet < 1000)
        switch (numericStreet%100)
        {
            // 11, 12, 13, 111, 112, 113 are special cases
          case 11:
          case 12:
          case 13:
            suffix = "TH";
            break;

          default:
            switch (numericStreet%10)
            {
              case 1:
                suffix = "ST";
                break;

              case 2:
                suffix = "ND";
                break;

              case 3:
                suffix = "RD";
                break;

              default:
                suffix = "TH";
                break;
            }
            break;
        }
      return suffix;
    }

    private IEnumerable<StreetAddressParseInfo> PossibleStreetAddressPatterns(
      IList<Part> parts)
    {
      if (parts.Count == 0) yield break;

      foreach (var houseNumberInfo in PossibleHouseNumbers(parts))
        if (houseNumberInfo.IsPoBox)
          yield return
            new StreetAddressParseInfo
              {
                HouseNumber = houseNumberInfo.HouseNumber,
                StreetParts =
                  new List<Part> {new Part(0, 0, "PO"), new Part(0, 0, "BOX")},
                PartsUsed = houseNumberInfo.HouseNumberParts
              };
        else
        {
          // The following is to prevent something like 727-17 being interpreted as 727 17th St.
          if (houseNumberInfo.HouseNumberParts > 0 &&
            houseNumberInfo.HouseNumberParts < parts.Count &&
            parts[houseNumberInfo.HouseNumberParts].Value.IsDigits())
          {
            var index = parts[houseNumberInfo.HouseNumberParts].Index - 1;
            if (index >= 0 && _ParsedInput[index] == '-')
              houseNumberInfo.HouseNumberParts++; // don't use the first part
          }

          foreach (var info in
            PossiblePatternsBeginningWithDirectionalPrefix(parts,
              houseNumberInfo.HouseNumberParts, houseNumberInfo.DirectionalPrefix))
          {
            info.HouseNumber = houseNumberInfo.HouseNumber;

            // If there is a directional prefix, we also try without it
            StreetAddressParseInfo clone = null;
            if (info.DirectionalPrefix != null)
              clone = info.Clone();

            yield return info;

            if (clone != null)
            {
              clone.DirectionalPrefix = null;
              yield return clone;
            }
          }

          //// A second pass omitting the house number
          //foreach (StreetAddressParseInfo info in
          // PossiblePatternsBeginningWithDirectionalPrefix(parts,
          //   houseNumberInfo.HouseNumberParts, houseNumberInfo.DirectionalPrefix))
          //  yield return info;
        }

      // If we have not matched yet, we try a second pass with just the street
      // name, if not PO Box
      foreach (var houseNumberInfo in PossibleHouseNumbers(parts))
        if (!houseNumberInfo.IsPoBox)
          foreach (var info in
            PossiblePatternsBeginningWithDirectionalPrefix(parts,
              houseNumberInfo.HouseNumberParts, houseNumberInfo.DirectionalPrefix))
            if (!string.IsNullOrWhiteSpace(houseNumberInfo.HouseNumber))
              yield return info;
    }

    private static IEnumerable<StreetAddressParseInfo>
      PossiblePatternsBeginningWithDirectionalPrefix(IList<Part> parts,
        int startingPart, string prefixFromHouseNumber)
    {
      // Variants with a directionalPrefix
      foreach (var dirInfo in PossibleDirections(parts, startingPart))
      {
        var newStartingPart = startingPart + dirInfo.PartsUsed;
        if (newStartingPart < parts.Count)
          foreach (var addrInfo in
            PossiblePatternsBeginningWithStreetName(parts, newStartingPart))
          {
            StreetAddressParseInfo cloned = null;
            if (addrInfo.DirectionalSuffix == null)
              cloned = addrInfo.Clone();

            addrInfo.DirectionalPrefix = dirInfo.Direction;
            yield return addrInfo;

            // This might be a misplaced suffix, so if we do not already have
            // a suffix, try this one
            if (cloned != null)
            {
              cloned.DirectionalSuffix = dirInfo.Direction;
              yield return cloned;
            }
          }
      }

      // Handle the address-based prefix
      if (!string.IsNullOrWhiteSpace(prefixFromHouseNumber))
        foreach (var addrInfo in
          PossiblePatternsBeginningWithStreetName(parts, startingPart))
        {
          addrInfo.DirectionalPrefix = prefixFromHouseNumber;
          yield return addrInfo;
        }

      // Variants without a directionalPrefix
      foreach (var addrInfo in
        PossiblePatternsBeginningWithStreetName(parts, startingPart))
      {
        StreetAddressParseInfo cloned = null;
        if (addrInfo.DirectionalSuffix != null)
          cloned = addrInfo.Clone();

        yield return addrInfo;

        // This might be a misplaced prefix, so if we have a suffix, 
        // we try it
        if (cloned != null)
        {
          cloned.DirectionalPrefix = cloned.DirectionalSuffix;
          cloned.DirectionalSuffix = null;
        }
      }

      // Special variant -- if first part starts with a valid directional
      // followed by a number, try splitting it into a directional and a number.
      if (startingPart < parts.Count)
      {
        var partToCheck = parts[startingPart];
        var match = StreetStartingDirectionalRegex.Match(partToCheck.Value);
        if (match.Success)
        {
          var directional = match.Groups["directional"].Captures[0].Value;
          var name = match.Groups["name"].Captures[0].Value;
          // temp replacement
          parts[startingPart] = partToCheck.ReplaceValue(name);
          foreach (var addrInfo in
            PossiblePatternsBeginningWithStreetName(parts, startingPart))
          {
            addrInfo.DirectionalPrefix = directional;
            yield return addrInfo;
          }
          // restore original
          parts[startingPart] = partToCheck;
        }
      }
    }

    private static IEnumerable<StreetAddressParseInfo>
      PossiblePatternsBeginningWithStreetName(IList<Part> parts, int startingPart)
    {
      var maxParts = Math.Min(parts.Count - startingPart, MaxWordsInStreet);
      if (maxParts <= 0) yield break;

      // Try longest to shortest
      // On this pass we reserve a part for the suffix
      for (var n = maxParts - 1; n >= 1; n--)
      {
        // Parts list for this iteration
        var streetParts = new List<Part>();
        for (var m = 1; m <= n; m++)
          streetParts.Add(parts[startingPart + m - 1]); // initialize with all parts

        // for each street name variation we have these possibilities
        //  - street streetSuffix directionalSuffix
        //  - street streetSuffix
        //  - street directionalSuffix
        //  - street

        var nextPart = startingPart + n;
        if (nextPart < parts.Count) // proceed
        {
          // First try street streetSuffix variants if we have one that's valid
          var streetSuffix = StandardizeStreetSuffix(parts[nextPart].Value);
          if (streetSuffix != null)
          {
            var newStartingPart = nextPart + 1;

            // Try with directionalSuffix
            if (newStartingPart < parts.Count) // there could be a directionalSuffix
              foreach (var dirInfo in PossibleDirections(parts, newStartingPart))
                foreach (
                  var streetList in PossibleAlternateStreetNames(streetParts, true))
                  yield return
                    new StreetAddressParseInfo
                      {
                        StreetParts = streetList,
                        StreetSuffix = streetSuffix,
                        DirectionalSuffix = dirInfo.Direction,
                        PartsUsed = newStartingPart + 1
                      };

            // Now return the variant without the directional suffix
            foreach (
              var streetList in PossibleAlternateStreetNames(streetParts, true))
              yield return
                new StreetAddressParseInfo
                  {
                    StreetParts = streetList,
                    StreetSuffix = streetSuffix,
                    PartsUsed = nextPart + 1
                  };
          }

          // Finally try no streetSuffix but with directionalSuffix
          foreach (var dirInfo in PossibleDirections(parts, nextPart))
            foreach (
              var streetList in PossibleAlternateStreetNames(streetParts, true))
              yield return
                new StreetAddressParseInfo
                  {
                    StreetParts = streetList,
                    DirectionalSuffix = dirInfo.Direction,
                    PartsUsed = nextPart + 1
                  };
        }

        //// Lastly, we try just the street
        //foreach (var streetList in PossibleAlternateStreetNames(streetParts))
        //  yield return new StreetAddressParseInfo()
        //  {
        //    StreetParts = streetList
        //  };
      }

      // As a last resort, we try just the street
      for (var n = maxParts; n >= 1; n--)
      {
        // Parts list for this iteration
        var streetParts = new List<Part>();
        for (var m = 1; m <= n; m++)
          streetParts.Add(parts[startingPart + m - 1]); // initialize with all parts

        foreach (var streetList in PossibleAlternateStreetNames(streetParts, true))
          yield return
            new StreetAddressParseInfo
              {
                StreetParts = streetList,
                PartsUsed = startingPart + n
              };
      }
    }

    private static IEnumerable<DirectionalInfo> PossibleDirections(IList<Part> parts,
      int startingPart)
    {
      if (startingPart >= parts.Count)
        yield break;

      var dir1 = parts[startingPart].Value.ToUpperInvariant();

      // Convert full names to and long abbreviations to short abbreviations
      switch (dir1)
      {
        case "NORTH":
        case "NO":
          dir1 = "N";
          break;

        case "SOUTH":
        case "SO":
          dir1 = "S";
          break;

        case "EAST":
          dir1 = "E";
          break;

        case "WEST":
          dir1 = "W";
          break;

        case "NORTHEAST":
          dir1 = "NE";
          break;

        case "NORTHWEST":
          dir1 = "NW";
          break;

        case "SOUTHEAST":
          dir1 = "SE";
          break;

        case "SOUTHWEST":
          dir1 = "SW";
          break;
      }

      if (dir1 == "N" || dir1 == "S" || dir1 == "E" || dir1 == "W" || dir1 == "NE" ||
        dir1 == "SE" || dir1 == "NW" || dir1 == "SW")
        yield return new DirectionalInfo {Direction = dir1, PartsUsed = 1};

      startingPart++;
      if ((dir1 == "N" || dir1 == "S") && startingPart < parts.Count)
      {
        var dir2 = parts[startingPart].Value.ToUpperInvariant();

        // Convert full names to abbreviations
        switch (dir1)
        {
          case "EAST":
            dir1 = "E";
            break;

          case "WEST":
            dir1 = "W";
            break;
        }

        if (dir2 == "E" || dir2 == "W")
          yield return new DirectionalInfo {Direction = dir1 + dir2, PartsUsed = 2};
      }
    }

    #endregion Iterator methods for trying various possibilities

    #region Public methods

    // Includes optional logging and email
    public static AddressFinderResult Find(string input, bool log, string email, bool remember = true)
    {
      var result = Find(input, null, remember);
      result.FillHostAndState();
      //if (log /*&& !string.IsNullOrWhiteSpace(email)*/) result.Log(email);
      return result;
    }

    //public static AddressFinderResult Find(string input)
    //{
    //  var result = Find(input, null);
    //  result.FillHostAndState();
    //  return result;
    //}

    public static AddressFinderResult Find(string input, string defaultState, bool remember = true)
    {
      return new AddressFinder().FindPrivate(input, defaultState, remember);
    }

    //public static ParseResults Parse(string input)
    //{
    //  return Parse(input, null);
    //}

    //public static ParseResults Parse(string input, string defaultState)
    //{
    //  return (new AddressFinder()).ParsePrivate(input, defaultState);
    //}

    #endregion Public methods
  }
}