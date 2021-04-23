using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Vote
{
  public class Tag
  {
    #region Private

    private readonly ParsedHtml _ParsedHtml;

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public Tag(ParsedHtml parsedHtml, int index)
    {
      if (parsedHtml.Constructed)
        throw new ParsedHtmlException(
          "Tag objects can only be created by ParsedHtml constructor");
      _ParsedHtml = parsedHtml;
      Index = index;
    }

    public string GetAttribute(string attribute)
    {
      //attribute = attribute.ToLowerInvariant();
      //string value = null;
      //Match tagMatch = TagMatch;
      //Group nameGroup = tagMatch.Groups["name"];
      //Group valueGroup = tagMatch.Groups["value"];
      //int nameCount = nameGroup == null ? 0 : nameGroup.Captures.Count;
      //int valueCount = valueGroup == null ? 0 : valueGroup.Captures.Count;
      //if (nameCount != valueCount)
      //  throw new ParsedHtmlException(string.Format(CultureInfo.InvariantCulture, 
      //    "Attribute names and values mismatch: {0}", tagMatch.ToString()));
      //for (int inx = 0; inx < nameGroup.Captures.Count; inx++)
      //{
      //  string name = nameGroup.Captures[inx].Value.ToLowerInvariant();
      //  if (name == attribute)
      //  {
      //    value = HttpUtility.HtmlDecode(valueGroup.Captures[inx].Value);
      //    break;
      //  }
      //}
      //return value;
      string value = null;

      var capture = GetAttributeCapture(attribute);
      if (capture != null)
        value = HttpUtility.HtmlDecode(capture.Value);

      return value;
    }

    public Capture GetAttributeCapture(string attribute)
    {
      attribute = attribute.ToLowerInvariant();
      Capture capture = null;
      var tagMatch = TagMatch;
      var nameGroup = tagMatch.Groups["name"];
      var valueGroup = tagMatch.Groups["value"];
      var nameCount = nameGroup?.Captures.Count ?? 0;
      var valueCount = valueGroup?.Captures.Count ?? 0;
      if (nameCount != valueCount)
        throw new ParsedHtmlException(
          string.Format(
            CultureInfo.InvariantCulture, "Attribute names and values mismatch: {0}",
            tagMatch));
      Debug.Assert(nameGroup != null, "nameGroup != null");
      for (var inx = 0; inx < nameGroup.Captures.Count; inx++)
      {
        var name = nameGroup.Captures[inx].Value.ToLowerInvariant();
        if (name != attribute) continue;
        Debug.Assert(valueGroup != null, "valueGroup != null");
        capture = valueGroup.Captures[inx];
        break;
      }
      return capture;
    }

    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public string GetBodyContent()
    {
      // a modified GetInnerText that skips Hx tags and continues to end of data
      var inx1 = Index;
      var inx2 = _ParsedHtml.TagArray.Length - 1;
      var sb = new StringBuilder();
      var inx = inx1;
      while (inx < inx2)
      {
        var tag = _ParsedHtml.TagArray[inx];
        if (!tag.IsEndTag && !tag.IsPhrase && (sb.Length > 0) &&
          !char.IsWhiteSpace(sb[sb.Length - 1]))
          sb.Append(' '); // force whitespace
        if ((tag.TagName.Length == 2) && (tag.TagName[0] == 'h') &&
          char.IsDigit(tag.TagName[1])) // skip
        {
          var endTag = tag.GetEndTag();
          if (endTag != null)
            inx = endTag.Index + 1;
          else
            inx++;
          continue;
        }
        if ((tag.TagName != "script") && (tag.TagName != "style"))
          sb.Append(tag.GetInnerHtml(_ParsedHtml.TagArray[inx + 1]));
        inx++;
      }
      return HttpUtility.HtmlDecode(sb.ToString());
    }

    [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
    public Tag GetEndTag()
    {
      Tag result = null;
      if (IsEndTag)
        throw new ParsedHtmlException("EndTag called on end tag");
      if (IsSelfEnding)
        result = this;
      else
      {
        var nest = 0;
        var tagName = TagName;
        var endTagName = '/' + tagName;
        var tagInx = Index + 1;
        while (tagInx < _ParsedHtml.TagArray.Length)
        {
          var tag = _ParsedHtml.TagArray[tagInx++];
          var name = tag.TagName;
          if ((name == tagName) && !tag.IsSelfEnding)
            nest++;
          else if (name == endTagName)
            if (nest == 0)
            {
              result = tag;
              break;
            }
            else
              nest--;
        }
      }

      return result;
    }

    public string GetInnerHtml()
    {
      return GetInnerHtml(GetEndTag());
    }

    public string GetInnerHtml(Tag end)
    {
      if (end == null) return string.Empty;
      var match1 = TagMatch;
      var match2 = end.TagMatch;
      var startPos = match1.Index + match1.Length;
      return _ParsedHtml.Html.Substring(startPos, match2.Index - startPos);
    }

    public string GetInnerText()
    {
      return GetInnerText(GetEndTag());
    }

    public string GetInnerText(Tag end)
    {
      if (end == null) return string.Empty;
      var inx1 = Index;
      var inx2 = end.Index;
      var sb = new StringBuilder();
      for (var inx = inx1; inx < inx2; inx++)
      {
        var tag = _ParsedHtml.TagArray[inx];
        if (!tag.IsEndTag && !tag.IsPhrase && (sb.Length > 0) &&
          !char.IsWhiteSpace(sb[sb.Length - 1]))
          sb.Append(' '); // force whitespace
        if ((tag.TagName != "script") && (tag.TagName != "style"))
          sb.Append(tag.GetInnerHtml(_ParsedHtml.TagArray[inx + 1]));
      }
      return HttpUtility.HtmlDecode(sb.ToString());
    }

    public string GetLinkText()
    {
      string image;
      return GetLinkText(out image);
    }

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
       MessageId = "0#")]
    public string GetLinkText(out string image)
    {
      var text = string.Empty;
      image = null;

      switch (TagName)
      {
        case "a":
        {
          var end = GetEndTag();
          if (end == null) return string.Empty;
          var inx1 = Index;
          var inx2 = end.Index;
          var sb = new StringBuilder();
          for (var inx = inx1; inx < inx2; inx++)
          {
            var tag = _ParsedHtml.TagArray[inx];
            if (!tag.IsEndTag && !tag.IsPhrase && (sb.Length > 0) &&
              !char.IsWhiteSpace(sb[sb.Length - 1]))
              sb.Append(' '); // force whitespace
            if (tag.TagName == "img")
            {
              var altText = tag.GetAttribute("alt");
              if (!string.IsNullOrEmpty(altText))
              {
                sb.Append(' '); // force whitespace
                sb.Append(altText);
                sb.Append(' '); // force whitespace
              }
              image = tag.GetAttribute("src");
            }
            if ((tag.TagName != "script") && (tag.TagName != "style"))
              sb.Append(tag.GetInnerHtml(_ParsedHtml.TagArray[inx + 1]));
          }
          text = HttpUtility.HtmlDecode(sb.ToString());
        }
          break;

        case "area":
        {
          var alt = GetAttribute("alt");
          if (!string.IsNullOrEmpty(alt))
            text = alt;
        }
          break;
      }

      return text;
    }

    public string GetOuterHtml(Tag end)
    {
      if (end == null) return string.Empty;
      var match1 = TagMatch;
      var match2 = end.TagMatch;
      var endPos = match2.Index + match2.Length;
      return _ParsedHtml.Html.Substring(match1.Index, endPos - match1.Index);
    }

    public string GetScriptText()
    {
      return GetInnerHtml(_ParsedHtml.TagArray[Index + 1]);
    }

    public string GetStyleText()
    {
      return GetInnerHtml(_ParsedHtml.TagArray[Index + 1]);
    }

    public string Html => ToString();

    public int Index { get; }

    public bool IsEndTag
    {
      get
      {
        var tag = TagMatch.ToString();
        return tag[1] == '/';
      }
    }

    public bool IsPhrase
    {
      get
      {
        var tagName = TagName;
        if (tagName.StartsWith("/", StringComparison.OrdinalIgnoreCase))
          tagName = tagName.Substring(1);
        return (tagName == "b") || (tagName == "big") || (tagName == "code") ||
          (tagName == "em") || (tagName == "font") || (tagName == "i") ||
          (tagName == "small") || (tagName == "span") || (tagName == "strong") ||
          (tagName == "sub") || (tagName == "sup") || (tagName == "u");
      }
    }

    public bool IsSelfEnding
    {
      get
      {
        var tag = TagMatch.ToString();
        return tag[tag.Length - 2] == '/';
      }
    }

    public int Length => TagMatch.Length;

    public int Start => TagMatch.Index;

    private Match TagMatch => _ParsedHtml.Matches[Index];

    public string TagName
    {
      get
      {
        string tagName = null;
        var tagGroup = TagMatch.Groups["tagname"];
        if ((tagGroup != null) && (tagGroup.Captures.Count == 1))
          tagName = tagGroup.Captures[0].Value.ToLowerInvariant();
        return tagName;
      }
    }

    public override string ToString()
    {
      return TagMatch.ToString();
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}