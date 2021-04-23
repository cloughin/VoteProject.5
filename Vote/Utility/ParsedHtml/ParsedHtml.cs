using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vote
{
  public class ParsedHtml
  {
    // fixes the problem: last tag has no closing >
    //Regex TagsRegex = new Regex(@"(<(?<tagname>!)--.*?-->)|<(?<tagname>/?\w+)(\s(?<attribute>(\s*(?<name>[A-Za-z0-9-]+)((\s*=\s*(?:""(?<value>.*?)""|'(?<value>.*?)'|(?<value>[^'"">\s]+)))|(\s*(?<value>)))?))+\s*|\s*)(?:((/(?!>))|[^/>])*)/?>",
    //  RegexOptions.Singleline | RegexOptions.IgnoreCase);
    private readonly Regex _TagsRegex =
      new Regex(
        @"(<(?<tagname>!)--.*?-->)|<(?<tagname>/?\w+)(\s(?<attribute>(\s*(?<name>[A-Za-z0-9-]+)((\s*=\s*(?:""(?<value>.*?)""|'(?<value>.*?)'|(?<value>[^'"">\s]+)))|(\s*(?<value>)))?))+\s*|\s*)(?:((/(?!>))|[^/>])*)/?(?:>|$)",
        RegexOptions.Singleline | RegexOptions.IgnoreCase);

    internal readonly string Html;
    internal readonly List<Match> Matches;
    internal readonly Tag[] TagArray;
    internal readonly bool Constructed;

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public ParsedHtml(string html)
    {
      Html = html;
      Matches = new List<Match>();

      var m = _TagsRegex.Match(html);
      while (m.Success)
      {
        Matches.Add(m);
        string tagName = null;
        var tagGroup = m.Groups["tagname"];
        if ((tagGroup != null) && (tagGroup.Captures.Count == 1))
          tagName = tagGroup.Captures[0].Value.ToLowerInvariant();
        if ((tagName == "script") || (tagName == "style"))
        {
          var endTag = "</" + tagName + ">";
          var endTagPos = html.IndexOf(endTag, m.Index + m.Length,
            StringComparison.OrdinalIgnoreCase);
          if (endTagPos >= 0) m = _TagsRegex.Match(html, endTagPos);
          else break;
        }
        else m = m.NextMatch();
      }

      TagArray = new Tag[Matches.Count];
      for (var index = 0; index < TagArray.Length; index++) TagArray[index] = new Tag(this, index);
      Constructed = true;
    }

    public Tag this[int index] => TagArray[index];

    public static string CompressWhiteSpace(string text)
    {
      text = text.Trim();
      var n1 = 0;
      var n2 = 0;
      while ((n2 < text.Length) && !char.IsWhiteSpace(text[n2])) n2++;
      if (n2 >= text.Length) return text;
      var result = string.Empty;
      while (true)
      {
        result += text.Substring(n1, n2 - n1);
        if (n2 >= text.Length) return result;
        result += ' ';
        n1 = n2 + 1;
        while ((n1 < text.Length) && char.IsWhiteSpace(text[n1])) n1++;
        if (n1 >= text.Length) return result;
        n2 = n1 + 1;
        while ((n2 < text.Length) && !char.IsWhiteSpace(text[n2])) n2++;
      }
    }

    public int Count => TagArray.Length;

    public Tag FindTag(string tagName)
    {
      return FindTag(null, tagName);
    }

    public Tag FindTag(Tag afterTag, string tagName)
    {
      Tag result = null;
      tagName = tagName.ToLowerInvariant();

      var tagInx = 0;
      if (afterTag != null) tagInx = afterTag.Index;
      while (tagInx < TagArray.Length)
      {
        var tag = TagArray[tagInx++];
        var name = tag.TagName;
        if (name != tagName) continue;
        result = tag;
        break;
      }

      return result;
    }

    public ReadOnlyCollection<Tag> FindTags(string tagName)
    {
      return FindTags(new[] {tagName});
    }

    public ReadOnlyCollection<Tag> FindTags(string[] tagNames)
    {
      var tags = new List<Tag>();
      foreach (var tag in TagArray)
      {
        var tagName = tag.TagName;
        if (tagNames.Any(validTagName => tagName == validTagName)) tags.Add(tag);
      }
      return tags.AsReadOnly();
    }

    public Tag FindTagWithAttribute(Tag afterTag, string tagName, string attribute,
      string value, StringComparison comparison = StringComparison.Ordinal)
    {
      Tag result = null;
      tagName = tagName.ToLowerInvariant();

      var tagInx = 0;
      if (afterTag != null) tagInx = afterTag.Index;
      while (tagInx < TagArray.Length)
      {
        var tag = TagArray[tagInx++];
        var name = tag.TagName;
        if (name == tagName)
          if (value.Equals(tag.GetAttribute(attribute), comparison))
          {
            result = tag;
            break;
          }
      }

      return result;
    }

    public Tag FindTagWithAttribute(string tagName, string attribute, string value,
      StringComparison comparison = StringComparison.Ordinal)
    {
      return FindTagWithAttribute(null, tagName, attribute, value, comparison);
    }

    public IEnumerable<Tag> Tags()
    {
      return TagArray;
    }

    public IEnumerable<Tag> Tags(Tag outerTag)
    {
      var endTag = outerTag?.GetEndTag();
      if (endTag == null) yield break;
      var inx1 = outerTag.Index + 1;
      var inx2 = endTag.Index;
      for (var inx = inx1; inx < inx2; inx++) yield return TagArray[inx];
    }

    public IEnumerable<Tag> Tags(string tagName)
    {
      return TagArray.Where(tag => tag.TagName == tagName);
    }

    public IEnumerable<Tag> Tags(string[] tagNames)
    {
      foreach (var tag in TagArray)
      {
        var tagName = tag.TagName;
        if (tagNames.Any(validTagName => tagName == validTagName)) yield return tag;
      }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}