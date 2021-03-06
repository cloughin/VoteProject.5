using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.String;

namespace Vote
{
  public static class ExtensionMethods
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static void Add<T>(this List<T> list, params T[] items)
    {
      list.AddRange(items);
    }

    public static Control AddAfter(this Control before, Control after)
    {
      var addAfter = before.Parent.Controls.IndexOf(before);
      before.Parent.Controls.AddAt(addAfter + 1, after);
      return after;
    }

    public static Control AddBefore(this Control after, Control before)
    {
      var addAt = after.Parent.Controls.IndexOf(after);
      after.Parent.Controls.AddAt(addAt, before);
      return before;
    }

    public static IFeedbackContainerControl AddCssClasses(this IFeedbackContainerControl control,
      string newClass, bool clear = false)
    {
      if (newClass == null) return control;
      var cssClass = clear ? Empty : control.CssClass;
      var classes = cssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      var newClasses = newClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      foreach (var classToAdd in
        newClasses.Where(classToAdd => !classes.Contains(classToAdd)))
      {
        if (!IsNullOrWhiteSpace(cssClass))
          cssClass += " ";
        cssClass += classToAdd;
      }
      control.CssClass = cssClass;
      return control;
    }

    public static WebControl AddCssClasses(this WebControl control,
      string newClass, bool clear = false)
    {
      if (newClass == null) return control;
      var cssClass = clear ? Empty : control.CssClass;
      var classes = cssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      var newClasses = newClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      foreach (var classToAdd in
        newClasses.Where(classToAdd => !classes.Contains(classToAdd)))
      {
        if (!IsNullOrWhiteSpace(cssClass))
          cssClass += " ";
        cssClass += classToAdd;
      }
      control.CssClass = cssClass;
      return control;
    }

    public static HtmlControl AddCssClasses(this HtmlControl control,
      string newClass, bool clear = false)
    {
      if (newClass == null) return control;
      var cssClass = clear ? Empty : control.Attributes["class"].SafeString();
      var classes = cssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      var newClasses = newClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      foreach (var classToAdd in
        newClasses.Where(classToAdd => !classes.Contains(classToAdd)))
      {
        if (!IsNullOrWhiteSpace(cssClass))
          cssClass += " ";
        cssClass += classToAdd;
      }
      control.Attributes["class"] = cssClass;
      return control;
    }

    public static HtmlTable AddCssClasses(this HtmlTable control,
      string newClass, bool clear = false)
    {
      return AddCssClasses(control as HtmlControl, newClass, clear) as HtmlTable;
    }

    public static HtmlTableCell AddCssClasses(this HtmlTableCell control,
      string newClass, bool clear = false)
    {
      return AddCssClasses(control as HtmlControl, newClass, clear) as HtmlTableCell;
    }

    public static HtmlTableRow AddCssClasses(this HtmlTableRow control,
      string newClass, bool clear = false)
    {
      return AddCssClasses(control as HtmlControl, newClass, clear) as HtmlTableRow;
    }

    public static Control AddCssClasses(this Control control, string newClass, bool clear = false)
    {
      if (control is HtmlControl htmlControl)
      {
        htmlControl.AddCssClasses(newClass, clear);
        return control;
      }
      if (control is WebControl webControl)
      {
        webControl.AddCssClasses(newClass, clear);
        return control;
      }
      if (control is IFeedbackContainerControl feedbackControl)
      {
        feedbackControl.AddCssClasses(newClass, clear);
        return control;
      }
      if (control is PlaceHolder) return control;

      throw new Exception("Unsupported control type");
    }

    public static ListItem AddItem(this HtmlSelect htmlSelect, string text,
      bool selected = false)
    {
      return htmlSelect.AddItem(text, text, selected);
    }

    public static ListItem AddItem(this HtmlSelect htmlSelect, string text,
      string value, bool selected = false)
    {
      if (value == null) value = text;
      var li = new ListItem(text, value);
      if (selected)
        li.Selected = true;
      htmlSelect.Items.Add(li);
      return li;
    }

    public static ListItem AddItem(this DropDownList dropDownList, string text,
      bool selected)
    {
      return dropDownList.AddItem(text, text, selected);
    }

