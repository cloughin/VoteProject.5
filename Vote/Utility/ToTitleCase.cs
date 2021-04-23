using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DB.Vote;

namespace Vote
{
  public static class ToTitleCase
  {
    static ToTitleCase()
    {
      var table = WordsSpecial.GetAllData();
      SpecialWords = table.ToDictionary(row => row.Word, row => row.Word, 
        StringComparer.OrdinalIgnoreCase);
    }

    internal static readonly Dictionary<string, string> SpecialWords;

    // Words that should not be capitalized in the middle of a title.
    private static readonly string[] SmallWords =
    {
      "a", "an", "and", "as", "at", "but", "by", "en", "for", "if", "in", "of",
      "on", "or", "the", "to", "v[.]?", "via", "vs[.]?"
    };

    // --------------------------------------------------------------------------
    // Pre-defined regular expressions
    // --------------------------------------------------------------------------

    // Small word list in regular expression format.
    private static readonly string SmallRe = string.Join("|", SmallWords);

    // Regular expression to use for splitting tokens in the input.
    private static readonly Regex SplitPattern =
      new Regex("([-:.;?!][ ]|(?:[ ]|^)[\"“])");

    // Regular expression to use for matching words to be capitalized.
    private static readonly Regex BasePattern = new Regex("\\b([A-Za-z][a-z.'’]*)\\b");

    // Regular expression to use for matching words with inline dots.
    private static readonly Regex InlineDotPattern =
      new Regex("[A-Za-z][.][A-Za-z]");

    // Regular expression to use for matching small words.
    private static readonly Regex SmallWordPattern = new Regex("\\b(" + SmallRe + ")\\b",
      RegexOptions.IgnoreCase);

    // Regular expression to use for matching small words at the beginning of the title.
    private static readonly Regex SmallWordFirstPattern =
      new Regex("^(\\W*)(" + SmallRe + ")\\b", RegexOptions.IgnoreCase);

    // Regular expression to use for matching small words at the beginning of the title.
    private static readonly Regex SmallWordLastPattern =
      new Regex("\\b(" + SmallRe + ")(\\W*)$", RegexOptions.IgnoreCase);

    // Regular expression to use for matching "v." and "vs."
    private static readonly Regex VsPattern = new Regex(" V(s?)\\. ");

    // Regular expression to use for matching apostrophe-S.
    private static readonly Regex PossessivePattern = new Regex("(['’])S\\b");

    // Regular expression to use for matching special words.
    private static readonly Regex SpecialPattern =
      new Regex("(?<=\\W|^)([-a-z&]+)(?=\\W|$)",
        RegexOptions.IgnoreCase);

    // Regular expression to use for Roman Numerals
    private static readonly Regex RomanNumeralPattern =
      new Regex("(?:\\W|^)(?:" +
        "(?:M{1,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{0,3}))|" +
        "(?:M{0,4}(?:CM|CD|D?C{1,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{0,3}))|" +
        "(?:M{0,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{1,3})(?:IX|IV|V?I{0,3}))|" +
        "(?:M{0,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{1,3}))" +
        ")(?:\\W|$)",
        RegexOptions.IgnoreCase);

    // --------------------------------------------------------------------------
    // Helper methods
    // --------------------------------------------------------------------------

    // Capitalizes the first letter in the given string.
    private static string Capitalize(string w)
    {
      return w.Substring(0, 1)
        .ToUpperInvariant() + w.Substring(1)
          .ToLowerInvariant();
    }

    // Capitalizes the first letter in the given string, unless the
    // string contains a dot in the middle of it.  This is used as
    // a replacement function in the "toTitleCase" method.
    private static string CapitalizeUnlessInlineDot(Match match)
    {
      return InlineDotPattern.IsMatch(match.Value) ? match.Value : Capitalize(match.Value);
    }

    // Capitalizes the given word, honoring any leading punctuation.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string CapitalizeFirstWord(Match match)
    {
      return match.Groups[1].Value + Capitalize(match.Groups[2].Value);
    }

    // Capitalizes the given word, honoring any leading punctuation.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string CapitalizeLastWord(Match match)
    {
      return Capitalize(match.Groups[1].Value) + match.Groups[2].Value;
    }

    // Calls "ToLowerInvariant" on the given string.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string ToLowerCase(Match match)
    {
      return match.Value.ToLowerInvariant();
    }

    // Calls "ToUpperInvariant" on the given string.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string ToUpperCase(Match match)
    {
      return match.Value.ToUpperInvariant();
    }

    // Handles v[.] and vs[.]
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string HandleVersus(Match match)
    {
      return " v" + match.Groups[1].Value + ". ";
    }

    // Handles 's
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string HandlePossessive(Match match)
    {
      return match.Groups[1].Value + "s";
    }

    // Handles words in the WordSpecial table
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string HandleSpecials(Match match)
    {
      string value;
      return SpecialWords.TryGetValue(match.Value, out value)
        ? value
        : match.Value;
    }

    // --------------------------------------------------------------------------
    // Main title case method
    // --------------------------------------------------------------------------

    public static string Do(string input)
    {
      var result = string.Empty;

      var tokens = SplitPattern.Split(input.ToLowerInvariant());
      foreach (var token in tokens)
      {
        var s = token;

        // Capitalize all words except those with inline dots.
        s = BasePattern.Replace(s, CapitalizeUnlessInlineDot);

        // Lowercase our list of small words.
        s = SmallWordPattern.Replace(s, ToLowerCase);

        // If the first word in the title is a small word, capitalize it.
        s = SmallWordFirstPattern.Replace(s, CapitalizeFirstWord);

        // If the last word in the title is a small word, capitalize it.
        s = SmallWordLastPattern.Replace(s, CapitalizeLastWord);

        result += s;
      }

      result = VsPattern.Replace(result, HandleVersus);
      result = PossessivePattern.Replace(result, HandlePossessive);
      result = SpecialPattern.Replace(result, HandleSpecials);
      result = RomanNumeralPattern.Replace(result, ToUpperCase);

      return result;
    }
  }
}