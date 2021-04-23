using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DB.Vote;

namespace Vote
{
  public static class ToSentenceCase
  {
    private class Replacement
    {
      public int Index;
      public string Phrase;
    };

    static ToSentenceCase() 
    {
      var commonTable = WordsCommon.GetAllData();
      CommonWords = commonTable.ToDictionary(row => row.Word, row => null as object,
        StringComparer.OrdinalIgnoreCase);
      var phrasesTable = WordsPhrases.GetAllData();
      Phrases = phrasesTable.ToLookup(row => row.FirstWord,
        row => row.FirstWord + PrependSpace(row.RestOfPhrase),
        StringComparer.OrdinalIgnoreCase);
      var leadersTable = WordsLeaders.GetAllData();
      Leaders = leadersTable.ToDictionary(row => AppendSpace(row.Leader),
        row => AppendSpace(row.Leader), StringComparer.OrdinalIgnoreCase);
      var followersTable = WordsFollowers.GetAllData();
      Followers = followersTable.ToDictionary(row => PrependSpace(row.Follower),
        row => PrependSpace(row.Follower), StringComparer.OrdinalIgnoreCase);
    }

    private static readonly Dictionary<string, object> CommonWords;
    private static readonly ILookup<string, string> Phrases;
    private static readonly Dictionary<string, string> Leaders;
    private static readonly Dictionary<string, string> Followers;

    // --------------------------------------------------------------------------
    // Pre-defined regular expressions
    // --------------------------------------------------------------------------

    // Regular expression to use for splitting tokens in the input.
    private static readonly Regex SplitPattern =
      new Regex("([.?!][ ]|(?:[ ]|^)[\"“])", RegexOptions.Multiline);

    // Regular expression to use for matching words to be capitalized.
    private static readonly Regex BasePattern =
      new Regex("(?<=\\W|^)([a-z]+)(?=\\W|$)",
        RegexOptions.IgnoreCase | RegexOptions.Multiline);

    // Regular expression to use for matching small words at the beginning of the title.
    private static readonly Regex WordFirstPattern = new Regex("^(\\W*)([a-z]+)",
      RegexOptions.IgnoreCase | RegexOptions.Multiline);

    // Regular expression to use for matching apostrophe-S.
    private static readonly Regex PossessivePattern = new Regex("(['’])S\\b");

    // Regular expression to use for matching special words.
    private static readonly Regex SpecialPattern =
      new Regex("(?<=\\W|^)([-a-z&]+)(?=\\W|$)",
        RegexOptions.IgnoreCase | RegexOptions.Multiline);

    // Regular expression to use for Roman Numerals
    private static readonly Regex RomanNumeralPattern =
      new Regex(
        "(?:\\W|^)(?:" +
          "(?:M{1,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{0,3}))|" +
          "(?:M{0,4}(?:CM|CD|D?C{1,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{0,3}))|" +
          "(?:M{0,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{1,3})(?:IX|IV|V?I{0,3}))|" +
          "(?:M{0,4}(?:CM|CD|D?C{0,3})(?:XC|XL|L?X{0,3})(?:IX|IV|V?I{1,3}))" +
          ")(?:\\W|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

    // --------------------------------------------------------------------------
    // Helper methods
    // --------------------------------------------------------------------------

    // Used in loading Dictionaries/Lookup
    private static string AppendSpace(string s)
    {
      return char.IsLetter(s[s.Length - 1]) ? s + " " : s;
    }

    // Used in loading Dictionaries/Lookup
    private static string PrependSpace(string s)
    {
      return char.IsLetter(s[0]) ? " " + s : s;
    }

    // Capitalizes the first letter in the given string.
    private static string Capitalize(string w)
    {
      return w.Substring(0, 1)
        .ToUpperInvariant() + w.Substring(1)
          .ToLowerInvariant();
    }

    // Capitalizes the given word, honoring any leading punctuation.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string CapitalizeFirstWord(Match match)
    {
      return match.Groups[1].Value + Capitalize(match.Groups[2].Value);
    }

    // Calls "ToUpperInvariant" on the given string.
    // This is used as a replacement function in the "ToTitleCase" method.
    private static string ToUpperCase(Match match)
    {
      return match.Value.ToUpperInvariant();
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
      return ToTitleCase.SpecialWords.TryGetValue(match.Value, out value)
        ? value
        : match.Value;
    }

    private static void LookForFollowers(Capture match, string s,
      ICollection<Replacement> replacements)
    {
      var inx = match.Index + match.Length;
      for (var i = 1; i <= 3; i++)
      {
        if (inx >= s.Length || s[inx++] != ' ') return;
        while (inx < s.Length)
          if (char.IsLetter(s[inx]))
            inx++;
          else break;
        var len = inx - (match.Index + match.Length);
        if (len <= 1) return;
        var follower = s.SafeSubstring(match.Index + match.Length, len);
        if (Followers.TryGetValue(follower, out follower))
          replacements.Add(new Replacement
            {
              Index = match.Index,
              Phrase = Capitalize(match.Value) + follower
            });
      }
    } 

    private static void LookForLeaders(Capture match, string s,
      ICollection<Replacement> replacements)
    {
      var inx = match.Index - 1;
      for (var i = 1; i <= 3; i++)
      {
        if (inx < 0 || s[inx--] != ' ') return;
        while (inx >= 0)
          if (char.IsLetter(s[inx]))
            inx--;
          else break;
        var len = match.Index - inx - 1;
        if (len <= 1) return;
        var leader = s.SafeSubstring(inx + 1, len);
        if (Leaders.TryGetValue(leader, out leader))
          replacements.Add(new Replacement
          {
            Index = inx + 1,
            Phrase = leader + Capitalize(match.Value)
          });
      }
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
        var replacements = new List<Replacement>();
        var s = token;
        var original = token;
        // Capitalize all words that aren't common.
        s = BasePattern.Replace(s, match =>
          {
            var isCommon = CommonWords.ContainsKey(match.Value);
            foreach (var phrase in Phrases[match.Value])
              if (
                phrase.IsEqIgnoreCase(original.SafeSubstring(match.Index,
                  phrase.Length)))
                replacements.Add(new Replacement
                  {
                    Index = match.Index,
                    Phrase = phrase
                  });
            if (!isCommon)
            {
              LookForLeaders(match, original, replacements);
              LookForFollowers(match, original, replacements);
            }
            return isCommon
              ? match.Value.ToLowerInvariant()
              : Capitalize(match.Value);
          });

        // Do phrase replacements
        if (replacements.Count > 0)
        {
          var sb = new StringBuilder(s);
          foreach (var replacement in replacements)
          {
            sb.Remove(replacement.Index, replacement.Phrase.Length);
            sb.Insert(replacement.Index, replacement.Phrase);
          }
          s = sb.ToString();
        } // Capitalize the first word in each token
        s = WordFirstPattern.Replace(s, CapitalizeFirstWord);

        result += s;
      }

      result = PossessivePattern.Replace(result, HandlePossessive);
      result = SpecialPattern.Replace(result, HandleSpecials);
      result = RomanNumeralPattern.Replace(result, ToUpperCase);

      return result;
    }
  }
}