    public static ListItem AddItem(this DropDownList dropDownList, string text,
      string value, bool selected = false)
    {
      if (value == null) value = text;
      var li = new ListItem(text, value);
      if (selected)
        li.Selected = true;
      dropDownList.Items.Add(li);
      return li;
    }

    public static Control AddTo(this Control control, Control parent,
      string classes = null, bool clear = false)
    {
      parent.Controls.Add(control);
      if (classes != null) control.AddCssClasses(classes, clear);
      return control;
    }

    public static HtmlSelect AddTo(this HtmlSelect control, Control parent,
      string classes = null, bool clear = false)
    {
      parent.Controls.Add(control);
      if (classes != null) control.AddCssClasses(classes, clear);
      return control;
    }

    public static HtmlControl AddTo(this HtmlControl control, Control parent,
      string classes = null, bool clear = false)
    {
      parent.Controls.Add(control);
      if (classes != null) control.AddCssClasses(classes, clear);
      return control;
    }

    public static HtmlTable AddTo(this HtmlTable table, Control parent,
      string classes = null, bool clear = false)
    {
      parent.Controls.Add(table);
      if (classes != null) table.AddCssClasses(classes, clear);
      return table;
    }

    public static HtmlTableRow AddTo(this HtmlTableRow row, HtmlTable table,
      string classes = null, bool clear = false)
    {
      table.Rows.Add(row);
      if (classes != null) row.AddCssClasses(classes, clear);
      return row;
    }

    public static HtmlTableCell AddTo(this HtmlTableCell cell, HtmlTableRow row,
      string classes = null, bool clear = false)
    {
      row.Cells.Add(cell);
      if (classes != null) cell.AddCssClasses(classes, clear);
      return cell;
    }

    public static DateTime AsUtc(this DateTime dateTime)
    {
      return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public static DateTime? AsUtc(this DateTime? dateTime)
    {
      return dateTime?.AsUtc();
    }

    public static HtmlControl Attribute(this HtmlControl control, string attribute,
      string value)
    {
      control.Attributes[attribute] = value;
      return control;
    }

    public static string BlankIfZero(this int value)
    {
      return value == 0 ? Empty : value.ToString();
    }

    public static int Digits(this int value)
    {
      if (value == 0) return 1;
      if (value < 0) value = -value;
      return (int) Math.Floor(Math.Log10(value) + 1);
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      var seenKeys = new HashSet<TKey>();
      return source.Where(element => seenKeys.Add(keySelector(element)));
    }

    public static IEnumerable<TKey> DistinctKeys<TSource, TKey>(
      this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      var seenKeys = new HashSet<TKey>();
      return source.Where(element => seenKeys.Add(keySelector(element)))
        .Select(keySelector);
    }

    public static int CompareAlphanumeric(this string self, string other)
    {
      if (self == null || other == null)
        return 0;

      var len1 = self.Length;
      var len2 = other.Length;
      var marker1 = 0;
      var marker2 = 0;

      // Walk through two the strings with two markers.
      while (marker1 < len1 && marker2 < len2)
      {
        var ch1 = self[marker1];
        var ch2 = other[marker2];

        // Some buffers we can build up characters in for each chunk.
        var space1 = new char[len1];
        var loc1 = 0;
        var space2 = new char[len2];
        var loc2 = 0;

        // Walk through all following characters that are digits or
        // characters in BOTH strings starting at the appropriate marker.
        // Collect char arrays.
        do
        {
          space1[loc1++] = ch1;
          marker1++;

          if (marker1 < len1)
            ch1 = self[marker1];
          else
            break;
        } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

        do
        {
          space2[loc2++] = ch2;
          marker2++;

          if (marker2 < len2)
            ch2 = other[marker2];
          else
            break;
        } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

        // If we have collected numbers, compare them numerically.
        // Otherwise, if we have strings, compare them alphabetically.
        var str1 = new string(space1);
        var str2 = new string(space2);

        int result;

        if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
        {
          var thisNumericChunk = int.Parse(str1);
          var thatNumericChunk = int.Parse(str2);
          result = thisNumericChunk.CompareTo(thatNumericChunk);
        }
        else
          result = Compare(str1, str2, StringComparison.Ordinal);

        if (result != 0)
          return result;
      }
      return len1 - len2;
    }

    //public static string DoubleMetaphone(this string input)
    //{
    //  return Vote.DoubleMetaphone.EncodePhrase(input);
    //}

    public static string FirstCapture(this Match match, string name)
    {
      return match.Groups[name].Captures[0].Value;
    }

    public static string FirstCaptureOrEmpty(this Match match, string name)
    {
      var captures = match.Groups[name].Captures;
      return captures.Count == 0 ? Empty : captures[0].Value;
    }

    public static string FormatRunningTime(this TimeSpan? time)
    {
      if (time == null) return Empty;
      return time.Value.FormatRunningTime();
    }

    public static string FormatRunningTime(this TimeSpan time)
    {
      if (time == default) return Empty;
      return time.ToString(time.TotalHours >= 1
        ? @"h\:mm\:ss"
        : @"m\:ss");
    }

    public static int GetMd5HashString(this string input, bool abs = false)
    {
      return Utility.Md5HashString(input, abs);
    }

    public static string GetValue(this Control control)
    {
      return Utility.GetControlValue(control);
    }

    public static bool HasClass(this HtmlControl control, string className)
    {
      if (control == null) return false;
      var cssClass = control.Attributes["class"].SafeString();
      return cssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries).Contains(className);
    }

