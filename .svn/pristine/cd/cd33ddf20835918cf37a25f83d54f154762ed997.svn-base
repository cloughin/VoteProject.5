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

    // Enclose string in single quotes and double up any embededded 
    // single quotes and backslashes
    //public static string SQLLit(string input)
    //{
    //  input = input.Replace("\\", "\\\\");
    //  input = "'" + input.Replace("'", "''") + "'";
    //  return input;
    //}

    public static int ToIntOr0(string input)
    {
      int result;
      if (!int.TryParse(input, out result))
        result = 0;
      return result;
    }

    public static DateTime ToDateTime(string input, DateTime defaultValue)
    {
      DateTime result;
      if (!DateTime.TryParse(input, out result))
        result = defaultValue;
      return result;
    }
  }
}