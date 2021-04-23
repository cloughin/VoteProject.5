using System;
using System.Collections.Generic;
using System.Text;

namespace AnalyzeLogFiles
{
  public enum Method
  {
    Unknown,
    Get,
    Post
  }

  public class LogEntry
  {
    private Dictionary<string, string> Dictionary;

    public LogEntry(string[] fields, string line)
    {
      if (fields == null)
        throw new ArgumentNullException("fields");
      if (line == null)
        throw new ArgumentNullException("line");
      string[] values = line.Trim().Split(' ');
      if (fields.Length != values.Length)
        throw new ArgumentException("Number of fields does not match 'fields' Length", "line");
      CreateDictionary(fields.Length);
      for (int n = 0; n < fields.Length; n++)
        Dictionary.Add(fields[n], values[n]);
    }

    public string this[string key]
    {
      get
      {
        string result = null;
        if (Dictionary != null)
          Dictionary.TryGetValue(key, out result);
        return result;
      }
    }

    public int BytesReceived
    {
      get
      {
        int bytes;
        if (!int.TryParse(this["cs-bytes"], out bytes))
          bytes = 0;
        return bytes;
      }
    }

    public int BytesSent
    {
      get
      {
        int bytes;
        if (!int.TryParse(this["sc-bytes"], out bytes))
          bytes = 0;
        return bytes;
      }
    }

    private void CreateDictionary(int capacity)
    {
      if (Dictionary == null)
        if (capacity > 0)
          Dictionary = new Dictionary<string, string>(capacity, StringComparer.OrdinalIgnoreCase);
        else
          Dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public string HostName
    {
      get
      {
        string hostname = this["cs-host"];
        if (hostname == null || hostname == "-") return string.Empty;
        string[] s = hostname.Split(':');
        return s[0];
      }
    }

    public string IPFrom
    {
      get
      {
        string path = this["c-ip"];
        if (path == null) return string.Empty;
        return path;
      }
    }

    public static bool IsData(string line, ref string[] fieldList)
    {
      if (line.StartsWith("#Fields: ", StringComparison.OrdinalIgnoreCase))
      {
        fieldList = line.Substring(9).Trim().Split(' ');
        return false;
      }
      else
        return !line.StartsWith("#") && !line.StartsWith("\0");
   }

    public Method Method
    {
      get
      {
        switch (this["cs-method"])
        {
          case "GET":
            return Method.Get;

          case "POST":
            return Method.Post;

          default:
            return Method.Unknown;
        }
      }
    }

    public string Path
    {
      get
      {
        string path = this["cs-uri-stem"];
        if (path == null) return string.Empty;
        return path;
      }
    }

    public string Query
    {
      get
      {
        string path = this["cs-uri-query"];
        if (path == null) return string.Empty;
        return path;
      }
    }

    public int Status
    {
      get
      {
        int status;
        if (!int.TryParse(this["sc-status"], out status))
          status = -1;
        return status;
      }
    }

    public string UserDomain
    {
      get
      {
        string username = this["cs-username"];
        if (username == null || username == "-") return string.Empty;
        string[] s = username.Split('\\');
        if (s.Length == 2) return s[0];
        return string.Empty;
      }
    }

    public string UserName
    {
      get
      {
        string username = this["cs-username"];
        if (username == null || username == "-") return string.Empty;
        string[] s = username.Split('\\');
        if (s.Length == 2) return s[1];
        return s[0];
      }
    }
  }
}