    public static bool HasClass(this WebControl control, string className)
    {
      if (control == null) return false;
      return control.CssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries).Contains(className);
    }

    public static bool HasClass(this Control control, string className)
    {
      if (control is HtmlControl containerControl)
        return containerControl.HasClass(className);
      if (control is WebControl webControl)
        return webControl.HasClass(className);
      throw new Exception("Unsupported control type");
    }

    public static bool IsAscii(this string input)
    {
      return input == input.ToAscii();
    }

    private static readonly Regex IsAllUpperCaseHasUpper = new Regex("[A-Z]");

    public static bool IsAllUpperCase(this string self)
    {
      if (!IsAllUpperCaseHasUpper.IsMatch(self)) return false;
      return self.ToUpperInvariant() == self;
    }

    public static bool IsDefaultDate(this DateTime date)
    {
      return date == VotePage.DefaultDbDate || date == default;
    }

    public static bool IsDefaultDate(this DateTime? date)
    {
      return date == null || date.Value == VotePage.DefaultDbDate || date.Value == default;
    }

    // matches any non-digit
    private static readonly Regex IsDigitsRegex = new Regex(@"[^0-9]");

    public static bool IsDigits(this string input)
    {
      if (IsNullOrEmpty(input)) return false;
      return !IsDigitsRegex.Match(input)
        .Success;
    }

    public static bool IsEqIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsGe(this string self, string other)
    {
      return CompareOrdinal(self, other) >= 0;
    }

    public static bool IsGeIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static bool IsGt(this string self, string other)
    {
      return CompareOrdinal(self, other) > 0;
    }

    public static bool IsGtIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) > 0;
    }

    // matches any non-letter
    private static readonly Regex IsLettersRegex = new Regex(@"\P{L}");

    public static bool IsLetters(this string input)
    {
      if (IsNullOrEmpty(input)) return false;
      return !IsLettersRegex.Match(input)
        .Success;
    }

    // matches any non-letter or non-digit
    private static readonly Regex IsLettersOrDigitsRegex = new Regex(@"[^\p{L}0-9]");

    public static bool IsLettersOrDigits(this string input)
    {
      if (IsNullOrEmpty(input)) return false;
      return !IsLettersOrDigitsRegex.Match(input)
        .Success;
    }

    // matches any non-lower
    private static readonly Regex IsLowerRegex = new Regex(@"\P{Ll}");

    public static bool IsLower(this string input)
    {
      return !IsLowerRegex.Match(input)
        .Success;
    }

    public static bool IsLe(this string self, string other)
    {
      return CompareOrdinal(self, other) <= 0;
    }

    public static bool IsLeIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) <= 0;
    }

    public static bool IsLt(this string self, string other)
    {
      return CompareOrdinal(self, other) < 0;
    }

    public static bool IsLtIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) < 0;
    }

    public static bool IsMixedCase(this string self)
    {
      return Str.IsMixedCase(self);
    }

    public static bool IsNeIgnoreCase(this string self, string other)
    {
      return Compare(self, other, StringComparison.OrdinalIgnoreCase) != 0;
    }

    // matches any non-lower
    private static readonly Regex IsUpperRegex = new Regex(@"\P{Lu}");

    public static bool IsUpper(this string input)
    {
      return !IsUpperRegex.Match(input)
        .Success;
    }

    public static bool IsValidDate(this string str)
    {
      return Validation.IsValidDate(str);
    }

    public static bool IsValidInteger(this string str)
    {
      return Validation.IsValidInteger(str);
    }

    public static string JavascriptEscapeString(this string str)
    {
      return str.Replace("'", "\\'").Replace("\"", "\\\"");
    }

    public static string JoinAsList(this IEnumerable<string> strings)
    {
      var list = strings as IList<string> ?? strings.ToList();
      switch (list.Count)
      {
        case 0:
          return Empty;

        default:
          return "<ul><li>" + Join("</li><li>", list) + "</li></ul>";
      }
    }

    public static string JoinAsParagraphs(this IEnumerable<string> strings)
    {
      var list = strings as IList<string> ?? strings.ToList();
      switch (list.Count)
      {
        case 0:
          return Empty;

        default:
          return "<p>" + Join("</p><p>", list) + "</p>";
      }
    }

    public static string JoinText(this IEnumerable<string> strings)
    {
      var list = strings as IList<string> ?? strings.ToList();
      switch (list.Count)
      {
        case 0:
          return Empty;

        case 1:
          return list[0];

        default:
          return Join(", ", list.Take(list.Count - 1)) + " and " + list.Last();
      }
    }

    public static int LevenshteinDistance(this string self, string other)
    {
      var n = self.Length; //length of s
      var m = other.Length; //length of t
      var d = new int[n + 1, m + 1]; // matrix

      // Step 1
      if (n == 0) return m;
      if (m == 0) return n;

      // Step 2
      for (var i = 0; i <= n; d[i, 0] = i++)
      {
      }
      for (var j = 0; j <= m; d[0, j] = j++)
      {
      }

      // Step 3
      for (var i = 1; i <= n; i++)
        //Step 4
        for (var j = 1; j <= m; j++)
        {
          // Step 5
          var cost = other.Substring(j - 1, 1) == self.Substring(i - 1, 1) ? 0 : 1;
          // cost
          // Step 6
          d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
            d[i - 1, j - 1] + cost);
        }

      // Step 7
      return d[n, m];
    }

    //private static readonly Regex LinkifyRegex = new Regex(@"(?:(http(?:s)?\:\/\/)?\S+(?:(?:\.)\S+)+(?:\/S+)*)", RegexOptions.IgnoreCase);
    // better regex from https://github.com/component/regexps/blob/master/index.js#L3 (modified to only match 2 char tlds if not followed by [a-z]
    private static readonly Regex LinkifyRegex2 =
      new Regex(
        @"(?:(?:ht|f)tp(?:s?)\:\/\/|~\/|\/)?(?:\w+:\w+@)?((?:(?:[-\w\d{1-3}]+\.)+(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|edu|co\.uk|ac\.uk|it|fr|tv|museum|asia|local|travel|(?:[a-z]{2}(?![a-z]))))|((\b25[0-5]\b|\b[2][0-4][0-9]\b|\b[0-1]?[0-9]?[0-9]\b)(\.(\b25[0-5]\b|\b[2][0-4][0-9]\b|\b[0-1]?[0-9]?[0-9]\b)){3}))(?::[\d]{1,5})?(?:(?:(?:\/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|\/)+|\?|#)?(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=?(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=?(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?:#(?:[-\w~!$ |\/.,*:;=]|%[a-f\d]{2})*)?",
        RegexOptions.IgnoreCase);

    public static string Linkify(this string self, string target = "_blank", bool nofollow = true)
    {
      return LinkifyRegex2.Replace(self, m =>
        $"<a href=\"{VotePage.NormalizeUrl(m.ToString())}" +
        $"\"{(IsNullOrWhiteSpace(target) ? Empty : $" target=\"{target}\"")}" +
        $"{(nofollow ? " rel=\"nofollow\"" : Empty)}>{m.ToString()}</a>");
    }

    [Flags]
    public enum StringNormalization
    {
      None = 0,
      Lowercase = 1,
      StripDiacriticals = 2,
      StripVowels = 4,
      MarkLeadingVowel = 8,
      StripNonLetters = 16,
      StripDoubleLetters = 32
    }

    private static readonly Regex NormalizeNewLinesRegex = new Regex(@"\r\n|\r");

    public static string NormalizeNewLines(this string input)
    {
      return input == null
        ? Empty
        : NormalizeNewLinesRegex.Replace(input, "\n");
    }

    public static string NormalizePhoneNumber(this string input)
    {
      var digits = Regex.Replace(input, @"\D", Empty);
      if (digits.Length == 11 && digits[0] == '1')
        digits = digits.Substring(1);
      if (digits.Length != 10) return input.Trim();
      return "(" + digits.Substring(0, 3) + ") " + digits.Substring(3, 3) + "-" +
        digits.Substring(6);
    }

    #region for ParseName

    private static readonly string[] Prefixes =
    {
      "dr", "miss", "mr", "mrs", "ms", "rev"
    };

    private static readonly string[] LastNameBeginners =
    {
      "da", "de", "del", "dela", "della", "den", "der", "des", "di", "du",
      "el", "la", "le", "los", "mac", "mc", "san", "santa", "st", "van",
      "vander", "ver", "von"
    };

    private static readonly string[] Suffixes =
    {
      "jr", "sr", "ii", "iii", "iv", "md", "phd"
    };

    private static string FirstPart(IList<string> parts, bool remove = false)
    {
      var part = parts.Count == 0 ? "" : parts[0].Replace(",", Empty);
      if (remove && parts.Count > 0) parts.RemoveAt(0);
      return part;
    }

    private static string LastPart(IList<string> parts, bool remove = false)
    {
      var part = parts.Count == 0 ? "" : parts[parts.Count - 1].Replace(",", Empty);
      if (remove && parts.Count > 0) parts.RemoveAt(parts.Count - 1);
      return part;
    }

    private static string NormalizedFirstPart(IList<string> parts)
    {
      return Regex.Replace(FirstPart(parts).ToLower(), "[^a-z]", "");
    }

    private static string NormalizedLastPart(IList<string> parts)
    {
      return Regex.Replace(LastPart(parts).ToLower(), "[^a-z]", "");
    }

    #endregion

    public static ParsedName ParseName(this string name)
    {
      var result = new ParsedName();
      var parts = name.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
        .ToList();
      result.Name = Join(" ", parts);

      // look for suffix
      if (Suffixes.Contains(NormalizedLastPart(parts)))
        result.Suffix = LastPart(parts, true);

      // look for prefix
      if (Prefixes.Contains(NormalizedFirstPart(parts)))
        result.Prefix = FirstPart(parts, true);

      // special case: if only one name left and not
      // prefixed or suffixed, assume first
      if (parts.Count == 1 && result.Suffix.Length == 0 && result.Prefix.Length == 0)
        result.First = LastPart(parts, true);
      else
      {
        // continue parse ... current last part is last name
        result.Last = LastPart(parts, true);

        // prefix with any lastNameBeginners
        var min = result.Prefix.Length > 0 ? 0 : 1;
        while (parts.Count > min &&
          LastNameBeginners.Contains(NormalizedLastPart(parts)))
          result.Last = LastPart(parts, true) + " " + result.Last;

        // use firstPart as firstName
        result.First = FirstPart(parts, true);

        // everything else is middle
        result.Middle = Join(" ", parts);
      }

      return result;
    }

    public static string Plural(this int value, string singular = "", string plural = "s")
    {
      return value == 1 ? singular : plural;
    }

    public static HtmlControl RemoveCssClass(this HtmlControl control,
      string classToRemove)
    {
      var cssClass = control.Attributes["class"];
      if (cssClass == null) return control;
      var classes = cssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      control.Attributes["class"] = Join(" ",
        classes.Where(c => c != classToRemove));
      return control;
    }

    public static WebControl RemoveCssClass(this WebControl control,
      string classToRemove)
    {
      var classes = control.CssClass.Split(new[] {' '},
        StringSplitOptions.RemoveEmptyEntries);
      control.CssClass = Join(" ",
        classes.Where(cssClass => cssClass != classToRemove));
      return control;
    }

    public static Control RemoveCssClass(this Control control, string classToRemove)
    {
      if (control is HtmlControl containerControl)
      {
        containerControl.RemoveCssClass(classToRemove);
        return control;
      }
      if (control is WebControl webControl)
      {
        webControl.RemoveCssClass(classToRemove);
        return control;
      }
      throw new Exception("Unsupported control type");
    }

    public static string RemoveHttp(this string url)
    {
      return Regex.Replace(url, "^https?://", "", RegexOptions.IgnoreCase); 
    }

    public static string RemoveMailTo(this string email)
    {
      var loc = email.IndexOf(@"mailto:", 0, email.Length, StringComparison.OrdinalIgnoreCase);
      if (loc != -1) email = email.Remove(loc, 7).Trim();
      return email;
    }

    public static string RenderToString(this Control control)
    {
      var stringWriter = new StringWriter();
      var htmlWriter = new HtmlTextWriter(stringWriter);
      control.RenderControl(htmlWriter);
      return stringWriter.ToString();
    }

    //public static List<T> ReorderVertically<T>(this IEnumerable<T> list, int columns)
    //{
    //  return list.ToList()
    //    .ReorderVertically(columns);
    //}

    public static List<T> ReorderVertically<T>(this IList<T> list, int columns)
    {
      var newList = new List<T>(list.Count);
      var indexToTake = 0;
      var rows = (list.Count + columns - 1) / columns;
      var fullColumns = columns - (rows * columns - list.Count);
      var row = 0;
      var column = 0;
// ReSharper disable UnusedVariable
      foreach (var t in list)
// ReSharper restore UnusedVariable
      {
        newList.Add(list[indexToTake]);
        indexToTake += rows;
        if (column++ >= fullColumns)
          indexToTake--;
        if (indexToTake < list.Count) continue;
        indexToTake = ++row;
        column = 0;
      }
      return newList;
    }

    private static readonly Regex ReplaceBreakTagsRegex = new Regex(@"<br\s*/?>",
      RegexOptions.IgnoreCase);

    public static string ReplaceBreakTags(this string input, string replacement)
    {
      return ReplaceBreakTagsRegex.Replace(input, replacement);
    }

    public static string ReplaceBreakTagsWithNewLines(this string input)
    {
      return ReplaceBreakTags(input, "\n");
    }

    public static string ReplaceBreakTagsWithSpaces(this string input)
    {
      return ReplaceBreakTags(input, " ");
    }

    private static readonly Regex ReplaceNewLinesRegex = new Regex(@"\r\n|\r|\n");

    public static string ReplaceNewLines(this string input, string replacement)
    {
      return input == null
        ? Empty
        : ReplaceNewLinesRegex.Replace(input, replacement);
    }

    public static string ReplaceNewLinesWithBreakTags(this string input)
    {
      return ReplaceNewLines(input, "<br />");
    }

    private static readonly Regex ReplaceNewLinesWithParagraphsRegex =
      new Regex(@"[\r\n]+");

    public static string ReplaceNewLinesWithParagraphs(this string input, bool includeOuter = true)
    {
      if (IsNullOrWhiteSpace(input)) return Empty;
      var result = ReplaceNewLinesWithParagraphsRegex.Replace(input, "</p><p>");
      if (includeOuter) result = "<p>" + result + "</p>";
      return result;
    }

    public static string ReplaceNewLinesWithSpans(this string input)
    {
      if (IsNullOrWhiteSpace(input)) return Empty;
      return "<span>" + ReplaceNewLinesWithParagraphsRegex.Replace(input, "</span><span>") +
        "</span>";
    }

    public static string ReplaceNewLinesWithEmptyString(this string input)
    {
      return ReplaceNewLines(input, Empty);
    }

    public static string ReplaceNewLinesWithSpaces(this string input)
    {
      return ReplaceNewLines(input, " ");
    }

    public static string SafeString(this string self)
    {
      return self ?? Empty;
    }

    public static string SafeSubstring(this string input, int start,
      int length = -1)
    {
      if (start < 0)
      {
        if (length >= 0) length = Math.Max(0, length + start);
        start = 0;
      }
      if (input == null || start >= input.Length) return Empty;
      if (length < 0) return input.Substring(start);
      length = Math.Min(length, input.Length - start);
      return length <= 0 ? Empty : input.Substring(start, length);
    }

    public static HtmlControl SetToolTip(this HtmlControl control, string tooltip)
    {
      control.Attributes["title"] = tooltip;
      return control;
    }

    public static WebControl SetToolTip(this WebControl control, string tooltip)
    {
      control.ToolTip = tooltip;
      return control;
    }

    public static Control SetToolTip(this Control control, string tooltip)
    {
      if (control is HtmlControl containerControl)
      {
        containerControl.SetToolTip(tooltip);
        return control;
      }
      if (control is WebControl webControl)
      {
        webControl.SetToolTip(tooltip);
        return control;
      }
      throw new Exception("Unsupported control type");
    }

    public static Control SetValue(this Control control, string value)
    {
      Utility.SetControlValue(control, value);
      return control;
    }

    private static readonly Regex SimpleRecaseRegex = new Regex("(?<=^|[^a-z0-9])[a-z]");
    public static string SimpleRecase(this string str)
    {
      return SimpleRecaseRegex.Replace(str.ToLower(), m => m.Value.ToUpper());
    }

    public static string SplitLinesWithBreakTags(this string input, int maxChars)
    {
      return input.SplitLines(maxChars, "<br />");
    }

    public static string SplitLines(this string input, int maxChars,
      string lineBreak)
    {
      var words = input.SplitOnWhiteSpace();
      var sb = new StringBuilder();
      var nextWord = 0;
      while (nextWord < words.Length)
      {
        if (sb.Length > 0) sb.Append(lineBreak);
        // always add one word
        var currentLineLength = words[nextWord].Length;
        sb.Append(words[nextWord++]);
        while (nextWord < words.Length &&
          currentLineLength + words[nextWord].Length < maxChars)
        {
          currentLineLength += words[nextWord].Length + 1;
          sb.Append(' ');
          sb.Append(words[nextWord++]);
        }
      }
      return sb.ToString();
    }

    private static readonly Regex SplitOnBreakTagsRegex =
      new Regex(@"^\s*(?:(?<line>.*?)\s*<br\s*/?>\s*)*(?<line>.*?)\s*$",
        RegexOptions.IgnoreCase);

    public static string[] SplitOnBreakTags(this string input)
    {
      return SplitOnBreakTagsRegex.Match(input)
        .Groups["line"].Captures.OfType<Capture>()
        .Select(c => c.Value)
        .ToArray();
    }

    private static readonly Regex SplitOnWhiteSpaceRegex =
      new Regex(@"^(?:\s*(?<word>\S+))*\s*$", RegexOptions.IgnoreCase);

    public static string[] SplitOnWhiteSpace(this string input)
    {
      return SplitOnWhiteSpaceRegex.Match(input)
        .Groups["word"].Captures.OfType<Capture>()
        .Select(c => c.Value)
        .ToArray();
    }

    private static readonly Regex SqlEscapeLikeRegex = new Regex(@"[_%\\]");

    public static string SqlEscapeLike(this string input)
    {
      return SqlEscapeLikeRegex.Replace(input, match => @"\" + match.Value);
    }

    public static string SqlEscapeString(this string str)
    {
      return str.Replace("'", "''");
    }

    public static string SqlIn(this IEnumerable<string> strings, string column)
    {
      return " " + column +
        " IN ('" + Join("','", strings.Select(s => s.SqlEscapeString())) + "') ";
    }

    public static string SqlIn(this IEnumerable<int> ints, string column)
    {
      return " " + column + " IN (" + Join(",", ints) + ") ";
    }

    public static string SqlLit(this string str)
    {
      return "'" + str.SqlEscapeString() + "'";
    }

    public static string StripAccents(this string input, bool retainCasing = false,
      bool retainDigits = false)
    {
      var normalizedString = input.Normalize(NormalizationForm.FormD);
      var sb = new StringBuilder(normalizedString.Length);
      foreach (var c in normalizedString)
        switch (CharUnicodeInfo.GetUnicodeCategory(c))
        {
          case UnicodeCategory.LowercaseLetter:
          case UnicodeCategory.UppercaseLetter:
            sb.Append(c);
            break;

          case UnicodeCategory.DecimalDigitNumber:
            if (retainDigits)
              sb.Append(c);
            break;
        }
      var result = sb.ToString();
      if (!retainCasing) result = result.ToLowerInvariant();
      return result;
    }

    public static string StripHtml(this string html, string replaceWith = "")
    {
      html = Regex.Replace(html, @"<(?![\s>])[^>]+>", replaceWith);
      return html;
    }

    private static readonly Regex StripNonAlphaNumericRegex = new Regex("[^a-z0-9]", RegexOptions.IgnoreCase);
    public static string StripNonAlphaNumeric(this string str)
    {
      return StripNonAlphaNumericRegex.Replace(str, Empty);
    }

    public static string StripRedundantWhiteSpace(this string str)
    {
      return str == null
        ? null
        : Regex.Replace(str, @"\s+", " ")
          .Trim();
    }

    public static string StripRedundantSpaces(this string str)
    {
      if (str == null) return null;
      return Join("\n", str.Split('\n')
        .Select(line => Regex.Replace(line, @" +", " ")
          .Trim()));
    }

    public static string StripVowels(this string input)
    {
      // a leading vowel is retained
      var deaccented = input.StripAccents();
      var sb = new StringBuilder(deaccented.Length);
      var last = ' ';
      foreach (var c in deaccented)
      {
        if (c != last && "aeiouy".IndexOf(c) < 0 || sb.Length == 0)
          sb.Append(c);
        last = c;
      }
      return sb.ToString();
    }

    public static string Substitute(this string str, params string[] parms)
    {
      return new Substitutions(parms).Substitute(str);
    }

    public static string Substitute(this string str, Substitutions s)
    {
      return s.Substitute(str);
    }

    public static string ToAscii(this string input)
    {
      return Encoding.ASCII.GetString(Encoding.GetEncoding(1251)
        .GetBytes(input));
    }

    public static int ToInt(this Enum value)
    {
      return Convert.ToInt32(value);
    }

    public static ulong ToJavascriptMilliseconds(this DateTime time)
    {
      return Convert.ToUInt64(time.Subtract(new DateTime(1969, 12, 31)).TotalMilliseconds);
    }

    public static string ToOptionalDate(this DateTime date,
      string format = "MMMM d, yyyy")
    {
      if (date.IsDefaultDate())
        return Empty;
      return date.ToString(format);
    }

    public static string ToOptionalDate(this DateTime? date,
      string format = "MMMM d, yyyy")
    {
      return date == null ? Empty : date.Value.ToOptionalDate(format);
    }

    public static string ToSentenceCase(this string self)
    {
      return Vote.ToSentenceCase.Do(self);
    }

    public static string ToStringOrNull(this object input)
    {
      return input?.ToString() ?? "<null>";
    }

    public static string ToTitleCase(this string self)
    {
      return Vote.ToTitleCase.Do(self);
    }

    public static Uri ToUri(this string input, bool https = false)
    {
      if (IsNullOrWhiteSpace(input)) return null;
      Uri result = null;
      if (!input.Contains("://"))
        input = (https ? Uri.UriSchemeHttps : Uri.UriSchemeHttp) + "://" + input;
      try
      {
        result = new Uri(input);
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch
      {
      }

      return result;
    }

    public static string Truncate(this DataColumn column, string value)
    {
      if (value == null || column.MaxLength <= 0 || column.MaxLength > value.Length)
        return value;
      return value.Substring(0, column.MaxLength);
    }

    public static string TruncateWithEllipsis(this string input, int maxLength)
    {
      if (input.Length > maxLength)
        input = input.Substring(0, maxLength - 3) + "...";
      return input;
    }

    public static string ZeroPad(this string input, int length)
    {
      return input.Trim()
        .PadLeft(length, '0');
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public class ParsedName
  {
    // ReSharper disable once NotAccessedField.Global
    public string Name = Empty;
    public string Prefix = Empty;
    public string First = Empty;
    // ReSharper disable once NotAccessedField.Global
    public string Middle = Empty;
    public string Last = Empty;
    public string Suffix = Empty;
  }
}