using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Vote
{
  public class QueryStringCollection : NameValueCollection
  {
    #region Private

    private static QueryStringCollection Parse(string queryString, bool urldecoded)
    {
      var qsc = new QueryStringCollection();
      if (queryString?.StartsWith("?", StringComparison.Ordinal) == true)
        queryString = queryString.Substring(1);
      if (!string.IsNullOrEmpty(queryString)) // empty
      {
        var parameters = queryString.Split('&');
        foreach (var parameter in parameters)
        {
          string key;
          string value;
          var equalPos = parameter.IndexOf('=');
          if (equalPos < 0) // null key
          {
            key = null;
            value = parameter;
          }
          else
          {
            key = parameter.Substring(0, equalPos);
            value = parameter.Substring(equalPos + 1);
          }
          if (urldecoded)
          {
            key = HttpUtility.UrlDecode(key);
            value = HttpUtility.UrlDecode(value);
          }
          Debug.Assert(key != null, "key != null");
          Debug.Assert(value != null, "value != null");
          qsc.Add(key, value);
        }
      }
      return qsc;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public string AddToPath(string path)
    {
      return Count == 0 ? path : path + "?" + ToString();
    }

    public static QueryStringCollection FromPairs(IEnumerable<string> pairs)
    {
      var list = pairs as IList<string> ?? pairs.ToArray();
      if (list.Count % 2 != 0)
        throw new ArgumentException("pairs entries not paired");
      var qsc = new QueryStringCollection();
      for (var inx = 0; inx < list.Count; inx += 2)
        if (!string.IsNullOrWhiteSpace(list[inx + 1]))
          qsc.Add(list[inx], list[inx + 1]);
      return qsc;
    }

    public static QueryStringCollection Parse(string queryString)
    {
      return Parse(queryString, true);
    }

    public override string ToString()
    {
      return ToString(true);
    }

    private string ToString(bool urlencoded)
    {
      if (Count == 0)
        return string.Empty;

      var sb = new StringBuilder();
      for (var i = 0; i < Count; i++)
      {
        var key = GetKey(i);
        if (urlencoded)
          key = Uri.EscapeUriString(key);
        //key = HttpUtility.UrlEncode(key);
        var keyWithEquals = !string.IsNullOrEmpty(key) ? key + "=" : string.Empty;
        var valueList = (ArrayList) BaseGet(i);
        var valueCount = valueList?.Count ?? 0;
        if (sb.Length > 0) // not first
          sb.Append('&');
        if (valueCount == 0)
          sb.Append(keyWithEquals);
        else
          for (var j = 0; j < valueCount; j++)
          {
            if (j > 0) // not first
              sb.Append('&');
            sb.Append(keyWithEquals);
            Debug.Assert(valueList != null, "valueList != null");
            var value = (string) valueList[j];
            if (urlencoded)
              value = Uri.EscapeUriString(value);
            //value = HttpUtility.UrlEncode(value);
            sb.Append(value);
          }
      }
      return sb.ToString();
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}