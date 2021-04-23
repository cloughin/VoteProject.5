using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Vote
{
  // Global string utilities
  public static class Str
  {

    // match any letter
    private static readonly Regex HasAlphasRegex = new Regex(@"\p{L}");

    public static bool HasAlphas(string input)
    {
      return HasAlphasRegex.Match(input)
        .Success;
    }

    public static bool IsMixedCase(string input)
    {
      var upper = input.ToUpperInvariant();
      var lower = input.ToLowerInvariant();
      return input != upper && input != lower;
    }

    // Simple minded conversion from uncased to mixed-case
    public static string ReCase(string input)
    {
      var sb = new StringBuilder(input.Length);
      var wordBegin = true;
      foreach (var c in input)
      {
        sb.Append(wordBegin ? char.ToUpper(c) : char.ToLower(c));
        wordBegin = char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsDigit(c);
      }
      return sb.ToString();
    }

    public static int ToIntOr0(string input)
    {
      if (!int.TryParse(input, out var result))
        result = 0;
      return result;
    }

    public static DateTime ToDateTime(string input, DateTime defaultValue)
    {
      if (!DateTime.TryParse(input, out var result))
        result = defaultValue;
      return result;
    }

    public static void Unreferenced() { }
  }
}