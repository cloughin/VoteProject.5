using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateDbClasses
{
  class ColumnInfo
  {
    private string _MappedName;
    private string _ParameterName;

    public ColumnInfo(string name, Type type, int size,
      bool isNullable, bool isAutoIncrement)
    {
      Name = name;
      _MappedName = name;
      Type = type;
      Size = size;
      IsNullable = isNullable;
      IsAutoIncrement = isAutoIncrement;
    }

    public static ColumnInfo GetAutoIncrementColumn(
      IEnumerable<ColumnInfo> columnInfos)
    {
      return columnInfos.SingleOrDefault(ci => ci.IsAutoIncrement);
    }

    public static IEnumerable<string> GetMappedNames(IEnumerable<ColumnInfo> columnInfos)
    {
      return columnInfos
        .Where(ci => !ci.Skip)
        .Select(ci => ci.MappedName);
    }

    public static IEnumerable<string> GetNames(IEnumerable<ColumnInfo> columnInfos)
    {
      return columnInfos
        .Where(ci => !ci.Skip)
        .Select(ci => ci.Name);
    }

    public static IEnumerable<string> GetParameterNames(IEnumerable<ColumnInfo> columnInfos)
    {
      return columnInfos
        .Where(ci => !ci.Skip)
        .Select(ci => ci.ParameterName);
    }

    public bool IsAutoIncrement { get; }

    public bool IsNullable { get; }

    public string MappedName
    {
      get { return _MappedName; }
      set 
      { 
        _MappedName = value;
        SetDefaultParameterName();
      }
    }

    public string Name { get; }

    public string ParameterName
    {
      get
      {
        if (_ParameterName == null)
          SetDefaultParameterName();
        return _ParameterName;
      }
    }

    private void SetDefaultParameterName()
    {
      // if MappedName begins with lower, use it
      if (char.IsLower(_MappedName[0]))
      {
        _ParameterName = _MappedName;
        return;
      }

      // if MappedName is all upper, make it all lower
      if (_MappedName == _MappedName.ToUpperInvariant())
      {
        _ParameterName = _MappedName.ToLowerInvariant();
        return;
      }

      // guaranteed to be at least 2 chars with 1st upper
      var sb = new StringBuilder(_MappedName);

      // if MappedName begins Xx... make it xx...
      // or if XXx... make it xXx...
      if (char.IsLower(sb[1]) || sb.Length >= 3 && char.IsLower(sb[2]))
      {
        sb[0] = char.ToLowerInvariant(sb[0]);
        _ParameterName = sb.ToString();
        return;
      }

      // if MappedName begins XXXx... make it xxXx...
      if (sb.Length >= 4 && char.IsUpper(sb[1])
        && char.IsUpper(sb[2]) && char.IsLower(sb[3]))
      {
        sb[0] = char.ToLowerInvariant(sb[0]);
        sb[1] = char.ToLowerInvariant(sb[1]);
        _ParameterName = sb.ToString();
        return;
      }

      // final default ... leave it be
      _ParameterName = _MappedName;
    }

    public int Size { get; }

    public bool Skip { get; set; }

    public Type Type { get; }
  }
}
