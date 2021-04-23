using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CopyIISLogFiles
{
  class IISLogFileReader : IDisposable
  {
    static Regex FieldsRegex = new Regex(@"#Fields:(\s(?<field>\S+))+");
    static string[] EmptyStringArray = new string[0];

    string FileName;
    StreamReader StreamReader;
    List<string> _Comments;
    List<string> _Fields;
    bool _HasNewFieldList;
    string[] _Values;

    public IISLogFileReader(string fileName)
    {
      FileName = fileName;
      StreamReader = new StreamReader(FileName);
    }

    public void Close()
    {
      StreamReader.Close();
    }

    public int CommentCount
    {
      get
      {
        if (_Comments == null) return 0;
        else return _Comments.Count;
      }
    }

    public string[] Comments
    {
      get
      {
        if (_Comments == null)
          return EmptyStringArray;
        else
          return _Comments.ToArray();
      }
    }

    public int FieldCount
    {
      get
      {
        if (_Fields == null) return 0;
        else return _Fields.Count;
      }
    }

    public string[] Fields
    {
      get
      {
        if (_Fields == null)
          return EmptyStringArray;
        else
          return _Fields.ToArray();
      }
    }

    public int ValueCount
    {
      get
      {
        if (_Values == null) return 0;
        else return _Values.Length;
      }
    }

    public string[] Values
    {
      get
      {
        if (_Values == null)
          return EmptyStringArray;
        else
          return _Values;
      }
    }

    public string GetName(int ordinal)
    {
      if (_Fields == null || ordinal < 0 || ordinal >= _Fields.Count)
        throw new IndexOutOfRangeException("ordinal");
      return _Fields[ordinal];
    }

    public int GetOrdinal(string name)
    {
      if (_Fields == null)
        return -1;
      return _Fields.IndexOf(name);
    }

    public string GetString(int ordinal)
    {
      return _Values[ordinal];
    }

    public object GetValue(int ordinal)
    {
      return _Values[ordinal];
    }

    public int GetValues(object[] values)
    {
      for (int n = 0; n < _Values.Length; n++ )
        values[n] = _Values[n];
      return _Values.Length;
    }

    public bool HasNewFieldList
    {
      get { return _HasNewFieldList; }
    }

    public object Item(int ordinal)
    {
      return _Values[ordinal];
    }

    public object Item(string name)
    {
      return _Values[GetOrdinal(name)];
    }

    public bool Read()
    {
      _Comments = null;
      _HasNewFieldList = false;
      string line;
      do 
      {
        line = StreamReader.ReadLine();
        if (line != null && line.Length != 0 && line[0] == '#') // comment
        {
          if (_Comments == null) _Comments = new List<string>();
          _Comments.Add(line);
          Match fieldsMatch = FieldsRegex.Match(line);
          if (fieldsMatch.Success) // new fields spec
          {
            _HasNewFieldList = true;
            _Fields = fieldsMatch
              .Groups["field"]
              .Captures
              .OfType<Capture>()
              .Select(c => c.Value)
              .ToList();
          }
        }
      } while (line != null && line.Length != 0 && line[0] == '#');

      if (line == null)
        _Values = null;
      else
        _Values = line.Split(' ');

      return line != null;
    }

    #region IDisposable Members

    public void Dispose()
    {
      Close();
      StreamReader.Dispose();
    }

    #endregion
  }
}
