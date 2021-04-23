using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vote
{
  internal sealed class Conditionals
  {
    private static readonly Regex ConditionalsRegex =
      new Regex(@"(?:\r?\n *)?(?:{{.+?}})(?: *\r?\n)?");

    private static readonly Regex NonAlphaRegex = new Regex(@"[^a-z]", RegexOptions.IgnoreCase);
    private readonly string _Input;
    private readonly List<Conditional> _ConditionalList = new List<Conditional>();
    private IfBlock _RootBlock;
    private int _ConditionalIndex;

    private static readonly Dictionary<string, Operator> KeywordDictionary =
      new Dictionary<string, Operator>
      {
        {"empty", Operator.Empty},
        {"notempty", Operator.NotEmpty},
        {"match", Operator.Match},
        {"notmatch", Operator.NotMatch},
        {"nomatch", Operator.NotMatch}
      };

    private enum Keyword
    {
      If,
      Then,
      Else,
      ElseIf,
      EndIf,
      Fail
    }

    private enum Operator
    {
      None,
      Empty,
      NotEmpty,
      Match,
      NotMatch,
      Gt,
      Ge,
      Lt,
      Le,
      Eq,
      Ne,
      True // for outer block
    }

    private sealed class IfBlock
    {
      public Conditional Fail;
      public Conditional If;
      public readonly List<IfBlock> IfContent = new List<IfBlock>();
      public Conditional Then;
      public readonly List<IfBlock> ThenContent = new List<IfBlock>();
      public Conditional Else;
      public readonly List<IfBlock> ElseContent = new List<IfBlock>();
      public Conditional Endif;
    }

    private sealed class Conditional
    {
      public readonly string MatchValue;
      public readonly Keyword Keyword;
      public readonly Operator Operator;
      public readonly object Comparand;
      public readonly int Index;
      public readonly int Length;

      public Conditional(Keyword keyword, int index = 0)
      {
        // used for beginning and end markers
        Keyword = keyword;
        Index = index;
        if (keyword == Keyword.If)
          Operator = Operator.True;
      }

      public Conditional(Capture match)
      {
        MatchValue = match.Value.Trim();
        Index = match.Index;
        Length = match.Length;
        var value = MatchValue.Substring(2, MatchValue.Length - 4) // strip %'s
          .Trim()
          .ToLowerInvariant();

        var matchNonAlpha = NonAlphaRegex.Match(value);
        string keyword;
        string predicate;
        if (matchNonAlpha.Success)
        {
          keyword = value.Substring(0, matchNonAlpha.Index)
            .Trim();
          predicate = value.Substring(matchNonAlpha.Index)
            .Trim();
        }
        else
        {
          keyword = value;
          predicate = string.Empty;
        }

        switch (keyword)
        {
          case "if":
            Keyword = Keyword.If;
            Operator = ParsePredicate(predicate, out Comparand);
            break;

          case "then":
            Keyword = Keyword.Then;
            break;

          case "else":
            Keyword = Keyword.Else;
            break;

          case "elseif":
            Operator = ParsePredicate(predicate, out Comparand);
            Keyword = Keyword.ElseIf;
            break;

          case "endif":
            Keyword = Keyword.EndIf;
            break;

          case "fail":
          case "exception":
            Keyword = Keyword.Fail;
            break;

          default:
            throw new VoteSubstitutionException("Unexpected conditional expression \"{0}\"",
              MatchValue);
        }
      }

      private Conditional(Conditional conditional)
      {
        MatchValue = conditional.MatchValue;
        Keyword = conditional.Keyword == Keyword.ElseIf
          ? Keyword.Else
          : Keyword.EndIf;
        Index = conditional.Index;
        Length = 0;
      }

      internal static Conditional CreateElseFromElseIf(Conditional elseIf)
      {
        return new Conditional(elseIf);
      }

      internal static Conditional CreateEndifForElseIf(Conditional endif)
      {
        return new Conditional(endif);
      }

      internal object ParseComparand(string predicate)
      {
        // Can be either a date or in integer
        predicate = predicate.Trim();
        int intValue;
        DateTime dateValue;
        if (int.TryParse(predicate, out intValue)) return intValue;
        if (DateTime.TryParse(predicate, out dateValue)) return dateValue.Date;
        throw new VoteSubstitutionException("Expected an integer or date comparand \"{0}\", {1}",
          MatchValue, predicate);
      }

      private Operator ParsePredicate(string predicate, out object comparand)
      {
        var @operator = Operator.None;
        comparand = null;
        predicate = predicate.Trim();

        if (string.IsNullOrEmpty(predicate))
          throw new VoteSubstitutionException("Missing predicate in {0} conditional \"{1}\"",
            Keyword, MatchValue);

        switch (predicate[0])
        {
          case '>':
            if ((predicate.Length > 1) && (predicate[1] == '='))
            {
              @operator = Operator.Ge;
              predicate = predicate.Substring(2);
            }
            else
            {
              @operator = Operator.Gt;
              predicate = predicate.Substring(1);
            }
            comparand = ParseComparand(predicate);
            break;

          case '<':
            if ((predicate.Length > 1) && (predicate[1] == '='))
            {
              @operator = Operator.Le;
              predicate = predicate.Substring(2);
            }
            else
            {
              @operator = Operator.Lt;
              predicate = predicate.Substring(1);
            }
            comparand = ParseComparand(predicate);
            break;

          case '=':
            if ((predicate.Length > 1) && (predicate[1] == '='))
              predicate = predicate.Substring(2);
            else
              predicate = predicate.Substring(1);
            @operator = Operator.Eq;
            comparand = ParseComparand(predicate);
            break;

          case '!':
            if ((predicate.Length > 1) && (predicate[1] == '='))
            {
              @operator = Operator.Ne;
              predicate = predicate.Substring(2);
              comparand = ParseComparand(predicate);
            }
            break;

          default:
          {
            foreach (var item in KeywordDictionary)
              if (predicate.StartsWith(item.Key, StringComparison.Ordinal))
              {
                @operator = item.Value;
                predicate = predicate.Substring(item.Key.Length);
                break;
              }
            switch (@operator)
            {
              case Operator.Empty:
              case Operator.NotEmpty:
                if (!string.IsNullOrWhiteSpace(predicate))
                  throw new VoteSubstitutionException("Unexpected comparand in conditional \"{0}\"",
                    MatchValue);
                break;

              case Operator.Match:
              case Operator.NotMatch:
                if ((predicate.Length > 0) && !char.IsWhiteSpace(predicate[0]))
                  @operator = Operator.None;
                else
                  comparand = predicate.Trim();
                break;
            }
            break;
          }
        }

        if (@operator == Operator.None)
          throw new VoteSubstitutionException("Invalid operator in conditional \"{0}\"",
            MatchValue);

        return @operator;
      }
    }

    private IfBlock BuildIfBlock()
    {
      if (_ConditionalList[_ConditionalIndex].Keyword == Keyword.Fail)
      {
        var failBlock = new IfBlock {Fail = _ConditionalList[_ConditionalIndex++]};
        ValidateIndex();
        return failBlock;
      }

      var ifBlock = new IfBlock {If = _ConditionalList[_ConditionalIndex++]};
      ValidateIndex();

      while (_ConditionalList[_ConditionalIndex].Keyword == Keyword.If)
        ifBlock.IfContent.Add(BuildIfBlock());
      ValidateIndex();

      if (_ConditionalList[_ConditionalIndex].Keyword != Keyword.Then)
        throw new VoteSubstitutionException("Expected Then, found \"{0}\"",
          _ConditionalList[_ConditionalIndex].MatchValue);

      ifBlock.Then = _ConditionalList[_ConditionalIndex++];
      ValidateIndex();

      while ((_ConditionalList[_ConditionalIndex].Keyword == Keyword.If) ||
        (_ConditionalList[_ConditionalIndex].Keyword == Keyword.Fail))
        ifBlock.ThenContent.Add(BuildIfBlock());
      ValidateIndex();

      switch (_ConditionalList[_ConditionalIndex].Keyword)
      {
        case Keyword.Else:
          ifBlock.Else = _ConditionalList[_ConditionalIndex++];
          ValidateIndex();
          while ((_ConditionalList[_ConditionalIndex].Keyword == Keyword.If) ||
            (_ConditionalList[_ConditionalIndex].Keyword == Keyword.Fail))
            ifBlock.ElseContent.Add(BuildIfBlock());
          ValidateIndex();
          break;

        case Keyword.ElseIf:
          ifBlock.Else = Conditional.CreateElseFromElseIf(_ConditionalList[_ConditionalIndex]);
          ifBlock.ElseContent.Add(BuildIfBlock());
          ValidateIndex();
          break;
      }

      if (_ConditionalList[_ConditionalIndex].Keyword != Keyword.EndIf)
        throw new VoteSubstitutionException("Expected EndIf, found \"{0}\"",
          _ConditionalList[_ConditionalIndex].MatchValue);

      ifBlock.Endif = ifBlock.If.Keyword == Keyword.ElseIf
        ? Conditional.CreateEndifForElseIf(_ConditionalList[_ConditionalIndex])
        : _ConditionalList[_ConditionalIndex++];

      return ifBlock;
    }

    private void BuildStructure()
    {
      _RootBlock = BuildIfBlock();
    }

    private Conditionals(string input, IEnumerable matches)
    {
      _Input = input;
      Parse(matches);
      BuildStructure();
    }

    private string DoEvaluate()
    {
      return EvaluateIf(_RootBlock);
    }

    public static string Evaluate(string input)
    {
      var matches = ConditionalsRegex.Matches(input);
      return matches.Count <= 0
        ? input
        : new Conditionals(input, matches).DoEvaluate();
    }

    private string EvaluateBlock(Conditional start, IEnumerable<IfBlock> content,
      Conditional end)
    {
      var sb = new StringBuilder();
      var index = start.Index + start.Length;
      foreach (var ifBlock in content)
        if (ifBlock.Fail != null)
          EvaluateFail(ifBlock.Fail);
        else
        {
          sb.Append(_Input, index, ifBlock.If.Index - index);
          sb.Append(EvaluateIf(ifBlock));
          index = ifBlock.Endif.Index + ifBlock.Endif.Length;
        }
      sb.Append(_Input, index, end.Index - index);
      return sb.ToString();
    }

    private bool EvaluateCondition(IfBlock ifBlock)
    {
      var truth = true;
      var testString = EvaluateBlock(ifBlock.If, ifBlock.IfContent, ifBlock.Then)
        .Trim();
      switch (ifBlock.If.Operator)
      {
        case Operator.Gt:
        case Operator.Ge:
        case Operator.Lt:
        case Operator.Le:
        case Operator.Eq:
        case Operator.Ne:
          var testValue = ifBlock.If.ParseComparand(testString);
          if (testValue.GetType() != ifBlock.If.Comparand.GetType())
            throw new VoteSubstitutionException("Type mismatch in comparison: \"{0}\", \"{1}\"",
              ifBlock.If.MatchValue, testString);
          int comparison;
          if (testValue is int) comparison = ((int) testValue).CompareTo(ifBlock.If.Comparand);
          else comparison = ((DateTime) testValue).CompareTo(ifBlock.If.Comparand);
          switch (ifBlock.If.Operator)
          {
            case Operator.Gt:
              truth = comparison > 0;
              break;

            case Operator.Ge:
              truth = comparison >= 0;
              break;

            case Operator.Lt:
              truth = comparison < 0;
              break;

            case Operator.Le:
              truth = comparison <= 0;
              break;

            case Operator.Eq:
              truth = comparison == 0;
              break;

            case Operator.Ne:
              truth = comparison != 0;
              break;
          }
          break;

        case Operator.Empty:
          truth = testString.Length == 0;
          break;

        case Operator.NotEmpty:
          truth = testString.Length != 0;
          break;

        case Operator.Match:
          truth = string.Compare(ifBlock.If.Comparand as string, testString,
            StringComparison.OrdinalIgnoreCase) == 0;
          break;

        case Operator.NotMatch:
          truth = string.Compare(ifBlock.If.Comparand as string, testString,
            StringComparison.OrdinalIgnoreCase) != 0;
          break;
      }
      return truth;
    }

    private static void EvaluateFail(Conditional conditional)
    {
      string msg = null;
      var isException = false;
      var match = Regex.Match(conditional.MatchValue,
        @"\{\{\s*(?<type>fail|exception)\s*(?<msg>.*?)\s*}\}", RegexOptions.IgnoreCase);
      if (match.Success)
      {
        var group = match.Groups["msg"];
        if ((group != null) && (group.Captures.Count != 0))
          msg = group.Captures[0].Value;
        group = match.Groups["type"];
        isException = group != null && group.Captures.Count != 0 && 
          group.Captures[0].Value == "exception";
      }
      string exceptionMsg;
      if (isException) exceptionMsg = msg;
      else
      {
        exceptionMsg = "Fail directive";
        if (!string.IsNullOrWhiteSpace(msg)) exceptionMsg += ": " + msg;
      }
      throw new VoteSubstitutionException(exceptionMsg);
    }

    private string EvaluateIf(IfBlock ifBlock)
    {
      if (ifBlock.Fail != null) EvaluateFail(ifBlock.Fail);
      if (EvaluateCondition(ifBlock))
        return EvaluateBlock(ifBlock.Then, ifBlock.ThenContent,
          ifBlock.Else ?? ifBlock.Endif);
      if (ifBlock.Else != null)
        return EvaluateBlock(ifBlock.Else, ifBlock.ElseContent,
          ifBlock.Endif);
      return string.Empty;
    }

    private void Parse(IEnumerable matches)
    {
      // We add a dummy If Then at the beginning and EndIf at the end
      _ConditionalList.Add(new Conditional(Keyword.If));
      _ConditionalList.Add(new Conditional(Keyword.Then));

      foreach (var match in matches.OfType<Match>())
        _ConditionalList.Add(new Conditional(match));

      _ConditionalList.Add(new Conditional(Keyword.EndIf, _Input.Length));
    }

    private void ValidateIndex()
    {
      if (_ConditionalIndex >= _ConditionalList.Count)
        throw new VoteSubstitutionException("Unmatched Ifs/Endifs");
    }
  }
}