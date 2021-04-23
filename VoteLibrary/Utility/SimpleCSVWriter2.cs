using System.Collections.Generic;
using System.IO;
using static System.String;

namespace Vote
{
  public class SimpleCsvWriter
  {
    private readonly List<string> _Fields = new List<string>();

    public void AddField(string field)
    {
      if (field == null)
        AddNull();
      else
      {
        if (field.Length > 32000)
          field = field.Substring(0, 32000) + "...";
        field = field.Replace("\"", "\"\""); // double up quotes
        _Fields.Add("\"" + field + "\""); // enclose in quotes
      }
    }

    public void AddFields(IEnumerable<string> fields)
    {
      foreach (var field in fields)
        AddField(field);
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
      writer.WriteLine(Join(",", _Fields));
      _Fields.Clear();
    }
  }
}