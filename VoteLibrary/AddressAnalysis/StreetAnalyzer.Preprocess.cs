using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vote
{
  public partial class StreetAnalyzer
  {
    #region Private classes

    private enum PartType
    {
      Alpha,
      Numeric,
      Fraction
    }

    private class AddressPart
    {
      private AddressPart(string key, PartType type)
      {
        Key = key;
        Type = type;
      }

      public static AddressPart CreateAlpha(string key) =>
        new AddressPart(key, PartType.Alpha);

      public static AddressPart CreateFraction(string key) =>
        new AddressPart(key, PartType.Fraction);

      public static AddressPart CreateNumeric(string key) =>
        new AddressPart(key, PartType.Numeric);

      public string Key { get; }

      public PartType Type { get; }
    }

    private class Range
    {
      public string Low;
      public string High;
    }

    #endregion Private classes

    #region Private data members

    private static readonly Regex Regex99Hyphen99 =
      new Regex(
        @"^(?<prenumber1>[0-9]*?)-?(?<prealpha1>[A-Z]*)-?(?<number1>[0-9]+)-(?<number2>[0-9]+)-?(?<postalpha1>[A-Z]*)-?(?<postnumber1>[0-9]*)$",
        RegexOptions.IgnoreCase);

    // ReSharper disable once InconsistentNaming
    private static readonly Regex RegexXXHyphenXX =
      new Regex(
        @"^(?<prenumber1>[0-9]*)-?(?<alpha1>[A-Z]+)-(?<alpha2>[A-Z]+)-?(?<postnumber1>[0-9]*)$",
        RegexOptions.IgnoreCase);

    // ReSharper disable once InconsistentNaming
    private static readonly Regex RegexXX99XX99XX =
      new Regex(
        @"^(?<alpha1>[A-Z]*)-?(?<number1>[0-9]*)-?(?<alpha2>[A-Z]*)-?(?<number2>[0-9]*)-?(?<alpha3>[A-Z]*)-?(?<number3>[0-9]*)$",
        RegexOptions.IgnoreCase);

    private static readonly Regex RegexFractional =
      new Regex(
        @"^(?:(?<prefix>(?:[0-9]*[A-Z]+)-?)|(?:[0-9]+-))??\s?(?:(?<number>[0-9]+)(?<suffix1>[A-Z]+)?-?\s)?(?<fraction>1/8|2/8|3/8|4/8|5/8|6/8|7/8|1/7|2/7|3/7|4/7|5/7|6/7|1/6|2/6|3/6|4/6|5/6|1/5|2/5|3/5|4/5|1/4|2/4|3/4|1/3|2/3|1/2|1/1)(?<suffix2>[A-Z]*)$",
        RegexOptions.IgnoreCase);

    #endregion Private data members

    private static void ExpandPreprocessRange(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions,
      int low, int high, string pattern)
    {
      if (low.Digits() == high.Digits())
      {
        data.PrimaryLowNumber = string.Format(pattern, low);
        data.PrimaryHighNumber = string.Format(pattern, high);
      }
      else
      {
        var sequence = 0;
        foreach (var range in RangePartitions(low, high))
        {
          sequence++;
          var toAdd = data.Clone();
          toAdd.UpdateKey += '-' + sequence.ToString().ZeroPad(2);
          toAdd.PrimaryLowNumber = string.Format(pattern, range.Low);
          toAdd.PrimaryHighNumber = string.Format(pattern, range.High);
          additions.Add(toAdd);
        }
        deletions.Add(data);
      }
    }

    private static void ExpandFractionRange(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions,
      string low, string high, string pattern)
    {
      if (low == high)
      {
        data.PrimaryLowNumber = string.Format(pattern, low);
        data.PrimaryHighNumber = string.Format(pattern, high);
      }
      else
      {
        var sequence = 0;
        foreach (var range in FractionPartitions(low, high))
        {
          sequence++;
          var toAdd = data.Clone();
          toAdd.UpdateKey += '-' + sequence.ToString().ZeroPad(2);
          toAdd.PrimaryLowNumber = string.Format(pattern, range.Low);
          toAdd.PrimaryHighNumber = string.Format(pattern, range.High);
          additions.Add(toAdd);
        }
        deletions.Add(data);
      }
    }

    private static bool FindVariablePart(IList<AddressPart> parts,
      string oddEven, IList<string> lows, IList<string> highs, IList<string> constants,
      ref int variableIndex, ref int numericLow, ref int numericHigh)
    {
      for (var n = 0; n < parts.Count; n++)
      {
        if (lows[n] == highs[n]) // constant
          if (parts[n].Type == PartType.Numeric)
            constants[n] = NormalizeNumber(lows[n]);
          else
            constants[n] = lows[n];
        else // variable
        if (variableIndex >= 0) // multiple variables
          return false;
        else // validate the variable
        {
          variableIndex = n;
          switch (parts[n].Type)
          {
            case PartType.Alpha:
              if (!IsValidVariableString(lows[n], highs[n]))
                return false;
              break;

            case PartType.Numeric:
              if (highs[n].Length == 0)
                return false;
              numericLow = GetLowNumber(oddEven, lows[n]);
              numericHigh = GetHighNumber(highs[n]);
              if (numericLow > numericHigh)
                return false;
              break;
          }
        }
      }

      return true;
    }

    private static IEnumerable<Range> FractionPartitions(string low, string high)
    {
      var list = new List<string> {low};

      // Always return low

      switch (low)
      {
        case "1/8":
          switch (high)
          {
            case "3/8":
              list.Add("2/8", "1/4");
              break;

            case "4/8":
            case "1/2":
              list.Add("2/8", "1/4", "3/8");
              break;

            case "5/8":
              list.Add("2/8", "1/4", "3/8", "4/8", "1/2");
              break;

            case "6/8":
            case "3/4":
              list.Add("2/8", "1/4", "3/8", "4/8", "1/2", "5/8");
              break;

            case "7/8":
              list.Add("2/8", "1/4", "3/8", "4/8", "1/2", "5/8", "6/8", "3/4");
              break;
          }
          break;

        case "2/8":
          switch (high)
          {
            case "4/8":
            case "1/2":
              list.Add("3/8");
              break;

            case "5/8":
              list.Add("3/8", "4/8", "1/2");
              break;

            case "6/8":
            case "3/4":
              list.Add("3/8", "4/8", "1/2", "5/8");
              break;

            case "7/8":
              list.Add("3/8", "4/8", "1/2", "5/8", "6/8", "3/4");
              break;
          }
          break;

        case "3/8":
          switch (high)
          {
            case "5/8":
              list.Add("4/8", "1/2");
              break;

            case "6/8":
            case "3/4":
              list.Add("4/8", "1/2", "5/8");
              break;

            case "7/8":
              list.Add("4/8", "1/2", "5/8", "6/8", "3/4");
              break;
          }
          break;

        case "4/8":
          switch (high)
          {
            case "6/8":
            case "3/4":
              list.Add("5/8");
              break;

            case "7/8":
              list.Add("5/8", "6/8", "3/4");
              break;
          }
          break;

        case "5/8":
          switch (high)
          {
            case "7/8":
              list.Add("6/8", "3/4");
              break;
          }
          break;

        case "1/7":
          switch (high)
          {
            case "3/7":
              list.Add("2/7");
              break;

            case "4/7":
              list.Add("2/7", "3/7");
              break;

            case "5/7":
              list.Add("2/7", "3/7", "4/7");
              break;

            case "6/7":
              list.Add("2/7", "3/7", "4/7", "5/7");
              break;
          }
          break;

        case "2/7":
          switch (high)
          {
            case "4/7":
              list.Add("3/7");
              break;

            case "5/7":
              list.Add("3/7", "4/7");
              break;

            case "6/7":
              list.Add("3/7", "4/7", "5/7");
              break;
          }
          break;

        case "3/7":
          switch (high)
          {
            case "5/7":
              list.Add("4/7");
              break;

            case "6/7":
              list.Add("4/7", "5/7");
              break;
          }
          break;

        case "4/7":
          switch (high)
          {
            case "6/7":
              list.Add("5/7");
              break;
          }
          break;

        case "1/6":
          switch (high)
          {
            case "3/6":
            case "1/2":
              list.Add("2/6", "1/3");
              break;

            case "4/6":
            case "2/3":
              list.Add("2/6", "1/3", "3/6", "1/2");
              break;

            case "5/6":
              list.Add("2/6", "1/3", "3/6", "1/2", "4/6", "2/3");
              break;
          }
          break;

        case "2/6":
          switch (high)
          {
            case "4/6":
            case "2/3":
              list.Add("3/6", "1/2");
              break;

            case "5/6":
              list.Add("3/6", "1/2", "4/6", "2/3");
              break;
          }
          break;

        case "3/6":
          switch (high)
          {
            case "5/6":
              list.Add("4/6", "2/3");
              break;
          }
          break;

        case "1/5":
          switch (high)
          {
            case "3/5":
              list.Add("2/5");
              break;

            case "4/5":
              list.Add("2/5", "3/5");
              break;
          }
          break;

        case "2/5":
          switch (high)
          {
            case "4/5":
              list.Add("3/5");
              break;
          }
          break;

        case "1/4":
          switch (high)
          {
            case "4/8":
              list.Add("3/8");
              break;

            case "5/8":
              list.Add("3/8", "4/8", "1/2");
              break;

            case "6/8":
              list.Add("3/8", "4/8", "1/2", "5/8");
              break;

            case "3/4":
              list.Add("1/2");
              break;

            case "7/8":
              list.Add("3/8", "4/8", "1/2", "5/8", "6/8", "3/4");
              break;
          }
          break;

        case "2/4":
          switch (high)
          {
            case "6/8":
              list.Add("5/8");
              break;

            case "7/8":
              list.Add("5/8", "6/8", "3/4");
              break;
          }
          break;

        case "1/3":
          switch (high)
          {
            case "4/6":
              list.Add("3/6");
              break;

            case "5/6":
              list.Add("3/6", "4/6", "2/3");
              break;
          }
          break;

        case "1/2":
          switch (high)
          {
            case "6/8":
              list.Add("5/8");
              break;

            case "7/8":
              list.Add("5/8", "6/8");
              break;
          }
          break;
      }

      // Always return high
      list.Add(high);

      return list.Select(fraction => new Range {Low = fraction, High = fraction});
    }

    private static void GetAllAddressParts(Match matchLow, Match matchHigh, IList<AddressPart> parts,
      IList<string> lows, IList<string> highs)
    {
      for (var n = 0; n < parts.Count; n++)
      {
        lows[n] = matchLow.FirstCaptureOrEmpty(parts[n].Key);
        highs[n] = matchHigh.FirstCaptureOrEmpty(parts[n].Key);
      }
    }

    private static int GetHighNumber(string highNumber)
    {
      if (highNumber.Length == 0)
        throw new VoteException();
      return int.Parse(highNumber);
    }

    private static int GetLowNumber(string oddEven, string lowNumber)
    {
      if (lowNumber.Length == 0)
        return oddEven == "E" ? 2 : 1;
      return int.Parse(lowNumber);
    }

    public static bool IsValidVariableString(string low, string high)
    {
      // The strings must be the same length, low must be <= high, and they must
      // vary only in the final character.
      if ((low.Length == 0) && (high.Length == 0))
        return true;
      if ((low.Length == 0) || (high.Length == 0))
        return false;
      if (low.Substring(0, low.Length - 1) != high.Substring(0, high.Length - 1))
        return false;
      if (low[low.Length - 1] > high[high.Length - 1])
        return false;
      return true;
    }

    private static void NormalizeAndExpandAddress(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions, ICollection<StreetAnalysisData> additions,
      string pattern, IList<AddressPart> parts, IList<string> lows, IList<string> highs,
      string[] constants, int variableIndex, int numericLow, int numericHigh)
    {
      if (variableIndex == -1) // no variables, straight substitution
      {
        // ReSharper disable once CoVariantArrayConversion
        var address = string.Format(pattern, constants);
        data.PrimaryLowNumber = address;
        data.PrimaryHighNumber = address;
      }
      else
      {
        // modify the pattern with constants and a single variable
        constants[variableIndex] = "{0}";
        // ReSharper disable once CoVariantArrayConversion
        var pattern2 = string.Format(pattern, constants);
        switch (parts[variableIndex].Type)
        {
          case PartType.Alpha:
            // Straight substitution to normalize
            data.PrimaryLowNumber = string.Format(pattern2, lows[variableIndex]);
            data.PrimaryHighNumber = string.Format(pattern2, highs[variableIndex]);
            break;

          case PartType.Numeric:
            ExpandPreprocessRange(data, deletions, additions,
              numericLow, numericHigh, pattern2);
            break;

          case PartType.Fraction:
            ExpandFractionRange(data, deletions, additions,
              lows[variableIndex], highs[variableIndex], pattern2);
            break;
        }
      }
    }

    private static string NormalizeNumber(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return string.Empty;
      return int.Parse(input).ToString();
    }

    private void PreprocessHouseNumber(ICollection<StreetAnalysisData> dataList)
    {
      // In this we normalize the house addresses to optimize later matching.

      // Preprocessing consists of looking for cases where the primary
      // numbers are like H9:H16, where the difference in digit length
      // means they won't collate properly. These aren't supposed to
      // be in the data, but they are.
      //
      // We deal with it by splitting the entries so H9:H16 becomes two:
      // H9:H9 and H10:H16. Also 901A:1099A and 123-9:123:10.

      var deletions = new List<StreetAnalysisData>();
      var additions = new List<StreetAnalysisData>();

      foreach (var data in dataList)
        try
        {
          // Eliminate leading hyphens, which seem to be fairly rampant
          if (data.PrimaryLowNumber.StartsWith("-"))
            data.PrimaryLowNumber = data.PrimaryLowNumber.Substring(1);
          if (data.PrimaryHighNumber.StartsWith("-"))
            data.PrimaryHighNumber = data.PrimaryHighNumber.Substring(1);

          // Give an immediate pass to any with empty primary address
          if ((data.PrimaryLowNumber.Length == 0) &&
            (data.PrimaryHighNumber.Length == 0))
            continue;

          // Any items where both are length 10 numerics are ok if collated 
          // properly
          if (data.PrimaryLowNumber.IsDigits() &&
            data.PrimaryHighNumber.IsDigits())
          {
            if (data.PrimaryLowNumber.Length != AddressFinder.MaxHouseNumberLength)
              data.PrimaryLowNumber =
                long.Parse(data.PrimaryLowNumber).ToString()
                  .ZeroPad(AddressFinder.MaxHouseNumberLength);
            if (data.PrimaryHighNumber.Length != AddressFinder.MaxHouseNumberLength)
              data.PrimaryHighNumber =
                long.Parse(data.PrimaryHighNumber).ToString()
                  .ZeroPad(AddressFinder.MaxHouseNumberLength);
            if (data.PrimaryLowNumber.IsLe(data.PrimaryHighNumber))
              continue;
          }

          // Convert dots to dashes
          data.PrimaryLowNumber = data.PrimaryLowNumber.Replace('.', '-');
          data.PrimaryHighNumber = data.PrimaryHighNumber.Replace('.', '-');

          if (PreprocessPattern99Hyphen99(data, deletions, additions))
            continue;

          if (PreprocessPatternXXHyphenXX(data, deletions, additions))
            continue;

          if (PreprocessPatternXX99XX99XX(data, deletions, additions))
            continue;

          if (PreprocessFractionals(data, deletions, additions))
            continue;

          // Make sure we pass collation
          if (data.PrimaryLowNumber.IsGt(data.PrimaryHighNumber))
            throw new VoteException();

          // Any equal all alphanumerics are ok
          if ((data.PrimaryLowNumber == data.PrimaryHighNumber) &&
            data.PrimaryLowNumber.IsLettersOrDigits())
            continue;

          // If both are pure alpha, check for valid variable string
          if (data.PrimaryLowNumber.IsLetters() &&
            data.PrimaryHighNumber.IsLetters() &&
            IsValidVariableString(data.PrimaryLowNumber,
              data.PrimaryHighNumber))
            continue;

          // Throw exceptions on anything else 
          throw new VoteException();
        }
        catch (VoteException)
        {
          ReportError("{0} Invalid Primaries: {1} {2}",
            data.UpdateKey, data.PrimaryLowNumber, data.PrimaryHighNumber);
        }

      if ((deletions.Count > 0) || (additions.Count > 0))
      {
        _Statistics.Expanded += additions.Count - deletions.Count;
        foreach (var toDelete in deletions)
          dataList.Remove(toDelete);
        foreach (var toAdd in additions)
          dataList.Add(toAdd);
      }
    }

    private static bool PreprocessPattern99Hyphen99(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions)
    {
      var matchLow = Regex99Hyphen99.Match(data.PrimaryLowNumber);
      var matchHigh = Regex99Hyphen99.Match(data.PrimaryHighNumber);

      if (!matchLow.Success || !matchHigh.Success) return false; // not handled

      ProcessAddress(
        data,
        deletions,
        additions,
        matchLow,
        matchHigh,
        "{0}{1}{2}-{3}{4}{5}",
        AddressPart.CreateNumeric("prenumber1"),
        AddressPart.CreateAlpha("prealpha1"),
        AddressPart.CreateNumeric("number1"),
        AddressPart.CreateNumeric("number2"),
        AddressPart.CreateAlpha("postalpha1"),
        AddressPart.CreateNumeric("postnumber1"));

      return true;
    }

    // ReSharper disable once InconsistentNaming
    private static bool PreprocessPatternXXHyphenXX(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions)
    {
      var matchLow = RegexXXHyphenXX.Match(data.PrimaryLowNumber);
      var matchHigh = RegexXXHyphenXX.Match(data.PrimaryHighNumber);

      if (!matchLow.Success || !matchHigh.Success) return false; // not handled

      ProcessAddress(
        data,
        deletions,
        additions,
        matchLow,
        matchHigh,
        "{0}{1}-{2}{3}",
        AddressPart.CreateNumeric("prenumber1"),
        AddressPart.CreateAlpha("alpha1"),
        AddressPart.CreateAlpha("alpha2"),
        AddressPart.CreateNumeric("postnumber1"));

      return true;
    }

    // ReSharper disable once InconsistentNaming
    private static bool PreprocessPatternXX99XX99XX(StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions)
    {
      var matchLow = RegexXX99XX99XX.Match(data.PrimaryLowNumber);
      var matchHigh = RegexXX99XX99XX.Match(data.PrimaryHighNumber);

      if (!matchLow.Success || !matchHigh.Success) return false; // not handled

      ProcessAddress(
        data,
        deletions,
        additions,
        matchLow,
        matchHigh,
        "{0}{1}{2}{3}{4}{5}",
        AddressPart.CreateAlpha("alpha1"),
        AddressPart.CreateNumeric("number1"),
        AddressPart.CreateAlpha("alpha2"),
        AddressPart.CreateNumeric("number2"),
        AddressPart.CreateAlpha("alpha3"),
        AddressPart.CreateNumeric("number3"));

      return true;
    }

    private static bool PreprocessFractionals(
      StreetAnalysisData data, ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions)
    {
      var matchLow = RegexFractional.Match(data.PrimaryLowNumber);
      var matchHigh = RegexFractional.Match(data.PrimaryHighNumber);

      if (!matchLow.Success || !matchHigh.Success) return false; // not handled

      // There are two special cases here:
      //   1. If any of prefix, number or suffix1 is not empty, the pattern
      //      needs a space before the fraction.
      //   2. If the numberLow is empty but numberHigh is not (like 1/2::2 1/2)
      //      we need to create an extra entry for 1/2::1/2 before we let the
      //      normal expansion handle the rest.

      var prefixHigh = matchHigh.FirstCaptureOrEmpty("prefix");
      var numberLow = matchLow.FirstCaptureOrEmpty("number");
      var numberHigh = matchHigh.FirstCaptureOrEmpty("number");
      var suffix1High = matchHigh.FirstCaptureOrEmpty("suffix1");

      // Case 1
      string pattern;
      if ((prefixHigh.Length == 0) && (numberHigh.Length == 0) && (suffix1High.Length == 0))
        pattern = "{0}{1}{2}{3}{4}";
      else
        pattern = "{0}{1}{2} {3}{4}";

      // Case 2
      if ((numberLow.Length == 0) && (numberHigh.Length != 0))
      {
        var toAdd = data.Clone();
        toAdd.UpdateKey += "-00";
        toAdd.PrimaryHighNumber = toAdd.PrimaryLowNumber;
        additions.Add(toAdd);
      }

      ProcessAddress(
        data,
        deletions,
        additions,
        matchLow,
        matchHigh,
        pattern,
        AddressPart.CreateAlpha("prefix"),
        AddressPart.CreateNumeric("number"),
        AddressPart.CreateAlpha("suffix1"),
        AddressPart.CreateFraction("fraction"),
        AddressPart.CreateAlpha("suffix2"));

      return true;
    }

    private static void ProcessAddress(
      StreetAnalysisData data,
      ICollection<StreetAnalysisData> deletions,
      ICollection<StreetAnalysisData> additions,
      Match matchLow,
      Match matchHigh,
      string pattern,
      params AddressPart[] parts)
    {
      // Matches assumed to be successful
      var lows = new string[parts.Length];
      var highs = new string[parts.Length];
      var constants = new string[parts.Length];
      var variableIndex = -1;
      var numericLow = -1;
      var numericHigh = -1;

      GetAllAddressParts(matchLow, matchHigh, parts, lows, highs);
      // Find the variable part. May be none, but never more than one.
      // A false return indicates inconsistent data
      if (!FindVariablePart(parts, data.PrimaryOddEven, lows, highs, constants,
        ref variableIndex, ref numericLow, ref numericHigh))
        throw new VoteException();
      NormalizeAndExpandAddress(data, deletions, additions, pattern,
        // ReSharper disable once CoVariantArrayConversion
        parts, lows, highs, constants, variableIndex,
        numericLow, numericHigh);
    }

    private static IEnumerable<Range> RangePartitions(int low, int high)
    {
      while (low <= high)
      {
        var newHighDigits = Math.Floor(Math.Log10(low) + 1);
        var newHigh = (int) Math.Pow(10.0, newHighDigits) - 1;
        newHigh = Math.Min(newHigh, high);
        yield return new Range {Low = low.ToString(), High = newHigh.ToString()};
        low = newHigh + 1;
      }
    }

    #region IncrementAddress, CanMergeAddresses

    private static bool CanMergeAddresses(string low, string high, string oddEven) =>
      (high == low) || (high == IncrementAddress(low, high, oddEven));

    private static string IncrementAddress(string low, string high, string oddEven)
    {
      // Returns the next sequential address after low,
      // taking the oddEven ("B", "O" or "E") and the various variabilities
      // into account

      if (low.Length != high.Length) return null;

      // First we handle the numeric case
      var isNumericLow = AddressFinder.IsNumericHouseNumber(low);
      var isNumericHigh = AddressFinder.IsNumericHouseNumber(high);
      if (isNumericLow && isNumericHigh)
      {
        var increment = oddEven == "B" ? 1 : 2;
        return (long.Parse(low) + increment).ToString()
          .ZeroPad(AddressFinder.MaxHouseNumberLength);
      }
      if (isNumericLow || isNumericHigh) // mismatch
        return null;

      // Try each of the four alphanumeric patterns
      // This is set to true if the patterns match even if the addresses 
      // do not. We can stop looking when this turns true.
      var stop = false;

      var result = IncrementAddress99Hyphen99(low, high, oddEven,
        ref stop);

      if (!stop)
        result = IncrementAddressXXHyphenXX(low, high, oddEven,
          ref stop);

      if (!stop)
        result = IncrementAddressXX99XX99XX(low, high, oddEven,
          ref stop);

      if (!stop)
        result = IncrementAddressFractionals(low, high, oddEven,
          ref stop);

      return result;
    }

    private static string IncrementAddress99Hyphen99(string low, string high,
      string oddEven, ref bool stop)
    {
      var matchLow = Regex99Hyphen99.Match(low);
      var matchHigh = Regex99Hyphen99.Match(high);

      if (!matchLow.Success || !matchHigh.Success) return null; // not handled
      stop = true;

      return IncrementAddressCommon(
        matchLow,
        matchHigh,
        "{0}{1}{2}-{3}{4}{5}",
        oddEven,
        AddressPart.CreateNumeric("prenumber1"),
        AddressPart.CreateAlpha("prealpha1"),
        AddressPart.CreateNumeric("number1"),
        AddressPart.CreateNumeric("number2"),
        AddressPart.CreateAlpha("postalpha1"),
        AddressPart.CreateNumeric("postnumber1"));
    }

    // ReSharper disable once InconsistentNaming
    private static string IncrementAddressXXHyphenXX(string low, string high,
      string oddEven, ref bool stop)
    {
      var matchLow = RegexXXHyphenXX.Match(low);
      var matchHigh = RegexXXHyphenXX.Match(high);

      if (!matchLow.Success || !matchHigh.Success) return null; // not handled
      stop = true;

      return IncrementAddressCommon(
        matchLow,
        matchHigh,
        "{0}{1}-{2}{3}",
        oddEven,
        AddressPart.CreateNumeric("prenumber1"),
        AddressPart.CreateAlpha("alpha1"),
        AddressPart.CreateAlpha("alpha2"),
        AddressPart.CreateNumeric("postnumber1"));
    }

    // ReSharper disable once InconsistentNaming
    private static string IncrementAddressXX99XX99XX(string low, string high,
      string oddEven, ref bool stop)
    {
      var matchLow = RegexXX99XX99XX.Match(low);
      var matchHigh = RegexXX99XX99XX.Match(high);

      if (!matchLow.Success || !matchHigh.Success) return null; // not handled
      stop = true;

      return IncrementAddressCommon(
        matchLow,
        matchHigh,
        "{0}{1}{2}{3}{4}{5}",
        oddEven,
        AddressPart.CreateAlpha("alpha1"),
        AddressPart.CreateNumeric("number1"),
        AddressPart.CreateAlpha("alpha2"),
        AddressPart.CreateNumeric("number2"),
        AddressPart.CreateAlpha("alpha3"),
        AddressPart.CreateNumeric("number3"));
    }

    private static string IncrementAddressFractionals(string low, string high,
      string oddEven, ref bool stop)
    {
      var matchLow = RegexFractional.Match(low);
      var matchHigh = RegexFractional.Match(high);

      if (!matchLow.Success || !matchHigh.Success) return null; // not handled
      stop = true;

      var prefixHigh = matchHigh.FirstCaptureOrEmpty("prefix");
      var numberHigh = matchHigh.FirstCaptureOrEmpty("number");
      var suffix1High = matchHigh.FirstCaptureOrEmpty("suffix1");

      string pattern;
      if ((prefixHigh.Length == 0) && (numberHigh.Length == 0) && (suffix1High.Length == 0))
        pattern = "{0}{1}{2}{3}{4}";
      else
        pattern = "{0}{1}{2} {3}{4}";

      return IncrementAddressCommon(
        matchLow,
        matchHigh,
        pattern,
        oddEven,
        AddressPart.CreateAlpha("prefix"),
        AddressPart.CreateNumeric("number"),
        AddressPart.CreateAlpha("suffix1"),
        AddressPart.CreateFraction("fraction"),
        AddressPart.CreateAlpha("suffix2"));
    }

    private static string IncrementAddressCommon(
      Match matchLow,
      Match matchHigh,
      string pattern,
      string oddEven,
      params AddressPart[] parts)
    {
      // Matches assumed to be successful
      var lows = new string[parts.Length];
      var highs = new string[parts.Length];
      var constants = new string[parts.Length];
      var variableIndex = -1;
      var numericLow = -1;
      var numericHigh = -1;

      GetAllAddressParts(matchLow, matchHigh, parts, lows, highs);
      if (!FindVariablePart(parts, oddEven, lows, highs, constants,
        ref variableIndex, ref numericLow, ref numericHigh))
        return null;
      // If there is no variable part, we can't merge
      if (variableIndex == -1)
        return null;
      return IncrementAddressVariable(pattern, parts, oddEven, lows,
        constants, variableIndex, numericLow, numericHigh);
    }

    private static string IncrementAddressVariable(string pattern,
      IList<AddressPart> parts, string oddEven, IList<string> lows,
      string[] constants, int variableIndex, int numericLow, int numericHigh)
    {
      string result = null;
      var increment = oddEven == "B" ? 1 : 2;

      // variableIndex will always be >= 0
      // modify the pattern with constants and a single variable
      constants[variableIndex] = "{0}";
      // ReSharper disable once CoVariantArrayConversion
      var pattern2 = string.Format(pattern, constants);

      switch (parts[variableIndex].Type)
      {
        case PartType.Alpha:
        {
          // Add increment to character value of last position
          var variableLow = lows[variableIndex];
          var newLastChar =
            (char) (variableLow[variableLow.Length - 1] + increment);
          lows[variableIndex] =
            variableLow.Substring(0, variableLow.Length - 1) + newLastChar;
          var formattedLow = string.Format(pattern2, lows[variableIndex]);
          result = formattedLow;
        }
          break;

        case PartType.Numeric:
          // We can't increment if there is a length transition
          if (numericLow.Digits() == numericHigh.Digits())
          {
            lows[variableIndex] = (numericLow + increment).ToString();
            var formattedLow = string.Format(pattern2, lows[variableIndex]);
            result = formattedLow;
          }
          break;

        case PartType.Fraction:
          // cannot merge when the variable part is the fraction
          break;
      }

      return result;
    }

    #endregion IncrementAddress, CanMergeAddresses
  }
}