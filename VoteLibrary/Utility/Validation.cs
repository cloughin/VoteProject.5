using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.String;

namespace Vote
{
  // Validation and fixup
  public static class Validation
  {
    #region Address

    // Attempts to apply mixed-casing to an all-uppercase street address
    public static string FixAddress(string address)
    {
      address = address.Trim();
      if (address == address.ToUpperInvariant())
      {
        address = Str.ReCase(address);

        // Fixup up 33rd etc.
        address = NumericSuffixToLower(address);

        // PO Box, Mail Client, Personal Mail Box
        address = IsolatedStringToUpper(address, "Pob");
        address = IsolatedStringToUpper(address, "Po");
        address = IsolatedStringToUpper(address, "Mc");
        address = IsolatedStringToUpper(address, "Pmb");

        // State Road, County Road, Rural Route, Rural Free Delivery
        address = IsolatedStringToUpper(address, "Sr");
        address = IsolatedStringToUpper(address, "Cr");
        address = IsolatedStringToUpper(address, "Rr");
        address = IsolatedStringToUpper(address, "Rfd");

        // Ordinal compass points
        address = IsolatedStringToUpper(address, "Ne");
        address = IsolatedStringToUpper(address, "Nw");
        address = IsolatedStringToUpper(address, "Se");
        address = IsolatedStringToUpper(address, "Sw");
      }
      return address;
    }

    // This is use as post-correction after a Str_ReCase for things
    // like Po -> PO and state codes. The "stringToIsolate" is the 
    // string to upcase (like "Po").
    private static string IsolatedStringToUpper(string input, string stringToIsolate)
    {
      // The match pattern makes sure the stringToIsolate 
      // is not followed by a (Unicode) letter, or is at the
      // end of the string
      var patternToIsolate = stringToIsolate + @"(?:\P{L}|$)";
      return Regex.Replace(
        input, patternToIsolate, match => match.Value.ToUpperInvariant());
    }

    // This is use as post-correction after a Str_ReCase for things 
    // like 33Rd St. -> 33rd St.
    // Matches a digit, followed by a delimited numeric suffix
    private static readonly Regex NumericSuffixToLowerRegex =
      new Regex(@"\d(?:St|Nd|Rd|Th)(?:\P{L}|$)");

    private static string NumericSuffixToLower(string input)
    {
      return NumericSuffixToLowerRegex.Replace(
        input, match => match.Value.ToLowerInvariant());
    }

    #endregion Address

    #region CityStateZip

    // matches a 2-character state code followed by a 5 or 9 digit zip
    // at the end of the string.
    private static readonly Regex FixCityStateZipRegex =
      new Regex(@"\P{L}[A-Z][a-z](?:\s+\d{5}(?:-\d{4})?)?$");

    public static string FixCityStateZip(string input)
    {
      input = input.Trim();
      if (input == input.ToUpperInvariant()) // no lowercase
      {
        input = Str.ReCase(input);

        // if the string ends with a StateCode/Zip pattern, 
        // that portion we uppercase
        input = FixCityStateZipRegex.Replace(
          input, match => match.Value.ToUpperInvariant());
      }

      if (!Str.HasAlphas(input))
        input = Empty;

      return input;
    }

    #endregion CityStateZip

    #region Given Name (First Name and Middle Name)

    // match anything but - ' ’ . space and letter
    private static readonly Regex FixGivenNameRegex = new Regex(@"[^-'’. \p{L}]");

    public static string FixGivenName(string input, bool forValidation = false)
    {
      // forValidation suppresses the addition of a period after an initial
      // so that validation of a name like "T" (no period) works ok.

      input = input.Trim();

      // strip out matching characters
      input = FixGivenNameRegex.Replace(input, match => Empty);

      var couldBe2Initials = input.Length == 2 && char.IsUpper(input[0]) &&
        char.IsUpper(input[1]);
      if (!Str.IsMixedCase(input) && !couldBe2Initials)
        input = Str.ReCase(input);

      // force period after single character initial
      if (!forValidation && input.Length == 1 && char.IsLetter(input[0]))
        input += ".";

      if (!Str.HasAlphas(input))
        input = Empty;

      return input;
    }

    #endregion Given Name (First Name and Middle Name)

    #region Last Name

    // match anything but -, en-dash . ' ’ space and letter
    private static readonly Regex FixLastNameRegex = new Regex(@"[^-‐.'’ \p{L}]");

    public static string FixLastName(string input)
    {
      input = input.Trim();

      // strip out matching characters
      input = FixLastNameRegex.Replace(input, match => Empty);

      // Special recasing for last names
      if (Str.IsMixedCase(input))
      {
        // look for names like DiPIAZZA, DeBERARDINE, LaBRUNO, McGOWAN
        if (input.Length > 2 &&
          (input.StartsWith("Di") || input.StartsWith("De") || input.StartsWith("La") ||
            input.StartsWith("Mc")) && input.Substring(2).IsAllUpperCase())
          input = input.Substring(0, 2) + Str.ReCase(input.Substring(2));
      }
      else 
        input = Str.ReCase(input);

      if (!Str.HasAlphas(input))
        input = Empty;

      if (input.Length > 2 &&
        input.StartsWith("mc", StringComparison.OrdinalIgnoreCase))
      {
        input = input.Substring(0, 2) + input.Substring(2, 1)
          .ToUpperInvariant() + input.Substring(3);
      }

      return input;
    }

    public static bool IsValidLastName(string input)
    {
      // valid if no non-letters are removed by fixup
      return
        Compare(
          input.Trim(), FixLastName(input), StringComparison.OrdinalIgnoreCase) == 0;
    }

    #endregion Last Name

    #region Name Suffix (Jr. MD etc.)

