using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Vote
{
  public class SimpleCsvWriter
  {
    List<string> _Fields = new List<string>();

    public void AddField(string field)
    {
      if (field == null)
        AddNull();
      else
      {
        field = field.Replace("\"", "\"\""); // double up quotes
        _Fields.Add("\"" + field + "\""); // enclose in quotes
      }
    }

    public void AddNull()
    {
      _Fields.Add("NULL");
    }

    public void Clear()
    {
      _Fields.Clear();
    }

    public void Write(TextWriter writer)
    {
      writer.WriteLine(string.Join(",", _Fields));
      _Fields.Clear();
    }
  }
}