    private static readonly Dictionary<string, string> NameSuffixDictionary =
      new Dictionary<string, string>
        {
          {"JR", "Jr."},
          {"SR", "Sr."},
          {"I", "I"},
          {"II", "II"},
          {"III", "III"},
          {"IV", "IV"},
          {"V", "V"},
          {"VI", "VI"},
          {"MD", "MD"},
          {"PHD", "Ph.D."},
          {"ESQ", "Esq."},
          {"DDS", "DDS"},
          {"DVM", "DVM"},
          {"PHARM", "PharmD"},
          {"DR", "Dr."},
          {"MED", "MEd."}
        };

    public static IDictionary<string, string> GetNameSuffixDictionary()
    {
      return NameSuffixDictionary;
    }

    //public static string NameSuffixValidationMessage
    //{
    //  get
    //  {
    //    var validValues = Join(", ", NameSuffixDictionary.Values.ToArray());
    //    return $"{{0}} is invalid. It must be one of {validValues} or blank.";
    //  }
    //}

    public static string FixNameSuffix(string input)
    {
      input = input.Trim()
        .ToUpper();
      input = input.Replace(".", Empty);
      if (!NameSuffixDictionary.TryGetValue(input, out var result))
        result = Empty;
      return result;
    }

    public static bool IsValidNameSuffix(string input)
    {
      if (IsNullOrWhiteSpace(input)) return true; // missing is ok
      return FixNameSuffix(input) != Empty; // use conversion for validation
    }

    #endregion Name Suffix (Jr. MD etc.)

    #region Nickname

    // match anything but - ' ’ . space and letter
    private static readonly Regex FixNicknameRegex = new Regex(@"[^-'’. \p{L}]");

    //// like Fix_Given_Name except period not forced on single letter
    //public const string NicknameValidationMessage =
    //  "{0} can only contain letters, spaces or a dash, period or apostrophe.";

    public static string FixNickname(string input)
    {
      input = input.Trim();

      // strip out matching characters
      input = FixNicknameRegex.Replace(input, match => Empty);

      var couldBe2Initials = input.Length == 2 && char.IsUpper(input[0]) &&
        char.IsUpper(input[1]);
      if (!Str.IsMixedCase(input) && !couldBe2Initials)
        input = Str.ReCase(input);

      if (!Str.HasAlphas(input))
        input = Empty;

      return input;
    }

    public static bool IsValidNickname(string input)
    {
      // valid if no non-letters are removed by fixup
      return
        Compare(
          input.Trim(), FixNickname(input), StringComparison.OrdinalIgnoreCase) == 0;
    }

    #endregion Nickname

    #region WebAddress

    public static string StripWebProtocol(string input)
    {
      return input.Trim().RemoveHttp().RemoveMailTo();
    }

    #endregion WebAddress

    // match a beginning script tag, case insensitive
    //private static readonly Regex IsScriptRegex = new Regex(
    //  @"<script", RegexOptions.IgnoreCase);

    //public static bool IsScript(string input)
    //{
    //  return IsScriptRegex.Match(input)
    //    .Success;
    //}

    public static bool IsValidDate(string input)
    {
      DateTime dummy;
      return DateTime.TryParse(
        input, CultureInfo.InvariantCulture, DateTimeStyles.None, out dummy);
    }

    public static bool IsValidInteger(string input)
    {
      int dummy;
      return int.TryParse(input, out dummy);
    }

    // regex from regexlib.com
    //private static readonly Regex IsValidEmailAddressRegex =
    //  new Regex(
    //    @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

    // regex from msdn
    private static readonly Regex IsValidEmailAddressRegex =
      new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase);

    public static bool IsValidEmailAddress(string input)
    {
      return input != null && IsValidEmailAddressRegex.IsMatch(input.Trim());
      //return IsValidEmailAddressRegex.Match(input.Trim())
      //  .Success;
    }

    public static (DateTime date, string errorMessage) ValidateDate(string text, string name,
      DateTime? min, DateTime? max, bool allowEmpty, DateTime? def = null)
    {
      text = text.Trim();
      if (IsNullOrWhiteSpace(name)) name = "Value";
      min = min ?? DateTime.MinValue;
      max = max ?? DateTime.MaxValue;
      var date = def ?? VotePage.DefaultDbDate;
      var errorMessage = Empty;

      if (text == Empty)
      {
        if (!allowEmpty)
          errorMessage = $"{name} is required";
      }
      else if (!DateTime.TryParse(text, out date) || date < min || date > max)
      {
        errorMessage = $"{name} is not valid";
      }
      return (date, errorMessage);
    }

    public static (DateTime date, string errorMessage) ValidateDate(string text,
      string name, DateTime? min, DateTime? max)
    {
      return ValidateDate(text, name, min, max, false);
    }

    public static (DateTime date, string errorMessage) ValidateDateOptional(string text,
      string name, DateTime? def = null)
    {
      return ValidateDate(text, name, DateTime.MinValue, DateTime.MaxValue, true, def);
    }

    public static (string text, string errorMessage) ValidateLength(string text, string name, 
      int min, int max)
    {
      text = text.Trim();
      var errorMessage = Empty;
      if (min == 1) 
        return ValidateRequired(text, name);
      if (text.Length < min)
        errorMessage =
          $"{name} must be at least {min.ToString(CultureInfo.InvariantCulture)} characters";
      if (text.Length > max)
        errorMessage =
          $"{name} cannot exceed {max.ToString(CultureInfo.InvariantCulture)} characters";
      return (text, errorMessage);
    }

    public static (string text, string errorMessage) ValidateRequired(string text, string name)
    {
      text = text.Trim();
      var errorMessage = Empty;
      if (text == Empty)
        errorMessage = $"{name} is required";
      return (text, errorMessage);
    }
  }
}